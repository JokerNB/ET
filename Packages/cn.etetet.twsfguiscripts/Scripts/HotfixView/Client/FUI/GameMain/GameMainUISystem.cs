using ET.Client.GameMain;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameMainUI))]
    [FriendOf(typeof(GameMainUI))]
    public static partial class GameMainUISystem
    {
        [EntitySystem]
        private static void Awake(this GameMainUI self)
        {
            //默认不显示
            self.SetBuildingUIState(0);
            self.FUIGameMainUI.FuncList.onClickItem.Add(self.OnFuncItemClick);
            self.FUIGameMainUI.FuncList.itemRenderer += self.OnFuncItemRenderer;
            self.FUIGameMainUI.FuncList.numItems = 9;

            self.AddChild<BuildingUI, FUI_BuildingUI>(self.FUIGameMainUI.BuildingUI);
        }

        public static void OnFuncItemRenderer(this GameMainUI self, int idx, GObject item)
        {
            var fuiButtonFunc = item as FUI_Button_Func;
            fuiButtonFunc.icon = $"ui://GameMain/Flag_{idx}";
            switch (idx)
            {
                case 0:
                    fuiButtonFunc.title = "建造";
                    fuiButtonFunc.Type.selectedIndex = 0;
                    break;
                case 1:
                    fuiButtonFunc.title = "房间";
                    fuiButtonFunc.Type.selectedIndex = 0;
                    break;
                case 2:
                    fuiButtonFunc.title = "道具";
                    fuiButtonFunc.Type.selectedIndex = 0;
                    break;
                case 3:
                    fuiButtonFunc.title = "设施";
                    fuiButtonFunc.Type.selectedIndex = 0;
                    break;
                case 4:
                    fuiButtonFunc.title = "酷客";
                    fuiButtonFunc.Type.selectedIndex = 0;
                    break;
                case 5:
                    fuiButtonFunc.title = "追随者";
                    fuiButtonFunc.Type.selectedIndex = 1;
                    break;
                case 6:
                    fuiButtonFunc.title = "任务";
                    fuiButtonFunc.Type.selectedIndex = 2;
                    break;
                case 7:
                    fuiButtonFunc.title = "研究";
                    fuiButtonFunc.Type.selectedIndex = 2;
                    break;
                case 8:
                    fuiButtonFunc.title = "神启";
                    fuiButtonFunc.Type.selectedIndex = 2;
                    break;
                default:
                    break;
            }
        }

        public static void OnFuncItemClick(this GameMainUI self, EventContext context)
        {
            var gButton = context.data as FUI_Button_Func;
            int idx = self.FUIGameMainUI.FuncList.GetChildIndex(gButton);
            if (idx == 0)
            {
                self.SetBuildingUIState(1 - self.FUIGameMainUI.BuildingUIState.selectedIndex);
            }
        }

        public static void SetBuildingUIState(this GameMainUI self, int idx)
        {
            self.FUIGameMainUI.BuildingUIState.selectedIndex = idx;
        }
    }
}