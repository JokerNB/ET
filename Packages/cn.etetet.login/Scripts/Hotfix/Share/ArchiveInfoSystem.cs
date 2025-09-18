using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(ArchiveInfo))]
    public static partial class ArchiveInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.ArchiveInfo self)
        {
        }

        public static ArchiveInfoProto ToMessage(this ArchiveInfo self)
        {
            ArchiveInfoProto archiveInfoProto = ArchiveInfoProto.Create();
            archiveInfoProto.AccountHash = self.AccountLongHash;
            archiveInfoProto.ArchiveNum = self.ArchiveNumber;
            archiveInfoProto.Recruits = new List<long>(self.RecruitUnitIds);
            return archiveInfoProto;
        }

        public static void FromMessage(this ET.ArchiveInfo self, ArchiveInfoProto archiveInfoProto)
        {
            self.RecruitUnitIds.Clear();
            self.RecruitUnitIds.AddRange(archiveInfoProto.Recruits);
            self.ArchiveNumber = archiveInfoProto.ArchiveNum;
            self.AccountLongHash = archiveInfoProto.AccountHash;
        }
    }
}