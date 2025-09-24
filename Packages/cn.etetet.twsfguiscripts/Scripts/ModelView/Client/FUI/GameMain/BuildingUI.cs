using System.Collections.Generic;
using ET.Client.GameMain;

namespace ET.Client
{
    [ChildOf]
    public class BuildingUI: Entity, IAwake<FUI_BuildingUI>
    {
        public FUI_BuildingUI FUIBuildingUI { get; set; }

        public Dictionary<int,int> groupItemIndex = new Dictionary<int, int>();
        public Dictionary<int,int> ItemIndex = new Dictionary<int, int>();
    }
}
