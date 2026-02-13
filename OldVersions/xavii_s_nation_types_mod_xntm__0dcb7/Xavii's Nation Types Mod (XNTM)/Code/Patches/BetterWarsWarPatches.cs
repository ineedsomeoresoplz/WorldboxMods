using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WarManager), nameof(WarManager.newWar))]
    public static class BetterWarsWarStartPatch
    {
        private static void Postfix(War __result, Kingdom pAttacker, Kingdom pDefender)
        {
            BetterWarsManager.OnWarStarted(__result, pAttacker, pDefender);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.newWar))]
    public static class BetterWarsWarGuardPatch
    {
        private static bool Prefix(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pType, ref War __result)
        {
            if (pAttacker == null)
                return true;

            
            if (pType != null && pType.id == "whisper_of_war")
                return true;

            
            if (BetterWarsManager.IsDemilitarized(pAttacker))
            {
                __result = null;
                return false;
            }

            
            if (BetterWarsManager.TryGetOverlord(pAttacker, out var overlord) && overlord != null && overlord != pDefender)
            {
                __result = null;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(War), nameof(War.update))]
    public static class BetterWarsWarUpdatePatch
    {
        private static void Postfix(War __instance)
        {
            BetterWarsManager.TickWar(__instance);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.endWar))]
    public static class BetterWarsWarEndPatch
    {
        private static void Postfix(War pWar, WarWinner pWinner)
        {
            BetterWarsManager.OnWarEnded(pWar, pWinner);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.update))]
    public static class BetterWarsWarManagerUpdatePatch
    {
        private static void Postfix()
        {
            BetterWarsManager.TickGlobal();
        }
    }
}
