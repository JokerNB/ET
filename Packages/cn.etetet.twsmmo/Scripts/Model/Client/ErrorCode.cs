namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Cast_ArgsError = ERR_WithException + PackageType.MMO * 1000 + 1;
        public const int ERR_Cast_CasterIsNull = ERR_WithException + PackageType.MMO * 1000 + 2;
        public const int ERR_Cast_TargetIsNull = ERR_WithException + PackageType.MMO * 1000 + 3;
        public const int ERR_Relive_Alive = ERR_WithException + PackageType.MMO * 1000 + 4;
    }
}
