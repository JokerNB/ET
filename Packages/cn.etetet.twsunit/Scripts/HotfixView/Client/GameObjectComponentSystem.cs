using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self)
        {
            self.MainCameraTr = Camera.main.transform;
            //初始化镜头位置
            self.MainCameraTr.position = new Vector3(0, 0, -10f);
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

            self.Root().CurrentScene().GetComponent<MapComponent>().RefreshMap(dirX, dirY);
        }
    }
}