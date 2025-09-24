/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_Button_BuildingItem: GButton
    {
        public GTextField Text_Desc;
        public GTextField Text_Price;
        public const string URL = "ui://xqkzagd9sb3t17";

        public static FUI_Button_BuildingItem CreateInstance()
        {
            return (FUI_Button_BuildingItem)UIPackage.CreateObject("GameMain", "Button_BuildingItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Text_Desc = (GTextField)GetChildAt(3);
            Text_Price = (GTextField)GetChildAt(4);
        }
    }
}
