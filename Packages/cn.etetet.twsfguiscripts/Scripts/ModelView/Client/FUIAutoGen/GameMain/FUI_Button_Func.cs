/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.GameMain
{
    [EnableClass]
    public partial class FUI_Button_Func: GButton
    {
        public enum TypePage
        {
            Normal,
            TextProgress,
            Progress,
        }

        public Controller Type;
        public GTextField Text_Progress;
        public GProgressBar Progress;
        public const string URL = "ui://xqkzagd9tfszh";

        public static FUI_Button_Func CreateInstance()
        {
            return (FUI_Button_Func)UIPackage.CreateObject("GameMain", "Button_Func");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Type = GetControllerAt(1);
            Text_Progress = (GTextField)GetChildAt(7);
            Progress = (GProgressBar)GetChildAt(8);
        }
    }
}
