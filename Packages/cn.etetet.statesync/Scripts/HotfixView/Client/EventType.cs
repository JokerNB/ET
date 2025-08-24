using System.Numerics;
using Unity.Mathematics;

namespace ET.Client
{
    public struct SceneChangeStart
    {
    }
    
    public struct SceneChangeFinish
    {
    }
    
    public struct AfterCreateClientScene
    {
    }
    
    public struct AfterCreateCurrentScene
    {
    }

    public struct AppStartInitFinish
    {
    }

    public struct LoginFinish
    {
    }

    public struct EnterMapFinish
    {
    }

    public struct AfterUnitCreate
    {
        public Unit_Client Unit;
    }
    
    public struct AfterMonsterCreate
    {
        public Unit_Client Unit;
    }
    
    public struct AfterBulletCreate
    {
        public Unit_Client Unit;
        public float3 pos;
        public int bulledId;
        public long ownerId;
    }
}