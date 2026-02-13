using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(TooltipLibrary), "showKingdom")]
    public static class TooltipLibraryShowKingdomContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            LocalizationContext.Push(pData?.kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showDeadKingdom")]
    public static class TooltipLibraryShowDeadKingdomContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            LocalizationContext.Push(pData?.kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showCity", new[] { typeof(string), typeof(Tooltip), typeof(TooltipData) })]
    public static class TooltipLibraryShowCityContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            LocalizationContext.Push(pData?.city?.kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showActor", new[] { typeof(string), typeof(Tooltip), typeof(TooltipData) })]
    public static class TooltipLibraryShowActorContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            LocalizationContext.Push(pData?.actor?.kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showArmy")]
    public static class TooltipLibraryShowArmyContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TooltipData pData)
        {
            LocalizationContext.Push(pData?.army?.getKingdom());
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(KingdomWindow), "showStatsRows")]
    public static class KingdomWindowShowStatsRowsContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            LocalizationContext.Push(SelectedMetas.selected_kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(CityWindow), "showStatsRows")]
    public static class CityWindowShowStatsRowsContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            LocalizationContext.Push(SelectedMetas.selected_city?.kingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(StatsWindow), "showStatsMetaKingdom", new[] { typeof(Kingdom), typeof(string) })]
    public static class StatsWindowShowStatsMetaKingdomContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Kingdom pKingdom)
        {
            LocalizationContext.Push(pKingdom);
        }

        [HarmonyFinalizer]
        private static System.Exception Finalizer(System.Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }
}
