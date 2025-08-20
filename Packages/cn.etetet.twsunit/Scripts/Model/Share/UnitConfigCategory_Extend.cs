using System.Collections.Generic;

namespace ET
{
    public partial class UnitConfigCategory
    {
        private readonly List<UnitConfig> unitConfig = new List<UnitConfig>();
        private readonly List<UnitConfig> monsterConfig = new List<UnitConfig>();

        public UnitConfig GetFirstUnitConfig()
        {
            if(unitConfig.Count == 0)
                return null;
            return this.unitConfig[0];
        }

        public override void EndInit()
        {
            foreach (UnitConfig config in this.DataList)
            {
                if (config.UnitType == UnitType.Player)
                    this.unitConfig.Add(config);
                else
                    this.monsterConfig.Add(config);
            }
        }
    }
}