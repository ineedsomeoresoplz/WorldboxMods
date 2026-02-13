using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Kingdom), "newCivKingdom")]
    public static class KingdomNewCivKingdomPatch
    {
        private static void Postfix(Kingdom __instance)
        {
            NationTypeManager.EnsureType(__instance);
            NationNamingHelper.ApplyAccurateName(__instance, __instance.data?.name, true);
        }
    }

    [HarmonyPatch(typeof(Kingdom), "load2")]
    public static class KingdomLoadPatch
    {
        private static void Postfix(Kingdom __instance)
        {
            NationTypeManager.EnsureType(__instance);
            NationNamingHelper.ApplyAccurateName(__instance, __instance.data?.name);
        }
    }
}
