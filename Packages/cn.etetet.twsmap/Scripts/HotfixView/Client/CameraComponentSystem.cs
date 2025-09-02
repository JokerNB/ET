using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(CameraComponent))]
    public static partial class CameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.CameraComponent self)
        {
            self.MainCamera = Camera.main;
            self.MainCameraTr.position = new Vector3(0, 0, -10f);
            self.OrthographicCameraEdge();
            self.InitUnityEventTrigger();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.CameraComponent self)
        {
        }

        public static void InitUnityEventTrigger(this ET.Client.CameraComponent self)
        {
            var unityEventTrigger = self.MainCameraTr.gameObject.GetComponent<UnityEventTrigger>();
            unityEventTrigger.unitType = (int)UnitType.Camera;
        }

        public static void Translate(this ET.Client.CameraComponent self, Vector3 pos)
        {
            self.MainCameraTr.Translate(pos);
        }

        public static void OrthographicCameraEdge(this ET.Client.CameraComponent self)
        {
            float CameraX = self.MainCameraTr.position.x;
            float RotationX = self.MainCameraTr.rotation.x;
            //相机大小
            float CameraSize = self.MainCamera.orthographicSize;
            //相机高度（y轴坐标）
            float CameraY = self.MainCameraTr.position.y;

            float CXSize = CameraSize * 2 * ((float)Screen.width / (float)Screen.height);
            float CYSize = CameraSize * 2 / Mathf.Cos(RotationX * Mathf.Deg2Rad);

            //左
            self.cameraBoundPos.x = CameraX - CXSize / 2;
            //下
            self.cameraBoundPos.y = CameraY - CameraSize;
            //右
            self.cameraBoundPos.z = CameraX + CXSize / 2;
            //上
            self.cameraBoundPos.w = CameraY + CYSize / 2;

            //摄像机宽高
            self.cameraBoundSize.x = self.cameraBoundPos.z - self.cameraBoundPos.x;
            self.cameraBoundSize.y = self.cameraBoundPos.w - self.cameraBoundPos.y;
        }

        public static Vector2 GetInputMousePos(this ET.Client.CameraComponent self)
        {
            return self.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}