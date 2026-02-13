using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Alliance), nameof(Alliance.join))]
    public static class AllianceJoinConflictPatch
    {
        private static bool Prefix(Alliance __instance, Kingdom pKingdom, ref bool __result)
        {
            if (__instance == null || pKingdom == null || !pKingdom.isAlive())
                return true;
            if (BetterWarsManager.CanJoinAlliance(pKingdom, __instance))
                return true;
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(AllianceManager), nameof(AllianceManager.newAlliance))]
    public static class AllianceManagerNewAllianceConflictPatch
    {
        private static bool Prefix(Kingdom pKingdom, Kingdom pKingdom2, ref Alliance __result)
        {
            if (BetterWarsManager.AreKingdomsAllianceCompatible(pKingdom, pKingdom2))
                return true;
            __result = null;
            return false;
        }
    }
}
