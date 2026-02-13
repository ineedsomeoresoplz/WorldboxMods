using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
using NCMS;

namespace AIBox
{
    public static class KingdomActions
    {
        // --- 1. SPENDING (Infrastructure & Happiness) ---
        public static void InvestInCity(Kingdom kingdom, int amount)
        {
            if(kingdom.cities.Count == 0) return;
            
            var data = WorldDataManager.Instance.GetKingdomData(kingdom);
            if(data.GoldReserves < amount) return; 
            
            data.GoldReserves -= amount;
            
            int share = amount / kingdom.cities.Count;
            if(share < 1) share = 1;

            foreach(City c in kingdom.cities) {
                SimulationGameloop.ChangeResource(c, "gold", share);
                ModifyLoyalty(c, 5f);
            }
            
            EconomyLogger.LogVerbose($"[ACTION] {kingdom.name} invests {amount} gold into infrastructure.");
        }

        public static void HoldFestival(Kingdom kingdom)
        {
             var data = WorldDataManager.Instance.GetKingdomData(kingdom);
             int cost = (int)(kingdom.getPopulationTotal() * 2f); 
             if(cost < 100) cost = 100;
             
             if(data.GoldReserves < cost) return;
             
             data.GoldReserves -= cost;
             
             foreach(City c in kingdom.cities) {
                 ModifyLoyalty(c, 30f);
             }
        }

        // --- 2. ARMY MANAGEMENT ---
        public static void DisbandRegiment(Kingdom kingdom, int count)
        {
            int removed = 0;
            for(int i = kingdom.units.Count - 1; i >= 0; i--) {
                if(removed >= count) break;
                
                Actor unit = kingdom.units[i];
                if(unit == null || !unit.isAlive()) continue;
                
                if(unit.isWarrior()) {
                    bool demoted = false;
                    try {
                        unit.setProfession(UnitProfession.Unit);
                        demoted = true;
                    } catch { }
                    
                    if(demoted) {
                        removed++;
                    }
                }
            }
            
            if(removed > 0) {
                EconomyLogger.LogVerbose($"[ACTION] {kingdom.name} disbands {removed} soldiers to save upkeep.");
            }
        }

        // --- 3. TRADE (Physical Resources) ---

        public static void ImportResource(Kingdom buyer, string resourceId, int amount)
        {
             var bData = WorldDataManager.Instance.GetKingdomData(buyer);
             if(bData == null) return;
             
             Kingdom seller = null;
             foreach(var k in MapBox.instance.kingdoms.list) {
                 if(!k.isCiv() || k == buyer || k.isEnemy(buyer)) continue;
                 
                 var sData = WorldDataManager.Instance.GetKingdomData(k);
                 if(sData != null && (sData.EmbargoList.Contains(buyer.name) || bData.EmbargoList.Contains(k.name))) continue;
                 
                 int stock = GetTotalResource(k, resourceId);
                 if(stock > amount * 2) {
                     seller = k;
                     break;
                 }
             }
             
             if(seller == null) return; 
             
             int cost = amount; 
             
             if(bData.GoldReserves < cost) return;
             
             bData.GoldReserves -= cost;
             var sellData = WorldDataManager.Instance.GetKingdomData(seller);
             if(sellData != null) sellData.GoldReserves += cost;
             
             RemoveResourceFromKingdom(seller, resourceId, amount);
             AddResourceToKingdom(buyer, resourceId, amount);
             
             EconomyLogger.LogVerbose($"[TRADE] {buyer.name} imports {amount} {resourceId} from {seller.name} for {cost} gold.");
        }

        // --- 4. DIPLOMACY (Economic) ---

        public static void EnforceEmbargo(Kingdom source, Kingdom target)
        {
             var data = WorldDataManager.Instance.GetKingdomData(source);
             if(data == null) return;
             
             if(!data.EmbargoList.Contains(target.name)) {
                 data.EmbargoList.Add(target.name);
             }
        }

        public static void LiftEmbargo(Kingdom source, Kingdom target)
        {
             var data = WorldDataManager.Instance.GetKingdomData(source);
             if(data == null) return;
             
             if(data.EmbargoList.Contains(target.name)) {
                 data.EmbargoList.Remove(target.name);
             }
        }

        public static void ProposeAlliance(Kingdom source, Kingdom target)
        {
            if(source == target) return;
            if(source.isEnemy(target)) return;

            // Check Opinion - STRICTER REQUIREMENTS
            int score = GetRelationScore(source, target);
            
            // MINIMUM THRESHOLD: Opinion must be at least 25 to even consider alliance
            const int MINIMUM_OPINION = 25;
            const int FREE_ALLIANCE_OPINION = 50; // Was 30, now requires better relations
            
            if (score < MINIMUM_OPINION) {
                Debug.Log($"[AIBox] {source.name} cannot ally {target.name} - Opinion {score} below minimum {MINIMUM_OPINION}. Build relationship first!");
                return;
            }

            Action makeAlliance = () => {
                // 1. Economic Alliance (Coin Union)
                SimulationGameloop.MakeAlliance(source, target);
                
                // 2. Vanilla Diplomatic Alliance
                bool success = false;
                try {
                    if (!source.hasAlliance() && !target.hasAlliance()) {
                        World.world.alliances.newAlliance(source, target);
                        success = true;
                    }
                    else if (source.hasAlliance() && !target.hasAlliance()) {
                        source.getAlliance().join(target);
                        success = true;
                    }
                    else if (!source.hasAlliance() && target.hasAlliance()) {
                        target.getAlliance().join(source);
                        success = true;
                    }
                } catch { 
                     try {
                        Reflection.CallMethod(source, "tryToMakeAlliance", target);
                     } catch {}
                }
                
                if(success) {
                    Debug.Log($"[AIBox] ALLIANCE FORMED: {source.name} <-> {target.name} (Opinion: {score})");
                    KingdomController.Instance.RequestGlobalBriefing();
                }
            };

            if (score >= FREE_ALLIANCE_OPINION) {
                // High opinion = free alliance!
                DiplomacyPatch.AllowDiplomacy = true;
                try {
                    makeAlliance();
                } finally {
                    DiplomacyPatch.AllowDiplomacy = false;
                }
            } else {
                // Medium opinion (25-49): Need to bribe - costs MORE gold now
                int opinionGap = FREE_ALLIANCE_OPINION - score;
                int goldDemand = opinionGap * 20; // Was 10, now 20 per point
                
                var sData = WorldDataManager.Instance.GetKingdomData(source);
                if(sData != null && sData.GoldReserves >= goldDemand) {
                    sData.GoldReserves -= goldDemand;
                    // Give to target
                    var tData = WorldDataManager.Instance.GetKingdomData(target);
                    if(tData != null) tData.GoldReserves += goldDemand;

                    Debug.Log($"[AIBox] {source.name} paid {goldDemand}g to ally {target.name} (Opinion: {score})");
                    
                    DiplomacyPatch.AllowDiplomacy = true;
                    try {
                        makeAlliance();
                    } finally {
                        DiplomacyPatch.AllowDiplomacy = false;
                    }
                } else {
                    Debug.Log($"[AIBox] {source.name} cannot afford to ally {target.name} - Need {goldDemand}g, have {sData?.GoldReserves ?? 0}g");
                }
            }
        }

        public static void CallAlliedEmbargo(Kingdom source, Kingdom target)
        {
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData == null) return;

