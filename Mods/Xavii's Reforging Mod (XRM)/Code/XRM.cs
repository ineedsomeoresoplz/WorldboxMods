using System.Reflection;
using HarmonyLib;
using NeoModLoader.api;
using XRM.Code.Content;
using XRM.Code.Patches;

namespace XRM.Code
{
    public class XRM : BasicMod<XRM>
    {
        private const string HarmonyId = "com.xavii.xrm";
        private const string LegacyHarmonyId = "com.xavii.reforgingmod";

        private Harmony _harmony;
        private bool _legacySweepDone;
        private short _legacySweepCounter;

        protected override void OnModLoad()
        {
            XrmBuffRegistry.EnsureInitialized();
            _harmony = new Harmony(HarmonyId);
            UnpatchLegacyReforgePrefixes();
            _harmony.PatchAll(typeof(ItemWindowReforgePatches).Assembly);
        }

        private void Update()
        {
            XrmBuffRegistry.EnsureInitialized();
            if (!_legacySweepDone && _harmony != null)
            {
                if (_legacySweepCounter++ > 60)
                {
                    UnpatchLegacyReforgePrefixes();
                    _legacySweepDone = true;
                }
            }
        }

        private void OnDestroy()
        {
            if (_harmony != null)
            {
                _harmony.UnpatchSelf();
            }
        }

        private void UnpatchLegacyReforgePrefixes()
        {
            if (_harmony == null)
            {
                return;
            }

            UnpatchLegacyPrefix(nameof(ItemWindow.clickReforge));
            UnpatchLegacyPrefix(nameof(ItemWindow.clickReforgeDivine));
        }

        private void UnpatchLegacyPrefix(string methodName)
        {
            MethodInfo method = AccessTools.Method(typeof(ItemWindow), methodName);
            if (method == null)
            {
                return;
            }

            _harmony.Unpatch(method, HarmonyPatchType.Prefix, LegacyHarmonyId);
        }
    }
}
