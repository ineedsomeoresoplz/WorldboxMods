using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using XaviiNowWePlayMod.Code.Features;

namespace XaviiNowWePlayMod.Code.Networking
{
    internal sealed class XNWPMNetworkManager : IDisposable
    {
        private const int DefaultPort = 29101;
        private const int MaxPayloadSize = 50 * 1024 * 1024; 
        private const int MinPlayers = 2;
        private const int MaxPlayers = 16;
        private const float MemberRefreshInterval = 1f;

        public const int DefaultMaxPlayers = 8;

        private enum SessionRole
        {
            None,
            Host,
            Client
        }

        private class ClientLink
        {
            public string Id;
            public string Name;
            public TcpClient Socket;
            public NetworkStream Stream;
            public CancellationTokenSource Cts;
        }

        [Serializable]
        private class WirePacket
        {
            public string type;
            public string token;
            public string name;
            public string worldId;
            public WorldPackage worldPackage;
            public GodCommandMessage command;
            public List<MemberEntry> roster;
            public string reason;
        }

        [Serializable]
        private class MemberEntry
        {
            public string id;
            public string name;
            public bool isHost;
        }

        [Serializable]
        private class WorldPackage
        {
            public string worldId;
            public string worldName;
            public string mapDataBase64; 
        }

        private readonly Queue<QueuedGodCommand> _incomingCommands = new();
        private readonly List<(string ClientId, string Name, bool IsHost)> _memberSnapshot = new();
        private readonly Dictionary<string, ClientLink> _clientLinks = new();
        private readonly object _memberLock = new();
        private readonly object _queueLock = new();
        private readonly object _sendLock = new();

        private TcpListener _listener;
        private CancellationTokenSource _sessionCts;
        private Task _acceptLoop;
        private Task _clientReceiveLoop;
        private TcpClient _hostConnection;
        private NetworkStream _hostStream;

        private SessionRole _role;
        private byte[] _cachedWorldBytes;
        private string _cachedWorldName = "world";
        private string _status = "idle";
        private string _sessionInfo = string.Empty;
        private string _sessionWorldId = string.Empty;
        private string _sessionToken = string.Empty;
        private string _joinCode = string.Empty;
        private float _nextMemberRefresh;
        private bool _disposed;
        private int _maxPlayers = DefaultMaxPlayers;
        private string _remoteEndpointLabel = string.Empty;
        private bool _upnpMapped;

        public event Action<string> OnLog;
        public event Action<string> OnStatusChanged;
        public event Action<string> OnSessionInfoChanged;

        public string Status => _status;
        public string SessionInfo => _sessionInfo;
        public string SessionWorldId => _sessionWorldId;
        public string JoinCode => _joinCode;
        public bool IsHost => _role == SessionRole.Host;
        public bool IsClient => _role == SessionRole.Client;
        public bool HasSession => _role != SessionRole.None;

        public IReadOnlyList<(string ClientId, string Name, bool IsHost)> CurrentMembers
        {
            get
            {
                lock (_memberLock)
                {
                    return new List<(string, string, bool)>(_memberSnapshot);
                }
            }
        }

        public XNWPMNetworkManager()
        {
            _sessionWorldId = WorldSyncGuard.CurrentWorldId ?? string.Empty;
        }

        public void Tick()
        {
            if (_disposed || !HasSession)
            {
                return;
            }

            RefreshMembers(false);
        }

        public void StartHosting(int requestedMaxPlayers)
        {
            if (HasSession)
            {
                return;
            }

            _maxPlayers = Mathf.Clamp(requestedMaxPlayers, MinPlayers, MaxPlayers);
            _sessionToken = Guid.NewGuid().ToString("N").Substring(0, 10);
            _sessionWorldId = WorldSyncGuard.CurrentWorldId ?? string.Empty;
            CacheWorldBytes();
            _sessionCts = new CancellationTokenSource();
            _role = SessionRole.Host;

            _ = StartListenerAsync(_sessionCts.Token);
        }

