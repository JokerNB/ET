/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.Loading
{
    [EnableClass]
    public partial class FUI_LoadingUI: GComponent
    {
        public ET.Client.Loading.FUI_LoadingProgress LoadingProgress;
        public const string URL = "ui://24sdw5inu1lv0";

        public static FUI_LoadingUI CreateInstance()
        {
            return (FUI_LoadingUI)UIPackage.CreateObject("Loading", "LoadingUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            LoadingProgress = (ET.Client.Loading.FUI_LoadingProgress)GetChildAt(1);
        }
    }
}
