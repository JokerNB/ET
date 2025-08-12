namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCache))]
    public static partial class UnitCacheSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitCache self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.UnitCache self)
        {
            // foreach (var entityRef in self.CacheComponentsDic.Values)
            // {
            //     Entity entity = entityRef;
            //     entity.Dispose();
            // }

            self.CacheComponentsDic.Clear();
            self.key = default;
        }

        public static async ETTask<Entity> Get(this ET.Server.UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentsDic.TryGetValue(unitId, out byte[] entityBson))
            {
                entity = await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            else
            {
                entity = MongoHelper.Deserialize<Entity>(entityBson);
            }

            return entity;
        }

        public static void Delete(this ET.Server.UnitCache self, long unitId)
        {
            if (self.CacheComponentsDic.TryGetValue(unitId, out byte[] entityBson))
            {
                self.CacheComponentsDic.Remove(unitId);
                Entity entity = MongoHelper.Deserialize<Entity>(entityBson);
                ;
                entity.Dispose();
            }
        }

        public static void AddOrUpdate(this ET.Server.UnitCache self, Entity entity)
        {
            if (entity == null)
                return;
            //TODO:传入的Entity与已缓存的反序列化的Entity是否有区别（entity数据未改变的情况下）
            if (self.CacheComponentsDic.TryGetValue(entity.Id, out byte[] oldEntityBson))
            {
                Entity oldEntity = MongoHelper.Deserialize<Entity>(oldEntityBson);
                if (entity != oldEntity)
                    oldEntity.Dispose();
                Log.Error(
                    $"UnitCache AddOrUpdate , oldEntityName : {oldEntity.GetType().FullName} , entityName : {entity.GetType().FullName} , 是否为同一个Entity : {oldEntity == entity}");
                self.CacheComponentsDic.Remove(entity.Id);
            }

            self.CacheComponentsDic.Add(entity.Id, entity.ToBson());
        }
    }
}