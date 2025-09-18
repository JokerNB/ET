namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class Other2Archive_RemoveCurArchiveHandler : MessageHandler<Scene, Other2Archive_RemoveCurArchive>
    {
        protected override async ETTask Run(Scene scene, Other2Archive_RemoveCurArchive message)
        {
            scene.GetComponent<ArchiveInfoManagerComponent>().RemoveCurArchive(message.AccountName);
            await ETTask.CompletedTask;
        }
    }
}

