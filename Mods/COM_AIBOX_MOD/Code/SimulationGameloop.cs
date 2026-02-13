using System;
using System.Collections.Generic;
using System.Linq;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace AIBox
{
    public static class SimulationGameloop
    {
        // Moved from EcoSystem
        public static List<string> IconCurrency = new List<string>{
          "SunstoneShillings","MoaiMarkers","Drachmas","ThunderMarks","HerzCoin","Dollar","dollares","RoseCoin",
          "PedisCoin","RoseGold","CandyToken","AngelsToken","NoticeCoin","HalfCoin","CatCoin","LemonToken",
          "DemonMarks","RegalEffigies","Stars","FlyCoin","BloodswordCrowns","Golthammer","GoldFlorins",
          "PhoenixPounds","MarkamiCoin","Crowns","Shillings","Marks","Denarii","Souls","Florins","FrogCoin",
          "NaturalCoin","GodCoin","AngelMarks","BoxCoin","CorruptCoin","Monopolism","RiverCoin","Boxcraft_Coin",
          "KingCoin","CrabCoin","HammerCoin","GregCoin","ElvenGold","BearsToken","TechCoin","Desolation",
          "DivineCoin","MonolithCoin","Bluehawk"
        };

        public static void ExecuteAICommand(Kingdom k, AIDecision d)
        {
            if(k == null || d == null) return;
            
            var data = WorldDataManager.Instance.GetKingdomData(k);
            
            // 1. Tax
            if(d.tax_rate_target > 0) {
                 float oldTax = data.TaxRate;
                 data.TaxRate = Mathf.Clamp(d.tax_rate_target, 0f, 1f);
                 // Only log significant tax changes or if requested explicitly
                 // data.LastTurnFeedback += $"[TAX] Set to {data.TaxRate:P0}.\n"; 
            }
            
            // 2. Policy
            if(!string.IsNullOrEmpty(d.policy_change) && d.policy_change != "None") {
                 try {
                    data.CurrentPolicy = (KingdomPolicy)Enum.Parse(typeof(KingdomPolicy), d.policy_change);
                    data.LastTurnFeedback += $"[POLICY] Enacted {d.policy_change}.\n";
                 } catch {
                    data.LastTurnFeedback += $"[POLICY] Failed. Unknown policy '{d.policy_change}'.\n";
                 }
            }
            
            // 3. Monetary Actions
            if(d.monetary_action != null && !string.IsNullOrEmpty(d.monetary_action.type) && d.monetary_action.type != "None") {
                string monType = d.monetary_action.type.ToLower();
                float amount = d.monetary_action.amount > 0 ? d.monetary_action.amount : 500f;
                
                if(monType == "print" || monType == "printmoney") {
                    PrintMoney(k, amount, WorldDataManager.Instance);
                    data.LastTurnFeedback += $"[MONETARY] Printed {amount:N0} currency.\n";
                }
                else if(monType == "burn") {
                    float burnAmt = Mathf.Min(amount, data.GoldReserves * 0.2f);
                    data.CurrencySupply -= burnAmt;
                    if(data.CurrencySupply < 100) data.CurrencySupply = 100;
                    data.CurrencyValue = CalculateCurrencyValue(k, WorldDataManager.Instance);
                    data.LastTurnFeedback += $"[MONETARY] Burned {burnAmt:N0} currency (Deflationary).\n";
                }
                else if(monType == "loan") {
                    float loanAmt = Mathf.Clamp(amount, 100f, 5000f);
                    data.GoldReserves += loanAmt;
                    data.NationalDebt += loanAmt * 1.2f;
                    if(k.capital != null) ChangeResource(k.capital, "gold", (int)(loanAmt * 0.5f));
                    data.LastTurnFeedback += $"[MONETARY] Taken Loan of {loanAmt:N0} gold.\n";
                }
                else if(monType == "repay") {
                    float repayAmt = Mathf.Min(amount, data.GoldReserves, data.NationalDebt);
                    if(repayAmt > 0) {
                        data.GoldReserves -= repayAmt;
                        data.NationalDebt -= repayAmt;
                        if(data.NationalDebt < 0) data.NationalDebt = 0;
                        data.LastTurnFeedback += $"[MONETARY] Repaid {repayAmt:N0} debt.\n";
                    } else {
                        data.LastTurnFeedback += $"[MONETARY] Repay failed. Insufficient funds or zero debt.\n";
                    }
                }
                else if(monType == "bankruptcy" || monType == "declarebankruptcy") {
                    KingdomActions.DeclareBankruptcy(k);
                    data.LastTurnFeedback += $"[MONETARY] DECLARED BANKRUPTCY! Debt reset, major penalties applied.\n";
                }
            }
            
            // 4. Diplomacy - Handle both "target" (singular) and "targets" (array)
            // SAFETY: Limit diplomatic actions to prevent AI from declaring war/peace/alliance with everyone
            if(d.diplomatic_action != null && d.diplomatic_action.type != "None" && !string.IsNullOrEmpty(d.diplomatic_action.type)) {
                List<Kingdom> targetKingdoms = new List<Kingdom>();
                string dipType = d.diplomatic_action.type;
                
                // First, try 'target' (singular)
                if(!string.IsNullOrEmpty(d.diplomatic_action.target)) {
                    Kingdom t = KingdomActions.FindKingdomByName(d.diplomatic_action.target);
                    if(t != null && t != k) targetKingdoms.Add(t);
                }
                
                // Then, try 'targets' (array)
                if(d.diplomatic_action.targets != null && d.diplomatic_action.targets.Count > 0) {
                    foreach(string tName in d.diplomatic_action.targets) {
                        Kingdom t = KingdomActions.FindKingdomByName(tName);
                        if(t != null && t != k && !targetKingdoms.Contains(t)) targetKingdoms.Add(t);
                    }
                }
                
                // SAFETY: Limit to maximum 3 diplomatic targets per turn to prevent mass-war declarations
                const int MAX_DIPLO_TARGETS = 3;
                if(targetKingdoms.Count > MAX_DIPLO_TARGETS) {
                    Debug.Log($"[SimulationGameloop] {k.name} tried to {dipType} {targetKingdoms.Count} kingdoms. Limiting to {MAX_DIPLO_TARGETS}.");
                    targetKingdoms = targetKingdoms.Take(MAX_DIPLO_TARGETS).ToList();
                }
                
                // If no valid targets are found, check if the action actually requires one
                bool needsTarget = dipType != "TrainArmy" && dipType != "Festival" && dipType != "AntiCorruption";

                if(targetKingdoms.Count == 0 && needsTarget) {
                    // Try to provide feedback in logs
                    if(!string.IsNullOrEmpty(d.diplomatic_action.target) || (d.diplomatic_action.targets != null && d.diplomatic_action.targets.Count > 0)) {
                        Debug.LogWarning($"[SimulationGameloop] {k.name} tried diplomatic action '{dipType}' but targets could not be resolved. Raw targets: {d.diplomatic_action.target ?? "NULL"} / List: {(d.diplomatic_action.targets != null ? string.Join(",", d.diplomatic_action.targets) : "NULL")}");
                    }
                } else {
                    // Handle non-targeted diplomatic actions (misclassified by AI)
                    if(!needsTarget) {
                        if(dipType == "TrainArmy") KingdomActions.TrainArmy(k);
                        else if(dipType == "Festival") KingdomActions.HoldFestival(k);
                        else if(dipType == "AntiCorruption") KingdomActions.AntiCorruption(k);
                    }

                    // Execute diplomatic actions only if we have valid targets
                    foreach(Kingdom target in targetKingdoms) {
                        if(target == null || !target.isAlive()) continue;

                        // Additional validation per action type
                        if(dipType == "War") {
                            if(!k.isEnemy(target)) { 
                                string reason = !string.IsNullOrEmpty(d.diplomatic_action.war_reason) ? d.diplomatic_action.war_reason : "Expansion";
                                KingdomActions.DeclareJustifiedWar(k, target, reason);
                                data.LastTurnFeedback += $"[WAR] Declared on {target.name}. Reason: {reason}.\n";
                            } else {
                                data.LastTurnFeedback += $"[WAR] Invalid. Already at war with {target.name}.\n";
                            }
                        } 
                        else if(dipType == "Peace") {
                            if(k.isEnemy(target)) { 
                                KingdomActions.ProposePeace(k, target, d.diplomatic_action.amount);
                                data.LastTurnFeedback += $"[PEACE] Offer sent to {target.name}.\n";
                            } else {
                                data.LastTurnFeedback += $"[PEACE] Invalid. Not at war with {target.name}.\n";
                            }
                        } 
                        else if(dipType == "Alliance") {
                            if(!k.isEnemy(target)) { 
                                // STRICT COMPATIBILITY CHECK
                                var cultA = CultureReligionHelper.GetKingdomCulture(k);
                                var cultB = CultureReligionHelper.GetKingdomCulture(target);
                                var relA = CultureReligionHelper.GetKingdomReligion(k);
                                var relB = CultureReligionHelper.GetKingdomReligion(target);

                                bool cultureMatch = cultA.name == cultB.name && cultA.name != "None";
                                bool religionMatch = relA.name == relB.name && relA.name != "None";
                                bool highOpinion = false; 
                                try {
                                    object relation = Reflection.CallMethod(k, "getRelation", target);
                                    if(relation != null) {
                                        int opinion = (int)Reflection.GetField(relation.GetType(), relation, "opinion");
                                        highOpinion = opinion > 100;
                                    }
                                } catch {}

                                if(cultureMatch || religionMatch || highOpinion) {
                                    KingdomActions.ProposeAlliance(k, target);
                                    Debug.Log($"[AIBox] Alliance FORMED: {k.name} + {target.name}");
                                    data.LastTurnFeedback += $"[SUCCESS] Alliance proposed to {target.name}.\n";
                                } else {
                                    Debug.Log($"[AIBox] Alliance BLOCKED: {k.name} + {target.name}");
                                    data.LastTurnFeedback += $"[FAILED] Alliance with {target.name} BLOCKED. Reason: Different Culture/Religion and Opinion <= 100. Use Messages/Gifts to improve relations first.\n";
                                }
                            } else {
                                data.LastTurnFeedback += $"[FAILED] Cannot ally with enemy {target.name}.\n";
                            }
                        } 
                        else if(dipType == "BreakAlliance") {
                            KingdomActions.BreakAlliance(k, target);
                            data.LastTurnFeedback += $"[ALLIANCE] Broke alliance with {target.name}.\n";
                        }
                        else if(dipType == "Embargo") {
                            KingdomActions.EnforceEmbargo(k, target);
                            data.LastTurnFeedback += $"[EMBARGO] Enforced against {target.name}.\n";
                        } 
                        else if(dipType == "Gift") {
                            int giftAmt = d.diplomatic_action.amount > 0 ? d.diplomatic_action.amount : 100;
                            KingdomActions.SendGift(k, target, giftAmt);
                            data.LastTurnFeedback += $"[GIFT] Sent {giftAmt} gold to {target.name}.\n";
                        }
                        else if(dipType == "Message") {
                            string msg = !string.IsNullOrEmpty(d.diplomatic_action.message) ? d.diplomatic_action.message : "Greetings.";
                            KingdomActions.SendDiplomaticMessage(k, target, msg);
                            data.LastTurnFeedback += $"[MESSAGE] Sent to {target.name}: \"{msg}\".\n";
                        }
                        else if(dipType == "Pact") {
                            string pactType = !string.IsNullOrEmpty(d.diplomatic_action.pact_type) ? d.diplomatic_action.pact_type : "Trade";
                            KingdomActions.ProposePact(k, target, pactType);
                            data.LastTurnFeedback += $"[PACT] Proposed {pactType} pact to {target.name}.\n";
                        }
                        else if(dipType == "Threaten") {
                            string threat = !string.IsNullOrEmpty(d.diplomatic_action.message) ? d.diplomatic_action.message : "Submit or face destruction!";
                            KingdomActions.Threaten(k, target, threat);
                            data.LastTurnFeedback += $"[THREAT] Sent to {target.name}: \"{threat}\".\n";
                        }
                        else if(dipType == "DemandTribute") {
                            KingdomActions.DemandTribute(k, target);
                            data.LastTurnFeedback += $"[TRIBUTE] Demanded from {target.name}.\n";
                        }
                        else if(dipType == "FundRebels") {
                            int goldAmt = d.diplomatic_action.amount > 0 ? (int)d.diplomatic_action.amount : 500;
                            KingdomActions.FundRebels(k, target, goldAmt);
                            data.LastTurnFeedback += $"[FUND] Sent {goldAmt} gold to rebels in {target.name}.\n";
                        }
                        else if(dipType == "Mediate") {
                           if(d.diplomatic_action.targets != null && d.diplomatic_action.targets.Count >= 2) {
                               Kingdom k1 = KingdomActions.FindKingdomByName(d.diplomatic_action.targets[0]);
                               Kingdom k2 = KingdomActions.FindKingdomByName(d.diplomatic_action.targets[1]);
                               if(k1 != null && k2 != null) {
                                   KingdomActions.MediateConflict(k, k1, k2);
                                   data.LastTurnFeedback += $"[MEDIATE] Mediating between {k1.name} and {k2.name}.\n";
                               }
                           }
                        }
                    }
                }
            }
            
            // 5. Trade Actions
            if(d.trade_action != null && !string.IsNullOrEmpty(d.trade_action.type) && d.trade_action.type != "None") {
                if(d.trade_action.type == "Propose" && !string.IsNullOrEmpty(d.trade_action.target)) {
                    Kingdom target = KingdomActions.FindKingdomByName(d.trade_action.target);
                    if(target != null) {
                        KingdomActions.ProposeTrade(k, target, d.trade_action.offer_res, d.trade_action.offer_amt, 
                            d.trade_action.request_res, d.trade_action.request_amt, d.trade_action.message);
                        data.LastTurnFeedback += $"[TRADE] Proposed to {target.name}.\n";
                    }
                }
                else if(d.trade_action.type == "Respond" && !string.IsNullOrEmpty(d.trade_action.offer_id)) {
                    KingdomActions.RespondToTrade(k, d.trade_action.offer_id, d.trade_action.accept, d.trade_action.message);
                    data.LastTurnFeedback += $"[TRADE] Responded to offer {d.trade_action.offer_id}.\n";
                }
            }
            
            // 6. Ruler Actions
            if(d.ruler_action != null && !string.IsNullOrEmpty(d.ruler_action.type) && d.ruler_action.type != "None") {
                if(d.ruler_action.type == "Festival") {
                    KingdomActions.HoldFestival(k);
                    data.LastTurnFeedback += $"[FESTIVAL] Held festival.\n";
                }
                else if(d.ruler_action.type == "Disband") {
                    int count = 10; // Default disband count
                    KingdomActions.DisbandRegiment(k, count);
                    data.LastTurnFeedback += $"[DISBAND] Disbanded {count} soldiers.\n";
                }
            }
            
            // 7. Market Focus
            if(!string.IsNullOrEmpty(d.target_resource) && d.target_resource != "None") {
                data.TargetResource = d.target_resource;
            }
            
            // 8. Covert Actions
            if(d.covert_action != null && !string.IsNullOrEmpty(d.covert_action.type) && d.covert_action.type != "None") {
                string covType = d.covert_action.type;
                Kingdom target = !string.IsNullOrEmpty(d.covert_action.target) ? KingdomActions.FindKingdomByName(d.covert_action.target) : null;
                Kingdom target2 = !string.IsNullOrEmpty(d.covert_action.target2) ? KingdomActions.FindKingdomByName(d.covert_action.target2) : null;
                Kingdom blameTarget = !string.IsNullOrEmpty(d.covert_action.blame_target) ? KingdomActions.FindKingdomByName(d.covert_action.blame_target) : null;
                
                if(covType == "Spy" && target != null) {
                    KingdomActions.SpyOnKingdom(k, target);
                    data.LastTurnFeedback += $"[SPY] Spying on {target.name}.\n";
                }
                else if(covType == "TrainArmy") {
                    KingdomActions.TrainArmy(k);
                    data.LastTurnFeedback += $"[TRAIN] Training army.\n";
                }
                else if(covType == "MediateConflict" && target != null && target2 != null) {
                    KingdomActions.MediateConflict(k, target, target2);
                    data.LastTurnFeedback += $"[MEDIATE] Mediated between {target.name} and {target2.name}.\n";
                }
                else if(covType == "Sabotage" && target != null) {
                    KingdomActions.Sabotage(k, target, blameTarget);
                    data.LastTurnFeedback += $"[SABOTAGE] Sabotaged {target.name}.\n";
                }
                else if(covType == "Assassinate" && target != null) {
                    KingdomActions.Assassinate(k, target, blameTarget);
                    data.LastTurnFeedback += $"[ASSASSINATE] Attempted in {target.name}.\n";
                }
                else if(covType == "AntiCorruption") {
                    KingdomActions.AntiCorruption(k);
                    data.LastTurnFeedback += $"[ANTICORRUPTION] Purged corruption.\n";
                }
            }
            
            // 9. Market Actions (Global Stock Market)
            if(d.market_action != null && !string.IsNullOrEmpty(d.market_action.type) && d.market_action.type != "None") {
                string mktType = d.market_action.type;
                string resource = d.market_action.resource;
                int amount = d.market_action.amount;
                
                if(mktType == "Buy" && !string.IsNullOrEmpty(resource) && amount > 0) {
                    KingdomActions.BuyResource(k, resource, amount);
                    data.LastTurnFeedback += $"[MARKET] Bought {amount} {resource}.\n";
                }
                else if(mktType == "Sell" && !string.IsNullOrEmpty(resource) && amount > 0) {
                    KingdomActions.SellResource(k, resource, amount);
                    data.LastTurnFeedback += $"[MARKET] Sold {amount} {resource}.\n";
                }
            }
            
            // 10. Vassal Actions
            if(d.vassal_action != null && !string.IsNullOrEmpty(d.vassal_action.type) && d.vassal_action.type != "None") {
                string vasType = d.vassal_action.type;
                Kingdom target = !string.IsNullOrEmpty(d.vassal_action.target) ? KingdomActions.FindKingdomByName(d.vassal_action.target) : null;
                
                if(vasType == "AnnexVassal" && target != null) {
                    KingdomActions.AnnexVassal(k, target);
                    data.LastTurnFeedback += $"[VASSAL] Annexed vassal {target.name}.\n";
                }
                else if(vasType == "InstallPuppet" && target != null) {
                    KingdomActions.InstallPuppet(k, target);
                    data.LastTurnFeedback += $"[VASSAL] Installed puppet in {target.name}.\n";
                }
                else if(vasType == "GrantIndependence" && target != null) {
                    KingdomActions.GrantIndependence(k, target);
                    data.LastTurnFeedback += $"[VASSAL] Granted independence to {target.name}.\n";
                }
            }
            
            // 11. Build Actions (Infrastructure)
            if(d.build_action != null && !string.IsNullOrEmpty(d.build_action.type) && d.build_action.type != "None") {
                if(d.build_action.type == "ConstructBuilding" && !string.IsNullOrEmpty(d.build_action.building_type)) {
                    KingdomActions.ConstructBuilding(k, d.build_action.building_type);
                    data.LastTurnFeedback += $"[BUILD] Constructed {d.build_action.building_type}.\n";
                }
            }
            
            // 12. City Actions (Buy/Defect)
            if(d.city_action != null && !string.IsNullOrEmpty(d.city_action.type) && d.city_action.type != "None") {
                string cityType = d.city_action.type;
                Kingdom targetK = !string.IsNullOrEmpty(d.city_action.target_kingdom) ? KingdomActions.FindKingdomByName(d.city_action.target_kingdom) : null;
                
                if(cityType == "BuyCity" && targetK != null && !string.IsNullOrEmpty(d.city_action.city_name)) {
                    KingdomActions.BuyCity(k, targetK, d.city_action.city_name);
                    data.LastTurnFeedback += $"[CITY] Bought {d.city_action.city_name} from {targetK.name}.\n";
                }
                else if(cityType == "DefectCity" && !string.IsNullOrEmpty(d.city_action.city_name)) {
                    // Find unhappy city by name in any kingdom
                    City unhappyCity = null;
                    foreach(var kingdom in World.world.kingdoms.list) {
                        if(!kingdom.isCiv()) continue;
                        unhappyCity = kingdom.cities.FirstOrDefault(c => c.name == d.city_action.city_name);
                        if(unhappyCity != null) break;
                    }
                    if(unhappyCity != null) {
                        KingdomActions.DefectCity(k, unhappyCity);
                        data.LastTurnFeedback += $"[CITY] City {d.city_action.city_name} defected to us!\n";
                    } else {
                        data.LastTurnFeedback += $"[CITY] Defect failed. City {d.city_action.city_name} not found or not unhappy.\n";
                    }
                }
            }
            
            // 13. Economic Union Actions
            if(d.union_action != null && !string.IsNullOrEmpty(d.union_action.type) && d.union_action.type != "None") {
                Kingdom targetK = !string.IsNullOrEmpty(d.union_action.target) ? KingdomActions.FindKingdomByName(d.union_action.target) : null;
                
                if(d.union_action.type == "FormEconomicUnion" && targetK != null) {
                    KingdomActions.FormEconomicUnion(k, targetK);
                    data.LastTurnFeedback += $"[UNION] Formed with {targetK.name}.\n";
                }
                else if(d.union_action.type == "LeaveEconomicUnion" && targetK != null) {
                    KingdomActions.LeaveEconomicUnion(k, targetK);
                    data.LastTurnFeedback += $"[UNION] Left union with {targetK.name}.\n";
                }
            }
        }

        // Merged Helper from EcoSystem
        public static void ChangeResource(City city, string id, int amount) {
            if (city == null) return;
            if (AssetManager.resources.get(id) != null) {
                if (amount > 0) city.addResourcesToRandomStockpile(id, amount);
                else if (amount < 0) city.takeResource(id, -amount);
            }
        }

        public static void UpdateKingdoms(WorldDataManager manager)
        {
            foreach (var kingdom in MapBox.instance.kingdoms.list.ToList())
            {
                if (kingdom == null || !kingdom.isCiv()) continue;

                var kData = manager.GetKingdomData(kingdom);
                
                // Dynamic Thresholds
                float avgWealth = manager.GlobalAverageWealth;
                
                bool isRich = kData.Wealth > Mathf.Max(5000f, avgWealth * 2f);
                bool isCrisis = kData.CurrencyValue < 0.2f || (kData.NationalDebt > kData.Wealth * 5f);
                
                // Apply/Remove Kingdom Traits
                if(isRich) {
                    if(!kingdom.hasTrait("econ_power")) kingdom.addTrait("econ_power");
                    kingdom.removeTrait("econ_crisis");
                    if(UnityEngine.Random.value < 0.05f) kingdom.addRenown(1); 
                    if(UnityEngine.Random.value < 0.01f && kingdom.culture != null) {
                         kingdom.addRenown(1); 
                    }
                } else if (isCrisis) {
                    if(!kingdom.hasTrait("econ_crisis")) kingdom.addTrait("econ_crisis");
                    kingdom.removeTrait("econ_power");
                    // Renown Decay
                    if(UnityEngine.Random.value < 0.05f && kingdom.data.renown > 0) kingdom.addRenown(-1);

                    // CRISIS RECOVERY MECHANIC (Buff)
                    // If in crisis, give a small "IMF" loan or recovery bonus naturally
                    if(UnityEngine.Random.value < 0.1f) {
                        float bailout = 200f;
                        kData.GoldReserves += bailout;
                        // Reduce debt slightly for free
                        kData.NationalDebt = Mathf.Max(0, kData.NationalDebt - 100f);
                        EconomyLogger.LogVerbose($"[RECOVERY] {kingdom.name} received emergency aid (+200g, -100 debt).");
                    }
                } else {
                    // Normal
                    kingdom.removeTrait("econ_power");
                    kingdom.removeTrait("econ_crisis");
                }

                bool canPaySoldiers = !isCrisis && kData.GoldReserves > 100; 
                
                if(UnityEngine.Random.value < 0.1f) 
                {
                    kingdom.units.RemoveAll(u => u == null);

                    foreach(var unit in kingdom.units)
                    {
                        if(unit.isWarrior()) 
                        {
                            if(canPaySoldiers) {
                                if(!unit.hasTrait("well_paid")) unit.addTrait("well_paid");
                                unit.removeTrait("unpaid");
                            } else {
                                if(isCrisis && !unit.hasTrait("unpaid")) unit.addTrait("unpaid"); 
                                unit.removeTrait("well_paid");
                            }
                        }
                    }
                }

                // Migration Logic
                foreach(var city in kingdom.cities)
                {
                    int cityGold = city.getResourcesAmount("gold");
                    float richCityThresh = Mathf.Max(500f, manager.GlobalAverageGold * 0.5f);
                    if(cityGold > richCityThresh && city.status.housing_free > 0)
                    {
                        if(UnityEngine.Random.value < 0.01f)
                        {
                            ActorAsset race = city.getActorAsset();
                            if(race != null) {
                                Actor migrant = World.world.units.spawnNewUnitByPlayer(race.id, city.getTile(), true, false, 6f, null); 
                                migrant.joinCity(city);
                                EconomyLogger.LogVerbose($"MIGRATION: {city.name} attracted a migrant due to wealth.");
                            }
                        }
                    }
                }
                
                InitializeKingdomAssets(kingdom, kData);
                CalculateWealth(kingdom, kData, manager);
                UpdateBanks(kingdom, kData);
                UpdateCycles(kingdom, kData);

                UpdateResourceNeeds(kingdom, kData);

                UpdateFiscalPolicy(kingdom, kData);
                
                HandleInflation(kingdom, kData, manager);
                
                HandleExcessWealth(kingdom, kData);
                UpdateDebtManagement(kingdom, kData, manager);
                
                UpdatePolicies(kingdom, kData);
                CheckRevolt(kingdom, kData);

                // Record History
                if(Time.frameCount % 60 == 0) {
                     kData.WealthHistory.Add(kData.Wealth);
                     if(kData.WealthHistory.Count > 100) kData.WealthHistory.RemoveAt(0);
                     
                     kData.CurrencyHistory.Add(kData.CurrencyValue);
                     if(kData.CurrencyHistory.Count > 100) kData.CurrencyHistory.RemoveAt(0);
                }
            }
        }

        private static void InitializeKingdomAssets(Kingdom kingdom, KingdomEconomyData kData)
        {
             if (string.IsNullOrEmpty(kData.CurrencyID) || kData.CurrencyID == "Gold")
            {
                if (IconCurrency != null && IconCurrency.Count > 0)
                {
                    string randomId = IconCurrency[UnityEngine.Random.Range(0, IconCurrency.Count)];
                    kData.CurrencyID = randomId;
                    kData.CurrencyIcon = randomId; 
                    kData.CurrencyName = randomId; 
                }
                else if (AssetManager.resources.list.Count > 0)
                {
                    var randomRes = AssetManager.resources.list[UnityEngine.Random.Range(0, AssetManager.resources.list.Count)];
                    kData.TargetResource = randomRes.id;
                    kData.CurrencyID = randomRes.id;
                    kData.CurrencyIcon = randomRes.id;
                    
                    string[] suffixes = new string[] { "Standard", "Mark", "Crown", "Guilder", "Franc", "Yen" };
                    string suffix = suffixes[UnityEngine.Random.Range(0, suffixes.Length)];
                    string resName = char.ToUpper(kData.CurrencyID[0]) + kData.CurrencyID.Substring(1);
                    kData.CurrencyName = $"{resName} {suffix}";
                }
                else 
                { 
                    kData.CurrencyID = "gold"; 
                    kData.TargetResource = "gold";
                    kData.CurrencyIcon = "gold";
                    kData.CurrencyName = "Gold Standard";
                }
            }

            if (!kData.Initialized)
            {
                if (kingdom.getPopulationTotal() > 50)
                {
                    float humanCapital = kingdom.getPopulationTotal() * 10f;
                    float resourceCapital = 0;
                    if (kingdom.capital != null) resourceCapital = kingdom.capital.getResourcesAmount("gold");
                    
                    float seedEquity = humanCapital + resourceCapital;
                    if (seedEquity < 100) seedEquity = 100;

                    kData.CurrencySupply = seedEquity;
                    kData.GoldReserves = resourceCapital;
                    
                    if (kData.GoldReserves < 100) kData.GoldReserves = 100;
                    
                    kData.CurrencyValue = 1.0f; 
                    // Debug.Log($"[EconomyBox] Seed Capital for {kingdom.name}: Supply={kData.CurrencySupply}, Reserves={kData.GoldReserves}");
                }
                
                kData.CurrencyIssuerID = kingdom.id.ToString();
                kData.Initialized = true;
            }
        }

        private static void CalculateWealth(Kingdom kingdom, KingdomEconomyData kData, WorldDataManager manager)
        {
            kData.OldWealth = kData.Wealth;
            if(kData.OldWealth < 1f && kData.Wealth > 1f) kData.OldWealth = kData.Wealth;
            
            if (kingdom.capital == null) 
            {
                kData.Wealth = 0;
                return;
            }

            float totalGDP = 0;
            foreach(var city in kingdom.cities)
            {
                totalGDP += WealthManager.CalculateCityValue(city, manager);
            }
            
            float warMod = kingdom.hasEnemies() ? 0.9f : 1.0f; 
            float peaceMod = kingdom.hasAlliance() ? 1.1f : 1.0f; 
            
            kingdom.data.get("WealthBonus", out int godBonus, 0);
            
            float corruptionMod = 1.0f;
            kData.Corruption = 0f; 
            if (kingdom.king != null && kingdom.king.hasTrait("Corrupt"))
            {
                corruptionMod = 0.85f; 
                kData.Corruption = 0.15f;
            }

            // BUFF: 1.5x multiplier to Base GDP to strengthen economies
            float baseGDP = ((totalGDP * warMod * peaceMod) + godBonus) * 1.5f * corruptionMod;

            float ageMod = 1.0f;
            string currentAge = manager.CurrentAgeID;

            int seed = (kingdom.id.ToString() + currentAge).GetHashCode();
            System.Random rnd = new System.Random(seed);
            float variance = (float)(rnd.NextDouble() * 0.30 - 0.15); 

            if(currentAge == "The Big Sad") {
                ageMod = 0.4f; 
                if(kData.EconomicSystem == "Capitalism") ageMod -= 0.15f; 
                 ageMod += variance;
            }
            else if(currentAge == "Bank Failure")
            {
                ageMod = 0.7f;
                 if(kData.HasBank) ageMod -= 0.25f; 
                 ageMod += variance;
            }
            else if (currentAge == "Roaring 20s")
            {
                ageMod = 1.5f; 
                if (kData.EconomicSystem == "Capitalism") ageMod += 0.2f; 
                ageMod += variance;
            }
            else if (currentAge == "Gold Rush")
            {
                ageMod = 1.3f;
                ageMod += variance;
            }
            else if (currentAge == "Economic bubble")
            {
                ageMod = 2.0f; 
                ageMod += variance;
            }
            
            if(ageMod < 0.1f) ageMod = 0.1f;
            
            baseGDP *= ageMod;

            float trend = Mathf.PerlinNoise(Time.time * 0.01f, 0.0f); 
            float marketTrend = 0.8f + (trend * 0.4f);

            float vix = Mathf.PerlinNoise(Time.time * 0.05f, 50.0f); 
            float currentVolatility = 0.02f + (vix * 0.15f); 

            float dailyJitter = UnityEngine.Random.Range(-currentVolatility, currentVolatility);

            if (manager.CurrentTickTradeVolume > 5000) dailyJitter *= 0.1f; 
            else if (manager.CurrentTickTradeVolume > 1000) dailyJitter *= 0.5f;

            float marketFactor = marketTrend + dailyJitter;
            
            float pppModifier = Mathf.Clamp(kData.CurrencyValue, 0.2f, 2.0f);
            
            kData.Wealth = (baseGDP * marketFactor) * pppModifier;
            
            kData.GoldReserves = 0;
            foreach(var city in kingdom.cities)
            {
                kData.GoldReserves += city.getResourcesAmount("gold");
            }
        }

        // Consolidated from CentralBank
        public static float CalculateCurrencyValue(Kingdom k, WorldDataManager manager)
        {
            var data = manager.GetKingdomData(k);
            if (data == null) return 1.0f;
            
            float backing = data.GoldReserves + data.Wealth;
            
            if (data.CurrencySupply <= 0) data.CurrencySupply = 100f; 
            
            float rawValue = backing / data.CurrencySupply;
            
            float debtRatio = 0f;
            if(data.Wealth > 0) debtRatio = data.NationalDebt / data.Wealth;
            
            // BUFF: Debt penalty capped at 30% instead of 50%, and requires more debt to trigger
            float debtPenalty = 1.0f - Mathf.Clamp((debtRatio - 0.2f) * 0.5f, 0f, 0.3f); 
            
            float phaseMod = 1.0f;
            switch(data.CurrentPhase) {
                case EconomicPhase.Expansion: phaseMod = 1.1f; break; 
                case EconomicPhase.Peak: phaseMod = 1.3f; break;      
                case EconomicPhase.Contraction: phaseMod = 0.7f; break; 
                case EconomicPhase.Recovery: phaseMod = 0.9f; break; 
            }
            
            float noise = UnityEngine.Random.Range(0.90f, 1.10f);
            
            if (rawValue < 0.01f) rawValue = 0.01f;
            
            float calculatedValue = rawValue * debtPenalty * phaseMod * noise;
            
            if (calculatedValue > 5.0f) 
            {
                float excess = calculatedValue - 5.0f;
                calculatedValue = 5.0f + (Mathf.Log10(excess + 1f) * 2.0f);
            }
            
            return calculatedValue;
        }

        // Consolidated from CentralBank
        public static void PrintMoney(Kingdom k, float amount, WorldDataManager manager)
        {
            var data = manager.GetKingdomData(k);
            if (data == null) return;

            if (k.capital != null)
            {
                ChangeResource(k.capital, "gold", (int)amount);
            }
            
            data.CurrencySupply += amount;
            
            // Recalculate Value
            data.CurrencyValue = CalculateCurrencyValue(k, manager);
        }

        private static void HandleInflation(Kingdom kingdom, KingdomEconomyData kData, WorldDataManager manager)
        {
            if (kingdom.capital == null) return;

            float population = kingdom.getPopulationTotal();
            float armyStr  = (population * 0.2f) * 10f; 
            
            float economicBacking = (kData.Wealth * 1.0f) + (kData.GoldReserves * 2.0f); 

            float totalAllianceDebt = 0f;
            foreach(var k in MapBox.instance.kingdoms.list) {
                 if(!k.isCiv()) continue;
                 var kd = manager.GetKingdomData(k);
                 if(kd != null && kd.CurrencyID == kData.CurrencyID) {
                     totalAllianceDebt += kd.NationalDebt;
                 }
            }
            
            float debtDrag = totalAllianceDebt * 0.2f; 
            economicBacking -= debtDrag;

            float minBacking = Mathf.Max(300f, manager.GlobalAverageGold * 0.1f);
            if(economicBacking < minBacking) economicBacking = minBacking;
            
            float demographicBacking = (population * 5f) + armyStr;
            
            if (kData.CurrencySupply <= 0) kData.CurrencySupply = 100f; 
            float rawValue = (economicBacking + demographicBacking) / kData.CurrencySupply;
            
            if (rawValue > 5.0f) rawValue = 5.0f + (rawValue - 5.0f) * 0.1f; 
            
            float trustMod = 0.5f + (kData.CreditScore / 100f); 
            
            kData.CurrencyValue = rawValue * trustMod;
            
            bool desperate = kData.GoldReserves < (kData.Expenses * 2f);
            
            long myId = Convert.ToInt64(kingdom.id);
            
            if (kData.CurrencyIssuerID == "0" || string.IsNullOrEmpty(kData.CurrencyIssuerID)) kData.CurrencyIssuerID = kingdom.id.ToString();
            
            if (desperate && kData.CurrencyIssuerID == kingdom.id.ToString())
            {
                float printAmount = kData.Expenses * 2f; 
                float maxPrint = kData.CurrencySupply * 0.2f; 
                
                 float smallPopThresh = Mathf.Max(50f, manager.GlobalAveragePop * 0.5f);
                if(kingdom.getPopulationTotal() < smallPopThresh) maxPrint = kData.CurrencySupply * 0.05f; 
                
                if (printAmount > maxPrint) printAmount = maxPrint;
                
                PrintMoney(kingdom, printAmount, manager);
                EconomyLogger.LogVerbose($"[PRINT] {kingdom.name} prints {printAmount:N0} (Panic Mode). New Supply: {kData.CurrencySupply:N0}.");
            }
            
            float growth = 0;
            if(kData.OldWealth > 0) growth = (kData.Wealth - kData.OldWealth) / kData.OldWealth;

            if (growth > 0.05f && kData.CurrencyValue > 3.0f && kData.CurrencyIssuerID == kingdom.id.ToString() && !desperate)
            {
                float naturalPrint = kData.CurrencySupply * 0.02f;
                PrintMoney(kingdom, naturalPrint, manager);
                EconomyLogger.LogVerbose($"[MINT] {kingdom.name} naturally mints {naturalPrint:N0} to match growth ({growth*100:F1}%).");
            }
            
            if (kData.CurrencyValue < 0.3f)
            {
                kData.HyperinflationTimer += 1f;
                if (kData.HyperinflationTimer > 60f) 
                {
                     foreach(var city in kingdom.cities) {
                         try {
                              float currentLoyalty = (float)Reflection.GetField(city.data.GetType(), city.data, "loyalty");
                              Reflection.SetField(city.data, "loyalty", currentLoyalty - 50f);
                         } catch {}
                     }
                     kData.HyperinflationTimer = 0f; 

                     float newSupply = kData.Wealth;
                     if(newSupply < 100) newSupply = 100;
                     kData.CurrencySupply = newSupply;
                     kData.CurrencyValue = 1.0f; 
                     EconomyLogger.LogVerbose($"[REFORM] {kingdom.name} enacts Currency Reform! Supply Reset to {newSupply:N0}.");
                }
            }
            else
            {
                if (kData.HyperinflationTimer > 0) kData.HyperinflationTimer -= 0.5f;
            }

            if (kData.CurrencyValue < 0.01f) kData.CurrencyValue = 0.01f;
            
            CheckForCurrencyAdoption(kingdom, kData, manager);
        }

        public static void MakeAlliance(Kingdom source, Kingdom target)
        {
            var manager = WorldDataManager.Instance;
            var sData = manager.GetKingdomData(source);
            var tData = manager.GetKingdomData(target);
            
            if(sData == null || tData == null) return;
            
            string oldName = tData.CurrencyName;
            tData.CurrencyID = sData.CurrencyID;
            tData.CurrencyName = sData.CurrencyName;
            tData.CurrencyIcon = sData.CurrencyIcon;
            
            tData.CurrencyIssuerID = sData.CurrencyIssuerID;
            
            float realValue = tData.CurrencySupply * tData.CurrencyValue;
            float targetVal = sData.CurrencyValue;
            if(targetVal < 0.01f) targetVal = 0.01f;
            
            float newSupply = realValue / targetVal;
            if(newSupply < 100f) newSupply = 100f;
            
            tData.CurrencySupply = newSupply;
            tData.CurrencyValue = sData.CurrencyValue; 
            
            tData.HyperinflationTimer = 0;
        }
        
        public static void DissolveAlliance(Kingdom kingdom)
        {
            var manager = WorldDataManager.Instance;
            var kData = manager.GetKingdomData(kingdom);
            if(kData == null) return;

            int allianceCount = 0;
            foreach(var otherK in manager.KingdomData.Values)
            {
                if(otherK.CurrencyID == kData.CurrencyID) allianceCount++;
            }

            if(allianceCount <= 1) return; 

            // Generate NEW Unique ID
            string newId = "Curr_" + kingdom.id + "_" + UnityEngine.Random.Range(0, 100000);
            
            kData.CurrencyID = newId;
            kData.CurrencyIssuerID = kingdom.id.ToString(); 

            // Randomize Visuals from defaults or list
            if (IconCurrency.Count > 0)
            {
                string randomId = IconCurrency[UnityEngine.Random.Range(0, IconCurrency.Count)];
                kData.CurrencyIcon = randomId;
                kData.CurrencyName = "The " + System.Text.RegularExpressions.Regex.Replace(randomId, "(\\B[A-Z])", " $1");
            }
            else
            {
                 kData.CurrencyIcon = "gold";
                 kData.CurrencyName = $"{kingdom.name} Gold";
            }
        }

        public static void AdoptCurrencyVisuals(Kingdom thief, string iconName, string currencyName)
        {
             var manager = WorldDataManager.Instance;
             var tData = manager.GetKingdomData(thief);
             if(tData == null) return;
             
             tData.CurrencyID = "Curr_" + thief.id + "_" + UnityEngine.Random.Range(0, 9999);
             tData.CurrencyIcon = iconName; 
             tData.CurrencyName = currencyName;
        }

        public static void StealCurrencyVisuals(Kingdom thief, Kingdom victim)
        {
             var manager = WorldDataManager.Instance;
            var tData = manager.GetKingdomData(thief);
            var vData = manager.GetKingdomData(victim);
            
            if(tData == null || vData == null) return;
            
            string stealIcon = vData.CurrencyIcon;
            string stealName = vData.CurrencyName;
            
            tData.CurrencyID = "Curr_" + thief.id + "_" + UnityEngine.Random.Range(0, 9999);
            tData.CurrencyIcon = stealIcon;
            tData.CurrencyName = stealName;
            
            DissolveAlliance(victim); 
        }
        
        private static void CheckForCurrencyAdoption(Kingdom kingdom, KingdomEconomyData kData, WorldDataManager manager)
        {
            if (kData.CurrencySupply > 1000000f || kData.CurrencyValue < 0.05f)
            {
                Kingdom savior = null;
                float bestMetric = 0f;

                foreach(var k in MapBox.instance.kingdoms.list)
                {
                    if(k == kingdom) continue;
                    if(k.getAlliance() == null || k.getAlliance() != kingdom.getAlliance()) continue;
                    
                    var sData = manager.GetKingdomData(k);
                    if(sData == null) continue;

                    if(sData.CurrencyValue < 0.9f) continue;
                    if(sData.CurrencySupply > 50000f) continue; 
                    
                    if(sData.Wealth > kData.Wealth * 2f && sData.Wealth > bestMetric)
                    {
                        bestMetric = sData.Wealth;
                        savior = k;
                    }
                }

                if(savior != null)
                {
                    var sData = manager.GetKingdomData(savior);
                    EconomyLogger.LogVerbose($"CURRENCY REFORM: {kingdom.name} abandons {kData.CurrencyName} and adopts {sData.CurrencyName} from {savior.name}!");
                    
                    kData.CurrencyID = sData.CurrencyID;
                    kData.CurrencyName = sData.CurrencyName;
                    kData.CurrencyIcon = sData.CurrencyIcon;
                    kData.CurrencyIssuerID = savior.id.ToString();
                    
                    float realValue = kData.CurrencySupply * kData.CurrencyValue;
                    float newSupply = realValue / sData.CurrencyValue;
                    
                    if(newSupply < 100f) newSupply = 100f;
                    
                    kData.CurrencySupply = newSupply;
                    kData.CurrencyValue = sData.CurrencyValue;
                    
                    kData.HyperinflationTimer = 0;
                }
            }
        }

        private static void UpdateBanks(Kingdom kingdom, KingdomEconomyData kData)
        {
             if (kingdom.capital != null && kingdom.capital.countBuildingsType("Bank") > 0)
            {
                kData.HasBank = true;
            }
        }
        private static void UpdateCycles(Kingdom kingdom, KingdomEconomyData kData)
        {
            float dt = 1.0f; // 1 tick
            kData.PhaseTimer -= dt;
            
            if (kData.PhaseTimer <= 0)
            {
                switch (kData.CurrentPhase)
                {
                    case EconomicPhase.Expansion:
                        kData.CurrentPhase = EconomicPhase.Peak;
                        kData.PhaseTimer = UnityEngine.Random.Range(20f, 40f);
                        break;
                    case EconomicPhase.Peak:
                        kData.CurrentPhase = EconomicPhase.Contraction;
                        kData.PhaseTimer = UnityEngine.Random.Range(30f, 60f); 
                        break;
                    case EconomicPhase.Contraction:
                        kData.CurrentPhase = EconomicPhase.Recovery;
                        kData.PhaseTimer = UnityEngine.Random.Range(30f, 50f);
                        break;
                    case EconomicPhase.Recovery:
                        kData.CurrentPhase = EconomicPhase.Expansion;
                        kData.PhaseTimer = UnityEngine.Random.Range(50f, 100f); 
                        break;
                }
            }
        }
        
        private static void UpdateResourceNeeds(Kingdom kingdom, KingdomEconomyData kData)
        {
            if (kingdom.capital == null) return;
            
            if (kData.NationalDebt > 100 || kData.OutstandingLoans.Count > 0)
            {
                if (kData.GoldReserves < kData.NationalDebt * 0.5f)
                {
                    kData.TargetResource = "gold";
                    return;
                }
            }

            float lifestyle = 0.5f;
            float luxuryThresh = Mathf.Max(1000f, WorldDataManager.Instance.GlobalAverageWealth);
            if(kData.Wealth > luxuryThresh) 
            {
                lifestyle += (kData.Wealth / 100000f);
            }
            if (lifestyle > 20f) lifestyle = 20f;

            int needs = (int)(kingdom.getPopulationTotal() * lifestyle);
            if (needs < 20) needs = 20; 
            
            int food = kingdom.capital.getResourcesAmount("wheat") + kingdom.capital.getResourcesAmount("bread");
            if (food < needs) { kData.TargetResource = "wheat"; return; }
            
            int wood = kingdom.capital.getResourcesAmount("wood");
            if (wood < needs) { kData.TargetResource = "wood"; return; }
            
            int stone = kingdom.capital.getResourcesAmount("stone");
            if (stone < needs) { kData.TargetResource = "stone"; return; }
            
            int iron = kingdom.capital.getResourcesAmount("iron");
            if (iron < (needs / 2)) { kData.TargetResource = "iron"; return; }
            
            int leather = kingdom.capital.getResourcesAmount("leather");
            if (leather < (needs / 2)) { kData.TargetResource = "leather"; return; }
            
            int pie = kingdom.capital.getResourcesAmount("pie");
            if (pie < (needs / 4)) { kData.TargetResource = "pie"; return; }
            int tea = kingdom.capital.getResourcesAmount("tea");
            if (tea < (needs / 4)) { kData.TargetResource = "tea"; return; }
            int fish = kingdom.capital.getResourcesAmount("fish");
            if (fish < (needs / 2)) { kData.TargetResource = "fish"; return; }
            
            int mithril = kingdom.capital.getResourcesAmount("mithril");
            if (mithril < 5) { kData.TargetResource = "mithril"; return; }
            int adamantine = kingdom.capital.getResourcesAmount("adamantine");
            if (adamantine < 5) { kData.TargetResource = "adamantine"; return; }
            int gem = kingdom.capital.getResourcesAmount("gem");
            if (gem < 5) { kData.TargetResource = "gem"; return; }
            
            if (kData.Wealth > 5000 && kData.GoldReserves > 1000 && UnityEngine.Random.value < 0.10f) 
            {
                 string[] targets = new string[] { "gem", "mithril", "adamantine", "tea", "pie" };
                 kData.MonopolyResource = targets[UnityEngine.Random.Range(0, targets.Length)];
                 kData.IsMonopolyActive = true;
                 kData.TargetResource = kData.MonopolyResource;
                 return;
            }
            if(string.IsNullOrEmpty(kData.MonopolyResource)) kData.IsMonopolyActive = false;
            
            if(kData.IsMonopolyActive) {
                kData.TargetResource = kData.MonopolyResource;
                return;
            }

            kData.TargetResource = "gold";
        }

        private static void ApplyAusterity(Kingdom kingdom) {
            foreach(var city in kingdom.cities) {
                if(UnityEngine.Random.value < 0.5f) continue; 
                try {
                    int currentGold = city.getResourcesAmount("gold");
                    if(currentGold > 10) {
                        int seized = (int)(currentGold * 0.1f);
                        ChangeResource(city, "gold", -seized);
                    }
                } catch { }
            }
        }

        private static void UpdateFiscalPolicy(Kingdom kingdom, KingdomEconomyData kData)
        {
            if (kData.CurrentPolicy == KingdomPolicy.FreeMarket)
            {
                if(UnityEngine.Random.value < 0.1f) kData.Wealth += 10; 
                if(UnityEngine.Random.value < 0.10f) kData.Corruption += 0.01f;
            }
            if (kData.Wealth > 0 && (kData.NationalDebt / kData.Wealth) > 0.8f) 
            {
                 if(UnityEngine.Random.value < 0.05f) kData.Corruption += 0.01f;
            }
            else if (kData.CurrentPolicy == KingdomPolicy.PlannedEconomy)
            {
                if(kData.Corruption > 0) kData.Corruption -= 0.01f;
                if(kData.Corruption < 0) kData.Corruption = 0;
                
                if(UnityEngine.Random.value < 0.1f && kData.Wealth > 100) kData.Wealth -= 5;
            }
            else if (kData.CurrentPolicy == KingdomPolicy.Isolationism)
            {
                if(kData.CurrencySupply > 1000 && UnityEngine.Random.value < 0.05f) 
                {
                    kData.CurrencySupply -= 10; 
                }
            }
            else if (kData.CurrentPolicy == KingdomPolicy.Austerity)
            {
                // Austerity: Cut spending, reduce debt faster, but hurts growth
                if(kData.NationalDebt > 0 && UnityEngine.Random.value < 0.15f) {
                    float debtReduction = Mathf.Min(kData.NationalDebt * 0.05f, 100f);
                    kData.NationalDebt -= debtReduction;
                    if(kData.NationalDebt < 0) kData.NationalDebt = 0;
                }
                // Reduced growth during austerity
                if(UnityEngine.Random.value < 0.1f && kData.Wealth > 100) kData.Wealth -= 3;
                // Government spending cuts
                ApplyAusterity(kingdom);
            }
            else if (kData.CurrentPolicy == KingdomPolicy.Stimulus)
            {
                // Stimulus: Boost growth but increase debt
                if(UnityEngine.Random.value < 0.15f) {
                    float stimulusBoost = Mathf.Max(kData.Wealth * 0.02f, 20f);
                    kData.Wealth += stimulusBoost;
                    kData.NationalDebt += stimulusBoost * 0.5f; // Deficit spending
                }
                // Chance for inflation
                if(UnityEngine.Random.value < 0.05f) {
                    kData.CurrencySupply += 50;
                }
            }

            if(kData.Corruption > 1f) kData.Corruption = 1f;
            if (kingdom.capital == null) return;

            float upkeepPerCity = 5f;
            float upkeepPerSoldier = 0.5f; 
            int armySize = (int)(kingdom.getPopulationTotal() * 0.10f);
            
            float totalUpkeep = (kingdom.cities.Count * upkeepPerCity) + (armySize * upkeepPerSoldier);
            
            if (kData.CurrentPhase == EconomicPhase.Contraction) 
            {
                totalUpkeep *= 1.2f; 
            }

            float interestRate = 0.01f; 
            float interestPayment = kData.NationalDebt * interestRate;
            totalUpkeep += interestPayment;

            kData.Expenses = totalUpkeep;
            
            float grossRevenue = kData.Wealth * kData.TaxRate; 
            
            float fiscalBalance = grossRevenue - totalUpkeep;
            
            if (fiscalBalance > 0)
            {
                if (kData.NationalDebt > 0) {
                    float repayment = fiscalBalance * 0.5f;
                    kData.NationalDebt -= repayment;
                    kData.GoldReserves += (fiscalBalance - repayment);
                } else {
                    kData.GoldReserves += fiscalBalance;
                }
            }
            else
            {
                float deficit = -fiscalBalance;
                kData.GoldReserves -= deficit; 
                
                if (kData.GoldReserves < 0)
                {
                    float bondAmount = deficit + 100; 
                    kData.NationalDebt += bondAmount;
                    kData.GoldReserves += bondAmount; 
                }
            }  

            if (kData.GoldReserves < 0)
            {
                kData.CreditScore -= 5f;
                if (kData.NationalDebt > kData.Wealth * 3f) {
                     ApplyAusterity(kingdom);
                }
            }
            
            if (UnityEngine.Random.value < 0.02f)
            {
                float lossPct = UnityEngine.Random.Range(0.1f, 0.25f);
                float loss = kData.GoldReserves * lossPct;
                if (loss > 10) {
                    kData.GoldReserves -= loss;
                }
            }
            else
            {
                if(kData.CreditScore < 100) kData.CreditScore += 1f;
            }
        }
        private static void UpdateDebtManagement(Kingdom kingdom, KingdomEconomyData kData, WorldDataManager manager)
        {
            if(kData.Wealth < 100 || kData.CurrencyValue < 0.4f)
            {
               manager.AttemptLoanRequest(kingdom);
            }

            bool healthy = kData.Wealth > 2000 && kData.NationalDebt < (kData.Wealth * 0.2f) && kData.CreditScore > 80;
            if (healthy && UnityEngine.Random.value < 0.01f)
            {
                 manager.AttemptLoanRequest(kingdom);
            }
        }

        public static void UpdatePolicies(Kingdom k, KingdomEconomyData data)
        {
            if(UnityEngine.Random.value > 0.10f) return;

            KingdomPolicy current = data.CurrentPolicy;
            KingdomPolicy target = current;

            float debtRatio = 0;
            if(data.Wealth > 0) debtRatio = data.NationalDebt / data.Wealth;
            
            float growth = 0;
            if(data.OldWealth > 0) growth = (data.Wealth - data.OldWealth) / data.OldWealth;

            if (debtRatio > 0.5f)
            {
                target = KingdomPolicy.Isolationism;
            }
            else if (growth > 0.10f && debtRatio < 0.1f)
            {
                target = KingdomPolicy.FreeMarket;
            }
            else if (data.Corruption > 0.2f || k.getPopulationTotal() < Mathf.Max(20f, WorldDataManager.Instance.GlobalAveragePop * 0.2f)) 
            {
                target = KingdomPolicy.PlannedEconomy;
            }
            
            if (target != current)
            {
                data.CurrentPolicy = target;
                EconomyLogger.LogVerbose($"[POLICY] {k.name} switches from {current} to {target}.");
            }
            
            if (k.king != null)
            {
                if (data.CurrentPhase == EconomicPhase.Contraction || data.CurrentPhase == EconomicPhase.Recovery)
                {
                     k.king.addStatusEffect("Recession");
                }
                else
                {
                     k.king.addStatusEffect("Expansion");
                }
            }
        }

        private static void CheckRevolt(Kingdom kingdom, KingdomEconomyData kData)
        {
            if (kData.BadEconomyTimer > 30f) 
            {
                kData.BadEconomyTimer = 0f;
                
                int madCount = 0;
                foreach(var city in kingdom.cities)
                {
                    foreach(var unit in city.units.ToList()) 
                    {
                        // Increased form 20% to 40% for a "harder" revolt
                        if(UnityEngine.Random.value < 0.9f)
                        {
                            //unit.addTrait("madness");
                            //unit.addTrait("powerup"); // Make them stronger
                            //if(UnityEngine.Random.value < 0.3f) unit.addTrait("veteran"); // Experienced fighters

                            // Increased chaos traits
                            //if(UnityEngine.Random.value < 0.5f) unit.addTrait("pyromaniac");
                            //if(UnityEngine.Random.value < 0.2f) unit.addTrait("bomberman");
                            
                            madCount++;
                        }
                    }
                }
                EconomyLogger.LogVerbose($"MASS REVOLT in {kingdom.name}: {madCount} citizens went mad and dangerous!");
            }
            
             bool isCrisis = kData.CurrencyValue < 0.2f || (kData.NationalDebt > kData.Wealth * 5f);
             if(isCrisis)
             {
                 kData.BadEconomyTimer += 1.0f; 
             }
             else
             {
                 if(kData.BadEconomyTimer > 0) kData.BadEconomyTimer -= 1.0f;
             }
        }
        public static void HandleExcessWealth(Kingdom kingdom, KingdomEconomyData kData)
        {
            float dynamicThreshold = Mathf.Max(5000f, WorldDataManager.Instance.GlobalAverageGold * 3f);
            
            if (kData.GoldReserves > dynamicThreshold && kingdom.capital != null)
            {
                int goldToSpend = (int)(kData.GoldReserves * 0.05f);
                if (goldToSpend > 0)
                {
                    ChangeResource(kingdom.capital, "gold", -goldToSpend);
                    
                    if (UnityEngine.Random.value < 0.2f)
                    {
                        kingdom.addRenown(1);
                    }
                }
            }
        }
    }
}

