namespace ET.Server
{
    public static class ItemNoticeHelper
    {
        public static void SyncItemInfo(Unit unit, Item item, ItemOpType itemOpType)
        {
            M2C_UpdateItemInfo m2CUpdateItemInfo = M2C_UpdateItemInfo.Create();
            m2CUpdateItemInfo.Op = (int)itemOpType;
            m2CUpdateItemInfo.ItemInfo = item.ToMessage();
            MapMessageHelper.SendToClient(unit, m2CUpdateItemInfo);
        }
    }
}