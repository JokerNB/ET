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
            self.MainCameraTr = Camera.main.transform;
            //初始化镜头位置
            self.MainCameraTr.position = new Vector3(0, 0, -10f);
            self.GameObject = go;
            self.SpriteRenderer = self.GameObject.GetComponentInChildren<SpriteRenderer>();
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
            self.UnityEventTrigger.OnTriggerEnterAction -= self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction -= self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdate;

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
            self.UnityEventTrigger.BelongToUnitId = self.GetParent<Unit>().Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit>().UnitType;

            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
        }

        private static void OnTriggerEnterAction(this GameObjectComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>()?.GetChild<MonsterComponent>(unitId);
            monsterComponent?.TestEnterChangeColor();
        }

        private static void OnTriggerExitAction(this GameObjectComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            var monsterComponent = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>()?.GetChild<MonsterComponent>(unitId);
            monsterComponent?.TestExitChangeColor();
        }

        public static void OnFixedUpdate(this GameObjectComponent self)
        {
            self.MoveCamera();
        }

        public static void MoveCamera(this ET.Client.GameObjectComponent self)
        {
            if (self.horizontalinput == 0 && self.Verticalinput == 0)
                return;

            if (self.horizontalinput != 0 && self.Verticalinput != 0)
            {
                self.horizontalinput *= 0.6f;
                self.Verticalinput *= 0.6f;
            }

            var x = Vector3.right * self.horizontalinput * Time.fixedDeltaTime * 10;
            var y = Vector3.up * self.Verticalinput * Time.fixedDeltaTime * 10;
            self.Transform.Translate(x);
            self.Transform.Translate(y);
            self.MainCameraTr.Translate(x);
            self.MainCameraTr.Translate(y);
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
            self.Root().CurrentScene().GetComponent<CameraComponent>().OrthographicCameraEdge();
            self.Root().CurrentScene().GetComponent<MapComponent>().RefreshMap(dirX, dirY);
        }
    }
}