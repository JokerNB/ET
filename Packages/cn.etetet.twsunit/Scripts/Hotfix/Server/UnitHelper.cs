namespace ET.Server
{
    [FriendOfAttribute(typeof(ET.Server.PlayerComponent))]
    public static partial class UnitHelper
    {
        public static Unit GetMyUnitFromClientScene(Scene root)
        {
            return null;
            // PlayerComponent playerComponent = root.GetComponent<PlayerComponent>();
            // Scene currentScene = root.GetComponent<CurrentScenesComponent>().Scene;
            // return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            return null;
            // PlayerComponent playerComponent = currentScene.Root().GetComponent<PlayerComponent>();
            // return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }
    }
}