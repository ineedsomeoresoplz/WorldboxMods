using System;
using HarmonyLib;
using ai.behaviours;
using XaviiBetterFamiliesMod.Code.Managers;
using XaviiBetterFamiliesMod.Code.Utils;

namespace XaviiBetterFamiliesMod.Code.Patches
{
    [HarmonyPatch]
    internal static class FamilyPatches
    {
        [HarmonyPatch(typeof(Actor), nameof(Actor.isRelatedTo))]
        private static class ActorIsRelatedToPatch
        {
            private static bool Prefix(Actor __instance, Actor pTarget, ref bool __result)
            {
                __result = KinshipUtils.IsRelated(__instance, pTarget);
                return false;
            }
        }

        [HarmonyPatch(typeof(Actor), nameof(Actor.canFallInLoveWith))]
        private static class ActorCanFallInLoveWithPatch
        {
            private static bool Prefix(Actor __instance, Actor pTarget, ref bool __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null && manager.IsCourtshipBlocked(__instance, pTarget))
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(BehFindLover), nameof(BehFindLover.execute))]
        private static class BehFindLoverExecutePatch
        {
            private static bool Prefix(Actor pActor, ref BehResult __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || pActor == null || pActor.isRekt() || !pActor.isSapient())
                    return true;
                if (pActor.hasLover())
                {
                    __result = BehResult.Stop;
                    return false;
                }
                Actor target = manager.FindBestLoverCandidate(pActor);
                if (target != null)
                    pActor.becomeLoversWith(target);
                __result = BehResult.Continue;
                return false;
            }
        }

        [HarmonyPatch(typeof(Actor), nameof(Actor.becomeLoversWith))]
        private static class ActorBecomeLoversWithPatch
        {
            private static void Postfix(Actor __instance, Actor pTarget)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.OnBecomeLovers(__instance, pTarget);
            }
        }

        [HarmonyPatch(typeof(BehCheckForBabiesFromSexualReproduction), "checkFamily")]
        private static class BehCheckForBabiesCheckFamilyPatch
        {
            private static bool Prefix(Actor pActor, Actor pLover)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null)
                    return true;
                manager.EnsureFamilyForParents(pActor, pLover);
                return false;
            }
        }

        [HarmonyPatch(typeof(Family), nameof(Family.isFull))]
        private static class FamilyIsFullPatch
        {
            private static bool Prefix(Family __instance, ref bool __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || __instance == null || __instance.isRekt() || !__instance.isSapient())
                    return true;
                int cap = manager.GetDynamicFamilyCap(__instance);
                __result = __instance.countUnits() > cap;
                return false;
            }
        }

        [HarmonyPatch(typeof(Family), nameof(Family.findAlpha))]
        private static class FamilyFindAlphaPatch
        {
            private static bool Prefix(Family __instance, ref Actor __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || __instance == null || __instance.isRekt() || !__instance.isSapient())
                    return true;
                __result = manager.SelectAlphaCandidate(__instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(BehFamilyGroupLeave), nameof(BehFamilyGroupLeave.execute))]
        private static class BehFamilyGroupLeaveExecutePatch
        {
            private static bool Prefix(Actor pActor, ref BehResult __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || pActor == null || pActor.isRekt() || !pActor.isSapient())
                    return true;
                if (!pActor.hasFamily())
                {
                    __result = BehResult.Stop;
                    return false;
                }
                __result = manager.TryCreateCadetBranch(pActor) ? BehResult.Continue : BehResult.Stop;
                return false;
            }
        }

        [HarmonyPatch(typeof(BehChildFindRandomFamilyParent), nameof(BehChildFindRandomFamilyParent.execute))]
        private static class BehChildFindRandomFamilyParentExecutePatch
        {
            private static bool Prefix(Actor pBabyActor, ref BehResult __result)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || pBabyActor == null || pBabyActor.isRekt())
                    return true;
                Actor guardian = manager.SelectGuardianForChild(pBabyActor);
                if (guardian == null)
                {
                    __result = BehResult.Stop;
                    return false;
                }
                pBabyActor.beh_actor_target = guardian;
                __result = BehResult.Continue;
                return false;
            }
        }

        [HarmonyPatch(typeof(BabyHelper), nameof(BabyHelper.traitsInherit))]
        private static class BabyHelperTraitsInheritPatch
        {
            private static void Postfix(Actor pActorTarget, Actor pParent1, Actor pParent2)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.ApplyEnhancedInheritance(pActorTarget, pParent1, pParent2);
            }
        }

        [HarmonyPatch(typeof(BabyHelper), nameof(BabyHelper.countBirth))]
        private static class BabyHelperCountBirthPatch
        {
            private static void Postfix(Actor pBaby)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.OnBabyBorn(pBaby);
            }
        }

        private struct ReproductionState
        {
            public bool Suppressed;
            public int ParentABirths;
            public int ParentBBirths;
        }

        [HarmonyPatch(typeof(BehCheckForBabiesFromSexualReproduction), "checkForBabies")]
        private static class BehCheckForBabiesCheckForBabiesPatch
        {
            private static bool Prefix(Actor pParentA, Actor pParentB, ref ReproductionState __state)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                __state = new ReproductionState();
                __state.ParentABirths = pParentA != null ? pParentA.data.births : 0;
                __state.ParentBBirths = pParentB != null ? pParentB.data.births : 0;
                if (manager == null)
                    return true;
                if (manager.ShouldSuppressBirth(pParentA, pParentB))
                {
                    __state.Suppressed = true;
                    return false;
                }
                return true;
            }

            private static void Postfix(Actor pParentA, Actor pParentB, ReproductionState __state)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || __state.Suppressed)
                    return;
                int birthsA = pParentA != null ? pParentA.data.births : 0;
                int birthsB = pParentB != null ? pParentB.data.births : 0;
                bool happened = birthsA > __state.ParentABirths || birthsB > __state.ParentBBirths;
                if (happened)
                    manager.TryApplyStabilityBonusBirth(pParentA, pParentB);
            }
        }

        [HarmonyPatch(typeof(Actor), nameof(Actor.setFamily))]
        private static class ActorSetFamilyPatch
        {
            private static void Prefix(Actor __instance, ref Family __state)
            {
                __state = __instance.family;
            }

            private static void Postfix(Actor __instance, Family pObject, Family __state)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.OnFamilyChanged(__instance, __state, pObject);
            }
        }

        [HarmonyPatch(typeof(Actor), "getHit", new[] { typeof(float), typeof(bool), typeof(AttackType), typeof(BaseSimObject), typeof(bool), typeof(bool), typeof(bool) })]
        private static class ActorGetHitPatch
        {
            private static void Postfix(Actor __instance, BaseSimObject pAttacker)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager == null || __instance == null || __instance.isRekt() || pAttacker == null || pAttacker.isRekt() || !pAttacker.isActor())
                    return;
                Actor attacker = pAttacker.a;
                if (attacker == null || attacker.isRekt())
                    return;
                manager.TryTriggerFamilyDefense(__instance, attacker);
            }
        }

        [HarmonyPatch(typeof(Actor), "die", new[] { typeof(bool), typeof(AttackType), typeof(bool), typeof(bool) })]
        private static class ActorDiePatch
        {
            private static void Prefix(Actor __instance, bool pDestroy, AttackType pType, bool pCountDeath, bool pLogFavorite)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.HandleDeath(__instance, pCountDeath);
            }
        }

        [HarmonyPatch(typeof(Actor), "eventBecomeAdult")]
        private static class ActorEventBecomeAdultPatch
        {
            private static void Postfix(Actor __instance)
            {
                FamilySystemsManager manager = FamilySystemsManager.Instance;
                if (manager != null)
                    manager.OnBecomeAdult(__instance);
            }
        }
    }
}
