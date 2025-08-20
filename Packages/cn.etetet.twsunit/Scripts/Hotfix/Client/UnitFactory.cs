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

        public static Unit CreateMonster(Scene currentScene, int monsterConfigId)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            UnitConfig monsterConfig = UnitConfigCategory.Instance.Get(monsterConfigId);
            UnitType unitType = monsterConfig.UnitType;
            Unit unit = unitComponent.AddChild<Unit, int, UnitType>(monsterConfigId, unitType);
            NumericDataComponent numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitSet(monsterConfig.NumericTypeValue);
            
            EventSystem.Instance.Publish(unit.Scene(), new AfterMonsterCreate() { Unit = unit });

            return unit;
        }
    }
}