using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.MapTileInfo))]
    [FriendOfAttribute(typeof(ET.Client.ArchiveInfoManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.ArchiveInfo))]
    public static partial class MapManagerComponent_ClientSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapManagerComponent_Client self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MapManagerComponent_Client self)
        {
        }

        public static void InitMapData(this ET.Client.MapManagerComponent_Client self)
        {
            ArchiveInfo curArchiveInfo = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>().GetCurArchiveInfo();

            if(curArchiveInfo == null)
                return;
            foreach (MapTileInfo mapTileInfo in curArchiveInfo.MapTileInfos)
            {
                self.ConfigID_MapTileInfoDic.Add(mapTileInfo.configId, mapTileInfo);
            }
        }

        public static void BuildFinished(this ET.Client.MapManagerComponent_Client self, int configId, List<int2> tilePos)
        {
            if (self.ConfigID_MapTileInfoDic.ContainsKey(configId))
            {
                MapTileInfo mapTileInfo = self.ConfigID_MapTileInfoDic[configId];
                mapTileInfo.UpdateTilePos(tilePos);
            }
            else
            {
                MapTileInfo mapTileInfo = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>().AddNewMapTileByCurArchive(configId, tilePos);
                self.ConfigID_MapTileInfoDic.Add(mapTileInfo.configId, mapTileInfo);
            }
        }
    }
}