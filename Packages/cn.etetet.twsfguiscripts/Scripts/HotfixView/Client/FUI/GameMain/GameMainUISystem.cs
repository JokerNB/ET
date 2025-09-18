namespace ET.Client
{
    [EntitySystemOf(typeof(GameMainUI))]
    [FriendOf(typeof(GameMainUI))]
    public static partial class GameMainUISystem
    {
        [EntitySystem]
        private static void Awake(this GameMainUI self)
        {
        }

    }
}