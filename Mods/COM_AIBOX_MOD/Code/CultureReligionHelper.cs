using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;

namespace AIBox
{
    /// Helper class for extracting culture, religion, and subspecies data from kingdoms
    /// and executing related AI actions.
    public static class CultureReligionHelper
    {
        // ===================================================================
        // CULTURE EXTRACTION
        // ===================================================================
        
        /// Get the dominant culture of a kingdom.
        public static (string name, List<string> traits, int followers) GetKingdomCulture(Kingdom k)
        {
            if (k == null || k.cities == null || k.cities.Count == 0)
                return ("None", new List<string>(), 0);
            
            try
            {
                // Count culture followers
                Dictionary<string, int> cultureCounts = new Dictionary<string, int>();
                Dictionary<string, object> cultureObjects = new Dictionary<string, object>();
                
                foreach (City city in k.cities)
                {
                    foreach (Actor unit in city.units)
                    {
                        if (unit == null || !unit.isAlive()) continue;
                        
                        try
                        {
                            // Access culture via reflection since it's a public field
                            var culture = unit.culture;
                            if (culture != null)
                            {
                                string cultureName = culture.name ?? "Unknown";
                                if (!cultureCounts.ContainsKey(cultureName))
                                {
                                    cultureCounts[cultureName] = 0;
                                    cultureObjects[cultureName] = culture;
                                }
                                cultureCounts[cultureName]++;
                            }
                        }
                        catch { }
                    }
                }
                
                if (cultureCounts.Count == 0)
                    return ("None", new List<string>(), 0);
                
                // Find dominant culture
                var dominant = cultureCounts.OrderByDescending(x => x.Value).First();
                
                // Extract traits from culture object
                List<string> traits = new List<string>();
                if (cultureObjects.ContainsKey(dominant.Key))
                {
                    try
                    {
                        var culture = cultureObjects[dominant.Key];
                        // Try to get traits via reflection
                        var traitsField = Reflection.GetField(culture.GetType(), culture, "traits");
                        if (traitsField != null && traitsField is IEnumerable<object> traitsList)
                        {
                            foreach (var trait in traitsList)
                            {
                                try
                                {
                                    var traitId = Reflection.GetField(trait.GetType(), trait, "id");
                                    if (traitId != null) traits.Add(traitId.ToString());
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                
                return (dominant.Key, traits, dominant.Value);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AIBox] Error getting kingdom culture: {ex.Message}");
                return ("Unknown", new List<string>(), 0);
            }
        }
        
        // ===================================================================
        // RELIGION EXTRACTION
        // ===================================================================
        
        /// Get the dominant religion of a kingdom.
        public static (string name, List<string> traits, int followers) GetKingdomReligion(Kingdom k)
        {
            if (k == null || k.cities == null || k.cities.Count == 0)
                return ("None", new List<string>(), 0);
            
            try
            {
                // Count religion followers
                Dictionary<string, int> religionCounts = new Dictionary<string, int>();
                Dictionary<string, object> religionObjects = new Dictionary<string, object>();
                
                foreach (City city in k.cities)
                {
                    foreach (Actor unit in city.units)
                    {
                        if (unit == null || !unit.isAlive()) continue;
                        
                        try
                        {
                            var religion = unit.religion;
                            if (religion != null)
                            {
                                string religionName = religion.name ?? "Unknown";
                                if (!religionCounts.ContainsKey(religionName))
                                {
                                    religionCounts[religionName] = 0;
                                    religionObjects[religionName] = religion;
                                }
                                religionCounts[religionName]++;
                            }
                        }
                        catch { }
                    }
                }
                
                if (religionCounts.Count == 0)
                    return ("None", new List<string>(), 0);
                
                // Find dominant religion
                var dominant = religionCounts.OrderByDescending(x => x.Value).First();
                
                // Extract traits from religion object
                List<string> traits = new List<string>();
                if (religionObjects.ContainsKey(dominant.Key))
                {
                    try
                    {
                        var religion = religionObjects[dominant.Key];
                        var traitsField = Reflection.GetField(religion.GetType(), religion, "traits");
                        if (traitsField != null && traitsField is IEnumerable<object> traitsList)
                        {
                            foreach (var trait in traitsList)
                            {
                                try
                                {
                                    var traitId = Reflection.GetField(trait.GetType(), trait, "id");
                                    if (traitId != null) traits.Add(traitId.ToString());
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                
                return (dominant.Key, traits, dominant.Value);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AIBox] Error getting kingdom religion: {ex.Message}");
                return ("Unknown", new List<string>(), 0);
            }
        }
        
        // ===================================================================
        // DEMOGRAPHICS / SUBSPECIES EXTRACTION
        // ===================================================================
        
        /// Get demographic breakdown of a kingdom (subspecies/race percentages).
        public static Dictionary<string, float> GetDemographics(Kingdom k)
        {
            Dictionary<string, float> demographics = new Dictionary<string, float>();
            
            if (k == null || k.cities == null || k.cities.Count == 0)
                return demographics;
            
            try
            {
                Dictionary<string, int> raceCounts = new Dictionary<string, int>();
                int totalUnits = 0;
                
                foreach (City city in k.cities)
                {
                    foreach (Actor unit in city.units)
                    {
                        if (unit == null || !unit.isAlive()) continue;
                        totalUnits++;
                        
                        try
                        {
                            // Try subspecies first
                            string raceName = "Unknown";
                            
                            if (unit.subspecies != null)
                            {
                                raceName = unit.subspecies.name ?? "Unknown";
                            }
                            else if (unit.asset != null)
                            {
                                // Fallback to actor asset race
                                string assetId = unit.asset.id.ToLower();
                                if (assetId.Contains("human")) raceName = "Humans";
                                else if (assetId.Contains("orc")) raceName = "Orcs";
                                else if (assetId.Contains("elf")) raceName = "Elves";
                                else if (assetId.Contains("dwarf")) raceName = "Dwarves";
                                else raceName = assetId.Replace("unit_", "").Replace("_", " ");
                            }
                            
                            if (!raceCounts.ContainsKey(raceName))
                                raceCounts[raceName] = 0;
                            raceCounts[raceName]++;
                        }
                        catch { }
                    }
                }
                
                // Convert to percentages
                if (totalUnits > 0)
                {
                    foreach (var kvp in raceCounts)
                    {
                        demographics[kvp.Key] = (float)kvp.Value / totalUnits * 100f;
                    }
                }
                
                return demographics;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AIBox] Error getting demographics: {ex.Message}");
                return demographics;
            }
        }
        
        /// Get religious minorities in a kingdom (religions with < 20% followers).
        public static List<(string name, float percentage)> GetReligiousMinorities(Kingdom k)
        {
            List<(string, float)> minorities = new List<(string, float)>();
            
            if (k == null || k.cities == null) return minorities;
            
            try
            {
                Dictionary<string, int> religionCounts = new Dictionary<string, int>();
                int totalReligious = 0;
                
                foreach (City city in k.cities)
                {
                    foreach (Actor unit in city.units)
                    {
                        if (unit == null || !unit.isAlive()) continue;
                        
                        try
                        {
                            if (unit.religion != null)
                            {
                                totalReligious++;
                                string relName = unit.religion.name ?? "Unknown";
                                if (!religionCounts.ContainsKey(relName))
                                    religionCounts[relName] = 0;
                                religionCounts[relName]++;
                            }
                        }
                        catch { }
                    }
                }
                
                if (totalReligious > 0)
                {
                    foreach (var kvp in religionCounts)
                    {
                        float pct = (float)kvp.Value / totalReligious * 100f;
                        if (pct < 20f)
                        {
                            minorities.Add((kvp.Key, pct));
                        }
                    }
                }
            }
            catch { }
            
            return minorities;
        }
        
        /// Calculate cultural tension level based on demographic diversity.
        public static string GetCulturalTensionLevel(Kingdom k)
        {
            try
            {
                var demographics = GetDemographics(k);
                
                if (demographics.Count <= 1) return "None";
                if (demographics.Count == 2)
                {
                    // Check if one is dominant (>70%)
                    float maxPct = demographics.Values.Max();
                    if (maxPct > 70f) return "Low";
                    if (maxPct > 50f) return "Medium";
                    return "High";
                }
                
                // Multiple races/subspecies
                float topPct = demographics.Values.Max();
                if (topPct > 80f) return "Low";
                if (topPct > 60f) return "Medium";
                return "High";
            }
            catch
            {
                return "Unknown";
            }
        }
        
        // ===================================================================
        // CULTURE ACTIONS - Execution
        // ===================================================================
        
        /// Execute a culture action for a kingdom.
        public static bool ExecuteCultureAction(Kingdom k, CultureAction action, KingdomEconomyData kData)
        {
            if (k == null || action == null || string.IsNullOrEmpty(action.type) || action.type == "None")
                return false;
            
            try
            {
                string targetCulture = action.target_culture ?? "";
                int affected = 0;
                
                switch (action.type)
                {
                    case "SpreadCulture":
                        // Assign state culture to units without culture
                        affected = SpreadStateCulture(k);
                        LogCultureAction(k, $"Spread state culture to {affected} units");
                        return true;
                        
                    case "SuppressCulture":
                        // Add negative traits to target culture units
                        affected = SuppressCulture(k, targetCulture);
                        LogCultureAction(k, $"Suppressed culture '{targetCulture}' - {affected} units affected");
                        return true;
                        
                    case "CulturalPurge":
                        // Execute units of target culture
                        affected = PurgeCulture(k, targetCulture);
                        LogCultureAction(k, $"Cultural purge against '{targetCulture}' - {affected} executed");
                        return true;
                        
                    case "AssimilateMinority":
                        // Peacefully integrate (add happiness, convert culture over time)
                        affected = AssimilateMinority(k, targetCulture);
                        LogCultureAction(k, $"Assimilating minority culture '{targetCulture}' - {affected} units converting");
                        return true;
                        
                    default:
                        Debug.LogWarning($"[AIBox] Unknown culture action: {action.type}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Error executing culture action: {ex.Message}");
                return false;
            }
        }
        
        private static int SpreadStateCulture(Kingdom k)
        {
            int count = 0;
            var (stateCulture, _, _) = GetKingdomCulture(k);
            
            // Find a unit with the state culture to use as reference
            object stateCultureObj = null;
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit?.culture?.name == stateCulture)
                    {
                        stateCultureObj = unit.culture;
                        break;
                    }
                }
                if (stateCultureObj != null) break;
            }
            
            if (stateCultureObj == null) return 0;
            
            // Spread to units without culture
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.culture == null)
                    {
                        try
                        {
                            // Assign culture via reflection
                            Reflection.SetField(unit, "culture", stateCultureObj);
                            count++;
                        }
                        catch { }
                    }
                }
            }
            
            return count;
        }
        
        private static int SuppressCulture(Kingdom k, string targetCulture)
        {
            int count = 0;
            
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    
                    try
                    {
                        if (unit.culture?.name == targetCulture)
                        {
                            // Add oppression effects - decrease happiness
                            if (!unit.hasTrait("cursed"))
                            {
                                unit.addTrait("cursed");
                                count++;
                            }
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int PurgeCulture(Kingdom k, string targetCulture)
        {
            int count = 0;
            List<Actor> targets = new List<Actor>();
            List<Actor> soldiers = GetKingdomSoldiers(k);
            
            if (soldiers.Count == 0)
            {
                Debug.LogWarning("[AIBox] No soldiers available for cultural purge");
                return 0;
            }
            
            // Find targets of the target culture
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king || unit.is_profession_warrior) continue;
                    
                    try
                    {
                        if (unit.culture?.name == targetCulture)
                        {
                            targets.Add(unit);
                        }
                    }
                    catch { }
                }
            }
            
            // Assign soldiers to attack targets
            int soldiersPerTarget = Math.Max(1, soldiers.Count / Math.Max(1, targets.Count));
            for (int i = 0; i < targets.Count && i < soldiers.Count; i++)
            {
                try
                {
                    Actor soldier = soldiers[i % soldiers.Count];
                    Actor target = targets[i];
                    soldier.attack_target = target;
                    soldier.has_attack_target = true;
                    count++;
                }
                catch { }
            }
            
            return count;
        }
        
        private static int AssimilateMinority(Kingdom k, string targetCulture)
        {
            int count = 0;
            var (stateCulture, _, _) = GetKingdomCulture(k);
            
            // Find state culture object
            object stateCultureObj = null;
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit?.culture?.name == stateCulture)
                    {
                        stateCultureObj = unit.culture;
                        break;
                    }
                }
                if (stateCultureObj != null) break;
            }
            
