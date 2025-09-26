namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_GetArchiveListHandler : MessageHandler<Scene, C2Archive_GetArchiveListRequest, C2Archive_GetArchiveListResponse>
    {
        protected override async ETTask Run(Scene scene, C2Archive_GetArchiveListRequest request, C2Archive_GetArchiveListResponse response)
        {
            var list = await scene.GetComponent<ArchiveInfoManagerComponent>().GetArchiveInfoList(request.AccountName);
            response.ArchiveInfos = list;
        }
    }
}

