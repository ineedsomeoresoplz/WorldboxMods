using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal partial class PixelArtStudioController
    {
        [Serializable]
        private sealed class CustomPaletteFile
        {
            public string[] slots;
        }

        private const int CustomPaletteSlotCount = 16;

        private readonly Color32[] _customPalette = new Color32[CustomPaletteSlotCount];
        private readonly System.Collections.Generic.List<Image> _customPaletteImages = new System.Collections.Generic.List<Image>();

        private void InitializeCustomPaletteDefaults()
        {
            Color32[] defaults = new Color32[]
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
                new Color32(128, 128, 128, 255),
                new Color32(32, 32, 32, 255),
                new Color32(182, 214, 255, 255),
                new Color32(40, 70, 120, 255),
                new Color32(0, 0, 0, 0)
            };

            for (int i = 0; i < CustomPaletteSlotCount; i++)
            {
                _customPalette[i] = defaults[i];
            }
        }

        private void BuildCustomPaletteSlots(RectTransform parent, float top)
        {
            _customPaletteImages.Clear();
            CreateLabel(parent, "CustomPaletteLabel", "Custom Palette (L=Use, R=Save)", 11, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(8f, -(top + 16f)), new Vector2(-8f, -top), new Color(0.72f, 0.84f, 0.98f, 1f));

            for (int i = 0; i < CustomPaletteSlotCount; i++)
            {
                int index = i;
                int row = i / 8;
                int column = i % 8;
                float left = 8f + column * 26f;
                float slotTop = top + 18f + row * 26f;

                GameObject slotObject = new GameObject("CustomPalette_" + index, typeof(RectTransform), typeof(Image), typeof(PaletteSwatchView));
                RectTransform rect = slotObject.GetComponent<RectTransform>();
                rect.SetParent(parent, false);
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(0f, 1f);
                rect.offsetMin = new Vector2(left, -(slotTop + 22f));
                rect.offsetMax = new Vector2(left + 22f, -slotTop);

                Image image = slotObject.GetComponent<Image>();
                image.color = _customPalette[index];
                image.raycastTarget = true;

                PaletteSwatchView swatch = slotObject.GetComponent<PaletteSwatchView>();
                swatch.Controller = this;
                swatch.SlotIndex = index;

                _customPaletteImages.Add(image);
            }

            RefreshCustomPaletteUi();
        }

        internal void HandleCustomPaletteSlotUse(int index)
        {
            if (index < 0 || index >= _customPalette.Length)
            {
                return;
            }

            SetSelectedColor(_customPalette[index]);
            SetStatus("Custom palette color selected", new Color(0.74f, 0.91f, 1f, 1f));
        }

        internal void HandleCustomPaletteSlotStore(int index)
        {
            if (index < 0 || index >= _customPalette.Length)
            {
                return;
            }

            _customPalette[index] = _selectedColor;
            RefreshCustomPaletteUi();
            SaveCustomPalette();
            SetStatus("Custom palette color saved", new Color(0.72f, 0.94f, 0.84f, 1f));
        }

        private void RefreshCustomPaletteUi()
        {
            int count = Mathf.Min(_customPaletteImages.Count, _customPalette.Length);
            for (int i = 0; i < count; i++)
            {
                if (_customPaletteImages[i] == null)
                {
                    continue;
                }

                _customPaletteImages[i].color = _customPalette[i];
            }
        }

        private void LoadCustomPalette()
        {
            string path = Path.Combine(PixelArtPathResolver.ResolvePaletteDirectory(false), "xpam_custom_palette.json");
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                string raw = File.ReadAllText(path);
                CustomPaletteFile file = JsonUtility.FromJson<CustomPaletteFile>(raw);
                if (file == null || file.slots == null)
                {
                    return;
                }

                int count = Mathf.Min(file.slots.Length, _customPalette.Length);
                for (int i = 0; i < count; i++)
                {
                    if (TryParseHexColor(file.slots[i], out Color32 parsed))
                    {
                        _customPalette[i] = parsed;
                    }
                }
            }
            catch
            {
            }
        }

        private void SaveCustomPalette()
        {
            try
            {
                string directory = PixelArtPathResolver.ResolvePaletteDirectory(true);
                string path = Path.Combine(directory, "xpam_custom_palette.json");
                CustomPaletteFile file = new CustomPaletteFile
                {
                    slots = new string[_customPalette.Length]
                };

                for (int i = 0; i < _customPalette.Length; i++)
                {
                    file.slots[i] = ToHexColor(_customPalette[i]);
                }

                string raw = JsonUtility.ToJson(file, true);
                File.WriteAllText(path, raw);
            }
            catch
            {
            }
        }

        private static string ToHexColor(Color32 color)
        {
            return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        }

        private static bool TryParseHexColor(string value, out Color32 color)
        {
            color = new Color32(0, 0, 0, 255);
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string text = value.Trim();
            if (text.StartsWith("#", StringComparison.Ordinal))
            {
                text = text.Substring(1);
            }

            if (text.Length == 6)
            {
                text += "FF";
            }

            if (text.Length != 8)
            {
                return false;
            }

            if (!byte.TryParse(text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out byte r)) return false;
            if (!byte.TryParse(text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out byte g)) return false;
            if (!byte.TryParse(text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b)) return false;
            if (!byte.TryParse(text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null, out byte a)) return false;

            color = new Color32(r, g, b, a);
            return true;
        }
    }
}
