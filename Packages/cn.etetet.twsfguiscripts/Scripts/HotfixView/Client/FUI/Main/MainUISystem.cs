namespace ET.Client
{
    [EntitySystemOf(typeof(MainUI))]
    [FriendOf(typeof(MainUI))]
    public static partial class MainUISystem
    {
        [EntitySystem]
        private static void Awake(this MainUI self)
        {
            self.FUIMainUI.Button_PlayGame.onClick.Add(self.OnPlayGameClick);
        }

        public static void OnPlayGameClick(this MainUI self)
        {
            
        }
        
    }
}