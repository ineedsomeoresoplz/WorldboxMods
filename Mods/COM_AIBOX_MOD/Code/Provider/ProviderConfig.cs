using System;
using System.Collections.Generic;

namespace AIBox.Provider
{
    /// <summary>
    /// Information about an AI model
    /// </summary>
    [Serializable]
    public class ModelInfo
    {
        public string Id;           // API identifier: "gpt-4o", "claude-3-5-sonnet-20241022"
        public string DisplayName;  // User-friendly: "GPT-4o", "Claude 3.5 Sonnet"
        public int Quality;         // 1-5 stars (5 = best)
        public string CostTier;     // "Free", "$", "$$", "$$$", "$$$$"
        public string Description;  // Brief description
        public int ContextWindow;   // Max tokens (for reference)

        public ModelInfo() { }

        public ModelInfo(string id, string displayName, int quality, string costTier, string description, int contextWindow = 8192)
        {
            Id = id;
            DisplayName = displayName;
            Quality = quality;
            CostTier = costTier;
            Description = description;
            ContextWindow = contextWindow;
        }

        public string GetQualityStars()
        {
            string stars = "";
            for (int i = 0; i < 5; i++)
                stars += i < Quality ? "★" : "☆";
            return stars;
        }

        public override string ToString() => $"{DisplayName} ({GetQualityStars()} | {CostTier})";
    }

    /// <summary>
    /// Information about an AI provider
    /// </summary>
    [Serializable]
    public class ProviderInfo
    {
        public string Id;             // Internal identifier: "openrouter", "lm_studio"
        public string Name;           // Display name: "OpenRouter", "LM Studio"
        public string DefaultUrl;     // Default API endpoint
        public bool RequiresApiKey;   // Whether API key is required
        public bool IsLocal;          // Local provider (no internet needed)
        public bool SupportsJsonMode; // Supports response_format: json_object
        public string ApiFormat;      // "openai" or "anthropic"
        public List<ModelInfo> Models;

        public ProviderInfo()
        {
            Models = new List<ModelInfo>();
        }
    }

    /// <summary>
    /// Static configuration for all supported AI providers and their models
    /// </summary>
    public static class ProviderConfig
    {
        private static List<ProviderInfo> _providers;

        public static List<ProviderInfo> Providers
        {
            get
            {
                if (_providers == null)
                    InitializeProviders();
                return _providers;
            }
        }

        public static ProviderInfo GetProviderById(string id)
        {
            return Providers.Find(p => p.Id == id);
        }

        public static ProviderInfo GetProviderByUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            
            foreach (var provider in Providers)
            {
                if (url.Contains(provider.DefaultUrl.Replace("https://", "").Replace("http://", "").Split('/')[0]))
                    return provider;
            }
            
            // Check for local providers by port
            if (url.Contains(":1234")) return GetProviderById("lm_studio");
            if (url.Contains(":11434")) return GetProviderById("ollama");
            
            return null;
        }

