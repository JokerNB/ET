using System;

namespace ET.Server
{
    public static class UnitCacheHelper
    {
        public static async ETTask<Unit> GetUnitCache(Scene gateScene, Scene mapScene, long unitId)
        {
            ActorId instanceId = StartSceneConfigCategory.Instance.GetOneBySceneType(gateScene.Zone(), SceneType.UnitCache).ActorId;
            Other2UnitCache_GetUnit message = Other2UnitCache_GetUnit.Create();
            message.UnitId = unitId;

            UnitCache2Other_GetUnit queryUnit =
                    await gateScene.Root().GetComponent<MessageSender>().Call(instanceId, message) as UnitCache2Other_GetUnit;
            if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
                return null;

            Unit unit = null;
            int indexOf = queryUnit.ComponentNameLIst.IndexOf(typeof(Unit).FullName);
            if (indexOf >= 0)
            {
                if (queryUnit.EntityList[indexOf] != null)
                    unit = MongoHelper.Deserialize<Entity>(queryUnit.EntityList[indexOf]) as Unit;
            }

            if (unit == null)
                return null;

            mapScene.GetComponent<UnitComponent>().AddChild(unit);
            if (unit.GetComponent<UnitDBSaveComponent>() == null)
                unit.AddComponent<UnitDBSaveComponent>();

            for (int i = 0; i < queryUnit.EntityList.Count; i++)
            {
                if (i == indexOf) //跳过Unit
                    continue;

                byte[] entityByte = queryUnit.EntityList[i];
                Type type = CodeTypes.Instance.GetType(queryUnit.ComponentNameLIst[i]);
                EventSystem.Instance.Invoke(SceneType.UnitCache, new AddToBytes()
                {
                    unit = unit,
                    type = type,
                    bytes = entityByte
                });
            }

            return unit;
        }

        public static void AddOrUpdateUnitAllCache(Unit unit)
        {
            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = unit.Id;

            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(unit.ToBson());

            foreach (Entity entity in unit.Components.Values)
            {
                Type type = entity.GetType();
                //判断Entity是否实现IUnitCache接口
                if (!typeof(IUnitCache).IsAssignableFrom(type))
                    continue;
                message.EntityTypes.Add(type.FullName);
                byte[] bytes = entity.ToBson();
                message.EntityBytes.Add(bytes);

                EventSystem.Instance.Invoke(SceneType.UnitCache, new AddToBytes
                {
                    unit = unit,
                    type = type,
                    bytes = bytes
                });
            }

            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetOneBySceneType(unit.Zone(), SceneType.UnitCache);
            unit.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId, message).NoContext();
        }
    }
}