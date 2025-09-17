namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string address, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();

            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();

            NetClient2Main_Login netClient2MainLogin = await clientSenderComponent.LoginAsync(address, account, password);

            if (netClient2MainLogin.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"response Error : {netClient2MainLogin.Error}");
                return;
            }

            Log.Debug("请求登录成功");
            string token = netClient2MainLogin.Token;

            //获取服务器列表
            C2R_GetServerInfos c2RGetServerInfos = C2R_GetServerInfos.Create();
            c2RGetServerInfos.Account = account;
            c2RGetServerInfos.Token = token;
            R2C_GetServerInfos r2CGetServerInfos = await clientSenderComponent.Call(c2RGetServerInfos) as R2C_GetServerInfos;
            if (r2CGetServerInfos.Error != ErrorCode.ERR_Success)
            {
                Log.Error("请求服务器列表失败");
                return;
            }

            ServerInfoProto serverInfoProto = r2CGetServerInfos.ServerInfoList[0];
            
            //TODO:请求存档信息

            //请求获取RealmKey
            C2R_GetRealmKey c2RGetRealmKey = C2R_GetRealmKey.Create();
            c2RGetRealmKey.Token = token;
            c2RGetRealmKey.AccountName = account;
            c2RGetRealmKey.ServerId = serverInfoProto.Id;
            R2C_GetRealmKey r2CGetRealmKey = await clientSenderComponent.Call(c2RGetRealmKey) as R2C_GetRealmKey;
            if (r2CGetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error("获取RealmKey失败");
                return;
            }
            
            C2R_GetArchiveListRequest c2RGetArchiveListRequest = C2R_GetArchiveListRequest.Create();
            c2RGetArchiveListRequest.AccountName = account;
            c2RGetArchiveListRequest.Token = token;
            C2R_GetArchiveListResponse c2RGetArchiveListResponse = await clientSenderComponent.Call(c2RGetArchiveListRequest) as C2R_GetArchiveListResponse;
            if (c2RGetArchiveListResponse.Error != ErrorCode.ERR_Success)
            {
                Log.Error("获取存档列表失败");
                return;
            }
            root.GetComponent<ArchiveInfoManagerComponent_Client>().InitArchiveList(c2RGetArchiveListResponse.ArchiveInfoList);

            var playerComponent = root.GetComponent<PlayerComponent>();
            playerComponent.Token = token;
            playerComponent.Key = r2CGetRealmKey.Key;
            playerComponent.Address = r2CGetRealmKey.Address;
            playerComponent.PlayerId = netClient2MainLogin.PlayerId;
            
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}