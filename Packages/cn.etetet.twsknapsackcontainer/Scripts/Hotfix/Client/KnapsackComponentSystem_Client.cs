namespace ET.Client
{
    [EntitySystemOf(typeof(KnapsackComponent_Client))]
    public static partial class KnapsackComponentSystem_Client
    {
        [EntitySystem]
        private static void Awake(this ET.Client.KnapsackComponent_Client self)
        {
            KnapsackContainerComponent_Client Inventory = self.AddChild<KnapsackContainerComponent_Client, int>((int)KnapsackContainerType.Inventory);
            self.ContainerInfoDic.Add((int)KnapsackContainerType.Inventory, Inventory);

            KnapsackContainerComponent_Client Warehouse = self.AddChild<KnapsackContainerComponent_Client, int>((int)KnapsackContainerType.WareHouse);
            self.ContainerInfoDic.Add((int)KnapsackContainerType.WareHouse, Warehouse);
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.KnapsackComponent_Client self)
        {
            self.ContainerInfoDic.Clear();
        }
        
        public static KnapsackContainerComponent_Client GetContainer(this KnapsackComponent_Client self, int containerType)
        {
            if(!self.ContainerInfoDic.TryGetValue(containerType, out EntityRef<KnapsackContainerComponent_Client> KnapsackContainer))
            {
                Log.Error($"container not found type: {containerType}");
            }
            return KnapsackContainer;
        }

        public static void ClearAllItems(this KnapsackComponent_Client self)
        {
            foreach (EntityRef<KnapsackContainerComponent_Client> containerRef in self.ContainerInfoDic.Values)
            {
                KnapsackContainerComponent_Client container = containerRef;
                container.Clear();
            }
        }
    }
}