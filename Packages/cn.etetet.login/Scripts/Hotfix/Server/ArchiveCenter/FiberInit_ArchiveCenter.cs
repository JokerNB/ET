namespace ET.Server
{
    [Invoke(SceneType.ArchiveCenter)]
    public class FiberInit_ArchiveCenter : AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, int>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();

            root.AddComponent<ArchiveInfoManagerComponent>();
            root.AddComponent<DBManagerComponent>();
            await ETTask.CompletedTask;
        }
    }
}