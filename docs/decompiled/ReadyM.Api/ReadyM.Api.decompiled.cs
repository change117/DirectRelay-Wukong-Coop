using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using LiteNetLib.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Api.Generators;
using ReadyM.Api.Idents;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: InternalsVisibleTo("ReadyM.Api.Multiplayer")]
[assembly: InternalsVisibleTo("ReadyM.Relay.Server")]
[assembly: InternalsVisibleTo("ReadyM.Relay.Tests")]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: AssemblyCompany("ReadyM.Api")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.422.1357")]
[assembly: AssemblyInformationalVersion("1.0.422.1357+63464565c8693c0537c1bd91e59ef593e8f0bde2")]
[assembly: AssemblyProduct("ReadyM.Api")]
[assembly: AssemblyTitle("ReadyM.Api")]
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
namespace ReadyM.Api
{
	public abstract class PatcherBase : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public bool IsPatched { get; private set; }

		public void Patch()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("Mod is already disposed.");
			}
			if (IsPatched)
			{
				throw new InvalidOperationException("Mod is already patched.");
			}
			IsPatched = true;
			OnPatch();
			OnCommit();
		}

		protected virtual void OnPatch()
		{
		}

		protected abstract void OnCommit();

		public void Unpatch()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("Mod is already disposed.");
			}
			if (!IsPatched)
			{
				throw new InvalidOperationException("Mod is not patched.");
			}
			IsPatched = false;
			OnUnpatch();
			OnCommit();
		}

		protected virtual void OnUnpatch()
		{
		}

		public void Dispose()
		{
			if (IsPatched)
			{
				Unpatch();
			}
			IsDisposed = true;
		}
	}
}
namespace ReadyM.Api.Serialization
{
	public static class DebugJson
	{
		public static void Assert([DoesNotReturnIf(false)] bool condition, string? message = null)
		{
			if (!condition)
			{
				throw new JsonException(message);
			}
		}
	}
	[AttributeUsage(AttributeTargets.Struct)]
	public sealed class DeriveJsonSerializableAttribute(SerializableMode mode = SerializableMode.Default) : Attribute()
	{
		public readonly SerializableMode Mode = mode;
	}
	public interface IDeltaEquatable<in T>
	{
		bool DeltaEquals(T other, float delta);
	}
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class RegisterJsonConverterAttribute : Attribute
	{
	}
	[Flags]
	public enum SerializableMode : byte
	{
		MapFields = 1,
		MapProperties = 2,
		MapFieldsAndProperties = 3,
		MapPrivate = 4,
		MapPublic = 8,
		MapInternal = 0x10,
		Default = 0xD
	}
}
namespace ReadyM.Api.Idents
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ArchetypeId : IEquatable<ArchetypeId>, INetSerializable
	{
		private byte _id;

		public static ArchetypeId None => new ArchetypeId(0);

		public ArchetypeId(byte id)
		{
			_id = id;
		}

		public bool Equals(ArchetypeId other)
		{
			return _id == other._id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is ArchetypeId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		public override string ToString()
		{
			return $"ArchetypeId[{_id}]";
		}

		public static bool operator ==(ArchetypeId left, ArchetypeId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ArchetypeId left, ArchetypeId right)
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
}
namespace ReadyM.Api.Helpers
{
	public abstract class PendingActionScheduler<TContext> : PendingActionSchedulerBase
	{
		protected class PooledCompletionSource<T> : IValueTaskSource<T>
		{
			private ManualResetValueTaskSourceCore<T> _core;

			public ValueTask<T> Task => new ValueTask<T>(this, _core.Version);

			ValueTaskSourceStatus IValueTaskSource<T>.GetStatus(short token)
			{
				return _core.GetStatus(token);
			}

			void IValueTaskSource<T>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
			{
				_core.OnCompleted(continuation, state, token, flags);
			}

			T IValueTaskSource<T>.GetResult(short token)
			{
				return _core.GetResult(token);
			}

			public void SetResult(T result)
			{
				_core.SetResult(result);
			}

			public void SetException(Exception ex)
			{
				_core.SetException(ex);
			}

			public void Reset()
			{
				_core.Reset();
			}
		}

		protected abstract class PendingGroupBase
		{
			protected static int TypeIndexCounter;

			public abstract void Update();
		}

		protected class PendingActionGroup(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext>, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext>, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext>, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext>, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext> action)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, result));
				return result;
			}

			public void Add(Action<TContext> action)
			{
				_items.Add((action, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext>, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext>, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingActionGroup<T>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext, T>, T, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext, T>, T, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext, T>, T, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext, T>, T, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext, T> action, T arg)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, arg, result));
				return result;
			}

			public void Add(Action<TContext, T> action, T arg)
			{
				_items.Add((action, arg, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, arg, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context, arg);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext, T>, T, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext, T>, T, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingActionGroup<T0, T1>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext, T0, T1> action, T0 arg0, T1 arg1)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, arg0, arg1, result));
				return result;
			}

			public void Add(Action<TContext, T0, T1> action, T0 arg0, T1 arg1)
			{
				_items.Add((action, arg0, arg1, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, arg, arg2, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context, arg, arg2);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext, T0, T1>, T0, T1, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingActionGroup<T0, T1, T2>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext, T0, T1, T2> action, T0 arg0, T1 arg1, T2 arg2)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, arg0, arg1, arg2, result));
				return result;
			}

			public void Add(Action<TContext, T0, T1, T2> action, T0 arg0, T1 arg1, T2 arg2)
			{
				_items.Add((action, arg0, arg1, arg2, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, arg, arg2, arg3, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context, arg, arg2, arg3);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext, T0, T1, T2>, T0, T1, T2, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingActionGroup<T0, T1, T2, T3>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext, T0, T1, T2, T3> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, arg0, arg1, arg2, arg3, result));
				return result;
			}

			public void Add(Action<TContext, T0, T1, T2, T3> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
			{
				_items.Add((action, arg0, arg1, arg2, arg3, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, arg, arg2, arg3, arg4, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context, arg, arg2, arg3, arg4);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext, T0, T1, T2, T3>, T0, T1, T2, T3, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingActionGroup<T0, T1, T2, T3, T4>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<bool>> _sources = new Stack<PooledCompletionSource<bool>>();

			private List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>?)> _items = new List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>)>(512);

			private List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>?)> _oldItems = new List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<bool> AddAsync(Action<TContext, T0, T1, T2, T3, T4> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
			{
				if (!_sources.TryPop<PooledCompletionSource<bool>>(out var result))
				{
					result = new PooledCompletionSource<bool>();
				}
				_items.Add((action, arg0, arg1, arg2, arg3, arg4, result));
				return result;
			}

			public void Add(Action<TContext, T0, T1, T2, T3, T4> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
			{
				_items.Add((action, arg0, arg1, arg2, arg3, arg4, null));
			}

			public void Release(PooledCompletionSource<bool> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (action, arg, arg2, arg3, arg4, arg5, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						action(<owner>P._context, arg, arg2, arg3, arg4, arg5);
						pooledCompletionSource?.SetResult(result: true);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>)> items = _items;
				List<(Action<TContext, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4, PooledCompletionSource<bool>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingFuncGroup<TResult>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<TResult>> _sources = new Stack<PooledCompletionSource<TResult>>();

			private List<(Func<TContext, TResult>, PooledCompletionSource<TResult>?)> _items = new List<(Func<TContext, TResult>, PooledCompletionSource<TResult>)>(512);

			private List<(Func<TContext, TResult>, PooledCompletionSource<TResult>?)> _oldItems = new List<(Func<TContext, TResult>, PooledCompletionSource<TResult>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<TResult> AddAsync(Func<TContext, TResult> action)
			{
				if (!_sources.TryPop<PooledCompletionSource<TResult>>(out var result))
				{
					result = new PooledCompletionSource<TResult>();
				}
				_items.Add((action, result));
				return result;
			}

			public void Add(Func<TContext, TResult> func)
			{
				_items.Add((func, null));
			}

			public void Release(PooledCompletionSource<TResult> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (func, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						TResult result = func(<owner>P._context);
						pooledCompletionSource?.SetResult(result);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Func<TContext, TResult>, PooledCompletionSource<TResult>)> items = _items;
				List<(Func<TContext, TResult>, PooledCompletionSource<TResult>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingFuncGroup<T, TResult>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<TResult>> _sources = new Stack<PooledCompletionSource<TResult>>();

			private List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>?)> _items = new List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>)>(512);

			private List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>?)> _oldItems = new List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<TResult> AddAsync(Func<TContext, T, TResult> func, T arg)
			{
				if (!_sources.TryPop<PooledCompletionSource<TResult>>(out var result))
				{
					result = new PooledCompletionSource<TResult>();
				}
				_items.Add((func, arg, result));
				return result;
			}

			public void Add(Func<TContext, T, TResult> func, T arg)
			{
				_items.Add((func, arg, null));
			}

			public void Release(PooledCompletionSource<TResult> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (func, arg, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						TResult result = func(<owner>P._context, arg);
						pooledCompletionSource?.SetResult(result);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>)> items = _items;
				List<(Func<TContext, T, TResult>, T, PooledCompletionSource<TResult>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingFuncGroup<T0, T1, TResult>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<TResult>> _sources = new Stack<PooledCompletionSource<TResult>>();

			private List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>?)> _items = new List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>)>(512);

			private List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>?)> _oldItems = new List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<TResult> AddAsync(Func<TContext, T0, T1, TResult> func, T0 arg0, T1 arg1)
			{
				if (!_sources.TryPop<PooledCompletionSource<TResult>>(out var result))
				{
					result = new PooledCompletionSource<TResult>();
				}
				_items.Add((func, arg0, arg1, result));
				return result;
			}

			public void Add(Func<TContext, T0, T1, TResult> func, T0 arg0, T1 arg1)
			{
				_items.Add((func, arg0, arg1, null));
			}

			public void Release(PooledCompletionSource<TResult> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (func, arg, arg2, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						TResult result = func(<owner>P._context, arg, arg2);
						pooledCompletionSource?.SetResult(result);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>)> items = _items;
				List<(Func<TContext, T0, T1, TResult>, T0, T1, PooledCompletionSource<TResult>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		protected class PendingFuncGroup<T0, T1, T2, TResult>(PendingActionScheduler<TContext> owner) : PendingGroupBase()
		{
			public static readonly int TypeIndex = PendingGroupBase.TypeIndexCounter++;

			private readonly Stack<PooledCompletionSource<TResult>> _sources = new Stack<PooledCompletionSource<TResult>>();

			private List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>?)> _items = new List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>)>(512);

			private List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>?)> _oldItems = new List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>)>(512);

			private int _updateIndex;

			public PooledCompletionSource<TResult> AddAsync(Func<TContext, T0, T1, T2, TResult> func, T0 arg0, T1 arg1, T2 arg2)
			{
				if (!_sources.TryPop<PooledCompletionSource<TResult>>(out var result))
				{
					result = new PooledCompletionSource<TResult>();
				}
				_items.Add((func, arg0, arg1, arg2, result));
				return result;
			}

			public void Add(Func<TContext, T0, T1, T2, TResult> func, T0 arg0, T1 arg1, T2 arg2)
			{
				_items.Add((func, arg0, arg1, arg2, null));
			}

			public void Release(PooledCompletionSource<TResult> tcs)
			{
				tcs.Reset();
				_sources.Push(tcs);
			}

			public override void Update()
			{
				if (_updateIndex >= _oldItems.Count)
				{
					Reset();
				}
				var (func, arg, arg2, arg3, pooledCompletionSource) = _oldItems[_updateIndex++];
				try
				{
					Monitor.Exit(<owner>P._lock);
					try
					{
						TResult result = func(<owner>P._context, arg, arg2, arg3);
						pooledCompletionSource?.SetResult(result);
					}
					catch (Exception exception)
					{
						<owner>P._logger.LogError(exception, "Error executing pending action");
						pooledCompletionSource?.SetException(exception);
					}
				}
				finally
				{
					Monitor.Enter(<owner>P._lock);
				}
			}

			private void Reset()
			{
				_oldItems.Clear();
				List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>)> items = _items;
				List<(Func<TContext, T0, T1, T2, TResult>, T0, T1, T2, PooledCompletionSource<TResult>)> oldItems = _oldItems;
				_oldItems = items;
				_items = oldItems;
				_updateIndex = 0;
			}
		}

		private readonly ILogger _logger;

		protected readonly object _lock = new object();

		protected readonly PendingActionGroup _group;

		protected readonly PendingGroupBase[] _groups = new PendingGroupBase[256];

		protected List<PendingGroupBase?> _queue = new List<PendingGroupBase>(2048);

		protected List<PendingGroupBase?> _oldQueue = new List<PendingGroupBase>(2048);

		protected int _queueIndex;

		protected readonly TContext _context;

		protected PendingActionScheduler(TContext context, ILogger logger)
		{
			_logger = logger;
			_group = new PendingActionGroup(this);
			_context = context;
		}

		public async ValueTask RunAsync(Action<TContext> action)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context);
				return;
			}
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				tcs = _group.AddAsync(action);
				_queueIndex++;
				_queue.Add(_group);
			}
			await tcs.Task;
			lock (_lock)
			{
				_group.Release(tcs);
			}
		}

		public async ValueTask RunAsync<T>(Action<TContext, T> action, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg);
				return;
			}
			PendingActionGroup<T> group;
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T>.TypeIndex;
				group = (PendingActionGroup<T>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingActionGroup<T>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(action, arg);
				_queueIndex++;
				_queue.Add(group);
			}
			await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
		}

		public async ValueTask RunAsync<T0, T1>(Action<TContext, T0, T1> action, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1);
				return;
			}
			PendingActionGroup<T0, T1> group;
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1>.TypeIndex;
				group = (PendingActionGroup<T0, T1>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingActionGroup<T0, T1>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(action, arg0, arg1);
				_queueIndex++;
				_queue.Add(group);
			}
			await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
		}

		public async ValueTask RunAsync<T0, T1, T2>(Action<TContext, T0, T1, T2> action, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2);
				return;
			}
			PendingActionGroup<T0, T1, T2> group;
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2>.TypeIndex;
				group = (PendingActionGroup<T0, T1, T2>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingActionGroup<T0, T1, T2>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(action, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(group);
			}
			await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
		}

		public async ValueTask RunAsync<T0, T1, T2, T3>(Action<TContext, T0, T1, T2, T3> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3);
				return;
			}
			PendingActionGroup<T0, T1, T2, T3> group;
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3>.TypeIndex;
				group = (PendingActionGroup<T0, T1, T2, T3>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingActionGroup<T0, T1, T2, T3>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(action, arg0, arg1, arg2, arg3);
				_queueIndex++;
				_queue.Add(group);
			}
			await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
		}

		public async ValueTask RunAsync<T0, T1, T2, T3, T4>(Action<TContext, T0, T1, T2, T3, T4> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3, arg4);
				return;
			}
			PendingActionGroup<T0, T1, T2, T3, T4> group;
			PooledCompletionSource<bool> tcs;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3, T4>.TypeIndex;
				group = (PendingActionGroup<T0, T1, T2, T3, T4>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingActionGroup<T0, T1, T2, T3, T4>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(action, arg0, arg1, arg2, arg3, arg4);
				_queueIndex++;
				_queue.Add(group);
			}
			await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
		}

		public async ValueTask<TResult> RunFuncAsync<TResult>(Func<TContext, TResult> func)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context);
			}
			PendingFuncGroup<TResult> group;
			PooledCompletionSource<TResult> tcs;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<TResult>.TypeIndex;
				group = (PendingFuncGroup<TResult>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingFuncGroup<TResult>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(func);
				_queueIndex++;
				_queue.Add(group);
			}
			TResult result = await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
			return result;
		}

		public async ValueTask<TResult> RunFuncAsync<T, TResult>(Func<TContext, T, TResult> func, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg);
			}
			PendingFuncGroup<T, TResult> group;
			PooledCompletionSource<TResult> tcs;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T, TResult>.TypeIndex;
				group = (PendingFuncGroup<T, TResult>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingFuncGroup<T, TResult>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(func, arg);
				_queueIndex++;
				_queue.Add(group);
			}
			TResult result = await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
			return result;
		}

		public async ValueTask<TResult> RunFuncAsync<T0, T1, TResult>(Func<TContext, T0, T1, TResult> func, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg0, arg1);
			}
			PendingFuncGroup<T0, T1, TResult> group;
			PooledCompletionSource<TResult> tcs;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, TResult>.TypeIndex;
				group = (PendingFuncGroup<T0, T1, TResult>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingFuncGroup<T0, T1, TResult>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(func, arg0, arg1);
				_queueIndex++;
				_queue.Add(group);
			}
			TResult result = await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
			return result;
		}

		public async ValueTask<TResult> RunFuncAsync<T0, T1, T2, TResult>(Func<TContext, T0, T1, T2, TResult> func, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg0, arg1, arg2);
			}
			PendingFuncGroup<T0, T1, T2, TResult> group;
			PooledCompletionSource<TResult> tcs;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, T2, TResult>.TypeIndex;
				group = (PendingFuncGroup<T0, T1, T2, TResult>)_groups[typeIndex];
				if (group == null)
				{
					group = new PendingFuncGroup<T0, T1, T2, TResult>(this);
					_groups[typeIndex] = group;
				}
				tcs = group.AddAsync(func, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(group);
			}
			TResult result = await tcs.Task;
			lock (_lock)
			{
				group.Release(tcs);
			}
			return result;
		}

		public void Schedule(Action<TContext> action)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context);
				return;
			}
			lock (_lock)
			{
				_group.Add(action);
				_queueIndex++;
				_queue.Add(_group);
			}
		}

		public void Schedule<T>(Action<TContext, T> action, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T>.TypeIndex;
				PendingActionGroup<T> pendingActionGroup = (PendingActionGroup<T>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pendingActionGroup.Add(action, arg);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
		}

		public void Schedule<T0, T1>(Action<TContext, T0, T1> action, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1>.TypeIndex;
				PendingActionGroup<T0, T1> pendingActionGroup = (PendingActionGroup<T0, T1>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pendingActionGroup.Add(action, arg0, arg1);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
		}

		public void Schedule<T0, T1, T2>(Action<TContext, T0, T1, T2> action, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2>.TypeIndex;
				PendingActionGroup<T0, T1, T2> pendingActionGroup = (PendingActionGroup<T0, T1, T2>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pendingActionGroup.Add(action, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
		}

		public void Schedule<T0, T1, T2, T3>(Action<TContext, T0, T1, T2, T3> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3>.TypeIndex;
				PendingActionGroup<T0, T1, T2, T3> pendingActionGroup = (PendingActionGroup<T0, T1, T2, T3>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2, T3>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pendingActionGroup.Add(action, arg0, arg1, arg2, arg3);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
		}

		public void Schedule<T0, T1, T2, T3, T4>(Action<TContext, T0, T1, T2, T3, T4> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3, arg4);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3, T4>.TypeIndex;
				PendingActionGroup<T0, T1, T2, T3, T4> pendingActionGroup = (PendingActionGroup<T0, T1, T2, T3, T4>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2, T3, T4>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pendingActionGroup.Add(action, arg0, arg1, arg2, arg3, arg4);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
		}

		public void ScheduleFunc<TResult>(Func<TContext, TResult> func)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				func(_context);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<TResult>.TypeIndex;
				PendingFuncGroup<TResult> pendingFuncGroup = (PendingFuncGroup<TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pendingFuncGroup.Add(func);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
		}

		public void ScheduleFunc<T, TResult>(Func<TContext, T, TResult> func, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				func(_context, arg);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T, TResult>.TypeIndex;
				PendingFuncGroup<T, TResult> pendingFuncGroup = (PendingFuncGroup<T, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pendingFuncGroup.Add(func, arg);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
		}

		public void ScheduleFunc<T0, T1, TResult>(Func<TContext, T0, T1, TResult> func, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				func(_context, arg0, arg1);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, TResult>.TypeIndex;
				PendingFuncGroup<T0, T1, TResult> pendingFuncGroup = (PendingFuncGroup<T0, T1, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T0, T1, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pendingFuncGroup.Add(func, arg0, arg1);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
		}

		public void ScheduleFunc<T0, T1, T2, TResult>(Func<TContext, T0, T1, T2, TResult> func, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				func(_context, arg0, arg1, arg2);
				return;
			}
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, T2, TResult>.TypeIndex;
				PendingFuncGroup<T0, T1, T2, TResult> pendingFuncGroup = (PendingFuncGroup<T0, T1, T2, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T0, T1, T2, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pendingFuncGroup.Add(func, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
		}

		public void RunSynchronously(Action<TContext> action)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context);
				return;
			}
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				pooledCompletionSource = _group.AddAsync(action);
				_queueIndex++;
				_queue.Add(_group);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				_group.Release(pooledCompletionSource);
			}
		}

		public void RunSynchronously<T>(Action<TContext, T> action, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg);
				return;
			}
			PendingActionGroup<T> pendingActionGroup;
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T>.TypeIndex;
				pendingActionGroup = (PendingActionGroup<T>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pooledCompletionSource = pendingActionGroup.AddAsync(action, arg);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingActionGroup.Release(pooledCompletionSource);
			}
		}

		public void RunSynchronously<T0, T1>(Action<TContext, T0, T1> action, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1);
				return;
			}
			PendingActionGroup<T0, T1> pendingActionGroup;
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1>.TypeIndex;
				pendingActionGroup = (PendingActionGroup<T0, T1>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pooledCompletionSource = pendingActionGroup.AddAsync(action, arg0, arg1);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingActionGroup.Release(pooledCompletionSource);
			}
		}

		public void RunSynchronously<T0, T1, T2>(Action<TContext, T0, T1, T2> action, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2);
				return;
			}
			PendingActionGroup<T0, T1, T2> pendingActionGroup;
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2>.TypeIndex;
				pendingActionGroup = (PendingActionGroup<T0, T1, T2>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pooledCompletionSource = pendingActionGroup.AddAsync(action, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingActionGroup.Release(pooledCompletionSource);
			}
		}

		public void RunSynchronously<T0, T1, T2, T3>(Action<TContext, T0, T1, T2, T3> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3);
				return;
			}
			PendingActionGroup<T0, T1, T2, T3> pendingActionGroup;
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3>.TypeIndex;
				pendingActionGroup = (PendingActionGroup<T0, T1, T2, T3>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2, T3>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pooledCompletionSource = pendingActionGroup.AddAsync(action, arg0, arg1, arg2, arg3);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingActionGroup.Release(pooledCompletionSource);
			}
		}

		public void RunSynchronously<T0, T1, T2, T3, T4>(Action<TContext, T0, T1, T2, T3, T4> action, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				action(_context, arg0, arg1, arg2, arg3, arg4);
				return;
			}
			PendingActionGroup<T0, T1, T2, T3, T4> pendingActionGroup;
			PooledCompletionSource<bool> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingActionGroup<T0, T1, T2, T3, T4>.TypeIndex;
				pendingActionGroup = (PendingActionGroup<T0, T1, T2, T3, T4>)_groups[typeIndex];
				if (pendingActionGroup == null)
				{
					pendingActionGroup = new PendingActionGroup<T0, T1, T2, T3, T4>(this);
					_groups[typeIndex] = pendingActionGroup;
				}
				pooledCompletionSource = pendingActionGroup.AddAsync(action, arg0, arg1, arg2, arg3, arg4);
				_queueIndex++;
				_queue.Add(pendingActionGroup);
			}
			pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingActionGroup.Release(pooledCompletionSource);
			}
		}

		public TResult RunSynchronously<TResult>(Func<TContext, TResult> func)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context);
			}
			PendingFuncGroup<TResult> pendingFuncGroup;
			PooledCompletionSource<TResult> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<TResult>.TypeIndex;
				pendingFuncGroup = (PendingFuncGroup<TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pooledCompletionSource = pendingFuncGroup.AddAsync(func);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
			TResult result = pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingFuncGroup.Release(pooledCompletionSource);
				return result;
			}
		}

		public TResult RunSynchronously<T, TResult>(Func<TContext, T, TResult> func, T arg)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg);
			}
			PendingFuncGroup<T, TResult> pendingFuncGroup;
			PooledCompletionSource<TResult> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T, TResult>.TypeIndex;
				pendingFuncGroup = (PendingFuncGroup<T, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pooledCompletionSource = pendingFuncGroup.AddAsync(func, arg);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
			TResult result = pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingFuncGroup.Release(pooledCompletionSource);
				return result;
			}
		}

		public TResult RunSynchronously<T0, T1, TResult>(Func<TContext, T0, T1, TResult> func, T0 arg0, T1 arg1)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg0, arg1);
			}
			PendingFuncGroup<T0, T1, TResult> pendingFuncGroup;
			PooledCompletionSource<TResult> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, TResult>.TypeIndex;
				pendingFuncGroup = (PendingFuncGroup<T0, T1, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T0, T1, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pooledCompletionSource = pendingFuncGroup.AddAsync(func, arg0, arg1);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
			TResult result = pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingFuncGroup.Release(pooledCompletionSource);
				return result;
			}
		}

		public TResult RunSynchronously<T0, T1, T2, TResult>(Func<TContext, T0, T1, T2, TResult> func, T0 arg0, T1 arg1, T2 arg2)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				if (_queueIndex > 0)
				{
					Update();
				}
				return func(_context, arg0, arg1, arg2);
			}
			PendingFuncGroup<T0, T1, T2, TResult> pendingFuncGroup;
			PooledCompletionSource<TResult> pooledCompletionSource;
			lock (_lock)
			{
				int typeIndex = PendingFuncGroup<T0, T1, T2, TResult>.TypeIndex;
				pendingFuncGroup = (PendingFuncGroup<T0, T1, T2, TResult>)_groups[typeIndex];
				if (pendingFuncGroup == null)
				{
					pendingFuncGroup = new PendingFuncGroup<T0, T1, T2, TResult>(this);
					_groups[typeIndex] = pendingFuncGroup;
				}
				pooledCompletionSource = pendingFuncGroup.AddAsync(func, arg0, arg1, arg2);
				_queueIndex++;
				_queue.Add(pendingFuncGroup);
			}
			TResult result = pooledCompletionSource.Task.AsTask().GetAwaiter().GetResult();
			lock (_lock)
			{
				pendingFuncGroup.Release(pooledCompletionSource);
				return result;
			}
		}
	}
	public abstract class PendingActionSchedulerBase
	{
		protected const int MaxGroupCount = 256;

		protected const int MaxPendingItemCount = 2048;

		protected const int MaxPendingGroupItemCount = 512;

		protected Thread? thread;

		public void EnsureThread()
		{
			if (thread != null && thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException($"This call should be made from the thread that created the scheduler. Expected: {thread}, Actual: {Thread.CurrentThread}");
			}
		}

		public NetDataReader MakeSafe(NetDataReader reader)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (thread == null || thread == Thread.CurrentThread)
			{
				return reader;
			}
			byte[] remainingBytes = reader.GetRemainingBytes();
			return new NetDataReader(remainingBytes, 0, remainingBytes.Length);
		}

		public NetDataWriter MakeSafe(NetDataWriter writer)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			if (thread == null || thread == Thread.CurrentThread)
			{
				return writer;
			}
			NetDataWriter val = new NetDataWriter(true, writer.Length);
			val.Put(writer.Data);
			return val;
		}

		public List<T> MakeSafe<T>(List<T> lst)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				return lst;
			}
			return new List<T>(lst);
		}

		public ReadOnlyList<T> MakeSafe<T>(ReadOnlyList<T> lst)
		{
			if (thread == null || thread == Thread.CurrentThread)
			{
				return lst;
			}
			ReadOnlyList<T> readOnlyList = lst;
			List<T> list = new List<T>(readOnlyList.Count);
			foreach (T item in readOnlyList)
			{
				list.Add(item);
			}
			return new ReadOnlyList<T>(list);
		}

		public abstract bool Update();
	}
	public class PendingActionUpdater<TContext> : PendingActionScheduler<TContext>
	{
		private bool _insideUpdate;

		public Thread? Thread => thread;

		public PendingActionUpdater(TContext context, ILogger logger)
			: base(context, logger)
		{
		}

		public override bool Update()
		{
			if (_insideUpdate)
			{
				return false;
			}
			try
			{
				_insideUpdate = true;
				Monitor.Enter(_lock);
				List<PendingActionScheduler<TContext>.PendingGroupBase> queue = _queue;
				List<PendingActionScheduler<TContext>.PendingGroupBase> oldQueue = _oldQueue;
				_oldQueue = queue;
				_queue = oldQueue;
				int queueIndex = _queueIndex;
				_queue.Clear();
				_queueIndex = 0;
				for (int i = 0; i < queueIndex; i++)
				{
					PendingActionScheduler<TContext>.PendingGroupBase? pendingGroupBase = _oldQueue[i];
					_oldQueue[i] = null;
					pendingGroupBase.Update();
				}
				return queueIndex > 0;
			}
			finally
			{
				Monitor.Exit(_lock);
				_insideUpdate = false;
			}
		}

		public void SetThread(Thread? newThread)
		{
			thread = newThread;
		}
	}
	public readonly struct ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		public int Count => <dictionary>P.Count;

		public TValue this[TKey key] => <dictionary>P[key];

		public Dictionary<TKey, TValue>.KeyCollection Keys => <dictionary>P.Keys;

		public Dictionary<TKey, TValue>.ValueCollection Values => <dictionary>P.Values;

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => <dictionary>P.Keys;

		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => <dictionary>P.Values;

		public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
		{
			<dictionary>P = dictionary;
		}

		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return <dictionary>P.GetEnumerator();
		}

		public bool ContainsKey(TKey key)
		{
			return <dictionary>P.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return <dictionary>P.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)<dictionary>P).GetEnumerator();
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return <dictionary>P.GetEnumerator();
		}
	}
	public readonly struct ReadOnlyList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		private static readonly List<T> _emptyList = new List<T>();

		public int Count => <list>P.Count;

		public T this[int index] => <list>P[index];

		public static ReadOnlyList<T> Empty => new ReadOnlyList<T>(_emptyList);

		public ReadOnlyList(List<T> list)
		{
			<list>P = list;
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return <list>P.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return <list>P.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)<list>P).GetEnumerator();
		}

		public bool Contains(T item)
		{
			return <list>P.Contains(item);
		}

		public ReadOnlyList<T> Copy()
		{
			return new ReadOnlyList<T>(new List<T>(<list>P));
		}
	}
	public static class ReadOnlyListExtensions
	{
		public static ReadOnlyList<T> WrapReadOnly<T>(this List<T> list)
		{
			return new ReadOnlyList<T>(list);
		}

		public static ReadOnlyList<T> NullableWrapReadOnly<T>(this List<T>? list)
		{
			if (list == null)
			{
				return ReadOnlyList<T>.Empty;
			}
			return new ReadOnlyList<T>(list);
		}
	}
}
namespace ReadyM.Api.Generators
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class WrapperForAttribute : Attribute
	{
		public Type TargetType { get; }

		public WrapperForAttribute(Type targetType)
		{
			TargetType = targetType;
		}
	}
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class WrapperIncludeAttribute : Attribute
	{
		public string Regex { get; }

		public WrapperIncludeAttribute(string regex)
		{
			Regex = regex;
		}
	}
}
namespace ReadyM.Api.ECS.Worlds
{
	[WrapperFor(typeof(CreateEntityBatch))]
	[WrapperInclude("Add")]
	public sealed class EntityBuilder
	{
		private readonly CreateEntityBatch _wrapped;

