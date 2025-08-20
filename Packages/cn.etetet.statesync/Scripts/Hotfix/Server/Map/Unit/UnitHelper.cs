using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(NumericDataComponent))]
    public static partial class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = UnitInfo.Create();
            NumericDataComponent nc = unit.GetComponent<NumericDataComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.UnitType;
            unitInfo.KV = new Dictionary<int, long>(nc.NumericDic);
            return unitInfo;
        }

        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, EntityRef<AOIEntity>> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }
    }
}