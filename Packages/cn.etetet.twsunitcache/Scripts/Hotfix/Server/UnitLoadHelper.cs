using Unity.Mathematics;

namespace ET.Server
{
    public static class UnitLoadHelper
    {
        public static async ETTask<(bool, Unit)> LoadUnit(Player player)
        {
            GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            gateMapComponent.Scene = await GateMapFactory.Create(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), "GateMap");
            Unit unit = await UnitCacheHelper.GetUnitCache(player.Root(), gateMapComponent.Scene, player.UnitId);

            bool isNewUnit = unit == null;
            if (isNewUnit)
            {
                unit = UnitFactory.Create(gateMapComponent.Scene, player.UnitId, UnitType.Player);
                unit.AddComponent<UnitDBSaveComponent>();
                unit.AddComponent<KnapsackComponent>();

                UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
            }
            //非缓存服需要存储数据的组件
            if (unit.GetComponent<UnitDBSaveComponent>() == null)
                unit.AddComponent<UnitDBSaveComponent>();
            if (unit.GetComponent<NumericDataComponent>() == null)
                unit.AddComponent<NumericDataComponent>();
            if (unit.GetComponent<NumericNoticeComponent>() == null)
                unit.AddComponent<NumericNoticeComponent>();
            

            return (isNewUnit, unit);
        }
    }
}