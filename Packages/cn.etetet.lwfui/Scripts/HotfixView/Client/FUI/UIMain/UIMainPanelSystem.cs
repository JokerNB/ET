namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainPanel))]
    [FriendOf(typeof(UIMainPanel))]
    public static partial class UIMainPanelSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainPanel self)
        {
            self.Item_Content_0 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_0, true);
            self.Item_Content_1 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_1, true);
            self.Item_Content_2 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_2, true);
            self.Item_Content_3 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_3, true);
            self.Item_Content_4 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_4, true);
            self.Item_Content_5 = self.AddChild<Item_Content, ET.Client.UIMain.FUI_Item_Content>(self.FUIUIMainPanel.Item_Content_5, true);
            self.Bottom = self.AddChild<Bottom, ET.Client.UIMain.FUI_Bottom>(self.FUIUIMainPanel.Bottom, true);
            
            self.FUIUIMainPanel.Button_Settings.onClick.Add(self.OnClick);
            self.FUIUIMainPanel.Button_Add.onClick.Add(self.OnClickAdd);
        }

        public static void OnClick(this UIMainPanel self)
        {
            self.Root().GetComponent<FUIComponent>().ShowPanelAsync<UISettingsPanel>().NoContext();
        }
        
        public static void OnClickAdd(this UIMainPanel self)
        {
            self.Root().GetComponent<FUIComponent>().ShowPanelAsync<UICoinShopPanel>().NoContext();
        }
    }
}