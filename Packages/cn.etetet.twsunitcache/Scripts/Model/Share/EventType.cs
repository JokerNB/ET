using System;

namespace ET
{
    public struct UnitGetComponent
    {
        public EntityRef<Unit> unit;
        public Type Type;
    }
    
    public struct UnitAddComponentByGetNull
    {
        public EntityRef<Unit> unit;
        public Type type;
    }
}