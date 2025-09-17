namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_UpdateArchiveHandler : MessageHandler<Scene, Other2Archive_UpdateArchiveRequest, Other2Archive_UpdateArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_UpdateArchiveRequest request, Other2Archive_UpdateArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().UpdateArchiveList(request.ArchiveInfo);
        }
    }
}