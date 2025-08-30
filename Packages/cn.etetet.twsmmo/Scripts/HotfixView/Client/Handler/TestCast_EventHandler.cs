namespace ET.Client
{
    [Event(SceneType.Current)]
    public class TestCast_EventHandler : AEvent<Scene, Event_TestCast>
    {
        protected override async ETTask Run(Scene scene, Event_TestCast a)
        {
            Unit_Client unitPlayer = scene.GetComponent<UnitComponent_Client>().Unit_Player;
            if(unitPlayer == null || unitPlayer.IsDisposed)
                return;

            Log.Error($"测试释放技能 {a.castConfigId}");
            unitPlayer.CreateCast(a.castConfigId);
            await ETTask.CompletedTask;
        }
    }
}