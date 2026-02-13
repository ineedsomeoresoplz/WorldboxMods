using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNTM.Code.Data;

namespace XNTM.Code.Utils
{
    
    
    
    public static class LandTypeManager
    {
        private const string CustomDataKey = "xntm_land_type";
        private const string BaseNameKey = "xntm_land_base_name";
        private const string NameMigrationKey = "xntm_land_name_migrated_v2";

        private static readonly List<LandTypeDefinition> _definitions = new List<LandTypeDefinition>
        {
            new LandTypeDefinition(
                "village",
                "land_type_village",
                "land_type_description_village",
                minPopulation: 0,
                housingMultiplier: 0.6f,
                warriorSlotCap: 0,
                buildSpeedMultiplier: 0.8f,
                loyaltyFlatModifier: 6),
            new LandTypeDefinition(
                "city",
                "land_type_city",
                "land_type_description_city",
                minPopulation: 45,
                housingMultiplier: 1.0f,
                warriorSlotCap: 3,
                buildSpeedMultiplier: 1.0f,
                loyaltyFlatModifier: 1),
            new LandTypeDefinition(
                "state",
                "land_type_state",
                "land_type_description_state",
                minPopulation: 120,
                housingMultiplier: 1.2f,
                warriorSlotCap: 6,
                buildSpeedMultiplier: 1.1f,
                loyaltyFlatModifier: -2),
            new LandTypeDefinition(
                "kingdom_land",
                "land_type_kingdom",
                "land_type_description_kingdom",
                minPopulation: 260,
                housingMultiplier: 1.35f,
                warriorSlotCap: 8,
                buildSpeedMultiplier: 1.05f,
                loyaltyFlatModifier: -6,
                requiresOverlordNation: true)
        };

        private static readonly Dictionary<string, LandTypeDefinition> _definitionMap = _definitions.ToDictionary(d => d.Id, d => d);
        private static readonly LandTypeDefinition _default = _definitionMap["village"];

        private static readonly HashSet<string> _overlordNationIds = new HashSet<string>
        {
            "empire",
            "federation",
            "confederation",
            "union",
            "commonwealth",
            "trade_league",
            "hanseatic_league"
        };

        public static LandTypeDefinition EnsureLandType(City city)
        {
            if (city == null)
                return _default;

            MigrateLegacyName(city);

            string stored = GetStoredType(city);
            if (!string.IsNullOrEmpty(stored) && _definitionMap.TryGetValue(stored, out var def))
                return def;

            LandTypeDefinition chosen = EvaluateLandType(city, NationTypeManager.GetDefinition(city.kingdom));
            SetStoredType(city, chosen.Id);
            return chosen;
        }

        public static void AssignBirthType(City city)
        {
            if (city == null)
                return;
            SetStoredType(city, _default.Id);
        }

        public static void Tick(City city, float elapsedSeconds)
        {
            if (city == null)
                return;

            NationTypeDefinition nation = NationTypeManager.GetDefinition(city.kingdom);
            LandTypeDefinition current = EnsureLandType(city);
            LandTypeDefinition desired = EvaluateLandType(city, nation);
            if (desired != current)
            {
                SetStoredType(city, desired.Id);
                current = desired;
            }

            ApplyBuildSpeed(city, current, nation, elapsedSeconds);
        }

        public static void ApplyStatusAdjustments(City city)
        {
            if (city?.status == null)
                return;

            NationTypeDefinition nation = NationTypeManager.GetDefinition(city.kingdom);
            LandTypeDefinition def = EnsureLandType(city);

            float housingMultiplier = def.HousingMultiplier + GetHousingBonusFromNation(def, nation);
            city.status.houses_max = Mathf.Max(0, Mathf.RoundToInt(city.status.houses_max * housingMultiplier));

            int warriorCap = def.WarriorSlotCap + GetWarriorSlotBonus(def, nation);
            if (warriorCap >= 0)
                city.status.warrior_slots = Math.Min(city.status.warrior_slots, warriorCap);
        }

