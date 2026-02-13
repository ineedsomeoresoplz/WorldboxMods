using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using NeoModLoader.General;
using NeoModLoader.General.UI.Prefabs;
using NeoModLoader.api;

namespace AIBox.Patches
{
    public static class ConfigUiPatch
    {   
        // ═══════════════════════════════════════════════════════════════
        // Custom Prompt Patch - Expand to multiline text editor
        // ═══════════════════════════════════════════════════════════════

        [HarmonyPatch]
        public static class CustomPromptPatch
        {
            public static MethodBase TargetMethod()
            {
                Type type = AccessTools.Inner(AccessTools.TypeByName("NeoModLoader.ui.ModConfigureWindow"), "ModConfigListItem");
                if (type == null)
                {
                    Debug.LogError("[AIBox] FATAL: Could not find ModConfigureWindow+ModConfigListItem for patching!");
                    return null;
                }
                return AccessTools.Method(type, "setup_text");
            }
            
            public static void Postfix(object __instance, ModConfigItem pItem)
            {
                if (pItem == null) return;

                try 
                {
                    // Handle customPrompt - expand to multiline
                    if (pItem.Id == "customPrompt")
                    {
                        PatchCustomPromptMultiline(__instance, pItem);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError($"[AIBox] Failed to patch config UI for {pItem.Id}: {e.Message}");
                }
            }

            private static void PatchCustomPromptMultiline(object instance, ModConfigItem pItem)
            {
                FieldInfo field = AccessTools.Field(instance.GetType(), "text_area");
                if(field == null) {
                    Debug.LogError("[AIBox] Could not find 'text_area' field on ModConfigListItem");
                    return;
                }
                
                GameObject text_area = (GameObject)field.GetValue(instance);
                if(text_area == null) return;
                
                Transform inputTrans = text_area.transform.Find("Input");
                if(inputTrans == null) return;
                
                TextInput textInput = inputTrans.GetComponent<TextInput>();
                if(textInput == null) return;
                
                float width = 400f;
                float height = 150f;

                // Resize Main Container
                RectTransform inputRect = textInput.GetComponent<RectTransform>();
                inputRect.sizeDelta = new Vector2(width, height);
                
                // Hide Icon
                if(textInput.icon != null) {
                    textInput.icon.gameObject.SetActive(false);
                }
                
                // Resize Text Area
                if(textInput.text != null) {
                    RectTransform textRect = textInput.text.GetComponent<RectTransform>();
                    textRect.anchorMin = Vector2.zero;
                    textRect.anchorMax = Vector2.one;
                    textRect.offsetMin = new Vector2(5, 5); 
                    textRect.offsetMax = new Vector2(-5, -5);
                    
                    textInput.text.alignment = TextAnchor.UpperLeft;
                    textInput.text.resizeTextForBestFit = false;
                    textInput.text.fontSize = 6; 
                    textInput.text.verticalOverflow = VerticalWrapMode.Overflow; 
                    textInput.text.horizontalOverflow = HorizontalWrapMode.Wrap;
                }

                // Configure for Multiline
                if(textInput.input != null) {
                    textInput.input.lineType = InputField.LineType.MultiLineNewline;
                    textInput.input.textComponent.alignment = TextAnchor.UpperLeft;
                    textInput.input.inputType = InputField.InputType.Standard;
                    textInput.input.keyboardType = TouchScreenKeyboardType.Default;
                    textInput.input.characterValidation = InputField.CharacterValidation.None;
                    textInput.input.characterLimit = 0;     

                    textInput.input.ForceLabelUpdate();
                    
                    Navigation nav = new Navigation();
                    nav.mode = Navigation.Mode.None;
                    textInput.input.navigation = nav;
                    
                    textInput.input.customCaretColor = true;
                    textInput.input.caretColor = Color.white;
                }
                
                // Adjust Layout
                LayoutElement le = text_area.GetComponent<LayoutElement>();
                if(le == null) le = text_area.AddComponent<LayoutElement>();
                le.minHeight = height + 10f; 
                le.preferredHeight = height + 10f;
                le.preferredWidth = width;

                LayoutRebuilder.ForceRebuildLayoutImmediate(text_area.GetComponent<RectTransform>());
                Debug.Log("[AIBox] Successfully patched customPrompt to Multiline Editor");
            }
        }
    }
}
