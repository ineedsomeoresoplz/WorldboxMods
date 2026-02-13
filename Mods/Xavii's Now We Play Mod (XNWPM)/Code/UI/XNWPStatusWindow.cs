using System;
using System.Collections.Generic;
using UnityEngine;
using XaviiNowWePlayMod.Code.Networking;

namespace XaviiNowWePlayMod.Code.UI
{
    public class XNWPMStatusWindow : MonoBehaviour
    {
        private const int MaxLogEntries = 16;
        private const int MaxPlayersMin = 2;
        private const int MaxPlayersMax = 16;
        private static readonly Vector2 WindowSize = new Vector2(320f, 310f);

        private XNWPMManager _manager;
        private XNWPMNetworkManager _networkManager;
        private Rect _windowRect;
        private string _maxPlayers = XNWPMNetworkManager.DefaultMaxPlayers.ToString();
        private string _joinCode = string.Empty;
        private Vector2 _logScroll;
        private readonly List<string> _logEntries = new List<string>();
        private bool _initialized;
        private bool _eventsAttached;
        private string _sessionInfoLabel = "No session";
        private Vector2 _memberScroll;
        private Vector2 _windowScroll;

        internal static XNWPMStatusWindow Create(XNWPMManager manager, XNWPMNetworkManager networkManager)
        {
            var root = new GameObject("XNWPMStatusWindow");
            DontDestroyOnLoad(root);
            var window = root.AddComponent<XNWPMStatusWindow>();
            window.Initialize(manager, networkManager);
            return window;
        }

        public void DestroyWindow()
        {
            if (gameObject == null)
            {
                return;
            }

            Destroy(gameObject);
        }

        private void Initialize(XNWPMManager manager, XNWPMNetworkManager networkManager)
        {
            _manager = manager;
            _networkManager = networkManager;
            _windowRect = new Rect(Mathf.Max(Screen.width - WindowSize.x - 20f, 10f), 20f, WindowSize.x, WindowSize.y);
            _initialized = true;
            AppendLog("XNWPM overlay initialized.");
            AttachNetworkEvents();
        }

        private void OnEnable()
        {
            AttachNetworkEvents();
        }

        private void OnDisable()
        {
            DetachNetworkEvents();
        }

        private void AttachNetworkEvents()
        {
            if (_networkManager == null || _eventsAttached)
            {
                return;
            }

            _networkManager.OnLog += HandleLog;
            _networkManager.OnStatusChanged += HandleStatusChanged;
            _networkManager.OnSessionInfoChanged += HandleSessionInfoChanged;
            _eventsAttached = true;
        }

        private void DetachNetworkEvents()
        {
            if (_networkManager == null || !_eventsAttached)
            {
                return;
            }

            _networkManager.OnLog -= HandleLog;
            _networkManager.OnStatusChanged -= HandleStatusChanged;
            _networkManager.OnSessionInfoChanged -= HandleSessionInfoChanged;
            _eventsAttached = false;
        }

        private void HandleLog(string line)
        {
            AppendLog(line);
        }

        private void HandleStatusChanged(string status)
        {
            AppendLog($"Status: {status}");
        }

        private void HandleSessionInfoChanged(string info)
        {
            _sessionInfoLabel = string.IsNullOrWhiteSpace(info) ? "No session" : info;
        }

        private void AppendLog(string message)
        {
            _logEntries.Add($"{DateTime.Now:HH:mm:ss} {message}");
            if (_logEntries.Count > MaxLogEntries)
            {
                _logEntries.RemoveAt(0);
            }

            _logScroll.y = float.MaxValue;
        }

        private void OnGUI()
        {
            if (!_initialized)
            {
                return;
            }

            _windowRect = GUI.Window(GetHashCode(), _windowRect, DrawWindow, "XNWPM");
            ClampWindow();
        }

