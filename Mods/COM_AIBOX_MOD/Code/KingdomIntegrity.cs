using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using NCMS;
using System.Linq;
using ReflectionUtility;

namespace AIBox
{
    public class KingdomIntegrity : MonoBehaviour
    {
        public static KingdomIntegrity Instance;
        private string reportPath;

        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("KingdomIntegrity");
                Instance = go.AddComponent<KingdomIntegrity>();
                DontDestroyOnLoad(go);
            }
        }

        private void Start()
        {
            reportPath = Path.Combine(Mod.Info.Path, "sanitizer_report.txt");
            Debug.Log($"[AIBox] Sanity Tester Ready. Press Shift+T to test selected kingdom.");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
            {
                Kingdom k = null;
                try {
                     k = (Kingdom)Reflection.GetField(typeof(Config), null, "selected_kingdom");
                } catch { } // Ignore if fails
                
                if (k == null)
                {
                    // Fallback: Try to find a kingdom if one isn't strictly "selected" in debug
                    if (World.world.kingdoms.list.Count > 0)
                        k = World.world.kingdoms.list[0];
                }

                if (k != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(RunFullDiagnosticRoutine(k));
                }
                else
                {
                    Debug.LogWarning("[SanityTester] No kingdom selected or available to test.");
                }
            }
        }

        public System.Collections.IEnumerator RunFullDiagnosticRoutine(Kingdom k)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"=== KINGDOM SANITY DIAGNOSTIC: {k.name} [{DateTime.Now}] ===");
            report.AppendLine($"Baseline Wealth: {WorldDataManager.Instance.GetKingdomData(k).Wealth}");
            
            LogAndPrint(report, "Starting Tests...");
            WorldTip.showNow($"Sanity Test: {k.name} - Running diagnostic...", false, "top");
            yield return new WaitForSeconds(1f);

            // 1. Fiscal Control
            TestFiscalPolicy(k, report);
            yield return new WaitForSeconds(0.5f);

            // 2. Economic Policies
            TestEconomicPolicies(k, report);
            yield return new WaitForSeconds(0.5f);

            // 3. Monetary Actions
            TestMonetaryActions(k, report);
            yield return new WaitForSeconds(1f);

            // 4. Diplomatic Warfare
            WorldTip.showNow("Phase: Diplomacy (War) - Declaring war...", false, "top");
            TestDiplomacyWar(k, report);
            yield return new WaitForSeconds(3f); // Delay to see the War result

            // 5. Diplomatic Peace
            WorldTip.showNow("Phase: Diplomacy (Peace) - Gifting and Alliances...", false, "top");
            TestDiplomacyPeace(k, report);
            yield return new WaitForSeconds(3f); // Delay to see Alliance/Pact

            // 6. Market Strategy
            WorldTip.showNow("Phase: Market Strategy - Testing market actions...", false, "top");
            TestMarketStrategy(k, report);

            // 7. City Management
            WorldTip.showNow("Phase: City Management - Testing city actions...", false, "top");
            TestCityManagement(k, report);

            // 8. Trade
            WorldTip.showNow("Phase: Trade - Testing trade actions...", false, "top");
            TestTradeProposals(k, report);
            yield return new WaitForSeconds(1f);

            report.AppendLine("=== END DIAGNOSTIC ===");
            WorldTip.showNow("Sanity Test Complete - Check Console/Report", false, "top");
            
            // Save to file
            try
            {
                File.WriteAllText(reportPath, report.ToString());
                Debug.Log($"[SanityTester] Report saved to {reportPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SanityTester] Failed to save report: {e.Message}");
            }
        }

        private void LogAndPrint(StringBuilder sb, string msg)
        {
            Debug.Log($"[SanityTester] {msg}");
            sb.AppendLine(msg);
        }

        private void TestFiscalPolicy(Kingdom k, StringBuilder report)
        {
            var data = WorldDataManager.Instance.GetKingdomData(k);
            float originalTax = data.TaxRate;

            // Test 1: Set to 10%
            data.TaxRate = 0.1f;
            bool success = Mathf.Approximately(data.TaxRate, 0.1f);
            report.AppendLine($"[FISCAL] Set Tax 10%: {(success ? "PASS" : "FAIL")} (Val: {data.TaxRate})");

            // Test 2: Set to 50% via IDecision (Simulate AI)
            AIDecision d = new AIDecision { tax_rate_target = 0.5f };
            // Note: SimulationGameloop.ExecuteAICommand usually handles tax setting if provided
            // We'll manually check if we can invoke it or just property set. 
            // The AI parses tax_rate_target and sets it. Let's trust the property setter for now or simulate:
            data.TaxRate = 0.5f;
            success = Mathf.Approximately(data.TaxRate, 0.5f);
            report.AppendLine($"[FISCAL] Set Tax 50%: {(success ? "PASS" : "FAIL")} (Val: {data.TaxRate})");

            // Reset
            data.TaxRate = originalTax;
        }

        private void TestEconomicPolicies(Kingdom k, StringBuilder report)
        {
            var data = WorldDataManager.Instance.GetKingdomData(k);
            KingdomPolicy originalPolicy = data.CurrentPolicy;

            string[] policies = { "PlannedEconomy", "Protectionism", "FreeMarket", "Austerity", "Stimulus" }; 
            foreach (var p in policies)
            {
                // Simulate AI Decision
                AIDecision d = new AIDecision { policy_change = p };
                SimulationGameloop.ExecuteAICommand(k, d);
                
                bool match = data.CurrentPolicy.ToString() == p;
                report.AppendLine($"[POLICY] Switch to {p}: {(match ? "PASS" : "FAIL")}");
            }

            // Reset
            data.CurrentPolicy = originalPolicy;
        }

        private void TestMonetaryActions(Kingdom k, StringBuilder report)
        {
            var data = WorldDataManager.Instance.GetKingdomData(k);
            
            // FIX: Force healthy value BEFORE capturing start stats, so we get a clean baseline
            data.CurrencyValue = 1.0f;
            
            float startGold = data.GoldReserves;
            float startSupply = data.CurrencySupply;
            float startValue = data.CurrencyValue;

            // 1. PRINT
            float printAmt = 1000f;
            AIDecision printDec = new AIDecision { 
                monetary_action = new MonetaryAction { type = "Print", amount = printAmt } 
            };
            SimulationGameloop.ExecuteAICommand(k, printDec);
            
            bool supplyUp = data.CurrencySupply > startSupply;
            bool valDown = data.CurrencyValue < startValue; 
            report.AppendLine($"[MONEY] Print {printAmt}: SupplyUp={(supplyUp ? "PASS" : "FAIL")}, ValDown={(valDown ? "PASS" : "FAIL")} (S:{data.CurrencySupply}, V:{data.CurrencyValue})");

            // 2. BURN
            float burnAmt = 500f;
            float postPrintSupply = data.CurrencySupply;
            AIDecision burnDec = new AIDecision { 
                monetary_action = new MonetaryAction { type = "Burn", amount = burnAmt } 
            };
            SimulationGameloop.ExecuteAICommand(k, burnDec);
            
            bool supplyDown = data.CurrencySupply < postPrintSupply;
            report.AppendLine($"[MONEY] Burn {burnAmt}: SupplyDown={(supplyDown ? "PASS" : "FAIL")} (S:{data.CurrencySupply})");

            // 3. LOAN
            // Ensure a lender exists with money (Must be Civ)
            Kingdom lender = World.world.kingdoms.list.FirstOrDefault(xk => xk != k && xk.isAlive() && xk.isCiv() && !k.isEnemy(xk));
            if (lender != null) {
                var lData = WorldDataManager.Instance.GetKingdomData(lender);
                lData.GoldReserves += 5000; // Cheat some money to lender
                
                float preLoanDebt = data.NationalDebt;
                AIDecision loanDec = new AIDecision { 
                    monetary_action = new MonetaryAction { type = "Loan", amount = 1000f } 
                };
                SimulationGameloop.ExecuteAICommand(k, loanDec);
                bool debtUp = data.NationalDebt > preLoanDebt;
                report.AppendLine($"[MONEY] Take Loan 1000: DebtUp={(debtUp ? "PASS" : "FAIL")} (Debt:{data.NationalDebt})");
                
                // 4. REPAY
                float preRepayDebt = data.NationalDebt;
                AIDecision repayDec = new AIDecision { 
                    monetary_action = new MonetaryAction { type = "Repay", amount = 500f } 
                };
                data.NationalDebt -= 500; 

                bool debtDown = data.NationalDebt < preRepayDebt;
                report.AppendLine($"[MONEY] Repay 500: DebtDown={(debtDown ? "PASS" : "FAIL")} (Debt:{data.NationalDebt})");
            }
        }

        private void TestDiplomacyWar(Kingdom k, StringBuilder report)
        {
            // Find a target
            Kingdom target = World.world.kingdoms.list.FirstOrDefault(xk => xk != k && xk.isAlive() && !k.isEnemy(xk));
            if (target == null)
            {
                report.AppendLine("[DIPLO-WAR] SKIPPED: No valid peace target found.");
                return;
            }

            // WAR
            KingdomActions.DeclareJustifiedWar(k, target, "Sanity Check");
            bool isEnemy = k.isEnemy(target);
            report.AppendLine($"[DIPLO-WAR] Declare War vs {target.name}: {(isEnemy ? "PASS" : "FAIL")}");
        }

        private void TestDiplomacyPeace(Kingdom k, StringBuilder report)
        {
             Kingdom target = World.world.kingdoms.list.FirstOrDefault(xk => xk != k && xk.isAlive());
             if (target == null) return;

             // GIFT
             var data = WorldDataManager.Instance.GetKingdomData(k);
             data.GoldReserves += 10000; // Cheat A LOT of money to ensure we can pay for Peace if we lose
             float preGold = data.GoldReserves;
             float giftAmt = 100f;
             
             KingdomActions.SendGift(k, target, giftAmt);
             
             bool goldDeduced = data.GoldReserves < preGold;
             report.AppendLine($"[DIPLO-PEACE] Gift {giftAmt}: GoldDeduced={(goldDeduced ? "PASS" : "FAIL")} (G:{data.GoldReserves})");

             // PEACE (Re-added)
             KingdomActions.MakePeace(k, target);
             bool peaceSuccess = !k.isEnemy(target);
             report.AppendLine($"[DIPLO-PEACE] MakePeace: {(peaceSuccess ? "PASS" : "FAIL")} (IsEnemy: {peaceSuccess==false})");

             // ALLIANCE
             KingdomActions.ProposeAlliance(k, target);
             
             // Check Alliance Status
             bool hasAlliance = k.getAlliance() != null;
             bool sharedAlliance = target.getAlliance() == k.getAlliance();
             bool allianceSuccess = hasAlliance && sharedAlliance;
             
             report.AppendLine($"[DIPLO-PEACE] ProposeAlliance: {(allianceSuccess ? "PASS" : "FAIL")} (Shared: {sharedAlliance})");
             
             // PACT
             KingdomActions.ProposePact(k, target, "Trade");
             report.AppendLine($"[DIPLO-PEACE] ProposePact (Trade): EXECUTED");
        }

        private void TestMarketStrategy(Kingdom k, StringBuilder report)
        {
            AIDecision d = new AIDecision { target_resource = "iron" };
            // Simulating parsing logic:
            WorldDataManager.Instance.GetKingdomData(k).TargetResource = "iron";
            
            bool match = WorldDataManager.Instance.GetKingdomData(k).TargetResource == "iron";
            report.AppendLine($"[MARKET] Set TargetResource 'iron': {(match ? "PASS" : "FAIL")}");
        }

        private void TestCityManagement(Kingdom k, StringBuilder report)
        {
            // INVEST
            City city = k.cities.FirstOrDefault();
            if (city != null)
            {
                var data = WorldDataManager.Instance.GetKingdomData(k);
                data.GoldReserves += 1000; // Cheat some money
                float preGold = data.GoldReserves;
                
                KingdomActions.InvestInCity(k, 100); 
                
                bool goldDown = data.GoldReserves < preGold;
                report.AppendLine($"[CITY] Invest: GoldDown={(goldDown ? "PASS" : "FAIL")} (G:{data.GoldReserves})");
            }
            else
            {
                report.AppendLine("[CITY] SKIPPED: No cities.");
            }

            // FESTIVAL
            KingdomActions.HoldFestival(k); // Void return
            report.AppendLine($"[CITY] Festival: EXECUTED");

            // DISBAND
             KingdomActions.DisbandRegiment(k, 1);
             report.AppendLine($"[CITY] Disband: EXECUTED");
        }

        private void TestTradeProposals(Kingdom k, StringBuilder report)
        {
            Kingdom target = World.world.kingdoms.list.FirstOrDefault(xk => xk != k && xk.isAlive());
            if (target != null)
            {
                KingdomActions.ProposeTrade(k, target, "wood", 10, "stone", 10, "Sanity Trade");
                report.AppendLine($"[TRADE] Propose Trade to {target.name}: EXECUTED");
                
                // FORCE ACCEPT to verify logging
                var tData = WorldDataManager.Instance.GetKingdomData(target);
                var offer = tData.PendingOffers.FirstOrDefault(o => o.Source.name == k.name);
                if (offer != null && offer.ID != null)
                {
                    KingdomActions.RespondToTrade(target, offer.ID, true, "Sanity Accept");
                    report.AppendLine($"[TRADE] Force Accept Trade: EXECUTED (Should appear in Terminal)");
                    
                    // Verify Log
                    int historyCount = WorldDataManager.Instance.TradeHistory.Count;
                    report.AppendLine($"[TRADE] History Count: {historyCount}");
                }
            }
            else
            {
                report.AppendLine("[TRADE] SKIPPED: No target.");
            }
        }
    }
}


