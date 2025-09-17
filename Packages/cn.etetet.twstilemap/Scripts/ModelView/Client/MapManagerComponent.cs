using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapManagerComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public Tilemap tileMap_Floor { get; set; }
        public Tilemap tileMap_Build { get; set; }
        
        public GameObject tileMap_FloorGo { get; set; }
        public GameObject tileMap_BuildGo { get; set; }
        
        public bool isDragging = false;
        
        public int2 CurrentCell;
        public int BrushSize = 1;

        public Sprite Sprite_Floor;
        public Sprite Sprite_Build;
        public Camera Camera;

        public int2 cell_X;
        public int2 cell_Y;
        
        public bool isStartUp = false;
    }
}