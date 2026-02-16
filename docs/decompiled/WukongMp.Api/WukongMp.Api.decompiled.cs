using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using Friflo.Json.Fliox;
using GSDispLib;
using GSE.GSUI;
using HarmonyLib;
using HttpMachine;
using IHttpMachine.Model;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Tls.Crypto;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using Org.BouncyCastle.X509;
using PreludeLib.Attributes;
using PreludeLib.Common;
using PreludeLib.Compat;
using PreludeLib.Runtime.Backend;
using PreludeLib.Runtime.Backend.WeaverCallback;
using PreludeLib.Runtime.Public;
using ReadyM.Api;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Idents;
using ReadyM.Api.Multiplayer;
using ReadyM.Api.Multiplayer.Client;
using ReadyM.Api.Multiplayer.Client.Blobs;
using ReadyM.Api.Multiplayer.Common;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Api.Multiplayer.ECS.Managers;
using ReadyM.Api.Multiplayer.ECS.Registry;
using ReadyM.Api.Multiplayer.ECS.Values;
using ReadyM.Api.Multiplayer.Generators;
using ReadyM.Api.Multiplayer.Idents;
using ReadyM.Api.Multiplayer.Protocol;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.Api.Serialization;
using ReadyM.Relay.Client;
using ReadyM.Relay.Client.Blobs;
using ReadyM.Relay.Client.Host;
using ReadyM.Relay.Client.Serialization;
using ReadyM.Relay.Client.Shim;
using ReadyM.Relay.Client.State;
using ReadyM.Relay.Common.ECS.Archetypes;
using ReadyM.Relay.Common.ECS.Components;
using ReadyM.Relay.Common.ECS.Jobs;
using ReadyM.Relay.Common.ECS.Registry;
using ReadyM.Relay.Common.Serialization;
using ReadyM.Relay.Common.Shim;
using ReadyM.Relay.Common.Wukong.ECS.Components;
using ReadyM.Relay.Common.Wukong.ECS.Registry;
using ReadyM.Relay.Common.Wukong.ECS.Values;
using ReadyM.Relay.Common.Wukong.RPC;
using UnrealEngine;
using UnrealEngine.AssetRegistry;
using UnrealEngine.Engine;
using UnrealEngine.LevelSequence;
using UnrealEngine.MovieScene;
using UnrealEngine.Plugins.EnhancedInput;
using UnrealEngine.Plugins.Niagara;
using UnrealEngine.Runtime;
using UnrealEngine.UMG;
using WukongMp.Api;
using WukongMp.Api.Chat;
using WukongMp.Api.Command;
using WukongMp.Api.Compat;
using WukongMp.Api.Configuration;
using WukongMp.Api.DTO;
using WukongMp.Api.ECS.Archetypes;
using WukongMp.Api.ECS.Components;
using WukongMp.Api.ECS.Entities;
using WukongMp.Api.ECS.Jobs;
using WukongMp.Api.ECS.Managers;
using WukongMp.Api.ECS.Systems;
using WukongMp.Api.ECS.Systems.MainCharacters;
using WukongMp.Api.ECS.Systems.Tamers;
using WukongMp.Api.ECS.Values;
using WukongMp.Api.FreeCamera;
using WukongMp.Api.Helpers;
using WukongMp.Api.Https;
using WukongMp.Api.Input;
using WukongMp.Api.NameCompressors;
using WukongMp.Api.PathCompressors;
using WukongMp.Api.Resources;
using WukongMp.Api.Serialization;
using WukongMp.Api.Shim;
using WukongMp.Api.State;
using WukongMp.Api.Tests;
using WukongMp.Api.Tests.TestActionSequences;
using WukongMp.Api.Tests.TestActions;
using WukongMp.Api.UI;
using WukongMp.Api.Windows;
using WukongMp.Api.WukongUtils;
using b1;
using b1.BGU.BUAnim;
using b1.BGW;
using b1.ECS;
using b1.EventDelDefine;
using b1.GSMUI;
using b1.GSMUI.GSWidget;
using b1.Localization;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("0.0.0.0")]
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
[HarmonyPatch(typeof(BUS_JumpComp), "TriggerJumpSkill", new Type[]
{
	typeof(ESkillDirection),
	typeof(FVector2D)
})]
[HarmonyPatchCategory("Connected")]
public class PatchTriggerJumpSkill
{
	public static void Prefix(BUS_JumpComp __instance, ESkillDirection StartJumpDir, FVector2D CurrentInputVector)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (DI.Instance.AreaState.InRoom)
		{
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			WukongPlayerState playerState = DI.Instance.PlayerState;
			if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				DI.Instance.Rpc.SendStartJump(new StartJumpData(StartJumpDir, CurrentInputVector));
			}
		}
	}
}
[HarmonyPatch(typeof(BUS_JumpComp), "OnReleased")]
[HarmonyPatchCategory("Connected")]
public class PatchJumpOnReleased
{
	public static void Prefix(BUS_JumpComp __instance)
	{
		if (DI.Instance.AreaState.InRoom)
		{
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			WukongPlayerState playerState = DI.Instance.PlayerState;
			if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				DI.Instance.Rpc.SendStopJump();
			}
		}
	}
}
[HarmonyPatch(typeof(BUS_PlayerInputActionComp), "CheckCanSelectTarget")]
[HarmonyPatchCategory("Connected")]
public class PatchCheckCanSelectTarget
{
	public static bool Prefix(AActor Player, string Socket, ref bool __result)
	{
		if (!DI.Instance.AreaState.InRoom)
		{
			return true;
		}
		ACharacter val = (ACharacter)(object)((Player is ACharacter) ? Player : null);
		if ((UObject)(object)val == (UObject)null)
		{
			return true;
		}
		if ((UObject)(object)((APawn)val).GetController() == (UObject)null)
		{
			__result = false;
			return false;
		}
		if (val is BGUPlayerCharacterCS && (Socket == "CAMERA_LOCK_Root" || BGUFunctionLibraryCS.BGUHasUnitSimpleState((AActor)(object)val, (EBGUSimpleState)88)))
		{
			__result = false;
			return false;
		}
		return true;
	}
}
[HarmonyPatch(typeof(PlayerWukongAttrDataInit), "SetAttrTransAfterActiveTalent")]
[HarmonyPatchCategory("Connected")]
public class PatchSetAttrTransAfterActiveTalent
{
	public static Exception? Finalizer(Exception? __exception)
	{
		if (__exception != null)
		{
			DI.Instance.Logger.LogError(__exception, "Suppressed crash in SetAttrTransAfterActiveTalent");
		}
		return null;
	}
}
[HarmonyPatch(typeof(BPS_RebirthPointSystem), "OnSetRebirthPointAsCurrentBirthPoint")]
[HarmonyPatchCategory("Connected")]
public class PatchOnSetRebirthPointAsCurrentBirthPoint
{
	public static void Postfix(UActorCompBaseCS __instance, int RebirthPointID)
	{
		PlayerUtils.LogRebirthPointChange(__instance.GetOwner(), RebirthPointID);
		AActor owner = __instance.GetOwner();
		BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
		if (val != null && DI.Instance.PawnState.TryGetEntityByCharacter(val, out var entity))
		{
			DI.Instance.GameplayEventRouter.RaiseOnRebirthPointChanged(entity.Value, RebirthPointID);
		}
	}
}
[HarmonyPatch(typeof(BPS_RebirthPointSystem), "OnSetCurrentBirthPoint")]
[HarmonyPatchCategory("Connected")]
public class PatchOnSetCurrentBirthPoint
{
	public static void Postfix(UActorCompBaseCS __instance, int BirthPointID)
	{
		PlayerUtils.LogRebirthPointChange(__instance.GetOwner(), BirthPointID);
		AActor owner = __instance.GetOwner();
		BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
		if (val != null && DI.Instance.PawnState.TryGetEntityByCharacter(val, out var entity))
		{
			DI.Instance.GameplayEventRouter.RaiseOnRebirthPointChanged(entity.Value, BirthPointID);
		}
	}
}
[HarmonyPatch(typeof(BPS_RebirthPointSystem), "OnForceSetRebirthPoint")]
[HarmonyPatchCategory("Connected")]
public class PatchOnForceSetRebirthPoint
{
	public static void Postfix(UActorCompBaseCS __instance, int RebirthPointId)
	{
		PlayerUtils.LogRebirthPointChange(__instance.GetOwner(), RebirthPointId);
		AActor owner = __instance.GetOwner();
		BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
		if (val != null && DI.Instance.PawnState.TryGetEntityByCharacter(val, out var entity))
		{
			DI.Instance.GameplayEventRouter.RaiseOnRebirthPointChanged(entity.Value, RebirthPointId);
		}
	}
}
[HarmonyPatch]
[HarmonyPatchCategory("Connected")]
public class PatchOnRebirthFinished
{
	[HarmonyTargetMethodHint("b1.BUS_RebirthComp", "CommonRebirthLogic", new Type[] { })]
	private static MethodBase TargetMethod()
	{
		return AccessTools.Method("b1.BUS_RebirthComp:CommonRebirthLogic", (Type[])null, (Type[])null);
	}

	public static void Postfix(UActorCompBaseCS __instance)
	{
		AActor owner = __instance.GetOwner();
		MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
		if (entityByPlayerPawn.HasValue)
		{
			entityByPlayerPawn.Value.GetLocalState().IsRespawning = false;
			DI.Instance.Rpc.SendAfterRebirth();
		}
	}
}
[HarmonyPatch(typeof(UBGUFunctionLibCollisionChannel), "BGUSetCollisionResponseToChannels")]
[HarmonyPatchCategory("Connected")]
public class PatchBGUSetCollisionResponseToChannels
{
	public static bool Prefix(UPrimitiveComponent Comp, Dictionary<ECollisionChannel, ECollisionResponseType> ResponseToChannels)
	{
		if (!DI.Instance.AreaState.InRoom)
		{
			return true;
		}
		AActor owner = ((UActorComponent)Comp).GetOwner();
		if ((UObject)(object)owner == (UObject)null)
		{
			return true;
		}
		MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
		if (!entityByPlayerPawn.HasValue)
		{
			return true;
		}
		DI.Instance.Logger.LogDebug("Prevented BGUSetCollisionResponseToChannels for player {Pawn}", entityByPlayerPawn.Value.GetState().PlayerId);
		MainCharacterEntity value = entityByPlayerPawn.Value;
		MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
		return value == localMainCharacter;
	}
}
[HarmonyPatch(typeof(FUStSkillSDesc), "get_CooldownTime")]
[HarmonyPatchCategory("Connected")]
public static class PatchSkillCooldownTime
{
	public static void Postfix(ref float __result)
	{
		if (DI.Instance.AreaState.InRoom && DI.Instance.AreaState.CurrentArea.HasValue && DI.Instance.AreaState.CurrentArea.Value.Room.CheatsAllowed)
		{
			float num = __result;
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			__result = num * ((localMainCharacter.HasValue && localMainCharacter.GetValueOrDefault().GetLocalState().InstantSkillCooldown) ? 0f : 1f);
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
namespace WukongMp.Common.Patches
{
	public static class PatchOverlapUtils
	{
		internal static readonly object OverlapLock = new object();
	}
	[HarmonyPatch(typeof(BGS_SimpleOverlapMgrSystem), "ThreadFunc")]
	[HarmonyPatchCategory("Global")]
	public static class PatchThreadFunc
	{
		public static void Prefix()
		{
			Monitor.Enter(PatchOverlapUtils.OverlapLock);
		}

		public static void Finalizer()
		{
			Monitor.Exit(PatchOverlapUtils.OverlapLock);
		}
	}
	[HarmonyPatch(typeof(BGS_SimpleOverlapMgrSystem), "OnRegisterEntityUpdatenfo")]
	[HarmonyPatchCategory("Global")]
	public static class PatchOnRegisterEntityUpdatenfo
	{
		public static void Prefix()
		{
			Monitor.Enter(PatchOverlapUtils.OverlapLock);
		}

		public static void Finalizer()
		{
			Monitor.Exit(PatchOverlapUtils.OverlapLock);
		}
	}
	[HarmonyPatch(typeof(BGS_SimpleOverlapMgrSystem), "OnDeregisterEntity")]
	[HarmonyPatchCategory("Global")]
	public static class PatchOnDeregisterEntity
	{
		public static void Prefix()
		{
			Monitor.Enter(PatchOverlapUtils.OverlapLock);
		}

		public static void Finalizer()
		{
			Monitor.Exit(PatchOverlapUtils.OverlapLock);
		}
	}
	[HarmonyPatch(typeof(BGC_SimpleOverlapMgrData), "GetOverlapGridIndexList")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGetOverlapGridIndexList
	{
		[ThreadStatic]
		private static List<int>? _list;

		[ThreadStatic]
		private static Func<BGC_SimpleOverlapMgrData, float>? _getter;

		private static bool IsRectangleOverlap(FVector2D StartPoint, FVector2D EndPoint, FVector2D OverlapStartPoint, FVector2D OverlapEndPoint)
		{
			if ((double)((FVector2D)(ref StartPoint)).X != (double)((FVector2D)(ref EndPoint)).X && (double)((FVector2D)(ref StartPoint)).Y != (double)((FVector2D)(ref EndPoint)).Y && (double)((FVector2D)(ref OverlapStartPoint)).X != (double)((FVector2D)(ref OverlapEndPoint)).X && (double)((FVector2D)(ref OverlapStartPoint)).Y != (double)((FVector2D)(ref OverlapEndPoint)).Y && (double)((FVector2D)(ref EndPoint)).X > (double)((FVector2D)(ref OverlapStartPoint)).X && (double)((FVector2D)(ref StartPoint)).X < (double)((FVector2D)(ref OverlapEndPoint)).X && (double)((FVector2D)(ref EndPoint)).Y > (double)((FVector2D)(ref OverlapStartPoint)).Y)
			{
				return (double)((FVector2D)(ref StartPoint)).Y < (double)((FVector2D)(ref OverlapEndPoint)).Y;
			}
			return false;
		}

		public static bool Prefix(BGC_SimpleOverlapMgrData __instance, FVector2D Location, FVector2D SquareSize, BGUGridInfo GridInfo, out List<int> OutIndexList, out bool __result)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			OutIndexList = _list ?? (_list = new List<int>());
			OutIndexList.Clear();
			if (_getter == null)
			{
				MethodInfo getMethod = ((object)__instance).GetType().GetProperty("GridSize", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(nonPublic: true);
				_getter = (Func<BGC_SimpleOverlapMgrData, float>)Delegate.CreateDelegate(typeof(Func<BGC_SimpleOverlapMgrData, float>), getMethod);
			}
			float num = _getter(__instance);
			FVector2D startPoint = GridInfo.CenterLocation - new FVector2D(4.5 * (double)num, 4.5 * (double)num);
			FVector2D endPoint = GridInfo.CenterLocation + new FVector2D(4.5 * (double)num, 4.5 * (double)num);
			FVector2D overlapStartPoint = Location - SquareSize;
			FVector2D overlapEndPoint = Location + SquareSize;
			int num2 = (IsRectangleOverlap(startPoint, endPoint, overlapStartPoint, overlapEndPoint) ? 1 : 0);
			((FVector2D)(ref overlapStartPoint))..ctor((double)FMath.Max(((FVector2D)(ref overlapStartPoint)).X, ((FVector2D)(ref startPoint)).X), (double)FMath.Max(((FVector2D)(ref overlapStartPoint)).Y, ((FVector2D)(ref startPoint)).Y));
			((FVector2D)(ref overlapEndPoint))..ctor((double)FMath.Min(((FVector2D)(ref overlapEndPoint)).X, ((FVector2D)(ref endPoint)).X), (double)FMath.Min(((FVector2D)(ref overlapEndPoint)).Y, ((FVector2D)(ref endPoint)).Y));
			double num3 = FMath.Max(((FVector2D)(ref overlapStartPoint)).X, ((FVector2D)(ref startPoint)).X);
			float num4 = FMath.Min(((FVector2D)(ref overlapEndPoint)).X, ((FVector2D)(ref endPoint)).X);
			FVector2D centerLocation = GridInfo.CenterLocation;
			double num5 = ((FVector2D)(ref centerLocation)).X;
			float num6 = (float)(num3 - num5);
			float num7 = num6 % num;
			int item = 4 + (int)((double)num6 / (double)num) + ((!((double)num6 < 0.0)) ? 1 : (-1)) * (((double)FMath.Abs(num7) > (double)num / 2.0) ? 1 : 0);
			centerLocation = GridInfo.CenterLocation;
			float num8 = num4 - ((FVector2D)(ref centerLocation)).X;
			float num9 = num8 % num;
			int item2 = 4 + (int)((double)num8 / (double)num) + ((!((double)num8 < 0.0)) ? 1 : (-1)) * (((double)FMath.Abs(num9) > (double)num / 2.0) ? 1 : 0);
			double num10 = FMath.Max(((FVector2D)(ref overlapStartPoint)).Y, ((FVector2D)(ref startPoint)).Y);
			float num11 = FMath.Min(((FVector2D)(ref overlapEndPoint)).Y, ((FVector2D)(ref endPoint)).Y);
			centerLocation = GridInfo.CenterLocation;
			double num12 = ((FVector2D)(ref centerLocation)).Y;
			float num13 = (float)(num10 - num12);
			float num14 = num13 % num;
			int item3 = 4 + (int)((double)num13 / (double)num) + ((!((double)num13 < 0.0)) ? 1 : (-1)) * (((double)FMath.Abs(num14) > (double)num / 2.0) ? 1 : 0);
			centerLocation = GridInfo.CenterLocation;
			float num15 = num11 - ((FVector2D)(ref centerLocation)).Y;
			float num16 = num15 % num;
			int item4 = 4 + (int)((double)num15 / (double)num) + ((!((double)num15 < 0.0)) ? 1 : (-1)) * (((double)FMath.Abs(num16) > (double)num / 2.0) ? 1 : 0);
			OutIndexList.Add(item);
			OutIndexList.Add(item2);
			OutIndexList.Add(item3);
			OutIndexList.Add(item4);
			__result = num2 != 0;
			return false;
		}
	}
	[HarmonyPatch(typeof(BGC_SimpleOverlapMgrData), "GetSimpleOverlapActorsByMask")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGetSimpleOverlapActorsByMask
	{
		public static Exception? Finalizer()
		{
			return null;
		}
	}
}
namespace WukongMp.Api
{
	public class ColliderDisableData(WukongPlayerState playerState, ILogger logger)
	{
		private readonly Dictionary<AActor, float> _colliderDisableTimes = new Dictionary<AActor, float>();

		public void PermanentlyDisableCollider(AActor actor)
		{
			if (_colliderDisableTimes.ContainsKey(actor))
			{
				_colliderDisableTimes.Remove(actor);
				logger.LogDebug("Permanently disabled collider for actor: {Actor}", BGU_DataUtil.GetActorGuid(actor, false));
			}
		}

		public void DisableCollider(AActor actor, float disableDuration)
		{
			_colliderDisableTimes[actor] = disableDuration;
			actor.SetActorEnableCollision(false);
		}

		public void TryReEnableColliders(float deltaTime)
		{
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			List<AActor> list = new List<AActor>();
			foreach (AActor item in _colliderDisableTimes.Keys.ToList())
			{
				float num = _colliderDisableTimes[item] - deltaTime;
				if (num <= 0f)
				{
					list.Add(item);
				}
				else
				{
					_colliderDisableTimes[item] = num;
				}
			}
			List<FHitResultSimple> source = default(List<FHitResultSimple>);
			foreach (AActor collider in list)
			{
				collider.SetActorEnableCollision(true);
				if (playerState.LocalMainCharacter.HasValue && (UObject)(object)playerState.LocalMainCharacter.Value.GetLocalState().Pawn != (UObject)null)
				{
					BGUCharacterCS pawn = playerState.LocalMainCharacter.Value.GetLocalState().Pawn;
					float num2 = ((ACharacter)pawn).CapsuleComponent.GetScaledCapsuleRadius() + 20f;
					FVector lineTraceDir_SafeNormal2D = GetLineTraceDir_SafeNormal2D(pawn);
					FVector val = BGUFuncLibActorTransformCS.BGUGetActorLocation((AActor)(object)pawn);
					FVector val2 = val - lineTraceDir_SafeNormal2D * (double)num2;
					FVector val3 = val + lineTraceDir_SafeNormal2D * (double)num2;
					if (UBGUSelectUtil.MultiSphereTraceForObjects((UObject)(object)pawn, val2, val3, num2, new List<EObjectTypeQuery>(1) { (EObjectTypeQuery)14 }, false, ref source) > 0 && source.Any((FHitResultSimple x) => (UObject)(object)x.HitActor == (UObject)(object)collider))
					{
						logger.LogDebug("Re-disabled collider for actor: {Actor} due to player proximity", BGU_DataUtil.GetActorGuid(collider, false));
						DisableCollider(collider, 3f);
						continue;
					}
				}
				_colliderDisableTimes.Remove(collider);
				logger.LogDebug("Re-enabled collider for actor: {Actor}", BGU_DataUtil.GetActorGuid(collider, false));
			}
		}

		private FVector GetLineTraceDir_SafeNormal2D(BGUCharacterCS playerCharacter)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			FVector val;
			if (((UNavMovementComponent)((ACharacter)playerCharacter).CharacterMovement).IsFalling())
			{
				val = ((AActor)playerCharacter).GetVelocity();
				return ((FVector)(ref val)).GetSafeNormal2D(9.99999993922529E-09);
			}
			val = ((ACharacter)playerCharacter).CharacterMovement.GetCurrentAcceleration();
			return ((FVector)(ref val)).GetSafeNormal2D(9.99999993922529E-09);
		}
	}
	public sealed class DI
	{
		public static DI Instance { get; } = new DI();

		public InputManager InputManager { get; private set; }

		public ILoggerFactory LoggerFactory { get; private set; }

		public ILogger Logger { get; private set; }

		public Store World { get; private set; }

		public ClientWukongArchetypeRegistration ArchetypeRegistration { get; private set; }

		public ArchetypeEventRouter ArchetypeEvent { get; private set; }

		public IClientEcsUpdateLoop EcsLoop { get; private set; }

		public RelaySerializer Serializer { get; private set; }

		public HotSwappableRelayClient RelayClient { get; private set; }

		public IBlobClient BlobClient { get; set; }

		public NetworkedEntityManager NetEntity { get; private set; }

		public RelayClientService RelayClientService { get; private set; }

		public ClientState State { get; private set; }

		public ClientNetworkedEntityState ClientNetEntity { get; private set; }

		public TextRelaySerializer TextSerializer { get; private set; }

		public AreaComponentRegistry AreaComponentRegistry { get; private set; }

		public PlayerComponentRegistry PlayerComponentRegistry { get; private set; }

		public NetworkedOwnershipManager OwnershipManager { get; private set; }

		public ClientOwnershipManager ClientOwnership { get; private set; }

		public WukongAreaState AreaState { get; private set; }

		public WukongPlayerState PlayerState { get; private set; }

		public WukongPawnState PawnState { get; private set; }

		public WukongPlayerModeManager ModeManager { get; private set; }

		public WukongPlayerPawnState PlayerPawnState { get; private set; }

		public WukongRpcCallbacks Rpc { get; private set; }

		public WukongServerRpcCallbacks ServerRpc { get; private set; }

		public WukongSaveRelay SaveRelay { get; private set; }

		public WukongEventBus EventBus { get; private set; }

		public GameplayConfiguration GameplayConfiguration { get; private set; }

		public GameplayEventRouter GameplayEventRouter { get; private set; }

		public ColliderDisableData ColliderDisableData { get; private set; }

		public WukongNetworkLogger NetLogger { get; private set; }

		public INetworkedComponentRegistry NetComponentRegistry { get; private set; }

		public JobRegistry JobRegistry { get; private set; }

		public WukongSynchronizer Synchronizer { get; private set; }

		public WukongConnectionManager Connection { get; private set; }

		public WukongLevelTransitionConnectionController ConnectionController { get; private set; }

		public NetworkPingMonitor PingMonitor { get; private set; }

		public PingWidgetUpdater PingWidgetUpdater { get; private set; }

		public FreeCameraManager FreeCameraManager { get; private set; }

		public FreeCameraController FreeCameraController { get; private set; }

		public GameStateSynchronizer GameStateSynchronizer { get; private set; }

		public WukongCommandConsole CommandConsole { get; set; }

		public WukongChatter Chatter { get; private set; }

		public WukongInputManager WukongInputManager { get; private set; }

		public RuntimePrelude Prelude { get; private set; }

		public RuntimeWeaverBackend PreludeBackend { get; private set; }

		public WukongWidgetManager WidgetManager { get; private set; }

		public TimerController TimerController { get; private set; }

		public ShimRelayMessageParser ShimParser { get; private set; }

		public ShimReplayDependencyTracker ShimDepTracker { get; set; }

		public ShimReplayDependencyTracker ShimReplayDependencyTracker { get; private set; }

		public HotSwappableRelayClient ShimRecorderRelayClient { get; set; }

		public ShimRelayRecorder ShimRecorder { get; private set; }

		public ShimController ShimController { get; private set; }

		public ShimPlaybackRelayClient ShimPlaybackRelayClient { get; private set; }

		public ClientEcsUpdateLoop ShimEcsLoop { get; set; }

		public RelayClientService ShimRelayClientService { get; set; }

		public NetworkedEntityManager ShimNetEntity { get; set; }

		public BlobClient ShimRelayBlobClient { get; set; }

		public ShimAutoStarter ShimAuto { get; set; }

		public TestsRunner TestsRunner { get; set; }

		public void InitLogging(ILoggerFactory loggerFactory)
		{
			LoggerFactory = loggerFactory;
			Logger = LoggerFactory.CreateLogger("");
		}

		public void Init()
		{
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Expected O, but got Unknown
			//IL_048b: Expected O, but got Unknown
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Expected O, but got Unknown
			//IL_04a1: Expected O, but got Unknown
			Logger.LogDebug("Initializing DI...");
			_ = LoggerFactory;
			ILogger logger = Logger;
			InputManager val = (InputManager = InputManager.Instance);
			InputManager inputManager = val;
			AreaComponentRegistry areaComponentRegistry = (AreaComponentRegistry = new AreaComponentRegistry(new global::<>z__ReadOnlySingleElementList<IAreaComponentRegistration>(new WukongAreaRegistration())));
			AreaComponentRegistry areaComponentRegistry3 = areaComponentRegistry;
			PlayerComponentRegistry playerComponentRegistry = (PlayerComponentRegistry = new PlayerComponentRegistry(new global::<>z__ReadOnlySingleElementList<IPlayerComponentRegistration>(new WukongPlayerRegistration())));
			DefaultAreaArchetypeRegistration defaultAreaArchetypeRegistration = new DefaultAreaArchetypeRegistration(areaComponentRegistry3);
			DefaultPlayerArchetypeRegistration defaultPlayerArchetypeRegistration = new DefaultPlayerArchetypeRegistration(playerComponentRegistry);
			ClientWukongArchetypeRegistration clientWukongArchetypeRegistration = (ArchetypeRegistration = new ClientWukongArchetypeRegistration());
			ClientWukongArchetypeRegistration clientWukongArchetypeRegistration3 = clientWukongArchetypeRegistration;
			Store store = (World = new Store(new EntityStore(), new global::<>z__ReadOnlyArray<IArchetypeRegistration>(new IArchetypeRegistration[3] { defaultAreaArchetypeRegistration, defaultPlayerArchetypeRegistration, clientWukongArchetypeRegistration3 })));
			Store store3 = store;
			ArchetypeEventRouter archetypeEventRouter = (ArchetypeEvent = new ArchetypeEventRouter(store3));
			RelaySerializer relaySerializer = (Serializer = new RelaySerializer(new global::<>z__ReadOnlyArray<IRelaySerializerRegistration>(new IRelaySerializerRegistration[2]
			{
				new DefaultRelaySerializerRegistration(),
				new WukongSerializerRegistration()
			})));
			RelaySerializer serializer = relaySerializer;
			HotSwappableRelayClient hotSwappableRelayClient = (RelayClient = new HotSwappableRelayClient());
			HotSwappableRelayClient hotSwappableRelayClient3 = hotSwappableRelayClient;
			IBlobClient blobClient = (BlobClient = new HttpBlobClient(logger));
			IBlobClient blobClient3 = blobClient;
			NetworkedEntityManager networkedEntityManager = (NetEntity = new NetworkedEntityManager(store3, logger, hotSwappableRelayClient3));
			NetworkedEntityManager netEntity = networkedEntityManager;
			RelayClientService relayClientService = (RelayClientService = new RelayClientService(hotSwappableRelayClient3, logger));
			RelayClientService relayClientService3 = relayClientService;
			WukongEventBus wukongEventBus = (EventBus = new WukongEventBus());
			WukongEventBus eventBus = wukongEventBus;
			GameplayConfiguration gameplayConfiguration = (GameplayConfiguration = new GameplayConfiguration(logger));
			GameplayEventRouter gameplayEventRouter = (GameplayEventRouter = new GameplayEventRouter());
			GameplayEventRouter eventRouter = gameplayEventRouter;
			TextRelaySerializer textRelaySerializer = (TextSerializer = new TextRelaySerializer(new global::<>z__ReadOnlyArray<ITextRelaySerializerRegistration>(new ITextRelaySerializerRegistration[3]
			{
				new DefaultTextRelaySerializerRegistration(),
				new WukongTextSerializerRegistration(),
				new ClientShimTextSerializerRegistration()
			})));
			TextRelaySerializer textSerializer = textRelaySerializer;
			IClientEcsUpdateLoop clientEcsUpdateLoop = (EcsLoop = new ClientEcsUpdateLoop(store3, logger));
			IClientEcsUpdateLoop ecsLoop = clientEcsUpdateLoop;
			INetworkedComponentRegistry networkedComponentRegistry = (NetComponentRegistry = new NetworkedComponentRegistry(new global::<>z__ReadOnlyArray<INetworkedComponentRegistration>(new INetworkedComponentRegistration[2]
			{
				new DefaultNetworkedComponentRegistration(),
				new WukongNetworkedComponentRegistration()
			})));
			INetworkedComponentRegistry networkedComponentRegistry3 = networkedComponentRegistry;
			JobRegistry jobRegistry = (JobRegistry = new JobRegistry(networkedComponentRegistry3, netEntity, hotSwappableRelayClient3, logger));
			JobRegistry jobRegistry3 = jobRegistry;
			ClientState clientState = (State = new ClientState(store3, netEntity, hotSwappableRelayClient3, ecsLoop, jobRegistry3, defaultAreaArchetypeRegistration, defaultPlayerArchetypeRegistration, logger));
			ClientState clientState3 = clientState;
			ClientNetworkedEntityState clientNetworkedEntityState = (ClientNetEntity = new ClientNetworkedEntityState(netEntity, clientState3, logger));
			ClientNetworkedEntityState clientNetworkedEntityState3 = clientNetworkedEntityState;
			WukongPlayerState wukongPlayerState = (PlayerState = new WukongPlayerState(store3, clientWukongArchetypeRegistration3, clientNetworkedEntityState3, clientState3, logger));
			WukongPlayerState playerState = wukongPlayerState;
			WukongWidgetManager wukongWidgetManager = (WidgetManager = new WukongWidgetManager(clientState3, playerState));
			WukongWidgetManager widgetManager = wukongWidgetManager;
			TimerController timerController = (TimerController = new TimerController(widgetManager));
			TimerController timerController3 = timerController;
			WukongPawnState wukongPawnState = (PawnState = new WukongPawnState(store3, clientWukongArchetypeRegistration3, clientNetworkedEntityState3));
			WukongPawnState pawnState = wukongPawnState;
			WukongPlayerPawnState wukongPlayerPawnState = (PlayerPawnState = new WukongPlayerPawnState(store3, playerState, logger));
			NetworkedOwnershipManager networkedOwnershipManager = (OwnershipManager = new NetworkedOwnershipManager(store3, logger));
			NetworkedOwnershipManager ownership = networkedOwnershipManager;
			ClientOwnershipManager clientOwnershipManager = (ClientOwnership = new ClientOwnershipManager(clientState3, ownership));
			ClientOwnershipManager clientOwnershipManager3 = clientOwnershipManager;
			FreeCameraManager freeCameraManager = (FreeCameraManager = new FreeCameraManager(playerState));
			FreeCameraManager freeCameraManager3 = freeCameraManager;
			FreeCameraController freeCameraController = (FreeCameraController = new FreeCameraController(clientState3, playerState, inputManager, freeCameraManager3, widgetManager));
			GameStateSynchronizer gameStateSynchronizer = (GameStateSynchronizer = new GameStateSynchronizer(clientState3, playerState));
			ColliderDisableData colliderDisableData = (ColliderDisableData = new ColliderDisableData(playerState, logger));
			WukongAreaState wukongAreaState = (AreaState = new WukongAreaState(clientState3, store3, clientOwnershipManager3));
			WukongAreaState areaState = wukongAreaState;
			WukongPlayerModeManager wukongPlayerModeManager = (ModeManager = new WukongPlayerModeManager(clientState3, eventRouter, freeCameraManager3));
			WukongConnectionManager wukongConnectionManager = (Connection = new WukongConnectionManager(relayClientService3, clientState3, playerState, areaState, logger));
			WukongConnectionManager connection = wukongConnectionManager;
			WukongNetworkLogger wukongNetworkLogger = (NetLogger = new WukongNetworkLogger(store3, clientState3, areaState, playerState, logger));
			WukongRpcCallbacks wukongRpcCallbacks = (Rpc = new WukongRpcCallbacks(serializer, hotSwappableRelayClient3, clientState3, areaState, clientNetworkedEntityState3, playerState, pawnState, clientOwnershipManager3, eventRouter, widgetManager, timerController3, ecsLoop, logger));
			WukongRpcCallbacks rpc = wukongRpcCallbacks;
			WukongServerRpcCallbacks wukongServerRpcCallbacks = (ServerRpc = new WukongServerRpcCallbacks(hotSwappableRelayClient3, ecsLoop, logger, widgetManager));
			WukongServerRpcCallbacks wukongServerRpcCallbacks3 = wukongServerRpcCallbacks;
			WukongSaveRelay wukongSaveRelay = (SaveRelay = new WukongSaveRelay(blobClient3, logger));
			WukongChatter wukongChatter = (Chatter = new WukongChatter(playerState, rpc, widgetManager));
			WukongChatter chatter = wukongChatter;
			WukongCommandConsole wukongCommandConsole = (CommandConsole = new WukongCommandConsole(connection, playerState, rpc, wukongServerRpcCallbacks3, Chatter, widgetManager, eventBus, areaState, ecsLoop));
			WukongCommandConsole commandConsole = wukongCommandConsole;
			WukongInputManager wukongInputManager = (WukongInputManager = new WukongInputManager(commandConsole, chatter, widgetManager));
			WukongLevelTransitionConnectionController wukongLevelTransitionConnectionController = (ConnectionController = new WukongLevelTransitionConnectionController(eventBus, connection));
			NetworkPingMonitor networkPingMonitor = (PingMonitor = new NetworkPingMonitor(hotSwappableRelayClient3));
			NetworkPingMonitor pingMonitor = networkPingMonitor;
			PingWidgetUpdater pingWidgetUpdater = (PingWidgetUpdater = new PingWidgetUpdater(pingMonitor, wukongServerRpcCallbacks3));
			ILogger logger2 = LoggerFactory.CreateLogger("Runtime");
			RuntimeWeaverBackend val2 = new RuntimeWeaverBackend(logger2);
			RuntimeWeaverBackend val3 = val2;
			PreludeBackend = val2;
			RuntimeWeaverBackend val4 = val3;
			RuntimePrelude val5 = new RuntimePrelude((IRuntimeBackend)(object)val4, logger2);
			RuntimePrelude val6 = val5;
			Prelude = val5;
			ILogger logger3 = LoggerFactory.CreateLogger("Shim");
			ILogger logger4 = LoggerFactory.CreateLogger("Shim Recorder");
			ILogger logger5 = LoggerFactory.CreateLogger("Shim Playback");
			Store world = new Store(new EntityStore(), new global::<>z__ReadOnlyArray<IArchetypeRegistration>(new IArchetypeRegistration[3] { defaultAreaArchetypeRegistration, defaultPlayerArchetypeRegistration, clientWukongArchetypeRegistration3 }));
			hotSwappableRelayClient = (ShimRecorderRelayClient = new HotSwappableRelayClient());
			HotSwappableRelayClient hotSwappableRelayClient5 = hotSwappableRelayClient;
			relayClientService = (ShimRelayClientService = new RelayClientService(hotSwappableRelayClient5, logger4));
			RelayClientService recorderRelayService = relayClientService;
			BlobClient blobClient4 = (ShimRelayBlobClient = new BlobClient(hotSwappableRelayClient5, logger4));
			BlobClient recorderRelayBlobClient = blobClient4;
			networkedEntityManager = (ShimNetEntity = new NetworkedEntityManager(world, logger4, hotSwappableRelayClient5));
			NetworkedEntityManager netEntity2 = networkedEntityManager;
			ClientEcsUpdateLoop clientEcsUpdateLoop3 = (ShimEcsLoop = new ClientEcsUpdateLoop(world, logger4));
			ClientEcsUpdateLoop clientEcsUpdateLoop5 = clientEcsUpdateLoop3;
			ClientState state = new ClientState(world, netEntity2, hotSwappableRelayClient5, clientEcsUpdateLoop5, jobRegistry3, defaultAreaArchetypeRegistration, defaultPlayerArchetypeRegistration, logger4);
			new ClientNetworkedStateSynchronizer(netEntity2, state, jobRegistry3, networkedComponentRegistry3, hotSwappableRelayClient5, clientEcsUpdateLoop5, clientOwnershipManager3, logger4);
			ShimRelayMessageParser shimRelayMessageParser = (ShimParser = new ShimRelayMessageParser(new global::<>z__ReadOnlyArray<IShimRelayMessageParserImpl>(new IShimRelayMessageParserImpl[2]
			{
				new BlobClientShimParserImpl(),
				new ClientSynchronizerShimParserImpl(netEntity2, logger3)
			})));
			ShimRelayMessageParser parser = shimRelayMessageParser;
			ShimReplayDependencyTracker shimReplayDependencyTracker = (ShimDepTracker = new ShimReplayDependencyTracker(new global::<>z__ReadOnlyArray<IShimDependencyTrackerImpl>(new IShimDependencyTrackerImpl[2]
			{
				new BlobClientShimTrackerImpl(),
				new ClientSynchronizerShimTrackerImpl()
			})));
			ShimReplayDependencyTracker depTracker = shimReplayDependencyTracker;
			ShimPlaybackRelayClient shimPlaybackRelayClient = (ShimPlaybackRelayClient = new ShimPlaybackRelayClient(depTracker, parser, logger5));
			ShimPlaybackRelayClient playbackClient = shimPlaybackRelayClient;
			ShimRelayRecorder shimRelayRecorder = (ShimRecorder = new ShimRelayRecorder(hotSwappableRelayClient5, parser, logger4));
			ShimRelayRecorder shimRelayRecorder3 = shimRelayRecorder;
			ShimController shimController = (ShimController = new ShimController(shimRelayRecorder3, textSerializer, logger4));
			ShimAutoStarter shimAutoStarter = (ShimAuto = new ShimAutoStarter(clientState3, eventBus, ecsLoop, clientEcsUpdateLoop5, playbackClient, shimRelayRecorder3, recorderRelayBlobClient, recorderRelayService, logger3));
			TestsRunner = new TestsRunner(logger);
			Logger.LogDebug("DI Initialized");
		}
	}
	public static class EcsExtensions
	{
		public static T? GetComponent<T>(this BGUActorBaseCS actor) where T : UActorCompBaseCS
		{
			UActorCompBaseCS? obj = ((IEnumerable<UActorCompBaseCS>)Traverse.Create((object)((BGUSimpleActorBaseCS)actor).ActorCompContainerCS).Field<List<UActorCompBaseCS>>("CompCSs").Value).FirstOrDefault((Func<UActorCompBaseCS, bool>)((UActorCompBaseCS x) => x is T));
			return (T)(object)((obj is T) ? obj : null);
		}
	}
	public class GameplayEventRouter
	{
		public event Action<CultureInfo>? OnLanguageChanged;

		public event Action<Entity, Entity>? OnUnitDead;

		public event Action<Entity, int>? OnRebirthPointChanged;

		public event Action<Entity>? OnMonsterSpawned;

		public event Action<PlayerEntity, MainCharacterEntity>? OnPlayerChangedTeam;

		public event Action<bool>? OnLocalPlayerChangedSpectator;

		public event Action? OnLocalPlayerBeforeRebirth;

		public void RaiseOnLanguageChanged(CultureInfo culture)
		{
			this.OnLanguageChanged?.Invoke(culture);
		}

		public void RaiseOnUnitDead(Entity victimEntity, Entity attackerEntity)
		{
			this.OnUnitDead?.Invoke(victimEntity, attackerEntity);
		}

		public void RaiseOnRebirthPointChanged(Entity playerEntity, int rebirthPointId)
		{
			this.OnRebirthPointChanged?.Invoke(playerEntity, rebirthPointId);
		}

		public void RaiseOnMonsterSpawned(Entity monsterEntity)
		{
			this.OnMonsterSpawned?.Invoke(monsterEntity);
		}

		public void RaiseOnPlayerChangedTeam(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			this.OnPlayerChangedTeam?.Invoke(playerEntity, mainEntity);
		}

		public void RaiseOnLocalPlayerChangedSpectator(bool enabled)
		{
			this.OnLocalPlayerChangedSpectator?.Invoke(enabled);
		}

		public void RaiseOnLocalPlayerBeforeRebirth()
		{
			this.OnLocalPlayerBeforeRebirth?.Invoke();
		}
	}
	public class GameStateSynchronizer : IDisposable
	{
		private readonly ClientState _state;

		private readonly WukongPlayerState _playerState;

		public GameStateSynchronizer(ClientState state, WukongPlayerState playerState)
		{
			_state = state;
			_playerState = playerState;
			_state.OnLeftArea += OnLeftAreaHandler;
		}

		public void Dispose()
		{
			_state.OnLeftArea -= OnLeftAreaHandler;
		}

		private void OnLeftAreaHandler(AreaId areaId, Entity entity)
		{
			Logging.LogDebug("Left area, cleaning up game state.");
			MainCharacterEntity? localMainCharacter = _playerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				CutsceneUtils.ClearLocalJoiningCutsceneStatus(localMainCharacter.Value);
			}
		}
	}
	public class LaunchParameters
	{
		private static LaunchParameters? _instance;

		public static LaunchParameters Instance => _instance ?? (_instance = new LaunchParameters());

		public bool Valid
		{
			get
			{
				if (ServerIp != null && ServerPort.HasValue)
				{
					return UserGuid != Guid.Empty;
				}
				return false;
			}
		}

		public bool ValidForCoOp
		{
			get
			{
				if (Valid && GameMode == "co-op" && JwtToken != null && ApiBaseUrl != null)
				{
					return ServerId.HasValue;
				}
				return false;
			}
		}

		public bool ValidForPvP
		{
			get
			{
				if (GameMode == "pvp")
				{
					return LevelId.HasValue;
				}
				return false;
			}
		}

		public string? ModFolderOverride { get; }

		public string? ServerIp { get; }

		public int? ServerPort { get; }

		public int? ServerId { get; }

		public Guid UserGuid { get; } = Guid.Empty;

		public string? GameMode { get; }

		public string? ApiBaseUrl { get; }

		public string? JwtToken { get; }

		public string Nickname { get; } = "Player";

		public int? LevelId { get; set; }

		public string? ShimDbName { get; }

		public string? ShimDbDir { get; }

		public bool RecordShimOnStart => RecordShimName != null;

		public string? RecordShimName { get; }

		public string? RecordShimFile { get; }

		public bool PlayShimOnStart => PlayShimName != null;

		public string? PlayShimName { get; }

		public string? PlayShimFile { get; }

		private LaunchParameters()
		{
			Dictionary<string, string> dict = IpcHelpers.ReadAndDeleteIpcHandshakeFile();
			GameMode = dict.GetValueOrDefault("GAME_MODE").ToLowerInvariant();
			if (string.IsNullOrWhiteSpace(GameMode))
			{
				Logging.LogError("Game mode not provided, launch the game from the ReadyM Launcher.");
				return;
			}
			ApiBaseUrl = dict.GetValueOrDefault("API_BASE_URL");
			JwtToken = dict.GetValueOrDefault("JWT_TOKEN");
			string valueOrDefault = dict.GetValueOrDefault("PLAYER_ID");
			if (Guid.TryParse(valueOrDefault, out var result))
			{
				UserGuid = result;
				Logging.LogDebug("User ID: {Guid}", UserGuid);
			}
			else
			{
				Logging.LogError("Invalid ID format: {Guid}", valueOrDefault);
			}
			string valueOrDefault2 = dict.GetValueOrDefault("SERVER_ID");
			if (!string.IsNullOrWhiteSpace(valueOrDefault2) && int.TryParse(valueOrDefault2, out var result2))
			{
				ServerId = result2;
			}
			ServerIp = dict.GetValueOrDefault("SERVER_IP");
			string valueOrDefault3 = dict.GetValueOrDefault("SERVER_PORT");
			if (!string.IsNullOrWhiteSpace(valueOrDefault3) && int.TryParse(valueOrDefault3, out var result3))
			{
				ServerPort = result3;
			}
			Nickname = dict.GetValueOrDefault("NICKNAME");
			string valueOrDefault4 = dict.GetValueOrDefault("LEVEL_ID");
			if (!string.IsNullOrWhiteSpace(valueOrDefault4) && int.TryParse(valueOrDefault4, out var result4))
			{
				LevelId = result4;
			}
			string valueOrDefault5 = dict.GetValueOrDefault("MOD_FOLDER");
			if (!string.IsNullOrWhiteSpace(valueOrDefault5))
			{
				ModFolderOverride = valueOrDefault5;
				Logging.LogDebug("Mod folder: {Folder}", ModFolderOverride);
			}
			string valueOrDefault6 = dict.GetValueOrDefault("SHIM_DB");
			if (!string.IsNullOrWhiteSpace(valueOrDefault6))
			{
				ShimDbName = valueOrDefault6;
				Logging.LogDebug("Shim DB: {ShimDbName}", ShimDbName);
			}
			else
			{
				ShimDbName = "Default";
				Logging.LogDebug("Shim DB not provided, using: Default");
			}
			ShimDbDir = Path.GetFullPath("CSharpLoader/Shims/" + ShimDbName);
			string valueOrDefault7 = dict.GetValueOrDefault("RECORD_SHIM");
			if (!string.IsNullOrWhiteSpace(valueOrDefault7))
			{
				RecordShimName = valueOrDefault7;
				RecordShimFile = Path.GetFullPath(ShimDbDir + "/" + RecordShimName + ".shim");
				Logging.LogDebug("Record Shim: {RecordShimFile}", RecordShimFile);
			}
			string valueOrDefault8 = dict.GetValueOrDefault("PLAY_SHIM");
			if (!string.IsNullOrWhiteSpace(valueOrDefault8))
			{
				PlayShimName = valueOrDefault8;
				PlayShimFile = Path.GetFullPath(ShimDbDir + "/" + PlayShimName + ".shim");
				Logging.LogDebug("Play Shim: {PlayShimFile}", PlayShimFile);
			}
		}
	}
	public static class LoggerExtensions
	{
		public static void LogNullDebug(this ILogger logger, string propertyName)
		{
			logger.LogDebug("{Value} is null", propertyName);
		}

		public static void LogNull(this ILogger logger, string propertyName)
		{
			logger.LogError("{Value} is null", propertyName);
		}
	}
	public static class Logging
	{
		public static void LogTrace(string? message, params object?[] args)
		{
		}

		public static void LogDebug(string? message, params object?[] args)
		{
		}

		public static void LogInformation(string? message, params object?[] args)
		{
			DI.Instance.Logger.LogInformation(message, args);
		}

		public static void LogWarning(string? message, params object?[] args)
		{
			DI.Instance.Logger.LogWarning(message, args);
		}

		public static void LogError(string? message, params object?[] args)
		{
			DI.Instance.Logger.LogError(message, args);
		}

		public static void LogError(Exception? ex, string? message = null, params object?[] args)
		{
			DI.Instance.Logger.LogError(ex, message, args);
		}

		public static void LogCritical(string? message, params object?[] args)
		{
			DI.Instance.Logger.LogCritical(message, args);
		}

		public static void LogCritical(Exception? ex, string? message = null, params object?[] args)
		{
			DI.Instance.Logger.LogCritical(ex, message, args);
		}

		public static void LogException(Exception ex, string? message = null, params object?[] args)
		{
			DI.Instance.Logger.LogError(ex, message ?? "An exception occurred", args);
		}

		public static void LogNull(string propertyName)
		{
			DI.Instance.Logger.LogNull(propertyName);
		}
	}
	public class PingWidgetUpdater : IDisposable
	{
		private readonly NetworkPingMonitor _pingMonitor;

		private readonly WukongServerRpcCallbacks _rpc;

		public PingWidgetUpdater(NetworkPingMonitor pingMonitor, WukongServerRpcCallbacks rpc)
		{
			_rpc = rpc;
			_pingMonitor = pingMonitor;
			_pingMonitor.OnPingUpdated += HandlePingUpdated;
		}

		public void Dispose()
		{
			_pingMonitor.OnPingUpdated -= HandlePingUpdated;
		}

		private void HandlePingUpdated(int _)
		{
			_rpc.SendPing();
		}
	}
	public class PreludePatcherBase : PatcherBase
	{
		protected readonly RuntimePreludeBuilder Prelude;

		public PreludePatcherBase(string harmonyId, RuntimePrelude prelude)
		{
			<prelude>P = prelude;
			Prelude = <prelude>P.Create(harmonyId);
			base..ctor();
		}

		protected override void OnCommit()
		{
			<prelude>P.Commit();
		}
	}
	public static class USharpExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(this float a, float b, float tolerance)
		{
			return MathF.Abs(a - b) < tolerance;
		}

		public static bool IsNullOrDestroyed([NotNullWhen(false)] this UObject? obj)
		{
			if (obj != null && !obj.IsDestroyed && !SharedRuntimeState.IsShutdown && !obj.HasAnyFlags((EObjectFlags)65536))
			{
				return obj.IsPendingKill;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this FVector vector)
		{
			return new Vector3(((FVector)(ref vector)).X, ((FVector)(ref vector)).Y, ((FVector)(ref vector)).Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FVector ToFVector(this Vector3 vector)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new FVector((double)vector.X, (double)vector.Y, (double)vector.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this FRotator vector)
		{
			return new Vector3(((FRotator)(ref vector)).Pitch, ((FRotator)(ref vector)).Yaw, ((FRotator)(ref vector)).Roll);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FRotator ToFRotator(this Vector3 vector)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new FRotator((double)vector.X, (double)vector.Y, (double)vector.Z);
		}
	}
	[Obsolete]
	public class WukongConnectionManager : IDisposable
	{
		private readonly ClientState _state;

		private readonly ILogger _logger;

		public RelayClientService RelayClientService { get; }

		public IRelayClient RelayClient { get; }

		public WukongAreaState AreaState { get; }

		public WukongPlayerState PlayerState { get; }

		public PlayerId? PlayerId => PlayerState.LocalPlayerId;

		public bool IsRunning => RelayClientService.IsRunning;

		public bool RequestedConnect => RelayClient.RequestedConnect;

		public AreaId? RequestedAreaId => RelayClient.RequestedAreaId;

		public WukongConnectionManager(RelayClientService relayClientService, ClientState state, WukongPlayerState playerState, WukongAreaState areaState, ILogger logger)
		{
			RelayClientService = relayClientService;
			RelayClient = relayClientService.RelayClient;
			AreaState = areaState;
			PlayerState = playerState;
			_state = state;
			_logger = logger;
			_state.OnConnected += OnConnectedHandler;
			_state.OnDisconnected += OnDisconnectedHandler;
		}

		private static void OnConnectedHandler(PlayerId player, Entity entity)
		{
			entity.GetComponent<PlayerComponent>().NickName = LaunchParameters.Instance.Nickname;
		}

		public void Dispose()
		{
			_state.OnConnected -= OnConnectedHandler;
			_state.OnDisconnected -= OnDisconnectedHandler;
		}

		public void Start()
		{
			RelayClientService.Start();
		}

		public void Stop()
		{
			RelayClientService.Stop();
		}

		public void Connect()
		{
			RelayClient.RequestConnect();
		}

		public void Disconnect()
		{
			if (RequestedAreaId.HasValue)
			{
				LeaveArea();
			}
			if (RequestedConnect)
			{
				RelayClient.RequestDisconnect();
			}
		}

		public void JoinArea(AreaId areaId)
		{
			RelayClient.RequestJoinArea(areaId);
		}

		public void LeaveArea()
		{
			RelayClient.RequestLeaveArea();
		}

		public void Reconnect()
		{
			Logging.LogInformation("Attempting to reconnect...");
			RelayClient.Scheduler.Schedule(async delegate(IRelayClientNetworkThreadContext context, WukongConnectionManager self)
			{
				_ = 1;
				try
				{
					AreaId? areaId = self.RequestedAreaId;
					if (self.RequestedAreaId.HasValue)
					{
						self.LeaveArea();
					}
					if (self.RequestedConnect)
					{
						self.Disconnect();
					}
					await Task.Delay(1000);
					if (!self.RequestedConnect)
					{
						self.Connect();
					}
					if (areaId.HasValue)
					{
						await Task.Delay(1000);
						self.JoinArea(areaId.Value);
					}
				}
				catch (Exception exception)
				{
					self._logger.LogError(exception, "Error while reconnecting");
				}
			}, this);
		}

		public void OnDisconnectedHandler(PlayerId playerId, Entity? entity, DisconnectReason disconnectReason)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogInformation("Disconnected");
			if ((int)disconnectReason == 5)
			{
				Logging.LogInformation("Disconnected: {Cause}", disconnectReason);
			}
			else
			{
				Logging.LogWarning("Disconnected: {Cause}", disconnectReason);
				Reconnect();
			}
		}
	}
	public class WukongEventBus
	{
		private enum LevelTransitionPhase
		{
			None,
			Loading,
			Playing,
			Ending
		}

		private LevelTransitionPhase _phase;

		public bool IsGameplayLevel { get; private set; }

		public event Action? OnBeginLoadGameplayLevel;

		public event Action? OnBeginPlayGameplayLevel;

		public event Action? OnEndPlayGameplayLevel;

		public event Action? OnLoadingScreenClose;

		public event Action? OnLevelLoaded;

		public event Action? OnExitLevel;

		public bool TryInvokeBeginLoadGameplayLevel()
		{
			if (_phase == LevelTransitionPhase.Loading)
			{
				return false;
			}
			_phase = LevelTransitionPhase.Loading;
			this.OnBeginLoadGameplayLevel?.Invoke();
			return true;
		}

		public bool TryInvokeBeginPlayGameplayLevel()
		{
			if (_phase == LevelTransitionPhase.Playing)
			{
				return false;
			}
			_phase = LevelTransitionPhase.Playing;
			IsGameplayLevel = true;
			this.OnBeginPlayGameplayLevel?.Invoke();
			return true;
		}

		public bool TryInvokeEndPlayGameplayLevel()
		{
			if (_phase == LevelTransitionPhase.Ending)
			{
				return false;
			}
			_phase = LevelTransitionPhase.Ending;
			this.OnEndPlayGameplayLevel?.Invoke();
			IsGameplayLevel = false;
			return true;
		}

		public void InvokeLoadingScreenClose()
		{
			_phase = LevelTransitionPhase.None;
			this.OnLoadingScreenClose?.Invoke();
		}

		public void InvokeOnLevelLoaded()
		{
			_phase = LevelTransitionPhase.None;
			this.OnLevelLoaded?.Invoke();
		}

		public void InvokeOnExitLevel()
		{
			_phase = LevelTransitionPhase.None;
			this.OnExitLevel?.Invoke();
		}
	}
	public class WukongLevelTransitionConnectionController : IDisposable
	{
		private readonly WukongEventBus _eventBus;

		private readonly WukongConnectionManager _connection;

		public WukongLevelTransitionConnectionController(WukongEventBus eventBus, WukongConnectionManager connection)
		{
			_eventBus = eventBus;
			_connection = connection;
			_eventBus.OnBeginPlayGameplayLevel += OnBeginPlayGameplayLevel;
			_eventBus.OnEndPlayGameplayLevel += OnEndPlayGameplayLevel;
		}

		public void Dispose()
		{
			_eventBus.OnEndPlayGameplayLevel -= OnEndPlayGameplayLevel;
			_eventBus.OnBeginPlayGameplayLevel -= OnBeginPlayGameplayLevel;
		}

		private void OnBeginPlayGameplayLevel()
		{
			int curLevelId = BGUFuncLibMap.GetCurLevelId((UObject)(object)GameUtils.GetWorld());
			if (curLevelId > 65535)
			{
				throw new InvalidCastException("AreaId is greater than ushort max value");
			}
			_connection.JoinArea(new AreaId((ushort)curLevelId));
		}

		private void OnEndPlayGameplayLevel()
		{
			_connection.LeaveArea();
		}
	}
	public class WukongNetworkLogger(Store world, ClientState state, WukongAreaState areaState, WukongPlayerState playerState, ILogger logger)
	{
		public void DumpDebugInfo()
		{
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			logger.LogDebug("Room state: {State}", areaState.ToString());
			if (playerState.LocalPlayerEntity.HasValue)
			{
				logger.LogDebug("Local player state: {State}", playerState.LocalPlayerEntity.Value.GetState());
			}
			else
			{
				logger.LogWarning("No local player state found.");
			}
			foreach (PlayerId allPlayer in state.AllPlayers)
			{
				PlayerEntity? playerById = playerState.GetPlayerById(allPlayer);
				logger.LogDebug("Player {PlayerId} state: {State}", allPlayer, playerById.ToString());
			}
			world.Query<MetadataComponent>().ForEachEntity(delegate(ref MetadataComponent metaComp, Entity entity)
			{
				logger.LogDebug("Entity {NetId} (owner {Owner}): {Entity}", metaComp.NetId, metaComp.Owner, entity.DebugJSON);
			});
			BGC_TeamRelationData val = (BGC_TeamRelationData)BGU_DataUtil.GetGameStateReadonlyData<IBGC_TeamRelationData, BGC_TeamRelationData>((UObject)(object)GameUtils.GetWorld());
			if (val != null)
			{
				int num = default(int);
				TeamRelationInfo val2 = default(TeamRelationInfo);
				foreach (KeyValuePair<int, TeamRelationInfo> teamHostileInfo in val.TeamHostileInfos)
				{
					CompatExtensions.Deconstruct<int, TeamRelationInfo>(teamHostileInfo, ref num, ref val2);
					int num2 = num;
					TeamRelationInfo val3 = val2;
					logger.LogDebug("Team {TeamId} hostility: {HostileTeams}", num2, string.Join(", ", val3.HostileTeamIDs));
				}
			}
			string perfLog = world.SystemRoot.GetPerfLog();
			if (perfLog != null)
			{
				logger.LogDebug("Perf log:\n{Log}", perfLog);
			}
			else
			{
				logger.LogWarning("Perf log is null");
			}
		}
	}
	public class WukongPatcher : PreludePatcherBase
	{
		public WukongPatcher(RuntimePrelude prelude)
			: base("ReadyM.WukongMp", prelude)
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
	public class WukongPlayerModeManager(ClientState state, GameplayEventRouter eventRouter, FreeCameraManager freeCameraManager)
	{
		private float _gravityScale;

		private FVector _lastValidLocation;

		public bool HandleBecameSpectator(PlayerEntity playerEntity, MainCharacterEntity mainEntity, bool isSpectator)
		{
			if (isSpectator)
			{
				return HandleBecameSpectator(playerEntity, mainEntity);
			}
			return HandleStoppedBeingSpectator(playerEntity, mainEntity);
		}

		public bool HandleBecameSpectator(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			ref MainCharacterComponent state = ref mainEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			PlayerId playerId = state.PlayerId;
			PlayerId? localPlayerId = state.LocalPlayerId;
			int num;
			if (!localPlayerId.HasValue)
			{
				num = 0;
			}
			else
			{
				num = ((playerId == localPlayerId.GetValueOrDefault()) ? 1 : 0);
				if (num != 0)
				{
					UiUtils.SetHudVisibility(visible: false);
				}
			}
			SetPlayerVisibility(playerEntity, mainEntity, visible: false);
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
			if (obj != null)
			{
				obj.Evt_BuffAllRemove.Invoke((EBuffEffectTriggerType)3, true);
			}
			if (num != 0)
			{
				freeCameraManager.EnterFreeCameraMode();
				eventRouter.RaiseOnLocalPlayerChangedSpectator(enabled: true);
			}
			SetSpectatorCollisionEnabled(playerEntity, mainEntity, enable: false);
			eventRouter.RaiseOnPlayerChangedTeam(playerEntity, mainEntity);
			return true;
		}

		public bool HandleStoppedBeingSpectator(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			PlayerId playerId = mainEntity.GetState().PlayerId;
			PlayerId? localPlayerId = state.LocalPlayerId;
			int num;
			if (!localPlayerId.HasValue)
			{
				num = 0;
			}
			else
			{
				num = ((playerId == localPlayerId.GetValueOrDefault()) ? 1 : 0);
				if (num != 0)
				{
					UiUtils.SetHudVisibility(visible: true);
				}
			}
			SetPlayerVisibility(playerEntity, mainEntity, visible: true);
			SetSpectatorCollisionEnabled(playerEntity, mainEntity, enable: true);
			if (num != 0)
			{
				freeCameraManager.LeaveFreeCameraMode();
				eventRouter.RaiseOnLocalPlayerChangedSpectator(enabled: false);
			}
			eventRouter.RaiseOnPlayerChangedTeam(playerEntity, mainEntity);
			return true;
		}

		public bool SetPlayerVisibility(PlayerEntity playerEntity, MainCharacterEntity mainEntity, bool visible)
		{
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			ref PlayerComponent state = ref playerEntity.GetState();
			BGUCharacterCS? pawn = localState.Pawn;
			if ((pawn != null && !((AActor)pawn).Hidden) == visible)
			{
				return false;
			}
			Logging.LogDebug("Setting player {PlayerName} visibility to: {Visibility}", state.NickName, visible);
			if ((UObject)(object)localState.Pawn == (UObject)null)
			{
				Logging.LogError("Player pawn is null");
				return false;
			}
			((AActor)localState.Pawn).SetActorHiddenInGame(!visible);
			AActor? markerActor = localState.MarkerActor;
			if (markerActor != null)
			{
				markerActor.SetActorHiddenInGame(!visible);
			}
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)102, visible);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)135, visible);
			}
			return true;
		}

		private bool SetSpectatorCollisionEnabled(PlayerEntity playerEntity, MainCharacterEntity mainEntity, bool enable)
		{
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			ref PlayerComponent state = ref playerEntity.GetState();
			Logging.LogDebug("Setting player {PlayerName} collision to: {Enabled}", state.NickName, enable);
			if ((UObject)(object)localState.Pawn == (UObject)null)
			{
				Logging.LogError("Player pawn is null");
				return false;
			}
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)1, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)38, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)30, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)165, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)6, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)138, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)40, enable);
			}
			if (obj != null)
			{
				obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)73, enable);
			}
			if (enable)
			{
				((ACharacter)localState.Pawn).CharacterMovement.GravityScale = _gravityScale;
				PlayerUtils.TeleportLocalPlayer(mainEntity, _lastValidLocation, default(FRotator), setLookAt: false);
			}
			else
			{
				_gravityScale = ((ACharacter)localState.Pawn).CharacterMovement.GravityScale;
				((ACharacter)localState.Pawn).CharacterMovement.GravityScale = 0f;
				_lastValidLocation = ((AActor)localState.Pawn).GetActorLocation();
				FVector val = default(FVector);
				((FVector)(ref val))..ctor(0.0, 0.0, (double)(((ACharacter)localState.Pawn).CapsuleComponent.GetScaledCapsuleHalfHeight() * -3f));
				FHitResult val2 = default(FHitResult);
				((AActor)localState.Pawn).SetActorLocation(_lastValidLocation + val, false, ref val2, true);
			}
			((UMovementComponent)((ACharacter)localState.Pawn).CharacterMovement).StopMovementImmediately();
			PlayerUtils.SetCollisionEnabled(localState.Pawn, enable);
			return true;
		}
	}
	public class WukongRpcCallbacks : IDisposable
	{
		protected readonly RelaySerializer Serializer;

		protected readonly IRelayClient RelayClient;

		private readonly ClientState _state;

		private readonly WukongAreaState _areaState;

		private readonly ClientNetworkedEntityState _netEntity;

		private readonly WukongPlayerState _playerState;

		private readonly WukongPawnState _pawnState;

		private readonly ClientOwnershipManager _clientOwnership;

		private readonly GameplayEventRouter _eventRouter;

		private readonly WukongWidgetManager _widgetManager;

		private readonly TimerController _timerController;

		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly ILogger _logger;

		private static readonly MethodInfo PlayDBC_ByType = AccessTools.Method(typeof(BGU_AbnormalStateHandlerBase), "PlayDBC_ByType", (Type[])null, (Type[])null);

		private static readonly MethodInfo EndAllDBC = AccessTools.Method(typeof(BGU_AbnormalStateHandlerBase), "EndAllDBC", (Type[])null, (Type[])null);

		[Obsolete("To be removed once per-file RPC is implemented")]
		public event Action<ChatMessage>? OnGetChatMessage;

		[Obsolete("To be removed once per-project RPC is implemented")]
		public event Action<int[]>? OnPvpEventReceived;

		public WukongRpcCallbacks(RelaySerializer serializer, IRelayClient relayClient, ClientState state, WukongAreaState areaState, ClientNetworkedEntityState netEntity, WukongPlayerState playerState, WukongPawnState pawnState, ClientOwnershipManager clientOwnership, GameplayEventRouter eventRouter, WukongWidgetManager widgetManager, TimerController timerController, IClientEcsUpdateLoop ecsLoop, ILogger logger)
		{
			Serializer = serializer;
			RelayClient = relayClient;
			_state = state;
			_areaState = areaState;
			_netEntity = netEntity;
			_playerState = playerState;
			_pawnState = pawnState;
			_clientOwnership = clientOwnership;
			_eventRouter = eventRouter;
			_widgetManager = widgetManager;
			_timerController = timerController;
			_ecsLoop = ecsLoop;
			_logger = logger;
			InitRpc();
		}

		public void Dispose()
		{
			DeInitRpc();
		}

		public void SendMontageCallback(NetworkId netId, UAnimMontage montage, float position, bool reset)
		{
			string shortName;
			bool flag = Compressors.MontageNameCompressor.Compress(((UObject)montage).PathName, out shortName);
			string text = (flag ? shortName : ((UObject)montage).PathName);
			MontageCallbackData payload = new MontageCallbackData(netId, flag, text, position, reset);
			_logger.LogDebug("Sent montage for {NetId} at {Position} - {Montage}", netId, position, text);
			SendMontageCallback(payload);
		}

		public void SendMontageCancel(NetworkId netId)
		{
			MontageCallbackData payload = new MontageCallbackData(netId, compressed: false, "", 0f, reset: false);
			_logger.LogDebug("Sent montage cancel for entity {NetId}", netId);
			SendMontageCallback(payload);
		}

		public void SendTriggerMagicallyChange(PlayerId player, UBGWDataAsset config, int skillID, int recoverSkillID, int curVigorSkillID, ECastReason_MagicallyChange castReason)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			string shortName;
			bool flag = Compressors.VigorNameCompressor.Compress(((UObject)config).PathName, out shortName);
			string text = (flag ? shortName : ((UObject)config).PathName);
			_logger.LogDebug("Sending magically change for player {PlayerId} with config {Config}, skillID {SkillID}, recoverSkillID {RecoverSkillId}, curVigorSkillID {CurVigorSkillID}, castReason {CastReason}", player, text, skillID, recoverSkillID, curVigorSkillID, castReason);
			MagicallyChangeData payload = new MagicallyChangeData(text, flag, skillID, recoverSkillID, curVigorSkillID, castReason);
			SendTriggerMagicallyChange(payload);
		}

		public void SendPlayMovieRequest(FPlayMovieRequest playMovieRequest)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			SendPlayMovieRequest(new PlayMovieData(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID, ((FPlayMovieRequest)(ref playMovieRequest)).bDisablePlayerControl, ((FPlayMovieRequest)(ref playMovieRequest)).bDisableMovementInput, ((FPlayMovieRequest)(ref playMovieRequest)).bDisableLookAtInput, ((FPlayMovieRequest)(ref playMovieRequest)).bHidePlayer, ((FPlayMovieRequest)(ref playMovieRequest)).bHideHud, "", ((FPlayMovieRequest)(ref playMovieRequest)).MatchType));
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnExitPhantomRush(PlayerId playerId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId playerId2)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(playerId2);
				if (mainCharacterById.HasValue)
				{
					MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
					ref MainCharacterComponent state = ref valueOrDefault.GetState();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					self._logger.LogDebug("Received exit phantom rush for player {Nickname}", state.CharacterNickName);
					BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
					localState.ReceivedPhantomRushExit = true;
					if (val != null)
					{
						val.Evt_RelievePhantomRush.Invoke();
					}
				}
			}, this, playerId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnAddBuff(PlayerId __sender, BuffAddData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, BuffAddData buffAddData)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(buffAddData.Id);
				if (!((UObject)(object)pawnByNetworkId == (UObject)null))
				{
					BuffUtils.AddBuff(pawnByNetworkId, buffAddData.BuffId, buffAddData.Duration);
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnRemoveBuff(PlayerId __sender, BuffRemoveData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, BuffRemoveData buffRemoveData)
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(buffRemoveData.Id);
				if (!((UObject)(object)pawnByNetworkId == (UObject)null))
				{
					BuffUtils.RemoveBuff(pawnByNetworkId, buffRemoveData.BuffId, buffRemoveData.TriggerType, buffRemoveData.Layer, buffRemoveData.WithTriggerRemoveEffect);
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnRemoveAllBuffs(PlayerId __sender, BuffRemoveAllData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, BuffRemoveAllData buffRemoveAllData)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(buffRemoveAllData.Id);
				if (!((UObject)(object)pawnByNetworkId == (UObject)null))
				{
					BuffUtils.RemoveAllBuffs(pawnByNetworkId, buffRemoveAllData.TriggerType, buffRemoveAllData.WithTriggerRemoveEffect);
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnUnitStateTrigger(StateTriggerData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, StateTriggerData stateTriggerData)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				NpcLocomotionUtils.SetStateTrigger(self._pawnState.GetPawnByNetworkId(stateTriggerData.NetId), stateTriggerData.Trigger, stateTriggerData.Time, stateTriggerData.NeedForceUpdate);
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnUnitSimpleState(SimpleStateData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, SimpleStateData simpleStateData)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				NpcLocomotionUtils.SetSimpleState(self._pawnState.GetPawnByNetworkId(simpleStateData.NetId), simpleStateData.SimpleState, simpleStateData.IsRemove);
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnTriggerFsmState(FsmStateData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, FsmStateData fsmStateData)
			{
				NpcLocomotionUtils.SetFsmState(self._pawnState.GetPawnByNetworkId(fsmStateData.NetId), fsmStateData.FsmStateName);
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnMotionMatchingState(MotionMatchingStateData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, MotionMatchingStateData motionMatchingStateData)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				NpcLocomotionUtils.SetMotionMatchingState(self._pawnState.GetPawnByNetworkId(motionMatchingStateData.NetId), motionMatchingStateData.State);
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnSpawnSummon(SummonRequestData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, SummonRequestData value)
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				self._logger.LogDebug("Received OnSpawnSummon for summoner {Summoner} with guid {Guid} for tamer path {Path}", value.SummonerId, value.SummonGuid, value.SummonClassPath);
				SpawningUtils.SpawnSummonedUnitWithGuid(value.ToGame());
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnRequestSpawnUnits(PlayerId __sender, UnitSpawnRequestData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, UnitSpawnRequestData unitSpawnRequestData)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				if (self._areaState.IsMasterClient)
				{
					SpawningUtils.SpawnUnitsAsOwner(unitSpawnRequestData.UnitName, unitSpawnRequestData.Count, unitSpawnRequestData.TeamId, unitSpawnRequestData.Location);
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnSpawnUnit(PlayerId __sender, UnitSpawnData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, UnitSpawnData unitSpawnData)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				SpawningUtils.SpawnUnitLocallyByName(unitSpawnData.Guid, unitSpawnData.UnitName, unitSpawnData.Location);
			}, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnPlayerTransBegin(PlayerId __sender, PlayerTransBeginData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, PlayerTransBeginData playerTransBeginData)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (mainCharacterById.HasValue)
				{
					TransformationUtils.TransformPlayer(mainCharacterById.GetValueOrDefault(), playerTransBeginData.UnitResId, playerTransBeginData.UnitBornSkillId, playerTransBeginData.EnableBlendViewTarget, playerTransBeginData.TransBeginType);
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnPlayerTransEnd(PlayerId __sender, PlayerTransEndData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, PlayerTransEndData playerTransEndData)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (mainCharacterById.HasValue)
				{
					TransformationUtils.TransformPlayerBack(mainCharacterById.GetValueOrDefault(), playerTransEndData.UnitResId, playerTransEndData.UnitBornSkillId, playerTransEndData.EnableBlendViewTarget, playerTransEndData.TransEndType);
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnPlayMovieRequest(PlayMovieData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, PlayMovieData data2)
			{
				CutsceneUtils.PlayCutscene(data2);
			}, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnSetTarget(TargetData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, TargetData targetData)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(targetData.Character);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("Character");
				}
				else if (targetData.ClearTarget)
				{
					TargetingUtils.ClearTarget(pawnByNetworkId);
				}
				else
				{
					BGUCharacterCS pawnByNetworkId2 = self._pawnState.GetPawnByNetworkId(targetData.Target);
					if ((UObject)(object)pawnByNetworkId2 == (UObject)null)
					{
						self._logger.LogNull("Target");
					}
					else
					{
						TargetingUtils.SetTarget(pawnByNetworkId, pawnByNetworkId2);
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnCastImmobilize(NetworkId caster)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, NetworkId netId)
			{
				if (self._areaState.IsMasterClient)
				{
					BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(netId);
					if ((UObject)(object)pawnByNetworkId == (UObject)null)
					{
						self._logger.LogNull("caster0");
					}
					else
					{
						ImmobilizeUtils.CastImmobilize(pawnByNetworkId);
					}
				}
			}, this, caster);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnTriggerImmobilize(TriggerImmobilizeData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, TriggerImmobilizeData triggerImmobilizeData)
			{
				BGUCharacterCS? pawnByNetworkId = self._pawnState.GetPawnByNetworkId(triggerImmobilizeData.PlayerId);
				BGUCharacterCS pawnByNetworkId2 = self._pawnState.GetPawnByNetworkId(triggerImmobilizeData.Target);
				ImmobilizeUtils.TriggerImmobilize(pawnByNetworkId, pawnByNetworkId2, triggerImmobilizeData.GreatSageTalentActiveBuff);
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnRelieveImmobilize(NetworkId affected)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, NetworkId netId)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(netId);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("affected0");
				}
				else
				{
					ImmobilizeUtils.RelieveImmobilize(pawnByNetworkId);
				}
			}, this, affected);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnBreakImmobilize(NetworkId netId)
		{
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnChatMessage(ChatMessage message)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, ChatMessage obj)
			{
				self.OnGetChatMessage?.Invoke(obj);
			}, this, message);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnPhantomRush(PlayerId __sender, ESkillDirection direction)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, ESkillDirection val)
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (mainCharacterById.HasValue)
				{
					MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
					ref MainCharacterComponent state = ref valueOrDefault.GetState();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player not found: {PlayerId}", state.PlayerId);
					}
					else
					{
						self._logger.LogDebug("Received phantom rush for player {Nickname} in direction {Direction}", state.CharacterNickName, val);
						BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
						if (obj != null)
						{
							obj.Evt_TriggerPhantomRush.Invoke(val);
						}
						PlayerUtils.ResetCooldown((APawn)(object)localState.Pawn);
						PlayerUtils.ResetMana((APawn)(object)localState.Pawn);
						if (self._playerState.LocalMainCharacter.HasValue)
						{
							BGUCharacterCS pawn = self._playerState.LocalMainCharacter.Value.GetLocalState().Pawn;
							if ((UObject)(object)pawn != (UObject)null)
							{
								IBUC_TargetInfoData readOnlyData = BGU_DataUtil.GetReadOnlyData<IBUC_TargetInfoData, BUC_TargetInfoData>((AActor)(object)pawn);
								object obj2;
								if (readOnlyData == null)
								{
									obj2 = null;
								}
								else
								{
									UnitLockTargetInfo targetInfo = readOnlyData.GetTargetInfo();
									obj2 = ((targetInfo != null) ? targetInfo.LockTargetActor : null);
								}
								if ((UObject)obj2 == (UObject)(object)localState.Pawn)
								{
									GSDel_Void evt_ClearCameraLock = BUS_EventCollectionCS.Get((AActor)(object)pawn).Evt_ClearCameraLock;
									if (evt_ClearCameraLock != null)
									{
										evt_ClearCameraLock.Invoke();
									}
								}
							}
						}
					}
				}
			}, this, __sender, direction);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		public void OnBroadcastPlayerTransform(PlayerTransformData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerTransformData playerTransformData)
			{
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					ref MainCharacterComponent state = ref valueOrDefault.GetState();
					if (!(playerTransformData.PlayerId != state.PlayerId))
					{
						PlayerUtils.TeleportLocalPlayer(valueOrDefault, playerTransformData.Location, playerTransformData.Rotation);
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnRebirthPlayer(PlayerId playerId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId playerId2)
			{
				self._logger.LogDebug("RebirthPlayer for player {PlayerId} called", playerId2);
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(playerId2);
				if (mainCharacterById.HasValue)
				{
					MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
					PlayerId? localPlayerId = self._state.LocalPlayerId;
					if (playerId2 == localPlayerId)
					{
						PlayerUtils.DisableSpectator(valueOrDefault);
					}
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if ((UObject)(object)localState.Pawn != (UObject)null)
					{
						PlayerUtils.RebirthPlayerInPlace(localState.Pawn);
					}
				}
			}, this, playerId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnDamageNum(DamageNumParam damageNum)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, DamageNumParam val)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				BGW_UIEventCollection.Get((UObject)(object)GameUtils.GetWorld()).Evt_UI_ShowHPChangeNum.Invoke(val);
			}, damageNum);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnTeleportFinish(PlayerId __sender)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {PlayerId}", sender);
				}
				else
				{
					BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)mainCharacterById.GetValueOrDefault().GetLocalState().Pawn);
					if (obj != null)
					{
						obj.Evt_UnitStateTrigger.Invoke((EBUStateTrigger)78, -1f, false);
					}
					if (obj != null)
					{
						obj.Evt_TeleportFinish.Invoke();
					}
				}
			}, this, __sender);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		public void OnMontageCallback(MontageCallbackData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, MontageCallbackData montageCallbackData)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(montageCallbackData.NetId);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("NetId");
				}
				else if (string.IsNullOrEmpty(montageCallbackData.MontagePath))
				{
					UAnimMontage currentMontage = ((ACharacter)pawnByNetworkId).GetCurrentMontage();
					if ((UObject)(object)currentMontage != (UObject)null)
					{
						float num = ((ACharacter)pawnByNetworkId).Mesh.GetAnimInstance().Montage_GetPosition(currentMontage);
						self._logger.LogDebug("Received montage cancel at {Time} for entity {NetId} - {Montage}", num, montageCallbackData.NetId, ((UObject)currentMontage).PathName);
					}
					((ACharacter)pawnByNetworkId).StopAnimMontage((UAnimMontage)null);
				}
				else
				{
					string text = (montageCallbackData.Compressed ? Compressors.MontageNameCompressor.Decompress(montageCallbackData.MontagePath) : montageCallbackData.MontagePath);
					if ((UObject)(object)((ACharacter)pawnByNetworkId).Mesh == (UObject)null)
					{
						self._logger.LogError("pawn.Mesh is null");
					}
					else
					{
						UAnimInstance animInstance = ((ACharacter)pawnByNetworkId).Mesh.GetAnimInstance();
						if ((UObject)(object)animInstance == (UObject)null)
						{
							self._logger.LogError("AnimInstance is null");
						}
						else
						{
							UAnimMontage currentActiveMontage = animInstance.GetCurrentActiveMontage();
							if (!((UObject)(object)currentActiveMontage != (UObject)null) || !(((UObject)currentActiveMontage).PathName == text) || montageCallbackData.Reset)
							{
								UAnimMontage val = BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UAnimMontage>(text, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
								if ((UObject)(object)val == (UObject)null)
								{
									if (!text.Contains("Engine/Transient.AnimMontage"))
									{
										self._logger.LogWarning("Montage not found: {Montage}", text);
									}
								}
								else
								{
									BUS_GSEventCollection val2 = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId);
									if ((UObject)(object)val2 == (UObject)null)
									{
										self._logger.LogError("events are null");
									}
									else
									{
										if (montageCallbackData.MontagePath == "LYS/LYS_KJLDragon/new/Montage/AM_LYS_KJLDragon_Atk_14_monster")
										{
											self._logger.LogDebug("Received host attack with offset {Offset} (reset: {Reset})", montageCallbackData.Position, montageCallbackData.Reset);
										}
										animInstance.Montage_Play(val, 1f, (EMontagePlayReturnType)0, montageCallbackData.Position, true);
										val2.Evt_PlayMontageCallback.Invoke((EMontageBindReason)0, val, (EMontageCallbackState)1);
									}
								}
							}
						}
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		public void OnAnimationSyncing(AnimationSyncingData data)
		{
			_logger.LogDebug("OnPreAnimationSyncing called for Host {Host} and Guest {Guest}", data.Host, data.Guest);
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, AnimationSyncingData animationSyncingData)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(animationSyncingData.Host);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("Host");
				}
				else
				{
					BGUCharacterCS pawnByNetworkId2 = self._pawnState.GetPawnByNetworkId(animationSyncingData.Guest);
					if ((UObject)(object)pawnByNetworkId2 == (UObject)null)
					{
						self._logger.LogNullDebug("Guest");
					}
					else
					{
						BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId);
						if (obj != null)
						{
							GSDel_NotifyEnterPreAnimationSyncingState evt_NotifyEnterPreAnimationSyncingStateOnHost = obj.Evt_NotifyEnterPreAnimationSyncingStateOnHost;
							if (evt_NotifyEnterPreAnimationSyncingStateOnHost != null)
							{
								evt_NotifyEnterPreAnimationSyncingStateOnHost.Invoke((AActor)(object)pawnByNetworkId2, new List<int>());
							}
						}
						BUS_GSEventCollection obj2 = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId2);
						if (obj2 != null)
						{
							GSDel_NotifyEnterPreAnimationSyncingState evt_NotifyEnterPreAnimationSyncingStateOnGuest = obj2.Evt_NotifyEnterPreAnimationSyncingStateOnGuest;
							if (evt_NotifyEnterPreAnimationSyncingStateOnGuest != null)
							{
								evt_NotifyEnterPreAnimationSyncingStateOnGuest.Invoke((AActor)(object)pawnByNetworkId, new List<int>());
							}
						}
						string text = (animationSyncingData.Compressed ? Compressors.MontageNameCompressor.Decompress(animationSyncingData.Montage) : animationSyncingData.Montage);
						UAnimMontage val = (string.IsNullOrEmpty(text) ? null : BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UAnimMontage>(text, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1)));
						BUS_GSEventCollection obj3 = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId);
						if (obj3 != null)
						{
							GSDel_NotifyEnterAnimationSyncingStateOnHost evt_NotifyEnterAnimationSyncingStateOnHost = obj3.Evt_NotifyEnterAnimationSyncingStateOnHost;
							if (evt_NotifyEnterAnimationSyncingStateOnHost != null)
							{
								evt_NotifyEnterAnimationSyncingStateOnHost.Invoke(new List<int>(), val);
							}
						}
						BUS_GSEventCollection obj4 = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId2);
						if (obj4 != null)
						{
							GSDel_NotifyEnterAnimationSyncingStateOnGuest evt_NotifyEnterAnimationSyncingStateOnGuest = obj4.Evt_NotifyEnterAnimationSyncingStateOnGuest;
							if (evt_NotifyEnterAnimationSyncingStateOnGuest != null)
							{
								evt_NotifyEnterAnimationSyncingStateOnGuest.Invoke(new List<int>());
							}
						}
						BGC_AnimationSyncData readOnlyData = BGU_DataUtil.GetReadOnlyData<BGC_AnimationSyncData>((AActor)(object)UGameplayStatics.GetGameState((UObject)(object)GameUtils.GetWorld()));
						if (readOnlyData != null)
						{
							readOnlyData.AddParticipants((AActor)(object)pawnByNetworkId, (AActor)(object)pawnByNetworkId2);
						}
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnBeginSyncAnimation(BeginSyncAnimationData data)
		{
			_logger.LogDebug("OnBeginSyncAnimation called for Host {NetId} with GuestMontage '{MontagePath}'", data.Host, data.GuestMontage);
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, BeginSyncAnimationData beginSyncAnimationData)
			{
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(beginSyncAnimationData.Host);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("Host");
				}
				else
				{
					string text = (beginSyncAnimationData.Shortened ? Compressors.MontageNameCompressor.Decompress(beginSyncAnimationData.GuestMontage) : beginSyncAnimationData.GuestMontage);
					UAnimMontage val = (string.IsNullOrEmpty(text) ? null : BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UAnimMontage>(text, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1)));
					BGS_GSEventCollection val2 = BGS_GSEventCollection.Get((AActor)(object)pawnByNetworkId);
					if ((UObject)(object)val2 == (UObject)null)
					{
						self._logger.LogError("Failed to get event collection for unit {Unit}", ((UObject)pawnByNetworkId).GetName());
					}
					else
					{
						GSDel_BGS_BeginSyncAnimation evt_BGS_BeginSyncAnimation = val2.Evt_BGS_BeginSyncAnimation;
						if (evt_BGS_BeginSyncAnimation != null)
						{
							evt_BGS_BeginSyncAnimation.Invoke((AActor)(object)pawnByNetworkId, val, beginSyncAnimationData.bFoundHostSyncPointOnDummyMesh, new FName(beginSyncAnimationData.SelfSyncPointOnHost, (EFindName)1), new FName(beginSyncAnimationData.TargetSyncPointOnHost, (EFindName)1), new FName(beginSyncAnimationData.SelfSyncPointOnGuest, (EFindName)1), beginSyncAnimationData.bForceSyncDummyMeshAnimation, beginSyncAnimationData.bEnableDebugDraw, beginSyncAnimationData.NotifyBeginTime, beginSyncAnimationData.TotalDuration, beginSyncAnimationData.AnimationSyncMontageInstanceId);
						}
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnUnitDead(UnitDeadPacket data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, UnitDeadPacket unitDeadPacket)
			{
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(unitDeadPacket.NetworkId);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("NetworkId");
				}
				else
				{
					BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId);
					if ((UObject)(object)val == (UObject)null)
					{
						self._logger.LogError("Failed to get event collection for unit {Unit}", ((UObject)pawnByNetworkId).GetName());
					}
					else
					{
						self._logger.LogDebug("OnUnitDead for unit {Unit}", ((UObject)pawnByNetworkId).GetName());
						val.Evt_UnitDead.Invoke((AActor)(object)GameUtils.GetControlledPawn(), unitDeadPacket.DeadReason, unitDeadPacket.DmgId, unitDeadPacket.StiffLevel, (UAnimMontage)null, default(FEffectInstReq), unitDeadPacket.IsDotDmg, unitDeadPacket.AbnormalType);
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.GlobalOthers)]
		internal void OnWaitingForSequence(SequenceWaitingData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, SequenceWaitingData joiningCutsceneStatus)
			{
				CutsceneUtils.SetJoiningCutsceneStatus(joiningCutsceneStatus);
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if (!((UObject)(object)localState.Pawn == (UObject)null) && valueOrDefault.GetState().IsDead)
					{
						PlayerUtils.DisableSpectator(valueOrDefault);
						PlayerUtils.RebirthPlayerInPlace(localState.Pawn);
						CutsceneUtils.TeleportLocalPlayerToCutsceneLocation();
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnIronBodyStart(PlayerId __sender)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						IronBodyUtils.TriggerIronBody(localState.Pawn);
					}
				}
			}, this, __sender);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		private void OnUnitSpawned(PlayerId __sender, NetworkId netId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, NetworkId netEntity0)
			{
				self._logger.LogDebug("OnUnitSpawned called for player {PlayerId} with entity netId: {NetId}", sender, netEntity0);
				if (self._clientOwnership.OwnsEntity(netEntity0))
				{
					Entity? entity;
					if (!self._playerState.GetMainCharacterById(sender).HasValue)
					{
						self._logger.LogError("Player not found: {Id}", sender);
					}
					else if (self._netEntity.TryGetEntityByNetworkId(netEntity0, out entity))
					{
						TamerUtils.AddSpawnedUnitRefCount(sender, new TamerEntity(entity.Value));
					}
				}
			}, this, __sender, netId);
		}

		[RpcEvent(RelayMode.EntityOwner)]
		private void OnUnitDespawn(PlayerId __sender, NetworkId netId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, NetworkId netEntity0)
			{
				self._logger.LogDebug("OnUnitDespawn called for player {PlayerId} with entity {Entity}", sender, netEntity0);
				Entity? entity;
				if (!self._playerState.GetMainCharacterById(sender).HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else if (self._netEntity.TryGetEntityByNetworkId(netEntity0, out entity))
				{
					TamerUtils.SubtractSpawnedUnitRefCount(sender, new TamerEntity(entity.Value));
				}
			}, this, __sender, netId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnTamerSkillInteract(SkillInteractData interactData)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, SkillInteractData skillInteractData)
			{
				if (self._netEntity.TryGetEntityByNetworkId(skillInteractData.InteractiveId, out var entity))
				{
					TamerUtils.TriggerSkillInteract(entity.Value, skillInteractData.SkillId);
				}
			}, this, interactData);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnTriggerMagicallyChange(PlayerId __sender, MagicallyChangeData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, MagicallyChangeData magicallyChangeData)
			{
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (mainCharacterById.HasValue)
				{
					MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
					ref MainCharacterComponent state = ref valueOrDefault.GetState();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						string text = (magicallyChangeData.Compressed ? Compressors.VigorNameCompressor.Decompress(magicallyChangeData.ConfigAssetName) : magicallyChangeData.ConfigAssetName);
						self._logger.LogDebug("Received trigger magically change for character {Nickname} with config {ConfigAssetPath}, skillID {SkillID}, recoverSkillID {RecoverSkillID}, curVigorSkillID {CurVigorSkillID}", state.CharacterNickName, text, magicallyChangeData.SkillID, magicallyChangeData.RecoverSkillID, magicallyChangeData.CurVigorSkillID);
						MagicallyChangeUtils.TriggerMagicallyChange(localState.Pawn, text, magicallyChangeData.SkillID, magicallyChangeData.RecoverSkillID, magicallyChangeData.CurVigorSkillID, magicallyChangeData.CastReason);
					}
				}
				else
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnResetMagicallyChange(PlayerId __sender, EResetReason_MagicallyChange reason)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, EResetReason_MagicallyChange val)
			{
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (mainCharacterById.HasValue)
				{
					MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
					ref MainCharacterComponent state = ref valueOrDefault.GetState();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						self._logger.LogDebug("Received reset magically change for character {Nickname} with reason {Reason}", state.CharacterNickName, val);
						MagicallyChangeUtils.ResetMagicallyChange(localState.Pawn, val);
					}
				}
				else
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
			}, this, __sender, reason);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnProjectileTarget(PlayerId __sender, ProjectileTargetData targetData)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, ProjectileTargetData projectileTargetData)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(projectileTargetData.Target);
						if ((UObject)(object)pawnByNetworkId == (UObject)null)
						{
							self._logger.LogNull("Target");
						}
						else
						{
							ProjectileUtils.SetProjectileTarget(localState.Pawn, projectileTargetData.ProjectileName, pawnByNetworkId, projectileTargetData.SocketName);
						}
					}
				}
			}, this, __sender, targetData);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnSwitchOneProjectile(PlayerId __sender, ProjectileSwitchData switchData)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, ProjectileSwitchData projectileSwitchData)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						ProjectileUtils.SwitchProjectileInfo(localState.Pawn, projectileSwitchData.ProjectileClassName, projectileSwitchData.BulletSwitchID, projectileSwitchData.SwitchIdx);
					}
				}
			}, this, __sender, switchData);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnProjectileDead(PlayerId __sender, ProjectileDeadData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, ProjectileDeadData projectileDeadData)
			{
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						ProjectileUtils.DestroyProjectile(localState.Pawn, projectileDeadData.ProjectileClassName, projectileDeadData.Reason);
					}
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnMagicFieldDead(string magicFieldClassName, EBGUBulletDestroyReason reason)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, string magicFieldClassName2, EBGUBulletDestroyReason reason2)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				MagicFieldUtils.DestroyMagicField(magicFieldClassName2, reason2);
			}, this, magicFieldClassName, reason);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnProjectileMoveMode(PlayerId __sender, ProjectileMoveModeData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, ProjectileMoveModeData projectileMoveModeData)
			{
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						ProjectileUtils.SetProjectileMode(localState.Pawn, projectileMoveModeData.ProjectileClassName, projectileMoveModeData.MoveMode);
					}
				}
			}, this, __sender, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		private void OnPartyRespawn(int birthPointId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, int shrineId)
			{
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if (!((UObject)(object)localState.Pawn == (UObject)null))
					{
						localState.IsRespawning = true;
						PlayerUtils.DisableSpectator(valueOrDefault);
						CutsceneUtils.ClearLocalJoiningCutsceneStatus(valueOrDefault);
						self._eventRouter.RaiseOnLocalPlayerBeforeRebirth();
						PlayerUtils.RebirthDeadPlayer(localState.Pawn, shrineId);
					}
				}
			}, this, birthPointId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnAfterRebirth(PlayerId __sender)
		{
			MainCharacterEntity? mainCharacterById = _playerState.GetMainCharacterById(__sender);
			if (mainCharacterById.HasValue)
			{
				BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)mainCharacterById.GetValueOrDefault().GetLocalState().Pawn);
				if ((UObject)(object)val != (UObject)null)
				{
					val.Evt_AfterUnitRebirth.Invoke((ERebirthType)0);
				}
			}
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnRestAtShrine(int birthPointId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, int shrineId)
			{
				foreach (PlayerId areaPlayer in DI.Instance.State.AreaPlayers)
				{
					MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(areaPlayer);
					if (mainCharacterById.HasValue)
					{
						MainCharacterEntity valueOrDefault = mainCharacterById.GetValueOrDefault();
						ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
						if (!((UObject)(object)localState.Pawn == (UObject)null))
						{
							if (valueOrDefault.GetState().IsDead)
							{
								localState.IsRespawning = true;
								PlayerUtils.DisableSpectator(valueOrDefault);
								PlayerUtils.RebirthDeadPlayer(localState.Pawn, shrineId);
							}
							else
							{
								PlayerUtils.RestPlayer(localState.Pawn);
							}
						}
					}
				}
			}, this, birthPointId);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		private void OnPartySoftlock(int birthPointId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, int shrineId)
			{
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					if (!((UObject)(object)localState.Pawn == (UObject)null))
					{
						localState.IsRespawning = true;
						PlayerUtils.DisableSpectator(valueOrDefault);
						CutsceneUtils.ClearLocalJoiningCutsceneStatus(valueOrDefault);
						self._eventRouter.RaiseOnLocalPlayerBeforeRebirth();
						PlayerUtils.RebirthAlivePlayer(localState.Pawn, shrineId);
					}
				}
			}, this, birthPointId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnStartJump(PlayerId __sender, StartJumpData jumpData)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender, StartJumpData startJumpData)
			{
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						PlayerUtils.StartJump(localState.Pawn, startJumpData.StartJumpDir, startJumpData.InputVector);
					}
				}
			}, this, __sender, jumpData);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnStopJump(PlayerId __sender)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayerId sender)
			{
				MainCharacterEntity? mainCharacterById = self._playerState.GetMainCharacterById(sender);
				if (!mainCharacterById.HasValue)
				{
					self._logger.LogError("Player not found: {Id}", sender);
				}
				else
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.GetValueOrDefault().GetLocalState();
					if ((UObject)(object)localState.Pawn == (UObject)null)
					{
						self._logger.LogError("Player pawn is null for player {Id}", sender);
					}
					else
					{
						PlayerUtils.StopJump(localState.Pawn);
					}
				}
			}, this, __sender);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnMonsterWakeUp(NetworkId netId)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, NetworkId netId2)
			{
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(netId2);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("netId0");
				}
				else
				{
					string actorGuid = BGU_DataUtil.GetActorGuid((AActor)(object)pawnByNetworkId, false);
					Logging.LogDebug("OnMonsterWakeup called for monster {Guid}", actorGuid);
					TamerUtils.TriggerWakeUp(pawnByNetworkId);
				}
			}, this, netId);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnPlayBaneEffect(PlayBaneEffectData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, PlayBaneEffectData playBaneEffectData)
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(playBaneEffectData.Id);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("Id");
				}
				else
				{
					BUC_AbnormalStateHandlers unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_AbnormalStateHandlers>((AActor)(object)pawnByNetworkId);
					if (unPersistentReadOnlyData != null)
					{
						BGU_AbnormalStateHandlerBase abnormalHanddler = unPersistentReadOnlyData.GetAbnormalHanddler(playBaneEffectData.StateType);
						PlayDBC_ByType.Invoke(abnormalHanddler, new object[3]
						{
							playBaneEffectData.ActionType,
							(object)default(FTransform),
							-1
						});
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		private void OnStopBaneEffect(StopBaneEffectData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, StopBaneEffectData stopBaneEffectData)
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(stopBaneEffectData.Id);
				if ((UObject)(object)pawnByNetworkId == (UObject)null)
				{
					self._logger.LogNullDebug("Id");
				}
				else
				{
					BUC_AbnormalStateHandlers unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_AbnormalStateHandlers>((AActor)(object)pawnByNetworkId);
					if (unPersistentReadOnlyData != null)
					{
						BGU_AbnormalStateHandlerBase abnormalHanddler = unPersistentReadOnlyData.GetAbnormalHanddler(stopBaneEffectData.StateType);
						EndAllDBC.Invoke(abnormalHanddler, Array.Empty<object>());
					}
				}
			}, this, data);
		}

		[RpcEvent(RelayMode.AreaOfInterestOthers)]
		internal void OnCastSkill(NetworkId caster, int skillId, ECastSkillSourceType skillType)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, NetworkId networkId, int num, ECastSkillSourceType val)
			{
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				BGUCharacterCS pawnByNetworkId = self._pawnState.GetPawnByNetworkId(networkId);
				if (pawnByNetworkId == null)
				{
					self._logger.LogError("Caster pawn not found: {NetId}", networkId);
				}
				else
				{
					Logging.LogDebug("OnCastSkill called for caster {Caster} with skillId {SkillId} and skillType {SkillType}", BGU_DataUtil.GetActorGuid((AActor)(object)pawnByNetworkId, false), num, val);
					BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)pawnByNetworkId);
					if (obj != null)
					{
						obj.Evt_UnitCastSkillTry.Invoke(new FCastSkillInfo(num, val, false, (ESkillDirection)0, (EMontageBindReason)0));
					}
				}
			}, this, caster, skillId, skillType);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnShowAntiStallWarning(int warningTime)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, int seconds)
			{
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue && !localMainCharacter.GetValueOrDefault().GetState().IsDead)
				{
					self._widgetManager.ShowInfoMessage(Texts.AntiStallWarning);
					self._timerController.SetTimer(0, seconds);
					self._timerController.StartTimer();
					Logging.LogDebug("OnShowAntiStallWarning received");
				}
			}, this, warningTime);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnShowAntiStallAction()
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self)
			{
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue && !localMainCharacter.GetValueOrDefault().GetState().IsDead)
				{
					self._widgetManager.ShowInfoMessage(Texts.StallingMessage);
					Logging.LogDebug("OnShowAntiStallAction received");
				}
			}, this);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnHideAntiStall()
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self)
			{
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					localMainCharacter.GetValueOrDefault();
					self._widgetManager.HideInfoMessage();
					self._timerController.StopTimer();
					self._widgetManager.SetTimerVisibility(visible: false);
					Logging.LogDebug("OnHideAntiStallWarning received");
				}
			}, this);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnStallDamage(NetworkId netId, float value)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongRpcCallbacks self, NetworkId netId2, float num)
			{
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					if (!valueOrDefault.GetState().IsDead && self._netEntity.TryGetEntityByNetworkId(netId2, out var entity) && entity == valueOrDefault.Entity)
					{
						Logging.LogDebug("Applying stall damage: {Damage}%", num);
						BGUCharacterCS pawn = valueOrDefault.GetLocalState().Pawn;
						if (!((UObject)(object)pawn == (UObject)null))
						{
							IBUC_AttrContainer readOnlyData = BGU_DataUtil.GetReadOnlyData<IBUC_AttrContainer, BUC_AttrContainer>((AActor)(object)pawn);
							float num2 = ((readOnlyData != null) ? readOnlyData.GetFloatValue((EBGUAttrFloat)8) : 1f);
							FSkillDamageConfig val = new FSkillDamageConfig
							{
								DamageCalcType = (EDamageCalcType)2,
								HPMaxINV10000Damage_Abs = num * 100f,
								DamageImmueLevel = 2,
								DmgReason = (EDamageReason)3
							};
							BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)pawn);
							if (obj != null)
							{
								obj.Evt_IncreaseAttrFloat.Invoke((EBGUAttrFloat)158, 0f - num2 * num / 100f * 3f);
							}
							if (obj != null)
							{
								GSDel_TriggerNormalDamageEffect evt_TriggerNormalDamageEffect = obj.Evt_TriggerNormalDamageEffect;
								FEffectInstReq val2 = default(FEffectInstReq);
								FBattleAttrSnapShot val3 = new FBattleAttrSnapShot((AActor)null, false);
								evt_TriggerNormalDamageEffect.Invoke((AActor)null, ref val, ref val2, ref val3);
							}
						}
					}
				}
			}, this, netId, value);
		}

		[RpcEvent(RelayMode.AreaOfInterestAll)]
		internal void OnPvpEvent(int[] data)
		{
			this.OnPvpEventReceived?.Invoke(data);
		}

		public void SendAddBuff(BuffAddData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode(RelayMessageCode.MinClientRpcEvent, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendAfterRebirth()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)1, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendAnimationSyncing(AnimationSyncingData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)2, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendBeginSyncAnimation(BeginSyncAnimationData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)3, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendBreakImmobilize(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)4, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendBroadcastPlayerTransform(PlayerTransformData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)5, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendCastImmobilize(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)6, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendCastSkill(NetworkId payload0, int payload1, ECastSkillSourceType payload2)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)7, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				writer.Put(payload1);
				Serializer.SerializeObject(writer, payload2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendChatMessage(ChatMessage payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)8, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendDamageNum(DamageNumParam payload0)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)9, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				Serializer.SerializeObject(writer, payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendExitPhantomRush(PlayerId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)10, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendHideAntiStall()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)11, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendIronBodyStart()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)12, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendMagicFieldDead(string payload0, EBGUBulletDestroyReason payload1)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)13, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				writer.Put(payload0);
				Serializer.SerializeObject(writer, payload1);
				RelayClient.SendMessage(message);
			}
		}

		public void SendMonsterWakeUp(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)14, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendMontageCallback(MontageCallbackData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)15, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendMotionMatchingState(MotionMatchingStateData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)16, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPartyRespawn(int payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)17, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				message.Writer.Put(payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPartySoftlock(int payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)18, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				message.Writer.Put(payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPhantomRush(ESkillDirection payload0)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)19, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				Serializer.SerializeObject(writer, payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPlayBaneEffect(PlayBaneEffectData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)20, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPlayerTransBegin(PlayerTransBeginData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)21, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPlayerTransEnd(PlayerTransEndData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)22, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPlayMovieRequest(PlayMovieData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)23, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendProjectileDead(ProjectileDeadData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)24, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendProjectileMoveMode(ProjectileMoveModeData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)25, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendProjectileTarget(ProjectileTargetData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)26, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendPvpEvent(int[] payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)27, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				Serializer.SerializeObject(writer, payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRebirthPlayer(PlayerId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)28, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRelieveImmobilize(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)29, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRemoveAllBuffs(BuffRemoveAllData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)30, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRemoveBuff(BuffRemoveData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)31, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRequestSpawnUnits(UnitSpawnRequestData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)32, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendResetMagicallyChange(EResetReason_MagicallyChange payload0)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)33, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				Serializer.SerializeObject(writer, payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendRestAtShrine(int payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)34, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				message.Writer.Put(payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendSetTarget(TargetData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)35, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendShowAntiStallAction()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)36, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendShowAntiStallWarning(int payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)37, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				message.Writer.Put(payload0);
				RelayClient.SendMessage(message);
			}
		}

		public void SendSpawnSummon(SummonRequestData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)38, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendSpawnUnit(UnitSpawnData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)39, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendStallDamage(NetworkId payload0, float payload1)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)40, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				writer.Put(payload1);
				RelayClient.SendMessage(message);
			}
		}

		public void SendStartJump(StartJumpData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)41, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendStopBaneEffect(StopBaneEffectData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)42, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendStopJump()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)43, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendSwitchOneProjectile(ProjectileSwitchData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)44, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendTamerSkillInteract(SkillInteractData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)45, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendTeleportFinish()
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)46, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				RelayClient.SendMessage(message);
			}
		}

		public void SendTriggerFsmState(FsmStateData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)47, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendTriggerImmobilize(TriggerImmobilizeData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)48, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendTriggerMagicallyChange(MagicallyChangeData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)49, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendUnitDead(UnitDeadPacket payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)50, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendUnitDespawn(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)51, RelayClient.PlayerId.Value, RelayMode.EntityOwner, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendUnitSimpleState(SimpleStateData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)52, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendUnitSpawned(NetworkId payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)53, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestAll, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendUnitStateTrigger(StateTriggerData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)54, RelayClient.PlayerId.Value, RelayMode.AreaOfInterestOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		public void SendWaitingForSequence(SequenceWaitingData payload0)
		{
			if (RelayClient.PlayerId.HasValue)
			{
				RelayMessage message = RelayMessage.ByRelayMode((RelayMessageCode)55, RelayClient.PlayerId.Value, RelayMode.GlobalOthers, (DeliveryMethod)2);
				NetDataWriter writer = message.Writer;
				payload0.Serialize(writer);
				RelayClient.SendMessage(message);
			}
		}

		protected void OnCustomRpcMessageHandler(IRelayClientNetworkThreadContext context, CustomRelayEventHeader header, NetDataReader reader)
		{
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			switch ((byte)header.EventCode)
			{
			case 0:
			{
				BuffAddData data27 = default(BuffAddData);
				data27.Deserialize(reader);
				OnAddBuff(header.Sender, data27);
				break;
			}
			case 1:
				OnAfterRebirth(header.Sender);
				break;
			case 2:
			{
				AnimationSyncingData data26 = default(AnimationSyncingData);
				data26.Deserialize(reader);
				OnAnimationSyncing(data26);
				break;
			}
			case 3:
			{
				BeginSyncAnimationData data25 = default(BeginSyncAnimationData);
				data25.Deserialize(reader);
				OnBeginSyncAnimation(data25);
				break;
			}
			case 4:
			{
				NetworkId netId5 = default(NetworkId);
				netId5.Deserialize(reader);
				OnBreakImmobilize(netId5);
				break;
			}
			case 5:
			{
				PlayerTransformData data24 = default(PlayerTransformData);
				data24.Deserialize(reader);
				OnBroadcastPlayerTransform(data24);
				break;
			}
			case 6:
			{
				NetworkId caster2 = default(NetworkId);
				caster2.Deserialize(reader);
				OnCastImmobilize(caster2);
				break;
			}
			case 7:
			{
				NetworkId caster = default(NetworkId);
				caster.Deserialize(reader);
				int skillId = reader.GetInt();
				ECastSkillSourceType skillType = Serializer.DeserializeObject<ECastSkillSourceType>(reader);
				OnCastSkill(caster, skillId, skillType);
				break;
			}
			case 8:
			{
				ChatMessage message = default(ChatMessage);
				message.Deserialize(reader);
				OnChatMessage(message);
				break;
			}
			case 9:
			{
				DamageNumParam damageNum = Serializer.DeserializeObject<DamageNumParam>(reader);
				OnDamageNum(damageNum);
				break;
			}
			case 10:
			{
				PlayerId playerId2 = default(PlayerId);
				playerId2.Deserialize(reader);
				OnExitPhantomRush(playerId2);
				break;
			}
			case 11:
				OnHideAntiStall();
				break;
			case 12:
				OnIronBodyStart(header.Sender);
				break;
			case 13:
			{
				string magicFieldClassName = reader.GetString();
				EBGUBulletDestroyReason reason2 = Serializer.DeserializeObject<EBGUBulletDestroyReason>(reader);
				OnMagicFieldDead(magicFieldClassName, reason2);
				break;
			}
			case 14:
			{
				NetworkId netId4 = default(NetworkId);
				netId4.Deserialize(reader);
				OnMonsterWakeUp(netId4);
				break;
			}
			case 15:
			{
				MontageCallbackData data23 = default(MontageCallbackData);
				data23.Deserialize(reader);
				OnMontageCallback(data23);
				break;
			}
			case 16:
			{
				MotionMatchingStateData data22 = default(MotionMatchingStateData);
				data22.Deserialize(reader);
				OnMotionMatchingState(data22);
				break;
			}
			case 17:
			{
				int birthPointId3 = reader.GetInt();
				OnPartyRespawn(birthPointId3);
				break;
			}
			case 18:
			{
				int birthPointId2 = reader.GetInt();
				OnPartySoftlock(birthPointId2);
				break;
			}
			case 19:
			{
				ESkillDirection direction = Serializer.DeserializeObject<ESkillDirection>(reader);
				OnPhantomRush(header.Sender, direction);
				break;
			}
			case 20:
			{
				PlayBaneEffectData data21 = default(PlayBaneEffectData);
				data21.Deserialize(reader);
				OnPlayBaneEffect(data21);
				break;
			}
			case 21:
			{
				PlayerTransBeginData data20 = default(PlayerTransBeginData);
				data20.Deserialize(reader);
				OnPlayerTransBegin(header.Sender, data20);
				break;
			}
			case 22:
			{
				PlayerTransEndData data19 = default(PlayerTransEndData);
				data19.Deserialize(reader);
				OnPlayerTransEnd(header.Sender, data19);
				break;
			}
			case 23:
			{
				PlayMovieData data18 = default(PlayMovieData);
				data18.Deserialize(reader);
				OnPlayMovieRequest(data18);
				break;
			}
			case 24:
			{
				ProjectileDeadData data17 = default(ProjectileDeadData);
				data17.Deserialize(reader);
				OnProjectileDead(header.Sender, data17);
				break;
			}
			case 25:
			{
				ProjectileMoveModeData data16 = default(ProjectileMoveModeData);
				data16.Deserialize(reader);
				OnProjectileMoveMode(header.Sender, data16);
				break;
			}
			case 26:
			{
				ProjectileTargetData targetData = default(ProjectileTargetData);
				targetData.Deserialize(reader);
				OnProjectileTarget(header.Sender, targetData);
				break;
			}
			case 27:
			{
				int[] data15 = Serializer.DeserializeObject<int[]>(reader);
				OnPvpEvent(data15);
				break;
			}
			case 28:
			{
				PlayerId playerId = default(PlayerId);
				playerId.Deserialize(reader);
				OnRebirthPlayer(playerId);
				break;
			}
			case 29:
			{
				NetworkId affected = default(NetworkId);
				affected.Deserialize(reader);
				OnRelieveImmobilize(affected);
				break;
			}
			case 30:
			{
				BuffRemoveAllData data14 = default(BuffRemoveAllData);
				data14.Deserialize(reader);
				OnRemoveAllBuffs(header.Sender, data14);
				break;
			}
			case 31:
			{
				BuffRemoveData data13 = default(BuffRemoveData);
				data13.Deserialize(reader);
				OnRemoveBuff(header.Sender, data13);
				break;
			}
			case 32:
			{
				UnitSpawnRequestData data12 = default(UnitSpawnRequestData);
				data12.Deserialize(reader);
				OnRequestSpawnUnits(header.Sender, data12);
				break;
			}
			case 33:
			{
				EResetReason_MagicallyChange reason = Serializer.DeserializeObject<EResetReason_MagicallyChange>(reader);
				OnResetMagicallyChange(header.Sender, reason);
				break;
			}
			case 34:
			{
				int birthPointId = reader.GetInt();
				OnRestAtShrine(birthPointId);
				break;
			}
			case 35:
			{
				TargetData data11 = default(TargetData);
				data11.Deserialize(reader);
				OnSetTarget(data11);
				break;
			}
			case 36:
				OnShowAntiStallAction();
				break;
			case 37:
			{
				int warningTime = reader.GetInt();
				OnShowAntiStallWarning(warningTime);
				break;
			}
			case 38:
			{
				SummonRequestData data10 = default(SummonRequestData);
				data10.Deserialize(reader);
				OnSpawnSummon(data10);
				break;
			}
			case 39:
			{
				UnitSpawnData data9 = default(UnitSpawnData);
				data9.Deserialize(reader);
				OnSpawnUnit(header.Sender, data9);
				break;
			}
			case 40:
			{
				NetworkId netId3 = default(NetworkId);
				netId3.Deserialize(reader);
				float value = reader.GetFloat();
				OnStallDamage(netId3, value);
				break;
			}
			case 41:
			{
				StartJumpData jumpData = default(StartJumpData);
				jumpData.Deserialize(reader);
				OnStartJump(header.Sender, jumpData);
				break;
			}
			case 42:
			{
				StopBaneEffectData data8 = default(StopBaneEffectData);
				data8.Deserialize(reader);
				OnStopBaneEffect(data8);
				break;
			}
			case 43:
				OnStopJump(header.Sender);
				break;
			case 44:
			{
				ProjectileSwitchData switchData = default(ProjectileSwitchData);
				switchData.Deserialize(reader);
				OnSwitchOneProjectile(header.Sender, switchData);
				break;
			}
			case 45:
			{
				SkillInteractData interactData = default(SkillInteractData);
				interactData.Deserialize(reader);
				OnTamerSkillInteract(interactData);
				break;
			}
			case 46:
				OnTeleportFinish(header.Sender);
				break;
			case 47:
			{
				FsmStateData data7 = default(FsmStateData);
				data7.Deserialize(reader);
				OnTriggerFsmState(data7);
				break;
			}
			case 48:
			{
				TriggerImmobilizeData data6 = default(TriggerImmobilizeData);
				data6.Deserialize(reader);
				OnTriggerImmobilize(data6);
				break;
			}
			case 49:
			{
				MagicallyChangeData data5 = default(MagicallyChangeData);
				data5.Deserialize(reader);
				OnTriggerMagicallyChange(header.Sender, data5);
				break;
			}
			case 50:
			{
				UnitDeadPacket data4 = default(UnitDeadPacket);
				data4.Deserialize(reader);
				OnUnitDead(data4);
				break;
			}
			case 51:
			{
				NetworkId netId2 = default(NetworkId);
				netId2.Deserialize(reader);
				OnUnitDespawn(header.Sender, netId2);
				break;
			}
			case 52:
			{
				SimpleStateData data3 = default(SimpleStateData);
				data3.Deserialize(reader);
				OnUnitSimpleState(data3);
				break;
			}
			case 53:
			{
				NetworkId netId = default(NetworkId);
				netId.Deserialize(reader);
				OnUnitSpawned(header.Sender, netId);
				break;
			}
			case 54:
			{
				StateTriggerData data2 = default(StateTriggerData);
				data2.Deserialize(reader);
				OnUnitStateTrigger(data2);
				break;
			}
			case 55:
			{
				SequenceWaitingData data = default(SequenceWaitingData);
				data.Deserialize(reader);
				OnWaitingForSequence(data);
				break;
			}
			default:
				throw new InvalidOperationException($"Unknown event code: {header.EventCode}");
			}
		}

		protected void InitRpc()
		{
			RelayClient.AddClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, (RelayMessageCode)55, OnCustomRpcMessageHandler);
		}

		protected void DeInitRpc()
		{
			RelayClient.RemoveClientRpcMessageHandler(RelayMessageCode.MinClientRpcEvent, (RelayMessageCode)55, OnCustomRpcMessageHandler);
		}
	}
	public class WukongSaveRelay(IBlobClient blobClient, ILogger logger)
	{
		private static string PlayerSaveName => $"player_{LaunchParameters.Instance.UserGuid:N}.sav";

		public Task<bool> UploadBlobAsync(string name, byte[] content, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				return blobClient.UploadBlobAsync(new BlobInfo(name, content), ct);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to upload blob: {BlobName}", name);
				throw new OperationCanceledException("Failed to upload blob", ex);
			}
		}

		public Task<BlobInfo?> DownloadBlobAsync(string name, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				return blobClient.DownloadBlobAsync(name, ct);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to download blob: {BlobName}", name);
				throw new OperationCanceledException("Failed to download blob", ex);
			}
		}

		public Task<bool> UploadWorldSaveAsync(byte[] content, CancellationToken ct = default(CancellationToken))
		{
			return UploadBlobAsync("world.sav", content, ct);
		}

		public Task<BlobInfo?> DownloadWorldSaveAsync(CancellationToken ct = default(CancellationToken))
		{
			return DownloadBlobAsync("world.sav", ct);
		}

		public Task<bool> UploadPlayerSaveAsync(byte[] content, CancellationToken ct = default(CancellationToken))
		{
			return UploadBlobAsync(PlayerSaveName, content, ct);
		}

		public Task<BlobInfo?> DownloadPlayerSaveAsync(CancellationToken ct = default(CancellationToken))
		{
			return DownloadBlobAsync(PlayerSaveName, ct);
		}
	}
	public class WukongServerRpcCallbacks : IDisposable
	{
		protected readonly IRelayClient RelayClient;

		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly ILogger _logger;

		private readonly WukongWidgetManager _widgetManager;

		private static readonly Stopwatch PingStopwatch = Stopwatch.StartNew();

		private static long _lastPingTimestamp;

		public WukongServerRpcCallbacks(IRelayClient relayClient, IClientEcsUpdateLoop ecsLoop, ILogger logger, WukongWidgetManager widgetManager)
		{
			RelayClient = relayClient;
			_ecsLoop = ecsLoop;
			_logger = logger;
			_widgetManager = widgetManager;
			InitRpc();
		}

		public void Dispose()
		{
			DeInitRpc();
		}

		[ServerRpcEvent("SkipMovie")]
		private void OnSkipMovie(SkipMovieData data)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongServerRpcCallbacks self, SkipMovieData skipMovieData)
			{
				self._logger.LogDebug("Received skip movie event from server, sequence id: {Id}, waiting: {Waiting}/{All}", skipMovieData.SequenceId, skipMovieData.WaitingPlayers, skipMovieData.AllPlayers);
				if (skipMovieData.WaitingPlayers == skipMovieData.AllPlayers)
				{
					self._widgetManager.HideInfoMessage();
					CutsceneUtils.SkipCutscene(skipMovieData.SequenceId);
				}
				else
				{
					self._widgetManager.ShowInfoMessage(string.Format(Texts.WaitForOtherPlayersCount, skipMovieData.WaitingPlayers, skipMovieData.AllPlayers));
				}
			}, this, data);
		}

		[ServerRpcEvent("MovieStarted")]
		private void OnMovieStarted(int sequenceId, AreaId areaId)
		{
		}

		[ServerRpcEvent("MovieFinished")]
		private void OnMovieFinished(int sequenceId, AreaId areaId)
		{
		}

		[ServerRpcEvent("BeguilingChant")]
		private void OnBeguilingChant(byte rawState)
		{
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced _, WukongServerRpcCallbacks self, BeguilingChantState state)
			{
				BGUIntervalArea[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<BGUIntervalArea>((UObject)(object)GameUtils.GetWorld());
				for (int i = 0; i < allActorsOfClass.Length; i++)
				{
					BUS_IntervalTriggerImpl component = ((BGUActorBaseCS)(object)allActorsOfClass[i]).GetComponent<BUS_IntervalTriggerImpl>();
					if (component != null)
					{
						bool flag = state == BeguilingChantState.Active;
						bool flag2 = state == BeguilingChantState.Warning;
						AccessTools.Method(typeof(BUS_IntervalTriggerImpl), "SetIsActive", (Type[])null, (Type[])null).Invoke(component, new object[1] { flag });
						if (flag2)
						{
							AccessTools.Method(typeof(BUS_IntervalTriggerImpl), "CheckIsWarning", (Type[])null, (Type[])null).Invoke(component, new object[1] { 0f });
						}
						else
						{
							AccessTools.Method(typeof(BUS_IntervalTriggerImpl), "ResetNotiedWarning", (Type[])null, (Type[])null).Invoke(component, Array.Empty<object>());
						}
					}
				}
			}, this, (BeguilingChantState)rawState);
		}

		[ServerRpcEvent("EnableCheats")]
		private void OnEnableCheats(AreaId areaId, bool enabled)
		{
		}

		[ServerRpcEvent("Ping")]
		private void OnPing(long timestamp)
		{
			if (timestamp != _lastPingTimestamp)
			{
				long num = PingStopwatch.ElapsedMilliseconds - timestamp;
				_logger.LogWarning("Received outdated ping response. Timestamp: {Timestamp}, now: {Now}, RTT: {Rtt}ms", timestamp, PingStopwatch.ElapsedMilliseconds, num);
				_widgetManager.SetPacketLossWarning();
			}
			else
			{
				long pingMs = PingStopwatch.ElapsedMilliseconds - timestamp;
				_widgetManager.UpdatePingIndicator(pingMs);
			}
		}

		public void SendPing()
		{
			_lastPingTimestamp = PingStopwatch.ElapsedMilliseconds;
			SendPing(_lastPingTimestamp);
		}

		public void SendBeguilingChant(byte payload0)
		{
			RelayMessage message = RelayMessage.ToServer(RelayMessageCode.MinServerRpcEvent, (DeliveryMethod)2);
			message.Writer.Put(payload0);
			RelayClient.SendMessage(message);
		}

		public void SendEnableCheats(AreaId payload0, bool payload1)
		{
			RelayMessage message = RelayMessage.ToServer((RelayMessageCode)151, (DeliveryMethod)2);
			NetDataWriter writer = message.Writer;
			payload0.Serialize(writer);
			writer.Put(payload1);
			RelayClient.SendMessage(message);
		}

		public void SendMovieFinished(int payload0, AreaId payload1)
		{
			RelayMessage message = RelayMessage.ToServer((RelayMessageCode)152, (DeliveryMethod)2);
			NetDataWriter writer = message.Writer;
			writer.Put(payload0);
			payload1.Serialize(writer);
			RelayClient.SendMessage(message);
		}

		public void SendMovieStarted(int payload0, AreaId payload1)
		{
			RelayMessage message = RelayMessage.ToServer((RelayMessageCode)153, (DeliveryMethod)2);
			NetDataWriter writer = message.Writer;
			writer.Put(payload0);
			payload1.Serialize(writer);
			RelayClient.SendMessage(message);
		}

		public void SendPing(long payload0)
		{
			RelayMessage message = RelayMessage.ToServer((RelayMessageCode)154, (DeliveryMethod)2);
			message.Writer.Put(payload0);
			RelayClient.SendMessage(message);
		}

		public void SendSkipMovie(SkipMovieData payload0)
		{
			RelayMessage message = RelayMessage.ToServer((RelayMessageCode)155, (DeliveryMethod)2);
			NetDataWriter writer = message.Writer;
			payload0.Serialize(writer);
			RelayClient.SendMessage(message);
		}

		protected void OnServerEvent(IRelayClientNetworkThreadContext context, ServerEventHeader header, NetDataReader reader)
		{
			switch (header.EventCode)
			{
			case RelayMessageCode.MinServerRpcEvent:
			{
				byte rawState = reader.GetByte();
				OnBeguilingChant(rawState);
				break;
			}
			case (RelayMessageCode)151:
			{
				AreaId areaId3 = default(AreaId);
				areaId3.Deserialize(reader);
				bool enabled = reader.GetBool();
				OnEnableCheats(areaId3, enabled);
				break;
			}
			case (RelayMessageCode)152:
			{
				int sequenceId2 = reader.GetInt();
				AreaId areaId2 = default(AreaId);
				areaId2.Deserialize(reader);
				OnMovieFinished(sequenceId2, areaId2);
				break;
			}
			case (RelayMessageCode)153:
			{
				int sequenceId = reader.GetInt();
				AreaId areaId = default(AreaId);
				areaId.Deserialize(reader);
				OnMovieStarted(sequenceId, areaId);
				break;
			}
			case (RelayMessageCode)154:
			{
				long timestamp = reader.GetLong();
				OnPing(timestamp);
				break;
			}
			case (RelayMessageCode)155:
			{
				SkipMovieData data = default(SkipMovieData);
				data.Deserialize(reader);
				OnSkipMovie(data);
				break;
			}
			default:
				throw new InvalidOperationException($"Unknown event code: {header.EventCode}");
			}
		}

		protected void InitRpc()
		{
			RelayClient.AddServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, OnServerEvent);
			RelayClient.AddServerRpcMessageHandler((RelayMessageCode)151, OnServerEvent);
			RelayClient.AddServerRpcMessageHandler((RelayMessageCode)152, OnServerEvent);
			RelayClient.AddServerRpcMessageHandler((RelayMessageCode)153, OnServerEvent);
			RelayClient.AddServerRpcMessageHandler((RelayMessageCode)154, OnServerEvent);
			RelayClient.AddServerRpcMessageHandler((RelayMessageCode)155, OnServerEvent);
		}

		protected void DeInitRpc()
		{
			RelayClient.RemoveServerRpcMessageHandler(RelayMessageCode.MinServerRpcEvent, OnServerEvent);
			RelayClient.RemoveServerRpcMessageHandler((RelayMessageCode)151, OnServerEvent);
			RelayClient.RemoveServerRpcMessageHandler((RelayMessageCode)152, OnServerEvent);
			RelayClient.RemoveServerRpcMessageHandler((RelayMessageCode)153, OnServerEvent);
			RelayClient.RemoveServerRpcMessageHandler((RelayMessageCode)154, OnServerEvent);
			RelayClient.RemoveServerRpcMessageHandler((RelayMessageCode)155, OnServerEvent);
		}
	}
	public class WukongSynchronizer : ClientNetworkedStateSynchronizer
	{
		protected readonly WukongAreaState AreaState;

		protected readonly WukongPlayerPawnState PlayerPawnState;

		private readonly SystemGroup _syncGroup;

		private readonly ClientWukongArchetypeRegistration _wukongArchetype;

		private readonly ClientState _state;

		protected readonly Store World;

		public WukongSynchronizer(ArchetypeEventRouter archetypeEvent, ClientState state, ClientWukongArchetypeRegistration wukongArchetype, Store world, WukongAreaState areaState, WukongPlayerState playerState, WukongPlayerPawnState playerPawnState, WukongPlayerModeManager modeManager, NetworkedEntityManager netManager, ClientOwnershipManager clientOwnership, JobRegistry jobRegistry, INetworkedComponentRegistry netComponentRegistry, IRelayClient relayClient, IClientEcsUpdateLoop ecsLoop, WukongEventBus eventBus, WukongWidgetManager widgetManager, GameplayEventRouter gameplayEventRouter, GameplayConfiguration configuration, FreeCameraManager freeCameraManager, FreeCameraController freeCameraController, ILogger logger)
			: base(netManager, state, jobRegistry, netComponentRegistry, relayClient, ecsLoop, clientOwnership, logger)
		{
			AreaState = areaState;
			PlayerPawnState = playerPawnState;
			_wukongArchetype = wukongArchetype;
			_state = state;
			World = world;
			_syncGroup = new SystemGroup("Sync");
			_syncGroup.Add(new SpawnTamersSystem(state, gameplayEventRouter, configuration));
			_syncGroup.Add(new SyncTamersSystem());
			_syncGroup.Add(new UnloadTamersSystem());
			_syncGroup.Add(new KillAlreadyDeadMonstersSystem(clientOwnership, playerState));
			_syncGroup.Add(new UpdateTamerMarkersSystem());
			_syncGroup.Add(new SyncMonsterTeamSystem());
			_syncGroup.Add(new ChangeTamerTargetSystem());
			_syncGroup.Add(new CreateLocalMainCharacterEntitySystem(state, playerState, eventBus, Logger));
			_syncGroup.Add(new SpawnOtherMainCharactersSystem(state, playerState, playerPawnState, eventBus, clientOwnership, Logger));
			_syncGroup.Add(new DespawnOtherMainCharactersSystem(archetypeEvent, playerState, wukongArchetype, playerPawnState, eventBus, Logger));
			_syncGroup.Add(new SyncMainCharactersSystem(playerState, modeManager, eventBus, widgetManager, configuration, gameplayEventRouter, logger));
			_syncGroup.Add(new EnableCollisionAfterCutsceneSystem(playerState));
			_syncGroup.Add(new UpdateMainCharacterMarkerSystem());
			_syncGroup.Add(new UpdateCooldownSystem(playerState, eventBus, areaState));
			_syncGroup.Add(new FreeCameraMovementSystem(eventBus, freeCameraManager, freeCameraController));
			_syncGroup.Add(new AfterMainCharacterDeathSystem(eventBus, playerState));
			_syncGroup.Add(new DebugViewSystem(eventBus, widgetManager));
			_syncGroup.SetMonitorPerf(enable: true);
			base.EcsLoop.AddSystem(_syncGroup);
		}

		protected override void OnDispose()
		{
			base.EcsLoop.RemoveSystem(_syncGroup);
			base.OnDispose();
		}

		protected override void OnOwnershipChanged(Entity entity)
		{
			MetadataComponent component = entity.GetComponent<MetadataComponent>();
			if (component.Archetype == _wukongArchetype.MonsterArchetype)
			{
				OnMonsterOwned(entity, component);
			}
		}

		private void OnMonsterOwned(Entity entity, MetadataComponent meta)
		{
			LocalTamerComponent component = entity.GetComponent<LocalTamerComponent>();
			if (!component.IsMonsterActive)
			{
				return;
			}
			if ((UObject)(object)component.Tamer == (UObject)null)
			{
				Logging.LogError("LocalTamerComponent.Tamer is null for entity {EntityId}", meta.NetId);
				return;
			}
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)component.Tamer);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("events are null");
				return;
			}
			PlayerId owner = meta.Owner;
			PlayerId? localPlayerId = _state.LocalPlayerId;
			if (!(owner == localPlayerId))
			{
				return;
			}
			TamerComponent component2 = entity.GetComponent<TamerComponent>();
			if (!component2.HasFsmPaused)
			{
				val.Evt_AIPauseBT.Invoke(false);
				val.Evt_AIPauseFsm.Invoke(false);
				val.Evt_AIPerceptionSetting.Invoke(true);
				Logging.LogDebug("Tamer actor enabled, guid: {Guid}.", BGU_DataUtil.GetActorGuid((AActor)(object)component.Tamer, false));
			}
			if (!(component2.Guid == "UGuid.HYS.JiRuHuo01"))
			{
				return;
			}
			val.Evt_DisablePhysicalMove.Invoke(false);
			BGUCharacterCS monster = component.Tamer.GetMonster();
			if (monster != null)
			{
				USkeletalMeshComponent mesh = ((ACharacter)monster).Mesh;
				if (mesh != null)
				{
					((UPrimitiveComponent)mesh).SetSimulatePhysics(true);
				}
			}
		}
	}
}
namespace WukongMp.Api.WukongUtils
{
	public static class AssetUtils
	{
		public static void ListAssetsInFolder(string path)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			UAssetDataArray assetsInFolder = UGSE_AssetUtilFuncLib.GetAssetsInFolder(new FName(path, (EFindName)1), true, false);
			if ((UObject)(object)assetsInFolder == (UObject)null)
			{
				return;
			}
			int num = 0;
			Enumerator<FAssetData> enumerator = ((TArrayBase<FAssetData>)(object)assetsInFolder.AssetDataArr).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FAssetData current = enumerator.Current;
					Logging.LogDebug("Asset {Id} path : {Name}", num++, UAssetRegistryHelpers_CsExtensions.GetFullName(current));
				}
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}

		public static void PlayBossDefeatedSound()
		{
			AccessTools.Method("B1UI.Script.GSUI.Util.GSUIAudioUtil:PlayUISound", (Type[])null, (Type[])null).Invoke(null, new object[1] { "EVT_ui_kill_jisha_manjingtou" });
		}
	}
	public static class BuffUtils
	{
		public static void AddBuff(BGUCharacterCS? character, int buffId, float duration)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for character {Character}", (character != null) ? ((UObject)character).GetName() : null);
			}
			else
			{
				val.Evt_BuffAdd.Invoke(buffId, (AActor)(object)character, (AActor)(object)character, duration, (EBuffSourceType)56, false, default(FBattleAttrSnapShot));
			}
		}

		public static void RemoveBuff(BGUCharacterCS? character, int buffId, EBuffEffectTriggerType removeTriggerType, int inLayer, bool withTriggerRemoveEffect)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for character {Character}", (character != null) ? ((UObject)character).GetName() : null);
			}
			else
			{
				val.Evt_BuffRemove.Invoke(buffId, removeTriggerType, inLayer, withTriggerRemoveEffect);
			}
		}

		public static void RemoveAllBuffs(BGUCharacterCS? character, EBuffEffectTriggerType removeTriggerType, bool withTriggerRemoveEffect)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for character {Character}", (character != null) ? ((UObject)character).GetName() : null);
			}
			else
			{
				val.Evt_BuffAllRemove.Invoke(removeTriggerType, withTriggerRemoveEffect);
			}
		}
	}
	public static class ClientUtils
	{
		private static Action<BGUCharacterCS, int>? _setter;

		public static void RegisterTeamHostility(int team1, int team2)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (team1 != team2)
			{
				BGC_TeamRelationData val = (BGC_TeamRelationData)BGU_DataUtil.GetGameStateReadonlyData<IBGC_TeamRelationData, BGC_TeamRelationData>((UObject)(object)GameUtils.GetWorld());
				EnsureTeamRelationExists(val, team1);
				EnsureTeamRelationExists(val, team2);
				TeamRelationInfo val2 = val.TeamHostileInfos[team1];
				TeamRelationInfo val3 = val.TeamHostileInfos[team2];
				if (!val2.HostileTeamIDs.Contains(team2))
				{
					val2.HostileTeamIDs.Add(team2);
				}
				if (!val3.HostileTeamIDs.Contains(team1))
				{
					val3.HostileTeamIDs.Add(team1);
				}
			}
		}

		public static void UnregisterTeamHostility(int team1, int team2)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			BGC_TeamRelationData val = (BGC_TeamRelationData)BGU_DataUtil.GetGameStateReadonlyData<IBGC_TeamRelationData, BGC_TeamRelationData>((UObject)(object)GameUtils.GetWorld());
			EnsureTeamRelationExists(val, team1);
			EnsureTeamRelationExists(val, team2);
			TeamRelationInfo val2 = val.TeamHostileInfos[team1];
			TeamRelationInfo obj = val.TeamHostileInfos[team2];
			val2.HostileTeamIDs.Remove(team2);
			obj.HostileTeamIDs.Remove(team1);
		}

		public static void RegisterAndSetPlayerTeam(BGUCharacterCS actor, int newTeamId)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			BGC_TeamRelationData val = (BGC_TeamRelationData)BGU_DataUtil.GetGameStateReadonlyData<IBGC_TeamRelationData, BGC_TeamRelationData>((UObject)(object)GameUtils.GetWorld());
			if (!val.TeamHostileInfos.ContainsKey(newTeamId))
			{
				int teamIDInCS = actor.GetTeamIDInCS();
				if (!val.TeamHostileInfos.TryGetValue(teamIDInCS, out var value))
				{
					value = new TeamRelationInfo
					{
						HostileTeamIDs = new List<int>(),
						TeamDamageReductionRatios = new Dictionary<int, int>()
					};
				}
				TeamRelationInfo value2 = new TeamRelationInfo
				{
					HostileTeamIDs = value.HostileTeamIDs.ToList(),
					TeamDamageReductionRatios = new Dictionary<int, int>(value.TeamDamageReductionRatios)
				};
				val.TeamHostileInfos.Add(newTeamId, value2);
			}
			if (_setter == null)
			{
				MethodInfo setMethod = typeof(BGUCharacterCS).GetProperty("TeamIDInCS", BindingFlags.Instance | BindingFlags.NonPublic).GetSetMethod(nonPublic: true);
				_setter = (Action<BGUCharacterCS, int>)Delegate.CreateDelegate(typeof(Action<BGUCharacterCS, int>), setMethod);
			}
			Logging.LogInformation("Setting team id {Team} for actor {Actor}", newTeamId, ((UObject)actor).GetName());
			_setter(actor, newTeamId);
			actor.SetTeamIDInCS(newTeamId);
		}

		private static void EnsureTeamRelationExists(BGC_TeamRelationData teamRelationData, int teamId)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if (!teamRelationData.TeamHostileInfos.ContainsKey(teamId))
			{
				teamRelationData.TeamHostileInfos.Add(teamId, new TeamRelationInfo());
			}
		}
	}
	public static class CutsceneUtils
	{
		public static void PlayCutscene(PlayMovieData data)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			BGW_EventCollection val = BGW_EventCollection.Get((UObject)(object)GameUtils.GetWorld());
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Failed to get BGW_EventCollection");
				return;
			}
			Del_PlayMovieRequest evt_RequestPlayMovie = val.Evt_RequestPlayMovie;
			FPlayMovieRequest val2 = default(FPlayMovieRequest);
			((FPlayMovieRequest)(ref val2)).SequenceID = data.SequenceId;
			((FPlayMovieRequest)(ref val2)).bDisablePlayerControl = data.DisablePlayerControl;
			((FPlayMovieRequest)(ref val2)).bDisableMovementInput = data.DisableMovementInput;
			((FPlayMovieRequest)(ref val2)).bDisableLookAtInput = data.DisableLookAtInput;
			((FPlayMovieRequest)(ref val2)).bHidePlayer = data.HidePlayer;
			((FPlayMovieRequest)(ref val2)).bHideHud = data.HideHud;
			((FPlayMovieRequest)(ref val2)).OverlapBoxGuid = data.OverlapBoxGuid;
			((FPlayMovieRequest)(ref val2)).MatchType = data.MatchType;
			evt_RequestPlayMovie.Invoke(val2);
		}

		public static void SetJoiningCutsceneStatus(SequenceWaitingData sequenceWaitingData)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Setting JoiningCutsceneStatus for sequenceId {SequenceId}", sequenceWaitingData.SequenceID);
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				Logging.LogError("Local player not found");
				return;
			}
			ref LocalMainCharacterComponent localState = ref localMainCharacter.Value.GetLocalState();
			if (!localState.IsWaitingForSequence)
			{
				localState.JoiningSequenceLocation = sequenceWaitingData.SequenceLocation;
				localState.IsJoiningSequence = true;
				DI.Instance.WidgetManager.ShowInfoMessage(Texts.JoinOtherPlayersToProceed);
			}
		}

		public static void RequestSkipCurrentCutscene()
		{
			BGUFunctionLibraryCS.SkipCurrentSequence((UObject)(object)GameUtils.GetWorld());
		}

		public static void SkipCutscene(int sequenceId)
		{
			MovieInstance cameraMovieInstance = BGU_DataUtil.GetGameStateReadonlyData<BGC_MovieData>((UObject)(object)GameUtils.GetWorld()).CameraMovieInstance;
			if ((UObject)(object)cameraMovieInstance != (UObject)null && cameraMovieInstance.CanSkipMovie() && cameraMovieInstance.SequenceId == sequenceId)
			{
				Logging.LogDebug("Skipping cutscene with sequenceId: {SequenceId}", sequenceId);
				cameraMovieInstance.SkipMovie();
			}
			else
			{
				Logging.LogWarning("Cannot skip cutscene, either not playing or sequenceId does not match. Current sequenceId: {CurrentSequenceId}, Requested: {RequestedSequenceId}", (cameraMovieInstance != null) ? new int?(cameraMovieInstance.SequenceId) : ((int?)null), sequenceId);
			}
		}

		public static bool CheckAllPlayersWaitingForCutscene(int sequenceId)
		{
			WukongPlayerState playerState = DI.Instance.PlayerState;
			return DI.Instance.State.AllPlayers.All(delegate(PlayerId p)
			{
				MainCharacterEntity? mainCharacterById = playerState.GetMainCharacterById(p);
				return mainCharacterById.HasValue && mainCharacterById.GetValueOrDefault().GetState().WaitingSequenceId == sequenceId;
			});
		}

		public static void TeleportLocalPlayerToCutsceneLocation()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				MainCharacterEntity value = localMainCharacter.Value;
				ref MainCharacterComponent state = ref value.GetState();
				value = localMainCharacter.Value;
				ref LocalMainCharacterComponent localState = ref value.GetLocalState();
				if (localState.IsJoiningSequence)
				{
					PlayerUtils.TeleportLocalPlayer(localMainCharacter.Value, localState.JoiningSequenceLocation, state.Rotation.ToFRotator(), setLookAt: false);
				}
			}
		}

		public static void ClearLocalJoiningCutsceneStatus(MainCharacterEntity mainCharacter)
		{
			ref LocalMainCharacterComponent localState = ref mainCharacter.GetLocalState();
			BGW_GameDataMgr obj = BGWGameInstanceCS.GetObject<BGW_GameDataMgr>((UObject)(object)localState.Pawn);
			BIC_MovieData obj2 = ((obj != null) ? obj.GetGameInstanceWritableData<BIC_MovieData>() : null);
			if (obj2 != null)
			{
				obj2.PlayMovieRequestQueue.Clear();
			}
			localState.IsJoiningSequence = false;
			localState.IsWaitingForSequence = false;
		}
	}
	public static class DebugUtils
	{
		private static readonly List<AActor> TmpActors = new List<AActor>();

		private static bool _superSpeed;

		private static float _originalFastSpeedRatio;

		private static float _originalNormalSpeedRatio;

		private static float _originalSlowSpeedRatio;

		public static bool ScaleMonsterHpToHalf { get; set; }

		public static bool InvincibilityEnabled { get; set; }

		public static void LogUe4SsPresence()
		{
			string text = FPaths.Combine(new string[3]
			{
				FPaths.ProjectDir,
				"Binaries",
				"Win64"
			});
			string text2 = FPaths.Combine(new string[2] { text, "dwmapi.dll" });
			string text3 = FPaths.Combine(new string[2] { text, "ue4ss" });
			if (File.Exists(text2))
			{
				DI.Instance.Logger.LogInformation("dwmapi.dll file found at {Path}", text2);
			}
			else
			{
				DI.Instance.Logger.LogInformation("dwmapi.dll file not found");
			}
			if (Directory.Exists(text3))
			{
				DI.Instance.Logger.LogInformation("ue4ss folder found at {Path}", text3);
				string path = FPaths.Combine(new string[2] { text3, "Mods" });
				if (!Directory.Exists(path))
				{
					return;
				}
				{
					foreach (string item in Directory.EnumerateDirectories(path))
					{
						string fileName = Path.GetFileName(item);
						DI.Instance.Logger.LogInformation("Found UE4SS mod folder: {ModFolder}", fileName);
					}
					return;
				}
			}
			DI.Instance.Logger.LogInformation("ue4ss folder not found");
		}

		public static UClass? GetDebugCubeActorClass()
		{
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world != (UObject)null)
			{
				UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("/Game/Mods/DebugMod/BP_DebugCube.BP_DebugCube_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				if ((UObject)(object)val == (UObject)null)
				{
					Logging.LogError("Cannot find class of {Class} to spawn", "/Game/Mods/DebugMod/BP_DebugCube.BP_DebugCube_C");
					return null;
				}
				return val;
			}
			return null;
		}

		public static UClass? GetDebugSphereActorClass()
		{
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world != (UObject)null)
			{
				UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("/Game/Mods/DebugMod/BP_DebugShpere.BP_DebugShpere_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				if ((UObject)(object)val == (UObject)null)
				{
					Logging.LogError("Cannot find class of {Class} to spawn", "/Game/Mods/DebugMod/BP_DebugShpere.BP_DebugShpere_C");
					return null;
				}
				return val;
			}
			return null;
		}

		public static AActor? SpawnActor(UClass unrealClass, FVector location, FRotator rotation)
		{
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world != (UObject)null)
			{
				AActor val = world.SpawnActor(unrealClass, ref location, ref rotation);
				if ((UObject)(object)val == (UObject)null)
				{
					Logging.LogError("Cannot spawn actor {ActorName}", ((UObject)unrealClass).GetName());
					return null;
				}
				return val;
			}
			return null;
		}

		public static List<AActor> GetActorsAroundPlayer(float radius, string actorsName)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			AActor[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<AActor>((UObject)(object)GameUtils.GetWorld());
			BGUPlayerCharacterCS? controlledPawn = GameUtils.GetControlledPawn();
			FVector val = ((controlledPawn != null) ? ((AActor)controlledPawn).GetActorLocation() : FVector.ZeroVector);
			List<AActor> list = new List<AActor>();
			bool flag = !string.IsNullOrEmpty(actorsName);
			AActor[] array = allActorsOfClass;
			foreach (AActor val2 in array)
			{
				if (!((UObject)(object)val2 == (UObject)null))
				{
					UClass obj = ((UObject)val2).GetClass();
					string text = ((obj != null) ? ((UObject)obj).GetName() : null);
					if (text != null && ((FVector.Distance(val2.GetActorLocation(), val) < radius && !flag) || (flag && text.Contains(actorsName))))
					{
						list.Add(val2);
					}
				}
			}
			return list;
		}

		public static void AddMarkerToActors(List<AActor> actors)
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			UWorld world = GameUtils.GetWorld();
			UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("/Game/Mods/WukongMod/BP_PlayerMarker.BP_PlayerMarker_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			int num = 0;
			FHitResult val3 = default(FHitResult);
			foreach (AActor actor in actors)
			{
				string actorGuid = BGU_DataUtil.GetActorGuid(actor, false);
				string name = ((UObject)actor).GetName();
				Logging.LogDebug("{Id}: Processing actor with class: {ActorClass}, name: {ActorName}, guid {ActorGuid}", num++, ((UObject)((UObject)actor).GetClass()).GetName(), name, actorGuid);
				AActor val2 = BGU_UnrealWorldUtil.SpawnActor(world, val);
				if ((UObject)(object)val2 == (UObject)null)
				{
					Logging.LogError("Cannot spawn player marker actor");
					break;
				}
				((UObject)val2).CallFunctionByNameWithArguments("SetText " + name + " ()", true);
				val2.SetActorLocation(actor.GetActorLocation(), false, ref val3, true);
				TmpActors.Add(val2);
			}
		}

		public static void DestroyTmpMarkerActors()
		{
			foreach (AActor tmpActor in TmpActors)
			{
				tmpActor.DestroyActor();
			}
			TmpActors.Clear();
		}

		public static void ShowMarkersForActors(float radius, string actorsName = "")
		{
			AddMarkerToActors(GetActorsAroundPlayer(radius, actorsName));
		}

		public static void ResetPlayersAnimation()
		{
			foreach (PlayerId allPlayer in DI.Instance.State.AllPlayers)
			{
				PlayerId value = allPlayer;
				PlayerId? localPlayerId = DI.Instance.PlayerState.LocalPlayerId;
				if (value != localPlayerId)
				{
					MainCharacterEntity? mainCharacterById = DI.Instance.PlayerState.GetMainCharacterById(allPlayer);
					if (!mainCharacterById.HasValue)
					{
						break;
					}
					BGUCharacterCS pawn = mainCharacterById.Value.GetLocalState().Pawn;
					if ((UObject)(object)pawn != (UObject)null)
					{
						ResetActorAnimation(pawn);
					}
				}
			}
		}

		public static void DumpPlayersAnimationDebugInfo()
		{
			foreach (PlayerId allPlayer in DI.Instance.State.AllPlayers)
			{
				MainCharacterEntity? mainCharacterById = DI.Instance.PlayerState.GetMainCharacterById(allPlayer);
				if (!mainCharacterById.HasValue)
				{
					break;
				}
				BGUCharacterCS pawn = mainCharacterById.Value.GetLocalState().Pawn;
				if ((UObject)(object)pawn != (UObject)null)
				{
					DumpActorAnimationDebugInfo((AActor)(object)pawn);
				}
			}
		}

		public static void DumpTamerAnimationDebugInfo(string name)
		{
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null)
			{
				return;
			}
			BUTamerActor[] allActorsOfClass = world.GetAllActorsOfClass<BUTamerActor>();
			foreach (BUTamerActor val in allActorsOfClass)
			{
				BGUCharacterCS monster = val.GetMonster();
				if ((UObject)(object)monster != (UObject)null && ((UObject)monster).GetName().Contains(name))
				{
					Logging.LogDebug("Found actor: {ActorName}", ((UObject)val).GetName());
					DumpActorAnimationDebugInfo((AActor)(object)monster);
				}
			}
		}

		public static void DumpActorAnimationDebugInfo(AActor pawn)
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			BUC_ABPHelperData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPHelperData>(pawn);
			BUC_ABPCommonSettingData unPersistentReadOnlyData2 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPCommonSettingData>(pawn);
			BUC_ABPMotionMatchingData unPersistentReadOnlyData3 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPMotionMatchingData>(pawn);
			BUC_ABPPlayerLocomotionData unPersistentReadOnlyData4 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPPlayerLocomotionData>(pawn);
			BUC_ABPCommonLocomotionData unPersistentReadOnlyData5 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPCommonLocomotionData>(pawn);
			BUC_ABPAdvancedMonsterLocomotionData unPersistentReadOnlyData6 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPAdvancedMonsterLocomotionData>(pawn);
			BUC_ABPBasicData unPersistentReadOnlyData7 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPBasicData>(pawn);
			BUC_ABPCharacterData unPersistentReadOnlyData8 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPCharacterData>(pawn);
			BUC_ABPBGUCharacterData unPersistentReadOnlyData9 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPBGUCharacterData>(pawn);
			BUC_ABPMonsterLocomotionData unPersistentReadOnlyData10 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPMonsterLocomotionData>(pawn);
			BUC_ABPWheelMoveData unPersistentReadOnlyData11 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPWheelMoveData>(pawn);
			BUC_ABPSplineMoveData unPersistentReadOnlyData12 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPSplineMoveData>(pawn);
			BUC_ABPSpeicalAdditiveData unPersistentReadOnlyData13 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPSpeicalAdditiveData>(pawn);
			BUC_ABPSpecialMoveData unPersistentReadOnlyData14 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPSpecialMoveData>(pawn);
			BUC_ABPNPCAnimData unPersistentReadOnlyData15 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPNPCAnimData>(pawn);
			Logging.LogDebug("Animation debug info for: {Name}", ((UObject)pawn).GetName());
			Logging.LogDebug("FinalABPMoveMode: {MoveMode}", unPersistentReadOnlyData2.FinalABPMoveMode);
			Logging.LogDebug("HasValidMoveAnimConfig: {IsValid}", unPersistentReadOnlyData.HasValidMoveAnimConfig((EMoveSpeedLevel)1, true));
			LogAllProperties(unPersistentReadOnlyData);
			LogAllProperties(unPersistentReadOnlyData2);
			LogAllProperties(unPersistentReadOnlyData3);
			LogAllProperties(unPersistentReadOnlyData3.CurrentAA);
			LogAllProperties(unPersistentReadOnlyData4);
			LogAllProperties(unPersistentReadOnlyData5);
			LogAllProperties(unPersistentReadOnlyData6);
			LogAllProperties(unPersistentReadOnlyData7);
			LogAllProperties(unPersistentReadOnlyData8);
			LogAllProperties(unPersistentReadOnlyData8.MovementComp);
			LogAllProperties(unPersistentReadOnlyData9);
			LogAllProperties(unPersistentReadOnlyData10);
			LogAllProperties(unPersistentReadOnlyData11);
			LogAllProperties(unPersistentReadOnlyData12);
			LogAllProperties(unPersistentReadOnlyData13);
			LogAllProperties(unPersistentReadOnlyData14);
			LogAllProperties(unPersistentReadOnlyData15);
			UAnimInstance animInst = unPersistentReadOnlyData.AnimInst;
			if (!((UObject)(object)animInst == (UObject)null))
			{
				BUAnimHumanoidCS val = (BUAnimHumanoidCS)(object)((animInst is BUAnimHumanoidCS) ? animInst : null);
				if (val != null)
				{
					UAnimInstance linkedAnimGraphInstanceByTag = ((UAnimInstance)val).GetLinkedAnimGraphInstanceByTag(B1GlobalFNames.Move);
					if (!((UObject?)(object)linkedAnimGraphInstanceByTag).IsNullOrDestroyed())
					{
						LogAllProperties(linkedAnimGraphInstanceByTag);
						UAnimInstance linkedAnimGraphInstanceByTag2 = linkedAnimGraphInstanceByTag.GetLinkedAnimGraphInstanceByTag(B1GlobalFNames.PlayerLocomotion);
						if (!((UObject?)(object)linkedAnimGraphInstanceByTag2).IsNullOrDestroyed())
						{
							LogAllProperties(linkedAnimGraphInstanceByTag2);
						}
						UAnimInstance linkedAnimGraphInstanceByTag3 = linkedAnimGraphInstanceByTag.GetLinkedAnimGraphInstanceByTag(B1GlobalFNames.AdvancedMonsterLocomotion);
						if (!((UObject?)(object)linkedAnimGraphInstanceByTag3).IsNullOrDestroyed())
						{
							LogAllProperties(linkedAnimGraphInstanceByTag3);
						}
						UAnimInstance linkedAnimGraphInstanceByTag4 = linkedAnimGraphInstanceByTag.GetLinkedAnimGraphInstanceByTag(B1GlobalFNames.MonsterLocomotion);
						if (!((UObject?)(object)linkedAnimGraphInstanceByTag4).IsNullOrDestroyed())
						{
							LogAllProperties(linkedAnimGraphInstanceByTag4);
						}
						UAnimInstance linkedAnimGraphInstanceByTag5 = linkedAnimGraphInstanceByTag.GetLinkedAnimGraphInstanceByTag(B1GlobalFNames.MotionMatching);
						if (!((UObject?)(object)linkedAnimGraphInstanceByTag5).IsNullOrDestroyed())
						{
							LogAllProperties(linkedAnimGraphInstanceByTag5);
						}
					}
				}
			}
			LogCurveValues(unPersistentReadOnlyData);
			LogStateMachineWeights(unPersistentReadOnlyData);
		}

		private static void LogAllProperties(object component)
		{
			foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(component))
			{
				if (property != null && !property.PropertyType.IsAssignableFrom(typeof(UBlendSpace)) && !property.PropertyType.IsAssignableFrom(typeof(UAnimSequence)))
				{
					object value = property.GetValue(component);
					if (value != null)
					{
						Logging.LogDebug("{ObjectName} property name: {Name}, value: {Value}", component.GetType().Name, property.Name, value.ToString());
					}
				}
			}
		}

		private static void LogStateMachineWeights(BUC_ABPHelperData animationHelperData)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			foreach (KeyValuePair<FName, Dictionary<FName, float>> stateMachineWeight in animationHelperData.StateMachineWeights)
			{
				foreach (KeyValuePair<FName, float> item in stateMachineWeight.Value)
				{
					Logging.LogDebug("StateMachineName: {StateMachineName}, stateName: {StateName}, value: {Value}", ((object)stateMachineWeight.Key/*cast due to .constrained prefix*/).ToString(), item.ToString(), item.Value);
				}
			}
		}

		private static void LogCurveValues(BUC_ABPHelperData animationHelperData)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			foreach (KeyValuePair<FName, float> floatCurveValue in animationHelperData.FloatCurveValues)
			{
				Logging.LogDebug("Curve name: {Name}, value {Value}", ((object)floatCurveValue.Key/*cast due to .constrained prefix*/).ToString(), floatCurveValue.Value);
			}
		}

		public static void ResetActorAnimation(BGUCharacterCS player)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)player);
			if (obj != null)
			{
				obj.Evt_ResetABPSetting.Invoke();
			}
		}

		public static void ResetActorStatus(BGUCharacterCS player)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)player);
			if (obj != null)
			{
				obj.Evt_ResetActorStatusPre.Invoke((EResetActorReason)1);
			}
		}

		public static void ToggleSuperFastSpeed()
		{
			IBUC_SpeedCtrlData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<IBUC_SpeedCtrlData, BUC_SpeedCtrlData>((AActor)(object)GameUtils.GetControlledPawn());
			BUC_SpeedCtrlData val = (BUC_SpeedCtrlData)(object)((unPersistentReadOnlyData is BUC_SpeedCtrlData) ? unPersistentReadOnlyData : null);
			if (val != null)
			{
				if (!_superSpeed)
				{
					_originalSlowSpeedRatio = val.GetMoveSpeedSlow();
					_originalNormalSpeedRatio = val.GetMoveSpeedNormal();
					_originalFastSpeedRatio = val.GetMoveSpeedFast();
					val.SetSpeedInfo(10000f, 10000f, 10000f);
					_superSpeed = true;
				}
				else
				{
					val.SetSpeedInfo(_originalSlowSpeedRatio, _originalNormalSpeedRatio, _originalFastSpeedRatio);
					_superSpeed = false;
				}
			}
		}

		public static void ToggleBoxTemp(UClass BP, UObject world)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((UObject)(object)BP == (UObject)null)
				{
					return;
				}
				AActor[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass(world, BP);
				for (int i = 0; i < allActorsOfClass.Length; i++)
				{
					TArrayUnsafe<UActorComponent> componentsByClass = allActorsOfClass[i].GetComponentsByClass(TSubclassOf<UActorComponent>.op_Implicit(UClass.GetClass<UStaticMeshComponent>()));
					for (int j = 0; j < componentsByClass.Count; j++)
					{
						UActorComponent obj = componentsByClass[j];
						UStaticMeshComponent val = (UStaticMeshComponent)(object)((obj is UStaticMeshComponent) ? obj : null);
						if ((UObject)(object)val != (UObject)null)
						{
							bool flag = !((USceneComponent)val).HiddenInGame;
							((USceneComponent)val).SetHiddenInGame(flag, false);
						}
					}
				}
			}
			catch (Exception ex)
			{
				USharpExceptionHandler.HandleException(ex, (EUSharpExceptionType)1);
			}
		}
	}
	public static class EquipmentUtils
	{
		private static readonly MethodInfo OnChangeEquipReal = typeof(BUS_EquipComp).GetMethod("OnChangeEquipReal", BindingFlags.Instance | BindingFlags.NonPublic);

		public static EquipmentState GetCurrentEquipmentStateForActor(APawn player)
		{
			return new EquipmentState(((IEnumerable<KeyValuePair<EquipPosition, int>>)BGU_DataUtil.GetReadOnlyData<IBPC_RoleBaseData, BPC_RoleBaseData>((AActor)(object)player.PlayerState).EquipList).Select((KeyValuePair<EquipPosition, int> kvp) => (kvp.Key.FromGame(), Value: kvp.Value)));
		}

		public static void SetActorEquipment(BGUCharacterCS actor, EquipmentState equipment)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			BUS_EquipComp equipComp = GetEquipComp(actor);
			foreach (var (value, num) in equipment.GetItems())
			{
				OnChangeEquipReal.Invoke(equipComp, new object[2]
				{
					value.ToGame(),
					num
				});
			}
		}

		private static BUS_EquipComp? GetEquipComp(BGUCharacterCS actor)
		{
			UActorCompBaseCS? obj = ((IEnumerable<UActorCompBaseCS>)Traverse.Create((object)actor.ActorCompContainerCS).Field<List<UActorCompBaseCS>>("CompCSs").Value).FirstOrDefault((Func<UActorCompBaseCS, bool>)((UActorCompBaseCS x) => x is BUS_EquipComp));
			return (BUS_EquipComp?)(object)((obj is BUS_EquipComp) ? obj : null);
		}
	}
	public static class GameSaveUtils
	{
		public static string GetModsDirectory()
		{
			if (LaunchParameters.Instance.ModFolderOverride != null)
			{
				return LaunchParameters.Instance.ModFolderOverride;
			}
			return FPaths.Combine(new string[5]
			{
				FPaths.ProjectDir,
				"Binaries",
				"Win64",
				"CSharpLoader",
				"Mods"
			});
		}

		private static string GetModDirectory(Assembly modAssembly)
		{
			string name = modAssembly.GetName().Name;
			if (LaunchParameters.Instance.ModFolderOverride != null)
			{
				return FPaths.Combine(new string[2]
				{
					LaunchParameters.Instance.ModFolderOverride,
					name
				});
			}
			return FPaths.Combine(new string[6]
			{
				FPaths.ProjectDir,
				"Binaries",
				"Win64",
				"CSharpLoader",
				"Mods",
				name
			});
		}

		public static string GetSaveFileFullName(Assembly modAssembly, string slotName)
		{
			slotName += ".sav";
			return FPaths.Combine(new string[2]
			{
				GetModDirectory(modAssembly),
				slotName
			});
		}
	}
	public static class GameUtils
	{
		private static UWorld? _world;

		public static UWorld? GetWorld()
		{
			if ((UObject)(object)_world == (UObject)null)
			{
				UObject val = GCHelper.FindRef(FGlobals.GWorld)?.Managed;
				_world = (UWorld?)(object)((((val is UWorld) ? val : null) is UWorld) ? ((val is UWorld) ? val : null) : null);
			}
			return _world;
		}

		public static BGUPlayerCharacterCS? GetControlledPawn()
		{
			APlayerController firstLocalPlayerController = UGSE_EngineFuncLib.GetFirstLocalPlayerController((UObject)(object)GetWorld());
			APawn obj = ((firstLocalPlayerController != null) ? ((AController)firstLocalPlayerController).GetControlledPawn() : null);
			BGUPlayerCharacterCS val = (BGUPlayerCharacterCS)(object)((obj is BGUPlayerCharacterCS) ? obj : null);
			if (!((UObject?)(object)val).IsNullOrDestroyed())
			{
				return val;
			}
			return null;
		}

		public static BGP_PlayerControllerB1 GetPlayerController()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			return (BGP_PlayerControllerB1)UGSE_EngineFuncLib.GetFirstLocalPlayerController((UObject)(object)GetWorld());
		}

		public static bool IsGameInstanceValid()
		{
			return (UObject)(object)BGWGameInstanceCS.Get((UObject)null) != (UObject)null;
		}

		public static bool IsWorldValid()
		{
			return (UObject)(object)GetWorld() != (UObject)null;
		}

		public static void PossesPawnWithViewTarget(ABGPPlayerController controller, APawn possessPawn, APawn unpossessPawn, FRotator controllerRotation)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			PossessPawn(controller, possessPawn, unpossessPawn);
			((APlayerController)controller).SetViewTargetWithBlend((AActor)(object)possessPawn, 0f, (EViewTargetBlendFunction)0, 0f, false);
			((AController)controller).SetControlRotation(controllerRotation);
			DI.Instance.FreeCameraManager.ReEnableFreeCamera();
		}

		public static void PossessPawn(ABGPPlayerController controller, APawn possessPawn, APawn unpossessPawn)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			((AController)controller).Possess(possessPawn);
			BPS_GSEventCollection.Get((APlayerController)(object)controller).Evt_BPS_OnControlledPawnChange.Invoke(possessPawn);
			BGS_GSEventCollection obj = BGS_EventCollectionCS.Get((UObject)(object)controller);
			if (obj != null)
			{
				obj.Evt_NotifyPossessEntityChanged.Invoke(ECSExtension.ToEntity((AActor)(object)unpossessPawn), ECSExtension.ToEntity((AActor)(object)possessPawn));
			}
		}

		public static void EnableThreading()
		{
			Logging.LogDebug("Enabling threading for ECSWorld");
			BGW_ECSWorld.ThreadCount = 4;
		}

		public static void DisableThreading()
		{
			Logging.LogDebug("Disabling threading for ECSWorld");
			BGW_ECSWorld.ThreadCount = 0;
		}
	}
	internal static class ImmobilizeUtils
	{
		internal static void CastImmobilize(BGUCharacterCS caster)
		{
			Logging.LogDebug("Received cast immobilize for character {Nickname}", ((UObject)caster).GetName());
			BUS_EventCollectionCS.Get((AActor)(object)caster).Evt_CastImmobilize.Invoke(0);
		}

		internal static void TriggerImmobilize(BGUCharacterCS? pawn, BGUCharacterCS? caster, bool hasBuff)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Received trigger immobilize for character {Pawn}", (pawn != null) ? ((UObject)pawn).GetName() : null);
			if ((UObject)(object)pawn == (UObject)null)
			{
				Logging.LogError("Failed to cast immobilizedCharacter to BGUCharacterCS");
				return;
			}
			if ((UObject)(object)caster == (UObject)null)
			{
				Logging.LogError("Failed to cast castingCharacter to BGUCharacterCS");
				return;
			}
			BUC_CastImmobilizeData val = (BUC_CastImmobilizeData)caster.GetDataByChunk(TypeManager.GetTypeIndex<BUC_CastImmobilizeData>());
			FUStImmobilizeSkillConfigDesc val2 = default(FUStImmobilizeSkillConfigDesc);
			((BUC_PassiveSkillData)caster.GetDataByChunk(TypeManager.GetTypeIndex<BUC_PassiveSkillData>())).TryGetCachedDesc<FUStImmobilizeSkillConfigDesc>(val.ResId, ref val2);
			if (val2 == null)
			{
				Logging.LogError("cachedImmobilizeConfigDesc is null");
				return;
			}
			ImmobilizeConfigInstance val3 = CreateImmobilizeConfig((AActor)(object)pawn, (AActor)(object)caster, val2, val.ResId, hasBuff, val);
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)pawn);
			if (obj != null)
			{
				obj.Evt_TriggerImmobilize.Invoke(val3);
			}
		}

		internal static void RelieveImmobilize(BGUCharacterCS pawn)
		{
			Logging.LogDebug("Received relieve immobilize for player {Nickname}", ((UObject)pawn).GetName());
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)pawn);
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)pawn);
			if (entityByTamerMonster.HasValue)
			{
				entityByTamerMonster.Value.GetLocalTamer().RunImmobilizePatches = true;
			}
			else
			{
				MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn((AActor?)(object)pawn);
				if (entityByPlayerPawn.HasValue)
				{
					entityByPlayerPawn.Value.GetLocalState().RunImmobilizePatches = true;
				}
			}
			if (obj != null)
			{
				obj.Evt_RelieveImmobilized.Invoke();
			}
		}

		public static ImmobilizeConfigInstance CreateImmobilizeConfig(AActor character, AActor casterActor, FUStImmobilizeSkillConfigDesc cachedImmobilizeConfigDesc, int castImmobilizeDataResId, bool hasBuff, BUC_CastImmobilizeData castImmobilizeData)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			ImmobilizeConfigInstance val = new ImmobilizeConfigInstance();
			int actorResID = BGU_DataUtil.GetActorResID(character);
			val.DurationSecond = (float)cachedImmobilizeConfigDesc.DurationMs * 0.001f;
			val.AlmostEndAheadTimeSecond = (float)cachedImmobilizeConfigDesc.AlmostEndAheadTimeMs * 0.001f;
			val.MinDurationSecond = (float)cachedImmobilizeConfigDesc.MinimalDurationMs * 0.001f;
			val.RepeatedImmobilizedDef = (float)cachedImmobilizeConfigDesc.RepeatedImmobilizedDef * 0.0001f;
			val.CasterActor = casterActor;
			val.bEnableGreatSageTalent = cachedImmobilizeConfigDesc.GreatSageTalentActiveBuff > 0 && hasBuff;
			val.BeginFX = GetFxAssetByResId((UObject)(object)character, (IList<FPlayFXByResID>)cachedImmobilizeConfigDesc.BeginFXs, actorResID, castImmobilizeDataResId, castImmobilizeData);
			val.AlmostEndFX = GetFxAssetByResId((UObject)(object)character, (IList<FPlayFXByResID>)cachedImmobilizeConfigDesc.AlmostEndFXs, actorResID, castImmobilizeDataResId, castImmobilizeData);
			val.EndFX = GetFxAssetByResId((UObject)(object)character, (IList<FPlayFXByResID>)cachedImmobilizeConfigDesc.EndFXs, actorResID, castImmobilizeDataResId, castImmobilizeData);
			val.QuickFX = GetFxAssetByResId((UObject)(object)character, (IList<FPlayFXByResID>)cachedImmobilizeConfigDesc.QuickEndFXs, actorResID, castImmobilizeDataResId, castImmobilizeData);
			val.BreakingFXsTriggerRatio = (float)cachedImmobilizeConfigDesc.BreakingFXsTriggerRatio * 0.0001f;
			val.BreakingFX = GetFxAssetByResId((UObject)(object)character, (IList<FPlayFXByResID>)cachedImmobilizeConfigDesc.BreakingFXs, actorResID, castImmobilizeDataResId, castImmobilizeData);
			foreach (FSpellEffect beginEffect in cachedImmobilizeConfigDesc.BeginEffects)
			{
				val.BeginEffects.Add(new FSpellEffectForData(beginEffect));
			}
			foreach (FSpellEffect endEffect in cachedImmobilizeConfigDesc.EndEffects)
			{
				val.EndEffects.Add(new FSpellEffectForData(endEffect));
			}
			foreach (FSpellEffect breakEffect in cachedImmobilizeConfigDesc.BreakEffects)
			{
				val.BreakEffects.Add(new FSpellEffectForData(breakEffect));
			}
			foreach (FSpellEffect deadEffect in cachedImmobilizeConfigDesc.DeadEffects)
			{
				val.DeadEffects.Add(new FSpellEffectForData(deadEffect));
			}
			return val;
		}

		public static UBGWDataAsset? GetFxAssetByResId(UObject context, IList<FPlayFXByResID> fXs, int targetResId, int ownerResId, BUC_CastImmobilizeData CastImmobilizeData)
		{
			string text = "";
			foreach (FPlayFXByResID fX in fXs)
			{
				if (fX.ResID == targetResId)
				{
					text = fX.FXPathByDBC;
					break;
				}
				if (fX.ResID == ownerResId)
				{
					text = fX.FXPathByDBC;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			UBGWDataAsset val = CastImmobilizeData.TryGetDBCFromCache(text);
			if ((UObject)(object)val == (UObject)null)
			{
				val = BGW_PreloadAssetMgr.Get(context).TryGetCachedResourceObj<UBGWDataAsset>(text, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				CastImmobilizeData.TryAddDBCCache(text, val);
			}
			return val;
		}
	}
	public static class IronBodyUtils
	{
		public static void TriggerIronBody(BGUCharacterCS pawn)
		{
			Logging.LogDebug("Received trigger iron body for character {Nickname}", ((UObject)pawn).GetName());
			BUS_EventCollectionCS.Get((AActor)(object)pawn);
			BGUFunctionLibraryCS.BGUTryCastSpell((AActor)(object)pawn, 10505, (ECastSkillSourceType)0, false);
		}
	}
	public static class MagicallyChangeUtils
	{
		public static void TriggerMagicallyChange(BGUCharacterCS pawn, string configAssetPath, int skillID, int recoverSkillID, int curVigorSkillID, ECastReason_MagicallyChange castReason = (ECastReason_MagicallyChange)2)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			UBGWDataAsset val = BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UBGWDataAsset>(configAssetPath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Failed to load MagicallyChangeConfig from path: {Path}", configAssetPath);
				return;
			}
			Logging.LogDebug("Received trigger magically change for character {Nickname} with config {ConfigAssetPath}, skillID {SkillID}, recoverSkillID {RecoverSkillID}, curVigorSkillID {CurVigorSkillID}, castReason {CastReason}", ((UObject)pawn).GetName(), configAssetPath, skillID, recoverSkillID, curVigorSkillID, castReason);
			BUC_MagicallyChangeData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_MagicallyChangeData, BUC_MagicallyChangeData>((AActor)(object)pawn);
			unPersistentReadOnlyData.CastReason = castReason;
			unPersistentReadOnlyData.CurVigorSkillID = curVigorSkillID;
			unPersistentReadOnlyData.bIsPendingCast = true;
			unPersistentReadOnlyData.bIsPendingReset = false;
			unPersistentReadOnlyData.PendingConfig = val;
			unPersistentReadOnlyData.MagicallyChangeSkillID = skillID;
			unPersistentReadOnlyData.RecoverSkillID = recoverSkillID;
		}

		public static void ResetMagicallyChange(BGUCharacterCS pawn, EResetReason_MagicallyChange reason)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Received reset magically change for character {Nickname} with reason {Reason}", ((UObject)pawn).GetName(), reason);
			BUC_MagicallyChangeData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_MagicallyChangeData, BUC_MagicallyChangeData>((AActor)(object)pawn);
			unPersistentReadOnlyData.bIsPendingReset = true;
			unPersistentReadOnlyData.bIsPendingCast = false;
			unPersistentReadOnlyData.ResetReason = reason;
		}
	}
	public static class MagicFieldUtils
	{
		public static void DestroyMagicField(string magicFieldClassName, EBGUBulletDestroyReason reason)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("DestroyMagicField called for magic field {MagicFieldClassName} with reason {Reason}", magicFieldClassName, reason);
			BGUMagicFieldBaseCS[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<BGUMagicFieldBaseCS>((UObject)(object)GameUtils.GetWorld());
			foreach (BGUMagicFieldBaseCS val in allActorsOfClass)
			{
				if (((UObject?)(object)val).IsNullOrDestroyed())
				{
					continue;
				}
				UClass val2 = ((UObject)val).GetClass();
				if ((UObject)(object)val2 != (UObject)null && ((UObject)val2).GetName() == magicFieldClassName)
				{
					BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)val);
					if (obj != null)
					{
						obj.Evt_OnProjectileDead.Invoke(reason);
					}
				}
			}
		}
	}
	public static class MarkerUtils
	{
		public static void CreateMarkerForCharacter(TamerEntity tamerEntity, string color)
		{
			AActor val = SpawnMarkerActor();
			if (!((UObject)(object)val == (UObject)null))
			{
				ref NicknameComponent nickname = ref tamerEntity.GetNickname();
				ref MarkerComponent marker = ref tamerEntity.GetMarker();
				((UObject)val).CallFunctionByNameWithArguments("SetText " + nickname.Nickname + " " + color, true);
				marker.MarkerActor = val;
				marker.DestroyQueued = false;
			}
		}

		public static void DestroyMarkerForCharacter(TamerEntity tamerEntity)
		{
			ref MarkerComponent marker = ref tamerEntity.GetMarker();
			if (!marker.DestroyQueued)
			{
				Logging.LogDebug("Destroying marker for monster {NetId}, guid {Guid}", tamerEntity.GetMeta().NetId, tamerEntity.GetTamer().Guid);
				marker.DestroyQueued = true;
				AActor markerActor = marker.MarkerActor;
				if (!((UObject?)(object)markerActor).IsNullOrDestroyed())
				{
					BGU_UnrealWorldUtil.DestroyActor(markerActor);
				}
				marker.MarkerActor = null;
			}
		}

		public static AActor? CreateMarkerForCharacter(MainCharacterEntity mainEntity, string color)
		{
			AActor val = SpawnMarkerActor();
			if ((UObject)(object)val == (UObject)null)
			{
				return null;
			}
			ref MainCharacterComponent state = ref mainEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			((UObject)val).CallFunctionByNameWithArguments("SetText " + state.CharacterNickName + " " + color, true);
			localState.MarkerActor = val;
			return val;
		}

		public static AActor? SpawnMarkerActor()
		{
			UWorld world = GameUtils.GetWorld();
			UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("/Game/Mods/WukongMod/BP_PlayerMarker.BP_PlayerMarker_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Cannot load marker class");
				return null;
			}
			AActor val2 = BGU_UnrealWorldUtil.SpawnActor(world, val);
			if ((UObject)(object)val2 == (UObject)null)
			{
				Logging.LogError("Cannot spawn marker actor");
				return null;
			}
			Logging.LogDebug("Marker actor spawned successfully");
			return val2;
		}
	}
	internal static class MathUtils
	{
		public static float LerpAngle(float current, float target, float alpha)
		{
			float num = (target - current + 540f) % 360f - 180f;
			return current + num * alpha;
		}
	}
	public static class ModWidgetsUtils
	{
		public static UUserWidget? SpawnWidget(string widgetPath)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null)
			{
				return null;
			}
			UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>(widgetPath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Cannot find class of {Class} to spawn", widgetPath);
				return null;
			}
			UUserWidget val2 = UGSE_UMGFuncLib.CreateUserWidgetWithClass((UObject)(object)world, TSubclassOf<UObject>.op_Implicit(val));
			if ((UObject)(object)val2 != (UObject)null)
			{
				Logging.LogDebug("Widget {Class} spawned successfully", widgetPath);
			}
			else
			{
				Logging.LogError("Cannot spawn widget {Class}", widgetPath);
			}
			return val2;
		}

		private static List<UUserWidget> GetWidgetsByPath(string widgetPath)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null)
			{
				return new List<UUserWidget>();
			}
			List<UUserWidget> result = new List<UUserWidget>();
			UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>(widgetPath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Cannot find class of {Class}", widgetPath);
				return new List<UUserWidget>();
			}
			List<UUserWidget> list = default(List<UUserWidget>);
			UWidgetLibrary.GetAllWidgetsOfClass((UObject)(object)world, ref list, TSubclassOf<UUserWidget>.op_Implicit(val), true);
			return result;
		}

		public static UUserWidget? GetWidget(string widgetPath)
		{
			return GetWidgetsByPath(widgetPath).SingleOrDefault();
		}
	}
	public static class NpcLocomotionUtils
	{
		public static void SetStateTrigger(BGUCharacterCS? character, EBUStateTrigger trigger, float time, bool needForceUpdate)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for pawn {PathName}", (character != null) ? ((UObject)character).PathName : null);
			}
			else
			{
				val.Evt_UnitStateTrigger.Invoke(trigger, time, needForceUpdate);
			}
		}

		public static void SetSimpleState(BGUCharacterCS? character, EBGUSimpleState state, bool isForce)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for pawn {PathName}", (character != null) ? ((UObject)character).PathName : null);
			}
			else
			{
				val.Evt_UnitSetSimpleState.Invoke(state, isForce);
			}
		}

		public static void SetFsmState(BGUCharacterCS? character, string stateName)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for character {Pawn}", (character != null) ? ((UObject)character).PathName : null);
			}
			else
			{
				val.Evt_TriggerFsmEvent.Invoke(GameplayTagExtension.MakeGameplayTag(stateName));
			}
		}

		public static void SetMotionMatchingState(BGUCharacterCS? character, EState_MM motionMatchingState)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)character);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Failed to get event collection for pawn {PathName}", (character != null) ? ((UObject)character).PathName : null);
			}
			else
			{
				val.Evt_ChangeMotionMatchingState.Invoke(motionMatchingState);
			}
		}
	}
	public static class PlayerUtils
	{
		public static void TeleportLocalPlayer(MainCharacterEntity mainEntity, FVector location, FRotator rotation, bool setLookAt = true)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if ((UObject)(object)localState.Pawn == (UObject)null)
			{
				Logging.LogError("Failed to teleport local player: Pawn is null");
				return;
			}
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
			if (obj != null)
			{
				obj.Evt_UnitStateTrigger.Invoke((EBUStateTrigger)77, -1f, false);
			}
			localState.TeleportFinishFrames = 5;
			FVector correctedSpawnLocation = SpawningUtils.GetCorrectedSpawnLocation((ACharacter)(object)localState.Pawn, location);
			FHitResult val = default(FHitResult);
			((AActor)localState.Pawn).SetActorTransform(new FTransform(rotation, correctedSpawnLocation), false, ref val, true);
			if (setLookAt)
			{
				BUS_GSEventCollection obj2 = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
				if (obj2 != null)
				{
					obj2.Evt_ResetCameraSpringArmRot.Invoke();
				}
			}
		}

		public static void SetPlayerInteractionEnabled(MainCharacterEntity mainEntity, bool enabled)
		{
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			bool flag = BGU_DataUtil.GetReadOnlyData<IBUC_SimpleStateData, BUC_SimpleStateData>((AActor)(object)localState.Pawn).HasSimpleState((EBGUSimpleState)162);
			if (!(!enabled && flag))
			{
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
				if (obj != null)
				{
					obj.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)162, enabled);
				}
			}
		}

		public static void SetLocalPlayerDamageImmunity(MainCharacterEntity mainEntity, bool enabled)
		{
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)mainEntity.GetLocalState().Pawn);
			if ((UObject)(object)val != (UObject)null)
			{
				if (val != null)
				{
					val.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)1, !enabled);
				}
				Logging.LogDebug("Set local player damage immunity to {Enabled}", enabled);
			}
		}

		public static void ResetLocalPlayerCooldown()
		{
			BGUPlayerCharacterCS controlledPawn = GameUtils.GetControlledPawn();
			if ((UObject)(object)controlledPawn == (UObject)null)
			{
				Logging.LogError("Failed to get local player");
				return;
			}
			ResetCooldown((APawn)(object)controlledPawn);
			ResetMana((APawn)(object)controlledPawn);
		}

		public static void ResetCooldown(APawn playerPawn)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			if (obj != null)
			{
				obj.Evt_ResetSkillCD.Invoke();
			}
			if (obj != null)
			{
				obj.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)188, BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)playerPawn, (EBGUAttrFloat)11));
			}
			if (obj != null)
			{
				obj.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)202, BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)playerPawn, (EBGUAttrFloat)17));
			}
			if (obj != null)
			{
				obj.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)201, BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)playerPawn, (EBGUAttrFloat)16));
			}
		}

		public static void ResetMana(APawn playerPawn)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			float floatValue = BGU_DataUtil.GetReadOnlyData<IBUC_AttrContainer, BUC_AttrContainer>((AActor)(object)playerPawn).GetFloatValue((EBGUAttrFloat)2);
			if (obj != null)
			{
				obj.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)152, floatValue);
			}
		}

		public static void TeleportLocalPlayerToCurrentRebirthPoint(MainCharacterEntity mainEntity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			FTransform currentRebirthPointTransform = GetCurrentRebirthPointTransform();
			FVector location = ((FTransform)(ref currentRebirthPointTransform)).GetLocation();
			FQuat rotation = ((FTransform)(ref currentRebirthPointTransform)).GetRotation();
			TeleportLocalPlayer(mainEntity, location, ((FQuat)(ref rotation)).Rotator(), setLookAt: false);
		}

		public static void TeleportLocalPlayerToRebirthPoint(MainCharacterEntity mainEntity, int rebirthPointId)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			FTransform rebirthPointTransform = GetRebirthPointTransform(rebirthPointId);
			FVector location = ((FTransform)(ref rebirthPointTransform)).GetLocation();
			FQuat rotation = ((FTransform)(ref rebirthPointTransform)).GetRotation();
			TeleportLocalPlayer(mainEntity, location, ((FQuat)(ref rotation)).Rotator(), setLookAt: false);
		}

		public static void RebirthDeadPlayer(BGUCharacterCS playerPawn, int rebirthPointId)
		{
			BPS_GSEventCollection obj = BPS_GSEventCollection.Get(((APawn)playerPawn).PlayerState);
			if (obj != null)
			{
				obj.Evt_SetCurrentRebirthPoint.Invoke(rebirthPointId);
			}
			BGU_DataUtil.GetReadOnlyData<BUC_UIControlData>((AActor)(object)playerPawn).SetActiveDeathUI(true);
			BGW_UIEventCollection obj2 = BGW_UIEventCollection.Get((UObject)(object)playerPawn);
			if (obj2 != null)
			{
				obj2.Evt_UI_ActiveDeathUI.Invoke(true);
			}
		}

		public static void RebirthAlivePlayer(BGUCharacterCS playerPawn, int rebirthPointId)
		{
			BPS_GSEventCollection obj = BPS_GSEventCollection.Get(((APawn)playerPawn).PlayerState);
			if (obj != null)
			{
				obj.Evt_SetCurrentRebirthPoint.Invoke(rebirthPointId);
			}
			BUS_GSEventCollection obj2 = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			if (obj2 != null)
			{
				obj2.Evt_UnitRebirth.Invoke((ERebirthType)0);
			}
		}

		public static void RebirthPlayerInPlace(BGUCharacterCS? playerPawn)
		{
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			if ((UObject)(object)val != (UObject)null)
			{
				val.Evt_OnLeaveFalling.Invoke();
				val.Evt_RebirthTeleportFinish.Invoke((ERebirthType)0);
				val.Evt_TriggerTeleportResetPlayer.Invoke();
			}
		}

		public static void RestPlayer(BGUCharacterCS playerPawn)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			if (obj != null)
			{
				obj.Evt_TriggerPlayerRest.Invoke();
			}
		}

		public static void StartJump(BGUCharacterCS playerPawn, ESkillDirection startJumpDir, FVector2D inputVector)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerPawn);
			if (obj != null)
			{
				obj.Evt_TriggerJumpSkill.Invoke(startJumpDir, inputVector);
			}
		}

		public static void StopJump(BGUCharacterCS playerPawn)
		{
			BUS_EventCollectionCS.Get((AActor)(object)playerPawn).Evt_Jump_OnReleased.Invoke();
		}

		private static FTransform GetCurrentRebirthPointTransform()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			BPC_RebirthPointData readOnlyData = BGU_DataUtil.GetReadOnlyData<BPC_RebirthPointData>((AActor)(object)GameUtils.GetPlayerController());
			if (readOnlyData == null)
			{
				Logging.LogError("rebirthPointData is null");
				return FTransform.Default;
			}
			FTransform result = default(FTransform);
			UBGWFunctionLibraryCS.GetRebirthPointTransform((UObject)(object)GameUtils.GetWorld(), readOnlyData.CurrentBirthPoint.PointID, ref result);
			return result;
		}

		private static FTransform GetRebirthPointTransform(int rebirthPointId)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			FTransform result = default(FTransform);
			UBGWFunctionLibraryCS.GetRebirthPointTransform((UObject)(object)GameUtils.GetWorld(), rebirthPointId, ref result);
			return result;
		}

		public static void LogRebirthPointChange(AActor worldContext, int rebirthPointID)
		{
			Logging.LogInformation("Rebirth point as current birth point ID updated: {Id}", rebirthPointID);
			FUStRebirthPointDesc fUStRebirthPointDesc = GameDBRuntime.GetFUStRebirthPointDesc(rebirthPointID);
			if (fUStRebirthPointDesc != null && BGUFuncLibMap.IsValidLevelId(fUStRebirthPointDesc.MapID))
			{
				Logging.LogDebug("MapId: {Id}", fUStRebirthPointDesc.MapID);
				Logging.LogDebug("MapAreaId: {Id}", BGUFuncLibMap.GetAreaId(worldContext));
			}
		}

		public static void SetCollisionEnabled(BGUCharacterCS? character, bool enabled)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (!((UObject)(object)character == (UObject)null))
			{
				((UPrimitiveComponent)((ACharacter)character).CapsuleComponent).SetCollisionProfileName(enabled ? B1GlobalFNames.Pawn : B1GlobalFNames.WindWalk_Pawn, true);
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)character);
				if (obj != null)
				{
					obj.Evt_SetIsEnableCollisionHitMove.Invoke(enabled, (ECollisionHitMoveEnableReqType)6, 0);
				}
			}
		}

		public static void RespawnSoftlockedParty(MainCharacterEntity mainCharacter)
		{
			int maxComp = 0;
			DI.Instance.World.Query<MainCharacterComponent>().ForEachEntity(delegate(ref MainCharacterComponent mainComp, Entity _)
			{
				maxComp = Math.Max(maxComp, mainComp.RebirthPointId);
			});
			mainCharacter.GetLocalState().IsRespawning = true;
			DI.Instance.Rpc.SendPartySoftlock(maxComp);
		}

		public static void DisableOtherPlayersCollision()
		{
			foreach (PlayerId otherAreaPlayer in DI.Instance.State.OtherAreaPlayers)
			{
				MainCharacterEntity? mainCharacterById = DI.Instance.PlayerState.GetMainCharacterById(otherAreaPlayer);
				if (mainCharacterById.HasValue)
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.Value.GetLocalState();
					if (!((UObject)(object)localState.Pawn == (UObject)null))
					{
						SetCollisionEnabled(localState.Pawn, enabled: false);
						localState.ShouldDisableCollision = true;
					}
				}
			}
		}

		public static void AllowOtherPlayersCollision()
		{
			foreach (PlayerId otherAreaPlayer in DI.Instance.State.OtherAreaPlayers)
			{
				MainCharacterEntity? mainCharacterById = DI.Instance.PlayerState.GetMainCharacterById(otherAreaPlayer);
				if (mainCharacterById.HasValue)
				{
					ref LocalMainCharacterComponent localState = ref mainCharacterById.Value.GetLocalState();
					if (!((UObject)(object)localState.Pawn == (UObject)null))
					{
						localState.ShouldDisableCollision = false;
					}
				}
			}
		}

		public static void EnableSpectator(MainCharacterEntity mainEntity, SpectatorReason reason)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Enabling spectator mode for player {PlayerId} with reason {Reason}", mainEntity.GetState().CharacterNickName, reason);
			ref PvPComponent pvP = ref mainEntity.GetPvP();
			pvP.IsSpectator = true;
			pvP.SpectatorReason = reason;
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if ((UObject)(object)localState.Pawn != (UObject)null)
			{
				localState.BeforeSpectatorLocation = ((AActor)localState.Pawn).GetActorLocation();
			}
		}

		public static void DisableSpectator(MainCharacterEntity mainEntity)
		{
			Logging.LogDebug("Disabling spectator mode for player {PlayerId}", mainEntity.GetState().CharacterNickName);
			mainEntity.GetPvP().IsSpectator = false;
			mainEntity.GetLocalState().IsDuringDeathAnim = false;
		}
	}
	public static class ProjectileUtils
	{
		private static Type? _projectileCtrlDataType;

		public static void SetProjectileTarget(BGUCharacterCS player, string projectileName, BGUCharacterCS target, string socketName)
		{
			if (((UObject?)(object)player).IsNullOrDestroyed())
			{
				Logging.LogWarning("Player is null in SwitchProjectileInfo");
				return;
			}
			Logging.LogDebug("SetProjectileTarget called for projectile {ProjectileName} with target {TargetName}", projectileName, ((UObject)target).GetName());
			BGUProjectileBaseActor playerProjectileByName = GetPlayerProjectileByName(player, projectileName);
			if (IsProjectileValid(playerProjectileByName, projectileName, ((UObject)player).GetName()))
			{
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerProjectileByName);
				if (obj != null)
				{
					obj.Evt_SwitchMovementTarget.Invoke((AActor)(object)target, socketName);
				}
			}
		}

		public static void DestroyProjectile(BGUCharacterCS player, string projectileName, EBGUBulletDestroyReason reason)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (((UObject?)(object)player).IsNullOrDestroyed())
			{
				Logging.LogWarning("Player is null in SwitchProjectileInfo");
				return;
			}
			Logging.LogDebug("DestroyProjectile called for projectile {ProjectileName} with reason {Reason}", projectileName, reason);
			BGUProjectileBaseActor playerProjectileByName = GetPlayerProjectileByName(player, projectileName);
			if (IsProjectileValid(playerProjectileByName, projectileName, ((UObject)player).GetName()))
			{
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerProjectileByName);
				if (obj != null)
				{
					obj.Evt_OnProjectileDead.Invoke(reason);
				}
			}
		}

		public static void SetProjectileMode(BGUCharacterCS player, string projectileName, EBulletOrMagicFieldMoveModeType moveMode)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (((UObject?)(object)player).IsNullOrDestroyed())
			{
				Logging.LogWarning("Player is null in SwitchProjectileInfo");
				return;
			}
			Logging.LogDebug("SetProjectileMode called for projectile {ProjectileName} with move mode {MoveMode}", projectileName, moveMode);
			BGUProjectileBaseActor playerProjectileByName = GetPlayerProjectileByName(player, projectileName);
			if (IsProjectileValid(playerProjectileByName, projectileName, ((UObject)player).GetName()))
			{
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)playerProjectileByName);
				if (obj != null)
				{
					obj.Evt_SetObjMoveMode.Invoke(moveMode);
				}
			}
		}

		public static void SwitchProjectileInfo(BGUCharacterCS player, string projectileName, int bulletSwitchID, int switchIdx)
		{
			if (((UObject?)(object)player).IsNullOrDestroyed())
			{
				Logging.LogWarning("Player is null in SwitchProjectileInfo");
				return;
			}
			Logging.LogDebug("SwitchProjectileInfo called for projectile {ProjectileName} with switch id {MoveMode}", projectileName, bulletSwitchID);
			BGUProjectileBaseActor playerProjectileByName = GetPlayerProjectileByName(player, projectileName);
			if (IsProjectileValid(playerProjectileByName, projectileName, ((UObject)player).GetName()))
			{
				BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)player);
				if (obj != null)
				{
					obj.Evt_OnSwitchOneProjectile.Invoke(playerProjectileByName, bulletSwitchID, switchIdx, (AActor)null);
				}
			}
		}

		private static BGUProjectileBaseActor? GetPlayerProjectileByName(BGUCharacterCS player, string projectileName)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			if (_projectileCtrlDataType == null)
			{
				InitProjectileCtrlDataType();
				if (_projectileCtrlDataType == null)
				{
					Logging.LogError("Failed to find type b1.BUC_ProjectileCtrData");
					return null;
				}
			}
			IBUC_ProjectileCtrlData val = (IBUC_ProjectileCtrlData)BGU_DataUtil.GetReadOnlyData((AActor)(object)player, TypeManager.GetTypeIndex(_projectileCtrlDataType));
			if (val == null)
			{
				Logging.LogError("Projectile control data is null for player: {PlayerName}", ((UObject)player).GetName());
				return null;
			}
			foreach (BGUProjectileBaseActor projectile in val.ProjectileList)
			{
				object obj;
				if (projectile == null)
				{
					obj = null;
				}
				else
				{
					UClass obj2 = ((UObject)projectile).GetClass();
					obj = ((obj2 != null) ? ((UObject)obj2).GetName() : null);
				}
				if ((string?)obj == projectileName)
				{
					return projectile;
				}
			}
			return null;
		}

		private static void InitProjectileCtrlDataType()
		{
			_projectileCtrlDataType = typeof(IBUC_ProjectileCtrlData).Assembly.GetType("b1.BUC_ProjectileCtrData", throwOnError: false, ignoreCase: false);
		}

		private static bool IsProjectileValid(BGUProjectileBaseActor? projectile, string projectileName, string playerName)
		{
			if ((UObject)(object)projectile == (UObject)null)
			{
				Logging.LogWarning("Projectile not found: {ProjectileName} for player: {PlayerName}", projectileName, playerName);
				return false;
			}
			return true;
		}
	}
	public static class SpawningUtils
	{
		public static BGUCharacterCS? SpawnCloneForPlayer(WukongPlayerState playerState, in MainCharacterEntity mainEntity)
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Expected O, but got Unknown
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Expected I4, but got Unknown
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			ref MainCharacterComponent state = ref mainEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			PvPComponent pvP = mainEntity.GetPvP();
			ref readonly TeamComponent team = ref mainEntity.GetTeam();
			PlayerId playerId = state.PlayerId;
			if (localState.HasPawn)
			{
				Logging.LogDebug("Player already exists: {Id}", playerId);
				return null;
			}
			BGUCharacterCS val = playerState.LocalMainCharacter?.GetLocalState().Pawn;
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Local player pawn is null");
				return null;
			}
			int curLevelId = BGUFuncLibMap.GetCurLevelId((UObject)(object)val);
			int areaId = BGUFuncLibMap.GetAreaId((AActor)(object)val);
			bool flag = curLevelId == 13 && areaId == 0;
			UClass val2 = BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld()).TryGetCachedResourceObj<UClass>(flag ? "/Game/00Main/Design/Units/Player/Unit_player_dasheng.Unit_player_dasheng_C" : "/Game/00Main/Design/Units/Player/Unit_Player_Wukong.Unit_Player_Wukong_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			if ((UObject)(object)val2 == (UObject)null)
			{
				Logging.LogError("Player pawn class is null");
				return null;
			}
			BGUPlayerCharacterCS controlledPawn = GameUtils.GetControlledPawn();
			if ((UObject)(object)controlledPawn == (UObject)null)
			{
				Logging.LogError("Old pawn is null");
				return null;
			}
			FVector val3 = state.Location.ToFVector();
			FRotator val4 = state.Rotation.ToFRotator();
			UClass val5 = UClass.GetClass("BGP_AIPlayerControllerB1");
			if ((UObject)(object)val5 == (UObject)null)
			{
				Logging.LogError("Class is null");
				return null;
			}
			BGP_PlayerControllerB1 playerController = GameUtils.GetPlayerController();
			FRotator controlRotation = ((AController)playerController).GetControlRotation();
			BGUCharacterCS val6 = SpawnWukong((ABGPPlayerController)(object)playerController, val2, new FTransform(val4, val3), (APawn)(object)controlledPawn);
			if ((UObject)(object)val6 == (UObject)null)
			{
				Logging.LogError("Failed to spawn new pawn");
				return null;
			}
			GameUtils.PossesPawnWithViewTarget((ABGPPlayerController)(object)playerController, (APawn)(object)controlledPawn, (APawn)(object)val6, controlRotation);
			Logging.LogDebug("Assigned player {PlayerId} clone {CloneHash}", playerId, AActorStatics.GetEntityHash((AActor)(object)val6));
			UWorld? world = GameUtils.GetWorld();
			AActor val7 = ((world != null) ? world.SpawnActor(val5, ref val3, ref val4) : null);
			if ((UObject)(object)val7 != (UObject)null)
			{
				BGP_AIPlayerControllerCS val8 = (BGP_AIPlayerControllerCS)(object)((val7 is BGP_AIPlayerControllerCS) ? val7 : null);
				if (val8 != null)
				{
					Logging.LogDebug("Spawned new controller");
					((AController)val8).Possess((APawn)(object)val6);
				}
			}
			BUS_EventCollectionCS.Get((AActor)(object)val6).Evt_OnLeaveFalling.Invoke();
			BUS_EventCollectionCS.Get((AActor)(object)controlledPawn).Evt_OnLeaveFalling.Invoke();
			int teamId = team.TeamId;
			float hp = state.Hp;
			Logging.LogDebug("Setting initial HP to {Hp}", hp);
			float hpMaxBase = state.HpMaxBase;
			Logging.LogDebug("Setting initial HPMax to {HpMax}", hpMaxBase);
			localState.Pawn = val6;
			BUC_AttrContainer val9 = (BUC_AttrContainer)BGU_DataUtil.GetReadOnlyData<IBUC_AttrContainer, BUC_AttrContainer>((AActor)(object)val6);
			if (val9 != null)
			{
				float num = val9.SetFloatValue((EBGUAttrFloat)101, hpMaxBase);
				float num2 = val9.SetFloatValue((EBGUAttrFloat)151, hp);
				Logging.LogDebug("Set actual Hp / HpMax: {Hp} {HpMax}", num2, num);
				foreach (EBGUAttrFloat syncedAttribute in WukongMp.Api.Configuration.Constants.SyncedAttributes)
				{
					if (state.Attributes.TryGetAttribute((byte)(int)syncedAttribute, out var value))
					{
						val9.SetFloatValue(syncedAttribute, value);
					}
				}
			}
			else
			{
				Logging.LogError("Failed to get attribute container from player");
			}
			Logging.LogDebug("Assigning team ID {TeamId} to player", teamId);
			ClientUtils.RegisterAndSetPlayerTeam(val6, teamId);
			Logging.LogDebug("Setting initial Nickname to {Nickname}", state.CharacterNickName);
			Logging.LogDebug("Setting initial IsReadyForPvP to {IsReady}", pvP.IsReadyForPvP);
			Logging.LogDebug("Setting initial IsSpectator to {IsSpectator}", pvP.IsSpectator);
			FUStUnitCommDesc unitCommDesc = BGW_GameDB.GetUnitCommDesc(val6.GetResID());
			if (unitCommDesc != null)
			{
				unitCommDesc.CameraLockDist = 10000f;
			}
			return val6;
		}

		public static BGUCharacterCS? SpawnWukong(ABGPPlayerController oldController, UClass pawnClass, FTransform spawnTransform, APawn oldPawn)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			AActor obj = BGU_UnrealActorUtil.BGUBeginDeferredActorSpawnFromClass((UObject)(object)((AActor)oldController).World, TSubclassOf<AActor>.op_Implicit(pawnClass), spawnTransform, (ESpawnActorCollisionHandlingMethod)2, (AActor)null);
			APawn val = (APawn)(object)((obj is APawn) ? obj : null);
			((AController)oldController).Possess(val);
			BGUCharacterCS val2 = (BGUCharacterCS)(object)((val is BGUCharacterCS) ? val : null);
			if (val2 == null)
			{
				Logging.LogError("Failed to cast pawn to ACharacter");
				return null;
			}
			((UPrimitiveComponent)((ACharacter)val2).CapsuleComponent).SetGenerateOverlapEvents(false);
			((UPrimitiveComponent)((ACharacter)val2).CapsuleComponent).SetGenerateOverlapEvents(false);
			BGU_UnrealActorUtil.BGUFinishSpawningActorAndECSBeginPlay((UObject)(object)oldController, (AActor)(object)val2, spawnTransform);
			BPS_GSEventCollection.Get((APlayerController)(object)oldController).Evt_BPS_OnControlledPawnChange.Invoke((APawn)(object)val2);
			BGS_GSEventCollection obj2 = BGS_EventCollectionCS.Get((UObject)(object)oldController);
			if (obj2 != null)
			{
				obj2.Evt_NotifyPossessEntityChanged.Invoke(ECSExtension.ToEntity((AActor)(object)oldPawn), ECSExtension.ToEntity((AActor)(object)val2));
			}
			((UPrimitiveComponent)((ACharacter)val2).CapsuleComponent).SetGenerateOverlapEvents(true);
			((UPrimitiveComponent)((ACharacter)val2).CapsuleComponent).SetGenerateOverlapEvents(true);
			UGSE_ActorFuncLib.UpdateActorOverlaps((AActor)(object)val2);
			return val2;
		}

		public static FVector CalculateSpawnLocation(FVector playerLocation, FVector playerForwardVector)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			FVector val = playerLocation + playerForwardVector * 2000.0;
			FVector val2 = val + FVector.UpVector * 2000.0 / 2.0;
			FVector val3 = val - FVector.UpVector * 2000.0 / 2.0;
			FHitResultSimple val4 = new FHitResultSimple();
			if (BGUFuncLibSelectTargetsCS.LineTraceForHitWorldItem((UObject)(object)GameUtils.GetWorld(), val2, val3, ref val4, false))
			{
				val = val4.HitLocation + FVector.UpVector * 200.0;
			}
			return val;
		}

		public static void SpawnUnitsAsOwner(string unitName, int count, int teamId, FVector spawnLocation)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)Math.Ceiling(Math.Sqrt(count));
			int num2 = (int)Math.Ceiling((float)count / (float)num);
			float num3 = (0f - (float)(num - 1) * 200f) / 2f;
			float num4 = (0f - (float)(num2 - 1) * 200f) / 2f;
			int num5 = 0;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num6 = num3 + (float)j * 200f;
					float num7 = num4 + (float)i * 200f;
					FVector locaction = spawnLocation + new FVector((double)num6, (double)num7, 0.0);
					SpawnUnitAsOwner(unitName, locaction, teamId);
					num5++;
					if (num5 == count)
					{
						return;
					}
				}
			}
		}

		public unsafe static void SpawnUnitAsOwner(string unitName, FVector locaction, int teamId)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			string guid = Guid.NewGuid().ToString();
			BUTamerActor val = SpawnUnitLocallyByName(guid, unitName, locaction);
			if ((UObject)(object)val != (UObject)null)
			{
				string unitPath = UnitPathsConfig.GetUnitPath(unitName);
				TamerEntity tamerEntity = CreateMonsterInEcs(guid, val, teamId, unitPath);
				ref TransformComponent transform = ref tamerEntity.GetTransform();
				transform.Position = locaction.ToVector3();
				transform.Rotation = Vector3.Zero;
				tamerEntity.GetNickname().Nickname = "Bot";
				Logging.LogDebug("Sending spawn unit {Name} at {Location}", unitName, ((object)(*(FVector*)(&locaction))/*cast due to .constrained prefix*/).ToString());
				DI.Instance.Rpc.SendSpawnUnit(new UnitSpawnData(unitName, guid, locaction));
			}
		}

		public static BUTamerActor? SpawnUnitLocallyByName(string guid, string unitName, FVector location)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (!UnitPathsConfig.IsValidUnitName(unitName))
			{
				Logging.LogError("Invalid unit name in SpawnUnitLocallyByName: {UnitName}", unitName);
				return null;
			}
			Logging.LogDebug("Spawn unit called for {UnitName}", unitName);
			string unitPath = UnitPathsConfig.GetUnitPath(unitName);
			if (string.IsNullOrEmpty(unitPath))
			{
				return null;
			}
			return SpawnUnitLocallyByPath(guid, unitPath, location);
		}

		public static BUTamerActor? SpawnUnitLocallyByPath(string guid, string unitPath, FVector location)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			UWorld? world = GameUtils.GetWorld();
			UClass val = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>(unitPath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			FTransform val2 = default(FTransform);
			((FTransform)(ref val2))..ctor(FRotator.ZeroRotator, location);
			AActor obj = UBGUFunctionLibrary.BGUBeginDeferredActorSpawnFromClass((UObject)(object)world, TSubclassOf<AActor>.op_Implicit(val), val2, (ESpawnActorCollisionHandlingMethod)2, (AActor)null);
			BUTamerActor val3 = (BUTamerActor)(object)((obj is BUTamerActor) ? obj : null);
			if ((UObject)(object)val3 == (UObject)null)
			{
				if (!unitPath.Contains("PersistentLevel"))
				{
					Logging.LogError("Could not spawn unit: {UnitPath}", unitPath);
				}
				return null;
			}
			val3.MarkAsSpawnedTamer((FUnitSpawnInfo)null);
			val3.ExtendConfigComp.ActorResetType = (EBGUResetType)1;
			((ABGUTamerBase)val3).SpawnedTamerGuid = guid;
			val3.GetFinalGuid(true);
			UBGUFunctionLibrary.BGUFinishSpawningActor((AActor)(object)val3, val2);
			Logging.LogDebug("Spawned enemy: {TamerName}, with Guid {Guid}", ((UObject)val3).GetName(), guid);
			return val3;
		}

		public static TamerEntity CreateMonsterInEcs(string guid, BUTamerActor tamer, int teamId, string unitName)
		{
			Logging.LogDebug("Created monster state with team ID: {TeamId} (assigned)", teamId);
			return new TamerEntity(DI.Instance.PawnState.CreateNetworkedMonster(new LocalTamerComponent(tamer), new TamerComponent
			{
				Guid = guid,
				UnitPath = unitName
			}, new TeamComponent
			{
				TeamId = teamId
			}));
		}

		public static BUTamerActor? BeginDeferredSummonSpawn(UWorld? world, TSubclassOf<BUTamerActor> tamerClass, FTransform transform, int summonId, bool safeClampToLand = false)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			if ((UObject)(object)world == (UObject)null || (UObject)(object)tamerClass.Value == (UObject)null)
			{
				return null;
			}
			AActor obj = UBGUFunctionLibrary.BGUBeginDeferredActorSpawnFromClass((UObject)(object)world, TSubclassOf<AActor>.op_Implicit(tamerClass.Value), transform, (ESpawnActorCollisionHandlingMethod)1, (AActor)null);
			BUTamerActor val = (BUTamerActor)(object)((obj is BUTamerActor) ? obj : null);
			if ((UObject)(object)val == (UObject)null)
			{
				return null;
			}
			if (safeClampToLand)
			{
				FVector val2 = BGUFuncLibActorTransformCS.BGUGetActorLocation((AActor)(object)val);
				float scaledCapsuleHalfHeight = ((ABGUTamerBase)val).CapsuleComponent.GetScaledCapsuleHalfHeight();
				float scaledCapsuleRadius = ((ABGUTamerBase)val).CapsuleComponent.GetScaledCapsuleRadius();
				FVector val3 = val2 + FVector.UpVector * (double)scaledCapsuleHalfHeight * 2.0;
				FVector val4 = val2 - FVector.UpVector * (double)scaledCapsuleHalfHeight * 2.0;
				List<AActor> list = new List<AActor>(1) { (AActor)(object)val };
				FHitResult val5 = default(FHitResult);
				if (USystemLibrary.CapsuleTraceSingleByProfile((UObject)(object)world, val3, val4, scaledCapsuleRadius, scaledCapsuleHalfHeight, B1GlobalFNames.Pawn, false, list, (EDrawDebugTrace)0, ref val5, true, FLinearColor.Red, FLinearColor.Blue, 3f))
				{
					FVector val6 = BGUFunctionLibraryCS.BGUGetVectorFromNetQuantizeVector(ref val5.ImpactPoint) + FVector.UpVector * (double)scaledCapsuleHalfHeight;
					BGUFuncLibActorTransformCS.BGUSetActorLocation((AActor)(object)val, val6, false, false, false, false);
				}
			}
			if (B1Global.GIsBossRushMode)
			{
				IBIC_BossRushBattleData gameInstanceReadonlyData = BGU_DataUtil.GetGameInstanceReadonlyData<IBIC_BossRushBattleData, BIC_BossRushBattleData>((UObject)(object)world);
				if (gameInstanceReadonlyData != null && gameInstanceReadonlyData.ServantPropertyOverrideList.TryGetValue(summonId, out var value))
				{
					val.ApplyServantPropertyOverride(value);
				}
			}
			return val;
		}

		public static void SpawnSummonedUnitWithGuid(FServantReq servantReq)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			UWorld world = GameUtils.GetWorld();
			BUTamerActor val = BeginDeferredSummonSpawn(world, TSubclassOf<BUTamerActor>.op_Implicit(servantReq.TamerTemplate), servantReq.BornTransform, servantReq.SummonID, servantReq.SafeClampToLand);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Cannot spawn tamer {Name}", ((UObject)servantReq.TamerTemplate).GetName());
				return;
			}
			Logging.LogDebug("Spawned tamer {Name} with type {Type}", ((UObject)servantReq.TamerTemplate).GetName(), servantReq.ServantType);
			((ABGUTamerBase)val).SpawnedTamerGuid = servantReq.ServantTamerGuid;
			val.MarkAsServant();
			BPS_EventCollectionCS.GetLocal((UObject)(object)world).Evt_SendServantReq.Invoke(servantReq);
			UBGUFunctionLibrary.BGUFinishSpawningActor((AActor)(object)val, servantReq.BornTransform);
		}

		public static bool CanSummon(AActor summoner, FVector summonLocation)
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			MainCharacterEntity? localCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (!localCharacter.HasValue)
			{
				return false;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(summoner);
			if (entityByPlayerPawn.HasValue && (UObject)(object)summoner == (UObject)(object)localCharacter.Value.GetLocalState().Pawn)
			{
				return true;
			}
			if (entityByPlayerPawn.HasValue)
			{
				return false;
			}
			if (!DI.Instance.PlayerState.LocalPlayerId.HasValue)
			{
				return false;
			}
			if (DI.Instance.AreaState.IsMasterClient)
			{
				return true;
			}
			PlayerId localPlayerId = DI.Instance.PlayerState.LocalPlayerId.Value;
			Vector3 localPosition = localCharacter.Value.GetState().Location;
			float num = FVector.DistSquared(localPosition.ToFVector(), summonLocation);
			float squaredSpawnOwnershipRadius = 56250000f;
			if (num > squaredSpawnOwnershipRadius)
			{
				return false;
			}
			bool canSummon = true;
			DI.Instance.World.Query<MainCharacterComponent>().ForEachEntity(delegate(ref MainCharacterComponent playerComp, Entity entity)
			{
				if (!(entity == localCharacter.Value.Entity) && Vector3.DistanceSquared(localPosition, playerComp.Location) < squaredSpawnOwnershipRadius && (DI.Instance.AreaState.MasterClientId == playerComp.PlayerId || playerComp.PlayerId.RawValue < localPlayerId.RawValue))
				{
					canSummon = false;
				}
			});
			return canSummon;
		}

		public static FVector GetCorrectedSpawnLocation(ACharacter character, FVector targetLocation)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			FVector result = targetLocation;
			UCapsuleComponent capsuleComponent = character.CapsuleComponent;
			float scaledCapsuleHalfHeight = capsuleComponent.GetScaledCapsuleHalfHeight();
			float scaledCapsuleRadius = capsuleComponent.GetScaledCapsuleRadius();
			FVector val = targetLocation + FVector.UpVector * (double)scaledCapsuleHalfHeight * 2.0;
			FVector val2 = targetLocation - FVector.UpVector * (double)scaledCapsuleHalfHeight * 2.0;
			FVector val3 = default(FVector);
			if (UGSE_TraceFuncLib.CharacterCapsuleTraceSingleByProfile((UObject)(object)character, val, val2, scaledCapsuleRadius, scaledCapsuleHalfHeight, B1GlobalFNames.Pawn, false, (AActor)(object)character, ref val3))
			{
				result = val3;
				((FVector)(ref result)).Z = ((FVector)(ref result)).Z + 2.4f;
			}
			return result;
		}
	}
	public static class TamerUtils
	{
		public static IEnumerable<BGUCharacterCS> GetMonsters()
		{
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null)
			{
				yield break;
			}
			BUTamerActor[] allActorsOfClass = world.GetAllActorsOfClass<BUTamerActor>();
			BUTamerActor[] array = allActorsOfClass;
			foreach (BUTamerActor val in array)
			{
				Logging.LogDebug("Found actor: {ActorName}", ((UObject)val).GetName());
				BGUCharacterCS monster = val.GetMonster();
				if ((UObject)(object)monster != (UObject)null)
				{
					Logging.LogDebug("Actor is a monster");
					yield return monster;
				}
			}
		}

		public static string UnifyUnitName(string unitName)
		{
			return unitName.ToLower().Replace("-", "").Replace("_", "");
		}

		public static void SpawnMonsterLocally(TamerEntity tamerEntity)
		{
			ref LocalTamerComponent localTamer = ref tamerEntity.GetLocalTamer();
			ref TamerComponent tamer = ref tamerEntity.GetTamer();
			BGS_GSEventCollection obj = BGS_EventCollectionCS.Get((UObject)(object)localTamer.Tamer);
			if (obj != null)
			{
				obj.Evt_TamerBlockingSpawnImmediately.Invoke(tamer.Guid);
			}
		}

		public static void DiscoverTamers()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Discovering tamers...");
			BUTamerActor[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<BUTamerActor>((UObject)(object)GameUtils.GetWorld());
			foreach (BUTamerActor val in allActorsOfClass)
			{
				FTamerRef currentRef = val.CurrentRef;
				string actorGuid = BGU_DataUtil.GetActorGuid((AActor)(object)val, false);
				Logging.LogDebug("Monster: {Name}, alive: {Flag}, phase {Phase}, type {Type}, guid: {Guid}", ((UObject)val).GetName(), (UObject)(object)val.GetMonster() != (UObject)null, currentRef.Phase, currentRef.TamerType, actorGuid);
				TamerEntity? entityByTamerGuid = DI.Instance.PawnState.GetEntityByTamerGuid(actorGuid);
				if (!entityByTamerGuid.HasValue)
				{
					SpawningUtils.CreateMonsterInEcs(actorGuid, val, 2, ((UObject)val).PathName);
					continue;
				}
				Logging.LogDebug("Monster already exists in ECS: {NetId}, guid {Guid}", entityByTamerGuid.Value.GetMeta().NetId, entityByTamerGuid.Value.GetTamer().Guid);
			}
		}

		public static void MarkMonsterLocallySpawned(ref LocalTamerComponent localTamer, MetadataComponent metadata)
		{
			if (!localTamer.IsLocallySpawned)
			{
				Logging.LogDebug("Sending UnitSpawn for tamer with guid: {Guid} (NetId {NetId})", BGU_DataUtil.GetActorGuid((AActor)(object)localTamer.Tamer, false), metadata.NetId);
				localTamer.IsLocallySpawned = true;
				DI.Instance.Rpc.SendUnitSpawned(metadata.NetId);
			}
		}

		public static void MarkMonsterLocallyDespawned(ref LocalTamerComponent localTamer, MetadataComponent metadata)
		{
			if (localTamer.IsLocallySpawned)
			{
				Logging.LogDebug("Sending UnitDespawn for tamer with guid: {Guid} (NetId {NetId})", BGU_DataUtil.GetActorGuid((AActor)(object)localTamer.Tamer, false), metadata.NetId);
				localTamer.IsLocallySpawned = false;
				DI.Instance.Rpc.SendUnitDespawn(metadata.NetId);
			}
		}

		public static void AddSpawnedUnitRefCount(PlayerId playerId, TamerEntity tamerEntity)
		{
			ref TamerComponent tamer = ref tamerEntity.GetTamer();
			MetadataComponent meta = tamerEntity.GetMeta();
			Logging.LogDebug("Adding spawned unit counter for tamer with guid: {Guid} (NetId {NetId}) for player {Player}", tamer.Guid, meta.NetId, playerId);
			tamer.HoldingPlayers = tamer.HoldingPlayers.Add(playerId);
		}

		public static void SubtractSpawnedUnitRefCount(PlayerId playerId, TamerEntity tamerEntity)
		{
			MetadataComponent meta = tamerEntity.GetMeta();
			ref TamerComponent tamer = ref tamerEntity.GetTamer();
			Logging.LogDebug("Subtracting spawned unit counter for tamer with guid: {Guid} (NetId {NetId}) for player {Player}", tamer.Guid, meta.NetId, playerId);
			SubtractSpawnedUnitRefCount(playerId, ref tamer);
		}

		public static void SubtractSpawnedUnitRefCount(PlayerId playerId, ref TamerComponent tamerComp)
		{
			tamerComp.HoldingPlayers = tamerComp.HoldingPlayers.Remove(playerId);
		}

		public static void ClearSpawnedUnitRefCount(TamerEntity tamerEntity)
		{
			ref TamerComponent tamer = ref tamerEntity.GetTamer();
			MetadataComponent meta = tamerEntity.GetMeta();
			Logging.LogDebug("Clearing spawned unit counter for tamer with guid: {Guid} (NetId {NetId})", tamer.Guid, meta.NetId);
			tamer.HoldingPlayers = tamer.HoldingPlayers.Clear();
			tamerEntity.GetLocalTamer().IsLocallySpawned = false;
		}

		public static void TriggerSkillInteract(Entity entity, int skillId)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("TriggerInteract for entity: {Entity}", entity.ToString());
			BUS_EventCollectionCS.Get((AActor)(object)entity.GetComponent<LocalTamerComponent>().Pawn).Evt_UnitCastSkillTryCMultiCast.Invoke(new FCastSkillInfo(skillId, (ECastSkillSourceType)40, false, (ESkillDirection)0, (EMontageBindReason)0));
		}

		public static void DestroyTamer(string guid, BUTamerActor? tamerActor, AActor? markerActor)
		{
			if (tamerActor != null)
			{
				FTamerRef currentRef = tamerActor.CurrentRef;
				if (currentRef != null)
				{
					currentRef.DestroyTamer();
				}
			}
			if (!((UObject?)(object)markerActor).IsNullOrDestroyed())
			{
				Logging.LogDebug("Destroying marker for tamer with guid {Guid}", guid);
				BGU_UnrealWorldUtil.DestroyActor(markerActor);
			}
		}

		public static void TriggerWakeUp(BGUCharacterCS character)
		{
			BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)character);
			if (obj != null)
			{
				obj.Evt_OnWakeUp.Invoke();
			}
		}
	}
	public static class TargetingUtils
	{
		public static AActor? GetTarget(BGUCharacterCS? pawn)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if ((UObject)(object)pawn == (UObject)null)
			{
				return null;
			}
			UnitLockTargetInfo targetInfo = ((BUC_TargetInfoData)BGU_DataUtil.GetReadOnlyData<IBUC_TargetInfoData, BUC_TargetInfoData>((AActor)(object)pawn)).GetTargetInfo();
			if (targetInfo == null)
			{
				return null;
			}
			return targetInfo.LockTargetActor;
		}

		public static void SetTarget(BGUCharacterCS pawn, BGUCharacterCS target)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			Logging.LogDebug("Updating target for pawn {Pawn} to pawn {Target}", BGU_DataUtil.GetActorGuid((AActor)(object)pawn, false), BGU_DataUtil.GetActorGuid((AActor)(object)target, false));
			((BUC_TargetInfoData)BGU_DataUtil.GetReadOnlyData<IBUC_TargetInfoData, BUC_TargetInfoData>((AActor)(object)pawn)).SetTargetInfo(new UnitLockTargetInfo((AActor)(object)target, (ETargetSourceType)41, (ELockTargetWayType)1, "", ""));
		}

		public static void ClearTarget(BGUCharacterCS pawn)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			Logging.LogDebug("Updating target for pawn {Pawn} to null", BGU_DataUtil.GetActorGuid((AActor)(object)pawn, false));
			((BUC_TargetInfoData)BGU_DataUtil.GetReadOnlyData<IBUC_TargetInfoData, BUC_TargetInfoData>((AActor)(object)pawn)).SetTargetInfo(new UnitLockTargetInfo());
		}
	}
	public static class TeleportUtils
	{
		public static void CheckForTeleportFinish(MainCharacterEntity mainEntity)
		{
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if (localState.TeleportFinishFrames >= 0)
			{
				if (localState.TeleportFinishFrames == 0)
				{
					DI.Instance.Rpc.SendTeleportFinish();
				}
				localState.TeleportFinishFrames--;
			}
		}
	}
	public static class TransformationUtils
	{
		public static void TransformPlayer(in MainCharacterEntity mainEntity, int toReplaceUnitResID, int toReplaceUnitBornSkillID, bool enableBlendViewTarget, EPlayerTransBeginType transBeginType)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			ref MainCharacterComponent state = ref mainEntity.GetState();
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)mainEntity.GetLocalState().Pawn);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Failed to get event collection for player {Nickname}", state.CharacterNickName);
				return;
			}
			Logging.LogDebug("Transforming player {Nickname} to unitId {UnitId} with trans type {Type}", state.CharacterNickName, toReplaceUnitResID, transBeginType);
			val.Evt_TransBeginSpawnNewOne.Invoke(toReplaceUnitResID, toReplaceUnitBornSkillID, enableBlendViewTarget, transBeginType);
		}

		public static void TransformPlayerBack(in MainCharacterEntity mainEntity, int toReplaceUnitResID, int toReplaceUnitBornSkillID, bool enableBlendViewTarget, EPlayerTransEndType transEndType)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			ref MainCharacterComponent state = ref mainEntity.GetState();
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)mainEntity.GetLocalState().Pawn);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogError("Failed to get event collection for player {Nickname}", state.CharacterNickName);
				return;
			}
			Logging.LogDebug("Transforming player {Nickname} from unitId {UnitId} with trans type {Type}", state.CharacterNickName, toReplaceUnitResID, transEndType);
			val.Evt_TransBackSpawnNewOne.Invoke(toReplaceUnitResID, toReplaceUnitBornSkillID, enableBlendViewTarget, transEndType);
		}
	}
	public static class UiUtils
	{
		public static void ShowTip(string tip, bool autoHide)
		{
			Utils.TryRunOnGameThread((Action)delegate
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Expected O, but got Unknown
				GenAGPage.ShowPage(39, "ShowTip", (ChangeReason)null, (object)null);
				GenACommTips.SetTipsData((DSTipsData)new DSSimTipsData((ETipsType)0, FText.FromString(tip), !autoHide, 5f), "ShowTip", (ChangeReason)null);
			});
		}

		private static void HideTip()
		{
			Utils.TryRunOnGameThread((Action)delegate
			{
				GenAGPage.FadeOutPage(39, "ShowTip", (ChangeReason)null);
			});
		}

		public static void SetHudVisibility(bool visible)
		{
			Utils.TryRunOnGameThread((Action)delegate
			{
				GenABattleMain.SetBattleMainTempHide(!visible, "TickUpdateUIShowState", (ChangeReason)null);
			});
		}
	}
}
namespace WukongMp.Api.Windows
{
	public class IpcHelpers
	{
		private static readonly HashSet<string> RedactedKeys = new HashSet<string> { "JWT_TOKEN" };

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern uint GetEnvironmentVariable(string lpName, StringBuilder lpBuffer, uint nSize);

		private static string? GetEnvironmentVariable(string variable)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			uint environmentVariable = GetEnvironmentVariable(variable, stringBuilder, (uint)stringBuilder.Capacity);
			if (environmentVariable == 0)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				switch (lastWin32Error)
				{
				case 203:
					return null;
				case 0:
					if (stringBuilder.Length == 0)
					{
						return null;
					}
					break;
				}
				throw new Win32Exception(lastWin32Error);
			}
			if (environmentVariable > stringBuilder.Capacity)
			{
				stringBuilder = new StringBuilder((int)environmentVariable);
				GetEnvironmentVariable(variable, stringBuilder, environmentVariable);
			}
			return stringBuilder.ToString();
		}

		public static Dictionary<string, string> ReadAndDeleteIpcHandshakeFile()
		{
			string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ReadyM.Launcher"), "wukong_handshake.env");
			if (!File.Exists(text))
			{
				Logging.LogError("Handshake file not found at {Path}. Launch the game from the ReadyM Launcher.", text);
				return new Dictionary<string, string>();
			}
			Logging.LogInformation("Reading handshake file: {FilePath}", text);
			string[] array = File.ReadAllLines(text);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Regex regex = new Regex("^(?<key>[^=]+)=(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				Match match = regex.Match(text2);
				if (match.Success)
				{
					string text3 = match.Groups["key"].Value.Trim();
					string text4 = (dictionary[text3] = match.Groups["value"].Value.Trim());
					if (RedactedKeys.Contains(text3))
					{
						Logging.LogInformation("Parsed {Key}=<redacted>", text3);
					}
					else
					{
						Logging.LogInformation("Parsed {Key}={Value}", text3, text4);
					}
				}
				else
				{
					Logging.LogError("Failed to parse line: {Line}", text2);
				}
			}
			try
			{
				File.Delete(text);
				Logging.LogInformation("Deleted handshake file: {FilePath}", text);
			}
			catch (Exception ex)
			{
				Logging.LogException(ex, null);
			}
			return dictionary;
		}
	}
	public static class MarshalHelper
	{
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		public static IntPtr GetHINSTANCE(Module module)
		{
			if (module.ModuleHandle == ModuleHandle.EmptyHandle)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return GetModuleHandle(module.Name);
		}
	}
}
namespace WukongMp.Api.UI
{
	public class ChatWidget : GameWidgetBase
	{
		private struct MessageEntry
		{
			public bool ShowSender;

			public int MessageId;

			public string Sender;

			public string Message;

			public FLinearColor Color;
		}

		private const string ChatWidgetPath = "/Game/Mods/WukongMod/WBP_MultiplayerChat.WBP_MultiplayerChat_C";

		private int _messageId;

		private bool _levelLoaded;

		private readonly Queue<string> _commandQueue = new Queue<string>();

		private readonly Queue<MessageEntry> _messageQueue = new Queue<MessageEntry>();

		private bool _hiddenManually;

		private static bool IsChatVisible_IsValid;

		private static IntPtr IsChatVisible_FunctionAddress;

		private static int IsChatVisible_ParamsSize;

		private static bool IsChatVisible_ReturnValue_IsValid;

		private static FFieldAddress? IsChatVisible_ReturnValue_PropertyAddress;

		private static int IsChatVisible_ReturnValue_Offset;

		private static bool AddMessageWithColor_IsValid;

		private static IntPtr AddMessageWithColor_FunctionAddress;

		private static int AddMessageWithColor_ParamsSize;

		private static bool AddMessageWithColor_ShowSender_IsValid;

		private static FFieldAddress? AddMessageWithColor_ShowSender_PropertyAddress;

		private static int AddMessageWithColor_ShowSender_Offset;

		private static bool AddMessageWithColor_MessageId_IsValid;

		private static FFieldAddress? AddMessageWithColor_MessageId_PropertyAddress;

		private static int AddMessageWithColor_MessageId_Offset;

		private static bool AddMessageWithColor_User_IsValid;

		private static FFieldAddress? AddMessageWithColor_User_PropertyAddress;

		private static int AddMessageWithColor_User_Offset;

		private static bool AddMessageWithColor_Message_IsValid;

		private static FFieldAddress? AddMessageWithColor_Message_PropertyAddress;

		private static int AddMessageWithColor_Message_Offset;

		private static bool AddMessageWithColor_MessageColor_IsValid;

		private static FFieldAddress? AddMessageWithColor_MessageColor_PropertyAddress;

		private static int AddMessageWithColor_MessageColor_Offset;

		public ChatWidget()
			: base("/Game/Mods/WukongMod/WBP_MultiplayerChat.WBP_MultiplayerChat_C")
		{
		}

		protected override void PostInitialize()
		{
			InitNativeFunctions();
			ClearMessages();
			ClearToolTipText();
			SetHelperText(Texts.ChatHelperNoSendDescription);
			SetWritable(isWritable: false);
		}

		public void SetWritingEnabled(bool enabled)
		{
			SetHelperText(enabled ? Texts.ChatHelperDescription : Texts.ChatHelperNoSendDescription);
			SetWritable(enabled);
		}

		public bool HasFocus()
		{
			if ((UObject)(object)GameWidget == (UObject)null)
			{
				return false;
			}
			return GameWidget.StopAction;
		}

		private void SetWritable(bool isWritable)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetWritable {isWritable}", true);
			}
		}

		public void AddMessage(bool isServerMessage, string sender, string message)
		{
			if ((UObject)(object)GameWidget == (UObject)null)
			{
				Logging.LogError("Could not add message. Chat widget not initialized");
				return;
			}
			Logging.LogDebug("Calling AddMessage function with message {Message} from {Sender}", message, sender);
			string text = $"AddMessage {isServerMessage} {++_messageId} {sender} {message}";
			if (!_levelLoaded)
			{
				_commandQueue.Enqueue(text);
			}
			else
			{
				((UObject)GameWidget).CallFunctionByNameWithArguments(text, true);
			}
		}

		public unsafe void AddMessageWithColor(bool showSender, string senderName, string message, FLinearColor messageColor)
		{
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if ((UObject)(object)GameWidget == (UObject)null || AddMessageWithColor_ShowSender_PropertyAddress == null || AddMessageWithColor_MessageId_PropertyAddress == null || AddMessageWithColor_User_PropertyAddress == null || AddMessageWithColor_Message_PropertyAddress == null || AddMessageWithColor_MessageColor_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_MultiplayerChat_C:AddMessageWithColor.");
				return;
			}
			if (!AddMessageWithColor_IsValid)
			{
				Logging.LogError("Function WBP_MultiplayerChat_C:AddMessageWithColor is not valid.");
				return;
			}
			Logging.LogDebug("Calling AddMessage function with message {Message} from {Sender}", message, senderName);
			int num = ++_messageId;
			if (!_levelLoaded)
			{
				_messageQueue.Enqueue(new MessageEntry
				{
					Color = messageColor,
					Message = message,
					Sender = senderName,
					MessageId = num,
					ShowSender = showSender
				});
				return;
			}
			byte* ptr = stackalloc byte[(int)(uint)(AddMessageWithColor_ParamsSize + 16)];
			int num2 = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num2;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)AddMessageWithColor_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			BoolMarshaler.ToNative(IntPtr.Add(intPtr, AddMessageWithColor_ShowSender_Offset), 0, AddMessageWithColor_ShowSender_PropertyAddress.Address, showSender);
			BlittableTypeMarshaler<int>.ToNative(IntPtr.Add(intPtr, AddMessageWithColor_MessageId_Offset), 0, AddMessageWithColor_MessageId_PropertyAddress.Address, num);
			FStringMarshaler.ToNative(IntPtr.Add(intPtr, AddMessageWithColor_User_Offset), 0, AddMessageWithColor_User_PropertyAddress.Address, senderName);
			FStringMarshaler.ToNative(IntPtr.Add(intPtr, AddMessageWithColor_Message_Offset), 0, AddMessageWithColor_Message_PropertyAddress.Address, message);
			BlittableTypeMarshaler<FLinearColor>.ToNative(IntPtr.Add(intPtr, AddMessageWithColor_MessageColor_Offset), 0, AddMessageWithColor_MessageColor_PropertyAddress.Address, messageColor);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, AddMessageWithColor_FunctionAddress, intPtr, AddMessageWithColor_ParamsSize);
			NativeReflection.DestroyValue_InContainer(AddMessageWithColor_User_PropertyAddress.Address, intPtr);
			NativeReflection.DestroyValue_InContainer(AddMessageWithColor_Message_PropertyAddress.Address, intPtr);
		}

		public override void SetVisibility(bool visible)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			base.SetVisibility(visible);
			if (visible)
			{
				if ((UObject)(object)GameWidget == (UObject)null)
				{
					Logging.LogError("Could not add message. Chat widget not initialized");
					return;
				}
				string item;
				while (CompatExtensions.TryDequeue(_commandQueue, out item))
				{
					((UObject)GameWidget).CallFunctionByNameWithArguments(item, true);
				}
				MessageEntry item2;
				while (CompatExtensions.TryDequeue(_messageQueue, out item2))
				{
					AddMessageWithColor(item2.ShowSender, item2.Sender, item2.Message, item2.Color);
				}
			}
			_levelLoaded = true;
		}

		public void ShowIfNotHidden()
		{
			if (!_hiddenManually)
			{
				SetVisibility(visible: true);
			}
			else
			{
				SetVisibility(visible: false);
			}
		}

		public void RemoveMessage(int messageId)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"RemoveMessage {messageId}", true);
			}
		}

		public string GetMessage()
		{
			if ((UObject)(object)GameWidget != (UObject)null)
			{
				((UObject)GameWidget).CallFunctionByNameWithArguments("GetSentMessage", true);
				string text = ((object)((UWidget)GameWidget).ToolTipText).ToString();
				if (text.Length > 0)
				{
					Logging.LogDebug("Got message: {Message} in GetSentMessage function", text);
				}
				ClearToolTipText();
				return text;
			}
			return "";
		}

		public void ToggleVisibility()
		{
			if (IsVisible())
			{
				_hiddenManually = true;
				SetVisibility(visible: false);
			}
			else
			{
				_hiddenManually = false;
				SetVisibility(visible: true);
			}
		}

		public void ClearMessages()
		{
			if ((UObject)(object)GameWidget != (UObject)null)
			{
				((UObject)GameWidget).CallFunctionByNameWithArguments("ClearMessages", true);
				_messageId = 0;
			}
		}

		public void SetHistoryNext()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SetHistoryNext", true);
				}
			}
		}

		public void SetHistoryPrev()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SetHistoryPrev", true);
				}
			}
		}

		public void SetInputFocus()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetInputFocus", true);
			}
		}

		public string CommitMessage()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("CommitMessage", true);
			}
			return GetMessage();
		}

		private void SetHelperText(string chatHelperText)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetHelperText " + chatHelperText, true);
			}
		}

		private void ClearToolTipText()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("GetSentMessage", true);
			}
		}

		public unsafe bool IsVisible()
		{
			if ((UObject)(object)GameWidget == (UObject)null || IsChatVisible_ReturnValue_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_MultiplayerChat_C:IsChatVisible.");
				return false;
			}
			if (!IsChatVisible_IsValid)
			{
				Logging.LogError("Function WBP_MultiplayerChat_C:IsChatVisible is not valid.");
				return false;
			}
			byte* ptr = stackalloc byte[(int)(uint)(IsChatVisible_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)IsChatVisible_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, IsChatVisible_FunctionAddress, intPtr, IsChatVisible_ParamsSize);
			return BlittableTypeMarshaler<bool>.FromNative(IntPtr.Add(intPtr, IsChatVisible_ReturnValue_Offset), 0, IsChatVisible_ReturnValue_PropertyAddress.Address);
		}

		static ChatWidget()
		{
			InitNativeFunctions();
		}

		public static void InitNativeFunctions()
		{
			IntPtr intPtr = NativeReflection.GetClass("/Game/Mods/WukongMod/WBP_MultiplayerChat.WBP_MultiplayerChat_C");
			IsChatVisible_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "IsChatVisible");
			IsChatVisible_ParamsSize = NativeReflection.GetFunctionParamsSize(IsChatVisible_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref IsChatVisible_ReturnValue_PropertyAddress, IsChatVisible_FunctionAddress, "IsVisible");
			IsChatVisible_ReturnValue_Offset = NativeReflectionCached.GetPropertyOffset(IsChatVisible_FunctionAddress, "IsVisible");
			IsChatVisible_ReturnValue_IsValid = NativeReflectionCached.ValidatePropertyClass(IsChatVisible_FunctionAddress, "IsVisible", Classes.FBoolProperty);
			IsChatVisible_IsValid = IsChatVisible_FunctionAddress != IntPtr.Zero && IsChatVisible_ReturnValue_IsValid;
			if (!IsChatVisible_IsValid)
			{
				Logging.LogError("Function WBP_MultiplayerChat_C:IsChatVisible is not valid.");
			}
			AddMessageWithColor_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "AddMessageWithColor");
			AddMessageWithColor_ParamsSize = NativeReflection.GetFunctionParamsSize(AddMessageWithColor_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref AddMessageWithColor_ShowSender_PropertyAddress, AddMessageWithColor_FunctionAddress, "ShowSender");
			AddMessageWithColor_ShowSender_Offset = NativeReflectionCached.GetPropertyOffset(AddMessageWithColor_FunctionAddress, "ShowSender");
			AddMessageWithColor_ShowSender_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessageWithColor_FunctionAddress, "ShowSender", Classes.FBoolProperty);
			NativeReflectionCached.GetPropertyRef(ref AddMessageWithColor_MessageId_PropertyAddress, AddMessageWithColor_FunctionAddress, "MessageId");
			AddMessageWithColor_MessageId_Offset = NativeReflectionCached.GetPropertyOffset(AddMessageWithColor_FunctionAddress, "MessageId");
			AddMessageWithColor_MessageId_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessageWithColor_FunctionAddress, "MessageId", Classes.FIntProperty);
			NativeReflectionCached.GetPropertyRef(ref AddMessageWithColor_User_PropertyAddress, AddMessageWithColor_FunctionAddress, "User");
			AddMessageWithColor_User_Offset = NativeReflectionCached.GetPropertyOffset(AddMessageWithColor_FunctionAddress, "User");
			AddMessageWithColor_User_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessageWithColor_FunctionAddress, "User", Classes.FStrProperty);
			NativeReflectionCached.GetPropertyRef(ref AddMessageWithColor_Message_PropertyAddress, AddMessageWithColor_FunctionAddress, "Message");
			AddMessageWithColor_Message_Offset = NativeReflectionCached.GetPropertyOffset(AddMessageWithColor_FunctionAddress, "Message");
			AddMessageWithColor_Message_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessageWithColor_FunctionAddress, "Message", Classes.FStrProperty);
			NativeReflectionCached.GetPropertyRef(ref AddMessageWithColor_MessageColor_PropertyAddress, AddMessageWithColor_FunctionAddress, "MessageColor");
			AddMessageWithColor_MessageColor_Offset = NativeReflectionCached.GetPropertyOffset(AddMessageWithColor_FunctionAddress, "MessageColor");
			AddMessageWithColor_MessageColor_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessageWithColor_FunctionAddress, "MessageColor", Classes.FStructProperty);
			AddMessageWithColor_IsValid = AddMessageWithColor_FunctionAddress != IntPtr.Zero && AddMessageWithColor_ShowSender_IsValid && AddMessageWithColor_MessageId_IsValid && AddMessageWithColor_User_IsValid && AddMessageWithColor_Message_IsValid && AddMessageWithColor_MessageColor_IsValid;
			if (!AddMessageWithColor_IsValid)
			{
				Logging.LogError("Function WBP_MultiplayerChat_C:AddMessageWithColor is not valid.");
			}
		}
	}
	public class CommandConsoleWidget : GameWidgetBase
	{
		private const string CommandConsoleWidgetPath = "/Game/Mods/CoreMod/WBP_CommandConsole.WBP_CommandConsole_C";

		private bool _hiddenManually;

		private static bool IsConsoleVisible_IsValid;

		private static IntPtr IsConsoleVisible_FunctionAddress;

		private static int IsConsoleVisible_ParamsSize;

		private static bool IsConsoleVisible_ReturnValue_IsValid;

		private static FFieldAddress? IsConsoleVisible_ReturnValue_PropertyAddress;

		private static int IsConsoleVisible_ReturnValue_Offset;

		private static bool HasFocus_IsValid;

		private static IntPtr HasFocus_FunctionAddress;

		private static int HasFocus_ParamsSize;

		private static bool HasFocus_ReturnValue_IsValid;

		private static FFieldAddress? HasFocus_ReturnValue_PropertyAddress;

		private static int HasFocus_ReturnValue_Offset;

		private static bool CommitCommand_IsValid;

		private static IntPtr CommitCommand_FunctionAddress;

		private static int CommitCommand_ParamsSize;

		private static bool CommitCommand_ReturnValue_IsValid;

		private static FFieldAddress? CommitCommand_ReturnValue_PropertyAddress;

		private static int CommitCommand_ReturnValue_Offset;

		private static bool SetAvailableCommands_IsValid;

		private static IntPtr SetAvailableCommands_FunctionAddress;

		private static int SetAvailableCommands_ParamsSize;

		private static bool SetAvailableCommands_Commands_IsValid;

		private static FFieldAddress? SetAvailableCommands_Commands_PropertyAddress;

		private static int SetAvailableCommands_Commands_Offset;

		private static bool AddCommandParameters_IsValid;

		private static IntPtr AddCommandParameters_FunctionAddress;

		private static int AddCommandParameters_ParamsSize;

		private static bool AddCommandParameters_Command_IsValid;

		private static FFieldAddress? AddCommandParameters_Command_PropertyAddress;

		private static int AddCommandParameters_Command_Offset;

		private static bool AddCommandParameters_AvailableParameters_IsValid;

		private static FFieldAddress? AddCommandParameters_AvailableParameters_PropertyAddress;

		private static int AddCommandParameters_AvailableParameters_Offset;

		private static bool AddMessage_IsValid;

		private static IntPtr AddMessage_FunctionAddress;

		private static int AddMessage_ParamsSize;

		private static bool AddMessage_Message_IsValid;

		private static FFieldAddress? AddMessage_Message_PropertyAddress;

		private static int AddMessage_Message_Offset;

		public CommandConsoleWidget()
			: base("/Game/Mods/CoreMod/WBP_CommandConsole.WBP_CommandConsole_C")
		{
		}

		protected override void PostInitialize()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				gameWidget.AddToViewport(1001);
			}
			InitNativeFunctions();
			SetHelperText(Texts.CommandHelperDescription);
		}

		public override void SetVisibility(bool visible)
		{
			base.SetVisibility(visible);
			if (visible)
			{
				SetInputFocus();
			}
		}

		public void ShowIfNotHidden()
		{
			if (!_hiddenManually)
			{
				SetVisibility(visible: true);
			}
			else
			{
				SetVisibility(visible: false);
			}
		}

		public void ToggleVisibility()
		{
			if (IsVisible())
			{
				_hiddenManually = true;
				SetVisibility(visible: false);
			}
			else
			{
				_hiddenManually = false;
				SetVisibility(visible: true);
			}
		}

		public void SelectUp()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SelectUp", true);
				}
			}
		}

		public void SelectDown()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SelectDown", true);
				}
			}
		}

		public void SelectSuggestion()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SelectSuggestion", true);
				}
			}
		}

		public void SetHistoryNext()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SetHistoryNext", true);
				}
			}
		}

		public void SetHistoryPrev()
		{
			if (HasFocus())
			{
				UUserWidget? gameWidget = GameWidget;
				if (gameWidget != null)
				{
					((UObject)gameWidget).CallFunctionByNameWithArguments("SetHistoryPrev", true);
				}
			}
		}

		public void SetInputFocus()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetInputFocus", true);
			}
		}

		private void SetHelperText(string chatHelperText)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetHelperText " + chatHelperText, true);
			}
		}

		public unsafe string CommitCommand()
		{
			if ((UObject)(object)GameWidget == (UObject)null || CommitCommand_ReturnValue_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:CommitCommand.");
				return "";
			}
			if (!CommitCommand_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:CommitCommand is not valid.");
				return "";
			}
			byte* ptr = stackalloc byte[(int)(uint)(CommitCommand_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)CommitCommand_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, CommitCommand_FunctionAddress, intPtr, CommitCommand_ParamsSize);
			string result = FStringMarshaler.FromNative(IntPtr.Add(intPtr, CommitCommand_ReturnValue_Offset), 0, CommitCommand_ReturnValue_PropertyAddress.Address);
			NativeReflection.DestroyValue_InContainer(CommitCommand_ReturnValue_PropertyAddress.Address, intPtr);
			return result;
		}

		public unsafe void SetAvailableCommands(List<string> availableCommands)
		{
			if ((UObject)(object)GameWidget == (UObject)null || SetAvailableCommands_Commands_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:SetAvailableCommands.");
				return;
			}
			if (!SetAvailableCommands_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:SetAvailableCommands is not valid.");
				return;
			}
			byte* ptr = stackalloc byte[(int)(uint)(SetAvailableCommands_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)SetAvailableCommands_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			TArrayCopyMarshaler<string> val = default(TArrayCopyMarshaler<string>);
			val..ctor(1, SetAvailableCommands_Commands_PropertyAddress, CachedMarshalingDelegates<string, FStringMarshaler>.FromNative, CachedMarshalingDelegates<string, FStringMarshaler>.ToNative);
			val.ToNative(IntPtr.Add(intPtr, SetAvailableCommands_Commands_Offset), (IList<string>)availableCommands);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, SetAvailableCommands_FunctionAddress, intPtr, SetAvailableCommands_ParamsSize);
			NativeReflection.DestroyValue_InContainer(SetAvailableCommands_Commands_PropertyAddress.Address, intPtr);
		}

		public unsafe void AddCommandParameters(string command, List<string> availableParameters)
		{
			if ((UObject)(object)GameWidget == (UObject)null || AddCommandParameters_Command_PropertyAddress == null || AddCommandParameters_AvailableParameters_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:AddCommandParameters.");
				return;
			}
			if (!AddCommandParameters_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:AddCommandParameters is not valid.");
				return;
			}
			byte* ptr = stackalloc byte[(int)(uint)(AddCommandParameters_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)AddCommandParameters_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			FStringMarshaler.ToNative(IntPtr.Add(intPtr, AddCommandParameters_Command_Offset), 0, AddCommandParameters_Command_PropertyAddress.Address, command);
			TArrayCopyMarshaler<string> val = default(TArrayCopyMarshaler<string>);
			val..ctor(1, AddCommandParameters_AvailableParameters_PropertyAddress, CachedMarshalingDelegates<string, FStringMarshaler>.FromNative, CachedMarshalingDelegates<string, FStringMarshaler>.ToNative);
			val.ToNative(IntPtr.Add(intPtr, AddCommandParameters_AvailableParameters_Offset), (IList<string>)availableParameters);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, AddCommandParameters_FunctionAddress, intPtr, AddCommandParameters_ParamsSize);
			NativeReflection.DestroyValue_InContainer(AddCommandParameters_Command_PropertyAddress.Address, intPtr);
			NativeReflection.DestroyValue_InContainer(AddCommandParameters_AvailableParameters_PropertyAddress.Address, intPtr);
		}

		public unsafe void AddMessage(string message)
		{
			if ((UObject)(object)GameWidget == (UObject)null || AddMessage_Message_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:AddMessage.");
				return;
			}
			if (!AddMessage_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:AddMessage is not valid.");
				return;
			}
			byte* ptr = stackalloc byte[(int)(uint)(AddMessage_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)AddMessage_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			FStringMarshaler.ToNative(IntPtr.Add(intPtr, AddMessage_Message_Offset), 0, AddMessage_Message_PropertyAddress.Address, message);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, AddMessage_FunctionAddress, intPtr, AddMessage_ParamsSize);
			NativeReflection.DestroyValue_InContainer(AddMessage_Message_PropertyAddress.Address, intPtr);
		}

		public unsafe bool IsVisible()
		{
			if ((UObject)(object)GameWidget == (UObject)null || IsConsoleVisible_ReturnValue_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:IsConsoleVisible.");
				return false;
			}
			if (!IsConsoleVisible_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:IsConsoleVisible is not valid.");
				return false;
			}
			byte* ptr = stackalloc byte[(int)(uint)(IsConsoleVisible_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)IsConsoleVisible_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, IsConsoleVisible_FunctionAddress, intPtr, IsConsoleVisible_ParamsSize);
			return BlittableTypeMarshaler<bool>.FromNative(IntPtr.Add(intPtr, IsConsoleVisible_ReturnValue_Offset), 0, IsConsoleVisible_ReturnValue_PropertyAddress.Address);
		}

		public unsafe bool HasFocus()
		{
			if ((UObject)(object)GameWidget == (UObject)null || HasFocus_ReturnValue_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_CommandConsole_C:GetHasFocus.");
				return false;
			}
			if (!HasFocus_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:GetHasFocus is not valid.");
				return false;
			}
			byte* ptr = stackalloc byte[(int)(uint)(HasFocus_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)HasFocus_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, HasFocus_FunctionAddress, intPtr, HasFocus_ParamsSize);
			return BlittableTypeMarshaler<bool>.FromNative(IntPtr.Add(intPtr, HasFocus_ReturnValue_Offset), 0, HasFocus_ReturnValue_PropertyAddress.Address);
		}

		static CommandConsoleWidget()
		{
			InitNativeFunctions();
		}

		public static void InitNativeFunctions()
		{
			IntPtr intPtr = NativeReflection.GetClass("/Game/Mods/CoreMod/WBP_CommandConsole.WBP_CommandConsole_C");
			IsConsoleVisible_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "IsConsoleVisible");
			IsConsoleVisible_ParamsSize = NativeReflection.GetFunctionParamsSize(IsConsoleVisible_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref IsConsoleVisible_ReturnValue_PropertyAddress, IsConsoleVisible_FunctionAddress, "IsVisible");
			IsConsoleVisible_ReturnValue_Offset = NativeReflectionCached.GetPropertyOffset(IsConsoleVisible_FunctionAddress, "IsVisible");
			IsConsoleVisible_ReturnValue_IsValid = NativeReflectionCached.ValidatePropertyClass(IsConsoleVisible_FunctionAddress, "IsVisible", Classes.FBoolProperty);
			IsConsoleVisible_IsValid = IsConsoleVisible_FunctionAddress != IntPtr.Zero && IsConsoleVisible_ReturnValue_IsValid;
			if (!IsConsoleVisible_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:IsConsoleVisible is not valid.");
			}
			HasFocus_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "GetHasFocus");
			HasFocus_ParamsSize = NativeReflection.GetFunctionParamsSize(HasFocus_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref HasFocus_ReturnValue_PropertyAddress, HasFocus_FunctionAddress, "HasFocus");
			HasFocus_ReturnValue_Offset = NativeReflectionCached.GetPropertyOffset(HasFocus_FunctionAddress, "HasFocus");
			HasFocus_ReturnValue_IsValid = NativeReflectionCached.ValidatePropertyClass(HasFocus_FunctionAddress, "HasFocus", Classes.FBoolProperty);
			HasFocus_IsValid = HasFocus_FunctionAddress != IntPtr.Zero && HasFocus_ReturnValue_IsValid;
			if (!HasFocus_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:GetHasFocus is not valid.");
			}
			CommitCommand_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "CommitCommand");
			CommitCommand_ParamsSize = NativeReflection.GetFunctionParamsSize(CommitCommand_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref CommitCommand_ReturnValue_PropertyAddress, CommitCommand_FunctionAddress, "Command");
			CommitCommand_ReturnValue_Offset = NativeReflectionCached.GetPropertyOffset(CommitCommand_FunctionAddress, "Command");
			CommitCommand_ReturnValue_IsValid = NativeReflectionCached.ValidatePropertyClass(CommitCommand_FunctionAddress, "Command", Classes.FStrProperty);
			CommitCommand_IsValid = CommitCommand_FunctionAddress != IntPtr.Zero && CommitCommand_ReturnValue_IsValid;
			if (!CommitCommand_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:CommitCommand is not valid.");
			}
			SetAvailableCommands_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "SetAvailableCommands");
			SetAvailableCommands_ParamsSize = NativeReflection.GetFunctionParamsSize(SetAvailableCommands_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref SetAvailableCommands_Commands_PropertyAddress, SetAvailableCommands_FunctionAddress, "Commands");
			SetAvailableCommands_Commands_Offset = NativeReflectionCached.GetPropertyOffset(SetAvailableCommands_FunctionAddress, "Commands");
			SetAvailableCommands_Commands_IsValid = NativeReflectionCached.ValidatePropertyClass(SetAvailableCommands_FunctionAddress, "Commands", Classes.FArrayProperty);
			SetAvailableCommands_IsValid = SetAvailableCommands_FunctionAddress != IntPtr.Zero && SetAvailableCommands_Commands_IsValid;
			if (!SetAvailableCommands_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:SetAvailableCommands is not valid.");
			}
			AddCommandParameters_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "AddCommandParameters");
			AddCommandParameters_ParamsSize = NativeReflection.GetFunctionParamsSize(AddCommandParameters_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref AddCommandParameters_Command_PropertyAddress, AddCommandParameters_FunctionAddress, "Command");
			AddCommandParameters_Command_Offset = NativeReflectionCached.GetPropertyOffset(AddCommandParameters_FunctionAddress, "Command");
			AddCommandParameters_Command_IsValid = NativeReflectionCached.ValidatePropertyClass(AddCommandParameters_FunctionAddress, "Command", Classes.FStrProperty);
			NativeReflectionCached.GetPropertyRef(ref AddCommandParameters_AvailableParameters_PropertyAddress, AddCommandParameters_FunctionAddress, "AvailableParameters");
			AddCommandParameters_AvailableParameters_Offset = NativeReflectionCached.GetPropertyOffset(AddCommandParameters_FunctionAddress, "AvailableParameters");
			AddCommandParameters_AvailableParameters_IsValid = NativeReflectionCached.ValidatePropertyClass(AddCommandParameters_FunctionAddress, "AvailableParameters", Classes.FArrayProperty);
			AddCommandParameters_IsValid = AddCommandParameters_FunctionAddress != IntPtr.Zero && AddCommandParameters_Command_IsValid && AddCommandParameters_AvailableParameters_IsValid;
			if (!AddCommandParameters_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:AddCommandParameters is not valid.");
			}
			AddMessage_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "AddMessage");
			AddMessage_ParamsSize = NativeReflection.GetFunctionParamsSize(AddMessage_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref AddMessage_Message_PropertyAddress, AddMessage_FunctionAddress, "Message");
			AddMessage_Message_Offset = NativeReflectionCached.GetPropertyOffset(AddMessage_FunctionAddress, "Message");
			AddMessage_Message_IsValid = NativeReflectionCached.ValidatePropertyClass(AddMessage_FunctionAddress, "Message", Classes.FStrProperty);
			AddMessage_IsValid = AddMessage_FunctionAddress != IntPtr.Zero && AddMessage_Message_IsValid;
			if (!AddMessage_IsValid)
			{
				Logging.LogError("Function WBP_CommandConsole_C:AddMessage is not valid.");
			}
		}
	}
	public class CoopStatusWidget : GameWidgetBase
	{
		private const string CoopStatusWidgetPath = "/Game/Mods/WukongMod/WBP_CoopStatus.WBP_CoopStatus_C";

		public CoopStatusWidget()
			: base("/Game/Mods/WukongMod/WBP_CoopStatus.WBP_CoopStatus_C")
		{
		}

		public void SetConnectedCount(int count)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetConnectedCount {count}", true);
			}
		}

		public void SetMaxConnectedCount(int count)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetMaxConnectedCount {count}", true);
			}
		}

		public void AddPlayer(string playerName)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("AddPlayer " + playerName, true);
			}
		}

		public void RemovePlayer(string playerName)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("RemovePlayer " + playerName, true);
			}
		}

		private void SetConnectedText(string connected)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetConnectedText " + connected, true);
			}
		}

		protected override void PostInitialize()
		{
			SetConnectedText(Texts.Connected);
		}
	}
	public class CountdownWidget : GameWidgetBase
	{
		private const string CountdownWidgetPath = "/Game/Mods/WukongMod/WBP_Countdown.WBP_Countdown_C";

		public CountdownWidget()
			: base("/Game/Mods/WukongMod/WBP_Countdown.WBP_Countdown_C")
		{
		}

		public void SetText(int seconds)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetText {seconds}", true);
			}
		}

		protected override void PostInitialize()
		{
		}
	}
	public class DebugViewWidget : GameWidgetBase
	{
		private const string DebugViewWidgetPath = "/Game/Mods/WukongMod/Debug/WBP_DebugView.WBP_DebugView_C";

		private static bool SetPlayerPosition_IsValid;

		private static IntPtr SetPlayerPosition_FunctionAddress;

		private static int SetPlayerPosition_ParamsSize;

		private static int SetPlayerPosition_PlayerName_Offset;

		private static bool SetPlayerPosition_PlayerName_IsValid;

		private static FFieldAddress? SetPlayerPosition_PlayerName_PropertyAddress;

		private static int SetPlayerPosition_GameLocation_Offset;

		private static bool SetPlayerPosition_GameLocation_IsValid;

		private static FFieldAddress? SetPlayerPosition_GameLocation_PropertyAddress;

		private static int SetPlayerPosition_EcsLocation_Offset;

		private static bool SetPlayerPosition_EcsLocation_IsValid;

		private static FFieldAddress? SetPlayerPosition_EcsLocation_PropertyAddress;

		private static bool IsWidgetVisible_IsValid;

		private static IntPtr IsWidgetVisible_FunctionAddress;

		private static int IsWidgetVisible_ParamsSize;

		private static bool IsWidgetVisible_ReturnValue_IsValid;

		private static FFieldAddress? IsWidgetVisible_ReturnValue_PropertyAddress;

		private static int IsWidgetVisible_ReturnValue_Offset;

		public DebugViewWidget()
			: base("/Game/Mods/WukongMod/Debug/WBP_DebugView.WBP_DebugView_C")
		{
		}

		public void SetVersionText(string version)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetVersionText " + version, true);
			}
		}

		public void AddPlayer(string playerName)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("AddPlayer " + playerName, true);
			}
		}

		public void RemovePlayer(string playerName)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("RemovePlayer " + playerName, true);
			}
		}

		public void ClearPlayers(string playerName)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("ClearPlayers " + playerName, true);
			}
		}

		public void ToggleVisibility()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("ToggleVisibility", true);
			}
		}

		public unsafe void SetPlayerPosition(string playerName, FVector gameLocation, FVector ecsLocation)
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if ((UObject)(object)GameWidget == (UObject)null || SetPlayerPosition_PlayerName_PropertyAddress == null || SetPlayerPosition_GameLocation_PropertyAddress == null || SetPlayerPosition_EcsLocation_PropertyAddress == null)
			{
				Logging.LogError("GameWidget or property address is null in WBP_DebugView:SetPlayerPosition.");
				return;
			}
			if (!SetPlayerPosition_IsValid)
			{
				Logging.LogError("Function WBP_DebugView:SetPlayerPosition is not valid.");
				return;
			}
			byte* ptr = stackalloc byte[(int)(uint)(SetPlayerPosition_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)SetPlayerPosition_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			FStringMarshaler.ToNative(IntPtr.Add(intPtr, SetPlayerPosition_PlayerName_Offset), 0, SetPlayerPosition_PlayerName_PropertyAddress.Address, playerName);
			BlittableTypeMarshaler<FVector>.ToNative(IntPtr.Add(intPtr, SetPlayerPosition_GameLocation_Offset), 0, SetPlayerPosition_GameLocation_PropertyAddress.Address, gameLocation);
			BlittableTypeMarshaler<FVector>.ToNative(IntPtr.Add(intPtr, SetPlayerPosition_EcsLocation_Offset), 0, SetPlayerPosition_EcsLocation_PropertyAddress.Address, ecsLocation);
			NativeReflection.InvokeStaticFunctionOptimized(((UObject)GameWidget).Address, SetPlayerPosition_FunctionAddress, intPtr, SetPlayerPosition_ParamsSize);
			NativeReflection.DestroyValue_InContainer(SetPlayerPosition_PlayerName_PropertyAddress.Address, intPtr);
		}

		public unsafe bool IsVisible()
		{
			if ((UObject)(object)GameWidget == (UObject)null || IsWidgetVisible_ReturnValue_PropertyAddress == null)
			{
				Logging.LogDebug("GameWidget or property address is null in WBP_DebugView_C:IsWidgetVisible.");
				return false;
			}
			if (!IsWidgetVisible_IsValid)
			{
				Logging.LogError("Function WBP_DebugView_C:IsWidgetVisible is not valid.");
				return false;
			}
			byte* ptr = stackalloc byte[(int)(uint)(IsWidgetVisible_ParamsSize + 16)];
			int num = (int)((16L - (long)ptr) & 0xF);
			byte* ptr2 = ptr + num;
			Unsafe.InitBlockUnaligned(ptr2, 0, (uint)IsWidgetVisible_ParamsSize);
			IntPtr intPtr = new IntPtr(ptr2);
			NativeReflection.InvokeFunctionOptimized(((UObject)GameWidget).Address, IsWidgetVisible_FunctionAddress, intPtr, IsWidgetVisible_ParamsSize);
			return BlittableTypeMarshaler<bool>.FromNative(IntPtr.Add(intPtr, IsWidgetVisible_ReturnValue_Offset), 0, IsWidgetVisible_ReturnValue_PropertyAddress.Address);
		}

		protected override void PostInitialize()
		{
			InitNativeFunctions();
		}

		static DebugViewWidget()
		{
			InitNativeFunctions();
		}

		public static void InitNativeFunctions()
		{
			IntPtr intPtr = NativeReflection.GetClass("/Game/Mods/WukongMod/Debug/WBP_DebugView.WBP_DebugView_C");
			SetPlayerPosition_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "SetPlayerPosition");
			SetPlayerPosition_ParamsSize = NativeReflection.GetFunctionParamsSize(SetPlayerPosition_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref SetPlayerPosition_PlayerName_PropertyAddress, SetPlayerPosition_FunctionAddress, "PlayerName");
			SetPlayerPosition_PlayerName_Offset = NativeReflectionCached.GetPropertyOffset(SetPlayerPosition_FunctionAddress, "PlayerName");
			SetPlayerPosition_PlayerName_IsValid = NativeReflectionCached.ValidatePropertyClass(SetPlayerPosition_FunctionAddress, "PlayerName", Classes.FStrProperty);
			NativeReflectionCached.GetPropertyRef(ref SetPlayerPosition_GameLocation_PropertyAddress, SetPlayerPosition_FunctionAddress, "GameLocation");
			SetPlayerPosition_GameLocation_Offset = NativeReflectionCached.GetPropertyOffset(SetPlayerPosition_FunctionAddress, "GameLocation");
			SetPlayerPosition_GameLocation_IsValid = NativeReflectionCached.ValidatePropertyClass(SetPlayerPosition_FunctionAddress, "GameLocation", Classes.FStructProperty);
			NativeReflectionCached.GetPropertyRef(ref SetPlayerPosition_EcsLocation_PropertyAddress, SetPlayerPosition_FunctionAddress, "EcsLocation");
			SetPlayerPosition_EcsLocation_Offset = NativeReflectionCached.GetPropertyOffset(SetPlayerPosition_FunctionAddress, "EcsLocation");
			SetPlayerPosition_EcsLocation_IsValid = NativeReflectionCached.ValidatePropertyClass(SetPlayerPosition_FunctionAddress, "EcsLocation", Classes.FStructProperty);
			SetPlayerPosition_IsValid = SetPlayerPosition_FunctionAddress != IntPtr.Zero && SetPlayerPosition_PlayerName_IsValid && SetPlayerPosition_GameLocation_IsValid && SetPlayerPosition_EcsLocation_IsValid;
			if (!SetPlayerPosition_IsValid)
			{
				Logging.LogError("Function WBP_DebugView_C:SetPlayerPosition is not valid.");
			}
			IsWidgetVisible_FunctionAddress = NativeReflectionCached.GetFunction(intPtr, "IsWidgetVisible");
			IsWidgetVisible_ParamsSize = NativeReflection.GetFunctionParamsSize(IsWidgetVisible_FunctionAddress);
			NativeReflectionCached.GetPropertyRef(ref IsWidgetVisible_ReturnValue_PropertyAddress, IsWidgetVisible_FunctionAddress, "IsVisible");
			IsWidgetVisible_ReturnValue_Offset = NativeReflectionCached.GetPropertyOffset(IsWidgetVisible_FunctionAddress, "IsVisible");
			IsWidgetVisible_ReturnValue_IsValid = NativeReflectionCached.ValidatePropertyClass(IsWidgetVisible_FunctionAddress, "IsVisible", Classes.FBoolProperty);
			IsWidgetVisible_IsValid = IsWidgetVisible_FunctionAddress != IntPtr.Zero && IsWidgetVisible_ReturnValue_IsValid;
			if (!IsWidgetVisible_IsValid)
			{
				Logging.LogError("Function WBP_DebugView_C:IsWidgetVisible is not valid.");
			}
		}
	}
	public class ErrorMessageWidget : GameWidgetBase
	{
		private const string ErrorMessageWidgetPath = "/Game/Mods/WukongMod/WBP_ErrorMessage.WBP_ErrorMessage_C";

		public ErrorMessageWidget()
			: base("/Game/Mods/WukongMod/WBP_ErrorMessage.WBP_ErrorMessage_C")
		{
		}

		public void SetText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetText " + message, true);
			}
		}

		public void ClearMessages()
		{
			SetText("");
		}

		protected override void PostInitialize()
		{
		}
	}
	public class FreeCameraControlsWidget : GameWidgetBase
	{
		private const string FreeCameraWidgetPath = "/Game/Mods/WukongMod/WBP_FreeCameraControls.WBP_FreeCameraControls_C";

		public FreeCameraControlsWidget()
			: base("/Game/Mods/WukongMod/WBP_FreeCameraControls.WBP_FreeCameraControls_C")
		{
		}

		public void SetDownDescriptionText(string down)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetDownDescriptionText " + down, true);
			}
		}

		public void SetMoveDescriptionText(string move)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetMoveDescriptionText " + move, true);
			}
		}

		public void SetRotateDescriptionText(string rotate)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetRotateDescriptionText " + rotate, true);
			}
		}

		public void SetUpDescriptionText(string up)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetUpDescriptionText " + up, true);
			}
		}

		public void SetSwitchDescriptionText(string up)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetSwitchDescriptionText " + up, true);
			}
		}

		public void SetDownControlsText(string down)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetDownControlsText " + down, true);
			}
		}

		public void SetMoveControlsText(string move)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetMoveControlsText " + move, true);
			}
		}

		public void SetRotateControlsText(string rotate)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetRotateControlsText " + rotate, true);
			}
		}

		public void SetUpControlsText(string up)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetUpControlsText " + up, true);
			}
		}

		public void SetSwitchControlsText(string up)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetSwitchControlsText " + up, true);
			}
		}

		private void SetStaticTexts(string downControls, string downDescription, string moveControls, string moveDescription, string rotateControls, string rotateDescription, string upControls, string upDescription, string switchControls, string switchDescription)
		{
			SetDownControlsText(downControls);
			SetDownDescriptionText(downDescription);
			SetMoveControlsText(moveControls);
			SetMoveDescriptionText(moveDescription);
			SetRotateControlsText(rotateControls);
			SetRotateDescriptionText(rotateDescription);
			SetUpControlsText(upControls);
			SetUpDescriptionText(upDescription);
			SetSwitchControlsText(switchControls);
			SetSwitchDescriptionText(switchDescription);
		}

		protected override void PostInitialize()
		{
			SetStaticTexts(Texts.CameraDownControls, Texts.CameraDownDescription, Texts.CameraMoveControls, Texts.CameraMoveDescription, Texts.CameraRotateControls, Texts.CameraRotateDescription, Texts.CameraUpControls, Texts.CameraUpDescription, Texts.CameraSwitchControls, Texts.CameraSwitchDescription);
		}
	}
	public class FreeCameraMessageWidget : GameWidgetBase
	{
		private const string FreeCameraMessageWidgetPath = "/Game/Mods/WukongMod/WBP_FreeCameraMessage.WBP_FreeCameraMessage_C";

		public FreeCameraMessageWidget()
			: base("/Game/Mods/WukongMod/WBP_FreeCameraMessage.WBP_FreeCameraMessage_C")
		{
		}

		public void SetMessageText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetMessageText " + message, true);
			}
		}

		protected override void PostInitialize()
		{
		}
	}
	public class GameMessageWidget : GameWidgetBase
	{
		private const string GameMessageWidgetPath = "/Game/Mods/WukongMod/WBP_GameMessage.WBP_GameMessage_C";

		public GameMessageWidget()
			: base("/Game/Mods/WukongMod/WBP_GameMessage.WBP_GameMessage_C")
		{
		}

		public void SetMainText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetMainText " + message, true);
			}
		}

		public void SetSecondText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetSecondText " + message, true);
			}
		}

		public void SetThirdText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetThirdText " + message, true);
			}
		}

		public void ClearMessages()
		{
			SetMainText("");
			SetSecondText("");
			SetThirdText("");
		}

		protected override void PostInitialize()
		{
		}
	}
	public abstract class GameWidgetBase(string path)
	{
		protected UUserWidget? GameWidget;

		public void Initialize()
		{
			GameWidget = ModWidgetsUtils.GetWidget(path);
			if ((UObject)(object)GameWidget == (UObject)null)
			{
				GameWidget = ModWidgetsUtils.SpawnWidget(path);
			}
			if ((UObject)(object)GameWidget != (UObject)null)
			{
				Logging.LogDebug("{Name} widget initialized!", path);
				PostInitialize();
			}
			else
			{
				Logging.LogError("Cannot initialize {Name} widget", path);
			}
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				gameWidget.AddToViewport(1000);
			}
			UUserWidget? gameWidget2 = GameWidget;
			if (gameWidget2 != null)
			{
				((UWidget)gameWidget2).SetVisibility((ESlateVisibility)4);
			}
		}

		protected abstract void PostInitialize();

		public virtual void SetVisibility(bool visible)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetWidgetVisibility {visible}", true);
			}
		}

		public void Deinitialize()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UWidget)gameWidget).RemoveFromParent();
			}
			GameWidget = null;
		}
	}
	public class InfoMessageWidget : GameWidgetBase
	{
		private const string InfoMessageWidgetPath = "/Game/Mods/WukongMod/WBP_InfoMessage.WBP_InfoMessage_C";

		public InfoMessageWidget()
			: base("/Game/Mods/WukongMod/WBP_InfoMessage.WBP_InfoMessage_C")
		{
		}

		public void SetText(string message)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetText " + message, true);
			}
		}

		public void ClearMessages()
		{
			SetText("");
		}

		protected override void PostInitialize()
		{
		}
	}
	public class ModVersionWidget : GameWidgetBase
	{
		private const string ModVersionWidgetPath = "/Game/Mods/WukongMod/WBP_ModVersion.WBP_ModVersion_C";

		public ModVersionWidget()
			: base("/Game/Mods/WukongMod/WBP_ModVersion.WBP_ModVersion_C")
		{
		}

		public void SetVersionText(string version)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetVersionText " + version, true);
			}
		}

		protected override void PostInitialize()
		{
		}
	}
	public class PingIndicatorWidget : GameWidgetBase
	{
		private const string PingWidgetPath = "/Game/Mods/WukongMod/WBP_PingIndicator.WBP_PingIndicator_C";

		public PingIndicatorWidget()
			: base("/Game/Mods/WukongMod/WBP_PingIndicator.WBP_PingIndicator_C")
		{
		}

		public void SetPingValue(long pingMs)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetPingValue {pingMs}", true);
			}
		}

		public void SetInfoText(string infoText)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetInfoText " + infoText, true);
			}
		}

		public void ShowInfoText()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("ShowInfoText", true);
			}
		}

		public void HideInfoText()
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("HideInfoText", true);
			}
		}

		private void SetPingText(string pingText)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetPingText " + pingText, true);
			}
		}

		private void SetUnitsText(string pingUnitsText)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments("SetUnitsText " + pingUnitsText, true);
			}
		}

		private void SetStaticTexts(string pingText, string pingUnitsText)
		{
			SetPingText(pingText);
			SetUnitsText(pingUnitsText);
		}

		protected override void PostInitialize()
		{
			SetStaticTexts(Texts.Ping, Texts.PingUnits);
		}
	}
	public static class TextUtils
	{
		public static string GetReadyText(int playersCount, bool isReady)
		{
			if (playersCount == 0)
			{
				if (!isReady)
				{
					return Texts.PressToPlayWithBots;
				}
				return Texts.PressToCancelMatch;
			}
			if (!isReady)
			{
				return Texts.PressToBeReady;
			}
			return Texts.PressToBeNotReady;
		}
	}
	public class TimerWidget : GameWidgetBase
	{
		private const string TimerWidgetPath = "/Game/Mods/WukongMod/WBP_Timer.WBP_Timer_C";

		public TimerWidget()
			: base("/Game/Mods/WukongMod/WBP_Timer.WBP_Timer_C")
		{
		}

		public void SetText(int minutes, int seconds)
		{
			UUserWidget? gameWidget = GameWidget;
			if (gameWidget != null)
			{
				((UObject)gameWidget).CallFunctionByNameWithArguments($"SetText {minutes} {seconds}", true);
			}
		}

		protected override void PostInitialize()
		{
		}
	}
	public sealed class WukongWidgetManager(ClientState clientState, WukongPlayerState playerState) : IDisposable
	{
		private string _lastDisconnectText = Texts.Disconnected;

		private bool _isInitialized;

		private string _fullModVersion = "";

		private string _shortModVersion = "";

		private List<string> _availableCommands = new List<string>();

		private Dictionary<string, IEnumerable<string>> _availableParameters = new Dictionary<string, IEnumerable<string>>();

		private readonly Lazy<CommandConsoleWidget> _commandConsoleWidget = new Lazy<CommandConsoleWidget>();

		private readonly Lazy<ChatWidget> _chatWidget = new Lazy<ChatWidget>();

		private readonly Lazy<InfoMessageWidget> _infoMessageWidget = new Lazy<InfoMessageWidget>();

		private readonly Lazy<ErrorMessageWidget> _errorMessageWidget = new Lazy<ErrorMessageWidget>();

		private readonly Lazy<PingIndicatorWidget> _pingIndicatorWidget = new Lazy<PingIndicatorWidget>();

		private readonly Lazy<FreeCameraControlsWidget> _freeCameraControlsWidget = new Lazy<FreeCameraControlsWidget>();

		private readonly Lazy<FreeCameraMessageWidget> _freeCameraMessageWidget = new Lazy<FreeCameraMessageWidget>();

		private readonly Lazy<ModVersionWidget> _modVersionWidget = new Lazy<ModVersionWidget>();

		private readonly Lazy<DebugViewWidget> _debugViewWidget = new Lazy<DebugViewWidget>();

		private readonly Lazy<TimerWidget> _timerWidget = new Lazy<TimerWidget>();

		public bool IsDebugViewVisible => _debugViewWidget.Value.IsVisible();

		public void Dispose()
		{
		}

		public void OnFreeCameraModeChanged(bool enabled)
		{
			_freeCameraControlsWidget.Value.SetVisibility(enabled);
		}

		public void ShowInGameWidgets(bool isOnGameplayLevel)
		{
			if (isOnGameplayLevel)
			{
				_pingIndicatorWidget.Value.SetVisibility(visible: true);
				_chatWidget.Value.ShowIfNotHidden();
			}
			_modVersionWidget.Value.SetVisibility(visible: true);
		}

		public void SetModVersion(string version)
		{
			_fullModVersion = version;
			string[] array = version.Split(new char[1] { '+' });
			if (array.Length != 0)
			{
				_shortModVersion = array[0];
			}
		}

		public void OnLevelLoaded()
		{
			Logging.LogDebug("Initializing widgets");
			InitializeWidgets();
			_chatWidget.Value.SetVisibility(visible: false);
			SetModVersionText();
			if (!clientState.IsConnected)
			{
				DI.Instance.RelayClient.Scheduler.Schedule(delegate(IRelayClientNetworkThreadContext ctx, WukongWidgetManager self)
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0019: Invalid comparison between Unknown and I4
					self._infoMessageWidget.Value.SetVisibility(visible: true);
					self._lastDisconnectText = (((int)ctx.LastDisconnectReason == 6) ? Texts.ConnectionRejectedByServer : Texts.Disconnected);
					self._infoMessageWidget.Value.SetText(self._lastDisconnectText);
				}, this);
			}
		}

		public void UpdateConsoleCommands(List<string> commands, Dictionary<string, IEnumerable<string>> availableParameters)
		{
			_availableCommands = commands;
			_availableParameters = availableParameters;
		}

		private void SetModVersionText()
		{
			_modVersionWidget.Value.SetVersionText(_shortModVersion);
			_debugViewWidget.Value.SetVersionText(_fullModVersion);
		}

		public void UpdatePlayerPosition(string playerName, FVector gameLocation, FVector ecsLocation)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			_debugViewWidget.Value.SetPlayerPosition(playerName, gameLocation, ecsLocation);
		}

		public void AddCharacterToDebugView(string name)
		{
			_debugViewWidget.Value.AddPlayer(name);
		}

		public void UpdatePingIndicator(long pingMs)
		{
			_pingIndicatorWidget.Value.SetPingValue(pingMs);
			_pingIndicatorWidget.Value.HideInfoText();
		}

		public void SetPacketLossWarning()
		{
			_pingIndicatorWidget.Value.SetPingValue(999L);
			_pingIndicatorWidget.Value.SetInfoText(Texts.SeverePacketLossDetected);
		}

		public void HideInfoMessage()
		{
			_infoMessageWidget.Value.SetVisibility(visible: false);
		}

		public void ShowInfoMessage(string message)
		{
			_infoMessageWidget.Value.SetText(message);
			_infoMessageWidget.Value.SetVisibility(visible: true);
		}

		public void OnExitLevel()
		{
			Logging.LogDebug("Deinitializing widgets");
			DeinitializeWidgets();
		}

		public void OnDisconnected(PlayerId playerId, Entity? entity, DisconnectReason reason)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			_infoMessageWidget.Value.SetVisibility(visible: true);
			_lastDisconnectText = (((int)reason == 6) ? Texts.ConnectionRejectedByServer : Texts.Disconnected);
			_infoMessageWidget.Value.SetText(_lastDisconnectText);
		}

		public void OnConnected(PlayerId playerId, Entity entity)
		{
			_infoMessageWidget.Value.SetVisibility(visible: false);
		}

		public void OnOtherPlayerInsideArea(PlayerId playerId, AreaId area, OtherPlayerInsideAreaReason reason)
		{
			PlayerEntity? playerById = playerState.GetPlayerById(playerId);
			if (playerById.HasValue)
			{
				_debugViewWidget.Value.AddPlayer(playerById.Value.GetState().NickName);
			}
		}

		public void OnOtherPlayerOutsideArea(PlayerId playerId, AreaId area, OtherPlayerOutsideAreaReason reason)
		{
			PlayerEntity? playerById = playerState.GetPlayerById(playerId);
			if (playerById.HasValue)
			{
				_debugViewWidget.Value.RemovePlayer(playerById.Value.GetState().NickName);
			}
		}

		public void OnJoinedArea(AreaId area, Entity areaEntity)
		{
			PlayerEntity? localPlayerEntity = playerState.LocalPlayerEntity;
			if (localPlayerEntity.HasValue)
			{
				_debugViewWidget.Value.AddPlayer(localPlayerEntity.Value.GetState().NickName);
			}
			AreaEntity areaEntity2 = new AreaEntity(areaEntity);
			_chatWidget.Value.SetWritingEnabled(areaEntity2.GetRoom().ChatEnabled);
		}

		public void OnLeftArea(AreaId arg1, Entity arg2)
		{
			PlayerEntity? localPlayerEntity = playerState.LocalPlayerEntity;
			if (localPlayerEntity.HasValue)
			{
				_debugViewWidget.Value.RemovePlayer(localPlayerEntity.Value.GetState().NickName);
			}
		}

		private void InitializeWidgets()
		{
			if (_isInitialized)
			{
				return;
			}
			_isInitialized = true;
			_commandConsoleWidget.Value.Initialize();
			_chatWidget.Value.Initialize();
			_infoMessageWidget.Value.Initialize();
			_errorMessageWidget.Value.Initialize();
			_pingIndicatorWidget.Value.Initialize();
			_freeCameraControlsWidget.Value.Initialize();
			_freeCameraMessageWidget.Value.Initialize();
			_modVersionWidget.Value.Initialize();
			_debugViewWidget.Value.Initialize();
			_timerWidget.Value.Initialize();
			_commandConsoleWidget.Value.SetAvailableCommands(_availableCommands);
			foreach (KeyValuePair<string, IEnumerable<string>> availableParameter in _availableParameters)
			{
				_commandConsoleWidget.Value.AddCommandParameters(availableParameter.Key, availableParameter.Value.ToList());
			}
		}

		private void DeinitializeWidgets()
		{
			_commandConsoleWidget.Value.Deinitialize();
			_chatWidget.Value.Deinitialize();
			_infoMessageWidget.Value.Deinitialize();
			_errorMessageWidget.Value.Deinitialize();
			_pingIndicatorWidget.Value.Deinitialize();
			_freeCameraControlsWidget.Value.Deinitialize();
			_freeCameraMessageWidget.Value.Deinitialize();
			_modVersionWidget.Value.Deinitialize();
			_debugViewWidget.Value.Deinitialize();
			_timerWidget.Value.Deinitialize();
			_isInitialized = false;
		}

		public void SetTimerText(int minutes, int seconds)
		{
			_timerWidget.Value.SetText(minutes, seconds);
		}

		public void SetTimerVisibility(bool visible)
		{
			_timerWidget.Value.SetVisibility(visible);
		}

		public void ToggleDebugVisibility()
		{
			_debugViewWidget.Value.ToggleVisibility();
		}

		public void ToggleChatVisibility()
		{
			_chatWidget.Value.ToggleVisibility();
		}

		public void AddChatMessage(bool isSystemMessage, string sender, string message, FLinearColor color)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_chatWidget.Value.AddMessageWithColor(!isSystemMessage, sender, message, color);
		}

		public bool ChatHasFocus()
		{
			return _chatWidget.Value.HasFocus();
		}

		public void SetChatInputFocus()
		{
			_chatWidget.Value.SetInputFocus();
		}

		public string CommitChatMessage()
		{
			return _chatWidget.Value.CommitMessage();
		}

		public void ToggleCommandVisibility()
		{
			_commandConsoleWidget.Value.ToggleVisibility();
		}

		public bool CommandHasFocus()
		{
			return _commandConsoleWidget.Value.HasFocus();
		}

		public bool IsCommandVisible()
		{
			return _commandConsoleWidget.Value.IsVisible();
		}

		public void CommandSelectUp()
		{
			_commandConsoleWidget.Value.SelectUp();
		}

		public void CommandSelectDown()
		{
			_commandConsoleWidget.Value.SelectDown();
		}

		public void CommandHistoryUp()
		{
			_commandConsoleWidget.Value.SetHistoryNext();
		}

		public void CommandHistoryDown()
		{
			_commandConsoleWidget.Value.SetHistoryPrev();
		}

		public void CommandSelectSuggestion()
		{
			_commandConsoleWidget.Value.SelectSuggestion();
		}

		public void SetCommandInputFocus()
		{
			_commandConsoleWidget.Value.SetInputFocus();
		}

		public string CommitCommand()
		{
			return _commandConsoleWidget.Value.CommitCommand();
		}

		public void AddMessageToConsole(string message)
		{
			_commandConsoleWidget.Value.AddMessage("> " + message);
		}

		public void SetSpectatingMessage(string message)
		{
			_freeCameraMessageWidget.Value.SetVisibility(visible: true);
			_freeCameraMessageWidget.Value.SetMessageText(message);
		}

		public void HideSpectatingMessage()
		{
			_freeCameraMessageWidget.Value.SetVisibility(visible: false);
		}
	}
}
namespace WukongMp.Api.Tests
{
	public class TestsRunner(ILogger logger)
	{
		private readonly ILogger _logger = logger;

		private TestActionBase? _currentTestAction;

		private TestActionSequenceBase? _testActionSequence;

		public bool IsRunning { get; private set; }

		public void Init(TestActionSequenceBase testActionSequence)
		{
			_testActionSequence = testActionSequence;
		}

		public void Start()
		{
			IsRunning = true;
		}

		public void Stop()
		{
			IsRunning = false;
		}

		public void Clear()
		{
			_testActionSequence?.Clear();
			_testActionSequence = null;
			_currentTestAction = null;
		}

		public void Update(float deltaTime)
		{
			if (!IsRunning || _testActionSequence == null)
			{
				return;
			}
			if (_currentTestAction == null)
			{
				if (!_testActionSequence.HasQueuedTests())
				{
					return;
				}
				_currentTestAction = _testActionSequence.GetNextQueuedTestAction();
				logger.LogDebug("Starting test action: {TestName}", _currentTestAction.GetType().Name);
			}
			switch (_currentTestAction.Update(deltaTime))
			{
			case TestState.Succeeded:
				logger.LogDebug("Test action: {TestName} successfully finishsed.", _currentTestAction.GetType().Name);
				_currentTestAction = null;
				break;
			case TestState.Failed:
				logger.LogError("Test action: {TestName} failed. Description: {Description}", _currentTestAction.GetType().Name, _currentTestAction.Description);
				_currentTestAction = null;
				break;
			}
		}
	}
	public enum TestState : byte
	{
		Running,
		Failed,
		Succeeded
	}
}
namespace WukongMp.Api.Tests.TestActions
{
	internal class BackToMainManuTestAction : TestActionBase
	{
		private enum InnerState
		{
			LeaveGame,
			WaitForMainMenu
		}

		private InnerState _currentState;

		private string GetCurLevelName()
		{
			UWorld worldFromObj = UGSE_EngineFuncLib.GetWorldFromObj((UObject)(object)GameUtils.GetWorld());
			if ((UObject)(object)worldFromObj != (UObject)null)
			{
				return ((UObject)worldFromObj).GetName();
			}
			return "";
		}

		private void TransferState(InnerState nextState)
		{
			_currentState = nextState;
		}

		public override TestState Update(float deltaTime)
		{
			switch (_currentState)
			{
			case InnerState.LeaveGame:
				BGW_EventCollection.Get((UObject)(object)GameUtils.GetWorld()).Evt_BGW_TriggerGlobalFSMEvent.Invoke((EGI_Global)11, (object)null);
				TransferState(InnerState.WaitForMainMenu);
				break;
			case InnerState.WaitForMainMenu:
				if (GetCurLevelName() == "Startup_V2_P")
				{
					return TestState.Succeeded;
				}
				break;
			default:
				return TestState.Failed;
			}
			return TestState.Running;
		}
	}
	internal class EnterLevelTestAction : TestActionBase
	{
		private enum InnerState
		{
			WXLogin,
			PreStartProcess,
			EnterMap,
			WaitForEnterMap,
			WaitForBeginPlay
		}

		private InnerState _currentState;

		private bool _roll;

		private bool _loadMapCompleted;

		private UObject? _worldContext;

		private BGW_GameLifeTimeMgr? _gameLifeTimeMgr;

		private void TransferState(InnerState nextState)
		{
			_currentState = nextState;
		}

		private string GetCurLevelName()
		{
			UWorld worldFromObj = UGSE_EngineFuncLib.GetWorldFromObj(_worldContext);
			if ((UObject)(object)worldFromObj != (UObject)null)
			{
				return ((UObject)worldFromObj).GetName();
			}
			return "";
		}

		private void OnPostLoadMapWithWorld()
		{
			_loadMapCompleted = true;
		}

		public override TestState Update(float deltaTime)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Expected O, but got Unknown
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Expected O, but got Unknown
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			if (_worldContext == (UObject)null)
			{
				_worldContext = (UObject?)(object)GameUtils.GetWorld();
			}
			switch (_currentState)
			{
			case InnerState.WXLogin:
				if (GetCurLevelName() == "WXLogin_P")
				{
					TransferState(InnerState.PreStartProcess);
				}
				else if (GetCurLevelName() == "Startup_V2_P")
				{
					TransferState(InnerState.EnterMap);
				}
				break;
			case InnerState.PreStartProcess:
			{
				if (GetCurLevelName() == "Startup_V2_P")
				{
					TransferState(InnerState.EnterMap);
					break;
				}
				List<UWidget> list = default(List<UWidget>);
				UGSE_UMGFuncLib.QAGetAllWidgetsOfClass((UObject)(object)UGSE_EngineFuncLib.GetWorldFromObj(_worldContext), ref list, TSubclassOf<UWidget>.op_Implicit(UClass.GetClass<GSScrollBox>()));
				if (list != null && list.Count > 0)
				{
					foreach (GSScrollBox item in list)
					{
						GSScrollBox val = item;
						if (!((UObject)(object)val == (UObject)null))
						{
							val.SetScrollOffset(val.GetScrollOffsetOfEnd(), false, false);
						}
					}
				}
				_roll = !_roll;
				if (_roll)
				{
					BGW_EventCollection.Get(_worldContext).Evt_InjectInputTriggerEvent.Invoke("IA_GSUIConfirm", (ETriggerEvent)2, FInputActionValue.True);
				}
				else
				{
					BGW_EventCollection.Get(_worldContext).Evt_InjectInputTriggerEvent.Invoke("IA_GSUIConfirm", (ETriggerEvent)5, FInputActionValue.False);
				}
				break;
			}
			case InnerState.EnterMap:
			{
				if ((UObject)(object)_gameLifeTimeMgr == (UObject)null)
				{
					_gameLifeTimeMgr = BGW_GameLifeTimeMgr.Get(_worldContext);
				}
				if ((UObject)(object)_gameLifeTimeMgr == (UObject)null)
				{
					base.Description = "GameLifeTimeMgr == null";
					return TestState.Failed;
				}
				if (!_gameLifeTimeMgr.IsInFSMState((SGI_Global)1))
				{
					break;
				}
				UClass val2 = BGW_PreloadAssetMgr.Get(_worldContext).TryGetCachedResourceObj<UClass>("WidgetBlueprint'/Game/00Main/UI/BluePrintsV3/StartGame/BUI_StartGame.BUI_StartGame_C'", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				if (!((UObject)(object)val2 == (UObject)null))
				{
					List<UUserWidget> list2 = default(List<UUserWidget>);
					UWidgetLibrary.GetAllWidgetsOfClass((UObject)(object)UGSE_EngineFuncLib.GetWorldFromObj(_worldContext), ref list2, TSubclassOf<UUserWidget>.op_Implicit(val2), false);
					if (list2.Count > 0)
					{
						BGW_EventCollection obj2 = BGW_EventCollection.Get(_worldContext);
						obj2.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Combine((Delegate?)(object)obj2.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
						BGW_EventCollection.Get(_worldContext).Evt_BGW_TriggerGlobalFSMEvent.Invoke((EGI_Global)3, (object)new FSMInputData_GI_Global_SubG_GI_Loading_TravelLevel
						{
							ArchiveId = 1
						});
						TransferState(InnerState.WaitForEnterMap);
					}
				}
				break;
			}
			case InnerState.WaitForEnterMap:
				if (_loadMapCompleted)
				{
					BGW_EventCollection obj = BGW_EventCollection.Get(_worldContext);
					obj.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Remove((Delegate?)(object)obj.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
					TransferState(InnerState.WaitForBeginPlay);
				}
				break;
			case InnerState.WaitForBeginPlay:
				if (DI.Instance.EventBus.IsGameplayLevel)
				{
					return TestState.Succeeded;
				}
				break;
			default:
				return TestState.Failed;
			}
			return TestState.Running;
		}
	}
	internal interface IGameplayTestAction
	{
	}
	internal class OpenLevelTestAction(string targetMapName) : TestActionBase()
	{
		private enum InnerState
		{
			Delay,
			OpenLevel,
			WaitForNewLevel,
			WaitForAreaConnection
		}

		private readonly string _targetMapName = targetMapName;

		private InnerState _currentState;

		private bool _loadMapCompleted;

		public override TestState Update(float deltaTime)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			switch (_currentState)
			{
			case InnerState.Delay:
				if (ElapsedTime > 20f)
				{
					TransferState(InnerState.WaitForNewLevel);
				}
				break;
			case InnerState.OpenLevel:
				if (ElapsedTime > 20f)
				{
					UWorld? world = GameUtils.GetWorld();
					BGW_EventCollection obj = BGW_EventCollection.Get((UObject)(object)world);
					obj.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Combine((Delegate?)(object)obj.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
					UGameplayStatics.OpenLevel((UObject)(object)world, new FName(targetMapName, (EFindName)1), true, (string)null);
					TransferState(InnerState.WaitForNewLevel);
				}
				break;
			case InnerState.WaitForNewLevel:
				if (_loadMapCompleted)
				{
					BGW_EventCollection obj2 = BGW_EventCollection.Get((UObject)(object)GameUtils.GetWorld());
					obj2.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Remove((Delegate?)(object)obj2.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
					TransferState(InnerState.WaitForAreaConnection);
				}
				break;
			case InnerState.WaitForAreaConnection:
				if (DI.Instance.AreaState.InRoom)
				{
					return TestState.Succeeded;
				}
				break;
			default:
				return TestState.Failed;
			}
			ElapsedTime += deltaTime;
			return TestState.Running;
		}

		private void TransferState(InnerState nextState)
		{
			_currentState = nextState;
		}

		private void OnPostLoadMapWithWorld()
		{
			_loadMapCompleted = true;
		}
	}
	internal class ReconnectTestAction : TestActionBase, IGameplayTestAction
	{
		private enum InnerState
		{
			WaitForInitialConnection,
			Reconnect,
			WaitForConnection
		}

		private InnerState _currentState;

		public override TestState Update(float deltaTime)
		{
			switch (_currentState)
			{
			case InnerState.WaitForInitialConnection:
				if (DI.Instance.AreaState.InRoom)
				{
					GoToState(InnerState.Reconnect);
				}
				break;
			case InnerState.Reconnect:
				DI.Instance.Connection.Reconnect();
				GoToState(InnerState.WaitForConnection);
				break;
			case InnerState.WaitForConnection:
				if (DI.Instance.AreaState.InRoom)
				{
					return TestState.Succeeded;
				}
				break;
			default:
				return TestState.Failed;
			}
			ElapsedTime += deltaTime;
			if (ElapsedTime > base.Timeout)
			{
				base.Description = "Timeout";
				return TestState.Failed;
			}
			return TestState.Running;
		}

		private void GoToState(InnerState state)
		{
			_currentState = state;
		}
	}
	internal class TeleportTestAction(int rebirthPointId) : TestActionBase()
	{
		private enum InnerState
		{
			Delay,
			TeleportNewLevel,
			WaitForNewLevel,
			WaitForAreaConnection
		}

		private readonly int _rebirthPointId = rebirthPointId;

		private InnerState _currentState;

		private bool _loadMapCompleted;

		public override TestState Update(float deltaTime)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			switch (_currentState)
			{
			case InnerState.Delay:
				if (ElapsedTime > 20f)
				{
					TransferState(InnerState.WaitForNewLevel);
				}
				break;
			case InnerState.TeleportNewLevel:
			{
				BGW_EventCollection obj = BGW_EventCollection.Get((UObject)(object)GameUtils.GetWorld());
				obj.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Combine((Delegate?)(object)obj.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
				BGUPlayerCharacterCS? controlledPawn = GameUtils.GetControlledPawn();
				BPS_EventCollectionCS.Get((controlledPawn != null) ? ((APawn)controlledPawn).PlayerState : null).Evt_BPS_TeleportTo.Invoke((ETeleportTypeV2)5, (ValueType)(object)new TeleportParam_RebirthPoint
				{
					RebirthPointId = rebirthPointId
				}, (EPlayerTeleportReason)2);
				TransferState(InnerState.WaitForNewLevel);
				break;
			}
			case InnerState.WaitForNewLevel:
				if (_loadMapCompleted)
				{
					BGW_EventCollection obj2 = BGW_EventCollection.Get((UObject)(object)GameUtils.GetWorld());
					obj2.Evt_PostLoadMapWithWorld = (Del_Void)Delegate.Remove((Delegate?)(object)obj2.Evt_PostLoadMapWithWorld, (Delegate?)new Del_Void(OnPostLoadMapWithWorld));
					TransferState(InnerState.WaitForAreaConnection);
				}
				break;
			case InnerState.WaitForAreaConnection:
				if (DI.Instance.AreaState.InRoom)
				{
					return TestState.Succeeded;
				}
				break;
			default:
				return TestState.Failed;
			}
			ElapsedTime += deltaTime;
			return TestState.Running;
		}

		private void TransferState(InnerState nextState)
		{
			_currentState = nextState;
		}

		private void OnPostLoadMapWithWorld()
		{
			_loadMapCompleted = true;
		}
	}
	public abstract class TestActionBase
	{
		protected float ElapsedTime;

		public string Description { get; set; } = "";

		protected float Timeout { get; set; } = 10f;

		public abstract TestState Update(float deltaTime);
	}
}
namespace WukongMp.Api.Tests.TestActionSequences
{
	public class AllGameplayTestsSequence : TestActionSequenceBase
	{
		private List<Type> _allAvailableTestTypes = new List<Type>();

		public AllGameplayTestsSequence(ILogger logger)
			: base(logger)
		{
			EnqueueTestAction<EnterLevelTestAction>();
			DiscoverAllGameplayTests();
			EnqueueAllGameplayTests();
		}

		private void DiscoverAllGameplayTests()
		{
			Type type = typeof(IGameplayTestAction);
			_allAvailableTestTypes = (from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly s) => s.GetTypes())
				where type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
				select t).ToList();
		}

		private void EnqueueAllGameplayTests()
		{
			foreach (Type allAvailableTestType in _allAvailableTestTypes)
			{
				EnueueTestAction(allAvailableTestType);
			}
		}
	}
	public class ReconnectTestsSequence : TestActionSequenceBase
	{
		public ReconnectTestsSequence(ILogger logger)
			: base(logger)
		{
			EnqueueTestAction<EnterLevelTestAction>();
			EnqueueTestAction<ReconnectTestAction>(2);
			EnqueueTestAction<BackToMainManuTestAction>();
		}
	}
	public class TestActionSequenceBase
	{
		private readonly Queue<TestActionBase> _testsToRun = new Queue<TestActionBase>();

		private readonly ILogger _logger;

		public TestActionSequenceBase(ILogger logger)
		{
			_logger = logger;
			base..ctor();
		}

		public bool HasQueuedTests()
		{
			return _testsToRun.Count > 0;
		}

		public TestActionBase GetNextQueuedTestAction()
		{
			return _testsToRun.Dequeue();
		}

		public void Clear()
		{
			_testsToRun.Clear();
		}

		protected void EnqueueTestAction<TestActionType>(int repetitions = 1) where TestActionType : TestActionBase
		{
			EnueueTestAction(typeof(TestActionType), repetitions);
		}

		protected void EnueueTestAction(Type testType, int repetitions = 1)
		{
			if (Activator.CreateInstance(testType) is TestActionBase testAction)
			{
				for (int i = 0; i < repetitions; i++)
				{
					EnueueTestActionInstance(testAction);
				}
			}
			else
			{
				_logger.LogError("Failed instantiating test: {TestName}", testType.Name);
			}
		}

		protected void EnueueTestActionInstance(TestActionBase testAction)
		{
			_testsToRun.Enqueue(testAction);
			_logger.LogDebug("Enqueued test: {TestName}", testAction.GetType().Name);
		}
	}
}
namespace WukongMp.Api.State
{
	public class WukongPawnState(Store world, ClientWukongArchetypeRegistration wukongArchetype, ClientNetworkedEntityState netEntity)
	{
		public Entity CreateNetworkedMonster(LocalTamerComponent localTamer, TamerComponent tamer, TeamComponent team)
		{
			var (result, networkId) = netEntity.CreateNetworkedAreaEntity(wukongArchetype.MonsterArchetype, delegate(EntityBuilder b)
			{
				b.Add(in localTamer);
				b.Add(in tamer);
				b.Add(in team);
			});
			Logging.LogDebug("Creating local networked monster with {NetId}", networkId);
			return result;
		}

		public bool IsTamerEntity(Entity entity)
		{
			return entity.HasComponent<TamerComponent>();
		}

		public bool IsMainCharacterEntity(Entity entity)
		{
			return entity.HasComponent<MainCharacterComponent>();
		}

		public bool TryGetTamerEntity(Entity entity, [NotNullWhen(true)] out TamerEntity? tamerEntity)
		{
			tamerEntity = null;
			if (!IsTamerEntity(entity))
			{
				return false;
			}
			tamerEntity = new TamerEntity(entity);
			return true;
		}

		public bool TryGetMainCharacterEntity(Entity entity, [NotNullWhen(true)] out MainCharacterEntity? mainCharacterEntity)
		{
			mainCharacterEntity = null;
			if (!IsMainCharacterEntity(entity))
			{
				return false;
			}
			mainCharacterEntity = new MainCharacterEntity(entity);
			return true;
		}

		public BGUCharacterCS? GetPawnByNetworkId(NetworkId netId)
		{
			if (!netEntity.TryGetEntityByNetworkId(netId, out var entity))
			{
				return null;
			}
			if (entity.Value.TryGetComponent<LocalTamerComponent>(out var result))
			{
				return result.Pawn;
			}
			if (entity.Value.TryGetComponent<LocalMainCharacterComponent>(out var result2))
			{
				return result2.Pawn;
			}
			return null;
		}

		public TamerEntity? GetEntityByTamerMonster(AActor? actor)
		{
			if ((UObject)(object)actor == (UObject)null)
			{
				return null;
			}
			TamerEntity? result = null;
			world.Query<LocalTamerComponent>().ForEachEntity(delegate(ref LocalTamerComponent localTamerComp, Entity entity)
			{
				if ((UObject)(object)localTamerComp.Pawn == (UObject)(object)actor)
				{
					result = new TamerEntity(entity);
				}
			});
			return result;
		}

		public TamerEntity? GetEntityByTamerGuid(string guid)
		{
			TamerEntity? result = null;
			world.Query<TamerComponent>().ForEachEntity(delegate(ref TamerComponent tamerComp, Entity entity)
			{
				if (tamerComp.Guid == guid)
				{
					result = new TamerEntity(entity);
				}
			});
			return result;
		}

		public TamerEntity? GetEntityByTamer(ABGUTamerBase? owner)
		{
			if ((UObject)(object)owner == (UObject)null)
			{
				return null;
			}
			TamerEntity? result = null;
			world.Query<LocalTamerComponent>().ForEachEntity(delegate(ref LocalTamerComponent localTamerComp, Entity entity)
			{
				if ((UObject)(object)localTamerComp.Tamer == (UObject)(object)owner)
				{
					result = new TamerEntity(entity);
				}
			});
			return result;
		}

		public MainCharacterEntity? GetEntityByPlayerPawn(AActor? owner)
		{
			if ((UObject)(object)owner == (UObject)null)
			{
				return null;
			}
			MainCharacterEntity? result = null;
			world.Query<LocalMainCharacterComponent>().ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, Entity entity)
			{
				if (localMainComp.HasPawn && (UObject)(object)localMainComp.Pawn == (UObject)(object)owner)
				{
					result = new MainCharacterEntity(entity);
				}
			});
			return result;
		}

		public MainCharacterEntity? GetEntityByLastPlayerPawn(AActor? owner)
		{
			if ((UObject)(object)owner == (UObject)null)
			{
				return null;
			}
			MainCharacterEntity? result = null;
			world.Query<LocalMainCharacterComponent>().ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, Entity entity)
			{
				if ((UObject)(object)localMainComp.LastPawn == (UObject)(object)owner)
				{
					result = new MainCharacterEntity(entity);
				}
			});
			return result;
		}

		public NetworkId? GetNetworkIdByActor(AActor? owner)
		{
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				return null;
			}
			MainCharacterEntity? entityByPlayerPawn = GetEntityByPlayerPawn(owner);
			if (entityByPlayerPawn.HasValue)
			{
				return entityByPlayerPawn.Value.GetMeta().NetId;
			}
			TamerEntity? entityByTamerMonster = GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue)
			{
				return entityByTamerMonster.Value.GetMeta().NetId;
			}
			return null;
		}

		public bool TryGetEntityByCharacter(BGUCharacterCS? character, [NotNullWhen(true)] out Entity? entity)
		{
			entity = null;
			if ((UObject)(object)character == (UObject)null)
			{
				return false;
			}
			MainCharacterEntity? entityByPlayerPawn = GetEntityByPlayerPawn((AActor?)(object)character);
			if (entityByPlayerPawn.HasValue)
			{
				entity = entityByPlayerPawn.Value.Entity;
				return true;
			}
			TamerEntity? entityByTamerMonster = GetEntityByTamerMonster((AActor?)(object)character);
			if (entityByTamerMonster.HasValue)
			{
				entity = entityByTamerMonster.Value.Entity;
				return true;
			}
			return false;
		}
	}
	public class WukongPlayerPawnState(Store world, WukongPlayerState playerState, ILogger logger)
	{
		public event Action<MainCharacterEntity, BGUCharacterCS>? OnPlayerPawnSpawned;

		public void AddPlayerPawn(PlayerId playerId)
		{
			logger.LogDebug("SPAWN OTHER MAIN CHARACTER ENTITY: {PlayerId}", playerId);
			MainCharacterEntity? mainCharacterById = playerState.GetMainCharacterById(playerId);
			if (!mainCharacterById.HasValue)
			{
				logger.LogError("Main character for player {PlayerId} not found in player state.", playerId);
				return;
			}
			BGUCharacterCS val = SpawningUtils.SpawnCloneForPlayer(playerState, mainCharacterById.Value);
			if ((UObject)(object)val == (UObject)null)
			{
				logger.LogError("Failed to spawn pawn for player {PlayerId}.", playerId);
			}
			else
			{
				this.OnPlayerPawnSpawned?.Invoke(mainCharacterById.Value, val);
				logger.LogDebug("Spawn successful: {PlayerId}", playerId);
			}
		}

		public void RemovePlayerPawn(PlayerId playerId, BGUCharacterCS? playerPawn, AActor? playerMarker)
		{
			logger.LogDebug("DESPAWN OTHER MAIN CHARACTER ENTITY: {PlayerId}", playerId);
			if (!((UObject?)(object)playerMarker).IsNullOrDestroyed())
			{
				logger.LogDebug("Other main character marker: {Actor}", (playerMarker != null) ? ((UObject)playerMarker).GetName() : null);
				BGU_UnrealWorldUtil.DestroyActor(playerMarker);
			}
			if (!((UObject?)(object)playerPawn).IsNullOrDestroyed())
			{
				logger.LogDebug("Other main character pawn: {Pawn}", (playerPawn != null) ? ((UObject)playerPawn).PathName : null);
				BGU_UnrealWorldUtil.DestroyActor((AActor)(object)playerPawn);
				world.Query<TamerComponent>().Each(new ClearPlayerTamerRefCountJob(playerId));
			}
			else
			{
				logger.LogWarning("Attempted to remove player pawn for {PlayerId} but it was already null.", playerId);
			}
		}
	}
	public class WukongPlayerState
	{
		private readonly ComponentIndex<MainCharacterComponent, PlayerId> _ix;

		private readonly ClientWukongArchetypeRegistration _wukongArchetype;

		private readonly ClientNetworkedEntityState _clientNetEntity;

		private readonly ClientState _state;

		private readonly ILogger _logger;

		public PlayerId? LocalPlayerId => _state.LocalPlayerId;

		public PlayerEntity? LocalPlayerEntity
		{
			get
			{
				Entity? localPlayerEntity = _state.LocalPlayerEntity;
				if (!localPlayerEntity.HasValue)
				{
					return null;
				}
				return new PlayerEntity(localPlayerEntity.Value);
			}
		}

		public MainCharacterEntity? LocalMainCharacter
		{
			get
			{
				if (!_state.LocalPlayerId.HasValue)
				{
					return null;
				}
				Entities entities = _ix[_state.LocalPlayerId.Value];
				switch (entities.Count)
				{
				case 0:
					return null;
				case 1:
					return new MainCharacterEntity(entities[0]);
				default:
					_logger.LogError("Multiple entities found with MainCharacterComponent for local player {PlayerId}. This should not happen.", _state.LocalPlayerId);
					return null;
				}
			}
		}

		public event Action<MainCharacterEntity>? OnMainCharacterEntityInitialized;

		public WukongPlayerState(Store world, ClientWukongArchetypeRegistration wukongArchetype, ClientNetworkedEntityState clientNetEntity, ClientState state, ILogger logger)
		{
			_wukongArchetype = wukongArchetype;
			_clientNetEntity = clientNetEntity;
			_state = state;
			_logger = logger;
			_ix = world.ComponentIndex<MainCharacterComponent, PlayerId>();
		}

		internal void InvokeMainCharacterEntityInitialized(MainCharacterEntity mainCharacterEntity)
		{
			this.OnMainCharacterEntityInitialized?.Invoke(mainCharacterEntity);
		}

		public PlayerEntity? GetPlayerById(PlayerId playerId)
		{
			if (!_state.PlayerEntries.TryGetValue(playerId, out var value))
			{
				return null;
			}
			return new PlayerEntity(value.PlayerEntity);
		}

		public MainCharacterEntity? GetMainCharacterById(PlayerId playerId)
		{
			Entities entities = _ix[playerId];
			switch (entities.Count)
			{
			case 0:
				return null;
			case 1:
				return new MainCharacterEntity(entities[0]);
			default:
				_logger.LogError("Multiple entities found with MainCharacterComponent {PlayerId}. This should not happen.", playerId);
				return null;
			}
		}

		public MainCharacterEntity CreateLocalMainCharacter()
		{
			if (!_state.LocalPlayerId.HasValue)
			{
				throw new InvalidOperationException("Local player ID is not set. Cannot create local main character.");
			}
			MainCharacterEntity? localMainCharacter = LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				return localMainCharacter.Value;
			}
			return new MainCharacterEntity(_clientNetEntity.CreateNetworkedAreaEntity(_wukongArchetype.MainCharacterArchetype, delegate(EntityBuilder b)
			{
				MainCharacterComponent component = new MainCharacterComponent
				{
					PlayerId = _state.LocalPlayerId.Value
				};
				b.Add(in component);
			}).Entity);
		}
	}
	public class WukongAreaState(ClientState state, Store world, ClientOwnershipManager clientOwnershipManager)
	{
		private Entity? _pvpStateEntity;

		public bool InRoom => state.CurrentAreaId.HasValue;

		public bool IsMasterClient
		{
			get
			{
				Entity? currentAreaEntity = state.CurrentAreaEntity;
				if (!currentAreaEntity.HasValue)
				{
					return false;
				}
				AreaScopeComponent component = currentAreaEntity.Value.GetComponent<AreaScopeComponent>();
				PlayerId masterClient = component.MasterClient;
				PlayerId? localPlayerId = state.LocalPlayerId;
				return masterClient == localPlayerId;
			}
		}

		public PlayerId? MasterClientId => CurrentArea?.Scope.MasterClient;

		public AreaEntity? CurrentArea
		{
			get
			{
				Entity? currentAreaEntity = state.CurrentAreaEntity;
				if (!currentAreaEntity.HasValue)
				{
					return null;
				}
				return new AreaEntity(currentAreaEntity.Value);
			}
		}

		public Entity? PvpStateEntity
		{
			get
			{
				if ((!(_pvpStateEntity?.IsNull)) ?? true)
				{
					return _pvpStateEntity;
				}
				return null;
			}
			set
			{
				_pvpStateEntity = value;
			}
		}

		public PvpStateComponent? PvpState
		{
			get
			{
				if (!CurrentArea.HasValue)
				{
					PvpStateEntity = null;
				}
				if (!PvpStateEntity.HasValue && CurrentArea.HasValue)
				{
					PvpStateEntity = world.Query<PvpStateComponent>().HasValue<InScopeComponent, Entity>(CurrentArea.Value.Entity).Entities.FirstOrDefault();
				}
				Entity? pvpStateEntity = PvpStateEntity;
				if (!pvpStateEntity.HasValue)
				{
					return null;
				}
				return pvpStateEntity.GetValueOrDefault().GetComponent<PvpStateComponent>();
			}
		}

		public bool OwnsPvpState
		{
			get
			{
				if (PvpStateEntity.HasValue)
				{
					return clientOwnershipManager.OwnsEntity(PvpStateEntity.Value);
				}
				return false;
			}
		}

		public ref PvpStateComponent OwnedPvpStateRef()
		{
			if (!PvpStateEntity.HasValue)
			{
				throw new InvalidOperationException("No PvP state entity available.");
			}
			if (!clientOwnershipManager.OwnsEntity(PvpStateEntity.Value))
			{
				throw new InvalidOperationException("Client does not own the PvP state entity.");
			}
			return ref PvpStateEntity.Value.GetComponent<PvpStateComponent>();
		}
	}
}
namespace WukongMp.Api.Shim
{
	public class ShimAutoStarter : IDisposable
	{
		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly IClientEcsUpdateLoop _shimEcsLoop;

		private readonly ClientState _clientState;

		private readonly WukongEventBus _eventBus;

		private readonly ILogger _logger;

		private readonly ShimPlaybackRelayClient _playbackClient;

		private readonly ShimRelayRecorder _recorder;

		private readonly IRelayClient _recorderRelayClient;

		private readonly IBlobClient _recorderRelayBlobClient;

		private readonly RelayClientService _recorderRelayService;

		private bool _autoRecordingEnabled;

		private bool _autoPlayingEnabled;

		private Task? _recordingStartedTask;

		public bool ShouldAutoRecord { get; set; }

		public bool ShouldAutoPlay { get; set; }

		public ShimAutoStarter(ClientState clientState, WukongEventBus eventBus, IClientEcsUpdateLoop ecsLoop, IClientEcsUpdateLoop shimEcsLoop, ShimPlaybackRelayClient playbackClient, ShimRelayRecorder recorder, BlobClient recorderRelayBlobClient, RelayClientService recorderRelayService, ILogger logger)
		{
			_clientState = clientState;
			_eventBus = eventBus;
			_ecsLoop = ecsLoop;
			_shimEcsLoop = shimEcsLoop;
			_playbackClient = playbackClient;
			_recorder = recorder;
			_recorderRelayClient = _recorder.AttachedRelayClient;
			_recorderRelayBlobClient = recorderRelayBlobClient;
			_recorderRelayService = recorderRelayService;
			_logger = logger;
			_eventBus.OnBeginLoadGameplayLevel += OnBeginLoadGameplayLevelHandler;
			_eventBus.OnEndPlayGameplayLevel += OnEndPlayGameplayLevelHandler;
			_ecsLoop.OnStarted += OnEcsStartedHandler;
			_ecsLoop.OnStopped += OnEcsStoppedHandler;
			_ecsLoop.OnUpdateLoop += OnEcsUpdateLoopHandler;
			_recorder.OnRecordingStarted += OnRecordingStartedHandler;
			_recorder.OnRecordingStopped += OnRecordingStoppedHandler;
		}

		public void Dispose()
		{
			_recordingStartedTask?.GetAwaiter().GetResult();
			if (_playbackClient.IsPlaying)
			{
				_playbackClient.StopPlaying();
			}
			_recorder.OnRecordingStopped -= OnRecordingStoppedHandler;
			_recorder.OnRecordingStarted -= OnRecordingStartedHandler;
			_ecsLoop.OnUpdateLoop -= OnEcsUpdateLoopHandler;
			_ecsLoop.OnStopped -= OnEcsStoppedHandler;
			_ecsLoop.OnStarted -= OnEcsStartedHandler;
			_eventBus.OnEndPlayGameplayLevel -= OnEndPlayGameplayLevelHandler;
			_eventBus.OnBeginLoadGameplayLevel -= OnBeginLoadGameplayLevelHandler;
		}

		private void OnEcsStartedHandler()
		{
			_shimEcsLoop.Start();
		}

		private void OnEcsStoppedHandler()
		{
			_shimEcsLoop.Stop();
		}

		private void OnEcsUpdateLoopHandler(CommandBufferSynced _)
		{
			_shimEcsLoop.Tick(0f);
		}

		private void OnBeginLoadGameplayLevelHandler()
		{
			if (ShouldAutoPlay)
			{
				_playbackClient.StartPlaying();
				_autoPlayingEnabled = true;
			}
			if (ShouldAutoRecord)
			{
				_recorder.StartRecording();
				_autoRecordingEnabled = true;
			}
		}

		private void OnEndPlayGameplayLevelHandler()
		{
			if (_autoPlayingEnabled)
			{
				_playbackClient.StopPlaying();
				_autoPlayingEnabled = false;
			}
			if (_autoRecordingEnabled)
			{
				_recorder.StopRecording();
				_autoRecordingEnabled = false;
			}
		}

		private void OnRecordingStartedHandler()
		{
			_recordingStartedTask = Task.Run((Func<Task?>)OnRecordingStartedAsync);
		}

		private async Task OnRecordingStartedAsync()
		{
			_logger.LogDebug("Connecting to record");
			_recorderRelayService.Start();
			_recorderRelayClient.RequestConnect();
			_logger.LogDebug("Waiting for establishing connection");
			while (!(await _recorderRelayClient.Scheduler.RunFuncAsync((IRelayClientNetworkThreadContext context) => context.IsConnected)))
			{
				await Task.Delay(100);
			}
			_logger.LogDebug("Entering room");
			AreaId? currentAreaId;
			while (true)
			{
				currentAreaId = _clientState.CurrentAreaId;
				if (currentAreaId.HasValue)
				{
					break;
				}
				await Task.Delay(100);
			}
			_recorderRelayClient.RequestJoinArea(currentAreaId.Value);
			_logger.LogDebug("Requesting saves to record the results for shim");
			WukongSaveRelay recordSaveRelay = new WukongSaveRelay(_recorderRelayBlobClient, _logger);
			BlobInfo blobInfo = await recordSaveRelay.DownloadWorldSaveAsync();
			_logger.LogDebug("World save downloaded: {WorldSave}, size {Size} bytes", blobInfo?.Name, blobInfo?.Content.Length);
			BlobInfo blobInfo2 = await recordSaveRelay.DownloadPlayerSaveAsync();
			_logger.LogDebug("Player save downloaded: {PlayerSave}, size {Size} bytes", blobInfo2?.Name, blobInfo2?.Content.Length);
		}

		private void OnRecordingStoppedHandler()
		{
			_recorderRelayClient.RequestLeaveArea();
			_recorderRelayService.Stop();
		}
	}
	public static class ShimUtils
	{
		private static RelayClient CreateRelayNetworked(DI container, string host, int port, Guid userGuid, bool noDisconnect, string? shimDbPath = null)
		{
			RelayConnectionOptions options = new RelayConnectionOptions
			{
				UserGuid = userGuid
			};
			if (shimDbPath != null)
			{
				ShimSerializer shimSerializer = new ShimSerializer(container.TextSerializer);
				container.Logger.LogInformation("Loading shim database from: {Path}", shimDbPath);
				ShimDatabaseMetadata shimDatabaseMetadata = shimSerializer.LoadDatabaseMetadata(shimDbPath);
				if (shimDatabaseMetadata != null && shimDatabaseMetadata.MaxPlayerId != PlayerId.Invalid)
				{
					options.PlayerIdMode = PlayerIdMode.MinId;
					options.PlayerId = new PlayerId((ushort)(shimDatabaseMetadata.MaxPlayerId.RawValue + 1));
				}
			}
			return new RelayClient(host, port, options, container.LoggerFactory.CreateLogger("Relay Client"), noDisconnect);
		}

		public static void InitRelayPlayShim(DI container, string shimPath)
		{
			ShimSerializer shimSerializer = new ShimSerializer(container.TextSerializer);
			container.Logger.LogInformation("Loading shim recording from: {Path}", shimPath);
			ShimRecording recording = shimSerializer.Load(shimPath);
			container.ShimPlaybackRelayClient.SetRecording(recording);
			container.RelayClient.Attach(container.ShimPlaybackRelayClient);
			container.ShimAuto.ShouldAutoPlay = true;
		}

		public static void InitRelayRecordShim(DI container, string host, int port, Guid userGuid, bool noDisconnect, string shimPath)
		{
			string directoryName = Path.GetDirectoryName(shimPath);
			RelayClient client = CreateRelayNetworked(container, host, port, userGuid, noDisconnect, directoryName);
			AttachRecording(container, host, port, noDisconnect);
			container.RelayClient.Attach(client);
			container.ShimAuto.ShouldAutoRecord = true;
		}

		private static void AttachRecording(DI container, string host, int port, bool noDisconnect)
		{
			Guid userGuid = new Guid("deadbeef-3333-3333-3333-deadbeef0001");
			RelayConnectionOptions options = new RelayConnectionOptions
			{
				UserGuid = userGuid,
				PlayerIdMode = PlayerIdMode.ExactId,
				PlayerId = new PlayerId(255)
			};
			RelayClient client = new RelayClient(host, port, options, container.LoggerFactory.CreateLogger("Recorder Relay"), noDisconnect);
			container.ShimRecorderRelayClient.Attach(client);
		}

		public static void InitRelay(DI container, string host, int port, Guid userGuid, bool noDisconnect)
		{
			RelayClient client = CreateRelayNetworked(container, host, port, userGuid, noDisconnect);
			container.RelayClient.Attach(client);
		}
	}
}
namespace WukongMp.Api.Serialization
{
	public static class SerializationHelpers
	{
		public static void SerializeFVector(NetDataWriter outStream, object obj)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			FVector val = (FVector)obj;
			outStream.Put(((FVector)(ref val)).X);
			outStream.Put(((FVector)(ref val)).Y);
			outStream.Put(((FVector)(ref val)).Z);
		}

		public static object DeserializeFVector(NetDataReader inStream)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			float num = inStream.GetFloat();
			float num2 = inStream.GetFloat();
			float num3 = inStream.GetFloat();
			return (object)new FVector((double)num, (double)num2, (double)num3);
		}

		public static void SerializeFVector2D(NetDataWriter outStream, object obj)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			FVector2D val = (FVector2D)obj;
			outStream.Put(((FVector2D)(ref val)).X);
			outStream.Put(((FVector2D)(ref val)).Y);
		}

		public static object DeserializeFVector2D(NetDataReader inStream)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			float num = inStream.GetFloat();
			float num2 = inStream.GetFloat();
			return (object)new FVector2D((double)num, (double)num2);
		}

		public static void SerializeFRotator(NetDataWriter outStream, object obj)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			FRotator val = (FRotator)obj;
			outStream.Put(((FRotator)(ref val)).Pitch);
			outStream.Put(((FRotator)(ref val)).Yaw);
			outStream.Put(((FRotator)(ref val)).Roll);
		}

		public static object DeserializeFRotator(NetDataReader inStream)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			float num = inStream.GetFloat();
			float num2 = inStream.GetFloat();
			float num3 = inStream.GetFloat();
			return (object)new FRotator((double)num, (double)num2, (double)num3);
		}

		public static void SerializeDamageNumParam(NetDataWriter outStream, object obj)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected I4, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			DamageNumParam val = (DamageNumParam)obj;
			outStream.Put(val.DamageNum);
			outStream.Put((byte)(int)val.DamageType);
			SerializeFVector(outStream, val.RealHitLocation);
			outStream.Put(val.Amplitude);
			outStream.Put((byte)val.AttackerTeamType);
			SerializeFVector(outStream, val.RealHitDir);
		}

		public static object DeserializeDamageNumParam(NetDataReader inStream)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			int num = inStream.GetInt();
			byte num2 = inStream.GetByte();
			FVector val = (FVector)DeserializeFVector(inStream);
			float num3 = inStream.GetFloat();
			EDmgNumUITeamType val2 = (EDmgNumUITeamType)inStream.GetByte();
			FVector val3 = (FVector)DeserializeFVector(inStream);
			return (object)new DamageNumParam((EDamageNumberType)num2, num, num3, val, val3, val2);
		}
	}
	public static class TextSerializationHelpers
	{
		public static void TextSerializeFVector(Utf8JsonWriter writer, FVector vec, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(((FVector)(ref vec)).X);
			writer.WriteNumberValue(((FVector)(ref vec)).Y);
			writer.WriteNumberValue(((FVector)(ref vec)).Z);
			writer.WriteEndArray();
		}

		public static FVector TextDeserializeFVector(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for X component of FVector");
			}
			float single = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for Y component of FVector");
			}
			float single2 = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for Z component of FVector");
			}
			float single3 = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
			{
				throw new JsonException("Expected end of array for FVector");
			}
			return new FVector((double)single, (double)single2, (double)single3);
		}

		public static void TextSerializeFRotator(Utf8JsonWriter writer, FRotator vec, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(((FRotator)(ref vec)).Pitch);
			writer.WriteNumberValue(((FRotator)(ref vec)).Yaw);
			writer.WriteNumberValue(((FRotator)(ref vec)).Roll);
			writer.WriteEndArray();
		}

		public static FRotator TextDeserializeFRotator(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for Pitch component of FRotator");
			}
			float single = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for Yaw component of FRotator");
			}
			float single2 = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number for Roll component of FRotator");
			}
			float single3 = reader.GetSingle();
			if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
			{
				throw new JsonException("Expected end of array for FRotator");
			}
			return new FRotator((double)single, (double)single2, (double)single3);
		}

		public static void TextSerializeDamageNumParam(Utf8JsonWriter writer, DamageNumParam dmg, JsonSerializerOptions options)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteNumber("damageNum", dmg.DamageNum);
			writer.WriteNumber("damageType", (int)dmg.DamageType);
			writer.WritePropertyName("realHitLocation");
			TextSerializeFVector(writer, dmg.RealHitLocation, options);
			writer.WriteNumber("amplitude", dmg.Amplitude);
			writer.WriteNumber("attackerTeamType", (byte)dmg.AttackerTeamType);
			writer.WritePropertyName("realHitDir");
			TextSerializeFVector(writer, dmg.RealHitDir, options);
			writer.WriteEndObject();
		}

		public static DamageNumParam TextDeserializeDamageNumParam(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			EDamageNumberType val = (EDamageNumberType)0;
			FVector val2 = default(FVector);
			float num2 = 0f;
			EDmgNumUITeamType val3 = (EDmgNumUITeamType)0;
			FVector val4 = default(FVector);
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartArray);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "damageNum":
					num = reader.GetInt32();
					break;
				case "damageType":
					val = (EDamageNumberType)reader.GetByte();
					break;
				case "realHitLocation":
					val2 = TextDeserializeFVector(ref reader, options);
					break;
				case "amplitude":
					num2 = reader.GetSingle();
					break;
				case "attackerTeamType":
					val3 = (EDmgNumUITeamType)reader.GetByte();
					break;
				case "realHitDir":
					val4 = TextDeserializeFVector(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new DamageNumParam(val, num, num2, val2, val4, val3);
		}
	}
	public class WukongSerializerRegistration : IRelaySerializerRegistration
	{
		public void Register(RelaySerializer serializer)
		{
			serializer.RegisterType(typeof(DamageNumParam), SerializationHelpers.SerializeDamageNumParam, SerializationHelpers.DeserializeDamageNumParam);
			serializer.RegisterType(typeof(EquipmentState), EquipmentState.SerializeUntyped, EquipmentState.DeserializeUntyped);
			serializer.RegisterType(typeof(FRotator), SerializationHelpers.SerializeFRotator, SerializationHelpers.DeserializeFRotator);
			serializer.RegisterType(typeof(FVector), SerializationHelpers.SerializeFVector, SerializationHelpers.DeserializeFVector);
		}
	}
	public class WukongTextSerializerRegistration : ITextRelaySerializerRegistration
	{
		public void Register(TextRelaySerializer serializer)
		{
			serializer.RegisterPolymorphicType("dataNumParam", TextSerializationHelpers.TextSerializeDamageNumParam, TextSerializationHelpers.TextDeserializeDamageNumParam);
			serializer.RegisterPolymorphicType("equipmentState", EquipmentState.TextSerialize, EquipmentState.TextDeserialize);
			serializer.RegisterPolymorphicType("fRotator", TextSerializationHelpers.TextSerializeFRotator, TextSerializationHelpers.TextDeserializeFRotator);
			serializer.RegisterPolymorphicType("fVector", TextSerializationHelpers.TextSerializeFVector, TextSerializationHelpers.TextDeserializeFVector);
		}
	}
}
namespace WukongMp.Api.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Texts
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("WukongMp.Api.Resources.Texts", typeof(Texts).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		public static string AntiStallWarning => ResourceManager.GetString("AntiStallWarning", resourceCulture);

		public static string BlueTeam => ResourceManager.GetString("BlueTeam", resourceCulture);

		public static string BotName => ResourceManager.GetString("BotName", resourceCulture);

		public static string CameraDownControls => ResourceManager.GetString("CameraDownControls", resourceCulture);

		public static string CameraDownDescription => ResourceManager.GetString("CameraDownDescription", resourceCulture);

		public static string CameraMoveControls => ResourceManager.GetString("CameraMoveControls", resourceCulture);

		public static string CameraMoveDescription => ResourceManager.GetString("CameraMoveDescription", resourceCulture);

		public static string CameraRotateControls => ResourceManager.GetString("CameraRotateControls", resourceCulture);

		public static string CameraRotateDescription => ResourceManager.GetString("CameraRotateDescription", resourceCulture);

		public static string CameraSwitchControls => ResourceManager.GetString("CameraSwitchControls", resourceCulture);

		public static string CameraSwitchDescription => ResourceManager.GetString("CameraSwitchDescription", resourceCulture);

		public static string CameraUpControls => ResourceManager.GetString("CameraUpControls", resourceCulture);

		public static string CameraUpDescription => ResourceManager.GetString("CameraUpDescription", resourceCulture);

		public static string ChatHelperDescription => ResourceManager.GetString("ChatHelperDescription", resourceCulture);

		public static string ChatHelperNoSendDescription => ResourceManager.GetString("ChatHelperNoSendDescription", resourceCulture);

		public static string CheatsAreDisabled => ResourceManager.GetString("CheatsAreDisabled", resourceCulture);

		public static string CheatsDisabled => ResourceManager.GetString("CheatsDisabled", resourceCulture);

		public static string CheatsEnabled => ResourceManager.GetString("CheatsEnabled", resourceCulture);

		public static string CommandHelperDescription => ResourceManager.GetString("CommandHelperDescription", resourceCulture);

		public static string Connected => ResourceManager.GetString("Connected", resourceCulture);

		public static string ConnectionRejectedByServer => ResourceManager.GetString("ConnectionRejectedByServer", resourceCulture);

		public static string CustomSpiritCooldown => ResourceManager.GetString("CustomSpiritCooldown", resourceCulture);

		public static string Disconnected => ResourceManager.GetString("Disconnected", resourceCulture);

		public static string HintCommandsUse => ResourceManager.GetString("HintCommandsUse", resourceCulture);

		public static string InfManaDisabled => ResourceManager.GetString("InfManaDisabled", resourceCulture);

		public static string InfManaEnabled => ResourceManager.GetString("InfManaEnabled", resourceCulture);

		public static string InfSpiritDisabled => ResourceManager.GetString("InfSpiritDisabled", resourceCulture);

		public static string InfSpiritEnabled => ResourceManager.GetString("InfSpiritEnabled", resourceCulture);

		public static string InfTransformDisabled => ResourceManager.GetString("InfTransformDisabled", resourceCulture);

		public static string InfTransformEnabled => ResourceManager.GetString("InfTransformEnabled", resourceCulture);

		public static string InfVesselDisabled => ResourceManager.GetString("InfVesselDisabled", resourceCulture);

		public static string InfVesselEnabled => ResourceManager.GetString("InfVesselEnabled", resourceCulture);

		public static string InMultiplayer => ResourceManager.GetString("InMultiplayer", resourceCulture);

		public static string InstantCooldownDisabled => ResourceManager.GetString("InstantCooldownDisabled", resourceCulture);

		public static string InstantCooldownEnabled => ResourceManager.GetString("InstantCooldownEnabled", resourceCulture);

		public static string InvalidCommand => ResourceManager.GetString("InvalidCommand", resourceCulture);

		public static string InvalidCooldown => ResourceManager.GetString("InvalidCooldown", resourceCulture);

		public static string InvalidUnitName => ResourceManager.GetString("InvalidUnitName", resourceCulture);

		public static string InvalidUnitsCount => ResourceManager.GetString("InvalidUnitsCount", resourceCulture);

		public static string JoinOtherPlayersToProceed => ResourceManager.GetString("JoinOtherPlayersToProceed", resourceCulture);

		public static string MissingPak => ResourceManager.GetString("MissingPak", resourceCulture);

		public static string More => ResourceManager.GetString("More", resourceCulture);

		public static string Ping => ResourceManager.GetString("Ping", resourceCulture);

		public static string PingUnits => ResourceManager.GetString("PingUnits", resourceCulture);

		public static string PlayerCooldown => ResourceManager.GetString("PlayerCooldown", resourceCulture);

		public static string PlayerGaveUp => ResourceManager.GetString("PlayerGaveUp", resourceCulture);

		public static string PlayerIsNotReady => ResourceManager.GetString("PlayerIsNotReady", resourceCulture);

		public static string PlayerIsReady => ResourceManager.GetString("PlayerIsReady", resourceCulture);

		public static string PlayerJoined => ResourceManager.GetString("PlayerJoined", resourceCulture);

		public static string PlayerKilledPlayer => ResourceManager.GetString("PlayerKilledPlayer", resourceCulture);

		public static string PlayerLeft => ResourceManager.GetString("PlayerLeft", resourceCulture);

		public static string PlayerRequestedRebirth => ResourceManager.GetString("PlayerRequestedRebirth", resourceCulture);

		public static string PlayerSpawned => ResourceManager.GetString("PlayerSpawned", resourceCulture);

		public static string PressToBeNotReady => ResourceManager.GetString("PressToBeNotReady", resourceCulture);

		public static string PressToBeReady => ResourceManager.GetString("PressToBeReady", resourceCulture);

		public static string PressToCancelMatch => ResourceManager.GetString("PressToCancelMatch", resourceCulture);

		public static string PressToPlayWithBots => ResourceManager.GetString("PressToPlayWithBots", resourceCulture);

		public static string PressToSwitchTeam => ResourceManager.GetString("PressToSwitchTeam", resourceCulture);

		public static string Ready => ResourceManager.GetString("Ready", resourceCulture);

		public static string RedTeam => ResourceManager.GetString("RedTeam", resourceCulture);

		public static string RoundCount => ResourceManager.GetString("RoundCount", resourceCulture);

		public static string RoundDraw => ResourceManager.GetString("RoundDraw", resourceCulture);

		public static string RoundEndedWinner => ResourceManager.GetString("RoundEndedWinner", resourceCulture);

		public static string SeverePacketLossDetected => ResourceManager.GetString("SeverePacketLossDetected", resourceCulture);

		public static string SoftlockDetected => ResourceManager.GetString("SoftlockDetected", resourceCulture);

		public static string Spectators => ResourceManager.GetString("Spectators", resourceCulture);

		public static string StallingMessage => ResourceManager.GetString("StallingMessage", resourceCulture);

		public static string StartingGame => ResourceManager.GetString("StartingGame", resourceCulture);

		public static string TournamentDraw => ResourceManager.GetString("TournamentDraw", resourceCulture);

		public static string TournamentEndedWinner => ResourceManager.GetString("TournamentEndedWinner", resourceCulture);

		public static string WaitForEnd => ResourceManager.GetString("WaitForEnd", resourceCulture);

		public static string WaitForOtherPlayers => ResourceManager.GetString("WaitForOtherPlayers", resourceCulture);

		public static string WaitForOtherPlayersCount => ResourceManager.GetString("WaitForOtherPlayersCount", resourceCulture);

		public static string YouAreReady => ResourceManager.GetString("YouAreReady", resourceCulture);

		internal Texts()
		{
		}
	}
}
namespace WukongMp.Api.Patches
{
	[HarmonyPatch(typeof(BGS_AnimationSyncSystem), "OnBeginAnimationSyncPreCheck")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnBeginAnimationSyncPreCheck
	{
		public static bool Prefix(AActor Host)
		{
			BGU_CharacterAI val = (BGU_CharacterAI)(object)((Host is BGU_CharacterAI) ? Host : null);
			if ((UObject)(object)val != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
				if (entityByTamerMonster.HasValue && (UObject)(object)TargetingUtils.GetTarget((BGUCharacterCS?)(object)val) != (UObject)null)
				{
					return DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity);
				}
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_AreaOverlapComp), "EnableOverlap")]
	[HarmonyPatchCategory("Connected")]
	public class PatchEnableOverlap
	{
		public static bool Prefix(BUS_AreaOverlapComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			string actorGuid = BGU_DataUtil.GetActorGuid(((UActorCompBaseUObj)__instance).GetOwner(), false);
			if (DI.Instance.GameplayConfiguration.IsAreaOverlapDisabled(actorGuid))
			{
				Logging.LogDebug("Preventing enabling area overlap for actor {Actor}", actorGuid);
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_AreaOverlapComp), "OnActorEnter_EnterArea")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnActorEnter_EnterArea
	{
		public static bool Prefix(BUS_AreaOverlapComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			string actorGuid = BGU_DataUtil.GetActorGuid(((UActorCompBaseUObj)__instance).GetOwner(), false);
			if (DI.Instance.GameplayConfiguration.IsAreaOverlapDisabled(actorGuid))
			{
				Logging.LogDebug("Preventing OnActorEnter_EnterArea for actor {Actor}", actorGuid);
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(IntervalTriggerEnableState), "OnTickAction")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchEventActiveTick
	{
		public static bool Prefix()
		{
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_StateMachineCompBase), "JumpToState")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchJumpToState
	{
		public static void Postfix(GSStateBase TargetState)
		{
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				ref MainCharacterComponent state = ref localMainCharacter.Value.GetState();
				bool beguilingChantEligible = TargetState is IntervalTriggerEnableState || (!(TargetState is IntervalTriggerDisableState) && state.BeguilingChantEligible);
				state.BeguilingChantEligible = beguilingChantEligible;
			}
		}
	}
	[HarmonyPatch(typeof(IntervalTriggerEnableState), "OnEnterAction")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchIntervalTriggerEnableStateOnEnterAction
	{
		public static bool Prefix(BUS_StateMachineCompBase InOwner)
		{
			return false;
		}
	}
	[HarmonyPatch(typeof(BUC_AttrContainer), "OnTick")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchAttrs
	{
		public static void Postfix(BUC_AttrContainer __instance)
		{
			if (!DI.Instance.AreaState.InRoom || !DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			if (((UObject?)(object)__instance.Owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			AActor owner = __instance.Owner;
			MainCharacterEntity value = DI.Instance.PlayerState.LocalMainCharacter.Value;
			if ((UObject)(object)owner == (UObject)(object)value.GetLocalState().Pawn)
			{
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(__instance.Owner);
			if (entityByPlayerPawn.HasValue)
			{
				value = entityByPlayerPawn.Value;
				ref MainCharacterComponent state = ref value.GetState();
				byte b = default(byte);
				float num = default(float);
				foreach (KeyValuePair<byte, float> attribute in state.Attributes)
				{
					CompatExtensions.Deconstruct<byte, float>(attribute, ref b, ref num);
					byte b2 = b;
					float num2 = num;
					__instance.SetFloatValue((EBGUAttrFloat)b2, num2);
				}
				if (state.Hp <= -80000f)
				{
					Logging.LogError("Would set HP to {HP} but will not (OOB fall damage)", state.Hp);
				}
				else if (!USharpExtensions.Equals(state.Hp, __instance.GetFloatValue((EBGUAttrFloat)151), 0.1f))
				{
					__instance.SetFloatValue((EBGUAttrFloat)151, state.Hp);
				}
				return;
			}
			WukongPawnState pawnState = DI.Instance.PawnState;
			AActor owner2 = __instance.Owner;
			TamerEntity? entityByTamerMonster = pawnState.GetEntityByTamerMonster((owner2 is BGUCharacterCS) ? owner2 : null);
			if (!entityByTamerMonster.HasValue || DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				return;
			}
			TamerEntity value2 = entityByTamerMonster.Value;
			if (!value2.GetLocalTamer().IsTamerSynced)
			{
				Logging.LogDebug("Monster {Name} is not synced, skipping HP update", ((UObject)__instance.Owner).GetName());
				return;
			}
			value2 = entityByTamerMonster.Value;
			ref HpComponent hp = ref value2.GetHp();
			if (!USharpExtensions.Equals(hp.HpMaxBase, __instance.GetFloatValue((EBGUAttrFloat)101), 0.1f))
			{
				__instance.SetFloatValue((EBGUAttrFloat)101, hp.HpMaxBase);
			}
			if (!USharpExtensions.Equals(hp.Hp, __instance.GetFloatValue((EBGUAttrFloat)151), 0.1f))
			{
				__instance.SetFloatValue((EBGUAttrFloat)151, hp.Hp);
			}
		}
	}
	[HarmonyPatch(typeof(BUS_AttrComp), "SetFloatValue")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchHp
	{
		public static bool Prefix(BUS_AttrComp __instance, EBGUAttrFloat AttrID, float NewValue, BUC_AttrContainer ___AttrContainer)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Invalid comparison between Unknown and I4
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Invalid comparison between Unknown and I4
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Invalid comparison between Unknown and I4
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Invalid comparison between Unknown and I4
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Invalid comparison between Unknown and I4
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			LocalMainCharacterComponent? localMainCharacterComponent = (localMainCharacter.HasValue ? new LocalMainCharacterComponent?(localMainCharacter.GetValueOrDefault().GetLocalState()) : ((LocalMainCharacterComponent?)null));
			bool flag = (UObject)(object)owner == (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter?.GetLocalState().Pawn;
			if ((int)AttrID == 151)
			{
				NetworkId? networkIdByActor = DI.Instance.PawnState.GetNetworkIdByActor(owner);
				if (networkIdByActor.HasValue)
				{
					return DI.Instance.ClientOwnership.OwnsEntity(networkIdByActor.Value);
				}
			}
			if (DI.Instance.AreaState.CurrentArea.HasValue && DI.Instance.AreaState.CurrentArea.Value.Room.CheatsAllowed && localMainCharacterComponent.HasValue && flag)
			{
				if ((int)AttrID == 202 && localMainCharacterComponent.Value.SpiritCooldownEnabled && !localMainCharacterComponent.Value.ShouldSetSpiritCooldown)
				{
					float floatValue = ___AttrContainer.GetFloatValue((EBGUAttrFloat)202);
					float floatValue2 = ___AttrContainer.GetFloatValue((EBGUAttrFloat)17);
					if (USharpExtensions.Equals(NewValue, floatValue2, 0.1f))
					{
						return true;
					}
					if (NewValue > floatValue)
					{
						return false;
					}
				}
				if ((int)AttrID == 201 && localMainCharacterComponent.Value.HasInfiniteVessel)
				{
					float floatValue3 = ___AttrContainer.GetFloatValue((EBGUAttrFloat)201);
					if (NewValue < floatValue3)
					{
						return false;
					}
				}
				if ((int)AttrID == 188 && localMainCharacterComponent.Value.HasInfiniteTransform)
				{
					float floatValue4 = ___AttrContainer.GetFloatValue((EBGUAttrFloat)188);
					if (NewValue < floatValue4)
					{
						return false;
					}
				}
				if ((int)AttrID == 152 && localMainCharacterComponent.Value.HasInfiniteMana)
				{
					float floatValue5 = ___AttrContainer.GetFloatValue((EBGUAttrFloat)152);
					if (NewValue < floatValue5)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static void Postfix(BUS_AttrComp __instance, BUC_AttrContainer ___AttrContainer, EBGUAttrFloat AttrID)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Invalid comparison between Unknown and I4
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Expected I4, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected I4, but got Unknown
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected I4, but got Unknown
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			float floatValue = ___AttrContainer.GetFloatValue(AttrID);
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			MainCharacterEntity value;
			if ((int)AttrID == 151)
			{
				if (localMainCharacter.HasValue)
				{
					value = localMainCharacter.Value;
					if ((UObject)(object)owner == (UObject)(object)value.GetLocalState().Pawn)
					{
						value = localMainCharacter.Value;
						ref MainCharacterComponent state = ref value.GetState();
						if (!USharpExtensions.Equals(state.Hp, floatValue, 0.1f))
						{
							state.Hp = floatValue;
							if (state.Hp > 0f)
							{
								state.IsDead = false;
							}
						}
						goto IL_0143;
					}
				}
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((owner is BGUCharacterCS) ? owner : null);
				if (!entityByTamerMonster.HasValue || !DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					return;
				}
				TamerEntity value2 = entityByTamerMonster.Value;
				if (!value2.GetLocalTamer().IsTamerSynced)
				{
					return;
				}
				value2 = entityByTamerMonster.Value;
				ref HpComponent hp = ref value2.GetHp();
				hp.HpMaxBase = ___AttrContainer.GetFloatValue((EBGUAttrFloat)101);
				hp.Hp = floatValue;
			}
			goto IL_0143;
			IL_0143:
			if (!localMainCharacter.HasValue || !WukongMp.Api.Configuration.Constants.SyncedAttributes.Contains(AttrID))
			{
				return;
			}
			value = localMainCharacter.Value;
			if (!((UObject)(object)owner == (UObject)(object)value.GetLocalState().Pawn))
			{
				return;
			}
			value = localMainCharacter.Value;
			ref MainCharacterComponent state2 = ref value.GetState();
			if (!state2.Attributes.TryGetAttribute((byte)(int)AttrID, out var value3) || !USharpExtensions.Equals(value3, floatValue, 0.1f))
			{
				state2.Attributes.SetAttribute((byte)(int)AttrID, floatValue);
				bool flag = default(bool);
				AttrCalcGroup<EBGUAttrFloat> calc = AttrMgr<EBGUAttrFloat, float>.getInstance().GetCalc(AttrID, ref flag);
				if (flag)
				{
					float floatValue2 = ___AttrContainer.GetFloatValue(calc.finalVal);
					state2.Attributes.SetAttribute((byte)(int)calc.finalVal, floatValue2);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUC_ABPCharacterData), "Update_GameThread")]
	[HarmonyPatchCategory("Connected")]
	public class PatchCharacterAnimation
	{
		public static void Postfix(BUC_ABPCharacterData? __instance, AActor Owner, IBUC_ABPHelperData HelperData, float DeltaTime)
		{
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			if (__instance == null)
			{
				Logging.LogError("__instance is null in BUC_ABPCharacterData.Update_GameThread");
				return;
			}
			BGUCharacterCS val = (BGUCharacterCS)(object)((Owner is BGUCharacterCS) ? Owner : null);
			if (val == null)
			{
				return;
			}
			if (((UObject?)(object)Owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			WukongPawnState pawnState = DI.Instance.PawnState;
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			MainCharacterEntity value;
			FVector val2;
			if (localMainCharacter.HasValue)
			{
				value = localMainCharacter.Value;
				if ((UObject)(object)val == (UObject)(object)value.GetLocalState().Pawn)
				{
					value = localMainCharacter.Value;
					ref MainCharacterComponent state = ref value.GetState();
					value = localMainCharacter.Value;
					if (value.GetLocalState().IsWaitingForSequence)
					{
						RestrictPlayerLocation(localMainCharacter.Value, __instance);
					}
					if (state.IsFlying != __instance.IsFlying)
					{
						state.IsFlying = __instance.IsFlying;
					}
					if (state.IsFalling != __instance.IsFalling)
					{
						state.IsFalling = __instance.IsFalling;
					}
					if (state.IsLandingMove != __instance.IsLandingMove)
					{
						state.IsLandingMove = __instance.IsLandingMove;
					}
					val2 = state.Velocity.ToFVector();
					if (!((FVector)(ref val2)).Equals(__instance.Velocity, 0.10000000149011612))
					{
						state.Velocity = __instance.Velocity.ToVector3();
					}
					val2 = state.MoveAcceleration.ToFVector();
					if (!((FVector)(ref val2)).Equals(__instance.MoveAcceleration, 0.10000000149011612))
					{
						state.MoveAcceleration = __instance.MoveAcceleration.ToVector3();
					}
					val2 = state.Location.ToFVector();
					if (!((FVector)(ref val2)).Equals(__instance.ActorLocation, 0.10000000149011612))
					{
						state.Location = __instance.ActorLocation.ToVector3();
					}
					FRotator val3 = state.Rotation.ToFRotator();
					if (!((FRotator)(ref val3)).Equals(__instance.ActorRotation, 0.10000000149011612))
					{
						state.Rotation = __instance.ActorRotation.ToVector3();
					}
					TeleportUtils.CheckForTeleportFinish(localMainCharacter.Value);
					return;
				}
			}
			MainCharacterEntity? entityByPlayerPawn = pawnState.GetEntityByPlayerPawn((AActor?)(object)val);
			if (entityByPlayerPawn.HasValue)
			{
				value = entityByPlayerPawn.Value;
				ref MainCharacterComponent state2 = ref value.GetState();
				BUS_GSEventCollection val4 = BUS_EventCollectionCS.Get((AActor)(object)val);
				__instance.IsFlying = state2.IsFlying;
				__instance.IsFalling = state2.IsFalling;
				__instance.IsLandingMove = state2.IsLandingMove;
				__instance.Velocity = state2.Velocity.ToFVector();
				val2 = __instance.Velocity;
				if (((FVector)(ref val2)).Equals(FVector.ZeroVector, 0.10000000149011612))
				{
					__instance.Velocity = FVector.ZeroVector;
					state2.Velocity = FVector.ZeroVector.ToVector3();
				}
				__instance.MoveAcceleration = state2.MoveAcceleration.ToFVector();
				val2 = __instance.MoveAcceleration;
				if (((FVector)(ref val2)).Equals(FVector.ZeroVector, 0.10000000149011612))
				{
					__instance.MoveAcceleration = FVector.ZeroVector;
					state2.MoveAcceleration = FVector.ZeroVector.ToVector3();
				}
				val2 = state2.Location.ToFVector();
				if (!((FVector)(ref val2)).Equals(__instance.ActorLocation, 0.10000000149011612))
				{
					val4.Evt_InterpolationMove.Invoke(state2.Location.ToFVector(), state2.Rotation.ToFRotator(), 0.05f, true, false, false, true);
				}
				val2 = __instance.RealWorldVelocity;
				if (((FVector)(ref val2)).Equals(FVector.ZeroVector, 0.10000000149011612))
				{
					__instance.Velocity = FVector.ZeroVector;
					state2.Velocity = FVector.ZeroVector.ToVector3();
					__instance.MoveAcceleration = FVector.ZeroVector;
					state2.MoveAcceleration = FVector.ZeroVector.ToVector3();
					__instance.LastVelocity = FVector.ZeroVector;
				}
				TeleportUtils.CheckForTeleportFinish(entityByPlayerPawn.Value);
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
			if (!entityByTamerMonster.HasValue)
			{
				return;
			}
			TamerEntity value2 = entityByTamerMonster.Value;
			ref LocalTamerComponent localTamer = ref value2.GetLocalTamer();
			if (!localTamer.IsTamerSynced || !localTamer.IsTamerValid || (UObject)(object)localTamer.Pawn == (UObject)null)
			{
				return;
			}
			if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				value2 = entityByTamerMonster.Value;
				ref AnimationComponent animation = ref value2.GetAnimation();
				animation.Velocity = __instance.Velocity.ToVector3();
				animation.MoveAcceleration = __instance.MoveAcceleration.ToVector3();
				value2 = entityByTamerMonster.Value;
				ref TransformComponent transform = ref value2.GetTransform();
				transform.Position = __instance.ActorLocation.ToVector3();
				BGU_CharacterAI val5 = (BGU_CharacterAI)(object)((val is BGU_CharacterAI) ? val : null);
				string text = default(string);
				if (val5 != null && ((ABGUCharacter)val5).GetActorGuid(ref text) && text == "UGuid.HYS.JiRuHuo01")
				{
					transform.Rotation = ((USceneComponent)((ACharacter)val5).Mesh).GetSocketRotation(new FName("Head", (EFindName)1)).ToVector3();
				}
				else
				{
					transform.Rotation = __instance.ActorRotation.ToVector3();
				}
				return;
			}
			value2 = entityByTamerMonster.Value;
			ref AnimationComponent animation2 = ref value2.GetAnimation();
			__instance.Velocity = animation2.Velocity.ToFVector();
			__instance.MoveAcceleration = animation2.MoveAcceleration.ToFVector();
			((UMovementComponent)__instance.MovementComp).Velocity = animation2.Velocity.ToFVector();
			BUS_GSEventCollection val6 = BUS_EventCollectionCS.Get((AActor)(object)localTamer.Pawn);
			value2 = entityByTamerMonster.Value;
			ref TransformComponent transform2 = ref value2.GetTransform();
			FVector val7 = transform2.Position.ToFVector();
			FRotator val8 = transform2.Rotation.ToFRotator();
			if (!((FVector)(ref val7)).Equals(__instance.ActorLocation, 0.10000000149011612) || !((FRotator)(ref val8)).Equals(__instance.ActorRotation, 0.10000000149011612))
			{
				val6.Evt_InterpolationMove.Invoke(val7, val8, 0.05f, true, false, false, true);
			}
		}

		private static void RestrictPlayerLocation(MainCharacterEntity mainEntity, BUC_ABPCharacterData characterData)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if (UMathLibrary_CsExtensions.Vector_DistanceSquared(localState.JoiningSequenceLocation, characterData.ActorLocation) > 250000.0)
			{
				FVector joiningSequenceLocation = localState.JoiningSequenceLocation;
				FVector val = characterData.ActorLocation - localState.JoiningSequenceLocation;
				characterData.ActorLocation = joiningSequenceLocation + 500.0 * ((FVector)(ref val)).GetSafeNormal(9.99999993922529E-09);
				BGUCharacterCS? pawn = localState.Pawn;
				if (pawn != null)
				{
					FHitResult val2 = default(FHitResult);
					((AActor)pawn).SetActorLocation(characterData.ActorLocation, false, ref val2, true);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_MovementSystem), "TickForInterpolationMove")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTickForInterpolationMove
	{
		public static void Postfix(BUS_MovementSystem __instance, BUC_MovementData ___MovementData, float DeltaTime, bool bForceUpdate = false)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || !___MovementData.IM_EnableMove)
			{
				return;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
			BGU_CharacterAI val2 = (BGU_CharacterAI)(object)((val is BGU_CharacterAI) ? val : null);
			string text = default(string);
			if (val2 != null && ((ABGUCharacter)val2).GetActorGuid(ref text) && text == "UGuid.HYS.JiRuHuo01")
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
				if (entityByTamerMonster.HasValue)
				{
					FName val3 = default(FName);
					((FName)(ref val3))..ctor("CAMERA_LOCK", (EFindName)1);
					FTransform socketTransform = ((USceneComponent)((ACharacter)val2).Mesh).GetSocketTransform(val3, (ERelativeTransformSpace)2);
					FVector location = ((FTransform)(ref socketTransform)).GetLocation();
					TransformComponent transform = entityByTamerMonster.Value.GetTransform();
					FVector val4 = transform.Position.ToFVector();
					FRotator val5 = transform.Rotation.ToFRotator();
					FRotator val6 = val5;
					FVector val7 = ((FRotator)(ref val5)).RotateVector(location);
					FVector val8 = val4 - val7;
					FHitResult val9 = default(FHitResult);
					((USceneComponent)((ACharacter)val2).Mesh).SetWorldLocationAndRotation(val8, val6, false, ref val9, false);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_UnitStateSystem), "OnUnitSimpleStateSet")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnUnitSimpleStateSet
	{
		public static void Postfix(EBGUSimpleState SimpleState, bool IsRemove, BUS_UnitStateSystem __instance)
		{
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Invalid comparison between Unknown and I4
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Invalid comparison between Unknown and I4
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Invalid comparison between Unknown and I4
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			NetworkId? networkId = null;
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				networkId = entityByTamerMonster.Value.GetMeta().NetId;
			}
			else
			{
				MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
				if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)owner)
				{
					networkId = localMainCharacter.Value.GetMeta().NetId;
				}
			}
			if (networkId.HasValue && (((int)SimpleState != 9 && (int)SimpleState != 77 && (int)SimpleState != 159) || 1 == 0))
			{
				DI.Instance.Rpc.SendUnitSimpleState(new SimpleStateData(networkId.Value, SimpleState, IsRemove));
			}
		}
	}
	[HarmonyPatch(typeof(BUS_UnitStateSystem), "OnUnitStateTrigger")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnUnitStateTrigger
	{
		public static void Postfix(EBUStateTrigger Trigger, float Time, bool NeedForceUpdate, BUS_UnitStateSystem __instance)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom && (int)Trigger != 1)
			{
				WukongPlayerState playerState = DI.Instance.PlayerState;
				AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
					DI.Instance.Rpc.SendUnitStateTrigger(new StateTriggerData(netId, Trigger, Time, NeedForceUpdate));
				}
				if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
				{
					NetworkId netId2 = playerState.LocalMainCharacter.Value.GetMeta().NetId;
					DI.Instance.Rpc.SendUnitStateTrigger(new StateTriggerData(netId2, Trigger, Time, NeedForceUpdate));
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_ABPHelperComp), "OnChangeMotionMatchingState")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnChangeMotionMatchingState
	{
		public static void Postfix(EState_MM MMState, BUS_ABPHelperComp __instance)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom)
			{
				AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
					DI.Instance.Rpc.SendMotionMatchingState(new MotionMatchingStateData(netId, MMState));
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBuffBegin
	{
		[HarmonyTargetMethodHint("b1.BUS_BuffComp", "BuffBegin", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BuffComp:BuffBegin", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance, int BuffID, float Duration)
		{
			if (!DI.Instance.AreaState.InRoom || UnsynchronizedBuffsData.Ids.Contains(BuffID))
			{
				return;
			}
			AActor owner = __instance.GetOwner();
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendAddBuff(new BuffAddData(netId, BuffID, Duration));
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)owner)
			{
				NetworkId netId2 = localMainCharacter.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendAddBuff(new BuffAddData(netId2, BuffID, Duration));
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBuffRemove
	{
		[HarmonyTargetMethodHint("b1.BUS_BuffComp", "BuffRemove", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BuffComp:BuffRemove", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance, int BuffID, EBuffEffectTriggerType RemoveTriggerType, int InLayer, bool WithTriggerRemoveEffect)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || UnsynchronizedBuffsData.Ids.Contains(BuffID))
			{
				return;
			}
			AActor owner = __instance.GetOwner();
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveBuff(new BuffRemoveData(netId, BuffID, RemoveTriggerType, InLayer, WithTriggerRemoveEffect));
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)owner)
			{
				NetworkId netId2 = localMainCharacter.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveBuff(new BuffRemoveData(netId2, BuffID, RemoveTriggerType, InLayer, WithTriggerRemoveEffect));
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBuffRemoveImmediately
	{
		[HarmonyTargetMethodHint("b1.BUS_BuffComp", "BuffRemoveImmediately", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BuffComp:BuffRemoveImmediately", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance, int BuffID, EBuffEffectTriggerType RemoveTriggerType, bool WithTriggerRemoveEffect)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || UnsynchronizedBuffsData.Ids.Contains(BuffID))
			{
				return;
			}
			AActor owner = __instance.GetOwner();
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveBuff(new BuffRemoveData(netId, BuffID, RemoveTriggerType, -1, WithTriggerRemoveEffect));
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)owner)
			{
				NetworkId netId2 = localMainCharacter.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveBuff(new BuffRemoveData(netId2, BuffID, RemoveTriggerType, -1, WithTriggerRemoveEffect));
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBuffAllRemove
	{
		[HarmonyTargetMethodHint("b1.BUS_BuffComp", "BuffAllRemove", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BuffComp:BuffAllRemove", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance, EBuffEffectTriggerType RemoveTriggerType, bool WithTriggerRemoveEffect)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			AActor owner = __instance.GetOwner();
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				NetworkId netId = entityByTamerMonster.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveAllBuffs(new BuffRemoveAllData(netId, RemoveTriggerType, WithTriggerRemoveEffect));
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)owner)
			{
				NetworkId netId2 = localMainCharacter.Value.GetMeta().NetId;
				DI.Instance.Rpc.SendRemoveAllBuffs(new BuffRemoveAllData(netId2, RemoveTriggerType, WithTriggerRemoveEffect));
			}
		}
	}
	[HarmonyPatch(typeof(BGUCharacterCS), "SetTeamIDInCS")]
	[HarmonyPatchCategory("Connected")]
	public class PatchSetTeamIDInCS
	{
		public static void Postfix(BGUCharacterCS __instance, int NewTeamID)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)__instance);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					entityByTamerMonster.Value.SetTeam(new TeamComponent
					{
						TeamId = NewTeamID
					});
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBeAttackedDeadEventSettlementProcess
	{
		[HarmonyTargetMethodHint("b1.BUS_BeAttackedComp.BeAttackedEvent_Dead", "EventSettlementProcess", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method(AccessTools.Inner(typeof(BUS_BeAttackedComp), "BeAttackedEvent_Dead"), "EventSettlementProcess", (Type[])null, (Type[])null);
		}

		public static bool Prefix(BGUCharacterCS ___VictimChr)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)___VictimChr);
			if (!entityByTamerMonster.HasValue)
			{
				return true;
			}
			if (!DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(CharacterAttrDataInitTemplate), "InitDataPreBeginPlay")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchTamerStatResetOnBeginPlay
	{
		public static void Postfix(AActor ___Owner)
		{
			BGU_CharacterAI val = (BGU_CharacterAI)(object)((___Owner is BGU_CharacterAI) ? ___Owner : null);
			if (val == null)
			{
				return;
			}
			ABGUTamerBase tamerOwner = ((ABGUCharacter)val).GetTamerOwner();
			if (!((UObject?)(object)tamerOwner).IsNullOrDestroyed())
			{
				TamerEntity? entityByTamer = DI.Instance.PawnState.GetEntityByTamer(tamerOwner);
				if (entityByTamer.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamer.Value.Entity) && entityByTamer.Value.GetLocalTamer().IsTamerSynced)
				{
					entityByTamer.Value.GetHp().HpMultiplier = 1f;
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUC_BattleStateData), "IsUnitInBattle")]
	[HarmonyPatchCategory("Connected")]
	public class PatchIsUnitInBattle
	{
		public static bool Prefix(BUC_BattleStateData __instance, ref bool __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!__instance.IsPlayerUnit)
			{
				return true;
			}
			GameplayConfiguration gameplayConfiguration = DI.Instance.GameplayConfiguration;
			if (gameplayConfiguration.EnableCustomIsPlayerInBattle)
			{
				__result = gameplayConfiguration.IsPlayerInBattle();
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BIC_GlobalActorData), "GetActorEntity")]
	[HarmonyPatchCategory("Connected")]
	public class PatchGetActorEntity
	{
		public static bool Prefix(BIC_GlobalActorData __instance, ref bool __result, string UnitGuid, out Entity Entity)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			Entity = Entity.Null;
			if (string.IsNullOrEmpty(UnitGuid))
			{
				__result = false;
				return false;
			}
			if (__instance.ActorGuid2Entity.TryGetValue(UnitGuid, out var value))
			{
				int count = value.Count;
				if (count > 1 && DI.Instance.PlayerState.LocalMainCharacter.HasValue && value[0] is BGUPlayerCharacterCS)
				{
					Entity = ECSExtension.ToEntity((AActor)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn);
					if (Entity != Entity.Null)
					{
						__result = true;
						return false;
					}
				}
				if (count > 0)
				{
					for (int num = count - 1; num >= 0; num--)
					{
						Entity = ECSExtension.ToEntity(value[num]);
						if (Entity != Entity.Null)
						{
							__result = true;
							return false;
						}
					}
				}
			}
			__result = false;
			return false;
		}
	}
	[HarmonyPatch(typeof(BGU_AbnormalStateHandlerBase), "PlayDBC_ByType")]
	[HarmonyPatchCategory("Connected")]
	public class PatchPlayDBC_ByType
	{
		public static void Postfix(BGUCharacterCS ___OwnerChr, EAbnormalStateType ___AbnormalType, EAbnromalDispActionType ActionType)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)___OwnerChr);
			if (entityByTamerMonster.HasValue)
			{
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					DI.Instance.Rpc.SendPlayBaneEffect(new PlayBaneEffectData(entityByTamerMonster.Value.GetMeta().NetId, ___AbnormalType, ActionType));
				}
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)___OwnerChr)
			{
				DI.Instance.Rpc.SendPlayBaneEffect(new PlayBaneEffectData(localMainCharacter.Value.GetMeta().NetId, ___AbnormalType, ActionType));
			}
		}
	}
	[HarmonyPatch(typeof(BGU_AbnormalStateHandlerBase), "EndAllDBC")]
	[HarmonyPatchCategory("Connected")]
	public class PatchEndAllDBC
	{
		public static void Postfix(BGUCharacterCS ___OwnerChr, EAbnormalStateType ___AbnormalType)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)___OwnerChr);
			if (entityByTamerMonster.HasValue)
			{
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					DI.Instance.Rpc.SendStopBaneEffect(new StopBaneEffectData(entityByTamerMonster.Value.GetMeta().NetId, ___AbnormalType));
				}
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue && (UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)___OwnerChr)
			{
				DI.Instance.Rpc.SendStopBaneEffect(new StopBaneEffectData(localMainCharacter.Value.GetMeta().NetId, ___AbnormalType));
			}
		}
	}
	[HarmonyPatch(typeof(BGW_ExceptionUIMgr), "HandleUSharpInvokeFunctionExcpetion")]
	[HarmonyPatchCategory("Global")]
	public class ExceptionPatches
	{
		public static void Postfix(Exception e)
		{
			Logging.LogCritical(e, null);
		}
	}
	[HarmonyPatch(typeof(BGW_DebugMgr), "UpdateUserConfigToSentry")]
	[HarmonyPatchCategory("Global")]
	public class SentryPatches
	{
		public static bool Prefix()
		{
			return false;
		}
	}
	[HarmonyPatch(typeof(NativeReflectionCached), "FindFieldInfo")]
	[HarmonyPatchCategory("Global")]
	public class NativeReflectionCachedPatches
	{
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

		public static void Prefix()
		{
			Semaphore.Wait();
		}

		public static void Postfix()
		{
			Semaphore.Release();
		}
	}
	[HarmonyPatch(typeof(BGW_GameLifeTimeMgr), "OnPostLoadMapWithWorld")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnPostLoadMapWithWorld
	{
		public static void Postfix()
		{
			UWorld world = GameUtils.GetWorld();
			if (!((UObject)(object)world == (UObject)null))
			{
				Logging.LogInformation("New level loaded: {LevelName}", UGameplayStatics_CsExtensions.GetCurrentLevelName(world, true));
			}
		}
	}
	[HarmonyPatch(typeof(GSG), "OnEnterLevel")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnEnterLevel
	{
		public static void Postfix()
		{
			UWorld world = GameUtils.GetWorld();
			if (!((UObject)(object)world == (UObject)null))
			{
				Logging.LogInformation("On enter level: {LevelName}", UGameplayStatics_CsExtensions.GetCurrentLevelName(world, true));
				DI.Instance.EventBus.InvokeOnLevelLoaded();
			}
		}
	}
	[HarmonyPatch(typeof(GSG), "OnLevelExit")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnLevelExit
	{
		public static void Postfix()
		{
			UWorld world = GameUtils.GetWorld();
			if (!((UObject)(object)world == (UObject)null))
			{
				Logging.LogInformation("On exit level: {LevelName}", UGameplayStatics_CsExtensions.GetCurrentLevelName(world, true));
				DI.Instance.EventBus.InvokeOnExitLevel();
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Global")]
	public class PatchOnLateBeginPlay
	{
		[HarmonyTargetMethodHint("b1.BUS_MiscInitComp", "LateBeginPlay", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_MiscInitComp:LateBeginPlay", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance)
		{
			if ((UObject)(object)__instance.GetOwner() == (UObject)(object)GameUtils.GetControlledPawn())
			{
				Logging.LogInformation("Local player late begin play");
			}
		}
	}
	[HarmonyPatch(typeof(BUS_DeadComp), "OnEndPlay")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnPlayerEndPlay
	{
		public static void Postfix(BUS_DeadComp __instance)
		{
			if ((UObject)(object)((UActorCompBaseCS)__instance).GetOwner() == (UObject)(object)GameUtils.GetControlledPawn())
			{
				Logging.LogInformation("Local player end play");
			}
		}
	}
	[HarmonyPatch(typeof(BPS_LiftTimeSystem), "OnBeginPlay")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnPlayerControllerBeginPlay
	{
		public static void Postfix(BPS_LiftTimeSystem __instance)
		{
			Logging.LogInformation("Player controller begin play");
			DI.Instance.EventBus.TryInvokeBeginPlayGameplayLevel();
		}
	}
	[HarmonyPatch(typeof(BPS_LiftTimeSystem), "OnEndPlay")]
	[HarmonyPatchCategory("Global")]
	public class PatchOnPlayerControllerEndPlay
	{
		public static void Postfix(BPS_LiftTimeSystem __instance)
		{
			Logging.LogInformation("Player controller end play");
			DI.Instance.EventBus.TryInvokeEndPlayGameplayLevel();
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Global")]
	public class PatchOnLoadingScreenClose
	{
		[HarmonyTargetMethodHint("b1.BGW_LoadingTipsMgr.FLoadingScreenTimeTracker", "OnLoadingScreenClose", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method(AccessTools.Inner(typeof(BGW_LoadingTipsMgr), "FLoadingScreenTimeTracker"), "OnLoadingScreenClose", (Type[])null, (Type[])null);
		}

		public static void Postfix()
		{
			Logging.LogInformation("Loading screen close");
			DI.Instance.EventBus.InvokeLoadingScreenClose();
		}
	}
	[HarmonyPatch(typeof(BGWGameInstanceCS), "ReceiveTick_Implementation")]
	[HarmonyPatchCategory("Global")]
	public static class ReceiveTickPatch
	{
		public static void Prefix(ref int TickGroup)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			if ((int)CustomTickGroupToTickGroupMask(TickGroup) == 0)
			{
				TickGroup = 3;
			}
		}

		public static void Postfix(float DeltaSeconds, int TickGroup)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)CustomTickGroupToTickGroupMask(TickGroup) == 1024)
			{
				RunMontageSync();
				DI.Instance.EcsLoop.Tick(DeltaSeconds);
			}
		}

		private static void RunMontageSync()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				SyncPlayerMontage(localMainCharacter.Value);
				PlayerId? localPlayerId = DI.Instance.State.LocalPlayerId;
				if (localPlayerId.HasValue)
				{
					DI.Instance.World.Query<LocalTamerComponent, MetadataComponent>().Each(new SyncMontageJob(DI.Instance.Rpc, localPlayerId.Value));
				}
			}
		}

		private static void SyncPlayerMontage(MainCharacterEntity mainEntity)
		{
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if ((UObject)(object)localState.Pawn == (UObject)null || (UObject)(object)((ACharacter)localState.Pawn).Mesh == (UObject)null)
			{
				return;
			}
			MontageState montageState = localState.MontageState;
			if ((UObject)(object)montageState.LocalAnimationInstance == (UObject)null)
			{
				montageState.LocalAnimationInstance = ((ACharacter)localState.Pawn).Mesh.GetAnimInstance();
				if ((UObject)(object)montageState.LocalAnimationInstance == (UObject)null)
				{
					return;
				}
			}
			UAnimMontage currentMontage = ((ACharacter)localState.Pawn).GetCurrentMontage();
			if ((UObject)(object)currentMontage != (UObject)null)
			{
				bool flag = (UObject)(object)montageState.LocalMontage != (UObject)(object)currentMontage;
				float num = montageState.LocalAnimationInstance.Montage_GetPosition(currentMontage);
				bool flag2 = num < montageState.LocalMontagePosition && !flag;
				bool flag3 = num - montageState.LocalMontagePosition > 0.5f && !flag;
				if (flag || flag2 || flag3)
				{
					NetworkId netId = mainEntity.GetMeta().NetId;
					DI.Instance.Rpc.SendMontageCallback(netId, currentMontage, num, flag2);
				}
				montageState.LocalMontagePosition = num;
			}
			else if ((UObject)(object)montageState.LocalMontage != (UObject)null)
			{
				NetworkId netId2 = mainEntity.GetMeta().NetId;
				DI.Instance.Rpc.SendMontageCancel(netId2);
			}
			montageState.LocalMontage = currentMontage;
			localState.MontageState = montageState;
		}

		private static BGW_TickGroupMask CustomTickGroupToTickGroupMask(int tickGroup)
		{
			switch (tickGroup)
			{
			case 0:
				return (BGW_TickGroupMask)1024;
			case 1:
				return (BGW_TickGroupMask)0;
			case 2:
				return (BGW_TickGroupMask)8;
			case 3:
				return (BGW_TickGroupMask)0;
			case 4:
				return (BGW_TickGroupMask)32;
			case 5:
				return (BGW_TickGroupMask)2048;
			case 101:
				return (BGW_TickGroupMask)64;
			case 102:
				return (BGW_TickGroupMask)8192;
			case 111:
				return (BGW_TickGroupMask)128;
			case 141:
				return (BGW_TickGroupMask)4096;
			case 151:
				return (BGW_TickGroupMask)256;
			default:
				Logging.LogError("CustomTickGroup_To_BGWTickGroupMask : unknown tickgroup");
				return (BGW_TickGroupMask)0;
			}
		}
	}
	[HarmonyPatch(typeof(BGW_GameArchiveMgr), "MarkSaveSetting")]
	[HarmonyPatchCategory("Global")]
	public class PatchArchiveReadWriterAppendArchive2
	{
		public static bool Prefix(UISettingArchiveData UISettingArchiveData)
		{
			return false;
		}
	}
	[HarmonyPatch(typeof(GSB1UIUtil), "CheckArchiveFull")]
	[HarmonyPatchCategory("Connected")]
	public class PatchCheckArchiveFull
	{
		public static bool Prefix(ref bool __result)
		{
			__result = false;
			return false;
		}
	}
	[HarmonyPatch(typeof(BGW_GameArchiveMgr), "TickSaveArchiveSnapshot")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTickSaveArchiveSnapshot
	{
		public static Exception? Finalizer(Exception? __exception)
		{
			if (__exception != null)
			{
				DI.Instance.Logger.LogError(__exception, "Suppressed crash in TickSaveArchiveSnapshot");
			}
			return null;
		}
	}
	public class HttpPatches
	{
		[HarmonyPatch(typeof(ServicePointManager))]
		[HarmonyPatchCategory("Disabled")]
		public static class ServicePointManagerPatch
		{
			private static bool _connectionInit;

			[HarmonyTargetMethodHint("System.Net.ServicePointManager", "FindServicePoint", new Type[]
			{
				typeof(Uri),
				typeof(IWebProxy)
			})]
			private static MethodBase TargetMethod()
			{
				return AccessTools.Method("System.Net.ServicePointManager:FindServicePoint", new Type[2]
				{
					typeof(Uri),
					typeof(IWebProxy)
				}, (Type[])null);
			}

			public static bool Prefix(ServicePoint __result, Uri address, IWebProxy proxy)
			{
				if (!_connectionInit)
				{
					FieldInfo field = typeof(ServicePointManager).GetField("manager", BindingFlags.Static | BindingFlags.NonPublic);
					if (field.GetValue(null) == null)
					{
						object value = Activator.CreateInstance(field.FieldType, new object[1]);
						field.SetValue(null, value);
					}
					_connectionInit = true;
				}
				return true;
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchComplexSkillDoInteractAction
	{
		[HarmonyTargetMethodHint("b1.BUIAComplexSkill", "DoInteractAction", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUIAComplexSkill:DoInteractAction", (Type[])null, (Type[])null);
		}

		public static void Prefix(int InteractiveActorID, AActor User, AActor InteractiveActor, FUStInteractionMappingDesc Action)
		{
			if (DI.Instance.AreaState.InRoom && Action.ParamsInt.Count > 1 && InteractiveActor is BGUCharacterCS)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(InteractiveActor);
				if (entityByTamerMonster.HasValue)
				{
					ref MetadataComponent meta = ref entityByTamerMonster.Value.GetMeta();
					Logging.LogDebug("Sending skill interact for {Name} with ID {Id}.", ((UObject)InteractiveActor).GetName(), meta.NetId);
					DI.Instance.Rpc.SendTamerSkillInteract(new SkillInteractData(meta.NetId, Action.ParamsInt[1]));
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGW_EffectTemplateList), "GetInteractTypeTemplate")]
	[HarmonyPatchCategory("Connected")]
	public class PatchGetInteractTypeTemplate
	{
		public static bool Prefix(EInteractType InteractType, BUInteractTypeTemplate? __result)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			Logging.LogDebug("GetInteractTypeTemplate called for {Type}", InteractType);
			if (DI.Instance.GameplayConfiguration.IsInteractionAllowed(InteractType))
			{
				return true;
			}
			__result = null;
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_InteractCompImpl), "TickPlayerInteractive")]
	[HarmonyPatchCategory("Connected")]
	public class PatchInterActivePreCheckFocus
	{
		public static void Postfix(BUC_InteractData ___InteractData)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom && !DI.Instance.GameplayConfiguration.IsInteractionAllowed(___InteractData.InteractiveUnitCommDesc.InteractType))
			{
				___InteractData.InteractConstraint = (EInteractConstraint)9;
				___InteractData.InteractUIState = (EInteractUIState)1;
			}
		}
	}
	[HarmonyPatch(typeof(UGameplayStatics), "OpenLevel")]
	[HarmonyPatchCategory("Global")]
	public static class PatchOpenLevel
	{
		public unsafe static void Postfix(UObject WorldContextObject, FName LevelName, bool bAbsolute = true)
		{
			Logging.LogDebug("OpenLevel called with LevelName {LevelName}, bAbsolute {bAbsolute}", ((object)(*(FName*)(&LevelName))/*cast due to .constrained prefix*/).ToString(), bAbsolute);
		}
	}
	[HarmonyPatch(typeof(BPS_PlayerTeleportSystem), "OnPlayerTeleportTo")]
	[HarmonyPatchCategory("Global")]
	public static class PatchOnPlayerTeleportTo
	{
		public static void Postfix(ETeleportTypeV2 TeleportType, ValueType? UserData, EPlayerTeleportReason Reason)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Invalid comparison between Unknown and I4
			Logging.LogDebug("OnPlayerTeleportTo called with TeleportType {TeleportType}, UserData {UserData}, Reason {Reason}", TeleportType, UserData?.ToString() ?? "Empty", Reason);
			bool flag = (((int)Reason == 2 || (int)Reason == 5) ? true : false);
			if (flag && DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				PlayerUtils.DisableSpectator(DI.Instance.PlayerState.LocalMainCharacter.Value);
			}
		}
	}
	[HarmonyPatch(typeof(TaskNodeInstance_ChapterClear), "PlayChapterMovie")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchPlayChapterMovie
	{
		public static bool Prefix(TaskNodeInstance_ChapterClear __instance)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			TaskCustom_ChapterClear val = (TaskCustom_ChapterClear)AccessTools.PropertyGetter(typeof(TaskNodeInstance_ChapterClear), "CustomData").Invoke(__instance, null);
			if (val.ChapterId != GSG.GamePlayer.RoleData.RoleCs.Chapter.CurChapter)
			{
				DI.Instance.Logger.LogWarning("Corrupted save detected: TaskNodeInstance_ChapterClear with ChapterId {ChapterId} does not match player's current chapter {CurrentChapter}. Skipping task node.", val.ChapterId, GSG.GamePlayer.RoleData.RoleCs.Chapter.CurChapter);
				AccessTools.Method(typeof(TaskNodeInstance_ChapterClear), "TriggerFirstOutput", new Type[1] { typeof(bool) }, (Type[])null).Invoke(__instance, new object[1] { true });
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_MFOverlapCompImpl), "OnMagicFieldDead")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnMagicFieldDead
	{
		public static void Postfix(BUS_MFOverlapCompImpl __instance, EBGUBulletDestroyReason Reason)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom)
			{
				string name = ((UObject)((UObject)((UActorCompBaseCS)__instance).GetOwner()).GetClass()).GetName();
				if (name.Contains("BP_szlc_wanglingguan_mf_hq"))
				{
					Logging.LogDebug("OnMagicFieldDead send for {Class}", name);
					DI.Instance.Rpc.SendMagicFieldDead(name, Reason);
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOSpawnAProjectileObj
	{
		[HarmonyTargetMethodHint("b1.BGS_ProjectileManager", "SpawnAProjectileObj", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BGS_ProjectileManager:SpawnAProjectileObj", (Type[])null, (Type[])null);
		}

		public static void Prefix(FGSProjectileSpawnInfo ProjectileSpawnInfo)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || DI.Instance.AreaState.IsMasterClient)
			{
				return;
			}
			AActor spawner = ProjectileSpawnInfo.Spawner;
			ABGUCharacter val = (ABGUCharacter)(object)((spawner is ABGUCharacter) ? spawner : null);
			if (((UObject?)(object)val).IsNullOrDestroyed())
			{
				return;
			}
			FUStProjectileCommDesc projectileCommDesc = BGW_GameDB.GetProjectileCommDesc(ProjectileSpawnInfo.ProjectileID, (AActor)(object)val);
			if (projectileCommDesc == null)
			{
				return;
			}
			string projectileBPTemplatePath = projectileCommDesc.ProjectileBPTemplatePath;
			FVector spawnPosition = ProjectileSpawnInfo.SpawnPosition;
			Logging.LogDebug("SpawnAProjectileObj send for {Projectile} at position {Position}", projectileBPTemplatePath, spawnPosition);
			if (projectileBPTemplatePath.Contains("BP_szlc_wanglingguan_mf_hq"))
			{
				FVector spawnPosition2 = ProjectileSpawnInfo.SpawnPosition;
				Logging.LogDebug("Modifying Supreme Inspector Firewall spawn position from {PrevPosition} to {Position}", spawnPosition2, WukongMp.Api.Configuration.Constants.SupremeInspectorFirewallLocation);
				ProjectileSpawnInfo.SpawnPosition = WukongMp.Api.Configuration.Constants.SupremeInspectorFirewallLocation;
				MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					Logging.LogDebug("Teleporting local player to firewall location");
					MainCharacterEntity value = localMainCharacter.Value;
					FVector supremeInspectorFirewallLocation = WukongMp.Api.Configuration.Constants.SupremeInspectorFirewallLocation;
					BGUCharacterCS? pawn = localMainCharacter.Value.GetLocalState().Pawn;
					PlayerUtils.TeleportLocalPlayer(value, supremeInspectorFirewallLocation, (pawn != null) ? ((AActor)pawn).GetActorRotation() : FRotator.ZeroRotator);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGUFuncLibActorTransformCS), "BGUSetActorLocation", new Type[]
	{
		typeof(AActor),
		typeof(FVector),
		typeof(bool),
		typeof(bool),
		typeof(FHitResult),
		typeof(bool),
		typeof(bool)
	})]
	[HarmonyPatchCategory("Connected")]
	public class PatchBGUSetActorLocationForPhysicsBasedMovement
	{
		public static void Prefix(AActor NeedSetInfoActor, ref bool bTeleport)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				BGU_CharacterAI val = (BGU_CharacterAI)(object)((NeedSetInfoActor is BGU_CharacterAI) ? NeedSetInfoActor : null);
				string text = default(string);
				if (val != null && ((ABGUCharacter)val).GetActorGuid(ref text) && text == "UGuid.HYS.JiRuHuo01")
				{
					bTeleport = true;
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGUFuncLibActorTransformCS), "BGUSetActorRotation")]
	[HarmonyPatchCategory("Connected")]
	public class PatchBGUSetActorRotationForPhysicsBasedMovement
	{
		public static void Prefix(AActor NeedSetInfoActor, ref bool bTeleportPhysics, ref bool bForceUpdate)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				BGU_CharacterAI val = (BGU_CharacterAI)(object)((NeedSetInfoActor is BGU_CharacterAI) ? NeedSetInfoActor : null);
				string text = default(string);
				if (val != null && ((ABGUCharacter)val).GetActorGuid(ref text) && text == "UGuid.HYS.JiRuHuo01")
				{
					bTeleportPhysics = true;
					bForceUpdate = true;
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGU_PhysicsSimulationMoveMode), "OnUpdate")]
	[HarmonyPatchCategory("Connected")]
	public class PatchPhysicsSimulationMoveMode
	{
		public static void Postfix(ACharacter ___OwnerCharacter)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom && !((UObject?)(object)___OwnerCharacter).IsNullOrDestroyed())
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)___OwnerCharacter);
				if (entityByTamerMonster.HasValue)
				{
					entityByTamerMonster.Value.GetTransform().Rotation = ((USceneComponent)___OwnerCharacter.Mesh).GetSocketRotation(new FName("Head", (EFindName)1)).ToVector3();
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnRegisterTamer
	{
		[HarmonyTargetMethodHint("b1.BGS_TamerManagerSystem", "OnRegisterTamer", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BGS_TamerManagerSystem:OnRegisterTamer", (Type[])null, (Type[])null);
		}

		public static void Postfix(FTamerRef InTamer)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				Logging.LogDebug("Tamer {Tamer} registered by game", InTamer.TamerName);
			}
		}
	}
	[HarmonyPatch(typeof(BUTamerActor), "BeginPlayCS_Implementation")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTamerBeginPlayCS_Implementation
	{
		public static void Postfix(BUTamerActor __instance)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			if (DI.Instance.AreaState.InRoom && DI.Instance.EventBus.IsGameplayLevel && DI.Instance.AreaState.IsMasterClient && (int)((ABGUTamerBase)__instance).TamerType != 3 && (int)((ABGUTamerBase)__instance).TamerType != 2)
			{
				string actorGuid = BGU_DataUtil.GetActorGuid((AActor)(object)__instance, false);
				TamerEntity? entityByTamerGuid = DI.Instance.PawnState.GetEntityByTamerGuid(actorGuid);
				if (!entityByTamerGuid.HasValue)
				{
					SpawningUtils.CreateMonsterInEcs(actorGuid, __instance, 2, ((UObject)__instance).PathName);
					return;
				}
				Logging.LogDebug("Monster already exists in ECS: {NetId}, guid: {Guid}", entityByTamerGuid.Value.GetMeta().NetId, entityByTamerGuid.Value.GetTamer().Guid);
			}
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "IncrementalBeginPlayUnit")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTamerLoad
	{
		public static void Postfix(FTamerRef __instance)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom && __instance.IsMonsterValid() && __instance.InstancePtr.IsValid())
			{
				BUTamerActor val = __instance.InstancePtr.Get();
				Logging.LogDebug("Monster {Guid} waking up locally", BGU_DataUtil.GetActorGuid((AActor)(object)val, false));
				string actorGuid = BGU_DataUtil.GetActorGuid((AActor)(object)val.GetMonster(), false);
				TamerEntity? entityByTamer = DI.Instance.PawnState.GetEntityByTamer((ABGUTamerBase?)(object)val);
				if (entityByTamer.HasValue)
				{
					TamerEntity value = entityByTamer.Value;
					ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
					value = entityByTamer.Value;
					MetadataComponent meta = value.GetMeta();
					TamerUtils.MarkMonsterLocallySpawned(ref localTamer, meta);
				}
				else if (!EcsExcludedMonsters.MonsterNames.Any(actorGuid.Contains) && !DI.Instance.GameplayConfiguration.IsTamerNotSynchronized(actorGuid))
				{
					Logging.LogError("Spawned monster is not in the ECS, guid: {Guid}", actorGuid);
				}
			}
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "CanTurnBack2Loaded")]
	[HarmonyPatchCategory("Global")]
	public class PatchCanTurnBack2Loaded
	{
		private static bool Prefix(ref bool __result)
		{
			__result = false;
			return false;
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "TurnBack2Loaded")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTurnBack2Loaded
	{
		private static bool Prefix(FTamerRef __instance)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!__instance.IsMonsterValid() || !__instance.InstancePtr.IsValid())
			{
				return true;
			}
			BUTamerActor val = __instance.InstancePtr.Get();
			string actorGuid = BGU_DataUtil.GetActorGuid((AActor)(object)val, false);
			TamerEntity? entityByTamer = DI.Instance.PawnState.GetEntityByTamer((ABGUTamerBase?)(object)val);
			if (entityByTamer.HasValue)
			{
				TamerEntity value = entityByTamer.Value;
				ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
				value = entityByTamer.Value;
				TamerUtils.MarkMonsterLocallyDespawned(ref localTamer, value.GetMeta());
				localTamer.HasPendingUnload = true;
				value = entityByTamer.Value;
				if (!value.GetTamer().ShouldBeSpawned)
				{
					Logging.LogDebug("Unloading monster {Guid} locally", actorGuid);
					localTamer.IsMonsterActive = false;
					localTamer.HasPendingUnload = false;
					MarkerUtils.DestroyMarkerForCharacter(entityByTamer.Value);
					return true;
				}
				return false;
			}
			if (!DI.Instance.GameplayConfiguration.IsTamerNotSynchronized(actorGuid))
			{
				Logging.LogError("Unloading monster is not in the ECS, guid: {Guid}", actorGuid);
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "DestroyTamer")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTamerUnload
	{
		public static void Prefix(FTamerRef __instance)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			if (DI.Instance.AreaState.InRoom && ((int)__instance.TamerType == 3 || ((int)__instance.TamerType == 2 && DI.Instance.GameplayConfiguration.EnableSpawnedTamers)))
			{
				TamerEntity? entityByTamer = DI.Instance.PawnState.GetEntityByTamer((ABGUTamerBase?)(object)__instance.InstancePtr.Value);
				if (entityByTamer.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamer.Value.Entity))
				{
					entityByTamer.Value.GetLocalTamer().Tamer = null;
					Logging.LogDebug("Deleting tamer entity from ECS: id {Entity} (DestroyTamer)", entityByTamer.Value.GetMeta().NetId);
					DI.Instance.EcsLoop.CommandBuffer.DeleteEntity(entityByTamer.Value.Entity.Id);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_AIComp), "OnAIPerceptionSetting")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnAIPerceptionSetting
	{
		public static bool Prefix(BUS_AIComp __instance, bool bEnable)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			if ((UObject)(object)owner != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					return true;
				}
			}
			return !bEnable;
		}
	}
	[HarmonyPatch(typeof(BUS_AIComp), "OnAIPauseBT")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnAIPauseBT
	{
		public static bool Prefix(BUS_AIComp __instance, bool IsPause)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			if ((UObject)(object)owner != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					return true;
				}
			}
			return IsPause;
		}
	}
	[HarmonyPatch(typeof(BUS_AIComp), "OnEnableCanSetBT")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnEnableCanSetBT
	{
		public static bool Prefix(BUS_AIComp __instance, bool bEnable)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			if ((UObject)(object)owner != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					return true;
				}
			}
			return !bEnable;
		}
	}
	[HarmonyPatch(typeof(BUS_FsmComp), "OnAIPauseFsm")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnAIPauseFsm
	{
		public static bool Prefix(BUS_FsmComp __instance, bool IsPause)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if ((UObject)(object)owner != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					Logging.LogDebug("Setting FSM pause state to {IsPause} for tamer {Tamer}", IsPause, entityByTamerMonster.Value.GetTamer().Guid);
					entityByTamerMonster.Value.GetTamer().HasFsmPaused = IsPause;
					return true;
				}
			}
			return IsPause;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnEnableCanUpdateHatred
	{
		[HarmonyTargetMethodHint("b1.BUS_BattleStateComp", "OnEnableCanUpdateHatred", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BattleStateComp:OnEnableCanUpdateHatred", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UActorCompBaseCS? __instance, bool bEnable)
		{
			if (__instance == null)
			{
				return true;
			}
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = __instance.GetOwner();
			if ((UObject)(object)owner != (UObject)null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					return true;
				}
			}
			return !bEnable;
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "OnReset")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTamerOnReset
	{
		private static bool Prefix(EResetActorReason ResetReason, FTamerRef __instance)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Invalid comparison between Unknown and I4
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			Logging.LogDebug("Tamer on reset called for tamer {Tamer} with reason {Reason}", __instance.TamerName, ResetReason);
			return (int)ResetReason != 6;
		}
	}
	[HarmonyPatch(typeof(BUS_FsmComp), "OnTriggerFsmEvent")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnTriggerFsmEvent
	{
		public unsafe static bool Prefix(FGameplayTag EventTag, BUS_FsmComp __instance)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (EventTag == NormalAIFsmEventTag.LifeTimeGoHome)
			{
				return false;
			}
			if (EventTag == NormalAIFsmEventTag.AIBattleAttack && DI.Instance.GameplayConfiguration.ShouldDisableTamerAttack())
			{
				return false;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (EventTag == NormalAIFsmEventTag.LifeTimeGazeAndSurround)
			{
				bool anyPlayerAlive = false;
				DI.Instance.World.Query<MainCharacterComponent>().ForEachEntity(delegate(ref MainCharacterComponent playerComp, Entity _)
				{
					if (!playerComp.IsDead)
					{
						anyPlayerAlive = true;
					}
				});
				if (anyPlayerAlive)
				{
					return false;
				}
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
			{
				TamerEntity value = entityByTamerMonster.Value;
				ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
				if ((UObject)(object)localTamer.Pawn != (UObject)null && !BGU_CommonUtil.IsInFsmState((AActor)(object)localTamer.Pawn, EventTag))
				{
					value = entityByTamerMonster.Value;
					NetworkId netId = value.GetMeta().NetId;
					DI.Instance.Rpc.SendTriggerFsmState(new FsmStateData(netId, ((object)(*(FName*)(&EventTag.TagName))/*cast due to .constrained prefix*/).ToString()));
				}
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_MovementSystem), "TickForMonster")]
	[HarmonyPatchCategory("Connected")]
	public class PatchMovementTickForMonster
	{
		public static void Postfix(float DeltaTime, bool bStopMove, bool bNeedPauseMoveModeUpdate, BUS_MovementSystem? __instance, BUC_MovementData ___MovementData)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected I4, but got Unknown
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			if ((UObject)(object)__instance == (UObject)null)
			{
				Logging.LogError("__instance is null in BUC_ABPCharacterData.Update_GameThread");
				return;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
			if (val == null)
			{
				return;
			}
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
			if (!entityByTamerMonster.HasValue)
			{
				return;
			}
			TamerEntity value = entityByTamerMonster.Value;
			ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
			if (localTamer.IsTamerValid)
			{
				value = entityByTamerMonster.Value;
				ref MonsterAnimationComponent monsterAnimation = ref value.GetMonsterAnimation();
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					monsterAnimation.MoveAiType = (byte)(int)___MovementData.MoveAIType;
				}
				else
				{
					BUS_EventCollectionCS.Get((AActor)(object)localTamer.Pawn).Evt_SwitchMoveAIType.Invoke((EBGUMoveAIType)monsterAnimation.MoveAiType);
				}
			}
		}
	}
	[HarmonyPatch(typeof(FTamerRef), "AfterMonsterDead")]
	[HarmonyPatchCategory("Connected")]
	public class PatchAfterMonsterDead
	{
		public static void Prefix(FTamerRef? __instance)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || __instance == null || (int)__instance.Phase == 8)
			{
				return;
			}
			BGUCharacterCS val = __instance.MonsterInstancePtr.Get();
			if (!((UObject)(object)val == (UObject)null))
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
				if (entityByTamerMonster.HasValue)
				{
					TamerEntity value = entityByTamerMonster.Value;
					ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
					value = entityByTamerMonster.Value;
					ref MetadataComponent meta = ref value.GetMeta();
					localTamer.IsMonsterActive = false;
					MarkerUtils.DestroyMarkerForCharacter(entityByTamerMonster.Value);
					value = entityByTamerMonster.Value;
					ref LocalTamerComponent localTamer2 = ref value.GetLocalTamer();
					value = entityByTamerMonster.Value;
					TamerUtils.MarkMonsterLocallyDespawned(ref localTamer2, value.GetMeta());
					Logging.LogDebug("Unloading monster locally. NetId: {NetId}, guid {Guid} (MonsterDead)", meta.NetId, BGU_DataUtil.GetActorGuid((AActor)(object)val, false));
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_AIComp), "TriggerWakeupActivated")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTriggerWakeupActivated
	{
		public static void Postfix(BUS_AIComp? __instance)
		{
			if (!DI.Instance.AreaState.InRoom || (UObject)(object)__instance == (UObject)null)
			{
				return;
			}
			AActor owner = ((UActorCompBaseUObj)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				return;
			}
			BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
			if (val != null)
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
				if (entityByTamerMonster.HasValue && entityByTamerMonster.Value.GetLocalTamer().IsTamerValid && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					DI.Instance.Rpc.SendMonsterWakeUp(entityByTamerMonster.Value.GetMeta().NetId);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_DumperTruckTriggerComp), "PatrolTick")]
	[HarmonyPatchCategory("Connected")]
	public class PatchPatrolTick
	{
		private static MethodInfo? _dumperTruckTriggerDataGetter;

		private static MethodInfo? _BeGetter;

		private static MethodInfo? _BeSetter;

		public static bool Prefix(BUS_DumperTruckTriggerComp? __instance)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (__instance == null)
			{
				return true;
			}
			if ((object)_dumperTruckTriggerDataGetter == null)
			{
				_dumperTruckTriggerDataGetter = AccessTools.PropertyGetter(typeof(BUS_DumperTruckTriggerComp), "DumperTruckTriggerData");
			}
			if ((object)_BeGetter == null)
			{
				_BeGetter = AccessTools.PropertyGetter(typeof(BUS_DumperTruckTriggerComp), "BE");
			}
			if ((object)_BeSetter == null)
			{
				_BeSetter = AccessTools.PropertySetter(typeof(BUS_DumperTruckTriggerComp), "BE");
			}
			BUC_DumperTruckTriggerData val = (BUC_DumperTruckTriggerData)_dumperTruckTriggerDataGetter.Invoke(__instance, null);
			ACharacter controlledUnit = val.ControlledUnit;
			if (((UObject?)(object)controlledUnit).IsNullOrDestroyed())
			{
				return true;
			}
			if ((UObject)(BUS_GSEventCollection)_BeGetter.Invoke(__instance, null) == (UObject)null)
			{
				BUS_GSEventCollection val2 = BUS_EventCollectionCS.Get(BGU_DataUtil.GetActorByGuid((UObject)(object)((UActorCompBaseCS)__instance).GetOwner(), val.UnitGuid));
				_BeSetter.Invoke(__instance, new object[1] { val2 });
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)controlledUnit);
			if (entityByTamerMonster.HasValue)
			{
				TamerEntity value = entityByTamerMonster.Value;
				if (!value.GetLocalTamer().IsTamerValid)
				{
					return true;
				}
				value = entityByTamerMonster.Value;
				ref MonsterAnimationComponent monsterAnimation = ref value.GetMonsterAnimation();
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					monsterAnimation.AnimationPlayRate = val.ControlledUnit.Mesh.GetPlayRate();
					return true;
				}
				controlledUnit.Mesh.SetPlayRate(monsterAnimation.AnimationPlayRate);
				float num = Math.Abs(monsterAnimation.AnimationPlayRate);
				if (num > val.DamageAvailableSpeedThreshold)
				{
					__instance.EnableSweepCheck();
					__instance.TriggerBeginEvent();
				}
				else if (num < val.DamageDisableSpeedThreshold)
				{
					__instance.DisableSweepCheck();
					__instance.TriggerEndEvent();
				}
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRequestPlayMovie
	{
		[HarmonyTargetMethodHint("b1.BGS_MovieSystem", "RequestPlayMovie", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BGS_MovieSystem:RequestPlayMovie", (Type[])null, (Type[])null);
		}

		public static bool Prefix(GameStateSystemBase __instance, FPlayMovieRequest Request)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			MethodInfo methodInfo = AccessTools.Method(((object)__instance).GetType(), "OnPlayMovieInstance", (Type[])null, (Type[])null);
			FMovieSceneSequencePlaybackSettings playbackSettings = new FMovieSceneSequencePlaybackSettings
			{
				AutoPlay = false,
				PlayRate = 1f,
				StartTime = 0f,
				RandomStartTime = false,
				RestoreState = false,
				DisableMovementInput = true,
				DisableLookAtInput = ((FPlayMovieRequest)(ref Request)).bDisableLookAtInput,
				HidePlayer = ((FPlayMovieRequest)(ref Request)).bHidePlayer,
				HideHud = ((FPlayMovieRequest)(ref Request)).bHideHud,
				DisableCameraCuts = !((FPlayMovieRequest)(ref Request)).bDisablePlayerControl,
				PauseAtEnd = false
			};
			FLevelSequenceCameraSettings cameraSettings = new FLevelSequenceCameraSettings
			{
				AspectRatioAxisConstraint = (EAspectRatioAxisConstraint)1,
				OverrideAspectRatioAxisConstraint = false
			};
			FMovieGraphPlaySettings val = new FMovieGraphPlaySettings
			{
				PlaybackSettings = playbackSettings,
				CameraSettings = cameraSettings,
				bUsePlayerCamera = !((FPlayMovieRequest)(ref Request)).bDisablePlayerControl,
				bTriggerMonsterGoHome = false
			};
			MovieInstance val2 = MovieInstance.Create((UObject)(object)owner, ((FPlayMovieRequest)(ref Request)).SequenceID, val);
			if ((UObject)(object)val2 == (UObject)null)
			{
				((FPlayMovieRequest)(ref Request)).BeforePlayFinishCallback?.Invoke();
				((FPlayMovieRequest)(ref Request)).MovieFinishCallback?.Invoke();
				return false;
			}
			AActor actorByGuid = BGU_DataUtil.GetActorByGuid((UObject)(object)owner, ((FPlayMovieRequest)(ref Request)).OverlapBoxGuid);
			if ((UObject)(object)actorByGuid != (UObject)null)
			{
				val2.OverlapGuid = ((FPlayMovieRequest)(ref Request)).OverlapBoxGuid;
				TArrayUnsafe<UActorComponent> componentsByTag = actorByGuid.GetComponentsByTag(TSubclassOf<UActorComponent>.op_Implicit(UClass.GetClass<USceneComponent>()), B1GlobalFNames.MatchPointA);
				if (componentsByTag.Count > 0)
				{
					val2.PointAPos = ((USceneComponent)componentsByTag[0]).GetWorldTransform();
				}
				componentsByTag = actorByGuid.GetComponentsByTag(TSubclassOf<UActorComponent>.op_Implicit(UClass.GetClass<USceneComponent>()), B1GlobalFNames.MatchPointB);
				if (componentsByTag.Count > 0)
				{
					val2.PointBPos = ((USceneComponent)componentsByTag[0]).GetWorldTransform();
				}
				val2.MatchingPosType = ((FPlayMovieRequest)(ref Request)).MatchType;
			}
			else
			{
				val2.OverlapGuid = "";
			}
			if (((FPlayMovieRequest)(ref Request)).BeforePlayFinishCallback != null)
			{
				val2.BeforePlayFinishCallBack = (Action)Delegate.Combine(val2.BeforePlayFinishCallBack, ((FPlayMovieRequest)(ref Request)).BeforePlayFinishCallback);
			}
			if (((FPlayMovieRequest)(ref Request)).MovieFinishCallback != null)
			{
				val2.MovieFinishCallBack = (Action)Delegate.Combine(val2.MovieFinishCallBack, ((FPlayMovieRequest)(ref Request)).MovieFinishCallback);
			}
			SetCallbacks(((FPlayMovieRequest)(ref Request)).SequenceID, val2);
			methodInfo?.Invoke(__instance, new object[2]
			{
				((FPlayMovieRequest)(ref Request)).SequenceID,
				val2
			});
			return false;
		}

		private static void SetCallbacks(int SequenceId, MovieInstance Instance)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Logging.LogDebug("Playing movie {Name} with sequenceId {Id}", ((UObject)Instance).GetName(), SequenceId);
			if (Instance.PlaySettings.PlaybackSettings.DisableCameraCuts)
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			MainCharacterEntity value;
			if (playerState.LocalMainCharacter.HasValue)
			{
				value = playerState.LocalMainCharacter.Value;
				value.GetLocalState().IsInSequence = true;
			}
			Logging.LogDebug("Movie with sequenceId {Id} started, hiding all players", SequenceId);
			foreach (PlayerId otherAreaPlayer in DI.Instance.State.OtherAreaPlayers)
			{
				MainCharacterEntity? mainCharacterById = playerState.GetMainCharacterById(otherAreaPlayer);
				if (mainCharacterById.HasValue)
				{
					value = mainCharacterById.Value;
					ref LocalMainCharacterComponent localState = ref value.GetLocalState();
					BGUCharacterCS? pawn = localState.Pawn;
					if (pawn != null)
					{
						((AActor)pawn).SetActorHiddenInGame(true);
					}
					AActor? markerActor = localState.MarkerActor;
					if (markerActor != null)
					{
						markerActor.SetActorHiddenInGame(true);
					}
					localState.ShouldDisableCollision = true;
					PlayerUtils.SetCollisionEnabled(localState.Pawn, enabled: false);
				}
			}
			Instance.MovieFinishCallBack = (Action)Delegate.Combine(Instance.MovieFinishCallBack, (Action)delegate
			{
				AreaEntity? currentArea = DI.Instance.AreaState.CurrentArea;
				if (currentArea.HasValue && !currentArea.Value.GetMovie().FinishedSequences.Contains(SequenceId))
				{
					DI.Instance.ServerRpc.SendMovieFinished(SequenceId, currentArea.Value.Scope.AreaId);
				}
				WukongPlayerState playerState2 = DI.Instance.PlayerState;
				MainCharacterEntity value2;
				if (playerState2.LocalMainCharacter.HasValue)
				{
					value2 = playerState2.LocalMainCharacter.Value;
					value2.GetLocalState().IsInSequence = false;
				}
				Logging.LogDebug("Movie with sequenceId {Id} finished, showing all players", SequenceId);
				foreach (PlayerId otherAreaPlayer2 in DI.Instance.State.OtherAreaPlayers)
				{
					MainCharacterEntity? mainCharacterById2 = playerState2.GetMainCharacterById(otherAreaPlayer2);
					if (mainCharacterById2.HasValue)
					{
						value2 = mainCharacterById2.Value;
						ref LocalMainCharacterComponent localState2 = ref value2.GetLocalState();
						BGUCharacterCS? pawn2 = localState2.Pawn;
						if (pawn2 != null)
						{
							((AActor)pawn2).SetActorHiddenInGame(false);
						}
						AActor? markerActor2 = localState2.MarkerActor;
						if (markerActor2 != null)
						{
							markerActor2.SetActorHiddenInGame(false);
						}
						localState2.ShouldDisableCollision = false;
						DI.Instance.WidgetManager.HideInfoMessage();
					}
				}
			});
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchTickForMovieSystem
	{
		[HarmonyTargetMethodHint("b1.BGS_MovieSystem", "TickForMovieSystem", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BGS_MovieSystem:TickForMovieSystem", (Type[])null, (Type[])null);
		}

		public static bool Prefix(GameStateSystemBase? __instance, float DeltaTime)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			if (__instance == null)
			{
				return true;
			}
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (DI.Instance.GameplayConfiguration.DisableCutscenes)
			{
				return false;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			Type type = ((object)__instance).GetType();
			BGC_MovieData val = (BGC_MovieData)AccessTools.PropertyGetter(type, "MovieData").Invoke(__instance, null);
			BIC_MovieData val2 = (BIC_MovieData)AccessTools.PropertyGetter(type, "GlobalMovieData").Invoke(__instance, null);
			IBGC_AnimationSyncData val3 = (IBGC_AnimationSyncData)AccessTools.PropertyGetter(type, "AnimationSyncData").Invoke(__instance, null);
			MethodInfo methodInfo = AccessTools.Method(type, "RequestPlayMovie", (Type[])null, (Type[])null);
			MethodInfo methodInfo2 = AccessTools.Method(type, "OnFinishTransBack", (Type[])null, (Type[])null);
			MethodInfo methodInfo3 = AccessTools.Method(type, "TickForDefeatSlowTime", (Type[])null, (Type[])null);
			MethodInfo methodInfo4 = AccessTools.Method(type, "OnSkipCurrentCameraMovie", (Type[])null, (Type[])null);
			if (!val.bCanTick)
			{
				return false;
			}
			val.bAllSeqCantSkip = val3.IsPlayerInAnimationSyncing((UObject)(object)((UActorCompBaseCS)__instance).GetOwner());
			if (val2.PlayMovieRequestQueue.Count > 0)
			{
				FPlayMovieRequest playMovieRequest = val2.PlayMovieRequestQueue.Peek();
				MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
				AreaEntity? currentArea = DI.Instance.AreaState.CurrentArea;
				bool flag = currentArea.HasValue && currentArea.Value.GetMovie().StartedSequences.Contains(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID);
				MainCharacterEntity mainCharacterEntity;
				if (CutsceneUtils.CheckAllPlayersWaitingForCutscene(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID) || !((FPlayMovieRequest)(ref playMovieRequest)).bDisablePlayerControl || flag)
				{
					DI.Instance.WidgetManager.HideInfoMessage();
					if (localMainCharacter.HasValue)
					{
						mainCharacterEntity = localMainCharacter.Value;
						ref LocalMainCharacterComponent localState = ref mainCharacterEntity.GetLocalState();
						localState.IsWaitingForSequence = false;
						localState.IsJoiningSequence = false;
					}
					List<int> list = default(List<int>);
					while (val2.PlayMovieRequestQueue.Count > 0)
					{
						FPlayMovieRequest val4 = val2.PlayMovieRequestQueue.Dequeue();
						if (currentArea.HasValue && !currentArea.Value.GetMovie().StartedSequences.Contains(((FPlayMovieRequest)(ref val4)).SequenceID))
						{
							DI.Instance.ServerRpc.SendMovieStarted(((FPlayMovieRequest)(ref val4)).SequenceID, currentArea.Value.Scope.AreaId);
						}
						val.GetPlayingMovieID(ref list);
						if (list == null)
						{
							list = new List<int>();
						}
						if (!list.Contains(((FPlayMovieRequest)(ref val4)).SequenceID))
						{
							methodInfo?.Invoke(__instance, new object[1] { val4 });
						}
					}
				}
				else if (localMainCharacter.HasValue)
				{
					mainCharacterEntity = localMainCharacter.GetValueOrDefault();
					if (!mainCharacterEntity.GetLocalState().IsWaitingForSequence)
					{
						if (WukongMp.Api.Configuration.Constants.SoloPlaySequences.Contains(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID))
						{
							return true;
						}
						mainCharacterEntity = localMainCharacter.Value;
						ref MainCharacterComponent state = ref mainCharacterEntity.GetState();
						mainCharacterEntity = localMainCharacter.Value;
						ref LocalMainCharacterComponent localState2 = ref mainCharacterEntity.GetLocalState();
						DI.Instance.WidgetManager.ShowInfoMessage(Texts.WaitForOtherPlayers);
						state.WaitingSequenceId = ((FPlayMovieRequest)(ref playMovieRequest)).SequenceID;
						localState2.IsWaitingForSequence = true;
						localState2.JoiningSequenceLocation = state.Location.ToFVector();
						Logging.LogDebug("Sending waiting for sequence with sequenceId {Id}", ((FPlayMovieRequest)(ref playMovieRequest)).SequenceID);
						DI.Instance.Rpc.SendWaitingForSequence(new SequenceWaitingData(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID, state.Location.ToFVector()));
						if (WukongMp.Api.Configuration.Constants.InstantTriggerSequences.Contains(((FPlayMovieRequest)(ref playMovieRequest)).SequenceID))
						{
							DI.Instance.Rpc.SendPlayMovieRequest(playMovieRequest);
						}
					}
				}
			}
			foreach (TStrongObjectPtr<MovieInstance> item in val.MovieInstances.Values.ToList())
			{
				MovieInstance obj = item.Get();
				if (obj != null)
				{
					obj.OnTick(DeltaTime);
				}
			}
			if (val.TransBackTimeForPreviewMovie > 1E-08f)
			{
				val.TransBackTimeForPreviewMovie -= DeltaTime;
				if (val.TransBackTimeForPreviewMovie <= 1E-08f)
				{
					val.TransBackTimeForPreviewMovie = -1f;
					methodInfo2?.Invoke(__instance, null);
				}
			}
			methodInfo3?.Invoke(__instance, new object[1] { DeltaTime });
			if (GSGameplayCVar.CVar_AutoSkipMovies.GetValueInGameThread() != 0 && val.IsCanSkip())
			{
				methodInfo4?.Invoke(__instance, null);
			}
			return false;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnSkipCurrentCameraMovie
	{
		[HarmonyTargetMethodHint("b1.BGS_MovieSystem", "OnSkipCurrentCameraMovie", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BGS_MovieSystem:OnSkipCurrentCameraMovie", (Type[])null, (Type[])null);
		}

		public static bool Prefix(GameStateSystemBase __instance)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return true;
			}
			MovieInstance cameraMovieInstance = ((BGC_MovieData)AccessTools.PropertyGetter(((object)__instance).GetType(), "MovieData").Invoke(__instance, null)).CameraMovieInstance;
			int num = ((cameraMovieInstance != null) ? cameraMovieInstance.SequenceId : 0);
			AreaEntity? currentArea = DI.Instance.AreaState.CurrentArea;
			if (currentArea.HasValue && currentArea.Value.GetMovie().StartedSequences.Contains(num) && !currentArea.Value.GetMovie().FinishedSequences.Contains(num))
			{
				Logging.LogDebug("Sending skip movie for sequence with sequenceId {Id}", num);
				DI.Instance.ServerRpc.SendSkipMovie(new SkipMovieData
				{
					SequenceId = num
				});
				return false;
			}
			Logging.LogDebug("Skipping local movie with sequenceId {Id}", num);
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_DispLibUnitMaterialsManageComp), "Internal_AddMaterialInfoForNewPrimComp")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRandomCrashOnMeshAssignedOnTamerReset
	{
		public static Exception? Finalizer()
		{
			return null;
		}
	}
	[HarmonyPatch(typeof(BUS_OSSCollectComp), "OnOSSCollectBattleData_AiUnit")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRandomCrashOnOSSCollectBattleData_AiUnit
	{
		public static Exception? Finalizer(Exception? __exception)
		{
			if (__exception != null)
			{
				DI.Instance.Logger.LogError(__exception, "Suppressed crash in BUS_OSSCollectComp.OnOSSCollectBattleData_AiUnit");
			}
			return null;
		}
	}
	[HarmonyPatch(typeof(BUS_DeadZoneLogicComp), "PlayerCliffFallRollBack")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRandomCrashOnPlayerCliffFallRollBack
	{
		public static Exception? Finalizer(Exception? __exception)
		{
			if (__exception != null)
			{
				DI.Instance.Logger.LogError(__exception, "Suppressed crash in BUS_DeadZoneLogicComp.PlayerCliffFallRollBack");
			}
			return null;
		}
	}
	[HarmonyPatch(typeof(GSG), "OnTopPageChange")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchGSGOnTopPageChange
	{
		public static bool Prefix(GSUIPage? NewValue)
		{
			return NewValue != null;
		}
	}
	[HarmonyPatch(typeof(BUC_ABPBGUCharacterData), "Update_GameThread")]
	[HarmonyPatchCategory("Connected")]
	public class PatchBGUPlayerAnimation
	{
		public static void Postfix(BUC_ABPBGUCharacterData? __instance, AActor Owner, IBUC_ABPCharacterData ChrData, IBUC_SpeedCtrlData SpeedCtrlData, float DeltaTime)
		{
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			if (__instance == null)
			{
				Logging.LogError("__instance is null in BUC_ABPBGUCharacterData.Update_GameThread");
			}
			else
			{
				if (!(Owner is BGUCharacterCS))
				{
					return;
				}
				if (((UObject?)(object)Owner).IsNullOrDestroyed())
				{
					Logging.LogError("Owner is null or destroyed");
					return;
				}
				WukongPlayerState playerState = DI.Instance.PlayerState;
				WukongPawnState pawnState = DI.Instance.PawnState;
				MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
				object obj;
				MainCharacterEntity mainCharacterEntity;
				if (!localMainCharacter.HasValue)
				{
					obj = null;
				}
				else
				{
					mainCharacterEntity = localMainCharacter.GetValueOrDefault();
					obj = mainCharacterEntity.GetLocalState().Pawn;
				}
				if ((UObject)(object)Owner == (UObject)obj)
				{
					mainCharacterEntity = localMainCharacter.Value;
					ref MainCharacterComponent state = ref mainCharacterEntity.GetState();
					if (state.IsStandRotate != __instance.IsStandRotate)
					{
						state.IsStandRotate = __instance.IsStandRotate;
					}
					if (state.IsAttacking != __instance.IsAttacking)
					{
						state.IsAttacking = __instance.IsAttacking;
					}
					FRotator val = state.TurnInplaceTargetRotation.ToFRotator();
					if (!((FRotator)(ref val)).Equals(__instance.TurnInplaceTargetRotation, 0.10000000149011612))
					{
						state.TurnInplaceTargetRotation = __instance.TurnInplaceTargetRotation.ToVector3();
					}
					if (!USharpExtensions.Equals(state.TurnInplaceRemainAngle, __instance.TurnInplaceRemainAngle, 0.1f))
					{
						state.TurnInplaceRemainAngle = __instance.TurnInplaceRemainAngle;
					}
					if (state.OrientRotationToMovement != __instance.bOrientRotationToMovement)
					{
						state.OrientRotationToMovement = __instance.bOrientRotationToMovement;
					}
				}
				else
				{
					localMainCharacter = pawnState.GetEntityByPlayerPawn(Owner);
					if (localMainCharacter.HasValue)
					{
						mainCharacterEntity = localMainCharacter.Value;
						ref MainCharacterComponent state2 = ref mainCharacterEntity.GetState();
						__instance.IsStandRotate = state2.IsStandRotate;
						__instance.IsAttacking = state2.IsAttacking;
						__instance.TurnInplaceTargetRotation = state2.TurnInplaceTargetRotation.ToFRotator();
						__instance.TurnInplaceRemainAngle = state2.TurnInplaceRemainAngle;
						__instance.bOrientRotationToMovement = state2.OrientRotationToMovement;
					}
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUC_ABPPlayerLocomotionData), "Update")]
	[HarmonyPatchCategory("Connected")]
	public class PatchPlayerLocomotion
	{
		public static void Postfix(BUC_ABPPlayerLocomotionData __instance, AActor Owner, IBUC_ABPCommonSettingData CommonData, IBUC_ABPBasicData BasicData, IBUC_ABPCharacterData ChrData, IBUC_ABPBGUCharacterData BGUData, IBUC_ABPCommonLocomotionData LocomotionData, IBUC_ABPSpecialMoveData SpecialMoveData, IBUC_ABPHelperData HelperData, float DeltaTime)
		{
			if (!DI.Instance.AreaState.InRoom || !(Owner is BGUCharacterCS))
			{
				return;
			}
			if (((UObject?)(object)Owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			object obj;
			MainCharacterEntity mainCharacterEntity;
			if (!localMainCharacter.HasValue)
			{
				obj = null;
			}
			else
			{
				mainCharacterEntity = localMainCharacter.GetValueOrDefault();
				obj = mainCharacterEntity.GetLocalState().Pawn;
			}
			if ((UObject)(object)Owner == (UObject)obj)
			{
				mainCharacterEntity = playerState.LocalMainCharacter.Value;
				ref MainCharacterComponent state = ref mainCharacterEntity.GetState();
				if (state.ShouldWaitRotateFinished != __instance.bShouldWaitRotateFinished)
				{
					state.ShouldWaitRotateFinished = __instance.bShouldWaitRotateFinished;
				}
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(Owner);
			if (entityByPlayerPawn.HasValue)
			{
				mainCharacterEntity = entityByPlayerPawn.Value;
				__instance.bShouldWaitRotateFinished = mainCharacterEntity.GetState().ShouldWaitRotateFinished;
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(Owner);
			if (entityByTamerMonster.HasValue && entityByTamerMonster.Value.GetLocalTamer().IsTamerSynced)
			{
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					entityByTamerMonster.Value.GetAnimation().ShouldWaitRotateFinished = __instance.bShouldWaitRotateFinished;
				}
				else
				{
					__instance.bShouldWaitRotateFinished = entityByTamerMonster.Value.GetAnimation().ShouldWaitRotateFinished;
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUC_ABPBasicData), "Update_WorkThread")]
	[HarmonyPatchCategory("Connected")]
	public class PatchBasicData
	{
		public static void Postfix(BUC_ABPBasicData __instance, AActor Owner)
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected I4, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Expected I4, but got Unknown
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			BGUCharacterCS val = (BGUCharacterCS)(object)((Owner is BGUCharacterCS) ? Owner : null);
			if (val == null)
			{
				return;
			}
			if (((UObject?)(object)Owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			object obj;
			MainCharacterEntity mainCharacterEntity;
			if (!localMainCharacter.HasValue)
			{
				obj = null;
			}
			else
			{
				mainCharacterEntity = localMainCharacter.GetValueOrDefault();
				obj = mainCharacterEntity.GetLocalState().Pawn;
			}
			if ((UObject)(object)Owner == (UObject)obj)
			{
				mainCharacterEntity = playerState.LocalMainCharacter.Value;
				ref MainCharacterComponent state = ref mainCharacterEntity.GetState();
				if (state.MoveSpeedLevel != __instance.MoveSpeedLevel.FromGame())
				{
					state.MoveSpeedLevel = __instance.MoveSpeedLevel.FromGame();
				}
				if (state.MoveSpeedState != __instance.MoveSpeedState.FromGame())
				{
					state.MoveSpeedState = __instance.MoveSpeedState.FromGame();
				}
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(Owner);
			if (entityByPlayerPawn.HasValue)
			{
				mainCharacterEntity = entityByPlayerPawn.Value;
				ref MainCharacterComponent state2 = ref mainCharacterEntity.GetState();
				__instance.MoveSpeedLevel = state2.MoveSpeedLevel.ToGame();
				__instance.MoveSpeedState = state2.MoveSpeedState.ToGame();
				return;
			}
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((AActor?)(object)val);
			if (entityByTamerMonster.HasValue)
			{
				ref AnimationComponent animation = ref entityByTamerMonster.Value.GetAnimation();
				if (DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					animation.MoveSpeedLevel = (byte)(int)__instance.MoveSpeedLevel;
					animation.MoveSpeedState = (byte)(int)__instance.MoveSpeedState;
				}
				else
				{
					__instance.MoveSpeedLevel = (EMoveSpeedLevel)animation.MoveSpeedLevel;
					__instance.MoveSpeedState = (EMoveSpeedLevel)animation.MoveSpeedState;
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_EquipComp), "OnChangeEquip")]
	[HarmonyPatchCategory("Connected")]
	public class PatchEqCompUpdate
	{
		public static bool Prefix(BUS_EquipComp __instance, EquipPosition EquipPosition, int EquipID)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			object obj;
			MainCharacterEntity mainCharacterEntity;
			if (!localMainCharacter.HasValue)
			{
				obj = null;
			}
			else
			{
				mainCharacterEntity = localMainCharacter.GetValueOrDefault();
				obj = mainCharacterEntity.GetLocalState().Pawn;
			}
			if ((UObject)(object)owner == (UObject)obj)
			{
				mainCharacterEntity = localMainCharacter.Value;
				ref MainCharacterComponent state = ref mainCharacterEntity.GetState();
				state.Equipment = state.Equipment.WithSetItem(EquipPosition.FromGame(), EquipID);
			}
			if (!((UObject)(object)owner == (UObject)(object)GameUtils.GetControlledPawn()) && !((UObject)owner).GetName().Contains("Preview") && !((UObject)owner).GetName().Contains("Performer"))
			{
				return ((UObject)owner).GetName().Contains("monkeysummon");
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_DeadComp), "OnUnitDead")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnUnitDead
	{
		public static void Prefix(BUS_DeadComp __instance, EDeadReason DeadReason, IBUC_SimpleStateData ___SimpleStateData, IBUC_UnitStateData ___UnitStateData, out bool __state)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			__state = false;
			if (DI.Instance.AreaState.InRoom && (int)DeadReason != 10)
			{
				AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
				if (((UObject?)(object)owner).IsNullOrDestroyed())
				{
					Logging.LogError("Owner is null or destroyed");
				}
				else if (owner is BGUCharacterCS && !___UnitStateData.HasState((EBGUUnitState)6) && !___SimpleStateData.HasSimpleState((EBGUSimpleState)103))
				{
					__state = true;
				}
			}
		}

		public static void Postfix(BUS_DeadComp __instance, bool __state, EDeadReason DeadReason, AActor Attacker, int DmgID = -1, int StiffLevel = -1, bool bIsDotDmg = false, EAbnormalStateType AbnormalType = (EAbnormalStateType)0)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			if (!__state || (int)DeadReason == 10)
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
			if (val == null)
			{
				return;
			}
			if (playerState.LocalMainCharacter.HasValue && (UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter.Value.GetLocalState().Pawn)
			{
				MainCharacterEntity value = playerState.LocalMainCharacter.Value;
				if (!value.GetState().IsTransformed)
				{
					ref LocalMainCharacterComponent localState = ref value.GetLocalState();
					localState.IsDuringDeathAnim = true;
					BGUCharacterCS? pawn = localState.Pawn;
					IBPC_BattleMainInfoData readOnlyData = BGU_DataUtil.GetReadOnlyData<IBPC_BattleMainInfoData, BPC_BattleMainInfoData>((AActor)(object)((pawn != null) ? ((APawn)pawn).GetController() : null));
					if (readOnlyData != null)
					{
						localState.DeadAnimationTime = readOnlyData.PlayerDeathUIDelayTime;
					}
					localState.DeadAnimationTime = 6f;
					NetworkId netId = value.GetMeta().NetId;
					UnitDeadPacket payload = new UnitDeadPacket(netId, DeadReason, DmgID, StiffLevel, bIsDotDmg, AbnormalType);
					value.GetState().IsDead = true;
					DI.Instance.Rpc.SendUnitDead(payload);
					Logging.LogDebug("Player {PlayerId} died, sending UnitDead event", value.GetState().PlayerId);
				}
			}
			else
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					MetadataComponent meta = entityByTamerMonster.Value.GetMeta();
					UnitDeadPacket payload2 = new UnitDeadPacket(meta.NetId, DeadReason, DmgID, StiffLevel, bIsDotDmg, AbnormalType);
					DI.Instance.Rpc.SendUnitDead(payload2);
					Logging.LogDebug("Entity {Entity} died, sending UnitDead event", meta.NetId);
				}
				BGUCharacterCS val2 = (BGUCharacterCS)(object)((Attacker is BGUCharacterCS) ? Attacker : null);
				if (val2 != null && DI.Instance.PawnState.TryGetEntityByCharacter(val, out var entity) && DI.Instance.PawnState.TryGetEntityByCharacter(val2, out var entity2))
				{
					DI.Instance.GameplayEventRouter.RaiseOnUnitDead(entity.Value, entity2.Value);
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnUnitTriggerDead
	{
		[HarmonyTargetMethodHint("b1.BUS_UIControlSystemV2", "OnUnitTriggerDead", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_UIControlSystemV2:OnUnitTriggerDead", (Type[])null, (Type[])null);
		}

		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_PlayerCameraCompImpl), "OnTickWithGroup")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchCameraCompTick
	{
		public static bool Prefix(BUS_PlayerCameraCompImpl __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				return false;
			}
			BGUCharacterCS pawn = localMainCharacter.Value.GetLocalState().Pawn;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			if ((UObject)(object)owner == (UObject)(object)pawn)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_FallingCompl), "SafeFallingTimerTick")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchFallDamage
	{
		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUC_TargetInfoData), "IsSupportMultiLockTarget")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchIsSupportMultiLockTarget
	{
		public static bool Prefix(ref bool __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!DI.Instance.GameplayConfiguration.IsSupportMultiLockEnabled)
			{
				__result = false;
			}
			return DI.Instance.GameplayConfiguration.IsSupportMultiLockEnabled;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchSetTargetToData
	{
		[HarmonyTargetMethodHint("b1.BUS_BattleStateComp", "SetTargetToData", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_BattleStateComp:SetTargetToData", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UnitLockTargetInfo NewTargetInfo, BUC_TargetInfoData ___TargetInfoData, UActorCompBaseCS __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = __instance.GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			UnitLockTargetInfo targetInfo = ___TargetInfoData.GetTargetInfo();
			if ((UObject)(object)((targetInfo != null) ? targetInfo.LockTargetActor : null) == (UObject)(object)NewTargetInfo.LockTargetActor)
			{
				return true;
			}
			NetworkId target = default(NetworkId);
			bool clearTarget = true;
			string text = "null (Clear target)";
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn((NewTargetInfo != null) ? NewTargetInfo.LockTargetActor : null);
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster((NewTargetInfo != null) ? NewTargetInfo.LockTargetActor : null);
			if (NewTargetInfo != null && (UObject)(object)NewTargetInfo.LockTargetActor != (UObject)null && !entityByPlayerPawn.HasValue && !entityByTamerMonster.HasValue)
			{
				return true;
			}
			if (entityByPlayerPawn.HasValue)
			{
				target = entityByPlayerPawn.Value.GetMeta().NetId;
				text = entityByPlayerPawn.Value.GetState().CharacterNickName;
				clearTarget = false;
			}
			else if (entityByTamerMonster.HasValue)
			{
				target = entityByTamerMonster.Value.GetMeta().NetId;
				text = entityByTamerMonster.Value.GetTamer().Guid ?? "Unknown monster";
				clearTarget = false;
			}
			if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				MainCharacterEntity value = playerState.LocalMainCharacter.Value;
				Logging.LogDebug("New target sent for {Subject} as: {Target}", value.GetState().CharacterNickName, text);
				DI.Instance.Rpc.SendSetTarget(new TargetData(value.GetMeta().NetId, target, clearTarget));
				return true;
			}
			TamerEntity? entityByTamerMonster2 = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (entityByTamerMonster2.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster2.Value.Entity))
			{
				Logging.LogDebug("New target sent for monster: {Subject} as: {Target}", entityByTamerMonster2.Value.GetTamer().Guid ?? "Unknown monster", text);
				MetadataComponent meta = entityByTamerMonster2.Value.GetMeta();
				DI.Instance.Rpc.SendSetTarget(new TargetData(meta.NetId, target, clearTarget));
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_PlayerCameraCompImpl), "ApplyCameraControlData")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchApplyCameraControlData
	{
		public static bool Prefix(GSCameraControlData InControlData)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (DI.Instance.GameplayConfiguration.EnableCustomCameraArmLength)
			{
				bool flag = DI.Instance.PlayerState.LocalMainCharacter?.GetState().IsTransformed ?? false;
				InControlData.ArmLength = Math.Max(InControlData.ArmLength, flag ? 1100f : 720f);
				InControlData.ArmTargetOffset = FVector.ZeroVector;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_BeAttackedComp), "IsDamageValid")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchIsDamageValid
	{
		public static bool Prefix(IBUC_SimpleStateData ___VictimSimpleStateData, ref bool __result)
		{
			if (DI.Instance.GameplayConfiguration.IsStrongDamageImmueEnabled && ___VictimSimpleStateData.HasSimpleState((EBGUSimpleState)117))
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_ParkourMoveCompImpl), "CheckStrideDown")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchCheckStrideDown
	{
		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BGW_GameDB), "GetUnitBattleInfoExtendDesc")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchGetUnitBattleInfoExtendDesc
	{
		public static void Postfix(ref FUStUnitBattleInfoExtendDesc? __result)
		{
			if (DI.Instance.AreaState.InRoom && __result != null && __result.DefaultCamID == 0)
			{
				__result.DefaultCamID = 101600;
			}
		}
	}
	[HarmonyPatch(typeof(BUS_PlayerInputActionComp), "OnTriggerInputActionImpl")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnTriggerInputActionImpl
	{
		public static bool Prefix(BUS_PlayerInputActionComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				return true;
			}
			if ((UObject)(object)localMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)((UActorCompBaseCS)__instance).GetOwner())
			{
				return !localMainCharacter.Value.GetPvP().IsSpectator;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_TimeScaleComp), "OnTriggerScaleTime")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnTriggerScaleTime
	{
		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchSetAllUnitCannotDead
	{
		[HarmonyTargetMethodHint("b1.BIS_DeathManager", "SetAllUnitCannotDead", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BIS_DeathManager:SetAllUnitCannotDead", (Type[])null, (Type[])null);
		}

		public static bool Prefix(bool bInCanUnitDead)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return !bInCanUnitDead;
		}
	}
	[HarmonyPatch(typeof(BUS_QuestDynamicObstacleComp), "DisableCollision")]
	[HarmonyPatchCategory("Connected")]
	public class PatchDisableCollision
	{
		public static void Postfix(BUS_QuestDynamicObstacleComp __instance)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
				DI.Instance.ColliderDisableData.PermanentlyDisableCollider(owner);
			}
		}
	}
	[HarmonyPatch(typeof(BUS_TouchWallFeedbackComp), "CheckCanTrigger_HitDynamicObstacleWall")]
	[HarmonyPatchCategory("Connected")]
	public class PatchCheckCanTrigger_HitDynamicObstacleWall
	{
		public static bool Prefix(BUS_TouchWallFeedbackComp __instance, AActor HitActor)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor obj = HitActor;
			BGU_QuestActor val = (BGU_QuestActor)(object)((obj is BGU_QuestActor) ? obj : null);
			if ((UObject)(object)val == (UObject)null)
			{
				return true;
			}
			if ((int)val.QuestActorType != 2)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			BGUPlayerCharacterCS val2 = (BGUPlayerCharacterCS)(object)((owner is BGUPlayerCharacterCS) ? owner : null);
			if ((UObject)(object)val2 == (UObject)null)
			{
				return true;
			}
			if ((UObject)(object)val2 != (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				return true;
			}
			AActor closestBossActor = GetClosestBossActor((UObject)(object)val2, ((AActor)val2).GetActorLocation());
			if ((UObject)(object)closestBossActor == (UObject)null)
			{
				return true;
			}
			FVector actorLocation = closestBossActor.GetActorLocation();
			List<FHitResultSimple> source = default(List<FHitResultSimple>);
			if (UBGUSelectUtil.MultiSphereTraceForObjects((UObject)(object)val2, ((AActor)val2).GetActorLocation(), actorLocation, 10f, new List<EObjectTypeQuery>(1) { (EObjectTypeQuery)14 }, false, ref source) > 0 && source.Any((FHitResultSimple x) => (UObject)(object)x.HitActor == (UObject)(object)HitActor))
			{
				Logging.LogDebug("Hit dynamic obstacle wall is between boss and player, disabling collision temporarily");
				DI.Instance.ColliderDisableData.DisableCollider((AActor)(object)val, 3f);
				return false;
			}
			return true;
		}

		private static AActor? GetClosestBossActor(UObject context, FVector position)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Invalid comparison between Unknown and I4
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Invalid comparison between Unknown and I4
			AActor result = null;
			double num = double.MaxValue;
			BGU_CharacterAI[] allActorsOfClass = UGameplayStatics.GetAllActorsOfClass<BGU_CharacterAI>(context);
			foreach (BGU_CharacterAI val in allActorsOfClass)
			{
				if (((UObject?)(object)val).IsNullOrDestroyed())
				{
					continue;
				}
				FUStUnitBattleInfoExtendDesc unitBattleInfoExtendDesc = BGW_GameDB.GetUnitBattleInfoExtendDesc(((BGUCharacterCS)val).GetFinalBattleInfoExtendID());
				if (unitBattleInfoExtendDesc == null)
				{
					continue;
				}
				bool flag = ((BGUCharacterCS)val).bBossRoomMonster;
				if (!flag)
				{
					EUnitQualityType qualityType = unitBattleInfoExtendDesc.QualityType;
					bool flag2 = qualityType - 7 <= 1;
					flag = flag2;
				}
				if (flag || (int)unitBattleInfoExtendDesc.BloodBarType == 1)
				{
					double num2 = FVector.DistSquared2D(((AActor)val).GetActorLocation(), position);
					if (num2 < num)
					{
						num = num2;
						result = (AActor)(object)val;
					}
				}
			}
			return result;
		}
	}
	[HarmonyPatch(typeof(InteractStepMatchPos), "StepBegin")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnInteractStepBegin
	{
		public static void Prefix(InteractStepMatchPos __instance, InteractContext ___Context)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				APawn controlledPawn = ((AController)___Context.OwnerController).GetControlledPawn();
				MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
				if (localMainCharacter.HasValue && !((UObject)(object)localMainCharacter.Value.GetLocalState().Pawn != (UObject)(object)controlledPawn))
				{
					Logging.LogWarning("InteractStepMatchPos started, disabling collision for all players");
					PlayerUtils.DisableOtherPlayersCollision();
				}
			}
		}
	}
	[HarmonyPatch(typeof(InteractStepBase), "TriggerFinish")]
	[HarmonyPatchCategory("Connected")]
	public class PatchTriggerFinish
	{
		public static void Prefix(InteractStepBase __instance, InteractContext ___Context)
		{
			if (__instance is InteractStepMatchPos && DI.Instance.AreaState.InRoom)
			{
				APawn controlledPawn = ((AController)___Context.OwnerController).GetControlledPawn();
				MainCharacterEntity? localMainCharacter = DI.Instance.PlayerState.LocalMainCharacter;
				if (localMainCharacter.HasValue && !((UObject)(object)localMainCharacter.Value.GetLocalState().Pawn != (UObject)(object)controlledPawn))
				{
					Logging.LogWarning("InteractStepMatchPos finished, enabling collision for all players");
					PlayerUtils.AllowOtherPlayersCollision();
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGS_GameBgmMgr), "OnUIShrineMainActive")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnUIShrineMainActive
	{
		public static void Postfix(bool IsActive)
		{
			if (IsActive)
			{
				Logging.LogWarning("OnUIShrineMainActive is active, disabling collision for all players");
				PlayerUtils.DisableOtherPlayersCollision();
			}
			else
			{
				Logging.LogWarning("OnUIShrineMainActive is not active, enabling collision for all players");
				PlayerUtils.AllowOtherPlayersCollision();
			}
		}
	}
	[HarmonyPatch(typeof(InteractStepMatchPos), "OnTick")]
	[HarmonyPatchCategory("Connected")]
	public class PatchInteractStepMatchPosOnTick
	{
		public static Exception? Finalizer(Exception? __exception)
		{
			if (__exception != null)
			{
				DI.Instance.Logger.LogError(__exception, "Exception in InteractStepMatchPos.OnTick");
			}
			return null;
		}
	}
	[HarmonyPatch(typeof(B1BattleLogicSvc), "RebirthPointRest")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnRebirthPointRest
	{
		public static bool Prefix(InteractStepMatchPos __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			BPC_RebirthPointData readOnlyData = BGU_DataUtil.GetReadOnlyData<BPC_RebirthPointData>((AActor)(object)GameUtils.GetPlayerController());
			DI.Instance.Rpc.SendRestAtShrine(readOnlyData.CurrentBirthPoint.PointID);
			return true;
		}
	}
	[HarmonyPatch(typeof(BUC_ABPMotionMatchingData), "UpdatePlayerMotionMatchingState")]
	[HarmonyPatchCategory("Connected")]
	public class PatchUpdatePlayerMotionMatchingState
	{
		public static bool Prefix(BUC_ABPMotionMatchingData __instance, AActor Owner, IBUC_TargetInfoData ___TargetInfoData, IBUC_UnitStateData ___UnitStateData, IBUC_PlayerCameraData ___CameraData, EMoveSpeedLevel ___MMMoveSpeedState)
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected I4, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Invalid comparison between Unknown and I4
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)Owner == (UObject)null)
			{
				return false;
			}
			ACharacter val = (ACharacter)(object)((Owner is ACharacter) ? Owner : null);
			if ((UObject)(object)val == (UObject)null || !(val is BGUPlayerCharacterCS))
			{
				return false;
			}
			bool flag = false;
			if (___TargetInfoData != null)
			{
				UnitLockTargetInfo targetInfo = ___TargetInfoData.GetTargetInfo();
				if (targetInfo != null && (UObject)(object)targetInfo.LockTargetActor != (UObject)null && (int)targetInfo.LockTargetWayType == 1)
				{
					flag = true;
				}
			}
			if (___UnitStateData != null && ___UnitStateData.HasState((EBGUUnitState)16))
			{
				flag = true;
			}
			if (___CameraData != null && ___CameraData.IsInG4Mode())
			{
				flag = true;
			}
			switch ((int)___MMMoveSpeedState)
			{
			case 0:
				__instance.TargetMMState = (EState_MM)(flag ? 6 : 3);
				break;
			case 1:
				__instance.TargetMMState = (EState_MM)(flag ? 7 : 4);
				break;
			case 2:
				__instance.TargetMMState = (EState_MM)(flag ? 8 : 5);
				break;
			default:
				__instance.TargetMMState = (EState_MM)(flag ? 1 : 2);
				break;
			}
			return false;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnSwitchBulletTarget
	{
		[HarmonyTargetMethodHint("b1.BUS_ProjectileCtrComp", "OnSwitchBulletTarget", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_ProjectileCtrComp:OnSwitchBulletTarget", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UActorCompBaseCS __instance, BGUProjectileBaseActor? ProjectileActor, AActor? InnerTarget, string SocketName = "")
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return true;
			}
			AActor val = ((__instance != null) ? __instance.GetOwner() : null);
			if (((UObject?)(object)val).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			if ((UObject)(object)ProjectileActor == (UObject)null || (UObject)(object)InnerTarget == (UObject)null)
			{
				return true;
			}
			if ((UObject)(object)val == (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn)
			{
				NetworkId target = default(NetworkId);
				if (InnerTarget is BGUPlayerCharacterCS)
				{
					MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(InnerTarget);
					if (!entityByPlayerPawn.HasValue)
					{
						Logging.LogError("Player character entity not found for actor: {ActorName}", ((UObject)InnerTarget).GetName());
						return false;
					}
					target = entityByPlayerPawn.Value.GetMeta().NetId;
				}
				else
				{
					TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(InnerTarget);
					if (entityByTamerMonster.HasValue)
					{
						target = entityByTamerMonster.Value.GetMeta().NetId;
					}
					else
					{
						Logging.LogError("Could not find tamer entity for projectile target");
					}
				}
				UClass val2 = ((UObject)ProjectileActor).GetClass();
				if ((UObject)(object)val2 != (UObject)null)
				{
					Logging.LogDebug("New projectile target sent for {Projectile} (Owner {NickName}) as: {Target}", ((UObject)val2).GetName(), DI.Instance.PlayerState.LocalMainCharacter.Value.GetState().CharacterNickName, ((UObject)InnerTarget).GetName());
					DI.Instance.Rpc.SendProjectileTarget(new ProjectileTargetData(((UObject)val2).GetName(), target, SocketName));
				}
				return true;
			}
			return true;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnSwitchBulletInfoIfNeed
	{
		[HarmonyTargetMethodHint("b1.BUS_ProjectileCtrComp", "SwitchBulletInfoIfNeed", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_ProjectileCtrComp:SwitchBulletInfoIfNeed", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UActorCompBaseCS __instance, BGUProjectileBaseActor? ProjectileActor, int BulletSwitchID, int SwitchIdx)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (!DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return true;
			}
			AActor val = ((__instance != null) ? __instance.GetOwner() : null);
			if (((UObject?)(object)val).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			if ((UObject)(object)ProjectileActor == (UObject)null)
			{
				return true;
			}
			if ((UObject)(object)val == (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn)
			{
				UClass val2 = ((UObject)ProjectileActor).GetClass();
				if ((UObject)(object)val2 != (UObject)null)
				{
					Logging.LogDebug("Switch projectile info sent for {Projectile} (Owner {NickName}) with switch id: {SwitchID}", ((UObject)val2).GetName(), DI.Instance.PlayerState.LocalMainCharacter.Value.GetState().CharacterNickName, BulletSwitchID);
					DI.Instance.Rpc.SendSwitchOneProjectile(new ProjectileSwitchData(((UObject)val2).GetName(), BulletSwitchID, SwitchIdx));
				}
				return true;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_ProjectileLifeComp), "OnProjectileDead")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnProjectileDead
	{
		public static void Postfix(BUS_ProjectileLifeComp __instance, IBUC_MasterData ___MasterData, EBGUBulletDestroyReason Reason)
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom || !DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			AActor masterActor = ___MasterData.GetMasterActor();
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			BGUProjectileBaseActor val = (BGUProjectileBaseActor)(object)((owner is BGUProjectileBaseActor) ? owner : null);
			if ((UObject)(object)val != (UObject)null && (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)masterActor)
			{
				UClass val2 = ((UObject)val).GetClass();
				if ((UObject)(object)val2 != (UObject)null)
				{
					Logging.LogDebug("BUS_ProjectileLifeComp OnProjectileDead send with reason: {Reason}", Reason);
					DI.Instance.Rpc.SendProjectileDead(new ProjectileDeadData(((UObject)val2).GetName(), Reason));
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_ObjActorMovementComp), "OnSetMoveMode")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnSetMoveMode
	{
		public static void Postfix(BUS_ObjActorMovementComp __instance, EBulletOrMagicFieldMoveModeType MoveMode)
		{
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			BGUProjectileBaseActor val = (BGUProjectileBaseActor)(object)((owner is BGUProjectileBaseActor) ? owner : null);
			if ((UObject)(object)val == (UObject)null)
			{
				return;
			}
			IBUC_MasterData readOnlyData = BGU_DataUtil.GetReadOnlyData<IBUC_MasterData, BUC_MasterData>((AActor)(object)val);
			if (readOnlyData == null || !DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			AActor masterActor = readOnlyData.GetMasterActor();
			if ((UObject)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn == (UObject)(object)masterActor)
			{
				UClass val2 = ((UObject)val).GetClass();
				if ((UObject)(object)val2 != (UObject)null)
				{
					Logging.LogDebug("New move mode sent for {Projectile} (Owner {NickName}) as: {MoveMode}", ((UObject)val2).GetName(), DI.Instance.PlayerState.LocalMainCharacter.Value.GetState().CharacterNickName, MoveMode);
					DI.Instance.Rpc.SendProjectileMoveMode(new ProjectileMoveModeData(((UObject)val2).GetName(), MoveMode));
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUEffectBulletSwitchSelf), "ApplyBySkill_Implement")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchApplyBySkill_Implement
	{
		public static bool Prefix(int EffectID, AActor? Caster, AActor? Target)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			BGUBulletBaseCS val = (BGUBulletBaseCS)(object)((Target is BGUBulletBaseCS) ? Target : null);
			if ((UObject)(object)val == (UObject)null)
			{
				return true;
			}
			BUC_MasterData readOnlyData = BGU_DataUtil.GetReadOnlyData<BUC_MasterData>((AActor)(object)val);
			if (readOnlyData == null)
			{
				return true;
			}
			AActor masterActor = readOnlyData.GetMasterActor();
			if (!DI.Instance.PlayerState.LocalMainCharacter.HasValue)
			{
				return true;
			}
			if (masterActor is BGUPlayerCharacterCS && (UObject)(object)masterActor != (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter.Value.GetLocalState().Pawn)
			{
				Logging.LogDebug("Skipping BUEffectBulletSwitchSelf ApplyBySkill_Implement called for non local player");
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BPS_MultiTargetProjectileCtrComp), "CheckTargetValid")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchCheckTargetValid
	{
		public static bool Prefix(AActor Target, ref bool __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (BGUFunctionLibraryCS.BGUHasUnitSimpleState(Target, (EBGUSimpleState)88))
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BPS_MultiTargetProjectileCtrComp), "SearchTargetTick")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchSearchTargetTick
	{
		private static MethodInfo? _changeToFollowMasterMethod;

		public static void Prefix(BPS_MultiTargetProjectileCtrComp __instance, BPC_MultiTargetProjectileCtrData ___MultiTargetProjectileCtrData, IBUC_TargetInfoData ___TargetInfoData)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			List<UnitLockTargetInfo> list = ((___TargetInfoData != null) ? ___TargetInfoData.GetMultiTargetInfoList() : null);
			if (list == null || list.Count == 0)
			{
				return;
			}
			AActor lockTargetActor = list[0].LockTargetActor;
			if (!((UObject?)(object)lockTargetActor).IsNullOrDestroyed() && BGUFunctionLibraryCS.BGUHasUnitSimpleState(lockTargetActor, (EBGUSimpleState)88))
			{
				if ((object)_changeToFollowMasterMethod == null)
				{
					_changeToFollowMasterMethod = AccessTools.Method(typeof(BPS_MultiTargetProjectileCtrComp), "ChangeToFollowMaster", (Type[])null, (Type[])null);
				}
				if (!(_changeToFollowMasterMethod == null))
				{
					_changeToFollowMasterMethod.Invoke(__instance, null);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_PlayerInputActionComp), "TriggerMagicSkill")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchTriggerMagicSkill
	{
		public static bool Prefix(int SkillID)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return DI.Instance.GameplayConfiguration.IsSkillEnabled(SkillID);
		}
	}
	[HarmonyPatch(typeof(BUS_PlayerInputActionComp), "TriggerItemSkill")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchTriggerItemSkill
	{
		public static bool Prefix(BUS_PlayerInputActionComp __instance)
		{
			if (!DI.Instance.AreaState.CurrentArea.HasValue)
			{
				return true;
			}
			int value = Traverse.Create((object)__instance).Field("ComboCacheData").Property<int>("LastItemSkillID", (object[])null)
				.Value;
			return DI.Instance.GameplayConfiguration.IsSkillEnabled(value);
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchDoPoleDrink
	{
		[HarmonyTargetMethodHint("b1.BUS_PoleDrinkComp", "DoPoleDrink", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_PoleDrinkComp:DoPoleDrink", (Type[])null, (Type[])null);
		}

		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			WukongAreaState areaState = DI.Instance.AreaState;
			if (!areaState.CurrentArea.HasValue)
			{
				return true;
			}
			return areaState.CurrentArea.Value.GetRoom().GourdAllowed;
		}
	}
	[HarmonyPatch(typeof(BUS_CastImmobilizeComp), "OnCastImmobilize")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnCastImmobilize
	{
		public static bool Prefix(int ConfigID, BUS_CastImmobilizeComp __instance)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			BUC_CastImmobilizeData val = (BUC_CastImmobilizeData)AccessTools.PropertyGetter(typeof(BUS_CastImmobilizeComp), "CastImmobilizeData").Invoke(__instance, null);
			IBUC_TargetInfoData val2 = (IBUC_TargetInfoData)AccessTools.PropertyGetter(typeof(BUS_CastImmobilizeComp), "TargetInfoData").Invoke(__instance, null);
			IBUC_PassiveSkillData val3 = (IBUC_PassiveSkillData)AccessTools.PropertyGetter(typeof(BUS_CastImmobilizeComp), "PassiveSkillData").Invoke(__instance, null);
			IBUC_BuffData val4 = (IBUC_BuffData)AccessTools.PropertyGetter(typeof(BUS_CastImmobilizeComp), "BuffData").Invoke(__instance, null);
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
			if (!DI.Instance.AreaState.IsMasterClient)
			{
				if (!entityByPlayerPawn.HasValue)
				{
					return false;
				}
				PlayerId playerId = entityByPlayerPawn.Value.GetState().PlayerId;
				PlayerId? obj = playerState.LocalMainCharacter?.GetState().PlayerId;
				if (playerId == obj)
				{
					DI.Instance.Rpc.SendCastImmobilize(entityByPlayerPawn.Value.GetMeta().NetId);
				}
				return false;
			}
			if (ConfigID == 0)
			{
				ConfigID = val.ResId;
			}
			FUStImmobilizeSkillConfigDesc val5 = default(FUStImmobilizeSkillConfigDesc);
			if (!val3.TryGetCachedDesc<FUStImmobilizeSkillConfigDesc>(ConfigID, ref val5) || BGW_LogUtil.LogIfNull((object)((owner is ABGUCharacter) ? owner : null), "CurCharacter is null"))
			{
				return false;
			}
			AActor lockTargetActor = val2.GetSkillBaseTarget().LockTargetActor;
			ABGUCharacter val6 = (ABGUCharacter)(object)((lockTargetActor is ABGUCharacter) ? lockTargetActor : null);
			if ((UObject)(object)val6 == (UObject)null)
			{
				AActor lockTargetActor2 = val2.GetTargetInfo().LockTargetActor;
				val6 = (ABGUCharacter)(object)((lockTargetActor2 is ABGUCharacter) ? lockTargetActor2 : null);
			}
			if ((UObject)(object)val6 == (UObject)null || !BGUFuncLibSelectTargetsCS.BGUIsSelectTargetByTeamFilter(owner, (AActor)(object)val6, val5.TargetFilter) || !BGUFuncLibSelectTargetsCS.BGUIsSelectTargetByAffiliationFilter(owner, (AActor)(object)val6, val5.AffiliationTypeFilter))
			{
				Logging.LogDebug("CurrentTarget As BGUCharacter is null in PatchOnCastImmobilize");
				return false;
			}
			int num = ((val5.TargetCount <= 0) ? 1 : val5.TargetCount);
			List<AActor> list = new List<AActor>();
			if (num > 1)
			{
				List<int> obj2 = new List<int>(1) { val5.RangeRadius };
				AActor owner2 = ((UActorCompBaseCS)__instance).GetOwner();
				FVector val7 = BGUFuncLibActorTransformCS.BGUGetActorLocation((AActor)(object)val6);
				int targetFilter = val5.TargetFilter;
				int targetTypeFilter = val5.TargetTypeFilter;
				int affiliationTypeFilter = val5.AffiliationTypeFilter;
				IList<int> list2 = obj2;
				BGUFuncLibSelectTargetsCS.BGUSelectTargetsInShape((UObject)(object)owner, ref list, owner2, val7, (ERangeType)1, -1, targetFilter, targetTypeFilter, affiliationTypeFilter, ref list2);
			}
			if (list.Contains((AActor)(object)val6))
			{
				list.Remove((AActor)(object)val6);
			}
			list.Insert(0, (AActor)(object)val6);
			int num2 = 0;
			int num3 = default(int);
			foreach (AActor item in list)
			{
				if (num2 >= num)
				{
					break;
				}
				if (BGUFunctionLibraryCS.BGUHasUnitState(item, (EBGUUnitState)6))
				{
					continue;
				}
				if (BGUFunctionLibraryCS.BGUHasUnitSimpleState(item, (EBGUSimpleState)18))
				{
					int actorResID = BGU_DataUtil.GetActorResID(item);
					UBGWDataAsset fxAssetByResId = ImmobilizeUtils.GetFxAssetByResId((UObject)(object)owner, (IList<FPlayFXByResID>)val5.FailedFXs, actorResID, val.ResId, val);
					if ((UObject)(object)fxAssetByResId != (UObject)null)
					{
						BUS_GSEventCollection obj3 = BUS_EventCollectionCS.Get(item);
						if (obj3 != null)
						{
							obj3.Evt_RequestSpawnFXByDispConfigDA.Invoke(fxAssetByResId, ref num3, (USceneComponent)null, false, default(FTransform), default(DBCSetCallbackParams));
						}
					}
					continue;
				}
				num2++;
				int actorResID2 = BGU_DataUtil.GetActorResID(item);
				if (!BGW_LogUtil.LogIfNull<int>((object)BGW_GameDB.GetUnitCommDesc(actorResID2), "BGW_GameDB.GetUnitCommDesc is null, ResID:%d", actorResID2))
				{
					bool flag = val4.HasBuff(val5.GreatSageTalentActiveBuff);
					ImmobilizeConfigInstance val8 = ImmobilizeUtils.CreateImmobilizeConfig(item, owner, val5, val.ResId, flag, val);
					BUS_GSEventCollection obj4 = BUS_EventCollectionCS.Get(item);
					if (obj4 != null)
					{
						obj4.Evt_TriggerImmobilize.Invoke(val8);
					}
					MainCharacterEntity? entityByPlayerPawn2 = DI.Instance.PawnState.GetEntityByPlayerPawn(item);
					TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(item);
					if ((entityByPlayerPawn2.HasValue || entityByTamerMonster.HasValue) && entityByPlayerPawn.HasValue)
					{
						Logging.LogDebug("Broadcasting trigger immobilize");
						NetworkId playerId2 = ((!entityByPlayerPawn2.HasValue) ? entityByTamerMonster.Value.GetMeta().NetId : entityByPlayerPawn2.Value.GetMeta().NetId);
						DI.Instance.Rpc.SendTriggerImmobilize(new TriggerImmobilizeData(playerId2, entityByPlayerPawn.Value.GetMeta().NetId, flag));
					}
				}
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_BeImmobilizedComp), "OnTickWithGroup")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchImmobilizeOnTickWithGroup
	{
		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if (DI.Instance.AreaState.IsMasterClient)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_BeImmobilizedComp), "RelieveImmobilized")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRelieveImmobilized
	{
		public static bool Prefix(BUS_BeImmobilizedComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
			TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
			if (!entityByPlayerPawn.HasValue && !entityByTamerMonster.HasValue)
			{
				return true;
			}
			TamerEntity value;
			NetworkId netId;
			MainCharacterEntity value2;
			if (!entityByPlayerPawn.HasValue)
			{
				value = entityByTamerMonster.Value;
				netId = value.GetMeta().NetId;
			}
			else
			{
				value2 = entityByPlayerPawn.Value;
				netId = value2.GetMeta().NetId;
			}
			NetworkId payload = netId;
			if (DI.Instance.AreaState.IsMasterClient)
			{
				DI.Instance.Rpc.SendRelieveImmobilize(payload);
				return true;
			}
			if (entityByPlayerPawn.HasValue)
			{
				value2 = entityByPlayerPawn.Value;
				ref LocalMainCharacterComponent localState = ref value2.GetLocalState();
				if (!localState.RunImmobilizePatches)
				{
					return false;
				}
				localState.RunImmobilizePatches = false;
				return true;
			}
			value = entityByTamerMonster.Value;
			ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
			if (!localTamer.RunImmobilizePatches)
			{
				return false;
			}
			localTamer.RunImmobilizePatches = false;
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_BeImmobilizedComp), "OnTriggerImmobilizedBreak")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnTriggerImmobilizedBreak
	{
		public static bool Prefix(BUS_BeImmobilizedComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			if (DI.Instance.AreaState.IsMasterClient)
			{
				MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
				if (entityByPlayerPawn.HasValue)
				{
					DI.Instance.Rpc.SendRelieveImmobilize(entityByPlayerPawn.Value.GetMeta().NetId);
					BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)entityByPlayerPawn.Value.GetLocalState().Pawn);
					if (obj != null)
					{
						obj.Evt_RelieveImmobilized.Invoke();
					}
					return false;
				}
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue)
				{
					TamerEntity value = entityByTamerMonster.Value;
					ref MetadataComponent meta = ref value.GetMeta();
					value = entityByTamerMonster.Value;
					ref LocalTamerComponent localTamer = ref value.GetLocalTamer();
					DI.Instance.Rpc.SendRelieveImmobilize(meta.NetId);
					BUS_GSEventCollection obj2 = BUS_EventCollectionCS.Get((AActor)(object)localTamer.Pawn);
					if (obj2 != null)
					{
						obj2.Evt_RelieveImmobilized.Invoke();
					}
				}
				Logging.LogDebug("Character state is null - continuing standard execution");
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_PhantomRushComp), "OnTriggerPhantomRush")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnTriggerPhantomRush
	{
		public static bool Prefix(BUS_PhantomRushComp __instance, IBUC_SimpleStateData ___SimpleStateData, IBUC_UnitStateData ___UnitStateData, BUC_PhantomRushData ___PhantomRushData, IBUC_SkillInstsData ___SkillInstsData, ESkillDirection PhantomRushDir)
		{
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AreaEntity? currentArea = DI.Instance.AreaState.CurrentArea;
			if (!currentArea.HasValue)
			{
				return true;
			}
			if (!currentArea.Value.GetRoom().PhantomRushAllowed)
			{
				return false;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return false;
			}
			if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				return true;
			}
			MethodInfo methodInfo = AccessTools.Method(typeof(BUS_PhantomRushComp), "GetActualUseConfigID", (Type[])null, (Type[])null);
			if (methodInfo == null)
			{
				Logging.LogError("GetActualUseConfigID method info is null");
				return false;
			}
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get(owner);
			BGS_GSEventCollection val2 = BGS_GSEventCollection.Get(owner);
			ACharacter val3 = (ACharacter)(object)((owner is ACharacter) ? owner : null);
			if ((UObject)(object)val3 == (UObject)null || ___SimpleStateData.HasSimpleState((EBGUSimpleState)88))
			{
				Logging.LogDebug("aCharacter is null or PhantomRush is already active");
				return false;
			}
			FUStPhantomRushSkillConfigDesc phantomRushSkillConfigDesc = BGW_GameDB.GetPhantomRushSkillConfigDesc((int)methodInfo.Invoke(__instance, null), owner);
			if (phantomRushSkillConfigDesc == null)
			{
				Logging.LogError("phantomRushSkillConfigDesc is null");
				return false;
			}
			((UActorCompBaseCS)__instance).PreloadAssetMgr.TryGetCachedResourceObj<BGWDataAsset_PhantomRushRelatedeSkillConfig>(phantomRushSkillConfigDesc.PhantomRushRelatedSkillConfigPath, (ELoadResourceType)0, (EAssetPriority)2, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			FPoseSnapshot poseSnapshot = default(FPoseSnapshot);
			val3.Mesh.SnapshotPose(ref poseSnapshot);
			___PhantomRushData.PoseSnapshot = poseSnapshot;
			UAnimInstance animInstance = val3.Mesh.GetAnimInstance();
			FContinueBehaviorInfo val4 = default(FContinueBehaviorInfo);
			if ((UObject)(object)animInstance != (UObject)null)
			{
				UAnimMontage currentActiveMontage = animInstance.GetCurrentActiveMontage();
				if ((UObject)(object)currentActiveMontage != (UObject)null)
				{
					if (___SimpleStateData.HasSimpleState((EBGUSimpleState)77))
					{
						val4.CBT = (EContinueBehaviorType)3;
						val4.MontagePos = animInstance.Montage_GetPosition(currentActiveMontage);
						val4.BeatbackMontage = currentActiveMontage;
					}
					else if (___UnitStateData.HasState((EBGUUnitState)1))
					{
						val4.MontagePos = animInstance.Montage_GetPosition(currentActiveMontage);
						val4.CBT = (EContinueBehaviorType)1;
						val4.SkillID = ___SkillInstsData.CurrentCastingSkillID;
					}
					else if (___UnitStateData.HasState((EBGUUnitState)4))
					{
						val4.CBT = (EContinueBehaviorType)2;
						val4.MontagePos = animInstance.Montage_GetPosition(currentActiveMontage);
						val4.BeatbackMontage = currentActiveMontage;
					}
				}
			}
			val.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)75, false);
			GSDel_UnitCastSkillTry evt_UnitCastSkillTry = val.Evt_UnitCastSkillTry;
			FCastSkillInfo val5 = default(FCastSkillInfo);
			((FCastSkillInfo)(ref val5))..ctor(phantomRushSkillConfigDesc.PhantomRushSkillID, (ECastSkillSourceType)27, false, PhantomRushDir, (EMontageBindReason)0);
			val5.NeedCheckSkillCanCast = true;
			evt_UnitCastSkillTry.Invoke(val5);
			val.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)75, true);
			if ((int)___SkillInstsData.GetLastSkillCastResult() != 0)
			{
				Logging.LogDebug("GetLastSkillCastResult was not success");
				return false;
			}
			val.Evt_ClearAbnormalState.Invoke(new HashSet<EAbnormalStateType>
			{
				(EAbnormalStateType)2,
				(EAbnormalStateType)1,
				(EAbnormalStateType)3,
				(EAbnormalStateType)4
			});
			int phantomRushSummonID = phantomRushSkillConfigDesc.PhantomRushSummonID;
			val.Evt_SummonSkillCastByPhantomRush.Invoke(phantomRushSummonID, val4);
			val.Evt_UnitSetSimpleState.Invoke((EBGUSimpleState)88, false);
			foreach (int phantomRushBeginAddBuffID in phantomRushSkillConfigDesc.PhantomRushBeginAddBuffIDList)
			{
				val.Evt_BuffAdd.Invoke(phantomRushBeginAddBuffID, owner, owner, -1f, (EBuffSourceType)29, false, default(FBattleAttrSnapShot));
			}
			___PhantomRushData.PhantomRushTimer = phantomRushSkillConfigDesc.PhantomRushDuration;
			___PhantomRushData.PhantomRushNoMagicProtectTimer = 1f;
			if (val2 != null)
			{
				val2.Evt_BGS_ClearAttachedProjectiles_OnUnit.Invoke(owner);
			}
			return false;
		}

		public static void Postfix(BUS_PhantomRushComp __instance, IBUC_SimpleStateData ___SimpleStateData, ESkillDirection PhantomRushDir)
		{
			if (!DI.Instance.AreaState.InRoom || !___SimpleStateData.HasSimpleState((EBGUSimpleState)88))
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
			if (entityByPlayerPawn.HasValue && entityByPlayerPawn != playerState.LocalMainCharacter && playerState.LocalPlayerEntity.HasValue)
			{
				DI.Instance.ModeManager.SetPlayerVisibility(playerState.LocalPlayerEntity.Value, entityByPlayerPawn.Value, visible: false);
			}
		}
	}
	[HarmonyPatch(typeof(BUS_SkillInstsCompSvr), "OnUnitCastSkillTry", new Type[] { typeof(FCastSkillInfo) })]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnUnitCastSkillTry
	{
		public static void Postfix(FCastSkillInfo CSI, BUC_SkillInstsData ___SkillInstsData, BUS_SkillInstsCompSvr __instance)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Invalid comparison between Unknown and I4
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if ((int)___SkillInstsData.GetLastSkillCastResult() != 0)
			{
				Logging.LogDebug("GetLastSkillCastResult was not success");
				return;
			}
			if ((int)CSI.SourceType == 27 && (UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				Logging.LogDebug("Sending phantom rush with direction: {Direction}", CSI.SkillDirection);
				DI.Instance.Rpc.SendPhantomRush(CSI.SkillDirection);
				return;
			}
			WukongPawnState pawnState = DI.Instance.PawnState;
			if ((int)CSI.SourceType == 36 && CSI.SkillID == 471236)
			{
				TamerEntity? entityByTamerMonster = pawnState.GetEntityByTamerMonster(owner);
				if (entityByTamerMonster.HasValue && DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					DI.Instance.Rpc.SendCastSkill(entityByTamerMonster.Value.GetMeta().NetId, CSI.SkillID, CSI.SourceType);
					Logging.LogDebug("Sent CBG skill cast for skill {SkillId}", CSI.SkillID);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_PhantomRushComp), "ExitPhantomRush")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchExitPhantomRush
	{
		public static void Prefix(BUS_PhantomRushComp __instance, IBUC_SimpleStateData ___SimpleStateData)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if (((UObject?)(object)owner).IsNullOrDestroyed())
			{
				Logging.LogError("Owner is null or destroyed");
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
			if (entityByPlayerPawn.HasValue)
			{
				MainCharacterEntity value = entityByPlayerPawn.Value;
				ref MainCharacterComponent state = ref value.GetState();
				value = entityByPlayerPawn.Value;
				ref LocalMainCharacterComponent localState = ref value.GetLocalState();
				if ((DI.Instance.AreaState.IsMasterClient || (UObject)(object)owner == (UObject)(object)localState.Pawn) && !localState.ReceivedPhantomRushExit)
				{
					Logging.LogDebug("Broadcasting phantom rush exit for player {Nickname}", state.CharacterNickName);
					DI.Instance.Rpc.SendExitPhantomRush(state.PlayerId);
					localState.ReceivedPhantomRushExit = false;
				}
				PlayerId playerId = state.PlayerId;
				PlayerEntity? playerById = DI.Instance.PlayerState.GetPlayerById(playerId);
				if (entityByPlayerPawn != playerState.LocalMainCharacter && playerById.HasValue)
				{
					DI.Instance.ModeManager.SetPlayerVisibility(playerById.Value, entityByPlayerPawn.Value, visible: true);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUFFPlayerWinePartnerAttr), "Apply")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchBuffPlayerWinePartnerAttr
	{
		public static bool Prefix(AActor Target, out float OutAbs, out float OutMul)
		{
			OutAbs = 0f;
			OutMul = 0f;
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			ABGUCharacter val = (ABGUCharacter)(object)((Target is ABGUCharacter) ? Target : null);
			if ((UObject)(object)val != (UObject)null)
			{
				IBPC_PlayerRoleData readOnlyData = BGU_DataUtil.GetReadOnlyData<IBPC_PlayerRoleData, BPC_PlayerRoleData>((AActor)(object)((APawn)val).GetController());
				if (readOnlyData != null && readOnlyData.RoleData == null)
				{
					return false;
				}
			}
			return true;
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class TransformationPatch
	{
		[HarmonyTargetMethodHint("b1.BUS_PlayerTransComp", "TransferData", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_PlayerTransComp:TransferData", (Type[])null, (Type[])null);
		}

		public static void Postfix(UActorCompBaseCS __instance, ABGUCharacter ToReplaceUnitInst)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return;
			}
			AActor owner = __instance.GetOwner();
			APawn val = (APawn)(object)((owner is APawn) ? owner : null);
			if ((UObject)(object)val == (UObject)null)
			{
				Logging.LogDebug("Skipping transformation because the owner is not a pawn");
				return;
			}
			BGUCharacterCS val2 = (BGUCharacterCS)(object)((ToReplaceUnitInst is BGUCharacterCS) ? ToReplaceUnitInst : null);
			if ((UObject)(object)val2 == (UObject)null)
			{
				Logging.LogDebug("Skipping transformation because the new owner is not a BGUCharacterCS");
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn((AActor?)(object)val);
			if (!entityByPlayerPawn.HasValue)
			{
				Logging.LogDebug("Skipping transformation of {OldOwner} because player state is null", ((UObject)val).GetName());
				return;
			}
			MainCharacterEntity value = entityByPlayerPawn.Value;
			ref MainCharacterComponent state = ref value.GetState();
			value = entityByPlayerPawn.Value;
			value.GetLocalState().Pawn = val2;
			EquipmentUtils.SetActorEquipment(val2, state.Equipment);
			Logging.LogDebug("Transformed {OldOwner} to {NewOwner}", (val != null) ? ((UObject)val).GetName() : null, (val2 != null) ? ((UObject)val2).GetName() : null);
		}
	}
	[HarmonyPatch(typeof(BPC_BattleMainInfoData), "GetCommonDisabledState")]
	[HarmonyPatchCategory("Connected")]
	public class PatchLogs4
	{
		public static bool Prefix(BPC_BattleMainInfoData __instance, ref bool __result, out bool IsDisabled)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				IsDisabled = false;
				return true;
			}
			BGUCharacterCS ownerCharacter = __instance.OwnerCharacter;
			string obj = ((ownerCharacter != null) ? ((UObject)ownerCharacter).GetName() : null);
			BGUPlayerCharacterCS? controlledPawn = GameUtils.GetControlledPawn();
			if (obj != ((controlledPawn != null) ? ((UObject)controlledPawn).GetName() : null))
			{
				__result = true;
				IsDisabled = false;
				return false;
			}
			IsDisabled = false;
			return true;
		}
	}
	[HarmonyPatchCategory("Connected")]
	public class PatchOnTransBeginSpawnNewOne
	{
		[HarmonyTargetMethodHint("b1.BUS_PlayerTransComp", "OnTransBeginSpawnNewOne", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_PlayerTransComp:OnTransBeginSpawnNewOne", (Type[])null, (Type[])null);
		}

		public static void Prefix(UActorCompBaseCS __instance, int ToReplaceUnitResID, int ToReplaceUnitBornSkillID, bool EnableBlendViewTarget, EPlayerTransBeginType TransBeginType)
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom)
			{
				WukongPlayerState playerState = DI.Instance.PlayerState;
				AActor owner = __instance.GetOwner();
				if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
				{
					Logging.LogDebug("OnTransBeginSpawnNewOne: Sending transform for player {Name} to unit with id {UnitId}", playerState.LocalMainCharacter.Value.GetState().CharacterNickName, ToReplaceUnitResID);
					DI.Instance.Rpc.SendPlayerTransBegin(new PlayerTransBeginData(ToReplaceUnitResID, ToReplaceUnitBornSkillID, EnableBlendViewTarget, TransBeginType));
				}
				MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
				if (entityByPlayerPawn.HasValue)
				{
					entityByPlayerPawn.Value.GetState().IsTransformed = true;
				}
			}
		}
	}
	[HarmonyPatchCategory("Connected")]
	public class PatchOnTransBackSpawnNewOne
	{
		[HarmonyTargetMethodHint("b1.BUS_PlayerTransComp", "OnTransBackSpawnNewOne", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_PlayerTransComp:OnTransBackSpawnNewOne", (Type[])null, (Type[])null);
		}

		public static void Prefix(UActorCompBaseCS __instance, int ToReplaceUnitResID, int ToReplaceUnitBornSkillID, bool EnableBlendViewTarget, EPlayerTransEndType TransEndType, out object? __state)
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				__state = null;
				return;
			}
			WukongPlayerState playerState = DI.Instance.PlayerState;
			AActor owner = __instance.GetOwner();
			if ((UObject)(object)owner == (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn)
			{
				Logging.LogDebug("OnTransBackSpawnNewOne: Sending transform for player {Name} to unit with id {UnitId}", playerState.LocalMainCharacter.Value.GetState().CharacterNickName, ToReplaceUnitResID);
				DI.Instance.Rpc.SendPlayerTransEnd(new PlayerTransEndData(ToReplaceUnitResID, ToReplaceUnitBornSkillID, EnableBlendViewTarget, TransEndType));
			}
			__state = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
		}

		public static void Postfix(UActorCompBaseCS __instance, object? __state)
		{
			if (DI.Instance.AreaState.InRoom)
			{
				MainCharacterEntity? mainCharacterEntity = (MainCharacterEntity?)__state;
				if (mainCharacterEntity.HasValue)
				{
					MainCharacterEntity value = mainCharacterEntity.Value;
					ref MainCharacterComponent state = ref value.GetState();
					state.IsTransformed = false;
					value = mainCharacterEntity.Value;
					IBUC_AttrContainer readOnlyData = BGU_DataUtil.GetReadOnlyData<IBUC_AttrContainer, BUC_AttrContainer>((AActor)(object)value.GetLocalState().Pawn);
					state.Hp = readOnlyData.GetFloatValue((EBGUAttrFloat)1);
					state.IsDead = false;
				}
			}
		}
	}
	[HarmonyPatchCategory("Connected")]
	public class PatchSpawnAndPossess
	{
		[HarmonyTargetMethodHint("b1.BUS_PlayerTransComp", "SpawnAndPossessTransUnit", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_PlayerTransComp:SpawnAndPossessTransUnit", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UActorCompBaseCS __instance, BUC_PlayerTransData ___PlayerTransData, BGUCharacterCS ___OwnerAsCharacterCS, AActor ___Owner, ref APawn? __result, UClass CharacterClass, FTransform BornTransform, SpawnControlledPawnBlendParam SpawnControlledPawnBlendParam, int ToReplaceUnitResID)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			BGW_EventCollection value = Traverse.Create((object)__instance).Property<BGW_EventCollection>("BGWEventCollection", (object[])null).Value;
			BUS_GSEventCollection value2 = Traverse.Create((object)__instance).Property<BUS_GSEventCollection>("BUSEventCollection", (object[])null).Value;
			APawn newPawn = null;
			EPlayerTransEndType val = (EPlayerTransEndType)(((int)___PlayerTransData.TransTypeCached == 0) ? 1 : ((int)___PlayerTransData.TransTypeCached));
			value.Evt_BGW_UnitTrans.Invoke(___Owner, val);
			value2.Evt_NotifyUnitTrans_BeforePosses.Invoke(val);
			APawn instigator = ((AActor)___OwnerAsCharacterCS).Instigator;
			AController val2 = (((UObject)(object)instigator != (UObject)null) ? instigator.GetController() : null);
			if ((UObject)(object)val2 == (UObject)null)
			{
				Logging.LogError("Controller is null, cannot transform");
				__result = null;
				return false;
			}
			ABGPPlayerController playerController = (ABGPPlayerController)(object)((val2 is ABGPPlayerController) ? val2 : null);
			SpawnTransform(val2, CharacterClass, BornTransform, delegate(APawn Pawn)
			{
				newPawn = Pawn;
				if ((UObject)(object)playerController != (UObject)null)
				{
					BPS_GSEventCollection obj = BPS_EventCollectionCS.Get((APlayerController)(object)playerController);
					if (obj != null)
					{
						obj.Evt_PlayerActorSpawn.Invoke();
					}
					BPS_GSEventCollection obj2 = BPS_EventCollectionCS.Get((APlayerController)(object)playerController);
					if (obj2 != null)
					{
						obj2.Evt_BPS_SwitchPlayerTransState.Invoke(___Owner, ToReplaceUnitResID);
					}
				}
			}, SpawnControlledPawnBlendParam);
			if ((UObject)(object)playerController != (UObject)null && !SpawnControlledPawnBlendParam.EnableBlendViewTarget)
			{
				((APlayerController)playerController).SetViewTargetWithBlend(___Owner, 0f, (EViewTargetBlendFunction)0, 0f, false);
			}
			__result = newPawn;
			return false;
		}

		private static APawn? SpawnTransform(AController controller, UClass pawnClass, FTransform spawnTransform, Action<APawn> beforeBeginPlayCb, SpawnControlledPawnBlendParam blendParam)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			APawn controlledPawn = controller.GetControlledPawn();
			ABGPPlayerController val = (ABGPPlayerController)(object)((controller is ABGPPlayerController) ? controller : null);
			AActor obj = BGU_UnrealActorUtil.BGUBeginDeferredActorSpawnFromClass((UObject)(object)((AActor)controller).World, TSubclassOf<AActor>.op_Implicit(pawnClass), spawnTransform, (ESpawnActorCollisionHandlingMethod)1, (AActor)null);
			APawn val2 = (APawn)(object)((obj is APawn) ? obj : null);
			if ((UObject)(object)val2 == (UObject)null)
			{
				Logging.LogError("New pawn is null, cannot transform");
				return null;
			}
			if (blendParam.NeedBlend && (UObject)(object)val != (UObject)null)
			{
				val.OnPossessWithViewTargetBlend(val2, blendParam.PossessBlendTime, (EViewTargetBlendFunction)blendParam.PossessBlendFunc, blendParam.PossessBlendExp, true, blendParam.EnableBlendViewTarget);
			}
			else
			{
				controller.Possess(val2);
			}
			beforeBeginPlayCb(val2);
			ACharacter val3 = (ACharacter)val2;
			BGUPlayerCharacterCS controlledPawn2 = GameUtils.GetControlledPawn();
			BGP_PlayerControllerB1 playerController = GameUtils.GetPlayerController();
			bool flag = false;
			FRotator controllerRotation = FRotator.ZeroRotator;
			if ((UObject)(object)controller != (UObject)(object)playerController && (UObject)(object)controlledPawn2 != (UObject)null)
			{
				flag = true;
				controllerRotation = ((AController)playerController).GetControlRotation();
				GameUtils.PossessPawn((ABGPPlayerController)(object)playerController, val2, (APawn)(object)controlledPawn2);
			}
			((UPrimitiveComponent)val3.CapsuleComponent).SetGenerateOverlapEvents(false);
			((UPrimitiveComponent)val3.CapsuleComponent).SetGenerateOverlapEvents(false);
			BGU_UnrealActorUtil.BGUFinishSpawningActorAndECSBeginPlay((UObject)(object)controller, (AActor)(object)val2, spawnTransform);
			if (flag && (UObject)(object)controlledPawn2 != (UObject)null)
			{
				GameUtils.PossesPawnWithViewTarget((ABGPPlayerController)(object)playerController, (APawn)(object)controlledPawn2, val2, controllerRotation);
				controller.Possess(val2);
			}
			if ((UObject)(object)val != (UObject)null)
			{
				BPS_GSEventCollection.Get((APlayerController)(object)val).Evt_BPS_OnControlledPawnChange.Invoke(val2);
				BGS_GSEventCollection obj2 = BGS_EventCollectionCS.Get((UObject)(object)val);
				if (obj2 != null)
				{
					obj2.Evt_NotifyPossessEntityChanged.Invoke(ECSExtension.ToEntity((AActor)(object)controlledPawn), ECSExtension.ToEntity((AActor)(object)val2));
				}
			}
			((UPrimitiveComponent)val3.CapsuleComponent).SetGenerateOverlapEvents(true);
			((UPrimitiveComponent)val3.CapsuleComponent).SetGenerateOverlapEvents(true);
			UGSE_ActorFuncLib.UpdateActorOverlaps((AActor)val3);
			return val2;
		}
	}
	[HarmonyPatch(typeof(BUS_TransGuideComp), "UpdateTransGuideData")]
	[HarmonyPatchCategory("Connected")]
	public class PatchUpdateTransGuideData
	{
		public static bool Prefix(BUS_TransGuideComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)((UActorCompBaseCS)__instance).GetOwner() != (UObject)(object)GameUtils.GetControlledPawn())
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_TransPlayerDataBindComp), "OnPostTransBindData")]
	[HarmonyPatchCategory("Connected")]
	public class PatchOnPostTransBindData
	{
		public static bool Prefix(BUS_TransPlayerDataBindComp __instance)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)((UActorCompBaseCS)__instance).GetOwner() != (UObject)(object)GameUtils.GetControlledPawn())
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(BUS_IronBodyComp), "OnIronBodyStart")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnIronBodyStart
	{
		public static void Postfix(BUS_IronBodyComp __instance)
		{
			if (DI.Instance.AreaState.InRoom && (UObject)(object)DI.Instance.PlayerState.LocalMainCharacter?.GetLocalState().Pawn == (UObject)(object)((UActorCompBaseCS)__instance).GetOwner())
			{
				DI.Instance.Rpc.SendIronBodyStart();
			}
		}
	}
	[HarmonyPatch(typeof(BPS_BattleMainInfoComp), "OnPossessed")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchBattleMainInfoCompOnPossessed
	{
		public static bool Prefix(AActor? OldActor, AActor? CurActor)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)CurActor != (UObject)null && (UObject)(object)CurActor == (UObject)(object)GameUtils.GetControlledPawn())
			{
				return true;
			}
			Logging.LogDebug("BPS_BattleMainInfoComp OnPossessed called, but the current actor is not the controlled pawn. OldActor: {OldActor}, CurActor: {CurActor}", (OldActor != null) ? ((UObject)OldActor).GetName() : null, (CurActor != null) ? ((UObject)CurActor).GetName() : null);
			return false;
		}
	}
	[HarmonyPatch(typeof(BPS_InputSystem), "OnPossessed")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchInputSystemOnPossessed
	{
		public static bool Prefix(AActor? OldActor, AActor? CurActor)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)CurActor != (UObject)null && (UObject)(object)CurActor == (UObject)(object)GameUtils.GetControlledPawn())
			{
				return true;
			}
			Logging.LogDebug("BPS_InputSystem OnPossessed called, but the current actor is not the controlled pawn. OldActor: {OldActor}, CurActor: {CurActor}", (OldActor != null) ? ((UObject)OldActor).GetName() : null, (CurActor != null) ? ((UObject)CurActor).GetName() : null);
			return false;
		}
	}
	[HarmonyPatch(typeof(BPS_MultiTargetProjectileCtrComp), "OnPossessed")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchMultiTargetOnPossessed
	{
		public static bool Prefix(AActor? OldActor, AActor? CurActor)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((UObject)(object)CurActor != (UObject)null && (UObject)(object)CurActor == (UObject)(object)GameUtils.GetControlledPawn())
			{
				return true;
			}
			Logging.LogDebug("BPS_MultiTargetProjectileCtrComp OnPossessed called, but the current actor is not the controlled pawn. OldActor: {OldActor}, CurActor: {CurActor}", (OldActor != null) ? ((UObject)OldActor).GetName() : null, (CurActor != null) ? ((UObject)CurActor).GetName() : null);
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_MagicallyChangeComp), "DoCastMagicallyChangeSkill_PendingCast")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchDoCastMagicallyChangeSkill_PendingCast
	{
		public static void Postfix(BUS_MagicallyChangeComp __instance, UBGWDataAsset? _Config, int _SkillID, int _RecoverSkillID, BUC_MagicallyChangeData ___MagicallyChangeData)
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom && (UObject)(object)_Config != (UObject)null)
			{
				Logging.LogDebug("BUS_MagicallyChangeComp DoCastMagicallyChangeSkill_PendingCast called with Config Path: {Path}, SkillID: {SkillID}, RecoverSkillID: {RecoverSkillID}, CurVigorSkillID {CurVigorSkillID}", ((UObject)_Config).PathName, _SkillID, _RecoverSkillID, ___MagicallyChangeData.CurVigorSkillID);
				WukongPlayerState playerState = DI.Instance.PlayerState;
				if (DI.Instance.State.LocalPlayerId.HasValue && (UObject)(object)playerState.LocalMainCharacter?.GetLocalState().Pawn == (UObject)(object)((UActorCompBaseCS)__instance).GetOwner())
				{
					DI.Instance.Rpc.SendTriggerMagicallyChange(DI.Instance.State.LocalPlayerId.Value, _Config, _SkillID, _RecoverSkillID, ___MagicallyChangeData.CurVigorSkillID, ___MagicallyChangeData.CastReason);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BUS_MagicallyChangeComp), "PendingReset")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchPendingReset
	{
		public static void Postfix(BUS_MagicallyChangeComp __instance, EResetReason_MagicallyChange Reason)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom)
			{
				Logging.LogDebug("BUS_MagicallyChangeComp PendingReset called with reason: {Reason}", Reason);
				if ((UObject)(object)DI.Instance.PlayerState.LocalMainCharacter?.GetLocalState().Pawn == (UObject)(object)((UActorCompBaseCS)__instance).GetOwner())
				{
					DI.Instance.Rpc.SendResetMagicallyChange(Reason);
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnSweepCheckHit
	{
		[HarmonyTargetMethodHint("b1.BUS_SweepCheckHitComp", "OnSweepCheckHit", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_SweepCheckHitComp:OnSweepCheckHit", (Type[])null, (Type[])null);
		}

		public static bool Prefix(UActorCompBaseCS __instance, AActor Victim)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			AActor owner = __instance.GetOwner();
			BGUCharacterCS val = (BGUCharacterCS)(object)((owner is BGUCharacterCS) ? owner : null);
			if (val != null)
			{
				BGUCharacterCS val2 = (BGUCharacterCS)(object)((Victim is BGUCharacterCS) ? Victim : null);
				if (val2 != null && val.GetTeamIDInCS() == val2.GetTeamIDInCS())
				{
					return false;
				}
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(FInputMappingContextProcessor), "SetCloudInputEnable")]
	[HarmonyPatchCategory("Global")]
	public static class PatchSetCloudInputEnable
	{
		public static bool Prefix(bool bEnable)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			BUC_CloudMoveData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_CloudMoveData>((AActor)(object)DI.Instance.PlayerState.LocalMainCharacter?.GetLocalState().Pawn);
			if (unPersistentReadOnlyData == null)
			{
				return true;
			}
			return unPersistentReadOnlyData.IsCloudMoveEnabled == bEnable;
		}
	}
	[HarmonyPatch(typeof(BGU_UnrealWorldUtil), "RequestSpawnServant")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchRequestSpawnServant
	{
		public static bool Prefix(ref string? __result, UWorld World, TSubclassOf<BUTamerActor> TamerClass, in FTransform InTransform, FServantReq InServantReq, bool SafeClampToLand = false)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			EServantType servantType = InServantReq.ServantType;
			if (((int)servantType == 4 || (int)servantType == 6) ? true : false)
			{
				return true;
			}
			__result = null;
			AActor summoner = InServantReq.Summoner;
			FTransform val = InTransform;
			if (SpawningUtils.CanSummon(summoner, ((FTransform)(ref val)).GetLocation()))
			{
				BUTamerActor val2 = SpawningUtils.BeginDeferredSummonSpawn(World, TamerClass, InTransform, InServantReq.SummonID, SafeClampToLand);
				if ((UObject)(object)val2 == (UObject)null)
				{
					return false;
				}
				val2.MarkAsServant();
				InServantReq.ServantTamerGuid = val2.GetFinalGuid(false);
				BPS_EventCollectionCS.GetLocal((UObject)(object)World).Evt_SendServantReq.Invoke(InServantReq);
				UBGUFunctionLibrary.BGUFinishSpawningActor((AActor)(object)val2, InTransform);
				__result = InServantReq.ServantTamerGuid;
				int teamId = 2;
				AActor masterActor = InServantReq.MasterActor;
				BGUCharacterCS val3 = (BGUCharacterCS)(object)((masterActor is BGUCharacterCS) ? masterActor : null);
				if (val3 != null)
				{
					teamId = val3.GetTeamIDInCS();
				}
				SpawningUtils.CreateMonsterInEcs(__result, val2, teamId, ((UObject)val2).PathName);
				object[] array = new object[3];
				AActor summoner2 = InServantReq.Summoner;
				array[0] = ((summoner2 != null) ? ((UObject)summoner2).GetName() : null) ?? "Null";
				array[1] = InServantReq.ServantTamerGuid;
				array[2] = ((UObject)InServantReq.TamerTemplate).GetName();
				Logging.LogDebug("Sending SpawnSummon for summoner {Summoner} with guid {Guid} for tamer path {Path}", array);
				DI.Instance.Rpc.SendSpawnSummon(InServantReq.FromGame());
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BGS_SummonManagerSystem), "RequestSummon")]
	[HarmonyPatchCategory("Connected")]
	public class PatchRequestSummon
	{
		public static bool Prefix(FSummonReq InSummonReq)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			if ((int)InSummonReq.SummonType == 4 || (int)InSummonReq.SummonType == 1)
			{
				return true;
			}
			return SpawningUtils.CanSummon(InSummonReq.Summoner, InSummonReq.HitLocation);
		}
	}
	[HarmonyPatch(typeof(BUS_BeAttackedComp), "CanShowDmgNumUI")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchCanShowDamage
	{
		public static bool Prefix(ref bool __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			__result = true;
			return false;
		}
	}
	[HarmonyPatch(typeof(BUS_BeAttackedComp), "CanShowDmgNumUI")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchDamageNumberDisplayCheck
	{
		public static void Postfix(BUS_BeAttackedComp __instance, ref bool __result)
		{
			if (!__result)
			{
				return;
			}
			AActor owner = ((UActorCompBaseCS)__instance).GetOwner();
			if ((UObject)(object)owner == (UObject)null)
			{
				return;
			}
			MainCharacterEntity? entityByPlayerPawn = DI.Instance.PawnState.GetEntityByPlayerPawn(owner);
			if (!entityByPlayerPawn.HasValue || !DI.Instance.ClientOwnership.OwnsEntity(entityByPlayerPawn.Value.Entity))
			{
				TamerEntity? entityByTamerMonster = DI.Instance.PawnState.GetEntityByTamerMonster(owner);
				if (!entityByTamerMonster.HasValue || !DI.Instance.ClientOwnership.OwnsEntity(entityByTamerMonster.Value.Entity))
				{
					__result = false;
				}
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchSendDamageNumbers
	{
		[HarmonyTargetMethodHint("b1.BUS_UIControlSystemV2", "OnDisplayDamageNumUI", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("b1.BUS_UIControlSystemV2:OnDisplayDamageNumUI", (Type[])null, (Type[])null);
		}

		public static void Prefix(DamageNumParam Param)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (DI.Instance.AreaState.InRoom)
			{
				DI.Instance.Rpc.SendDamageNum(Param);
			}
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public class PatchBossRushTimerCountdown
	{
		[HarmonyTargetMethodHint("B1UI.GSUI.UIBossRushTime", "GetRemainTimeStr", new Type[] { })]
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method("B1UI.GSUI.UIBossRushTime:GetRemainTimeStr", (Type[])null, (Type[])null);
		}

		public static bool Prefix(ref string __result)
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			__result = "00:00";
			return false;
		}
	}
	[HarmonyPatch(typeof(GenAGPage), "ShowPage")]
	[HarmonyPatchCategory("Connected")]
	public class PatchShowPage
	{
		public static void Prefix(int NewPageID, string Source, ChangeReason Reason, object exParam)
		{
			Logging.LogInformation("ShowPage: {NewPageID}, {Source}, {Reason}, {ExParam}", NewPageID, Source, Reason, exParam);
		}
	}
	[HarmonyPatch(typeof(B1BattleLogicSvc), "UISetGamePaused")]
	[HarmonyPatchCategory("Connected")]
	public class PatchUISetGamePaused
	{
		public static bool Prefix()
		{
			if (!DI.Instance.AreaState.InRoom)
			{
				return true;
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(BGW_PauseGameMgr), "SetGamePause")]
	[HarmonyPatchCategory("Connected")]
	public class PatchSetGamePause
	{
		public static bool Prefix(EPauseEvent PauseEvent, bool bPause)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			if (!DI.Instance.Connection.IsRunning)
			{
				return true;
			}
			if (!bPause)
			{
				return true;
			}
			if (((int)PauseEvent == 3 || (int)PauseEvent == 7) ? true : false)
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(GSLocalization), "SetCurrentCulture")]
	[HarmonyPatchCategory("Global")]
	public class PatchSetCurrentCulture
	{
		public static void Postfix(string Culture)
		{
			string text = ((!(Culture == "zh-Hans-CN") && !(Culture == "zh-Hant")) ? Culture : "zh-Hans");
			string text2 = text;
			Logging.LogInformation("Culture changed to: {Culture}", text2);
			Texts.Culture = new CultureInfo(text2);
			DI.Instance.GameplayEventRouter.RaiseOnLanguageChanged(Texts.Culture);
		}
	}
	[HarmonyPatch(typeof(GSProcBar), "SetParamValue")]
	[HarmonyPatchCategory("Connected")]
	public class ThreadSafeHealthBarPatch
	{
		public static readonly ReaderWriterLockSlim GsProcBarSemaphore = new ReaderWriterLockSlim();

		public static void Prefix()
		{
			GsProcBarSemaphore.EnterWriteLock();
		}

		public static void Postfix()
		{
			GsProcBarSemaphore.ExitWriteLock();
		}
	}
	[HarmonyPatch(typeof(GSProcBar), "GetParamValue")]
	[HarmonyPatchCategory("Connected")]
	public class ThreadSafeHealthBarPatch2
	{
		public static void Prefix()
		{
			ThreadSafeHealthBarPatch.GsProcBarSemaphore.EnterReadLock();
		}

		public static void Postfix()
		{
			ThreadSafeHealthBarPatch.GsProcBarSemaphore.ExitReadLock();
		}
	}
	[HarmonyPatch(typeof(GSProcBar), "SetParamPercent")]
	[HarmonyPatchCategory("Connected")]
	public class ThreadSafeHealthBarPatch3
	{
		public static void Prefix()
		{
			ThreadSafeHealthBarPatch.GsProcBarSemaphore.EnterReadLock();
		}

		public static void Postfix()
		{
			ThreadSafeHealthBarPatch.GsProcBarSemaphore.ExitReadLock();
		}
	}
	[HarmonyPatch]
	[HarmonyPatchCategory("Connected")]
	public static class PatchOnInfoChange
	{
		[HarmonyTargetMethodHint("B1UI.GSUI.UILoadingAdaptor", "OnInfoChange", new Type[] { })]
		public static MethodBase TargetMethod()
		{
			return AccessTools.Method("B1UI.GSUI.UILoadingAdaptor:OnInfoChange", (Type[])null, (Type[])null);
		}

		public static bool Prefix(ChangeReason Reason, FLoadingAdaptorInfo NewValue, UObject ___WorldContext)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (Reason == ChangeReason.UiInit)
			{
				return true;
			}
			if (GameDBRuntime.GetChapterDescByLevelId(NewValue.TargetLevelId) == null)
			{
				return true;
			}
			if (!NewValue.IsFadeIn)
			{
				return true;
			}
			int curLevelId = BGUFuncLibMap.GetCurLevelId(___WorldContext);
			return NewValue.TargetLevelId != curLevelId;
		}
	}
	[HarmonyPatch(typeof(GSMUITickMgr), "DoGSTicking")]
	[HarmonyPatchCategory("Connected")]
	public static class PatchDoGSTicking
	{
		public static void Prefix(List<IGSMUITickable> ___TickingQueue)
		{
			for (int num = ___TickingQueue.Count - 1; num >= 0; num--)
			{
				if (___TickingQueue[num] == null)
				{
					___TickingQueue.RemoveAt(num);
				}
			}
		}
	}
	[HarmonyPatch(typeof(BGW_GameDB), "GetUnitBattleInfoExtendDesc")]
	[HarmonyPatchCategory("Global")]
	public static class PatchIsStandAlone
	{
		public static void Postfix(ref FUStUnitBattleInfoExtendDesc? __result)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			FUStUnitBattleInfoExtendDesc val = __result;
			if (val != null && (int)val.BloodBarType == 4)
			{
				__result.BloodBarType = (EBGUBloodBarType)2;
			}
		}
	}
	[HarmonyPatch(typeof(GSPlayerDataMgr), "OnTick")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGSPlayerDataMgrOnTick
	{
		public static bool Prefix(float DeltaTime)
		{
			PatchVISimTipsOnTick.OriginalDeltaTime = DeltaTime;
			return true;
		}
	}
	[HarmonyPatch(typeof(VISimTips), "OnTick")]
	[HarmonyPatchCategory("Global")]
	public static class PatchVISimTipsOnTick
	{
		public static float OriginalDeltaTime;

		public static bool Prefix(VISimTips __instance, float DeltaTime, DSTipsData ___DataStore)
		{
			DSSimTipsData val = (DSSimTipsData)(object)((___DataStore is DSSimTipsData) ? ___DataStore : null);
			if (val != null && ((DSTipsData)val).IsShowing.Value && !val.IsCloseAutoHide)
			{
				if (val.ShowTime.Value <= 0f)
				{
					((VITips)__instance).FadeOut();
				}
				else
				{
					val.SetShowTime(val.ShowTime.Value - OriginalDeltaTime);
				}
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(UObject), "GetName")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGetName
	{
		public static bool Prefix(UObject? __instance, ref string? __result)
		{
			if (__instance == (UObject)null)
			{
				Logging.LogError("Trying to call GetName on invalid UObject");
				__result = "Invalid";
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(UObject), "GetPathName")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGetPathName
	{
		public static bool Prefix(UObject? __instance, ref string? __result)
		{
			if (__instance == (UObject)null)
			{
				Logging.LogError("Trying to call GetPathName on invalid UObject");
				__result = "Invalid";
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(UObject), "GetFullName")]
	[HarmonyPatchCategory("Global")]
	public static class PatchGetFullName
	{
		public static bool Prefix(UObject? __instance, ref string? __result)
		{
			if (__instance == (UObject)null)
			{
				Logging.LogError("Trying to call GetFullName on invalid UObject");
				__result = "Invalid";
				return false;
			}
			return true;
		}
	}
}
namespace WukongMp.Api.PathCompressors
{
	public class NameCompressor
	{
		private readonly string _commonFolder;

		private readonly Regex _longNameRegex;

		private readonly Regex _shortNameRegex;

		public NameCompressor(string commonFolder, Regex longNameRegex, Regex shortNameRegex)
		{
			_commonFolder = commonFolder;
			_longNameRegex = longNameRegex;
			_shortNameRegex = shortNameRegex;
		}

		public bool Compress(string? fullName, out string shortName)
		{
			if (fullName == null)
			{
				shortName = "";
				return false;
			}
			Match match = _longNameRegex.Match(fullName);
			if (match.Success)
			{
				if (match.Groups[2].Value != match.Groups[3].Value)
				{
					Logging.LogError("Found full name with mismatched package/asset name: {FullName}", fullName);
					shortName = "";
					return false;
				}
				shortName = match.Groups[1].Value + "/" + match.Groups[2].Value;
				return true;
			}
			Logging.LogDebug("Failed to compress asset name: {FullName}", fullName);
			shortName = "";
			return false;
		}

		public string Decompress(string shortName)
		{
			Match match = _shortNameRegex.Match(shortName);
			if (match.Success)
			{
				return _commonFolder + "/" + match.Groups[1].Value + "/" + match.Groups[2].Value + "." + match.Groups[2].Value;
			}
			Logging.LogError("Failed to decompress asset name: {ShortName}", shortName);
			return "";
		}
	}
}
namespace WukongMp.Api.NameCompressors
{
	public static class Compressors
	{
		public static readonly NameCompressor MontageNameCompressor = new NameCompressor("/Game/00Main/Animation", new Regex("/Game/00Main/Animation/([\\w/]+)/(\\w+)\\.(\\w+)", RegexOptions.Compiled), new Regex("([\\w/]+)/(\\w+)", RegexOptions.Compiled));

		public static readonly NameCompressor VigorNameCompressor = new NameCompressor("/Game/00MainHZ/Characters/Transform/VigorSkill", new Regex("/Game/00MainHZ/Characters/Transform/VigorSkill/([\\w/]+)/(\\w+)\\.(\\w+)", RegexOptions.Compiled), new Regex("([\\w/]+)/(\\w+)", RegexOptions.Compiled));
	}
}
namespace WukongMp.Api.Monitors
{
	internal class ComponentMonitor
	{
		private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

		private readonly string _componentName;

		private readonly object? _component;

		internal ComponentMonitor(object component, string componentName)
		{
			_componentName = componentName;
			_component = component;
			foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(component))
			{
				if (property != null)
				{
					object value = property.GetValue(component);
					if (value != null)
					{
						string name = property.Name;
						_properties[name] = value;
					}
				}
			}
		}

		internal void Update()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			if (_component == null)
			{
				return;
			}
			foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(_component))
			{
				if (property == null || property.PropertyType == null)
				{
					continue;
				}
				object value = property.GetValue(_component);
				if (value != null)
				{
					string name = property.Name;
					if (_properties.TryGetValue(name, out object value2) && value2 != null && ((value is FVector val && !((FVector)(ref val)).Equals((FVector)_properties[name], 50.0)) || (value is FRotator val2 && !((FRotator)(ref val2)).Equals((FRotator)_properties[name], 10.0)) || (value is float a && !USharpExtensions.Equals(a, (float)_properties[name], 1f)) || (!property.PropertyType.IsAssignableFrom(typeof(FVector)) && !property.PropertyType.IsAssignableFrom(typeof(FRotator)) && !property.PropertyType.IsAssignableFrom(typeof(float)) && !value.Equals(_properties[name]))))
					{
						Logging.LogDebug("[{Component}] Property {Name} changed from {OldValue} to {NewValue}", _componentName, name, _properties[name].ToString(), value.ToString());
						_properties[name] = value;
					}
				}
			}
		}
	}
	public class ComponentMonitorManager
	{
		private readonly List<ComponentMonitor> _componentMonitors = new List<ComponentMonitor>();

		public static ComponentMonitorManager Instance { get; } = new ComponentMonitorManager();

		private ComponentMonitorManager()
		{
		}

		public void AddComponentMonitor(object component, string componentName)
		{
			_componentMonitors.Add(new ComponentMonitor(component, componentName));
		}

		public void Update()
		{
			foreach (ComponentMonitor componentMonitor in _componentMonitors)
			{
				componentMonitor.Update();
			}
		}
	}
}
namespace WukongMp.Api.Input
{
	public class WukongInputManager
	{
		private readonly WukongCommandConsole _commandConsole;

		private readonly WukongChatter _chatter;

		private readonly WukongWidgetManager _widgetManager;

		public WukongInputManager(WukongCommandConsole commandConsole, WukongChatter chatter, WukongWidgetManager widgetManager)
		{
			_commandConsole = commandConsole;
			_chatter = chatter;
			_widgetManager = widgetManager;
		}

		public void HandleEnterPressed()
		{
			if (_widgetManager.IsCommandVisible())
			{
				if (!_widgetManager.CommandHasFocus())
				{
					_widgetManager.SetCommandInputFocus();
					return;
				}
				string command = _widgetManager.CommitCommand();
				_commandConsole.ProcessCommand(command);
			}
			else if (!_widgetManager.ChatHasFocus())
			{
				_widgetManager.SetChatInputFocus();
			}
			else
			{
				string message = _widgetManager.CommitChatMessage();
				_chatter.ProcessMessage(message);
			}
		}

		public bool CanApplyInput()
		{
			if (!_widgetManager.ChatHasFocus())
			{
				return !_widgetManager.CommandHasFocus();
			}
			return false;
		}
	}
}
namespace WukongMp.Api.Https
{
	public class BouncyCastleHttpsClient(ILogger logger)
	{
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			PropertyNameCaseInsensitive = true
		};

		public async Task<T?> GetAsync<T>(Uri url, Dictionary<string, string>? headers = null, CancellationToken ct = default(CancellationToken))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			await _semaphore.WaitAsync(ct);
			try
			{
				HttpRequestResponse httpRequestResponse = await GetRawAsync(url, headers, ct);
				bool flag = (object)httpRequestResponse == null;
				if (!flag)
				{
					int statusCode = httpRequestResponse.StatusCode;
					bool flag2 = ((statusCode < 200 || statusCode >= 300) ? true : false);
					flag = flag2;
				}
				if (flag)
				{
					return default(T);
				}
				using Stream body = GetResponseBody(httpRequestResponse);
				return await JsonSerializer.DeserializeAsync<T>(body, JsonOptions, ct);
			}
			finally
			{
				_semaphore.Release();
				logger.LogInformation("GET {Url} completed in {ElapsedMilliseconds} ms", url.ToString(), stopwatch.ElapsedMilliseconds);
			}
		}

		public async Task<byte[]?> GetBytesAsync(Uri url, Dictionary<string, string>? headers = null, CancellationToken ct = default(CancellationToken))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			await _semaphore.WaitAsync(ct);
			try
			{
				HttpRequestResponse httpRequestResponse = await GetRawAsync(url, headers, ct);
				bool flag = (object)httpRequestResponse == null;
				if (!flag)
				{
					int statusCode = httpRequestResponse.StatusCode;
					bool flag2 = ((statusCode < 200 || statusCode >= 300) ? true : false);
					flag = flag2;
				}
				if (flag)
				{
					return null;
				}
				using Stream body = GetResponseBody(httpRequestResponse);
				using MemoryStream ms = new MemoryStream();
				await body.CopyToAsync(ms);
				return ms.ToArray();
			}
			finally
			{
				_semaphore.Release();
				logger.LogInformation("GET bytes from {Url} completed in {ElapsedMilliseconds} ms", url.ToString(), stopwatch.ElapsedMilliseconds);
			}
		}

		public async Task<HttpStatusCode> PutBytesAsync(Uri url, Dictionary<string, string> headers, byte[] fileBytes, CancellationToken ct = default(CancellationToken))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			await _semaphore.WaitAsync(ct);
			try
			{
				using TcpClient tcp = new TcpClient(url.Host, url.Port);
				using NetworkStream stream = tcp.GetStream();
				Stream requestStream = stream;
				if (url.Scheme == "https")
				{
					BcTlsCrypto crypto = new BcTlsCrypto();
					BouncyCastleTlsClient tlsClient = new BouncyCastleTlsClient(url.Host, crypto);
					TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(stream);
					tlsClientProtocol.Connect(tlsClient);
					requestStream = tlsClientProtocol.Stream;
				}
				using StreamWriter writer = new StreamWriter(requestStream);
				writer.NewLine = "\r\n";
				await writer.WriteLineAsync("PUT " + url.PathAndQuery + " HTTP/1.1");
				if ((object)url != null)
				{
					string scheme = url.Scheme;
					if (!(scheme == "http"))
					{
						if (scheme == "https" && url.Port == 443)
						{
							goto IL_0262;
						}
					}
					else if (url.Port == 80)
					{
						goto IL_0262;
					}
				}
				bool flag = false;
				goto IL_026a;
				IL_026a:
				if (!flag)
				{
					await writer.WriteLineAsync($"Host: {url.Host}:{url.Port}");
				}
				else
				{
					await writer.WriteLineAsync("Host: " + url.Host);
				}
				await writer.WriteLineAsync("Connection: close");
				await writer.WriteLineAsync("Accept: application/xml, application/json, text/plain, */*");
				await writer.WriteLineAsync($"Content-Length: {fileBytes.Length}");
				await writer.WriteLineAsync("Content-Type: application/octet-stream");
				foreach (KeyValuePair<string, string> header in headers)
				{
					await writer.WriteLineAsync(header.Key + ": " + header.Value);
				}
				await writer.WriteLineAsync();
				await writer.FlushAsync();
				await requestStream.WriteAsync(fileBytes, 0, fileBytes.Length, ct);
				await requestStream.FlushAsync(ct);
				using (HttpParserDelegate handler = new HttpParserDelegate())
				{
					using HttpCombinedParser parser = new HttpCombinedParser(handler);
					byte[] buffer = new byte[8192];
					Unsafe.SkipInit(out Socket client);
					while (!ct.IsCancellationRequested)
					{
						int num;
						try
						{
							num = await requestStream.ReadAsync(buffer, 0, buffer.Length, ct);
						}
						catch (TlsNoCloseNotifyException)
						{
							break;
						}
						catch (IOException) when (((Func<bool>)delegate
						{
							// Could not convert BlockContainer to single expression
							client = tcp.Client;
							return client != null && !client.Connected;
						}).Invoke())
						{
							break;
						}
						if (num == 0)
						{
							break;
						}
						using MemoryStream buff = new MemoryStream(buffer, 0, num, writable: false);
						parser.Execute(buff);
						if (!parser.ShouldKeepAlive)
						{
							if ((object)handler.HttpRequestResponse == null)
							{
								handler.OnMessageEnd(parser);
							}
							break;
						}
						if ((object)handler.HttpRequestResponse != null)
						{
							break;
						}
					}
					if ((object)handler.HttpRequestResponse == null)
					{
						logger.LogWarning("No valid HTTP response received for PUT to {Url}", url.ToString());
						return HttpStatusCode.ServiceUnavailable;
					}
					return (HttpStatusCode)handler.HttpRequestResponse.StatusCode;
				}
				IL_0262:
				flag = true;
				goto IL_026a;
			}
			finally
			{
				_semaphore.Release();
				logger.LogInformation("PUT bytes to {Url} completed in {ElapsedMilliseconds} ms", url.ToString(), stopwatch.ElapsedMilliseconds);
			}
		}

		public async Task<HttpStatusCode> PutMultipartAsync(Uri url, Dictionary<string, object>? fields, string fileFieldName, string fileName, byte[] fileBytes, Dictionary<string, string>? headers = null, CancellationToken ct = default(CancellationToken))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			await _semaphore.WaitAsync(ct);
			try
			{
				using TcpClient tcp = new TcpClient(url.Host, url.Port);
				using NetworkStream stream = tcp.GetStream();
				Stream requestStream = stream;
				if (url.Scheme == "https")
				{
					BcTlsCrypto crypto = new BcTlsCrypto();
					BouncyCastleTlsClient tlsClient = new BouncyCastleTlsClient(url.Host, crypto);
					TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(stream);
					tlsClientProtocol.Connect(tlsClient);
					requestStream = tlsClientProtocol.Stream;
				}
				using StreamWriter writer = new StreamWriter(requestStream);
				writer.NewLine = "\r\n";
				string boundary = $"----BOUNDARY{DateTime.UtcNow.Ticks}";
				using MemoryStream bodyStream = new MemoryStream();
				byte[] newline = "\r\n"u8.ToArray();
				if (fields != null)
				{
					foreach (KeyValuePair<string, object> field in fields)
					{
						byte[] bytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\nContent-Disposition: form-data; name=\"" + field.Key + "\"\r\n\r\n");
						byte[] valueBytes = Encoding.UTF8.GetBytes(field.Value.ToString() ?? "");
						await bodyStream.WriteAsync(bytes, 0, bytes.Length, ct);
						await bodyStream.WriteAsync(valueBytes, 0, valueBytes.Length, ct);
						await bodyStream.WriteAsync(newline, 0, newline.Length, ct);
					}
				}
				byte[] bytes2 = Encoding.UTF8.GetBytes("--" + boundary + "\r\nContent-Disposition: form-data; name=\"" + fileFieldName + "\"; filename=\"" + fileName + "\"\r\nContent-Type: application/octet-stream\r\nContent-Encoding: gzip\r\n\r\n");
				byte[] endBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
				await bodyStream.WriteAsync(bytes2, 0, bytes2.Length, ct);
				using (GZipStream gzip = new GZipStream(bodyStream, CompressionLevel.Optimal, leaveOpen: true))
				{
					await gzip.WriteAsync(fileBytes, 0, fileBytes.Length, ct);
				}
				await bodyStream.WriteAsync(endBoundary, 0, endBoundary.Length, ct);
				long contentLength = bodyStream.Length;
				bodyStream.Position = 0L;
				await writer.WriteLineAsync("PUT " + url.PathAndQuery + " HTTP/1.1");
				await writer.WriteLineAsync($"Host: {url.Host}:{url.Port}");
				await writer.WriteLineAsync("Connection: close");
				await writer.WriteLineAsync("Content-Type: multipart/form-data; boundary=" + boundary);
				await writer.WriteLineAsync($"Content-Length: {contentLength}");
				if (headers != null)
				{
					foreach (KeyValuePair<string, string> header in headers)
					{
						await writer.WriteLineAsync(header.Key + ": " + header.Value);
					}
				}
				await writer.WriteLineAsync();
				await writer.FlushAsync();
				await bodyStream.CopyToAsync(requestStream);
				await requestStream.FlushAsync(ct);
				using HttpParserDelegate handler = new HttpParserDelegate();
				using HttpCombinedParser parser = new HttpCombinedParser(handler);
				MemoryStream responseStream = new MemoryStream();
				try
				{
					await requestStream.CopyToAsync(responseStream);
				}
				catch (TlsNoCloseNotifyException)
				{
				}
				parser.Execute(responseStream);
				if ((object)handler.HttpRequestResponse == null)
				{
					return HttpStatusCode.NoContent;
				}
				return (HttpStatusCode)handler.HttpRequestResponse.StatusCode;
			}
			finally
			{
				_semaphore.Release();
				logger.LogInformation("PUT multipart to {Url} completed in {ElapsedMilliseconds} ms", url.ToString(), stopwatch.ElapsedMilliseconds);
			}
		}

		private async Task<HttpRequestResponse?> GetRawAsync(Uri url, Dictionary<string, string>? headers = null, CancellationToken ct = default(CancellationToken))
		{
			using TcpClient tcp = new TcpClient(url.Host, url.Port);
			using NetworkStream stream = tcp.GetStream();
			Stream requestStream = stream;
			if (url.Scheme == "https")
			{
				BcTlsCrypto crypto = new BcTlsCrypto();
				BouncyCastleTlsClient tlsClient = new BouncyCastleTlsClient(url.Host, crypto);
				TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(stream);
				tlsClientProtocol.Connect(tlsClient);
				requestStream = tlsClientProtocol.Stream;
			}
			using StreamWriter writer = new StreamWriter(requestStream);
			writer.NewLine = "\r\n";
			await writer.WriteLineAsync("GET " + url.PathAndQuery + " HTTP/1.1");
			await writer.WriteLineAsync($"Host: {url.Host}:{url.Port}");
			await writer.WriteLineAsync("Connection: close");
			if (headers != null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					await writer.WriteLineAsync(header.Key + ": " + header.Value);
				}
			}
			await writer.WriteLineAsync();
			await writer.FlushAsync();
			using HttpParserDelegate handler = new HttpParserDelegate();
			using HttpCombinedParser parser = new HttpCombinedParser(handler);
			byte[] buffer = new byte[8192];
			Unsafe.SkipInit(out Socket client);
			while (!ct.IsCancellationRequested)
			{
				int num;
				try
				{
					num = await requestStream.ReadAsync(buffer, 0, buffer.Length, ct);
				}
				catch (IOException) when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					client = tcp.Client;
					return client != null && !client.Connected;
				}).Invoke())
				{
					break;
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (TlsNoCloseNotifyException)
				{
					break;
				}
				if (num == 0)
				{
					break;
				}
				using MemoryStream buff = new MemoryStream(buffer, 0, num, writable: false);
				parser.Execute(buff);
				if ((object)handler.HttpRequestResponse != null && (!handler.HttpRequestResponse.Headers.TryGetValue("CONTENT-LENGTH", out var value) || !long.TryParse(value.FirstOrDefault(), out var result) || handler.HttpRequestResponse.Body.Length >= result))
				{
					break;
				}
			}
			if ((object)handler.HttpRequestResponse == null)
			{
				return null;
			}
			handler.HttpRequestResponse.Body.Position = 0L;
			return handler.HttpRequestResponse;
		}

		private static Stream GetResponseBody(HttpRequestResponse response)
		{
			if (!response.Headers.TryGetValue("CONTENT-ENCODING", out var value))
			{
				return response.Body;
			}
			string text = value.First().ToLowerInvariant();
			if (!(text == "gzip"))
			{
				if (text == "deflate")
				{
					return new DeflateStream(response.Body, CompressionMode.Decompress, leaveOpen: false);
				}
				throw new NotSupportedException("Unsupported content encoding: " + text);
			}
			return new GZipStream(response.Body, CompressionMode.Decompress, leaveOpen: false);
		}
	}
	internal class BouncyCastleTlsAuthentication : TlsAuthentication
	{
		private static X509Certificate[] _trustedRoots;

		private readonly string _host;

		public BouncyCastleTlsAuthentication(string host)
		{
			_host = host;
			if (_trustedRoots != null)
			{
				return;
			}
			using FileStream stream = File.OpenRead(Path.Combine(GameSaveUtils.GetModsDirectory(), "WukongMp.Coop", "cacert.pem"));
			using StreamReader reader = new StreamReader(stream);
			PemReader pemReader = new PemReader(reader);
			List<X509Certificate> list = new List<X509Certificate>();
			while (true)
			{
				object obj = pemReader.ReadObject();
				if (obj == null)
				{
					break;
				}
				if (obj is X509Certificate item)
				{
					list.Add(item);
				}
			}
			_trustedRoots = list.ToArray();
		}

		public void NotifyServerCertificate(TlsServerCertificate serverCertificate)
		{
			ValidateServerCertificate(serverCertificate.Certificate.GetCertificateList());
		}

		public TlsCredentials? GetClientCredentials(CertificateRequest certificateRequest)
		{
			return null;
		}

		private void ValidateServerCertificate(TlsCertificate[] chain)
		{
			if (chain.Length == 0)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateParser parser = new X509CertificateParser();
			List<X509Certificate> list = chain.Select((TlsCertificate c) => parser.ReadCertificate(c.GetEncoded())).ToList();
			IEnumerable<int[]> permutations = GetPermutations(Enumerable.Range(0, list.Count).ToArray());
			bool flag = false;
			X509Certificate x509Certificate = null;
			X509Certificate x509Certificate2 = null;
			foreach (int[] item in permutations)
			{
				try
				{
					for (int num = 0; num < item.Length - 1; num++)
					{
						list[item[num]].Verify(list[item[num + 1]].GetPublicKey());
					}
					flag = true;
					x509Certificate2 = list[item[0]];
					x509Certificate = list[item[^1]];
				}
				catch
				{
					continue;
				}
				break;
			}
			if (!flag || x509Certificate == null || x509Certificate2 == null)
			{
				throw new TlsFatalAlert(42);
			}
			bool flag2 = false;
			X509Certificate[] trustedRoots = _trustedRoots;
			foreach (X509Certificate x509Certificate3 in trustedRoots)
			{
				try
				{
					x509Certificate.Verify(x509Certificate3.GetPublicKey());
					flag2 = true;
				}
				catch
				{
					continue;
				}
				break;
			}
			if (!flag2)
			{
				throw new TlsFatalAlert(42);
			}
			if (!VerifyHostname(x509Certificate2))
			{
				throw new TlsFatalAlert(42);
			}
		}

		private static IEnumerable<int[]> GetPermutations(int[] list)
		{
			if (list.Length == 1)
			{
				yield return list;
				yield break;
			}
			int i;
			for (i = 0; i < list.Length; i++)
			{
				int current = list[i];
				int[] list2 = list.Where((int _, int index) => index != i).ToArray();
				foreach (int[] permutation in GetPermutations(list2))
				{
					yield return new int[1] { current }.Concat(permutation).ToArray();
				}
			}
		}

		private bool VerifyHostname(X509Certificate cert)
		{
			Asn1OctetString extensionValue = cert.GetExtensionValue(X509Extensions.SubjectAlternativeName);
			if (extensionValue != null)
			{
				foreach (Asn1Encodable item in Asn1Sequence.GetInstance(Asn1Object.FromByteArray(extensionValue.GetOctets())))
				{
					GeneralName instance = GeneralName.GetInstance(item);
					if (instance.TagNo == 2)
					{
						string value = instance.Name.ToString().Replace("*", "");
						if (_host.EndsWith(value))
						{
							return true;
						}
					}
				}
			}
			IList<string> valueList = cert.SubjectDN.GetValueList(X509Name.CN);
			if (valueList != null)
			{
				foreach (string item2 in valueList)
				{
					if (string.Equals(_host, item2, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
	internal class BouncyCastleTlsClient : DefaultTlsClient
	{
		public BouncyCastleTlsClient(string host, TlsCrypto crypto)
		{
			<host>P = host;
			base..ctor(crypto);
		}

		public override TlsAuthentication GetAuthentication()
		{
			return new BouncyCastleTlsAuthentication(<host>P);
		}

		public override int[] GetCipherSuites()
		{
			return new int[4] { 49199, 49200, 49195, 49196 };
		}

		protected override ProtocolVersion[] GetSupportedVersions()
		{
			return new ProtocolVersion[1] { ProtocolVersion.TLSv12 };
		}

		public override IDictionary<int, byte[]> GetClientExtensions()
		{
			IDictionary<int, byte[]> obj = base.GetClientExtensions() ?? new Dictionary<int, byte[]>();
			TlsExtensionsUtilities.AddServerNameExtensionClient(obj, new List<ServerName>(1)
			{
				new ServerName(0, Encoding.UTF8.GetBytes(<host>P))
			});
			return obj;
		}

		public override void NotifyConnectionClosed()
		{
		}
	}
	public class DownloadServerFileResponse
	{
		public string DownloadUrl { get; set; }
	}
	public class HttpBlobClient(ILogger logger) : IBlobClient
	{
		private enum FileType
		{
			WorldSave,
			PlayerSave
		}

		public async Task<bool> UploadBlobAsync(BlobInfo blob, CancellationToken ct = default(CancellationToken))
		{
			BouncyCastleHttpsClient client = new BouncyCastleHttpsClient(logger);
			int serverId = LaunchParameters.Instance.ServerId.Value;
			FileType fileType = ((!(blob.Name == "world.sav")) ? FileType.PlayerSave : FileType.WorldSave);
			Guid? guid = null;
			if (fileType == FileType.PlayerSave)
			{
				string[] array = blob.Name.Split('_', '.');
				if (array.Length == 3 && Guid.TryParse(array[1], out var result))
				{
					guid = result;
				}
			}
			string arg = $"?kind={fileType}&userGuid={guid}&serverId={serverId}";
			Uri url = new Uri($"{LaunchParameters.Instance.ApiBaseUrl}/api/server/{serverId}/files/upload-sas{arg}");
			string uploadUrl = await client.GetAsync<string>(url, new Dictionary<string, string> { 
			{
				"Authorization",
				"Bearer " + LaunchParameters.Instance.JwtToken
			} }, ct);
			if (uploadUrl != null)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					using (GZipStream gzip = new GZipStream(stream, CompressionLevel.Optimal, leaveOpen: true))
					{
						await gzip.WriteAsync(blob.Content, 0, blob.Content.Length, ct);
					}
					byte[] array2 = stream.ToArray();
					byte[] inArray = MD5.Create().ComputeHash(array2);
					Uri url2 = new Uri(uploadUrl);
					Dictionary<string, string> headers = new Dictionary<string, string>
					{
						{ "x-ms-blob-type", "BlockBlob" },
						{ "x-ms-version", "2025-07-05" },
						{ "x-ms-blob-content-encoding", "gzip" },
						{
							"Content-MD5",
							Convert.ToBase64String(inArray)
						}
					};
					HttpStatusCode httpStatusCode = await client.PutBytesAsync(url2, headers, array2, ct);
					return httpStatusCode >= HttpStatusCode.OK && httpStatusCode < HttpStatusCode.MultipleChoices;
				}
			}
			logger.LogError("Failed to get upload URL for blob '{BlobName}' for server {ServerId}", blob.Name, serverId);
			return false;
		}

		public async Task<BlobInfo?> DownloadBlobAsync(string name, CancellationToken ct = default(CancellationToken))
		{
			int serverId = LaunchParameters.Instance.ServerId.Value;
			string arg = Uri.EscapeDataString(name);
			BouncyCastleHttpsClient client = new BouncyCastleHttpsClient(logger);
			Uri url = new Uri($"{LaunchParameters.Instance.ApiBaseUrl}/api/server/{serverId}/files/{arg}");
			DownloadServerFileResponse downloadServerFileResponse = await client.GetAsync<DownloadServerFileResponse>(url, new Dictionary<string, string> { 
			{
				"Authorization",
				"Bearer " + LaunchParameters.Instance.JwtToken
			} }, ct);
			if (string.IsNullOrWhiteSpace(downloadServerFileResponse?.DownloadUrl))
			{
				logger.LogWarning("Failed to get download URL for blob '{BlobName}' for server {ServerId}", name, serverId);
				return null;
			}
			Uri url2 = new Uri(downloadServerFileResponse.DownloadUrl);
			byte[] array = await client.GetBytesAsync(url2, null, ct);
			if (array == null)
			{
				logger.LogError("Failed to download blob content '{BlobName}' for server {ServerId}", name, serverId);
				return null;
			}
			return new BlobInfo(name, array);
		}
	}
	[Obsolete("Superseded by direct SAS URL upload")]
	public class OldHttpBlobClient(ILogger logger) : IBlobClient
	{
		public async Task<bool> UploadBlobAsync(BlobInfo blob, CancellationToken ct = default(CancellationToken))
		{
			int value = LaunchParameters.Instance.ServerId.Value;
			string arg = Uri.EscapeDataString(blob.Name);
			BouncyCastleHttpsClient bouncyCastleHttpsClient = new BouncyCastleHttpsClient(logger);
			Uri url = new Uri($"{LaunchParameters.Instance.ApiBaseUrl}/api/server/{value}/files/{arg}");
			HttpStatusCode httpStatusCode = await bouncyCastleHttpsClient.PutMultipartAsync(url, new Dictionary<string, object>(), "file", blob.Name, blob.Content, new Dictionary<string, string> { 
			{
				"Authorization",
				"Bearer " + LaunchParameters.Instance.JwtToken
			} }, ct);
			return httpStatusCode >= HttpStatusCode.OK && httpStatusCode < HttpStatusCode.MultipleChoices;
		}

		public async Task<BlobInfo?> DownloadBlobAsync(string name, CancellationToken ct = default(CancellationToken))
		{
			int serverId = LaunchParameters.Instance.ServerId.Value;
			string arg = Uri.EscapeDataString(name);
			BouncyCastleHttpsClient client = new BouncyCastleHttpsClient(logger);
			Uri url = new Uri($"{LaunchParameters.Instance.ApiBaseUrl}/api/server/{serverId}/files/{arg}");
			DownloadServerFileResponse downloadServerFileResponse = await client.GetAsync<DownloadServerFileResponse>(url, new Dictionary<string, string> { 
			{
				"Authorization",
				"Bearer " + LaunchParameters.Instance.JwtToken
			} }, ct);
			if (string.IsNullOrWhiteSpace(downloadServerFileResponse?.DownloadUrl))
			{
				logger.LogWarning("Failed to get download URL for blob '{BlobName}' for server {ServerId}", name, serverId);
				return null;
			}
			Uri url2 = new Uri(downloadServerFileResponse.DownloadUrl);
			byte[] array = await client.GetBytesAsync(url2, null, ct);
			if (array == null)
			{
				logger.LogError("Failed to download blob content '{BlobName}' for server {ServerId}", name, serverId);
				return null;
			}
			return new BlobInfo(name, array);
		}
	}
}
namespace WukongMp.Api.Helpers
{
	public class CountdownTimer
	{
		private int _remainingSeconds;

		private int _totalSeconds;

		private readonly System.Timers.Timer _timer;

		private Action? _callback;

		private Action<int, int>? _onTickCallback;

		public CountdownTimer(int minutes, int seconds)
		{
			_totalSeconds = minutes * 60 + seconds;
			_remainingSeconds = _totalSeconds;
			_timer = new System.Timers.Timer(1000.0);
			_timer.Elapsed += TimerElapsed;
		}

		public void SetTime(int minutes, int seconds)
		{
			_totalSeconds = minutes * 60 + seconds;
			Reset();
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (_remainingSeconds > 0)
			{
				_remainingSeconds--;
				_onTickCallback?.Invoke(_remainingSeconds / 60, _remainingSeconds % 60);
			}
			else
			{
				Stop();
				_callback?.Invoke();
			}
		}

		public void Start(Action onFinishedCallback, Action<int, int> onTickCallback)
		{
			_timer.Start();
			_callback = onFinishedCallback;
			_onTickCallback = onTickCallback;
		}

		public void Stop()
		{
			_timer.Stop();
		}

		public void Reset()
		{
			Stop();
			_callback = null;
			_onTickCallback = null;
			_remainingSeconds = _totalSeconds;
		}
	}
	public sealed class EnumSet<T> : IEnumerable<T>, IEnumerable where T : Enum
	{
		private readonly byte[] _flags = new byte[32];

		private EnumSet()
		{
			if (Enum.GetValues(typeof(T)).Length > 256)
			{
				throw new ArgumentException("EnumSet only supports enums with a maximum of 256 values");
			}
		}

		public EnumSet(IEnumerable<T> initial)
			: this()
		{
			foreach (T item in initial)
			{
				Add(item);
			}
		}

		public void Add(T value)
		{
			int num = Convert.ToInt32(value);
			_flags[num / 8] |= (byte)(1 << num % 8);
		}

		public void Remove(T value)
		{
			int num = Convert.ToInt32(value);
			_flags[num / 8] &= (byte)(~(1 << num % 8));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(T value)
		{
			int num = Convert.ToInt32(value);
			return (_flags[num / 8] & (1 << num % 8)) != 0;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < _flags.Length; i++)
			{
				byte flag = _flags[i];
				for (int j = 0; j < 8; j++)
				{
					if ((flag & (1 << j)) != 0)
					{
						yield return (T)Enum.ToObject(typeof(T), i * 8 + j);
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	public class TimerController
	{
		private readonly WukongWidgetManager _widgetManager;

		private CountdownTimer? _timer;

		private int _initialMinutes;

		private int _initialSeconds;

		public TimerController(WukongWidgetManager widgetManager)
		{
			_widgetManager = widgetManager;
		}

		public void SetTimer(int minutes, int seconds)
		{
			_initialMinutes = minutes;
			_initialSeconds = seconds;
			_timer = new CountdownTimer(minutes, seconds);
			_widgetManager.SetTimerVisibility(visible: true);
			_widgetManager.SetTimerText(_initialMinutes, _initialSeconds);
		}

		public void StartTimer()
		{
			_timer?.Start(OnTimerFinished, OnTimerTick);
		}

		private void OnTimerTick(int minutes, int seconds)
		{
			_widgetManager.SetTimerText(minutes, seconds);
		}

		private void OnTimerFinished()
		{
			_widgetManager.SetTimerText(0, 0);
			_widgetManager.SetTimerVisibility(visible: false);
		}

		public void StopTimer()
		{
			_timer?.Stop();
		}

		public void ResetTimer()
		{
			_timer?.Reset();
			_widgetManager.SetTimerVisibility(visible: true);
			_widgetManager.SetTimerText(_initialMinutes, _initialSeconds);
		}
	}
}
namespace WukongMp.Api.FreeCamera
{
	public class FreeCameraController : IDisposable
	{
		private float _rotateDirLR;

		private float _rotateDirUD;

		private float _moveDirLR;

		private float _moveDirUD;

		private float _moveDirFB;

		private float _orbitYaw;

		private float _currentPlayerOrbitYaw;

		private FVector _targetLocation;

		private float _orbitPitch;

		private float _orbitDistance = 1000f;

		private const float MoveSpeed = 1000f;

		private const float RotateSpeed = 3000f;

		private const float OrbitDistanceMin = 200f;

		private const float OrbitDistanceMax = 2000f;

		private const float OrbitDistanceSpeed = 500f;

		private const float OrbitPitchSpeed = 60f;

		private const float OrbitYawSpeed = 120f;

		private const float OrbitYawFollowSpeed = 2f;

		private const float MouseOrbitSensitivity = 100f;

		private bool _isDragging;

		private FVector2D _lastDragPos;

		private MainCharacterEntity _spectatedEntity;

		private readonly FreeCameraManager _freeCameraManager;

		private readonly WukongPlayerState _playerState;

		private readonly ClientState _state;

		private readonly WukongWidgetManager _widgetManager;

		private int _currentSpectatedIndex = -1;

		private IEnumerable<PlayerId> AllNotSpectatingPlayerIds => _state.AreaPlayers.Where(delegate(PlayerId p)
		{
			MainCharacterEntity? mainCharacterById = _playerState.GetMainCharacterById(p);
			return mainCharacterById.HasValue && !mainCharacterById.GetValueOrDefault().GetPvP().IsSpectator;
		});

		private IEnumerable<(PlayerId PlayerId, PlayerEntity Player, MainCharacterEntity Character)> AllPvPPlayers => AllNotSpectatingPlayerIds.Select(GetEntities).OfType<(PlayerId, PlayerEntity, MainCharacterEntity)>();

		private (PlayerId PlayerId, PlayerEntity Player, MainCharacterEntity Character)? GetEntities(PlayerId playerId)
		{
			PlayerEntity? playerById = _playerState.GetPlayerById(playerId);
			MainCharacterEntity? mainCharacterById = _playerState.GetMainCharacterById(playerId);
			if (!playerById.HasValue || !mainCharacterById.HasValue)
			{
				return null;
			}
			return (playerId, playerById.Value, mainCharacterById.Value);
		}

		public FreeCameraController(ClientState state, WukongPlayerState playerState, InputManager inputManager, FreeCameraManager freeCameraManager, WukongWidgetManager widgetManager)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			_state = state;
			_playerState = playerState;
			_freeCameraManager = freeCameraManager;
			_widgetManager = widgetManager;
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)2, (Action)OnRightMouseStarted, (Action)OnRightMouseCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)87, (Action)OnForwardStarted, (Action)OnForwardCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)83, (Action)OnBackwardStarted, (Action)OnBackwardCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)65, (Action)OnLeftStarted, (Action)OnLeftCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)68, (Action)OnRightStarted, (Action)OnRightCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)69, (Action)OnUpStarted, (Action)OnUpCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)81, (Action)OnDownStarted, (Action)OnDownCompleted));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)39, (Action)OnNextStarted, (Action)null));
			inputManager.RegisterKeyBind(new HotKeyItem((ModifierKeys)0, (Key)37, (Action)OnPrevStarted, (Action)null));
			_freeCameraManager.OnFreeCameraModeChanged += OnFreeCameraModeChanged;
		}

		public void Dispose()
		{
			_freeCameraManager.OnFreeCameraModeChanged -= OnFreeCameraModeChanged;
		}

		public void Update(float DeltaTime)
		{
			ExecMove(DeltaTime);
			ExecRotate(DeltaTime);
		}

		private void ExecMove(float DeltaTime)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (_currentSpectatedIndex == -1)
			{
				FVector val = FVector.UpVector * (double)_moveDirUD;
				FVector val2 = _freeCameraManager.GetForwardVector() * (double)_moveDirFB;
				FVector val3 = _freeCameraManager.GetRightVector() * (double)_moveDirLR;
				FVector moveOffset = (val + val2 + val3) * 1000.0 * (double)DeltaTime;
				if (((FVector)(ref moveOffset)).Size() > 0f)
				{
					_freeCameraManager.MoveFreeCameraActor(moveOffset, isLocal: false);
				}
			}
		}

		private void ExecRotate(float DeltaTime)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			CalculateMouseRotate();
			if (_currentSpectatedIndex == -1)
			{
				if (_rotateDirLR != 0f)
				{
					FRotator rotatorOffset = default(FRotator);
					((FRotator)(ref rotatorOffset))..ctor(0.0, (double)(_rotateDirLR * 3000f * DeltaTime), 0.0);
					_freeCameraManager.RotateFreeCameraActor(rotatorOffset, isLocal: false);
				}
				if (_rotateDirUD != 0f)
				{
					float num = _rotateDirUD * 3000f * DeltaTime;
					float num2 = num + _freeCameraManager.GetFreeCameraActorPitch();
					if (!(num2 > 89f) && !(num2 < -89f))
					{
						FRotator rotatorOffset2 = default(FRotator);
						((FRotator)(ref rotatorOffset2))..ctor((double)num, 0.0, 0.0);
						_freeCameraManager.RotateFreeCameraActor(rotatorOffset2, isLocal: true);
					}
				}
				return;
			}
			if (_spectatedEntity.IsNull)
			{
				UpdateSpectatedPlayer(-1);
				return;
			}
			LocalMainCharacterComponent localState = _spectatedEntity.GetLocalState();
			if ((UObject)(object)localState.Pawn == (UObject)null)
			{
				UpdateSpectatedPlayer(-1);
				return;
			}
			BGUCharacterCS pawn = localState.Pawn;
			float num3 = 0f - _moveDirLR + _rotateDirLR * 100f;
			float num4 = 0f - _moveDirUD + _rotateDirUD * 100f;
			_orbitYaw += num3 * 120f * DeltaTime;
			_orbitPitch = FMath.Clamp(_orbitPitch + num4 * 60f * DeltaTime, -89f, 89f);
			_orbitDistance = FMath.Clamp(_orbitDistance + (0f - _moveDirFB) * 500f * DeltaTime, 200f, 2000f);
			_targetLocation = ((AActor)pawn).GetActorLocation();
			FRotator actorRotation = ((AActor)pawn).GetActorRotation();
			float yaw = ((FRotator)(ref actorRotation)).Yaw;
			_currentPlayerOrbitYaw = MathUtils.LerpAngle(_currentPlayerOrbitYaw, yaw, 1f - (float)Math.Exp(-2f * DeltaTime));
			_freeCameraManager.SetFreeCameraActorTransform(_targetLocation, new FRotator((double)_orbitPitch, (double)(_currentPlayerOrbitYaw + _orbitYaw), 0.0));
			_freeCameraManager.SetSpringArmLength(_orbitDistance);
		}

		private void UpdateSpectatedPlayer(int direction)
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			List<(PlayerId, PlayerEntity, MainCharacterEntity)> list = AllPvPPlayers.ToList();
			if (list.Count == 0)
			{
				DisablePlayerSpectating();
				return;
			}
			_currentSpectatedIndex += direction;
			if (_currentSpectatedIndex < 0)
			{
				DisablePlayerSpectating();
				return;
			}
			_currentSpectatedIndex = FMath.Clamp(_currentSpectatedIndex, 0, list.Count - 1);
			(PlayerId, PlayerEntity, MainCharacterEntity) tuple = list[_currentSpectatedIndex];
			LocalMainCharacterComponent localState = tuple.Item3.GetLocalState();
			if (!localState.HasPawn)
			{
				DisablePlayerSpectating();
				return;
			}
			_spectatedEntity = tuple.Item3;
			BGUCharacterCS pawn = localState.Pawn;
			FVector currentCameraPosition = _freeCameraManager.GetCurrentCameraPosition();
			FVector actorLocation = ((AActor)pawn).GetActorLocation();
			SetInitialOrbitFromCamera(currentCameraPosition, actorLocation, ((AActor)pawn).GetActorRotation());
			_widgetManager.SetSpectatingMessage(tuple.Item3.GetState().CharacterNickName);
		}

		private void DisablePlayerSpectating()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_freeCameraManager.SetFreeCameraActorTransform(_freeCameraManager.GetSpringArmEndTransform());
			_freeCameraManager.SetSpringArmLength(0f);
			_currentSpectatedIndex = -1;
			_widgetManager.HideSpectatingMessage();
		}

		private void SetInitialOrbitFromCamera(FVector cameraPosition, FVector targetPosition, FRotator targetRotation)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			FVector val = cameraPosition - targetPosition;
			float num = ((FVector)(ref val)).Size();
			float yaw = ((FRotator)(ref targetRotation)).Yaw;
			float num2 = 180f + FMath.RadiansToDegrees(FMath.Atan2(((FVector)(ref val)).Y, ((FVector)(ref val)).X)) - yaw;
			num2 = (num2 + 180f) % 360f - 180f;
			float orbitPitch = 0f - FMath.RadiansToDegrees(FMath.Atan2(((FVector)(ref val)).Z, ((FVector)(ref val)).Size2D()));
			_orbitYaw = num2;
			_orbitPitch = orbitPitch;
			_orbitDistance = FMath.Clamp(num, 200f, 2000f);
			_currentPlayerOrbitYaw = yaw;
		}

		public void CalculateMouseRotate()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (_isDragging)
			{
				FVector2D mouseScreenPosition = GSG.BattleLogicSvc.GetMouseScreenPosition();
				FVector2D dragRatio = GetDragRatio(mouseScreenPosition - _lastDragPos);
				_rotateDirLR = ((FVector2D)(ref dragRatio)).X;
				_rotateDirUD = 0f - ((FVector2D)(ref dragRatio)).Y;
				_lastDragPos = mouseScreenPosition;
			}
		}

		private FVector2D GetDragRatio(FVector2D mouseOffset)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			UWorld? world = GameUtils.GetWorld();
			float viewportScale = UWidgetLayoutLibrary.GetViewportScale((UObject)(object)world);
			FVector2D val = UWidgetLayoutLibrary.GetViewportSize((UObject)(object)world) * (double)viewportScale;
			float num = 0f;
			float num2 = 0f;
			if (((FVector2D)(ref val)).X > 0f)
			{
				num = ((FVector2D)(ref mouseOffset)).X / ((FVector2D)(ref val)).X;
			}
			if (((FVector2D)(ref val)).Y > 0f)
			{
				num2 = ((FVector2D)(ref mouseOffset)).Y / ((FVector2D)(ref val)).Y;
			}
			return new FVector2D((double)num, (double)num2);
		}

		private void OnFreeCameraModeChanged(bool enabled)
		{
			if (!enabled)
			{
				DisablePlayerSpectating();
			}
			ResetInput();
		}

		private void ResetInput()
		{
			_isDragging = false;
			_rotateDirLR = 0f;
			_rotateDirUD = 0f;
			_moveDirLR = 0f;
			_moveDirUD = 0f;
			_moveDirFB = 0f;
		}

		private void OnRightMouseStarted()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_isDragging = true;
				_lastDragPos = GSG.BattleLogicSvc.GetMouseScreenPosition();
			}
		}

		private void OnRightMouseCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_isDragging = false;
				_rotateDirLR = 0f;
				_rotateDirUD = 0f;
			}
		}

		private void OnForwardStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirFB = 1f;
			}
		}

		private void OnForwardCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirFB > 0f)
			{
				_moveDirFB = 0f;
			}
		}

		private void OnBackwardStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirFB = -1f;
			}
		}

		private void OnBackwardCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirFB < 0f)
			{
				_moveDirFB = 0f;
			}
		}

		private void OnLeftStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirLR = -1f;
			}
		}

		private void OnLeftCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirLR < 0f)
			{
				_moveDirLR = 0f;
			}
		}

		private void OnRightStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirLR = 1f;
			}
		}

		private void OnRightCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirLR > 0f)
			{
				_moveDirLR = 0f;
			}
		}

		private void OnUpStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirUD = 1f;
			}
		}

		private void OnUpCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirUD > 0f)
			{
				_moveDirUD = 0f;
			}
		}

		private void OnDownStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				_moveDirUD = -1f;
			}
		}

		private void OnDownCompleted()
		{
			if (_freeCameraManager.IsInFreeCameraMode && _moveDirUD < 0f)
			{
				_moveDirUD = 0f;
			}
		}

		private void OnNextStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				UpdateSpectatedPlayer(1);
			}
		}

		private void OnPrevStarted()
		{
			if (_freeCameraManager.IsInFreeCameraMode)
			{
				UpdateSpectatedPlayer(-1);
			}
		}
	}
	public class FreeCameraManager(WukongPlayerState playerState)
	{
		private bool _isInFreeCameraMode;

		private BGUCharacterCS? _cachePlayerPawn;

		private AActor? _freeCameraActor;

		private USpringArmComponent? _springArmComponent;

		private float _gameFov;

		private AActor? _cacheCameraViewTarget;

		private const string FreeCameraActorPath = "/Game/Mods/WukongMod/BP_FreeCameraActor.BP_FreeCameraActor_C";

		public bool IsInFreeCameraMode => _isInFreeCameraMode;

		public event Action<bool>? OnFreeCameraModeChanged;

		public void EnterFreeCameraMode()
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Invalid comparison between Unknown and I4
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null || _isInFreeCameraMode)
			{
				return;
			}
			_cachePlayerPawn = playerState.LocalMainCharacter?.GetLocalState().Pawn;
			if (((UObject?)(object)_cachePlayerPawn).IsNullOrDestroyed())
			{
				Logging.LogError("[FreeCameraManager] EnterFreeCameraMode PlayerPawn IsNull");
				return;
			}
			AController controller = ((APawn)_cachePlayerPawn).GetController();
			ABGPPlayerController val = (ABGPPlayerController)(object)((controller is ABGPPlayerController) ? controller : null);
			if (((UObject?)(object)val).IsNullOrDestroyed())
			{
				Logging.LogError("[FreeCameraManager] EnterFreeCameraMode PlayerController IsNull");
				return;
			}
			APlayerCameraManager localPlayerCameraManager = UGSE_EngineFuncLib.GetLocalPlayerCameraManager((UObject)(object)world);
			if (((UObject?)(object)localPlayerCameraManager).IsNullOrDestroyed())
			{
				Logging.LogError("[FreeCameraManager] EnterFreeCameraMode PlayerCameraManager IsNull");
				return;
			}
			if ((int)BGW_EnhancedInputMgrV2.Get((UObject)(object)world).InputModeTracker.InputMode != 2)
			{
				Logging.LogError("[FreeCameraManager] Game is currently not in GameOnly mode");
				return;
			}
			FVector cameraLocation = localPlayerCameraManager.GetCameraLocation();
			FRotator cameraRotation = localPlayerCameraManager.GetCameraRotation();
			if (((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				UClass val2 = BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("/Game/Mods/WukongMod/BP_FreeCameraActor.BP_FreeCameraActor_C", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				if ((UObject)(object)val2 == (UObject)null)
				{
					Logging.LogError("[FreeCameraManager] FreeCameraActor class is null");
					return;
				}
				_freeCameraActor = world.SpawnActor(val2, ref cameraLocation, ref cameraRotation);
			}
			if (((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				Logging.LogError("[FreeCameraManager] EnterFreeCameraMode Spawn FreeCameraActor Failed");
				return;
			}
			_springArmComponent = _freeCameraActor.GetComponentByClass<USpringArmComponent>();
			if ((UObject)(object)_springArmComponent == (UObject)null)
			{
				Logging.LogError("[FreeCameraManager] FreeCameraActor SpringArmComponent IsNull");
				return;
			}
			Logging.LogInformation("[FreeCameraManager] Entering free camera");
			_freeCameraActor.SetActorHiddenInGame(false);
			_freeCameraActor.SetActorEnableCollision(true);
			_cacheCameraViewTarget = ((AController)val).GetViewTarget();
			_gameFov = localPlayerCameraManager.GetFOVAngle();
			FHitResult val3 = default(FHitResult);
			_freeCameraActor.SetActorLocationAndRotation(cameraLocation, cameraRotation, false, ref val3, true);
			((UObject)_freeCameraActor).CallFunctionByNameWithArguments($"SetCameraFOV {_gameFov}", true);
			((APlayerController)val).SetViewTargetWithBlend(_freeCameraActor, 0f, (EViewTargetBlendFunction)0, 0f, false);
			BGW_EventCollection.Get((UObject)(object)world).Evt_SetInputMode.Invoke((EGSInputMode)3, (EGSInputModeChangeReason)14);
			_isInFreeCameraMode = true;
			this.OnFreeCameraModeChanged?.Invoke(obj: true);
		}

		public void LeaveFreeCameraMode()
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Invalid comparison between Unknown and I4
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null || !_isInFreeCameraMode)
			{
				return;
			}
			APlayerController firstLocalPlayerController = UGSE_EngineFuncLib.GetFirstLocalPlayerController((UObject)(object)world);
			ABGPPlayerController val = (ABGPPlayerController)(object)((firstLocalPlayerController is ABGPPlayerController) ? firstLocalPlayerController : null);
			if (((UObject?)(object)val).IsNullOrDestroyed())
			{
				Logging.LogError("[FreeCameraManager] LeaveFreeCameraMode PlayerController IsNull");
				return;
			}
			if (((UObject?)(object)_cacheCameraViewTarget).IsNullOrDestroyed())
			{
				APawn controlledPawn = ((AController)val).GetControlledPawn();
				BGUCharacterCS val2 = (BGUCharacterCS)(object)((controlledPawn is BGUCharacterCS) ? controlledPawn : null);
				if (((UObject?)(object)val2).IsNullOrDestroyed())
				{
					Logging.LogError("[FreeCameraManager] LeaveFreeCameraMode PlayerCharacter IsNull");
					return;
				}
				((APlayerController)val).SetViewTargetWithBlend((AActor)(object)val2, 0f, (EViewTargetBlendFunction)0, 0f, false);
			}
			else
			{
				((APlayerController)val).SetViewTargetWithBlend(_cacheCameraViewTarget, 0f, (EViewTargetBlendFunction)0, 0f, false);
			}
			if ((int)BGW_EnhancedInputMgrV2.Get((UObject)(object)world).InputModeTracker.InputMode == 3)
			{
				BGW_EventCollection.Get((UObject)(object)world).Evt_SetInputMode.Invoke((EGSInputMode)2, (EGSInputModeChangeReason)12);
			}
			if (!((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				BGU_UnrealWorldUtil.DestroyActor(_freeCameraActor);
			}
			Logging.LogInformation("[FreeCameraManager] Leaving free camera");
			_freeCameraActor = null;
			_springArmComponent = null;
			_cachePlayerPawn = null;
			_isInFreeCameraMode = false;
			this.OnFreeCameraModeChanged?.Invoke(obj: false);
		}

		public void ReEnableFreeCamera()
		{
			if (_isInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed() && (UObject)(object)_cachePlayerPawn != (UObject)null)
			{
				AController controller = ((APawn)_cachePlayerPawn).GetController();
				ABGPPlayerController val = (ABGPPlayerController)(object)((controller is ABGPPlayerController) ? controller : null);
				if (((UObject?)(object)val).IsNullOrDestroyed())
				{
					Logging.LogError("[FreeCameraManager] EnterFreeCameraMode PlayerController IsNull");
					return;
				}
				_cacheCameraViewTarget = ((AController)val).GetViewTarget();
				((APlayerController)val).SetViewTargetWithBlend(_freeCameraActor, 0f, (EViewTargetBlendFunction)0, 0f, false);
			}
		}

		public bool MoveFreeCameraToPosition(FVector position)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (!IsInFreeCameraMode || ((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				return false;
			}
			FVector actorLocation = _freeCameraActor.GetActorLocation();
			FVector moveOffset = position - actorLocation;
			return MoveFreeCameraActor(moveOffset, isLocal: false);
		}

		public bool MoveFreeCameraWithObstacleCheck(FVector targetPosition, FVector desiredCameraPosition, float safeDistance = 20f)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (!IsInFreeCameraMode || ((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				return false;
			}
			UWorld world = GameUtils.GetWorld();
			if ((UObject)(object)world == (UObject)null)
			{
				return false;
			}
			FHitResult val = default(FHitResult);
			USystemLibrary.SphereTraceSingle((UObject)(object)world, targetPosition, desiredCameraPosition, safeDistance, (ETraceTypeQuery)1, false, new List<AActor>(), (EDrawDebugTrace)0, ref val, true, FLinearColor.Green, FLinearColor.Red, 1f);
			FVector val2 = desiredCameraPosition;
			if (val.BlockingHit)
			{
				val2 = BGUFunctionLibraryCS.BGUGetVectorFromNetQuantizeVector(ref val.Location);
			}
			FHitResult val3 = default(FHitResult);
			_freeCameraActor.SetActorLocation(val2, false, ref val3, true);
			UpdatePawnPositionToCamera();
			return true;
		}

		public bool MoveFreeCameraActor(FVector moveOffset, bool isLocal)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			if (!IsInFreeCameraMode || ((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				return false;
			}
			FVector actorLocation = _freeCameraActor.GetActorLocation();
			FVector val;
			if (!isLocal)
			{
				val = moveOffset;
			}
			else
			{
				FTransform actorTransform = _freeCameraActor.GetActorTransform();
				val = ((FTransform)(ref actorTransform)).TransformVectorNoScale(moveOffset);
			}
			FVector moveOffset2 = val;
			if (!MoveDetection(actorLocation, moveOffset2, out var adjustedMoveOffset, 0))
			{
				return false;
			}
			FHitResult val2 = default(FHitResult);
			_freeCameraActor.AddActorWorldOffset(adjustedMoveOffset, true, ref val2, false);
			if (val2.BlockingHit)
			{
				FVector val3 = BGUFunctionLibraryCS.BGUGetVectorFromNetQuantizeVector(ref val2.Normal);
				FVector moveOffset3 = FVector.VectorPlaneProject(adjustedMoveOffset, val3);
				if (MoveDetection(actorLocation, moveOffset3, out var adjustedMoveOffset2, 1))
				{
					FHitResult val4 = default(FHitResult);
					_freeCameraActor.AddActorWorldOffset(adjustedMoveOffset2, true, ref val4, false);
				}
			}
			UpdatePawnPositionToCamera();
			return true;
		}

		private bool MoveDetection(FVector currentCameraPos, FVector moveOffset, out FVector adjustedMoveOffset, int traceNum)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			adjustedMoveOffset = moveOffset;
			FVector val = moveOffset;
			((FVector)(ref val)).Normalize(9.99999993922529E-09);
			FHitResult val2 = default(FHitResult);
			USystemLibrary.SphereTraceSingle((UObject)(object)GameUtils.GetWorld(), currentCameraPos + val, currentCameraPos + moveOffset, 20f, (ETraceTypeQuery)1, false, new List<AActor>(), (EDrawDebugTrace)0, ref val2, true, FLinearColor.Green, FLinearColor.Red, 1f);
			traceNum++;
			if (val2.BlockingHit)
			{
				if (traceNum < 2)
				{
					FVector val3 = BGUFunctionLibraryCS.BGUGetVectorFromNetQuantizeVector(ref val2.Normal);
					FVector moveOffset2 = FVector.VectorPlaneProject(moveOffset, val3);
					return MoveDetection(currentCameraPos, moveOffset2, out adjustedMoveOffset, traceNum);
				}
				return false;
			}
			return true;
		}

		public void RotateFreeCameraActor(FRotator rotatorOffset, bool isLocal)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				FHitResult val = default(FHitResult);
				if (isLocal)
				{
					_freeCameraActor.AddActorLocalRotation(rotatorOffset, true, ref val, false);
				}
				else
				{
					_freeCameraActor.AddActorWorldRotation(rotatorOffset, true, ref val, false);
				}
			}
		}

		public void SetLookAtTarget(FVector targetLocation)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				FRotator val = UMathLibrary.FindLookAtRotation(_freeCameraActor.GetActorLocation(), targetLocation);
				_freeCameraActor.SetActorRotation(val, false);
			}
		}

		public FVector GetCurrentCameraPosition()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				FTransform springArmEndTransform = GetSpringArmEndTransform();
				return ((FTransform)(ref springArmEndTransform)).GetLocation();
			}
			return FVector.ZeroVector;
		}

		public FVector GetForwardVector()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				return _freeCameraActor.GetActorForwardVector();
			}
			return FVector.ForwardVector;
		}

		public FVector GetRightVector()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				return _freeCameraActor.GetActorRightVector();
			}
			return FVector.RightVector;
		}

		public float GetFreeCameraActorPitch()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				FRotator actorRotation = _freeCameraActor.GetActorRotation();
				return ((FRotator)(ref actorRotation)).Pitch;
			}
			return 0f;
		}

		public void SetFreeCameraActorTransform(FVector location, FRotator rotation)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_freeCameraActor).IsNullOrDestroyed())
			{
				FHitResult val = default(FHitResult);
				_freeCameraActor.SetActorLocationAndRotation(location, rotation, false, ref val, true);
				UpdatePawnPositionToCamera();
			}
		}

		public void SetFreeCameraActorTransform(FTransform transform)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			FVector location = ((FTransform)(ref transform)).GetLocation();
			FQuat rotation = ((FTransform)(ref transform)).GetRotation();
			SetFreeCameraActorTransform(location, ((FQuat)(ref rotation)).Rotator());
		}

		public void SetSpringArmLength(float length)
		{
			if (IsInFreeCameraMode && !((UObject?)(object)_springArmComponent).IsNullOrDestroyed())
			{
				_springArmComponent.TargetArmLength = length;
				UpdatePawnPositionToCamera();
			}
		}

		public FTransform GetSpringArmEndTransform()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_springArmComponent).IsNullOrDestroyed())
			{
				return ((USceneComponent)_springArmComponent).GetSocketTransform(new FName("SpringEndpoint", (EFindName)1), (ERelativeTransformSpace)0);
			}
			return FTransform.Identity;
		}

		private void UpdatePawnPositionToCamera()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (IsInFreeCameraMode && !((UObject?)(object)_cachePlayerPawn).IsNullOrDestroyed())
			{
				FHitResult val = default(FHitResult);
				((AActor)_cachePlayerPawn).SetActorLocation(GetCurrentCameraPosition(), false, ref val, true);
			}
		}
	}
}
namespace WukongMp.Api.ECS.Values
{
	public static class EquipPositionExtensions
	{
		public static EquipPosition ToGame(this EquipPosition value)
		{
			return (EquipPosition)value;
		}

		public static EquipPosition FromGame(this EquipPosition value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Expected I4, but got Unknown
			return (EquipPosition)(int)value;
		}
	}
	public struct MontageState
	{
		[Ignore]
		public UAnimMontage? LocalMontage { get; set; }

		public float LocalMontagePosition { get; set; }

		[Ignore]
		public UAnimInstance? LocalAnimationInstance { get; set; }
	}
	public static class MoveSpeedLevelExtensions
	{
		public static EMoveSpeedLevel ToGame(this MoveSpeedLevel value)
		{
			return (EMoveSpeedLevel)value;
		}

		public static MoveSpeedLevel FromGame(this EMoveSpeedLevel value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Expected I4, but got Unknown
			return (MoveSpeedLevel)(int)value;
		}
	}
}
namespace WukongMp.Api.ECS.Systems
{
	public class DebugViewSystem(WukongEventBus eventBus, WukongWidgetManager widgetManager) : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>()
	{
		private const ulong TickInterval = 10uL;

		private ulong tickCounter;

		protected override void OnUpdate()
		{
			if (eventBus.IsGameplayLevel && widgetManager.IsDebugViewVisible && tickCounter++ % 10 == 0L)
			{
				base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainCharacter, ref MainCharacterComponent mainCharacter, Entity entity)
				{
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0024: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					string characterNickName = mainCharacter.CharacterNickName;
					BGUCharacterCS? pawn = localMainCharacter.Pawn;
					FVector gameLocation = ((pawn != null) ? ((AActor)pawn).GetActorLocation() : FVector.ZeroVector);
					FVector ecsLocation = mainCharacter.Location.ToFVector();
					widgetManager.UpdatePlayerPosition(characterNickName, gameLocation, ecsLocation);
				});
			}
		}
	}
	public class SyncMonsterTeamSystem : QuerySystem<TeamComponent, LocalTamerComponent>
	{
		protected override void OnUpdate()
		{
			base.Query.ForEachEntity(delegate(ref TeamComponent team, ref LocalTamerComponent localTamer, Entity entity)
			{
				if (localTamer.IsMonsterActive && !((UObject)(object)localTamer.Pawn == (UObject)null) && team.TeamId != localTamer.Pawn.GetTeamIDInCS())
				{
					ClientUtils.RegisterAndSetPlayerTeam(localTamer.Pawn, team.TeamId);
				}
			});
		}
	}
}
namespace WukongMp.Api.ECS.Systems.Tamers
{
	public sealed class ChangeTamerTargetSystem : QuerySystem<LocalTamerComponent>
	{
		private float _elapsedTime;

		protected override void OnUpdate()
		{
			if (_elapsedTime >= 7f)
			{
				_elapsedTime = 0f;
				base.Query.ForEachEntity(delegate(ref LocalTamerComponent localTamerComp, Entity entity)
				{
					if (DI.Instance.ClientOwnership.OwnsEntity(entity) && BGUFunctionLibraryCS.BGUIsUnitInBattle((AActor)(object)localTamerComp.Pawn))
					{
						BGUFuncLibAICS.SearchTargetSP((AActor)(object)localTamerComp.Pawn);
					}
				});
			}
			else
			{
				_elapsedTime += base.Tick.deltaTime;
			}
		}
	}
	public sealed class DespawnTamerSystem : BaseSystem, IDisposable
	{
		private struct PendingDeleteEvent
		{
			public string Guid;

			public BUTamerActor? Tamer;

			public AActor? Marker;
		}

		private readonly ArchetypeEventRouter _archetypeEvent;

		private readonly WukongPlayerState _playerState;

		private readonly ClientWukongArchetypeRegistration _wukongArchetype;

		private readonly WukongEventBus _eventBus;

		private readonly ILogger _logger;

		private readonly List<PendingDeleteEvent> _pendingDeleteEvents = new List<PendingDeleteEvent>();

		public DespawnTamerSystem(ArchetypeEventRouter archetypeEvent, WukongPlayerState playerState, ClientWukongArchetypeRegistration wukongArchetype, WukongEventBus eventBus, ILogger logger)
		{
			_archetypeEvent = archetypeEvent;
			_playerState = playerState;
			_wukongArchetype = wukongArchetype;
			_eventBus = eventBus;
			_logger = logger;
			ArchetypeEventRouter.ArchetypeEntry archetypeEntry = _archetypeEvent[_wukongArchetype.MonsterArchetype];
			archetypeEntry.OnEntityDelete += OnEntityDeleteHandler;
		}

		public void Dispose()
		{
			ArchetypeEventRouter.ArchetypeEntry archetypeEntry = _archetypeEvent[_wukongArchetype.MonsterArchetype];
			archetypeEntry.OnEntityDelete -= OnEntityDeleteHandler;
		}

		private void OnEntityDeleteHandler(EntityDelete evt)
		{
			if (!_playerState.LocalPlayerId.HasValue)
			{
				_logger.LogWarning("Local player ID is null, cannot despawn monster.");
				return;
			}
			TamerEntity tamerEntity = new TamerEntity(evt.Entity);
			TamerComponent tamer = tamerEntity.GetTamer();
			LocalTamerComponent localTamer = tamerEntity.GetLocalTamer();
			MarkerComponent marker = tamerEntity.GetMarker();
			_pendingDeleteEvents.Add(new PendingDeleteEvent
			{
				Guid = tamer.Guid,
				Tamer = localTamer.Tamer,
				Marker = marker.MarkerActor
			});
		}

		protected override void OnUpdateGroup()
		{
			if (!_eventBus.IsGameplayLevel)
			{
				return;
			}
			foreach (PendingDeleteEvent pendingDeleteEvent in _pendingDeleteEvents)
			{
				TamerUtils.DestroyTamer(pendingDeleteEvent.Guid, pendingDeleteEvent.Tamer, pendingDeleteEvent.Marker);
			}
			_pendingDeleteEvents.Clear();
		}
	}
	public sealed class KillAlreadyDeadMonstersSystem(ClientOwnershipManager clientOwnership, WukongPlayerState playerState) : QuerySystem<TamerComponent, LocalTamerComponent, MetadataComponent, HpComponent>()
	{
		private const ulong TickInterval = 10uL;

		private ulong tickCounter;

		protected override void OnUpdate()
		{
			if (tickCounter++ % 10 != 0L || !playerState.LocalPlayerId.HasValue)
			{
				return;
			}
			base.Query.ForEachEntity(delegate(ref TamerComponent tamerComp, ref LocalTamerComponent localTamerComp, ref MetadataComponent metaComp, ref HpComponent hpComp, Entity entity)
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Invalid comparison between Unknown and I4
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				if (!localTamerComp.IsCheckedForDead && localTamerComp.IsTamerSynced && hpComp.IsDead && !clientOwnership.OwnsEntity(entity))
				{
					BUTamerActor? tamer = localTamerComp.Tamer;
					BGUCharacterCS val = ((tamer != null) ? tamer.GetMonster() : null);
					if (!((UObject)(object)val == (UObject)null))
					{
						BUTamerActor? tamer2 = localTamerComp.Tamer;
						if (tamer2 != null)
						{
							FTamerRef currentRef = tamer2.CurrentRef;
							if ((int)((currentRef != null) ? new ETamerPhase?(currentRef.Phase) : ((ETamerPhase?)null)).GetValueOrDefault() == 7 && !BGUFunctionLibraryCS.BGUHasUnitState((AActor)(object)val, (EBGUUnitState)6))
							{
								Logging.LogDebug("Monster is dead, sending unitDead locally. Guid: {Guid}, netId: {NetId}.", tamerComp.Guid, metaComp.NetId);
								if (tamerComp.Guid == "UGuid.LYS.KJL.Women")
								{
									BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)val);
									if (obj != null)
									{
										obj.Evt_UnitDead.Invoke((AActor)(object)val, (EDeadReason)2, 11213, 5, (UAnimMontage)null, default(FEffectInstReq), false, (EAbnormalStateType)0);
									}
								}
								else
								{
									BUS_GSEventCollection obj2 = BUS_EventCollectionCS.Get((AActor)(object)val);
									if (obj2 != null)
									{
										obj2.Evt_UnitDead.Invoke((AActor)(object)val, (EDeadReason)2, -1, -1, (UAnimMontage)null, default(FEffectInstReq), false, (EAbnormalStateType)0);
									}
								}
								localTamerComp.IsCheckedForDead = true;
							}
						}
					}
				}
			});
		}
	}
	public sealed class SpawnTamersSystem(ClientState state, GameplayEventRouter router, GameplayConfiguration configuration) : QuerySystem<MetadataComponent, HpComponent, TeamComponent, TamerComponent, LocalTamerComponent>()
	{
		private readonly HashSet<string?> _notYetSpawnedGuids = new HashSet<string>();

		protected override void OnUpdate()
		{
			base.Query.ForEachEntity(delegate(ref MetadataComponent metaComp, ref HpComponent hpComp, ref TeamComponent teamComp, ref TamerComponent tamerComp, ref LocalTamerComponent localTamerComp, Entity entity)
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Invalid comparison between Unknown and I4
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Invalid comparison between Unknown and I4
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Invalid comparison between Unknown and I4
				//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b7: Invalid comparison between Unknown and I4
				if (!localTamerComp.IsMonsterActive && tamerComp.ShouldBeSpawned && localTamerComp.IsTamerSynced && !((UObject)(object)localTamerComp.Tamer == (UObject)null))
				{
					FTamerRef currentRef = localTamerComp.Tamer.CurrentRef;
					if (currentRef == null || (int)currentRef.Phase != 8)
					{
						BUTamerActor? tamer = localTamerComp.Tamer;
						BGUCharacterCS val = ((tamer != null) ? tamer.GetMonster() : null);
						FTamerRef currentRef2 = localTamerComp.Tamer.CurrentRef;
						if ((int)((currentRef2 != null) ? new ETamerPhase?(currentRef2.Phase) : ((ETamerPhase?)null)).GetValueOrDefault() != 7 || (UObject)(object)val == (UObject)null)
						{
							TamerUtils.SpawnMonsterLocally(new TamerEntity(entity));
						}
						val = localTamerComp.Tamer.GetMonster();
						FTamerRef currentRef3 = localTamerComp.Tamer.CurrentRef;
						if ((int)((currentRef3 != null) ? new ETamerPhase?(currentRef3.Phase) : ((ETamerPhase?)null)).GetValueOrDefault() != 7 || (UObject)(object)val == (UObject)null)
						{
							if (_notYetSpawnedGuids.Add(tamerComp.Guid))
							{
								Logging.LogInformation("Monster {Guid} not yet spawned, waiting...", tamerComp.Guid);
							}
						}
						else if (!BGUFunctionLibraryCS.BGUHasUnitState((AActor)(object)val, (EBGUUnitState)6))
						{
							BUC_AttrContainer readOnlyData = BGU_DataUtil.GetReadOnlyData<BUC_AttrContainer>((AActor)(object)val);
							if (readOnlyData != null && DI.Instance.ClientOwnership.OwnsEntity(entity))
							{
								hpComp.HpMaxBase = readOnlyData.GetFloatValue((EBGUAttrFloat)101);
								hpComp.Hp = readOnlyData.GetFloatValue((EBGUAttrFloat)151);
								if (configuration.SyncTamerTeamFromGameToEcs)
								{
									teamComp.TeamId = val.GetTeamIDInCS();
								}
							}
							BUS_GSEventCollection val2 = BUS_EventCollectionCS.Get((AActor)(object)localTamerComp.Tamer);
							if ((UObject)(object)val2 == (UObject)null)
							{
								Logging.LogError("events are null");
							}
							else
							{
								IBUC_ABPMotionMatchingData unPersistentReadOnlyData = (IBUC_ABPMotionMatchingData)(object)BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_ABPMotionMatchingData>((AActor)(object)localTamerComp.Pawn);
								if (unPersistentReadOnlyData != null)
								{
									val2.Evt_ChangeMotionMatchingState.Invoke(unPersistentReadOnlyData.DefaultMMState);
								}
								PlayerId owner = metaComp.Owner;
								PlayerId? localPlayerId = state.LocalPlayerId;
								if (owner != localPlayerId)
								{
									val2.Evt_AIPauseBT.Invoke(true);
									val2.Evt_AIPauseFsm.Invoke(true);
									val2.Evt_AIPerceptionSetting.Invoke(false);
									Logging.LogDebug("Tamer actor disabled, guid: {Guid}.", tamerComp.Guid);
									if (tamerComp.Guid == "UGuid.HYS.JiRuHuo01")
									{
										((UPrimitiveComponent)((ACharacter)val).Mesh).SetSimulatePhysics(false);
										val2.Evt_DisablePhysicalMove.Invoke(true);
									}
								}
								else
								{
									IBUC_FsmData readOnlyData2 = BGU_DataUtil.GetReadOnlyData<IBUC_FsmData, BUC_FsmData>((AActor)(object)val);
									if (readOnlyData2 != null)
									{
										Logging.LogDebug("Initial tamer bFsmPaused state {State}, guid: {Guid}.", readOnlyData2.bFsmPaused, tamerComp.Guid);
										tamerComp.HasFsmPaused = readOnlyData2.bFsmPaused;
									}
								}
								localTamerComp.IsMonsterActive = true;
								if ((int)((ABGUTamerBase)localTamerComp.Tamer).TamerType == 2)
								{
									router.RaiseOnMonsterSpawned(entity);
								}
								Logging.LogDebug("Monster {Guid} synced", tamerComp.Guid);
							}
						}
					}
				}
			});
		}
	}
	public sealed class SyncTamersSystem : QuerySystem<TamerComponent, LocalTamerComponent, TransformComponent, MetadataComponent, HpComponent>
	{
		private const ulong TickInterval = 10uL;

		private ulong tickCounter;

		protected override void OnUpdate()
		{
			if (tickCounter++ % 10 != 0L)
			{
				return;
			}
			Dictionary<string, BUTamerActor> allTamers = (from x in UGameplayStatics.GetAllActorsOfClass<BUTamerActor>((UObject)(object)GameUtils.GetWorld())
				where (UObject)(object)x != (UObject)null
				group x by x.GetFinalGuid(false)).ToDictionary((IGrouping<string, BUTamerActor> g) => g.Key, (IGrouping<string, BUTamerActor> g) => g.Last());
			base.Query.ForEachEntity(delegate(ref TamerComponent tamerComp, ref LocalTamerComponent localTamerComp, ref TransformComponent translation, ref MetadataComponent metaComp, ref HpComponent hpComp, Entity entity)
			{
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				if (!localTamerComp.IsTamerSynced)
				{
					BUTamerActor value;
					if (tamerComp.Guid == null)
					{
						Logging.LogError("Entity {EntityId} has a TamerComponent with a null Guid. Cannot sync tamer.", entity.Id);
					}
					else if (allTamers.TryGetValue(tamerComp.Guid, out value))
					{
						localTamerComp.Tamer = value;
						localTamerComp.IsTamerSynced = true;
						Logging.LogDebug("Found matching tamer with guid: {Guid}", tamerComp.Guid);
						if ((UObject)(object)localTamerComp.Tamer.GetMonster() != (UObject)null)
						{
							Logging.LogDebug("Monster already spawned on the level, guid: {Guid}, netId: {NetId}. Marking as spawned.", tamerComp.Guid, metaComp.NetId);
							TamerUtils.MarkMonsterLocallySpawned(ref localTamerComp, metaComp);
						}
					}
					else
					{
						TamerComponent tamerComponent = tamerComp;
						if (tamerComponent.UnitPath != null)
						{
							SpawningUtils.SpawnUnitLocallyByPath(tamerComp.Guid, tamerComp.UnitPath, translation.Position.ToFVector());
						}
					}
				}
			});
		}
	}
	public sealed class UnloadTamersSystem : QuerySystem<TamerComponent, LocalTamerComponent>
	{
		protected override void OnUpdate()
		{
			base.Query.ForEachEntity(delegate(ref TamerComponent tamerComp, ref LocalTamerComponent localTamerComp, Entity _)
			{
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Invalid comparison between Unknown and I4
				if (localTamerComp.IsTamerSynced && !((UObject)(object)localTamerComp.Tamer == (UObject)null) && localTamerComp.Tamer.CurrentRef != null && !((UObject)(object)localTamerComp.Pawn == (UObject)null))
				{
					LocalTamerComponent localTamerComponent = localTamerComp;
					if (localTamerComponent.IsMonsterActive && !localTamerComponent.IsLocallySpawned && localTamerComponent.HasPendingUnload && !tamerComp.ShouldBeSpawned && (int)localTamerComp.Tamer.CurrentRef.Phase != 2)
					{
						localTamerComp.Tamer.CurrentRef.TurnBack2Loaded();
					}
				}
			});
		}

		private bool CanTurnBack2Loaded(FTamerRef tamerRef)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (tamerRef.MonsterInstancePtr.IsValid())
			{
				BUC_BattleStateData unPersistentReadOnlyData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_BattleStateData>((AActor)(object)tamerRef.MonsterInstancePtr.Get());
				if (unPersistentReadOnlyData != null && unPersistentReadOnlyData.IsUnitInBattle())
				{
					return false;
				}
				BUC_PatrolData unPersistentReadOnlyData2 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_PatrolData>((AActor)(object)tamerRef.MonsterInstancePtr.Get());
				if (unPersistentReadOnlyData2 != null && unPersistentReadOnlyData2.bIsPatroling)
				{
					return false;
				}
				BUC_UnitStateData unPersistentReadOnlyData3 = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_UnitStateData>((AActor)(object)tamerRef.MonsterInstancePtr.Get());
				if (unPersistentReadOnlyData3 != null && unPersistentReadOnlyData3.HasState((EBGUUnitState)6))
				{
					return false;
				}
			}
			return true;
		}
	}
	public sealed class UpdateTamerMarkersSystem : QuerySystem<LocalTamerComponent, MarkerComponent, TransformComponent, NicknameComponent, TamerComponent>
	{
		protected override void OnUpdate()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			BGP_PlayerControllerB1 playerController = GameUtils.GetPlayerController();
			if ((UObject)(object)playerController == (UObject)null)
			{
				return;
			}
			AActor viewTarget = ((AController)playerController).GetViewTarget();
			FVector viewTargetLocation = viewTarget.GetActorLocation();
			base.Query.ForEachEntity(delegate(ref LocalTamerComponent localTamerComp, ref MarkerComponent markerComp, ref TransformComponent transComp, ref NicknameComponent nameComp, ref TamerComponent tamerComp, Entity _)
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				if (!((UObject)(object)markerComp.MarkerActor == (UObject)null) && (UObject)(object)localTamerComp.Tamer != (UObject)null && (UObject)(object)localTamerComp.Pawn != (UObject)null)
				{
					FVector actorLocation = ((AActor)localTamerComp.Pawn).GetActorLocation();
					float num = Math.Min(FVector.Dist2D(viewTargetLocation, actorLocation) / 10000f, 1f);
					float num2 = ((ABGUTamerBase)localTamerComp.Tamer).CapsuleComponent.GetScaledCapsuleHalfHeight() * (1.12f + num);
					FHitResult val = default(FHitResult);
					markerComp.MarkerActor.SetActorLocation(actorLocation + new FVector(0.0, 0.0, (double)num2), false, ref val, true);
				}
			});
		}
	}
}
namespace WukongMp.Api.ECS.Systems.MainCharacters
{
	public class AfterMainCharacterDeathSystem(WukongEventBus eventBus, WukongPlayerState playerState) : BaseSystem()
	{
		protected override void OnUpdateGroup()
		{
			if (!eventBus.IsGameplayLevel || !playerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			MainCharacterEntity value = playerState.LocalMainCharacter.Value;
			if (value.GetState().IsDead && value.GetLocalState().IsDuringDeathAnim)
			{
				ref LocalMainCharacterComponent localState = ref value.GetLocalState();
				localState.DeadAnimationTime -= base.Tick.deltaTime;
				if (localState.DeadAnimationTime <= 0f)
				{
					localState.IsDuringDeathAnim = false;
					PlayerUtils.EnableSpectator(value, SpectatorReason.Death);
				}
			}
		}
	}
	public class CreateLocalMainCharacterEntitySystem(ClientState clientState, WukongPlayerState playerState, WukongEventBus eventBus, ILogger logger) : BaseSystem()
	{
		protected override void OnUpdateGroup()
		{
			if (!eventBus.IsGameplayLevel)
			{
				return;
			}
			PlayerEntity? localPlayerEntity = playerState.LocalPlayerEntity;
			if (localPlayerEntity.HasValue && clientState.CurrentAreaId.HasValue)
			{
				BGUPlayerCharacterCS controlledPawn = GameUtils.GetControlledPawn();
				MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
				if (!((UObject?)(object)controlledPawn).IsNullOrDestroyed() && !localMainCharacter.HasValue)
				{
					logger.LogDebug("CREATING LOCAL MAIN CHARACTER ENTITY");
					CreateLocalMainEntity(controlledPawn, localPlayerEntity.Value);
				}
			}
		}

		private void CreateLocalMainEntity(BGUPlayerCharacterCS pawn, PlayerEntity playerEntity)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected I4, but got Unknown
			logger.LogDebug("Setting initial player properties");
			ref PlayerComponent state = ref playerEntity.GetState();
			MainCharacterEntity mainCharacterEntity = playerState.CreateLocalMainCharacter();
			ref MainCharacterComponent state2 = ref mainCharacterEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainCharacterEntity.GetLocalState();
			logger.LogDebug("Local main character pawn: {Pawn}", ((UObject)pawn).PathName);
			localState.Pawn = (BGUCharacterCS?)(object)pawn;
			state2.Location = ((AActor)pawn).GetActorLocation().ToVector3();
			state2.Rotation = ((AActor)pawn).GetActorRotation().ToVector3();
			IBUC_AttrContainer readOnlyData = BGU_DataUtil.GetReadOnlyData<IBUC_AttrContainer, BUC_AttrContainer>((AActor)(object)pawn);
			state2.Hp = readOnlyData.GetFloatValue((EBGUAttrFloat)151);
			state2.HpMaxBase = readOnlyData.GetFloatValue((EBGUAttrFloat)101);
			if (state2.Hp > 0f)
			{
				state2.IsDead = false;
			}
			foreach (EBGUAttrFloat syncedAttribute in WukongMp.Api.Configuration.Constants.SyncedAttributes)
			{
				float floatValue = readOnlyData.GetFloatValue(syncedAttribute);
				state2.Attributes.SetAttribute((byte)(int)syncedAttribute, floatValue);
			}
			state2.CharacterNickName = state.NickName;
			EquipmentState currentEquipmentStateForActor = EquipmentUtils.GetCurrentEquipmentStateForActor((APawn)(object)pawn);
			state2.Equipment = currentEquipmentStateForActor;
			int teamIDInCS = ((BGUCharacterCS)pawn).GetTeamIDInCS();
			mainCharacterEntity.SetTeam(new TeamComponent
			{
				TeamId = teamIDInCS
			});
			localState.IsPlayerSynced = true;
			playerState.InvokeMainCharacterEntityInitialized(mainCharacterEntity);
			Logging.LogDebug("Finished setting initial player properties");
		}
	}
	public sealed class DespawnOtherMainCharactersSystem : BaseSystem, IDisposable
	{
		private struct PendingDeleteEvent
		{
			public PlayerId PlayerId;

			public BGUCharacterCS? PlayerCharacter;

			public AActor? PlayerMarker;
		}

		private readonly ArchetypeEventRouter _archetypeEvent;

		private readonly WukongPlayerState _playerState;

		private readonly ClientWukongArchetypeRegistration _wukongArchetype;

		private readonly WukongPlayerPawnState _playerPawnState;

		private readonly WukongEventBus _eventBus;

		private readonly ILogger _logger;

		private readonly List<PendingDeleteEvent> _pendingDeleteEvents = new List<PendingDeleteEvent>();

		public DespawnOtherMainCharactersSystem(ArchetypeEventRouter archetypeEvent, WukongPlayerState playerState, ClientWukongArchetypeRegistration wukongArchetype, WukongPlayerPawnState playerPawnState, WukongEventBus eventBus, ILogger logger)
		{
			_archetypeEvent = archetypeEvent;
			_playerState = playerState;
			_wukongArchetype = wukongArchetype;
			_playerPawnState = playerPawnState;
			_eventBus = eventBus;
			_logger = logger;
			ArchetypeEventRouter.ArchetypeEntry archetypeEntry = _archetypeEvent[_wukongArchetype.MainCharacterArchetype];
			archetypeEntry.OnEntityDelete += OnEntityDeleteHandler;
		}

		public void Dispose()
		{
			ArchetypeEventRouter.ArchetypeEntry archetypeEntry = _archetypeEvent[_wukongArchetype.MainCharacterArchetype];
			archetypeEntry.OnEntityDelete -= OnEntityDeleteHandler;
		}

		private void OnEntityDeleteHandler(EntityDelete evt)
		{
			if (!_playerState.LocalPlayerId.HasValue)
			{
				_logger.LogWarning("Local player ID is null, cannot despawn other main characters.");
				return;
			}
			MainCharacterEntity mainCharacterEntity = new MainCharacterEntity(evt.Entity);
			MainCharacterComponent state = mainCharacterEntity.GetState();
			PlayerId playerId = state.PlayerId;
			PlayerId value = playerId;
			PlayerId? localPlayerId = _playerState.LocalPlayerId;
			if (!(value == localPlayerId))
			{
				LocalMainCharacterComponent localState = mainCharacterEntity.GetLocalState();
				_pendingDeleteEvents.Add(new PendingDeleteEvent
				{
					PlayerId = playerId,
					PlayerCharacter = localState.Pawn,
					PlayerMarker = localState.MarkerActor
				});
			}
		}

		protected override void OnUpdateGroup()
		{
			if (!_eventBus.IsGameplayLevel)
			{
				return;
			}
			foreach (PendingDeleteEvent pendingDeleteEvent in _pendingDeleteEvents)
			{
				_logger.LogDebug("ATTEMPTING TO DESPAWN OTHER MAIN CHARACTER ENTITY: {PlayerId}", pendingDeleteEvent.PlayerId);
				_playerPawnState.RemovePlayerPawn(pendingDeleteEvent.PlayerId, pendingDeleteEvent.PlayerCharacter, pendingDeleteEvent.PlayerMarker);
			}
			_pendingDeleteEvents.Clear();
		}
	}
	public class EnableCollisionAfterCutsceneSystem(WukongPlayerState playerState) : QuerySystem<MainCharacterComponent, LocalMainCharacterComponent>()
	{
		protected override void OnUpdate()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (!playerState.LocalMainCharacter.HasValue)
			{
				return;
			}
			BGUCharacterCS pawn = playerState.LocalMainCharacter.Value.GetLocalState().Pawn;
			if ((UObject)(object)pawn == (UObject)null)
			{
				return;
			}
			FVector myCapsuleCenter = ((AActor)pawn).GetActorLocation();
			float myCapsuleRadius = ((ACharacter)pawn).CapsuleComponent.GetScaledCapsuleRadius();
			base.Query.ForEachEntity(delegate(ref MainCharacterComponent main, ref LocalMainCharacterComponent local, Entity _)
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				if (!(playerState.LocalPlayerId == main.PlayerId) && !((UObject)(object)local.Pawn == (UObject)null) && !local.ShouldDisableCollision && ((UPrimitiveComponent)((ACharacter)local.Pawn).CapsuleComponent).GetCollisionProfileName() == B1GlobalFNames.WindWalk_Pawn)
				{
					FVector actorLocation = ((AActor)local.Pawn).GetActorLocation();
					float scaledCapsuleRadius = ((ACharacter)local.Pawn).CapsuleComponent.GetScaledCapsuleRadius();
					float num = FVector.Dist2D(myCapsuleCenter, actorLocation);
					float num2 = myCapsuleRadius + scaledCapsuleRadius;
					if (num > num2)
					{
						PlayerUtils.SetCollisionEnabled(local.Pawn, enabled: true);
					}
				}
			});
		}
	}
	public class FreeCameraMovementSystem(WukongEventBus eventBus, FreeCameraManager freeCameraManager, FreeCameraController freeCameraController) : BaseSystem()
	{
		protected override void OnUpdateGroup()
		{
			if (eventBus.IsGameplayLevel && freeCameraManager.IsInFreeCameraMode)
			{
				freeCameraController.Update(base.Tick.deltaTime);
			}
		}
	}
	public class SpawnOtherMainCharactersSystem(ClientState clientState, WukongPlayerState playerState, WukongPlayerPawnState playerPawn, WukongEventBus eventBus, ClientOwnershipManager ownershipManager, ILogger logger) : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>()
	{
		protected override void OnUpdate()
		{
			if (!eventBus.IsGameplayLevel || (UObject)(object)GameUtils.GetControlledPawn() == (UObject)null)
			{
				return;
			}
			PlayerId? playerId = playerState.LocalPlayerId;
			if (!playerId.HasValue || !clientState.CurrentAreaId.HasValue)
			{
				return;
			}
			base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, ref MainCharacterComponent mainComp, Entity entity)
			{
				PlayerId playerId2 = mainComp.PlayerId;
				PlayerId? playerId3 = playerId;
				if (!(playerId2 == playerId3) && !localMainComp.HasPawn)
				{
					MainCharacterEntity mainEntity = new MainCharacterEntity(entity);
					if (!playerState.GetPlayerById(mainComp.PlayerId).HasValue && ownershipManager.OwnsEntity(entity))
					{
						base.CommandBuffer.DeleteEntity(entity.Id);
					}
					else
					{
						logger.LogDebug("ATTEMPTING TO **SPAWN** OTHER MAIN CHARACTER ENTITY: {PlayerId}", mainComp.PlayerId);
						AddPlayer(mainEntity);
					}
				}
			});
		}

		private void AddPlayer(MainCharacterEntity mainEntity)
		{
			ref MainCharacterComponent state = ref mainEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			PlayerId playerId = state.PlayerId;
			playerPawn.AddPlayerPawn(playerId);
			localState.IsPlayerSynced = true;
		}
	}
	public class SyncMainCharactersSystem : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>
	{
		public SyncMainCharactersSystem(WukongPlayerState playerState, WukongPlayerModeManager modeManager, WukongEventBus eventBus, WukongWidgetManager widgetManager, GameplayConfiguration configuration, GameplayEventRouter eventRouter, ILogger logger)
		{
			<playerState>P = playerState;
			<modeManager>P = modeManager;
			<eventBus>P = eventBus;
			<configuration>P = configuration;
			<eventRouter>P = eventRouter;
			<logger>P = logger;
			base..ctor();
		}

		protected override void OnUpdate()
		{
			if (!<eventBus>P.IsGameplayLevel)
			{
				return;
			}
			base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, ref MainCharacterComponent mainComp, Entity entity)
			{
				if (!((UObject)(object)localMainComp.Pawn == (UObject)null))
				{
					PlayerEntity? playerById = <playerState>P.GetPlayerById(mainComp.PlayerId);
					if (playerById.HasValue)
					{
						MainCharacterEntity mainEntity = new MainCharacterEntity(entity);
						PlayerId playerId = mainComp.PlayerId;
						PlayerId? localPlayerId = <playerState>P.LocalPlayerId;
						if (playerId != localPlayerId)
						{
							SyncOtherMainCharacterState(playerById.Value, mainEntity);
						}
						else
						{
							SyncLocalMainCharacterState(playerById.Value, mainEntity);
						}
					}
				}
			});
		}

		private void SyncMainCharacterStateBase(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			ref PlayerComponent state = ref playerEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			bool isSpectator = mainEntity.GetPvP().IsSpectator;
			if (isSpectator != localState.IsSpectatorLocally)
			{
				localState.IsSpectatorLocally = isSpectator;
				if (<modeManager>P.HandleBecameSpectator(playerEntity, mainEntity, isSpectator))
				{
					PlayerId owner = playerEntity.Entity.GetComponent<MetadataComponent>().Owner;
					Logging.LogInformation("Player {Id} spectator status changed: {Spectator}", owner, isSpectator);
				}
			}
			ref readonly TeamComponent team = ref mainEntity.GetTeam();
			if (localState.Pawn.GetTeamIDInCS() != team.TeamId)
			{
				<logger>P.LogInformation("Assigning team ID {TeamId} to player {Name}", team.TeamId, state.NickName);
				ClientUtils.RegisterAndSetPlayerTeam(localState.Pawn, team.TeamId);
				<eventRouter>P.RaiseOnPlayerChangedTeam(playerEntity, mainEntity);
			}
		}

		private void SyncLocalMainCharacterState(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			SyncMainCharacterStateBase(playerEntity, mainEntity);
			if (<configuration>P.OverrideLocalPlayerTeamFromGlobalEntity)
			{
				ref PlayerComponent state = ref playerEntity.GetState();
				int teamId = state.TeamId;
				if (teamId != mainEntity.GetTeam().TeamId)
				{
					<logger>P.LogDebug("Assigning team ID {TeamId} to player {Name} from player to character", teamId, state.NickName);
					mainEntity.SetTeam(new TeamComponent
					{
						TeamId = teamId
					});
				}
			}
		}

		private void SyncOtherMainCharacterState(PlayerEntity playerEntity, MainCharacterEntity mainEntity)
		{
			SyncMainCharacterStateBase(playerEntity, mainEntity);
			ref MainCharacterComponent state = ref mainEntity.GetState();
			ref LocalMainCharacterComponent localState = ref mainEntity.GetLocalState();
			if ((UObject)(object)localState.Pawn == (UObject)null)
			{
				return;
			}
			EquipmentState equipment = state.Equipment;
			if (equipment.IsLocallyDirty)
			{
				if (((UObject)((UObject)localState.Pawn).GetClass()).PathName != "/Game/00Main/Design/Units/Player/Unit_player_dasheng.Unit_player_dasheng_C")
				{
					EquipmentUtils.SetActorEquipment(localState.Pawn, state.Equipment);
				}
				equipment.ClearLocallyDirty();
				state.Equipment = equipment;
			}
		}
	}
	public class UpdateCooldownSystem(WukongPlayerState playerState, WukongEventBus eventBus, WukongAreaState areaState) : BaseSystem()
	{
		private float _vigorRegenAccumulator;

		protected override void OnUpdateGroup()
		{
			if (!eventBus.IsGameplayLevel || (areaState.CurrentArea.HasValue && !areaState.CurrentArea.Value.Room.CheatsAllowed))
			{
				return;
			}
			MainCharacterEntity? localMainCharacter = playerState.LocalMainCharacter;
			if (!localMainCharacter.HasValue)
			{
				return;
			}
			ref LocalMainCharacterComponent localState = ref localMainCharacter.Value.GetLocalState();
			if (!localState.SpiritCooldownEnabled)
			{
				return;
			}
			BGUCharacterCS pawn = localState.Pawn;
			if ((UObject)(object)pawn == (UObject)null)
			{
				return;
			}
			if (BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_MagicallyChangeData>((AActor)(object)pawn).DurMagicallyChange)
			{
				_vigorRegenAccumulator = 0f;
				return;
			}
			float num = BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)pawn, (EBGUAttrFloat)202);
			if (USharpExtensions.Equals(num, 0f, 0.1f))
			{
				_vigorRegenAccumulator = 0f;
			}
			BUS_GSEventCollection val = BUS_EventCollectionCS.Get((AActor)(object)pawn);
			if (USharpExtensions.Equals(localState.SpiritCooldownTime, 0f, 0.1f))
			{
				localState.ShouldSetSpiritCooldown = true;
				if (val != null)
				{
					val.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)202, BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)pawn, (EBGUAttrFloat)17));
				}
				localState.ShouldSetSpiritCooldown = false;
			}
			else
			{
				if (_vigorRegenAccumulator > localState.SpiritCooldownTime)
				{
					return;
				}
				_vigorRegenAccumulator += base.Tick.deltaTime;
				float num2 = FMath.Lerp(0f, BGUFunctionLibraryCS.BGUGetFloatAttr((AActor)(object)pawn, (EBGUAttrFloat)17), FMath.Clamp(_vigorRegenAccumulator / localState.SpiritCooldownTime, 0f, 1f));
				if (num2 > num)
				{
					localState.ShouldSetSpiritCooldown = true;
					if (val != null)
					{
						val.Evt_SetAttrFloat.Invoke((EBGUAttrFloat)202, num2);
					}
					localState.ShouldSetSpiritCooldown = false;
				}
			}
		}
	}
	public class UpdateMainCharacterMarkerSystem : QuerySystem<LocalMainCharacterComponent, MainCharacterComponent>
	{
		protected override void OnUpdate()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			BGP_PlayerControllerB1 playerController = GameUtils.GetPlayerController();
			if ((UObject)(object)playerController == (UObject)null)
			{
				return;
			}
			AActor viewTarget = ((AController)playerController).GetViewTarget();
			FVector viewTargetLocation = viewTarget.GetActorLocation();
			base.Query.ForEachEntity(delegate(ref LocalMainCharacterComponent localMainComp, ref MainCharacterComponent mainComp, Entity _)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				if (!((UObject)(object)localMainComp.MarkerActor == (UObject)null) && localMainComp.HasPawn)
				{
					FVector actorLocation = ((AActor)localMainComp.Pawn).GetActorLocation();
					float num = Math.Min(FVector.Dist2D(viewTargetLocation, actorLocation) / 10000f, 1f);
					float num2 = ((ACharacter)localMainComp.Pawn).CapsuleComponent.GetScaledCapsuleHalfHeight() * (1.12f + num);
					FHitResult val = default(FHitResult);
					localMainComp.MarkerActor.SetActorLocation(actorLocation + new FVector(0.0, 0.0, (double)num2), false, ref val, true);
				}
			});
		}
	}
}
namespace WukongMp.Api.ECS.Managers
{
	public sealed class ArchetypeEventRouter : IDisposable
	{
		public readonly struct ArchetypeEntry
		{
			public event Action<EntityCreate>? OnEntityCreate
			{
				add
				{
					Dictionary<ArchetypeId, Action<EntityCreate>> createHandlers = <owner>P._createHandlers;
					ArchetypeId key = <archetypeId>P;
					createHandlers[key] = (Action<EntityCreate>)Delegate.Combine(createHandlers[key], value);
				}
				remove
				{
					Dictionary<ArchetypeId, Action<EntityCreate>> createHandlers = <owner>P._createHandlers;
					ArchetypeId key = <archetypeId>P;
					createHandlers[key] = (Action<EntityCreate>)Delegate.Remove(createHandlers[key], value);
				}
			}

			public event Action<EntityDelete>? OnEntityDelete
			{
				add
				{
					Dictionary<ArchetypeId, Action<EntityDelete>> deleteHandlers = <owner>P._deleteHandlers;
					ArchetypeId key = <archetypeId>P;
					deleteHandlers[key] = (Action<EntityDelete>)Delegate.Combine(deleteHandlers[key], value);
				}
				remove
				{
					Dictionary<ArchetypeId, Action<EntityDelete>> deleteHandlers = <owner>P._deleteHandlers;
					ArchetypeId key = <archetypeId>P;
					deleteHandlers[key] = (Action<EntityDelete>)Delegate.Remove(deleteHandlers[key], value);
				}
			}

			public ArchetypeEntry(ArchetypeEventRouter owner, ArchetypeId archetypeId)
			{
				<owner>P = owner;
				<archetypeId>P = archetypeId;
			}
		}

		private readonly Store _store;

		private readonly Dictionary<ArchetypeId, Action<EntityCreate>?> _createHandlers = new Dictionary<ArchetypeId, Action<EntityCreate>>();

		private readonly Dictionary<ArchetypeId, Action<EntityDelete>?> _deleteHandlers = new Dictionary<ArchetypeId, Action<EntityDelete>>();

		public ArchetypeEntry this[ArchetypeId archetypeId]
		{
			get
			{
				if (!_createHandlers.ContainsKey(archetypeId))
				{
					_createHandlers[archetypeId] = null;
					_deleteHandlers[archetypeId] = null;
				}
				return new ArchetypeEntry(this, archetypeId);
			}
		}

		public ArchetypeEventRouter(Store store)
		{
			_store = store;
			_store.OnEntityCreate += OnEntityCreate;
			_store.OnEntityDelete += OnEntityDelete;
		}

		public void Dispose()
		{
			_store.OnEntityDelete -= OnEntityDelete;
			_store.OnEntityCreate -= OnEntityCreate;
		}

		private void OnEntityCreate(EntityCreate ev)
		{
			if (ev.Entity.TryGetComponent<MetadataComponent>(out var result) && _createHandlers.TryGetValue(result.Archetype, out Action<EntityCreate> value))
			{
				value?.Invoke(ev);
			}
		}

		private void OnEntityDelete(EntityDelete ev)
		{
			if (ev.Entity.TryGetComponent<MetadataComponent>(out var result) && _deleteHandlers.TryGetValue(result.Archetype, out Action<EntityDelete> value))
			{
				value?.Invoke(ev);
			}
		}
	}
}
namespace WukongMp.Api.ECS.Jobs
{
	public readonly struct ClearPlayerTamerRefCountJob : IEach<TamerComponent>
	{
		public ClearPlayerTamerRefCountJob(PlayerId playerId)
		{
			<playerId>P = playerId;
		}

		public void Execute(ref TamerComponent tamer)
		{
			TamerUtils.SubtractSpawnedUnitRefCount(<playerId>P, ref tamer);
		}
	}
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct DiscoverLocallySpawnedMonsters : IEach<LocalTamerComponent, MetadataComponent>
	{
		public void Execute(ref LocalTamerComponent tamer, ref MetadataComponent metadata)
		{
			BUTamerActor? tamer2 = tamer.Tamer;
			if ((UObject)(object)((tamer2 != null) ? tamer2.GetMonster() : null) != (UObject)null)
			{
				Logging.LogDebug("Monster {Guid} is already spawned", BGU_DataUtil.GetActorGuid((AActor)(object)tamer.Tamer, false));
				TamerUtils.MarkMonsterLocallySpawned(ref tamer, metadata);
			}
		}
	}
	public readonly struct SyncMontageJob : IEach<LocalTamerComponent, MetadataComponent>
	{
		public SyncMontageJob(WukongRpcCallbacks rpc, PlayerId ownerPlayerId)
		{
			<rpc>P = rpc;
			<ownerPlayerId>P = ownerPlayerId;
		}

		public void Execute(ref LocalTamerComponent tamerComponent, ref MetadataComponent meta)
		{
			if (meta.Owner != <ownerPlayerId>P || (UObject)(object)tamerComponent.Pawn == (UObject)null || (UObject)(object)((ACharacter)tamerComponent.Pawn).Mesh == (UObject)null)
			{
				return;
			}
			MontageState montageState = tamerComponent.MontageState;
			if ((UObject)(object)montageState.LocalAnimationInstance == (UObject)null)
			{
				montageState.LocalAnimationInstance = ((ACharacter)tamerComponent.Pawn).Mesh.GetAnimInstance();
				if ((UObject)(object)montageState.LocalAnimationInstance == (UObject)null)
				{
					return;
				}
			}
			UAnimMontage currentMontage = ((ACharacter)tamerComponent.Pawn).GetCurrentMontage();
			if ((UObject)(object)currentMontage != (UObject)null)
			{
				bool flag = (UObject)(object)montageState.LocalMontage != (UObject)(object)currentMontage;
				float num = montageState.LocalAnimationInstance.Montage_GetPosition(currentMontage);
				bool flag2 = num < montageState.LocalMontagePosition && !flag;
				bool flag3 = num - montageState.LocalMontagePosition > 0.5f && !flag;
				if (flag || flag2 || flag3)
				{
					<rpc>P.SendMontageCallback(meta.NetId, currentMontage, num, flag2);
				}
				montageState.LocalMontagePosition = num;
			}
			else if ((UObject)(object)montageState.LocalMontage != (UObject)null)
			{
				DI.Instance.Logger.LogDebug("Sent cancel at {Position} for montage {Montage}", montageState.LocalMontagePosition, ((UObject)montageState.LocalMontage).PathName);
				DI.Instance.Rpc.SendMontageCancel(meta.NetId);
			}
			montageState.LocalMontage = currentMontage;
			tamerComponent.MontageState = montageState;
		}
	}
}
namespace WukongMp.Api.ECS.Entities
{
	public readonly struct AreaEntity : Friflo.Engine.ECS.IComponent
	{
		public readonly Entity Entity;

		public bool IsNull => Entity.IsNull;

		public RoomComponent Room => Entity.GetComponent<RoomComponent>();

		public ref AreaScopeComponent Scope => ref Entity.GetComponent<AreaScopeComponent>();

		public AreaEntity(Entity entity)
		{
			Entity = entity;
		}

		public ref MetadataComponent GetMeta()
		{
			return ref Entity.GetComponent<MetadataComponent>();
		}

		public ref RoomComponent GetRoom()
		{
			return ref Entity.GetComponent<RoomComponent>();
		}

		public ref MovieComponent GetMovie()
		{
			return ref Entity.GetComponent<MovieComponent>();
		}
	}
	public readonly struct MainCharacterEntity : IEquatable<MainCharacterEntity>
	{
		public readonly Entity Entity;

		public bool IsNull => Entity.IsNull;

		public MainCharacterEntity(Entity entity)
		{
			Entity = entity;
		}

		public ref MetadataComponent GetMeta()
		{
			return ref Entity.GetComponent<MetadataComponent>();
		}

		public ref MainCharacterComponent GetState()
		{
			return ref Entity.GetComponent<MainCharacterComponent>();
		}

		public ref LocalMainCharacterComponent GetLocalState()
		{
			return ref Entity.GetComponent<LocalMainCharacterComponent>();
		}

		public ref readonly TeamComponent GetTeam()
		{
			return ref Entity.GetComponent<TeamComponent>();
		}

		public ref PvPComponent GetPvP()
		{
			return ref Entity.GetComponent<PvPComponent>();
		}

		public void SetTeam(TeamComponent team)
		{
			Entity.Set(in team);
		}

		public bool Equals(MainCharacterEntity other)
		{
			return Entity.Equals(other.Entity);
		}

		public override bool Equals(object? obj)
		{
			if (obj is MainCharacterEntity other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Entity.GetHashCode();
		}

		public static bool operator ==(MainCharacterEntity left, MainCharacterEntity right)
		{
			return left.Entity == right.Entity;
		}

		public static bool operator !=(MainCharacterEntity left, MainCharacterEntity right)
		{
			return left.Entity != right.Entity;
		}
	}
	public readonly struct PlayerEntity
	{
		public readonly Entity Entity;

		public bool IsNull => Entity.IsNull;

		public PlayerEntity(Entity entity)
		{
			Entity = entity;
		}

		public ref PlayerComponent GetState()
		{
			return ref Entity.GetComponent<PlayerComponent>();
		}
	}
	public readonly struct TamerEntity : IEquatable<TamerEntity>
	{
		public readonly Entity Entity;

		public bool IsNull => Entity.IsNull;

		public TamerEntity(Entity entity)
		{
			Entity = entity;
		}

		public ref MetadataComponent GetMeta()
		{
			return ref Entity.GetComponent<MetadataComponent>();
		}

		public ref readonly TeamComponent GetTeam()
		{
			return ref Entity.GetComponent<TeamComponent>();
		}

		public void SetTeam(TeamComponent team)
		{
			Entity.Set(in team);
		}

		public ref NicknameComponent GetNickname()
		{
			return ref Entity.GetComponent<NicknameComponent>();
		}

		public bool HasMarker()
		{
			return Entity.HasComponent<MarkerComponent>();
		}

		public void AddMarker()
		{
			Entity.AddComponent<MarkerComponent>();
		}

		public ref MarkerComponent GetMarker()
		{
			return ref Entity.GetComponent<MarkerComponent>();
		}

		public ref TransformComponent GetTransform()
		{
			return ref Entity.GetComponent<TransformComponent>();
		}

		public ref TamerComponent GetTamer()
		{
			return ref Entity.GetComponent<TamerComponent>();
		}

		public ref LocalTamerComponent GetLocalTamer()
		{
			return ref Entity.GetComponent<LocalTamerComponent>();
		}

		public ref HpComponent GetHp()
		{
			return ref Entity.GetComponent<HpComponent>();
		}

		public ref AnimationComponent GetAnimation()
		{
			return ref Entity.GetComponent<AnimationComponent>();
		}

		public ref MonsterAnimationComponent GetMonsterAnimation()
		{
			return ref Entity.GetComponent<MonsterAnimationComponent>();
		}

		public bool Equals(TamerEntity other)
		{
			return Entity.Equals(other.Entity);
		}

		public override bool Equals(object? obj)
		{
			if (obj is TamerEntity other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Entity.GetHashCode();
		}

		public static bool operator ==(TamerEntity left, TamerEntity right)
		{
			return left.Entity == right.Entity;
		}

		public static bool operator !=(TamerEntity left, TamerEntity right)
		{
			return left.Entity != right.Entity;
		}
	}
}
namespace WukongMp.Api.ECS.Components
{
	public struct LocalMainCharacterComponent : Friflo.Engine.ECS.IComponent
	{
		private BGUCharacterCS? _pawn;

		public bool IsPlayerSynced;

		public bool IsSpectatorLocally;

		public FVector BeforeSpectatorLocation;

		public bool ShouldDisableCollision;

		[CompilerGenerated]
		private AActor? <MarkerActor>k__BackingField;

		[Ignore]
		public BGUCharacterCS? LastPawn { get; private set; }

		[Ignore]
		public BGUCharacterCS? Pawn
		{
			get
			{
				if (!IsPlayerSynced)
				{
					return null;
				}
				if (((UObject?)(object)_pawn).IsNullOrDestroyed())
				{
					Logging.LogWarning("Player pawn is null or destroyed");
					return null;
				}
				return _pawn;
			}
			set
			{
				LastPawn = _pawn;
				_pawn = value;
			}
		}

		public bool HasPawn => !((UObject?)(object)_pawn).IsNullOrDestroyed();

		public bool IsRespawning { get; set; }

		public bool RunImmobilizePatches { get; set; }

		public MontageState MontageState { get; set; }

		public bool ReceivedPhantomRushExit { get; set; }

		public int TeleportFinishFrames { get; set; }

		public bool IsWaitingForSequence { get; set; }

		public bool IsJoiningSequence { get; set; }

		public FVector JoiningSequenceLocation { get; set; }

		public bool IsInSequence { get; set; }

		public bool InstantSkillCooldown { get; set; }

		public bool HasInfiniteMana { get; set; }

		public bool HasInfiniteVessel { get; set; }

		public bool HasInfiniteTransform { get; set; }

		public bool SpiritCooldownEnabled { get; set; }

		public float SpiritCooldownTime { get; set; }

		public bool ShouldSetSpiritCooldown { get; set; }

		public bool IsDuringDeathAnim { get; set; }

		public float DeadAnimationTime { get; set; }

		[Ignore]
		public AActor? MarkerActor
		{
			get
			{
				if ((UObject)(object)<MarkerActor>k__BackingField != (UObject)null && ((UObject?)(object)<MarkerActor>k__BackingField).IsNullOrDestroyed())
				{
					return null;
				}
				return <MarkerActor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				<MarkerActor>k__BackingField = value;
			}
		}
	}
	public struct LocalTamerComponent : Friflo.Engine.ECS.IComponent
	{
		public bool IsTamerSynced;

		public bool IsMonsterActive;

		public bool RunImmobilizePatches;

		public MontageState MontageState;

		public bool IsLocallySpawned;

		public bool HasPendingUnload;

		public bool IsCheckedForDead;

		private BUTamerActor? _tamer;

		[Ignore]
		public BUTamerActor? Tamer
		{
			get
			{
				if (!((UObject?)(object)_tamer).IsNullOrDestroyed())
				{
					return _tamer;
				}
				return null;
			}
			set
			{
				_tamer = value;
			}
		}

		[Ignore]
		public BGUCharacterCS? Pawn
		{
			get
			{
				if (!IsMonsterActive)
				{
					return null;
				}
				BUTamerActor tamer = Tamer;
				if ((UObject)(object)tamer == (UObject)null)
				{
					Logging.LogDebug("Tamer is null or destroyed in getPawn");
					return null;
				}
				BGUCharacterCS monster = tamer.GetMonster();
				if (!((UObject?)(object)monster).IsNullOrDestroyed())
				{
					return monster;
				}
				return null;
			}
		}

		public bool IsTamerValid => !((UObject?)(object)Tamer).IsNullOrDestroyed();

		public LocalTamerComponent(BUTamerActor tamer)
		{
			IsTamerSynced = false;
			IsMonsterActive = false;
			RunImmobilizePatches = false;
			MontageState = default(MontageState);
			IsLocallySpawned = false;
			HasPendingUnload = false;
			IsCheckedForDead = false;
			_tamer = tamer;
		}
	}
	public struct MarkerComponent : Friflo.Engine.ECS.IComponent
	{
		public bool DestroyQueued;

		private AActor? _markerActor;

		[Ignore]
		public AActor? MarkerActor
		{
			get
			{
				if ((UObject)(object)_markerActor != (UObject)null && ((UObject?)(object)_markerActor).IsNullOrDestroyed())
				{
					return null;
				}
				return _markerActor;
			}
			set
			{
				_markerActor = value;
			}
		}
	}
}
namespace WukongMp.Api.ECS.Archetypes
{
	public class ClientWukongArchetypeRegistration : IArchetypeRegistration
	{
		public ArchetypeId MonsterArchetype { get; private set; }

		public ArchetypeId MainCharacterArchetype { get; private set; }

		public ArchetypeId PvPStateSingletonArchetype { get; private set; }

		public void Register(Store world)
		{
			MonsterArchetype = world.RegisterArchetype(delegate(EntityBuilder b)
			{
				WukongComponentUtils.SetupServerMonsterArchetype(b);
				b.Add<LocalTamerComponent>();
				b.Add<MarkerComponent>();
			});
			MainCharacterArchetype = world.RegisterArchetype(delegate(EntityBuilder b)
			{
				WukongComponentUtils.SetupServerMainCharacterArchetype(b);
				b.Add<LocalMainCharacterComponent>();
			});
			PvPStateSingletonArchetype = world.RegisterArchetype(WukongComponentUtils.SetupServerPvpStateArchetype);
		}
	}
}
namespace WukongMp.Api.DTO
{
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct AnimationSyncingData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<AnimationSyncingData>
		{
			public override AnimationSyncingData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, AnimationSyncingData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Host;

		public NetworkId Guest;

		public bool Compressed;

		public string Montage;

		public AnimationSyncingData(NetworkId host, NetworkId guest, bool compressed, string montage)
		{
			Host = host;
			Guest = guest;
			Compressed = compressed;
			Montage = montage;
		}

		public void Serialize(NetDataWriter writer)
		{
			Host.Serialize(writer);
			Guest.Serialize(writer);
			writer.Put(Compressed);
			writer.Put(Montage);
		}

		public void Deserialize(NetDataReader reader)
		{
			Host.Deserialize(reader);
			Guest.Deserialize(reader);
			Compressed = reader.GetBool();
			Montage = reader.GetString();
		}

		public static AnimationSyncingData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId host = default(NetworkId);
			NetworkId guest = default(NetworkId);
			bool compressed = false;
			string montage = null;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "host":
					host = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "guest":
					guest = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "compressed":
					compressed = reader.GetBoolean();
					break;
				case "montage":
					montage = reader.GetString();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new AnimationSyncingData(host, guest, compressed, montage);
		}

		public static void TextSerialize(Utf8JsonWriter writer, AnimationSyncingData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("host");
			NetworkId.TextSerialize(writer, obj.Host, options);
			writer.WritePropertyName("guest");
			NetworkId.TextSerialize(writer, obj.Guest, options);
			writer.WriteBoolean("compressed", obj.Compressed);
			writer.WriteString("montage", obj.Montage);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct BeginSyncAnimationData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<BeginSyncAnimationData>
		{
			public override BeginSyncAnimationData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, BeginSyncAnimationData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Host;

		public bool Shortened;

		public string GuestMontage;

		public bool bFoundHostSyncPointOnDummyMesh;

		public string SelfSyncPointOnHost;

		public string TargetSyncPointOnHost;

		public string SelfSyncPointOnGuest;

		public bool bForceSyncDummyMeshAnimation;

		public bool bEnableDebugDraw;

		public float NotifyBeginTime;

		public float TotalDuration;

		public int AnimationSyncMontageInstanceId;

		public BeginSyncAnimationData(NetworkId host, bool shortened, string guestMontage, bool bFoundHostSyncPointOnDummyMesh, string selfSyncPointOnHost, string targetSyncPointOnHost, string selfSyncPointOnGuest, bool bForceSyncDummyMeshAnimation, bool bEnableDebugDraw, float notifyBeginTime, float totalDuration, int animationSyncMontageInstanceId)
		{
			Host = host;
			Shortened = shortened;
			GuestMontage = guestMontage;
			this.bFoundHostSyncPointOnDummyMesh = bFoundHostSyncPointOnDummyMesh;
			SelfSyncPointOnHost = selfSyncPointOnHost;
			TargetSyncPointOnHost = targetSyncPointOnHost;
			SelfSyncPointOnGuest = selfSyncPointOnGuest;
			this.bForceSyncDummyMeshAnimation = bForceSyncDummyMeshAnimation;
			this.bEnableDebugDraw = bEnableDebugDraw;
			NotifyBeginTime = notifyBeginTime;
			TotalDuration = totalDuration;
			AnimationSyncMontageInstanceId = animationSyncMontageInstanceId;
		}

		public void Serialize(NetDataWriter writer)
		{
			Host.Serialize(writer);
			writer.Put(Shortened);
			writer.Put(GuestMontage);
			writer.Put(bFoundHostSyncPointOnDummyMesh);
			writer.Put(SelfSyncPointOnHost);
			writer.Put(TargetSyncPointOnHost);
			writer.Put(SelfSyncPointOnGuest);
			writer.Put(bForceSyncDummyMeshAnimation);
			writer.Put(bEnableDebugDraw);
			writer.Put(NotifyBeginTime);
			writer.Put(TotalDuration);
			writer.Put(AnimationSyncMontageInstanceId);
		}

		public void Deserialize(NetDataReader reader)
		{
			Host.Deserialize(reader);
			Shortened = reader.GetBool();
			GuestMontage = reader.GetString();
			bFoundHostSyncPointOnDummyMesh = reader.GetBool();
			SelfSyncPointOnHost = reader.GetString();
			TargetSyncPointOnHost = reader.GetString();
			SelfSyncPointOnGuest = reader.GetString();
			bForceSyncDummyMeshAnimation = reader.GetBool();
			bEnableDebugDraw = reader.GetBool();
			NotifyBeginTime = reader.GetFloat();
			TotalDuration = reader.GetFloat();
			AnimationSyncMontageInstanceId = reader.GetInt();
		}

		public static BeginSyncAnimationData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId host = default(NetworkId);
			bool shortened = false;
			string guestMontage = null;
			bool flag = false;
			string selfSyncPointOnHost = null;
			string targetSyncPointOnHost = null;
			string selfSyncPointOnGuest = null;
			bool flag2 = false;
			bool flag3 = false;
			float notifyBeginTime = 0f;
			float totalDuration = 0f;
			int animationSyncMontageInstanceId = 0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "host":
					host = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "shortened":
					shortened = reader.GetBoolean();
					break;
				case "guestMontage":
					guestMontage = reader.GetString();
					break;
				case "bFoundHostSyncPointOnDummyMesh":
					flag = reader.GetBoolean();
					break;
				case "selfSyncPointOnHost":
					selfSyncPointOnHost = reader.GetString();
					break;
				case "targetSyncPointOnHost":
					targetSyncPointOnHost = reader.GetString();
					break;
				case "selfSyncPointOnGuest":
					selfSyncPointOnGuest = reader.GetString();
					break;
				case "bForceSyncDummyMeshAnimation":
					flag2 = reader.GetBoolean();
					break;
				case "bEnableDebugDraw":
					flag3 = reader.GetBoolean();
					break;
				case "notifyBeginTime":
					notifyBeginTime = reader.GetSingle();
					break;
				case "totalDuration":
					totalDuration = reader.GetSingle();
					break;
				case "animationSyncMontageInstanceId":
					animationSyncMontageInstanceId = reader.GetInt32();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new BeginSyncAnimationData(host, shortened, guestMontage, flag, selfSyncPointOnHost, targetSyncPointOnHost, selfSyncPointOnGuest, flag2, flag3, notifyBeginTime, totalDuration, animationSyncMontageInstanceId);
		}

		public static void TextSerialize(Utf8JsonWriter writer, BeginSyncAnimationData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("host");
			NetworkId.TextSerialize(writer, obj.Host, options);
			writer.WriteBoolean("shortened", obj.Shortened);
			writer.WriteString("guestMontage", obj.GuestMontage);
			writer.WriteBoolean("bFoundHostSyncPointOnDummyMesh", obj.bFoundHostSyncPointOnDummyMesh);
			writer.WriteString("selfSyncPointOnHost", obj.SelfSyncPointOnHost);
			writer.WriteString("targetSyncPointOnHost", obj.TargetSyncPointOnHost);
			writer.WriteString("selfSyncPointOnGuest", obj.SelfSyncPointOnGuest);
			writer.WriteBoolean("bForceSyncDummyMeshAnimation", obj.bForceSyncDummyMeshAnimation);
			writer.WriteBoolean("bEnableDebugDraw", obj.bEnableDebugDraw);
			writer.WriteNumber("notifyBeginTime", obj.NotifyBeginTime);
			writer.WriteNumber("totalDuration", obj.TotalDuration);
			writer.WriteNumber("animationSyncMontageInstanceId", obj.AnimationSyncMontageInstanceId);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct BuffAddData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<BuffAddData>
		{
			public override BuffAddData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, BuffAddData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Id;

		public int BuffId;

		public float Duration;

		public BuffAddData(NetworkId id, int buffId, float duration)
		{
			Id = id;
			BuffId = buffId;
			Duration = duration;
		}

		public void Serialize(NetDataWriter writer)
		{
			Id.Serialize(writer);
			writer.Put(BuffId);
			writer.Put(Duration);
		}

		public void Deserialize(NetDataReader reader)
		{
			Id.Deserialize(reader);
			BuffId = reader.GetInt();
			Duration = reader.GetFloat();
		}

		public static BuffAddData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId id = default(NetworkId);
			int buffId = 0;
			float duration = 0f;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "id":
					id = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "buffId":
					buffId = reader.GetInt32();
					break;
				case "duration":
					duration = reader.GetSingle();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new BuffAddData(id, buffId, duration);
		}

		public static void TextSerialize(Utf8JsonWriter writer, BuffAddData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("id");
			NetworkId.TextSerialize(writer, obj.Id, options);
			writer.WriteNumber("buffId", obj.BuffId);
			writer.WriteNumber("duration", obj.Duration);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct BuffRemoveAllData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<BuffRemoveAllData>
		{
			public override BuffRemoveAllData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, BuffRemoveAllData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Id;

		public EBuffEffectTriggerType TriggerType;

		public bool WithTriggerRemoveEffect;

		public BuffRemoveAllData(NetworkId id, EBuffEffectTriggerType triggerType, bool withTriggerRemoveEffect)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			TriggerType = triggerType;
			WithTriggerRemoveEffect = withTriggerRemoveEffect;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			Id.Serialize(writer);
			writer.Put((int)TriggerType);
			writer.Put(WithTriggerRemoveEffect);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Id.Deserialize(reader);
			TriggerType = (EBuffEffectTriggerType)(byte)reader.GetInt();
			WithTriggerRemoveEffect = reader.GetBool();
		}

		public static BuffRemoveAllData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId id = default(NetworkId);
			EBuffEffectTriggerType triggerType = (EBuffEffectTriggerType)0;
			bool withTriggerRemoveEffect = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "id":
					id = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "triggerType":
					triggerType = JsonSerializer.Deserialize<EBuffEffectTriggerType>(ref reader, options);
					break;
				case "withTriggerRemoveEffect":
					withTriggerRemoveEffect = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new BuffRemoveAllData(id, triggerType, withTriggerRemoveEffect);
		}

		public static void TextSerialize(Utf8JsonWriter writer, BuffRemoveAllData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("id");
			NetworkId.TextSerialize(writer, obj.Id, options);
			writer.WritePropertyName("triggerType");
			JsonSerializer.Serialize<EBuffEffectTriggerType>(writer, obj.TriggerType, options);
			writer.WriteBoolean("withTriggerRemoveEffect", obj.WithTriggerRemoveEffect);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct BuffRemoveData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<BuffRemoveData>
		{
			public override BuffRemoveData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, BuffRemoveData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Id;

		public int BuffId;

		public EBuffEffectTriggerType TriggerType;

		public int Layer;

		public bool WithTriggerRemoveEffect;

		public BuffRemoveData(NetworkId id, int buffId, EBuffEffectTriggerType triggerType, int layer, bool withTriggerRemoveEffect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			BuffId = buffId;
			TriggerType = triggerType;
			Layer = layer;
			WithTriggerRemoveEffect = withTriggerRemoveEffect;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Id.Serialize(writer);
			writer.Put(BuffId);
			writer.Put((int)TriggerType);
			writer.Put(Layer);
			writer.Put(WithTriggerRemoveEffect);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Id.Deserialize(reader);
			BuffId = reader.GetInt();
			TriggerType = (EBuffEffectTriggerType)(byte)reader.GetInt();
			Layer = reader.GetInt();
			WithTriggerRemoveEffect = reader.GetBool();
		}

		public static BuffRemoveData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId id = default(NetworkId);
			int buffId = 0;
			EBuffEffectTriggerType triggerType = (EBuffEffectTriggerType)0;
			int layer = 0;
			bool withTriggerRemoveEffect = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "id":
					id = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "buffId":
					buffId = reader.GetInt32();
					break;
				case "triggerType":
					triggerType = JsonSerializer.Deserialize<EBuffEffectTriggerType>(ref reader, options);
					break;
				case "layer":
					layer = reader.GetInt32();
					break;
				case "withTriggerRemoveEffect":
					withTriggerRemoveEffect = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new BuffRemoveData(id, buffId, triggerType, layer, withTriggerRemoveEffect);
		}

		public static void TextSerialize(Utf8JsonWriter writer, BuffRemoveData obj, JsonSerializerOptions options)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("id");
			NetworkId.TextSerialize(writer, obj.Id, options);
			writer.WriteNumber("buffId", obj.BuffId);
			writer.WritePropertyName("triggerType");
			JsonSerializer.Serialize<EBuffEffectTriggerType>(writer, obj.TriggerType, options);
			writer.WriteNumber("layer", obj.Layer);
			writer.WriteBoolean("withTriggerRemoveEffect", obj.WithTriggerRemoveEffect);
			writer.WriteEndObject();
		}
	}
	public struct ChatMessage : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ChatMessage>
		{
			public override ChatMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ChatMessage value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public PlayerId PlayerId;

		public string? Nickname;

		public string Message;

		public string[] Placeholders;

		private ChatMessage(PlayerId playerId, string? nickname, string message, string[] placeholders)
		{
			PlayerId = playerId;
			Nickname = nickname;
			Message = message;
			Placeholders = placeholders;
		}

		public static ChatMessage CreateServerMessage(string message, string[] placeholders)
		{
			return new ChatMessage(PlayerId.Server, "", message, placeholders);
		}

		public static ChatMessage CreateClientMessage(PlayerId playerId, string nickname, string message)
		{
			return new ChatMessage(playerId, nickname, message, Array.Empty<string>());
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put<PlayerId>(PlayerId);
			writer.Put(Message);
			if (PlayerId != PlayerId.Server)
			{
				writer.Put(Nickname);
			}
			else
			{
				writer.PutArray(Placeholders);
			}
		}

		public void Deserialize(NetDataReader reader)
		{
			PlayerId = reader.Get<PlayerId>();
			Message = reader.GetString();
			if (PlayerId != PlayerId.Server)
			{
				Nickname = reader.GetString();
			}
			else
			{
				Placeholders = reader.GetStringArray();
			}
		}

		public static void TextSerialize(Utf8JsonWriter writer, ChatMessage obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("playerId", (uint)obj.PlayerId.RawValue);
			writer.WriteString("message", obj.Message);
			writer.WriteString("nickname", obj.Nickname);
			writer.WriteStartArray("placeholders");
			writer.WriteEndObject();
		}

		public static ChatMessage TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			PlayerId playerId = PlayerId.Invalid;
			string nickname = null;
			string message = "";
			string[] array = null;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "playerId":
					playerId = new PlayerId((ushort)reader.GetUInt32());
					break;
				case "nickname":
					nickname = reader.GetString();
					break;
				case "message":
					message = reader.GetString();
					break;
				case "placeholders":
					array = JsonSerializer.Deserialize<string[]>(ref reader, options);
					break;
				default:
					throw new JsonException("Unexpected property: " + text);
				}
			}
			return new ChatMessage(playerId, nickname, message, array ?? Array.Empty<string>());
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct FsmStateData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<FsmStateData>
		{
			public override FsmStateData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, FsmStateData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetId;

		public string FsmStateName;

		public FsmStateData(NetworkId netId, string fsmStateName)
		{
			NetId = netId;
			FsmStateName = fsmStateName;
		}

		public void Serialize(NetDataWriter writer)
		{
			NetId.Serialize(writer);
			writer.Put(FsmStateName);
		}

		public void Deserialize(NetDataReader reader)
		{
			NetId.Deserialize(reader);
			FsmStateName = reader.GetString();
		}

		public static FsmStateData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			string fsmStateName = null;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "netId")
				{
					netId = NetworkId.TextDeserialize(ref reader, options);
				}
				else if (text == "fsmStateName")
				{
					fsmStateName = reader.GetString();
				}
				else
				{
					reader.Skip();
				}
			}
			return new FsmStateData(netId, fsmStateName);
		}

		public static void TextSerialize(Utf8JsonWriter writer, FsmStateData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("netId");
			NetworkId.TextSerialize(writer, obj.NetId, options);
			writer.WriteString("fsmStateName", obj.FsmStateName);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct TriggerImmobilizeData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<TriggerImmobilizeData>
		{
			public override TriggerImmobilizeData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, TriggerImmobilizeData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId PlayerId;

		public NetworkId Target;

		public bool GreatSageTalentActiveBuff;

		public TriggerImmobilizeData(NetworkId playerId, NetworkId target, bool greatSageTalentActiveBuff)
		{
			PlayerId = playerId;
			Target = target;
			GreatSageTalentActiveBuff = greatSageTalentActiveBuff;
		}

		public void Serialize(NetDataWriter writer)
		{
			PlayerId.Serialize(writer);
			Target.Serialize(writer);
			writer.Put(GreatSageTalentActiveBuff);
		}

		public void Deserialize(NetDataReader reader)
		{
			PlayerId.Deserialize(reader);
			Target.Deserialize(reader);
			GreatSageTalentActiveBuff = reader.GetBool();
		}

		public static TriggerImmobilizeData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId playerId = default(NetworkId);
			NetworkId target = default(NetworkId);
			bool greatSageTalentActiveBuff = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "playerId":
					playerId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "target":
					target = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "greatSageTalentActiveBuff":
					greatSageTalentActiveBuff = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new TriggerImmobilizeData(playerId, target, greatSageTalentActiveBuff);
		}

		public static void TextSerialize(Utf8JsonWriter writer, TriggerImmobilizeData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("playerId");
			NetworkId.TextSerialize(writer, obj.PlayerId, options);
			writer.WritePropertyName("target");
			NetworkId.TextSerialize(writer, obj.Target, options);
			writer.WriteBoolean("greatSageTalentActiveBuff", obj.GreatSageTalentActiveBuff);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	public struct MagicallyChangeData : INetSerializable
	{
		public string ConfigAssetName;

		public int SkillID;

		public int RecoverSkillID;

		public int CurVigorSkillID;

		public ECastReason_MagicallyChange CastReason;

		public bool Compressed;

		public MagicallyChangeData(string configAssetName, bool compressed, int skillID, int recoverSkillID, int curVigorSkillID, ECastReason_MagicallyChange castReason)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			ConfigAssetName = configAssetName;
			SkillID = skillID;
			RecoverSkillID = recoverSkillID;
			CurVigorSkillID = curVigorSkillID;
			CastReason = castReason;
			Compressed = compressed;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected I4, but got Unknown
			writer.Put(ConfigAssetName);
			writer.Put(SkillID);
			writer.Put(RecoverSkillID);
			writer.Put(CurVigorSkillID);
			writer.Put((int)CastReason);
			writer.Put(Compressed);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			ConfigAssetName = reader.GetString();
			SkillID = reader.GetInt();
			RecoverSkillID = reader.GetInt();
			CurVigorSkillID = reader.GetInt();
			CastReason = (ECastReason_MagicallyChange)reader.GetInt();
			Compressed = reader.GetBool();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct MontageCallbackData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<MontageCallbackData>
		{
			public override MontageCallbackData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, MontageCallbackData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetId;

		public bool Compressed;

		public string MontagePath;

		public float Position;

		public bool Reset;

		public MontageCallbackData(NetworkId netId, bool compressed, string montagePath, float position, bool reset)
		{
			NetId = netId;
			Compressed = compressed;
			MontagePath = montagePath;
			Position = position;
			Reset = reset;
		}

		public void Serialize(NetDataWriter writer)
		{
			NetId.Serialize(writer);
			writer.Put(Compressed);
			writer.Put(MontagePath);
			writer.Put(Position);
			writer.Put(Reset);
		}

		public void Deserialize(NetDataReader reader)
		{
			NetId.Deserialize(reader);
			Compressed = reader.GetBool();
			MontagePath = reader.GetString();
			Position = reader.GetFloat();
			Reset = reader.GetBool();
		}

		public static MontageCallbackData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			bool compressed = false;
			string montagePath = null;
			float position = 0f;
			bool reset = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "netId":
					netId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "compressed":
					compressed = reader.GetBoolean();
					break;
				case "montagePath":
					montagePath = reader.GetString();
					break;
				case "position":
					position = reader.GetSingle();
					break;
				case "reset":
					reset = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new MontageCallbackData(netId, compressed, montagePath, position, reset);
		}

		public static void TextSerialize(Utf8JsonWriter writer, MontageCallbackData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("netId");
			NetworkId.TextSerialize(writer, obj.NetId, options);
			writer.WriteBoolean("compressed", obj.Compressed);
			writer.WriteString("montagePath", obj.MontagePath);
			writer.WriteNumber("position", obj.Position);
			writer.WriteBoolean("reset", obj.Reset);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct MotionMatchingStateData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<MotionMatchingStateData>
		{
			public override MotionMatchingStateData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, MotionMatchingStateData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetId;

		public EState_MM State;

		public MotionMatchingStateData(NetworkId netId, EState_MM state)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			NetId = netId;
			State = state;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			NetId.Serialize(writer);
			writer.Put((int)State);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			NetId.Deserialize(reader);
			State = (EState_MM)(byte)reader.GetInt();
		}

		public static MotionMatchingStateData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			EState_MM state = (EState_MM)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "netId")
				{
					netId = NetworkId.TextDeserialize(ref reader, options);
				}
				else if (text == "state")
				{
					state = JsonSerializer.Deserialize<EState_MM>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new MotionMatchingStateData(netId, state);
		}

		public static void TextSerialize(Utf8JsonWriter writer, MotionMatchingStateData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("netId");
			NetworkId.TextSerialize(writer, obj.NetId, options);
			writer.WritePropertyName("state");
			JsonSerializer.Serialize<EState_MM>(writer, obj.State, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayBaneEffectData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayBaneEffectData>
		{
			public override PlayBaneEffectData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayBaneEffectData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Id;

		public EAbnormalStateType StateType;

		public EAbnromalDispActionType ActionType;

		public PlayBaneEffectData(NetworkId id, EAbnormalStateType stateType, EAbnromalDispActionType actionType)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			StateType = stateType;
			ActionType = actionType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Id.Serialize(writer);
			writer.Put((int)StateType);
			writer.Put((int)ActionType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Id.Deserialize(reader);
			StateType = (EAbnormalStateType)(byte)reader.GetInt();
			ActionType = (EAbnromalDispActionType)(byte)reader.GetInt();
		}

		public static PlayBaneEffectData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId id = default(NetworkId);
			EAbnormalStateType stateType = (EAbnormalStateType)0;
			EAbnromalDispActionType actionType = (EAbnromalDispActionType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "id":
					id = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "stateType":
					stateType = JsonSerializer.Deserialize<EAbnormalStateType>(ref reader, options);
					break;
				case "actionType":
					actionType = JsonSerializer.Deserialize<EAbnromalDispActionType>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new PlayBaneEffectData(id, stateType, actionType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayBaneEffectData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("id");
			NetworkId.TextSerialize(writer, obj.Id, options);
			writer.WritePropertyName("stateType");
			JsonSerializer.Serialize<EAbnormalStateType>(writer, obj.StateType, options);
			writer.WritePropertyName("actionType");
			JsonSerializer.Serialize<EAbnromalDispActionType>(writer, obj.ActionType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayerTransBeginData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayerTransBeginData>
		{
			public override PlayerTransBeginData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayerTransBeginData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public int UnitResId;

		public int UnitBornSkillId;

		public bool EnableBlendViewTarget;

		public EPlayerTransBeginType TransBeginType;

		public PlayerTransBeginData(int unitResId, int unitBornSkillId, bool enbleBlendViewTarget, EPlayerTransBeginType transBeginType)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			UnitResId = unitResId;
			UnitBornSkillId = unitBornSkillId;
			EnableBlendViewTarget = enbleBlendViewTarget;
			TransBeginType = transBeginType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			writer.Put(UnitResId);
			writer.Put(UnitBornSkillId);
			writer.Put(EnableBlendViewTarget);
			writer.Put((int)TransBeginType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			UnitResId = reader.GetInt();
			UnitBornSkillId = reader.GetInt();
			EnableBlendViewTarget = reader.GetBool();
			TransBeginType = (EPlayerTransBeginType)(byte)reader.GetInt();
		}

		public static PlayerTransBeginData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			int unitResId = 0;
			int unitBornSkillId = 0;
			bool enbleBlendViewTarget = false;
			EPlayerTransBeginType transBeginType = (EPlayerTransBeginType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "unitResId":
					unitResId = reader.GetInt32();
					break;
				case "unitBornSkillId":
					unitBornSkillId = reader.GetInt32();
					break;
				case "enableBlendViewTarget":
					enbleBlendViewTarget = reader.GetBoolean();
					break;
				case "transBeginType":
					transBeginType = JsonSerializer.Deserialize<EPlayerTransBeginType>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new PlayerTransBeginData(unitResId, unitBornSkillId, enbleBlendViewTarget, transBeginType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayerTransBeginData obj, JsonSerializerOptions options)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteNumber("unitResId", obj.UnitResId);
			writer.WriteNumber("unitBornSkillId", obj.UnitBornSkillId);
			writer.WriteBoolean("enableBlendViewTarget", obj.EnableBlendViewTarget);
			writer.WritePropertyName("transBeginType");
			JsonSerializer.Serialize<EPlayerTransBeginType>(writer, obj.TransBeginType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayerTransEndData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayerTransEndData>
		{
			public override PlayerTransEndData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayerTransEndData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public int UnitResId;

		public int UnitBornSkillId;

		public bool EnableBlendViewTarget;

		public EPlayerTransEndType TransEndType;

		public PlayerTransEndData(int unitResId, int unitBornSkillId, bool enbleBlendViewTarget, EPlayerTransEndType transEndType)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			UnitResId = unitResId;
			UnitBornSkillId = unitBornSkillId;
			EnableBlendViewTarget = enbleBlendViewTarget;
			TransEndType = transEndType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			writer.Put(UnitResId);
			writer.Put(UnitBornSkillId);
			writer.Put(EnableBlendViewTarget);
			writer.Put((int)TransEndType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			UnitResId = reader.GetInt();
			UnitBornSkillId = reader.GetInt();
			EnableBlendViewTarget = reader.GetBool();
			TransEndType = (EPlayerTransEndType)(byte)reader.GetInt();
		}

		public static PlayerTransEndData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			int unitResId = 0;
			int unitBornSkillId = 0;
			bool enbleBlendViewTarget = false;
			EPlayerTransEndType transEndType = (EPlayerTransEndType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "unitResId":
					unitResId = reader.GetInt32();
					break;
				case "unitBornSkillId":
					unitBornSkillId = reader.GetInt32();
					break;
				case "enableBlendViewTarget":
					enbleBlendViewTarget = reader.GetBoolean();
					break;
				case "transEndType":
					transEndType = JsonSerializer.Deserialize<EPlayerTransEndType>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new PlayerTransEndData(unitResId, unitBornSkillId, enbleBlendViewTarget, transEndType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayerTransEndData obj, JsonSerializerOptions options)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteNumber("unitResId", obj.UnitResId);
			writer.WriteNumber("unitBornSkillId", obj.UnitBornSkillId);
			writer.WriteBoolean("enableBlendViewTarget", obj.EnableBlendViewTarget);
			writer.WritePropertyName("transEndType");
			JsonSerializer.Serialize<EPlayerTransEndType>(writer, obj.TransEndType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayerTransformData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayerTransformData>
		{
			public override PlayerTransformData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayerTransformData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public PlayerId PlayerId;

		public FVector Location;

		public FRotator Rotation;

		public PlayerTransformData(PlayerId playerId, FVector location, FRotator rotation)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			PlayerId = playerId;
			Location = location;
			Rotation = rotation;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			writer.Put<PlayerId>(PlayerId);
			SerializationHelpers.SerializeFVector(writer, Location);
			SerializationHelpers.SerializeFRotator(writer, Rotation);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			PlayerId = reader.Get<PlayerId>();
			Location = (FVector)SerializationHelpers.DeserializeFVector(reader);
			Rotation = (FRotator)SerializationHelpers.DeserializeFRotator(reader);
		}

		public static PlayerTransformData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			PlayerId playerId = default(PlayerId);
			FVector location = default(FVector);
			FRotator rotation = default(FRotator);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "playerId":
					playerId = PlayerId.TextDeserialize(ref reader, options);
					break;
				case "location":
					location = JsonSerializer.Deserialize<FVector>(ref reader, options);
					break;
				case "rotation":
					rotation = JsonSerializer.Deserialize<FRotator>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new PlayerTransformData(playerId, location, rotation);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayerTransformData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("playerId");
			PlayerId.TextSerialize(writer, obj.PlayerId, options);
			writer.WritePropertyName("location");
			JsonSerializer.Serialize<FVector>(writer, obj.Location, options);
			writer.WritePropertyName("rotation");
			JsonSerializer.Serialize<FRotator>(writer, obj.Rotation, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct PlayMovieData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<PlayMovieData>
		{
			public override PlayMovieData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, PlayMovieData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public int SequenceId;

		public bool DisablePlayerControl;

		public bool DisableMovementInput;

		public bool DisableLookAtInput;

		public bool HidePlayer;

		public bool HideHud;

		public string OverlapBoxGuid;

		public ESequenceBlendInMatchPositionType MatchType;

		public PlayMovieData(int sequenceID, bool disablePlayerControl, bool disableMovementInput, bool disableLookAtInput, bool hidePlayer, bool hideHud, string overlapBoxGuid, ESequenceBlendInMatchPositionType matchType)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			SequenceId = sequenceID;
			DisablePlayerControl = disablePlayerControl;
			DisableMovementInput = disableMovementInput;
			DisableLookAtInput = disableLookAtInput;
			HidePlayer = hidePlayer;
			HideHud = hideHud;
			OverlapBoxGuid = overlapBoxGuid;
			MatchType = matchType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			writer.Put(SequenceId);
			writer.Put(DisablePlayerControl);
			writer.Put(DisableMovementInput);
			writer.Put(DisableLookAtInput);
			writer.Put(HidePlayer);
			writer.Put(HideHud);
			writer.Put(OverlapBoxGuid);
			writer.Put((int)MatchType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			SequenceId = reader.GetInt();
			DisablePlayerControl = reader.GetBool();
			DisableMovementInput = reader.GetBool();
			DisableLookAtInput = reader.GetBool();
			HidePlayer = reader.GetBool();
			HideHud = reader.GetBool();
			OverlapBoxGuid = reader.GetString();
			MatchType = (ESequenceBlendInMatchPositionType)(byte)reader.GetInt();
		}

		public static PlayMovieData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			int sequenceID = 0;
			bool disablePlayerControl = false;
			bool disableMovementInput = false;
			bool disableLookAtInput = false;
			bool hidePlayer = false;
			bool hideHud = false;
			string overlapBoxGuid = null;
			ESequenceBlendInMatchPositionType matchType = (ESequenceBlendInMatchPositionType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "sequenceId":
					sequenceID = reader.GetInt32();
					break;
				case "disablePlayerControl":
					disablePlayerControl = reader.GetBoolean();
					break;
				case "disableMovementInput":
					disableMovementInput = reader.GetBoolean();
					break;
				case "disableLookAtInput":
					disableLookAtInput = reader.GetBoolean();
					break;
				case "hidePlayer":
					hidePlayer = reader.GetBoolean();
					break;
				case "hideHud":
					hideHud = reader.GetBoolean();
					break;
				case "overlapBoxGuid":
					overlapBoxGuid = reader.GetString();
					break;
				case "matchType":
					matchType = JsonSerializer.Deserialize<ESequenceBlendInMatchPositionType>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new PlayMovieData(sequenceID, disablePlayerControl, disableMovementInput, disableLookAtInput, hidePlayer, hideHud, overlapBoxGuid, matchType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, PlayMovieData obj, JsonSerializerOptions options)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteNumber("sequenceId", obj.SequenceId);
			writer.WriteBoolean("disablePlayerControl", obj.DisablePlayerControl);
			writer.WriteBoolean("disableMovementInput", obj.DisableMovementInput);
			writer.WriteBoolean("disableLookAtInput", obj.DisableLookAtInput);
			writer.WriteBoolean("hidePlayer", obj.HidePlayer);
			writer.WriteBoolean("hideHud", obj.HideHud);
			writer.WriteString("overlapBoxGuid", obj.OverlapBoxGuid);
			writer.WritePropertyName("matchType");
			JsonSerializer.Serialize<ESequenceBlendInMatchPositionType>(writer, obj.MatchType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ProjectileDeadData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ProjectileDeadData>
		{
			public override ProjectileDeadData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ProjectileDeadData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string ProjectileClassName;

		public EBGUBulletDestroyReason Reason;

		public ProjectileDeadData(string projectileClassName, EBGUBulletDestroyReason reason)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ProjectileClassName = projectileClassName;
			Reason = reason;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			writer.Put(ProjectileClassName);
			writer.Put((int)Reason);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			ProjectileClassName = reader.GetString();
			Reason = (EBGUBulletDestroyReason)(byte)reader.GetInt();
		}

		public static ProjectileDeadData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string projectileClassName = null;
			EBGUBulletDestroyReason reason = (EBGUBulletDestroyReason)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "projectileClassName")
				{
					projectileClassName = reader.GetString();
				}
				else if (text == "reason")
				{
					reason = JsonSerializer.Deserialize<EBGUBulletDestroyReason>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new ProjectileDeadData(projectileClassName, reason);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ProjectileDeadData obj, JsonSerializerOptions options)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteString("projectileClassName", obj.ProjectileClassName);
			writer.WritePropertyName("reason");
			JsonSerializer.Serialize<EBGUBulletDestroyReason>(writer, obj.Reason, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ProjectileMoveModeData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ProjectileMoveModeData>
		{
			public override ProjectileMoveModeData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ProjectileMoveModeData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string ProjectileClassName;

		public EBulletOrMagicFieldMoveModeType MoveMode;

		public ProjectileMoveModeData(string projectileClassName, EBulletOrMagicFieldMoveModeType moveMode)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ProjectileClassName = projectileClassName;
			MoveMode = moveMode;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			writer.Put(ProjectileClassName);
			writer.Put((int)MoveMode);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			ProjectileClassName = reader.GetString();
			MoveMode = (EBulletOrMagicFieldMoveModeType)(byte)reader.GetInt();
		}

		public static ProjectileMoveModeData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string projectileClassName = null;
			EBulletOrMagicFieldMoveModeType moveMode = (EBulletOrMagicFieldMoveModeType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "projectileClassName")
				{
					projectileClassName = reader.GetString();
				}
				else if (text == "moveMode")
				{
					moveMode = JsonSerializer.Deserialize<EBulletOrMagicFieldMoveModeType>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new ProjectileMoveModeData(projectileClassName, moveMode);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ProjectileMoveModeData obj, JsonSerializerOptions options)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteString("projectileClassName", obj.ProjectileClassName);
			writer.WritePropertyName("moveMode");
			JsonSerializer.Serialize<EBulletOrMagicFieldMoveModeType>(writer, obj.MoveMode, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ProjectileSwitchData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ProjectileSwitchData>
		{
			public override ProjectileSwitchData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ProjectileSwitchData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string ProjectileClassName;

		public int BulletSwitchID;

		public int SwitchIdx;

		public ProjectileSwitchData(string projectileClassName, int bulletSwitchID, int switchIdx)
		{
			ProjectileClassName = projectileClassName;
			BulletSwitchID = bulletSwitchID;
			SwitchIdx = switchIdx;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(ProjectileClassName);
			writer.Put(BulletSwitchID);
			writer.Put(SwitchIdx);
		}

		public void Deserialize(NetDataReader reader)
		{
			ProjectileClassName = reader.GetString();
			BulletSwitchID = reader.GetInt();
			SwitchIdx = reader.GetInt();
		}

		public static ProjectileSwitchData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string projectileClassName = null;
			int bulletSwitchID = 0;
			int switchIdx = 0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "projectileClassName":
					projectileClassName = reader.GetString();
					break;
				case "bulletSwitchID":
					bulletSwitchID = reader.GetInt32();
					break;
				case "switchIdx":
					switchIdx = reader.GetInt32();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new ProjectileSwitchData(projectileClassName, bulletSwitchID, switchIdx);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ProjectileSwitchData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteString("projectileClassName", obj.ProjectileClassName);
			writer.WriteNumber("bulletSwitchID", obj.BulletSwitchID);
			writer.WriteNumber("switchIdx", obj.SwitchIdx);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct ProjectileTargetData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<ProjectileTargetData>
		{
			public override ProjectileTargetData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, ProjectileTargetData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string ProjectileName;

		public NetworkId Target;

		public string SocketName;

		public ProjectileTargetData(string projectileName, NetworkId target, string socketName)
		{
			ProjectileName = projectileName;
			Target = target;
			SocketName = socketName;
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(ProjectileName);
			Target.Serialize(writer);
			writer.Put(SocketName);
		}

		public void Deserialize(NetDataReader reader)
		{
			ProjectileName = reader.GetString();
			Target.Deserialize(reader);
			SocketName = reader.GetString();
		}

		public static ProjectileTargetData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string projectileName = null;
			NetworkId target = default(NetworkId);
			string socketName = null;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "projectileName":
					projectileName = reader.GetString();
					break;
				case "target":
					target = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "socketName":
					socketName = reader.GetString();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new ProjectileTargetData(projectileName, target, socketName);
		}

		public static void TextSerialize(Utf8JsonWriter writer, ProjectileTargetData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteString("projectileName", obj.ProjectileName);
			writer.WritePropertyName("target");
			NetworkId.TextSerialize(writer, obj.Target, options);
			writer.WriteString("socketName", obj.SocketName);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct SequenceWaitingData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<SequenceWaitingData>
		{
			public override SequenceWaitingData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, SequenceWaitingData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public int SequenceID;

		public FVector SequenceLocation;

		public SequenceWaitingData(int sequenceID, FVector sequenceLocation)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			SequenceID = sequenceID;
			SequenceLocation = sequenceLocation;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			writer.Put(SequenceID);
			SerializationHelpers.SerializeFVector(writer, SequenceLocation);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			SequenceID = reader.GetInt();
			SequenceLocation = (FVector)SerializationHelpers.DeserializeFVector(reader);
		}

		public static SequenceWaitingData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			int sequenceID = 0;
			FVector sequenceLocation = default(FVector);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "sequenceID")
				{
					sequenceID = reader.GetInt32();
				}
				else if (text == "sequenceLocation")
				{
					sequenceLocation = JsonSerializer.Deserialize<FVector>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new SequenceWaitingData(sequenceID, sequenceLocation);
		}

		public static void TextSerialize(Utf8JsonWriter writer, SequenceWaitingData obj, JsonSerializerOptions options)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteNumber("sequenceID", obj.SequenceID);
			writer.WritePropertyName("sequenceLocation");
			JsonSerializer.Serialize<FVector>(writer, obj.SequenceLocation, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct SimpleStateData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<SimpleStateData>
		{
			public override SimpleStateData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, SimpleStateData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetId;

		public EBGUSimpleState SimpleState;

		public bool IsRemove;

		public SimpleStateData(NetworkId netId, EBGUSimpleState simpleState, bool isRemove)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			NetId = netId;
			SimpleState = simpleState;
			IsRemove = isRemove;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			NetId.Serialize(writer);
			writer.Put((int)SimpleState);
			writer.Put(IsRemove);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			NetId.Deserialize(reader);
			SimpleState = (EBGUSimpleState)(byte)reader.GetInt();
			IsRemove = reader.GetBool();
		}

		public static SimpleStateData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			EBGUSimpleState simpleState = (EBGUSimpleState)0;
			bool isRemove = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "netId":
					netId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "simpleState":
					simpleState = JsonSerializer.Deserialize<EBGUSimpleState>(ref reader, options);
					break;
				case "isRemove":
					isRemove = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new SimpleStateData(netId, simpleState, isRemove);
		}

		public static void TextSerialize(Utf8JsonWriter writer, SimpleStateData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("netId");
			NetworkId.TextSerialize(writer, obj.NetId, options);
			writer.WritePropertyName("simpleState");
			JsonSerializer.Serialize<EBGUSimpleState>(writer, obj.SimpleState, options);
			writer.WriteBoolean("isRemove", obj.IsRemove);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	public struct SkillInteractData : INetSerializable
	{
		public NetworkId InteractiveId;

		public int SkillId;

		public SkillInteractData(NetworkId interactiveId, int skillId)
		{
			InteractiveId = interactiveId;
			SkillId = skillId;
		}

		public void Serialize(NetDataWriter writer)
		{
			InteractiveId.Serialize(writer);
			writer.Put(SkillId);
		}

		public void Deserialize(NetDataReader reader)
		{
			InteractiveId.Deserialize(reader);
			SkillId = reader.GetInt();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct StartJumpData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<StartJumpData>
		{
			public override StartJumpData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, StartJumpData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public ESkillDirection StartJumpDir;

		public FVector2D InputVector;

		public StartJumpData(ESkillDirection startJumpDir, FVector2D inputVector)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			StartJumpDir = startJumpDir;
			InputVector = inputVector;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected I4, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			writer.Put((byte)(int)StartJumpDir);
			SerializationHelpers.SerializeFVector2D(writer, InputVector);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			StartJumpDir = (ESkillDirection)reader.GetByte();
			InputVector = (FVector2D)SerializationHelpers.DeserializeFVector2D(reader);
		}

		public static StartJumpData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			ESkillDirection startJumpDir = (ESkillDirection)0;
			FVector2D inputVector = default(FVector2D);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "startJumpDir")
				{
					startJumpDir = JsonSerializer.Deserialize<ESkillDirection>(ref reader, options);
				}
				else if (text == "inputVector")
				{
					inputVector = JsonSerializer.Deserialize<FVector2D>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new StartJumpData(startJumpDir, inputVector);
		}

		public static void TextSerialize(Utf8JsonWriter writer, StartJumpData obj, JsonSerializerOptions options)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("startJumpDir");
			JsonSerializer.Serialize<ESkillDirection>(writer, obj.StartJumpDir, options);
			writer.WritePropertyName("inputVector");
			JsonSerializer.Serialize<FVector2D>(writer, obj.InputVector, options);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct StateTriggerData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<StateTriggerData>
		{
			public override StateTriggerData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, StateTriggerData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetId;

		public EBUStateTrigger Trigger;

		public float Time;

		public bool NeedForceUpdate;

		public StateTriggerData(NetworkId netId, EBUStateTrigger trigger, float time, bool needForceUpdate)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			NetId = netId;
			Trigger = trigger;
			Time = time;
			NeedForceUpdate = needForceUpdate;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			NetId.Serialize(writer);
			writer.Put((int)Trigger);
			writer.Put(Time);
			writer.Put(NeedForceUpdate);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			NetId.Deserialize(reader);
			Trigger = (EBUStateTrigger)(byte)reader.GetInt();
			Time = reader.GetFloat();
			NeedForceUpdate = reader.GetBool();
		}

		public static StateTriggerData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			EBUStateTrigger trigger = (EBUStateTrigger)0;
			float time = 0f;
			bool needForceUpdate = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "netId":
					netId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "trigger":
					trigger = JsonSerializer.Deserialize<EBUStateTrigger>(ref reader, options);
					break;
				case "time":
					time = reader.GetSingle();
					break;
				case "needForceUpdate":
					needForceUpdate = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new StateTriggerData(netId, trigger, time, needForceUpdate);
		}

		public static void TextSerialize(Utf8JsonWriter writer, StateTriggerData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("netId");
			NetworkId.TextSerialize(writer, obj.NetId, options);
			writer.WritePropertyName("trigger");
			JsonSerializer.Serialize<EBUStateTrigger>(writer, obj.Trigger, options);
			writer.WriteNumber("time", obj.Time);
			writer.WriteBoolean("needForceUpdate", obj.NeedForceUpdate);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct StopBaneEffectData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<StopBaneEffectData>
		{
			public override StopBaneEffectData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, StopBaneEffectData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Id;

		public EAbnormalStateType StateType;

		public StopBaneEffectData(NetworkId id, EAbnormalStateType stateType)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			StateType = stateType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			Id.Serialize(writer);
			writer.Put((int)StateType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Id.Deserialize(reader);
			StateType = (EAbnormalStateType)(byte)reader.GetInt();
		}

		public static StopBaneEffectData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId id = default(NetworkId);
			EAbnormalStateType stateType = (EAbnormalStateType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				if (text == "id")
				{
					id = NetworkId.TextDeserialize(ref reader, options);
				}
				else if (text == "stateType")
				{
					stateType = JsonSerializer.Deserialize<EAbnormalStateType>(ref reader, options);
				}
				else
				{
					reader.Skip();
				}
			}
			return new StopBaneEffectData(id, stateType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, StopBaneEffectData obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("id");
			NetworkId.TextSerialize(writer, obj.Id, options);
			writer.WritePropertyName("stateType");
			JsonSerializer.Serialize<EAbnormalStateType>(writer, obj.StateType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct SummonRequestData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<SummonRequestData>
		{
			public override SummonRequestData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, SummonRequestData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId SummonerId;

		public string SummonGuid;

		public string SummonClassPath;

		public FVector Location;

		public FRotator Rotation;

		public bool SafeClampToLand;

		public int SummonId;

		public Guid SummonInstanceId;

		public EServantType ServantType;

		public EServantSearchTargetType SearchTargetType;

		public string CooperativeSCGuid;

		public float AliveTime;

		public NetworkId CatchTargetId;

		public float DelayBornTime;

		public string BornMontagePath;

		public int BornSkill;

		public float DelayEffectTime;

		public float DelaySummonTime;

		public bool IsSummonerAsMaster;

		public EquipmentState EquipmentState;

		public float InitSpeed;

		public string BornEffectPath;

		public List<string> DisappearMontagePathList;

		public float DestroyDelayTime;

		public SummonRequestData(NetworkId summonerId, string summonGuid, string summonClassPath)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Location = default(FVector);
			Rotation = default(FRotator);
			SafeClampToLand = false;
			SummonId = 0;
			ServantType = (EServantType)0;
			SearchTargetType = (EServantSearchTargetType)0;
			AliveTime = 0f;
			CatchTargetId = default(NetworkId);
			DelayBornTime = 0f;
			BornSkill = 0;
			DelayEffectTime = 0f;
			DelaySummonTime = 0f;
			IsSummonerAsMaster = false;
			EquipmentState = default(EquipmentState);
			InitSpeed = 0f;
			DestroyDelayTime = 0f;
			SummonerId = summonerId;
			SummonGuid = summonGuid;
			SummonClassPath = summonClassPath;
			CooperativeSCGuid = "";
			BornMontagePath = "";
			BornEffectPath = "";
			DisappearMontagePathList = new List<string>();
		}

		public SummonRequestData(NetworkId summonerId, string summonGuid, string summonClassPath, FVector location, FRotator rotation, bool safeClampToLand, int summonId, Guid summonInstanceId, EServantType servantType, EServantSearchTargetType searchTargetType, string cooperativeSCGuid, float aliveTime, NetworkId catchTargetId, float delayBornTime, string bornMontagePath, int bornSkill, float delayEffectTime, float delaySummonTime, bool isSummonerAsMaster, EquipmentState equipmentState, float initSpeed, string bornEffectPath, List<string> disappearMontagePathList, float destroyDelayTime)
			: this(summonerId, summonGuid, summonClassPath)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Location = location;
			Rotation = rotation;
			SafeClampToLand = safeClampToLand;
			SummonId = summonId;
			SummonInstanceId = summonInstanceId;
			ServantType = servantType;
			SearchTargetType = searchTargetType;
			CooperativeSCGuid = cooperativeSCGuid;
			AliveTime = aliveTime;
			CatchTargetId = catchTargetId;
			DelayBornTime = delayBornTime;
			BornMontagePath = bornMontagePath;
			BornSkill = bornSkill;
			DelayEffectTime = delayEffectTime;
			DelaySummonTime = delaySummonTime;
			IsSummonerAsMaster = isSummonerAsMaster;
			EquipmentState = equipmentState;
			InitSpeed = initSpeed;
			BornEffectPath = bornEffectPath;
			DisappearMontagePathList = disappearMontagePathList;
			DestroyDelayTime = destroyDelayTime;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected I4, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected I4, but got Unknown
			writer.Put<NetworkId>(SummonerId);
			writer.Put(SummonGuid);
			writer.Put(SummonClassPath);
			SerializationHelpers.SerializeFVector(writer, Location);
			SerializationHelpers.SerializeFRotator(writer, Rotation);
			writer.Put(SafeClampToLand);
			writer.Put(SummonId);
			writer.Put(SummonInstanceId);
			writer.Put((byte)(int)ServantType);
			writer.Put((byte)(int)SearchTargetType);
			writer.Put(CooperativeSCGuid);
			writer.Put(AliveTime);
			writer.Put<NetworkId>(CatchTargetId);
			writer.Put(DelayBornTime);
			writer.Put(BornMontagePath);
			writer.Put(BornSkill);
			writer.Put(DelayEffectTime);
			writer.Put(DelaySummonTime);
			writer.Put(IsSummonerAsMaster);
			EquipmentState.Serialize(writer);
			writer.Put(InitSpeed);
			writer.Put(BornEffectPath);
			writer.Put(DisappearMontagePathList.Count);
			foreach (string disappearMontagePath in DisappearMontagePathList)
			{
				writer.Put(disappearMontagePath);
			}
			writer.Put(DestroyDelayTime);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			SummonerId = reader.Get<NetworkId>();
			SummonGuid = reader.GetString();
			SummonClassPath = reader.GetString();
			Location = (FVector)SerializationHelpers.DeserializeFVector(reader);
			Rotation = (FRotator)SerializationHelpers.DeserializeFRotator(reader);
			SafeClampToLand = reader.GetBool();
			SummonId = reader.GetInt();
			SummonInstanceId = reader.GetGuid();
			ServantType = (EServantType)reader.GetByte();
			SearchTargetType = (EServantSearchTargetType)reader.GetByte();
			CooperativeSCGuid = reader.GetString();
			AliveTime = reader.GetFloat();
			CatchTargetId = reader.Get<NetworkId>();
			DelayBornTime = reader.GetFloat();
			BornMontagePath = reader.GetString();
			BornSkill = reader.GetInt();
			DelayEffectTime = reader.GetFloat();
			DelaySummonTime = reader.GetFloat();
			IsSummonerAsMaster = reader.GetBool();
			EquipmentState.Deserialize(reader);
			InitSpeed = reader.GetFloat();
			BornEffectPath = reader.GetString();
			int num = reader.GetInt();
			DisappearMontagePathList = new List<string>();
			for (int i = 0; i < num; i++)
			{
				DisappearMontagePathList.Add(reader.GetString());
			}
			DestroyDelayTime = reader.GetFloat();
		}

		public static SummonRequestData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId summonerId = default(NetworkId);
			string summonGuid = null;
			string summonClassPath = null;
			FVector location = default(FVector);
			FRotator rotation = default(FRotator);
			bool safeClampToLand = false;
			int summonId = 0;
			Guid summonInstanceId = default(Guid);
			EServantType servantType = (EServantType)0;
			EServantSearchTargetType searchTargetType = (EServantSearchTargetType)0;
			string cooperativeSCGuid = null;
			float aliveTime = 0f;
			NetworkId catchTargetId = default(NetworkId);
			float delayBornTime = 0f;
			string bornMontagePath = null;
			int bornSkill = 0;
			float delayEffectTime = 0f;
			float delaySummonTime = 0f;
			bool isSummonerAsMaster = false;
			EquipmentState equipmentState = default(EquipmentState);
			float initSpeed = 0f;
			string bornEffectPath = null;
			List<string> disappearMontagePathList = null;
			float destroyDelayTime = 0f;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "summonerId":
					summonerId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "summonGuid":
					summonGuid = reader.GetString();
					break;
				case "summonClassPath":
					summonClassPath = reader.GetString();
					break;
				case "location":
					location = JsonSerializer.Deserialize<FVector>(ref reader, options);
					break;
				case "rotation":
					rotation = JsonSerializer.Deserialize<FRotator>(ref reader, options);
					break;
				case "safeClampToLand":
					safeClampToLand = reader.GetBoolean();
					break;
				case "summonId":
					summonId = reader.GetInt32();
					break;
				case "summonInstanceId":
					summonInstanceId = JsonSerializer.Deserialize<Guid>(ref reader, options);
					break;
				case "servantType":
					servantType = JsonSerializer.Deserialize<EServantType>(ref reader, options);
					break;
				case "searchTargetType":
					searchTargetType = JsonSerializer.Deserialize<EServantSearchTargetType>(ref reader, options);
					break;
				case "cooperativeSCGuid":
					cooperativeSCGuid = reader.GetString();
					break;
				case "aliveTime":
					aliveTime = reader.GetSingle();
					break;
				case "catchTargetId":
					catchTargetId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "delayBornTime":
					delayBornTime = reader.GetSingle();
					break;
				case "bornMontagePath":
					bornMontagePath = reader.GetString();
					break;
				case "bornSkill":
					bornSkill = reader.GetInt32();
					break;
				case "delayEffectTime":
					delayEffectTime = reader.GetSingle();
					break;
				case "delaySummonTime":
					delaySummonTime = reader.GetSingle();
					break;
				case "isSummonerAsMaster":
					isSummonerAsMaster = reader.GetBoolean();
					break;
				case "equipmentState":
					equipmentState = JsonSerializer.Deserialize<EquipmentState>(ref reader, options);
					break;
				case "initSpeed":
					initSpeed = reader.GetSingle();
					break;
				case "bornEffectPath":
					bornEffectPath = reader.GetString();
					break;
				case "disappearMontagePathList":
					disappearMontagePathList = JsonSerializer.Deserialize<List<string>>(ref reader, options);
					break;
				case "destroyDelayTime":
					destroyDelayTime = reader.GetSingle();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new SummonRequestData(summonerId, summonGuid, summonClassPath, location, rotation, safeClampToLand, summonId, summonInstanceId, servantType, searchTargetType, cooperativeSCGuid, aliveTime, catchTargetId, delayBornTime, bornMontagePath, bornSkill, delayEffectTime, delaySummonTime, isSummonerAsMaster, equipmentState, initSpeed, bornEffectPath, disappearMontagePathList, destroyDelayTime);
		}

		public static void TextSerialize(Utf8JsonWriter writer, SummonRequestData obj, JsonSerializerOptions options)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("summonerId");
			NetworkId.TextSerialize(writer, obj.SummonerId, options);
			writer.WriteString("summonGuid", obj.SummonGuid);
			writer.WriteString("summonClassPath", obj.SummonClassPath);
			writer.WritePropertyName("location");
			JsonSerializer.Serialize<FVector>(writer, obj.Location, options);
			writer.WritePropertyName("rotation");
			JsonSerializer.Serialize<FRotator>(writer, obj.Rotation, options);
			writer.WriteBoolean("safeClampToLand", obj.SafeClampToLand);
			writer.WriteNumber("summonId", obj.SummonId);
			writer.WritePropertyName("summonInstanceId");
			JsonSerializer.Serialize(writer, obj.SummonInstanceId, options);
			writer.WritePropertyName("servantType");
			JsonSerializer.Serialize<EServantType>(writer, obj.ServantType, options);
			writer.WritePropertyName("searchTargetType");
			JsonSerializer.Serialize<EServantSearchTargetType>(writer, obj.SearchTargetType, options);
			writer.WriteString("cooperativeSCGuid", obj.CooperativeSCGuid);
			writer.WriteNumber("aliveTime", obj.AliveTime);
			writer.WritePropertyName("catchTargetId");
			NetworkId.TextSerialize(writer, obj.CatchTargetId, options);
			writer.WriteNumber("delayBornTime", obj.DelayBornTime);
			writer.WriteString("bornMontagePath", obj.BornMontagePath);
			writer.WriteNumber("bornSkill", obj.BornSkill);
			writer.WriteNumber("delayEffectTime", obj.DelayEffectTime);
			writer.WriteNumber("delaySummonTime", obj.DelaySummonTime);
			writer.WriteBoolean("isSummonerAsMaster", obj.IsSummonerAsMaster);
			writer.WritePropertyName("equipmentState");
			JsonSerializer.Serialize(writer, obj.EquipmentState, options);
			writer.WriteNumber("initSpeed", obj.InitSpeed);
			writer.WriteString("bornEffectPath", obj.BornEffectPath);
			writer.WritePropertyName("disappearMontagePathList");
			JsonSerializer.Serialize(writer, obj.DisappearMontagePathList, options);
			writer.WriteNumber("destroyDelayTime", obj.DestroyDelayTime);
			writer.WriteEndObject();
		}
	}
	public static class SummonRequestExtensions
	{
		public static FServantReq ToGame(this SummonRequestData value)
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			BGW_PreloadAssetMgr val = BGW_PreloadAssetMgr.Get((UObject)(object)GameUtils.GetWorld());
			BGUCharacterCS pawnByNetworkId = DI.Instance.PawnState.GetPawnByNetworkId(value.SummonerId);
			BGUCharacterCS pawnByNetworkId2 = DI.Instance.PawnState.GetPawnByNetworkId(value.CatchTargetId);
			UClass tamerTemplate = val.TryGetCachedResourceObj<UClass>(value.SummonClassPath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			UAnimMontage bornMontage = null;
			if (!string.IsNullOrEmpty(value.BornMontagePath))
			{
				bornMontage = val.TryGetCachedResourceObj<UAnimMontage>(value.BornMontagePath, (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
			}
			Dictionary<EquipPosition, int> dictionary = new Dictionary<EquipPosition, int>();
			foreach (var (value2, value3) in value.EquipmentState.GetItems())
			{
				dictionary.Add(value2.ToGame(), value3);
			}
			BGWDataAsset_B1DBC val2 = null;
			UNiagaraSystem val3 = null;
			UParticleSystem bornParticle = null;
			if (!string.IsNullOrEmpty(value.BornEffectPath))
			{
				UObject val4 = val.TryGetCachedResourceObj<UObject>(value.BornEffectPath, (ELoadResourceType)0, (EAssetPriority)2, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1));
				if (val4 != (UObject)null)
				{
					val2 = (BGWDataAsset_B1DBC)(object)((val4 is BGWDataAsset_B1DBC) ? val4 : null);
					if ((UObject)(object)val2 == (UObject)null)
					{
						val3 = (UNiagaraSystem)(object)((val4 is UNiagaraSystem) ? val4 : null);
						if ((UObject)(object)val3 == (UObject)null)
						{
							bornParticle = (UParticleSystem)(object)((val4 is UParticleSystem) ? val4 : null);
						}
					}
				}
			}
			return new FServantReq
			{
				Summoner = (AActor)(object)pawnByNetworkId,
				SummonID = value.SummonId,
				SummonInstanceID = GameplayTagExtension.ConvertToCalliopeGuid(value.SummonInstanceId),
				ServantTamerGuid = value.SummonGuid,
				ServantType = value.ServantType,
				SearchTargetType = value.SearchTargetType,
				CooperativeSCGuid = value.CooperativeSCGuid,
				BornTransform = new FTransform(value.Rotation, value.Location),
				AliveTime = value.AliveTime,
				TamerTemplate = tamerTemplate,
				SafeClampToLand = value.SafeClampToLand,
				CatchTarget = (AActor)(object)pawnByNetworkId2,
				BirthBuffIDs = new List<int>(),
				MasterActor = (AActor)(object)(value.IsSummonerAsMaster ? pawnByNetworkId : null),
				DelayBornTime = value.DelayBornTime,
				BornMontage = bornMontage,
				BornSkill = value.BornSkill,
				DelayEffectTime = value.DelayEffectTime,
				DelaySummonTime = value.DelaySummonTime,
				MapEquip = dictionary,
				InitSpeed = value.InitSpeed,
				BornDBC = val2,
				BornNiagara = val3,
				BornParticle = bornParticle,
				DisappearMontagePathList = value.DisappearMontagePathList,
				DestroyDelayTime = value.DestroyDelayTime
			};
		}

		public static SummonRequestData FromGame(this FServantReq value)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			NetworkId? networkIdByActor = DI.Instance.PawnState.GetNetworkIdByActor(value.Summoner);
			NetworkId? networkIdByActor2 = DI.Instance.PawnState.GetNetworkIdByActor(value.CatchTarget);
			string pathName = ((UObject)value.TamerTemplate).PathName;
			UAnimMontage bornMontage = value.BornMontage;
			string bornMontagePath = ((bornMontage != null) ? ((UObject)bornMontage).PathName : null) ?? "";
			EquipmentState equipmentState = new EquipmentState();
			if (value.MapEquip != null)
			{
				equipmentState = new EquipmentState(value.MapEquip.Select((KeyValuePair<EquipPosition, int> kvp) => (kvp.Key.FromGame(), Value: kvp.Value)));
			}
			string bornEffectPath = "";
			if ((UObject)(object)value.BornDBC != (UObject)null)
			{
				bornEffectPath = ((UObject)value.BornDBC).PathName;
			}
			else if ((UObject)(object)value.BornNiagara != (UObject)null)
			{
				bornEffectPath = ((UObject)value.BornNiagara).PathName;
			}
			else if ((UObject)(object)value.BornParticle != (UObject)null)
			{
				bornEffectPath = ((UObject)value.BornParticle).PathName;
			}
			SummonRequestData result = new SummonRequestData
			{
				SummonerId = networkIdByActor.GetValueOrDefault(),
				SummonGuid = value.ServantTamerGuid,
				SummonClassPath = pathName,
				Location = ((FTransform)(ref value.BornTransform)).GetLocation()
			};
			FQuat rotation = ((FTransform)(ref value.BornTransform)).GetRotation();
			result.Rotation = ((FQuat)(ref rotation)).Rotator();
			result.SafeClampToLand = value.SafeClampToLand;
			result.SummonId = value.SummonID;
			result.SummonInstanceId = GameplayTagExtension.ConvertToGuid(value.SummonInstanceID);
			result.ServantType = value.ServantType;
			result.SearchTargetType = value.SearchTargetType;
			result.CooperativeSCGuid = value.CooperativeSCGuid;
			result.AliveTime = value.AliveTime;
			result.CatchTargetId = networkIdByActor2.GetValueOrDefault();
			result.DelayBornTime = value.DelayBornTime;
			result.BornMontagePath = bornMontagePath;
			result.BornSkill = value.BornSkill;
			result.DelayEffectTime = value.DelayEffectTime;
			result.DelaySummonTime = value.DelaySummonTime;
			result.IsSummonerAsMaster = (UObject)(object)value.MasterActor == (UObject)(object)value.Summoner;
			result.EquipmentState = equipmentState;
			result.InitSpeed = value.InitSpeed;
			result.BornEffectPath = bornEffectPath;
			result.DisappearMontagePathList = value.DisappearMontagePathList ?? new List<string>();
			result.DestroyDelayTime = value.DestroyDelayTime;
			return result;
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct TargetData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<TargetData>
		{
			public override TargetData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, TargetData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId Character;

		public NetworkId Target;

		public bool ClearTarget;

		public TargetData(NetworkId character, NetworkId target, bool clearTarget)
		{
			Character = character;
			Target = target;
			ClearTarget = clearTarget;
		}

		public void Serialize(NetDataWriter writer)
		{
			Character.Serialize(writer);
			Target.Serialize(writer);
			writer.Put(ClearTarget);
		}

		public void Deserialize(NetDataReader reader)
		{
			Character.Deserialize(reader);
			Target.Deserialize(reader);
			ClearTarget = reader.GetBool();
		}

		public static TargetData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId character = default(NetworkId);
			NetworkId target = default(NetworkId);
			bool clearTarget = false;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "character":
					character = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "target":
					target = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "clearTarget":
					clearTarget = reader.GetBoolean();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new TargetData(character, target, clearTarget);
		}

		public static void TextSerialize(Utf8JsonWriter writer, TargetData obj, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("character");
			NetworkId.TextSerialize(writer, obj.Character, options);
			writer.WritePropertyName("target");
			NetworkId.TextSerialize(writer, obj.Target, options);
			writer.WriteBoolean("clearTarget", obj.ClearTarget);
			writer.WriteEndObject();
		}
	}
	[DeriveINetSerializable(SerializableMode.Default)]
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct UnitDeadPacket : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<UnitDeadPacket>
		{
			public override UnitDeadPacket Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, UnitDeadPacket value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public NetworkId NetworkId;

		public EDeadReason DeadReason;

		public int DmgId;

		public int StiffLevel;

		public bool IsDotDmg;

		public EAbnormalStateType AbnormalType;

		public UnitDeadPacket(NetworkId netId, EDeadReason deadReason, int dmgId, int stiffLevel, bool isDotDmg, EAbnormalStateType abnormalType)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			NetworkId = netId;
			DeadReason = deadReason;
			DmgId = dmgId;
			StiffLevel = stiffLevel;
			IsDotDmg = isDotDmg;
			AbnormalType = abnormalType;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected I4, but got Unknown
			NetworkId.Serialize(writer);
			writer.Put((int)DeadReason);
			writer.Put(DmgId);
			writer.Put(StiffLevel);
			writer.Put(IsDotDmg);
			writer.Put((int)AbnormalType);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			NetworkId.Deserialize(reader);
			DeadReason = (EDeadReason)(byte)reader.GetInt();
			DmgId = reader.GetInt();
			StiffLevel = reader.GetInt();
			IsDotDmg = reader.GetBool();
			AbnormalType = (EAbnormalStateType)(byte)reader.GetInt();
		}

		public static UnitDeadPacket TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			NetworkId netId = default(NetworkId);
			EDeadReason deadReason = (EDeadReason)0;
			int dmgId = 0;
			int stiffLevel = 0;
			bool isDotDmg = false;
			EAbnormalStateType abnormalType = (EAbnormalStateType)0;
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "networkId":
					netId = NetworkId.TextDeserialize(ref reader, options);
					break;
				case "deadReason":
					deadReason = JsonSerializer.Deserialize<EDeadReason>(ref reader, options);
					break;
				case "dmgId":
					dmgId = reader.GetInt32();
					break;
				case "stiffLevel":
					stiffLevel = reader.GetInt32();
					break;
				case "isDotDmg":
					isDotDmg = reader.GetBoolean();
					break;
				case "abnormalType":
					abnormalType = JsonSerializer.Deserialize<EAbnormalStateType>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new UnitDeadPacket(netId, deadReason, dmgId, stiffLevel, isDotDmg, abnormalType);
		}

		public static void TextSerialize(Utf8JsonWriter writer, UnitDeadPacket obj, JsonSerializerOptions options)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WritePropertyName("networkId");
			NetworkId.TextSerialize(writer, obj.NetworkId, options);
			writer.WritePropertyName("deadReason");
			JsonSerializer.Serialize<EDeadReason>(writer, obj.DeadReason, options);
			writer.WriteNumber("dmgId", obj.DmgId);
			writer.WriteNumber("stiffLevel", obj.StiffLevel);
			writer.WriteBoolean("isDotDmg", obj.IsDotDmg);
			writer.WritePropertyName("abnormalType");
			JsonSerializer.Serialize<EAbnormalStateType>(writer, obj.AbnormalType, options);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct UnitSpawnData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<UnitSpawnData>
		{
			public override UnitSpawnData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, UnitSpawnData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string UnitName;

		public string Guid;

		public FVector Location;

		public UnitSpawnData(string unitName, string guid, FVector location)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			UnitName = unitName;
			Guid = guid;
			Location = location;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			writer.Put(UnitName);
			writer.Put(Guid);
			SerializationHelpers.SerializeFVector(writer, Location);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			UnitName = reader.GetString();
			Guid = reader.GetString();
			Location = (FVector)SerializationHelpers.DeserializeFVector(reader);
		}

		public static UnitSpawnData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string unitName = null;
			string guid = null;
			FVector location = default(FVector);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "unitName":
					unitName = reader.GetString();
					break;
				case "guid":
					guid = reader.GetString();
					break;
				case "location":
					location = JsonSerializer.Deserialize<FVector>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new UnitSpawnData(unitName, guid, location);
		}

		public static void TextSerialize(Utf8JsonWriter writer, UnitSpawnData obj, JsonSerializerOptions options)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteString("unitName", obj.UnitName);
			writer.WriteString("guid", obj.Guid);
			writer.WritePropertyName("location");
			JsonSerializer.Serialize<FVector>(writer, obj.Location, options);
			writer.WriteEndObject();
		}
	}
	[DeriveJsonSerializable(SerializableMode.Default)]
	public struct UnitSpawnRequestData : INetSerializable
	{
		[RegisterJsonConverter]
		public class Converter : JsonConverter<UnitSpawnRequestData>
		{
			public override UnitSpawnRequestData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return TextDeserialize(ref reader, options);
			}

			public override void Write(Utf8JsonWriter writer, UnitSpawnRequestData value, JsonSerializerOptions options)
			{
				TextSerialize(writer, value, options);
			}
		}

		public string UnitName;

		public int Count;

		public int TeamId;

		public FVector Location;

		public UnitSpawnRequestData(string unitName, int count, int teamId, FVector location)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			UnitName = unitName;
			Count = count;
			TeamId = teamId;
			Location = location;
		}

		public void Serialize(NetDataWriter writer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			writer.Put(UnitName);
			writer.Put(Count);
			writer.Put(TeamId);
			SerializationHelpers.SerializeFVector(writer, Location);
		}

		public void Deserialize(NetDataReader reader)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			UnitName = reader.GetString();
			Count = reader.GetInt();
			TeamId = reader.GetInt();
			Location = (FVector)SerializationHelpers.DeserializeFVector(reader);
		}

		public static UnitSpawnRequestData TextDeserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			DebugJson.Assert(reader.TokenType == JsonTokenType.StartObject);
			string unitName = null;
			int count = 0;
			int teamId = 0;
			FVector location = default(FVector);
			while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
			{
				DebugJson.Assert(reader.TokenType == JsonTokenType.PropertyName);
				string text = reader.GetString();
				reader.Read();
				switch (text)
				{
				case "unitName":
					unitName = reader.GetString();
					break;
				case "count":
					count = reader.GetInt32();
					break;
				case "teamId":
					teamId = reader.GetInt32();
					break;
				case "location":
					location = JsonSerializer.Deserialize<FVector>(ref reader, options);
					break;
				default:
					reader.Skip();
					break;
				}
			}
			return new UnitSpawnRequestData(unitName, count, teamId, location);
		}

		public static void TextSerialize(Utf8JsonWriter writer, UnitSpawnRequestData obj, JsonSerializerOptions options)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			writer.WriteString("unitName", obj.UnitName);
			writer.WriteNumber("count", obj.Count);
			writer.WriteNumber("teamId", obj.TeamId);
			writer.WritePropertyName("location");
			JsonSerializer.Serialize<FVector>(writer, obj.Location, options);
			writer.WriteEndObject();
		}
	}
}
namespace WukongMp.Api.Configuration
{
	public static class CharacterKind
	{
		public static readonly string Monkey = TamerUtils.UnifyUnitName("monkey");

		public static readonly string AxeStalwart = TamerUtils.UnifyUnitName("axe_stalwart");

		public static readonly string Bandit = TamerUtils.UnifyUnitName("bandit");

		public static readonly string BladeMonk = TamerUtils.UnifyUnitName("blade_monk");

		public static readonly string BullSergeant = TamerUtils.UnifyUnitName("bull_sergeant");

		public static readonly string BullSoldier = TamerUtils.UnifyUnitName("bull_soldier");

		public static readonly string BullStalwart = TamerUtils.UnifyUnitName("bull_stalwart");

		public static readonly string CrowDiviner = TamerUtils.UnifyUnitName("crow_diviner");

		public static readonly string EagleSoldier = TamerUtils.UnifyUnitName("eagle_soldier");

		public static readonly string EarthRakshasa = TamerUtils.UnifyUnitName("earth_rakshasa");

		public static readonly string RatCaptain = TamerUtils.UnifyUnitName("rat_captain");

		public static readonly string RatSoldier = TamerUtils.UnifyUnitName("rat_soldier");

		public static readonly string SnakePatroller = TamerUtils.UnifyUnitName("snake_patroller");

		public static readonly string TurtleTreasure = TamerUtils.UnifyUnitName("turtle_treasure");

		public static readonly string WolfArcher = TamerUtils.UnifyUnitName("wolf_archer");

		public static readonly string WolfArcherMove = TamerUtils.UnifyUnitName("wolf_archer_move");

		public static readonly string WolfAssassin = TamerUtils.UnifyUnitName("wolf_assassin");

		public static readonly string WolfGuardian = TamerUtils.UnifyUnitName("wolf_guardian");

		public static readonly string WolfScout = TamerUtils.UnifyUnitName("wolf_scout");

		public static readonly string WolfSentinel = TamerUtils.UnifyUnitName("wolf_sentinel");

		public static readonly string WolfSoldier = TamerUtils.UnifyUnitName("wolf_soldier");

		public static readonly string WolfStalwart = TamerUtils.UnifyUnitName("wolf_stalwart");

		public static readonly string WolfSwornsword = TamerUtils.UnifyUnitName("wolf_swornsword");

		public static readonly string YakshaArcher = TamerUtils.UnifyUnitName("yaksha_archer");

		public static readonly string YakshaPatroller = TamerUtils.UnifyUnitName("yaksha_patroller");

		public static readonly string Acolyte = TamerUtils.UnifyUnitName("acolyte");

		public static readonly string ApramanaBat = TamerUtils.UnifyUnitName("apramana_bat");

		public static readonly string BlackBear = TamerUtils.UnifyUnitName("black_bear");

		public static readonly string BlackLoong = TamerUtils.UnifyUnitName("black_loong");

		public static readonly string BlackWind = TamerUtils.UnifyUnitName("black_wind");

		public static readonly string CyanLoong = TamerUtils.UnifyUnitName("cyan_loong");

		public static readonly string Dear = TamerUtils.UnifyUnitName("dear");

		public static readonly string EarthWolf = TamerUtils.UnifyUnitName("earth_wolf");

		public static readonly string Erlang = TamerUtils.UnifyUnitName("erlang");

		public static readonly string ErlangShen = TamerUtils.UnifyUnitName("erlang_shen");

		public static readonly string FatherOfStones = TamerUtils.UnifyUnitName("father_of_stones");

		public static readonly string GoldRhino = TamerUtils.UnifyUnitName("gold_rhino");

		public static readonly string GoreEye = TamerUtils.UnifyUnitName("gore_eye");

		public static readonly string KangLoong = TamerUtils.UnifyUnitName("kang_loong");

		public static readonly string KangStar = TamerUtils.UnifyUnitName("kang_star");

		public static readonly string MadTiger = TamerUtils.UnifyUnitName("mad_tiger");

		public static readonly string Mantis = TamerUtils.UnifyUnitName("mantis");

		public static readonly string NonPure = TamerUtils.UnifyUnitName("non_pure");

		public static readonly string NonVoid = TamerUtils.UnifyUnitName("non_void");

		public static readonly string PoisonChief = TamerUtils.UnifyUnitName("poison_chief");

		public static readonly string RedBoy = TamerUtils.UnifyUnitName("red_boy");

		public static readonly string RedLoong = TamerUtils.UnifyUnitName("red_loong");

		public static readonly string StoneMonkey = TamerUtils.UnifyUnitName("stone_monkey");

		public static readonly string TigerVanguard = TamerUtils.UnifyUnitName("tiger_vanguard");

		public static readonly string WhitecladNoble = TamerUtils.UnifyUnitName("whiteclad_noble");

		public static readonly string YellowLoong = TamerUtils.UnifyUnitName("yellow_loong");

		public static readonly string YellowSquire = TamerUtils.UnifyUnitName("yellow_squire");

		public static readonly string YellowWind = TamerUtils.UnifyUnitName("yellow_wind");

		public static readonly string YinTiger = TamerUtils.UnifyUnitName("yin_tiger");

		public static readonly string DaSheng = TamerUtils.UnifyUnitName("da_sheng");

		public static readonly string DaSheng2 = TamerUtils.UnifyUnitName("da_sheng_2");

		public static readonly string BawLangLang = TamerUtils.UnifyUnitName("baw_lang_lang");

		public static readonly string BlazeBone = TamerUtils.UnifyUnitName("blaze_bone");

		public static readonly string BossB = TamerUtils.UnifyUnitName("boss_b");

		public static readonly string BossC = TamerUtils.UnifyUnitName("boss_c");

		public static readonly string JackalSoldier = TamerUtils.UnifyUnitName("jackal_soldier");

		public static readonly string JiaoLoong = TamerUtils.UnifyUnitName("jiao_loong");

		public static readonly string LotusVision = TamerUtils.UnifyUnitName("lotus_vision");

		public static readonly string MacaqueChief = TamerUtils.UnifyUnitName("macaque_chief");

		public static readonly string Martialist = TamerUtils.UnifyUnitName("martialist");

		public static readonly string Spider = TamerUtils.UnifyUnitName("spider");

		public static readonly string Spider2 = TamerUtils.UnifyUnitName("spider2");
	}
	public static class Constants
	{
		public const int ToleratedLatencyMs = 50;

		public const float FloatComparisonTolerance = 0.1f;

		public const string ConnectedPatches = "Connected";

		public const string GlobalPatches = "Global";

		public const string DisabledPatches = "Disabled";

		public const float MonsterSpawnDistance = 2000f;

		public const float MonsterSpawnTraceHeight = 2000f;

		public const float MonsterHalfHeight = 200f;

		public const float MonsterSpawnSpread = 200f;

		public const float CameraArmLength = 720f;

		public const float TransformedCameraArmLength = 1100f;

		public const int NewCharacterArchiveId = 1;

		public const int MaxPlayers = 10;

		public const int DefaultMonsterTeamId = 2;

		public const int ReconnectDelayMs = 1000;

		public const float RestrictedMovementRadius = 500f;

		public const float RestrictedMovementRadiusSquare = 250000f;

		public const float MonsterUpdateTargetTime = 7f;

		public const float SpawnOwnershipRadius = 7500f;

		public const float BaseMarkerHeightCoefficient = 0.12f;

		public const float MaxMarkerHeightDistance = 10000f;

		public const float ColliderDisableTime = 3f;

		public const string SupremeInspectorFirewallName = "BP_szlc_wanglingguan_mf_hq";

		public static readonly FVector SupremeInspectorFirewallLocation = new FVector(107491.7, 92122.52, 15129.59);

		public static readonly FLinearColor ServerMessageColor = new FLinearColor(0.3f, 0.3f, 0.3f, 1f);

		public static readonly FLinearColor PlayerMessageColor = new FLinearColor(0.9f, 0.9f, 0.9f, 1f);

		public static readonly FLinearColor EnemyPlayerMessageColor = new FLinearColor(1f, 0.3f, 0.3f, 1f);

		public static readonly HashSet<int> InstantTriggerSequences = new HashSet<int> { 30105200, 40104151, 62103371, 62103351, 62103321, 62103301 };

		public static readonly HashSet<int> SoloPlaySequences = new HashSet<int> { 1102021, 1102031, 1103011, 90005015, 90005016, 90005017, 90005018 };

		public const int GourdSkillId = 10530;

		public const int ImmobilizeSkillId = 10518;

		public const int IncenseTrailTalismanSkillId = 10909;

		public const int RuyiScrollSkillId = 10912;

		public const int ConsumableBuffSkillId = 10913;

		public const int IronBodySkillId = 10505;

		public const string ChestCameraLockNode = "CAMERA_LOCK";

		public const string FeetCameraLockNode = "CAMERA_LOCK_Root";

		public const string SpringArmEndSocket = "SpringEndpoint";

		public const string WukongClassPath = "/Game/00Main/Design/Units/Player/Unit_Player_Wukong.Unit_Player_Wukong_C";

		public const string WukongDashengClassPath = "/Game/00Main/Design/Units/Player/Unit_player_dasheng.Unit_player_dasheng_C";

		public const string PlayerMarkerPath = "/Game/Mods/WukongMod/BP_PlayerMarker.BP_PlayerMarker_C";

		public const string DebugCubeActorPath = "/Game/Mods/DebugMod/BP_DebugCube.BP_DebugCube_C";

		public const string DebugSphereActorPath = "/Game/Mods/DebugMod/BP_DebugShpere.BP_DebugShpere_C";

		public const string CoopWorldArchiveName = "world.sav";

		public static readonly EnumSet<EBGUAttrFloat> SyncedAttributes;

		public const string ShimFolder = "CSharpLoader/Shims";

		static Constants()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			EBGUAttrFloat[] array = new EBGUAttrFloat[88];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			SyncedAttributes = new EnumSet<EBGUAttrFloat>(new global::<>z__ReadOnlyArray<EBGUAttrFloat>((EBGUAttrFloat[])(object)array));
		}
	}
	internal static class DisabledCollidersData
	{
		private static readonly List<string> Guids = new List<string>(6) { "1716325759611-719bd9374ae623ee09ed3983024db13e-BP_DynamicObstcle_C_1", "1686949186349-6b25c5f04b1967cd5863e485d95e3cd9-BP_DynamicObstcle_C_3", "UGuid.HFM.HuStone.DOin01", "1700513547161-6b25c5f04b1967cd5863e485d95e3cd9-BP_DynamicObstcle_C_1", "1654114656537-719bd9374ae623ee09ed3983024db13e-BP_DynamicObstcle_C_5", "UGuid.HFS.DBL.DOin" };

		public static bool IsDisabled(string guid)
		{
			return Guids.Contains(guid);
		}
	}
	internal static class EcsExcludedMonsters
	{
		public static List<string> MonsterNames = new List<string>(11)
		{
			"szlc_rabbit", "SZLC_Bullfrog", "SZLC_Mouse", "szlc_deer", "szlc_turtle", "szlc_crane", "SZLC_WildBoar", "UGuid.LYS.LittleFox", "UGuid.PSD.EmptyUnit", "monkeysummon_pr",
			"monkeysummon_fs"
		};
	}
	public class GameplayConfiguration(ILogger logger)
	{
		private Func<bool>? disableTamerAttackQuery;

		private Func<int, bool>? isSkillEnabledQuery;

		private Func<bool>? isPlayerInBattleQuery;

		private Func<EInteractType, bool>? isInteractionAllowedQuery;

		private Func<string, bool>? isTamerNotSynchronizedQuery;

		private Func<string, bool>? isAreaOverlapDisabledQuery;

		public bool IsSupportMultiLockEnabled { get; set; }

		public bool IsStrongDamageImmueEnabled { get; set; }

		public bool EnableCustomCameraArmLength { get; set; }

		public bool EnableSpawnedTamers { get; set; }

		public bool DisableCutscenes { get; set; }

		[Obsolete("To be replaced by data sync direction after refactoring")]
		public bool SyncTamerTeamFromGameToEcs { get; set; }

		[Obsolete("To be replaced by data sync direction after refactoring")]
		public bool OverrideLocalPlayerTeamFromGlobalEntity { get; set; }

		public bool EnableCustomIsPlayerInBattle { get; set; }

		public void SetDisableTamerAttackQuery(Func<bool> query)
		{
			if (disableTamerAttackQuery != null)
			{
				logger.LogError("DisableTamerAttackQuery is already set. Overriding the existing query.");
			}
			disableTamerAttackQuery = query;
		}

		public void ClearDisableTamerAttackQuery()
		{
			disableTamerAttackQuery = null;
		}

		public bool ShouldDisableTamerAttack()
		{
			return disableTamerAttackQuery?.Invoke() ?? false;
		}

		public void SetIsSkillEnabledQuery(Func<int, bool> query)
		{
			if (isSkillEnabledQuery != null)
			{
				logger.LogError("IsSkillEnabledQuery is already set. Overriding the existing query.");
			}
			isSkillEnabledQuery = query;
		}

		public void ClearIsSkillEnabledQuery()
		{
			isSkillEnabledQuery = null;
		}

		public bool IsSkillEnabled(int skillId)
		{
			return isSkillEnabledQuery?.Invoke(skillId) ?? true;
		}

		public void SetIsPlayerInBattleQuery(Func<bool> query)
		{
			if (isPlayerInBattleQuery != null)
			{
				logger.LogError("IsPlayerInBattleQuery is already set. Overriding the existing query.");
			}
			isPlayerInBattleQuery = query;
		}

		public void ClearIsPlayerInBattleQuery()
		{
			isPlayerInBattleQuery = null;
		}

		public bool IsPlayerInBattle()
		{
			return isPlayerInBattleQuery?.Invoke() ?? false;
		}

		public void SetIsInteractionAllowedQuery(Func<EInteractType, bool> query)
		{
			if (isInteractionAllowedQuery != null)
			{
				logger.LogError("IsInteractionAllowedQuery is already set. Overriding the existing query.");
			}
			isInteractionAllowedQuery = query;
		}

		public void ClearIsInteractionAllowedQuery()
		{
			isPlayerInBattleQuery = null;
		}

		public bool IsInteractionAllowed(EInteractType interactType)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return isInteractionAllowedQuery?.Invoke(interactType) ?? true;
		}

		public void SetIsTamerNotSynchronizedQuery(Func<string, bool> query)
		{
			if (isTamerNotSynchronizedQuery != null)
			{
				logger.LogError("IsTamerNotSynchronizedQuery is already set. Overriding the existing query.");
			}
			isTamerNotSynchronizedQuery = query;
		}

		public void ClearIsTamerNotSynchronizedQuery()
		{
			isTamerNotSynchronizedQuery = null;
		}

		public bool IsTamerNotSynchronized(string guid)
		{
			return isTamerNotSynchronizedQuery?.Invoke(guid) ?? true;
		}

		public void SetIsAreaOverlapDisabledQuery(Func<string, bool> query)
		{
			if (isAreaOverlapDisabledQuery != null)
			{
				logger.LogError("IsAreaOverlapDisabledQuery is already set. Overriding the existing query.");
			}
			isAreaOverlapDisabledQuery = query;
		}

		public void ClearIsAreaOverlapDisabledQuery()
		{
			isAreaOverlapDisabledQuery = null;
		}

		public bool IsAreaOverlapDisabled(string guid)
		{
			return isAreaOverlapDisabledQuery?.Invoke(guid) ?? false;
		}
	}
	public static class UnitPathsConfig
	{
		private static readonly Dictionary<string, string> Configurations = new Dictionary<string, string>
		{
			{
				CharacterKind.Monkey,
				"/Game/00Main/Design/Units/Player/TAMER_monkeysummon_fs.TAMER_monkeysummon_fs_C"
			},
			{
				CharacterKind.AxeStalwart,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_niu_02.TAMER_hys_niu_02_C"
			},
			{
				CharacterKind.Bandit,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_tufei_01.TAMER_gycy_tufei_01_C"
			},
			{
				CharacterKind.BladeMonk,
				"/Game/00Main/Design/Units/LYS/TAMER_LYS_JieDaoSeng.TAMER_LYS_JieDaoSeng_C"
			},
			{
				CharacterKind.BullSergeant,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_techushi_01.TAMER_hys_techushi_01_C"
			},
			{
				CharacterKind.BullSoldier,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_techushi_02.TAMER_hys_techushi_02_C"
			},
			{
				CharacterKind.BullStalwart,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_techushi_03.TAMER_hys_techushi_03_C"
			},
			{
				CharacterKind.CrowDiviner,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_yaxiangke.TAMER_gycy_yaxiangke_C"
			},
			{
				CharacterKind.EagleSoldier,
				"/Game/00Main/Design/Units/MGD/Tamer_mgd_tianbing_02.TAMER_mgd_tianbing_02_C"
			},
			{
				CharacterKind.EarthRakshasa,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_huijingrenou_03.TAMER_hys_huijingrenou_03_C"
			},
			{
				CharacterKind.RatCaptain,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_shuangtoushu_01a.TAMER_hfm_shuangtoushu_01a_C"
			},
			{
				CharacterKind.RatSoldier,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_shu_05a.TAMER_hfm_shu_05a_C"
			},
			{
				CharacterKind.SnakePatroller,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_she_03.TAMER_gycy_she_03_C"
			},
			{
				CharacterKind.TurtleTreasure,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_niaozui.TAMER_hys_niaozui_C"
			},
			{
				CharacterKind.WolfArcher,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_08_NotMove.TAMER_gycy_lang_08_NotMove_C"
			},
			{
				CharacterKind.WolfArcherMove,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_08.TAMER_gycy_lang_08_C"
			},
			{
				CharacterKind.WolfAssassin,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_02.TAMER_gycy_lang_02_C"
			},
			{
				CharacterKind.WolfGuardian,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_huangpaolang.TAMER_gycy_huangpaolang_C"
			},
			{
				CharacterKind.WolfScout,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_03.TAMER_gycy_lang_03_C"
			},
			{
				CharacterKind.WolfSentinel,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_01.TAMER_gycy_lang_01_C"
			},
			{
				CharacterKind.WolfSoldier,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_07a.TAMER_gycy_lang_07a_C"
			},
			{
				CharacterKind.WolfStalwart,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_06.TAMER_gycy_lang_06_C"
			},
			{
				CharacterKind.WolfSwornsword,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_lang_05.TAMER_gycy_lang_05_C"
			},
			{
				CharacterKind.YakshaArcher,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_guishiwei_01a.TAMER_gycy_guishiwei_01a_C"
			},
			{
				CharacterKind.YakshaPatroller,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_xunshangui_01a.TAMER_hfm_xunshangui_01a_C"
			},
			{
				CharacterKind.Acolyte,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_shawuliang_01a.TAMER_hfm_shawuliang_01a_C"
			},
			{
				CharacterKind.ApramanaBat,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_mo3.TAMER_lys_mo3_C"
			},
			{
				CharacterKind.BlackBear,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_xiong_02.TAMER_gycy_xiong_02_C"
			},
			{
				CharacterKind.BlackLoong,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_chuilong_01a.TAMER_lys_chuilong_01a_C"
			},
			{
				CharacterKind.BlackWind,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_hfdw.TAMER_gycy_hfdw_C"
			},
			{
				CharacterKind.CyanLoong,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_wudulong_03a.TAMER_lys_wudulong_03a_C"
			},
			{
				CharacterKind.Dear,
				"/Game/00Main/Design/Units/Online/SZLC/TAMER_szlc_yingzuilu_01.TAMER_szlc_yingzuilu_01_C"
			},
			{
				CharacterKind.EarthWolf,
				"/Game/00Main/Design/Units/HFM/TAMER_HFM_Suoyang_01a.TAMER_HFM_Suoyang_01a_C"
			},
			{
				CharacterKind.Erlang,
				"/Game/00Main/Design/Units/MGD/TAMER_mgd_yangjian_01.TAMER_mgd_yangjian_01_C"
			},
			{
				CharacterKind.ErlangShen,
				"/Game/00Main/Design/Units/MGD/TAMER_mgd_erlangshen_01.TAMER_mgd_erlangshen_01_C"
			},
			{
				CharacterKind.FatherOfStones,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_hms.TAMER_hys_hms_C"
			},
			{
				CharacterKind.GoldRhino,
				"/Game/00Main/Design/Units/Online/SZLC/TAMER_szlc_xiniu_01.TAMER_szlc_xiniu_01_C"
			},
			{
				CharacterKind.GoreEye,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_hou_01a.TAMER_hfm_hou_01a_C"
			},
			{
				CharacterKind.KangLoong,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_kjldragon.TAMER_lys_kjldragon_C"
			},
			{
				CharacterKind.KangStar,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_kjlwoman.TAMER_lys_kjlwoman_C"
			},
			{
				CharacterKind.MadTiger,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_bashanhu_01.TAMER_hfm_bashanhu_01_C"
			},
			{
				CharacterKind.Mantis,
				"/Game/00Main/Design/Units/Online/SZLC/TAMER_szlc_tanglang01.TAMER_szlc_tanglang01_C"
			},
			{
				CharacterKind.NonPure,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_seng_04.TAMER_gycy_seng_04_C"
			},
			{
				CharacterKind.NonVoid,
				"/Game/00Main/Design/Units/LYS/TAMER_LYS_LaoSeng_01.TAMER_LYS_LaoSeng_01_C"
			},
			{
				CharacterKind.PoisonChief,
				"/Game/00Main/Design/Units/Online/SL/TAMER_sl_shitongling.TAMER_sl_shitongling_C"
			},
			{
				CharacterKind.RedBoy,
				"/Game/00Main/Design/Units/HYS/TAMER_hys_honghaier_01a.TAMER_hys_honghaier_01a_C"
			},
			{
				CharacterKind.RedLoong,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_wudulong_02a.TAMER_lys_wudulong_02a_C"
			},
			{
				CharacterKind.StoneMonkey,
				"/Game/00Main/Design/Units/MGD/TAMER_mgd_yuan.TAMER_mgd_yuan_C"
			},
			{
				CharacterKind.TigerVanguard,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_hu_01.TAMER_hfm_hu_01_C"
			},
			{
				CharacterKind.WhitecladNoble,
				"/Game/00Main/Design/Units/GYCY/TAMER_gycy_baiyi_03.TAMER_gycy_baiyi_03_C"
			},
			{
				CharacterKind.YellowLoong,
				"/Game/00Main/Design/Units/LYS/TAMER_lys_dage.TAMER_lys_dage_C"
			},
			{
				CharacterKind.YellowSquire,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_huangpaozhu.TAMER_hfm_huangpaozhu_C"
			},
			{
				CharacterKind.YellowWind,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_hfds_01a.TAMER_hfm_hfds_01a_C"
			},
			{
				CharacterKind.YinTiger,
				"/Game/00Main/Design/Units/HFM/TAMER_hfm_hu_wind_01.TAMER_hfm_hu_wind_01_C"
			},
			{
				CharacterKind.DaSheng,
				"/Game/00Main/Design/Units/MGD/TAMER_mgd_jsds.TAMER_mgd_jsds_C"
			},
			{
				CharacterKind.DaSheng2,
				"/Game/00Main/Design/Units/MGD/TAMER_mgd_jsds_p2.TAMER_mgd_jsds_p2_C"
			}
		};

		public static string GetUnitPath(string unitName)
		{
			return Configurations[TamerUtils.UnifyUnitName(unitName)];
		}

		public static bool IsValidUnitName(string unitName)
		{
			return Configurations.ContainsKey(TamerUtils.UnifyUnitName(unitName));
		}

		public static IEnumerable<string> GetAllValidUnitNames()
		{
			return Configurations.Keys;
		}
	}
	internal static class UnsynchronizedBuffsData
	{
		public static readonly List<int> Ids = new List<int>(1) { 931 };
	}
}
namespace WukongMp.Api.Compat
{
	public static class CompatExtensions
	{
		private static readonly ThreadLocal<char[]> _splitBuffer = new ThreadLocal<char[]>(() => new char[1]);

		public static bool TryDequeue<T>(this Queue<T> queue, out T item)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			if (queue.Count > 0)
			{
				item = queue.Dequeue();
				return true;
			}
			item = default(T);
			return false;
		}

		public static string[] Split(this string str, char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			char[] value = _splitBuffer.Value;
			value[0] = separator;
			return str.Split(value, options);
		}

		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dict, K key)
		{
			if (dict == null)
			{
				throw new ArgumentNullException("dict");
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!dict.TryGetValue(key, out V value))
			{
				return default(V);
			}
			return value;
		}

		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dict, K key, V defaultValue)
		{
			if (dict == null)
			{
				throw new ArgumentNullException("dict");
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!dict.TryGetValue(key, out V value))
			{
				return defaultValue;
			}
			return value;
		}
	}
	public static class DebugHelper
	{
		[Conditional("DEBUG")]
		public static void AssertNotNull<T>([NotNull] T? value)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertNotNull<T>([NotNull] T? value, string message)
		{
		}
	}
}
namespace WukongMp.Api.Command
{
	public class ConsoleCommand(Action<ReadOnlyMemory<string>> handler)
	{
		public Action<ReadOnlyMemory<string>> Handler { get; } = handler;
	}
	public class WukongCommandConsole : IDisposable
	{
		private readonly WukongConnectionManager _connection;

		private readonly WukongPlayerState _playerState;

		private readonly WukongRpcCallbacks _rpc;

		private readonly WukongServerRpcCallbacks _serverRpc;

		private readonly WukongChatter _wukongChatter;

		private readonly WukongWidgetManager _widgetManager;

		private readonly WukongEventBus _eventBus;

		private readonly WukongAreaState _areaState;

		private readonly IClientEcsUpdateLoop _ecsLoop;

		private readonly Dictionary<string, ConsoleCommand> _commands = new Dictionary<string, ConsoleCommand>();

		private readonly Dictionary<string, IEnumerable<string>> _commandsParams = new Dictionary<string, IEnumerable<string>>();

		private const char Separator = ' ';

		private string NickName => _playerState.LocalPlayerEntity?.GetState().NickName ?? "";

		public WukongCommandConsole(WukongConnectionManager connection, WukongPlayerState playerState, WukongRpcCallbacks rpc, WukongServerRpcCallbacks serverRpc, WukongChatter wukongChatter, WukongWidgetManager widgetManager, WukongEventBus eventBus, WukongAreaState areaState, IClientEcsUpdateLoop ecsLoop)
		{
			Logging.LogDebug("Initializing WukongCommandConsole");
			_connection = connection;
			_playerState = playerState;
			_rpc = rpc;
			_serverRpc = serverRpc;
			_wukongChatter = wukongChatter;
			_widgetManager = widgetManager;
			_eventBus = eventBus;
			_areaState = areaState;
			_ecsLoop = ecsLoop;
			_eventBus.OnLoadingScreenClose += OnLoadingScreenClose;
			SetupCommands();
		}

		public void Dispose()
		{
			_eventBus.OnLoadingScreenClose -= OnLoadingScreenClose;
			Logging.LogDebug("Disposing WukongCommandConsole");
		}

		public void ProcessCommand(string command)
		{
			if (!string.IsNullOrWhiteSpace(command))
			{
				command = command.Trim();
				TryHandleCommand(command);
			}
		}

		public void AddCommand(string command, ConsoleCommand handler, IEnumerable<string>? availableFirstParams = null)
		{
			if (!_commands.ContainsKey(command))
			{
				_commands.Add(command, handler);
				if (availableFirstParams != null)
				{
					_commandsParams[command] = availableFirstParams;
				}
				_widgetManager.UpdateConsoleCommands(GetAvailableCommands(), _commandsParams);
			}
		}

		public void AddMessageToConsole(string message)
		{
			_widgetManager.AddMessageToConsole(message);
		}

		public void AddLocalizedMessageToConsole(string message, params string[] placeholders)
		{
			string message2 = string.Format(Texts.ResourceManager.GetString(message, Texts.Culture), ((IEnumerable<object>)placeholders).ToArray());
			_widgetManager.AddMessageToConsole(message2);
		}

		private void SetupCommands()
		{
			AddCommand("/reconnect", new ConsoleCommand(RequestReconnect));
			AddCommand("/giveup", new ConsoleCommand(RequestGiveUp));
			AddCommand("/rebirth", new ConsoleCommand(RequestRebirth));
			AddCommand("/rebirth_shrine", new ConsoleCommand(RequestPointRebirth));
		}

		private void RequestRebirth(ReadOnlyMemory<string> _)
		{
			PlayerId? playerId = _connection.PlayerId;
			if (playerId.HasValue)
			{
				_rpc.SendRebirthPlayer(playerId.Value);
				_wukongChatter.SendServerMessage("PlayerRequestedRebirth", NickName);
			}
		}

		private void RequestPointRebirth(ReadOnlyMemory<string> _)
		{
			MainCharacterEntity? localMainCharacter = _playerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
				PlayerId playerId = valueOrDefault.GetState().PlayerId;
				PlayerUtils.TeleportLocalPlayerToCurrentRebirthPoint(valueOrDefault);
				_rpc.SendRebirthPlayer(playerId);
				_wukongChatter.SendServerMessage("PlayerRequestedRebirth", NickName);
			}
		}

		private void ToggleCheats(ReadOnlyMemory<string> _)
		{
			MainCharacterEntity? localMainCharacter = _playerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				localMainCharacter.GetValueOrDefault();
				if (_areaState.IsMasterClient && _areaState.CurrentArea.HasValue)
				{
					RoomComponent room = _areaState.CurrentArea.Value.Room;
					_wukongChatter.SendServerMessage(room.CheatsAllowed ? "CheatsDisabled" : "CheatsEnabled");
					_serverRpc.SendEnableCheats(_areaState.CurrentArea.Value.Scope.AreaId, !room.CheatsAllowed);
				}
			}
		}

		private void ResolveSoftlock(ReadOnlyMemory<string> _)
		{
			MainCharacterEntity? localMainCharacter = _playerState.LocalMainCharacter;
			if (localMainCharacter.HasValue)
			{
				MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
				PlayerUtils.RespawnSoftlockedParty(valueOrDefault);
			}
		}

		private void RequestGiveUp(ReadOnlyMemory<string> _)
		{
			_wukongChatter.SendServerMessage("PlayerGaveUp", NickName);
			_ecsLoop.Scheduler.Schedule(delegate(CommandBufferSynced commandBufferSynced, WukongCommandConsole self)
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				MainCharacterEntity? localMainCharacter = self._playerState.LocalMainCharacter;
				if (localMainCharacter.HasValue)
				{
					MainCharacterEntity valueOrDefault = localMainCharacter.GetValueOrDefault();
					DebugUtils.InvincibilityEnabled = false;
					ref LocalMainCharacterComponent localState = ref valueOrDefault.GetLocalState();
					BUS_GSEventCollection obj = BUS_EventCollectionCS.Get((AActor)(object)localState.Pawn);
					if (obj != null)
					{
						obj.Evt_IncreaseAttrFloat.Invoke((EBGUAttrFloat)151, -2000f);
					}
					if (obj != null)
					{
						obj.Evt_UnitDead.Invoke((AActor)(object)localState.Pawn, (EDeadReason)8, -1, -1, (UAnimMontage)null, default(FEffectInstReq), false, (EAbnormalStateType)0);
					}
				}
			}, this);
		}

		private void RequestReconnect(ReadOnlyMemory<string> _)
		{
			_connection.Reconnect();
		}

		private void RequestDisconnect(ReadOnlyMemory<string> _)
		{
			if (_connection.AreaState.InRoom)
			{
				_wukongChatter.SendServerMessage("PlayerLeft", NickName);
				_connection.Disconnect();
			}
		}

		private void ExecuteConsoleCommand(ReadOnlyMemory<string> args)
		{
			string text = string.Join(" ", args.ToArray());
			Logging.LogDebug("Executing command: {Command}", text);
			USystemLibrary.ExecuteConsoleCommand((UObject)(object)GameUtils.GetWorld(), text, (APlayerController)null);
		}

		private void ToggleDynamicObstacles(ReadOnlyMemory<string> _)
		{
			try
			{
				UWorld world = GameUtils.GetWorld();
				if ((UObject)(object)world != (UObject)null)
				{
					DebugUtils.ToggleBoxTemp(BGW_PreloadAssetMgr.Get((UObject)(object)world).TryGetCachedResourceObj<UClass>("Blueprint'/Game/00Main/BPLibrary/SceneObj/BP_DynamicObstcle.BP_DynamicObstcle_C'", (ELoadResourceType)1, (EAssetPriority)0, (Action<int, UObject>)null, -1, -1, (EAssetImportance)(-1)), (UObject)(object)world);
				}
			}
			catch (Exception ex)
			{
				USharpExceptionHandler.HandleException(ex, (EUSharpExceptionType)1);
			}
		}

		private bool TryHandleCommand(string message)
		{
			string[] array = message.Split(new char[1] { ' ' });
			if (array.Length != 0)
			{
				if (_commands.ContainsKey(array[0]))
				{
					if (CanExecuteCommand())
					{
						ConsoleCommand consoleCommand = _commands[array[0]];
						string[] array2 = array.Skip(1).ToArray();
						consoleCommand.Handler(array2);
					}
					return true;
				}
				AddLocalizedMessageToConsole("InvalidCommand", array[0]);
			}
			return false;
		}

		private bool CanExecuteCommand()
		{
			if (_playerState.LocalMainCharacter.HasValue)
			{
				return !_playerState.LocalMainCharacter.Value.GetLocalState().IsInSequence;
			}
			return false;
		}

		private List<string> GetAvailableCommands()
		{
			return _commands.Keys.ToList();
		}

		private void OnLoadingScreenClose()
		{
			if (_eventBus.IsGameplayLevel && _areaState.CurrentArea.HasValue && _areaState.CurrentArea.Value.Room.CheatsAllowed)
			{
				_wukongChatter.AddLocalServerMessage("CheatsEnabled");
			}
		}
	}
}
namespace WukongMp.Api.Chat
{
	public class WukongChatter : IDisposable
	{
		private readonly WukongPlayerState _playerState;

		private readonly WukongRpcCallbacks _rpc;

		private readonly WukongWidgetManager _widgetManager;

		private string NickName => _playerState.LocalPlayerEntity?.GetState().NickName ?? "";

		public WukongChatter(WukongPlayerState playerState, WukongRpcCallbacks rpc, WukongWidgetManager widgetManager)
		{
			Logging.LogDebug("Initializing WukongChatter");
			_playerState = playerState;
			_rpc = rpc;
			_widgetManager = widgetManager;
			_rpc.OnGetChatMessage += OnGetMessage;
		}

		public void Dispose()
		{
			Logging.LogDebug("Disposing WukongChatter");
			_rpc.OnGetChatMessage -= OnGetMessage;
		}

		public void ProcessMessage(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}
			message = message.Trim();
			if (_playerState.LocalPlayerId.HasValue)
			{
				if (message.StartsWith("/"))
				{
					AddLocalServerMessage("HintCommandsUse");
				}
				SendChatMessage(_playerState.LocalPlayerId.Value, NickName, message);
			}
			else
			{
				Logging.LogError("Cannot send chat message because local player ID is not set");
			}
		}

		private void SendChatMessage(PlayerId playerId, string nickname, string message)
		{
			Logging.LogDebug("Sending message {Message}", message);
			_rpc.SendChatMessage(ChatMessage.CreateClientMessage(playerId, nickname, message));
		}

		public void SendServerMessage(string message, params string[] args)
		{
			Logging.LogDebug("Sending server message {Message}", message);
			_rpc.SendChatMessage(ChatMessage.CreateServerMessage(message, args));
		}

		private void OnGetMessage(ChatMessage message)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			bool flag = message.PlayerId == PlayerId.Server;
			FLinearColor color = (flag ? WukongMp.Api.Configuration.Constants.ServerMessageColor : WukongMp.Api.Configuration.Constants.PlayerMessageColor);
			MainCharacterEntity? mainCharacterById = _playerState.GetMainCharacterById(message.PlayerId);
			if (mainCharacterById.HasValue && _playerState.LocalMainCharacter.HasValue)
			{
				BGUCharacterCS pawn = mainCharacterById.Value.GetLocalState().Pawn;
				if (BGUFunctionLibraryCS.BGUIsEnemyTeam((AActor)(object)_playerState.LocalMainCharacter.Value.GetLocalState().Pawn, (AActor)(object)pawn))
				{
					color = WukongMp.Api.Configuration.Constants.EnemyPlayerMessageColor;
				}
			}
			string text = (flag ? "Server" : message.Nickname);
			string message2 = message.Message;
			if (flag)
			{
				message2 = string.Format(Texts.ResourceManager.GetString(message.Message, Texts.Culture), ((IEnumerable<object>)message.Placeholders).ToArray());
			}
			Logging.LogDebug("Message \"{Message}\" received from \"{Sender}\"", message.Message, text);
			_widgetManager.AddChatMessage(flag, text, message2, color);
		}

		public void AddLocalServerMessage(string message, params string[] placeholders)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			string message2 = string.Format(Texts.ResourceManager.GetString(message, Texts.Culture), ((IEnumerable<object>)placeholders).ToArray());
			_widgetManager.AddChatMessage(isSystemMessage: true, "Server", message2, WukongMp.Api.Configuration.Constants.ServerMessageColor);
		}
	}
}
[CompilerGenerated]
internal sealed class <>z__ReadOnlyArray<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	int ICollection.Count => _items.Length;

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

	int IReadOnlyCollection<T>.Count => _items.Length;

	T IReadOnlyList<T>.this[int index] => _items[index];

	int ICollection<T>.Count => _items.Length;

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

	public <>z__ReadOnlyArray(T[] items)
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
		return ((ICollection<T>)_items).Contains(item);
	}

	void ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		((ICollection<T>)_items).CopyTo(array, arrayIndex);
	}

	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	int IList<T>.IndexOf(T item)
	{
		return ((IList<T>)_items).IndexOf(item);
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
[CompilerGenerated]
internal sealed class <>z__ReadOnlySingleElementList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	private sealed class Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		object IEnumerator.Current => _item;

		T IEnumerator<T>.Current => _item;

		public Enumerator(T item)
		{
			_item = item;
		}

		bool IEnumerator.MoveNext()
		{
			if (!_moveNextCalled)
			{
				return _moveNextCalled = true;
			}
			return false;
		}

		void IEnumerator.Reset()
		{
			_moveNextCalled = false;
		}

		void IDisposable.Dispose()
		{
		}
	}

	int ICollection.Count => 1;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	object IList.this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return _item;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	bool IList.IsFixedSize => true;

	bool IList.IsReadOnly => true;

	int IReadOnlyCollection<T>.Count => 1;

	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return _item;
		}
	}

	int ICollection<T>.Count => 1;

	bool ICollection<T>.IsReadOnly => true;

	T IList<T>.this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return _item;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public <>z__ReadOnlySingleElementList(T item)
	{
		_item = item;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(_item);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		array.SetValue(_item, index);
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
		return EqualityComparer<T>.Default.Equals(_item, (T)value);
	}

	int IList.IndexOf(object value)
	{
		if (!EqualityComparer<T>.Default.Equals(_item, (T)value))
		{
			return -1;
		}
		return 0;
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
		return new Enumerator(_item);
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
		return EqualityComparer<T>.Default.Equals(_item, item);
	}

	void ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		array[arrayIndex] = _item;
	}

	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	int IList<T>.IndexOf(T item)
	{
		if (!EqualityComparer<T>.Default.Equals(_item, item))
		{
			return -1;
		}
		return 0;
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
