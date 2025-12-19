/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UISettings
{
    [EnableClass]
    public partial class FUI_Item_Settings: GComponent
    {
        public Controller SelectionType;
        public const string URL = "ui://0rrl8mreqj47h";

        public static FUI_Item_Settings CreateInstance()
        {
            return (FUI_Item_Settings)UIPackage.CreateObject("UISettings", "Item_Settings");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            SelectionType = GetControllerAt(0);
        }
    }
}
