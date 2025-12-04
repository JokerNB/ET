using ET.Client.Loading;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.UILoading, "Loading", "UILoading")]
    public class UILoading: Entity, IAwake
    {
        private FUI_UILoading _fuiUILoading;

        public FUI_UILoading FUIUILoading
        {
            get => _fuiUILoading ??= (FUI_UILoading)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
