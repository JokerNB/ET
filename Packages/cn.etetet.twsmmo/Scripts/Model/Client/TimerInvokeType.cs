namespace ET
{
    public static partial class TimerInvokeType
    {
        public const int BuffExpireTimer = PackageType.MMO * 1000 + 1;
        public const int BuffTick = PackageType.MMO * 1000 + 2;
        public const int CastRepeatedTick = PackageType.MMO * 1000 + 3;
        public const int CastAutoDisposeTimer = PackageType.MMO * 1000 + 4;
    }
}