            foreach(var k in World.world.kingdoms.list) {
                if(!k.isCiv() || k == target || k == source) continue;
                
                // Are we allies? (Coin union check)
                var kData = WorldDataManager.Instance.GetKingdomData(k);
                if(kData != null && kData.CurrencyID == sData.CurrencyID) {
                    if(!kData.EmbargoList.Contains(target.name)) {
                        kData.EmbargoList.Add(target.name);
                    }
                }
            }
        }

        public static void DeclareJustifiedWar(Kingdom attacker, Kingdom defender, string reason)
        {
            if(attacker.isEnemy(defender)) return;
            
            // --- SAFEGUARDS (PREVENT SUICIDE) ---
            var aData = WorldDataManager.Instance.GetKingdomData(attacker);
            if(aData != null) {
                // 1. Economic safeguard
                if(aData.GoldReserves < 100 && aData.Wealth < 500f) {
                    EconomyLogger.LogVerbose($"[BLOCK] {attacker.name} blocked from warring {defender.name}: Too poor.");
                    return;
                }
                
                // 2. Multi-war safeguard
                int currentWars = 0;
                var wars = World.world.wars.getWars(attacker);
                foreach(var w in wars) {
                    if(!w.hasEnded()) currentWars++;
                }

                if(currentWars >= 2) {
                    EconomyLogger.LogVerbose($"[BLOCK] {attacker.name} blocked from warring {defender.name}: Already fighting {currentWars} wars.");
                    return; 
                }
            }

            // 3. Power safeguard
            int myArmy = attacker.countTotalWarriors();
            int theirArmy = defender.countTotalWarriors();
            if(theirArmy > myArmy * 2 ) { // Only block if we are actually small/weak, giants can fight giants
                 EconomyLogger.LogVerbose($"[BLOCK] {attacker.name} blocked from warring {defender.name}: Target too strong ({theirArmy} vs {myArmy}).");
                 return;
            }
            
            // Check for shared alliance and dissolve if necessary (Betrayal)
            if (attacker.hasAlliance() && defender.hasAlliance())
            {
                var myAlliance = attacker.getAlliance();
                var theirAlliance = defender.getAlliance();
                
                if (myAlliance == theirAlliance)
                {
                    World.world.alliances.dissolveAlliance(myAlliance);
                }
            }
            
            if(aData != null) aData.LastWarReason = reason;

            try {
                // Try "whisper_of_war" first as per request, fallback to spite
                var warAsset = AssetManager.war_types_library.get("whisper_of_war");
                if (warAsset == null) warAsset = AssetManager.war_types_library.get("spite");

                if (warAsset != null) {
                    DiplomacyPatch.AllowDiplomacy = true;
                    try {
                        World.world.diplomacy.startWar(attacker, defender, warAsset, true);
                    } finally {
                        DiplomacyPatch.AllowDiplomacy = false;
                    }
                }
            } catch {
                Debug.LogWarning("[AIBox] Failed to declare war via reflection.");
            }
            
            KingdomController.Instance.RequestGlobalBriefing();

            // GLOBAL EVENT TRIGGER for AI Reaction
            string globalMsg = $"WAR_DECLARED|{attacker.name}|{defender.name}|{reason}";
            foreach(var k in World.world.kingdoms.list) {
                if(k.isCiv()) {
                    var kd = WorldDataManager.Instance.GetKingdomData(k);
                    if(kd != null) kd.RecentDiplomaticEvents.Add(globalMsg);
                }
            }

            // Political Fallout
            foreach(var other in World.world.kingdoms.list) {
                if(!other.isCiv() || other == attacker || other == defender) continue;
                var oData = WorldDataManager.Instance.GetKingdomData(other);
                
                int relationHit = 0;
                if(reason.ToLower().Contains("expansion") || reason.ToLower().Contains("conquest")) {
                    relationHit = -30; // Global distrust for conquerors
                } else if(reason.ToLower().Contains("defense") || reason.ToLower().Contains("liberation")) {
                    relationHit = -5; // Minor hit for instability
                } else {
                    relationHit = -15; // Unclear reasons cause suspicion
                }

                // Allies (Coin Union) are more forgiving
                if(oData != null && aData != null && oData.CurrencyID == aData.CurrencyID) {
                    relationHit /= 2; // Half penalty from allies
                }

                // Apply relation hit
                UpdateRelationWeight(other, attacker, relationHit);
            }
        }

        private static void UpdateRelationWeight(Kingdom k1, Kingdom k2, int delta)
        {
            try {
                var relation = World.world.diplomacy.getRelation(k1, k2);
                 object relObj = Reflection.CallMethod(k1, "getRelation", k2);
                 if(relObj != null) {
                    int current = (int)Reflection.GetField(relObj.GetType(), relObj, "weights");
                    Reflection.SetField(relObj, "weights", current + delta);
                 }
            } catch {}
        }

        private static int GetRelationScore(Kingdom k1, Kingdom k2)
        {
             try {
                var opinionObj = World.world.diplomacy.getOpinion(k1, k2);
                if(opinionObj != null) {
                    try { return (int)Reflection.GetField(opinionObj.GetType(), opinionObj, "total"); }
                    catch { 
                        try { return opinionObj.total; } catch {}
                    }
                }
                return 0;
             } catch {
                 return 0;
             }
        }

        public static void MakePeace(Kingdom winner, Kingdom loser)
        {
            if (!winner.isEnemy(loser)) return;

             var wars = World.world.wars.getWars(winner);
             War activeWar = null;
             foreach(var w in wars) {
                if (!w.hasEnded() && (w.isAttacker(loser) || w.isDefender(loser))) {
                    activeWar = w; break;
                }
             }

             if (activeWar != null) {
                 // Check Power
                int winnerPower = winner.countTotalWarriors();
                int loserPower = loser.countTotalWarriors();
                
                if(winnerPower >= loserPower) {
                    // Forced Peace by Winner
                    World.world.wars.endWar(activeWar, WarWinner.Peace);
                    KingdomController.Instance.RequestGlobalBriefing();
                } else {                    
                    // If Initiator is Weaker, they must Pay
                    int goldDemand = Mathf.Clamp((loserPower - winnerPower) * 5, 100, 5000);
                    var wData = WorldDataManager.Instance.GetKingdomData(winner);
                    
                    if(wData != null && wData.GoldReserves >= goldDemand) {
                        wData.GoldReserves -= goldDemand;
                         // Pay Loser
                        var lData = WorldDataManager.Instance.GetKingdomData(loser);
                        if(lData != null) lData.GoldReserves += goldDemand;

                        World.world.wars.endWar(activeWar, WarWinner.Peace);
                        KingdomController.Instance.RequestGlobalBriefing();
                    }
                }
             }
        }

        public static void SendGift(Kingdom from, Kingdom to, float amount)
        {
            var fData = WorldDataManager.Instance.GetKingdomData(from);
            var tData = WorldDataManager.Instance.GetKingdomData(to);
            if(fData == null || tData == null || fData.GoldReserves < amount) return;

            fData.GoldReserves -= amount;
            tData.GoldReserves += amount;

            // Update WorldBox Relation
            try {
                object relation = Reflection.CallMethod(from, "getRelation", to);
                if(relation != null) {
                    int current = (int)Reflection.GetField(relation.GetType(), relation, "weights");
                    int boost = (int)(amount / 100f); // 1 point per 100 gold
                    if(boost > 50) boost = 50; // Cap boost
                    Reflection.SetField(relation, "weights", current + boost);
                }
            } catch {}

            tData.RecentDiplomaticEvents.Add($"{from.name}: A gift for our friends. (Relation +)");
        }

