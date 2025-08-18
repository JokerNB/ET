using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MonsterManagerComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MonsterManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterManagerComponent self)
        {
            self.InitMonster();
        }

        public static void InitMonster(this MonsterManagerComponent self)
        {
            for (int i = 0; i < 10; i++)
            {
                self.AddChild<MonsterComponent, int, int, int>(2001, -1, -1);
            }

            for (int i = 0; i < 10; i++)
            {
                self.AddChild<MonsterComponent, int, int, int>(2001, -1, 1);
            }

            for (int i = 0; i < 10; i++)
            {
                self.AddChild<MonsterComponent, int, int, int>(3001, 1, -1);
            }

            for (int i = 0; i < 10; i++)
            {
                self.AddChild<MonsterComponent, int, int, int>(3001, 1, 1);
            }
        }
    }
}