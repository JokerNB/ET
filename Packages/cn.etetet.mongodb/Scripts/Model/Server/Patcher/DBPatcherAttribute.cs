namespace ET.Server
{
    public class DBPatcherAttribute : BaseAttribute
    {
        public int Version { get; }

        public DBPatcherAttribute(int version)
        {
            this.Version = version;
        }
    }
}