        private static void InitializeProviders()
        {
            _providers = new List<ProviderInfo>();

            // ═══════════════════════════════════════════════════════════════
            // CLOUD PROVIDERS (Listed First)
            // ═══════════════════════════════════════════════════════════════

            // --- OpenRouter (Aggregator - Many Models) ---
            _providers.Add(new ProviderInfo
            {
                Id = "openrouter",
                Name = "OpenRouter",
                DefaultUrl = "https://openrouter.ai/api/v1/chat/completions",
                RequiresApiKey = true,
                IsLocal = false,
                SupportsJsonMode = true,
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    // Top Tier
                    new ModelInfo("anthropic/claude-sonnet-4", "Claude Sonnet 4", 5, "$$$", "Latest Anthropic flagship", 200000),
                    new ModelInfo("openai/gpt-4o", "GPT-4o", 5, "$$$", "OpenAI multimodal flagship", 128000),
                    new ModelInfo("google/gemini-2.0-flash-001", "Gemini 2.0 Flash", 5, "$$", "Google's latest fast model", 1000000),
                    new ModelInfo("deepseek/deepseek-r1", "DeepSeek R1", 5, "$", "Powerful reasoning model", 64000),
                    
                    // High Quality
                    new ModelInfo("anthropic/claude-3.5-sonnet", "Claude 3.5 Sonnet", 4, "$$", "Fast and capable", 200000),
                    new ModelInfo("openai/gpt-4o-mini", "GPT-4o Mini", 4, "$", "Fast and affordable", 128000),
                    new ModelInfo("meta-llama/llama-3.3-70b-instruct", "Llama 3.3 70B", 4, "$", "Meta's best open model", 131072),
                    new ModelInfo("qwen/qwen-2.5-72b-instruct", "Qwen 2.5 72B", 4, "$", "Alibaba's flagship", 32768),
                    
                    // Budget Options
                    new ModelInfo("deepseek/deepseek-chat", "DeepSeek V3", 4, "$", "Excellent value", 64000),
                    new ModelInfo("mistralai/mistral-small-24b-instruct-2501", "Mistral Small 24B", 3, "$", "Efficient mid-size", 32000),
                    new ModelInfo("meta-llama/llama-3.1-8b-instruct", "Llama 3.1 8B", 3, "$", "Fast small model", 131072),
                    new ModelInfo("google/gemma-2-9b-it", "Gemma 2 9B", 3, "$", "Google's small model", 8192),
                }
            });

