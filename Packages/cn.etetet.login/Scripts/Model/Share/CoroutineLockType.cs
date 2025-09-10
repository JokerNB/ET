namespace ET
{
    public static partial class CoroutineLockType
    {
        public const int LoginAccount = PackageType.Login * 1000 + 1;                  // 账号登录
        public const int GetRoles = PackageType.Login * 1000 + 2;                  // 获取角色列表
        public const int CreateRole = PackageType.Login * 1000 + 3;                  // 创建角色
        public const int LoginCenterLock = PackageType.Login * 1000 + 4;                  
        public const int LoginGate = PackageType.Login * 1000 + 5;                  
    }
}