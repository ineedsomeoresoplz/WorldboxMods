using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    public static class NationTypeLogHelper
    {
        private const string RulerToken = "$ruler_title$";
        private const string HeirToken = "$heir_title$";
        private const string NationToken = "$nation_type$";
        private const string AttackerNationToken = "$attacker_nation_type$";
        private const string DefenderNationToken = "$defender_nation_type$";

        public static string ResolveLocaleId(WorldLogMessage message, WorldLogAsset asset)
        {
            if (message == null || asset == null)
                return asset?.getLocaleID() ?? string.Empty;

            string baseLocale = GetBaseLocaleId(message, asset);
            if (asset.id == "diplomacy_war_started")
            {
                string attackerType = GetTypeId(WorldLogMetadataHelper.ResolveKingdomFromSpecial(message.special1, message.color_special_1));
                string defenderType = GetTypeId(WorldLogMetadataHelper.ResolveKingdomFromSpecial(message.special2, message.color_special_2));
                if (!string.IsNullOrEmpty(attackerType) && !string.IsNullOrEmpty(defenderType))
                {
                    string pairKey = $"{baseLocale}__{attackerType}__{defenderType}";
                    if (LocalizedTextManager.stringExists(pairKey))
                        return pairKey;
                }
            }
            else
            {
                string typeId = GetTypeId(WorldLogMetadataHelper.ResolvePrimaryKingdom(message), message);
                if (!string.IsNullOrEmpty(typeId))
                {
                    string typeKey = $"{baseLocale}__{typeId}";
                    if (LocalizedTextManager.stringExists(typeKey))
                        return typeKey;
                }
            }

            return baseLocale;
        }

        public static string ReplaceTokens(WorldLogMessage message, string text)
        {
            if (string.IsNullOrEmpty(text) || message == null)
                return text;

            bool storedFemale = false;
            NationTypeManager.RegisterTraits();
            WorldLogMetadataHelper.TryGetStored(message, out var storedDef, out storedFemale);

            Kingdom kingdom = WorldLogMetadataHelper.ResolvePrimaryKingdom(message);
            NationTypeDefinition def = storedDef ?? NationTypeManager.GetDefinition(kingdom);
            if (def == null)
                return text;

            Actor ruler = WorldLogMetadataHelper.ResolveRuler(message, kingdom);
            bool rulerFemale = storedFemale || ruler?.isSexFemale() == true;

            if (text.Contains(AttackerNationToken) || text.Contains(DefenderNationToken))
            {
                Kingdom attacker = WorldLogMetadataHelper.ResolveKingdomFromSpecial(message.special1, message.color_special_1) ?? kingdom;
                Kingdom defender = WorldLogMetadataHelper.ResolveKingdomFromSpecial(message.special2, message.color_special_2);

                NationTypeDefinition attackerDef = NationTypeManager.GetDefinition(attacker);
                NationTypeDefinition defenderDef = NationTypeManager.GetDefinition(defender);

                if (text.Contains(AttackerNationToken))
                    text = text.Replace(AttackerNationToken, attackerDef?.GetLocalizedName() ?? attacker?.name ?? def.GetLocalizedName());
                if (text.Contains(DefenderNationToken))
                    text = text.Replace(DefenderNationToken, defenderDef?.GetLocalizedName() ?? defender?.name ?? def.GetLocalizedName());
            }

            if (text.Contains(RulerToken))
                text = text.Replace(RulerToken, def.GetLocalizedRulerTitle(rulerFemale));
            if (text.Contains(HeirToken))
                text = text.Replace(HeirToken, def.GetLocalizedHeirTitle());
            if (text.Contains(NationToken))
                text = text.Replace(NationToken, def.GetLocalizedName());
            return text;
        }

        private static string GetTypeId(Kingdom kingdom, WorldLogMessage message = null)
        {
            NationTypeManager.RegisterTraits();
            if (kingdom == null)
                return string.Empty;
            if (message != null && WorldLogMetadataHelper.TryGetStored(message, out var stored, out _))
                return stored?.Id ?? string.Empty;
            return NationTypeManager.GetDefinition(kingdom)?.Id ?? string.Empty;
        }

        private static string GetBaseLocaleId(WorldLogMessage message, WorldLogAsset asset)
        {
            if (asset.random_ids > 0)
            {
                int index = message.timestamp % asset.random_ids + 1;
                return asset.getLocaleID(index);
            }

            return asset.getLocaleID();
        }
    }
}
