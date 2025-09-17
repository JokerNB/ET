namespace ET.Client
{
    [EntitySystemOf(typeof(LoginUI))]
    [FriendOf(typeof(LoginUI))]
    public static partial class LoginUISystem
    {
        [EntitySystem]
        private static void Awake(this LoginUI self)
        {
            self.SetValue().NoContext();
        }

        public static async ETTask SetValue(this LoginUI self)
        {
            int num = 2;
            var list = self.FUILoginUI.LoadingProgress.ItemList;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            list.numItems = num;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            list.numItems = num + 2;

            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            var playerComponent = self.Root().GetComponent<PlayerComponent>();
            await LoginHelper.Login(self.Root(), globalComponent.GlobalConfig.Address, playerComponent.Account, playerComponent.Password);
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            list.numItems = num + 4;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            list.numItems = num + 6;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            list.numItems = num + 12;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            await self.Root().GetComponent<FUIComponent>().ShowPanelAsync<MainUI>();
            self.Root().GetComponent<FUIComponent>().ClosePanel<LoginUI>();
        }
    }
}