using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using XaviiWindowsMod.API;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmAutoloadEntry
    {
        public string modTarget;
        public string fileName;
        public string runtimeId;
        public bool show = true;
    }

    internal sealed class XwmAutoloadProfile
    {
        public string version = "1.0.0";
        public bool enabled = true;
        public List<XwmAutoloadEntry> entries = new List<XwmAutoloadEntry>();
    }

    internal static class XwmAutoloadProfileStore
    {
        private static XwmAutoloadProfile _profile;
        private static bool _loaded;

        public static string FilePath
        {
            get
            {
                string folder = Path.Combine(XwmPathResolver.ThisModFolder, "XWM");
                Directory.CreateDirectory(folder);
                return Path.Combine(folder, "autoload_profile.json");
            }
        }

        public static void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            Load();
        }

        public static void Load()
        {
            _profile = new XwmAutoloadProfile();
            _loaded = true;

            string path = FilePath;
            if (!File.Exists(path))
            {
                Save();
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                XwmAutoloadProfile parsed = JsonConvert.DeserializeObject<XwmAutoloadProfile>(json);
                if (parsed != null)
                {
                    _profile = parsed;
                }
            }
            catch
            {
                _profile = new XwmAutoloadProfile();
            }

            if (_profile.entries == null)
            {
                _profile.entries = new List<XwmAutoloadEntry>();
            }

            Cleanup();
        }

        public static void Save()
        {
            EnsureLoaded();
            Cleanup();

            try
            {
                string json = JsonConvert.SerializeObject(_profile, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch
            {
            }
        }

        public static bool Enabled
        {
            get
            {
                EnsureLoaded();
                return _profile.enabled;
            }
            set
            {
                EnsureLoaded();
                _profile.enabled = value;
                Save();
            }
        }

        public static IReadOnlyList<XwmAutoloadEntry> GetEntries()
        {
            EnsureLoaded();
            Cleanup();
            return _profile.entries.AsReadOnly();
        }

        public static bool Contains(string modTarget, string fileName)
        {
            EnsureLoaded();
            return FindEntry(modTarget, fileName) != null;
        }

        public static bool Toggle(string modTarget, string fileName)
        {
            EnsureLoaded();
            XwmAutoloadEntry existing = FindEntry(modTarget, fileName);
            if (existing != null)
            {
                _profile.entries.Remove(existing);
                Save();
                return false;
            }

            SetEnabled(modTarget, fileName, true, true, null);
            return true;
        }

        public static void SetEnabled(string modTarget, string fileName, bool enabled, bool show, string runtimeId)
        {
            EnsureLoaded();
            XwmAutoloadEntry existing = FindEntry(modTarget, fileName);
            if (!enabled)
            {
                if (existing != null)
                {
                    _profile.entries.Remove(existing);
                    Save();
                }

                return;
            }

            if (existing == null)
            {
                existing = new XwmAutoloadEntry();
                _profile.entries.Add(existing);
            }

            existing.modTarget = string.IsNullOrWhiteSpace(modTarget) ? string.Empty : modTarget.Trim();
            existing.fileName = string.IsNullOrWhiteSpace(fileName) ? string.Empty : fileName.Trim();
            existing.runtimeId = string.IsNullOrWhiteSpace(runtimeId) ? string.Empty : runtimeId.Trim();
            existing.show = show;
            Save();
        }

        public static int ApplyAll()
        {
            EnsureLoaded();
            if (!_profile.enabled)
            {
                return 0;
            }

            int loaded = 0;
            for (int i = 0; i < _profile.entries.Count; i++)
            {
                XwmAutoloadEntry entry = _profile.entries[i];
                if (entry == null || string.IsNullOrWhiteSpace(entry.modTarget) || string.IsNullOrWhiteSpace(entry.fileName))
                {
                    continue;
                }

                string runtimeId = string.IsNullOrWhiteSpace(entry.runtimeId) ? null : entry.runtimeId;
                XwmWindowHandle handle = XwmFiles.GetOrLoad(entry.modTarget, entry.fileName, runtimeId, entry.show);
                if (handle != null)
                {
                    loaded++;
                }
            }

            return loaded;
        }

        private static XwmAutoloadEntry FindEntry(string modTarget, string fileName)
        {
            if (string.IsNullOrWhiteSpace(modTarget) || string.IsNullOrWhiteSpace(fileName) || _profile == null || _profile.entries == null)
            {
                return null;
            }

            for (int i = 0; i < _profile.entries.Count; i++)
            {
                XwmAutoloadEntry entry = _profile.entries[i];
                if (entry == null)
                {
                    continue;
                }

                if (string.Equals(entry.modTarget, modTarget.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(entry.fileName, fileName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return entry;
                }
            }

            return null;
        }

        private static void Cleanup()
        {
            if (_profile == null)
            {
                _profile = new XwmAutoloadProfile();
            }

            if (_profile.entries == null)
            {
                _profile.entries = new List<XwmAutoloadEntry>();
            }

            for (int i = _profile.entries.Count - 1; i >= 0; i--)
            {
                XwmAutoloadEntry entry = _profile.entries[i];
                if (entry == null || string.IsNullOrWhiteSpace(entry.modTarget) || string.IsNullOrWhiteSpace(entry.fileName))
                {
                    _profile.entries.RemoveAt(i);
                    continue;
                }

                entry.modTarget = entry.modTarget.Trim();
                entry.fileName = entry.fileName.Trim();
                entry.runtimeId = string.IsNullOrWhiteSpace(entry.runtimeId) ? string.Empty : entry.runtimeId.Trim();
            }
        }
    }
}
