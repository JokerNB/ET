namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(ET.Server.RoleInfo))]
    public class C2R_CreateRoleHandler : MessageSessionHandler<C2R_CreateRole, R2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2R_CreateRole request, R2C_CreateRole response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session?.Disconnect().NoContext();
                return;
            }

            string token = session.Root().GetComponent<TokenComponent>().Get(request.Account);
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                session?.Disconnect().NoContext();
                return;
            }

            if (string.IsNullOrEmpty(request.RoleName))
            {
                response.Error = ErrorCode.ERR_RoleNameIsNull;
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.CreateRole, request.Account.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());

                    var roleInfos = await dbComponent.Query<RoleInfo>(d => d.RoleName == request.RoleName && d.ServerId == request.ServerId);
                    if (roleInfos != null && roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameSame;
                        return;
                    }

                    RoleInfo roleInfo = session.AddChild<RoleInfo>();
                    roleInfo.RoleName = request.RoleName;
                    roleInfo.State = (int)RoleInfoState.Normal;
                    roleInfo.ServerId = request.ServerId;
                    roleInfo.AccountName = request.Account;
                    roleInfo.CreateTime = TimeInfo.Instance.ServerNow();
                    roleInfo.LastLoginTime = 0;

                    await dbComponent.Save<RoleInfo>(roleInfo);
                    response.RoleInfo = roleInfo.ToMessage();
                    roleInfo?.Dispose();
                }
            }
        }
    }
}