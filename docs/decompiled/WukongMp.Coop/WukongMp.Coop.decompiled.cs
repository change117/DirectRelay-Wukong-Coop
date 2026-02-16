using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ArchiveB1;
using B1UI;
using B1UI.GSSvc;
using B1UI.GSUI;
using BtlB1;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using CommB1;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GSE.GSUI;
using Google.Protobuf.Collections;
using HarmonyLib;
using LiteNetLib;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using PreludeLib.Attributes;
using PreludeLib.Common;
using PreludeLib.Compat;
using PreludeLib.Runtime.Public;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Multiplayer.Client;
using ReadyM.Api.Multiplayer.Client.Blobs;
using ReadyM.Api.Multiplayer.Common;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Managers;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Relay.Client;
using ReadyM.Relay.Client.State;
using ReadyM.Relay.Common.ECS.Jobs;
using ReadyM.Relay.Common.Serialization;
using ReadyM.Relay.Common.Wukong.ECS.Components;
using ResB1;
using UnrealEngine.Engine;
using UnrealEngine.Runtime;
using UnrealEngine.UMG;
using WukongMp.Api;
using WukongMp.Api.Command;
using WukongMp.Api.Configuration;
using WukongMp.Api.ECS.Archetypes;
using WukongMp.Api.ECS.Components;
using WukongMp.Api.ECS.Entities;
using WukongMp.Api.ECS.Jobs;
using WukongMp.Api.ECS.Managers;
using WukongMp.Api.FreeCamera;
using WukongMp.Api.Resources;
using WukongMp.Api.Shim;
using WukongMp.Api.State;
using WukongMp.Api.UI;
using WukongMp.Api.WukongUtils;
using WukongMp.Coop.Command;
using WukongMp.Coop.Configuration;
using WukongMp.Coop.ECS.Systems;
using WukongMp.Coop.Gamemode;
using WukongMp.Coop.UI;
using b1;
using b1.BGW;
using b1.ECS;
using b1.UI;
using b1.UI.Comm;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("WukongMp.Coop")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+fb95a1d4a24c1121439135db870127b86691267e")]
[assembly: AssemblyProduct("WukongMp.Coop")]
[assembly: AssemblyTitle("WukongMp.Coop")]
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
namespace WukongMp.Coop
{
	internal class CoopDI
	{
		public static CoopDI Instance { get; } = new CoopDI();

		public DI DI { get; private set; }

		public CoopCommandConsole CoopCommandConsole { get; private set; }

		public CoopGameplayConfiguration GameplayConfiguration { get; private set; }

		public CoopSynchronizer Synchronizer { get; private set; }

		public CoopSaveManager SaveManager { get; private set; }

		public CoopWidgetManager WidgetManager { get; private set; }

		public WukongPatcher Patcher { get; private set; }

		public CoopMode Coop { get; private set; }

		public void Init(DI wukongDI)
		{
			wukongDI.Logger.LogDebug("Initializing Coop DI...");
			DI = wukongDI;
			WukongPatcher wukongPatcher = (Patcher = new CoopPatcher(DI.Prelude));
			CoopCommandConsole coopCommandConsole = (CoopCommandConsole = new CoopCommandConsole(DI.CommandConsole));
			CoopGameplayConfiguration coopGameplayConfiguration = (GameplayConfiguration = new CoopGameplayConfiguration(DI.GameplayConfiguration));
			CoopSaveManager coopSaveManager = (SaveManager = new CoopSaveManager(DI.Logger));
			CoopWidgetManager coopWidgetManager = (WidgetManager = new CoopWidgetManager(DI.WidgetManager, DI.State, DI.PlayerState, DI.EventBus, DI.FreeCameraManager, DI.AreaState, DI.GameplayEventRouter));
			CoopMode coopMode = (Coop = new CoopMode(DI.Serializer, DI.RelayClient, DI.AreaState, DI.PlayerState, DI.PawnState, DI.GameplayEventRouter));
			CoopSynchronizer coopSynchronizer = (Synchronizer = new CoopSynchronizer(DI.ArchetypeEvent, DI.State, DI.ArchetypeRegistration, DI.World, DI.AreaState, DI.PlayerState, DI.PlayerPawnState, DI.ModeManager, DI.NetEntity, DI.ClientOwnership, DI.JobRegistry, DI.NetComponentRegistry, DI.RelayClient, DI.EcsLoop, DI.EventBus, DI.WidgetManager, DI.Rpc, DI.GameplayEventRouter, DI.GameplayConfiguration, DI.ColliderDisableData, DI.FreeCameraManager, DI.FreeCameraController, DI.Logger));
		}
	}
	public class CoopPatcher : WukongPatcher
	{
		public CoopPatcher(RuntimePrelude prelude)
			: base(prelude)
		{
		}

		protected override void OnPatch()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			base.OnPatch();
			Prelude.ScanAndPatchCategory(Assembly.GetExecutingAssembly(), new Category("Global"));
			Logging.LogInformation("Patched Prelude category: {Category}", "Global");
			Prelude.ScanAndPatchCategory(Assembly.GetExecutingAssembly(), new Category("Connected"));
			Logging.LogInformation("Patched Prelude category: {Category}", "Connected");
		}

