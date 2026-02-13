using System;
using HarmonyLib;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(TooltipLibrary), "showKingdom")]
    public static class TooltipLibraryShowKingdomPatch
    {
        private static void Postfix(Tooltip pTooltip, string pType, TooltipData pData)
        {
            Kingdom kingdom = pData?.kingdom;
            if (kingdom == null)
                return;

            string color = kingdom.getColor().color_text;
            pTooltip.setTitle(kingdom.name, "kingdom", color);
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showCity", new Type[] { typeof(string), typeof(Tooltip), typeof(TooltipData) })]
    public static class TooltipLibraryShowCityPatch
    {
        private static void Postfix(string pTitleID, Tooltip pTooltip, TooltipData pData)
        {
            City city = pData?.city;
            if (city == null)
                return;

            var land = LandTypeManager.EnsureLandType(city);
            string title = LandTypeManager.GetDisplayName(city);
            if (string.IsNullOrWhiteSpace(title))
                title = city.data?.name ?? city.name ?? string.Empty;

            string subtitle = !string.IsNullOrEmpty(land?.DisplayNameKey) ? land.DisplayNameKey : pTitleID;
            string color = city.kingdom?.getColor()?.color_text ?? "#FFFFFF";
            pTooltip.setTitle(title, subtitle, color);
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showKing")]
    public static class TooltipLibraryShowKingPatch
    {
        private static bool Prefix(TooltipLibrary __instance, Tooltip pTooltip, string pType, TooltipData pData)
        {
            Actor actor = pData?.actor;
            Kingdom kingdom = actor?.kingdom;
            NationTypeDefinition def = NationTypeManager.GetDefinition(kingdom);
            string subtitle = def?.GetLocalizedRulerTitle(actor?.isSexFemale() == true) ?? "village_statistics_king";
            var method = AccessTools.Method(typeof(TooltipLibrary), "showActor", new Type[] { typeof(string), typeof(Tooltip), typeof(TooltipData) });
            method?.Invoke(__instance, new object[] { subtitle, pTooltip, pData });
            return false;
        }
    }

    [HarmonyPatch(typeof(TooltipLibrary), "showPastRulers")]
    public static class TooltipLibraryShowPastRulersPatch
    {
        private static void Postfix(Tooltip pTooltip, string pType, TooltipData pData)
        {
            Kingdom kingdom = pData?.kingdom;
            if (kingdom == null)
                kingdom = pData?.city?.kingdom;
            if (kingdom == null)
                kingdom = SelectedMetas.selected_kingdom ?? SelectedMetas.selected_city?.kingdom;
            if (kingdom == null)
                return;
            NationTypeDefinition def = NationTypeManager.GetDefinition(kingdom);
            if (def == null)
                return;
            string rulerTitle = def.GetLocalizedRulerTitle();
            if (string.IsNullOrEmpty(rulerTitle))
                return;
            string baseTitle = LocalizedTextManager.stringExists("past_rulers") ? LocalizedTextManager.getText("past_rulers") : "Past";
            pTooltip.name.text = $"{baseTitle} ({rulerTitle})";
        }
    }

    [HarmonyPatch(typeof(Tooltip), "addLineText", new[] { typeof(string), typeof(string), typeof(string), typeof(bool), typeof(bool), typeof(int) })]
    public static class TooltipAddLineTextKingPatch
    {
        private static void Prefix(ref string pID, ref string pValue, ref int pLimitValue)
        {
            if (!string.Equals(pID, "village_statistics_king", StringComparison.Ordinal))
                return;

            Kingdom kingdom = LocalizationContext.CurrentKingdom;
            if (kingdom == null)
                return;

            if (!CouncilManager.HasMultipleRulers(kingdom))
                return;

            pValue = CouncilManager.GetRulerDisplay(kingdom);
            pLimitValue = Math.Max(pLimitValue, 256);
        }
    }
}
