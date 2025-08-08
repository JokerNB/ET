namespace ET.Server
{
    [EntitySystemOf(typeof(RoleInfo))]
    public static partial class RoleInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.RoleInfo self)
        {

        }

        public static void FromMessage(this RoleInfo self, RoleInfoProto proto)
        {
            self.RoleName = proto.Name;
            self.ServerId = proto.ServerId;
            self.CreateTime = proto.CreateTime;
            self.LastLoginTime = proto.LastLoginTime;
            self.State = proto.State;
            self.AccountName = proto.Account;
        }

        public static RoleInfoProto ToMessage(this ET.Server.RoleInfo self)
        {
            RoleInfoProto roleInfoProto = RoleInfoProto.Create();
            roleInfoProto.Id = self.Id;
            roleInfoProto.ServerId = self.ServerId;
            roleInfoProto.CreateTime = self.CreateTime;
            roleInfoProto.LastLoginTime = self.LastLoginTime;
            roleInfoProto.Name = self.RoleName;
            roleInfoProto.State = self.State;
            roleInfoProto.Account = self.AccountName;
            return roleInfoProto;
        }
    }
}
