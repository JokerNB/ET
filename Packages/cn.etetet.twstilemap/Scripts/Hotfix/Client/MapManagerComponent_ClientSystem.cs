using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.MapTileInfo))]
    [FriendOfAttribute(typeof(ET.Client.ArchiveInfoManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.ArchiveInfo))]
    public static partial class MapManagerComponent_ClientSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapManagerComponent_Client self)
        {
            // self.InitMap().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MapManagerComponent_Client self)
        {
            // UnityEngine.Object.DestroyImmediate(self.tile_Green);
            // UnityEngine.Object.DestroyImmediate(self.tile_Red);
        }

        public static void InitMapData(this ET.Client.MapManagerComponent_Client self)
        {
            ArchiveInfo curArchiveInfo = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>().GetCurArchiveInfo();

            if(curArchiveInfo == null)
                return;
            foreach (MapTileInfo mapTileInfo in curArchiveInfo.MapTileInfos)
            {
                self.ConfigID_MapTileInfoDic.Add(mapTileInfo.configId, mapTileInfo);
            }
        }

        public static void BuildFinished(this ET.Client.MapManagerComponent_Client self, int configId, List<int2> tilePos)
        {
            if (self.ConfigID_MapTileInfoDic.ContainsKey(configId))
            {
                MapTileInfo mapTileInfo = self.ConfigID_MapTileInfoDic[configId];
                mapTileInfo.UpdateTilePos(tilePos);
            }
            else
            {
                MapTileInfo mapTileInfo = self.Root().GetComponent<ArchiveInfoManagerComponent_Client>().AddNewMapTileByCurArchive(configId, tilePos);
                self.ConfigID_MapTileInfoDic.Add(mapTileInfo.configId, mapTileInfo);
            }
        }

        /*public static async ETTask InitMap(this ET.Client.MapManagerComponent_Client self)
        {
            var referenceCollector = GameObject.Find("World").GetComponent<ReferenceCollector>();

            self.tileMap_Floor = referenceCollector.Get<Tilemap>("Tilemap_Floor");
            self.tileMap_Build = referenceCollector.Get<Tilemap>("Tilemap_Build");
            self.tileMap_Road = referenceCollector.Get<Tilemap>("Tilemap_Road");
            self.tileMap_Water = referenceCollector.Get<Tilemap>("Tilemap_Water");
            self.tileMap_Preview = referenceCollector.Get<Tilemap>("Tilemap_Preview");

            self.tile_Green = ScriptableObject.CreateInstance<Tile>();
            self.tile_Green.color = Color.green;
            self.tile_Red = ScriptableObject.CreateInstance<Tile>();
            self.tile_Red.color = Color.red;

            self.spritesDic = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadSubAssetAsync<Sprite>(self.defaultSpriteAtlas);

            self.defaultGroundTile_Grasslands =
                    await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Tile>("Floor_Base");
            self.Camera = Camera.main;

            self.GenerateTilesAroundCamera();

            var single = self.AddChild<MapDrawTileComponent>();
            self.singleID = single.Id;
            var ring = self.AddChild<MapDraw_RingComponent>();
            self.ringID = ring.Id;
            var rectangle = self.AddChild<MapDraw_RectangleComponent>();
            self.rectangleID = rectangle.Id;
            var drag = self.AddChild<MapDraw_DragComponent>();
            self.dragID = drag.Id;

            EventSystem.Instance.Publish(self.Root().CurrentScene(), new SetLoadingValue
            {
                Value = 5,
                isComplete = true,
            });
        }

        public static void GenerateTilesAroundCamera(this ET.Client.MapManagerComponent_Client self)
        {
            // 获取相机世界坐标下的可视范围
            Vector3 bottomLeft = self.Camera.ScreenToWorldPoint(new Vector3(0, 0, self.Camera.orthographicSize));
            Vector3 topRight = self.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, self.Camera.orthographicSize));

            // 计算需要覆盖的网格范围
            Vector3Int minCell = self.tileMap_Floor.WorldToCell(bottomLeft);
            Vector3Int maxCell = self.tileMap_Floor.WorldToCell(topRight);

            // 计算需要填充的单元格数量（+1 确保完全覆盖）
            int numCellsX = maxCell.x - minCell.x + 1;
            int numCellsY = maxCell.y - minCell.y + 1;

            // 填充 Tilemap
            for (int x = 0; x < numCellsX; x++)
            {
                for (int y = 0; y < numCellsY; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(minCell.x + x, minCell.y + y, 0);
                    self.CreateDefaultTile(cellPosition);
                }
            }

            // 可选：优化性能，一次性更新所有 Tile
            self.tileMap_Floor.RefreshAllTiles();
        }

        public static void CreateDefaultTile(this ET.Client.MapManagerComponent_Client self, Vector3Int pos)
        {
            Tile tile = self.defaultGroundTile_Grasslands;
            self.tileMap_Floor.SetTile(pos, tile);
        }

        public static Vector3Int GetMousePosition(this ET.Client.MapManagerComponent_Client self)
        {
            var mouseWorldPosition = self.Camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            var cellPos = self.tileMap_Floor.WorldToCell(mouseWorldPosition);
            return new Vector3Int(cellPos.x, cellPos.y);
        }

        /*public static void ClearTileAt(this ET.Client.MapManagerComponent self, int2 cell, Tilemap tilemap)
        {
            Vector3Int cellPos = new Vector3Int(cell.x, cell.y, 0);
            if (tilemap.HasTile(cellPos))
                tilemap.SetTile(cellPos, null);
        }

        //绘制笔刷
        public static void DrawBrush(this ET.Client.MapManagerComponent self, int2 center, int2 cellData)
        {
            for (int x = -cellData.x + 1; x < cellData.x; x++)
            {
                for (int y = -cellData.y + 1; y < cellData.y; y++)
                {
                    int2 targetCell = center + new int2(x, y);
                    if (targetCell.x < self.cell_X.x)
                        self.cell_X.x = targetCell.x;
                    if (targetCell.x > self.cell_X.y)
                        self.cell_X.y = targetCell.x;
                    if (targetCell.y < self.cell_Y.x)
                        self.cell_Y.x = targetCell.y;
                    if (targetCell.y > self.cell_Y.y)
                        self.cell_Y.y = targetCell.y;
                    Vector3Int pos = new Vector3Int(targetCell.x, targetCell.y, 0);
                    self.CreateShowTile(pos);
                }
            }
        }

        //绘制笔刷Preview
        public static void DrawBrushPreview(this ET.Client.MapManagerComponent self, int2 center, int2 cellData)
        {
            for (int x = -cellData.x + 1; x < cellData.x; x++)
            {
                for (int y = -cellData.y + 1; y < cellData.y; y++)
                {
                    int2 targetCell = center + new int2(x, y);
                    if (targetCell.x < self.cell_X.x)
                        self.cell_X.x = targetCell.x;
                    if (targetCell.x > self.cell_X.y)
                        self.cell_X.y = targetCell.x;
                    if (targetCell.y < self.cell_Y.x)
                        self.cell_Y.x = targetCell.y;
                    if (targetCell.y > self.cell_Y.y)
                        self.cell_Y.y = targetCell.y;
                    Vector3Int pos = new Vector3Int(targetCell.x, targetCell.y, 0);
                    self.CreateShowTile(pos);
                }
            }
        }

        // 绘制直线
        public static void DrawBrushLine(this ET.Client.MapManagerComponent self, int2 from, int2 to, int brushSize)
        {
            // 使用 Bresenham 算法绘制直线
            var points = self.BresenhamLine(from.x, from.y, to.x, to.y);

            foreach (var point in points)
            {
                self.DrawBrush(new int2(point.x, point.y), brushSize);
            }
        }

        // 绘制矩形
        public static void FillRectangle(this ET.Client.MapManagerComponent self)
        {
            int minX = self.cell_X.x;
            int maxX = self.cell_X.y;
            int minY = self.cell_Y.x;
            int maxY = self.cell_Y.y;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    self.CreateShowTile(pos);
                }
            }
        }

        public static void FillRectangle_Preview(this ET.Client.MapManagerComponent self)
        {
            int minX = self.StartCell.x < self.CurrentCell.x ? self.StartCell.x : self.CurrentCell.x;
            int maxX = self.StartCell.x > self.CurrentCell.x ? self.StartCell.x : self.CurrentCell.x;
            int minY = self.StartCell.y < self.CurrentCell.y ? self.StartCell.y : self.CurrentCell.y;
            int maxY = self.StartCell.y > self.CurrentCell.y ? self.StartCell.y : self.CurrentCell.y;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (self.curTilemap.HasTile(pos))
                    {
                        var tile = self.tileMap_Preview.GetTile(pos) as Tile;
                        tile.color = Color.red;
                    }
                    else
                    {
                        var tile = self.tileMap_Floor.GetTile(pos) as Tile;
                        tile.color = Color.green;
                    }

                    self.tileMap_Floor.RefreshTile(pos);
                }
            }
        }

        // 擦除直线
        public static void EraseLine(this ET.Client.MapManagerComponent self, int2 from, int2 to)
        {
            var points = self.BresenhamLine(from.x, from.y, to.x, to.y);

            foreach (var point in points)
            {
                self.ClearTileAt(point, self.tileMap_Build);
            }
        }

        // 擦除单个 tile
        public static void EraseTile(this ET.Client.MapManagerComponent self, int2 cell)
        {
            self.ClearTileAt(cell, self.tileMap_Build);
        }

        // Bresenham 直线算法
        private static List<int2> BresenhamLine(this ET.Client.MapManagerComponent self, int x0, int y0, int x1, int y1)
        {
            List<int2> points = new List<int2>();

            int dx = math.abs(x1 - x0);
            int dy = math.abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                points.Add(new int2(x0, y0));

                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            return points;
        }

        private static void RotateTileAt(this ET.Client.MapManagerComponent self, int2 cell)
        {
            Vector3Int pos = new Vector3Int(cell.x, cell.y, 0);
            if (self.tileMap_Build.HasTile(pos))
            {
                Matrix4x4 oldMatrix = self.tileMap_Build.GetTransformMatrix(pos);
                Matrix4x4 newMatrix = oldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90));
                self.tileMap_Build.SetTransformMatrix(pos, newMatrix);
            }
        }#1#

        public static bool isMouseInGameView(this ET.Client.MapManagerComponent_Client self)
        {
            var viewportPoint = self.Camera.ScreenToViewportPoint(Input.mousePosition);
            return viewportPoint is { x: >= 0 and <= 1, y: >= 0 and <= 1 };
        }

        public static async ETTask BuildStart(this ET.Client.MapManagerComponent_Client self, TileItemConfig itemConfig)
        {
            if (string.IsNullOrEmpty(itemConfig.PrefabDataResName))
                return;
            TileBase tileBase = await self.LoadTile(itemConfig.PrefabDataResName, itemConfig.PrefabDataResType);
            Tilemap targetTilemap = self.GetTargetTilemap(itemConfig.LandType);

            switch (itemConfig.PrefabDataResType)
            {
                case TileType.Tile:
                    self.GetChild<MapDrawTileComponent>(self.singleID).BuildStart(itemConfig, tileBase, targetTilemap);
                    break;
                case TileType.RuleTile:
                    break;
                case TileType.CustomRuleTile:
                    break;
                case TileType.Sprite:
                    self.GetChild<MapDrawTileComponent>(self.singleID).BuildStart(itemConfig, tileBase, targetTilemap);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Tilemap GetTargetTilemap(this ET.Client.MapManagerComponent_Client self, LandType landType)
        {
            switch (landType)
            {
                case LandType.Floor:
                    return self.tileMap_Floor;
                case LandType.Build:
                    return self.tileMap_Build;
                case LandType.Road:
                    return self.tileMap_Road;
                case LandType.Water:
                    return self.tileMap_Water;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static async ETTask<TileBase> LoadTile(this ET.Client.MapManagerComponent_Client self, string name, TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Tile:
                    return await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Tile>(name);
                case TileType.RuleTile:
                    return await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<RuleTile>(name);
                case TileType.CustomRuleTile:
                    return await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<CustomRuleTile_Ring>(name);
                case TileType.Sprite:
                    if (self.spritesDic.ContainsKey(name))
                    {
                        var tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = self.spritesDic[name];
                        tile.color = Color.white;
                        return tile;
                    }

                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }
        }

        public static void BuildFinish(this MapManagerComponent_Client self, bool isBuildSuccess)
        {
        }*/
    }
}