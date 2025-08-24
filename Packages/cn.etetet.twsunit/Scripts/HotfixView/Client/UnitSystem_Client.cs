namespace ET.Client
{
    [EntitySystemOf(typeof(Unit_Client))]
    public static partial class UnitSystem_Client
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Unit_Client self, int args2)
        {
            self.ConfigId = args2;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Unit_Client self)
        {
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
            return self.UnitType is not UnitType.None and not UnitType.Bullet;
        }
    }
}

