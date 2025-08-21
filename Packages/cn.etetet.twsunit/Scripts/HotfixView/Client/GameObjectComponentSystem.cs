using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self, GameObject go)
        {
            self.GameObject = go;
            self.SpriteRenderer = self.GameObject.GetComponentInChildren<SpriteRenderer>();
            self.UnityEventTrigger = self.GameObject.GetComponentInChildren<UnityEventTrigger>();
            self.InitColliderBox();
            self.InitColliderTrigger();
        }

        [EntitySystem]
        private static void Update(this ET.Client.GameObjectComponent self)
        {
            self.horizontalinput = Input.GetAxis("Horizontal");
            self.Verticalinput = Input.GetAxis("Vertical");
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }

        public static void InitColliderBox(this GameObjectComponent self)
        {
            var sprite = self.GameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            var boxCollider2D = self.GameObject.GetComponentInChildren<BoxCollider2D>();
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = sprite.bounds.center;
        }

        public static void InitColliderTrigger(this GameObjectComponent self)
        {
            self.UnityEventTrigger = self.GameObject.GetComponentInChildren<UnityEventTrigger>();
            self.UnityEventTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
            self.UnityEventTrigger.BelongToUnitId = self.GetParent<Unit>().Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit>().UnitType;
        }

        private static void OnTriggerEnterAction(this GameObjectComponent self, Collider2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>()?.GetChild<MonsterGameObjectComponent>(unitId);
            monsterComponent?.TestEnterChangeColor();
        }

        private static void OnTriggerExitAction(this GameObjectComponent self, Collider2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>()?.GetChild<MonsterGameObjectComponent>(unitId);
            monsterComponent?.TestExitChangeColor();
        }

        public static void OnFixedUpdate(this GameObjectComponent self)
        {
            self.Move();
        }

        public static void Move(this ET.Client.GameObjectComponent self)
        {
            if (self.horizontalinput == 0 && self.Verticalinput == 0)
                return;

            if (self.horizontalinput != 0 && self.Verticalinput != 0)
            {
                self.horizontalinput *= 0.6f;
                self.Verticalinput *= 0.6f;
            }

            float speed0 = self.GetParent<Unit>().NumericComponent.GetAsFloat(ENumericType.Speed0);
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