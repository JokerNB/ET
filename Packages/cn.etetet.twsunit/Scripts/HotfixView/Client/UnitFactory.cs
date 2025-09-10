
namespace ET.Client
{
    public static partial class UnitFactory
    {
        public static Unit_Client Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent_Client unitComponent = currentScene.GetComponent<UnitComponent_Client>();
            Unit_Client unit = unitComponent.AddChildWithId<Unit_Client, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            NumericDataComponent numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitToServer(unitInfo.KV);

            unit.AddComponent<ObjectWait>();

            EventSystem.Instance.Publish(currentScene, new AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}