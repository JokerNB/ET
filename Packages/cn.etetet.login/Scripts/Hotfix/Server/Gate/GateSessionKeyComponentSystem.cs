namespace ET.Server
{
    [FriendOf(typeof(GateSessionKeyComponent))]
    public static partial class GateSessionKeyComponentSystem
    {
        public static void Add(this GateSessionKeyComponent self, long key, string account)
        {
            self.sessionKey.Add(key, account);
            self.sessionKey_AccountName.Add(account, key);
            self.TimeoutRemoveKey(key).NoContext();
        }

        public static string Get(this GateSessionKeyComponent self, long key)
        {
            string account = null;
            self.sessionKey.TryGetValue(key, out account);
            return account;
        }

        public static long Get(this GateSessionKeyComponent self, string accountName)
        {
            self.sessionKey_AccountName.TryGetValue(accountName, out long key);
            return key;
        }

        public static void Remove(this GateSessionKeyComponent self, long key)
        {
            string AccountName = self.Get(key);
            if (!string.IsNullOrEmpty(AccountName))
            {
                self.sessionKey_AccountName.Remove(AccountName);
            }
            self.sessionKey.Remove(key);
        }

        public static void Remove(this GateSessionKeyComponent self, string AccountName)
        {
            long key = self.Get(AccountName);
            self.sessionKey.Remove(key);
            self.sessionKey_AccountName.Remove(AccountName);
        }

        private static async ETTask TimeoutRemoveKey(this GateSessionKeyComponent self, long key)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(20000);
            self.Remove(key);
        }
    }
}