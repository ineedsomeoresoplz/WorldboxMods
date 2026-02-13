using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AIBox.Provider;

namespace AIBox
{
    public class KingdomController : MonoBehaviour
    {
        public static KingdomController Instance;
        private Queue<Kingdom> _thinkQueue = new Queue<Kingdom>();
        public static int MAX_CONCURRENT_REQUESTS = 3; 
        private int _activeRequests = 0;
        public bool IsAIEnabled = false;
        
        // Briefing
        public float GlobalBriefingInterval = 30f;
        public float KingdomThinkInterval = 30f;
        private float _nextBriefingTime = 0f;
        private float _checkTimer = 0f;
        public string LastGlobalBriefing = "No intelligence reports yet.";
        public bool IsGeneratingBriefing = false;

        // Configuration (Redirected to ProviderSettings)
        private string _modelApiUrl => ProviderSettings.Data.apiUrl;
        private string _modelName => ProviderSettings.Data.modelId;
        private string _apiKey => ProviderSettings.Data.apiKey;
        private string _customPrompt = ""; 
        private string _aiLanguage = "English"; 
        
        // Provider-aware checks
        private bool IsSelfHosted() => LLMClient.IsLocalProvider(_modelApiUrl);
        private bool SupportsJsonMode() => !IsSelfHosted() && !LLMClient.IsAnthropicUrl(_modelApiUrl);

        // Pause-aware time (SimTime)
        private float _simTime = 0f;
        public float SimTime => _simTime;

        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("KingdomController");
                Instance = go.AddComponent<KingdomController>();
                DontDestroyOnLoad(go);
                
                AILogger.Init();
                
                // FORCE CONFIG LOAD
                Instance.LoadConfig();

