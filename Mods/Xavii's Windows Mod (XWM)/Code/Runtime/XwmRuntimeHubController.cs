using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XaviiWindowsMod.API;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmRuntimeHubController : MonoBehaviour
    {
        private const string WindowId = "xwm_runtime_hub";
        private const float RefreshInterval = 0.8f;

        private WindowInstance _window;
        private RectTransform _contentRoot;
        private RectTransform _listRoot;
        private Text _statusText;
        private Text _modButtonLabel;
        private Text _onlyLoadedButtonLabel;
        private InputField _searchInput;
        private bool _onlyLoaded;
        private string _selectedMod;
        private string _search;
        private float _nextRefreshAt;
        private bool _pendingRefresh;

        private XwmWorkspaceStateRunner _workspaceState;

        internal bool IsOpen => _window != null && _window.Root != null && _window.Root.activeSelf;

        private void Awake()
        {
            _workspaceState = GetComponent<XwmWorkspaceStateRunner>();
        }

        private void Update()
        {
            if (!IsOpen)
            {
                return;
            }

            if (Time.unscaledTime < _nextRefreshAt && !_pendingRefresh)
            {
                return;
            }

            RefreshList();
            _pendingRefresh = false;
            _nextRefreshAt = Time.unscaledTime + RefreshInterval;
        }

        public void Toggle()
        {
            if (!EnsureWindow())
            {
                return;
            }

            if (IsOpen)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            if (!EnsureWindow())
            {
                return;
            }

            _window.Show();
            _window.BringToFront();
            MarkRefresh();
            SetStatus("Runtime Hub open", new Color(0.72f, 0.93f, 1f, 1f));
        }

        public void Hide()
        {
            if (_window == null)
            {
                return;
            }

            _window.Hide();
        }

        public void MarkRefresh()
        {
            _pendingRefresh = true;
            _nextRefreshAt = 0f;
        }

        private bool EnsureWindow()
        {
            if (_window != null && _window.Root != null)
            {
                return true;
            }

            _window = WindowSystem.GetOrCreate(WindowId, "XWM Runtime Hub", new Vector2(1180f, 760f), true);
            if (_window == null || _window.Root == null)
            {
                return false;
            }

            _window.SetOpacity(0.97f);
            _window.Position = new Vector2(220f, 120f);
            _window.SetSize(new Vector2(1180f, 760f));
            _window.SetTitle("XWM Runtime Hub");

            if (_window.CloseButton != null)
            {
                _window.CloseButton.onClick.RemoveAllListeners();
                _window.CloseButton.onClick.AddListener(Hide);
            }

            BuildUi();
            return true;
        }

        private void BuildUi()
        {
            if (_window == null || _window.Content == null)
            {
                return;
            }

            if (_window.ScrollRect != null)
            {
                _window.ScrollRect.horizontal = false;
                _window.ScrollRect.vertical = true;
                _window.ScrollRect.scrollSensitivity = 24f;
            }

            _contentRoot = _window.Content;
            _contentRoot.anchorMin = new Vector2(0f, 1f);
            _contentRoot.anchorMax = new Vector2(1f, 1f);
            _contentRoot.pivot = new Vector2(0f, 1f);
            _contentRoot.anchoredPosition = Vector2.zero;
            _contentRoot.sizeDelta = new Vector2(0f, 0f);

            for (int i = _contentRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_contentRoot.GetChild(i).gameObject);
            }

            VerticalLayoutGroup rootLayout = _contentRoot.GetComponent<VerticalLayoutGroup>();
            if (rootLayout == null)
            {
                rootLayout = _contentRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            }

            rootLayout.spacing = 8f;
            rootLayout.padding = new RectOffset(8, 8, 8, 8);
            rootLayout.childAlignment = TextAnchor.UpperLeft;
            rootLayout.childControlWidth = true;
            rootLayout.childControlHeight = false;
            rootLayout.childForceExpandWidth = true;
            rootLayout.childForceExpandHeight = false;

            ContentSizeFitter rootFitter = _contentRoot.GetComponent<ContentSizeFitter>();
            if (rootFitter == null)
            {
                rootFitter = _contentRoot.gameObject.AddComponent<ContentSizeFitter>();
            }

            rootFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            rootFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            BuildControlRows();
            BuildListContainer();
            MarkRefresh();
        }

        private void BuildControlRows()
        {
            RectTransform rowA = CreateRow(_contentRoot, 40f, new Color(0.11f, 0.16f, 0.25f, 0.92f));
            _modButtonLabel = CreateButton(rowA, "Mod: (none)", 210f, CycleSelectedMod).GetComponentInChildren<Text>();
            CreateButton(rowA, "Refresh", 90f, () =>
            {
                MarkRefresh();
                SetStatus("Refreshed", new Color(0.75f, 0.9f, 1f, 1f));
            });
            CreateButton(rowA, "Prewarm Mod", 120f, PrewarmSelectedMod);
            CreateButton(rowA, "Show Mod", 95f, ShowSelectedMod);
            CreateButton(rowA, "Hide Mod", 95f, HideSelectedMod);
            CreateButton(rowA, "Unload Mod", 100f, UnloadSelectedMod);
            CreateButton(rowA, "Reload Loaded", 120f, ReloadLoaded);
            CreateButton(rowA, "Save Layout", 110f, SaveWorkspace);

            RectTransform rowB = CreateRow(_contentRoot, 40f, new Color(0.1f, 0.14f, 0.22f, 0.92f));
            CreateText(rowB, "Search", 14, TextAnchor.MiddleLeft, new Color(0.87f, 0.93f, 1f, 1f), 80f, false);
            _searchInput = CreateInput(rowB, "Search files", value =>
            {
                _search = value ?? string.Empty;
                MarkRefresh();
            });
            LayoutElement searchLayout = _searchInput.GetComponent<LayoutElement>();
            if (searchLayout != null)
            {
                searchLayout.flexibleWidth = 1f;
            }

            _onlyLoadedButtonLabel = CreateButton(rowB, "Only Loaded: Off", 150f, ToggleOnlyLoaded).GetComponentInChildren<Text>();
            CreateButton(rowB, "Autoload: On/Off", 140f, ToggleAutoloadMaster);

            RectTransform rowC = CreateRow(_contentRoot, 30f, new Color(0.09f, 0.12f, 0.19f, 0.92f));
            _statusText = CreateText(rowC, "Ready", 13, TextAnchor.MiddleLeft, new Color(0.72f, 0.93f, 1f, 1f), 0f, false);
            LayoutElement statusLayout = _statusText.GetComponent<LayoutElement>();
            if (statusLayout != null)
            {
                statusLayout.flexibleWidth = 1f;
            }

            CreateHeaderRow();
        }

        private void BuildListContainer()
        {
            GameObject listObject = new GameObject("List", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(LayoutElement));
            RectTransform listRect = listObject.GetComponent<RectTransform>();
            listRect.SetParent(_contentRoot, false);

            Image listImage = listObject.GetComponent<Image>();
            listImage.sprite = XwmUiBootstrap.ResolveSprite("ui/icons/windowInnerSliced", "ui/icons/windowInnerSliced");
            listImage.type = Image.Type.Sliced;
            listImage.color = new Color(0.09f, 0.13f, 0.2f, 0.94f);

            VerticalLayoutGroup layout = listObject.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(6, 6, 6, 6);
            layout.spacing = 5f;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter = listObject.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            LayoutElement element = listObject.GetComponent<LayoutElement>();
            element.minHeight = 120f;
            element.flexibleHeight = 1f;

            _listRoot = listRect;
        }

        private void CreateHeaderRow()
        {
            RectTransform header = CreateRow(_contentRoot, 28f, new Color(0.08f, 0.11f, 0.17f, 0.95f));
            CreateText(header, "File", 13, TextAnchor.MiddleLeft, new Color(0.9f, 0.95f, 1f, 1f), 0f, false).GetComponent<LayoutElement>().flexibleWidth = 1f;
            CreateText(header, "State", 13, TextAnchor.MiddleCenter, new Color(0.9f, 0.95f, 1f, 1f), 110f, false);
            CreateText(header, "Actions", 13, TextAnchor.MiddleCenter, new Color(0.9f, 0.95f, 1f, 1f), 545f, false);
        }

        private void RefreshList()
        {
            if (_listRoot == null)
            {
                return;
            }

            IReadOnlyList<string> mods = XwmFiles.ListModTargets(true);
            if (mods.Count == 0)
            {
                _selectedMod = null;
            }
            else if (string.IsNullOrWhiteSpace(_selectedMod) || !ContainsIgnoreCase(mods, _selectedMod))
            {
                _selectedMod = mods[0];
            }

            if (_modButtonLabel != null)
            {
                _modButtonLabel.text = string.IsNullOrWhiteSpace(_selectedMod) ? "Mod: (none)" : "Mod: " + _selectedMod;
            }

            if (_onlyLoadedButtonLabel != null)
            {
                _onlyLoadedButtonLabel.text = _onlyLoaded ? "Only Loaded: On" : "Only Loaded: Off";
            }

            for (int i = _listRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_listRoot.GetChild(i).gameObject);
            }

            IReadOnlyList<XwmFileDescriptor> all = XwmFiles.ListAllFiles(false);
            int visibleRows = 0;

            for (int i = 0; i < all.Count; i++)
            {
                XwmFileDescriptor descriptor = all[i];
                if (descriptor == null)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(_selectedMod) && !string.Equals(descriptor.ModTarget, _selectedMod, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!PassesSearch(descriptor))
                {
                    continue;
                }

                XwmWindowHandle runtime = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                if (_onlyLoaded && runtime == null)
                {
                    continue;
                }

                CreateEntryRow(descriptor, runtime, visibleRows);
                visibleRows++;
            }

            if (visibleRows == 0)
            {
                RectTransform empty = CreateRow(_listRoot, 34f, new Color(0.07f, 0.1f, 0.15f, 0.8f));
                Text text = CreateText(empty, "No files match current filters", 13, TextAnchor.MiddleLeft, new Color(0.8f, 0.86f, 0.95f, 1f), 0f, false);
                LayoutElement layout = text.GetComponent<LayoutElement>();
                if (layout != null)
                {
                    layout.flexibleWidth = 1f;
                }
            }
        }

        private void CreateEntryRow(XwmFileDescriptor descriptor, XwmWindowHandle runtime, int index)
        {
            bool loaded = runtime != null && !runtime.IsDestroyed;
            bool visible = loaded && runtime.IsVisible;
            bool autoloadEnabled = XwmAutoloadProfileStore.Contains(descriptor.ModTarget, descriptor.FileName);
            string runtimeId = loaded ? runtime.RuntimeId : descriptor.RuntimeId;

            Color baseColor = index % 2 == 0
                ? new Color(0.11f, 0.16f, 0.24f, 0.82f)
                : new Color(0.1f, 0.14f, 0.21f, 0.82f);

            RectTransform row = CreateRow(_listRoot, 34f, baseColor);
            Text fileLabel = CreateText(row, descriptor.FileName, 13, TextAnchor.MiddleLeft, new Color(0.93f, 0.97f, 1f, 1f), 0f, false);
            LayoutElement fileLayout = fileLabel.GetComponent<LayoutElement>();
            if (fileLayout != null)
            {
                fileLayout.flexibleWidth = 1f;
            }

            string state = loaded ? (visible ? "Visible" : "Hidden") : "Not Loaded";
            Color stateColor = loaded
                ? (visible ? new Color(0.64f, 0.98f, 0.74f, 1f) : new Color(0.96f, 0.88f, 0.52f, 1f))
                : new Color(0.86f, 0.8f, 0.8f, 1f);
            CreateText(row, state, 12, TextAnchor.MiddleCenter, stateColor, 110f, false);

            CreateButton(row, "Load", 62f, () =>
            {
                XwmWindowHandle current = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                string targetRuntimeId = current != null ? current.RuntimeId : runtimeId;
                XwmWindowHandle handle = XwmFiles.GetOrLoad(descriptor.ModTarget, descriptor.FileName, targetRuntimeId, false);
                SetStatus(handle != null ? "Loaded " + descriptor.FileName : "Failed to load " + descriptor.FileName, handle != null ? new Color(0.68f, 0.96f, 0.78f, 1f) : new Color(1f, 0.67f, 0.67f, 1f));
                MarkRefresh();
            });

            CreateButton(row, "Show", 62f, () =>
            {
                XwmWindowHandle current = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                string targetRuntimeId = current != null ? current.RuntimeId : runtimeId;
                XwmWindowHandle handle = XwmFiles.GetOrLoad(descriptor.ModTarget, descriptor.FileName, targetRuntimeId, true);
                SetStatus(handle != null ? "Shown " + descriptor.FileName : "Failed to show " + descriptor.FileName, handle != null ? new Color(0.68f, 0.96f, 0.78f, 1f) : new Color(1f, 0.67f, 0.67f, 1f));
                MarkRefresh();
            });

            CreateButton(row, "Hide", 62f, () =>
            {
                XwmWindowHandle current = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                string targetRuntimeId = current != null ? current.RuntimeId : runtimeId;
                bool ok = XwmFiles.Hide(targetRuntimeId);
                SetStatus(ok ? "Hidden " + descriptor.FileName : descriptor.FileName + " was not loaded", ok ? new Color(0.96f, 0.9f, 0.54f, 1f) : new Color(0.92f, 0.85f, 0.85f, 1f));
                MarkRefresh();
            });

            CreateButton(row, "Reload", 68f, () =>
            {
                XwmWindowHandle current = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                string targetRuntimeId = current != null ? current.RuntimeId : runtimeId;
                bool show = current != null && current.IsVisible;
                XwmWindowHandle handle = XwmFiles.Reload(descriptor.ModTarget, descriptor.FileName, targetRuntimeId, show);
                SetStatus(handle != null ? "Reloaded " + descriptor.FileName : "Failed to reload " + descriptor.FileName, handle != null ? new Color(0.68f, 0.96f, 0.78f, 1f) : new Color(1f, 0.67f, 0.67f, 1f));
                MarkRefresh();
            });

            CreateButton(row, "Destroy", 72f, () =>
            {
                XwmWindowHandle current = XwmFiles.FindRuntime(descriptor.ModTarget, descriptor.FileName);
                string targetRuntimeId = current != null ? current.RuntimeId : runtimeId;
                bool ok = XwmFiles.Destroy(targetRuntimeId);
                SetStatus(ok ? "Destroyed " + descriptor.FileName : descriptor.FileName + " was not loaded", ok ? new Color(0.96f, 0.81f, 0.57f, 1f) : new Color(0.92f, 0.85f, 0.85f, 1f));
                MarkRefresh();
            });

            CreateButton(row, autoloadEnabled ? "Auto On" : "Auto Off", 78f, () =>
            {
                bool enabled = XwmAutoloadProfileStore.Toggle(descriptor.ModTarget, descriptor.FileName);
                SetStatus((enabled ? "Enabled" : "Disabled") + " autoload for " + descriptor.FileName, enabled ? new Color(0.7f, 0.95f, 0.8f, 1f) : new Color(0.95f, 0.8f, 0.7f, 1f));
                MarkRefresh();
            });
        }

        private void CycleSelectedMod()
        {
            IReadOnlyList<string> mods = XwmFiles.ListModTargets(true);
            if (mods.Count == 0)
            {
                _selectedMod = null;
                MarkRefresh();
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedMod))
            {
                _selectedMod = mods[0];
                MarkRefresh();
                return;
            }

            int index = IndexOfIgnoreCase(mods, _selectedMod);
            if (index < 0 || index == mods.Count - 1)
            {
                _selectedMod = mods[0];
            }
            else
            {
                _selectedMod = mods[index + 1];
            }

            MarkRefresh();
        }

        private void ToggleOnlyLoaded()
        {
            _onlyLoaded = !_onlyLoaded;
            MarkRefresh();
        }

        private void ToggleAutoloadMaster()
        {
            XwmAutoloadProfileStore.Enabled = !XwmAutoloadProfileStore.Enabled;
            SetStatus(XwmAutoloadProfileStore.Enabled ? "Autoload profile enabled" : "Autoload profile disabled", XwmAutoloadProfileStore.Enabled ? new Color(0.7f, 0.95f, 0.8f, 1f) : new Color(0.95f, 0.8f, 0.7f, 1f));
            MarkRefresh();
        }

        private void PrewarmSelectedMod()
        {
            if (string.IsNullOrWhiteSpace(_selectedMod))
            {
                SetStatus("No mod selected", new Color(0.96f, 0.78f, 0.78f, 1f));
                return;
            }

            int loaded = XwmFiles.PrewarmAll(_selectedMod, false, null);
            SetStatus("Prewarmed " + loaded + " file(s) from " + _selectedMod, new Color(0.7f, 0.95f, 0.8f, 1f));
            MarkRefresh();
        }

        private void ShowSelectedMod()
        {
            if (string.IsNullOrWhiteSpace(_selectedMod))
            {
                SetStatus("No mod selected", new Color(0.96f, 0.78f, 0.78f, 1f));
                return;
            }

            int shown = XwmFiles.ShowByMod(_selectedMod);
            SetStatus("Shown " + shown + " runtime(s)", new Color(0.7f, 0.95f, 0.8f, 1f));
            MarkRefresh();
        }

        private void HideSelectedMod()
        {
            if (string.IsNullOrWhiteSpace(_selectedMod))
            {
                SetStatus("No mod selected", new Color(0.96f, 0.78f, 0.78f, 1f));
                return;
            }

            int hidden = XwmFiles.HideByMod(_selectedMod);
            SetStatus("Hidden " + hidden + " runtime(s)", new Color(0.95f, 0.88f, 0.56f, 1f));
            MarkRefresh();
        }

        private void UnloadSelectedMod()
        {
            if (string.IsNullOrWhiteSpace(_selectedMod))
            {
                SetStatus("No mod selected", new Color(0.96f, 0.78f, 0.78f, 1f));
                return;
            }

            int destroyed = XwmFiles.DestroyByMod(_selectedMod);
            SetStatus("Destroyed " + destroyed + " runtime(s)", new Color(0.96f, 0.81f, 0.57f, 1f));
            MarkRefresh();
        }

        private void ReloadLoaded()
        {
            int reloaded = XwmFiles.ReloadAllLoaded(true);
            SetStatus("Reloaded " + reloaded + " runtime(s)", new Color(0.7f, 0.95f, 0.8f, 1f));
            MarkRefresh();
        }

        private void SaveWorkspace()
        {
            if (_workspaceState == null)
            {
                _workspaceState = GetComponent<XwmWorkspaceStateRunner>();
            }

            _workspaceState?.ForceSave();
            SetStatus("Workspace state saved", new Color(0.7f, 0.95f, 0.8f, 1f));
        }

        private bool PassesSearch(XwmFileDescriptor descriptor)
        {
            if (descriptor == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_search))
            {
                return true;
            }

            string query = _search.Trim();
            return ContainsIgnoreCase(descriptor.FileName, query)
                || ContainsIgnoreCase(descriptor.ModTarget, query)
                || ContainsIgnoreCase(descriptor.RuntimeId, query);
        }

        private void SetStatus(string text, Color color)
        {
            if (_statusText == null)
            {
                return;
            }

            _statusText.text = text ?? string.Empty;
            _statusText.color = color;
        }

        private static RectTransform CreateRow(Transform parent, float height, Color color)
        {
            GameObject rowObject = new GameObject("Row", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(parent, false);

            Image image = rowObject.GetComponent<Image>();
            image.sprite = XwmUiBootstrap.ResolveSprite("ui/icons/windowInnerSliced", "ui/icons/windowInnerSliced");
            image.type = Image.Type.Sliced;
            image.color = color;

            HorizontalLayoutGroup layout = rowObject.GetComponent<HorizontalLayoutGroup>();
            layout.spacing = 6f;
            layout.padding = new RectOffset(6, 6, 4, 4);
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            LayoutElement element = rowObject.GetComponent<LayoutElement>();
            element.minHeight = height;
            element.preferredHeight = height;
            element.flexibleWidth = 1f;

            return rowRect;
        }

        private static Text CreateText(Transform parent, string value, int fontSize, TextAnchor alignment, Color color, float width, bool raycast)
        {
            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text), typeof(LayoutElement));
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            Text text = textObject.GetComponent<Text>();
            text.font = XwmUiBootstrap.DefaultFont;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.text = value ?? string.Empty;
            text.raycastTarget = raycast;

            LayoutElement element = textObject.GetComponent<LayoutElement>();
            if (width > 0f)
            {
                element.minWidth = width;
                element.preferredWidth = width;
                element.flexibleWidth = 0f;
            }
            else
            {
                element.flexibleWidth = 1f;
            }

            return text;
        }

        private static Button CreateButton(Transform parent, string label, float width, UnityAction onClick)
        {
            GameObject buttonObject = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            Image image = buttonObject.GetComponent<Image>();
            image.sprite = XwmUiBootstrap.ResolveSprite("ui/icons/backgroundTabButton", "ui/icons/windowInnerSliced");
            image.type = Image.Type.Sliced;
            image.color = new Color(0.24f, 0.42f, 0.62f, 0.95f);

            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            if (onClick != null)
            {
                button.onClick.AddListener(onClick);
            }

            LayoutElement layout = buttonObject.GetComponent<LayoutElement>();
            layout.minWidth = width;
            layout.preferredWidth = width;
            layout.minHeight = 28f;
            layout.preferredHeight = 28f;

            GameObject textObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.SetParent(rect, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text text = textObject.GetComponent<Text>();
            text.font = XwmUiBootstrap.DefaultFont;
            text.fontSize = 12;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(0.95f, 0.98f, 1f, 1f);
            text.text = label ?? string.Empty;
            text.raycastTarget = false;

            return button;
        }

        private static InputField CreateInput(Transform parent, string placeholder, UnityAction<string> onChanged)
        {
            GameObject inputObject = new GameObject("Input", typeof(RectTransform), typeof(Image), typeof(InputField), typeof(LayoutElement));
            RectTransform rect = inputObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            Image image = inputObject.GetComponent<Image>();
            image.sprite = XwmUiBootstrap.ResolveSprite("ui/icons/windowInnerSliced", "ui/icons/windowInnerSliced");
            image.type = Image.Type.Sliced;
            image.color = new Color(0.12f, 0.18f, 0.27f, 0.95f);

            LayoutElement layout = inputObject.GetComponent<LayoutElement>();
            layout.minHeight = 28f;
            layout.preferredHeight = 28f;
            layout.minWidth = 180f;
            layout.preferredWidth = 220f;
            layout.flexibleWidth = 1f;

            InputField input = inputObject.GetComponent<InputField>();

            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.SetParent(rect, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10f, 4f);
            textRect.offsetMax = new Vector2(-10f, -4f);

            Text text = textObject.GetComponent<Text>();
            text.font = XwmUiBootstrap.DefaultFont;
            text.fontSize = 13;
            text.alignment = TextAnchor.MiddleLeft;
            text.color = new Color(0.93f, 0.97f, 1f, 1f);
            text.text = string.Empty;
            text.supportRichText = false;

            GameObject placeholderObject = new GameObject("Placeholder", typeof(RectTransform), typeof(Text));
            RectTransform placeholderRect = placeholderObject.GetComponent<RectTransform>();
            placeholderRect.SetParent(rect, false);
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(10f, 4f);
            placeholderRect.offsetMax = new Vector2(-10f, -4f);

            Text placeholderText = placeholderObject.GetComponent<Text>();
            placeholderText.font = XwmUiBootstrap.DefaultFont;
            placeholderText.fontSize = 13;
            placeholderText.alignment = TextAnchor.MiddleLeft;
            placeholderText.color = new Color(0.66f, 0.75f, 0.87f, 0.82f);
            placeholderText.text = placeholder ?? string.Empty;
            placeholderText.supportRichText = false;

            input.textComponent = text;
            input.placeholder = placeholderText;
            input.lineType = InputField.LineType.SingleLine;
            input.onValueChanged.AddListener(onChanged);

            return input;
        }

        private static bool ContainsIgnoreCase(IReadOnlyList<string> values, string value)
        {
            if (values == null || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                string candidate = values[i];
                if (string.Equals(candidate, value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static int IndexOfIgnoreCase(IReadOnlyList<string> values, string value)
        {
            if (values == null || string.IsNullOrWhiteSpace(value))
            {
                return -1;
            }

            for (int i = 0; i < values.Count; i++)
            {
                string candidate = values[i];
                if (string.Equals(candidate, value, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool ContainsIgnoreCase(string value, string query)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(query))
            {
                return false;
            }

            return value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
