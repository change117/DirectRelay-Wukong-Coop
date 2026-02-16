using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ReadyM.Api.ECS.Jobs;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Helpers;
using ReadyM.Api.Idents;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Managers;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.ECS.Values;
using ReadyM.Api.Multiplayer.Extensions;
using ReadyM.Api.Multiplayer.Generators;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Api.Multiplayer.Protocol;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.Api.Multiplayer.Server;
using ReadyM.Api.Serialization;
using ReadyM.Relay.Common.ECS.Components;
using ReadyM.Relay.Common.ECS.Jobs;
using ReadyM.Relay.Common.ECS.Registry;
using ReadyM.Relay.Common.Serialization;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: InternalsVisibleTo("ReadyM.Relay.Tests")]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("ReadyM.Relay.Common")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+63464565c8693c0537c1bd91e59ef593e8f0bde2")]
[assembly: AssemblyProduct("ReadyM.Relay.Common")]
[assembly: AssemblyTitle("ReadyM.Relay.Common")]
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
namespace ReadyM.Relay.Common.Shim
{
	public struct ShimBuffer
	{
		public byte[]? Data { get; }

		public int Offset { get; }

		public int MaxSize { get; }

		public ShimBuffer(byte[] data, int offset, int? length = null)
		{
			Data = data;
			Offset = offset;
			MaxSize = length ?? (data.Length - offset);
		}

