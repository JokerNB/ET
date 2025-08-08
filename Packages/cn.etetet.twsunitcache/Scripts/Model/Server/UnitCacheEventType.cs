using System;

namespace ET.Server
{
    public struct LRUUnitCacheDelete
    {
        public EntityRef<LRUCache> LRUCache;
        public long key;
    }

    public struct AddToBytes
    {
        public EntityRef<Unit> unit;
        public Type type;
        public byte[] bytes;
    }
}