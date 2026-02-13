using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiWindowsMod.Xwm.Studio
{
    internal class XwmColorWheelInput : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public RectTransform TargetRect;
        public Action<Vector2> PointerChanged;

        public void OnPointerDown(PointerEventData eventData)
        {
            Handle(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Handle(eventData);
        }

        private void Handle(PointerEventData eventData)
        {
            RectTransform rect = TargetRect != null ? TargetRect : transform as RectTransform;
            if (rect == null || eventData == null)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                return;
            }

            Rect bounds = rect.rect;
            if (bounds.width <= 0f || bounds.height <= 0f)
            {
                return;
            }

            float normalizedX = Mathf.InverseLerp(bounds.xMin, bounds.xMax, localPoint.x);
            float normalizedY = Mathf.InverseLerp(bounds.yMin, bounds.yMax, localPoint.y);
            PointerChanged?.Invoke(new Vector2(normalizedX, normalizedY));
        }
    }
}
