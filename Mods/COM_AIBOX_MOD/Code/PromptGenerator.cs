using System;
using UnityEngine;

namespace AIBox
{
    
    /// Builds system prompts for AI kingdom governors.
    /// Extracted from KingdomController for cleaner separation of concerns.
    /// COMMAND PRIORITY: Divine Whispers > Divine Laws > World Context > Base Rules
    public static class PromptGenerator
    {
        // =====================================================================
        // LANGUAGE SUPPORT
        // =====================================================================
        /// Returns language-specific instruction to include in system prompt
        /// The AI will respond in the selected language for 'reasoning' field
        /// while keeping all JSON keys in English
        public static string GetLanguageInstruction(string language)
        {
            switch (language)
            {
                case "Spanish":
                    return @"
=== IDIOMA / LANGUAGE ===
Responde con el campo 'reasoning' en ESPAÑOL. Todos los nombres de campos JSON deben permanecer en inglés.
Ejemplo: {""reasoning"": ""Mis pensamientos en español..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "French":
                    return @"
=== LANGUE / LANGUAGE ===
Répondez avec le champ 'reasoning' en FRANÇAIS. Tous les noms de champs JSON doivent rester en anglais.
Exemple: {""reasoning"": ""Mes pensées en français..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "German":
                    return @"
=== SPRACHE / LANGUAGE ===
Antworten Sie mit dem Feld 'reasoning' auf DEUTSCH. Alle JSON-Feldnamen müssen auf Englisch bleiben.
Beispiel: {""reasoning"": ""Meine Gedanken auf Deutsch..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Italian":
                    return @"
=== LINGUA / LANGUAGE ===
Rispondi con il campo 'reasoning' in ITALIANO. Tutti i nomi dei campi JSON devono rimanere in inglese.
Esempio: {""reasoning"": ""I miei pensieri in italiano..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Portuguese":
                    return @"
=== IDIOMA / LANGUAGE ===
Responda com o campo 'reasoning' em PORTUGUÊS. Todos os nomes de campos JSON devem permanecer em inglês.
Exemplo: {""reasoning"": ""Meus pensamentos em português..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Japanese":
                    return @"
=== 言語 / LANGUAGE ===
'reasoning'フィールドを日本語で応答してください。すべてのJSONフィールド名は英語のままにしてください。
例: {""reasoning"": ""日本語での私の考え..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Chinese":
                    return @"
=== 语言 / LANGUAGE ===
使用中文回复'reasoning'字段。所有JSON字段名必须保持英文。
示例: {""reasoning"": ""我的中文想法..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Russian":
                    return @"
=== ЯЗЫК / LANGUAGE ===
Отвечайте с полем 'reasoning' на РУССКОМ. Все названия полей JSON должны оставаться на английском.
Пример: {""reasoning"": ""Мои мысли на русском..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "Indonesian":
                    return @"
=== BAHASA / LANGUAGE ===
Jawab dengan bidang 'reasoning' dalam BAHASA INDONESIA. Semua nama bidang JSON harus tetap dalam bahasa Inggris.
Contoh: {""reasoning"": ""Pemikiran saya dalam bahasa Indonesia..."", ""tax_rate_target"": 0.2}
=====================";
                
                case "English":
                default:
                    return ""; // No additional instruction needed for English
            }
        }

        // =====================================================================
        // COMPACT SYSTEM PROMPT 
        // =====================================================================
        public static string GetCompactSystem(bool includeModernWarfare, string language = "English")
        {
            string mwField = includeModernWarfare ? 
                ",\n\"modern_warfare_action\":{\"type\":\"None\",\"target\":\"\"}" : "";
            string mwAction = includeModernWarfare ? 
                "\n• MODERN: LaunchNuke/LaunchMissile via modern_warfare_action" : "";
            
            string languageInstruction = GetLanguageInstruction(language);

            return $@"You are a kingdom ruler. JSON ONLY. 5+ actions per turn.
{languageInstruction}

PRIORITY: Divine Whisper > Divine Laws > World Context > Traits

RULES:
• Min 5 actions | Max 3 diplomatic targets | Names from DIPLOMACY list only
• Reasoning: 2-3 sentences, in-character, no numbers
• Festival: STRICTLY FORBIDDEN unless unhappy_cities > 0 OR loyalty < 40% (Don't spam!)
• VARIETY: Check YOUR RECENT ACTIONS. Don't repeat same action type 3x in a row. Try different categories each turn!
• ANTI-HALLUCINATION: ONLY use kingdom names from DIPLOMACY list. NEVER invent events/treaties.

JSON STRUCTURE (CRITICAL):
• Each field = SINGLE OBJECT {{}} not array []
• Multiple targets go in ""targets"" array: [""K1"",""K2"",""K3""]
• WRONG: ""diplomatic_action"":[...] | RIGHT: ""diplomatic_action"":{{...}}

DIPLOMATIC TYPES:
• War: {{""type"":""War"",""targets"":[""X""],""war_reason"":""Y""}} - EXECUTES war
• Peace: {{""type"":""Peace"",""targets"":[""X""]}} - ENDS war
• Alliance: {{""type"":""Alliance"",""targets"":[""X""]}} - STRICT: Same Culture/Religion OR Opinion>100 REQUIRED.
• BreakAlliance: {{""type"":""BreakAlliance"",""targets"":[""X""]}} - Betray ally (Huge relations hit)
• Gift: {{""type"":""Gift"",""targets"":[""X""],""amount"":100}} - Send Gold to improve relations
• Mediate: {{""type"":""Mediate"",""targets"":[""K1"",""K2""]}} - End war between others (Cost: Gold)
• Message: {{""type"":""Message"",""targets"":[""X""],""message"":""Y""}} - TALK only, no action!

ACTIONS:
• DIPLO: War/Peace/Alliance/BreakAlliance/Gift/Mediate/Message/Threaten/Pact
• ECON: tax_rate_target(0-1), policy_change (ONLY: Austerity, Stimulus, Protectionism, FreeMarket)
• MONETARY: Print/Burn/Loan/Repay + amount | DeclareBankruptcy (Reset Debt, Lose Army/Stability)
• COVERT: Spy/TrainArmy/Sabotage/Assassinate + target
• MARKET: Buy/Sell + resource + amount | target_resource for focus
• RULER: Festival(Conditional)/Disband | UNION: FormEconomicUnion/Leave
• VASSAL: InstallPuppet (after winning war) / AnnexVassal / GrantIndependence
• BUILD: ConstructBuilding + building_type (barracks/mine/windmill/fishing_docks){mwAction}
• CULTURE: SpreadCulture/SuppressCulture/CulturalPurge/AssimilateMinority + target_culture
• RELIGION: EnforceReligion/BanReligion/ReligiousPersecution/Tolerance + target_religion
• DEMOGRAPHIC: Segregate/Integrate/Expel/Purge + target_race

STRATEGIC HINTS:
WINNING WAR? → Use InstallPuppet to make enemy a vassal (ends war, gains tribute)
WEALTHY (Gold>500)? → Use Build to construct barracks/mine for long-term power
HIGH DEBT? → Use Austerity policy + Repay monetary action
LOSING WAR? → Use Peace to end the conflict before destruction

JSON:
{{
""reasoning"":""In-character thoughts"",
""tax_rate_target"":0.1,
""policy_change"":""None"",
""target_resource"":""None"",
""monetary_action"":{{""type"":""None"",""amount"":0}},
""diplomatic_action"":{{""type"":""None"",""targets"":[],""message"":"""",""war_reason"":""""}},
""covert_action"":{{""type"":""None"",""target"":"""",""blame_target"":""""}},
""market_action"":{{""type"":""None"",""resource"":"""",""amount"":0}},
""ruler_action"":{{""type"":""None""}},
""union_action"":{{""type"":""None"",""target"":""""}},
""vassal_action"":{{""type"":""None"",""target"":""""}},
""build_action"":{{""type"":""None"",""building_type"":""""}}{mwField},
""culture_action"":{{""type"":""None"",""target_culture"":""""}},
""religion_action"":{{""type"":""None"",""target_religion"":""""}},
""demographic_action"":{{""type"":""None"",""target_race"":""""}}
}}";
        }

        // =====================================================================
        // SECRET AMBITIONS
        // =====================================================================
        public static readonly string[] SECRET_AMBITIONS = new string[] {
            "Crush the strongest kingdom and claim their lands.",
            "Bankrupt my rivals through economic warfare.",
            "Build a war chest to fund the annihilation of my enemies.",
            "Isolate my neighbors and pick them off one by one.",
            "Destabilize the world with inflation and pick up the pieces.",
            "Create a puppet empire of vassal states.",
            "Control all trade and strangle my enemies economically.",
            "Prepare a surprise war against the most trusted 'ally'."
        };

        
        /// Detects race from actor asset ID
        public static string DetectRace(Actor king)
        {
            if (king == null) return "Unknown";
            
            string assetId = king.asset.id.ToLower();
            if (assetId.Contains("human")) return "Human";
            if (assetId.Contains("orc")) return "Orc";
            if (assetId.Contains("elf")) return "Elf";
            if (assetId.Contains("dwarf")) return "Dwarf";
            return assetId.Replace("unit_", "").ToUpper();
        }

        
        /// Gets personality notes based on king's traits
        public static string GetPersonalityNotes(Actor king, string race)
        {
            if (king == null) return "";
            
            string notes = "";
            
            if (king.hasTrait("paranoid") || king.hasTrait("suspicious"))
                notes += "I trust NO ONE. Even my allies are plotting against me. I must watch them closely. When at peace, I spy and sabotage to stay ahead.\n";
            
            if (king.hasTrait("ambitious") || king.hasTrait("greedy"))
                notes += $"My race - the {race}s - shall DOMINATE. All other races are stepping stones to my supremacy.\n";
            
            if (king.hasTrait("deceitful") || king.hasTrait("liar"))
                notes += "I will smile at my enemies while sharpening my blade. Alliances are temporary tools for betrayal. Covert operations are my true strength.\n";
            
            if (king.hasTrait("savage") || king.hasTrait("bloodlust"))
                notes += "War is my purpose. Blood must flow. The weak exist only to be crushed.\n";
            
            if (king.hasTrait("madness") || king.hasTrait("cursed"))
                notes += "The voices whisper strategies... chaos is beautiful... unpredictability is my weapon.\n";
            
            return notes;
        }

        
        /// Gets a random secret ambition for a king
        public static string GetRandomAmbition()
        {
            return SECRET_AMBITIONS[UnityEngine.Random.Range(0, SECRET_AMBITIONS.Length)];
        }

        
        /// Builds the WORLD CONTEXT section for scenario/theme roleplaying
        public static string BuildWorldContextSection(string customPrompt)
        {
            if (string.IsNullOrEmpty(customPrompt)) return "";
            return $@"
=== WORLD CONTEXT (ROLEPLAY THIS) ===
{customPrompt}

You MUST roleplay according to this context. Stay in character.
Do NOT invent details not specified. If the context says 'medieval', be medieval.
If the context gives you a role or scenario, embody it completely.
This context sets the THEME but Divine Laws and Whispers still override your actions.
===========================================";
        }
    }
}



