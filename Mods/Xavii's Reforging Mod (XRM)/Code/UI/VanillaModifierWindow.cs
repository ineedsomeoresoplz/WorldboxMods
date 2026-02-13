using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using XaviiWindowsMod.API;
using XRM.Code.Content;

namespace XRM.Code.UI
{
    internal sealed class VanillaModifierWindow : MonoBehaviour
    {
        private const string ModGuid = "com.xavii.xrm";
        private const string WindowFile = "vanilla_modifier_editor";
        private const string RuntimeId = "com.xavii.xrm::vanilla_modifier_editor";

        private static readonly Color ConfirmEnabledColor = new Color(0.20f, 0.47f, 0.24f, 1f);
        private static readonly Color ConfirmDisabledColor = new Color(0.23f, 0.30f, 0.36f, 1f);

        private static readonly HashSet<string> AllowedVanillaTypes = new HashSet<string>
        {
            "power",
            "truth",
            "protection",
            "speed",
            "balance",
            "health",
            "finesse",
            "mastery",
            "knowledge",
            "sharpness",
            "flame",
            "ice",
            "stun",
            "slowness",
            "poison"
        };

        private ItemWindow _itemWindow;
        private Item _item;
        private XwmWindowHandle _windowHandle;
        private readonly List<ModifierGroup> _groups = new List<ModifierGroup>();
        private readonly List<RowBinding> _rows = new List<RowBinding>();

        public static bool Show(ItemWindow itemWindow)
        {
            if (itemWindow == null || SelectedMetas.selected_item == null)
            {
                return false;
            }

            VanillaModifierWindow window = itemWindow.GetComponentInChildren<VanillaModifierWindow>(true);
            if (window == null)
            {
                GameObject host = new GameObject("XRM_VanillaModifierController", typeof(VanillaModifierWindow));
                host.transform.SetParent(itemWindow.transform, false);
                window = host.GetComponent<VanillaModifierWindow>();
            }

            return window.Open(itemWindow, SelectedMetas.selected_item);
        }

        private void Update()
        {
            bool missingContext = _itemWindow == null || _item == null;
            bool itemWindowClosed = _itemWindow != null && (_itemWindow.gameObject == null || !_itemWindow.gameObject.activeInHierarchy);
            bool selectedItemChanged = SelectedMetas.selected_item != _item;
            if (missingContext || itemWindowClosed || selectedItemChanged)
            {
                ClosePanel();
            }
        }

        private void OnDestroy()
        {
            DestroyWindowHandle();
        }

        private bool Open(ItemWindow itemWindow, Item item)
        {
            _itemWindow = itemWindow;
            _item = item;
            _groups.Clear();
            _rows.Clear();

            CollectModifierGroups();

            if (!BuildRuntimeWindow())
            {
                Destroy(gameObject);
                return false;
            }

            BuildModifierRows();
            UpdateUiState();
            _windowHandle.Show();
            _windowHandle.BringToFront();
            return true;
        }

        private bool BuildRuntimeWindow()
        {
            DestroyWindowHandle();
            _windowHandle = XwmFiles.GetOrLoad(ModGuid, WindowFile, RuntimeId, true);
            if (_windowHandle == null)
            {
                return false;
            }

            _windowHandle.ConnectButtonClick("confirmButton", OnConfirmPressed);
            _windowHandle.ConnectButtonClick("cancelButton", ClosePanel);

            _windowHandle.SetText("titleLabel", Localize("xrm_ui_vanilla_title", "Vanilla Modifier Reforge"));
            _windowHandle.SetText("availableHeaderLabel", Localize("xrm_ui_vanilla_available", "Vanilla Modifiers"));
            _windowHandle.SetText("selectedHeaderLabel", Localize("xrm_ui_vanilla_selected", "Selected Modifiers"));
            _windowHandle.SetText("confirmButton", Localize("xrm_ui_vanilla_confirm", "Apply Reforge"));
            _windowHandle.SetText("cancelButton", Localize("xrm_ui_vanilla_cancel", "Cancel"));
            _windowHandle.SetText("modeCounterLabel", LocalizeFormat("xrm_ui_vanilla_counter", "Selected {0}/{1}", 0, 0));
            _windowHandle.SetActive("modifierTemplateRow", false);

            return true;
        }

