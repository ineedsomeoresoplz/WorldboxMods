using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AIBox.Provider;
using NeoModLoader.General;

namespace AIBox.UI
{
    /// <summary>
    /// Configuration window for AI provider and model selection
    /// </summary>
    public class AIProviderWindow : MonoBehaviour
    {
        private static AIProviderWindow _instance;
        private static GameObject _windowObject;

        // UI Elements
        private ComboBoxUI _providerCombo;
        private ComboBoxUI _modelCombo;
        private InputField _apiKeyInput;
        private InputField _customUrlInput;
        private Text _statusText;
        private Text _modelInfoText;
        private Button _testButton;
        private Button _refreshButton;
        private GameObject _apiKeyRow;
        private GameObject _customUrlRow;

        // State
        private ProviderInfo _currentProvider;
        private bool _isTestingConnection = false;

        public static void Show()
        {
            if (_windowObject != null)
            {
                _windowObject.SetActive(true);
                return;
            }
            
            // Create using WindowManager
            _windowObject = WindowManager.Instance.CreateWindow("Provider Config", new Vector2(330, 260), Vector2.zero);
            
            _instance = _windowObject.AddComponent<AIProviderWindow>();
            _instance.BuildUI(_windowObject.transform);
        }

        public static void Hide()
        {
            if (_windowObject != null)
            {
                // Reload settings on close to ensure they apply
                if (KingdomController.Instance != null)
                {
                    KingdomController.Instance.ReloadFromProviderSettings();
                }
                
                Destroy(_windowObject);
                _windowObject = null;
                _instance = null;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // UI Construction
        // ═══════════════════════════════════════════════════════════════

        private void BuildUI(Transform parent)
        {
            // Close Button
            WindowHelper.CreateHeader(parent, "⚙️ AI Better Config", () => Hide());
            
            // Content Container
            GameObject content = new GameObject("Content");
            content.transform.SetParent(parent, false);
            
            RectTransform contentRt = content.AddComponent<RectTransform>();
            contentRt.anchorMin = Vector2.zero;
            contentRt.anchorMax = Vector2.one;
            contentRt.offsetMin = new Vector2(8, 8);
            contentRt.offsetMax = new Vector2(-8, -25); 

            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.padding = new RectOffset(5, 5, 12, 5);
            layout.spacing = 6;
            layout.childAlignment = TextAnchor.UpperCenter;

            // ─────────────────────────────────────────────────────
            // Provider Selection
            // ─────────────────────────────────────────────────────
            CreateLabel(content.transform, "ProviderLabel", "Provider:");
            
            GameObject providerRow = CreateRow(content.transform, "ProviderRow", 22);
            _providerCombo = ComboBoxUI.Create(providerRow.transform, "Provider");
            _providerCombo.OnSelectionChanged += OnProviderChanged;
            _providerCombo.SetAllowCustomInput(false);
            
            // ─────────────────────────────────────────────────────
            // Custom URL (shown for Custom provider)
            // ─────────────────────────────────────────────────────
            _customUrlRow = CreateRow(content.transform, "CustomUrlRow", 24);
            CreateLabel(_customUrlRow.transform, "UrlLabel", "URL:");
            _customUrlInput = CreateInputField(_customUrlRow.transform, "CustomUrl", "https://...", 240);
            _customUrlInput.onEndEdit.AddListener(OnCustomUrlChanged);
            _customUrlRow.SetActive(false);

            // ─────────────────────────────────────────────────────
            // API Key (hidden for local providers)
            // ─────────────────────────────────────────────────────
            _apiKeyRow = CreateRow(content.transform, "ApiKeyRow", 20);
            
            // Fix alignment to put button nicely on the right
            HorizontalLayoutGroup keyLayout = _apiKeyRow.GetComponent<HorizontalLayoutGroup>();
            if (keyLayout == null) keyLayout = _apiKeyRow.AddComponent<HorizontalLayoutGroup>();
            
            keyLayout.childAlignment = TextAnchor.MiddleLeft;
            keyLayout.childControlWidth = true; 
            keyLayout.childForceExpandWidth = false;
            keyLayout.spacing = 8;
            keyLayout.padding = new RectOffset(5, 5, 0, 0);

            // Input Field
            _apiKeyInput = CreateInputField(_apiKeyRow.transform, "ApiKey", "sk-...", 0); 
            _apiKeyInput.contentType = InputField.ContentType.Password;
            _apiKeyInput.onEndEdit.AddListener(OnApiKeyChanged);
            
            // Force width using LayoutElement
            LayoutElement le = _apiKeyInput.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = _apiKeyInput.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = 210;
            le.minWidth = 100;
            
            Button showKeyBtn = CreateButton(_apiKeyRow.transform, "ShowKeyBtn", "👁", new Vector2(85, 24));
            showKeyBtn.onClick.AddListener(ToggleShowKey);

            // ─────────────────────────────────────────────────────
            // Model Selection
            // ─────────────────────────────────────────────────────
            CreateLabel(content.transform, "ModelLabel", "Model:");

            GameObject modelRow = CreateRow(content.transform, "ModelRow", 22);
            _modelCombo = ComboBoxUI.Create(modelRow.transform, "Model");
            _modelCombo.OnSelectionChanged += OnModelChanged;
            _modelCombo.OnCustomValueEntered += OnCustomModelEntered;

            // Model Info Display
            GameObject modelInfoRow = CreateRow(content.transform, "ModelInfoRow", 20);
            _modelInfoText = CreateText(modelInfoRow.transform, "ModelInfo", "", 6);
            _modelInfoText.color = new Color(0.7f, 0.9f, 0.7f);
            _modelInfoText.alignment = TextAnchor.MiddleLeft;

            // ─────────────────────────────────────────────────────
            // Action Buttons
            // ─────────────────────────────────────────────────────
            GameObject buttonRow = CreateRow(content.transform, "ButtonRow", 24);
            
            HorizontalLayoutGroup btnLayout = buttonRow.AddComponent<HorizontalLayoutGroup>();
            btnLayout.childControlWidth = false;
            btnLayout.childForceExpandWidth = false;
            btnLayout.spacing = 8;
            btnLayout.childAlignment = TextAnchor.MiddleCenter;

            _refreshButton = CreateButton(buttonRow.transform, "RefreshBtn", "🔄 Refresh", new Vector2(100, 24));
            _refreshButton.onClick.AddListener(OnRefreshModels);

            _testButton = CreateButton(buttonRow.transform, "TestBtn", "✓ Test", new Vector2(100, 24));
            _testButton.onClick.AddListener(OnTestConnection);

            // ─────────────────────────────────────────────────────
            // Status
            // ─────────────────────────────────────────────────────
            GameObject statusRow = CreateRow(content.transform, "StatusRow", 20);
            _statusText = CreateText(statusRow.transform, "Status", "", 7);
            _statusText.alignment = TextAnchor.MiddleCenter;
            _statusText.color = new Color(0.8f, 0.8f, 0.8f);

            // Populate initial data
            PopulateProviders();
            LoadCurrentSettings();
        }

        // ═══════════════════════════════════════════════════════════════
        // Data Population
        // ═══════════════════════════════════════════════════════════════

        private void PopulateProviders()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();

            var cloudProviders = ProviderConfig.GetCloudProviders();
            var localProviders = ProviderConfig.GetLocalProviders();
            Debug.Log($"[AIBox] Populating Providers. Found {cloudProviders.Count} cloud providers.");

            // 1. Custom Providers (First Choice)
            var custom = ProviderConfig.GetProviderById("custom");
            if (custom != null)
            {
                items.Add(new ComboBoxItem(custom.Id, custom.Name, "Manual", custom));
                items.Add(ComboBoxItem.Separator());
            }

            // 2. Cloud Providers
            items.Add(new ComboBoxItem("_cloud_header_", "── Cloud Providers ──") { IsSeparator = true });
            foreach (var provider in cloudProviders)
            {
                string sub = provider.RequiresApiKey ? "Key Required" : "";
                items.Add(new ComboBoxItem(provider.Id, provider.Name, sub, provider));
            }

            // 3. Local Providers
            items.Add(ComboBoxItem.Separator());
            items.Add(new ComboBoxItem("_local_header_", "── Local Providers ──") { IsSeparator = true });
            foreach (var provider in localProviders)
            {
                items.Add(new ComboBoxItem(provider.Id, provider.Name, "Free", provider));
            }

            _providerCombo.SetItems(items);
        }

        private void PopulateModels(ProviderInfo provider)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();

            if (provider == null)
            {
                _modelCombo.SetItems(items);
                return;
            }

            // For local providers, only show what we've actually detected
            if (provider.IsLocal)
            {
                if (ProviderSettings.Data.cachedLocalModels != null)
                {
                    foreach (string modelId in ProviderSettings.Data.cachedLocalModels)
                    {
                        items.Add(new ComboBoxItem(modelId, modelId, "★★★☆☆ | Free"));
                    }
                }
            }
            else if (provider.Models != null)
            {
                // For cloud providers, show the curated list
                foreach (var model in provider.Models)
                {
                    string sub = $"{model.GetQualityStars()} | {model.CostTier}";
                    items.Add(new ComboBoxItem(model.Id, model.DisplayName, sub, model));
                }
            }

            _modelCombo.SetItems(items);
        }

