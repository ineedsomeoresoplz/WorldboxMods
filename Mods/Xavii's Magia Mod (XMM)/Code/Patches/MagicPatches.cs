using System;
using System.Reflection;
using HarmonyLib;
using XaviiMagiaMod.Code.Data;
using XaviiMagiaMod.Code.Managers;

namespace XaviiMagiaMod.Code.Patches
{
    [HarmonyPatch(typeof(BabyMaker), "makeBaby")]
    internal static class BabyMaker_makeBaby_Patch
    {
        private static void Postfix(Actor __result, Actor pParent1, Actor pParent2)
        {
            var manager = MagicManager.Instance;
            if (manager == null || __result == null)
                return;

            manager.HandleBirth(__result, pParent1, pParent2);
        }
    }

    [HarmonyPatch(typeof(ActorManager), "finalizeActor")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(Actor), typeof(WorldTile), typeof(float) })]
    internal static class ActorManager_finalizeActor_Patch
    {
        private static void Postfix(Actor pActor)
        {
            MagicManager.Instance?.HandleSpawn(pActor);
        }
    }

[HarmonyPatch(typeof(Actor), nameof(Actor.addTrait), new[] { typeof(ActorTrait), typeof(bool) })]
internal static class Actor_addTrait_Patch
{
private static void Postfix(Actor __instance, ActorTrait pTrait)
{
        MagicManager.Instance?.HandleTraitAdded(__instance, pTrait);
    }
}

[HarmonyPatch(typeof(Actor), nameof(Actor.removeTrait), new[] { typeof(ActorTrait) })]
internal static class Actor_removeTrait_Patch
{
    private static void Postfix(Actor __instance, ActorTrait pTrait)
    {
        MagicManager.Instance?.HandleTraitRemoved(__instance, pTrait);
    }
}

[HarmonyPatch(typeof(Actor), "die")]
[HarmonyPatch(new Type[] { typeof(bool), typeof(AttackType), typeof(bool), typeof(bool) })]
internal static class Actor_die_Patch
    {
    private static void Postfix(Actor __instance)
    {
        MagicManager.Instance?.HandleActorDestroyed(__instance);
    }
}

[HarmonyPatch(typeof(Actor), "getRandomSpell")]
internal static class Actor_getRandomSpell_Patch
{
    private static void Postfix(Actor __instance, ref SpellAsset __result)
    {
        if (__result == null)
            return;

        var manager = MagicManager.Instance;
        if (manager == null)
            return;

        __result = manager.DecorateSpellForActor(__instance, __result);
    }
}

[HarmonyPatch(typeof(Actor), "getHit", new[] { typeof(float), typeof(bool), typeof(AttackType), typeof(BaseSimObject), typeof(bool), typeof(bool), typeof(bool) })]
internal static class Actor_getHit_Patch
{
    private static bool Prefix(
        Actor __instance,
        float pDamage,
        bool pFlash,
        AttackType pAttackType,
        BaseSimObject pAttacker,
        bool pSkipIfShake,
        bool pMetallicWeapon,
        bool pCheckDamageReduction)
    {
        var manager = MagicManager.Instance;
        if (manager == null)
            return true;

        var attacker = pAttacker?.a;
        if (attacker != null && manager.TrySealDemonLord(attacker, __instance))
            return false;

        if (manager.HasSealedTrait(__instance) || manager.HasPermaSealedTrait(__instance))
            return false;

        return true;
    }
}

