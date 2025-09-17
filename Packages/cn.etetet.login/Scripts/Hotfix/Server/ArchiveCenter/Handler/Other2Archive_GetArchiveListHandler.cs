namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_GetArchiveListHandler : MessageHandler<Scene, Other2Archive_GetArchiveListRequest, Other2Archive_GetArchiveListResponse>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_GetArchiveListRequest request, Other2Archive_GetArchiveListResponse response)
        {
            var list = await scene.GetComponent<ArchiveInfoManagerComponent>().GetArchiveInfoList(request.AccountName);
            response.ArchiveInfos = list;
        }
    }
}

