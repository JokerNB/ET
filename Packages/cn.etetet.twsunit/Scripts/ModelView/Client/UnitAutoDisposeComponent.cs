namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class UnitAutoDisposeComponent : Entity, IAwake<long>, IDestroy
    {
        public long Timer;
        public long durTime;
    }
}