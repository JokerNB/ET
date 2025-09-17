using System.Text.RegularExpressions;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(ET.Server.Account))]
    public class C2R_LoginAccountHandler : MessageSessionHandler<C2R_LoginAccount, R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session.Disconnect().NoContext();
                return;
            }

            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNull;
                session.Disconnect().NoContext();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                session.Disconnect().NoContext();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                session.Disconnect().NoContext();
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount, request.AccountName.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
                    var accountInfoList = await dbComponent.Query<Account>(d => d.AccountName.Equals(request.AccountName));
                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            session.Disconnect().NoContext();
                            account?.Dispose();
                            return;
                        }

                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            session.Disconnect().NoContext();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName;
                        account.Password = request.Password;
                        account.AccountType = (int)AccountType.General;
                        account.CreateTime = TimeInfo.Instance.ServerNow();
                        account.PlayerId = IdGenerater.Instance.GenerateId();
                        await dbComponent.Save(account);
                    }
                    
                    R2L_LoginAccountRequest r2LLoginAccountRequest = R2L_LoginAccountRequest.Create();
                    r2LLoginAccountRequest.AccountName = request.AccountName;

                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.loginCenterConfig;
                    var l2rLoginAccountResponse =
                            await session.Fiber().Root.GetComponent<MessageSender>()
                                            .Call(loginCenterConfig.ActorId, r2LLoginAccountRequest) as
                                    L2R_LoginAccountResponse;
                    if (l2rLoginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2rLoginAccountResponse.Error;
                        session?.Disconnect().NoContext();
                        account?.Dispose();
                        return;
                    }

                    Session otherSession = session.Root().GetComponent<AccountSessionComponent>().Get(request.AccountName);

                    otherSession?.Send(A2C_Disconnect.Create());
                    otherSession?.Disconnect().NoContext();

                    session.Root().GetComponent<AccountSessionComponent>().Add(request.AccountName, session);
                    session.AddComponent<AccountCheckOutTimeComponent, string>(request.AccountName);
                    
                    string Token = TimeInfo.Instance.ServerNow().ToString() + RandomGenerator.RandomNumber(int.MinValue,int.MaxValue).ToString();
                    session.Root().GetComponent<TokenComponent>().Remove(request.AccountName);
                    session.Root().GetComponent<TokenComponent>().Add(request.AccountName, Token);
                    
                    response.Token = Token;
                    response.PlayerId = account.PlayerId;
                    account?.Dispose();
                }
            }
        }
    }
}