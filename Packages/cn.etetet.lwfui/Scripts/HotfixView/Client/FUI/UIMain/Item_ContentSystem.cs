namespace ET.Client
{
    [EntitySystemOf(typeof(Item_Content))]
    [FriendOf(typeof(Item_Content))]
    public static partial class Item_ContentSystem
    {
        [EntitySystem]
        private static void Awake(this Item_Content self, ET.Client.UIMain.FUI_Item_Content fuiItem_Content)
        {
            self.FUIItem_Content = fuiItem_Content;
        }

    }
}