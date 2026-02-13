using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    
    
    
    public static class NationNamingHelper
    {
        private const string NameVersionFlag = "xntm_name_v2";

        private static readonly HashSet<string> GovernmentTokens = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Empire", "Kingdom", "Republic", "Union", "Confederation", "Federation",
            "Commonwealth", "Tribe", "Horde", "City-State", "City State", "Clan-State",
            "Clan State", "Duchy", "Grand Duchy", "Principality", "Caliphate", "Sultanate",
            "Khanate", "Tsardom", "Emirate", "Sheikhdom", "Dominion", "Order", "Alliance",
            "League", "State", "Nation", "Realm"
        };

        public static bool ApplyAccurateName(Kingdom kingdom, string seedName = null, bool force = false)
        {
            if (kingdom?.data == null)
                return false;
            if (kingdom.data.custom_name)
                return false;

            NationTypeDefinition definition = NationTypeManager.EnsureType(kingdom);
            if (definition == null)
                return false;

            if (!force && kingdom.data.hasFlag(NameVersionFlag))
                return false;

            string baseName = BuildBaseName(seedName ?? kingdom.data.name, definition);
            if (string.IsNullOrWhiteSpace(baseName))
                baseName = "Unnamed";

            string formatted = ComposeName(kingdom, baseName);
            if (string.IsNullOrWhiteSpace(formatted))
            {
                kingdom.data.addFlag(NameVersionFlag);
                return false;
            }

            if (!string.Equals(kingdom.data.name, formatted, StringComparison.Ordinal))
                kingdom.setName(formatted);

            kingdom.data.addFlag(NameVersionFlag);
            return true;
        }

        private static string BuildBaseName(string raw, NationTypeDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            string cleaned = StripGovernmentTerms(raw);
            cleaned = StripGovernmentTerms(cleaned, definition);

            cleaned = Regex.Replace(cleaned, @"\b(?:of|the)\b", string.Empty, RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"\s{2,}", " ").Trim(' ', '-', ',', '\'', '"');

            return string.IsNullOrWhiteSpace(cleaned) ? raw.Trim() : cleaned;
        }

        private static string StripGovernmentTerms(string value, NationTypeDefinition definition = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string result = GovernmentTokens.Aggregate(value, (current, token) =>
                Regex.Replace(current, $@"\b{Regex.Escape(token)}\b", string.Empty, RegexOptions.IgnoreCase));

            if (definition != null)
            {
                foreach (NationTypeDefinition def in NationTypeManager.Definitions)
                {
                    string localized = def.GetLocalizedName();
                    if (!string.IsNullOrWhiteSpace(localized))
                        result = Regex.Replace(result, $@"\b{Regex.Escape(localized)}\b", string.Empty, RegexOptions.IgnoreCase);
                }
            }

            return result;
        }

        private static string ComposeName(Kingdom kingdom, string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
                return baseName;
            return Regex.Replace(baseName, @"\s{2,}", " ").Trim();
        }
    }
}
