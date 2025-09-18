namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_SelectCurArchiveRequestHandler : MessageHandler<Scene, Other2Archive_SelectCurArchiveRequest, Other2Archive_SelectCurArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_SelectCurArchiveRequest request, Other2Archive_SelectCurArchiveResponse response)
        {
            int err = scene.GetComponent<ArchiveInfoManagerComponent>().SelectCurArchive(request.AccountName, request.ArchiveNum);
            response.Error = err;
            await ETTask.CompletedTask;
        }
    }
}

