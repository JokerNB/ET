namespace ET.Client
{
    [EntitySystemOf(typeof(Item_CoinShop))]
    [FriendOf(typeof(Item_CoinShop))]
    public static partial class Item_CoinShopSystem
    {
        [EntitySystem]
        private static void Awake(this Item_CoinShop self, ET.Client.UICoinShop.FUI_Item_CoinShop fuiItem_CoinShop)
        {
            self.FUIItem_CoinShop = fuiItem_CoinShop;
        }

    }
}