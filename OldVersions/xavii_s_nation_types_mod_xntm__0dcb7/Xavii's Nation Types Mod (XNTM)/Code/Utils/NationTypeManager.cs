using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityPools;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    public static class NationTypeManager
    {
        private const string CustomDataKey = "nationals_type_id"; 
        private const string TraitPrefix = "xntm_nation_type_";
        private const string TraitGroupId = "miscellaneous"; 
        private const string AutoCheckKey = "xntm_auto_nation_check";
        private static readonly HashSet<string> MaritimeIds = new HashSet<string> { "maritime_republic", "port_state", "trade_league", "hanseatic_league", "merchant_republic" };
        private static readonly HashSet<string> ClanRequiredIds = new HashSet<string> { "clan_state", "tribal_confederation" };
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
            new NationTypeDefinition("diarchy", "nation_type_diarchy", "nation_title_diarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_diarchy"),
            new NationTypeDefinition("triarchy", "nation_type_triarchy", "nation_title_triarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_triarchy"),
            new NationTypeDefinition("tetrarchy", "nation_type_tetrarchy", "nation_title_tetrarch", "nation_heir_council", NationSuccessionMode.Council, "nation_type_description_tetrarchy"),
            new NationTypeDefinition("republic", "nation_type_republic", "nation_title_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_republic"),
            new NationTypeDefinition("democratic_republic", "nation_type_democratic_republic", "nation_title_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_democratic_republic"),
            new NationTypeDefinition("federal_republic", "nation_type_federal_republic", "nation_title_federal_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_federal_republic"),
            new NationTypeDefinition("peoples_republic", "nation_type_peoples_republic", "nation_title_peoples_president", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_peoples_republic"),
            new NationTypeDefinition("constitutional_republic", "nation_type_constitutional_republic", "nation_title_constitutional_leader", "nation_heir_elect", NationSuccessionMode.Elective, "nation_type_description_constitutional_republic"),
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
        private static readonly List<KingdomTrait> _traitAssets = new List<KingdomTrait>(_definitions.Count);
        private static bool _traitsRegistered;

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
                .Where(def => (previous == null || def.Id != previous.Id) && _preferredRebelModes.Contains(def.SuccessionMode) && IsEligible(kingdom, def))
                .ToList();
            if (candidates.Count == 0)
                candidates = _definitions.Where(def => (previous == null || def.Id != previous.Id) && IsEligible(kingdom, def)).ToList();
            if (candidates.Count == 0)
                return previous ?? PickRandomType(kingdom);
            return candidates[Randy.randomInt(0, candidates.Count)];
        }

        private static NationTypeDefinition PickRandomType(Kingdom kingdom)
        {
            if (_definitions.Count == 0)
                return _default;
            List<NationTypeDefinition> eligible = GetEligibleDefinitions(kingdom);
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
                SetStoredType(kingdom, next.Id);
                NationNamingHelper.ApplyAccurateName(kingdom, kingdom.data?.name, true);
                return next;
            }
            return current;
        }

        public static Actor SelectHeir(Kingdom kingdom, Actor exclude)
        {
            EnsureTraitsRegistered();
            if (kingdom == null)
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
            return GetDefinition(kingdom).SuccessionMode == NationSuccessionMode.None;
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
            SetStoredType(kingdom, definition.Id);
            NationNamingHelper.ApplyAccurateName(kingdom, kingdom.data?.name, true);
            return true;
        }

        public static void TickAuto(Kingdom kingdom)
        {
            if (kingdom == null || kingdom.wild || !kingdom.isAlive())
                return;
            double now = World.world.getCurWorldTime();
            if (TryGetNextCheck(kingdom, out float next) && now < next)
                return;
            SetNextCheck(kingdom, (float)(now + 10f));
            NationTypeDefinition target = DetermineAutoTarget(kingdom);
            NationTypeDefinition current = GetDefinition(kingdom);
            if (target != null && target != current)
                TrySetType(kingdom, target, true);
        }

        private static NationTypeDefinition DetermineAutoTarget(Kingdom kingdom)
        {
            NationTypeDefinition current = GetDefinition(kingdom);
            if (!IsEligible(kingdom, current))
                return GetNaturalType(kingdom);
            if (current.Id == "empire" && !HasCulturalDiversity(kingdom))
                return GetRankedType(kingdom) ?? _default;
            return current;
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

        private static List<NationTypeDefinition> GetEligibleDefinitions(Kingdom kingdom)
        {
            List<NationTypeDefinition> result = new List<NationTypeDefinition>(_definitions.Count);
            foreach (NationTypeDefinition def in _definitions)
            {
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
        }

        private static void EnsureTraitsRegistered()
        {
            if (_traitsRegistered)
                return;

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
