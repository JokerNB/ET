using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class ArchiveInfoManagerComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<long, List<EntityRef<ArchiveInfo>>> ArchiveInfos = new Dictionary<long, List<EntityRef<ArchiveInfo>>>();
        public Dictionary<long, int> ArchiveLastNumber = new Dictionary<long, int>();
    }
}