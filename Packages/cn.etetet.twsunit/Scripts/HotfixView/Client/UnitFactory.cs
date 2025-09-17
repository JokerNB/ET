namespace ET.Client
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            NumericDataComponent numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitToServer(unitInfo.KV);

            unit.AddComponent<ObjectWait>();

            if (unit.UnitType != UnitType.Player)
                EventSystem.Instance.Publish(currentScene, new AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}