namespace ET.Client
{
    [EntitySystemOf(typeof(Unit_Client))]
    public static partial class UnitSystem_Client
    {
        [EntitySystem]
        private static void Awake(this Unit_Client self, int configId)
        {
            self.ConfigId = configId;
        }

        [EntitySystem]
        private static void Destroy(this Unit_Client self)
        {
            self.ConfigId = default;
        }

        public static UnitConfig Config(this Unit_Client self)
        {
            return UnitConfigCategory.Instance.Get(self.ConfigId);
        }

        public static bool IsMonster(this Unit_Client unit)
        {
            return unit.UnitType is UnitType.Monster or UnitType.EliteMonster or UnitType.LittleBoss or UnitType.FinalBoss;
        }

        public static bool isUnit(this Unit_Client self)
        {
            return self.UnitType is not UnitType.None and not UnitType.ParticleEffect;
        }
    }
}