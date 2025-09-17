namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_RemoveArchiveHandler : MessageHandler<Scene, Other2Archive_RemoveArchiveRequest, Other2Archive_RemoveArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_RemoveArchiveRequest request, Other2Archive_RemoveArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().RemoveArchive(request.AccountName, request.ArchiveNum);
        }
    }
}