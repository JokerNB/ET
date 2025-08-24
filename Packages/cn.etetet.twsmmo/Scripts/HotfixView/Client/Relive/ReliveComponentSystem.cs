namespace ET.Client
{
    [EntitySystemOf(typeof(ReliveComponent))]
    public static partial class ReliveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.ReliveComponent self)
        {
            self.Alive = true;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.ReliveComponent self)
        {
            self.Alive = default;
        }
    }
}