		protected override void OnUnpatch()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Prelude.UnpatchCategory(Assembly.GetExecutingAssembly(), new Category("Connected"));
			Logging.LogInformation("Unpatched Prelude category: {Category}", "Connected");
			Prelude.UnpatchCategory(Assembly.GetExecutingAssembly(), new Category("Global"));
			Logging.LogInformation("Unpatched Prelude category: {Category}", "Global");
			base.OnUnpatch();
		}
	}
	public class CoopSynchronizer : WukongSynchronizer
	{
		private readonly SystemGroup _modeGroup;

		private readonly WukongPlayerState PlayerState;

		public CoopSynchronizer(ArchetypeEventRouter archetypeEvent, ClientState state, ClientWukongArchetypeRegistration wukongArchetype, Store world, WukongAreaState areaState, WukongPlayerState playerState, WukongPlayerPawnState playerPawnState, WukongPlayerModeManager modeManager, NetworkedEntityManager netManager, ClientOwnershipManager clientOwnership, JobRegistry jobRegistry, INetworkedComponentRegistry netComponentRegistry, IRelayClient relayClient, IClientEcsUpdateLoop ecsLoop, WukongEventBus eventBus, WukongWidgetManager widgetManager, WukongRpcCallbacks rpc, GameplayEventRouter gameplayEventRouter, GameplayConfiguration configuration, ColliderDisableData colliderDisableData, FreeCameraManager freeCameraManager, FreeCameraController freeCameraController, ILogger logger)
			: base(archetypeEvent, state, wukongArchetype, world, areaState, playerState, playerPawnState, modeManager, netManager, clientOwnership, jobRegistry, netComponentRegistry, relayClient, ecsLoop, eventBus, widgetManager, gameplayEventRouter, configuration, freeCameraManager, freeCameraController, logger)
		{
			State.OnJoinedArea += OnJoinedAreaHandler;
			JobRegistry.OnApplySnapshot += OnApplySnapshot;
			PlayerPawnState.OnPlayerPawnSpawned += OnPlayerPawnSpawned;
			PlayerState = playerState;
			PlayerState.OnMainCharacterEntityInitialized += OnMainCharacterEntityInitialized;
			_modeGroup = new SystemGroup("Coop");
			_modeGroup.Add(new ScaleMonsterHpSystem());
			_modeGroup.Add(new ReEnableCollidersSystem(colliderDisableData, eventBus));
			_modeGroup.Add(new RespawnMainCharacterSystem(areaState, playerState, rpc, Logger));
			_modeGroup.Add(new FixYellowbrowSystem(areaState, playerState));
			_modeGroup.Add(new DetectSoftlockSystem(areaState, playerState, widgetManager, Logger));
			_modeGroup.SetMonitorPerf(enable: true);
			base.EcsLoop.AddSystem(_modeGroup);
		}

		protected override void OnDispose()
		{
			State.OnJoinedArea -= OnJoinedAreaHandler;
			JobRegistry.OnApplySnapshot -= OnApplySnapshot;
			PlayerPawnState.OnPlayerPawnSpawned -= OnPlayerPawnSpawned;
			PlayerState.OnMainCharacterEntityInitialized -= OnMainCharacterEntityInitialized;
			base.EcsLoop.RemoveSystem(_modeGroup);
			base.OnDispose();
		}

		private void OnPlayerPawnSpawned(MainCharacterEntity mainCharacterEntity, BGUCharacterCS pawn)
		{
			if ((UObject)(object)MarkerUtils.CreateMarkerForCharacter(mainCharacterEntity, "(R=0.9,G=0.9,B=0.9)") == (UObject)null)
			{
				Logger.LogError("Failed to create marker for player {PlayerId}.", mainCharacterEntity.GetState().CharacterNickName);
			}
		}

		private void OnMainCharacterEntityInitialized(MainCharacterEntity mainCharacterEntity)
		{
			BGUIntervalArea[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<BGUIntervalArea>((UObject)(object)GameUtils.GetWorld());
			for (int i = 0; i < allActorsOfClass.Length; i++)
			{
				BUS_IntervalTriggerImpl component = ((BGUActorBaseCS)(object)allActorsOfClass[i]).GetComponent<BUS_IntervalTriggerImpl>();
				if (component != null)
				{
					bool beguilingChantEligible = ((BUS_StateMachineCompBase)component).CurrentState is IntervalTriggerEnableState;
					mainCharacterEntity.GetState().BeguilingChantEligible = beguilingChantEligible;
					break;
				}
			}
		}

		private void OnJoinedAreaHandler(AreaId areaId, Entity entity)
		{
			bool isMasterClient = AreaState.IsMasterClient;
			Logger.LogInformation("Joined area {AreaId}, is master client: {IsMasterClient}", areaId, isMasterClient);
			if (isMasterClient)
			{
				TamerUtils.DiscoverTamers();
			}
		}

		private void OnApplySnapshot()
		{
			World.Query<LocalTamerComponent, MetadataComponent>().Each(default(DiscoverLocallySpawnedMonsters));
		}
	}
	public class Mod : ICSharpModExV2, ICSharpModEx, ICSharpMod
	{
		private ILogger _logger;

		public string Name => "WukongMp co-op";

		public string Version => "1.0.0";

		public bool IsDebug => false;

		public void SetLoggerFactory(ILoggerFactory loggerFactory)
		{
			DI.Instance.InitLogging(loggerFactory);
			_logger = DI.Instance.Logger;
		}

		public void Init()
		{
			if (!LaunchParameters.Instance.Valid)
			{
				_logger.LogError("Multiplayer is disabled. Launch the game through the ReadyM Launcher to play WukongMP.");
				return;
			}
			if (!LaunchParameters.Instance.ValidForCoOp)
			{
				_logger.LogDebug("Co-op not launching.");
				return;
			}
			DI.Instance.Init();
			CoopDI.Instance.Init(DI.Instance);
			if (LaunchParameters.Instance.PlayShimOnStart)
			{
				ShimUtils.InitRelayPlayShim(DI.Instance, LaunchParameters.Instance.PlayShimFile);
			}
			else if (LaunchParameters.Instance.RecordShimOnStart)
			{
				ShimUtils.InitRelayRecordShim(DI.Instance, LaunchParameters.Instance.ServerIp, LaunchParameters.Instance.ServerPort.Value, LaunchParameters.Instance.UserGuid, noDisconnect: false, LaunchParameters.Instance.RecordShimFile);
			}
			else
			{
				ShimUtils.InitRelay(DI.Instance, LaunchParameters.Instance.ServerIp, LaunchParameters.Instance.ServerPort.Value, LaunchParameters.Instance.UserGuid, noDisconnect: false);
			}
			if (!CoopDI.Instance.Patcher.IsPatched)
			{
				CoopDI.Instance.Patcher.Patch();
			}
		}

		public void LateInit()
		{
			if (!LaunchParameters.Instance.Valid)
			{
				_logger.LogError("Multiplayer is disabled. Launch the game through the ReadyM Launcher to play WukongMP.");
				return;
			}
			if (!LaunchParameters.Instance.ValidForCoOp)
			{
				_logger.LogDebug("Co-op not launching.");
				return;
			}
			_logger.LogInformation("Init WukongMP mod");
			DebugUtils.LogUe4SsPresence();
			string text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "";
			_logger.LogInformation("Mod version: {Version}", text);
			_logger.LogInformation("Process name: {ProcessName}", Process.GetCurrentProcess().ProcessName);
			DI.Instance.WidgetManager.SetModVersion(text);
			Utils.TryRunOnGameThread((Action)delegate
			{
				if (!DI.Instance.Connection.IsRunning)
				{
					DI.Instance.EcsLoop.Start();
					DI.Instance.Connection.Start();
					if (!DI.Instance.Connection.RequestedConnect)
					{
						DI.Instance.Connection.Connect();
					}
				}
				else
				{
					_logger.LogError("WukongMP is already initialized");
				}
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)116, (Action)delegate
			{
				_logger.LogDebug("F5: Toggle debug widget visibility");
				DI.Instance.WidgetManager.ToggleDebugVisibility();
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)75, (Action)delegate
			{
				_logger.LogDebug("K");
				if (DI.Instance.WukongInputManager.CanApplyInput())
				{
					DI.Instance.WidgetManager.ToggleChatVisibility();
				}
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)112, (Action)delegate
			{
				_logger.LogDebug("F1");
				if (DI.Instance.WukongInputManager.CanApplyInput())
				{
					DI.Instance.WidgetManager.ToggleCommandVisibility();
				}
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)38, (Action)delegate
			{
				_logger.LogDebug("UP");
				DI.Instance.WidgetManager.CommandSelectUp();
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)40, (Action)delegate
			{
				_logger.LogDebug("DOWN");
				DI.Instance.WidgetManager.CommandSelectDown();
			});
			DI.Instance.InputManager.RegisterKeyBind((ModifierKeys)1, (Key)40, (Action)delegate
			{
				_logger.LogDebug("ALT + DOWN");
				DI.Instance.WidgetManager.CommandHistoryDown();
			});
			DI.Instance.InputManager.RegisterKeyBind((ModifierKeys)1, (Key)38, (Action)delegate
			{
				_logger.LogDebug("ALT + UP");
				DI.Instance.WidgetManager.CommandHistoryUp();
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)9, (Action)delegate
			{
				_logger.LogDebug("TAB");
				DI.Instance.WidgetManager.CommandSelectSuggestion();
			});
			DI.Instance.InputManager.RegisterKeyBind((Key)13, (Action)delegate
			{
				_logger.LogDebug("ENTER");
				DI.Instance.WukongInputManager.HandleEnterPressed();
			});
		}

		public void DeInit()
		{
			_logger.LogInformation("DeInit");
			if (!LaunchParameters.Instance.ValidForCoOp)
			{
				return;
			}
			Utils.TryRunOnGameThread((Action)delegate
			{
				if (DI.Instance.Connection.RequestedConnect)
				{
					DI.Instance.Connection.Disconnect();
				}
				if (DI.Instance.Connection.IsRunning)
				{
					DI.Instance.Connection.Stop();
					DI.Instance.EcsLoop.Stop();
				}
				if (CoopDI.Instance.Patcher.IsPatched)
				{
					CoopDI.Instance.Patcher.Unpatch();
				}
			});
		}

		public object GetReloadContext()
		{
			_logger.LogInformation("GetReloadContext");
			return DI.Instance.AreaState.InRoom;
		}

		public void Reload(object? context)
		{
			_logger.LogInformation("Reload");
			if (context as bool? == true)
			{
				_logger.LogInformation("Reconnecting after a reload");
				DI.Instance.Connection.Reconnect();
			}
		}
	}
}
namespace WukongMp.Coop.UI
{
	internal class CoopWidgetManager : IDisposable
	{
		private readonly ClientState _clientState;

