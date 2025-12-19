using ET.Client.UICoinShop;

namespace ET.Client
{
    [ChildOf]
    public class Item_CoinShop: Entity, IAwake<FUI_Item_CoinShop>
    {
        public FUI_Item_CoinShop FUIItem_CoinShop { get; set; }
    }
}
