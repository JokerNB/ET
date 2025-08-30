namespace ET.Client
{
    [EntitySystemOf(typeof(Actions))]
    public static partial class ActionsSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Actions self, int args2, int idx)
        {
            self.ConfigId = args2;
            self.idx = idx;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Actions self)
        {
            self.ConfigId = default;
            self.Caster = default;
            self.Owner = default;
            self.idx = default;
        }
    }
}