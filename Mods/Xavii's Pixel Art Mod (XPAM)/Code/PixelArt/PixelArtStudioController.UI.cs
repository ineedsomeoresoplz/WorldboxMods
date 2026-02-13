using System;
using UnityEngine;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        private void EnsureUi()
        {
            if (_studioRoot != null)
            {
                return;
            }

            RectTransform parent = PixelArtService.Instance != null ? PixelArtService.Instance.Root : null;
            if (parent == null)
            {
                return;
            }

            const float toolbarHeight = 236f;

            _studioRoot = new GameObject("XPAM_Studio", typeof(RectTransform), typeof(Image));
            RectTransform rootRect = _studioRoot.GetComponent<RectTransform>();
            rootRect.SetParent(parent, false);
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            Image backdrop = _studioRoot.GetComponent<Image>();
            backdrop.color = new Color(0.03f, 0.05f, 0.08f, 0.82f);
            backdrop.raycastTarget = true;

            _drawAreaRect = CreatePanel(_studioRoot.transform, "DrawArea", new Color(0.07f, 0.1f, 0.14f, 0.94f), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(14f, toolbarHeight + 12f), new Vector2(-14f, -14f));
            _toolbarRect = CreatePanel(_studioRoot.transform, "Toolbar", new Color(0.1f, 0.13f, 0.18f, 0.98f), new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(14f, 14f), new Vector2(-14f, toolbarHeight));

            CreateLabel(_drawAreaRect, "Title", "Xavii's Pixel Art Mod", 24, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -44f), new Vector2(-110f, -8f), new Color(0.82f, 0.93f, 1f, 1f));
            CreateButton(_drawAreaRect, "Close", "Close", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-96f, -42f), new Vector2(-10f, -10f), new Color(0.72f, 0.25f, 0.28f, 1f), HideStudio);
            _statusText = CreateLabel(_drawAreaRect, "Status", string.Empty, 14, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -76f), new Vector2(-12f, -48f), new Color(0.76f, 0.9f, 1f, 1f));

            _gridHostRect = CreatePanel(_drawAreaRect, "GridHost", new Color(0.07f, 0.1f, 0.14f, 0.82f), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(12f, 12f), new Vector2(-376f, -84f));
            _presetPanelRect = CreatePanel(_drawAreaRect, "PresetPanel", new Color(0.09f, 0.13f, 0.18f, 0.96f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-364f, 12f), new Vector2(-12f, -84f));

            _gridRect = CreatePanel(_gridHostRect, "Grid", new Color(0.16f, 0.19f, 0.23f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-320f, -320f), new Vector2(320f, 320f));
            _gridLayout = _gridRect.gameObject.AddComponent<GridLayoutGroup>();
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = _width;
            _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            _gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _gridLayout.childAlignment = TextAnchor.UpperLeft;
            _gridLayout.spacing = new Vector2(1f, 1f);
            _gridLayout.padding = new RectOffset(0, 0, 0, 0);

            RectTransform controls = CreatePanel(_toolbarRect, "Controls", new Color(0.11f, 0.15f, 0.21f, 0.95f), new Vector2(0f, 0f), new Vector2(0.72f, 1f), new Vector2(8f, 8f), new Vector2(-4f, -8f));
            RectTransform colorPanel = CreatePanel(_toolbarRect, "ColorPanel", new Color(0.12f, 0.16f, 0.22f, 0.95f), new Vector2(0.72f, 0f), new Vector2(1f, 1f), new Vector2(4f, 8f), new Vector2(-8f, -8f));

            BuildControls(controls);
            BuildColorPanel(colorPanel);
            BuildPresetPanel(_presetPanelRect);
            RebuildGridCells();
            UpdateColorUi();
            UpdateToolText();
            _studioRoot.SetActive(false);
        }

        private void BuildControls(RectTransform parent)
        {
            CreateToolButton(parent, "Pencil", ToolMode.Pencil, 8f, 8f, 84f);
            CreateToolButton(parent, "Eraser", ToolMode.Eraser, 96f, 8f, 84f);
            CreateToolButton(parent, "Fill", ToolMode.Fill, 184f, 8f, 84f);
            CreateToolButton(parent, "Line", ToolMode.Line, 272f, 8f, 84f);
            CreateToolButton(parent, "Rect", ToolMode.Rectangle, 360f, 8f, 84f);
            CreateToolButton(parent, "FillRect", ToolMode.FilledRectangle, 448f, 8f, 84f);
            CreateToolButton(parent, "Circle", ToolMode.Circle, 536f, 8f, 84f);
            CreateToolButton(parent, "FillCircle", ToolMode.FilledCircle, 624f, 8f, 84f);
            CreateToolButton(parent, "Picker", ToolMode.Picker, 712f, 8f, 84f);
            CreateToolButton(parent, "Replace", ToolMode.Replace, 800f, 8f, 84f);
            CreateToolButton(parent, "Select", ToolMode.Select, 888f, 8f, 84f);
            CreateToolButton(parent, "Move", ToolMode.Move, 976f, 8f, 84f);

            CreateTopLeftButton(parent, "Brush1", "Brush 1", 8f, 46f, 90f, 32f, new Color(0.3f, 0.43f, 0.64f, 1f), () => SetBrushSize(1));
            CreateTopLeftButton(parent, "Brush2", "Brush 2", 102f, 46f, 90f, 32f, new Color(0.3f, 0.43f, 0.64f, 1f), () => SetBrushSize(2));
            CreateTopLeftButton(parent, "Brush4", "Brush 4", 196f, 46f, 90f, 32f, new Color(0.3f, 0.43f, 0.64f, 1f), () => SetBrushSize(4));
            CreateTopLeftButton(parent, "Brush8", "Brush 8", 290f, 46f, 90f, 32f, new Color(0.3f, 0.43f, 0.64f, 1f), () => SetBrushSize(8));

            CreateTopLeftButton(parent, "Size16", "16x16", 390f, 46f, 90f, 32f, new Color(0.2f, 0.5f, 0.34f, 1f), () => NewCanvas(16));
            CreateTopLeftButton(parent, "Size32", "32x32", 484f, 46f, 90f, 32f, new Color(0.2f, 0.5f, 0.34f, 1f), () => NewCanvas(32));
            CreateTopLeftButton(parent, "Size64", "64x64", 578f, 46f, 90f, 32f, new Color(0.2f, 0.5f, 0.34f, 1f), () => NewCanvas(64));
            CreateTopLeftButton(parent, "Size96", "96x96", 672f, 46f, 90f, 32f, new Color(0.2f, 0.5f, 0.34f, 1f), () => NewCanvas(96));
            CreateTopLeftButton(parent, "Size128", "128x128", 766f, 46f, 90f, 32f, new Color(0.2f, 0.5f, 0.34f, 1f), () => NewCanvas(128));

            CreateTopLeftButton(parent, "Undo", "Undo", 8f, 84f, 90f, 32f, new Color(0.24f, 0.39f, 0.57f, 1f), Undo);
            CreateTopLeftButton(parent, "Redo", "Redo", 102f, 84f, 90f, 32f, new Color(0.24f, 0.39f, 0.57f, 1f), Redo);
            CreateTopLeftButton(parent, "Clear", "Clear", 196f, 84f, 90f, 32f, new Color(0.68f, 0.32f, 0.28f, 1f), ClearCanvas);

            Button gridButton = CreateTopLeftButton(parent, "Grid", "Grid On", 290f, 84f, 90f, 32f, new Color(0.29f, 0.45f, 0.68f, 1f), ToggleGrid);
            _gridButtonLabel = gridButton.GetComponentInChildren<Text>();

            CreateTopLeftButton(parent, "FlipX", "Flip X", 390f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), FlipHorizontal);
            CreateTopLeftButton(parent, "FlipY", "Flip Y", 484f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), FlipVertical);
            CreateTopLeftButton(parent, "RotCW", "Rot CW", 578f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), RotateClockwise);
            CreateTopLeftButton(parent, "RotCCW", "Rot CCW", 672f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), RotateCounterClockwise);
            CreateTopLeftButton(parent, "ShiftL", "Shift L", 766f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), ShiftLeft);
            CreateTopLeftButton(parent, "ShiftR", "Shift R", 860f, 84f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), ShiftRight);

            CreateTopLeftButton(parent, "SymNone", "Sym 1", 8f, 122f, 90f, 32f, new Color(0.22f, 0.4f, 0.55f, 1f), () => SetSymmetryMode(SymmetryMode.None));
            CreateTopLeftButton(parent, "SymVert", "Sym 2", 102f, 122f, 90f, 32f, new Color(0.22f, 0.4f, 0.55f, 1f), () => SetSymmetryMode(SymmetryMode.Vertical));
            CreateTopLeftButton(parent, "SymHorz", "Sym 3", 196f, 122f, 90f, 32f, new Color(0.22f, 0.4f, 0.55f, 1f), () => SetSymmetryMode(SymmetryMode.Horizontal));
            CreateTopLeftButton(parent, "SymQuad", "Sym 4", 290f, 122f, 90f, 32f, new Color(0.22f, 0.4f, 0.55f, 1f), () => SetSymmetryMode(SymmetryMode.Quadrant));

            CreateTopLeftButton(parent, "ShiftUp", "Shift Up", 390f, 122f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), ShiftUp);
            CreateTopLeftButton(parent, "ShiftDown", "Shift Down", 484f, 122f, 90f, 32f, new Color(0.27f, 0.44f, 0.62f, 1f), ShiftDown);

            Button wrapButton = CreateTopLeftButton(parent, "ShiftWrap", "Wrap Off", 578f, 122f, 90f, 32f, new Color(0.24f, 0.46f, 0.63f, 1f), ToggleWrapShift);
            _wrapShiftButtonLabel = wrapButton.GetComponentInChildren<Text>();

            CreateTopLeftButton(parent, "Import", "Import PNG", 672f, 122f, 136f, 32f, new Color(0.2f, 0.52f, 0.37f, 1f), ImportPng);
            CreateTopLeftButton(parent, "Export", "Export PNG", 812f, 122f, 136f, 32f, new Color(0.16f, 0.56f, 0.41f, 1f), ExportPng);

            _fileNameInput = CreateInputField(parent, "FileName", "sprite", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(8f, -190f), new Vector2(260f, -160f));
            _fileNameInput.text = "sprite";

            _projectNameInput = CreateInputField(parent, "ProjectName", "project", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(266f, -190f), new Vector2(518f, -160f));
            _projectNameInput.text = "project";

            CreateTopLeftButton(parent, "SaveProject", "Save Project", 524f, 160f, 136f, 30f, new Color(0.2f, 0.45f, 0.68f, 1f), SaveProject);
            CreateTopLeftButton(parent, "LoadProject", "Load Project", 664f, 160f, 136f, 30f, new Color(0.2f, 0.45f, 0.68f, 1f), LoadProject);
            CreateTopLeftButton(parent, "CopySel", "Copy", 804f, 160f, 86f, 30f, new Color(0.23f, 0.46f, 0.66f, 1f), CopySelection);
            CreateTopLeftButton(parent, "CutSel", "Cut", 894f, 160f, 86f, 30f, new Color(0.25f, 0.47f, 0.62f, 1f), CutSelection);
            CreateTopLeftButton(parent, "PasteSel", "Paste", 984f, 160f, 86f, 30f, new Color(0.21f, 0.5f, 0.65f, 1f), PasteSelection);
            CreateTopLeftButton(parent, "DelSel", "Delete", 1074f, 160f, 90f, 30f, new Color(0.63f, 0.31f, 0.29f, 1f), DeleteSelection);
            CreateTopLeftButton(parent, "SelectAll", "Select All", 1168f, 160f, 102f, 30f, new Color(0.27f, 0.44f, 0.62f, 1f), SelectAll);

            CreateLabel(parent, "Hint", "F9 Toggle  Ctrl+S Export  Ctrl+Shift+S Save  Ctrl+O Load  Ctrl+I Import  Ctrl+A/C/X/V  Q Select  M Move  Arrows Nudge  [ ] Brush", 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(8f, 4f), new Vector2(-8f, 22f), new Color(0.7f, 0.81f, 0.95f, 1f));
        }

        private void BuildColorPanel(RectTransform parent)
        {
            _toolText = CreateLabel(parent, "ToolText", string.Empty, 13, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(8f, -28f), new Vector2(-64f, -6f), new Color(0.86f, 0.93f, 1f, 1f));

            RectTransform previewRect = CreatePanel(parent, "ColorPreview", Color.white, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-56f, -34f), new Vector2(-8f, -6f));
            _colorPreviewImage = previewRect.GetComponent<Image>();

            CreateSliderRow(parent, "R", 34f, out _redSlider, out _redValueText, OnColorSliderChanged);
            CreateSliderRow(parent, "G", 58f, out _greenSlider, out _greenValueText, OnColorSliderChanged);
            CreateSliderRow(parent, "B", 82f, out _blueSlider, out _blueValueText, OnColorSliderChanged);
            CreateSliderRow(parent, "A", 106f, out _alphaSlider, out _alphaValueText, OnColorSliderChanged);

            Color32[] palette = new Color32[]
            {
                new Color32(0, 0, 0, 255),
                new Color32(255, 255, 255, 255),
                new Color32(255, 56, 56, 255),
                new Color32(255, 143, 34, 255),
                new Color32(255, 214, 58, 255),
                new Color32(100, 214, 74, 255),
                new Color32(74, 214, 203, 255),
                new Color32(73, 137, 255, 255),
                new Color32(151, 94, 255, 255),
                new Color32(255, 110, 202, 255),
                new Color32(155, 97, 51, 255),
                new Color32(128, 128, 128, 255)
            };

            for (int i = 0; i < palette.Length; i++)
            {
                int index = i;
                float x = 8f + index * 24f;
                Color32 paletteColor = palette[index];
                CreateTopLeftButton(parent, "Palette_" + index, string.Empty, x, 126f, 20f, 20f, paletteColor, () => SetSelectedColor(paletteColor));
            }

            BuildCustomPaletteSlots(parent, 150f);
        }

        private void CreateSliderRow(RectTransform parent, string label, float top, out Slider slider, out Text valueText, Action<float> onChanged)
        {
            CreateLabel(parent, label + "Label", label, 12, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(8f, -(top + 18f)), new Vector2(24f, -top), new Color(0.88f, 0.94f, 1f, 1f));
            slider = CreateSlider(parent, label + "Slider", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(26f, -(top + 16f)), new Vector2(-54f, -top), 0f, 255f, true);
            valueText = CreateLabel(parent, label + "Value", "255", 12, TextAnchor.MiddleRight, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-52f, -(top + 18f)), new Vector2(-8f, -top), new Color(0.8f, 0.88f, 1f, 1f));
            slider.onValueChanged.AddListener(value => onChanged(value));
        }

        private void CreateToolButton(RectTransform parent, string label, ToolMode mode, float left, float top, float width)
        {
            Button button = CreateTopLeftButton(parent, label + "Tool", label, left, top, width, 32f, new Color(0.23f, 0.36f, 0.56f, 1f), () => SetTool(mode));
            _toolButtonImages[mode] = button.GetComponent<Image>();
        }

        private Button CreateTopLeftButton(RectTransform parent, string name, string text, float left, float top, float width, float height, Color color, Action onClick)
        {
            return CreateButton(parent, name, text, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(left, -(top + height)), new Vector2(left + width, -top), color, onClick);
        }

        private RectTransform CreatePanel(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform rect = panelObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Image image = panelObject.GetComponent<Image>();
            image.color = color;
            return rect;
        }

        private Text CreateLabel(Transform parent, string name, string value, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, Color color)
        {
            GameObject labelObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            RectTransform rect = labelObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Text text = labelObject.GetComponent<Text>();
            text.font = PixelArtUiBootstrap.DefaultFont;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = color;
            text.text = value;
            text.raycastTarget = false;
            return text;
        }

        private Button CreateButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, Color color, Action onClick)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Image image = buttonObject.GetComponent<Image>();
            image.color = color;
            image.type = Image.Type.Sliced;

            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.SetParent(rect, false);
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(2f, 0f);
            labelRect.offsetMax = new Vector2(-2f, 0f);

            Text text = labelObject.GetComponent<Text>();
            text.font = PixelArtUiBootstrap.DefaultFont;
            text.fontSize = 12;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(0.94f, 0.97f, 1f, 1f);
            text.text = label;
            text.raycastTarget = false;
            return button;
        }

        private Slider CreateSlider(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, float min, float max, bool wholeNumbers)
        {
            GameObject sliderObject = new GameObject(name, typeof(RectTransform), typeof(Slider));
            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            sliderRect.SetParent(parent, false);
            sliderRect.anchorMin = anchorMin;
            sliderRect.anchorMax = anchorMax;
            sliderRect.offsetMin = offsetMin;
            sliderRect.offsetMax = offsetMax;

            Slider slider = sliderObject.GetComponent<Slider>();
            slider.minValue = min;
            slider.maxValue = max;
            slider.wholeNumbers = wholeNumbers;

            GameObject backgroundObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.SetParent(sliderRect, false);
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            backgroundObject.GetComponent<Image>().color = new Color(0.18f, 0.23f, 0.31f, 1f);

            GameObject fillAreaObject = new GameObject("Fill Area", typeof(RectTransform));
            RectTransform fillAreaRect = fillAreaObject.GetComponent<RectTransform>();
            fillAreaRect.SetParent(sliderRect, false);
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = new Vector2(5f, 5f);
            fillAreaRect.offsetMax = new Vector2(-5f, -5f);

            GameObject fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.SetParent(fillAreaRect, false);
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fillObject.GetComponent<Image>().color = new Color(0.4f, 0.66f, 0.96f, 1f);

            GameObject handleObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            RectTransform handleRect = handleObject.GetComponent<RectTransform>();
            handleRect.SetParent(sliderRect, false);
            handleRect.anchorMin = new Vector2(0f, 0f);
            handleRect.anchorMax = new Vector2(0f, 1f);
            handleRect.sizeDelta = new Vector2(14f, 0f);
            Image handleImage = handleObject.GetComponent<Image>();
            handleImage.color = new Color(0.95f, 0.97f, 1f, 1f);

            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;
            return slider;
        }

        private InputField CreateInputField(Transform parent, string name, string placeholder, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject inputObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(InputField));
            RectTransform inputRect = inputObject.GetComponent<RectTransform>();
            inputRect.SetParent(parent, false);
            inputRect.anchorMin = anchorMin;
            inputRect.anchorMax = anchorMax;
            inputRect.offsetMin = offsetMin;
            inputRect.offsetMax = offsetMax;

            inputObject.GetComponent<Image>().color = new Color(0.18f, 0.23f, 0.31f, 1f);
            InputField input = inputObject.GetComponent<InputField>();
            input.characterLimit = 64;

            GameObject placeholderObject = new GameObject("Placeholder", typeof(RectTransform), typeof(Text));
            RectTransform placeholderRect = placeholderObject.GetComponent<RectTransform>();
            placeholderRect.SetParent(inputRect, false);
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(8f, 0f);
            placeholderRect.offsetMax = new Vector2(-8f, 0f);

            Text placeholderText = placeholderObject.GetComponent<Text>();
            placeholderText.font = PixelArtUiBootstrap.DefaultFont;
            placeholderText.fontSize = 12;
            placeholderText.alignment = TextAnchor.MiddleLeft;
            placeholderText.color = new Color(0.56f, 0.63f, 0.74f, 1f);
            placeholderText.text = placeholder;

            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.SetParent(inputRect, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(8f, 0f);
            textRect.offsetMax = new Vector2(-8f, 0f);

            Text inputText = textObject.GetComponent<Text>();
            inputText.font = PixelArtUiBootstrap.DefaultFont;
            inputText.fontSize = 12;
            inputText.alignment = TextAnchor.MiddleLeft;
            inputText.color = new Color(0.9f, 0.95f, 1f, 1f);
            inputText.supportRichText = false;

            input.textComponent = inputText;
            input.placeholder = placeholderText;
            input.text = string.Empty;
            return input;
        }
    }
}
