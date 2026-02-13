using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(KingdomManager), nameof(KingdomManager.makeNewCivKingdom))]
    public static class KingdomManagerMakeNewCivKingdomPatch
    {
        private static void Postfix()
        {
            LocalizationPrewarmer.Prewarm();
        }
    }
}
