using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviiPixelArtMod
{
    internal static class PixelArtUiBootstrap
    {
        private static Font _font;

        public static Font DefaultFont
        {
            get
            {
                if (_font == null)
                {
                    _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                }

                return _font;
            }
        }

        public static void EnsureEventSystem()
        {
            EventSystem system = Object.FindObjectOfType<EventSystem>();
            if (system != null)
            {
                if (system.GetComponent<StandaloneInputModule>() == null)
                {
                    system.gameObject.AddComponent<StandaloneInputModule>();
                }

                return;
            }

            GameObject eventSystemObject = new GameObject("XPAM_EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Object.DontDestroyOnLoad(eventSystemObject);
        }
    }
}