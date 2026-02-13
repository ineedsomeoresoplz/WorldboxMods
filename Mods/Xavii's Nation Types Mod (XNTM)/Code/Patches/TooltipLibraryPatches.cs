using System;
using HarmonyLib;
using XNTM.Code.Features.BetterWars;
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

            string color = kingdom.getColor()?.color_text ?? "#FFFFFF";
            NationTypeDefinition nation = NationTypeManager.GetDefinition(kingdom);
            string nationLabel = nation?.GetLocalizedName() ?? (LocalizedTextManager.stringExists("kingdom") ? LocalizedTextManager.getText("kingdom") : "Kingdom");
            string subtitle = $"<color={Toolbox.makeDarkerColor(color, 0.8f)}>{nationLabel}</color>";

            if (BetterWarsManager.TryGetOverlord(kingdom, out Kingdom overlord) && overlord != null && overlord.isAlive())
            {
                string template = LocalizedTextManager.stringExists("xntm_puppet_suffix") ? LocalizedTextManager.getText("xntm_puppet_suffix") : "Puppet to $kingdom$";
                string label = template.Replace("$kingdom$", overlord.name);
                subtitle = $"{subtitle} <color=#FF3C3C>({label})</color>";
            }

            pTooltip.name.text = $"{Toolbox.coloredText(kingdom.name, color)}\n<size=7>{subtitle}</size>";
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
            string subtitle = def?.DetermineRulerTitleKey(actor?.isSexFemale() == true);
            if (string.IsNullOrEmpty(subtitle))
                subtitle = "village_statistics_king";
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

            if (!CouncilManager.IsCouncilNation(kingdom))
                return;

            pValue = CouncilManager.GetCouncilTooltipSummary(kingdom);
            pLimitValue = Math.Max(pLimitValue, 256);
        }
    }
}
