namespace ET.Server
{
    public enum ServerStatus
    {
        Normal = 0,
        Stop = 1
    }

    [ChildOf]
    public class ServerInfo : Entity, IAwake
    {
        public int ServerStatus;
        public string ServerName;
    }
}