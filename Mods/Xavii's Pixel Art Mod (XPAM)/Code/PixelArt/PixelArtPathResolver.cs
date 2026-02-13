using System;
using System.IO;
using System.Reflection;
using NeoModLoader.constants;
using UnityEngine;

namespace XaviiPixelArtMod
{
    internal static class PixelArtPathResolver
    {
        private static string _modsRootCache;
        private static string _thisModFolderCache;

        public static string ModsRoot
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_modsRootCache) && Directory.Exists(_modsRootCache))
                {
                    return _modsRootCache;
                }

                if (!string.IsNullOrWhiteSpace(Paths.ModsPath) && Directory.Exists(Paths.ModsPath))
                {
                    _modsRootCache = Paths.ModsPath;
                    return _modsRootCache;
                }

                _modsRootCache = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Mods"));
                return _modsRootCache;
            }
        }

        public static string ThisModFolder
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_thisModFolderCache) && Directory.Exists(_thisModFolderCache))
                {
                    return _thisModFolderCache;
                }

                try
                {
                    string location = Assembly.GetExecutingAssembly().Location;
                    string directory = Path.GetDirectoryName(location);
                    if (!string.IsNullOrWhiteSpace(directory))
                    {
                        DirectoryInfo info = new DirectoryInfo(directory);
                        while (info != null)
                        {
                            if (File.Exists(Path.Combine(info.FullName, "mod.json")))
                            {
                                _thisModFolderCache = info.FullName;
                                return _thisModFolderCache;
                            }

                            info = info.Parent;
                        }
                    }
                }
                catch
                {
                }

                _thisModFolderCache = Path.Combine(ModsRoot, "Xavii's Pixel Art Mod (XPAM)");
                return _thisModFolderCache;
            }
        }

        public static string ResolveExportsDirectory(bool create)
        {
            string path = Path.Combine(ThisModFolder, "Exports");
            if (create)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string ResolvePresetCloneDirectory(bool create)
        {
            string path = Path.Combine(ThisModFolder, "Presets", "VanillaClones");
            if (create)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string ResolveImportsDirectory(bool create)
        {
            string path = Path.Combine(ThisModFolder, "Imports");
            if (create)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string ResolveProjectsDirectory(bool create)
        {
            string path = Path.Combine(ThisModFolder, "Projects");
            if (create)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string ResolvePaletteDirectory(bool create)
        {
            string path = Path.Combine(ThisModFolder, "Palettes");
            if (create)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string SanitizeBaseName(string input, string fallback)
        {
            string name = string.IsNullOrWhiteSpace(input) ? fallback : input.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = fallback;
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            for (int i = 0; i < invalid.Length; i++)
            {
                name = name.Replace(invalid[i].ToString(), "_");
            }

            return name;
        }

        public static string EnsureExtension(string fileName, string extension)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "file";
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                return fileName;
            }

            if (!extension.StartsWith(".", StringComparison.Ordinal))
            {
                extension = "." + extension;
            }

            if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            {
                fileName += extension;
            }

            return fileName;
        }

        public static string SanitizeFileName(string input)
        {
            string name = SanitizeBaseName(input, "sprite");
            return EnsureExtension(name, ".png");
        }
    }
}
