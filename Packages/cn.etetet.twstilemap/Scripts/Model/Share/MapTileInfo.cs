using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(ArchiveInfo))]
    public class MapTileInfo : Entity, IAwake, ISerializeToEntity, IDeserialize
    {
        public int configId;
        [BsonIgnore]
        public TileItemConfig Config => TileItemConfigCategory.Instance.Get(this.configId);
        [BsonIgnore]
        public List<int2> tilePos = new List<int2>();

        public List<int[]> tilePos_DB = new List<int[]>();

        [BsonIgnore]
        public int2 tileSize => new int2(this.Config.CellData[0], Config.CellData[1]);
    }
}