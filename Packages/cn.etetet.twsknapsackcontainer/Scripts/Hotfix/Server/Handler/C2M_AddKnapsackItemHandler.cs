namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class C2M_AddKnapsackItemHandler : MessageLocationHandler<Unit, C2M_AddKnapsackItem, M2C_AddKnapsackItem>
    {
        protected override async ETTask Run(Unit unit, C2M_AddKnapsackItem request, M2C_AddKnapsackItem response)
        {
            KnapsackContainerComponent containerType = unit.GetComponent<KnapsackComponent>().GetContainer(request.ContainerType);
            Item item = ItemFactory.CreateItem(containerType, request.ConfigId);
            containerType.AddItem(item);
            await ETTask.CompletedTask;
        }
    }
}