        public static int ApplyLoyaltyModifier(City city, int loyalty)
        {
            LandTypeDefinition def = EnsureLandType(city);
            NationTypeDefinition nation = NationTypeManager.GetDefinition(city.kingdom);

            loyalty += def.LoyaltyFlatModifier;
            loyalty += GetNationLoyaltyAdjustment(def, nation);
            return loyalty;
        }

        public static bool CanFormArmy(City city)
        {
            LandTypeDefinition def = EnsureLandType(city);
            return def.WarriorSlotCap > 0;
        }

        public static string GetDisplayName(City city)
        {
            if (city?.data == null)
                return city?.name ?? string.Empty;

            string name = city.data.name;
            if (string.IsNullOrWhiteSpace(name))
                return city.name ?? string.Empty;

            string stripped = StripLandTypeSuffix(name);
            if (string.IsNullOrWhiteSpace(stripped))
                return name;

            return stripped;
        }

        private static LandTypeDefinition EvaluateLandType(City city, NationTypeDefinition nation)
        {
            int pop = GetPopulationEstimate(city);
            bool canHaveKingdom = nation != null && AllowsSubordinateKingdom(nation);

            LandTypeDefinition best = _default;
            foreach (LandTypeDefinition def in _definitions)
            {
                if (pop >= def.MinPopulation && (!def.RequiresOverlordNation || canHaveKingdom))
                    best = def;
            }

            return best;
        }

        private static int GetPopulationEstimate(City city)
        {
            if (city == null)
                return 0;
            int statusPop = city.status?.population ?? 0;
            if (statusPop > 0)
                return statusPop;
            return city.getPopulationPeople();
        }

        private static bool AllowsSubordinateKingdom(NationTypeDefinition nation)
        {
            if (nation == null)
                return false;
            return _overlordNationIds.Contains(nation.Id);
        }

        private static void ApplyBuildSpeed(City city, LandTypeDefinition def, NationTypeDefinition nation, float elapsedSeconds)
        {
            float multiplier = Mathf.Max(0f, def.BuildSpeedMultiplier + GetSuccessionBuildModifier(def, nation));
            if (Math.Abs(multiplier - 1f) < 0.001f)
                return;

            float delta = elapsedSeconds * (multiplier - 1f);
            if (delta > 0f)
            {
                city.timer_build = Mathf.Max(0f, city.timer_build - delta);
                city.timer_build_boat = Mathf.Max(0f, city.timer_build_boat - delta);
            }
            else
            {
                float reclaim = -delta;
                city.timer_build += reclaim;
                city.timer_build_boat += reclaim;
            }
        }

        private static float GetHousingBonusFromNation(LandTypeDefinition land, NationTypeDefinition nation)
        {
            if (nation == null)
                return 0f;

            switch (nation.Id)
            {
                case "empire":
                    return land.Id == "kingdom_land" || land.Id == "state" ? 0.1f : 0f;
                case "federation":
                case "confederation":
                case "union":
                case "commonwealth":
                    return land.Id == "state" ? 0.12f : 0.04f;
                case "trade_league":
                case "hanseatic_league":
                    return land.Id == "city" ? 0.1f : 0f;
                default:
                    return 0f;
            }
        }

        private static int GetWarriorSlotBonus(LandTypeDefinition land, NationTypeDefinition nation)
        {
            if (nation == null)
                return 0;

            switch (nation.SuccessionMode)
            {
                case NationSuccessionMode.RoyalLine:
                    return land.Id == "state" || land.Id == "kingdom_land" ? 1 : 0;
                case NationSuccessionMode.Elective:
                    return land.Id == "kingdom_land" ? -2 : 0;
                case NationSuccessionMode.Council:
                    if (land.Id == "city")
                        return 1;
                    if (land.Id == "kingdom_land")
                        return -1;
                    return 0;
                case NationSuccessionMode.Religious:
                    return land.Id == "village" ? 1 : 0;
                case NationSuccessionMode.None:
                    if (land.Id == "village" || land.Id == "city")
                        return -1;
                    if (land.Id == "state")
                        return -2;
                    if (land.Id == "kingdom_land")
                        return -3;
                    return 0;
                default:
                    return 0;
            }
        }

