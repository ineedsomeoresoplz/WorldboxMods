using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(SuccessionTool), nameof(SuccessionTool.findNextHeir))]
    public static class SuccessionToolFindNextHeirPatch
    {
        private static bool Prefix(Kingdom pKingdom, Actor pExculdeActor, ref Actor __result)
        {
            __result = NationTypeManager.SelectHeir(pKingdom, pExculdeActor);
            return false;
        }
    }
}
