using HarmonyLib;
using XaviiBetterTimeMod.Code.Managers;

namespace XaviiBetterTimeMod.Code.Features
{
    internal static class TimePatches
    {
        [HarmonyPatch(typeof(MapStats), nameof(MapStats.updateWorldTime))]
        private static class MapStatsUpdateWorldTime
        {
            private static void Prefix(ref float pElapsed)
            {
                pElapsed *= BetterTimeManager.WorldTimeScaleFactor;
            }
        }

        [HarmonyPatch(typeof(WorldAgeManager), nameof(WorldAgeManager.startNextAge))]
        private static class WorldAgeManagerStartNextAge
        {
            private static bool Prefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(WorldAgeManager), nameof(WorldAgeManager.setCurrentAge))]
        private static class WorldAgeManagerSetCurrentAge
        {
            private static void Prefix(ref WorldAgeAsset pAsset, ref bool pOverrideTime)
            {
                if (BetterTimeManager.Instance != null && BetterTimeManager.Instance.TryGetCurrentCycleAsset(out var overrideAsset))
                {
                    if (overrideAsset != null)
                    {
                        pAsset = overrideAsset;
                    }
                }

                pOverrideTime = false;
            }

            private static void Postfix()
            {
                if (BetterTimeManager.Instance == null || World.world?.map_stats == null)
                    return;

                World.world.map_stats.current_world_ages_duration = (float)BetterTimeManager.DayLengthWorldTime;
            }
        }
    }
}
