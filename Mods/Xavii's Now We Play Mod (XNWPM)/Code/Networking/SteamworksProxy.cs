using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace XaviiNowWePlayMod.Code.Networking
{
    internal sealed class SteamworksProxy
    {
        public readonly struct LobbyMemberInfo
        {
            public LobbyMemberInfo(ulong steamId, string name)
            {
                SteamId = steamId;
                Name = name;
            }

            public ulong SteamId { get; }
            public string Name { get; }
        }

        private static readonly string[] CandidateAssemblyNames =
        {
            "Facepunch.Steamworks.Win64",
            "Facepunch.Steamworks.Posix"
        };

        private readonly Assembly _steamAssembly;
        private readonly Type _steamClientType;
        private readonly Type _steamMatchmakingType;
        private readonly Type _steamNetworkingType;
        private readonly Type _steamIdType;
        private readonly Type _lobbyType;
        private readonly Type _friendType;
        private readonly Type _p2pSendType;

        private readonly MethodInfo _steamClientInit;
        private readonly MethodInfo _steamClientRunCallbacks;
        private readonly MethodInfo _steamClientShutdown;
        private readonly PropertyInfo _steamClientIsValidProp;
        private readonly PropertyInfo _steamClientSteamIdProp;

        private readonly MethodInfo _steamMatchmakingCreateLobbyAsync;
        private readonly MethodInfo _steamMatchmakingJoinLobbyAsync;
        private readonly MethodInfo _steamMatchmakingInstallEvents;

        private readonly MethodInfo _steamNetworkingInstallEvents;
        private readonly MethodInfo _steamNetworkingAllowP2PPacketRelay;
        private readonly MethodInfo _steamNetworkingIsP2PPacketAvailable;
        private readonly MethodInfo _steamNetworkingReadP2PPacket;
        private readonly MethodInfo _steamNetworkingSendP2PPacket;
        private readonly MethodInfo _steamNetworkingAcceptP2PSessionWithUser;

        private readonly PropertyInfo _lobbyIdProp;
        private readonly PropertyInfo _lobbyOwnerProp;
        private readonly PropertyInfo _lobbyMembersProp;
        private readonly PropertyInfo _lobbyMemberCountProp;
        private readonly PropertyInfo _lobbyMaxMembersProp;
        private readonly MethodInfo _lobbySetDataMethod;
        private readonly MethodInfo _lobbyGetDataMethod;
        private readonly MethodInfo _lobbyLeaveMethod;

        private readonly PropertyInfo _friendNameProp;
        private readonly PropertyInfo _friendIdProp;

        private readonly FieldInfo _steamIdValueField;
        private readonly PropertyInfo _steamIdIsValidProp;

        private readonly object _p2pSendReliableValue;

        public bool IsAvailable { get; }
        public bool IsInitialized { get; private set; }

        public SteamworksProxy()
        {
            _steamAssembly = LoadSteamAssembly();
            if (_steamAssembly == null)
            {
                return;
            }

            _steamClientType = _steamAssembly.GetType("Steamworks.SteamClient");
            _steamMatchmakingType = _steamAssembly.GetType("Steamworks.SteamMatchmaking");
            _steamNetworkingType = _steamAssembly.GetType("Steamworks.SteamNetworking");
            _steamIdType = _steamAssembly.GetType("Steamworks.SteamId");
            _lobbyType = _steamAssembly.GetType("Steamworks.Data.Lobby");
            _friendType = _steamAssembly.GetType("Steamworks.Friend");
            _p2pSendType = _steamAssembly.GetType("Steamworks.P2PSend");

            if (_steamClientType == null || _steamMatchmakingType == null || _steamNetworkingType == null ||
                _steamIdType == null || _lobbyType == null || _friendType == null)
            {
                return;
            }

            _steamClientInit = _steamClientType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
            _steamClientRunCallbacks = _steamClientType.GetMethod("RunCallbacks", BindingFlags.Public | BindingFlags.Static);
            _steamClientShutdown = _steamClientType.GetMethod("Shutdown", BindingFlags.Public | BindingFlags.Static);
            _steamClientIsValidProp = _steamClientType.GetProperty("IsValid", BindingFlags.Public | BindingFlags.Static);
            _steamClientSteamIdProp = _steamClientType.GetProperty("SteamId", BindingFlags.Public | BindingFlags.Static);

            _steamMatchmakingCreateLobbyAsync = _steamMatchmakingType.GetMethod("CreateLobbyAsync", BindingFlags.Public | BindingFlags.Static);
            _steamMatchmakingJoinLobbyAsync = _steamMatchmakingType.GetMethod("JoinLobbyAsync", BindingFlags.Public | BindingFlags.Static);
            _steamMatchmakingInstallEvents = _steamMatchmakingType.GetMethod("InstallEvents", BindingFlags.Public | BindingFlags.Static);

            _steamNetworkingInstallEvents = _steamNetworkingType.GetMethod("InstallEvents", BindingFlags.Public | BindingFlags.Static);
            _steamNetworkingAllowP2PPacketRelay = _steamNetworkingType.GetMethod("AllowP2PPacketRelay", BindingFlags.Public | BindingFlags.Static);
            _steamNetworkingIsP2PPacketAvailable = _steamNetworkingType.GetMethod("IsP2PPacketAvailable", BindingFlags.Public | BindingFlags.Static);
            _steamNetworkingReadP2PPacket = FindStaticMethod(_steamNetworkingType, "ReadP2PPacket", parameters =>
                parameters.Length == 4 &&
                parameters[0].ParameterType == typeof(byte[]) &&
                parameters[1].ParameterType.IsByRef &&
                parameters[1].ParameterType.GetElementType() == typeof(uint) &&
                parameters[2].ParameterType.IsByRef &&
                parameters[2].ParameterType.GetElementType() == _steamIdType &&
                parameters[3].ParameterType == typeof(int));

            Type? sendEnumType = _p2pSendType;
            _steamNetworkingSendP2PPacket = sendEnumType != null
                ? FindStaticMethod(_steamNetworkingType, "SendP2PPacket", parameters =>
                    parameters.Length == 5 &&
                    parameters[0].ParameterType == _steamIdType &&
                    parameters[1].ParameterType == typeof(byte[]) &&
                    parameters[2].ParameterType == typeof(int) &&
                    parameters[3].ParameterType == typeof(int) &&
                    parameters[4].ParameterType == sendEnumType)
                : _steamNetworkingType.GetMethod("SendP2PPacket", BindingFlags.Public | BindingFlags.Static);

            _steamNetworkingAcceptP2PSessionWithUser = _steamNetworkingType.GetMethod("AcceptP2PSessionWithUser", BindingFlags.Public | BindingFlags.Static);

            _lobbyIdProp = _lobbyType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            _lobbyOwnerProp = _lobbyType.GetProperty("Owner", BindingFlags.Public | BindingFlags.Instance);
            _lobbyMembersProp = _lobbyType.GetProperty("Members", BindingFlags.Public | BindingFlags.Instance);
            _lobbyMemberCountProp = _lobbyType.GetProperty("MemberCount", BindingFlags.Public | BindingFlags.Instance);
            _lobbyMaxMembersProp = _lobbyType.GetProperty("MaxMembers", BindingFlags.Public | BindingFlags.Instance);
            _lobbySetDataMethod = _lobbyType.GetMethod("SetData", BindingFlags.Public | BindingFlags.Instance);
            _lobbyGetDataMethod = _lobbyType.GetMethod("GetData", BindingFlags.Public | BindingFlags.Instance);
            _lobbyLeaveMethod = _lobbyType.GetMethod("Leave", BindingFlags.Public | BindingFlags.Instance);

            _friendNameProp = _friendType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
            _friendIdProp = _friendType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

            _steamIdValueField = _steamIdType.GetField("Value", BindingFlags.Public | BindingFlags.Instance);
            _steamIdIsValidProp = _steamIdType.GetProperty("IsValid", BindingFlags.Public | BindingFlags.Instance);

            if (_p2pSendType != null)
            {
                _p2pSendReliableValue = Enum.Parse(_p2pSendType, "Reliable");
            }

            IsAvailable = _steamClientInit != null && _steamClientRunCallbacks != null && _steamClientIsValidProp != null &&
                          _steamClientSteamIdProp != null;
        }

        public bool Initialize(uint appId)
        {
            if (!IsAvailable || _steamClientInit == null)
            {
                return false;
            }

            try
            {
                _steamClientInit.Invoke(null, new object[] { appId, true });
                _steamMatchmakingInstallEvents?.Invoke(null, null);
                _steamNetworkingInstallEvents?.Invoke(null, new object[] { true });
                _steamNetworkingAllowP2PPacketRelay?.Invoke(null, new object[] { true });
                IsInitialized = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsSteamClientValid()
        {
            return IsAvailable && _steamClientIsValidProp != null && (bool)_steamClientIsValidProp.GetValue(null);
        }

        public void RunCallbacks()
        {
            _steamClientRunCallbacks?.Invoke(null, null);
        }

        public void Shutdown()
        {
            _steamClientShutdown?.Invoke(null, null);
            IsInitialized = false;
        }

        public ulong GetLocalSteamId()
        {
            if (!IsAvailable || _steamClientSteamIdProp == null)
            {
                return 0;
            }

            object value = _steamClientSteamIdProp.GetValue(null);
            return value != null ? GetSteamIdValue(value) : 0;
        }

        public async Task<object?> CreateLobbyAsync(int maxPlayers)
        {
            object? task = _steamMatchmakingCreateLobbyAsync?.Invoke(null, new object[] { maxPlayers });
            return await AwaitLobbyTask(task).ConfigureAwait(false);
        }

        public async Task<object?> JoinLobbyAsync(ulong lobbyId)
        {
            object? steamId = CreateSteamId(lobbyId);
            object? task = _steamMatchmakingJoinLobbyAsync?.Invoke(null, new object[] { steamId });
            return await AwaitLobbyTask(task).ConfigureAwait(false);
        }

        public IReadOnlyList<LobbyMemberInfo> GetLobbyMembers(object lobby)
        {
            if (_lobbyMembersProp == null)
            {
                return Array.Empty<LobbyMemberInfo>();
            }

            var members = _lobbyMembersProp.GetValue(lobby) as IEnumerable;
            if (members == null)
            {
                return Array.Empty<LobbyMemberInfo>();
            }

            List<LobbyMemberInfo> result = new();
            foreach (object member in members)
            {
                ulong steamId = GetFriendSteamId(member);
                string name = GetFriendName(member);
                result.Add(new LobbyMemberInfo(steamId, name));
            }

            return result;
        }

        public ulong? GetLobbyOwnerId(object lobby)
        {
            if (_lobbyOwnerProp == null)
            {
                return null;
            }

            object owner = _lobbyOwnerProp.GetValue(lobby);
            if (owner == null)
            {
                return null;
            }

            return GetFriendSteamId(owner);
        }

        public ulong GetLobbyId(object lobby)
        {
            if (_lobbyIdProp == null)
            {
                return 0;
            }

            object steamId = _lobbyIdProp.GetValue(lobby);
            return steamId != null ? GetSteamIdValue(steamId) : 0;
        }

        public int GetLobbyMemberCount(object lobby)
        {
            if (_lobbyMemberCountProp == null)
            {
                return 0;
            }

            return (int)((_lobbyMemberCountProp.GetValue(lobby) as int?) ?? 0);
        }

        public int GetLobbyMaxMembers(object lobby)
        {
            if (_lobbyMaxMembersProp == null)
            {
                return 0;
            }

            return (int)((_lobbyMaxMembersProp.GetValue(lobby) as int?) ?? 0);
        }

        public string? GetLobbyData(object lobby, string key)
        {
            if (_lobbyGetDataMethod == null)
            {
                return null;
            }

            return _lobbyGetDataMethod.Invoke(lobby, new object[] { key }) as string;
        }

        public bool SetLobbyData(object lobby, string key, string value)
        {
            if (_lobbySetDataMethod == null)
            {
                return false;
            }

            return (bool)_lobbySetDataMethod.Invoke(lobby, new object[] { key, value });
        }

        public void LeaveLobby(object lobby)
        {
            _lobbyLeaveMethod?.Invoke(lobby, null);
        }

        public bool IsP2PPacketAvailable()
        {
            if (_steamNetworkingIsP2PPacketAvailable == null)
            {
                return false;
            }

            return (bool)_steamNetworkingIsP2PPacketAvailable.Invoke(null, new object[] { 0 });
        }

        public bool TryReadP2PPacket(byte[] buffer, out uint size, out ulong senderSteamId)
        {
            size = 0;
            senderSteamId = 0;

            if (_steamNetworkingReadP2PPacket == null || _steamIdType == null)
            {
                return false;
            }

            object? steamId = CreateSteamId(0);
            object[] args = { buffer, (uint)buffer.Length, steamId, 0 };
            bool success = (bool)_steamNetworkingReadP2PPacket.Invoke(null, args);
            size = args[1] is uint readSize ? readSize : 0;
            if (args[2] != null)
            {
                senderSteamId = GetSteamIdValue(args[2]);
            }

            return success && size > 0;
        }

        public bool SendP2PPacket(ulong targetSteamId, byte[] payload, int length)
        {
            if (_steamNetworkingSendP2PPacket == null || _p2pSendReliableValue == null)
            {
                return false;
            }

            object steamId = CreateSteamId(targetSteamId);
            object[] args = { steamId, payload, length, 0, _p2pSendReliableValue };
            return (bool)_steamNetworkingSendP2PPacket.Invoke(null, args);
        }

        public void AcceptP2PSession(ulong steamId)
        {
            if (_steamNetworkingAcceptP2PSessionWithUser == null)
            {
                return;
            }

            object target = CreateSteamId(steamId);
            _steamNetworkingAcceptP2PSessionWithUser.Invoke(null, new object[] { target });
        }

        private static Assembly? LoadSteamAssembly()
        {
            foreach (string assemblyName in CandidateAssemblyNames)
            {
                Assembly? loaded = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a =>
                    string.Equals(a.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase));
                if (loaded != null)
                {
                    return loaded;
                }
            }

            foreach (string assemblyName in CandidateAssemblyNames)
            {
                try
                {
                    return Assembly.Load(assemblyName);
                }
                catch
                {
                    
                }

                string folder = GetAssembliesFolder();
                if (!string.IsNullOrEmpty(folder))
                {
                    string path = Path.Combine(folder, $"{assemblyName}.dll");
                    if (File.Exists(path))
                    {
                        try
                        {
                            return Assembly.LoadFrom(path);
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }

            return null;
        }

        private static string GetAssembliesFolder()
        {
            string streamingAssets = Application.streamingAssetsPath;
            if (string.IsNullOrEmpty(streamingAssets))
            {
                return string.Empty;
            }

            string nativeMods = Path.Combine(streamingAssets, "mods");
            string nmlPath = Path.Combine(nativeMods, "NML");
            string assemblies = Path.Combine(nmlPath, "Assemblies");
            return Path.GetFullPath(assemblies);
        }

        private object CreateSteamId(ulong value)
        {
            object steamId = Activator.CreateInstance(_steamIdType)!;
            _steamIdValueField?.SetValue(steamId, value);
            return steamId;
        }

        private ulong GetSteamIdValue(object steamId)
        {
            if (_steamIdValueField == null)
            {
                return 0;
            }

            return (ulong)(_steamIdValueField.GetValue(steamId) ?? 0UL);
        }

        private ulong GetFriendSteamId(object friend)
        {
            if (friend == null || _friendIdProp == null)
            {
                return 0;
            }

            object value = _friendIdProp.GetValue(friend);
            return value != null ? GetSteamIdValue(value) : 0;
        }

        private string GetFriendName(object friend)
        {
            if (friend == null || _friendNameProp == null)
            {
                return string.Empty;
            }

            return _friendNameProp.GetValue(friend) as string ?? string.Empty;
        }

        private static async Task<object?> AwaitLobbyTask(object? taskObj)
        {
            if (taskObj is not Task task)
            {
                return null;
            }

            await task.ConfigureAwait(false);
            PropertyInfo? resultProp = task.GetType().GetProperty("Result");
            return resultProp?.GetValue(task);
        }

        private static MethodInfo? FindStaticMethod(Type type, string name, Func<ParameterInfo[], bool> predicate)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(method => method.Name == name && predicate(method.GetParameters()));
        }
    }
}