        private void DrawWindow(int windowId)
        {
            GUILayout.BeginVertical("box");
            _windowScroll = GUILayout.BeginScrollView(_windowScroll, GUILayout.Height(WindowSize.y - 28f));
            GUILayout.Label($"Status: {_networkManager?.Status ?? "idle"}");
            GUILayout.Label($"Session: {_sessionInfoLabel}");
            GUILayout.Space(6f);

            GUILayout.Label("Host with join code", GetLabelStyle());
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max players", GUILayout.Width(90f));
            _maxPlayers = GUILayout.TextField(_maxPlayers, GUILayout.Width(60f));
            GUILayout.EndHorizontal();

            if (_networkManager?.IsHost == true)
            {
                if (GUILayout.Button("Stop hosting"))
                {
                    _networkManager.StopHosting();
                }

                if (GUILayout.Button("Copy join code"))
                {
                    GUIUtility.systemCopyBuffer = _networkManager.JoinCode ?? string.Empty;
                    AppendLog("Join code copied to clipboard.");
                }
            }
            else if (GUILayout.Button("Create session"))
            {
                if (int.TryParse(_maxPlayers, out int players))
                {
                    players = Mathf.Clamp(players, MaxPlayersMin, MaxPlayersMax);
                    _maxPlayers = players.ToString();
                    _networkManager?.StartHosting(players);
                }
                else
                {
                    AppendLog("Max players must be a number.");
                }
            }

            GUILayout.Space(5f);
            GUILayout.Label("Join a session", GetLabelStyle());
            GUILayout.BeginHorizontal();
            GUILayout.Label("Join code", GUILayout.Width(70f));
            _joinCode = GUILayout.TextField(_joinCode, GUILayout.Width(170f));
            GUILayout.EndHorizontal();

            string joinLabel = _networkManager?.IsClient == true ? "Leave session" : "Join session";
            if (GUILayout.Button(joinLabel))
            {
                if (_networkManager?.IsClient == true)
                {
                    _networkManager.Disconnect();
                }
                else if (!string.IsNullOrWhiteSpace(_joinCode))
                {
                    if (!_networkManager.JoinSession(_joinCode.Trim()))
                    {
                        AppendLog("Enter a valid join code (copied from host).");
                    }
                }
                else
                {
                    AppendLog("Enter a join code to connect.");
                }
            }

            GUILayout.Space(6f);
            GUILayout.Label("Session members", GetLabelStyle());
            var members = _networkManager?.CurrentMembers ?? Array.Empty<(string, string, bool)>();
            _memberScroll = GUILayout.BeginScrollView(_memberScroll, GUILayout.Height(90f));
            if (members.Count > 0)
            {
                foreach (var member in members)
                {
                    string name = string.IsNullOrWhiteSpace(member.Name) ? member.ClientId : member.Name;
                    string role = member.IsHost ? "Host" : "Player";
                    GUILayout.Label($"{name} [{role}]");
                }
            }
            else
            {
                GUILayout.Label("No players joined yet.");
            }
            GUILayout.EndScrollView();

            GUILayout.Space(8f);
            GUILayout.Label("Connection Log", GetLabelStyle());
            _logScroll = GUILayout.BeginScrollView(_logScroll, GUILayout.Height(90f));
            foreach (string entry in _logEntries)
            {
                GUILayout.Label(entry);
            }
            GUILayout.EndScrollView();

            GUILayout.Space(6f);
            GUILayout.Label("Tips", GetLabelStyle());
            GUILayout.Label("Share the join code shown above; the host auto-maps port 29101 via UPnP/NAT-PMP when possible.");
            GUILayout.Label("If the router blocks mapping, the host may still need a one-time port forward.");
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, WindowSize.x, 20f));
        }

        private void ClampWindow()
        {
            _windowRect.x = Mathf.Clamp(_windowRect.x, 0f, Screen.width - _windowRect.width);
            _windowRect.y = Mathf.Clamp(_windowRect.y, 0f, Screen.height - _windowRect.height);
        }

        private GUIStyle GetLabelStyle()
        {
            return new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
        }
    }
}
