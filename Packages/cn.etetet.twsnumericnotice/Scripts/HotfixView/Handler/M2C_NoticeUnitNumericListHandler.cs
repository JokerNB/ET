namespace ET.Client
{
    [MessageHandler(SceneType.StateSync)]
    public class M2C_NoticeUnitNumericListHandler : MessageHandler<Scene, M2C_NoticeUnitNumericList>
    {
        protected override async ETTask Run(Scene root, M2C_NoticeUnitNumericList message)
        {
            Unit_Client unit = root?.CurrentScene()?.GetComponent<UnitComponent_Client>()?.Get(message.UnitId);
            NumericDataComponent NumericDataComponent = unit.GetComponent<NumericDataComponent>();
            int count = message.NumericTypeList.Count;
            for (int i = 0; i < count; i++)
            {
                int numericType = message.NumericTypeList[i];
                long newValue = message.NewValueList[i];
                NumericDataComponent?.Set(numericType, newValue);
            }

            await ETTask.CompletedTask;
        }
    }
}