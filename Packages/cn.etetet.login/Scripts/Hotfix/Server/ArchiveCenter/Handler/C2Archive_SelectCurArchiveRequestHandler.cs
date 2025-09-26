namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_SelectCurArchiveRequestHandler : MessageHandler<Scene, C2Archive_SelectCurArchiveRequest, C2Archive_SelectCurArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, C2Archive_SelectCurArchiveRequest request, C2Archive_SelectCurArchiveResponse response)
        {
            int err = scene.GetComponent<ArchiveInfoManagerComponent>().SelectCurArchive(request.AccountName, request.ArchiveNum);
            response.Error = err;
            await ETTask.CompletedTask;
        }
    }
}

