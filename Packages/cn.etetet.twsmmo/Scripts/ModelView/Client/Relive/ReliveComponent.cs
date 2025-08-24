namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class ReliveComponent : Entity, IAwake, IDestroy
    {
        public bool Alive = true;
    }
}