		internal EntityBuilder(CreateEntityBatch wrapped)
		{
			_wrapped = wrapped;
		}

		[ExcludeFromCodeCoverage]
		public CreateEntityBatch Add<T>(in T component) where T : struct, IComponent
		{
			return _wrapped.Add(in component);
		}

		[ExcludeFromCodeCoverage]
		public CreateEntityBatch Add<T>() where T : struct, IComponent
		{
			return _wrapped.Add<T>();
		}

		[ExcludeFromCodeCoverage]
		public CreateEntityBatch AddTag<T>() where T : struct, ITag
		{
			return _wrapped.AddTag<T>();
		}

		[ExcludeFromCodeCoverage]
		public CreateEntityBatch AddTags(in Tags tags)
		{
			return _wrapped.AddTags(in tags);
		}
	}
	[WrapperFor(typeof(EntityStore))]
	[WrapperInclude("^Query.*")]
	[WrapperInclude("^Count$")]
	[WrapperInclude("^GetCommandBuffer$")]
	[WrapperInclude("^OnEntit.*")]
	[WrapperInclude("^OnTag.*")]
	[WrapperInclude("^EventRecorder")]
	public sealed class Store
	{
		private struct ArchetypeEntry
		{
			public Action<EntityBuilder> Constructor;

			public Action<Entity>? LateInit;
		}

