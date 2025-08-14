using System;
using Unity.Mathematics;

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
                    Unit unit = unitComponent.AddChildWithId<Unit, int, UnitType>(id, 1001, unitType);

                    NumericDataComponent numericComponent = unit.AddComponent<NumericDataComponent>();
                    numericComponent.Set(ENumericType.Speed1, 6f); // 速度是6米每秒
                    numericComponent.Set(ENumericType.AOI1, 15); // 视野15米

                    unitComponent.Add(unit);

                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}