        private void CollectModifierGroups()
        {
            if (_item == null || _item.getAsset() == null || AssetManager.items_modifiers == null || AssetManager.items_modifiers.list == null)
            {
                return;
            }

            string poolToken = XrmBuffRegistry.GetPoolToken(_item.getAsset().equipment_type);

            Dictionary<string, ModifierGroup> groupsByType = new Dictionary<string, ModifierGroup>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(poolToken))
            {
                for (int i = 0; i < AssetManager.items_modifiers.list.Count; i++)
                {
                    ItemModAsset mod = AssetManager.items_modifiers.list[i];
                    if (!IsVanillaModifierCandidate(mod, poolToken))
                    {
                        continue;
                    }

                    RegisterModifierGroup(groupsByType, mod);
                }
            }

            if (groupsByType.Count == 0)
            {
                for (int i = 0; i < AssetManager.items_modifiers.list.Count; i++)
                {
                    ItemModAsset mod = AssetManager.items_modifiers.list[i];
                    if (!IsVanillaModifierFallbackCandidate(mod))
                    {
                        continue;
                    }

                    RegisterModifierGroup(groupsByType, mod);
                }
            }

            foreach (KeyValuePair<string, ModifierGroup> pair in groupsByType)
            {
                ModifierGroup group = pair.Value;
                group.Variants.Sort(CompareModifierVariants);
                if (group.Variants.Count > 0)
                {
                    _groups.Add(group);
                }
            }

            _groups.Sort(delegate(ModifierGroup left, ModifierGroup right)
            {
                return string.Compare(GetGroupTitle(left), GetGroupTitle(right), StringComparison.OrdinalIgnoreCase);
            });
        }

        private static void RegisterModifierGroup(Dictionary<string, ModifierGroup> groupsByType, ItemModAsset mod)
        {
            if (groupsByType == null || mod == null)
            {
                return;
            }

            string modType = string.IsNullOrEmpty(mod.mod_type) ? mod.id : mod.mod_type;
            ModifierGroup group;
            if (!groupsByType.TryGetValue(modType, out group))
            {
                group = new ModifierGroup(modType);
                groupsByType[modType] = group;
            }

            group.Variants.Add(mod);
        }

        private static bool IsVanillaModifierCandidate(ItemModAsset mod, string poolToken)
        {
            if (mod == null || !mod.mod_can_be_given)
            {
                return false;
            }

            if (string.IsNullOrEmpty(mod.id) || mod.id.StartsWith("xrm_", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mod.pool) || string.IsNullOrEmpty(poolToken) || !mod.pool.Contains(poolToken))
            {
                return false;
            }

            string modType = string.IsNullOrEmpty(mod.mod_type) ? mod.id : mod.mod_type;
            return AllowedVanillaTypes.Contains(modType);
        }

        private static bool IsVanillaModifierFallbackCandidate(ItemModAsset mod)
        {
            if (mod == null || !mod.mod_can_be_given)
            {
                return false;
            }

            if (string.IsNullOrEmpty(mod.id) || mod.id.StartsWith("xrm_", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.Equals(mod.id, "normal", StringComparison.OrdinalIgnoreCase)
                || string.Equals(mod.id, "eternal", StringComparison.OrdinalIgnoreCase)
                || string.Equals(mod.id, "cursed", StringComparison.OrdinalIgnoreCase)
                || string.Equals(mod.id, "divine_rune", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mod.pool))
            {
                return false;
            }

            return mod.pool.Contains("weapon") || mod.pool.Contains("armor") || mod.pool.Contains("accessory");
        }

        private static int CompareModifierVariants(ItemModAsset left, ItemModAsset right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }

            if (left == null)
            {
                return 1;
            }

            if (right == null)
            {
                return -1;
            }

            int rankCompare = left.mod_rank.CompareTo(right.mod_rank);
            if (rankCompare != 0)
            {
                return rankCompare;
            }

            int qualityCompare = left.quality.CompareTo(right.quality);
            if (qualityCompare != 0)
            {
                return qualityCompare;
            }

            return string.Compare(left.id, right.id, StringComparison.OrdinalIgnoreCase);
        }

