namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastHit_EventHandler : AEvent<Scene, Event_CastHit>
    {
        protected override async ETTask Run(Scene scene, Event_CastHit a)
        {
            //技能命中，一般播放命中特效之类的，或者和前摇技能配合校正技能位置等
            Log.Error($"-> 玩家 {a.casterId} 的技能 {a.castId} 命中了 {a.TargetId}");
            Unit_Client unit = scene.GetComponent<UnitComponent_Client>().Get(a.TargetId);
            if(unit == null || unit.IsDisposed)
                return;
            if (unit.IsMonster())
            {
                unit.GetComponent<MonsterMoveComponent>().DOFlash().NoContext();
            }
            else if (unit.UnitType == UnitType.Player)
            {
                unit.GetComponent<PlayerMoveComponent>().DoFlash().NoContext();
            }
            await ETTask.CompletedTask;
        }
    }
}