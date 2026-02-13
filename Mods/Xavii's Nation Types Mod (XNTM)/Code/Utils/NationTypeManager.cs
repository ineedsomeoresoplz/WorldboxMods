using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    public static class NationTypeManager
    {
        private const string CustomDataKey = "nationals_type_id"; 
        private const string TraitPrefix = "xntm_nation_type_";
        private const string TraitGroupId = "miscellaneous"; 
        private const string PreferenceTraitPrefix = "xntm_prefers_";
        private const string PreferenceTraitGroupId = "xntm_nation_preferences";
        private const string PreferenceTraitGroupLocaleKey = "trait_group_xntm_nation_preferences";
        private const string PreferenceTraitGroupColor = "#7FA8FF";
        private const string PreferenceTypeCacheKey = "xntm_pref_type_cache";
        private const string AutoCheckKey = "xntm_auto_nation_check";
        private const string AutoLastKingKey = "xntm_auto_last_king";
        private const string AutoLastCouncilSignatureKey = "xntm_auto_last_council";
        private const string AutoLeadershipShiftUntilKey = "xntm_auto_shift_until";
        private const string RepublicElectionDueKey = "xntm_rep_election_due";
        private const string RepublicElectionLeaderKey = "xntm_rep_election_leader";
        private const string CitySupportCacheValueKey = "xntm_city_support_value";
        private const string CitySupportCacheUntilKey = "xntm_city_support_until";
        private const string CitySupportCacheTypeKey = "xntm_city_support_type";
        private const string NationTypeChangedLogId = "xntm_nation_type_changed";
        private static readonly HashSet<string> MaritimeIds = new HashSet<string> { "maritime_republic", "port_state", "trade_league", "hanseatic_league", "merchant_republic" };
        private static readonly HashSet<string> ClanRequiredIds = new HashSet<string> { "clan_state", "tribal_confederation" };
        private static readonly HashSet<string> RepublicIds = new HashSet<string>
        {
            "republic",
            "democratic_republic",
            "federal_republic",
            "peoples_republic",
            "constitutional_republic",
            "parliamentary_republic",
            "merchant_republic",
            "maritime_republic"
        };
        private static readonly HashSet<string> AutoFilteredIds = new HashSet<string>
        {
            "anarcho_state",
            "democratic_republic",
            "federal_republic",
            "peoples_republic",
            "constitutional_republic",
            "parliamentary_republic",
            "hanseatic_league",
            "port_state",
            "sacred_dominion",
            "divine_kingdom",
            "mandate_state"
        };
        private static readonly string[] RankOrder = { "barony", "county", "duchy", "grand_duchy", "principality", "kingdom" };
        private static readonly RankThreshold[] RankThresholds =
        {
            new RankThreshold("barony", 1, 0),
            new RankThreshold("county", 1, 60),
            new RankThreshold("duchy", 2, 150),
            new RankThreshold("grand_duchy", 3, 240),
            new RankThreshold("principality", 4, 340),
            new RankThreshold("kingdom", 5, 450)
        };
        private static readonly Dictionary<string, string> IconOverrides = new Dictionary<string, string>
        {
            ["barony"] = "ui/icons/kingdom_traits/xntm_duchy",
            ["county"] = "ui/icons/kingdom_traits/xntm_grand_duchy"
        };

        private readonly struct RankThreshold
        {
            public RankThreshold(string id, int minCities, int minPopulation)
            {
                Id = id;
                MinCities = minCities;
                MinPopulation = minPopulation;
            }

            public string Id { get; }
            public int MinCities { get; }
            public int MinPopulation { get; }
        }

        private static readonly List<NationTypeDefinition> _definitions = new List<NationTypeDefinition>
        {
            new NationTypeDefinition("kingdom", "nation_type_kingdom", "nation_title_king", "nation_heir_prince", NationSuccessionMode.RoyalLine, "nation_type_description_kingdom", "nation_title_queen"),
            new NationTypeDefinition("empire", "nation_type_empire", "nation_title_emperor", "nation_heir_emperor", NationSuccessionMode.RoyalLine, "nation_type_description_empire", "nation_title_empress"),
            new NationTypeDefinition("barony", "nation_type_barony", "nation_title_baron", "nation_heir_baron", NationSuccessionMode.RoyalLine, "nation_type_description_barony", "nation_title_baroness"),
            new NationTypeDefinition("county", "nation_type_county", "nation_title_count", "nation_heir_count", NationSuccessionMode.RoyalLine, "nation_type_description_county", "nation_title_countess"),
            new NationTypeDefinition("principality", "nation_type_principality", "nation_title_prince", "nation_heir_prince", NationSuccessionMode.RoyalLine, "nation_type_description_principality", "nation_title_princess"),
            new NationTypeDefinition("duchy", "nation_type_duchy", "nation_title_duke", "nation_heir_duke", NationSuccessionMode.RoyalLine, "nation_type_description_duchy", "nation_title_duchess"),
            new NationTypeDefinition("grand_duchy", "nation_type_grand_duchy", "nation_title_grand_duke", "nation_heir_grand_duke", NationSuccessionMode.RoyalLine, "nation_type_description_grand_duchy", "nation_title_grand_duchess"),
            new NationTypeDefinition("tsardom", "nation_type_tsardom", "nation_title_tsar", "nation_heir_tsarevich", NationSuccessionMode.RoyalLine, "nation_type_description_tsardom", "nation_title_tsarina"),
            new NationTypeDefinition("sultanate", "nation_type_sultanate", "nation_title_sultan", "nation_heir_sultan", NationSuccessionMode.RoyalLine, "nation_type_description_sultanate", "nation_title_sultana"),
            new NationTypeDefinition("caliphate", "nation_type_caliphate", "nation_title_caliph", "nation_heir_caliph", NationSuccessionMode.Religious, "nation_type_description_caliphate", "nation_title_calipha"),
            new NationTypeDefinition("khanate", "nation_type_khanate", "nation_title_khan", "nation_heir_khan", NationSuccessionMode.RoyalLine, "nation_type_description_khanate", "nation_title_khanum"),
            new NationTypeDefinition("emirate", "nation_type_emirate", "nation_title_emir", "nation_heir_emir", NationSuccessionMode.RoyalLine, "nation_type_description_emirate", "nation_title_emira"),
            new NationTypeDefinition("sheikhdom", "nation_type_sheikhdom", "nation_title_sheikh", "nation_heir_sheikh", NationSuccessionMode.RoyalLine, "nation_type_description_sheikhdom", "nation_title_sheikha"),
            new NationTypeDefinition("despotate", "nation_type_despotate", "nation_title_despot", "nation_heir_despot", NationSuccessionMode.RoyalLine, "nation_type_description_despotate", "nation_title_despotess"),
            new NationTypeDefinition("autocracy", "nation_type_autocracy", "nation_title_autocrat", "nation_heir_autocrat", NationSuccessionMode.RoyalLine, "nation_type_description_autocracy", "nation_title_autocratess"),
            new NationTypeDefinition("military_government", "nation_type_military_government", "nation_title_commander", "nation_heir_commander", NationSuccessionMode.Random, "nation_type_description_military_government"),
            new NationTypeDefinition("diarchy", "nation_type_diarchy", "nation_title_diarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_diarchy"),
            new NationTypeDefinition("triarchy", "nation_type_triarchy", "nation_title_triarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_triarchy"),
            new NationTypeDefinition("tetrarchy", "nation_type_tetrarchy", "nation_title_tetrarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_tetrarchy"),
            new NationTypeDefinition("republic", "nation_type_republic", "nation_title_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_republic"),
            new NationTypeDefinition("council_republic", "nation_type_council_republic", "nation_title_workers_council", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_council_republic"),
            new NationTypeDefinition("democratic_republic", "nation_type_democratic_republic", "nation_title_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_democratic_republic"),
            new NationTypeDefinition("federal_republic", "nation_type_federal_republic", "nation_title_federal_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_federal_republic"),
            new NationTypeDefinition("peoples_republic", "nation_type_peoples_republic", "nation_title_peoples_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_peoples_republic"),
            new NationTypeDefinition("constitutional_republic", "nation_type_constitutional_republic", "nation_title_constitutional_leader", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_constitutional_republic"),
            new NationTypeDefinition("constitutional_monarchy", "nation_type_constitutional_monarchy", "nation_title_king", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_constitutional_monarchy", "nation_title_queen"),
            new NationTypeDefinition("parliamentary_republic", "nation_type_parliamentary_republic", "nation_title_prime_chancellor", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_parliamentary_republic"),
            new NationTypeDefinition("confederation", "nation_type_confederation", "nation_title_confederate", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_confederation"),
            new NationTypeDefinition("federation", "nation_type_federation", "nation_title_federal_chair", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_federation"),
            new NationTypeDefinition("commonwealth", "nation_type_commonwealth", "nation_title_commonwealth_champion", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_commonwealth"),
            new NationTypeDefinition("union", "nation_type_union", "nation_title_union_chair", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_union"),
            new NationTypeDefinition("theocracy", "nation_type_theocracy", "nation_title_high_priest", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_theocracy", "nation_title_high_priestess"),
            new NationTypeDefinition("hierocracy", "nation_type_hierocracy", "nation_title_hierarch", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_hierocracy", "nation_title_hierarchess"),
            new NationTypeDefinition("ecclesiarchy", "nation_type_ecclesiarchy", "nation_title_ecclesarch", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_ecclesiarchy", "nation_title_ecclesiarchess"),
            new NationTypeDefinition("holy_state", "nation_type_holy_state", "nation_title_holy_overseer", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_holy_state"),
            new NationTypeDefinition("sacred_dominion", "nation_type_sacred_dominion", "nation_title_sacred_dominator", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_sacred_dominion"),
            new NationTypeDefinition("divine_kingdom", "nation_type_divine_kingdom", "nation_title_divine_sovereign", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_divine_kingdom"),
            new NationTypeDefinition("mandate_state", "nation_type_mandate_state", "nation_title_mandate_guardian", "nation_heir_priest", NationSuccessionMode.Religious, "nation_type_description_mandate_state"),
            new NationTypeDefinition("city_state", "nation_type_city_state", "nation_title_city_leader", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_city_state"),
            new NationTypeDefinition("free_city", "nation_type_free_city", "nation_title_magistrate", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_free_city"),
            new NationTypeDefinition("merchant_republic", "nation_type_merchant_republic", "nation_title_merchant_council", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_merchant_republic"),
            new NationTypeDefinition("trade_league", "nation_type_trade_league", "nation_title_league_master", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_trade_league"),
            new NationTypeDefinition("hanseatic_league", "nation_type_hanseatic_league", "nation_title_hanseatic_master", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_hanseatic_league"),
            new NationTypeDefinition("maritime_republic", "nation_type_maritime_republic", "nation_title_marine_consul", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_maritime_republic"),
            new NationTypeDefinition("port_state", "nation_type_port_state", "nation_title_port_marshal", "nation_heir_council", NationSuccessionMode.Elective, "nation_type_description_port_state"),
            new NationTypeDefinition("anarchy", "nation_type_anarchy", "nation_title_anarch", "nation_heir_none", NationSuccessionMode.None, "nation_type_description_anarchy"),
            new NationTypeDefinition("anarcho_state", "nation_type_anarcho_state", "nation_title_anarch", "nation_heir_none", NationSuccessionMode.None, "nation_type_description_anarcho_state"),
            new NationTypeDefinition("tribal_confederation", "nation_type_tribal_confederation", "nation_title_confederate", "nation_heir_elder", NationSuccessionMode.Council, "nation_type_description_tribal_confederation"),
            new NationTypeDefinition("clan_state", "nation_type_clan_state", "nation_title_clan_chief", "nation_heir_chief", NationSuccessionMode.RoyalLine, "nation_type_description_clan_state"),
            new NationTypeDefinition("nomadic_horde", "nation_type_nomadic_horde", "nation_title_horde_chief", "nation_heir_horde", NationSuccessionMode.RoyalLine, "nation_type_description_nomadic_horde"),
            new NationTypeDefinition("chiefdom", "nation_type_chiefdom", "nation_title_chief", "nation_heir_chief", NationSuccessionMode.RoyalLine, "nation_type_description_chiefdom"),
            new NationTypeDefinition("gerontocracy", "nation_type_gerontocracy", "nation_title_elder_council", "nation_heir_elder", NationSuccessionMode.AgeRule, "nation_type_description_gerontocracy"),
            new NationTypeDefinition("timocracy", "nation_type_timocracy", "nation_title_timarch", "nation_heir_timarch", NationSuccessionMode.WealthRule, "nation_type_description_timocracy"),
            new NationTypeDefinition("noocracy", "nation_type_noocracy", "nation_title_noarch", "nation_heir_noarch", NationSuccessionMode.Council, "nation_type_description_noocracy")
        };

        private static readonly Dictionary<string, NationTypeDefinition> _definitionMap = _definitions.ToDictionary(def => def.Id, def => def);
        private static readonly NationTypeDefinition _default = _definitionMap["kingdom"];

        private static readonly Dictionary<string, string> _typeToTraitId = _definitions.ToDictionary(def => def.Id, def => TraitPrefix + def.Id);
        private static readonly Dictionary<string, string> _traitIdToType = _typeToTraitId.ToDictionary(kv => kv.Value, kv => kv.Key);
        private static readonly Dictionary<string, string> _typeToPreferenceTraitId = _definitions.ToDictionary(def => def.Id, def => PreferenceTraitPrefix + def.Id);
        private static readonly Dictionary<string, string> _preferenceTraitIdToType = _typeToPreferenceTraitId.ToDictionary(kv => kv.Value, kv => kv.Key);
        private static readonly List<KingdomTrait> _traitAssets = new List<KingdomTrait>(_definitions.Count);
        private static readonly List<ActorTrait> _preferenceTraitAssets = new List<ActorTrait>(_definitions.Count);
        private static bool _traitsRegistered;
        private static bool _preferenceTraitsRegistered;

        private static RebellionBirthContext _pendingRebellionBirth;
        private static readonly HashSet<NationSuccessionMode> _preferredRebelModes = new HashSet<NationSuccessionMode>
        {
            NationSuccessionMode.Elective,
            NationSuccessionMode.Council,
            NationSuccessionMode.Religious,
            NationSuccessionMode.AgeRule,
            NationSuccessionMode.WealthRule
        };

        public static IReadOnlyList<NationTypeDefinition> Definitions => _definitions;

        public static bool TryGetDefinition(string id, out NationTypeDefinition definition)
        {
            if (string.IsNullOrEmpty(id))
            {
                definition = null;
                return false;
            }
            return _definitionMap.TryGetValue(id, out definition);
        }

        public static NationTypeDefinition GetDefinition(Kingdom kingdom)
        {
            EnsureTraitsRegistered();
            if (kingdom == null)
                return _default;
            string stored = GetStoredType(kingdom);
            if (!string.IsNullOrEmpty(stored) && _definitionMap.TryGetValue(stored, out var def) && IsEligible(kingdom, def))
                return def;
            NationTypeDefinition fallback = GetNaturalType(kingdom) ?? _default;
            SetStoredType(kingdom, fallback.Id);
            return fallback;
        }

        public static NationTypeDefinition EnsureType(Kingdom kingdom)
        {
            EnsureTraitsRegistered();
            if (kingdom == null)
                return _default;
            NationTypeDefinition assigned = AssignBirthTypeIfMissing(kingdom);
            if (assigned != null)
                return assigned;
            var def = GetDefinition(kingdom);
            if (string.IsNullOrEmpty(GetStoredType(kingdom)))
                SetStoredType(kingdom, def.Id);
            return def;
        }

        public static void PrepareRebellionBirth(Kingdom previousKingdom)
        {
            EnsureTraitsRegistered();
            if (previousKingdom == null)
            {
                _pendingRebellionBirth = null;
                return;
            }
            _pendingRebellionBirth = new RebellionBirthContext(GetDefinition(previousKingdom));
        }

        public static void ResetPendingRebellion()
        {
            _pendingRebellionBirth = null;
        }

        private static NationTypeDefinition AssignBirthTypeIfMissing(Kingdom kingdom)
        {
            if (kingdom == null)
                return null;
            if (!string.IsNullOrEmpty(GetStoredType(kingdom)))
                return null;
            NationTypeDefinition chosen = SelectBirthType(kingdom);
            SetStoredType(kingdom, chosen.Id);
            _pendingRebellionBirth = null;
            return chosen;
        }

        private static NationTypeDefinition SelectBirthType(Kingdom kingdom)
        {
            if (_pendingRebellionBirth?.PreviousType != null)
                return SelectRebellionType(_pendingRebellionBirth.PreviousType, kingdom);
            return PickRandomType(kingdom);
        }

        private static NationTypeDefinition SelectRebellionType(NationTypeDefinition previous, Kingdom kingdom)
        {
            if (previous != null && Randy.randomChance(0.4f))
                return previous;
            List<NationTypeDefinition> candidates = _definitions
                .Where(def => (previous == null || def.Id != previous.Id) && _preferredRebelModes.Contains(def.SuccessionMode) && IsAllowedForAutoReform(def) && IsEligible(kingdom, def))
                .ToList();
            if (candidates.Count == 0)
                candidates = _definitions.Where(def => (previous == null || def.Id != previous.Id) && IsAllowedForAutoReform(def) && IsEligible(kingdom, def)).ToList();
            if (candidates.Count == 0)
                return previous ?? PickRandomType(kingdom);
            return candidates[Randy.randomInt(0, candidates.Count)];
        }

        private static NationTypeDefinition PickRandomType(Kingdom kingdom)
        {
            if (_definitions.Count == 0)
                return _default;
            List<NationTypeDefinition> eligible = GetEligibleDefinitions(kingdom, true);
            if (eligible.Count == 0)
                return _default;
            return eligible[Randy.randomInt(0, eligible.Count)];
        }

        public static NationTypeDefinition CycleType(Kingdom kingdom)
        {
            if (kingdom == null)
                return _default;
            var current = GetDefinition(kingdom);
            int index = _definitions.IndexOf(current);
            for (int step = 1; step <= _definitions.Count; step++)
            {
                int nextIndex = (index + step) % _definitions.Count;
                var next = _definitions[nextIndex];
                if (!IsEligible(kingdom, next))
                    continue;
                return TrySetType(kingdom, next, true) ? next : current;
            }
            return current;
        }

        public static bool IsAllowedForAutoReform(NationTypeDefinition definition)
        {
            if (definition == null)
                return false;
            return !XntmConfig.FilterRepetitiveNationTypes || !AutoFilteredIds.Contains(definition.Id);
        }

        public static NationTypeDefinition SelectPoliticalTarget(Kingdom kingdom, IReadOnlyList<NationTypeDefinition> pool)
        {
            if (kingdom == null || pool == null || pool.Count == 0)
                return null;

            Actor ruler = kingdom.king;
            NationTypeDefinition rulerPreference = GetRulerPreferredNationType(kingdom, pool);
            NationTypeDefinition populationPreference = GetPopulationPreferredNationType(kingdom, pool);

            if (rulerPreference == null && populationPreference == null)
                return null;
            if (rulerPreference != null && populationPreference == null)
                return rulerPreference;
            if (populationPreference != null && rulerPreference == null)
                return populationPreference;

            if (ruler != null)
            {
                if (ruler.hasTrait("evil") || ruler.hasTrait("madness") || ruler.hasTrait("bloodlust") || ruler.hasTrait("ambitious"))
                    return rulerPreference;
                if (ruler.hasTrait("peaceful") || ruler.hasTrait("content") || ruler.hasTrait("wise"))
                    return populationPreference;
            }

            return Randy.randomBool() ? rulerPreference : populationPreference;
        }

        public static NationTypeDefinition GetRulerPreferredNationType(Kingdom kingdom, IReadOnlyList<NationTypeDefinition> pool = null)
        {
            Actor ruler = kingdom?.king;
            if (ruler == null || !ruler.isAlive())
                return null;

            NationTypeDefinition traitPreference = GetPreferredNationType(ruler);
            if (IsTypeInPool(kingdom, traitPreference, pool))
                return traitPreference;

            List<string> fallbacks = new List<string>();
            if (ruler.hasTrait("evil") || ruler.hasTrait("ambitious"))
                fallbacks.AddRange(new[] { "military_government", "empire", "autocracy", "despotate" });
            if (ruler.hasTrait("peaceful") || ruler.hasTrait("content"))
                fallbacks.AddRange(new[] { "republic", "constitutional_monarchy", "commonwealth", "federation", "council_republic" });
            if (ruler.hasTrait("wise"))
                fallbacks.AddRange(new[] { "council_republic", "noocracy", "gerontocracy", "diarchy", "triarchy" });
            if (ruler.hasTrait("bloodlust"))
                fallbacks.AddRange(new[] { "military_government", "khanate", "empire", "nomadic_horde" });

            for (int i = 0; i < fallbacks.Count; i++)
            {
                if (!TryGetDefinition(fallbacks[i], out NationTypeDefinition fallback))
                    continue;
                if (IsTypeInPool(kingdom, fallback, pool))
                    return fallback;
            }

            return null;
        }

        public static NationTypeDefinition GetPopulationPreferredNationType(Kingdom kingdom, IReadOnlyList<NationTypeDefinition> pool = null)
        {
            if (kingdom == null)
                return null;

            Dictionary<string, int> counts = new Dictionary<string, int>();
            int sampled = 0;
            foreach (Actor actor in kingdom.getUnits())
            {
                if (actor == null || !actor.isAlive() || actor.asset.is_boat)
                    continue;

                NationTypeDefinition preference = GetPreferredNationType(actor);
                if (preference == null || !IsTypeInPool(kingdom, preference, pool))
                    continue;

                sampled++;
                if (!counts.TryGetValue(preference.Id, out int current))
                    current = 0;
                counts[preference.Id] = current + 1;
                if (sampled >= 220)
                    break;
            }

            if (counts.Count == 0)
                return null;

            string bestId = null;
            int bestVotes = -1;
            foreach (var pair in counts)
            {
                if (pair.Value <= bestVotes)
                    continue;
                bestVotes = pair.Value;
                bestId = pair.Key;
            }

            if (string.IsNullOrEmpty(bestId))
                return null;
            return TryGetDefinition(bestId, out NationTypeDefinition definition) ? definition : null;
        }

        public static float GetPopulationSupportForCurrentType(Kingdom kingdom)
        {
            if (kingdom == null)
                return 0.5f;
            NationTypeDefinition current = GetDefinition(kingdom);
            return GetPopulationSupportForType(kingdom, current?.Id);
        }

        public static float GetCitySupportForCurrentType(City city)
        {
            if (city == null || city.kingdom == null)
                return 0.5f;
            NationTypeDefinition current = GetDefinition(city.kingdom);
            string currentId = current?.Id;
            if (string.IsNullOrEmpty(currentId))
                return 0.5f;
            double now = World.world.getCurWorldTime();
            if (TryGetCachedCitySupport(city, currentId, now, out float cachedSupport))
                return cachedSupport;

            int total = 0;
            int support = 0;
            int count = city.units.Count;
            for (int i = 0; i < count; i++)
            {
                Actor actor = city.units[i];
                if (actor == null || !actor.isAlive() || actor.asset.is_boat)
                    continue;
                NationTypeDefinition pref = GetPreferredNationType(actor);
                if (pref == null)
                    continue;

                total++;
                if (pref.Id == currentId)
                    support++;
                if (total >= 120)
                    break;
            }

            if (total == 0)
                return 0.5f;
            float value = Mathf.Clamp01(support / (float)total);
            CacheCitySupport(city, currentId, value, now);
            return value;
        }

        public static NationTypeDefinition GetPreferredNationType(Actor actor)
        {
            EnsurePreferenceTraitsRegistered();
            if (actor == null || !actor.isAlive() || actor.asset.is_boat)
                return null;
            if (actor.data?.custom_data_string != null && actor.data.custom_data_string.dict.TryGetValue(PreferenceTypeCacheKey, out string cachedTypeId) && !string.IsNullOrEmpty(cachedTypeId) && TryGetDefinition(cachedTypeId, out NationTypeDefinition cachedDefinition))
                return cachedDefinition;

            string typeId = GetPreferenceTypeFromTraits(actor);
            if (!string.IsNullOrEmpty(typeId) && TryGetDefinition(typeId, out NationTypeDefinition existing))
            {
                SetPreferenceTypeCache(actor, existing.Id);
                return existing;
            }

            NationTypeDefinition selected = SelectInitialPreference(actor);
            if (selected == null)
                return null;

            string traitId = GetPreferenceTraitId(selected.Id);
            if (string.IsNullOrEmpty(traitId))
                return selected;

            ActorTrait trait = AssetManager.traits?.get(traitId);
            if (trait != null)
                actor.addTrait(trait, true);
            SetPreferenceTypeCache(actor, selected.Id);
            return selected;
        }

        public static bool HasRecentLeadershipShift(Kingdom kingdom, float withinYears = 20f)
        {
            if (kingdom?.data?.custom_data_float == null)
                return false;
            if (!kingdom.data.custom_data_float.dict.TryGetValue(AutoLeadershipShiftUntilKey, out float until))
                return false;
            double now = World.world.getCurWorldTime();
            if (until <= now)
            {
                kingdom.data.custom_data_float.dict.Remove(AutoLeadershipShiftUntilKey);
                return false;
            }
            return until > now;
        }

        public static float GetAutoReformCooldownYears(Kingdom kingdom)
        {
            if (kingdom == null)
                return 24f;

            float loyalty = GetAverageLoyalty(kingdom);
            float support = GetPopulationSupportForCurrentType(kingdom);
            bool leadershipShift = HasRecentLeadershipShift(kingdom, 30f);
            Actor ruler = kingdom.king;
            float councilBias = BuildCouncilReformBias(kingdom);

            float cooldown = 26f;
            cooldown -= Mathf.Clamp((55f - loyalty) * 0.16f, -6f, 8f);
            cooldown -= Mathf.Clamp((0.5f - support) * 30f, -4f, 7f);
            cooldown -= leadershipShift ? 8f : 0f;
            cooldown -= councilBias * 4f;

            if (ruler != null)
            {
                if (ruler.hasTrait("ambitious") || ruler.hasTrait("madness"))
                    cooldown -= 4f;
                if (ruler.hasTrait("evil"))
                    cooldown -= 2f;
                if (ruler.hasTrait("peaceful") || ruler.hasTrait("content"))
                    cooldown += 5f;
                cooldown -= Mathf.Clamp((ruler.stats?.get("personality_aggression") ?? 0f) * 6f, -3f, 4f);
            }

            return Mathf.Clamp(cooldown, 7f, 48f);
        }

        private static bool IsTypeInPool(Kingdom kingdom, NationTypeDefinition definition, IReadOnlyList<NationTypeDefinition> pool)
        {
            if (definition == null)
                return false;
            if (!IsEligible(kingdom, definition))
                return false;
            if (pool == null || pool.Count == 0)
                return true;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i] != null && pool[i].Id == definition.Id)
                    return true;
            }
            return false;
        }

        private static float GetPopulationSupportForType(Kingdom kingdom, string typeId)
        {
            if (kingdom == null || string.IsNullOrEmpty(typeId))
                return 0.5f;

            int total = 0;
            int support = 0;
            foreach (Actor actor in kingdom.getUnits())
            {
                if (actor == null || !actor.isAlive() || actor.asset.is_boat)
                    continue;

                NationTypeDefinition preference = GetPreferredNationType(actor);
                if (preference == null)
                    continue;

                total++;
                if (preference.Id == typeId)
                    support++;
                if (total >= 220)
                    break;
            }

            if (total == 0)
                return 0.5f;
            return Mathf.Clamp01(support / (float)total);
        }

        private static string GetPreferenceTypeFromTraits(Actor actor)
        {
            if (actor == null)
                return string.Empty;
            foreach (var pair in _typeToPreferenceTraitId)
            {
                if (actor.hasTrait(pair.Value))
                    return pair.Key;
            }
            return string.Empty;
        }

        private static NationTypeDefinition SelectInitialPreference(Actor actor)
        {
            if (actor == null)
                return _default;

            List<NationTypeDefinition> pool = new List<NationTypeDefinition>();
            Kingdom kingdom = actor.kingdom;
            if (kingdom != null)
            {
                NationTypeDefinition current = GetDefinition(kingdom);
                if (current != null)
                    pool.Add(current);
            }

            if (actor.hasTrait("evil") || actor.hasTrait("ambitious"))
                AddPreferredIds(pool, kingdom, "military_government", "empire", "autocracy", "despotate", "khanate");
            if (actor.hasTrait("peaceful") || actor.hasTrait("content"))
                AddPreferredIds(pool, kingdom, "republic", "constitutional_monarchy", "federation", "commonwealth", "confederation", "council_republic");
            if (actor.hasTrait("wise"))
                AddPreferredIds(pool, kingdom, "council_republic", "noocracy", "gerontocracy", "diarchy", "triarchy");
            if (actor.hasTrait("bloodlust"))
                AddPreferredIds(pool, kingdom, "military_government", "nomadic_horde", "khanate", "empire");
            if (actor.hasTrait("greedy"))
                AddPreferredIds(pool, kingdom, "merchant_republic", "trade_league", "timocracy");

            if (pool.Count == 0)
            {
                List<NationTypeDefinition> eligible = GetEligibleDefinitions(kingdom);
                if (eligible.Count == 0)
                    return _default;
                return eligible[Randy.randomInt(0, eligible.Count)];
            }

            return pool[Randy.randomInt(0, pool.Count)];
        }

        private static void AddPreferredIds(List<NationTypeDefinition> pool, Kingdom kingdom, params string[] ids)
        {
            if (ids == null || pool == null)
                return;
            for (int i = 0; i < ids.Length; i++)
            {
                if (!TryGetDefinition(ids[i], out NationTypeDefinition definition))
                    continue;
                if (!IsEligible(kingdom, definition))
                    continue;
                if (!pool.Any(entry => entry.Id == definition.Id))
                    pool.Add(definition);
            }
        }

        private static float GetAverageLoyalty(Kingdom kingdom)
        {
            if (kingdom == null)
                return 60f;

            int count = 0;
            float total = 0f;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                total += Mathf.Clamp(city.getLoyalty(), 0, 100);
                count++;
            }

            if (count == 0)
                return 60f;
            return total / count;
        }

        private static float BuildCouncilReformBias(Kingdom kingdom)
        {
            if (kingdom == null)
                return 0f;

            List<Actor> council = CouncilManager.GetRulers(kingdom);
            if (council == null || council.Count == 0)
                return 0f;

            float score = 0f;
            int considered = 0;
            for (int i = 0; i < council.Count; i++)
            {
                Actor actor = council[i];
                if (actor == null || !actor.isAlive())
                    continue;
                considered++;
                if (actor.hasTrait("ambitious") || actor.hasTrait("madness"))
                    score += 1.1f;
                if (actor.hasTrait("evil"))
                    score += 0.8f;
                if (actor.hasTrait("content") || actor.hasTrait("peaceful"))
                    score -= 1f;
                score += (actor.stats?.get("personality_aggression") ?? 0f) * 0.8f;
                score -= (actor.stats?.get("personality_diplomatic") ?? 0f) * 0.4f;
            }

            if (considered == 0)
                return 0f;
            return Mathf.Clamp(score / considered, -1f, 1f);
        }

        public static bool IsRepublicGovernment(Kingdom kingdom)
        {
            NationTypeDefinition definition = GetDefinition(kingdom);
            if (definition == null)
                return false;
            return RepublicIds.Contains(definition.Id);
        }

        public static void TickRepublicLeadership(Kingdom kingdom)
        {
            if (kingdom == null || !kingdom.isAlive() || kingdom.wild || !XntmConfig.RepublicElections)
                return;
            if (!IsRepublicGovernment(kingdom))
                return;
            if (!kingdom.hasKing())
                return;

            Actor incumbent = kingdom.king;
            if (incumbent == null || !incumbent.isAlive() || incumbent.kingdom != kingdom)
                return;

            double now = World.world.getCurWorldTime();
            EnsureRepublicElectionData(kingdom);
            SyncRepublicElectionLeader(kingdom, incumbent, now);

            float termLimit = XntmConfig.RepublicTermLimitYears;
            float yearsInOffice = GetYearsInOffice(kingdom, now);
            bool termExpired = termLimit > 0f && yearsInOffice >= termLimit;
            float dueAt = GetRepublicElectionDue(kingdom, now, termLimit);
            bool scheduleExpired = now >= dueAt;

            if (!termExpired && !scheduleExpired)
                return;

            if (TryRunRepublicElection(kingdom, incumbent, now))
                return;

            float retry = Mathf.Clamp(GetRepublicElectionInterval(termLimit) * 0.35f, 0.8f, 4f);
            SetRepublicElectionDue(kingdom, (float)(now + retry));
        }

        private static void EnsureRepublicElectionData(Kingdom kingdom)
        {
            if (kingdom?.data == null)
                return;
            kingdom.data.custom_data_long ??= new CustomDataContainer<long>();
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
        }

        private static void SyncRepublicElectionLeader(Kingdom kingdom, Actor incumbent, double now)
        {
            if (kingdom?.data?.custom_data_long == null || kingdom.data.custom_data_float == null || incumbent == null)
                return;

            long currentLeaderId = incumbent.getID();
            bool hasStored = kingdom.data.custom_data_long.dict.TryGetValue(RepublicElectionLeaderKey, out long storedLeaderId);
            if (hasStored && storedLeaderId == currentLeaderId)
                return;

            kingdom.data.custom_data_long.dict[RepublicElectionLeaderKey] = currentLeaderId;
            float termLimit = XntmConfig.RepublicTermLimitYears;
            SetRepublicElectionDue(kingdom, (float)(now + GetRepublicElectionInterval(termLimit)));
        }

        private static float GetYearsInOffice(Kingdom kingdom, double now)
        {
            if (kingdom?.data == null)
                return 0f;
            double startedAt = kingdom.data.timestamp_king_rule;
            if (startedAt <= 0d || startedAt > now)
                return 0f;
            return Mathf.Max(0f, (float)(now - startedAt));
        }

        private static float GetRepublicElectionDue(Kingdom kingdom, double now, float termLimit)
        {
            if (kingdom?.data == null)
                return (float)(now + GetRepublicElectionInterval(termLimit));

            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            if (!kingdom.data.custom_data_float.dict.TryGetValue(RepublicElectionDueKey, out float dueAt))
            {
                dueAt = (float)(now + GetRepublicElectionInterval(termLimit));
                kingdom.data.custom_data_float.dict[RepublicElectionDueKey] = dueAt;
            }

            return dueAt;
        }

        private static void SetRepublicElectionDue(Kingdom kingdom, float dueAt)
        {
            if (kingdom?.data == null)
                return;
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            kingdom.data.custom_data_float.dict[RepublicElectionDueKey] = dueAt;
        }

        private static float GetRepublicElectionInterval(float termLimit)
        {
            float min = Mathf.Max(0.5f, XntmConfig.RepublicElectionMinYears);
            float max = Mathf.Max(min, XntmConfig.RepublicElectionMaxYears);
            if (termLimit > 0f)
                max = Mathf.Min(max, termLimit);
            if (max < min)
                max = min;
            return Randy.randomFloat(min, max);
        }

        private static bool TryRunRepublicElection(Kingdom kingdom, Actor incumbent, double now)
        {
            Actor challenger = SelectRepublicChallenger(kingdom, incumbent);
            if (challenger == null)
                return false;

            if (kingdom.hasKing() && kingdom.king == incumbent)
                kingdom.kingLeftEvent();

            if (challenger.hasCity())
            {
                challenger.stopBeingWarrior();
                if (challenger.isCityLeader())
                    challenger.city.removeLeader();
            }

            if (kingdom.hasCapital() && challenger.city != kingdom.capital)
                challenger.joinCity(kingdom.capital);

            kingdom.setKing(challenger);
            kingdom.data.timer_new_king = 0f;
            WorldLog.logNewKing(kingdom);

            EnsureRepublicElectionData(kingdom);
            kingdom.data.custom_data_long.dict[RepublicElectionLeaderKey] = challenger.getID();
            SetRepublicElectionDue(kingdom, (float)(now + GetRepublicElectionInterval(XntmConfig.RepublicTermLimitYears)));
            MarkLeadershipShift(kingdom, now, 20f);
            return true;
        }

        private static Actor SelectRepublicChallenger(Kingdom kingdom, Actor incumbent)
        {
            using (ListPool<Actor> allCandidates = new ListPool<Actor>())
            using (ListPool<Actor> alternatePartyCandidates = new ListPool<Actor>())
            {
                Clan incumbentClan = incumbent?.clan != null && incumbent.clan.isAlive() ? incumbent.clan : null;
                foreach (Actor actor in kingdom.getUnits())
                {
                    if (!IsRepublicElectionCandidate(kingdom, actor, incumbent))
                        continue;

                    allCandidates.Add(actor);
                    if (XntmConfig.RepublicPreferClanSwitch && incumbentClan != null && actor.clan != null && actor.clan.isAlive() && actor.clan != incumbentClan)
                        alternatePartyCandidates.Add(actor);
                }

                if (allCandidates.Count == 0)
                    return null;

                ListPool<Actor> source = alternatePartyCandidates.Count > 0 ? alternatePartyCandidates : allCandidates;
                return PickRepublicElectionWinner(source);
            }
        }

        private static bool IsRepublicElectionCandidate(Kingdom kingdom, Actor actor, Actor incumbent)
        {
            return actor != null
                && actor != incumbent
                && actor.isAlive()
                && !actor.asset.is_boat
                && !actor.isBaby()
                && actor.kingdom == kingdom;
        }

        private static Actor PickRepublicElectionWinner(ListPool<Actor> candidates)
        {
            if (candidates == null || candidates.Count == 0)
                return null;

            Actor best = null;
            float bestScore = float.MinValue;
            for (int i = 0; i < candidates.Count; i++)
            {
                Actor actor = candidates[i];
                float score = BuildRepublicElectionScore(actor);
                if (score <= bestScore)
                    continue;
                best = actor;
                bestScore = score;
            }

            return best ?? candidates.GetRandom();
        }

        private static float BuildRepublicElectionScore(Actor actor)
        {
            if (actor == null)
                return float.MinValue;

            float diplomacy = actor.stats?.get("diplomacy") ?? 0f;
            float cities = actor.stats?.get("cities") ?? 0f;
            float renown = actor.data?.renown ?? 0f;
            float diplomatic = actor.stats?.get("personality_diplomatic") ?? 0f;
            float aggression = actor.stats?.get("personality_aggression") ?? 0f;
            float clanRenown = actor.clan?.getRenown() ?? 0f;
            float random = Randy.randomFloat(-2.5f, 2.5f);

            return diplomacy * 0.75f
                + cities * 0.35f
                + renown * 0.22f
                + diplomatic * 0.6f
                - aggression * 0.35f
                + clanRenown * 0.05f
                + random;
        }

        public static bool IsCouncilRepublic(Kingdom kingdom)
        {
            NationTypeDefinition definition = GetDefinition(kingdom);
            if (definition == null)
                return false;
            return string.Equals(definition.Id, "council_republic", StringComparison.Ordinal);
        }

        public static bool IsLeaderlessGovernment(Kingdom kingdom)
        {
            NationTypeDefinition definition = GetDefinition(kingdom);
            if (definition == null)
                return false;
            if (definition.SuccessionMode == NationSuccessionMode.None)
                return true;
            return string.Equals(definition.Id, "council_republic", StringComparison.Ordinal);
        }

        public static Actor SelectHeir(Kingdom kingdom, Actor exclude)
        {
            EnsureTraitsRegistered();
            if (kingdom == null)
                return null;
            if (IsLeaderlessGovernment(kingdom))
                return null;
            var def = GetDefinition(kingdom);
            switch (def.SuccessionMode)
            {
                case NationSuccessionMode.RoyalLine:
                    return SuccessionTool.getKingFromRoyalClan(kingdom, exclude);
                case NationSuccessionMode.Elective:
                    return SelectByComparator(kingdom, exclude, ListSorters.sortUnitByRenown);
                case NationSuccessionMode.Council:
                    return SelectCouncilRepresentative(kingdom, exclude);
                case NationSuccessionMode.Religious:
                    return SelectReligious(kingdom, exclude);
                case NationSuccessionMode.AgeRule:
                    return SelectByComparator(kingdom, exclude, ListSorters.sortUnitByAgeOldFirst);
                case NationSuccessionMode.WealthRule:
                    return SelectByComparator(kingdom, exclude, (left, right) => ListSorters.sortUnitByGoldCoins(left, right));
                case NationSuccessionMode.Random:
                    return SelectRandom(kingdom, exclude);
                case NationSuccessionMode.None:
                    return null;
                default:
                    return SuccessionTool.getKingFromRoyalClan(kingdom, exclude);
            }
        }

        public static bool IsNoRulerType(Kingdom kingdom)
        {
            return IsLeaderlessGovernment(kingdom);
        }

        private static Actor SelectByComparator(Kingdom kingdom, Actor exclude, Comparison<Actor> sorter, Predicate<Actor> filter = null)
        {
            using (ListPool<Actor> pool = new ListPool<Actor>())
            {
                foreach (Actor actor in kingdom.getUnits())
                {
                    if (actor == null || !actor.isAlive() || actor.asset.is_boat || actor == exclude)
                        continue;
                    if (filter != null && !filter(actor))
                        continue;
                    pool.Add(actor);
                }
                if (pool.Count == 0)
                    return null;
                if (sorter == null)
                    return pool[0];
                Actor[] sorted = pool.ToArray();
                Array.Sort(sorted, sorter);
                return sorted[0];
            }
        }

        private static Actor SelectReligious(Kingdom kingdom, Actor exclude)
        {
            using (ListPool<Actor> pool = new ListPool<Actor>())
            {
                foreach (Actor actor in kingdom.getUnits())
                {
                    if (actor == null || actor == exclude || !actor.isAlive() || actor.asset.is_boat || !actor.hasReligion())
                        continue;
                    pool.Add(actor);
                }
                if (pool.Count == 0)
                return SelectByComparator(kingdom, exclude, ListSorters.sortUnitByRenown);
            Actor[] sorted = pool.ToArray();
            Array.Sort(sorted, (left, right) =>
            {
                bool leftMatch = left.religion == kingdom.religion;
                    bool rightMatch = right.religion == kingdom.religion;
                    if (leftMatch != rightMatch)
                        return leftMatch ? -1 : 1;
                    return ListSorters.sortUnitByRenown(left, right);
                });
                return sorted[0];
            }
        }

        private static Actor SelectRandom(Kingdom kingdom, Actor exclude)
        {
            using (ListPool<Actor> pool = new ListPool<Actor>())
            {
                foreach (Actor actor in kingdom.getUnits())
                {
                    if (actor == null || !actor.isAlive() || actor.asset.is_boat || actor == exclude)
                        continue;
                    pool.Add(actor);
                }
                if (pool.Count == 0)
                    return null;
                return pool.GetRandom();
            }
        }

        private static Actor SelectCouncilRepresentative(Kingdom kingdom, Actor exclude)
        {
            return SelectByComparator(kingdom, exclude, (left, right) =>
            {
                float leftScore = BuildCouncilScore(kingdom, left);
                float rightScore = BuildCouncilScore(kingdom, right);
                int comparison = -leftScore.CompareTo(rightScore);
                return comparison != 0 ? comparison : ListSorters.sortUnitByRenown(left, right);
            });
        }

        private static float BuildCouncilScore(Kingdom kingdom, Actor actor)
        {
            if (actor == null)
                return float.MinValue;

            bool atWar = kingdom != null && kingdom.hasEnemies();
            float diplomacy = actor.stats?.get("diplomacy") ?? 0f;
            float warfare = actor.stats?.get("warfare") ?? 0f;
            float renown = actor.data?.renown ?? 0f;
            float capitalBonus = kingdom != null && actor.city == kingdom.capital ? 2.5f : 0f;

            float diplomacyWeight = atWar ? 0.45f : 0.65f;
            float warfareWeight = atWar ? 0.35f : 0.15f;
            float renownWeight = 0.2f;

            return (diplomacy * diplomacyWeight) + (warfare * warfareWeight) + (renown * renownWeight) + capitalBonus;
        }

        private static Actor SelectAnarchyFigurehead(Kingdom kingdom, Actor exclude)
        {
            Actor candidate = SelectByComparator(kingdom, exclude, (left, right) =>
            {
                float leftScore = BuildAnarchyScore(left);
                float rightScore = BuildAnarchyScore(right);
                int comparison = -leftScore.CompareTo(rightScore);
                return comparison != 0 ? comparison : ListSorters.sortUnitByRenown(left, right);
            });

            if (candidate == null)
                return null;

            bool forcedByWar = kingdom != null && kingdom.hasEnemies();
            
            if (forcedByWar || Randy.randomChance(0.6f))
                return candidate;

            return null;
        }

        private static float BuildAnarchyScore(Actor actor)
        {
            if (actor == null)
                return float.MinValue;

            float kills = actor.data?.kills ?? 0f;
            float warfare = actor.stats?.get("warfare") ?? 0f;
            float renown = actor.data?.renown ?? 0f;

            return (kills * 0.55f) + (warfare * 0.25f) + (renown * 0.2f);
        }

        private sealed class RebellionBirthContext
        {
            public NationTypeDefinition PreviousType { get; }

            public RebellionBirthContext(NationTypeDefinition previousType)
            {
                PreviousType = previousType;
            }
        }

        private static string GetStoredType(Kingdom kingdom)
        {
            if (kingdom == null)
                return string.Empty;

            string fromTraits = GetTypeFromTraits(kingdom);
            if (!string.IsNullOrEmpty(fromTraits))
                return fromTraits;

            string legacy = GetLegacyStoredType(kingdom);
            if (!string.IsNullOrEmpty(legacy))
            {
                SetStoredType(kingdom, legacy);
                return legacy;
            }

            return string.Empty;
        }

        private static string GetTypeFromTraits(Kingdom kingdom)
        {
            foreach (var pair in _typeToTraitId)
            {
                if (kingdom.hasTrait(pair.Value))
                    return pair.Key;
            }
            return string.Empty;
        }

        private static string GetLegacyStoredType(Kingdom kingdom)
        {
            if (kingdom?.data?.custom_data_string != null && kingdom.data.custom_data_string.TryGetValue(CustomDataKey, out var stored))
                return stored;
            return string.Empty;
        }

        private static void SetStoredType(Kingdom kingdom, string typeId)
        {
            if (kingdom == null || string.IsNullOrEmpty(typeId))
                return;
            if (!_definitionMap.ContainsKey(typeId))
                return;

            EnsureTraitsRegistered();
            RemoveNationTypeTraits(kingdom);

            string traitId = GetTraitId(typeId);
            if (!string.IsNullOrEmpty(traitId))
                kingdom.addTrait(traitId, true);

            ClearLegacyCustomData(kingdom);
        }

        private static void RemoveNationTypeTraits(Kingdom kingdom)
        {
            if (kingdom == null)
                return;

            foreach (var traitId in _typeToTraitId.Values)
            {
                if (kingdom.hasTrait(traitId))
                    kingdom.removeTrait(traitId);
            }
        }

        private static string GetTraitId(string typeId)
        {
            return _typeToTraitId.TryGetValue(typeId, out var traitId) ? traitId : string.Empty;
        }

        private static string GetPreferenceTraitId(string typeId)
        {
            return _typeToPreferenceTraitId.TryGetValue(typeId, out var traitId) ? traitId : string.Empty;
        }

        private static void ClearLegacyCustomData(Kingdom kingdom)
        {
            if (kingdom?.data?.custom_data_string == null)
                return;

            
            kingdom.data.custom_data_string.Remove(CustomDataKey);
        }

        public static bool IsEligible(Kingdom kingdom, NationTypeDefinition definition)
        {
            if (definition == null)
                return false;
            if (kingdom == null)
                return true;
            if (definition.SuccessionMode == NationSuccessionMode.Religious && !HasReligion(kingdom))
                return false;
            if (definition.Id == "empire" && !HasCulturalDiversity(kingdom))
                return false;
            if (MaritimeIds.Contains(definition.Id) && !HasMaritimeAccess(kingdom))
                return false;
            if (ClanRequiredIds.Contains(definition.Id) && !HasClanPresence(kingdom))
                return false;
            if (IsRanked(definition.Id) && !MeetsRankThreshold(definition.Id, kingdom))
                return false;
            return true;
        }

        public static NationTypeDefinition GetNaturalType(Kingdom kingdom)
        {
            if (kingdom == null)
                return _default;
            if (_definitionMap.TryGetValue("empire", out var empire) && IsEligible(kingdom, empire) && HasCulturalDiversity(kingdom))
                return empire;
            var ranked = GetRankedType(kingdom);
            if (ranked != null)
                return ranked;
            List<NationTypeDefinition> eligible = GetEligibleDefinitions(kingdom);
            if (eligible.Count > 0)
                return eligible[0];
            return _default;
        }

        public static bool TrySetType(Kingdom kingdom, NationTypeDefinition definition, bool allowIneligible = false)
        {
            if (kingdom == null || definition == null)
                return false;
            if (!allowIneligible && !IsEligible(kingdom, definition))
                return false;
            NationTypeDefinition current = GetDefinition(kingdom);
            if (current != null && current.Id == definition.Id)
                return false;
            SetStoredType(kingdom, definition.Id);
            NationNamingHelper.ApplyAccurateName(kingdom, kingdom.data?.name, true);
            LogNationTypeChange(kingdom, current, definition);
            return true;
        }

        private static void LogNationTypeChange(Kingdom kingdom, NationTypeDefinition previous, NationTypeDefinition next)
        {
            if (kingdom == null || previous == null || next == null)
                return;
            if (string.Equals(previous.Id, next.Id, StringComparison.Ordinal))
                return;

            WorldLogAsset asset = AssetManager.world_log_library?.get(NationTypeChangedLogId);
            if (asset == null)
                return;

            Color kingdomColor = kingdom.getColor()?.getColorText() ?? Color.white;
            WorldLogMessage message = new WorldLogMessage(asset, kingdom.name, previous.GetLocalizedName(), next.GetLocalizedName())
            {
                kingdom = kingdom,
                location = (Vector2)kingdom.location,
                color_special1 = kingdomColor,
                color_special2 = kingdomColor,
                color_special3 = kingdomColor
            };
            message.add();
        }

        public static void TickAuto(Kingdom kingdom)
        {
            if (kingdom == null || kingdom.wild || !kingdom.isAlive())
                return;
            TickRepublicLeadership(kingdom);
            double now = World.world.getCurWorldTime();
            if (TryGetNextCheck(kingdom, out float next) && now < next)
                return;
            UpdateLeadershipMarkers(kingdom, now);
            SetNextCheck(kingdom, (float)(now + GetAutoReformCooldownYears(kingdom)));
            NationTypeDefinition target = DetermineAutoTarget(kingdom);
            NationTypeDefinition current = GetDefinition(kingdom);
            if (target != null && target != current)
            {
                TrySetType(kingdom, target, true);
                SetNextCheck(kingdom, (float)(now + GetAutoReformCooldownYears(kingdom) + Randy.randomFloat(6f, 14f)));
            }
        }

        private static NationTypeDefinition DetermineAutoTarget(Kingdom kingdom)
        {
            NationTypeDefinition current = GetDefinition(kingdom);
            if (!IsEligible(kingdom, current))
                return GetNaturalType(kingdom);
            if (current.Id == "empire" && !HasCulturalDiversity(kingdom))
                return GetRankedType(kingdom) ?? _default;

            float loyalty = GetAverageLoyalty(kingdom);
            float support = GetPopulationSupportForCurrentType(kingdom);
            float pressure = Mathf.Clamp01((0.52f - support) * 1.5f + (58f - loyalty) / 68f);
            bool leadershipShift = HasRecentLeadershipShift(kingdom, 30f);
            float councilBias = BuildCouncilReformBias(kingdom);
            Actor ruler = kingdom.king;

            if (!leadershipShift && pressure < 0.22f)
                return current;

            float chance = 0.08f + pressure * 0.55f + councilBias * 0.14f + (leadershipShift ? 0.22f : 0f);
            if (ruler != null)
            {
                if (ruler.hasTrait("ambitious") || ruler.hasTrait("madness"))
                    chance += 0.12f;
                if (ruler.hasTrait("evil"))
                    chance += 0.08f;
                if (ruler.hasTrait("peaceful") || ruler.hasTrait("content"))
                    chance -= 0.14f;
                chance += Mathf.Clamp((ruler.stats?.get("personality_aggression") ?? 0f) * 0.12f, -0.06f, 0.08f);
            }

            if (!Randy.randomChance(Mathf.Clamp01(chance)))
                return current;

            List<NationTypeDefinition> pool = GetEligibleDefinitions(kingdom);
            pool.RemoveAll(def => def == null || def.Id == current.Id);
            if (pool.Count == 0)
                return current;

            NationTypeDefinition target = SelectPoliticalTarget(kingdom, pool);
            return target ?? current;
        }

        private static void UpdateLeadershipMarkers(Kingdom kingdom, double now)
        {
            if (kingdom?.data == null)
                return;

            kingdom.data.custom_data_long ??= new CustomDataContainer<long>();
            kingdom.data.custom_data_string ??= new CustomDataContainer<string>();
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();

            long currentKing = kingdom.king?.getID() ?? -1L;
            if (!kingdom.data.custom_data_long.dict.TryGetValue(AutoLastKingKey, out long storedKing))
            {
                kingdom.data.custom_data_long.dict[AutoLastKingKey] = currentKing;
            }
            else if (storedKing != currentKing)
            {
                kingdom.data.custom_data_long.dict[AutoLastKingKey] = currentKing;
                MarkLeadershipShift(kingdom, now, 24f);
            }

            string signature = BuildCouncilSignature(kingdom);
            if (!kingdom.data.custom_data_string.dict.TryGetValue(AutoLastCouncilSignatureKey, out string storedSignature))
            {
                kingdom.data.custom_data_string.dict[AutoLastCouncilSignatureKey] = signature;
            }
            else if (!string.Equals(storedSignature, signature, StringComparison.Ordinal))
            {
                kingdom.data.custom_data_string.dict[AutoLastCouncilSignatureKey] = signature;
                MarkLeadershipShift(kingdom, now, 18f);
            }
        }

        private static void MarkLeadershipShift(Kingdom kingdom, double now, float years)
        {
            if (kingdom?.data == null)
                return;
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            float until = (float)(now + years + Randy.randomFloat(1f, 5f));
            if (!kingdom.data.custom_data_float.dict.TryGetValue(AutoLeadershipShiftUntilKey, out float currentUntil) || currentUntil < until)
                kingdom.data.custom_data_float.dict[AutoLeadershipShiftUntilKey] = until;
        }

        private static string BuildCouncilSignature(Kingdom kingdom)
        {
            List<Actor> council = CouncilManager.GetRulers(kingdom);
            if (council == null || council.Count == 0)
                return string.Empty;

            List<long> ids = new List<long>(council.Count);
            for (int i = 0; i < council.Count; i++)
            {
                Actor actor = council[i];
                if (actor == null || !actor.isAlive())
                    continue;
                ids.Add(actor.getID());
            }

            if (ids.Count == 0)
                return string.Empty;
            ids.Sort();
            return string.Join(",", ids);
        }

        private static bool TryGetNextCheck(Kingdom kingdom, out float next)
        {
            if (kingdom?.data?.custom_data_float != null && kingdom.data.custom_data_float.dict.TryGetValue(AutoCheckKey, out next))
                return true;
            next = 0f;
            return false;
        }

        private static void SetNextCheck(Kingdom kingdom, float when)
        {
            if (kingdom?.data == null)
                return;
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            kingdom.data.custom_data_float.dict[AutoCheckKey] = when;
        }

        private static bool TryGetCachedCitySupport(City city, string typeId, double now, out float support)
        {
            support = 0.5f;
            if (city?.data?.custom_data_float == null || city.data.custom_data_string == null)
                return false;
            if (!city.data.custom_data_string.dict.TryGetValue(CitySupportCacheTypeKey, out string cachedType) || !string.Equals(cachedType, typeId, StringComparison.Ordinal))
                return false;
            if (!city.data.custom_data_float.dict.TryGetValue(CitySupportCacheUntilKey, out float until) || now >= until)
                return false;
            if (!city.data.custom_data_float.dict.TryGetValue(CitySupportCacheValueKey, out support))
                return false;
            support = Mathf.Clamp01(support);
            return true;
        }

        private static void CacheCitySupport(City city, string typeId, float support, double now)
        {
            if (city?.data == null)
                return;
            city.data.custom_data_float ??= new CustomDataContainer<float>();
            city.data.custom_data_string ??= new CustomDataContainer<string>();
            city.data.custom_data_float.dict[CitySupportCacheValueKey] = Mathf.Clamp01(support);
            city.data.custom_data_float.dict[CitySupportCacheUntilKey] = (float)(now + 3f + Randy.randomFloat(0f, 1.2f));
            city.data.custom_data_string.dict[CitySupportCacheTypeKey] = typeId;
        }

        private static void SetPreferenceTypeCache(Actor actor, string typeId)
        {
            if (actor?.data == null || string.IsNullOrEmpty(typeId))
                return;
            actor.data.custom_data_string ??= new CustomDataContainer<string>();
            actor.data.custom_data_string.dict[PreferenceTypeCacheKey] = typeId;
        }

        private static List<NationTypeDefinition> GetEligibleDefinitions(Kingdom kingdom, bool autoReformFiltered = false)
        {
            List<NationTypeDefinition> result = new List<NationTypeDefinition>(_definitions.Count);
            foreach (NationTypeDefinition def in _definitions)
            {
                if (autoReformFiltered && !IsAllowedForAutoReform(def))
                    continue;
                if (IsEligible(kingdom, def))
                    result.Add(def);
            }
            return result;
        }

        private static bool HasReligion(Kingdom kingdom)
        {
            return kingdom?.religion != null;
        }

        private static bool HasCulturalDiversity(Kingdom kingdom)
        {
            if (kingdom == null)
                return false;
            Culture kingdomCulture = kingdom.culture;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                Culture cityCulture = city.culture;
                if (cityCulture != null && cityCulture != kingdomCulture)
                    return true;
            }
            return false;
        }

        private static bool HasMaritimeAccess(Kingdom kingdom)
        {
            if (kingdom == null)
                return false;
            if (IsCoastal(kingdom.capital))
                return true;
            foreach (City city in kingdom.getCities())
            {
                if (IsCoastal(city))
                    return true;
            }
            return false;
        }

        private static bool IsCoastal(City city)
        {
            if (city == null || !city.isAlive())
                return false;
            foreach (Building building in city.buildings)
            {
                if (building?.asset != null && building.asset.docks)
                    return true;
            }
            foreach (TileZone zone in city.zones)
            {
                if (zone?.centerTile != null && zone.centerTile.Type.ocean)
                    return true;
            }
            return false;
        }

        private static bool HasClanPresence(Kingdom kingdom)
        {
            if (kingdom == null)
                return false;
            foreach (Actor unit in kingdom.getUnits())
            {
                if (unit == null || !unit.isAlive() || unit.asset.is_boat)
                    continue;
                if (unit.clan != null && unit.clan.isAlive())
                    return true;
            }
            return false;
        }

        private static bool IsRanked(string id)
        {
            return RankOrder.Contains(id);
        }

        private static bool MeetsRankThreshold(string id, Kingdom kingdom)
        {
            int cities = kingdom?.countCities() ?? 0;
            int pop = kingdom?.getPopulationTotal() ?? 0;
            for (int i = 0; i < RankThresholds.Length; i++)
            {
                RankThreshold threshold = RankThresholds[i];
                if (threshold.Id == id)
                    return cities >= threshold.MinCities && pop >= threshold.MinPopulation;
            }
            return true;
        }

        private static NationTypeDefinition GetRankedType(Kingdom kingdom)
        {
            int cities = kingdom?.countCities() ?? 0;
            int pop = kingdom?.getPopulationTotal() ?? 0;
            for (int i = RankThresholds.Length - 1; i >= 0; i--)
            {
                RankThreshold threshold = RankThresholds[i];
                if (cities >= threshold.MinCities && pop >= threshold.MinPopulation && _definitionMap.TryGetValue(threshold.Id, out var def) && IsEligible(kingdom, def))
                    return def;
            }
            return null;
        }

        public static void RegisterTraits()
        {
            EnsureTraitsRegistered();
            EnsurePreferenceTraitsRegistered();
        }

        private static void EnsureTraitsRegistered()
        {
            if (_traitsRegistered)
            {
                EnsurePreferenceTraitsRegistered();
                return;
            }

            var library = AssetManager.kingdoms_traits;
            if (library == null)
                return;

            foreach (var definition in _definitions)
            {
                string traitId = GetTraitId(definition.Id);
                KingdomTrait trait = library.get(traitId);
                if (trait == null)
                {
                    trait = new KingdomTrait
                    {
                        id = traitId,
                        group_id = TraitGroupId,
                        spawn_random_trait_allowed = false,
                        can_be_in_book = false,
                        rarity = Rarity.R0_Normal
                    };
                    library.add(trait);
                }

                
                trait.group_id = TraitGroupId;
                trait.has_localized_id = true;
                trait.has_description_1 = !string.IsNullOrEmpty(definition.DescriptionKey);
                trait.has_description_2 = false;
                trait.special_icon_logic = false;
                trait.show_for_unlockables_ui = false;
                trait.path_icon = BuildIconPath(definition.Id);
                trait.special_locale_id = definition.DisplayNameKey;
                trait.special_locale_description = definition.DescriptionKey;
                trait.special_locale_description_2 = string.Empty;
                trait.can_be_in_book = false;
                trait.spawn_random_trait_allowed = false;
                trait.spawn_random_rate = 0;
                trait.has_locales = true;
                _traitAssets.Add(trait);
            }

            BuildOpposites();
            FinalizeOpposites(library);
            _traitsRegistered = true;
            EnsurePreferenceTraitsRegistered();
        }

        private static void EnsurePreferenceTraitsRegistered()
        {
            if (_preferenceTraitsRegistered)
                return;

            var library = AssetManager.traits;
            if (library == null)
                return;
            if (!EnsurePreferenceTraitGroupRegistered())
                return;

            bool noTouch = XntmConfig.PreferenceTraitsNoTouch;
            _preferenceTraitAssets.Clear();

            foreach (NationTypeDefinition definition in _definitions)
            {
                string traitId = GetPreferenceTraitId(definition.Id);
                ActorTrait trait = library.get(traitId);
                if (trait == null)
                {
                    trait = new ActorTrait
                    {
                        id = traitId
                    };
                    library.add(trait);
                }

                trait.group_id = PreferenceTraitGroupId;
                trait.path_icon = BuildIconPath(definition.Id);
                trait.has_localized_id = true;
                trait.special_locale_id = traitId;
                trait.has_description_1 = true;
                trait.special_locale_description = "xntm_prefers_description";
                trait.has_description_2 = false;
                trait.special_locale_description_2 = string.Empty;
                trait.can_be_in_book = false;
                trait.spawn_random_trait_allowed = false;
                trait.spawn_random_rate = 0;
                trait.rate_birth = 0;
                trait.rate_acquire_grow_up = 0;
                trait.rate_inherit = 0;
                trait.show_for_unlockables_ui = false;
                trait.show_in_meta_editor = !noTouch;
                trait.can_be_given = !noTouch;
                trait.can_be_removed = !noTouch;
                _preferenceTraitAssets.Add(trait);
            }

            BuildPreferenceOpposites();
            FinalizePreferenceOpposites(library);
            _preferenceTraitsRegistered = true;
        }

        private static bool EnsurePreferenceTraitGroupRegistered()
        {
            var groups = AssetManager.trait_groups;
            if (groups == null)
                return false;

            ActorTraitGroupAsset group = groups.get(PreferenceTraitGroupId);
            if (group == null)
            {
                group = new ActorTraitGroupAsset
                {
                    id = PreferenceTraitGroupId,
                    name = PreferenceTraitGroupLocaleKey,
                    color = PreferenceTraitGroupColor
                };
                groups.add(group);
                return true;
            }

            group.name = PreferenceTraitGroupLocaleKey;
            if (string.IsNullOrWhiteSpace(group.color))
                group.color = PreferenceTraitGroupColor;
            return true;
        }

        private static void BuildPreferenceOpposites()
        {
            int count = _preferenceTraitAssets.Count;
            for (int i = 0; i < count; i++)
            {
                ActorTrait trait = _preferenceTraitAssets[i];
                if (trait == null)
                    continue;

                trait.opposite_list = new List<string>(count - 1);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    ActorTrait other = _preferenceTraitAssets[j];
                    if (other == null)
                        continue;
                    trait.opposite_list.Add(other.id);
                }
            }
        }

        private static void FinalizePreferenceOpposites(ActorTraitLibrary library)
        {
            int count = _preferenceTraitAssets.Count;
            for (int i = 0; i < count; i++)
            {
                ActorTrait trait = _preferenceTraitAssets[i];
                if (trait == null)
                    continue;

                trait.opposite_traits = new HashSet<ActorTrait>(count - 1);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    ActorTrait other = _preferenceTraitAssets[j];
                    if (other != null)
                        trait.opposite_traits.Add(other);
                }
            }

            try
            {
                MethodInfo filler = typeof(BaseTraitLibrary<ActorTrait>).GetMethod("fillOppositeHashsetsWithAssets", BindingFlags.Instance | BindingFlags.NonPublic);
                filler?.Invoke(library, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[XNTM] Failed to finalize preference trait opposites: {ex}");
            }
        }

        private static string BuildIconPath(string typeId)
        {
            if (IconOverrides.TryGetValue(typeId, out var pathOverride))
                return pathOverride;
            return $"ui/icons/kingdom_traits/xntm_{typeId}";
        }

        private static void BuildOpposites()
        {
            int count = _traitAssets.Count;
            for (int i = 0; i < count; i++)
            {
                KingdomTrait trait = _traitAssets[i];
                if (trait == null)
                    continue;

                trait.opposite_list = new List<string>(count - 1);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    KingdomTrait other = _traitAssets[j];
                    if (other == null)
                        continue;
                    trait.opposite_list.Add(other.id);
                }
            }
        }

        private static void FinalizeOpposites(KingdomTraitLibrary library)
        {
            int count = _traitAssets.Count;
            for (int i = 0; i < count; i++)
            {
                KingdomTrait trait = _traitAssets[i];
                if (trait == null)
                    continue;

                trait.opposite_traits = new HashSet<KingdomTrait>(count - 1);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    KingdomTrait other = _traitAssets[j];
                    if (other != null)
                        trait.opposite_traits.Add(other);
                }
            }

            try
            {
                MethodInfo filler = typeof(BaseTraitLibrary<KingdomTrait>).GetMethod("fillOppositeHashsetsWithAssets", BindingFlags.Instance | BindingFlags.NonPublic);
                filler?.Invoke(library, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[XNTM] Failed to finalize kingdom trait opposites: {ex}");
            }
        }
    }
}
