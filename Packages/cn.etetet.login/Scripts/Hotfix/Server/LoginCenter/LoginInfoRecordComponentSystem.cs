namespace ET.Server
{
    [EntitySystemOf(typeof(LoginInfoRecordComponent))]
    public static partial class LoginInfoRecordComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.LoginInfoRecordComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Server.LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDic.Clear();
        }

        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
            {
                self.AccountLoginInfoDic[key] = value;
                return;
            }
            self.AccountLoginInfoDic.Add(key, value);
        }
        
        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            if (!self.AccountLoginInfoDic.TryGetValue(key,out int value))
            {
                return -1;
            }

            return value;
        }
        
        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
            {
                self.AccountLoginInfoDic.Remove(key);
            }
        }
        
        public static bool IsExist(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDic.ContainsKey(key);
        }
    }
}
