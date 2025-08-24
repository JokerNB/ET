namespace ET.Client
{
    [Event(SceneType.Current)]
    public class BuffTimeOut_EventHandler : AEvent<Scene, Event_BuffTimeOut>
    {
        protected override async ETTask Run(Scene scene, Event_BuffTimeOut a)
        {
            await ETTask.CompletedTask;
        }
    }
}