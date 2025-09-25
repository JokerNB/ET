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
            self.CurArchiveInfo.Clear();
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
                        self.AddChild(info);
                        self.Add(accountName, info);
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
                int archiveNumber = self.ArchiveLastNumber.TryGetValue(archiveInfo.AccountLongHash, out int value) ? value + 1 : 1;
                archiveInfo.Initialize(accountName,archiveNumber);
                // archiveInfo.AccountLongHash = accountName.GetLongHashCode();
                // archiveInfo.ArchiveNumber = self.ArchiveLastNumber.TryGetValue(archiveInfo.AccountLongHash, out int value) ? value + 1 : 1;
                
                self.Add(accountName, archiveInfo);
                await dbComponent.Save(archiveInfo);
            }
        }

        public static async ETTask UpdateArchiveList(this ET.Server.ArchiveInfoManagerComponent self, ArchiveInfoProto archiveInfoProto)
        {
            if (self.ArchiveInfos.TryGetValue(archiveInfoProto.AccountHash, out var archiveInfoList))
            {
                for (int i = 0; i < archiveInfoList.Count; i++)
                {
                    ArchiveInfo archiveInfo = archiveInfoList[i];
                    if (archiveInfo.AccountLongHash == archiveInfoProto.AccountHash)
                    {
                        archiveInfo.FromMessage(archiveInfoProto);
                        await self.SaveArchive(archiveInfo);
                        Log.Error("存档成功 ！！！");
                        break;
                    }
                }
            }
        }

        public static async ETTask SaveArchive(this ET.Server.ArchiveInfoManagerComponent self, ArchiveInfo archiveInfo)
        {
            CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.OperaArchiveInfos, archiveInfo.AccountLongHash))
            {
                DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
                await dbComponent.Save(archiveInfo);
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

        public static int SelectCurArchive(this ET.Server.ArchiveInfoManagerComponent self, string accountName, int archiveNumber)
        {
            if (self.ArchiveInfos.TryGetValue(accountName.GetLongHashCode(), out var archiveInfoList))
            {
                foreach (ArchiveInfo archiveInfo in archiveInfoList)
                {
                    if (archiveInfo.ArchiveNumber == archiveNumber)
                    {
                        self.CurArchiveInfo[archiveInfo.AccountLongHash] = archiveInfo;
                        return ErrorCode.ERR_Success;
                    }
                }
            }

            return ErrorCode.ERR_SelectArchiveError;
        }

        public static void RemoveCurArchive(this ET.Server.ArchiveInfoManagerComponent self, string accountName)
        {
            if (self.CurArchiveInfo.ContainsKey(accountName.GetLongHashCode()))
                self.CurArchiveInfo.Remove(accountName.GetLongHashCode());
        }
    }
}