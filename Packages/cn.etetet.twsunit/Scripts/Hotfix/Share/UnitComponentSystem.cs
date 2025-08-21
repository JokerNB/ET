using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [FriendOfAttribute(typeof(ET.UnitComponent))]
    public static partial class UnitComponentSystem
    {
        public static void Add(this UnitComponent self, Unit unit)
        {
            if (unit.UnitType != UnitType.None && unit.UnitType != UnitType.Player)
                self.monsters.Add(unit);
        }

        public static Unit Get(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            return unit;
        }

        public static void Remove(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            unit?.Dispose();
        }

        public static void RemoveAllMonster(this UnitComponent self)
        {
            foreach (Unit unit in self.monsters)
            {
                self.Remove(unit.Id);
            }

            self.monsters.Clear();
        }
    }
}