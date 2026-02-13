using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Features
{
    public static class NationTypePlot
    {
        private const string PlotId = "xntm_change_nation_type";
        private const string LastChangedAtKey = "xntm_last_nation_change";
        private static readonly string[] RankedTypeOrder = { "barony", "county", "duchy", "grand_duchy", "principality", "kingdom" };

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
                    can_be_done_by_leader = false,
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

            asset.progress_needed = 62f;
            asset.min_diplomacy = 4;
            asset.min_warfare = 3;
            asset.can_be_done_by_leader = false;

            if (asset != null && !library.basic_plots.Contains(asset))
                library.basic_plots.Add(asset);
        }

        private static bool CheckPossible(Actor actor)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            if (!IsRuler(actor, actor.kingdom))
                return false;
            if (HasActiveNationTypePlot(actor.kingdom))
                return false;
            if (IsOnCooldown(actor.kingdom))
                return false;
            return SelectTarget(actor.kingdom, true) != null;
        }

        private static bool CheckContinue(Actor actor)
        {
            if (actor == null || actor.plot == null)
                return false;
            Kingdom kingdom = actor.plot.target_kingdom;
            if (kingdom == null || !kingdom.isAlive())
                return false;
            if (!IsRuler(actor, kingdom))
                return false;
            return SelectTarget(kingdom, true) != null;
        }

        private static bool TryStart(Actor actor, PlotAsset asset, bool forced)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            if (!IsRuler(actor, actor.kingdom))
                return false;
            if (HasActiveNationTypePlot(actor.kingdom))
                return false;
            if (IsOnCooldown(actor.kingdom))
                return false;
            Plot plot = World.world.plots.newPlot(actor, asset, forced);
            plot.target_kingdom = actor.kingdom;
            return plot.target_kingdom != null;
        }

        private static bool Execute(Actor actor)
        {
            if (actor == null || actor.kingdom == null || !actor.kingdom.isAlive())
                return false;
            if (!IsRuler(actor, actor.kingdom))
                return false;
            if (IsOnCooldown(actor.kingdom))
                return false;
            NationTypeDefinition target = SelectTarget(actor.kingdom, true);
            if (target == null)
                return false;
            bool changed = NationTypeManager.TrySetType(actor.kingdom, target, true);
            if (changed)
                MarkChanged(actor.kingdom);
            return changed;
        }

        private static string Describe(Plot plot)
        {
            Kingdom kingdom = plot?.target_kingdom ?? plot?.getAuthor()?.kingdom;
            NationTypeDefinition target = SelectTarget(kingdom, false);
            if (target == null)
                return LocalizedTextManager.getText("plot_xntm_change_nation_type_info");
            string name = target.GetLocalizedName();
            return LocalizedTextManager.getText("plot_xntm_change_nation_type_info").Replace("{TYPE}", name);
        }

        private static NationTypeDefinition SelectTarget(Kingdom kingdom, bool randomize)
        {
            var pool = BuildEligiblePool(kingdom);
            if (pool.Count == 0)
                return null;
            if (!randomize)
                return pool[0];
            NationTypeDefinition political = NationTypeManager.SelectPoliticalTarget(kingdom, pool);
            if (political != null)
                return political;
            return pool[Randy.randomInt(0, pool.Count)];
        }

        private static System.Collections.Generic.List<NationTypeDefinition> BuildEligiblePool(Kingdom kingdom)
        {
            var result = new System.Collections.Generic.List<NationTypeDefinition>();
            if (kingdom == null || !kingdom.isAlive())
                return result;

            NationTypeDefinition current = NationTypeManager.GetDefinition(kingdom);
            var definitions = NationTypeManager.Definitions;
            for (int i = 0; i < definitions.Count; i++)
            {
                NationTypeDefinition definition = definitions[i];
                if (definition == null || definition == current)
                    continue;
                if (!NationTypeManager.IsEligible(kingdom, definition))
                    continue;
                if (!NationTypeManager.IsAllowedForAutoReform(definition))
                    continue;
                result.Add(definition);
            }

            if (result.Count == 0)
                return result;

            string highestRank = null;
            for (int i = RankedTypeOrder.Length - 1; i >= 0; i--)
            {
                string rankId = RankedTypeOrder[i];
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j].Id == rankId)
                    {
                        highestRank = rankId;
                        i = -1;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(highestRank))
                return result;

            var filtered = new System.Collections.Generic.List<NationTypeDefinition>(result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                NationTypeDefinition definition = result[i];
                bool isRanked = false;
                for (int j = 0; j < RankedTypeOrder.Length; j++)
                {
                    if (definition.Id == RankedTypeOrder[j])
                    {
                        isRanked = true;
                        break;
                    }
                }
                if (!isRanked || definition.Id == highestRank)
                    filtered.Add(definition);
            }

            return filtered;
        }

        private static bool IsOnCooldown(Kingdom kingdom)
        {
            if (kingdom?.data?.custom_data_float == null)
                return false;
            if (!kingdom.data.custom_data_float.dict.TryGetValue(LastChangedAtKey, out float lastChanged))
                return false;
            float yearsSinceLastChanged = Date.getYearsSince((double)lastChanged);
            float cooldown = NationTypeManager.GetAutoReformCooldownYears(kingdom) + 8f;
            if (NationTypeManager.HasRecentLeadershipShift(kingdom, 28f))
                cooldown *= 0.65f;
            return yearsSinceLastChanged < cooldown;
        }

        private static void MarkChanged(Kingdom kingdom)
        {
            if (kingdom?.data == null)
                return;
            kingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            kingdom.data.custom_data_float.dict[LastChangedAtKey] = (float)World.world.getCurWorldTime();
        }

        private static bool IsRuler(Actor actor, Kingdom kingdom)
        {
            if (actor == null || kingdom == null || actor.kingdom != kingdom || !actor.isAlive())
                return false;
            if (actor.isKing())
                return true;
            var rulers = CouncilManager.GetRulers(kingdom);
            for (int i = 0; i < rulers.Count; i++)
            {
                if (rulers[i] == actor)
                    return true;
            }
            return false;
        }

        private static bool HasActiveNationTypePlot(Kingdom kingdom)
        {
            if (kingdom == null || World.world?.plots == null)
                return false;
            foreach (Plot plot in World.world.plots)
            {
                if (plot == null || !plot.isActive())
                    continue;
                PlotAsset plotAsset = plot.getAsset();
                if (plotAsset == null || plotAsset.id != PlotId)
                    continue;
                Kingdom plotKingdom = plot.target_kingdom ?? plot.getAuthor()?.kingdom;
                if (plotKingdom == kingdom)
                    return true;
            }
            return false;
        }
    }
}
