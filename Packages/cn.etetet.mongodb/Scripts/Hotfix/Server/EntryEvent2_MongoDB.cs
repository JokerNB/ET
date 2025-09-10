namespace ET.Server
{
    [Event(SceneType.MongoDB)]
    public class EntryEvent2_MongoDB: AEvent<Scene, EntryEvent2>
    {
        protected override async ETTask Run(Scene root, EntryEvent2 args)
        {
            World.Instance.AddSingleton<DBPatchDispatcher>();
            if (Options.Instance.Console == 1)
            {
                root.AddComponent<ConsoleComponent>();    
            }
            root.AddComponent<DBManagerComponent>();
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            
            await ETTask.CompletedTask;
        }
    }
}