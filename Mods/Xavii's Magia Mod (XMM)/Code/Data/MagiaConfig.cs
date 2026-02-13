using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace XaviiMagiaMod.Code.Data
{
    [Serializable]
    internal class MagiaConfigData
    {
        public int demonLordReincarnationDelayYears = 5;
        public int demonLordSealDurationYears = 80;
        public bool sealedTraitAssignable = true;
        public double GodTimeAgeScale = 1d;
        public float GodTimeBabyMultiplier = 1f;
        public float GodTimeTeenMultiplier = 1.15f;
        public float GodTimeYoungAdultMultiplier = 1.35f;
        public float GodTimeAdultMultiplier = 1.6f;
        public float GodTimeElderMultiplier = 2.1f;
        public AffinitySpawnRateNamed AffinitySpawnRate = new AffinitySpawnRateNamed();
        public List<AffinitySpawnRate> affinitySpawnRates = new List<AffinitySpawnRate>();
        public List<MageRankConfigEntry> MageRankDefinition = new List<MageRankConfigEntry>();
    }

    [Serializable]
    internal class AffinitySpawnRate
    {
        public string id;
        public float rate = 1f;
    }

    [Serializable]
    internal class AffinitySpawnRateNamed
    {
        public float pyro = 1f;
        public float aero = 1f;
        public float aqua = 1f;
        public float terra = 1f;
        public float haro = 1f;
        public float barku = 1f;
        public float none = 1f;
    }

    [Serializable]
    internal class MageRankConfigEntry
    {
        public string traitId;
        public int minLevel = 1;
        public int minKills = 0;
    }

    internal static class MagiaConfig
    {
        private const string ConfigFileName = "xmm_config.json";
        private static readonly MagiaConfigData DefaultConfig = CreateDefault();
        private static MagiaConfigData _current = Clone(DefaultConfig);
        private static List<MageRankDefinition> _mageRankDefinitions = BuildMageRankDefinitions(DefaultConfig.MageRankDefinition);

        public static int DemonLordReincarnationDelayYears =>
            Math.Max(0, _current?.demonLordReincarnationDelayYears ?? DefaultConfig.demonLordReincarnationDelayYears);

        public static int DemonLordSealDurationYears =>
            Math.Max(0, _current?.demonLordSealDurationYears ?? DefaultConfig.demonLordSealDurationYears);

        public static double GodTimeAgeScale => SanitizeGodTimeAgeScale(_current?.GodTimeAgeScale ?? DefaultConfig.GodTimeAgeScale);

        public static float GodTimeBabyMultiplier =>
            SanitizeMultiplier(_current?.GodTimeBabyMultiplier ?? DefaultConfig.GodTimeBabyMultiplier, DefaultConfig.GodTimeBabyMultiplier);

        public static float GodTimeTeenMultiplier =>
            SanitizeMultiplier(_current?.GodTimeTeenMultiplier ?? DefaultConfig.GodTimeTeenMultiplier, DefaultConfig.GodTimeTeenMultiplier);

        public static float GodTimeYoungAdultMultiplier =>
            SanitizeMultiplier(_current?.GodTimeYoungAdultMultiplier ?? DefaultConfig.GodTimeYoungAdultMultiplier, DefaultConfig.GodTimeYoungAdultMultiplier);

        public static float GodTimeAdultMultiplier =>
            SanitizeMultiplier(_current?.GodTimeAdultMultiplier ?? DefaultConfig.GodTimeAdultMultiplier, DefaultConfig.GodTimeAdultMultiplier);

        public static float GodTimeElderMultiplier =>
            SanitizeMultiplier(_current?.GodTimeElderMultiplier ?? DefaultConfig.GodTimeElderMultiplier, DefaultConfig.GodTimeElderMultiplier);

        public static IReadOnlyList<MageRankDefinition> MageRankDefinitions => _mageRankDefinitions;

        public static float GetAffinitySpawnRate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return 0f;

            if (TryGetNamedRate(_current, id, out float namedRate))
                return Mathf.Max(0f, namedRate);

            if (TryGetRate(_current, id, out float rate))
                return Mathf.Max(0f, rate);

            if (TryGetNamedRate(DefaultConfig, id, out float namedFallback))
                return Mathf.Max(0f, namedFallback);

            if (TryGetRate(DefaultConfig, id, out float fallback))
                return Mathf.Max(0f, fallback);

            return 0f;
        }

        public static void Load()
        {
            string path = GetConfigPath();
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (!File.Exists(path))
            {
                Save(path, DefaultConfig);
                _current = Clone(DefaultConfig);
                RebuildRuntimeCaches();
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                MagiaConfigData data = JsonUtility.FromJson<MagiaConfigData>(json);
                _current = data ?? Clone(DefaultConfig);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[XMM] Failed to read {ConfigFileName}: {ex.Message}");
                _current = Clone(DefaultConfig);
            }

            Normalize(_current);
            RebuildRuntimeCaches();
            Save(path, _current);
        }

        public static bool AllowSealedTraitAssignment =>
            _current?.sealedTraitAssignable ?? DefaultConfig.sealedTraitAssignable;

        private static void Normalize(MagiaConfigData data)
        {
            if (data == null)
                return;

            data.demonLordReincarnationDelayYears = Math.Max(0, data.demonLordReincarnationDelayYears);
            if (data.demonLordReincarnationDelayYears == 150)
                data.demonLordReincarnationDelayYears = DefaultConfig.demonLordReincarnationDelayYears;
            data.sealedTraitAssignable = true;
            data.GodTimeAgeScale = SanitizeGodTimeAgeScale(data.GodTimeAgeScale);
            data.GodTimeBabyMultiplier = SanitizeMultiplier(data.GodTimeBabyMultiplier, DefaultConfig.GodTimeBabyMultiplier);
            data.GodTimeTeenMultiplier = SanitizeMultiplier(data.GodTimeTeenMultiplier, DefaultConfig.GodTimeTeenMultiplier);
            data.GodTimeYoungAdultMultiplier = SanitizeMultiplier(data.GodTimeYoungAdultMultiplier, DefaultConfig.GodTimeYoungAdultMultiplier);
            data.GodTimeAdultMultiplier = SanitizeMultiplier(data.GodTimeAdultMultiplier, DefaultConfig.GodTimeAdultMultiplier);
            data.GodTimeElderMultiplier = SanitizeMultiplier(data.GodTimeElderMultiplier, DefaultConfig.GodTimeElderMultiplier);

            if (data.AffinitySpawnRate == null)
                data.AffinitySpawnRate = CloneNamedRates(DefaultConfig.AffinitySpawnRate);
            NormalizeNamedRates(data.AffinitySpawnRate);

            if (data.affinitySpawnRates == null)
                data.affinitySpawnRates = new List<AffinitySpawnRate>();

            data.affinitySpawnRates = data.affinitySpawnRates
                .Where(entry => entry != null && !string.IsNullOrWhiteSpace(entry.id))
                .Select(entry => new AffinitySpawnRate
                {
                    id = entry.id.Trim(),
                    rate = Mathf.Max(0f, entry.rate)
                })
                .GroupBy(entry => entry.id, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();

            foreach (var entry in DefaultConfig.affinitySpawnRates)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id))
                    continue;

                if (data.affinitySpawnRates.All(rate => !string.Equals(rate.id, entry.id, StringComparison.OrdinalIgnoreCase)))
                {
                    data.affinitySpawnRates.Add(new AffinitySpawnRate { id = entry.id, rate = entry.rate });
                }
            }

            SyncNamedRatesFromLegacyList(data);

            if (data.MageRankDefinition == null || data.MageRankDefinition.Count == 0)
            {
                data.MageRankDefinition = CloneMageRankDefinitions(DefaultConfig.MageRankDefinition);
            }
            else
            {
                data.MageRankDefinition = NormalizeMageRankDefinitions(data.MageRankDefinition);
                if (data.MageRankDefinition.Count == 0)
                    data.MageRankDefinition = CloneMageRankDefinitions(DefaultConfig.MageRankDefinition);
            }
        }

        public static void EnsureAffinitySpawnRates(IEnumerable<string> affinityIds)
        {
            if (affinityIds == null)
                return;

            if (_current == null)
                _current = Clone(DefaultConfig);

            string path = GetConfigPath();
            if (string.IsNullOrWhiteSpace(path))
                return;

            var normalizedIds = affinityIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (normalizedIds.Count == 0)
                return;

            if (_current.affinitySpawnRates == null)
                _current.affinitySpawnRates = new List<AffinitySpawnRate>();

            var knownIds = new HashSet<string>(
                _current.affinitySpawnRates
                    .Where(entry => entry != null && !string.IsNullOrWhiteSpace(entry.id))
                    .Select(entry => entry.id.Trim()),
                StringComparer.OrdinalIgnoreCase);

            bool changed = false;
            foreach (var id in normalizedIds)
            {
                if (knownIds.Contains(id))
                    continue;

                float rate = TryGetRate(DefaultConfig, id, out float defaultRate) ? defaultRate : 1f;
                _current.affinitySpawnRates.Add(new AffinitySpawnRate { id = id, rate = Mathf.Max(0f, rate) });
                knownIds.Add(id);
                changed = true;
            }

            if (changed)
                Save(path, _current);
        }

        private static void RebuildRuntimeCaches()
        {
            _mageRankDefinitions = BuildMageRankDefinitions(_current?.MageRankDefinition);
        }

        private static List<MageRankDefinition> BuildMageRankDefinitions(IEnumerable<MageRankConfigEntry> entries)
        {
            var normalized = NormalizeMageRankDefinitions(entries);
            if (normalized.Count == 0)
                normalized = CloneMageRankDefinitions(DefaultConfig.MageRankDefinition);

            if (normalized.Count == 0)
            {
                normalized.Add(new MageRankConfigEntry { traitId = "mage_apprentice", minLevel = 1, minKills = 0 });
                normalized.Add(new MageRankConfigEntry { traitId = "mage_formal", minLevel = 25, minKills = 15 });
                normalized.Add(new MageRankConfigEntry { traitId = "mage_archmage", minLevel = 45, minKills = 60 });
            }

            return normalized
                .Select(entry => new MageRankDefinition(entry.traitId, entry.minLevel, entry.minKills))
                .ToList();
        }

        private static List<MageRankConfigEntry> NormalizeMageRankDefinitions(IEnumerable<MageRankConfigEntry> entries)
        {
            if (entries == null)
                return new List<MageRankConfigEntry>();

            return entries
                .Where(entry => entry != null && !string.IsNullOrWhiteSpace(entry.traitId))
                .Select(entry => new MageRankConfigEntry
                {
                    traitId = entry.traitId.Trim(),
                    minLevel = Math.Max(1, entry.minLevel),
                    minKills = Math.Max(0, entry.minKills)
                })
                .GroupBy(entry => entry.traitId, StringComparer.Ordinal)
                .Select(group => group.First())
                .OrderBy(entry => entry.minLevel)
                .ThenBy(entry => entry.minKills)
                .ToList();
        }

        private static List<MageRankConfigEntry> CloneMageRankDefinitions(IEnumerable<MageRankConfigEntry> entries)
        {
            return entries?
                .Where(entry => entry != null && !string.IsNullOrWhiteSpace(entry.traitId))
                .Select(entry => new MageRankConfigEntry
                {
                    traitId = entry.traitId,
                    minLevel = entry.minLevel,
                    minKills = entry.minKills
                })
                .ToList() ?? new List<MageRankConfigEntry>();
        }

        private static bool TryGetNamedRate(MagiaConfigData data, string id, out float rate)
        {
            rate = 0f;
            if (data?.AffinitySpawnRate == null || string.IsNullOrWhiteSpace(id))
                return false;

            switch (id.Trim().ToLowerInvariant())
            {
                case "pyro":
                    rate = data.AffinitySpawnRate.pyro;
                    return true;
                case "aero":
                    rate = data.AffinitySpawnRate.aero;
                    return true;
                case "aqua":
                    rate = data.AffinitySpawnRate.aqua;
                    return true;
                case "terra":
                    rate = data.AffinitySpawnRate.terra;
                    return true;
                case "haro":
                    rate = data.AffinitySpawnRate.haro;
                    return true;
                case "barku":
                    rate = data.AffinitySpawnRate.barku;
                    return true;
                case "none":
                    rate = data.AffinitySpawnRate.none;
                    return true;
                default:
                    return false;
            }
        }

        private static void SetNamedRate(AffinitySpawnRateNamed rates, string id, float value)
        {
            if (rates == null || string.IsNullOrWhiteSpace(id))
                return;

            value = Mathf.Max(0f, value);
            switch (id.Trim().ToLowerInvariant())
            {
                case "pyro":
                    rates.pyro = value;
                    break;
                case "aero":
                    rates.aero = value;
                    break;
                case "aqua":
                    rates.aqua = value;
                    break;
                case "terra":
                    rates.terra = value;
                    break;
                case "haro":
                    rates.haro = value;
                    break;
                case "barku":
                    rates.barku = value;
                    break;
                case "none":
                    rates.none = value;
                    break;
            }
        }

        private static void SyncNamedRatesFromLegacyList(MagiaConfigData data)
        {
            if (data?.AffinitySpawnRate == null || data.affinitySpawnRates == null)
                return;

            foreach (var entry in data.affinitySpawnRates)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id))
                    continue;

                if (!TryGetNamedRate(data, entry.id, out float currentNamed))
                    continue;

                if (!TryGetNamedRate(DefaultConfig, entry.id, out float defaultNamed))
                    continue;

                float legacyRate = Mathf.Max(0f, entry.rate);
                if (Mathf.Approximately(currentNamed, defaultNamed) && !Mathf.Approximately(legacyRate, defaultNamed))
                    SetNamedRate(data.AffinitySpawnRate, entry.id, legacyRate);
            }
        }

        private static void NormalizeNamedRates(AffinitySpawnRateNamed rates)
        {
            if (rates == null)
                return;

            rates.pyro = Mathf.Max(0f, rates.pyro);
            rates.aero = Mathf.Max(0f, rates.aero);
            rates.aqua = Mathf.Max(0f, rates.aqua);
            rates.terra = Mathf.Max(0f, rates.terra);
            rates.haro = Mathf.Max(0f, rates.haro);
            rates.barku = Mathf.Max(0f, rates.barku);
            rates.none = Mathf.Max(0f, rates.none);
        }

        private static AffinitySpawnRateNamed CloneNamedRates(AffinitySpawnRateNamed source)
        {
            if (source == null)
                return new AffinitySpawnRateNamed();

            return new AffinitySpawnRateNamed
            {
                pyro = source.pyro,
                aero = source.aero,
                aqua = source.aqua,
                terra = source.terra,
                haro = source.haro,
                barku = source.barku,
                none = source.none
            };
        }

        private static bool TryGetRate(MagiaConfigData data, string id, out float rate)
        {
            var entry = data?.affinitySpawnRates?.FirstOrDefault(rate =>
                string.Equals(rate.id, id, StringComparison.OrdinalIgnoreCase));
            if (entry == null)
            {
                rate = 0f;
                return false;
            }

            rate = entry.rate;
            return true;
        }

        private static void Save(string path, MagiaConfigData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[XMM] Failed to write {ConfigFileName}: {ex.Message}");
            }
        }

        private static string GetConfigPath()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string directory = string.IsNullOrWhiteSpace(location)
                ? Directory.GetCurrentDirectory()
                : Path.GetDirectoryName(location);
            if (string.IsNullOrWhiteSpace(directory))
                return null;
            return Path.Combine(directory, ConfigFileName);
        }

        private static double SanitizeGodTimeAgeScale(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                return DefaultConfig.GodTimeAgeScale;
            return Math.Max(1d, value);
        }

        private static float SanitizeMultiplier(float value, float fallback)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0f)
                return Mathf.Max(0f, fallback);
            return Mathf.Max(0f, value);
        }

        private static MagiaConfigData CreateDefault()
        {
            var data = new MagiaConfigData
            {
                demonLordReincarnationDelayYears = 5,
                demonLordSealDurationYears = 80,
                sealedTraitAssignable = true,
                GodTimeAgeScale = 1d,
                GodTimeBabyMultiplier = 1f,
                GodTimeTeenMultiplier = 1.15f,
                GodTimeYoungAdultMultiplier = 1.35f,
                GodTimeAdultMultiplier = 1.6f,
                GodTimeElderMultiplier = 2.1f,
                AffinitySpawnRate = new AffinitySpawnRateNamed
                {
                    pyro = 1f,
                    aero = 1f,
                    aqua = 1f,
                    terra = 1f,
                    haro = 1f,
                    barku = 1f,
                    none = 1f
                },
                affinitySpawnRates = new List<AffinitySpawnRate>
                {
                    new AffinitySpawnRate { id = "pyro", rate = 1f },
                    new AffinitySpawnRate { id = "aero", rate = 1f },
                    new AffinitySpawnRate { id = "aqua", rate = 1f },
                    new AffinitySpawnRate { id = "terra", rate = 1f },
                    new AffinitySpawnRate { id = "haro", rate = 1f },
                    new AffinitySpawnRate { id = "barku", rate = 1f },
                    new AffinitySpawnRate { id = "none", rate = 1f }
                },
                MageRankDefinition = new List<MageRankConfigEntry>
                {
                    new MageRankConfigEntry { traitId = "mage_apprentice", minLevel = 1, minKills = 0 },
                    new MageRankConfigEntry { traitId = "mage_formal", minLevel = 25, minKills = 15 },
                    new MageRankConfigEntry { traitId = "mage_archmage", minLevel = 45, minKills = 60 }
                }
            };
            return data;
        }

        private static MagiaConfigData Clone(MagiaConfigData data)
        {
            if (data == null)
                return new MagiaConfigData();

            return new MagiaConfigData
            {
                demonLordReincarnationDelayYears = data.demonLordReincarnationDelayYears,
                demonLordSealDurationYears = data.demonLordSealDurationYears,
                sealedTraitAssignable = data.sealedTraitAssignable,
                GodTimeAgeScale = data.GodTimeAgeScale,
                GodTimeBabyMultiplier = data.GodTimeBabyMultiplier,
                GodTimeTeenMultiplier = data.GodTimeTeenMultiplier,
                GodTimeYoungAdultMultiplier = data.GodTimeYoungAdultMultiplier,
                GodTimeAdultMultiplier = data.GodTimeAdultMultiplier,
                GodTimeElderMultiplier = data.GodTimeElderMultiplier,
                AffinitySpawnRate = CloneNamedRates(data.AffinitySpawnRate),
                affinitySpawnRates = data.affinitySpawnRates?.Select(rate => new AffinitySpawnRate
                {
                    id = rate.id,
                    rate = rate.rate
                }).ToList() ?? new List<AffinitySpawnRate>(),
                MageRankDefinition = CloneMageRankDefinitions(data.MageRankDefinition)
            };
        }
    }
}