            // --- OpenAI Direct ---
            _providers.Add(new ProviderInfo
            {
                Id = "openai",
                Name = "OpenAI",
                DefaultUrl = "https://api.openai.com/v1/chat/completions",
                RequiresApiKey = true,
                IsLocal = false,
                SupportsJsonMode = true,
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    new ModelInfo("gpt-4o", "GPT-4o", 5, "$$$", "Most capable model", 128000),
                    new ModelInfo("gpt-4o-mini", "GPT-4o Mini", 4, "$", "Fast and affordable", 128000),
                    new ModelInfo("gpt-4-turbo", "GPT-4 Turbo", 4, "$$", "Previous flagship", 128000),
                    new ModelInfo("gpt-3.5-turbo", "GPT-3.5 Turbo", 3, "$", "Legacy fast model", 16385),
                }
            });

            // --- Anthropic Direct ---
            _providers.Add(new ProviderInfo
            {
                Id = "anthropic",
                Name = "Anthropic",
                DefaultUrl = "https://api.anthropic.com/v1/messages",
                RequiresApiKey = true,
                IsLocal = false,
                SupportsJsonMode = false, // Uses different format
                ApiFormat = "anthropic",
                Models = new List<ModelInfo>
                {
                    new ModelInfo("claude-sonnet-4-20250514", "Claude Sonnet 4", 5, "$$$", "Latest flagship model", 200000),
                    new ModelInfo("claude-3-5-sonnet-20241022", "Claude 3.5 Sonnet", 5, "$$", "Excellent all-rounder", 200000),
                    new ModelInfo("claude-3-5-haiku-20241022", "Claude 3.5 Haiku", 4, "$", "Fast and efficient", 200000),
                    new ModelInfo("claude-3-opus-20240229", "Claude 3 Opus", 5, "$$$$", "Maximum capability", 200000),
                }
            });

            // --- Groq (Fast Inference) ---
            _providers.Add(new ProviderInfo
            {
                Id = "groq",
                Name = "Groq",
                DefaultUrl = "https://api.groq.com/openai/v1/chat/completions",
                RequiresApiKey = true,
                IsLocal = false,
                SupportsJsonMode = true,
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    new ModelInfo("llama-3.3-70b-versatile", "Llama 3.3 70B", 4, "$", "Fastest 70B inference", 128000),
                    new ModelInfo("llama-3.1-70b-versatile", "Llama 3.1 70B", 4, "$", "High quality open model", 131072),
                    new ModelInfo("llama-3.1-8b-instant", "Llama 3.1 8B", 3, "$", "Ultra-fast small model", 131072),
                    new ModelInfo("mixtral-8x7b-32768", "Mixtral 8x7B", 3, "$", "Efficient MoE model", 32768),
                    new ModelInfo("gemma2-9b-it", "Gemma 2 9B", 3, "$", "Google's efficient model", 8192),
                }
            });

            // --- Google AI Studio ---
            _providers.Add(new ProviderInfo
            {
                Id = "google",
                Name = "Google AI Studio",
                DefaultUrl = "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions",
                RequiresApiKey = true,
                IsLocal = false,
                SupportsJsonMode = true,
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    new ModelInfo("gemini-2.0-flash", "Gemini 2.0 Flash", 5, "$$", "Latest multimodal", 1000000),
                    new ModelInfo("gemini-1.5-pro", "Gemini 1.5 Pro", 4, "$$", "Long context pro", 2000000),
                    new ModelInfo("gemini-1.5-flash", "Gemini 1.5 Flash", 4, "$", "Fast and capable", 1000000),
                }
            });

            // ═══════════════════════════════════════════════════════════════
            // LOCAL PROVIDERS (Listed Second)
            // ═══════════════════════════════════════════════════════════════

            // --- LM Studio ---
            _providers.Add(new ProviderInfo
            {
                Id = "lm_studio",
                Name = "LM Studio (Local)",
                DefaultUrl = "http://localhost:1234/v1/chat/completions",
                RequiresApiKey = false,
                IsLocal = true,
                SupportsJsonMode = false, // Most local models don't support it
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    // These are common models - actual list fetched dynamically
                    new ModelInfo("auto", "Auto-detect", 3, "Free", "Uses currently loaded model", 8192),
                    new ModelInfo("deepseek-r1-distill-qwen-7b", "DeepSeek R1 Qwen 7B", 4, "Free", "Reasoning distilled", 32768),
                    new ModelInfo("qwen2.5-7b-instruct", "Qwen 2.5 7B", 3, "Free", "Balanced performance", 32768),
                    new ModelInfo("llama-3.2-3b-instruct", "Llama 3.2 3B", 2, "Free", "Very fast, lightweight", 131072),
                    new ModelInfo("phi-3-mini-4k-instruct", "Phi-3 Mini", 2, "Free", "Microsoft small model", 4096),
                }
            });

            // --- Ollama ---
            _providers.Add(new ProviderInfo
            {
                Id = "ollama",
                Name = "Ollama (Local)",
                DefaultUrl = "http://localhost:11434/v1/chat/completions",
                RequiresApiKey = false,
                IsLocal = true,
                SupportsJsonMode = false,
                ApiFormat = "openai",
                Models = new List<ModelInfo>
                {
                    new ModelInfo("deepseek-r1:8b", "DeepSeek R1 8B", 4, "Free", "Reasoning model", 32768),
                    new ModelInfo("deepseek-r1:14b", "DeepSeek R1 14B", 4, "Free", "Better reasoning", 32768),
                    new ModelInfo("qwen2.5:7b", "Qwen 2.5 7B", 3, "Free", "Balanced", 32768),
                    new ModelInfo("llama3.2:3b", "Llama 3.2 3B", 2, "Free", "Fast and light", 131072),
                    new ModelInfo("mistral:7b", "Mistral 7B", 3, "Free", "Efficient base", 32768),
                    new ModelInfo("gemma2:9b", "Gemma 2 9B", 3, "Free", "Google open model", 8192),
                }
            });

            // --- Custom Provider ---
            _providers.Add(new ProviderInfo
            {
                Id = "custom",
                Name = "Custom URL",
                DefaultUrl = "",
                RequiresApiKey = false,
                IsLocal = false,
                SupportsJsonMode = false,
                ApiFormat = "openai",
                Models = new List<ModelInfo>()
            });
        }

        /// <summary>
        /// Get all cloud providers (for dropdown ordering)
        /// </summary>
        public static List<ProviderInfo> GetCloudProviders()
        {
            return Providers.FindAll(p => !p.IsLocal && p.Id != "custom");
        }

        /// <summary>
        /// Get all local providers
        /// </summary>
        public static List<ProviderInfo> GetLocalProviders()
        {
            return Providers.FindAll(p => p.IsLocal);
        }

        /// <summary>
        /// Check if a URL belongs to a local provider
        /// </summary>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            return url.Contains("localhost") || url.Contains("127.0.0.1");
        }
    }
}
