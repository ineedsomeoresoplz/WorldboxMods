using HarmonyLib;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Kingdom), nameof(Kingdom.logKingDead))]
    public static class KingdomLogKingDeadPatch
    {
        private static bool Prefix(Kingdom __instance, Actor pActor)
        {
            NationTypeDefinition def = NationTypeManager.GetDefinition(__instance);
            if (def.SuccessionMode == NationSuccessionMode.None)
                return false;

            if (def.SuccessionMode == NationSuccessionMode.Council)
                return false;

            return true;
        }
    }
}
