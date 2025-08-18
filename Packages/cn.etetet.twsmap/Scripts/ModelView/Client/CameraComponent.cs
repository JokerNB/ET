using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraComponent : Entity, IAwake
    {
        public Camera MainCamera;
        public Transform MainCameraTr => this.MainCamera.transform;
        /// <summary>
        /// 左 下 右 上
        /// </summary>
        public Vector4 cameraBoundPos = Vector4.zero;

        /// <summary>
        /// 宽高
        /// </summary>
        public Vector2 cameraBoundSize = Vector2.zero;

    }
}

