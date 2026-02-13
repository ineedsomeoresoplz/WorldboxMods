using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AIBox.UI
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;
        public Transform MainCanvas;

        private void Awake()
        {
            Instance = this;
            
            GameObject canvasGO = GameObject.Find("/Canvas Container Main/Canvas - Windows");
            if (canvasGO != null)
            {
                MainCanvas = canvasGO.transform;
            }
            else
            {
                GameObject newCanvas = new GameObject("EconomyBoxCanvas");
                Canvas c = newCanvas.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay;
                newCanvas.AddComponent<CanvasScaler>();
                newCanvas.AddComponent<GraphicRaycaster>();
                MainCanvas = newCanvas.transform;
            }
        }

        public GameObject CreateWindow(string title, Vector2 size, Vector2 position)
        {
            GameObject window = new GameObject("Window_" + title);
            window.transform.SetParent(MainCanvas, false);
            
            RectTransform rt = window.AddComponent<RectTransform>();
            rt.sizeDelta = size;
            rt.anchoredPosition = position;
            
            // Background
            Image bg = window.AddComponent<Image>();
            Sprite winSprite = null;
            try {
                 winSprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/EmbededResources/UI/Interface/windowInnerSliced.png");
            } catch (System.Exception) {
                 try {
                     winSprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/EmbededResources/UI/windowInnerSliced.png");
                 } catch {}
            }

            if (winSprite != null) {
                bg.sprite = winSprite;
                bg.type = Image.Type.Sliced;
                bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            } else {
                bg.color = new Color(0.05f, 0.05f, 0.05f, 1f); 
            }

            // Header
            GameObject header = new GameObject("Header");
            header.transform.SetParent(window.transform, false);
            RectTransform headerRT = header.AddComponent<RectTransform>();
            headerRT.anchorMin = new Vector2(0, 1);
            headerRT.anchorMax = new Vector2(1, 1);
            headerRT.pivot = new Vector2(0.5f, 1);
            headerRT.sizeDelta = new Vector2(0, 20); 
            headerRT.anchoredPosition = Vector2.zero;
            
            Image headerImg = header.AddComponent<Image>();
            headerImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            // Title Text
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(header.transform, false);
            Text titleTx = titleObj.AddComponent<Text>();
            titleTx.text = title;
            titleTx.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleTx.alignment = TextAnchor.MiddleCenter;
            titleTx.color = Color.white;
            titleTx.raycastTarget = false; 
            RectTransform titleRT = titleObj.GetComponent<RectTransform>();
            titleRT.anchorMin = Vector2.zero;
            titleRT.anchorMax = Vector2.one;
            titleRT.offsetMin = Vector2.zero;
            titleRT.offsetMax = Vector2.zero;

            // Make draggable
            header.AddComponent<DraggableWindow>().TargetWindow = window.transform;

            return window;
        }
    }
}
