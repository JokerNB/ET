namespace ET.Client
{
    [EntitySystemOf(typeof(UICoinShopPanel))]
    [FriendOf(typeof(UICoinShopPanel))]
    public static partial class UICoinShopPanelSystem
    {
        [EntitySystem]
        private static void Awake(this UICoinShopPanel self)
        {
            self.Item_CoinShop_0 = self.AddChild<Item_CoinShop, ET.Client.UICoinShop.FUI_Item_CoinShop>(self.FUIUICoinShopPanel.Item_CoinShop_0, true);
            self.Item_CoinShop_1 = self.AddChild<Item_CoinShop, ET.Client.UICoinShop.FUI_Item_CoinShop>(self.FUIUICoinShopPanel.Item_CoinShop_1, true);
            self.Item_CoinShop_2 = self.AddChild<Item_CoinShop, ET.Client.UICoinShop.FUI_Item_CoinShop>(self.FUIUICoinShopPanel.Item_CoinShop_2, true);
            self.Item_CoinShop_3 = self.AddChild<Item_CoinShop, ET.Client.UICoinShop.FUI_Item_CoinShop>(self.FUIUICoinShopPanel.Item_CoinShop_3, true);
            self.Item_CoinShop_4 = self.AddChild<Item_CoinShop, ET.Client.UICoinShop.FUI_Item_CoinShop>(self.FUIUICoinShopPanel.Item_CoinShop_4, true);
            
            self.FUIUICoinShopPanel.Button_Close.onClick.Add(self.OnClick);
        }

        public static void OnClick(this UICoinShopPanel self)
        {
            self.Root().GetComponent<FUIComponent>().ClosePanel<UICoinShopPanel>();
        }

    }
}