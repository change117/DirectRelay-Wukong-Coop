using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.IO.Pipes;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using I18N.Avalonia.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using ReadyM.Launcher.Core.Api.Api;
using ReadyM.Launcher.Core.Api.Client;
using ReadyM.Launcher.Core.Api.Model;
using ReadyM.Launcher.Core.Common;
using ReadyM.Launcher.Core.Common.HostingPlatforms;
using ReadyM.Launcher.Core.Downloaders;
using ReadyM.Launcher.Core.Games;
using ReadyM.Launcher.Core.Identity;
using ReadyM.Launcher.Core.Insights;
using ReadyM.Launcher.Core.Installers.Manager;
using ReadyM.Launcher.Core.Protocol;
using ReadyM.Launcher.Core.Regions;
using ReadyM.Launcher.Core.Settings;
using ReadyM.Launcher.Core.UI;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETCoreApp,Version=v10.0", FrameworkDisplayName = ".NET 10.0")]
[assembly: AssemblyCompany("ReadyM.Launcher.Core")]
[assembly: AssemblyConfiguration("Production")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0+687606eb3d93ad26ad1604802700916eab9ca5dd")]
[assembly: AssemblyProduct("ReadyM.Launcher.Core")]
[assembly: AssemblyTitle("ReadyM.Launcher.Core")]
[assembly: TargetPlatform("Windows7.0")]
[assembly: SupportedOSPlatform("Windows7.0")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace ReadyM.Launcher.Core
{
	public class AppConfiguration
	{
		public string AlphaTestingLink => "https://discord.gg/kUf6z3zvma";

		public string FeedbackLink => "https://discord.gg/jv56Dpp9x9";

		public string PlayerPortalLink => "https://portal.ready.mp/dashboard";

		public string LokiHost => "logs-prod-036.grafana.net";

		public string DiscordAuthScopes => "identify email";

		public string MasterServerUrl => "https://api.ready.mp/";

		public string UpdateServerUrl => "https://readymstorage.blob.core.windows.net/releases";

		public string DiscordClientId => "1384197940238749826";

		public string LokiUser => "1264775";

		public string LokiAccessToken => "REDACTED_GRAFANA_TOKEN";

		public string PrometheusUser => "1306981";

		public string PrometheusToken => "REDACTED_GRAFANA_TOKEN";
	}
}
namespace ReadyM.Launcher.Core.UI
{
	public interface INotificationDisplay
	{
		event Action<NotificationType, string, Action?> OnNotificationRequested;

		void ShowNotification(NotificationType type, string message, Action? onClick = null);

		void ShowResponseNotification(IApiResponse response, string? okMessage = null);
	}
	public interface IViewSwitcher
	{
		void SwitchToView(ViewType viewType);

		void ShowErrorView(string errorMessage);
	}
	public enum ViewType
	{
		Login,
		GameSelection,
		Coop,
		Pvp,
		LinkWithXModo,
		XModoError
	}
}
namespace ReadyM.Launcher.Core.Settings
{
	public interface ISettingsManager
	{
		LocalSettings ReadSettingsFromDisk();

		void WriteSettingsToDisk(LocalSettings settings);
	}
	public class LocalSettings
	{
		public Dictionary<Game, string> GameInstallationFolderOverrides { get; init; } = new Dictionary<Game, string>();

		public string? ISOLanguageCode { get; set; }
	}
	public class SettingsManager(ILogger<SettingsManager> logger) : ISettingsManager
	{
		private readonly string _settingsPath = Path.Combine(StorageHelper.GetPersistentStoragePath(), "settings.json");

		public LocalSettings ReadSettingsFromDisk()
		{
			if (!File.Exists(_settingsPath))
			{
				return new LocalSettings();
			}
			try
			{
				return JsonSerializer.Deserialize<LocalSettings>(File.ReadAllText(_settingsPath)) ?? new LocalSettings();
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Failed to read settings from disk");
				return new LocalSettings();
			}
		}

		public void WriteSettingsToDisk(LocalSettings settings)
		{
			try
			{
				string contents = JsonSerializer.Serialize(settings, new JsonSerializerOptions
				{
					WriteIndented = true
				});
				File.WriteAllText(_settingsPath, contents);
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Failed to write settings to disk");
			}
		}
	}
}
namespace ReadyM.Launcher.Core.Security
{
	[SupportedOSPlatform("Windows")]
	public static class FastWmi
	{
		public static string FirstOrEmpty(string wql, params string[] props)
		{
			try
			{
				ManagementScope scope = new ManagementScope("\\\\.\\root\\cimv2");
				ObjectQuery query = new ObjectQuery(wql);
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query);
				try
				{
					managementObjectSearcher.Options.ReturnImmediately = true;
					managementObjectSearcher.Options.Rewindable = false;
					using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator();
					if (managementObjectEnumerator.MoveNext())
					{
						ManagementBaseObject current = managementObjectEnumerator.Current;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (string propertyName in props)
						{
							object obj = current.Properties[propertyName]?.Value;
							if (obj != null)
							{
								stringBuilder.Append(obj);
							}
						}
						return stringBuilder.ToString();
					}
				}
				finally
				{
					((IDisposable)managementObjectSearcher)?.Dispose();
				}
			}
			catch
			{
			}
			return "";
		}

		public static string CpuId()
		{
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("\\\\.\\root\\cimv2", "SELECT UniqueId, ProcessorId, Name, Manufacturer, MaxClockSpeed FROM Win32_Processor");
				try
				{
					managementObjectSearcher.Options.ReturnImmediately = true;
					managementObjectSearcher.Options.Rewindable = false;
					using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator();
					ManagementBaseObject mo;
					if (managementObjectEnumerator.MoveNext())
					{
						mo = managementObjectEnumerator.Current;
						string text = Get("UniqueId");
						if (!string.IsNullOrWhiteSpace(text))
						{
							return text;
						}
						string text2 = Get("ProcessorId");
						if (!string.IsNullOrWhiteSpace(text2))
						{
							return text2;
						}
						string text3 = Get("Name");
						string text4 = Get("Manufacturer");
						string text5 = Get("MaxClockSpeed");
						return ((!string.IsNullOrWhiteSpace(text3)) ? text3 : text4) + text5;
					}
					string Get(string k)
					{
						return mo.Properties[k]?.Value?.ToString() ?? "";
					}
				}
				finally
				{
					((IDisposable)managementObjectSearcher)?.Dispose();
				}
			}
			catch
			{
			}
			return "";
		}
	}
	public static class Helpers
	{
		public static string GetHash(string s)
		{
			return GetHexString(MD5.HashData(new ASCIIEncoding().GetBytes(s)));
		}

		private static string GetHexString(byte[] bt)
		{
			string text = string.Empty;
			for (int i = 0; i < bt.Length; i++)
			{
				byte num = bt[i];
				int num2 = num & 0xF;
				int num3 = (num >> 4) & 0xF;
				text = ((num3 <= 9) ? (text + num3) : (text + (char)(num3 - 10 + 65)));
				text = ((num2 <= 9) ? (text + num2) : (text + (char)(num2 - 10 + 65)));
				if (i + 1 != bt.Length && (i + 1) % 2 == 0)
				{
					text += "-";
				}
			}
			return text;
		}
	}
	public interface IFingerprintProvider
	{
		string GetDeviceFingerprint();
	}
	public class VpnCheck(AuthManager authManager, RegionPinger regionPinger, ILogger<VpnCheck> logger)
	{
		private record HttpProbeResult(string Url, bool Success, int? StatusCode, string Error, long ElapsedMs);

		private class NicReport
		{
			public string Name { get; set; }

			public string Description { get; set; }

			public string Type { get; set; }

			public OperationalStatus Status { get; set; }

			public List<string> IPv4 { get; set; } = new List<string>();

			public List<string> IPv6 { get; set; } = new List<string>();

			public List<string> Gateways { get; set; } = new List<string>();

			public List<string> Dns { get; set; } = new List<string>();

			public double VpnScore { get; set; }

			public bool VpnCandidate { get; set; }

			public List<string> VpnReasons { get; set; } = new List<string>();
		}

		private static readonly string[] GamingHttpsProbeUrls = new string[3] { "https://store.steampowered.com/", "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1", "https://steamcommunity.com/" };

		private static readonly string[] VpnKeywords = new string[19]
		{
			"vpn", "tap", "tun", "openvpn", "wireguard", "tailscale", "wg", "softether", "cisco", "fortinet",
			"globalprotect", "nord", "expressvpn", "protonvpn", "vpnc", "ipsec", "sstp", "l2tp", "pptp"
		};

		private const int HttpTimeoutMs = 30000;

		private static (string tzId, int offsetMinutes) GetSystemTimeZone()
		{
			TimeZoneInfo local = TimeZoneInfo.Local;
			return new ValueTuple<string, int>(item2: (int)DateTimeOffset.Now.Offset.TotalMinutes, item1: local.Id);
		}

		private static (string culture, string uiCulture) GetSystemCultures()
		{
			string name = CultureInfo.CurrentCulture.Name;
			string name2 = CultureInfo.CurrentUICulture.Name;
			return (culture: name, uiCulture: name2);
		}

		public async Task ReportNetworkConditions(CancellationToken ct = default(CancellationToken))
		{
			(string, int) systemTimeZone = GetSystemTimeZone();
			string tzId = systemTimeZone.Item1;
			int tzOffsetMin = systemTimeZone.Item2;
			(string, string) systemCultures = GetSystemCultures();
			string culture = systemCultures.Item1;
			string uiCulture = systemCultures.Item2;
			Task<List<HttpProbeResult>> gamingResultsTask = ProbeManyHttpAsync(GamingHttpsProbeUrls, 30000);
			Task<RegionPingResult> readyMResultsTask = regionPinger.PingRegionsAsync(ct);
			global::<>y__InlineArray2<Task> buffer = default(global::<>y__InlineArray2<Task>);
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray2<Task>, Task>(ref buffer, 0) = gamingResultsTask;
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray2<Task>, Task>(ref buffer, 1) = readyMResultsTask;
			await Task.WhenAll(global::<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<global::<>y__InlineArray2<Task>, Task>(in buffer, 2));
			List<HttpProbeResult> result = gamingResultsTask.Result;
			ReadyM.Launcher.Core.Regions.RegionInfo[] regions = readyMResultsTask.Result.Regions;
			List<NicReport> interfaceReports = GetInterfaceReports();
			string text = JsonSerializer.Serialize(new
			{
				systemTimeZoneId = tzId,
				systemTimeZoneOffsetMinutes = tzOffsetMin,
				systemCulture = culture,
				systemUICulture = uiCulture,
				userId = authManager.UserId,
				gamingResults = result,
				readyMResults = regions,
				maybeVpnInterfaces = (from x in interfaceReports
					where x != null && x.VpnCandidate && x.Status == OperationalStatus.Up
					select new { x.Name, x.Description, x.Dns, x.IPv4, x.Type, x.Gateways, x.VpnReasons }).ToArray()
			}, new JsonSerializerOptions
			{
				WriteIndented = false
			});
			logger.LogDebug("VPN Check Networking Report: {ReportJson}", text);
			using HttpClient http = new HttpClient();
			using StringContent content = new StringContent(text, Encoding.UTF8, "application/json");
			_ = 1;
			try
			{
				using HttpResponseMessage httpResponseMessage = await http.PostAsync("https://n8n.readycode.io/webhook/3ad5133e-a19b-48bb-97bd-f99d00f8d24e", content, ct);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					logger.LogDebug("VPN Check networking report sent successfully.");
					return;
				}
				logger.LogWarning("VPN Check networking report failed with status code {StatusCode}.", (int)httpResponseMessage.StatusCode);
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "VPN Check networking report failed with exception.");
			}
		}

		private async Task<HttpProbeResult> ProbeHttpAsync(string url, int timeoutMs)
		{
			Stopwatch sw = Stopwatch.StartNew();
			using CancellationTokenSource cts = new CancellationTokenSource(timeoutMs);
			try
			{
				using HttpClientHandler handler = new HttpClientHandler();
				handler.AllowAutoRedirect = true;
				using HttpClient http = new HttpClient(handler);
				http.Timeout = TimeSpan.FromMilliseconds(timeoutMs);
				using HttpResponseMessage httpResponseMessage = await http.GetAsync(url, cts.Token);
				sw.Stop();
				return new HttpProbeResult(url, httpResponseMessage.IsSuccessStatusCode || httpResponseMessage.StatusCode == HttpStatusCode.NoContent, (int)httpResponseMessage.StatusCode, null, sw.ElapsedMilliseconds);
			}
			catch (HttpRequestException ex)
			{
				sw.Stop();
				string error = ((ex.InnerException is SocketException ex2) ? $"SocketError={ex2.SocketErrorCode}" : ex.Message);
				return new HttpProbeResult(url, Success: false, null, error, sw.ElapsedMilliseconds);
			}
			catch (TaskCanceledException)
			{
				sw.Stop();
				return new HttpProbeResult(url, Success: false, null, "Timeout", sw.ElapsedMilliseconds);
			}
			catch (Exception ex4)
			{
				sw.Stop();
				return new HttpProbeResult(url, Success: false, null, ex4.GetType().Name + ": " + ex4.Message, sw.ElapsedMilliseconds);
			}
		}

		private async Task<List<HttpProbeResult>> ProbeManyHttpAsync(IEnumerable<string> urls, int timeoutMs)
		{
			List<HttpProbeResult> results = new List<HttpProbeResult>();
			foreach (string url in urls)
			{
				List<HttpProbeResult> list = results;
				list.Add(await ProbeHttpAsync(url, timeoutMs));
			}
			return results;
		}

		private List<NicReport> GetInterfaceReports()
		{
			List<NicReport> list = new List<NicReport>();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
				NicReport nicReport = new NicReport
				{
					Name = networkInterface.Name,
					Description = networkInterface.Description,
					Type = networkInterface.NetworkInterfaceType.ToString(),
					Status = networkInterface.OperationalStatus,
					IPv4 = (from a in iPProperties.UnicastAddresses
						where a.Address.AddressFamily == AddressFamily.InterNetwork
						select a.Address.ToString()).ToList(),
					IPv6 = (from a in iPProperties.UnicastAddresses
						where a.Address.AddressFamily == AddressFamily.InterNetworkV6
						select a.Address.ToString()).ToList(),
					Gateways = iPProperties.GatewayAddresses.Select((GatewayIPAddressInformation g) => g.Address.ToString()).ToList(),
					Dns = iPProperties.DnsAddresses.Select((IPAddress d) => d.ToString()).ToList()
				};
				double num = 0.0;
				List<string> list2 = new List<string>();
				string lowered = (networkInterface.Name + " " + networkInterface.Description).ToLowerInvariant();
				NetworkInterfaceType networkInterfaceType = networkInterface.NetworkInterfaceType;
				if ((networkInterfaceType == NetworkInterfaceType.Ppp || networkInterfaceType == NetworkInterfaceType.Tunnel) ? true : false)
				{
					num += 0.5;
					list2.Add($"Type={networkInterface.NetworkInterfaceType}");
				}
				if (VpnKeywords.Any((string k) => lowered.Contains(k)))
				{
					num += 0.35;
					list2.Add("Name/description contains VPN keyword");
				}
				if (lowered.Contains("virtual") || lowered.Contains("adapter"))
				{
					num += 0.1;
					list2.Add("Virtual/adapter hint");
				}
				bool flag = nicReport.Gateways.Any();
				if (flag)
				{
					networkInterfaceType = networkInterface.NetworkInterfaceType;
					bool flag2 = ((networkInterfaceType == NetworkInterfaceType.Ppp || networkInterfaceType == NetworkInterfaceType.Tunnel) ? true : false);
					flag = flag2;
				}
				if (flag)
				{
					num += 0.1;
					list2.Add("Gateway on tunnel/PPP");
				}
				nicReport.VpnScore = Math.Min(1.0, num);
				nicReport.VpnCandidate = nicReport.VpnScore >= 0.35;
				nicReport.VpnReasons = list2;
				list.Add(nicReport);
			}
			return list;
		}
	}
	[SupportedOSPlatform("Windows")]
	public class WindowsFingerprintProvider : IFingerprintProvider
	{
		private static string _fingerPrint = string.Empty;

		public string GetDeviceFingerprint()
		{
			return GetDeviceFingerprintStatic();
		}

		public static string GetDeviceFingerprintStatic()
		{
			if (string.IsNullOrEmpty(_fingerPrint))
			{
				string value = FastWmi.CpuId();
				string value2 = FastWmi.FirstOrEmpty("SELECT Manufacturer, SMBIOSBIOSVersion, IdentificationCode, SerialNumber, ReleaseDate, Version FROM Win32_BIOS", "Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode", "SerialNumber", "ReleaseDate", "Version");
				string value3 = FastWmi.FirstOrEmpty("SELECT Model, Manufacturer, Name, SerialNumber FROM Win32_BaseBoard", "Model", "Manufacturer", "Name", "SerialNumber");
				_fingerPrint = Helpers.GetHash($"CPU >> {value}\nBIOS >> {value2}\nBASE >> {value3}");
			}
			return _fingerPrint;
		}
	}
}
namespace ReadyM.Launcher.Core.Regions
{
	public readonly record struct RegionInfo(Region Region, long PingMs, bool Available);
	public readonly record struct RegionPingResult(RegionInfo[] Regions, bool Success);
	public sealed class RegionPinger(IMatchmakingApi matchmakingApi, AuthManager authManager, ILogger<RegionPinger> logger)
	{
		private const int CancelAfterMs = 999;

