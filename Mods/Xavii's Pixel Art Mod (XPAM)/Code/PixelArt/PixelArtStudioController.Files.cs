using System;
using System.IO;
using UnityEngine;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        [Serializable]
        private sealed class PixelProjectData
        {
            public string format;
            public string version;
            public string updatedAtUtc;
            public int width;
            public int height;
            public string pixels;
            public string exportName;
            public int brushSize;
            public string symmetry;
            public bool wrapShift;
            public string selectedColor;
        }

        private const int MaxProjectCanvasSide = 256;

        private void SaveProject()
        {
            try
            {
                string directory = PixelArtPathResolver.ResolveProjectsDirectory(true);
                string request = GetProjectRequestName();
                string safe = PixelArtPathResolver.SanitizeBaseName(request, "project");
                string fileName = PixelArtPathResolver.EnsureExtension(safe, ".xpam.json");
                string path = Path.Combine(directory, fileName);

                if (File.Exists(path))
                {
                    string stamped = safe + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xpam.json";
                    path = Path.Combine(directory, stamped);
                }

                PixelProjectData data = new PixelProjectData
                {
                    format = "xpam_project",
                    version = "2.1.0",
                    updatedAtUtc = DateTime.UtcNow.ToString("o"),
                    width = _width,
                    height = _height,
                    pixels = EncodePixelsToBase64(_pixels),
                    exportName = _fileNameInput != null ? _fileNameInput.text : "sprite",
                    brushSize = _brushSize,
                    symmetry = GetSymmetryName(_symmetryMode),
                    wrapShift = _wrapShift,
                    selectedColor = ToHexColor(_selectedColor)
                };

                string raw = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, raw);

                if (_projectNameInput != null)
                {
                    _projectNameInput.text = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Path.GetFileName(path)));
                }

                SetStatus("Project saved: " + Path.GetFileName(path), new Color(0.72f, 0.95f, 0.84f, 1f));
            }
            catch (Exception ex)
            {
                SetStatus("Project save failed: " + ex.Message, new Color(1f, 0.74f, 0.74f, 1f));
            }
        }

        private void LoadProject()
        {
            try
            {
                string directory = PixelArtPathResolver.ResolveProjectsDirectory(true);
                string request = GetProjectRequestName();
                string path = ResolveProjectLoadPath(directory, request);
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    SetStatus("Project not found", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                string raw = File.ReadAllText(path);
                PixelProjectData data = JsonUtility.FromJson<PixelProjectData>(raw);
                if (data == null)
                {
                    SetStatus("Project load failed", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                int width = Mathf.Clamp(data.width, 1, MaxProjectCanvasSide);
                int height = Mathf.Clamp(data.height, 1, MaxProjectCanvasSide);
                Color32[] pixels = DecodePixelsFromBase64(data.pixels, width * height);
                if (pixels == null || pixels.Length != width * height)
                {
                    SetStatus("Project pixels are invalid", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                CaptureUndoState();
                _width = width;
                _height = height;
                _pixels = pixels;
                _shapeDragActive = false;
                _shapeBasePixels = null;
                _strokeActive = false;
                _strokeCaptured = false;
                _leftPointerHeld = false;
                _lineStart = new Vector2Int(-1, -1);
                ClearSelection(false);
                RebuildGridCells();

                if (data.brushSize > 0)
                {
                    SetBrushSize(data.brushSize);
                }

                if (TryParseSymmetryMode(data.symmetry, out SymmetryMode loadedSymmetry))
                {
                    SetSymmetryMode(loadedSymmetry);
                }

                SetWrapShiftEnabled(data.wrapShift, false);

                if (TryParseHexColor(data.selectedColor, out Color32 loadedColor))
                {
                    SetSelectedColor(loadedColor);
                }

                if (_projectNameInput != null)
                {
                    _projectNameInput.text = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Path.GetFileName(path)));
                }

                if (_fileNameInput != null && !string.IsNullOrWhiteSpace(data.exportName))
                {
                    _fileNameInput.text = data.exportName;
                }

                SetStatus("Project loaded: " + Path.GetFileName(path), new Color(0.74f, 0.92f, 1f, 1f));
            }
            catch (Exception ex)
            {
                SetStatus("Project load failed: " + ex.Message, new Color(1f, 0.74f, 0.74f, 1f));
            }
        }

        private string ResolveProjectLoadPath(string directory, string request)
        {
            if (!Directory.Exists(directory))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(request))
            {
                string safe = PixelArtPathResolver.SanitizeBaseName(request, "project");
                string fileName = PixelArtPathResolver.EnsureExtension(safe, ".xpam.json");
                string requestedPath = Path.Combine(directory, fileName);
                if (File.Exists(requestedPath))
                {
                    return requestedPath;
                }
            }

            string[] files = Directory.GetFiles(directory, "*.xpam.json");
            if (files == null || files.Length == 0)
            {
                return null;
            }

            Array.Sort(files, (left, right) =>
            {
                DateTime leftWrite = File.GetLastWriteTimeUtc(left);
                DateTime rightWrite = File.GetLastWriteTimeUtc(right);
                return rightWrite.CompareTo(leftWrite);
            });
            return files[0];
        }

        private void ImportPng()
        {
            try
            {
                string directory = PixelArtPathResolver.ResolveImportsDirectory(true);
                string requested = _fileNameInput != null ? _fileNameInput.text : string.Empty;
                string path = ResolveImportPath(directory, requested);
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    SetStatus("PNG import failed: file not found", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                byte[] bytes = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Point;
                if (!texture.LoadImage(bytes, false))
                {
                    Destroy(texture);
                    SetStatus("PNG import failed: unsupported file", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                int sourceWidth = texture.width;
                int sourceHeight = texture.height;
                int targetWidth = sourceWidth;
                int targetHeight = sourceHeight;

                if (targetWidth > 128 || targetHeight > 128)
                {
                    float scale = Mathf.Max((float)targetWidth / 128f, (float)targetHeight / 128f);
                    targetWidth = Mathf.Max(1, Mathf.RoundToInt(targetWidth / scale));
                    targetHeight = Mathf.Max(1, Mathf.RoundToInt(targetHeight / scale));
                }

                Color32[] canvasPixels = BuildCanvasPixels(texture, targetWidth, targetHeight);
                Destroy(texture);

                if (canvasPixels == null || canvasPixels.Length != targetWidth * targetHeight)
                {
                    SetStatus("PNG import failed: decode error", new Color(1f, 0.74f, 0.74f, 1f));
                    return;
                }

                CaptureUndoState();
                _width = targetWidth;
                _height = targetHeight;
                _pixels = canvasPixels;
                _shapeDragActive = false;
                _shapeBasePixels = null;
                _strokeActive = false;
                _strokeCaptured = false;
                _leftPointerHeld = false;
                _lineStart = new Vector2Int(-1, -1);
                ClearSelection(false);
                RebuildGridCells();

                string baseName = Path.GetFileNameWithoutExtension(path);
                if (_fileNameInput != null)
                {
                    _fileNameInput.text = baseName;
                }

                if (_projectNameInput != null && string.IsNullOrWhiteSpace(_projectNameInput.text))
                {
                    _projectNameInput.text = baseName;
                }

                string message = sourceWidth == targetWidth && sourceHeight == targetHeight
                    ? "PNG imported: " + Path.GetFileName(path) + " (" + targetWidth + "x" + targetHeight + ")"
                    : "PNG imported: " + Path.GetFileName(path) + " (" + sourceWidth + "x" + sourceHeight + " -> " + targetWidth + "x" + targetHeight + ")";
                SetStatus(message, new Color(0.74f, 0.92f, 1f, 1f));
            }
            catch (Exception ex)
            {
                SetStatus("PNG import failed: " + ex.Message, new Color(1f, 0.74f, 0.74f, 1f));
            }
        }

        private string ResolveImportPath(string directory, string request)
        {
            if (!Directory.Exists(directory))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(request))
            {
                string safe = PixelArtPathResolver.SanitizeBaseName(request, "sprite");
                string fileName = PixelArtPathResolver.EnsureExtension(safe, ".png");
                string requestedPath = Path.Combine(directory, fileName);
                if (File.Exists(requestedPath))
                {
                    return requestedPath;
                }
            }

            string[] files = Directory.GetFiles(directory, "*.png");
            if (files == null || files.Length == 0)
            {
                return null;
            }

            Array.Sort(files, (left, right) =>
            {
                DateTime leftWrite = File.GetLastWriteTimeUtc(left);
                DateTime rightWrite = File.GetLastWriteTimeUtc(right);
                return rightWrite.CompareTo(leftWrite);
            });
            return files[0];
        }

        private string GetProjectRequestName()
        {
            if (_projectNameInput != null && !string.IsNullOrWhiteSpace(_projectNameInput.text))
            {
                return _projectNameInput.text;
            }

            if (_fileNameInput != null && !string.IsNullOrWhiteSpace(_fileNameInput.text))
            {
                return _fileNameInput.text;
            }

            return "project";
        }

        private static string EncodePixelsToBase64(Color32[] pixels)
        {
            if (pixels == null || pixels.Length == 0)
            {
                return string.Empty;
            }

            byte[] data = new byte[pixels.Length * 4];
            for (int i = 0; i < pixels.Length; i++)
            {
                int offset = i * 4;
                Color32 color = pixels[i];
                data[offset] = color.r;
                data[offset + 1] = color.g;
                data[offset + 2] = color.b;
                data[offset + 3] = color.a;
            }

            return Convert.ToBase64String(data);
        }

        private static Color32[] DecodePixelsFromBase64(string raw, int expectedPixelCount)
        {
            if (expectedPixelCount <= 0)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            byte[] data;
            try
            {
                data = Convert.FromBase64String(raw);
            }
            catch
            {
                return null;
            }

            if (data == null || data.Length < expectedPixelCount * 4)
            {
                return null;
            }

            Color32[] pixels = new Color32[expectedPixelCount];
            for (int i = 0; i < expectedPixelCount; i++)
            {
                int offset = i * 4;
                pixels[i] = new Color32(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]);
            }

            return pixels;
        }
    }
}
