namespace ET.Client
{
    [Invoke(TimerInvokeType.CastAutoDisposeTimer)]
    public class CastAutoDisposeTimer_Handler : ATimer<UnitAutoDisposeComponent>
    {
        protected override void Run(UnitAutoDisposeComponent t)
        {
            t?.AutoDispose();
        }
    }
    [EntitySystemOf(typeof(UnitAutoDisposeComponent))]
    public static partial class UnitAutoDisposeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UnitAutoDisposeComponent self, long args2)
        {
            self.durTime = args2;
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.ServerNow() + self.durTime, TimerInvokeType.CastAutoDisposeTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.UnitAutoDisposeComponent self)
        {
            self.durTime = default;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void AutoDispose(this ET.Client.UnitAutoDisposeComponent self)
        {
            self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(self.GetParent<Unit_Client>().Id);
        }
    }
}