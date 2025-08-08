namespace ET.Server
{
    [Invoke(TimerInvokeType.PlaterOfflineOutTime)]
    public class PlayerOfflineOutTime : ATimer<PlayerOfflineOutTimeComponent>
    {
        protected override void Run(PlayerOfflineOutTimeComponent t)
        {
            t?.KickPlayer();
        }
    }

    [EntitySystemOf(typeof(PlayerOfflineOutTimeComponent))]
    public static partial class PlayerOfflineOutTimeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.PlayerOfflineOutTimeComponent self)
        {
            self.timer = self.Root().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.ServerNow() + 10000, TimerInvokeType.PlaterOfflineOutTime, self);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.PlayerOfflineOutTimeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.timer);
        }

        public static void KickPlayer(this ET.Server.PlayerOfflineOutTimeComponent self)
        {
            DisconnectHelper.KickPlayer(self.GetParent<Player>()).NoContext();
        }
    }
}