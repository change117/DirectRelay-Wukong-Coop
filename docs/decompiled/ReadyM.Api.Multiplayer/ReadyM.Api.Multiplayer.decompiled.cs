using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Friflo.Json.Fliox;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Helpers;
using ReadyM.Api.Idents;
using ReadyM.Api.Multiplayer.Client;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Managers;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.ECS.Values;
using ReadyM.Api.Multiplayer.Extensions;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Api.Multiplayer.Protocol;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.Api.Serialization;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: InternalsVisibleTo("ReadyM.Relay.Server")]
[assembly: InternalsVisibleTo("ReadyM.Relay.Tests")]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("ReadyM.Api.Multiplayer")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+63464565c8693c0537c1bd91e59ef593e8f0bde2")]
[assembly: AssemblyProduct("ReadyM.Api.Multiplayer")]
[assembly: AssemblyTitle("ReadyM.Api.Multiplayer")]
[assembly: AssemblyVersion("1.0.422.1357")]
[module: System.Runtime.CompilerServices.RefSafetyRules(11)]
namespace Microsoft.CodeAnalysis
{
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	internal sealed class EmbeddedAttribute : Attribute
	{
	}
}
namespace System.Runtime.CompilerServices
{
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	internal sealed class IsReadOnlyAttribute : Attribute
	{
	}
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte P_0)
		{
			NullableFlags = new byte[1] { P_0 };
		}

		public NullableAttribute(byte[] P_0)
		{
			NullableFlags = P_0;
		}
	}
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableContextAttribute : Attribute
	{
		public readonly byte Flag;

		public NullableContextAttribute(byte P_0)
		{
			Flag = P_0;
		}
	}
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
	internal sealed class RefSafetyRulesAttribute : Attribute
	{
		public readonly int Version;

		public RefSafetyRulesAttribute(int P_0)
		{
			Version = P_0;
		}
	}
}
namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
	internal sealed class NotNullAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute()
	{
		public bool ReturnValue { get; } = returnValue;
	}
}
namespace ReadyM.Api.Multiplayer
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServerRpcEventAttribute : Attribute
	{
		public ServerRpcEventAttribute(string name)
		{
		}
	}
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServerRpcHandlerAttribute : Attribute
	{
		public ServerRpcHandlerAttribute(string name)
		{
		}
	}
}
namespace ReadyM.Api.Multiplayer.Server
{
	public interface IRelayServer : IDisposable
	{
		PendingActionScheduler<IRelayServerNetworkThreadContext> Scheduler { get; }

		event Func<CancellationToken, Task>? OnServerStarting;

		event Action? OnServerStarted;

		event Action? OnServerStopped;

		event Action<IRelayServerNetworkThreadContext>? OnServerUpdate;

		event Action<IRelayServerNetworkThreadContext, PlayerId, Guid>? OnPlayerConnected;

		event Action<IRelayServerNetworkThreadContext, PlayerId, Guid, DisconnectReason>? OnPlayerDisconnected;

		event Action<IRelayServerNetworkThreadContext, AreaId>? OnAreaCreated;

		event Action<IRelayServerNetworkThreadContext, AreaId>? OnAreaDeleted;

		event Action<IRelayServerNetworkThreadContext, PlayerId, AreaId>? OnPlayerJoinedArea;

		event Action<IRelayServerNetworkThreadContext, PlayerId, AreaId>? OnPlayerLeftArea;

		event Func<RelayConnectionOptions, ConnectionRequest, bool>? OnCanConnect;

		Task StartAsync(CancellationToken token);

		Task RunAsync(CancellationToken token);

		void Stop();

		void AddBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayServerNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayServerNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void AddServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayServerNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayServerNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void SendToOne(PlayerId playerId, NetDataWriter writer, DeliveryMethod deliveryMethod);

		void SendToAll(ReadyM.Api.Helpers.ReadOnlyList<PlayerId> playerIds, NetDataWriter writer, DeliveryMethod deliveryMethod);

		void SendToAllExcept(ReadyM.Api.Helpers.ReadOnlyList<PlayerId> playerIds, PlayerId exceptPlayerId, NetDataWriter writer, DeliveryMethod deliveryMethod);

		int? GetMaxPacketSize(DeliveryMethod deliveryMethod);
	}
	public interface IRelayServerNetworkThreadContext
	{
		ReadOnlyDictionary<PlayerId, NetPeer> PeerByPlayer { get; }

		ReadyM.Api.Helpers.ReadOnlyList<PlayerId> AllPlayers { get; }

		ReadyM.Api.Helpers.ReadOnlyList<AreaId> Areas { get; }

		ReadOnlyDictionary<PlayerId, Guid> UserGuids { get; }

