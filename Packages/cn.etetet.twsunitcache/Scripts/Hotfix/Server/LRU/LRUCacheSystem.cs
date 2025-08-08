using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LRUCache))]
    [FriendOfAttribute(typeof(ET.Server.LRUNode))]
    public static partial class LRUCacheSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.LRUCache self)
        {
            self.MinFrequency = 0;
            self.FrequencyDic.Add(0, new LinkedList<EntityRef<LRUNode>>());
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.LRUCache self)
        {
            self.MinFrequency = 0;
            self.FrequencyDic.Clear();
            self.LRUNodeDic.Clear();
        }

        public static void Call(this LRUCache self, long key)
        {
            EntityRef<LRUNode> nodeRef;
            LRUNode n;
            if (self.LRUNodeDic.TryGetValue(key, out nodeRef))
            {
                n = nodeRef;
                self.FrequencyDic[n.Frequency].Remove(n);
                n.Frequency++;
                if (!self.FrequencyDic.ContainsKey(n.Frequency))
                    self.FrequencyDic.Add(n.Frequency, new LinkedList<EntityRef<LRUNode>>());

                if (self.FrequencyDic[self.MinFrequency].Count == 0)
                    self.MinFrequency = n.Frequency;
                return;
            }

            n = self.AddChild<LRUNode, long>(key);
            n.Frequency = 0;

            self.FrequencyDic[0].AddLast(n);
            self.MinFrequency = 0;
            self.LRUNodeDic[key] = n;

            if (self.LRUNodeDic.Count >= 3000)
            {
                LRUNode fn = self.FrequencyDic[self.MinFrequency].First.Value;
                long unitId = fn.Key;
                self.FrequencyDic[self.MinFrequency].RemoveFirst();
                self.LRUNodeDic.Remove(unitId);
                fn.Dispose();

                EventSystem.Instance.Invoke(SceneType.UnitCache, new LRUUnitCacheDelete
                {
                    LRUCache = self,
                    key = unitId
                });
            }
        }
    }
}