		private RegionInfo[] _cachedRegions = Array.Empty<RegionInfo>();

		private readonly string _appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public bool ShouldDisplayXModoOnly
		{
			get
			{
				AuthManager authManager = authManager;
				if (authManager != null && authManager.LaunchedThroughXModo)
				{
					return authManager.LinkedWithXModo;
				}
				return false;
			}
		}

		public async Task<RegionPingResult> GetCachedRegionsAsync(CancellationToken ct = default(CancellationToken))
		{
			if (_cachedRegions.Length != 0)
			{
				RegionInfo[] regions = _cachedRegions.Select(delegate(RegionInfo x)
				{
					int num;
					if (x.Region == Region.XModo)
					{
						AuthManager authManager = authManager;
						num = ((authManager != null && authManager.LaunchedThroughXModo && authManager.LinkedWithXModo) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					bool available = (byte)num != 0;
					return x with
					{
						Available = available
					};
				}).ToArray();
				return new RegionPingResult(regions, Success: true);
			}
			return await PingRegionsAsync(ct);
		}

		public async Task<RegionPingResult> PingRegionsAsync(CancellationToken ct = default(CancellationToken))
		{
			_ = 1;
			try
			{
				if (!(await matchmakingApi.RegionsAsync("udp", _appVersion, ct)).TryOk(out List<RegionDto> result))
				{
					return new RegionPingResult(_cachedRegions, _cachedRegions.Length != 0);
				}
				List<Task<RegionInfo>> list = new List<Task<RegionInfo>>();
				foreach (RegionDto item in result)
				{
					Task<RegionInfo>[] array = new Task<RegionInfo>[3];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = PingRegion(item, ct);
					}
					list.Add(Task.WhenAny(array).Unwrap());
				}
				_cachedRegions = await Task.WhenAll(list);
				MemoryExtensions.Sort(_cachedRegions, (RegionInfo x, RegionInfo y) => x.PingMs.CompareTo(y.PingMs));
				return new RegionPingResult(_cachedRegions, Success: true);
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Failed to ping regions");
				return new RegionPingResult(_cachedRegions, Success: false);
			}
		}

		private async Task<RegionInfo> PingRegion(RegionDto regionDto, CancellationToken externalCt)
		{
			using CancellationTokenSource timedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
			timedCts.CancelAfter(999);
			CancellationToken ct = timedCts.Token;
			byte[] payload = new byte[10];
			Random.Shared.NextBytes(payload);
			string[] array = regionDto.PingUrl.Split(':');
			string hostname = array[0];
			int port = int.Parse(array[1]);
			using UdpClient udpClient = new UdpClient();
			long elapsed = 999L;
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				await udpClient.SendAsync(payload, hostname, port, ct);
				if (((ReadOnlySpan<byte>)(await udpClient.ReceiveAsync(ct)).Buffer).SequenceEqual((ReadOnlySpan<byte>)payload))
				{
					stopwatch.Stop();
					elapsed = stopwatch.ElapsedMilliseconds;
				}
				else
				{
					logger.LogWarning("Invalid ping response from region {Region} at {Endpoint}:{Port}", regionDto.Region, hostname, port);
				}
			}
			catch (OperationCanceledException exception)
			{
				logger.LogDebug(exception, "Ping to region {Region} at {Endpoint}:{Port} timed out", regionDto.Region, hostname, port);
			}
			catch (Exception exception2)
			{
				logger.LogError(exception2, "Exception while pinging region {Region} at {Endpoint}:{Port}", regionDto.Region, hostname, port);
			}
			int num;
			if (regionDto.Region == Region.XModo)
			{
				AuthManager authManager = authManager;
				num = ((authManager != null && authManager.LaunchedThroughXModo && authManager.LinkedWithXModo) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool available = (byte)num != 0;
			return new RegionInfo(regionDto.Region, elapsed, available);
		}
	}
}
namespace ReadyM.Launcher.Core.Protocol
{
	public static class Constants
	{
		public const string ProtocolScheme = "readym";

