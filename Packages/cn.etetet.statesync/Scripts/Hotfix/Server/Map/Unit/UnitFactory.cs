using System;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit CreatePlayerUnit(Scene scene, long id)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            UnitConfig unitConfig = UnitConfigCategory.Instance.Get(1);
            Unit unit = unitComponent.AddChildWithId<Unit, int>(id, unitConfig.Id);
            unitComponent.Add(unit);
            return unit;
        }
    }
}