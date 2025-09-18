namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_GetArchiveListRequestHandler : MessageSessionHandler<C2R_GetArchiveListRequest, C2R_GetArchiveListResponse>
    {
        protected override async ETTask Run(Session session, C2R_GetArchiveListRequest request, C2R_GetArchiveListResponse response)
        {
            string token = session.Root().GetComponent<TokenComponent>().Get(request.AccountName);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                session?.Disconnect().NoContext();
                return;
            }
            
            Other2Archive_GetArchiveListRequest msg = Other2Archive_GetArchiveListRequest.Create();
            msg.AccountName = request.AccountName;
            
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            var response1 =
                    await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, msg) as Other2Archive_GetArchiveListResponse;
            response.Error = response1.Error;
            response.ArchiveInfoList = response1.ArchiveInfos;
        }
    }
}