using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(PlayerMoveComponent))]
    public static partial class PlayerMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.PlayerMoveComponent self)
        {
            self.GameObject = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().GameObject;
            self.SpriteRenderer = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().sprite;

        }

        [EntitySystem]
        private static void Update(this ET.Client.PlayerMoveComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.PlayerMoveComponent self)
        {
        }
    }
}