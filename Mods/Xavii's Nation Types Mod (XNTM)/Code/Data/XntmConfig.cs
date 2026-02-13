using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace XNTM.Code.Data
{
    [Serializable]
    internal class XntmConfigData
    {
        public bool showDescriptions = true;
        public bool logActorDiscoveriesForAllActors;
        public bool preferenceTraitsNoTouch = true;
        public bool filterRepetitiveNationTypes = true;
        public bool republicElections = true;
        public float republicElectionMinYears = 3f;
        public float republicElectionMaxYears = 8f;
        public float republicTermLimitYears = 10f;
        public bool republicPreferClanSwitch = true;
    }

    internal static class XntmConfig
    {
        private const string ConfigFileName = "xntm_config.json";
        private static readonly XntmConfigData Default = CreateDefault();
        private static XntmConfigData _current = Clone(Default);

        public static bool ShowDescriptions => _current.showDescriptions;
        public static bool LogActorDiscoveriesForAllActors => _current.logActorDiscoveriesForAllActors;
        public static bool PreferenceTraitsNoTouch => _current.preferenceTraitsNoTouch;
        public static bool FilterRepetitiveNationTypes => _current.filterRepetitiveNationTypes;
        public static bool RepublicElections => _current.republicElections;
        public static bool RepublicPreferClanSwitch => _current.republicPreferClanSwitch;
        public static float RepublicElectionMinYears => _current.republicElectionMinYears;
        public static float RepublicElectionMaxYears => _current.republicElectionMaxYears;
        public static float RepublicTermLimitYears => _current.republicTermLimitYears;

        public static void Load()
        {
            string path = GetConfigPath();
            if (string.IsNullOrWhiteSpace(path))
            {
                _current = Clone(Default);
                return;
            }

            if (!File.Exists(path))
            {
                _current = Clone(Default);
                Save(path, _current);
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                XntmConfigData loaded = JsonUtility.FromJson<XntmConfigData>(json);
                _current = loaded ?? Clone(Default);
            }
            catch
            {
                _current = Clone(Default);
            }

            Normalize(_current);
            Save(path, _current);
        }

        private static void Normalize(XntmConfigData data)
        {
            if (data == null)
                return;

            data.republicElectionMinYears = SanitizeYears(data.republicElectionMinYears, 1f, 40f, Default.republicElectionMinYears);
            data.republicElectionMaxYears = SanitizeYears(data.republicElectionMaxYears, data.republicElectionMinYears, 60f, Default.republicElectionMaxYears);
            if (float.IsNaN(data.republicTermLimitYears) || float.IsInfinity(data.republicTermLimitYears))
                data.republicTermLimitYears = Default.republicTermLimitYears;
            if (data.republicTermLimitYears <= 0f)
                data.republicTermLimitYears = 0f;
            else
                data.republicTermLimitYears = Mathf.Clamp(data.republicTermLimitYears, data.republicElectionMinYears, 120f);

            if (data.republicTermLimitYears > 0f && data.republicElectionMaxYears > data.republicTermLimitYears)
                data.republicElectionMaxYears = data.republicTermLimitYears;
            if (data.republicElectionMaxYears < data.republicElectionMinYears)
                data.republicElectionMaxYears = data.republicElectionMinYears;
        }

        private static float SanitizeYears(float value, float min, float max, float fallback)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                value = fallback;
            if (float.IsNaN(value) || float.IsInfinity(value))
                value = min;
            if (max < min)
                max = min;
            return Mathf.Clamp(value, min, max);
        }

        private static void Save(string path, XntmConfigData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
            }
            catch
            {
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

        private static XntmConfigData Clone(XntmConfigData source)
        {
            if (source == null)
                return CreateDefault();

            return new XntmConfigData
            {
                showDescriptions = source.showDescriptions,
                logActorDiscoveriesForAllActors = source.logActorDiscoveriesForAllActors,
                preferenceTraitsNoTouch = source.preferenceTraitsNoTouch,
                filterRepetitiveNationTypes = source.filterRepetitiveNationTypes,
                republicElections = source.republicElections,
                republicElectionMinYears = source.republicElectionMinYears,
                republicElectionMaxYears = source.republicElectionMaxYears,
                republicTermLimitYears = source.republicTermLimitYears,
                republicPreferClanSwitch = source.republicPreferClanSwitch
            };
        }

        private static XntmConfigData CreateDefault()
        {
            return new XntmConfigData
            {
                showDescriptions = true,
                logActorDiscoveriesForAllActors = false,
                preferenceTraitsNoTouch = true,
                filterRepetitiveNationTypes = true,
                republicElections = true,
                republicElectionMinYears = 3f,
                republicElectionMaxYears = 8f,
                republicTermLimitYears = 10f,
                republicPreferClanSwitch = true
            };
        }
    }
}
