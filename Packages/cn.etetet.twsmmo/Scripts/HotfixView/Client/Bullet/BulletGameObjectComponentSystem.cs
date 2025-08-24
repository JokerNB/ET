using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(BulletGameObjectComponent))]
    public static partial class BulletGameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.BulletGameObjectComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Client.BulletGameObjectComponent self)
        {

        }

        public static void SetPosition(this ET.Client.BulletGameObjectComponent self, float3 position)
        {
            
        }
    }
}

