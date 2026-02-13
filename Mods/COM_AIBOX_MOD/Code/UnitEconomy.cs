using System;
using System.Linq;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace AIBox
{
    public static class UnitEconomy
    {
        // Update all units
        public static void UpdateUnits(WorldDataManager manager)
        {
            foreach (var unit in MapBox.instance.units.ToList())
            {
                if (unit == null || !unit.isAlive() || unit.kingdom == null || !unit.kingdom.isCiv()) continue;
                if (unit.hasTrait("boat") || unit.isBaby()) continue;

                var uData = manager.GetUnitData(unit);
                var kData = manager.GetKingdomData(unit.kingdom);

                if (kData == null) continue;

                UpdateUnitWealth(unit, uData, unit.kingdom, kData);
                ProcessUnitActions(unit, uData, unit.kingdom, kData, manager);
            }
        }

        // Update Unit Wealth
        private static void UpdateUnitWealth(Actor unit, UnitEconomyData uData, Kingdom kingdom, KingdomEconomyData kData)
        {
            uData.OldPersonalWealth = uData.PersonalWealth;
            
            float baseWage = 1.0f + (unit.stats["level"] * 0.2f);
            
            float jobWage = 1.0f; 
            if (unit.isKing()) jobWage = 25.0f;  
            else if (unit.isCityLeader()) jobWage = 12.0f;
            else if (unit.hasTrait("miner")) jobWage = 4.0f;
            
            float traitMod = 1.0f;
            if (unit.hasTrait("genius")) traitMod += 0.5f;
            if (unit.hasTrait("greedy")) traitMod += 0.5f;
            if (unit.hasTrait("stupid")) traitMod -= 0.3f;
            
            float luck = unit.stats["luck"];
            float grossIncome = (baseWage * jobWage * traitMod * luck);
            
            if (unit.hasTrait("Trader")) grossIncome += 15.0f; 
            if (unit.hasTrait("lucky")) grossIncome += 10.0f;

            float tax = grossIncome * kData.TaxRate;
            float lifestyleCost = 10f;

            float totalExpense = tax + lifestyleCost;

            float netChange = grossIncome - totalExpense;
            
            uData.PersonalWealth += netChange;
            
            if (uData.PersonalWealth < 0) uData.PersonalWealth = 0;
        }

        // Process Unit Actions
        private static void ProcessUnitActions(Actor unit, UnitEconomyData uData, Kingdom kingdom, KingdomEconomyData kData, WorldDataManager manager)
        {
            if (uData.PersonalWealth > 100)
            {
                if (unit.hasTrait("stupid") || unit.hasTrait("mad"))
                {
                    if (UnityEngine.Random.value < 0.05f) 
                    {
                        float bet = uData.PersonalWealth * 0.2f;
                        if (UnityEngine.Random.value < 0.80f) {
                            uData.PersonalWealth -= bet;
                        } else {
                            uData.PersonalWealth += bet * 2f;
                        }
                    }
                }
                else if (unit.hasTrait("genius") || unit.hasTrait("wise"))
                {
                    if (UnityEngine.Random.value < 0.02f)
                    {
                        float gain = uData.PersonalWealth * 0.05f;
                        uData.PersonalWealth += gain;
                    }
                }
            }
            
            if (uData.PersonalWealth > 50)
            {
                 bool isTyrant = kData.NationalDebt > 5000;
                 bool isCriminal = unit.hasTrait("deceitful") || unit.hasTrait("bloodlust");
                 
                 if (isTyrant || isCriminal)
                 {
                     if (UnityEngine.Random.value < 0.005f)
                     {
                         float seized = uData.PersonalWealth;
                         uData.PersonalWealth = 0;
                         kData.GoldReserves += seized; 
                         // Logging removed
                     }
                 }
            }

            if (unit.getAge() > 60 && uData.PersonalWealth > 200)
            {
                if (UnityEngine.Random.value < 0.01f) 
                {
                     float legacyGift = uData.PersonalWealth * 0.5f;
                     uData.PersonalWealth -= legacyGift;

                     kData.GoldReserves += legacyGift;
                }
            }
            
            if (unit.isKing()) 
            {
                TryBuyKingdom(unit, uData, kingdom, manager);
            }

            if (uData.PersonalWealth < 500 && UnityEngine.Random.value < 0.0005f) 
            {
                float windfall = UnityEngine.Random.Range(500f, 2000f);
                uData.PersonalWealth += windfall;
            }
        }
        
        //Buy Kingdom
        private static void TryBuyKingdom(Actor buyer, UnitEconomyData uData, Kingdom buyerKingdom, WorldDataManager manager)
        {
            Kingdom target = MapBox.instance.kingdoms.getRandom();
            if (target != null && target != buyerKingdom && manager.KingdomData.ContainsKey(target))
            {
                var tData = manager.KingdomData[target];

                if (tData.Wealth <= -20 && uData.PersonalWealth > 500 && UnityEngine.Random.value < 0.0001f)
                {
                    foreach(var city in target.cities.ToList())
                    {
                         System.Reflection.MethodInfo join = city.GetType().GetMethod("joinKingdom");
                         if(join != null) join.Invoke(city, new object[] { buyerKingdom });
                    }
                    uData.PersonalWealth -= 200; 
                    EconomyLogger.LogVerbose($"{buyer.getName()} bought the {target.data.name} kingdom assets!");
                }
            }
        }
    }
}

