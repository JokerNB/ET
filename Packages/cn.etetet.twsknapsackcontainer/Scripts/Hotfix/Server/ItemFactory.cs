namespace ET.Server
{
    public static class ItemFactory
    {
        public static Item CreateItem(KnapsackContainerComponent container, int configId)
        {
            if (ItemConfigCategory.Instance.GetOrDefault(configId) == null)
            {
                Log.Error($"当前所创建的物品ID不存在: {configId}");
                return null;
            }

            Item item = container.AddChild<Item, int>(configId);
            item.ContainerType = container.KnapsackContainerType;
            return item;
        }
    }
}