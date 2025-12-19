namespace ET.Client
{
    [EntitySystemOf(typeof(UISettingsPanel))]
    [FriendOf(typeof(UISettingsPanel))]
    public static partial class UISettingsPanelSystem
    {
        [EntitySystem]
        private static void Awake(this UISettingsPanel self)
        {
            self.FUIUISettingsPanel.Button_Close.onClick.Add(self.OnClick);
        }

        public static void OnClick(this UISettingsPanel self)
        {
            self.Root().GetComponent<FUIComponent>().ClosePanel<UISettingsPanel>();
        }
    }
}