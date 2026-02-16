using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Friflo.Engine.ECS;
using Friflo.Json.Fliox;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Helpers;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.Extensions;
using ReadyM.Api.Multiplayer.Generators;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Api.Serialization;
using ReadyM.Relay.Common.ECS.Tags;
using ReadyM.Relay.Common.Wukong.ECS.Components;
using ReadyM.Relay.Common.Wukong.ECS.Values;
using WukongMp.Api.ECS.Components;
using WukongMp.Api.ECS.Values;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("ReadyM.Relay.Common.Wukong")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+63464565c8693c0537c1bd91e59ef593e8f0bde2")]
[assembly: AssemblyProduct("ReadyM.Relay.Common.Wukong")]
[assembly: AssemblyTitle("ReadyM.Relay.Common.Wukong")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("1.0.422.1357")]
[module: UnverifiableCode]
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
namespace WukongMp.Api.ECS.Values
{
	public enum SpectatorReason
	{
		Observer,
		Death
	}
}
namespace WukongMp.Api.ECS.Components
{
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct PlayerComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private string _nickName;

		private int _teamId;

		private byte _dirtyMask;

		public string NickName
		{
			get
			{
				return _nickName;
			}
			set
			{
				if (!(_nickName?.Equals(value) ?? (value == null)))
				{
					_nickName = value;
					_dirtyMask |= 1;
				}
			}
		}

		public int TeamId
		{
			get
			{
				return _teamId;
			}
			set
			{
				if (!_teamId.Equals(value))
				{
					_teamId = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_nickName);
			writer.Put(_teamId);
		}

		public void Deserialize(NetDataReader reader)
		{
			NickName = reader.GetString();
			TeamId = reader.GetInt();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_nickName);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_teamId);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				NickName = reader.GetString();
			}
			if ((num & 2) != 0)
			{
				TeamId = reader.GetInt();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetString();
			}
			if ((num & 2) != 0)
			{
				reader.GetInt();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
}
namespace ReadyM.Relay.Common.Wukong.RPC
{
	public enum BeguilingChantState : byte
	{
		Inactive,
		Warning,
		Active
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	public struct SkipMovieData : INetSerializable
	{
		public int SequenceId;

		public int WaitingPlayers;

		public int AllPlayers;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(SequenceId);
			writer.Put(WaitingPlayers);
			writer.Put(AllPlayers);
		}

		public void Deserialize(NetDataReader reader)
		{
			SequenceId = reader.GetInt();
			WaitingPlayers = reader.GetInt();
			AllPlayers = reader.GetInt();
		}
	}
}
namespace ReadyM.Relay.Common.Wukong.ECS.Values
{
	public struct AttributesState : INetSerializable, IDeltaEquatable<AttributesState>
	{
		private Dictionary<byte, float> _data;

		public ReadOnlyDictionary<byte, float> Data => new ReadOnlyDictionary<byte, float>(_data);

		public AttributesState()
		{
			_data = new Dictionary<byte, float>();
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put((byte)Data.Count);
			foreach (KeyValuePair<byte, float> datum in Data)
			{
				writer.Put(datum.Key);
				writer.Put(datum.Value);
			}
		}

		public void Deserialize(NetDataReader reader)
		{
			byte b = reader.GetByte();
			if (_data == null)
			{
				_data = new Dictionary<byte, float>();
			}
			_data.Clear();
			StandardCollectionExtensions.EnsureCapacity(_data, b);
			for (int i = 0; i < b; i++)
			{
				byte key = reader.GetByte();
				float value = reader.GetFloat();
				_data?.Add(key, value);
			}
		}

		public bool DeltaEquals(AttributesState other, float delta)
		{
			foreach (KeyValuePair<byte, float> datum in _data)
			{
				if (!other._data.TryGetValue(datum.Key, out var value) || Math.Abs(datum.Value - value) > delta)
				{
					return false;
				}
			}
			foreach (KeyValuePair<byte, float> datum2 in other._data)
			{
				if (!_data.ContainsKey(datum2.Key))
				{
					return false;
				}
			}
			return true;
		}

		public float GetAttribute(byte attr)
		{
			return _data[attr];
		}

		public void SetAttribute(byte key, float value)
		{
			_data[key] = value;
		}

		public Dictionary<byte, float>.Enumerator GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		public bool TryGetAttribute(byte key, out float value)
		{
			return _data.TryGetValue(key, out value);
		}
	}
	public struct EquipmentState : INetSerializable, IDeltaEquatable<EquipmentState>
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<EquipmentState>
		{
			public override EquipmentState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, EquipmentState value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		private bool _locallyDirty;

		private int[] _equipments;

		public bool IsLocallyDirty => _locallyDirty;

		public EquipmentState()
		{
			_locallyDirty = false;
			_equipments = new int[8];
		}

		private EquipmentState(int[] eq)
		{
			_locallyDirty = true;
			_equipments = eq;
		}

		public EquipmentState(IEnumerable<(EquipPosition, int)> equipments)
		{
			_locallyDirty = true;
			_equipments = new int[8];
			foreach (var (equipPosition, num) in equipments)
			{
				_equipments[(uint)equipPosition] = num;
			}
		}

		public EquipmentState WithSetItem(EquipPosition position, int eqId)
		{
			EquipmentState result = new EquipmentState((int[])_equipments.Clone());
			result._equipments[(uint)position] = eqId;
			return result;
		}

		public IEnumerable<(EquipPosition, int)> GetItems()
		{
			for (int i = 0; i < 8; i++)
			{
				int num = _equipments[i];
				if (num != 0)
				{
					yield return ((EquipPosition)i, num);
				}
			}
		}

		public void ClearLocallyDirty()
		{
			_locallyDirty = false;
		}

		public void Serialize(NetDataWriter writer)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = _equipments[i];
				writer.Put(num);
			}
		}

		public void Deserialize(NetDataReader reader)
		{
			if (_equipments == null)
			{
				_equipments = new int[8];
			}
			_locallyDirty = true;
			for (int i = 0; i < 8; i++)
			{
				int num = reader.GetInt();
				_equipments[i] = num;
			}
		}

		public static void SerializeUntyped(NetDataWriter writer, object customObject)
		{
			writer.PutArray(((EquipmentState)customObject)._equipments);
		}

		public static object DeserializeUntyped(NetDataReader reader)
		{
			int[] intArray = reader.GetIntArray();
			if (intArray.Length != 8)
			{
				throw new ArgumentException($"Invalid equipment state length: {intArray.Length}");
			}
			return new EquipmentState(intArray);
		}

		public static void TextSerialize(Utf8JsonWriter writer, EquipmentState obj, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, obj._equipments, options);
		}

		public static EquipmentState TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			int[] array = JsonSerializer.Deserialize<int[]>(ref reader, options);
			if (array == null)
			{
				throw new JsonException("Failed to deserialize equipment state.");
			}
			if (array.Length != 8)
			{
				throw new JsonException($"Invalid equipment state length: {array.Length}");
			}
			return new EquipmentState(array);
		}

		public bool DeltaEquals(EquipmentState other, float delta)
		{
			if (_locallyDirty != other._locallyDirty)
			{
				return false;
			}
			if (_equipments.Length != other._equipments.Length)
			{
				return false;
			}
			for (int i = 0; i < _equipments.Length; i++)
			{
				if (_equipments[i] != other._equipments[i])
				{
					return false;
				}
			}
			return true;
		}
	}
	public enum EquipPosition : byte
	{
		Head,
		Upwear,
		Arm,
		Foot,
		Hulu,
		Weapon,
		Fabao,
		Accessory,
		EnumMax
	}
	public enum MoveSpeedLevel : byte
	{
		Walk,
		Run,
		Sprint
	}
}
namespace ReadyM.Relay.Common.Wukong.ECS.Registry
{
	public class WukongAreaRegistration : IAreaComponentRegistration, IComponentRegistrationBase<IAreaComponentRegistry, IComponent>
	{
		public void Register(IAreaComponentRegistry registry)
		{
			registry.RegisterComponent(new RoomComponent
			{
				ConsumablesAllowed = false,
				ImmobilizeAllowed = true,
				GourdAllowed = false,
				PhantomRushAllowed = true,
				CheatsAllowed = false,
				AntiStallEnabled = false
			});
			registry.RegisterComponent<MovieComponent>();
		}
	}
	public static class WukongComponentUtils
	{
		public static void SetupServerMonsterArchetype(EntityBuilder b)
		{
			TamerComponent component = new TamerComponent
			{
				HoldingPlayers = ImmutableHashSet.Create<PlayerId>(default(ReadOnlySpan<PlayerId>))
			};
			CreateEntityBatch createEntityBatch = b.Add(in component).Add<AnimationComponent>();
			HpComponent component2 = new HpComponent
			{
				HpMultiplier = 1f
			};
			createEntityBatch.Add(in component2).Add<MonsterAnimationComponent>().Add<NicknameComponent>()
				.Add<TeamComponent>()
				.Add<TransformComponent>();
		}

