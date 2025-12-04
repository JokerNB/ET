namespace ET.Client
{
    [EntitySystemOf(typeof(UILoading))]
    [FriendOf(typeof(UILoading))]
    public static partial class UILoadingSystem
    {
        [EntitySystem]
        private static void Awake(this UILoading self)
        {
        }

    }
}