using System;
using System.Collections.Generic;
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
            // self.previewTilemap = self.GetParent<MapManagerComponent>().tileMap_Preview;
            self.InitMap().NoContext();
        }

        [EntitySystem]
        private static void Update(this ET.Client.MapDrawTileComponent self)
        {
            // if (!self.isBuilding)
            //     return;
            // if(!self.GetParent<MapManagerComponent_Client>().isMouseInGameView())
            //     return;
            // self.DrawDataPreview();
            // if (Input.GetMouseButtonDown(0) && !Stage.isTouchOnUI)
            // {
            //     if (self.isCanDraw)
            //     {
            //         self.DrawData();
            //     }
            //
            //     self.BuildFinished(self.isCanDraw);
            // }
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MapDrawTileComponent self)
        {
            // self.ClearTempData();
        }

        public static async ETTask InitMap(this ET.Client.MapDrawTileComponent self)
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
                    tilemap.SetTile(ve3Pos, tileBase);
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

        public static void GenerateTilesAroundCamera(this ET.Client.MapDrawTileComponent self)
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
                    Tile tile = self.defaultGroundTile_Grasslands;
                    self.tileMap_Floor.SetTile(cellPosition, tile);
                }
            }

            // 可选：优化性能，一次性更新所有 Tile
            self.tileMap_Floor.RefreshAllTiles();
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
                    throw new ArgumentOutOfRangeException(nameof(groupId), groupId, null);
            }
        }

        public static void Draw(this ET.Client.MapDrawTileComponent self, int2 tilePos)
        {
            if (!self.isCanDraw)
                return;
            //左下为原点
            for (int x = 0; x < self.cellSize.x; x++)
            {
                for (int y = 0; y < self.cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(tilePos.x + x, tilePos.y + y, 0);
                    self.targetTilemap.SetTile(pos, self.TileBase);
                }
            }

            self.targetTilemap.RefreshAllTiles();
        }

        public static void Draw(this ET.Client.MapDrawTileComponent self, Vector3Int tilePos)
        {
            if (!self.isCanDraw)
                return;
            //左下为原点

            for (int x = 0; x < self.cellSize.x; x++)
            {
                for (int y = 0; y < self.cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(tilePos.x + x, tilePos.y + y, 0);
                    self.targetTilemap.SetTile(pos, self.TileBase);
                }
            }

            self.targetTilemap.RefreshAllTiles();
        }

        /*public static void BuildStart(this ET.Client.MapDrawTileComponent self, TileItemConfig itemConfig, TileBase tileBase,
        Tilemap targetTilemap)
        {
            if (tileBase == null || targetTilemap == null)
            {
                Log.Error("BuildStart Data Error!!!");
                return;
            }
            self.isBuilding = true;
            self.Config = itemConfig;
            self.TileBase = tileBase;
            self.targetTilemap = targetTilemap;
            self.cellSize = new int2(self.Config.CellData[0], self.Config.CellData[1]);
        }



        public static void DrawDataPreview(this ET.Client.MapDrawTileComponent self)
        {
            self.ClearPreview();
            //未建造时实时显示当前建造地块可建造状态
            var cellPos_Mouse = self.GetMousePosition();
            //左下为原点
            for (int x = 0; x < self.cellSize.x; x++)
            {
                for (int y = 0; y < self.cellSize.y; y++)
                {
                    Vector3Int pos = new Vector3Int(cellPos_Mouse.x + x, cellPos_Mouse.y + y, 0);
                    bool isCanDraw = self.DrawPreview(pos);
                    if (self.isCanDraw && !isCanDraw)
                        self.isCanDraw = false;
                }
            }

            self.previewTilemap.RefreshAllTiles();
        }

        public static Vector3Int GetMousePosition(this ET.Client.MapDrawTileComponent self)
        {
            return self.GetParent<MapManagerComponent_Client>().GetMousePosition();
        }

        public static void Draw(this ET.Client.MapDrawTileComponent self, Vector3Int cellPos)
        {
            self.targetTilemap.SetTile(cellPos, self.TileBase);
        }

        public static bool DrawPreview(this ET.Client.MapDrawTileComponent self, Vector3Int cellPos)
        {
            bool isCanDraw = false;
            if (self.previewTilemap.HasTile(cellPos))
            {
                self.previewTilemap.SetTile(cellPos, self.GetParent<MapManagerComponent_Client>().tile_Red);
            }
            else
            {
                isCanDraw = true;
                self.previewTilemap.SetTile(cellPos, self.GetParent<MapManagerComponent_Client>().tile_Green);
            }

            self.previewTilesPos.Add(cellPos);
            return isCanDraw;
        }

        public static void ClearPreview(this ET.Client.MapDrawTileComponent self)
        {
            foreach (Vector3Int cellPos in self.previewTilesPos)
            {
                self.previewTilemap.SetTile(cellPos, null);
            }

            self.previewTilemap.RefreshAllTiles();
        }

        public static void BuildFinished(this ET.Client.MapDrawTileComponent self, bool success)
        {
            self.ClearTempData();
            self.GetParent<MapManagerComponent_Client>().BuildFinish(success);
        }

        public static void ClearTempData(this ET.Client.MapDrawTileComponent self)
        {
            self.ClearPreview();
            self.isBuilding = false;
            self.targetTilemap = null;
            self.BuildUpPos = default;
            self.cellSize = default;
            self.Config = null;
            self.TileBase = null;
            self.isCanDraw = false;
            self.previewTilesPos.Clear();
        }*/
    }
}