using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NCMS;
using ReflectionUtility;
using HarmonyLib; 

namespace AIBox
{
    public static class ModerBoxHelper
    {
        public static bool IsInstalled { get; private set; }
        
        // Reflection targets
        private static Type _modernBoxMainType;
        private static Type _vehiclesType;
        private static Type _bombUtilsType;
        private static Type _customItemsListType;
        private static Type _traitsType;
        
        // Action costs
        public const int NUKE_COST = 200;
        public const int MISSILE_COST = 50;

        public static void Init()
        {
            try
            {
                // Try to find ModernBox types
                _modernBoxMainType = AccessTools.TypeByName("ModernBox.Main");
                
                if (_modernBoxMainType != null)
                {
                    IsInstalled = true;
                    _vehiclesType = AccessTools.TypeByName("ModernBox.Vehicles");
                    _bombUtilsType = AccessTools.TypeByName("ModernBox.BombUtilities");
                    _customItemsListType = AccessTools.TypeByName("ModernBox.CustomItemsList");
                    _traitsType = AccessTools.TypeByName("ModernBox.Traits");
                    
                    // Manually apply patches
                    Harmony harmony = new Harmony("com.aibox.moderbox");
                    
                    // We must manually patch because using [HarmonyPatch] attributes caused 
                    // Main.Awake's PatchAll() to crash before ModerBox was loaded.
                    
                    MethodInfo prefix = AccessTools.Method(typeof(AIBox.Patches.ModerBoxPatches), "Prefix_BlockAutoAction");
                    HarmonyMethod hmPrefix = new HarmonyMethod(prefix);

                    // List of methods to block
                    string[] methodsToBlock = new string[] {
                        "NuclearMissileArtilleryEffect",
                        "MissileArtilleryEffect",
                        "AntiBossNuke",
                        "GAIAmissileArtilleryEffect",
                        "HARDENmissileArtilleryEffect",
                        "HORDEmissileArtilleryEffect"
                    };

                    foreach (string methodName in methodsToBlock)
                    {
                        MethodInfo target = AccessTools.Method(_vehiclesType, methodName);
                        if (target != null)
                        {
                            harmony.Patch(target, prefix: hmPrefix);
                            Debug.Log($"[AIBox] Blocked auto-action: {methodName}");
                        }
                    }
                    
                    Debug.Log("[AIBox] ModerBox detected! Integration features enabled and patches applied.");
                }
                else
                {
                    IsInstalled = false;
                    Debug.Log("[AIBox] ModerBox not found. Integration features disabled.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIBox] Error initializing ModerBox integration: {e.Message}");
                IsInstalled = false;
            }
        }

        public static bool AIBlocksActions => IsInstalled && KingdomController.Instance != null && KingdomController.Instance.IsAIEnabled;

        public static bool CanKingdomNuke(Kingdom k)
        {
            if (!IsInstalled || k == null) return false;
            
            // Requirements:
            // 1. Modern Era (Bonfire level >= 2)
            // 2. Has Missile System units
            // 3. Enough gold (200)
            
            var data = WorldDataManager.Instance.GetKingdomData(k);
            if(data == null) return false;
            
            return IsModernEra(k) && HasMissileSystem(k) && data.GoldReserves >= NUKE_COST;
        }

        public static bool HasMissileSystem(Kingdom k)
        {
            if (k.units == null) return false;
            
            // Broad check for any missile-capable unit
            return k.units.Any(u => 
                u.isAlive() && 
                (u.asset.id.ToLower().Contains("missile") || 
                 u.asset.id.ToLower().Contains("nuke") || 
                 u.asset.id.ToLower().Contains("artillery") ||
                 u.asset.id.Contains("MA900") ||
                 u.asset.id.Contains("dreadnaught") ||
                 u.hasTrait("Unitpotential")) // Vehicles often have this trait
            );
        }

        public static bool IsModernEra(Kingdom k)
        {
            if (k.cities == null || k.cities.Count == 0) return false;
            
            foreach (City city in k.cities)
            {
                Building bonfire = city.getBuildingOfType("type_bonfire");
                if (bonfire != null && bonfire.asset != null && bonfire.asset.upgrade_level >= 2)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountMissileUnits(Kingdom k)
        {
            if (k.units == null) return 0;
            return k.units.Count(u => 
                u.isAlive() && 
                (u.asset.id.Contains("MissileSystem") || 
                 u.asset.id.Contains("NUKER") || 
                 u.asset.id.Contains("MA9000") ||
                 u.asset.id.Contains("dreadnaught"))
            );
        }
        
        public static bool AreNukesEnabled()
        {
            if (!IsInstalled) return false;
            // Reflect: ModernBox.Vehicles.nukesEnabled
            FieldInfo field = AccessTools.Field(_vehiclesType, "nukesEnabled");
            if (field != null)
            {
                return (bool)field.GetValue(null);
            }
            return false;
        }

        // --- Actions ---

        public static void LaunchNuke(Kingdom source, Kingdom target)
        {
            if (!IsInstalled || source == null || target == null) return;
            
            // Validate capability before launching
            if (!CanKingdomNuke(source)) {
                Debug.Log($"[AIBox] {source.name} cannot launch nuke - capability check failed");
                return;
            }
            
            // Safety check: target must have cities
            if (target.cities == null || target.cities.Count == 0) {
                Debug.Log($"[AIBox] Cannot nuke {target.name} - no cities to target");
                return;
            }
            
            // Find a caster (missile unit) in source kingdom
            // Use broad check similar to HasMissileSystem
            Actor caster = source.units.FirstOrDefault(u => 
                u.isAlive() && 
                (u.asset.id.ToLower().Contains("missile") || 
                 u.asset.id.ToLower().Contains("nuke") || 
                 u.asset.id.ToLower().Contains("artillery") ||
                 u.asset.id.Contains("MA900") ||
                 u.asset.id.Contains("dreadnaught") ||
                 u.hasTrait("Unitpotential"))
            );
            
            if (caster == null)
            {
                // Fallback: Use king as "order giver" if no specific unit found
                caster = source.king;
            }
            
            if (caster == null) {
                Debug.Log($"[AIBox] {source.name} cannot launch nuke - no valid caster unit");
                return;
            }

            City targetCity = target.cities.GetRandom();
            if (targetCity == null) return;
            
            WorldTile targetTile = targetCity.getTile();
            if (targetTile == null) return;

            // Launch visual
            Vector3 startPos = caster.current_position;
            Vector3 endPos = targetTile.posV3;
            
            float dist = Vector3.Distance(startPos, endPos);
            Vector3 attackVector = Toolbox.getNewPoint(startPos.x, startPos.y, endPos.x, endPos.y, dist);
            Vector3 startProjectile = Toolbox.getNewPoint(startPos.x, startPos.y, endPos.x, endPos.y, 0.5f); // approximate size
            startProjectile.y += 0.5f;

            // "NUKER" is the projectile ID in ModerBox
            World.world.projectiles.spawn(caster, null, "NUKER", startProjectile, attackVector);
            
            // Pay cost
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if(sData != null) sData.GoldReserves -= NUKE_COST;
            
            // Log it
            AILogger.LogInteraction(source, "[DIRECT ACTION - NO AI DECISION]", "Nuke Strategy", "Nuclear Launch Detected", $"Launched NUKE at {target.name}");
            WorldTip.showNow($"{source.name} NUKED {target.name}!", true, "top");
            Debug.Log($"[AIBox] NUCLEAR LAUNCH: {source.name} -> {target.name}");
        }

        public static void LaunchMissile(Kingdom source, Kingdom target)
        {
            if (!IsInstalled || source == null || target == null) return;
            
            // Validate gold reserves
            var sData = WorldDataManager.Instance.GetKingdomData(source);
            if (sData == null || sData.GoldReserves < MISSILE_COST) {
                Debug.Log($"[AIBox] {source.name} cannot launch missile - insufficient gold (need {MISSILE_COST}, have {sData?.GoldReserves ?? 0})");
                return;
            }
            
            // Safety check: target must have cities
            if (target.cities == null || target.cities.Count == 0) {
                Debug.Log($"[AIBox] Cannot launch missile at {target.name} - no cities to target");
                return;
            }
            
            // Check for missile capability
            if (!HasMissileSystem(source)) {
                Debug.Log($"[AIBox] {source.name} cannot launch missile - no missile system");
                return;
            }
            
            Actor caster = source.units.FirstOrDefault(u => 
                u.isAlive() && 
                (u.asset.id.ToLower().Contains("missile") || 
                 u.asset.id.ToLower().Contains("nuke") || 
                 u.asset.id.ToLower().Contains("artillery") ||
                 u.asset.id.Contains("MA900") ||
                 u.asset.id.Contains("dreadnaught") ||
                 u.hasTrait("Unitpotential"))
            );
            if (caster == null) caster = source.king;
            if (caster == null) {
                Debug.Log($"[AIBox] {source.name} cannot launch missile - no valid caster unit");
                return;
            }

            City targetCity = target.cities.GetRandom();
            if (targetCity == null) return;
            WorldTile targetTile = targetCity.getTile();
            if (targetTile == null) return;
             
            Vector3 startPos = caster.current_position;
            Vector3 endPos = targetTile.posV3;
            float dist = Vector3.Distance(startPos, endPos);
            Vector3 attackVector = Toolbox.getNewPoint(startPos.x, startPos.y, endPos.x, endPos.y, dist);
            Vector3 startProjectile = Toolbox.getNewPoint(startPos.x, startPos.y, endPos.x, endPos.y, 0.5f);
            startProjectile.y += 0.5f;

            // "missileartillery" is standard missile
            World.world.projectiles.spawn(caster, null, "missileartillery", startProjectile, attackVector);
             
            sData.GoldReserves -= MISSILE_COST;
            
            // Log it
            AILogger.LogInteraction(source, "[DIRECT ACTION - NO AI DECISION]", "Missile Strike", "Missile Launch Detected", $"Launched missile at {target.name}");
            Debug.Log($"[AIBox] MISSILE LAUNCH: {source.name} -> {target.name}");
        }

        public static void ToggleGuns(bool enable)
        {
            if (!IsInstalled) return;
            // CustomItemsList.turnOnGuns() / turnOffGuns()
            string methodName = enable ? "turnOnGuns" : "turnOffGuns";
            MethodInfo method = AccessTools.Method(_customItemsListType, methodName);
            method?.Invoke(null, null);
        }

        public static void ToggleVehicles(bool enable)
        {
            if (!IsInstalled) return;
            // Traits.turnOnVehicles() / turnOffVehicles()
            string methodName = enable ? "turnOnVehicles" : "turnOffVehicles";
            MethodInfo method = AccessTools.Method(_traitsType, methodName);
            method?.Invoke(null, null);
        }
    }
}
