namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SetLoadingValue_Handler : AEvent<Scene, SetLoadingValue>
    {
        protected override async ETTask Run(Scene scene, SetLoadingValue a)
        {
            LoadingUI loadingUI = scene.Root().GetComponent<FUIComponent>().GetPanelLogic<LoadingUI>(true);
            loadingUI.SetValue(a.Value, a.isComplete);
            await ETTask.CompletedTask;
        }
    }
}