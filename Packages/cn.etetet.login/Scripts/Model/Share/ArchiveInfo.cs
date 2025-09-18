using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf]
    public class ArchiveInfo : Entity, IAwake
    {
        //存档编号
        public int ArchiveNumber;
        //玩家ID，每个账号唯一的ID
        public long AccountLongHash;
        //招募的人员
        public List<long> RecruitUnitIds = new List<long>();
    }
}

