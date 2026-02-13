using System;
using HarmonyLib;
using UnityEngine;

namespace AIBox
{
    
    /// Harmony patches to hook into the game's save/load system.
    [HarmonyPatch]
    public static class SaveLoadPatches
    {
        private static string _lastSavePath = "";

        
        /// Saves AIBox mod data after the game saves the world.
        [HarmonyPatch(typeof(SaveManager), "saveWorldToDirectory")]
        [HarmonyPostfix]
        public static void SaveWorldPostfix(string pFolder)
        {
            try
            {
                if (string.IsNullOrEmpty(pFolder)) return;
                
                Debug.Log($"[AIBox] Save hook triggered for: {pFolder}");
                AIBoxSaveManager.SaveModData(pFolder);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Error in save postfix: {ex.Message}");
            }
        }

        
        /// Prefix patch for SaveManager.loadWorld to capture the path
        [HarmonyPatch(typeof(SaveManager), "loadWorld", new Type[] { typeof(string), typeof(bool) })]
        [HarmonyPrefix]
        public static void LoadWorldPrefix(string pPath)
        {
            _lastSavePath = pPath;
        }

        
        /// Loads AIBox mod data after the game finishes loading.
        [HarmonyPatch(typeof(SaveManager), "loadData", new Type[] { typeof(SavedMap), typeof(string) })]
        [HarmonyPostfix]
        public static void LoadDataPostfix(SavedMap pData, string pPath)
        {
            try
            {
                string loadPath = !string.IsNullOrEmpty(pPath) ? pPath : _lastSavePath;
                
                if (string.IsNullOrEmpty(loadPath))
                {
                    loadPath = SaveManager.currentSavePath;
                }
                
                if (string.IsNullOrEmpty(loadPath)) return;
                
                Debug.Log($"[AIBox] Load hook triggered for: {loadPath}");
                DelayedLoadModData(loadPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AIBox] Error in load postfix: {ex.Message}");
            }
        }

        
        /// Delays mod data loading to ensure kingdoms are fully initialized
        private static void DelayedLoadModData(string path)
        {
            if (WorldDataManager.Instance != null)
            {
                WorldDataManager.Instance.StartCoroutine(LoadAfterDelay(path));
            }
            else
            {
                AIBoxSaveManager.LoadModData(path);
            }
        }

        private static System.Collections.IEnumerator LoadAfterDelay(string path)
        {
            // Wait for 2 frames to ensure all kingdoms are initialized
            yield return null;
            yield return null;
            
            AIBoxSaveManager.LoadModData(path);
        }
    }
}
