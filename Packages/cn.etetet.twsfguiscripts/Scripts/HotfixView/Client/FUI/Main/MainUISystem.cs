using ET.Client.Main;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(MainUI))]
    [FriendOf(typeof(MainUI))]
    [FriendOfAttribute(typeof(ET.Client.ArchiveInfoManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.ArchiveInfo))]
    public static partial class MainUISystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MainUI self)
        {
            self.FUIMainUI.Button_PlayGame.onClick.Add(self.OnPlayGameClick);
            self.FUIMainUI.Button_ArchiveList.onClick.Add(self.OnArchiveListClick);
            self.FUIMainUI.Button_ExitGame.onClick.Add(self.OnExitGameClick);

            self.InitArchiveList();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MainUI self)
        {
            self.curArchiveNum = -1;
        }

        public static void InitArchiveList(this ET.Client.MainUI self)
        {
            //默认关闭
            self.FUIMainUI.ListState.selectedIndex = 0;
            var archiveInfoManagerComponentClient = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>();
            self.FUIMainUI.ArchiveList.itemRenderer = self.RenderListItem;
            self.FUIMainUI.ArchiveList.onClickItem.Add(self.ClickItem);
            self.FUIMainUI.ArchiveList.numItems = archiveInfoManagerComponentClient.ArchiveInfos.Count;
        }

        public static void RenderListItem(this ET.Client.MainUI self, int index, GObject item)
        {
            var archiveInfoManagerComponentClient = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>();
            ArchiveInfo archiveInfo = archiveInfoManagerComponentClient.ArchiveInfos[index];
            FUI_Button_PlayGame button = item as FUI_Button_PlayGame;
            button.title = archiveInfo.ArchiveNumber.ToString();
        }

        public static void ClickItem(this ET.Client.MainUI self, EventContext context)
        {
            //选中存档
            var index = self.FUIMainUI.ArchiveList.GetChildIndex(context.data as GButton);
            var archiveInfoManagerComponentClient = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>();
            ArchiveInfo archiveInfo = archiveInfoManagerComponentClient.ArchiveInfos[index];
            self.curArchiveNum = archiveInfo.ArchiveNumber;
            //登录游戏
            self.LoginGame().NoContext();
        }

        public static void OnPlayGameClick(this MainUI self)
        {
            self.LoginGame().NoContext();
        }

        public static void OnArchiveListClick(this MainUI self)
        {
            self.FUIMainUI.ListState.selectedIndex = 1 - self.FUIMainUI.ListState.selectedIndex;
        }

        public static void OnExitGameClick(this MainUI self)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static async ETTask<int> OperaArchive(this ET.Client.MainUI self)
        {
            var playerComponent = self.Root().GetComponent<PlayerComponent>();

            C2G_SelectOrAddArchiveRequest req = C2G_SelectOrAddArchiveRequest.Create();
            req.AccountName = playerComponent.Account;
            req.ArchiveNum = self.curArchiveNum;

            var response = await self.Root().GetComponent<ClientSenderComponent>().Call(req) as C2G_SelectOrAddArchiveResponse;
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            self.curArchiveNum = response.ArchiveNum;
            self.Root().GetComponent<ArchiveInfoManagerComponent_Client>().SetCurArchiveInfoByArchiveNum(self.curArchiveNum);
            return ErrorCode.ERR_Success;
        }

        public static async ETTask LoginGame(this MainUI self)
        {
            int err = await self.OperaArchive();
            if (err != ErrorCode.ERR_Success)
            {
                Log.Error($"选择存档出错：{err}");
                return;
            }

            var clientSenderComponent = self.Root().GetComponent<ClientSenderComponent>();
            var playerComponent = self.Root().GetComponent<PlayerComponent>();
            string account = playerComponent.Account;

            var c2GEnterGame = C2G_EnterGame.Create();
            c2GEnterGame.Account = account;
            G2C_EnterGame g2CEnterGame = await clientSenderComponent.Call(c2GEnterGame) as G2C_EnterGame;
            if (g2CEnterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"登录Map失败：{g2CEnterGame.Error}");
                return;
            }

            Log.Debug("登录Map成功！");
            await self.Root().GetComponent<FUIComponent>().ShowPanelAsync<LoadingUI, bool>(false);
            self.Root().GetComponent<FUIComponent>().ClosePanel<MainUI>();
        }
    }
}