		public const string OAuth2ProtocolHost = "oauth2";

		public const string XModoProtocolHost = "xmodo";

		public const string WindowFocusHost = "focus";

		public const string HeadlessStartHost = "headless";

		public const string LogOutHost = "logout";

		public const string ApplicationCloseHost = "close";

		public const string WindowHideHost = "hide";

		public const string DiscordOAuth2ProtocolUrl = "readym://oauth2/discord";

		public const string IpcPipeName = "ReadyM-Pipe";

		public const string XModoRestartHost = "auto-update-xmodo-restart";

		public static Uri UrlForHost(string host)
		{
			return new Uri("readym://" + host);
		}
	}
	public interface IIpcServer
	{
		void RegisterHandler(string host, Func<Uri, CancellationToken, Task> handler);
	}
	public class IpcClient(Action<string> log)
	{
		public async Task ParseAndSendProtocolMessage(Uri uri, CancellationToken ct = default(CancellationToken))
		{
			await using NamedPipeClientStream pipe = new NamedPipeClientStream(".", "ReadyM-Pipe", PipeDirection.Out);
			_ = 3;
			try
			{
				await pipe.ConnectAsync(3000, ct);
				await using StreamWriter writer = new StreamWriter(pipe);
				await writer.WriteLineAsync(uri.ToString());
				await writer.FlushAsync(ct);
			}
			catch (TimeoutException)
			{
				log("Failed to connect to the existing instance of the launcher via IPC pipe: Timeout");
			}
		}
	}
	public sealed class IpcServer(ILogger<IpcServer> logger) : BackgroundService(), IIpcServer
	{
		public static Uri? LaunchCommand;

		private readonly ConcurrentDictionary<string, Func<Uri, CancellationToken, Task>> _handlers = new ConcurrentDictionary<string, Func<Uri, CancellationToken, Task>>();

		private readonly SemaphoreSlim _handlerConcurrency = new SemaphoreSlim(4, 4);

		public void RegisterHandler(string host, Func<Uri, CancellationToken, Task> handler)
		{
			ArgumentNullException.ThrowIfNull(handler, "handler");
			if (string.IsNullOrWhiteSpace(host))
			{
				throw new ArgumentException("Host is required.", "host");
			}
			if (!_handlers.TryAdd(host, handler))
			{
				throw new InvalidOperationException("Handler already registered for host '" + host + "'.");
			}
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("IPC server starting (pipe: {PipeName})", "ReadyM-Pipe");
			if ((object)LaunchCommand != null)
			{
				logger.LogInformation("Processing launch command URI received before server start: {Scheme}://{Host}", LaunchCommand.Scheme, LaunchCommand.Host);
				Dispatch(LaunchCommand, stoppingToken);
			}
			while (!stoppingToken.IsCancellationRequested)
			{
				await using (NamedPipeServerStream pipe = CreateServerPipe())
				{
					try
					{
						await pipe.WaitForConnectionAsync(stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
						logger.LogDebug("IPC client connected.");
						await HandleClientAsync(pipe, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
					{
						goto end_IL_00af;
					}
					catch (Exception exception)
					{
						logger.LogError(exception, "IPC server loop error.");
						try
						{
							await Task.Delay(250, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						catch
						{
						}
					}
					continue;
					end_IL_00af:;
				}
				break;
			}
			logger.LogInformation("IPC server stopping.");
		}

		private static NamedPipeServerStream CreateServerPipe()
		{
			return new NamedPipeServerStream("ReadyM-Pipe", PipeDirection.In, -1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
		}

		private async Task HandleClientAsync(NamedPipeServerStream pipe, CancellationToken ct)
		{
			using StreamReader reader = new StreamReader(pipe, null, detectEncodingFromByteOrderMarks: true, -1, leaveOpen: true);
			while (!ct.IsCancellationRequested && pipe.IsConnected)
			{
				string text;
				try
				{
					text = await reader.ReadLineAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException) when (ct.IsCancellationRequested)
				{
					break;
				}
				if (text == null)
				{
					logger.LogDebug("IPC client disconnected.");
					break;
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (!Uri.TryCreate(text, UriKind.Absolute, out Uri result))
					{
						logger.LogWarning("Invalid URI received over IPC: {Line}", text);
					}
					else
					{
						Dispatch(result, ct);
					}
				}
			}
		}

		private void Dispatch(Uri uri, CancellationToken ct)
		{
			if (!_handlers.TryGetValue(uri.Host, out Func<Uri, CancellationToken, Task> value))
			{
				logger.LogWarning("No handler registered for host: {Host}", uri.Host);
				return;
			}
			logger.LogInformation("Dispatching IPC URI: {Scheme}://{Host}", uri.Scheme, uri.Host);
			RunHandlerAsync(value, uri, ct);
		}

		private async Task RunHandlerAsync(Func<Uri, CancellationToken, Task> handler, Uri uri, CancellationToken ct)
		{
			await _handlerConcurrency.WaitAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await handler(uri, ct).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException) when (ct.IsCancellationRequested)
			{
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "IPC handler failed for URI: {Uri}", uri);
			}
			finally
			{
				_handlerConcurrency.Release();
			}
		}
	}
}
namespace ReadyM.Launcher.Core.Launchers
{
	public class BlackMythWukongLauncher(IGameHostingPlatformManager gameHostingPlatformHelper, WukongLogUploader wukongLogUploader, INotificationDisplay notificationDisplay, ILogger<BlackMythWukongLauncher> logger) : IGameLauncher
	{
		public Task LaunchVanillaGameAsync(GameInfo game, Dictionary<string, string> data, CancellationToken ct = default(CancellationToken))
		{
			Task.Run(() => wukongLogUploader.UploadLogsAsync(ct), ct);
			logger.LogInformation("Launching game {Name} with data {Args}", game.Name, data);
			string gameInstallFolder = gameHostingPlatformHelper.GetGameInstallFolder(game.Game);
			global::<>y__InlineArray5<string> buffer = default(global::<>y__InlineArray5<string>);
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 0) = gameInstallFolder;
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 1) = "b1";
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 2) = "Binaries";
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 3) = "Win64";
			global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 4) = "b1-Win64-Shipping.exe";
			string text = Path.Combine(global::<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<global::<>y__InlineArray5<string>, string>(in buffer, 5));
			if (!File.Exists(text))
			{
				notificationDisplay.ShowNotification(NotificationType.Error, "Game executable not found: " + text);
				return Task.CompletedTask;
			}
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = text,
				WorkingDirectory = gameInstallFolder,
				UseShellExecute = true
			};
			WriteHandshakeFile(data);
			Process process = Process.Start(startInfo);
			if (process == null)
			{
				logger.LogError("Failed to start process for game {Name}", game.Name);
				return Task.CompletedTask;
			}
			WindowFocusHelper.BringMainWindowToFront(process.ProcessName);
			Task.Run(async delegate
			{
				await process.WaitForExitAsync(ct);
				logger.LogInformation("Game {Name} exited with code {ExitCode}", game.Name, process.ExitCode);
				await wukongLogUploader.UploadLogsAsync(ct);
			}, ct);
			return Task.CompletedTask;
		}

		public string WriteHandshakeFile(Dictionary<string, string> data)
		{
			string persistentStoragePath = StorageHelper.GetPersistentStoragePath();
			int processId = Environment.ProcessId;
			string text = Path.Combine(persistentStoragePath, "wukong_handshake.env");
			logger.LogInformation("Writing handshake file to {FilePath} with PID {Pid}", text, processId);
			using StreamWriter streamWriter = new StreamWriter(text, append: false);
			streamWriter.WriteLine($"LAUNCHER_PID={processId}");
			foreach (KeyValuePair<string, string> datum in data)
			{
				streamWriter.WriteLine(datum.Key + "=" + datum.Value);
			}
			return text;
		}

		public async Task LaunchVanillaGameAsync(GameInfo game, CancellationToken ct = default(CancellationToken))
		{
			await LaunchVanillaGameAsync(game, new Dictionary<string, string>(), ct);
		}

