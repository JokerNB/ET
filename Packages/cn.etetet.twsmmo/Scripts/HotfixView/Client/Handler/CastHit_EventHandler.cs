namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastHit_EventHandler : AEvent<Scene, Event_CastHit>
    {
        protected override async ETTask Run(Scene scene, Event_CastHit a)
        {
            //技能命中，一般播放命中特效之类的，或者和前摇技能配合校正技能位置等
            Log.Error($"-> 玩家 {a.casterId} 的技能 {a.castId} 命中了 {a.TargetsId.ListToString()}");
            Unit_Client caster = scene.GetComponent<UnitComponent_Client>().Get(a.casterId);
            if (caster == null)
                return;
            Cast cast = caster.GetComponent<CastComponent>().Get(a.castId);
            if (cast == null)
                return;
            foreach (long targetId in a.TargetsId)
            {
                Unit_Client unitClient = scene.GetComponent<UnitComponent_Client>().Get(targetId);
                if (unitClient == null)
                    continue;
                CastConfig castConfig = cast.CastConfig;
                //TODO:播放命中动画
                foreach (int effectId in castConfig.HitEffect)
                {
                    ParticleEffectHelper.CreateParticle(unitClient, effectId).NoContext();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}