		ReadyM.Api.Helpers.ReadOnlyList<PlayerId> GetAreaPlayers(AreaId areaId);
	}
}
namespace ReadyM.Api.Multiplayer.Server.Rpc
{
	public readonly struct ServerRpcEventEntry : IEquatable<ServerRpcEventEntry>, IComparable<ServerRpcEventEntry>
	{
		public RelayMessageCode EventCode { get; }

		public ServerRpcEventEntry(RelayMessageCode eventCode)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Invalid server rpc event code");
			}
			EventCode = eventCode;
		}

		public override bool Equals(object? obj)
		{
			if (obj is ServerRpcEventEntry other)
			{
				return Equals(other);
			}
			return false;
		}

		public bool Equals(ServerRpcEventEntry other)
		{
			return EventCode == other.EventCode;
		}

		public override int GetHashCode()
		{
			return EventCode.GetHashCode();
		}

		public static bool operator ==(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return !(left == right);
		}

		public int CompareTo(ServerRpcEventEntry other)
		{
			return EventCode.CompareTo(other.EventCode);
		}

		public static bool operator <(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator >(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator <=(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator >=(ServerRpcEventEntry left, ServerRpcEventEntry right)
		{
			return left.CompareTo(right) >= 0;
		}
	}
	public readonly struct ServerRpcEventRange
	{
		private readonly ServerRpcEventEntry _minEventCode;

		private readonly ServerRpcEventEntry _maxEventCode;

		public RelayMessageCode MinEventCode => _minEventCode.EventCode;

		public RelayMessageCode MaxEventCode => _maxEventCode.EventCode;

		public ServerRpcEventRange(RelayMessageCode minEventCode, RelayMessageCode maxEventCode)
			: this(new ServerRpcEventEntry(minEventCode), new ServerRpcEventEntry(maxEventCode))
		{
		}

		public ServerRpcEventRange(ServerRpcEventEntry minEventCode, ServerRpcEventEntry maxEventCode)
		{
			if (minEventCode > maxEventCode)
			{
				throw new ArgumentException("Min event code cannot be greater than max event code", "minEventCode");
			}
			_minEventCode = minEventCode;
			_maxEventCode = maxEventCode;
		}
	}
}
namespace ReadyM.Api.Multiplayer.Protocol
{
	public static class Constants
	{
		public static PlayerId UnsetPeerId = PlayerId.Invalid;

		public const string RoomPropertyAnnotationPrefix = "roomProperty/";

		public const string AssignedPlayerList = "assignedPlayers";

		public const string VirtualServerId = "serverId";

		public const string RegionLabel = "region";

		public const string AgonesLastAllocated = "agones.dev/last-allocated";

		public const int ServerNetworkTickRateMs = 5;

		public const int ClientNetworkTickRateMs = 2;

		public const int ShimClientTickRateMs = 1;

		public const int ServerEcsUpdateRateMs = 16;

		public const int ClientEcsUpdateRateMs = 16;

		public const int ClientConnectionTimeoutMs = 5000;
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct CustomRelayEventHeader
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<CustomRelayEventHeader>
		{
			public override CustomRelayEventHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, CustomRelayEventHeader value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public RelayMessageCode EventCode;

		public PlayerId Sender;

		public PlayerId[]? Peers;

		public RelayMode RelayMode;

		public CustomRelayEventHeader(RelayMessageCode eventCode, PlayerId sender, PlayerId[]? peers, RelayMode relayMode = RelayMode.AreaOfInterestOthers)
		{
			EventCode = eventCode;
			Sender = sender;
			Peers = peers;
			RelayMode = relayMode;
		}

		public static CustomRelayEventHeader TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			RelayMessageCode eventCode = RelayMessageCode.MinClientRpcEvent;
			PlayerId sender = default(PlayerId);
			PlayerId[] peers = null;
			RelayMode relayMode = RelayMode.AreaOfInterestOthers;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "eventCode":
					eventCode = JsonSerializer.Deserialize<RelayMessageCode>(ref reader, options);
					break;
				case "sender":
					sender = PlayerId.TextDeserialize(ref reader, options);
					break;
				case "peers":
					peers = JsonSerializer.Deserialize<PlayerId[]>(ref reader, options);
					break;
				case "relayMode":
					relayMode = JsonSerializer.Deserialize<RelayMode>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new CustomRelayEventHeader(eventCode, sender, peers, relayMode);
		}

		public static void TextSerialize(Utf8JsonWriter writer, CustomRelayEventHeader obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("eventCode");
			JsonSerializer.Serialize(writer, obj.EventCode, options);
			writer.WritePropertyName("sender");
			PlayerId.TextSerialize(writer, obj.Sender, options);
			writer.WritePropertyName("peers");
			JsonSerializer.Serialize(writer, obj.Peers, options);
			writer.WritePropertyName("relayMode");
			JsonSerializer.Serialize(writer, obj.RelayMode, options);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.MapFields | SerializableMode.MapPublic)]
	public struct ServerEventHeader
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ServerEventHeader>
		{
			public override ServerEventHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ServerEventHeader value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public RelayMessageCode EventCode;

		public PlayerId Sender;

		public ServerEventHeader(RelayMessageCode eventCode, PlayerId sender)
		{
			EventCode = eventCode;
			Sender = sender;
		}

		public static ServerEventHeader TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			RelayMessageCode eventCode = RelayMessageCode.MinClientRpcEvent;
			PlayerId sender = default(PlayerId);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "eventCode")
				{
					eventCode = JsonSerializer.Deserialize<RelayMessageCode>(ref reader, options);
				}
				else if (text == "sender")
				{
					sender = PlayerId.TextDeserialize(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new ServerEventHeader(eventCode, sender);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ServerEventHeader obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("eventCode");
			JsonSerializer.Serialize(writer, obj.EventCode, options);
			writer.WritePropertyName("sender");
			PlayerId.TextSerialize(writer, obj.Sender, options);
			writer.WriteEndObject();
		}
	}
}
namespace ReadyM.Api.Multiplayer.Protocol.Enums
{
	public static class PlayerProperties
	{
		public const byte PlayerId = 1;
	}
	public enum RelayMessageCode : byte
	{
		HandshakeConnected = byte.MaxValue,
		RequestAreaEvent = 254,
		AreaEvent = 253,
		OtherPlayerConnectionEvent = 252,
		OtherPlayerAreaEvent = 251,
		EcsDelta = 250,
		MaxBuiltInEvent = 250,
		EcsSnapshot = 249,
		EcsCreateEntity = 248,
		EcsDeleteEntity = 247,
		EcsChangeOwnership = 246,
		RequestDownloadBlob = 245,
		DownloadBlobData = 244,
		RequestUploadBlob = 243,
		UploadBlobAck = 242,
		MinBuiltInEvent = 242,
		MaxAnyCustomEvent = 241,
		MaxServerRpcEvent = 241,
		MinServerRpcEvent = 150,
		MaxClientRpcEvent = 149,
		MinClientRpcEvent = 0,
		MinAnyCustomEvent = 0
	}
	public enum RelayMode : byte
	{
		AreaOfInterestOthers,
		AreaOfInterestAll,
		GlobalOthers,
		GlobalAll,
		EntityOwner,
		Peers
	}
}
namespace ReadyM.Api.Multiplayer.Idents
{
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct AreaId : INetSerializable, IEquatable<AreaId>
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<AreaId>
		{
			public override AreaId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, AreaId value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		private ushort _id;

		public static AreaId Invalid => default(AreaId);

		public static AreaId Max => new AreaId(ushort.MaxValue);

		public AreaId(ushort id)
		{
			_id = id;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_id);
		}

		public void Deserialize(NetDataReader reader)
		{
			_id = reader.GetUShort();
		}

		public bool Equals(AreaId other)
		{
			return _id == other._id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is AreaId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		public static bool operator ==(AreaId left, AreaId right)
		{
			return left._id == right._id;
		}

		public static bool operator !=(AreaId left, AreaId right)
		{
			return left._id != right._id;
		}

		public override string ToString()
		{
			if (_id != Invalid._id)
			{
				return $"AreaId[{_id}]";
			}
			return "AreaId.Invalid";
		}

		public static AreaId TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			ushort id = 0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string? text = reader.GetString();
				reader.Read();
				if (text == "_id")
				{
					id = reader.GetUInt16();
				}
				else
				{
					reader.Skip();
				}
			}
			return new AreaId(id);
		}

		public static void TextSerialize(Utf8JsonWriter writer, AreaId obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("_id", obj._id);
			writer.WriteEndObject();
		}
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayerId : INetSerializable, IEquatable<PlayerId>
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayerId>
		{
			public override PlayerId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayerId value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		private ushort _id;

		public ushort RawValue => _id;

		public static PlayerId Server => default(PlayerId);

		public static PlayerId Invalid => default(PlayerId);

		public static PlayerId Max => new PlayerId(ushort.MaxValue);

		public PlayerId(ushort id)
		{
			_id = id;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_id);
		}

		public void Deserialize(NetDataReader reader)
		{
			_id = reader.GetUShort();
		}

		public bool Equals(PlayerId other)
		{
			return _id == other._id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is PlayerId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		public static bool operator ==(PlayerId left, PlayerId right)
		{
			return left._id == right._id;
		}

		public static bool operator !=(PlayerId left, PlayerId right)
		{
			return left._id != right._id;
		}

		public override string ToString()
		{
			if (_id != Invalid._id)
			{
				return $"PlayerId[{_id}]";
			}
			return "PlayerId.Server";
		}

		public static PlayerId TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			ushort id = 0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string? text = reader.GetString();
				reader.Read();
				if (text == "_id")
				{
					id = reader.GetUInt16();
				}
				else
				{
					reader.Skip();
				}
			}
			return new PlayerId(id);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayerId obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("_id", obj._id);
			writer.WriteEndObject();
		}
	}
}
namespace ReadyM.Api.Multiplayer.Generators
{
	[AttributeUsage(AttributeTargets.Struct)]
	public sealed class DeriveINetSerializableAttribute(SerializableMode mode = SerializableMode.Default) : Attribute()
	{
		public readonly SerializableMode Mode = mode;
	}
	[AttributeUsage(AttributeTargets.Struct)]
	public sealed class DeriveINetworkedComponentAttribute(SerializableMode mode = SerializableMode.Default) : Attribute()
	{
		public readonly SerializableMode Mode = mode;
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ExcludeSerializableAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class IncludeSerializableAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class RpcEventAttribute : Attribute
	{
		public RpcEventAttribute(RelayMode relayMode)
		{
		}
	}
}
namespace ReadyM.Api.Multiplayer.Extensions
{
	public static class NetDataReaderExtensions
	{
		public static bool TryGetMetadataComponent(this NetDataReader reader, out MetadataComponent result)
		{
			if (reader.AvailableBytes >= 9)
			{
				result = reader.Get<MetadataComponent>();
				return true;
			}
			result = default(MetadataComponent);
			return false;
		}
	}
	public static class NetDataWriterExtensions
	{
		public static void PutCustomRelayEventHeader(this NetDataWriter writer, RelayMessageCode eventCode, PlayerId playerId, RelayMode relayMode)
		{
			if (relayMode == RelayMode.Peers)
			{
				throw new ArgumentException("Use PutCustomRelayEventHeader with PlayerId[] for RelayMode.Peers", "relayMode");
			}
			writer.Put((byte)eventCode);
			writer.Put<PlayerId>(playerId);
			byte b = (byte)relayMode;
			writer.Put(b);
		}

