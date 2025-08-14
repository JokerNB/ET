using Unity.Mathematics;

namespace ET.Client
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int, UnitType>(unitInfo.UnitId, unitInfo.ConfigId, (UnitType)unitInfo.Type);
            unitComponent.Add(unit);

            NumericDataComponent numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitToServer(unitInfo.KV);

            unit.AddComponent<ObjectWait>();

            EventSystem.Instance.Publish(unit.Scene(), new AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}