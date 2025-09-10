using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    // *************************
    // ******* 通用容器相关 *********
    // *************************
    [MemoryPackable]
    [Message(KnapsackOuter.ItemProto)]
    public partial class ItemProto : MessageObject
    {
        public static ItemProto Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<ItemProto>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(1)]
        public int ContainerTyper { get; set; }

        [MemoryPackOrder(2)]
        public long Id { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ConfigId = default;
            this.ContainerTyper = default;
            this.Id = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.M2C_UpdateItemInfo)]
    public partial class M2C_UpdateItemInfo : MessageObject, IMessage
    {
        public static M2C_UpdateItemInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_UpdateItemInfo>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int Op { get; set; }

        [MemoryPackOrder(1)]
        public ItemProto ItemInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Op = default;
            this.ItemInfo = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.C2M_GetAllKnapsack)]
    [ResponseType(nameof(M2C_GetAllKnapsack))]
    public partial class C2M_GetAllKnapsack : MessageObject, ILocationRequest
    {
        public static C2M_GetAllKnapsack Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_GetAllKnapsack>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.M2C_GetAllKnapsack)]
    public partial class M2C_GetAllKnapsack : MessageObject, ILocationResponse
    {
        public static M2C_GetAllKnapsack Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_GetAllKnapsack>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ItemProto> ItemList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ItemList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.C2M_AddKnapsackItem)]
    [ResponseType(nameof(M2C_AddKnapsackItem))]
    public partial class C2M_AddKnapsackItem : MessageObject, ILocationRequest
    {
        public static C2M_AddKnapsackItem Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_AddKnapsackItem>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int ContainerType { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ConfigId = default;
            this.ContainerType = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.M2C_AddKnapsackItem)]
    public partial class M2C_AddKnapsackItem : MessageObject, ILocationResponse
    {
        public static M2C_AddKnapsackItem Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_AddKnapsackItem>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.C2M_RemoveKnapsackItem)]
    [ResponseType(nameof(M2C_RemoveKnapsackItem))]
    public partial class C2M_RemoveKnapsackItem : MessageObject, ILocationRequest
    {
        public static C2M_RemoveKnapsackItem Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_RemoveKnapsackItem>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int ContainerType { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ConfigId = default;
            this.ContainerType = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(KnapsackOuter.M2C_RemoveKnapsackItem)]
    public partial class M2C_RemoveKnapsackItem : MessageObject, ILocationResponse
    {
        public static M2C_RemoveKnapsackItem Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_RemoveKnapsackItem>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Recycle(this);
        }
    }

    public static class KnapsackOuter
    {
        public const ushort ItemProto = 50002;
        public const ushort M2C_UpdateItemInfo = 50003;
        public const ushort C2M_GetAllKnapsack = 50004;
        public const ushort M2C_GetAllKnapsack = 50005;
        public const ushort C2M_AddKnapsackItem = 50006;
        public const ushort M2C_AddKnapsackItem = 50007;
        public const ushort C2M_RemoveKnapsackItem = 50008;
        public const ushort M2C_RemoveKnapsackItem = 50009;
    }
}