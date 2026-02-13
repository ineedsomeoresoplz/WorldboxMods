using System;
using System.Reflection;
using HarmonyLib;
using XaviiHistorybookMod.Code;

namespace XaviiHistorybookMod.Code.Compatibility
{
    internal static class InterracialRomanceCompatibilityPatches
    {
        private const string WorldLawsTypeName = "NoRaceRestrictions.WorldLaws";
        private const string LawPropertyName = "InterracialRomanceEnabled";

        private static MethodInfo _lawEnabledGetter;

        public static void Register(Harmony harmony)
        {
            if (harmony == null)
                return;

            var lawsType = AccessTools.TypeByName(WorldLawsTypeName);
            if (lawsType == null)
                return;

            _lawEnabledGetter = AccessTools.PropertyGetter(lawsType, LawPropertyName);
            if (_lawEnabledGetter == null)
                return;

            var target = AccessTools.Method(typeof(Actor), nameof(Actor.becomeLoversWith),
                new[] { typeof(Actor) });
            var postfix = AccessTools.Method(typeof(InterracialRomanceCompatibilityPatches),
                nameof(OnBecomeLoversWithPostfix));

            TryPatch(harmony, target, postfix);
        }

        private static void TryPatch(Harmony harmony, MethodInfo method, MethodInfo postfix)
        {
            if (harmony == null || method == null || postfix == null)
                return;

            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
        }

        private static void OnBecomeLoversWithPostfix(Actor __instance, Actor pTarget)
        {
            if (__instance == null || pTarget == null)
                return;

            if (!IsLawEnabled() || !IsInterracialPair(__instance, pTarget))
                return;

            HistorybookEvents.RecordInterracialRomance(__instance, pTarget);
            HistorybookEvents.RecordInterracialRomance(pTarget, __instance);
        }

        private static bool IsLawEnabled()
        {
            if (_lawEnabledGetter == null)
                return false;

            var result = _lawEnabledGetter.Invoke(null, null);
            return result is bool enabled && enabled;
        }

        private static bool IsInterracialPair(Actor actor, Actor partner)
        {
            var actorSpecies = actor.subspecies?.data?.species_id;
            var partnerSpecies = partner.subspecies?.data?.species_id;

            if (string.IsNullOrEmpty(actorSpecies) || string.IsNullOrEmpty(partnerSpecies))
                return false;

            return !string.Equals(actorSpecies, partnerSpecies, StringComparison.Ordinal);
        }
    }
}
