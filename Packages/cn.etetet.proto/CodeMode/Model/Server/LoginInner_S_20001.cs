using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(LoginInner.R2G_GetLoginKey)]
    [ResponseType(nameof(G2R_GetLoginKey))]
    public partial class R2G_GetLoginKey : MessageObject, IRequest
    {
        public static R2G_GetLoginKey Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<R2G_GetLoginKey>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Account { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.G2R_GetLoginKey)]
    public partial class G2R_GetLoginKey : MessageObject, IResponse
    {
        public static G2R_GetLoginKey Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2R_GetLoginKey>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Key { get; set; }

        [MemoryPackOrder(4)]
        public long GateId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Key = default;
            this.GateId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.G2M_SessionDisconnect)]
    public partial class G2M_SessionDisconnect : MessageObject, ILocationMessage
    {
        public static G2M_SessionDisconnect Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2M_SessionDisconnect>(isFromPool);
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
    [Message(LoginInner.R2L_LoginAccountRequest)]
    [ResponseType(nameof(L2R_LoginAccountResponse))]
    public partial class R2L_LoginAccountRequest : MessageObject, IRequest
    {
        public static R2L_LoginAccountRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<R2L_LoginAccountRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.L2R_LoginAccountResponse)]
    public partial class L2R_LoginAccountResponse : MessageObject, IResponse
    {
        public static L2R_LoginAccountResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<L2R_LoginAccountResponse>(isFromPool);
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
    [Message(LoginInner.L2G_DisconnectGateUnit)]
    [ResponseType(nameof(G2L_DisconnectGateUnit))]
    public partial class L2G_DisconnectGateUnit : MessageObject, IRequest
    {
        public static L2G_DisconnectGateUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<L2G_DisconnectGateUnit>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.G2L_DisconnectGateUnit)]
    public partial class G2L_DisconnectGateUnit : MessageObject, IResponse
    {
        public static G2L_DisconnectGateUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2L_DisconnectGateUnit>(isFromPool);
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
    [Message(LoginInner.G2L_AddLoginRecord)]
    [ResponseType(nameof(L2G_AddLoginRecord))]
    public partial class G2L_AddLoginRecord : MessageObject, IRequest
    {
        public static G2L_AddLoginRecord Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2L_AddLoginRecord>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        [MemoryPackOrder(2)]
        public int ServerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;
            this.ServerId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.L2G_AddLoginRecord)]
    public partial class L2G_AddLoginRecord : MessageObject, IResponse
    {
        public static L2G_AddLoginRecord Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<L2G_AddLoginRecord>(isFromPool);
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
    [Message(LoginInner.G2M_RequestExitGame)]
    [ResponseType(nameof(M2G_RequestExitGame))]
    public partial class G2M_RequestExitGame : MessageObject, ILocationRequest
    {
        public static G2M_RequestExitGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2M_RequestExitGame>(isFromPool);
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
    [Message(LoginInner.M2G_RequestExitGame)]
    public partial class M2G_RequestExitGame : MessageObject, ILocationResponse
    {
        public static M2G_RequestExitGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2G_RequestExitGame>(isFromPool);
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
    [Message(LoginInner.G2L_RemoveLoginRecord)]
    [ResponseType(nameof(L2G_RemoveLoginRecord))]
    public partial class G2L_RemoveLoginRecord : MessageObject, IRequest
    {
        public static G2L_RemoveLoginRecord Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2L_RemoveLoginRecord>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        [MemoryPackOrder(2)]
        public int ServerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;
            this.ServerId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.L2G_RemoveLoginRecord)]
    public partial class L2G_RemoveLoginRecord : MessageObject, IResponse
    {
        public static L2G_RemoveLoginRecord Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<L2G_RemoveLoginRecord>(isFromPool);
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
    [Message(LoginInner.G2M_SecondLogin)]
    [ResponseType(nameof(M2G_SecondLogin))]
    public partial class G2M_SecondLogin : MessageObject, ILocationRequest
    {
        public static G2M_SecondLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2M_SecondLogin>(isFromPool);
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
    [Message(LoginInner.M2G_SecondLogin)]
    public partial class M2G_SecondLogin : MessageObject, ILocationResponse
    {
        public static M2G_SecondLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2G_SecondLogin>(isFromPool);
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
    [Message(LoginInner.ArchiveInfoProto)]
    public partial class ArchiveInfoProto : MessageObject
    {
        public static ArchiveInfoProto Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<ArchiveInfoProto>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public long AccountHash { get; set; }

        [MemoryPackOrder(1)]
        public List<long> Recruits { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.AccountHash = default;
            this.Recruits.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_GetArchiveListRequest)]
    [ResponseType(nameof(Other2Archive_GetArchiveListResponse))]
    public partial class Other2Archive_GetArchiveListRequest : MessageObject, IRequest
    {
        public static Other2Archive_GetArchiveListRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_GetArchiveListRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_GetArchiveListResponse)]
    public partial class Other2Archive_GetArchiveListResponse : MessageObject, IResponse
    {
        public static Other2Archive_GetArchiveListResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_GetArchiveListResponse>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ArchiveInfoProto> ArchiveInfos { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ArchiveInfos.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_AddNewArchiveRequest)]
    [ResponseType(nameof(Other2Archive_AddNewArchiveResponse))]
    public partial class Other2Archive_AddNewArchiveRequest : MessageObject, IRequest
    {
        public static Other2Archive_AddNewArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_AddNewArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_AddNewArchiveResponse)]
    public partial class Other2Archive_AddNewArchiveResponse : MessageObject, IResponse
    {
        public static Other2Archive_AddNewArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_AddNewArchiveResponse>(isFromPool);
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
    [Message(LoginInner.Other2Archive_UpdateArchiveRequest)]
    [ResponseType(nameof(Other2Archive_UpdateArchiveResponse))]
    public partial class Other2Archive_UpdateArchiveRequest : MessageObject, IRequest
    {
        public static Other2Archive_UpdateArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_UpdateArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(3)]
        public ArchiveInfoProto ArchiveInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ArchiveInfo = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_UpdateArchiveResponse)]
    public partial class Other2Archive_UpdateArchiveResponse : MessageObject, IResponse
    {
        public static Other2Archive_UpdateArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_UpdateArchiveResponse>(isFromPool);
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
    [Message(LoginInner.Other2Archive_RemoveArchiveRequest)]
    [ResponseType(nameof(Other2Archive_RemoveArchiveResponse))]
    public partial class Other2Archive_RemoveArchiveRequest : MessageObject, IRequest
    {
        public static Other2Archive_RemoveArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_RemoveArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        [MemoryPackOrder(2)]
        public int ArchiveNum { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;
            this.ArchiveNum = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginInner.Other2Archive_RemoveArchiveResponse)]
    public partial class Other2Archive_RemoveArchiveResponse : MessageObject, IResponse
    {
        public static Other2Archive_RemoveArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2Archive_RemoveArchiveResponse>(isFromPool);
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

    public static class LoginInner
    {
        public const ushort R2G_GetLoginKey = 20002;
        public const ushort G2R_GetLoginKey = 20003;
        public const ushort G2M_SessionDisconnect = 20004;
        public const ushort R2L_LoginAccountRequest = 20005;
        public const ushort L2R_LoginAccountResponse = 20006;
        public const ushort L2G_DisconnectGateUnit = 20007;
        public const ushort G2L_DisconnectGateUnit = 20008;
        public const ushort G2L_AddLoginRecord = 20009;
        public const ushort L2G_AddLoginRecord = 20010;
        public const ushort G2M_RequestExitGame = 20011;
        public const ushort M2G_RequestExitGame = 20012;
        public const ushort G2L_RemoveLoginRecord = 20013;
        public const ushort L2G_RemoveLoginRecord = 20014;
        public const ushort G2M_SecondLogin = 20015;
        public const ushort M2G_SecondLogin = 20016;
        public const ushort ArchiveInfoProto = 20017;
        public const ushort Other2Archive_GetArchiveListRequest = 20018;
        public const ushort Other2Archive_GetArchiveListResponse = 20019;
        public const ushort Other2Archive_AddNewArchiveRequest = 20020;
        public const ushort Other2Archive_AddNewArchiveResponse = 20021;
        public const ushort Other2Archive_UpdateArchiveRequest = 20022;
        public const ushort Other2Archive_UpdateArchiveResponse = 20023;
        public const ushort Other2Archive_RemoveArchiveRequest = 20024;
        public const ushort Other2Archive_RemoveArchiveResponse = 20025;
    }
}