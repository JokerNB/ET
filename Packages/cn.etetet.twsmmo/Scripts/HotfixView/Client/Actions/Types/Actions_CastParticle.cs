namespace ET.Client
{
    [Actions(ActionType.CastParticle)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_CastParticle : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client target = actions.Owner;
            if (target == null || target.IsDisposed)
                return;
            Buff buff = actions.BuffSelf;
            if (actionsRunType == ActionsRunType.BuffAdd)
                UnitFactory.CreateParticleUnit(target.Root().CurrentScene(), (int)buff.Config.AddActionParam[actions.idx]);
            else if (actionsRunType == ActionsRunType.BuffTick)
                UnitFactory.CreateParticleUnit(target.Root().CurrentScene(), (int)buff.Config.TickActionParam[actions.idx]);
            else if (actionsRunType == ActionsRunType.BuffRemove)
                UnitFactory.CreateParticleUnit(target.Root().CurrentScene(), (int)buff.Config.RemoveActionParam[actions.idx]);
        }
    }
}