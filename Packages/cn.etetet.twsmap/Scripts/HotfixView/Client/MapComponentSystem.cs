using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapComponent))]
    [FriendOfAttribute(typeof(ET.Client.MapChildComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapComponent self)
        {
            self.MainCamera = Camera.main;
            self.MainCameraTr = self.MainCamera.transform;
            self.MapSpriteRenderer = GameObject.Find("MapSpriteRenderer").GetComponent<SpriteRenderer>();
            self.MapRoot = GameObject.Find("Map").transform;
            self.InitMap().NoContext();
        }

        public static async ETTask InitMap(this ET.Client.MapComponent self)
        {
            var gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            self.Sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>(self.ResPath + gameLevelConfig.MapResName);
            self.MapSpriteRenderer.sprite = self.Sprite;
            self.spriteSize = new Vector2(self.Sprite.bounds.size.x, self.Sprite.bounds.size.y);
            //初始化位置
            self.MapSpriteRenderer.transform.position = Vector3.zero + new Vector3(0, 0, 10f);
            CameraComponent cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            //铺地图
            int x = Mathf.CeilToInt(cameraComponent.cameraBoundSize.x / self.spriteSize.x);
            int y = Mathf.CeilToInt(cameraComponent.cameraBoundSize.y / self.spriteSize.y);
            self.UpdateMapChild(x, y, cameraComponent.cameraBoundPos.x, cameraComponent.cameraBoundPos.y);
            self.MapSpriteRenderer.gameObject.SetActive(false);
        }

        public static void UpdateMapChild(this ET.Client.MapComponent self, int x, int y, float rootX, float rootY)
        {
            Vector2 center_Child = Vector2.zero;
            for (int i = 0; i < x; i++)
            {
                float centerX_H = rootX + self.spriteSize.x * (i + 0.5f);
                for (int j = 0; j < y; j++)
                {
                    float centerY_Y = rootY + self.spriteSize.y * (j + 0.5f);
                    center_Child.x = centerX_H;
                    center_Child.y = centerY_Y;
                    self.CreateMapChild(center_Child);
                }
            }
        }

        public static void CreateMapChild(this ET.Client.MapComponent self, Vector2 center_Child)
        {
            MapChildComponent mapChildComponent = self.AddChild<MapChildComponent, Vector2, Sprite, Transform, GameObject, Vector2>(center_Child,
                self.Sprite,
                self.MapRoot, self.MapSpriteRenderer.gameObject, self.spriteSize);
            self.MapChildren.Add(mapChildComponent);
        }

        public static void CheckMapChild(this ET.Client.MapComponent self, int dirX, int dirY)
        {
            float totalShow_X = 0;
            float totalShow_Y = 0;
            int totalShow_X_Num = 0;
            int totalShow_Y_Num = 0;
            float posX = 0;
            float posY = 0;
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            bool isInitValue = false;
            using ListComponent<float> list_x = ListComponent<float>.Create();
            using ListComponent<float> list_y = ListComponent<float>.Create();
            Stack<EntityRef<MapChildComponent>> needUpdateChildrenStack = new Stack<EntityRef<MapChildComponent>>();
            int dontNeedUpdateChildrenCount = 0;
            foreach (MapChildComponent mapChild in self.MapChildren)
            {
                if (!mapChild.CheckNeedUpdate())
                {
                    posX = mapChild.centerPos.x;
                    posY = mapChild.centerPos.y;
                    if (isInitValue)
                    {
                        if (posX < minX)
                            minX = posX;
                        if (posX > maxX)
                            maxX = posX;
                        if (posY < minY)
                            minY = posY;
                        if (posY > maxY)
                            maxY = posY;
                    }
                    else
                    {
                        minX = posX;
                        maxX = posX;
                        minY = posY;
                        maxY = posY;
                        isInitValue = true;
                    }

                    if (!list_x.Contains(posX))
                    {
                        list_x.Add(posX);
                        totalShow_X += mapChild.GetRemain().x;
                    }

                    if (!list_y.Contains(posY))
                    {
                        list_y.Add(posY);
                        totalShow_Y += mapChild.GetRemain().y;
                    }

                    dontNeedUpdateChildrenCount++;

                    continue;
                }

                needUpdateChildrenStack.Push(mapChild);
            }

            totalShow_X_Num = list_x.Count;
            totalShow_Y_Num = list_y.Count;

            CameraComponent cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();

            bool checkX = totalShow_X >= cameraComponent.cameraBoundSize.x || Mathf.Abs(totalShow_X - cameraComponent.cameraBoundSize.x) <= 0.1f;
            bool checkY = totalShow_Y >= cameraComponent.cameraBoundSize.y || Mathf.Abs(totalShow_Y - cameraComponent.cameraBoundSize.y) <= 0.1f;

            if (checkX && checkY && dontNeedUpdateChildrenCount >= totalShow_X_Num * totalShow_Y_Num)
                return;

            float remainX = cameraComponent.cameraBoundSize.x - totalShow_X;
            float remainY = cameraComponent.cameraBoundSize.y - totalShow_Y;
            int x = Mathf.CeilToInt(remainX / self.spriteSize.x);
            int y = Mathf.CeilToInt(remainY / self.spriteSize.y);

            float createPosX = 0f;
            float createPosY = 0f;
            float updatePosX = 0f;
            float updatePosY = 0f;
            //向右铺
            if (dirX > 0)
            {
                //向上铺
                if (dirY > 0)
                {
                    createPosX = maxX;
                    createPosY = maxY;
                }
                //向下铺
                else
                {
                    createPosX = maxX;
                    createPosY = minY;
                }
            }
            //向左铺
            else
            {
                //向上铺
                if (dirY > 0)
                {
                    createPosX = minX;
                    createPosY = maxY;
                }
                //向下铺
                else
                {
                    createPosX = minX;
                    createPosY = minY;
                }
            }

            for (int i = 0; i < totalShow_X_Num; i++)
            {
                updatePosX = list_x[i];
                for (int j = 0; j < y; j++)
                {
                    updatePosY = dirY > 0 ? createPosY + self.spriteSize.y * (j + 1) : createPosY - self.spriteSize.y * (j + 1);
                    if (needUpdateChildrenStack.Count <= 0)
                    {
                        self.CreateMapChild(new Vector2(updatePosX, updatePosY));
                    }
                    else
                    {
                        MapChildComponent mapChildComponent = needUpdateChildrenStack.Pop();
                        mapChildComponent.UpdatePos(updatePosX, updatePosY);
                    }
                }
            }

            for (int i = 0; i < x; i++)
            {
                updatePosX = dirX > 0 ? createPosX + self.spriteSize.x * (i + 1) : createPosX - self.spriteSize.x * (i + 1);
                for (int j = 0; j < totalShow_Y_Num; j++)
                {
                    updatePosY = list_y[j];
                    if (needUpdateChildrenStack.Count <= 0)
                    {
                        self.CreateMapChild(new Vector2(updatePosX, updatePosY));
                    }
                    else
                    {
                        MapChildComponent mapChildComponent = needUpdateChildrenStack.Pop();
                        mapChildComponent.UpdatePos(updatePosX, updatePosY);
                    }
                }
            }

            for (int i = 0; i < x; i++)
            {
                updatePosX = dirX > 0 ? createPosX + self.spriteSize.x * (i + 1) : createPosX - self.spriteSize.x * (i + 1);
                for (int j = 0; j < y; j++)
                {
                    updatePosY = dirY > 0 ? createPosY + self.spriteSize.y * (j + 1) : createPosY - self.spriteSize.y * (j + 1);
                    if (needUpdateChildrenStack.Count <= 0)
                    {
                        self.CreateMapChild(new Vector2(updatePosX, updatePosY));
                    }
                    else
                    {
                        MapChildComponent mapChildComponent = needUpdateChildrenStack.Pop();
                        mapChildComponent.UpdatePos(updatePosX, updatePosY);
                    }
                }
            }
        }

        public static void RefreshMap(this ET.Client.MapComponent self, int dirX, int dirY)
        {
            self.CheckMapChild(dirX, dirY);
        }
    }
}