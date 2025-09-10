namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class AfterCreateClientScene_AddComponent: AEvent<Scene, AfterCreateClientScene>
    {
        protected override async ETTask Run(Scene scene, AfterCreateClientScene args)
        {
            scene.AddComponent<ResourcesLoaderComponent>();
            scene.AddComponent<FUIComponent>();
            await ETTask.CompletedTask;
        }
    }
}