namespace ET.Client
{
    [ChildOf(typeof(BuffTempComponent))]
    public class BuffCreateInfo : Entity, IAwake<int>, IDestroy
    {
        public int ConfigId;
        //此处可以放：施法者，状态来源，关联技能等等辅助变量
    }
}