		private readonly WukongPlayerState _playerState;

		private readonly WukongEventBus _eventBus;

		private readonly FreeCameraManager _freeCameraManager;

		private readonly WukongWidgetManager _widgetManager;

		private readonly WukongAreaState _areaState;

		private readonly GameplayEventRouter _eventRouter;

		private readonly Lazy<CoopStatusWidget> _coopStatusWidget = new Lazy<CoopStatusWidget>();

		public CoopWidgetManager(WukongWidgetManager widgetManager, ClientState clientState, WukongPlayerState playerState, WukongEventBus eventBus, FreeCameraManager freeCameraManager, WukongAreaState areaState, GameplayEventRouter eventRouter)
		{
			_widgetManager = widgetManager;
			_clientState = clientState;
			_playerState = playerState;
			_eventBus = eventBus;
			_freeCameraManager = freeCameraManager;
			_areaState = areaState;
			_eventRouter = eventRouter;
			_clientState.OnJoinedArea += OnJoinedArea;
			_clientState.OnLeftArea += OnLeftArea;
			_clientState.OnOtherPlayerInsideArea += OnOtherPlayerInsideArea;
			_clientState.OnOtherPlayerOutsideArea += OnOtherPlayerOutsideArea;
			_clientState.OnConnected += OnConnected;
			_clientState.OnDisconnected += OnDisconnected;
			_eventBus.OnLevelLoaded += OnLevelLoaded;
			_eventBus.OnExitLevel += OnExitLevel;
			_eventBus.OnLoadingScreenClose += OnLoadingScreenClose;
			_freeCameraManager.OnFreeCameraModeChanged += OnFreeCameraModeChanged;
			_eventRouter.OnPlayerChangedTeam += UpdatePlayerTeam;
			_eventRouter.OnLocalPlayerBeforeRebirth += OnLocalPlayerBeforeRebirth;
		}

		public void Dispose()
		{
			_clientState.OnJoinedArea -= OnJoinedArea;
			_clientState.OnLeftArea -= OnLeftArea;
			_clientState.OnOtherPlayerInsideArea -= OnOtherPlayerInsideArea;
			_clientState.OnOtherPlayerOutsideArea -= OnOtherPlayerOutsideArea;
			_clientState.OnConnected -= OnConnected;
			_clientState.OnDisconnected -= OnDisconnected;
			_eventBus.OnLevelLoaded -= OnLevelLoaded;
			_eventBus.OnExitLevel -= OnExitLevel;
			_eventBus.OnLoadingScreenClose -= OnLoadingScreenClose;
			_freeCameraManager.OnFreeCameraModeChanged -= OnFreeCameraModeChanged;
			_eventRouter.OnPlayerChangedTeam -= UpdatePlayerTeam;
			_eventRouter.OnLocalPlayerBeforeRebirth -= OnLocalPlayerBeforeRebirth;
		}

		public void UpdatePlayerTeam(PlayerEntity playerEntity, MainCharacterEntity mainCharacterEntity)
		{
			ref PlayerComponent state = ref playerEntity.GetState();
			_coopStatusWidget.Value.RemovePlayer(state.NickName);
			_coopStatusWidget.Value.AddPlayer(state.NickName);
			RefreshWidgets();
		}

		public void ShowInGameWidgets()
		{
			_coopStatusWidget.Value.SetVisibility(visible: true);
			_coopStatusWidget.Value.SetMaxConnectedCount(10);
		}

		private void OnLevelLoaded()
		{
			_widgetManager.OnLevelLoaded();
			Logging.LogDebug("Initializing pvp widgets");
			InitializeWidgets();
		}

		private void OnExitLevel()
		{
			Logging.LogDebug("Deinitializing pvp widgets");
			DeinitializeWidgets();
			_widgetManager.OnExitLevel();
		}

		private void OnLoadingScreenClose()
		{
			bool hasValue = _areaState.CurrentArea.HasValue;
			_widgetManager.ShowInGameWidgets(hasValue);
			if (hasValue)
			{
				ShowInGameWidgets();
			}
		}

		private void OnConnected(PlayerId playerId, Entity entity)
		{
			_widgetManager.OnConnected(playerId, entity);
		}

		private void OnDisconnected(PlayerId playerId, Entity? entity, DisconnectReason reason)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_widgetManager.OnDisconnected(playerId, entity, reason);
		}

