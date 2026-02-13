using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using NeoModLoader.constants;

namespace XaviiHistorybookMod.Code.Compatibility
{
    internal enum CompatibilityMod
    {
        Magia,
        NationTypes,
        InterracialRomance
    }

    internal static class CompatibilityRegistrar
    {
        private static readonly IReadOnlyDictionary<CompatibilityMod, string> ModFolders =
            new Dictionary<CompatibilityMod, string>
            {
                [CompatibilityMod.Magia] = "xavii_s_magia_mod_xmm_",
                [CompatibilityMod.NationTypes] = "xavii_s_nation_types_mod_xntm__ee0d0",
                [CompatibilityMod.InterracialRomance] = "interracial_romance"
            };

        public static void Register(Harmony harmony)
        {
            if (harmony == null)
                return;

            TryRegister(harmony, CompatibilityMod.Magia, XMMCompatibilityPatches.Register);
            TryRegister(harmony, CompatibilityMod.NationTypes, null);
            TryRegister(harmony, CompatibilityMod.InterracialRomance, InterracialRomanceCompatibilityPatches.Register);
        }

        private static void TryRegister(Harmony harmony, CompatibilityMod mod, Action<Harmony> registerAction)
        {
            if (!IsInstalled(mod) || registerAction == null)
                return;

            registerAction(harmony);
        }

        public static bool IsInstalled(CompatibilityMod mod)
        {
            if (!ModFolders.TryGetValue(mod, out var folder))
                return false;

            var modsRoot = Paths.ModsPath;
            if (string.IsNullOrEmpty(modsRoot))
                return false;

            return Directory.Exists(Path.Combine(modsRoot, folder));
        }
    }
}
