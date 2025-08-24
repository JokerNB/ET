using Unity.Mathematics;

namespace ET.Client
{
    public static class UnitHelper_Client
    {
        public static Unit_Client GetMyUnitFromClientScene(Scene root)
        {
            PlayerComponent playerComponent = root.GetComponent<PlayerComponent>();
            Scene currentScene = root.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent_Client>().Get(playerComponent.MyId);
        }

        public static Unit_Client GetMyUnitFromCurrentScene(Scene currentScene)
        {
            PlayerComponent playerComponent = currentScene.Root().GetComponent<PlayerComponent>();
            return currentScene.GetComponent<UnitComponent_Client>().Get(playerComponent.MyId);
        }

        public static float3 GetUnitPosition(this Unit_Client unit)
        {
            if (unit == null || unit.IsDisposed)
                return float3.zero;
            if (unit.UnitType == UnitType.Player)
                return unit.GetComponent<GameObjectComponent>().Transform.position;
            else if (unit.IsMonster())
                return unit.GetComponent<MonsterGameObjectComponent>().goTr.position;
            else if (unit.UnitType == UnitType.Bullet)
                return unit.GetComponent<BulletGameObjectComponent>().Transform.position;
            return float3.zero;
        }

        public static void SetUnitPosition(this Unit_Client unit, float3 pos)
        {
            if (unit == null || unit.IsDisposed)
                return;
            if (unit.UnitType is UnitType.Player or UnitType.Bullet)
                unit.GetComponent<GameObjectComponent>().Transform.position = pos;
            else if (unit.IsMonster())
                unit.GetComponent<MonsterGameObjectComponent>().SetPos(pos);
            else if (unit.UnitType == UnitType.Bullet)
                unit.GetComponent<BulletGameObjectComponent>().SetPosition(pos);
        }
    }
}