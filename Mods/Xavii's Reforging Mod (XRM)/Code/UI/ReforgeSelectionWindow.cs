using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using XaviiWindowsMod.API;
using XRM.Code.Content;

namespace XRM.Code.UI
{
    internal sealed class ReforgeSelectionWindow : MonoBehaviour
    {
        private const string ModGuid = "com.xavii.xrm";
        private const string WindowFile = "reforge_selection";
        private const string RuntimeId = "com.xavii.xrm::reforge_selection";

        private static readonly Color UnselectedButtonColor = new Color(0.21f, 0.29f, 0.39f, 1f);
        private static readonly Color SelectedButtonColor = new Color(0.24f, 0.56f, 0.32f, 1f);
        private static readonly Color UnselectedTextColor = new Color(0.94f, 0.96f, 0.99f, 1f);
        private static readonly Color SelectedTextColor = Color.white;
        private static readonly Color ConfirmEnabledColor = new Color(0.20f, 0.47f, 0.24f, 1f);
        private static readonly Color ConfirmDisabledColor = new Color(0.23f, 0.30f, 0.36f, 1f);

        private ItemWindow _itemWindow;
        private Item _item;
        private int _maxSelections;
        private bool _divineMode;
        private XwmWindowHandle _windowHandle;
        private readonly HashSet<string> _selectedIds = new HashSet<string>();
        private readonly List<XrmBuffDefinition> _availableBuffs = new List<XrmBuffDefinition>();
        private readonly Dictionary<string, XwmElementRef> _buffButtons = new Dictionary<string, XwmElementRef>();

