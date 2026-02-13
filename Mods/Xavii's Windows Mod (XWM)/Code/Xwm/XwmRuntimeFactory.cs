using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XaviiWindowsMod.API;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmRuntimeFactory
    {
        internal static XwmWindowHandle Build(XwmDocumentData document, RectTransform parent, string runtimeId, string modTarget, string fileName, bool startVisible, bool previewMode, Action<XwmElementRef> onElementReady)
        {
            if (document == null || parent == null || string.IsNullOrWhiteSpace(runtimeId))
            {
                return null;
            }

            XwmPropertyUtility.EnsureDocument(document);
            XwmUiBootstrap.EnsureEventSystem();

            GameObject rootObject = new GameObject("XWM_Runtime_" + runtimeId, typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
            RectTransform rootRect = rootObject.GetComponent<RectTransform>();
            rootRect.SetParent(parent, false);
            rootRect.anchorMin = new Vector2(0.5f, 0.5f);
            rootRect.anchorMax = new Vector2(0.5f, 0.5f);
            rootRect.pivot = new Vector2(0.5f, 0.5f);
            rootRect.sizeDelta = document.canvasSize.x > 0f && document.canvasSize.y > 0f ? document.canvasSize : new Vector2(900f, 620f);
            rootRect.anchoredPosition = Vector2.zero;

            Image rootImage = rootObject.GetComponent<Image>();
            rootImage.color = new Color(0f, 0f, 0f, 0f);
            rootImage.raycastTarget = true;

            List<XwmElementRef> elements = new List<XwmElementRef>();
            Dictionary<string, XwmNodeData> nodesById = new Dictionary<string, XwmNodeData>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, List<XwmNodeData>> childrenByParent = new Dictionary<string, List<XwmNodeData>>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < document.nodes.Count; i++)
            {
                XwmNodeData node = document.nodes[i];
                if (node == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(node.id))
                {
                    continue;
                }

                nodesById[node.id] = node;

                if (string.Equals(node.id, "root", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string parentId = string.IsNullOrWhiteSpace(node.parentId) ? "root" : node.parentId;
                if (!childrenByParent.TryGetValue(parentId, out List<XwmNodeData> list))
                {
                    list = new List<XwmNodeData>();
                    childrenByParent[parentId] = list;
                }

                list.Add(node);
            }

            foreach (KeyValuePair<string, List<XwmNodeData>> pair in childrenByParent)
            {
                pair.Value.Sort((a, b) =>
                {
                    int layerCompare = a.layer.CompareTo(b.layer);
                    if (layerCompare != 0)
                    {
                        return layerCompare;
                    }

                    return a.order.CompareTo(b.order);
                });
            }

            XwmNodeData rootNode = XwmPropertyUtility.GetRootNode(document) ?? XwmDocumentData.CreateDefault(document.documentName).nodes[0];
            XwmElementRef rootElement = CreateElement(rootNode, rootRect, true, previewMode);
            if (rootElement != null)
            {
                elements.Add(rootElement);
                onElementReady?.Invoke(rootElement);
                BuildChildren(rootNode.id, rootElement.RectTransform != null ? rootElement.RectTransform : rootRect, childrenByParent, elements, previewMode, onElementReady);
            }

            XwmWindowHandle handle = new XwmWindowHandle(runtimeId, modTarget, fileName, rootObject, rootRect, elements);
            if (!startVisible)
            {
                handle.Hide();
            }

            return handle;
        }

        private static void BuildChildren(string parentId, RectTransform parent, Dictionary<string, List<XwmNodeData>> childrenByParent, List<XwmElementRef> elements, bool previewMode, Action<XwmElementRef> onElementReady)
        {
            if (parent == null)
            {
                return;
            }

            if (!childrenByParent.TryGetValue(parentId, out List<XwmNodeData> children))
            {
                return;
            }

            for (int i = 0; i < children.Count; i++)
            {
                XwmNodeData child = children[i];
                XwmElementRef element = CreateElement(child, parent, false, previewMode);
                if (element == null)
                {
                    continue;
                }

                elements.Add(element);
                onElementReady?.Invoke(element);
                RectTransform nextParent = ResolveChildContainer(element, parent);
                BuildChildren(child.id, nextParent, childrenByParent, elements, previewMode, onElementReady);
            }
        }

        private static XwmElementRef CreateElement(XwmNodeData node, RectTransform parent, bool forceFrame, bool previewMode)
        {
            if (node == null || parent == null)
            {
                return null;
            }

            string normalizedType = forceFrame ? XwmTypeLibrary.Frame : XwmTypeLibrary.Normalize(node.type);
            string name = string.IsNullOrWhiteSpace(node.name) ? normalizedType : node.name;

            GameObject gameObject = new GameObject(name, typeof(RectTransform));
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            ApplyRectTransform(rect, node.properties, forceFrame);

            Image image = null;
            Text text = null;
            Button button = null;
            InputField inputField = null;

            if (string.Equals(normalizedType, XwmTypeLibrary.Frame, StringComparison.OrdinalIgnoreCase) || string.Equals(normalizedType, XwmTypeLibrary.ScrollingFrame, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/windowInnerSliced"), "ui/icons/windowInnerSliced");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#2A3447D9"), new Color(0.16f, 0.2f, 0.28f, 0.85f));
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                if (string.Equals(normalizedType, XwmTypeLibrary.ScrollingFrame, StringComparison.OrdinalIgnoreCase))
                {
                    ConfigureScrollingFrame(gameObject, node.properties);
                }
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.TextLabel, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/backgroundTabButton"), "ui/icons/backgroundTabButton");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#4D79A7FF"), new Color(0.3f, 0.47f, 0.65f, 1f));
                bool labelRaycastTarget = previewMode || XwmPropertyUtility.GetBool(node.properties, "raycastTarget", false);
                image.raycastTarget = labelRaycastTarget;
                int fontSize = XwmPropertyUtility.GetInt(node.properties, "fontSize", 14);
                string fontType = XwmPropertyUtility.GetProperty(node.properties, "fontType", "Default");
                bool textScaled = XwmPropertyUtility.GetBool(node.properties, "textScaled", false);
                bool textWrapped = XwmPropertyUtility.GetBool(node.properties, "textWrapped", true);
                text = CreateTextChild(gameObject.transform, "Label", XwmPropertyUtility.GetProperty(node.properties, "text", "Label"), fontSize, XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "textColor", "#F4F9FFFF"), Color.white), fontType, textScaled, textWrapped);
                text.alignment = ParseTextAnchor(XwmPropertyUtility.GetProperty(node.properties, "alignment", "MiddleCenter"), TextAnchor.MiddleCenter);
                text.raycastTarget = labelRaycastTarget;
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                ApplyTransparency(node.properties, "textTransparency", text);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.ImageLabel, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/iconPortrait"), "ui/icons/iconPortrait");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#FFFFFFFF"), Color.white);
                image.raycastTarget = previewMode || XwmPropertyUtility.GetBool(node.properties, "raycastTarget", false);
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                ApplyTransparency(node.properties, "imageTransparency", image);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.TextButton, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/backgroundTabButton"), "ui/icons/backgroundTabButton");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#4D79A7FF"), new Color(0.3f, 0.47f, 0.65f, 1f));
                button = gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.interactable = XwmPropertyUtility.GetBool(node.properties, "interactable", true);
                int fontSize = XwmPropertyUtility.GetInt(node.properties, "fontSize", 14);
                string fontType = XwmPropertyUtility.GetProperty(node.properties, "fontType", "Default");
                bool textScaled = XwmPropertyUtility.GetBool(node.properties, "textScaled", false);
                bool textWrapped = XwmPropertyUtility.GetBool(node.properties, "textWrapped", true);
                text = CreateTextChild(gameObject.transform, "Label", XwmPropertyUtility.GetProperty(node.properties, "text", "Button"), fontSize, XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "textColor", "#F4F9FFFF"), Color.white), fontType, textScaled, textWrapped);
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                ApplyTransparency(node.properties, "textTransparency", text);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.TextBox, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/windowInnerSliced"), "ui/icons/windowInnerSliced");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#1E2733FF"), new Color(0.12f, 0.15f, 0.2f, 1f));

                inputField = gameObject.AddComponent<InputField>();
                inputField.interactable = XwmPropertyUtility.GetBool(node.properties, "interactable", true);
                int fontSize = XwmPropertyUtility.GetInt(node.properties, "fontSize", 14);
                string fontType = XwmPropertyUtility.GetProperty(node.properties, "fontType", "Default");
                bool textScaled = XwmPropertyUtility.GetBool(node.properties, "textScaled", false);
                bool textWrapped = XwmPropertyUtility.GetBool(node.properties, "textWrapped", false);
                text = CreateTextChild(gameObject.transform, "Text", XwmPropertyUtility.GetProperty(node.properties, "text", string.Empty), fontSize, XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "textColor", "#F4F9FFFF"), Color.white), fontType, textScaled, textWrapped);
                Text placeholder = CreateTextChild(gameObject.transform, "Placeholder", XwmPropertyUtility.GetProperty(node.properties, "placeholder", "Type here"), fontSize, XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "placeholderColor", "#91A8B9CC"), new Color(0.56f, 0.66f, 0.72f, 0.8f)), fontType, textScaled, textWrapped);

                RectTransform textRect = text.GetComponent<RectTransform>();
                textRect.offsetMin = new Vector2(10f, 6f);
                textRect.offsetMax = new Vector2(-10f, -6f);

                RectTransform placeholderRect = placeholder.GetComponent<RectTransform>();
                placeholderRect.offsetMin = new Vector2(10f, 6f);
                placeholderRect.offsetMax = new Vector2(-10f, -6f);

                inputField.textComponent = text;
                inputField.placeholder = placeholder;
                inputField.lineType = textWrapped ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine;
                inputField.text = XwmPropertyUtility.GetProperty(node.properties, "text", string.Empty);
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                ApplyTransparency(node.properties, "textTransparency", text);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.ImageButton, StringComparison.OrdinalIgnoreCase))
            {
                image = gameObject.AddComponent<Image>();
                image.type = Image.Type.Sliced;
                image.sprite = XwmUiBootstrap.ResolveSprite(XwmPropertyUtility.GetProperty(node.properties, "sprite", "ui/icons/iconPortrait"), "ui/icons/iconPortrait");
                image.color = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "color", "#FFFFFFFF"), Color.white);
                button = gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.interactable = XwmPropertyUtility.GetBool(node.properties, "interactable", true);
                ApplyTransparency(node.properties, "backgroundTransparency", image);
                ApplyTransparency(node.properties, "imageTransparency", image);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UICorner, StringComparison.OrdinalIgnoreCase))
            {
                XwmCornerComponent corner = GetOrAddComponent<XwmCornerComponent>(parent.gameObject);
                corner.Radius = XwmPropertyUtility.GetFloat(node.properties, "radius", 8f);
                XwmRoundedImageComponent rounded = GetOrAddComponent<XwmRoundedImageComponent>(parent.gameObject);
                rounded.Radius = corner.Radius;
                rounded.ApplyNow();
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIScale, StringComparison.OrdinalIgnoreCase))
            {
                XwmScaleComponent scale = gameObject.AddComponent<XwmScaleComponent>();
                float amount = XwmPropertyUtility.GetFloat(node.properties, "scale", 1f);
                scale.Scale = amount;
                if (parent != null)
                {
                    parent.localScale = new Vector3(amount, amount, 1f);
                }
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIListLayout, StringComparison.OrdinalIgnoreCase))
            {
                ApplyListLayout(parent.gameObject, node.properties);
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIDragDetector, StringComparison.OrdinalIgnoreCase))
            {
                XwmRuntimeDragDetector detector = GetOrAddComponent<XwmRuntimeDragDetector>(parent.gameObject);
                detector.Target = parent;
                detector.Enabled = XwmPropertyUtility.GetBool(node.properties, "enabled", true);
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIGradient, StringComparison.OrdinalIgnoreCase))
            {
                XwmGradientComponent gradient = GetOrAddComponent<XwmGradientComponent>(parent.gameObject);
                gradient.TopColor = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "topColor", "#5DA4E5FF"), new Color(0.36f, 0.64f, 0.9f, 1f));
                gradient.BottomColor = XwmPropertyUtility.ParseColor(XwmPropertyUtility.GetProperty(node.properties, "bottomColor", "#1A3150FF"), new Color(0.1f, 0.19f, 0.31f, 1f));
                gradient.Angle = XwmPropertyUtility.GetFloat(node.properties, "angle", 90f);
                Graphic targetGraphic = parent.GetComponent<Graphic>();
                if (targetGraphic != null)
                {
                    XwmGradientEffect effect = GetOrAddComponent<XwmGradientEffect>(targetGraphic.gameObject);
                    effect.TopColor = gradient.TopColor;
                    effect.BottomColor = gradient.BottomColor;
                    effect.Angle = gradient.Angle;
                    targetGraphic.SetVerticesDirty();
                }
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIGridLayout, StringComparison.OrdinalIgnoreCase))
            {
                ApplyGridLayout(parent.gameObject, node.properties);
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIPadding, StringComparison.OrdinalIgnoreCase))
            {
                ApplyPadding(parent.gameObject, node.properties);
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UIPageLayout, StringComparison.OrdinalIgnoreCase))
            {
                ApplyPageLayout(parent.gameObject, node.properties);
                gameObject.SetActive(false);
            }
            else if (string.Equals(normalizedType, XwmTypeLibrary.UITableLayout, StringComparison.OrdinalIgnoreCase))
            {
                ApplyTableLayout(parent.gameObject, node.properties);
                gameObject.SetActive(false);
            }

            if (image == null && !XwmTypeLibrary.IsHelperType(normalizedType))
            {
                image = gameObject.AddComponent<Image>();
                image.color = new Color(1f, 1f, 1f, 0.05f);
            }

            XwmPointerRelay pointerRelay = gameObject.GetComponent<XwmPointerRelay>();
            if (pointerRelay == null)
            {
                pointerRelay = gameObject.AddComponent<XwmPointerRelay>();
            }

            bool active = node.active && XwmPropertyUtility.GetBool(node.properties, "active", true);
            if (!previewMode && XwmTypeLibrary.IsHelperType(normalizedType))
            {
                active = false;
            }

            gameObject.SetActive(active);

            XwmElementRef element = new XwmElementRef(node.id, name, normalizedType, gameObject, rect, image, text, button, inputField, pointerRelay);
            return element;
        }

        private static RectTransform ResolveChildContainer(XwmElementRef element, RectTransform fallback)
        {
            if (element == null)
            {
                return fallback;
            }

            if (string.Equals(element.Type, XwmTypeLibrary.ScrollingFrame, StringComparison.OrdinalIgnoreCase) && element.GameObject != null)
            {
                XwmScrollingFrameComponent scrollingFrame = element.GameObject.GetComponent<XwmScrollingFrameComponent>();
                if (scrollingFrame != null && scrollingFrame.Content != null)
                {
                    return scrollingFrame.Content;
                }
            }

            return element.RectTransform != null ? element.RectTransform : fallback;
        }

        private static TextAnchor ParseTextAnchor(string value, TextAnchor fallback)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return fallback;
            }

            string trimmed = value.Trim();
            if (string.Equals(trimmed, "UpperLeft", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.UpperLeft;
            }

            if (string.Equals(trimmed, "UpperCenter", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.UpperCenter;
            }

            if (string.Equals(trimmed, "UpperRight", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.UpperRight;
            }

            if (string.Equals(trimmed, "MiddleLeft", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.MiddleLeft;
            }

            if (string.Equals(trimmed, "MiddleCenter", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.MiddleCenter;
            }

            if (string.Equals(trimmed, "MiddleRight", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.MiddleRight;
            }

            if (string.Equals(trimmed, "LowerLeft", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.LowerLeft;
            }

            if (string.Equals(trimmed, "LowerCenter", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.LowerCenter;
            }

            if (string.Equals(trimmed, "LowerRight", StringComparison.OrdinalIgnoreCase))
            {
                return TextAnchor.LowerRight;
            }

            return fallback;
        }

        private static void ApplyRectTransform(RectTransform rect, List<XwmPropertyData> properties, bool root)
        {
            if (rect == null)
            {
                return;
            }

            float anchorMinX = XwmPropertyUtility.GetFloat(properties, "anchorMinX", root ? 0.5f : 0f);
            float anchorMinY = XwmPropertyUtility.GetFloat(properties, "anchorMinY", root ? 0.5f : 1f);
            float anchorMaxX = XwmPropertyUtility.GetFloat(properties, "anchorMaxX", root ? 0.5f : 0f);
            float anchorMaxY = XwmPropertyUtility.GetFloat(properties, "anchorMaxY", root ? 0.5f : 1f);
            float pivotX = XwmPropertyUtility.GetFloat(properties, "pivotX", root ? 0.5f : 0f);
            float pivotY = XwmPropertyUtility.GetFloat(properties, "pivotY", root ? 0.5f : 1f);
            float x = XwmPropertyUtility.GetFloat(properties, "x", root ? 0f : 20f);
            float y = XwmPropertyUtility.GetFloat(properties, "y", root ? 0f : 20f);
            float width = XwmPropertyUtility.GetFloat(properties, "width", root ? 900f : 220f);
            float height = XwmPropertyUtility.GetFloat(properties, "height", root ? 620f : 64f);
            float rotation = XwmPropertyUtility.GetFloat(properties, "rotation", 0f);
            float scaleX = XwmPropertyUtility.GetFloat(properties, "scaleX", 1f);
            float scaleY = XwmPropertyUtility.GetFloat(properties, "scaleY", 1f);

            rect.anchorMin = new Vector2(anchorMinX, anchorMinY);
            rect.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            rect.pivot = new Vector2(pivotX, pivotY);
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.localEulerAngles = new Vector3(0f, 0f, rotation);
            rect.localScale = new Vector3(scaleX, scaleY, 1f);
        }

        private static Text CreateTextChild(Transform parent, string name, string value, int fontSize, Color color, string fontType, bool textScaled, bool textWrapped)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.SetParent(parent, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(6f, 4f);
            textRect.offsetMax = new Vector2(-6f, -4f);
            textRect.pivot = new Vector2(0.5f, 0.5f);

            Text text = textObject.GetComponent<Text>();
            text.font = XwmUiBootstrap.ResolveFont(fontType, XwmUiBootstrap.DefaultFont);
            text.fontSize = Mathf.Max(8, fontSize);
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = textScaled;
            text.resizeTextMinSize = 1;
            text.resizeTextMaxSize = 512;
            text.horizontalOverflow = textWrapped ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.color = color;
            text.text = value ?? string.Empty;
            return text;
        }

        private static void ApplyTransparency(List<XwmPropertyData> properties, string key, Graphic graphic)
        {
            if (graphic == null)
            {
                return;
            }

            float fallback = 1f - graphic.color.a;
            float transparency = Mathf.Clamp01(XwmPropertyUtility.GetFloat(properties, key, fallback));
            Color color = graphic.color;
            color.a = 1f - transparency;
            graphic.color = color;
        }

        private static void ConfigureScrollingFrame(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            ScrollRect scrollRect = GetOrAddComponent<ScrollRect>(target);
            scrollRect.horizontal = XwmPropertyUtility.GetBool(properties, "scrollHorizontal", false);
            scrollRect.vertical = XwmPropertyUtility.GetBool(properties, "scrollVertical", true);
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = Mathf.Max(1f, XwmPropertyUtility.GetFloat(properties, "scrollSensitivity", 18f));

            GameObject viewportObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
            viewportRect.SetParent(target.transform, false);
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;

            Image viewportImage = viewportObject.GetComponent<Image>();
            viewportImage.color = new Color(1f, 1f, 1f, 0f);
            viewportImage.raycastTarget = false;
            Mask viewportMask = viewportObject.GetComponent<Mask>();
            viewportMask.showMaskGraphic = false;

            GameObject contentObject = new GameObject("Content", typeof(RectTransform));
            RectTransform contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.SetParent(viewportRect, false);
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;

            XwmScrollingFrameComponent scrollingFrame = GetOrAddComponent<XwmScrollingFrameComponent>(target);
            scrollingFrame.ScrollRect = scrollRect;
            scrollingFrame.Viewport = viewportRect;
            scrollingFrame.Content = contentRect;
            scrollingFrame.RefreshBounds();
        }

        private static void ApplyListLayout(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            string orientation = XwmPropertyUtility.GetProperty(properties, "orientation", "Vertical");
            float spacing = XwmPropertyUtility.GetFloat(properties, "spacing", 8f);
            bool childControlWidth = XwmPropertyUtility.GetBool(properties, "childControlWidth", true);
            bool childControlHeight = XwmPropertyUtility.GetBool(properties, "childControlHeight", false);
            bool childForceExpandWidth = XwmPropertyUtility.GetBool(properties, "childForceExpandWidth", false);
            bool childForceExpandHeight = XwmPropertyUtility.GetBool(properties, "childForceExpandHeight", false);

            if (string.Equals(orientation, "Horizontal", StringComparison.OrdinalIgnoreCase))
            {
                VerticalLayoutGroup vertical = target.GetComponent<VerticalLayoutGroup>();
                if (vertical != null)
                {
                    UnityEngine.Object.Destroy(vertical);
                }

                HorizontalLayoutGroup layout = GetOrAddComponent<HorizontalLayoutGroup>(target);
                layout.spacing = spacing;
                layout.childControlWidth = childControlWidth;
                layout.childControlHeight = childControlHeight;
                layout.childForceExpandWidth = childForceExpandWidth;
                layout.childForceExpandHeight = childForceExpandHeight;
            }
            else
            {
                HorizontalLayoutGroup horizontal = target.GetComponent<HorizontalLayoutGroup>();
                if (horizontal != null)
                {
                    UnityEngine.Object.Destroy(horizontal);
                }

                VerticalLayoutGroup layout = GetOrAddComponent<VerticalLayoutGroup>(target);
                layout.spacing = spacing;
                layout.childControlWidth = childControlWidth;
                layout.childControlHeight = childControlHeight;
                layout.childForceExpandWidth = childForceExpandWidth;
                layout.childForceExpandHeight = childForceExpandHeight;
            }
        }

        private static void ApplyGridLayout(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            GridLayoutGroup layout = GetOrAddComponent<GridLayoutGroup>(target);
            layout.cellSize = new Vector2(XwmPropertyUtility.GetFloat(properties, "cellWidth", 96f), XwmPropertyUtility.GetFloat(properties, "cellHeight", 48f));
            layout.spacing = new Vector2(XwmPropertyUtility.GetFloat(properties, "spacingX", 8f), XwmPropertyUtility.GetFloat(properties, "spacingY", 8f));

            string constraint = XwmPropertyUtility.GetProperty(properties, "constraint", "FixedColumnCount");
            if (string.Equals(constraint, "FixedRowCount", StringComparison.OrdinalIgnoreCase))
            {
                layout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            }
            else if (string.Equals(constraint, "Flexible", StringComparison.OrdinalIgnoreCase))
            {
                layout.constraint = GridLayoutGroup.Constraint.Flexible;
            }
            else
            {
                layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            }

            layout.constraintCount = Mathf.Max(1, XwmPropertyUtility.GetInt(properties, "constraintCount", 3));
        }

        private static void ApplyPadding(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            LayoutGroup layout = target.GetComponent<LayoutGroup>();

            if (layout == null)
            {
                VerticalLayoutGroup vertical = GetOrAddComponent<VerticalLayoutGroup>(target);
                vertical.childControlHeight = false;
                vertical.childControlWidth = false;
                vertical.childForceExpandHeight = false;
                vertical.childForceExpandWidth = false;
                layout = vertical;
            }

            layout.padding = new RectOffset(
                XwmPropertyUtility.GetInt(properties, "left", 8),
                XwmPropertyUtility.GetInt(properties, "right", 8),
                XwmPropertyUtility.GetInt(properties, "top", 8),
                XwmPropertyUtility.GetInt(properties, "bottom", 8));
        }

        private static void ApplyPageLayout(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            string axis = XwmPropertyUtility.GetProperty(properties, "axis", "Horizontal");
            HorizontalLayoutGroup horizontal = target.GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup vertical = target.GetComponent<VerticalLayoutGroup>();
            if (string.Equals(axis, "Vertical", StringComparison.OrdinalIgnoreCase))
            {
                if (horizontal != null)
                {
                    UnityEngine.Object.Destroy(horizontal);
                }

                vertical = GetOrAddComponent<VerticalLayoutGroup>(target);
                vertical.spacing = XwmPropertyUtility.GetFloat(properties, "spacing", 4f);
                vertical.childControlWidth = true;
                vertical.childControlHeight = true;
                vertical.childForceExpandWidth = true;
                vertical.childForceExpandHeight = true;
            }
            else
            {
                if (vertical != null)
                {
                    UnityEngine.Object.Destroy(vertical);
                }

                horizontal = GetOrAddComponent<HorizontalLayoutGroup>(target);
                horizontal.spacing = XwmPropertyUtility.GetFloat(properties, "spacing", 4f);
                horizontal.childControlWidth = true;
                horizontal.childControlHeight = true;
                horizontal.childForceExpandWidth = true;
                horizontal.childForceExpandHeight = true;
            }

            XwmPageLayoutComponent pageLayout = GetOrAddComponent<XwmPageLayoutComponent>(target);
            pageLayout.CurrentPage = Mathf.Max(0, XwmPropertyUtility.GetInt(properties, "page", 0));
            pageLayout.Apply();
        }

        private static void ApplyTableLayout(GameObject target, List<XwmPropertyData> properties)
        {
            if (target == null)
            {
                return;
            }

            GridLayoutGroup grid = GetOrAddComponent<GridLayoutGroup>(target);
            grid.cellSize = new Vector2(XwmPropertyUtility.GetFloat(properties, "cellWidth", 120f), XwmPropertyUtility.GetFloat(properties, "cellHeight", 40f));
            grid.spacing = new Vector2(XwmPropertyUtility.GetFloat(properties, "spacingX", 6f), XwmPropertyUtility.GetFloat(properties, "spacingY", 6f));
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Mathf.Max(1, XwmPropertyUtility.GetInt(properties, "columns", 3));
        }

        private static T GetOrAddComponent<T>(GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (component == null)
            {
                component = target.AddComponent<T>();
            }

            return component;
        }
    }
}
