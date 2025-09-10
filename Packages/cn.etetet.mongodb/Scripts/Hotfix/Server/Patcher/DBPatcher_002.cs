namespace ET.Server
{
    [DBPatcher(2)]
    public class DBPatcher_002: IDBPatcher
    {
        public async ETTask Run(DBComponent dbComponent, int zone)
        {
            await ETTask.CompletedTask;
        }
    }
}