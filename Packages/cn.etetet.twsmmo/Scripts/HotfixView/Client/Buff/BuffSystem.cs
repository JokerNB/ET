using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Buff self, int args2)
        {
            self.ConfigId = args2;
            self.AddComponent<ActionsTempComponent>();

            self.CreateTime = TimeInfo.Instance.ServerNow();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Buff self)
        {
            self.ConfigId = default;
            self.Owner = default;
            self.CreateTime = default;
            self.TickTime = default;
            self.TickBeginTime = default;

            self.Root().GetComponent<TimerComponent>().Remove(ref self.TickTimer);
            self.Root().GetComponent<TimerComponent>().Remove(ref self.WaitTickTimer);

            self.ExpireTime = default;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.ExpireTimer);
        }

        public static void AddActions(this Buff self)
        {
            long instanceId = self.InstanceId;
            int idx = 0;
            foreach (int i in self.Config.AddAction)
            {
                try
                {
                    self.CreateActions(i, idx, ActionsRunType.BuffAdd);
                    //可能在效果的过程中，本Buff被移除回池了，然后又从池里取出来了，所以如果值判断IsDisposed是不够的
                    if (self.InstanceId != instanceId)
                        break;
                    idx++;
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"AddActions error, OwnerId: {((Unit_Client)self.Owner).Id} , buffId: {self.Id} , buffConfig: {self.ConfigId} , Actions:{i} , {e}");
                }
            }
        }

        public static void RemoveActions(this Buff self)
        {
            long instanceId = self.InstanceId;
            int idx = 0;
            foreach (int i in self.Config.RemoveAction)
            {
                try
                {
                    self.CreateActions(i, idx, ActionsRunType.BuffRemove);
                    if (self.InstanceId != instanceId)
                        break;
                    idx++;
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"RemoveActions error, OwnerId: {((Unit_Client)self.Owner).Id} , buffId: {self.Id} , buffConfig: {self.ConfigId} , Actions:{i} , {e}");
                }
            }
        }

        public static void TickActions(this Buff self)
        {
            if (self.IsDisposed)
                return;
            long instanceId = self.InstanceId;
            int idx = 0;
            foreach (int i in self.Config.TickAction)
            {
                try
                {
                    self.CreateActions(i,idx, ActionsRunType.BuffTick);
                    if (self.InstanceId != instanceId)
                        break;
                    idx++;
                }
                catch (Exception e)
                {
                    if (instanceId == self.InstanceId)
                    {
                        Log.Error(
                            $"TickActions error, OwnerId: {((Unit_Client)self.Owner)?.Id ?? 0} , buffId: {self.Id} , buffConfig: {self.ConfigId} , Actions:{i} , {e}");
                    }
                    else
                    {
                        Log.Error($"TickActions error, OwnerId: {((Unit_Client)self.Owner)?.Id ?? 0} , buffId: {self.Id} , Actions:{i} , {e}");
                    }
                }
            }

            if (self.InstanceId != instanceId)
                return;

            if (self.Config.TickAction.Count > 0)
            {
                Unit_Client owner = self.Owner;
                if (owner == null)
                    return;
                EventSystem.Instance.Publish(self.Root().CurrentScene(), new Event_BuffTick
                {
                    BuffId = self.Id,
                    OwnerId = owner.Id
                });
            }
        }
    }
}