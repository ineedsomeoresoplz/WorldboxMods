using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    internal static class WorldLawState
    {
        internal static bool IsEnabled(string lawId)
        {
            WorldLawAsset law = AssetManager.world_laws_library?.get(lawId);
            return law != null && law.isEnabled();
        }
    }

    [HarmonyPatch]
    public static class ActorMovePathContextPatch
    {
        [System.ThreadStatic]
        private static Actor _currentActor;

        internal static Actor CurrentActor => _currentActor;

        private static bool Prepare()
        {
            return TargetMethod() != null;
        }

        private static MethodBase TargetMethod()
        {
            List<MethodInfo> methods = AccessTools.GetDeclaredMethods(typeof(ActorMove));
            for (int i = 0; i < methods.Count; i++)
            {
                MethodInfo method = methods[i];
                if (method.Name != "goTo")
                    continue;
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 0)
                    continue;
                return method;
            }
            return null;
        }

        private static void Prefix(object __0)
        {
            _currentActor = __0 as Actor;
        }

        private static void Finalizer()
        {
            _currentActor = null;
        }
    }

    [HarmonyPatch]
    public static class MapBoxArmyNoCrossForeignPatch
    {
        private const string ArmyNoCrossForeignLawId = "world_law_army_no_cross_foreign";

        private static bool Prepare()
        {
            return TargetMethod() != null;
        }

        private static MethodBase TargetMethod()
        {
            var withActor = AccessTools.Method(typeof(MapBox), "calcPath", new[]
            {
                typeof(Actor),
                typeof(WorldTile),
                typeof(WorldTile),
                typeof(List<WorldTile>)
            });
            if (withActor != null)
                return withActor;
            return AccessTools.Method(typeof(MapBox), "calcPath", new[]
            {
                typeof(WorldTile),
                typeof(WorldTile),
                typeof(List<WorldTile>)
            });
        }

        private static void Postfix(ref bool __result, object[] __args)
        {
            if (!__result)
                return;
            if (!WorldLawState.IsEnabled(ArmyNoCrossForeignLawId))
                return;

            Actor pActor = null;
            List<WorldTile> pSavePath = null;
            if (__args != null)
            {
                for (int i = 0; i < __args.Length; i++)
                {
                    if (pActor == null)
                        pActor = __args[i] as Actor;
                    if (pSavePath == null)
                        pSavePath = __args[i] as List<WorldTile>;
                }
            }
            if (pActor == null)
                pActor = ActorMovePathContextPatch.CurrentActor;

            if (pActor == null || pActor.army == null)
                return;
            if (pSavePath == null || pSavePath.Count == 0)
                return;

            Kingdom actorKingdom = pActor.kingdom;
            if (actorKingdom == null)
                return;

            for (int i = 0; i < pSavePath.Count; i++)
            {
                WorldTile tile = pSavePath[i];
                Kingdom tileKingdom = ResolveTileKingdom(tile);
                if (!IsTerritoryPassable(actorKingdom, tileKingdom))
                {
                    pSavePath.Clear();
                    __result = false;
                    return;
                }
            }
        }

        private static Kingdom ResolveTileKingdom(WorldTile tile)
        {
            if (tile?.zone?.city == null)
                return null;
            return tile.zone.city.kingdom;
        }

        internal static bool IsTerritoryPassable(Kingdom movingKingdom, Kingdom territoryKingdom)
        {
            if (movingKingdom == null)
                return false;
            if (territoryKingdom == null || territoryKingdom == movingKingdom)
                return true;
            if (movingKingdom.isInWarWith(territoryKingdom))
                return true;

            Alliance alliance = movingKingdom.getAlliance();
            if (alliance != null && alliance.kingdoms_hashset != null && alliance.kingdoms_hashset.Contains(territoryKingdom))
                return true;

            if (BetterWarsManager.TryGetOverlord(movingKingdom, out Kingdom movingOverlord) && movingOverlord == territoryKingdom)
                return true;
            if (BetterWarsManager.TryGetOverlord(territoryKingdom, out Kingdom territoryOverlord) && territoryOverlord == movingKingdom)
                return true;
            if (movingOverlord != null && territoryOverlord != null && movingOverlord == territoryOverlord)
                return true;

            return false;
        }
    }

    [HarmonyPatch(typeof(Docks), nameof(Docks.addBoatToDock))]
    public static class DocksArmyNoCrossForeignPatch
    {
        private const string ArmyNoCrossForeignLawId = "world_law_army_no_cross_foreign";

        private static bool Prefix(Docks __instance, Actor pBoat)
        {
            if (!WorldLawState.IsEnabled(ArmyNoCrossForeignLawId))
                return true;
            if (pBoat == null || pBoat.army == null)
                return true;
            if (__instance?.building == null || !__instance.building.hasCity())
                return true;

            Kingdom boatKingdom = pBoat.kingdom;
            Kingdom dockKingdom = __instance.building.city?.kingdom;
            if (boatKingdom == null)
                return true;

            return MapBoxArmyNoCrossForeignPatch.IsTerritoryPassable(boatKingdom, dockKingdom);
        }
    }
}
