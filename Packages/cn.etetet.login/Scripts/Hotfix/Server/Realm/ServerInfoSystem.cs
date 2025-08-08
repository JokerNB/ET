namespace ET.Server
{

    [EntitySystemOf(typeof(ServerInfo))]
    public static partial class ServerInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.ServerInfo self)
        {
        }

        public static void FromMessage(this ET.Server.ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.ServerStatus = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ET.Server.ServerInfo self)
        {
            ServerInfoProto serverInfoProto = ServerInfoProto.Create();
            serverInfoProto.Id = (int)self.Id;
            serverInfoProto.ServerName = self.ServerName;
            serverInfoProto.Status = self.ServerStatus;
            
            return serverInfoProto;
        }
    }
}
