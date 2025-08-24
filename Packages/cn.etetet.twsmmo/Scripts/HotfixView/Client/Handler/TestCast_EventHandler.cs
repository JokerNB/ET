namespace ET.Client
{
    [Event(SceneType.Current)]
    public class TestCast_EventHandler : AEvent<Scene, Event_TestCast>
    {
        protected override async ETTask Run(Scene scene, Event_TestCast a)
        {
            Unit_Client unitPlayer = scene.GetComponent<UnitComponent_Client>().Unit_Player;
            if (unitPlayer == null)
            {
                Log.Error("测试释放技能 Unit player is null");
                return;
            }

            if (!unitPlayer.IsAlive())
            {
                Log.Error("测试释放技能 Unit player is not Alive");
                return;
            }

            Log.Error($"测试释放技能 {a.castConfigId}");
            int errCode = unitPlayer.CreateAndCast(a.castConfigId);
            if (errCode != ErrorCode.ERR_Success)
                Log.Error($"释放技能失败：{errCode}");
            await ETTask.CompletedTask;
        }
    }
}