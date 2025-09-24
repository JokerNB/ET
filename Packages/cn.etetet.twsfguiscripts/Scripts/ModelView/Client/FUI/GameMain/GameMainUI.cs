using ET.Client.GameMain;
using FairyGUI;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.GameMainUI, UIPanelType.Normal, "GameMain", "GameMainUI")]
    public class GameMainUI: Entity, IAwake
    {
        private FUI_GameMainUI _fuiGameMainUI;

        public FUI_GameMainUI FUIGameMainUI
        {
            get => _fuiGameMainUI ??= (FUI_GameMainUI)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