		public ShimBuffer(byte[] data)
			: this(data, 0)
		{
		}
	}
	public class ShimBufferConverter : JsonConverter<ShimBuffer>
	{
		public override ShimBuffer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return default(ShimBuffer);
			}
			DebugJson.Assert(reader.TokenType == JsonTokenType.String);
			string text = reader.GetString();
			if (text == null)
			{
				return default(ShimBuffer);
			}
			return new ShimBuffer(Convert.FromBase64String(text));
		}

		public override void Write(Utf8JsonWriter writer, ShimBuffer value, JsonSerializerOptions options)
		{
			if (value.Data == null)
			{
				writer.WriteNullValue();
				return;
			}
			string value2 = Convert.ToBase64String(value.Data, value.Offset, value.MaxSize);
			writer.WriteStringValue(value2);
		}
	}
	public class ShimDatabaseMetadata
	{
		public PlayerId MaxPlayerId { get; set; }
	}
	public class ShimRecording
	{
		private PlayerId? _playerId;

		private readonly List<ShimResponseItem> _responseItems;

		[JsonPropertyName("playerId")]
		public PlayerId? PlayerId => _playerId;

		[JsonPropertyName("responseItems")]
		public IReadOnlyList<ShimResponseItem> ResponseItems => _responseItems;

		public void SetPlayerId(PlayerId? playerId)
		{
			_playerId = playerId;
		}

		public ShimRecording()
		{
			_responseItems = new List<ShimResponseItem>();
		}

		public ShimRecording(ShimRecording recording)
		{
			_playerId = recording._playerId;
			_responseItems = new List<ShimResponseItem>(recording._responseItems);
		}

		public ShimRecording(ShimRecording recording, PlayerId? overridePlayerId)
		{
			_playerId = overridePlayerId;
			_responseItems = new List<ShimResponseItem>(recording._responseItems);
		}

		public ShimRecording(IEnumerable<ShimResponseItem> items, PlayerId? playerId)
		{
			_playerId = playerId;
			_responseItems = new List<ShimResponseItem>(items);
		}

		public void AddResponseItem(ShimResponseItem responseItem)
		{
			_responseItems.Add(responseItem);
		}
	}
	public class ShimRecordingJsonConverter : JsonConverter<ShimRecording>
	{
		public override ShimRecording Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			PlayerId? playerId = null;
			List<ShimResponseItem> items = new List<ShimResponseItem>();
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "playerId")
				{
					int @int = reader.GetInt32();
					playerId = ((@int >= 0) ? new PlayerId?(new PlayerId((ushort)@int)) : ((PlayerId?)null));
				}
				else if (text == "items")
				{
					items = JsonSerializer.Deserialize<List<ShimResponseItem>>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new ShimRecording(items, playerId);
		}

		public override void Write(Utf8JsonWriter writer, ShimRecording value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("playerId");
			int value2 = ((int?)value.PlayerId?.RawValue) ?? (-1);
			writer.WriteNumberValue(value2);
			writer.WritePropertyName("items");
			JsonSerializer.Serialize(writer, value.ResponseItems, options);
			writer.WriteEndObject();
		}
	}
	public class ShimRecordingMetadata
	{
		public PlayerId PlayerId { get; set; }

		public List<PlayerId> Dependencies { get; set; } = new List<PlayerId>();

		public void AddDependency(PlayerId playerId)
		{
			Dependencies.Add(playerId);
		}
	}
	public struct ShimRequestItem
	{
		public ShimRequestKind Kind;

		public AreaId AreaId;

		public ServerEventHeader ServerHeader;

		public CustomRelayEventHeader ClientHeader;

		public ShimBuffer RawData;

		public object? CustomData;

		public RelayMessageCode? EventCode => Kind switch
		{
			ShimRequestKind.SentBuiltInMessage => ServerHeader.EventCode, 
			ShimRequestKind.SentServerRpcMessage => ServerHeader.EventCode, 
			ShimRequestKind.SentClientRpcMessage => ClientHeader.EventCode, 
			_ => null, 
		};

		public T GetCustomData<T>() where T : struct
		{
			if (CustomData == null)
			{
				return default(T);
			}
			return (T)CustomData;
		}
	}
	public enum ShimRequestKind
	{
		None,
		RequestedConnect,
		RequestedDisconnect,
		RequestedJoinArea,
		RequestedLeaveArea,
		SentBuiltInMessage,
		SentServerRpcMessage,
		SentClientRpcMessage
	}
	public struct ShimResponseItem
	{
		[JsonPropertyName("elapsed")]
		public long Elapsed { get; set; }

		[JsonPropertyName("kind")]
		public ShimResponseKind Kind { get; set; }

		[JsonPropertyName("disconnectReason")]
		public DisconnectReason DisconnectReason { get; set; }

		[JsonPropertyName("playerId")]
		public PlayerId PlayerId { get; set; }

		[JsonPropertyName("nextId")]
		public uint NextId { get; set; }

		[JsonPropertyName("otherPlayers")]
		public List<PlayerId>? OtherPlayers { get; set; }

		[JsonPropertyName("areaId")]
		public AreaId AreaId { get; set; }

		[JsonPropertyName("ping")]
		public int Ping { get; set; }

		[JsonPropertyName("serverHeader")]
		public ServerEventHeader ServerHeader { get; set; }

		[JsonPropertyName("clientHeader")]
		public CustomRelayEventHeader ClientHeader { get; set; }

		[JsonPropertyName("rawData")]
		public ShimBuffer RawData { get; set; }

		[JsonPropertyName("customData")]
		public object? CustomData { get; set; }

		public RelayMessageCode? EventCode => Kind switch
		{
			ShimResponseKind.AnyBuiltInMessage => ServerHeader.EventCode, 
			ShimResponseKind.AnyServerMessage => ServerHeader.EventCode, 
			ShimResponseKind.AnyClientMessage => ClientHeader.EventCode, 
			_ => null, 
		};

		public T GetCustomData<T>() where T : struct
		{
			if (CustomData == null)
			{
				return default(T);
			}
			return (T)CustomData;
		}
	}
	public enum ShimResponseKind
	{
		None = 0,
		Connected = 2,
		Disconnected = 4,
		OtherPlayerConnected = 5,
		OtherPlayerDisconnected = 6,
		JoinedArea = 8,
		LeftArea = 10,
		OtherPlayerJoinedArea = 11,
		OtherPlayerLeftArea = 12,
		PingUpdated = 13,
		AnyBuiltInMessage = 14,
		AnyServerMessage = 15,
		AnyClientMessage = 16
	}
	public class ShimSerializer
	{
		private readonly JsonSerializerOptions _options;

		public ShimSerializer(TextRelaySerializer serializer)
		{
			ShimRecordingJsonConverter item = new ShimRecordingJsonConverter();
			PolymorphicObjectJsonConverter item2 = new PolymorphicObjectJsonConverter(serializer);
			PolymorphicNullableObjectJsonConverter item3 = new PolymorphicNullableObjectJsonConverter(serializer);
			ShimBufferConverter item4 = new ShimBufferConverter();
			_options = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			foreach (JsonConverter converter in serializer.Converters)
			{
				_options.Converters.Add(converter);
			}
			_options.Converters.Add(item);
			_options.Converters.Add(item2);
			_options.Converters.Add(item3);
			_options.Converters.Add(item4);
		}

		public async Task<ShimRecording?> LoadAsync(Stream stream)
		{
			string text = await new StreamReader(stream).ReadToEndAsync();
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return JsonSerializer.Deserialize<ShimRecording>(text, _options);
		}

		public ShimRecording? Load(Stream stream)
		{
			return LoadAsync(stream).GetAwaiter().GetResult();
		}

		public ShimRecording? Load(string path)
		{
			using FileStream stream = File.OpenRead(path);
			return Load(stream);
		}

		public async Task<ShimRecordingMetadata?> LoadMetadataAsync(Stream stream)
		{
			string text = await new StreamReader(stream).ReadToEndAsync();
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return JsonSerializer.Deserialize<ShimRecordingMetadata>(text, _options);
		}

		public ShimRecordingMetadata? LoadMetadata(Stream stream)
		{
			return LoadMetadataAsync(stream).GetAwaiter().GetResult();
		}

		public ShimRecordingMetadata? LoadMetadata(string path)
		{
			using FileStream stream = File.OpenRead(path);
			return LoadMetadata(stream);
		}

		public async Task<ShimDatabaseMetadata?> LoadDatabaseMetadataAsync(string path)
		{
			string[] files = Directory.GetFiles(path);
			if (files.Length == 0)
			{
				return null;
			}
			List<FileStream> streams = new List<FileStream>();
			List<Task<ShimRecordingMetadata?>> metadataTasks = new List<Task<ShimRecordingMetadata>>();
			ushort maxPlayerIndex = 0;
			try
			{
				string[] array = files;
				foreach (string text in array)
				{
					if (text.EndsWith(".shim.meta"))
					{
						FileStream fileStream = File.OpenRead(text);
						streams.Add(fileStream);
						Task<ShimRecordingMetadata> item = LoadMetadataAsync(fileStream);
						metadataTasks.Add(item);
					}
				}
				await Task.WhenAll(metadataTasks);
				foreach (Task<ShimRecordingMetadata> item2 in metadataTasks)
				{
					if (!item2.IsFaulted && !item2.IsCanceled)
					{
						ShimRecordingMetadata shimRecordingMetadata = await item2;
						if (shimRecordingMetadata != null)
						{
							maxPlayerIndex = Math.Max(maxPlayerIndex, shimRecordingMetadata.PlayerId.RawValue);
						}
					}
				}
			}
			finally
			{
				foreach (FileStream item3 in streams)
				{
					item3.Dispose();
				}
			}
			return new ShimDatabaseMetadata
			{
				MaxPlayerId = new PlayerId(maxPlayerIndex)
			};
		}

		public ShimDatabaseMetadata? LoadDatabaseMetadata(string path)
		{
			return LoadDatabaseMetadataAsync(path).GetAwaiter().GetResult();
		}

		public void Save(ShimRecording recording, Stream stream)
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			JsonSerializerOptions options = _options;
			lock (recording)
			{
				streamWriter.Write(JsonSerializer.Serialize(recording, options));
			}
		}

		public void Save(ShimRecording recording, string path)
		{
			using FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
			Save(recording, stream);
		}

		public void SaveMetadata(ShimRecordingMetadata metadata, Stream stream)
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			JsonSerializerOptions options = _options;
			lock (metadata)
			{
				streamWriter.Write(JsonSerializer.Serialize(metadata, options));
			}
		}

		public void SaveMetadata(ShimRecordingMetadata metadata, string path)
		{
			using FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
			SaveMetadata(metadata, stream);
		}
	}
}
namespace ReadyM.Relay.Common.Serialization
{
	public class DefaultRelaySerializerRegistration : IRelaySerializerRegistration
	{
		public void Register(RelaySerializer serializer)
		{
			serializer.HashtableTypeCode = serializer.RegisterType(typeof(Dictionary<object, object>), delegate(NetDataWriter stream, object customObject)
			{
				Dictionary<object, object> dictionary = (Dictionary<object, object>)customObject;
				stream.Put((ushort)dictionary.Count);
				foreach (KeyValuePair<object, object> item in dictionary)
				{
					serializer.SerializeObject(stream, item.Key);
					serializer.SerializeObject(stream, item.Value);
				}
			}, delegate(NetDataReader stream)
			{
				ushort uShort = stream.GetUShort();
				Dictionary<object, object> dictionary = new Dictionary<object, object>();
				for (int i = 0; i < uShort; i++)
				{
					object key = serializer.DeserializeObject(stream);
					object value = serializer.DeserializeObject(stream);
					dictionary.Add(key, value);
				}
				return dictionary;
			});
			serializer.RegisterType(typeof(PlayerId), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put<PlayerId>((PlayerId)customObject);
			}, (NetDataReader stream) => stream.Get<PlayerId>());
			serializer.RegisterType(typeof(byte), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((byte)customObject);
			}, (NetDataReader stream) => stream.GetByte());
			serializer.RegisterType(typeof(short), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((short)customObject);
			}, (NetDataReader stream) => stream.GetShort());
			serializer.RegisterType(typeof(ushort), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((ushort)customObject);
			}, (NetDataReader stream) => stream.GetUShort());
			serializer.RegisterType(typeof(int), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((int)customObject);
			}, (NetDataReader stream) => stream.GetInt());
			serializer.RegisterType(typeof(long), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((long)customObject);
			}, (NetDataReader stream) => stream.GetLong());
			serializer.RegisterType(typeof(float), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((float)customObject);
			}, (NetDataReader stream) => stream.GetFloat());
			serializer.RegisterType(typeof(double), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((double)customObject);
			}, (NetDataReader stream) => stream.GetDouble());
			serializer.RegisterType(typeof(string), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((string)customObject);
			}, (NetDataReader stream) => stream.GetString());
			serializer.RegisterType(typeof(bool), delegate(NetDataWriter stream, object customObject)
			{
				stream.Put((bool)customObject);
			}, (NetDataReader stream) => stream.GetBool());
			serializer.RegisterType(typeof(byte[]), delegate(NetDataWriter stream, object customObject)
			{
				byte[] array = (byte[])customObject;
				stream.PutBytesWithLength(array);
			}, (NetDataReader stream) => stream.GetBytesWithLength());
			serializer.RegisterType(typeof(int[]), delegate(NetDataWriter stream, object customObject)
			{
				int[] array = (int[])customObject;
				stream.PutArray(array);
			}, (NetDataReader stream) => stream.GetIntArray());
			serializer.RegisterType(typeof(Vector3), delegate(NetDataWriter stream, object customObject)
			{
				Vector3 vector = (Vector3)customObject;
				stream.Put(vector.X);
				stream.Put(vector.Y);
				stream.Put(vector.Z);
			}, delegate(NetDataReader stream)
			{
				float x = stream.GetFloat();
				float y = stream.GetFloat();
				float z = stream.GetFloat();
				return new Vector3(x, y, z);
			});
		}
	}
	public class DefaultTextRelaySerializerRegistration : ITextRelaySerializerRegistration
	{
		public void Register(TextRelaySerializer serializer)
		{
			serializer.RegisterPolymorphicType("byte", delegate(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for byte");
				return reader.GetByte();
			});
			serializer.RegisterPolymorphicType("short", delegate(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for short");
				return reader.GetInt16();
			});
			serializer.RegisterPolymorphicType("int", delegate(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for int");
				return reader.GetInt32();
			});
			serializer.RegisterPolymorphicType("long", delegate(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for long");
				return reader.GetInt64();
			});
			serializer.RegisterPolymorphicType("float", delegate(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for float");
				return reader.GetSingle();
			});
			serializer.RegisterPolymorphicType("double", delegate(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
			{
				writer.WriteNumberValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number value for double");
				return reader.GetDouble();
			});
			serializer.RegisterPolymorphicType("string", delegate(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
			{
				writer.WriteStringValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.String, "Expected string value");
				return reader.GetString();
			});
			serializer.RegisterPolymorphicType("bool", delegate(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
			{
				writer.WriteBooleanValue(value);
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False, "Expected boolean value");
				return reader.GetBoolean();
			});
			serializer.RegisterPolymorphicType("byteArray", delegate(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
			{
				writer.WriteStartArray();
				foreach (byte value2 in value)
				{
					writer.WriteNumberValue(value2);
				}
				writer.WriteEndArray();
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				List<byte> list = new List<byte>();
				DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
				while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				{
					DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number in byte array");
					list.Add(reader.GetByte());
				}
				return list.ToArray();
			});
			serializer.RegisterPolymorphicType("intArray", delegate(Utf8JsonWriter writer, int[] value, JsonSerializerOptions options)
			{
				writer.WriteStartArray();
				foreach (int value2 in value)
				{
					writer.WriteNumberValue(value2);
				}
				writer.WriteEndArray();
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				List<int> list = new List<int>();
				DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
				while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				{
					DebugJson.Assert(reader.TokenType == JsonTokenType.Number, "Expected number in int array");
					list.Add(reader.GetInt32());
				}
				return list.ToArray();
			});
			serializer.RegisterPolymorphicType("vector3", delegate(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
			{
				writer.WriteStartArray();
				writer.WriteNumberValue(value.X);
				writer.WriteNumberValue(value.Y);
				writer.WriteNumberValue(value.Z);
				writer.WriteEndArray();
			}, delegate(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
				if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
				{
					throw new JsonException("Expected number for X component of Vector3");
				}
				float single = reader.GetSingle();
				if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
				{
					throw new JsonException("Expected number for Y component of Vector3");
				}
				float single2 = reader.GetSingle();
				if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
				{
					throw new JsonException("Expected number for Z component of Vector3");
				}
				float single3 = reader.GetSingle();
				if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
				{
					throw new JsonException("Expected end of array for Vector3");
				}
				return new Vector3(single, single2, single3);
			});
			serializer.RegisterPolymorphicType<PlayerId>("playerId");
			foreach (Type item in ReflectionHelpers.GetTypesWithAttribute<RegisterJsonConverterAttribute>())
			{
				if (!typeof(JsonConverter).IsAssignableFrom(item))
				{
					throw new InvalidOperationException("Type " + item.FullName + " is marked with [RegisterJsonConverter] but does not derive from JsonConverter");
				}
				object obj = Activator.CreateInstance(item);
				if (obj == null)
				{
					throw new InvalidOperationException("Failed to instantiate a JSON converter " + item.FullName);
				}
				serializer.RegisterConverter((JsonConverter)obj);
			}
		}
	}
	public delegate object DeserializeMethod(NetDataReader reader);
	public class FuncJsonConverter<T>(TextSerializeMethod<T> serializeFunc, TextDeserializeMethod<T> deserializeFunc) : JsonConverter<T>()
	{
		public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return <deserializeFunc>P(ref reader, options);
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			<serializeFunc>P(writer, value, options);
		}
	}
	public interface IRelaySerializerRegistration
	{
		void Register(RelaySerializer serializer);
	}
	public interface ITextRelaySerializerRegistration
	{
		void Register(TextRelaySerializer serializer);
	}
	public class PolymorphicNullableObjectJsonConverter(TextRelaySerializer serializer) : JsonConverter<object?>()
	{
		private TextRelaySerializer _serializer { get; } = serializer;

		public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
			reader.Read();
			string text = reader.GetString();
			object result = null;
			if (text != null)
			{
				if (!_serializer.PolymorphicByDiscriminator.TryGetValue(text, out Type value))
				{
					throw new JsonException("Unknown discriminator: " + text);
				}
				reader.Read();
				result = JsonSerializer.Deserialize(ref reader, value, options);
			}
			else
			{
				reader.Read();
				reader.Skip();
			}
			reader.Read();
			DebugJson.Assert(reader.TokenType == JsonTokenType.EndArray, "Expected end of array after object value");
			return result;
		}

		public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			if (value == null)
			{
				writer.WriteNullValue();
				writer.WriteNullValue();
			}
			else
			{
				Type type = value.GetType();
				if (!_serializer.PolymorphicByType.TryGetValue(type, out string value2))
				{
					throw new JsonException($"Unknown type: {type}");
				}
				writer.WriteStringValue(value2);
				JsonSerializer.Serialize(writer, value, type, options);
			}
			writer.WriteEndArray();
		}
	}
	public class PolymorphicObjectJsonConverter(TextRelaySerializer serializer) : JsonConverter<object>()
	{
		private TextRelaySerializer _serializer { get; } = serializer;

		public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
			reader.Read();
			string text = reader.GetString();
			if (!_serializer.PolymorphicByDiscriminator.TryGetValue(text, out Type value))
			{
				throw new JsonException("Unknown discriminator: " + text);
			}
			reader.Read();
			object? result = JsonSerializer.Deserialize(ref reader, value, options);
			reader.Read();
			DebugJson.Assert(reader.TokenType == JsonTokenType.EndArray, "Expected end of array after object value");
			return result;
		}

		public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			Type type = value.GetType();
			if (!_serializer.PolymorphicByType.TryGetValue(type, out string value2))
			{
				throw new JsonException($"Unknown type: {type}");
			}
			writer.WriteStringValue(value2);
			JsonSerializer.Serialize(writer, value, type, options);
			writer.WriteEndArray();
		}
	}
	public static class ReflectionHelpers
	{
		public static IEnumerable<Type> GetTypesWithAttribute<T>()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			string attributeAsmFullName = typeof(T).Assembly.FullName;
			return from t in assemblies.Where((Assembly asm) => asm.FullName == attributeAsmFullName || asm.GetReferencedAssemblies().Any((AssemblyName x) => x.FullName == attributeAsmFullName)).SelectMany(delegate(Assembly asm)
				{
					try
					{
						return asm.ExportedTypes;
					}
					catch (ReflectionTypeLoadException ex)
					{
						return ex.Types.Where((Type t) => t != null);
					}
				})
				where t.GetCustomAttribute<RegisterJsonConverterAttribute>(inherit: false) != null
				select t;
		}
	}
	public class RelaySerializer
	{
		private byte _nextTypeCode = byte.MaxValue;

		public byte HashtableTypeCode;

		private readonly Dictionary<Type, (byte Code, SerializeMethod Serialize, DeserializeMethod Deserialize)> _registeredTypes = new Dictionary<Type, (byte, SerializeMethod, DeserializeMethod)>();

		private readonly Dictionary<byte, (Type Type, SerializeMethod Serialize, DeserializeMethod Deserialize)> _code2Type = new Dictionary<byte, (Type, SerializeMethod, DeserializeMethod)>();

		public RelaySerializer(IEnumerable<IRelaySerializerRegistration> registrations)
		{
			foreach (IRelaySerializerRegistration registration in registrations)
			{
				registration.Register(this);
			}
		}

		public byte RegisterType(Type customType, SerializeMethod serializeMethod, DeserializeMethod deserializeMethod)
		{
			if (_registeredTypes.ContainsKey(customType))
			{
				throw new ArgumentException($"Type {customType} is already registered");
			}
			byte b = _nextTypeCode--;
			_registeredTypes[customType] = (b, serializeMethod, deserializeMethod);
			_code2Type[b] = (customType, serializeMethod, deserializeMethod);
			return b;
		}

		[Obsolete]
		public static Dictionary<object, object?> UpdateAndGetDiff(Dictionary<object, object> state, IEnumerable<(object, object?)> changes)
		{
			Dictionary<object, object> dictionary = new Dictionary<object, object>();
			foreach (var (key, obj) in changes)
			{
				if (state.TryGetValue(key, out object value))
				{
					if (obj == null)
					{
						state.Remove(key);
						dictionary[key] = null;
					}
					else if (value != obj)
					{
						dictionary[key] = obj;
						state[key] = obj;
					}
				}
				else if (obj != null)
				{
					state[key] = obj;
					dictionary[key] = obj;
				}
			}
			return dictionary;
		}

		[Obsolete]
		public static Dictionary<object, object?> UpdateAndGetDiff(Dictionary<object, object> state, Dictionary<object, object?> changes)
		{
			return UpdateAndGetDiff(state, changes.Select<KeyValuePair<object, object>, (object, object)>((KeyValuePair<object, object> kv) => (Key: kv.Key, Value: kv.Value)));
		}

		public void SerializeObject(NetDataWriter writer, object? data)
		{
			if (data == null)
			{
				writer.Put((ushort)0);
				return;
			}
			Type type = data.GetType();
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			if (!_registeredTypes.TryGetValue(type, out (byte, SerializeMethod, DeserializeMethod) value))
			{
				throw new ArgumentException($"Type {data.GetType()} is not registered");
			}
			int length = writer.Length;
			writer.Put((ushort)0);
			writer.Put(value.Item1);
			value.Item2(writer, data);
			int length2 = writer.Length;
			ushort num = (ushort)(length2 - length - 2);
			writer.SetPosition(length);
			writer.Put(num);
			writer.SetPosition(length2);
		}

		public object? DeserializeObject(NetDataReader stream)
		{
			if (stream.GetUShort() == 0)
			{
				return null;
			}
			byte b = stream.GetByte();
			if (!_code2Type.TryGetValue(b, out (Type, SerializeMethod, DeserializeMethod) value))
			{
				throw new ArgumentException($"Type code {b} is not registered");
			}
			return value.Item3(stream);
		}

		public T DeserializeObject<T>(NetDataReader stream)
		{
			try
			{
				return (T)DeserializeObject(stream);
			}
			catch
			{
				ushort num = BitConverter.ToUInt16(stream.RawData, stream.UserDataOffset);
				byte b = stream.RawData[stream.UserDataOffset + 2];
				throw new SerializationException($"Failed to deserialize object of type {typeof(T)}, received {num} bytes of type {b}");
			}
		}
	}
	public delegate void SerializeMethod(NetDataWriter writer, object customObject);
	public delegate object? TextDeserializeMethod(ref Utf8JsonReader reader, JsonSerializerOptions options);
	public delegate T? TextDeserializeMethod<out T>(ref Utf8JsonReader reader, JsonSerializerOptions options);
	public class TextRelaySerializer
	{
		private readonly Dictionary<Type, string> _polymorphicByType = new Dictionary<Type, string>();

		private readonly Dictionary<string, Type> _polymorphicByDiscriminator = new Dictionary<string, Type>();

		private readonly List<JsonConverter> _converters = new List<JsonConverter>();

		public ReadOnlyDictionary<Type, string> PolymorphicByType => new ReadOnlyDictionary<Type, string>(_polymorphicByType);

		public ReadOnlyDictionary<string, Type> PolymorphicByDiscriminator => new ReadOnlyDictionary<string, Type>(_polymorphicByDiscriminator);

		public ReadyM.Api.Helpers.ReadOnlyList<JsonConverter> Converters => new ReadyM.Api.Helpers.ReadOnlyList<JsonConverter>(_converters);

		public TextRelaySerializer(IEnumerable<ITextRelaySerializerRegistration> registrations)
		{
			foreach (ITextRelaySerializerRegistration registration in registrations)
			{
				registration.Register(this);
			}
		}

		public void RegisterConverter(JsonConverter converter)
		{
			_converters.Add(converter);
		}

		public void RegisterConverter<T>(TextSerializeMethod<T> serializeMethod, TextDeserializeMethod<T> deserializeMethod)
		{
			_converters.Add(new FuncJsonConverter<T>(serializeMethod, deserializeMethod));
		}

		public void RegisterPolymorphicType(Type type, string discriminator)
		{
			if (_polymorphicByType.TryGetValue(type, out string value))
			{
				throw new InvalidOperationException($"Type {type} is already registered with discriminator {value}");
			}
			_polymorphicByType[type] = discriminator;
			_polymorphicByDiscriminator[discriminator] = type;
		}

		public void RegisterPolymorphicType<T>(string discriminator)
		{
			RegisterPolymorphicType(typeof(T), discriminator);
		}

		public void RegisterPolymorphicType<T>(string discriminator, TextSerializeMethod<T> serializeMethod, TextDeserializeMethod<T> deserializeMethod)
		{
			RegisterPolymorphicType<T>(discriminator);
			RegisterConverter(serializeMethod, deserializeMethod);
		}
	}
	public delegate void TextSerializeMethod(Utf8JsonWriter writer, object customObject, JsonSerializerOptions options);
	public delegate void TextSerializeMethod<in T>(Utf8JsonWriter writer, T customObject, JsonSerializerOptions options);
}
namespace ReadyM.Relay.Common.Rpc
{
	public interface IServerRpcRegistration
	{
		void InitRpc(RpcRelayServer rpc);

