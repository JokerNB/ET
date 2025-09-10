namespace ET.Server
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze
    }
    [ChildOf]
    public class RoleInfo : Entity, IAwake
    {
        public string RoleName;
        public int State;
        public long LastLoginTime;
        public long CreateTime;
        public int ServerId;
        public string AccountName;
    }
}
