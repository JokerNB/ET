namespace ET.Client
{
    [Actions(ActionType.CreateCast)]
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_CreateCast : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            RunAsync(actions, actionsRunType).NoContext();
        }

        public async ETTask RunAsync(Actions actions, ActionsRunType actionsRunType)
        {
            Cast cast = actions.CastSelf;
            if (cast == null || actionsRunType != ActionsRunType.CastFinish)
                return;
            int castConfigId = (int)cast.CastConfig.SelfActionParam[actions.idx];
            Unit_Client unit = cast.OwnerUnit;
            await unit.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            if (unit.IsDisposed)
                return;
            unit.CreateCast(castConfigId);
            await ETTask.CompletedTask;
        }
    }
}