        public static bool Show(ItemWindow itemWindow, int maxSelections, bool divineMode)
        {
            if (itemWindow == null || SelectedMetas.selected_item == null)
            {
                return false;
            }

            ReforgeSelectionWindow window = itemWindow.GetComponentInChildren<ReforgeSelectionWindow>(true);
            if (window == null)
            {
                GameObject host = new GameObject("XRM_ReforgeSelectionController", typeof(ReforgeSelectionWindow));
                host.transform.SetParent(itemWindow.transform, false);
                window = host.GetComponent<ReforgeSelectionWindow>();
            }

            return window.Open(itemWindow, SelectedMetas.selected_item, maxSelections, divineMode);
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

        private bool Open(ItemWindow itemWindow, Item item, int maxSelections, bool divineMode)
        {
            _itemWindow = itemWindow;
            _item = item;
            _maxSelections = Mathf.Max(1, maxSelections);
            _divineMode = divineMode;
            _selectedIds.Clear();
            _availableBuffs.Clear();
            _buffButtons.Clear();

            if (_item != null && _item.getAsset() != null)
            {
                _availableBuffs.AddRange(XrmBuffRegistry.GetAvailableBuffs(_item.getAsset().equipment_type));
            }
            if (_availableBuffs.Count == 0)
            {
                _availableBuffs.AddRange(XrmBuffRegistry.GetAllBuffs());
            }

            if (!BuildRuntimeWindow())
            {
                Destroy(gameObject);
                return false;
            }

            BuildBuffButtons();
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

            _windowHandle.SetText("titleLabel", Localize("xrm_ui_reforge_title", "Reforging Selection"));
            _windowHandle.SetText("availableHeaderLabel", Localize("xrm_ui_reforge_available", "Available Buffs"));
            _windowHandle.SetText("selectedHeaderLabel", Localize("xrm_ui_reforge_selected", "Selected Buffs"));
            _windowHandle.SetText("collisionHeaderLabel", Localize("xrm_ui_reforge_collisions", "Collisions"));
            _windowHandle.SetText("confirmButton", Localize("xrm_ui_reforge_confirm", "Confirm Reforge"));
            _windowHandle.SetText("cancelButton", Localize("xrm_ui_reforge_cancel", "Cancel"));
            _windowHandle.SetActive("buffTemplateLeftButton", false);
            _windowHandle.SetActive("buffTemplateRightButton", false);

            return true;
        }

        private void BuildBuffButtons()
        {
            for (int index = 0; index < _availableBuffs.Count; index++)
            {
                XrmBuffDefinition buff = _availableBuffs[index];
                int row = index / 2;
                bool leftColumn = index % 2 == 0;
                string templateId = leftColumn ? "buffTemplateLeftButton" : "buffTemplateRightButton";
                Vector2 offset = new Vector2(0f, row * 42f);
                XwmElementRef element = _windowHandle.Duplicate(templateId, offset);
                if (element == null)
                {
                    continue;
                }

                element.IsActive = true;
                SetElementText(element, Localize(buff.NameKey, buff.Id));
                element.SetImageColor(UnselectedButtonColor);
                SetElementTextColor(element, UnselectedTextColor);

                string buffId = buff.Id;
                element.ConnectClick(delegate
                {
                    ToggleSelection(buffId);
                });

                _buffButtons[buffId] = element;
            }
        }

        private void ToggleSelection(string buffId)
        {
            if (_selectedIds.Contains(buffId))
            {
                _selectedIds.Remove(buffId);
            }
            else if (_selectedIds.Count < _maxSelections)
            {
                _selectedIds.Add(buffId);
            }

            UpdateUiState();
        }

        private void UpdateUiState()
        {
            if (_windowHandle == null || _windowHandle.IsDestroyed)
            {
                return;
            }

            string modeText = _divineMode
                ? Localize("xrm_ui_reforge_divine", "Divine Reforge")
                : Localize("xrm_ui_reforge_standard", "Standard Reforge");

            _windowHandle.SetText("modeCounterLabel", modeText + "  " + _selectedIds.Count + "/" + _maxSelections);

            foreach (KeyValuePair<string, XwmElementRef> entry in _buffButtons)
            {
                bool isSelected = _selectedIds.Contains(entry.Key);
                entry.Value.SetImageColor(isSelected ? SelectedButtonColor : UnselectedButtonColor);
                SetElementTextColor(entry.Value, isSelected ? SelectedTextColor : UnselectedTextColor);
            }

            bool canConfirm = _selectedIds.Count > 0;
            _windowHandle.SetInteractable("confirmButton", canConfirm);
            XwmElementRef confirmButton = _windowHandle.Get("confirmButton");
            if (confirmButton != null)
            {
                confirmButton.SetImageColor(canConfirm ? ConfirmEnabledColor : ConfirmDisabledColor);
            }

            StringBuilder selectedBuilder = new StringBuilder();
            if (_availableBuffs.Count == 0)
            {
                selectedBuilder.Append(Localize("xrm_ui_reforge_no_buffs", "No buffs available for this item type."));
            }
            else if (_selectedIds.Count == 0)
            {
                selectedBuilder.Append(Localize("xrm_ui_reforge_none_selected", "No buffs selected."));
            }
            else
            {
                for (int i = 0; i < _availableBuffs.Count; i++)
                {
                    XrmBuffDefinition buff = _availableBuffs[i];
                    if (!_selectedIds.Contains(buff.Id))
                    {
                        continue;
                    }

                    selectedBuilder.Append("- ");
                    selectedBuilder.Append(Localize(buff.NameKey, buff.Id));
                    selectedBuilder.Append(": ");
                    selectedBuilder.Append(buff.Summary);
                    selectedBuilder.AppendLine();
                }
            }

            _windowHandle.SetText("selectedSummaryLabel", selectedBuilder.ToString().TrimEnd('\n'));

            List<XrmCollisionDefinition> collisions = XrmBuffRegistry.GetTriggeredCollisions(_selectedIds);
            if (collisions.Count == 0)
            {
                _windowHandle.SetText("collisionSummaryLabel", Localize("xrm_ui_reforge_no_collision", "No collisions triggered."));
            }
            else
            {
                StringBuilder collisionBuilder = new StringBuilder();
                for (int i = 0; i < collisions.Count; i++)
                {
                    XrmCollisionDefinition collision = collisions[i];
                    collisionBuilder.Append("- ");
                    collisionBuilder.Append(Localize(collision.PenaltyNameKey, collision.PenaltyId));
                    collisionBuilder.Append(": ");
                    collisionBuilder.Append(collision.Summary);
                    collisionBuilder.AppendLine();
                }

                _windowHandle.SetText("collisionSummaryLabel", collisionBuilder.ToString().TrimEnd('\n'));
            }
        }

        private void OnConfirmPressed()
        {
            if (_item == null)
            {
                ClosePanel();
                return;
            }

            XrmReforgeService.ApplyReforge(_item, _selectedIds);

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
            _buffButtons.Clear();
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

        private static void SetElementText(XwmElementRef element, string value)
        {
            if (element == null)
            {
                return;
            }

            element.SetText(value);
            Text label = ResolveElementTextComponent(element);
            if (label != null)
            {
                label.text = value ?? string.Empty;
            }
        }

        private static void SetElementTextColor(XwmElementRef element, Color color)
        {
            if (element == null)
            {
                return;
            }

            element.SetTextColor(color);
            Text label = ResolveElementTextComponent(element);
            if (label != null)
            {
                label.color = color;
            }
        }

        private static Text ResolveElementTextComponent(XwmElementRef element)
        {
            if (element == null || element.GameObject == null)
            {
                return null;
            }

            if (element.Text != null)
            {
                return element.Text;
            }

            return element.GameObject.GetComponentInChildren<Text>(true);
        }
    }
}
