using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(MapComponent))]
    public class MapChildComponent : Entity, IAwake<Vector2, Sprite, Transform, GameObject, Vector2>
    {
        public Vector2 centerPos;
        public Sprite sprite;
        public GameObject go;
        public Vector2 remaind = Vector2.zero;
        public SpriteRenderer spriteRenderer;
        public Vector2 spriteSize;

        public bool isXLeftInside = false;
        public bool isXRightInside = false;
        public bool isYTopInside = false;
        public bool isYBottomInside = false;
    }
}