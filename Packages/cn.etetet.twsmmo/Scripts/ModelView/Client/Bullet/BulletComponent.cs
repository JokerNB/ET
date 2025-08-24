namespace ET.Client
{
    //子弹unit
    [ComponentOf(typeof(Unit_Client))]
    public class BulletComponent : Entity, IAwake<int>, IDestroy
    {
        public int configId;
        public BulletConfig config => BulletConfigCategory.Instance.Get(this.configId);

        public long ownerId;
        public long TickTimer;
    }
}