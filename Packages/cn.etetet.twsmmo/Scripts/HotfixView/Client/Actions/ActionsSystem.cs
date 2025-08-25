namespace ET.Client
{
    [EntitySystemOf(typeof(Actions))]
    public static partial class ActionsSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Actions self, int args2)
        {
            self.ConfigId = args2;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Actions self)
        {
            self.ConfigId = default;
            self.Caster = default;
            self.Owner = default;
        }
    }
}