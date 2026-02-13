using System;
using System.Collections.Generic;
using UnityEngine;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    public static class NationTypeOpinionBuilder
    {
        private static bool _registered;

        private static readonly HashSet<string> Maritime = new HashSet<string>
        {
            "maritime_republic",
            "port_state",
            "trade_league",
            "hanseatic_league",
            "merchant_republic"
        };

        private static readonly HashSet<string> Tribal = new HashSet<string>
        {
            "clan_state",
            "tribal_confederation",
            "nomadic_horde",
            "chiefdom"
        };

        private static readonly HashSet<string> Overlord = new HashSet<string>
        {
            "empire",
            "federation",
            "confederation",
            "union",
            "commonwealth",
            "trade_league",
            "hanseatic_league"
        };

        private static readonly HashSet<string> Anarchy = new HashSet<string>
        {
            "anarchy",
            "anarcho_state"
        };

        public static void Register()
        {
            Register(AssetManager.opinion_library);
        }

        public static void Register(OpinionLibrary library)
        {
            if (_registered)
                return;
            if (library == null)
                return;

            var definitions = NationTypeManager.Definitions;
            int count = definitions.Count;
            for (int i = 0; i < count; i++)
            {
                NationTypeDefinition source = definitions[i];
                for (int j = 0; j < count; j++)
                {
                    NationTypeDefinition target = definitions[j];
                    string id = BuildId(source.Id, target.Id);
                    OpinionAsset asset = library.get(id);
                    if (asset == null)
                    {
                        asset = new OpinionAsset
                        {
                            id = id
                        };
                        library.add(asset);
                    }

                    asset.translation_key = id;
                    asset.translation_key_negative = string.Empty;
                    asset.calc = (OpinionDelegateCalc)((main, other) => Calculate(main, other, source.Id, target.Id));
                }
            }

            _registered = true;
        }

        private static string BuildId(string sourceId, string targetId)
        {
            return $"xntm_opinion_{sourceId}_vs_{targetId}";
        }

        private static int Calculate(Kingdom main, Kingdom target, string sourceId, string targetId)
        {
            var mainDef = NationTypeManager.GetDefinition(main);
            var targetDef = NationTypeManager.GetDefinition(target);
            if (mainDef == null || targetDef == null)
                return 0;
            if (!string.Equals(mainDef.Id, sourceId, StringComparison.Ordinal))
                return 0;
            if (!string.Equals(targetDef.Id, targetId, StringComparison.Ordinal))
                return 0;

            int score = GetSuccessionScore(mainDef.SuccessionMode, targetDef.SuccessionMode);
            if (mainDef.Id == targetDef.Id)
                score += 6;

            score += GetMaritimeScore(mainDef.Id, targetDef.Id);
            score += GetTribalScore(mainDef.Id, targetDef.Id);
            score += GetOverlordScore(mainDef.Id, targetDef.Id);
            score += GetAnarchyScore(mainDef.Id, targetDef.Id);

            return Mathf.Clamp(score, -40, 40);
        }

        private static int GetSuccessionScore(NationSuccessionMode source, NationSuccessionMode target)
        {
            if (source == target)
                return source == NationSuccessionMode.None ? 4 : 12;

            switch (source)
            {
                case NationSuccessionMode.RoyalLine:
                    return target switch
                    {
                        NationSuccessionMode.Elective => -14,
                        NationSuccessionMode.Council => -8,
                        NationSuccessionMode.Religious => 6,
                        NationSuccessionMode.AgeRule => -4,
                        NationSuccessionMode.WealthRule => -8,
                        NationSuccessionMode.None => -16,
                        _ => 0
                    };
                case NationSuccessionMode.Elective:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => -12,
                        NationSuccessionMode.Council => 10,
                        NationSuccessionMode.Religious => -2,
                        NationSuccessionMode.AgeRule => 2,
                        NationSuccessionMode.WealthRule => 6,
                        NationSuccessionMode.None => -10,
                        _ => 0
                    };
                case NationSuccessionMode.Council:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => -6,
                        NationSuccessionMode.Elective => 8,
                        NationSuccessionMode.Religious => -2,
                        NationSuccessionMode.AgeRule => 4,
                        NationSuccessionMode.WealthRule => 4,
                        NationSuccessionMode.None => -8,
                        _ => 0
                    };
                case NationSuccessionMode.Religious:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => 4,
                        NationSuccessionMode.Elective => -2,
                        NationSuccessionMode.Council => -2,
                        NationSuccessionMode.AgeRule => 2,
                        NationSuccessionMode.WealthRule => -4,
                        NationSuccessionMode.None => -12,
                        _ => 0
                    };
                case NationSuccessionMode.AgeRule:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => -4,
                        NationSuccessionMode.Elective => 2,
                        NationSuccessionMode.Council => 4,
                        NationSuccessionMode.Religious => 2,
                        NationSuccessionMode.WealthRule => 0,
                        NationSuccessionMode.None => -6,
                        _ => 0
                    };
                case NationSuccessionMode.WealthRule:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => -8,
                        NationSuccessionMode.Elective => 6,
                        NationSuccessionMode.Council => 4,
                        NationSuccessionMode.Religious => -4,
                        NationSuccessionMode.AgeRule => 0,
                        NationSuccessionMode.None => -6,
                        _ => 0
                    };
                case NationSuccessionMode.None:
                    return target switch
                    {
                        NationSuccessionMode.RoyalLine => -10,
                        NationSuccessionMode.Elective => -10,
                        NationSuccessionMode.Council => -8,
                        NationSuccessionMode.Religious => -12,
                        NationSuccessionMode.AgeRule => -6,
                        NationSuccessionMode.WealthRule => -6,
                        _ => 4
                    };
                default:
                    return 0;
            }
        }

        private static int GetMaritimeScore(string sourceId, string targetId)
        {
            bool sourceMaritime = Maritime.Contains(sourceId);
            bool targetMaritime = Maritime.Contains(targetId);
            if (sourceMaritime && targetMaritime)
                return 6;
            if (sourceMaritime ^ targetMaritime)
                return -2;
            return 0;
        }

        private static int GetTribalScore(string sourceId, string targetId)
        {
            bool sourceTribal = Tribal.Contains(sourceId);
            bool targetTribal = Tribal.Contains(targetId);
            if (sourceTribal && targetTribal)
                return 5;
            if (sourceTribal ^ targetTribal)
                return -3;
            return 0;
        }

        private static int GetOverlordScore(string sourceId, string targetId)
        {
            bool sourceOverlord = Overlord.Contains(sourceId);
            bool targetOverlord = Overlord.Contains(targetId);
            if (sourceOverlord && targetOverlord)
                return -4;
            if (sourceOverlord && !targetOverlord)
                return 3;
            if (!sourceOverlord && targetOverlord)
                return -3;
            return 0;
        }

        private static int GetAnarchyScore(string sourceId, string targetId)
        {
            bool sourceAnarchy = Anarchy.Contains(sourceId);
            bool targetAnarchy = Anarchy.Contains(targetId);
            if (sourceAnarchy && targetAnarchy)
                return 4;
            if (sourceAnarchy && !targetAnarchy)
                return -6;
            if (!sourceAnarchy && targetAnarchy)
                return -8;
            return 0;
        }
    }
}
