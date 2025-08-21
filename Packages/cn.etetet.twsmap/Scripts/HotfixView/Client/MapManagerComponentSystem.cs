using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapManagerComponent))]
    [FriendOfAttribute(typeof(ET.Client.MapComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MapManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapManagerComponent self)
        {
            var referenceCollector = GameObject.Find("World").GetComponent<ReferenceCollector>();
            self.MapRootTr = referenceCollector.Get<Transform>("MapRoot");
            self.MapNav = referenceCollector.Get<PolyNav2D>("PolyNav2D");
            self.MapNavCollider = self.MapNav.gameObject.GetComponent<PolygonCollider2D>();
            self.InitMap().NoContext();
        }

        public static async ETTask InitMap(this ET.Client.MapManagerComponent self)
        {
            var gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            self.spriteAsset = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>(self.ResPath + gameLevelConfig.MapResName);
            self.goAsset = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>("MapSpriteRenderer");
            self.spriteSize = new Vector2(self.spriteAsset.bounds.size.x, self.spriteAsset.bounds.size.y);
            self.UpdateMapChild();
        }

        public static void UpdateMapChild(this ET.Client.MapManagerComponent self)
        {
            CameraComponent cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            //铺地图
            int x = Mathf.CeilToInt(cameraComponent.cameraBoundSize.x / self.spriteSize.x);
            int y = Mathf.CeilToInt(cameraComponent.cameraBoundSize.y / self.spriteSize.y);
            float rootX = cameraComponent.cameraBoundPos.x;
            float rootY = cameraComponent.cameraBoundPos.y;
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
            
            self.RefreshNav();
        }

        public static void CreateMapChild(this ET.Client.MapManagerComponent self, Vector2 center_Child)
        {
            MapComponent mapComponent = self.AddChild<MapComponent, Vector2>(center_Child);
            self.MapChildren.Add(mapComponent);
        }

        public static void CheckMapChild(this ET.Client.MapManagerComponent self, int dirX, int dirY)
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
            Stack<EntityRef<MapComponent>> needUpdateChildrenStack = new Stack<EntityRef<MapComponent>>();
            int dontNeedUpdateChildrenCount = 0;
            foreach (MapComponent mapChild in self.MapChildren)
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
                        MapComponent mapComponent = needUpdateChildrenStack.Pop();
                        mapComponent.UpdatePos(updatePosX, updatePosY);
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
                        MapComponent mapComponent = needUpdateChildrenStack.Pop();
                        mapComponent.UpdatePos(updatePosX, updatePosY);
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
                        MapComponent mapComponent = needUpdateChildrenStack.Pop();
                        mapComponent.UpdatePos(updatePosX, updatePosY);
                    }
                }
            }
        }

        public static void RefreshMap(this ET.Client.MapManagerComponent self, int dirX, int dirY)
        {
            self.CheckMapChild(dirX, dirY);
            self.RefreshNav();
        }

        public static void RefreshNav(this ET.Client.MapManagerComponent self)
        {
            //TODO:根据地图资源设置可行走区域
            //先根据照相机位置设置可行走区域
            var cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            var cameraBoundPos = cameraComponent.cameraBoundPos;
            float offset = 20;
            List<Vector2> points = new List<Vector2>()
            {
                new Vector2(cameraBoundPos.x - offset, cameraBoundPos.y - offset), //左下
                new Vector2(cameraBoundPos.x - offset, cameraBoundPos.w + offset), //左上
                new Vector2(cameraBoundPos.z + offset, cameraBoundPos.w + offset), //右上
                new Vector2(cameraBoundPos.z + offset, cameraBoundPos.y - offset) //右下
            };
            self.MapNavCollider.SetPath(0, points);
            self.MapNav.GenerateMap(true);
        }
    }
}