        private void LoadCurrentSettings()
        {
            var settings = ProviderSettings.Data;
            
            // Set provider
            _providerCombo.SetSelectedById(settings.selectedProviderId);
            _currentProvider = ProviderSettings.GetCurrentProvider();
            
            // Update UI based on provider
            UpdateUIForProvider(_currentProvider);
            
            // Set model
            _modelCombo.SetSelectedById(settings.modelId);
            
            // Allow custom model input?
            bool allowCustom = _currentProvider != null && (_currentProvider.IsLocal || _currentProvider.Id == "custom" || _currentProvider.Id == "openai");
            _modelCombo.SetAllowCustomInput(allowCustom);

            // Fill inputs with current settings
            _apiKeyInput.text = settings.apiKey;
            
            // If currently on custom, show the URL
            if (_currentProvider != null && _currentProvider.Id == "custom")
            {
                _customUrlInput.text = settings.apiUrl;
            }
            
            UpdateModelInfo();
        }

        private void UpdateUIForProvider(ProviderInfo provider)
        {
            if (provider == null) return;

            // Update URL and Key visibility
            _customUrlRow.SetActive(provider.Id == "custom");
            
            // Custom provider always gets a Key row option
            bool needsKey = provider.RequiresApiKey || provider.Id == "custom"; 
            _apiKeyRow.SetActive(needsKey);
            
            // Sync inputs with global settings
            _apiKeyInput.text = ProviderSettings.Data.apiKey;
            if (provider.Id == "custom")
            {
                _customUrlInput.text = ProviderSettings.Data.apiUrl;
            }

            // Update refresh button state
            _refreshButton.interactable = provider.IsLocal;

            // Populate models for this provider
            PopulateModels(provider);
        }