        public bool JoinSession(string joinCode)
        {
            if (HasSession || string.IsNullOrWhiteSpace(joinCode))
            {
                return false;
            }

            if (!TryDecodeJoinCode(joinCode.Trim(), out var endpoint, out string token))
            {
                return false;
            }

            _sessionToken = token;
            _joinCode = joinCode.Trim();
            _sessionWorldId = WorldSyncGuard.CurrentWorldId ?? string.Empty;
            _role = SessionRole.Client;
            _sessionCts = new CancellationTokenSource();
            _ = ConnectToHostAsync(endpoint, _sessionCts.Token);
            return true;
        }

        public void StopHosting()
        {
            if (!IsHost)
            {
                return;
            }

            CleanSession("Host stopped the session.");
        }

        public void Disconnect()
        {
            if (!IsClient)
            {
                return;
            }

            CleanSession("Disconnected from the session.");
        }

        public void BroadcastToClients(GodCommandMessage message, string excludeClientId = null)
        {
            if (!IsHost || message == null)
            {
                return;
            }

            var packet = new WirePacket { type = "command", command = message };
            foreach (var link in GetClientLinks())
            {
                if (!string.IsNullOrEmpty(excludeClientId) && link.Id == excludeClientId)
                {
                    continue;
                }

                _ = SendPacketAsync(link.Stream, packet, link.Cts.Token);
            }
        }

        public void SendCommandToHost(GodCommandMessage message)
        {
            if (!IsClient || message == null || _hostStream == null)
            {
                return;
            }

            var packet = new WirePacket { type = "command", command = message };
            _ = SendPacketAsync(_hostStream, packet, _sessionCts?.Token ?? CancellationToken.None);
        }

        public bool TryDequeueCommand(out QueuedGodCommand command)
        {
            lock (_queueLock)
            {
                if (_incomingCommands.Count == 0)
                {
                    command = default;
                    return false;
                }

                command = _incomingCommands.Dequeue();
                return true;
            }
        }

        public void UpdateSessionWorldId(string worldId, bool pushToPeers)
        {
            if (string.IsNullOrWhiteSpace(worldId) || _sessionWorldId == worldId)
            {
                return;
            }

            _sessionWorldId = worldId;
            if (IsHost && pushToPeers)
            {
                var rosterPacket = BuildRosterPacket();
                foreach (var link in GetClientLinks())
                {
                    _ = SendPacketAsync(link.Stream, rosterPacket, link.Cts.Token);
                }
            }

            UpdateSessionInfo();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            CleanSession("Network manager disposed.");
        }

        #region Hosting
        private async Task StartListenerAsync(CancellationToken ct)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, DefaultPort);
                _listener.Start();

                
                _upnpMapped = await UpnpPortMapper.TryMapPortAsync(DefaultPort, "XNWPM", TimeSpan.FromSeconds(2));
                if (_upnpMapped)
                {
                    Log("UPnP/NAT-PMP: port 29101 mapped automatically.");
                }
                else
                {
                    Log("UPnP/NAT-PMP: could not map port automatically; direct TCP will be used.");
                }

