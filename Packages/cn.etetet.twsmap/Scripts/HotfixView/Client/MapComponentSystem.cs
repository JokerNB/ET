using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapComponent))]
    [FriendOfAttribute(typeof(ET.Client.MapManagerComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapComponent self, UnityEngine.Vector2 args2)
        {
            self.centerPos = args2;
            self.Create();
        }

        public static void Create(this MapComponent self)
        {
            var mapComponent = self.GetParent<MapManagerComponent>();
            self.go = UnityEngine.Object.Instantiate(mapComponent.goAsset, mapComponent.MapRootTr, true);
            self.go.GetComponent<SpriteRenderer>().sprite = mapComponent.spriteAsset;
            self.goTr = self.go.transform;
            self.goTr.localPosition = self.centerPos;
            self.spriteSize = mapComponent.spriteSize;
            if (!self.go.activeSelf)
                self.go.SetActive(true);
        }

        public static bool CheckNeedUpdate(this MapComponent self)
        {
            if (!self.go.activeSelf)
                return true;
            var cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            //摄像机宽高
            var wh = cameraComponent.cameraBoundSize;
            //摄像机中心点
            var cameraPos = cameraComponent.MainCameraTr.position;
            //摄像机左边
            float minX = cameraPos.x - wh.x * 0.5f;
            float maxX = cameraPos.x + wh.x * 0.5f;
            float minY = cameraPos.y - wh.y * 0.5f;
            float maxY = cameraPos.y + wh.y * 0.5f;
            
            bool isOverSizeX = self.spriteSize.x >= cameraComponent.cameraBoundSize.x;
            bool isOverSizeY = self.spriteSize.y >= cameraComponent.cameraBoundSize.y;
            
            if (isOverSizeX)
            {
                self.isXLeftInside = self.centerPos.x - self.spriteSize.x * 0.5f <= maxX &&
                        self.centerPos.x - self.spriteSize.x * 0.5f >= minX - self.spriteSize.x;
                self.isXRightInside = self.centerPos.x + self.spriteSize.x * 0.5f >= minX &&
                        self.centerPos.x + self.spriteSize.x * 0.5f <= maxX + self.spriteSize.x;
            }
            else
            {
                self.isXLeftInside = self.centerPos.x - self.spriteSize.x * 0.5f >= minX && self.centerPos.x - self.spriteSize.x * 0.5f <= maxX;
                self.isXRightInside = self.centerPos.x + self.spriteSize.x * 0.5f >= minX && self.centerPos.x + self.spriteSize.x * 0.5f <= maxX;
            }
            
            if (isOverSizeY)
            {
                self.isYBottomInside = self.centerPos.y - self.spriteSize.y * 0.5f >= minY - self.spriteSize.y &&
                        self.centerPos.y - self.spriteSize.y * 0.5f <= maxY;
                self.isYTopInside = self.centerPos.y + self.spriteSize.y * 0.5f >= minY &&
                        self.centerPos.y + self.spriteSize.y * 0.5f <= maxY + self.spriteSize.y;
            }
            else
            {
                self.isYTopInside = self.centerPos.y + self.spriteSize.y * 0.5f >= minY && self.centerPos.y + self.spriteSize.y * 0.5f <= maxY;
                self.isYBottomInside = self.centerPos.y - self.spriteSize.y * 0.5f >= minY && self.centerPos.y - self.spriteSize.y * 0.5f <= maxY;
            }
            
            bool isInX = self.isXLeftInside || self.isXRightInside;
            bool isInY = self.isYTopInside || self.isYBottomInside;
            
            bool isNeedUpdate = !(isInX && isInY);
            if (isNeedUpdate)
                self.go.SetActive(false);
            
            return isNeedUpdate;
        }

        /// <summary>
        /// 不考虑整个sprite在屏幕之外的情况
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Vector2 GetRemain(this MapComponent self)
        {
            //摄像机宽高
            var cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            //摄像机边界
            float minX = cameraComponent.cameraBoundPos.x;
            float maxX = cameraComponent.cameraBoundPos.z;
            float minY = cameraComponent.cameraBoundPos.y;
            float maxY = cameraComponent.cameraBoundPos.w;


            float remainX = 0;
            float remainY = 0;

            bool isIgnoreX = self.spriteSize.x >= cameraComponent.cameraBoundSize.x;
            bool isIgnoreY = self.spriteSize.y >= cameraComponent.cameraBoundSize.y;
            if (isIgnoreX)
            {
                if (self.isXLeftInside || self.isXRightInside)
                {
                    remainX = Mathf.Abs(self.centerPos.x - cameraComponent.MainCameraTr.position.x);
                }
                else
                {
                    remainX = cameraComponent.cameraBoundSize.x;
                }
            }
            else
            {
                if (self.centerPos.x < minX)
                    remainX = self.centerPos.x + self.spriteSize.x * 0.5f - minX;
                else if (self.centerPos.x > maxX)
                    remainX = maxX - (self.centerPos.x - self.spriteSize.x * 0.5f);
                else
                    remainX = self.spriteSize.x;
            }

            if (isIgnoreY)
            {
                if (self.isYTopInside || self.isYBottomInside)
                {
                    remainY = Mathf.Abs(self.centerPos.y - cameraComponent.MainCameraTr.position.y);
                }
                else
                {
                    remainY = cameraComponent.cameraBoundSize.y;
                }
            }
            else
            {
                if (self.centerPos.y < minY)
                    remainY = self.centerPos.y + self.spriteSize.y * 0.5f - minY;
                else if (self.centerPos.y > maxY)
                    remainY = maxY - (self.centerPos.y - self.spriteSize.y * 0.5f);
                else
                    remainY = self.spriteSize.y;
            }

            self.remaind.x = remainX;
            self.remaind.y = remainY;
            return self.remaind;
        }

        public static void UpdatePos(this MapComponent self, float x, float y)
        {
            self.centerPos.x = x;
            self.centerPos.y = y;
            self.go.transform.localPosition = self.centerPos;
            if(!self.go.activeSelf)
                self.go.SetActive(true);
        }
    }
}