		private void OnFreeCameraModeChanged(bool enabled)
		{
			_widgetManager.OnFreeCameraModeChanged(enabled);
		}

		private void InitializeWidgets()
		{
			_coopStatusWidget.Value.Initialize();
		}

		private void DeinitializeWidgets()
		{
			_coopStatusWidget.Value.Deinitialize();
		}

		public void RefreshWidgets()
		{
			_coopStatusWidget.Value.SetConnectedCount(_clientState.AreaPlayers.Count);
			_coopStatusWidget.Value.SetMaxConnectedCount(10);
		}

		private void OnLocalPlayerBeforeRebirth()
		{
			_widgetManager.HideInfoMessage();
		}

		private void OnOtherPlayerInsideArea(PlayerId playerId, AreaId area, OtherPlayerInsideAreaReason reason)
		{
			_widgetManager.OnOtherPlayerInsideArea(playerId, area, reason);
			PlayerEntity? playerById = _playerState.GetPlayerById(playerId);
			if (playerById.HasValue)
			{
				string nickName = playerById.Value.GetState().NickName;
				_coopStatusWidget.Value.AddPlayer(nickName);
				RefreshWidgets();
			}
		}

		private void OnOtherPlayerOutsideArea(PlayerId arg1, AreaId arg2, OtherPlayerOutsideAreaReason arg3)
		{
			_widgetManager.OnOtherPlayerOutsideArea(arg1, arg2, arg3);
			PlayerEntity? playerById = _playerState.GetPlayerById(arg1);
			if (playerById.HasValue)
			{
				string nickName = playerById.Value.GetState().NickName;
				_coopStatusWidget.Value.RemovePlayer(nickName);
				RefreshWidgets();
			}
		}

		private void OnJoinedArea(AreaId area, Entity areaEntity)
		{
			_widgetManager.OnJoinedArea(area, areaEntity);
			PlayerEntity? localPlayerEntity = _playerState.LocalPlayerEntity;
			if (localPlayerEntity.HasValue)
			{
				_coopStatusWidget.Value.AddPlayer(localPlayerEntity.Value.GetState().NickName);
				RefreshWidgets();
			}
		}

