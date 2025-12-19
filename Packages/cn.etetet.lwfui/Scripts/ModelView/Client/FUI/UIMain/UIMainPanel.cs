using ET.Client.UIMain;

namespace ET.Client
{
    [ComponentOf(typeof(FUIEntity))]
    [FUIPanel(PanelId.UIMainPanel, "UIMain", "UIMainPanel")]
    public class UIMainPanel: Entity, IAwake
    {
        public Item_Content Item_Content_0 {get; set;}

        public Item_Content Item_Content_1 {get; set;}

        public Item_Content Item_Content_2 {get; set;}

        public Item_Content Item_Content_3 {get; set;}

        public Item_Content Item_Content_4 {get; set;}

        public Item_Content Item_Content_5 {get; set;}

        public Bottom Bottom {get; set;}

        private FUI_UIMainPanel _fuiUIMainPanel;

        public FUI_UIMainPanel FUIUIMainPanel
        {
            get => _fuiUIMainPanel ??= (FUI_UIMainPanel)this.GetParent<FUIEntity>().GComponent;
        }
    }
}
