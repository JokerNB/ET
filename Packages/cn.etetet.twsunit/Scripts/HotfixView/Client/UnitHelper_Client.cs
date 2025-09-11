using UnityEngine;

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
    }
}