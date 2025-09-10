namespace ET.Server
{
    [EntitySystemOf(typeof(KnapsackComponent))]
    public static partial class KnapsackComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.KnapsackComponent self)
        {
            KnapsackContainerComponent Inventory = self.AddChild<KnapsackContainerComponent, int>((int)KnapsackContainerType.Inventory);
            self.ContainersInfoDic.Add((int)KnapsackContainerType.Inventory, Inventory);

            KnapsackContainerComponent Warehouse = self.AddChild<KnapsackContainerComponent, int>((int)KnapsackContainerType.WareHouse);
            self.ContainersInfoDic.Add((int)KnapsackContainerType.WareHouse, Warehouse);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.KnapsackComponent self)
        {
            self.ContainersInfoDic.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this ET.Server.KnapsackComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is KnapsackContainerComponent KnapsackContainer)
                    self.ContainersInfoDic.Add(KnapsackContainer.KnapsackContainerType, KnapsackContainer);
            }
        }

        public static void GetAllItems(this ET.Server.KnapsackComponent self,ListComponent<Item> itemList)
        {
            foreach (KnapsackContainerComponent container in self.ContainersInfoDic.Values)
            {
                container.GetItems(itemList);
            }
        }

        public static KnapsackContainerComponent GetContainer(this ET.Server.KnapsackComponent self, int containerType)
        {
            self.ContainersInfoDic.TryGetValue(containerType, out EntityRef<KnapsackContainerComponent> KnapsackContainer);
            return KnapsackContainer;
        }
    }
}