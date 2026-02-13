using UnityEngine;
using UnityEngine.UI;
using XaviiWindowsMod.API;

namespace XaviiWindowsMod
{
    internal static class WindowFactory
    {
        internal static WindowInstance BuildWindow(RectTransform root, string id, string title, Vector2 size)
        {
            if (root == null)
            {
                GameObject fallbackRoot = new GameObject("XWM_FallbackRoot", typeof(RectTransform));
                root = fallbackRoot.GetComponent<RectTransform>();
                root.anchorMin = new Vector2(0, 0);
                root.anchorMax = new Vector2(1, 1);
                root.pivot = new Vector2(0.5f, 0.5f);
                root.offsetMin = Vector2.zero;
                root.offsetMax = Vector2.zero;
            }

            GameObject windowRoot = new GameObject($"XWM_Window_{id}", typeof(RectTransform), typeof(CanvasGroup));
            RectTransform windowRect = windowRoot.GetComponent<RectTransform>();
            windowRect.SetParent(root, false);
            windowRect.anchorMin = new Vector2(0.5f, 0.5f);
            windowRect.anchorMax = new Vector2(0.5f, 0.5f);
            windowRect.pivot = new Vector2(0.5f, 0.5f);
            windowRect.sizeDelta = size;
            windowRect.localScale = Vector3.one;
            windowRect.anchoredPosition = ResolveDefaultPosition(root, size);

            Image background = windowRoot.AddComponent<Image>();
            background.sprite = Resources.Load<Sprite>("ui/icons/window");
            if (background.sprite == null)
            {
                background.sprite = Resources.Load<Sprite>("ui/icons/windowInnerSliced");
            }
            background.type = Image.Type.Sliced;
            background.color = new Color(1f, 1f, 1f, 0.95f);

            RectTransform header = BuildHeader(windowRect, title, id, out Text titleText, out Button closeButton);
            RectTransform body = BuildBody(windowRect, header);
            ScrollRect scrollRect = BuildScroll(body, out RectTransform contentRect);

            WindowInstance instance = new WindowInstance(id, windowRoot, windowRect, body, contentRect, scrollRect, titleText, closeButton);
            closeButton.onClick.AddListener(() => WindowSystem.Close(id));
            return instance;
        }

        private static Vector2 ResolveDefaultPosition(RectTransform root, Vector2 windowSize)
        {
            return new Vector2(100f, 180f);
        }

        private static RectTransform BuildHeader(RectTransform parent, string title, string id, out Text titleText, out Button closeButton)
        {
            GameObject headerObj = new GameObject("Header", typeof(RectTransform), typeof(Image));
            RectTransform headerRect = headerObj.GetComponent<RectTransform>();
            headerRect.SetParent(parent, false);
            headerRect.anchorMin = new Vector2(0, 1);
            headerRect.anchorMax = new Vector2(1, 1);
            headerRect.pivot = new Vector2(0.5f, 1f);
            headerRect.sizeDelta = new Vector2(0, 26);
            headerRect.anchoredPosition = Vector2.zero;

            Image headerImage = headerObj.GetComponent<Image>();
            headerImage.sprite = Resources.Load<Sprite>("ui/icons/windowInnerSliced");
            headerImage.type = Image.Type.Sliced;
            headerImage.color = new Color(0.9f, 0.95f, 1f, 0.95f);

            GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(Text));
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.SetParent(headerRect, false);
            titleRect.anchorMin = new Vector2(0, 0);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(8, 4);
            titleRect.offsetMax = new Vector2(-30, -4);
            titleRect.pivot = new Vector2(0, 0.5f);

            titleText = titleObj.GetComponent<Text>();
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 12;
            titleText.text = title;
            titleText.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            titleText.alignment = TextAnchor.MiddleLeft;