		void DeInitRpc();
	}
	public class RpcRelayServer : IDisposable
	{
		private readonly List<IServerRpcRegistration> _registrations;

		public IRelayServer RelayServer { get; }

		public RelaySerializer Serializer { get; }

		public RpcRelayServer(IRelayServer relayServer, RelaySerializer serializer, IEnumerable<IServerRpcRegistration> registrations)
		{
			RelayServer = relayServer;
			Serializer = serializer;
			_registrations = new List<IServerRpcRegistration>(registrations);
			for (int i = 0; i < _registrations.Count; i++)
			{
				_registrations[i].InitRpc(this);
			}
		}

		public void Dispose()
		{
			for (int num = _registrations.Count - 1; num >= 0; num--)
			{
				_registrations[num].DeInitRpc();
			}
		}
	}
	public abstract class ServerRpcRegistrationBase : IServerRpcRegistration
	{
		private RpcRelayServer? _rpc;

		protected IRelayServer RelayServer => _rpc.RelayServer;

		protected RelaySerializer Serializer => _rpc.Serializer;

		public virtual void InitRpc(RpcRelayServer rpc)
		{
			_rpc = rpc;
		}

		public virtual void DeInitRpc()
		{
			_rpc = null;
		}
	}
}
namespace ReadyM.Relay.Common.Extensions
{
	public static class NetPeerExtensions
	{
		public static void SendImmediately(this NetPeer peer, byte[] data, int start, int length, DeliveryMethod options)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			peer.Send(data, start, length, options);
			peer.NetManager.TriggerUpdate();
		}

