using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Building), "getHit", new[] { typeof(float), typeof(bool), typeof(AttackType), typeof(BaseSimObject), typeof(bool), typeof(bool), typeof(bool) })]
    public static class BuildingCollateralDamagePatch
    {
        private static void Prefix(Building __instance, BaseSimObject pAttacker, out (bool hadHealth, Kingdom kingdom, BaseSimObject attacker) __state)
        {
            __state = (__instance != null && __instance.hasHealth(), __instance?.city?.kingdom, pAttacker);
        }

        private static void Postfix(Building __instance, (bool hadHealth, Kingdom kingdom, BaseSimObject attacker) __state)
        {
            if (!__state.hadHealth)
                return;
            if (__instance == null || __instance.hasHealth())
                return;
            CollateralWarManager.RegisterCollateralBuildingDestruction(__state.kingdom, __state.attacker);
        }
    }
}
