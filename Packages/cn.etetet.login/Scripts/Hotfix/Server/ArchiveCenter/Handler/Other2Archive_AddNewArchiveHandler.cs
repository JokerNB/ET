namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_AddNewArchiveHandler : MessageHandler<Scene, Other2Archive_AddNewArchiveRequest, Other2Archive_AddNewArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_AddNewArchiveRequest request, Other2Archive_AddNewArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().AddNewArchive(request.AccountName);
        }
    }
}

