namespace ET.Client
{
    [MessageHandler(SceneType.StateSync)]
    public class M2C_NoticeUnitNumericHandler : MessageHandler<Scene, M2C_NoticeUnitNumeric>
    {
        protected override async ETTask Run(Scene root, M2C_NoticeUnitNumeric message)
        {
            root?.CurrentScene()?.GetComponent<UnitComponent_Client>()?.Get(message.UnitId)?.GetComponent<NumericDataComponent>()
                    ?.Set(message.NumericType, message.NewValue);
            await ETTask.CompletedTask;
        }
    }
}