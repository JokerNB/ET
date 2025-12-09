namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class FTServerModelComponent : Entity, IAwake
    {
        public Fantasy.Scene _scene;
        public Fantasy.Network.Session _session;
    }
}
