using System.Collections.Generic;

namespace ET
{
    public partial class UnitConfigCategory
    {
        private readonly List<UnitConfig> unitConfig = new List<UnitConfig>();
        public UnitConfig castUnitConfig { get; private set; }  = null;
        public UnitConfig energyBlockUnitConfig { get; private set; }  = null;

        public UnitConfig GetFirstUnitConfig()
        {
            if (unitConfig.Count == 0)
                return null;
            return this.unitConfig[0];
        }

        public override void EndInit()
        {
            foreach (UnitConfig config in this.DataList)
            {
                if (config.UnitType == UnitType.Player)
                    this.unitConfig.Add(config);
                else if (config.UnitType == UnitType.Cast)
                {
                    if (this.castUnitConfig != null)
                    {
                        Log.Error("castUnitConfig is exist!!!");
                        continue;
                    }

                    this.castUnitConfig = config;
                }
                else if (config.UnitType == UnitType.EnergyBlock)
                {
                    if (this.energyBlockUnitConfig != null)
                    {
                        Log.Error("energyBlockUnitConfig is exist!!!");
                        continue;
                    }

                    this.energyBlockUnitConfig = config;
                }
            }
        }
    }
}