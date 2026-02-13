using NeoModLoader.api.features;

namespace XWASM.Code.Features
{
    public class HarmonyFeature : ModObjectFeature<HarmonyLib.Harmony>
    {
        public HarmonyLib.Harmony Instance => Object;

        protected override HarmonyLib.Harmony InitObject()
        {
            return new HarmonyLib.Harmony("com.xavii.xwasm");
        }
    }
}
