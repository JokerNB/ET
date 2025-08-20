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
                case UnitType.Player:
                {
                    UnitConfig unitConfig = UnitConfigCategory.Instance.GetFirstUnitConfig();
                    Unit unit = unitComponent.AddChildWithId<Unit, int, UnitType>(id, unitConfig.Id, unitConfig.UnitType);

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