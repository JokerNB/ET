using UnityEngine;

namespace ET.Client
{
    [Invoke(TimerInvokeType.CastRepeatedTick)]
    public class CastRepeatedTick_Handler : ATimer<Cast>
    {
        protected override void Run(Cast t)
        {
            t?.CreateCastUnit().NoContext();
        }
    }

    [NumericHandlerDynamic(SceneType.Current, ENumericType.SkillTickInterval0, 0)]
    public class CastNumericChangeEventHandler_SkillTickInterval0 : NumericHandlerDynamicSystem<Cast, Cast, NumericChange>
    {
        protected override async ETTask Run(Cast self, Cast entity, NumericChange data)
        {
            self.UnRegisterTimer();
            self.RegisterTimer();
            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Cast self, int configId, Unit_Client ownerUnit)
        {
            self.ConfigId = configId;
            self.OwnerUnit = ownerUnit;
            self.AddComponent<ActionsTempComponent>();
            var numericDataComponent = self.AddComponent<NumericDataComponent>();
            numericDataComponent.InitSet(self.CastConfig.NumericTypeValue);
            self.CreateCastUnit().NoContext();
            self.RegisterTimer();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Cast self)
        {
            self.ConfigId = default;
            self.OwnerUnit = default;
            self.UnRegisterTimer();
        }

        public static void RegisterTimer(this ET.Client.Cast self)
        {
            long interval = self.GetComponent<NumericDataComponent>().GetAsLong(ENumericType.SkillTickInterval0);
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(interval, TimerInvokeType.CastRepeatedTick, self);
        }

        public static void UnRegisterTimer(this ET.Client.Cast self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static async ETTask CreateCastUnit(this ET.Client.Cast self)
        {
            if (Time.timeScale <= 0)
                return;
            int childNum = self.GetComponent<NumericDataComponent>().GetAsInt(ENumericType.SkillNum0);
            if (childNum == 0)
                return;
            UnitComponent_Client unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            for (int i = 0; i < childNum; i++)
            {
                Unit_Client skillUnit = unitComponentClient.AddChild<Unit_Client, int>(UnitConfigCategory.Instance.castUnitConfig.Id);
                unitComponentClient.Add(skillUnit);
                await EventSystem.Instance.PublishAsync(self.Root().CurrentScene(), new AfterSkillCreate
                {
                    OwnerUnit = self.OwnerUnit,
                    SkillUnit = skillUnit,
                    castConfigId = self.ConfigId,
                    castSelf = self
                });
                
                self.Cast(skillUnit);
                
                await self.Root().GetComponent<TimerComponent>().WaitAsync(100);
            }
        }

        /// <summary>
        /// 释放Cast
        /// </summary>
        public static int Cast(this ET.Client.Cast self, Unit_Client skillUnit)
        {
            int err = self.CastCheck();
            if (err != ErrorCode.ERR_Success)
            {
                self.Dispose();
                return err;
            }

            self.CastBeginAsync(skillUnit).NoContext();
            return ErrorCode.ERR_Success;
        }

        /// <summary>
        /// Cast释放的前置判断
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int CastCheck(this ET.Client.Cast self)
        {
            if (self == null || self.IsDisposed)
                return ErrorCode.ERR_Cast_ArgsError;
            Unit_Client unit = self.OwnerUnit;
            if (unit == null || unit.IsDisposed)
                return ErrorCode.ERR_Cast_CasterIsNull;
            return ErrorCode.ERR_Success;
        }

        public static async ETTask CastBeginAsync(this ET.Client.Cast self, Unit_Client skillUnit)
        {
            Unit_Client caster = self.OwnerUnit;

            CastConfig castConfig = self.CastConfig;
            EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_CastStart
            {
                castId = self.Id,
                casterId = caster.Id,
                castConfigId = self.ConfigId,
            });

            int idx = 0;
            foreach (int actionId in castConfig.SelfAction)
            {
                self.CreateActions(actionId, idx, self.OwnerUnit, skillUnit, ActionsRunType.CastStart);
                idx++;
            }

            //一次性技能
            if (castConfig.CastType == CastType.OnceCast)
            {
                await self.CastHit(caster.Id, skillUnit);
                await self.CastFinish();
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask CastHit(this ET.Client.Cast self, long targetUnitId, Unit_Client skillUnit)
        {
            Unit_Client caster = self.OwnerUnit;
            var castConfig = self.CastConfig;

            await EventSystem.Instance.PublishAsync(self.Root().CurrentScene(), new Event_CastHit
            {
                castId = self.Id,
                casterId = caster.Id,
                TargetId = targetUnitId
            });

            var unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            int idx = 0;
            foreach (int actionId in castConfig.HitAction)
            {
                Unit_Client hitOwnerUnit = unitComponentClient.Get(targetUnitId);
                self.CreateActions(actionId, idx, hitOwnerUnit, skillUnit, ActionsRunType.CastHit);
                idx++;
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask CastFinish(this ET.Client.Cast self)
        {
            self.GetParent<CastComponent>().Remove(self.ConfigId);
            await ETTask.CompletedTask;
        }

        public static bool CheckAsyncInvalid(this Cast self, long castInstanceId, long casterInstanceId)
        {
            Unit_Client caster = self.OwnerUnit;
            if (caster == null)
                return false;
            if (self.InstanceId != castInstanceId || caster.InstanceId != casterInstanceId)
                return false;
            return true;
        }
    }
}