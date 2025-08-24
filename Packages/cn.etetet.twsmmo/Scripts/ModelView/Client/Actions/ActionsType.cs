namespace ET.Client
{
    [UniqueId]
    public static class ActionsType
    {
        public const int NumericChange = 1; //改变目标数值，如果是buff，删除会还原数值
        public const int Damage = 2; //造成伤害
        public const int CastBullet = 3; //创建子弹
    }
}

