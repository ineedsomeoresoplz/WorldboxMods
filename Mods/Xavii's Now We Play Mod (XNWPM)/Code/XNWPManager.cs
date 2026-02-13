using UnityEngine;
using XaviiNowWePlayMod.Code.Features;
using XaviiNowWePlayMod.Code.Networking;
using XaviiNowWePlayMod.Code.UI;

namespace XaviiNowWePlayMod.Code
{
    public class XNWPMManager : MonoBehaviour
    {
        public static XNWPMManager Instance { get; private set; }

        internal XNWPMNetworkManager NetworkManager => _networkManager;

        private XNWPMNetworkManager _networkManager;
        private XNWPMStatusWindow _statusWindow;
        private string _trackedWorldId;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            WorldSyncGuard.OnWorldLoaded += HandleWorldLoaded;
            WorldSyncGuard.RefreshCurrentWorldId();
            _networkManager = new XNWPMNetworkManager();
            _statusWindow = XNWPMStatusWindow.Create(this, _networkManager);
            HandleWorldLoaded(WorldSyncGuard.CurrentWorldId);
        }

        private void Update()
        {
            if (_networkManager == null)
            {
                return;
            }

            _networkManager.Tick();

            while (_networkManager.TryDequeueCommand(out var entry))
            {
                if (GodCommandExecutor.Execute(entry.Message) && _networkManager.IsHost)
                {
                    _networkManager.BroadcastToClients(entry.Message, entry.SourceClientId);
                }
            }
        }

        internal void ReportLocalCommand(Vector2Int target, string powerId)
        {
            if (_networkManager == null || string.IsNullOrWhiteSpace(powerId) || !_networkManager.HasSession)
            {
                return;
            }

            var command = new GodCommandMessage(powerId, target, SystemInfo.deviceName ?? "Player");

            if (_networkManager.IsHost)
            {
                _networkManager.BroadcastToClients(command);
            }
            else if (_networkManager.IsClient)
            {
                _networkManager.SendCommandToHost(command);
            }
        }

        private void OnDestroy()
        {
            WorldSyncGuard.OnWorldLoaded -= HandleWorldLoaded;
            _statusWindow?.DestroyWindow();
            _networkManager?.Dispose();
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void HandleWorldLoaded(string worldId)
        {
            if (string.IsNullOrWhiteSpace(worldId))
            {
                return;
            }

            _trackedWorldId = worldId;

            if (_networkManager == null)
            {
                return;
            }

            if (_networkManager.IsHost)
            {
                if (!string.IsNullOrWhiteSpace(_networkManager.SessionWorldId) && _networkManager.SessionWorldId != worldId)
                {
                    _networkManager.AppendLog("Host changed to a different world; stopping the lobby to keep everyone synced.");
                    _networkManager.StopHosting();
                    return;
                }

                _networkManager.UpdateSessionWorldId(worldId, true);
            }
            else if (_networkManager.IsClient)
            {
                if (!string.IsNullOrWhiteSpace(_networkManager.SessionWorldId) && _networkManager.SessionWorldId != worldId)
                {
                    _networkManager.AppendLog("Local world change detected while in a lobby; leaving to avoid desync.");
                    _networkManager.Disconnect();
                }
            }
        }
    }
}
