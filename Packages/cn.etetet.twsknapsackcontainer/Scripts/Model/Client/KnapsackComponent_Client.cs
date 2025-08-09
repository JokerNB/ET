using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class KnapsackComponent_Client : Entity, IAwake, IDestroy
    {
        public Dictionary<int, EntityRef<KnapsackContainerComponent_Client>> ContainerInfoDic =
                new Dictionary<int, EntityRef<KnapsackContainerComponent_Client>>();
    }
}