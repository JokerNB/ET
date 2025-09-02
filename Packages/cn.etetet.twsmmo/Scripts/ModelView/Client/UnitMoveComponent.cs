using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class UnitMoveComponent : Entity, IAwake<int,long>, IDestroy
    {
        public Vector2 dir;
        public float moveSpeed;
        public int castConfigId;
        public long ownerUnitId;
        public ContactPoint2D contactPoint;
        public Rigidbody2D rigidbody;
    }
}