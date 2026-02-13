using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(City), "finishCapture")]
    public static class BetterWarsCityFinishCapturePatch
    {
        private struct CaptureState
        {
            public Kingdom OldOwner;
            public bool Skipped;
        }

        private static bool Prefix(City __instance, Kingdom pNewKingdom, ref CaptureState __state)
        {
            __state = default;
            if (__instance == null)
                return true;

            __state.OldOwner = __instance.kingdom;
            if (!BetterWarsManager.BeforeCityCapture(__instance, __state.OldOwner, pNewKingdom))
            {
                __state.Skipped = true;
                return false;
            }

            return true;
        }

        private static void Postfix(City __instance, Kingdom pNewKingdom, CaptureState __state)
        {
            if (__state.Skipped)
                return;
            BetterWarsManager.OnCityCaptured(__instance, __state.OldOwner, pNewKingdom);
        }
    }

    [HarmonyPatch(typeof(City), nameof(City.save))]
    public static class BetterWarsCitySavePatch
    {
        private static void Postfix(City __instance)
        {
            BetterWarsManager.OnCitySave(__instance);
        }
    }
}
