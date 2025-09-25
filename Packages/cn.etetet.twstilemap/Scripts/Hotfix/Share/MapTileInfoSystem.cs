using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(MapTileInfo))]
    public static partial class MapTileInfoSystem
    {

        [EntitySystem]
        private static void Awake(this ET.MapTileInfo self)
        {
        }
        
        [EntitySystem]
        private static void Deserialize(this ET.MapTileInfo self)
        {
            foreach (int[] tilePos in self.tilePos_DB)
            {
                int2 pos = new int2(tilePos[0], tilePos[1]);
                self.tilePos.Add(pos);
            }
        }

        public static MapTileInfoProto ToMessage(this MapTileInfo self)
        {
            MapTileInfoProto mapTileInfoProto = MapTileInfoProto.Create();
            mapTileInfoProto.configId = self.configId;
            mapTileInfoProto.tilePos = new List<int2>(self.tilePos);
            return mapTileInfoProto;
        }

        public static void FromMessage(this ET.MapTileInfo self, MapTileInfoProto mapTileInfoProto)
        {
            self.configId = mapTileInfoProto.configId;
            self.tilePos = new List<int2>(mapTileInfoProto.tilePos);
            self.tilePos_DB.Clear();
            foreach (int2 pos in self.tilePos)
            {
                int[] tilePos = new int[] { pos.x, pos.y };
                self.tilePos_DB.Add(tilePos);
            }
        }

        public static void UpdateTilePos(this ET.MapTileInfo self, List<int2> tilePos)
        {
            foreach (int2 pos in tilePos)
            {
                if (self.tilePos.Contains(pos))
                {
                    Log.Error($"Contains pos : {pos}");
                    continue;
                }
                self.tilePos.Add(pos);
                self.tilePos_DB.Add(new[] { pos.x, pos.y });
            }
        }
    }
}