using System;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace AIBox
{
    public static class WealthManager
    {
        // Calculate City Value 
        public static float CalculateCityValue(City city, WorldDataManager manager)
        {
            if (city == null) return 0;

            float totalWealth = 0;
            int buildingCount = 0;
            
            try { 
                buildingCount = city.buildings.Count;
            } catch { buildingCount = 10; }
            
            totalWealth += buildingCount * 100f; 

            totalWealth += city.units.Count * 25f;

            foreach (var unit in city.units) 
            {
                if(unit == null) continue;
                var uData = manager.GetUnitData(unit);
                if (uData != null)
                {
                    totalWealth += uData.PersonalWealth;
                }
            }
            
            totalWealth += city.getResourcesAmount("gold") * 0.05f;
            totalWealth += city.getResourcesAmount("iron") * 0.5f;
            totalWealth += city.getResourcesAmount("stone") * 0.1f;
            totalWealth += city.getResourcesAmount("wood") * 0.1f;
            
            return totalWealth;
        }

        // City Capture Logic
        public static void OnCityCaptured(City city, Kingdom oldKingdom, Kingdom newKingdom, WorldDataManager manager)
        {
            if (manager == null) return;
            var oldData = manager.GetKingdomData(oldKingdom);
            var newData = manager.GetKingdomData(newKingdom);

            // Capital Flight
            float flightAmount = 0;
            foreach (var unit in city.units)
            {
                var uData = manager.GetUnitData(unit);
                if (uData != null && uData.PersonalWealth > 100)
                {
                    float loss = uData.PersonalWealth * 0.2f;
                    uData.PersonalWealth -= loss;
                    flightAmount += loss;
                }
            }

            if (oldData != null)
            {
                oldData.CurrencyValue *= 0.9f; 
                EconomyLogger.LogVerbose($"MARKET PANIC: {oldKingdom.data.name} currency drops on loss of {city.data.name}!");
            }
            
            // New Kingdom gains confidence
            if (newData != null)
            {
                newData.CurrencyValue *= 1.05f;
            }
        }

        // City Destroyed Logic
        public static void OnCityDestroyed(City city, Kingdom kingdom, WorldDataManager manager)
        {
             if (manager == null || kingdom == null) return;
             var kData = manager.GetKingdomData(kingdom);

             // Total Asset Writeoff
             if (kData != null)
             {
                 kData.CurrencyValue *= 0.8f; 
                 EconomyLogger.LogVerbose($"CATASTROPHE: {kingdom.data.name} loses a city! Currency crashes!");
             }
        }
    }
}

