/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.Main
{
    [EnableClass]
    public partial class FUI_MainUI: GComponent
    {
        public enum ListStatePage
        {
            hide,
            show,
        }

        public Controller ListState;
        public GList ArchiveList;
        public ET.Client.Main.FUI_Button_PlayGame Button_PlayGame;
        public ET.Client.Main.FUI_Button_PlayGame Button_ArchiveList;
        public ET.Client.Main.FUI_Button_PlayGame Button_ExitGame;
        public const string URL = "ui://y6b7eitgjpf41";

        public static FUI_MainUI CreateInstance()
        {
            return (FUI_MainUI)UIPackage.CreateObject("Main", "MainUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            ListState = GetControllerAt(0);
            ArchiveList = (GList)GetChildAt(1);
            Button_PlayGame = (ET.Client.Main.FUI_Button_PlayGame)GetChildAt(2);
            Button_ArchiveList = (ET.Client.Main.FUI_Button_PlayGame)GetChildAt(3);
            Button_ExitGame = (ET.Client.Main.FUI_Button_PlayGame)GetChildAt(4);
        }
    }
}
