using NeoModLoader.api;
using XNTM.Code.Data;
using XNTM.Code.Features;
using XNTM.Code.Features.BetterWars;
using XNTM.Code.Features.Council;
using XNTM.Code.Utils;

namespace XNTM.Code
{
    public class XNTM : BasicMod<XNTM>
    {
        private static bool _worldLogsRegistered;

        protected override void OnModLoad()
        {
            XntmConfig.Load();
            WorldLawsFeature.Register();
            Utils.NationTypeManager.RegisterTraits();
            Utils.NationTypeOpinionBuilder.Register();
            NationTypeTask.Register();
            NationTypePlot.Register();
            LocalizationPrewarmer.Register();
            LocalizationPrewarmer.Prewarm();
            RegisterWorldLogs();
        }

        private void RegisterWorldLogs()
        {
            if (_worldLogsRegistered)
                return;

            var library = AssetManager.world_log_library;
            if (library == null)
                return;

            DisableRandom(WorldLogLibrary.king_fled_capital);
            DisableRandom(WorldLogLibrary.king_fled_city);
            DisableRandom(WorldLogLibrary.king_killed);
            DisableRandom(WorldLogLibrary.kingdom_fractured);
            DisableRandom(WorldLogLibrary.kingdom_shattered);
            DisableRandom(WorldLogLibrary.kingdom_royal_clan_new);
            DisableRandom(WorldLogLibrary.kingdom_royal_clan_changed);
            DisableRandom(WorldLogLibrary.kingdom_royal_clan_dead);

            WorldLogAssets.WhitePeace = library.add(new WorldLogAsset
            {
                id = "xntm_bw_white_peace",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_good,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.StatusQuo = library.add(new WorldLogAsset
            {
                id = "xntm_bw_status_quo",
                group = "wars",
                path_icon = "ui/Icons/iconFlagWhite",
                color = Toolbox.color_log_neutral,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.Ceasefire = library.add(new WorldLogAsset
            {
                id = "xntm_bw_ceasefire",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_neutral,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.MediatedPeace = library.add(new WorldLogAsset
            {
                id = "xntm_bw_mediated",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_good,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.MediatedPeaceDenied = library.add(new WorldLogAsset
            {
                id = "xntm_bw_mediated_denied",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.ConditionalSurrender = library.add(new WorldLogAsset
            {
                id = "xntm_bw_conditional",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.Tribute = library.add(new WorldLogAsset
            {
                id = "xntm_bw_tribute",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_neutral,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.Puppet = library.add(new WorldLogAsset
            {
                id = "xntm_bw_puppet",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.GoalAchieved = library.add(new WorldLogAsset
            {
                id = "xntm_bw_goal",
                group = "wars",
                path_icon = "ui/Icons/actor_traits/iconPacifist",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.Independence = library.add(new WorldLogAsset
            {
                id = "xntm_bw_independence",
                group = "wars",
                path_icon = "ui/Icons/iconDiplomacy",
                color = Toolbox.color_log_good,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.OverlordJoins = library.add(new WorldLogAsset
            {
                id = "xntm_bw_overlord",
                group = "wars",
                path_icon = "ui/Icons/iconCrown",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.PuppetSideSwap = library.add(new WorldLogAsset
            {
                id = "xntm_bw_puppet_side_swap",
                group = "wars",
                path_icon = "ui/Icons/iconCrown",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            WorldLogAssets.Resistance = library.add(new WorldLogAsset
            {
                id = "xntm_bw_resistance",
                group = "wars",
                path_icon = "ui/Icons/iconFlagWhite",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$attacker$", msg.getSpecial(1));
                    text = text.Replace("$defender$", msg.getSpecial(2));
                    text = text.Replace("$reason$", msg.getSpecial(3));
                })
            });

            CouncilLogAssets.CouncilorElected = library.add(new WorldLogAsset
            {
                id = "xntm_councilor_elected",
                group = "kings",
                path_icon = "ui/Icons/iconDiplomacy",
                color = Toolbox.color_log_neutral,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$name$", msg.getSpecial(1));
                    text = text.Replace("$council_title$", msg.getSpecial(2));
                    string summary = msg.getSpecial(3);
                    if (string.IsNullOrEmpty(summary))
                        summary = CouncilManager.GetCouncilSummary(msg.kingdom);
                    text = text.Replace("$council_summary$", summary);
                    text = text.Replace("$kingdom$", msg.kingdom?.name ?? msg.getSpecial(1));
                })
            });

            CouncilLogAssets.CouncilorDead = library.add(new WorldLogAsset
            {
                id = "xntm_councilor_dead",
                group = "kings",
                path_icon = "ui/Icons/iconDead",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$name$", msg.getSpecial(1));
                    text = text.Replace("$council_title$", msg.getSpecial(2));
                    string summary = CouncilManager.GetCouncilSummary(msg.kingdom);
                    text = text.Replace("$council_summary$", summary);
                    text = text.Replace("$kingdom$", msg.kingdom?.name ?? msg.getSpecial(1));
                })
            });

            CouncilLogAssets.CouncilorKilled = library.add(new WorldLogAsset
            {
                id = "xntm_councilor_killed",
                group = "kings",
                path_icon = "ui/Icons/actor_traits/iconKingslayer",
                color = Toolbox.color_log_warning,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$name$", msg.getSpecial(1));
                    text = text.Replace("$council_title$", msg.getSpecial(2));
                    string summary = CouncilManager.GetCouncilSummary(msg.kingdom);
                    text = text.Replace("$council_summary$", summary);
                    text = text.Replace("$kingdom$", msg.kingdom?.name ?? msg.getSpecial(1));
                    text = text.Replace("$killer$", msg.getSpecial(3));
                })
            });

            library.add(new WorldLogAsset
            {
                id = "xntm_nation_type_changed",
                group = "kingdoms",
                path_icon = "ui/Icons/iconKingdom",
                color = Toolbox.color_log_neutral,
                text_replacer = (WorldLogTextFormatter)((WorldLogMessage msg, ref string text) =>
                {
                    text = text.Replace("$kingdom$", msg.getSpecial(1));
                    text = text.Replace("$old_nation_type$", msg.getSpecial(2));
                    text = text.Replace("$new_nation_type$", msg.getSpecial(3));
                })
            });

            _worldLogsRegistered = true;
        }

        private void DisableRandom(WorldLogAsset asset)
        {
            if (asset == null)
                return;
            asset.random_ids = 0;
        }
    }
}
