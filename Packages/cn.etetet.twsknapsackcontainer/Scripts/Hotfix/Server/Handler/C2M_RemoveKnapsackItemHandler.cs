namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class C2M_RemoveKnapsackItemHandler : MessageLocationHandler<Unit, C2M_RemoveKnapsackItem, M2C_RemoveKnapsackItem>
    {
        protected override async ETTask Run(Unit unit, C2M_RemoveKnapsackItem request, M2C_RemoveKnapsackItem response)
        {
            KnapsackContainerComponent containerType = unit.GetComponent<KnapsackComponent>().GetContainer(request.ContainerType);
            if (!containerType.RemoveItem(request.ConfigId))
                response.Error = ErrorCode.ERR_RemoveItemError;
            await ETTask.CompletedTask;
        }
    }
}