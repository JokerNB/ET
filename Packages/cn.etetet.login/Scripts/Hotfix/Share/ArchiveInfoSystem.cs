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

        [EntitySystem]
        private static void Deserialize(this ET.ArchiveInfo self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is MapTileInfo mapTileInfo)
                {
                    self.MapTileInfosDic.Add(mapTileInfo.configId, mapTileInfo);
                }
            }
        }

        public static void Initialize(this ET.ArchiveInfo self, string accountName, int archiveNumber)
        {
            self.AccountLongHash = accountName.GetLongHashCode();
            self.ArchiveNumber = archiveNumber;
        }

        public static ArchiveInfoProto ToMessage(this ArchiveInfo self)
        {
            ArchiveInfoProto archiveInfoProto = ArchiveInfoProto.Create();
            archiveInfoProto.AccountHash = self.AccountLongHash;
            archiveInfoProto.ArchiveNum = self.ArchiveNumber;
            archiveInfoProto.Recruits = new List<long>(self.RecruitUnitIds);
            var list = new List<MapTileInfoProto>();
            foreach (MapTileInfo mapTileInfo in self.MapTileInfosDic.Values)
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
                if (self.MapTileInfosDic.TryGetValue(mapTileInfoProto.configId, out var mapTileInfoRef))
                {
                    MapTileInfo mapTileInfo = mapTileInfoRef;
                    mapTileInfo.FromMessage(mapTileInfoProto);
                }
                else
                {
                    var mapTileInfo = self.AddChild<MapTileInfo>();
                    mapTileInfo.FromMessage(mapTileInfoProto);
                    self.MapTileInfosDic.Add(mapTileInfo.configId, mapTileInfo);
                }
            }
        }

        public static MapTileInfo AddNewMapTileInfo(this ET.ArchiveInfo self, int configId, List<int2> tilePos)
        {
            var mapTileInfo = self.AddChild<MapTileInfo>();
            mapTileInfo.configId = configId;
            mapTileInfo.UpdateTilePos(tilePos);
            self.MapTileInfosDic.Add(mapTileInfo.configId, mapTileInfo);
            return mapTileInfo;
        }

        public static bool UpdateMapTileInfo(this ET.ArchiveInfo self, MapTileInfo mapTileInfo)
        {
            if (self.MapTileInfosDic.TryGetValue(mapTileInfo.configId, out var mapTileInfoRef))
            {
                self.MapTileInfosDic[mapTileInfo.configId] = mapTileInfo;
                return true;
            }

            return false;
        }
    }
}