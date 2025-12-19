using ET.Client.UIMain;

namespace ET.Client
{
    [ChildOf]
    public class Item_Content: Entity, IAwake<FUI_Item_Content>
    {
        public FUI_Item_Content FUIItem_Content { get; set; }
    }
}
