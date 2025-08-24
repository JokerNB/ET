using System.Collections.Generic;

namespace ET.Client
{
    
    [EntitySystemOf(typeof(UnitComponent_Client))]
    public static partial class UnitComponentSystem_Client
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UnitComponent_Client self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Client.UnitComponent_Client self)
        {

        }

        public static void Add(this UnitComponent_Client self, Unit_Client unit)
        {
            if (unit.IsMonster())
                self.monsters.Add(unit);
            if (unit.isUnit())
                self.units.Add(unit);
            if(unit.UnitType == UnitType.Player)
                self.Unit_Player = unit;
                
        }
        
        public static Unit_Client Get(this UnitComponent_Client self, long id)
        {
            Unit_Client unit = self.GetChild<Unit_Client>(id);
            return unit;
        }

        public static void Remove(this UnitComponent_Client self, long id)
        {
            Unit_Client unit = self.GetChild<Unit_Client>(id);
            unit?.Dispose();
        }

        public static List<EntityRef<Unit_Client>> GetAllUnits(this UnitComponent_Client self)
        {
            return self.units;
        }
        
        public static List<EntityRef<Unit_Client>> GetAllMonster(this UnitComponent_Client self)
        {
            return self.monsters;
        }

        public static void RemoveAllMonster(this UnitComponent_Client self)
        {
            foreach (Unit_Client unit in self.monsters)
            {
                self.Remove(unit.Id);
            }

            self.monsters.Clear();
        }
    }
}
