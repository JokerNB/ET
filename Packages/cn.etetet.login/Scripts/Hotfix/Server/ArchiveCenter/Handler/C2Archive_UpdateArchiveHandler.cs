namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_UpdateArchiveHandler : MessageHandler<Scene, C2Archive_UpdateArchiveRequest, C2Archive_UpdateArchiveResponse>
    {
        protected override async ETTask Run(Scene scene, C2Archive_UpdateArchiveRequest request, C2Archive_UpdateArchiveResponse response)
        {
            await scene.GetComponent<ArchiveInfoManagerComponent>().UpdateArchiveList(request.ArchiveInfo);
        }
    }
}