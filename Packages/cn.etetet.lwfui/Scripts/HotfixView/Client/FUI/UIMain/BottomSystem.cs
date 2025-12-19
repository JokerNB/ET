namespace ET.Client
{
    [EntitySystemOf(typeof(Bottom))]
    [FriendOf(typeof(Bottom))]
    public static partial class BottomSystem
    {
        [EntitySystem]
        private static void Awake(this Bottom self, ET.Client.UIMain.FUI_Bottom fuiBottom)
        {
            self.FUIBottom = fuiBottom;
        }

    }
}