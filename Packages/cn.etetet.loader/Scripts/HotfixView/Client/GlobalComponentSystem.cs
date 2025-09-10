using UnityEngine;

namespace ET
{
    [EntitySystemOf(typeof(GlobalComponent))]
    public static partial class GlobalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GlobalComponent self)
        {
            self.Unit = GameObject.Find("/Global/Unit").transform;
            self.GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
        }
    }
}