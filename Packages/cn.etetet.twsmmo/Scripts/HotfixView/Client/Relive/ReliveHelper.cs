using UnityEngine;

namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.ReliveComponent))]
    public static class ReliveHelper
    {
        public static bool IsAlive(this Unit_Client unit)
        {
            return unit.GetComponent<ReliveComponent>()?.Alive ?? true;
        }

        /// <summary>
        /// 原地复活
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static int OnSiteRelive(this Unit_Client unit)
        {
            if (unit.IsAlive())
                return ErrorCode.ERR_Relive_Alive;
            unit.DoRelive(unit.GetSelfPosition(), 1);
            return ErrorCode.ERR_Success;
        }

        public static int PointRelive(this Unit_Client unit, Vector2 pos)
        {
            if (unit.IsAlive())
                return ErrorCode.ERR_Relive_Alive;
            unit.DoRelive(pos, 1);
            return ErrorCode.ERR_Success;
        }

        public static void DoRelive(this Unit_Client unit, Vector2 pos, float hpRate)
        {
            if (unit.IsAlive())
                return;

            unit.SetUnitPosition(pos);
            var newHp = unit.NumericComponent.GetAsInt(ENumericType.MaxHp0) * hpRate;
            unit.NumericComponent.Change(ENumericType.Hp0, newHp);
            unit.GetComponent<ReliveComponent>().Alive = true;
        }
    }
}