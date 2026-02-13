using UnityEngine;
using UnityEngine.EventSystems;

namespace AIBox.UI
{
    public class DraggableWindow : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public Transform TargetWindow;
        private Vector2 dragOffset;
        private RectTransform targetRect;
        private RectTransform parentRect;
        private Vector2 startMousePosition;
        private Vector3 startWindowPosition;

        void Start()
        {
            if (TargetWindow == null) TargetWindow = transform.parent;
            targetRect = TargetWindow.GetComponent<RectTransform>();
            if (targetRect != null)
            {
                parentRect = targetRect.parent as RectTransform;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (targetRect == null || parentRect == null) return;

            startWindowPosition = targetRect.localPosition;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out startMousePosition
            );

            // Bring to front
            TargetWindow.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (targetRect == null || parentRect == null) return;

            Vector2 currentMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out currentMousePosition
            ))
            {
                Vector3 diff = currentMousePosition - startMousePosition;
                targetRect.localPosition = startWindowPosition + diff;
            }
        }
    }
}
