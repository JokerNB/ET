using DG.Tweening;
using UnityEngine;

namespace ET.Client
{
    [Invoke(TimerInvokeType.RemoveEnergyBlockTimer)]
    public class RemoveEnergyBlockTimer_Handler : ATimer<EnergyBlockComponent>
    {
        protected override void Run(EnergyBlockComponent t)
        {
            t?.RemoveSelf();
        }
    }
    
    [EntitySystemOf(typeof(EnergyBlockComponent))]
    public static partial class EnergyBlockComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.EnergyBlockComponent self)
        {
            self.RegisterTimer();
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.EnergyBlockComponent self)
        {
            self.UnRegisterTimer();
        }
        
        public static void RegisterTimer (this ET.Client.EnergyBlockComponent self)
        {
            long dur = self.GetParent<Unit_Client>().NumericComponent.GetAsLong(ENumericType.BattleDuration0);
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.ServerNow() + dur, TimerInvokeType.RemoveEnergyBlockTimer, self);
        }

        public static void UnRegisterTimer(this ET.Client.EnergyBlockComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void Collect(this EnergyBlockComponent self)
        {
            Unit_Client unit = self.GetParent<Unit_Client>();
            if (unit == null || unit.IsDisposed)
                return;

            Unit_Client unitPlayer = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Unit_Player;
            if (unitPlayer == null || unitPlayer.IsDisposed)
                return;

            Transform parent = unitPlayer.GetComponent<GameObjectComponent>().Transform;
            Transform transform = unit.GetComponent<GameObjectComponent>().Transform;
            var pos = unitPlayer.GetComponent<GameObjectComponent>().sprite.transform.localPosition;
            transform.SetParent(parent);
            float time = unit.NumericComponent.GetAsFloat(ENumericType.Speed0);
            transform.DOLocalMove(pos, time).OnComplete(() =>
            {
                Unit_Client unit = self.GetParent<Unit_Client>();
                if (unit == null || unit.IsDisposed)
                    return;
                //添加属性
                var battleExp = unit.NumericComponent.GetAsInt(ENumericType.BattleExp0);
                unitPlayer.NumericComponent.Change(ENumericType.BattleExp0, battleExp);
                self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(unit.Id);
            });
        }

        public static void RemoveSelf(this ET.Client.EnergyBlockComponent self)
        {
            Unit_Client unit = self.GetParent<Unit_Client>();
            if (unit == null || unit.IsDisposed)
                return;
            self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(unit.Id);
        }
    }
}