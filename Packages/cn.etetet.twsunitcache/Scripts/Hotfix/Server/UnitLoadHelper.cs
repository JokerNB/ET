namespace ET.Server
{
    public static class UnitLoadHelper
    {
        public static async ETTask<(bool, Unit)> LoadPlayerUnit(Player player, long unitId)
        {
            GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            gateMapComponent.Scene = await GateMapFactory.Create(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), "GateMap");
            Unit unit = await UnitCacheHelper.GetUnitCache(player.Root(), gateMapComponent.Scene, unitId);
            bool isNewUnit = unit == null;
            if (isNewUnit)
            {
                unit = UnitFactory.CreatePlayerUnit(gateMapComponent.Scene, unitId);
            }
            return (isNewUnit, unit);
        }
    }
}