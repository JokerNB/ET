using ET.Client.Main;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.MainUI, UIPanelType.Normal, "Main", "MainUI")]
    public class MainUI : Entity, IAwake
    {
        private FUI_MainUI _fuiMainUI;

        public FUI_MainUI FUIMainUI
        {
            get => _fuiMainUI ??= (FUI_MainUI)this.GetParent<FUIEntity>().GComponent;
        }
    }
}