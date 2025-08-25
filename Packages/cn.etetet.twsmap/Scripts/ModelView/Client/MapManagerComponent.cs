using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapManagerComponent : Entity, IAwake
    {
        public Sprite spriteAsset { get; set; }
        public GameObject goAsset { get; set; }
        public string ResPath => "Packages/cn.etetet.twsmap/Assets/GameRes/MapAtlas/";
        public string BundlesPath => "Packages/cn.etetet.twsmap/Assets/GameRes/Bundles/";

        public List<EntityRef<MapComponent>> MapChildren = new List<EntityRef<MapComponent>>();

        /// <summary>
        /// 资源宽高
        /// </summary>
        public Vector2 spriteSize { get; set; } = Vector2.zero;

        public Transform MapRootTr { get; set; }
        public Transform ParticleRootTr { get; set; }
        public PolyNav2D MapNav;
        public PolygonCollider2D MapNavCollider;
    }
}