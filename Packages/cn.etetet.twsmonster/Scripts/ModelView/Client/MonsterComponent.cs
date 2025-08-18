using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(MonsterManagerComponent))]
    public class MonsterComponent : Entity, IAwake<int,int,int>, IUpdate
    {

        public int ConfigId { get; set; }
        public MonsterConfig Config => MonsterConfigCategory.Instance.Get(this.ConfigId);
        public UnitType UnitType => this.Config.UnitType;

        public GameObject go;
        public int dirX;
        public int dirY;
        
        //碰撞触发器
        public ColliderTrigger colliderTrigger;
        
        public SpriteRenderer spriteRenderer;
    }
}