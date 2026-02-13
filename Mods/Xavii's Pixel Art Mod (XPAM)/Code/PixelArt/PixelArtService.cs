using UnityEngine;
using UnityEngine.UI;

namespace XaviiPixelArtMod
{
    internal class PixelArtService : MonoBehaviour
    {
        public static PixelArtService Instance { get; private set; }

        public RectTransform Root { get; private set; }

        private Canvas _canvas;
        private PixelArtStudioController _studio;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureRoot();
            EnsureStudio();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void EnsureRoot()
        {
            if (Root != null)
            {
                return;
            }

            GameObject existing = GameObject.Find("XPAM_Root");
            if (existing != null)
            {
                Root = existing.GetComponent<RectTransform>();
                _canvas = existing.GetComponent<Canvas>();
            }

            if (Root == null)
            {
                GameObject rootObject = new GameObject("XPAM_Root", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                rootObject.transform.SetParent(transform, false);
                Root = rootObject.GetComponent<RectTransform>();
                _canvas = rootObject.GetComponent<Canvas>();
            }

            if (_canvas == null && Root != null)
            {
                _canvas = Root.gameObject.AddComponent<Canvas>();
            }

            if (Root != null && Root.GetComponent<GraphicRaycaster>() == null)
            {
                Root.gameObject.AddComponent<GraphicRaycaster>();
            }

            if (_canvas != null)
            {
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _canvas.sortingOrder = 230;
            }

            CanvasScaler scaler = Root.GetComponent<CanvasScaler>();
            if (scaler == null && Root != null)
            {
                scaler = Root.gameObject.AddComponent<CanvasScaler>();
            }

            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.matchWidthOrHeight = 0.5f;
            }

            Root.anchorMin = Vector2.zero;
            Root.anchorMax = Vector2.one;
            Root.offsetMin = Vector2.zero;
            Root.offsetMax = Vector2.zero;

            PixelArtUiBootstrap.EnsureEventSystem();
        }

        private void EnsureStudio()
        {
            if (_studio != null)
            {
                return;
            }

            _studio = gameObject.GetComponent<PixelArtStudioController>();
            if (_studio == null)
            {
                _studio = gameObject.AddComponent<PixelArtStudioController>();
            }
        }
    }
}