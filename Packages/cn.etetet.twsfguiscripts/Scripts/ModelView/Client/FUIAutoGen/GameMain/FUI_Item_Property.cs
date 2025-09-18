/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_Item_Property: GComponent
    {
        public GTextField Text;
        public const string URL = "ui://xqkzagd9tfszn";

        public static FUI_Item_Property CreateInstance()
        {
            return (FUI_Item_Property)UIPackage.CreateObject("GameMain", "Item_Property");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Text = (GTextField)GetChildAt(2);
        }
    }
}
