using HarmonyLib;
using NeoModLoader.api.features;
using XaviiHistorybookMod.Code.Compatibility;
using XaviiHistorybookMod.Code.Patches;

namespace XaviiHistorybookMod.Code.Features
{
    public class Harmony : ModObjectFeature<HarmonyLib.Harmony>
    {
        public HarmonyLib.Harmony Instance => Object;

        protected override HarmonyLib.Harmony InitObject()
        {
            var harmony = new HarmonyLib.Harmony("xavii.worldbox.historybook");
            harmony.PatchAll(typeof(SwitchFavoritePatch).Assembly);
            CompatibilityRegistrar.Register(harmony);
            return harmony;
        }
    }
}
