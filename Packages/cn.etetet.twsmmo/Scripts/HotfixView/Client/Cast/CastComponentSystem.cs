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
            self.Casts.Clear();
        }

        public static Cast Create(this ET.Client.CastComponent self, int configId, Unit_Client ownerUnit)
        {
            Cast cast = self.Get(configId);
            if (cast != null)
                return cast;
            cast = self.AddChild<Cast, int, Unit_Client>(configId, ownerUnit, true);
            self.Casts.Add(configId, cast);
            return cast;
        }

        public static Cast Get(this ET.Client.CastComponent self, int configId)
        {
            if (self.Casts.ContainsKey(configId))
            {
                Cast cast = self.Casts[configId];
                return cast;
            }
            else
                return null;
        }

        public static void Remove(this ET.Client.CastComponent self, int configId)
        {
            if (self.Casts.ContainsKey(configId))
            {
                Cast cast = self.Casts[configId];
                self.RemoveChild(cast.Id);
                self.Casts.Remove(configId);
            }
        }
    }
}