		public static void SendImmediately(this NetPeer peer, NetDataWriter dataWriter, DeliveryMethod deliveryMethod)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			peer.Send(dataWriter, deliveryMethod);
			peer.NetManager.TriggerUpdate();
		}
	}
}
namespace ReadyM.Relay.Common.ECS.Tags
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct DisallowOwnershipTransferTag : ITag
	{
	}
}
namespace ReadyM.Relay.Common.ECS.Systems
{
	public class QueryCacheHelper<TContext, TKey, TQuery>(Func<TContext, TKey> keyFunc, Func<TContext, TQuery> queryFactory) where TQuery : ArchetypeQuery
	{
		private TQuery? _nullQuery;

		private readonly Dictionary<TKey, TQuery> _queryCache = new Dictionary<TKey, TQuery>();

		public TQuery GetQuery(TContext context)
		{
			TKey val = <keyFunc>P(context);
			TQuery value;
			if (val == null)
			{
				if (_nullQuery == null)
				{
					_nullQuery = <queryFactory>P(context);
				}
				value = _nullQuery;
			}
			else if (!_queryCache.TryGetValue(val, out value))
			{
				value = <queryFactory>P(context);
				_queryCache.Add(val, value);
			}
			return value;
		}
	}
	public abstract class SendComponentDeltaSystemBase<T> : QuerySystem<MetadataComponent, T> where T : struct, INetworkedComponent
	{
		private readonly NetworkedComponentId _componentId;

