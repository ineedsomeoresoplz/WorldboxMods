using System;
using HarmonyLib;
using UnityEngine;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WindowMetaGeneric<Kingdom, KingdomData>), nameof(WindowMetaGeneric<Kingdom, KingdomData>.startShowingWindow))]
    public static class KingdomWindowLocalizationRefreshPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            LocalizationContext.Push(SelectedMetas.selected_kingdom);
        }

        [HarmonyPostfix]
        private static void Postfix(WindowMetaGeneric<Kingdom, KingdomData> __instance)
        {
            WindowLocalizationRefresher.Refresh(__instance);
        }

        [HarmonyFinalizer]
        private static Exception Finalizer(Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(WindowMetaGeneric<City, CityData>), nameof(WindowMetaGeneric<City, CityData>.startShowingWindow))]
    public static class CityWindowLocalizationRefreshPatch
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            LocalizationContext.Push(SelectedMetas.selected_city?.kingdom);
        }

        [HarmonyPostfix]
        private static void Postfix(WindowMetaGeneric<City, CityData> __instance)
        {
            WindowLocalizationRefresher.Refresh(__instance);
        }

        [HarmonyFinalizer]
        private static Exception Finalizer(Exception __exception)
        {
            LocalizationContext.Pop();
            return __exception;
        }
    }

    internal static class WindowLocalizationRefresher
    {
        internal static void Refresh(Component root)
        {
            if (root == null)
                return;

            LocalizedText[] fields = root.GetComponentsInChildren<LocalizedText>(true);
            for (int i = 0; i < fields.Length; i++)
            {
                LocalizedText field = fields[i];
                if (field == null)
                    continue;
                string key = field.key;
                if (string.IsNullOrEmpty(key) || key == "??????")
                    continue;
                field.setKeyAndUpdate(key);
            }
        }
    }
}