                if(string.IsNullOrEmpty(Instance._apiKey) && !Instance.IsSelfHosted()) {
                    Debug.LogWarning("[KingdomController] No API key configured for cloud provider.");
                    WorldTip.instance.show("<color=orange>⚠ AIBox: Cloud provider requires API key!\nSet it in Mod Settings</color>", false, "top", 8f);
                }
            }
        }
        
        private void LoadConfig()
        {
             try {
                 // PRIORITY 1: Load from new ProviderSettings system
                 LoadFromProviderSettings();
                 
                 // PRIORITY 2: Load remaining settings from default_config.json
                 var configPath = System.IO.Path.Combine(Mod.Info.Path, "default_config.json");
                 if(System.IO.File.Exists(configPath)) {
                     string json = System.IO.File.ReadAllText(configPath);
                     
                     // Helper to extract value by ID from NML style config
                     string ExtractValue(string id) {
                         var pattern = $"\"Id\"\\s*:\\s*\"{id}\".*?\"TextVal\"\\s*:\\s*\"([^\"]+)\"";
                         var match = System.Text.RegularExpressions.Regex.Match(json, pattern, System.Text.RegularExpressions.RegexOptions.Singleline);
                         if(match.Success) return match.Groups[1].Value;
                         return null;
                     }
                     
                     int ExtractIntValue(string id, int defaultVal) {
                         var pattern = $"\"Id\"\\s*:\\s*\"{id}\".*?\"IntVal\"\\s*:\\s*([0-9]+)";
                         var match = System.Text.RegularExpressions.Regex.Match(json, pattern, System.Text.RegularExpressions.RegexOptions.Singleline);
                         if(match.Success && int.TryParse(match.Groups[1].Value, out int result)) return result;
                         return defaultVal;
                     }
                     
                     // CRITICAL FIX: Load the custom prompt (WW2 scenario, etc.)
                     string prompt = ExtractValue("customPrompt");
                     if(!string.IsNullOrEmpty(prompt)) {
                         // Unescape the JSON string (convert \r\n to actual newlines)
                         prompt = prompt.Replace("\\r\\n", "\n").Replace("\\r", "\n").Replace("\\n", "\n");
                         _customPrompt = prompt;
                         Debug.Log($"[KingdomController] Loaded Custom Prompt: {_customPrompt.Substring(0, Math.Min(100, _customPrompt.Length))}...");
                     }
                     
                     // Load AI Language Setting
                     string langValue = ExtractValue("aiLanguage");
                     if(!string.IsNullOrEmpty(langValue)) {
                         _aiLanguage = langValue;
                         Debug.Log($"[KingdomController] AI Language set to: {_aiLanguage}");
                     } else {
                         // Fallback: try reading as integer index for backward compatibility
                         int langIndex = ExtractIntValue("aiLanguage", 0);
                         string[] languages = { "English", "Spanish", "French", "German", "Italian", "Portuguese", "Japanese", "Chinese", "Russian", "Indonesian" };
                         if(langIndex >= 0 && langIndex < languages.Length) {
                             _aiLanguage = languages[langIndex];
                             Debug.Log($"[KingdomController] AI Language set to: {_aiLanguage} (from index)");
                         }
                     }

                     // Load Integers
                     int queueSize = ExtractIntValue("maxConcurrentRequests", 3);
                     MAX_CONCURRENT_REQUESTS = queueSize;

                     int briefingInt = ExtractIntValue("globalBriefingInterval", 60);
                     GlobalBriefingInterval = (float)briefingInt;

                     int thinkInt = ExtractIntValue("kingdomThinkInterval", 30);
                     KingdomThinkInterval = (float)thinkInt;

                     Debug.Log($"[KingdomController] Loaded Config. Model: {_modelName}, Queue: {MAX_CONCURRENT_REQUESTS}, ThinkInt: {KingdomThinkInterval}");
                 }
             } catch (Exception e) {
                 Debug.LogWarning($"[KingdomController] Config Load Failed: {e.Message}");
             }
        }

        /// <summary>
        /// Load API settings from the new ProviderSettings system
        /// </summary>
        private void LoadFromProviderSettings()
        {
            try {
                ProviderSettings.Load();
                // Url/Key/Model/CustomPrompt/AILanguage are now managed by ProviderSettings (autoloaded via properties)
                // ProviderSettings.Load() is called staticly on access
                
                Debug.Log($"[KingdomController] Loaded from ProviderSettings: {ProviderSettings.GetCurrentProvider()?.Name ?? "Unknown"} / {_modelName}");
            }
            catch (Exception e) {
                Debug.LogWarning($"[KingdomController] ProviderSettings load failed (using fallback): {e.Message}");
            }
        }

        /// <summary>
        /// Reload settings from ProviderSettings (call after config window closes)
        /// </summary>
        public void ReloadFromProviderSettings()
        {
            LoadFromProviderSettings();
            Debug.Log($"[KingdomController] Reloaded config: API={_modelApiUrl}, Model={_modelName}");
        }

        public void QueueRequest(Kingdom k)
        {
            if(!IsAIEnabled) return;
            if(!_thinkQueue.Contains(k)) {
                _thinkQueue.Enqueue(k);
            }
        }

        void Update()
        {
            // PAUSE CHECK: Do not advance AI timers if paused
            if(Config.paused) return;

            // Advance custom sim time
            _simTime += Time.unscaledDeltaTime; 
            
            if(!IsAIEnabled) return;

            // Global Briefing Timer
            if(_simTime > _nextBriefingTime)
            {
                _nextBriefingTime = _simTime + GlobalBriefingInterval;
                RequestGlobalBriefing();
            }

            // AI Schedule Loop (Check every 1s)
            _checkTimer += Time.unscaledDeltaTime; // Use unscaled delta for the check loop itself, but it's gated by !paused
            if(_checkTimer > 1.0f) {
                _checkTimer = 0f;
                if(World.world?.kingdoms?.list != null) {
                    // ToList to avoid modification issues if list changes
                    foreach(var k in World.world.kingdoms.list) {
                        if(k == null || !k.isAlive() || !k.isCiv()) continue;
                        
                        var data = WorldDataManager.Instance.GetKingdomData(k);
                        if(data == null) continue;

                        // Initialize if 0
                        if(data.NextThinkTime <= 0) {
                            data.NextThinkTime = _simTime + UnityEngine.Random.Range(5f, KingdomThinkInterval);
                        }

                        if(_simTime >= data.NextThinkTime && !data.AI_IsThinking && !_thinkQueue.Contains(k)) {
                            QueueRequest(k);
                        }
                    }
                }
            }

            // Concurrent Throttling
            while (_activeRequests < MAX_CONCURRENT_REQUESTS && _thinkQueue.Count > 0)
            {
                Kingdom k = _thinkQueue.Dequeue();
                if(k != null && k.isAlive())
                {
                    StartCoroutine(ProcessKingdom(k));
                }
            }
        }

        private IEnumerator ProcessKingdom(Kingdom k)
        {
            _activeRequests++;
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            kData.AI_IsThinking = true;

            // Build Prompt using Builder
            string race = PromptGenerator.DetectRace(k.king);
            string personality = PromptGenerator.GetPersonalityNotes(k.king, race);
            
            // Ambition
            if(kData.AmbitionTimer <= 0 || string.IsNullOrEmpty(kData.SecretAmbition)) {
                kData.SecretAmbition = PromptGenerator.GetRandomAmbition();
                kData.AmbitionTimer = 3600f; 
            } else {
                kData.AmbitionTimer -= 600f;
            }

            string traitList = "Calculating";
            if(k.king != null) traitList = k.king.getTraitsAsLocalizedString();

            // Construct System Prompt - COMPACT VERSION (cost-optimized)
            StringBuilder system = new StringBuilder();
            
            // 1. COMPACT SYSTEM (contains rules, actions, format - all in ~400 tokens)
            system.AppendLine(PromptGenerator.GetCompactSystem(ModerBoxHelper.IsInstalled, _aiLanguage));
            
            // 2. WORLD CONTEXT - Sets the theme/scenario (if provided)
            if(!string.IsNullOrEmpty(_customPrompt)) {
                system.AppendLine($"\n[WORLD CONTEXT] {_customPrompt}");
            }
            
            // 3. Identity (compact)
            system.AppendLine($"\n[YOU] King {k.king?.getName() ?? "Regent"} of {k.name} ({race}). Traits: [{traitList}]");
            if(!string.IsNullOrEmpty(personality)) system.AppendLine(personality);
            
            // 4. Current goal (only if no divine commands)
            system.AppendLine($"[AMBITION] {kData.SecretAmbition}");
            
            // 5. World news (brief)
            if(!string.IsNullOrEmpty(LastGlobalBriefing) && LastGlobalBriefing != "No intelligence reports yet.") {
                system.AppendLine($"[NEWS] {LastGlobalBriefing}");
            }
            
            string systemPromptStr = system.ToString();
            string userContext = KingdomPerception.GetStateSnapshot(k, kData);

            // Inject Feedback from previous turn
            if(!string.IsNullOrEmpty(kData.LastTurnFeedback)) {
                userContext += $"\n\n[FEEDBACK FROM PREVIOUS ACTIONS]\n{kData.LastTurnFeedback}";
            }

            // Inject Divine Laws / Orders (Keep logic here as it depends on WorldDataManager state)
            if(WorldDataManager.Instance.GlobalDivineLaws.Count > 0) {
                userContext += "\n\n╔══════════════════════════════════════════════════════════════╗";
                userContext += "\n║           DIVINE LAWS - PERMANENT ABSOLUTE COMMANDS           ║";
                userContext += "\n╠══════════════════════════════════════════════════════════════╣\n";
                int i = 1;
                foreach(var kvp in WorldDataManager.Instance.GlobalDivineLaws) {
                    string law = kvp.Key;
                    string lawLower = law.ToLower();
                    string hint = "";
                    
                    if(lawLower.Contains("war") || lawLower.Contains("attack")) {
                        hint = "\n   → JSON: diplomatic_action.type=\"War\", targets=[ALL non-allied kingdoms]";
                    } else if(lawLower.Contains("peace")) {
                        hint = "\n   → JSON: diplomatic_action.type=\"Peace\", targets=[ALL enemy kingdoms]";
                    } else if(lawLower.Contains("alliance") || lawLower.Contains("ally")) {
                        hint = "\n   → JSON: diplomatic_action.type=\"Alliance\", targets=[ALL non-allied kingdoms]";
                    }
                    
                    userContext += $"║ LAW {i}: {law}{hint}\n";
                    i++;
                }
                userContext += "║\n";
                userContext += "║ EXECUTE EVERY TURN. Your diplomatic_action MUST match these laws.\n";
                userContext += "║ If law says 'peace' and you are at war → type=\"Peace\" NOW.\n";
                userContext += "║ If law says 'alliance' → type=\"Alliance\" with ALL valid kingdoms.\n";
                userContext += "╚══════════════════════════════════════════════════════════════╝\n";
            }
            if(!string.IsNullOrEmpty(kData.StandingOrders)) {
                 userContext += "\n\n╔═══════════════════════════════════════╗";
                 userContext += "\n║       YOUR DIVINE MANDATE             ║";
                 userContext += "\n╠═══════════════════════════════════════╣\n";
                 userContext += "║ " + kData.StandingOrders + "\n";
                 userContext += "╚═══════════════════════════════════════╝\n";
            }
            if(!string.IsNullOrEmpty(kData.PendingDivineWhisper)) {
                 kData.WasDivineCommand = true;
                 string whisper = kData.PendingDivineWhisper.ToLower();
                 
                 // Build explicit JSON instruction
                 string actionHint = "";
                 if(whisper.Contains("war") || whisper.Contains("attack") || whisper.Contains("destroy") || whisper.Contains("bloody")) {
                     actionHint = "diplomatic_action.type = \"War\" ← MUST BE 'War', NOT 'Message'!\ndiplomatic_action.targets = [Pick 1-3 kingdoms from DIPLOMACY]\ndiplomatic_action.war_reason = \"Divine Will\"";
                 } else if(whisper.Contains("peace") || whisper.Contains("truce")) {
                     actionHint = "diplomatic_action.type = \"Peace\" ← MUST BE 'Peace', NOT 'Message'!\ndiplomatic_action.targets = [List enemy kingdoms]";
                 } else if(whisper.Contains("alliance") || whisper.Contains("ally")) {
                     actionHint = "diplomatic_action.type = \"Alliance\" ← MUST BE 'Alliance', NOT 'Message'!\ndiplomatic_action.targets = [Pick 1-3 non-allied kingdoms]";
                 } else {
                     actionHint = "Execute the command described above. Set appropriate diplomatic_action fields.";
                 }
                 
                 userContext += "\n\n╔══════════════════════════════════════════════════════════════╗";
                 userContext += "\n║             DIVINE WHISPER - INSTANT EXECUTION               ║";
                 userContext += "\n╠══════════════════════════════════════════════════════════════╣";
                 userContext += $"\n║ GOD COMMANDS: \"{kData.PendingDivineWhisper}\"";
                 userContext += "\n║";
                 userContext += $"\n║ YOUR JSON MUST CONTAIN:\n║ {actionHint.Replace("\n", "\n║ ")}";
                 userContext += "\n║";
                 userContext += "\n║  ⚠ type='Message' = TALKING, NOT EXECUTING! You will FAIL!";
                 userContext += "\n║  ⚠ EXECUTE THE ACTION NOW. THIS TURN. NO EXCEPTIONS.";
                 userContext += "\n╚══════════════════════════════════════════════════════════════╝\n";
                 kData.PendingDivineWhisper = ""; 
            } else {
                 kData.WasDivineCommand = false;
            }

            kData.PendingOffers.Clear();
            kData.RecentDiplomaticEvents.Clear();
            
            string sysMsg = JsonUtility.ToJson(new OpenAIMessage { role = "system", content = systemPromptStr });
            string usrMsg = JsonUtility.ToJson(new OpenAIMessage { role = "user", content = userContext });
            
            // Only include response_format for providers that support it
            string responseFormat = SupportsJsonMode() ? "\"response_format\": { \"type\": \"json_object\" }," : "";
            
            string jsonPayload = $@"{{
                ""model"": ""{_modelName}"",
                ""messages"": [
                    {sysMsg},
                    {usrMsg}
                ],
                {responseFormat}
                ""max_tokens"": 2048,
                ""stream"": false
            }}";

            try
            {
                yield return LLMClient.PostJson(_modelApiUrl, _apiKey, jsonPayload, 
                    (response) => {
                        try {
                            ParseAndApply(k, response, systemPromptStr, userContext);
                        } catch(Exception ex) {
                            Debug.LogError($"[KingdomController] Parse Error: {ex}");
                        }
                    },
                    (error) => {
                        Debug.LogWarning($"[KingdomController] Request failed: {error}");
                    }
                );
            }
            finally
            {
                kData.AI_IsThinking = false;
                kData.NextThinkTime = _simTime + KingdomThinkInterval;
                _activeRequests--; 
            }
        }

        public void RequestGlobalBriefing()
        {
            if(IsGeneratingBriefing) return;
            _nextBriefingTime = _simTime + GlobalBriefingInterval;
            StartCoroutine(ProcessBriefing());
        }

        private IEnumerator ProcessBriefing()
        {
            IsGeneratingBriefing = true;
            
            // Build Facts (Briefing Logic)
             StringBuilder sb = new StringBuilder();
            
            var allKingdoms = World.world.kingdoms.list.Where(xk => xk.isAlive() && xk.isCiv()).ToList();
            List<string> facts = new List<string>();
            foreach(Kingdom k1 in allKingdoms) {
                foreach(Kingdom k2 in allKingdoms) {
                    if(k1 == k2) continue;
                    if(k1.isEnemy(k2) && k1.id < k2.id) facts.Add($"WAR: {k1.name} vs {k2.name}");
                }
            }
            // Rich/Poor
            var richList = allKingdoms.Select(xk => new { K = xk, D = WorldDataManager.Instance.GetKingdomData(xk) })
                                     .Where(x => x.D != null).OrderByDescending(x => x.D.Wealth).ToList();
            if(richList.Count > 0) {
                facts.Add($"Richest: {richList[0].K.name}");
                facts.Add($"Poorest: {richList[richList.Count-1].K.name}");
            }
            // Logs
            var logsList = WorldDataManager.Instance.GlobalDecisionLog.OrderByDescending(x => x.Timestamp).Take(5).ToList();
            foreach(var log in logsList) {
                if(log.DecisionSummary.Contains("WAR") || log.DecisionSummary.Contains("ALLIANCE")) {
                    facts.Add($"{log.KingdomName}: {log.DecisionSummary}");
                }
            }

            bool hasImportantEvent = facts.Any(f => f.Contains("WAR") || f.Contains("ALLIANCE") || f.Contains("CRISIS"));
            if(facts.Count < 3 && !hasImportantEvent) {
                IsGeneratingBriefing = false;
                yield break;
            }
            
            sb.AppendLine("Write 3-5 bullet points. Start with '•'. Max 50 words. FACTS:");
            foreach(var f in facts) sb.AppendLine($"- {f}");

            string userContext = sb.ToString(); 
            string sysMsg = JsonUtility.ToJson(new OpenAIMessage { role = "system", content = "You are a neutral, concise world news broadcaster." });
            string usrMsg = JsonUtility.ToJson(new OpenAIMessage { role = "user", content = userContext });
            
            string responseFormat = IsSelfHosted() ? "" : "\"response_format\": { \"type\": \"json_object\" },";

            string jsonPayload = $@"{{
                ""model"": ""{_modelName}"",
                ""messages"": [{sysMsg},{usrMsg}],
                {responseFormat}
                ""max_tokens"": 512,
                ""stream"": false
            }}";  

            yield return LLMClient.PostJson(_modelApiUrl, _apiKey, jsonPayload, 
                (response) => {
                     try {
                         int contentIndex = response.IndexOf("\"content\"");
                         if(contentIndex != -1) {
                             int start = response.IndexOf("\"", contentIndex + 10) + 1;
                             int end = response.IndexOf("\"", start); 
                             string content = response.Substring(start, end - start);
                             content = content.Replace("\\n", "\n").Replace("\\\"", "\"");
                             LastGlobalBriefing = content;
                             AILogger.LogGlobalBriefing(content);
                         }
                    } catch { LastGlobalBriefing = "Failed to transcribe history."; }
                },
                (error) => {
                     LastGlobalBriefing = "Intelligence network offline.";
                }
            );
            IsGeneratingBriefing = false;
        }

        private void ParseAndApply(Kingdom k, string jsonResponse, string systemPrompt, string userContext)
        {
            try {
                string innerJson = "";
                try {
                    var match = Regex.Match(jsonResponse, "\"content\"\\s*:\\s*\"((?:\\\\.|[^\"\\\\])*)\"");
                    if (!match.Success) return;

                    string rawContent = match.Groups[1].Value;
                    
                    // First, handle literal backslash-n sequences that some models output
                    // These appear as \\n in the raw content but should be newlines
                    rawContent = rawContent.Replace("\\n", "\n");
                    rawContent = rawContent.Replace("\\r", "");
                    rawContent = rawContent.Replace("\\t", "\t");
                    rawContent = rawContent.Replace("\\\"", "\"");
                    
                    // Now use Regex.Unescape for any remaining escape sequences
                    try {
                        innerJson = Regex.Unescape(rawContent);
                    } catch {
                        // If Unescape fails, use the manually cleaned content
                        innerJson = rawContent;
                    }
                    
                    // Remove carriage returns (source of corruption in logs)
                    innerJson = innerJson.Replace("\r", "").Trim();
                } catch (Exception ex) {
                    Debug.LogError($"[KingdomController] Extraction Error: {ex.Message}");
                    return;
                }
                    
                // Improved JSON Extraction: Remove <think> blocks and find first { and last }
                if (innerJson.Contains("<think>"))
                {
                    int thinkEnd = innerJson.LastIndexOf("</think>");
                    if (thinkEnd != -1) innerJson = innerJson.Substring(thinkEnd + 8);
                }

                if(innerJson.Contains("```json")) {
                    int codeStart = innerJson.IndexOf("```json") + 7;
                    int codeEnd = innerJson.LastIndexOf("```");
                    if(codeEnd > codeStart) {
                        innerJson = innerJson.Substring(codeStart, codeEnd - codeStart);
                    }
                } else if(innerJson.Contains("```")) {
                    // Generic code block
                    int codeStart = innerJson.IndexOf("```") + 3;
                    int codeEnd = innerJson.LastIndexOf("```");
                    if(codeEnd > codeStart) {
                        innerJson = innerJson.Substring(codeStart, codeEnd - codeStart);
                    }
                }

                int firstBrace = innerJson.IndexOf("{");
                int lastBrace = innerJson.LastIndexOf("}");
                
                if (firstBrace != -1 && lastBrace != -1 && lastBrace > firstBrace)
                {
                    innerJson = innerJson.Substring(firstBrace, lastBrace - firstBrace + 1);
                } else {
                    Debug.LogWarning($"[KingdomController] Could not find valid JSON object in response. Raw length: {jsonResponse.Length}");
                    return; 
                }
                
                // Fix common LLM JSON errors
                innerJson = Regex.Replace(innerJson, @",\s*}", "}");  // Trailing comma before }
                innerJson = Regex.Replace(innerJson, @",\s*]", "]");  // Trailing comma before ]
                innerJson = Regex.Replace(innerJson, @":\s*,", ": null,"); // Empty value fix

                AIDecision decision = null;
                try {
                    decision = JsonUtility.FromJson<AIDecision>(innerJson);
                } catch (Exception parseEx) {
                     Debug.LogError($"[KingdomController] JSON Parse Error: {parseEx.Message}");
                     Debug.LogError($"[KingdomController] Failed JSON Content: {innerJson}");
                     return; // Stop processing
                }
                
                if(decision != null) {
                    if(decision.diplomatic_action == null) {
                        decision.diplomatic_action = new DiplomaticAction();
                    }
                    
                    string dipType = ParseNestedJson(innerJson, "diplomatic_action", "type");
                    if(!string.IsNullOrEmpty(dipType) && dipType != "None") {
                        decision.diplomatic_action.type = dipType;
                        decision.diplomatic_action.target = ParseNestedJson(innerJson, "diplomatic_action", "target");
                        decision.diplomatic_action.war_reason = ParseNestedJson(innerJson, "diplomatic_action", "war_reason");
                        decision.diplomatic_action.message = ParseNestedJson(innerJson, "diplomatic_action", "text");
                        if(string.IsNullOrEmpty(decision.diplomatic_action.message) || decision.diplomatic_action.message == "None") {
                             decision.diplomatic_action.message = ParseNestedJson(innerJson, "diplomatic_action", "message");
                        }
                        
                        // Parse amount for Gifts
                        float dipAmt = 0; 
                        string amountStr = ParseNestedJson(innerJson, "diplomatic_action", "amount");
                        if(!string.IsNullOrEmpty(amountStr) && amountStr != "None") {
                             float.TryParse(amountStr, out dipAmt);
                             decision.diplomatic_action.amount = (int)dipAmt;
                        }

                        decision.diplomatic_action.targets = ParseTargetsArray(innerJson);
                        
                        Debug.Log($"[KingdomController] Parsed Diplo: type={dipType}, target={decision.diplomatic_action.target}, amt={decision.diplomatic_action.amount}, targets count={decision.diplomatic_action.targets?.Count}");
                    }

                    if(decision.monetary_action == null) {
                        decision.monetary_action = new MonetaryAction();
                    }
                    string mType = ParseNestedJson(innerJson, "monetary_action", "type");
                    if(!string.IsNullOrEmpty(mType) && mType != "None") {
                        decision.monetary_action.type = mType; 
                        float mAmt = 0; float.TryParse(ParseNestedJson(innerJson, "monetary_action", "amount"), out mAmt);
                        decision.monetary_action.amount = mAmt;
                    }
                    
                    // Policy Toggles for ModerBox (clean implementation)
                    if (!string.IsNullOrEmpty(decision.policy_change) && ModerBoxHelper.IsInstalled)
                    {
                        string policy = decision.policy_change.ToLower();
                        
                        // Check for gun production toggle
                        if (policy.Contains("gun")) {
                            bool enable = !policy.Contains("disable") && !policy.Contains("off");
                            KingdomActions.ToggleGunProduction(enable);
                            Debug.Log($"[AIBox] ModerBox: Gun production {(enable ? "enabled" : "disabled")}");
                        }
                        
                        // Check for vehicle production toggle
                        if (policy.Contains("vehicle")) {
                            bool enable = !policy.Contains("disable") && !policy.Contains("off");
                            KingdomActions.ToggleVehicleProduction(enable);
                            Debug.Log($"[AIBox] ModerBox: Vehicle production {(enable ? "enabled" : "disabled")}");
                        }
                    }

                    // Reset Feedback Log for this turn
                    var executionData = WorldDataManager.Instance.GetKingdomData(k);
                    if(executionData != null) executionData.LastTurnFeedback = "";

                    SimulationGameloop.ExecuteAICommand(k, decision);

                    // --- COMPREHENSIVE SUMMARY ---
                    var kData = WorldDataManager.Instance.GetKingdomData(k);
                    float newTax = kData.TaxRate;
                    List<string> parts = new List<string>();
                    
                    // 1. Tax
                    parts.Add($"TAX: {newTax:P0}");

                    // 2. Policy
                    if(decision.policy_change != "None" && !string.IsNullOrEmpty(decision.policy_change)) 
                        parts.Add($"POLICY: {decision.policy_change.ToUpper()}");

                    // 3. Monetary
                    if(decision.monetary_action != null && !string.IsNullOrEmpty(decision.monetary_action.type) && decision.monetary_action.type != "None") {
                        string monPart = $"MONETARY: {decision.monetary_action.type.ToUpper()}";
                        if(decision.monetary_action.amount > 0) monPart += $" {decision.monetary_action.amount:N0}";
                        parts.Add(monPart);
                    }

                    // 4. Diplo - Use the already-parsed diplomatic action
                    if(decision.diplomatic_action != null && !string.IsNullOrEmpty(decision.diplomatic_action.type) && decision.diplomatic_action.type != "None") {
                        string dipTarget = decision.diplomatic_action.target ?? "";
                        
                        // If we have targets array, use those names
                        if(decision.diplomatic_action.targets != null && decision.diplomatic_action.targets.Count > 0) {
                            dipTarget = string.Join(", ", decision.diplomatic_action.targets);
                        }
                        
                        // Fallback if both are empty
                        if(string.IsNullOrEmpty(dipTarget)) dipTarget = "All";
                        
                        string dipPart = "DIPLO: ";
                        if(decision.diplomatic_action.type == "War") {
                            string warReason = decision.diplomatic_action.war_reason ?? "Expansion";
                            dipPart += $"WAR vs {dipTarget} ({warReason})";
                        } else if(decision.diplomatic_action.type == "Message") {
                            dipPart += $"MSG {dipTarget}";
                        } else {
                            dipPart += $"{decision.diplomatic_action.type.ToUpper()} {dipTarget}";
                        }
                        parts.Add(dipPart);
                    }

                    // 5. Covert Actions
                    try {
                        string covertType = ParseNestedJson(innerJson, "covert_action", "type");
                        if(!string.IsNullOrEmpty(covertType) && covertType != "None") {
                            string covertTarget = ParseNestedJson(innerJson, "covert_action", "target");
                            if(decision.covert_action == null) decision.covert_action = new CovertAction();
                            decision.covert_action.type = covertType;
                            decision.covert_action.target = covertTarget;
                            
                            string covertPart = $"COVERT: {covertType.ToUpper()}";
                            if(!string.IsNullOrEmpty(covertTarget) && covertTarget != "None") {
                                covertPart += $" on {covertTarget}";
                            }
                            parts.Add(covertPart);
                        }
                    } catch {}

                    // 6. Ruler Actions (Add to log)
                    try {
                        string rulerType = ParseNestedJson(innerJson, "ruler_action", "type");
                        if(!string.IsNullOrEmpty(rulerType) && rulerType != "None") {
                            decision.ruler_action = new RulerAction();
                            decision.ruler_action.type = rulerType;
                            parts.Add($"RULER: {rulerType.ToUpper()}");
                        }
                    } catch {}
                    
                    // 7. Modern Warfare (Auto + Manual Fallback)
                    try {
                        // Check if JsonUtility already parsed it
                        if (decision.modern_warfare_action != null && !string.IsNullOrEmpty(decision.modern_warfare_action.type) && decision.modern_warfare_action.type != "None") {
                             // Auto-parsed successfully!
                        } else {
                            // Fallback Manual Parse
                            string mwType = ParseNestedJson(innerJson, "modern_warfare_action", "type");
                            if (!string.IsNullOrEmpty(mwType) && mwType != "None") {
                                 if (decision.modern_warfare_action == null) decision.modern_warfare_action = new ModernWarfareAction();
                                 decision.modern_warfare_action.type = mwType;
                                 decision.modern_warfare_action.target = ParseNestedJson(innerJson, "modern_warfare_action", "target");
                            }
                        }

                        // Execution
                        if (decision.modern_warfare_action != null && 
                            !string.IsNullOrEmpty(decision.modern_warfare_action.type) && 
                            decision.modern_warfare_action.type != "None") 
                        {
                             string mwType = decision.modern_warfare_action.type;
                             string mwTarget = decision.modern_warfare_action.target;
                             Debug.Log($"[AIBox] EXECUTE MODERN WARFARE: {mwType} vs {mwTarget}");

                             Kingdom targetK = KingdomActions.FindKingdomByName(mwTarget);
                             if (mwType == "LaunchNuke") {
                                 KingdomActions.LaunchNuke(k, targetK);
                                 parts.Add($"NUKE: Launched at {mwTarget}");
                             }
                             else if (mwType == "LaunchMissile") {
                                 KingdomActions.LaunchMissile(k, targetK);
                                 parts.Add($"MISSILE: Launched at {mwTarget}");
                             }
                        }
                    } catch (Exception ex) {
                        Debug.LogError($"[AIBox] MW Error: {ex.Message}");
                    }

                    // 8. Trade
                    try {
                        string tradeType = ParseNestedJson(innerJson, "trade_action", "type");
                        if(!string.IsNullOrEmpty(tradeType) && tradeType != "None") {
                             parts.Add($"TRADE: {tradeType.ToUpper()}");
                        }
                    } catch {}

                    // 9. Market
                    try {
                        if(!string.IsNullOrEmpty(decision.target_resource) && decision.target_resource != "None") {
                            parts.Add($"MARKET: Focus {decision.target_resource.ToUpper()}");
                        }
                    } catch {}

                    // 10. Culture Actions
                    try {
                        // Auto-parse or manual fallback
                        if (decision.culture_action == null || string.IsNullOrEmpty(decision.culture_action?.type)) {
                            string cType = ParseNestedJson(innerJson, "culture_action", "type");
                            if (!string.IsNullOrEmpty(cType) && cType != "None") {
                                if (decision.culture_action == null) decision.culture_action = new CultureAction();
                                decision.culture_action.type = cType;
                                decision.culture_action.target_culture = ParseNestedJson(innerJson, "culture_action", "target_culture");
                            }
                        }
                        
                        if (decision.culture_action != null && 
                            !string.IsNullOrEmpty(decision.culture_action.type) && 
                            decision.culture_action.type != "None") 
                        {
                            bool success = CultureReligionHelper.ExecuteCultureAction(k, decision.culture_action, kData);
                            string cultPart = $"CULTURE: {decision.culture_action.type.ToUpper()}";
                            if (!string.IsNullOrEmpty(decision.culture_action.target_culture)) {
                                cultPart += $" ({decision.culture_action.target_culture})";
                            }
                            parts.Add(cultPart);
                            Debug.Log($"[AIBox] Culture Action: {decision.culture_action.type} - Success: {success}");
                        }
                    } catch (Exception ex) {
                        Debug.LogError($"[AIBox] Culture Action Error: {ex.Message}");
                    }

                    // 11. Religion Actions
                    try {
                        if (decision.religion_action == null || string.IsNullOrEmpty(decision.religion_action?.type)) {
                            string rType = ParseNestedJson(innerJson, "religion_action", "type");
                            if (!string.IsNullOrEmpty(rType) && rType != "None") {
                                if (decision.religion_action == null) decision.religion_action = new ReligionAction();
                                decision.religion_action.type = rType;
                                decision.religion_action.target_religion = ParseNestedJson(innerJson, "religion_action", "target_religion");
                            }
                        }
                        
                        if (decision.religion_action != null && 
                            !string.IsNullOrEmpty(decision.religion_action.type) && 
                            decision.religion_action.type != "None") 
                        {
                            bool success = CultureReligionHelper.ExecuteReligionAction(k, decision.religion_action, kData);
                            string relPart = $"RELIGION: {decision.religion_action.type.ToUpper()}";
                            if (!string.IsNullOrEmpty(decision.religion_action.target_religion)) {
                                relPart += $" ({decision.religion_action.target_religion})";
                            }
                            parts.Add(relPart);
                            Debug.Log($"[AIBox] Religion Action: {decision.religion_action.type} - Success: {success}");
                        }
                    } catch (Exception ex) {
                        Debug.LogError($"[AIBox] Religion Action Error: {ex.Message}");
                    }

                    // 12. Demographic Actions
                    try {
                        if (decision.demographic_action == null || string.IsNullOrEmpty(decision.demographic_action?.type)) {
                            string dType = ParseNestedJson(innerJson, "demographic_action", "type");
                            if (!string.IsNullOrEmpty(dType) && dType != "None") {
                                if (decision.demographic_action == null) decision.demographic_action = new DemographicAction();
                                decision.demographic_action.type = dType;
                                decision.demographic_action.target_race = ParseNestedJson(innerJson, "demographic_action", "target_race");
                            }
                        }
                        
                        if (decision.demographic_action != null && 
                            !string.IsNullOrEmpty(decision.demographic_action.type) && 
                            decision.demographic_action.type != "None") 
                        {
                            bool success = CultureReligionHelper.ExecuteDemographicAction(k, decision.demographic_action, kData);
                            string demoPart = $"DEMOGRAPHIC: {decision.demographic_action.type.ToUpper()}";
                            if (!string.IsNullOrEmpty(decision.demographic_action.target_race)) {
                                demoPart += $" ({decision.demographic_action.target_race})";
                            }
                            parts.Add(demoPart);
                            Debug.Log($"[AIBox] Demographic Action: {decision.demographic_action.type} - Success: {success}");
                        }
                    } catch (Exception ex) {
                        Debug.LogError($"[AIBox] Demographic Action Error: {ex.Message}");
                    }


                    string summary = string.Join(" | ", parts);
                    
                    try {
                        WorldDataManager.Instance.LogDecision(k, decision.reasoning ?? "", summary, userContext, jsonResponse);
                        AILogger.LogInteraction(k, systemPrompt, userContext, decision.reasoning ?? "", summary, jsonResponse);
                        
                        // Save to action memory so AI knows what it has done
                        kData.RecentActions.Add(summary);
                        if(kData.RecentActions.Count > 5) kData.RecentActions.RemoveAt(0);
                    } catch (Exception logEx) {
                        Debug.LogWarning($"[KingdomController] Log Error: {logEx.Message}");
                    }
                }
            } catch (Exception e) {
                Debug.LogError($"[KingdomController] Parse Error: {e.Message}\n{e.StackTrace}");
            }
        }

        private string ParseNestedJson(string json, string parentKey, string childKey)
        {
            try {
                int pIdx = json.IndexOf("\"" + parentKey + "\"");
                if(pIdx == -1) return "None";
                int openBrace = json.IndexOf("{", pIdx);
                int closeBrace = json.IndexOf("}", openBrace);
                if(openBrace == -1) return "None";
                string segment = json.Substring(openBrace, closeBrace - openBrace + 1);
                int cIdx = segment.IndexOf("\"" + childKey + "\"");
                if(cIdx == -1) return "None";
                int colon = segment.IndexOf(":", cIdx);
                int curr = colon + 1;
                while(curr < segment.Length && char.IsWhiteSpace(segment[curr])) curr++;
                if (segment[curr] == '"') {
                     int valStart = curr + 1;
                     int valEnd = segment.IndexOf("\"", valStart);
                     return segment.Substring(valStart, valEnd - valStart);
                } else {
                     int nextComma = segment.IndexOf(",", curr);
                     int nextBrace = segment.IndexOf("}", curr);
                     int end = nextComma;
                     if (end == -1 || (nextBrace != -1 && nextBrace < end)) end = nextBrace;
                     return segment.Substring(curr, end - curr).Trim();
                }
            } catch {}
            return "None";
        }

        private string ParseNestedArrayJson(string json, string parentKey, string childKey)
        {
             // Simple helper to detect if an array exists at a key for summary purposes
             // Not a full array parser, just checks presence
             try {
                int pIdx = json.IndexOf("\"" + parentKey + "\"");
                if(pIdx == -1) return null;
                int openBrace = json.IndexOf("{", pIdx);
                if(openBrace == -1) return null;
                
                // Find child key inside
                int cIdx = json.IndexOf("\"" + childKey + "\"", openBrace);
                if(cIdx == -1) return null;
                
                int colon = json.IndexOf(":", cIdx);
                int curr = colon + 1;
                while(curr < json.Length && char.IsWhiteSpace(json[curr])) curr++;
                
                if(json[curr] == '[') return "Array";
             } catch {}
             return null;
        }

        /// Parses the "targets" array from diplomatic_action in the JSON
        private List<string> ParseTargetsArray(string json)
        {
            List<string> result = new List<string>();
            try {
                // Find diplomatic_action
                int dipIdx = json.IndexOf("\"diplomatic_action\"");
                if (dipIdx == -1) return result;
                
                // Find targets within diplomatic_action
                int targetsIdx = json.IndexOf("\"targets\"", dipIdx);
                if (targetsIdx == -1) return result;
                
                // Find the opening bracket
                int openBracket = json.IndexOf("[", targetsIdx);
                if (openBracket == -1) return result;
                
                // Find the closing bracket
                int closeBracket = json.IndexOf("]", openBracket);
                if (closeBracket == -1) return result;
                
                // Extract array content
                string arrayContent = json.Substring(openBracket + 1, closeBracket - openBracket - 1);
                
                // Parse each quoted string
                int pos = 0;
                while (pos < arrayContent.Length) {
                    int startQuote = arrayContent.IndexOf("\"", pos);
                    if (startQuote == -1) break;
                    
                    int endQuote = arrayContent.IndexOf("\"", startQuote + 1);
                    if (endQuote == -1) break;
                    
                    string name = arrayContent.Substring(startQuote + 1, endQuote - startQuote - 1);
                    if (!string.IsNullOrEmpty(name) && name != "KingdomName" && name != "Multiple Targets") {
                        result.Add(name);
                    }
                    
                    pos = endQuote + 1;
                }
            } catch (Exception ex) {
                Debug.LogWarning($"[KingdomController] ParseTargetsArray error: {ex.Message}");
            }
            return result;
        }

        // Callbacks for NML Config (Legacy fields removed, but these are kept)
        public static void OnCustomPromptChanged(string pValue) { if(Instance) Instance._customPrompt = pValue; }
        public static void OnAILanguageTextChanged(string pValue) { 
            if(Instance && !string.IsNullOrEmpty(pValue)) {
                Instance._aiLanguage = pValue;
                Debug.Log($"[KingdomController] AI Language changed to: {Instance._aiLanguage}");
            }
        }
        // Removed: OnApiUrlChanged, OnModelNameChanged, OnApiKeyChanged
        public static void OnGlobalBriefingIntervalChanged(int pValue) { if(Instance) Instance.GlobalBriefingInterval = pValue; }
        public static void OnKingdomThinkIntervalChanged(int pValue) { if(Instance) Instance.KingdomThinkInterval = (float)pValue; }
        public static void OnMaxConcurrentRequestsChanged(int pValue) { MAX_CONCURRENT_REQUESTS = pValue; }

        public IEnumerator TestAPIConnection()
        {
            WorldTip.instance.show("<color=yellow>Testing connection...</color>", false, "top", 3f);
            string jsonPayload = $@"{{ ""model"": ""{_modelName}"", ""messages"": [{{""role"":""user"",""content"":""hi""}}], ""stream"": false, ""max_tokens"": 5 }}";
            
            yield return LLMClient.PostJson(_modelApiUrl, _apiKey, jsonPayload, 
                (res) => { WorldTip.instance.show("<color=green>✓ SUCCESS!</color>", false, "top", 5f); },
                (err) => { WorldTip.instance.show($"<color=red>FAILED: {err}</color>", false, "top", 5f); }
            );
        }
    }
}


