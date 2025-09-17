namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_AddNewArchiveRequestHandler : MessageSessionHandler<C2R_AddNewArchiveRequest, C2R_AddNewArchiveResponse>
    {
        protected override async ETTask Run(Session session, C2R_AddNewArchiveRequest request, C2R_AddNewArchiveResponse response)
        {
            Other2Archive_AddNewArchiveRequest msg = Other2Archive_AddNewArchiveRequest.Create();
            msg.AccountName = request.AccountName;
            
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            var response1 =
                    await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, msg) as Other2Archive_AddNewArchiveResponse;
            response.Error = response1.Error;
        }
    }
}