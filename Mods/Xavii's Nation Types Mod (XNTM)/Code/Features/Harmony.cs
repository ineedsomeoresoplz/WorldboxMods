using HarmonyLib;
using NeoModLoader.api.features;
using XNTM.Code.Patches;

namespace XNTM.Code.Features
{
    public class Harmony : ModObjectFeature<HarmonyLib.Harmony>
    {
        public HarmonyLib.Harmony Instance => Object;

        protected override HarmonyLib.Harmony InitObject()
        {
            var harmony = new HarmonyLib.Harmony("worldbox.xntm");
            harmony.PatchAll(typeof(KingdomBehCheckKingPatch).Assembly);
            return harmony;
        }
    }
}