        public static void ProposePact(Kingdom from, Kingdom to, string pactType)
        {
            var tData = WorldDataManager.Instance.GetKingdomData(to);
            if(tData == null) return;

            // Pacts are messages that suggest long-term cooperation
            string msg = $"We propose a {pactType} pact between our people.";
            if(pactType == "Trade") msg = "We propose an Open Borders Trade Pact to boost our economies.";
            else if(pactType == "Ideological") msg = $"As fellow {WorldDataManager.Instance.GetKingdomData(from).EconomicSystem}s, we should stand together.";

            TradeOffer pactOffer = new TradeOffer {
                ID = "PACT-" + UnityEngine.Random.Range(1000, 9999),
                Source = from,
                Target = to,
                OfferResource = "Pact",
                OfferAmount = 0,
                RequestResource = pactType,
                Message = msg,
                IsResponse = false
            };
            tData.PendingOffers.Add(pactOffer);
        }

        public static void SendDiplomaticMessage(Kingdom from, Kingdom to, string text)
        {
            var tData = WorldDataManager.Instance.GetKingdomData(to);
            if(tData != null) {
                tData.RecentDiplomaticEvents.Add($"{from.name}: {text}");
                if(tData.RecentDiplomaticEvents.Count > 10) tData.RecentDiplomaticEvents.RemoveAt(0);
            }
        }

        public static void ProposeTrade(Kingdom from, Kingdom to, string offRes, float offAmt, string reqRes, float reqAmt, string msg)
        {
            if (offAmt <= 0 && reqAmt <= 0) return;

            var tData = WorldDataManager.Instance.GetKingdomData(to);
            var fData = WorldDataManager.Instance.GetKingdomData(from);
            if(tData == null || fData == null) return;

            TradeOffer offer = new TradeOffer {
                ID = "TR-" + UnityEngine.Random.Range(100000, 999999),
                Source = from,
                Target = to,
                OfferResource = offRes,
                OfferAmount = (int)offAmt,
                RequestResource = reqRes,
                RequestAmount = (int)reqAmt,
                Message = msg,
                IsResponse = false,
                ExpirationTick = 0 
            };

            tData.PendingOffers.Add(offer);
            fData.SentOffers.Add(offer);
            
            EconomyLogger.LogVerbose($"[TRADE] {from.name} offers {offAmt} {offRes} for {reqAmt} {reqRes} to {to.name}.");
        }

        public static void RespondToTrade(Kingdom from, string offerId, bool accept, string msg)
        {
            var fData = WorldDataManager.Instance.GetKingdomData(from);
            if(fData == null) return;

            int idx = fData.PendingOffers.FindIndex(o => o.ID == offerId);
            if(idx == -1) return;

            TradeOffer offer = fData.PendingOffers[idx];
            fData.PendingOffers.RemoveAt(idx);

            Kingdom targetK = offer.Source; // Use Source reference
            if(targetK == null || !targetK.isAlive()) return;
            var tData = WorldDataManager.Instance.GetKingdomData(targetK);
            if(tData == null) return;

            if(accept) {
                if(offer.OfferResource == "Pact") {
                    RegisterPact(from, targetK, offer.RequestResource);
                } else {
                    ExecuteTradeInternal(targetK, from, offer);
                }
            }

            TradeOffer response = new TradeOffer {
                ID = offer.ID,
                Source = from,
                Target = targetK,
                Accepted = accept,
                Message = msg,
                IsResponse = true
            };
            tData.PendingOffers.Add(response);
        }

        private static void RegisterPact(Kingdom k1, Kingdom k2, string type)
        {
            var d1 = WorldDataManager.Instance.GetKingdomData(k1);
            var d2 = WorldDataManager.Instance.GetKingdomData(k2);
            if(d1 == null || d2 == null) return;

            string p1 = $"{k2.name}:{type}";
            string p2 = $"{k1.name}:{type}";

            if(!d1.ActivePacts.Contains(p1)) d1.ActivePacts.Add(p1);
            if(!d2.ActivePacts.Contains(p2)) d2.ActivePacts.Add(p2);
        }

        private static void ExecuteTradeInternal(Kingdom k1, Kingdom k2, TradeOffer offer)
        {
             // 1. Initiator gives to Responder
             TransferAnyResource(k1, k2, offer.OfferResource, (int)offer.OfferAmount);
             
             // 2. Responder gives to Initiator
             TransferAnyResource(k2, k1, offer.RequestResource, (int)offer.RequestAmount);
        }

        private static void TransferAnyResource(Kingdom from, Kingdom to, string res, int amt)
        {
            if(res.ToLower() == "coins" || res.ToLower() == "gold") {
                 var fData = WorldDataManager.Instance.GetKingdomData(from);
                 var tData = WorldDataManager.Instance.GetKingdomData(to);
                 if(fData != null && tData != null) {
                     float take = Mathf.Min(fData.GoldReserves, amt);
                     fData.GoldReserves -= take;
                     tData.GoldReserves += take;
                 }
            } else {
                RemoveResourceFromKingdom(from, res, (int)amt);
                AddResourceToKingdom(to, res, (int)amt);
                
                // Log the resource transfer part of the trade
                LogTradeEvent(from, to, res, amt, 0, 0, "");
            }
        }
        
        private static void LogTradeEvent(Kingdom seller, Kingdom buyer, string resId, int amount, int gold, float coin, string coinId)
        {
             if (WorldDataManager.Instance == null) return;
             
             TradeEvent evt = new TradeEvent {
                 Tick = Time.frameCount,
                 Seller = seller,
                 Buyer = buyer,
                 ResourceId = resId,
                 Amount = amount,
                 CostGold = gold,
                 CostCoin = coin,
                 CoinID = coinId,
                 TotalValue = gold + (int)coin 
             };
             
             WorldDataManager.Instance.TradeHistory.Add(evt);
             if(WorldDataManager.Instance.TradeHistory.Count > 100) WorldDataManager.Instance.TradeHistory.RemoveAt(0);
             
             WorldDataManager.Instance.CurrentTickTradeVolume += (gold + coin);
        }

        // --- HELPERS ---

        private static void ModifyLoyalty(City c, float change)
        {
            try {
                float val = (float)Reflection.GetField(c.data.GetType(), c.data, "loyalty");
                val += change;
                if(val > 100f) val = 100f;
                if(val < -100f) val = -100f;
                Reflection.SetField(c.data, "loyalty", val);
            } catch { } 
        }

        private static int GetTotalResource(Kingdom k, string resId)
        {
            int total = 0;
            foreach(City c in k.cities) {
                total += c.getResourcesAmount(resId);
            }
            return total;
        }

        public static void AddResourceToKingdom(Kingdom k, string resId, int amount)
        {
            if(k.cities.Count == 0) return;
            
            if(k.capital != null) {
                SimulationGameloop.ChangeResource(k.capital, resId, amount);
            } else {
                SimulationGameloop.ChangeResource(k.cities[0], resId, amount);
            }
        }