            GameObject closeObj = new GameObject("Close", typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform closeRect = closeObj.GetComponent<RectTransform>();
            closeRect.SetParent(headerRect, false);
            closeRect.anchorMin = new Vector2(1, 0.5f);
            closeRect.anchorMax = new Vector2(1, 0.5f);
            closeRect.pivot = new Vector2(0.5f, 0.5f);
            closeRect.sizeDelta = new Vector2(18, 18);
            closeRect.anchoredPosition = new Vector2(-8, 0);

            Image closeImage = closeObj.GetComponent<Image>();
            closeImage.sprite = Resources.Load<Sprite>("ui/icons/close");
            if (closeImage.sprite == null)
            {
                closeImage.sprite = Resources.Load<Sprite>("ui/icons/backgroundTabButton");
            }
            closeImage.type = Image.Type.Sliced;
            closeImage.color = new Color(1f, 0.65f, 0.65f, 1f);

            GameObject closeLabel = new GameObject("Label", typeof(RectTransform), typeof(Text));
            RectTransform labelRect = closeLabel.GetComponent<RectTransform>();
            labelRect.SetParent(closeRect, false);
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(1, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelRect.pivot = new Vector2(0.5f, 0.5f);

            Text closeText = closeLabel.GetComponent<Text>();
            closeText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            closeText.fontSize = 12;
            closeText.alignment = TextAnchor.MiddleCenter;
            closeText.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            closeText.text = "X";

            closeButton = closeObj.GetComponent<Button>();
            return headerRect;
        }

        private static RectTransform BuildBody(RectTransform parent, RectTransform header)
        {
            GameObject bodyObj = new GameObject("Body", typeof(RectTransform), typeof(Image));
            RectTransform bodyRect = bodyObj.GetComponent<RectTransform>();
            bodyRect.SetParent(parent, false);
            bodyRect.anchorMin = new Vector2(0, 0);
            bodyRect.anchorMax = new Vector2(1, 1);
            bodyRect.pivot = new Vector2(0.5f, 0.5f);
            bodyRect.offsetMin = new Vector2(10, 10);
            bodyRect.offsetMax = new Vector2(-10, -header.sizeDelta.y);

            Image bodyImage = bodyObj.GetComponent<Image>();
            bodyImage.sprite = Resources.Load<Sprite>("ui/icons/windowInnerSliced");
            bodyImage.type = Image.Type.Sliced;
            bodyImage.color = new Color(1f, 1f, 1f, 0.9f);

            return bodyRect;
        }

        private static ScrollRect BuildScroll(RectTransform body, out RectTransform contentRect)
        {
            GameObject scrollObj = new GameObject("Scroll View", typeof(RectTransform), typeof(ScrollRect));
            RectTransform scrollRectTransform = scrollObj.GetComponent<RectTransform>();
            scrollRectTransform.SetParent(body, false);
            scrollRectTransform.anchorMin = new Vector2(0, 0);
            scrollRectTransform.anchorMax = new Vector2(1, 1);
            scrollRectTransform.pivot = new Vector2(0.5f, 0.5f);
            scrollRectTransform.offsetMin = new Vector2(6, 6);
            scrollRectTransform.offsetMax = new Vector2(-6, -6);

            GameObject viewportObj = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            RectTransform viewportRect = viewportObj.GetComponent<RectTransform>();
            viewportRect.SetParent(scrollRectTransform, false);
            viewportRect.anchorMin = new Vector2(0, 0);
            viewportRect.anchorMax = new Vector2(1, 1);
            viewportRect.pivot = new Vector2(0.5f, 0.5f);
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;

            Image viewportImage = viewportObj.GetComponent<Image>();
            viewportImage.color = new Color(1f, 1f, 1f, 0);
            viewportObj.GetComponent<Mask>().showMaskGraphic = false;

            GameObject contentObj = new GameObject("Content", typeof(RectTransform));
            contentRect = contentObj.GetComponent<RectTransform>();
            contentRect.SetParent(viewportRect, false);
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0, 1);
            contentRect.offsetMin = new Vector2(0, -200);
            contentRect.offsetMax = new Vector2(0, 0);
            contentRect.anchoredPosition = new Vector2(0, 0);
            contentRect.sizeDelta = new Vector2(0, 200);

            ScrollRect scrollRect = scrollObj.GetComponent<ScrollRect>();
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 16f;

            return scrollRect;
        }
    }
}
