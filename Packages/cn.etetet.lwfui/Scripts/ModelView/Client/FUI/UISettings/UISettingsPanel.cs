using ET.Client.UISettings;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.UISettingsPanel, "UISettings", "UISettingsPanel")]
    public class UISettingsPanel: Entity, IAwake
    {
        private FUI_UISettingsPanel _fuiUISettingsPanel;

        public FUI_UISettingsPanel FUIUISettingsPanel
        {
            get => _fuiUISettingsPanel ??= (FUI_UISettingsPanel)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
