using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(BuffComponent))]
    [FriendOfAttribute(typeof(ET.Client.BuffCreateInfo))]
    [FriendOfAttribute(typeof(ET.Client.Buff))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.BuffComponent self)
        {
            self.AddComponent<BuffTempComponent>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.BuffComponent self)
        {
            self.ConfigIdBuffs.Clear();
        }

        public static BuffCreateInfo Create(this ET.Client.BuffComponent self, int configId)
        {
            return self.GetComponent<BuffTempComponent>().AddChild<BuffCreateInfo, int>(configId);
        }

        public static bool CreateAndAdd(this ET.Client.BuffComponent self, int configId)
        {
            using (BuffCreateInfo buffCreateInfo = self.Create(configId))
            {
                return self.Add(buffCreateInfo);
            }
        }

        public static bool Add(this ET.Client.BuffComponent self, BuffCreateInfo buffCreateInfo)
        {
            if (buffCreateInfo == null || buffCreateInfo.IsDisposed)
                return false;
            if (self == null || self.IsDisposed)
                return false;
            Buff buff = self.AddChild<Buff, int>(buffCreateInfo.ConfigId);
            Unit_Client owner = self.GetParent<Unit_Client>();
            buff.Owner = owner;

            if (owner == null)
            {
                buff.Dispose();
                return false;
            }

            int configId = buff.ConfigId;
            if (self.ConfigIdBuffs.ContainsKey(configId))
            {
                //已有相同的Buff，框架Demo是直接顶掉，根据各自项目需求设计
                Buff oldBuff = self.ConfigIdBuffs[configId];
                self.Remove(oldBuff.Id);
            }

            self.ConfigIdBuffs.Add(configId, buff);
            EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_BuffAdd
            {
                OwnerId = owner.Id,
                buffData = buff
            });
            buff.AddActions();
            return true;
        }

        public static void Remove(this ET.Client.BuffComponent self, long buffId)
        {
            if (!self.Children.TryGetValue(buffId, out Entity entity))
                return;
            Buff buff = entity as Buff;
            try
            {
                self.ConfigIdBuffs.Remove(buff.ConfigId);
                EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_BuffRemove
                {
                    BuffId = buff.Id,
                    OwnerId = ((Unit_Client)buff.Owner).Id
                });
                //TODO:触发移除Buff的行为实体逻辑
                buff.RemoveActions();
                buff.Dispose();
            }
            catch (Exception e)
            {
                Log.Error($"Buff 'Remove' error! buffCompId : {self.Id} , buffId: {buffId} , buffConfigId: {buff.Config?.Id ?? 0} , {e}");
            }
        }

        public static Buff Get(this BuffComponent self, long buffId)
        {
            return self.GetChild<Buff>(buffId);
        }
    }
}