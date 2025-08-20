using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapManagerComponent : Entity, IAwake
    {
        public Sprite spriteAsset;
        public GameObject goAsset;
        public string ResPath => "Packages/cn.etetet.twsmap/Assets/GameRes/MapAtlas/";
        public string BundlesPath => "Packages/cn.etetet.twsmap/Assets/GameRes/Bundles/";

        public List<EntityRef<MapComponent>> MapChildren = new List<EntityRef<MapComponent>>();
        
        /// <summary>
        /// 资源宽高
        /// </summary>
        public Vector2 spriteSize = Vector2.zero;
        
        public Transform MapRootTr;
    }
}