        private void BuildModifierRows()
        {
            for (int index = 0; index < _groups.Count; index++)
            {
                ModifierGroup group = _groups[index];
                XwmElementRef rowElement = _windowHandle.Duplicate("modifierTemplateRow", new Vector2(0f, index * 62f));
                if (rowElement == null)
                {
                    continue;
                }

                rowElement.IsActive = true;

                Text nameLabel = FindChildText(rowElement.GameObject != null ? rowElement.GameObject.transform : null, "ModifierNameLabel");
                Text valueLabel = FindChildText(rowElement.GameObject != null ? rowElement.GameObject.transform : null, "ModifierValueLabel");
                RectTransform sliderHost = FindChildRect(rowElement.GameObject != null ? rowElement.GameObject.transform : null, "SliderHostFrame");
                if (sliderHost == null)
                {
                    sliderHost = rowElement.RectTransform;
                }

                Slider slider = CreateSnapSlider(sliderHost, group.Variants.Count);
                RowBinding binding = new RowBinding(group, slider, nameLabel, valueLabel);
                if (binding.NameLabel != null)
                {
                    binding.NameLabel.text = GetGroupTitle(group);
                }

                if (slider != null)
                {
                    slider.onValueChanged.AddListener(delegate(float _)
                    {
                        OnSliderChanged(binding);
                    });
                }

                _rows.Add(binding);
                UpdateBindingValueLabel(binding);
            }
        }

        private Slider CreateSnapSlider(RectTransform parent, int maxLevel)
        {
            if (parent == null)
            {
                return null;
            }

            int clampedMaxLevel = Mathf.Max(1, maxLevel);

            GameObject sliderObject = new GameObject("ModifierSlider", typeof(RectTransform), typeof(Slider));
            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            sliderRect.SetParent(parent, false);
            sliderRect.anchorMin = Vector2.zero;
            sliderRect.anchorMax = Vector2.one;
            sliderRect.offsetMin = new Vector2(6f, 4f);
            sliderRect.offsetMax = new Vector2(-6f, -4f);

            Slider slider = sliderObject.GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = clampedMaxLevel;
            slider.wholeNumbers = true;

            GameObject backgroundObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.SetParent(sliderRect, false);
            backgroundRect.anchorMin = new Vector2(0f, 0.5f);
            backgroundRect.anchorMax = new Vector2(1f, 0.5f);
            backgroundRect.sizeDelta = new Vector2(0f, 8f);
            Image backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.color = new Color(0.12f, 0.17f, 0.24f, 1f);

            GameObject fillAreaObject = new GameObject("Fill Area", typeof(RectTransform));
            RectTransform fillAreaRect = fillAreaObject.GetComponent<RectTransform>();
            fillAreaRect.SetParent(sliderRect, false);
            fillAreaRect.anchorMin = new Vector2(0f, 0.5f);
            fillAreaRect.anchorMax = new Vector2(1f, 0.5f);
            fillAreaRect.sizeDelta = new Vector2(-2f, 8f);

            GameObject fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.SetParent(fillAreaRect, false);
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            Image fillImage = fillObject.GetComponent<Image>();
            fillImage.color = new Color(0.25f, 0.58f, 0.34f, 1f);

            GameObject handleObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            RectTransform handleRect = handleObject.GetComponent<RectTransform>();
            handleRect.SetParent(sliderRect, false);
            handleRect.anchorMin = new Vector2(0f, 0.5f);
            handleRect.anchorMax = new Vector2(0f, 0.5f);
            handleRect.sizeDelta = new Vector2(12f, 14f);
            Image handleImage = handleObject.GetComponent<Image>();
            handleImage.color = new Color(0.95f, 0.97f, 1f, 1f);

            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;

            CreateSliderTicks(sliderRect, clampedMaxLevel);

            return slider;
        }

        private static void CreateSliderTicks(RectTransform sliderRect, int maxLevel)
        {
            int points = Mathf.Max(1, maxLevel);
            for (int i = 0; i <= points; i++)
            {
                float t = i / (float)points;
                GameObject tickObject = new GameObject("Tick_" + i, typeof(RectTransform), typeof(Image));
                RectTransform tickRect = tickObject.GetComponent<RectTransform>();
                tickRect.SetParent(sliderRect, false);
                tickRect.anchorMin = new Vector2(t, 0.5f);
                tickRect.anchorMax = new Vector2(t, 0.5f);
                tickRect.sizeDelta = new Vector2(3f, 16f);
                Image tickImage = tickObject.GetComponent<Image>();
                tickImage.color = new Color(0.74f, 0.84f, 0.95f, 0.65f);
                tickImage.raycastTarget = false;
            }
        }

