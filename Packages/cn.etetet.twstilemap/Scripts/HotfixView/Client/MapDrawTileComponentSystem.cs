using System;
using System.Collections.Generic;
using FairyGUI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapDrawTileComponent))]
    [FriendOfAttribute(typeof(ET.Client.MapManagerComponent_Client))]
    [FriendOfAttribute(typeof(ET.MapTileInfo))]
    public static partial class MapDrawTileComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapDrawTileComponent self)
        {
            self.InitMap().NoContext();
        }

        [EntitySystem]
        private static void Update(this ET.Client.MapDrawTileComponent self)
        {
            if (!self.isBuilding)
                return;
            if (Stage.isTouchOnUI)
                return;
            if (!self.isMouseInGameView())
                return;
            switch (self.curDrawType)
            {
                case DrawType.Single:
                    self.DrawInPreviewBySingle(false);
                    if (Input.GetMouseButtonDown(0) && self.isCanDraw)
                    {
                        if (self.isCanDraw)
                        {
                            self.DrawInTargetTileMapBySingle();
                            self.BuildFinished();
                        }
                        else
                        {
                            self.CancelBuild();
                        }
                    }

                    break;
                case DrawType.Drag:
                    if (!self.isDrag)
                        self.DrawInPreviewBySingle(false);
                    if (Input.GetMouseButtonDown(0))
                    {
                        self.isDrag = true;
                        self.DrawInPreviewBySingle(true);
                    }

                    if (Input.GetMouseButton(0))
                    {
                        self.Drag_FillRectangle(true);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        if (self.isCanDraw)
                        {
                            self.Drag_FillRectangle(false);
                            self.BuildFinished();
                        }
                        else
                        {
                            self.CancelBuild();
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Input.GetMouseButtonDown(1))
                self.CancelBuild();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MapDrawTileComponent self)
        {
            self.ClearTempData();
        }

        public static async ETTask InitMap(this ET.Client.MapDrawTileComponent self)
        {
            var referenceCollector = GameObject.Find("World").GetComponent<ReferenceCollector>();

            self.tileMap_Base = referenceCollector.Get<Tilemap>("Tilemap_Base");
            self.tileMap_Floor = referenceCollector.Get<Tilemap>("Tilemap_Floor");
            self.tileMap_Build = referenceCollector.Get<Tilemap>("Tilemap_Build");
            self.tileMap_Road = referenceCollector.Get<Tilemap>("Tilemap_Road");
            self.tileMap_Water = referenceCollector.Get<Tilemap>("Tilemap_Water");
            self.tileMap_Preview = referenceCollector.Get<Tilemap>("Tilemap_Preview");

            self.spritesDic = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadSubAssetAsync<Sprite>(self.defaultSpriteAtlas);

            self.defaultGroundTile_Grasslands =
                    await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Tile>("Floor_Base");
            self.tile_Green = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Tile>("Floor_Preview_Green");
            self.tile_Red = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Tile>("Floor_Preview_Red");
            self.Camera = Camera.main;

            self.GenerateTilesAroundCamera();
        }

        public static async ETTask DrawByInit(this ET.Client.MapDrawTileComponent self)
        {
            var managerComponentClient = self.Root().GetComponent<MapManagerComponent_Client>();
            HashSet<Tilemap> tilemaps = new HashSet<Tilemap>();
            foreach (MapTileInfo mapTileInfo in managerComponentClient.ConfigID_MapTileInfoDic.Values)
            {
                var tileData = mapTileInfo.Config.TileData;
                TileBase tileBase = await self.LoadTile(tileData.ResName, tileData.TileType);
                var tilemap = self.GetTargetTilemapByGroupId(mapTileInfo.Config.GroupID);
                if (tileBase == null || tilemap == null)
                    continue;
                foreach (int2 pos in mapTileInfo.tilePos)
                {
                    var ve3Pos = new Vector3Int(pos.x, pos.y, 0);
                    switch (mapTileInfo.Config.TileData.DrawType)
                    {
                        case DrawType.Single:
                            self.DrawInTargetTileMapSingleByInit(ve3Pos, mapTileInfo.tileSize, tileBase, tilemap);
                            break;
                        case DrawType.Drag:
                            tilemap.SetTile(ve3Pos, tileBase);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                tilemaps.Add(tilemap);
            }

            foreach (Tilemap tilemap in tilemaps)
            {
                tilemap.RefreshAllTiles();
            }

            EventSystem.Instance.Publish(self.Root().CurrentScene(), new SetLoadingValue
            {
                Value = 5,
                isComplete = true,
            });
        }
        
        public static void DrawInTargetTileMapSingleByInit(this ET.Client.MapDrawTileComponent self, Vector3Int buildStartPos, int2 cellSize, TileBase tileBase, Tilemap targetTilemap)
        {
            for (int x = 0; x < cellSize.x; x++)
            {
                for (int y = 0; y < cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(buildStartPos.x + x, buildStartPos.y + y, 0);
                    targetTilemap.SetTile(pos, tileBase);
                }
            }
            targetTilemap.RefreshAllTiles();
        }

        public static void GenerateTilesAroundCamera(this ET.Client.MapDrawTileComponent self)
        {
            // 获取相机世界坐标下的可视范围
            Vector3 bottomLeft = self.Camera.ScreenToWorldPoint(new Vector3(0, 0, self.Camera.orthographicSize));
            Vector3 topRight = self.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, self.Camera.orthographicSize));

            // 计算需要覆盖的网格范围
            Vector3Int minCell = self.tileMap_Base.WorldToCell(bottomLeft);
            Vector3Int maxCell = self.tileMap_Base.WorldToCell(topRight);

            // 计算需要填充的单元格数量（+1 确保完全覆盖）
            int numCellsX = maxCell.x - minCell.x + 1;
            int numCellsY = maxCell.y - minCell.y + 1;

            // 填充 Tilemap
            for (int x = 0; x < numCellsX; x++)
            {
                for (int y = 0; y < numCellsY; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(minCell.x + x, minCell.y + y, 0);
                    Tile tile = self.defaultGroundTile_Grasslands;
                    self.tileMap_Base.SetTile(cellPosition, tile);
                }
            }

            // 可选：优化性能，一次性更新所有 Tile
            self.tileMap_Base.RefreshAllTiles();
        }

        public static async ETTask<TileBase> LoadTile(this ET.Client.MapDrawTileComponent self, string name, TileType tileType)
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

        public static Tilemap GetTargetTilemapByGroupId(this ET.Client.MapDrawTileComponent self, int groupId)
        {
            switch (groupId)
            {
                case 1:
                    return self.tileMap_Road;
                case 2:
                    return self.tileMap_Floor;
                case 3:
                    return self.tileMap_Build;
                default:
                    return null;
            }
        }

        public static Vector3Int GetMousePosition(this ET.Client.MapDrawTileComponent self)
        {
            var mouseWorldPosition = self.Camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            var cellPos = self.tileMap_Base.WorldToCell(mouseWorldPosition);
            return new Vector3Int(cellPos.x, cellPos.y);
        }

        public static void DrawInPreviewBySingle(this ET.Client.MapDrawTileComponent self, bool needSetDragStartPos)
        {
            self.ClearPreview();
            //未建造时实时显示当前建造地块可建造状态
            var cellPos_Mouse = self.GetMousePosition();
            if (needSetDragStartPos)
                self.dragStartPos = cellPos_Mouse;
            self.BuildStartPos = cellPos_Mouse;
            self.isCanDraw = true;
            //左下为原点
            for (int x = 0; x < self.cellSize.x; x++)
            {
                for (int y = 0; y < self.cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(cellPos_Mouse.x + x, cellPos_Mouse.y + y, 0);
                    if (self.targetTilemap.HasTile(pos))
                    {
                        self.tileMap_Preview.SetTile(pos, self.tile_Red);
                        if (self.isCanDraw)
                            self.isCanDraw = false;
                    }
                    else
                    {
                        self.tileMap_Preview.SetTile(pos, self.tile_Green);
                    }

                    self.previewTilesPos.Add(pos);
                }
            }

            self.tileMap_Preview.RefreshAllTiles();
        }

        public static void ClearPreview(this ET.Client.MapDrawTileComponent self)
        {
            foreach (Vector3Int cellPos in self.previewTilesPos)
            {
                self.tileMap_Preview.SetTile(cellPos, null);
            }

            self.tileMap_Preview.RefreshAllTiles();
            self.previewTilesPos.Clear();
        }

        public static void DrawInTargetTileMapBySingle(this ET.Client.MapDrawTileComponent self)
        {
            var buildStartPos = self.BuildStartPos;
            for (int x = 0; x < self.cellSize.x; x++)
            {
                for (int y = 0; y < self.cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(buildStartPos.x + x, buildStartPos.y + y, 0);
                    self.targetTilemap.SetTile(pos, self.curTileBase);
                }
            }
            self.targetTilesPos.Add(buildStartPos);
            self.targetTilemap.RefreshAllTiles();
        }

        public static bool isMouseInGameView(this ET.Client.MapDrawTileComponent self)
        {
            var viewportPoint = self.Camera.ScreenToViewportPoint(Input.mousePosition);
            return viewportPoint is { x: >= 0 and <= 1, y: >= 0 and <= 1 };
        }

        // 绘制矩形
        public static void Drag_FillRectangle(this ET.Client.MapDrawTileComponent self, bool isPreview)
        {
            if(isPreview)
                self.ClearPreview();
            var mousePos = self.GetMousePosition();
            int minX = self.dragStartPos.x < mousePos.x ? self.dragStartPos.x : mousePos.x;
            int maxX = self.dragStartPos.x > mousePos.x ? self.dragStartPos.x : mousePos.x;
            int minY = self.dragStartPos.y < mousePos.y ? self.dragStartPos.y : mousePos.y;
            int maxY = self.dragStartPos.y > mousePos.y ? self.dragStartPos.y : mousePos.y;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (isPreview)
                    {
                        if (self.targetTilemap.HasTile(pos))
                        {
                            self.tileMap_Preview.SetTile(pos, self.tile_Red);
                            if (self.isCanDraw)
                                self.isCanDraw = false;
                        }
                        else
                        {
                            self.tileMap_Preview.SetTile(pos, self.tile_Green);
                        }

                        self.previewTilesPos.Add(pos);
                    }
                    else
                    {
                        self.targetTilemap.SetTile(pos, self.curTileBase);
                        self.targetTilesPos.Add(pos);
                    }
                }
            }

            if (isPreview)
                self.tileMap_Preview.RefreshAllTiles();
            else
                self.targetTilemap.RefreshAllTiles();
        }

        public static void BuildStart(this ET.Client.MapDrawTileComponent self, int configId)
        {
            self.BuildStart_PrepareDataAsync(configId).NoContext();
        }

        public static async ETTask BuildStart_PrepareDataAsync(this ET.Client.MapDrawTileComponent self, int configId)
        {
            var config = TileItemConfigCategory.Instance.Get(configId);
            if (config == null)
                return;
            self.curConfig = config;
            var tileData = config.TileData;
            self.curDrawType = tileData.DrawType;
            if (self.curDrawType == DrawType.None)
            {
                self.ClearTempData();
                return;
            }
            if (self.ConfigId_tileBaseDic.TryGetValue(configId, out TileBase value))
            {
                self.curTileBase = value;
            }
            else
            {
                TileBase tileBase = await self.LoadTile(tileData.ResName, tileData.TileType);
                if (tileBase == null)
                {
                    self.ClearTempData();
                    return;
                }
                self.ConfigId_tileBaseDic.Add(configId, tileBase);
                self.curTileBase = tileBase;
            }

            var targetTilemap = self.GetTargetTilemapByGroupId(config.GroupID);
            if (targetTilemap == null)
            {
                self.ClearTempData();
                return;
            }
            self.targetTilemap = targetTilemap;
            self.cellSize = new int2(self.curConfig.CellData[0], self.curConfig.CellData[1]);
            if (self.curDrawType == DrawType.Drag && (self.cellSize.x != 1 || self.cellSize.y != 1))
            {
                self.ClearTempData();
                return;
            }

            self.isBuilding = true;
        }

        public static void CancelBuild(this ET.Client.MapDrawTileComponent self)
        {
            self.ClearTempData();
        }

        public static void BuildFinished(this ET.Client.MapDrawTileComponent self)
        {
            List<int2> tilePos = new List<int2>();
            foreach (Vector3Int vector3Int in self.targetTilesPos)
            {
                int2 pos = new int2(vector3Int.x, vector3Int.y);
                tilePos.Add(pos);
            }

            self.Root().GetComponent<MapManagerComponent_Client>().BuildFinished(self.curConfig.Id, tilePos);
            self.ClearTempData();
        }

        public static void ClearTempData(this ET.Client.MapDrawTileComponent self)
        {
            self.ClearPreview();
            self.isBuilding = false;
            self.targetTilemap = null;
            self.BuildStartPos = default;
            self.cellSize = default;
            self.curConfig = null;
            self.curTileBase = null;
            self.isCanDraw = false;
            self.targetTilesPos.Clear();
            self.curDrawType = DrawType.None;
            self.dragStartPos = default;
            self.isDrag = false;
        }

        /*
        // 绘制直线
        public static void DrawBrushLine(this ET.Client.MapDrawTileComponent self, int2 from, int2 to, int brushSize)
        {
            // 使用 Bresenham 算法绘制直线
            var points = self.BresenhamLine(from.x, from.y, to.x, to.y);

            foreach (var point in points)
            {
                self.DrawBrush(new int2(point.x, point.y), brushSize);
            }
        }

        // 擦除直线
        public static void EraseLine(this ET.Client.MapDrawTileComponent self, int2 from, int2 to)
        {
            var points = self.BresenhamLine(from.x, from.y, to.x, to.y);

            foreach (var point in points)
            {
                self.ClearTileAt(point, self.tileMap_Build);
            }
        }

        // 擦除单个 tile
        public static void EraseTile(this ET.Client.MapDrawTileComponent self, int2 cell)
        {
            self.ClearTileAt(cell, self.tileMap_Build);
        }

        // Bresenham 直线算法
        private static List<int2> BresenhamLine(this ET.Client.MapDrawTileComponent self, int x0, int y0, int x1, int y1)
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
        }*/
    }
}