        // ═══════════════════════════════════════════════════════════════
        // Event Handlers
        // ═══════════════════════════════════════════════════════════════

        private void OnProviderChanged(ComboBoxItem item)
        {
            var provider = item.Data as ProviderInfo;
            if (provider != null)
            {
                _currentProvider = provider;
                ProviderSettings.SetProvider(provider.Id);
                UpdateUIForProvider(provider);
                
                if (provider.IsLocal)
                {
                    OnRefreshModels();
                }
                else if (provider.Models.Count > 0)
                {
                    _modelCombo.SetSelectedById(provider.Models[0].Id);
                }
            }
        }

        private void OnModelChanged(ComboBoxItem item)
        {
            if (item == null) return;
            string modelId = item.Id;
            ProviderSettings.SetModel(modelId);
            UpdateModelInfo();
        }

        private void OnCustomModelEntered(string modelId)
        {
            ProviderSettings.SetModel(modelId);
            UpdateModelInfo();
        }

        private void UpdateModelInfo()
        {
            string id = ProviderSettings.Data.modelId;
            var provider = ProviderSettings.GetCurrentProvider();
            var info = provider?.Models?.Find(m => m.Id == id);
            
            if (info != null)
            {
                _modelInfoText.text = $"{info.Description} (Context: {info.ContextWindow / 1024}k)";
            }
            else
            {
                _modelInfoText.text = "Custom Model";
            }
        }

        private void OnApiKeyChanged(string key)
        {
            ProviderSettings.SetApiKey(key);
            SetStatus("API key updated");
        }

        private void OnCustomUrlChanged(string url)
        {
            ProviderSettings.SetCustomUrl(url);
            SetStatus("Custom URL updated");
        }

        private void ToggleShowKey()
        {
            if (_apiKeyInput.contentType == InputField.ContentType.Password)
                _apiKeyInput.contentType = InputField.ContentType.Standard;
            else
                _apiKeyInput.contentType = InputField.ContentType.Password;
            
            _apiKeyInput.ForceLabelUpdate();
        }

        private void OnRefreshModels()
        {
            if (_currentProvider == null || !_currentProvider.IsLocal)
            {
                SetStatus("Refresh only works for local providers");
                return;
            }

            SetStatus("🔄 Fetching models...");
            _refreshButton.interactable = false;

            string baseUrl = ProviderSettings.Data.apiUrl;
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = _currentProvider.DefaultUrl;
            }

            StartCoroutine(FetchModelsCoroutine(baseUrl));
        }

        private IEnumerator FetchModelsCoroutine(string baseUrl)
        {
            List<string> fetchedModels = null;

            yield return LLMClient.FetchModels(baseUrl, (models) => {
                fetchedModels = models;
            });

            _refreshButton.interactable = true;

            if (fetchedModels != null && fetchedModels.Count > 0)
            {
                ProviderSettings.UpdateCachedModels(fetchedModels);
                PopulateModels(_currentProvider);
                
                // Select first fetched model
                _modelCombo.SetSelectedByIndex(0);
                
                SetStatus($"✓ Found {fetchedModels.Count} models", true);
            }
            else
            {
                SetStatus("No models found. Is the server running?", false);
            }
        }

