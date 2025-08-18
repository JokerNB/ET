using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapComponent : Entity, IAwake
    {
        public Camera MainCamera;
        public Transform MainCameraTr;
        public SpriteRenderer MapSpriteRenderer;
        public Sprite Sprite;
        public Transform MapRoot;
        public string ResPath => "Packages/cn.etetet.twsmap/Assets/GameRes/MapAtlas/";

        public List<EntityRef<MapChildComponent>> MapChildren = new List<EntityRef<MapChildComponent>>();
        
        /// <summary>
        /// 资源宽高
        /// </summary>
        public Vector2 spriteSize = Vector2.zero;
    }
}