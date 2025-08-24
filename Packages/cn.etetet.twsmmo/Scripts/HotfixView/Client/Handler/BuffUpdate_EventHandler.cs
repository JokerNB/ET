namespace ET.Client
{
    [Event(SceneType.Current)]
    public class BuffUpdate_EventHandler : AEvent<Scene, Event_BuffUpdate>
    {
        protected override async ETTask Run(Scene scene, Event_BuffUpdate a)
        {
            Log.Error($"-> 玩家 {a.ownerId} 更新了 {a.buffConfigId} BUFF {a.buffId}");
            //buff上信息更新，各自根据更新的逻辑进行处理
            await ETTask.CompletedTask;
        }
    }
}