using ET.Client.Login;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.LoginUI, "Login", "LoginUI")]
    public class LoginUI : Entity, IAwake
    {
        private FUI_LoginUI _fuiLoginUI;

        public FUI_LoginUI FUILoginUI
        {
            get => _fuiLoginUI ??= (FUI_LoginUI)this.GetParent<FUIEntity>().GComponent;
        }
    }
}