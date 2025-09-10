using System;
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(DBComponent))]
    public static class MongoPatcherHelper
    {
        public static async ETTask CreateDB(Scene root)
        {
            DBManagerComponent dbManagerComponent = root.GetComponent<DBManagerComponent>();
            foreach ((int zone, StartZoneConfig startZoneConfig) in StartZoneConfigCategory.Instance.GetAll())
            {
                await dbManagerComponent.DropDB(zone);
            }

            foreach ((int zone, StartZoneConfig startZoneConfig) in StartZoneConfigCategory.Instance.GetAll())
            {
                if (startZoneConfig.DBName == "")
                {
                    continue;
                }
                
                DBComponent dbComponent = dbManagerComponent.GetZoneDB(zone);

                await dbComponent.CreateCollection<DBVersion>();
                using DBVersion dbVersion = dbComponent.AddComponentWithId<DBVersion>(1);
                // 先保存新的版本号
                dbVersion.Version = DBPatchDispatcher.Instance.MaxVersion();
                await dbComponent.Save(dbVersion);
                
                await DBPatcher_CreateDB.Create(dbComponent, zone);
            }
        }
        
        public static async ETTask UpdateDB(Scene root)
        {
            DBManagerComponent dbManagerComponent = root.GetComponent<DBManagerComponent>();
            foreach ((int zone, StartZoneConfig startZoneConfig) in StartZoneConfigCategory.Instance.GetAll())
            {
                if (startZoneConfig.DBName == "")
                {
                    continue;
                }
                
                DBComponent dbComponent = dbManagerComponent.GetZoneDB(zone);

                DBVersion dbVersion = await dbComponent.Query<DBVersion>(1);
                
                int from = dbVersion.Version;
                
                // 先保存新的版本号
                dbVersion.Version = DBPatchDispatcher.Instance.MaxVersion();
                await dbComponent.Save(dbVersion);
                // 执行补丁
                await DBPatchDispatcher.Instance.Run(dbComponent, zone, from);
            }
        }
    } 
}