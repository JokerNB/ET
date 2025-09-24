using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(ArchiveInfo))]
    [FriendOfAttribute(typeof(ET.MapTileInfo))]
    public static partial class ArchiveInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.ArchiveInfo self)
        {
        }

        public static ArchiveInfoProto ToMessage(this ArchiveInfo self)
        {
            ArchiveInfoProto archiveInfoProto = ArchiveInfoProto.Create();
            archiveInfoProto.AccountHash = self.AccountLongHash;
            archiveInfoProto.ArchiveNum = self.ArchiveNumber;
            archiveInfoProto.Recruits = new List<long>(self.RecruitUnitIds);
            var list = new List<MapTileInfoProto>();
            foreach (MapTileInfo mapTileInfo in self.MapTileInfos)
            {
                list.Add(mapTileInfo.ToMessage());
            }
            archiveInfoProto.MapTileInfos = list;
            return archiveInfoProto;
        }

        public static void FromMessage(this ET.ArchiveInfo self, ArchiveInfoProto archiveInfoProto)
        {
            self.RecruitUnitIds.Clear();
            self.RecruitUnitIds.AddRange(archiveInfoProto.Recruits);
            self.ArchiveNumber = archiveInfoProto.ArchiveNum;
            self.AccountLongHash = archiveInfoProto.AccountHash;
            foreach (MapTileInfoProto mapTileInfoProto in archiveInfoProto.MapTileInfos)
            {
                var mapTileInfo = self.AddChild<MapTileInfo>();
                mapTileInfo.FromMessage(mapTileInfoProto);
            }
        }

        public static MapTileInfo AddNewMapTileInfo(this ET.ArchiveInfo self, int configId, List<int2> tilePos)
        {
            var mapTileInfo = self.AddChild<MapTileInfo>();
            mapTileInfo.configId = configId;
            mapTileInfo.tilePos = tilePos;
            return mapTileInfo;
        }
    }
}