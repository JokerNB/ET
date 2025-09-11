/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.Login
{
    [EnableClass]
    public partial class FUI_LoginUI: GComponent
    {
        public ET.Client.Login.FUI_LoadingProgress LoadingProgress;
        public const string URL = "ui://24sdw5inu1lv0";

        public static FUI_LoginUI CreateInstance()
        {
            return (FUI_LoginUI)UIPackage.CreateObject("Login", "LoginUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            LoadingProgress = (ET.Client.Login.FUI_LoadingProgress)GetChildAt(1);
        }
    }
}
