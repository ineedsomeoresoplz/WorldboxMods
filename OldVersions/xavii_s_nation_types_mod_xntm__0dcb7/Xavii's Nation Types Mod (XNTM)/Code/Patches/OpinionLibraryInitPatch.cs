using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(OpinionLibrary), nameof(OpinionLibrary.init))]
    public static class OpinionLibraryInitPatch
    {
        private static void Postfix(OpinionLibrary __instance)
        {
            NationTypeManager.RegisterTraits();
            NationTypeOpinionBuilder.Register(__instance);
        }
    }
}
