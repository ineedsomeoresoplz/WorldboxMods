using System;
using System.Threading;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace AIBox
{
    class Traits
    {
        public static void init()
        {
            InitializeEconomyTraits();
            InitializeGameplayImpacts();
        }

        private static void InitializeEconomyTraits()
        {
            ActorTrait Corrupt = new ActorTrait();
            Corrupt.id = "Corrupt";
            Corrupt.group_id = "miscellaneous";
            Corrupt.path_icon = "ui/Icons/iconCorrupt";
            Corrupt.rate_inherit = 60;
            Corrupt.rate_birth = 30;
            Corrupt.base_stats = new BaseStats(); 
            Corrupt.base_stats["intelligence"] = 5f;
            Corrupt.base_stats["diplomacy"] = 5f;
            Corrupt.base_stats["warfare"] = 5f;
            Corrupt.base_stats["stewardship"] = 5f;
            AssetManager.traits.add(Corrupt);
            addTraitToLocalizedLibrary(Corrupt.id, "Not a very trustworthy guy");

            ActorTrait Trader = new ActorTrait();
            Trader.id = "Trader";
            Trader.group_id = "miscellaneous";
            Trader.path_icon = "ui/Icons/iconTrader";
            Trader.rate_inherit = 60;
            Trader.rate_birth = 30;
            Trader.base_stats = new BaseStats(); 
            Trader.base_stats["intelligence"] = 10f;
            AssetManager.traits.add(Trader);
            addTraitToLocalizedLibrary(Trader.id, "Buy low, sell high is the secret");
        }

        private static void InitializeGameplayImpacts()
        {
            ActorTrait wellPaid = new ActorTrait();
            wellPaid.id = "well_paid";
            wellPaid.group_id = "miscellaneous"; 
            wellPaid.path_icon = "ui/Icons/icon_well_paid"; 
            wellPaid.base_stats = new BaseStats(); 
            wellPaid.base_stats["health"] = 50;
            wellPaid.base_stats["damage"] = 5; 
            wellPaid.base_stats["speed"] = 5f; 
            wellPaid.base_stats["loyalty_traits"] = 20; 
            AssetManager.traits.add(wellPaid);
            addTraitToLocalizedLibrary("well_paid", "Well Paid", "These soldiers are fighting for a good paycheck.");

            ActorTrait unpaid = new ActorTrait();
            unpaid.id = "unpaid";
            unpaid.group_id = "miscellaneous"; 
            unpaid.path_icon = "ui/Icons/icon_unpaid"; 
            unpaid.base_stats = new BaseStats(); 
            unpaid.base_stats["health"] = -20;
            unpaid.base_stats["damage"] = -5;
            unpaid.base_stats["speed"] = -5f;
            unpaid.base_stats["loyalty_traits"] = -50; 
            AssetManager.traits.add(unpaid);
            addTraitToLocalizedLibrary("unpaid", "Unpaid", "No gold, no glory. Morale is low.");

            KingdomTrait econPower = new KingdomTrait();
            econPower.id = "econ_power";
            econPower.group_id = "miscellaneous";
            econPower.path_icon = "ui/Icons/iconGold"; 
            econPower.base_stats = new BaseStats(); 
            econPower.base_stats["diplomacy"] = 30; 
            econPower.base_stats["opinion"] = 20;
            AssetManager.kingdoms_traits.add(econPower);
            addTraitToLocalizedLibrary("econ_power", "Economic Superpower", "A dominant economic force. Wealth buys influence.");

            KingdomTrait econCrisis = new KingdomTrait();
            econCrisis.id = "econ_crisis";
            econCrisis.group_id = "miscellaneous";
            econCrisis.path_icon = "ui/Icons/iconRedCross"; 
            econCrisis.base_stats = new BaseStats(); 
            econCrisis.base_stats["diplomacy"] = -20; 
            econCrisis.base_stats["opinion"] = -20; 
            AssetManager.kingdoms_traits.add(econCrisis);
            addTraitToLocalizedLibrary("econ_crisis", "Economic Crisis", "Bankrupt and unstable. Neighbors smell weakness.");

            InitializeLoyalty();
        }

        // Initialize Loyalty
        private static void InitializeLoyalty()
        {
            LoyaltyAsset econLoyalty = new LoyaltyAsset();
            econLoyalty.id = "economy_rating";
            econLoyalty.translation_key = "loyalty_economy";
            econLoyalty.translation_key_negative = "loyalty_economy_crisis";
            
            econLoyalty.calc = (City pCity) =>
            {
                if (pCity.kingdom == null || pCity.kingdom.isNeutral()) return 0;
                
                var kData = WorldDataManager.Instance.GetKingdomData(pCity.kingdom);
                if (kData == null) return 0;

                int score = 0;

                if (kData.Wealth > 2000) score += 10;
                if (kData.Wealth > 5000) score += 10; 
                
                if (kData.TaxRate > 0.4f) score -= 20;
                if (kData.TaxRate < 0.1f) score += 10;

                if (kData.CurrencyValue < 0.2f) score -= 40; 
                if (kData.NationalDebt > kData.Wealth * 3f) score -= 10; 

                return score;
            };

            AssetManager.loyalty_library.add(econLoyalty);
            
            LocalizedTextManager.add("loyalty_economy", "Economic Prosperity");
            LocalizedTextManager.add("loyalty_economy_description", "The people are wealthy and content.");
            LocalizedTextManager.add("loyalty_economy_crisis", "Economic Ruin");
            LocalizedTextManager.add("loyalty_economy_crisis_description", "Hyperinflation and poverty are fueling rebellion.");
        }

        public static void addTraitToLocalizedLibrary(string id, string description, string info = "")
        {
          NCMS.Utils.Localization.AddOrSet("trait_" + id, id.Replace("_", " ").FirstToUpper());
          NCMS.Utils.Localization.AddOrSet("trait_" + id + "_info", description);
          if(!string.IsNullOrEmpty(info))
          {
               // Handle extended description if needed or just alias
          }
        }
    }
}