		public static void PutCustomRelayEventHeader(this NetDataWriter writer, RelayMessageCode eventCode, PlayerId playerId, PlayerId[] peers)
		{
			writer.Put((byte)eventCode);
			writer.Put<PlayerId>(playerId);
			byte b = 5;
			writer.Put(b);
			writer.Put((ushort)peers.Length);
			foreach (PlayerId playerId2 in peers)
			{
				writer.Put<PlayerId>(playerId2);
			}
		}

		public static CustomRelayEventHeader GetCustomRelayEventHeader(this NetDataReader reader, RelayMessageCode eventCode)
		{
			PlayerId sender = reader.Get<PlayerId>();
			RelayMode relayMode = (RelayMode)reader.GetByte();
			if (relayMode == RelayMode.Peers)
			{
				ushort uShort = reader.GetUShort();
				PlayerId[] array = new PlayerId[uShort];
				for (int i = 0; i < uShort; i++)
				{
					array[i] = reader.Get<PlayerId>();
				}
				return new CustomRelayEventHeader(eventCode, sender, array, relayMode);
			}
			return new CustomRelayEventHeader(eventCode, sender, null, relayMode);
		}

		public static void PutServerRpcEventHeader(this NetDataWriter writer, RelayMessageCode eventCode)
		{
			if (((int)eventCode < 150 || (int)eventCode > 241) ? true : false)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			writer.Put((byte)eventCode);
		}
	}
	public static class NetIdSerializationExtensions
	{
		public static bool TryGetNetworkId(this NetDataReader reader, out NetworkId result)
		{
			if (reader.AvailableBytes >= 6)
			{
				result = reader.Get<NetworkId>();
				return true;
			}
			result = default(NetworkId);
			return false;
		}

