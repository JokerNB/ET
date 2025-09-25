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
                if (itemConfig.CellData.Count != 2)
                {
                    Log.Error($"TileItemConfigCategory CellData Count is wrong : {itemConfig.Id}");
                    continue;
                }
                int groupId = itemConfig.GroupID;
                if (itemConfig.TileData.DrawType == DrawType.Drag)
                {
                    if (itemConfig.CellData[0] != 1 || itemConfig.CellData[1] != 1)
                    {
                        Log.Error($"TileItemConfigCategory CellData is wrong : {itemConfig.Id} , DrawType is Drag, CellData must be 1");
                        continue;
                    }
                }

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