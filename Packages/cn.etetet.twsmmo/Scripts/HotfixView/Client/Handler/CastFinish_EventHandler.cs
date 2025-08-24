namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastFinish_EventHandler : AEvent<Scene, Event_CastFinish>
    {
        protected override async ETTask Run(Scene scene, Event_CastFinish a)
        {
            //技能结束，播放技能后摇，回到Idle状态，回收技能特效、模型、UI等资源
            Log.Error($"-> 挖煤靠{a.casterId} 的技能 {a.castId} 结束了");
            await ETTask.CompletedTask;
        }
    }
}