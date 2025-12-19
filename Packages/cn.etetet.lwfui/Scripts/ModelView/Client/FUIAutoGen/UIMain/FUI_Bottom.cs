/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UIMain
{
    [EnableClass]
    public partial class FUI_Bottom: GComponent
    {
        public ET.Client.UIMain.FUI_Button_Bottom Button_Bottom_Shop;
        public ET.Client.UIMain.FUI_Button_Bottom Button_Bottom_Game;
        public ET.Client.UIMain.FUI_Button_Bottom Button_Bottom_Auction;
        public const string URL = "ui://kjwej5knqj4714";

        public static FUI_Bottom CreateInstance()
        {
            return (FUI_Bottom)UIPackage.CreateObject("UIMain", "Bottom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Button_Bottom_Shop = (ET.Client.UIMain.FUI_Button_Bottom)GetChildAt(1);
            Button_Bottom_Game = (ET.Client.UIMain.FUI_Button_Bottom)GetChildAt(2);
            Button_Bottom_Auction = (ET.Client.UIMain.FUI_Button_Bottom)GetChildAt(3);
        }
    }
}
