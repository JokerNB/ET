namespace ET.Client
{
    [Event(SceneType.Current)]
    public class BuffTick_EventHandler : AEvent<Scene, Event_BuffTick>
    {
        protected override async ETTask Run(Scene scene, Event_BuffTick a)
        {
            Log.Error($"-> 玩家 {a.OwnerId} 的BUFF {a.BuffId} 触发Tick");
            //buffTick，播放特效、伤害等等，例如流血动画，飘字子类的
            await ETTask.CompletedTask;
        }
    }
}