		public static bool TryGetNetworkedComponentId(this NetDataReader reader, out NetworkedComponentId result)
		{
			if (reader.AvailableBytes >= 1)
			{
				result = reader.Get<NetworkedComponentId>();
				return true;
			}
			result = default(NetworkedComponentId);
			return false;
		}
	}
	public static class SerializationExtensions
	{
		public static void Serialize(this Vector3 vector, NetDataWriter writer)
		{
			writer.Put(vector.X);
			writer.Put(vector.Y);
			writer.Put(vector.Z);
		}

		public static void Deserialize(this ref Vector3 vector, NetDataReader reader)
		{
			vector.X = reader.GetFloat();
			vector.Y = reader.GetFloat();
			vector.Z = reader.GetFloat();
		}
	}
}
namespace ReadyM.Api.Multiplayer.ECS.Values
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct NetworkId : IEquatable<NetworkId>, INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<NetworkId>
		{
			public override NetworkId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, NetworkId value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public PlayerId Creator { get; private set; }

		public uint Id { get; private set; }

		public NetworkId(PlayerId creator, uint id)
		{
			Creator = creator;
			Id = id;
		}

		public bool Equals(NetworkId other)
		{
			if (Creator == other.Creator)
			{
				return Id == other.Id;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj is NetworkId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Creator.GetHashCode() * 397) ^ Id.GetHashCode();
		}

		public static bool operator ==(NetworkId left, NetworkId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(NetworkId left, NetworkId right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"NetId[{Creator}, {Id}]";
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put<PlayerId>(Creator);
			writer.Put(Id);
		}

		public void Deserialize(NetDataReader reader)
		{
			Creator = reader.Get<PlayerId>();
			Id = reader.GetUInt();
		}

		public static NetworkId TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				reader.GetString();
				reader.Read();
			}
			return default(NetworkId);
		}

