using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self, GameObject go, bool isInGamePool)
        {
            self.GameObject = go;
            self.isInGamePool = isInGamePool;
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            if (self.isInGamePool)
                YIUIGameObjectPool.Inst.Put(self.GameObject);
            else
                UnityEngine.Object.Destroy(self.GameObject);
        }

        public static void SetPosition(this GameObjectComponent self, Vector2 position)
        {
            self.Transform.position = position;
        }

        public static void SetRotation(this GameObjectComponent self, Quaternion rotation)
        {
            self.Transform.rotation = rotation;
        }
    }
}