using System;
using UnityEngine;
using UnityEngine.UI;
using NCMS.Utils;

namespace AIBox.UI
{
    public static class WindowHelper
    {
        public static void CreateHeader(Transform windowTransform, string title, Action onClose)
        {
            GameObject header = new GameObject("Header");
            header.transform.SetParent(windowTransform, false);
            
            var headerRect = header.AddComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0, 1);
            headerRect.anchorMax = new Vector2(1, 1);
            headerRect.pivot = new Vector2(0.5f, 1);
            headerRect.sizeDelta = new Vector2(0, 30);
            headerRect.anchoredPosition = new Vector2(0, 0);
            
            var headerImage = header.AddComponent<Image>();
            headerImage.color = new Color(0.1f, 0.1f, 0.1f);
            
            // Draggable
            header.AddComponent<DraggableWindow>().TargetWindow = windowTransform;

            // Title
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(header.transform, false);
             var titleText = titleObj.AddComponent<Text>();
            titleText.text = title;
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 12; 
            titleText.fontStyle = FontStyle.Bold; 
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;
            
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = Vector2.zero;
            titleRect.anchorMax = Vector2.one;
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Close Button
            GameObject closeBtn = new GameObject("CloseButton");
            closeBtn.transform.SetParent(header.transform, false);
            var closeRect = closeBtn.AddComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(1, 1);
            closeRect.anchorMax = new Vector2(1, 1);
            closeRect.pivot = new Vector2(1, 1);
            closeRect.sizeDelta = new Vector2(20, 20); 
            closeRect.anchoredPosition = new Vector2(-5, -5);
            
            closeBtn.AddComponent<Image>().color = new Color(0.8f, 0, 0, 0.8f); 
            closeBtn.AddComponent<Button>().onClick.AddListener(() => onClose?.Invoke());
            
            GameObject xObj = new GameObject("X");
            xObj.transform.SetParent(closeBtn.transform, false);
            var xText = xObj.AddComponent<Text>();
            xText.text = "X";
            xText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            xText.fontSize = 12;
            xText.color = Color.white;
            xText.alignment = TextAnchor.MiddleCenter;
            
            var xRect = xObj.GetComponent<RectTransform>();
            xRect.anchorMin = Vector2.zero;
            xRect.anchorMax = Vector2.one;
        }
    }
}
