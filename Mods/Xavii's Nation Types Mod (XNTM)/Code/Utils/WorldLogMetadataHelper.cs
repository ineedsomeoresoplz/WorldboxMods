using System;
using UnityEngine;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    public static class WorldLogMetadataHelper
    {
        private const char MetaDelimiter = '\u001F'; 
        private const char MetaSeparator = '|';

        private static readonly string[] WarDeclarationAssets = { "diplomacy_war_started", "total_war_started" };
        private static readonly string[] RulerLogs =
        {
            "king_new", "king_left", "king_fled_city", "king_fled_capital", "king_dead", "king_killed",
            "kingdom_royal_clan_new", "kingdom_royal_clan_changed"
        };

        public static void AttachNationMetadata(WorldLogMessage message)
        {
            if (message == null)
                return;
            if (!ShouldAttachMetadata(message))
                return;

            if (!string.IsNullOrEmpty(message.special3) && message.special3.IndexOf(MetaDelimiter) >= 0)
                return;

            Kingdom kingdom = ResolvePrimaryKingdom(message);
            if (message.kingdom == null && kingdom != null)
                message.kingdom = kingdom;
            NationTypeDefinition def = NationTypeManager.GetDefinition(kingdom);
            Actor ruler = ResolveRuler(message, kingdom);
            bool isFemale = ruler?.isSexFemale() == true;

            string baseSpecial = GetBaseSpecial(message.special3);
            message.special3 = Serialize(baseSpecial, def.Id, isFemale);
        }

        private static bool ShouldAttachMetadata(WorldLogMessage message)
        {
            WorldLogAsset asset = message.getAsset();
            if (asset == null)
                return false;

            string localeRoot = asset.getLocaleID();
            if (string.IsNullOrEmpty(localeRoot))
                return false;

            if (LocalizedTextManager.stringExists($"{localeRoot}__kingdom"))
                return true;

            if (LocalizedTextManager.stringExists($"{localeRoot}__kingdom__kingdom"))
                return true;

            return false;
        }

        public static bool TryGetStored(WorldLogMessage message, out NationTypeDefinition def, out bool isFemale)
        {
            def = null;
            isFemale = false;
            if (message == null || string.IsNullOrEmpty(message.special3))
                return false;

            string baseSpecial;
            string defId;
            if (!TryParse(message.special3, out baseSpecial, out defId, out isFemale))
                return false;

            return NationTypeManager.TryGetDefinition(defId, out def);
        }

        public static string StripMetadataFromFormatted(string formatted)
        {
            if (string.IsNullOrEmpty(formatted))
                return formatted;

            int delimiterIndex = formatted.IndexOf(MetaDelimiter);
            if (delimiterIndex < 0)
                return formatted;

            int end = formatted.IndexOf("</color>", delimiterIndex, StringComparison.Ordinal);
            if (end < 0)
                end = formatted.Length;

            return formatted.Remove(delimiterIndex, end - delimiterIndex);
        }

        public static string GetBaseSpecial(string special3)
        {
            if (string.IsNullOrEmpty(special3))
                return string.Empty;

            int delimiterIndex = special3.IndexOf(MetaDelimiter);
            return delimiterIndex >= 0 ? special3.Substring(0, delimiterIndex) : special3;
        }

        public static Kingdom ResolvePrimaryKingdom(WorldLogMessage message)
        {
            if (message == null)
                return null;

            if (message.kingdom != null)
                return message.kingdom;

            if (message.unit?.kingdom != null && !Array.Exists(WarDeclarationAssets, id => id == message.asset_id))
                return message.unit.kingdom;

            switch (message.asset_id)
            {
                case "diplomacy_war_started":
                case "total_war_started":
                case "kingdom_new":
                case "kingdom_destroyed":
                case "kingdom_fractured":
                case "kingdom_shattered":
                case "kingdom_royal_clan_new":
                case "kingdom_royal_clan_changed":
                case "kingdom_royal_clan_dead":
                    return ResolveKingdomFromSpecial(message.special1, message.color_special_1);
                default:
                    return message.unit?.kingdom;
            }
        }

        public static Kingdom ResolveKingdomFromSpecial(string special, string colorHex = null)
        {
            if (string.IsNullOrEmpty(special) || World.world?.kingdoms == null)
                return null;

            Kingdom byColor = null;
            if (!string.IsNullOrEmpty(colorHex))
            {
                foreach (var kingdom in World.world.kingdoms)
                {
                    if (kingdom?.getColor() == null)
                        continue;
                    Color kingdomTextColor = kingdom.getColor().getColorText();
                    string hex = Toolbox.colorToHex((Color32)kingdomTextColor, false);
                    if (hex == colorHex)
                    {
                        byColor = kingdom;
                        break;
                    }
                }
            }

            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom != null && kingdom.name == special)
                    return kingdom;
            }

            return byColor;
        }

        public static Actor ResolveRuler(WorldLogMessage message, Kingdom kingdom)
        {
            if (message == null)
                return kingdom?.king;

            if (Array.Exists(RulerLogs, id => id == message.asset_id))
            {
                Actor byName = FindActorByName(message.special2) ?? FindActorByName(message.special3);
                if (byName != null)
                    return byName;
            }

            if (message.asset_id == "king_killed")
            {
                Actor target = FindActorByName(message.special2);
                if (target != null)
                    return target;
            }

            return kingdom?.king ?? message.unit;
        }

        private static Actor FindActorByName(string name)
        {
            if (string.IsNullOrEmpty(name) || World.world?.units == null)
                return null;

            foreach (Actor actor in World.world.units)
            {
                if (actor != null && actor.getName() == name)
                    return actor;
            }
            return null;
        }

        private static string Serialize(string baseSpecial, string defId, bool isFemale)
        {
            baseSpecial ??= string.Empty;
            defId ??= string.Empty;
            return $"{baseSpecial}{MetaDelimiter}{defId}{MetaSeparator}{(isFemale ? 'F' : 'M')}";
        }

        private static bool TryParse(string special3, out string baseSpecial, out string defId, out bool isFemale)
        {
            baseSpecial = GetBaseSpecial(special3);
            defId = string.Empty;
            isFemale = false;

            int delimiterIndex = special3.IndexOf(MetaDelimiter);
            if (delimiterIndex < 0 || delimiterIndex + 1 >= special3.Length)
                return false;

            string meta = special3.Substring(delimiterIndex + 1);
            string[] parts = meta.Split(MetaSeparator);
            if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
                return false;

            defId = parts[0];
            if (parts.Length > 1)
                isFemale = parts[1].StartsWith("F", StringComparison.OrdinalIgnoreCase);

            return true;
        }
    }
}
