using System;
using System.IO;
using System.Text;
using UnityEngine;
using NCMS;

namespace AIBox
{
    public static class AILogger
    {
        private static string _logFilePath;

        public static void Init()
        {
            _logFilePath = Path.Combine(Mod.Info.Path, "ai_story_log.txt");
            // Create or overwrite the file header
            File.WriteAllText(_logFilePath, $"AI STORY LOG - Started at {DateTime.Now}\n==========================================\n\n");
        }

        public static void LogInteraction(Kingdom k, string systemPrompt, string userContext, string reasoning, string action, string rawAIOutput = "")
        {
            if (string.IsNullOrEmpty(_logFilePath)) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"╔══════════════════════════════════════════════════════════════════════════════");
            sb.AppendLine($"║ [{DateTime.Now:HH:mm:ss}] KINGDOM: {k.name}");
            sb.AppendLine($"╚══════════════════════════════════════════════════════════════════════════════");
            sb.AppendLine();
            
            // SYSTEM PROMPT (What instructions the AI received)
            sb.AppendLine("┌─ SYSTEM PROMPT ───────────────────────────────────────────────────────────────");
            sb.AppendLine(systemPrompt.Trim());
            sb.AppendLine("└───────────────────────────────────────────────────────────────────────────────");
            sb.AppendLine();
            
            // USER CONTEXT (Current state, diplomacy, resources, etc.)
            sb.AppendLine("┌─ USER CONTEXT (INPUT) ────────────────────────────────────────────────────────");
            sb.AppendLine(userContext.Trim());
            sb.AppendLine("└───────────────────────────────────────────────────────────────────────────────");
            sb.AppendLine();
            
            // RAW AI OUTPUT (Complete JSON response from LLM)
            if (!string.IsNullOrEmpty(rawAIOutput))
            {
                sb.AppendLine("┌─ RAW AI OUTPUT (COMPLETE RESPONSE) ───────────────────────────────────────────");
                sb.AppendLine(rawAIOutput.Trim());
                sb.AppendLine("└───────────────────────────────────────────────────────────────────────────────");
                sb.AppendLine();
            }
            
            // PARSED REASONING (What the AI was thinking)
            sb.AppendLine("┌─ AI REASONING ────────────────────────────────────────────────────────────────");
            sb.AppendLine(reasoning);
            sb.AppendLine("└───────────────────────────────────────────────────────────────────────────────");
            sb.AppendLine();
            
            // EXECUTED ACTION (Summary of what actually happened)
            sb.AppendLine("┌─ EXECUTED ACTION ─────────────────────────────────────────────────────────────");
            sb.AppendLine(action);
            sb.AppendLine("└───────────────────────────────────────────────────────────────────────────────");
            sb.AppendLine();
            sb.AppendLine("================================================================================\n");

            try
            {
                File.AppendAllText(_logFilePath, sb.ToString());
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AILogger] Failed to write log: {e.Message}");
            }
        }

        public static void LogGlobalBriefing(string reportContent)
        {
            if (string.IsNullOrEmpty(_logFilePath)) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine($"========= GLOBAL BRIEFING [{DateTime.Now:HH:mm:ss}] =========");
            sb.AppendLine("==================================================");
            sb.AppendLine(reportContent.Trim());
            sb.AppendLine("==================================================\n");

            try
            {
                File.AppendAllText(_logFilePath, sb.ToString());
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AILogger] Failed to write global briefing log: {e.Message}");
            }
        }
    }
}
