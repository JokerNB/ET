using UnityEngine;

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

        public static Unit_Client CreateMonster(Scene currentScene, int monsterConfigId)
        {
            UnitComponent_Client unitComponent = currentScene.GetComponent<UnitComponent_Client>();
            UnitConfig monsterConfig = UnitConfigCategory.Instance.Get(monsterConfigId);
            Unit_Client unit = unitComponent.AddChild<Unit_Client, int>(monsterConfigId);
            unitComponent.Add(unit);
            NumericDataComponent numericDataComponent = unit.AddComponent<NumericDataComponent>();
            numericDataComponent.InitSet(monsterConfig.NumericTypeValue);
            EventSystem.Instance.Publish(currentScene, new AfterMonsterCreate() { Unit = unit });

            return unit;
        }

        public static Unit_Client CreateParticleUnit(Scene currentScene, int unitConfigId)
        {
            UnitComponent_Client unitComponent = currentScene.GetComponent<UnitComponent_Client>();
            Unit_Client unit = unitComponent.AddChild<Unit_Client, int>(unitConfigId);
            unitComponent.Add(unit);
            unit.AddComponent<ObjectWait>();
            unit.AddComponent<NumericDataComponent>();
            unit.NumericComponent.InitSet(unit.Config.NumericTypeValue);
            EventSystem.Instance.Publish(currentScene, new AfterParticleCreate());

            return unit;
        }
    }
}