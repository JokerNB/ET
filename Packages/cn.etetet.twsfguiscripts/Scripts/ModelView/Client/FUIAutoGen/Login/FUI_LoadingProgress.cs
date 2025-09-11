/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.Login
{
    [EnableClass]
    public partial class FUI_LoadingProgress: GComponent
    {
        public GList ItemList;
        public const string URL = "ui://24sdw5inu1lv8";

        public static FUI_LoadingProgress CreateInstance()
        {
            return (FUI_LoadingProgress)UIPackage.CreateObject("Login", "LoadingProgress");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            ItemList = (GList)GetChildAt(1);
        }
    }
}
