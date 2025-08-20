using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(MapManagerComponent))]
    public class MapComponent : Entity, IAwake<Vector2>
    {
        public Vector2 centerPos;
        public GameObject go;
        public Transform goTr;
        public Vector2 remaind = Vector2.zero;
        public Vector2 spriteSize;

        public bool isXLeftInside = false;
        public bool isXRightInside = false;
        public bool isYTopInside = false;
        public bool isYBottomInside = false;
    }
}