using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiWindowsMod.Xwm.Studio
{
    internal class XwmStudioPanelDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RectTransform Target;
        public Func<bool> CanDrag;
        public Action<Vector2> Dragged;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Target == null || eventData == null)
            {
                return;
            }

            if (CanDrag != null && !CanDrag())
            {
                return;
            }

            Target.anchoredPosition += eventData.delta;
            Dragged?.Invoke(eventData.delta);
        }
    }
}
