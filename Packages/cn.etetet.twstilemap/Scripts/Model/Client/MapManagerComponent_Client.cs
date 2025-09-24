using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapManagerComponent_Client : Entity, IAwake, IDestroy
    {
        public Dictionary<int, EntityRef<MapTileInfo>> ConfigID_MapTileInfoDic = new Dictionary<int, EntityRef<MapTileInfo>>();
    }
}