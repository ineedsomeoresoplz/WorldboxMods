using System;
using System.Diagnostics;
using UnityEngine;

namespace XaviiWindowsMod.Xwm.Studio
{
    internal static class XwmNativeFolderPicker
    {
        public static bool TryPickFolder(out string path)
        {
            path = null;
            RuntimePlatform platform = Application.platform;
            if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
            {
                return TryPickWindows(out path);
            }

            if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXEditor)
            {
                return TryPickMac(out path);
            }

            return false;
        }

        public static bool TryPickImageFile(out string path)
        {
            path = null;
            RuntimePlatform platform = Application.platform;
            if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
            {
                return TryPickWindowsImageFile(out path);
            }

            if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXEditor)
            {
                return TryPickMacImageFile(out path);
            }

            return false;
        }

        private static bool TryPickWindows(out string path)
        {
            path = null;
            string script = "Add-Type -AssemblyName System.Windows.Forms; $d = New-Object System.Windows.Forms.FolderBrowserDialog; $d.Description = 'Select target mod folder'; $d.ShowNewFolderButton = $false; if ($d.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) { [Console]::Write($d.SelectedPath) }";
            string arguments = "-NoProfile -STA -Command \"" + EscapeForDoubleQuotedArgument(script) + "\"";
            if (!TryRun("powershell", arguments, out string output))
            {
                if (!TryRun("pwsh", arguments, out output))
                {
                    return false;
                }
            }

            string trimmed = (output ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return false;
            }

            path = trimmed;
            return true;
        }

        private static bool TryPickMac(out string path)
        {
            path = null;
            string arguments = "-e \"set chosenFolder to choose folder with prompt \\\"Select target mod folder\\\"\" -e \"POSIX path of chosenFolder\"";
            if (!TryRun("osascript", arguments, out string output))
            {
                return false;
            }

            string trimmed = (output ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return false;
            }

            path = trimmed;
            return true;
        }

        private static bool TryPickWindowsImageFile(out string path)
        {
            path = null;
            string script = "Add-Type -AssemblyName System.Windows.Forms; $d = New-Object System.Windows.Forms.OpenFileDialog; $d.Title = 'Select image file'; $d.Filter = 'Image Files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All Files (*.*)|*.*'; $d.Multiselect = $false; if ($d.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) { [Console]::Write($d.FileName) }";
            string arguments = "-NoProfile -STA -Command \"" + EscapeForDoubleQuotedArgument(script) + "\"";
            if (!TryRun("powershell", arguments, out string output))
            {
                if (!TryRun("pwsh", arguments, out output))
                {
                    return false;
                }
            }

            string trimmed = (output ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return false;
            }

            path = trimmed;
            return true;
        }

        private static bool TryPickMacImageFile(out string path)
        {
            path = null;
            string arguments = "-e \"set chosenFile to choose file with prompt \\\"Select image file\\\"\" -e \"POSIX path of chosenFile\"";
            if (!TryRun("osascript", arguments, out string output))
            {
                return false;
            }

            string trimmed = (output ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return false;
            }

            path = trimmed;
            return true;
        }

        private static bool TryRun(string executable, string arguments, out string output)
        {
            output = string.Empty;
            try
            {
                ProcessStartInfo start = new ProcessStartInfo(executable, arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(start))
                {
                    if (process == null)
                    {
                        return false;
                    }

                    output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit(30000);
                    if (!process.HasExited)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                        }

                        return false;
                    }

                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string EscapeForDoubleQuotedArgument(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
