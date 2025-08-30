namespace ET.Client
{
    [Actions(ActionType.CastBuff)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class ActionsCastBuff : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client owner = actions.Owner;
            if(owner == null || owner.IsDisposed)
                return;
            Cast cast = actions.CastSelf;
            if (actionsRunType == ActionsRunType.CastStart)
                owner.GetComponent<BuffComponent>().CreateAndAdd((int)cast.CastConfig.SelfActionParam[0]);
            if (actionsRunType == ActionsRunType.CastHit)
                owner.GetComponent<BuffComponent>().CreateAndAdd((int)cast.CastConfig.HitActionParam[0]);
        }
    }
}

