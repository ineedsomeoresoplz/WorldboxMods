using HarmonyLib;
using NeoModLoader.api;
using UnityEngine;
using XaviiBetterTimeMod.Code.Managers;

namespace XaviiBetterTimeMod.Code
{
    public class XBTM : BasicMod<XBTM>
    {
        private Harmony _harmony;

        protected override void OnModLoad()
        {
            _harmony = new Harmony("com.xavii.bettertimemod");
            _harmony.PatchAll(typeof(Features.TimePatches).Assembly);

            if (!TryGetComponent<BetterTimeManager>(out _))
            {
                gameObject.AddComponent<BetterTimeManager>();
            }
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
