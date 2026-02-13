using System;
using System.Collections.Generic;
using UnityEngine;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmPropertyUtility
    {
        private static readonly Dictionary<string, List<string>> EditableKeyMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            [XwmTypeLibrary.Frame] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "color", "backgroundTransparency", "sprite" },
            [XwmTypeLibrary.ScrollingFrame] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "color", "backgroundTransparency", "sprite", "scrollHorizontal", "scrollVertical", "scrollSensitivity" },
            [XwmTypeLibrary.TextLabel] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "text", "fontType", "textScaled", "textWrapped", "fontSize", "textColor", "textTransparency", "color", "backgroundTransparency", "sprite", "alignment", "raycastTarget" },
            [XwmTypeLibrary.ImageLabel] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "color", "backgroundTransparency", "imageTransparency", "sprite", "raycastTarget" },
            [XwmTypeLibrary.TextButton] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "text", "fontType", "textScaled", "textWrapped", "fontSize", "textColor", "textTransparency", "color", "backgroundTransparency", "sprite", "interactable" },
            [XwmTypeLibrary.TextBox] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "text", "placeholder", "fontType", "textScaled", "textWrapped", "fontSize", "textColor", "textTransparency", "placeholderColor", "color", "backgroundTransparency", "interactable" },
            [XwmTypeLibrary.ImageButton] = new List<string> { "name", "parentId", "layer", "active", "x", "y", "width", "height", "anchorMinX", "anchorMinY", "anchorMaxX", "anchorMaxY", "pivotX", "pivotY", "rotation", "scaleX", "scaleY", "color", "backgroundTransparency", "imageTransparency", "sprite", "interactable" },
            [XwmTypeLibrary.UICorner] = new List<string> { "name", "parentId", "layer", "active", "radius" },
            [XwmTypeLibrary.UIScale] = new List<string> { "name", "parentId", "layer", "active", "scale" },
            [XwmTypeLibrary.UIListLayout] = new List<string> { "name", "parentId", "layer", "active", "orientation", "spacing", "childControlWidth", "childControlHeight", "childForceExpandWidth", "childForceExpandHeight" },
            [XwmTypeLibrary.UIDragDetector] = new List<string> { "name", "parentId", "layer", "active", "enabled" },
            [XwmTypeLibrary.UIGradient] = new List<string> { "name", "parentId", "layer", "active", "topColor", "bottomColor", "angle" },
            [XwmTypeLibrary.UIGridLayout] = new List<string> { "name", "parentId", "layer", "active", "cellWidth", "cellHeight", "spacingX", "spacingY", "constraint", "constraintCount" },
            [XwmTypeLibrary.UIPadding] = new List<string> { "name", "parentId", "layer", "active", "left", "right", "top", "bottom" },
            [XwmTypeLibrary.UIPageLayout] = new List<string> { "name", "parentId", "layer", "active", "spacing", "axis", "page" },
            [XwmTypeLibrary.UITableLayout] = new List<string> { "name", "parentId", "layer", "active", "columns", "cellWidth", "cellHeight", "spacingX", "spacingY" }
        };

        public static List<XwmPropertyData> CreateDefaultProperties(string type)
        {
            string normalized = XwmTypeLibrary.Normalize(type);
            List<XwmPropertyData> properties = new List<XwmPropertyData>();

            if (string.Equals(normalized, XwmTypeLibrary.Frame, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "width", "320");
                SetProperty(properties, "height", "160");
                SetProperty(properties, "color", "#2A3447D9");
                SetProperty(properties, "backgroundTransparency", "0.15");
                SetProperty(properties, "sprite", "ui/icons/windowInnerSliced");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.ScrollingFrame, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "width", "320");
                SetProperty(properties, "height", "180");
                SetProperty(properties, "color", "#2A3447D9");
                SetProperty(properties, "backgroundTransparency", "0.15");
                SetProperty(properties, "sprite", "ui/icons/windowInnerSliced");
                SetProperty(properties, "scrollHorizontal", "false");
                SetProperty(properties, "scrollVertical", "true");
                SetProperty(properties, "scrollSensitivity", "18");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.TextButton, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "text", "Button");
                SetProperty(properties, "fontType", "Default");
                SetProperty(properties, "textScaled", "false");
                SetProperty(properties, "textWrapped", "true");
                SetProperty(properties, "fontSize", "14");
                SetProperty(properties, "textColor", "#F4F9FFFF");
                SetProperty(properties, "sprite", "ui/icons/backgroundTabButton");
                SetProperty(properties, "color", "#4D79A7FF");
                SetProperty(properties, "backgroundTransparency", "0");
                SetProperty(properties, "textTransparency", "0");
                SetProperty(properties, "interactable", "true");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.TextLabel, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "text", "Label");
                SetProperty(properties, "fontType", "Default");
                SetProperty(properties, "textScaled", "false");
                SetProperty(properties, "textWrapped", "true");
                SetProperty(properties, "fontSize", "14");
                SetProperty(properties, "textColor", "#F4F9FFFF");
                SetProperty(properties, "sprite", "ui/icons/backgroundTabButton");
                SetProperty(properties, "color", "#4D79A7FF");
                SetProperty(properties, "backgroundTransparency", "0");
                SetProperty(properties, "textTransparency", "0");
                SetProperty(properties, "alignment", "MiddleCenter");
                SetProperty(properties, "raycastTarget", "false");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.ImageLabel, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "width", "220");
                SetProperty(properties, "height", "120");
                SetProperty(properties, "sprite", "ui/icons/iconPortrait");
                SetProperty(properties, "color", "#FFFFFFFF");
                SetProperty(properties, "backgroundTransparency", "0");
                SetProperty(properties, "imageTransparency", "0");
                SetProperty(properties, "raycastTarget", "false");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.TextBox, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "width", "260");
                SetProperty(properties, "height", "42");
                SetProperty(properties, "text", string.Empty);
                SetProperty(properties, "placeholder", "Type here");
                SetProperty(properties, "fontType", "Default");
                SetProperty(properties, "textScaled", "false");
                SetProperty(properties, "textWrapped", "false");
                SetProperty(properties, "fontSize", "14");
                SetProperty(properties, "textColor", "#F4F9FFFF");
                SetProperty(properties, "textTransparency", "0");
                SetProperty(properties, "placeholderColor", "#91A8B9CC");
                SetProperty(properties, "color", "#1E2733FF");
                SetProperty(properties, "backgroundTransparency", "0");
                SetProperty(properties, "interactable", "true");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.ImageButton, StringComparison.OrdinalIgnoreCase))
            {
                AddTransformDefaults(properties);
                SetProperty(properties, "width", "56");
                SetProperty(properties, "height", "56");
                SetProperty(properties, "sprite", "ui/icons/iconPortrait");
                SetProperty(properties, "color", "#FFFFFFFF");
                SetProperty(properties, "backgroundTransparency", "0");
                SetProperty(properties, "imageTransparency", "0");
                SetProperty(properties, "interactable", "true");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UICorner, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "radius", "8");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIScale, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "scale", "1");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIListLayout, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "orientation", "Vertical");
                SetProperty(properties, "spacing", "8");
                SetProperty(properties, "childControlWidth", "true");
                SetProperty(properties, "childControlHeight", "false");
                SetProperty(properties, "childForceExpandWidth", "false");
                SetProperty(properties, "childForceExpandHeight", "false");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIDragDetector, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "enabled", "true");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIGradient, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "topColor", "#5DA4E5FF");
                SetProperty(properties, "bottomColor", "#1A3150FF");
                SetProperty(properties, "angle", "90");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIGridLayout, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "cellWidth", "96");
                SetProperty(properties, "cellHeight", "48");
                SetProperty(properties, "spacingX", "8");
                SetProperty(properties, "spacingY", "8");
                SetProperty(properties, "constraint", "FixedColumnCount");
                SetProperty(properties, "constraintCount", "3");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIPadding, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "left", "8");
                SetProperty(properties, "right", "8");
                SetProperty(properties, "top", "8");
                SetProperty(properties, "bottom", "8");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UIPageLayout, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "spacing", "4");
                SetProperty(properties, "axis", "Horizontal");
                SetProperty(properties, "page", "0");
                return properties;
            }

            if (string.Equals(normalized, XwmTypeLibrary.UITableLayout, StringComparison.OrdinalIgnoreCase))
            {
                SetProperty(properties, "columns", "3");
                SetProperty(properties, "cellWidth", "120");
                SetProperty(properties, "cellHeight", "40");
                SetProperty(properties, "spacingX", "6");
                SetProperty(properties, "spacingY", "6");
                return properties;
            }

            AddTransformDefaults(properties);
            return properties;
        }

        private static void AddTransformDefaults(List<XwmPropertyData> properties)
        {
            if (properties == null)
            {
                return;
            }

            SetProperty(properties, "x", "20");
            SetProperty(properties, "y", "20");
            SetProperty(properties, "width", "220");
            SetProperty(properties, "height", "64");
            SetProperty(properties, "anchorMinX", "0");
            SetProperty(properties, "anchorMinY", "1");
            SetProperty(properties, "anchorMaxX", "0");
            SetProperty(properties, "anchorMaxY", "1");
            SetProperty(properties, "pivotX", "0");
            SetProperty(properties, "pivotY", "1");
            SetProperty(properties, "rotation", "0");
            SetProperty(properties, "scaleX", "1");
            SetProperty(properties, "scaleY", "1");
        }

        public static string GetProperty(List<XwmPropertyData> properties, string key, string fallback = "")
        {
            if (properties == null || string.IsNullOrWhiteSpace(key))
            {
                return fallback;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                XwmPropertyData property = properties[i];
                if (property != null && string.Equals(property.key, key, StringComparison.OrdinalIgnoreCase))
                {
                    return property.value ?? fallback;
                }
            }

            return fallback;
        }

        public static void SetProperty(List<XwmPropertyData> properties, string key, string value)
        {
            if (properties == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                XwmPropertyData property = properties[i];
                if (property != null && string.Equals(property.key, key, StringComparison.OrdinalIgnoreCase))
                {
                    property.value = value ?? string.Empty;
                    return;
                }
            }

            properties.Add(new XwmPropertyData { key = key, value = value ?? string.Empty });
        }

        public static bool RemoveProperty(List<XwmPropertyData> properties, string key)
        {
            if (properties == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                XwmPropertyData property = properties[i];
                if (property != null && string.Equals(property.key, key, StringComparison.OrdinalIgnoreCase))
                {
                    properties.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static float GetFloat(List<XwmPropertyData> properties, string key, float fallback)
        {
            string raw = GetProperty(properties, key, string.Empty);
            if (float.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }

            if (float.TryParse(raw, out value))
            {
                return value;
            }

            return fallback;
        }

        public static int GetInt(List<XwmPropertyData> properties, string key, int fallback)
        {
            string raw = GetProperty(properties, key, string.Empty);
            if (int.TryParse(raw, out int value))
            {
                return value;
            }

            return fallback;
        }

        public static bool GetBool(List<XwmPropertyData> properties, string key, bool fallback)
        {
            string raw = GetProperty(properties, key, string.Empty);
            if (bool.TryParse(raw, out bool value))
            {
                return value;
            }

            if (string.Equals(raw, "1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(raw, "0", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return fallback;
        }

        public static Color ParseColor(string value, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return fallback;
            }

            if (ColorUtility.TryParseHtmlString(value.Trim(), out Color parsed))
            {
                return parsed;
            }

            string[] parts = value.Split(',');
            if (parts.Length >= 3)
            {
                if (float.TryParse(parts[0], out float r) && float.TryParse(parts[1], out float g) && float.TryParse(parts[2], out float b))
                {
                    float a = 1f;
                    if (parts.Length >= 4)
                    {
                        float.TryParse(parts[3], out a);
                    }

                    if (r > 1f || g > 1f || b > 1f || a > 1f)
                    {
                        r /= 255f;
                        g /= 255f;
                        b /= 255f;
                        a /= 255f;
                    }

                    return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), Mathf.Clamp01(a));
                }
            }

            return fallback;
        }

        public static string ToColorString(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }

        public static List<string> GetEditableKeys(string type)
        {
            string normalized = XwmTypeLibrary.Normalize(type);
            if (!EditableKeyMap.TryGetValue(normalized, out List<string> keys))
            {
                keys = EditableKeyMap[XwmTypeLibrary.Frame];
            }

            return keys;
        }

        public static void EnsureDocument(XwmDocumentData document)
        {
            if (document == null)
            {
                return;
            }

            if (document.nodes == null)
            {
                document.nodes = new List<XwmNodeData>();
            }

            XwmNodeData root = null;
            for (int i = 0; i < document.nodes.Count; i++)
            {
                XwmNodeData node = document.nodes[i];
                if (node == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(node.id))
                {
                    node.id = "node_" + Guid.NewGuid().ToString("N");
                }

                if (string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
                {
                    root = node;
                }

                if (string.IsNullOrWhiteSpace(node.name))
                {
                    node.name = string.IsNullOrWhiteSpace(node.type) ? "Node" : node.type;
                }

                node.type = XwmTypeLibrary.Normalize(node.type);
                if (node.properties == null)
                {
                    node.properties = CreateDefaultProperties(node.type);
                }
                else
                {
                    EnsureDefaults(node);
                }
            }

            if (root == null)
            {
                root = XwmDocumentData.CreateDefault(document.documentName).nodes[0];
                document.nodes.Insert(0, root);
            }

            root.id = "root";
            root.parentId = string.Empty;
            root.type = XwmTypeLibrary.Frame;
            root.name = string.IsNullOrWhiteSpace(root.name) ? "CanvasRoot" : root.name;
            if (root.properties == null)
            {
                root.properties = CreateDefaultProperties(XwmTypeLibrary.Frame);
            }

            float canvasWidth = document.canvasSize.x <= 0f ? 900f : document.canvasSize.x;
            float canvasHeight = document.canvasSize.y <= 0f ? 620f : document.canvasSize.y;
            document.canvasSize = new Vector2(canvasWidth, canvasHeight);
            SetProperty(root.properties, "width", canvasWidth.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            SetProperty(root.properties, "height", canvasHeight.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));

            if (document.nextOrder < 1)
            {
                int maxOrder = 0;
                for (int i = 0; i < document.nodes.Count; i++)
                {
                    XwmNodeData node = document.nodes[i];
                    if (node != null && node.order > maxOrder)
                    {
                        maxOrder = node.order;
                    }
                }

                document.nextOrder = maxOrder + 1;
            }
        }

        public static XwmNodeData GetNodeById(XwmDocumentData document, string id)
        {
            if (document == null || document.nodes == null || string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            for (int i = 0; i < document.nodes.Count; i++)
            {
                XwmNodeData node = document.nodes[i];
                if (node != null && string.Equals(node.id, id, StringComparison.OrdinalIgnoreCase))
                {
                    return node;
                }
            }

            return null;
        }

        public static XwmNodeData GetRootNode(XwmDocumentData document)
        {
            return GetNodeById(document, "root");
        }

        public static List<XwmNodeData> GetChildren(XwmDocumentData document, string parentId)
        {
            List<XwmNodeData> output = new List<XwmNodeData>();
            if (document == null || document.nodes == null)
            {
                return output;
            }

            string resolvedParent = string.IsNullOrWhiteSpace(parentId) ? "root" : parentId;
            for (int i = 0; i < document.nodes.Count; i++)
            {
                XwmNodeData node = document.nodes[i];
                if (node == null)
                {
                    continue;
                }

                string candidateParent = string.IsNullOrWhiteSpace(node.parentId) ? "root" : node.parentId;
                if (string.Equals(candidateParent, resolvedParent, StringComparison.OrdinalIgnoreCase) && !string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
                {
                    output.Add(node);
                }
            }

            output.Sort((a, b) =>
            {
                int layerCompare = a.layer.CompareTo(b.layer);
                if (layerCompare != 0)
                {
                    return layerCompare;
                }

                return a.order.CompareTo(b.order);
            });
            return output;
        }

        public static void EnsureDefaults(XwmNodeData node)
        {
            if (node == null)
            {
                return;
            }

            if (node.properties == null)
            {
                node.properties = new List<XwmPropertyData>();
            }

            List<XwmPropertyData> defaults = CreateDefaultProperties(node.type);
            HashSet<string> allowedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < defaults.Count; i++)
            {
                XwmPropertyData def = defaults[i];
                if (def == null || string.IsNullOrWhiteSpace(def.key))
                {
                    continue;
                }

                allowedKeys.Add(def.key);
            }

            allowedKeys.Add("active");

            for (int i = node.properties.Count - 1; i >= 0; i--)
            {
                XwmPropertyData property = node.properties[i];
                if (property == null || string.IsNullOrWhiteSpace(property.key))
                {
                    node.properties.RemoveAt(i);
                    continue;
                }

                if (!allowedKeys.Contains(property.key))
                {
                    node.properties.RemoveAt(i);
                }
            }

            for (int i = 0; i < defaults.Count; i++)
            {
                XwmPropertyData def = defaults[i];
                string current = GetProperty(node.properties, def.key, null);
                if (current == null)
                {
                    if (IsTransparencyKey(def.key))
                    {
                        continue;
                    }

                    SetProperty(node.properties, def.key, def.value);
                }
            }
        }

        private static bool IsTransparencyKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return key.IndexOf("transparency", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string NextNodeName(XwmDocumentData document, string type)
        {
            string normalized = XwmTypeLibrary.Normalize(type);
            int index = 1;
            if (document != null && document.nodes != null)
            {
                for (int i = 0; i < document.nodes.Count; i++)
                {
                    XwmNodeData node = document.nodes[i];
                    if (node == null)
                    {
                        continue;
                    }

                    if (string.Equals(node.type, normalized, StringComparison.OrdinalIgnoreCase))
                    {
                        index++;
                    }
                }
            }

            return normalized + index;
        }

        public static string NextNodeId(XwmDocumentData document)
        {
            int index = document != null ? document.nextOrder : 1;
            if (index < 1)
            {
                index = 1;
            }

            string id = "node_" + index;
            while (document != null && GetNodeById(document, id) != null)
            {
                index++;
                id = "node_" + index;
            }

            if (document != null)
            {
                document.nextOrder = index + 1;
            }

            return id;
        }
    }
}
