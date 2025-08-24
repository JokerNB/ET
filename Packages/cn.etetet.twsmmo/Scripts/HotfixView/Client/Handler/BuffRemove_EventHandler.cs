namespace ET.Client
{
    [Event(SceneType.Current)]
    public class BuffRemove_EventHandler : AEvent<Scene, Event_BuffRemove>
    {
        protected override async ETTask Run(Scene scene, Event_BuffRemove a)
        {
            Log.Error($"-> 玩家 {a.OwnerId} 移除了 {a.BuffId} BUFF");
            //buff移除，状态移除在客户端buffcomponent，回收特效、ui等资源
            await ETTask.CompletedTask;
        }
    }
}