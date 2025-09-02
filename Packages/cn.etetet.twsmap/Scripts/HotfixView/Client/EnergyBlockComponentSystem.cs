using DG.Tweening;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(EnergyBlockComponent))]
    public static partial class EnergyBlockComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.EnergyBlockComponent self)
        {
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
                self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(unit.Id);
            });
        }
    }
}