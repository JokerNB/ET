using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.UnitCache)]
    [FriendOfAttribute(typeof(ET.Server.UnitCacheComponent))]
    public class Other2UnitCache_GetUnitHandler : MessageHandler<Scene, Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string, Entity> dic = ObjectPool.Fetch(typeof(Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                if (request.ComponentNameList.Count == 0)
                {
                    string unitName = typeof(Unit).FullName ?? "ET.Unit";
                    dic.Add(unitName, null);
                    foreach (var s in unitCacheComponent.UnitCacheKeyList)
                    {
                        if (s == unitName)
                            continue;
                        dic.Add(s, null);
                    }
                }
                else
                {
                    foreach (var s in request.ComponentNameList)
                    {
                        dic.Add(s, null);
                    }
                }

                long unitId = request.UnitId;
                CoroutineLockComponent coroutineLockComponent = scene.GetComponent<CoroutineLockComponent>();
                using (await coroutineLockComponent.Wait(CoroutineLockType.UnitCacheGet, unitId))
                {
                    unitCacheComponent.CallCache(unitId);
                    using (ListComponent<string> keyList = ListComponent<string>.Create())
                    {
                        foreach (var key in dic.Keys)
                        {
                            keyList.Add(key);
                        }

                        foreach (var key in keyList)
                        {
                            Entity entity = await unitCacheComponent.Get(key, request.UnitId);
                            dic[key] = entity;
                        }
                    }

                    foreach (var info in dic)
                    {
                        response.ComponentNameLIst.Add(info.Key);
                        response.EntityList.Add(info.Value?.ToBson() ?? null);
                    }
                }
            }
            finally
            {
                dic.Clear();
                ObjectPool.Recycle(dic);
            }
        }
    }
}