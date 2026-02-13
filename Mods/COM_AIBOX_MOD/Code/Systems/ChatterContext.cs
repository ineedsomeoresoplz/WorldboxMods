using System;
using System.Collections.Generic;
using System.Linq;

namespace AIBox
{
    public enum MoodLevel { Ecstatic, Happy, Neutral, Unhappy, Miserable }
    
    public class ChatterContext
    {
        // Unit State
        public MoodLevel Mood;
        public string UnitName;
        public string Role;  
        public List<string> Traits = new List<string>();
        public bool IsHungry;
        public bool IsInjured;
        
        // Kingdom State
        public string KingdomName;
        public bool AtWar;
        public string EnemyName;
        public bool HighTaxes;
        public bool LowTaxes;
        public bool InDebt;
        public bool Prosperous;
        public string EconomicPolicy;
        
        // Recent AI Decision 
        public string LastAIAction;
        public string LastAITarget;
        public string LastAIReasoning;
        
        // World Events
        public bool GlobalWar;
        public bool RecentPeace;
        
        private static System.Random rnd = new System.Random();

        public static ChatterContext Extract(Actor unit)
        {
            var ctx = new ChatterContext();
            
            if (unit == null || unit.kingdom == null) return ctx;
            
            // Unit basics
            ctx.UnitName = unit.getName();
            ctx.KingdomName = unit.kingdom.name;
            
            // Role detection
            if (unit.isKing()) ctx.Role = "King";
            else if (unit.isCityLeader()) ctx.Role = "Leader";
            else if (unit.isWarrior()) ctx.Role = "Soldier";
            else ctx.Role = "Citizen";
            
            // Mood from happiness
            try {
                float happinessRatio = unit.getHappinessRatio();
                if (happinessRatio > 0.8f) ctx.Mood = MoodLevel.Ecstatic;
                else if (happinessRatio > 0.6f) ctx.Mood = MoodLevel.Happy;
                else if (happinessRatio > 0.4f) ctx.Mood = MoodLevel.Neutral;
                else if (happinessRatio > 0.2f) ctx.Mood = MoodLevel.Unhappy;
                else ctx.Mood = MoodLevel.Miserable;
            } catch { ctx.Mood = MoodLevel.Neutral; }
            
            // Traits
            try {
                foreach (var trait in unit.getTraits()) {
                    ctx.Traits.Add(trait.id);
                }
            } catch {}
            
            // Physical state
            try { ctx.IsHungry = unit.isHungry(); } catch {}
            try { ctx.IsInjured = unit.getHealthRatio() < 0.5f; } catch {}
            
            // Kingdom economy data
            var kData = WorldDataManager.Instance?.GetKingdomData(unit.kingdom);
            if (kData != null)
            {
                ctx.HighTaxes = kData.TaxRate > 0.25f;
                ctx.LowTaxes = kData.TaxRate < 0.05f;
                ctx.InDebt = kData.NationalDebt > kData.Wealth * 0.5f;
                ctx.Prosperous = kData.Wealth > WorldDataManager.Instance.GlobalAverageWealth * 1.5f;
                ctx.EconomicPolicy = kData.CurrentPolicy.ToString();
                
                // Recent AI decision
                if (kData.ThinkingHistory != null && kData.ThinkingHistory.Count > 0)
                {
                    var lastDecision = kData.ThinkingHistory.Last();
                    ctx.LastAIAction = lastDecision.DecisionSummary;
                    ctx.LastAIReasoning = lastDecision.Reasoning;
                    
                    // Parse target from decision (simplified)
                    if (!string.IsNullOrEmpty(lastDecision.ParsedDecision))
                    {
                        ctx.LastAITarget = ExtractTarget(lastDecision.ParsedDecision);
                    }
                }
            }
            
            // War status
            ctx.AtWar = unit.kingdom.hasEnemies();
            if (ctx.AtWar)
            {
                try {
                    var enemies = World.world.kingdoms.list.Where(k => k.isAlive() && unit.kingdom.isEnemy(k)).ToList();
                    if (enemies.Count > 0) ctx.EnemyName = enemies[rnd.Next(enemies.Count)].name;
                } catch {}
            }
            
            // Global war check
            try {
                foreach (var k1 in World.world.kingdoms.list) {
                    if (k1.isAlive() && k1.isCiv() && k1.hasEnemies()) {
                        ctx.GlobalWar = true;
                        break;
                    }
                }
            } catch {}
            
            return ctx;
        }
        
        private static string ExtractTarget(string json)
        {
            try {
                int idx = json.IndexOf("\"target\"");
                if (idx < 0) return null;
                int start = json.IndexOf(":", idx) + 1;
                int end = json.IndexOf(",", start);
                if (end < 0) end = json.IndexOf("}", start);
                if (start > 0 && end > start) {
                    return json.Substring(start, end - start).Trim().Trim('"');
                }
            } catch {}
            return null;
        }
    }
}
