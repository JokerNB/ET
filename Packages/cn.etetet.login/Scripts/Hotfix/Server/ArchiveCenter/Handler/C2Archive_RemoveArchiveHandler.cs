namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_RemoveArchiveHandler : MessageHandler<Scene, C2Archive_RemoveArchiveRequest, C2Archive_RemoveArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, C2Archive_RemoveArchiveRequest request, C2Archive_RemoveArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().RemoveArchive(request.AccountName, request.ArchiveNum);
        }
    }
}