using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WarListElement), "show", new[] { typeof(War) })]
    public static class WarListElementShowReasonContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(War pWar)
        {
            WarLocalizationContext.Push(pWar);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            WarLocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(WarWindow), "showStatsRows")]
    public static class WarWindowShowStatsRowsReasonContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            WarLocalizationContext.Push(SelectedMetas.selected_war);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            WarLocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showWar", new[] { typeof(Tooltip), typeof(string), typeof(TooltipData) })]
    public static class TooltipLibraryShowWarReasonContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            WarLocalizationContext.Push(pData?.war);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            WarLocalizationContext.Pop();
            return __exception;
        }
    }
}
