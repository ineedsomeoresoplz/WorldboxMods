using HarmonyLib;
using XNTM.Code.Features.BetterWars;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WarManager), nameof(WarManager.newWar))]
    public static class BetterWarsWarStartPatch
    {
        private static void Postfix(War __result, Kingdom pAttacker, Kingdom pDefender)
        {
            BetterWarsManager.OnWarStarted(__result, pAttacker, pDefender);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.newWar))]
    public static class BetterWarsWarGuardPatch
    {
        private static bool Prefix(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pType, ref War __result)
        {
            if (pAttacker == null)
                return true;

            
            if (pType != null && pType.id == "whisper_of_war")
                return true;

            
            if (BetterWarsManager.IsDemilitarized(pAttacker))
            {
                __result = null;
                return false;
            }

            
            if (BetterWarsManager.TryGetOverlord(pAttacker, out var overlord) && overlord != null)
            {
                if (pDefender == null || !BetterWarsManager.IsKingdomInOverlordBloc(overlord, pDefender))
                {
                    __result = null;
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(DiplomacyManager), "startWar", new System.Type[] { typeof(Kingdom), typeof(Kingdom), typeof(WarTypeAsset), typeof(bool) })]
    public static class BetterWarsDiplomacyStartWarPatch
    {
        private static bool Prefix(DiplomacyManager __instance, Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pAsset, bool pLog, ref War __result)
        {
            if (pAsset.total_war)
                return true;
            if (pAttacker == pDefender)
            {
                __result = null;
                return false;
            }
            if (World.world.wars.getWar(pAttacker, pDefender) != null)
            {
                __result = null;
                return false;
            }
            War war = World.world.wars.newWar(pAttacker, pDefender, pAsset);
            if (war == null)
            {
                __result = null;
                return false;
            }
            if (pLog)
                WorldLog.logNewWar(pAttacker, pDefender);
            if (pAsset.alliance_join)
            {
                Alliance alliance1 = pAttacker.getAlliance();
                Alliance alliance2 = pDefender.getAlliance();
                if (alliance1 != null)
                {
                    foreach (Kingdom pKingdom in alliance1.kingdoms_hashset)
                        war.joinAttackers(pKingdom);
                }
                if (alliance2 != null)
                {
                    foreach (Kingdom pKingdom in alliance2.kingdoms_hashset)
                        war.joinDefenders(pKingdom);
                }
            }
            __result = war;
            return false;
        }
    }

    [HarmonyPatch(typeof(DiplomacyHelpersRebellion), nameof(DiplomacyHelpersRebellion.startRebellion))]
    public static class BetterWarsRebellionStartPatch
    {
        private static bool Prefix(Actor pActor, Plot pPlot, bool pCheckForHappiness)
        {
            City city1 = pActor.city;
            Kingdom kingdom1 = city1.kingdom;
            if (pActor.isCityLeader())
                pActor.city.removeLeader();
            Kingdom kingdom2 = city1.makeOwnKingdom(pActor, true);
            using (ListPool<City> pNewCities = new ListPool<City>())
            {
                pNewCities.Add(city1);
                pActor.joinCity(city1);
                War war1 = null;
                foreach (War war2 in kingdom1.getWars())
                {
                    if (war2.isMainAttacker(kingdom1) && war2.getAsset() == WarTypeLibrary.rebellion)
                    {
                        war1 = war2;
                        war1.joinDefenders(kingdom2);
                        break;
                    }
                }
                if (war1 == null)
                {
                    War war3 = World.world.diplomacy.startWar(kingdom1, kingdom2, WarTypeLibrary.rebellion);
                    if (war3 != null && kingdom1.hasAlliance())
                    {
                        foreach (Kingdom pKingdom in kingdom1.getAlliance().kingdoms_hashset)
                        {
                            if (pKingdom != kingdom1 && pKingdom.isOpinionTowardsKingdomGood(kingdom1))
                                war3.joinAttackers(pKingdom);
                        }
                    }
                }
                foreach (Actor unit in pPlot.units)
                {
                    City city2 = unit.city;
                    if (city2 != null && city2.kingdom != kingdom2 && city2.kingdom == kingdom1)
                        city2.joinAnotherKingdom(kingdom2, true);
                }
                int num1 = kingdom1.countCities();
                int num2 = kingdom2.getMaxCities() - pNewCities.Count;
                if (num2 < 0)
                    num2 = 0;
                if (num2 > num1 / 3)
                    num2 = (int)((double)num1 / 3.0);
                int num3 = 0;
                while (num3 < num2 && DiplomacyHelpersRebellion.checkMoreAlignedCities(kingdom2, kingdom1, pNewCities, pCheckForHappiness))
                    ++num3;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(War), nameof(War.update))]
    public static class BetterWarsWarUpdatePatch
    {
        private static void Postfix(War __instance)
        {
            BetterWarsManager.TickWar(__instance);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.endWar))]
    public static class BetterWarsWarEndPatch
    {
        private static void Postfix(War pWar, WarWinner pWinner)
        {
            BetterWarsManager.OnWarEnded(pWar, pWinner);
        }
    }

    [HarmonyPatch(typeof(WarManager), nameof(WarManager.update))]
    public static class BetterWarsWarManagerUpdatePatch
    {
        private static void Postfix()
        {
            BetterWarsManager.TickGlobal();
        }
    }
}
