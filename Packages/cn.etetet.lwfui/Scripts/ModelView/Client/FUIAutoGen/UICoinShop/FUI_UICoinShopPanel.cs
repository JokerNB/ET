/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UICoinShop
{
    [EnableClass]
    public partial class FUI_UICoinShopPanel: GComponent
    {
        public ET.Client.UICommon.FUI_Button_Close Button_Close;
        public GTextField Text_Tittle;
        public GTextField Text_Head;
        public ET.Client.UICoinShop.FUI_Item_CoinShop Item_CoinShop_0;
        public ET.Client.UICoinShop.FUI_Item_CoinShop Item_CoinShop_1;
        public ET.Client.UICoinShop.FUI_Item_CoinShop Item_CoinShop_2;
        public ET.Client.UICoinShop.FUI_Item_CoinShop Item_CoinShop_3;
        public ET.Client.UICoinShop.FUI_Item_CoinShop Item_CoinShop_4;
        public const string URL = "ui://0mie2hotqj47g";

        public static FUI_UICoinShopPanel CreateInstance()
        {
            return (FUI_UICoinShopPanel)UIPackage.CreateObject("UICoinShop", "UICoinShopPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Button_Close = (ET.Client.UICommon.FUI_Button_Close)GetChildAt(2);
            Text_Tittle = (GTextField)GetChildAt(4);
            Text_Head = (GTextField)GetChildAt(7);
            Item_CoinShop_0 = (ET.Client.UICoinShop.FUI_Item_CoinShop)GetChildAt(8);
            Item_CoinShop_1 = (ET.Client.UICoinShop.FUI_Item_CoinShop)GetChildAt(9);
            Item_CoinShop_2 = (ET.Client.UICoinShop.FUI_Item_CoinShop)GetChildAt(10);
            Item_CoinShop_3 = (ET.Client.UICoinShop.FUI_Item_CoinShop)GetChildAt(11);
            Item_CoinShop_4 = (ET.Client.UICoinShop.FUI_Item_CoinShop)GetChildAt(12);
        }
    }
}
