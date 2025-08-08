using System;

namespace ET.Server
{
    [Invoke(TimerInvokeType.SaveChangeDBData)]
    public class UnitDBSaveComponentTimer : ATimer<UnitDBSaveComponent>
    {
        protected override void Run(UnitDBSaveComponent t)
        {
            try
            {
                t?.SaveChange().NoContext();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    [EntitySystemOf(typeof(UnitDBSaveComponent))]
    public static partial class UnitDBSaveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitDBSaveComponent self)
        {
            //正式上线部署 每10-15分钟随机存储落地一次
            // self.Timer = self.Root().GetComponent<TimerComponent>()
            //         .NewRepeatedTimer(RandomGenerator.RandomNumber(10, 16) * 60 * 1000, TimerInvokeType.SaveChangeDBData, self);

            //开发时每4s落地一次
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(4 * 1000, TimerInvokeType.SaveChangeDBData, self);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.UnitDBSaveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void AddToBytes(this ET.Server.UnitDBSaveComponent self, Type type, byte[] bytes)
        {
            self.Bytes[type] = bytes;
        }

        public static void AddChanges(this ET.Server.UnitDBSaveComponent self, Type type)
        {
            self.EntityChangeTypeSet.Add(type);
        }

        public static async ETTask SaveChange(this UnitDBSaveComponent self)
        {
            CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.Mailbox, self.GetParent<Unit>().InstanceId))
            {
                self.SaveChangeNoWait();
            }
        }

        public static void SaveChangeNoWait(this UnitDBSaveComponent self)
        {
            if (self.IsDisposed || self.Parent == null)
                return;
            if (self.Root() == null)
                return;
            Unit unit = self.GetParent<Unit>();
            if (unit == null || unit.IsDisposed)
                return;
            if (self.EntityChangeTypeSet.Count <= 0)
                return;

            Other2UnitCache_AddOrUpdateUnit other2UnitCacheAddOrUpdateUnit = Other2UnitCache_AddOrUpdateUnit.Create();
            other2UnitCacheAddOrUpdateUnit.UnitId = unit.Id;
            other2UnitCacheAddOrUpdateUnit.EntityTypes.Add(unit.GetType().FullName);
            other2UnitCacheAddOrUpdateUnit.EntityBytes.Add(unit.ToBson());

            foreach (var type in self.EntityChangeTypeSet)
            {
                Entity entity = unit.GetComponent(type);
                if (entity == null || entity.IsDisposed)
                    continue;
                Log.Debug($"开始保存变化部分Entity数据：{type.FullName}");
                byte[] bytes = entity.ToBson();
                other2UnitCacheAddOrUpdateUnit.EntityTypes.Add(type.FullName);
                other2UnitCacheAddOrUpdateUnit.EntityBytes.Add(bytes);
                self.AddToBytes(type, bytes);
            }

            self.EntityChangeTypeSet.Clear();

            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetOneBySceneType(unit.Zone(), SceneType.UnitCache);
            self.Root()?.GetComponent<MessageSender>().Call(startSceneConfig.ActorId, other2UnitCacheAddOrUpdateUnit).NoContext();
        }
    }
}