		public async Task LaunchGameProfileAsync(GameInfo game, string profileFolder, CancellationToken ct = default(CancellationToken))
		{
			await LaunchVanillaGameAsync(game, ct);
		}
	}
	public interface IGameLauncher
	{
		Task LaunchVanillaGameAsync(GameInfo game, Dictionary<string, string> envVars, CancellationToken ct = default(CancellationToken));

		Task LaunchGameProfileAsync(GameInfo game, string profilePath, CancellationToken ct = default(CancellationToken));
	}
	public static class WindowFocusHelper
	{
		private enum ShowWindowEnum
		{
			Hide = 0,
			ShowNormal = 1,
			ShowMinimized = 2,
			ShowMaximized = 3,
			Maximize = 3,
			ShowNormalNoActivate = 4,
			Show = 5,
			Minimize = 6,
			ShowMinNoActivate = 7,
			ShowNoActivate = 8,
			Restore = 9,
			ShowDefault = 10,
			ForceMinimized = 11
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShowWindow(nint hWnd, ShowWindowEnum flags);

		[DllImport("user32.dll")]
		private static extern int SetForegroundWindow(nint hwnd);

		public static void BringMainWindowToFront(string processName)
		{
			Task.Run(async delegate
			{
				for (int i = 0; i < 600; i++)
				{
					if (TryBringMainWindowToFront(processName))
					{
						break;
					}
					await Task.Delay(500);
				}
			});
		}

		private static bool TryBringMainWindowToFront(string processName)
		{
			Process process = Process.GetProcessesByName(processName).FirstOrDefault((Process p) => p.MainWindowHandle != IntPtr.Zero);
			if (process != null)
			{
				ShowWindow(process.MainWindowHandle, ShowWindowEnum.Restore);
				return SetForegroundWindow(process.MainWindowHandle) != 0;
			}
			return false;
		}
	}
}
namespace ReadyM.Launcher.Core.Installers
{
	public interface IProfileInstallationStep
	{
		Task InstallProfileAsync(GameInfo gameInfo, ProfileInstallationOptions options, CancellationToken ct = default(CancellationToken));
	}
}
namespace ReadyM.Launcher.Core.Installers.Manager
{
	public interface IProfileInstallationPipeline
	{
		Task<bool> InstallAsync(GameInfo game, ProfileInstallationOptions options, CancellationToken ct = default(CancellationToken));
	}
	public class ProfileInstallationException : Exception
	{
		public ProfileInstallationException(string message)
			: base(message)
		{
		}
	}
	public class ProfileInstallationOptions
	{
		public required string ModVersion { get; init; }

		public required string LoaderVersion { get; init; }

		public required string ProfileFolder { get; init; }

		public required IProgress<double> OnModProgress { get; init; }

		public required IProgress<double> OnLoaderProgress { get; init; }
	}
	public class ProfileInstallationPipeline(IProfileInstallationStep[] steps, INotificationDisplay notifications, ILocalizer localizer, ILogger<ProfileInstallationPipeline> logger) : IProfileInstallationPipeline
	{
		public async Task<bool> InstallAsync(GameInfo game, ProfileInstallationOptions options, CancellationToken ct = default(CancellationToken))
		{
			Directory.CreateDirectory(options.ProfileFolder);
			try
			{
				IProfileInstallationStep[] array = steps;
				for (int i = 0; i < array.Length; i++)
				{
					await array[i].InstallProfileAsync(game, options, ct);
				}
			}
			catch (IOException exception)
			{
				notifications.ShowNotification(NotificationType.Warning, localizer.GetValueFromCulture("InstallationFailedCheckGameRunning"));
				logger.LogWarning(exception, "Installation failed for {Game} server at {Path}", game.Name, options.ProfileFolder);
				return true;
			}
			catch (UnauthorizedAccessException exception2)
			{
				notifications.ShowNotification(NotificationType.Warning, localizer.GetValueFromCulture("InstallationFailedCheckGameRunning"));
				logger.LogWarning(exception2, "Installation failed for {Game} server at {Path}", game.Name, options.ProfileFolder);
				return true;
			}
			catch (ProfileInstallationException ex)
			{
				notifications.ShowNotification(NotificationType.Error, ex.Message);
				logger.LogError(ex, "Installation failed for {Game} server at {Path}", game.Name, options.ProfileFolder);
				return true;
			}
			logger.LogInformation("Profile installed for {Game} server at {Path}", game.Name, options.ProfileFolder);
			return false;
		}
	}
}
namespace ReadyM.Launcher.Core.Installers.CSharpLoader
{
	public class WukongMPUnpackingStep(IGameHostingPlatformManager gamePlatformManager, IModsApi modsApi, ProgressCallbackDownloader downloader, ILogger<WukongMPUnpackingStep> logger) : IProfileInstallationStep
	{
		public async Task InstallProfileAsync(GameInfo gameInfo, ProfileInstallationOptions options, CancellationToken ct = default(CancellationToken))
		{
			string gameInstallFolder = gamePlatformManager.GetGameInstallFolder(gameInfo.Game);
			logger.LogInformation("Game install folder: {GameDir}", gameInstallFolder);
			if (gameInstallFolder == null)
			{
				throw new ProfileInstallationException("Couldn't find " + gameInfo.Name + " installation folder.");
			}
			string b1Dir = Path.Combine(gameInstallFolder, "b1");
			string loaderPath = Path.Combine(options.ProfileFolder, "CSharpLoader");
			string modsPath = Path.Combine(loaderPath, "Mods");
			IOk<WukongFilesVersionResponse> ok = await GetWukongFilesResponse(options, ct);
			if (!ok.TryOk(out WukongFilesVersionResponse resourceResponse))
			{
				if (ok.StatusCode == HttpStatusCode.Forbidden)
				{
					throw new ProfileInstallationException("Your account is not on the early access list. If you think this is a mistake, contact us.");
				}
				throw new ProfileInstallationException($"Couldn't get resources: code {ok.StatusCode}.");
			}
			string extractPath = Path.Combine(StorageHelper.GetDownloadCachePath(), "Loader", options.LoaderVersion);
			if (!Directory.Exists(extractPath) || Directory.GetFileSystemEntries(extractPath).Length == 0)
			{
				logger.LogInformation("CSharpLoader version {Version} not found in cache, downloading", options.LoaderVersion);
				await DownloadAndExtractAsync(extractPath, resourceResponse.LoaderLink, options.OnLoaderProgress, ct);
			}
			else
			{
				logger.LogInformation("CSharpLoader version {Version} already downloaded, skipping", options.LoaderVersion);
			}
			string sourceFolder = Path.Combine(extractPath, "@GAME");
			string profileContent = Path.Combine(extractPath, "@APPDATA");
			await FileHelpers.CopyDirectoryAsync(sourceFolder, b1Dir, ct);
			await FileHelpers.CopyDirectoryAsync(profileContent, options.ProfileFolder, ct);
			profileContent = Path.Combine(StorageHelper.GetDownloadCachePath(), "Mods", "WukongMP", options.ModVersion);
			if (!Directory.Exists(profileContent) || Directory.GetFileSystemEntries(profileContent).Length == 0)
			{
				logger.LogInformation("{ModName} version {Version} not found in cache, downloading", "WukongMP", options.ModVersion);
				await DownloadAndExtractAsync(profileContent, resourceResponse.ModLink, options.OnModProgress, ct);
			}
			else
			{
				logger.LogInformation("{ModName} version {Version} already downloaded, skipping", "WukongMP", options.ModVersion);
			}
			if (Directory.Exists(modsPath))
			{
				Directory.Delete(modsPath, recursive: true);
			}
			await FileHelpers.CopyDirectoryAsync(profileContent, loaderPath, ct);
		}

		private async Task<IOk<WukongFilesVersionResponse?>> GetWukongFilesResponse(ProfileInstallationOptions options, CancellationToken ct)
		{
			return await modsApi.GetWukongMpModFilesAsync(options.LoaderVersion, options.ModVersion, ct);
		}

		private async Task DownloadAndExtractAsync(string extractFolder, string downloadLink, IProgress<double> progress, CancellationToken ct)
		{
			Directory.CreateDirectory(extractFolder);
			string zipPath = Path.GetTempFileName();
			await downloader.DownloadFileAsync(downloadLink, zipPath, progress, ct);
			await FileHelpers.Extract7ZAsync(zipPath, extractFolder, ct);
			try
			{
				File.Delete(zipPath);
			}
			catch (Exception exception)
			{
				logger.LogWarning(exception, "Failed to delete temporary file {ZipPath}", zipPath);
			}
		}
	}
}
namespace ReadyM.Launcher.Core.Insights
{
	public static class Telemetry
	{
		public static readonly ActivitySource ActivitySource = new ActivitySource("ReadyM.Launcher");

		public static readonly Meter Meter = new Meter("ReadyM.Launcher");

		public static string? UserGuid;
	}
	public class WukongLogUploader
	{
		private readonly string[] _logDirectories = Array.Empty<string>();

		private readonly ILogger<WukongLogUploader> _logger;

		private readonly ILoggingApi _loggingApi;

