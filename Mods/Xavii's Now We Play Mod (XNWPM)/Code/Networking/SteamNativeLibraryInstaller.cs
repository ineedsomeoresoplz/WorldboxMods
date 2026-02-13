using System;
using System.IO;
using NeoModLoader.constants;
using UnityEngine;

namespace XaviiNowWePlayMod.Code.Networking
{
    internal static class SteamNativeLibraryInstaller
    {
        private const string LibraryDirectoryName = "steam_api64";
        private const string LibraryFileName = "steam_api64.dll";

        public static void EnsureInstalled()
        {
            try
            {
                if (!TryLocateLibrary(out string sourcePath))
                {
                    Debug.LogWarning(
                        $"XNWPM: Could not locate {LibraryFileName} inside any known mod folder. " +
                        "Place the DLL in Mods/steam_api64 (next to this mod) or StreamingAssets/mods/steam_api64 and restart the game.");
                    return;
                }

                string destinationDirectory = Paths.NMLAssembliesPath;
                Directory.CreateDirectory(destinationDirectory);

                string destinationPath = Path.Combine(destinationDirectory, LibraryFileName);
                if (File.Exists(destinationPath) && AreIdentical(sourcePath, destinationPath))
                {
                    return;
                }

                File.Copy(sourcePath, destinationPath, true);
                Debug.Log($"XNWPM: Copied {LibraryFileName} into the NML assemblies folder.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"XNWPM: Failed to install native Steam library: {ex.Message}");
            }
        }

        private static bool AreIdentical(string sourcePath, string destinationPath)
        {
            FileInfo sourceInfo = new(sourcePath);
            FileInfo destinationInfo = new(destinationPath);
            return sourceInfo.Length == destinationInfo.Length &&
                   sourceInfo.LastWriteTimeUtc <= destinationInfo.LastWriteTimeUtc;
        }

        private static bool TryLocateLibrary(out string path)
        {
            string[] candidateRoots =
            {
                Paths.NativeModsPath,
                Paths.ModsPath
            };

            foreach (string candidateRoot in candidateRoots)
            {
                if (string.IsNullOrWhiteSpace(candidateRoot))
                {
                    continue;
                }

                string nestedCandidate = Path.Combine(candidateRoot, LibraryDirectoryName, LibraryFileName);
                if (File.Exists(nestedCandidate))
                {
                    path = nestedCandidate;
                    return true;
                }

                string rootCandidate = Path.Combine(candidateRoot, LibraryFileName);
                if (File.Exists(rootCandidate))
                {
                    path = rootCandidate;
                    return true;
                }
            }

            path = string.Empty;
            return false;
        }
    }
}
