using System;
using System.Collections.Generic;
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

        private static double _lastGlobalTick;

        public static void OnWarStarted(War war, Kingdom attacker, Kingdom defender)
        {
            if (!ShouldProcess(war))
                return;

            EnsureContainers(war.data);
            if (!war.data.custom_data_string.dict.ContainsKey(ReasonKey))
                war.data.custom_data_string.dict[ReasonKey] = SelectReason(attacker, defender);
            SetWarGoal(war, attacker, defender);
            war.data.custom_data_float.dict[StartAttackerCitiesKey] = attacker?.countCities() ?? 0;
            war.data.custom_data_float.dict[StartDefenderCitiesKey] = defender?.countCities() ?? 0;
            war.data.custom_data_float.dict[StartAttackerArmyKey] = attacker?.units.Count ?? 0;
            war.data.custom_data_float.dict[StartDefenderArmyKey] = defender?.units.Count ?? 0;
            EnsureDefenderOverlordJoins(attacker, defender);
            ScheduleNextPeaceCheck(war, 8f);
        }

        public static void TickWar(War war)
        {
            if (!ShouldProcess(war) || war.hasEnded())
                return;

            EnsureContainers(war.data);
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
        }

        private static void TryTriggerPeaceAction(War war, WarContext ctx)
        {
            if (ctx == null || ctx.Attacker == null || ctx.Defender == null)
                return;

            float desperation = ctx.DefenderDesperation;
            float exhaustion = ctx.GlobalExhaustion;
            float diplomacyBias = Mathf.Max(ctx.AttackerDiplomacy, ctx.DefenderDiplomacy);

            if (desperation < 0.15f && exhaustion < 0.1f && ctx.WarDurationYears < 2f && !ctx.DefenderLostCoreCity)
                return;

            if (desperation > 0.55f)
            {
                if (ctx.DefenderLosingBadly)
                {
                    if (ctx.CanPuppet)
                    {
                        ApplyPuppetAgreement(war, ctx);
                        return;
                    }
                    ApplyConditionalSurrender(war, ctx);
                    return;
                }

                if (ctx.CanPayTribute)
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
                if (exhaustion > 0.25f && ctx.CanResumeLater)
                {
                    ApplyCeasefire(war, ctx);
                    return;
                }

                ApplyWhitePeace(war, ctx);
                return;
            }

            if (ctx.AttackerBoldButThin && ctx.DefenderStillStrong)
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
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.WhitePeace, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyCeasefire(War war, WarContext ctx)
        {
            double resumeAt = World.world.getCurWorldTime() + UnityEngine.Random.Range(25f, 40f);

            MarkCeasefire(ctx.Attacker, ctx.Defender, resumeAt);
            MarkCeasefire(ctx.Defender, ctx.Attacker, resumeAt);

            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.Ceasefire, ctx.Attacker, ctx.Defender, ctx.Reason, Mathf.RoundToInt((float)(resumeAt - World.world.getCurWorldTime())));
        }

        private static void ApplyMediatedPeace(War war, WarContext ctx)
        {
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.MediatedPeace, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyConditionalSurrender(War war, WarContext ctx)
        {
            TransferBorderCity(ctx.Defender, ctx.Attacker);

            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.ConditionalSurrender, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyTributeAgreement(War war, WarContext ctx)
        {
            double until = World.world.getCurWorldTime() + 60f;
            SetFloat(ctx.Defender.data, ReparationsUntilKey, (float)until);
            SetLong(ctx.Defender.data, ReparationsTargetKey, ctx.Attacker.id);
            World.world.wars.endWar(war, WarWinner.Peace);
            LogBetterWar(WorldLogAssets.Tribute, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyPuppetAgreement(War war, WarContext ctx)
        {
            SetLong(ctx.Defender.data, PuppetOfKey, ctx.Attacker.id);
            SetFloat(ctx.Defender.data, DemilitarizedUntilKey, (float)(World.world.getCurWorldTime() + 90f));
            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.Puppet, ctx.Attacker, ctx.Defender, ctx.Reason);
        }

        private static void ApplyGoalPeace(War war, WarContext ctx, string goalLabel)
        {
            World.world.wars.endWar(war, WarWinner.Attackers);
            LogBetterWar(WorldLogAssets.GoalAchieved, ctx.Attacker, ctx.Defender, goalLabel);
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

            bool defenderLosing = defenderPower < attackerPower * 0.65f || war.getDeadDefenders() > war.getDeadAttackers() * 1.35f;
            bool stalemate = Mathf.Abs(attackerPower - defenderPower) < Mathf.Max(4f, attackerPower * 0.1f);

            float desperation = Mathf.Clamp01((1f - defMood) * 0.6f + (defenderLosing ? 0.4f : 0f) + Mathf.Clamp01(warDurationYears / 12f));
            float exhaustion = Mathf.Clamp01((war.getDeadAttackers() + war.getDeadDefenders()) / Mathf.Max(1f, war.countTotalArmy() * 2f));
            float globalWarPressure = Mathf.Clamp01(World.world.wars.Count / 6f);

            NationTypeDefinition atkType = NationTypeManager.GetDefinition(attacker);
            bool puppetPossible = atkType.SuccessionMode == NationSuccessionMode.RoyalLine || atkType.SuccessionMode == NationSuccessionMode.Religious;
            bool tributePossible = defender.countCities() > 0;
            WarGoal goal = GetGoal(war);
            City goalCity = goal.TargetCityId > 0 ? World.world.cities.get(goal.TargetCityId) : null;
            bool attackerGoalStalled = goalCity != null && goalCity.kingdom != null && goalCity.kingdom != defender && goalCity.kingdom != attacker;
            bool defenderLostCoreCity = goalCity != null && goalCity.kingdom == attacker;
            int attackerCitiesStart = GetStoredInt(war.data, StartAttackerCitiesKey, attacker.countCities());
            int defenderCitiesStart = GetStoredInt(war.data, StartDefenderCitiesKey, defender.countCities());
            int attackerCitiesNow = attacker.countCities();
            int defenderCitiesNow = defender.countCities();
            bool attackerLostCoreCity = attackerCitiesNow < attackerCitiesStart;
            bool defenderLostAnyCity = defenderCitiesNow < defenderCitiesStart;

            return new WarContext
            {
                War = war,
                Attacker = attacker,
                Defender = defender,
                Reason = reason,
                WarDurationYears = warDurationYears,
                DefenderLosingBadly = defenderLosing,
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
                DefenderLostCoreCity = defenderLostAnyCity,
                AttackerLostCoreCity = attackerLostCoreCity
            };
        }

        private static float GetMoodScore(Actor ruler)
        {
            if (ruler?.data == null)
                return 0.6f;

            float happy = ruler.data.happiness;
            return Mathf.Clamp01(happy / 100f);
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

            if (attacker != null && attacker.countCities() >= 5 && defender != null && defender.countCities() >= 5)
                return "world_dominance";

            if (attacker != null && attacker.countCities() <= 2 && defender != null && defender.countCities() >= 4)
                return "strategic_defense";

            return reasons[Randy.randomInt(0, reasons.Count)];
        }

        private static string GetReason(War war)
        {
            if (war?.data?.custom_data_string != null && war.data.custom_data_string.dict.TryGetValue(ReasonKey, out var value))
                return value;
            return "unknown";
        }

        private static void SetWarGoal(War war, Kingdom attacker, Kingdom defender)
        {
            if (war?.data == null)
                return;
            WarGoal goal = SelectGoal(attacker, defender, GetReason(war));
            war.data.custom_data_string.dict[GoalTypeKey] = goal.Type;
            if (goal.TargetCityId > 0)
            {
                war.data.custom_data_long ??= new CustomDataContainer<long>();
                war.data.custom_data_long.dict[GoalTargetCityKey] = goal.TargetCityId;
            }
        }

        private static WarGoal SelectGoal(Kingdom attacker, Kingdom defender, string reason)
        {
            City target = ChooseTargetCity(attacker, defender);
            string type = "seize_city";
            if (reason == "independence")
                type = "break_puppet";
            else if (reason == "puppet_enforcement" || reason == "forced_vassalization")
                type = "vassalize";
            else if (reason == "trade_war")
                type = "trade_route";
            else if (reason == "religious")
                type = "convert";
            return new WarGoal { Type = type, TargetCityId = target?.id ?? -1 };
        }

        private static City ChooseTargetCity(Kingdom attacker, Kingdom defender)
        {
            if (defender == null)
                return null;
            City best = null;
            int bestScore = int.MinValue;
            foreach (City city in defender.getCities())
            {
                int score = city.status?.population ?? 0;
                score -= city.countUnits();
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
            if (ctx.Goal.Type == "seize_city" && ctx.Goal.TargetCityId > 0)
            {
                City city = World.world.cities.get(ctx.Goal.TargetCityId);
                if (city != null && city.kingdom == ctx.Attacker)
                {
                    ApplyGoalPeace(war, ctx, "seize_city");
                    return true;
                }
            }
            if (ctx.Goal.Type == "break_puppet")
            {
                if (TryGetOverlord(ctx.Attacker, out var overlord) && overlord == ctx.Defender)
                {
                    if (ctx.WarDurationYears > 1.2f && ctx.DefenderLosingBadly)
                    {
                        ApplyIndependence(ctx.Attacker, ctx.Defender);
                        World.world.wars.endWar(war, WarWinner.Attackers);
                        LogBetterWar(WorldLogAssets.GoalAchieved, ctx.Attacker, ctx.Defender, "independence");
                        return true;
                    }
                }
            }
            if (ctx.Goal.Type == "vassalize" && ctx.DefenderLosingBadly)
            {
                ApplyPuppetAgreement(war, ctx);
                return true;
            }
            return false;
        }

        private static void TransferBorderCity(Kingdom from, Kingdom to)
        {
            if (from == null || to == null)
                return;

            City chosen = null;
            int bestScore = int.MinValue;
            foreach (City city in from.getCities())
            {
                int score = -city.countUnits() + city.countWeapons();
                if (city.getTile() != null && city.kingdom != null && city.kingdom.isEnemy(to))
                    score += 3;
                if (score > bestScore)
                {
                    bestScore = score;
                    chosen = city;
                }
            }

            if (chosen != null)
                chosen.joinAnotherKingdom(to, true);
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

            string reasonText = reason ?? "unknown";
            if (extra >= 0)
                reasonText = $"{reasonText} ({extra}s)";

            var message = new WorldLogMessage(asset, attacker?.name ?? "Unknown", defender?.name ?? "Unknown", reasonText);
            message.kingdom = attacker;
            if (attacker?.getColor() != null)
                message.color_special1 = attacker.getColor().getColorText();
            if (defender?.getColor() != null)
                message.color_special2 = defender.getColor().getColorText();
            WorldLogMetadataHelper.AttachNationMetadata(message);
            message.add();
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
        }

        private sealed class WarGoal
        {
            public string Type;
            public long TargetCityId;
        }
    }
}
