using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MoveComponent))]
    public static partial class MoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MoveComponent self)
        {
            self.GameObject = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().GameObject;
            self.SpriteRenderer = self.GameObject.GetComponentInChildren<SpriteRenderer>();
            self.UnityEventTrigger = self.GameObject.GetComponentInChildren<UnityEventTrigger>();
            self.InitColliderBox();
            self.InitColliderTrigger();
        }
        
        [EntitySystem]
        private static void Update(this ET.Client.MoveComponent self)
        {
            self.horizontalinput = Input.GetAxis("Horizontal");
            self.Verticalinput = Input.GetAxis("Vertical");
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.MoveComponent self)
        {
            self.UnityEventTrigger.OnTriggerEnterAction -= self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction -= self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdate;
        }
        
        public static void InitColliderBox(this MoveComponent self)
        {
            var sprite = self.GameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            var boxCollider2D = self.GameObject.GetComponentInChildren<BoxCollider2D>();
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = sprite.bounds.center;
        }

        public static void InitColliderTrigger(this MoveComponent self)
        {
            self.UnityEventTrigger = self.GameObject.GetComponentInChildren<UnityEventTrigger>();
            self.UnityEventTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
            self.UnityEventTrigger.BelongToUnitId = self.GetParent<Unit_Client>().Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
        }

        private static void OnTriggerEnterAction(this MoveComponent self, Collider2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<UnitComponent_Client>()?.GetChild<MonsterMoveComponent>(unitId);
            monsterComponent?.TestEnterChangeColor();
        }

        private static void OnTriggerExitAction(this MoveComponent self, Collider2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>()?.GetChild<MonsterMoveComponent>(unitId);
            monsterComponent?.TestExitChangeColor();
        }

        public static void OnFixedUpdate(this MoveComponent self)
        {
            self.Move();
        }

        public static void Move(this ET.Client.MoveComponent self)
        {
            if (self.horizontalinput == 0 && self.Verticalinput == 0)
                return;

            if (self.horizontalinput != 0 && self.Verticalinput != 0)
            {
                self.horizontalinput *= 0.6f;
                self.Verticalinput *= 0.6f;
            }

            float speed0 = self.GetParent<Unit_Client>().NumericComponent.GetAsFloat(ENumericType.Speed0);
            var x = Vector3.right * self.horizontalinput * Time.fixedDeltaTime * speed0;
            var y = Vector3.up * self.Verticalinput * Time.fixedDeltaTime * speed0;
            self.Transform.Translate(x);
            self.Transform.Translate(y);
            //计算相机边界，调整地图
            int dirX = 0;
            if (self.horizontalinput > 0)
            {
                dirX = 1;
                if (self.SpriteRenderer.flipX)
                    self.SpriteRenderer.flipX = false;
            }
            else if (self.horizontalinput < 0)
            {
                dirX = -1;
                if (!self.SpriteRenderer.flipX)
                    self.SpriteRenderer.flipX = true;
            }

            int dirY = 0;
            if (self.Verticalinput > 0)
                dirY = 1;
            else if (self.Verticalinput < 0)
                dirY = -1;

            //须保证顺序，地图计算需要根据相机位置
            self.Root().CurrentScene().GetComponent<CameraComponent>().Translate(x);
            self.Root().CurrentScene().GetComponent<CameraComponent>().Translate(y);
            self.Root().CurrentScene().GetComponent<CameraComponent>().OrthographicCameraEdge();
            self.Root().CurrentScene().GetComponent<MapManagerComponent>().RefreshMap(dirX, dirY);
        }
    }
}