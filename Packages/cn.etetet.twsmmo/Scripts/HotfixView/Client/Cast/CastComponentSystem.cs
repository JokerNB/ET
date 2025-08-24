namespace ET.Client
{
    [EntitySystemOf(typeof(CastComponent))]
    public static partial class CastComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.CastComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.CastComponent self)
        {
        }

        public static Cast Create(this ET.Client.CastComponent self, int configId)
        {
            Cast cast = self.AddChild<Cast, int>(configId);
            return cast;
        }
    }
}