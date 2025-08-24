using System;

namespace ET.Server
{
    public struct UnitGetComponent
    {
        public EntityRef<Unit> unit;
        public Type Type;
    }
}