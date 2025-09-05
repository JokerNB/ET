namespace ET
{
    public static partial class TimerInvokeType
    {
        public const int MonsterCheckTimer = PackageType.Monster * 1000 + 1;
        public const int CreateNormalMonster = PackageType.Monster * 1000 + 2;
        public const int CreateFinalMonster = PackageType.Monster * 1000 + 3;
        public const int MonsterAttackRepeatedTimer = PackageType.Monster * 1000 + 4;
    }
}