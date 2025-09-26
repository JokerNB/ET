namespace ET.Server
{
    [MessageHandler(SceneType.ArchiveCenter)]
    public class C2Archive_RemoveCurArchiveHandler : MessageHandler<Scene, C2Archive_RemoveCurArchive>
    {
        protected override async ETTask Run(Scene scene, C2Archive_RemoveCurArchive message)
        {
            scene.GetComponent<ArchiveInfoManagerComponent>().RemoveCurArchive(message.AccountName);
            await ETTask.CompletedTask;
        }
    }
}

