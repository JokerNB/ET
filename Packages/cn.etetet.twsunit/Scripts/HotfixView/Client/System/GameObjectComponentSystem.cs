using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self, GameObject go)
        {
            self.GameObject = go;
            self.sprite = self.GameObject.Get<SpriteRenderer>("sprite");
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }

        public static void SetPosition(this GameObjectComponent self, Vector2 position)
        {
            self.Transform.position = position;
        }

        public static void SetLocalPosition(this GameObjectComponent self, Vector2 position)
        {
            self.Transform.localPosition = position;
        }

        public static void SetScale(this GameObjectComponent self, Vector2 scale)
        {
            self.Transform.localScale = scale;
        }

        public static void SetRotation(this GameObjectComponent self, Quaternion rotation)
        {
            self.Transform.rotation = rotation;
        }

        public static void SetLocalRotation(this GameObjectComponent self, Quaternion rotation)
        {
            self.Transform.localRotation = rotation;
        }

        public static void SetGameObjectParent(this GameObjectComponent self, Transform parent)
        {
            self.Transform.SetParent(parent);
        }
    }
}