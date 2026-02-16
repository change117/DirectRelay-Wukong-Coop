using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using LiteNetLib;
using LiteNetLib.Layers;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Helpers;
using ReadyM.Api.Idents;
using ReadyM.Api.Multiplayer.Client;
using ReadyM.Api.Multiplayer.Client.Blobs;
using ReadyM.Api.Multiplayer.Common;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Managers;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.ECS.Values;
using ReadyM.Api.Multiplayer.Extensions;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Api.Multiplayer.Protocol;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.Api.Serialization;
using ReadyM.Relay.Client.ECS.Systems;
using ReadyM.Relay.Client.Shim;
using ReadyM.Relay.Client.State;
using ReadyM.Relay.Common.ECS.Archetypes;
using ReadyM.Relay.Common.ECS.Components;
using ReadyM.Relay.Common.ECS.Jobs;
using ReadyM.Relay.Common.ECS.Systems;
using ReadyM.Relay.Common.Extensions;
using ReadyM.Relay.Common.Serialization;
using ReadyM.Relay.Common.Shim;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("ReadyM.Relay.Client")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+63464565c8693c0537c1bd91e59ef593e8f0bde2")]
[assembly: AssemblyProduct("ReadyM.Relay.Client")]
[assembly: AssemblyTitle("ReadyM.Relay.Client")]
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
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class NotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public NotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
	internal sealed class NotNullAttribute : Attribute
	{
	}
}
namespace ReadyM.Relay.Client
{
	public class ClientEcsUpdateLoop : IClientEcsUpdateLoop
	{
		public readonly Store World;

		private readonly ILogger _logger;

		private readonly PendingActionUpdater<CommandBufferSynced> _scheduler;

		private float _applicationTime;

		public CommandBufferSynced CommandBuffer { get; }

		public PendingActionScheduler<CommandBufferSynced> Scheduler => _scheduler;

		public bool IsRunning { get; private set; }

		public event Action<CommandBufferSynced>? OnUpdateLoop;

		public event Action? OnStarted;

		public event Action? OnStopped;

		public ClientEcsUpdateLoop(Store world, ILogger logger)
		{
			World = world;
			CommandBuffer commandBuffer = World.GetCommandBuffer();
			commandBuffer.ReuseBuffer = true;
			CommandBuffer = commandBuffer.Synced;
			_logger = logger;
			_scheduler = new PendingActionUpdater<CommandBufferSynced>(CommandBuffer, logger);
		}

		public void Start()
		{
			if (IsRunning)
			{
				_logger.LogError("ECS update loop is already running");
				return;
			}
			IsRunning = true;
			_logger.LogInformation("Starting ECS update loop");
			_scheduler.SetThread(Thread.CurrentThread);
			this.OnStarted?.Invoke();
			_logger.LogInformation("ECS update loop started successfully");
		}

		public void Tick(float deltaTime)
		{
			if (!IsRunning)
			{
				_logger.LogError("ECS update loop is not running. Call `StartAsync()` first.");
				return;
			}
			try
			{
				CommandBuffer.Playback();
			}
			catch (InvalidOperationException exception)
			{
				_logger.LogError(exception, "Error during CommandBuffer playback");
			}
			_applicationTime += deltaTime;
			World.SystemRoot.Update(new UpdateTick(deltaTime, _applicationTime));
			_scheduler.Update();
			this.OnUpdateLoop?.Invoke(CommandBuffer);
		}

		public void Wait(Task task)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (!task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
			{
				Tick(stopwatch.ElapsedMilliseconds);
				stopwatch.Restart();
				Thread.Sleep(16);
			}
		}

		public void Stop()
		{
			if (!IsRunning)
			{
				_logger.LogError("ECS update loop is not running. Cannot stop.");
				return;
			}
			IsRunning = false;
			_scheduler.SetThread(null);
			this.OnStopped?.Invoke();
			_logger.LogInformation("ECS update loop stopped.");
		}

		public void AddSystem(BaseSystem system)
		{
			World.SystemRoot.Add(system);
		}

		public void AddSystem<T>() where T : BaseSystem, new()
		{
			World.SystemRoot.Add(new T());
		}

		public void RemoveSystem(BaseSystem system)
		{
			World.SystemRoot.Remove(system);
		}
	}
	public class ClientNetworkedStateSynchronizer : IDisposable
	{
		private class RegisterSystemCallback(ClientNetworkedStateSynchronizer owner) : INetworkedComponentRegistryCallback, IComponentRegistryCallbackBase<INetworkedComponentRegistry, INetworkedComponent>
		{
			public void AcceptComponent<T>(INetworkedComponentRegistry registry, T defaultValue = default(T)) where T : struct, INetworkedComponent
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				NetworkedComponentId networkedComponentId = registry.GetNetworkedComponentId<T>();
				DeliveryMethod networkedComponentDeliveryMethod = registry.GetNetworkedComponentDeliveryMethod<T>();
				owner.Logger.LogDebug("Registering client send for: {ComponentType} with ID {Id}", typeof(T).Name, networkedComponentId);
				owner._systemGroup.Add(new ClientSendComponentDeltaSystem<T>(networkedComponentId, networkedComponentDeliveryMethod, owner.RelayClient));
			}
		}

		protected readonly ClientState State;

		protected readonly NetworkedEntityManager NetEntity;

		protected readonly IRelayClient RelayClient;

		protected readonly ILogger Logger;

		protected readonly JobRegistry JobRegistry;

		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly ClientOwnershipManager _ownershipManager;

		private readonly SystemGroup _systemGroup;

		[ThreadStatic]
		private static int _skipEcsEventMessages;

		protected IClientEcsUpdateLoop EcsLoop => _ecsLoop;

		public ClientNetworkedStateSynchronizer(NetworkedEntityManager netEntity, ClientState state, JobRegistry jobRegistry, INetworkedComponentRegistry netComponentRegistry, IRelayClient relayClient, IClientEcsUpdateLoop ecsLoop, ClientOwnershipManager ownershipManager, ILogger logger)
		{
			State = state;
			_ecsLoop = ecsLoop;
			_ownershipManager = ownershipManager;
			NetEntity = netEntity;
			RelayClient = relayClient;
			Logger = logger;
			JobRegistry = jobRegistry;
			RelayClient.AddBuiltInMessageHandler(RelayMessageCode.EcsSnapshot, OnEcsSnapshotMessageHandler);
			RelayClient.AddBuiltInMessageHandler(RelayMessageCode.EcsDelta, OnEcsDeltaMessageHandler);
			RelayClient.AddBuiltInMessageHandler(RelayMessageCode.EcsCreateEntity, OnEcsCreateEntityMessageHandler);
			RelayClient.AddBuiltInMessageHandler(RelayMessageCode.EcsDeleteEntity, OnEcsDeleteEntityMessageHandler);
			RelayClient.AddBuiltInMessageHandler(RelayMessageCode.EcsChangeOwnership, OnEcsChangeOwnershipMessageHandler);
			NetEntity.OnEntityDelete += OnEntityDeleteHandler;
			_systemGroup = new SystemGroup("Network");
			_systemGroup.Add(new ClientSendEntityCreatedSystem(jobRegistry, state, relayClient));
			netComponentRegistry.Accept(new RegisterSystemCallback(this));
			_systemGroup.SetMonitorPerf(enable: true);
			_ecsLoop.AddSystem(_systemGroup);
		}

		public void Dispose()
		{
			OnDispose();
		}

		protected virtual void OnDispose()
		{
			_ecsLoop.RemoveSystem(_systemGroup);
			RelayClient.RemoveBuiltInMessageHandler(RelayMessageCode.EcsDeleteEntity, OnEcsDeleteEntityMessageHandler);
			RelayClient.RemoveBuiltInMessageHandler(RelayMessageCode.EcsCreateEntity, OnEcsCreateEntityMessageHandler);
			RelayClient.RemoveBuiltInMessageHandler(RelayMessageCode.EcsDelta, OnEcsDeltaMessageHandler);
			RelayClient.RemoveBuiltInMessageHandler(RelayMessageCode.EcsSnapshot, OnEcsSnapshotMessageHandler);
			RelayClient.RemoveBuiltInMessageHandler(RelayMessageCode.EcsChangeOwnership, OnEcsChangeOwnershipMessageHandler);
			NetEntity.OnEntityDelete -= OnEntityDeleteHandler;
		}

		protected virtual void OnOwnershipChanged(Entity entity)
		{
		}

