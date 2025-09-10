namespace ET.Client
{
    [EntitySystemOf(typeof(KnapsackContainerComponent_Client))]
    public static partial class KnapsackContainerComponentSystem_Client
    {
        [EntitySystem]
        private static void Awake(this ET.Client.KnapsackContainerComponent_Client self, int arg)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.KnapsackContainerComponent_Client self)
        {
        }

        public static void AddItemFromMessage(this KnapsackContainerComponent_Client self, ItemProto itemProto)
        {
            Item item = self.AddChildWithId<Item, int>(itemProto.Id, itemProto.ConfigId);
            item.ContainerType = itemProto.ContainerTyper;
            self.Items.Add(item.Id, item);
        }

        public static void RemoveItemById(this KnapsackContainerComponent_Client self, long itemId)
        {
            if (!self.Items.TryGetValue(itemId, out EntityRef<Item> itemRef))
            {
                Log.Error($"itemId : {itemId} not found");
                return;
            }

            Item item = itemRef;
            self.Items.Remove(itemId);
            item?.Dispose();
        }
        
        public static void UpdateItem(this KnapsackContainerComponent_Client self, ItemProto itemProto)
        {
            if (!self.Items.TryGetValue(itemProto.Id, out EntityRef<Item> itemRef))
            {
                Log.Error($"itemId : {itemProto.Id} not found");
                return;
            }

            Item item = itemRef;
            item.FromMessage(itemProto);
        }

        public static void Clear(this ET.Client.KnapsackContainerComponent_Client self)
        {
            foreach (EntityRef<Item> itemRef in self.Items.Values)
            {
                Item item = itemRef;
                item?.Dispose();
            }
            self.Items.Clear();
        }
    }
}