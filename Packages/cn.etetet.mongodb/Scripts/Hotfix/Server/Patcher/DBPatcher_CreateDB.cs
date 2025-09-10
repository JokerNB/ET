using MongoDB.Driver;

namespace ET.Server
{
    public static class DBPatcher_CreateDB
    {
        public static async ETTask Create(DBComponent dbComponent, int zone)
        {
            Log.Info("Create DB");
            switch (zone)
            {
                case 1: // 游戏区
                    // 创建一个Unit表
                    await dbComponent.CreateCollection<Unit>();
                    // 给Unit.ConfigId创建一个索引
                    await dbComponent.CreateIndex(Builders<Unit>.IndexKeys.Ascending(v => v.ConfigId));
                    // 插入初始数据……
                    break;
                case 2: // 机器人区
                    break;
                case 3: // 路由区
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}