		protected void OnEcsSnapshotMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced commandBufferSynced, ClientNetworkedStateSynchronizer self, NetDataReader readerCopy)
			{
				try
				{
					_skipEcsEventMessages++;
					NetworkId networkId = readerCopy.Get<NetworkId>();
					Entity? entity = null;
					uint uInt = readerCopy.GetUInt();
					for (int i = 0; i < uInt; i++)
					{
						MetadataComponent meta = readerCopy.Get<MetadataComponent>();
						if (!self.NetEntity.TryGetEntityByNetworkId(meta.NetId, out var _))
						{
							self.NetEntity.CreateRemoteNetworkedEntity(meta, entity);
						}
						else
						{
							self.Logger.LogError("Received snapshot create event for already existing entity: {Id} scope: {Scope}", meta.NetId, networkId);
						}
						if (i == 0 && networkId != default(NetworkId))
						{
							self.Logger.LogInformation("Looking up scope entity with NetId {ScopeNetId}", networkId);
							if (!self.NetEntity.TryGetEntityByNetworkId(networkId, out entity))
							{
								throw new InvalidOperationException($"Scope entity with NetId {networkId} not found");
							}
						}
					}
					self.JobRegistry.ApplySnapshot(readerCopy);
				}
				finally
				{
					_skipEcsEventMessages--;
				}
			}, this, _ecsLoop.Scheduler.MakeSafe(reader));
		}

		protected void OnEcsChangeOwnershipMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced commandBufferSynced, ClientNetworkedStateSynchronizer self, NetDataReader readerCopy)
			{
				try
				{
					_skipEcsEventMessages++;
					PlayerId owner = readerCopy.Get<PlayerId>();
					NetworkId result;
					while (readerCopy.TryGetNetworkId(out result))
					{
						if (self.NetEntity.TryGetEntityByNetworkId(result, out var entity))
						{
							entity.Value.GetComponent<MetadataComponent>().Owner = owner;
							self.OnOwnershipChanged(entity.Value);
						}
						else
						{
							self.Logger.LogWarning("Received ownership transfer event for non-existent entity: {Id}", result);
						}
					}
				}
				finally
				{
					_skipEcsEventMessages--;
				}
			}, this, _ecsLoop.Scheduler.MakeSafe(reader));
		}

		protected void OnEcsDeltaMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientNetworkedStateSynchronizer self, NetDataReader readerCopy)
			{
				try
				{
					_skipEcsEventMessages++;
					self.JobRegistry.ApplyDelta(readerCopy);
				}
				finally
				{
					_skipEcsEventMessages--;
				}
			}, this, _ecsLoop.Scheduler.MakeSafe(reader));
		}

		protected void OnEcsCreateEntityMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced cb, ClientNetworkedStateSynchronizer self, NetDataReader readerCopy)
			{
				try
				{
					_skipEcsEventMessages++;
					NetworkId networkId = readerCopy.Get<NetworkId>();
					Entity? entity = null;
					if (networkId != default(NetworkId) && !self.NetEntity.TryGetEntityByNetworkId(networkId, out entity))
					{
						throw new InvalidOperationException($"Scope entity with NetId {networkId} not found");
					}
					uint uInt = readerCopy.GetUInt();
					for (int i = 0; i < uInt; i++)
					{
						MetadataComponent meta = readerCopy.Get<MetadataComponent>();
						if (!self.NetEntity.TryGetEntityByNetworkId(meta.NetId, out var _))
						{
							self.NetEntity.CreateRemoteNetworkedEntity(meta, entity);
						}
						else
						{
							self.Logger.LogError("Received create event for already existing entity: {Id}", meta.NetId);
						}
					}
					self.JobRegistry.ApplySnapshot(readerCopy);
				}
				finally
				{
					_skipEcsEventMessages--;
				}
			}, this, _ecsLoop.Scheduler.MakeSafe(reader));
		}

		protected void OnEcsDeleteEntityMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			NetworkId arg = reader.Get<NetworkId>();
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced cb, ClientNetworkedStateSynchronizer self, NetworkId netId0)
			{
				try
				{
					_skipEcsEventMessages++;
					if (self.NetEntity.TryGetEntityByNetworkId(netId0, out var entity))
					{
						self.Logger.LogDebug("Deleting remote entity: {Id}", netId0);
						cb.DeleteEntity(entity.Value.Id);
					}
					else
					{
						self.Logger.LogWarning("Received destroy event for locally non-existent entity: {Id}", netId0);
					}
				}
				finally
				{
					_skipEcsEventMessages--;
				}
			}, this, arg);
		}

		protected void OnEntityDeleteHandler(NetworkId netId, Entity entity)
		{
			if (_skipEcsEventMessages <= 0)
			{
				_ecsLoop.Scheduler.EnsureThread();
				if (_ownershipManager.OwnsEntity(netId))
				{
					Logger.LogDebug("Networked entity destroyed: {NetworkId} (owned)", netId);
					RelayClient.SendMessageToServer<NetworkId>(RelayMessageCode.EcsDeleteEntity, netId, (DeliveryMethod)2);
				}
			}
		}
	}
	public class HotSwappableRelayClient : IRelayClient, IPlayerIdProvider, IDisposable
	{
		private IRelayClient? _client;

		private readonly Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>?[] _serverMessageHandlers = new Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>[251];

		private readonly Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>?[] _clientMessageHandlers = new Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>[150];

		public bool IsAttached => _client != null;

		public PlayerId? PlayerId => _client?.PlayerId;

		public bool RequestedConnect => _client?.RequestedConnect ?? false;

		public AreaId? RequestedAreaId => _client?.RequestedAreaId;

		public PendingActionScheduler<IRelayClientNetworkThreadContext> Scheduler => _client.Scheduler;

		public event Action<IRelayClient>? OnRelayClientAttach;

		public event Action<IRelayClient>? OnRelayClientDetach;

		public event Action? OnStart;

		public event Action? OnRequestedStop;

		public event Action? OnRequestedConnect;

		public event Action<IRelayClientNetworkThreadContext, PlayerId, uint>? OnConnected;

		public event Action? OnRequestedDisconnect;

		public event Action<IRelayClientNetworkThreadContext, DisconnectReason>? OnDisconnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerConnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerDisconnected;

		public event Action<AreaId>? OnRequestedJoinArea;

		public event Action<IRelayClientNetworkThreadContext, AreaId>? OnJoinedArea;

		public event Action? OnRequestedLeaveArea;

		public event Action<IRelayClientNetworkThreadContext>? OnLeftArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerJoinedArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerLeftArea;

		public event Action<IRelayClientNetworkThreadContext, int>? OnPingUpdated;

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyBuiltInMessage
		{
			add
			{
				AddBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
			remove
			{
				RemoveBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyServerRpcMessage
		{
			add
			{
				AddServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
			remove
			{
				RemoveServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>? OnAnyClientRpcMessage
		{
			add
			{
				AddClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
			remove
			{
				RemoveClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext>? OnClientUpdate;

		public void Attach(IRelayClient client)
		{
			if (_client != null)
			{
				if (_client.RequestedConnect)
				{
					throw new InvalidOperationException("Cannot swap RelayClient while it is connected. Please stop the client first.");
				}
				this.OnRelayClientDetach?.Invoke(_client);
				DetachRelayClient(_client);
			}
			_client = client;
			AttachRelayClient(_client);
			this.OnRelayClientAttach?.Invoke(_client);
		}

		public void Detach()
		{
			if (_client != null)
			{
				if (_client.RequestedConnect)
				{
					throw new InvalidOperationException("Cannot swap RelayClient while it is connected. Please stop the client first.");
				}
				this.OnRelayClientDetach?.Invoke(_client);
				DetachRelayClient(_client);
			}
			_client = null;
		}

		private void AttachRelayClient(IRelayClient client)
		{
			client.OnStart += OnRequestedStartHandler;
			client.OnRequestedStop += OnRequestedRequestedStopHandler;
			client.OnRequestedConnect += OnRequestedConnectHandler;
			client.OnConnected += OnConnectedHandler;
			client.OnRequestedDisconnect += OnRequestedDisconnectHandler;
			client.OnDisconnected += OnDisconnectedHandler;
			client.OnOtherPlayerConnected += OnOtherPlayerConnectedHandler;
			client.OnOtherPlayerDisconnected += OnOtherPlayerDisconnectedHandler;
			client.OnRequestedJoinArea += OnRequestedJoinAreaHandler;
			client.OnJoinedArea += OnJoinedAreaHandler;
			client.OnRequestedLeaveArea += OnRequestedLeaveAreaHandler;
			client.OnLeftArea += OnLeftAreaHandler;
			client.OnOtherPlayerJoinedArea += OnOtherPlayerJoinedAreaHandler;
			client.OnOtherPlayerLeftArea += OnOtherPlayerLeftAreaHandler;
			client.OnPingUpdated += OnPingUpdatedHandler;
			client.OnAnyBuiltInMessage += OnAnyBuiltInMessageHandler;
			client.OnAnyServerRpcMessage += OnAnyServerRpcMessageHandler;
			client.OnAnyClientRpcMessage += OnAnyClientRpcMessageHandler;
			client.OnClientUpdate += OnClientUpdateHandler;
		}

		private void DetachRelayClient(IRelayClient client)
		{
			client.OnClientUpdate -= OnClientUpdateHandler;
			client.OnAnyClientRpcMessage -= OnAnyClientRpcMessageHandler;
			client.OnAnyServerRpcMessage -= OnAnyServerRpcMessageHandler;
			client.OnAnyBuiltInMessage -= OnAnyBuiltInMessageHandler;
			client.OnPingUpdated -= OnPingUpdatedHandler;
			client.OnOtherPlayerLeftArea -= OnOtherPlayerLeftAreaHandler;
			client.OnOtherPlayerJoinedArea -= OnOtherPlayerJoinedAreaHandler;
			client.OnLeftArea -= OnLeftAreaHandler;
			client.OnRequestedLeaveArea -= OnRequestedLeaveAreaHandler;
			client.OnJoinedArea -= OnJoinedAreaHandler;
			client.OnRequestedJoinArea -= OnRequestedJoinAreaHandler;
			client.OnOtherPlayerDisconnected -= OnOtherPlayerDisconnectedHandler;
			client.OnOtherPlayerConnected -= OnOtherPlayerConnectedHandler;
			client.OnDisconnected -= OnDisconnectedHandler;
			client.OnRequestedDisconnect -= OnRequestedDisconnectHandler;
			client.OnConnected -= OnConnectedHandler;
			client.OnRequestedConnect -= OnRequestedConnectHandler;
			client.OnRequestedStop -= OnRequestedRequestedStopHandler;
			client.OnStart -= OnRequestedStartHandler;
		}

		public void Dispose()
		{
		}

		public void AddBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void AddClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public int GetMaxPacketSize(DeliveryMethod deliveryMethod)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return _client?.GetMaxPacketSize(deliveryMethod) ?? 1300;
		}

		public void Start()
		{
			_client.Start();
		}

		public Task RunAsync(CancellationToken token)
		{
			return _client.RunAsync(token);
		}

		public void Stop()
		{
			_client.Stop();
		}

		public void RequestConnect()
		{
			_client.RequestConnect();
		}

		public void RequestDisconnect()
		{
			_client.RequestDisconnect();
		}

		public void RequestReconnect()
		{
			_client.RequestReconnect();
		}

		public void RequestJoinArea(AreaId areaId)
		{
			_client.RequestJoinArea(areaId);
		}

		public void RequestLeaveArea()
		{
			_client.RequestLeaveArea();
		}

		public void SendRawMessage(NetDataWriter writer, DeliveryMethod deliveryMethod)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_client.SendRawMessage(writer, deliveryMethod);
		}

		public void SendMessage(RelayMessage message)
		{
			_client.SendMessage(message);
		}

		public void SendMessageToServer<T>(RelayMessageCode eventCode, T data, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_client.SendMessageToServer(eventCode, data, deliveryMethod);
		}

		public void SendMessageToPeers<T>(RelayMessageCode eventCode, T data, PlayerId[] peers, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_client.SendMessageToPeers(eventCode, data, peers, deliveryMethod);
		}

		public void SendMessageRelayMode<T>(RelayMessageCode eventCode, T data, RelayMode mode, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_client.SendMessageRelayMode(eventCode, data, mode, deliveryMethod);
		}

		public void LogEventStats()
		{
			_client?.LogEventStats();
		}

		private void OnRequestedStartHandler()
		{
			this.OnStart?.Invoke();
		}

		private void OnRequestedRequestedStopHandler()
		{
			this.OnRequestedStop?.Invoke();
		}

		private void OnRequestedConnectHandler()
		{
			this.OnRequestedConnect?.Invoke();
		}

		private void OnConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId, uint nextId)
		{
			this.OnConnected?.Invoke(context, playerId, nextId);
		}

		private void OnRequestedDisconnectHandler()
		{
			this.OnRequestedDisconnect?.Invoke();
		}

		private void OnDisconnectedHandler(IRelayClientNetworkThreadContext context, DisconnectReason disconnectReason)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			this.OnDisconnected?.Invoke(context, disconnectReason);
		}

		private void OnOtherPlayerConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			this.OnOtherPlayerConnected?.Invoke(context, playerId);
		}

		private void OnOtherPlayerDisconnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			this.OnOtherPlayerDisconnected?.Invoke(context, playerId);
		}

		private void OnRequestedJoinAreaHandler(AreaId areaId)
		{
			this.OnRequestedJoinArea?.Invoke(areaId);
		}

		private void OnJoinedAreaHandler(IRelayClientNetworkThreadContext context, AreaId areaId)
		{
			this.OnJoinedArea?.Invoke(context, areaId);
		}

		private void OnRequestedLeaveAreaHandler()
		{
			this.OnRequestedLeaveArea?.Invoke();
		}

		private void OnLeftAreaHandler(IRelayClientNetworkThreadContext context)
		{
			this.OnLeftArea?.Invoke(context);
		}

		private void OnOtherPlayerJoinedAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			this.OnOtherPlayerJoinedArea?.Invoke(context, playerId);
		}

		private void OnOtherPlayerLeftAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			this.OnOtherPlayerLeftArea?.Invoke(context, playerId);
		}

		private void OnPingUpdatedHandler(IRelayClientNetworkThreadContext context, int ping)
		{
			this.OnPingUpdated?.Invoke(context, ping);
		}

		private void OnAnyBuiltInMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action = _serverMessageHandlers[(uint)header.EventCode];
			if (action != null)
			{
				int position = reader.Position;
				Delegate[] invocationList = action.GetInvocationList();
				foreach (Delegate obj in invocationList)
				{
					reader.SetPosition(position);
					((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj)(context, header, reader);
				}
			}
		}

		private void OnAnyServerRpcMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action = _serverMessageHandlers[(uint)header.EventCode];
			if (action != null)
			{
				int position = reader.Position;
				Delegate[] invocationList = action.GetInvocationList();
				foreach (Delegate obj in invocationList)
				{
					reader.SetPosition(position);
					((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj)(context, header, reader);
				}
			}
		}

		private void OnAnyClientRpcMessageHandler(IRelayClientNetworkThreadContext context, CustomRelayEventHeader header, NetDataReader reader)
		{
			Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> action = _clientMessageHandlers[(uint)header.EventCode];
			if (action != null)
			{
				int position = reader.Position;
				Delegate[] invocationList = action.GetInvocationList();
				foreach (Delegate obj in invocationList)
				{
					reader.SetPosition(position);
					((Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)obj)(context, header, reader);
				}
			}
		}

		private void OnClientUpdateHandler(IRelayClientNetworkThreadContext context)
		{
			this.OnClientUpdate?.Invoke(context);
		}
	}
	public interface IClientEcsUpdateLoop
	{
		PendingActionScheduler<CommandBufferSynced> Scheduler { get; }

		CommandBufferSynced CommandBuffer { get; }

		bool IsRunning { get; }

		event Action? OnStarted;

		event Action? OnStopped;

		event Action<CommandBufferSynced>? OnUpdateLoop;

		void Start();

		void Stop();

		void AddSystem(BaseSystem system);

		void AddSystem<T>() where T : BaseSystem, new();

		void RemoveSystem(BaseSystem system);

		void Tick(float deltaTime);

		void Wait(Task task);
	}
	public class RelayClient : IRelayClient, IPlayerIdProvider, IDisposable
	{
		private class NetworkThreadContext : IRelayClientNetworkThreadContext
		{
			public readonly List<PlayerId> AllPlayers = new List<PlayerId>();

			public readonly List<PlayerId> AreaPlayers = new List<PlayerId>();

			public bool IsConnected { get; set; }

			public PlayerId? PlayerId { get; set; }

			public AreaId? CurrentAreaId { get; set; }

			public DisconnectReason LastDisconnectReason { get; set; }

			ReadyM.Api.Helpers.ReadOnlyList<PlayerId> IRelayClientNetworkThreadContext.AllPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(AllPlayers);

			ReadyM.Api.Helpers.ReadOnlyList<PlayerId> IRelayClientNetworkThreadContext.AreaPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(AreaPlayers);
		}

		private readonly ILogger _logger;

		private readonly NetManager _client;

		private readonly EventBasedNetListener _listener;

		private readonly RelayConnectionOptions _options;

		private readonly string _host;

		private readonly int _port;

		private readonly Random _rng = new Random(2137);

		private readonly NetworkThreadContext _netThreadContext = new NetworkThreadContext();

		private readonly PendingActionUpdater<IRelayClientNetworkThreadContext> _scheduler;

		private volatile bool _isRunning;

		private readonly ManualResetEventSlim _playerIdAssignedEvent = new ManualResetEventSlim();

		private readonly Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>?[] _serverMessageHandlers = new Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>[251];

		private readonly Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>?[] _clientMessageHandlers = new Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>[150];

		private readonly ConcurrentDictionary<RelayMessageCode, (long Count, long Bytes)> _statsSent = new ConcurrentDictionary<RelayMessageCode, (long, long)>();

		private readonly ConcurrentDictionary<RelayMessageCode, (long Count, long Bytes)> _statsRecv = new ConcurrentDictionary<RelayMessageCode, (long, long)>();

		private readonly object _statLock = new object();

		private long _lastBytesReceived;

		private long _lastBytesSent;

		private DateTimeOffset _lastStatCheck = DateTimeOffset.Now;

		public PlayerId? PlayerId
		{
			get
			{
				if (!RequestedConnect)
				{
					return null;
				}
				return _netThreadContext.PlayerId;
			}
		}

		public bool RequestedConnect { get; private set; }

		public AreaId? RequestedAreaId { get; private set; }

		public NetPeer? Server => _client.FirstPeer;

		public PendingActionScheduler<IRelayClientNetworkThreadContext> Scheduler => _scheduler;

		public event Action? OnStart;

		public event Action? OnRequestedStop;

		public event Action? OnRequestedConnect;

		public event Action<IRelayClientNetworkThreadContext, PlayerId, uint>? OnConnected;

		public event Action? OnRequestedDisconnect;

		public event Action<IRelayClientNetworkThreadContext, DisconnectReason>? OnDisconnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerConnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerDisconnected;

		public event Action<AreaId>? OnRequestedJoinArea;

		public event Action<IRelayClientNetworkThreadContext, AreaId>? OnJoinedArea;

		public event Action? OnRequestedLeaveArea;

		public event Action<IRelayClientNetworkThreadContext>? OnLeftArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerJoinedArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerLeftArea;

		public event Action<IRelayClientNetworkThreadContext, int>? OnPingUpdated;

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyBuiltInMessage
		{
			add
			{
				AddBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
			remove
			{
				RemoveBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyServerRpcMessage
		{
			add
			{
				AddServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
			remove
			{
				RemoveServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>? OnAnyClientRpcMessage
		{
			add
			{
				AddClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
			remove
			{
				RemoveClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext>? OnClientUpdate;

		public void AddBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void AddClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public RelayClient(string host, int port, RelayConnectionOptions options, ILogger logger, bool noDisconnect)
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			_logger = logger;
			_options = options;
			_host = host;
			_port = port;
			_scheduler = new PendingActionUpdater<IRelayClientNetworkThreadContext>(_netThreadContext, _logger);
			_listener = new EventBasedNetListener();
			_listener.NetworkReceiveEvent += new OnNetworkReceive(OnListenerNetworkReceiveEvent);
			_listener.NetworkLatencyUpdateEvent += new OnNetworkLatencyUpdate(OnNetworkLatencyUpdateEvent);
			_listener.PeerDisconnectedEvent += new OnPeerDisconnected(OnPeerDisconnectedEvent);
			_client = new NetManager((INetEventListener)(object)_listener, (PacketLayerBase)null)
			{
				AutoRecycle = true,
				EnableStatistics = true,
				UnsyncedEvents = true
			};
			if (noDisconnect)
			{
				_client.DisconnectTimeout = 3600000;
				_client.DisconnectOnUnreachable = false;
			}
			else
			{
				_client.DisconnectTimeout = 5000;
				_client.DisconnectOnUnreachable = true;
			}
		}

		public void Dispose()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (_isRunning)
			{
				Stop();
			}
			_listener.PeerDisconnectedEvent -= new OnPeerDisconnected(OnPeerDisconnectedEvent);
			_listener.NetworkLatencyUpdateEvent -= new OnNetworkLatencyUpdate(OnNetworkLatencyUpdateEvent);
			_listener.NetworkReceiveEvent -= new OnNetworkReceive(OnListenerNetworkReceiveEvent);
		}

		public int GetMaxPacketSize(DeliveryMethod deliveryMethod)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			NetPeer? server = Server;
			if (server == null)
			{
				return 1300;
			}
			return server.GetMaxSinglePacketSize(deliveryMethod);
		}

		public void Start()
		{
			if (_isRunning)
			{
				_logger.LogWarning("Relay client is already running");
				return;
			}
			_isRunning = true;
			this.OnStart?.Invoke();
			_logger.LogDebug("Starting on {Host}:{Port}...", _host, _port);
			_client.Start();
			_logger.LogDebug("Started on {Host}:{Port}", _host, _port);
		}

		public async Task RunAsync(CancellationToken token)
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running");
				return;
			}
			await Task.Yield();
			_scheduler.SetThread(Thread.CurrentThread);
			while (!token.IsCancellationRequested)
			{
				try
				{
					_client.PollEvents(0);
					this.OnClientUpdate?.Invoke(_netThreadContext);
					bool num = _scheduler.Update();
					_client.TriggerUpdate();
					if (!num)
					{
						await Task.Delay(2, token);
					}
				}
				catch (OperationCanceledException exception)
				{
					_logger.LogWarning(exception, "Client loop was cancelled");
				}
				catch (Exception exception2)
				{
					_logger.LogError(exception2, "Unhandled exception in client thread");
				}
			}
		}

		public void Stop()
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Invalid comparison between Unknown and I4
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (!_isRunning)
			{
				_logger.LogWarning("Relay client is not running");
				return;
			}
			_isRunning = false;
			_scheduler.SetThread(null);
			_logger.LogDebug("Stopping on {Host}:{Port}...", _host, _port);
			this.OnRequestedStop?.Invoke();
			_client.DisconnectAll();
			_client.PollEvents(0);
			_client.Stop();
			if ((int)_netThreadContext.LastDisconnectReason != 5)
			{
				_logger.LogWarning("Already disconnected: {Reason}", _netThreadContext.LastDisconnectReason);
			}
			_logger.LogDebug("Stopped on {Host}:{Port}", _host, _port);
		}

		public void RequestConnect()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			if (RequestedConnect)
			{
				_logger.LogWarning("Relay client is already connecting");
				return;
			}
			RequestedConnect = true;
			_logger.LogInformation("Connecting on {Host}:{Port}...", _host, _port);
			this.OnRequestedConnect?.Invoke();
			NetDataWriter val = new NetDataWriter();
			_options.Serialize(val);
			_client.Connect(_host, _port, val);
			_playerIdAssignedEvent.Wait(5000);
			if (!_netThreadContext.PlayerId.HasValue)
			{
				_logger.LogError("Failed to assign PlayerId within {Timeout} ms", 5000);
				this.OnDisconnected?.Invoke(_netThreadContext, _netThreadContext.LastDisconnectReason);
			}
		}

		public void RequestDisconnect()
		{
			if (!RequestedConnect)
			{
				_logger.LogWarning("Relay client is already disconnecting");
				return;
			}
			RequestedConnect = false;
			_logger.LogInformation("Explicitly disconnecting from {Host}:{Port}", _host, _port);
			this.OnRequestedDisconnect?.Invoke();
			_client.DisconnectAll();
			_playerIdAssignedEvent.Reset();
		}

		public void RequestReconnect()
		{
			RequestDisconnect();
			RequestConnect();
		}

		public void RequestJoinArea(AreaId areaId)
		{
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			if (!RequestedConnect)
			{
				_logger.LogError("Relay client is not connected to the server");
				return;
			}
			if (RequestedAreaId.HasValue)
			{
				_logger.LogWarning("Already requested to join a different area {AreaId}", RequestedAreaId.Value);
				RequestLeaveArea();
			}
			if (RequestedAreaId == areaId)
			{
				_logger.LogWarning("Already requested to join area {AreaId}", areaId);
				return;
			}
			RequestedAreaId = areaId;
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				_logger.LogError("PlayerId cannot be null");
				return;
			}
			NetDataWriter val = new NetDataWriter();
			val.Put((byte)254);
			val.Put<PlayerId>(playerId.Value);
			val.Put(true);
			areaId.Serialize(val);
			SendRawMessage(val, (DeliveryMethod)2);
		}

		public void RequestLeaveArea()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			if (!RequestedConnect)
			{
				_logger.LogError("Relay client is not connected to the server");
				return;
			}
			if (!RequestedAreaId.HasValue)
			{
				_logger.LogWarning("Already requested to leave area");
				return;
			}
			RequestedAreaId = null;
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				_logger.LogError("PlayerId cannot be null");
				return;
			}
			NetDataWriter val = new NetDataWriter();
			val.Put((byte)254);
			val.Put<PlayerId>(playerId.Value);
			val.Put(false);
			SendRawMessage(val, (DeliveryMethod)2);
		}

		public void SendRawMessage(NetDataWriter writer, DeliveryMethod deliveryMethod)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Server?.SendImmediately(writer, deliveryMethod);
			byte ev = writer.Data[0];
			AppendToSentStats((RelayMessageCode)ev, writer.Length);
		}

		public void SendMessage(RelayMessage message)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Server?.SendImmediately(message.Writer, message.DeliveryMethod);
			AppendToSentStats(message.EventCode, message.Writer.Length);
		}

		public void SendMessageToServer<T>(RelayMessageCode eventCode, T data, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			RelayMessage message = RelayMessage.ToServer(eventCode, deliveryMethod);
			((INetSerializable)data/*cast due to .constrained prefix*/).Serialize(message.Writer);
			SendMessage(message);
		}

		public void SendMessageToPeers<T>(RelayMessageCode eventCode, T data, PlayerId[] peers, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				_logger.LogError("PlayerId cannot be null");
				return;
			}
			RelayMessage message = RelayMessage.ToPeers(eventCode, playerId.Value, peers, deliveryMethod);
			((INetSerializable)data/*cast due to .constrained prefix*/).Serialize(message.Writer);
			SendMessage(message);
		}

		public void SendMessageRelayMode<T>(RelayMessageCode eventCode, T data, RelayMode mode, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				_logger.LogError("PlayerId cannot be null");
				return;
			}
			RelayMessage message = RelayMessage.ByRelayMode(eventCode, playerId.Value, mode, deliveryMethod);
			SendMessage(message);
		}

		private void AppendToSentStats(RelayMessageCode ev, long bytesSent)
		{
			_statsSent.AddOrUpdate(ev, (1L, bytesSent), (RelayMessageCode _, (long Count, long Bytes) data) => (Count: data.Count + 1, Bytes: data.Bytes + bytesSent));
		}

		private void AppendToRecvStats(RelayMessageCode ev, long bytesRecv)
		{
			_statsRecv.AddOrUpdate(ev, (1L, bytesRecv), (RelayMessageCode _, (long Count, long Bytes) data) => (Count: data.Count + 1, Bytes: data.Bytes + bytesRecv));
		}

		private void OnListenerNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliverymethod)
		{
			RelayMessageCode relayMessageCode = (RelayMessageCode)((NetDataReader)reader).GetByte();
			AppendToRecvStats(relayMessageCode, ((NetDataReader)reader).UserDataSize);
			switch (relayMessageCode)
			{
			case RelayMessageCode.HandshakeConnected:
			{
				PlayerId playerId4 = ((NetDataReader)reader).Get<PlayerId>();
				uint uInt = ((NetDataReader)reader).GetUInt();
				if (_netThreadContext.PlayerId.HasValue && _netThreadContext.PlayerId != playerId4)
				{
					_logger.LogError("Missing handshake for player {PlayerId} but already assigned {AssignedPlayerId}", playerId4, _netThreadContext.PlayerId);
				}
				_netThreadContext.PlayerId = playerId4;
				_netThreadContext.AllPlayers.Add(playerId4);
				_netThreadContext.IsConnected = true;
				_playerIdAssignedEvent.Set();
				int num = ((NetDataReader)reader).GetInt();
				for (int j = 0; j < num; j++)
				{
					PlayerId playerId5 = ((NetDataReader)reader).Get<PlayerId>();
					if (!_netThreadContext.AllPlayers.Contains(playerId5))
					{
						_netThreadContext.AllPlayers.Add(playerId5);
						continue;
					}
					_logger.LogError("Received handshake for player {PlayerId} that already is marked as connected", playerId5);
				}
				_logger.LogInformation("Assigned Actor ID {PlayerId}", playerId4);
				_logger.LogDebug("Next available NetworkId is {NextNetworkId}", uInt);
				this.OnConnected?.Invoke(_netThreadContext, playerId4, uInt);
				return;
			}
			case RelayMessageCode.AreaEvent:
			{
				PlayerId playerId = ((NetDataReader)reader).Get<PlayerId>();
				if (((NetDataReader)reader).GetBool())
				{
					if (!_netThreadContext.PlayerId.HasValue)
					{
						_logger.LogError("Received handshake for joining area {AreaId} by player {PlayerId} but PlayerId is not set", playerId, _netThreadContext.PlayerId);
						return;
					}
					PlayerId value = playerId;
					PlayerId? playerId2 = PlayerId;
					if (value != playerId2)
					{
						_logger.LogError("Received handshake for player {PlayerId} but expected {ExpectedPlayerId}", playerId, PlayerId);
						return;
					}
					if (_netThreadContext.CurrentAreaId.HasValue)
					{
						_logger.LogError("Received handshake for joining area {AreaId} by player {PlayerId} but already in area {CurrentArea}", playerId, _netThreadContext.PlayerId, _netThreadContext.CurrentAreaId);
						return;
					}
					AreaId areaId = ((NetDataReader)reader).Get<AreaId>();
					_logger.LogInformation("NETWORK JOINING {AreaId} by player {PlayerId}", areaId, playerId);
					_netThreadContext.CurrentAreaId = areaId;
					_netThreadContext.AreaPlayers.Clear();
					_netThreadContext.AreaPlayers.Add(playerId);
					ushort uShort = ((NetDataReader)reader).GetUShort();
					for (int i = 0; i < uShort; i++)
					{
						PlayerId item = ((NetDataReader)reader).Get<PlayerId>();
						_netThreadContext.AreaPlayers.Add(item);
					}
					this.OnJoinedArea?.Invoke(_netThreadContext, areaId);
				}
				else if (!_netThreadContext.PlayerId.HasValue)
				{
					_logger.LogError("Received handshake for leaving area by player {PlayerId} but PlayerId is not set", playerId);
				}
				else
				{
					PlayerId value = playerId;
					PlayerId? playerId2 = PlayerId;
					if (value != playerId2)
					{
						_logger.LogError("Received handshake for player {PlayerId} but expected {ExpectedPlayerId}", playerId, PlayerId);
						return;
					}
					if (!_netThreadContext.CurrentAreaId.HasValue)
					{
						_logger.LogError("Received handshake for leaving area by player {PlayerId} but not in any area", playerId);
						return;
					}
					this.OnLeftArea?.Invoke(_netThreadContext);
					_logger.LogInformation("NETWORK LEAVING {AreaId} by player {PlayerId}", _netThreadContext.CurrentAreaId, playerId);
					_netThreadContext.CurrentAreaId = null;
					_netThreadContext.AreaPlayers.Remove(playerId);
				}
				return;
			}
			case RelayMessageCode.OtherPlayerConnectionEvent:
			{
				PlayerId playerId3 = ((NetDataReader)reader).Get<PlayerId>();
				if (((NetDataReader)reader).GetBool())
				{
					if (!_netThreadContext.AllPlayers.Contains(playerId3))
					{
						_netThreadContext.AllPlayers.Add(playerId3);
						this.OnOtherPlayerConnected?.Invoke(_netThreadContext, playerId3);
					}
					else
					{
						_logger.LogError("Player connected event for player {PlayerId} that already is marked as connected", playerId3);
					}
				}
				else if (_netThreadContext.AllPlayers.Contains(playerId3))
				{
					if (_netThreadContext.AreaPlayers.Contains(playerId3))
					{
						_logger.LogInformation("Player disconnected event for player {PlayerId} that is still in the area", playerId3);
						_netThreadContext.AreaPlayers.Remove(playerId3);
					}
					this.OnOtherPlayerDisconnected?.Invoke(_netThreadContext, playerId3);
					_netThreadContext.AllPlayers.Remove(playerId3);
				}
				else
				{
					_logger.LogError("Player disconnected event for player {PlayerId} that already is marked as NOT connected", playerId3);
				}
				return;
			}
			case RelayMessageCode.OtherPlayerAreaEvent:
			{
				PlayerId playerId6 = ((NetDataReader)reader).Get<PlayerId>();
				if (((NetDataReader)reader).GetBool())
				{
					if (!_netThreadContext.CurrentAreaId.HasValue)
					{
						_logger.LogError("Received area event for player {PlayerId} but current area is not set", playerId6);
					}
					else if (!_netThreadContext.AreaPlayers.Contains(playerId6))
					{
						_netThreadContext.AreaPlayers.Add(playerId6);
						this.OnOtherPlayerJoinedArea?.Invoke(_netThreadContext, playerId6);
					}
					else
					{
						_logger.LogError("Player joined area event for player {PlayerId} that already is marked as in the area", playerId6);
					}
				}
				else if (_netThreadContext.AreaPlayers.Contains(playerId6))
				{
					this.OnOtherPlayerLeftArea?.Invoke(_netThreadContext, playerId6);
					_netThreadContext.AreaPlayers.Remove(playerId6);
				}
				else
				{
					_logger.LogError("Player left area event for player {PlayerId} that already is marked as NOT in the area", playerId6);
				}
				return;
			}
			}
			if ((int)relayMessageCode >= 242)
			{
				ServerEventHeader arg = new ServerEventHeader(relayMessageCode, ReadyM.Api.Multiplayer.Idents.PlayerId.Server);
				Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action = _serverMessageHandlers[(uint)relayMessageCode];
				if (action != null)
				{
					int position = ((NetDataReader)reader).Position;
					Delegate[] invocationList = action.GetInvocationList();
					foreach (Delegate obj in invocationList)
					{
						((NetDataReader)reader).SetPosition(position);
						((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj)(_netThreadContext, arg, (NetDataReader)(object)reader);
					}
				}
				return;
			}
			if ((int)relayMessageCode >= 150)
			{
				ServerEventHeader arg2 = new ServerEventHeader(relayMessageCode, ReadyM.Api.Multiplayer.Idents.PlayerId.Server);
				Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action2 = _serverMessageHandlers[(uint)relayMessageCode];
				if (action2 != null)
				{
					int position2 = ((NetDataReader)reader).Position;
					Delegate[] invocationList = action2.GetInvocationList();
					foreach (Delegate obj2 in invocationList)
					{
						((NetDataReader)reader).SetPosition(position2);
						((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj2)(_netThreadContext, arg2, (NetDataReader)(object)reader);
					}
				}
				return;
			}
			CustomRelayEventHeader customRelayEventHeader = ((NetDataReader)(object)reader).GetCustomRelayEventHeader(relayMessageCode);
			Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> action3 = _clientMessageHandlers[(uint)relayMessageCode];
			if (action3 != null)
			{
				int position3 = ((NetDataReader)reader).Position;
				Delegate[] invocationList = action3.GetInvocationList();
				foreach (Delegate obj3 in invocationList)
				{
					((NetDataReader)reader).SetPosition(position3);
					((Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)obj3)(_netThreadContext, customRelayEventHeader, (NetDataReader)(object)reader);
				}
			}
		}

		private void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
		{
			this.OnPingUpdated?.Invoke(_netThreadContext, 2 * latency + _rng.Next(2));
			long bytesReceived = _client.Statistics.BytesReceived;
			long bytesSent = _client.Statistics.BytesSent;
			long num;
			long num2;
			TimeSpan timeSpan;
			lock (_statLock)
			{
				num = bytesReceived - _lastBytesReceived;
				_lastBytesReceived = bytesReceived;
				num2 = bytesSent - _lastBytesSent;
				_lastBytesSent = bytesSent;
				DateTimeOffset now = DateTimeOffset.Now;
				timeSpan = now - _lastStatCheck;
				_lastStatCheck = now;
			}
			long num3 = (long)((double)num / timeSpan.TotalSeconds);
			long num4 = (long)((double)num2 / timeSpan.TotalSeconds);
			long packetLoss = _client.Statistics.PacketLoss;
			long packetsSent = _client.Statistics.PacketsSent;
			_logger.LogDebug("Avg recv: {Recv} B/s, Avg sent: {Sent} B/s, Lost: {Loss} / Sent: {SentPackets}", num3, num4, packetLoss, packetsSent);
		}

		private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo info)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			_logger.LogInformation("Disconnected from server: {Reason}", info.Reason);
			_netThreadContext.CurrentAreaId = null;
			_netThreadContext.IsConnected = false;
			_netThreadContext.AllPlayers.Clear();
			_netThreadContext.AreaPlayers.Clear();
			_netThreadContext.LastDisconnectReason = info.Reason;
			this.OnDisconnected?.Invoke(_netThreadContext, info.Reason);
		}

		public void LogEventStats()
		{
			foreach (KeyValuePair<RelayMessageCode, (long, long)> item in _statsSent.OrderByDescending((KeyValuePair<RelayMessageCode, (long Count, long Bytes)> x) => x.Value))
			{
				_logger.LogDebug("Event {Event}: sent {Bytes} B, avg {Average} B", item.Key, item.Value.Item2, item.Value.Item2 / item.Value.Item1);
			}
			_logger.LogTrace("----------------------------------------");
			foreach (KeyValuePair<RelayMessageCode, (long, long)> item2 in _statsRecv.OrderByDescending((KeyValuePair<RelayMessageCode, (long Count, long Bytes)> x) => x.Value))
			{
				_logger.LogDebug("Event {Event}: recv {Bytes} B, avg {Average} B", item2.Key, item2.Value.Item2, item2.Value.Item2 / item2.Value.Item1);
			}
		}
	}
}
namespace ReadyM.Relay.Client.State
{
	public class ClientNetworkedEntityState(NetworkedEntityManager netEntity, ClientState state, ILogger logger)
	{
		public (Entity Entity, NetworkId NetId) CreateNetworkedGlobalEntity(ArchetypeId archetypeId, Action<EntityBuilder>? setComponents = null)
		{
			return netEntity.CreateNetworkedEntity(archetypeId, null, setComponents);
		}

