using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitMoveComponent))]
    public static partial class UnitMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitMoveComponent self, int castConfigId, long ownerUnitId)
        {
            self.castConfigId = castConfigId;
            self.ownerUnitId = ownerUnitId;
            self.InitUnityEventTrigger();
        }

        [EntitySystem]
        private static void Destroy(this UnitMoveComponent self)
        {
            self.castConfigId = default;
            self.ownerUnitId = default;
            self.moveSpeed = default;
            self.dir = default;
            self.UnRegisterUnityEventTrigger();
        }

        public static void InitUnityEventTrigger(this ET.Client.UnitMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            var unityEventTrigger = gameObjectComponent.GameObject.GetComponent<UnityEventTrigger>();
            unityEventTrigger.BelongToUnitId = self.GetParent<Unit_Client>().Id;
            unityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
            unityEventTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            unityEventTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            unityEventTrigger.OnCollisionEnterAction += self.OnCollisionEnterAction;
        }

        public static void UnRegisterUnityEventTrigger(this ET.Client.UnitMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            var unityEventTrigger = gameObjectComponent.GameObject.GetComponent<UnityEventTrigger>();
            unityEventTrigger.OnTriggerEnterAction -= self.OnTriggerEnterAction;
            unityEventTrigger.OnTriggerExitAction -= self.OnTriggerExitAction;
            unityEventTrigger.OnCollisionEnterAction -= self.OnCollisionEnterAction;
        }

        public static void OnTriggerEnterAction(this UnitMoveComponent self, Collider2D collider, long unitId, int unitType)
        {
            Log.Error($"UnitMoveComponent.OnTriggerEnterAction: unitId:{unitId} unitType:{unitType}");
            if (unitType == (int)UnitType.Camera && self.GetParent<Unit_Client>().UnitType == UnitType.Bullet)
            {
                //子弹消失
                self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(self.GetParent<Unit_Client>().Id);
            }
        }

        public static void OnTriggerExitAction(this UnitMoveComponent self, Collider2D collider, long unitId, int unitType)
        {
            Log.Error($"UnitMoveComponent.OnTriggerExitAction: unitId:{unitId} unitType:{unitType}");
        }

        public static void OnCollisionEnterAction(this UnitMoveComponent self, Collision2D collider, long unitId, int unitType)
        {
            var unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            Unit_Client ownerUnit = unitComponentClient.Get(self.ownerUnitId);
            //自己发射的技能屏蔽自身伤害
            if (unitType == (int)ownerUnit.UnitType)
                return;

            self.contactPoint = collider.GetContact(0);
            var cast = ownerUnit.GetComponent<CastComponent>().Get(self.castConfigId);
            cast.CastHit(unitId, self.GetParent<Unit_Client>()).NoContext();
        }

        public static void InitAndMove(this ET.Client.UnitMoveComponent self, bool flipX, Vector2 dir, float speed, Vector2 ownerUnitPos)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipX = flipX;
            gameObjectComponent.Transform.localPosition = ownerUnitPos;
            gameObjectComponent.Transform.localScale = Vector2.one;
            self.dir = dir;
            self.moveSpeed = speed;
            self.SetRotateByDir();
            self.StartMove();
        }

        public static void StartMove(this ET.Client.UnitMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            Transform transform = gameObjectComponent.Transform;
            if (self.dir == Vector2.zero)
            {
                var spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer.flipX)
                    self.dir = Vector2.left;
                else
                    self.dir = Vector2.right;
            }

            var rigidbody2D = gameObjectComponent.GameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);
        }

        public static void SetRotateByDir(this ET.Client.UnitMoveComponent self)
        {
            float horizontalinput = self.dir.x;
            float verticalinput = self.dir.y;
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
            bool flipX = spriteRenderer.flipX;
            int dir = 0;
            if (horizontalinput > 0)
            {
                if (verticalinput > 0)
                    dir = 45;
                else if (verticalinput < 0)
                    dir = -45;
                else
                    dir = 0;
            }
            else if (horizontalinput < 0)
            {
                if (verticalinput > 0)
                    dir = -45;
                else if (verticalinput < 0)
                    dir = 45;
                else
                    dir = 0;
            }
            else
            {
                if (verticalinput > 0)
                    dir = flipX ? -90 : 90;
                else if (verticalinput < 0)
                    dir = flipX ? 90 : -90;
                else
                    dir = 0;
            }

            Quaternion rotation = Quaternion.Euler(0, 0, dir);
            spriteRenderer.transform.localRotation = rotation;
        }

        public static void Contact(this ET.Client.UnitMoveComponent self, Vector2 dir)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipX = dir.x < 0;

            self.dir = dir;
            self.SetRotateByDir();
        }
    }
}