		public static void TextSerialize(Utf8JsonWriter writer, NetworkId obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteEndObject();
		}
	}
}
namespace ReadyM.Api.Multiplayer.ECS.Registry
{
	public interface INetworkedComponentRegistration : IComponentRegistrationBase<INetworkedComponentRegistry, INetworkedComponent>
	{
	}
	public interface INetworkedComponentRegistry : IComponentRegistryBase<INetworkedComponentRegistry, INetworkedComponent>
	{
		INetworkedComponentRegistry RegisterComponent<T>(DeliveryMethod deliveryMethod = (DeliveryMethod)4, T defaultValue = default(T)) where T : struct, INetworkedComponent;

		NetworkedComponentId GetNetworkedComponentId<T>();

		DeliveryMethod GetNetworkedComponentDeliveryMethod<T>();
	}
	public interface INetworkedComponentRegistryCallback : IComponentRegistryCallbackBase<INetworkedComponentRegistry, INetworkedComponent>
	{
	}
	public struct NetworkedComponentId : IEquatable<NetworkedComponentId>, INetSerializable
	{
		private byte _id;

		public static NetworkedComponentId None => new NetworkedComponentId(0);

		public NetworkedComponentId(byte id)
		{
			_id = id;
		}

		public bool Equals(NetworkedComponentId other)
		{
			return _id == other._id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is NetworkedComponentId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		public static bool operator ==(NetworkedComponentId left, NetworkedComponentId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(NetworkedComponentId left, NetworkedComponentId right)
		{
			return !left.Equals(right);
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_id);
		}

		public void Deserialize(NetDataReader reader)
		{
			_id = reader.GetByte();
		}
	}
	public class NetworkedComponentRegistry : ComponentRegistryBase<INetworkedComponentRegistry, INetworkedComponent>, INetworkedComponentRegistry, IComponentRegistryBase<INetworkedComponentRegistry, INetworkedComponent>
	{
		private byte _nextComponentId;

		private readonly Dictionary<Type, (NetworkedComponentId Id, DeliveryMethod DeliveryMethod)> _ids = new Dictionary<Type, (NetworkedComponentId, DeliveryMethod)>();

		public NetworkedComponentRegistry(IEnumerable<INetworkedComponentRegistration> registrations)
			: base((IEnumerable<IComponentRegistrationBase<INetworkedComponentRegistry, INetworkedComponent>>)registrations)
		{
		}

		public override INetworkedComponentRegistry RegisterComponent<T>(T defaultValue = default(T))
		{
			NetworkedComponentId item = new NetworkedComponentId(_nextComponentId++);
			_ids.Add(typeof(T), (item, (DeliveryMethod)4));
			return base.RegisterComponent(defaultValue);
		}

		public INetworkedComponentRegistry RegisterComponent<T>(DeliveryMethod deliveryMethod = (DeliveryMethod)4, T defaultValue = default(T)) where T : struct, INetworkedComponent
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			NetworkedComponentId item = new NetworkedComponentId(_nextComponentId++);
			_ids.Add(typeof(T), (item, deliveryMethod));
			return base.RegisterComponent(defaultValue);
		}

		public NetworkedComponentId GetNetworkedComponentId<T>()
		{
			return _ids[typeof(T)].Id;
		}

		public DeliveryMethod GetNetworkedComponentDeliveryMethod<T>()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return _ids[typeof(T)].DeliveryMethod;
		}
	}
}
namespace ReadyM.Api.Multiplayer.ECS.Managers
{
	public interface IPlayerIdProvider
	{
		PlayerId? PlayerId { get; }
	}
	public sealed class NetworkedEntityManager : IDisposable
	{
		private readonly Store _world;

		private readonly CommandBuffer _commandBuffer;

		private readonly IPlayerIdProvider _playerIdProvider;

		private readonly ILogger _logger;

		private readonly ComponentIndex<MetadataComponent, NetworkId> _ix;

		private readonly HashSet<NetworkId> _netIdTombstones = new HashSet<NetworkId>();

		private uint _nextNetworkedId;

		private int _skipNetSync;

		public event Action<NetworkId, Entity>? OnEntityDelete;

