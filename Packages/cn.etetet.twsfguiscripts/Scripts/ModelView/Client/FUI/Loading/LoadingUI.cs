using ET.Client.Loading;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.LoadingUI, UIPanelType.Other, "Loading", "LoadingUI")]
    public class LoadingUI : Entity, IAwake, IShow<bool>
    {
        private FUI_LoadingUI _fuiLoadingUI;

        public FUI_LoadingUI FUILoadingUI
        {
            get => _fuiLoadingUI ??= (FUI_LoadingUI)this.GetParent<FUIEntity>().GComponent;
        }
    }
}