/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UISettings
{
    [EnableClass]
    public partial class FUI_UISettingsPanel: GComponent
    {
        public ET.Client.UICommon.FUI_Button_Close Button_Close;
        public GTextField Text_Tittle;
        public GTextField Text_AUDIO;
        public ET.Client.UISettings.FUI_Item_Settings Item_Settings_0;
        public ET.Client.UISettings.FUI_Item_Settings Item_Settings_1;
        public GTextField Text_NOTIFICATIONS;
        public ET.Client.UISettings.FUI_Item_Settings Item_Settings_2;
        public ET.Client.UISettings.FUI_Item_Settings Item_Settings_3;
        public GTextField Text_GENERAL;
        public ET.Client.UISettings.FUI_Item_General Item_General_0;
        public ET.Client.UISettings.FUI_Item_General Item_General_1;
        public ET.Client.UISettings.FUI_Item_General Item_General_2;
        public ET.Client.UISettings.FUI_Button_LogOut Button_LogOut;
        public const string URL = "ui://0rrl8mreqj47g";

        public static FUI_UISettingsPanel CreateInstance()
        {
            return (FUI_UISettingsPanel)UIPackage.CreateObject("UISettings", "UISettingsPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Button_Close = (ET.Client.UICommon.FUI_Button_Close)GetChildAt(2);
            Text_Tittle = (GTextField)GetChildAt(3);
            Text_AUDIO = (GTextField)GetChildAt(4);
            Item_Settings_0 = (ET.Client.UISettings.FUI_Item_Settings)GetChildAt(5);
            Item_Settings_1 = (ET.Client.UISettings.FUI_Item_Settings)GetChildAt(6);
            Text_NOTIFICATIONS = (GTextField)GetChildAt(7);
            Item_Settings_2 = (ET.Client.UISettings.FUI_Item_Settings)GetChildAt(8);
            Item_Settings_3 = (ET.Client.UISettings.FUI_Item_Settings)GetChildAt(9);
            Text_GENERAL = (GTextField)GetChildAt(10);
            Item_General_0 = (ET.Client.UISettings.FUI_Item_General)GetChildAt(11);
            Item_General_1 = (ET.Client.UISettings.FUI_Item_General)GetChildAt(12);
            Item_General_2 = (ET.Client.UISettings.FUI_Item_General)GetChildAt(13);
            Button_LogOut = (ET.Client.UISettings.FUI_Button_LogOut)GetChildAt(14);
        }
    }
}