		public WukongLogUploader(IGameHostingPlatformManager gamePlatformHelper, ILoggingApi loggingApi, ILogger<WukongLogUploader> logger)
		{
			_loggingApi = loggingApi;
			string wukongMpInstallationPath = StorageHelper.GetWukongMpInstallationPath();
			string gameInstallFolder = gamePlatformHelper.GetGameInstallFolder(Game.BlackMythWukongCoop);
			if (!string.IsNullOrEmpty(gameInstallFolder))
			{
				string[] obj = new string[3]
				{
					Path.Combine(wukongMpInstallationPath, "wukong-mp-logs"),
					null,
					null
				};
				global::<>y__InlineArray5<string> buffer = default(global::<>y__InlineArray5<string>);
				global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 0) = gameInstallFolder;
				global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 1) = "b1";
				global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 2) = "Binaries";
				global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 3) = "Win64";
				global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray5<string>, string>(ref buffer, 4) = "wukong-mp-logs";
				obj[1] = Path.Combine(global::<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<global::<>y__InlineArray5<string>, string>(in buffer, 5));
				obj[2] = Path.Combine(gameInstallFolder, "wukong-mp-logs");
				string[] source = obj;
				_logDirectories = source.Where(Directory.Exists).ToArray();
			}
			_logger = logger;
		}

		public async Task UploadLogsAsync(CancellationToken ct = default(CancellationToken))
		{
			try
			{
				string[] logDirectories = _logDirectories;
				foreach (string text in logDirectories)
				{
					if (!Directory.Exists(text))
					{
						_logger.LogWarning("Log directory does not exist: {LogDirectory}", text);
					}
					else
					{
						await Parallel.ForEachAsync(Directory.GetFiles(text, "log_*.json"), ct, async delegate(string file, CancellationToken token)
						{
							await ProcessFileAsync(file, token);
						});
					}
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Failed to upload Wukong logs");
			}
		}

		private async Task ProcessFileAsync(string file, CancellationToken ct = default(CancellationToken))
		{
			_ = 1;
			try
			{
				if (new FileInfo(file).LastWriteTime < DateTime.Now.AddMonths(-1))
				{
					File.Delete(file);
					_logger.LogInformation("Deleted old log file: {File}", file);
				}
				else
				{
					if (!(await Upload(file, ct)))
					{
						return;
					}
					_logger.LogInformation("Uploaded log file: {File}", file);
					Exception lastException = null;
					for (int i = 0; i < 10; i++)
					{
						try
						{
							File.Delete(file);
							_logger.LogInformation("Deleted log file after upload: {File}", file);
							return;
						}
						catch (Exception ex)
						{
							lastException = ex;
							await Task.Delay(500, ct);
						}
					}
					_logger.LogError(lastException, "Failed to delete log file after upload: {File}", file);
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Failed to process log file: {File}", file);
			}
		}

		private async Task<bool> Upload(string filePath, CancellationToken ct)
		{
			IGetLogsUploadLinkApiResponse uploadUrl = await _loggingApi.GetLogsUploadLinkAsync(ct);
			if (uploadUrl.TryOk(out string result))
			{
				BlobClient blobClient = new BlobClient(new Uri(result));
				bool result2;
				await using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
					Response<BlobContentInfo> response = await blobClient.UploadAsync((Stream)fileStream, overwrite: true, ct);
					if (response.GetRawResponse().IsError)
					{
						_logger.LogError("Failed to upload log file: {File}. Response: {Response}", filePath, response.GetRawResponse());
						result2 = false;
					}
					else
					{
						result2 = true;
					}
				}
				return result2;
			}
			_logger.LogError("Failed to get upload URL for log file. Code: {Code}", uploadUrl.StatusCode);
			return false;
		}
	}
}
namespace ReadyM.Launcher.Core.Identity
{
	public class AuthManager
	{
		private string _lastDiscordState = string.Empty;

		private string _lastDiscordCodeVerifier = string.Empty;

		private readonly SecureStorage _secureStorage;

		private readonly IAuthApi _authClient;

		private readonly ILocalizer _localizer;

		private readonly IViewSwitcher _viewSwitcher;

		private readonly AppConfiguration _appConfig;

		private readonly ILogger<AuthManager> _logger;

		private int? _xModoId;

		private TaskCompletionSource<string?>? _currentOAuthTcs;

		private string? _lastXModoLinkToken;

		private readonly SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(1, 1);

		public int UserId { get; private set; }

		public Guid UserGuid { get; private set; }

		public string Username { get; private set; } = "";

		public DateTime JwtExpiration { get; private set; }

		public bool HasEarlyAccess { get; private set; }

		public bool HasCoop { get; private set; }

		public bool LaunchedThroughXModo { get; private set; }

		public bool LinkedWithXModo => _xModoId.HasValue;

		public bool TesterFeaturesEnabled { get; private set; }

		public bool IsLoggedIn => !string.IsNullOrEmpty(_secureStorage.AccessToken);

		private string OAuthLoginFailed => _localizer.GetValueFromCulture("OAuthLoginFailed");

		private string AnUnexpectedErrorOccuredTryAgainLater => _localizer.GetValueFromCulture("AnUnexpectedErrorOccuredTryAgainLater");

		public AuthManager(IIpcServer ipcServer, SecureStorage secureStorage, IAuthApi authClient, ILocalizer localizer, IViewSwitcher viewSwitcher, AppConfiguration appConfig, ILogger<AuthManager> logger)
		{
			_secureStorage = secureStorage;
			_authClient = authClient;
			_localizer = localizer;
			_viewSwitcher = viewSwitcher;
			_appConfig = appConfig;
			_logger = logger;
			ipcServer.RegisterHandler("auto-update-xmodo-restart", delegate
			{
				_logger.LogInformation("Application was restarted after being launched through XModo.");
				LaunchedThroughXModo = true;
				return Task.CompletedTask;
			});
			ipcServer.RegisterHandler("oauth2", CompleteOAuth2LoginAsync);
			ipcServer.RegisterHandler("xmodo", CompleteXModoLoginAsync);
			ipcServer.RegisterHandler("logout", async delegate(Uri _, CancellationToken ct)
			{
				await LogoutAsync(ct);
				_viewSwitcher.SwitchToView(ViewType.Login);
			});
		}

		public async Task<string?> Login(string email, string password, bool rememberMe, CancellationToken ct = default(CancellationToken))
		{
			if ((await _authClient.LoginAsync(new LoginCommand(rememberMe, email, password), ct)).TryOk(out AuthResult authResult))
			{
				if (authResult.Succeeded)
				{
					await _secureStorage.StoreTokensAsync(authResult.AccessToken, authResult.RefreshToken, ct);
					UpdateData(authResult.AccessToken, launchedThroughXModo: false);
					return null;
				}
				return authResult.Message;
			}
			return AnUnexpectedErrorOccuredTryAgainLater;
		}

		public Task<string?> LoginWithDiscordAsync(CancellationToken ct = default(CancellationToken))
		{
			string discordAuthorizationUrl = GetDiscordAuthorizationUrl();
			Process.Start(new ProcessStartInfo
			{
				FileName = discordAuthorizationUrl,
				UseShellExecute = true
			});
			_currentOAuthTcs = new TaskCompletionSource<string>(ct);
			return _currentOAuthTcs.Task;
		}

		private void UpdateData(string accessToken, bool launchedThroughXModo)
		{
			JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
			JwtExpiration = jwtSecurityToken.ValidTo;
			Username = jwtSecurityToken.Claims.First((Claim c) => c.Type == "name").Value;
			UserGuid = Guid.Parse(jwtSecurityToken.Claims.First((Claim c) => c.Type == "unique_name").Value);
			UserId = int.Parse(jwtSecurityToken.Claims.First((Claim c) => c.Type == "nameid").Value);
			HasEarlyAccess = jwtSecurityToken.Claims.FirstOrDefault((Claim c) => c.Type == "early_access")?.Value == "true";
			HasCoop = jwtSecurityToken.Claims.FirstOrDefault((Claim c) => c.Type == "co-op")?.Value == "true";
			string text = jwtSecurityToken.Claims.FirstOrDefault((Claim c) => c.Type == "xmodo")?.Value;
			_xModoId = ((text != null && int.TryParse(text, out var result)) ? new int?(result) : ((int?)null));
			TesterFeaturesEnabled = jwtSecurityToken.Claims.Any((Claim c) => c.Type == "role" && c.Value == "Tester");
			LaunchedThroughXModo = launchedThroughXModo;
			Telemetry.UserGuid = UserGuid.ToString();
		}

		public async Task LogoutAsync(CancellationToken ct = default(CancellationToken))
		{
			string oldToken = await _secureStorage.RetrieveRefreshToken(ct);
			await _secureStorage.StoreTokensAsync(null, string.Empty, ct);
			Telemetry.UserGuid = null;
			if (oldToken != null)
			{
				await _authClient.LogoutAsync(new RefreshRequest(oldToken), ct);
			}
		}

		public async Task<bool> RefreshTokenAsync(CancellationToken ct = default(CancellationToken))
		{
			await _refreshSemaphore.WaitAsync(ct);
			try
			{
				if (JwtExpiration > DateTime.UtcNow)
				{
					return true;
				}
				_logger.LogDebug("Attempting to refresh token.");
				string text = await _secureStorage.RetrieveRefreshToken(ct);
				if (string.IsNullOrEmpty(text))
				{
					_logger.LogWarning("Refresh token is not saved.");
					return false;
				}
				(string AccessToken, string RefreshToken)? newTokens = await RefreshTokenOnBackend(text, ct);
				if (!newTokens.HasValue)
				{
					return false;
				}
				_logger.LogDebug("Refreshed access token.");
				await _secureStorage.StoreTokensAsync(newTokens.Value.AccessToken, newTokens.Value.RefreshToken, ct);
				UpdateData(newTokens.Value.AccessToken, LaunchedThroughXModo);
				return true;
			}
			finally
			{
				_refreshSemaphore.Release();
			}
		}

