using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(MonsterManagerComponent))]
    public class MonsterComponent : Entity, IAwake<int, int, int>, IDestroy
    {
        public int ConfigId { get; set; }
        public MonsterConfig Config => MonsterConfigCategory.Instance.Get(this.ConfigId);
        public UnitType UnitType => this.Config.UnitType;

        public GameObject go;
        public int dirX;
        public int dirY;

        //碰撞触发器
        public UnityEventTrigger UnityEventTrigger;

        public SpriteRenderer spriteRenderer;

        public Rigidbody2D rigidbody2D;
    }
}