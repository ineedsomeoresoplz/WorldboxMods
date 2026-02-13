using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AIBox.UI
{
    /// <summary>
    /// Item in a ComboBox dropdown
    /// </summary>
    public class ComboBoxItem
    {
        public string Id;
        public string DisplayText;
        public string SubText;      // Secondary text (e.g., cost tier)
        public bool IsSeparator;
        public bool IsCustomInput;
        public object Data;         // Arbitrary data attached to item

        public ComboBoxItem(string id, string displayText, string subText = null, object data = null)
        {
            Id = id;
            DisplayText = displayText;
            SubText = subText;
            Data = data;
            IsSeparator = false;
            IsCustomInput = false;
        }

        public static ComboBoxItem Separator() => new ComboBoxItem("_sep_", "") { IsSeparator = true };
        public static ComboBoxItem CustomInput(string placeholder = "Custom...") => 
            new ComboBoxItem("_custom_", placeholder) { IsCustomInput = true };
    }

    /// <summary>
    /// A dropdown control with optional text input capability
    /// Similar to HTML's <select> with editable option
    /// </summary>
    public class ComboBoxUI : MonoBehaviour
    {
        // UI Elements
        private GameObject _dropdownButton;
        private Text _selectedText;
        private Text _subText;
        private GameObject _dropdownPanel;
        private Transform _itemsContainer;
        private InputField _customInput;
        private ScrollRect _scrollRect;

        // State
        private List<ComboBoxItem> _items = new List<ComboBoxItem>();
        private ComboBoxItem _selectedItem;
        private bool _isOpen = false;
        private bool _allowCustomInput = true;

        // Events
        public Action<ComboBoxItem> OnSelectionChanged;
        public Action<string> OnCustomValueEntered;

        // Prefab references (set at runtime)
        private static GameObject _prefab;

        // ═══════════════════════════════════════════════════════════════
        // Factory
        // ═══════════════════════════════════════════════════════════════

        public static ComboBoxUI Create(Transform parent, string label = "")
        {
            GameObject go = new GameObject("ComboBox", typeof(RectTransform), typeof(ComboBoxUI), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            
            // Setup to stretch to fill parent
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            LayoutElement le = go.GetComponent<LayoutElement>();
            le.flexibleWidth = 1;
            le.minHeight = 22;
            le.preferredHeight = 22;
            
            ComboBoxUI combo = go.GetComponent<ComboBoxUI>();
            combo.Initialize(label);
            return combo;
        }

        // ═══════════════════════════════════════════════════════════════
        // Setup
        // ═══════════════════════════════════════════════════════════════

        private void Initialize(string label)
        {
            // Main Button - fills the combobox area
            _dropdownButton = new GameObject("DropdownButton", typeof(RectTransform), typeof(Image), typeof(Button));
            _dropdownButton.transform.SetParent(transform, false);

            Image btnImg = _dropdownButton.GetComponent<Image>();
            btnImg.sprite = SpriteTextureLoader.getSprite("ui/special/button2");
            btnImg.type = Image.Type.Sliced;
            btnImg.color = new Color(0.25f, 0.25f, 0.28f);

            RectTransform btnRt = _dropdownButton.GetComponent<RectTransform>();
            btnRt.anchorMin = Vector2.zero;
            btnRt.anchorMax = Vector2.one;
            btnRt.offsetMin = Vector2.zero;
            btnRt.offsetMax = Vector2.zero;
            
            // Selected Text
            GameObject textGo = new GameObject("SelectedText", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(_dropdownButton.transform, false);
            _selectedText = textGo.GetComponent<Text>();
            SetupText(_selectedText, TextAnchor.MiddleLeft, 8);
            RectTransform textRt = textGo.GetComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0, 0);
            textRt.anchorMax = new Vector2(1, 1);
            textRt.offsetMin = new Vector2(10, 0);
            textRt.offsetMax = new Vector2(-30, 0);

            // SubText (for metadata like cost)
            GameObject subGo = new GameObject("SubText", typeof(RectTransform), typeof(Text));
            subGo.transform.SetParent(_dropdownButton.transform, false);
            _subText = subGo.GetComponent<Text>();
            SetupText(_subText, TextAnchor.MiddleRight, 7);
            _subText.color = new Color(0.7f, 0.7f, 0.7f);
            RectTransform subRt = subGo.GetComponent<RectTransform>();
            subRt.anchorMin = new Vector2(0, 0);
            subRt.anchorMax = new Vector2(1, 1);
            subRt.offsetMin = new Vector2(150, 0);
            subRt.offsetMax = new Vector2(-30, 0);

            // Arrow indicator
            GameObject arrowGo = new GameObject("Arrow", typeof(RectTransform), typeof(Text));
            arrowGo.transform.SetParent(_dropdownButton.transform, false);
            Text arrowText = arrowGo.GetComponent<Text>();
            SetupText(arrowText, TextAnchor.MiddleCenter, 8);
            arrowText.text = "▼";
            RectTransform arrowRt = arrowGo.GetComponent<RectTransform>();
            arrowRt.anchorMin = new Vector2(1, 0);
            arrowRt.anchorMax = new Vector2(1, 1);
            arrowRt.sizeDelta = new Vector2(25, 0);
            arrowRt.anchoredPosition = new Vector2(-12.5f, 0);

            // Dropdown Panel (hidden by default)
            CreateDropdownPanel();

            // Button click handler
            Button btn = _dropdownButton.GetComponent<Button>();
            btn.onClick.AddListener(ToggleDropdown);

            // Start closed
            _dropdownPanel.SetActive(false);

            _selectedText.text = "Select...";
        }

        private void CreateDropdownPanel()
        {
            // Add RectMask2D to clip children
            _dropdownPanel = new GameObject("DropdownPanel", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(RectMask2D));
            _dropdownPanel.transform.SetParent(transform, false);
            
            // Add Canvas to make it render on top
            Canvas canvas = _dropdownPanel.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 3000; // High value to ensure top
            _dropdownPanel.AddComponent<GraphicRaycaster>();
            
            Image panelBg = _dropdownPanel.GetComponent<Image>();
            panelBg.color = new Color(0.15f, 0.15f, 0.15f, 0.98f);
            panelBg.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
            panelBg.type = Image.Type.Sliced;

            RectTransform panelRt = _dropdownPanel.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(0, 1);
            panelRt.anchorMax = new Vector2(1, 1);
            panelRt.pivot = new Vector2(0.5f, 1);
            panelRt.anchoredPosition = new Vector2(0, -32);
            panelRt.sizeDelta = new Vector2(0, 200);

            VerticalLayoutGroup layout = _dropdownPanel.GetComponent<VerticalLayoutGroup>();
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.padding = new RectOffset(4, 4, 4, 4);
            layout.spacing = 2;

            // Scroll View
            GameObject scrollGo = new GameObject("ScrollView", typeof(RectTransform), typeof(ScrollRect));
            scrollGo.transform.SetParent(_dropdownPanel.transform, false);
            scrollGo.transform.localScale = Vector3.one;
            _scrollRect = scrollGo.GetComponent<ScrollRect>();
            
            RectTransform scrollRt = scrollGo.GetComponent<RectTransform>();
            scrollRt.anchorMin = Vector2.zero;
            scrollRt.anchorMax = Vector2.one;
            scrollRt.offsetMin = Vector2.zero;
            _scrollRect.movementType = ScrollRect.MovementType.Clamped; // Prevent overscrolling

            // Viewport
            GameObject viewportGo = new GameObject("Viewport", typeof(RectTransform));
            viewportGo.transform.SetParent(scrollGo.transform, false);
            viewportGo.transform.localScale = Vector3.one;
            
            // NOTE: REMOVED MASKING (Image/Mask/RectMask2D) to ensure visibility
            // This means items might spill out, but they will be visible.
            
            RectTransform viewportRt = viewportGo.GetComponent<RectTransform>();
            viewportRt.anchorMin = Vector2.zero;
            viewportRt.anchorMax = Vector2.one;
            viewportRt.offsetMin = Vector2.zero;
            viewportRt.offsetMax = Vector2.zero;

            // Content
            GameObject contentGo = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            contentGo.transform.SetParent(viewportGo.transform, false);
            contentGo.transform.localScale = Vector3.one;
            _itemsContainer = contentGo.transform;

            RectTransform contentRt = contentGo.GetComponent<RectTransform>();
            contentRt.anchorMin = new Vector2(0, 1);
            contentRt.anchorMax = new Vector2(1, 1);
            contentRt.pivot = new Vector2(0.5f, 1);
            contentRt.anchoredPosition = Vector2.zero;
            contentRt.sizeDelta = new Vector2(0, 0);

            VerticalLayoutGroup contentLayout = contentGo.GetComponent<VerticalLayoutGroup>();
            contentLayout.childControlHeight = true;
            contentLayout.childControlWidth = true;
            contentLayout.childForceExpandHeight = false;
            contentLayout.childForceExpandWidth = true;
            contentLayout.spacing = 2;

            ContentSizeFitter fitter = contentGo.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _scrollRect.content = contentRt;
            _scrollRect.viewport = viewportRt;
            _scrollRect.horizontal = false;
            _scrollRect.vertical = true;
        }

        // ═══════════════════════════════════════════════════════════════
        // Public API
        // ═══════════════════════════════════════════════════════════════

        public void SetItems(List<ComboBoxItem> items)
        {
            _items = items ?? new List<ComboBoxItem>();
            RebuildDropdownItems();
        }

        public void AddItem(ComboBoxItem item)
        {
            _items.Add(item);
            RebuildDropdownItems();
        }

        public void ClearItems()
        {
            _items.Clear();
            RebuildDropdownItems();
        }

        public void SetSelectedById(string id)
        {
            var item = _items.Find(i => i.Id == id);
            if (item != null)
            {
                SelectItem(item, false);
            }
        }

        public void SetSelectedByIndex(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                SelectItem(_items[index], false);
            }
        }

        public ComboBoxItem GetSelected() => _selectedItem;
        public string GetSelectedId() => _selectedItem?.Id;

        public void SetAllowCustomInput(bool allow)
        {
            _allowCustomInput = allow;
        }

        // ═══════════════════════════════════════════════════════════════
        // Internal
        // ═══════════════════════════════════════════════════════════════

        private void RebuildDropdownItems()
        {
            // Clear existing
            foreach (Transform child in _itemsContainer)
            {
                Destroy(child.gameObject);
            }

            // Create items
            foreach (var item in _items)
            {
                if (item.IsSeparator)
                {
                    CreateSeparator();
                }
                else if (item.IsCustomInput)
                {
                    CreateCustomInputItem(item.DisplayText);
                }
                else
                {
                    CreateDropdownItem(item);
                }
            }

            // Add custom input at end if allowed
            if (_allowCustomInput && !_items.Exists(i => i.IsCustomInput))
            {
                CreateSeparator();
                CreateCustomInputItem("Enter custom...");
                if (_itemsContainer.GetComponent<ContentSizeFitter>() != null)
                {
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer.GetComponent<RectTransform>());
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer.GetComponent<RectTransform>());
        }

        private void CreateDropdownItem(ComboBoxItem item)
        {
            GameObject itemGo = new GameObject("Item_" + item.Id, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            itemGo.transform.SetParent(_itemsContainer, false);

            Image bg = itemGo.GetComponent<Image>();
            bg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            bg.sprite = SpriteTextureLoader.getSprite("ui/special/button2");
            bg.type = Image.Type.Sliced;

            LayoutElement le = itemGo.GetComponent<LayoutElement>();
            le.minHeight = 22;
            le.preferredHeight = 22;

            // Text
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(itemGo.transform, false);
            Text txt = textGo.GetComponent<Text>();
            SetupText(txt, TextAnchor.MiddleLeft, 7);
            txt.text = item.DisplayText;
            
            RectTransform textRt = textGo.GetComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0, 0);
            textRt.anchorMax = new Vector2(0.6f, 1);
            textRt.offsetMin = new Vector2(8, 0);
            textRt.offsetMax = new Vector2(0, 0);

            // SubText (right side)
            if (!string.IsNullOrEmpty(item.SubText))
            {
                GameObject subGo = new GameObject("SubText", typeof(RectTransform), typeof(Text));
                subGo.transform.SetParent(itemGo.transform, false);
                Text subTxt = subGo.GetComponent<Text>();
                SetupText(subTxt, TextAnchor.MiddleRight, 6);
                subTxt.text = item.SubText;
                subTxt.color = new Color(0.6f, 0.8f, 0.6f);
                
                RectTransform subRt = subGo.GetComponent<RectTransform>();
                subRt.anchorMin = new Vector2(0.6f, 0);
                subRt.anchorMax = new Vector2(1, 1);
                subRt.offsetMin = new Vector2(0, 0);
                subRt.offsetMax = new Vector2(-8, 0);
            }

            // Click handler
            Button btn = itemGo.GetComponent<Button>();
            ComboBoxItem capturedItem = item; // Capture for lambda
            btn.onClick.AddListener(() => SelectItem(capturedItem, true));
        }

        private void CreateSeparator()
        {
            GameObject sepGo = new GameObject("Separator", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            sepGo.transform.SetParent(_itemsContainer, false);

            Image line = sepGo.GetComponent<Image>();
            line.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);

            LayoutElement le = sepGo.GetComponent<LayoutElement>();
            le.minHeight = 2;
            le.preferredHeight = 2;
        }

        private void CreateCustomInputItem(string placeholder)
        {
            GameObject itemGo = new GameObject("CustomInput", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            itemGo.transform.SetParent(_itemsContainer, false);

            Image bg = itemGo.GetComponent<Image>();
            bg.color = new Color(0.25f, 0.25f, 0.3f, 0.9f);
            bg.sprite = SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty");
            bg.type = Image.Type.Sliced;

            LayoutElement le = itemGo.GetComponent<LayoutElement>();
            le.minHeight = 24;
            le.preferredHeight = 24;

            // Input Field
            GameObject inputGo = new GameObject("InputField", typeof(RectTransform), typeof(Text), typeof(InputField));
            inputGo.transform.SetParent(itemGo.transform, false);

            Text inputText = inputGo.GetComponent<Text>();
            SetupText(inputText, TextAnchor.MiddleLeft, 8);

            _customInput = inputGo.GetComponent<InputField>();
            _customInput.textComponent = inputText;
            _customInput.text = "";
            _customInput.placeholder = CreatePlaceholder(inputGo.transform, placeholder);
            
            RectTransform inputRt = inputGo.GetComponent<RectTransform>();
            inputRt.anchorMin = Vector2.zero;
            inputRt.anchorMax = Vector2.one;
            inputRt.offsetMin = new Vector2(8, 2);
            inputRt.offsetMax = new Vector2(-8, -2);

            _customInput.onEndEdit.AddListener(OnCustomInputSubmit);
        }

        private Graphic CreatePlaceholder(Transform parent, string text)
        {
            GameObject go = new GameObject("Placeholder", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            
            Text txt = go.GetComponent<Text>();
            SetupText(txt, TextAnchor.MiddleLeft, 8);
            txt.text = text;
            txt.fontStyle = FontStyle.Italic;
            txt.color = new Color(0.5f, 0.5f, 0.5f);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            return txt;
        }

        private void SelectItem(ComboBoxItem item, bool triggerEvent)
        {
            _selectedItem = item;
            _selectedText.text = item.DisplayText;
            _subText.text = item.SubText ?? "";
            
            CloseDropdown();

            if (triggerEvent)
            {
                OnSelectionChanged?.Invoke(item);
            }
        }

        private void OnCustomInputSubmit(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            // Create a custom item
            var customItem = new ComboBoxItem("custom_" + value, value, "Custom");
            SelectItem(customItem, false);
            
            OnCustomValueEntered?.Invoke(value);
            CloseDropdown();
        }

        private void ToggleDropdown()
        {
            if (_isOpen) CloseDropdown();
            else OpenDropdown();
        }

        private void OpenDropdown()
        {
            _dropdownPanel.SetActive(true);
            _isOpen = true;
        }

        private void CloseDropdown()
        {
            _dropdownPanel.SetActive(false);
            _isOpen = false;
        }

        private void Update()
        {
            // Close on click outside
            if (_isOpen && Input.GetMouseButtonDown(0))
            {
                // Check if click is outside our bounds
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                    GetComponent<RectTransform>(), Input.mousePosition, null) &&
                    !RectTransformUtility.RectangleContainsScreenPoint(
                    _dropdownPanel.GetComponent<RectTransform>(), Input.mousePosition, null))
                {
                    CloseDropdown();
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════════

        private GameObject CreateButton(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;

            Image img = go.GetComponent<Image>();
            img.sprite = SpriteTextureLoader.getSprite("ui/special/button2");
            img.type = Image.Type.Sliced;
            img.color = new Color(0.25f, 0.25f, 0.28f);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.anchoredPosition = position;
            rt.sizeDelta = new Vector2(0, size.y);

            return go;
        }

        private void SetupText(Text text, TextAnchor alignment, int fontSize)
        {
            text.font = LocalizedTextManager.current_font ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }
}
