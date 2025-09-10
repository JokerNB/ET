using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCacheComponent))]
    [FriendOfAttribute(typeof(ET.Server.UnitCache))]
    public static partial class UnitCacheComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitCacheComponent self)
        {
            self.UnitCacheKeyList.Clear();
            foreach (Type type in CodeTypes.Instance.GetTypes().Values)
            {
                if (type != typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                {
                    self.UnitCacheKeyList.Add(type.FullName);
                }
            }

            foreach (string key in self.UnitCacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }

            self.AddComponent<LRUCache>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.UnitCacheComponent self)
        {
            foreach (var unitCacheRef in self.UnitCaches.Values)
            {
                UnitCache unitCache = unitCacheRef;
                unitCache?.Dispose();
            }
            self.UnitCaches.Clear();
        }

        public static void CallCache(this UnitCacheComponent self, long id)
        {
            self.GetComponent<LRUCache>().Call(id);
        }

        public static async ETTask<Entity> Get(this ET.Server.UnitCacheComponent self, string key, long unitId)
        {
            UnitCache unitCache = default;
            if (!self.UnitCaches.TryGetValue(key, out EntityRef<UnitCache> unitCacheRef))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            else
                unitCache = unitCacheRef;

            return await unitCache.Get(unitId);
        }

        public static async ETTask Delete(this ET.Server.UnitCacheComponent self, long unitId)
        {
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UnitCacheGet,unitId))
            {
                foreach (EntityRef<UnitCache> unitCacheRef in self.UnitCaches.Values)
                {
                    UnitCache unitCache = unitCacheRef;
                    unitCache.Delete(unitId);
                }
            }
        }

        public static async ETTask AddOrUpdate(this ET.Server.UnitCacheComponent self, long id, List<Entity> entityList)
        {
            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                self.CallCache(id);
                foreach (var entity in entityList)
                {
                    string key = entity.GetType().FullName;
                    UnitCache unitCache = default;
                    if (!self.UnitCaches.TryGetValue(key, out EntityRef<UnitCache> unitCacheRef))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCaches.Add(key, unitCache);
                    }
                    else
                        unitCache = unitCacheRef;
                    
                    unitCache.AddOrUpdate(entity);
                    list.Add(entity);
                }

                Log.Error($"UnitCacheComponent AddOrUpdate Count == {list.Count}");
                if (list.Count > 0)
                    await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(id, list);
            }
        }
    }
}