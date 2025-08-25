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
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            if (self.GetParent<Unit_Client>().UnitType == UnitType.Monster)
                YIUIGameObjectPool.Inst.Put(self.GameObject);
            else
                UnityEngine.Object.Destroy(self.GameObject);
        }

        public static void SetPosition(this GameObjectComponent self, Vector3 position)
        {
            if (self.GetParent<Unit_Client>().UnitType == UnitType.Monster)
                self.GetParent<Unit_Client>().GetComponent<MonsterMoveComponent>().SetPos(position);
            else
                self.Transform.position = position;
        }
    }
}