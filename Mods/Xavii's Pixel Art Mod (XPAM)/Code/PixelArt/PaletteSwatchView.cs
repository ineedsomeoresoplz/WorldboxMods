using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiPixelArtMod
{
    internal sealed class PaletteSwatchView : MonoBehaviour, IPointerClickHandler
    {
        public PixelArtStudioController Controller;
        public int SlotIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Controller == null)
            {
                return;
            }

            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                Controller.HandleCustomPaletteSlotStore(SlotIndex);
            }
            else
            {
                Controller.HandleCustomPaletteSlotUse(SlotIndex);
            }
        }
    }
}
