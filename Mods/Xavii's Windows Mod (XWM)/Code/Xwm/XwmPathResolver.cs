using System;
using System.IO;
using System.Reflection;
using NeoModLoader.constants;
using UnityEngine;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmPathResolver
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

                string fallback = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Mods"));
                _modsRootCache = fallback;
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
                            string modJson = Path.Combine(info.FullName, "mod.json");
                            if (File.Exists(modJson))
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

                string fallback = Path.Combine(ModsRoot, "Xavii's Windows Mod (XWM)");
                _thisModFolderCache = fallback;
                return _thisModFolderCache;
            }
        }

        public static string ResolveModFolder(string modGuidOrFolder)
        {
            if (string.IsNullOrWhiteSpace(modGuidOrFolder))
            {
                return ThisModFolder;
            }

            string query = modGuidOrFolder.Trim();
            if (Path.IsPathRooted(query) && Directory.Exists(query))
            {
                return query;
            }

            string direct = Path.Combine(ModsRoot, query);
            if (Directory.Exists(direct))
            {
                return direct;
            }

            try
            {
                string[] directories = Directory.GetDirectories(ModsRoot);
                for (int i = 0; i < directories.Length; i++)
                {
                    string folder = directories[i];
                    string folderName = Path.GetFileName(folder);
                    if (string.Equals(folderName, query, StringComparison.OrdinalIgnoreCase))
                    {
                        return folder;
                    }

                    string modJsonPath = Path.Combine(folder, "mod.json");
                    if (!File.Exists(modJsonPath))
                    {
                        continue;
                    }

                    string json = File.ReadAllText(modJsonPath);
                    if (TryExtractGuid(json, out string guid) && string.Equals(guid, query, StringComparison.OrdinalIgnoreCase))
                    {
                        return folder;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static string ResolveXwmDirectory(string modGuidOrFolder, bool create)
        {
            string modFolder = ResolveModFolder(modGuidOrFolder);
            if (string.IsNullOrWhiteSpace(modFolder))
            {
                return null;
            }

            string xwmFolder = Path.Combine(modFolder, "XWM");
            if (create)
            {
                Directory.CreateDirectory(xwmFolder);
            }

            return xwmFolder;
        }

        public static string ResolveXwmFilePath(string modGuidOrFolder, string fileName, bool createDirectory)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string xwmFolder = ResolveXwmDirectory(modGuidOrFolder, createDirectory);
            if (string.IsNullOrWhiteSpace(xwmFolder))
            {
                return null;
            }

            string normalized = SanitizeFileName(XwmSerializer.EnsureExtension(fileName));
            return Path.Combine(xwmFolder, normalized);
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "untitled.xwm";
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            string output = fileName;
            for (int i = 0; i < invalid.Length; i++)
            {
                output = output.Replace(invalid[i].ToString(), "_");
            }

            return output;
        }

        private static bool TryExtractGuid(string json, out string guid)
        {
            guid = null;
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            const string key = "\"GUID\"";
            int keyIndex = json.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (keyIndex < 0)
            {
                return false;
            }

            int colonIndex = json.IndexOf(':', keyIndex);
            if (colonIndex < 0)
            {
                return false;
            }

            int firstQuote = json.IndexOf('"', colonIndex + 1);
            if (firstQuote < 0)
            {
                return false;
            }

            int secondQuote = json.IndexOf('"', firstQuote + 1);
            if (secondQuote < 0)
            {
                return false;
            }

            guid = json.Substring(firstQuote + 1, secondQuote - firstQuote - 1).Trim();
            return !string.IsNullOrWhiteSpace(guid);
        }
    }
}