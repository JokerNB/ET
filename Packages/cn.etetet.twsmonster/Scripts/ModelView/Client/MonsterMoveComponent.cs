using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class MonsterMoveComponent : Entity, IAwake, IDestroy
    {
        //碰撞触发器
        public UnityEventTrigger UnityEventTrigger;

        public SpriteRenderer spriteRenderer;

        public PolyNavAgent agent;
    }
}