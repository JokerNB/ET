/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UIMain
{
    [EnableClass]
    public partial class FUI_Button_Bottom: GButton
    {
        public enum BottomTypePage
        {
            Shop,
            Game,
            Auction,
        }

        public Controller BottomType;
        public const string URL = "ui://kjwej5knqj4716";

        public static FUI_Button_Bottom CreateInstance()
        {
            return (FUI_Button_Bottom)UIPackage.CreateObject("UIMain", "Button_Bottom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            BottomType = GetControllerAt(1);
        }
    }
}
