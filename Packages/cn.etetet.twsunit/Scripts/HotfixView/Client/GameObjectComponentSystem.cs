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
            self.InitColliderBox();
            self.InitColliderTrigger();
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
            self.colliderTrigger = self.GameObject.GetComponentInChildren<ColliderTrigger>();
            self.colliderTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            self.colliderTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            self.colliderTrigger.BelongToUnitId = self.GetParent<Unit>().Id;
            self.colliderTrigger.unitType = (int)self.GetParent<Unit>().UnitType;
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

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }

        [EntitySystem]
        private static void Update(this ET.Client.GameObjectComponent self)
        {
            self.MoveCamera();
        }

        public static void MoveCamera(this ET.Client.GameObjectComponent self)
        {
            float horizontalinput = Input.GetAxis("Horizontal");
            float Verticalinput = Input.GetAxis("Vertical");
            if (horizontalinput == 0 && Verticalinput == 0)
                return;

            if (horizontalinput != 0 && Verticalinput != 0)
            {
                horizontalinput *= 0.6f;
                Verticalinput *= 0.6f;
            }

            var x = Vector3.right * horizontalinput * Time.deltaTime * 10;
            var y = Vector3.up * Verticalinput * Time.deltaTime * 10;
            self.Transform.Translate(x);
            self.Transform.Translate(y);
            self.MainCameraTr.Translate(x);
            self.MainCameraTr.Translate(y);
            //计算相机边界，调整地图
            int dirX = 0;
            if (horizontalinput > 0)
                dirX = 1;
            else if (horizontalinput < 0)
                dirX = -1;

            int dirY = 0;
            if (Verticalinput > 0)
                dirY = 1;
            else if (Verticalinput < 0)
                dirY = -1;

            //须保证顺序，地图计算需要根据相机位置
            self.Root().CurrentScene().GetComponent<CameraComponent>().OrthographicCameraEdge();
            self.Root().CurrentScene().GetComponent<MapComponent>().RefreshMap(dirX, dirY);
        }
    }
}