using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using AIBox.Provider;

namespace AIBox
{
    public static class LLMClient
    {
        // ═══════════════════════════════════════════════════════════════
        // Provider Detection
        // ═══════════════════════════════════════════════════════════════

        public static bool IsOpenRouterUrl(string url) => 
            !string.IsNullOrEmpty(url) && url.Contains("openrouter.ai");

        public static bool IsAnthropicUrl(string url) => 
            !string.IsNullOrEmpty(url) && url.Contains("api.anthropic.com");

        public static bool IsLMStudioUrl(string url) => 
            !string.IsNullOrEmpty(url) && (url.Contains("localhost:1234") || url.Contains("127.0.0.1:1234"));

        public static bool IsOllamaUrl(string url) => 
            !string.IsNullOrEmpty(url) && (url.Contains("localhost:11434") || url.Contains("127.0.0.1:11434"));

        public static bool IsLocalProvider(string url) => 
            IsLMStudioUrl(url) || IsOllamaUrl(url) || 
            (!string.IsNullOrEmpty(url) && (url.Contains("localhost") || url.Contains("127.0.0.1")));

        public static bool IsGroqUrl(string url) =>
            !string.IsNullOrEmpty(url) && url.Contains("api.groq.com");

        public static bool IsGoogleUrl(string url) =>
            !string.IsNullOrEmpty(url) && url.Contains("generativelanguage.googleapis.com");

        // ═══════════════════════════════════════════════════════════════
        // Main API Call
        // ═══════════════════════════════════════════════════════════════

        public static IEnumerator PostJson(string url, string apiKey, string jsonPayload, Action<string> onSuccess, Action<string> onFailure)
        {
            // Handle Anthropic's different API format
            if (IsAnthropicUrl(url))
            {
                yield return PostJsonAnthropic(url, apiKey, jsonPayload, onSuccess, onFailure);
                yield break;
            }

            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.uploadHandler.contentType = "application/json";
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                // Debug: Log what we're sending
                Debug.Log($"[LLMClient] Sending to: {url}");
                
                // Authorization Header (skip for local providers or if empty)
                if (!string.IsNullOrEmpty(apiKey) && !IsLocalProvider(url)) 
                {
                    www.SetRequestHeader("Authorization", "Bearer " + apiKey);
                }

                // Provider-specific headers
                if (IsOpenRouterUrl(url)) 
                {
                    www.SetRequestHeader("HTTP-Referer", "https://worldbox.mod"); 
                    www.SetRequestHeader("X-Title", "AIBox Mod");
                }

                www.timeout = 120; // 2 minute timeout for slow models

                yield return www.SendWebRequest(); 

                if (www.result != UnityWebRequest.Result.Success)
                {
                    string errorMsg = FormatError(www);
                    Debug.LogWarning($"[LLMClient] Error: {errorMsg}");
                    onFailure?.Invoke(errorMsg);
                }
                else
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Anthropic API (Different Format)
        // ═══════════════════════════════════════════════════════════════

        private static IEnumerator PostJsonAnthropic(string url, string apiKey, string openAiPayload, Action<string> onSuccess, Action<string> onFailure)
        {
            // Convert OpenAI format to Anthropic format
            string anthropicPayload = ConvertToAnthropicFormat(openAiPayload);
            
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(anthropicPayload);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("x-api-key", apiKey);
                www.SetRequestHeader("anthropic-version", "2023-06-01");

                Debug.Log($"[LLMClient] Sending to Anthropic: {url}");

                www.timeout = 120;

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    string errorMsg = FormatError(www);
                    Debug.LogWarning($"[LLMClient] Anthropic Error: {errorMsg}");
                    onFailure?.Invoke(errorMsg);
                }
                else
                {
                    // Convert Anthropic response back to OpenAI format for consistent parsing
                    string response = ConvertFromAnthropicResponse(www.downloadHandler.text);
                    onSuccess?.Invoke(response);
                }
            }
        }

