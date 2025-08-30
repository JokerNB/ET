using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 释放组件，释放技能
    /// </summary>
    [ComponentOf(typeof(Unit_Client))]
    public class CastComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<int, EntityRef<Cast>> Casts = new Dictionary<int, EntityRef<Cast>>();
    }
}