		public NetworkedEntityManager(Store world, ILogger logger, IPlayerIdProvider playerIdProvider)
		{
			_world = world;
			_commandBuffer = world.GetCommandBuffer();
			_commandBuffer.ReuseBuffer = true;
			_logger = logger;
			_playerIdProvider = playerIdProvider;
			_ix = _world.ComponentIndex<MetadataComponent, NetworkId>();
			_world.OnEntityDelete += OnEntityDeleteHandler;
		}

		public void Dispose()
		{
			_world.OnEntityDelete -= OnEntityDeleteHandler;
		}

		public void SetNextNetworkedId(uint nextId)
		{
			_nextNetworkedId = nextId;
		}

		public bool IsNetworkEntityDeleted(NetworkId netId)
		{
			return _netIdTombstones.Contains(netId);
		}

		private void OnEntityDeleteHandler(EntityDelete evt)
		{
			if (evt.Entity.TryGetComponent<MetadataComponent>(out var result))
			{
				_netIdTombstones.Add(result.NetId);
				if (_skipNetSync == 0)
				{
					this.OnEntityDelete?.Invoke(result.NetId, evt.Entity);
				}
				_logger.LogDebug("Network entity {NetId} deleted", result.NetId);
			}
		}

		public (Entity Entity, NetworkId NetId) CreateNetworkedEntity(ArchetypeId archetypeId, Entity? scopeEntity, Action<EntityBuilder>? setComponents = null, PlayerId? ownerOverride = null)
		{
			PlayerId? playerId = _playerIdProvider.PlayerId;
			if (!playerId.HasValue)
			{
				throw new InvalidOperationException();
			}
			NetworkId networkId = new NetworkId(playerId.Value, ++_nextNetworkedId);
			PlayerId owner = ownerOverride ?? networkId.Creator;
			MetadataComponent meta = new MetadataComponent(networkId, archetypeId, owner);
			Entity item = _world.CreateEntity(archetypeId, delegate(EntityBuilder b)
			{
				b.Add(in meta);
				if (scopeEntity.HasValue)
				{
					b.Add<InScopeComponent>(new InScopeComponent(scopeEntity.Value));
				}
				b.AddTag<LocallyCreatedEntityTag>();
				setComponents?.Invoke(b);
			});
			_logger.LogDebug("Network entity {NetId} created (locally)", meta.NetId);
			return (Entity: item, NetId: networkId);
		}

		public Entity CreateRemoteNetworkedEntity(MetadataComponent meta, Entity? scopeEntity)
		{
			Entity result = _world.CreateEntity(meta.Archetype, delegate(EntityBuilder b)
			{
				b.Add(in meta);
				if (scopeEntity.HasValue)
				{
					b.Add<InScopeComponent>(new InScopeComponent(scopeEntity.Value));
				}
			});
			_logger.LogDebug("Network entity {NetId} created (remote)", meta.NetId);
			return result;
		}

		public bool TryGetEntityByNetworkId(NetworkId netId, [NotNullWhen(true)] out Entity? entity)
		{
			Entities entities = _ix[netId];
			switch (entities.Count)
			{
			case 0:
				entity = null;
				return false;
			case 1:
				entity = entities[0];
				return true;
			default:
				_logger.LogError("Multiple entities found with NetworkId {NetworkId}. This should not happen.", netId);
				entity = null;
				return false;
			}
		}

		public void DeleteEntitiesInScope(Entity scopeEntity, bool skipSync, bool deleteScopeEntity)
		{
			if (!scopeEntity.Tags.Has<ScopeEntityTag>())
			{
				throw new InvalidOperationException("Entity is not a scope entity.");
			}
			if (skipSync)
			{
				_skipNetSync++;
			}
			_world.Query<MetadataComponent>().HasValue<InScopeComponent, Entity>(scopeEntity).ForEachEntity(delegate(ref MetadataComponent meta, Entity entity)
			{
				_commandBuffer.DeleteEntity(entity.Id);
			});
			_commandBuffer.Playback();
			if (deleteScopeEntity)
			{
				scopeEntity.DeleteEntity();
			}
			if (skipSync)
			{
				_skipNetSync--;
			}
		}

		public void DeleteAllNetworkedEntities(bool skipSync)
		{
			if (skipSync)
			{
				_skipNetSync++;
			}
			_world.Query<MetadataComponent>().ForEachEntity(delegate(ref MetadataComponent meta, Entity entity)
			{
				_commandBuffer.DeleteEntity(entity.Id);
			});
			_commandBuffer.Playback();
			if (skipSync)
			{
				_skipNetSync--;
			}
		}
	}
	public class NetworkedOwnershipManager
	{
		private readonly ComponentIndex<MetadataComponent, NetworkId> _ix;

		public NetworkedOwnershipManager(Store store, ILogger logger)
		{
			<logger>P = logger;
			_ix = store.ComponentIndex<MetadataComponent, NetworkId>();
			base..ctor();
		}

