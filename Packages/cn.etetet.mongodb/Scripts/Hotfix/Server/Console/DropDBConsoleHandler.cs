namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.DropDB)]
    public class DropDBConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            foreach (var kv in StartZoneConfigCategory.Instance.GetAll())
            {
                await fiber.Root.GetComponent<DBManagerComponent>().DropDB(kv.Key);
            }
            contex.Parent.RemoveComponent<ModeContex>();
        }
    }
}