using System.Collections.Generic;

namespace ET.Client
{
    public struct Event_CastStart
    {
        public long castId;
        public long casterId;
        public int castConfigId;
        public List<long> TargetsId;
    }
    public struct Event_CastHit
    {
        public long castId;
        public long casterId;
        public List<long> TargetsId;
    }

    public struct Event_CastFinish
    {
        public long castId;
        public long casterId;
    }

    public struct Event_CastBreak
    {
        public long castId;
        public long casterId;
    }

    public struct Event_BuffAdd
    {
        public long OwnerId;

        public EntityRef<Buff> buffData;
    }

    public struct Event_BuffRemove
    {
        public long BuffId;
        public long OwnerId;
    }

    public struct Event_BuffTick
    {
        public long BuffId;
        public long OwnerId;
    }

    public struct Event_BuffTimeOut
    {
        public EntityRef<Unit_Client> unit;
        public long BuffId;
    }

    public struct Event_BuffUpdate
    {
        public long ownerId;
        public long buffId;
        public int buffConfigId;
    }

    public struct Event_TestCast
    {
        public int castConfigId;
    }
}