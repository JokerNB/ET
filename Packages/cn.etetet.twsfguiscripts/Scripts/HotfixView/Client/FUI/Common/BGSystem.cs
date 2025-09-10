namespace ET.Client
{
    [EntitySystemOf(typeof(BG))]
    [FriendOf(typeof(BG))]
    public static partial class BGSystem
    {
        [EntitySystem]
        private static void Awake(this BG self)
        {
        }

    }
}