using Unity.Mathematics;
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
            var gameObject = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().GameObject;
            self.rigidbody = gameObject.Get<Rigidbody2D>("rigidbody");
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
            var unityEventTrigger = gameObjectComponent.unityEventTrigger;
            unityEventTrigger.BelongToUnitId = self.GetParent<Unit_Client>().Id;
            unityEventTrigger.unitType = (int)UnitType.Cast;
            unityEventTrigger.OnCollisionEnterAction += self.OnCollisionEnterAction;
        }

        public static void UnRegisterUnityEventTrigger(this ET.Client.UnitMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            var unityEventTrigger = gameObjectComponent.unityEventTrigger;
            unityEventTrigger.OnCollisionEnterAction -= self.OnCollisionEnterAction;
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

        public static void InitAndMove(this ET.Client.UnitMoveComponent self, bool flipX, Vector2 dir, float speed)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.sprite;
            spriteRenderer.flipX = flipX;
            self.dir = dir;
            self.moveSpeed = speed;
            self.SetRotateByDir();
            self.StartMove();
        }

        public static void StartMove(this ET.Client.UnitMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            if (self.dir == Vector2.zero)
            {
                var spriteRenderer = gameObjectComponent.sprite;
                if (spriteRenderer.flipX)
                    self.dir = Vector2.left;
                else
                    self.dir = Vector2.right;
            }

            var rigidbody2D = self.rigidbody;
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);
        }

        public static void SetRotateByDir(this ET.Client.UnitMoveComponent self)
        {
            float horizontalinput = self.dir.x;
            float verticalinput = self.dir.y;
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.sprite;
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
            SpriteRenderer spriteRenderer = gameObjectComponent.sprite;
            spriteRenderer.flipX = dir.x < 0;

            self.dir = dir;
            self.SetRotateByDir();
        }

        public static async ETTask MoveByManeuver(this UnitMoveComponent self, Vector2 dir, float speed)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            var spriteRenderer = gameObjectComponent.sprite;

            self.dir = dir;
            self.moveSpeed = speed;

            if (self.dir == Vector2.zero)
            {
                if (spriteRenderer.flipX)
                    self.dir = Vector2.left;
                else
                    self.dir = Vector2.right;
            }

            var rigidbody2D = self.rigidbody;
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);

            long forwardMoveTime = 1000;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(forwardMoveTime);
            self.dir = -self.dir;
            self.moveSpeed *= 3f;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);
        }

        public static void MoveToMouse(this ET.Client.UnitMoveComponent self, Vector2 dir, float speed)
        {
            self.dir = dir;
            self.moveSpeed = speed;

            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();

            float dot_X = Vector2.Dot(gameObjectComponent.Transform.right, dir);
            float dot_Y = Vector2.Dot(gameObjectComponent.Transform.up, dir);
            float angle = Mathf.Acos(Vector2.Dot(gameObjectComponent.Transform.right.normalized, dir.normalized)) * Mathf.Rad2Deg;
            if (dot_Y < 0)
                angle = -angle;
            var quaternion = Quaternion.Euler(0, 0, angle);
            gameObjectComponent.SetRotation(quaternion);

            var rigidbody2D = self.rigidbody;
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);
        }
    }
}