[HarmonyPatch(typeof(Actor), nameof(Actor.needsFood))]
internal static class Actor_needsFood_Patch
{
    private static bool Prefix(Actor __instance, ref bool __result)
    {
        if (MagicManager.Instance?.HasSealedTrait(__instance) == true)
        {
            __result = false;
            return false;
        }

        if (MagicManager.Instance?.HasPermaSealedTrait(__instance) == true)
        {
            __result = false;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(CombatActionLibrary), "tryToCastSpell")]
internal static class CombatActionLibrary_tryToCastSpell_Patch
{
    private static bool Prefix(CombatActionLibrary __instance, AttackData pData, ref bool __result)
    {
        Actor a = pData.initiator?.a;
        if (a == null)
        {
            __result = false;
            return false;
        }

        var manager = MagicManager.Instance;
        if (manager == null)
            return true;

        if (manager.HasSealedTrait(a))
        {
            __result = false;
            return false;
        }

        if (manager.HasPermaSealedTrait(a))
        {
            __result = false;
            return false;
        }

        if (!manager.HasElementalAffinity(a))
            return true;

        BaseSimObject pTarget = pData.target;
        SpellAsset randomSpell = a.getRandomSpell();
        if (randomSpell == null)
        {
            __result = false;
            return false;
        }

        if (randomSpell.cast_target == CastTarget.Himself)
            pTarget = (BaseSimObject) a;

        float chance = randomSpell.chance;
        MagicSpellDefinition definition = null;
        if (!manager.TryPrepareSpellForCombat(a, randomSpell, pTarget, out chance, out definition))
        {
            __result = false;
            return false;
        }

        if (definition == null)
            return true;

        bool inCombat = MagicManager.IsActorInCombat(a);

        if (!a.hasEnoughMana(randomSpell.cost_mana))
        {
            __result = false;
            return false;
        }

        if (!inCombat)
        {
            float skillChance = chance + chance * a.stats["skill_spell"];
            if (!Randy.randomChance(skillChance))
            {
                __result = false;
                return false;
            }
        }

        if (randomSpell.cast_entity == CastEntity.BuildingsOnly)
        {
            if (pTarget.isActor())
            {
                __result = false;
                return false;
            }
        }
        else if (randomSpell.cast_entity == CastEntity.UnitsOnly && pTarget.isBuilding())
        {
            __result = false;
            return false;
        }

        if ((double) randomSpell.health_ratio > 0.0)
        {
            float healthRatio = a.getHealthRatio();
            if ((double) randomSpell.health_ratio <= (double) healthRatio)
            {
                __result = false;
                return false;
            }
        }

        if ((double) randomSpell.min_distance > 0.0 && (double) Toolbox.SquaredDistTile(a.current_tile, pTarget.current_tile) < (double) randomSpell.min_distance * (double) randomSpell.min_distance)
        {
            __result = false;
            return false;
        }

        bool scheduledSpell = manager.QueueSpellForCasting(a, randomSpell, pTarget, definition);
        __result = scheduledSpell;
        return false;
    }
}

[HarmonyPatch(typeof(TooltipLibrary), "showStatus", new Type[] { typeof(Tooltip), typeof(string), typeof(TooltipData) })]
internal static class TooltipLibrary_showStatus_Patch
{
    private static void Postfix(Tooltip pTooltip, string pType, TooltipData pData)
    {
        MagicManager.Instance?.OverrideStatusTooltip(pTooltip, pData.status);
    }
}

[HarmonyPatch(typeof(BaseSimObject), "canAttackTarget", new[] { typeof(BaseSimObject), typeof(bool), typeof(bool) })]
internal static class BaseSimObject_canAttackTarget_Patch
{
    private static bool Prefix(BaseSimObject __instance, BaseSimObject pTarget, ref bool __result)
    {
        var manager = MagicManager.Instance;
        if (pTarget?.isActor() == true)
        {
            var targetActor = pTarget.a;
            if (manager?.HasSealedTrait(targetActor) == true)
            {
                __result = false;
                return false;
            }

            if (manager?.HasPermaSealedTrait(targetActor) == true)
            {
                __result = false;
                return false;
            }

            if (manager != null && manager.IsDemonLordSummon(targetActor) && __instance?.isActor() == true)
            {
                var attacker = __instance.a;
                if (attacker != null &&
                    attacker.kingdom != null &&
                    targetActor.kingdom != null &&
                    attacker.kingdom == targetActor.kingdom &&
                    !manager.CanFriendlyAttackDemonLordSummon(attacker, targetActor))
                {
                    __result = false;
                    return false;
                }
            }
        }

        return true;
    }
}

[HarmonyPatch(typeof(BaseSimObject), nameof(BaseSimObject.isActor))]
internal static class BaseSimObject_isActor_Patch
{
    private static void Postfix(BaseSimObject __instance, ref bool __result)
    {
        if (__result && MagicManager.Instance?.HasSealedTrait(__instance.a) == true)
            __result = false;

        if (__result && MagicManager.Instance?.HasPermaSealedTrait(__instance.a) == true)
            __result = false;
    }
}

[HarmonyPatch(typeof(Batch<Actor>), "check", new[] { typeof(ObjectContainer<Actor>) })]
internal static class BatchActors_check_Patch
{
    private static readonly FieldInfo ArrayField =
        AccessTools.Field(typeof(BatchActors), "_array");
    private static readonly FieldInfo CountField =
        AccessTools.Field(typeof(BatchActors), "_count");

    private static void Postfix(
        BatchActors __instance,
        ObjectContainer<Actor> pContainer,
        ref bool __result)
    {
        if (!__result)
            return;

        FilterSealed(__instance);
    }

    private static void FilterSealed(BatchActors instance)
    {
        var manager = MagicManager.Instance;
        if (manager == null)
            return;

        Actor[] array = ArrayField?.GetValue(instance) as Actor[];
        int count = CountField != null ? (int) (CountField.GetValue(instance) ?? 0) : 0;
        if (array == null || count == 0)
            return;

        int keep = 0;
        for (int i = 0; i < count; i++)
        {
            Actor actor = array[i];
            if (actor != null && manager.HasSealedTrait(actor))
                continue;
            
            if (actor != null && manager.HasPermaSealedTrait(actor))
                continue;

            if (keep != i)
                array[keep] = actor;
            keep++;
        }

        if (keep != count && CountField != null)
            CountField.SetValue(instance, keep);
    }
}

[HarmonyPatch(typeof(StatsRowsContainer), "showStatRow")]
internal static class StatsRowsContainer_showStatRow_ReincarnationRow_Patch
{
    private static void Postfix(string pId, KeyValueField __result)
    {
        if (__result == null || !string.Equals(pId, "past_names", StringComparison.Ordinal))
            return;

        var actor = SelectedUnit.unit;
        if (actor == null || actor.isRekt())
            return;

        if (!actor.hasTrait("magic_orl") && !actor.hasTrait("magic_demonlord"))
            return;

        __result.name_text.text = LocalizedTextManager.stringExists("xmm_reincarnation_history")
            ? LocalizedTextManager.getText("xmm_reincarnation_history")
            : "Reincarnation History";
    }
}

[HarmonyPatch(typeof(Actor), nameof(Actor.getAge))]
internal static class Actor_getAge_GodTime_Patch
{
    private static void Postfix(Actor __instance, ref int __result)
    {
        if (__instance == null)
            return;

        MagicManager.Instance?.ApplyGodTimeAgeAdjustment(__instance, ref __result);
    }
}

internal static class MagicPatches
{
}
}
