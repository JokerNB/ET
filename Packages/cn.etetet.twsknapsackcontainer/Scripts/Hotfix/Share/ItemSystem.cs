namespace ET
{
    [EntitySystemOf(typeof(Item))]
    public static partial class ItemSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Item self, int args2)
        {
            self.ConfigId = args2;
        }

        [EntitySystem]
        private static void Destroy(this ET.Item self)
        {
            self.ConfigId = default;
            self.ContainerType = (int)KnapsackContainerType.None;
        }

        public static void FromMessage(this Item self, ItemProto itemProto)
        {
            self.ConfigId = itemProto.ConfigId;
            self.ContainerType = itemProto.ContainerTyper;
        }

        public static ItemProto ToMessage(this ET.Item self)
        {
            ItemProto itemProto = ItemProto.Create();
            itemProto.ConfigId = self.ConfigId;
            itemProto.ContainerTyper = self.ContainerType;
            return itemProto;
        }
    }
}