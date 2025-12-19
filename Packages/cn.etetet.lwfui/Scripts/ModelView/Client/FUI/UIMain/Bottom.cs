using ET.Client.UIMain;

namespace ET.Client
{
    [ChildOf]
    public class Bottom: Entity, IAwake<FUI_Bottom>
    {
        public FUI_Bottom FUIBottom { get; set; }
    }
}
