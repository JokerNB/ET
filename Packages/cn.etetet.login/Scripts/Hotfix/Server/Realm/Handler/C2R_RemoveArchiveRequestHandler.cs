namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_RemoveArchiveRequestHandler : MessageSessionHandler<C2R_RemoveArchiveRequest, C2R_RemoveArchiveResponse>
    {
        protected override async ETTask Run(Session session, C2R_RemoveArchiveRequest request, C2R_RemoveArchiveResponse response)
        {
            Other2Archive_RemoveArchiveRequest msg = Other2Archive_RemoveArchiveRequest.Create();
            msg.AccountName = request.AccountName;
            msg.ArchiveNum = request.ArchiveNum;
            
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            var response1 =
                    await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, msg) as Other2Archive_RemoveArchiveResponse;
            response.Error = response1.Error;
        }
    }
}