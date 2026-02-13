using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XaviiWindowsMod.API;
using XaviiWindowsMod.Runtime;
using XaviiWindowsMod.Xwm;
using XaviiWindowsMod.Xwm.Studio;

namespace XaviiWindowsMod
{
    internal class WindowService : MonoBehaviour
    {
        public static WindowService Instance { get; private set; }

        public RectTransform Root { get; private set; }
        public Dictionary<string, WindowInstance> Registry { get; } = new Dictionary<string, WindowInstance>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, XwmWindowHandle> _runtimes = new Dictionary<string, XwmWindowHandle>(StringComparer.OrdinalIgnoreCase);
        private Canvas _canvas;
        private XwmStudioController _studio;
        private XwmWorkspaceStateRunner _workspaceStateRunner;
        private XwmRuntimeHubController _runtimeHubController;
        private XwmRuntimeHotkeys _runtimeHotkeys;
        private XwmAutoloadRunner _autoloadRunner;

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
            EnsureRuntimeSuite();
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

            GameObject existing = GameObject.Find("XWM_Root");
            if (existing != null)
            {
                Root = existing.GetComponent<RectTransform>();
                _canvas = existing.GetComponent<Canvas>();
            }

            if (Root == null)
            {
                GameObject rootObject = new GameObject("XWM_Root", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
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
                _canvas.sortingOrder = 220;
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

            XwmUiBootstrap.EnsureEventSystem();
        }

        private void EnsureStudio()
        {
            if (_studio != null)
            {
                return;
            }

            _studio = gameObject.GetComponent<XwmStudioController>();
            if (_studio == null)
            {
                _studio = gameObject.AddComponent<XwmStudioController>();
            }
        }

        private void EnsureRuntimeSuite()
        {
            _workspaceStateRunner = gameObject.GetComponent<XwmWorkspaceStateRunner>();
            if (_workspaceStateRunner == null)
            {
                _workspaceStateRunner = gameObject.AddComponent<XwmWorkspaceStateRunner>();
            }

            _runtimeHubController = gameObject.GetComponent<XwmRuntimeHubController>();
            if (_runtimeHubController == null)
            {
                _runtimeHubController = gameObject.AddComponent<XwmRuntimeHubController>();
            }

            _runtimeHotkeys = gameObject.GetComponent<XwmRuntimeHotkeys>();
            if (_runtimeHotkeys == null)
            {
                _runtimeHotkeys = gameObject.AddComponent<XwmRuntimeHotkeys>();
            }

            _autoloadRunner = gameObject.GetComponent<XwmAutoloadRunner>();
            if (_autoloadRunner == null)
            {
                _autoloadRunner = gameObject.AddComponent<XwmAutoloadRunner>();
            }
        }

        public WindowInstance Create(string id, string title, Vector2 size, bool focusOnShow)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            EnsureRoot();
            if (Registry.TryGetValue(id, out WindowInstance existing))
            {
                existing.Destroy();
                Registry.Remove(id);
            }

            WindowInstance instance = WindowFactory.BuildWindow(Root, id, title, size);
            Registry[id] = instance;

            if (focusOnShow)
            {
                BringToFront(instance);
            }

            return instance;
        }

        public WindowInstance Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return Registry.TryGetValue(id, out WindowInstance instance) ? instance : null;
        }

        public void Close(string id, bool destroy)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            if (!Registry.TryGetValue(id, out WindowInstance instance))
            {
                return;
            }

            if (destroy)
            {
                instance.Destroy();
                Registry.Remove(id);
            }
            else
            {
                instance.Hide();
            }
        }

        public void CloseAll(bool destroy)
        {
            List<string> keys = new List<string>(Registry.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                Close(keys[i], destroy);
            }
        }

        public void BringToFront(WindowInstance instance)
        {
            if (instance == null || instance.RootRect == null)
            {
                return;
            }

            instance.RootRect.SetAsLastSibling();
        }

        public void RegisterRuntime(string id, XwmWindowHandle handle)
        {
            if (string.IsNullOrWhiteSpace(id) || handle == null)
            {
                return;
            }

            _runtimes[id] = handle;
        }

        public void UnregisterRuntime(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            _runtimes.Remove(id);
        }

        public XwmWindowHandle GetRuntime(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _runtimes.TryGetValue(id, out XwmWindowHandle handle) ? handle : null;
        }

        public IReadOnlyCollection<XwmWindowHandle> GetAllRuntimes()
        {
            return _runtimes.Values;
        }
    }
}
