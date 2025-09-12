using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapManagerComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public Tilemap tileMap_Base { get; set; }
        public Tilemap tileMap_Build { get; set; }
        public Tilemap tileMap_Show { get; set; }
        
        public GameObject tileMap_BaseGo { get; set; }
        public GameObject tileMap_BuildGo { get; set; }
        public GameObject tileMap_ShowGo { get; set; }
        
        public TileBase curTileBase_Build { get; set; }
        
        public bool isDragging = false;
        
        public int2 CurrentCell;
        public int BrushSize = 1;

        public Sprite Sprite;
        public Sprite Sprite_Show;
        public Camera Camera;

        public int2 cell_X;
        public int2 cell_Y;
    }
}