        public static void RemoveResourceFromKingdom(Kingdom k, string resId, int amount)
        {
             int remaining = amount;
             if(k.capital != null) {
                 int has = k.capital.getResourcesAmount(resId);
                 int take = Mathf.Min(has, remaining);
                 SimulationGameloop.ChangeResource(k.capital, resId, -take);
                 remaining -= take;
             }
             
             if(remaining <= 0) return;
             
             foreach(City c in k.cities) {
                 if(c == k.capital) continue;
                 if(remaining <= 0) break;
                 
                 int has = c.getResourcesAmount(resId);
                 int take = Mathf.Min(has, remaining);
                 SimulationGameloop.ChangeResource(c, resId, -take);
                 remaining -= take;
             }
        }
        public static void CondemnKingdom(Kingdom source, Kingdom target, string reason)
        {
             if(source == target) return;
             UpdateRelationWeight(source, target, -50); 
             UpdateRelationWeight(target, source, -50); 
        }

        public static void JoinWar(Kingdom source, Kingdom target, string reason)
        {
             if(source == target) return;
             if(source.isEnemy(target)) return;

             DeclareJustifiedWar(source, target, "Intervention: " + reason);
        }

        public static void Threaten(Kingdom source, Kingdom target, string threat)
        {
            if(source == null || target == null || source == target) return;
            
            // 1. Reduce relations
            UpdateRelationWeight(source, target, -30);
            UpdateRelationWeight(target, source, -30);
            
            // 2. Send threatening message
            SendDiplomaticMessage(source, target, $"⚠ THREAT: {threat}");
            
            // 3. Target may react - add to their events
            var tData = WorldDataManager.Instance.GetKingdomData(target);
            if(tData != null) {
                tData.RecentDiplomaticEvents.Add($"THREAT|{source.name}|{threat}");
            }
            
            // 4. If target is weak enough, they might submit
            int sPower = source.countTotalWarriors();
            int tPower = target.countTotalWarriors();
            
            if(sPower > tPower * 2 && UnityEngine.Random.value < 0.3f) {
                // Target may offer tribute
                var sData = WorldDataManager.Instance.GetKingdomData(source);
                if(sData != null && tData != null) {
                    int tribute = (int)(tData.GoldReserves * 0.1f);
                    if(tribute > 0) {
                        tData.GoldReserves -= tribute;
                        sData.GoldReserves += tribute;
                        SendDiplomaticMessage(target, source, $"We offer {tribute} gold as a sign of... respect.");
                    }
                }
            }
        }

        public static void ProposePeace(Kingdom source, Kingdom target, float offerGold, bool forceDivineCommand = false)
        {
            if(!source.isEnemy(target)) return;
            
            if(forceDivineCommand) {
                MakePeace(source, target);
                return;
            }
            
            // Calculate Acceptance Chance
            int myPower = source.countTotalWarriors();
            int theirPower = target.countTotalWarriors();
            
            bool accept = false;

            if (myPower > theirPower * 1.5f) {
                // We are winning, they should accept if we offer mercy
                accept = true; 
            }
            else if (theirPower > myPower * 2f) {
                // They are crushing us. We need to pay A LOT.
                if (offerGold > 1000) accept = true;
            }
            else {
                // Even fight. Peace if paid slightly.
                if (offerGold >= 100) accept = true;
            }

            if (accept) {
                // Pay
                var sData = WorldDataManager.Instance.GetKingdomData(source);
                var tData = WorldDataManager.Instance.GetKingdomData(target);
                
                if(sData.GoldReserves >= offerGold) {
                    sData.GoldReserves -= offerGold;
                    tData.GoldReserves += offerGold;
                    
                    MakePeace(source, target); 
                }
            }
        }

        public static void BribeNeutrality(Kingdom source, Kingdom target, float amount)
        {
             var sData = WorldDataManager.Instance.GetKingdomData(source);
             if(sData.GoldReserves < amount) return;

             sData.GoldReserves -= amount;
             var tData = WorldDataManager.Instance.GetKingdomData(target);
             tData.GoldReserves += amount;

             UpdateRelationWeight(target, source, 100); 
        }
        public static void FundRebels(Kingdom source, Kingdom target, int goldAmount)
        {
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData.GoldReserves < goldAmount) return;

            sData.GoldReserves -= goldAmount;
            
            // 50 gold per rebel
            int maxRebels = goldAmount / 50; 
            int actual = 0;
            
            // Target random cities
            var cities = target.cities.ToList();
            // Shuffle
            for (int i = 0; i < cities.Count; i++) {
                City temp = cities[i];
                int randomIndex = UnityEngine.Random.Range(i, cities.Count);
                cities[i] = cities[randomIndex];
                cities[randomIndex] = temp;
            }

            foreach(City c in cities) {
                if(actual >= maxRebels) break;
                if(c.units.Count == 0) continue;

                foreach(Actor unit in c.units.ToList()) {
                    if(actual >= maxRebels) break;
                    
                    if(unit.isKing() || unit.isCityLeader()) continue;

                    if(UnityEngine.Random.value < 0.4f) { 
                        // unit.addTrait("madness");
                        // unit.addTrait("bomberman");
                        if(UnityEngine.Random.value < 0.5f) unit.addTrait("veteran");
                        // if(UnityEngine.Random.value < 0.5f) unit.addTrait("pyromaniac"); 
                        actual++;
                    }
                }
            }
            
