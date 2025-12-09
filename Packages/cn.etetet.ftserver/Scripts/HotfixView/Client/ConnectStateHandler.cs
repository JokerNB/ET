namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class ConnectStateHandler : AEvent<Scene, ConnectState>
    {
        protected override async ETTask Run(Scene scene, ConnectState a)
        {
            scene.GetComponent<UIComponent>().Get(UIType.UILogin).GetComponent<UILoginComponent>().SetText(a.state);
            await ETTask.CompletedTask;
        }
    }
}
