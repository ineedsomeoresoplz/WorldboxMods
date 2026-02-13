using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController : MonoBehaviour
    {
        private enum ToolMode
        {
            Pencil,
            Eraser,
            Fill,
            Line,
            Rectangle,
            FilledRectangle,
            Circle,
            FilledCircle,
            Picker,
            Replace,
            Select,
            Move
        }

        private enum SymmetryMode
        {
            None,
            Vertical,
            Horizontal,
            Quadrant
        }

        private sealed class CanvasState
        {
            public int Width;
            public int Height;
            public Color32[] Pixels;
        }

        private sealed class ClipboardState
        {
            public int Width;
            public int Height;
            public Color32[] Pixels;
        }

        private const int MaxHistory = 80;

        private GameObject _studioRoot;
        private RectTransform _drawAreaRect;
        private RectTransform _toolbarRect;
        private RectTransform _gridRect;
        private GridLayoutGroup _gridLayout;
        private Text _statusText;
        private Text _toolText;
        private Text _gridButtonLabel;
        private Text _wrapShiftButtonLabel;
        private InputField _fileNameInput;
        private InputField _projectNameInput;
        private Image _colorPreviewImage;
        private Slider _redSlider;
        private Slider _greenSlider;
        private Slider _blueSlider;
        private Slider _alphaSlider;
        private Text _redValueText;
        private Text _greenValueText;
        private Text _blueValueText;
        private Text _alphaValueText;

        private readonly Dictionary<ToolMode, Image> _toolButtonImages = new Dictionary<ToolMode, Image>();
        private readonly List<Image> _cellImages = new List<Image>();
        private readonly List<CanvasState> _undoHistory = new List<CanvasState>();
        private readonly List<CanvasState> _redoHistory = new List<CanvasState>();

        private int _width = 32;
        private int _height = 32;
        private Color32[] _pixels;
        private ToolMode _tool = ToolMode.Pencil;
        private int _brushSize = 1;
        private bool _strokeActive;
        private bool _strokeCaptured;
        private bool _showGrid = true;
        private bool _suppressColorCallbacks;
        private Vector2 _lastDrawAreaSize;
        private Vector2Int _lineStart = new Vector2Int(-1, -1);
        private Color32 _selectedColor = new Color32(255, 255, 255, 255);
        private SymmetryMode _symmetryMode = SymmetryMode.None;
        private bool _wrapShift;
        private bool _leftPointerHeld;
        private bool _shapeDragActive;
        private Vector2Int _shapeStart = new Vector2Int(-1, -1);
        private Vector2Int _shapeCurrent = new Vector2Int(-1, -1);
        private Color32[] _shapeBasePixels;
        private bool _selectionActive;
        private RectInt _selectionRect;
        private bool _selectionPreviewActive;
        private RectInt _selectionPreviewRect;
        private bool _selectionDragActive;
        private Vector2Int _selectionDragStart = new Vector2Int(-1, -1);
        private bool _moveDragActive;
        private Vector2Int _moveDragLastCell = new Vector2Int(-1, -1);
        private bool _moveDragCaptured;
        private ClipboardState _clipboard;

        private readonly Color32 _checkerA = new Color32(64, 64, 72, 255);
        private readonly Color32 _checkerB = new Color32(84, 84, 94, 255);

        internal bool IsOpen => _studioRoot != null && _studioRoot.activeSelf;

        private void Awake()
        {
            _pixels = new Color32[_width * _height];
            InitializeCustomPaletteDefaults();
            LoadCustomPalette();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                ToggleStudio();
                return;
            }

            if (!IsOpen)
            {
                return;
            }

            if (_leftPointerHeld && !Input.GetMouseButton(0))
            {
                HandlePointerReleasedOutsideCell();
            }

            if (_drawAreaRect != null)
            {
                Vector2 size = _drawAreaRect.rect.size;
                if (Vector2.SqrMagnitude(size - _lastDrawAreaSize) > 0.25f)
                {
                    _lastDrawAreaSize = size;
                    UpdateGridLayoutSizing();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideStudio();
                return;
            }

            if (IsTypingInInputField())
            {
                return;
            }

            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (control && Input.GetKeyDown(KeyCode.Z)) Undo();
            if (control && Input.GetKeyDown(KeyCode.Y)) Redo();
            if (control && Input.GetKeyDown(KeyCode.S))
            {
                if (shift)
                {
                    SaveProject();
                }
                else
                {
                    ExportPng();
                }
            }

            if (control && Input.GetKeyDown(KeyCode.I)) ImportPng();
            if (control && Input.GetKeyDown(KeyCode.O)) LoadProject();
            if (control && Input.GetKeyDown(KeyCode.A)) SelectAll();
            if (control && Input.GetKeyDown(KeyCode.C)) CopySelection();
            if (control && Input.GetKeyDown(KeyCode.X)) CutSelection();
            if (control && Input.GetKeyDown(KeyCode.V)) PasteSelection();

            if (!control)
            {
                if (Input.GetKeyDown(KeyCode.B)) SetTool(ToolMode.Pencil);
                if (Input.GetKeyDown(KeyCode.E)) SetTool(ToolMode.Eraser);
                if (Input.GetKeyDown(KeyCode.G)) SetTool(ToolMode.Fill);
                if (Input.GetKeyDown(KeyCode.L)) SetTool(ToolMode.Line);
                if (Input.GetKeyDown(KeyCode.R)) SetTool(ToolMode.Rectangle);
                if (Input.GetKeyDown(KeyCode.F)) SetTool(ToolMode.FilledRectangle);
                if (Input.GetKeyDown(KeyCode.C)) SetTool(ToolMode.Circle);
                if (Input.GetKeyDown(KeyCode.V)) SetTool(ToolMode.FilledCircle);
                if (Input.GetKeyDown(KeyCode.I)) SetTool(ToolMode.Picker);
                if (Input.GetKeyDown(KeyCode.H)) SetTool(ToolMode.Replace);
                if (Input.GetKeyDown(KeyCode.Q)) SetTool(ToolMode.Select);
                if (Input.GetKeyDown(KeyCode.M)) SetTool(ToolMode.Move);
                if (Input.GetKeyDown(KeyCode.LeftBracket)) SetBrushSize(_brushSize - 1);
                if (Input.GetKeyDown(KeyCode.RightBracket)) SetBrushSize(_brushSize + 1);
                if (Input.GetKeyDown(KeyCode.Alpha1)) SetSymmetryMode(SymmetryMode.None);
                if (Input.GetKeyDown(KeyCode.Alpha2)) SetSymmetryMode(SymmetryMode.Vertical);
                if (Input.GetKeyDown(KeyCode.Alpha3)) SetSymmetryMode(SymmetryMode.Horizontal);
                if (Input.GetKeyDown(KeyCode.Alpha4)) SetSymmetryMode(SymmetryMode.Quadrant);
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSelection();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) NudgeSelectionFromKeyboard(-1, 0, shift);
            if (Input.GetKeyDown(KeyCode.RightArrow)) NudgeSelectionFromKeyboard(1, 0, shift);
            if (Input.GetKeyDown(KeyCode.UpArrow)) NudgeSelectionFromKeyboard(0, -1, shift);
            if (Input.GetKeyDown(KeyCode.DownArrow)) NudgeSelectionFromKeyboard(0, 1, shift);

            TickPresetUi();
        }

        internal void HandleCellPointerDown(int x, int y, PointerEventData eventData)
        {
            if (!IsOpen || !IsWithinCanvas(x, y))
            {
                return;
            }

            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                PickColorAt(x, y);
                return;
            }

            if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            _leftPointerHeld = true;

            if (_tool == ToolMode.Select)
            {
                BeginSelectionDrag(x, y);
                return;
            }

            if (_tool == ToolMode.Move)
            {
                BeginMoveDrag(x, y);
                return;
            }

            if (IsShapeTool(_tool))
            {
                BeginShapeDrag(x, y);
                return;
            }

            if (_tool == ToolMode.Fill)
            {
                CaptureUndoState();
                FloodFillWithSymmetry(x, y, _selectedColor);
                SetStatus("Fill applied", new Color(0.72f, 0.92f, 1f, 1f));
                _leftPointerHeld = false;
                return;
            }

            if (_tool == ToolMode.Picker)
            {
                PickColorAt(x, y);
                _leftPointerHeld = false;
                return;
            }

            if (_tool == ToolMode.Replace)
            {
                Color32 from = GetPixel(x, y);
                if (ColorsEqual(from, _selectedColor))
                {
                    _leftPointerHeld = false;
                    return;
                }

                CaptureUndoState();
                ReplaceColor(from, _selectedColor);
                SetStatus("Color replaced", new Color(0.72f, 0.94f, 0.84f, 1f));
                _leftPointerHeld = false;
                return;
            }

            BeginStroke();
            DrawBrushAt(x, y, _tool == ToolMode.Eraser ? new Color32(0, 0, 0, 0) : _selectedColor);
            _strokeActive = true;
        }

        internal void HandleCellPointerEnter(int x, int y, PointerEventData eventData)
        {
            if (!IsOpen || !IsWithinCanvas(x, y))
            {
                return;
            }

            if (!_leftPointerHeld || !Input.GetMouseButton(0))
            {
                return;
            }

            if (_tool == ToolMode.Pencil || _tool == ToolMode.Eraser)
            {
                if (_strokeActive)
                {
                    DrawBrushAt(x, y, _tool == ToolMode.Eraser ? new Color32(0, 0, 0, 0) : _selectedColor);
                }
                return;
            }

            if (IsShapeTool(_tool))
            {
                UpdateShapeDrag(x, y);
                return;
            }

            if (_tool == ToolMode.Select)
            {
                UpdateSelectionDrag(x, y);
                return;
            }

            if (_tool == ToolMode.Move)
            {
                UpdateMoveDrag(x, y);
            }
        }

        internal void HandleCellPointerUp(int x, int y, PointerEventData eventData)
        {
            if (!IsOpen)
            {
                return;
            }

            if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (IsWithinCanvas(x, y))
            {
                if (IsShapeTool(_tool))
                {
                    UpdateShapeDrag(x, y);
                    FinalizeShapeDrag();
                }
                else if (_tool == ToolMode.Select)
                {
                    UpdateSelectionDrag(x, y);
                    FinalizeSelectionDrag();
                }
                else if (_tool == ToolMode.Move)
                {
                    UpdateMoveDrag(x, y);
                    FinalizeMoveDrag();
                }
            }
            else
            {
                if (IsShapeTool(_tool))
                {
                    FinalizeShapeDrag();
                }
                else if (_tool == ToolMode.Select)
                {
                    FinalizeSelectionDrag();
                }
                else if (_tool == ToolMode.Move)
                {
                    FinalizeMoveDrag();
                }
            }

            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
        }

        private void HandlePointerReleasedOutsideCell()
        {
            if (IsShapeTool(_tool))
            {
                FinalizeShapeDrag();
            }
            else if (_tool == ToolMode.Select)
            {
                FinalizeSelectionDrag();
            }
            else if (_tool == ToolMode.Move)
            {
                FinalizeMoveDrag();
            }

            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
        }

        private void ToggleStudio()
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

        private void ShowStudio()
        {
            EnsureUi();
            if (_studioRoot == null)
            {
                return;
            }

            _studioRoot.SetActive(true);
            _lineStart = new Vector2Int(-1, -1);
            RefreshToolButtonStates();
            UpdateToolText();
            UpdateGridLayoutSizing();
            EnsureVanillaPresetsLoaded();
            SetStatus("XPAM open", new Color(0.74f, 0.92f, 1f, 1f));
        }

        private void HideStudio()
        {
            if (_shapeDragActive)
            {
                RestoreShapeBasePixels();
            }

            if (_studioRoot != null)
            {
                _studioRoot.SetActive(false);
            }

            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
            _lineStart = new Vector2Int(-1, -1);
            _shapeDragActive = false;
            _shapeBasePixels = null;
            _selectionDragActive = false;
            _selectionPreviewActive = false;
            _moveDragActive = false;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(-1, -1);
            RefreshAllCells();
        }

        private bool IsTypingInInputField()
        {
            EventSystem current = EventSystem.current;
            if (current == null)
            {
                return false;
            }

            GameObject selected = current.currentSelectedGameObject;
            if (selected == null)
            {
                return false;
            }

            return selected.GetComponent<InputField>() != null || selected.GetComponentInParent<InputField>() != null;
        }
    }
}
