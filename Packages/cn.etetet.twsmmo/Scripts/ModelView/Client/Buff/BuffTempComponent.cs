namespace ET.Client
{
    //用于创建和管理BuffCreateInfo
    [ComponentOf(typeof(BuffComponent))]
    public class BuffTempComponent : Entity, IAwake, IDestroy
    {
    }
}