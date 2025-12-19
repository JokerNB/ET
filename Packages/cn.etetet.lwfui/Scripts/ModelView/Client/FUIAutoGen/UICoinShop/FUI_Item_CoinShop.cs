/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UICoinShop
{
    [EnableClass]
    public partial class FUI_Item_CoinShop: GComponent
    {
        public Controller ColorType;
        public Controller TopType;
        public GLoader Icon;
        public GTextField Text_Value;
        public GTextField Text_Tittle;
        public ET.Client.UICoinShop.FUI_Button_Item_CoinShop Button_Item_CoinShop;
        public GTextField Text_Top;
        public const string URL = "ui://0mie2hotqj47j";

        public static FUI_Item_CoinShop CreateInstance()
        {
            return (FUI_Item_CoinShop)UIPackage.CreateObject("UICoinShop", "Item_CoinShop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            ColorType = GetControllerAt(0);
            TopType = GetControllerAt(1);
            Icon = (GLoader)GetChildAt(6);
            Text_Value = (GTextField)GetChildAt(7);
            Text_Tittle = (GTextField)GetChildAt(8);
            Button_Item_CoinShop = (ET.Client.UICoinShop.FUI_Button_Item_CoinShop)GetChildAt(9);
            Text_Top = (GTextField)GetChildAt(11);
        }
    }
}
