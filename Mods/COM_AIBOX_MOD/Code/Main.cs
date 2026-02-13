using System;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using NCMS.Utils;
using HarmonyLib;
using System.Collections.Generic;
using NeoModLoader.api;
using NeoModLoader.General;
using AIBox.UI;

namespace AIBox{

    [ModEntry]
    class Main : MonoBehaviour, IConfigurable {
        // Force Recompile Timestamp: 3
        public static Main instance;

        public void Awake()
        {
          instance = this;
          
          // Apply Harmony Patches
          Harmony harmony = new Harmony("com.aibox.mod");
          harmony.PatchAll();
          Config.show_console_on_error = false;
          
          // Initialize core systems
          WorldDataManager.Init();
          KingdomController.Init();
          KingdomIntegrity.Init();
          
          // Init restored features
          AIBoxTab.init();
          Traits.init();
          SimulationControls.init();
          ResourcePatches.init();
          ChatterManager.Init();
          UnitIntelligenceManager.Init();
          BrainInspectorUI.Init();
        }


        private bool _initialized = false;
        private void Update()
        {
            if (!_initialized)
            {
                _initialized = true;
                
                // Initialize UI (was accidentally removed)
                GameObject wmGO = new GameObject("WindowManager");
                wmGO.AddComponent<AIBox.UI.WindowManager>();
                DontDestroyOnLoad(wmGO);
                
                // Initialize ModerBox Integration (Delayed to ensure load order)
                ModerBoxHelper.Init();
            }
        }

        public ModConfig GetConfig()
        {
            try
            {
                string configPath = System.IO.Path.Combine(Mod.Info.Path, "default_config.json");
                Debug.Log($"[AIBox] Loading config from: {configPath}");
                var config = new ModConfig(configPath);
                Debug.Log("[AIBox] Config loaded successfully");
                return config;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Config loading failed: {ex.Message}");
                Debug.LogError($"[AIBox] Stack trace: {ex.StackTrace}");
                // Return null to prevent further errors
                return null;
            }
        }
    }
}