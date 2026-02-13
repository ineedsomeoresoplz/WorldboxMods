using HarmonyLib;

namespace XaviiNowWePlayMod.Code.Features
{
    internal static class HarmonyPatches
    {
        private const string HarmonyId = "com.xavii.nowweplay";
        private static Harmony _harmony;

        public static void Apply()
        {
            if (_harmony != null)
            {
                return;
            }

            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(typeof(PlayerControlClickedFinalPatch).Assembly);
        }

        public static void Remove()
        {
            _harmony?.UnpatchSelf();
            _harmony = null;
        }
    }
}
