using System.Collections.Generic;
using ET;
using UnityEngine;

namespace FairyGUI
{
    /// <summary>
    /// 
    /// </summary>
    public class HitTestContext
    {
        //set before hit test
        [StaticField]
        public static Vector3 screenPoint;
        [StaticField]
        public static Vector3 worldPoint;
        [StaticField]
        public static Vector3 direction;
        [StaticField]
        public static bool forTouch;
        [StaticField]
        public static Camera camera;

        [StaticField]
        public static int layerMask = -1;
        [StaticField]
        public static float maxDistance = Mathf.Infinity;

        [StaticField]
        public static Camera cachedMainCamera;

        [StaticField]
        static Dictionary<Camera, RaycastHit?> raycastHits = new Dictionary<Camera, RaycastHit?>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="hit"></param>
        /// <returns></returns>
        public static bool GetRaycastHitFromCache(Camera camera, out RaycastHit hit)
        {
            RaycastHit? hitRef;
            if (!raycastHits.TryGetValue(camera, out hitRef))
            {
                Ray ray = camera.ScreenPointToRay(screenPoint);
                if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
                {
                    raycastHits[camera] = hit;
                    return true;
                }
                else
                {
                    raycastHits[camera] = null;
                    return false;
                }
            }
            else if (hitRef == null)
            {
                hit = new RaycastHit();
                return false;
            }
            else
            {
                hit = (RaycastHit)hitRef;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="hit"></param>
        public static void CacheRaycastHit(Camera camera, ref RaycastHit hit)
        {
            raycastHits[camera] = hit;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearRaycastHitCache()
        {
            raycastHits.Clear();
        }

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnLoad()
        {
            cachedMainCamera = null;
            raycastHits.Clear();
        }
#endif
    }

}
