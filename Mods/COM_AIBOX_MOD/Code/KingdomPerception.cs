using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NCMS;
using ReflectionUtility;

namespace AIBox
{
    public static class KingdomPerception
    {
        public static string GetStateSnapshot(Kingdom k, KingdomEconomyData data)
        {
            StringBuilder sb = new StringBuilder();
            
            // 0. KING PERSONALITY & RACE
            string kingName = "The Crown";
            string kingTraits = "None";
            string race = "Unknown";
            try {
                Actor king = (Actor)Reflection.GetField(k.GetType(), k, "king");
                if(king != null) {
                    kingName = king.getName();
                    
                    // Detect race from asset ID
                    string assetId = king.asset.id.ToLower();
                    if (assetId.Contains("human")) race = "Human";
                    else if (assetId.Contains("orc")) race = "Orc";
                    else if (assetId.Contains("elf")) race = "Elf";
                    else if (assetId.Contains("dwarf")) race = "Dwarf";
                    else race = assetId.Replace("unit_", "").ToUpper();
                    
                    try {
                        // Try to get localized trait string
                        string traitStr = (string)Reflection.CallMethod(king, "getTraitsAsLocalizedString");
                        if(!string.IsNullOrEmpty(traitStr)) kingTraits = traitStr;
                    } catch {
                       // Fallback
                       if(king.data != null && king.data.saved_traits != null) 
                           kingTraits = $"{king.data.saved_traits} traits";
                    }
                }
            } catch {}
            
            sb.AppendLine($"--- MY KINGDOM ---");
            sb.AppendLine($"I am King {kingName}, a {race} sovereign.");
            sb.AppendLine($"My nature is defined by: [{kingTraits}]");
            sb.AppendLine();

            // 1. BASICS WITH NARRATIVE LABELS
            int year = 0;
            try {
                    year = (int)World.world.getCurWorldTime();
            } catch { }

            int armyVal = 0;
            try {
                 try { armyVal = (int)Reflection.CallMethod(k, "getArmy"); } 
                 catch { 
                    object armyObj = Reflection.GetField(k.GetType(), k, "army"); 
                    if(armyObj != null) armyVal = (int)Reflection.CallMethod(armyObj, "Count"); 
                 }
            } catch {}

            // Narrative labels for numbers - DYNAMIC based on global averages
            float avgGold = WorldDataManager.Instance.GlobalAverageGold;
            float avgWealth = WorldDataManager.Instance.GlobalAverageWealth;
            float avgArmy = WorldDataManager.Instance.GlobalAverageArmy;
            
            string goldLabel = data.GoldReserves < avgGold * 0.2f ? "[CRISIS]" :
                               data.GoldReserves < avgGold * 0.5f ? "[Low]" :
                               data.GoldReserves < avgGold * 1.5f ? "[Adequate]" :
                               data.GoldReserves < avgGold * 3f ? "[Wealthy]" : "[Overflowing!]";
            
            string wealthLabel = data.Wealth < avgWealth * 0.2f ? "[Impoverished]" :
                                 data.Wealth < avgWealth * 0.5f ? "[Struggling]" :
                                 data.Wealth < avgWealth * 1.0f ? "[Growing]" :
                                 data.Wealth < avgWealth * 2f ? "[Prosperous]" : "[Superpower!]";
            
            string debtLabel = data.NationalDebt <= 0 ? "[Debt-free]" :
                               data.NationalDebt < data.Wealth * 0.3f ? "[Manageable]" :
                               data.NationalDebt < data.Wealth * 0.6f ? "[Heavy]" :
                               data.NationalDebt < data.Wealth ? "[CRUSHING]" : "[BANKRUPTCY!]";
            
            string armyLabel = armyVal < avgArmy * 0.3f ? "[Weak]" :
                               armyVal < avgArmy * 0.7f ? "[Small]" :
                               armyVal < avgArmy * 1.3f ? "[Average]" :
                               armyVal < avgArmy * 2f ? "[Strong]" : "[MIGHTY!]";

            sb.AppendLine($"[YEAR {year}]");
            sb.AppendLine($"REALM: Pop:{k.getPopulationTotal()} | Cities:{k.cities.Count} | Army:{armyVal} {armyLabel}");
            sb.AppendLine($"TREASURY: Gold:{data.GoldReserves:F0} {goldLabel}");
            sb.AppendLine($"ECONOMY: GDP:{data.Wealth:F0} {wealthLabel} | Debt:{data.NationalDebt:F0} {debtLabel}");
            sb.AppendLine($"POLICIES: Tax:{data.TaxRate:P0} | Currency:{data.CurrencyValue:F2} ({data.CurrencyName})");

            // CULTURE & RELIGION CONTEXT
            try
            {
                var (cultureName, cultureTraits, cultureFollowers) = CultureReligionHelper.GetKingdomCulture(k);
                var (religionName, religionTraits, religionFollowers) = CultureReligionHelper.GetKingdomReligion(k);
                var demographics = CultureReligionHelper.GetDemographics(k);
                var religiousMinorities = CultureReligionHelper.GetReligiousMinorities(k);
                string tensionLevel = CultureReligionHelper.GetCulturalTensionLevel(k);
                
                sb.AppendLine();
                sb.AppendLine("CULTURE & RELIGION:");
                sb.AppendLine($"  State Culture: {cultureName}" + (cultureTraits.Count > 0 ? $" (Traits: {string.Join(", ", cultureTraits.Take(3))})" : ""));
                sb.AppendLine($"  State Religion: {religionName}" + (religionTraits.Count > 0 ? $" (Traits: {string.Join(", ", religionTraits.Take(3))})" : ""));
                
                // Demographics breakdown
                if (demographics.Count > 0)
                {
                    var demoStr = string.Join(", ", demographics.OrderByDescending(x => x.Value).Take(4).Select(x => $"{x.Key} {x.Value:F0}%"));
                    sb.AppendLine($"  Demographics: {demoStr}");
                }
                
                // Religious minorities
                if (religiousMinorities.Count > 0)
                {
                    var minStr = string.Join(", ", religiousMinorities.Select(x => $"{x.name} ({x.percentage:F0}%)"));
                    sb.AppendLine($"  Religious Minorities: {minStr}");
                }
                
                // Cultural tensions
                if (tensionLevel != "None" && tensionLevel != "Unknown")
                {
                    sb.AppendLine($"  Cultural Tensions: [{tensionLevel}]");
                }

                // Global Lists for AI Targeting (Reflection Safe)
                List<string> knownCultures = new List<string>();
                if (World.world.cultures != null && World.world.cultures.list != null)
                {
                    foreach (var c in World.world.cultures.list)
                    {
                        if (c == null) continue;
                        // Default to inclusion if we can't read followers, to ensure AI sees OPTIONS
                        int followers = 1; 
                        try { followers = (int)Reflection.CallMethod(c, "getFollowersAmount"); } catch { }
                        
                        // Only add if it has name
                        if (!string.IsNullOrEmpty(c.name) && followers > 0) knownCultures.Add(c.name);
                    }
                }

                List<string> knownReligions = new List<string>();
                if (World.world.religions != null && World.world.religions.list != null)
                {
                    foreach (var r in World.world.religions.list)
                    {
                        if (r == null) continue;
                        int followers = 1;
                        try { followers = (int)Reflection.CallMethod(r, "getFollowersAmount"); } catch { }
                        
                        if (!string.IsNullOrEmpty(r.name) && followers > 0) knownReligions.Add(r.name);
                    }
                }

                if (knownCultures.Count > 0) sb.AppendLine($"  KNOWN CULTURES: {string.Join(", ", knownCultures)}");
                if (knownReligions.Count > 0) sb.AppendLine($"  KNOWN RELIGIONS: {string.Join(", ", knownReligions)}");
            }
            catch { }

            // 2. TRENDS
            sb.AppendLine($"[System v2 Active]"); // DEBUG TAG

            // 2. TRENDS - DYNAMIC thresholds
            if(data.WealthHistory != null && data.WealthHistory.Count > 1) {
                float prev = data.WealthHistory[data.WealthHistory.Count - 2];
                float change = data.Wealth - prev;
                float significantChange = avgWealth * 0.05f; // 5% of average is significant
                string sign = change >= 0 ? "+" : "";
                string trendLabel = change > significantChange ? "[BOOM]" : change > 0 ? "[Growing]" : change > -significantChange ? "[Stagnant]" : "[RECESSION]";
                sb.AppendLine($"TREND: GDP {sign}{change:F0} {trendLabel}");
            }

            // MODERBOX CAPABILITY
            if(ModerBoxHelper.IsInstalled) {
                bool canNuke = ModerBoxHelper.CanKingdomNuke(k);
                int missileUnits = ModerBoxHelper.CountMissileUnits(k);
                string era = ModerBoxHelper.IsModernEra(k) ? "Modern" : "Pre-Modern";
                string nukeStatus = canNuke ? "READY" : "NOT READY";
                if(missileUnits == 0) nukeStatus += " (No Missiles)";
                else if(data.GoldReserves < ModerBoxHelper.NUKE_COST) nukeStatus += $" (Need {ModerBoxHelper.NUKE_COST}g)";
                
                sb.AppendLine($"MILITARY TECH: Era:{era} | Missiles:{missileUnits} | NUCLEAR CAPABILITY: {nukeStatus}");
            }

            // 3. RESOURCES - COMPACT (key resources only) - DYNAMIC thresholds
            var keyResources = new string[] { "gold", "iron", "wood", "wheat", "mithril", "adamantine" };
            List<string> have = new List<string>();
            List<string> need = new List<string>();
            List<string> surplus = new List<string>();
            
            float avgResourceThreshold = Mathf.Max(100f, avgGold * 0.5f); // Dynamic based on global gold
            float surplusThreshold = avgResourceThreshold * 5f;
            float needThreshold = avgResourceThreshold * 0.5f;
            
            foreach(City c in k.cities) {
                foreach(string res in keyResources) {
                    int amt = c.getResourcesAmount(res);
                    if(res == "gold") have.Add($"gold:{amt}");
                    else if(amt > surplusThreshold) surplus.Add(res);
                    else if(amt < needThreshold) need.Add(res);
                }
            }
            
            string resourceLine = $"RESOURCES: {string.Join(", ", have)}";
            if(need.Count > 0) resourceLine += $" | NEED: {string.Join(",", need.Distinct())}";
            if(surplus.Count > 0) resourceLine += $" | SURPLUS: {string.Join(",", surplus.Distinct().Take(3))}";
            sb.AppendLine(resourceLine);
            
            // VASSALS INFO
            if(!string.IsNullOrEmpty(data.VassalLord)) {
                sb.AppendLine($"STATUS: VASSAL of {data.VassalLord} (Tribute: {data.TributeRate:P0})");
            }
            if(data.Vassals != null && data.Vassals.Count > 0) {
                sb.AppendLine($"MY VASSALS: {string.Join(", ", data.Vassals)}");
            }

            // 4. DIPLOMACY - SMART SELECTION (max 11 kingdoms for cost efficiency)
            sb.AppendLine("\nDIPLOMACY & FOREIGN RELATIONS:");
            
            // Collect and categorize all kingdoms
            List<Kingdom> allKingdoms = World.world.kingdoms.list.Where(o => o != k && o.isAlive() && o.isCiv()).ToList();
            List<Kingdom> enemies = new List<Kingdom>();
            List<Kingdom> allies = new List<Kingdom>();
            List<(Kingdom k, int score, int army, float distance)> neutrals = new List<(Kingdom, int, int, float)>();
            
            int myArmy = 0;
            try { myArmy = k.countTotalWarriors(); } catch { try { myArmy = k.cities.Sum(c => c.units.Count); } catch {} }
            
            foreach(Kingdom other in allKingdoms) {
                bool isEnemy = k.isEnemy(other);
                bool isAllied = false;
                try {
                    var myAlliance = k.getAlliance();
                    var theirAlliance = other.getAlliance();
                    isAllied = myAlliance != null && myAlliance == theirAlliance;
                } catch {}
                
                if(isEnemy) { enemies.Add(other); }
                else if(isAllied) { allies.Add(other); }
                else {
                    // Calculate priority score for neutrals
                    int opinion = 0;
                    try { 
                        var opinionObj = World.world.diplomacy.getOpinion(k, other);
                        if(opinionObj != null) {
                            // Use .total property for the opinion score
                            try { opinion = (int)Reflection.GetField(opinionObj.GetType(), opinionObj, "total"); }
                            catch { 
                                // Fallback: try direct property access
                                try { opinion = opinionObj.total; } catch {}
                            }
                        }
                    } catch {}
                    int theirArmy = 0;
                    try { theirArmy = other.countTotalWarriors(); } catch { try { theirArmy = other.cities.Sum(c => c.units.Count); } catch {} }
                    float distance = 999f;
                    try { distance = (float)Kingdom.distanceBetweenKingdom(k, other); } catch {}
                    
                    neutrals.Add((other, opinion, theirArmy, distance));
                }
            }

            // EXPLICIT WAR LIST FOR AI CONTEXT
            if(enemies.Count > 0) {
                 string enemyNames = string.Join(", ", enemies.Select(e => e.name));
                 sb.AppendLine($"[CRITICAL] AT WAR WITH: [{enemyNames}] (Do NOT declare War again. Use Peace to end it.)");
            } else {
                 sb.AppendLine("[CRITICAL] AT WAR WITH: [None] (You are at peace).");
            }
            // Sort neutrals: threats (high army, negative opinion), neighbors (low distance), trade partners (positive opinion)
            // DYNAMIC thresholds based on global averages
            var threats = neutrals.Where(n => n.army > myArmy || n.score < -(int)(avgArmy * 0.2f)).OrderByDescending(n => n.army).Take(3).Select(n => n.k).ToList();
            var neighbors = neutrals.Where(n => !threats.Contains(n.k)).OrderBy(n => n.distance).Take(2).Select(n => n.k).ToList();
            var tradePartners = neutrals.Where(n => !threats.Contains(n.k) && !neighbors.Contains(n.k) && n.score >= 0).OrderByDescending(n => n.score).Take(3).Select(n => n.k).ToList();
            
            // Combine selected kingdoms
            HashSet<Kingdom> selectedKingdoms = new HashSet<Kingdom>();
            foreach(var e in enemies) selectedKingdoms.Add(e);
            foreach(var a in allies) selectedKingdoms.Add(a);
            foreach(var t in threats) selectedKingdoms.Add(t);
            foreach(var n in neighbors) selectedKingdoms.Add(n);
            foreach(var p in tradePartners) selectedKingdoms.Add(p);
            
            // Output selected kingdoms
            foreach(Kingdom other in selectedKingdoms) {
                int score = 0;
                try { 
                    var opinionObj = World.world.diplomacy.getOpinion(k, other);
                    if(opinionObj != null) {
                        // Use .total property for the opinion score
                        try { score = (int)Reflection.GetField(opinionObj.GetType(), opinionObj, "total"); }
                        catch { 
                            // Fallback: try direct property access
                            try { score = opinionObj.total; } catch {}
                        }
                    }
                } catch {}
                
                string dipStatus = enemies.Contains(other) ? "WAR" : allies.Contains(other) ? "ALLIED" : "Neutral";
                if(data.EmbargoList.Contains(other.name)) dipStatus += " (EMBARGO)";
                var oData = WorldDataManager.Instance.GetKingdomData(other);
                if(oData != null && oData.CurrencyID == data.CurrencyID) dipStatus += " (UNION)";

                int theirArmy = 0;
                try { theirArmy = other.countTotalWarriors(); } catch {}
                string milStatus = theirArmy < myArmy * 0.5f ? "[WEAK]" : theirArmy > myArmy * 1.5f ? "[THREAT]" : "[PEER]";
                
                // Tag why this kingdom is shown (text tags instead of emoji for AI compatibility)
                string tag = "";
                if(enemies.Contains(other)) tag = "[WAR]";
                else if(allies.Contains(other)) tag = "[ALLY]";
                else if(threats.Contains(other)) tag = "[THREAT]";
                else if(neighbors.Contains(other)) tag = "[NEIGHBOR]";
                else tag = "[TRADE]"; // trade partner
                
                // Compact spy info
                string spyInfo = "";
                if(data.SpiedKingdoms.Contains(other.name) && Time.frameCount < data.SpyExpiry && oData != null) {
                    spyInfo = $" [SPY: {oData.GoldReserves:N0}g, {oData.CurrentPolicy}]";
                }

                sb.AppendLine($"{tag} {other.name}: {dipStatus} (Opinion:{score}) {milStatus} GDP:{oData?.Wealth:F0}{spyInfo}");
            }
            
            // VASSALAGE STATUS (compact)
            if(!string.IsNullOrEmpty(data.VassalLord)) {
                sb.AppendLine($"STATUS: VASSAL of {data.VassalLord}. Paying {data.TributeRate:P0} tribute daily.");
                sb.AppendLine("CONSTRAINT: You CANNOT declare war on your Lord.");
            } else if(data.Vassals.Count > 0) {
                sb.AppendLine($"VASSALS: {string.Join(", ", data.Vassals)}");
            }
            
            // Compact: Economy + Key Market Prices in one line
            sb.AppendLine($"\n[ECONOMY] System: {data.EconomicSystem} | Prices: wheat={GlobalCommerce.GetResourcePrice(k, "wheat"):F0}g, iron={GlobalCommerce.GetResourcePrice(k, "iron"):F0}g");

            // 5. GLOBAL INTELLIGENCE (Recent News & World State)
            sb.AppendLine("\nGLOBAL INTELLIGENCE (What others are doing):");
            
            // --- LIVE WARS ---
            bool globalWarFound = false;
            foreach(Kingdom k1 in World.world.kingdoms.list) {
                if(!k1.isAlive() || !k1.isCiv()) continue;
                foreach(Kingdom k2 in World.world.kingdoms.list) {
                    if(k1 == k2 || !k2.isAlive() || !k2.isCiv()) continue;
                    if(k1.isEnemy(k2) && k1.id < k2.id) {
                        sb.AppendLine($"- GLOBAL WAR: {k1.name} is at WAR with {k2.name}!");
                        globalWarFound = true;
                    }
                }
            }
            if(!globalWarFound) sb.AppendLine("- The world is currently at peace (no active wars).");
            
            // --- MY WAR STATUS (Detailed for decision-making) ---
            try {
                var myWars = World.world.wars.getWars(k);
                if(myWars != null && myWars.Any()) {
                    sb.AppendLine("\n⚔ MY ACTIVE WARS:");
                    foreach(var war in myWars) {
                        if(war == null || war.hasEnded()) continue;
                        
                        bool amAttacker = war.isAttacker(k);
                        int myDeaths = amAttacker ? war.getDeadAttackers() : war.getDeadDefenders();
                        int enemyDeaths = amAttacker ? war.getDeadDefenders() : war.getDeadAttackers();
                        int myCities = amAttacker ? war.countAttackersCities() : war.countDefendersCities();
                        int enemyCities = amAttacker ? war.countDefendersCities() : war.countAttackersCities();
                        int myWarriors = amAttacker ? war.countAttackersWarriors() : war.countDefendersWarriors();
                        int enemyWarriors = amAttacker ? war.countDefendersWarriors() : war.countAttackersWarriors();
                        
                        // Determine main enemy name
                        Kingdom mainEnemy = amAttacker ? war.main_defender : war.main_attacker;
                        string enemyName = mainEnemy?.name ?? "Unknown";
                        
                        // Calculate war score
                        int warScore = (enemyDeaths - myDeaths) + (myCities - enemyCities) * 10 + (myWarriors - enemyWarriors) / 5;
                        string warStatus = warScore > 50 ? "[WINNING DECISIVELY]" :
                                          warScore > 10 ? "[WINNING]" :
                                          warScore > -10 ? "[STALEMATE]" :
                                          warScore > -50 ? "[LOSING]" : "[LOSING BADLY]";
                        
                        string role = amAttacker ? "ATTACKER" : "DEFENDER";
                        sb.AppendLine($"  vs {enemyName} ({role}) {warStatus}");
                        sb.AppendLine($"    Casualties: We lost {myDeaths}, Enemy lost {enemyDeaths}");
                        sb.AppendLine($"    Territory: Our cities {myCities}, Enemy cities {enemyCities}");
                        sb.AppendLine($"    Army strength: Ours {myWarriors}, Theirs {enemyWarriors}");
                        
                        // Strategic hints based on war status
                        if(warScore > 30 && enemyCities <= 1) {
                            sb.AppendLine($"    → CONSIDER: InstallPuppet to make {enemyName} a vassal!");
                        } else if(warScore < -30) {
                            sb.AppendLine($"    → CONSIDER: Peace offer to end this costly war!");
                        }
                    }
                }
            } catch {}

            sb.AppendLine("\nRECENT MOD EVENTS:");
            float recentThreshold = Time.time - 60f;
            bool newsFound = false;

            if (WorldDataManager.Instance != null) {
                foreach (var kvp in WorldDataManager.Instance.KingdomData) {
                    Kingdom otherK = kvp.Key;
                    KingdomEconomyData otherData = kvp.Value;
                    
                    if (otherK == k || !otherK.isAlive()) continue;

                    foreach (var entry in otherData.ThinkingHistory) {
                        if (entry.Timestamp < recentThreshold) break;

                        string s = entry.ParsedDecision;
                        string otherKing = (otherK.king != null) ? otherK.king.getName() : "Regent";

                        if (s.Contains("PLANNING WAR")) {
                            sb.AppendLine($"- WARNING: King {otherKing} ({otherK.name}) is plotting war! Spy report: \"{entry.Reasoning}\"");
                            newsFound = true;
                        }
                        else if (s.Contains("PACT")) {
                             sb.AppendLine($"- NEWS: {otherK.name} is seeking alliances/pacts.");
                             newsFound = true;
                        }
                        else if (s.Contains("GIFT")) {
                             sb.AppendLine($"- NEWS: {otherK.name} is sending diplomatic gifts.");
                             newsFound = true;
                        }
                        else if (s.Contains("POLICY")) {
                             sb.AppendLine($"- INTEL: {otherK.name} shift policy: {s}");
                             newsFound = true;
                        }
                    }
                }
            }
            if(!newsFound) sb.AppendLine("- No major mod-level events reported.");

            // 6. PENDING OFFERS & MESSAGES
            sb.AppendLine("\nINBOX (Trade Offers & Diplomatic Messages):");
            if(data.PendingOffers.Count == 0 && data.RecentDiplomaticEvents.Count == 0) {
                sb.AppendLine("- No new messages.");
            } else {
                foreach(var offer in data.PendingOffers) {
                    if(offer.IsResponse) {
                        string result = offer.Accepted ? "ACCEPTED" : "REJECTED";
                        sb.AppendLine($"- [{offer.ID}] RESPONSE from {offer.FromKingdom}: {result}. Message: {offer.Message}");
                    } else {
                        sb.AppendLine($"- [{offer.ID}] OFFER from {offer.FromKingdom}: {offer.OfferAmount} {offer.OfferResource} for {offer.RequestAmount} {offer.RequestResource}. Message: {offer.Message}");
                    }
                }
                foreach(var evt in data.RecentDiplomaticEvents) {
                    sb.AppendLine($"- MSG: {evt}");
                }
            }
            
            // 6. INTERNAL
            sb.AppendLine("\nINTERNAL:");
            float avgLoyalty = 0;
            if(k.cities.Count > 0) {
                 foreach(City c in k.cities) {
                    avgLoyalty += c.getCachedLoyalty(); 
                 }
                 avgLoyalty /= k.cities.Count;
            }
            sb.AppendLine($"Loyalty Avg: {avgLoyalty:F0}%");

            // Happiness / Unrest Check
            int unhappyCount = 0;
            int totalCities = k.cities.Count;
            foreach(City c in k.cities) {
                try {
                    bool happy = (bool)Reflection.CallMethod(c, "isHappy");
                    if(!happy) unhappyCount++;
                } catch { }
            }
            sb.AppendLine($"Public Sentiment: {unhappyCount} Unhappy Cities / {totalCities} Total.");
            
            // Critical Loyalty Check
            List<string> criticalCities = new List<string>();
            foreach(City c in k.cities) {
                 int l = c.getCachedLoyalty();
                 if(l < 30) criticalCities.Add($"{c.name}({l}%)");
            }
            if(criticalCities.Count > 0) {
                sb.AppendLine($"CRITICAL LOYALTY WARNING: {string.Join(", ", criticalCities)}. CONSIDER FESTIVAL.");
            }
            float infraGold = 0;
            foreach(City c in k.cities) infraGold += c.getResourcesAmount("gold");
            sb.AppendLine($"City Gold: {infraGold:N0}");
            
            // 7. RECENT ACTIONS (What you already did - avoid repeating!)
            if(data.RecentActions != null && data.RecentActions.Count > 0) {
                sb.AppendLine("\nYOUR RECENT ACTIONS (Don't repeat - try NEW actions!):");
                int actionNum = data.RecentActions.Count;
                foreach(var action in data.RecentActions) {
                    sb.AppendLine($"  Turn -{actionNum}: {action}");
                    actionNum--;
                }
                sb.AppendLine("TIP: Vary your actions! If you did Alliance last turn, try War/Spy/Trade/Build this turn.");
            }

            return sb.ToString();
        }
    }
}


