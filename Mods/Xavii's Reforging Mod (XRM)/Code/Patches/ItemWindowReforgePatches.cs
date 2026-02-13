using HarmonyLib;
using XRM.Code.Content;
using XRM.Code.UI;

namespace XRM.Code.Patches
{
    [HarmonyPatch(typeof(ItemWindow))]
    internal static class ItemWindowReforgePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemWindow.clickReforge))]
        [HarmonyPriority(Priority.First)]
        private static bool ClickReforgePrefix(ItemWindow __instance)
        {
            if (__instance == null || !XrmBuffRegistry.EnsureInitialized())
            {
                return true;
            }

            return !VanillaModifierWindow.Show(__instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemWindow.clickReforgeDivine))]
        [HarmonyPriority(Priority.First)]
        private static bool ClickReforgeDivinePrefix(ItemWindow __instance)
        {
            if (__instance == null || !XrmBuffRegistry.EnsureInitialized())
            {
                return true;
            }

            return !ReforgeSelectionWindow.Show(__instance, 5, true);
        }
    }
}
