namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class C2M_UpdateArchiveRequestHandler : MessageLocationHandler<Unit ,C2M_UpdateArchiveRequest, C2M_UpdateArchiveResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_UpdateArchiveRequest request, C2M_UpdateArchiveResponse response)
        {
            Other2Archive_UpdateArchiveRequest msg = Other2Archive_UpdateArchiveRequest.Create();
            msg.ArchiveInfo = request.ArchiveInfoProto;
            
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            var response1 =
                    await unit.Root().GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, msg) as Other2Archive_UpdateArchiveResponse;
            response.Error = response1.Error;
        }
    }
}