        private void OnSliderChanged(RowBinding binding)
        {
            UpdateBindingValueLabel(binding);
            UpdateUiState();
        }

        private void UpdateBindingValueLabel(RowBinding binding)
        {
            if (binding == null || binding.ValueLabel == null)
            {
                return;
            }

            ItemModAsset selected = GetSelectedModifier(binding);
            if (selected == null)
            {
                binding.ValueLabel.text = Localize("xrm_ui_vanilla_off", "Off");
                return;
            }

            int plusCount = Mathf.Clamp(selected.mod_rank, 1, 7);
            binding.ValueLabel.text = new string('+', plusCount);
        }

        private ItemModAsset GetSelectedModifier(RowBinding binding)
        {
            if (binding == null || binding.Group == null || binding.Slider == null)
            {
                return null;
            }

            int level = Mathf.RoundToInt(binding.Slider.value);
            if (level <= 0 || binding.Group.Variants.Count == 0)
            {
                return null;
            }

            int index = Mathf.Clamp(level - 1, 0, binding.Group.Variants.Count - 1);
            return binding.Group.Variants[index];
        }

        private void UpdateUiState()
        {
            if (_windowHandle == null || _windowHandle.IsDestroyed)
            {
                return;
            }

            int selectedCount = 0;
            StringBuilder summaryBuilder = new StringBuilder();
            for (int i = 0; i < _rows.Count; i++)
            {
                RowBinding binding = _rows[i];
                ItemModAsset selected = GetSelectedModifier(binding);
                if (selected == null)
                {
                    continue;
                }

                selectedCount++;
                summaryBuilder.Append("- ");
                summaryBuilder.Append(GetModifierName(selected));
                string statSummary = BuildStatSummary(selected);
                if (!string.IsNullOrEmpty(statSummary))
                {
                    summaryBuilder.Append(": ");
                    summaryBuilder.Append(statSummary);
                }
                summaryBuilder.AppendLine();
            }

            _windowHandle.SetText("modeCounterLabel", LocalizeFormat("xrm_ui_vanilla_counter", "Selected {0}/{1}", selectedCount, _rows.Count));

            bool canConfirm = selectedCount > 0;
            _windowHandle.SetInteractable("confirmButton", canConfirm);
            XwmElementRef confirmButton = _windowHandle.Get("confirmButton");
            if (confirmButton != null)
            {
                confirmButton.SetImageColor(canConfirm ? ConfirmEnabledColor : ConfirmDisabledColor);
            }

            if (selectedCount == 0)
            {
                _windowHandle.SetText("selectedSummaryLabel", Localize("xrm_ui_vanilla_none", "No vanilla modifiers selected."));
            }
            else
            {
                _windowHandle.SetText("selectedSummaryLabel", summaryBuilder.ToString().TrimEnd('\n'));
            }
        }

        private string BuildStatSummary(ItemModAsset modifier)
        {
            if (modifier == null || modifier.base_stats == null)
            {
                return string.Empty;
            }

            List<BaseStatsContainer> stats = modifier.base_stats.getList();
            if (stats == null || stats.Count == 0)
            {
                if (modifier.action_attack_target != null)
                {
                    return Localize("xrm_ui_vanilla_special", "Special effect");
                }

                return Localize("xrm_ui_vanilla_no_stats", "No stat changes.");
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < stats.Count; i++)
            {
                BaseStatsContainer container = stats[i];
                if (container == null || Mathf.Abs(container.value) < 0.0001f)
                {
                    continue;
                }

                BaseStatAsset statAsset = container.asset;
                string localeKey = statAsset != null ? statAsset.getLocaleID() : container.id;
                string statName = LocalizedTextManager.getText(localeKey);
                if (string.IsNullOrEmpty(statName) || statName == localeKey)
                {
                    statName = container.id;
                }

                float value = container.value;
                bool showAsPercent = false;
                if (statAsset != null)
                {
                    if (Mathf.Abs(statAsset.tooltip_multiply_for_visual_number - 1f) > 0.001f)
                    {
                        value *= statAsset.tooltip_multiply_for_visual_number;
                    }

                    showAsPercent = statAsset.show_as_percents;
                }

                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(statName);
                builder.Append(' ');
                builder.Append(FormatSignedValue(value, showAsPercent));
            }

            if (modifier.action_attack_target != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(Localize("xrm_ui_vanilla_special", "Special effect"));
            }

            if (builder.Length == 0)
            {
                return Localize("xrm_ui_vanilla_no_stats", "No stat changes.");
            }

            return builder.ToString();
        }

