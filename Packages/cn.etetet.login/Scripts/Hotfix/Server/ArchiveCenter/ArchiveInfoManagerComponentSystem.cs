using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(ArchiveInfoManagerComponent))]
    [FriendOfAttribute(typeof(ET.ArchiveInfo))]
    public static partial class ArchiveInfoManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.ArchiveInfoManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.ArchiveInfoManagerComponent self)
        {
            self.ArchiveInfos.Clear();
            self.ArchiveLastNumber.Clear();
        }

        public static void Add(this ET.Server.ArchiveInfoManagerComponent self, string accountName, ET.ArchiveInfo archiveInfo)
        {
            var hash = accountName.GetLongHashCode();
            if (self.ArchiveInfos.TryGetValue(hash, out var archiveInfosList))
            {
                archiveInfosList.Add(archiveInfo);
                self.ArchiveLastNumber[hash] = archiveInfo.ArchiveNumber;
            }
            else
            {
                self.ArchiveInfos.Add(hash, new List<EntityRef<ArchiveInfo>>()
                {
                    archiveInfo
                });
                self.ArchiveLastNumber.Add(hash, archiveInfo.ArchiveNumber);
            }
        }

        public static async ETTask<List<ArchiveInfoProto>> GetArchiveInfoList(this ET.Server.ArchiveInfoManagerComponent self, string accountName)
        {
            List<ArchiveInfoProto> archiveInfoProtoList = new List<ArchiveInfoProto>();
            var hash = accountName.GetLongHashCode();
            if (self.ArchiveInfos.TryGetValue(hash, out var archiveInfoList))
            {
                foreach (ArchiveInfo archiveInfo in archiveInfoList)
                {
                    archiveInfoProtoList.Add(archiveInfo.ToMessage());
                }
            }
            else
            {
                await self.LoadArchive(accountName);
                if (self.ArchiveInfos.TryGetValue(hash, out var archiveInfoList1))
                {
                    foreach (ArchiveInfo archiveInfo in archiveInfoList1)
                    {
                        archiveInfoProtoList.Add(archiveInfo.ToMessage());
                    }
                }
            }

            return archiveInfoProtoList;
        }

        public static async ETTask LoadArchive(this ET.Server.ArchiveInfoManagerComponent self, string accountName)
        {
            CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            long longHashCode = accountName.GetLongHashCode();
            using (await coroutineLockComponent.Wait(CoroutineLockType.OperaArchiveInfos, longHashCode))
            {
                DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
                var archiveInfos = await dbComponent.Query<ArchiveInfo>(d => d.AccountLongHash == longHashCode);
                if (archiveInfos != null && archiveInfos.Count > 0)
                {
                    foreach (ArchiveInfo info in archiveInfos)
                    {
                        self.Add(accountName, info);
                        self.AddChild(info);
                    }
                }
            }
        }

        public static async ETTask AddNewArchive(this ET.Server.ArchiveInfoManagerComponent self, string accountName)
        {
            CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.OperaArchiveInfos, accountName.GetLongHashCode()))
            {
                DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());

                var archiveInfo = self.AddChild<ArchiveInfo>();
                archiveInfo.AccountLongHash = accountName.GetLongHashCode();
                archiveInfo.ArchiveNumber = self.ArchiveLastNumber[archiveInfo.AccountLongHash] + 1;
                self.Add(accountName, archiveInfo);
                await dbComponent.Save(archiveInfo);
            }
        }

        public static async ETTask UpdateArchiveList(this ET.Server.ArchiveInfoManagerComponent self, ArchiveInfoProto archiveInfoProto)
        {
            if (self.ArchiveInfos.TryGetValue(archiveInfoProto.AccountHash, out var archiveInfoList))
            {
                foreach (ArchiveInfo archiveInfo in archiveInfoList)
                {
                    if (archiveInfo.AccountLongHash == archiveInfoProto.AccountHash)
                    {
                        archiveInfo.FromMessage(archiveInfoProto);
                        CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
                        using (await coroutineLockComponent.Wait(CoroutineLockType.OperaArchiveInfos, archiveInfoProto.AccountHash))
                        {
                            DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
                            await dbComponent.Save(archiveInfo);
                        }

                        break;
                    }
                }
            }
        }

        public static async ETTask RemoveArchive(this ET.Server.ArchiveInfoManagerComponent self, string accountName, int archiveNumber)
        {
            var hash = accountName.GetLongHashCode();
            if (self.ArchiveInfos.TryGetValue(hash, out var archiveInfosList))
            {
                ArchiveInfo archiveInfo_Remove = null;
                foreach (ArchiveInfo archiveInfo in archiveInfosList)
                {
                    if (archiveInfo.ArchiveNumber == archiveNumber)
                    {
                        archiveInfo_Remove = archiveInfo;
                        break;
                    }
                }

                if (archiveInfo_Remove != null)
                {
                    self.ArchiveInfos[hash].Remove(archiveInfo_Remove);
                }

                CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
                using (await coroutineLockComponent.Wait(CoroutineLockType.OperaArchiveInfos, hash))
                {
                    DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());

                    await dbComponent.Remove<ArchiveInfo>(archiveInfo_Remove.Id);
                }

                await ETTask.CompletedTask;
            }
        }
    }
}