		private byte _nextArchetypeId;

		private readonly Dictionary<ArchetypeId, ArchetypeEntry> _archetypeEntries = new Dictionary<ArchetypeId, ArchetypeEntry>();

		private readonly EntityStore _wrapped;

		public SystemRoot SystemRoot { get; }

		[ExcludeFromCodeCoverage]
		public EventRecorder EventRecorder => _wrapped.EventRecorder;

		[ExcludeFromCodeCoverage]
		public int Count => _wrapped.Count;

		[ExcludeFromCodeCoverage]
		public event EventHandler<EntitiesChanged> OnEntitiesChanged
		{
			add
			{
				_wrapped.OnEntitiesChanged += value;
			}
			remove
			{
				_wrapped.OnEntitiesChanged -= value;
			}
		}

		[ExcludeFromCodeCoverage]
		public event Action<EntityCreate> OnEntityCreate
		{
			add
			{
				_wrapped.OnEntityCreate += value;
			}
			remove
			{
				_wrapped.OnEntityCreate -= value;
			}
		}

		[ExcludeFromCodeCoverage]
		public event Action<EntityDelete> OnEntityDelete
		{
			add
			{
				_wrapped.OnEntityDelete += value;
			}
			remove
			{
				_wrapped.OnEntityDelete -= value;
			}
		}

