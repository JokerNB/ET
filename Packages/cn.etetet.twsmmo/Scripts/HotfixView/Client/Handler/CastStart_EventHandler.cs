namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastStart_EventHandler : AEvent<Scene, Event_CastStart>
    {
        protected override async ETTask Run(Scene scene, Event_CastStart a)
        {
            //技能释放流程的开始，此处可以自行接入行为树或状态机之类的
            //开始播放技能前摇，播放技能特效、音效等等
            Log.Error($"-> 玩家 {a.casterId} 开始释放 {a.castConfigId} 技能 {a.castId}");
            await ETTask.CompletedTask;
        }
    }
}