using FairyGUI;

namespace ET.Client
{
    public static class FUIRootHelper
    {
        public static GComponent GetTargetRoot(Scene scene, UIPanelType panelType)
        {
            return scene.GetComponent<FUIComponent>().GetTargetRoot(panelType);
        }
    }
}