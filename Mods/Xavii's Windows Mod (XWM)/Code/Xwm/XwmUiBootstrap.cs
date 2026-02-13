using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmUiBootstrap
    {
        private static Font _font;
        private static readonly Dictionary<string, Font> FontCache = new Dictionary<string, Font>(StringComparer.OrdinalIgnoreCase);

        public static Font DefaultFont
        {
            get
            {
                if (_font == null)
                {
                    _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                }

                return _font;
            }
        }

        public static Sprite ResolveSprite(string primary, string fallback)
        {
            Sprite sprite = null;
            if (!string.IsNullOrWhiteSpace(primary))
            {
                sprite = Resources.Load<Sprite>(primary);
            }

            if (sprite == null && !string.IsNullOrWhiteSpace(fallback))
            {
                sprite = Resources.Load<Sprite>(fallback);
            }

            return sprite;
        }

        public static Font ResolveFont(string fontType, Font fallback = null)
        {
            Font resolvedFallback = fallback != null ? fallback : DefaultFont;
            if (string.IsNullOrWhiteSpace(fontType))
            {
                return resolvedFallback;
            }

            string key = fontType.Trim();
            if (string.Equals(key, "default", StringComparison.OrdinalIgnoreCase))
            {
                return resolvedFallback;
            }

            if (FontCache.TryGetValue(key, out Font cached))
            {
                return cached != null ? cached : resolvedFallback;
            }

            Font resolved = null;
            Font[] loadedFonts = Resources.FindObjectsOfTypeAll<Font>();
            for (int i = 0; i < loadedFonts.Length; i++)
            {
                Font candidate = loadedFonts[i];
                if (candidate == null || string.IsNullOrWhiteSpace(candidate.name))
                {
                    continue;
                }

                if (string.Equals(candidate.name, key, StringComparison.OrdinalIgnoreCase))
                {
                    resolved = candidate;
                    break;
                }
            }

            if (resolved == null)
            {
                resolved = Resources.Load<Font>(key);
            }

            if (resolved == null && (key.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) || key.EndsWith(".otf", StringComparison.OrdinalIgnoreCase)))
            {
                string withoutExtension = key.Substring(0, key.Length - 4);
                resolved = Resources.Load<Font>(withoutExtension);
            }

            if (resolved == null)
            {
                string builtinName = key;
                if (!builtinName.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) && !builtinName.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
                {
                    builtinName += ".ttf";
                }

                try
                {
                    resolved = Resources.GetBuiltinResource<Font>(builtinName);
                }
                catch
                {
                }
            }

            if (resolved == null)
            {
                try
                {
                    resolved = Font.CreateDynamicFontFromOSFont(key, 14);
                }
                catch
                {
                }
            }

            FontCache[key] = resolved;
            return resolved != null ? resolved : resolvedFallback;
        }

        public static List<string> GetAvailableFontTypes(string currentValue = null)
        {
            HashSet<string> unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<string> options = new List<string>();
            unique.Add("Default");
            options.Add("Default");

            Font[] loadedFonts = Resources.FindObjectsOfTypeAll<Font>();
            for (int i = 0; i < loadedFonts.Length; i++)
            {
                Font font = loadedFonts[i];
                if (font == null || string.IsNullOrWhiteSpace(font.name))
                {
                    continue;
                }

                if (unique.Add(font.name))
                {
                    options.Add(font.name);
                }
            }

            if (!string.IsNullOrWhiteSpace(currentValue))
            {
                string value = currentValue.Trim();
                if (unique.Add(value))
                {
                    options.Add(value);
                }
            }

            if (options.Count > 1)
            {
                List<string> sortable = options.GetRange(1, options.Count - 1);
                sortable.Sort(StringComparer.OrdinalIgnoreCase);
                options.RemoveRange(1, options.Count - 1);
                options.AddRange(sortable);
            }

            return options;
        }

        public static void EnsureEventSystem()
        {
            EventSystem system = UnityEngine.Object.FindObjectOfType<EventSystem>();
            if (system != null)
            {
                if (system.GetComponent<StandaloneInputModule>() == null)
                {
                    system.gameObject.AddComponent<StandaloneInputModule>();
                }

                return;
            }

            GameObject eventSystemObject = new GameObject("XWM_EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            UnityEngine.Object.DontDestroyOnLoad(eventSystemObject);
        }
    }
}
