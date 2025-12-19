/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Client.UIMain
{
    [EnableClass]
    public partial class FUI_UIMainPanel: GComponent
    {
        public ET.Client.UIMain.FUI_Button_Settings Button_Settings;
        public GTextField Text_Energy;
        public GTextField Text_Coin;
        public ET.Client.UIMain.FUI_Button_Add Button_Add;
        public ET.Client.UIMain.FUI_Button_LanguageSwitcher Button_LanguageSwitcher;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_0;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_1;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_2;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_3;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_4;
        public ET.Client.UIMain.FUI_Item_Content Item_Content_5;
        public ET.Client.UIMain.FUI_Bottom Bottom;
        public const string URL = "ui://kjwej5knkp3z0";

        public static FUI_UIMainPanel CreateInstance()
        {
            return (FUI_UIMainPanel)UIPackage.CreateObject("UIMain", "UIMainPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            Button_Settings = (ET.Client.UIMain.FUI_Button_Settings)GetChildAt(1);
            Text_Energy = (GTextField)GetChildAt(4);
            Text_Coin = (GTextField)GetChildAt(7);
            Button_Add = (ET.Client.UIMain.FUI_Button_Add)GetChildAt(8);
            Button_LanguageSwitcher = (ET.Client.UIMain.FUI_Button_LanguageSwitcher)GetChildAt(9);
            Item_Content_0 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(10);
            Item_Content_1 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(11);
            Item_Content_2 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(12);
            Item_Content_3 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(13);
            Item_Content_4 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(14);
            Item_Content_5 = (ET.Client.UIMain.FUI_Item_Content)GetChildAt(15);
            Bottom = (ET.Client.UIMain.FUI_Bottom)GetChildAt(16);
        }
    }
}