		private async Task<(string AccessToken, string RefreshToken)?> RefreshTokenOnBackend(string refreshToken, CancellationToken ct = default(CancellationToken))
		{
			_logger.LogInformation("Refreshing token...");
			try
			{
				if ((await _authClient.RefreshAsync(new RefreshRequest(refreshToken), ct)).TryOk(out RefreshAuthResult result))
				{
					if (!result.Succeeded)
					{
						_logger.LogWarning("Failed to refresh the token. Logging out.");
						await _secureStorage.StoreTokensAsync(null, string.Empty, ct);
						return null;
					}
					return (result.AccessToken, result.RefreshToken);
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Exception occurred while refreshing token.");
				return null;
			}
			_logger.LogError("Failed to refresh token, server may be down.");
			return null;
		}

		private string GetDiscordAuthorizationUrl()
		{
			string text = Guid.NewGuid().ToString();
			_lastDiscordState = DiscordIntent.Login.ToString() + "-" + text;
			_lastDiscordCodeVerifier = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-')
				.Replace('/', '_');
			string value = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(_lastDiscordCodeVerifier))).TrimEnd('=').Replace('+', '-')
				.Replace('/', '_');
			Dictionary<string, string> source = new Dictionary<string, string>
			{
				["client_id"] = _appConfig.DiscordClientId,
				["redirect_uri"] = "readym://oauth2/discord",
				["response_type"] = "code",
				["scope"] = _appConfig.DiscordAuthScopes,
				["state"] = _lastDiscordState,
				["code_challenge"] = value,
				["code_challenge_method"] = "S256"
			};
			string text2 = string.Join("&", source.Select((KeyValuePair<string, string> kvp) => kvp.Key + "=" + Uri.EscapeDataString(kvp.Value)));
			return "https://discord.com/api/oauth2/authorize?" + text2;
		}

		private async Task CompleteOAuth2LoginAsync(Uri uri, CancellationToken ct)
		{
			if (!uri.AbsolutePath.StartsWith("/discord"))
			{
				_logger.LogWarning("Unsupported OAuth2 provider: {Path}", uri.AbsolutePath);
			}
			else
			{
				string result = await HandleDiscordLoginAsync(uri, ct);
				_currentOAuthTcs?.SetResult(result);
			}
		}

		private async Task<string?> HandleDiscordLoginAsync(Uri uri, CancellationToken ct)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('=', 2);
				if (array2.Length == 2)
				{
					dictionary[array2[0]] = Uri.UnescapeDataString(array2[1]);
				}
			}
			if (dictionary.TryGetValue("error", out var value))
			{
				_logger.LogError("Discord OAuth2 error: {Error}", value);
				return OAuthLoginFailed;
			}
			if (!dictionary.TryGetValue("code", out var value2))
			{
				_logger.LogError("Missing 'code' in query parameters.");
				return OAuthLoginFailed;
			}
			if (!dictionary.TryGetValue("state", out var value3))
			{
				_logger.LogError("Missing 'state' in query parameters.");
				return OAuthLoginFailed;
			}
			if (value3 != _lastDiscordState)
			{
				_logger.LogError("Invalid or missing state parameter.");
				return OAuthLoginFailed;
			}
			IDiscordLoginPkceApiResponse discordLoginPkceApiResponse = await _authClient.DiscordLoginPkceAsync(new DiscordLoginPkceRequest(value2, _lastDiscordCodeVerifier), ct);
			if (discordLoginPkceApiResponse.TryOk(out AuthResult authResult))
			{
				if (authResult.Succeeded)
				{
					await _secureStorage.StoreTokensAsync(authResult.AccessToken, authResult.RefreshToken, ct);
					UpdateData(authResult.AccessToken, launchedThroughXModo: false);
					return null;
				}
				_logger.LogError("Discord login failed: {Message}", authResult.Message);
				return OAuthLoginFailed;
			}
			_logger.LogError("Failed to complete Discord login.");
			Error result;
			return discordLoginPkceApiResponse.TryBadRequest(out result) ? _localizer.GetValueFromCulture(result.Code) : OAuthLoginFailed;
		}

		private async Task CompleteXModoLoginAsync(Uri uri, CancellationToken ct)
		{
			string jwt = Uri.UnescapeDataString(uri.PathAndQuery.Trim('/'));
			IXModoLoginApiResponse iXModoLoginApiResponse = await _authClient.XModoLoginAsync(new XModoLoginRequest(jwt), ct);
			Error result;
			if (iXModoLoginApiResponse.TryOk(out XModoLoginResponse loginResponse))
			{
				if (loginResponse.AuthResult != null)
				{
					await _secureStorage.StoreTokensAsync(loginResponse.AuthResult.AccessToken, loginResponse.AuthResult.RefreshToken, ct);
					UpdateData(loginResponse.AuthResult.AccessToken, launchedThroughXModo: true);
					_viewSwitcher.SwitchToView(ViewType.GameSelection);
				}
				else if (loginResponse.LinkToken != null)
				{
					_lastXModoLinkToken = loginResponse.LinkToken;
					_viewSwitcher.SwitchToView(ViewType.LinkWithXModo);
				}
			}
			else if (iXModoLoginApiResponse.TryBadRequest(out result))
			{
				string valueFromCulture = _localizer.GetValueFromCulture(result.Code);
				_viewSwitcher.ShowErrorView(valueFromCulture);
			}
		}

		public async Task<string?> LinkWithXModoAsync(CancellationToken ct = default(CancellationToken))
		{
			if (_lastXModoLinkToken == null)
			{
				_logger.LogError("XModo link token is null.");
				return AnUnexpectedErrorOccuredTryAgainLater;
			}
			IXModoLinkApiResponse iXModoLinkApiResponse = await _authClient.XModoLinkAsync(new XModoLinkRequest(_lastXModoLinkToken), ct);
			if (iXModoLinkApiResponse.TryOk(out AuthResult authResult))
			{
				if (authResult.Succeeded)
				{
					await _secureStorage.StoreTokensAsync(authResult.AccessToken, authResult.RefreshToken, ct);
					UpdateData(authResult.AccessToken, launchedThroughXModo: true);
					return null;
				}
				return _localizer.GetValueFromCulture(authResult.Message);
			}
			Error result;
			return iXModoLinkApiResponse.TryBadRequest(out result) ? _localizer.GetValueFromCulture(result.Code) : AnUnexpectedErrorOccuredTryAgainLater;
		}
	}
	public abstract class PlainStorageBase(ILogger<SecureStorage> logger)
	{
		protected abstract string GetStorageFilePath();

		protected static byte[] EncodeData(string data)
		{
			return Encoding.UTF8.GetBytes(data);
		}

		protected string? DecodeData(byte[] bytes)
		{
			try
			{
				return Encoding.UTF8.GetString(bytes);
			}
			catch (System.Security.Cryptography.CryptographicException exception)
			{
				logger.LogError(exception, "Failed to decode data");
				File.Delete(GetStorageFilePath());
				return null;
			}
		}

		protected async Task StoreDataAsync(string data, CancellationToken ct = default(CancellationToken))
		{
			byte[] bytes = EncodeData(data);
			await File.WriteAllBytesAsync(GetStorageFilePath(), bytes, ct);
		}

		public async Task<string?> RetrieveDataAsync(CancellationToken ct = default(CancellationToken))
		{
			if (File.Exists(GetStorageFilePath()))
			{
				return DecodeData(await File.ReadAllBytesAsync(GetStorageFilePath(), ct));
			}
			return null;
		}
	}
	public class SecureStorage(ILogger<SecureStorage> logger) : SecureStorageBase(<logger>P)
	{
		[CompilerGenerated]
		private string? <AccessToken>k__BackingField;

		public string? AccessToken
		{
			[CompilerGenerated]
			get
			{
				return <AccessToken>k__BackingField;
			}
			private set
			{
				<AccessToken>k__BackingField = value;
				this.OnAccessTokenRefresh?.Invoke();
			}
		}

		public event Action? OnAccessTokenRefresh;

		protected override string GetStorageFilePath()
		{
			return Path.Combine(StorageHelper.GetPersistentStoragePath(), "secure_storage.bin");
		}

		public async Task StoreTokensAsync(string? accessToken, string refreshToken, CancellationToken ct = default(CancellationToken))
		{
			logger.LogDebug("Storing new tokens in Secure Storage");
			AccessToken = accessToken;
			await StoreDataAsync(refreshToken, ct);
		}

		public async Task<string?> RetrieveRefreshToken(CancellationToken ct = default(CancellationToken))
		{
			return await RetrieveDataAsync(ct);
		}
	}
	public abstract class SecureStorageBase(ILogger<SecureStorage> logger)
	{
		protected abstract string GetStorageFilePath();

		protected static byte[] ProtectData(string data)
		{
			return ProtectedData.Protect(Encoding.UTF8.GetBytes(data), null, DataProtectionScope.CurrentUser);
		}

		protected string? UnprotectData(byte[] encryptedBytes)
		{
			try
			{
				byte[] bytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
				return Encoding.UTF8.GetString(bytes);
			}
			catch (System.Security.Cryptography.CryptographicException exception)
			{
				logger.LogError(exception, "Failed to decrypt data");
				File.Delete(GetStorageFilePath());
				return null;
			}
		}

		protected async Task StoreDataAsync(string data, CancellationToken ct = default(CancellationToken))
		{
			byte[] bytes = ProtectData(data);
			await File.WriteAllBytesAsync(GetStorageFilePath(), bytes, ct);
		}

		public async Task<string?> RetrieveDataAsync(CancellationToken ct = default(CancellationToken))
		{
			if (File.Exists(GetStorageFilePath()))
			{
				return UnprotectData(await File.ReadAllBytesAsync(GetStorageFilePath(), ct));
			}
			return null;
		}
	}
	public class TokenRefreshHandler(SecureStorage tokenService, IServiceProvider serviceProvider) : DelegatingHandler()
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.AccessToken);
			HttpResponseMessage response = await base.SendAsync(request, ct);
			if (response.StatusCode == HttpStatusCode.Unauthorized && await serviceProvider.GetRequiredService<AuthManager>().RefreshTokenAsync(ct))
			{
				response.Dispose();
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.AccessToken);
				response = await base.SendAsync(request, ct);
			}
			return response;
		}
	}
}
namespace ReadyM.Launcher.Core.Games
{
	public record GameInfo
	{
		public required Game Game { get; init; }

		public required ModHostingWebsite ModHosting { get; init; }

		public required ModLoader ModLoader { get; init; }

		public required string Name { get; init; }

		public required string DirName { get; init; }

		public required string SteamId { get; init; }

		public required string EpicAppName { get; init; }

		public required string ProcessName { get; init; }

