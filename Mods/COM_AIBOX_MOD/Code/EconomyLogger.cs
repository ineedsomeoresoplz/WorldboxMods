using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using NCMS;

namespace AIBox
{
    public static class EconomyLogger
    {
        private static string logPath;
        private static bool initialized = false;

        public static void Init()
        {
            logPath = Path.Combine(Application.persistentDataPath, "economy_simulation_log.txt");
            File.WriteAllText(logPath, $"=== ECONOMY SIMULATION LOG ===\nStarted: {DateTime.Now}\n");
            initialized = true;
            Debug.Log($"[EconomyBox] Logger initialized at: {logPath}");
        }

        public static void LogVerbose(string message)
        {
            if (!initialized) Init();
            // Append to file immediately for debugging
            try {
                File.AppendAllText(logPath, $"[{DateTime.Now.ToString("HH:mm:ss")}] {message}\n");
            } catch {}
        }
        
        public static void Log(string message)
        {
             LogVerbose(message);
        }

        public static void LogTick(WorldDataManager manager)
        {
            if (!initialized) Init();
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\n\n================================ TICK {Time.frameCount} ================================");
            
            // -----------------------------------------------------------
            // 1. GLOBAL MARKET SNAPSHOT
            // -----------------------------------------------------------
            float globalVol = manager.GlobalTradeVolumeHistory.LastOrDefault();
            int activeKingdoms = manager.KingdomData.Count(k => k.Key.isAlive());
            int totalTrades = manager.TradeHistory.Count(t => t.Tick == Time.frameCount);
            
            sb.AppendLine("[TRADE TICKER]");
            var tickTrades = manager.TradeHistory.Where(t => t.Tick == Time.frameCount).ToList();
            if (tickTrades.Count > 0)
            {
                foreach (var t in tickTrades)
                {
                    // Format: [Seller] -> [Buyer] : [Resource] x[Amt] ($[Value])
                    string seller = (t.Seller != null) ? t.Seller.name : "Unknown";
                    string buyer = (t.Buyer != null) ? t.Buyer.name : "Unknown";
                    
                    // Detailed Forex Log
                    // t.Year = Gold Cost (Legacy)
                    // t.CostCoin = Buyer Coin Cost
                    // t.CoinID = Currency Name
                    // Format: Standardized Block
                    sb.AppendLine($"  [TRADE] {seller} -> {buyer}");
                    sb.AppendLine($"      Sold: {t.Amount} {t.ResourceId}");
                    sb.AppendLine($"      Paid: {t.CostGold:N0} Gold"); 
                    sb.AppendLine($"      Forex: {t.CostCoin:N1} {t.CoinID}");
                }
            }
            else
            {
                sb.AppendLine("  (No trades executed this tick)");
            }
            sb.AppendLine("--------------------------------------------------------------------------------");

            var sortedKingdoms = manager.KingdomData.OrderByDescending(k => k.Value.Wealth).ToList();
            foreach (var kvp in sortedKingdoms)
            {
                Kingdom k = kvp.Key;
                KingdomEconomyData d = kvp.Value;
                
                if (!k.isAlive()) continue;

                // --- 3.1 STATUS HEADER ---
                // Name | ID | Population | Cities | Capital
                string statusMsg = $"[KINGDOM] {k.name} (ID:{k.id}) | Pop: {k.getPopulationTotal()} | Cities: {k.cities.Count} | King: {(k.king != null ? k.king.getName() : "None")}";
                sb.AppendLine(statusMsg);

                // --- 3.2 DIPLOMATIC & MILITARY CONTEXT ---
                // Alliances, Wars, Enemies
                string warStatus = k.hasEnemies() ? "AT WAR" : "PEACE";
                string allianceId = (k.getAlliance() != null) ? k.getAlliance().id.ToString() : "None";
                int enemyCount = 0; 
                // Count enemies manually if needed or just use hasEnemies
                sb.AppendLine($"  > DIPLOMACY: {warStatus} | AllianceID: {allianceId} | Enemies: {(k.getEnemiesKingdoms().Count)}");

                // --- 3.3 STRATEGIC AI ---
                // Phase | Strategy | Monopoly Target
                string monopoly = d.IsMonopolyActive ? $"ACTIVE ({d.MonopolyResource})" : "None";
                sb.AppendLine($"  > STRATEGY: Phase: {d.CurrentPhase} | Target: {d.TargetResource} | Monopoly: {monopoly}");

                // --- 3.4 FINANCIALS ---
                // GDP | Wealth | Tax | Income | Expenses
                float wDiff = d.Wealth - d.OldWealth;
                string trend = wDiff > 0 ? "GROWTH" : "RECESSION";
                // Net Transfer (Change)
                sb.AppendLine($"  > FINANCIALS: GDP: {d.Wealth:N0} | Change: {wDiff:+0.0;-0.0} ({trend})");
                sb.AppendLine($"  > TREASURY: Liquid Gold: {d.GoldReserves:N0} | Tax Rate: {d.TaxRate:P0} | Credit Score: {d.CreditScore:F0}");

                // --- 3.5 CURRENCY ---
                // Name | Value | Supply | Stabilizer Cap Status
                string infRisk = d.CurrencyValue < 0.5f ? "HYPERINFLATION RISK" : "STABLE";
                sb.AppendLine($"  > CURRENCY: {d.CurrencyName} (ID:{d.CurrencyID}) | Val: {d.CurrencyValue:F2} | Supply: {d.CurrencySupply:N0} | {infRisk}");
                sb.AppendLine($"    (Timer: H-Inf={d.HyperinflationTimer} ticks)");

                // --- 3.6 DEBT & LOANS ---
                // Principal | Interest | Monthly Payment
                // List Active Loans where this kingdom is Borrowing
                var myLoans = manager.ActiveLoans.Where(l => l.BorrowerKingdomID == k.id.ToString()).ToList();
                sb.AppendLine($"  > DEBT LOAD: Total Loans: {myLoans.Count} | Total Principal: {d.NationalDebt:N0}");
                if(myLoans.Count > 0)
                {
                    foreach(var l in myLoans)
                    {
                        var lender = MapBox.instance.kingdoms.list.FirstOrDefault(lk => lk.id.ToString() == l.LenderKingdomID);
                        string lName = lender != null ? lender.name : "Unknown";
                        sb.AppendLine($"    - Loan #{l.CreationTick}: {l.RemainingAmount:N0} remaining to {lName} @ {l.InterestRate:P1} (Status: {l.LastPaymentStatus})");
                    }
                }

                // --- 3.7 RESOURCE HOLDINGS (AI Analysis) ---
                // Gold | Food | Gems | Mithril | Adamantine | Tea | Pie
                // Aggregated across all cities
                int rGold=0, rFood=0, rGems=0, rMithril=0, rAdamantine=0, rTea=0, rPie=0;
                foreach(var c in k.cities)
                {
                    rGold += c.getResourcesAmount("gold");
                    rFood += c.getResourcesAmount("food"); 
                    rGems += c.getResourcesAmount("gem");
                    rMithril += c.getResourcesAmount("mithril");
                    rAdamantine += c.getResourcesAmount("adamantine");
                    rTea += c.getResourcesAmount("tea");
                    rPie += c.getResourcesAmount("pie");
                }
                sb.AppendLine($"  > RESOURCES: Gold: {rGold} | Gems: {rGems} | Mithril: {rMithril} | Adamantine: {rAdamantine} | Tea: {rTea} | Pie: {rPie}");
                
                sb.AppendLine(new string('-', 40));
            }
            sb.AppendLine("--------------------------------------------------------------------------------");

            sb.AppendLine("[FORBES TOP 5]");
            var richest = manager.UnitData
                .Where(u => u.Key != null && u.Key.isAlive())
                .OrderByDescending(u => u.Value.PersonalWealth)
                .Take(5)
                .ToList();

            if (richest.Count > 0)
            {
                int rank = 1;
                foreach (var kvp in richest)
                {
                    Actor a = kvp.Key;
                    UnitEconomyData ud = kvp.Value;
                    string job = (a.city != null && a.city.leader == a) ? "Leader" : (a.kingdom.king == a) ? "KING" : "Citizen";
                    
                    // Trait Scan for Economic relevant traits
                    string traits = "";
                    if(a.hasTrait("Trader")) traits += "[Trader]";
                    if(a.hasTrait("miner")) traits += "[Miner]";
                    if(a.hasTrait("greedy")) traits += "[Greedy]";
                    if(a.hasTrait("genius")) traits += "[Genius]";
                    if(a.hasTrait("lucky")) traits += "[Lucky]";
                    
                    sb.AppendLine($"  #{rank} {a.getName(),-10} ({a.kingdom.name}) [{job}] Lvl:{a.stats["level"]} Age:{a.getAge()} {traits}");
                    sb.AppendLine($"      Net Worth: ${ud.PersonalWealth:N0} | Income Potential: High"); 
                    rank++;
                }
            }
            sb.AppendLine("==================================================================================");

            // Write File
            try
            {
                File.AppendAllText(logPath, sb.ToString());
            }
            catch {}
        }
    }
}

