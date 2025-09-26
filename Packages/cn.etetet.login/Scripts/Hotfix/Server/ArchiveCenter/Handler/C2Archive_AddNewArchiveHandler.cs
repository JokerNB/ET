namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_AddNewArchiveHandler : MessageHandler<Scene, C2Archive_AddNewArchiveRequest, C2Archive_AddNewArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, C2Archive_AddNewArchiveRequest request, C2Archive_AddNewArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().AddNewArchive(request.AccountName);
        }
    }
}

