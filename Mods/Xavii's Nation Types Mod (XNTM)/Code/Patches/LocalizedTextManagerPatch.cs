using System;
using HarmonyLib;
using UnityEngine.UI;
using XNTM.Code.Data;
using XNTM.Code.Features.BetterWars;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(LocalizedTextManager), nameof(LocalizedTextManager.getText), new[] { typeof(string), typeof(Text), typeof(bool) })]
    public static class LocalizedTextManagerPatch
    {
        private static bool Prefix(string pKey, Text text, bool pForceEnglish, ref string __result)
        {
            return !LocalizedTextManagerPatchShared.TryHandleMissingLocaleKey(pKey, pForceEnglish, ref __result);
        }

        private static void Postfix(string pKey, Text text, bool pForceEnglish, ref string __result)
        {
            LocalizedTextManagerPatchShared.ApplyPostOverrides(pKey, pForceEnglish, ref __result);
        }
    }

    [HarmonyPatch(typeof(LocalizedTextManager), nameof(LocalizedTextManager.stringExists))]
    public static class LocalizedTextManagerStringExistsPatch
    {
        private static void Postfix(string pKey, ref bool __result)
        {
            LocalizedTextManagerPatchShared.ApplyStringExistsOverride(pKey, ref __result);
        }
    }

    internal static class LocalizedTextManagerPatchShared
    {
        private const string PuppetSuffixKey = "xntm_puppet_suffix";
        private const string PreferenceTemplateKey = "xntm_prefers_template";
        private const string PreferenceDescriptionKey = "xntm_prefers_description";
        private const string PreferenceTraitGroupKey = "trait_group_xntm_nation_preferences";
        private const string ItemTemplateFullKey = "item_template_description_full";
        private const string ItemTemplateFullPlayerKey = "item_template_description_full_player";
        private const string ItemTemplateAgeOnlyKey = "item_template_description_age_only";
        private const string ItemTemplateYearKey = "item_template_description_year";
        private const string ItemTemplateYearsKey = "item_template_description_years";
        private const string PreferencePrefix = "xntm_prefers_";
        private const string OpinionPrefix = "xntm_opinion_";
        private const string OpinionSeparator = "_vs_";
        private const string DefaultPuppetSuffix = "Puppet to $kingdom$";
        private const string DefaultPreferenceTemplate = "Prefers a $nation_type$ Nation";
        private const string DefaultPreferenceDescription = "Political preference only. No direct stat effects.";
        private const string DefaultPreferenceTraitGroupLabel = "Nation Preferences";
        private const string DefaultOpinionTemplate = "Governance alignment: {0} vs {1}";
        private const string DefaultItemTemplateFull = "Created by $item_creator_name$ from $item_creator_kingdom$ $item_creator_years$ $year_ending$ ago";
        private const string DefaultItemTemplateFullPlayer = "Created by player $item_creator_years$ $year_ending$ ago";
        private const string DefaultItemTemplateAgeOnly = "Created $item_creator_years$ $year_ending$ ago";
        private const string DefaultItemTemplateYear = "year";
        private const string DefaultItemTemplateYears = "years";

        internal static bool TryHandleMissingLocaleKey(string pKey, bool pForceEnglish, ref string __result)
        {
            if (string.IsNullOrEmpty(pKey) || pForceEnglish)
                return false;
            if (HasRawLocaleKey(pKey))
                return false;
            if (!TryResolveSyntheticLocale(pKey, out string resolved))
                return false;
            __result = resolved;
            return true;
        }

        internal static void ApplyStringExistsOverride(string pKey, ref bool __result)
        {
            if (__result || string.IsNullOrEmpty(pKey))
                return;
            if (IsSyntheticLocaleKey(pKey))
                __result = true;
        }

        internal static void ApplyPostOverrides(string pKey, bool pForceEnglish, ref string __result)
        {
            if (string.IsNullOrEmpty(pKey) || pForceEnglish)
                return;
            if (TryFixItemCreationTemplate(pKey, __result, out string itemTemplate))
            {
                __result = itemTemplate;
                return;
            }

            if (!HasRawLocaleKey(pKey) && TryResolveSyntheticLocale(pKey, out string synthetic))
            {
                __result = synthetic;
                return;
            }
            if (TryResolveWarReasonLocale(pKey, out string warReasonText))
            {
                __result = warReasonText;
                return;
            }

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

        private static bool IsSyntheticLocaleKey(string pKey)
        {
            if (string.IsNullOrEmpty(pKey))
                return false;
            if (pKey == PuppetSuffixKey || pKey == PreferenceTemplateKey || pKey == PreferenceDescriptionKey || pKey == PreferenceTraitGroupKey)
                return true;
            if (IsWarReasonLocaleKey(pKey))
                return true;
            if (TryParseOpinionKey(pKey, out _, out _))
                return true;
            if (!pKey.StartsWith(PreferencePrefix, StringComparison.Ordinal))
                return false;
            string typeId = pKey.Substring(PreferencePrefix.Length);
            NationTypeDefinition definition;
            return NationTypeManager.TryGetDefinition(typeId, out definition);
        }

        private static bool TryResolveSyntheticLocale(string pKey, out string resolved)
        {
            resolved = null;

            if (pKey == PuppetSuffixKey)
            {
                resolved = DefaultPuppetSuffix;
                return true;
            }

            if (pKey == PreferenceTemplateKey)
            {
                resolved = DefaultPreferenceTemplate;
                return true;
            }

            if (pKey == PreferenceDescriptionKey)
            {
                resolved = DefaultPreferenceDescription;
                return true;
            }

            if (pKey == PreferenceTraitGroupKey)
            {
                resolved = DefaultPreferenceTraitGroupLabel;
                return true;
            }

            if (TryResolveOpinionLocale(pKey, out resolved))
                return true;

            if (!pKey.StartsWith(PreferencePrefix, StringComparison.Ordinal))
                return TryResolveWarReasonLocale(pKey, out resolved);

            string typeId = pKey.Substring(PreferencePrefix.Length);
            NationTypeDefinition definition;
            if (!NationTypeManager.TryGetDefinition(typeId, out definition))
                return false;

            string template = HasRawLocaleKey(PreferenceTemplateKey)
                ? LocalizedTextManager.getText(PreferenceTemplateKey)
                : DefaultPreferenceTemplate;
            resolved = template.Replace("$nation_type$", definition.GetLocalizedName());
            return true;
        }

        private static bool TryResolveOpinionLocale(string pKey, out string resolved)
        {
            resolved = null;
            if (!TryParseOpinionKey(pKey, out NationTypeDefinition source, out NationTypeDefinition target))
                return false;
            resolved = string.Format(DefaultOpinionTemplate, source.GetLocalizedName(), target.GetLocalizedName());
            return true;
        }

        private static bool TryParseOpinionKey(string pKey, out NationTypeDefinition source, out NationTypeDefinition target)
        {
            source = null;
            target = null;
            if (string.IsNullOrEmpty(pKey) || !pKey.StartsWith(OpinionPrefix, StringComparison.Ordinal))
                return false;

            string payload = pKey.Substring(OpinionPrefix.Length);
            int separator = payload.IndexOf(OpinionSeparator, StringComparison.Ordinal);
            if (separator <= 0 || separator + OpinionSeparator.Length >= payload.Length)
                return false;

            string sourceId = payload.Substring(0, separator);
            string targetId = payload.Substring(separator + OpinionSeparator.Length);
            if (!NationTypeManager.TryGetDefinition(sourceId, out source))
                return false;
            if (!NationTypeManager.TryGetDefinition(targetId, out target))
                return false;
            return true;
        }

        private static bool TryResolveWarReasonLocale(string pKey, out string resolved)
        {
            resolved = null;
            if (!IsWarReasonLocaleKey(pKey))
                return false;

            War war = WarLocalizationContext.CurrentWar;
            if (war == null)
                return false;

            if (!BetterWarsManager.TryGetReasonDisplayName(war, out string reasonName))
                return false;

            resolved = reasonName;
            return true;
        }

        private static bool IsWarReasonLocaleKey(string pKey)
        {
            return pKey == "war_name_conquest" || pKey == "war_type_conquest";
        }

        private static bool HasRawLocaleKey(string pKey)
        {
            try
            {
                return LocalizedTextManager.instance != null && LocalizedTextManager.instance.contains(pKey);
            }
            catch
            {
                return false;
            }
        }

        private static bool TryFixItemCreationTemplate(string pKey, string current, out string resolved)
        {
            resolved = null;
            if (!IsItemCreationTemplateKey(pKey))
                return false;
            if (!LooksBrokenItemTemplate(pKey, current))
                return false;
            resolved = GetDefaultItemTemplate(pKey);
            return !string.IsNullOrEmpty(resolved);
        }

        private static bool IsItemCreationTemplateKey(string pKey)
        {
            return pKey == ItemTemplateFullKey
                || pKey == ItemTemplateFullPlayerKey
                || pKey == ItemTemplateAgeOnlyKey
                || pKey == ItemTemplateYearKey
                || pKey == ItemTemplateYearsKey;
        }

        private static bool LooksBrokenItemTemplate(string pKey, string current)
        {
            if (string.IsNullOrWhiteSpace(current))
                return true;
            if (string.Equals(current, pKey, StringComparison.OrdinalIgnoreCase))
                return true;
            return string.Equals(current.Trim(), "item", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetDefaultItemTemplate(string pKey)
        {
            switch (pKey)
            {
                case ItemTemplateFullKey:
                    return DefaultItemTemplateFull;
                case ItemTemplateFullPlayerKey:
                    return DefaultItemTemplateFullPlayer;
                case ItemTemplateAgeOnlyKey:
                    return DefaultItemTemplateAgeOnly;
                case ItemTemplateYearKey:
                    return DefaultItemTemplateYear;
                case ItemTemplateYearsKey:
                    return DefaultItemTemplateYears;
                default:
                    return null;
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
