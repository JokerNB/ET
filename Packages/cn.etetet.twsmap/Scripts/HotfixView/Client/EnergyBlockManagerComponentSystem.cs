using UnityEngine;

namespace ET.Client
{
    [Invoke(TimerInvokeType.CreateEnergyBlock_S)]
    public class CreateEnergyBlock_S_Handler : ATimer<EnergyBlockManagerComponent>
    {
        protected override void Run(EnergyBlockManagerComponent t)
        {
            t?.CreateEnergyBlock(1);
        }
    }

    [Invoke(TimerInvokeType.CreateEnergyBlock_M)]
    public class CreateEnergyBlock_M_Handler : ATimer<EnergyBlockManagerComponent>
    {
        protected override void Run(EnergyBlockManagerComponent t)
        {
            t?.CreateEnergyBlock(2);
        }
    }

    [Invoke(TimerInvokeType.CreateEnergyBlock_L)]
    public class CreateEnergyBlock_L_Handler : ATimer<EnergyBlockManagerComponent>
    {
        protected override void Run(EnergyBlockManagerComponent t)
        {
            t?.CreateEnergyBlock(3);
        }
    }

    [EntitySystemOf(typeof(EnergyBlockManagerComponent))]
    public static partial class EnergyBlockManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.EnergyBlockManagerComponent self)
        {
            self.Init();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.EnergyBlockManagerComponent self)
        {
            self.ClearTimer();
        }

        public static void Init(this ET.Client.EnergyBlockManagerComponent self)
        {
            self.Rate_S_Timer = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(self.createInterval_S, TimerInvokeType.CreateEnergyBlock_S, self);
            self.Rate_M_Timer = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(self.createInterval_M, TimerInvokeType.CreateEnergyBlock_M, self);
            self.Rate_L_Timer = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(self.createInterval_L, TimerInvokeType.CreateEnergyBlock_L, self);
        }

        public static void ClearTimer(this ET.Client.EnergyBlockManagerComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Rate_S_Timer);
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Rate_M_Timer);
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Rate_L_Timer);
        }

        public static async ETTask CreateEnergyBlock(this ET.Client.EnergyBlockManagerComponent self, int configId)
        {
            var config = EnergyBlockConfigCategory.Instance.Get(configId);
            string resName = config.ResName;
            GameObject go = await YIUIGameObjectPool.Inst.Get(resName, self.GetParent<MapManagerComponent>().EnergyBlockRootTr);

            var unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            Unit_Client unit = unitComponentClient.AddChild<Unit_Client, int>(UnitConfigCategory.Instance.energyBlockUnitConfig.Id);
            unitComponentClient.Add(unit);
            var gameObjectComponent = unit.AddComponent<GameObjectComponent, GameObject, bool>(go, true, true);

            //设置位置
            Unit_Client unitPlayer = unitComponentClient.Unit_Player;
            Vector2 pos = unitPlayer.GetSelfPosition();
            pos.x += Random.Range(-10f, 10f);
            pos.y += Random.Range(-10f, 10f);
            gameObjectComponent.SetPosition(pos);

            gameObjectComponent.unityEventTrigger.BelongToUnitId = unit.Id;
            gameObjectComponent.unityEventTrigger.unitType = (int)UnitType.EnergyBlock;

            var numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitSet(config.NumericDataType);

            unit.AddComponent<EnergyBlockComponent>();
        }
    }
}