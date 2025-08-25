namespace ET.Client
{
    public static class BattleHelper
    {
        /// <summary>
        /// 战斗结算
        /// 此处必须是同步的，不可改为异步，否则需求复杂之后，很可能出现问题
        /// 可以用协程，但不可把整个战斗计算改成异步
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="actions"></param>
        public static void CalcAttack(Unit_Client attacker, Unit_Client target, Actions actions)
        {
            //这里应该是根据各个项目实际情况，攻击力，防御力之类的一堆公式计算，得出一个伤害值
            //现在简化为直接读取固定伤害
            int damage = actions.Config.Param[0];
            int oldHp = target.NumericComponent.GetAsInt(ENumericType.Hp0);
            //扣血逻辑
            target.NumericComponent.Change(ENumericType.Hp0, damage);
            int newHp = target.NumericComponent.GetAsInt(ENumericType.Hp0);
            Log.Error($"CalcAttack {damage} , oldHp: {oldHp}, newHp: {newHp}");
            int res_damage = newHp - oldHp;
            if (res_damage != 0)
            {
                //TODO:伤害飘字
            }

            if (oldHp > 0 && newHp == 0)
            {
                //处理死亡逻辑
                Kill(attacker, target);
            }
        }

        /// <summary>
        /// 击杀
        /// 负责击杀双方相关的逻辑
        /// </summary>
        /// <param name="killer"></param>
        /// <param name="killed"></param>
        public static void Kill(Unit_Client killer, Unit_Client killed)
        {
            //此处击杀者有很多处理，例如红名，pk值，记录到被杀的仇恨列表，击杀排行榜记录等等的需求
            killed.GetParent<UnitComponent_Client>().Remove(killed.Id);
        }
    }
}