		private readonly QueryCacheHelper<SendContext, Entity?, ArchetypeQuery<MetadataComponent, T>> _queryCache;

		[ThreadStatic]
		private static NetDataWriter? _writer;

		protected SendComponentDeltaSystemBase(NetworkedComponentId componentId)
		{
			_componentId = componentId;
			_queryCache = new QueryCacheHelper<SendContext, Entity?, ArchetypeQuery<MetadataComponent, T>>((SendContext context) => context.ScopeEntity, delegate(SendContext context)
			{
				QueryFilter queryFilter = new QueryFilter();
				queryFilter = SetupFilter(queryFilter, context);
				return base.Query.Store.Query<MetadataComponent, T>(queryFilter);
			});
		}

		protected abstract int? GetMaxPacketSize();

		protected abstract QueryFilter SetupFilter(QueryFilter filter, SendContext context);

		protected abstract uint SentOwners();

		protected abstract void Send(PlayerId owner, NetDataWriter data, SendContext context);

		private void CreatePacketHeader(NetDataWriter writer)
		{
			writer.Put((byte)250);
			writer.Put<NetworkedComponentId>(_componentId);
		}

		protected override void OnUpdate()
		{
			OnUpdate(default(SendContext));
		}

		protected void OnUpdate(SendContext context)
		{
			int? maxPacketSize = GetMaxPacketSize();
			if (maxPacketSize.HasValue)
			{
				OnUpdateChunked(context, maxPacketSize.Value);
			}
			else
			{
				OnUpdateReliable(context);
			}
		}

