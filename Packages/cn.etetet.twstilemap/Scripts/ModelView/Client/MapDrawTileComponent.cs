using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapDrawTileComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public Tilemap tileMap_Floor { get; set; }
        public Tilemap tileMap_Build { get; set; }
        public Tilemap tileMap_Road { get; set; }
        public Tilemap tileMap_Water { get; set; }
        public Tilemap tileMap_Preview { get; set; }
        
        public Dictionary<TileType, List<TileBase>> tileBaseMap_Map { get; set; } = new Dictionary<TileType, List<TileBase>>();
        
        public Tile tile_Green { get; set; }
        public Tile tile_Red { get; set; }
        
        public string defaultSpriteAtlas = "FDR_Grasslands";
        public Tile defaultGroundTile_Grasslands;
        public Dictionary<string, Sprite> spritesDic = new Dictionary<string, Sprite>();
        
        public Camera Camera;
        
        
        public Tilemap previewTilemap;
        
        public bool isBuilding = false;
        public Tilemap targetTilemap;
        public Vector3Int BuildUpPos = default;
        public int2 cellSize = default;
        public TileItemConfig Config = null;
        public TileBase TileBase = null;
        public bool isCanDraw = false;
        public List<Vector3Int> previewTilesPos = new List<Vector3Int>();
        public DrawType curDrawType;
    }
}