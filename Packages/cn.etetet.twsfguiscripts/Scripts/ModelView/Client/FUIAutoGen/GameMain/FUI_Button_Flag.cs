/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_Button_Flag: GButton
    {
        public GLoader GLoader_Flag;
        public const string URL = "ui://xqkzagd9tfsz3";

        public static FUI_Button_Flag CreateInstance()
        {
            return (FUI_Button_Flag)UIPackage.CreateObject("GameMain", "Button_Flag");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            GLoader_Flag = (GLoader)GetChildAt(5);
        }
    }
}