		private void OnUpdateReliable(SendContext context)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			uint num = SentOwners();
			ArchetypeQuery<MetadataComponent, T> archetypeQuery = _queryCache.GetQuery(context);
			for (int i = 0; i < 32; i++)
			{
				if ((num & (1 << i)) == 0L)
				{
					continue;
				}
				PlayerId playerId = new PlayerId((ushort)i);
				if (_writer == null)
				{
					_writer = new NetDataWriter();
				}
				_writer.Reset();
				CreatePacketHeader(_writer);
				int length = _writer.Length;
				foreach (Chunks<MetadataComponent, T> chunk5 in archetypeQuery.Chunks)
				{
					chunk5.Deconstruct(out var chunk, out var chunk2, out var _);
					Chunk<MetadataComponent> chunk3 = chunk;
					Chunk<T> chunk4 = chunk2;
					Span<MetadataComponent> span = chunk3.Span;
					Span<T> span2 = chunk4.Span;
					for (int j = 0; j < chunk3.Length; j++)
					{
						MetadataComponent metadataComponent = span[j];
						ref T reference = ref span2[j];
						if (!(metadataComponent.Owner != playerId) && reference.IsDirty)
						{
							_writer.Put<NetworkId>(metadataComponent.NetId);
							reference.WriteDelta(_writer);
							reference.ClearDirty();
						}
					}
				}
				if (_writer.Length > length)
				{
					Send(playerId, _writer, context);
				}
			}
		}

		private void OnUpdateChunked(SendContext context, int maxPacketSize)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			uint num = SentOwners();
			ArchetypeQuery<MetadataComponent, T> archetypeQuery = _queryCache.GetQuery(context);
			for (int i = 0; i < 32; i++)
			{
				if ((num & (1 << i)) == 0L)
				{
					continue;
				}
				PlayerId playerId = new PlayerId((ushort)i);
				if (_writer == null)
				{
					_writer = new NetDataWriter();
				}
				_writer.Reset();
				CreatePacketHeader(_writer);
				int length = _writer.Length;
				foreach (Chunks<MetadataComponent, T> chunk5 in archetypeQuery.Chunks)
				{
					chunk5.Deconstruct(out var chunk, out var chunk2, out var _);
					Chunk<MetadataComponent> chunk3 = chunk;
					Chunk<T> chunk4 = chunk2;
					Span<MetadataComponent> span = chunk3.Span;
					Span<T> span2 = chunk4.Span;
					for (int j = 0; j < chunk3.Length; j++)
					{
						MetadataComponent metadataComponent = span[j];
						ref T reference = ref span2[j];
						if (metadataComponent.Owner != playerId)
						{
							continue;
						}
						bool flag = false;
						while (reference.IsDirty)
						{
							int length2 = _writer.Length;
							_writer.Put<NetworkId>(metadataComponent.NetId);
							reference.WriteDelta(_writer);
							if (_writer.Length > maxPacketSize)
							{
								if (flag)
								{
									throw new Exception("Packet too large, unable to send");
								}
								_writer.SetPosition(length2);
								Send(playerId, _writer, context);
								_writer.Reset();
								CreatePacketHeader(_writer);
								flag = true;
								continue;
							}
							reference.ClearDirty();
							break;
						}
					}
				}
				if (_writer.Length > length)
				{
					Send(playerId, _writer, context);
				}
			}
		}
	}
	public readonly struct SendContext
	{
		public readonly AreaId? AreaId;

		public readonly PlayerId? PlayerId;

		public readonly Entity? ScopeEntity;

		public bool IsArea => AreaId.HasValue;

		public bool IsPlayer => PlayerId.HasValue;

		public bool IsGlobal
		{
			get
			{
				if (!AreaId.HasValue)
				{
					return !PlayerId.HasValue;
				}
				return false;
			}
		}

		public static SendContext Global => new SendContext(null, null, null);

		public SendContext(AreaId? areaId, PlayerId? playerId, Entity? scopeEntity)
		{
			AreaId = areaId;
			PlayerId = playerId;
			ScopeEntity = scopeEntity;
		}

		public static SendContext FromArea(AreaId areaId, Entity scopeEntity)
		{
			return new SendContext(areaId, null, scopeEntity);
		}

		public static SendContext FromPlayer(PlayerId playerId, Entity scopeEntity)
		{
			return new SendContext(null, playerId, scopeEntity);
		}
	}
	public abstract class SendEntityCreatedSystemBase : QuerySystem<MetadataComponent>
	{
		private readonly JobRegistry _jobRegistry;

		private readonly QueryCacheHelper<SendContext, Entity?, ArchetypeQuery<MetadataComponent>> _queryCache;

		[ThreadStatic]
		private static NetDataWriter? _writer;

		protected SendEntityCreatedSystemBase(JobRegistry jobRegistry)
		{
			_jobRegistry = jobRegistry;
			_queryCache = new QueryCacheHelper<SendContext, Entity?, ArchetypeQuery<MetadataComponent>>((SendContext context) => context.ScopeEntity, delegate(SendContext context)
			{
				QueryFilter queryFilter = new QueryFilter();
				queryFilter = SetupFilter(queryFilter, context);
				return base.Query.Store.Query<MetadataComponent>(queryFilter);
			});
			SetupBaseFilter(base.Filter);
		}

		protected abstract QueryFilter SetupFilter(QueryFilter filter, SendContext context);

		protected QueryFilter SetupBaseFilter(QueryFilter filter)
		{
			return filter.AllTags(Friflo.Engine.ECS.Tags.Get<LocallyCreatedEntityTag>());
		}

		protected abstract void CreatePacketHeader(NetDataWriter writer, SendContext context);

		protected abstract void Send(NetDataWriter writer, SendContext context);

		protected override void OnUpdate()
		{
			OnUpdate(default(SendContext));
		}

		protected void OnUpdate(SendContext context)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			if (_writer == null)
			{
				_writer = new NetDataWriter();
			}
			_writer.Reset();
			CreatePacketHeader(_writer, context);
			ArchetypeQuery<MetadataComponent> archetypeQuery = _queryCache.GetQuery(context);
			int count = archetypeQuery.Count;
			if (count != 0)
			{
				_writer.Put(count);
				archetypeQuery.ForEachEntity(delegate(ref MetadataComponent meta, Entity entity)
				{
					_writer.Put<MetadataComponent>(meta);
					base.CommandBuffer.RemoveTag<LocallyCreatedEntityTag>(entity.Id);
				});
				_jobRegistry.WriteSnapshot(archetypeQuery.Store, archetypeQuery.Filter, null, _writer);
				if (count > 0)
				{
					Send(_writer, context);
				}
			}
		}
	}
}
namespace ReadyM.Relay.Common.ECS.Registry
{
	public class AreaComponentRegistry : ComponentRegistryBase<IAreaComponentRegistry, IComponent>, IAreaComponentRegistry, IComponentRegistryBase<IAreaComponentRegistry, IComponent>
	{
		public AreaComponentRegistry(IEnumerable<IAreaComponentRegistration> registrations)
			: base((IEnumerable<IComponentRegistrationBase<IAreaComponentRegistry, IComponent>>)registrations)
		{
		}
	}
	public class DefaultNetworkedComponentRegistration : INetworkedComponentRegistration, IComponentRegistrationBase<INetworkedComponentRegistry, INetworkedComponent>
	{
		public void Register(INetworkedComponentRegistry registry)
		{
			registry.RegisterComponent<PlayerScopeComponent>((DeliveryMethod)2);
			registry.RegisterComponent<AreaScopeComponent>((DeliveryMethod)2);
		}
	}
	public interface IAreaComponentRegistryCallback : IComponentRegistryCallbackBase<IAreaComponentRegistry, IComponent>
	{
	}
	public interface IPlayerComponentRegistryCallback : IComponentRegistryCallbackBase<IPlayerComponentRegistry, IComponent>
	{
	}
	public class PlayerComponentRegistry : ComponentRegistryBase<IPlayerComponentRegistry, IComponent>, IPlayerComponentRegistry, IComponentRegistryBase<IPlayerComponentRegistry, IComponent>
	{
		public PlayerComponentRegistry(IEnumerable<IPlayerComponentRegistration> registrations)
			: base((IEnumerable<IComponentRegistrationBase<IPlayerComponentRegistry, IComponent>>)registrations)
		{
		}
	}
}
namespace ReadyM.Relay.Common.ECS.Jobs
{
	public class ApplyDeltaJob<T>(NetworkedEntityManager netEntity, IPlayerIdProvider playerIdProvider) : IJob<NetDataReader> where T : struct, INetworkedComponent
	{
		public void Execute(NetDataReader reader)
		{
			PlayerId? playerId = <playerIdProvider>P.PlayerId;
			if (!playerId.HasValue)
			{
				return;
			}
			NetworkId result;
			while (reader.TryGetNetworkId(out result))
			{
				if (!<netEntity>P.TryGetEntityByNetworkId(result, out var entity))
				{
					default(T).SkipDelta(reader);
					continue;
				}
				Entity value = entity.Value;
				PlayerId owner = value.GetComponent<MetadataComponent>().Owner;
				if (playerId != PlayerId.Server && playerId == owner)
				{
					default(T).SkipDelta(reader);
					continue;
				}
				value = entity.Value;
				ref T component = ref value.GetComponent<T>();
				component.ReadDelta(reader);
				if (playerId == owner)
				{
					component.ClearDirty();
				}
			}
		}
	}
	public class ApplySnapshotJob<T>(NetworkedEntityManager netEntity) : IJob<NetDataReader> where T : struct, INetworkedComponent
	{
		public void Execute(NetDataReader reader)
		{
			uint uInt = reader.GetUInt();
			for (uint num = 0u; num < uInt; num++)
			{
				NetworkId netId = reader.Get<NetworkId>();
				if (!<netEntity>P.TryGetEntityByNetworkId(netId, out var entity))
				{
					((INetSerializable)default(T)/*cast due to .constrained prefix*/).Deserialize(reader);
				}
				else
				{
					entity.Value.AddComponent<T>(reader.Get<T>());
				}
			}
		}
	}
	public class JobRegistry
	{
		private class RegisterJobsCallback(JobRegistry owner) : INetworkedComponentRegistryCallback, IComponentRegistryCallbackBase<INetworkedComponentRegistry, INetworkedComponent>
		{
			public void AcceptComponent<T>(INetworkedComponentRegistry registry, T defaultValue = default(T)) where T : struct, INetworkedComponent
			{
				NetworkedComponentId networkedComponentId = registry.GetNetworkedComponentId<T>();
				owner.Logger.LogDebug("Registering jobs for: {ComponentType} with ID {Id}", typeof(T).Name, networkedComponentId);
				owner.RegisterApplyDeltaJob(networkedComponentId, new ApplyDeltaJob<T>(owner.NetEntity, owner.PlayerIdProvider));
				owner.RegisterApplySnapshotJob(networkedComponentId, new ApplySnapshotJob<T>(owner.NetEntity));
				owner.RegisterWriteSnapshotJob(networkedComponentId, new WriteSnapshotJob<T>(networkedComponentId), new WriteSnapshotJob<T>(networkedComponentId));
			}
		}