            if (stateCultureObj == null) return 0;
            
            // Gradually convert minority culture (25% chance per unit)
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    
                    try
                    {
                        if (unit.culture?.name == targetCulture && UnityEngine.Random.value < 0.25f)
                        {
                            Reflection.SetField(unit, "culture", stateCultureObj);
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        // ===================================================================
        // RELIGION ACTIONS 
        // ===================================================================
        
        /// Execute a religion action for a kingdom.
        public static bool ExecuteReligionAction(Kingdom k, ReligionAction action, KingdomEconomyData kData)
        {
            if (k == null || action == null || string.IsNullOrEmpty(action.type) || action.type == "None")
                return false;
            
            try
            {
                string targetReligion = action.target_religion ?? "";
                int affected = 0;
                
                switch (action.type)
                {
                    case "EnforceReligion":
                        // Convert or exile non-state religion followers
                        affected = EnforceStateReligion(k);
                        LogReligionAction(k, $"Enforcing state religion - {affected} converted/exiled");
                        return true;
                        
                    case "BanReligion":
                        // Ban a specific religion (add negative traits)
                        affected = BanReligion(k, targetReligion);
                        LogReligionAction(k, $"Banned religion '{targetReligion}' - {affected} followers affected");
                        return true;
                        
                    case "ReligiousPersecution":
                        // Execute followers of target religion
                        affected = PersecuteReligion(k, targetReligion);
                        LogReligionAction(k, $"Religious persecution of '{targetReligion}' - {affected} executed");
                        return true;
                        
                    case "Tolerance":
                        // Remove persecution effects, boost happiness
                        affected = DeclareTolerance(k);
                        LogReligionAction(k, $"Declared religious tolerance - {affected} units blessed");
                        return true;
                        
                    default:
                        Debug.LogWarning($"[AIBox] Unknown religion action: {action.type}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Error executing religion action: {ex.Message}");
                return false;
            }
        }
        
        private static int EnforceStateReligion(Kingdom k)
        {
            int count = 0;
            var (stateReligion, _, _) = GetKingdomReligion(k);
            
            // Find state religion object
            object stateReligionObj = null;
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit?.religion?.name == stateReligion)
                    {
                        stateReligionObj = unit.religion;
                        break;
                    }
                }
                if (stateReligionObj != null) break;
            }
            
            if (stateReligionObj == null) return 0;
            
            // Find another kingdom to flee to
            Kingdom fleeKingdom = GetRandomOtherKingdom(k);
            
            foreach (City city in k.cities)
            {
                List<Actor> unitsToProcess = new List<Actor>(city.units);
                foreach (Actor unit in unitsToProcess)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king) continue;
                    
                    try
                    {
                        if (unit.religion != null && unit.religion.name != stateReligion)
                        {
                            // 30% flee to another kingdom, 70% stay (but marked)
                            if (UnityEngine.Random.value < 0.30f && fleeKingdom != null)
                            {
                                // Flee to another kingdom
                                unit.joinKingdom(fleeKingdom);
                            }
                            else
                            {
                                // Convert to state religion
                                Reflection.SetField(unit, "religion", stateReligionObj);
                            }
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int BanReligion(Kingdom k, string targetReligion)
        {
            int count = 0;
            
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    
                    try
                    {
                        if (unit.religion?.name == targetReligion)
                        {
                            // Mark as persecuted
                            if (!unit.hasTrait("cursed"))
                            {
                                unit.addTrait("cursed");
                            }
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int PersecuteReligion(Kingdom k, string targetReligion)
        {
            int count = 0;
            List<Actor> targets = new List<Actor>();
            List<Actor> soldiers = GetKingdomSoldiers(k);
            
            if (soldiers.Count == 0)
            {
                Debug.LogWarning("[AIBox] No soldiers available for religious persecution");
                return 0;
            }
            
            // Find targets of the target religion
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king || unit.is_profession_warrior) continue;
                    
                    try
                    {
                        if (unit.religion?.name == targetReligion)
                        {
                            targets.Add(unit);
                        }
                    }
                    catch { }
                }
            }
            
            // Assign soldiers to attack targets
            for (int i = 0; i < targets.Count && i < soldiers.Count; i++)
            {
                try
                {
                    Actor soldier = soldiers[i % soldiers.Count];
                    Actor target = targets[i];
                    soldier.attack_target = target;
                    soldier.has_attack_target = true;
                    count++;
                }
                catch { }
            }
            
            return count;
        }
        
        private static int DeclareTolerance(Kingdom k)
        {
            int count = 0;
            
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    
                    try
                    {
                        // Only remove cursed trait (from previous persecution)
                        if (unit.hasTrait("cursed"))
                        {
                            unit.removeTrait("cursed");
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        // ===================================================================
        // DEMOGRAPHIC ACTIONS - Execution
        // ===================================================================
        
        /// Execute a demographic action for a kingdom.
        public static bool ExecuteDemographicAction(Kingdom k, DemographicAction action, KingdomEconomyData kData)
        {
            if (k == null || action == null || string.IsNullOrEmpty(action.type) || action.type == "None")
                return false;
            
            try
            {
                string targetRace = action.target_race ?? "";
                int affected = 0;
                
                switch (action.type)
                {
                    case "Segregate":
                        // Move minority race to specific cities (simulate via trait)
                        affected = SegregateRace(k, targetRace);
                        LogDemographicAction(k, $"Segregated '{targetRace}' population - {affected} affected");
                        return true;
                        
                    case "Integrate":
                        // Mix populations (remove segregation effects)
                        affected = IntegratePopulation(k);
                        LogDemographicAction(k, $"Integrated populations - {affected} units freed");
                        return true;
                        
                    case "Expel":
                        // Exile units of target race
                        affected = ExpelRace(k, targetRace);
                        LogDemographicAction(k, $"Expelled '{targetRace}' from kingdom - {affected} exiled");
                        return true;
                        
                    case "Purge":
                        // Execute units of target race (genocide)
                        affected = PurgeRace(k, targetRace);
                        LogDemographicAction(k, $"Purged '{targetRace}' population - {affected} killed");
                        return true;
                        
                    default:
                        Debug.LogWarning($"[AIBox] Unknown demographic action: {action.type}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Error executing demographic action: {ex.Message}");
                return false;
            }
        }
        
        private static int SegregateRace(Kingdom k, string targetRace)
        {
            int count = 0;
            
            // Find the smallest city to segregate minorities to
            City segregationCity = null;
            int minPop = int.MaxValue;
            foreach (City city in k.cities)
            {
                if (city.units.Count < minPop)
                {
                    minPop = city.units.Count;
                    segregationCity = city;
                }
            }
            
            if (segregationCity == null) return 0;
            
            foreach (City city in k.cities)
            {
                if (city == segregationCity) continue;
                
                List<Actor> unitsToMove = new List<Actor>();
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king || unit.isCityLeader()) continue;
                    
                    try
                    {
                        string unitRace = GetUnitRace(unit);
                        if (unitRace.ToLower().Contains(targetRace.ToLower()))
                        {
                            unitsToMove.Add(unit);
                        }
                    }
                    catch { }
                }
                
                // Move units to segregation city
                foreach (Actor unit in unitsToMove)
                {
                    try
                    {
                        city.eventUnitRemoved(unit);
                        segregationCity.eventUnitAdded(unit);
                        Reflection.SetField(unit, "city", segregationCity);
                        count++;
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int IntegratePopulation(Kingdom k)
        {
            int count = 0;
            
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    
                    try
                    {
                        // Remove segregation/persecution effects
                        if (unit.hasTrait("slowpoke"))
                        {
                            unit.removeTrait("slowpoke");
                            count++;
                        }
                        if (unit.hasTrait("cursed"))
                        {
                            unit.removeTrait("cursed");
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int ExpelRace(Kingdom k, string targetRace)
        {
            int count = 0;
            Kingdom exileKingdom = GetRandomOtherKingdom(k);
            
            if (exileKingdom == null)
            {
                Debug.LogWarning("[AIBox] No other kingdom to expel units to");
                return 0;
            }
            
            foreach (City city in k.cities)
            {
                List<Actor> unitsToExpel = new List<Actor>();
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king) continue;
                    
                    try
                    {
                        string unitRace = GetUnitRace(unit);
                        if (unitRace.ToLower().Contains(targetRace.ToLower()))
                        {
                            unitsToExpel.Add(unit);
                        }
                    }
                    catch { }
                }
                
                // Expel units to another kingdom
                foreach (Actor unit in unitsToExpel)
                {
                    try
                    {
                        unit.joinKingdom(exileKingdom);
                        count++;
                    }
                    catch { }
                }
            }
            
            return count;
        }
        
        private static int PurgeRace(Kingdom k, string targetRace)
        {
            int count = 0;
            List<Actor> targets = new List<Actor>();
            List<Actor> soldiers = GetKingdomSoldiers(k);
            
            if (soldiers.Count == 0)
            {
                Debug.LogWarning("[AIBox] No soldiers available for racial purge");
                return 0;
            }
            
            // Find targets of the target race
            foreach (City city in k.cities)
            {
                foreach (Actor unit in city.units)
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.is_profession_king || unit.is_profession_warrior) continue;
                    
                    try
                    {
                        string unitRace = GetUnitRace(unit);
                        if (unitRace.ToLower().Contains(targetRace.ToLower()))
                        {
                            targets.Add(unit);
                        }
                    }
                    catch { }
                }
            }
            
            // Assign soldiers to attack targets
            for (int i = 0; i < targets.Count && i < soldiers.Count; i++)
            {
                try
                {
                    Actor soldier = soldiers[i % soldiers.Count];
                    Actor target = targets[i];
                    soldier.attack_target = target;
                    soldier.has_attack_target = true;
                    count++;
                }
                catch { }
            }
            
            return count;
        }
        
        private static string GetUnitRace(Actor unit)
        {
            if (unit == null) return "Unknown";
            
            try
            {
                // Try subspecies first
                if (unit.subspecies != null)
                {
                    return unit.subspecies.name ?? "Unknown";
                }
                
                // Fallback to asset
                if (unit.asset != null)
                {
                    string assetId = unit.asset.id.ToLower();
                    if (assetId.Contains("human")) return "human";
                    if (assetId.Contains("orc")) return "orc";
                    if (assetId.Contains("elf")) return "elf";
                    if (assetId.Contains("dwarf")) return "dwarf";
                    return assetId.Replace("unit_", "");
                }
            }
            catch { }
            
            return "Unknown";
        }
        
        // ===================================================================
        // HELPER METHODS
        // ===================================================================
        
        /// Get all soldiers/warriors from a kingdom.
        private static List<Actor> GetKingdomSoldiers(Kingdom k)
        {
            List<Actor> soldiers = new List<Actor>();
            
            if (k == null) return soldiers;
            
            try
            {
                foreach (Actor unit in k.getUnits())
                {
                    if (unit == null || !unit.isAlive()) continue;
                    if (unit.isKing() || unit.isCityLeader()) continue;
                    
                    // Check if unit is a warrior
                    if (unit._profession == UnitProfession.Warrior)
                    {
                        soldiers.Add(unit);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AIBox] Error getting soldiers: {ex.Message}");
            }
            
            return soldiers;
        }
        
        /// Get a random other kingdom for exile/expulsion purposes.
        private static Kingdom GetRandomOtherKingdom(Kingdom excludeKingdom)
        {
            try
            {
                var allKingdoms = World.world.kingdoms.list
                    .Where(xk => xk != null && xk.isAlive() && xk.isCiv() && xk != excludeKingdom)
                    .ToList();
                
                if (allKingdoms.Count == 0) return null;
                
                return allKingdoms[UnityEngine.Random.Range(0, allKingdoms.Count)];
            }
            catch
            {
                return null;
            }
        }
        
        // ===================================================================
        // LOGGING
        // ===================================================================
        private static void LogCultureAction(Kingdom k, string message)
        {
            string logEntry = $"[CULTURE] {k.name}: {message}";
            Debug.Log($"[AIBox] {logEntry}");
            
            // Add to thinking history for AI data viewer
            try
            {
                var kData = WorldDataManager.Instance.GetKingdomData(k);
                if (kData != null)
                {
                    kData.RecentDiplomaticEvents.Add(logEntry);
                    if (kData.RecentDiplomaticEvents.Count > 10)
                        kData.RecentDiplomaticEvents.RemoveAt(0);
                }
            }
            catch { }
        }
        
        private static void LogReligionAction(Kingdom k, string message)
        {
            string logEntry = $"[RELIGION] {k.name}: {message}";
            Debug.Log($"[AIBox] {logEntry}");
            
            try
            {
                var kData = WorldDataManager.Instance.GetKingdomData(k);
                if (kData != null)
                {
                    kData.RecentDiplomaticEvents.Add(logEntry);
                    if (kData.RecentDiplomaticEvents.Count > 10)
                        kData.RecentDiplomaticEvents.RemoveAt(0);
                }
            }
            catch { }
        }
        
        private static void LogDemographicAction(Kingdom k, string message)
        {
            string logEntry = $"[DEMOGRAPHIC] {k.name}: {message}";
            Debug.Log($"[AIBox] {logEntry}");
            
            try
            {
                var kData = WorldDataManager.Instance.GetKingdomData(k);
                if (kData != null)
                {
                    kData.RecentDiplomaticEvents.Add(logEntry);
                    if (kData.RecentDiplomaticEvents.Count > 10)
                        kData.RecentDiplomaticEvents.RemoveAt(0);
                }
            }
            catch { }
        }
    }
}
