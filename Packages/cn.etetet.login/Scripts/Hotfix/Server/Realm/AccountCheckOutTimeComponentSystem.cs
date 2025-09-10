namespace ET.Server
{
    [Invoke(TimerInvokeType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimeComponent>
    {
        protected override void Run(AccountCheckOutTimeComponent t)
        {
            t?.DeleteSession();
        }
    }
    [EntitySystemOf(typeof(AccountCheckOutTimeComponent))]
    public static partial class AccountCheckOutTimeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.AccountCheckOutTimeComponent self, string args2)
        {
            self.AccountName = args2;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.ServerNow() + 600000, TimerInvokeType.AccountSessionCheckOutTime, self);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.AccountCheckOutTimeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void DeleteSession(this ET.Server.AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            Session originSession = session.Root().GetComponent<AccountSessionComponent>().Get(self.AccountName);
            if (originSession != null && session.InstanceId == originSession.InstanceId)
            {
                session.Root().GetComponent<AccountSessionComponent>().Remove(self.AccountName);
            }
            
            A2C_Disconnect a2CDisconnect = A2C_Disconnect.Create();
            a2CDisconnect.Error = 1;
            session?.Send(a2CDisconnect);
            session?.Disconnect().NoContext();
        }
    }
}