		public bool TryGetOwner(NetworkId netId, out PlayerId ownerId)
		{
			Entities entities = _ix[netId];
			switch (entities.Count)
			{
			case 0:
				ownerId = default(PlayerId);
				return false;
			case 1:
				ownerId = entities[0].GetComponent<MetadataComponent>().Owner;
				return true;
			default:
				<logger>P.LogError("Multiple entities found with NetworkId {NetworkId}. This should not happen.", netId);
				ownerId = default(PlayerId);
				return false;
			}
		}

		public bool TryGetOwner(Entity entity, out PlayerId ownerId)
		{
			if (!entity.TryGetComponent<MetadataComponent>(out var result))
			{
				ownerId = default(PlayerId);
				return false;
			}
			ownerId = result.Owner;
			return true;
		}
	}
}
namespace ReadyM.Api.Multiplayer.ECS.Components
{
	public interface INetworkedComponent : IComponent, INetSerializable
	{
		bool IsDirty { get; }

		void ClearDirty();

		void WriteDelta(NetDataWriter writer);

		void ReadDelta(NetDataReader reader);

		void SkipDelta(NetDataReader reader);
	}
	[StructLayout(LayoutKind.Auto)]
	public struct InScopeComponent : ILinkComponent, IIndexedComponent<Entity>, IComponent
	{
		[Ignore]
		public Entity ScopeEntity;

		public InScopeComponent(Entity scopeEntity)
		{
			ScopeEntity = scopeEntity;
		}

		public Entity GetIndexedValue()
		{
			return ScopeEntity;
		}
	}
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct LocallyCreatedEntityTag : ITag
	{
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MetadataComponent : IIndexedComponent<NetworkId>, IComponent, INetSerializable
	{
		public PlayerId Owner;

		public NetworkId NetId { get; private set; }

		public ArchetypeId Archetype { get; private set; }

		public MetadataComponent(NetworkId netId, ArchetypeId archetype, PlayerId owner)
		{
			NetId = netId;
			Archetype = archetype;
			Owner = owner;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put<NetworkId>(NetId);
			writer.Put<ArchetypeId>(Archetype);
			writer.Put<PlayerId>(Owner);
		}

		public void Deserialize(NetDataReader reader)
		{
			NetId = reader.Get<NetworkId>();
			Archetype = reader.Get<ArchetypeId>();
			Owner = reader.Get<PlayerId>();
		}

		public NetworkId GetIndexedValue()
		{
			return NetId;
		}
	}
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct ScopeEntityTag : ITag
	{
	}
}
namespace ReadyM.Api.Multiplayer.Common
{
	public enum OtherPlayerCreatedReason
	{
		OtherConnected,
		NotifyAfterSelfConnected
	}
	public enum OtherPlayerDeletedReason
	{
		OtherDisconnected,
		NotifyBeforeSelfDisconnected
	}
	public enum OtherPlayerInsideAreaReason
	{
		OtherJoined,
		NotifyAfterSelfJoined
	}
	public enum OtherPlayerOutsideAreaReason
	{
		OtherLeft,
		OtherDisconnected,
		NotifyBeforeSelfLeft,
		NotifyBeforeSelfDisconnected
	}
}
namespace ReadyM.Api.Multiplayer.Client
{
	public interface IRelayClient : IPlayerIdProvider, IDisposable
	{
		bool RequestedConnect { get; }

		AreaId? RequestedAreaId { get; }

		PendingActionScheduler<IRelayClientNetworkThreadContext> Scheduler { get; }

		event Action OnStart;

		event Action OnRequestedStop;

		event Action OnRequestedConnect;

		event Action<IRelayClientNetworkThreadContext, PlayerId, uint>? OnConnected;

		event Action OnRequestedDisconnect;

		event Action<IRelayClientNetworkThreadContext, DisconnectReason>? OnDisconnected;

		event Action<IRelayClientNetworkThreadContext, PlayerId> OnOtherPlayerConnected;

		event Action<IRelayClientNetworkThreadContext, PlayerId> OnOtherPlayerDisconnected;

		event Action<AreaId>? OnRequestedJoinArea;

		event Action<IRelayClientNetworkThreadContext, AreaId> OnJoinedArea;

		event Action? OnRequestedLeaveArea;

		event Action<IRelayClientNetworkThreadContext> OnLeftArea;

		event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerJoinedArea;

		event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerLeftArea;

		event Action<IRelayClientNetworkThreadContext, int>? OnPingUpdated;

		event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyBuiltInMessage;

		event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyServerRpcMessage;

		event Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>? OnAnyClientRpcMessage;

		event Action<IRelayClientNetworkThreadContext>? OnClientUpdate;

		void AddBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void AddBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void AddServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void AddServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void RemoveServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler);

		void AddClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler);