		public string PathName => Name.Replace(":", "");

		[CompilerGenerated]
		[SetsRequiredMembers]
		protected GameInfo(GameInfo original)
		{
			Game = original.Game;
			ModHosting = original.ModHosting;
			ModLoader = original.ModLoader;
			Name = original.Name;
			DirName = original.DirName;
			SteamId = original.SteamId;
			EpicAppName = original.EpicAppName;
			ProcessName = original.ProcessName;
		}

		public GameInfo()
		{
		}
	}
	public static class GameInfoExtensions
	{
		public static GameInfo GetInfo(this Game game)
		{
			return game switch
			{
				Game.BlackMythWukongCoop => new GameInfo
				{
					Game = Game.BlackMythWukongCoop,
					ModHosting = ModHostingWebsite.NexusMods,
					ModLoader = ModLoader.CSharpLoader,
					Name = "Black Myth: Wukong",
					DirName = "Black Myth Wukong Co-op",
					SteamId = "2358720",
					EpicAppName = "f53c5471fd0e47619e72b6d21a527abe",
					ProcessName = "b1-Win64-Shipping"
				}, 
				Game.BlackMythWukongPvp => new GameInfo
				{
					Game = Game.BlackMythWukongPvp,
					ModHosting = ModHostingWebsite.NexusMods,
					ModLoader = ModLoader.CSharpLoader,
					Name = "Black Myth: Wukong",
					DirName = "Black Myth Wukong PvP",
					SteamId = "2358720",
					EpicAppName = "f53c5471fd0e47619e72b6d21a527abe",
					ProcessName = "b1-Win64-Shipping"
				}, 
				_ => throw new ArgumentOutOfRangeException("game", game, null), 
			};
		}
	}
	[JsonConverter(typeof(JsonStringEnumConverter<Game>))]
	[Obsolete("Part of old servers API")]
	public enum Game
	{
		BlackMythWukongCoop,
		BlackMythWukongPvp
	}
	[JsonConverter(typeof(JsonStringEnumConverter<ModHostingWebsite>))]
	[Obsolete("Part of old servers API")]
	public enum ModHostingWebsite
	{
		Thunderstore = 1,
		SotfMods,
		ModIo,
		NexusMods
	}
	[JsonConverter(typeof(JsonStringEnumConverter<ModLoader>))]
	[Obsolete("Part of old servers API")]
	public enum ModLoader
	{
		BepInExMono = 1,
		BepInExIL2CPP,
		RedLoader,
		CSharpLoader
	}
}
namespace ReadyM.Launcher.Core.Downloaders
{
	public class ProgressCallbackDownloader(ILogger<ProgressCallbackDownloader> logger)
	{
		public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double> progress, CancellationToken ct)
		{
			string directoryName = Path.GetDirectoryName(destinationPath);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using HttpClient httpClient = new HttpClient();
			httpClient.Timeout = TimeSpan.FromSeconds(300L);
			using HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
			response.EnsureSuccessStatusCode();
			long? contentLength = response.Content.Headers.ContentLength;
			bool isGzip = response.Content.Headers.ContentEncoding.Contains<string>("gzip", StringComparer.OrdinalIgnoreCase);
			await using Stream responseStream = await response.Content.ReadAsStreamAsync(ct);
			Stream inputStream = responseStream;
			if (isGzip)
			{
				logger.LogDebug("Response is gzip-encoded. Decompressing on the fly.");
				inputStream = new GZipStream(responseStream, CompressionMode.Decompress);
			}
			await using (inputStream)
			{
				await using FileStream output = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);
				byte[] buffer = new byte[81920];
				long totalRead = 0L;
				int lastPercent = -1;
				while (true)
				{
					int read = await inputStream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
					if (read == 0)
					{
						break;
					}
					await output.WriteAsync(buffer.AsMemory(0, read), ct);
					totalRead += read;
					if (contentLength.HasValue)
					{
						int num = (int)(100 * totalRead / contentLength.Value);
						if (num > 100)
						{
							num = 100;
						}
						if (num != lastPercent)
						{
							lastPercent = num;
							progress.Report(num);
						}
					}
					else
					{
						logger.LogWarning("Content-Length header not found, progress percentage cannot be calculated.");
					}
				}
			}
		}
	}
}
namespace ReadyM.Launcher.Core.Common
{
	public static class FileHelpers
	{
		public static async Task CopyDirectoryAsync(string sourceFolder, string targetFolder, CancellationToken ct = default(CancellationToken))
		{
			await Task.Run(delegate
			{
				CopyDirectory(sourceFolder, targetFolder, ct);
			}, ct);
		}

		public static async Task Extract7ZAsync(string sevenZipPath, string extractPath, CancellationToken ct = default(CancellationToken))
		{
			Directory.Delete(extractPath, recursive: true);
			await Task.Run(delegate
			{
				Extract7Z(sevenZipPath, extractPath, ct);
			}, ct);
		}

		private static void Extract7Z(string sevenZipPath, string extractPath, CancellationToken ct = default(CancellationToken))
		{
			Directory.CreateDirectory(extractPath);
			using SevenZipArchive sevenZipArchive = SevenZipArchive.Open(sevenZipPath);
			IReader reader = sevenZipArchive.ExtractAllEntries();
			while (reader.MoveToNextEntry())
			{
				ct.ThrowIfCancellationRequested();
				if (!reader.Entry.IsDirectory)
				{
					reader.WriteEntryToDirectory(extractPath, new ExtractionOptions
					{
						ExtractFullPath = true,
						Overwrite = true
					});
				}
			}
		}

		private static void CopyDirectory(string sourceFolder, string targetFolder, CancellationToken ct = default(CancellationToken))
		{
			Directory.CreateDirectory(targetFolder);
			string[] files = Directory.GetFiles(sourceFolder);
			foreach (string text in files)
			{
				ct.ThrowIfCancellationRequested();
				string destFileName = Path.Combine(targetFolder, Path.GetFileName(text));
				File.Copy(text, destFileName, overwrite: true);
			}
			files = Directory.GetDirectories(sourceFolder);
			foreach (string text2 in files)
			{
				string targetFolder2 = Path.Combine(targetFolder, Path.GetFileName(text2));
				CopyDirectory(text2, targetFolder2, ct);
			}
		}