		[ExcludeFromCodeCoverage]
		public event Action<TagsChanged> OnTagsChanged
		{
			add
			{
				_wrapped.OnTagsChanged += value;
			}
			remove
			{
				_wrapped.OnTagsChanged -= value;
			}
		}

		public Store(EntityStore wrapped, IEnumerable<IArchetypeRegistration> registrations)
		{
			_wrapped = wrapped;
			SystemRoot = new SystemRoot();
			SystemRoot.AddStore(wrapped);
			foreach (IArchetypeRegistration registration in registrations)
			{
				registration.Register(this);
			}
		}

		public ArchetypeId RegisterArchetype(Action<EntityBuilder> constructor, Action<Entity>? lateInit = null)
		{
			ArchetypeId archetypeId = new ArchetypeId(_nextArchetypeId++);
			_archetypeEntries[archetypeId] = new ArchetypeEntry
			{
				Constructor = constructor,
				LateInit = lateInit
			};
			return archetypeId;
		}

		public Entity CreateEntity(ArchetypeId archetypeId, Action<EntityBuilder>? setComponents = null)
		{
			if (!_archetypeEntries.TryGetValue(archetypeId, out var value))
			{
				throw new ArgumentException($"Archetype with ID {archetypeId} is not registered.");
			}
			CreateEntityBatch createEntityBatch = _wrapped.Batch();
			EntityBuilder obj = new EntityBuilder(createEntityBatch);
			value.Constructor(obj);
			setComponents?.Invoke(obj);
			Entity entity = createEntityBatch.CreateEntity();
			value.LateInit?.Invoke(entity);
			return entity;
		}

