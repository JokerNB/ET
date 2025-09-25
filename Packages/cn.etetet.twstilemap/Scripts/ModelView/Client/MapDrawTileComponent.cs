using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapDrawTileComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public Tilemap tileMap_Base { get; set; }
        public Tilemap tileMap_Floor { get; set; }
        public Tilemap tileMap_Build { get; set; }
        public Tilemap tileMap_Road { get; set; }
        public Tilemap tileMap_Water { get; set; }
        public Tilemap tileMap_Preview { get; set; }
        
        public Dictionary<int, TileBase> ConfigId_tileBaseDic { get; set; } = new Dictionary<int, TileBase>();
        
        public Tile tile_Green { get; set; }
        public Tile tile_Red { get; set; }
        
        public string defaultSpriteAtlas = "FDR_Grasslands";
        public Tile defaultGroundTile_Grasslands;
        public Dictionary<string, Sprite> spritesDic = new Dictionary<string, Sprite>();
        
        public Camera Camera;
        
        //curdata
        public bool isBuilding = false;
        public Tilemap targetTilemap;
        public Vector3Int BuildStartPos = default;
        public int2 cellSize = default;
        public TileItemConfig curConfig = null;
        public TileBase curTileBase = null;
        public bool isCanDraw = false;
        public List<Vector3Int> previewTilesPos = new List<Vector3Int>();
        public List<Vector3Int> targetTilesPos = new List<Vector3Int>();
        public DrawType curDrawType = DrawType.None;
        public Vector3Int dragStartPos = default;
        public bool isDrag = false;
    }
}