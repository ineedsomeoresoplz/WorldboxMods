using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiWindowsMod.Xwm.Studio
{
    internal class XwmStudioSelectionProxy : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public string NodeId;
        public XwmStudioController Controller;
        private bool _resizeMode;

        public void OnPointerClick(PointerEventData eventData)
        {
            Controller?.SelectNodeFromPreview(NodeId);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Controller?.SelectNodeFromPreview(NodeId);
            if (Controller != null && Controller.IsNodeControlledByLayout(NodeId))
            {
                Controller.OnLayoutDragBlocked(NodeId);
                _resizeMode = false;
                return;
            }

            _resizeMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || eventData.button == PointerEventData.InputButton.Right;
            Controller?.BeginPreviewTransformChange(NodeId);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Controller != null && Controller.IsNodeControlledByLayout(NodeId))
            {
                return;
            }

            RectTransform rect = transform as RectTransform;
            if (rect == null)
            {
                return;
            }
            bool resize = _resizeMode || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || eventData.button == PointerEventData.InputButton.Right;
            if (resize)
            {
                Vector2 size = rect.sizeDelta + new Vector2(eventData.delta.x, -eventData.delta.y);
                size.x = Mathf.Max(4f, size.x);
                size.y = Mathf.Max(4f, size.y);
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    size.x = Mathf.Round(size.x / 5f) * 5f;
                    size.y = Mathf.Round(size.y / 5f) * 5f;
                }

                rect.sizeDelta = size;
                Controller?.OnPreviewResized(NodeId, rect.sizeDelta);
            }
            else
            {
                Vector2 pos = rect.anchoredPosition + eventData.delta;
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    pos.x = Mathf.Round(pos.x / 5f) * 5f;
                    pos.y = Mathf.Round(pos.y / 5f) * 5f;
                }

                rect.anchoredPosition = pos;
                Controller?.OnPreviewDragged(NodeId, rect.anchoredPosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Controller?.EndPreviewTransformChange(NodeId);
        }
    }
}
