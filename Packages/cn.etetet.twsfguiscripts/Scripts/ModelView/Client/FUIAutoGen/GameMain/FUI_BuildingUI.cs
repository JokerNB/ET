/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_BuildingUI: GComponent
    {
        public GList BuildingItemList;
        public GTextField Text_Title;
        public const string URL = "ui://xqkzagd9tfszr";

        public static FUI_BuildingUI CreateInstance()
        {
            return (FUI_BuildingUI)UIPackage.CreateObject("GameMain", "BuildingUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            BuildingItemList = (GList)GetChildAt(1);
            Text_Title = (GTextField)GetChildAt(2);
        }
    }
}
