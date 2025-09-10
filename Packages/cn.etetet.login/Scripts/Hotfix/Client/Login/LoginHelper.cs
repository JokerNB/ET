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

            //获取区服角色列表
            C2R_GetRoles c2RGetRoles = C2R_GetRoles.Create();
            c2RGetRoles.Account = account;
            c2RGetRoles.Token = token;
            c2RGetRoles.ServerId = serverInfoProto.Id;
            R2C_GetRoles r2CGetRoles = await clientSenderComponent.Call(c2RGetRoles) as R2C_GetRoles;
            if (r2CGetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error("请求角色列表失败");
                return;
            }

            RoleInfoProto roleInfoProto = default;
            if (r2CGetRoles.RoleInfos.Count <= 0)
            {
                //无角色信息，创建角色
                C2R_CreateRole c2R_CreateRole = C2R_CreateRole.Create();
                c2R_CreateRole.Account = account;
                c2R_CreateRole.ServerId = serverInfoProto.Id;
                c2R_CreateRole.Token = token;
                c2R_CreateRole.RoleName = "Admin";
                R2C_CreateRole r2CCreateRole = await clientSenderComponent.Call(c2R_CreateRole) as R2C_CreateRole;
                if (r2CCreateRole.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("创建角色失败");
                    return;
                }

                roleInfoProto = r2CCreateRole.RoleInfo;
            }
            else
            {
                roleInfoProto = r2CGetRoles.RoleInfos[0];
            }
            
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
            
            //请求角色进入Map地图
            NetClient2Main_LoginGame netClient2MainLoginGame =
                    await clientSenderComponent.LoginGameAsync(account, r2CGetRealmKey.Key, roleInfoProto.Id, r2CGetRealmKey.Address);
            if (netClient2MainLoginGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"进入游戏失败: {netClient2MainLoginGame.Error}");
                return;
            }
            
            Log.Debug("进入游戏成功！");
            root.GetComponent<PlayerComponent>().MyId = netClient2MainLoginGame.PlayerId;
            root.GetComponent<PlayerComponent>().Token = token;
            
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}