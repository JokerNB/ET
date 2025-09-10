using ET.Client.Common;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.BG, "Common", "BG")]
    public class BG: Entity, IAwake
    {
        private FUI_BG _fuiBG;

        public FUI_BG FUIBG
        {
            get => _fuiBG ??= (FUI_BG)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
