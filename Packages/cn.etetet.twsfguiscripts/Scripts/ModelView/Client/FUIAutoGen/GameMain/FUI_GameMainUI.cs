/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_GameMainUI: GComponent
    {
        public enum BuildingUIStatePage
        {
            不显示,
            显示,
        }

        public Controller BuildingUIState;
        public ET.Client.GameMain.FUI_Button_Flag Button_Flag;
        public GList FuncList;
        public ET.Client.GameMain.FUI_Item_Property Item_Property;
        public ET.Client.GameMain.FUI_BuildingUI BuildingUI;
        public const string URL = "ui://xqkzagd9tfsz1";

        public static FUI_GameMainUI CreateInstance()
        {
            return (FUI_GameMainUI)UIPackage.CreateObject("GameMain", "GameMainUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            BuildingUIState = GetControllerAt(0);
            Button_Flag = (ET.Client.GameMain.FUI_Button_Flag)GetChildAt(1);
            FuncList = (GList)GetChildAt(2);
            Item_Property = (ET.Client.GameMain.FUI_Item_Property)GetChildAt(3);
            BuildingUI = (ET.Client.GameMain.FUI_BuildingUI)GetChildAt(5);
        }
    }
}
