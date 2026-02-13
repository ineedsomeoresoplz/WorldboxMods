using System;
using HarmonyLib;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(Batch<Actor>), "check", new[] { typeof(ObjectContainer<Actor>) })]
    public static class BatchActorCheckSafetyPatch
    {
        [HarmonyFinalizer]
        private static Exception Finalizer(Exception __exception)
        {
            if (__exception != null)
                return null;
            return null;
        }
    }
}
