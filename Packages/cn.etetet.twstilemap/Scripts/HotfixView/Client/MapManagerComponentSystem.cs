using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapManagerComponent))]
    public static partial class MapManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MapManagerComponent self)
        {
            self.InitMap().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MapManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this ET.Client.MapManagerComponent self)
        {
            int2 currentCell = self.GetMousePosition();
            if (Input.GetMouseButtonDown(0))
            {
                self.isDragging = true;
                self.CurrentCell = currentCell;
                self.cell_X = currentCell.x;
                self.cell_Y = currentCell.y;
                self.DrawBrush(currentCell, self.BrushSize);
            }

            if (self.isDragging && Input.GetMouseButton(0))
            {
                self.DrawBrushLine(self.CurrentCell, currentCell, self.BrushSize);
                self.CurrentCell = currentCell;
            }

            if (self.isDragging && Input.GetMouseButtonUp(0))
            {
                self.isDragging = false;
                self.FillRectangle();
                self.tileMap_Show.RefreshAllTiles();
            }

            if (Input.GetMouseButton(1))
                self.ClearTileAt(currentCell, self.tileMap_Show);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                self.RotateTileAt(self.CurrentCell);
            }
        }

        public static async ETTask InitMap(this ET.Client.MapManagerComponent self)
        {
            var referenceCollector = GameObject.Find("World").GetComponent<ReferenceCollector>();
            self.tileMap_BaseGo = referenceCollector.Get<GameObject>("Tilemap_Base");
            self.tileMap_BuildGo = referenceCollector.Get<GameObject>("Tilemap_Build");
            self.tileMap_ShowGo = referenceCollector.Get<GameObject>("Tilemap_Show");

            self.tileMap_Base = self.tileMap_BaseGo.GetComponent<Tilemap>();
            self.tileMap_Build = self.tileMap_BuildGo.GetComponent<Tilemap>();
            self.tileMap_Show = self.tileMap_ShowGo.GetComponent<Tilemap>();

            // self.tileMap_Base.layoutGrid.cellSize = new Vector3(8, 8, 0);
            // self.tileMap_Show.layoutGrid.cellSize = new Vector3(2.56f, 2.56f, 0);
            // self.tileMap_Build.layoutGrid.cellSize = new Vector3(1f, 1f, 0);
            self.Sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>("Tile_1");
            self.Sprite_Show = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>("Tile_2");
            self.Camera = Camera.main;

            self.GenerateTilesAroundCamera();
        }

        public static void GenerateTilesAroundCamera(this ET.Client.MapManagerComponent self)
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
                    self.CreateDefaultTile(cellPosition);
                }
            }

            // 可选：优化性能，一次性更新所有 Tile
            self.tileMap_Base.RefreshAllTiles();
        }

        public static Tile CreateDefaultTile(this ET.Client.MapManagerComponent self, Vector3Int pos)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = self.Sprite;
            tile.color = Color.white;
            // 只在空的位置放置 Tile
            if (!self.tileMap_Base.HasTile(pos))
            {
                self.tileMap_Base.SetTile(pos, tile);
            }

            return tile;
        }

        public static Tile CreateShowTile(this ET.Client.MapManagerComponent self, Vector3Int pos)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = self.Sprite_Show;
            tile.color = Color.yellow;
            // 只在空的位置放置 Tile
            if (!self.tileMap_Show.HasTile(pos))
            {
                self.tileMap_Show.SetTile(pos, tile);
            }

            return tile;
        }

        public static int2 GetMousePosition(this ET.Client.MapManagerComponent self)
        {
            Camera camera = Camera.main;
            var mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            var cellPos = self.tileMap_Show.WorldToCell(mouseWorldPosition);
            return new int2(cellPos.x, cellPos.y);
        }

        public static void ClearTileAt(this ET.Client.MapManagerComponent self, int2 cell, Tilemap tilemap)
        {
            Vector3Int cellPos = new Vector3Int(cell.x, cell.y, 0);
            if (tilemap.HasTile(cellPos))
                tilemap.SetTile(cellPos, null);
        }

        //绘制笔刷
        public static void DrawBrush(this ET.Client.MapManagerComponent self, int2 center, int brushSize)
        {
            for (int x = -brushSize + 1; x < brushSize; x++)
            {
                for (int y = -brushSize + 1; y < brushSize; y++)
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

        // 矩形预览（可以用于显示半透明预览）
        public static void PreviewRectangle(this ET.Client.MapManagerComponent self, int2 start, int2 end)
        {
            // 实现预览逻辑，可以使用临时 tile 或者颜色变换
        }

        // 擦除直线
        public static void EraseLine(this ET.Client.MapManagerComponent self, int2 from, int2 to)
        {
            var points = self.BresenhamLine(from.x, from.y, to.x, to.y);

            foreach (var point in points)
            {
                self.ClearTileAt(point, self.tileMap_Show);
            }
        }

        // 擦除单个 tile
        public static void EraseTile(this ET.Client.MapManagerComponent self, int2 cell)
        {
            self.ClearTileAt(cell, self.tileMap_Show);
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
            if (self.tileMap_Show.HasTile(pos))
            {
                Matrix4x4 oldMatrix = self.tileMap_Show.GetTransformMatrix(pos);
                Matrix4x4 newMatrix = oldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90));
                self.tileMap_Show.SetTransformMatrix(pos, newMatrix);
            }
        }
    }
}