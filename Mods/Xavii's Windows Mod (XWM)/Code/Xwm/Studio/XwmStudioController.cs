
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using XaviiWindowsMod.API;

namespace XaviiWindowsMod.Xwm.Studio
{
    internal class XwmStudioController : MonoBehaviour
    {
        private GameObject _studioRoot;
        private RectTransform _treeContent;
        private RectTransform _propertyContent;
        private RectTransform _previewMount;
        private Text _statusText;
        private Text _selectionText;
        private Text _typeText;
        private InputField _treeFilterInput;
        private Text _previewHintText;
        private InputField _targetModInput;
        private InputField _fileNameInput;
        private XwmDocumentData _document;
        private string _selectedId = "root";
        private XwmWindowHandle _previewHandle;
        private int _typeIndex;
        private bool _suppressPropertyCallbacks;
        private readonly Dictionary<string, InputField> _propertyInputs = new Dictionary<string, InputField>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Button> _treeButtons = new Dictionary<string, Button>(StringComparer.OrdinalIgnoreCase);
        private Button _undoButton;
        private Button _redoButton;
        private GameObject _colorPickerRoot;
        private RectTransform _colorWheelRect;
        private RectTransform _colorWheelCursor;
        private Image _colorPreviewImage;
        private Slider _colorValueSlider;
        private Slider _colorAlphaSlider;
        private InputField _colorHexInput;
        private Texture2D _colorWheelTexture;
        private Sprite _colorWheelSprite;
        private bool _suppressColorPickerCallbacks;
        private float _pickerHue;
        private float _pickerSaturation;
        private float _pickerValue = 1f;
        private float _pickerAlpha = 1f;
        private Action<string> _activeColorSetter;
        private InputField _activeColorInput;
        private Image _activeColorSwatch;
        private RectTransform _explorerPanelRect;
        private RectTransform _propertiesPanelRect;
        private RectTransform _previewPanelRect;
        private InputField _propertyFilterInput;
        private Image _studioBackdropImage;
        private Slider _backgroundTransparencySlider;
        private Text _backgroundTransparencyValueText;
        private Vector2 _lastStudioSize;
        private readonly HashSet<string> _collapsedTreeNodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, StudioPanelModel> _panelModels = new Dictionary<string, StudioPanelModel>(StringComparer.OrdinalIgnoreCase);
        private readonly List<StudioHistoryState> _undoHistory = new List<StudioHistoryState>();
        private readonly List<StudioHistoryState> _redoHistory = new List<StudioHistoryState>();
        private bool _applyingHistory;
        private bool _colorPickerHistoryCaptured;
        private bool _previewTransformHistoryCaptured;
        private string _previewTransformNodeId;
        private const int MaxHistoryStates = 120;

        private enum PanelLockLocation
        {
            Left,
            Right,
            Top,
            Bottom,
            Center,
            Free
        }

        private class StudioPanelModel
        {
            public string Key;
            public RectTransform Panel;
            public RectTransform Header;
            public RectTransform Body;
            public Text TitleText;
            public Button DockButton;
            public Text DockLabel;
            public Button LockButton;
            public Text LockLabel;
            public PanelLockLocation Location;
            public bool Locked;
            public Vector2 PreferredSize;
        }

        private class StudioHistoryState
        {
            public XwmDocumentData Document;
            public string SelectedId;
            public List<string> CollapsedNodes = new List<string>();
        }

        internal bool IsOpen => _studioRoot != null && _studioRoot.activeSelf;

        private void Awake()
        {
            _document = XwmDocumentData.CreateDefault("window");
        }

        private void OnDestroy()
        {
            if (_colorWheelSprite != null)
            {
                Destroy(_colorWheelSprite);
                _colorWheelSprite = null;
            }

            if (_colorWheelTexture != null)
            {
                Destroy(_colorWheelTexture);
                _colorWheelTexture = null;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                ToggleStudio();
                return;
            }

            if (!IsOpen || IsTypingInInputField())
            {
                return;
            }

            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (control && Input.GetKeyDown(KeyCode.S))
            {
                ExportDocument();
            }

            if (control && Input.GetKeyDown(KeyCode.N))
            {
                NewDocument();
            }

            if (control && Input.GetKeyDown(KeyCode.Z))
            {
                if (shift)
                {
                    Redo();
                }
                else
                {
                    Undo();
                }
            }

            if (control && Input.GetKeyDown(KeyCode.Y))
            {
                Redo();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSelectedNode();
            }

            if (_studioRoot != null)
            {
                RectTransform rect = _studioRoot.GetComponent<RectTransform>();
                if (rect != null)
                {
                    Vector2 size = rect.rect.size;
                    if (Vector2.SqrMagnitude(size - _lastStudioSize) > 0.25f)
                    {
                        _lastStudioSize = size;
                        ApplyLockedPanelLayout();
                    }
                }
            }
        }

        internal void ToggleStudio()
        {
            EnsureUi();
            if (IsOpen)
            {
                HideStudio();
            }
            else
            {
                ShowStudio();
            }
        }

        internal void ShowStudio()
        {
            EnsureUi();
            if (_studioRoot == null)
            {
                return;
            }

            _studioRoot.SetActive(true);
            ApplyLockedPanelLayout();
            RebuildAll();
            RefreshHistoryControls();
            SetStatus("Studio opened", new Color(0.72f, 0.93f, 1f, 1f));
        }

        internal void HideStudio()
        {
            CloseColorPicker();
            _previewTransformHistoryCaptured = false;
            _previewTransformNodeId = null;
            if (_studioRoot != null)
            {
                _studioRoot.SetActive(false);
            }

            if (_previewHandle != null)
            {
                _previewHandle.Destroy();
                _previewHandle = null;
            }
        }

        internal void SelectNodeFromPreview(string id)
        {
            SelectNode(id, true);
        }

        internal void OnPreviewDragged(string id, Vector2 anchoredPosition)
        {
            if (IsNodeControlledByLayout(id))
            {
                return;
            }

            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, id);
            if (node == null)
            {
                return;
            }

            if (!_previewTransformHistoryCaptured || !string.Equals(_previewTransformNodeId, id, StringComparison.OrdinalIgnoreCase))
            {
                BeginPreviewTransformChange(id);
            }

