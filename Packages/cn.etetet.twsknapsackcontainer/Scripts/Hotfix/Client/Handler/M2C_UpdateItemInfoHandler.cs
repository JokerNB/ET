namespace ET.Client
{
    [MessageHandler(SceneType.StateSync)]
    public class M2C_UpdateItemInfoHandler : MessageHandler<Scene, M2C_UpdateItemInfo>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateItemInfo message)
        {
            KnapsackContainerComponent_Client container = root.GetComponent<KnapsackComponent_Client>().GetContainer(message.ItemInfo.ContainerTyper);
            if (message.Op == (int)ItemOpType.Add)
                container?.AddItemFromMessage(message.ItemInfo);
            else if (message.Op == (int)ItemOpType.Remove)
                container?.RemoveItemById(message.ItemInfo.Id);
            else if (message.Op == (int)ItemOpType.Update)
                container?.UpdateItem(message.ItemInfo);

            EventSystem.Instance.Publish(root, new ItemInfoChange()
            {
                itemProto = message.ItemInfo,
            });

            await ETTask.CompletedTask;
        }
    }
}