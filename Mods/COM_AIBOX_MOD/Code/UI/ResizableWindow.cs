using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIBox.UI
{
    public class ResizableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        private RectTransform windowRT;
        private Vector2 minSize;
        private Vector2 maxSize;
        private Vector2 currentPointerPosition;
        private Vector2 previousPointerPosition;

        public static void CreateResizeHandle(Transform windowTransform, RectTransform windowRT, Vector2 minSize, Vector2 maxSize)
        {
            GameObject handleObj = new GameObject("ResizeHandle");
            handleObj.transform.SetParent(windowTransform, false);
            
            Image handleImage = handleObj.AddComponent<Image>();
            handleImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            
            RectTransform rt = handleObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1, 0);
            rt.sizeDelta = new Vector2(20, 20);
            rt.anchoredPosition = Vector2.zero;

            ResizableWindow script = handleObj.AddComponent<ResizableWindow>();
            script.windowRT = windowRT;
            script.minSize = minSize;
            script.maxSize = maxSize;
        }

        public void OnPointerDown(PointerEventData data)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(windowRT, data.position, data.pressEventCamera, out previousPointerPosition);
        }

        public void OnDrag(PointerEventData data)
        {
            if (windowRT == null) return;

            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(windowRT, data.position, data.pressEventCamera, out localPointerPosition);
            
            Vector2 offsetToOriginal = localPointerPosition - previousPointerPosition;
            Vector2 newSize = windowRT.sizeDelta + new Vector2(offsetToOriginal.x, -offsetToOriginal.y);

            newSize.x = Mathf.Clamp(newSize.x, minSize.x, maxSize.x);
            newSize.y = Mathf.Clamp(newSize.y, minSize.y, maxSize.y);

            windowRT.sizeDelta = newSize;
            previousPointerPosition = localPointerPosition;
        }
    }
}