                string publicIp = await ResolvePublicIPv4Async();
                _joinCode = CreateJoinCode(publicIp, DefaultPort, _sessionToken);
                SetStatus("Hosting");
                Log($"Hosting on {publicIp}:{DefaultPort} with join code {_joinCode} (world {_sessionWorldId})");
                UpdateSessionInfo();
                _acceptLoop = AcceptLoopAsync(ct);
                await _acceptLoop;
            }
            catch (Exception ex)
            {
                Log($"Failed to start listener: {ex.Message}");
                CleanSession("Listener failed to start.");
            }
        }

        private async Task AcceptLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    if (ct.IsCancellationRequested)
                    {
                        client.Close();
                        break;
                    }
                    _ = HandleClientAsync(client, ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Log($"Accept error: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken sessionToken)
        {
            client.NoDelay = true;
            using var linkCts = CancellationTokenSource.CreateLinkedTokenSource(sessionToken);

            try
            {
                var stream = client.GetStream();
                var hello = await ReadPacketAsync(stream, linkCts.Token);
                if (hello == null || hello.type != "hello" || hello.token != _sessionToken)
                {
                    await SendPacketAsync(stream, new WirePacket { type = "reject", reason = "Invalid join code." }, linkCts.Token);
                    client.Close();
                    return;
                }

                if (_clientLinks.Count + 1 >= _maxPlayers)
                {
                    await SendPacketAsync(stream, new WirePacket { type = "reject", reason = "Session full." }, linkCts.Token);
                    client.Close();
                    return;
                }

                bool needsWorldTransfer = !string.IsNullOrWhiteSpace(_sessionWorldId) && !string.IsNullOrWhiteSpace(hello.worldId) && hello.worldId != _sessionWorldId;

                string clientId = Guid.NewGuid().ToString("N");
                string displayName = string.IsNullOrWhiteSpace(hello.name) ? client.Client.RemoteEndPoint?.ToString() ?? "Player" : hello.name.Trim();
                var link = new ClientLink
                {
                    Id = clientId,
                    Name = displayName,
                    Socket = client,
                    Stream = stream,
                    Cts = linkCts
                };

                lock (_memberLock)
                {
                    _clientLinks[clientId] = link;
                }

                var acceptPacket = new WirePacket { type = "accept", worldId = _sessionWorldId };
                if (needsWorldTransfer)
                {
                    acceptPacket.worldPackage = BuildWorldPackage();
                }

                await SendPacketAsync(stream, acceptPacket, linkCts.Token);
                BroadcastRoster();
                Log($"{displayName} joined the session.");
                await ReceiveLoopAsync(link, linkCts.Token);
            }
            catch (Exception ex)
            {
                Log($"Client handler error: {ex.Message}");
            }
            finally
            {
                RemoveClient(client);
                BroadcastRoster();
            }
        }
        #endregion

        #region Client connect
        private async Task ConnectToHostAsync((string Host, int Port) endpoint, CancellationToken ct)
        {
            try
            {
                _hostConnection = new TcpClient();
                var connectTask = _hostConnection.ConnectAsync(endpoint.Host, endpoint.Port);
                var cancelTask = Task.Delay(Timeout.Infinite, ct);
                var completed = await Task.WhenAny(connectTask, cancelTask);
                if (completed == cancelTask)
                {
                    _hostConnection.Close();
                    ct.ThrowIfCancellationRequested();
                }

                await connectTask;
                _hostConnection.NoDelay = true;
                _hostStream = _hostConnection.GetStream();
                _remoteEndpointLabel = $"{endpoint.Host}:{endpoint.Port}";

                var hello = new WirePacket
                {
                    type = "hello",
                    token = _sessionToken,
                    name = SystemInfo.deviceName ?? "Player",
                    worldId = _sessionWorldId
                };

                await SendPacketAsync(_hostStream, hello, ct);
                SetStatus("Connecting to host...");
                Log($"Connecting to {_remoteEndpointLabel} with world {_sessionWorldId} and token {_sessionToken}...");
                _clientReceiveLoop = ClientReceiveLoopAsync(ct);
                await _clientReceiveLoop;
            }
            catch (Exception ex)
            {
                Log($"Unable to connect to host: {ex.Message}");
                CleanSession("Connection failed.");
            }
        }
        #endregion

        #region Receive loops
        private async Task ReceiveLoopAsync(ClientLink link, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var packet = await ReadPacketAsync(link.Stream, ct);
                if (packet == null)
                {
                    break;
                }

                if (packet.type == "command" && packet.command != null)
                {
                    EnqueueCommand(packet.command, link.Id);
                }
            }
        }

        private async Task ClientReceiveLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && _hostStream != null)
            {
                var packet = await ReadPacketAsync(_hostStream, ct);
                if (packet == null)
                {
                    break;
                }

                switch (packet.type)
                {
                    case "accept":
                        if (packet.worldPackage != null)
                        {
                            if (ApplyWorldPackage(packet.worldPackage))
                            {
                                Log("Downloaded host world and loading it now...");
                            }
                            else
                            {
                                Log("Failed to apply host world; staying on local world.");
                            }
                        }

                        SetStatus("Connected to host.");
                        _sessionWorldId = string.IsNullOrWhiteSpace(packet.worldId) ? _sessionWorldId : packet.worldId;
                        UpdateSessionInfo();
                        break;
                    case "reject":
                        Log(string.IsNullOrWhiteSpace(packet.reason) ? "Join request rejected." : packet.reason);
                        CleanSession(packet.reason ?? "Join rejected.");
                        return;
                    case "command":
                        if (packet.command != null)
                        {
                            EnqueueCommand(packet.command, "host");
                        }
                        break;
                    case "roster":
                        if (packet.roster != null)
                        {
                            ApplyRoster(packet.roster);
                        }
                        break;
                }
            }

            if (!ct.IsCancellationRequested)
            {
                CleanSession("Disconnected from host.");
            }
        }
        #endregion

        #region Roster
        private void ApplyRoster(List<MemberEntry> roster)
        {
            var entries = new List<(string, string, bool)>();
            foreach (var member in roster)
            {
                entries.Add((member.id, string.IsNullOrWhiteSpace(member.name) ? member.id : member.name, member.isHost));
            }

            lock (_memberLock)
            {
                _memberSnapshot.Clear();
                _memberSnapshot.AddRange(entries);
            }

            UpdateSessionInfo();
        }

        private void BroadcastRoster()
        {
            if (!IsHost)
            {
                return;
            }

            var packet = BuildRosterPacket();
            foreach (var link in GetClientLinks())
            {
                _ = SendPacketAsync(link.Stream, packet, link.Cts.Token);
            }

            RefreshMembers(true);
        }

        private WirePacket BuildRosterPacket()
        {
            var roster = new List<MemberEntry>();
            roster.Add(new MemberEntry { id = "host", name = SystemInfo.deviceName ?? "Host", isHost = true });
            foreach (var link in GetClientLinks())
            {
                roster.Add(new MemberEntry { id = link.Id, name = link.Name, isHost = false });
            }

            return new WirePacket
            {
                type = "roster",
                roster = roster,
                worldId = _sessionWorldId
            };
        }

        private void RefreshMembers(bool force)
        {
            if (!IsHost)
            {
                return;
            }

            float now = Time.realtimeSinceStartup;
            if (!force && now < _nextMemberRefresh)
            {
                return;
            }

            _nextMemberRefresh = now + MemberRefreshInterval;

            var entries = new List<(string, string, bool)>
            {
                ("host", SystemInfo.deviceName ?? "Host", true)
            };

            foreach (var link in GetClientLinks())
            {
                entries.Add((link.Id, link.Name, false));
            }

            lock (_memberLock)
            {
                _memberSnapshot.Clear();
                _memberSnapshot.AddRange(entries);
            }

            UpdateSessionInfo();
        }
        #endregion

        #region Utilities (send/receive)
        private IEnumerable<ClientLink> GetClientLinks()
        {
            lock (_memberLock)
            {
                return new List<ClientLink>(_clientLinks.Values);
            }
        }

        private void EnqueueCommand(GodCommandMessage message, string source)
        {
            lock (_queueLock)
            {
                _incomingCommands.Enqueue(new QueuedGodCommand(message, source));
            }
        }

        private async Task<WirePacket> ReadPacketAsync(NetworkStream stream, CancellationToken ct)
        {
            byte[] lengthBytes = await ReadExactAsync(stream, 4, ct);
            if (lengthBytes == null)
            {
                return null;
            }

            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBytes, 0));
            if (length <= 0 || length > MaxPayloadSize)
            {
                throw new InvalidOperationException($"Packet length {length} is invalid.");
            }

            byte[] payload = await ReadExactAsync(stream, length, ct);
            if (payload == null)
            {
                return null;
            }

            string json = Encoding.UTF8.GetString(payload);
            return JsonUtility.FromJson<WirePacket>(json);
        }

        private static async Task<byte[]> ReadExactAsync(NetworkStream stream, int length, CancellationToken ct)
        {
            byte[] buffer = new byte[length];
            int offset = 0;
            while (offset < length)
            {
                int read = await stream.ReadAsync(buffer, offset, length - offset, ct);
                if (read == 0)
                {
                    return null;
                }

                offset += read;
            }

            return buffer;
        }

        private async Task SendPacketAsync(NetworkStream stream, WirePacket packet, CancellationToken ct)
        {
            if (stream == null || packet == null)
            {
                return;
            }

            string json = JsonUtility.ToJson(packet);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));

            lock (_sendLock)
            {
                stream.Write(len, 0, len.Length);
                stream.Write(data, 0, data.Length);
            }

            await stream.FlushAsync(ct);
        }
        #endregion

        #region Session teardown
        private void RemoveClient(TcpClient client)
        {
            string keyToRemove = null;
            lock (_memberLock)
            {
                foreach (var kvp in _clientLinks)
                {
                    if (kvp.Value.Socket == client)
                    {
                        keyToRemove = kvp.Key;
                        break;
                    }
                }

                if (keyToRemove != null)
                {
                    _clientLinks.Remove(keyToRemove);
                }
            }

            try
            {
                client?.Close();
            }
            catch { }
        }

        private void CleanSession(string message)
        {
            try
            {
                _sessionCts?.Cancel();
            }
            catch { }

            try { _listener?.Stop(); } catch { }

            foreach (var link in GetClientLinks())
            {
                try { link.Cts?.Cancel(); } catch { }
                try { link.Socket?.Close(); } catch { }
            }

            try { _hostConnection?.Close(); } catch { }

            if (_upnpMapped)
            {
                UpnpPortMapper.TryUnmapPortAsync(DefaultPort).Forget();
                _upnpMapped = false;
            }

            lock (_memberLock)
            {
                _clientLinks.Clear();
                _memberSnapshot.Clear();
            }

            lock (_queueLock)
            {
                _incomingCommands.Clear();
            }

            _listener = null;
            _hostConnection = null;
            _hostStream = null;
            _acceptLoop = null;
            _clientReceiveLoop = null;
            _cachedWorldBytes = null;
            _role = SessionRole.None;
            _joinCode = string.Empty;
            _sessionToken = string.Empty;
            _sessionInfo = string.Empty;
            _remoteEndpointLabel = string.Empty;
            SetStatus("idle");
            UpdateSessionInfo();

            if (!string.IsNullOrWhiteSpace(message))
            {
                Log(message);
            }
        }
        #endregion

        #region Status + logging
        private void SetStatus(string value)
        {
            if (_status == value)
            {
                return;
            }

            _status = value;
            OnStatusChanged?.Invoke(value);
        }

        private void UpdateSessionInfo()
        {
            string info = string.Empty;
            if (IsHost)
            {
                info = string.IsNullOrWhiteSpace(_joinCode)
                    ? "Hosting (no code)"
                    : $"Join code: {_joinCode} ({_clientLinks.Count + 1}/{_maxPlayers}) | World: {_sessionWorldId}";
            }
            else if (IsClient)
            {
                info = string.IsNullOrWhiteSpace(_remoteEndpointLabel)
                    ? "Connected"
                    : $"Connected to {_remoteEndpointLabel} | World: {_sessionWorldId}";
            }

            if (_sessionInfo != info)
            {
                _sessionInfo = info;
                OnSessionInfoChanged?.Invoke(info);
            }
        }

        private void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            OnLog?.Invoke(message);
        }

        public void AppendLog(string message)
        {
            Log(message);
        }
        #endregion

        #region World transfer
        private void CacheWorldBytes()
        {
            try
            {
                string mapPath = SaveManager.getSavePath(SaveManager.currentSavePath);
                if (string.IsNullOrWhiteSpace(mapPath) || !File.Exists(mapPath))
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), "xnwpm_host_world");
                    Directory.CreateDirectory(tempDir);
                    SaveManager.saveWorldToDirectory(tempDir, true, true);
                    mapPath = Path.Combine(tempDir, "map.wbox");
                }

                if (!File.Exists(mapPath))
                {
                    Log("Unable to cache world: no map.wbox found.");
                    _cachedWorldBytes = null;
                    return;
                }

                _cachedWorldBytes = File.ReadAllBytes(mapPath);
                _cachedWorldName = World.world?.map_stats?.name ?? "world";
            }
            catch (Exception ex)
            {
                Log($"Cache world failed: {ex.Message}");
                _cachedWorldBytes = null;
            }
        }

        private WorldPackage BuildWorldPackage()
        {
            try
            {
                if (_cachedWorldBytes == null || _cachedWorldBytes.Length == 0)
                {
                    CacheWorldBytes();
                }

                if (_cachedWorldBytes == null || _cachedWorldBytes.Length == 0)
                {
                    Log("World transfer failed: no cached data.");
                    return null;
                }

                var package = new WorldPackage
                {
                    worldId = _sessionWorldId,
                    worldName = _cachedWorldName,
                    mapDataBase64 = Convert.ToBase64String(_cachedWorldBytes)
                };

                return package;
            }
            catch (Exception ex)
            {
                Log($"World transfer failed: {ex.Message}");
                return null;
            }
        }

        private bool ApplyWorldPackage(WorldPackage package)
        {
            if (package == null || string.IsNullOrWhiteSpace(package.mapDataBase64))
            {
                return false;
            }

            try
            {
                byte[] mapBytes = Convert.FromBase64String(package.mapDataBase64);
                SaveManager.loadMapFromBytes(mapBytes);

                string sessionFolder = $"xnwpm_session_{_sessionToken}";
                string targetPath = SaveManager.generateMainPath("saves") + SaveManager.folderPath(sessionFolder);
                Directory.CreateDirectory(targetPath);
                File.WriteAllBytes(Path.Combine(targetPath, "map.wbox"), mapBytes);
                SaveManager.setCurrentPath(targetPath);

                _sessionWorldId = package.worldId ?? _sessionWorldId;
                _cachedWorldBytes = mapBytes;
                _cachedWorldName = package.worldName ?? _cachedWorldName;
                return true;
            }
            catch (Exception ex)
            {
                Log($"Failed to apply host world: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Join codes
        private static string CreateJoinCode(string ip, int port, string token)
        {
            string payload = $"{ip}|{port}|{token}";
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)).TrimEnd('=');
            base64 = base64.Replace('+', '-').Replace('/', '_');
            return ChunkString(base64, 4);
        }

        private static string ChunkString(string value, int chunkSize)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                if (i > 0 && i % chunkSize == 0)
                {
                    sb.Append('-');
                }

                sb.Append(value[i]);
            }

            return sb.ToString();
        }

        private static bool TryDecodeJoinCode(string code, out (string Host, int Port) endpoint, out string token)
        {
            endpoint = (null, 0);
            token = string.Empty;
            try
            {
                string cleaned = code.Replace("-", string.Empty).Replace(" ", string.Empty);
                string base64 = cleaned.Replace('-', '+').Replace('_', '/');
                int padding = 4 - (base64.Length % 4);
                if (padding < 4)
                {
                    base64 = base64.PadRight(base64.Length + padding, '=');
                }

                string payload = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                string[] parts = payload.Split('|');
                if (parts.Length != 3)
                {
                    return false;
                }

                if (!int.TryParse(parts[1], out int port))
                {
                    return false;
                }

                endpoint = (parts[0], port);
                token = parts[2];
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Helpers
        private static async Task<string> ResolvePublicIPv4Async()
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                string ip = await client.GetStringAsync("https://api.ipify.org");
                if (IPAddress.TryParse(ip.Trim(), out _))
                {
                    return ip.Trim();
                }
            }
            catch { }

            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
        #endregion

        public readonly struct QueuedGodCommand
        {
            public QueuedGodCommand(GodCommandMessage message, string sourceClientId)
            {
                Message = message;
                SourceClientId = sourceClientId;
            }

            public string SourceClientId { get; }
            public GodCommandMessage Message { get; }
        }
    }
}