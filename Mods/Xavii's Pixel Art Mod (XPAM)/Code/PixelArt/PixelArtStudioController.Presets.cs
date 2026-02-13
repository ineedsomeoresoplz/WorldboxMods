using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        private sealed class PresetEntry
        {
            public string Id;
            public string DisplayName;
            public string Detail;
            public string Source;
            public int Width;
            public int Height;
            public Sprite Sprite;
        }

        private sealed class PresetRowView
        {
            public GameObject RootObject;
            public RectTransform Rect;
            public Button Button;
            public Image Preview;
            public Text NameText;
            public Text DetailText;
            public int BoundIndex = -1;
        }

        private const float PresetRowHeight = 34f;
        private const int MaxPresetCanvasSide = 128;

        private RectTransform _gridHostRect;
        private RectTransform _presetPanelRect;
        private RectTransform _presetViewportRect;
        private RectTransform _presetContentRect;
        private ScrollRect _presetScrollRect;
        private InputField _presetSearchInput;
        private Toggle _presetLoadOnClickToggle;
        private Toggle _presetCloneOnClickToggle;
        private Text _presetCountText;

        private readonly List<PresetEntry> _allPresets = new List<PresetEntry>();
        private readonly List<PresetEntry> _filteredPresets = new List<PresetEntry>();
        private readonly List<PresetRowView> _presetRows = new List<PresetRowView>();

        private bool _presetsLoaded;
        private bool _presetsLoading;
        private Vector2 _lastPresetViewportSize;

        private void BuildPresetPanel(RectTransform parent)
        {
            if (parent == null)
            {
                return;
            }

            CreateLabel(parent, "PresetTitle", "Vanilla Presets", 15, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(10f, -34f), new Vector2(-120f, -8f), new Color(0.87f, 0.95f, 1f, 1f));
            CreateButton(parent, "PresetRefresh", "Refresh", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-92f, -34f), new Vector2(-10f, -8f), new Color(0.28f, 0.46f, 0.68f, 1f), RefreshVanillaPresets);
            _presetCountText = CreateLabel(parent, "PresetCount", "Presets: 0", 12, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(10f, -58f), new Vector2(-10f, -36f), new Color(0.72f, 0.83f, 0.96f, 1f));

            _presetSearchInput = CreateInputField(parent, "PresetSearch", "Search presets", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(10f, -92f), new Vector2(-10f, -62f));
            _presetSearchInput.text = string.Empty;
            _presetSearchInput.onValueChanged.AddListener(OnPresetSearchChanged);

            _presetLoadOnClickToggle = CreateToggle(parent, "PresetLoadToggle", "Load on click", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(10f, -124f), new Vector2(166f, -98f), true, value => SetStatus(value ? "Preset load enabled" : "Preset load disabled", new Color(0.74f, 0.91f, 1f, 1f)));
            _presetCloneOnClickToggle = CreateToggle(parent, "PresetCloneToggle", "Clone on click", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(176f, -124f), new Vector2(332f, -98f), false, value => SetStatus(value ? "Preset clone enabled" : "Preset clone disabled", new Color(0.74f, 0.91f, 1f, 1f)));

            _presetScrollRect = CreatePresetScroll(parent, "PresetScroll", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(10f, 40f), new Vector2(-10f, -132f), out _presetViewportRect, out _presetContentRect);
            _presetScrollRect.onValueChanged.AddListener(_ => RefreshPresetRows());

            string cloneDirectory = PixelArtPathResolver.ResolvePresetCloneDirectory(false);
            string relative = cloneDirectory.StartsWith(PixelArtPathResolver.ThisModFolder, StringComparison.OrdinalIgnoreCase)
                ? cloneDirectory.Substring(PixelArtPathResolver.ThisModFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                : cloneDirectory;
            CreateLabel(parent, "PresetClonePath", "Clone Dir: " + relative, 10, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(10f, 12f), new Vector2(-10f, 34f), new Color(0.62f, 0.75f, 0.9f, 1f));

            UpdatePresetCountText();
        }

        private Toggle CreateToggle(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, bool defaultValue, Action<bool> onChanged)
        {
            GameObject toggleObject = new GameObject(name, typeof(RectTransform), typeof(Toggle));
            RectTransform toggleRect = toggleObject.GetComponent<RectTransform>();
            toggleRect.SetParent(parent, false);
            toggleRect.anchorMin = anchorMin;
            toggleRect.anchorMax = anchorMax;
            toggleRect.offsetMin = offsetMin;
            toggleRect.offsetMax = offsetMax;

            GameObject backgroundObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.SetParent(toggleRect, false);
            backgroundRect.anchorMin = new Vector2(0f, 0.5f);
            backgroundRect.anchorMax = new Vector2(0f, 0.5f);
            backgroundRect.offsetMin = new Vector2(2f, -8f);
            backgroundRect.offsetMax = new Vector2(18f, 8f);
            Image backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.color = new Color(0.2f, 0.28f, 0.37f, 1f);

            GameObject checkmarkObject = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
            RectTransform checkmarkRect = checkmarkObject.GetComponent<RectTransform>();
            checkmarkRect.SetParent(backgroundRect, false);
            checkmarkRect.anchorMin = Vector2.zero;
            checkmarkRect.anchorMax = Vector2.one;
            checkmarkRect.offsetMin = new Vector2(2f, 2f);
            checkmarkRect.offsetMax = new Vector2(-2f, -2f);
            Image checkmarkImage = checkmarkObject.GetComponent<Image>();
            checkmarkImage.color = new Color(0.2f, 0.72f, 0.43f, 1f);

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.SetParent(toggleRect, false);
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(24f, 0f);
            labelRect.offsetMax = new Vector2(-2f, 0f);
            Text labelText = labelObject.GetComponent<Text>();
            labelText.font = PixelArtUiBootstrap.DefaultFont;
            labelText.fontSize = 11;
            labelText.alignment = TextAnchor.MiddleLeft;
            labelText.color = new Color(0.8f, 0.89f, 1f, 1f);
            labelText.text = label;
            labelText.raycastTarget = false;

            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.targetGraphic = backgroundImage;
            toggle.graphic = checkmarkImage;
            toggle.isOn = defaultValue;
            if (onChanged != null)
            {
                toggle.onValueChanged.AddListener(value => onChanged(value));
            }

            return toggle;
        }

        private ScrollRect CreatePresetScroll(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, out RectTransform viewportRect, out RectTransform contentRect)
        {
            GameObject scrollObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            RectTransform scrollRect = scrollObject.GetComponent<RectTransform>();
            scrollRect.SetParent(parent, false);
            scrollRect.anchorMin = anchorMin;
            scrollRect.anchorMax = anchorMax;
            scrollRect.offsetMin = offsetMin;
            scrollRect.offsetMax = offsetMax;

            Image scrollBackground = scrollObject.GetComponent<Image>();
            scrollBackground.color = new Color(0.05f, 0.08f, 0.12f, 0.9f);

            GameObject viewportObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(RectMask2D));
            viewportRect = viewportObject.GetComponent<RectTransform>();
            viewportRect.SetParent(scrollRect, false);
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = new Vector2(2f, 2f);
            viewportRect.offsetMax = new Vector2(-2f, -2f);
            Image viewportImage = viewportObject.GetComponent<Image>();
            viewportImage.color = new Color(0.08f, 0.12f, 0.17f, 0.72f);

            GameObject contentObject = new GameObject("Content", typeof(RectTransform));
            contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.SetParent(viewportRect, false);
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 0f);

            ScrollRect scroll = scrollObject.GetComponent<ScrollRect>();
            scroll.viewport = viewportRect;
            scroll.content = contentRect;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 20f;
            return scroll;
        }

        private void TickPresetUi()
        {
            if (!IsOpen || _presetViewportRect == null)
            {
                return;
            }

            Vector2 size = _presetViewportRect.rect.size;
            if (Vector2.SqrMagnitude(size - _lastPresetViewportSize) > 0.25f)
            {
                _lastPresetViewportSize = size;
                EnsurePresetRowPool();
                RefreshPresetRows();
            }
        }

        private void EnsureVanillaPresetsLoaded()
        {
            if (_presetsLoaded || _presetsLoading)
            {
                return;
            }

            RefreshVanillaPresets();
        }

        private void RefreshVanillaPresets()
        {
            if (_presetsLoading)
            {
                return;
            }

            _presetsLoading = true;
            _presetsLoaded = false;
            _allPresets.Clear();
            _filteredPresets.Clear();
            UpdatePresetCountText();

            try
            {
                HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                AddSpritesToPresetCatalog(Resources.LoadAll<Sprite>("GameResources"), "GameResources", seen);
                AddSpritesToPresetCatalog(Resources.LoadAll<Sprite>(string.Empty), "Resources", seen);

                _allPresets.Sort((left, right) =>
                {
                    int byName = string.Compare(left.DisplayName, right.DisplayName, StringComparison.OrdinalIgnoreCase);
                    if (byName != 0)
                    {
                        return byName;
                    }

                    return string.Compare(left.Detail, right.Detail, StringComparison.OrdinalIgnoreCase);
                });

                _presetsLoaded = true;
                ApplyPresetFilter();
                SetStatus("Presets loaded: " + _allPresets.Count, new Color(0.72f, 0.92f, 1f, 1f));
            }
            catch (Exception ex)
            {
                SetStatus("Preset load failed: " + ex.Message, new Color(1f, 0.74f, 0.74f, 1f));
            }
            finally
            {
                _presetsLoading = false;
                UpdatePresetCountText();
            }
        }

        private void AddSpritesToPresetCatalog(Sprite[] sprites, string source, HashSet<string> seen)
        {
            if (sprites == null || seen == null)
            {
                return;
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[i];
                if (sprite == null || sprite.texture == null)
                {
                    continue;
                }

                Rect rect = sprite.rect;
                int x = Mathf.RoundToInt(rect.x);
                int y = Mathf.RoundToInt(rect.y);
                int width = Mathf.Max(1, Mathf.RoundToInt(rect.width));
                int height = Mathf.Max(1, Mathf.RoundToInt(rect.height));

                string textureName = sprite.texture.name ?? string.Empty;
                string spriteName = sprite.name ?? string.Empty;
                string id = source + "|" + textureName + "|" + spriteName + "|" + x + "|" + y + "|" + width + "|" + height;
                if (!seen.Add(id))
                {
                    continue;
                }

                string displayName = string.IsNullOrWhiteSpace(spriteName) ? textureName : spriteName;
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = "sprite";
                }

                _allPresets.Add(new PresetEntry
                {
                    Id = id,
                    DisplayName = displayName,
                    Detail = source + " | " + textureName + " | " + width + "x" + height,
                    Source = source,
                    Width = width,
                    Height = height,
                    Sprite = sprite
                });
            }
        }

        private void OnPresetSearchChanged(string value)
        {
            ApplyPresetFilter();
        }

        private void ApplyPresetFilter()
        {
            _filteredPresets.Clear();

            string filter = _presetSearchInput != null ? _presetSearchInput.text : string.Empty;
            if (string.IsNullOrWhiteSpace(filter))
            {
                _filteredPresets.AddRange(_allPresets);
            }
            else
            {
                string normalized = filter.Trim();
                for (int i = 0; i < _allPresets.Count; i++)
                {
                    PresetEntry entry = _allPresets[i];
                    if (entry.DisplayName.IndexOf(normalized, StringComparison.OrdinalIgnoreCase) >= 0
                        || entry.Detail.IndexOf(normalized, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        _filteredPresets.Add(entry);
                    }
                }
            }

            UpdatePresetContentHeight();
            EnsurePresetRowPool();
            RefreshPresetRows();
            if (_presetScrollRect != null)
            {
                _presetScrollRect.verticalNormalizedPosition = 1f;
            }

            UpdatePresetCountText();
        }

        private void UpdatePresetCountText()
        {
            if (_presetCountText == null)
            {
                return;
            }

            if (_presetsLoading)
            {
                _presetCountText.text = "Loading vanilla presets...";
                return;
            }

            _presetCountText.text = "Presets: " + _filteredPresets.Count + " / " + _allPresets.Count;
        }

        private void UpdatePresetContentHeight()
        {
            if (_presetContentRect == null)
            {
                return;
            }

            float viewportHeight = _presetViewportRect != null ? _presetViewportRect.rect.height : 0f;
            float contentHeight = Mathf.Max(viewportHeight, _filteredPresets.Count * PresetRowHeight + 4f);
            _presetContentRect.sizeDelta = new Vector2(0f, contentHeight);
        }

        private void EnsurePresetRowPool()
        {
            if (_presetViewportRect == null || _presetContentRect == null)
            {
                return;
            }

            int neededRows = Mathf.Max(8, Mathf.CeilToInt(_presetViewportRect.rect.height / PresetRowHeight) + 3);
            while (_presetRows.Count < neededRows)
            {
                _presetRows.Add(CreatePresetRow(_presetRows.Count));
            }

            while (_presetRows.Count > neededRows)
            {
                PresetRowView row = _presetRows[_presetRows.Count - 1];
                if (row != null && row.RootObject != null)
                {
                    Destroy(row.RootObject);
                }

                _presetRows.RemoveAt(_presetRows.Count - 1);
            }
        }

        private PresetRowView CreatePresetRow(int rowIndex)
        {
            GameObject rowObject = new GameObject("PresetRow_" + rowIndex, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(_presetContentRect, false);
            rowRect.anchorMin = new Vector2(0f, 1f);
            rowRect.anchorMax = new Vector2(1f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);

            Image rowBackground = rowObject.GetComponent<Image>();
            rowBackground.color = new Color(0.12f, 0.17f, 0.23f, 0.86f);

            Button button = rowObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.86f, 0.96f, 1f, 1f);
            colors.pressedColor = new Color(0.76f, 0.9f, 1f, 1f);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color(0.56f, 0.56f, 0.56f, 1f);
            button.colors = colors;

            GameObject previewObject = new GameObject("Preview", typeof(RectTransform), typeof(Image));
            RectTransform previewRect = previewObject.GetComponent<RectTransform>();
            previewRect.SetParent(rowRect, false);
            previewRect.anchorMin = new Vector2(0f, 0.5f);
            previewRect.anchorMax = new Vector2(0f, 0.5f);
            previewRect.offsetMin = new Vector2(8f, -10f);
            previewRect.offsetMax = new Vector2(28f, 10f);
            Image previewImage = previewObject.GetComponent<Image>();
            previewImage.preserveAspect = true;
            previewImage.raycastTarget = false;

            Text nameText = CreateLabel(rowRect, "Name", string.Empty, 12, TextAnchor.LowerLeft, new Vector2(0f, 0.5f), new Vector2(1f, 1f), new Vector2(34f, 0f), new Vector2(-8f, 0f), new Color(0.88f, 0.95f, 1f, 1f));
            Text detailText = CreateLabel(rowRect, "Detail", string.Empty, 10, TextAnchor.UpperLeft, new Vector2(0f, 0f), new Vector2(1f, 0.5f), new Vector2(34f, 0f), new Vector2(-8f, 0f), new Color(0.63f, 0.77f, 0.92f, 1f));

            return new PresetRowView
            {
                RootObject = rowObject,
                Rect = rowRect,
                Button = button,
                Preview = previewImage,
                NameText = nameText,
                DetailText = detailText
            };
        }

        private void RefreshPresetRows()
        {
            if (_presetContentRect == null)
            {
                return;
            }

            UpdatePresetContentHeight();

            float scrollOffset = Mathf.Max(0f, _presetContentRect.anchoredPosition.y);
            int firstIndex = Mathf.Max(0, Mathf.FloorToInt(scrollOffset / PresetRowHeight));
            for (int i = 0; i < _presetRows.Count; i++)
            {
                PresetRowView row = _presetRows[i];
                int dataIndex = firstIndex + i;
                if (dataIndex < 0 || dataIndex >= _filteredPresets.Count)
                {
                    if (row.RootObject.activeSelf)
                    {
                        row.RootObject.SetActive(false);
                    }

                    row.BoundIndex = -1;
                    continue;
                }

                if (!row.RootObject.activeSelf)
                {
                    row.RootObject.SetActive(true);
                }

                PositionPresetRow(row.Rect, dataIndex);
                if (row.BoundIndex != dataIndex)
                {
                    BindPresetRow(row, dataIndex);
                }
            }
        }

        private void PositionPresetRow(RectTransform rowRect, int index)
        {
            if (rowRect == null)
            {
                return;
            }

            float top = index * PresetRowHeight;
            rowRect.offsetMin = new Vector2(4f, -(top + PresetRowHeight - 2f));
            rowRect.offsetMax = new Vector2(-4f, -(top + 2f));
        }

        private void BindPresetRow(PresetRowView row, int dataIndex)
        {
            PresetEntry entry = _filteredPresets[dataIndex];
            row.BoundIndex = dataIndex;
            row.NameText.text = entry.DisplayName;
            row.DetailText.text = entry.Detail;

            if (entry.Sprite != null)
            {
                row.Preview.sprite = entry.Sprite;
                row.Preview.color = Color.white;
            }
            else
            {
                row.Preview.sprite = null;
                row.Preview.color = new Color(0.35f, 0.43f, 0.52f, 1f);
            }

            row.Button.onClick.RemoveAllListeners();
            row.Button.onClick.AddListener(() => OnPresetClicked(entry));
        }

        private void OnPresetClicked(PresetEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            bool shouldLoad = _presetLoadOnClickToggle == null || _presetLoadOnClickToggle.isOn;
            bool shouldClone = _presetCloneOnClickToggle != null && _presetCloneOnClickToggle.isOn;
            if (!shouldLoad && !shouldClone)
            {
                SetStatus("Preset selected: " + entry.DisplayName, new Color(0.74f, 0.91f, 1f, 1f));
                return;
            }

            Texture2D texture = CreateTextureFromPreset(entry);
            if (texture == null)
            {
                SetStatus("Preset read failed: " + entry.DisplayName, new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            bool loaded = false;
            bool cloned = false;
            string loadMessage = null;
            string clonePath = null;
            string cloneError = null;

            try
            {
                if (shouldLoad)
                {
                    loaded = TryLoadPresetToCanvas(entry, texture, out loadMessage);
                }

                if (shouldClone)
                {
                    cloned = TryClonePreset(entry, texture, out clonePath, out cloneError);
                }
            }
            finally
            {
                Destroy(texture);
            }

            if (loaded && cloned)
            {
                string fileName = Path.GetFileName(clonePath);
                SetStatus((loadMessage ?? ("Loaded preset: " + entry.DisplayName)) + " | Cloned: " + fileName, new Color(0.74f, 0.95f, 0.83f, 1f));
                return;
            }

            if (loaded)
            {
                SetStatus(loadMessage ?? ("Loaded preset: " + entry.DisplayName), new Color(0.74f, 0.92f, 1f, 1f));
                return;
            }

            if (cloned)
            {
                SetStatus("Cloned preset: " + Path.GetFileName(clonePath), new Color(0.74f, 0.95f, 0.83f, 1f));
                return;
            }

            string error = !string.IsNullOrWhiteSpace(cloneError) ? cloneError : "Preset action failed: " + entry.DisplayName;
            SetStatus(error, new Color(1f, 0.74f, 0.74f, 1f));
        }

        private bool TryLoadPresetToCanvas(PresetEntry entry, Texture2D texture, out string message)
        {
            message = null;
            if (texture == null)
            {
                message = "Preset texture unavailable";
                return false;
            }

            int sourceWidth = texture.width;
            int sourceHeight = texture.height;
            int targetWidth = sourceWidth;
            int targetHeight = sourceHeight;

            if (targetWidth > MaxPresetCanvasSide || targetHeight > MaxPresetCanvasSide)
            {
                float scale = Mathf.Max((float)targetWidth / MaxPresetCanvasSide, (float)targetHeight / MaxPresetCanvasSide);
                targetWidth = Mathf.Max(1, Mathf.RoundToInt(targetWidth / scale));
                targetHeight = Mathf.Max(1, Mathf.RoundToInt(targetHeight / scale));
            }

            Color32[] pixels = BuildCanvasPixels(texture, targetWidth, targetHeight);
            if (pixels == null || pixels.Length != targetWidth * targetHeight)
            {
                message = "Preset load failed";
                return false;
            }

            CaptureUndoState();
            _width = targetWidth;
            _height = targetHeight;
            _pixels = pixels;
            _shapeDragActive = false;
            _shapeBasePixels = null;
            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RebuildGridCells();

            if (_fileNameInput != null)
            {
                _fileNameInput.text = BuildPresetBaseName(entry);
            }

            message = sourceWidth == targetWidth && sourceHeight == targetHeight
                ? "Loaded preset: " + entry.DisplayName + " (" + targetWidth + "x" + targetHeight + ")"
                : "Loaded preset: " + entry.DisplayName + " (" + sourceWidth + "x" + sourceHeight + " -> " + targetWidth + "x" + targetHeight + ")";
            return true;
        }

        private bool TryClonePreset(PresetEntry entry, Texture2D texture, out string targetPath, out string error)
        {
            targetPath = null;
            error = null;
            if (texture == null)
            {
                error = "Preset clone failed: texture unavailable";
                return false;
            }

            try
            {
                string directory = PixelArtPathResolver.ResolvePresetCloneDirectory(true);
                string safeName = PixelArtPathResolver.SanitizeFileName(BuildPresetBaseName(entry));
                string filePath = Path.Combine(directory, safeName);

                if (File.Exists(filePath))
                {
                    string stamped = Path.GetFileNameWithoutExtension(safeName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                    filePath = Path.Combine(directory, stamped);
                }

                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
                targetPath = filePath;
                return true;
            }
            catch (Exception ex)
            {
                error = "Preset clone failed: " + ex.Message;
                return false;
            }
        }

        private Color32[] BuildCanvasPixels(Texture2D texture, int targetWidth, int targetHeight)
        {
            if (texture == null || targetWidth <= 0 || targetHeight <= 0)
            {
                return null;
            }

            Color32[] source = texture.GetPixels32();
            int sourceWidth = texture.width;
            int sourceHeight = texture.height;
            if (source == null || source.Length != sourceWidth * sourceHeight)
            {
                return null;
            }

            Color32[] result = new Color32[targetWidth * targetHeight];
            for (int y = 0; y < targetHeight; y++)
            {
                int sourceYFromTop = Mathf.Clamp(Mathf.FloorToInt(((float)y + 0.5f) * sourceHeight / targetHeight), 0, sourceHeight - 1);
                int sourceYFromBottom = sourceHeight - 1 - sourceYFromTop;
                int sourceRow = sourceYFromBottom * sourceWidth;
                int targetRow = y * targetWidth;

                for (int x = 0; x < targetWidth; x++)
                {
                    int sourceX = Mathf.Clamp(Mathf.FloorToInt(((float)x + 0.5f) * sourceWidth / targetWidth), 0, sourceWidth - 1);
                    result[targetRow + x] = source[sourceRow + sourceX];
                }
            }

            return result;
        }

        private string BuildPresetBaseName(PresetEntry entry)
        {
            if (entry == null)
            {
                return "preset";
            }

            string baseName = entry.DisplayName;
            if (string.IsNullOrWhiteSpace(baseName) && entry.Sprite != null && entry.Sprite.texture != null)
            {
                baseName = entry.Sprite.texture.name;
            }

            if (string.IsNullOrWhiteSpace(baseName))
            {
                baseName = "preset";
            }

            return baseName + "_" + entry.Width + "x" + entry.Height;
        }

        private Texture2D CreateTextureFromPreset(PresetEntry entry)
        {
            if (entry == null)
            {
                return null;
            }

            if (entry.Sprite != null)
            {
                return CreateTextureFromSprite(entry.Sprite);
            }

            return null;
        }

        private Texture2D CreateTextureFromSprite(Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
            {
                return null;
            }

            Texture2D source = sprite.texture;
            Rect rect = sprite.rect;
            int width = Mathf.Max(1, Mathf.RoundToInt(rect.width));
            int height = Mathf.Max(1, Mathf.RoundToInt(rect.height));
            int x = Mathf.Clamp(Mathf.RoundToInt(rect.x), 0, Mathf.Max(0, source.width - width));
            int y = Mathf.Clamp(Mathf.RoundToInt(rect.y), 0, Mathf.Max(0, source.height - height));

            Color[] pixels = null;
            try
            {
                pixels = source.GetPixels(x, y, width, height);
            }
            catch
            {
                Texture2D readable = CopyReadableTexture(source);
                if (readable == null)
                {
                    return null;
                }

                try
                {
                    pixels = readable.GetPixels(x, y, width, height);
                }
                catch
                {
                    return null;
                }
                finally
                {
                    Destroy(readable);
                }
            }

            if (pixels == null || pixels.Length != width * height)
            {
                return null;
            }

            Texture2D output = new Texture2D(width, height, TextureFormat.RGBA32, false);
            output.filterMode = FilterMode.Point;
            output.wrapMode = TextureWrapMode.Clamp;
            output.SetPixels(pixels);
            output.Apply(false, false);
            return output;
        }

        private Texture2D CopyReadableTexture(Texture2D source)
        {
            if (source == null)
            {
                return null;
            }

            RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            RenderTexture previous = RenderTexture.active;
            try
            {
                Graphics.Blit(source, temporary);
                RenderTexture.active = temporary;

                Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                readable.filterMode = FilterMode.Point;
                readable.wrapMode = TextureWrapMode.Clamp;
                readable.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0, false);
                readable.Apply(false, false);
                return readable;
            }
            catch
            {
                return null;
            }
            finally
            {
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(temporary);
            }
        }
    }
}