		public (Entity Entity, NetworkId NetId) CreateNetworkedAreaEntity(ArchetypeId archetypeId, Action<EntityBuilder>? setComponents = null)
		{
			if (state.CurrentAreaEntity.HasValue)
			{
				Entity value = state.CurrentAreaEntity.Value;
				return netEntity.CreateNetworkedEntity(archetypeId, value, setComponents);
			}
			logger.LogError("Attempted to create a networked entity in area but no area is set.");
			return (Entity: default(Entity), NetId: default(NetworkId));
		}

		public (Entity Entity, NetworkId NetId) CreateNetworkedPlayerEntity(ArchetypeId archetypeId, Action<EntityBuilder>? setComponents = null)
		{
			if (state.LocalPlayerEntity.HasValue)
			{
				Entity value = state.LocalPlayerEntity.Value;
				return netEntity.CreateNetworkedEntity(archetypeId, value, setComponents);
			}
			logger.LogError("Attempted to create a networked entity for player but no player entity is set.");
			return (Entity: default(Entity), NetId: default(NetworkId));
		}

		public bool TryGetEntityByNetworkId(NetworkId netId, [NotNullWhen(true)] out Entity? entity)
		{
			return netEntity.TryGetEntityByNetworkId(netId, out entity);
		}
	}
	public class ClientOwnershipManager(ClientState state, NetworkedOwnershipManager ownership)
	{
		public bool TryGetOwner(NetworkId netId, out PlayerId ownerId)
		{
			return ownership.TryGetOwner(netId, out ownerId);
		}

		public bool TryGetOwner(Entity entity, out PlayerId ownerId)
		{
			return ownership.TryGetOwner(entity, out ownerId);
		}

		public bool OwnsEntity(NetworkId netId)
		{
			if (ownership.TryGetOwner(netId, out var ownerId))
			{
				PlayerId value = ownerId;
				PlayerId? localPlayerId = state.LocalPlayerId;
				return value == localPlayerId;
			}
			return false;
		}

		public bool OwnsEntity(Entity entity)
		{
			if (ownership.TryGetOwner(entity, out var ownerId))
			{
				PlayerId value = ownerId;
				PlayerId? localPlayerId = state.LocalPlayerId;
				return value == localPlayerId;
			}
			return false;
		}
	}
	public class ClientState : IDisposable
	{
		private enum PendingEventKind
		{
			Connected,
			Disconnected,
			JoinedArea,
			LeftArea,
			OtherPlayerCreated,
			OtherPlayerDeleted,
			OtherPlayerInsideArea,
			OtherPlayerOutsideArea
		}

		private struct PendingEvent
		{
			public bool Invalidated;

			public PendingEventKind Kind;

			public AreaId AreaId;

			public PlayerId PlayerId;

			public bool IsNotify;

			public DisconnectReason DisconnectReason;
		}

		public struct AreaEntry
		{
			public AreaId AreaId { get; internal set; }

			public Entity AreaEntity { get; internal set; }

			public NetworkId AreaNetworkId { get; internal set; }

			public List<PlayerId> AreaPlayers { get; internal set; }
		}

		public struct PlayerEntry
		{
			public PlayerId PlayerId { get; internal set; }

			public Entity PlayerEntity { get; internal set; }

			public NetworkId PlayerNetworkId { get; internal set; }

			public AreaId? CurrentAreaId { get; internal set; }
		}

		private readonly Store _world;

		private readonly NetworkedEntityManager _netEntity;

		private readonly IRelayClient _relayClient;

		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly JobRegistry _jobRegistry;

		private readonly ILogger _logger;

		private readonly List<PlayerId> _allPlayers = new List<PlayerId>();

		private readonly Dictionary<PlayerId, PlayerEntry> _playerEntries = new Dictionary<PlayerId, PlayerEntry>();

		private PlayerEntry? _localPlayerEntry;

		private AreaEntry? _currentAreaEntry;

		private readonly object _lock = new object();

		private readonly List<PendingEvent> _pendingEvents = new List<PendingEvent>();

		public bool IsConnected => _localPlayerEntry.HasValue;

		public PlayerId? LocalPlayerId => _localPlayerEntry?.PlayerId;

		public PlayerEntry? LocalPlayerEntry => _localPlayerEntry;

		public Entity? LocalPlayerEntity => _localPlayerEntry?.PlayerEntity;

		public ReadyM.Api.Helpers.ReadOnlyList<PlayerId> AllPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(_allPlayers);

		public ReadyM.Api.Helpers.ReadOnlyList<PlayerId> OtherPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(_allPlayers.Where(delegate(PlayerId p)
		{
			PlayerId? localPlayerId = LocalPlayerId;
			return p != localPlayerId;
		}).ToList());

		public System.Collections.ObjectModel.ReadOnlyDictionary<PlayerId, PlayerEntry> PlayerEntries => new System.Collections.ObjectModel.ReadOnlyDictionary<PlayerId, PlayerEntry>(_playerEntries);

		public ReadyM.Api.Helpers.ReadOnlyList<PlayerId> AreaPlayers => (_currentAreaEntry?.AreaPlayers).NullableWrapReadOnly();

		public ReadyM.Api.Helpers.ReadOnlyList<PlayerId> OtherAreaPlayers => (_currentAreaEntry?.AreaPlayers.Where(delegate(PlayerId p)
		{
			PlayerId? localPlayerId = LocalPlayerId;
			return p != localPlayerId;
		}).ToList()).NullableWrapReadOnly();

		public bool JoinedArea
		{
			get
			{
				AreaEntry? currentAreaEntry = _currentAreaEntry;
				if (currentAreaEntry.HasValue)
				{
					return currentAreaEntry.GetValueOrDefault().AreaId != AreaId.Invalid;
				}
				return false;
			}
		}

		public AreaId? CurrentAreaId => _currentAreaEntry?.AreaId;

		public AreaEntry? CurrentAreaEntry => _currentAreaEntry;

		public Entity? CurrentAreaEntity => _currentAreaEntry?.AreaEntity;

		public ArchetypeId AreaArchetype { get; }

		public ArchetypeId PlayerArchetype { get; }

		public event Action<PlayerId, Entity>? OnConnected;

		public event Action<PlayerId, Entity?, DisconnectReason>? OnDisconnected;

		public event Action<PlayerId, Entity, OtherPlayerCreatedReason>? OnOtherPlayerCreated;

		public event Action<PlayerId, Entity, OtherPlayerDeletedReason>? OnOtherPlayerDeleted;

		public event Action<AreaId, Entity>? OnJoinedArea;

		public event Action<AreaId, Entity>? OnLeftArea;

		public event Action<PlayerId, AreaId, OtherPlayerInsideAreaReason>? OnOtherPlayerInsideArea;

