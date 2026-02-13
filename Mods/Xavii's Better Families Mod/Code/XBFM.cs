using HarmonyLib;
using NeoModLoader.api;
using XaviiBetterFamiliesMod.Code.Managers;

namespace XaviiBetterFamiliesMod.Code
{
    public class XBFM : BasicMod<XBFM>
    {
        private Harmony _harmony;

        protected override void OnModLoad()
        {
            _harmony = new Harmony("com.xavii.betterfamiliesmod");
            _harmony.PatchAll(typeof(Patches.FamilyPatches).Assembly);

            if (!TryGetComponent<FamilySystemsManager>(out _))
            {
                gameObject.AddComponent<FamilySystemsManager>();
            }
        }

        private void OnDestroy()
        {
            if (_harmony != null)
            {
                _harmony.UnpatchSelf();
            }
        }
    }
}
