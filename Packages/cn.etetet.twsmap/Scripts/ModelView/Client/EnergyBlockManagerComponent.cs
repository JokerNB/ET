namespace ET.Client
{
    [ComponentOf(typeof(MapManagerComponent))]
    public class EnergyBlockManagerComponent : Entity, IAwake, IDestroy
    {
        public long createInterval_S = 10000; //10s
        public long createInterval_M = 10000; //10s
        public long createInterval_L = 10000; //10s
        
        public float Rate_S = 0.6f;
        public float Rate_M = 0.3f;
        public float Rate_L = 0.1f;

        public long Rate_S_Timer = default;
        public long Rate_M_Timer = default;
        public long Rate_L_Timer = default;
    }
}