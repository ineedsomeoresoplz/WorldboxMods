using System;
using HarmonyLib;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(StatsRowsContainer), "showStatRow")]
    public static class StatsRowsContainerPatch
    {
        private static void Postfix(StatsRowsContainer __instance, string pId, object pValue, string pColor, MetaType pMetaType, long pMetaId, bool pColorText, string pIconPath, string pTooltipId, TooltipDataGetter pTooltipData, bool pLocalize, KeyValueField __result)
        {
            if (__result == null || string.IsNullOrEmpty(pId) || World.world == null)
                return;

            StatsWindow window = AccessTools.Field(typeof(StatsRowsContainer), "stats_window").GetValue(__instance) as StatsWindow;
            Kingdom kingdom = ResolveKingdom(window, pMetaType, pMetaId);
            if (kingdom == null)
                return;

            NationTypeDefinition def = NationTypeManager.GetDefinition(kingdom);
            if (def == null)
                return;

            switch (pId)
            {
                case "kingdom":
                    __result.name_text.text = def.GetLocalizedName();
                    break;
                case "king":
                    __result.name_text.text = def.GetLocalizedRulerTitle(IsFemaleActor(pMetaType, pMetaId));
                    if (CouncilManager.HasMultipleRulers(kingdom))
                        __result.value.text = CouncilManager.GetRulerDisplay(kingdom);
                    break;
                case "heir":
                    __result.name_text.text = def.GetLocalizedHeirTitle();
                    break;
                case "ruler_money":
                    __result.name_text.text = BuildRulerMoneyLabel(def);
                    break;
                case "past_kings":
                    __result.name_text.text = BuildPastRulerLabel(def);
                    break;
                case "kingdom_statistics_king_ruled":
                    __result.name_text.text = BuildRuleDurationLabel(def);
                    break;
            }
        }

        private static Kingdom ResolveKingdom(StatsWindow window, MetaType pMetaType, long pMetaId)
        {
            if (window != null)
            {
                var metaProp = AccessTools.Property(window.GetType(), "meta_object");
                object meta = metaProp?.GetValue(window);
                if (meta is Kingdom kingdom)
                    return kingdom;
                if (meta is City city)
                    return city.kingdom;
            }

            if (pMetaType == MetaType.Kingdom && pMetaId >= 0)
                return World.world.kingdoms.get(pMetaId);

            if (pMetaType == MetaType.Unit && pMetaId >= 0)
            {
                Actor actor = World.world.units.get(pMetaId);
                if (actor != null && actor.kingdom != null)
                    return actor.kingdom;
            }

            return null;
        }

        private static bool IsFemaleActor(MetaType pMetaType, long pMetaId)
        {
            if (pMetaType != MetaType.Unit || pMetaId < 0 || World.world == null)
                return false;
            Actor actor = World.world.units.get(pMetaId);
            return actor != null && actor.isSexFemale();
        }

        private static string BuildPastRulerLabel(NationTypeDefinition def)
        {
            string baseText = LocalizedTextManager.stringExists("past_rulers") ? LocalizedTextManager.getText("past_rulers") : "Past rulers";
            return $"{baseText} ({def.GetLocalizedRulerTitle()})";
        }

        private static string BuildRuleDurationLabel(NationTypeDefinition def)
        {
            string suffix = LocalizedTextManager.stringExists("kingdom_statistics_ruled_suffix") ? LocalizedTextManager.getText("kingdom_statistics_ruled_suffix") : "ruled";
            return $"{def.GetLocalizedRulerTitle()} {suffix}";
        }

        private static string BuildRulerMoneyLabel(NationTypeDefinition def)
        {
            return $"{def.GetLocalizedRulerTitle()}'s Money";
        }
    }
}
