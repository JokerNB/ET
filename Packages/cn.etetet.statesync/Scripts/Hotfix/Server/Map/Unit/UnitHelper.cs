using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(NumericDataComponent))]
    public static partial class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = UnitInfo.Create();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.UnitType;
            NumericDataComponent nc = unit.GetComponent<NumericDataComponent>();
            if (nc != null)
                unitInfo.KV = new Dictionary<int, long>(nc.NumericDic);
            return unitInfo;
        }
    }
}