            // Risk of discovery
            if(UnityEngine.Random.value < 0.3f) { 
                UpdateRelationWeight(target, source, -100);
                DeclareJustifiedWar(target, source, "Caught inciting madness and chaos!");
            } else {
                EconomyLogger.LogVerbose($"[SECRET] {source.name} funded {actual} chaos agents in {target.name} (Madness/Bomberman). cost={goldAmount}");
            }
        }

        public static void DemandTribute(Kingdom source, Kingdom target)
        {
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            var tData = WorldDataManager.Instance.GetKingdomData(target);
            
            if(sData == null || tData == null) return;
            if(!string.IsNullOrEmpty(tData.VassalLord)) {
                return; 
            }
            if(tData.VassalLord == source.name) return; 

            int sArmy = source.countTotalWarriors();
            int tArmy = target.countTotalWarriors();

            bool isStronger = sArmy > (tArmy * 1.5f);
            
            if (isStronger)
            {
                tData.VassalLord = source.name;
                sData.Vassals.Add(target.name);
                UpdateRelationWeight(target, source, 50); 
            }
            else
            {
                UpdateRelationWeight(target, source, -100);
                DeclareJustifiedWar(source, target, "Refusal of Tribute");
            }
        }

        public static void PayTribute(Kingdom vassal, string lordName)
        {
            Kingdom lord = FindKingdomByName(lordName);
            if(lord == null) return;
            
            var vData = WorldDataManager.Instance.GetKingdomData(vassal);
            var lData = WorldDataManager.Instance.GetKingdomData(lord);
            
            int payment = (int)(vData.GoldReserves * 0.01f); 
            if (payment < 1) payment = 1;
            
            if(vData.GoldReserves >= payment) {
                vData.GoldReserves -= payment;
                lData.GoldReserves += payment;
                EconomyLogger.LogVerbose($"[TRIBUTE] {vassal.name} pays {payment}g to {lord.name}");
            }
        }

        public static Kingdom FindKingdomByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return null;
            if(name.ToLower() == "none" || name.ToLower() == "null") return null;
            
            string searchName = name.Trim().ToLowerInvariant();
            
            // 1. Exact match (case insensitive)
            foreach(var k in World.world.kingdoms.list)
            {
                if(k.name.ToLowerInvariant() == searchName) return k;
            }
            
            // 2. Contains match (handles partial names)
            foreach(var k in World.world.kingdoms.list)
            {
                string kName = k.name.ToLowerInvariant();
                if(kName.Contains(searchName) || searchName.Contains(kName)) return k;
            }
            
            // 3. Fuzzy match - handle common AI mistakes (spaces, special chars)
            string normalized = System.Text.RegularExpressions.Regex.Replace(searchName, @"[^a-z0-9]", "");
            foreach(var k in World.world.kingdoms.list)
            {
                string kNormalized = System.Text.RegularExpressions.Regex.Replace(k.name.ToLowerInvariant(), @"[^a-z0-9]", "");
                if(kNormalized == normalized) return k;
            }
            
            // 4. Levenshtein distance fallback for typos (max 2 edits)
            int bestDistance = 3;
            Kingdom bestMatch = null;
            foreach(var k in World.world.kingdoms.list)
            {
                int dist = LevenshteinDistance(searchName, k.name.ToLowerInvariant());
                if(dist < bestDistance) {
                    bestDistance = dist;
                    bestMatch = k;
                }
            }
            
            if(bestMatch != null) {
                Debug.Log($"[KingdomActions] Fuzzy matched '{name}' to '{bestMatch.name}' (distance={bestDistance})");
                return bestMatch;
            }
            
            // Debug.LogWarning($"[KingdomActions] Could not find kingdom: '{name}'");
            return null;
        }
        
        private static int LevenshteinDistance(string s1, string s2)
        {
            if(string.IsNullOrEmpty(s1)) return string.IsNullOrEmpty(s2) ? 0 : s2.Length;
            if(string.IsNullOrEmpty(s2)) return s1.Length;
            
            int[,] d = new int[s1.Length + 1, s2.Length + 1];
            for(int i = 0; i <= s1.Length; i++) d[i, 0] = i;
            for(int j = 0; j <= s2.Length; j++) d[0, j] = j;
            
            for(int i = 1; i <= s1.Length; i++) {
                for(int j = 1; j <= s2.Length; j++) {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[s1.Length, s2.Length];
        }

        // ============ ADVANCED COVERT ACTIONS ============
        public static void SpyOnKingdom(Kingdom source, Kingdom target)
        {
            if(source == null || target == null || source == target) return;
            
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData == null) return;
            
            // Cost: 5% of gold reserves (minimum 100)
            float cost = Mathf.Max(100f, sData.GoldReserves * 0.05f);
            if(sData.GoldReserves < cost) return;
            
            sData.GoldReserves -= cost;
            
            // Check if already spying
            if(!sData.SpiedKingdoms.Contains(target.name)) {
                sData.SpiedKingdoms.Add(target.name);
            }
            sData.SpyExpiry = Time.frameCount + 3600; // 1 minute of intel 
            
            // Risk of discovery: 20%
            if(UnityEngine.Random.value < 0.2f) {
                UpdateRelationWeight(target, source, -50);
                SendDiplomaticMessage(target, source, "We caught your spies! This act will not be forgotten.");
                var tData = WorldDataManager.Instance.GetKingdomData(target);
                if(tData != null) tData.RecentDiplomaticEvents.Add($"SPY_CAUGHT|{source.name}");
            }
            
            EconomyLogger.LogVerbose($"[COVERT] {source.name} spies on {target.name}. Cost: {cost:N0}g");
        }

        public static void TrainArmy(Kingdom k)
        {
            if(k == null) return;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            if(kData == null) return;
            
            // Cost: 10% of gold reserves (minimum 200)
            float cost = Mathf.Max(200f, kData.GoldReserves * 0.10f);
            if(kData.GoldReserves < cost) return;
            
            kData.GoldReserves -= cost;
            
            // Training traits to add (combat-useful ones from WorldBox)
            string[] trainingTraits = { "veteran", "savage", "regeneration", "fire_proof", "immune", "tough_buddy" };
            
            int trained = 0;
            int maxTrain = 10;
            
            foreach(var unit in k.units) {
                if(trained >= maxTrain) break;
                if(unit == null || !unit.isAlive() || !unit.isWarrior()) continue;
                
                // Pick a random trait they don't have
                string trait = trainingTraits[UnityEngine.Random.Range(0, trainingTraits.Length)];
                if(!unit.hasTrait(trait)) {
                    unit.addTrait(trait);
                    trained++;
                }
            }
            
            EconomyLogger.LogVerbose($"[MILITARY] {k.name} trained {trained} warriors. Cost: {cost:N0}g");
        }

        public static void MediateConflict(Kingdom mediator, Kingdom k1, Kingdom k2)
        {
            if(mediator == null || k1 == null || k2 == null) return;
            if(mediator == k1 || mediator == k2) return;
            if(!k1.isEnemy(k2)) return; // Not at war
            
            var mData = WorldDataManager.Instance.GetKingdomData(mediator);
            if(mData == null) return;
            
            // Cost: 5% of gold reserves (minimum 200)
            float cost = Mathf.Max(200f, mData.GoldReserves * 0.05f);
            if(mData.GoldReserves < cost) return;
            
            mData.GoldReserves -= cost;
            
            // Find and end the war
            var wars = World.world.wars.getWars(k1);
            foreach(var w in wars) {
                if(!w.hasEnded() && (w.isAttacker(k2) || w.isDefender(k2))) {
                    World.world.wars.endWar(w, WarWinner.Peace);
                    break;
                }
            }
            
            // Rewards: Relations boost and renown
            UpdateRelationWeight(k1, mediator, 70);
            UpdateRelationWeight(k2, mediator, 70);
            mediator.addRenown(10);
            
            // Global event
            foreach(var kingdom in World.world.kingdoms.list) {
                if(kingdom.isCiv()) {
                    var kd = WorldDataManager.Instance.GetKingdomData(kingdom);
                    if(kd != null) kd.RecentDiplomaticEvents.Add($"MEDIATION|{mediator.name}|{k1.name}|{k2.name}");
                }
            }
            
            EconomyLogger.LogVerbose($"[DIPLOMACY] {mediator.name} mediated peace between {k1.name} and {k2.name}. +10 Renown.");
        }

        public static void Sabotage(Kingdom source, Kingdom target, Kingdom blameTarget = null)
        {
            if(source == null || target == null || source == target) return;
            
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData == null) return;
            
            // Cost: 8% of gold reserves (minimum 300)
            float cost = Mathf.Max(300f, sData.GoldReserves * 0.08f);
            if(sData.GoldReserves < cost) return;
            
            sData.GoldReserves -= cost;
            
            // Find and destroy a random building
            if(target.cities.Count > 0) {
                City targetCity = target.cities[UnityEngine.Random.Range(0, target.cities.Count)];
                if(targetCity.buildings.Count > 0) {
                    Building b = targetCity.buildings[UnityEngine.Random.Range(0, targetCity.buildings.Count)];
                    if(b != null && b.isAlive()) {
                        b.startDestroyBuilding();
                    }
                }
            }
            
            // False flag logic: 40% chance
            Kingdom blamed = source;
            if(blameTarget != null && blameTarget != source && blameTarget != target && UnityEngine.Random.value < 0.4f) {
                blamed = blameTarget;
                // Target thinks blameTarget did it!
                var tData = WorldDataManager.Instance.GetKingdomData(target);
                if(tData != null) {
                    tData.RecentDiplomaticEvents.Add($"SABOTAGE|{blamed.name}|Building destroyed");
                    UpdateRelationWeight(target, blamed, -80);
                    // May trigger war
                    if(UnityEngine.Random.value < 0.5f) {
                        DeclareJustifiedWar(target, blamed, "Sabotage of our infrastructure");
                    }
                }
            } else {
                // 25% discovery chance
                if(UnityEngine.Random.value < 0.25f) {
                    UpdateRelationWeight(target, source, -100);
                    DeclareJustifiedWar(target, source, "Caught sabotaging our kingdom");
                }
            }
            
            EconomyLogger.LogVerbose($"[COVERT] {source.name} sabotaged {target.name}. Blamed: {blamed.name}");
        }

        public static void Assassinate(Kingdom source, Kingdom target, Kingdom blameTarget = null)
        {
            if(source == null || target == null || source == target) return;
            
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData == null) return;
            
            // Cost: 20% of gold reserves (minimum 1000)
            float cost = Mathf.Max(1000f, sData.GoldReserves * 0.20f);
            if(sData.GoldReserves < cost) return;
            
            sData.GoldReserves -= cost;
            
            // Find target: King or city leader
            Actor victim = target.king;
            if(victim == null || !victim.isAlive()) {
                // Try a city leader
                foreach(var city in target.cities) {
                    if(city.leader != null && city.leader.isAlive()) {
                        victim = city.leader;
                        break;
                    }
                }
            }
            
            if(victim == null) return;
            
            // 60% success rate
            if(UnityEngine.Random.value < 0.6f) {
                // Kill the victim
                victim.addTrait("cursed");
                victim.addTrait("sick");
                // Most reliable kill method in WorldBox
                victim.data.health = 1;
            }
            
            // False flag logic: 50% chance
            Kingdom blamed = source;
            if(blameTarget != null && blameTarget != source && blameTarget != target && UnityEngine.Random.value < 0.5f) {
                blamed = blameTarget;
                var tData = WorldDataManager.Instance.GetKingdomData(target);
                if(tData != null) {
                    tData.RecentDiplomaticEvents.Add($"ASSASSINATION|{blamed.name}|{victim.getName()}");
                    UpdateRelationWeight(target, blamed, -150);
                    DeclareJustifiedWar(target, blamed, "Assassination of our leader");
                }
            } else {
                // 40% discovery chance - much bigger consequences
                if(UnityEngine.Random.value < 0.4f) {
                    UpdateRelationWeight(target, source, -200);
                    DeclareJustifiedWar(target, source, "Caught assassinating our leader");
                    // Other kingdoms also upset
                    foreach(var k in World.world.kingdoms.list) {
                        if(k.isCiv() && k != source && k != target) {
                            UpdateRelationWeight(k, source, -30);
                        }
                    }
                }
            }
            
            EconomyLogger.LogVerbose($"[COVERT] {source.name} attempted assassination in {target.name}. Blamed: {blamed.name}");
        }

        public static void AntiCorruption(Kingdom k)
        {
            if(k == null) return;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            if(kData == null) return;
            
            // Cost: 5% of gold reserves (minimum 200)
            float cost = Mathf.Max(200f, kData.GoldReserves * 0.05f);
            if(kData.GoldReserves < cost) return;
            
            kData.GoldReserves -= cost;
            
            // Reduce corruption
            float reduction = UnityEngine.Random.Range(0.05f, 0.15f);
            kData.Corruption -= reduction;
            if(kData.Corruption < 0) kData.Corruption = 0;
            
            // Small chance of unrest 
            if(UnityEngine.Random.value < 0.1f) {
                foreach(var city in k.cities) {
                    if(UnityEngine.Random.value < 0.3f) {
                        try {
                            float currentLoyalty = (float)ReflectionUtility.Reflection.GetField(city.data.GetType(), city.data, "loyalty");
                            ReflectionUtility.Reflection.SetField(city.data, "loyalty", currentLoyalty - 10f);
                        } catch {}
                    }
                }
            }
            
            EconomyLogger.LogVerbose($"[REFORM] {k.name} ran anti-corruption campaign. Corruption: {kData.Corruption:P0}");
        }

        // ============ GLOBAL STOCK MARKET ACTIONS ============
        public static void BuyResource(Kingdom k, string resourceId, int amount)
        {
            if(k == null || string.IsNullOrEmpty(resourceId) || amount <= 0) return;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            if(kData == null) return;
            
            float price = WorldDataManager.Instance.GetResourcePrice(resourceId);
            float totalCost = price * amount;
            
            if(kData.GoldReserves < totalCost) {
                amount = (int)(kData.GoldReserves / price);
                if(amount <= 0) return;
                totalCost = price * amount;
            }
            
            kData.GoldReserves -= totalCost;
            AddResourceToKingdom(k, resourceId, amount);
            
            EconomyLogger.LogVerbose($"[MARKET] {k.name} bought {amount} {resourceId} for {totalCost:F0} gold (price: {price:F2}/unit)");
        }

        /// Sell a resource to the global market at current price.
        public static void SellResource(Kingdom k, string resourceId, int amount)
        {
            if(k == null || string.IsNullOrEmpty(resourceId) || amount <= 0) return;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            if(kData == null) return;
            
            // Check how much we actually have
            int available = GetTotalResource(k, resourceId);
            if(available < amount) amount = available;
            if(amount <= 0) return;
            
            float price = WorldDataManager.Instance.GetResourcePrice(resourceId);
            float totalRevenue = price * amount;
            
            RemoveResourceFromKingdom(k, resourceId, amount);
            kData.GoldReserves += totalRevenue;
            
            EconomyLogger.LogVerbose($"[MARKET] {k.name} sold {amount} {resourceId} for {totalRevenue:F0} gold (price: {price:F2}/unit)");
        }

        // ============ VASSAL EXPANSION ACTIONS ============
        public static void AnnexVassal(Kingdom overlord, Kingdom vassal)
        {
            if(overlord == null || vassal == null || overlord == vassal) return;
            
            var vData = WorldDataManager.Instance.GetKingdomData(vassal);
            if(vData == null || vData.VassalLord != overlord.name) {
                EconomyLogger.LogVerbose($"[ANNEX FAIL] {vassal.name} is not a vassal of {overlord.name}");
                return;
            }
            
            // Economic annexation - transfer all resources and gold to overlord
            var oData = WorldDataManager.Instance.GetKingdomData(overlord);
            if(oData != null && vData != null) {
                oData.GoldReserves += vData.GoldReserves;
                vData.GoldReserves = 0;
                
                // Transfer resources from vassal cities
                foreach(var city in vassal.cities) {
                    if(overlord.capital != null) {
                        SimulationGameloop.ChangeResource(overlord.capital, "gold", city.getResourcesAmount("gold"));
                    }
                    city.takeResource("gold", city.getResourcesAmount("gold"));
                }
                
                oData.Vassals.Remove(vassal.name);
            }
            
            // Global notification
            foreach(var k in World.world.kingdoms.list) {
                if(k.isCiv() && k != overlord) {
                    UpdateRelationWeight(k, overlord, -20); // Annexation is seen as aggressive
                }
            }
            
            EconomyLogger.LogVerbose($"[ANNEX] {overlord.name} economically absorbed vassal {vassal.name}");
        }

        /// Install a puppet ruler in a defeated kingdom (after winning a war).
        public static void InstallPuppet(Kingdom victor, Kingdom defeated)
        {
            if(victor == null || defeated == null || victor == defeated) return;
            if(!victor.isEnemy(defeated)) {
                EconomyLogger.LogVerbose($"[PUPPET FAIL] {victor.name} and {defeated.name} are not at war");
                return;
            }
            
            // End the war first
            var wars = World.world.wars.getWars(victor);
            foreach(var w in wars) {
                if(!w.hasEnded() && (w.isAttacker(defeated) || w.isDefender(defeated))) {
                    World.world.wars.endWar(w, WarWinner.Peace);
                    break;
                }
            }
            
            // Set up vassal relationship
            var dData = WorldDataManager.Instance.GetKingdomData(defeated);
            var vData = WorldDataManager.Instance.GetKingdomData(victor);
            
            if(dData != null && vData != null) {
                dData.VassalLord = victor.name;
                dData.TributeRate = 0.1f; // 10% tribute
                
                if(!vData.Vassals.Contains(defeated.name)) {
                    vData.Vassals.Add(defeated.name);
                }
                
                // Force alliance
                try {
                    if(victor.hasAlliance() && !defeated.hasAlliance()) {
                        victor.getAlliance().join(defeated);
                    } else if(!victor.hasAlliance() && !defeated.hasAlliance()) {
                        World.world.alliances.newAlliance(victor, defeated);
                    }
                } catch {}
            }
            
            EconomyLogger.LogVerbose($"[PUPPET] {victor.name} installed puppet government in {defeated.name}");
        }

        /// Release a vassal, granting them independence.
        public static void GrantIndependence(Kingdom overlord, Kingdom vassal)
        {
            if(overlord == null || vassal == null) return;
            
            var vData = WorldDataManager.Instance.GetKingdomData(vassal);
            var oData = WorldDataManager.Instance.GetKingdomData(overlord);
            
            if(vData == null || vData.VassalLord != overlord.name) {
                EconomyLogger.LogVerbose($"[INDEPENDENCE FAIL] {vassal.name} is not a vassal of {overlord.name}");
                return;
            }
            
            // Clear vassal status
            vData.VassalLord = "";
            vData.TributeRate = 0f;
            
            if(oData != null) {
                oData.Vassals.Remove(vassal.name);
            }
            
            // Massive reputation boost
            foreach(var k in World.world.kingdoms.list) {
                if(k.isCiv() && k != overlord) {
                    UpdateRelationWeight(k, overlord, 30);
                }
            }
            
            // Newly freed kingdom is grateful
            UpdateRelationWeight(vassal, overlord, 100);
            
            EconomyLogger.LogVerbose($"[INDEPENDENCE] {overlord.name} granted independence to {vassal.name}. Reputation boosted.");
        }

        // ============ INFRASTRUCTURE ACTIONS ============
        public static void ConstructBuilding(Kingdom k, string buildingType)
        {
            if(k == null || string.IsNullOrEmpty(buildingType)) return;
            if(k.cities.Count == 0) return;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            if(kData == null) return;
            
            // Building costs
            int cost = 500;
            switch(buildingType.ToLower())
            {
                case "barracks": cost = 800; break;
                case "mine": cost = 600; break;
                case "watch_tower": cost = 400; break;
                case "windmill": cost = 500; break;
                case "fishing_docks": cost = 450; break;
                case "hall": cost = 1000; break;
            }
            
            if(kData.GoldReserves < cost) return;
            
            // Deduct cost
            kData.GoldReserves -= cost;
            
            // Add building effect as resource bonus 
            City targetCity = k.capital ?? k.cities[0];
            switch(buildingType.ToLower())
            {
                case "barracks":
                    TrainArmy(k);
                    break;
                case "mine":
                    SimulationGameloop.ChangeResource(targetCity, "stone", 50);
                    SimulationGameloop.ChangeResource(targetCity, "iron", 20);
                    break;
                case "windmill":
                    SimulationGameloop.ChangeResource(targetCity, "wheat", 50);
                    break;
                case "fishing_docks":
                    SimulationGameloop.ChangeResource(targetCity, "fish", 40);
                    break;
            }
            
            EconomyLogger.LogVerbose($"[BUILD] {k.name} invested {cost} gold in {buildingType} infrastructure");
        }

        // ============ CITY ANNEXATION ACTIONS ============
        public static void BuyCity(Kingdom buyer, Kingdom seller, string cityName)
        {
            if(buyer == null || seller == null || buyer == seller) return;
            if(buyer.isEnemy(seller)) {
                EconomyLogger.LogVerbose($"[BUY CITY FAIL] {buyer.name} is at war with {seller.name}");
                return;
            }
            
            var buyerData = WorldDataManager.Instance.GetKingdomData(buyer);
            var sellerData = WorldDataManager.Instance.GetKingdomData(seller);
            if(buyerData == null || sellerData == null) return;
            
            float basePrice = seller.cities.Count * 1000f + 2000f;
            
            // Adjust price by relations (better relations = cheaper)
            int relation = 0;
            try {
                object rel = ReflectionUtility.Reflection.CallMethod(buyer, "getRelation", seller);
                if(rel != null) relation = (int)ReflectionUtility.Reflection.GetField(rel.GetType(), rel, "weights");
            } catch {}
            
            float relationMod = relation > 50 ? 0.8f : (relation < -50 ? 1.5f : 1f);
            int finalPrice = (int)(basePrice * relationMod);
            
            if(buyerData.GoldReserves < finalPrice) {
                EconomyLogger.LogVerbose($"[BUY CITY FAIL] {buyer.name} can't afford territory agreement (needs {finalPrice}, has {buyerData.GoldReserves})");
                return;
            }
            
            if(relation < 0) {
                EconomyLogger.LogVerbose($"[BUY CITY FAIL] {seller.name} relations too poor for agreement");
                return;
            }
            
            // Economic acquisition - transfer gold and boost relations
            buyerData.GoldReserves -= finalPrice;
            sellerData.GoldReserves += finalPrice;
            
            // Major relations boost
            UpdateRelationWeight(buyer, seller, 50);
            UpdateRelationWeight(seller, buyer, 50);
            
            // Form economic alliance as part of deal
            if(!buyerData.ActivePacts.Contains($"EconomicUnion:{seller.name}")) {
                buyerData.ActivePacts.Add($"EconomicUnion:{seller.name}");
                sellerData.ActivePacts.Add($"EconomicUnion:{buyer.name}");
            }
            
            EconomyLogger.LogVerbose($"[TERRITORY AGREEMENT] {buyer.name} paid {seller.name} {finalPrice} gold for territorial cooperation");
        }

        /// Unhappy city defects to a neighboring kingdom.
        public static void DefectCity(Kingdom receiver, City unhappyCity)
        {
            if(receiver == null || unhappyCity == null) return;
            Kingdom currentOwner = unhappyCity.kingdom;
            if(currentOwner == null || currentOwner == receiver) return;
            
            // Check city unhappiness
            bool isUnhappy = false;
            try {
                isUnhappy = !unhappyCity.isHappy();
            } catch {
                // Fallback: assume unhappy if loyalty is low
                try {
                    float loyalty = (float)ReflectionUtility.Reflection.GetField(unhappyCity.data.GetType(), unhappyCity.data, "loyalty");
                    isUnhappy = loyalty < 30f;
                } catch {
                    isUnhappy = false;
                }
            }
            
            if(!isUnhappy) {
                EconomyLogger.LogVerbose($"[DEFECT FAIL] {unhappyCity.name} is not unhappy enough to defect");
                return;
            }
            
            // Economic sabotage - take resources from city and give to receiver
            var receiverData = WorldDataManager.Instance.GetKingdomData(receiver);
            var ownerData = WorldDataManager.Instance.GetKingdomData(currentOwner);
            
            if(receiverData != null && ownerData != null) {
                // Drain some gold from original owner
                float drainAmount = Mathf.Min(ownerData.GoldReserves * 0.1f, 500f);
                ownerData.GoldReserves -= drainAmount;
                receiverData.GoldReserves += drainAmount;
                
                // Increase corruption in owner kingdom
                ownerData.Corruption = Mathf.Min(1f, ownerData.Corruption + 0.1f);
            }
            
            // Major diplomatic hit
            UpdateRelationWeight(currentOwner, receiver, -50);
            EconomyLogger.LogVerbose($"[UNREST] {unhappyCity.name} citizens caused unrest, benefiting {receiver.name}");
        }

        // ============ ECONOMIC UNION ACTIONS ============

        /// Form an Economic Union with another kingdom.
        /// Union members share trade bonuses and reduced embargo effects.
        public static void FormEconomicUnion(Kingdom k1, Kingdom k2)
        {
            if(k1 == null || k2 == null || k1 == k2) return;
            if(k1.isEnemy(k2)) {
                EconomyLogger.LogVerbose($"[UNION FAIL] {k1.name} and {k2.name} are at war");
                return;
            }
            
            var data1 = WorldDataManager.Instance.GetKingdomData(k1);
            var data2 = WorldDataManager.Instance.GetKingdomData(k2);
            if(data1 == null || data2 == null) return;
            
            // Add to each other's active pacts
            string unionPact = $"EconomicUnion:{k2.name}";
            string unionPact2 = $"EconomicUnion:{k1.name}";
            
            if(!data1.ActivePacts.Contains(unionPact)) {
                data1.ActivePacts.Add(unionPact);
            }
            if(!data2.ActivePacts.Contains(unionPact2)) {
                data2.ActivePacts.Add(unionPact2);
            }
            
            // Boost trade efficiency for both
            float baseBoost = 0.1f;
            bool areAllies = false;
            
            // Check if they're already allies
            try {
                areAllies = k1.hasAlliance() && k2.hasAlliance() && k1.getAlliance() == k2.getAlliance();
            } catch {}
            
            if(areAllies) {
                baseBoost = 0.2f; // Double bonus for allied kingdoms
                EconomyLogger.LogVerbose($"[UNION] Allied kingdoms {k1.name} and {k2.name} receive enhanced union benefits!");
            }
            
            data1.TradeEfficiency = Mathf.Min(2f, data1.TradeEfficiency + baseBoost);
            data2.TradeEfficiency = Mathf.Min(2f, data2.TradeEfficiency + baseBoost);
            
            // Clear any embargoes between them
            data1.EmbargoList.Remove(k2.name);
            data2.EmbargoList.Remove(k1.name);
            
            // Improve relations (bigger boost if not allied yet, to encourage future alliance)
            int relationBoost = areAllies ? 20 : 40;
            UpdateRelationWeight(k1, k2, relationBoost);
            UpdateRelationWeight(k2, k1, relationBoost);
            
            EconomyLogger.LogVerbose($"[UNION] {k1.name} and {k2.name} formed an Economic Union");
        }

        /// Leave an Economic Union with another kingdom.
        public static void LeaveEconomicUnion(Kingdom k1, Kingdom k2)
        {
            if(k1 == null || k2 == null) return;
            
            var data1 = WorldDataManager.Instance.GetKingdomData(k1);
            var data2 = WorldDataManager.Instance.GetKingdomData(k2);
            if(data1 == null || data2 == null) return;
            
            string unionPact = $"EconomicUnion:{k2.name}";
            string unionPact2 = $"EconomicUnion:{k1.name}";
            
            data1.ActivePacts.Remove(unionPact);
            data2.ActivePacts.Remove(unionPact2);
            
            // Reduce trade efficiency
            data1.TradeEfficiency = Mathf.Max(0.5f, data1.TradeEfficiency - 0.1f);
            data2.TradeEfficiency = Mathf.Max(0.5f, data2.TradeEfficiency - 0.1f);
            
            // Slight relations hit
            UpdateRelationWeight(k1, k2, -10);
            
            EconomyLogger.LogVerbose($"[UNION END] {k1.name} left Economic Union with {k2.name}");
        }

        // --- ModerBox Integration Actions ---
        public static void LaunchNuke(Kingdom source, Kingdom target)
        {
            ModerBoxHelper.LaunchNuke(source, target);
        }

        public static void LaunchMissile(Kingdom source, Kingdom target)
        {
            ModerBoxHelper.LaunchMissile(source, target);
        }

        public static void ToggleGunProduction(bool enable)
        {
            ModerBoxHelper.ToggleGuns(enable);
        }

        public static void ToggleVehicleProduction(bool enable)
        {
            ModerBoxHelper.ToggleVehicles(enable);
        }
        
        public static void BreakAlliance(Kingdom source, Kingdom target)
        {
            if(source == null || target == null || source == target) return;
            
            bool isAllied = false;
            try {
                if(source.hasAlliance() && source.getAlliance() == target.getAlliance()) {
                    isAllied = true;
                }
            } catch {}
            
            if(!isAllied) return;

            try {
                var alliance = source.getAlliance();
                if(alliance.kingdoms_hashset.Count <= 2) {
                    World.world.alliances.dissolveAlliance(alliance);
                    EconomyLogger.LogVerbose($"[DIPLOMACY] {source.name} broke the alliance with {target.name}. Alliance dissolved.");
                } else {
                    alliance.leave(source, true);
                    EconomyLogger.LogVerbose($"[DIPLOMACY] {source.name} left the alliance containing {target.name}.");
                }
                
                // Major relations hit
                UpdateRelationWeight(source, target, -80);
                UpdateRelationWeight(target, source, -80);
                
                // Global event
                foreach(var k in World.world.kingdoms.list) {
                    if(k.isCiv()) {
                        var kd = WorldDataManager.Instance.GetKingdomData(k);
                        if(kd != null) kd.RecentDiplomaticEvents.Add($"BETRAYAL|{source.name} broke alliance|{target.name}");
                    }
                }
            } catch (Exception ex) {
                Debug.LogWarning($"[AIBox] Failed to break alliance: {ex.Message}");
            }
        }

        public static void DeclareBankruptcy(Kingdom k)
        {
            if(k == null) return;
            var data = WorldDataManager.Instance.GetKingdomData(k);
            if(data == null) return;
            
            if(data.NationalDebt <= 0 && data.GoldReserves > 0) return; // Can't declare if rich/stable
            
            float oldDebt = data.NationalDebt;
            data.NationalDebt = 0;
            data.GoldReserves = 0;
            
            // Consequences:
            // Lose Stability / Happiness (Revolts likely)
            foreach(var city in k.cities) {
                ModifyLoyalty(city, -50f); // Massive loyalty hit
            }
            
            // Disband 30% of army (unpaid soldiers leave)
            int disbandCount = (int)(k.units.Count * 0.3f);
            DisbandRegiment(k, disbandCount);
            
            // Economy Ruined (Credit Score)
            data.CreditScore = 0f;
            
            EconomyLogger.LogVerbose($"[BANKRUPTCY] {k.name} defaults on {oldDebt} debt! Army reduced, cities spiraling into chaos.");
            
            // Notification
            WorldTip.instance.show($"{k.name} DECLARES BANKRUPTCY!", true, "top", 5f);
        }
    }
}



