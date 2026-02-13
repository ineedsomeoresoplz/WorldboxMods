using System;
using System.IO;
using HarmonyLib;
using UnityEngine;

namespace XaviiNowWePlayMod.Code.Features
{
    public static class WorldSyncGuard
    {
        private static string _currentWorldId;

        public static string CurrentWorldId => _currentWorldId;

        public static event Action<string> OnWorldLoadRequested;
        public static event Action<string> OnWorldLoaded;

        internal static void NotifyWorldLoadRequested(string worldId)
        {
            if (string.IsNullOrEmpty(worldId))
            {
                return;
            }

            OnWorldLoadRequested?.Invoke(worldId);
        }

        internal static void NotifyWorldLoaded(string worldId)
        {
            if (string.IsNullOrEmpty(worldId))
            {
                return;
            }

            _currentWorldId = worldId;
            OnWorldLoaded?.Invoke(worldId);
        }

        public static void RefreshCurrentWorldId()
        {
            _currentWorldId = BuildWorldIdentifier(null);
        }

        internal static string BuildWorldIdentifier(string explicitPath)
        {
            
            
            MapStats stats = World.world?.map_stats;
            string worldName = null;

            
            if (stats != null && !string.IsNullOrWhiteSpace(stats.name))
            {
                worldName = stats.name.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(explicitPath))
            {
                worldName = Path.GetFileNameWithoutExtension(explicitPath);
            }
            else if (!string.IsNullOrWhiteSpace(SaveManager.currentSavePath))
            {
                worldName = Path.GetFileNameWithoutExtension(SaveManager.currentSavePath);
            }

            string dna = stats != null ? stats.life_dna.ToString() : "dna_unknown";
            string age = string.IsNullOrWhiteSpace(stats?.world_age_id) ? "age_unknown" : stats.world_age_id;
            string size = $"{MapBox.width}x{MapBox.height}";

            if (!string.IsNullOrWhiteSpace(worldName))
            {
                return $"save:{worldName.ToLowerInvariant()}:{dna}:{age}:{size}";
            }

            return $"meta:{dna}:{age}:{size}";
        }

    }

    [HarmonyPatch(typeof(SaveManager))]
    internal static class SaveManagerLoadWorldPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("loadWorld", new Type[] { typeof(string), typeof(bool) })]
        private static void Prefix(string pPath)
        {
            WorldSyncGuard.NotifyWorldLoadRequested(WorldSyncGuard.BuildWorldIdentifier(pPath));
        }

        [HarmonyPostfix]
        [HarmonyPatch("loadWorld", new Type[] { typeof(string), typeof(bool) })]
        private static void Postfix(string pPath)
        {
            WorldSyncGuard.NotifyWorldLoaded(WorldSyncGuard.BuildWorldIdentifier(pPath));
        }
    }

    [HarmonyPatch(typeof(SaveManager))]
    [HarmonyPatch("loadWorld", new Type[0])]
    internal static class SaveManagerLoadCurrentWorldPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            WorldSyncGuard.NotifyWorldLoadRequested(WorldSyncGuard.BuildWorldIdentifier(null));
        }

        [HarmonyPostfix]
        private static void Postfix()
        {
            WorldSyncGuard.NotifyWorldLoaded(WorldSyncGuard.BuildWorldIdentifier(null));
        }
    }
}
