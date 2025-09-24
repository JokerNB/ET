using System.Collections.Generic;

namespace ET
{
    public partial class TileItemConfigCategory
    {
        public Dictionary<int, List<TileItemConfig>> TileItemConfigs = new Dictionary<int, List<TileItemConfig>>();

        public override void EndInit()
        {
            base.EndInit();

            foreach (var itemConfig in this.DataList)
            {
                int groupId = itemConfig.GroupID;
                if (!this.DataMap.ContainsKey(groupId))
                    continue;
                if (this.TileItemConfigs.TryGetValue(groupId, out List<TileItemConfig> tileItemConfigs))
                {
                    tileItemConfigs.Add(itemConfig);
                }
                else
                {
                    this.TileItemConfigs.Add(groupId, new List<TileItemConfig>()
                    {
                        itemConfig
                    });
                }
            }
        }
    }
}

