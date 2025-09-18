namespace ET.Client
{
    [EntitySystemOf(typeof(LoadingUI))]
    [FriendOf(typeof(LoadingUI))]
    public static partial class LoadingUISystem
    {
        [EntitySystem]
        private static void Awake(this LoadingUI self)
        {
        }

        [EntitySystem]
        private static void Show(this LoadingUI self, bool isAutoLogin)
        {
            if (isAutoLogin)
                self.AutoLogin().NoContext();
            else
            {
                self.FUILoadingUI.LoadingProgress.ItemList.numItems = 0;
            }
        }

        public static async ETTask AutoLogin(this LoadingUI self)
        {
            int num = 2;
            var list = self.FUILoadingUI.LoadingProgress.ItemList;
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
            self.Root().GetComponent<FUIComponent>().HidePanel<LoadingUI>();
        }

        public static void SetValue(this LoadingUI self, int value, bool isComplete = false)
        {
            if (value is < 0 or > 14)
            {
                Log.Error($"SetValue Error : {value}");
                return;
            }

            var list = self.FUILoadingUI.LoadingProgress.ItemList;
            list.numItems = value;
            if (isComplete)
            {
                self.Complete().NoContext();
            }
        }

        public static async ETTask Complete(this LoadingUI self)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            var list = self.FUILoadingUI.LoadingProgress.ItemList;
            list.numItems = 14;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(500);
            await self.Root().GetComponent<FUIComponent>().ShowPanelAsync<GameMainUI>();
            self.Root().GetComponent<FUIComponent>().HidePanel<LoadingUI>();
        }
    }
}