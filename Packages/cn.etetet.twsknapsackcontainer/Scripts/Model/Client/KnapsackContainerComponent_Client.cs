using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof(KnapsackComponent_Client))]
    public class KnapsackContainerComponent_Client : Entity, IAwake<int>, IDestroy
    {
        public int KnapsackContainerType { get; set; }

        public Dictionary<long, EntityRef<Item>> Items = new Dictionary<long, EntityRef<Item>>();
    }
}