using HarmonyLib;
using XWASM.Code.Content;

namespace XWASM.Code.Patches
{
    public static class SapiencePatches
    {
        private static Harmony _harmony;

        public static void Apply()
        {
            if (_harmony != null)
                return;
            _harmony = new Harmony("com.xavii.xwasm.patches");
            _harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Actor), "isSapient")]
    internal static class ActorIsSapientPatch
    {
        private static void Postfix(Actor __instance, ref bool __result)
        {
            if (!__result && SapienceHelper.IsSapientByTrait(__instance))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Subspecies), "isSapient")]
    internal static class SubspeciesIsSapientPatch
    {
        private static void Postfix(Subspecies __instance, ref bool __result)
        {
            if (!__result && SapienceHelper.HasSapientSpecies(__instance))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(CityManager), "canStartNewCityCivilizationHere")]
    internal static class CityManagerCanStartNewCityPatch
    {
        private static void Postfix(Actor pActor, ref bool __result)
        {
            if (__result)
                return;
            if (!SapienceHelper.IsSapientByTrait(pActor))
                return;
            var kingdomAsset = AssetManager.kingdoms.get(SapienceHelper.GetKingdomId(pActor));
            if (kingdomAsset != null && kingdomAsset.civ)
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Kingdom), "newCivKingdom")]
    internal static class KingdomNewCivKingdomPatch
    {
        private static bool Prefix(Kingdom __instance, Actor pActor)
        {
            string assetId = SapienceHelper.GetKingdomId(pActor);
            KingdomAsset asset = AssetManager.kingdoms.get(assetId);
            if (asset == null)
                return true;
            __instance.asset = asset;
            __instance.data.original_actor_asset = pActor.asset.id;
            __instance.setName(pActor.generateName(MetaType.Kingdom, __instance.getID()));
            Culture culture = __instance.culture;
            __instance.data.name_culture_id = culture != null ? culture.id : -1;
            __instance.generateNewMetaObject();
            return false;
        }
    }
}
