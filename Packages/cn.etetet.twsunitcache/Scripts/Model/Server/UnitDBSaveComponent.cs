using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class UnitDBSaveComponent : Entity, IAwake,IDestroy
    {
        public long Timer;
        public HashSet<Type> EntityChangeTypeSet { get; } = new HashSet<Type>();
        public Dictionary<Type,byte[]> Bytes { get; } = new Dictionary<Type,byte[]>();
    }
}

