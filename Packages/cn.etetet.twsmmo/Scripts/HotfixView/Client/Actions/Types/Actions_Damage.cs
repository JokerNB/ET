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

            Unit_Client target = actions.Owner;
            if (target == null || target.IsDisposed)
                return;

            Unit_Client attacker = actions.Caster;
            if (attacker == null || attacker.IsDisposed)
                return;
            
            int damage = -attacker.NumericComponent.GetAsInt(ENumericType.NormalDamage0);
            var numericType = actions.Config.NumericType;
            int oldHp = target.NumericComponent.GetAsInt(numericType);
            //扣血逻辑
            target.NumericComponent.Change(numericType, damage);
            int newHp = target.NumericComponent.GetAsInt(numericType);
            Log.Error($"CalcAttack {damage} , oldHp: {oldHp}, newHp: {newHp}");

            if (damage != 0)
            {
                //TODO:伤害飘字
            }

            if (oldHp > 0 && newHp == 0)
            {
                //处理死亡逻辑
                target.GetParent<UnitComponent_Client>().Remove(target.Id);
            }
        }
    }
}