        private static int GetNationLoyaltyAdjustment(LandTypeDefinition land, NationTypeDefinition nation)
        {
            if (nation == null)
                return 0;

            switch (nation.SuccessionMode)
            {
                case NationSuccessionMode.RoyalLine:
                    if (land.Id == "kingdom_land")
                        return 4;
                    if (land.Id == "state")
                        return -1;
                    return 0;
                case NationSuccessionMode.Elective:
                    if (land.Id == "state")
                        return 2;
                    if (land.Id == "city")
                        return 1;
                    if (land.Id == "kingdom_land")
                        return -2;
                    return 0;
                case NationSuccessionMode.Council:
                    if (land.Id == "state")
                        return 3;
                    if (land.Id == "city")
                        return 2;
                    if (land.Id == "kingdom_land")
                        return -1;
                    return 0;
                case NationSuccessionMode.Religious:
                    if (land.Id == "village")
                        return 2;
                    if (land.Id == "city")
                        return 1;
                    return 0;
                case NationSuccessionMode.None:
                    if (land.Id == "village")
                        return -1;
                    if (land.Id == "city")
                        return -2;
                    if (land.Id == "state")
                        return -4;
                    if (land.Id == "kingdom_land")
                        return -6;
                    return 0;
                default:
                    return 0;
            }
        }

        private static string GetStoredType(City city)
        {
            if (city?.data?.custom_data_string != null && city.data.custom_data_string.TryGetValue(CustomDataKey, out var stored))
                return stored;
            return string.Empty;
        }

        private static void SetStoredType(City city, string id)
        {
            if (city?.data == null || string.IsNullOrEmpty(id))
                return;

            if (!_definitionMap.ContainsKey(id))
                return;

            if (city.data.custom_data_string == null)
                city.data.custom_data_string = new CustomDataContainer<string>();

            city.data.custom_data_string[CustomDataKey] = id;
        }

        private static float GetSuccessionBuildModifier(LandTypeDefinition land, NationTypeDefinition nation)
        {
            if (nation == null)
                return 0f;

            switch (nation.SuccessionMode)
            {
                case NationSuccessionMode.Council:
                    if (land.Id == "city")
                        return 0.05f;
                    if (land.Id == "state")
                        return 0.03f;
                    return 0f;
                case NationSuccessionMode.None:
                    if (land.Id == "kingdom_land")
                        return -0.12f;
                    if (land.Id == "state")
                        return -0.08f;
                    if (land.Id == "city")
                        return -0.04f;
                    return 0f;
                default:
                    return 0f;
            }
        }

        private static void MigrateLegacyName(City city)
        {
            if (city?.data == null)
                return;

            if (city.data.custom_data_string == null)
                city.data.custom_data_string = new CustomDataContainer<string>();

            if (city.data.custom_data_string.TryGetValue(NameMigrationKey, out var migrated) && migrated == "1")
                return;

            string legacyBase = string.Empty;
            if (city.data.custom_data_string.TryGetValue(BaseNameKey, out var stored) && !string.IsNullOrWhiteSpace(stored))
                legacyBase = stored.Trim();

            if (string.IsNullOrWhiteSpace(legacyBase))
                legacyBase = StripLandTypeSuffix(city.data.name);

            if (!string.IsNullOrWhiteSpace(legacyBase))
                city.data.name = legacyBase;

            city.data.custom_data_string.Remove(BaseNameKey);
            city.data.custom_data_string[NameMigrationKey] = "1";
        }

        private static string StripLandTypeSuffix(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            foreach (LandTypeDefinition def in _definitions)
            {
                string typeLabel = def.GetLocalizedName();
                if (string.IsNullOrEmpty(typeLabel))
                    continue;

                string suffix = $" ({typeLabel})";
                if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    return name.Substring(0, name.Length - suffix.Length);
            }

            return name;
        }
    }
}
