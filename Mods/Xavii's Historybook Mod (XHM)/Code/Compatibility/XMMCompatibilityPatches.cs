using System;
using System.Reflection;
using HarmonyLib;

namespace XaviiHistorybookMod.Code.Compatibility
{
    internal static class XMMCompatibilityPatches
    {
        private const string MagicManagerTypeName = "XaviiMagiaMod.Code.Managers.MagicManager";

        public static void Register(Harmony harmony)
        {
            if (harmony == null)
                return;

            var managerType = AccessTools.TypeByName(MagicManagerTypeName);
            if (managerType == null)
                return;

            TryPatch(harmony,
                managerType,
                "LogReincarnation",
                new[] { typeof(Actor), typeof(Actor) },
                AccessTools.Method(typeof(XMMCompatibilityPatches), nameof(LogReincarnationPostfix)));

            TryPatch(harmony,
                managerType,
                "DecorateSpellForActor",
                new[] { typeof(Actor), typeof(SpellAsset) },
                AccessTools.Method(typeof(XMMCompatibilityPatches), nameof(DecorateSpellForActorPostfix)));
        }

        private static void TryPatch(
            Harmony harmony,
            Type managerType,
            string methodName,
            Type[] parameters,
            MethodInfo postfixMethod)
        {
            if (harmony == null || managerType == null || string.IsNullOrEmpty(methodName) || postfixMethod == null)
                return;

            var target = AccessTools.Method(managerType, methodName, parameters);
            if (target == null)
                return;

            harmony.Patch(target, postfix: new HarmonyMethod(postfixMethod));
        }

        private static void LogReincarnationPostfix(Actor soul, Actor host)
        {
            HistorybookEvents.RecordOrlReincarnation(host, soul);
        }

        private static void DecorateSpellForActorPostfix(Actor actor, SpellAsset __result)
        {
            HistorybookEvents.RecordOrlSpellCast(actor, __result);
        }
    }
}
