using System.Collections.Generic;
using System.Linq;

namespace ET
{
	public static partial class UnitComponentSystem
	{
		public static void Add(this UnitComponent self, Unit unit)
		{
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
			foreach (Entity entity in self.Children.Values.ToArray())
			{
				if (entity is Unit unit)
				{
					if(unit.UnitType == UnitType.Monster)
						self.Remove(unit.Id);
				}
			}

			self.RemoveAllChild();
		}
	}
}