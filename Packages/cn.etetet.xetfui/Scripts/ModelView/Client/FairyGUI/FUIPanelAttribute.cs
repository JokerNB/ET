using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FUIPanelAttribute: BaseAttribute
    {
        public PanelId PanelId
        {
            get;
        }

        public PanelInfo PanelInfo
        {
            get;
        }

        public FUIPanelAttribute(PanelId panelId, UIPanelType uiPanelType, string packageName, string componentName)
        {
            this.PanelId = panelId;
            this.PanelInfo = new PanelInfo
            {
                PanelId = panelId,
                UIPanelType = uiPanelType,
                PackageName = packageName,
                ComponentName = componentName,
            };
        }
    }
}