		void AddClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler);

		void RemoveClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler);

		void RemoveClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler);

		int GetMaxPacketSize(DeliveryMethod deliveryMethod);

		void Start();

		Task RunAsync(CancellationToken token);

		void Stop();

		void RequestConnect();

		void RequestDisconnect();

		void RequestReconnect();

		void RequestJoinArea(AreaId areaId);

		void RequestLeaveArea();

		void SendRawMessage(NetDataWriter writer, DeliveryMethod deliveryMethod);

		void SendMessage(RelayMessage message);

		void SendMessageToServer<T>(RelayMessageCode eventCode, T data, DeliveryMethod deliveryMethod) where T : INetSerializable;

		void SendMessageToPeers<T>(RelayMessageCode eventCode, T data, PlayerId[] peers, DeliveryMethod deliveryMethod) where T : INetSerializable;

		void SendMessageRelayMode<T>(RelayMessageCode eventCode, T data, RelayMode mode, DeliveryMethod deliveryMethod) where T : INetSerializable;

		void LogEventStats();
	}
	public interface IRelayClientNetworkThreadContext
	{
		bool IsConnected { get; }

		DisconnectReason LastDisconnectReason { get; }

		PlayerId? PlayerId { get; }

		ReadyM.Api.Helpers.ReadOnlyList<PlayerId> AllPlayers { get; }

		AreaId? CurrentAreaId { get; }

		ReadyM.Api.Helpers.ReadOnlyList<PlayerId> AreaPlayers { get; }
	}
	public class NetworkPingMonitor : IDisposable
	{
		private readonly IRelayClient _relayClient;

		public int CurrentPing { get; private set; }

		public event Action<int>? OnPingUpdated;

		public NetworkPingMonitor(IRelayClient relayClient)
		{
			_relayClient = relayClient;
			relayClient.OnPingUpdated += HandlePingUpdated;
		}

		public void Dispose()
		{
			_relayClient.OnPingUpdated -= HandlePingUpdated;
		}

		private void HandlePingUpdated(IRelayClientNetworkThreadContext relayClientNetworkThreadContext, int ping)
		{
			CurrentPing = ping;
			this.OnPingUpdated?.Invoke(ping);
		}
	}
	public enum PlayerIdMode
	{
		Auto,
		MinId,
		ExactId
	}
	public struct RelayConnectionOptions : INetSerializable
	{
		public Guid UserGuid { get; set; }

		public PlayerIdMode PlayerIdMode { get; set; }

		public PlayerId PlayerId { get; set; }

		public RelayConnectionOptions(RelayConnectionOptions options)
		{
			UserGuid = options.UserGuid;
			PlayerIdMode = options.PlayerIdMode;
			PlayerId = options.PlayerId;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(UserGuid.ToString());
			writer.Put((byte)PlayerIdMode);
			writer.Put<PlayerId>(PlayerId);
		}

		public void Deserialize(NetDataReader reader)
		{
			UserGuid = Guid.Parse(reader.GetString());
			PlayerIdMode = (PlayerIdMode)reader.GetByte();
			PlayerId = reader.Get<PlayerId>();
		}
	}
	public struct RelayMessage
	{
		public readonly RelayMessageCode EventCode;

		public readonly NetDataWriter Writer;

		public readonly PlayerId[]? Peers;

		public readonly RelayMode Mode;

		public readonly DeliveryMethod DeliveryMethod;

		private RelayMessage(RelayMessageCode eventCode, NetDataWriter writer, PlayerId[]? peers, RelayMode mode, DeliveryMethod deliveryMethod)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			EventCode = eventCode;
			Writer = writer;
			Peers = peers;
			Mode = mode;
			DeliveryMethod = deliveryMethod;
		}

		public static RelayMessage ToServer(RelayMessageCode eventCode, DeliveryMethod deliveryMethod)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			NetDataWriter val = new NetDataWriter();
			val.Put((byte)eventCode);
			return new RelayMessage(eventCode, val, null, RelayMode.AreaOfInterestOthers, deliveryMethod);
		}

		public static RelayMessage ToPeers(RelayMessageCode eventCode, PlayerId playerId, PlayerId[] peers, DeliveryMethod deliveryMethod)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			NetDataWriter writer = new NetDataWriter();
			writer.PutCustomRelayEventHeader(eventCode, playerId, peers);
			return new RelayMessage(eventCode, writer, peers, RelayMode.Peers, deliveryMethod);
		}

		public static RelayMessage ByRelayMode(RelayMessageCode eventCode, PlayerId playerId, RelayMode mode, DeliveryMethod deliveryMethod)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			NetDataWriter writer = new NetDataWriter();
			writer.PutCustomRelayEventHeader(eventCode, playerId, mode);
			return new RelayMessage(eventCode, writer, null, mode, deliveryMethod);
		}
	}
}
namespace ReadyM.Api.Multiplayer.Client.Blobs
{
	public class BlobInfo(string name, byte[] content)
	{
		public string Name { get; } = name;

		public byte[] Content { get; } = content;
	}
}
