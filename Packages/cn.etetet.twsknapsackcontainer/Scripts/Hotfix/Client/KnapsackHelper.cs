namespace ET.Client
{
    public static class KnapsackHelper
    {
        public static async ETTask<int> GetAllItems(Scene root)
        {
            C2M_GetAllKnapsack c2MGetAllKnapsack = C2M_GetAllKnapsack.Create();

            M2C_GetAllKnapsack m2CGetAllKnapsack = await root.GetComponent<ClientSenderComponent>().Call(c2MGetAllKnapsack) as M2C_GetAllKnapsack;
            if (m2CGetAllKnapsack.Error != ErrorCode.ERR_Success)
            {
                return m2CGetAllKnapsack.Error;
            }

            root.GetComponent<KnapsackComponent_Client>().ClearAllItems();

            foreach (ItemProto itemProto in m2CGetAllKnapsack.ItemList)
            {
                root.GetComponent<KnapsackComponent_Client>().GetContainer(itemProto.ContainerTyper).AddItemFromMessage(itemProto);
            }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> RequestAddItem(Scene root, KnapsackContainerType containerType, int configId)
        {
            C2M_AddKnapsackItem request = C2M_AddKnapsackItem.Create();
            request.ContainerType = (int)containerType;
            request.ConfigId = configId;
            M2C_AddKnapsackItem respone = await root.GetComponent<ClientSenderComponent>().Call(request) as M2C_AddKnapsackItem;
            return respone.Error;
        }
        
        public static async ETTask<int> RequestRemoveItem(Scene root, KnapsackContainerType containerType, int configId)
        {
            C2M_RemoveKnapsackItem request = C2M_RemoveKnapsackItem.Create();
            request.ContainerType = (int)containerType;
            request.ConfigId = configId;
            M2C_RemoveKnapsackItem respone = await root.GetComponent<ClientSenderComponent>().Call(request) as M2C_RemoveKnapsackItem;
            return respone.Error;
        }
    }
}