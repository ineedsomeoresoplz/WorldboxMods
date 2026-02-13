using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(City), "makeOwnKingdom")]
    public static class CityRebellionBirthPatch
    {
        private static void Prefix(City __instance, bool pRebellion)
        {
            if (!pRebellion)
                return;
            if (__instance?.kingdom == null)
                return;
            NationTypeManager.PrepareRebellionBirth(__instance.kingdom);
        }

        private static void Postfix(bool pRebellion)
        {
            if (!pRebellion)
                return;
            NationTypeManager.ResetPendingRebellion();
        }
    }
}
