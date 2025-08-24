namespace ET.Client
{
    [EntitySystemOf(typeof(BuffCreateInfo))]
    public static partial class BuffCreateInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.BuffCreateInfo self, int args2)
        {
            self.ConfigId = args2;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.BuffCreateInfo self)
        {
            self.ConfigId = default;
        }
    }
}