        private void OnTestConnection()
        {
            if (_isTestingConnection) return;
            StartCoroutine(TestConnectionCoroutine());
        }

        private IEnumerator TestConnectionCoroutine()
        {
            _isTestingConnection = true;
            _testButton.interactable = false;
            SetStatus("Testing connection...");

            bool success = false;
            string message = "";

            yield return LLMClient.TestConnection(ProviderSettings.Data.apiUrl, ProviderSettings.Data.apiKey, (ok, msg) => {
                success = ok;
                message = msg;
            });

            _isTestingConnection = false;
            _testButton.interactable = true;

            SetStatus(success ? "✓ Connection Successful!" : $"❌ Failed: {message}", success);
        }

        private void SetStatus(string msg, bool isSuccess = true)
        {
            if (_statusText != null)
            {
                _statusText.text = msg;
                _statusText.color = isSuccess ? new Color(0.6f, 1f, 0.6f) : new Color(1f, 0.6f, 0.6f);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // UI Helpers
        // ═══════════════════════════════════════════════════════════════

        private GameObject CreateRow(Transform parent, string name, float height)
        {
            GameObject row = new GameObject(name, typeof(RectTransform), typeof(LayoutElement));
            row.transform.SetParent(parent, false);

            RectTransform rt = row.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(0, height);

            LayoutElement le = row.GetComponent<LayoutElement>();
            le.minHeight = height;
            le.preferredHeight = height;
            le.flexibleWidth = 1;

            return row;
        }

        private Text CreateLabel(Transform parent, string name, string text)
        {
            GameObject row = CreateRow(parent, name, 13);
            return CreateText(row.transform, name + "Text", text, 7);
        }

        private Text CreateText(Transform parent, string name, string content, int fontSize)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);

            Text txt = go.GetComponent<Text>();
            txt.font = GetSafeFont();
            txt.fontSize = fontSize;
            txt.text = content;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleLeft;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            return txt;
        }

        private InputField CreateInputField(Transform parent, string name, string placeholder, float width)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(InputField));
            go.transform.SetParent(parent, false);

            Image bg = go.GetComponent<Image>();
            bg.sprite = SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty");
            bg.type = Image.Type.Sliced;
            bg.color = new Color(0.2f, 0.2f, 0.22f);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(0, 26);
            rt.anchoredPosition = Vector2.zero;

            // Text component
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(go.transform, false);
            
            Text text = textGo.GetComponent<Text>();
            text.font = GetSafeFont();
            text.fontSize = 9;
            text.color = Color.white;
            
            RectTransform textRt = textGo.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = new Vector2(5, 2);
            textRt.offsetMax = new Vector2(-5, -2);

            // Placeholder
            GameObject phGo = new GameObject("Placeholder", typeof(RectTransform), typeof(Text));
            phGo.transform.SetParent(go.transform, false);
            
            Text ph = phGo.GetComponent<Text>();
            ph.font = GetSafeFont();
            ph.fontSize = 9;
            ph.fontStyle = FontStyle.Italic;
            ph.color = new Color(0.5f, 0.5f, 0.5f);
            ph.text = placeholder;

            RectTransform phRt = phGo.GetComponent<RectTransform>();
            phRt.anchorMin = Vector2.zero;
            phRt.anchorMax = Vector2.one;
            phRt.offsetMin = new Vector2(5, 2);
            phRt.offsetMax = new Vector2(-5, -2);

            InputField input = go.GetComponent<InputField>();
            input.textComponent = text;
            input.placeholder = ph;
            input.targetGraphic = bg;

            return input;
        }

        private Button CreateButton(Transform parent, string name, string text, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            Image bg = go.GetComponent<Image>();
            bg.sprite = SpriteTextureLoader.getSprite("ui/special/button2");
            bg.type = Image.Type.Sliced;
            bg.color = new Color(0.25f, 0.25f, 0.25f);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = size;

            // Label
            GameObject label = new GameObject("Label", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(go.transform, false);
            
            Text txt = label.GetComponent<Text>();
            txt.font = GetSafeFont();
            txt.fontSize = 10;
            txt.text = text;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleCenter;
            
            RectTransform labelRt = label.GetComponent<RectTransform>();
            labelRt.anchorMin = Vector2.zero;
            labelRt.anchorMax = Vector2.one;
            labelRt.offsetMin = Vector2.zero;
            labelRt.offsetMax = Vector2.zero;

            return go.GetComponent<Button>();
        }

        private Font GetSafeFont()
        {
            if (LocalizedTextManager.current_font != null) return LocalizedTextManager.current_font;
            
            // Try to find ANY font in the game if Arial is missing
            Font fallback = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (fallback != null) return fallback;

            // Last resort: find any loaded font
            Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
            if (allFonts != null && allFonts.Length > 0) return allFonts[0];

            return null; // Should not happen in WorldBox
        }
    }
}
