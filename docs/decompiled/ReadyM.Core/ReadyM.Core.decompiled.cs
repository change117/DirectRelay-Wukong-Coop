using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.DependencyInjection;
using ReadyM.Core.Types;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETCoreApp,Version=v10.0", FrameworkDisplayName = ".NET 10.0")]
[assembly: AssemblyCompany("ReadyM.Core")]
[assembly: AssemblyConfiguration("Production")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0+2612d8b0622b4c01f619f7ca5d665df3b98ec32c")]
[assembly: AssemblyProduct("ReadyM.Core")]
[assembly: AssemblyTitle("ReadyM.Core")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace ReadyM.Core
{
	public record ModId
	{
		public required string VersionlessId { get; init; }

		public required string Version { get; init; }

		public override string ToString()
		{
			return VersionlessId;
		}

		[CompilerGenerated]
		[SetsRequiredMembers]
		protected ModId(ModId original)
		{
			VersionlessId = original.VersionlessId;
			Version = original.Version;
		}

		public ModId()
		{
		}
	}
}
namespace ReadyM.Core.Types
{
	public record ModMetadata
	{
		public required string platform_id { get; init; }

		public required string versionless_id { get; init; }

		public required string author { get; init; }

		public string? description { get; init; }

		public required string name { get; init; }

		public required string version { get; init; }

		public string? logoLink { get; init; }

		public required string viewLink { get; init; }

		public required string downloadLink { get; init; }

		[JsonIgnore]
		public ModId Id => new ModId
		{
			VersionlessId = versionless_id,
			Version = version
		};

		[CompilerGenerated]
		[SetsRequiredMembers]
		protected ModMetadata(ModMetadata original)
		{
			platform_id = original.platform_id;
			versionless_id = original.versionless_id;
			author = original.author;
			description = original.description;
			name = original.name;
			version = original.version;
			logoLink = original.logoLink;
			viewLink = original.viewLink;
			downloadLink = original.downloadLink;
		}

		public ModMetadata()
		{
		}
	}
	public enum RatingClass
	{
		Unknown,
		Good,
		Mediocre,
		Bad
	}
	public abstract record ModRating : IComparable
	{
		public abstract RatingClass RatingClass { get; }

		public abstract int CompareTo(object? obj);
	}
	public record PercentageModRating(int Percent, int Total) : ModRating()
	{
		public override RatingClass RatingClass
		{
			get
			{
				if (Total == 0)
				{
					return RatingClass.Unknown;
				}
				int percent = Percent;
				return (percent >= 80) ? RatingClass.Good : ((percent < 50) ? RatingClass.Bad : RatingClass.Mediocre);
			}
		}

		public override int CompareTo(object? obj)
		{
			if (obj is PercentageModRating percentageModRating)
			{
				return Percent.CompareTo(percentageModRating.Percent);
			}
			return 0;
		}
	}
	public record UpvoteCountModRating(int Upvotes) : ModRating()
	{
		public override RatingClass RatingClass
		{
			get
			{
				if (Upvotes <= 0)
				{
					return RatingClass.Unknown;
				}
				return RatingClass.Good;
			}
		}

		public override int CompareTo(object? obj)
		{
			if (obj is UpvoteCountModRating upvoteCountModRating)
			{
				return Upvotes.CompareTo(upvoteCountModRating.Upvotes);
			}
			return 0;
		}
	}
	public record OnlineModMetadata : ModMetadata
	{
		public int downloads { get; init; }

		public ModRating rating { get; init; }

		public DateTime created { get; init; }

		public DateTime lastUpdated { get; init; }

		public List<string> tags { get; init; } = new List<string>();

		[CompilerGenerated]
		[SetsRequiredMembers]
		protected OnlineModMetadata(OnlineModMetadata original)
			: base(original)
		{
			downloads = original.downloads;
			rating = original.rating;
			created = original.created;
			lastUpdated = original.lastUpdated;
			tags = original.tags;
		}

		public OnlineModMetadata()
		{
		}
	}
	public record PagedQueryResult<T>(ICollection<T> Page, int TotalResults)
	{
		public static PagedQueryResult<T> Empty => new PagedQueryResult<T>(Array.Empty<T>(), 0);
	}
}
namespace ReadyM.Core.Primitives
{
	public sealed class Error : ValueObject
	{
		public string Code { get; }

		[Obsolete("Front ends should localize error messages themselves. Right now, the player portal displays these on code 400. This should be removed after we localize the website.")]
		public string Message { get; }

		internal static Error None => new Error(string.Empty, string.Empty);

		public Error(string code, string message = "")
		{
			Code = code;
			Message = message;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Code;
			yield return Message;
		}
	}
	public abstract class ValueObject : IEquatable<ValueObject>
	{
		public static bool operator ==(ValueObject a, ValueObject b)
		{
			if ((object)a == null && (object)b == null)
			{
				return true;
			}
			if ((object)a == null || (object)b == null)
			{
				return false;
			}
			return a.Equals(b);
		}

		public static bool operator !=(ValueObject a, ValueObject b)
		{
			return !(a == b);
		}

		public bool Equals(ValueObject other)
		{
			if ((object)other != null)
			{
				return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			if (!(obj is ValueObject valueObject))
			{
				return false;
			}
			return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
		}

		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			foreach (object atomicValue in GetAtomicValues())
			{
				hashCode.Add(atomicValue);
			}
			return hashCode.ToHashCode();
		}

		protected abstract IEnumerable<object> GetAtomicValues();
	}
}
namespace ReadyM.Core.Primitives.Result
{
	public class Result
	{
		public bool IsSuccess { get; }

		public bool IsFailure => !IsSuccess;

		public Error Error { get; }

		protected Result(bool isSuccess, Error error)
		{
			if (isSuccess && error != ReadyM.Core.Primitives.Error.None)
			{
				throw new InvalidOperationException();
			}
			if (!isSuccess && error == ReadyM.Core.Primitives.Error.None)
			{
				throw new InvalidOperationException();
			}
			IsSuccess = isSuccess;
			Error = error;
		}

		public static Result Success()
		{
			return new Result(isSuccess: true, ReadyM.Core.Primitives.Error.None);
		}

		public static Result<TValue> Success<TValue>(TValue value)
		{
			return new Result<TValue>(value, isSuccess: true, ReadyM.Core.Primitives.Error.None);
		}

		public static Result<TValue> Create<TValue>(TValue? value, Error error) where TValue : class
		{
			if (value != null)
			{
				return Success(value);
			}
			return Failure<TValue>(error);
		}

		public static Result Failure(Error error)
		{
			return new Result(isSuccess: false, error);
		}

		public static Result<TValue> Failure<TValue>(Error error)
		{
			return new Result<TValue>(default(TValue), isSuccess: false, error);
		}

		public static Result FirstFailureOrSuccess(params Result[] results)
		{
			foreach (Result result in results)
			{
				if (result.IsFailure)
				{
					return result;
				}
			}
			return Success();
		}
	}
	public class Result<TValue> : Result
	{
		private readonly TValue _value;

		public TValue Value
		{
			get
			{
				if (!base.IsSuccess)
				{
					throw new InvalidOperationException("The value of a failure result can not be accessed.");
				}
				return _value;
			}
		}

		protected internal Result(TValue value, bool isSuccess, Error error)
			: base(isSuccess, error)
		{
			_value = value;
		}

		public static implicit operator Result<TValue>(TValue value)
		{
			return Result.Success(value);
		}
	}
	public static class ResultExtensions
	{
		public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
		{
			if (result.IsFailure)
			{
				return result;
			}
			if (!result.IsSuccess || !predicate(result.Value))
			{
				return Result.Failure<T>(error);
			}
			return result;
		}

		public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func)
		{
			if (!result.IsSuccess)
			{
				return Result.Failure<TOut>(result.Error);
			}
			return func(result.Value);
		}

		public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
		{
			return (!result.IsSuccess) ? Result.Failure(result.Error) : (await func(result.Value));
		}

		public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
		{
			return (!result.IsSuccess) ? Result.Failure<TOut>(result.Error) : (await func(result.Value));
		}

		public static async Task<T> Match<T>(this Task<Result> resultTask, Func<T> onSuccess, Func<Error, T> onFailure)
		{
			Result result = await resultTask;
			return result.IsSuccess ? onSuccess() : onFailure(result.Error);
		}

		public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
		{
			Result<TIn> result = await resultTask;
			return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
		}
	}
}
namespace ReadyM.Core.Mods
{
	public record InstalledModMetadata : ModMetadata
	{
		public required bool IsEnabled { get; set; }

		public required HashSet<ModId> Dependencies { get; set; }

		public required HashSet<ModId> Dependants { get; set; }

		public required DateTime InstalledAt { get; init; }

		public static InstalledModMetadata FromModMetadata(ModMetadata modMetadata, IEnumerable<ModId> dependants, IEnumerable<ModId> dependencies)
		{
			InstalledModMetadata installedModMetadata = new InstalledModMetadata();
			installedModMetadata.platform_id = modMetadata.platform_id;
			installedModMetadata.versionless_id = modMetadata.versionless_id;
			installedModMetadata.author = modMetadata.author;
			installedModMetadata.description = modMetadata.description;
			installedModMetadata.downloadLink = modMetadata.downloadLink;
			installedModMetadata.name = modMetadata.name;
			installedModMetadata.version = modMetadata.version;
			installedModMetadata.viewLink = modMetadata.viewLink;
			installedModMetadata.logoLink = modMetadata.logoLink;
			installedModMetadata.InstalledAt = DateTime.Now;
			installedModMetadata.IsEnabled = true;
			InstalledModMetadata installedModMetadata2 = installedModMetadata;
			HashSet<ModId> hashSet = new HashSet<ModId>();
			foreach (ModId dependant in dependants)
			{
				hashSet.Add(dependant);
			}
			installedModMetadata2.Dependants = hashSet;
			InstalledModMetadata installedModMetadata3 = installedModMetadata;
			HashSet<ModId> hashSet2 = new HashSet<ModId>();
			foreach (ModId dependency in dependencies)
			{
				hashSet2.Add(dependency);
			}
			installedModMetadata3.Dependencies = hashSet2;
			return installedModMetadata;
		}

		[CompilerGenerated]
		[SetsRequiredMembers]
		protected InstalledModMetadata(InstalledModMetadata original)
			: base(original)
		{
			IsEnabled = original.IsEnabled;
			Dependencies = original.Dependencies;
			Dependants = original.Dependants;
			InstalledAt = original.InstalledAt;
		}

		public InstalledModMetadata()
		{
		}
	}
}
namespace ReadyM.Core.Helpers
{
	public static class AsyncEnumerableExtensions
	{
		public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken ct = default(CancellationToken))
		{
			List<T> list = new List<T>();
			await foreach (T item in source.WithCancellation(ct).ConfigureAwait(continueOnCapturedContext: false))
			{
				list.Add(item);
			}
			return list;
		}

		public static async IAsyncEnumerable<T2> Select<T1, T2>(this IAsyncEnumerable<T1> source, Func<T1, T2> selector)
		{
			await foreach (T1 item in source)
			{
				yield return selector(item);
			}
		}
	}
	public record DiffResult<T>(List<T> Added, List<T> Removed, List<(T Old, T New)> Modified);
	public static class CollectionHelpers
	{
		public static DiffResult<T> Diff<T>(List<T> oldList, List<T> newList, Func<T, T, bool> keyFunc, Func<T, T, bool> isUnchanged)
		{
			List<T> list = new List<T>();
			List<T> list2 = new List<T>();
			List<(T, T)> list3 = new List<(T, T)>();
			foreach (T oldItem in oldList)
			{
				T val = newList.FirstOrDefault((T x) => keyFunc(oldItem, x));
				if (val == null)
				{
					list2.Add(oldItem);
				}
				else if (!isUnchanged(oldItem, val))
				{
					list3.Add((oldItem, val));
				}
			}
			foreach (T newItem in newList)
			{
				if (!oldList.Any((T x) => keyFunc(x, newItem)))
				{
					list.Add(newItem);
				}
			}
			return new DiffResult<T>(list, list2, list3);
		}
	}
	public static class DependencyResolutionHelper
	{
		[CompilerGenerated]
		private sealed class <ResolveSortedDependencies>d__0 : IAsyncEnumerable<ModId>, IAsyncEnumerator<ModId>, IAsyncDisposable, IValueTaskSource<bool>, IValueTaskSource, IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncIteratorMethodBuilder <>t__builder;

			public ManualResetValueTaskSourceCore<bool> <>v__promiseOfValueOrEnd;

			private ModId <>2__current;

			private bool <>w__disposeMode;

			private CancellationTokenSource <>x__combinedTokens;

			private int <>l__initialThreadId;

			private IEnumerable<ModId> topLevelPackages;

			public IEnumerable<ModId> <>3__topLevelPackages;

			private Func<ModId, CancellationToken, ValueTask<ModId[]>> getPackageDeps;

			public Func<ModId, CancellationToken, ValueTask<ModId[]>> <>3__getPackageDeps;

			private CancellationToken ct;

			public CancellationToken <>3__ct;

			private bool includeTopLevel;

			public bool <>3__includeTopLevel;

			private HashSet<ModId> <topLevel>5__2;

			private Queue<ModId> <toProcess>5__3;

			private HashSet<ModId> <visited>5__4;

			private ValueTaskAwaiter<ModId[]> <>u__1;

			private Dictionary<string, ModId>.ValueCollection.Enumerator <>7__wrap4;

			ModId IAsyncEnumerator<ModId>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <ResolveSortedDependencies>d__0(int <>1__state)
			{
				<>t__builder = AsyncIteratorMethodBuilder.Create();
				this.<>1__state = <>1__state;
				<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			private void MoveNext()
			{
				int num = <>1__state;
				try
				{
					ValueTaskAwaiter<ModId[]> awaiter;
					ModId modId;
					Dictionary<string, ModId> dictionary;
					HashSet<ModId>.Enumerator enumerator2;
					ModId[] result;
					switch (num)
					{
					default:
						if (!<>w__disposeMode)
						{
							num = (<>1__state = -1);
							HashSet<ModId> hashSet = new HashSet<ModId>();
							IEnumerator<ModId> enumerator = topLevelPackages.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									ModId current = enumerator.Current;
									hashSet.Add(current);
								}
							}
							finally
							{
								if (num == -1)
								{
									enumerator?.Dispose();
								}
							}
							if (!<>w__disposeMode)
							{
								<topLevel>5__2 = hashSet;
								<toProcess>5__3 = new Queue<ModId>(<topLevel>5__2);
								<visited>5__4 = new HashSet<ModId>();
								goto IL_017d;
							}
						}
						goto end_IL_0007;
					case 0:
						awaiter = <>u__1;
						<>u__1 = default(ValueTaskAwaiter<ModId[]>);
						num = (<>1__state = -1);
						goto IL_013e;
					case -4:
						break;
						IL_017d:
						while (<toProcess>5__3.Count > 0)
						{
							modId = <toProcess>5__3.Dequeue();
							if (!<visited>5__4.Add(modId))
							{
								continue;
							}
							goto IL_00cb;
						}
						dictionary = new Dictionary<string, ModId>();
						enumerator2 = <visited>5__4.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								ModId current2 = enumerator2.Current;
								Version version = Version.Parse(current2.Version);
								if (dictionary.TryGetValue(current2.VersionlessId, out var value))
								{
									if (version > Version.Parse(value.Version))
									{
										dictionary[current2.VersionlessId] = current2;
									}
								}
								else
								{
									dictionary[current2.VersionlessId] = current2;
								}
							}
						}
						finally
						{
							if (num == -1)
							{
								((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
							}
						}
						if (!<>w__disposeMode)
						{
							<>7__wrap4 = dictionary.Values.GetEnumerator();
							break;
						}
						goto end_IL_0007;
						IL_013e:
						result = awaiter.GetResult();
						foreach (ModId item in result)
						{
							if (!<visited>5__4.Contains(item))
							{
								<toProcess>5__3.Enqueue(item);
							}
						}
						goto IL_017d;
						IL_00cb:
						<>2__current = null;
						awaiter = getPackageDeps(modId, ct).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = awaiter;
							<ResolveSortedDependencies>d__0 stateMachine = this;
							<>t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
							return;
						}
						goto IL_013e;
					}
					try
					{
						if (num != -4)
						{
							goto IL_028f;
						}
						num = (<>1__state = -1);
						if (!<>w__disposeMode)
						{
							goto IL_028f;
						}
						goto end_IL_023c;
						IL_028f:
						while (<>7__wrap4.MoveNext())
						{
							ModId current3 = <>7__wrap4.Current;
							if (!includeTopLevel && <topLevel>5__2.Contains(current3))
							{
								continue;
							}
							<>2__current = current3;
							num = (<>1__state = -4);
							goto IL_0394;
						}
						end_IL_023c:;
					}
					finally
					{
						if (num == -1)
						{
							((IDisposable)<>7__wrap4/*cast due to .constrained prefix*/).Dispose();
						}
					}
					if (!<>w__disposeMode)
					{
						<>7__wrap4 = default(Dictionary<string, ModId>.ValueCollection.Enumerator);
					}
					end_IL_0007:;
				}
				catch (Exception exception)
				{
					<>1__state = -2;
					<topLevel>5__2 = null;
					<toProcess>5__3 = null;
					<visited>5__4 = null;
					<>7__wrap4 = default(Dictionary<string, ModId>.ValueCollection.Enumerator);
					if (<>x__combinedTokens != null)
					{
						<>x__combinedTokens.Dispose();
						<>x__combinedTokens = null;
					}
					<>2__current = null;
					<>t__builder.Complete();
					<>v__promiseOfValueOrEnd.SetException(exception);
					return;
				}
				<>1__state = -2;
				<topLevel>5__2 = null;
				<toProcess>5__3 = null;
				<visited>5__4 = null;
				<>7__wrap4 = default(Dictionary<string, ModId>.ValueCollection.Enumerator);
				if (<>x__combinedTokens != null)
				{
					<>x__combinedTokens.Dispose();
					<>x__combinedTokens = null;
				}
				<>2__current = null;
				<>t__builder.Complete();
				<>v__promiseOfValueOrEnd.SetResult(result: false);
				return;
				IL_0394:
				<>v__promiseOfValueOrEnd.SetResult(result: true);
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}

			[DebuggerHidden]
			IAsyncEnumerator<ModId> IAsyncEnumerable<ModId>.GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
			{
				<ResolveSortedDependencies>d__0 <ResolveSortedDependencies>d__;
				if (<>1__state == -2 && <>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					<>1__state = -3;
					<>t__builder = AsyncIteratorMethodBuilder.Create();
					<>w__disposeMode = false;
					<ResolveSortedDependencies>d__ = this;
				}
				else
				{
					<ResolveSortedDependencies>d__ = new <ResolveSortedDependencies>d__0(-3);
				}
				<ResolveSortedDependencies>d__.topLevelPackages = <>3__topLevelPackages;
				<ResolveSortedDependencies>d__.includeTopLevel = <>3__includeTopLevel;
				<ResolveSortedDependencies>d__.getPackageDeps = <>3__getPackageDeps;
				if (<>3__ct.Equals(default(CancellationToken)))
				{
					<ResolveSortedDependencies>d__.ct = cancellationToken;
				}
				else if (cancellationToken.Equals(<>3__ct) || cancellationToken.Equals(default(CancellationToken)))
				{
					<ResolveSortedDependencies>d__.ct = <>3__ct;
				}
				else
				{
					<>x__combinedTokens = CancellationTokenSource.CreateLinkedTokenSource(<>3__ct, cancellationToken);
					<ResolveSortedDependencies>d__.ct = <>x__combinedTokens.Token;
				}
				return <ResolveSortedDependencies>d__;
			}

			[DebuggerHidden]
			ValueTask<bool> IAsyncEnumerator<ModId>.MoveNextAsync()
			{
				if (<>1__state == -2)
				{
					return default(ValueTask<bool>);
				}
				<>v__promiseOfValueOrEnd.Reset();
				<ResolveSortedDependencies>d__0 stateMachine = this;
				<>t__builder.MoveNext(ref stateMachine);
				short version = <>v__promiseOfValueOrEnd.Version;
				if (<>v__promiseOfValueOrEnd.GetStatus(version) == ValueTaskSourceStatus.Succeeded)
				{
					return new ValueTask<bool>(<>v__promiseOfValueOrEnd.GetResult(version));
				}
				return new ValueTask<bool>(this, version);
			}

			[DebuggerHidden]
			bool IValueTaskSource<bool>.GetResult(short token)
			{
				return <>v__promiseOfValueOrEnd.GetResult(token);
			}

			[DebuggerHidden]
			ValueTaskSourceStatus IValueTaskSource<bool>.GetStatus(short token)
			{
				return <>v__promiseOfValueOrEnd.GetStatus(token);
			}

			[DebuggerHidden]
			void IValueTaskSource<bool>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
			{
				<>v__promiseOfValueOrEnd.OnCompleted(continuation, state, token, flags);
			}

			[DebuggerHidden]
			void IValueTaskSource.GetResult(short token)
			{
				<>v__promiseOfValueOrEnd.GetResult(token);
			}

			[DebuggerHidden]
			ValueTaskSourceStatus IValueTaskSource.GetStatus(short token)
			{
				return <>v__promiseOfValueOrEnd.GetStatus(token);
			}

			[DebuggerHidden]
			void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
			{
				<>v__promiseOfValueOrEnd.OnCompleted(continuation, state, token, flags);
			}

			[DebuggerHidden]
			ValueTask IAsyncDisposable.DisposeAsync()
			{
				if (<>1__state >= -1)
				{
					throw new NotSupportedException();
				}
				if (<>1__state == -2)
				{
					return default(ValueTask);
				}
				<>w__disposeMode = true;
				<>v__promiseOfValueOrEnd.Reset();
				<ResolveSortedDependencies>d__0 stateMachine = this;
				<>t__builder.MoveNext(ref stateMachine);
				return new ValueTask(this, <>v__promiseOfValueOrEnd.Version);
			}
		}

		[AsyncIteratorStateMachine(typeof(<ResolveSortedDependencies>d__0))]
		public static IAsyncEnumerable<ModId> ResolveSortedDependencies(IEnumerable<ModId> topLevelPackages, bool includeTopLevel, Func<ModId, CancellationToken, ValueTask<ModId[]>> getPackageDeps, [EnumeratorCancellation] CancellationToken ct)
		{
			return new <ResolveSortedDependencies>d__0(-2)
			{
				<>3__topLevelPackages = topLevelPackages,
				<>3__includeTopLevel = includeTopLevel,
				<>3__getPackageDeps = getPackageDeps,
				<>3__ct = ct
			};
		}
	}
	public static class InjectionHelper
	{
		public static T GetServiceFromPartialArgs<T>(this IServiceProvider provider, params object[] args)
		{
			ConstructorInfo constructorInfo = typeof(T).GetConstructors().Single();
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			List<object> list = args.ToList();
			for (int i = args.Length; i < parameters.Length; i++)
			{
				if (parameters[i].GetCustomAttributes(typeof(FromKeyedServicesAttribute), inherit: true).FirstOrDefault() is FromKeyedServicesAttribute fromKeyedServicesAttribute)
				{
					list.Add(provider.GetRequiredKeyedService(parameters[i].ParameterType, fromKeyedServicesAttribute.Key));
				}
				else
				{
					list.Add(provider.GetRequiredService(parameters[i].ParameterType));
				}
			}
			return (T)constructorInfo.Invoke(list.ToArray());
		}
	}
	public static class StringHelpers
	{
		public static string ToDelimiterSeparatedString<T>(this IEnumerable<T> items, string delimiter = ", ")
		{
			return "[" + string.Join(delimiter, items) + "]";
		}
	}
}
namespace ReadyM.Core.Extensions
{
	public static class BlobSasExtensions
	{
		public static async Task<string?> GetBlobSasUri(this BlobContainerClient containerClient, string blobName, CancellationToken ct = default(CancellationToken))
		{
			BlobClient blobClient = containerClient.GetBlobClient(blobName);
			if (!(await blobClient.ExistsAsync(ct)))
			{
				return null;
			}
			BlobSasBuilder blobSasBuilder = new BlobSasBuilder
			{
				BlobContainerName = containerClient.Name,
				BlobName = blobName,
				Resource = "b",
				ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(5.0)
			};
			blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
			string query = blobClient.GenerateSasUri(blobSasBuilder).Query;
			return $"{blobClient.Uri}{query}";
		}

		public static string GetUploadSasUri(this BlobContainerClient containerClient, string blobName, TimeSpan time, string? contentEncoding = null)
		{
			BlobClient blobClient = containerClient.GetBlobClient(blobName);
			BlobSasBuilder blobSasBuilder = new BlobSasBuilder
			{
				BlobContainerName = containerClient.Name,
				BlobName = blobName,
				Resource = "b",
				ExpiresOn = DateTimeOffset.UtcNow.Add(time),
				ContentEncoding = contentEncoding
			};
			blobSasBuilder.SetPermissions(BlobSasPermissions.Write);
			string query = blobClient.GenerateSasUri(blobSasBuilder).Query;
			return $"{blobClient.Uri}{query}";
		}
	}
}
namespace ReadyM.Core.Common
{
	public static class Claims
	{
		public const string Role = "role";

		public const string EarlyAccess = "early_access";

		public const string Cooperative = "co-op";

		public const string XModoId = "xmodo";
	}
	public static class Roles
	{
		public const string Administrator = "Administrator";

		public const string Tester = "Tester";
	}
}
