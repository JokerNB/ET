namespace ET.Server
{
    public class DBVersion: Entity, IAwake
    {
        public int Version { get; set; }
    }
}