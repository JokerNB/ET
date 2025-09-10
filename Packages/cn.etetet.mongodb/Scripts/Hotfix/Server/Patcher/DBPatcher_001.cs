namespace ET.Server
{
    [DBPatcher(1)]
    public class DBPatcher_001: IDBPatcher
    {
        public async ETTask Run(DBComponent dbComponent, int zone)
        {
            await ETTask.CompletedTask;
        }
    }
}