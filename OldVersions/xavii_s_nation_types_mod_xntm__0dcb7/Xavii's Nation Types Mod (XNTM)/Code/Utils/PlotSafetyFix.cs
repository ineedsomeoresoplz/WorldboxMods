using System.Linq;

namespace XNTM.Code.Utils
{
    public static class PlotSafetyFix
    {
        private static PlotAction _origAllianceCreateAction;
        private static PlotCheckerDelegate _origAllianceCreateContinue;
        private static PlotCheckerDelegate _origAllianceJoinContinue;
        private static PlotAction _origNewWarAction;

        public static void Apply()
        {
            var lib = AssetManager.plots_library;
            if (lib == null)
                return;

            var allianceCreate = lib.get("alliance_create");
            if (allianceCreate != null)
            {
                _origAllianceCreateContinue ??= allianceCreate.check_should_continue;
                _origAllianceCreateAction ??= allianceCreate.action;
                allianceCreate.check_should_continue = SafeAllianceCreateContinue;
                allianceCreate.action = SafeAllianceCreateAction;
            }

            var allianceJoin = lib.get("alliance_join");
            if (allianceJoin != null)
            {
                _origAllianceJoinContinue ??= allianceJoin.check_should_continue;
                allianceJoin.check_should_continue = SafeAllianceJoinContinue;
            }

            var newWar = lib.get("new_war");
            if (newWar != null)
            {
                _origNewWarAction ??= newWar.action;
                newWar.action = SafeNewWarAction;
            }
        }

        private static bool SafeAllianceCreateContinue(Actor actor)
        {
            if (actor == null || actor.plot == null)
                return false;
            var target = actor.plot.target_kingdom;
            if (target == null || !target.isAlive())
            {
                target = DiplomacyHelpers.getAllianceTarget(actor.kingdom);
                actor.plot.target_kingdom = target;
            }
            if (target == null || !target.isAlive())
                return false;
            if (actor.plot.units == null || actor.plot.units.Count == 0)
                return false;

            foreach (var unit in actor.plot.units.ToList())
            {
                if (unit == null || !unit.isAlive())
                    continue;
                var kingdom = unit.kingdom;
                if (kingdom == null || !kingdom.isAlive())
                    return false;
                if (kingdom.hasEnemies())
                    return false;
                if (kingdom != actor.kingdom && kingdom != target && !kingdom.isOpinionTowardsKingdomGood(actor.kingdom))
                    return false;
            }
            return _origAllianceCreateContinue?.Invoke(actor) ?? true;
        }

        private static bool SafeAllianceJoinContinue(Actor actor)
        {
            if (actor == null || actor.plot == null)
                return false;
            var targetAlliance = actor.plot.target_alliance;
            if (targetAlliance == null || !targetAlliance.isAlive())
                return false;
            if (actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            if (actor.kingdom.hasEnemies())
                return false;
            if (!targetAlliance.canJoin(actor.kingdom))
                return false;
            if (targetAlliance.hasWars())
                return false;
            return _origAllianceJoinContinue?.Invoke(actor) ?? true;
        }

        private static bool SafeAllianceCreateAction(Actor actor)
        {
            if (actor?.kingdom == null || actor.plot == null)
                return false;
            var partner = actor.plot.target_kingdom;
            if (partner == null || !partner.isAlive() || partner == actor.kingdom)
                partner = DiplomacyHelpers.getAllianceTarget(actor.kingdom);
            if (partner == null || !partner.isAlive() || partner == actor.kingdom)
                return false;
            if (World.world?.alliances == null)
                return false;
            actor.plot.target_kingdom = partner;
            var alliance = actor.plot.target_alliance;
            if (alliance != null && !alliance.isAlive())
                alliance = null;
            alliance ??= GetAllianceBetween(actor.kingdom, partner);
            alliance ??= World.world.alliances.newAlliance(actor.kingdom, partner);
            if (alliance == null || !alliance.isAlive())
                return false;
            actor.plot.target_alliance = alliance;
            var result = _origAllianceCreateAction?.Invoke(actor);
            if (result.HasValue && result.Value)
                return true;
            alliance = actor.plot.target_alliance ?? alliance;
            if (alliance == null || !alliance.isAlive())
                return false;
            var units = actor.plot.units;
            if (units != null)
            {
                foreach (var unit in units.ToList())
                {
                    if (unit != null && unit.isAlive())
                        unit.leavePlot();
                }
            }
            alliance.recalculate();
            return true;
        }

        private static bool SafeNewWarAction(Actor actor)
        {
            if (actor?.kingdom == null || actor.plot == null)
                return false;
            var attacker = actor.kingdom;
            var defender = actor.plot.target_kingdom;
            if (defender == null || !defender.isAlive())
                defender = DiplomacyHelpers.getWarTarget(attacker);
            if (defender == null || defender == attacker)
                return false;
            actor.plot.target_kingdom = defender;
            var result = _origNewWarAction?.Invoke(actor);
            if (result.HasValue && result.Value)
                return true;
            var war = World.world.diplomacy.startWar(attacker, defender, WarTypeLibrary.normal);
            if (war == null)
                return false;
            actor.plot.target_war = war;
            return true;
        }

        private static Alliance GetAllianceBetween(Kingdom a, Kingdom b)
        {
            if (a == null || b == null)
                return null;
            var alliance = a.getAlliance();
            if (alliance != null && alliance.hasKingdom(b))
                return alliance;
            alliance = b.getAlliance();
            if (alliance != null && alliance.hasKingdom(a))
                return alliance;
            return null;
        }
    }
}
