namespace ET.Client
{
    [Actions(ActionType.Damage)]
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_Damage : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Cast cast = actions.CastSelf;

            if (cast == null || actionsRunType != ActionsRunType.CastHit)
                return;

            Unit_Client unit = actions.Owner;
            if (unit == null || unit.IsDisposed)
                return;
            BattleHelper.CalcAttack(cast.OwnerUnit, unit, actions);
        }
    }
}