using ET.Server;

namespace ET
{
    [EntitySystemOf(typeof(Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void GetComponentSys(this ET.Unit self, System.Type type)
        {
            if(self.UnitType == UnitType.Player)
                return;
            
            if (!(typeof(IUnitCache).IsAssignableFrom(type)))
                return;
            
            EventSystem.Instance.Publish(self.Scene(), new UnitGetComponent
            {
                unit = self,
                Type = type
            });
        }

        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }

        public static T GetOrAddComponent<T>(this Unit self) where T : Entity, IAwake, new()
        {
            T entity = self.GetComponent<T>();
            if (entity == null)
            {
                entity = self.AddComponent<T>();
                if (!(typeof(IUnitCache).IsAssignableFrom(typeof(T))))
                    return entity;
                EventSystem.Instance.Publish(self.Root(), new UnitAddComponentByGetNull()
                {
                    unit = self,
                    type = entity.GetType()
                });
            }

            return entity;
        }
    }
}