using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class BuffComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<int, EntityRef<Buff>> ConfigIdBuffs = new Dictionary<int, EntityRef<Buff>>();
    }
}