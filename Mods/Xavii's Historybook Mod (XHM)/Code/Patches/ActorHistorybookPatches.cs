using System;
using HarmonyLib;

namespace XaviiHistorybookMod.Code.Patches
{
    [HarmonyPatch(typeof(Actor), "switchFavorite")]
    internal static class SwitchFavoritePatch
    {
        private static void Prefix(Actor __instance, out bool __state)
        {
            __state = __instance.isFavorite();
        }

        private static void Postfix(Actor __instance, bool __state)
        {
            HistorybookEvents.RecordFavoriteChange(__instance, __state, __instance.isFavorite());
        }
    }

    [HarmonyPatch(typeof(Actor), "newKillAction")]
    internal static class KillActionPatch
    {
        private static void Postfix(Actor __instance, Actor pDeadUnit, Kingdom pPrevKingdom, AttackType pAttackType)
        {
            HistorybookEvents.RecordKillVictory(__instance, pDeadUnit, pAttackType);
        }
    }

    [HarmonyPatch(typeof(Actor), "setTask", new Type[] { typeof(string), typeof(bool), typeof(bool), typeof(bool) })]
    internal static class SetTaskPatch
    {
        private static void Postfix(Actor __instance, string pTaskId, bool pClean, bool pCleanJob, bool pForceAction)
        {
            HistorybookEvents.RecordAction(__instance, pTaskId);
        }
    }

    [HarmonyPatch(typeof(Actor), "getHit", new Type[]
    {
        typeof(float),
        typeof(bool),
        typeof(AttackType),
        typeof(BaseSimObject),
        typeof(bool),
        typeof(bool),
        typeof(bool)
    })]
    internal static class GetHitPatch
    {
        private static void Prefix(Actor __instance, out float __state)
        {
            __state = __instance.getHealth();
        }

        private static void Postfix(
            Actor __instance,
            float pDamage,
            bool pFlash,
            AttackType pAttackType,
            BaseSimObject pAttacker,
            bool pSkipIfShake,
            bool pMetallicWeapon,
            bool pCheckDamageReduction,
            float __state)
        {
            if (!__instance.isAlive())
                return;
            var damageTaken = __state - __instance.getHealth();
            if (damageTaken <= 0f)
                return;
            HistorybookEvents.RecordInjury(__instance, damageTaken, pAttackType);
        }
    }

    [HarmonyPatch(typeof(Actor), "die")]
    internal static class DiePatch
    {
        private static void Prefix(Actor __instance, bool pDestroy, AttackType pType, bool pCountDeath, bool pLogFavorite, out BaseSimObject __state)
        {
            __state = __instance.attackedBy;
        }

        private static void Postfix(Actor __instance, bool pDestroy, AttackType pType, bool pCountDeath, bool pLogFavorite, BaseSimObject __state)
        {
            var killer = __state?.a;
            HistorybookEvents.RecordDeath(__instance, pType, killer);
        }
    }

    [HarmonyPatch(typeof(Actor), "becomeLoversWith")]
    internal static class BecomeLoversPatch
    {
        private static void Postfix(Actor __instance, Actor pTarget)
        {
            if (pTarget == null)
                return;
            HistorybookEvents.RecordLoverBond(__instance, pTarget);
            HistorybookEvents.RecordLoverBond(pTarget, __instance);
        }
    }

    [HarmonyPatch(typeof(ActorManager), "spawnNewUnit")]
    internal static class SpawnNewUnitPatch
    {
        private static void Postfix(Actor __result)
        {
            if (__result != null)
                HistorybookEvents.RecordNewBirth(__result);
        }
    }

    [HarmonyPatch(typeof(Kingdom), "setKing")]
    internal static class KingdomSetKingPatch
    {
        private static void Postfix(Kingdom __instance, Actor pActor, bool pFromLoad)
        {
            if (pActor != null)
                HistorybookEvents.RecordRoyalAscension(pActor, __instance, pFromLoad);
        }
    }
}
