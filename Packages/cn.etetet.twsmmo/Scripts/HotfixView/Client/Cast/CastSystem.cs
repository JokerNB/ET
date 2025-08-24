using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(Cast))]
    [FriendOfAttribute(typeof(ET.Client.MonsterGameObjectComponent))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Cast self, int args2)
        {
            self.ConfigId = args2;
            self.AddComponent<ActionsTempComponent>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Cast self)
        {
            self.ConfigId = default;
            self.Caster = default;
            self.Target.Clear();
            self.StartTime = default;
        }

        /// <summary>
        /// 释放Cast
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Cast(this ET.Client.Cast self)
        {
            int err = self.CastCheck();
            if (err != ErrorCode.ERR_Success)
                return err;

            //选择目标
            self.SelectTarget();

            err = self.CastCheckBeforeBegin();
            if (err != ErrorCode.ERR_Success)
                return err;

            self.CastBeginAsync().NoContext();
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
            Entity unit = self.Caster;
            if (unit == null || unit.IsDisposed)
                return ErrorCode.ERR_Cast_CasterIsNull;
            return ErrorCode.ERR_Success;
        }

        public static void SelectTarget(this ET.Client.Cast self)
        {
            Unit_Client caster = self.Caster;
            CastConfig config = self.CastConfig;

            var allUnits = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().GetAllUnits();
            //case多的时候可以做逻辑分发
            int rang = 0;
            switch (config.SelectType)
            {
                case 1: //选择身边一定范围内的一个人
                    rang = int.Parse(config.SelectParam[0]);
                    foreach (Unit_Client unit in allUnits)
                    {
                        if (unit == caster)
                        {
                            //不选择自己
                            continue;
                        }

                        if (math.length(unit.GetUnitPosition() - caster.GetUnitPosition()) < rang)
                        {
                            //选择找到的第一个
                            self.Target.Add(unit.Id);
                            break;
                        }
                    }

                    break;
                case 2: //选择身边一定范围内的所有人
                    rang = int.Parse(config.SelectParam[0]);
                    foreach (Unit_Client unit in allUnits)
                    {
                        if (math.length(unit.GetUnitPosition() - caster.GetUnitPosition()) < rang)
                        {
                            self.Target.Add(unit.Id);
                        }
                    }

                    break;
            }
        }

        public static int CastCheckBeforeBegin(this ET.Client.Cast self)
        {
            switch (self.CastConfig.SelectType)
            {
                case 1:
                case 2:
                    if (self.Target.Count <= 0)
                    {
                        return ErrorCode.ERR_Cast_TargetIsNull;
                    }

                    break;
            }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask CastBeginAsync(this ET.Client.Cast self)
        {
            self.StartTime = TimeInfo.Instance.ServerNow();
            //通知其他unit开始释放技能
            //单机不需要
            Unit_Client caster = self.Caster;
            CastConfig castConfig = self.CastConfig;
            EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_CastStart
            {
                castId = self.Id,
                casterId = caster.Id,
                castConfigId = self.ConfigId,
                TargetsId = self.Target
            });
            if (castConfig.Times.Count <= 0)
                return;
            //技能实体
            long castInstanceId = 0;
            //技能释放实体
            long casterInstanceId = 0;

            foreach (int time in castConfig.Times)
            {
                castInstanceId = self.InstanceId;
                casterInstanceId = caster.InstanceId;
                await self.Root().GetComponent<TimerComponent>().WaitTillAsync(self.StartTime + time);
                if (!self.CheckAsyncInvalid(castInstanceId, casterInstanceId))
                {
                    Log.Error($"Cast AsyncInvalid {castInstanceId} {casterInstanceId}");
                    return;
                }

                //TODO:创建出一系列技能行为
                foreach (CastActionTime castActionTime in castConfig.TimesDic[time])
                {
                    if (castActionTime.isSelfHit)
                    {
                        self.HandleSelfHit(castActionTime.Index);
                    }
                    else
                    {
                        self.HandleTargetHit(castActionTime.Index);
                    }
                }
            }

            if (castConfig.TotalTime > 0)
            {
                castInstanceId = self.InstanceId;
                casterInstanceId = caster.InstanceId;

                await self.Root().GetComponent<TimerComponent>().WaitTillAsync(self.StartTime + castConfig.TotalTime);

                if (!self.CheckAsyncInvalid(castInstanceId, casterInstanceId))
                {
                    Log.Error($"Cast AsyncInvalid {castInstanceId} {casterInstanceId}");
                    return;
                }
            }

            self.CastFinish();
        }

        public static void HandleSelfHit(this Cast self, int index)
        {
            CastConfig castConfig = self.CastConfig;
            self.SelectTarget();
            if(self.Target.Count<=0)
                return;
            if (castConfig.SelfHitAction.Count > index)
            {
                int actionId = castConfig.SelfHitAction[index];
                self.CreateActions(actionId, self.Caster, ActionsRunType.CastHit);
            }
        }

        public static void HandleTargetHit(this Cast self, int index)
        {
            CastConfig castConfig = self.CastConfig;
            self.SelectTarget();
            if(self.Target.Count<=0)
                return;
            //技能命中消息
            //单机不需要
            Unit_Client caster = self.Caster;
            EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_CastHit
            {
                castId = self.Id,
                casterId = caster.Id,
                TargetsId = self.Target
            });
            
            UnitComponent_Client unitComponent = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            foreach (long unitId in self.Target)
            {
                Unit_Client unit = unitComponent.Get(unitId);
                if(unit == null || unit.IsDisposed)
                    continue;
                if (castConfig.HitAction.Count > index)
                {
                    int actionId = castConfig.HitAction[index];
                    self.CreateActions(actionId,unit, ActionsRunType.CastHit);
                }
            }
            
        }

        public static void CastFinish(this ET.Client.Cast self)
        {
            //self.CastConfig.TotalTime<=0
            //没有持续时间，就是瞬发的技能流程，可以不用通知结束，客户端自行结束
            if (self.CastConfig.TotalTime > 0)
            {
                Unit_Client caster = self.Caster;
                EventSystem.Instance.Publish(self.Root().CurrentScene(),new Event_CastFinish
                {
                    castId = self.Id,
                    casterId = caster.Id
                });
            }

            self?.Dispose();
        }

        public static bool CheckAsyncInvalid(this Cast self, long castInstanceId, long casterInstanceId)
        {
            Unit_Client caster = self.Caster;
            if (caster == null)
                return false;
            if (self.InstanceId != castInstanceId || caster.InstanceId != casterInstanceId)
                return false;
            return true;
        }
    }
}