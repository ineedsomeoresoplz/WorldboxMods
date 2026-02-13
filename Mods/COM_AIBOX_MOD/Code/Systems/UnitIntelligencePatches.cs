using HarmonyLib;
using UnityEngine;

namespace AIBox
{
    [HarmonyPatch(typeof(Actor))]
    public static class UnitIntelligencePatches
    {
        // Hook into setAsset, which is always called during initialization
        [HarmonyPatch("setAsset")]
        [HarmonyPostfix]
        public static void Postfix_setAsset(Actor __instance)
        {
            if (__instance == null) return;
            // Register unit when it is initialized
            UnitIntelligenceManager.Instance.RegisterUnit(__instance);
        }

        [HarmonyPatch("Dispose")]
        [HarmonyPrefix]
        public static void Prefix_Dispose(Actor __instance)
        {
             if (__instance == null) return;
             // Unregister unit when it is destroyed to free memory
             UnitIntelligenceManager.Instance.UnregisterUnit(__instance);
        }
    }

}
