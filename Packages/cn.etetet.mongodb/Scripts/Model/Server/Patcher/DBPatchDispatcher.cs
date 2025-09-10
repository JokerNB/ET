using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    public class DBPatchDispatcher: Singleton<DBPatchDispatcher>, ISingletonAwake
    {
        private readonly SortedDictionary<int, IDBPatcher> dispatcher = new();
        
        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof (DBPatcherAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (DBPatcherAttribute), false);

                foreach (object attr in attrs)
                {
                    DBPatcherAttribute dbPatcherAttribute = (DBPatcherAttribute) attr;

                    object obj = Activator.CreateInstance(type);

                    IDBPatcher idbPatcher = obj as IDBPatcher;

                    if (idbPatcher == null)
                    {
                        Log.Error($"{obj.GetType().Name} 没有继承IDBPatch");
                        continue;
                    }

                    dispatcher.Add(dbPatcherAttribute.Version, idbPatcher);
                }
            }
        }

        public int MaxVersion()
        {
            if (this.dispatcher.Count == 0)
            {
                return 0;
            }
            return this.dispatcher.Last().Key;
        }
        
        public async ETTask Run(DBComponent dbComponent, int zone, int from)
        {
            foreach (var kv in dispatcher)
            {
                if (kv.Key <= from)
                {
                    continue;
                }

                await kv.Value.Run(dbComponent, zone);
                Log.Console($"patch db to version: {kv.Key}");
            }
        }
    }
}