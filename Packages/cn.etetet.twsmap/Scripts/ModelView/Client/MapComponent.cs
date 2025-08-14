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
        public string ResPath { get; } = "Packages/cn.etetet.twsmap/Assets/GameRes/MapAtlas/";

        /// <summary>
        /// 左 下 右 上
        /// </summary>
        public Vector4 cameraBoundPos = Vector4.zero;

        /// <summary>
        /// 宽高
        /// </summary>
        public Vector2 cameraBoundSize = Vector2.zero;

        public List<EntityRef<MapChildComponent>> MapChildren = new List<EntityRef<MapChildComponent>>();
        
        /// <summary>
        /// 资源宽高
        /// </summary>
        public Vector2 spriteSize = Vector2.zero;
    }
}