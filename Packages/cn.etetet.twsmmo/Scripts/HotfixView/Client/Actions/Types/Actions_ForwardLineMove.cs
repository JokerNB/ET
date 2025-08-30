namespace ET.Client
{
    [Actions(ActionType.ForwardLineMove)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_ForwardLineMove : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client unitClient = actions.Caster;
            if(unitClient == null || unitClient.IsDisposed)
                return;
        }
    }
}

