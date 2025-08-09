using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf]
    public class Item : Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int ContainerType { get; set; }

        [BsonIgnore]
        public ItemConfig ItemConfig => ItemConfigCategory.Instance.Get(this.ConfigId);
    }
}