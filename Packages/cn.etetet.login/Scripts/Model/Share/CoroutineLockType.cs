namespace ET
{
    public static partial class CoroutineLockType
    {
        public const int LoginAccount = PackageType.Login * 1000 + 1;                  // 账号登录
        public const int OperaArchiveInfos = PackageType.Login * 1000 + 2;                  // 操作存档列表
        public const int LoginCenterLock = PackageType.Login * 1000 + 4;                  
        public const int LoginGate = PackageType.Login * 1000 + 5;                  
    }
}