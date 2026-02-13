using System;
using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Actor), "die", new Type[] { typeof(bool), typeof(AttackType), typeof(bool), typeof(bool) })]
    public static class ActorDiePatch
    {
        private static void Prefix(Actor __instance, out (Kingdom kingdom, Actor killer) __state)
        {
            __state = (__instance?.kingdom, __instance?.attackedBy?.a);
        }

        private static void Postfix(Actor __instance, (Kingdom kingdom, Actor killer) __state)
        {
            var (stateKingdom, stateKiller) = __state;
            if (stateKingdom == null)
                return;

            CouncilManager.OnActorDied(__instance, stateKingdom, stateKiller);
        }
    }
}
