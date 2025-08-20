using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class MonsterGameObjectComponent : Entity, IAwake, IDestroy
    {
        public GameObject go;
        public Transform goTr;

        //碰撞触发器
        public UnityEventTrigger UnityEventTrigger;

        public SpriteRenderer spriteRenderer;

        public Rigidbody2D rigidbody2D;

        public Transform monsterRoot;
    }
}