using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(KingdomCitiesContainer), "showContent")]
    public static class KingdomCitiesContainerPuppetPatch
    {
        private static readonly FieldInfo MetaObjectField = AccessTools.Field(typeof(WindowMetaElement<Kingdom, KingdomData>), "meta_object");
        private static readonly FieldInfo TrackObjectsField = AccessTools.Field(typeof(WindowMetaElementBase), "track_objects");
        private static readonly MethodInfo ShowCityElementMethod = AccessTools.Method(typeof(KingdomCitiesContainer), "showCityElement");

        [HarmonyPrefix]
        private static bool Prefix(KingdomCitiesContainer __instance, ref IEnumerator __result)
        {
            __result = ShowWithPuppetSettlements(__instance);
            return false;
        }

        private static IEnumerator ShowWithPuppetSettlements(KingdomCitiesContainer instance)
        {
            Kingdom overlord = MetaObjectField?.GetValue(instance) as Kingdom;
            if (overlord == null)
                yield break;

            var uniqueCityIds = new HashSet<long>();
            var citiesToShow = new List<City>();

            CollectCities(overlord, citiesToShow, uniqueCityIds, false);
            foreach (Kingdom puppet in PuppetVillageWindowShared.GetPuppetKingdoms(overlord))
                CollectCities(puppet, citiesToShow, uniqueCityIds, true);

            if (TrackObjectsField?.GetValue(instance) is List<NanoObject> trackedObjects)
            {
                for (int i = 0; i < citiesToShow.Count; i++)
                    trackedObjects.Add(citiesToShow[i]);
            }

            citiesToShow.Sort((left, right) => right.getPopulationPeople().CompareTo(left.getPopulationPeople()));

            for (int i = 0; i < citiesToShow.Count; i++)
            {
                City city = citiesToShow[i];
                yield return new WaitForSecondsRealtime(0.025f);
                ShowCityElementMethod?.Invoke(instance, new object[] { city });
            }
        }

        private static void CollectCities(Kingdom kingdom, List<City> output, HashSet<long> seen, bool includeCapitals)
        {
            if (kingdom == null || !kingdom.isAlive())
                return;

            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                if (!includeCapitals && city.isCapitalCity())
                    continue;
                if (!seen.Add(city.getID()))
                    continue;
                output.Add(city);
            }
        }
    }

    [HarmonyPatch(typeof(KingdomSelectedContainerCities), "refresh")]
    public static class KingdomSelectedContainerCitiesPuppetPatch
    {
        private static readonly MethodInfo AddBannerMethod = AccessTools.Method(typeof(KingdomSelectedContainerCities), "addBanner");

        [HarmonyPostfix]
        private static void Postfix(KingdomSelectedContainerCities __instance, NanoObject pNano)
        {
            Kingdom overlord = pNano as Kingdom;
            if (overlord == null)
                return;

            var shownCityIds = new HashSet<long>();
            foreach (City city in overlord.getCities())
            {
                if (city == null)
                    continue;
                shownCityIds.Add(city.getID());
            }

            foreach (Kingdom puppet in PuppetVillageWindowShared.GetPuppetKingdoms(overlord))
            {
                foreach (City city in puppet.getCities())
                {
                    if (city == null || !city.isAlive())
                        continue;
                    if (!shownCityIds.Add(city.getID()))
                        continue;
                    AddBannerMethod?.Invoke(__instance, new object[] { city });
                }
            }
        }
    }

    internal static class PuppetVillageWindowShared
    {
        internal static IEnumerable<Kingdom> GetPuppetKingdoms(Kingdom overlord)
        {
            if (overlord == null || World.world?.kingdoms == null)
                yield break;

            foreach (Kingdom candidate in World.world.kingdoms)
            {
                if (candidate == null || !candidate.isAlive() || candidate == overlord)
                    continue;
                if (IsUnderOverlord(candidate, overlord))
                    yield return candidate;
            }
        }

        private static bool IsUnderOverlord(Kingdom kingdom, Kingdom overlord)
        {
            if (kingdom == null || overlord == null || kingdom == overlord)
                return false;

            Kingdom current = kingdom;
            var visited = new HashSet<long>();

            while (current != null && visited.Add(current.getID()))
            {
                if (!BetterWarsManager.TryGetOverlord(current, out Kingdom currentOverlord) || currentOverlord == null)
                    return false;
                if (!currentOverlord.isAlive())
                    return false;
                if (currentOverlord == overlord)
                    return true;
                if (currentOverlord == current)
                    return false;
                current = currentOverlord;
            }

            return false;
        }
    }
}
