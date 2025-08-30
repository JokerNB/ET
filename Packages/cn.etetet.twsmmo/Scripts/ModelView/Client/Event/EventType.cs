using System.Collections.Generic;

namespace ET.Client
{
    public struct Event_CastStart
    {
        public long castId;
        public long casterId;
        public int castConfigId;
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
    
    
    public struct AfterMonsterCreate
    {
        public EntityRef<Unit_Client> Unit;
    }

    public struct AfterSkillCreate
    {
        public EntityRef<Unit_Client> OwnerUnit;
        public EntityRef<Unit_Client> SkillUnit;
        public EntityRef<Cast> castSelf;
        public int castConfigId;
    }
    
    public struct AfterParticleCreate
    {
        public EntityRef<Unit_Client> Caster;
        public EntityRef<Unit_Client> Owner;
        public EntityRef<Unit_Client> Unit_Particle;

    }
    
}