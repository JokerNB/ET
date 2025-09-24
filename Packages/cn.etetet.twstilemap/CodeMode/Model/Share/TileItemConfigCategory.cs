using System.Collections.Generic;

namespace ET
{
    public partial class TileItemConfigCategory
    {
        public Dictionary<int, List<TileItemConfig>> TileItemConfigs = new Dictionary<int, List<TileItemConfig>>();

        public override void EndInit()
        {
            base.EndInit();

            foreach (TileItemConfig tileItemConfig in this.DataList)
            {
                if (this.TileItemConfigs.TryGetValue(tileItemConfig.GroupID, out List<TileItemConfig> tileItemConfigs))
                {
                    this.TileItemConfigs[tileItemConfig.GroupID].Add(tileItemConfig);
                }
                else
                {
                    this.TileItemConfigs.Add(tileItemConfig.GroupID, new List<TileItemConfig>()
                    {
                        tileItemConfig
                    });
                }
            }
        }
    }
}

