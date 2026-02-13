using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Features
{
    public static class NationTypePlot
    {
        private const string PlotId = "xntm_change_nation_type";

        public static void Register()
        {
            var library = AssetManager.plots_library;
            if (library == null)
                return;

            PlotAsset asset = library.get(PlotId);
            if (asset == null)
            {
                asset = new PlotAsset
                {
                    id = PlotId,
                    limit_members = 1,
                    pot_rate = 1,
                    min_level = 5,
                    min_intelligence = 2,
                    min_diplomacy = 3,
                    min_warfare = 2,
                    min_stewardship = 2,
                    can_be_done_by_king = true,
                    can_be_done_by_leader = true,
                    requires_diplomacy = false,
                    requires_rebellion = false,
                    progress_needed = 45f,
                    money_cost = 0,
                    is_basic_plot = true,
                    rarity = Rarity.R0_Normal,
                    group_id = "culture",
                    priority = 95,
                    check_is_possible = CheckPossible,
                    check_can_be_forced = CheckPossible,
                    check_should_continue = CheckContinue,
                    try_to_start_advanced = TryStart,
                    action = Execute
                };
                asset.get_formatted_description = Describe;
                asset.path_icon = "plots/icons/plot_new_culture";
                library.add(asset);
            }
        }

        private static bool CheckPossible(Actor actor)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            if (!actor.isKing() && !actor.isCityLeader())
                return false;
            NationTypeDefinition target = NationTypeManager.GetNaturalType(actor.kingdom);
            if (target == null)
                return false;
            return target != NationTypeManager.GetDefinition(actor.kingdom);
        }

        private static bool CheckContinue(Actor actor)
        {
            if (actor == null || actor.plot == null)
                return false;
            var kingdom = actor.plot.target_kingdom;
            if (kingdom == null || !kingdom.isAlive())
                return false;
            return CheckPossible(actor);
        }

        private static bool TryStart(Actor actor, PlotAsset asset, bool forced)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            Plot plot = World.world.plots.newPlot(actor, asset, forced);
            plot.target_kingdom = actor.kingdom;
            return plot.target_kingdom != null;
        }

        private static bool Execute(Actor actor)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            NationTypeDefinition target = NationTypeManager.GetNaturalType(actor.kingdom);
            if (target == null)
                return false;
            return NationTypeManager.TrySetType(actor.kingdom, target, true);
        }

        private static string Describe(Plot plot)
        {
            Kingdom kingdom = plot?.target_kingdom ?? plot?.getAuthor()?.kingdom;
            NationTypeDefinition target = NationTypeManager.GetNaturalType(kingdom);
            if (target == null)
                return LocalizedTextManager.getText("plot_xntm_change_nation_type_info");
            string name = target.GetLocalizedName();
            return LocalizedTextManager.getText("plot_xntm_change_nation_type_info").Replace("{TYPE}", name);
        }
    }
}