		public event Action<PlayerId, AreaId, OtherPlayerOutsideAreaReason>? OnOtherPlayerOutsideArea;

		public ClientState(Store world, NetworkedEntityManager netEntity, IRelayClient relayClient, IClientEcsUpdateLoop ecsLoop, JobRegistry jobRegistry, DefaultAreaArchetypeRegistration areaArchetype, DefaultPlayerArchetypeRegistration playerArchetype, ILogger logger)
		{
			_world = world;
			_netEntity = netEntity;
			_relayClient = relayClient;
			_ecsLoop = ecsLoop;
			_jobRegistry = jobRegistry;
			_logger = logger;
			AreaArchetype = areaArchetype.AreaArchetype;
			PlayerArchetype = playerArchetype.PlayerArchetype;
			_ecsLoop.OnUpdateLoop += ProcessPendingEvents;
			_relayClient.OnConnected += OnConnectedHandler;
			_relayClient.OnDisconnected += OnDisconnectedHandler;
			_relayClient.OnJoinedArea += OnJoinedAreaHandler;
			_relayClient.OnLeftArea += OnLeftAreaHandler;
			_relayClient.OnOtherPlayerConnected += OnOtherPlayerConnectedHandler;
			_relayClient.OnOtherPlayerDisconnected += OnOtherPlayerDisconnectedHandler;
			_relayClient.OnOtherPlayerJoinedArea += OnOtherPlayerJoinedAreaHandler;
			_relayClient.OnOtherPlayerLeftArea += OnOtherPlayerLeftAreaHandler;
			_jobRegistry.OnApplySnapshot += OnApplySnapshotHandler;
		}

		public void Dispose()
		{
			_ecsLoop.OnUpdateLoop -= ProcessPendingEvents;
			_jobRegistry.OnApplySnapshot -= OnApplySnapshotHandler;
			_relayClient.OnOtherPlayerLeftArea -= OnOtherPlayerLeftAreaHandler;
			_relayClient.OnOtherPlayerJoinedArea -= OnOtherPlayerJoinedAreaHandler;
			_relayClient.OnOtherPlayerDisconnected -= OnOtherPlayerDisconnectedHandler;
			_relayClient.OnOtherPlayerConnected -= OnOtherPlayerConnectedHandler;
			_relayClient.OnLeftArea -= OnLeftAreaHandler;
			_relayClient.OnJoinedArea -= OnJoinedAreaHandler;
			_relayClient.OnDisconnected -= OnDisconnectedHandler;
			_relayClient.OnConnected -= OnConnectedHandler;
		}

		private void OnConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId, uint nextId)
		{
			_netEntity.SetNextNetworkedId(nextId);
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.Connected,
					PlayerId = playerId
				});
				foreach (PlayerId allPlayer in context.AllPlayers)
				{
					if (!(allPlayer == playerId))
					{
						_pendingEvents.Add(new PendingEvent
						{
							Kind = PendingEventKind.OtherPlayerCreated,
							PlayerId = allPlayer,
							IsNotify = true
						});
					}
				}
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnDisconnectedHandler(IRelayClientNetworkThreadContext context, DisconnectReason disconnectReason)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.Disconnected,
					PlayerId = (context.PlayerId ?? PlayerId.Invalid),
					DisconnectReason = disconnectReason
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnJoinedAreaHandler(IRelayClientNetworkThreadContext context, AreaId areaId)
		{
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.JoinedArea,
					AreaId = areaId,
					PlayerId = context.PlayerId.Value
				});
				foreach (PlayerId areaPlayer in context.AreaPlayers)
				{
					PlayerId value = areaPlayer;
					PlayerId? playerId = context.PlayerId;
					if (!(value == playerId))
					{
						_pendingEvents.Add(new PendingEvent
						{
							Kind = PendingEventKind.OtherPlayerInsideArea,
							AreaId = areaId,
							PlayerId = areaPlayer,
							IsNotify = true
						});
					}
				}
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnLeftAreaHandler(IRelayClientNetworkThreadContext context)
		{
			lock (_lock)
			{
				AreaId? currentAreaId = context.CurrentAreaId;
				if (!currentAreaId.HasValue)
				{
					_logger.LogError("LeftArea event received, but no current area. This should not happen.");
					return;
				}
				PlayerId? playerId = context.PlayerId;
				if (!playerId.HasValue)
				{
					_logger.LogError("LeftArea event received, but no player ID. This should not happen.");
					return;
				}
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.LeftArea,
					AreaId = currentAreaId.Value,
					PlayerId = playerId.Value
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnOtherPlayerConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.OtherPlayerCreated,
					PlayerId = playerId
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnOtherPlayerDisconnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.OtherPlayerDeleted,
					PlayerId = playerId
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnOtherPlayerJoinedAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			lock (_lock)
			{
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.OtherPlayerInsideArea,
					AreaId = context.CurrentAreaId.Value,
					PlayerId = playerId
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnOtherPlayerLeftAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			lock (_lock)
			{
				AreaId? currentAreaId = context.CurrentAreaId;
				if (!currentAreaId.HasValue)
				{
					_logger.LogError("OtherPlayerLeftArea event received, but no current area. This should not happen.");
					return;
				}
				_pendingEvents.Add(new PendingEvent
				{
					Kind = PendingEventKind.OtherPlayerOutsideArea,
					AreaId = currentAreaId.Value,
					PlayerId = playerId
				});
			}
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void OnApplySnapshotHandler()
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, ClientState self)
			{
				self.ProcessPendingEvents();
			}, this);
		}

		private void PrunePendingEvents()
		{
			int? num = null;
			PlayerId? playerId = _localPlayerEntry?.PlayerId;
			int? num2 = null;
			AreaId? areaId = _currentAreaEntry?.AreaId;
			Dictionary<PlayerId, int> dictionary = new Dictionary<PlayerId, int>();
			Dictionary<PlayerId, int> dictionary2 = new Dictionary<PlayerId, int>();
			for (int i = 0; i < _pendingEvents.Count; i++)
			{
				PendingEvent value = _pendingEvents[i];
				if (value.Invalidated)
				{
					continue;
				}
				switch (value.Kind)
				{
				case PendingEventKind.Connected:
					if (playerId == value.PlayerId)
					{
						value.Invalidated = true;
						break;
					}
					if (num.HasValue)
					{
						InvalidateRange(num.Value, i - 1);
					}
					num = i;
					playerId = value.PlayerId;
					break;
				case PendingEventKind.Disconnected:
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (num.HasValue)
					{
						InvalidateRange(num.Value, i);
						value.Invalidated = true;
					}
					num = null;
					playerId = null;
					num2 = null;
					areaId = null;
					dictionary.Clear();
					dictionary2.Clear();
					break;
				case PendingEventKind.JoinedArea:
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (areaId == value.AreaId)
					{
						value.Invalidated = true;
						break;
					}
					if (num2.HasValue)
					{
						InvalidateRange(num2.Value, i - 1, IsAreaEvent);
					}
					num2 = i;
					areaId = value.AreaId;
					break;
				case PendingEventKind.LeftArea:
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (num2.HasValue)
					{
						InvalidateRange(num2.Value, i, IsAreaEvent);
						value.Invalidated = true;
					}
					num2 = null;
					areaId = null;
					dictionary2.Clear();
					break;
				case PendingEventKind.OtherPlayerCreated:
				{
					int value4;
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
					}
					else if (dictionary.TryGetValue(value.PlayerId, out value4))
					{
						value.Invalidated = true;
					}
					else
					{
						dictionary.Add(value.PlayerId, i);
					}
					break;
				}
				case PendingEventKind.OtherPlayerDeleted:
				{
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (dictionary.TryGetValue(value.PlayerId, out var value3))
					{
						InvalidateOne(value3);
						value.Invalidated = true;
					}
					dictionary.Remove(value.PlayerId);
					dictionary2.Remove(value.PlayerId);
					break;
				}
				case PendingEventKind.OtherPlayerInsideArea:
				{
					int value5;
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
					}
					else if (!areaId.HasValue)
					{
						value.Invalidated = true;
					}
					else if (dictionary2.TryGetValue(value.PlayerId, out value5))
					{
						value.Invalidated = true;
					}
					else
					{
						dictionary2.Add(value.PlayerId, i);
					}
					break;
				}
				case PendingEventKind.OtherPlayerOutsideArea:
				{
					if (!playerId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (!areaId.HasValue)
					{
						value.Invalidated = true;
						break;
					}
					if (dictionary2.TryGetValue(value.PlayerId, out var value2))
					{
						InvalidateOne(value2);
						value.Invalidated = true;
					}
					dictionary2.Remove(value.PlayerId);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
				_pendingEvents[i] = value;
			}
			void InvalidateOne(int index)
			{
				PendingEvent value6 = _pendingEvents[index];
				value6.Invalidated = true;
				_pendingEvents[index] = value6;
			}
			void InvalidateRange(int fromIndex, int toIndex, Func<PendingEvent, bool>? predicate = null)
			{
				for (int j = fromIndex; j <= toIndex; j++)
				{
					PendingEvent pendingEvent = _pendingEvents[j];
					pendingEvent.Invalidated = predicate?.Invoke(pendingEvent) ?? true;
					_pendingEvents[j] = pendingEvent;
				}
			}
			static bool IsAreaEvent(PendingEvent p)
			{
				switch (p.Kind)
				{
				case PendingEventKind.Connected:
				case PendingEventKind.Disconnected:
				case PendingEventKind.OtherPlayerCreated:
				case PendingEventKind.OtherPlayerDeleted:
					return false;
				case PendingEventKind.JoinedArea:
				case PendingEventKind.LeftArea:
				case PendingEventKind.OtherPlayerInsideArea:
				case PendingEventKind.OtherPlayerOutsideArea:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void ProcessPendingEvents(CommandBufferSynced _)
		{
			ProcessPendingEvents();
		}

		private void ProcessPendingEvents()
		{
			bool flag = false;
			while (true)
			{
				PendingEvent pendingEvent;
				lock (_lock)
				{
					if (flag)
					{
						_pendingEvents.RemoveAt(0);
					}
					PrunePendingEvents();
					if (_pendingEvents.Count == 0)
					{
						break;
					}
					pendingEvent = _pendingEvents[0];
				}
				if (pendingEvent.Invalidated)
				{
					flag = true;
					continue;
				}
				flag = ProcessPendingEvent(pendingEvent);
				if (flag)
				{
					continue;
				}
				break;
			}
		}

		private bool ProcessPendingEvent(PendingEvent pendingEvent)
		{
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			switch (pendingEvent.Kind)
			{
			case PendingEventKind.Connected:
			{
				if (_localPlayerEntry.HasValue)
				{
					_logger.LogError("Connected event received, but local player entry already exists. This should not happen.");
					_localPlayerEntry = null;
				}
				PlayerId playerId11 = pendingEvent.PlayerId;
				ArchetypeQuery<PlayerScopeComponent, MetadataComponent> archetypeQuery4 = _world.Query<PlayerScopeComponent, MetadataComponent>().HasValue<PlayerScopeComponent, PlayerId>(playerId11);
				if (archetypeQuery4.Count == 0)
				{
					return false;
				}
				Entity entity3 = archetypeQuery4.Entities.First();
				MetadataComponent component3 = entity3.GetComponent<MetadataComponent>();
				PlayerEntry value12 = new PlayerEntry
				{
					PlayerId = playerId11,
					PlayerEntity = entity3,
					PlayerNetworkId = component3.NetId,
					CurrentAreaId = null
				};
				_localPlayerEntry = value12;
				_allPlayers.Add(playerId11);
				_playerEntries.Add(playerId11, value12);
				this.OnConnected?.Invoke(playerId11, entity3);
				break;
			}
			case PendingEventKind.Disconnected:
			{
				PlayerId playerId8 = pendingEvent.PlayerId;
				if (_currentAreaEntry.HasValue)
				{
					AreaId areaId5 = _currentAreaEntry.Value.AreaId;
					int num2 = 0;
					while (num2 < _currentAreaEntry.Value.AreaPlayers.Count)
					{
						PlayerId playerId9 = _currentAreaEntry.Value.AreaPlayers[num2];
						if (playerId9 == playerId8)
						{
							num2++;
							continue;
						}
						this.OnOtherPlayerOutsideArea?.Invoke(playerId9, areaId5, OtherPlayerOutsideAreaReason.NotifyBeforeSelfDisconnected);
						_currentAreaEntry.Value.AreaPlayers.RemoveAt(num2);
						PlayerEntry value10 = _playerEntries[playerId9];
						value10.CurrentAreaId = null;
						_playerEntries[playerId9] = value10;
					}
					this.OnLeftArea?.Invoke(areaId5, _currentAreaEntry.Value.AreaEntity);
					_currentAreaEntry.Value.AreaPlayers.Clear();
					PlayerEntry value11 = _playerEntries[playerId8];
					value11.CurrentAreaId = null;
					_playerEntries[playerId8] = value11;
					_netEntity.DeleteEntitiesInScope(_currentAreaEntry.Value.AreaEntity, skipSync: true, deleteScopeEntity: true);
					_currentAreaEntry = null;
				}
				int num3 = 0;
				while (num3 < _allPlayers.Count)
				{
					PlayerId playerId10 = _allPlayers[num3];
					if (playerId10 == playerId8)
					{
						num3++;
						continue;
					}
					PlayerEntry playerEntry = _playerEntries[playerId10];
					this.OnOtherPlayerDeleted?.Invoke(playerId10, playerEntry.PlayerEntity, OtherPlayerDeletedReason.NotifyBeforeSelfDisconnected);
					_allPlayers.RemoveAt(num3);
					_playerEntries.Remove(playerId10);
				}
				this.OnDisconnected?.Invoke(pendingEvent.PlayerId, _localPlayerEntry?.PlayerEntity, pendingEvent.DisconnectReason);
				_allPlayers.Clear();
				_playerEntries.Clear();
				_netEntity.DeleteAllNetworkedEntities(skipSync: true);
				_localPlayerEntry = null;
				break;
			}
			case PendingEventKind.JoinedArea:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("JoinedArea event received, but no local player entry found. This should not happen.");
					break;
				}
				if (_currentAreaEntry.HasValue)
				{
					_logger.LogError("JoinedArea event received, but already in an area. This should not happen.");
					_currentAreaEntry = null;
				}
				PlayerId playerId4 = pendingEvent.PlayerId;
				AreaId areaId3 = pendingEvent.AreaId;
				_logger.LogInformation("ECS JOINING (before query) {AreaId} by player {PlayerId}", areaId3, playerId4);
				ArchetypeQuery<AreaScopeComponent, MetadataComponent> archetypeQuery = _world.Query<AreaScopeComponent, MetadataComponent>().HasValue<AreaScopeComponent, AreaId>(areaId3);
				if (archetypeQuery.Count == 0)
				{
					return false;
				}
				Entity entity = archetypeQuery.Entities.First();
				if (entity.GetComponent<AreaScopeComponent>().MasterClient == PlayerId.Invalid)
				{
					return false;
				}
				MetadataComponent component = entity.GetComponent<MetadataComponent>();
				AreaEntry value4 = new AreaEntry
				{
					AreaId = areaId3,
					AreaEntity = entity,
					AreaNetworkId = component.NetId,
					AreaPlayers = new List<PlayerId>(1) { playerId4 }
				};
				_currentAreaEntry = value4;
				PlayerEntry value5 = _playerEntries[playerId4];
				value5.CurrentAreaId = areaId3;
				_playerEntries[playerId4] = value5;
				_localPlayerEntry = value5;
				_logger.LogInformation("ECS JOINING {AreaId} by player {PlayerId}", areaId3, playerId4);
				this.OnJoinedArea?.Invoke(areaId3, entity);
				break;
			}
			case PendingEventKind.LeftArea:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("LeftArea event received, but no local player entry found. This should not happen.");
					break;
				}
				if (!_currentAreaEntry.HasValue)
				{
					_logger.LogWarning("LeftArea event received, but no current area entry found. This happens when a player leaves before the JoinedArea event is processed.");
					break;
				}
				PlayerId playerId2 = pendingEvent.PlayerId;
				AreaId areaId2 = _currentAreaEntry.Value.AreaId;
				int num = 0;
				while (num < _currentAreaEntry.Value.AreaPlayers.Count)
				{
					PlayerId playerId3 = _currentAreaEntry.Value.AreaPlayers[num];
					if (playerId3 == playerId2)
					{
						num++;
						continue;
					}
					this.OnOtherPlayerOutsideArea?.Invoke(playerId3, areaId2, OtherPlayerOutsideAreaReason.NotifyBeforeSelfLeft);
					_currentAreaEntry.Value.AreaPlayers.RemoveAt(num);
					PlayerEntry value2 = _playerEntries[playerId3];
					value2.CurrentAreaId = null;
					_playerEntries[playerId3] = value2;
				}
				this.OnLeftArea?.Invoke(areaId2, _currentAreaEntry.Value.AreaEntity);
				_logger.LogInformation("ECS LEAVING {AreaId} by player {PlayerId}", areaId2, playerId2);
				_netEntity.DeleteEntitiesInScope(_currentAreaEntry.Value.AreaEntity, skipSync: true, deleteScopeEntity: true);
				_currentAreaEntry = null;
				PlayerEntry value3 = _playerEntries[playerId2];
				value3.CurrentAreaId = null;
				_playerEntries[playerId2] = value3;
				_localPlayerEntry = value3;
				break;
			}
			case PendingEventKind.OtherPlayerCreated:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("OtherPlayerCreated event received, but no local player entry found. This should not happen.");
					break;
				}
				PlayerId playerId6 = pendingEvent.PlayerId;
				if (_playerEntries.ContainsKey(playerId6))
				{
					_logger.LogError("OtherPlayerCreated event received for player {PlayerId}, but player entry already exists. This should not happen.", playerId6);
					_playerEntries.Remove(playerId6);
				}
				ArchetypeQuery<PlayerScopeComponent, MetadataComponent> archetypeQuery3 = _world.Query<PlayerScopeComponent, MetadataComponent>().HasValue<PlayerScopeComponent, PlayerId>(playerId6);
				_logger.LogInformation("ECS OTHER CONNECTED player {PlayerId} (before query)", playerId6);
				if (archetypeQuery3.Count == 0)
				{
					return false;
				}
				_logger.LogInformation("ECS OTHER CONNECTED player {PlayerId}", playerId6);
				Entity entity2 = archetypeQuery3.Entities.First();
				MetadataComponent component2 = entity2.GetComponent<MetadataComponent>();
				PlayerEntry value7 = new PlayerEntry
				{
					PlayerId = playerId6,
					PlayerEntity = entity2,
					PlayerNetworkId = component2.NetId,
					CurrentAreaId = null
				};
				_allPlayers.Add(playerId6);
				_playerEntries.Add(playerId6, value7);
				OtherPlayerCreatedReason arg2 = (pendingEvent.IsNotify ? OtherPlayerCreatedReason.NotifyAfterSelfConnected : OtherPlayerCreatedReason.OtherConnected);
				this.OnOtherPlayerCreated?.Invoke(playerId6, entity2, arg2);
				break;
			}
			case PendingEventKind.OtherPlayerDeleted:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("OtherPlayerDeleted event received, but no local player entry found. This should not happen.");
					break;
				}
				PlayerId playerId7 = pendingEvent.PlayerId;
				if (!_playerEntries.TryGetValue(playerId7, out var value8))
				{
					_logger.LogWarning("OtherPlayerDeleted event received for player {PlayerId}, but no player entry found. This happens when the other player disconnects before the OtherPlayerCreated event is processed.", playerId7);
					break;
				}
				if (value8.CurrentAreaId.HasValue && value8.CurrentAreaId == CurrentAreaId)
				{
					AreaId value9 = value8.CurrentAreaId.Value;
					this.OnOtherPlayerOutsideArea?.Invoke(playerId7, value9, OtherPlayerOutsideAreaReason.OtherDisconnected);
					_currentAreaEntry.Value.AreaPlayers.Remove(playerId7);
				}
				this.OnOtherPlayerDeleted?.Invoke(playerId7, value8.PlayerEntity, OtherPlayerDeletedReason.OtherDisconnected);
				_netEntity.DeleteEntitiesInScope(value8.PlayerEntity, skipSync: true, deleteScopeEntity: true);
				_allPlayers.Remove(playerId7);
				_playerEntries.Remove(playerId7);
				if (value8.CurrentAreaId.HasValue && value8.CurrentAreaId == CurrentAreaId)
				{
					_currentAreaEntry.Value.AreaPlayers.Remove(playerId7);
				}
				break;
			}
			case PendingEventKind.OtherPlayerInsideArea:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("OtherPlayerInsideArea event received, but no local player entry found. This should not happen.");
					break;
				}
				if (!_currentAreaEntry.HasValue)
				{
					_logger.LogError("OtherPlayerInsideArea event received, but no current area entry found. This should not happen.");
					break;
				}
				PlayerId playerId5 = pendingEvent.PlayerId;
				if (!_playerEntries.TryGetValue(playerId5, out var value6))
				{
					_logger.LogWarning("OtherPlayerInsideArea event received for player {PlayerId}, but no player entry found. This should not happen.", playerId5);
					break;
				}
				if (value6.CurrentAreaId.HasValue)
				{
					_logger.LogError("OtherPlayerInsideArea event received for player {PlayerId}, but player is already inside area {CurrentAreaId}. This should not happen.", playerId5, value6.CurrentAreaId);
					value6.CurrentAreaId = null;
					_playerEntries[playerId5] = value6;
				}
				AreaId areaId4 = pendingEvent.AreaId;
				if (_currentAreaEntry.Value.AreaId != areaId4)
				{
					_logger.LogWarning("Received OtherPlayerInsideArea event for player {PlayerId} in area {AreaId}, but current area is {CurrentAreaId}", playerId5, areaId4, _currentAreaEntry.Value.AreaId);
					break;
				}
				ArchetypeQuery<PlayerScopeComponent, MetadataComponent> archetypeQuery2 = _world.Query<PlayerScopeComponent, MetadataComponent>().HasValue<PlayerScopeComponent, PlayerId>(playerId5);
				_logger.LogInformation("ECS OTHER JOINING player {PlayerId} into area {AreaId} (before query)", playerId5, areaId4);
				if (archetypeQuery2.Count == 0)
				{
					return false;
				}
				if (_world.Query<AreaScopeComponent, MetadataComponent>().HasValue<AreaScopeComponent, AreaId>(areaId4).Count == 0)
				{
					return false;
				}
				_logger.LogInformation("ECS OTHER JOINING player {PlayerId} into area {AreaId}", playerId5, areaId4);
				_currentAreaEntry.Value.AreaPlayers.Add(playerId5);
				value6.CurrentAreaId = areaId4;
				_playerEntries[playerId5] = value6;
				OtherPlayerInsideAreaReason arg = (pendingEvent.IsNotify ? OtherPlayerInsideAreaReason.NotifyAfterSelfJoined : OtherPlayerInsideAreaReason.OtherJoined);
				this.OnOtherPlayerInsideArea?.Invoke(playerId5, areaId4, arg);
				break;
			}
			case PendingEventKind.OtherPlayerOutsideArea:
			{
				if (!_localPlayerEntry.HasValue)
				{
					_logger.LogError("OtherPlayerOutsideArea event received, but no local player entry found. This should not happen.");
					break;
				}
				if (!_currentAreaEntry.HasValue)
				{
					_logger.LogError("OtherPlayerOutsideArea event received, but no current area entry found. This should not happen.");
					break;
				}
				PlayerId playerId = pendingEvent.PlayerId;
				if (!_playerEntries.TryGetValue(playerId, out var value))
				{
					_logger.LogWarning("OtherPlayerOutsideArea event received for player {PlayerId}, but no player entry found. This happens when a player disconnects before the OtherPlayerOutsideArea event gets processed.", playerId);
					break;
				}
				if (!value.CurrentAreaId.HasValue)
				{
					_logger.LogWarning("OtherPlayerOutsideArea event received for player {PlayerId}, but player is already outside area. This happens when a player leaves before the OtherPlayerInsideArea event is processed.", playerId);
					break;
				}
				AreaId areaId = pendingEvent.AreaId;
				_currentAreaEntry.Value.AreaPlayers.Remove(playerId);
				value.CurrentAreaId = null;
				_playerEntries[playerId] = value;
				this.OnOtherPlayerOutsideArea?.Invoke(playerId, areaId, OtherPlayerOutsideAreaReason.OtherLeft);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("Kind", pendingEvent.Kind, null);
			}
			return true;
		}
	}
}
namespace ReadyM.Relay.Client.Shim
{
	public class BlobClientShimParserImpl : ShimBuiltInMessageParserImplBase<ShimBlobDependencyData>
	{
		public override bool SupportsRequest(ServerEventHeader header)
		{
			RelayMessageCode eventCode = header.EventCode;
			if (eventCode == RelayMessageCode.RequestUploadBlob || eventCode == RelayMessageCode.RequestDownloadBlob)
			{
				return true;
			}
			return false;
		}