		protected readonly NetworkedEntityManager NetEntity;

		protected readonly IPlayerIdProvider PlayerIdProvider;

		protected readonly ILogger Logger;

		protected readonly Dictionary<NetworkedComponentId, IJob<NetDataReader>> ApplyDeltaJobs = new Dictionary<NetworkedComponentId, IJob<NetDataReader>>();

		protected readonly Dictionary<NetworkedComponentId, IJob<NetDataReader>> ApplySnapshotJobs = new Dictionary<NetworkedComponentId, IJob<NetDataReader>>();

		protected readonly Dictionary<NetworkedComponentId, IJob<EntityStore, QueryFilter, Entity?, NetDataWriter>> WriteSnapshotJobs = new Dictionary<NetworkedComponentId, IJob<EntityStore, QueryFilter, Entity?, NetDataWriter>>();

		protected readonly Dictionary<NetworkedComponentId, IJob<Entity, NetDataWriter>> WriteOneSnapshotJobs = new Dictionary<NetworkedComponentId, IJob<Entity, NetDataWriter>>();

		public event Action? OnApplySnapshot;

		public JobRegistry(INetworkedComponentRegistry registry, NetworkedEntityManager netEntity, IPlayerIdProvider playerIdProvider, ILogger logger)
		{
			NetEntity = netEntity;
			PlayerIdProvider = playerIdProvider;
			Logger = logger;
			registry.Accept(new RegisterJobsCallback(this));
		}

		protected void RegisterApplyDeltaJob(NetworkedComponentId componentId, IJob<NetDataReader> job)
		{
			ApplyDeltaJobs.Add(componentId, job);
		}

		protected void RegisterApplySnapshotJob(NetworkedComponentId componentId, IJob<NetDataReader> job)
		{
			ApplySnapshotJobs.Add(componentId, job);
		}

		protected void RegisterWriteSnapshotJob(NetworkedComponentId componentId, IJob<EntityStore, QueryFilter, Entity?, NetDataWriter> job, IJob<Entity, NetDataWriter> oneJob)
		{
			WriteSnapshotJobs.Add(componentId, job);
			WriteOneSnapshotJobs.Add(componentId, oneJob);
		}

		public void WriteSnapshot(EntityStore world, QueryFilter filter, Entity? scopeEntity, NetDataWriter writer)
		{
			foreach (IJob<EntityStore, QueryFilter, Entity?, NetDataWriter> value in WriteSnapshotJobs.Values)
			{
				value.Execute(world, filter, scopeEntity, writer);
			}
		}

		public void WriteSnapshot(Entity entity, NetDataWriter writer)
		{
			foreach (IJob<Entity, NetDataWriter> value in WriteOneSnapshotJobs.Values)
			{
				value.Execute(entity, writer);
			}
		}

		public void ApplyDelta(NetDataReader reader)
		{
			NetworkedComponentId networkedComponentId = reader.Get<NetworkedComponentId>();
			if (!ApplyDeltaJobs.TryGetValue(networkedComponentId, out IJob<NetDataReader> value))
			{
				Logger.LogError("No reader job registered for component ID: {Id}", networkedComponentId);
			}
			else
			{
				value.Execute(reader);
			}
		}

