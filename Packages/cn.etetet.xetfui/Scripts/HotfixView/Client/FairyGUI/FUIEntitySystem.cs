using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIEntity))]
    [FriendOf(typeof(FUIEntity))]
    public static partial class FUIEntitySystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.FUIEntity self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.FUIEntity self)
        {
            if (self.GComponent != null)
            {
                self.GComponent.Dispose();
                self.GComponent = null;
            }
            
            self.IsUsingStack = false;
        }

        public static void SetRoot(this FUIEntity self, GComponent rootGComponent)
        {
            if (self.GComponent == null)
            {
                Log.Error($"FUIEntity {self.panelInfo.PanelId} GComponent is null!!!");
                return;
            }
            if (rootGComponent == null)
            {
                Log.Error($"FUIEntity {self.panelInfo.PanelId} rootGComponent is null!!!");
                return;
            }
            rootGComponent.AddChild(self.GComponent);
        }
    }
}