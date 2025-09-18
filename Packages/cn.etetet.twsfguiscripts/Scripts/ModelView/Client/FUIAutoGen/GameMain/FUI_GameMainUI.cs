/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_GameMainUI: GComponent
    {
        public ET.Client.GameMain.FUI_Button_Func Button_Func;
        public ET.Client.GameMain.FUI_Item_Property Item_Property;
        public const string URL = "ui://xqkzagd9tfsz1";

        public static FUI_GameMainUI CreateInstance()
        {
            return (FUI_GameMainUI)UIPackage.CreateObject("GameMain", "GameMainUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Button_Func = (ET.Client.GameMain.FUI_Button_Func)GetChildAt(2);
            Item_Property = (ET.Client.GameMain.FUI_Item_Property)GetChildAt(4);
        }
    }
}
