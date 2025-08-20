namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Cast_ArgsError = ERR_WithException + PackageType.Cast * 1000 + 1;
        public const int ERR_Cast_CasterIsNull = ERR_WithException + PackageType.Cast * 1000 + 2;
    }
}