        private static string FormatSignedValue(float value, bool showAsPercent)
        {
            float absValue = Mathf.Abs(value);
            string formatted = absValue >= 1f ? absValue.ToString("0.#") : absValue.ToString("0.##");
            return (value >= 0f ? "+" : "-") + formatted + (showAsPercent ? "%" : string.Empty);
        }

        private void OnConfirmPressed()
        {
            if (_item == null)
            {
                ClosePanel();
                return;
            }

            HashSet<string> selectedModifierIds = new HashSet<string>();
            for (int i = 0; i < _rows.Count; i++)
            {
                ItemModAsset selected = GetSelectedModifier(_rows[i]);
                if (selected != null)
                {
                    selectedModifierIds.Add(selected.id);
                }
            }

            if (selectedModifierIds.Count == 0)
            {
                ClosePanel();
                return;
            }

            XrmReforgeService.ApplyVanillaReforge(_item, selectedModifierIds);

            if (_itemWindow != null)
            {
                try
                {
                    Traverse.Create(_itemWindow).Method("updateStates").GetValue();
                }
                catch
                {
                }
            }

            ClosePanel();
        }

        private void ClosePanel()
        {
            DestroyWindowHandle();
            Destroy(gameObject);
        }

        private void DestroyWindowHandle()
        {
            if (_windowHandle != null && !_windowHandle.IsDestroyed)
            {
                _windowHandle.Destroy();
            }

            _windowHandle = null;
            _rows.Clear();
        }

        private string GetGroupTitle(ModifierGroup group)
        {
            if (group == null || group.Variants.Count == 0)
            {
                return string.Empty;
            }

            return GetModifierName(group.Variants[0]);
        }

        private string GetModifierName(ItemModAsset modifier)
        {
            if (modifier == null)
            {
                return string.Empty;
            }

            string localeKey = modifier.getLocaleID();
            if (string.IsNullOrEmpty(localeKey))
            {
                return modifier.id;
            }

            string localized = LocalizedTextManager.getText(localeKey);
            if (string.IsNullOrEmpty(localized) || localized == localeKey)
            {
                return modifier.id;
            }

            return localized;
        }

        private string Localize(string key, string fallback)
        {
            string text = LocalizedTextManager.getText(key);
            if (string.IsNullOrEmpty(text) || text == key)
            {
                return fallback;
            }

            return text;
        }

        private string LocalizeFormat(string key, string fallback, params object[] args)
        {
            string format = Localize(key, fallback);
            try
            {
                return string.Format(format, args);
            }
            catch
            {
                return fallback;
            }
        }

        private static Text FindChildText(Transform root, string childName)
        {
            if (root == null || string.IsNullOrEmpty(childName))
            {
                return null;
            }

            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child != null && string.Equals(child.name, childName, StringComparison.Ordinal))
                {
                    return child.GetComponent<Text>();
                }
            }

            return null;
        }

        private static RectTransform FindChildRect(Transform root, string childName)
        {
            if (root == null || string.IsNullOrEmpty(childName))
            {
                return null;
            }

            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child != null && string.Equals(child.name, childName, StringComparison.Ordinal))
                {
                    return child as RectTransform;
                }
            }

            return null;
        }

        private sealed class ModifierGroup
        {
            public readonly string ModType;
            public readonly List<ItemModAsset> Variants = new List<ItemModAsset>();

            public ModifierGroup(string modType)
            {
                ModType = modType;
            }
        }

        private sealed class RowBinding
        {
            public readonly ModifierGroup Group;
            public readonly Slider Slider;
            public readonly Text NameLabel;
            public readonly Text ValueLabel;

            public RowBinding(ModifierGroup group, Slider slider, Text nameLabel, Text valueLabel)
            {
                Group = group;
                Slider = slider;
                NameLabel = nameLabel;
                ValueLabel = valueLabel;
            }
        }
    }
}
