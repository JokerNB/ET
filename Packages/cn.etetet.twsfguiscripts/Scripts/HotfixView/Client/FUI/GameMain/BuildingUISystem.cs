using System.Collections.Generic;
using ET.Client.GameMain;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(BuildingUI))]
    [FriendOf(typeof(BuildingUI))]
    public static partial class BuildingUISystem
    {
        [EntitySystem]
        private static void Awake(this BuildingUI self, ET.Client.GameMain.FUI_BuildingUI fuiBuildingUI)
        {
            self.FUIBuildingUI = fuiBuildingUI;
            self.FUIBuildingUI.BuildingItemList.itemProvider += self.BuildingItemListProvider;
            self.FUIBuildingUI.BuildingItemList.itemRenderer += self.BuildingItemListRenderer;
            self.FUIBuildingUI.BuildingItemList.onClickItem.Add(self.OnClickItem);
            self.InitItem();
        }

        public static string BuildingItemListProvider(this BuildingUI self, int index)
        {
            if (self.groupItemIndex.ContainsKey(index))
                return "ui://GameMain/Label_BuildingGroup";
            return "ui://GameMain/Button_BuildingItem";
        }

        public static void BuildingItemListRenderer(this BuildingUI self, int index, GObject item)
        {
            if (self.groupItemIndex.ContainsKey(index))
            {
                var gLabel = item as GLabel;
                gLabel.title = TileItemGroupConfigCategory.Instance.Get(self.groupItemIndex[index]).Desc;
            }
            else
            {
                var fuiButtonBuildingItem = item as FUI_Button_BuildingItem;
                TileItemConfig config = TileItemConfigCategory.Instance.Get(self.ItemIndex[index]);
                fuiButtonBuildingItem.icon = $"ui://GameMain/{config.UIName}";
                fuiButtonBuildingItem.title = config.Title;
                fuiButtonBuildingItem.Text_Desc.text = config.Desc;
                var price = config.NumericTypeValue[ENumericType.Price0];
                fuiButtonBuildingItem.Text_Price.SetVar("price", price.ToString()).FlushVars();
            }
        }

        public static void OnClickItem(this BuildingUI self, EventContext context)
        {
            var fuiButtonBuildingItem = context.data as FUI_Button_BuildingItem;
            var index = self.FUIBuildingUI.BuildingItemList.GetChildIndex(fuiButtonBuildingItem);
            if (self.ItemIndex.ContainsKey(index))
            {
                TileItemConfig config = TileItemConfigCategory.Instance.Get(self.ItemIndex[index]);
                self.Root().CurrentScene().GetComponent<MapDrawTileComponent>().BuildStart(config.Id);
                self.Root().GetComponent<FUIComponent>().GetPanelLogic<GameMainUI>(true).SetBuildingUIState(0);
            }
        }

        public static void InitItem(this BuildingUI self)
        {
            int idx = 0, itemIdx = 0;
            foreach (TileItemGroupConfig tileItemGroupConfig in TileItemGroupConfigCategory.Instance.DataList)
            {
                int id = tileItemGroupConfig.Id;
                if (TileItemConfigCategory.Instance.TileItemConfigs.ContainsKey(id))
                {
                    self.groupItemIndex.Add(idx, id);
                    foreach (TileItemConfig tileItemConfig in TileItemConfigCategory.Instance.TileItemConfigs[id])
                    {
                        itemIdx++;
                        self.ItemIndex.Add(itemIdx, tileItemConfig.Id);
                    }

                    idx += TileItemConfigCategory.Instance.TileItemConfigs[id].Count + 1;
                    itemIdx = idx;
                }
            }

            self.FUIBuildingUI.BuildingItemList.numItems = TileItemConfigCategory.Instance.DataList.Count + self.groupItemIndex.Count;
        }
    }
}