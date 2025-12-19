namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class AppStartInitFinishHandler : AEvent<Scene,AppStartInitFinish>
    {
        protected override async ETTask Run(Scene scene, AppStartInitFinish a)
        {
            await scene.GetComponent<FUIComponent>().ShowPanelAsync<UIMainPanel>();
            await ETTask.CompletedTask;
        }
    }
}