		internal Entity CreateEntity(Action<EntityBuilder>? setComponents = null)
		{
			CreateEntityBatch createEntityBatch = _wrapped.Batch();
			EntityBuilder obj = new EntityBuilder(createEntityBatch);
			setComponents?.Invoke(obj);
			return createEntityBatch.CreateEntity();
		}

		public ComponentIndex<TIndexedComponent, TValue> ComponentIndex<TIndexedComponent, TValue>() where TIndexedComponent : struct, IIndexedComponent<TValue>
		{
			return _wrapped.ComponentIndex<TIndexedComponent, TValue>();
		}

		public LinkComponentIndex<TLinkComponent> LinkComponentIndex<TLinkComponent>() where TLinkComponent : struct, ILinkComponent
		{
			return _wrapped.LinkComponentIndex<TLinkComponent>();
		}

		[ExcludeFromCodeCoverage]
		public CommandBuffer GetCommandBuffer()
		{
			return _wrapped.GetCommandBuffer();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1> QueryRelation<T1>() where T1 : struct, IRelation
		{
			return _wrapped.QueryRelation<T1>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1> QueryRelation<T1>(QueryFilter filter) where T1 : struct, IRelation
		{
			return _wrapped.QueryRelation<T1>(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery Query()
		{
			return _wrapped.Query();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery Query(QueryFilter filter)
		{
			return _wrapped.Query(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1> Query<T1>(Signature<T1> signature) where T1 : struct, IComponent
		{
			return _wrapped.Query(signature);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1> Query<T1>() where T1 : struct, IComponent
		{
			return _wrapped.Query<T1>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1> Query<T1>(QueryFilter filter) where T1 : struct, IComponent
		{
			return _wrapped.Query<T1>(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2> Query<T1, T2>(Signature<T1, T2> signature) where T1 : struct, IComponent where T2 : struct, IComponent
		{
			return _wrapped.Query(signature);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2> Query<T1, T2>() where T1 : struct, IComponent where T2 : struct, IComponent
		{
			return _wrapped.Query<T1, T2>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2> Query<T1, T2>(QueryFilter filter) where T1 : struct, IComponent where T2 : struct, IComponent
		{
			return _wrapped.Query<T1, T2>(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3> Query<T1, T2, T3>(Signature<T1, T2, T3> signature) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
		{
			return _wrapped.Query(signature);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3> Query<T1, T2, T3>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3> Query<T1, T2, T3>(QueryFilter filter) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3>(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4> Query<T1, T2, T3, T4>(Signature<T1, T2, T3, T4> signature) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
		{
			return _wrapped.Query(signature);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4> Query<T1, T2, T3, T4>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3, T4>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4> Query<T1, T2, T3, T4>(QueryFilter filter) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3, T4>(filter);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>(Signature<T1, T2, T3, T4, T5> signature) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent
		{
			return _wrapped.Query(signature);
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3, T4, T5>();
		}

		[ExcludeFromCodeCoverage]
		public ArchetypeQuery<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>(QueryFilter filter) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent
		{
			return _wrapped.Query<T1, T2, T3, T4, T5>(filter);
		}
	}
}
namespace ReadyM.Api.ECS.Registry
{
	public abstract class ComponentRegistryBase<TRegistry, TComponent> : IComponentRegistryBase<TRegistry, TComponent> where TRegistry : IComponentRegistryBase<TRegistry, TComponent>
	{
		private readonly List<Action<IComponentRegistryCallbackBase<TRegistry, TComponent>>> _acceptCallbacks = new List<Action<IComponentRegistryCallbackBase<TRegistry, TComponent>>>();

		private readonly List<Type> _componentTypes = new List<Type>();

		public IReadOnlyList<Type> ComponentTypes => _componentTypes;

		protected ComponentRegistryBase(IEnumerable<IComponentRegistrationBase<TRegistry, TComponent>> registrations)
		{
			TRegistry registry = (TRegistry)(object)this;
			foreach (IComponentRegistrationBase<TRegistry, TComponent> registration in registrations)
			{
				registration.Register(registry);
			}
		}

		public virtual TRegistry RegisterComponent<T>(T defaultValue = default(T)) where T : struct, TComponent
		{
			_componentTypes.Add(typeof(T));
			_acceptCallbacks.Add(delegate(IComponentRegistryCallbackBase<TRegistry, TComponent> callback)
			{
				callback.AcceptComponent<T>((TRegistry)(object)this, defaultValue);
			});
			return (TRegistry)(object)this;
		}

		public void Accept(IComponentRegistryCallbackBase<TRegistry, TComponent> callbackBase)
		{
			foreach (Action<IComponentRegistryCallbackBase<TRegistry, TComponent>> acceptCallback in _acceptCallbacks)
			{
				acceptCallback(callbackBase);
			}
		}
	}
	public interface IArchetypeRegistration
	{
		void Register(Store world);
	}
	public interface IAreaComponentRegistration : IComponentRegistrationBase<IAreaComponentRegistry, IComponent>
	{
	}
	public interface IAreaComponentRegistry : IComponentRegistryBase<IAreaComponentRegistry, IComponent>
	{
	}
	public interface IComponentRegistrationBase<in TRegistry, TComponent> where TRegistry : IComponentRegistryBase<TRegistry, TComponent>
	{
		void Register(TRegistry registry);
	}
	public interface IComponentRegistryBase<out TRegistry, TComponent> where TRegistry : IComponentRegistryBase<TRegistry, TComponent>
	{
		IReadOnlyList<Type> ComponentTypes { get; }

		TRegistry RegisterComponent<T>(T defaultValue = default(T)) where T : struct, TComponent;

		void Accept(IComponentRegistryCallbackBase<TRegistry, TComponent> callbackBase);
	}
	public interface IComponentRegistryCallbackBase<in TRegistry, in TComponent>
	{
		void AcceptComponent<T>(TRegistry registry, T defaultValue = default(T)) where T : struct, TComponent;
	}
	public interface IPlayerComponentRegistration : IComponentRegistrationBase<IPlayerComponentRegistry, IComponent>
	{
	}
	public interface IPlayerComponentRegistry : IComponentRegistryBase<IPlayerComponentRegistry, IComponent>
	{
	}
}
namespace ReadyM.Api.ECS.Jobs
{
	public interface IJob
	{
		void Execute();
	}
	public interface IJob<in T>
	{
		void Execute(T arg);
	}
	public interface IJob<in T0, in T1>
	{
		void Execute(T0 arg0, T1 arg1);
	}
	public interface IJob<in T0, in T1, in T2>
	{
		void Execute(T0 arg0, T1 arg1, T2 arg2);
	}
	public interface IJob<in T0, in T1, in T2, in T3>
	{
		void Execute(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
	}
}
namespace System.Diagnostics.CodeAnalysis
{
	internal class DoesNotReturnIfAttribute : Attribute
	{
		public bool ParameterValue { get; }

		public DoesNotReturnIfAttribute(bool parameterValue)
		{
			ParameterValue = parameterValue;
		}
	}
}