		private void OnLeftArea(AreaId arg1, Entity arg2)
		{
			_widgetManager.OnLeftArea(arg1, arg2);
			PlayerEntity? localPlayerEntity = _playerState.LocalPlayerEntity;
			if (localPlayerEntity.HasValue)
			{
				_coopStatusWidget.Value.RemovePlayer(localPlayerEntity.Value.GetState().NickName);
				RefreshWidgets();
			}
		}
	}
}
namespace WukongMp.Coop.Patches
{
	[HarmonyPatch(typeof(GSWindowsPlatformSaveGame), "GetFileFullName")]
	[HarmonyPatchCategory("Global")]
	public class PatchWindowsSaveGame
	{
		public static bool Prefix(ref string __result, string SlotName, string UserId)
		{
			if (!CoopDI.Instance.SaveManager.ShouldRedirectSaveFiles)
			{
				return true;
			}
			if (!SlotName.StartsWith("ArchiveSaveFile"))
			{
				return true;
			}
			Assembly assembly = typeof(PatchWindowsSaveGame).Assembly;
			__result = GameSaveUtils.GetSaveFileFullName(assembly, SlotName);
			return false;
		}
	}
	[HarmonyPatch(typeof(GSB1UIUtil), "StartNewGame")]
	[HarmonyPatchCategory("Global")]
	public class PatchStartNewGame
	{
		public static bool Prefix(UObject WorldContext)
		{
			CoopDI.Instance.SaveManager.OnNewGameLoad(WorldContext);
			return false;
		}
	}
	[HarmonyPatch(typeof(BGW_GameArchiveMgr), "LoadArchive")]
	[HarmonyPatchCategory("Global")]
	public class PatchGameArchive
	{
		public static void Postfix(BGW_GameArchiveMgr __instance, ref ReadArchiveResult __result, int ArchiveId, ref FUStBEDArchivesData? OutArchiveData)
		{
			if (__result)
			{
				Logging.LogError("Original readArchiveData Failed, Result: {Result}", __result);
			}
			else if (OutArchiveData == null)
			{
				Logging.LogError("Original OutArchiveData is null");
			}
			else
			{
				DI.Instance.EventBus.TryInvokeBeginLoadGameplayLevel();
				CoopDI.Instance.SaveManager.OnLoadArchive(__instance, ref __result, ArchiveId, ref OutArchiveData);
			}
		}
	}
	[HarmonyPatch(typeof(GSWindowsPlatformSaveGame), "SaveDataToSlot")]
	[HarmonyPatchCategory("Global")]
	public class PatchGSWindowsPlatformSaveGame
	{
		private static bool Prefix(List<byte> InSaveData, string SlotName, string UserId, ref bool __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!SlotName.StartsWith("ArchiveSaveFile"))
			{
				return true;
			}
			CoopDI.Instance.SaveManager.OnSaveData(InSaveData, SlotName);
			__result = true;
			return false;
		}
	}
	[HarmonyPatch(typeof(BGW_GameArchiveMgr), "GetLatestArchive")]
	[HarmonyPatchCategory("Global")]
	public class PatchGetLatestArchive
	{
		public static bool Prefix(BGW_GameArchiveMgr __instance, ref ArchiveSummaryData? __result)
		{
			ArchiveSummaryData val = null;
			List<ArchiveSummaryData> list = (List<ArchiveSummaryData>)AccessTools.Method(typeof(BGW_GameArchiveMgr), "_GetArchiveInfoList", (Type[])null, (Type[])null).Invoke(__instance, Array.Empty<object>());
			for (int i = 0; i < list.Count; i++)
			{
				if (val == null || list[i].ArchiveId > val.ArchiveId)
				{
					val = list[i];
				}
			}
			__result = ((val != null) ? val.Clone() : null);
			return false;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Global")]
	public static class PatchStartGameUiCoop
	{
		[HarmonyTargetMethodHint("B1UI.GSUI.UIStartGame", "OnUIPageConstructImpl", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("B1UI.GSUI.UIStartGame:OnUIPageConstructImpl", (Type[])null, (Type[])null);
		}

		public static void Postfix(GSUIView __instance, ref List<VIButtonBaseV2> ___StartGameBtnList, ref UTextBlock ___TxtMainName, ref UTextBlock ___TxtSubName, DSStartGame ___DataStore)
		{
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			for (int num = ___DataStore.BtnDataList.Count - 1; num >= 0; num--)
			{
				DSButtonBase val = ___DataStore.BtnDataList[num];
				Logging.LogDebug("Button name: {Name}, id: {Id}", val.Name.Value, val.Id.Value);
				string text = ((object)val.Name.Value).ToString();
				if (text == ((object)GSB1UIUtil.GetUIWordDescFText((EUIWordID)725)).ToString())
				{
					if ((UObject)(object)BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UClass>("/Game/Mods/WukongMod/BP_PlayerMarker.BP_PlayerMarker_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1)) == (UObject)null)
					{
						((UWidget)___StartGameBtnList[num].GetBUIButton()).SetVisibility((ESlateVisibility)1);
						___StartGameBtnList.RemoveAt(num);
						UiUtils.ShowTip(Texts.MissingPak, autoHide: false);
						Logging.LogError("WukongMP.pak is not loaded. Could not continue game.");
					}
					else if (!DI.Instance.State.IsConnected)
					{
						((UWidget)___StartGameBtnList[num].GetBUIButton()).SetVisibility((ESlateVisibility)1);
						___StartGameBtnList.RemoveAt(num);
						DI.Instance.RelayClient.Scheduler.Schedule(delegate(IRelayClientNetworkThreadContext ctx)
						{
							Utils.TryRunOnGameThread((Action)delegate
							{
								//IL_0010: Unknown result type (might be due to invalid IL or missing references)
								//IL_0016: Invalid comparison between Unknown and I4
								DI.Instance.WidgetManager.ShowInfoMessage(((int)ctx.LastDisconnectReason == 6) ? Texts.ConnectionRejectedByServer : Texts.Disconnected);
							});
						});
						Logging.LogError("Disconnected. Could not continue game.");
					}
					else
					{
						Logging.LogDebug("New game UI name desc: {Description}", GSB1UIUtil.GetUIWordDescFText((EUIWordID)725));
						___StartGameBtnList[num].SetTxtName(GSB1UIUtil.GetUIWordDescFText((EUIWordID)732));
					}
				}
				else if (text != ((object)GSB1UIUtil.GetUIWordDescFText((EUIWordID)727)).ToString() && text != ((object)GSB1UIUtil.GetUIWordDescFText((EUIWordID)726)).ToString() && text != ((object)GSB1UIUtil.GetUIWordDescFText((EUIWordID)728)).ToString())
				{
					Logging.LogDebug("UI name desc to hide: {Description}", GSB1UIUtil.GetUIWordDescFText((EUIWordID)727));
					((UWidget)___StartGameBtnList[num].GetBUIButton()).SetVisibility((ESlateVisibility)1);
					___StartGameBtnList.RemoveAt(num);
				}
			}
			__instance.GSAnimKeyToState("GSAKBContinueBtn", "CBtnFocus", false, (BUI_Widget)null, 0f, -1f);
			___TxtMainName.SetText(FText.FromString(""));
			___TxtSubName.SetText(FText.FromString("Wukong Multiplayer Mod"));
			((UWidget)___TxtSubName).SetRenderScale(new FVector2D(1.2, 1.2));
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Global")]
	public class PatchShrineRegisterFunc
	{
		[HarmonyTargetMethodHint(typeof(FMenuHelper<EShrineMenuTag>), "RegisterFunc", new Type[] { })]
		public static MethodBase TargetMethod()
		{
			return typeof(FMenuHelper<EShrineMenuTag>).GetMethod("RegisterFunc");
		}

		public static bool Prefix(int FuncId)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Invalid comparison between Unknown and I4
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			InteractionFuncDesc interactionFuncDesc = GameDBRuntime.GetInteractionFuncDesc(FuncId);
			if ((int)interactionFuncDesc.MenuBtnActionType != 24)
			{
				return (int)interactionFuncDesc.MenuBtnActionType != 23;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUI_BattleInfoCS), "InitBloodBarUI")]
	[HarmonyPatchCategory("Global")]
	public class PatchInitBloodBarUI
	{
		public static bool Prefix(BUI_BattleInfoCS __instance, Dictionary<Entity, BUI_ProjWidget> ___EntityDic, Dictionary<AActor, DSBarInfoBind> ___BloodBarActorBindDict, Entity Entity)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Invalid comparison between Unknown and I4
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Invalid comparison between Unknown and I4
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			if (___EntityDic.ContainsKey(Entity))
			{
				return false;
			}
			AActor val = ECSExtension.ToActor(Entity);
			BGUCharacterCS val2 = (BGUCharacterCS)(object)((val is BGUCharacterCS) ? val : null);
			if ((UObject)(object)val2 == (UObject)null)
			{
				return false;
			}
			FUStUnitCommDesc unitCommDesc = BGW_GameDB.GetUnitCommDesc(val2.GetResID());
			if (unitCommDesc == null)
			{
				return false;
			}
			FUStUnitBattleInfoExtendDesc unitBattleInfoExtendDesc = BGW_GameDB.GetUnitBattleInfoExtendDesc(val2.GetFinalBattleInfoExtendID());
			if (unitBattleInfoExtendDesc == null)
			{
				return false;
			}
			bool hasValue = DI.Instance.PawnState.GetEntityByPlayerPawn(val).HasValue;
			EBGUBloodBarShowType val3 = (EBGUBloodBarShowType)(hasValue ? 1 : 2);
			bool flag = !hasValue && BGU_DataUtil.GetIsInPlayerTeam(val);
			if ((int)unitBattleInfoExtendDesc.BloodBarType == 0 || flag)
			{
				return false;
			}
			BUI_ProjWidget topBarPoolWidget = __instance.GetTopBarPoolWidget(val2, true);
			BUI_MBarBase val4 = (BUI_MBarBase)(object)((topBarPoolWidget is BUI_MBarBase) ? topBarPoolWidget : null);
			if (val4 != null)
			{
				val4.InitBloodBar(unitBattleInfoExtendDesc.BloodBarType, unitCommDesc.HPBarHeightOffset);
			}
			if ((UObject)(object)val4 != (UObject)null)
			{
				if ((int)val3 == 1)
				{
					((BUI_ProjWidget)val4).SetAlwaysShowSetting((AlwaysShowSetting)1, true, -1f);
				}
				___EntityDic.Add(Entity, (BUI_ProjWidget)(object)val4);
			}
			if (!___EntityDic.ContainsKey(Entity) || !___BloodBarActorBindDict.TryGetValue(val, out var value))
			{
				return false;
			}
			((DSBarInfoBind)(ref value)).ReInit();
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_UnitBarInfoComp), "ShowEnemyBar")]
	[HarmonyPatchCategory("Global")]
	public class PatchShowEnemyBar
	{
		public static bool Prefix(BUS_UnitBarInfoComp __instance, ref bool __result)
		{
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (owner is BGUPlayerCharacterCS)
			{
				__result = !BGUFunctionLibraryCS.BGUHasUnitSimpleState(owner, (EBGUSimpleState)102);
				return false;
			}
			return true;
		}
	}
}
namespace WukongMp.Coop.Gamemode
{
	public class CoopMode : IDisposable
	{
		protected readonly RelaySerializer Serializer;

		protected readonly IRelayClient RelayClient;

		private readonly WukongAreaState _areaState;

		private readonly WukongPlayerState _playerState;

		private readonly WukongPawnState _pawnState;

		private readonly GameplayEventRouter _eventRouter;

		public CoopMode(RelaySerializer serializer, IRelayClient relayClient, WukongAreaState areaState, WukongPlayerState playerState, WukongPawnState pawnState, GameplayEventRouter eventRouter)
		{
			Serializer = serializer;
			RelayClient = relayClient;
			_areaState = areaState;
			_playerState = playerState;
			_pawnState = pawnState;
			_eventRouter = eventRouter;
			_eventRouter.OnRebirthPointChanged += OnRebirthPointChanged;
		}

		public void Dispose()
		{
			_eventRouter.OnRebirthPointChanged -= OnRebirthPointChanged;
		}

		private void OnRebirthPointChanged(Entity entity, int rebirthPointId)
		{
			if (_pawnState.TryGetMainCharacterEntity(entity, out var _) && _playerState.LocalMainCharacter.HasValue && entity == _playerState.LocalMainCharacter.Value.Entity)
			{
				_playerState.LocalMainCharacter.Value.GetState().RebirthPointId = rebirthPointId;
			}
		}
	}
	internal class CoopSaveManager(ILogger logger)
	{
		public bool ShouldRedirectSaveFiles => true;

		public void OnNewGameLoad(UObject worldContext)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			GSGMSvc.ClearAllAutoRunTag();
			if (BGW_GameLifeTimeMgr.Get(worldContext).IsInFSMState((SGI_Global)1))
			{
				BGW_EventCollection.Get(worldContext).Evt_ResetGameInstanceData.Invoke((EGameInstanceResetType)2);
			}
			BGW_EventCollection.Get(worldContext).Evt_BGW_TriggerGlobalFSMEvent.Invoke((EGI_Global)3, (object)new FSMInputData_GI_Global_SubG_GI_Loading_TravelLevel
			{
				ArchiveId = 1
			});
		}

		public void OnLoadArchive(BGW_GameArchiveMgr __instance, ref ReadArchiveResult __result, int ArchiveId, ref FUStBEDArchivesData OutArchiveData)
		{
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			byte[] array = Array.Empty<byte>();
			byte[] bytes;
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Task<BlobInfo> task = DI.Instance.SaveRelay.DownloadWorldSaveAsync();
				Task<BlobInfo> task2 = DI.Instance.SaveRelay.DownloadPlayerSaveAsync();
				Task<BlobInfo[]> task3 = Task.WhenAll<BlobInfo>(task, task2);
				DI.Instance.EcsLoop.Wait(task3);
				stopwatch.Stop();
				logger.LogInformation("Downloaded world and player save files in {Time} ms", stopwatch.ElapsedMilliseconds);
				if (task.Result == null)
				{
					logger.LogInformation("Failed to download world save file from the cloud, will start new game");
					flag = true;
				}
				else
				{
					array = task.Result.Content;
				}
				if (task2.Result == null)
				{
					logger.LogInformation("Player has no save file in the cloud, using default world save");
					bytes = array;
				}
				else
				{
					bytes = task2.Result.Content;
				}
			}
			catch (Exception)
			{
				__result = (ReadArchiveResult)5;
				OutArchiveData = null;
				return;
			}
			ArchiveFileUnpacked val2 = default(ArchiveFileUnpacked);
			ArchiveFileUnpacked val4 = default(ArchiveFileUnpacked);
			if (flag)
			{
				EArchiveRepairStatus val3 = default(EArchiveRepairStatus);
				ReadArchiveResult val = __instance.ReadArchiveData(1, ref val2, ref val3);
				if ((int)val != 0)
				{
					logger.LogError("ReadArchiveData Failed, Result: {Result}", val);
					return;
				}
				val4 = val2;
			}
			else
			{
				File.WriteAllBytes(GSWindowsPlatformSaveGame.GetFileFullName(GSE_SaveGameUtil.GetArchiveSlotName((SaveFileType)0, 8), __instance.ArchiveWorker.UserId), array);
				File.WriteAllBytes(GSWindowsPlatformSaveGame.GetFileFullName(GSE_SaveGameUtil.GetArchiveSlotName((SaveFileType)0, 7), __instance.ArchiveWorker.UserId), bytes);
				EArchiveRepairStatus val6 = default(EArchiveRepairStatus);
				ReadArchiveResult val5 = __instance.ReadArchiveData(8, ref val2, ref val6);
				if ((int)val5 != 0)
				{
					logger.LogError("ReadArchiveData Failed, Result: {Result}", val5);
					return;
				}
				ReadArchiveResult val7 = __instance.ReadArchiveData(7, ref val4, ref val6);
				if ((int)val7 != 0)
				{
					logger.LogError("ReadArchiveData Failed, Result: {Result}", val7);
					return;
				}
			}
			OutArchiveData = val4.GameArchiveData;
			OutArchiveData.LevelArchiveData = val2.GameArchiveData.LevelArchiveData;
			OutArchiveData.PersistentECSData = val2.GameArchiveData.PersistentECSData;
			OutArchiveData.StateMachineArchiveData = val2.GameArchiveData.StateMachineArchiveData;
			OutArchiveData.TaskArchiveData = val2.GameArchiveData.TaskArchiveData;
			foreach (int spell in val2.GameArchiveData.RoleData.RoleCs.Actor.Progress.SpellList)
			{
				if (!OutArchiveData.RoleData.RoleCs.Actor.Progress.SpellList.Contains(spell))
				{
					OutArchiveData.RoleData.RoleCs.Actor.Progress.SpellList.Add(spell);
				}
			}
			Dictionary<SpellType, int> dictionary = new Dictionary<SpellType, int>(((IEnumerable<SpellItem>)val2.GameArchiveData.RoleData.RoleCs.Actor.Wear.SpellList).ToDictionary((SpellItem spell) => spell.Type, (SpellItem spell) => spell.SpellId));
			Dictionary<SpellType, int> dictionary2 = new Dictionary<SpellType, int>(((IEnumerable<SpellItem>)OutArchiveData.RoleData.RoleCs.Actor.Wear.SpellList).ToDictionary((SpellItem spell) => spell.Type, (SpellItem spell) => spell.SpellId));
			SpellType val8 = default(SpellType);
			int num = default(int);
			foreach (KeyValuePair<SpellType, int> item in dictionary)
			{
				CompatExtensions.Deconstruct<SpellType, int>(item, ref val8, ref num);
				SpellType val9 = val8;
				int num2 = num;
				if (num2 != 0)
				{
					if (dictionary2.TryGetValue(val9, out var value) && value == 0)
					{
						logger.LogDebug("Assigning spell ID {SpellId} to type {SpellType}", num2, val9);
						dictionary2[val9] = num2;
					}
					else if (!dictionary2.ContainsKey(val9))
					{
						logger.LogDebug("Adding spell ID {SpellId} to type {SpellType}", num2, val9);
						dictionary2.Add(val9, num2);
					}
				}
			}
			OutArchiveData.RoleData.RoleCs.Actor.Wear.SpellList.Clear();
			RepeatedField<SpellItem> spellList = OutArchiveData.RoleData.RoleCs.Actor.Wear.SpellList;
			List<SpellItem> list = new List<SpellItem>();
			list.AddRange(((IEnumerable<KeyValuePair<SpellType, int>>)dictionary2).Select((Func<KeyValuePair<SpellType, int>, SpellItem>)((KeyValuePair<SpellType, int> kvp) => new SpellItem
			{
				SpellId = kvp.Value,
				Type = kvp.Key
			})));
			spellList.AddRange((IEnumerable<SpellItem>)new <>z__ReadOnlyList<SpellItem>(list));
			foreach (int interactionFunc in val2.GameArchiveData.RoleData.RoleCs.Interaction.InteractionFuncList)
			{
				if (!OutArchiveData.RoleData.RoleCs.Interaction.InteractionFuncList.Contains(interactionFunc))
				{
					OutArchiveData.RoleData.RoleCs.Interaction.InteractionFuncList.Add(interactionFunc);
				}
			}
		}

		public void OnSaveData(List<byte> inSaveData, string slotName)
		{
			logger.LogInformation("Will upload save to the cloud, Slot: {SlotName}, Size: {Size} Mb", slotName, ((double)inSaveData.Count / 1048576.0).ToString("F2"));
			byte[] data = inSaveData.ToArray();
			Task.Run(async delegate
			{
				if (DI.Instance.AreaState.IsMasterClient)
				{
					Stopwatch worldTimer = Stopwatch.StartNew();
					bool success = await DI.Instance.SaveRelay.UploadWorldSaveAsync(data);
					LogSuccess(worldTimer, success, "world save");
				}
				Stopwatch playerTimer = Stopwatch.StartNew();
				bool success2 = await DI.Instance.SaveRelay.UploadPlayerSaveAsync(data);
				LogSuccess(playerTimer, success2, "player save");
			});
		}

		private void LogSuccess(Stopwatch stopwatch, bool success, string name)
		{
			stopwatch.Stop();
			if (success)
			{
				logger.LogInformation("Blob uploaded successfully: {Name} in {Time} ms", name, stopwatch.ElapsedMilliseconds);
			}
			else
			{
				logger.LogError("Failed to upload blob: {Name} in {Time} ms", name, stopwatch.ElapsedMilliseconds);
			}
		}
	}
}
namespace WukongMp.Coop.ECS.Systems
{
	public class DetectSoftlockSystem(WukongAreaState areaState, WukongPlayerState playerState, WukongWidgetManager widgetManager, ILogger logger) : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>()
	{
		protected override void OnUpdate()
		{
			if (!areaState.IsMasterClient)
			{
				return;
			}
			int players = 0;
			HashSet<int> waitingSequencesIds = new HashSet<int>();
			base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, ref MainCharacterComponent mainComp, Entity _)
			{
				if (localMainComp.HasPawn)
				{
					players++;
					if (localMainComp.IsWaitingForSequence)
					{
						waitingSequencesIds.Add(mainComp.WaitingSequenceId);
					}
				}
			});
			if (players == 0)
			{
				return;
			}
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				logger.LogWarning("Skipping respawn, no local main character entity");
				return;
			}
			ref LocalMainCharacterComponent localState = ref localMainCharacter.Value.GetLocalState();
			if (players > 0 && waitingSequencesIds.Count > 1 && !localState.IsRespawning)
			{
				logger.LogDebug("Softlock detected");
				widgetManager.ShowInfoMessage(Texts.SoftlockDetected);
			}
		}
	}
	public class FixYellowbrowSystem(WukongAreaState areaState, WukongPlayerState playerState) : QuerySystem<TamerComponent, LocalTamerComponent, HpComponent>()
	{
		protected override void OnUpdate()
		{
			if (!areaState.InRoom || !playerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			base.Query.ForEachEntity(delegate(ref TamerComponent tamer, ref LocalTamerComponent localTamer, ref HpComponent hp, Entity entity)
			{
				if (localTamer.IsMonsterActive && hp.Hp < 1f && tamer.Guid == "UGuid.LYS.HuangMei.Big" && playerState.LocalMainCharacter.Value.GetState().IsDead)
				{
					PlayerUtils.DisableSpectator(playerState.LocalMainCharacter.Value);
					PlayerUtils.RebirthPlayerInPlace(playerState.LocalMainCharacter.Value.GetLocalState().Pawn);
				}
			});
		}
	}
	public class ReEnableCollidersSystem(ColliderDisableData colliderDisableData, WukongEventBus eventBus) : BaseSystem()
	{
		private const float TickIntervalSeconds = 1f;

		private float _elapsedTime;

		protected override void OnUpdateGroup()
		{
			if (eventBus.IsGameplayLevel)
			{
				_elapsedTime += base.Tick.deltaTime;
				if (!(_elapsedTime < 1f))
				{
					colliderDisableData.TryReEnableColliders(_elapsedTime);
					_elapsedTime = 0f;
				}
			}
		}
	}
	public class RespawnMainCharacterSystem(WukongAreaState areaState, WukongPlayerState playerState, WukongRpcCallbacks rpc, ILogger logger) : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>()
	{
		protected override void OnUpdate()
		{
			if (!areaState.IsMasterClient)
			{
				return;
			}
			bool allDead = true;
			int players = 0;
			base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, ref MainCharacterComponent mainComp, Entity _)
			{
				if (localMainComp.HasPawn)
				{
					players++;
					bool num = allDead;
					MainCharacterComponent mainCharacterComponent = mainComp;
					allDead = num & (mainCharacterComponent.IsDead && !mainCharacterComponent.IsTransformed && !localMainComp.IsRespawning);
				}
			});
			if (players == 0)
			{
				return;
			}
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				logger.LogWarning("Skipping respawn, no local main character entity");
				return;
			}
			ref LocalMainCharacterComponent localState = ref localMainCharacter.Value.GetLocalState();
			if (players > 0 && allDead && !localState.IsRespawning)
			{
				logger.LogDebug("All {Players} players are dead, respawning player {Player}", players, playerState.LocalPlayerId);
				int maxComp = 0;
				base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent _, ref MainCharacterComponent mainComp, Entity _)
				{
					maxComp = Math.Max(maxComp, mainComp.RebirthPointId);
				});
				localState.IsRespawning = true;
				rpc.SendPartyRespawn(maxComp);
			}
		}
	}
	public class ScaleMonsterHpSystem : QuerySystem<HpComponent, LocalTamerComponent>
	{
		protected override void OnUpdate()
		{
			int areaPlayers = DI.Instance.State.AreaPlayers.Count;
			float targetScaling = 1f + 1.5f * (float)(areaPlayers - 1);
			base.Query.ForEachEntity(delegate(ref HpComponent hp, ref LocalTamerComponent localTamer, Entity entity)
			{
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Invalid comparison between Unknown and I4
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Invalid comparison between Unknown and I4
				//IL_0164: Unknown result type (might be due to invalid IL or missing references)
				if (localTamer.IsMonsterActive && DI.Instance.ClientOwnership.OwnsEntity(entity) && (!USharpExtensions.Equals(hp.Hp, 0f, 0.1f) || !USharpExtensions.Equals(hp.HpMaxBase, 0f, 0.1f)) && Math.Abs(targetScaling - hp.HpMultiplier) > 0.1f && !((UObject)(object)localTamer.Pawn == (UObject)null))
				{
					FUStUnitBattleInfoExtendDesc unitBattleInfoExtendDesc = BGW_GameDB.GetUnitBattleInfoExtendDesc(localTamer.Pawn.GetFinalBattleInfoExtendID());
					if (unitBattleInfoExtendDesc != null)
					{
						EBGUBloodBarType bloodBarType = unitBattleInfoExtendDesc.BloodBarType;
						if (((int)bloodBarType == 1 || (int)bloodBarType == 3) ? true : false)
						{
							BUC_AttrContainer readOnlyData = BGU_DataUtil.GetReadOnlyData<BUC_AttrContainer>((AActor)(object)localTamer.Pawn);
							if (readOnlyData == null)
							{
								DI.Instance.Logger.LogWarning("Failed to get AttrContainer for pawn {Pawn}", ((UObject)localTamer.Pawn).GetName());
							}
							else
							{
								float floatValue = readOnlyData.GetFloatValue((EBGUAttrFloat)151);
								float floatValue2 = readOnlyData.GetFloatValue((EBGUAttrFloat)101);
								hp.HpMaxBase = floatValue2 / hp.HpMultiplier * targetScaling;
								hp.Hp = floatValue / hp.HpMultiplier * targetScaling;
								readOnlyData.SetFloatValue((EBGUAttrFloat)101, hp.HpMaxBase);
								readOnlyData.SetFloatValue((EBGUAttrFloat)151, hp.Hp);
								hp.HpMultiplier = targetScaling;
								DI.Instance.Logger.LogDebug("Scaled {MonsterType} HP to {Hp}/{HpMaxBase} (x{Multiplier}) for {Players} players", bloodBarType, hp.Hp, hp.HpMaxBase, targetScaling, areaPlayers);
							}
						}
					}
				}
			});
		}
	}
}
namespace WukongMp.Coop.Configuration
{
	public static class CoopConstants
	{
		public const int CoopWorldArchiveId = 8;

