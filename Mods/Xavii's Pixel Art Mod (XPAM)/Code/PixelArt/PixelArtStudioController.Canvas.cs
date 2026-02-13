using UnityEngine;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        private void RebuildGridCells()
        {
            if (_gridRect == null || _gridLayout == null)
            {
                return;
            }

            for (int i = _gridRect.childCount - 1; i >= 0; i--)
            {
                Destroy(_gridRect.GetChild(i).gameObject);
            }

            _cellImages.Clear();
            _gridLayout.constraintCount = _width;

            if (_pixels == null || _pixels.Length != _width * _height)
            {
                _pixels = new Color32[_width * _height];
            }

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    GameObject cellObject = new GameObject("Cell_" + x + "_" + y, typeof(RectTransform), typeof(Image), typeof(PixelCellView));
                    RectTransform cellRect = cellObject.GetComponent<RectTransform>();
                    cellRect.SetParent(_gridRect, false);

                    Image image = cellObject.GetComponent<Image>();
                    image.raycastTarget = true;
                    _cellImages.Add(image);

                    PixelCellView view = cellObject.GetComponent<PixelCellView>();
                    view.Controller = this;
                    view.X = x;
                    view.Y = y;
                }
            }

            _selectionPreviewActive = false;
            _selectionDragActive = false;
            _moveDragActive = false;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(-1, -1);
            ClearSelection(false);
            RefreshAllCells();
            UpdateGridLayoutSizing();
        }

        private void UpdateGridLayoutSizing()
        {
            RectTransform layoutHost = _gridHostRect != null ? _gridHostRect : _drawAreaRect;
            if (layoutHost == null || _gridLayout == null || _gridRect == null)
            {
                return;
            }

            float availableWidth = Mathf.Max(64f, layoutHost.rect.width - 18f);
            float availableHeight = Mathf.Max(64f, layoutHost.rect.height - 18f);
            float side = Mathf.Min(availableWidth, availableHeight);

            float spacing = _showGrid ? 1f : 0f;
            float cell = Mathf.Floor((side - spacing * (_width - 1)) / _width);
            cell = Mathf.Clamp(cell, 2f, 40f);

            float contentWidth = cell * _width + spacing * (_width - 1);
            float contentHeight = cell * _height + spacing * (_height - 1);
            _gridRect.sizeDelta = new Vector2(contentWidth, contentHeight);
            _gridLayout.cellSize = new Vector2(cell, cell);
            _gridLayout.spacing = new Vector2(spacing, spacing);
        }

        private void RefreshAllCells()
        {
            int count = Mathf.Min(_cellImages.Count, _width * _height);
            for (int i = 0; i < count; i++)
            {
                int x = i % _width;
                int y = i / _width;
                Color32 source = _pixels[i];
                _cellImages[i].color = GetDisplayColor(x, y, source);
            }
        }

        private void RefreshCell(int x, int y)
        {
            int index = GetIndex(x, y);
            if (index < 0 || index >= _cellImages.Count)
            {
                return;
            }

            Color32 source = _pixels[index];
            _cellImages[index].color = GetDisplayColor(x, y, source);
        }

        private Color GetDisplayColor(int x, int y, Color32 source)
        {
            Color baseColor = source.a == 0 ? (((x + y) & 1) == 0 ? (Color)_checkerA : (Color)_checkerB) : (Color)source;
            return ApplySelectionOverlayColor(x, y, baseColor);
        }

        private Color ApplySelectionOverlayColor(int x, int y, Color baseColor)
        {
            if (!TryGetSelectionOverlayRect(out RectInt rect))
            {
                return baseColor;
            }

            if (!IsPointOnSelectionBorder(x, y, rect))
            {
                return baseColor;
            }

            Color overlay = ((x + y) & 1) == 0
                ? new Color(1f, 0.93f, 0.32f, 1f)
                : new Color(0.08f, 0.18f, 0.31f, 1f);
            return Color.Lerp(baseColor, overlay, 0.72f);
        }

        private bool TryGetSelectionOverlayRect(out RectInt rect)
        {
            if (_selectionPreviewActive && _selectionPreviewRect.width > 0 && _selectionPreviewRect.height > 0)
            {
                rect = ClampRectToCanvas(_selectionPreviewRect);
                return rect.width > 0 && rect.height > 0;
            }

            if (_selectionActive && _selectionRect.width > 0 && _selectionRect.height > 0)
            {
                rect = ClampRectToCanvas(_selectionRect);
                return rect.width > 0 && rect.height > 0;
            }

            rect = new RectInt(0, 0, 0, 0);
            return false;
        }

        private static bool IsPointOnSelectionBorder(int x, int y, RectInt rect)
        {
            if (x < rect.xMin || y < rect.yMin || x >= rect.xMax || y >= rect.yMax)
            {
                return false;
            }

            return x == rect.xMin || x == rect.xMax - 1 || y == rect.yMin || y == rect.yMax - 1;
        }

        private void SetTool(ToolMode tool)
        {
            if (_shapeDragActive)
            {
                RestoreShapeBasePixels();
                _shapeDragActive = false;
                _shapeBasePixels = null;
            }

            _selectionPreviewActive = false;
            _selectionDragActive = false;
            _moveDragActive = false;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(-1, -1);
            _leftPointerHeld = false;
            _strokeActive = false;
            _strokeCaptured = false;

            _tool = tool;
            if (!IsShapeTool(_tool))
            {
                _lineStart = new Vector2Int(-1, -1);
            }

            RefreshToolButtonStates();
            UpdateToolText();
            RefreshAllCells();
            SetStatus("Tool: " + GetToolName(_tool), new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void SetBrushSize(int size)
        {
            _brushSize = Mathf.Clamp(size, 1, 8);
            UpdateToolText();
            SetStatus("Brush size: " + _brushSize, new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void SetSymmetryMode(SymmetryMode mode)
        {
            _symmetryMode = mode;
            UpdateToolText();
            SetStatus("Symmetry: " + GetSymmetryName(mode), new Color(0.74f, 0.91f, 1f, 1f));
        }

        private bool TryParseSymmetryMode(string raw, out SymmetryMode mode)
        {
            mode = SymmetryMode.None;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            string normalized = raw.Trim();
            if (string.Equals(normalized, "none", System.StringComparison.OrdinalIgnoreCase))
            {
                mode = SymmetryMode.None;
                return true;
            }

            if (string.Equals(normalized, "vertical", System.StringComparison.OrdinalIgnoreCase))
            {
                mode = SymmetryMode.Vertical;
                return true;
            }

            if (string.Equals(normalized, "horizontal", System.StringComparison.OrdinalIgnoreCase))
            {
                mode = SymmetryMode.Horizontal;
                return true;
            }

            if (string.Equals(normalized, "quadrant", System.StringComparison.OrdinalIgnoreCase))
            {
                mode = SymmetryMode.Quadrant;
                return true;
            }

            return false;
        }

        private void SetWrapShiftEnabled(bool enabled, bool announce)
        {
            _wrapShift = enabled;
            if (_wrapShiftButtonLabel != null)
            {
                _wrapShiftButtonLabel.text = _wrapShift ? "Wrap On" : "Wrap Off";
            }

            if (announce)
            {
                SetStatus(_wrapShift ? "Shift wrap enabled" : "Shift wrap disabled", new Color(0.74f, 0.91f, 1f, 1f));
            }
        }

        private void ToggleWrapShift()
        {
            SetWrapShiftEnabled(!_wrapShift, true);
        }

        private void RefreshToolButtonStates()
        {
            foreach (System.Collections.Generic.KeyValuePair<ToolMode, Image> pair in _toolButtonImages)
            {
                if (pair.Value == null)
                {
                    continue;
                }

                bool selected = pair.Key == _tool;
                pair.Value.color = selected ? new Color(0.25f, 0.65f, 0.43f, 1f) : new Color(0.23f, 0.36f, 0.56f, 1f);
            }
        }

        private void UpdateToolText()
        {
            if (_toolText == null)
            {
                return;
            }

            _toolText.text = "Tool: " + GetToolName(_tool) + "   Brush: " + _brushSize + "   Sym: " + GetSymmetryName(_symmetryMode);
        }

        private string GetToolName(ToolMode mode)
        {
            switch (mode)
            {
                case ToolMode.Pencil:
                    return "Pencil";
                case ToolMode.Eraser:
                    return "Eraser";
                case ToolMode.Fill:
                    return "Fill";
                case ToolMode.Line:
                    return "Line";
                case ToolMode.Rectangle:
                    return "Rectangle";
                case ToolMode.FilledRectangle:
                    return "Fill Rect";
                case ToolMode.Circle:
                    return "Circle";
                case ToolMode.FilledCircle:
                    return "Fill Circle";
                case ToolMode.Picker:
                    return "Picker";
                case ToolMode.Replace:
                    return "Replace";
                case ToolMode.Select:
                    return "Select";
                case ToolMode.Move:
                    return "Move";
                default:
                    return "Pencil";
            }
        }

        private string GetSymmetryName(SymmetryMode mode)
        {
            switch (mode)
            {
                case SymmetryMode.None:
                    return "None";
                case SymmetryMode.Vertical:
                    return "Vertical";
                case SymmetryMode.Horizontal:
                    return "Horizontal";
                case SymmetryMode.Quadrant:
                    return "Quadrant";
                default:
                    return "None";
            }
        }

        private void BeginStroke()
        {
            if (_strokeCaptured)
            {
                return;
            }

            CaptureUndoState();
            _strokeCaptured = true;
        }

        private void DrawBrushAt(int centerX, int centerY, Color32 color)
        {
            int radius = Mathf.Max(0, _brushSize - 1);
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    SetPixelWithSymmetry(centerX + x, centerY + y, color);
                }
            }
        }

        private bool IsShapeTool(ToolMode tool)
        {
            return tool == ToolMode.Line
                || tool == ToolMode.Rectangle
                || tool == ToolMode.FilledRectangle
                || tool == ToolMode.Circle
                || tool == ToolMode.FilledCircle;
        }

        private void BeginShapeDrag(int x, int y)
        {
            if (!IsWithinCanvas(x, y))
            {
                return;
            }

            if (_shapeBasePixels == null || _shapeBasePixels.Length != _pixels.Length)
            {
                _shapeBasePixels = ClonePixels(_pixels);
            }
            else
            {
                System.Array.Copy(_pixels, _shapeBasePixels, _pixels.Length);
            }

            CaptureUndoState();
            _shapeDragActive = true;
            _shapeStart = new Vector2Int(x, y);
            _shapeCurrent = _shapeStart;
            ApplyShapeToCanvas(_shapeStart, _shapeCurrent, _selectedColor);
            SetStatus("Shape start: " + (x + 1) + "," + (y + 1), new Color(0.95f, 0.88f, 0.72f, 1f));
        }

        private void UpdateShapeDrag(int x, int y)
        {
            if (!_shapeDragActive || !IsWithinCanvas(x, y))
            {
                return;
            }

            if (_shapeCurrent.x == x && _shapeCurrent.y == y)
            {
                return;
            }

            _shapeCurrent = new Vector2Int(x, y);
            RestoreShapeBasePixels();
            ApplyShapeToCanvas(_shapeStart, _shapeCurrent, _selectedColor);
        }

        private void FinalizeShapeDrag()
        {
            if (!_shapeDragActive)
            {
                return;
            }

            _shapeDragActive = false;
            _shapeBasePixels = null;
            _lineStart = new Vector2Int(-1, -1);
            SetStatus("Shape applied", new Color(0.72f, 0.94f, 0.84f, 1f));
        }

        private void RestoreShapeBasePixels()
        {
            if (_shapeBasePixels == null || _pixels == null || _shapeBasePixels.Length != _pixels.Length)
            {
                return;
            }

            System.Array.Copy(_shapeBasePixels, _pixels, _shapeBasePixels.Length);
            RefreshAllCells();
        }

        private void ApplyShapeToCanvas(Vector2Int start, Vector2Int end, Color32 color)
        {
            if (_tool == ToolMode.Line)
            {
                DrawLine(start.x, start.y, end.x, end.y, color);
            }
            else if (_tool == ToolMode.Rectangle)
            {
                DrawRectangle(start.x, start.y, end.x, end.y, color);
            }
            else if (_tool == ToolMode.FilledRectangle)
            {
                DrawFilledRectangle(start.x, start.y, end.x, end.y, color);
            }
            else if (_tool == ToolMode.Circle)
            {
                DrawCircle(start.x, start.y, end.x, end.y, color);
            }
            else if (_tool == ToolMode.FilledCircle)
            {
                DrawFilledCircle(start.x, start.y, end.x, end.y, color);
            }
        }

        private void DrawLine(int x0, int y0, int x1, int y1, Color32 color)
        {
            int dx = Mathf.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int error = dx + dy;

            while (true)
            {
                DrawBrushAt(x0, y0, color);
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int error2 = error * 2;
                if (error2 >= dy)
                {
                    error += dy;
                    x0 += sx;
                }

                if (error2 <= dx)
                {
                    error += dx;
                    y0 += sy;
                }
            }
        }

        private void DrawRectangle(int x0, int y0, int x1, int y1, Color32 color)
        {
            int minX = Mathf.Min(x0, x1);
            int maxX = Mathf.Max(x0, x1);
            int minY = Mathf.Min(y0, y1);
            int maxY = Mathf.Max(y0, y1);

            for (int x = minX; x <= maxX; x++)
            {
                DrawBrushAt(x, minY, color);
                DrawBrushAt(x, maxY, color);
            }

            for (int y = minY; y <= maxY; y++)
            {
                DrawBrushAt(minX, y, color);
                DrawBrushAt(maxX, y, color);
            }
        }

        private void DrawFilledRectangle(int x0, int y0, int x1, int y1, Color32 color)
        {
            int minX = Mathf.Min(x0, x1);
            int maxX = Mathf.Max(x0, x1);
            int minY = Mathf.Min(y0, y1);
            int maxY = Mathf.Max(y0, y1);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    DrawBrushAt(x, y, color);
                }
            }
        }

        private void DrawCircle(int centerX, int centerY, int edgeX, int edgeY, Color32 color)
        {
            int radius = Mathf.RoundToInt(Vector2Int.Distance(new Vector2Int(centerX, centerY), new Vector2Int(edgeX, edgeY)));
            if (radius <= 0)
            {
                DrawBrushAt(centerX, centerY, color);
                return;
            }

            int x = radius;
            int y = 0;
            int decision = 1 - radius;

            while (x >= y)
            {
                PlotCirclePoints(centerX, centerY, x, y, color);
                y++;
                if (decision < 0)
                {
                    decision += 2 * y + 1;
                }
                else
                {
                    x--;
                    decision += 2 * (y - x) + 1;
                }
            }
        }

        private void DrawFilledCircle(int centerX, int centerY, int edgeX, int edgeY, Color32 color)
        {
            int radius = Mathf.RoundToInt(Vector2Int.Distance(new Vector2Int(centerX, centerY), new Vector2Int(edgeX, edgeY)));
            if (radius <= 0)
            {
                DrawBrushAt(centerX, centerY, color);
                return;
            }

            int radiusSquared = radius * radius;
            for (int y = -radius; y <= radius; y++)
            {
                int xExtent = Mathf.FloorToInt(Mathf.Sqrt(Mathf.Max(0, radiusSquared - y * y)));
                for (int x = -xExtent; x <= xExtent; x++)
                {
                    DrawBrushAt(centerX + x, centerY + y, color);
                }
            }
        }

        private void PlotCirclePoints(int centerX, int centerY, int x, int y, Color32 color)
        {
            DrawBrushAt(centerX + x, centerY + y, color);
            DrawBrushAt(centerX + y, centerY + x, color);
            DrawBrushAt(centerX - y, centerY + x, color);
            DrawBrushAt(centerX - x, centerY + y, color);
            DrawBrushAt(centerX - x, centerY - y, color);
            DrawBrushAt(centerX - y, centerY - x, color);
            DrawBrushAt(centerX + y, centerY - x, color);
            DrawBrushAt(centerX + x, centerY - y, color);
        }

        private void BeginSelectionDrag(int x, int y)
        {
            if (!IsWithinCanvas(x, y))
            {
                return;
            }

            _selectionDragActive = true;
            _selectionDragStart = new Vector2Int(x, y);
            _selectionPreviewActive = true;
            _selectionPreviewRect = BuildRectFromPoints(x, y, x, y);
            RefreshAllCells();
            SetStatus("Selection start: " + (x + 1) + "," + (y + 1), new Color(0.9f, 0.9f, 0.72f, 1f));
        }

        private void UpdateSelectionDrag(int x, int y)
        {
            if (!_selectionDragActive || !IsWithinCanvas(x, y))
            {
                return;
            }

            RectInt next = BuildRectFromPoints(_selectionDragStart.x, _selectionDragStart.y, x, y);
            if (next.x == _selectionPreviewRect.x
                && next.y == _selectionPreviewRect.y
                && next.width == _selectionPreviewRect.width
                && next.height == _selectionPreviewRect.height)
            {
                return;
            }

            _selectionPreviewRect = next;
            RefreshAllCells();
        }

        private void FinalizeSelectionDrag()
        {
            if (!_selectionDragActive && !_selectionPreviewActive)
            {
                return;
            }

            _selectionDragActive = false;
            _selectionDragStart = new Vector2Int(-1, -1);

            RectInt target = _selectionPreviewActive ? ClampRectToCanvas(_selectionPreviewRect) : new RectInt(0, 0, 0, 0);
            _selectionPreviewActive = false;

            if (target.width <= 0 || target.height <= 0)
            {
                _selectionActive = false;
                _selectionRect = new RectInt(0, 0, 0, 0);
                RefreshAllCells();
                SetStatus("Selection cleared", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _selectionActive = true;
            _selectionRect = target;
            RefreshAllCells();
            SetStatus("Selection: " + target.width + "x" + target.height, new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void ClearSelection(bool announce)
        {
            bool hadSelection = _selectionActive || _selectionPreviewActive || _selectionDragActive;
            _selectionActive = false;
            _selectionPreviewActive = false;
            _selectionDragActive = false;
            _selectionRect = new RectInt(0, 0, 0, 0);
            _selectionPreviewRect = new RectInt(0, 0, 0, 0);
            _selectionDragStart = new Vector2Int(-1, -1);
            _moveDragActive = false;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(-1, -1);

            if (hadSelection)
            {
                RefreshAllCells();
            }

            if (announce)
            {
                SetStatus("Selection cleared", new Color(0.95f, 0.84f, 0.72f, 1f));
            }
        }

        private void SelectAll()
        {
            _selectionPreviewActive = false;
            _selectionDragActive = false;
            _selectionActive = true;
            _selectionRect = new RectInt(0, 0, _width, _height);
            RefreshAllCells();
            SetStatus("Selection: " + _width + "x" + _height, new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void CopySelection()
        {
            if (!TryGetSelectionRect(out RectInt rect))
            {
                SetStatus("No selection to copy", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _clipboard = CreateClipboard(rect);
            SetStatus("Copied " + rect.width + "x" + rect.height, new Color(0.72f, 0.94f, 0.84f, 1f));
        }

        private void CutSelection()
        {
            if (!TryGetSelectionRect(out RectInt rect))
            {
                SetStatus("No selection to cut", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _clipboard = CreateClipboard(rect);
            CaptureUndoState();
            ClearRectPixels(rect);
            RefreshAllCells();
            SetStatus("Cut " + rect.width + "x" + rect.height, new Color(0.95f, 0.86f, 0.72f, 1f));
        }

        private void PasteSelection()
        {
            if (_clipboard == null || _clipboard.Pixels == null || _clipboard.Width <= 0 || _clipboard.Height <= 0)
            {
                SetStatus("Clipboard is empty", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            int targetX = _selectionActive ? _selectionRect.x : 0;
            int targetY = _selectionActive ? _selectionRect.y : 0;
            CaptureUndoState();
            BlitPixels(_clipboard.Pixels, _clipboard.Width, _clipboard.Height, targetX, targetY);

            RectInt pasted = ClampRectToCanvas(new RectInt(targetX, targetY, _clipboard.Width, _clipboard.Height));
            _selectionPreviewActive = false;
            _selectionDragActive = false;
            if (pasted.width > 0 && pasted.height > 0)
            {
                _selectionActive = true;
                _selectionRect = pasted;
            }
            else
            {
                _selectionActive = false;
                _selectionRect = new RectInt(0, 0, 0, 0);
            }

            RefreshAllCells();
            SetStatus("Pasted " + _clipboard.Width + "x" + _clipboard.Height, new Color(0.72f, 0.94f, 0.84f, 1f));
        }

        private void DeleteSelection()
        {
            if (!TryGetSelectionRect(out RectInt rect))
            {
                return;
            }

            CaptureUndoState();
            ClearRectPixels(rect);
            RefreshAllCells();
            SetStatus("Selection deleted", new Color(0.95f, 0.84f, 0.72f, 1f));
        }

        private void NudgeSelectionFromKeyboard(int deltaX, int deltaY, bool duplicate)
        {
            if (!TryGetSelectionRect(out _))
            {
                return;
            }

            MoveSelectionByDelta(deltaX, deltaY, duplicate, true, false);
        }

        private void BeginMoveDrag(int x, int y)
        {
            if (!TryGetSelectionRect(out RectInt rect))
            {
                SetStatus("No selection to move", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            if (!IsPointInsideRect(x, y, rect))
            {
                SetStatus("Click inside selection to move", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _moveDragActive = true;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(x, y);
            SetStatus("Move drag active", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void UpdateMoveDrag(int x, int y)
        {
            if (!_moveDragActive || !IsWithinCanvas(x, y))
            {
                return;
            }

            int deltaX = x - _moveDragLastCell.x;
            int deltaY = y - _moveDragLastCell.y;
            if (deltaX == 0 && deltaY == 0)
            {
                return;
            }

            bool moved = MoveSelectionByDelta(deltaX, deltaY, false, !_moveDragCaptured, true);
            if (moved)
            {
                _moveDragCaptured = true;
                _moveDragLastCell = new Vector2Int(x, y);
            }
        }

        private void FinalizeMoveDrag()
        {
            if (!_moveDragActive && !_moveDragCaptured)
            {
                return;
            }

            _moveDragActive = false;
            _moveDragLastCell = new Vector2Int(-1, -1);
            if (_moveDragCaptured)
            {
                SetStatus("Selection moved", new Color(0.72f, 0.94f, 0.84f, 1f));
            }

            _moveDragCaptured = false;
        }

        private bool MoveSelectionByDelta(int deltaX, int deltaY, bool duplicate, bool captureUndo, bool fromDrag)
        {
            if (deltaX == 0 && deltaY == 0)
            {
                return false;
            }

            if (!TryGetSelectionRect(out RectInt source))
            {
                return false;
            }

            int maxX = Mathf.Max(0, _width - source.width);
            int maxY = Mathf.Max(0, _height - source.height);
            int targetX = Mathf.Clamp(source.x + deltaX, 0, maxX);
            int targetY = Mathf.Clamp(source.y + deltaY, 0, maxY);
            if (targetX == source.x && targetY == source.y)
            {
                return false;
            }

            Color32[] block = ExtractRectPixels(source);
            if (captureUndo)
            {
                CaptureUndoState();
            }

            if (!duplicate)
            {
                ClearRectPixels(source);
            }

            BlitPixels(block, source.width, source.height, targetX, targetY);
            _selectionActive = true;
            _selectionPreviewActive = false;
            _selectionRect = new RectInt(targetX, targetY, source.width, source.height);
            RefreshAllCells();

            if (!fromDrag)
            {
                string label = duplicate ? "Selection duplicated" : "Selection moved";
                SetStatus(label + " to " + (targetX + 1) + "," + (targetY + 1), new Color(0.72f, 0.94f, 0.84f, 1f));
            }

            return true;
        }

        private ClipboardState CreateClipboard(RectInt rect)
        {
            ClipboardState state = new ClipboardState
            {
                Width = rect.width,
                Height = rect.height,
                Pixels = ExtractRectPixels(rect)
            };
            return state;
        }

        private Color32[] ExtractRectPixels(RectInt rect)
        {
            RectInt clamped = ClampRectToCanvas(rect);
            Color32[] block = new Color32[clamped.width * clamped.height];
            for (int y = 0; y < clamped.height; y++)
            {
                int sourceRow = (clamped.y + y) * _width;
                int targetRow = y * clamped.width;
                for (int x = 0; x < clamped.width; x++)
                {
                    block[targetRow + x] = _pixels[sourceRow + clamped.x + x];
                }
            }

            return block;
        }

        private void ClearRectPixels(RectInt rect)
        {
            RectInt clamped = ClampRectToCanvas(rect);
            Color32 transparent = new Color32(0, 0, 0, 0);
            for (int y = 0; y < clamped.height; y++)
            {
                int row = (clamped.y + y) * _width;
                for (int x = 0; x < clamped.width; x++)
                {
                    _pixels[row + clamped.x + x] = transparent;
                }
            }
        }

        private void BlitPixels(Color32[] source, int sourceWidth, int sourceHeight, int targetX, int targetY)
        {
            if (source == null || sourceWidth <= 0 || sourceHeight <= 0)
            {
                return;
            }

            for (int y = 0; y < sourceHeight; y++)
            {
                int destinationY = targetY + y;
                if (destinationY < 0 || destinationY >= _height)
                {
                    continue;
                }

                int sourceRow = y * sourceWidth;
                int destinationRow = destinationY * _width;
                for (int x = 0; x < sourceWidth; x++)
                {
                    int destinationX = targetX + x;
                    if (destinationX < 0 || destinationX >= _width)
                    {
                        continue;
                    }

                    _pixels[destinationRow + destinationX] = source[sourceRow + x];
                }
            }
        }

        private bool TryGetSelectionRect(out RectInt rect)
        {
            if (!_selectionActive || _selectionRect.width <= 0 || _selectionRect.height <= 0)
            {
                rect = new RectInt(0, 0, 0, 0);
                return false;
            }

            RectInt clamped = ClampRectToCanvas(_selectionRect);
            if (clamped.width <= 0 || clamped.height <= 0)
            {
                _selectionActive = false;
                _selectionRect = new RectInt(0, 0, 0, 0);
                rect = new RectInt(0, 0, 0, 0);
                return false;
            }

            _selectionRect = clamped;
            rect = clamped;
            return true;
        }

        private static bool IsPointInsideRect(int x, int y, RectInt rect)
        {
            return x >= rect.xMin && y >= rect.yMin && x < rect.xMax && y < rect.yMax;
        }

        private RectInt BuildRectFromPoints(int x0, int y0, int x1, int y1)
        {
            int minX = Mathf.Clamp(Mathf.Min(x0, x1), 0, _width - 1);
            int maxX = Mathf.Clamp(Mathf.Max(x0, x1), 0, _width - 1);
            int minY = Mathf.Clamp(Mathf.Min(y0, y1), 0, _height - 1);
            int maxY = Mathf.Clamp(Mathf.Max(y0, y1), 0, _height - 1);
            return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        private RectInt ClampRectToCanvas(RectInt rect)
        {
            int xMin = Mathf.Clamp(rect.xMin, 0, _width);
            int xMax = Mathf.Clamp(rect.xMax, 0, _width);
            int yMin = Mathf.Clamp(rect.yMin, 0, _height);
            int yMax = Mathf.Clamp(rect.yMax, 0, _height);
            return new RectInt(xMin, yMin, Mathf.Max(0, xMax - xMin), Mathf.Max(0, yMax - yMin));
        }

        private void FloodFillWithSymmetry(int x, int y, Color32 to)
        {
            System.Collections.Generic.HashSet<int> seeds = new System.Collections.Generic.HashSet<int>();
            ForEachSymmetryPoint(x, y, (sx, sy) =>
            {
                if (!IsWithinCanvas(sx, sy))
                {
                    return;
                }

                int seedIndex = GetIndex(sx, sy);
                if (!seeds.Add(seedIndex))
                {
                    return;
                }

                Color32 from = GetPixel(sx, sy);
                if (ColorsEqual(from, to))
                {
                    return;
                }

                FloodFill(sx, sy, from, to);
            });
        }

        private void FloodFill(int startX, int startY, Color32 from, Color32 to)
        {
            if (ColorsEqual(from, to))
            {
                return;
            }

            System.Collections.Generic.Queue<int> queue = new System.Collections.Generic.Queue<int>();
            int startIndex = GetIndex(startX, startY);
            if (startIndex < 0)
            {
                return;
            }

            queue.Enqueue(startIndex);
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                if (index < 0 || index >= _pixels.Length)
                {
                    continue;
                }

                if (!ColorsEqual(_pixels[index], from))
                {
                    continue;
                }

                _pixels[index] = to;
                int x = index % _width;
                int y = index / _width;
                RefreshCell(x, y);

                if (x > 0) queue.Enqueue(index - 1);
                if (x < _width - 1) queue.Enqueue(index + 1);
                if (y > 0) queue.Enqueue(index - _width);
                if (y < _height - 1) queue.Enqueue(index + _width);
            }
        }

        private void ReplaceColor(Color32 from, Color32 to)
        {
            if (ColorsEqual(from, to))
            {
                return;
            }

            bool changed = false;
            for (int i = 0; i < _pixels.Length; i++)
            {
                if (ColorsEqual(_pixels[i], from))
                {
                    _pixels[i] = to;
                    changed = true;
                }
            }

            if (changed)
            {
                RefreshAllCells();
            }
        }

        private void SetPixelWithSymmetry(int x, int y, Color32 color)
        {
            ForEachSymmetryPoint(x, y, (sx, sy) => SetPixel(sx, sy, color));
        }

        private void ForEachSymmetryPoint(int x, int y, System.Action<int, int> apply)
        {
            int mirrorX = _width - 1 - x;
            int mirrorY = _height - 1 - y;

            apply(x, y);

            if (_symmetryMode == SymmetryMode.Vertical || _symmetryMode == SymmetryMode.Quadrant)
            {
                if (mirrorX != x)
                {
                    apply(mirrorX, y);
                }
            }

            if (_symmetryMode == SymmetryMode.Horizontal || _symmetryMode == SymmetryMode.Quadrant)
            {
                if (mirrorY != y)
                {
                    apply(x, mirrorY);
                }
            }

            if (_symmetryMode == SymmetryMode.Quadrant)
            {
                if (mirrorX != x || mirrorY != y)
                {
                    apply(mirrorX, mirrorY);
                }
            }
        }

        private void SetPixel(int x, int y, Color32 color)
        {
            if (!IsWithinCanvas(x, y))
            {
                return;
            }

            int index = GetIndex(x, y);
            if (index < 0 || ColorsEqual(_pixels[index], color))
            {
                return;
            }

            _pixels[index] = color;
            RefreshCell(x, y);
        }

        private Color32 GetPixel(int x, int y)
        {
            int index = GetIndex(x, y);
            if (index < 0 || index >= _pixels.Length)
            {
                return new Color32(0, 0, 0, 0);
            }

            return _pixels[index];
        }

        private bool IsWithinCanvas(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        private int GetIndex(int x, int y)
        {
            return IsWithinCanvas(x, y) ? y * _width + x : -1;
        }

        private static bool ColorsEqual(Color32 a, Color32 b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        private void PickColorAt(int x, int y)
        {
            SetSelectedColor(GetPixel(x, y));
            SetStatus("Color picked", new Color(0.74f, 0.92f, 1f, 1f));
        }

        private void SetSelectedColor(Color32 color)
        {
            _selectedColor = color;
            UpdateColorUi();
        }

        private void OnColorSliderChanged(float value)
        {
            if (_suppressColorCallbacks)
            {
                return;
            }

            byte r = (byte)Mathf.Clamp(Mathf.RoundToInt(_redSlider != null ? _redSlider.value : _selectedColor.r), 0, 255);
            byte g = (byte)Mathf.Clamp(Mathf.RoundToInt(_greenSlider != null ? _greenSlider.value : _selectedColor.g), 0, 255);
            byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(_blueSlider != null ? _blueSlider.value : _selectedColor.b), 0, 255);
            byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(_alphaSlider != null ? _alphaSlider.value : _selectedColor.a), 0, 255);
            _selectedColor = new Color32(r, g, b, a);
            UpdateColorUi();
        }

        private void UpdateColorUi()
        {
            _suppressColorCallbacks = true;

            if (_redSlider != null) _redSlider.value = _selectedColor.r;
            if (_greenSlider != null) _greenSlider.value = _selectedColor.g;
            if (_blueSlider != null) _blueSlider.value = _selectedColor.b;
            if (_alphaSlider != null) _alphaSlider.value = _selectedColor.a;

            if (_redValueText != null) _redValueText.text = _selectedColor.r.ToString();
            if (_greenValueText != null) _greenValueText.text = _selectedColor.g.ToString();
            if (_blueValueText != null) _blueValueText.text = _selectedColor.b.ToString();
            if (_alphaValueText != null) _alphaValueText.text = _selectedColor.a.ToString();

            if (_colorPreviewImage != null)
            {
                _colorPreviewImage.color = _selectedColor;
            }

            RefreshCustomPaletteUi();
            _suppressColorCallbacks = false;
        }

        private void FlipHorizontal()
        {
            CaptureUndoState();
            Color32[] next = new Color32[_width * _height];

            for (int y = 0; y < _height; y++)
            {
                int row = y * _width;
                for (int x = 0; x < _width; x++)
                {
                    int sourceIndex = row + x;
                    int targetIndex = row + (_width - 1 - x);
                    next[targetIndex] = _pixels[sourceIndex];
                }
            }

            _pixels = next;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RefreshAllCells();
            SetStatus("Flipped horizontally", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void FlipVertical()
        {
            CaptureUndoState();
            Color32[] next = new Color32[_width * _height];

            for (int y = 0; y < _height; y++)
            {
                int sourceRow = y * _width;
                int targetRow = (_height - 1 - y) * _width;
                for (int x = 0; x < _width; x++)
                {
                    next[targetRow + x] = _pixels[sourceRow + x];
                }
            }

            _pixels = next;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RefreshAllCells();
            SetStatus("Flipped vertically", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void RotateClockwise()
        {
            CaptureUndoState();
            int newWidth = _height;
            int newHeight = _width;
            Color32[] next = new Color32[newWidth * newHeight];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int newX = newWidth - 1 - y;
                    int newY = x;
                    next[newY * newWidth + newX] = _pixels[y * _width + x];
                }
            }

            _width = newWidth;
            _height = newHeight;
            _pixels = next;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RebuildGridCells();
            SetStatus("Rotated clockwise", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void RotateCounterClockwise()
        {
            CaptureUndoState();
            int newWidth = _height;
            int newHeight = _width;
            Color32[] next = new Color32[newWidth * newHeight];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int newX = y;
                    int newY = newHeight - 1 - x;
                    next[newY * newWidth + newX] = _pixels[y * _width + x];
                }
            }

            _width = newWidth;
            _height = newHeight;
            _pixels = next;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RebuildGridCells();
            SetStatus("Rotated counter-clockwise", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void ShiftLeft()
        {
            ShiftCanvas(-1, 0);
        }

        private void ShiftRight()
        {
            ShiftCanvas(1, 0);
        }

        private void ShiftUp()
        {
            ShiftCanvas(0, -1);
        }

        private void ShiftDown()
        {
            ShiftCanvas(0, 1);
        }

        private void ShiftCanvas(int deltaX, int deltaY)
        {
            CaptureUndoState();
            Color32[] next = new Color32[_width * _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int sourceX = x - deltaX;
                    int sourceY = y - deltaY;

                    if (_wrapShift)
                    {
                        sourceX = ((sourceX % _width) + _width) % _width;
                        sourceY = ((sourceY % _height) + _height) % _height;
                    }

                    if (sourceX < 0 || sourceY < 0 || sourceX >= _width || sourceY >= _height)
                    {
                        next[y * _width + x] = new Color32(0, 0, 0, 0);
                    }
                    else
                    {
                        next[y * _width + x] = _pixels[sourceY * _width + sourceX];
                    }
                }
            }

            _pixels = next;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RefreshAllCells();
            SetStatus("Canvas shifted", new Color(0.74f, 0.91f, 1f, 1f));
        }
    }
}
