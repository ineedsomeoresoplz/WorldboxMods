using HarmonyLib;
using UnityEngine.UI;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(LocalizedTextManager), nameof(LocalizedTextManager.getText), new[] { typeof(string), typeof(Text), typeof(bool) })]
    public static class LocalizedTextManagerPatch
    {
        private static void Postfix(string pKey, Text text, bool pForceEnglish, ref string __result)
        {
            LocalizedTextManagerPatchShared.Apply(pKey, pForceEnglish, ref __result);
        }
    }

    internal static class LocalizedTextManagerPatchShared
    {
        internal static void Apply(string pKey, bool pForceEnglish, ref string __result)
        {
            if (string.IsNullOrEmpty(pKey) || pForceEnglish)
                return;

            switch (pKey)
            {
                case "kingdoms":
                    __result = LocalizedTextManager.stringExists("xntm_nations_label")
                        ? LocalizedTextManager.getText("xntm_nations_label")
                        : "Nations";
                    return;
                case "statistics_kingdom_cities":
                    __result = "Number of Lands";
                    return;
                case "statistics_cities_description":
                    __result = "Browse all the lands scattered across your realm";
                    return;
                case "statistics_villages":
                    __result = "Lands";
                    return;
                case "statistics_villages_description":
                    __result = "Show lands of this nation";
                    return;
                case "creature_statistics_home_village":
                    __result = "Hometown";
                    return;
                case "villages":
                    __result = "Lands";
                    return;
                case "kingdom_traits":
                    __result = "Nation Traits";
                    return;
                case "kingdom_editor":
                    __result = "Nation Editor";
                    return;
            }

            Kingdom context = ResolveContext();
            if (context == null)
                return;

            NationTypeDefinition def = NationTypeManager.GetDefinition(context);
            if (def == null)
                return;

            switch (pKey)
            {
                case "kingdom":
                    __result = def.GetLocalizedName();
                    break;
                case "king":
                case "village_statistics_king":
                    __result = def.GetLocalizedRulerTitle();
                    break;
                case "kings":
                    __result = $"{def.GetLocalizedRulerTitle()}s";
                    break;
                case "ruler_money":
                    __result = $"{def.GetLocalizedRulerTitle()}'s Money";
                    break;
            }
        }

        private static Kingdom ResolveContext()
        {
            Kingdom context = LocalizationContext.CurrentKingdom;
            if (context != null)
                return context;

            if (Tooltip.anyActive())
                return null;

            ScrollWindow window = ScrollWindow.getCurrentWindow();
            string windowId = window?.screen_id;
            if (string.IsNullOrEmpty(windowId))
                windowId = WindowOpenContext.CurrentWindowId;

            if (windowId == "kingdom")
                return SelectedMetas.selected_kingdom;

            if (windowId == "city")
                return SelectedMetas.selected_city?.kingdom;

            return null;
        }
    }
}