		public static void SetupServerMainCharacterArchetype(EntityBuilder b)
		{
			b.Add<MainCharacterComponent>(new MainCharacterComponent()).Add<TeamComponent>().Add<PvPComponent>()
				.AddTag<DisallowOwnershipTransferTag>();
		}

		public static void SetupServerPvpStateArchetype(EntityBuilder b)
		{
			b.Add<PvpStateComponent>();
		}
	}
	public class WukongNetworkedComponentRegistration : INetworkedComponentRegistration, IComponentRegistrationBase<INetworkedComponentRegistry, INetworkedComponent>
	{
		public void Register(INetworkedComponentRegistry registry)
		{
			registry.RegisterComponent<TamerComponent>((DeliveryMethod)2);
			registry.RegisterComponent<AnimationComponent>((DeliveryMethod)4);
			registry.RegisterComponent<HpComponent>((DeliveryMethod)2);
			registry.RegisterComponent<MonsterAnimationComponent>((DeliveryMethod)4);
			registry.RegisterComponent<NicknameComponent>((DeliveryMethod)2);
			registry.RegisterComponent<TeamComponent>((DeliveryMethod)2);
			registry.RegisterComponent<TransformComponent>((DeliveryMethod)4);
			registry.RegisterComponent<MainCharacterComponent>((DeliveryMethod)4);
			registry.RegisterComponent<RoomComponent>((DeliveryMethod)2);
			registry.RegisterComponent<MovieComponent>((DeliveryMethod)2);
			registry.RegisterComponent<PlayerComponent>((DeliveryMethod)2);
			registry.RegisterComponent<PvPComponent>((DeliveryMethod)2);
			registry.RegisterComponent<PvpStateComponent>((DeliveryMethod)2);
		}
	}
	public class WukongPlayerRegistration : IPlayerComponentRegistration, IComponentRegistrationBase<IPlayerComponentRegistry, IComponent>
	{
		public void Register(IPlayerComponentRegistry registry)
		{
			registry.RegisterComponent<PlayerComponent>();
		}
	}
}
namespace ReadyM.Relay.Common.Wukong.ECS.Components
{
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct AnimationComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private Vector3 _velocity;

		private Vector3 _moveAcceleration;

		private byte _moveSpeedLevel;

		private byte _moveSpeedState;

		private bool _shouldWaitRotateFinished;

		private byte _dirtyMask;