		public const int CoopPlayerArchiveId = 7;
	}
	internal class CoopGameplayConfiguration
	{
		private readonly GameplayConfiguration _configuration;

		public CoopGameplayConfiguration(GameplayConfiguration configuration)
		{
			_configuration = configuration;
			ConfigureCoopGameplay();
		}

		public void ConfigureCoopGameplay()
		{
			_configuration.IsSupportMultiLockEnabled = true;
			_configuration.IsStrongDamageImmueEnabled = false;
			_configuration.EnableCustomCameraArmLength = false;
			_configuration.EnableSpawnedTamers = false;
			_configuration.SyncTamerTeamFromGameToEcs = true;
		}
	}
}
namespace WukongMp.Coop.Command
{
	internal class CoopCommandConsole
	{
		private readonly WukongCommandConsole _wukongCommandConsole;

		public CoopCommandConsole(WukongCommandConsole wukongCommandConsole)
		{
			Logging.LogDebug("Initializing CoopCommandConsole");
			_wukongCommandConsole = wukongCommandConsole;
			SetupCommands();
		}

		private void SetupCommands()
		{
		}

		private void PlayCutscene(ReadOnlyMemory<string> args)
		{
			if (args.Length == 1 && int.TryParse(args.Span[0], out var result))
			{
				GSG.GMSvc.GMTeleportToTargetSequence(result);
			}
		}

