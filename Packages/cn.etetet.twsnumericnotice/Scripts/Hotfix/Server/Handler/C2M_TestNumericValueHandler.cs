namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class C2M_TestNumericValueHandler : MessageLocationHandler<Unit, C2M_TestNumericValue, M2C_TestNumericValue>
    {
        protected override async ETTask Run(Unit unit, C2M_TestNumericValue request, M2C_TestNumericValue response)
        {
            int hp = unit.GetComponent<NumericDataComponent>().GetAsInt(ENumericType.Hp0);
            hp += 10;
            unit.GetComponent<NumericDataComponent>().Set(ENumericType.Hp0, hp);
            
            await ETTask.CompletedTask;
        }
    }
}