		public void ApplySnapshot(NetDataReader reader)
		{
			NetworkedComponentId result;
			while (reader.TryGetNetworkedComponentId(out result))
			{
				if (!ApplySnapshotJobs.TryGetValue(result, out IJob<NetDataReader> value))
				{
					Logger.LogError("No snapshot reader job registered for component ID: {Id}", result);
					break;
				}
				value.Execute(reader);
			}
			this.OnApplySnapshot?.Invoke();
		}
	}
	public class WriteSnapshotJob<T>(NetworkedComponentId componentId) : IJob<EntityStore, QueryFilter, Entity?, NetDataWriter>, IJob<Entity, NetDataWriter> where T : struct, INetworkedComponent
	{
		[ThreadStatic]
		private static NetDataWriter? _writer;

		[ThreadStatic]
		private static uint _counter;

		public void Execute(EntityStore world, QueryFilter filter, Entity? scopeEntity, NetDataWriter writer)
		{
			int length = writer.Length;
			writer.Put<NetworkedComponentId>(<componentId>P);
			int length2 = writer.Length;
			writer.Put(0u);
			uint num = 0u;
			if (scopeEntity.HasValue && scopeEntity.Value.TryGetComponent<T>(out var result))
			{
				num++;
				writer.Put<NetworkId>(scopeEntity.Value.GetComponent<MetadataComponent>().NetId);
				writer.Put<T>(result);
			}
			ArchetypeQuery<MetadataComponent, T> archetypeQuery = world.Query<MetadataComponent, T>(filter);
			_counter = num;
			_writer = writer;
			archetypeQuery.ForEachEntity(delegate(ref MetadataComponent meta, ref T comp, Entity _)
			{
				_counter++;
				_writer.Put<NetworkId>(meta.NetId);
				_writer.Put<T>(comp);
			});
			num = _counter;
			writer = _writer;
			if (num == 0)
			{
				writer.SetPosition(length);
				return;
			}
			int length3 = writer.Length;
			writer.SetPosition(length2);
			writer.Put(num);
			writer.SetPosition(length3);
		}

		public void Execute(Entity entity, NetDataWriter writer)
		{
			if (entity.TryGetComponent<T>(out var result))
			{
				writer.Put<NetworkedComponentId>(<componentId>P);
				writer.Put(1u);
				MetadataComponent component = entity.GetComponent<MetadataComponent>();
				writer.Put<NetworkId>(component.NetId);
				writer.Put<T>(result);
			}
		}
	}
}
namespace ReadyM.Relay.Common.ECS.Components
{
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct AreaScopeComponent : IIndexedComponent<AreaId>, IComponent, INetworkedComponent, INetSerializable
	{
		private AreaId _areaId;

		private PlayerId _masterClient;

		private byte _dirtyMask;

		public AreaId AreaId
		{
			get
			{
				return _areaId;
			}
			set
			{
				if (!_areaId.Equals(value))
				{
					_areaId = value;
					_dirtyMask |= 1;
				}
			}
		}

		public PlayerId MasterClient
		{
			get
			{
				return _masterClient;
			}
			set
			{
				if (!_masterClient.Equals(value))
				{
					_masterClient = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public AreaId GetIndexedValue()
		{
			return AreaId;
		}

		public void Serialize(NetDataWriter writer)
		{
			_areaId.Serialize(writer);
			_masterClient.Serialize(writer);
		}

		public void Deserialize(NetDataReader reader)
		{
			_areaId.Deserialize(reader);
			_dirtyMask |= 1;
			_masterClient.Deserialize(reader);
			_dirtyMask |= 2;
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				_areaId.Serialize(writer);
			}
			if ((dirtyMask & 2) != 0)
			{
				_masterClient.Serialize(writer);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				_areaId.Deserialize(reader);
				_dirtyMask |= 1;
			}
			if ((num & 2) != 0)
			{
				_masterClient.Deserialize(reader);
				_dirtyMask |= 2;
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				default(AreaId).Deserialize(reader);
			}
			if ((num & 2) != 0)
			{
				default(PlayerId).Deserialize(reader);
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct PlayerScopeComponent : IIndexedComponent<PlayerId>, IComponent, INetworkedComponent, INetSerializable
	{
		private PlayerId _playerId;

		private byte _dirtyMask;

		public PlayerId PlayerId
		{
			get
			{
				return _playerId;
			}
			set
			{
				if (!_playerId.Equals(value))
				{
					_playerId = value;
					_dirtyMask |= 1;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public PlayerId GetIndexedValue()
		{
			return PlayerId;
		}

		public void Serialize(NetDataWriter writer)
		{
			_playerId.Serialize(writer);
		}

		public void Deserialize(NetDataReader reader)
		{
			_playerId.Deserialize(reader);
			_dirtyMask |= 1;
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				_playerId.Serialize(writer);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				_playerId.Deserialize(reader);
				_dirtyMask |= 1;
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				default(PlayerId).Deserialize(reader);
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
}
namespace ReadyM.Relay.Common.ECS.Archetypes
{
	public class DefaultAreaArchetypeRegistration(IAreaComponentRegistry areaComponentRegistry) : IArchetypeRegistration
	{
		private class RegisterAreaComponentsCallback(EntityBuilder builder) : IAreaComponentRegistryCallback, IComponentRegistryCallbackBase<IAreaComponentRegistry, IComponent>
		{
			public void AcceptComponent<T>(IAreaComponentRegistry registry, T defaultValue = default(T)) where T : struct, IComponent
			{
				builder.Add(in defaultValue);
			}
		}

		public ArchetypeId AreaArchetype { get; private set; }

		public void Register(Store world)
		{
			AreaArchetype = world.RegisterArchetype(delegate(EntityBuilder b)
			{
				b.Add<MetadataComponent>();
				b.Add<AreaScopeComponent>();
				b.AddTag<ScopeEntityTag>();
				areaComponentRegistry.Accept(new RegisterAreaComponentsCallback(b));
			});
		}
	}
	public class DefaultPlayerArchetypeRegistration(IPlayerComponentRegistry playerComponentRegistry) : IArchetypeRegistration
	{
		private class RegisterPlayerComponentsCallback(EntityBuilder builder) : IPlayerComponentRegistryCallback, IComponentRegistryCallbackBase<IPlayerComponentRegistry, IComponent>
		{
			public void AcceptComponent<T>(IPlayerComponentRegistry registry, T defaultValue = default(T)) where T : struct, IComponent
			{
				builder.Add(in defaultValue);
			}
		}

		public ArchetypeId PlayerArchetype { get; private set; }

		public void Register(Store world)
		{
			PlayerArchetype = world.RegisterArchetype(delegate(EntityBuilder b)
			{
				b.Add<MetadataComponent>();
				b.Add<PlayerScopeComponent>();
				b.AddTag<ScopeEntityTag>();
				playerComponentRegistry.Accept(new RegisterPlayerComponentsCallback(b));
			});
		}
	}
}
