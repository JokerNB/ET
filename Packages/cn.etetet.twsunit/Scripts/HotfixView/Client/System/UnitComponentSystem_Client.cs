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
        }

        public static Unit_Client Get(this UnitComponent_Client self, long id)
        {
            Unit_Client unit = self.GetChild<Unit_Client>(id);
            return unit;
        }

        public static void Remove(this UnitComponent_Client self, long id)
        {
            self.RemoveChild(id);
        }
    }
}