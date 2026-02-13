using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiPixelArtMod
{
    internal sealed class PixelCellView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
    {
        public PixelArtStudioController Controller;
        public int X;
        public int Y;

        public void OnPointerDown(PointerEventData eventData)
        {
            Controller?.HandleCellPointerDown(X, Y, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Controller?.HandleCellPointerEnter(X, Y, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Controller?.HandleCellPointerUp(X, Y, eventData);
        }
    }
}
