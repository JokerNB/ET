using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ChildOf]
    public class MapTileInfo : Entity, IAwake
    {
        public int configId;
        [BsonIgnore]
        public TileItemConfig Config => TileItemConfigCategory.Instance.Get(this.configId);

        public List<int2> tilePos = new List<int2>();

        [BsonIgnore]
        public int2 tileSize => new int2(this.Config.CellData[0], Config.CellData[1]);
    }
}