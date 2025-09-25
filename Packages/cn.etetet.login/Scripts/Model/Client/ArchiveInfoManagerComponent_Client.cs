using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ArchiveInfoManagerComponent_Client : Entity, IAwake, IDestroy
    {
        public List<EntityRef<ArchiveInfo>> ArchiveInfos = new List<EntityRef<ArchiveInfo>>();

        public EntityRef<ArchiveInfo> CurArchiveInfo { get; set; } = default;

        public bool hasUpdate = false;
        public long UpdateTime = default;
        public long UpdateTimeInterval = 5 * 60 * 1000; //5分钟
    }
}