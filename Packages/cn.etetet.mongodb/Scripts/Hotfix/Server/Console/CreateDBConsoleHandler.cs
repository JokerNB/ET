namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.CreateDB)]
    public class CreateDBConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            await MongoPatcherHelper.CreateDB(fiber.Root);
            
            contex.Parent.RemoveComponent<ModeContex>();
        }
    }
}