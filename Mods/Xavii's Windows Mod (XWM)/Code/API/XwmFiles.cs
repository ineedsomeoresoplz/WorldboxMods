using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using XaviiWindowsMod.Runtime;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.API
{
    public static class XwmFiles
    {
        public static bool Ready => WindowService.Instance != null && WindowService.Instance.Root != null;

        public static event Action<XwmWindowHandle> RuntimeLoaded;
        public static event Action<string> RuntimeDestroyed;

        public static XwmWindowHandle Prewarm(string modGuidOrFolder, string fileName, string runtimeId = null)
        {
            Ensure();
            if (!Ready)
            {
                return null;
            }

            string resolvedRuntimeId = string.IsNullOrWhiteSpace(runtimeId) ? BuildRuntimeId(modGuidOrFolder, fileName) : runtimeId;
            XwmWindowHandle existing = WindowService.Instance.GetRuntime(resolvedRuntimeId);
            if (existing != null)
            {
                return existing;
            }

            string filePath = XwmPathResolver.ResolveXwmFilePath(modGuidOrFolder, fileName, false);
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            XwmDocumentData document = XwmSerializer.Load(filePath);
            if (document == null)
            {
                return null;
            }

            XwmWindowHandle handle = XwmRuntimeFactory.Build(document, WindowService.Instance.Root, resolvedRuntimeId, modGuidOrFolder, fileName, false, false, null);
            if (handle == null)
            {
                return null;
            }

            WindowService.Instance.RegisterRuntime(resolvedRuntimeId, handle);
            XwmWorkspaceStateStore.Apply(handle);
            RuntimeLoaded?.Invoke(handle);
            return handle;
        }

        public static bool TryPrewarm(string modGuidOrFolder, string fileName, out XwmWindowHandle handle, string runtimeId = null)
        {
            handle = Prewarm(modGuidOrFolder, fileName, runtimeId);
            return handle != null;
        }

        public static XwmWindowHandle LoadAndShow(string modGuidOrFolder, string fileName, string runtimeId = null)
        {
            XwmWindowHandle handle = Prewarm(modGuidOrFolder, fileName, runtimeId);
            handle?.Show();
            return handle;
        }

        public static XwmWindowHandle GetOrLoad(string modGuidOrFolder, string fileName, string runtimeId = null, bool show = false)
        {
            string resolvedRuntimeId = string.IsNullOrWhiteSpace(runtimeId) ? BuildRuntimeId(modGuidOrFolder, fileName) : runtimeId;
            XwmWindowHandle existing = Get(resolvedRuntimeId);
            if (existing != null)
            {
                if (show)
                {
                    existing.Show();
                }

                return existing;
            }

            XwmWindowHandle created = Prewarm(modGuidOrFolder, fileName, resolvedRuntimeId);
            if (show)
            {
                created?.Show();
            }

            return created;
        }

        public static XwmWindowHandle LoadFromPath(string filePath, string runtimeId = null, bool show = false)
        {
            Ensure();
            if (!Ready || string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(filePath);
            }
            catch
            {
                return null;
            }

            if (!File.Exists(fullPath))
            {
                return null;
            }

            XwmDocumentData document = XwmSerializer.Load(fullPath);
            if (document == null)
            {
                return null;
            }

            string modTarget = ResolveModTargetFromXwmPath(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string resolvedRuntimeId = string.IsNullOrWhiteSpace(runtimeId) ? BuildRuntimeId(modTarget, fileName) : runtimeId;

            XwmWindowHandle existing = Get(resolvedRuntimeId);
            if (existing != null)
            {
                if (show)
                {
                    existing.Show();
                }

                return existing;
            }

            XwmWindowHandle handle = XwmRuntimeFactory.Build(document, WindowService.Instance.Root, resolvedRuntimeId, modTarget, fileName, show, false, null);
            if (handle == null)
            {
                return null;
            }

            WindowService.Instance.RegisterRuntime(resolvedRuntimeId, handle);
            XwmWorkspaceStateStore.Apply(handle);
            if (show)
            {
                handle.Show();
            }

            RuntimeLoaded?.Invoke(handle);
            return handle;
        }

        public static bool TryLoadFromPath(string filePath, out XwmWindowHandle handle, string runtimeId = null, bool show = false)
        {
            handle = LoadFromPath(filePath, runtimeId, show);
            return handle != null;
        }

        public static XwmWindowHandle Show(string runtimeId)
        {
            XwmWindowHandle handle = Get(runtimeId);
            handle?.Show();
            return handle;
        }

        public static int ShowAll()
        {
            IReadOnlyCollection<XwmWindowHandle> handles = All();
            int shown = 0;
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null)
                {
                    continue;
                }

                handle.Show();
                shown++;
            }

            return shown;
        }

        public static int ShowByMod(string modGuidOrFolder)
        {
            IReadOnlyCollection<XwmWindowHandle> handles = AllByMod(modGuidOrFolder);
            int shown = 0;
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null)
                {
                    continue;
                }

                handle.Show();
                shown++;
            }

            return shown;
        }

        public static bool Hide(string runtimeId)
        {
            XwmWindowHandle handle = Get(runtimeId);
            if (handle == null)
            {
                return false;
            }

            handle.Hide();
            return true;
        }

        public static int HideAll()
        {
            IReadOnlyCollection<XwmWindowHandle> handles = All();
            int hidden = 0;
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null)
                {
                    continue;
                }

                handle.Hide();
                hidden++;
            }

            return hidden;
        }

        public static int HideByMod(string modGuidOrFolder)
        {
            IReadOnlyCollection<XwmWindowHandle> handles = AllByMod(modGuidOrFolder);
            int hidden = 0;
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null)
                {
                    continue;
                }

                handle.Hide();
                hidden++;
            }

            return hidden;
        }

        public static bool Destroy(string runtimeId)
        {
            XwmWindowHandle handle = Get(runtimeId);
            if (handle == null)
            {
                return false;
            }

            XwmWorkspaceStateStore.Capture(handle);
            XwmWorkspaceStateStore.Save(false);
            handle.Destroy();
            RuntimeDestroyed?.Invoke(runtimeId);
            return true;
        }

        public static int DestroyAll()
        {
            IReadOnlyCollection<XwmWindowHandle> handles = All();
            List<XwmWindowHandle> snapshot = new List<XwmWindowHandle>(handles);
            int destroyed = 0;
            for (int i = 0; i < snapshot.Count; i++)
            {
                XwmWindowHandle handle = snapshot[i];
                if (handle == null)
                {
                    continue;
                }

                string runtimeId = handle.RuntimeId;
                XwmWorkspaceStateStore.Capture(handle);
                handle.Destroy();
                RuntimeDestroyed?.Invoke(runtimeId);
                destroyed++;
            }

            XwmWorkspaceStateStore.Save(false);
            return destroyed;
        }

        public static int DestroyByMod(string modGuidOrFolder)
        {
            IReadOnlyCollection<XwmWindowHandle> handles = AllByMod(modGuidOrFolder);
            List<XwmWindowHandle> snapshot = new List<XwmWindowHandle>(handles);
            int destroyed = 0;
            for (int i = 0; i < snapshot.Count; i++)
            {
                XwmWindowHandle handle = snapshot[i];
                if (handle == null)
                {
                    continue;
                }

                string runtimeId = handle.RuntimeId;
                XwmWorkspaceStateStore.Capture(handle);
                handle.Destroy();
                RuntimeDestroyed?.Invoke(runtimeId);
                destroyed++;
            }

            XwmWorkspaceStateStore.Save(false);
            return destroyed;
        }

        public static XwmWindowHandle Reload(string runtimeId, bool show = false)
        {
            XwmWindowHandle existing = Get(runtimeId);
            if (existing == null)
            {
                return null;
            }

            string modTarget = existing.ModTarget;
            string fileName = existing.FileName;
            existing.Destroy();
            RuntimeDestroyed?.Invoke(runtimeId);
            XwmWindowHandle handle = Prewarm(modTarget, fileName, runtimeId);
            if (show)
            {
                handle?.Show();
            }

            return handle;
        }

        public static XwmWindowHandle Reload(string modGuidOrFolder, string fileName, string runtimeId = null, bool show = false)
        {
            string resolvedRuntimeId = string.IsNullOrWhiteSpace(runtimeId) ? BuildRuntimeId(modGuidOrFolder, fileName) : runtimeId;
            Destroy(resolvedRuntimeId);
            XwmWindowHandle handle = Prewarm(modGuidOrFolder, fileName, resolvedRuntimeId);
            if (show)
            {
                handle?.Show();
            }

            return handle;
        }

        public static int ReloadAllLoaded(bool keepVisibility = true)
        {
            IReadOnlyCollection<XwmWindowHandle> handles = All();
            List<XwmWindowHandle> snapshot = new List<XwmWindowHandle>(handles);
            int reloaded = 0;

            for (int i = 0; i < snapshot.Count; i++)
            {
                XwmWindowHandle handle = snapshot[i];
                if (handle == null || handle.IsDestroyed)
                {
                    continue;
                }

                bool show = keepVisibility && handle.IsVisible;
                XwmWindowHandle reloadedHandle = Reload(handle.RuntimeId, show);
                if (reloadedHandle != null)
                {
                    reloaded++;
                }
            }

            return reloaded;
        }

        public static XwmWindowHandle Get(string runtimeId)
        {
            if (!Ready || string.IsNullOrWhiteSpace(runtimeId))
            {
                return null;
            }

            return WindowService.Instance.GetRuntime(runtimeId);
        }

        public static XwmWindowHandle FindRuntime(string modGuidOrFolder, string fileName)
        {
            if (!Ready || string.IsNullOrWhiteSpace(modGuidOrFolder) || string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string normalizedFile = Path.GetFileNameWithoutExtension(fileName.Trim());
            IReadOnlyCollection<XwmWindowHandle> handles = All();
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null || handle.IsDestroyed)
                {
                    continue;
                }

                if (!MatchesMod(handle, modGuidOrFolder))
                {
                    continue;
                }

                string candidate = Path.GetFileNameWithoutExtension(handle.FileName ?? string.Empty);
                if (string.Equals(candidate, normalizedFile, StringComparison.OrdinalIgnoreCase))
                {
                    return handle;
                }
            }

            return null;
        }

        public static bool ExistsRuntime(string runtimeId)
        {
            return Get(runtimeId) != null;
        }

        public static IReadOnlyCollection<XwmWindowHandle> All()
        {
            if (!Ready)
            {
                return new List<XwmWindowHandle>();
            }

            return WindowService.Instance.GetAllRuntimes();
        }

        public static IReadOnlyCollection<XwmWindowHandle> AllByMod(string modGuidOrFolder)
        {
            List<XwmWindowHandle> filtered = new List<XwmWindowHandle>();
            if (!Ready || string.IsNullOrWhiteSpace(modGuidOrFolder))
            {
                return filtered;
            }

            IReadOnlyCollection<XwmWindowHandle> all = All();
            foreach (XwmWindowHandle handle in all)
            {
                if (handle == null)
                {
                    continue;
                }

                if (MatchesMod(handle, modGuidOrFolder))
                {
                    filtered.Add(handle);
                }
            }

            return filtered;
        }

        public static IReadOnlyCollection<string> RuntimeIds()
        {
            if (!Ready)
            {
                return new List<string>();
            }

            List<string> ids = new List<string>();
            foreach (XwmWindowHandle handle in WindowService.Instance.GetAllRuntimes())
            {
                if (handle == null || string.IsNullOrWhiteSpace(handle.RuntimeId))
                {
                    continue;
                }

                ids.Add(handle.RuntimeId);
            }

            return ids;
        }

        public static bool Exists(string modGuidOrFolder, string fileName)
        {
            string path = XwmPathResolver.ResolveXwmFilePath(modGuidOrFolder, fileName, false);
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
        }

        public static string ResolvePath(string modGuidOrFolder, string fileName, bool createFolder = false)
        {
            return XwmPathResolver.ResolveXwmFilePath(modGuidOrFolder, fileName, createFolder);
        }

        public static string ResolveDirectory(string modGuidOrFolder, bool create = false)
        {
            return XwmPathResolver.ResolveXwmDirectory(modGuidOrFolder, create);
        }

        public static string ResolveModFolder(string modGuidOrFolder)
        {
            return XwmPathResolver.ResolveModFolder(modGuidOrFolder);
        }

        public static IReadOnlyList<string> ListFiles(string modGuidOrFolder, bool includeExtension = false)
        {
            string directory = ResolveDirectory(modGuidOrFolder, false);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return new List<string>();
            }

            string[] files = Directory.GetFiles(directory, "*.xwm", SearchOption.TopDirectoryOnly);
            List<string> names = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string name = includeExtension ? Path.GetFileName(file) : Path.GetFileNameWithoutExtension(file);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    names.Add(name);
                }
            }

            names.Sort(StringComparer.OrdinalIgnoreCase);
            return names;
        }

        public static IReadOnlyList<string> ListFilePaths(string modGuidOrFolder)
        {
            string directory = ResolveDirectory(modGuidOrFolder, false);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return new List<string>();
            }

            string[] files = Directory.GetFiles(directory, "*.xwm", SearchOption.TopDirectoryOnly);
            return files.OrderBy(path => path, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public static IReadOnlyList<string> ListModTargets(bool onlyWithXwm = true)
        {
            List<string> mods = new List<string>();
            string root = XwmPathResolver.ModsRoot;
            if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
            {
                return mods;
            }

            string[] directories = Directory.GetDirectories(root);
            for (int i = 0; i < directories.Length; i++)
            {
                string directory = directories[i];
                string name = Path.GetFileName(directory);
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                if (onlyWithXwm)
                {
                    string xwmFolder = Path.Combine(directory, "XWM");
                    if (!Directory.Exists(xwmFolder))
                    {
                        continue;
                    }
                }

                mods.Add(name);
            }

            mods.Sort(StringComparer.OrdinalIgnoreCase);
            return mods;
        }

        public static IReadOnlyList<XwmFileDescriptor> ListAllFiles(bool includeExtension = false)
        {
            List<XwmFileDescriptor> files = new List<XwmFileDescriptor>();
            IReadOnlyList<string> mods = ListModTargets(true);
            for (int i = 0; i < mods.Count; i++)
            {
                string mod = mods[i];
                IReadOnlyList<string> names = ListFiles(mod, includeExtension);
                for (int n = 0; n < names.Count; n++)
                {
                    string fileName = names[n];
                    string normalized = includeExtension ? Path.GetFileNameWithoutExtension(fileName) : fileName;
                    files.Add(new XwmFileDescriptor
                    {
                        ModTarget = mod,
                        FileName = includeExtension ? fileName : normalized,
                        FullPath = ResolvePath(mod, normalized, false),
                        RuntimeId = BuildRuntimeId(mod, normalized)
                    });
                }
            }

            files = files.OrderBy(file => file.ModTarget, StringComparer.OrdinalIgnoreCase)
                .ThenBy(file => file.FileName, StringComparer.OrdinalIgnoreCase)
                .ToList();
            return files;
        }

        public static bool DeleteFile(string modGuidOrFolder, string fileName)
        {
            string path = ResolvePath(modGuidOrFolder, fileName, false);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        public static bool CopyFile(string modGuidOrFolder, string fileName, string destinationFileName, bool overwrite = false)
        {
            string source = ResolvePath(modGuidOrFolder, fileName, false);
            if (string.IsNullOrWhiteSpace(source) || !File.Exists(source))
            {
                return false;
            }

            string destination = ResolvePath(modGuidOrFolder, destinationFileName, true);
            if (string.IsNullOrWhiteSpace(destination))
            {
                return false;
            }

            if (File.Exists(destination) && !overwrite)
            {
                return false;
            }

            File.Copy(source, destination, overwrite);
            return true;
        }

        public static bool RenameFile(string modGuidOrFolder, string oldFileName, string newFileName, bool overwrite = false)
        {
            string source = ResolvePath(modGuidOrFolder, oldFileName, false);
            if (string.IsNullOrWhiteSpace(source) || !File.Exists(source))
            {
                return false;
            }

            string destination = ResolvePath(modGuidOrFolder, newFileName, true);
            if (string.IsNullOrWhiteSpace(destination))
            {
                return false;
            }

            if (File.Exists(destination))
            {
                if (!overwrite)
                {
                    return false;
                }

                File.Delete(destination);
            }

            File.Move(source, destination);
            return true;
        }

        public static int PrewarmAll(string modGuidOrFolder, bool show = false, string runtimePrefix = null)
        {
            IReadOnlyList<string> files = ListFiles(modGuidOrFolder, false);
            int loaded = 0;
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string runtimeId = string.IsNullOrWhiteSpace(runtimePrefix)
                    ? BuildRuntimeId(modGuidOrFolder, file)
                    : runtimePrefix + "::" + file;
                XwmWindowHandle handle = Prewarm(modGuidOrFolder, file, runtimeId);
                if (handle == null)
                {
                    continue;
                }

                if (show)
                {
                    handle.Show();
                }

                loaded++;
            }

            return loaded;
        }

        public static string BuildRuntimeId(string modGuidOrFolder, string fileName)
        {
            string left = string.IsNullOrWhiteSpace(modGuidOrFolder) ? "self" : modGuidOrFolder.Trim();
            string right = string.IsNullOrWhiteSpace(fileName) ? "window" : fileName.Trim();
            return left + "::" + right;
        }

        private static void Ensure()
        {
            if (WindowService.Instance != null)
            {
                return;
            }

            GameObject host = new GameObject("XWM_Autobootstrap");
            host.AddComponent<WindowService>();
        }

        private static bool MatchesMod(XwmWindowHandle handle, string modGuidOrFolder)
        {
            if (handle == null || string.IsNullOrWhiteSpace(modGuidOrFolder))
            {
                return false;
            }

            if (string.Equals(handle.ModTarget, modGuidOrFolder, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string requestedFolder = XwmPathResolver.ResolveModFolder(modGuidOrFolder);
            if (string.IsNullOrWhiteSpace(requestedFolder))
            {
                return false;
            }

            string requestedName = Path.GetFileName(requestedFolder);
            if (string.Equals(handle.ModTarget, requestedName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string handleFolder = XwmPathResolver.ResolveModFolder(handle.ModTarget);
            if (string.IsNullOrWhiteSpace(handleFolder))
            {
                return false;
            }

            return string.Equals(handleFolder, requestedFolder, StringComparison.OrdinalIgnoreCase);
        }

        private static string ResolveModTargetFromXwmPath(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                return "self";
            }

            try
            {
                string normalized = Path.GetFullPath(fullPath);
                string root = XwmPathResolver.ModsRoot;
                if (string.IsNullOrWhiteSpace(root))
                {
                    return "self";
                }

                string rootNormalized = Path.GetFullPath(root);
                if (!normalized.StartsWith(rootNormalized, StringComparison.OrdinalIgnoreCase))
                {
                    return "self";
                }

                string relative = normalized.Substring(rootNormalized.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                int separatorIndex = relative.IndexOf(Path.DirectorySeparatorChar);
                if (separatorIndex < 0)
                {
                    separatorIndex = relative.IndexOf(Path.AltDirectorySeparatorChar);
                }

                if (separatorIndex <= 0)
                {
                    return "self";
                }

                return relative.Substring(0, separatorIndex);
            }
            catch
            {
                return "self";
            }
        }
    }
}
