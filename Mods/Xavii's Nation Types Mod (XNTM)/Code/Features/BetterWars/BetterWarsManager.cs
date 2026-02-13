using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Features.BetterWars
{
    public static class BetterWarsManager
    {
        private const string ReasonKey = "xntm_bw_reason";
        private const string NextCheckKey = "xntm_bw_next_check";
        private const string CeasefireResumeKey = "xntm_bw_resume_at";
        private const string CeasefireEnemyKey = "xntm_bw_resume_enemy";
        private const string PuppetOfKey = "xntm_bw_puppet_of";
        private const string DemilitarizedUntilKey = "xntm_bw_demil_until";
        private const string ReconstructionUntilKey = "xntm_bw_rebuild_until";
        private const string ResentmentUntilKey = "xntm_bw_resent_until";
        private const string OccupationUntilKey = "xntm_bw_occupy_until";
        private const string ReparationsUntilKey = "xntm_bw_reparations_until";
        private const string ReparationsTargetKey = "xntm_bw_reparations_target";
        private const string GoalTypeKey = "xntm_bw_goal_type";
        private const string GoalTargetCityKey = "xntm_bw_goal_city";
        private const string StartAttackerCitiesKey = "xntm_bw_start_atk_cities";
        private const string StartDefenderCitiesKey = "xntm_bw_start_def_cities";
        private const string StartAttackerArmyKey = "xntm_bw_start_atk_army";
        private const string StartDefenderArmyKey = "xntm_bw_start_def_army";
        private const string StartCityOwnersKey = "xntm_bw_start_city_owners";
        private const string SettlementModeKey = "xntm_bw_settle_mode";
        private const string SettlementAppliedKey = "xntm_bw_settle_applied";
        private const string SettlementOwnerOverridesKey = "xntm_bw_settle_owner_overrides";
        private const string CityOwnerHistoryKey = "xntm_bw_owner_history";
        private const string SettlementModeRestore = "restore";
        private const string SettlementModeKeep = "keep";
        private const float InitialPeaceCheckDelay = 20f;
        private const float MinYearsBeforePeaceActions = 1.25f;

        private static double _lastGlobalTick;
        private static double _lastSupremeRefresh;
        private static readonly string[] StrategicResourceIds = { "gold", "wood", "stone", "common_metals" };
        private static readonly MethodInfo CityAddZoneMethod = AccessTools.Method(typeof(City), "addZone");
        private static readonly MethodInfo CityRemoveZoneMethod = AccessTools.Method(typeof(City), "removeZone");
        private static readonly FieldInfo CityBorderZonesField = AccessTools.Field(typeof(City), "border_zones");
        private static readonly FieldInfo CityZonesField = AccessTools.Field(typeof(City), "zones");
        private static readonly MethodInfo CitySetCultureMethod = AccessTools.Method(typeof(City), "setCulture");
        private static readonly MethodInfo CitySetReligionMethod = AccessTools.Method(typeof(City), "setReligion");
        private static readonly MethodInfo CityMakeOwnKingdomMethod = AccessTools.Method(typeof(City), "makeOwnKingdom");

        public static void OnWarStarted(War war, Kingdom attacker, Kingdom defender)
        {
            if (!ShouldProcess(war))
                return;

            EnsureContainers(war.data);
            string reason = war.data.custom_data_string.dict.ContainsKey(ReasonKey)
                ? war.data.custom_data_string.dict[ReasonKey]
                : SelectReason(attacker, defender);
            reason = EnsureReasonAvailable(attacker, defender, reason);
            war.data.custom_data_string.dict[ReasonKey] = reason;
            SetWarGoal(war, attacker, defender);
            war.data.custom_data_float.dict[StartAttackerCitiesKey] = attacker?.countCities() ?? 0;
            war.data.custom_data_float.dict[StartDefenderCitiesKey] = defender?.countCities() ?? 0;
            war.data.custom_data_float.dict[StartAttackerArmyKey] = attacker?.countTotalWarriors() ?? 0;
            war.data.custom_data_float.dict[StartDefenderArmyKey] = defender?.countTotalWarriors() ?? 0;
            SaveInitialCityOwners(war);
            SetSettlementMode(war, IsTerritoryReason(reason) ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, false);
            EnsureDefenderOverlordJoins(attacker, defender);
            EnforcePuppetWarSideConsistency(war);
            ScheduleNextPeaceCheck(war, InitialPeaceCheckDelay);
        }

        public static void TickWar(War war)
        {
            if (!ShouldProcess(war) || war.hasEnded())
                return;

            EnsureContainers(war.data);
            EnforcePuppetWarSideConsistency(war);
            if (war.hasEnded())
                return;
            double now = World.world.getCurWorldTime();
            if (TryGetFloat(war.data, NextCheckKey, out float next) && now < next)
                return;

            float interval = Mathf.Lerp(12f, 28f, Mathf.Clamp01(war.getDuration() / 15f));
            ScheduleNextPeaceCheck(war, interval);

            var ctx = BuildContext(war);
            if (ResolveGoalIfMet(war, ctx))
                return;
            TryTriggerPeaceAction(war, ctx);
        }

        public static void OnWarEnded(War war, WarWinner winner)
        {
            if (!ShouldProcess(war))
                return;

            EnsureContainers(war.data);
            FinalizeWarSettlement(war, winner);
            ApplyPostWarEffects(war, winner);
        }

        public static void TickGlobal()
        {
            double now = World.world.getCurWorldTime();
            if (now - _lastGlobalTick < 8f)
                return;
            _lastGlobalTick = now;

            ResumeCeasefires(now);
            ProcessScheduledReconstruction(now);
            ProcessResentment(now);
            if (now - _lastSupremeRefresh >= 6f)
            {
                _lastSupremeRefresh = now;
                RefreshSupremeKingdom();
            }
            CleanupConflictingPuppetAlliances();
        }

        private static void TryTriggerPeaceAction(War war, WarContext ctx)
        {
            if (ctx == null || ctx.Attacker == null || ctx.Defender == null)
                return;
            if (ctx.WarDurationYears < MinYearsBeforePeaceActions && !ctx.DefenderLostCoreCity && !ctx.AttackerLostCoreCity)
                return;

            float desperation = ctx.DefenderDesperation;
            float exhaustion = ctx.GlobalExhaustion;
            float diplomacyBias = Mathf.Max(ctx.AttackerDiplomacy, ctx.DefenderDiplomacy);

            if (desperation < 0.15f && exhaustion < 0.1f && ctx.WarDurationYears < 2f && !ctx.DefenderLostCoreCity)
                return;

            if (ctx.AttackerLosingBadly && (ctx.WarDurationYears > 1.15f || exhaustion > 0.35f))
            {
                if (ApplyDefenderCounterSettlement(war, ctx))
                    return;
            }

            if (desperation > 0.55f)
            {
                if (ctx.DefenderLosingBadly)
                {
                    if (ReasonBlocksAttackerConcessions(ctx.Reason))
                    {
                        if (exhaustion > 0.35f || ctx.WarDurationYears > 4f)
                            ApplyWhitePeace(war, ctx);
                        return;
                    }
                    if (ctx.CanPuppet && ShouldAllowPuppetAgreement(ctx))
                    {
                        ApplyPuppetAgreement(war, ctx);
                        return;
                    }
                    ApplyConditionalSurrender(war, ctx);
                    return;
                }

                if (ctx.CanPayTribute && !ReasonBlocksAttackerConcessions(ctx.Reason))
                {
                    ApplyTributeAgreement(war, ctx);
                    return;
                }
            }

            if (exhaustion > 0.45f && diplomacyBias > 6f)
            {
                ApplyMediatedPeace(war, ctx);
                return;
            }

            if (ctx.IsStalemate || ctx.WarDurationYears > 8f)
            {
                if (ShouldUseStatusQuo(ctx))
                {
                    ApplyStatusQuoPeace(war, ctx);
                    return;
                }

                if (exhaustion > 0.25f && ctx.CanResumeLater)
                {
                    ApplyCeasefire(war, ctx);
                    return;
                }

                ApplyWhitePeace(war, ctx);
                return;
            }

            if (ctx.AttackerBoldButThin && ctx.DefenderStillStrong && !ReasonBlocksAttackerConcessions(ctx.Reason))
            {
                ApplyConditionalSurrender(war, ctx);
                return;
            }

            if (ctx.AttackerGoalStalled && exhaustion > 0.2f)
            {
                ApplyMediatedPeace(war, ctx);
            }
        }

        private static void ApplyWhitePeace(War war, WarContext ctx)
        {
            SetSettlementMode(war, SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.WhitePeace, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyStatusQuoPeace(War war, WarContext ctx)
        {
            SetSettlementMode(war, SettlementModeKeep);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.StatusQuo ?? WorldLogAssets.WhitePeace, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyCeasefire(War war, WarContext ctx)
        {
            double resumeAt = World.world.getCurWorldTime() + UnityEngine.Random.Range(25f, 40f);

            MarkCeasefire(ctx.Attacker, ctx.Defender, resumeAt);
            MarkCeasefire(ctx.Defender, ctx.Attacker, resumeAt);

            SetSettlementMode(war, ShouldUseStatusQuo(ctx) ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.Ceasefire, ctx.Attacker, ctx.Defender, ctx.Reason, Mathf.RoundToInt((float)(resumeAt - World.world.getCurWorldTime())));
        }

        private static void ApplyMediatedPeace(War war, WarContext ctx)
        {
            if (war == null || ctx == null)
                return;
            if (!TrySelectMediator(war, out var mediator))
                return;

            List<Kingdom> participants = GetWarParticipants(war);
            if (participants.Count < 2)
                return;

            bool multiPartyWar = participants.Count > 2;
            string context = multiPartyWar
                ? $"in the {BuildWarDisplayName(war)}"
                : $"between {ctx.Attacker?.name ?? "Unknown"} and {ctx.Defender?.name ?? "Unknown"}";
            string reasonDetail = BuildMediationReasonDetail(ctx.Reason);

            var deniers = new List<Kingdom>();
            for (int i = 0; i < participants.Count; i++)
            {
                Kingdom participant = participants[i];
                if (participant == null || !participant.isAlive())
                    continue;
                if (Randy.randomChance(0.3f))
                    deniers.Add(participant);
            }

            if (deniers.Count > 0)
            {
                string denialDetail = BuildMediationDenialDetail(reasonDetail, multiPartyWar, deniers);
                LogMediatedPeace(WorldLogAssets.MediatedPeaceDenied, mediator, context, denialDetail);
                return;
            }

            bool territoryConcession = false;
            bool attackerConceded = false;
            bool defenderConceded = false;

            if (ctx.DefenderLosingBadly || ctx.DefenderCityLossRatio > ctx.AttackerCityLossRatio + 0.06f || Randy.randomChance(0.35f))
                defenderConceded = ApplyConcessionFromLoser(war, ctx, ctx.Defender, ctx.Attacker, true, 0.48f, out territoryConcession);
            if (ctx.AttackerLosingBadly || ctx.AttackerCityLossRatio > ctx.DefenderCityLossRatio + 0.06f || Randy.randomChance(0.35f))
                attackerConceded = ApplyConcessionFromLoser(war, ctx, ctx.Attacker, ctx.Defender, false, 0.44f, out territoryConcession);

            if (ctx.TerritoryWar && (attackerConceded || defenderConceded || HasAnyCapturedCities(ctx)))
                SetSettlementMode(war, SettlementModeKeep);
            else
                SetSettlementMode(war, SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogMediatedPeace(WorldLogAssets.MediatedPeace, mediator, context, reasonDetail);
        }

        private static void ApplyConditionalSurrender(War war, WarContext ctx)
        {
            bool territoryConcession = false;
            if (!ApplyConcessionFromLoser(war, ctx, ctx.Defender, ctx.Attacker, true, 0.66f, out territoryConcession))
            {
                City transferred = TransferBorderCity(ctx.Defender, ctx.Attacker);
                if (transferred != null)
                {
                    territoryConcession = true;
                    RegisterSettledCityOwner(war, transferred, ctx.Attacker);
                }
            }

            SetSettlementMode(war, ctx.TerritoryWar ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.ConditionalSurrender, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyTributeAgreement(War war, WarContext ctx)
        {
            double until = World.world.getCurWorldTime() + 60f;
            SetFloat(ctx.Defender.data, ReparationsUntilKey, (float)until);
            SetLong(ctx.Defender.data, ReparationsTargetKey, ctx.Attacker.id);
            bool territoryConcession = false;
            ApplyConcessionFromLoser(war, ctx, ctx.Defender, ctx.Attacker, true, 0.38f, out territoryConcession);
            SetSettlementMode(war, SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.Tribute, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyPuppetAgreement(War war, WarContext ctx)
        {
            if (!ShouldAllowPuppetAgreement(ctx))
            {
                ApplyConditionalSurrender(war, ctx);
                return;
            }
            SetLong(ctx.Defender.data, PuppetOfKey, ctx.Attacker.id);
            SetFloat(ctx.Defender.data, DemilitarizedUntilKey, (float)(World.world.getCurWorldTime() + 90f));
            SetSettlementMode(war, ctx.TerritoryWar ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.Puppet, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyGoalPeace(War war, WarContext ctx, string goalLabel)
        {
            if (!string.Equals(goalLabel, "independence", StringComparison.Ordinal) && !ReasonBlocksAttackerConcessions(ctx.Reason))
            {
                bool territoryConcession = false;
                ApplyConcessionFromLoser(war, ctx, ctx.Defender, ctx.Attacker, true, 0.72f, out territoryConcession);
            }
            SetSettlementMode(war, IsTerritoryGoal(ctx.Goal?.Type, ctx.Reason) ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.GoalAchieved, ctx.Attacker, ctx.Defender, goalLabel);
        }

        private static bool ApplyDefenderCounterSettlement(War war, WarContext ctx)
        {
            if (war == null || ctx == null || ctx.Attacker == null || ctx.Defender == null)
                return false;

            bool territoryConcession = false;
            bool applied = ApplyConcessionFromLoser(war, ctx, ctx.Attacker, ctx.Defender, false, 0.68f, out territoryConcession);
            if (!applied && string.Equals(ctx.Reason, "purification", StringComparison.Ordinal))
            {
                ApplyDemilitarization(ctx.Attacker, World.world.getCurWorldTime() + 90f);
                applied = true;
            }
            if (!applied)
                return false;

            SetSettlementMode(war, ctx.TerritoryWar ? SettlementModeKeep : SettlementModeRestore);
            MarkSettlementApplied(war, true);
            World.world.wars.endWar(war, WarWinner.Defenders);
            LogBetterWar(WorldLogAssets.GoalAchieved, ctx.Defender, ctx.Attacker, "defensive_victory");
            return true;
        }

        private static void ApplyIndependence(Kingdom puppet, Kingdom overlord)
        {
            if (puppet == null)
                return;
            if (puppet.data?.custom_data_long != null && puppet.data.custom_data_long.dict.ContainsKey(PuppetOfKey))
                puppet.data.custom_data_long.dict.Remove(PuppetOfKey);
            if (overlord != null)
                LogBetterWar(WorldLogAssets.Independence, puppet, overlord, "independence");
        }

        private static void ApplyPostWarEffects(War war, WarWinner winner)
        {
            var attacker = war.getMainAttacker();
            var defender = war.getMainDefender();
            if (attacker == null || defender == null)
                return;

            bool attackerWon = winner == WarWinner.Attackers;
            bool defenderWon = winner == WarWinner.Defenders;

            if (attackerWon)
            {
                float occupyUntil = (float)(World.world.getCurWorldTime() + 30f);
                SetFloat(defender.data, OccupationUntilKey, occupyUntil);
                SetFloat(defender.data, ResentmentUntilKey, occupyUntil + 60f);
            }
            else if (defenderWon)
            {
                float rebuild = (float)(World.world.getCurWorldTime() + 25f);
                SetFloat(attacker.data, ReconstructionUntilKey, rebuild);
            }
            if (attackerWon && TryGetLong(defender.data, PuppetOfKey, out long overlordId) && overlordId == attacker.id)
                SetFloat(defender.data, DemilitarizedUntilKey, (float)(World.world.getCurWorldTime() + 60f));
        }

        private static void ResumeCeasefires(double now)
        {
            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom?.data?.custom_data_float == null || kingdom.data.custom_data_string == null)
                    continue;

                if (TryGetFloat(kingdom.data, CeasefireResumeKey, out float resumeAt) && resumeAt > 0f && now >= resumeAt)
                {
                    if (!TryGetLong(kingdom.data, CeasefireEnemyKey, out long enemyId))
                        continue;

                    Kingdom enemy = World.world.kingdoms.get(enemyId);
                    if (enemy == null || !enemy.isAlive() || !kingdom.isAlive())
                    {
                        kingdom.data.custom_data_float.dict.Remove(CeasefireResumeKey);
                        kingdom.data.custom_data_long?.dict.Remove(CeasefireEnemyKey);
                        continue;
                    }

                    WarTypeAsset type = AssetManager.war_types_library.get("normal");
                    if (!World.world.wars.isInWarWith(kingdom, enemy))
                        World.world.wars.newWar(kingdom, enemy, type);

                    kingdom.data.custom_data_float.dict.Remove(CeasefireResumeKey);
                    kingdom.data.custom_data_long?.dict.Remove(CeasefireEnemyKey);
                }
            }
        }

        private static void ProcessScheduledReconstruction(double now)
        {
            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom?.data?.custom_data_float == null)
                    continue;

                if (TryGetFloat(kingdom.data, ReconstructionUntilKey, out float until) && now >= until)
                    kingdom.data.custom_data_float.dict.Remove(ReconstructionUntilKey);
                if (TryGetFloat(kingdom.data, ReparationsUntilKey, out float repUntil) && now >= repUntil)
                {
                    kingdom.data.custom_data_float.dict.Remove(ReparationsUntilKey);
                    kingdom.data.custom_data_long?.dict.Remove(ReparationsTargetKey);
                }
            }
        }

        private static void ProcessResentment(double now)
        {
            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom?.data?.custom_data_float == null)
                    continue;
                if (TryGetFloat(kingdom.data, ResentmentUntilKey, out float until) && until > now)
                {
                    if (kingdom.isInWar())
                        continue;
                    if (UnityEngine.Random.value < 0.01f)
                    {
                        ApplyDemilitarization(kingdom, now + 12f);
                        LogBetterWar(WorldLogAssets.Resistance, kingdom, null, "resentment");
                    }
                }
            }
        }

        private static void EnforcePuppetWarSideConsistency(War war)
        {
            if (war == null || war.hasEnded() || war.isTotalWar() || World.world?.wars == null)
                return;

            List<Kingdom> participants = GetWarParticipants(war);
            for (int i = 0; i < participants.Count; i++)
            {
                Kingdom puppet = participants[i];
                if (puppet == null || !puppet.isAlive())
                    continue;
                if (!TryGetOverlord(puppet, out var overlord) || overlord == null || !overlord.isAlive() || overlord == puppet)
                    continue;

                ResolvePuppetWarConflict(war, puppet, overlord);
                if (war.hasEnded())
                    return;
            }

            EnsureWarHasBothSides(war);
        }

        private static void CleanupConflictingPuppetAlliances()
        {
            if (World.world?.kingdoms == null)
                return;

            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom == null || !kingdom.isAlive())
                    continue;
                if (!TryGetOverlord(kingdom, out var overlord) || overlord == null || !overlord.isAlive() || overlord == kingdom)
                    continue;
                TryBreakConflictingAlliance(kingdom, overlord);
            }
        }

        private static void ResolvePuppetWarConflict(War war, Kingdom puppet, Kingdom overlord)
        {
            if (!TryGetDesiredPuppetSide(war, puppet, overlord, out bool targetAttackers, out Kingdom supportTarget, out string reasonKey))
                return;

            bool wasOpposite = targetAttackers ? war.isDefender(puppet) : war.isAttacker(puppet);
            if (!wasOpposite)
                return;

            bool brokeAlliance = TryBreakConflictingAlliance(puppet, overlord, war, targetAttackers, true);
            if (war.hasEnded() || !puppet.isAlive())
            {
                if (brokeAlliance)
                    LogBetterWar(WorldLogAssets.PuppetSideSwap, puppet, supportTarget ?? overlord, reasonKey);
                return;
            }

            if (!MoveKingdomToWarSide(war, puppet, targetAttackers))
            {
                bool nowOnDesiredSide = targetAttackers
                    ? war.isAttacker(puppet) && !war.isDefender(puppet)
                    : war.isDefender(puppet) && !war.isAttacker(puppet);
                if (nowOnDesiredSide || brokeAlliance)
                    LogBetterWar(WorldLogAssets.PuppetSideSwap, puppet, supportTarget ?? overlord, reasonKey);
                EnsureWarHasBothSides(war);
                return;
            }

            LogBetterWar(WorldLogAssets.PuppetSideSwap, puppet, supportTarget ?? overlord, reasonKey);
            EnsureWarHasBothSides(war);
        }

        private static bool TryGetDesiredPuppetSide(
            War war,
            Kingdom puppet,
            Kingdom overlord,
            out bool targetAttackers,
            out Kingdom supportTarget,
            out string reasonKey)
        {
            targetAttackers = false;
            supportTarget = null;
            reasonKey = string.Empty;

            bool puppetAttacker = war.isAttacker(puppet);
            bool puppetDefender = war.isDefender(puppet);
            if (!puppetAttacker && !puppetDefender)
                return false;

            if (puppetAttacker)
            {
                if (war.isDefender(overlord))
                {
                    targetAttackers = false;
                    supportTarget = overlord;
                    reasonKey = "overlord";
                    return true;
                }

                Kingdom defenderAlly = FindOverlordAllyOnWarSide(war, overlord, false, puppet);
                if (defenderAlly != null)
                {
                    targetAttackers = false;
                    supportTarget = defenderAlly;
                    reasonKey = "overlord_ally";
                    return true;
                }
            }

            if (puppetDefender)
            {
                if (war.isAttacker(overlord))
                {
                    targetAttackers = true;
                    supportTarget = overlord;
                    reasonKey = "overlord";
                    return true;
                }

                Kingdom attackerAlly = FindOverlordAllyOnWarSide(war, overlord, true, puppet);
                if (attackerAlly != null)
                {
                    targetAttackers = true;
                    supportTarget = attackerAlly;
                    reasonKey = "overlord_ally";
                    return true;
                }
            }

            return false;
        }

        private static Kingdom FindOverlordAllyOnWarSide(War war, Kingdom overlord, bool attackersSide, Kingdom puppet)
        {
            Alliance alliance = overlord?.getAlliance();
            if (alliance?.kingdoms_hashset == null)
                return null;

            foreach (Kingdom ally in alliance.kingdoms_hashset)
            {
                if (ally == null || !ally.isAlive() || ally == puppet || ally == overlord)
                    continue;
                if (attackersSide)
                {
                    if (war.isAttacker(ally))
                        return ally;
                }
                else if (war.isDefender(ally))
                {
                    return ally;
                }
            }

            return null;
        }

        private static bool MoveKingdomToWarSide(War war, Kingdom kingdom, bool targetAttackers)
        {
            if (war == null || kingdom == null || !kingdom.isAlive() || war.hasEnded())
                return false;

            bool changed = false;

            if (targetAttackers)
            {
                if (!war.isAttacker(kingdom))
                {
                    war.joinAttackers(kingdom);
                    changed = true;
                }

                if (war.isDefender(kingdom))
                {
                    bool wasMainDefender = war.isMainDefender(kingdom);
                    war.removeDefender(kingdom, true);
                    if (wasMainDefender)
                        war.trySelectNewMainDefender();
                    war.prepare();
                    changed = true;
                }
            }
            else
            {
                if (!war.isDefender(kingdom))
                {
                    war.joinDefenders(kingdom);
                    changed = true;
                }

                if (war.isAttacker(kingdom))
                {
                    bool wasMainAttacker = war.isMainAttacker(kingdom);
                    war.removeAttacker(kingdom, true);
                    if (wasMainAttacker)
                        war.trySelectNewMainAttacker();
                    war.prepare();
                    changed = true;
                }
            }

            if (!changed)
                return false;

            if (targetAttackers)
                return war.isAttacker(kingdom) && !war.isDefender(kingdom);
            return war.isDefender(kingdom) && !war.isAttacker(kingdom);
        }

        private static void EnsureWarHasBothSides(War war)
        {
            if (war == null || war.hasEnded() || war.isTotalWar())
                return;
            if (war.countAttackers() > 0 && war.countDefenders() > 0)
                return;
            World.world.wars.endWar(war, WarWinner.Peace);
        }

        private static bool TryBreakConflictingAlliance(
            Kingdom puppet,
            Kingdom overlord,
            War war = null,
            bool targetAttackers = false,
            bool enforceWarSide = false)
        {
            Alliance alliance = puppet?.getAlliance();
            if (alliance == null)
                return false;

            if (!IsAllianceConflictingForPuppet(puppet, overlord, alliance, war, targetAttackers, enforceWarSide))
                return false;

            alliance.leave(puppet);
            return true;
        }

        private static bool IsAllianceConflictingForPuppet(
            Kingdom puppet,
            Kingdom overlord,
            Alliance alliance,
            War war = null,
            bool targetAttackers = false,
            bool enforceWarSide = false)
        {
            if (puppet == null || overlord == null || alliance?.kingdoms_hashset == null)
                return false;

            foreach (Kingdom member in alliance.kingdoms_hashset)
            {
                if (member == null || !member.isAlive() || member == puppet)
                    continue;

                if (IsKingdomConflictingWithOverlordBloc(overlord, member, puppet))
                    return true;

                if (war != null && enforceWarSide)
                {
                    if (targetAttackers)
                    {
                        if (war.isDefender(member))
                            return true;
                    }
                    else if (war.isAttacker(member))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsKingdomConflictingWithOverlordBloc(Kingdom overlord, Kingdom candidate, Kingdom puppet = null)
        {
            if (overlord == null || candidate == null || !overlord.isAlive() || !candidate.isAlive() || candidate == puppet)
                return false;
            if (IsKingdomInOverlordBlocInternal(overlord, candidate))
                return false;
            if (AreKingdomsInConflict(candidate, overlord))
                return true;

            Alliance overlordAlliance = overlord.getAlliance();
            if (overlordAlliance?.kingdoms_hashset == null)
                return false;

            foreach (Kingdom ally in overlordAlliance.kingdoms_hashset)
            {
                if (ally == null || !ally.isAlive() || ally == overlord || ally == candidate || ally == puppet)
                    continue;
                if (AreKingdomsInConflict(candidate, ally))
                    return true;
            }

            return false;
        }

        private static bool AreKingdomsInConflict(Kingdom first, Kingdom second)
        {
            if (first == null || second == null || first == second || !first.isAlive() || !second.isAlive())
                return false;
            if (World.world?.wars == null)
                return false;
            return first.isEnemy(second) || World.world.wars.isInWarWith(first, second);
        }

        private static bool IsKingdomInOverlordBlocInternal(Kingdom overlord, Kingdom kingdom)
        {
            if (overlord == null || kingdom == null || !overlord.isAlive() || !kingdom.isAlive())
                return false;
            if (overlord == kingdom)
                return true;
            Alliance alliance = overlord.getAlliance();
            return alliance != null && alliance.hasKingdom(kingdom);
        }

        private static WarContext BuildContext(War war)
        {
            var attacker = war.getMainAttacker();
            var defender = war.getMainDefender();
            if (attacker == null || defender == null)
                return null;

            string reason = GetReason(war);
            float warDurationYears = war.getDuration();

            float attackerPower = war.countAttackersWarriors() + war.countAttackersCities() * 0.5f;
            float defenderPower = war.countDefendersWarriors() + war.countDefendersCities() * 0.5f;

            Actor atkRuler = attacker.king;
            Actor defRuler = defender.king;

            float atkMood = GetMoodScore(atkRuler);
            float defMood = GetMoodScore(defRuler);
            float atkDip = atkRuler?.stats?.get("diplomacy") ?? 0f;
            float defDip = defRuler?.stats?.get("diplomacy") ?? 0f;
            float atkWar = atkRuler?.stats?.get("warfare") ?? 0f;
            float defWar = defRuler?.stats?.get("warfare") ?? 0f;

            int attackerCitiesStart = GetStoredInt(war.data, StartAttackerCitiesKey, attacker.countCities());
            int defenderCitiesStart = GetStoredInt(war.data, StartDefenderCitiesKey, defender.countCities());
            int attackerArmyStart = GetStoredInt(war.data, StartAttackerArmyKey, attacker.countTotalWarriors());
            int defenderArmyStart = GetStoredInt(war.data, StartDefenderArmyKey, defender.countTotalWarriors());

            int attackerCitiesNow = attacker.countCities();
            int defenderCitiesNow = defender.countCities();
            int attackerArmyNow = attacker.countTotalWarriors();
            int defenderArmyNow = defender.countTotalWarriors();

            float defenderCityLossRatio = Mathf.Clamp01((defenderCitiesStart - defenderCitiesNow) / Mathf.Max(1f, defenderCitiesStart));
            float defenderArmyLossRatio = Mathf.Clamp01((defenderArmyStart - defenderArmyNow) / Mathf.Max(1f, defenderArmyStart));
            float attackerCityLossRatio = Mathf.Clamp01((attackerCitiesStart - attackerCitiesNow) / Mathf.Max(1f, attackerCitiesStart));
            float attackerArmyLossRatio = Mathf.Clamp01((attackerArmyStart - attackerArmyNow) / Mathf.Max(1f, attackerArmyStart));

            float powerGap = Mathf.Clamp01((attackerPower - defenderPower) / Mathf.Max(1f, attackerPower));
            float defenderPowerGap = Mathf.Clamp01((defenderPower - attackerPower) / Mathf.Max(1f, defenderPower));
            bool defenderLosing = powerGap > 0.22f || defenderCityLossRatio > 0.08f || defenderArmyLossRatio > 0.2f || war.getDeadDefenders() > war.getDeadAttackers() * 1.4f;
            bool defenderLosingBadly = defenderLosing && (powerGap > 0.36f || defenderCityLossRatio > 0.15f || defenderArmyLossRatio > 0.3f || war.getDeadDefenders() > war.getDeadAttackers() * 1.7f);
            bool attackerLosing = defenderPowerGap > 0.22f || attackerCityLossRatio > 0.08f || attackerArmyLossRatio > 0.2f || war.getDeadAttackers() > war.getDeadDefenders() * 1.4f;
            bool attackerLosingBadly = attackerLosing && (defenderPowerGap > 0.36f || attackerCityLossRatio > 0.15f || attackerArmyLossRatio > 0.3f || war.getDeadAttackers() > war.getDeadDefenders() * 1.7f);
            bool stalemate = Mathf.Abs(attackerPower - defenderPower) < Mathf.Max(4f, attackerPower * 0.1f);

            float averageLoyalty = GetAverageLoyalty(defender);
            float loyaltyPressure = Mathf.Clamp01((55f - averageLoyalty) / 55f);
            float populacePressure = GetPopulationSubmissionPressure(defender);
            float desperation = Mathf.Clamp01((1f - defMood) * 0.35f + (defenderLosing ? 0.2f : 0f) + defenderCityLossRatio * 0.35f + defenderArmyLossRatio * 0.25f + loyaltyPressure * 0.2f + Mathf.Clamp01(warDurationYears / 16f));
            float exhaustion = Mathf.Clamp01((war.getDeadAttackers() + war.getDeadDefenders()) / Mathf.Max(1f, war.countTotalArmy() * 2f));
            float globalWarPressure = Mathf.Clamp01(World.world.wars.Count / 6f);

            NationTypeDefinition atkType = NationTypeManager.GetDefinition(attacker);
            NationTypeDefinition defType = NationTypeManager.GetDefinition(defender);
            bool attackerCanEnforcePuppet = atkType.SuccessionMode == NationSuccessionMode.RoyalLine || atkType.SuccessionMode == NationSuccessionMode.Religious;
            float capitulationBias = BuildPuppetCapitulationBias(defender, defType, defRuler);
            float puppetWillingness = Mathf.Clamp01(powerGap * 0.35f + defenderCityLossRatio * 0.3f + defenderArmyLossRatio * 0.25f + populacePressure * 0.2f + loyaltyPressure * 0.15f + Mathf.Clamp01(warDurationYears / 20f) + capitulationBias);
            bool puppetPossible = attackerCanEnforcePuppet
                && warDurationYears >= 1.25f
                && defenderLosingBadly
                && (defenderCityLossRatio > 0.1f || defenderArmyLossRatio > 0.25f || powerGap > 0.3f)
                && puppetWillingness >= 0.5f;
            bool tributePossible = defender.countCities() > 0;
            WarGoal goal = GetGoal(war);
            City goalCity = goal.TargetCityId > 0 ? World.world.cities.get(goal.TargetCityId) : null;
            bool attackerGoalStalled = goalCity != null && goalCity.kingdom != null && goalCity.kingdom != defender && goalCity.kingdom != attacker;
            bool defenderLostCoreCity = goalCity != null && goalCity.kingdom == attacker;
            bool attackerLostCoreCity = attackerCitiesNow < attackerCitiesStart;
            CountCapturedCitiesFromSnapshot(war, attacker, defender, out int attackerCapturedCities, out int defenderCapturedCities);
            bool territoryWar = IsTerritoryReason(reason);
            bool bothHaveDocks = HasDockCity(attacker) && HasDockCity(defender);

            return new WarContext
            {
                War = war,
                Attacker = attacker,
                Defender = defender,
                Reason = reason,
                WarDurationYears = warDurationYears,
                DefenderLosingBadly = defenderLosingBadly,
                AttackerLosingBadly = attackerLosingBadly,
                IsStalemate = stalemate,
                DefenderDesperation = desperation,
                GlobalExhaustion = Mathf.Clamp01(exhaustion + globalWarPressure * 0.2f),
                AttackerDiplomacy = atkDip,
                DefenderDiplomacy = defDip,
                AttackerBoldButThin = atkWar > 6f && attackerPower < defenderPower * 0.9f,
                DefenderStillStrong = defenderPower > attackerPower * 0.75f,
                CanPuppet = puppetPossible,
                CanPayTribute = tributePossible,
                CanResumeLater = true,
                Goal = goal,
                AttackerGoalStalled = attackerGoalStalled,
                DefenderLostCoreCity = defenderLostCoreCity,
                AttackerLostCoreCity = attackerLostCoreCity,
                DefenderCityLossRatio = defenderCityLossRatio,
                DefenderArmyLossRatio = defenderArmyLossRatio,
                AttackerCityLossRatio = attackerCityLossRatio,
                AttackerArmyLossRatio = attackerArmyLossRatio,
                AverageDefenderLoyalty = averageLoyalty,
                PopulationPressure = populacePressure,
                PuppetWillingness = puppetWillingness,
                TerritoryWar = territoryWar,
                BothHaveDocks = bothHaveDocks,
                AttackerCapturedEnemyCities = attackerCapturedCities,
                DefenderCapturedEnemyCities = defenderCapturedCities
            };
        }

        private static float GetMoodScore(Actor ruler)
        {
            if (ruler?.data == null)
                return 0.6f;

            float happy = ruler.data.happiness;
            return Mathf.Clamp01(happy / 100f);
        }

        private static bool ShouldAllowPuppetAgreement(WarContext ctx)
        {
            if (ctx == null || ctx.Attacker == null || ctx.Defender == null)
                return false;
            if (!ctx.CanPuppet || !ctx.DefenderLosingBadly)
                return false;
            if (ctx.WarDurationYears < 1.25f)
                return false;
            if (ctx.DefenderCityLossRatio < 0.1f && ctx.DefenderArmyLossRatio < 0.25f && ctx.DefenderDesperation < 0.62f)
                return false;
            if (ctx.PuppetWillingness < 0.5f)
                return false;
            return true;
        }

        private static float BuildPuppetCapitulationBias(Kingdom defender, NationTypeDefinition defenderType, Actor defenderRuler)
        {
            float bias = 0f;
            if (defenderType != null)
            {
                switch (defenderType.SuccessionMode)
                {
                    case NationSuccessionMode.RoyalLine:
                        bias += 0.08f;
                        break;
                    case NationSuccessionMode.Council:
                    case NationSuccessionMode.Elective:
                        bias -= 0.05f;
                        break;
                    case NationSuccessionMode.Religious:
                        bias -= 0.08f;
                        break;
                    case NationSuccessionMode.None:
                        bias += 0.15f;
                        break;
                }
            }

            if (defenderRuler != null)
            {
                if (defenderRuler.hasTrait("evil"))
                    bias -= 0.18f;
                if (defenderRuler.hasTrait("ambitious"))
                    bias -= 0.14f;
                if (defenderRuler.hasTrait("bloodlust"))
                    bias -= 0.1f;
                if (defenderRuler.hasTrait("peaceful"))
                    bias += 0.12f;
                if (defenderRuler.hasTrait("content"))
                    bias += 0.08f;
                if (defenderRuler.hasTrait("madness"))
                    bias -= 0.06f;

                float aggression = defenderRuler.stats?.get("personality_aggression") ?? 0f;
                float diplomatic = defenderRuler.stats?.get("personality_diplomatic") ?? 0f;
                float rationality = defenderRuler.stats?.get("personality_rationality") ?? 0f;
                float diplomacy = defenderRuler.stats?.get("diplomacy") ?? 0f;
                float warfare = defenderRuler.stats?.get("warfare") ?? 0f;

                bias -= aggression * 0.3f;
                bias += diplomatic * 0.2f;
                bias += Mathf.Clamp(rationality * 0.12f, -0.08f, 0.12f);
                bias += Mathf.Clamp((diplomacy - warfare) * 0.02f, -0.12f, 0.12f);
            }

            float population = defender?.getPopulationTotal() ?? 0;
            if (population > 450)
                bias -= 0.08f;
            else if (population < 140)
                bias += 0.05f;

            return Mathf.Clamp(bias, -0.35f, 0.35f);
        }

        private static float GetAverageLoyalty(Kingdom kingdom)
        {
            if (kingdom == null)
                return 60f;

            int count = 0;
            float total = 0f;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                total += Mathf.Clamp(city.getLoyalty(), 0, 100);
                count++;
            }

            if (count == 0)
                return 60f;
            return total / count;
        }

        private static float GetPopulationSubmissionPressure(Kingdom kingdom)
        {
            if (kingdom == null)
                return 0f;

            float loyalty = GetAverageLoyalty(kingdom);
            float loyaltyPressure = Mathf.Clamp01((60f - loyalty) / 60f);
            float supportForCurrent = NationTypeManager.GetPopulationSupportForCurrentType(kingdom);
            float preferencePressure = Mathf.Clamp01((0.52f - supportForCurrent) / 0.52f);
            return Mathf.Clamp01(loyaltyPressure * 0.55f + preferencePressure * 0.45f);
        }

        private static string SelectReason(Kingdom attacker, Kingdom defender)
        {
            var reasons = new List<string>
            {
                "territorial_reclaim",
                "border_dispute",
                "imperial_expansion",
                "world_dominance",
                "independence",
                "civil_war",
                "succession",
                "religious",
                "cultural_supremacy",
                "purification",
                "resource",
                "trade_war",
                "naval_supremacy",
                "strategic_defense",
                "puppet_enforcement",
                "liberation",
                "forced_vassalization"
            };

            if (defender != null && TryGetLong(defender.data, PuppetOfKey, out long overlord) && overlord == attacker.id)
                return "puppet_enforcement";

            if (attacker != null && TryGetLong(attacker.data, PuppetOfKey, out long puppetOf) && puppetOf == defender?.id)
                return "independence";

            if (attacker != null && attacker.countCities() <= 2 && defender != null && defender.countCities() >= 4)
            {
                if (IsReasonAvailable("strategic_defense", attacker, defender))
                    return "strategic_defense";
            }

            if (attacker != null && defender != null && attacker.countCities() >= 5 && IsReasonAvailable("world_dominance", attacker, defender))
                return "world_dominance";

            var valid = new List<string>(reasons.Count);
            for (int i = 0; i < reasons.Count; i++)
            {
                string reason = reasons[i];
                if (IsReasonAvailable(reason, attacker, defender))
                    valid.Add(reason);
            }

            if (valid.Count == 0)
                return "imperial_expansion";
            return valid[Randy.randomInt(0, valid.Count)];
        }

        private static string EnsureReasonAvailable(Kingdom attacker, Kingdom defender, string reason)
        {
            if (IsReasonAvailable(reason, attacker, defender))
                return reason;
            return SelectReason(attacker, defender);
        }

        private static bool IsReasonAvailable(string reason, Kingdom attacker, Kingdom defender)
        {
            if (attacker == null || defender == null || !attacker.isAlive() || !defender.isAlive())
                return false;
            if (string.IsNullOrEmpty(reason))
                return false;

            switch (reason)
            {
                case "naval_supremacy":
                    return HasDockCity(attacker) && HasDockCity(defender);
                case "resource":
                case "trade_war":
                    return EstimateKingdomResources(defender) >= 120;
                case "cultural_supremacy":
                    return attacker.hasCulture() && defender.hasCulture() && attacker.getCulture() != defender.getCulture();
                case "religious":
                    return attacker.hasReligion() && defender.hasReligion() && attacker.getReligion() != defender.getReligion();
                case "border_dispute":
                    return AreKingdomsBordering(attacker, defender);
                case "territorial_reclaim":
                    return HasHistoricalClaim(attacker, defender) || AreKingdomsBordering(attacker, defender);
                case "succession":
                    return defender.countCities() >= 2;
                case "strategic_defense":
                    return attacker.countCities() <= defender.countCities();
                case "world_dominance":
                    return attacker.countCities() >= 4 || DiplomacyManager.kingdom_supreme == attacker;
                case "independence":
                    return TryGetLong(attacker.data, PuppetOfKey, out long puppetOf) && puppetOf == defender.id;
                case "puppet_enforcement":
                    return TryGetLong(defender.data, PuppetOfKey, out long overlord) && overlord == attacker.id;
                case "purification":
                    return !string.Equals(attacker.getSpecies(), defender.getSpecies(), StringComparison.Ordinal);
                default:
                    return true;
            }
        }

        private static string GetReason(War war)
        {
            if (war?.data?.custom_data_string != null && war.data.custom_data_string.dict.TryGetValue(ReasonKey, out var value))
                return value;
            return "unknown";
        }

        public static bool TryGetReasonDisplayName(War war, out string displayName)
        {
            displayName = null;
            if (!ShouldProcess(war))
                return false;
            string reason = GetReason(war);
            if (string.IsNullOrWhiteSpace(reason) || reason == "unknown")
                return false;
            displayName = HumanizeReason(reason);
            return !string.IsNullOrEmpty(displayName);
        }

        private static void SetWarGoal(War war, Kingdom attacker, Kingdom defender)
        {
            if (war?.data == null)
                return;
            string reason = EnsureReasonAvailable(attacker, defender, GetReason(war));
            war.data.custom_data_string.dict[ReasonKey] = reason;
            WarGoal goal = SelectGoal(attacker, defender, reason);
            war.data.custom_data_string.dict[GoalTypeKey] = goal.Type;
            if (goal.TargetCityId > 0)
            {
                war.data.custom_data_long ??= new CustomDataContainer<long>();
                war.data.custom_data_long.dict[GoalTargetCityKey] = goal.TargetCityId;
            }
            else if (war.data.custom_data_long != null)
            {
                war.data.custom_data_long.dict.Remove(GoalTargetCityKey);
            }
        }

        private static WarGoal SelectGoal(Kingdom attacker, Kingdom defender, string reason)
        {
            string type = "seize_city";
            switch (reason)
            {
                case "independence":
                    type = "break_puppet";
                    break;
                case "puppet_enforcement":
                case "forced_vassalization":
                    type = "vassalize";
                    break;
                case "trade_war":
                case "resource":
                    type = "resource";
                    break;
                case "religious":
                    type = "religious_conversion";
                    break;
                case "cultural_supremacy":
                    type = "cultural_supremacy";
                    break;
                case "border_dispute":
                    type = "border_dispute";
                    break;
                case "territorial_reclaim":
                case "liberation":
                    type = "reclaim_territory";
                    break;
                case "naval_supremacy":
                    type = "naval_supremacy";
                    break;
                case "succession":
                case "civil_war":
                    type = "succession_split";
                    break;
                case "strategic_defense":
                    type = "strategic_defense";
                    break;
                case "world_dominance":
                    type = "world_dominance";
                    break;
                case "purification":
                    type = "purification";
                    break;
            }
            City target = ChooseTargetCity(attacker, defender, reason);
            return new WarGoal { Type = type, TargetCityId = target?.id ?? -1 };
        }

        private static City ChooseTargetCity(Kingdom attacker, Kingdom defender, string reason)
        {
            if (defender == null)
                return null;

            if (reason == "naval_supremacy")
                return ChooseDockCity(defender);
            if (reason == "border_dispute")
                return ChooseBorderCity(defender, attacker);
            if (reason == "territorial_reclaim" || reason == "liberation")
            {
                City reclaimed = ChooseCityByHistoricalClaim(defender, attacker);
                if (reclaimed != null)
                    return reclaimed;
                return ChooseBorderCity(defender, attacker);
            }
            if (reason == "world_dominance")
                return defender.capital ?? ChooseBorderCity(defender, attacker) ?? ChooseDockCity(defender);
            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in defender.getCities())
            {
                int population = city.status?.population ?? city.countUnits();
                int warriors = city.countWarriors();
                int civilians = Mathf.Max(0, population - warriors);
                int score = civilians * 2 + city.countWeapons() - warriors;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = city;
                }
            }
            return best;
        }

        private static WarGoal GetGoal(War war)
        {
            string type = null;
            long city = -1;
            if (war?.data?.custom_data_string != null)
                war.data.custom_data_string.dict.TryGetValue(GoalTypeKey, out type);
            if (war?.data?.custom_data_long != null)
                war.data.custom_data_long.dict.TryGetValue(GoalTargetCityKey, out city);
            return new WarGoal { Type = type, TargetCityId = city };
        }

        private static bool ResolveGoalIfMet(War war, WarContext ctx)
        {
            if (ctx?.Goal == null || string.IsNullOrEmpty(ctx.Goal.Type))
                return false;

            City targetCity = ctx.Goal.TargetCityId > 0 ? World.world.cities.get(ctx.Goal.TargetCityId) : null;
            bool capturedTarget = targetCity != null && targetCity.kingdom == ctx.Attacker;

            if ((ctx.Goal.Type == "seize_city" || ctx.Goal.Type == "reclaim_territory" || ctx.Goal.Type == "border_dispute") && capturedTarget)
            {
                ApplyGoalPeace(war, ctx, ctx.Goal.Type);
                return true;
            }

            if (ctx.Goal.Type == "naval_supremacy" && capturedTarget && targetCity != null && targetCity.countBuildingsType("type_docks") > 0 && ctx.BothHaveDocks)
            {
                ApplyGoalPeace(war, ctx, "naval_supremacy");
                return true;
            }

            if (ctx.Goal.Type == "resource" && ctx.DefenderLosingBadly && ctx.WarDurationYears > 1f)
            {
                ApplyGoalPeace(war, ctx, "resource");
                return true;
            }

            if (ctx.Goal.Type == "cultural_supremacy" && ctx.DefenderLosingBadly && ctx.WarDurationYears > 1.2f)
            {
                ApplyGoalPeace(war, ctx, "cultural_supremacy");
                return true;
            }

            if (ctx.Goal.Type == "religious_conversion" && ctx.DefenderLosingBadly && ctx.WarDurationYears > 1.2f)
            {
                ApplyGoalPeace(war, ctx, "religious");
                return true;
            }

            if (ctx.Goal.Type == "strategic_defense" && ctx.DefenderLosingBadly && ctx.WarDurationYears > 1.1f)
            {
                ApplyGoalPeace(war, ctx, "strategic_defense");
                return true;
            }

            if (ctx.Goal.Type == "succession_split" && (ctx.DefenderLosingBadly || ctx.DefenderCityLossRatio > 0.12f) && ctx.WarDurationYears > 1.15f)
            {
                ApplyGoalPeace(war, ctx, "succession");
                return true;
            }

            if (ctx.Goal.Type == "world_dominance" && DiplomacyManager.kingdom_supreme == ctx.Attacker && (ctx.DefenderLosingBadly || ctx.AttackerCapturedEnemyCities > 0))
            {
                ApplyGoalPeace(war, ctx, "world_dominance");
                return true;
            }

            if (ctx.Goal.Type == "break_puppet")
            {
                if (TryGetOverlord(ctx.Attacker, out var overlord) && overlord == ctx.Defender)
                {
                    if (ctx.WarDurationYears > 1.2f && ctx.DefenderLosingBadly)
                    {
                        ApplyIndependence(ctx.Attacker, ctx.Defender);
                        SetSettlementMode(war, SettlementModeRestore);
                        MarkSettlementApplied(war, true);
                        World.world.wars.endWar(war, WarWinner.Attackers);
                        LogBetterWar(WorldLogAssets.GoalAchieved, ctx.Attacker, ctx.Defender, "independence");
                        return true;
                    }
                }
            }
            if (ctx.Goal.Type == "vassalize" && ctx.DefenderLosingBadly && ShouldAllowPuppetAgreement(ctx))
            {
                ApplyPuppetAgreement(war, ctx);
                return true;
            }
            return false;
        }

        private static void SetSettlementMode(War war, string mode)
        {
            if (war?.data == null)
                return;
            EnsureContainers(war.data);
            war.data.custom_data_string.dict[SettlementModeKey] = string.IsNullOrEmpty(mode) ? SettlementModeRestore : mode;
        }

        private static string GetSettlementMode(War war)
        {
            if (war?.data?.custom_data_string == null)
                return SettlementModeRestore;
            if (war.data.custom_data_string.dict.TryGetValue(SettlementModeKey, out string mode) && !string.IsNullOrEmpty(mode))
                return mode;
            return SettlementModeRestore;
        }

        private static void MarkSettlementApplied(War war, bool applied)
        {
            if (war?.data == null)
                return;
            EnsureContainers(war.data);
            war.data.custom_data_bool.dict[SettlementAppliedKey] = applied;
        }

        private static bool IsSettlementApplied(War war)
        {
            if (war?.data?.custom_data_bool == null)
                return false;
            return war.data.custom_data_bool.dict.TryGetValue(SettlementAppliedKey, out bool applied) && applied;
        }

        private static void SaveInitialCityOwners(War war)
        {
            if (war?.data == null || World.world?.cities == null)
                return;
            EnsureContainers(war.data);
            HashSet<long> participantIds = GetWarParticipantIds(war, false);
            var snapshot = new Dictionary<long, long>();
            foreach (City city in World.world.cities)
            {
                if (city == null || !city.isAlive())
                    continue;
                long ownerId = city.kingdom?.id ?? -1;
                if (ownerId <= 0 || !participantIds.Contains(ownerId))
                    continue;
                snapshot[city.id] = ownerId;
            }
            war.data.custom_data_string.dict[StartCityOwnersKey] = SerializeCityOwnerSnapshot(snapshot);
            war.data.custom_data_string.dict.Remove(SettlementOwnerOverridesKey);
        }

        private static void ParseCityOwnerSnapshot(string raw, Dictionary<long, long> output)
        {
            output.Clear();
            if (string.IsNullOrEmpty(raw))
                return;
            string[] entries = raw.Split(';');
            for (int i = 0; i < entries.Length; i++)
            {
                string entry = entries[i];
                if (string.IsNullOrEmpty(entry))
                    continue;
                string[] parts = entry.Split('=');
                if (parts.Length != 2)
                    continue;
                if (!long.TryParse(parts[0], out long cityId))
                    continue;
                if (!long.TryParse(parts[1], out long ownerId))
                    continue;
                output[cityId] = ownerId;
            }
        }

        private static string SerializeCityOwnerSnapshot(Dictionary<long, long> source)
        {
            if (source == null || source.Count == 0)
                return string.Empty;
            var entries = new string[source.Count];
            int index = 0;
            foreach (var pair in source)
                entries[index++] = pair.Key.ToString() + "=" + pair.Value.ToString();
            return string.Join(";", entries);
        }

        private static HashSet<long> GetWarParticipantIds(War war, bool includeHistoric)
        {
            var ids = new HashSet<long>();
            if (war == null)
                return ids;
            IEnumerable<Kingdom> attackers = includeHistoric ? war.getAllAttackers() : war.getAttackers();
            foreach (Kingdom attacker in attackers)
            {
                if (attacker == null)
                    continue;
                ids.Add(attacker.id);
            }
            IEnumerable<Kingdom> defenders = includeHistoric ? war.getAllDefenders() : war.getDefenders();
            foreach (Kingdom defender in defenders)
            {
                if (defender == null)
                    continue;
                ids.Add(defender.id);
            }
            return ids;
        }

        private static void CountCapturedCitiesFromSnapshot(War war, Kingdom attacker, Kingdom defender, out int attackerCaptured, out int defenderCaptured)
        {
            attackerCaptured = 0;
            defenderCaptured = 0;
            if (war?.data?.custom_data_string == null)
                return;
            if (!war.data.custom_data_string.dict.TryGetValue(StartCityOwnersKey, out string raw))
                return;
            var snapshot = new Dictionary<long, long>();
            ParseCityOwnerSnapshot(raw, snapshot);
            foreach (var pair in snapshot)
            {
                City city = World.world.cities.get(pair.Key);
                if (city == null || !city.isAlive() || city.kingdom == null || !city.kingdom.isAlive())
                    continue;
                long startOwner = pair.Value;
                long currentOwner = city.kingdom.id;
                if (startOwner == currentOwner)
                    continue;
                if (attacker != null && defender != null)
                {
                    if (startOwner == defender.id && currentOwner == attacker.id)
                        attackerCaptured++;
                    else if (startOwner == attacker.id && currentOwner == defender.id)
                        defenderCaptured++;
                }
            }
        }

        private static bool HasAnyCapturedCities(WarContext ctx)
        {
            return ctx != null && (ctx.AttackerCapturedEnemyCities > 0 || ctx.DefenderCapturedEnemyCities > 0);
        }

        private static bool ShouldUseStatusQuo(WarContext ctx)
        {
            return ctx != null && ctx.TerritoryWar && HasAnyCapturedCities(ctx);
        }

        private static void FinalizeWarSettlement(War war, WarWinner winner)
        {
            if (war?.data == null)
                return;
            EnsureContainers(war.data);

            string reason = GetReason(war);
            string mode = GetSettlementMode(war);
            bool settlementApplied = IsSettlementApplied(war);
            if (!settlementApplied)
            {
                WarGoal goal = GetGoal(war);
                mode = IsTerritoryGoal(goal?.Type, reason) || IsTerritoryReason(reason) ? SettlementModeKeep : SettlementModeRestore;
                SetSettlementMode(war, mode);
                MarkSettlementApplied(war, true);
            }

            if (string.Equals(mode, SettlementModeRestore, StringComparison.Ordinal))
                RestoreCapturedCities(war);

            RecordSettledOwnerHistory(war);
        }

        private static void RestoreCapturedCities(War war)
        {
            if (war?.data?.custom_data_string == null)
                return;
            if (!war.data.custom_data_string.dict.TryGetValue(StartCityOwnersKey, out string raw))
                return;

            var snapshot = new Dictionary<long, long>();
            ParseCityOwnerSnapshot(raw, snapshot);
            HashSet<long> participantIds = GetWarParticipantIds(war, true);
            var overrides = new Dictionary<long, long>();
            if (war.data.custom_data_string.dict.TryGetValue(SettlementOwnerOverridesKey, out string overrideRaw))
                ParseCityOwnerSnapshot(overrideRaw, overrides);
            foreach (var pair in snapshot)
            {
                City city = World.world.cities.get(pair.Key);
                if (city == null || !city.isAlive())
                    continue;
                Kingdom currentOwner = city.kingdom;
                long targetOwnerId = pair.Value;
                if (overrides.TryGetValue(pair.Key, out long overrideOwnerId))
                    targetOwnerId = overrideOwnerId;
                if (!participantIds.Contains(targetOwnerId))
                    continue;
                if (currentOwner != null && !participantIds.Contains(currentOwner.id))
                    continue;
                Kingdom targetOwner = World.world.kingdoms.get(targetOwnerId);
                if (targetOwner == null || !targetOwner.isAlive())
                    continue;
                if (currentOwner == targetOwner)
                    continue;
                city.joinAnotherKingdom(targetOwner, true);
            }
        }

        private static void RecordSettledOwnerHistory(War war)
        {
            if (war?.data?.custom_data_string == null)
                return;
            if (!war.data.custom_data_string.dict.TryGetValue(StartCityOwnersKey, out string raw))
                return;

            var snapshot = new Dictionary<long, long>();
            ParseCityOwnerSnapshot(raw, snapshot);
            foreach (var pair in snapshot)
            {
                City city = World.world.cities.get(pair.Key);
                if (city == null || !city.isAlive() || city.kingdom == null || !city.kingdom.isAlive())
                    continue;
                AppendOwnerHistory(city, city.kingdom.id);
            }
        }

        private static void RegisterSettledCityOwner(War war, City city, Kingdom owner)
        {
            if (war?.data == null || city == null || owner == null || !owner.isAlive())
                return;
            EnsureContainers(war.data);
            if (city.kingdom != owner)
                city.joinAnotherKingdom(owner, true);
            var overrides = new Dictionary<long, long>();
            if (war.data.custom_data_string.dict.TryGetValue(SettlementOwnerOverridesKey, out string raw))
                ParseCityOwnerSnapshot(raw, overrides);
            overrides[city.id] = owner.id;
            war.data.custom_data_string.dict[SettlementOwnerOverridesKey] = SerializeCityOwnerSnapshot(overrides);
        }

        private static void AppendOwnerHistory(City city, long ownerId)
        {
            if (city?.data == null || ownerId <= 0)
                return;

            var history = new List<long>();
            GetOwnerHistory(city, history);
            if (history.Count == 0 || history[history.Count - 1] != ownerId)
                history.Add(ownerId);
            SetOwnerHistory(city, history);
        }

        private static void GetOwnerHistory(City city, List<long> output)
        {
            output.Clear();
            if (city?.data?.custom_data_string == null)
                return;
            if (!city.data.custom_data_string.dict.TryGetValue(CityOwnerHistoryKey, out string raw))
                return;
            if (string.IsNullOrEmpty(raw))
                return;
            string[] ids = raw.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (!long.TryParse(ids[i], out long id))
                    continue;
                output.Add(id);
            }
        }

        private static void SetOwnerHistory(City city, List<long> history)
        {
            if (city?.data == null)
                return;
            city.data.custom_data_string ??= new CustomDataContainer<string>();
            if (history == null || history.Count == 0)
            {
                city.data.custom_data_string.dict.Remove(CityOwnerHistoryKey);
                return;
            }
            var ids = new string[history.Count];
            for (int i = 0; i < history.Count; i++)
                ids[i] = history[i].ToString();
            city.data.custom_data_string.dict[CityOwnerHistoryKey] = string.Join(",", ids);
        }

        public static void OnCitySave(City city)
        {
            if (city?.data == null)
                return;
            var history = new List<long>();
            GetOwnerHistory(city, history);
            if (history.Count == 0)
                return;
            var filtered = new List<long>(history.Count);
            for (int i = 0; i < history.Count; i++)
            {
                long id = history[i];
                Kingdom kingdom = World.world.kingdoms.get(id);
                if (kingdom == null || !kingdom.isAlive())
                    continue;
                if (filtered.Count > 0 && filtered[filtered.Count - 1] == id)
                    continue;
                filtered.Add(id);
            }
            SetOwnerHistory(city, filtered);
        }

        public static bool BeforeCityCapture(City city, Kingdom oldOwner, Kingdom newOwner)
        {
            if (city == null || oldOwner == null || newOwner == null || oldOwner == newOwner)
                return true;
            if (!oldOwner.isAlive() || !newOwner.isAlive())
                return true;
            if (oldOwner.countCities() > 1)
                return true;

            War nonTerritoryWar = FindNonTerritoryWarForCapture(oldOwner, newOwner);
            if (nonTerritoryWar == null)
                return true;

            SetSettlementMode(nonTerritoryWar, SettlementModeRestore);
            MarkSettlementApplied(nonTerritoryWar, true);
            World.world.wars.endWar(nonTerritoryWar, GetCaptureWinner(nonTerritoryWar, newOwner, oldOwner));
            return false;
        }

        public static void OnCityCaptured(City city, Kingdom oldOwner, Kingdom newOwner)
        {
            if (city == null || oldOwner == null || newOwner == null || oldOwner == newOwner)
                return;
            if (!oldOwner.isAlive() || !newOwner.isAlive())
                return;

            War war = FindActiveWarBetween(oldOwner, newOwner);
            if (war == null)
                return;

            string reason = GetReason(war);
            if (!IsTerritoryReason(reason) && !IsTerritoryGoal(GetGoal(war)?.Type, reason))
            {
                SetSettlementMode(war, SettlementModeRestore);
                if (oldOwner.countCities() == 0)
                {
                    MarkSettlementApplied(war, true);
                    World.world.wars.endWar(war, GetCaptureWinner(war, newOwner, oldOwner));
                }
            }
        }

        private static War FindActiveWarBetween(Kingdom first, Kingdom second)
        {
            if (first == null || second == null || !first.isAlive() || !second.isAlive())
                return null;
            foreach (War war in first.getWars())
            {
                if (war == null || war.hasEnded() || !ShouldProcess(war))
                    continue;
                if (war.isInWarWith(first, second))
                    return war;
            }
            return null;
        }

        private static War FindNonTerritoryWarForCapture(Kingdom oldOwner, Kingdom newOwner)
        {
            if (oldOwner == null || newOwner == null)
                return null;
            foreach (War war in oldOwner.getWars())
            {
                if (war == null || war.hasEnded() || !ShouldProcess(war))
                    continue;
                if (!war.isInWarWith(oldOwner, newOwner))
                    continue;
                string reason = GetReason(war);
                if (IsTerritoryReason(reason) || IsTerritoryGoal(GetGoal(war)?.Type, reason))
                    continue;
                return war;
            }
            return null;
        }

        private static WarWinner GetCaptureWinner(War war, Kingdom newOwner, Kingdom oldOwner)
        {
            if (war == null || newOwner == null || oldOwner == null)
                return WarWinner.Peace;
            if (war.isAttacker(newOwner) && war.isDefender(oldOwner))
                return WarWinner.Attackers;
            if (war.isDefender(newOwner) && war.isAttacker(oldOwner))
                return WarWinner.Defenders;
            return WarWinner.Peace;
        }

        private static City TransferBorderCity(Kingdom from, Kingdom to)
        {
            if (from == null || to == null)
                return null;

            City chosen = ChooseBorderCity(from, to);

            if (chosen == null)
            {
                int bestScore = int.MinValue;
                foreach (City city in from.getCities())
                {
                    int population = city.status?.population ?? city.countUnits();
                    int warriors = city.countWarriors();
                    int civilians = Mathf.Max(0, population - warriors);
                    int score = city.countWeapons() + civilians - warriors;
                    if (city.getTile() != null && city.kingdom != null && city.kingdom.isEnemy(to))
                        score += 3;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        chosen = city;
                    }
                }
            }

            if (chosen != null)
                chosen.joinAnotherKingdom(to, true);
            return chosen;
        }

        private static bool ReasonBlocksAttackerConcessions(string reason)
        {
            return string.Equals(reason, "purification", StringComparison.Ordinal);
        }

        private static bool IsTerritoryReason(string reason)
        {
            switch (reason)
            {
                case "territorial_reclaim":
                case "border_dispute":
                case "imperial_expansion":
                case "liberation":
                case "world_dominance":
                case "naval_supremacy":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsTerritoryGoal(string goalType, string reason)
        {
            switch (goalType)
            {
                case "seize_city":
                case "reclaim_territory":
                case "border_dispute":
                case "naval_supremacy":
                case "world_dominance":
                    return true;
            }
            return IsTerritoryReason(reason);
        }

        private static bool ApplyConcessionFromLoser(War war, WarContext ctx, Kingdom loser, Kingdom winner, bool winnerIsAttacker, float baseIntensity, out bool territoryConcession)
        {
            territoryConcession = false;
            if (loser == null || winner == null || !loser.isAlive() || !winner.isAlive())
                return false;
            if (winnerIsAttacker && ReasonBlocksAttackerConcessions(ctx?.Reason))
                return false;

            float intensity = Mathf.Clamp01(baseIntensity);
            string reason = ctx?.Reason ?? string.Empty;
            bool applied = false;

            switch (reason)
            {
                case "resource":
                case "trade_war":
                    applied = TryResourceConcession(loser, winner, Mathf.Clamp01(0.2f + intensity * 0.35f));
                    break;
                case "cultural_supremacy":
                    applied = TryCulturalConcession(loser, winner, intensity);
                    break;
                case "religious":
                    applied = TryReligiousConcession(loser, winner, intensity);
                    break;
                case "border_dispute":
                    applied = TryBorderDisputeConcession(war, ctx, loser, winner, intensity, out territoryConcession);
                    break;
                case "territorial_reclaim":
                case "liberation":
                case "imperial_expansion":
                    applied = TryTerritoryConcession(war, loser, winner, intensity, out territoryConcession);
                    break;
                case "naval_supremacy":
                    applied = TryNavalSupremacyConcession(war, loser, winner, intensity, out territoryConcession);
                    break;
                case "succession":
                case "civil_war":
                    applied = TrySuccessionSplitConcession(loser, winner, intensity);
                    break;
                case "strategic_defense":
                    applied = TryStrategicDefenseConcession(loser, intensity);
                    break;
                case "world_dominance":
                    applied = TryTerritoryConcession(war, loser, winner, intensity, out territoryConcession);
                    if (!applied)
                        applied = TryResourceConcession(loser, winner, Mathf.Clamp01(0.14f + intensity * 0.24f));
                    break;
                case "purification":
                    if (!winnerIsAttacker)
                        applied = TryStrategicDefenseConcession(loser, intensity + 0.25f);
                    break;
            }

            if (!applied && ctx?.Goal != null)
            {
                switch (ctx.Goal.Type)
                {
                    case "resource":
                        applied = TryResourceConcession(loser, winner, Mathf.Clamp01(0.18f + intensity * 0.3f));
                        break;
                    case "cultural_supremacy":
                        applied = TryCulturalConcession(loser, winner, intensity);
                        break;
                    case "religious_conversion":
                        applied = TryReligiousConcession(loser, winner, intensity);
                        break;
                    case "border_dispute":
                        applied = TryBorderDisputeConcession(war, ctx, loser, winner, intensity, out territoryConcession);
                        break;
                    case "reclaim_territory":
                    case "world_dominance":
                    case "naval_supremacy":
                        applied = TryTerritoryConcession(war, loser, winner, intensity, out territoryConcession);
                        break;
                    case "strategic_defense":
                        applied = TryStrategicDefenseConcession(loser, intensity);
                        break;
                    case "succession_split":
                        applied = TrySuccessionSplitConcession(loser, winner, intensity);
                        break;
                }
            }

            if (!applied && (IsTerritoryReason(reason) || HasHistoricalClaim(winner, loser)))
                applied = TryTerritoryConcession(war, loser, winner, intensity * 0.85f, out territoryConcession);

            if (!applied)
                applied = TryResourceConcession(loser, winner, Mathf.Clamp01(0.08f + intensity * 0.2f));

            if (!applied && string.Equals(reason, "purification", StringComparison.Ordinal) && !winnerIsAttacker)
                applied = TryStrategicDefenseConcession(loser, intensity + 0.15f);

            return applied;
        }

        private static bool TryTerritoryConcession(War war, Kingdom loser, Kingdom winner, float intensity, out bool territoryConcession)
        {
            territoryConcession = false;
            if (loser == null || winner == null || !loser.isAlive() || !winner.isAlive() || loser.countCities() <= 0)
                return false;

            List<City> capturedCities = GetCapturedCitiesFromLoser(war, winner, loser);
            HashSet<long> capturedCityIds = BuildCityIdSet(capturedCities);
            if (capturedCities.Count > 0)
            {
                capturedCities.Sort((a, b) => ScoreCapturedRetentionCity(b, winner, capturedCityIds).CompareTo(ScoreCapturedRetentionCity(a, winner, capturedCityIds)));
                int keepCount = Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp(1f, 2.4f, Mathf.Clamp01(intensity))), 1, capturedCities.Count);
                for (int i = 0; i < keepCount; i++)
                    RegisterSettledCityOwner(war, capturedCities[i], winner);
                territoryConcession = true;
                if (intensity < 0.58f)
                    return true;
            }

            City sourceCity = ChooseCityByHistoricalClaim(loser, winner) ?? ChooseBorderCity(loser, winner, capturedCityIds, false) ?? ChooseBorderCity(loser, winner);
            if (sourceCity == null)
            {
                foreach (City city in loser.getCities())
                {
                    if (city == null || !city.isAlive())
                        continue;
                    sourceCity = city;
                    if (city != loser.capital)
                        break;
                }
            }

            if (sourceCity == null)
                return false;

            bool fullCity = intensity >= 0.62f || loser.countCities() <= 2 || Randy.randomChance(Mathf.Clamp01(intensity - 0.12f));
            if (fullCity)
            {
                sourceCity.joinAnotherKingdom(winner, true);
                territoryConcession = true;
                RegisterSettledCityOwner(war, sourceCity, winner);
                return true;
            }

            City receiverCity = ChooseBorderCity(winner, loser, capturedCityIds, true) ?? ChooseBorderCity(winner, loser) ?? winner.capital;
            if (receiverCity == null)
            {
                sourceCity.joinAnotherKingdom(winner, true);
                territoryConcession = true;
                RegisterSettledCityOwner(war, sourceCity, winner);
                return true;
            }

            int movedZones = TransferBorderZones(sourceCity, receiverCity, intensity);
            if (movedZones > 0)
            {
                territoryConcession = true;
                return true;
            }

            if (Randy.randomChance(Mathf.Clamp01(intensity + 0.15f)))
            {
                sourceCity.joinAnotherKingdom(winner, true);
                territoryConcession = true;
                RegisterSettledCityOwner(war, sourceCity, winner);
                return true;
            }

            return false;
        }

        private static bool TryBorderDisputeConcession(War war, WarContext ctx, Kingdom loser, Kingdom winner, float intensity, out bool territoryConcession)
        {
            territoryConcession = false;
            if (loser == null || winner == null)
                return false;

            bool heavyLoss = (ctx != null && loser == ctx.Defender && ctx.DefenderLosingBadly) || (ctx != null && loser == ctx.Attacker && ctx.AttackerLosingBadly);
            if (heavyLoss || intensity >= 0.68f)
                return TryTerritoryConcession(war, loser, winner, intensity, out territoryConcession);

            List<City> capturedCities = GetCapturedCitiesFromLoser(war, winner, loser);
            HashSet<long> capturedCityIds = BuildCityIdSet(capturedCities);

            City loserBorder = ChooseBorderCity(loser, winner, capturedCityIds, false) ?? ChooseBorderCity(loser, winner);
            City winnerBorder = ChooseBorderCity(winner, loser, capturedCityIds, true) ?? ChooseBorderCity(winner, loser) ?? winner.capital;
            if (winnerBorder == null && capturedCities.Count > 0)
                winnerBorder = capturedCities[0];
            if (loserBorder == null || winnerBorder == null)
                return false;

            int movedZones = TransferBorderZones(loserBorder, winnerBorder, Mathf.Clamp01(intensity * 0.9f));
            if (movedZones > 0)
            {
                territoryConcession = true;
                return true;
            }

            return false;
        }

        private static int TransferBorderZones(City fromCity, City toCity, float intensity)
        {
            if (fromCity == null || toCity == null || fromCity == toCity || toCity.kingdom == null)
                return 0;
            if (CityAddZoneMethod == null || CityRemoveZoneMethod == null)
                return 0;

            var candidates = new List<TileZone>();
            object borderRaw = CityBorderZonesField?.GetValue(fromCity);
            if (borderRaw is HashSet<TileZone> borderSet)
            {
                foreach (TileZone zone in borderSet)
                {
                    if (zone != null && zone.city == fromCity && ZoneTouchesKingdom(zone, toCity.kingdom))
                        candidates.Add(zone);
                }
            }

            if (candidates.Count == 0)
            {
                object zonesRaw = CityZonesField?.GetValue(fromCity);
                if (zonesRaw is List<TileZone> zonesList)
                {
                    for (int i = 0; i < zonesList.Count; i++)
                    {
                        TileZone zone = zonesList[i];
                        if (zone != null && zone.city == fromCity && ZoneTouchesKingdom(zone, toCity.kingdom))
                            candidates.Add(zone);
                    }
                }
            }

            if (candidates.Count == 0)
                return 0;

            int moveCount = Mathf.Clamp(Mathf.RoundToInt(candidates.Count * Mathf.Lerp(0.25f, 0.8f, Mathf.Clamp01(intensity))), 1, candidates.Count);
            int moved = 0;
            for (int i = 0; i < moveCount; i++)
            {
                TileZone zone = candidates[i];
                if (zone == null || zone.city != fromCity)
                    continue;
                try
                {
                    CityRemoveZoneMethod.Invoke(fromCity, new object[] { zone });
                    CityAddZoneMethod.Invoke(toCity, new object[] { zone });
                    moved++;
                }
                catch
                {
                }
            }

            if (moved > 0)
            {
                fromCity.recalculateNeighbourZones();
                fromCity.recalculateNeighbourCities();
                toCity.recalculateNeighbourZones();
                toCity.recalculateNeighbourCities();
            }

            return moved;
        }

        private static bool ZoneTouchesKingdom(TileZone zone, Kingdom kingdom)
        {
            if (zone == null || kingdom == null || zone.neighbours_all == null)
                return false;
            for (int i = 0; i < zone.neighbours_all.Length; i++)
            {
                TileZone neighbour = zone.neighbours_all[i];
                if (neighbour?.city?.kingdom == kingdom)
                    return true;
            }
            return false;
        }

        private static bool TryNavalSupremacyConcession(War war, Kingdom loser, Kingdom winner, float intensity, out bool territoryConcession)
        {
            territoryConcession = false;
            if (!HasDockCity(loser) || !HasDockCity(winner))
                return false;
            City dockCity = ChooseDockCity(loser);
            if (dockCity == null)
                return false;
            if (intensity < 0.45f && !Randy.randomChance(0.6f))
                return false;
            dockCity.joinAnotherKingdom(winner, true);
            territoryConcession = true;
            RegisterSettledCityOwner(war, dockCity, winner);
            return true;
        }

        private static bool TrySuccessionSplitConcession(Kingdom loser, Kingdom winner, float intensity)
        {
            if (loser == null || !loser.isAlive() || loser.countCities() < 2)
                return false;

            City seed = ChooseBorderCity(loser, winner);
            if (seed == null || seed == loser.capital)
            {
                foreach (City city in loser.getCities())
                {
                    if (city == null || !city.isAlive() || city == loser.capital)
                        continue;
                    seed = city;
                    break;
                }
            }
            if (seed == null)
                return false;

            Actor founder = seed.hasLeader() ? seed.leader : seed.getRandomUnit();
            if (founder == null || !founder.isAlive())
                founder = loser.hasKing() ? loser.king : null;
            if (founder == null)
                return false;

            Kingdom split = null;
            if (CityMakeOwnKingdomMethod != null)
            {
                try
                {
                    split = CityMakeOwnKingdomMethod.Invoke(seed, new object[] { founder, true, false }) as Kingdom;
                }
                catch
                {
                    split = null;
                }
            }
            if (split == null)
            {
                try
                {
                    split = seed.makeOwnKingdom(founder, true, false);
                }
                catch
                {
                    split = null;
                }
            }
            if (split == null || !split.isAlive())
                return false;

            int extraTarget = Mathf.Clamp(Mathf.RoundToInt((loser.countCities() - 1) * Mathf.Lerp(0.1f, 0.4f, Mathf.Clamp01(intensity))), 0, Mathf.Max(0, loser.countCities() - 1));
            while (extraTarget > 0 && loser.countCities() > 1)
            {
                City extra = ChooseBorderCity(loser, split) ?? ChooseBorderCity(loser, winner);
                if (extra == null || extra == loser.capital || extra.kingdom != loser)
                    break;
                extra.joinAnotherKingdom(split, true);
                extraTarget--;
            }
            return true;
        }

        private static bool TryStrategicDefenseConcession(Kingdom loser, float intensity)
        {
            if (loser == null || !loser.isAlive())
                return false;
            double until = World.world.getCurWorldTime() + Mathf.Lerp(55f, 110f, Mathf.Clamp01(intensity));
            ApplyDemilitarization(loser, until);
            return true;
        }

        private static bool TryCulturalConcession(Kingdom loser, Kingdom winner, float intensity)
        {
            if (loser == null || winner == null || !winner.hasCulture())
                return false;

            Culture targetCulture = winner.getCulture();
            bool changed = false;
            if (loser.getCulture() != targetCulture)
            {
                loser.setCulture(targetCulture);
                changed = true;
            }

            var cities = GetCitiesByPressure(loser, winner);
            if (cities.Count == 0)
                return changed;
            int cityCount = Mathf.Clamp(Mathf.RoundToInt(cities.Count * Mathf.Lerp(0.25f, 0.65f, Mathf.Clamp01(intensity))), 1, cities.Count);
            for (int i = 0; i < cityCount; i++)
            {
                City city = cities[i];
                if (SetCityCulture(city, targetCulture))
                    changed = true;
            }
            return changed;
        }

        private static bool TryReligiousConcession(Kingdom loser, Kingdom winner, float intensity)
        {
            if (loser == null || winner == null || !winner.hasReligion())
                return false;

            Religion targetReligion = winner.getReligion();
            bool changed = false;
            if (loser.getReligion() != targetReligion)
            {
                loser.setReligion(targetReligion);
                changed = true;
            }

            var cities = GetCitiesByPressure(loser, winner);
            if (cities.Count == 0)
                return changed;
            int cityCount = Mathf.Clamp(Mathf.RoundToInt(cities.Count * Mathf.Lerp(0.25f, 0.65f, Mathf.Clamp01(intensity))), 1, cities.Count);
            for (int i = 0; i < cityCount; i++)
            {
                City city = cities[i];
                if (SetCityReligion(city, targetReligion))
                    changed = true;
            }
            return changed;
        }

        private static List<City> GetCitiesByPressure(Kingdom kingdom, Kingdom against)
        {
            var cities = new List<City>();
            if (kingdom == null)
                return cities;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                cities.Add(city);
            }
            cities.Sort((a, b) => ScoreConcessionCity(b, against).CompareTo(ScoreConcessionCity(a, against)));
            return cities;
        }

        private static int ScoreConcessionCity(City city, Kingdom against)
        {
            if (city == null)
                return int.MinValue;
            int population = city.status?.population ?? city.countUnits();
            int score = population + city.countWeapons() * 2 + city.countBuildingsType("type_docks") * 8;
            if (against != null && CityTouchesKingdom(city, against))
                score += 22;
            if (city.isCapitalCity())
                score += 10;
            return score;
        }

        private static bool SetCityCulture(City city, Culture culture)
        {
            if (city == null || culture == null)
                return false;
            if (city.getCulture() == culture)
                return false;
            if (CitySetCultureMethod != null)
            {
                try
                {
                    CitySetCultureMethod.Invoke(city, new object[] { culture });
                }
                catch
                {
                    city.culture = culture;
                }
            }
            else
                city.culture = culture;
            return true;
        }

        private static bool SetCityReligion(City city, Religion religion)
        {
            if (city == null || religion == null)
                return false;
            if (city.getReligion() == religion)
                return false;
            if (CitySetReligionMethod != null)
            {
                try
                {
                    CitySetReligionMethod.Invoke(city, new object[] { religion });
                }
                catch
                {
                    city.religion = religion;
                }
            }
            else
                city.religion = religion;
            return true;
        }

        private static bool TryResourceConcession(Kingdom loser, Kingdom winner, float share)
        {
            if (loser == null || winner == null || !loser.isAlive() || !winner.isAlive())
                return false;
            int totalMoved = 0;
            for (int i = 0; i < StrategicResourceIds.Length; i++)
            {
                string resource = StrategicResourceIds[i];
                int available = CountResourceInKingdom(loser, resource);
                int toMove = Mathf.Max(0, Mathf.RoundToInt(available * Mathf.Clamp01(share)));
                if (toMove <= 0)
                    continue;
                totalMoved += MoveResourceBetweenKingdoms(loser, winner, resource, toMove);
            }
            return totalMoved > 0;
        }

        private static int EstimateKingdomResources(Kingdom kingdom)
        {
            if (kingdom == null || !kingdom.isAlive())
                return 0;
            int total = 0;
            for (int i = 0; i < StrategicResourceIds.Length; i++)
                total += CountResourceInKingdom(kingdom, StrategicResourceIds[i]);
            return total;
        }

        private static int CountResourceInKingdom(Kingdom kingdom, string resource)
        {
            if (kingdom == null || string.IsNullOrEmpty(resource))
                return 0;
            int total = 0;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                total += city.getResourcesAmount(resource);
            }
            return total;
        }

        private static int MoveResourceBetweenKingdoms(Kingdom from, Kingdom to, string resourceId, int amount)
        {
            if (from == null || to == null || amount <= 0 || string.IsNullOrEmpty(resourceId))
                return 0;

            int remaining = amount;
            int moved = 0;
            while (remaining > 0)
            {
                City donor = null;
                int donorAmount = 0;
                foreach (City city in from.getCities())
                {
                    if (city == null || !city.isAlive())
                        continue;
                    int local = city.getResourcesAmount(resourceId);
                    if (local <= donorAmount)
                        continue;
                    donor = city;
                    donorAmount = local;
                }
                if (donor == null || donorAmount <= 0)
                    break;

                int transfer = Mathf.Min(remaining, Mathf.Max(1, donorAmount / 2));
                donor.takeResource(resourceId, transfer);
                int deposited = DepositResourceToKingdom(to, resourceId, transfer);
                if (deposited < transfer)
                    donor.addResourcesToRandomStockpile(resourceId, transfer - deposited);
                if (deposited <= 0)
                    break;
                remaining -= deposited;
                moved += deposited;
            }

            return moved;
        }

        private static int DepositResourceToKingdom(Kingdom kingdom, string resourceId, int amount)
        {
            if (kingdom == null || amount <= 0)
                return 0;

            int remaining = amount;
            int added = 0;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                int chunk = Mathf.Max(1, remaining);
                int pushed = city.addResourcesToRandomStockpile(resourceId, chunk);
                if (pushed <= 0)
                    continue;
                added += pushed;
                remaining -= pushed;
                if (remaining <= 0)
                    break;
            }

            return added;
        }

        private static bool HasDockCity(Kingdom kingdom)
        {
            return ChooseDockCity(kingdom) != null;
        }

        private static City ChooseDockCity(Kingdom kingdom)
        {
            if (kingdom == null || !kingdom.isAlive())
                return null;
            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                int docks = city.countBuildingsType("type_docks");
                if (docks <= 0)
                    continue;
                int population = city.status?.population ?? city.countUnits();
                int score = docks * 40 + population + city.countWeapons();
                if (score > bestScore)
                {
                    bestScore = score;
                    best = city;
                }
            }
            return best;
        }

        private static bool AreKingdomsBordering(Kingdom first, Kingdom second)
        {
            if (first == null || second == null || !first.isAlive() || !second.isAlive() || first == second)
                return false;
            foreach (City city in first.getCities())
            {
                if (CityTouchesKingdom(city, second))
                    return true;
            }
            return false;
        }

        private static bool CityTouchesKingdom(City city, Kingdom kingdom)
        {
            if (city == null || kingdom == null)
                return false;
            object borderRaw = CityBorderZonesField?.GetValue(city);
            if (borderRaw is HashSet<TileZone> borderSet)
            {
                foreach (TileZone border in borderSet)
                {
                    if (ZoneTouchesKingdom(border, kingdom))
                        return true;
                }
            }
            else if (borderRaw is List<TileZone> borderList)
            {
                for (int i = 0; i < borderList.Count; i++)
                {
                    if (ZoneTouchesKingdom(borderList[i], kingdom))
                        return true;
                }
            }
            return false;
        }

        private static List<City> GetCapturedCitiesFromLoser(War war, Kingdom winner, Kingdom loser)
        {
            var result = new List<City>();
            if (war?.data?.custom_data_string == null || winner == null || loser == null)
                return result;
            if (!war.data.custom_data_string.dict.TryGetValue(StartCityOwnersKey, out string raw))
                return result;

            var snapshot = new Dictionary<long, long>();
            ParseCityOwnerSnapshot(raw, snapshot);
            foreach (var pair in snapshot)
            {
                if (pair.Value != loser.id)
                    continue;
                City city = World.world.cities.get(pair.Key);
                if (city == null || !city.isAlive() || city.kingdom != winner)
                    continue;
                result.Add(city);
            }
            return result;
        }

        private static HashSet<long> BuildCityIdSet(List<City> cities)
        {
            var ids = new HashSet<long>();
            if (cities == null)
                return ids;
            for (int i = 0; i < cities.Count; i++)
            {
                City city = cities[i];
                if (city == null || !city.isAlive())
                    continue;
                ids.Add(city.id);
            }
            return ids;
        }

        private static int ScoreCapturedRetentionCity(City city, Kingdom claimant, HashSet<long> capturedCityIds)
        {
            if (city == null)
                return int.MinValue;
            int population = city.status?.population ?? city.countUnits();
            int score = population + city.countWeapons() * 2 + city.countBuildingsType("type_docks") * 10;
            if (claimant != null && CityHasHistory(city, claimant.id))
                score += 420;
            if (capturedCityIds != null && capturedCityIds.Count > 0 && CityTouchesAnyCapturedCity(city, capturedCityIds))
                score += 170;
            return score;
        }

        private static bool CityTouchesAnyCapturedCity(City city, HashSet<long> capturedCityIds)
        {
            if (city == null || capturedCityIds == null || capturedCityIds.Count == 0)
                return false;
            object borderRaw = CityBorderZonesField?.GetValue(city);
            if (borderRaw is HashSet<TileZone> borderSet)
            {
                foreach (TileZone zone in borderSet)
                {
                    if (ZoneTouchesAnyCapturedCity(zone, capturedCityIds))
                        return true;
                }
            }
            else if (borderRaw is List<TileZone> borderList)
            {
                for (int i = 0; i < borderList.Count; i++)
                {
                    if (ZoneTouchesAnyCapturedCity(borderList[i], capturedCityIds))
                        return true;
                }
            }
            return false;
        }

        private static bool ZoneTouchesAnyCapturedCity(TileZone zone, HashSet<long> capturedCityIds)
        {
            if (zone == null || zone.neighbours_all == null || capturedCityIds == null || capturedCityIds.Count == 0)
                return false;
            for (int i = 0; i < zone.neighbours_all.Length; i++)
            {
                TileZone neighbour = zone.neighbours_all[i];
                City neighbourCity = neighbour?.city;
                if (neighbourCity != null && capturedCityIds.Contains(neighbourCity.id))
                    return true;
            }
            return false;
        }

        private static City ChooseBorderCity(Kingdom kingdom, Kingdom against)
        {
            if (kingdom == null || !kingdom.isAlive())
                return null;
            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                int borderContacts = CountBorderContacts(city, against);
                if (borderContacts <= 0 && against != null)
                    continue;
                int population = city.status?.population ?? city.countUnits();
                int score = borderContacts * 35 + population + city.countWeapons() - city.countWarriors();
                if (city.isCapitalCity())
                    score -= 20;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = city;
                }
            }
            return best;
        }

        private static City ChooseBorderCity(Kingdom kingdom, Kingdom against, HashSet<long> capturedCityIds, bool prioritizeCapturedCity)
        {
            if (kingdom == null || !kingdom.isAlive())
                return null;
            if (capturedCityIds == null || capturedCityIds.Count == 0)
                return ChooseBorderCity(kingdom, against);

            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in kingdom.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                int borderContacts = CountBorderContacts(city, against);
                if (borderContacts <= 0 && against != null)
                    continue;

                int population = city.status?.population ?? city.countUnits();
                int score = borderContacts * 35 + population + city.countWeapons() - city.countWarriors();
                if (city.isCapitalCity())
                    score -= 20;
                if (CityTouchesAnyCapturedCity(city, capturedCityIds))
                    score += 140;
                if (prioritizeCapturedCity && capturedCityIds.Contains(city.id))
                    score += 220;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = city;
                }
            }

            return best;
        }

        private static int CountBorderContacts(City city, Kingdom against)
        {
            if (city == null)
                return 0;
            if (against == null)
                return 1;
            int contacts = 0;
            object borderRaw = CityBorderZonesField?.GetValue(city);
            if (borderRaw is HashSet<TileZone> borderSet)
            {
                foreach (TileZone zone in borderSet)
                {
                    if (ZoneTouchesKingdom(zone, against))
                        contacts++;
                }
            }
            else if (borderRaw is List<TileZone> borderList)
            {
                for (int i = 0; i < borderList.Count; i++)
                {
                    if (ZoneTouchesKingdom(borderList[i], against))
                        contacts++;
                }
            }
            return contacts;
        }

        private static City ChooseCityByHistoricalClaim(Kingdom from, Kingdom claimant)
        {
            if (from == null || claimant == null || !from.isAlive() || !claimant.isAlive())
                return null;
            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in from.getCities())
            {
                if (city == null || !city.isAlive())
                    continue;
                if (!CityHasHistory(city, claimant.id))
                    continue;
                int score = (city.status?.population ?? city.countUnits()) + city.countWeapons();
                if (CityTouchesKingdom(city, claimant))
                    score += 22;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = city;
                }
            }
            return best;
        }

        private static bool HasHistoricalClaim(Kingdom claimant, Kingdom owner)
        {
            if (claimant == null || owner == null || !claimant.isAlive() || !owner.isAlive())
                return false;
            foreach (City city in owner.getCities())
            {
                if (CityHasHistory(city, claimant.id))
                    return true;
            }
            return false;
        }

        private static bool CityHasHistory(City city, long kingdomId)
        {
            if (city == null || kingdomId <= 0)
                return false;
            var history = new List<long>();
            GetOwnerHistory(city, history);
            for (int i = 0; i < history.Count; i++)
            {
                if (history[i] == kingdomId)
                    return true;
            }
            return false;
        }

        private static void RefreshSupremeKingdom()
        {
            if (World.world?.kingdoms == null || World.world.kingdoms.Count == 0)
            {
                DiplomacyManager.kingdom_supreme = null;
                DiplomacyManager.kingdom_second = null;
                return;
            }

            Kingdom best = null;
            Kingdom second = null;
            float bestScore = float.MinValue;
            float secondScore = float.MinValue;

            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom == null || !kingdom.isAlive())
                    continue;
                float score = kingdom.countCities() * 8f
                    + kingdom.countTotalWarriors() * 2f
                    + kingdom.getPopulationTotal() * 0.06f
                    + kingdom.getRenown() * 0.5f;
                if (HasDockCity(kingdom))
                    score += 2f;
                if (score > bestScore)
                {
                    second = best;
                    secondScore = bestScore;
                    best = kingdom;
                    bestScore = score;
                }
                else if (score > secondScore)
                {
                    second = kingdom;
                    secondScore = score;
                }
            }

            DiplomacyManager.kingdom_supreme = best;
            DiplomacyManager.kingdom_second = second;
            DiplomacyManager.superpowers.Clear();
            if (best != null)
                DiplomacyManager.superpowers.Add(best);
            if (second != null)
                DiplomacyManager.superpowers.Add(second);
        }

        private static void MarkCeasefire(Kingdom origin, Kingdom opponent, double resumeAt)
        {
            if (origin?.data == null || opponent == null)
                return;

            origin.data.custom_data_float ??= new CustomDataContainer<float>();
            origin.data.custom_data_long ??= new CustomDataContainer<long>();

            origin.data.custom_data_float.dict[CeasefireResumeKey] = (float)resumeAt;
            origin.data.custom_data_long.dict[CeasefireEnemyKey] = opponent.id;
        }

        private static void LogBetterWar(WorldLogAsset asset, Kingdom attacker, Kingdom defender, string reason, int extra = -1)
        {
            if (asset == null)
                return;

            string reasonText = BuildCompromiseReasonText(asset, reason, extra);

            var message = new WorldLogMessage(asset, attacker?.name ?? "Unknown", defender?.name ?? "Unknown", reasonText);
            message.kingdom = attacker;
            if (attacker?.getColor() != null)
                message.color_special1 = attacker.getColor().getColorText();
            if (defender?.getColor() != null)
                message.color_special2 = defender.getColor().getColorText();
            WorldLogMetadataHelper.AttachNationMetadata(message);
            message.add();
        }

        private static void LogMediatedPeace(WorldLogAsset asset, Kingdom mediator, string context, string detail)
        {
            if (asset == null)
                return;

            var message = new WorldLogMessage(asset, mediator?.name ?? "Unknown", context ?? "between Unknown and Unknown", detail ?? "to stop further bloodshed");
            message.kingdom = mediator;
            if (mediator?.getColor() != null)
                message.color_special1 = mediator.getColor().getColorText();
            WorldLogMetadataHelper.AttachNationMetadata(message);
            message.add();
        }

        private static bool TrySelectMediator(War war, out Kingdom mediator)
        {
            mediator = null;
            if (war == null || World.world?.kingdoms == null)
                return false;

            List<Kingdom> participants = GetWarParticipants(war);
            if (participants.Count < 2)
                return false;

            var participantIds = new HashSet<long>();
            float totalPopulation = 0f;
            float totalArmy = 0f;
            float strongestPopulation = 0f;
            float strongestArmy = 0f;

            for (int i = 0; i < participants.Count; i++)
            {
                Kingdom participant = participants[i];
                if (participant == null || !participant.isAlive())
                    continue;
                participantIds.Add(participant.id);
                float population = participant.getPopulationTotal();
                float army = participant.countTotalWarriors();
                totalPopulation += population;
                totalArmy += army;
                if (population > strongestPopulation)
                    strongestPopulation = population;
                if (army > strongestArmy)
                    strongestArmy = army;
            }

            if (participantIds.Count < 2)
                return false;

            float averagePopulation = totalPopulation / participantIds.Count;
            float averageArmy = totalArmy / participantIds.Count;

            Kingdom best = null;
            float bestScore = float.MinValue;

            foreach (Kingdom candidate in World.world.kingdoms)
            {
                if (!CanAttemptMediation(candidate, participantIds, averagePopulation, averageArmy, strongestPopulation, strongestArmy, out float decisionChance, out float score))
                    continue;
                if (!Randy.randomChance(decisionChance))
                    continue;
                if (score <= bestScore)
                    continue;

                best = candidate;
                bestScore = score;
            }

            mediator = best;
            return mediator != null;
        }

        private static bool CanAttemptMediation(
            Kingdom candidate,
            HashSet<long> participantIds,
            float averagePopulation,
            float averageArmy,
            float strongestPopulation,
            float strongestArmy,
            out float decisionChance,
            out float score)
        {
            decisionChance = 0f;
            score = 0f;

            if (candidate == null || !candidate.isAlive() || participantIds.Contains(candidate.id))
                return false;

            Actor ruler = candidate.king;
            if (ruler == null || !ruler.isAlive())
                return false;

            float population = Mathf.Max(0f, candidate.getPopulationTotal());
            float army = Mathf.Max(0f, candidate.countTotalWarriors());
            bool strongByPopulation = population >= Mathf.Max(80f, averagePopulation * 1.15f) || population >= strongestPopulation * 0.9f;
            bool strongByArmy = army >= Mathf.Max(25f, averageArmy * 1.15f) || army >= strongestArmy * 0.9f;
            if (!strongByPopulation && !strongByArmy)
                return false;

            int renown = ruler.renown;
            if (renown < 35)
                return false;

            float diplomacy = ruler.stats?.get("diplomacy") ?? 0f;
            float populationScore = population / Mathf.Max(1f, averagePopulation);
            float armyScore = army / Mathf.Max(1f, averageArmy);
            float renownScore = Mathf.Clamp01(renown / 160f);
            float diplomacyScore = Mathf.Clamp01(diplomacy / 12f);

            score = populationScore * 0.4f + armyScore * 0.35f + renownScore * 0.2f + diplomacyScore * 0.15f;
            decisionChance = Mathf.Clamp01(
                0.2f
                + Mathf.Clamp01(populationScore - 1f) * 0.2f
                + Mathf.Clamp01(armyScore - 1f) * 0.2f
                + renownScore * 0.25f
                + diplomacyScore * 0.1f);

            return true;
        }

        private static List<Kingdom> GetWarParticipants(War war)
        {
            var result = new List<Kingdom>();
            if (war == null)
                return result;

            var seen = new HashSet<long>();
            foreach (Kingdom kingdom in war.getAttackers())
            {
                if (kingdom == null || !kingdom.isAlive() || !seen.Add(kingdom.id))
                    continue;
                result.Add(kingdom);
            }

            foreach (Kingdom kingdom in war.getDefenders())
            {
                if (kingdom == null || !kingdom.isAlive() || !seen.Add(kingdom.id))
                    continue;
                result.Add(kingdom);
            }

            return result;
        }

        private static string BuildWarDisplayName(War war)
        {
            string warName = war?.name;
            if (string.IsNullOrWhiteSpace(warName))
            {
                string attacker = war?.getMainAttacker()?.name ?? "Unknown";
                string defender = war?.getMainDefender()?.name ?? "Unknown";
                warName = $"{attacker}-{defender} war";
            }
            if (warName.IndexOf("war", StringComparison.OrdinalIgnoreCase) < 0)
                warName = $"{warName} war";
            return warName;
        }

        private static string BuildMediationDenialDetail(string reasonDetail, bool multiPartyWar, List<Kingdom> deniers)
        {
            if (deniers == null || deniers.Count == 0)
                return reasonDetail ?? "to stop further bloodshed";
            if (deniers.Count == 1)
                return $"{reasonDetail}, but {deniers[0]?.name ?? "Unknown"} denied it";
            if (multiPartyWar)
                return $"{reasonDetail}, but a few nations denied making peace";
            return $"{reasonDetail}, but both nations denied it";
        }

        private static string BuildCompromiseReasonText(WorldLogAsset asset, string reason, int extra = -1)
        {
            if (asset == WorldLogAssets.WhitePeace)
                return BuildWhitePeaceReasonText(reason);
            if (asset == WorldLogAssets.StatusQuo)
                return BuildStatusQuoReasonText(reason);
            if (asset == WorldLogAssets.Ceasefire)
                return BuildCeasefireReasonText(reason, extra);
            if (asset == WorldLogAssets.ConditionalSurrender)
                return BuildConditionalReasonText(reason);
            if (asset == WorldLogAssets.Tribute)
                return BuildTributeReasonText(reason);
            if (asset == WorldLogAssets.Puppet)
                return BuildPuppetReasonText(reason);
            if (asset == WorldLogAssets.PuppetSideSwap)
                return BuildPuppetSideSwapReasonText(reason);
            if (asset == WorldLogAssets.GoalAchieved)
                return BuildGoalReasonText(reason);
            return BuildGenericReasonText(reason);
        }

        private static string BuildWhitePeaceReasonText(string reason)
        {
            return $"after both sides were exhausted by {GetReasonTopic(reason)}";
        }

        private static string BuildStatusQuoReasonText(string reason)
        {
            return $"while keeping the lines formed around {GetReasonTopic(reason)}";
        }

        private static string BuildCeasefireReasonText(string reason, int extra)
        {
            int seconds = extra >= 0 ? Mathf.Max(1, extra) : Randy.randomInt(25, 41);
            return $"regroup around {GetReasonTopic(reason)} for {seconds} seconds";
        }

        private static string BuildConditionalReasonText(string reason)
        {
            return $"after being cornered by losses tied to {GetReasonTopic(reason)}";
        }

        private static string BuildTributeReasonText(string reason)
        {
            return $"to buy time while negotiating {GetReasonTopic(reason)}";
        }

        private static string BuildPuppetReasonText(string reason)
        {
            return $"after collapsing under pressure from {GetReasonTopic(reason)}";
        }

        private static string BuildPuppetSideSwapReasonText(string reason)
        {
            switch (reason)
            {
                case "overlord":
                    return "after facing their overlord in war";
                case "overlord_ally":
                    return "after facing an overlord ally in war";
                default:
                    return "to honor overlord obligations";
            }
        }

        private static string BuildGoalReasonText(string reason)
        {
            switch (reason)
            {
                case "seize_city":
                    return "by securing the targeted city";
                case "independence":
                    return "by breaking puppet control";
                case "vassalize":
                    return "by forcing vassal terms";
                case "trade_route":
                    return "by taking key trade routes";
                case "convert":
                    return "by enforcing conversion terms";
                default:
                    return $"through pressure linked to {GetReasonTopic(reason)}";
            }
        }

        private static string BuildGenericReasonText(string reason)
        {
            if (string.IsNullOrEmpty(reason))
                return "amid the escalating conflict";
            return GetReasonTopic(reason);
        }

        private static string BuildMediationReasonDetail(string reason)
        {
            switch (reason)
            {
                case "territorial_reclaim":
                    return "to settle disputed borders";
                case "border_dispute":
                    return "to halt a border dispute";
                case "imperial_expansion":
                    return "to contain imperial expansion";
                case "world_dominance":
                    return "to prevent regional domination";
                case "independence":
                    return "to settle independence claims";
                case "civil_war":
                    return "to contain a civil conflict";
                case "succession":
                    return "to settle succession claims";
                case "religious":
                    return "to resolve religious conflict";
                case "cultural_supremacy":
                    return "to ease cultural supremacy tensions";
                case "purification":
                    return "to stop purification campaigns";
                case "resource":
                    return "to stabilize access to resources";
                case "trade_war":
                    return "to restore trade access";
                case "naval_supremacy":
                    return "to secure sea lanes";
                case "strategic_defense":
                    return "for strategic defense";
                case "puppet_enforcement":
                    return "to curb puppet enforcement";
                case "liberation":
                    return "to secure liberation terms";
                case "forced_vassalization":
                    return "to prevent forced vassalization";
                default:
                    return "to stop further bloodshed";
            }
        }

        private static string HumanizeReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return null;

            string[] parts = reason.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (string.IsNullOrEmpty(part))
                    continue;
                string lower = part.ToLowerInvariant();
                if (lower == "war")
                {
                    parts[i] = "War";
                    continue;
                }
                if (lower == "of")
                {
                    parts[i] = "of";
                    continue;
                }
                if (part.Length == 1)
                {
                    parts[i] = char.ToUpperInvariant(part[0]).ToString();
                    continue;
                }
                parts[i] = char.ToUpperInvariant(part[0]) + part.Substring(1).ToLowerInvariant();
            }

            return string.Join(" ", parts);
        }

        private static string GetReasonTopic(string reason)
        {
            switch (reason)
            {
                case "territorial_reclaim":
                    return "disputed border lands";
                case "border_dispute":
                    return "a worsening border dispute";
                case "imperial_expansion":
                    return "imperial expansion pressure";
                case "world_dominance":
                    return "the struggle for regional dominance";
                case "independence":
                    return "independence demands";
                case "civil_war":
                    return "a civil fracture";
                case "succession":
                    return "a succession crisis";
                case "religious":
                    return "religious legitimacy disputes";
                case "cultural_supremacy":
                    return "cultural supremacy demands";
                case "purification":
                    return "purification campaigns";
                case "resource":
                    return "competition for key resources";
                case "trade_war":
                    return "control of trade routes";
                case "naval_supremacy":
                    return "control of sea lanes";
                case "strategic_defense":
                    return "strategic defense priorities";
                case "puppet_enforcement":
                    return "puppet enforcement demands";
                case "liberation":
                    return "liberation demands";
                case "forced_vassalization":
                    return "forced vassalization pressure";
                default:
                    return "the conflict";
            }
        }

        private static void ScheduleNextPeaceCheck(War war, float seconds)
        {
            war.data.custom_data_float ??= new CustomDataContainer<float>();
            war.data.custom_data_float.dict[NextCheckKey] = (float)(World.world.getCurWorldTime() + seconds);
        }

        private static void EnsureContainers(WarData data)
        {
            data.custom_data_string ??= new CustomDataContainer<string>();
            data.custom_data_float ??= new CustomDataContainer<float>();
            data.custom_data_long ??= new CustomDataContainer<long>();
            data.custom_data_bool ??= new CustomDataContainer<bool>();
        }

        private static bool ShouldProcess(War war)
        {
            if (war?.data == null)
                return false;

            string id = war.getAsset()?.id;
            if (string.IsNullOrEmpty(id))
                return false;

            return !id.Equals("whisper_of_war", StringComparison.Ordinal);
        }

        private static void SetFloat(BaseSystemData data, string key, float value)
        {
            data.custom_data_float ??= new CustomDataContainer<float>();
            data.custom_data_float.dict[key] = value;
        }

        private static void SetLong(BaseSystemData data, string key, long value)
        {
            data.custom_data_long ??= new CustomDataContainer<long>();
            data.custom_data_long.dict[key] = value;
        }

        private static int GetStoredInt(WarData data, string key, int fallback)
        {
            if (TryGetFloat(data, key, out float value))
                return Mathf.RoundToInt(value);
            return fallback;
        }

        private static bool TryGetFloat(BaseSystemData data, string key, out float value)
        {
            value = 0f;
            return data?.custom_data_float != null && data.custom_data_float.dict.TryGetValue(key, out value);
        }

        private static bool TryGetLong(BaseSystemData data, string key, out long value)
        {
            value = 0;
            return data?.custom_data_long != null && data.custom_data_long.dict.TryGetValue(key, out value);
        }

        public static bool IsDemilitarized(Kingdom kingdom)
        {
            if (kingdom?.data == null)
                return false;
            if (TryGetFloat(kingdom.data, DemilitarizedUntilKey, out float until))
            {
                double now = World.world.getCurWorldTime();
                if (until > now)
                    return true;
                kingdom.data.custom_data_float.dict.Remove(DemilitarizedUntilKey);
            }
            return false;
        }

        public static bool TryGetOverlord(Kingdom kingdom, out Kingdom overlord)
        {
            overlord = null;
            if (kingdom?.data == null)
                return false;
            if (TryGetLong(kingdom.data, PuppetOfKey, out long id))
            {
                overlord = World.world.kingdoms.get(id);
                if (overlord == null || !overlord.isAlive())
                {
                    kingdom.data.custom_data_long.dict.Remove(PuppetOfKey);
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool IsKingdomInOverlordBloc(Kingdom overlord, Kingdom kingdom)
        {
            return IsKingdomInOverlordBlocInternal(overlord, kingdom);
        }

        public static bool AreKingdomsAllianceCompatible(Kingdom first, Kingdom second)
        {
            if (first == null || second == null || !first.isAlive() || !second.isAlive() || first == second)
                return false;
            return CanKingdomJoinAllianceWith(first, second) && CanKingdomJoinAllianceWith(second, first);
        }

        public static bool CanJoinAlliance(Kingdom kingdom, Alliance alliance)
        {
            if (kingdom == null || !kingdom.isAlive() || alliance == null)
                return false;
            if (alliance.kingdoms_hashset == null)
                return true;

            if (TryGetOverlord(kingdom, out var overlord) && overlord != null && overlord.isAlive())
            {
                if (IsAllianceConflictingForPuppet(kingdom, overlord, alliance))
                    return false;
            }

            foreach (Kingdom member in alliance.kingdoms_hashset)
            {
                if (member == null || !member.isAlive() || member == kingdom)
                    continue;
                if (!AreKingdomsAllianceCompatible(kingdom, member))
                    return false;
            }

            return true;
        }

        private static bool CanKingdomJoinAllianceWith(Kingdom kingdom, Kingdom other)
        {
            if (kingdom == null || other == null || !kingdom.isAlive() || !other.isAlive() || kingdom == other)
                return false;
            if (!TryGetOverlord(kingdom, out var overlord) || overlord == null || !overlord.isAlive())
                return true;
            return !IsKingdomConflictingWithOverlordBloc(overlord, other, kingdom);
        }

        private static void ApplyDemilitarization(Kingdom kingdom, double until)
        {
            SetFloat(kingdom.data, DemilitarizedUntilKey, (float)until);
        }

        private static void EnsureDefenderOverlordJoins(Kingdom attacker, Kingdom defender)
        {
            if (attacker == null || defender == null)
                return;
            if (TryGetOverlord(defender, out var overlord) && overlord != null && overlord != attacker && overlord.isAlive())
            {
                if (TryGetOverlord(attacker, out var attackerOverlord) && attackerOverlord == overlord)
                    return;
                if (!World.world.wars.isInWarWith(overlord, attacker))
                {
                    WarTypeAsset type = AssetManager.war_types_library.get("normal");
                    World.world.wars.newWar(overlord, attacker, type);
                    LogBetterWar(WorldLogAssets.OverlordJoins, overlord, attacker, "defend_puppet");
                }
            }
        }

        private sealed class WarContext
        {
            public War War;
            public Kingdom Attacker;
            public Kingdom Defender;
            public string Reason;
            public float WarDurationYears;
            public bool DefenderLosingBadly;
            public bool AttackerLosingBadly;
            public bool IsStalemate;
            public float DefenderDesperation;
            public float GlobalExhaustion;
            public float AttackerDiplomacy;
            public float DefenderDiplomacy;
            public bool AttackerBoldButThin;
            public bool DefenderStillStrong;
            public bool CanPuppet;
            public bool CanPayTribute;
            public bool CanResumeLater;
            public WarGoal Goal;
            public bool AttackerGoalStalled;
            public bool DefenderLostCoreCity;
            public bool AttackerLostCoreCity;
            public float DefenderCityLossRatio;
            public float DefenderArmyLossRatio;
            public float AttackerCityLossRatio;
            public float AttackerArmyLossRatio;
            public float AverageDefenderLoyalty;
            public float PopulationPressure;
            public float PuppetWillingness;
            public bool TerritoryWar;
            public bool BothHaveDocks;
            public int AttackerCapturedEnemyCities;
            public int DefenderCapturedEnemyCities;
        }

        private sealed class WarGoal
        {
            public string Type;
            public long TargetCityId;
        }
    }
}
