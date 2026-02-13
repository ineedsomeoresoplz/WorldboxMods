using System;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using UnityEngine;

namespace AIBox
{
    public static class GlobalCommerce
    {
        public static void ProcessMarket(WorldDataManager manager)
        {
             UpdateMarketPrices(manager);
             ExecuteTrades(manager);
        }

        public static float GetResourcePrice(Kingdom k, string resource)
        {
             if(resource == "gold") return 1f;
             if(k == null) return 10f;
             
             float stock = 0;
             if(k.capital != null) stock = k.capital.getResourcesAmount(resource);
             foreach(var c in k.cities) {
                 if(c != k.capital) stock += c.getResourcesAmount(resource);
             }
             
             // Base Demand based on population
             float demand = k.getPopulationTotal() * 0.5f; 
             if(demand < 10) demand = 10;
             
             // Supply
             float supply = stock + 1f;
             
             // P = Demand / Supply
             float rawPrice = (demand / supply) * 5f; 
             
             // Clamp
             if(rawPrice < 0.1f) rawPrice = 0.1f;
             if(rawPrice > 100f) rawPrice = 100f;
             
             // Adjust for Currency Value (Inflation)
             var data = WorldDataManager.Instance.GetKingdomData(k);
             if(data != null) {
                 if(data.CurrencyValue > 0) rawPrice /= data.CurrencyValue;
                 
                 // "Asking" markup if it's Target Resource
                 if(data.TargetResource == resource) rawPrice *= 1.5f; 
             }
             
             return rawPrice;
        }

        private static void UpdateMarketPrices(WorldDataManager manager)
        {
            float globalRisk = 1.0f;
            int activeWars = 0;

            if(MapBox.instance.kingdoms.list.Count > 0)
            {
               activeWars = MapBox.instance.kingdoms.list.Count(k => k.isCiv() && k.hasEnemies());
            }
            
            if (activeWars > 2) globalRisk = 1.0f + (activeWars * 0.1f); 
            
            foreach (var kvp in manager.KingdomData)
            {
                Kingdom k = kvp.Key;
                KingdomEconomyData data = kvp.Value;
                
                if (k.capital == null) continue;

                string resId = data.TargetResource;
                int currentAmt = k.capital.getResourcesAmount(resId);
                
                float scarcity = 1.0f;
                if (currentAmt < 10) scarcity = 5.0f;
                else if (currentAmt > 100) scarcity = 0.5f;
                
                float basePrice = UnityEngine.Random.Range(5, 15);
                
                data.MarketPrice = basePrice * scarcity * globalRisk;
                
                float noise = UnityEngine.Random.Range(0.95f, 1.05f);
                data.MarketPrice *= noise;
                if(data.MarketPrice < 1.0f) data.MarketPrice = 1.0f;
                
                data.OldCurrencyValue = data.CurrencyValue;
                data.CurrencyValue = SimulationGameloop.CalculateCurrencyValue(k, manager);
                
                if (data.CurrencyValue < 0.01f) data.CurrencyValue = 0.01f;
            }
        }

