using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UI;
using AIBox.UI;
using NeoModLoader.General;
using AIBox.Provider;

namespace AIBox
{
    class SimulationControls
    {
        public static Kingdom embargoSource;
        public static Kingdom loanSource;
        
        public static void init()
        {
          NCMS.Utils.Localization.AddOrSet("EconomyBox", "AIBox");
          NCMS.Utils.Localization.AddOrSet("economy_tab_desc", "AI Controls and Logs");

          NCMS.Utils.Localization.AddOrSet("aibox_logs_button", "Global Logs");
          NCMS.Utils.Localization.AddOrSet("aibox_logs_button_description", "Open the Global AI Decision Log.");
          
          NCMS.Utils.Localization.AddOrSet("aibox_force_briefing", "Force Briefing");
          NCMS.Utils.Localization.AddOrSet("aibox_force_briefing_description", "Force the KingdomController to generate a global situation report now.");

          PowersTab tab = AIBoxTab.EconomyTab; 

          PowerButton toggleBtn = null;
          Action updateColor = () => {
              if(toggleBtn == null) return;
              var img = toggleBtn.transform.Find("Icon")?.GetComponent<Image>();
              if(img != null && KingdomController.Instance != null) {
                  img.color = KingdomController.Instance.IsAIEnabled ? Color.green : Color.red;
              }
          };

          toggleBtn = PowerButtons.CreateButton(
            "aibox_toggle_ai",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_toggle_ai"), 
            "Toggle AI",
            "Enable/Disable AI + Check Sanity",
            new Vector2(72, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => {
                 if(KingdomController.Instance != null) {
                     KingdomController.Instance.IsAIEnabled = !KingdomController.Instance.IsAIEnabled;
                     bool on = KingdomController.Instance.IsAIEnabled;
                     WorldTip.showNow($"AI Thinking: {(on ? "ON" : "OFF")}", false, "top");
                     if(on) {
                         var provider = ProviderSettings.GetCurrentProvider();
                         if (provider != null && provider.RequiresApiKey && string.IsNullOrEmpty(ProviderSettings.Data.apiKey)) {
                             AIProviderWindow.Show();
                             WorldTip.showNow("Please configure API Key!", true, "top");
                         }
                         KingdomController.Instance.StartCoroutine(KingdomController.Instance.TestAPIConnection());
                     }
                     updateColor();
                 }
            }
          );
          updateColor();

          PowerButtons.CreateButton(
            "aibox_mind_terminal",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_mind_terminal"), 
            "Mind Terminal",
            "Open detailed AI thinking log for specific kingdoms.",
            new Vector2(108, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => KingdomDecisionWindow.Open()
          );

          PowerButtons.CreateButton(
            "aibox_logs",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_logs"), 
            "Global Logs",
            "Open the Global AI Decision Log.",
            new Vector2(144, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => GlobalLogWindow.Open()
          );

          PowerButtons.CreateButton(
            "aibox_divine_laws",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_divine_laws"), 
            "Divine Laws",
            "Configure Divine Laws & Orders.",
            new Vector2(180, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => DivineLawsWindow.ShowWindow()
          );

          PowerButtons.CreateButton(
            "aibox_force_think",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_force_think"), 
            "Force All Think",
            "Force ALL kingdoms to process their AI decisions NOW.",
            new Vector2(216, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => {
                 if(KingdomController.Instance == null || !KingdomController.Instance.IsAIEnabled) {
                     WorldTip.showNow("AI is disabled!", false, "top");
                     return;
                 }
                 int count = 0;
                 foreach(var k in World.world.kingdoms.list) {
                     if(k != null && k.isAlive() && k.isCiv()) {
                         KingdomController.Instance.QueueRequest(k);
                         count++;
                     }
                 }
                 WorldTip.showNow($"Queued {count} kingdoms for AI processing.", false, "top");
            }
          );

          // AI Data Viewer button - commented out, replaced with chatter toggle
          /*
          PowerButtons.CreateButton(
            "aibox_data_viewer",
            SpriteTextureLoader.getSprite("ui/Icons/aibox_data_viewer"), 
            "AI Data Viewer",
            "View the raw AI input data for the selected kingdom.",
            new Vector2(252, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => AIDataViewerWindow.Open()
          );
          */
          
          // Unit Chatter Toggle Button
          PowerButton chatterBtn = null;
          Action updateChatterColor = () => {
              if(chatterBtn == null) return;
              var img = chatterBtn.transform.Find("Icon")?.GetComponent<Image>();
              if(img != null && ChatterManager.Instance != null) {
                  img.color = ChatterManager.Instance.ChatterEnabled ? Color.green : Color.red;
              }
          };
          
          chatterBtn = PowerButtons.CreateButton(
            "aibox_chatter_toggle",
            SpriteTextureLoader.getSprite("ui/Icons/iconShowTalkBubbles"), 
            "Unit Chatter",
            "Toggle unit speech bubbles on/off.",
            new Vector2(252, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => {
                 if(ChatterManager.Instance != null) {
                     ChatterManager.Instance.ChatterEnabled = !ChatterManager.Instance.ChatterEnabled;
                     bool on = ChatterManager.Instance.ChatterEnabled;
                     WorldTip.showNow($"Unit Chatter: {(on ? "ON" : "OFF")}", false, "top");
                     updateChatterColor();
                 }
            }
          );
          
          updateChatterColor();

          // Brain Inspector Toggle
          PowerButton brainBtn = null;
          Action updateBrainColor = () => {
              if(brainBtn == null) return;
              var img = brainBtn.transform.Find("Icon")?.GetComponent<Image>();
              if(img != null && BrainInspectorUI.Instance != null) {
                  img.color = BrainInspectorUI.Instance.Enabled ? Color.green : Color.red;
              }
          };

          brainBtn = PowerButtons.CreateButton(
            "aibox_brain_inspector",
            SpriteTextureLoader.getSprite("ui/Icons/iconBrain"), // Verify this icon exists or use "iconDebug"
            "Brain Inspector",
            "See the hidden personality of units.",
            new Vector2(288, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => {
                 if(BrainInspectorUI.Instance != null) {
                     BrainInspectorUI.Instance.Enabled = !BrainInspectorUI.Instance.Enabled;
                     bool on = BrainInspectorUI.Instance.Enabled;
                     WorldTip.showNow($"Brain Inspector: {(on ? "ON" : "OFF")}", false, "top");
                     updateBrainColor();
                 }
            }
          );
          updateBrainColor();

          // AI Provider Configuration Button
          PowerButtons.CreateButton(
            "aibox_ai_provider",
            SpriteTextureLoader.getSprite("ui/Icons/iconOptions"), // Use gear/settings icon
            "AI Provider",
            "Configure AI provider, key and model.",
            new Vector2(324, 18),
            NCMS.Utils.ButtonType.Click,
            tab.transform,
            () => AIProviderWindow.Show()
          );

        }
    }
}



