using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Kingdom), nameof(Kingdom.updateCiv))]
    public static class KingdomAutoNationTypePatch
    {
        private static void Postfix(Kingdom __instance)
        {
            NationTypeManager.TickAuto(__instance);
        }
    }
}