            XwmPropertyUtility.SetProperty(node.properties, "x", anchoredPosition.x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            XwmPropertyUtility.SetProperty(node.properties, "y", (-anchoredPosition.y).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            if (_propertyInputs.TryGetValue("x", out InputField xInput))
            {
                xInput.text = XwmPropertyUtility.GetProperty(node.properties, "x", "0");
            }

            if (_propertyInputs.TryGetValue("y", out InputField yInput))
            {
                yInput.text = XwmPropertyUtility.GetProperty(node.properties, "y", "0");
            }
        }

        internal void OnPreviewResized(string id, Vector2 size)
        {
            if (IsNodeControlledByLayout(id))
            {
                return;
            }

            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, id);
            if (node == null)
            {
                return;
            }

            if (!_previewTransformHistoryCaptured || !string.Equals(_previewTransformNodeId, id, StringComparison.OrdinalIgnoreCase))
            {
                BeginPreviewTransformChange(id);
            }

            float width = Mathf.Max(1f, size.x);
            float height = Mathf.Max(1f, size.y);
            XwmPropertyUtility.SetProperty(node.properties, "width", width.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            XwmPropertyUtility.SetProperty(node.properties, "height", height.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));

            if (_propertyInputs.TryGetValue("width", out InputField widthInput))
            {
                widthInput.text = XwmPropertyUtility.GetProperty(node.properties, "width", "0");
            }

            if (_propertyInputs.TryGetValue("height", out InputField heightInput))
            {
                heightInput.text = XwmPropertyUtility.GetProperty(node.properties, "height", "0");
            }

            if (string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
            {
                _document.canvasSize = new Vector2(width, height);
            }
        }

        internal bool IsNodeControlledByLayout(string id)
        {
            if (_document == null || string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, id);
            if (node == null || string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string parentId = string.IsNullOrWhiteSpace(node.parentId) ? "root" : node.parentId;
            List<XwmNodeData> siblings = XwmPropertyUtility.GetChildren(_document, parentId);
            for (int i = 0; i < siblings.Count; i++)
            {
                XwmNodeData sibling = siblings[i];
                if (sibling == null)
                {
                    continue;
                }

                string type = XwmTypeLibrary.Normalize(sibling.type);
                if (string.Equals(type, XwmTypeLibrary.UIListLayout, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(type, XwmTypeLibrary.UIGridLayout, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(type, XwmTypeLibrary.UIPageLayout, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(type, XwmTypeLibrary.UITableLayout, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        internal void OnLayoutDragBlocked(string id)
        {
            SetStatus("Layout-managed items can only be moved via layout properties", new Color(1f, 0.87f, 0.62f, 1f));
        }

        internal void BeginPreviewTransformChange(string id)
        {
            if (_previewTransformHistoryCaptured && string.Equals(_previewTransformNodeId, id, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (IsNodeControlledByLayout(id))
            {
                return;
            }

            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, id);
            if (node == null)
            {
                return;
            }

            CaptureUndoState();
            _previewTransformHistoryCaptured = true;
            _previewTransformNodeId = id;
        }

        internal void EndPreviewTransformChange(string id)
        {
            if (!_previewTransformHistoryCaptured)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(id) || string.Equals(_previewTransformNodeId, id, StringComparison.OrdinalIgnoreCase))
            {
                _previewTransformHistoryCaptured = false;
                _previewTransformNodeId = null;
            }
        }

        private void EnsureUi()
        {
            if (_studioRoot != null)
            {
                return;
            }

            if (_document == null)
            {
                _document = XwmDocumentData.CreateDefault("window");
            }

            RectTransform parent = WindowService.Instance != null ? WindowService.Instance.Root : null;
            if (parent == null)
            {
                return;
            }

            _studioRoot = new GameObject("XWM_Studio", typeof(RectTransform), typeof(Image));
            RectTransform studioRect = _studioRoot.GetComponent<RectTransform>();
            studioRect.SetParent(parent, false);
            studioRect.anchorMin = Vector2.zero;
            studioRect.anchorMax = Vector2.one;
            studioRect.offsetMin = Vector2.zero;
            studioRect.offsetMax = Vector2.zero;
            Image studioImage = _studioRoot.GetComponent<Image>();
            studioImage.color = new Color(0.03f, 0.05f, 0.08f, 0.78f);
            studioImage.raycastTarget = true;
            _studioBackdropImage = studioImage;
            _panelModels.Clear();

            RectTransform explorerPanel = CreatePanel(_studioRoot.transform, "ExplorerPanel", new Color(0.08f, 0.12f, 0.17f, 0.96f), new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(340f, 0f));
            RectTransform propertiesPanel = CreatePanel(_studioRoot.transform, "PropertiesPanel", new Color(0.08f, 0.11f, 0.16f, 0.96f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-360f, 0f), new Vector2(0f, 0f));
            RectTransform previewPanel = CreatePanel(_studioRoot.transform, "PreviewPanel", new Color(0.05f, 0.08f, 0.12f, 0.92f), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(340f, 0f), new Vector2(-360f, 0f));
            _explorerPanelRect = explorerPanel;
            _propertiesPanelRect = propertiesPanel;
            _previewPanelRect = previewPanel;

            RectTransform explorerHeader = CreatePanel(explorerPanel, "Header", new Color(0.1f, 0.15f, 0.22f, 0.98f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -40f), new Vector2(0f, 0f));
            RectTransform propertiesHeader = CreatePanel(propertiesPanel, "Header", new Color(0.1f, 0.15f, 0.22f, 0.98f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -40f), new Vector2(0f, 0f));
            RectTransform previewHeader = CreatePanel(previewPanel, "Header", new Color(0.1f, 0.15f, 0.22f, 0.98f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -40f), new Vector2(0f, 0f));

            Text explorerTitleText = CreateLabel(explorerHeader, "ExplorerTitle", "XWM Explorer", 17, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(10f, 0f), new Vector2(-210f, 0f), new Color(0.84f, 0.93f, 1f, 1f));
            Text propertiesTitleText = CreateLabel(propertiesHeader, "PropertiesTitle", "Properties", 17, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(10f, 0f), new Vector2(-144f, 0f), new Color(0.84f, 0.93f, 1f, 1f));
            Text previewTitleText = CreateLabel(previewHeader, "PreviewTitle", "Window Builder", 17, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(10f, 0f), new Vector2(-144f, 0f), new Color(0.84f, 0.93f, 1f, 1f));

            Button explorerDockButton = CreateAnchoredButton(explorerHeader, "DockButton", "Left", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-196f, 4f), new Vector2(-132f, -4f), null, new Color(0.24f, 0.39f, 0.57f, 1f));
            Button explorerLockButton = CreateAnchoredButton(explorerHeader, "LockButton", "Unlock", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-128f, 4f), new Vector2(-64f, -4f), null, new Color(0.66f, 0.33f, 0.23f, 1f));
            CreateAnchoredButton(explorerHeader, "CloseStudio", "Close", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-60f, 4f), new Vector2(-4f, -4f), () => HideStudio(), new Color(0.7f, 0.22f, 0.26f, 1f));

            Button propertiesDockButton = CreateAnchoredButton(propertiesHeader, "DockButton", "Right", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-136f, 4f), new Vector2(-72f, -4f), null, new Color(0.24f, 0.39f, 0.57f, 1f));
            Button propertiesLockButton = CreateAnchoredButton(propertiesHeader, "LockButton", "Unlock", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-68f, 4f), new Vector2(-4f, -4f), null, new Color(0.66f, 0.33f, 0.23f, 1f));

            Button previewDockButton = CreateAnchoredButton(previewHeader, "DockButton", "Center", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-136f, 4f), new Vector2(-72f, -4f), null, new Color(0.24f, 0.39f, 0.57f, 1f));
            Button previewLockButton = CreateAnchoredButton(previewHeader, "LockButton", "Unlock", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-68f, 4f), new Vector2(-4f, -4f), null, new Color(0.66f, 0.33f, 0.23f, 1f));

            _typeText = CreateLabel(explorerPanel, "TypeText", XwmTypeLibrary.CreatableTypes[_typeIndex], 13, TextAnchor.MiddleCenter, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(48f, -78f), new Vector2(-48f, -50f), Color.white);
            CreateAnchoredButton(explorerPanel, "TypePrev", "<", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(12f, -78f), new Vector2(40f, -50f), () => CycleType(-1), new Color(0.23f, 0.42f, 0.62f, 1f));
            CreateAnchoredButton(explorerPanel, "TypeNext", ">", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-40f, -78f), new Vector2(-12f, -50f), () => CycleType(1), new Color(0.23f, 0.42f, 0.62f, 1f));

            CreateAnchoredButton(explorerPanel, "AddChild", "Add Child", new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector2(12f, -112f), new Vector2(-4f, -82f), () => AddNode(true), new Color(0.18f, 0.52f, 0.34f, 1f));
            CreateAnchoredButton(explorerPanel, "AddSibling", "Add Sibling", new Vector2(0.5f, 1f), new Vector2(1f, 1f), new Vector2(4f, -112f), new Vector2(-12f, -82f), () => AddNode(false), new Color(0.18f, 0.43f, 0.62f, 1f));
            CreateAnchoredButton(explorerPanel, "Delete", "Delete", new Vector2(0f, 1f), new Vector2(0.33f, 1f), new Vector2(12f, -146f), new Vector2(-4f, -116f), () => DeleteSelectedNode(), new Color(0.66f, 0.24f, 0.27f, 1f));
            CreateAnchoredButton(explorerPanel, "Duplicate", "Duplicate", new Vector2(0.33f, 1f), new Vector2(0.66f, 1f), new Vector2(4f, -146f), new Vector2(-4f, -116f), () => DuplicateSelectedNode(), new Color(0.36f, 0.42f, 0.71f, 1f));
            CreateAnchoredButton(explorerPanel, "LayerDown", "Layer -", new Vector2(0.66f, 1f), new Vector2(0.83f, 1f), new Vector2(4f, -146f), new Vector2(-4f, -116f), () => MoveLayer(-1), new Color(0.24f, 0.38f, 0.61f, 1f));
            CreateAnchoredButton(explorerPanel, "LayerUp", "Layer +", new Vector2(0.83f, 1f), new Vector2(1f, 1f), new Vector2(4f, -146f), new Vector2(-12f, -116f), () => MoveLayer(1), new Color(0.24f, 0.38f, 0.61f, 1f));
            CreateAnchoredButton(explorerPanel, "OrderDown", "Order -", new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector2(12f, -180f), new Vector2(-4f, -150f), () => MoveOrder(-1), new Color(0.2f, 0.36f, 0.52f, 1f));
            CreateAnchoredButton(explorerPanel, "OrderUp", "Order +", new Vector2(0.5f, 1f), new Vector2(1f, 1f), new Vector2(4f, -180f), new Vector2(-12f, -150f), () => MoveOrder(1), new Color(0.2f, 0.36f, 0.52f, 1f));
            _undoButton = CreateAnchoredButton(explorerPanel, "Undo", "Undo", new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector2(12f, -214f), new Vector2(-4f, -184f), () => Undo(), new Color(0.34f, 0.46f, 0.22f, 1f));
            _redoButton = CreateAnchoredButton(explorerPanel, "Redo", "Redo", new Vector2(0.5f, 1f), new Vector2(1f, 1f), new Vector2(4f, -214f), new Vector2(-12f, -184f), () => Redo(), new Color(0.36f, 0.39f, 0.66f, 1f));
            CreateLabel(explorerPanel, "BackdropLabel", "Background Transparency", 11, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -248f), new Vector2(-96f, -224f), new Color(0.84f, 0.93f, 1f, 1f));
            _backgroundTransparencyValueText = CreateLabel(explorerPanel, "BackdropValue", "22%", 11, TextAnchor.MiddleRight, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-90f, -248f), new Vector2(-12f, -224f), new Color(0.84f, 0.93f, 1f, 1f));
            _backgroundTransparencySlider = CreateSlider(explorerPanel, "BackdropSlider", 0.22f, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -282f), new Vector2(-12f, -252f), value => ApplyBackgroundTransparency(value));
            _treeFilterInput = CreateInputField(explorerPanel, "TreeFilter", string.Empty, "search nodes...", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -316f), new Vector2(-12f, -286f), null);
            if (_treeFilterInput != null)
            {
                _treeFilterInput.onValueChanged.AddListener(_ => RefreshTree());
            }

            ScrollRect treeScroll = CreateScrollArea(explorerPanel, "TreeScroll", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(12f, 12f), new Vector2(-12f, -326f));
            _treeContent = treeScroll.content;

            ScrollRect propertyScroll = CreateScrollArea(propertiesPanel, "PropertyScroll", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(12f, 206f), new Vector2(-12f, -108f));
            _propertyContent = propertyScroll.content;

            _selectionText = CreateLabel(propertiesPanel, "SelectionText", "Selected: root", 12, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -74f), new Vector2(-12f, -48f), new Color(0.87f, 0.93f, 1f, 1f));
            _propertyFilterInput = CreateInputField(propertiesPanel, "PropertyFilter", string.Empty, "filter properties...", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -106f), new Vector2(-12f, -78f), null);
            if (_propertyFilterInput != null)
            {
                _propertyFilterInput.onValueChanged.AddListener(_ => RefreshProperties());
            }

            CreateLabel(propertiesPanel, "TargetModLabel", "Target Mod (GUID or folder)", 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 178f), new Vector2(-12f, 196f), new Color(0.82f, 0.9f, 1f, 1f));
            _targetModInput = CreateInputField(propertiesPanel, "TargetModInput", string.Empty, "com.your.mod", new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 146f), new Vector2(-96f, 176f), null);
            CreateAnchoredButton(propertiesPanel, "TargetModBrowse", "Browse", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-90f, 146f), new Vector2(-12f, 176f), () => BrowseTargetMod(), new Color(0.23f, 0.42f, 0.62f, 1f));
            CreateLabel(propertiesPanel, "FileNameLabel", "File Name", 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 122f), new Vector2(-12f, 140f), new Color(0.82f, 0.9f, 1f, 1f));
            _fileNameInput = CreateInputField(propertiesPanel, "FileNameInput", "window", "window", new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 90f), new Vector2(-12f, 120f), null);

            CreateAnchoredButton(propertiesPanel, "NewDoc", "New", new Vector2(0f, 0f), new Vector2(0.33f, 0f), new Vector2(12f, 54f), new Vector2(-4f, 84f), () => NewDocument(), new Color(0.22f, 0.37f, 0.58f, 1f));
            CreateAnchoredButton(propertiesPanel, "LoadDoc", "Load", new Vector2(0.33f, 0f), new Vector2(0.66f, 0f), new Vector2(4f, 54f), new Vector2(-4f, 84f), () => LoadDocument(), new Color(0.2f, 0.44f, 0.34f, 1f));
            CreateAnchoredButton(propertiesPanel, "ExportDoc", "Export .xwm", new Vector2(0.66f, 0f), new Vector2(1f, 0f), new Vector2(4f, 54f), new Vector2(-12f, 84f), () => ExportDocument(), new Color(0.57f, 0.4f, 0.17f, 1f));
            _statusText = CreateLabel(propertiesPanel, "StatusText", "Ready", 11, TextAnchor.UpperLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 10f), new Vector2(-12f, 50f), new Color(0.78f, 0.93f, 0.85f, 1f));

            RectTransform previewBody = CreatePanel(previewPanel, "PreviewBody", new Color(0.08f, 0.13f, 0.2f, 1f), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(12f, 12f), new Vector2(-12f, -48f));
            previewBody.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            _previewMount = CreatePanel(previewBody, "PreviewMount", new Color(0f, 0f, 0f, 0f), Vector2.zero, Vector2.one, new Vector2(18f, 18f), new Vector2(-18f, -18f));
            _previewHintText = CreateLabel(previewPanel, "HintText", "Drag to move. Shift-drag to resize. Ctrl to snap. Ctrl+Z undo, Ctrl+Y redo. Layout-managed items are locked.", 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 12f), new Vector2(-12f, 36f), new Color(0.74f, 0.84f, 0.95f, 0.9f));

            StudioPanelModel explorerModel = new StudioPanelModel
            {
                Key = "Explorer",
                Panel = explorerPanel,
                Header = explorerHeader,
                Body = explorerPanel,
                TitleText = explorerTitleText,
                DockButton = explorerDockButton,
                DockLabel = explorerDockButton != null ? explorerDockButton.GetComponentInChildren<Text>() : null,
                LockButton = explorerLockButton,
                LockLabel = explorerLockButton != null ? explorerLockButton.GetComponentInChildren<Text>() : null,
                Location = PanelLockLocation.Left,
                Locked = true,
                PreferredSize = new Vector2(340f, 240f)
            };
            StudioPanelModel propertiesModel = new StudioPanelModel
            {
                Key = "Properties",
                Panel = propertiesPanel,
                Header = propertiesHeader,
                Body = propertiesPanel,
                TitleText = propertiesTitleText,
                DockButton = propertiesDockButton,
                DockLabel = propertiesDockButton != null ? propertiesDockButton.GetComponentInChildren<Text>() : null,
                LockButton = propertiesLockButton,
                LockLabel = propertiesLockButton != null ? propertiesLockButton.GetComponentInChildren<Text>() : null,
                Location = PanelLockLocation.Right,
                Locked = true,
                PreferredSize = new Vector2(360f, 240f)
            };
            StudioPanelModel previewModel = new StudioPanelModel
            {
                Key = "Preview",
                Panel = previewPanel,
                Header = previewHeader,
                Body = previewPanel,
                TitleText = previewTitleText,
                DockButton = previewDockButton,
                DockLabel = previewDockButton != null ? previewDockButton.GetComponentInChildren<Text>() : null,
                LockButton = previewLockButton,
                LockLabel = previewLockButton != null ? previewLockButton.GetComponentInChildren<Text>() : null,
                Location = PanelLockLocation.Center,
                Locked = true,
                PreferredSize = Vector2.zero
            };
            _panelModels[explorerModel.Key] = explorerModel;
            _panelModels[propertiesModel.Key] = propertiesModel;
            _panelModels[previewModel.Key] = previewModel;

            AttachPanelControls(explorerModel);
            AttachPanelControls(propertiesModel);
            AttachPanelControls(previewModel);
            ApplyBackgroundTransparency(0.22f);
            _lastStudioSize = studioRect.rect.size;
            ApplyLockedPanelLayout();
            RefreshHistoryControls();

            _studioRoot.SetActive(false);
        }

        private void AttachPanelControls(StudioPanelModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.DockButton != null)
            {
                model.DockButton.onClick.RemoveAllListeners();
                model.DockButton.onClick.AddListener(() => CyclePanelLocation(model.Key));
            }

            if (model.LockButton != null)
            {
                model.LockButton.onClick.RemoveAllListeners();
                model.LockButton.onClick.AddListener(() => TogglePanelLock(model.Key));
            }

            if (model.Header != null && model.Panel != null)
            {
                XwmStudioPanelDragHandle handle = model.Header.GetComponent<XwmStudioPanelDragHandle>();
                if (handle == null)
                {
                    handle = model.Header.gameObject.AddComponent<XwmStudioPanelDragHandle>();
                }

                handle.Target = model.Panel;
                handle.CanDrag = () => !model.Locked;
                handle.Dragged = _ =>
                {
                    model.Panel.SetAsLastSibling();
                    ClampPanelInsideStudio(model.Panel);
                };
            }

            UpdatePanelHeader(model);
        }

        private void UpdatePanelHeader(StudioPanelModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.DockLabel != null)
            {
                model.DockLabel.text = model.Location.ToString();
            }

            if (model.LockLabel != null)
            {
                model.LockLabel.text = model.Locked ? "Unlock" : "Lock";
            }

            if (model.LockButton != null)
            {
                Image lockImage = model.LockButton.GetComponent<Image>();
                if (lockImage != null)
                {
                    lockImage.color = model.Locked ? new Color(0.66f, 0.33f, 0.23f, 1f) : new Color(0.24f, 0.53f, 0.34f, 1f);
                }
            }
        }

        private void CyclePanelLocation(string key)
        {
            if (!_panelModels.TryGetValue(key, out StudioPanelModel model) || model == null)
            {
                return;
            }

            switch (model.Location)
            {
                case PanelLockLocation.Left:
                    model.Location = PanelLockLocation.Right;
                    break;
                case PanelLockLocation.Right:
                    model.Location = PanelLockLocation.Top;
                    break;
                case PanelLockLocation.Top:
                    model.Location = PanelLockLocation.Bottom;
                    break;
                case PanelLockLocation.Bottom:
                    model.Location = PanelLockLocation.Center;
                    break;
                default:
                    model.Location = PanelLockLocation.Left;
                    break;
            }

            UpdatePanelHeader(model);
            ApplyLockedPanelLayout();
        }

        private void TogglePanelLock(string key)
        {
            if (!_panelModels.TryGetValue(key, out StudioPanelModel model) || model == null)
            {
                return;
            }

            model.Locked = !model.Locked;
            if (model.Locked)
            {
                ApplyLockedPanelLayout();
            }
            else
            {
                ConvertPanelToFree(model.Panel);
                ClampPanelInsideStudio(model.Panel);
            }

            UpdatePanelHeader(model);
        }

        private void ApplyBackgroundTransparency(float value)
        {
            float transparency = Mathf.Clamp01(value);
            if (_studioBackdropImage != null)
            {
                Color color = _studioBackdropImage.color;
                color.a = Mathf.Clamp01(1f - transparency);
                _studioBackdropImage.color = color;
            }

            if (_backgroundTransparencyValueText != null)
            {
                _backgroundTransparencyValueText.text = Mathf.RoundToInt(transparency * 100f) + "%";
            }
        }

        private void ApplyLockedPanelLayout()
        {
            if (_studioRoot == null)
            {
                return;
            }

            RectTransform studioRect = _studioRoot.GetComponent<RectTransform>();
            if (studioRect == null)
            {
                return;
            }

            float width = studioRect.rect.width;
            float height = studioRect.rect.height;
            if (width < 10f || height < 10f)
            {
                return;
            }

            List<StudioPanelModel> panels = new List<StudioPanelModel>(_panelModels.Values);
            panels.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));

            List<StudioPanelModel> leftPanels = new List<StudioPanelModel>();
            List<StudioPanelModel> rightPanels = new List<StudioPanelModel>();
            List<StudioPanelModel> topPanels = new List<StudioPanelModel>();
            List<StudioPanelModel> bottomPanels = new List<StudioPanelModel>();
            List<StudioPanelModel> centerPanels = new List<StudioPanelModel>();
            for (int i = 0; i < panels.Count; i++)
            {
                StudioPanelModel model = panels[i];
                if (model == null || model.Panel == null)
                {
                    continue;
                }

                if (!model.Locked)
                {
                    if (model.Panel.anchorMin != new Vector2(0f, 1f) || model.Panel.anchorMax != new Vector2(0f, 1f))
                    {
                        ConvertPanelToFree(model.Panel);
                    }

                    ClampPanelInsideStudio(model.Panel);
                    continue;
                }

                switch (model.Location)
                {
                    case PanelLockLocation.Left:
                        leftPanels.Add(model);
                        break;
                    case PanelLockLocation.Right:
                        rightPanels.Add(model);
                        break;
                    case PanelLockLocation.Top:
                        topPanels.Add(model);
                        break;
                    case PanelLockLocation.Bottom:
                        bottomPanels.Add(model);
                        break;
                    default:
                        centerPanels.Add(model);
                        break;
                }
            }

            const float spacing = 8f;
            float leftInset = SumPanelSizes(leftPanels, true, width, height, spacing);
            float rightInset = SumPanelSizes(rightPanels, true, width, height, spacing);
            float topInset = SumPanelSizes(topPanels, false, width, height, spacing);
            float bottomInset = SumPanelSizes(bottomPanels, false, width, height, spacing);

            float leftCursor = 0f;
            for (int i = 0; i < leftPanels.Count; i++)
            {
                float panelWidth = ResolvePanelWidth(leftPanels[i], width);
                SetLockedPanelRect(leftPanels[i].Panel, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(leftCursor, bottomInset), new Vector2(leftCursor + panelWidth, -topInset));
                leftCursor += panelWidth + spacing;
            }

            float rightCursor = 0f;
            for (int i = 0; i < rightPanels.Count; i++)
            {
                float panelWidth = ResolvePanelWidth(rightPanels[i], width);
                SetLockedPanelRect(rightPanels[i].Panel, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-rightCursor - panelWidth, bottomInset), new Vector2(-rightCursor, -topInset));
                rightCursor += panelWidth + spacing;
            }

            float topCursor = 0f;
            for (int i = 0; i < topPanels.Count; i++)
            {
                float panelHeight = ResolvePanelHeight(topPanels[i], height);
                SetLockedPanelRect(topPanels[i].Panel, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(leftInset, -topCursor - panelHeight), new Vector2(-rightInset, -topCursor));
                topCursor += panelHeight + spacing;
            }

            float bottomCursor = 0f;
            for (int i = 0; i < bottomPanels.Count; i++)
            {
                float panelHeight = ResolvePanelHeight(bottomPanels[i], height);
                SetLockedPanelRect(bottomPanels[i].Panel, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(leftInset, bottomCursor), new Vector2(-rightInset, bottomCursor + panelHeight));
                bottomCursor += panelHeight + spacing;
            }

            for (int i = 0; i < centerPanels.Count; i++)
            {
                float inset = i * 18f;
                SetLockedPanelRect(centerPanels[i].Panel, Vector2.zero, Vector2.one, new Vector2(leftInset + inset, bottomInset + inset), new Vector2(-rightInset - inset, -topInset - inset));
            }
        }

        private float SumPanelSizes(List<StudioPanelModel> panels, bool horizontal, float totalWidth, float totalHeight, float spacing)
        {
            float sum = 0f;
            for (int i = 0; i < panels.Count; i++)
            {
                sum += horizontal ? ResolvePanelWidth(panels[i], totalWidth) : ResolvePanelHeight(panels[i], totalHeight);
                if (i < panels.Count - 1)
                {
                    sum += spacing;
                }
            }

            return sum;
        }

        private float ResolvePanelWidth(StudioPanelModel model, float totalWidth)
        {
            float preferred = model != null && model.PreferredSize.x > 0.01f ? model.PreferredSize.x : 320f;
            return Mathf.Clamp(preferred, 160f, Mathf.Max(200f, totalWidth * 0.62f));
        }

        private float ResolvePanelHeight(StudioPanelModel model, float totalHeight)
        {
            float preferred = model != null && model.PreferredSize.y > 0.01f ? model.PreferredSize.y : 240f;
            return Mathf.Clamp(preferred, 120f, Mathf.Max(180f, totalHeight * 0.62f));
        }

        private void SetLockedPanelRect(RectTransform panel, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            if (panel == null)
            {
                return;
            }

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
            panel.pivot = new Vector2(0.5f, 0.5f);
            panel.offsetMin = offsetMin;
            panel.offsetMax = offsetMax;
        }

        private void ConvertPanelToFree(RectTransform panel)
        {
            if (panel == null || _studioRoot == null)
            {
                return;
            }

            RectTransform studioRect = _studioRoot.GetComponent<RectTransform>();
            if (studioRect == null)
            {
                return;
            }

            Vector3[] corners = new Vector3[4];
            panel.GetWorldCorners(corners);
            Vector2 bottomLeft = studioRect.InverseTransformPoint(corners[0]);
            Vector2 topRight = studioRect.InverseTransformPoint(corners[2]);
            float panelWidth = Mathf.Max(160f, topRight.x - bottomLeft.x);
            float panelHeight = Mathf.Max(120f, topRight.y - bottomLeft.y);

            panel.anchorMin = new Vector2(0f, 1f);
            panel.anchorMax = new Vector2(0f, 1f);
            panel.pivot = new Vector2(0f, 1f);
            panel.sizeDelta = new Vector2(panelWidth, panelHeight);
            panel.anchoredPosition = new Vector2(bottomLeft.x - studioRect.rect.xMin, topRight.y - studioRect.rect.yMax);
        }

        private void ClampPanelInsideStudio(RectTransform panel)
        {
            if (panel == null || _studioRoot == null)
            {
                return;
            }

            RectTransform studioRect = _studioRoot.GetComponent<RectTransform>();
            if (studioRect == null)
            {
                return;
            }

            Vector3[] corners = new Vector3[4];
            panel.GetWorldCorners(corners);
            Vector2 bottomLeft = studioRect.InverseTransformPoint(corners[0]);
            Vector2 topRight = studioRect.InverseTransformPoint(corners[2]);
            float margin = 24f;
            float dx = 0f;
            float dy = 0f;

            if (topRight.x < studioRect.rect.xMin + margin)
            {
                dx = studioRect.rect.xMin + margin - topRight.x;
            }
            else if (bottomLeft.x > studioRect.rect.xMax - margin)
            {
                dx = studioRect.rect.xMax - margin - bottomLeft.x;
            }

            if (topRight.y < studioRect.rect.yMin + margin)
            {
                dy = studioRect.rect.yMin + margin - topRight.y;
            }
            else if (bottomLeft.y > studioRect.rect.yMax - margin)
            {
                dy = studioRect.rect.yMax - margin - bottomLeft.y;
            }

            if (Mathf.Abs(dx) > 0.01f || Mathf.Abs(dy) > 0.01f)
            {
                panel.anchoredPosition += new Vector2(dx, dy);
            }
        }

        private RectTransform CreatePanel(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            Image image = panel.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            return rect;
        }

        private Text CreateLabel(Transform parent, string name, string value, int fontSize, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, Color color)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Text text = textObject.GetComponent<Text>();
            text.font = XwmUiBootstrap.DefaultFont;
            text.fontSize = fontSize;
            text.text = value;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private Button CreateAnchoredButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, UnityAction onClick, Color color)
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

            Button button = buttonObject.GetComponent<Button>();
            if (onClick != null)
            {
                button.onClick.AddListener(onClick);
            }

            CreateLabel(buttonObject.transform, "Label", label, 12, TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, new Vector2(3f, 2f), new Vector2(-3f, -2f), Color.white);
            return button;
        }

        private InputField CreateInputField(Transform parent, string name, string value, string placeholder, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, UnityAction<string> onEndEdit)
        {
            GameObject inputObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(InputField));
            RectTransform rect = inputObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Image image = inputObject.GetComponent<Image>();
            image.color = new Color(0.14f, 0.19f, 0.27f, 1f);

            InputField input = inputObject.GetComponent<InputField>();
            Text text = CreateLabel(inputObject.transform, "Text", value, 12, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(8f, 2f), new Vector2(-8f, -2f), Color.white);
            Text place = CreateLabel(inputObject.transform, "Placeholder", placeholder, 12, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(8f, 2f), new Vector2(-8f, -2f), new Color(0.67f, 0.75f, 0.82f, 0.8f));
            input.textComponent = text;
            input.placeholder = place;
            input.text = value;
            if (onEndEdit != null)
            {
                input.onEndEdit.AddListener(onEndEdit);
            }

            return input;
        }

        private Toggle CreateCheckbox(Transform parent, string name, bool value, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, UnityAction<bool> onChanged)
        {
            GameObject toggleObject = new GameObject(name, typeof(RectTransform), typeof(Toggle));
            RectTransform rect = toggleObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            GameObject backgroundObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.SetParent(rect, false);
            backgroundRect.anchorMin = new Vector2(0f, 0.5f);
            backgroundRect.anchorMax = new Vector2(0f, 0.5f);
            backgroundRect.pivot = new Vector2(0f, 0.5f);
            backgroundRect.sizeDelta = new Vector2(20f, 20f);
            backgroundRect.anchoredPosition = new Vector2(8f, 0f);
            Image background = backgroundObject.GetComponent<Image>();
            background.color = new Color(0.15f, 0.21f, 0.3f, 1f);

            GameObject checkmarkObject = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
            RectTransform checkmarkRect = checkmarkObject.GetComponent<RectTransform>();
            checkmarkRect.SetParent(backgroundRect, false);
            checkmarkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkmarkRect.sizeDelta = new Vector2(12f, 12f);
            Image checkmark = checkmarkObject.GetComponent<Image>();
            checkmark.color = new Color(0.35f, 0.78f, 0.43f, 1f);

            Text valueText = CreateLabel(toggleObject.transform, "Value", value ? "true" : "false", 12, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(36f, 2f), new Vector2(-8f, -2f), Color.white);

            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.targetGraphic = background;
            toggle.graphic = checkmark;
            toggle.isOn = value;
            if (onChanged != null)
            {
                toggle.onValueChanged.AddListener(v =>
                {
                    valueText.text = v ? "true" : "false";
                    onChanged.Invoke(v);
                });
            }

            return toggle;
        }

        private Dropdown CreateDropdown(Transform parent, string name, IList<string> options, string value, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, UnityAction<string> onChanged)
        {
            GameObject dropdownObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Dropdown));
            RectTransform rect = dropdownObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            Image background = dropdownObject.GetComponent<Image>();
            background.color = new Color(0.14f, 0.19f, 0.27f, 1f);

            Text label = CreateLabel(dropdownObject.transform, "Label", string.Empty, 12, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(8f, 2f), new Vector2(-24f, -2f), Color.white);
            Text arrow = CreateLabel(dropdownObject.transform, "Arrow", "v", 12, TextAnchor.MiddleCenter, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-20f, 2f), new Vector2(-6f, -2f), new Color(0.82f, 0.9f, 1f, 1f));
            arrow.raycastTarget = false;

            GameObject templateObject = new GameObject("Template", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            RectTransform templateRect = templateObject.GetComponent<RectTransform>();
            templateRect.SetParent(rect, false);
            templateRect.anchorMin = new Vector2(0f, 0f);
            templateRect.anchorMax = new Vector2(1f, 0f);
            templateRect.pivot = new Vector2(0.5f, 1f);
            templateRect.anchoredPosition = new Vector2(0f, -2f);
            templateRect.sizeDelta = new Vector2(0f, 120f);
            Image templateImage = templateObject.GetComponent<Image>();
            templateImage.color = new Color(0.1f, 0.15f, 0.22f, 0.98f);

            GameObject viewportObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
            viewportRect.SetParent(templateRect, false);
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = new Vector2(4f, 4f);
            viewportRect.offsetMax = new Vector2(-4f, -4f);
            Image viewportImage = viewportObject.GetComponent<Image>();
            viewportImage.color = new Color(1f, 1f, 1f, 0.02f);
            viewportObject.GetComponent<Mask>().showMaskGraphic = false;

            GameObject contentObject = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            RectTransform contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.SetParent(viewportRect, false);
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 0f);
            VerticalLayoutGroup layout = contentObject.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 2f;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            ContentSizeFitter fitter = contentObject.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            GameObject itemObject = new GameObject("Item", typeof(RectTransform), typeof(Toggle));
            RectTransform itemRect = itemObject.GetComponent<RectTransform>();
            itemRect.SetParent(contentRect, false);
            itemRect.anchorMin = new Vector2(0f, 1f);
            itemRect.anchorMax = new Vector2(1f, 1f);
            itemRect.pivot = new Vector2(0.5f, 1f);
            itemRect.sizeDelta = new Vector2(0f, 24f);

            GameObject itemBackgroundObject = new GameObject("Item Background", typeof(RectTransform), typeof(Image));
            RectTransform itemBackgroundRect = itemBackgroundObject.GetComponent<RectTransform>();
            itemBackgroundRect.SetParent(itemRect, false);
            itemBackgroundRect.anchorMin = Vector2.zero;
            itemBackgroundRect.anchorMax = Vector2.one;
            itemBackgroundRect.offsetMin = Vector2.zero;
            itemBackgroundRect.offsetMax = Vector2.zero;
            Image itemBackground = itemBackgroundObject.GetComponent<Image>();
            itemBackground.color = new Color(0.12f, 0.19f, 0.28f, 0.96f);

            GameObject itemCheckmarkObject = new GameObject("Item Checkmark", typeof(RectTransform), typeof(Image));
            RectTransform itemCheckmarkRect = itemCheckmarkObject.GetComponent<RectTransform>();
            itemCheckmarkRect.SetParent(itemRect, false);
            itemCheckmarkRect.anchorMin = new Vector2(0f, 0.5f);
            itemCheckmarkRect.anchorMax = new Vector2(0f, 0.5f);
            itemCheckmarkRect.pivot = new Vector2(0f, 0.5f);
            itemCheckmarkRect.sizeDelta = new Vector2(14f, 14f);
            itemCheckmarkRect.anchoredPosition = new Vector2(6f, 0f);
            Image itemCheckmark = itemCheckmarkObject.GetComponent<Image>();
            itemCheckmark.color = new Color(0.35f, 0.78f, 0.43f, 1f);

            Text itemLabel = CreateLabel(itemRect, "Item Label", "Option", 12, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(24f, 1f), new Vector2(-6f, -1f), Color.white);

            Toggle itemToggle = itemObject.GetComponent<Toggle>();
            itemToggle.targetGraphic = itemBackground;
            itemToggle.graphic = itemCheckmark;
            itemToggle.isOn = false;

            ScrollRect scrollRect = templateObject.GetComponent<ScrollRect>();
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 18f;

            templateObject.SetActive(false);

            Dropdown dropdown = dropdownObject.GetComponent<Dropdown>();
            dropdown.template = templateRect;
            dropdown.captionText = label;
            dropdown.itemText = itemLabel;
            dropdown.options = new List<Dropdown.OptionData>();
            if (options != null)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    dropdown.options.Add(new Dropdown.OptionData(options[i]));
                }
            }

            int selectedIndex = 0;
            if (!string.IsNullOrWhiteSpace(value) && options != null)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    if (string.Equals(options[i], value, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }

            dropdown.value = Mathf.Clamp(selectedIndex, 0, Mathf.Max(0, dropdown.options.Count - 1));
            dropdown.RefreshShownValue();
            if (onChanged != null)
            {
                dropdown.onValueChanged.AddListener(index =>
                {
                    if (index < 0 || index >= dropdown.options.Count)
                    {
                        return;
                    }

                    onChanged.Invoke(dropdown.options[index].text);
                });
            }

            return dropdown;
        }

        private Slider CreateSlider(Transform parent, string name, float value, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, UnityAction<float> onChanged)
        {
            GameObject sliderObject = new GameObject(name, typeof(RectTransform), typeof(Slider));
            RectTransform rect = sliderObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            GameObject backgroundObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.SetParent(rect, false);
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = new Vector2(0f, 8f);
            backgroundRect.offsetMax = new Vector2(0f, -8f);
            Image backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.color = new Color(0.16f, 0.22f, 0.31f, 1f);

            GameObject fillAreaObject = new GameObject("FillArea", typeof(RectTransform));
            RectTransform fillAreaRect = fillAreaObject.GetComponent<RectTransform>();
            fillAreaRect.SetParent(rect, false);
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = new Vector2(6f, 10f);
            fillAreaRect.offsetMax = new Vector2(-6f, -10f);

            GameObject fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.SetParent(fillAreaRect, false);
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            Image fillImage = fillObject.GetComponent<Image>();
            fillImage.color = new Color(0.39f, 0.67f, 0.96f, 1f);

            GameObject handleAreaObject = new GameObject("HandleSlideArea", typeof(RectTransform));
            RectTransform handleAreaRect = handleAreaObject.GetComponent<RectTransform>();
            handleAreaRect.SetParent(rect, false);
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(6f, 0f);
            handleAreaRect.offsetMax = new Vector2(-6f, 0f);

            GameObject handleObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            RectTransform handleRect = handleObject.GetComponent<RectTransform>();
            handleRect.SetParent(handleAreaRect, false);
            handleRect.sizeDelta = new Vector2(14f, 26f);
            Image handleImage = handleObject.GetComponent<Image>();
            handleImage.color = Color.white;

            Slider slider = sliderObject.GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = Mathf.Clamp01(value);
            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;
            if (onChanged != null)
            {
                slider.onValueChanged.AddListener(onChanged);
            }

            return slider;
        }

        private ScrollRect CreateScrollArea(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject scrollObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            RectTransform scrollRect = scrollObject.GetComponent<RectTransform>();
            scrollRect.SetParent(parent, false);
            scrollRect.anchorMin = anchorMin;
            scrollRect.anchorMax = anchorMax;
            scrollRect.offsetMin = offsetMin;
            scrollRect.offsetMax = offsetMax;
            scrollObject.GetComponent<Image>().color = new Color(0.05f, 0.08f, 0.12f, 0.85f);

            GameObject viewportObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
            viewportRect.SetParent(scrollRect, false);
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            Image viewportImage = viewportObject.GetComponent<Image>();
            viewportImage.color = new Color(1f, 1f, 1f, 0.02f);
            viewportObject.GetComponent<Mask>().showMaskGraphic = false;

            GameObject contentObject = new GameObject("Content", typeof(RectTransform));
            RectTransform contentRect = contentObject.GetComponent<RectTransform>();
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
            scroll.scrollSensitivity = 18f;
            return scroll;
        }

        private void CycleType(int direction)
        {
            _typeIndex += direction;
            if (_typeIndex < 0)
            {
                _typeIndex = XwmTypeLibrary.CreatableTypes.Length - 1;
            }

            if (_typeIndex >= XwmTypeLibrary.CreatableTypes.Length)
            {
                _typeIndex = 0;
            }

            if (_typeText != null)
            {
                _typeText.text = XwmTypeLibrary.CreatableTypes[_typeIndex];
            }
        }

        private string CurrentType => XwmTypeLibrary.CreatableTypes[Mathf.Clamp(_typeIndex, 0, XwmTypeLibrary.CreatableTypes.Length - 1)];

        private void AddNode(bool asChild)
        {
            if (_document == null)
            {
                return;
            }

            XwmNodeData selected = XwmPropertyUtility.GetNodeById(_document, _selectedId) ?? XwmPropertyUtility.GetRootNode(_document);
            string parentId;
            if (asChild)
            {
                parentId = selected != null ? selected.id : "root";
            }
            else
            {
                if (selected == null || string.Equals(selected.id, "root", StringComparison.OrdinalIgnoreCase))
                {
                    parentId = "root";
                }
                else
                {
                    parentId = string.IsNullOrWhiteSpace(selected.parentId) ? "root" : selected.parentId;
                }
            }

            XwmNodeData node = new XwmNodeData
            {
                id = XwmPropertyUtility.NextNodeId(_document),
                parentId = string.IsNullOrWhiteSpace(parentId) ? "root" : parentId,
                type = CurrentType,
                name = XwmPropertyUtility.NextNodeName(_document, CurrentType),
                order = _document.nextOrder++,
                layer = ResolveNextLayer(parentId),
                active = true,
                properties = XwmPropertyUtility.CreateDefaultProperties(CurrentType)
            };
            CaptureUndoState();
            _document.nodes.Add(node);
            SelectNode(node.id, false);
            RebuildAll();
            SetStatus("Added " + node.type, new Color(0.75f, 0.94f, 0.84f, 1f));
        }

        private int ResolveNextLayer(string parentId)
        {
            List<XwmNodeData> siblings = XwmPropertyUtility.GetChildren(_document, parentId);
            int max = 0;
            for (int i = 0; i < siblings.Count; i++)
            {
                if (siblings[i].layer > max)
                {
                    max = siblings[i].layer;
                }
            }

            return siblings.Count == 0 ? 0 : max + 1;
        }
        private void DeleteSelectedNode()
        {
            if (_document == null || string.IsNullOrWhiteSpace(_selectedId) || string.Equals(_selectedId, "root", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            HashSet<string> toRemove = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectDescendants(_selectedId, toRemove);
            toRemove.Add(_selectedId);

            CaptureUndoState();
            _document.nodes.RemoveAll(n => n != null && toRemove.Contains(n.id));
            _selectedId = "root";
            RebuildAll();
            SetStatus("Node deleted", new Color(0.95f, 0.76f, 0.76f, 1f));
        }

        private void CollectDescendants(string id, HashSet<string> output)
        {
            List<XwmNodeData> children = XwmPropertyUtility.GetChildren(_document, id);
            for (int i = 0; i < children.Count; i++)
            {
                XwmNodeData child = children[i];
                if (child == null || output.Contains(child.id))
                {
                    continue;
                }

                output.Add(child.id);
                CollectDescendants(child.id, output);
            }
        }

        private void DuplicateSelectedNode()
        {
            if (_document == null || string.IsNullOrWhiteSpace(_selectedId) || string.Equals(_selectedId, "root", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            XwmNodeData source = XwmPropertyUtility.GetNodeById(_document, _selectedId);
            if (source == null)
            {
                return;
            }

            CaptureUndoState();
            Dictionary<string, string> idMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            XwmNodeData cloneRoot = DuplicateRecursive(source, source.parentId, idMap, true);
            if (cloneRoot != null)
            {
                _selectedId = cloneRoot.id;
                RebuildAll();
                SetStatus("Node duplicated", new Color(0.78f, 0.86f, 1f, 1f));
            }
        }

        private XwmNodeData DuplicateRecursive(XwmNodeData source, string newParentId, Dictionary<string, string> idMap, bool offsetRoot)
        {
            if (source == null)
            {
                return null;
            }

            XwmNodeData clone = new XwmNodeData
            {
                id = XwmPropertyUtility.NextNodeId(_document),
                parentId = string.IsNullOrWhiteSpace(newParentId) ? "root" : newParentId,
                type = source.type,
                name = source.name + "_copy",
                order = _document.nextOrder++,
                layer = source.layer + (offsetRoot ? 1 : 0),
                active = source.active,
                properties = CopyProperties(source.properties)
            };

            if (offsetRoot)
            {
                float x = XwmPropertyUtility.GetFloat(clone.properties, "x", 20f) + 20f;
                float y = XwmPropertyUtility.GetFloat(clone.properties, "y", 20f) + 20f;
                XwmPropertyUtility.SetProperty(clone.properties, "x", x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
                XwmPropertyUtility.SetProperty(clone.properties, "y", y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            }

            _document.nodes.Add(clone);
            idMap[source.id] = clone.id;

            List<XwmNodeData> children = XwmPropertyUtility.GetChildren(_document, source.id);
            for (int i = 0; i < children.Count; i++)
            {
                DuplicateRecursive(children[i], clone.id, idMap, false);
            }

            return clone;
        }

        private List<XwmPropertyData> CopyProperties(List<XwmPropertyData> source)
        {
            List<XwmPropertyData> copy = new List<XwmPropertyData>();
            if (source == null)
            {
                return copy;
            }

            for (int i = 0; i < source.Count; i++)
            {
                XwmPropertyData property = source[i];
                if (property == null)
                {
                    continue;
                }

                copy.Add(new XwmPropertyData { key = property.key, value = property.value });
            }

            return copy;
        }

        private void MoveLayer(int delta)
        {
            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, _selectedId);
            if (node == null || string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            CaptureUndoState();
            node.layer += delta;
            if (node.layer < 0)
            {
                node.layer = 0;
            }

            RebuildAll();
        }

        private void MoveOrder(int delta)
        {
            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, _selectedId);
            if (node == null || string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string parentId = string.IsNullOrWhiteSpace(node.parentId) ? "root" : node.parentId;
            List<XwmNodeData> siblings = XwmPropertyUtility.GetChildren(_document, parentId);
            List<XwmNodeData> sameLayer = new List<XwmNodeData>();
            for (int i = 0; i < siblings.Count; i++)
            {
                if (siblings[i].layer == node.layer)
                {
                    sameLayer.Add(siblings[i]);
                }
            }

            int index = sameLayer.IndexOf(node);
            if (index < 0)
            {
                return;
            }

            int targetIndex = Mathf.Clamp(index + delta, 0, sameLayer.Count - 1);
            if (targetIndex == index)
            {
                return;
            }

            CaptureUndoState();
            int swapOrder = sameLayer[targetIndex].order;
            sameLayer[targetIndex].order = node.order;
            node.order = swapOrder;
            RebuildAll();
        }

        private void ToggleNodeActive(string id)
        {
            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, id);
            if (node == null)
            {
                return;
            }

            CaptureUndoState();
            node.active = !node.active;
            XwmPropertyUtility.SetProperty(node.properties, "active", node.active ? "true" : "false");
            if (string.Equals(id, _selectedId, StringComparison.OrdinalIgnoreCase))
            {
                RefreshProperties();
            }

            RefreshTree();
            RebuildPreview();
        }

        private void SelectNode(string id, bool rebuild)
        {
            if (string.IsNullOrWhiteSpace(id) || XwmPropertyUtility.GetNodeById(_document, id) == null)
            {
                _selectedId = "root";
            }
            else
            {
                _selectedId = id;
            }

            if (rebuild)
            {
                ExpandTreePath(_selectedId);
                RefreshTree();
                RefreshProperties();
                RefreshPreviewSelection();
            }
        }

        private void RebuildAll()
        {
            XwmPropertyUtility.EnsureDocument(_document);
            RefreshTree();
            RefreshProperties();
            RebuildPreview();
        }

        private void RefreshTree()
        {
            if (_treeContent == null)
            {
                return;
            }

            for (int i = _treeContent.childCount - 1; i >= 0; i--)
            {
                Destroy(_treeContent.GetChild(i).gameObject);
            }

            _treeButtons.Clear();
            string filter = _treeFilterInput != null ? _treeFilterInput.text : string.Empty;
            filter = string.IsNullOrWhiteSpace(filter) ? string.Empty : filter.Trim();
            float y = 0f;
            XwmNodeData root = _document != null ? XwmPropertyUtility.GetRootNode(_document) : null;
            if (root != null)
            {
                bool rootHasChildren = HasVisibleChildren(root.id, filter);
                bool rootExpanded = string.IsNullOrWhiteSpace(filter) ? !_collapsedTreeNodes.Contains(root.id) : true;
                AddTreeRow(root, 0, ref y, MatchesFilter(root, filter), rootHasChildren, rootExpanded);
                if (rootHasChildren && rootExpanded)
                {
                    AddTreeRows(root.id, 1, ref y, filter);
                }
            }
            _treeContent.sizeDelta = new Vector2(0f, Mathf.Max(8f, y + 8f));
            RefreshTreeSelection();
        }

        private void AddTreeRows(string parentId, int depth, ref float y, string filter)
        {
            List<XwmNodeData> children = XwmPropertyUtility.GetChildren(_document, parentId);
            bool forceExpanded = !string.IsNullOrWhiteSpace(filter);
            for (int i = 0; i < children.Count; i++)
            {
                XwmNodeData node = children[i];
                if (!HasFilterMatch(node, filter))
                {
                    continue;
                }

                bool hasChildren = HasVisibleChildren(node.id, filter);
                bool expanded = forceExpanded || !_collapsedTreeNodes.Contains(node.id);
                AddTreeRow(node, depth, ref y, MatchesFilter(node, filter), hasChildren, expanded);
                if (hasChildren && expanded)
                {
                    AddTreeRows(node.id, depth + 1, ref y, filter);
                }
            }
        }

        private void AddTreeRow(XwmNodeData node, int depth, ref float y, bool match, bool hasChildren, bool expanded)
        {
            if (node == null)
            {
                return;
            }

            GameObject rowObject = new GameObject("TreeRow_" + node.id, typeof(RectTransform));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(_treeContent, false);
            rowRect.anchorMin = new Vector2(0f, 1f);
            rowRect.anchorMax = new Vector2(1f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, -y);
            rowRect.sizeDelta = new Vector2(0f, 26f);
            y += 28f;

            string capturedId = node.id;
            GameObject selectObject = new GameObject("Select", typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform selectRect = selectObject.GetComponent<RectTransform>();
            selectRect.SetParent(rowObject.transform, false);
            selectRect.anchorMin = Vector2.zero;
            selectRect.anchorMax = Vector2.one;
            selectRect.offsetMin = Vector2.zero;
            selectRect.offsetMax = Vector2.zero;
            Image image = selectObject.GetComponent<Image>();
            image.color = new Color(0.14f, 0.18f, 0.26f, 0.9f);

            Button button = selectObject.GetComponent<Button>();
            button.onClick.AddListener(() => SelectNode(capturedId, true));
            float indent = 8f + depth * 16f;
            if (hasChildren)
            {
                CreateAnchoredButton(rowObject.transform, "Expand", expanded ? "v" : ">", new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(indent, 4f), new Vector2(indent + 18f, -4f), () => ToggleTreeCollapse(capturedId), new Color(0.2f, 0.27f, 0.39f, 1f));
            }

            CreateLabel(rowObject.transform, "Name", node.name, 12, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(indent + 22f, 1f), new Vector2(-172f, -1f), match ? Color.white : new Color(0.68f, 0.76f, 0.86f, 0.76f));
            CreateLabel(rowObject.transform, "Type", node.type, 10, TextAnchor.MiddleRight, Vector2.zero, Vector2.one, new Vector2(0f, 1f), new Vector2(-108f, -1f), new Color(0.74f, 0.82f, 0.92f, 0.85f));
            CreateLabel(rowObject.transform, "LayerOrder", "L" + node.layer + " O" + node.order, 10, TextAnchor.MiddleRight, Vector2.zero, Vector2.one, new Vector2(0f, 1f), new Vector2(-60f, -1f), new Color(0.74f, 0.82f, 0.92f, 0.72f));
            CreateAnchoredButton(rowObject.transform, "ActiveToggle", node.active ? "On" : "Off", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-54f, 3f), new Vector2(-6f, -3f), () => ToggleNodeActive(capturedId), node.active ? new Color(0.22f, 0.58f, 0.34f, 1f) : new Color(0.22f, 0.24f, 0.3f, 1f));
            _treeButtons[node.id] = button;
        }

        private bool HasVisibleChildren(string parentId, string filter)
        {
            List<XwmNodeData> children = XwmPropertyUtility.GetChildren(_document, parentId);
            for (int i = 0; i < children.Count; i++)
            {
                if (HasFilterMatch(children[i], filter))
                {
                    return true;
                }
            }

            return false;
        }

        private void ToggleTreeCollapse(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return;
            }

            if (_collapsedTreeNodes.Contains(nodeId))
            {
                _collapsedTreeNodes.Remove(nodeId);
            }
            else
            {
                _collapsedTreeNodes.Add(nodeId);
            }

            RefreshTree();
        }

        private void ExpandTreePath(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId) || _document == null)
            {
                return;
            }

            string current = nodeId;
            int guard = 0;
            while (!string.IsNullOrWhiteSpace(current) && guard++ < 256)
            {
                _collapsedTreeNodes.Remove(current);
                XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, current);
                if (node == null)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(node.parentId))
                {
                    _collapsedTreeNodes.Remove("root");
                    break;
                }

                current = node.parentId;
            }
        }

        private void RefreshTreeSelection()
        {
            foreach (KeyValuePair<string, Button> pair in _treeButtons)
            {
                if (pair.Value == null)
                {
                    continue;
                }

                Image image = pair.Value.GetComponent<Image>();
                if (image == null)
                {
                    continue;
                }

                bool selected = string.Equals(pair.Key, _selectedId, StringComparison.OrdinalIgnoreCase);
                XwmNodeData node = _document != null ? XwmPropertyUtility.GetNodeById(_document, pair.Key) : null;
                bool active = node == null || node.active;
                Color baseColor = active ? new Color(0.14f, 0.18f, 0.26f, 0.92f) : new Color(0.1f, 0.12f, 0.18f, 0.7f);
                image.color = selected ? new Color(0.24f, 0.45f, 0.72f, 0.96f) : baseColor;
            }
        }

        private bool MatchesFilter(XwmNodeData node, string filter)
        {
            if (node == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            string query = filter.Trim();
            return (!string.IsNullOrWhiteSpace(node.name) && node.name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                   || (!string.IsNullOrWhiteSpace(node.type) && node.type.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                   || (!string.IsNullOrWhiteSpace(node.id) && node.id.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool HasFilterMatch(XwmNodeData node, string filter)
        {
            if (node == null)
            {
                return false;
            }

            if (MatchesFilter(node, filter))
            {
                return true;
            }

            List<XwmNodeData> children = XwmPropertyUtility.GetChildren(_document, node.id);
            for (int i = 0; i < children.Count; i++)
            {
                if (HasFilterMatch(children[i], filter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PropertyMatchesFilter(string key, string value, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            string query = filter.Trim();
            return (!string.IsNullOrWhiteSpace(key) && key.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                   || (!string.IsNullOrWhiteSpace(value) && value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void RefreshProperties()
        {
            if (_propertyContent == null)
            {
                return;
            }

            CloseColorPicker();

            for (int i = _propertyContent.childCount - 1; i >= 0; i--)
            {
                Destroy(_propertyContent.GetChild(i).gameObject);
            }

            _propertyInputs.Clear();
            XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, _selectedId) ?? XwmPropertyUtility.GetRootNode(_document);
            if (node == null)
            {
                return;
            }

            if (_selectionText != null)
            {
                _selectionText.text = "Selected: " + node.name + " (" + node.type + ")";
            }

            List<string> keys = new List<string>(XwmPropertyUtility.GetEditableKeys(node.type));
            string propertyFilter = _propertyFilterInput != null ? _propertyFilterInput.text : string.Empty;

            float y = 0f;
            _suppressPropertyCallbacks = true;

            if (PropertyMatchesFilter("name", node.name, propertyFilter))
            {
                AddPropertyRow(node, "name", node.name, ref y, value =>
                {
                    CaptureUndoState();
                    node.name = string.IsNullOrWhiteSpace(value) ? node.type : value.Trim();
                    RefreshTree();
                    RefreshPreviewSelection();
                });
            }

            string parentValue = string.IsNullOrWhiteSpace(node.parentId) ? "root" : node.parentId;
            if (PropertyMatchesFilter("parentId", parentValue, propertyFilter))
            {
                AddPropertyRow(node, "parentId", parentValue, ref y, value =>
                {
                    if (string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
                    {
                        node.parentId = string.Empty;
                        return;
                    }

                    string candidate = string.IsNullOrWhiteSpace(value) ? "root" : value.Trim();
                    if (string.Equals(candidate, node.id, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    if (!string.Equals(candidate, "root", StringComparison.OrdinalIgnoreCase) && XwmPropertyUtility.GetNodeById(_document, candidate) == null)
                    {
                        return;
                    }

                    CaptureUndoState();
                    node.parentId = candidate;
                    RebuildAll();
                });
            }

            string layerValue = node.layer.ToString();
            if (PropertyMatchesFilter("layer", layerValue, propertyFilter))
            {
                AddPropertyRow(node, "layer", layerValue, ref y, value =>
                {
                    if (int.TryParse(value, out int parsed))
                    {
                        CaptureUndoState();
                        node.layer = Mathf.Max(0, parsed);
                        RebuildAll();
                    }
                });
            }

            string orderValue = node.order.ToString();
            if (PropertyMatchesFilter("order", orderValue, propertyFilter))
            {
                AddPropertyRow(node, "order", orderValue, ref y, value =>
                {
                    if (int.TryParse(value, out int parsed))
                    {
                        CaptureUndoState();
                        node.order = Mathf.Max(0, parsed);
                        RebuildAll();
                    }
                });
            }

            string activeValue = node.active.ToString().ToLowerInvariant();
            if (PropertyMatchesFilter("active", activeValue, propertyFilter))
            {
                AddPropertyRow(node, "active", activeValue, ref y, value =>
                {
                    if (bool.TryParse(value, out bool parsed))
                    {
                        CaptureUndoState();
                        node.active = parsed;
                        XwmPropertyUtility.SetProperty(node.properties, "active", parsed ? "true" : "false");
                        RebuildPreview();
                        RefreshTree();
                    }
                });
            }

            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                if (string.Equals(key, "name", StringComparison.OrdinalIgnoreCase) || string.Equals(key, "parentId", StringComparison.OrdinalIgnoreCase) || string.Equals(key, "layer", StringComparison.OrdinalIgnoreCase) || string.Equals(key, "active", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string value = XwmPropertyUtility.GetProperty(node.properties, key, string.Empty);
                if (!PropertyMatchesFilter(key, value, propertyFilter))
                {
                    continue;
                }

                AddPropertyRow(node, key, value, ref y, inputValue =>
                {
                    CapturePropertyUndoState(key);
                    XwmPropertyUtility.SetProperty(node.properties, key, inputValue);
                    if (string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase) && (string.Equals(key, "width", StringComparison.OrdinalIgnoreCase) || string.Equals(key, "height", StringComparison.OrdinalIgnoreCase)))
                    {
                        float w = XwmPropertyUtility.GetFloat(node.properties, "width", _document.canvasSize.x);
                        float h = XwmPropertyUtility.GetFloat(node.properties, "height", _document.canvasSize.y);
                        _document.canvasSize = new Vector2(Mathf.Max(1f, w), Mathf.Max(1f, h));
                    }

                    RebuildPreview();
                    if (string.Equals(key, "textScaled", StringComparison.OrdinalIgnoreCase))
                    {
                        RefreshProperties();
                    }
                });
            }

            _propertyContent.sizeDelta = new Vector2(0f, Mathf.Max(12f, y + 12f));
            _suppressPropertyCallbacks = false;
        }

        private void AddPropertyRow(XwmNodeData node, string key, string value, ref float y, Action<string> onChanged)
        {
            bool locked = IsPropertyLocked(node, key);
            GameObject row = new GameObject("Property_" + key, typeof(RectTransform), typeof(Image));
            RectTransform rowRect = row.GetComponent<RectTransform>();
            rowRect.SetParent(_propertyContent, false);
            rowRect.anchorMin = new Vector2(0f, 1f);
            rowRect.anchorMax = new Vector2(1f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, -y);
            rowRect.sizeDelta = new Vector2(0f, 30f);
            y += 32f;

            Image rowImage = row.GetComponent<Image>();
            rowImage.color = locked ? new Color(0.1f, 0.13f, 0.18f, 0.9f) : new Color(0.12f, 0.16f, 0.23f, 0.9f);

            CreateLabel(row.transform, "Key", key, 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(8f, 4f), new Vector2(130f, -4f), new Color(0.86f, 0.93f, 1f, 1f));
            if (IsBooleanPropertyKey(key, value))
            {
                bool initial = ParseBooleanValue(value, false);
                Toggle toggle = CreateCheckbox(row.transform, "ValueToggle", initial, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(136f, 4f), new Vector2(-6f, -4f), v =>
                {
                    if (_suppressPropertyCallbacks)
                    {
                        return;
                    }

                    onChanged?.Invoke(v ? "true" : "false");
                });
                if (toggle != null)
                {
                    toggle.interactable = !locked;
                }
                return;
            }

            if (TryGetDropdownOptions(key, out List<string> options))
            {
                Dropdown dropdown = CreateDropdown(row.transform, "ValueDropdown", options, value, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(136f, 4f), new Vector2(-6f, -4f), selected =>
                {
                    if (_suppressPropertyCallbacks)
                    {
                        return;
                    }

                    onChanged?.Invoke(selected);
                });
                if (dropdown != null)
                {
                    dropdown.interactable = !locked;
                }
                return;
            }

            bool colorKey = IsColorPropertyKey(key);
            bool imageKey = !colorKey && IsImagePropertyKey(key);
            bool fontTypeKey = string.Equals(key, "fontType", StringComparison.OrdinalIgnoreCase);
            Image swatch = null;
            Vector2 inputOffsetMax = colorKey
                ? new Vector2(-40f, -4f)
                : imageKey
                    ? new Vector2(-78f, -4f)
                    : fontTypeKey
                        ? new Vector2(-114f, -4f)
                        : new Vector2(-6f, -4f);
            InputField input = CreateInputField(row.transform, "Value", value, key, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(136f, 4f), inputOffsetMax, s =>
            {
                if (_suppressPropertyCallbacks)
                {
                    return;
                }

                onChanged?.Invoke(s);
                if (colorKey)
                {
                    UpdateColorSwatch(swatch, s);
                }
            });
            if (input != null)
            {
                input.interactable = !locked;
                if (locked)
                {
                    Image inputImage = input.GetComponent<Image>();
                    if (inputImage != null)
                    {
                        inputImage.color = new Color(0.11f, 0.14f, 0.19f, 1f);
                    }
                }
            }

            if (fontTypeKey)
            {
                List<string> fontOptions = XwmUiBootstrap.GetAvailableFontTypes(value);
                Dropdown fontDropdown = CreateDropdown(row.transform, "FontPicker", fontOptions, value, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-108f, 4f), new Vector2(-6f, -4f), selected =>
                {
                    if (_suppressPropertyCallbacks)
                    {
                        return;
                    }

                    if (input != null)
                    {
                        input.text = selected;
                    }

                    onChanged?.Invoke(selected);
                });
                if (fontDropdown != null)
                {
                    fontDropdown.interactable = !locked;
                }
            }

            if (colorKey)
            {
                swatch = CreateColorSwatch(row.transform, value);
                Button swatchButton = swatch.gameObject.AddComponent<Button>();
                swatchButton.targetGraphic = swatch;
                swatchButton.onClick.AddListener(() => OpenColorPicker(input, swatch, onChanged));
                swatchButton.interactable = !locked;
            }
            else if (imageKey)
            {
                Button browseButton = CreateAnchoredButton(row.transform, "BrowseImage", "Browse", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-72f, 4f), new Vector2(-6f, -4f), () => BrowseImagePropertyValue(key, input, onChanged), new Color(0.23f, 0.42f, 0.62f, 1f));
                if (browseButton != null)
                {
                    browseButton.interactable = !locked;
                }
            }

            _propertyInputs[key] = input;
        }

        private bool IsPropertyLocked(XwmNodeData node, string key)
        {
            if (node == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (!string.Equals(key, "fontSize", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return IsTextNodeType(node.type) && XwmPropertyUtility.GetBool(node.properties, "textScaled", false);
        }

        private bool IsTextNodeType(string type)
        {
            string normalized = XwmTypeLibrary.Normalize(type);
            return string.Equals(normalized, XwmTypeLibrary.TextLabel, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(normalized, XwmTypeLibrary.TextButton, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(normalized, XwmTypeLibrary.TextBox, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsColorPropertyKey(string key)
        {
            return !string.IsNullOrWhiteSpace(key) && key.IndexOf("color", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool IsImagePropertyKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            string normalized = key.Trim().ToLowerInvariant();
            if (normalized.EndsWith("transparency"))
            {
                return false;
            }

            return normalized == "sprite"
                   || normalized.EndsWith("sprite")
                   || normalized.Contains("image")
                   || normalized.Contains("icon")
                   || normalized.EndsWith("texture");
        }

        private bool IsBooleanPropertyKey(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (bool.TryParse(value, out _))
            {
                return true;
            }

            string normalized = key.Trim().ToLowerInvariant();
            return normalized == "active"
                   || normalized == "enabled"
                   || normalized == "interactable"
                   || normalized == "textscaled"
                   || normalized == "textwrapped"
                   || normalized == "childcontrolwidth"
                   || normalized == "childcontrolheight"
                   || normalized == "childforceexpandwidth"
                   || normalized == "childforceexpandheight"
                   || normalized == "raycasttarget"
                   || normalized == "scrollhorizontal"
                   || normalized == "scrollvertical";
        }

        private bool ParseBooleanValue(string value, bool fallback)
        {
            if (bool.TryParse(value, out bool parsed))
            {
                return parsed;
            }

            if (string.Equals(value, "1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(value, "0", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return fallback;
        }

        private bool TryGetDropdownOptions(string key, out List<string> options)
        {
            options = null;
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            string normalized = key.Trim().ToLowerInvariant();
            if (normalized == "orientation" || normalized == "axis")
            {
                options = new List<string> { "Vertical", "Horizontal" };
                return true;
            }

            if (normalized == "constraint")
            {
                options = new List<string> { "FixedColumnCount", "FixedRowCount", "Flexible" };
                return true;
            }

            if (normalized == "alignment")
            {
                options = new List<string>
                {
                    "UpperLeft",
                    "UpperCenter",
                    "UpperRight",
                    "MiddleLeft",
                    "MiddleCenter",
                    "MiddleRight",
                    "LowerLeft",
                    "LowerCenter",
                    "LowerRight"
                };
                return true;
            }

            return false;
        }

        private void BrowseImagePropertyValue(string key, InputField input, Action<string> onChanged)
        {
            if (!XwmNativeFolderPicker.TryPickImageFile(out string selectedPath))
            {
                SetStatus("Image picker canceled or unavailable", new Color(1f, 0.78f, 0.72f, 1f));
                return;
            }

            string resolved = ResolvePickedImagePropertyValue(selectedPath);
            if (string.IsNullOrWhiteSpace(resolved))
            {
                SetStatus("Invalid image path", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            if (input != null)
            {
                input.text = resolved;
            }

            onChanged?.Invoke(resolved);
            SetStatus("Image path selected for " + key, new Color(0.74f, 0.93f, 1f, 1f));
        }

        private string ResolvePickedImagePropertyValue(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return string.Empty;
            }

            string candidate;
            try
            {
                candidate = Path.GetFullPath(selectedPath.Trim());
            }
            catch
            {
                candidate = selectedPath.Trim();
            }

            string normalized = candidate.Replace('\\', '/');
            string withoutExtension = Path.ChangeExtension(normalized, null) ?? normalized;
            string fromResources = TryExtractPathAfterMarker(withoutExtension, "/Resources/");
            if (!string.IsNullOrWhiteSpace(fromResources))
            {
                return fromResources;
            }

            string fromGameResources = TryExtractPathAfterMarker(withoutExtension, "/GameResources/");
            if (!string.IsNullOrWhiteSpace(fromGameResources))
            {
                return fromGameResources;
            }

            try
            {
                string modsRoot = Path.GetFullPath(XwmPathResolver.ModsRoot).TrimEnd('\\', '/').Replace('\\', '/');
                string fullCandidate = Path.GetFullPath(candidate).TrimEnd('\\', '/').Replace('\\', '/');
                if (fullCandidate.StartsWith(modsRoot + "/", StringComparison.OrdinalIgnoreCase))
                {
                    return Path.ChangeExtension(fullCandidate.Substring(modsRoot.Length + 1), null).Replace('\\', '/');
                }
            }
            catch
            {
            }

            return withoutExtension;
        }

        private string TryExtractPathAfterMarker(string value, string marker)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(marker))
            {
                return string.Empty;
            }

            int index = value.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return string.Empty;
            }

            return value.Substring(index + marker.Length).TrimStart('/').Trim();
        }

        private Image CreateColorSwatch(Transform parent, string value)
        {
            GameObject swatchObject = new GameObject("ColorSwatch", typeof(RectTransform), typeof(Image));
            RectTransform swatchRect = swatchObject.GetComponent<RectTransform>();
            swatchRect.SetParent(parent, false);
            swatchRect.anchorMin = new Vector2(1f, 0f);
            swatchRect.anchorMax = new Vector2(1f, 1f);
            swatchRect.offsetMin = new Vector2(-34f, 4f);
            swatchRect.offsetMax = new Vector2(-6f, -4f);
            Image swatchImage = swatchObject.GetComponent<Image>();
            UpdateColorSwatch(swatchImage, value);
            return swatchImage;
        }

        private void UpdateColorSwatch(Image swatch, string value)
        {
            if (swatch == null)
            {
                return;
            }

            swatch.color = XwmPropertyUtility.ParseColor(value, Color.white);
        }

        private void AddCustomPropertyRow(ref float y)
        {
            GameObject row = new GameObject("CustomProperty", typeof(RectTransform), typeof(Image));
            RectTransform rowRect = row.GetComponent<RectTransform>();
            rowRect.SetParent(_propertyContent, false);
            rowRect.anchorMin = new Vector2(0f, 1f);
            rowRect.anchorMax = new Vector2(1f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, -y);
            rowRect.sizeDelta = new Vector2(0f, 34f);
            y += 36f;

            row.GetComponent<Image>().color = new Color(0.1f, 0.15f, 0.22f, 0.94f);

            InputField keyInput = CreateInputField(row.transform, "KeyInput", string.Empty, "new key", new Vector2(0f, 0f), new Vector2(0.4f, 1f), new Vector2(6f, 4f), new Vector2(-3f, -4f), null);
            InputField valueInput = CreateInputField(row.transform, "ValueInput", string.Empty, "value", new Vector2(0.4f, 0f), new Vector2(0.82f, 1f), new Vector2(3f, 4f), new Vector2(-3f, -4f), null);
            CreateAnchoredButton(row.transform, "AddProperty", "Add", new Vector2(0.82f, 0f), new Vector2(1f, 1f), new Vector2(3f, 4f), new Vector2(-6f, -4f), () =>
            {
                XwmNodeData node = XwmPropertyUtility.GetNodeById(_document, _selectedId);
                if (node == null)
                {
                    return;
                }

                string key = keyInput.text != null ? keyInput.text.Trim() : string.Empty;
                if (string.IsNullOrWhiteSpace(key))
                {
                    return;
                }

                CaptureUndoState();
                XwmPropertyUtility.SetProperty(node.properties, key, valueInput.text ?? string.Empty);
                RefreshProperties();
                RebuildPreview();
            }, new Color(0.27f, 0.47f, 0.24f, 1f));
        }

        private void OpenColorPicker(InputField input, Image swatch, Action<string> onChanged)
        {
            EnsureColorPickerUi();
            if (_colorPickerRoot == null)
            {
                return;
            }

            _activeColorInput = input;
            _activeColorSwatch = swatch;
            _activeColorSetter = onChanged;
            _colorPickerHistoryCaptured = false;

            Color initial = swatch != null ? swatch.color : Color.white;
            if (input != null && !string.IsNullOrWhiteSpace(input.text))
            {
                initial = XwmPropertyUtility.ParseColor(input.text, initial);
            }

            SetColorPickerFromColor(initial, false);
            _colorPickerRoot.SetActive(true);
            _colorPickerRoot.transform.SetAsLastSibling();
        }

        private void CloseColorPicker()
        {
            if (_colorPickerRoot != null)
            {
                _colorPickerRoot.SetActive(false);
            }

            _activeColorSetter = null;
            _activeColorInput = null;
            _activeColorSwatch = null;
            _colorPickerHistoryCaptured = false;
        }

        private void EnsureColorPickerUi()
        {
            if (_studioRoot == null || _colorPickerRoot != null)
            {
                return;
            }

            _colorPickerRoot = new GameObject("XWM_ColorPicker", typeof(RectTransform), typeof(Image));
            RectTransform rootRect = _colorPickerRoot.GetComponent<RectTransform>();
            rootRect.SetParent(_studioRoot.transform, false);
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;
            Image overlay = _colorPickerRoot.GetComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.58f);
            overlay.raycastTarget = true;

            RectTransform panel = CreatePanel(_colorPickerRoot.transform, "PickerPanel", new Color(0.08f, 0.12f, 0.18f, 0.99f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-220f, -190f), new Vector2(220f, 190f));
            CreateLabel(panel, "PickerTitle", "Color Picker", 16, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -38f), new Vector2(-96f, -8f), new Color(0.88f, 0.95f, 1f, 1f));
            CreateAnchoredButton(panel, "PickerClose", "Close", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-84f, -38f), new Vector2(-12f, -8f), () => CloseColorPicker(), new Color(0.57f, 0.23f, 0.27f, 1f));

            _colorWheelRect = CreatePanel(panel, "Wheel", Color.white, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(14f, 70f), new Vector2(214f, 270f));
            Image wheelImage = _colorWheelRect.GetComponent<Image>();
            wheelImage.sprite = BuildColorWheelSprite(256);
            wheelImage.type = Image.Type.Simple;
            wheelImage.preserveAspect = true;
            wheelImage.color = Color.white;
            XwmColorWheelInput wheelInput = _colorWheelRect.gameObject.AddComponent<XwmColorWheelInput>();
            wheelInput.TargetRect = _colorWheelRect;
            wheelInput.PointerChanged += OnColorWheelPointerChanged;

            GameObject cursorObject = new GameObject("WheelCursor", typeof(RectTransform), typeof(Image), typeof(Outline));
            _colorWheelCursor = cursorObject.GetComponent<RectTransform>();
            _colorWheelCursor.SetParent(_colorWheelRect, false);
            _colorWheelCursor.anchorMin = new Vector2(0.5f, 0.5f);
            _colorWheelCursor.anchorMax = new Vector2(0.5f, 0.5f);
            _colorWheelCursor.sizeDelta = new Vector2(16f, 16f);
            Image cursorImage = cursorObject.GetComponent<Image>();
            cursorImage.color = Color.white;
            cursorImage.raycastTarget = false;
            Outline cursorOutline = cursorObject.GetComponent<Outline>();
            cursorOutline.effectColor = new Color(0f, 0f, 0f, 1f);
            cursorOutline.effectDistance = new Vector2(1f, -1f);

            CreateLabel(panel, "ValueLabel", "Value", 11, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -80f), new Vector2(-12f, -58f), new Color(0.84f, 0.92f, 1f, 1f));
            _colorValueSlider = CreateSlider(panel, "ValueSlider", 1f, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -110f), new Vector2(-12f, -82f), v => OnColorValueChanged(v));

            CreateLabel(panel, "AlphaLabel", "Alpha", 11, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -144f), new Vector2(-12f, -122f), new Color(0.84f, 0.92f, 1f, 1f));
            _colorAlphaSlider = CreateSlider(panel, "AlphaSlider", 1f, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -174f), new Vector2(-12f, -146f), v => OnColorAlphaChanged(v));

            CreateLabel(panel, "HexLabel", "Hex", 11, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -208f), new Vector2(-12f, -186f), new Color(0.84f, 0.92f, 1f, 1f));
            _colorHexInput = CreateInputField(panel, "HexInput", "#FFFFFFFF", "#RRGGBBAA", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(228f, -242f), new Vector2(-12f, -210f), s => OnColorHexEdited(s));

            RectTransform previewRect = CreatePanel(panel, "PreviewRect", new Color(1f, 1f, 1f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(228f, 14f), new Vector2(-12f, 58f));
            _colorPreviewImage = previewRect.GetComponent<Image>();
            CreateLabel(panel, "PreviewLabel", "Preview", 11, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(228f, 60f), new Vector2(-12f, 84f), new Color(0.84f, 0.92f, 1f, 1f));

            _colorPickerRoot.SetActive(false);
        }

        private Sprite BuildColorWheelSprite(int size)
        {
            if (_colorWheelSprite != null && _colorWheelTexture != null && _colorWheelTexture.width == size && _colorWheelTexture.height == size)
            {
                return _colorWheelSprite;
            }

            if (_colorWheelSprite != null)
            {
                Destroy(_colorWheelSprite);
                _colorWheelSprite = null;
            }

            if (_colorWheelTexture != null)
            {
                Destroy(_colorWheelTexture);
                _colorWheelTexture = null;
            }

            _colorWheelTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            _colorWheelTexture.wrapMode = TextureWrapMode.Clamp;
            _colorWheelTexture.filterMode = FilterMode.Bilinear;

            float center = (size - 1) * 0.5f;
            float radius = center;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = (x - center) / radius;
                    float dy = (y - center) / radius;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    if (distance > 1f)
                    {
                        _colorWheelTexture.SetPixel(x, y, new Color(0f, 0f, 0f, 0f));
                        continue;
                    }

                    float hue = Mathf.Atan2(dy, dx) / (Mathf.PI * 2f);
                    if (hue < 0f)
                    {
                        hue += 1f;
                    }

                    Color color = Color.HSVToRGB(hue, distance, 1f);
                    color.a = 1f;
                    _colorWheelTexture.SetPixel(x, y, color);
                }
            }

            _colorWheelTexture.Apply();
            _colorWheelSprite = Sprite.Create(_colorWheelTexture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _colorWheelSprite;
        }

        private void OnColorWheelPointerChanged(Vector2 normalized)
        {
            Vector2 centered = new Vector2(normalized.x * 2f - 1f, normalized.y * 2f - 1f);
            float magnitude = centered.magnitude;
            if (magnitude > 1f)
            {
                centered /= magnitude;
            }

            _pickerSaturation = Mathf.Clamp01(centered.magnitude);
            float hue = Mathf.Atan2(centered.y, centered.x) / (Mathf.PI * 2f);
            if (hue < 0f)
            {
                hue += 1f;
            }

            _pickerHue = hue;
            UpdateColorPickerUi(true);
        }

        private void OnColorValueChanged(float value)
        {
            if (_suppressColorPickerCallbacks)
            {
                return;
            }

            _pickerValue = Mathf.Clamp01(value);
            UpdateColorPickerUi(true);
        }

        private void OnColorAlphaChanged(float value)
        {
            if (_suppressColorPickerCallbacks)
            {
                return;
            }

            _pickerAlpha = Mathf.Clamp01(value);
            UpdateColorPickerUi(true);
        }

        private void OnColorHexEdited(string value)
        {
            if (_suppressColorPickerCallbacks)
            {
                return;
            }

            Color fallback = Color.HSVToRGB(_pickerHue, _pickerSaturation, _pickerValue);
            fallback.a = _pickerAlpha;
            Color parsed = XwmPropertyUtility.ParseColor(value, fallback);
            SetColorPickerFromColor(parsed, true);
        }

        private void SetColorPickerFromColor(Color color, bool applyToTarget)
        {
            Color colorNoAlpha = new Color(color.r, color.g, color.b, 1f);
            Color.RGBToHSV(colorNoAlpha, out _pickerHue, out _pickerSaturation, out _pickerValue);
            _pickerAlpha = Mathf.Clamp01(color.a);
            UpdateColorPickerUi(applyToTarget);
        }

        private void UpdateColorPickerUi(bool applyToTarget)
        {
            Color color = Color.HSVToRGB(_pickerHue, _pickerSaturation, _pickerValue);
            color.a = _pickerAlpha;

            _suppressColorPickerCallbacks = true;
            if (_colorValueSlider != null)
            {
                _colorValueSlider.value = _pickerValue;
            }

            if (_colorAlphaSlider != null)
            {
                _colorAlphaSlider.value = _pickerAlpha;
            }

            if (_colorHexInput != null)
            {
                _colorHexInput.text = XwmPropertyUtility.ToColorString(color);
            }

            if (_colorPreviewImage != null)
            {
                _colorPreviewImage.color = color;
            }

            SetSliderFillColor(_colorValueSlider, Color.HSVToRGB(_pickerHue, _pickerSaturation, 1f));
            SetSliderFillColor(_colorAlphaSlider, new Color(color.r, color.g, color.b, 1f));
            UpdateColorWheelCursor();
            _suppressColorPickerCallbacks = false;

            if (applyToTarget)
            {
                ApplyColorPickerToActive(color);
            }
        }

        private void SetSliderFillColor(Slider slider, Color color)
        {
            if (slider == null || slider.fillRect == null)
            {
                return;
            }

            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = color;
            }
        }

        private void UpdateColorWheelCursor()
        {
            if (_colorWheelRect == null || _colorWheelCursor == null)
            {
                return;
            }

            float radius = Mathf.Min(_colorWheelRect.rect.width, _colorWheelRect.rect.height) * 0.5f;
            float radians = _pickerHue * Mathf.PI * 2f;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            _colorWheelCursor.anchoredPosition = direction * (_pickerSaturation * radius);
        }

        private void ApplyColorPickerToActive(Color color)
        {
            string colorString = XwmPropertyUtility.ToColorString(color);
            if (_activeColorInput != null)
            {
                _activeColorInput.text = colorString;
            }

            if (_activeColorSwatch != null)
            {
                _activeColorSwatch.color = color;
            }

            _activeColorSetter?.Invoke(colorString);
        }

        private void BrowseTargetMod()
        {
            if (!XwmNativeFolderPicker.TryPickFolder(out string selectedPath))
            {
                SetStatus("Folder picker canceled or unavailable", new Color(1f, 0.78f, 0.72f, 1f));
                return;
            }

            string resolved = ResolvePickedTargetModValue(selectedPath);
            if (string.IsNullOrWhiteSpace(resolved))
            {
                SetStatus("Invalid target folder", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            if (_targetModInput != null)
            {
                _targetModInput.text = resolved;
            }

            SetStatus("Target mod selected", new Color(0.74f, 0.93f, 1f, 1f));
        }

        private string ResolvePickedTargetModValue(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return string.Empty;
            }

            string candidate;
            try
            {
                candidate = Path.GetFullPath(selectedPath.Trim());
            }
            catch
            {
                candidate = selectedPath.Trim();
            }

            try
            {
                if (Directory.Exists(candidate))
                {
                    string modsRoot = Path.GetFullPath(XwmPathResolver.ModsRoot).TrimEnd('\\', '/');
                    DirectoryInfo info = new DirectoryInfo(candidate);
                    while (info != null)
                    {
                        string current = info.FullName.TrimEnd('\\', '/');
                        if (File.Exists(Path.Combine(current, "mod.json")))
                        {
                            candidate = current;
                            break;
                        }

                        if (string.Equals(current, modsRoot, StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        info = info.Parent;
                    }
                }
            }
            catch
            {
            }

            try
            {
                string modsRoot = Path.GetFullPath(XwmPathResolver.ModsRoot).TrimEnd('\\', '/');
                string fullCandidate = Path.GetFullPath(candidate).TrimEnd('\\', '/');
                if (fullCandidate.StartsWith(modsRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || fullCandidate.StartsWith(modsRoot + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    string relative = fullCandidate.Substring(modsRoot.Length).TrimStart('\\', '/');
                    if (relative.IndexOf('\\') < 0 && relative.IndexOf('/') < 0)
                    {
                        return relative;
                    }
                }
            }
            catch
            {
            }

            return candidate;
        }

        private void RebuildPreview()
        {
            if (_previewMount == null)
            {
                return;
            }

            if (_previewHandle != null)
            {
                _previewHandle.Destroy();
                _previewHandle = null;
            }

            _previewHandle = XwmRuntimeFactory.Build(_document, _previewMount, "studio_preview", "studio", "preview", true, true, OnPreviewElementReady);
            RefreshPreviewSelection();
        }

        private void OnPreviewElementReady(XwmElementRef element)
        {
            if (element == null || element.GameObject == null)
            {
                return;
            }

            XwmStudioSelectionProxy proxy = element.GameObject.GetComponent<XwmStudioSelectionProxy>();
            if (proxy == null)
            {
                proxy = element.GameObject.AddComponent<XwmStudioSelectionProxy>();
            }

            proxy.NodeId = element.Id;
            proxy.Controller = this;
        }

        private void RefreshPreviewSelection()
        {
            if (_previewHandle == null || _previewHandle.Elements == null)
            {
                return;
            }

            for (int i = 0; i < _previewHandle.Elements.Count; i++)
            {
                XwmElementRef element = _previewHandle.Elements[i];
                if (element == null || element.GameObject == null)
                {
                    continue;
                }

                Graphic graphic = element.GameObject.GetComponent<Graphic>();
                Outline outline = element.GameObject.GetComponent<Outline>();
                bool selected = string.Equals(element.Id, _selectedId, StringComparison.OrdinalIgnoreCase);
                if (selected && graphic != null)
                {
                    if (outline == null)
                    {
                        outline = element.GameObject.AddComponent<Outline>();
                    }

                    outline.effectColor = new Color(1f, 0.87f, 0.25f, 1f);
                    outline.effectDistance = new Vector2(2f, -2f);
                }
                else
                {
                    if (outline != null)
                    {
                        Destroy(outline);
                    }
                }
            }
        }

        private void NewDocument()
        {
            string name = _fileNameInput != null && !string.IsNullOrWhiteSpace(_fileNameInput.text) ? _fileNameInput.text.Trim() : "window";
            CaptureUndoState();
            _document = XwmDocumentData.CreateDefault(name);
            _selectedId = "root";
            _collapsedTreeNodes.Clear();
            RebuildAll();
            SetStatus("New document created", new Color(0.76f, 0.9f, 1f, 1f));
        }

        private void ExportDocument()
        {
            CommitFocusedInputField();

            if (_document == null)
            {
                return;
            }

            string targetMod = _targetModInput != null ? _targetModInput.text : string.Empty;
            string fileName = _fileNameInput != null ? _fileNameInput.text : string.Empty;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                SetStatus("Provide file name", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            _document.documentName = fileName.Trim();
            string path = XwmPathResolver.ResolveXwmFilePath(targetMod, fileName, true);
            if (string.IsNullOrWhiteSpace(path))
            {
                SetStatus("Target mod not found", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            bool saved = XwmSerializer.Save(path, _document);
            SetStatus(saved ? "Exported " + path : "Export failed", saved ? new Color(0.72f, 0.95f, 0.84f, 1f) : new Color(1f, 0.74f, 0.74f, 1f));
        }

        private void LoadDocument()
        {
            string targetMod = _targetModInput != null ? _targetModInput.text : string.Empty;
            string fileName = _fileNameInput != null ? _fileNameInput.text : string.Empty;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                SetStatus("Provide file name", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            string path = XwmPathResolver.ResolveXwmFilePath(targetMod, fileName, false);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                SetStatus("File not found", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            XwmDocumentData loaded = XwmSerializer.Load(path);
            if (loaded == null)
            {
                SetStatus("Load failed", new Color(1f, 0.74f, 0.74f, 1f));
                return;
            }

            CaptureUndoState();
            _document = loaded;
            _selectedId = "root";
            _collapsedTreeNodes.Clear();
            RebuildAll();
            SetStatus("Loaded " + path, new Color(0.74f, 0.93f, 1f, 1f));
        }

        private void Undo()
        {
            CommitFocusedInputField();

            if (_undoHistory.Count == 0)
            {
                SetStatus("Nothing to undo", new Color(0.96f, 0.84f, 0.72f, 1f));
                RefreshHistoryControls();
                return;
            }

            StudioHistoryState current = CreateHistoryState();
            StudioHistoryState target = _undoHistory[_undoHistory.Count - 1];
            _undoHistory.RemoveAt(_undoHistory.Count - 1);
            _redoHistory.Add(current);
            if (_redoHistory.Count > MaxHistoryStates)
            {
                _redoHistory.RemoveAt(0);
            }

            _applyingHistory = true;
            try
            {
                ApplyHistoryState(target);
            }
            finally
            {
                _applyingHistory = false;
            }

            SetStatus("Undo", new Color(0.86f, 0.93f, 1f, 1f));
            RefreshHistoryControls();
        }

        private void Redo()
        {
            CommitFocusedInputField();

            if (_redoHistory.Count == 0)
            {
                SetStatus("Nothing to redo", new Color(0.96f, 0.84f, 0.72f, 1f));
                RefreshHistoryControls();
                return;
            }

            StudioHistoryState current = CreateHistoryState();
            StudioHistoryState target = _redoHistory[_redoHistory.Count - 1];
            _redoHistory.RemoveAt(_redoHistory.Count - 1);
            _undoHistory.Add(current);
            if (_undoHistory.Count > MaxHistoryStates)
            {
                _undoHistory.RemoveAt(0);
            }

            _applyingHistory = true;
            try
            {
                ApplyHistoryState(target);
            }
            finally
            {
                _applyingHistory = false;
            }

            SetStatus("Redo", new Color(0.86f, 0.93f, 1f, 1f));
            RefreshHistoryControls();
        }

        private void CapturePropertyUndoState(string key)
        {
            if (IsColorPropertyKey(key) && _colorPickerRoot != null && _colorPickerRoot.activeSelf)
            {
                if (_colorPickerHistoryCaptured)
                {
                    return;
                }

                CaptureUndoState();
                _colorPickerHistoryCaptured = true;
                return;
            }

            CaptureUndoState();
        }

        private void CaptureUndoState()
        {
            if (_applyingHistory || _document == null)
            {
                return;
            }

            _undoHistory.Add(CreateHistoryState());
            if (_undoHistory.Count > MaxHistoryStates)
            {
                _undoHistory.RemoveAt(0);
            }

            _redoHistory.Clear();
            RefreshHistoryControls();
        }

        private void ApplyHistoryState(StudioHistoryState state)
        {
            if (state == null || state.Document == null)
            {
                return;
            }

            _document = CloneDocument(state.Document);
            _selectedId = string.IsNullOrWhiteSpace(state.SelectedId) ? "root" : state.SelectedId;
            _collapsedTreeNodes.Clear();
            if (state.CollapsedNodes != null)
            {
                for (int i = 0; i < state.CollapsedNodes.Count; i++)
                {
                    string nodeId = state.CollapsedNodes[i];
                    if (!string.IsNullOrWhiteSpace(nodeId))
                    {
                        _collapsedTreeNodes.Add(nodeId);
                    }
                }
            }

            if (XwmPropertyUtility.GetNodeById(_document, _selectedId) == null)
            {
                _selectedId = "root";
            }

            _previewTransformHistoryCaptured = false;
            _previewTransformNodeId = null;
            _colorPickerHistoryCaptured = false;
            RebuildAll();
        }

        private StudioHistoryState CreateHistoryState()
        {
            StudioHistoryState state = new StudioHistoryState
            {
                Document = CloneDocument(_document),
                SelectedId = _selectedId,
                CollapsedNodes = new List<string>(_collapsedTreeNodes)
            };
            return state;
        }

        private XwmDocumentData CloneDocument(XwmDocumentData source)
        {
            if (source == null)
            {
                return null;
            }

            XwmDocumentData clone = new XwmDocumentData
            {
                format = source.format,
                version = source.version,
                documentName = source.documentName,
                createdAtUtc = source.createdAtUtc,
                canvasSize = source.canvasSize,
                nextOrder = source.nextOrder,
                nodes = new List<XwmNodeData>()
            };

            if (source.nodes != null)
            {
                for (int i = 0; i < source.nodes.Count; i++)
                {
                    XwmNodeData node = source.nodes[i];
                    if (node == null)
                    {
                        continue;
                    }

                    XwmNodeData nodeCopy = new XwmNodeData
                    {
                        id = node.id,
                        parentId = node.parentId,
                        type = node.type,
                        name = node.name,
                        order = node.order,
                        layer = node.layer,
                        active = node.active,
                        properties = new List<XwmPropertyData>()
                    };

                    if (node.properties != null)
                    {
                        for (int p = 0; p < node.properties.Count; p++)
                        {
                            XwmPropertyData property = node.properties[p];
                            if (property == null)
                            {
                                continue;
                            }

                            nodeCopy.properties.Add(new XwmPropertyData
                            {
                                key = property.key,
                                value = property.value
                            });
                        }
                    }

                    clone.nodes.Add(nodeCopy);
                }
            }

            XwmPropertyUtility.EnsureDocument(clone);
            return clone;
        }

        private void RefreshHistoryControls()
        {
            if (_undoButton != null)
            {
                _undoButton.interactable = _undoHistory.Count > 0;
            }

            if (_redoButton != null)
            {
                _redoButton.interactable = _redoHistory.Count > 0;
            }
        }

        private void CommitFocusedInputField()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected == null)
            {
                return;
            }

            InputField input = selected.GetComponent<InputField>() ?? selected.GetComponentInParent<InputField>();
            if (input == null)
            {
                return;
            }

            string value = input.text ?? string.Empty;
            input.onEndEdit.Invoke(value);
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void SetStatus(string message, Color color)
        {
            if (_statusText == null)
            {
                return;
            }

            _statusText.text = message;
            _statusText.color = color;
        }

        private bool IsTypingInInputField()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected == null)
            {
                return false;
            }

            return selected.GetComponent<InputField>() != null || selected.GetComponentInParent<InputField>() != null;
        }
    }
}
