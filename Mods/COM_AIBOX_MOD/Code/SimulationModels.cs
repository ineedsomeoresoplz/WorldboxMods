using System;
using System.Collections.Generic;

namespace AIBox
{
    // ============ Game State Data Models ============
    [Serializable]
    public class KingdomEconomyData {
        public bool Initialized = false;
        
        // Economic Stats
        public float Wealth = 0;
        public float OldWealth = 0;
        public float GoldReserves = 0;
        public float NationalDebt = 0;
        public float Expenses = 0;
        public float TaxRate = 0.1f;
        public float CreditScore = 100f;
        
        // Currency
        public string CurrencyID = "gold";
        public string CurrencyName = "Gold";
        public string CurrencyIcon = "gold";
        public string CurrencyIssuerID = "";
        public float CurrencySupply = 1000f;
        public float CurrencyValue = 1.0f;
        public float OldCurrencyValue = 1.0f; 
        public float HyperinflationTimer = 0;
        
        // Strategy & AI
        public string TargetResource = "";
        public string MonopolyResource = "";
        public bool IsMonopolyActive = false;
        public EconomicPhase CurrentPhase = EconomicPhase.Expansion;
        public float PhaseTimer = 0;
        public KingdomPolicy CurrentPolicy = KingdomPolicy.FreeMarket;
        
        public List<string> EmbargoList = new List<string>();
        public float MarketPrice = 1.0f;
        public float TradeBalance = 0;
        public string EconomicSystem = "Capitalism";
        
        // Fields from EconomyData
        public float OldMarketPrice = 1.0f;
        public bool HasBank = false;
        public float BankWealth = 0f;
        public float TradeEfficiency = 1.0f;
        public string LastAllianceID = "";
        public float BadEconomyTimer = 0f;
        public float Corruption = 0f;

        // AI Controller Fields
        public bool AI_IsThinking = false;
        public float NextThinkTime = 0f;
        public float AmbitionTimer = 0f;
        public string SecretAmbition = "";
        public string StandingOrders = ""; 
        public string PendingDivineWhisper = "";
        public bool WasDivineCommand = false;
        public string LastWarReason = "";
        public float TributeRate = 0f;
        public string VassalLord = "";
        public List<string> Vassals = new List<string>();
        
        public List<string> RecentDiplomaticEvents = new List<string>();
        public List<TradeOffer> PendingOffers = new List<TradeOffer>();
        public List<TradeOffer> SentOffers = new List<TradeOffer>();
        public List<string> ActivePacts = new List<string>();
        
        public List<ThinkingLogEntry> ThinkingHistory = new List<ThinkingLogEntry>();
        
        public List<float> WealthHistory = new List<float>();
        public List<float> CurrencyHistory = new List<float>();
        
        public List<Loan> OutstandingLoans = new List<Loan>();
        
        // Espionage
        public List<string> SpiedKingdoms = new List<string>();
        public int SpyExpiry = 0; 
        
        // Action Memory - Track what AI has done recently
        public List<string> RecentActions = new List<string>(); 
        public string LastTurnFeedback = ""; 
    }

    [Serializable]
    public class UnitEconomyData {
        public float PersonalWealth = 0f; 
        public float OldPersonalWealth = 0f;
        public string Job = "Peasant";
        public float Coins = 0f;
        public float BuyingPower = 0f;
    }

    [Serializable]
    public class Loan {
        public string LenderKingdomID;
        public string BorrowerKingdomID;
        public float Principal;
        public float InterestRate;
        public float RepaidAmount;
        public float CreationTick; 
        public string LastPaymentStatus;
        public float NextRepaymentTick;
        public float RepaymentInterval;
        
        // EconomyData Helper
        public float RemainingAmount => (Principal * (1f + InterestRate)) - RepaidAmount;
    }

