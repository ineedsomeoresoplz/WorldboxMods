using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XaviiWindowsMod.API
{
    public static class WindowSystem
    {
        public static bool Ready => XaviiWindowsMod.WindowService.Instance != null && XaviiWindowsMod.WindowService.Instance.Root != null;

        public static WindowInstance Create(string id, string title, Vector2? size = null, bool focusOnShow = true)
        {
            Ensure();
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return null;
            }

            Vector2 resolvedSize = size ?? new Vector2(220, 260);
            return XaviiWindowsMod.WindowService.Instance.Create(id, title, resolvedSize, focusOnShow);
        }

        public static WindowInstance GetOrCreate(string id, string title, Vector2? size = null, bool focusOnShow = true)
        {
            WindowInstance existing = Get(id);
            if (existing != null)
            {
                return existing;
            }

            return Create(id, title, size, focusOnShow);
        }

        public static WindowInstance Get(string id)
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return null;
            }

            return XaviiWindowsMod.WindowService.Instance.Get(id);
        }

        public static bool Exists(string id)
        {
            return Get(id) != null;
        }

        public static IReadOnlyCollection<WindowInstance> All()
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return new List<WindowInstance>();
            }

            return XaviiWindowsMod.WindowService.Instance.Registry.Values.ToList();
        }

        public static int Count()
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return 0;
            }

            return XaviiWindowsMod.WindowService.Instance.Registry.Count;
        }

        public static void Close(string id, bool destroy = true)
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return;
            }

            XaviiWindowsMod.WindowService.Instance.Close(id, destroy);
        }

        public static void CloseAll(bool destroy = true)
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return;
            }

            XaviiWindowsMod.WindowService.Instance.CloseAll(destroy);
        }

        public static bool Show(string id, bool bringToFront = true)
        {
            WindowInstance instance = Get(id);
            if (instance == null)
            {
                return false;
            }

            instance.Show();
            if (bringToFront)
            {
                BringToFront(instance);
            }

            return true;
        }

        public static bool Hide(string id)
        {
            WindowInstance instance = Get(id);
            if (instance == null)
            {
                return false;
            }

            instance.Hide();
            return true;
        }

        public static bool Toggle(string id, bool bringToFront = true)
        {
            WindowInstance instance = Get(id);
            if (instance == null)
            {
                return false;
            }

            bool nextVisible = !instance.IsVisible;
            if (nextVisible)
            {
                instance.Show();
                if (bringToFront)
                {
                    BringToFront(instance);
                }
            }
            else
            {
                instance.Hide();
            }

            return nextVisible;
        }

        public static void BringToFront(WindowInstance instance)
        {
            if (XaviiWindowsMod.WindowService.Instance == null)
            {
                return;
            }

            XaviiWindowsMod.WindowService.Instance.BringToFront(instance);
        }

        public static bool BringToFront(string id)
        {
            WindowInstance instance = Get(id);
            if (instance == null)
            {
                return false;
            }

            BringToFront(instance);
            return true;
        }

        public static bool SendToBack(string id)
        {
            WindowInstance instance = Get(id);
            if (instance == null || instance.RootRect == null)
            {
                return false;
            }

            instance.RootRect.SetAsFirstSibling();
            return true;
        }

        private static void Ensure()
        {
            if (XaviiWindowsMod.WindowService.Instance != null)
            {
                return;
            }

            GameObject host = new GameObject("XWM_Autobootstrap");
            host.AddComponent<XaviiWindowsMod.WindowService>();
        }
    }
}
