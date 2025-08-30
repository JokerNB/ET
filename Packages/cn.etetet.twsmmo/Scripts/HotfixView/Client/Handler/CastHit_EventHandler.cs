namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastHit_EventHandler : AEvent<Scene, Event_CastHit>
    {
        protected override async ETTask Run(Scene scene, Event_CastHit a)
        {
            //技能命中，一般播放命中特效之类的，或者和前摇技能配合校正技能位置等
            // Log.Error($"-> 玩家 {a.casterId} 的技能 {a.castId} 命中了 {a.TargetId}");
            await ETTask.CompletedTask;
        }
    }
}