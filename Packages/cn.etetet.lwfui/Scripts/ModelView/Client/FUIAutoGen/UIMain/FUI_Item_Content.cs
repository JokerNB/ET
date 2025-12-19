/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UIMain
{
    [EnableClass]
    public partial class FUI_Item_Content: GComponent
    {
        public enum IsNewPage
        {
            False,
            True,
        }

        public Controller ColorType;
        public Controller IsNew;
        public GTextField Text_Content;
        public GTextField Text_New;
        public const string URL = "ui://kjwej5knqj4712";

        public static FUI_Item_Content CreateInstance()
        {
            return (FUI_Item_Content)UIPackage.CreateObject("UIMain", "Item_Content");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            ColorType = GetControllerAt(0);
            IsNew = GetControllerAt(1);
            Text_Content = (GTextField)GetChildAt(7);
            Text_New = (GTextField)GetChildAt(9);
        }
    }
}