		public static void FlattenFolderContents(string path, string[] restrictedFolders)
		{
			List<string> list = (from d in Directory.GetDirectories(path)
				where !((ReadOnlySpan<string>)restrictedFolders).Contains(Path.GetFileName(d))
				select d).ToList();
			if (list.Count == 0)
			{
				return;
			}
			foreach (string item in list)
			{
				string[] files = Directory.GetFiles(item);
				foreach (string obj in files)
				{
					string fileName = Path.GetFileName(obj);
					string destFileName = Path.Combine(path, fileName);
					File.Move(obj, destFileName);
				}
				files = Directory.GetDirectories(item);
				foreach (string obj2 in files)
				{
					string fileName2 = Path.GetFileName(obj2);
					string destDirName = Path.Combine(path, fileName2);
					Directory.Move(obj2, destDirName);
				}
				Directory.Delete(item);
			}
			FlattenFolderContents(path, restrictedFolders);
		}
	}
	public enum KnownFolder
	{
		Contacts,
		Downloads,
		Favorites,
		Links,
		SavedGames,
		SavedSearches
	}
	public static class KnownFolders
	{
		private static readonly Dictionary<KnownFolder, Guid> Guids = new Dictionary<KnownFolder, Guid>
		{
			[KnownFolder.Contacts] = new Guid("56784854-C6CB-462B-8169-88E350ACB882"),
			[KnownFolder.Downloads] = new Guid("374DE290-123F-4565-9164-39C4925E467B"),
			[KnownFolder.Favorites] = new Guid("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
			[KnownFolder.Links] = new Guid("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
			[KnownFolder.SavedGames] = new Guid("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
			[KnownFolder.SavedSearches] = new Guid("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
		};

		public static string GetPath(KnownFolder knownFolder)
		{
			return SHGetKnownFolderPath(Guids[knownFolder], 0u);
		}

		[DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = 0);
	}
	public static class StorageHelper
	{
		public static string GetPersistentStoragePath()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ReadyM.Launcher");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		public static string GetDownloadCachePath()
		{
			string text = Path.Combine(GetPersistentStoragePath(), "DownloadCache");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		public static string GetAvatarCachePath()
		{
			string text = Path.Combine(GetPersistentStoragePath(), "AvatarCache");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		public static string GetWukongMpInstallationPath(string? loaderVersion = null)
		{
			string text = Path.Combine(GetPersistentStoragePath(), "WukongMP");
			string result = Path.Combine(GetPersistentStoragePath(), "game_modes", "Black Myth Wukong Co-op");
			if (loaderVersion != null)
			{
				if (string.Compare(loaderVersion, "0.7.303.1106", StringComparison.OrdinalIgnoreCase) <= 0)
				{
					return result;
				}
				return text;
			}
			if (Directory.Exists(text))
			{
				return text;
			}
			return result;
		}
	}
}
namespace ReadyM.Launcher.Core.Common.Resources
{
	public interface IResourceLocator
	{
		string GetBepInExArchivePath(GameInfo game);

		string GetRedLoaderArchivePath(GameInfo game);
	}
	public class WindowsResourceLocator : IResourceLocator
	{
		public string GetBepInExArchivePath(GameInfo game)
		{
			return Path.Combine(AppContext.BaseDirectory, game.PathName, "BepInEx-windows.zip");
		}

		public string GetRedLoaderArchivePath(GameInfo game)
		{
			return Path.Combine(AppContext.BaseDirectory, game.PathName, "RedLoader.zip");
		}
	}
}
namespace ReadyM.Launcher.Core.Common.HostingPlatforms
{
	public class EpicGamesHelper : IGameHostingPlatformHelper
	{
		private class EpicGamesManifest
		{
			public required string LaunchExecutable { get; set; }

			public required string InstallLocation { get; set; }

			public required string CatalogNamespace { get; set; }

			public required string CatalogItemId { get; set; }

			public required string AppName { get; set; }
		}

		private readonly ISettingsManager _settingsManager;

		private readonly IFileSystem _fileSystem;

		private readonly ILogger<EpicGamesHelper> _logger;

		private readonly Dictionary<string, EpicGamesManifest> _manifestCache;

		public EpicGamesHelper(IFileSystem fileSystem, ISettingsManager settingsManager, ILogger<EpicGamesHelper> logger)
		{
			_fileSystem = fileSystem;
			_settingsManager = settingsManager;
			_logger = logger;
			_manifestCache = ParseManifestFiles();
		}

		private Dictionary<string, EpicGamesManifest> ParseManifestFiles()
		{
			string reference = "C:\\ProgramData\\Epic\\EpicGamesLauncher\\Data\\Manifests";
			string text = Path.Combine(new ReadOnlySpan<string>(in reference));
			if (!_fileSystem.Directory.Exists(text))
			{
				_logger.LogWarning("Epic games manifest folder not found: {File}", text);
				return new Dictionary<string, EpicGamesManifest>();
			}
			Dictionary<string, EpicGamesManifest> dictionary = new Dictionary<string, EpicGamesManifest>();
			string[] files = _fileSystem.Directory.GetFiles(text, "*.item");
			foreach (string text2 in files)
			{
				try
				{
					EpicGamesManifest epicGamesManifest = JsonSerializer.Deserialize<EpicGamesManifest>(_fileSystem.File.ReadAllText(text2));
					if (epicGamesManifest == null)
					{
						_logger.LogError("Invalid manifest file {File}: missing required fields", text2);
					}
					else
					{
						dictionary[epicGamesManifest.AppName] = epicGamesManifest;
					}
				}
				catch (Exception exception)
				{
					_logger.LogError(exception, "Error parsing manifest file {File}", text2);
				}
			}
			return dictionary;
		}

		public bool IsGameInstalled(Game game)
		{
			string epicAppName = game.GetInfo().EpicAppName;
			return _manifestCache.ContainsKey(epicAppName);
		}

		public string? GetGameInstallFolder(Game game)
		{
			string epicAppName = game.GetInfo().EpicAppName;
			if (_settingsManager.ReadSettingsFromDisk().GameInstallationFolderOverrides.TryGetValue(game, out string value))
			{
				return value;
			}
			if (_manifestCache.TryGetValue(epicAppName, out EpicGamesManifest value2))
			{
				return value2.InstallLocation;
			}
			return null;
		}

		public void LaunchGame(Game game, string args = "")
		{
			string epicAppName = game.GetInfo().EpicAppName;
			EpicGamesManifest valueOrDefault = _manifestCache.GetValueOrDefault(epicAppName);
			if (valueOrDefault == null)
			{
				_logger.LogError("Game manifest not found for game ID: {AppId}", epicAppName);
				return;
			}
			string text = Path.Combine(valueOrDefault.InstallLocation, valueOrDefault.LaunchExecutable);
			Process.Start(new ProcessStartInfo
			{
				FileName = text,
				Arguments = args,
				UseShellExecute = true
			});
			_logger.LogInformation("Launched game with Epic Games Launcher, command: {Program} {Args}", text, args);
		}
	}
	public enum GameHostingPlatform
	{
		Steam,
		EpicGames
	}
	public static class GameHostingPlatformExtensions
	{
		public static string ToFriendlyString(this GameHostingPlatform platform)
		{
			return platform switch
			{
				GameHostingPlatform.Steam => "Steam", 
				GameHostingPlatform.EpicGames => "Epic Games", 
				_ => throw new ArgumentOutOfRangeException("platform", platform, null), 
			};
		}
	}
	public class GameHostingPlatformManager(IServiceProvider serviceProvider, ILogger<GameHostingPlatformManager> logger) : IGameHostingPlatformManager
	{
		public GameHostingPlatform PreferredPlatform { get; set; }

		public void FindGameInstallationPlatform(Game game)
		{
			GameHostingPlatform[] values = Enum.GetValues<GameHostingPlatform>();
			foreach (GameHostingPlatform gameHostingPlatform in values)
			{
				if (serviceProvider.GetRequiredKeyedService<IGameHostingPlatformHelper>(gameHostingPlatform).IsGameInstalled(game))
				{
					PreferredPlatform = gameHostingPlatform;
					logger.LogDebug("Found game {GameName} on platform {Platform}", game.GetInfo().Name, gameHostingPlatform);
					break;
				}
			}
		}

		public string? GetGameInstallFolder(Game game)
		{
			FindGameInstallationPlatform(game);
			return serviceProvider.GetRequiredKeyedService<IGameHostingPlatformHelper>(PreferredPlatform).GetGameInstallFolder(game);
		}

		public void LaunchGame(Game game, string args = "")
		{
			FindGameInstallationPlatform(game);
			serviceProvider.GetRequiredKeyedService<IGameHostingPlatformHelper>(PreferredPlatform).LaunchGame(game, args);
		}
	}
	public interface IGameHostingPlatformHelper
	{
		bool IsGameInstalled(Game game);

		string? GetGameInstallFolder(Game game);

		void LaunchGame(Game game, string args = "");
	}
	public interface IGameHostingPlatformManager
	{
		GameHostingPlatform PreferredPlatform { get; set; }

		void FindGameInstallationPlatform(Game game);

		string? GetGameInstallFolder(Game game);

		void LaunchGame(Game game, string args = "");
	}
	public class SteamHelper : IGameHostingPlatformHelper
	{
		private readonly Dictionary<string, string> _steamLibraryCache;

		private readonly ISettingsManager _settingsManager;

		private readonly IFileSystem _fileSystem;

		private readonly ILogger<SteamHelper> _logger;

		public SteamHelper(IFileSystem fileSystem, ISettingsManager settingsManager, ILogger<SteamHelper> logger)
		{
			_fileSystem = fileSystem;
			_settingsManager = settingsManager;
			_logger = logger;
			_steamLibraryCache = ParseLibraryFolders();
		}

		private Dictionary<string, string> ParseLibraryFolders()
		{
			string text = Path.Combine(GetSteamInstallDir(), "config", "libraryfolders.vdf");
			if (!_fileSystem.File.Exists(text))
			{
				_logger.LogWarning("Steam library folders file not found: {File}", text);
				return new Dictionary<string, string>();
			}
			VProperty vProperty = VdfConvert.Deserialize(_fileSystem.File.ReadAllText(text));
			Dictionary<string, List<long>> dictionary = new Dictionary<string, List<long>>();
			foreach (dynamic item in vProperty.Value.Children())
			{
				string key = (string)item.Value.path.Value;
				dictionary[key] = new List<long>();
				foreach (dynamic item2 in item.Value.apps.Children())
				{
					dynamic val = long.Parse(item2.Key);
					dictionary[key].Add(val);
				}
			}
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, List<long>> item3 in dictionary)
			{
				item3.Deconstruct(out var key2, out var value);
				string path = key2;
				List<long> list = value;
				string path2 = Path.Combine(path, "steamapps");
				foreach (long item4 in list)
				{
					string text2 = Path.Combine(path2, $"appmanifest_{item4}.acf");
					if (!_fileSystem.File.Exists(text2))
					{
						_logger.LogWarning("Steam app manifest file not found: {File}", text2);
						continue;
					}
					dynamic val2 = VdfConvert.Deserialize(_fileSystem.File.ReadAllText(text2));
					string path3 = (string)val2.Value.installdir.Value;
					string text3 = (string)val2.Value.name.Value;
					dictionary2[item4.ToString()] = Path.Combine(path2, "common", path3);
					_logger.LogDebug("Found Steam game installation path for game {Game}: {Path}", text3, dictionary2[item4.ToString()]);
				}
			}
			return dictionary2;
		}

		public string GetSteamInstallDir()
		{
			try
			{
				using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Valve\\Steam");
				object obj = registryKey?.GetValue("InstallPath");
				if (obj != null)
				{
					return obj.ToString();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Error reading Steam installation path: " + ex.Message);
			}
			return "C:\\Program Files (x86)\\Steam";
		}

		public string GetSteamExePath()
		{
			return _fileSystem.Path.Combine(GetSteamInstallDir(), "steam.exe");
		}

		public bool IsGameInstalled(Game game)
		{
			string steamId = game.GetInfo().SteamId;
			return _steamLibraryCache.ContainsKey(steamId);
		}

		public string? GetGameInstallFolder(Game game)
		{
			string steamId = game.GetInfo().SteamId;
			if (_settingsManager.ReadSettingsFromDisk().GameInstallationFolderOverrides.TryGetValue(game, out string value))
			{
				return value;
			}
			if (_steamLibraryCache.TryGetValue(steamId, out value))
			{
				return value;
			}
			string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App " + steamId;
			try
			{
				using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
				object obj = registryKey?.GetValue("InstallLocation");
				if (obj != null)
				{
					_logger.LogDebug("Found Steam game installation path in registry for game {AppId}: {Value}", steamId, obj);
					return obj.ToString();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Error reading Steam game installation path: {Error}", ex.Message);
			}
			return null;
		}

		public void LaunchGame(Game game, string args = "")
		{
			string steamId = game.GetInfo().SteamId;
			string steamExePath = GetSteamExePath();
			Process.Start(new ProcessStartInfo
			{
				FileName = steamExePath,
				Arguments = "-applaunch " + steamId + " " + args,
				UseShellExecute = true
			});
			_logger.LogInformation("Launched game with Steam, command: {Program} {Args}", steamExePath, args);
		}
	}
}
[StructLayout(LayoutKind.Auto)]
[InlineArray(2)]
internal struct <>y__InlineArray2<T>
{
}
[StructLayout(LayoutKind.Auto)]
[InlineArray(5)]
internal struct <>y__InlineArray5<T>
{
}
