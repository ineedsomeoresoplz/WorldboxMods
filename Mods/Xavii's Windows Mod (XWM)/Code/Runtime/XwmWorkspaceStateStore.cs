using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using XaviiWindowsMod.API;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmWorkspaceStateDocument
    {
        public string version = "1.0.0";
        public string updatedAtUtc = DateTime.UtcNow.ToString("o");
        public List<XwmWorkspaceRuntimeState> runtimes = new List<XwmWorkspaceRuntimeState>();
    }

    internal sealed class XwmWorkspaceRuntimeState
    {
        public string runtimeId;
        public string modTarget;
        public string fileName;
        public bool visible;
        public float x;
        public float y;
        public float width;
        public float height;
        public float opacity;
        public float scaleX;
        public float scaleY;
        public float rotation;
        public int siblingIndex;
    }

    internal static class XwmWorkspaceStateStore
    {
        private static readonly Dictionary<string, XwmWorkspaceRuntimeState> States = new Dictionary<string, XwmWorkspaceRuntimeState>(StringComparer.OrdinalIgnoreCase);
        private static bool _loaded;
        private static string _lastSerialized;

        public static string FilePath
        {
            get
            {
                string folder = Path.Combine(XwmPathResolver.ThisModFolder, "XWM");
                Directory.CreateDirectory(folder);
                return Path.Combine(folder, "workspace_state.json");
            }
        }

        public static void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            Load();
        }

        public static bool Load()
        {
            States.Clear();
            _loaded = true;

            string path = FilePath;
            if (!File.Exists(path))
            {
                _lastSerialized = null;
                return false;
            }

            try
            {
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    _lastSerialized = string.Empty;
                    return false;
                }

                XwmWorkspaceStateDocument document = JsonConvert.DeserializeObject<XwmWorkspaceStateDocument>(json);
                if (document == null || document.runtimes == null)
                {
                    _lastSerialized = json;
                    return false;
                }

                for (int i = 0; i < document.runtimes.Count; i++)
                {
                    XwmWorkspaceRuntimeState state = document.runtimes[i];
                    if (state == null || string.IsNullOrWhiteSpace(state.runtimeId))
                    {
                        continue;
                    }

                    States[state.runtimeId] = state;
                }

                _lastSerialized = json;
                return States.Count > 0;
            }
            catch
            {
                _lastSerialized = null;
                return false;
            }
        }

        public static bool Save(bool force)
        {
            EnsureLoaded();

            XwmWorkspaceStateDocument document = new XwmWorkspaceStateDocument();
            document.updatedAtUtc = DateTime.UtcNow.ToString("o");
            document.runtimes = States.Values.OrderBy(state => state.runtimeId, StringComparer.OrdinalIgnoreCase).ToList();

            try
            {
                string json = JsonConvert.SerializeObject(document, Formatting.Indented);
                if (!force && string.Equals(_lastSerialized, json, StringComparison.Ordinal))
                {
                    return false;
                }

                File.WriteAllText(FilePath, json);
                _lastSerialized = json;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Capture(XwmWindowHandle handle)
        {
            EnsureLoaded();
            if (handle == null || handle.IsDestroyed || string.IsNullOrWhiteSpace(handle.RuntimeId))
            {
                return;
            }

            XwmWorkspaceRuntimeState state = new XwmWorkspaceRuntimeState
            {
                runtimeId = handle.RuntimeId,
                modTarget = handle.ModTarget,
                fileName = handle.FileName,
                visible = handle.IsVisible,
                x = handle.Position.x,
                y = handle.Position.y,
                width = handle.Size.x,
                height = handle.Size.y,
                opacity = handle.Opacity,
                scaleX = handle.RootRect != null ? handle.RootRect.localScale.x : 1f,
                scaleY = handle.RootRect != null ? handle.RootRect.localScale.y : 1f,
                rotation = handle.RootRect != null ? handle.RootRect.localEulerAngles.z : 0f,
                siblingIndex = handle.RootRect != null ? handle.RootRect.GetSiblingIndex() : 0
            };

            States[state.runtimeId] = state;
        }

        public static void CaptureAll()
        {
            EnsureLoaded();
            IReadOnlyCollection<XwmWindowHandle> handles = XwmFiles.All();
            foreach (XwmWindowHandle handle in handles)
            {
                Capture(handle);
            }
        }

        public static void ApplyAllLoaded()
        {
            EnsureLoaded();
            IReadOnlyCollection<XwmWindowHandle> handles = XwmFiles.All();
            foreach (XwmWindowHandle handle in handles)
            {
                Apply(handle);
            }
        }

        public static bool Apply(XwmWindowHandle handle)
        {
            EnsureLoaded();
            if (handle == null || handle.IsDestroyed || string.IsNullOrWhiteSpace(handle.RuntimeId))
            {
                return false;
            }

            if (!States.TryGetValue(handle.RuntimeId, out XwmWorkspaceRuntimeState state) || state == null)
            {
                return false;
            }

            if (state.width > 1f && state.height > 1f)
            {
                handle.Size = new Vector2(state.width, state.height);
            }

            handle.Position = new Vector2(state.x, state.y);
            handle.Opacity = Mathf.Clamp01(state.opacity);

            if (handle.RootRect != null)
            {
                handle.RootRect.localScale = new Vector3(Mathf.Max(0.01f, state.scaleX), Mathf.Max(0.01f, state.scaleY), 1f);
                handle.RootRect.localEulerAngles = new Vector3(0f, 0f, state.rotation);

                Transform parent = handle.RootRect.parent;
                if (parent != null)
                {
                    int maxIndex = Mathf.Max(0, parent.childCount - 1);
                    int targetIndex = Mathf.Clamp(state.siblingIndex, 0, maxIndex);
                    handle.RootRect.SetSiblingIndex(targetIndex);
                }
            }

            if (state.visible)
            {
                handle.Show();
            }
            else
            {
                handle.Hide();
            }

            return true;
        }

        public static void Remove(string runtimeId)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(runtimeId))
            {
                return;
            }

            States.Remove(runtimeId);
        }
    }
}
