using System;
using System.Collections.Generic;
using System.Linq;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace AIBox
{
    public class WorldDataManager : MonoBehaviour
    {
        public static WorldDataManager Instance;
        
        // Data Stores
        public Dictionary<Kingdom, KingdomEconomyData> KingdomData = new Dictionary<Kingdom, KingdomEconomyData>();
        public Dictionary<Actor, UnitEconomyData> UnitData = new Dictionary<Actor, UnitEconomyData>();
        
        // Data Logging
        public List<TradeEvent> TradeHistory = new List<TradeEvent>();
        public List<float> GlobalTradeVolumeHistory = new List<float>();
        public float CurrentTickTradeVolume = 0;
        public float GlobalAverageGold = 1000f; 
        public float GlobalAverageWealth = 1000f;
        public float GlobalAveragePop = 100f;
        public float GlobalAverageCurrencyValue = 1.0f;
        public float GlobalAverageArmy = 50f;
        
        // Active Loans
        public List<Loan> ActiveLoans = new List<Loan>();
        
        // Global Stock Market
        public Dictionary<string, float> ResourcePrices = new Dictionary<string, float>();
        public Dictionary<string, float> ResourceSupply = new Dictionary<string, float>();
        public static readonly string[] TRADABLE_RESOURCES = { "wheat", "wood", "stone", "iron", "gold", "mithril", "adamantine" };
        
        // AI Globals
        public Dictionary<string, bool> GlobalDivineLaws = new Dictionary<string, bool>();
        public List<ThinkingLogEntry> GlobalDecisionLog = new List<ThinkingLogEntry>();

        public void LogDecision(Kingdom k, string reasoning, string decision, string context = "", string parsedJson = "")
        {
            var entry = new ThinkingLogEntry {
                Timestamp = Time.frameCount,
                KingdomName = k.name,
                LeaderName = k.king != null ? k.king.getName() : "Council",
                Decision = decision,
                DecisionSummary = decision, // Populate both
                Reasoning = reasoning,
                InputContext = context,
                ParsedDecision = parsedJson,
                Year = Reflection.GetField(typeof(MapBox), MapBox.instance, "mapStats") is MapStats stats ? stats.year.ToString() : "0"
            };
            
            GlobalDecisionLog.Add(entry);
            if(GlobalDecisionLog.Count > 100) GlobalDecisionLog.RemoveAt(0);
            
            // Also add to Kingdom History
            var kData = GetKingdomData(k);
            if(kData != null) {
                kData.ThinkingHistory.Add(entry);
                if(kData.ThinkingHistory.Count > 50) kData.ThinkingHistory.RemoveAt(0);
            }
        }

        // Timer
        private float tickTimer = 1.0f; 
        private const float TICK_INTERVAL = 1.0f;

        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("WorldDataManager");
                Instance = go.AddComponent<WorldDataManager>();
                DontDestroyOnLoad(go);
            }
        }

        void Update()
        {
            if (MapBox.instance == null) return;
             if (Config.paused) return;
             if (Time.timeScale == 0) return;

            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                tickTimer = TICK_INTERVAL;
                UpdateCurrentAge();
                ProcessEconomyTick();
                
                string currentPower = "";
                try {
                     object pConfig = PlayerConfig.instance;
                     GodPower power = (GodPower)Reflection.GetField(pConfig.GetType(), pConfig, "selected_god_power");
                     if (power != null) currentPower = power.id;
                } catch {
                     try {
                         object pConfig = PlayerConfig.instance;
                         GodPower power = (GodPower)Reflection.GetField(pConfig.GetType(), pConfig, "god_power");
                         if (power != null) currentPower = power.id;
                     } catch {}
                }

                if (currentPower != "TradeEmbargoButton" && SimulationControls.embargoSource != null) SimulationControls.embargoSource = null;
                if (currentPower != "TakeLoanButton" && SimulationControls.loanSource != null) SimulationControls.loanSource = null;
            }
        } 

        private void UpdateGlobalStats()
        {
            if (KingdomData.Count == 0) return;
            
            float totalGold = 0;
            float totalWealth = 0;
            float totalPop = 0;
            float totalCurrencyVal = 0;
            float totalArmy = 0;
            int count = 0;

            foreach(var kvp in KingdomData)
            {
                if (kvp.Key != null && kvp.Key.isAlive())
                {
                    totalGold += kvp.Value.GoldReserves;
                    totalWealth += kvp.Value.Wealth;
                    totalPop += kvp.Key.getPopulationTotal();
                    totalCurrencyVal += kvp.Value.CurrencyValue;
                    try { totalArmy += kvp.Key.countTotalWarriors(); } catch { try { totalArmy += kvp.Key.cities.Sum(c => c.units.Count); } catch {} }
                    count++;
                }
            }
            
            if (count > 0)
            {
                GlobalAverageGold = totalGold / count;
                GlobalAverageWealth = totalWealth / count;
                GlobalAveragePop = totalPop / count;
                GlobalAverageCurrencyValue = totalCurrencyVal / count;
                GlobalAverageArmy = totalArmy / count;
            }
        }
        
        private void ProcessEconomyTick()
        {
            CleanupInvalidData();
            UpdateGlobalStats(); 
            
            // 1. Kingdom Economy
            SimulationGameloop.UpdateKingdoms(this);

            // 2. Unit Economy
            UnitEconomy.UpdateUnits(this);

            // 3. Market System
            GlobalCommerce.ProcessMarket(this);
            
            // 4. Update Global Resource Prices
            UpdateMarketPrices();
            
            // Process Loan Repayments
            ProcessLoanRepayments();

            // 4. Record History for Graphs
            foreach(var kvp in KingdomData) {
                var data = kvp.Value;
                data.WealthHistory.Add(data.Wealth);
                data.CurrencyHistory.Add(data.CurrencyValue);
                
                // CONDITIONAL LOGGING TRIGGERS (Replaces NewsTicker)
                if(data.CurrencyValue <= 0.01f && UnityEngine.Random.value < 0.05f) 
                {
                     EconomyLogger.LogVerbose($"HYPERINFLATION WARNING: {kvp.Key.name}");
                }
                
                if(data.NationalDebt > data.Wealth * 0.6f && UnityEngine.Random.value < 0.01f)
                {
                    EconomyLogger.LogVerbose($"DEBT TRAP WARNING: {kvp.Key.name} Debt Ratio {data.NationalDebt/data.Wealth:F2}");
                }
                
                // 7. AUTO-POLICIES
                SimulationGameloop.UpdatePolicies(kvp.Key, data);

                if(data.WealthHistory.Count > 60) data.WealthHistory.RemoveAt(0);
                if(data.CurrencyHistory.Count > 60) data.CurrencyHistory.RemoveAt(0);
            }

            // 5. Global Updates 
            GlobalTradeVolumeHistory.Add(CurrentTickTradeVolume);
            if(GlobalTradeVolumeHistory.Count > 60) GlobalTradeVolumeHistory.RemoveAt(0);
            CurrentTickTradeVolume = 0; 

            // 6. LOGGING
            EconomyLogger.LogTick(this);
            
            // 7. SYNC DEBT 
            foreach(var k in KingdomData.Values) k.NationalDebt = 0;
            
            foreach(var loan in ActiveLoans)
            {
                var borrower = MapBox.instance.kingdoms.list.FirstOrDefault(k => k.id.ToString() == loan.BorrowerKingdomID);
                if(borrower != null && KingdomData.TryGetValue(borrower, out var data))
                {
                    data.NationalDebt += (loan.Principal - loan.RepaidAmount);
                }
            }
        }
        
        public void AttemptLoanRequest(Kingdom borrower)
        {
            if(borrower == null) return;
            var borrowerData = GetKingdomData(borrower);
            
            if(borrower.getPopulationTotal() < 15) return;

            int activeLoansInfo = ActiveLoans.Count(l => l.BorrowerKingdomID == borrower.id.ToString()); 
            
            bool isInvestment = borrowerData.Wealth > 1000 && borrowerData.CreditScore > 80;
            
            if(activeLoansInfo >= 3 && !isInvestment) return; 
            if(activeLoansInfo >= 5) return; 

            float maxDebt = Mathf.Max(2000f, borrowerData.Wealth * 2.0f);
            if (borrowerData.NationalDebt >= maxDebt) return;

            float baseAmount = 500f + (borrower.cities.Count * 300f);
            
            float amountNeeded = 0;

            if (isInvestment)
            {
                amountNeeded = borrowerData.Wealth * UnityEngine.Random.Range(0.1f, 0.2f);
                if(amountNeeded < 1000) amountNeeded = 1000; 
            }
            else
            {
                float deficit = borrowerData.Wealth < 200 ? (200 - borrowerData.Wealth) : 0;
                amountNeeded = Mathf.Max(baseAmount, deficit + 500f); 

                bool isAtWar = borrower.hasEnemies();
                if(isAtWar)
                {
                    amountNeeded *= 1.5f; 
                } 
                
                amountNeeded += UnityEngine.Random.Range(0, 10) * 50f;
            }

            amountNeeded = Mathf.Min(amountNeeded, 20000f); 

            float remainingCap = maxDebt - borrowerData.NationalDebt;
            if (amountNeeded > remainingCap) amountNeeded = remainingCap;
            
            if (amountNeeded < 100f) return; 

            // Credit Score Based Interest
            float riskPremium = (100f - borrowerData.CreditScore) / 10f; 
            if (riskPremium < 0) riskPremium = 0;
            
            float marketFlux = UnityEngine.Random.Range(-0.005f, 0.01f);
            float finalRate = 0.05f + (riskPremium * 0.01f) + marketFlux;

            if(borrowerData.CurrencyValue < 0.4f)
            {
                amountNeeded *= 2.5f; 
                finalRate += 0.15f; 
            }

            if(finalRate < 0.01f) finalRate = 0.01f; 

            Kingdom lender = null;
            float bestWealth = 0;

            foreach(var k in MapBox.instance.kingdoms.list)
            {
                if(k == borrower) continue;
                if(k.isEnemy(borrower)) continue; 

                var kData = GetKingdomData(k);
                if(kData.Wealth > (amountNeeded * 1.5f) && kData.Wealth > bestWealth)
                {
                    bestWealth = kData.Wealth;
                    lender = k;
                }
            }

            if(lender != null)
            {
                CreateLoan(lender, borrower, amountNeeded, finalRate);
                EconomyLogger.LogVerbose($"LOAN: {borrower.name} borrowed {amountNeeded:F0} from {lender.name} at {finalRate:P0}");
            }
        }

        public void CreateLoan(Kingdom lender, Kingdom borrower, float principal, float interestRate)
        {
            int index = ActiveLoans.FindIndex(l => l.LenderKingdomID == lender.id.ToString() && l.BorrowerKingdomID == borrower.id.ToString());
            
            if (index != -1)
            {
                var existingLoan = ActiveLoans[index];

                float oldVal = existingLoan.Principal;
                float newVal = principal;
                float total = oldVal + newVal;
                
                float weightedRate = ((oldVal * existingLoan.InterestRate) + (newVal * interestRate)) / total;
                
                existingLoan.Principal += principal; 
                existingLoan.InterestRate = weightedRate;
                existingLoan.CreationTick = Time.frameCount; 
                
                ActiveLoans[index] = existingLoan;
                EconomyLogger.LogVerbose($"LOAN MERGED: {borrower.name} added {principal:F0} to existing loan from {lender.name}");
            }
            else
            {
                float t = Mathf.InverseLerp(500f, 20000f, principal);
                float duration = Mathf.Lerp(60f, 1200f, t);
                if (principal < 1000) duration = 30f; 
                
                float installmentTime = duration / 10f; 
                float intervalTicks = installmentTime * 60f;

                var loan = new Loan
                {
                    LenderKingdomID = lender.id.ToString(),
                    BorrowerKingdomID = borrower.id.ToString(),
                    Principal = principal, 
                    InterestRate = interestRate,
                    RepaidAmount = 0f,
                    CreationTick = Time.frameCount,
                    LastPaymentStatus = "New",
                    
                    NextRepaymentTick = Time.frameCount + intervalTicks, 
                    RepaymentInterval = intervalTicks
                };
                ActiveLoans.Add(loan);
                EconomyLogger.LogVerbose($"[LOAN] New: {borrower.name} borrows {principal:F0} from {lender.name} @ {interestRate:P1}. Duration: {duration}s.");
            }

            GetKingdomData(lender).Wealth -= principal;
            GetKingdomData(borrower).Wealth += principal;
            
            GetKingdomData(borrower).NationalDebt += principal;
        }
        public UnitEconomyData GetUnitData(Actor unit)
        {
            if (unit == null) return null;
            if (!UnitData.ContainsKey(unit))
            {
                UnitData[unit] = new UnitEconomyData();
                UnitData[unit].PersonalWealth = 10; 
            }
            return UnitData[unit];
        }

        private void ProcessLoanRepayments()
        {
            for (int i = ActiveLoans.Count - 1; i >= 0; i--)
            {
                var loan = ActiveLoans[i];
                if(Time.frameCount < loan.NextRepaymentTick) continue; 
                
                loan.NextRepaymentTick += loan.RepaymentInterval;
                
                var borrower = MapBox.instance.kingdoms.list.FirstOrDefault(k => k.id.ToString() == loan.BorrowerKingdomID);
                var lender = MapBox.instance.kingdoms.list.FirstOrDefault(k => k.id.ToString() == loan.LenderKingdomID);

                if (borrower == null || lender == null)
                {
                    ActiveLoans.RemoveAt(i); 
                    continue;
                }

                var bData = GetKingdomData(borrower);
                var lData = GetKingdomData(lender);

                float totalLoanCost = loan.Principal * (1f + loan.InterestRate);
                float installment = totalLoanCost / 10f; 
                if(installment < 10) installment = 10; 
                
                if(installment > 2000000000f) installment = 2000000000f;

                int paymentInt = Mathf.CeilToInt(installment); 

                bool paid = false;

                int actualGold = 0; 
                
                if (bData.CurrencyID == lData.CurrencyID && !string.IsNullOrEmpty(bData.CurrencyID)) 
                {
                     loan.LastPaymentStatus = "Alliance";
                     paid = true; 
                }
                else 
                {
                    foreach(var city in borrower.cities) actualGold += city.getResourcesAmount("gold");
                    
                    if (actualGold >= paymentInt)
                    {
                        TransferGold(borrower, lender, paymentInt);

                        loan.RepaidAmount += paymentInt;
                        loan.LastPaymentStatus = "Gold"; 
                        EconomyLogger.LogVerbose($"[LOAN PAY] {borrower.name} paid {paymentInt} G to {lender.name} (from Reserves).");
                        paid = true;
                    }
                    else
                    {
                        int deficit = paymentInt - actualGold;
                        int goldRaised = LiquidateAssets(borrower, deficit);
                        
                        int totalGoldAvailable = actualGold + goldRaised;
                        
                        if (totalGoldAvailable >= paymentInt)
                        {
                            TransferGold(borrower, lender, actualGold); 
                            TransferGold(borrower, lender, deficit); 

                            loan.RepaidAmount += paymentInt;
                            loan.LastPaymentStatus = "Assets"; 
                            EconomyLogger.LogVerbose($"[LOAN PAY] {borrower.name} sold assets to pay {paymentInt} G to {lender.name}.");
                            paid = true;
                        }
                        else
                        {
                            bool sanityCheck = bData.CurrencySupply < (bData.Wealth * 50f);
                            if(bData.CurrencyValue > 0.05f && sanityCheck) 
                            {
                                float amountInCoins = paymentInt / bData.CurrencyValue;
                                bData.CurrencySupply += amountInCoins; 
                                bData.CurrencyValue *= 0.995f; 
                                
                                loan.RepaidAmount += paymentInt;
                                loan.LastPaymentStatus = "Print"; 
                                EconomyLogger.LogVerbose($"[LOAN PAY] {borrower.name} printed {amountInCoins:F0} {bData.CurrencyID} to pay {lender.name} (Value dropped).");
                                paid = true;
                            }
                        }
                    }
                }

                if(paid)
                {
                    ActiveLoans[i] = loan; 
                    CheckForFullRepayment(loan, borrower, lender, bData, i);
                }
                else
                {
                    if(bData.CreditScore > 0) bData.CreditScore -= 5f; 
                    loan.LastPaymentStatus = "Miss";
                    ActiveLoans[i] = loan;
                    EconomyLogger.LogVerbose($"[LOAN MISS] {borrower.name} defaulted on payment to {lender.name} (Reserves: {actualGold}).");
                    
                    if (bData.HyperinflationTimer > 50 && bData.CurrencySupply > 1000000f)
                    {
                         TriggerSovereignDefault(borrower, bData);
                         ActiveLoans.RemoveAt(i); 
                         continue;
                    }
                }
            }
        }

        private void TransferGold(Kingdom from, Kingdom to, int amount)
        {
            int remaining = amount;
            foreach(var city in from.cities)
            {
                if(remaining <= 0) break;
                int has = city.getResourcesAmount("gold");
                int take = Mathf.Min(has, remaining);
                SimulationGameloop.ChangeResource(city, "gold", -take);
                remaining -= take;
            }

            if(to.cities.Count > 0)
            {
                var cap = to.cities[0]; 
                SimulationGameloop.ChangeResource(cap, "gold", amount);
            }
        }

        private int LiquidateAssets(Kingdom k, int goldNeeded)
        {
            int goldGenerated = 0;
            string[] sellable = new string[] { "gem", "mithril", "adamantine", "silver", "token" }; 
            
            foreach(var resID in sellable)
            {
                if(goldGenerated >= goldNeeded) break;
                
                foreach(var city in k.cities)
                {
                    if(goldGenerated >= goldNeeded) break;

                    int amount = city.getResourcesAmount(resID);
                    if(amount > 0)
                    {
                        int price = 10; 
                        if(resID == "silver") price = 5;
                        
                        int toSell = Mathf.Min(amount, (goldNeeded - goldGenerated) / price + 1);
                        
                        SimulationGameloop.ChangeResource(city, resID, -toSell);
                        int gain = toSell * price;
                        SimulationGameloop.ChangeResource(city, "gold", gain); 
                        
                        goldGenerated += gain;
                    }
                }
            }
            
            if(goldGenerated < goldNeeded)
            {
                string[] basics = new string[] { "wood", "stone", "wheat" };
                foreach(var resID in basics)
                {
                     if(goldGenerated >= goldNeeded) break;
                     foreach(var city in k.cities)
                     {
                         if(goldGenerated >= goldNeeded) break;
                         int amount = city.getResourcesAmount(resID);
                         if(amount > 50) 
                         {
                             int toSell = 50;
                             SimulationGameloop.ChangeResource(city, resID, -toSell);
                             int gain = toSell * 1; 
                             SimulationGameloop.ChangeResource(city, "gold", gain); 
                             goldGenerated += gain;
                         }
                     }
                }
            }

            return goldGenerated;
        }
        
        private void CheckForFullRepayment(Loan loan, Kingdom borrower, Kingdom lender, KingdomEconomyData bData, int index)
        {
             float totalDue = loan.Principal * (1f + loan.InterestRate);
             if (loan.RepaidAmount >= totalDue)
             {
                 ActiveLoans.RemoveAt(index);
                 EconomyLogger.LogVerbose($"LOAN PAID: {borrower.name} paid off loan to {lender.name}");
                 
                 if(bData.CreditScore < 100) bData.CreditScore += 5f;
             }
             else
             {
                 if(bData.CreditScore < 100) bData.CreditScore += 0.1f;
             }
        }


        private void CleanupInvalidData()
        {
            List<Kingdom> kingdomsToRemove = new List<Kingdom>();
            foreach (var k in KingdomData.Keys)
            {
                if (k == null || !MapBox.instance.kingdoms.list.Contains(k))
                {
                    kingdomsToRemove.Add(k);
                }
            }
            foreach (var k in kingdomsToRemove) KingdomData.Remove(k);

            List<Actor> unitsToRemove = new List<Actor>();
            foreach (var u in UnitData.Keys)
            {
                if (u == null || !u.isAlive())
                {
                    unitsToRemove.Add(u);
                }
            }
            foreach (var u in unitsToRemove) UnitData.Remove(u);

            // Clean Invalid Loans
            ActiveLoans.RemoveAll(l => 
            {
                var lender = MapBox.instance.kingdoms.list.FirstOrDefault(k => k.id.ToString() == l.LenderKingdomID);
                var borrower = MapBox.instance.kingdoms.list.FirstOrDefault(k => k.id.ToString() == l.BorrowerKingdomID);
                return lender == null || !lender.isAlive() || borrower == null || !borrower.isAlive();
            });
        }

        public string CurrentAgeID = "";

        public KingdomEconomyData GetKingdomData(Kingdom k)
        {
            if (k == null) return null;
            if (!KingdomData.ContainsKey(k))
            {
                KingdomData[k] = new KingdomEconomyData();
                 KingdomData[k].EconomicSystem = UnityEngine.Random.value > 0.5f ? "Capitalism" : "Socialism";
            }
            return KingdomData[k];
        }

        private void UpdateCurrentAge()
        {
            if(MapBox.instance == null) return;
            try {
                var eraManager = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "era_manager");
                if(eraManager != null) {
                    var asset = Reflection.GetField(eraManager.GetType(), eraManager, "asset") as WorldAgeAsset;
                    if(asset != null && asset.id != CurrentAgeID)
                    {
                        CurrentAgeID = asset.id;
                    }
                }
            } catch { }
        }
        
        private void TriggerSovereignDefault(Kingdom k, KingdomEconomyData data)
        {
            float oldSupply = data.CurrencySupply;
            data.CurrencySupply = 1000f; 
            data.CurrencyValue = 1.0f;   
            data.HyperinflationTimer = 0;
            data.GoldReserves = 0; 
            data.CreditScore = 0;  

            int wipedCount = 0;
            for(int i = ActiveLoans.Count - 1; i >= 0; i--)
            {
                if(ActiveLoans[i].BorrowerKingdomID == k.id.ToString())
                {
                    ActiveLoans.RemoveAt(i);
                    wipedCount++;
                }
            }
            
            data.CurrencyName = "New " + data.CurrencyName;
            EconomyLogger.LogVerbose($"COLLAPSE: {k.name} reset currency. Old Supply: {oldSupply:N0}. Loans Wiped: {wipedCount}");
        }
        
        /// <summary>
        /// Updates global resource prices based on world supply/demand.
        /// Price = BasePrice * (1000 / (GlobalSupply + 100))
        /// More supply = lower price, less supply = higher price.
        /// </summary>
        private void UpdateMarketPrices()
        {
            if(AssetManager.resources == null || AssetManager.resources.list == null) return;
            
            foreach(ResourceAsset res in AssetManager.resources.list)
            {
                if(res == null) continue;
                string resId = res.id;
                
                float globalSupply = 0f;
                
                // Calculate total world supply
                foreach(var k in MapBox.instance.kingdoms.list)
                {
                    if(!k.isCiv()) continue;
                    foreach(var city in k.cities)
                    {
                        globalSupply += city.getResourcesAmount(resId);
                    }
                }
                
                ResourceSupply[resId] = globalSupply;
                
                // Base price based on resource type
                float basePrice = 1f;
                switch(resId)
                {
                    case "wheat": case "berries": case "fish": basePrice = 0.5f; break;
                    case "wood": case "leather": basePrice = 0.8f; break;
                    case "stone": case "bones": basePrice = 1f; break;
                    case "iron": case "copper": basePrice = 2f; break;
                    case "gold": case "silver": basePrice = 5f; break;
                    case "mithril": case "gem": basePrice = 10f; break;
                    case "adamantine": basePrice = 20f; break;
                    default: basePrice = 1f; break;
                }
                
                // Price formula: higher supply = lower price
                float supplyFactor = 1000f / (globalSupply + 100f);
                float price = basePrice * Mathf.Clamp(supplyFactor, 0.1f, 10f);
                
                // Add market volatility
                price *= UnityEngine.Random.Range(0.95f, 1.05f);
                
                ResourcePrices[resId] = price;
            }
        }
        
        /// <summary>
        /// Gets the current market price for a resource.
        /// </summary>
        public float GetResourcePrice(string resId)
        {
            if(ResourcePrices.TryGetValue(resId, out float price))
                return price;
            return 1f;
        }
    }
}


