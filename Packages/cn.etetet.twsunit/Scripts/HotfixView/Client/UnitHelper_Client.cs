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
            return unit.GetComponent<GameObjectComponent>().Transform.position; 
        }

        public static void SetUnitPosition(this Unit_Client unit, float3 pos)
        {
            if (unit == null || unit.IsDisposed)
                return;
            unit.GetComponent<GameObjectComponent>().SetPosition(pos);
        }
    }
}