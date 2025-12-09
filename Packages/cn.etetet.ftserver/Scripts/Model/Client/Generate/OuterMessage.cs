using ProtoBuf;
using System;
using MemoryPack;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618
// ReSharper disable InconsistentNaming
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable RedundantNameQualifier
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable RedundantUsingDirective
namespace Fantasy
{
    [Serializable]
    [ProtoContract]
    public partial class C2G_TestMessage : AMessage, IMessage
    {
        public static C2G_TestMessage Create(Scene scene)
        {
            return scene.MessagePoolComponent.Rent<C2G_TestMessage>();
        }

        public override void Dispose()
        {
            Tag = default;
#if FANTASY_NET || FANTASY_UNITY
            GetScene().MessagePoolComponent.Return<C2G_TestMessage>(this);
#endif
        }
public uint OpCode() { return OuterOpcode.C2G_TestMessage; } 
        [ProtoMember(1)]
        public string Tag { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class C2G_TestRequest : AMessage, IRequest
    {
        public static C2G_TestRequest Create(Scene scene)
        {
            return scene.MessagePoolComponent.Rent<C2G_TestRequest>();
        }

        public override void Dispose()
        {
            Tag = default;
#if FANTASY_NET || FANTASY_UNITY
            GetScene().MessagePoolComponent.Return<C2G_TestRequest>(this);
#endif
        }
public uint OpCode() { return OuterOpcode.C2G_TestRequest; } 
        [ProtoIgnore]
        public G2C_TestResponse ResponseType { get; set; }
        [ProtoMember(1)]
        public string Tag { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class G2C_TestResponse : AMessage, IResponse
    {
        public static G2C_TestResponse Create(Scene scene)
        {
            return scene.MessagePoolComponent.Rent<G2C_TestResponse>();
        }

        public override void Dispose()
        {
            Tag = default;
#if FANTASY_NET || FANTASY_UNITY
            GetScene().MessagePoolComponent.Return<G2C_TestResponse>(this);
#endif
        }
public uint OpCode() { return OuterOpcode.G2C_TestResponse; } 
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
        [ProtoMember(2)]
        public string Tag { get; set; }
    }
}