		private void Teleport(ReadOnlyMemory<string> args)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (args.Length == 1 && int.TryParse(args.Span[0], out var result))
			{
				BGUPlayerCharacterCS? controlledPawn = GameUtils.GetControlledPawn();
				BPS_EventCollectionCS.Get((controlledPawn != null) ? ((APawn)controlledPawn).PlayerState : null).Evt_BPS_TeleportTo.Invoke((ETeleportTypeV2)5, (ValueType)(object)new TeleportParam_RebirthPoint
				{
					RebirthPointId = result
				}, (EPlayerTeleportReason)2);
			}
		}

		private void OpenLevel(ReadOnlyMemory<string> args)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (args.Length == 1)
			{
				UGameplayStatics.OpenLevel((UObject)(object)GameUtils.GetWorld(), new FName(args.Span[0], (EFindName)1), true, (string)null);
			}
		}
	}
}
[CompilerGenerated]
internal sealed class <>z__ReadOnlyList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	int ICollection.Count => _items.Count;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	object IList.this[int index]
	{
		get
		{
			return _items[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	bool IList.IsFixedSize => true;

	bool IList.IsReadOnly => true;

	int IReadOnlyCollection<T>.Count => _items.Count;

	T IReadOnlyList<T>.this[int index] => _items[index];

	int ICollection<T>.Count => _items.Count;

	bool ICollection<T>.IsReadOnly => true;

	T IList<T>.this[int index]
	{
		get
		{
			return _items[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public <>z__ReadOnlyList(List<T> items)
	{
		_items = items;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_items).GetEnumerator();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		((ICollection)_items).CopyTo(array, index);
	}

	int IList.Add(object value)
	{
		throw new NotSupportedException();
	}

	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	bool IList.Contains(object value)
	{
		return ((IList)_items).Contains(value);
	}

	int IList.IndexOf(object value)
	{
		return ((IList)_items).IndexOf(value);
	}

	void IList.Insert(int index, object value)
	{
		throw new NotSupportedException();
	}

	void IList.Remove(object value)
	{
		throw new NotSupportedException();
	}

	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return ((IEnumerable<T>)_items).GetEnumerator();
	}

	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException();
	}

	void ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	bool ICollection<T>.Contains(T item)
	{
		return _items.Contains(item);
	}

	void ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		_items.CopyTo(array, arrayIndex);
	}

	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	int IList<T>.IndexOf(T item)
	{
		return _items.IndexOf(item);
	}

	void IList<T>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	void IList<T>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}
}
