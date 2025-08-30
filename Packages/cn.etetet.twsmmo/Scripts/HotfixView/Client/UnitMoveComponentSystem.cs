using UnityEngine;

namespace ET.Client
{
    [Invoke(TimerInvokeType.UnitFrameMoveTimer)]
    public class UnitFrameMoveTimer_Invoke : ATimer<UnitMoveComponent>
    {
        protected override void Run(UnitMoveComponent t)
        {
            t?.StartMove();
        }
    }

    [EntitySystemOf(typeof(UnitMoveComponent))]
    public static partial class UnitMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UnitMoveComponent self, int castConfigId, long ownerUnitId)
        {
            self.castConfigId = castConfigId;
            self.ownerUnitId = ownerUnitId;
            self.InitUnityEventTrigger();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.UnitMoveComponent self)
        {
            self.StopMove();
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
            if (unitType != (int)UnitType.Camera)
            {
                var unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
                Unit_Client ownerUnit = unitComponentClient.Get(self.ownerUnitId);
                if(unitType == (int)ownerUnit.UnitType)
                    return;
                                
                return;
            }
            else
            {
                var selfUnit = self.GetParent<Unit_Client>();
                if (selfUnit.UnitType == UnitType.Bullet)
                {
                    var unitComponentClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>();
                    unitComponentClient.Remove(selfUnit.Id);
                    return;
                }
                
            }
            var reflectDir = Vector3.Reflect(self.dir, collider.GetContact(0).normal).normalized;
            if (Mathf.Abs(reflectDir.x) <= 0.01f)
                reflectDir.x = 0;
            if (Mathf.Abs(reflectDir.y) <= 0.01f)
                reflectDir.y = 0;
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipX = reflectDir.x < 0;
            self.dir = reflectDir;
            self.SetRotateByDir();
            Log.Error($"UnitMoveComponent.OnCollisionEnterAction: unitId:{unitId} unitType:{unitType} , normalized:{reflectDir}");
        }

        public static void InitAndMove(this ET.Client.UnitMoveComponent self, bool flipX, Vector2 dir, float speed, Vector2 ownerUnitPos)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            SpriteRenderer spriteRenderer = gameObjectComponent.GameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipX = flipX;
            // CastConfig config = CastConfigCategory.Instance.Get(self.castConfigId);
            gameObjectComponent.Transform.localPosition = ownerUnitPos;
            gameObjectComponent.Transform.localScale = Vector2.one;

            self.MoveByDirection(dir, speed);
        }

        public static void MoveByDirection(this ET.Client.UnitMoveComponent self, Vector2 dir, float speed)
        {
            self.dir = dir;
            self.moveSpeed = speed;
            // self.frameTimer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.UnitFrameMoveTimer, self);
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

            // transform.Translate(self.dir * Time.deltaTime * self.moveSpeed, Space.Self);
            // var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            var rigidbody2D = gameObjectComponent.GameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(self.dir * self.moveSpeed, ForceMode2D.Impulse);
        }

        public static void StopMove(this ET.Client.UnitMoveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.frameTimer);
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
    }
}