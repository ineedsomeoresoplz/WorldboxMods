using HarmonyLib;

namespace NoRaceRestrictions
{
    [HarmonyPatch(typeof(Actor), "canFallInLoveWith")]
    internal static class NoRaceRestrictionsPatch
    {
        static bool Prefix(Actor __instance, Actor pTarget, ref bool __result)
        {
            if(!WorldLaws.InterracialRomanceEnabled)
            {
                return true;
            } 
            else 
            {
                __result = !__instance.hasLover()
                && __instance.isAdult()
                && __instance.isBreedingAge()
                && __instance.subspecies.needs_mate
                && pTarget.subspecies.needs_mate
                // && __instance.isSameSpecies(pTarget)  убирем проверку условия, Пон^^
                && __instance.subspecies.isPartnerSuitableForReproduction(__instance, pTarget)
                && !pTarget.hasLover()
                && pTarget.isAdult()
                && pTarget.isBreedingAge()
                && !__instance.isRelatedTo(pTarget);
            
                return false;
            }
        }
    }
}