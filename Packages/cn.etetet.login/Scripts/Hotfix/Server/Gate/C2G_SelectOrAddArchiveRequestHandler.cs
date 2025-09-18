namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_SelectOrAddArchiveRequestHandler : MessageSessionHandler<C2G_SelectOrAddArchiveRequest, C2G_SelectOrAddArchiveResponse>
    {
        protected override async ETTask Run(Session session, C2G_SelectOrAddArchiveRequest request, C2G_SelectOrAddArchiveResponse response)
        {
            StartSceneConfig archiveCenterConfig = StartSceneConfigCategory.Instance.archiveCenterConfig;
            if (request.ArchiveNum == -1)
            {
                Other2Archive_AddNewArchiveRequest req = Other2Archive_AddNewArchiveRequest.Create();
                req.AccountName = request.AccountName;
                var res =
                        await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, req) as
                                Other2Archive_AddNewArchiveResponse;
                if (res.Error != ErrorCode.ERR_Success)
                {
                    response.Error = res.Error;
                    return;
                }

                response.ArchiveNum = res.ArchiveNum;
            }
            else
            {
                Other2Archive_SelectCurArchiveRequest req = Other2Archive_SelectCurArchiveRequest.Create();
                req.AccountName = request.AccountName;
                req.ArchiveNum = request.ArchiveNum;
                var res =
                        await session.Fiber().Root.GetComponent<MessageSender>().Call(archiveCenterConfig.ActorId, req) as
                                Other2Archive_SelectCurArchiveResponse;
                if (res.Error != ErrorCode.ERR_Success)
                {
                    response.Error = res.Error;
                    return;
                }

                response.ArchiveNum = request.ArchiveNum;
            }

            await ETTask.CompletedTask;
        }
    }
}