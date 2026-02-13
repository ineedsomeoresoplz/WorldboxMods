using System;
using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(ScrollWindow), nameof(ScrollWindow.setActive))]
    public static class ScrollWindowOpenContextPatch
    {
        [HarmonyPrefix]
        private static void Prefix(ScrollWindow __instance, bool pActive)
        {
            if (!pActive)
                return;
            WindowOpenContext.Set(__instance?.screen_id);
        }

        [HarmonyFinalizer]
        private static Exception Finalizer(Exception __exception)
        {
            WindowOpenContext.Clear();
            return __exception;
        }
    }
}
