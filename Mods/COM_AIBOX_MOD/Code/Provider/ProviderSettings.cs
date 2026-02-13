using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIBox.Provider
{
    /// <summary>
    /// Saved provider settings - persisted to JSON file
    /// </summary>
    [Serializable]
    public class ProviderSettingsData
    {
        public string selectedProviderId = "custom";
        public string apiUrl = "https://openrouter.ai/api/v1/chat/completions";
        public string apiKey = "";
        public string modelId = "deepseek/deepseek-chat";
        public string customModelName = ""; // For manual model entry
        public List<string> cachedLocalModels = new List<string>(); // Models fetched from local provider
        public long lastModelFetch = 0; // Unix timestamp
    }

    /// <summary>
    /// Manages provider settings persistence
    /// </summary>
    public static class ProviderSettings
    {
        private static ProviderSettingsData _data;
        private static string _settingsPath;
        private static bool _isDirty = false;

        public static ProviderSettingsData Data
        {
            get
            {
                if (_data == null) Load();
                return _data;
            }
        }

        public static string SettingsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_settingsPath))
                {
                    _settingsPath = Path.Combine(Mod.Info.Path, "provider_settings.json");
                }
                return _settingsPath;
            }
        }

        /// <summary>
        /// Load settings from file, migrating from old config if needed
        /// </summary>
        public static void Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    _data = JsonUtility.FromJson<ProviderSettingsData>(json);
                    Debug.Log($"[AIBox] Loaded provider settings: {_data.selectedProviderId} / {_data.modelId}");
                }
                else
                {
                    // Try to migrate from old config
                    _data = MigrateFromOldConfig();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AIBox] Failed to load provider settings: {e.Message}");
                _data = new ProviderSettingsData();
            }

            if (_data == null)
            {
                _data = new ProviderSettingsData();
            }
        }

        /// <summary>
        /// Save settings to file
        /// </summary>
        public static void Save()
        {
            try
            {
                string json = JsonUtility.ToJson(_data, true);
                File.WriteAllText(SettingsPath, json);
                _isDirty = false;
                Debug.Log($"[AIBox] Saved provider settings");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIBox] Failed to save provider settings: {e.Message}");
            }
        }

        /// <summary>
        /// Mark settings as needing save (for deferred save)
        /// </summary>
        public static void MarkDirty()
        {
            Save(); // Force immediate save
        }

        /// <summary>
        /// Save if dirty
        /// </summary>
        public static void SaveIfDirty()
        {
            if (_isDirty) Save();
        }

        /// <summary>
        /// Migrate from old default_config.json format
        /// </summary>
        private static ProviderSettingsData MigrateFromOldConfig()
        {
            var data = new ProviderSettingsData();
            
            try
            {
                string oldConfigPath = Path.Combine(Mod.Info.Path, "default_config.json");
                if (!File.Exists(oldConfigPath)) return data;

                string json = File.ReadAllText(oldConfigPath);

                // Extract apiUrl
                var urlMatch = System.Text.RegularExpressions.Regex.Match(json, 
                    "\"Id\"\\s*:\\s*\"apiUrl\".*?\"TextVal\"\\s*:\\s*\"([^\"]+)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                if (urlMatch.Success)
                {
                    data.apiUrl = urlMatch.Groups[1].Value;
                    
                    // Detect provider from URL
                    var provider = ProviderConfig.GetProviderByUrl(data.apiUrl);
                    if (provider != null)
                    {
                        data.selectedProviderId = provider.Id;
                    }
                    else if (data.apiUrl.Contains("localhost") || data.apiUrl.Contains("127.0.0.1"))
                    {
                        data.selectedProviderId = "lm_studio";
                    }
                }

                // Extract modelName
                var modelMatch = System.Text.RegularExpressions.Regex.Match(json,
                    "\"Id\"\\s*:\\s*\"modelName\".*?\"TextVal\"\\s*:\\s*\"([^\"]+)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                if (modelMatch.Success)
                {
                    data.modelId = modelMatch.Groups[1].Value;
                }

                // Extract apiKey
                var keyMatch = System.Text.RegularExpressions.Regex.Match(json,
                    "\"Id\"\\s*:\\s*\"apiKey\".*?\"TextVal\"\\s*:\\s*\"([^\"]+)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                if (keyMatch.Success)
                {
                    data.apiKey = keyMatch.Groups[1].Value;
                }

                Debug.Log($"[AIBox] Migrated from old config: provider={data.selectedProviderId}, model={data.modelId}");
                
                // Save the new format
                _data = data;
                Save();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AIBox] Migration failed: {e.Message}");
            }

            return data;
        }

        /// <summary>
        /// Get the currently selected provider info
        /// </summary>
        public static ProviderInfo GetCurrentProvider()
        {
            return ProviderConfig.GetProviderById(Data.selectedProviderId) ?? ProviderConfig.Providers[0];
        }

        /// <summary>
        /// Get the currently selected model info (if known)
        /// </summary>
        public static ModelInfo GetCurrentModel()
        {
            var provider = GetCurrentProvider();
            return provider.Models.Find(m => m.Id == Data.modelId);
        }

        /// <summary>
        /// Update provider and reset model to first available
        /// </summary>
        public static void SetProvider(string providerId)
        {
            var provider = ProviderConfig.GetProviderById(providerId);
            if (provider == null) return;

            Data.selectedProviderId = providerId;
            Data.apiUrl = provider.DefaultUrl;
            
            // Set default model for this provider
            if (provider.Models.Count > 0)
            {
                Data.modelId = provider.Models[0].Id;
            }
            
            MarkDirty();
        }

        /// <summary>
        /// Set the model
        /// </summary>
        public static void SetModel(string modelId)
        {
            Data.modelId = modelId;
            MarkDirty();
        }

        /// <summary>
        /// Set the API key
        /// </summary>
        public static void SetApiKey(string key)
        {
            Data.apiKey = key;
            MarkDirty();
        }

        /// <summary>
        /// Set custom API URL (for custom provider)
        /// </summary>
        public static void SetCustomUrl(string url)
        {
            Data.apiUrl = url;
            Data.selectedProviderId = "custom";
            MarkDirty();
        }

        /// <summary>
        /// Update cached models from local provider
        /// </summary>
        public static void UpdateCachedModels(List<string> models)
        {
            Data.cachedLocalModels = models ?? new List<string>();
            Data.lastModelFetch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            MarkDirty();
        }

        /// <summary>
        /// Check if we need to refresh local models (cache older than 1 hour)
        /// </summary>
        public static bool NeedsModelRefresh()
        {
            var provider = GetCurrentProvider();
            if (!provider.IsLocal) return false;
            
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return (now - Data.lastModelFetch) > 3600; // 1 hour cache
        }
    }
}