		public override bool SupportsResponse(ServerEventHeader header)
		{
			RelayMessageCode eventCode = header.EventCode;
			if (eventCode == RelayMessageCode.UploadBlobAck || eventCode == RelayMessageCode.DownloadBlobData)
			{
				return true;
			}
			return false;
		}

		public override ShimBlobDependencyData GetBuiltInRequestCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int requestId = reader.GetInt();
			return new ShimBlobDependencyData
			{
				RequestId = requestId
			};
		}

		public override ShimBlobDependencyData GetBuiltInResponseCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int requestId = reader.GetInt();
			return new ShimBlobDependencyData
			{
				RequestId = requestId
			};
		}
	}
	public class BlobClientShimTrackerImpl : ShimDependencyTrackerImplBase<ShimBlobDependencyData>
	{
		public override bool Supports(ShimRequestItem requestItem, ShimBlobDependencyData dependencyData)
		{
			if (requestItem.Kind == ShimRequestKind.SentBuiltInMessage)
			{
				switch (requestItem.EventCode)
				{
				case RelayMessageCode.RequestUploadBlob:
				case RelayMessageCode.RequestDownloadBlob:
					return true;
				}
			}
			return false;
		}

		public override bool Supports(ShimResponseItem responseItem, ShimBlobDependencyData dependencyData)
		{
			if (responseItem.Kind == ShimResponseKind.AnyBuiltInMessage)
			{
				switch (responseItem.EventCode)
				{
				case RelayMessageCode.UploadBlobAck:
				case RelayMessageCode.DownloadBlobData:
					return true;
				}
			}
			return false;
		}

		public override bool CheckRequestHasResponse(ShimRequestItem requestItem, ShimResponseItem responseItem)
		{
			if (requestItem.EventCode == RelayMessageCode.RequestUploadBlob && responseItem.EventCode == RelayMessageCode.UploadBlobAck)
			{
				return requestItem.GetCustomData<ShimBlobDependencyData>().RequestId == responseItem.GetCustomData<ShimBlobDependencyData>().RequestId;
			}
			if (requestItem.EventCode == RelayMessageCode.RequestDownloadBlob && responseItem.EventCode == RelayMessageCode.DownloadBlobData)
			{
				return requestItem.GetCustomData<ShimBlobDependencyData>().RequestId == responseItem.GetCustomData<ShimBlobDependencyData>().RequestId;
			}
			return false;
		}

		public override bool CheckResponseShouldWait(ShimResponseItem responseItem, IRelayClientNetworkThreadContext context, IEnumerable<ShimRequestItem> requestItems)
		{
			foreach (ShimRequestItem requestItem in requestItems)
			{
				if (!(responseItem.CustomData is ShimBlobDependencyData shimBlobDependencyData) || requestItem.Kind != ShimRequestKind.SentBuiltInMessage || !(requestItem.CustomData is ShimBlobDependencyData shimBlobDependencyData2))
				{
					continue;
				}
				RelayMessageCode? eventCode = responseItem.EventCode;
				if (eventCode.HasValue && eventCode == RelayMessageCode.UploadBlobAck)
				{
					eventCode = requestItem.EventCode;
					if (eventCode.HasValue && eventCode == RelayMessageCode.RequestUploadBlob)
					{
						goto IL_00d8;
					}
				}
				eventCode = responseItem.EventCode;
				if (!eventCode.HasValue || eventCode != RelayMessageCode.DownloadBlobData)
				{
					continue;
				}
				eventCode = requestItem.EventCode;
				if (!eventCode.HasValue || eventCode != RelayMessageCode.RequestDownloadBlob)
				{
					continue;
				}
				goto IL_00d8;
				IL_00d8:
				if (shimBlobDependencyData.RequestId == shimBlobDependencyData2.RequestId)
				{
					return false;
				}
			}
			return true;
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ShimBlobDependencyData
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ShimBlobDependencyData>
		{
			public override ShimBlobDependencyData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ShimBlobDependencyData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public int RequestId;

		public ShimBlobDependencyData(int requestId)
		{
			RequestId = requestId;
		}

		public static ShimBlobDependencyData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			int requestId = 0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string? text = reader.GetString();
				reader.Read();
				if (text == "requestId")
				{
					requestId = reader.GetInt32();
				}
				else
				{
					reader.Skip();
				}
			}
			return new ShimBlobDependencyData(requestId);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ShimBlobDependencyData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("requestId", obj.RequestId);
			writer.WriteEndObject();
		}
	}
	public class ClientSynchronizerShimParserImpl(NetworkedEntityManager netEntity, ILogger logger) : ShimBuiltInMessageParserImplBase<ShimEcsDependencyData>()
	{
		public override bool SupportsRequest(ServerEventHeader header)
		{
			return false;
		}

		public override bool SupportsResponse(ServerEventHeader header)
		{
			RelayMessageCode eventCode = header.EventCode;
			if (eventCode - 247 <= (RelayMessageCode)3)
			{
				return true;
			}
			return false;
		}

		public override ShimEcsDependencyData GetBuiltInRequestCustomData(ServerEventHeader header, NetDataReader reader)
		{
			throw new NotSupportedException();
		}

		public override ShimEcsDependencyData GetBuiltInResponseCustomData(ServerEventHeader header, NetDataReader reader)
		{
			RelayMessageCode eventCode = header.EventCode;
			if (eventCode - 248 <= (RelayMessageCode)1)
			{
				NetworkId networkId = reader.Get<NetworkId>();
				if (networkId == default(NetworkId))
				{
					return default(ShimEcsDependencyData);
				}
				if (!netEntity.TryGetEntityByNetworkId(networkId, out var entity))
				{
					logger.LogError("Failed to find scope entity {NetworkId} in ECS {EventCode}", networkId, header.EventCode);
					return default(ShimEcsDependencyData);
				}
				return GetDataForScopeEntity(entity.Value);
			}
			if (header.EventCode == RelayMessageCode.EcsDelta)
			{
				reader.Get<NetworkedComponentId>();
				NetworkId networkId2 = reader.Get<NetworkId>();
				if (!netEntity.TryGetEntityByNetworkId(networkId2, out var entity2))
				{
					logger.LogWarning("Failed to find entity {NetworkId} in ECS {EventCode}", networkId2, header.EventCode);
					return default(ShimEcsDependencyData);
				}
				if (!entity2.Value.TryGetComponent<InScopeComponent>(out var result))
				{
					return default(ShimEcsDependencyData);
				}
				return GetDataForScopeEntity(result.ScopeEntity);
			}
			if (header.EventCode == RelayMessageCode.EcsDeleteEntity)
			{
				reader.Get<NetworkedComponentId>();
				NetworkId networkId3 = reader.Get<NetworkId>();
				if (!netEntity.TryGetEntityByNetworkId(networkId3, out var entity3))
				{
					logger.LogWarning("Failed to find entity {NetworkId} in ECS {EventCode}", networkId3, header.EventCode);
					return default(ShimEcsDependencyData);
				}
				if (!entity3.Value.TryGetComponent<InScopeComponent>(out var result2))
				{
					return default(ShimEcsDependencyData);
				}
				return GetDataForScopeEntity(result2.ScopeEntity);
			}
			logger.LogError("Unsupported event code {EventCode} for built-in response data", header.EventCode);
			return default(ShimEcsDependencyData);
			ShimEcsDependencyData GetDataForScopeEntity(Entity scopeEntity)
			{
				if (scopeEntity.TryGetComponent<AreaScopeComponent>(out var result3))
				{
					return new ShimEcsDependencyData
					{
						AreaId = result3.AreaId
					};
				}
				if (scopeEntity.TryGetComponent<PlayerScopeComponent>(out var result4))
				{
					return new ShimEcsDependencyData
					{
						PlayerId = result4.PlayerId
					};
				}
				logger.LogError("Entity {Id} is not a valid scope entity", scopeEntity);
				return default(ShimEcsDependencyData);
			}
		}
	}
	public class ClientSynchronizerShimTrackerImpl : ShimDependencyTrackerImplBase<ShimEcsDependencyData>
	{
		public override bool Supports(ShimRequestItem requestItem, ShimEcsDependencyData dependencyData)
		{
			if (requestItem.Kind == ShimRequestKind.SentBuiltInMessage)
			{
				switch (requestItem.EventCode)
				{
				case RelayMessageCode.EcsDeleteEntity:
				case RelayMessageCode.EcsCreateEntity:
				case RelayMessageCode.EcsDelta:
					return true;
				}
			}
			return false;
		}

		public override bool Supports(ShimResponseItem responseItem, ShimEcsDependencyData dependencyData)
		{
			if (responseItem.Kind == ShimResponseKind.AnyBuiltInMessage)
			{
				RelayMessageCode eventCode = responseItem.ServerHeader.EventCode;
				if (eventCode - 247 <= (RelayMessageCode)3)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CheckRequestHasResponse(ShimRequestItem requestItem, ShimResponseItem responseItem)
		{
			return true;
		}

		public override bool CheckResponseShouldWait(ShimResponseItem responseItem, IRelayClientNetworkThreadContext context, IEnumerable<ShimRequestItem> requestItems)
		{
			if (!context.IsConnected)
			{
				return true;
			}
			ShimEcsDependencyData customData = responseItem.GetCustomData<ShimEcsDependencyData>();
			if (customData.AreaId.HasValue && context.CurrentAreaId != customData.AreaId)
			{
				return true;
			}
			if (customData.PlayerId.HasValue && context.AllPlayers.Contains(customData.PlayerId.Value))
			{
				return true;
			}
			return false;
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ShimEcsDependencyData
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ShimEcsDependencyData>
		{
			public override ShimEcsDependencyData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ShimEcsDependencyData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		private AreaId _areaId;

		private PlayerId _playerId;

		public AreaId? AreaId
		{
			get
			{
				if (!(_areaId == ReadyM.Api.Multiplayer.Idents.AreaId.Invalid))
				{
					return _areaId;
				}
				return null;
			}
			set
			{
				_areaId = value ?? ReadyM.Api.Multiplayer.Idents.AreaId.Invalid;
			}
		}

		public PlayerId? PlayerId
		{
			get
			{
				if (!(_playerId == ReadyM.Api.Multiplayer.Idents.PlayerId.Invalid))
				{
					return _playerId;
				}
				return null;
			}
			set
			{
				_playerId = value ?? ReadyM.Api.Multiplayer.Idents.PlayerId.Invalid;
			}
		}

		public ShimEcsDependencyData(AreaId areaId, PlayerId playerId)
		{
			_areaId = areaId;
			_playerId = playerId;
		}

		public static ShimEcsDependencyData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			AreaId areaId = default(AreaId);
			PlayerId playerId = default(PlayerId);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "_areaId")
				{
					areaId = ReadyM.Api.Multiplayer.Idents.AreaId.TextDeserialize(ref reader, options);
				}
				else if (text == "_playerId")
				{
					playerId = ReadyM.Api.Multiplayer.Idents.PlayerId.TextDeserialize(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new ShimEcsDependencyData(areaId, playerId);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ShimEcsDependencyData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("_areaId");
			ReadyM.Api.Multiplayer.Idents.AreaId.TextSerialize(writer, obj._areaId, options);
			writer.WritePropertyName("_playerId");
			ReadyM.Api.Multiplayer.Idents.PlayerId.TextSerialize(writer, obj._playerId, options);
			writer.WriteEndObject();
		}
	}
	public interface IShimDependencyTrackerImpl
	{
		bool Supports(ShimRequestItem requestItem);

		bool Supports(ShimResponseItem responseItem);

		bool CheckRequestHasResponse(ShimRequestItem requestItem, ShimResponseItem responseItem);

		bool CheckResponseShouldWait(ShimResponseItem responseItem, IRelayClientNetworkThreadContext context, IEnumerable<ShimRequestItem> requestItems);
	}
	public interface IShimRelayMessageParserImpl
	{
		bool SupportsRequest(ServerEventHeader header);

		bool SupportsRequest(CustomRelayEventHeader header);

		bool SupportsResponse(ServerEventHeader header);

		bool SupportsResponse(CustomRelayEventHeader header);

		object? GetBuiltInRequestCustomDataUntyped(ServerEventHeader header, NetDataReader reader);

		object? GetServerRpcRequestCustomDataUntyped(ServerEventHeader header, NetDataReader reader);

		object? GetClientRpcRequestCustomDataUntyped(CustomRelayEventHeader header, NetDataReader reader);

		object? GetBuiltInResponseCustomDataUntyped(ServerEventHeader header, NetDataReader reader);

		object? GetServerRpcResponseCustomDataUntyped(ServerEventHeader header, NetDataReader reader);

		object? GetClientRpcResponseCustomDataUntyped(CustomRelayEventHeader header, NetDataReader reader);
	}
	public interface IShimRelayMessageParserImpl<out TCustomData> : IShimRelayMessageParserImpl
	{
		TCustomData GetBuiltInRequestCustomData(ServerEventHeader header, NetDataReader reader);

		TCustomData GetServerRpcRequestCustomData(ServerEventHeader header, NetDataReader reader);

		TCustomData GetClientRpcRequestCustomData(CustomRelayEventHeader header, NetDataReader reader);

		TCustomData GetBuiltInResponseCustomData(ServerEventHeader header, NetDataReader reader);

		TCustomData GetServerRpcResponseCustomData(ServerEventHeader header, NetDataReader reader);

		TCustomData GetClientRpcResponseCustomData(CustomRelayEventHeader header, NetDataReader reader);
	}
	public abstract class ShimBuiltInMessageParserImplBase<TCustomData> : ShimRelayMessageParserImplBase<TCustomData>
	{
		public override bool SupportsRequest(CustomRelayEventHeader header)
		{
			return false;
		}

		public override bool SupportsResponse(CustomRelayEventHeader header)
		{
			return false;
		}

		public override TCustomData GetServerRpcRequestCustomData(ServerEventHeader header, NetDataReader reader)
		{
			throw new NotSupportedException();
		}

		public override TCustomData GetClientRpcRequestCustomData(CustomRelayEventHeader header, NetDataReader reader)
		{
			throw new NotSupportedException();
		}

		public override TCustomData GetServerRpcResponseCustomData(ServerEventHeader header, NetDataReader reader)
		{
			throw new NotSupportedException();
		}

		public override TCustomData GetClientRpcResponseCustomData(CustomRelayEventHeader header, NetDataReader reader)
		{
			throw new NotSupportedException();
		}
	}
	public class ShimController
	{
		private readonly ShimRelayRecorder? _shimRecorder;

		private readonly TextRelaySerializer _textSerializer;

		private readonly ILogger _logger;

		public ShimController(ShimRelayRecorder? shimRecorder, TextRelaySerializer textSerializer, ILogger logger)
		{
			_shimRecorder = shimRecorder;
			_textSerializer = textSerializer;
			_logger = logger;
		}

		public void Save(string recordShimFile)
		{
			if (_shimRecorder == null)
			{
				_logger.LogError("Shim recorder is not initialized. Cannot save recording.");
				return;
			}
			ShimSerializer shimSerializer = new ShimSerializer(_textSerializer);
			ShimRecording recording = _shimRecorder.GetRecording();
			_logger.LogInformation("Saving shim recording to: {Path}", recordShimFile);
			shimSerializer.Save(recording, recordShimFile);
		}
	}
	public abstract class ShimDependencyTrackerImplBase<TCustomData> : IShimDependencyTrackerImpl
	{
		public bool Supports(ShimRequestItem requestItem)
		{
			if (requestItem.CustomData is TCustomData customData)
			{
				return Supports(requestItem, customData);
			}
			return false;
		}

		public bool Supports(ShimResponseItem responseItem)
		{
			if (responseItem.CustomData is TCustomData customData)
			{
				return Supports(responseItem, customData);
			}
			return false;
		}

		public abstract bool Supports(ShimRequestItem requestItem, TCustomData customData);

		public abstract bool Supports(ShimResponseItem responseItem, TCustomData customData);

		public abstract bool CheckRequestHasResponse(ShimRequestItem requestItem, ShimResponseItem responseItem);

		public abstract bool CheckResponseShouldWait(ShimResponseItem responseItem, IRelayClientNetworkThreadContext context, IEnumerable<ShimRequestItem> requestItems);
	}
	public class ShimPlaybackRelayClient : IRelayClient, IPlayerIdProvider, IDisposable
	{
		private class NetworkThreadContext : IRelayClientNetworkThreadContext
		{
			public readonly List<PlayerId> AllPlayers = new List<PlayerId>();

			public readonly List<PlayerId> AreaPlayers = new List<PlayerId>();

			public bool IsConnected { get; set; }

			public DisconnectReason LastDisconnectReason => (DisconnectReason)1;

			public PlayerId? PlayerId { get; set; }

			public AreaId? CurrentAreaId { get; set; }

			ReadyM.Api.Helpers.ReadOnlyList<PlayerId> IRelayClientNetworkThreadContext.AllPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(AllPlayers);

			ReadyM.Api.Helpers.ReadOnlyList<PlayerId> IRelayClientNetworkThreadContext.AreaPlayers => new ReadyM.Api.Helpers.ReadOnlyList<PlayerId>(AreaPlayers);
		}

		private readonly ShimReplayDependencyTracker _depTracker;

		private readonly ShimRelayMessageParser _parser;

		private readonly ILogger _logger;

		private int _delay;

		private Stopwatch? _stopwatch;

		private readonly object _lock = new object();

		private PlayerId? _playerId;

		private List<ShimRequestItem> _requestItems = new List<ShimRequestItem>();

		private int _responseItemIndex;

		private List<ShimResponseItem> _responseItems = new List<ShimResponseItem>();

		private readonly NetworkThreadContext _netThreadContext = new NetworkThreadContext();

		private readonly PendingActionUpdater<IRelayClientNetworkThreadContext> _scheduler;

		private volatile bool _isRunning;

		private volatile bool _isPlaying;

		private readonly ManualResetEventSlim _playerIdAssignedEvent = new ManualResetEventSlim();

		private DisconnectReason _lastDisconnectReason;

		private readonly Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>?[] _serverMessageHandlers = new Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>[251];

		private readonly Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>?[] _clientMessageHandlers = new Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>[150];

		[ThreadStatic]
		private static NetDataReader? _reader;

		public bool IsPlaying => _isPlaying;

		public bool RequestedConnect { get; private set; }

		public AreaId? RequestedAreaId { get; private set; }

		public PlayerId? PlayerId
		{
			get
			{
				if (!RequestedConnect)
				{
					return null;
				}
				lock (_lock)
				{
					return _playerId;
				}
			}
		}

		public PendingActionScheduler<IRelayClientNetworkThreadContext> Scheduler => _scheduler;

		public event Action? OnStart;

		public event Action? OnRequestedStop;

		public event Action? OnRequestedConnect;

		public event Action<IRelayClientNetworkThreadContext, PlayerId, uint>? OnConnected;

		public event Action? OnRequestedDisconnect;

		public event Action<IRelayClientNetworkThreadContext, DisconnectReason>? OnDisconnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerConnected;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerDisconnected;

		public event Action<AreaId>? OnRequestedJoinArea;

		public event Action<IRelayClientNetworkThreadContext, AreaId>? OnJoinedArea;

		public event Action? OnRequestedLeaveArea;

		public event Action<IRelayClientNetworkThreadContext>? OnLeftArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerJoinedArea;

		public event Action<IRelayClientNetworkThreadContext, PlayerId>? OnOtherPlayerLeftArea;

		public event Action<IRelayClientNetworkThreadContext, int>? OnPingUpdated;

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyBuiltInMessage
		{
			add
			{
				AddBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
			remove
			{
				RemoveBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, RelayMessageCode.EcsDelta, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>? OnAnyServerRpcMessage
		{
			add
			{
				AddServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
			remove
			{
				RemoveServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, RelayMessageCode.MaxAnyCustomEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>? OnAnyClientRpcMessage
		{
			add
			{
				AddClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
			remove
			{
				RemoveClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, RelayMessageCode.MaxClientRpcEvent, value);
			}
		}

		public event Action<IRelayClientNetworkThreadContext>? OnClientUpdate;

		public void AddBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 242 || (int)eventCode > 250)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveBuiltInMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 242 || (int)minEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			if ((int)maxEventCode < 242 || (int)maxEventCode > 250)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinBuiltInEvent` and `MaxBuiltInEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void AddServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Combine(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode < 150 || (int)eventCode > 241)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinServerRpcEvent` and `MaxServerRpcEvent`");
			}
			_serverMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveServerRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode < 150 || (int)minEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode < 150 || (int)maxEventCode > 241)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_serverMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)Delegate.Remove(_serverMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void AddClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void AddClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			if ((int)maxEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("maxEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Combine(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode eventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)eventCode > 149)
			{
				throw new ArgumentOutOfRangeException("eventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			_clientMessageHandlers[(uint)eventCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)eventCode], handler);
		}

		public void RemoveClientRpcMessageHandler(RelayMessageCode minEventCode, RelayMessageCode maxEventCode, Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> handler)
		{
			if ((int)minEventCode > 149)
			{
				throw new ArgumentOutOfRangeException("minEventCode", "Event code must be between `MinClientRpcEvent` and `MaxClientRpcEvent`");
			}
			RelayMessageCode relayMessageCode = minEventCode;
			while ((int)relayMessageCode <= (int)maxEventCode)
			{
				_clientMessageHandlers[(uint)relayMessageCode] = (Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)Delegate.Remove(_clientMessageHandlers[(uint)relayMessageCode], handler);
				relayMessageCode++;
			}
		}

		public ShimPlaybackRelayClient(ShimReplayDependencyTracker depTracker, ShimRelayMessageParser parser, ILogger logger)
		{
			_depTracker = depTracker;
			_parser = parser;
			_logger = logger;
			_scheduler = new PendingActionUpdater<IRelayClientNetworkThreadContext>(_netThreadContext, _logger);
		}

		public void Dispose()
		{
			Stop();
		}

		private void AddRequest(ShimRequestItem requestItem)
		{
			lock (_lock)
			{
				_requestItems.Add(requestItem);
			}
		}

		public void SetRecording(ShimRecording recording, int delay = 1000)
		{
			if (_isRunning)
			{
				throw new InvalidOperationException("Cannot set recording while the client is running");
			}
			_delay = delay;
			lock (_lock)
			{
				_playerId = recording.PlayerId;
				_requestItems = new List<ShimRequestItem>();
				_responseItems = new List<ShimResponseItem>(recording.ResponseItems);
			}
		}

		public int GetMaxPacketSize(DeliveryMethod deliveryMethod)
		{
			return 1300;
		}

		private async Task ProcessLoop(CancellationToken token)
		{
			_ = 2;
			try
			{
				ShimResponseItem? shimResponseItem = null;
				lock (_lock)
				{
					if (_responseItemIndex < _responseItems.Count)
					{
						shimResponseItem = _responseItems[_responseItemIndex];
					}
				}
				if (!shimResponseItem.HasValue)
				{
					await Task.Delay(2, token);
				}
				else
				{
					long num = _stopwatch.ElapsedMilliseconds - _delay;
					if (shimResponseItem.Value.Elapsed > num)
					{
						await Task.Delay(2, token);
					}
					else if (ProcessResponseItem(shimResponseItem.Value))
					{
						_responseItemIndex++;
						int num2 = 0;
						while (num2 < _requestItems.Count)
						{
							ShimRequestItem requestItem = _requestItems[num2];
							if (_depTracker.CheckRequestCanDelete(requestItem, shimResponseItem.Value))
							{
								_requestItems.RemoveAt(num2);
							}
							else
							{
								num2++;
							}
						}
					}
				}
				this.OnClientUpdate?.Invoke(_netThreadContext);
				if (!_scheduler.Update())
				{
					await Task.Delay(2, token);
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Unhandled exception in client thread (starting)");
			}
		}

		public void StartPlaying()
		{
			if (!_isRunning)
			{
				throw new InvalidOperationException("Shim relay client is not running");
			}
			if (_isPlaying)
			{
				_logger.LogError("Shim relay client is already playing");
			}
			else
			{
				_isPlaying = true;
			}
		}

		public void StopPlaying()
		{
			if (!_isPlaying)
			{
				_logger.LogError("Shim relay client is not playing");
			}
			else
			{
				_isPlaying = false;
			}
		}

		public void Start()
		{
			if (_isRunning)
			{
				_logger.LogError("Relay client is already running");
				return;
			}
			_isRunning = true;
			_logger.LogInformation("Starting shim relay client...");
			this.OnStart?.Invoke();
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
			_responseItemIndex = 0;
			_logger.LogInformation("Started shim relay client");
		}

		public async Task RunAsync(CancellationToken token)
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running. Call `StartAsync()` first.");
				return;
			}
			_logger.LogInformation("Running shim relay client");
			while (!token.IsCancellationRequested)
			{
				if (!_isPlaying)
				{
					await Task.Delay(2, token);
				}
				else
				{
					await ProcessLoop(token);
				}
			}
		}

		public void Stop()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (!_isRunning)
			{
				_logger.LogInformation("Shim relay client is not running");
				return;
			}
			_isRunning = false;
			_scheduler.SetThread(null);
			_logger.LogDebug("Stopping shim relay client");
			this.OnRequestedStop?.Invoke();
			if ((int)_lastDisconnectReason != 5)
			{
				_logger.LogWarning("Shim relay client already disconnected: {Reason}", _lastDisconnectReason);
			}
			_logger.LogDebug("Stopped shim relay client");
		}

		public void RequestConnect()
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running");
				return;
			}
			if (RequestedConnect)
			{
				_logger.LogWarning("Relay client is already connecting");
				return;
			}
			RequestedConnect = true;
			this.OnRequestedConnect?.Invoke();
			AddRequest(new ShimRequestItem
			{
				Kind = ShimRequestKind.RequestedConnect
			});
			_playerIdAssignedEvent.Wait(5000);
		}

		public void RequestDisconnect()
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running");
				return;
			}
			if (!RequestedConnect)
			{
				_logger.LogWarning("Relay client is already disconnecting");
				return;
			}
			RequestedConnect = false;
			AddRequest(new ShimRequestItem
			{
				Kind = ShimRequestKind.RequestedDisconnect
			});
			this.OnRequestedDisconnect?.Invoke();
		}

		public void RequestReconnect()
		{
			RequestDisconnect();
			RequestConnect();
		}

		public void RequestJoinArea(AreaId areaId)
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running");
				return;
			}
			if (!RequestedConnect)
			{
				_logger.LogError("Relay client is not connected to the server");
				return;
			}
			if (RequestedAreaId.HasValue)
			{
				_logger.LogWarning("Already requested to join a different area {AreaId}", RequestedAreaId.Value);
				RequestLeaveArea();
			}
			if (RequestedAreaId == areaId)
			{
				_logger.LogWarning("Already requested to join area {AreaId}", areaId);
				return;
			}
			AddRequest(new ShimRequestItem
			{
				Kind = ShimRequestKind.RequestedJoinArea,
				AreaId = areaId
			});
			this.OnRequestedJoinArea?.Invoke(areaId);
		}

		public void RequestLeaveArea()
		{
			if (!_isRunning)
			{
				_logger.LogError("Relay client is not running");
				return;
			}
			if (!RequestedConnect)
			{
				_logger.LogError("Relay client is not connected to the server");
				return;
			}
			if (!RequestedAreaId.HasValue)
			{
				_logger.LogWarning("Already requested to leave area");
				return;
			}
			RequestedAreaId = null;
			AddRequest(new ShimRequestItem
			{
				Kind = ShimRequestKind.RequestedLeaveArea
			});
			this.OnRequestedLeaveArea?.Invoke();
		}

		public void SendRawMessage(NetDataWriter writer, DeliveryMethod deliveryMethod)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			object obj = _reader;
			if (obj == null)
			{
				NetDataReader val = new NetDataReader();
				_reader = val;
				obj = (object)val;
			}
			NetDataReader val2 = (NetDataReader)obj;
			val2.SetSource(writer.Data, 0, writer.Length);
			RelayMessageCode relayMessageCode = (RelayMessageCode)val2.GetByte();
			ShimRequestItem requestItem = default(ShimRequestItem);
			if ((int)relayMessageCode >= 242)
			{
				ServerEventHeader serverEventHeader = new ServerEventHeader(relayMessageCode, ReadyM.Api.Multiplayer.Idents.PlayerId.Server);
				requestItem.Kind = ShimRequestKind.SentBuiltInMessage;
				requestItem.ServerHeader = serverEventHeader;
				requestItem.CustomData = _parser.GetBuiltInRequestCustomData(serverEventHeader, val2);
			}
			else if ((int)relayMessageCode >= 150)
			{
				ServerEventHeader serverEventHeader2 = new ServerEventHeader(relayMessageCode, ReadyM.Api.Multiplayer.Idents.PlayerId.Server);
				requestItem.Kind = ShimRequestKind.SentServerRpcMessage;
				requestItem.ServerHeader = serverEventHeader2;
				requestItem.CustomData = _parser.GetServerRpcRequestCustomData(serverEventHeader2, val2);
			}
			else
			{
				CustomRelayEventHeader customRelayEventHeader = val2.GetCustomRelayEventHeader(relayMessageCode);
				requestItem.Kind = ShimRequestKind.SentClientRpcMessage;
				requestItem.ClientHeader = customRelayEventHeader;
				requestItem.CustomData = _parser.GetClientRpcRequestCustomData(customRelayEventHeader, val2);
			}
			requestItem.RawData = new ShimBuffer(writer.Data, 1, writer.Length);
			AddRequest(requestItem);
		}

		public void SendMessage(RelayMessage message)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			SendRawMessage(message.Writer, message.DeliveryMethod);
		}

		public void SendMessageToServer<T>(RelayMessageCode eventCode, T data, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			RelayMessage message = RelayMessage.ToServer(eventCode, deliveryMethod);
			((INetSerializable)data/*cast due to .constrained prefix*/).Serialize(message.Writer);
			SendMessage(message);
		}

		public void SendMessageToPeers<T>(RelayMessageCode eventCode, T data, PlayerId[] peers, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				throw new Exception("PlayerId cannot be null");
			}
			RelayMessage message = RelayMessage.ToPeers(eventCode, playerId.Value, peers, deliveryMethod);
			((INetSerializable)data/*cast due to .constrained prefix*/).Serialize(message.Writer);
			SendMessage(message);
		}

		public void SendMessageRelayMode<T>(RelayMessageCode eventCode, T data, RelayMode mode, DeliveryMethod deliveryMethod) where T : INetSerializable
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			PlayerId? playerId = PlayerId;
			if (!playerId.HasValue)
			{
				throw new Exception("PlayerId cannot be null");
			}
			RelayMessage message = RelayMessage.ByRelayMode(eventCode, playerId.Value, mode, deliveryMethod);
			SendMessage(message);
		}

		public void LogEventStats()
		{
		}

		public bool ProcessResponseItem(ShimResponseItem responseItem)
		{
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Expected O, but got Unknown
			//IL_0956: Unknown result type (might be due to invalid IL or missing references)
			//IL_095d: Expected O, but got Unknown
			//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a28: Expected O, but got Unknown
			if (_depTracker.CheckResponseShouldWait(responseItem, _netThreadContext, _requestItems))
			{
				return false;
			}
			switch (responseItem.Kind)
			{
			case ShimResponseKind.Connected:
			{
				PlayerId playerId2 = responseItem.PlayerId;
				uint nextId = responseItem.NextId;
				if (_netThreadContext.PlayerId.HasValue)
				{
					_logger.LogError("Missing handshake for player {PlayerId} but already assigned {AssignedPlayerId}", playerId2, _netThreadContext.PlayerId);
				}
				_netThreadContext.IsConnected = true;
				_netThreadContext.PlayerId = playerId2;
				_netThreadContext.AllPlayers.Add(playerId2);
				_playerIdAssignedEvent.Set();
				for (int j = 0; j < responseItem.OtherPlayers.Count; j++)
				{
					PlayerId playerId3 = responseItem.OtherPlayers[j];
					if (!_netThreadContext.AllPlayers.Contains(playerId3))
					{
						_netThreadContext.AllPlayers.Add(playerId3);
						continue;
					}
					_logger.LogError("Received handshake for player {PlayerId} that already is marked as connected", playerId3);
				}
				_logger.LogInformation("Assigned Actor ID {PlayerId}", playerId2);
				_logger.LogDebug("Next available NetworkId is {NextNetworkId}", nextId);
				this.OnConnected?.Invoke(_netThreadContext, playerId2, nextId);
				break;
			}
			case ShimResponseKind.Disconnected:
				_logger.LogInformation("Disconnected from server: {Reason}", responseItem.DisconnectReason);
				_netThreadContext.IsConnected = false;
				_lastDisconnectReason = responseItem.DisconnectReason;
				this.OnDisconnected?.Invoke(_netThreadContext, responseItem.DisconnectReason);
				break;
			case ShimResponseKind.OtherPlayerConnected:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				PlayerId playerId6 = responseItem.PlayerId;
				if (!_netThreadContext.AllPlayers.Contains(playerId6))
				{
					_netThreadContext.AllPlayers.Add(playerId6);
					this.OnOtherPlayerConnected?.Invoke(_netThreadContext, playerId6);
				}
				else
				{
					_logger.LogError("Player connected event for player {PlayerId} that already is marked as connected", playerId6);
				}
				break;
			}
			case ShimResponseKind.OtherPlayerDisconnected:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				PlayerId playerId5 = responseItem.PlayerId;
				if (_netThreadContext.AllPlayers.Contains(playerId5))
				{
					_netThreadContext.AllPlayers.Remove(playerId5);
					if (_netThreadContext.AreaPlayers.Contains(playerId5))
					{
						_logger.LogInformation("Player disconnected event for player {PlayerId} that is still in the area", playerId5);
						_netThreadContext.AreaPlayers.Remove(playerId5);
						this.OnOtherPlayerLeftArea?.Invoke(_netThreadContext, playerId5);
					}
					this.OnOtherPlayerDisconnected?.Invoke(_netThreadContext, playerId5);
				}
				else
				{
					_logger.LogError("Player disconnected event for player {PlayerId} that already is marked as NOT connected", playerId5);
				}
				break;
			}
			case ShimResponseKind.JoinedArea:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				PlayerId playerId9 = responseItem.PlayerId;
				if (!_netThreadContext.PlayerId.HasValue)
				{
					_logger.LogError("Received handshake for joining area {AreaId} by player {PlayerId} but PlayerId is not set", playerId9, _netThreadContext.PlayerId);
					break;
				}
				PlayerId value = playerId9;
				PlayerId? playerId8 = PlayerId;
				if (value != playerId8)
				{
					_logger.LogError("Received handshake for player {PlayerId} but expected {ExpectedPlayerId}", playerId9, PlayerId);
					break;
				}
				if (_netThreadContext.CurrentAreaId.HasValue)
				{
					_logger.LogError("Received handshake for joining area {AreaId} by player {PlayerId} but already in area {CurrentArea}", playerId9, _netThreadContext.PlayerId, _netThreadContext.CurrentAreaId);
					break;
				}
				AreaId areaId = responseItem.AreaId;
				_netThreadContext.CurrentAreaId = areaId;
				_netThreadContext.AreaPlayers.Clear();
				_netThreadContext.AreaPlayers.Add(playerId9);
				for (int k = 0; k < responseItem.OtherPlayers.Count; k++)
				{
					PlayerId playerId10 = responseItem.OtherPlayers[k];
					if (playerId10 == playerId9)
					{
						_logger.LogError("Received handshake for joining area {AreaId} by player {PlayerId} but other player list contains the same player", areaId, playerId9);
					}
					else
					{
						_netThreadContext.AreaPlayers.Add(playerId10);
					}
				}
				this.OnJoinedArea?.Invoke(_netThreadContext, areaId);
				break;
			}
			case ShimResponseKind.LeftArea:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				if (!_netThreadContext.CurrentAreaId.HasValue)
				{
					return false;
				}
				PlayerId playerId7 = responseItem.PlayerId;
				if (!_netThreadContext.PlayerId.HasValue)
				{
					_logger.LogError("Received handshake for leaving area by player {PlayerId} but PlayerId is not set", playerId7);
					break;
				}
				PlayerId value = playerId7;
				PlayerId? playerId8 = PlayerId;
				if (value != playerId8)
				{
					_logger.LogError("Received handshake for player {PlayerId} but expected {ExpectedPlayerId}", playerId7, PlayerId);
				}
				else if (!_netThreadContext.CurrentAreaId.HasValue)
				{
					_logger.LogError("Received handshake for leaving area by player {PlayerId} but not in any area", playerId7);
				}
				else
				{
					_netThreadContext.CurrentAreaId = null;
					_netThreadContext.AreaPlayers.Remove(playerId7);
					this.OnLeftArea?.Invoke(_netThreadContext);
				}
				break;
			}
			case ShimResponseKind.OtherPlayerJoinedArea:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				if (!_netThreadContext.CurrentAreaId.HasValue)
				{
					return false;
				}
				PlayerId playerId4 = responseItem.PlayerId;
				if (!_netThreadContext.CurrentAreaId.HasValue)
				{
					_logger.LogError("Received area event for player {PlayerId} but current area is not set", playerId4);
				}
				else if (!_netThreadContext.AreaPlayers.Contains(playerId4))
				{
					_netThreadContext.AreaPlayers.Add(playerId4);
					this.OnOtherPlayerJoinedArea?.Invoke(_netThreadContext, playerId4);
				}
				else
				{
					_logger.LogError("Player joined area event for player {PlayerId} that already is marked as in the area", playerId4);
				}
				break;
			}
			case ShimResponseKind.OtherPlayerLeftArea:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				if (!_netThreadContext.CurrentAreaId.HasValue)
				{
					return false;
				}
				PlayerId playerId = responseItem.PlayerId;
				if (_netThreadContext.AreaPlayers.Contains(playerId))
				{
					_netThreadContext.AreaPlayers.Remove(playerId);
					this.OnOtherPlayerLeftArea?.Invoke(_netThreadContext, playerId);
				}
				else
				{
					_logger.LogError("Player left area event for player {PlayerId} that already is marked as NOT in the area", playerId);
				}
				break;
			}
			case ShimResponseKind.PingUpdated:
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				this.OnPingUpdated?.Invoke(_netThreadContext, responseItem.Ping);
				break;
			case ShimResponseKind.AnyBuiltInMessage:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				ServerEventHeader serverHeader = responseItem.ServerHeader;
				ShimBuffer rawData2 = responseItem.RawData;
				NetDataReader val2 = new NetDataReader(rawData2.Data, rawData2.Offset, rawData2.MaxSize);
				Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action2 = _serverMessageHandlers[(uint)serverHeader.EventCode];
				if (action2 != null)
				{
					int position2 = val2.Position;
					Delegate[] invocationList = action2.GetInvocationList();
					foreach (Delegate obj2 in invocationList)
					{
						val2.SetPosition(position2);
						((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj2)(_netThreadContext, serverHeader, val2);
					}
				}
				break;
			}
			case ShimResponseKind.AnyServerMessage:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				ServerEventHeader serverHeader2 = responseItem.ServerHeader;
				ShimBuffer rawData3 = responseItem.RawData;
				NetDataReader val3 = new NetDataReader(rawData3.Data, rawData3.Offset, rawData3.MaxSize);
				Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader> action3 = _serverMessageHandlers[(uint)serverHeader2.EventCode];
				if (action3 != null)
				{
					int position3 = val3.Position;
					Delegate[] invocationList = action3.GetInvocationList();
					foreach (Delegate obj3 in invocationList)
					{
						val3.SetPosition(position3);
						((Action<IRelayClientNetworkThreadContext, ServerEventHeader, NetDataReader>)obj3)(_netThreadContext, serverHeader2, val3);
					}
				}
				break;
			}
			case ShimResponseKind.AnyClientMessage:
			{
				if (!_netThreadContext.IsConnected)
				{
					return false;
				}
				CustomRelayEventHeader clientHeader = responseItem.ClientHeader;
				if ((clientHeader.RelayMode == RelayMode.AreaOfInterestOthers || clientHeader.RelayMode == RelayMode.AreaOfInterestAll) && !_netThreadContext.CurrentAreaId.HasValue)
				{
					return false;
				}
				ShimBuffer rawData = responseItem.RawData;
				NetDataReader val = new NetDataReader(rawData.Data, rawData.Offset, rawData.MaxSize);
				Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader> action = _clientMessageHandlers[(uint)clientHeader.EventCode];
				if (action != null)
				{
					int position = val.Position;
					Delegate[] invocationList = action.GetInvocationList();
					foreach (Delegate obj in invocationList)
					{
						val.SetPosition(position);
						((Action<IRelayClientNetworkThreadContext, CustomRelayEventHeader, NetDataReader>)obj)(_netThreadContext, clientHeader, val);
					}
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("Kind", responseItem.Kind, $"Unknown ShimItemKind: {responseItem.Kind}");
			}
			return true;
		}
	}
	public class ShimRelayMessageParser
	{
		private readonly List<IShimRelayMessageParserImpl> _impls = impls.ToList();

		public ShimRelayMessageParser(IEnumerable<IShimRelayMessageParserImpl> impls)
		{
		}

		public object? GetBuiltInRequestCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsRequest(header))
				{
					object? builtInRequestCustomDataUntyped = impl.GetBuiltInRequestCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return builtInRequestCustomDataUntyped;
				}
			}
			return null;
		}

		public object? GetServerRpcRequestCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsRequest(header))
				{
					object? serverRpcRequestCustomDataUntyped = impl.GetServerRpcRequestCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return serverRpcRequestCustomDataUntyped;
				}
			}
			return null;
		}

		public object? GetClientRpcRequestCustomData(CustomRelayEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsRequest(header))
				{
					object? clientRpcRequestCustomDataUntyped = impl.GetClientRpcRequestCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return clientRpcRequestCustomDataUntyped;
				}
			}
			return null;
		}

		public object? GetBuiltInResponseCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsResponse(header))
				{
					object? builtInResponseCustomDataUntyped = impl.GetBuiltInResponseCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return builtInResponseCustomDataUntyped;
				}
			}
			return null;
		}

		public object? GetServerRpcResponseCustomData(ServerEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsResponse(header))
				{
					object? serverRpcResponseCustomDataUntyped = impl.GetServerRpcResponseCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return serverRpcResponseCustomDataUntyped;
				}
			}
			return null;
		}

		public object? GetClientRpcResponseCustomData(CustomRelayEventHeader header, NetDataReader reader)
		{
			int position = reader.Position;
			foreach (IShimRelayMessageParserImpl impl in _impls)
			{
				if (impl.SupportsResponse(header))
				{
					object? clientRpcResponseCustomDataUntyped = impl.GetClientRpcResponseCustomDataUntyped(header, reader);
					reader.SetPosition(position);
					return clientRpcResponseCustomDataUntyped;
				}
			}
			return null;
		}
	}
	public abstract class ShimRelayMessageParserImplBase<TCustomData> : IShimRelayMessageParserImpl<TCustomData>, IShimRelayMessageParserImpl
	{
		public abstract bool SupportsRequest(ServerEventHeader header);

		public abstract bool SupportsRequest(CustomRelayEventHeader header);

		public abstract bool SupportsResponse(ServerEventHeader header);

		public abstract bool SupportsResponse(CustomRelayEventHeader header);

		public object? GetBuiltInRequestCustomDataUntyped(ServerEventHeader header, NetDataReader reader)
		{
			return GetBuiltInRequestCustomData(header, reader);
		}

		public object? GetServerRpcRequestCustomDataUntyped(ServerEventHeader header, NetDataReader reader)
		{
			return GetServerRpcRequestCustomData(header, reader);
		}

		public object? GetClientRpcRequestCustomDataUntyped(CustomRelayEventHeader header, NetDataReader reader)
		{
			return GetClientRpcRequestCustomData(header, reader);
		}

		public object? GetBuiltInResponseCustomDataUntyped(ServerEventHeader header, NetDataReader reader)
		{
			return GetBuiltInResponseCustomData(header, reader);
		}

		public object? GetServerRpcResponseCustomDataUntyped(ServerEventHeader header, NetDataReader reader)
		{
			return GetServerRpcResponseCustomData(header, reader);
		}

		public object? GetClientRpcResponseCustomDataUntyped(CustomRelayEventHeader header, NetDataReader reader)
		{
			return GetClientRpcResponseCustomData(header, reader);
		}

		public abstract TCustomData GetBuiltInRequestCustomData(ServerEventHeader header, NetDataReader reader);

		public abstract TCustomData GetServerRpcRequestCustomData(ServerEventHeader header, NetDataReader reader);

		public abstract TCustomData GetClientRpcRequestCustomData(CustomRelayEventHeader header, NetDataReader reader);

		public abstract TCustomData GetBuiltInResponseCustomData(ServerEventHeader header, NetDataReader reader);

		public abstract TCustomData GetServerRpcResponseCustomData(ServerEventHeader header, NetDataReader reader);

		public abstract TCustomData GetClientRpcResponseCustomData(CustomRelayEventHeader header, NetDataReader reader);
	}
	public class ShimRelayRecorder : IDisposable
	{
		private readonly object _lock = new object();

		private readonly List<ShimResponseItem> _responseItems = new List<ShimResponseItem>();

		private readonly IRelayClient _attachedRelayClient;

		private bool _isRecording;

		private readonly Stopwatch _stopwatch = new Stopwatch();

		private readonly ShimRelayMessageParser _parser;

		private readonly ILogger _logger;

		public IRelayClient AttachedRelayClient => _attachedRelayClient;

		public event Action? OnRecordingStarted;

		public event Action? OnRecordingStopped;

		public ShimRelayRecorder(IRelayClient attachedRelayClient, ShimRelayMessageParser parser, ILogger logger)
		{
			_parser = parser;
			_logger = logger;
			_attachedRelayClient = attachedRelayClient;
			_attachedRelayClient.OnConnected += OnConnectedHandler;
			_attachedRelayClient.OnDisconnected += OnDisconnectedHandler;
			_attachedRelayClient.OnOtherPlayerConnected += OnOtherPlayerConnectedHandler;
			_attachedRelayClient.OnOtherPlayerDisconnected += OnOtherPlayerDisconnectedHandler;
			_attachedRelayClient.OnJoinedArea += OnJoinedAreaHandler;
			_attachedRelayClient.OnLeftArea += OnLeftAreaHandler;
			_attachedRelayClient.OnOtherPlayerJoinedArea += OnOtherPlayerJoinedAreaHandler;
			_attachedRelayClient.OnOtherPlayerLeftArea += OnOtherPlayerLeftAreaHandler;
			_attachedRelayClient.OnPingUpdated += OnPingUpdatedHandler;
			_attachedRelayClient.OnAnyBuiltInMessage += OnAnyBuiltInMessageHandler;
			_attachedRelayClient.OnAnyServerRpcMessage += OnAnyServerRpcMessageHandler;
			_attachedRelayClient.OnAnyClientRpcMessage += OnAnyClientRpcMessageHandler;
		}

		public void Dispose()
		{
			if (_isRecording)
			{
				StopRecording();
			}
			_attachedRelayClient.OnAnyClientRpcMessage -= OnAnyClientRpcMessageHandler;
			_attachedRelayClient.OnAnyServerRpcMessage -= OnAnyServerRpcMessageHandler;
			_attachedRelayClient.OnAnyBuiltInMessage -= OnAnyBuiltInMessageHandler;
			_attachedRelayClient.OnPingUpdated -= OnPingUpdatedHandler;
			_attachedRelayClient.OnOtherPlayerLeftArea -= OnOtherPlayerLeftAreaHandler;
			_attachedRelayClient.OnOtherPlayerJoinedArea -= OnOtherPlayerJoinedAreaHandler;
			_attachedRelayClient.OnLeftArea -= OnLeftAreaHandler;
			_attachedRelayClient.OnJoinedArea -= OnJoinedAreaHandler;
			_attachedRelayClient.OnOtherPlayerDisconnected -= OnOtherPlayerDisconnectedHandler;
			_attachedRelayClient.OnOtherPlayerConnected -= OnOtherPlayerConnectedHandler;
			_attachedRelayClient.OnDisconnected -= OnDisconnectedHandler;
			_attachedRelayClient.OnConnected -= OnConnectedHandler;
		}

		public ShimRecording GetRecording()
		{
			lock (_lock)
			{
				return new ShimRecording(_responseItems, _attachedRelayClient?.PlayerId);
			}
		}

		public void StartRecording()
		{
			if (!_isRecording)
			{
				_isRecording = true;
				_logger.LogDebug("Starting shim recording");
				_stopwatch.Start();
				this.OnRecordingStarted?.Invoke();
			}
		}

		public void StopRecording()
		{
			if (_isRecording)
			{
				_logger.LogDebug("Stopping shim recording");
				this.OnRecordingStopped?.Invoke();
				_isRecording = false;
				_stopwatch.Stop();
			}
		}

		private void AddResponseItem(ShimResponseItem responseItem)
		{
			responseItem.Elapsed = _stopwatch.ElapsedMilliseconds;
			lock (_lock)
			{
				_responseItems.Add(responseItem);
			}
		}

		private void OnConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId, uint nextId)
		{
			List<PlayerId> list = context.AllPlayers.ToList();
			list.Remove(playerId);
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.Connected,
				PlayerId = playerId,
				OtherPlayers = list,
				NextId = nextId
			};
			AddResponseItem(responseItem);
		}

		private void OnDisconnectedHandler(IRelayClientNetworkThreadContext context, DisconnectReason disconnectReason)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.Disconnected,
				DisconnectReason = disconnectReason
			};
			AddResponseItem(responseItem);
		}

		private void OnOtherPlayerConnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.OtherPlayerConnected,
				PlayerId = playerId
			};
			AddResponseItem(responseItem);
		}

		private void OnOtherPlayerDisconnectedHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.OtherPlayerDisconnected,
				PlayerId = playerId
			};
			AddResponseItem(responseItem);
		}

		private void OnJoinedAreaHandler(IRelayClientNetworkThreadContext context, AreaId areaId)
		{
			PlayerId value = context.PlayerId.Value;
			List<PlayerId> list = context.AreaPlayers.ToList();
			list.Remove(value);
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.JoinedArea,
				AreaId = areaId,
				PlayerId = context.PlayerId.Value,
				OtherPlayers = list
			};
			AddResponseItem(responseItem);
		}

		private void OnLeftAreaHandler(IRelayClientNetworkThreadContext context)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.LeftArea,
				PlayerId = context.PlayerId.Value
			};
			AddResponseItem(responseItem);
		}

		private void OnOtherPlayerJoinedAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.OtherPlayerJoinedArea,
				PlayerId = playerId
			};
			AddResponseItem(responseItem);
		}

		private void OnOtherPlayerLeftAreaHandler(IRelayClientNetworkThreadContext context, PlayerId playerId)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.OtherPlayerLeftArea,
				PlayerId = playerId
			};
			AddResponseItem(responseItem);
		}

		private void OnPingUpdatedHandler(IRelayClientNetworkThreadContext context, int ping)
		{
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.PingUpdated,
				Ping = ping
			};
			AddResponseItem(responseItem);
		}

		private void OnAnyBuiltInMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			object builtInResponseCustomData = _parser.GetBuiltInResponseCustomData(header, reader);
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.AnyBuiltInMessage,
				ServerHeader = header,
				RawData = GetShimBuffer(reader),
				CustomData = builtInResponseCustomData
			};
			AddResponseItem(responseItem);
		}

		private void OnAnyServerRpcMessageHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			object serverRpcResponseCustomData = _parser.GetServerRpcResponseCustomData(header, reader);
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.AnyServerMessage,
				ServerHeader = header,
				RawData = GetShimBuffer(reader),
				CustomData = serverRpcResponseCustomData
			};
			AddResponseItem(responseItem);
		}

		private void OnAnyClientRpcMessageHandler(IRelayClientNetworkThreadContext context, CustomRelayEventHeader header, NetDataReader reader)
		{
			object clientRpcResponseCustomData = _parser.GetClientRpcResponseCustomData(header, reader);
			ShimResponseItem responseItem = new ShimResponseItem
			{
				Kind = ShimResponseKind.AnyClientMessage,
				ClientHeader = header,
				RawData = GetShimBuffer(reader),
				CustomData = clientRpcResponseCustomData
			};
			AddResponseItem(responseItem);
		}

		private ShimBuffer GetShimBuffer(NetDataReader reader)
		{
			if (reader.AvailableBytes > 0)
			{
				byte[] array = new byte[reader.AvailableBytes];
				Array.Copy(reader.RawData, reader.Position, array, 0, reader.AvailableBytes);
				return new ShimBuffer(array);
			}
			return new ShimBuffer(Array.Empty<byte>());
		}
	}
	public class ShimReplayDependencyTracker
	{
		private readonly List<IShimDependencyTrackerImpl> _impls = impls.ToList();

		public ShimReplayDependencyTracker(IEnumerable<IShimDependencyTrackerImpl> impls)
		{
		}

		public bool CheckRequestCanDelete(ShimRequestItem requestItem, ShimResponseItem responseItem)
		{
			foreach (IShimDependencyTrackerImpl impl in _impls)
			{
				if (impl.Supports(requestItem) && impl.Supports(responseItem) && impl.CheckRequestHasResponse(requestItem, responseItem))
				{
					return true;
				}
			}
			return false;
		}

		public bool CheckResponseShouldWait(ShimResponseItem responseItem, IRelayClientNetworkThreadContext context, IEnumerable<ShimRequestItem> requestItems)
		{
			foreach (IShimDependencyTrackerImpl impl in _impls)
			{
				if (impl.Supports(responseItem) && impl.CheckResponseShouldWait(responseItem, context, requestItems))
				{
					return true;
				}
			}
			return false;
		}
	}
}
namespace ReadyM.Relay.Client.Serialization
{
	public class ClientShimTextSerializerRegistration : ITextRelaySerializerRegistration
	{
		public void Register(TextRelaySerializer serializer)
		{
			serializer.RegisterPolymorphicType<ShimBlobDependencyData>("shimBlob");
			serializer.RegisterPolymorphicType<ShimEcsDependencyData>("shimEcs");
		}
	}
}
namespace ReadyM.Relay.Client.Host
{
	public class RelayClientService(IRelayClient relayClient, ILogger logger) : IDisposable
	{
		private AsyncContextThread? _isolatedNoParallelismAsyncContextThread;

		private Task? _task;

		private CancellationTokenSource? _source;

		public IRelayClient RelayClient => relayClient;

		public bool IsRunning { get; private set; }

		public void Dispose()
		{
			if (IsRunning)
			{
				Stop();
			}
			_isolatedNoParallelismAsyncContextThread?.Dispose();
			_task?.Dispose();
			_source?.Dispose();
		}

		public void Start()
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;
			logger.LogInformation("Starting RelayClientService...");
			_source = new CancellationTokenSource();
			CancellationToken stoppingToken = _source.Token;
			ManualResetEventSlim startedEvent = new ManualResetEventSlim();
			_isolatedNoParallelismAsyncContextThread = new AsyncContextThread();
			_task = _isolatedNoParallelismAsyncContextThread.Factory.Run(async delegate
			{
				try
				{
					relayClient.Start();
				}
				finally
				{
					startedEvent.Set();
				}
				await relayClient.RunAsync(stoppingToken);
			});
			startedEvent.Wait(stoppingToken);
			logger.LogInformation("Started RelayClientService.");
		}

		public void Stop()
		{
			if (IsRunning)
			{
				logger.LogInformation("Stopping RelayClientService...");
				_source?.Cancel();
				_isolatedNoParallelismAsyncContextThread?.Join();
				_task?.GetAwaiter().GetResult();
				IsRunning = false;
				relayClient.Stop();
				logger.LogInformation("Stopped RelayClientService.");
			}
		}
	}
}
namespace ReadyM.Relay.Client.ECS.Systems
{
	public class ClientSendComponentDeltaSystem<T> : SendComponentDeltaSystemBase<T> where T : struct, INetworkedComponent
	{
		public ClientSendComponentDeltaSystem(NetworkedComponentId componentId, DeliveryMethod deliveryMethod, IRelayClient relay)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			<deliveryMethod>P = deliveryMethod;
			<relay>P = relay;
			base..ctor(componentId);
		}

		protected override QueryFilter SetupFilter(QueryFilter filter, SendContext context)
		{
			return filter;
		}

		protected override int? GetMaxPacketSize()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if ((int)<deliveryMethod>P != 2)
			{
				return <relay>P.GetMaxPacketSize(<deliveryMethod>P);
			}
			return null;
		}

		protected override uint SentOwners()
		{
			if (<relay>P.PlayerId.HasValue)
			{
				return (uint)(1 << (int)<relay>P.PlayerId.Value.RawValue);
			}
			return 0u;
		}

		protected override void Send(PlayerId _, NetDataWriter data, SendContext context)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			<relay>P.SendRawMessage(data, <deliveryMethod>P);
		}
	}
	public class ClientSendEntityCreatedSystem : SendEntityCreatedSystemBase
	{
		public ClientSendEntityCreatedSystem(JobRegistry jobRegistry, ClientState state, IRelayClient relay)
		{
			<state>P = state;
			<relay>P = relay;
			base..ctor(jobRegistry);
		}

		protected override QueryFilter SetupFilter(QueryFilter filter, SendContext context)
		{
			filter = SetupBaseFilter(filter);
			filter = ((!context.IsGlobal) ? filter.HasValue<InScopeComponent, Entity>(context.ScopeEntity.Value) : filter.WithoutAnyComponents(ComponentTypes.Get<InScopeComponent>()));
			return filter;
		}

		protected override void CreatePacketHeader(NetDataWriter writer, SendContext context)
		{
			writer.Put((byte)248);
			if (context.ScopeEntity.HasValue)
			{
				MetadataComponent component = context.ScopeEntity.Value.GetComponent<MetadataComponent>();
				writer.Put<NetworkId>(component.NetId);
			}
			else
			{
				writer.Put<NetworkId>(default(NetworkId));
			}
		}

		protected override void Send(NetDataWriter data, SendContext context)
		{
			<relay>P.SendRawMessage(data, (DeliveryMethod)2);
		}

		protected override void OnUpdate()
		{
			OnUpdate(SendContext.Global);
			ClientState clientState = <state>P;
			if (clientState != null && clientState.CurrentAreaEntry.HasValue && clientState.LocalPlayerEntry.HasValue)
			{
				Entity playerEntity = <state>P.LocalPlayerEntry.Value.PlayerEntity;
				SendContext context = SendContext.FromPlayer(<state>P.LocalPlayerEntry.Value.PlayerId, playerEntity);
				OnUpdate(context);
				Entity areaEntity = <state>P.CurrentAreaEntry.Value.AreaEntity;
				SendContext context2 = SendContext.FromArea(<state>P.CurrentAreaEntry.Value.AreaId, areaEntity);
				OnUpdate(context2);
			}
		}
	}
}
namespace ReadyM.Relay.Client.Blobs
{
	public class BlobClient : IBlobClient, IDisposable
	{
		private readonly IRelayClient _relayClient;

		private readonly ILogger _logger;

		private int _requestCounter;

		private readonly ConcurrentDictionary<int, TaskCompletionSource<BlobInfo?>> _blobDownloadTasks = new ConcurrentDictionary<int, TaskCompletionSource<BlobInfo>>();

		private readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> _blobUploadTasks = new ConcurrentDictionary<int, TaskCompletionSource<bool>>();

		public event Action<IRelayClientNetworkThreadContext, int, bool>? OnUploadBlobAck;

		public event Action<IRelayClientNetworkThreadContext, int, BlobInfo?>? OnDownloadBlobData;

		private int GetNextRequestId()
		{
			return ++_requestCounter;
		}

		public BlobClient(IRelayClient relayClient, ILogger logger)
		{
			_relayClient = relayClient;
			_logger = logger;
			_relayClient.AddBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, OnUploadBlobAckHandler);
			_relayClient.AddBuiltInMessageHandler(RelayMessageCode.DownloadBlobData, OnDownloadBlobDataHandler);
		}

		public void Dispose()
		{
			_relayClient.RemoveBuiltInMessageHandler(RelayMessageCode.DownloadBlobData, OnDownloadBlobDataHandler);
			_relayClient.RemoveBuiltInMessageHandler(RelayMessageCode.UploadBlobAck, OnUploadBlobAckHandler);
		}

		private void OnUploadBlobAckHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			int num = reader.GetInt();
			bool flag = reader.GetBool();
			_logger.LogInformation("File upload with request ID {RequestId} completed with success: {Success}", num, flag);
			if (!_blobUploadTasks.TryRemove(num, out TaskCompletionSource<bool> value))
			{
				_logger.LogWarning("No task found for request ID {RequestId} when receiving upload ack", num);
			}
			else if (value.Task.IsCanceled)
			{
				_logger.LogWarning("Upload task already cancelled, not setting result for request ID {RequestId}", num);
			}
			else if (value.TrySetResult(flag))
			{
				this.OnUploadBlobAck?.Invoke(context, num, flag);
			}
			else
			{
				_logger.LogError("Failed to set result for file upload task with request ID {RequestId}", num);
			}
		}

		private void OnDownloadBlobDataHandler(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			int num = reader.GetInt();
			bool flag = reader.GetBool();
			_logger.LogInformation("File download with request ID {RequestId} completed with success: {Succeeded}", num, flag);
			if (!_blobDownloadTasks.TryRemove(num, out TaskCompletionSource<BlobInfo> value))
			{
				_logger.LogError("No task found for request ID {RequestId}", num);
				return;
			}
			BlobInfo blobInfo = null;
			if (flag)
			{
				string text = reader.GetString();
				int num2 = reader.GetInt();
				byte[] array = new byte[num2];
				reader.GetBytes(array, num2);
				_logger.LogInformation("Received file stream for {FileName} with request ID {RequestId}", text, num);
				blobInfo = new BlobInfo(text, array);
			}
			else
			{
				_logger.LogWarning("File download with request ID {RequestId} failed", num);
			}
			if (value.Task.IsCanceled)
			{
				_logger.LogWarning("Download task already cancelled, not setting result for request ID {RequestId}", num);
			}
			else if (value.TrySetResult(blobInfo))
			{
				this.OnDownloadBlobData?.Invoke(context, num, blobInfo);
			}
			else
			{
				_logger.LogError("Failed to set result for file download task with request ID {RequestId}", num);
			}
		}

		public async Task<bool> UploadBlobAsync(BlobInfo blob, CancellationToken ct = default(CancellationToken))
		{
			if (!_relayClient.RequestedConnect)
			{
				throw new InvalidOperationException();
			}
			ct.ThrowIfCancellationRequested();
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { ct });
			cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(15000.0));
			ct = cancellationTokenSource.Token;
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			int requestId = GetNextRequestId();
			_blobUploadTasks[requestId] = tcs;
			NetDataWriter val = new NetDataWriter();
			val.Put((byte)243);
			val.Put(requestId);
			val.Put(blob.Name);
			val.Put(blob.Content.Length);
			val.Put(blob.Content);
			_relayClient.SendRawMessage(val, (DeliveryMethod)2);
			_logger.LogInformation("Uploading file: {FileName} with request ID {RequestId}", blob.Name, requestId);
			using (ct.Register(CancelCallback))
			{
				try
				{
					return await tcs.Task;
				}
				catch (OperationCanceledException exception)
				{
					_logger.LogWarning(exception, "File upload for {FileName} was cancelled with request ID {RequestId}", blob.Name, requestId);
					throw;
				}
				finally
				{
					_blobUploadTasks.TryRemove(requestId, out TaskCompletionSource<bool> _);
				}
			}
			void CancelCallback()
			{
				_logger.LogWarning("Upload task for {FileName} with request ID {RequestId} was cancelled (TIMEOUT)", blob.Name, requestId);
				tcs.TrySetCanceled();
			}
		}

		public async Task<BlobInfo?> DownloadBlobAsync(string name, CancellationToken ct = default(CancellationToken))
		{
			if (!_relayClient.RequestedConnect)
			{
				throw new InvalidOperationException();
			}
			ct.ThrowIfCancellationRequested();
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { ct });
			cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10000.0));
			ct = cancellationTokenSource.Token;
			TaskCompletionSource<BlobInfo?> tcs = new TaskCompletionSource<BlobInfo>();
			int requestId = GetNextRequestId();
			_blobDownloadTasks[requestId] = tcs;
			NetDataWriter val = new NetDataWriter();
			val.Put((byte)245);
			val.Put(requestId);
			val.Put(name);
			_relayClient.SendRawMessage(val, (DeliveryMethod)2);
			_logger.LogInformation("Requesting file download: {FileName} with request ID {RequestId}", name, requestId);
			using (ct.Register(CancelCallback))
			{
				try
				{
					ct.ThrowIfCancellationRequested();
					return await tcs.Task;
				}
				catch (OperationCanceledException exception)
				{
					_logger.LogWarning(exception, "File download for {FileName} was cancelled with request ID {RequestId}", name, requestId);
					throw;
				}
				finally
				{
					_blobDownloadTasks.TryRemove(requestId, out TaskCompletionSource<BlobInfo> _);
				}
			}
			void CancelCallback()
			{
				_logger.LogWarning("Download task for {FileName} with request ID {RequestId} was cancelled (TIMEOUT)", name, requestId);
				tcs.TrySetCanceled();
			}
		}
	}
	public interface IBlobClient
	{
		Task<bool> UploadBlobAsync(BlobInfo blob, CancellationToken ct = default(CancellationToken));

		Task<BlobInfo?> DownloadBlobAsync(string name, CancellationToken ct = default(CancellationToken));
	}
}
