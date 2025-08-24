namespace ET.Server
{
    [EntitySystemOf(typeof(Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void GetComponentSys(this ET.Server.Unit self, System.Type args2)
        {

        }
        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }
    }
}