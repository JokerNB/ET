using System;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Clerk:
                {
                    UnitConfig unitConfig = UnitConfigCategory.Instance.Get(1001);
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, unitConfig.Id);

                    NumericDataComponent numericComponent = unit.AddComponent<NumericDataComponent>();
                    numericComponent.InitSet(unitConfig.NumericTypeValue);

                    unitComponent.Add(unit);

                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}