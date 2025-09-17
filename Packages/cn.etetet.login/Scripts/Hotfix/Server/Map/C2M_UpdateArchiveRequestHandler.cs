namespace ET.Server
{
    [MessageSessionHandler(SceneType.Map)]
    public class C2M_UpdateArchiveRequestHandler : MessageSessionHandler<C2M_UpdateArchiveRequest, C2M_UpdateArchiveResponse>
    {
        protected override async ETTask Run(Session session, C2M_UpdateArchiveRequest request, C2M_UpdateArchiveResponse response)
        {
            Other2Archive_UpdateArchiveRequest msg = Other2Archive_UpdateArchiveRequest.Create();
            msg.ArchiveInfo = request.ArchiveInfoProto;
            
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            var response1 =
                    await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, msg) as Other2Archive_UpdateArchiveResponse;
            response.Error = response1.Error;
        }
    }
}