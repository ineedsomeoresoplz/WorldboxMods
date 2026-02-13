using System;
using System.IO;
using UnityEngine;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        private void CaptureUndoState()
        {
            _undoHistory.Add(CreateCanvasState());
            if (_undoHistory.Count > MaxHistory)
            {
                _undoHistory.RemoveAt(0);
            }

            _redoHistory.Clear();
        }

        private CanvasState CreateCanvasState()
        {
            CanvasState state = new CanvasState
            {
                Width = _width,
                Height = _height,
                Pixels = ClonePixels(_pixels)
            };
            return state;
        }

        private Color32[] ClonePixels(Color32[] source)
        {
            if (source == null)
            {
                return null;
            }

            Color32[] clone = new Color32[source.Length];
            Array.Copy(source, clone, source.Length);
            return clone;
        }

        private void Undo()
        {
            if (_undoHistory.Count == 0)
            {
                SetStatus("Nothing to undo", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _redoHistory.Add(CreateCanvasState());
            if (_redoHistory.Count > MaxHistory)
            {
                _redoHistory.RemoveAt(0);
            }

            CanvasState target = _undoHistory[_undoHistory.Count - 1];
            _undoHistory.RemoveAt(_undoHistory.Count - 1);
            ApplyCanvasState(target);
            SetStatus("Undo", new Color(0.8f, 0.91f, 1f, 1f));
        }

        private void Redo()
        {
            if (_redoHistory.Count == 0)
            {
                SetStatus("Nothing to redo", new Color(0.95f, 0.84f, 0.72f, 1f));
                return;
            }

            _undoHistory.Add(CreateCanvasState());
            if (_undoHistory.Count > MaxHistory)
            {
                _undoHistory.RemoveAt(0);
            }

            CanvasState target = _redoHistory[_redoHistory.Count - 1];
            _redoHistory.RemoveAt(_redoHistory.Count - 1);
            ApplyCanvasState(target);
            SetStatus("Redo", new Color(0.8f, 0.91f, 1f, 1f));
        }

        private void ApplyCanvasState(CanvasState state)
        {
            if (state == null || state.Pixels == null)
            {
                return;
            }

            bool sizeChanged = _width != state.Width || _height != state.Height;
            _width = Mathf.Max(1, state.Width);
            _height = Mathf.Max(1, state.Height);
            _pixels = ClonePixels(state.Pixels);
            if (_pixels == null || _pixels.Length != _width * _height)
            {
                _pixels = new Color32[_width * _height];
            }

            _shapeDragActive = false;
            _shapeBasePixels = null;
            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
            _selectionPreviewActive = false;
            _selectionDragActive = false;
            _moveDragActive = false;
            _moveDragCaptured = false;
            _moveDragLastCell = new Vector2Int(-1, -1);
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            if (sizeChanged || _cellImages.Count != _width * _height)
            {
                RebuildGridCells();
            }
            else
            {
                RefreshAllCells();
            }
        }

        private void NewCanvas(int size)
        {
            size = Mathf.Clamp(size, 8, 128);
            CaptureUndoState();
            _width = size;
            _height = size;
            _pixels = new Color32[_width * _height];
            _shapeDragActive = false;
            _shapeBasePixels = null;
            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RebuildGridCells();
            SetStatus("New canvas: " + size + "x" + size, new Color(0.72f, 0.93f, 0.86f, 1f));
        }

        private void ClearCanvas()
        {
            CaptureUndoState();
            for (int i = 0; i < _pixels.Length; i++)
            {
                _pixels[i] = new Color32(0, 0, 0, 0);
            }

            _shapeDragActive = false;
            _shapeBasePixels = null;
            _strokeActive = false;
            _strokeCaptured = false;
            _leftPointerHeld = false;
            _lineStart = new Vector2Int(-1, -1);
            ClearSelection(false);
            RefreshAllCells();
            SetStatus("Canvas cleared", new Color(0.95f, 0.8f, 0.72f, 1f));
        }

        private void ToggleGrid()
        {
            _showGrid = !_showGrid;
            if (_gridButtonLabel != null)
            {
                _gridButtonLabel.text = _showGrid ? "Grid On" : "Grid Off";
            }

            UpdateGridLayoutSizing();
            SetStatus(_showGrid ? "Grid enabled" : "Grid disabled", new Color(0.74f, 0.91f, 1f, 1f));
        }

        private void ExportPng()
        {
            try
            {
                string directory = PixelArtPathResolver.ResolveExportsDirectory(true);
                string requested = _fileNameInput != null ? _fileNameInput.text : "sprite";
                string safeName = PixelArtPathResolver.SanitizeFileName(requested);
                string targetPath = Path.Combine(directory, safeName);

                if (File.Exists(targetPath))
                {
                    string stamped = Path.GetFileNameWithoutExtension(safeName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                    targetPath = Path.Combine(directory, stamped);
                }

                Texture2D texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Point;
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        texture.SetPixel(x, _height - 1 - y, GetPixel(x, y));
                    }
                }

                texture.Apply(false, false);
                byte[] bytes = texture.EncodeToPNG();
                Destroy(texture);
                File.WriteAllBytes(targetPath, bytes);

                string fileName = Path.GetFileName(targetPath);
                SetStatus("Exported: " + fileName, new Color(0.72f, 0.95f, 0.84f, 1f));
            }
            catch (Exception ex)
            {
                SetStatus("Export failed: " + ex.Message, new Color(1f, 0.74f, 0.74f, 1f));
            }
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
    }
}
