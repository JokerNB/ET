namespace ET.Client
{
    [Actions(ActionType.Damage)]
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    public class Actions_Damage : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Cast cast = actions.CastSelf;

            if (cast == null || actionsRunType != ActionsRunType.CastHit)
                return;

            if (cast.Target.Count <= 0)
                return;

            UnitComponent_Client unitComponent = cast.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            foreach (long unitId in cast.Target)
            {
                Unit_Client unit = unitComponent.Get(unitId);
                if(unit == null || unit.IsDisposed)
                    continue;
                BattleHelper.CalcAttack(cast.Caster, unit, actions);
            }
        }
    }
}