		public Vector3 Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				if (Vector3.DistanceSquared(_velocity, value) > 0.01f)
				{
					_velocity = value;
					_dirtyMask |= 1;
				}
			}
		}

		public Vector3 MoveAcceleration
		{
			get
			{
				return _moveAcceleration;
			}
			set
			{
				if (Vector3.DistanceSquared(_moveAcceleration, value) > 0.01f)
				{
					_moveAcceleration = value;
					_dirtyMask |= 2;
				}
			}
		}

		public byte MoveSpeedLevel
		{
			get
			{
				return _moveSpeedLevel;
			}
			set
			{
				if (!_moveSpeedLevel.Equals(value))
				{
					_moveSpeedLevel = value;
					_dirtyMask |= 4;
				}
			}
		}

		public byte MoveSpeedState
		{
			get
			{
				return _moveSpeedState;
			}
			set
			{
				if (!_moveSpeedState.Equals(value))
				{
					_moveSpeedState = value;
					_dirtyMask |= 8;
				}
			}
		}

		public bool ShouldWaitRotateFinished
		{
			get
			{
				return _shouldWaitRotateFinished;
			}
			set
			{
				if (!_shouldWaitRotateFinished.Equals(value))
				{
					_shouldWaitRotateFinished = value;
					_dirtyMask |= 16;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			_velocity.Serialize(writer);
			_moveAcceleration.Serialize(writer);
			writer.Put(_moveSpeedLevel);
			writer.Put(_moveSpeedState);
			writer.Put(_shouldWaitRotateFinished);
		}

		public void Deserialize(NetDataReader reader)
		{
			_velocity.Deserialize(reader);
			_dirtyMask |= 1;
			_moveAcceleration.Deserialize(reader);
			_dirtyMask |= 2;
			MoveSpeedLevel = reader.GetByte();
			MoveSpeedState = reader.GetByte();
			ShouldWaitRotateFinished = reader.GetBool();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				_velocity.Serialize(writer);
			}
			if ((dirtyMask & 2) != 0)
			{
				_moveAcceleration.Serialize(writer);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_moveSpeedLevel);
			}
			if ((dirtyMask & 8) != 0)
			{
				writer.Put(_moveSpeedState);
			}
			if ((dirtyMask & 0x10) != 0)
			{
				writer.Put(_shouldWaitRotateFinished);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				_velocity.Deserialize(reader);
				_dirtyMask |= 1;
			}
			if ((num & 2) != 0)
			{
				_moveAcceleration.Deserialize(reader);
				_dirtyMask |= 2;
			}
			if ((num & 4) != 0)
			{
				MoveSpeedLevel = reader.GetByte();
			}
			if ((num & 8) != 0)
			{
				MoveSpeedState = reader.GetByte();
			}
			if ((num & 0x10) != 0)
			{
				ShouldWaitRotateFinished = reader.GetBool();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				Vector3 vector = default(Vector3);
				vector.Deserialize(reader);
			}
			if ((num & 2) != 0)
			{
				Vector3 vector2 = default(Vector3);
				vector2.Deserialize(reader);
			}
			if ((num & 4) != 0)
			{
				reader.GetByte();
			}
			if ((num & 8) != 0)
			{
				reader.GetByte();
			}
			if ((num & 0x10) != 0)
			{
				reader.GetBool();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct EquipmentComponent : INetSerializable
	{
		public unsafe fixed int Slots[8];

		public unsafe Span<int> AsSpan()
		{
			fixed (int* slots = Slots)
			{
				return new Span<int>(slots, 8);
			}
		}

		public static EquipmentComponent FromSpan(Span<int> span)
		{
			if (span.Length != 8)
			{
				throw new ArgumentException("Span must be of length 8", "span");
			}
			EquipmentComponent result = default(EquipmentComponent);
			span.CopyTo(result.AsSpan());
			return result;
		}

		public unsafe void Serialize(NetDataWriter writer)
		{
			for (int i = 0; i < 8; i++)
			{
				writer.Put(Slots[i]);
			}
		}

		public unsafe void Deserialize(NetDataReader reader)
		{
			for (int i = 0; i < 8; i++)
			{
				Slots[i] = reader.GetInt();
			}
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct HpComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private float _hp;

		private float _hpMaxBase;

		private float _hpMultiplier;

		private byte _dirtyMask;

		public bool IsDead
		{
			get
			{
				if (Hp <= 0f)
				{
					return HpMaxBase > 0f;
				}
				return false;
			}
		}

		public float Hp
		{
			get
			{
				return _hp;
			}
			set
			{
				if (Math.Abs(_hp - value) > 0.1f)
				{
					_hp = value;
					_dirtyMask |= 1;
				}
			}
		}

		public float HpMaxBase
		{
			get
			{
				return _hpMaxBase;
			}
			set
			{
				if (Math.Abs(_hpMaxBase - value) > 0.1f)
				{
					_hpMaxBase = value;
					_dirtyMask |= 2;
				}
			}
		}

		public float HpMultiplier
		{
			get
			{
				return _hpMultiplier;
			}
			set
			{
				if (Math.Abs(_hpMultiplier - value) > 0.1f)
				{
					_hpMultiplier = value;
					_dirtyMask |= 4;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_hp);
			writer.Put(_hpMaxBase);
			writer.Put(_hpMultiplier);
		}

		public void Deserialize(NetDataReader reader)
		{
			Hp = reader.GetFloat();
			HpMaxBase = reader.GetFloat();
			HpMultiplier = reader.GetFloat();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_hp);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_hpMaxBase);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_hpMultiplier);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				Hp = reader.GetFloat();
			}
			if ((num & 2) != 0)
			{
				HpMaxBase = reader.GetFloat();
			}
			if ((num & 4) != 0)
			{
				HpMultiplier = reader.GetFloat();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetFloat();
			}
			if ((num & 2) != 0)
			{
				reader.GetFloat();
			}
			if ((num & 4) != 0)
			{
				reader.GetFloat();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct MainCharacterComponent : IIndexedComponent<PlayerId>, IComponent, INetworkedComponent, INetSerializable
	{
		private PlayerId _playerId;

		private Vector3 _location;

		private Vector3 _rotation;

		private Vector3 _velocity;

		private Vector3 _moveAcceleration;

		private MoveSpeedLevel _moveSpeedLevel;

		private MoveSpeedLevel _moveSpeedState;

		private float _hp;

		private float _hpMaxBase;

		private int _rebirthPointId;

		private int _waitingSequenceId;

		private bool _isTransformed;

		private string _characterNickName;

		private bool _isDead;

		private bool _beguilingChantEligible;

		private bool _inJump;

		private bool _isFlying;

		private bool _isFalling;

		private bool _isLandingMove;

		private Vector3 _turnInplaceTargetRotation;

		private bool _isStandRotate;

		private float _turnInplaceRemainAngle;

		private bool _isAttacking;

		private bool _orientRotationToMovement;

		private bool _shouldWaitRotateFinished;

		private AttributesState _attributes;

		private EquipmentState _equipment;

		private uint _dirtyMask;

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
					_dirtyMask |= 1u;
				}
			}
		}

		public Vector3 Location
		{
			get
			{
				return _location;
			}
			set
			{
				if (Vector3.DistanceSquared(_location, value) > 0.01f)
				{
					_location = value;
					_dirtyMask |= 2u;
				}
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				if (Vector3.DistanceSquared(_rotation, value) > 0.01f)
				{
					_rotation = value;
					_dirtyMask |= 4u;
				}
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				if (Vector3.DistanceSquared(_velocity, value) > 0.01f)
				{
					_velocity = value;
					_dirtyMask |= 8u;
				}
			}
		}

		public Vector3 MoveAcceleration
		{
			get
			{
				return _moveAcceleration;
			}
			set
			{
				if (Vector3.DistanceSquared(_moveAcceleration, value) > 0.01f)
				{
					_moveAcceleration = value;
					_dirtyMask |= 16u;
				}
			}
		}

		public MoveSpeedLevel MoveSpeedLevel
		{
			get
			{
				return _moveSpeedLevel;
			}
			set
			{
				if (_moveSpeedLevel != value)
				{
					_moveSpeedLevel = value;
					_dirtyMask |= 32u;
				}
			}
		}

		public MoveSpeedLevel MoveSpeedState
		{
			get
			{
				return _moveSpeedState;
			}
			set
			{
				if (_moveSpeedState != value)
				{
					_moveSpeedState = value;
					_dirtyMask |= 64u;
				}
			}
		}

		public float Hp
		{
			get
			{
				return _hp;
			}
			set
			{
				if (Math.Abs(_hp - value) > 0.1f)
				{
					_hp = value;
					_dirtyMask |= 128u;
				}
			}
		}

		public float HpMaxBase
		{
			get
			{
				return _hpMaxBase;
			}
			set
			{
				if (Math.Abs(_hpMaxBase - value) > 0.1f)
				{
					_hpMaxBase = value;
					_dirtyMask |= 256u;
				}
			}
		}

		public int RebirthPointId
		{
			get
			{
				return _rebirthPointId;
			}
			set
			{
				if (!_rebirthPointId.Equals(value))
				{
					_rebirthPointId = value;
					_dirtyMask |= 512u;
				}
			}
		}

		public int WaitingSequenceId
		{
			get
			{
				return _waitingSequenceId;
			}
			set
			{
				if (!_waitingSequenceId.Equals(value))
				{
					_waitingSequenceId = value;
					_dirtyMask |= 1024u;
				}
			}
		}

		public bool IsTransformed
		{
			get
			{
				return _isTransformed;
			}
			set
			{
				if (!_isTransformed.Equals(value))
				{
					_isTransformed = value;
					_dirtyMask |= 2048u;
				}
			}
		}

		public string CharacterNickName
		{
			get
			{
				return _characterNickName;
			}
			set
			{
				if (!(_characterNickName?.Equals(value) ?? (value == null)))
				{
					_characterNickName = value;
					_dirtyMask |= 4096u;
				}
			}
		}

		public bool IsDead
		{
			get
			{
				return _isDead;
			}
			set
			{
				if (!_isDead.Equals(value))
				{
					_isDead = value;
					_dirtyMask |= 8192u;
				}
			}
		}

		public bool BeguilingChantEligible
		{
			get
			{
				return _beguilingChantEligible;
			}
			set
			{
				if (!_beguilingChantEligible.Equals(value))
				{
					_beguilingChantEligible = value;
					_dirtyMask |= 16384u;
				}
			}
		}

		public bool InJump
		{
			get
			{
				return _inJump;
			}
			set
			{
				if (!_inJump.Equals(value))
				{
					_inJump = value;
					_dirtyMask |= 32768u;
				}
			}
		}

		public bool IsFlying
		{
			get
			{
				return _isFlying;
			}
			set
			{
				if (!_isFlying.Equals(value))
				{
					_isFlying = value;
					_dirtyMask |= 65536u;
				}
			}
		}

		public bool IsFalling
		{
			get
			{
				return _isFalling;
			}
			set
			{
				if (!_isFalling.Equals(value))
				{
					_isFalling = value;
					_dirtyMask |= 131072u;
				}
			}
		}

		public bool IsLandingMove
		{
			get
			{
				return _isLandingMove;
			}
			set
			{
				if (!_isLandingMove.Equals(value))
				{
					_isLandingMove = value;
					_dirtyMask |= 262144u;
				}
			}
		}

		public Vector3 TurnInplaceTargetRotation
		{
			get
			{
				return _turnInplaceTargetRotation;
			}
			set
			{
				if (Vector3.DistanceSquared(_turnInplaceTargetRotation, value) > 0.01f)
				{
					_turnInplaceTargetRotation = value;
					_dirtyMask |= 524288u;
				}
			}
		}

		public bool IsStandRotate
		{
			get
			{
				return _isStandRotate;
			}
			set
			{
				if (!_isStandRotate.Equals(value))
				{
					_isStandRotate = value;
					_dirtyMask |= 1048576u;
				}
			}
		}

		public float TurnInplaceRemainAngle
		{
			get
			{
				return _turnInplaceRemainAngle;
			}
			set
			{
				if (Math.Abs(_turnInplaceRemainAngle - value) > 0.1f)
				{
					_turnInplaceRemainAngle = value;
					_dirtyMask |= 2097152u;
				}
			}
		}

		public bool IsAttacking
		{
			get
			{
				return _isAttacking;
			}
			set
			{
				if (!_isAttacking.Equals(value))
				{
					_isAttacking = value;
					_dirtyMask |= 4194304u;
				}
			}
		}

		public bool OrientRotationToMovement
		{
			get
			{
				return _orientRotationToMovement;
			}
			set
			{
				if (!_orientRotationToMovement.Equals(value))
				{
					_orientRotationToMovement = value;
					_dirtyMask |= 8388608u;
				}
			}
		}

		public bool ShouldWaitRotateFinished
		{
			get
			{
				return _shouldWaitRotateFinished;
			}
			set
			{
				if (!_shouldWaitRotateFinished.Equals(value))
				{
					_shouldWaitRotateFinished = value;
					_dirtyMask |= 16777216u;
				}
			}
		}

		public AttributesState Attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				if (!_attributes.DeltaEquals(value, 0.01f))
				{
					_attributes = value;
					_dirtyMask |= 33554432u;
				}
			}
		}

		public EquipmentState Equipment
		{
			get
			{
				return _equipment;
			}
			set
			{
				if (!_equipment.DeltaEquals(value, 0.01f))
				{
					_equipment = value;
					_dirtyMask |= 67108864u;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public MainCharacterComponent()
		{
			_playerId = default(PlayerId);
			_location = default(Vector3);
			_rotation = default(Vector3);
			_velocity = default(Vector3);
			_moveAcceleration = default(Vector3);
			_hp = 0f;
			_hpMaxBase = 0f;
			_rebirthPointId = 0;
			_waitingSequenceId = 0;
			_isTransformed = false;
			_isDead = false;
			_beguilingChantEligible = false;
			_inJump = false;
			_isFlying = false;
			_isFalling = false;
			_isLandingMove = false;
			_turnInplaceTargetRotation = default(Vector3);
			_isStandRotate = false;
			_turnInplaceRemainAngle = 0f;
			_isAttacking = false;
			_orientRotationToMovement = false;
			_shouldWaitRotateFinished = false;
			_dirtyMask = 0u;
			_moveSpeedLevel = MoveSpeedLevel.Run;
			_moveSpeedState = MoveSpeedLevel.Run;
			_characterNickName = "";
			_attributes = new AttributesState();
			_equipment = new EquipmentState();
		}

		public PlayerId GetIndexedValue()
		{
			return PlayerId;
		}

		public void Serialize(NetDataWriter writer)
		{
			_playerId.Serialize(writer);
			_location.Serialize(writer);
			_rotation.Serialize(writer);
			_velocity.Serialize(writer);
			_moveAcceleration.Serialize(writer);
			writer.Put((int)_moveSpeedLevel);
			writer.Put((int)_moveSpeedState);
			writer.Put(_hp);
			writer.Put(_hpMaxBase);
			writer.Put(_rebirthPointId);
			writer.Put(_waitingSequenceId);
			writer.Put(_isTransformed);
			writer.Put(_characterNickName);
			writer.Put(_isDead);
			writer.Put(_beguilingChantEligible);
			writer.Put(_inJump);
			writer.Put(_isFlying);
			writer.Put(_isFalling);
			writer.Put(_isLandingMove);
			_turnInplaceTargetRotation.Serialize(writer);
			writer.Put(_isStandRotate);
			writer.Put(_turnInplaceRemainAngle);
			writer.Put(_isAttacking);
			writer.Put(_orientRotationToMovement);
			writer.Put(_shouldWaitRotateFinished);
			_attributes.Serialize(writer);
			_equipment.Serialize(writer);
		}

		public void Deserialize(NetDataReader reader)
		{
			_playerId.Deserialize(reader);
			_dirtyMask |= 1u;
			_location.Deserialize(reader);
			_dirtyMask |= 2u;
			_rotation.Deserialize(reader);
			_dirtyMask |= 4u;
			_velocity.Deserialize(reader);
			_dirtyMask |= 8u;
			_moveAcceleration.Deserialize(reader);
			_dirtyMask |= 16u;
			_moveSpeedLevel = (MoveSpeedLevel)reader.GetInt();
			_moveSpeedState = (MoveSpeedLevel)reader.GetInt();
			Hp = reader.GetFloat();
			HpMaxBase = reader.GetFloat();
			RebirthPointId = reader.GetInt();
			WaitingSequenceId = reader.GetInt();
			IsTransformed = reader.GetBool();
			CharacterNickName = reader.GetString();
			IsDead = reader.GetBool();
			BeguilingChantEligible = reader.GetBool();
			InJump = reader.GetBool();
			IsFlying = reader.GetBool();
			IsFalling = reader.GetBool();
			IsLandingMove = reader.GetBool();
			_turnInplaceTargetRotation.Deserialize(reader);
			_dirtyMask |= 524288u;
			IsStandRotate = reader.GetBool();
			TurnInplaceRemainAngle = reader.GetFloat();
			IsAttacking = reader.GetBool();
			OrientRotationToMovement = reader.GetBool();
			ShouldWaitRotateFinished = reader.GetBool();
			_attributes.Deserialize(reader);
			_dirtyMask |= 33554432u;
			_equipment.Deserialize(reader);
			_dirtyMask |= 67108864u;
		}

		public void WriteDelta(NetDataWriter writer)
		{
			uint dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				_playerId.Serialize(writer);
			}
			if ((dirtyMask & 2) != 0)
			{
				_location.Serialize(writer);
			}
			if ((dirtyMask & 4) != 0)
			{
				_rotation.Serialize(writer);
			}
			if ((dirtyMask & 8) != 0)
			{
				_velocity.Serialize(writer);
			}
			if ((dirtyMask & 0x10) != 0)
			{
				_moveAcceleration.Serialize(writer);
			}
			if ((dirtyMask & 0x20) != 0)
			{
				writer.Put((int)_moveSpeedLevel);
			}
			if ((dirtyMask & 0x40) != 0)
			{
				writer.Put((int)_moveSpeedState);
			}
			if ((dirtyMask & 0x80) != 0)
			{
				writer.Put(_hp);
			}
			if ((dirtyMask & 0x100) != 0)
			{
				writer.Put(_hpMaxBase);
			}
			if ((dirtyMask & 0x200) != 0)
			{
				writer.Put(_rebirthPointId);
			}
			if ((dirtyMask & 0x400) != 0)
			{
				writer.Put(_waitingSequenceId);
			}
			if ((dirtyMask & 0x800) != 0)
			{
				writer.Put(_isTransformed);
			}
			if ((dirtyMask & 0x1000) != 0)
			{
				writer.Put(_characterNickName);
			}
			if ((dirtyMask & 0x2000) != 0)
			{
				writer.Put(_isDead);
			}
			if ((dirtyMask & 0x4000) != 0)
			{
				writer.Put(_beguilingChantEligible);
			}
			if ((dirtyMask & 0x8000) != 0)
			{
				writer.Put(_inJump);
			}
			if ((dirtyMask & 0x10000) != 0)
			{
				writer.Put(_isFlying);
			}
			if ((dirtyMask & 0x20000) != 0)
			{
				writer.Put(_isFalling);
			}
			if ((dirtyMask & 0x40000) != 0)
			{
				writer.Put(_isLandingMove);
			}
			if ((dirtyMask & 0x80000) != 0)
			{
				_turnInplaceTargetRotation.Serialize(writer);
			}
			if ((dirtyMask & 0x100000) != 0)
			{
				writer.Put(_isStandRotate);
			}
			if ((dirtyMask & 0x200000) != 0)
			{
				writer.Put(_turnInplaceRemainAngle);
			}
			if ((dirtyMask & 0x400000) != 0)
			{
				writer.Put(_isAttacking);
			}
			if ((dirtyMask & 0x800000) != 0)
			{
				writer.Put(_orientRotationToMovement);
			}
			if ((dirtyMask & 0x1000000) != 0)
			{
				writer.Put(_shouldWaitRotateFinished);
			}
			if ((dirtyMask & 0x2000000) != 0)
			{
				_attributes.Serialize(writer);
			}
			if ((dirtyMask & 0x4000000) != 0)
			{
				_equipment.Serialize(writer);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			uint uInt = reader.GetUInt();
			if ((uInt & 1) != 0)
			{
				_playerId.Deserialize(reader);
				_dirtyMask |= 1u;
			}
			if ((uInt & 2) != 0)
			{
				_location.Deserialize(reader);
				_dirtyMask |= 2u;
			}
			if ((uInt & 4) != 0)
			{
				_rotation.Deserialize(reader);
				_dirtyMask |= 4u;
			}
			if ((uInt & 8) != 0)
			{
				_velocity.Deserialize(reader);
				_dirtyMask |= 8u;
			}
			if ((uInt & 0x10) != 0)
			{
				_moveAcceleration.Deserialize(reader);
				_dirtyMask |= 16u;
			}
			if ((uInt & 0x20) != 0)
			{
				MoveSpeedLevel = (MoveSpeedLevel)reader.GetInt();
			}
			if ((uInt & 0x40) != 0)
			{
				MoveSpeedState = (MoveSpeedLevel)reader.GetInt();
			}
			if ((uInt & 0x80) != 0)
			{
				Hp = reader.GetFloat();
			}
			if ((uInt & 0x100) != 0)
			{
				HpMaxBase = reader.GetFloat();
			}
			if ((uInt & 0x200) != 0)
			{
				RebirthPointId = reader.GetInt();
			}
			if ((uInt & 0x400) != 0)
			{
				WaitingSequenceId = reader.GetInt();
			}
			if ((uInt & 0x800) != 0)
			{
				IsTransformed = reader.GetBool();
			}
			if ((uInt & 0x1000) != 0)
			{
				CharacterNickName = reader.GetString();
			}
			if ((uInt & 0x2000) != 0)
			{
				IsDead = reader.GetBool();
			}
			if ((uInt & 0x4000) != 0)
			{
				BeguilingChantEligible = reader.GetBool();
			}
			if ((uInt & 0x8000) != 0)
			{
				InJump = reader.GetBool();
			}
			if ((uInt & 0x10000) != 0)
			{
				IsFlying = reader.GetBool();
			}
			if ((uInt & 0x20000) != 0)
			{
				IsFalling = reader.GetBool();
			}
			if ((uInt & 0x40000) != 0)
			{
				IsLandingMove = reader.GetBool();
			}
			if ((uInt & 0x80000) != 0)
			{
				_turnInplaceTargetRotation.Deserialize(reader);
				_dirtyMask |= 524288u;
			}
			if ((uInt & 0x100000) != 0)
			{
				IsStandRotate = reader.GetBool();
			}
			if ((uInt & 0x200000) != 0)
			{
				TurnInplaceRemainAngle = reader.GetFloat();
			}
			if ((uInt & 0x400000) != 0)
			{
				IsAttacking = reader.GetBool();
			}
			if ((uInt & 0x800000) != 0)
			{
				OrientRotationToMovement = reader.GetBool();
			}
			if ((uInt & 0x1000000) != 0)
			{
				ShouldWaitRotateFinished = reader.GetBool();
			}
			if ((uInt & 0x2000000) != 0)
			{
				_attributes.Deserialize(reader);
				_dirtyMask |= 33554432u;
			}
			if ((uInt & 0x4000000) != 0)
			{
				_equipment.Deserialize(reader);
				_dirtyMask |= 67108864u;
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			uint uInt = reader.GetUInt();
			if ((uInt & 1) != 0)
			{
				default(PlayerId).Deserialize(reader);
			}
			if ((uInt & 2) != 0)
			{
				Vector3 vector = default(Vector3);
				vector.Deserialize(reader);
			}
			if ((uInt & 4) != 0)
			{
				Vector3 vector2 = default(Vector3);
				vector2.Deserialize(reader);
			}
			if ((uInt & 8) != 0)
			{
				Vector3 vector3 = default(Vector3);
				vector3.Deserialize(reader);
			}
			if ((uInt & 0x10) != 0)
			{
				Vector3 vector4 = default(Vector3);
				vector4.Deserialize(reader);
			}
			if ((uInt & 0x20) != 0)
			{
				reader.GetInt();
			}
			if ((uInt & 0x40) != 0)
			{
				reader.GetInt();
			}
			if ((uInt & 0x80) != 0)
			{
				reader.GetFloat();
			}
			if ((uInt & 0x100) != 0)
			{
				reader.GetFloat();
			}
			if ((uInt & 0x200) != 0)
			{
				reader.GetInt();
			}
			if ((uInt & 0x400) != 0)
			{
				reader.GetInt();
			}
			if ((uInt & 0x800) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x1000) != 0)
			{
				reader.GetString();
			}
			if ((uInt & 0x2000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x4000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x8000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x10000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x20000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x40000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x80000) != 0)
			{
				Vector3 vector5 = default(Vector3);
				vector5.Deserialize(reader);
			}
			if ((uInt & 0x100000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x200000) != 0)
			{
				reader.GetFloat();
			}
			if ((uInt & 0x400000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x800000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x1000000) != 0)
			{
				reader.GetBool();
			}
			if ((uInt & 0x2000000) != 0)
			{
				default(AttributesState).Deserialize(reader);
			}
			if ((uInt & 0x4000000) != 0)
			{
				default(EquipmentState).Deserialize(reader);
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0u;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct MonsterAnimationComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private byte _moveAiType;

		private float _animationPlayRate;

		private byte _dirtyMask;

		public byte MoveAiType
		{
			get
			{
				return _moveAiType;
			}
			set
			{
				if (!_moveAiType.Equals(value))
				{
					_moveAiType = value;
					_dirtyMask |= 1;
				}
			}
		}

		public float AnimationPlayRate
		{
			get
			{
				return _animationPlayRate;
			}
			set
			{
				if (Math.Abs(_animationPlayRate - value) > 0.1f)
				{
					_animationPlayRate = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_moveAiType);
			writer.Put(_animationPlayRate);
		}

		public void Deserialize(NetDataReader reader)
		{
			MoveAiType = reader.GetByte();
			AnimationPlayRate = reader.GetFloat();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_moveAiType);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_animationPlayRate);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				MoveAiType = reader.GetByte();
			}
			if ((num & 2) != 0)
			{
				AnimationPlayRate = reader.GetFloat();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetByte();
			}
			if ((num & 2) != 0)
			{
				reader.GetFloat();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct MovieComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private string? _startedSequencesEncoded;

		private string? _finishedSequencesEncoded;

		private byte _dirtyMask;

		[Ignore]
		public ImmutableHashSet<int> StartedSequences
		{
			get
			{
				string startedSequencesEncoded = StartedSequencesEncoded;
				IEnumerable<int> source;
				if (startedSequencesEncoded != null)
				{
					source = startedSequencesEncoded.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
				}
				else
				{
					IEnumerable<int> enumerable = Array.Empty<int>();
					source = enumerable;
				}
				return ImmutableHashSet.Create<int>(new ReadOnlySpan<int>(source.ToArray()));
			}
			set
			{
				StartedSequencesEncoded = string.Join(";", value.Select((int s) => s.ToString()));
			}
		}

		[Ignore]
		public ImmutableHashSet<int> FinishedSequences
		{
			get
			{
				string finishedSequencesEncoded = FinishedSequencesEncoded;
				IEnumerable<int> source;
				if (finishedSequencesEncoded != null)
				{
					source = finishedSequencesEncoded.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
				}
				else
				{
					IEnumerable<int> enumerable = Array.Empty<int>();
					source = enumerable;
				}
				return ImmutableHashSet.Create<int>(new ReadOnlySpan<int>(source.ToArray()));
			}
			set
			{
				FinishedSequencesEncoded = string.Join(";", value.Select((int s) => s.ToString()));
			}
		}

		public string? StartedSequencesEncoded
		{
			get
			{
				return _startedSequencesEncoded;
			}
			set
			{
				if (!(_startedSequencesEncoded?.Equals(value) ?? (value == null)))
				{
					_startedSequencesEncoded = value;
					_dirtyMask |= 1;
				}
			}
		}

		public string? FinishedSequencesEncoded
		{
			get
			{
				return _finishedSequencesEncoded;
			}
			set
			{
				if (!(_finishedSequencesEncoded?.Equals(value) ?? (value == null)))
				{
					_finishedSequencesEncoded = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_startedSequencesEncoded);
			writer.Put(_finishedSequencesEncoded);
		}

		public void Deserialize(NetDataReader reader)
		{
			StartedSequencesEncoded = reader.GetString();
			FinishedSequencesEncoded = reader.GetString();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_startedSequencesEncoded);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_finishedSequencesEncoded);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				StartedSequencesEncoded = reader.GetString();
			}
			if ((num & 2) != 0)
			{
				FinishedSequencesEncoded = reader.GetString();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetString();
			}
			if ((num & 2) != 0)
			{
				reader.GetString();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct NicknameComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private string _nickname;

		private byte _dirtyMask;

		public string Nickname
		{
			get
			{
				return _nickname;
			}
			set
			{
				if (!(_nickname?.Equals(value) ?? (value == null)))
				{
					_nickname = value;
					_dirtyMask |= 1;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_nickname);
		}

		public void Deserialize(NetDataReader reader)
		{
			Nickname = reader.GetString();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_nickname);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				Nickname = reader.GetString();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				reader.GetString();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct PvPComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private SpectatorReason _spectatorReason;

		private bool _isReadyForPvP;

		private bool _isSpectator;

		private byte _dirtyMask;

		public bool IsObserver
		{
			get
			{
				if (_isSpectator)
				{
					return _spectatorReason == SpectatorReason.Observer;
				}
				return false;
			}
		}

		public SpectatorReason SpectatorReason
		{
			get
			{
				return _spectatorReason;
			}
			set
			{
				if (_spectatorReason != value)
				{
					_spectatorReason = value;
					_dirtyMask |= 1;
				}
			}
		}

		public bool IsReadyForPvP
		{
			get
			{
				return _isReadyForPvP;
			}
			set
			{
				if (!_isReadyForPvP.Equals(value))
				{
					_isReadyForPvP = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsSpectator
		{
			get
			{
				return _isSpectator;
			}
			set
			{
				if (!_isSpectator.Equals(value))
				{
					_isSpectator = value;
					_dirtyMask |= 4;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put((int)_spectatorReason);
			writer.Put(_isReadyForPvP);
			writer.Put(_isSpectator);
		}

		public void Deserialize(NetDataReader reader)
		{
			_spectatorReason = (SpectatorReason)reader.GetInt();
			IsReadyForPvP = reader.GetBool();
			IsSpectator = reader.GetBool();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put((int)_spectatorReason);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_isReadyForPvP);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_isSpectator);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				SpectatorReason = (SpectatorReason)reader.GetInt();
			}
			if ((num & 2) != 0)
			{
				IsReadyForPvP = reader.GetBool();
			}
			if ((num & 4) != 0)
			{
				IsSpectator = reader.GetBool();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetInt();
			}
			if ((num & 2) != 0)
			{
				reader.GetBool();
			}
			if ((num & 4) != 0)
			{
				reader.GetBool();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct PvpStateComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private bool _inPvP;

		private bool _inTournament;

		private string? _roundWinnersEncoded;

		private byte _dirtyMask;

		[Ignore]
		public IEnumerable<int> RoundWinners
		{
			get
			{
				string roundWinnersEncoded = RoundWinnersEncoded;
				if (roundWinnersEncoded != null)
				{
					return roundWinnersEncoded.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
				}
				return Array.Empty<int>();
			}
			set
			{
				RoundWinnersEncoded = string.Join(";", value);
			}
		}

		public int CurrentRound => RoundWinners.Count() + 1;

		public bool InPvP
		{
			get
			{
				return _inPvP;
			}
			set
			{
				if (!_inPvP.Equals(value))
				{
					_inPvP = value;
					_dirtyMask |= 1;
				}
			}
		}

		public bool InTournament
		{
			get
			{
				return _inTournament;
			}
			set
			{
				if (!_inTournament.Equals(value))
				{
					_inTournament = value;
					_dirtyMask |= 2;
				}
			}
		}

		public string? RoundWinnersEncoded
		{
			get
			{
				return _roundWinnersEncoded;
			}
			set
			{
				if (!(_roundWinnersEncoded?.Equals(value) ?? (value == null)))
				{
					_roundWinnersEncoded = value;
					_dirtyMask |= 4;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void SetLastRoundWinnerTeam(int teamId)
		{
			List<int> list = RoundWinners.ToList();
			list.Add(teamId);
			RoundWinners = list;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_inPvP);
			writer.Put(_inTournament);
			writer.Put(_roundWinnersEncoded);
		}

		public void Deserialize(NetDataReader reader)
		{
			InPvP = reader.GetBool();
			InTournament = reader.GetBool();
			RoundWinnersEncoded = reader.GetString();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_inPvP);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_inTournament);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_roundWinnersEncoded);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				InPvP = reader.GetBool();
			}
			if ((num & 2) != 0)
			{
				InTournament = reader.GetBool();
			}
			if ((num & 4) != 0)
			{
				RoundWinnersEncoded = reader.GetString();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetBool();
			}
			if ((num & 2) != 0)
			{
				reader.GetBool();
			}
			if ((num & 4) != 0)
			{
				reader.GetString();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct RoomComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private int _levelId;

		private int _tournamentRounds;

		private bool _gourdAllowed;

		private bool _consumablesAllowed;

		private bool _immobilizeAllowed;

		private bool _phantomRushAllowed;

		private int _enemiesNgPlusLevel;

		private bool _cheatsAllowed;

		private bool _chatEnabled;

		private bool _antiStallEnabled;

		private ushort _dirtyMask;

		public int LevelId
		{
			get
			{
				return _levelId;
			}
			set
			{
				if (!_levelId.Equals(value))
				{
					_levelId = value;
					_dirtyMask |= 1;
				}
			}
		}

		public int TournamentRounds
		{
			get
			{
				return _tournamentRounds;
			}
			set
			{
				if (!_tournamentRounds.Equals(value))
				{
					_tournamentRounds = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool GourdAllowed
		{
			get
			{
				return _gourdAllowed;
			}
			set
			{
				if (!_gourdAllowed.Equals(value))
				{
					_gourdAllowed = value;
					_dirtyMask |= 4;
				}
			}
		}

		public bool ConsumablesAllowed
		{
			get
			{
				return _consumablesAllowed;
			}
			set
			{
				if (!_consumablesAllowed.Equals(value))
				{
					_consumablesAllowed = value;
					_dirtyMask |= 8;
				}
			}
		}

		public bool ImmobilizeAllowed
		{
			get
			{
				return _immobilizeAllowed;
			}
			set
			{
				if (!_immobilizeAllowed.Equals(value))
				{
					_immobilizeAllowed = value;
					_dirtyMask |= 16;
				}
			}
		}

		public bool PhantomRushAllowed
		{
			get
			{
				return _phantomRushAllowed;
			}
			set
			{
				if (!_phantomRushAllowed.Equals(value))
				{
					_phantomRushAllowed = value;
					_dirtyMask |= 32;
				}
			}
		}

		public int EnemiesNgPlusLevel
		{
			get
			{
				return _enemiesNgPlusLevel;
			}
			set
			{
				if (!_enemiesNgPlusLevel.Equals(value))
				{
					_enemiesNgPlusLevel = value;
					_dirtyMask |= 64;
				}
			}
		}

		public bool CheatsAllowed
		{
			get
			{
				return _cheatsAllowed;
			}
			set
			{
				if (!_cheatsAllowed.Equals(value))
				{
					_cheatsAllowed = value;
					_dirtyMask |= 128;
				}
			}
		}

		public bool ChatEnabled
		{
			get
			{
				return _chatEnabled;
			}
			set
			{
				if (!_chatEnabled.Equals(value))
				{
					_chatEnabled = value;
					_dirtyMask |= 256;
				}
			}
		}

		public bool AntiStallEnabled
		{
			get
			{
				return _antiStallEnabled;
			}
			set
			{
				if (!_antiStallEnabled.Equals(value))
				{
					_antiStallEnabled = value;
					_dirtyMask |= 512;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_levelId);
			writer.Put(_tournamentRounds);
			writer.Put(_gourdAllowed);
			writer.Put(_consumablesAllowed);
			writer.Put(_immobilizeAllowed);
			writer.Put(_phantomRushAllowed);
			writer.Put(_enemiesNgPlusLevel);
			writer.Put(_cheatsAllowed);
			writer.Put(_chatEnabled);
			writer.Put(_antiStallEnabled);
		}

		public void Deserialize(NetDataReader reader)
		{
			LevelId = reader.GetInt();
			TournamentRounds = reader.GetInt();
			GourdAllowed = reader.GetBool();
			ConsumablesAllowed = reader.GetBool();
			ImmobilizeAllowed = reader.GetBool();
			PhantomRushAllowed = reader.GetBool();
			EnemiesNgPlusLevel = reader.GetInt();
			CheatsAllowed = reader.GetBool();
			ChatEnabled = reader.GetBool();
			AntiStallEnabled = reader.GetBool();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			ushort dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_levelId);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_tournamentRounds);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_gourdAllowed);
			}
			if ((dirtyMask & 8) != 0)
			{
				writer.Put(_consumablesAllowed);
			}
			if ((dirtyMask & 0x10) != 0)
			{
				writer.Put(_immobilizeAllowed);
			}
			if ((dirtyMask & 0x20) != 0)
			{
				writer.Put(_phantomRushAllowed);
			}
			if ((dirtyMask & 0x40) != 0)
			{
				writer.Put(_enemiesNgPlusLevel);
			}
			if ((dirtyMask & 0x80) != 0)
			{
				writer.Put(_cheatsAllowed);
			}
			if ((dirtyMask & 0x100) != 0)
			{
				writer.Put(_chatEnabled);
			}
			if ((dirtyMask & 0x200) != 0)
			{
				writer.Put(_antiStallEnabled);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			ushort uShort = reader.GetUShort();
			if ((uShort & 1) != 0)
			{
				LevelId = reader.GetInt();
			}
			if ((uShort & 2) != 0)
			{
				TournamentRounds = reader.GetInt();
			}
			if ((uShort & 4) != 0)
			{
				GourdAllowed = reader.GetBool();
			}
			if ((uShort & 8) != 0)
			{
				ConsumablesAllowed = reader.GetBool();
			}
			if ((uShort & 0x10) != 0)
			{
				ImmobilizeAllowed = reader.GetBool();
			}
			if ((uShort & 0x20) != 0)
			{
				PhantomRushAllowed = reader.GetBool();
			}
			if ((uShort & 0x40) != 0)
			{
				EnemiesNgPlusLevel = reader.GetInt();
			}
			if ((uShort & 0x80) != 0)
			{
				CheatsAllowed = reader.GetBool();
			}
			if ((uShort & 0x100) != 0)
			{
				ChatEnabled = reader.GetBool();
			}
			if ((uShort & 0x200) != 0)
			{
				AntiStallEnabled = reader.GetBool();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			ushort uShort = reader.GetUShort();
			if ((uShort & 1) != 0)
			{
				reader.GetInt();
			}
			if ((uShort & 2) != 0)
			{
				reader.GetInt();
			}
			if ((uShort & 4) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 8) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 0x10) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 0x20) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 0x40) != 0)
			{
				reader.GetInt();
			}
			if ((uShort & 0x80) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 0x100) != 0)
			{
				reader.GetBool();
			}
			if ((uShort & 0x200) != 0)
			{
				reader.GetBool();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct TamerComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private string? _guid;

		private string? _unitPath;

		private string? _holdingPlayersEncoded;

		private bool _hasFsmPaused;

		private byte _dirtyMask;

		[Ignore]
		public ImmutableHashSet<PlayerId> HoldingPlayers
		{
			get
			{
				string holdingPlayersEncoded = HoldingPlayersEncoded;
				IEnumerable<PlayerId> source;
				if (holdingPlayersEncoded != null)
				{
					source = from s in holdingPlayersEncoded.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new PlayerId(ushort.Parse(s));
				}
				else
				{
					IEnumerable<PlayerId> enumerable = Array.Empty<PlayerId>();
					source = enumerable;
				}
				return ImmutableHashSet.Create<PlayerId>(new ReadOnlySpan<PlayerId>(source.ToArray()));
			}
			set
			{
				HoldingPlayersEncoded = string.Join(";", value.Select((PlayerId s) => s.RawValue.ToString()));
			}
		}

		public bool ShouldBeSpawned => HoldingPlayers.Count > 0;

		public string? Guid
		{
			get
			{
				return _guid;
			}
			set
			{
				if (!(_guid?.Equals(value) ?? (value == null)))
				{
					_guid = value;
					_dirtyMask |= 1;
				}
			}
		}

		public string? UnitPath
		{
			get
			{
				return _unitPath;
			}
			set
			{
				if (!(_unitPath?.Equals(value) ?? (value == null)))
				{
					_unitPath = value;
					_dirtyMask |= 2;
				}
			}
		}

		public string? HoldingPlayersEncoded
		{
			get
			{
				return _holdingPlayersEncoded;
			}
			set
			{
				if (!(_holdingPlayersEncoded?.Equals(value) ?? (value == null)))
				{
					_holdingPlayersEncoded = value;
					_dirtyMask |= 4;
				}
			}
		}

		public bool HasFsmPaused
		{
			get
			{
				return _hasFsmPaused;
			}
			set
			{
				if (!_hasFsmPaused.Equals(value))
				{
					_hasFsmPaused = value;
					_dirtyMask |= 8;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_guid);
			writer.Put(_unitPath);
			writer.Put(_holdingPlayersEncoded);
			writer.Put(_hasFsmPaused);
		}

		public void Deserialize(NetDataReader reader)
		{
			Guid = reader.GetString();
			UnitPath = reader.GetString();
			HoldingPlayersEncoded = reader.GetString();
			HasFsmPaused = reader.GetBool();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_guid);
			}
			if ((dirtyMask & 2) != 0)
			{
				writer.Put(_unitPath);
			}
			if ((dirtyMask & 4) != 0)
			{
				writer.Put(_holdingPlayersEncoded);
			}
			if ((dirtyMask & 8) != 0)
			{
				writer.Put(_hasFsmPaused);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				Guid = reader.GetString();
			}
			if ((num & 2) != 0)
			{
				UnitPath = reader.GetString();
			}
			if ((num & 4) != 0)
			{
				HoldingPlayersEncoded = reader.GetString();
			}
			if ((num & 8) != 0)
			{
				HasFsmPaused = reader.GetBool();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				reader.GetString();
			}
			if ((num & 2) != 0)
			{
				reader.GetString();
			}
			if ((num & 4) != 0)
			{
				reader.GetString();
			}
			if ((num & 8) != 0)
			{
				reader.GetBool();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct TeamComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private int _teamId;

		private byte _dirtyMask;

		public int TeamId
		{
			get
			{
				return _teamId;
			}
			set
			{
				if (!_teamId.Equals(value))
				{
					_teamId = value;
					_dirtyMask |= 1;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(_teamId);
		}

		public void Deserialize(NetDataReader reader)
		{
			TeamId = reader.GetInt();
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				writer.Put(_teamId);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				TeamId = reader.GetInt();
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			if ((reader.GetByte() & 1) != 0)
			{
				reader.GetInt();
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
	[StructLayout(LayoutKind.Auto)]
	[DeriveINetworkedComponent(SerializableMode.Default)]
	public struct TransformComponent : INetworkedComponent, IComponent, INetSerializable
	{
		private Vector3 _position;

		private Vector3 _rotation;

		private byte _dirtyMask;

		public Vector3 Position
		{
			get
			{
				return _position;
			}
			set
			{
				if (Vector3.DistanceSquared(_position, value) > 0.01f)
				{
					_position = value;
					_dirtyMask |= 1;
				}
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				if (Vector3.DistanceSquared(_rotation, value) > 0.01f)
				{
					_rotation = value;
					_dirtyMask |= 2;
				}
			}
		}

		public bool IsDirty => _dirtyMask != 0;

		public void Serialize(NetDataWriter writer)
		{
			_position.Serialize(writer);
			_rotation.Serialize(writer);
		}

		public void Deserialize(NetDataReader reader)
		{
			_position.Deserialize(reader);
			_dirtyMask |= 1;
			_rotation.Deserialize(reader);
			_dirtyMask |= 2;
		}

		public void WriteDelta(NetDataWriter writer)
		{
			byte dirtyMask = _dirtyMask;
			writer.Put(dirtyMask);
			if ((dirtyMask & 1) != 0)
			{
				_position.Serialize(writer);
			}
			if ((dirtyMask & 2) != 0)
			{
				_rotation.Serialize(writer);
			}
		}

		public void ReadDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				_position.Deserialize(reader);
				_dirtyMask |= 1;
			}
			if ((num & 2) != 0)
			{
				_rotation.Deserialize(reader);
				_dirtyMask |= 2;
			}
		}

		public void SkipDelta(NetDataReader reader)
		{
			byte num = reader.GetByte();
			if ((num & 1) != 0)
			{
				Vector3 vector = default(Vector3);
				vector.Deserialize(reader);
			}
			if ((num & 2) != 0)
			{
				Vector3 vector2 = default(Vector3);
				vector2.Deserialize(reader);
			}
		}

		public void ClearDirty()
		{
			_dirtyMask = 0;
		}
	}
}
