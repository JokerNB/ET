using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(LoginOuter.Main2NetClient_Login)]
    [ResponseType(nameof(NetClient2Main_Login))]
    public partial class Main2NetClient_Login : MessageObject, IRequest
    {
        public static Main2NetClient_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Main2NetClient_Login>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int OwnerFiberId { get; set; }

        [MemoryPackOrder(2)]
        public string Address { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [MemoryPackOrder(3)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MemoryPackOrder(4)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OwnerFiberId = default;
            this.Address = default;
            this.Account = default;
            this.Password = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.NetClient2Main_Login)]
    public partial class NetClient2Main_Login : MessageObject, IResponse
    {
        public static NetClient2Main_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<NetClient2Main_Login>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Token = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.Main2NetClient_LoginGame)]
    [ResponseType(nameof(NetClient2Main_LoginGame))]
    public partial class Main2NetClient_LoginGame : MessageObject, IRequest
    {
        public static Main2NetClient_LoginGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Main2NetClient_LoginGame>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Account { get; set; }

        [MemoryPackOrder(2)]
        public long RealmKey { get; set; }

        [MemoryPackOrder(3)]
        public string GateAddress { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;
            this.RealmKey = default;
            this.GateAddress = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.NetClient2Main_LoginGame)]
    public partial class NetClient2Main_LoginGame : MessageObject, IResponse
    {
        public static NetClient2Main_LoginGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<NetClient2Main_LoginGame>(isFromPool);
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
    [Message(LoginOuter.C2G_Ping)]
    [ResponseType(nameof(G2C_Ping))]
    public partial class C2G_Ping : MessageObject, ISessionRequest
    {
        public static C2G_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2G_Ping>(isFromPool);
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
    [Message(LoginOuter.G2C_Ping)]
    public partial class G2C_Ping : MessageObject, ISessionResponse
    {
        public static G2C_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2C_Ping>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Time { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Time = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_LoginAccount)]
    [ResponseType(nameof(R2C_LoginAccount))]
    public partial class C2R_LoginAccount : MessageObject, ISessionRequest
    {
        public static C2R_LoginAccount Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_LoginAccount>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string AccountName { get; set; }

        [MemoryPackOrder(2)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountName = default;
            this.Password = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.R2C_LoginAccount)]
    public partial class R2C_LoginAccount : MessageObject, ISessionResponse
    {
        public static R2C_LoginAccount Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<R2C_LoginAccount>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Token = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.ServerInfoProto)]
    public partial class ServerInfoProto : MessageObject
    {
        public static ServerInfoProto Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<ServerInfoProto>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int Id { get; set; }

        [MemoryPackOrder(1)]
        public int Status { get; set; }

        [MemoryPackOrder(2)]
        public string ServerName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.Status = default;
            this.ServerName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_GetServerInfos)]
    [ResponseType(nameof(R2C_GetServerInfos))]
    public partial class C2R_GetServerInfos : MessageObject, ISessionRequest
    {
        public static C2R_GetServerInfos Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_GetServerInfos>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string Account { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.Account = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.R2C_GetServerInfos)]
    public partial class R2C_GetServerInfos : MessageObject, ISessionResponse
    {
        public static R2C_GetServerInfos Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<R2C_GetServerInfos>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ServerInfoProto> ServerInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ServerInfoList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_GetRealmKey)]
    [ResponseType(nameof(R2C_GetRealmKey))]
    public partial class C2R_GetRealmKey : MessageObject, ISessionRequest
    {
        public static C2R_GetRealmKey Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_GetRealmKey>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string AccountName { get; set; }

        [MemoryPackOrder(3)]
        public int ServerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.AccountName = default;
            this.ServerId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.R2C_GetRealmKey)]
    public partial class R2C_GetRealmKey : MessageObject, ISessionResponse
    {
        public static R2C_GetRealmKey Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<R2C_GetRealmKey>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string Address { get; set; }

        [MemoryPackOrder(4)]
        public long Key { get; set; }

        [MemoryPackOrder(5)]
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
            this.Address = default;
            this.Key = default;
            this.GateId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2G_LoginGameGate)]
    [ResponseType(nameof(G2C_LoginGameGate))]
    public partial class C2G_LoginGameGate : MessageObject, ISessionRequest
    {
        public static C2G_LoginGameGate Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2G_LoginGameGate>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Key { get; set; }

        [MemoryPackOrder(2)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Key = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.G2C_LoginGameGate)]
    public partial class G2C_LoginGameGate : MessageObject, ISessionResponse
    {
        public static G2C_LoginGameGate Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2C_LoginGameGate>(isFromPool);
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
    [Message(LoginOuter.C2G_EnterGame)]
    [ResponseType(nameof(G2C_EnterGame))]
    public partial class C2G_EnterGame : MessageObject, ISessionRequest
    {
        public static C2G_EnterGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2G_EnterGame>(isFromPool);
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
    [Message(LoginOuter.G2C_EnterGame)]
    public partial class G2C_EnterGame : MessageObject, ISessionResponse
    {
        public static G2C_EnterGame Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<G2C_EnterGame>(isFromPool);
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
    [Message(LoginOuter.C2R_GetArchiveListRequest)]
    [ResponseType(nameof(C2R_GetArchiveListResponse))]
    public partial class C2R_GetArchiveListRequest : MessageObject, ISessionRequest
    {
        public static C2R_GetArchiveListRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_GetArchiveListRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_GetArchiveListResponse)]
    public partial class C2R_GetArchiveListResponse : MessageObject, ISessionResponse
    {
        public static C2R_GetArchiveListResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_GetArchiveListResponse>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ArchiveInfoProto> ArchiveInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ArchiveInfoList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_AddNewArchiveRequest)]
    [ResponseType(nameof(C2R_AddNewArchiveResponse))]
    public partial class C2R_AddNewArchiveRequest : MessageObject, ISessionRequest
    {
        public static C2R_AddNewArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_AddNewArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string AccountName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.AccountName = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_AddNewArchiveResponse)]
    public partial class C2R_AddNewArchiveResponse : MessageObject, ISessionResponse
    {
        public static C2R_AddNewArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_AddNewArchiveResponse>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ArchiveInfoProto> ArchiveInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ArchiveInfoList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_RemoveArchiveRequest)]
    [ResponseType(nameof(C2R_RemoveArchiveResponse))]
    public partial class C2R_RemoveArchiveRequest : MessageObject, ISessionRequest
    {
        public static C2R_RemoveArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_RemoveArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string AccountName { get; set; }

        [MemoryPackOrder(3)]
        public int ArchiveNum { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.AccountName = default;
            this.ArchiveNum = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2R_RemoveArchiveResponse)]
    public partial class C2R_RemoveArchiveResponse : MessageObject, ISessionResponse
    {
        public static C2R_RemoveArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2R_RemoveArchiveResponse>(isFromPool);
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
    [Message(LoginOuter.C2M_UpdateArchiveRequest)]
    [ResponseType(nameof(C2M_UpdateArchiveResponse))]
    public partial class C2M_UpdateArchiveRequest : MessageObject, ILocationRequest
    {
        public static C2M_UpdateArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_UpdateArchiveRequest>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public ArchiveInfoProto ArchiveInfoProto { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.ArchiveInfoProto = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LoginOuter.C2M_UpdateArchiveResponse)]
    public partial class C2M_UpdateArchiveResponse : MessageObject, ILocationResponse
    {
        public static C2M_UpdateArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_UpdateArchiveResponse>(isFromPool);
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
    [Message(LoginOuter.C2G_SelectOrAddArchiveRequest)]
    [ResponseType(nameof(C2G_SelectOrAddArchiveResponse))]
    public partial class C2G_SelectOrAddArchiveRequest : MessageObject, ISessionRequest
    {
        public static C2G_SelectOrAddArchiveRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2G_SelectOrAddArchiveRequest>(isFromPool);
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
    [Message(LoginOuter.C2G_SelectOrAddArchiveResponse)]
    public partial class C2G_SelectOrAddArchiveResponse : MessageObject, ISessionResponse
    {
        public static C2G_SelectOrAddArchiveResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2G_SelectOrAddArchiveResponse>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(2)]
        public int ArchiveNum { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ArchiveNum = default;

            ObjectPool.Recycle(this);
        }
    }

    public static class LoginOuter
    {
        public const ushort Main2NetClient_Login = 1001;
        public const ushort NetClient2Main_Login = 1002;
        public const ushort Main2NetClient_LoginGame = 1003;
        public const ushort NetClient2Main_LoginGame = 1004;
        public const ushort C2G_Ping = 1005;
        public const ushort G2C_Ping = 1006;
        public const ushort C2R_LoginAccount = 1007;
        public const ushort R2C_LoginAccount = 1008;
        public const ushort ServerInfoProto = 1009;
        public const ushort C2R_GetServerInfos = 1010;
        public const ushort R2C_GetServerInfos = 1011;
        public const ushort C2R_GetRealmKey = 1012;
        public const ushort R2C_GetRealmKey = 1013;
        public const ushort C2G_LoginGameGate = 1014;
        public const ushort G2C_LoginGameGate = 1015;
        public const ushort C2G_EnterGame = 1016;
        public const ushort G2C_EnterGame = 1017;
        public const ushort C2R_GetArchiveListRequest = 1018;
        public const ushort C2R_GetArchiveListResponse = 1019;
        public const ushort C2R_AddNewArchiveRequest = 1020;
        public const ushort C2R_AddNewArchiveResponse = 1021;
        public const ushort C2R_RemoveArchiveRequest = 1022;
        public const ushort C2R_RemoveArchiveResponse = 1023;
        public const ushort C2M_UpdateArchiveRequest = 1024;
        public const ushort C2M_UpdateArchiveResponse = 1025;
        public const ushort C2G_SelectOrAddArchiveRequest = 1026;
        public const ushort C2G_SelectOrAddArchiveResponse = 1027;
    }
}