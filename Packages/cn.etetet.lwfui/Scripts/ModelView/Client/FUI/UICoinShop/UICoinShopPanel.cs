using ET.Client.UICoinShop;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.UICoinShopPanel, "UICoinShop", "UICoinShopPanel")]
    public class UICoinShopPanel: Entity, IAwake
    {
        public Item_CoinShop Item_CoinShop_0 {get; set;}

        public Item_CoinShop Item_CoinShop_1 {get; set;}

        public Item_CoinShop Item_CoinShop_2 {get; set;}

        public Item_CoinShop Item_CoinShop_3 {get; set;}

        public Item_CoinShop Item_CoinShop_4 {get; set;}

        private FUI_UICoinShopPanel _fuiUICoinShopPanel;

        public FUI_UICoinShopPanel FUIUICoinShopPanel
        {
            get => _fuiUICoinShopPanel ??= (FUI_UICoinShopPanel)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