    [Serializable]
    public class TradeOffer {
        public string ID;
        public Kingdom Source;
        public Kingdom Target;
        public string OfferResource;
        public int OfferAmount;
        public string RequestResource;
        public int RequestAmount;
        public int Timestamp;
        
        // Compatibility Helpers for KingdomPerception / KingdomActions
        public Kingdom FromKingdom => Source;
        public Kingdom TargetKingdom => Target;
        public string Message;
        public bool IsResponse;
        public bool Accepted;
        public int ExpirationTick;
    }

    [Serializable]
    public class TradeEvent {
        public string Description;
        public int Timestamp;
        
        // Fields from EconomyData
        public int Tick;
        public Kingdom Seller;
        public Kingdom Buyer;
        public string ResourceId;
        public int Amount;
        public int TotalValue;
        public int CostGold;
        public float CostCoin;
        public string CoinID;
    }

    [Serializable]
    public class ThinkingLogEntry {
        public int Timestamp;
        public string KingdomName;
        public string LeaderName;
        
        public string DecisionSummary; 
        public string ParsedDecision; 
        public string Reasoning;
        public string InputContext;    
        
        public string Year;
        public string Decision; 
    }

    public enum EconomicPhase {
        Expansion,
        Peak,
        Contraction,
        Recovery
    }

    public enum KingdomPolicy {
        FreeMarket,
        PlannedEconomy,
        Isolationism,
        Protectionism,
        Austerity,   
        Stimulus     
    }

    // ============ AI JSON Models ============
    [Serializable]
    public struct OpenAIMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class AIDecision
    {
        public string reasoning;
        public string policy_change;
        public string target_resource;
        public float tax_rate_target; 
        public MonetaryAction monetary_action;
        public DiplomaticAction diplomatic_action; 
        public RulerAction ruler_action;
        public TradeAction trade_action;
        public CovertAction covert_action;
        public MarketAction market_action;
        public VassalAction vassal_action;
        public BuildAction build_action;
        public CityAction city_action;
        public UnionAction union_action;
        public ModernWarfareAction modern_warfare_action;
        public CultureAction culture_action;
        public ReligionAction religion_action;
        public DemographicAction demographic_action;
    }

    [Serializable]
    public class MonetaryAction
    {
        public string type; 
        public float amount;
    }

    [Serializable]
    public class DiplomaticAction
    {
        public string type;
        public string target;
        public List<string> targets;
        public string war_reason;
        public string message;
        public string alliance_action;
        public int amount;
        public string pact_type;
    }

    [Serializable]
    public class RulerAction
    {
        public string type;
        public string target;
    }

    [Serializable]
    public class TradeAction
    {
        public string type;
        public string target;
        public string resource;
        public int amount;
        
        public string offer_res;
        public int offer_amt;
        public string request_res;
        public int request_amt;
        public string message;
        
        public string offer_id;
        public bool accept;
    }

    [Serializable]
    public class CovertAction
    {
        public string type; 
        public string target; 
        public string target2; 
        public string blame_target; 
    }

    [Serializable]
    public class MarketAction
    {
        public string type; 
        public string resource; 
        public int amount; 
    }

    [Serializable]
    public class VassalAction
    {
        public string type; 
        public string target; 
    }

    [Serializable]
    public class BuildAction
    {
        public string type; 
        public string building_type; 
    }

    [Serializable]
    public class CityAction
    {
        public string type; 
        public string target_kingdom; 
        public string city_name; 
    }

    [Serializable]
    public class UnionAction
    {
        public string type; 
        public string target; 
    }

    [Serializable]
    public class ModernWarfareAction
    {
        public string type; 
        public string target; 
    }

    [Serializable]
    public class CultureAction
    {
        public string type; 
        public string target_culture; 
    }

    [Serializable]
    public class ReligionAction
    {
        public string type; 
        public string target_religion; 
    }

    [Serializable]
    public class DemographicAction
    {
        public string type; 
        public string target_race; 
        public string target_cities; 
    }

}