        private static void ExecuteTrades(WorldDataManager manager)
        {
            var kingdoms = manager.KingdomData.Keys.ToList();
            if (kingdoms.Count < 2) return;

            for (int i = 0; i < 100; i++) 
            {
                var allRes = AssetManager.resources.list;
                var resource = allRes.GetRandom(); 
                string tradeRes = resource.id;

                var buyers = kingdoms.Where(k => {
                    if(k.capital == null) return false;
                    int stock = k.capital.getResourcesAmount(tradeRes);
                    int gold = k.capital.getResourcesAmount("gold");
                    
                    var kd = manager.GetKingdomData(k);
                    if(kd == null) return false;
                    
                    if(kd.CurrentPolicy == KingdomPolicy.Isolationism) return false;
                    if(kd.CurrentPolicy == KingdomPolicy.Protectionism) return false;

                    bool monopoly = kd != null && kd.IsMonopolyActive && kd.MonopolyResource == tradeRes;
                    
                    bool need = stock < 50;
                    bool hoard = (gold > 100 && stock < 500) || monopoly; 
                    return (need || hoard) && gold > 10;
                }).ToList();

                if (buyers.Count == 0) continue;
                Kingdom buyer = buyers[UnityEngine.Random.Range(0, buyers.Count)];
                var bData = manager.KingdomData[buyer];

                List<Kingdom> sellers = kingdoms.Where(k => {
                    if (k == buyer || k.capital == null) return false;
                    
                    var kd = manager.GetKingdomData(k);
                    if(kd != null && bData != null)
                    {
                        if(kd.CurrentPolicy == KingdomPolicy.Isolationism) return false;

                        string buyerId = buyer.id.ToString();
                        string sellerId = k.id.ToString();
                        if(kd.EmbargoList.Contains(buyerId) || bData.EmbargoList.Contains(sellerId)) return false;
                    }
                    
                    bool sameSystem = kd.EconomicSystem == bData.EconomicSystem;
                    if (!sameSystem)
                    {
                        if(UnityEngine.Random.value < 0.5f) return false;
                    }

                    return k.capital.getResourcesAmount(tradeRes) > 10;
                }).ToList();

                if (sellers.Count == 0) continue;
                Kingdom seller = sellers[UnityEngine.Random.Range(0, sellers.Count)];
                var sData = manager.KingdomData[seller];

                int tradeAmount = 10;
                int maxSell = seller.capital.getResourcesAmount(tradeRes) - 5; 
                if(tradeAmount > maxSell) tradeAmount = maxSell;
                if(tradeAmount < 1) continue;
                
                float baseMarketPrice = sData.MarketPrice;
                float forexRatio = sData.CurrencyValue / bData.CurrencyValue;
                
                if (!buyer.isEnemy(seller) && (buyer.getAlliance() != null && buyer.getAlliance() == seller.getAlliance())) forexRatio *= 0.8f;
                
                if (sData.IsMonopolyActive && sData.MonopolyResource == tradeRes) forexRatio *= 3.0f; 

                float unitPrice = baseMarketPrice * forexRatio;
                int totalCost = (int)(unitPrice * tradeAmount);
                if(totalCost < 1) totalCost = 1;

                int buyerGold = buyer.capital.getResourcesAmount("gold");
                
                if (buyerGold < totalCost)
                {
                    float deficit = totalCost - buyerGold;
                    if (deficit < bData.CurrencySupply * 0.1f)
                    {
                         bData.CurrencySupply += (deficit / bData.CurrencyValue);
                         SimulationGameloop.ChangeResource(buyer.capital, "gold", (int)deficit);
                         buyerGold += (int)deficit;
                    }
                }

                if (buyerGold >= totalCost)
                {
                    SimulationGameloop.ChangeResource(buyer.capital, "gold", -totalCost);
                    SimulationGameloop.ChangeResource(seller.capital, "gold", totalCost);
                    
                    bData.TradeBalance -= totalCost;
                    sData.TradeBalance += totalCost;

                    SimulationGameloop.ChangeResource(seller.capital, tradeRes, -tradeAmount);
                    SimulationGameloop.ChangeResource(buyer.capital, tradeRes, tradeAmount);

                    bData.MarketPrice += 0.5f;
                    sData.MarketPrice -= 0.5f;
                    
                    sData.CurrencyValue = SimulationGameloop.CalculateCurrencyValue(seller, manager);
                    bData.CurrencyValue = SimulationGameloop.CalculateCurrencyValue(buyer, manager); 
                    
                    if(manager != null)
                    {
                        manager.TradeHistory.Insert(0, new TradeEvent{
                            Seller = seller,
                            Buyer = buyer,
                            ResourceId = tradeRes,
                            Amount = tradeAmount,
                            TotalValue = totalCost,
                            Tick = Time.frameCount,
                            CostGold = totalCost,
                            CostCoin = totalCost / bData.CurrencyValue,
                            CoinID = bData.CurrencyID
                        });
                        if(manager.TradeHistory.Count > 100) manager.TradeHistory.RemoveAt(manager.TradeHistory.Count - 1);
                        manager.CurrentTickTradeVolume += totalCost;
                        
                        EconomyLogger.LogVerbose($"[TRADE] {seller.name} -> {buyer.name}: {tradeAmount} {tradeRes} for {totalCost} G");
                    }
                }
            }
        }
    }
}


