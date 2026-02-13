using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AIBox
{
    public static class AIBoxSaveManager
    {
        private const string SAVE_FILE_NAME = "aibox_data.json";
        private const int SAVE_VERSION = 1;

        public static void SaveModData(string savePath)
        {
            try
            {
                if (string.IsNullOrEmpty(savePath)) return;
                if (WorldDataManager.Instance == null) return;

                // Ensure path ends with separator
                if (!savePath.EndsWith("/") && !savePath.EndsWith("\\"))
                    savePath += "/";

                string filePath = savePath + SAVE_FILE_NAME;
                AIBoxSaveData saveData = CollectSaveData();
                
                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(filePath, json);
                
                Debug.Log($"[AIBox] Saved mod data to {filePath} ({saveData.kingdoms?.Count ?? 0} kingdoms, {saveData.activeLoans?.Count ?? 0} loans)");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Failed to save mod data: {ex.Message}");
            }
        }

        public static void LoadModData(string savePath)
        {
            try
            {
                if (string.IsNullOrEmpty(savePath)) return;

                // Ensure path ends with separator
                if (!savePath.EndsWith("/") && !savePath.EndsWith("\\"))
                    savePath += "/";

                string filePath = savePath + SAVE_FILE_NAME;
                
                if (!File.Exists(filePath))
                {
                    Debug.Log($"[AIBox] No saved mod data found at {filePath} - starting fresh");
                    return;
                }

                string json = File.ReadAllText(filePath);
                AIBoxSaveData saveData = JsonConvert.DeserializeObject<AIBoxSaveData>(json);
                
                if (saveData == null)
                {
                    Debug.LogWarning("[AIBox] Failed to deserialize save data");
                    return;
                }

                ApplySaveData(saveData);
                Debug.Log($"[AIBox] Loaded mod data: {saveData.kingdoms?.Count ?? 0} kingdoms, {saveData.activeLoans?.Count ?? 0} loans");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Failed to load mod data: {ex.Message}");
            }
        }

        public static AIBoxSaveData CollectSaveData()
        {
            var saveData = new AIBoxSaveData
            {
                version = SAVE_VERSION,
                kingdoms = new List<KingdomSaveEntry>(),
                activeLoans = new List<LoanSaveEntry>(),
                globalDivineLaws = new Dictionary<string, bool>(),
                globalDecisionLog = new List<ThinkingLogEntry>()
            };

            // Collect Kingdom Economy Data
            foreach (var kvp in WorldDataManager.Instance.KingdomData)
            {
                Kingdom k = kvp.Key;
                if (k == null || !k.isAlive()) continue;

                var entry = new KingdomSaveEntry
                {
                    kingdomId = k.id.ToString(),
                    kingdomName = k.name,
                    economyData = CloneEconomyData(kvp.Value)
                };
                saveData.kingdoms.Add(entry);
            }

            // Collect Active Loans (already use string IDs)
            foreach (var loan in WorldDataManager.Instance.ActiveLoans)
            {
                var loanEntry = new LoanSaveEntry
                {
                    lenderKingdomId = loan.LenderKingdomID,
                    borrowerKingdomId = loan.BorrowerKingdomID,
                    principal = loan.Principal,
                    interestRate = loan.InterestRate,
                    repaidAmount = loan.RepaidAmount,
                    creationTick = loan.CreationTick,
                    lastPaymentStatus = loan.LastPaymentStatus,
                    nextRepaymentTick = loan.NextRepaymentTick,
                    repaymentInterval = loan.RepaymentInterval
                };
                saveData.activeLoans.Add(loanEntry);
            }

            // Collect Divine Laws
            foreach (var kvp in WorldDataManager.Instance.GlobalDivineLaws)
            {
                saveData.globalDivineLaws[kvp.Key] = kvp.Value;
            }

            // Collect Decision Log
            foreach (var entry in WorldDataManager.Instance.GlobalDecisionLog)
            {
                saveData.globalDecisionLog.Add(entry);
            }

            return saveData;
        }

        /// Apply loaded save data to the current world.
        public static void ApplySaveData(AIBoxSaveData saveData)
        {
            if (saveData == null) return;
            if (WorldDataManager.Instance == null) return;

            // Clear existing data
            WorldDataManager.Instance.KingdomData.Clear();
            WorldDataManager.Instance.ActiveLoans.Clear();
            WorldDataManager.Instance.GlobalDivineLaws.Clear();

            // Build lookup for current kingdoms
            var kingdomLookup = new Dictionary<string, Kingdom>();
            foreach (var k in MapBox.instance.kingdoms.list)
            {
                if (k != null && k.isAlive())
                {
                    kingdomLookup[k.id.ToString()] = k;
                    // Also allow lookup by name as fallback
                    if (!kingdomLookup.ContainsKey(k.name))
                        kingdomLookup[k.name] = k;
                }
            }

            // Restore Kingdom Economy Data
            if (saveData.kingdoms != null)
            {
                foreach (var entry in saveData.kingdoms)
                {
                    Kingdom k = null;
                    
                    // Try to find by ID first, then by name
                    if (kingdomLookup.TryGetValue(entry.kingdomId, out k) ||
                        kingdomLookup.TryGetValue(entry.kingdomName, out k))
                    {
                        if (k != null && entry.economyData != null)
                        {
                            WorldDataManager.Instance.KingdomData[k] = entry.economyData;
                        }
                    }
                }
            }

            // Restore Active Loans
            if (saveData.activeLoans != null)
            {
                foreach (var loanEntry in saveData.activeLoans)
                {
                    var loan = new Loan
                    {
                        LenderKingdomID = loanEntry.lenderKingdomId,
                        BorrowerKingdomID = loanEntry.borrowerKingdomId,
                        Principal = loanEntry.principal,
                        InterestRate = loanEntry.interestRate,
                        RepaidAmount = loanEntry.repaidAmount,
                        CreationTick = loanEntry.creationTick,
                        LastPaymentStatus = loanEntry.lastPaymentStatus,
                        NextRepaymentTick = loanEntry.nextRepaymentTick,
                        RepaymentInterval = loanEntry.repaymentInterval
                    };
                    WorldDataManager.Instance.ActiveLoans.Add(loan);
                }
            }

            // Restore Divine Laws
            if (saveData.globalDivineLaws != null)
            {
                foreach (var kvp in saveData.globalDivineLaws)
                {
                    WorldDataManager.Instance.GlobalDivineLaws[kvp.Key] = kvp.Value;
                }
            }

            // Restore Decision Log
            if (saveData.globalDecisionLog != null)
            {
                WorldDataManager.Instance.GlobalDecisionLog.Clear();
                foreach (var entry in saveData.globalDecisionLog)
                {
                    WorldDataManager.Instance.GlobalDecisionLog.Add(entry);
                }
            }
        }

        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Error = (sender, args) => { args.ErrorContext.Handled = true; },
            NullValueHandling = NullValueHandling.Ignore
        };

        private static KingdomEconomyData CloneEconomyData(KingdomEconomyData original)
        {
            // Clear non-serializable references temporarily
            var pendingOffers = original.PendingOffers;
            var sentOffers = original.SentOffers;
            
            // Clear Kingdom references from trade offers (they can't be serialized)
            original.PendingOffers = new List<TradeOffer>();
            original.SentOffers = new List<TradeOffer>();
            
            try
            {
                string json = JsonConvert.SerializeObject(original, jsonSettings);
                return JsonConvert.DeserializeObject<KingdomEconomyData>(json);
            }
            finally
            {
                // Restore original references
                original.PendingOffers = pendingOffers;
                original.SentOffers = sentOffers;
            }
        }
    }

    // ============ SAVE DATA STRUCTURES ============
    [Serializable]
    public class AIBoxSaveData
    {
        public int version;
        public List<KingdomSaveEntry> kingdoms;
        public List<LoanSaveEntry> activeLoans;
        public Dictionary<string, bool> globalDivineLaws;
        public List<ThinkingLogEntry> globalDecisionLog;
    }

    [Serializable]
    public class KingdomSaveEntry
    {
        public string kingdomId;
        public string kingdomName;
        public KingdomEconomyData economyData;
    }

    [Serializable]
    public class LoanSaveEntry
    {
        public string lenderKingdomId;
        public string borrowerKingdomId;
        public float principal;
        public float interestRate;
        public float repaidAmount;
        public float creationTick;
        public string lastPaymentStatus;
        public float nextRepaymentTick;
        public float repaymentInterval;
    }
}
