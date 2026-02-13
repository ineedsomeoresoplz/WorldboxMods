using HarmonyLib;
using ai.behaviours;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(CityManager), nameof(CityManager.newCity))]
    public static class CityManagerNewCityPatch
    {
        private static void Postfix(City __result)
        {
            LandTypeManager.AssignBirthType(__result);
        }
    }

    [HarmonyPatch(typeof(City), nameof(City.loadCity))]
    public static class CityLoadPatch
    {
        private static void Postfix(City __instance)
        {
            LandTypeManager.EnsureLandType(__instance);
        }
    }

    [HarmonyPatch(typeof(City), nameof(City.update))]
    public static class CityUpdatePatch
    {
        private static void Postfix(City __instance, float pElapsed)
        {
            LandTypeManager.Tick(__instance, pElapsed);
        }
    }

    [HarmonyPatch(typeof(City), "updateCityStatus")]
    public static class CityUpdateStatusPatch
    {
        private static void Postfix(City __instance)
        {
            LandTypeManager.ApplyStatusAdjustments(__instance);
        }
    }

    [HarmonyPatch(typeof(City), nameof(City.getLoyalty))]
    public static class CityGetLoyaltyPatch
    {
        private static void Postfix(City __instance, ref int __result)
        {
            __result = LandTypeManager.ApplyLoyaltyModifier(__instance, __result);
        }
    }

    [HarmonyPatch(typeof(CityBehCheckArmy), nameof(CityBehCheckArmy.execute))]
    public static class CityBehCheckArmyPatch
    {
        private static bool Prefix(City pCity, ref BehResult __result)
        {
            LandTypeManager.EnsureLandType(pCity);
            if (LandTypeManager.CanFormArmy(pCity))
                return true;

            
            __result = BehResult.Continue;
            return false;
        }
    }
}
