using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(ArchiveInfoManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.ArchiveInfo))]
    public static partial class ArchiveInfoManagerComponent_ClientSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.ArchiveInfoManagerComponent_Client self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.ArchiveInfoManagerComponent_Client self)
        {
            self.RemoveCurArchive();
            self.ArchiveInfos.Clear();
        }

        public static void Add(this ArchiveInfoManagerComponent_Client self, ET.ArchiveInfo archiveInfo)
        {
            self.ArchiveInfos.Add(archiveInfo);
        }

        public static void Remove(this ArchiveInfoManagerComponent_Client self, ET.ArchiveInfo archiveInfo)
        {
            self.ArchiveInfos.Remove(archiveInfo);
        }

        public static async ETTask<int> AddNew(this ArchiveInfoManagerComponent_Client self)
        {
            var archiveInfo = self.AddChild<ArchiveInfo>();
            self.Add(archiveInfo);

            C2R_AddNewArchiveRequest msg = C2R_AddNewArchiveRequest.Create();
            msg.AccountName = self.Root().GetComponent<PlayerComponent>().Account;
            var response = await self.Root().GetComponent<ClientSenderComponent>().Call(msg) as C2R_AddNewArchiveResponse;
            return response.Error;
        }

        public static async ETTask<int> Update(this ArchiveInfoManagerComponent_Client self, ArchiveInfo archiveInfo)
        {
            C2M_UpdateArchiveRequest msg = C2M_UpdateArchiveRequest.Create();
            msg.ArchiveInfoProto = archiveInfo.ToMessage();
            var response = await self.Root().GetComponent<ClientSenderComponent>().Call(msg) as C2M_UpdateArchiveResponse;
            return response.Error;
        }

        public static void InitArchiveList(this ET.Client.ArchiveInfoManagerComponent_Client self, List<ArchiveInfoProto> archiveInfoList)
        {
            foreach (ArchiveInfoProto archiveInfoProto in archiveInfoList)
            {
                var archiveInfo = self.AddChild<ArchiveInfo>();
                archiveInfo.FromMessage(archiveInfoProto);
                self.Add(archiveInfo);
            }
        }

        public static void SelectCurArchive(this ArchiveInfoManagerComponent_Client self, int idx)
        {
            if (idx >= 0 && idx < self.ArchiveInfos.Count)
            {
                self.CurArchiveInfo = self.ArchiveInfos[idx];
            }
        }

        public static void RemoveCurArchive(this ArchiveInfoManagerComponent_Client self)
        {
            self.CurArchiveInfo = default;
        }

        public static void SetCurArchiveInfoByArchiveNum(this ArchiveInfoManagerComponent_Client self, int archiveNum)
        {
            foreach (ArchiveInfo archiveInfo in self.ArchiveInfos)
            {
                if (archiveInfo.ArchiveNumber == archiveNum)
                {
                    self.CurArchiveInfo = archiveInfo;
                    return;
                }
            }

            if (self.GetCurArchiveInfo() == default)
            {
                var archiveInfo = self.AddChild<ArchiveInfo>();
                archiveInfo.ArchiveNumber = archiveNum;
                archiveInfo.AccountLongHash = self.Root().GetComponent<PlayerComponent>().AccountLongHashCode;
                self.CurArchiveInfo = archiveInfo;
            }
        }

        public static ArchiveInfo GetCurArchiveInfo(this ArchiveInfoManagerComponent_Client self)
        {
            if (self.CurArchiveInfo == default)
                return null;
            return self.CurArchiveInfo;
        }

        public static MapTileInfo AddNewMapTileByCurArchive(this ArchiveInfoManagerComponent_Client self, int configId, List<int2> tilePos)
        {
            ArchiveInfo curArchiveInfo = self.CurArchiveInfo;
            return curArchiveInfo.AddNewMapTileInfo(configId, tilePos);
        }
    }
}