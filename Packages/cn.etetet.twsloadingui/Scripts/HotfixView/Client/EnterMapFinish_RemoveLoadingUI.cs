namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class EnterMapFinish_RemoveLoadingUI : AEvent<Scene, EnterMapFinish>
    {
        protected override async ETTask Run(Scene scene, EnterMapFinish a)
        {
            await scene.YIUIMgr().ClosePanelAsync<ETTWSLoginUIPanelComponent>();
        }
    }
}