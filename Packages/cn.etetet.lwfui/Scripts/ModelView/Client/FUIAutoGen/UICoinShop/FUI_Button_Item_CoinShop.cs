/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UICoinShop
{
    [EnableClass]
    public partial class FUI_Button_Item_CoinShop: GButton
    {
        public GTextField tittle;
        public const string URL = "ui://0mie2hotqj47k";

        public static FUI_Button_Item_CoinShop CreateInstance()
        {
            return (FUI_Button_Item_CoinShop)UIPackage.CreateObject("UICoinShop", "Button_Item_CoinShop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            tittle = (GTextField)GetChildAt(4);
        }
    }
}