        /// <summary>
        /// Convert OpenAI chat format to Anthropic messages format
        /// </summary>
        private static string ConvertToAnthropicFormat(string openAiPayload)
        {
            try
            {
                // Extract model
                var modelMatch = System.Text.RegularExpressions.Regex.Match(openAiPayload, "\"model\"\\s*:\\s*\"([^\"]+)\"");
                string model = modelMatch.Success ? modelMatch.Groups[1].Value : "claude-3-5-sonnet-20241022";

                // Extract max_tokens
                var tokensMatch = System.Text.RegularExpressions.Regex.Match(openAiPayload, "\"max_tokens\"\\s*:\\s*(\\d+)");
                int maxTokens = tokensMatch.Success ? int.Parse(tokensMatch.Groups[1].Value) : 2048;

                // Extract system message
                string systemContent = "";
                var systemMatch = System.Text.RegularExpressions.Regex.Match(openAiPayload, 
                    "\"role\"\\s*:\\s*\"system\".*?\"content\"\\s*:\\s*\"((?:\\\\.|[^\"\\\\])*)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                if (systemMatch.Success)
                {
                    systemContent = systemMatch.Groups[1].Value;
                }

                // Extract user message
                string userContent = "";
                var userMatch = System.Text.RegularExpressions.Regex.Match(openAiPayload,
                    "\"role\"\\s*:\\s*\"user\".*?\"content\"\\s*:\\s*\"((?:\\\\.|[^\"\\\\])*)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                if (userMatch.Success)
                {
                    userContent = userMatch.Groups[1].Value;
                }

                // Build Anthropic format
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append($"\"model\": \"{model}\",");
                sb.Append($"\"max_tokens\": {maxTokens},");
                
                if (!string.IsNullOrEmpty(systemContent))
                {
                    sb.Append($"\"system\": \"{systemContent}\",");
                }
                
                sb.Append("\"messages\": [");
                sb.Append($"{{\"role\": \"user\", \"content\": \"{userContent}\"}}");
                sb.Append("]");
                sb.Append("}");

                return sb.ToString();
            }
            catch (Exception e)
            {
                Debug.LogError($"[LLMClient] Failed to convert to Anthropic format: {e.Message}");
                return openAiPayload; // Return original as fallback
            }
        }

        /// <summary>
        /// Convert Anthropic response to OpenAI format for consistent parsing
        /// </summary>
        private static string ConvertFromAnthropicResponse(string anthropicResponse)
        {
            try
            {
                // Anthropic format: {"content": [{"type": "text", "text": "..."}], ...}
                // OpenAI format: {"choices": [{"message": {"content": "..."}}]}
                
                var textMatch = System.Text.RegularExpressions.Regex.Match(anthropicResponse,
                    "\"text\"\\s*:\\s*\"((?:\\\\.|[^\"\\\\])*)\"",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                
                if (textMatch.Success)
                {
                    string content = textMatch.Groups[1].Value;
                    return $"{{\"choices\": [{{\"message\": {{\"role\": \"assistant\", \"content\": \"{content}\"}}}}]}}";
                }

                return anthropicResponse; // Return as-is if parsing fails
            }
            catch (Exception e)
            {
                Debug.LogError($"[LLMClient] Failed to convert from Anthropic format: {e.Message}");
                return anthropicResponse;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Model Discovery (For Local Providers)
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Fetch available models from a local provider's /v1/models endpoint
        /// </summary>
        public static IEnumerator FetchModels(string baseUrl, Action<List<string>> callback)
        {
            // Build models endpoint URL
            string modelsUrl = baseUrl;
            if (modelsUrl.Contains("/chat/completions"))
            {
                modelsUrl = modelsUrl.Replace("/chat/completions", "/models");
            }
            else if (!modelsUrl.EndsWith("/models"))
            {
                if (!modelsUrl.EndsWith("/")) modelsUrl += "/";
                modelsUrl += "models";
            }

            Debug.Log($"[LLMClient] Fetching models from: {modelsUrl}");

            using (UnityWebRequest www = UnityWebRequest.Get(modelsUrl))
            {
                www.timeout = 10; // Short timeout for model list

                yield return www.SendWebRequest();

                List<string> models = new List<string>();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        string response = www.downloadHandler.text;
                        
                        // Parse model IDs from response
                        // Format: {"data": [{"id": "model-name", ...}, ...]}
                        var matches = System.Text.RegularExpressions.Regex.Matches(response, "\"id\"\\s*:\\s*\"([^\"]+)\"");
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            string modelId = match.Groups[1].Value;
                            if (!string.IsNullOrEmpty(modelId) && !modelId.StartsWith("text-embedding"))
                            {
                                models.Add(modelId);
                            }
                        }

                        Debug.Log($"[LLMClient] Found {models.Count} models");
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[LLMClient] Failed to parse models response: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[LLMClient] Failed to fetch models: {www.error}");
                }

                callback?.Invoke(models);
            }
        }

        public static IEnumerator TestConnection(string url, string apiKey, Action<bool, string> callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Invoke(false, "API URL is empty");
                yield break;
            }

            // Always use the user-selected model for a real test
            string modelId = ProviderSettings.Data.modelId;
            if (string.IsNullOrEmpty(modelId)) modelId = "gpt-3.5-turbo";

            // Minimal test payload to check both connection and model availability
            string testPayload = $"{{\"model\": \"{modelId}\", \"messages\": [{{ \"role\": \"user\", \"content\": \"ping\" }}], \"max_tokens\": 1}}";
            
            bool testSuccess = false;
            string testMessage = "";

            yield return PostJson(url, apiKey, testPayload,
                (response) => {
                    testSuccess = true;
                    testMessage = "Connection successful!";
                },
                (error) => {
                    testSuccess = false;
                    testMessage = error;
                }
            );

            callback?.Invoke(testSuccess, testMessage);
        }

        // ═══════════════════════════════════════════════════════════════
        // Error Formatting
        // ═══════════════════════════════════════════════════════════════

        private static string FormatError(UnityWebRequest www)
        {
            string errorMsg = www.error ?? "Unknown error";

            // Enhanced error messages
            switch (www.responseCode)
            {
                case 401:
                    return "Invalid API Key";
                case 403:
                    return "Access Denied - Check API Key permissions";
                case 429:
                    return "Rate Limit Hit / Credits Exhausted";
                case 500:
                    return "Server Error - Provider may be overloaded";
                case 502:
                case 503:
                    return "Provider Unavailable - Try again later";
                case 0:
                    if (IsLocalProvider(www.url))
                    {
                        return "Cannot connect - Is LM Studio/Ollama running?";
                    }
                    return "Cannot connect - Check internet connection";
            }

            // Try to extract error message from response body
            string responseBody = "";
            try
            {
                responseBody = www.downloadHandler?.text ?? "";
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var errorMatch = System.Text.RegularExpressions.Regex.Match(responseBody, "\"error\".*?\"message\"\\s*:\\s*\"([^\"]+)\"");
                    if (errorMatch.Success)
                    {
                        return errorMatch.Groups[1].Value;
                    }
                }
            }
            catch { }

            // If we have a body but no specific error field, return part of the body for debugging
            if (!string.IsNullOrEmpty(responseBody) && responseBody.Length > 2)
            {
                string cleanBody = responseBody.Trim();
                if (cleanBody.Length > 100) cleanBody = cleanBody.Substring(0, 100) + "...";
                return $"{errorMsg} | Body: {cleanBody}";
            }

            return errorMsg;
        }
    }
}
