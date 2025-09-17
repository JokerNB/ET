namespace ET.Client
{
    [EntitySystemOf(typeof(MainUI))]
    [FriendOf(typeof(MainUI))]
    public static partial class MainUISystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MainUI self)
        {
        }

        public static void OnPlayGameClick(this MainUI self)
        {
            self.LoginGame().NoContext();
        }

        public static async ETTask LoginGame(this MainUI self)
        {
            var clientSenderComponent = self.Root().GetComponent<ClientSenderComponent>();
            var playerComponent = self.Root().GetComponent<PlayerComponent>();
            string account = playerComponent.Account;
            long key = playerComponent.Key;
            string address = playerComponent.Address;
            //请求角色进入Map地图
            NetClient2Main_LoginGame netClient2MainLoginGame = await clientSenderComponent.LoginGameAsync(account, key, address);
            if (netClient2MainLoginGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"进入游戏失败: {netClient2MainLoginGame.Error}");
                return;
            }

            Log.Debug("进入游戏成功！");
            playerComponent.MyId = netClient2MainLoginGame.PlayerId;
        }
    }
}