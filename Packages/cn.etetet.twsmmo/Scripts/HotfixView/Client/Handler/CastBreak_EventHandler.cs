namespace ET.Client
{
    [Event(SceneType.Current)]
    public class CastBreak_EventHandler : AEvent<Scene,Event_CastBreak>
    {
        protected override async ETTask Run(Scene scene, Event_CastBreak a)
        {
            Log.Error($"-> 挖煤靠{a.casterId} 的技能 {a.castId} 被打断了");
            //技能被打断，回到Idle状态，回收技能特效，模型，UI等资源
            Unit_Client caster = scene.GetComponent<UnitComponent_Client>().Get(a.casterId);
            if(caster == null)
                return;
            Cast cast = caster.GetComponent<CastComponent>().Get(a.castId);
            if(cast == null)
                return;
            //TODO:播放技能打断动画
            await ETTask.CompletedTask;
        }
    }
}
