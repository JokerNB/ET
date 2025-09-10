namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.UpdateDB)]
    public class UpdateDBConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            await MongoPatcherHelper.UpdateDB(fiber.Root);
            contex.Parent.RemoveComponent<ModeContex>();
        }
    }
}