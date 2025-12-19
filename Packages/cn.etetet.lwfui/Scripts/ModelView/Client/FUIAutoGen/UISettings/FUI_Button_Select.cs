/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UISettings
{
    [EnableClass]
    public partial class FUI_Button_Select: GButton
    {
        public enum SwitchPage
        {
            Off,
            On,
        }

        public Controller Switch;
        public const string URL = "ui://0rrl8mreqj47j";

        public static FUI_Button_Select CreateInstance()
        {
            return (FUI_Button_Select)UIPackage.CreateObject("UISettings", "Button_Select");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Switch = GetControllerAt(1);
        }
    }
}
