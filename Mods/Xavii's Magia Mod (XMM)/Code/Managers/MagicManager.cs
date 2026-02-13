using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ai.behaviours;
using UnityEngine;
using XaviiMagiaMod.Code.Data;
using XaviiMagiaMod.Code.Extensions;
using XaviiMagiaMod.Code.Patches;

namespace XaviiMagiaMod.Code.Managers
{
    public sealed class MagicManager : MonoBehaviour
    {
        private const string OrlTraitId = "magic_orl";
        private const string OrlCounterKey = "magic_orl_reincarnations";
        private const string OrlBoostKey = "magic_orl_power_boosts";
        private const int OrlReincarnationLimit = 3;
        private const string OrlSpellId = "magic_orl_passive";
        private const string OrlIconPath = "ui/Icons/iconFavoriteStar";
        private const string DemonLordTraitId = "magic_demonlord";
        private const string DemonLordSpellEternalId = "magic_demonlord_eternal";
        private const string DemonLordSpellJudgementId = "magic_demonlord_judgement";
        private const string DemonLordSpellSummonId = "magic_demonlord_summon";
        private const string DemonLordIconPath = "ui/Icons/iconDemon";
        private const string HeroTraitId = "magic_hero";
        private const string HeroPowerCycleKey = "magic_hero_power_cycles";
        private const float HeroCheckInterval = 10f;
        private const string MentorTraitId = "magic_mentor";
        private const string MentorHeroKey = "magic_mentor_target";
        private const string HeroMentorKey = "magic_hero_mentor";
        private const string GodTraitId = "magic_god";
        private const string GodIconPath = "ui/Icons/actor_traits/iconSunblessed";
        private const string HeroSpellArcaneBlastId = "magic_hero_arcane_blast";
        private const string HeroSpellRadiantBurstId = "magic_hero_radiant_burst";
        private const string GodSpellDivineStormId = "magic_god_divine_storm";
        private const string GodSpellSolarJudgmentId = "magic_god_solar_judgment";
        private const string GodTimePowerScaleKey = "magic_god_age_power_scale";
        private const int GodTimeBabyAgeMax = 50;
        private const int GodTimeTeenAgeMax = 250;
        private const int GodTimeYoungAdultAgeMax = 1000;
        private const int GodTimeAdultAgeMax = 5000;
        private const string ImmortalTraitId = "immortal";
        private const string HeroPartyWhiteSpellId = "magic_hero_party_white_barrier";
        private const string HeroPartyTankSpellId = "magic_hero_party_tank_slam";
        private const string HeroPartyAssassinSpellId = "magic_hero_party_assassin_strike";
        private const string HeroPartyKnightSpellId = "magic_hero_party_knight_charge";
        private const string HeroPartyArcherSpellId = "magic_hero_party_archer_volley";
        private const string HeroPartyBarbarianSpellId = "magic_hero_party_barbarian_frenzy";
        private const string HeroPartyBlockKeyPrefix = "magic_god_block_hero_party_";
        private const string SealedTraitId = "magic_sealed";
        private const string PermaSealedTraitId = "magic_permasealed";
        private const string DemonLordSealTimestampKey = "magic_demonlord_sealed_at";
        private const string DemonLordSummonTimestampKey = "magic_demonlord_summon_at";
        private const int DemonLordSummonCooldownYears = 3;
        private const string DemonLordRebellionFlagKey = "magic_demonlord_rebel_done";
        private const int DemonLordRebellionMinAge = 30;
        private const int DemonLordSummonCount = 30;
        private const string HeroPartyHeroKey = "magic_hero_party_hero";
        private const float MentorFollowDistanceSq = 16f;
        private const float MentorSealDistanceSq = 9f;
        private const string DestinyStateKey = "magic_destiny_state";
        private const string DestinyTimerKey = "magic_destiny_timer";
        private const string DestinyDecisionDelayKey = "magic_destiny_decision_delay";
        private const string HeroPartyDecisionDelayKey = "magic_hero_party_decision_delay";
        private const string HeroPartySacredFlagKey = "magic_hero_party_sacred_flag";
        private const string HeroBondTimeKey = "magic_hero_bond_time";
        private const string HeroBondLevelKey = "magic_hero_bond_level";
        private const float HeroRespondDistanceSq = 2500f;
        private const float HeroRespondPowerThreshold = 6000f;
        private const float HeroPreparingDelay = 30f;
        private const float HeroFightingEnterDistance = 18f;
        private const float HeroFightingExitDistance = 28f;
        private const float HeroPartyRespondDistanceSq = 1600f;
        private const float HeroPartyCombatAssistDistanceSq = 400f;
        private const float HeroPartyAwarenessWindowMin = 10f;
        private const float HeroPartyAwarenessWindowMax = 20f;
        private const float HeroPartyPreparingWindowMin = 3f;
        private const float HeroPartyPreparingWindowMax = 6f;
        private const float HeroPartyRespondingWindowMin = 3f;
        private const float HeroPartyRespondingWindowMax = 6f;
        private const float HeroPartyTetherNearDistance = 14f;
        private const float HeroPartyTetherFarDistance = 32f;
        private const float HeroPartyBattleOffsetRadius = 4f;
        private const float HeroPartyAwarenessChance = 0.7f;
        private const float HeroBondLevelOneTime = 15f;
        private const float HeroBondLevelTwoTime = 40f;
        private const float HeroBondDecayRate = 0.5f;
        private const float HeroBondDamagePerLevel = 2f;
        private const float HeroBondHealthPerLevel = 4f;
        private const float HeroBondCriticalPerLevel = 0.015f;
        private const float HeroBondComboAttackSpeed = 0.08f;
        private const float MentorPreparingDelay = 20f;
        private const float MentorRespondDistanceSq = 2500f;
        private const float HeroStateDecisionWindowMin = 4f;
        private const float HeroStateDecisionWindowMax = 7f;
        private static readonly float HeroFightingEnterDistanceSq = HeroFightingEnterDistance * HeroFightingEnterDistance;
        private static readonly float HeroFightingExitDistanceSq = HeroFightingExitDistance * HeroFightingExitDistance;
        private static readonly float HeroPartyTetherNearSq = HeroPartyTetherNearDistance * HeroPartyTetherNearDistance;
        private static readonly float HeroPartyTetherFarSq = HeroPartyTetherFarDistance * HeroPartyTetherFarDistance;
        private static readonly float HeroPartyNearHeroEnterSq = HeroPartyTetherNearDistance * HeroPartyTetherNearDistance;
        private static readonly float HeroPartyNearHeroExitSq = HeroPartyTetherFarDistance * HeroPartyTetherFarDistance;
        private static readonly float HeroPartyNearDemonEnterSq = 16f * 16f;
        private static readonly float HeroPartyNearDemonExitSq = 24f * 24f;

        private enum DestinyState
        {
            NormalLife,
            AwareOfThreat,
            Preparing,
            Responding,
            Fighting
        }
        private enum ChampionTier
        {
            HeroParty,
            Hero,
            Legendary
        }

        private readonly struct ChampionStatTargets
        {
            public float Health { get; }
            public float Mana { get; }
            public float Damage { get; }
            public float Armor { get; }
            public float Range { get; }
            public float Speed { get; }
            public float AttackSpeed { get; }
            public float Critical { get; }

            public ChampionStatTargets(float health, float mana, float damage, float armor, float range, float speed, float attackSpeed, float critical)
            {
                Health = health;
                Mana = mana;
                Damage = damage;
                Armor = armor;
                Range = range;
                Speed = speed;
                AttackSpeed = attackSpeed;
                Critical = critical;
            }
        }
        private static readonly string[] HeroPartyTraitIds =
        {
            "magic_hero_party_white",
            "magic_hero_party_tank",
            "magic_hero_party_assassin",
            "magic_hero_party_knight",
            "magic_hero_party_archer",
            "magic_hero_party_barbarian"
        };
        private static readonly HashSet<string> HeroPartyTraitSet = new HashSet<string>(HeroPartyTraitIds);
        private const float HeroPartyFollowDistanceSq = 25f;
        private const int DemonLordExplosionRadius = 70;
        private const float DemonLordExplosionShakeIntensity = 2.5f;
        private const float DemonLordExplosionScaleMin = 1.4f;
        private const float DemonLordExplosionScaleMax = 1.6f;
        private const int DemonLordReincarnationBuffer = 15;
        private const string MentorLogBornId = "magic_mentor_born";
        private const string HeroPartyLogBornId = "magic_hero_party_born";
        private const string HeroPartyLogDeathId = "magic_hero_party_fallen";
        private const string SpellCooldownKeyPrefix = "magic_spell_cd_";
        private const string NoElementTypeId = "none";
        private const string MagiaStatusId = "magia_casting";
        private const string ChargingMagiaStatusId = "magia_charging";
        private const string ChargingMagiaSpellKey = "magic_charging_spell";
        private const string MagiaSpellKey = "magic_magia_spell";
        private const float CombatSpellFrequencyMultiplier = 1.99f;
        private const string MagiaTitleKey = "status_title_magia";
        private const string MagiaDescriptionKey = "status_description_magia";
        private const string ChargingTitleKey = "status_title_charging_magia";
        private const string ChargingDescriptionKey = "status_description_charging_magia";
        private const string PlayerAffinityDataKey = "magic_player_affinities";
        private const char PlayerAffinitySeparator = ';';
        private const float AffinityMonitorInterval = 5f;
        private const string SpellPlaceholder = "$spell$";
        private const string HeroLogBornId = "magic_hero_born";
        private const string HeroLogDeathId = "magic_hero_fallen";
        private const string DemonLordLogBornId = "magic_demonlord_born";
        private const string DemonLordLogDeathId = "magic_demonlord_fallen";
        private const string DemonLordLogSealId = "magic_demonlord_sealed";
        private const string DemonLordLogUnsealId = "magic_demonlord_unsealed";
        private const string GodLogBornId = "magic_god_born";
        private const string GodLogDeathId = "magic_god_fallen";
        private static readonly string[] OrlBoostStatIds =
        {
            "damage",
            "health",
            "armor",
            "speed",
            "stamina",
            "mana",
            "attack_speed",
            "critical_chance",
            "range"
        };
        private static readonly AttackAction FireDropsSpawnAction = (pSelf, pTarget, pTile) => ActionLibrary.fireDropsSpawn(pTarget, pTile);
        private static readonly AttackAction DeathBombAction = (pSelf, pTarget, pTile) => ActionLibrary.deathBomb(pTarget, pTile);

        public static MagicManager Instance { get; private set; }

        private readonly List<MagicTypeDefinition> _typeDefinitions = new List<MagicTypeDefinition>
        {
            new MagicTypeDefinition("pyro", "magic_pyro", "ui/Icons/iconFire"),
            new MagicTypeDefinition("aero", "magic_aero", "ui/Icons/actor_traits/iconLightning"),
            new MagicTypeDefinition("aqua", "magic_aqua", "ui/Icons/iconRain"),
            new MagicTypeDefinition("terra", "magic_terra", "ui/Icons/iconStone"),
            new MagicTypeDefinition("haro", "magic_haro", "ui/Icons/actor_traits/iconSunblessed"),
            new MagicTypeDefinition("barku", "magic_barku", "ui/Icons/actor_traits/iconNightchild"),
            new MagicTypeDefinition("none", "magic_none", "ui/Icons/iconFavoriteKilled"),
            new MagicTypeDefinition("orl", OrlTraitId, OrlIconPath, allowAutomatic: false),
            new MagicTypeDefinition("demonlord", DemonLordTraitId, DemonLordIconPath, allowAutomatic: false),
            new MagicTypeDefinition("hero", HeroTraitId, "ui/Icons/actor_traits/iconAttractive", allowAutomatic: false),
            new MagicTypeDefinition("god", GodTraitId, GodIconPath, allowAutomatic: false),
            new MagicTypeDefinition("hero_party_white", "magic_hero_party_white", "ui/Icons/iconShield", allowAutomatic: false),
            new MagicTypeDefinition("hero_party_tank", "magic_hero_party_tank", "ui/Icons/iconFamilyDestroyed", allowAutomatic: false),
            new MagicTypeDefinition("hero_party_assassin", "magic_hero_party_assassin", "ui/Icons/iconBandit", allowAutomatic: false),
            new MagicTypeDefinition("hero_party_knight", "magic_hero_party_knight", "ui/Icons/iconDragon", allowAutomatic: false),
            new MagicTypeDefinition("hero_party_archer", "magic_hero_party_archer", "ui/Icons/iconWalker", allowAutomatic: false),
            new MagicTypeDefinition("hero_party_barbarian", "magic_hero_party_barbarian", "ui/Icons/iconSnowMan", allowAutomatic: false),
            new MagicTypeDefinition("mentor", MentorTraitId, "ui/Icons/iconHelixDNA", allowAutomatic: false),
            new MagicTypeDefinition("sealed", SealedTraitId, "ui/Icons/iconFrozen", allowAutomatic: false),
            new MagicTypeDefinition("permasealed", PermaSealedTraitId, "ui/Icons/iconFrozen", allowAutomatic: false)
        };

        private readonly Dictionary<string, string[]> _typeSpellMap = new Dictionary<string, string[]>
        {
            { "pyro", new[] { "magic_pyro_flare", "magic_pyro_rain", "magic_pyro_shield", "magic_pyro_burst" } },
            { "aero", new[] { "magic_aero_torrent", "magic_aero_crackle", "magic_aero_shift", "magic_aero_whirl" } },
            { "aqua", new[] { "magic_aqua_wave", "magic_aqua_deluge", "magic_aqua_refresh", "magic_aqua_purity" } },
            { "terra", new[] { "magic_terra_rise", "magic_terra_quake", "magic_terra_stone", "magic_terra_shockwave" } },
            { "haro", new[] { "magic_haro_beam", "magic_haro_silence", "magic_haro_guard" } },
            { "barku", new[] { "magic_barku_blade", "magic_barku_veil", "magic_barku_shroud" } },
            { "orl", new[] { OrlSpellId } },
            { "demonlord", new[] { DemonLordSpellEternalId, DemonLordSpellSummonId, DemonLordSpellJudgementId } },
            { "hero", new[] { HeroSpellArcaneBlastId, HeroSpellRadiantBurstId } },
            { "god", new[] { GodSpellDivineStormId, GodSpellSolarJudgmentId } },
            { "hero_party_white", new[] { HeroPartyWhiteSpellId } },
            { "hero_party_tank", new[] { HeroPartyTankSpellId } },
            { "hero_party_assassin", new[] { HeroPartyAssassinSpellId } },
            { "hero_party_knight", new[] { HeroPartyKnightSpellId } },
            { "hero_party_archer", new[] { HeroPartyArcherSpellId } },
            { "hero_party_barbarian", new[] { HeroPartyBarbarianSpellId } },
            { "mentor", Array.Empty<string>() },
            { "sealed", Array.Empty<string>() },
            { "permasealed", Array.Empty<string>() }
        };
        private readonly Dictionary<Actor, Actor> _heroPartyGroupLeaders = new Dictionary<Actor, Actor>();
        private readonly HashSet<long> _demonLordSummonIds = new HashSet<long>();
        private readonly Dictionary<long, Army> _demonLordArmies = new Dictionary<long, Army>();

        private readonly List<MagicSpellDefinition> _spellDefinitions = new List<MagicSpellDefinition>
        {
            new MagicSpellDefinition(
                "magic_pyro_flare",
                ActionLibrary.castFire,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.78f,
                3,
                requiredLevel: 1,
                cooldown: 0.5f,
                rangeBonus: 0.18f,
                rangeFalloffDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 2f,
                chargeDuration: 1.1f),
            new MagicSpellDefinition(
                "magic_pyro_rain",
                ActionLibrary.castBloodRain,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                0.72f,
                6,
                requiredLevel: 3,
                cooldown: 2.5f,
                rangeBonus: 0.1f,
                rangeFalloffDistance: 6f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.6f),
            new MagicSpellDefinition(
                "magic_pyro_shield",
                ActionLibrary.castShieldOnHimself,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                4,
                requiredLevel: 2,
                cooldown: 4f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                "magic_pyro_burst",
                FireDropsSpawnAction,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.44f,
                7,
                requiredLevel: 6,
                cooldown: 3.2f,
                rangeBonus: 0.1f,
                rangeFalloffDistance: 6f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 3f,
                chargeDuration: 1.7f),
            new MagicSpellDefinition(
                "magic_aero_torrent",
                ActionLibrary.castTornado,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.75f,
                5,
                requiredLevel: 2,
                cooldown: 0.5f,
                rangeBonus: 0.2f,
                rangeFalloffDistance: 6f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 4f,
                chargeDuration: 1.3f),
            new MagicSpellDefinition(
                "magic_aero_crackle",
                ActionLibrary.castLightning,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.7f,
                6,
                requiredLevel: 3,
                cooldown: 2.1f,
                rangeBonus: 0.15f,
                rangeFalloffDistance: 7f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 3f,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                "magic_aero_shift",
                ActionLibrary.teleportRandom,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                5,
                requiredLevel: 2,
                cooldown: 4f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.4f),
            new MagicSpellDefinition(
                "magic_aero_whirl",
                ActionLibrary.castTornado,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.46f,
                7,
                requiredLevel: 5,
                cooldown: 3.1f,
                rangeBonus: 0.14f,
                rangeFalloffDistance: 7f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 4f,
                chargeDuration: 1.5f),
            new MagicSpellDefinition(
                "magic_aqua_wave",
                ActionLibrary.castSpawnGrassSeeds,
                CastTarget.Region,
                CastEntity.Tile,
                0.77f,
                5,
                requiredLevel: 1,
                cooldown: 0.5f,
                rangeBonus: 0.12f,
                rangeFalloffDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.3f),
            new MagicSpellDefinition(
                "magic_aqua_deluge",
                ActionLibrary.castSpawnFertilizer,
                CastTarget.Region,
                CastEntity.Tile,
                0.73f,
                6,
                requiredLevel: 4,
                cooldown: 3f,
                rangeBonus: 0.08f,
                rangeFalloffDistance: 7f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.7f),
            new MagicSpellDefinition(
                "magic_aqua_refresh",
                ActionLibrary.castCure,
                CastTarget.Friendly,
                CastEntity.UnitsOnly,
                1f,
                4,
                requiredLevel: 2,
                cooldown: 3.5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.1f),
            new MagicSpellDefinition(
                "magic_aqua_purity",
                ActionLibrary.castCure,
                CastTarget.Friendly,
                CastEntity.UnitsOnly,
                0.62f,
                6,
                requiredLevel: 5,
                cooldown: 3.8f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.3f),
            new MagicSpellDefinition(
                "magic_terra_rise",
                ActionLibrary.castSpawnSkeleton,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                0.76f,
                7,
                requiredLevel: 2,
                cooldown: 0.5f,
                rangeBonus: 0.1f,
                rangeFalloffDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.4f),
            new MagicSpellDefinition(
                "magic_terra_quake",
                ActionLibrary.castCurses,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.69f,
                5,
                requiredLevel: 3,
                cooldown: 2.4f,
                rangeBonus: 0.08f,
                rangeFalloffDistance: 6f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 3f,
                chargeDuration: 1.6f),
            new MagicSpellDefinition(
                "magic_terra_shockwave",
                DeathBombAction,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.42f,
                7,
                requiredLevel: 6,
                cooldown: 3.6f,
                rangeBonus: 0.08f,
                rangeFalloffDistance: 7f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 4f,
                chargeDuration: 1.8f),
            new MagicSpellDefinition(
                "magic_terra_stone",
                ActionLibrary.castShieldOnHimself,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                5,
                requiredLevel: 3,
                cooldown: 3.5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                "magic_haro_beam",
                ActionLibrary.castFire,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.79f,
                4,
                requiredLevel: 2,
                cooldown: 0.5f,
                rangeBonus: 0.13f,
                rangeFalloffDistance: 6f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.4f),
            new MagicSpellDefinition(
                "magic_haro_silence",
                ActionLibrary.castSpellSilence,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.68f,
                5,
                requiredLevel: 4,
                cooldown: 3.2f,
                rangeBonus: 0.15f,
                rangeFalloffDistance: 8f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 4f,
                chargeDuration: 1.5f),
            new MagicSpellDefinition(
                "magic_haro_guard",
                ActionLibrary.castShieldOnHimself,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                5,
                requiredLevel: 2,
                cooldown: 3.5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                "magic_barku_blade",
                ActionLibrary.castCurses,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.76f,
                6,
                requiredLevel: 2,
                cooldown: 0.5f,
                rangeBonus: 0.1f,
                rangeFalloffDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 3f,
                chargeDuration: 1.3f),
            new MagicSpellDefinition(
                "magic_barku_veil",
                ActionLibrary.castSpellSilence,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.7f,
                5,
                requiredLevel: 3,
                cooldown: 2.1f,
                rangeBonus: 0.12f,
                rangeFalloffDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: 2f,
                chargeDuration: 1.4f),
            new MagicSpellDefinition(
                "magic_barku_shroud",
                ActionLibrary.castSpawnSkeleton,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                5,
                requiredLevel: 3,
                cooldown: 3.4f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                HeroPartyWhiteSpellId,
                ActionLibrary.castShieldOnHimself,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                1f,
                6,
                requiredLevel: 1,
                cooldown: 3.5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.2f),
            new MagicSpellDefinition(
                HeroPartyTankSpellId,
                ActionLibrary.castCurses,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.85f,
                7,
                requiredLevel: 2,
                cooldown: 2.6f,
                rangeBonus: 0.12f,
                rangeFalloffDistance: 5f,
                minDistance: 3f,
                chargeDuration: 1.4f),
            new MagicSpellDefinition(
                HeroPartyAssassinSpellId,
                ActionLibrary.castLightning,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.88f,
                7,
                requiredLevel: 3,
                cooldown: 1.5f,
                rangeBonus: 0.2f,
                rangeFalloffDistance: 7f,
                minDistance: 3f,
                chargeDuration: 1.3f),
            new MagicSpellDefinition(
                HeroPartyKnightSpellId,
                ActionLibrary.castTornado,
                CastTarget.Region,
                CastEntity.Tile,
                0.9f,
                8,
                requiredLevel: 3,
                cooldown: 2.2f,
                rangeBonus: 0.18f,
                rangeFalloffDistance: 8f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 1.7f),
            new MagicSpellDefinition(
                HeroPartyArcherSpellId,
                ActionLibrary.castBloodRain,
                CastTarget.Region,
                CastEntity.Tile,
                0.92f,
                7,
                requiredLevel: 3,
                cooldown: 2.4f,
                rangeBonus: 0.25f,
                rangeFalloffDistance: 10f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 2f),
            new MagicSpellDefinition(
                HeroPartyBarbarianSpellId,
                ActionLibrary.castSpawnSkeleton,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                0.9f,
                7,
                requiredLevel: 2,
                cooldown: 3.5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1.9f),
            new MagicSpellDefinition(
                HeroSpellArcaneBlastId,
                ActionLibrary.castFire,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.9f,
                9,
                requiredLevel: 5,
                cooldown: 0.4f,
                rangeBonus: 0.25f,
                rangeFalloffDistance: 9f,
                minDistance: 3f,
                chargeDuration: 1.9f),
            new MagicSpellDefinition(
                HeroSpellRadiantBurstId,
                ActionLibrary.castBloodRain,
                CastTarget.Region,
                CastEntity.Tile,
                0.95f,
                12,
                requiredLevel: 6,
                cooldown: 1.8f,
                rangeBonus: 0.28f,
                rangeFalloffDistance: 11f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 2.1f),
            new MagicSpellDefinition(
                GodSpellDivineStormId,
                ActionLibrary.castTornado,
                CastTarget.Region,
                CastEntity.Tile,
                0.96f,
                11,
                requiredLevel: 8,
                cooldown: 1.5f,
                rangeBonus: 0.3f,
                rangeFalloffDistance: 12f,
                minDistance: 5f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                chargeDuration: 2.3f),
            new MagicSpellDefinition(
                GodSpellSolarJudgmentId,
                ActionLibrary.castLightning,
                CastTarget.Enemy,
                CastEntity.UnitsOnly,
                0.94f,
                10,
                requiredLevel: 10,
                cooldown: 1.1f,
                rangeBonus: 0.18f,
                rangeFalloffDistance: 11f,
                minDistance: 4f,
                chargeDuration: 1.6f),
            new MagicSpellDefinition(
                OrlSpellId,
                OrlPassiveSpell,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                0f,
                0,
                isAttackSpell: false,
                canBeUsedInCombat: false,
                chargeDuration: 0f),
            new MagicSpellDefinition(
                DemonLordSpellEternalId,
                DemonLordEternalSpell,
                CastTarget.Himself,
                CastEntity.UnitsOnly,
                0f,
                0,
                requiredLevel: 1,
                cooldown: 5f,
                isAttackSpell: false,
                canBeUsedInCombat: true,
                chargeDuration: 1f),
            new MagicSpellDefinition(
                DemonLordSpellSummonId,
                DemonLordSummonSpell,
                CastTarget.Region,
                CastEntity.Tile,
                0.85f,
                6,
                requiredLevel: 1,
                cooldown: 3f,
                rangeBonus: 0.15f,
                rangeFalloffDistance: 12f,
                isAttackSpell: false,
                canBeUsedInCombat: true),
            new MagicSpellDefinition(
                DemonLordSpellJudgementId,
                DemonLordJudgementSpell,
                CastTarget.Region,
                CastEntity.Tile,
                0.95f,
                1,
                requiredLevel: 200,
                cooldown: 0.5f,
                rangeBonus: 0.1f,
                rangeFalloffDistance: 10f,
                isAttackSpell: true,
                canBeUsedInCombat: true,
                minDistance: DemonLordExplosionRadius + 5f,
                chargeDuration: 2.5f)
        };

        private readonly Dictionary<string, MagicSpellDefinition> _spellDefinitionLookup = new Dictionary<string, MagicSpellDefinition>();
        private readonly List<SpellChargeRequest> _pendingSpellCharges = new List<SpellChargeRequest>();
        private List<MagicTypeDefinition> _autoAssignableTypes;
        private float _affinityMonitorTimer = AffinityMonitorInterval;
        private int _playerAffinityMetadataSuppression;
        private Harmony _harmony;
        private WorldLogAsset _orlLogAsset;
        private WorldLogAsset _heroLogBornAsset;
        private WorldLogAsset _heroLogDeathAsset;
        private WorldLogAsset _demonLogBornAsset;
        private WorldLogAsset _demonLogDeathAsset;
        private WorldLogAsset _demonLogSealAsset;
        private WorldLogAsset _demonLogUnsealAsset;
        private WorldLogAsset _godLogBornAsset;
        private WorldLogAsset _godLogDeathAsset;
        private WorldLogAsset _mentorLogBornAsset;
        private WorldLogAsset _heroPartyLogBornAsset;
        private WorldLogAsset _heroPartyLogDeathAsset;
        private bool _initialized;
        private bool _worldProcessed;
        private float _heroCheckTimer;
        private bool _isUnsealingDemonLord;
        private bool _isApplyingSealedTrait;
        private long _mentorActorId;
        private long _godActorId;
        private readonly HashSet<long> _processedDeaths = new HashSet<long>();
        private readonly List<PendingDemonLordReincarnation> _pendingDemonReincarnations = new List<PendingDemonLordReincarnation>();
        private readonly Dictionary<string, ActorTrait> _mageRankTraitLookup = new Dictionary<string, ActorTrait>(StringComparer.Ordinal);
        private readonly Dictionary<long, Vector2Int> _heroPartyFollowTargets = new Dictionary<long, Vector2Int>();
        private readonly Dictionary<long, Vector2Int> _battleZoneOffsets = new Dictionary<long, Vector2Int>();
        private Vector2Int _mentorLastHeroTile = new Vector2Int(int.MinValue, int.MinValue);
        private bool _mentorHasFollowTarget;
        private Vector2Int _battleZoneCenter = new Vector2Int(int.MinValue, int.MinValue);
        private Vector2Int _lastDemonPosition = new Vector2Int(int.MinValue, int.MinValue);
        private readonly HashSet<long> _pendingAffinityCleanupActors = new HashSet<long>();
        private bool _affinityInitialScanQueued;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
            Destroy(this);
            return;
        }

        Instance = this;
        _heroCheckTimer = 0f;
        DontDestroyOnLoad(gameObject);
            _harmony = new Harmony("xavii.magia");
            _harmony.PatchAll(typeof(MagicPatches).Assembly);
            MapBox.on_world_loaded += OnWorldLoaded;
            StartCoroutine(InitializeWhenReady());
        }

        private void OnDestroy()
        {
            MapBox.on_world_loaded -= OnWorldLoaded;
            _harmony?.UnpatchSelf();
            if (Instance == this)
                Instance = null;
        }

        private IEnumerator InitializeWhenReady()
        {
            while (AssetManager.traits == null || AssetManager.spells == null || AssetManager.world_log_library == null || AssetManager.status == null)
                yield return null;

            RegisterStatuses();
            RegisterSpells();
            RegisterTraits();
            RegisterMageRankTraits();
            RegisterWorldLog();
            _initialized = true;
            _processedDeaths.Clear();
            _pendingDemonReincarnations.Clear();
            TryProcessWorldOrls();
            ProcessExistingHeroActors();
        }

        private void OnWorldLoaded()
        {
            _worldProcessed = false;
            _affinityInitialScanQueued = false;
            _heroCheckTimer = 0f;
            _processedDeaths.Clear();
            _pendingDemonReincarnations.Clear();
            TryProcessWorldOrls();
            ProcessExistingHeroActors();
        }

        private void TryProcessWorldOrls()
        {
            if (!_initialized || _worldProcessed)
                return;

            if (World.world == null || World.world.units == null)
                return;

            ProcessExistingOrlActors();
            ProcessExistingDemonLords();
            ProcessExistingGodActors();
            QueueAffinityCleanupForWorld();
            _worldProcessed = true;
        }

        private void QueueAffinityCleanupForWorld()
        {
            if (_affinityInitialScanQueued || World.world?.units == null)
                return;

            _affinityInitialScanQueued = true;
            string noElementTraitId = GetTypeDefinition(NoElementTypeId)?.TraitId;
            var units = World.world.units.getSimpleList();
            if (units == null)
                return;

            foreach (var actor in units)
            {
                if (actor == null || actor.isRekt())
                    continue;

                if (!string.IsNullOrEmpty(noElementTraitId) && actor.hasTrait(noElementTraitId))
                    EnforceNoElementState(actor);

                var traits = actor.getTraits();
                if (traits == null)
                    continue;

                if (traits.Count(trait => trait != null && IsAffinityTrait(trait.id)) > 1)
                    ScheduleAffinityCleanup(actor);
            }
        }

        private void ScheduleAffinityCleanup(Actor actor)
        {
            if (actor?.data == null)
                return;

            if (ShouldSkipAffinityCleanup(actor))
                return;

            long actorId = actor.data.id;
            if (actorId <= 0)
                return;

            _pendingAffinityCleanupActors.Add(actorId);
        }

        private void Update()
        {
            if (!_initialized)
                return;

            ProcessSpellCharges();

            _affinityMonitorTimer -= Time.deltaTime;
            if (_affinityMonitorTimer <= 0f)
            {
                _affinityMonitorTimer = AffinityMonitorInterval;
                MonitorAffinityConflicts();
            }

            _heroCheckTimer -= Time.deltaTime;
            if (_heroCheckTimer <= 0f)
            {
                _heroCheckTimer = HeroCheckInterval;
                CheckHeroPowers();
            }

            var hero = GetHeroActor();
            var demon = GetActiveDemonLordActor();
            bool demonActive = demon != null;
            TryAutoCastSummon(demon);
            bool heroInCombat = IsActorInCombat(hero);

            UpdateBattleZone(hero, demon, heroInCombat);
            UpdateHeroDestiny(hero, demon, demonActive, heroInCombat);
            UpdateHeroPartyDestiny(hero, demon, demonActive);
            UpdateMentorBehavior(hero, demon, demonActive);
            CheckMageRanks();
            CheckSealExpiration();
            ProcessPendingDemonReincarnations();
        }

        private void MonitorAffinityConflicts()
        {
            if (_pendingAffinityCleanupActors.Count == 0)
                return;

            if (World.world?.units == null)
                return;

            var actorIds = _pendingAffinityCleanupActors.ToArray();
            _pendingAffinityCleanupActors.Clear();

            foreach (var actorId in actorIds)
            {
                var actor = GetActorById(actorId);
                if (actor == null || actor.isRekt())
                    continue;

                var traits = actor.getTraits();
                if (traits == null)
                    continue;

                var affinityTraits = traits
                    .Where(trait => trait != null && IsAffinityTrait(trait.id))
                    .ToList();

                if (affinityTraits.Count <= 1 || ShouldSkipAffinityCleanup(actor))
                    continue;

                var playerAssigned = GetPlayerAssignedAffinityIds(actor);
                RemoveUnauthorizedAffinities(actor, affinityTraits, playerAssigned);
            }
        }

        private bool ShouldSkipAffinityCleanup(Actor actor)
        {
            if (actor == null)
                return true;
            return actor.hasTrait(HeroTraitId) || actor.hasTrait(DemonLordTraitId);
        }

        private void RemoveUnauthorizedAffinities(Actor actor, List<ActorTrait> affinityTraits, HashSet<string> playerAssigned)
        {
            if (actor == null || affinityTraits == null || affinityTraits.Count <= 1)
                return;

            List<ActorTrait> toRemove;
            if (playerAssigned != null && playerAssigned.Count > 0)
            {
                toRemove = affinityTraits
                    .Where(trait => trait != null && !playerAssigned.Contains(trait.id))
                    .ToList();
            }
            else
            {
                toRemove = affinityTraits
                    .Skip(1)
                    .Where(trait => trait != null)
                    .ToList();
            }

            if (toRemove.Count == 0)
                return;

            foreach (var trait in toRemove)
                actor.removeTrait(trait);
        }

        private bool IsAffinityTrait(string traitId)
        {
            if (string.IsNullOrEmpty(traitId))
                return false;
            var definition = GetTypeDefinition(traitId);
            return definition != null && definition.AllowAutomatic;
        }

        private void ProcessExistingOrlActors()
        {
            var units = World.world?.units?.getSimpleList();
            if (units == null)
                return;

            foreach (var actor in units)
            {
                if (actor == null || actor.isRekt() || !actor.hasTrait(OrlTraitId))
                    continue;

                actor.data.get(OrlCounterKey, out int remaining, OrlReincarnationLimit);
                if (remaining <= 0)
                {
                    remaining = OrlReincarnationLimit;
                    actor.data.set(OrlCounterKey, remaining);
                }

                int usedDeaths = Math.Max(0, OrlReincarnationLimit - remaining);
                EnsureOrlBoostsReceived(actor, usedDeaths);
            }
        }

        private void ProcessExistingHeroActors()
        {
            var units = World.world?.units?.getSimpleList();
            if (units == null)
                return;

            foreach (var actor in units)
            {
                EnsureHeroState(actor);
                EnsureHeroPartyState(actor);
            }
        }

        private void ProcessExistingDemonLords()
        {
            var units = World.world?.units?.getSimpleList();
            if (units == null)
                return;

            foreach (var actor in units)
            {
                if (actor == null || actor.isRekt() || !actor.hasTrait(DemonLordTraitId))
                    continue;

                EnsureDemonLordState(actor);
            }
        }

        private void ProcessExistingGodActors()
        {
            var units = World.world?.units?.getSimpleList();
            if (units == null)
            {
                _godActorId = 0;
                return;
            }

            var god = units.FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(GodTraitId));
            _godActorId = god?.data?.id ?? 0;
            EnsureGodState(god);
        }

        private void CheckHeroPowers()
        {
            if (World.world?.units == null)
                return;

            foreach (var actor in World.world.units.getSimpleList())
            {
                HandleHeroCycle(actor);
            }
        }

        private void HandleHeroCycle(Actor actor)
        {
            if (actor == null || actor.isRekt() || !actor.hasTrait(HeroTraitId))
                return;

            int age = actor.getAge();
            if (age < 10)
            {
                actor.data.set(HeroPowerCycleKey, 0);
                return;
            }

            actor.data.get(HeroPowerCycleKey, out int applied, 0);
            if (applied < 0)
                applied = 0;

            int targetCycles = age / 10;
            if (targetCycles <= applied)
                return;

            for (int i = applied; i < targetCycles; i++)
                ScaleOrlStats(actor, 3f);

            actor.data.set(HeroPowerCycleKey, targetCycles);
            actor.setMaxHealth();
            actor.setMaxStamina();
            actor.setMaxMana();
            actor.setStatsDirty();
            actor.setHealth(actor.getMaxHealth());
            actor.setStamina(actor.getMaxStamina());
            actor.setMana(actor.getMaxMana());
        }

        private void EnsureHeroState(Actor hero)
        {
            if (hero == null || hero.isRekt() || !hero.hasTrait(HeroTraitId))
                return;

            GrantAllElementalAffinities(hero);
            AssignMentorToHero(hero);
            HandleHeroCycle(hero);
            ApplyChampionStats(hero, ChampionTier.Hero);
        }

        private void EnsureDemonLordState(Actor demon)
        {
            if (demon == null || demon.isRekt() || !demon.hasTrait(DemonLordTraitId))
                return;

            GrantAllElementalAffinities(demon);
            TryTriggerDemonLordRebellion(demon);
            if (!demon.hasTrait(SealedTraitId))
                demon.data.set(DemonLordSealTimestampKey, -1);
            
            if (!demon.hasTrait(PermaSealedTraitId))
                demon.data.set(DemonLordSealTimestampKey, -1);
            ApplyChampionStats(demon, ChampionTier.Legendary);
            ApplyGodTimePowerScaling(demon);
        }

        private void TryTriggerDemonLordRebellion(Actor demon)
        {
            if (demon == null || demon.isRekt() || !demon.hasTrait(DemonLordTraitId))
                return;

            demon.data.get(DemonLordRebellionFlagKey, out int rebelFlag, 0);
            if (rebelFlag > 0)
                return;

            if (demon.getAge() < DemonLordRebellionMinAge || !demon.hasKingdom() || demon.isKing())
                return;

            City city = demon.city ?? demon.current_tile?.zone_city;
            if (city == null || city.kingdom == null || city.kingdom.isRekt())
                return;

            Kingdom homeKingdom = city.kingdom;
            if (!homeKingdom.isCiv() || demon.kingdom != homeKingdom)
                return;

            Kingdom rebelKingdom = city.makeOwnKingdom(demon, true);
            if (rebelKingdom == null)
                return;

            demon.data.set(DemonLordRebellionFlagKey, 1);
            if (World.world?.diplomacy == null)
                return;

            War war = World.world.diplomacy.startWar(homeKingdom, rebelKingdom, WarTypeLibrary.rebellion);
            if (war == null || !homeKingdom.hasAlliance())
                return;

            foreach (Kingdom ally in homeKingdom.getAlliance().kingdoms_hashset)
            {
                if (ally != homeKingdom && ally.isOpinionTowardsKingdomGood(homeKingdom))
                    war.joinAttackers(ally);
            }

            TryAutoCastSummon(demon, force: true);
        }

        private void TryAutoCastSummon(Actor demon, bool force = false)
        {
            if (demon == null || demon.isRekt() || demon.hasTrait(SealedTraitId) || demon.hasTrait(PermaSealedTraitId))
                return;

            if (!_spellDefinitionLookup.TryGetValue(DemonLordSpellSummonId, out var definition))
                return;

            if (!force && !IsSummonReady(demon))
                return;

            var spellAsset = AssetManager.spells.get(DemonLordSpellSummonId);
            if (spellAsset == null)
                return;

            var request = new SpellChargeRequest(demon, spellAsset, demon, definition, 0f);
            if (!TryExecuteSpell(request))
                return;

            demon.data.set(DemonLordSummonTimestampKey, Date.getCurrentYear());
        }

        private bool IsSummonReady(Actor demon)
        {
            if (demon == null)
                return false;

            demon.data.get(DemonLordSummonTimestampKey, out int lastYear, int.MinValue);
            return Date.getCurrentYear() - lastYear >= DemonLordSummonCooldownYears;
        }

        private void GrantAllElementalAffinities(Actor actor)
        {
            if (actor == null)
                return;

            foreach (var type in _typeDefinitions.Where(def => def.AllowAutomatic && def.Id != "none"))
            {
                if (actor.hasTrait(type.TraitId))
                    continue;
                AddTraitWithoutTracking(actor, type.TraitId);
            }
        }

        private static Actor GetActorById(long id)
        {
            if (id <= 0 || World.world?.units == null)
                return null;
            return World.world.units.get(id);
        }

        private Actor GetMentorActor()
        {
            if (_mentorActorId > 0)
            {
                var existing = GetActorById(_mentorActorId);
                if (existing != null && !existing.isRekt() && existing.hasTrait(MentorTraitId))
                    return existing;
                _mentorActorId = 0;
            }

            if (World.world?.units == null)
                return null;

            var mentor = World.world.units.getSimpleList()
                .FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(MentorTraitId));
            if (mentor != null)
                _mentorActorId = mentor.data.id;
            return mentor;
        }

        private Actor GetGodActor()
        {
            if (_godActorId > 0)
            {
                var existing = GetActorById(_godActorId);
                if (existing != null && !existing.isRekt() && existing.hasTrait(GodTraitId))
                    return existing;
                _godActorId = 0;
            }

            if (World.world?.units == null)
                return null;

            var god = World.world.units.getSimpleList()
                .FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(GodTraitId));
            if (god != null)
                _godActorId = god.data.id;
            return god;
        }

        private Actor GetHeroForMentor(Actor mentor)
        {
            if (mentor == null)
                return null;

            mentor.data.get(MentorHeroKey, out long heroId);
            var hero = GetActorById(heroId);
            if (hero != null && !hero.isRekt() && hero.hasTrait(HeroTraitId))
                return hero;

            if (World.world?.units == null)
                return null;

            hero = World.world.units.getSimpleList()
                .FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(HeroTraitId));
            if (hero != null)
                AssignMentorToHero(hero);
            return hero;
        }

        private void UpdateMentorBehavior(Actor hero, Actor demon, bool demonActive)
        {
            var mentor = GetMentorActor();
            if (mentor == null || mentor.isRekt() || !mentor.hasTrait(MentorTraitId))
            {
                _mentorActorId = 0;
                ResetMentorFollowTarget();
                return;
            }

            var heroForMentor = GetHeroForMentor(mentor) ?? hero;
            if (heroForMentor == null || heroForMentor.isRekt())
            {
                mentor.data.set(MentorHeroKey, 0L);
                ResetMentorFollowTarget();
                return;
            }

            hero = heroForMentor;

            EnsureMentorLinked(hero, mentor);
            if (!demonActive)
            {
                ResetMentorFollowTarget();
                TransitionDestinyState(mentor, DestinyState.NormalLife);
                return;
            }

            if (mentor.current_tile == null)
                return;

            var state = GetDestinyState(mentor);
            float timer = GetDestinyTimer(mentor);
            bool heroFollowClose = hero.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, mentor.current_tile) <= MentorFollowDistanceSq;
            bool heroRespondClose = hero.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, mentor.current_tile) <= MentorRespondDistanceSq;
            bool heroNearDemon = hero.current_tile != null && demon?.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, demon.current_tile) <= HeroRespondDistanceSq;
            bool mentorNearDemon = mentor.current_tile != null && demon?.current_tile != null &&
                Toolbox.SquaredDistTile(mentor.current_tile, demon.current_tile) <= HeroRespondDistanceSq;
            bool heroInCombat = IsActorInCombat(hero);

            switch (state)
            {
                case DestinyState.NormalLife:
                    TransitionDestinyState(mentor, DestinyState.AwareOfThreat);
                    break;
                case DestinyState.AwareOfThreat:
                    timer += Time.deltaTime;
                    SetDestinyTimer(mentor, timer);
                    if (timer >= MentorPreparingDelay)
                    {
                        TransitionDestinyState(mentor, DestinyState.Preparing);
                        SendMentorToSacredLocation(mentor);
                    }
                    break;
                case DestinyState.Preparing:
                    timer += Time.deltaTime;
                    SetDestinyTimer(mentor, timer);
                    if (heroNearDemon || mentorNearDemon)
                    {
                        TransitionDestinyState(mentor, DestinyState.Responding);
                        break;
                    }

                    if (heroRespondClose && heroFollowClose && Randy.randomFloat(0f, 1f) <= 0.35f)
                    {
                        QueueMentorMovement(mentor, hero);
                    }

                    if (timer >= MentorPreparingDelay)
                    {
                        SendMentorToSacredLocation(mentor);
                        ResetDestinyTimer(mentor);
                    }
                    break;
                case DestinyState.Responding:
                    if (heroNearDemon || mentorNearDemon || heroRespondClose)
                    {
                        QueueMentorMovement(mentor, hero);
                    }
                    else if (demon?.current_tile != null)
                    {
                        mentor.goTo(demon.current_tile);
                    }

                    if (heroInCombat)
                    {
                        TransitionDestinyState(mentor, DestinyState.Fighting);
                    }
                    break;
                case DestinyState.Fighting:
                    QueueMentorMovement(mentor, hero);
                    if (!heroInCombat)
                    {
                        TransitionDestinyState(mentor, DestinyState.Responding);
                    }
                    break;
            }
        }

        private void UpdateHeroPartyDestiny(Actor hero, Actor demon, bool demonActive)
        {
            if (World.world?.units == null)
                return;

            foreach (var member in World.world.units.getSimpleList())
            {
                if (member == null || member.isRekt() || !IsHeroPartyTrait(member))
                    continue;

                if (!member.hasHealth() || member.getHealth() <= 0f)
                {
                    member.dieSimpleNone();
                    ClearHeroPartyTarget(member);
                    continue;
                }

                UpdateHeroPartyMemberDestiny(member, hero, demon, demonActive);
            }
        }

        private void UpdateHeroPartyMemberDestiny(Actor member, Actor hero, Actor demon, bool demonActive)
        {
            if (member == null || member.current_tile == null)
                return;

            if (hero != null && hero.data != null)
                member.data.set(HeroPartyHeroKey, hero.data.id);

            if (!demonActive)
            {
                ResetHeroPartyMemberState(member);
                return;
            }

            var state = GetDestinyState(member);
            float timer = GetDestinyTimer(member);
            bool shouldRespond = ShouldHeroPartyRespond(member, hero, demon, state);
            bool combatReady = IsHeroPartyCombatReady(member, hero);

            switch (state)
            {
                case DestinyState.NormalLife:
                    timer += Time.deltaTime;
                    SetDestinyTimer(member, timer);
                    if (timer >= GetDecisionDelay(member) && Randy.randomFloat(0f, 1f) <= HeroPartyAwarenessChance)
                    {
                        TransitionDestinyState(member, DestinyState.AwareOfThreat);
                    }
                    else if (timer >= GetDecisionDelay(member))
                    {
                        ResetDestinyTimer(member);
                    }
                    break;
                case DestinyState.AwareOfThreat:
                    timer += Time.deltaTime;
                    SetDestinyTimer(member, timer);
                    if (timer >= GetDecisionDelay(member))
                    {
                        TransitionDestinyState(member, DestinyState.Preparing);
                        TrySendHeroPartyToSacredLocation(member, hero);
                    }
                    break;
                case DestinyState.Preparing:
                    timer += Time.deltaTime;
                    SetDestinyTimer(member, timer);
                    if (timer >= GetDecisionDelay(member))
                    {
                        if (shouldRespond)
                        {
                            TransitionDestinyState(member, DestinyState.Responding);
                            break;
                        }

                        TrySendHeroPartyToSacredLocation(member, hero);
                        ResetDestinyTimer(member);
                    }
                    break;
                case DestinyState.Responding:
                    if (combatReady)
                    {
                        TransitionDestinyState(member, DestinyState.Fighting);
                        break;
                    }

                    SendHeroPartyToThreat(member, hero, demon);
                    EnsureHeroPartyGroupLeader(member, hero);
                    break;
                case DestinyState.Fighting:
                    SyncHeroPartyCombat(member, hero);
                    EnsureHeroPartyGroupLeader(member, hero);
                    SendHeroPartyToThreat(member, hero, demon);
                    if (!combatReady)
                    {
                        TransitionDestinyState(member, DestinyState.Responding);
                        AssignHeroPartyLeader(member, null);
                    }
                    break;
            }

            UpdateHeroPartyBond(member, hero);
        }

        private void UpdateHeroPartyBond(Actor member, Actor hero)
        {
            if (member == null || hero == null)
                return;

            bool heroCombat = IsActorInCombat(hero);
            bool memberCombat = IsActorInCombat(member);
            bool nearHero = hero.current_tile != null && member.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, member.current_tile) <= HeroPartyCombatAssistDistanceSq;

            float bondTime = GetHeroBondTime(member);
            if (heroCombat && memberCombat && nearHero)
            {
                bondTime = Mathf.Min(HeroBondLevelTwoTime, bondTime + Time.deltaTime);
            }
            else
            {
                bondTime = Mathf.Max(0f, bondTime - HeroBondDecayRate * Time.deltaTime);
            }

            SetHeroBondTime(member, bondTime);
            int targetLevel = CalculateHeroBondLevel(bondTime);
            ApplyHeroBondLevel(member, targetLevel);
        }

        private int CalculateHeroBondLevel(float bondTime)
        {
            if (bondTime >= HeroBondLevelTwoTime)
                return 2;
            if (bondTime >= HeroBondLevelOneTime)
                return 1;
            return 0;
        }

        private float GetHeroBondTime(Actor actor)
        {
            if (actor?.data == null)
                return 0f;

            actor.data.get(HeroBondTimeKey, out float value);
            return value;
        }

        private void SetHeroBondTime(Actor actor, float bondTime)
        {
            if (actor?.data == null)
                return;

            actor.data.set(HeroBondTimeKey, Mathf.Max(0f, bondTime));
        }

        private int GetHeroBondLevel(Actor actor)
        {
            if (actor?.data == null)
                return 0;

            actor.data.get(HeroBondLevelKey, out int value);
            return value;
        }

        private void SetHeroBondLevel(Actor actor, int level)
        {
            if (actor?.data == null)
                return;

            actor.data.set(HeroBondLevelKey, Mathf.Max(0, level));
        }

        private void ApplyHeroBondLevel(Actor actor, int level)
        {
            if (actor == null || actor.data == null)
                return;

            int currentLevel = GetHeroBondLevel(actor);
            if (currentLevel == level)
                return;

            int delta = level - currentLevel;
            actor.stats["damage"] += HeroBondDamagePerLevel * delta;
            actor.stats["health"] += HeroBondHealthPerLevel * delta;
            actor.stats["critical_chance"] += HeroBondCriticalPerLevel * delta;
            bool hadCombo = currentLevel >= 2;
            bool hasCombo = level >= 2;
            if (hasCombo != hadCombo)
                actor.stats["attack_speed"] += HeroBondComboAttackSpeed * (hasCombo ? 1 : -1);
            actor.setStatsDirty();
            actor.setMaxHealth();
            actor.setMaxStamina();
            actor.setMaxMana();

            SetHeroBondLevel(actor, level);
        }

        private void ResetHeroPartyMemberState(Actor member)
        {
            if (member == null)
                return;

            TransitionDestinyState(member, DestinyState.NormalLife);
            ClearHeroPartyTarget(member);
            var heroPartyLeader = GetHeroPartyLeader(member);
            if (heroPartyLeader != null && heroPartyLeader.hasTrait(HeroTraitId))
                AssignHeroPartyLeader(member, null);
            SetHeroPartySacredFlag(member, true);
            SetHeroBondTime(member, 0f);
            ApplyHeroBondLevel(member, 0);
        }

        private void SendHeroPartyToSacredLocation(Actor member, Actor hero)
        {
            if (member == null || HasHeroPartyUsedSacred(member))
                return;

            var anchor = hero?.current_tile ?? member.current_tile;
            if (anchor == null)
                return;

            WorldTile destination = null;
            for (int i = 0; i < 3; i++)
            {
                destination = Toolbox.getRandomTileWithinDistance(anchor, 45);
                if (destination != null)
                    break;
            }

            if (destination != null)
            {
                member.goTo(destination);
                SetHeroPartySacredFlag(member, true);
            }
        }

        private void TrySendHeroPartyToSacredLocation(Actor member, Actor hero)
        {
            if (member == null || HasHeroPartyUsedSacred(member))
                return;

            SendHeroPartyToSacredLocation(member, hero);
        }

        private bool HasHeroPartyUsedSacred(Actor actor)
        {
            if (actor?.data == null)
                return true;

            actor.data.get(HeroPartySacredFlagKey, out int flag, 1);
            return flag != 0;
        }

        private void SetHeroPartySacredFlag(Actor actor, bool value)
        {
            if (actor?.data == null)
                return;

            actor.data.set(HeroPartySacredFlagKey, value ? 1 : 0);
        }

        private void SendHeroPartyToThreat(Actor member, Actor hero, Actor demon)
        {
            var target = DeterminePartyTarget(member, hero, demon);
            if (target == null)
                return;

            QueueHeroPartyMovement(member, target);
        }

        private void EnsureHeroPartyGroupLeader(Actor member, Actor hero)
        {
            if (member == null)
                return;

            if (hero != null && hero.current_tile != null && member.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, member.current_tile) <= HeroPartyCombatAssistDistanceSq)
            {
                AssignHeroPartyLeader(member, hero);
                return;
            }

            var heroPartyLeader = GetHeroPartyLeader(member);
            if (heroPartyLeader != null && heroPartyLeader == hero)
                AssignHeroPartyLeader(member, null);
        }

        private Actor GetHeroPartyLeader(Actor member)
        {
            if (member == null)
                return null;

            if (!_heroPartyGroupLeaders.TryGetValue(member, out var leader))
                return null;

            if (leader == null || leader.isRekt())
            {
                _heroPartyGroupLeaders.Remove(member);
                return null;
            }

            return leader;
        }

        private void AssignHeroPartyLeader(Actor member, Actor leader)
        {
            if (member == null)
                return;

            if (leader == null)
            {
                _heroPartyGroupLeaders.Remove(member);
                return;
            }

            _heroPartyGroupLeaders[member] = leader;
        }

        private WorldTile DeterminePartyTarget(Actor member, Actor hero, Actor demon)
        {
            if (member == null)
                return null;

            WorldTile heroTile = hero?.current_tile;
            WorldTile memberTile = member.current_tile;
            if (heroTile != null && memberTile != null)
            {
                float distSq = Toolbox.SquaredDistTile(heroTile, memberTile);
                if (distSq > HeroPartyTetherFarSq)
                    return heroTile;
                if (distSq <= HeroPartyTetherNearSq)
                    return GetBattleZoneTarget(member) ?? heroTile;
            }

            return GetBattleZoneTarget(member) ?? heroTile ?? memberTile;
        }

        private WorldTile GetBattleZoneTarget(Actor member)
        {
            if (World.world == null || _battleZoneCenter.x == int.MinValue)
                return null;

            Vector2Int offset = GetBattleOffset(member);
            Vector2Int targetPos = _battleZoneCenter + offset;
            WorldTile tile = Toolbox.getTileAt(targetPos.x, targetPos.y);
            if (tile == null)
                tile = Toolbox.getTileAt(_battleZoneCenter.x, _battleZoneCenter.y);
            return tile;
        }

        private Vector2Int GetBattleOffset(Actor member)
        {
            if (member == null || member.data == null)
                return Vector2Int.zero;

            if (_battleZoneOffsets.TryGetValue(member.data.id, out var offset))
                return offset;

            int radius = Mathf.Max(1, Mathf.RoundToInt(HeroPartyBattleOffsetRadius));
            offset = new Vector2Int(Randy.randomInt(-radius, radius), Randy.randomInt(-radius, radius));
            _battleZoneOffsets[member.data.id] = offset;
            return offset;
        }

        private void UpdateBattleZone(Actor hero, Actor demon, bool heroInCombat)
        {
            if (demon?.current_tile != null)
            {
                _lastDemonPosition = demon.current_tile.pos;
                if (heroInCombat && hero?.current_tile != null)
                {
                    _battleZoneCenter = GetBattleZoneCentroid(hero.current_tile.pos, demon.current_tile.pos);
                    return;
                }

                _battleZoneCenter = demon.current_tile.pos;
                return;
            }

            if (hero?.current_tile != null)
            {
                _battleZoneCenter = hero.current_tile.pos;
                return;
            }

            if (_battleZoneCenter.x == int.MinValue && _lastDemonPosition.x != int.MinValue)
                _battleZoneCenter = _lastDemonPosition;
        }

        private Vector2Int GetBattleZoneCentroid(Vector2Int heroPos, Vector2Int demonPos)
        {
            return new Vector2Int((heroPos.x + demonPos.x) / 2, (heroPos.y + demonPos.y) / 2);
        }

        private bool ShouldHeroPartyRespond(Actor member, Actor hero, Actor demon, DestinyState state)
        {
            if (member?.current_tile == null)
                return false;

            bool useExitThreshold = state >= DestinyState.Responding;
            if (IsHeroPartyHeroNear(member, hero, useExitThreshold) || IsHeroPartyDemonNear(member, demon, useExitThreshold))
                return true;

            if (hero != null && IsActorInCombat(hero) && hero.current_tile != null && member.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, member.current_tile) <= HeroPartyCombatAssistDistanceSq)
                return true;

            return false;
        }

        private bool IsHeroPartyHeroNear(Actor member, Actor hero, bool useExitThreshold)
        {
            if (member?.current_tile == null || hero?.current_tile == null)
                return false;

            float distSq = Toolbox.SquaredDistTile(hero.current_tile, member.current_tile);
            return useExitThreshold ? distSq <= HeroPartyNearHeroExitSq : distSq <= HeroPartyNearHeroEnterSq;
        }

        private bool IsHeroPartyDemonNear(Actor member, Actor demon, bool useExitThreshold)
        {
            if (member?.current_tile == null || demon?.current_tile == null)
                return false;

            float distSq = Toolbox.SquaredDistTile(member.current_tile, demon.current_tile);
            return useExitThreshold ? distSq <= HeroPartyNearDemonExitSq : distSq <= HeroPartyNearDemonEnterSq;
        }

        private bool IsHeroPartyCombatReady(Actor member, Actor hero)
        {
            if (member == null)
                return false;

            if (IsActorInCombat(member))
                return true;

            if (hero != null && hero.current_tile != null && member.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, member.current_tile) <= HeroPartyCombatAssistDistanceSq &&
                IsActorInCombat(hero))
            {
                return true;
            }

            return false;
        }

        private void UpdateHeroDestiny(Actor hero, Actor demon, bool demonActive, bool heroInCombat)
        {
            if (hero == null || hero.isRekt())
                return;

            if (!demonActive)
            {
                TransitionDestinyState(hero, DestinyState.NormalLife);
                return;
            }

            float timer = GetDestinyTimer(hero);
            var state = GetDestinyState(hero);
            bool nearDemon = hero.current_tile != null && demon?.current_tile != null &&
                Toolbox.SquaredDistTile(hero.current_tile, demon.current_tile) <= HeroRespondDistanceSq;
            bool hasPower = GetActorPower(hero) >= HeroRespondPowerThreshold;
            float heroDemonDistSq = hero.current_tile != null && demon?.current_tile != null
                ? Toolbox.SquaredDistTile(hero.current_tile, demon.current_tile)
                : float.MaxValue;

            switch (state)
            {
                case DestinyState.NormalLife:
                    TransitionDestinyState(hero, DestinyState.AwareOfThreat);
                    break;
                case DestinyState.AwareOfThreat:
                    timer += Time.deltaTime;
                    SetDestinyTimer(hero, timer);
                    if (timer >= GetDecisionDelay(hero) || hasPower || nearDemon)
                        TransitionDestinyState(hero, DestinyState.Preparing);
                    break;
                case DestinyState.Preparing:
                    timer += Time.deltaTime;
                    SetDestinyTimer(hero, timer);
                    if (timer >= GetDecisionDelay(hero))
                    {
                        if (nearDemon || heroInCombat || hasPower)
                        {
                            TransitionDestinyState(hero, DestinyState.Responding);
                        }
                        else
                        {
                            ResetDestinyTimer(hero);
                        }
                    }
                    break;
                case DestinyState.Responding:
                    if ((heroDemonDistSq <= HeroFightingEnterDistanceSq || heroInCombat) && heroDemonDistSq <= HeroRespondDistanceSq)
                    {
                        TransitionDestinyState(hero, DestinyState.Fighting);
                    }
                    break;
                case DestinyState.Fighting:
                    if (heroDemonDistSq >= HeroFightingExitDistanceSq && !heroInCombat)
                        TransitionDestinyState(hero, DestinyState.Responding);
                    break;
            }
        }

        private void TransitionDestinyState(Actor actor, DestinyState newState)
        {
            if (actor == null)
                return;

            SetDestinyState(actor, newState);
            ResetDestinyTimer(actor);
            ConfigureDecisionWindow(actor, newState);

            if (IsHeroPartyTrait(actor))
            {
                if (newState == DestinyState.Preparing)
                    SetHeroPartySacredFlag(actor, false);
                if (newState == DestinyState.NormalLife)
                    SetHeroPartySacredFlag(actor, true);
            }
        }

        private void ConfigureDecisionWindow(Actor actor, DestinyState state)
        {
            if (actor == null || actor.data == null)
                return;

            float delay = IsHeroPartyTrait(actor)
                ? GetHeroPartyDecisionWindow(state)
                : GetHeroDecisionWindow(state);

            SetDecisionDelay(actor, delay);
        }

        private float GetDecisionDelay(Actor actor)
        {
            if (actor?.data == null)
                return 0f;

            string key = IsHeroPartyTrait(actor) ? HeroPartyDecisionDelayKey : DestinyDecisionDelayKey;
            actor.data.get(key, out float delay);
            if (delay <= 0f)
            {
                ConfigureDecisionWindow(actor, GetDestinyState(actor));
                actor.data.get(key, out delay);
            }

            return Mathf.Max(0.01f, delay);
        }

        private void SetDecisionDelay(Actor actor, float delay)
        {
            if (actor?.data == null)
                return;

            string key = IsHeroPartyTrait(actor) ? HeroPartyDecisionDelayKey : DestinyDecisionDelayKey;
            actor.data.set(key, Mathf.Max(0f, delay));
        }

        private float GetHeroPartyDecisionWindow(DestinyState state)
        {
            switch (state)
            {
                case DestinyState.Preparing:
                    return Randy.randomFloat(HeroPartyPreparingWindowMin, HeroPartyPreparingWindowMax);
                case DestinyState.Responding:
                case DestinyState.Fighting:
                    return Randy.randomFloat(HeroPartyRespondingWindowMin, HeroPartyRespondingWindowMax);
                case DestinyState.AwareOfThreat:
                case DestinyState.NormalLife:
                default:
                    return Randy.randomFloat(HeroPartyAwarenessWindowMin, HeroPartyAwarenessWindowMax);
            }
        }

        private float GetHeroDecisionWindow(DestinyState state)
        {
            return Randy.randomFloat(HeroStateDecisionWindowMin, HeroStateDecisionWindowMax);
        }

        private DestinyState GetDestinyState(Actor actor)
        {
            if (actor?.data == null)
                return DestinyState.NormalLife;

            actor.data.get(DestinyStateKey, out int raw, (int)DestinyState.NormalLife);
            if (Enum.IsDefined(typeof(DestinyState), raw))
                return (DestinyState)raw;

            return DestinyState.NormalLife;
        }

        private void SetDestinyState(Actor actor, DestinyState state)
        {
            if (actor?.data == null)
                return;

            actor.data.set(DestinyStateKey, (int)state);
        }

        private float GetDestinyTimer(Actor actor)
        {
            if (actor?.data == null)
                return 0f;

            actor.data.get(DestinyTimerKey, out float value);
            return value;
        }

        private void SetDestinyTimer(Actor actor, float value)
        {
            if (actor?.data == null)
                return;

            actor.data.set(DestinyTimerKey, value);
        }

        private void ResetDestinyTimer(Actor actor) => SetDestinyTimer(actor, 0f);

        public static bool IsActorInCombat(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return false;

            if (actor.attack_target?.isActor() == true)
                return true;

            if (actor.attackedBy?.isActor() == true)
                return true;

            return false;
        }

        private void CheckMageRanks()
        {
            if (World.world?.units == null)
                return;

            foreach (var actor in World.world.units.getSimpleList())
            {
                if (actor == null || actor.isRekt())
                    continue;
                EnsureMageRank(actor);
            }
        }

        private void AssignHeroPartyToHero(Actor follower)
        {
            if (follower == null)
                return;

            var hero = GetHeroActor();
            follower.data.set(HeroPartyHeroKey, hero?.data.id ?? 0L);
        }

        private Actor GetHeroActor()
        {
            if (World.world?.units == null)
                return null;

            return World.world.units.getSimpleList()
                .FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(HeroTraitId));
        }

        private Actor GetHeroForHeroParty(Actor follower)
        {
            if (follower == null)
                return null;

            follower.data.get(HeroPartyHeroKey, out long heroId);
            var hero = GetActorById(heroId);
            if (hero != null && !hero.isRekt() && hero.hasTrait(HeroTraitId))
                return hero;

            return GetHeroActor();
        }

        private void SyncHeroPartyCombat(Actor follower, Actor hero)
        {
            if (follower == null || hero == null)
                return;

            BaseSimObject target = hero.attack_target;
            if (target == null && hero.attackedBy?.isActor() == true)
                target = hero.attackedBy;

            if (target == null || !target.isActor())
                return;

            var targetActor = target.a;
            if (targetActor == null || targetActor.isRekt())
                return;

            if (follower.attack_target == targetActor)
                return;

            follower.setAttackTarget(targetActor);
        }

        private void QueueHeroPartyMovement(Actor follower, WorldTile targetTile)
        {
            if (follower == null || targetTile == null || follower.data == null || follower.current_tile == null)
                return;

            if (follower.current_tile.region == null || targetTile.region == null)
                return;

            Vector2Int targetPosition = targetTile.pos;
            if (_heroPartyFollowTargets.TryGetValue(follower.data.id, out var lastPosition) && lastPosition == targetPosition)
                return;

            _heroPartyFollowTargets[follower.data.id] = targetPosition;
            follower.goTo(targetTile);
        }

        private void ClearHeroPartyTarget(Actor follower)
        {
            if (follower?.data == null)
                return;

            _heroPartyFollowTargets.Remove(follower.data.id);
        }

        private void QueueMentorMovement(Actor mentor, Actor hero)
        {
            if (mentor == null || hero == null || hero.current_tile == null)
                return;

            Vector2Int heroPosition = hero.current_tile.pos;
            if (_mentorHasFollowTarget && _mentorLastHeroTile == heroPosition)
                return;

            _mentorHasFollowTarget = true;
            _mentorLastHeroTile = heroPosition;
            mentor.goTo(hero.current_tile);
        }

        private void SendMentorToSacredLocation(Actor mentor)
        {
            if (mentor == null || mentor.current_tile == null)
                return;

            WorldTile destination = null;
            for (int i = 0; i < 3; i++)
            {
                destination = Toolbox.getRandomTileWithinDistance(mentor.current_tile, 60);
                if (destination != null)
                    break;
            }

            if (destination != null)
                mentor.goTo(destination);
        }

        private void ResetMentorFollowTarget()
        {
            _mentorHasFollowTarget = false;
            _mentorLastHeroTile = new Vector2Int(int.MinValue, int.MinValue);
        }

        private Actor GetActiveDemonLordActor()
        {
            if (World.world?.units == null)
                return null;

            return World.world.units.getSimpleList()
                .FirstOrDefault(actor => actor != null && !actor.isRekt() && actor.hasTrait(DemonLordTraitId) && !actor.hasTrait(SealedTraitId) && !actor.hasTrait(PermaSealedTraitId));
        }

        private bool IsDemonLordActive()
        {
            return GetActiveDemonLordActor() != null;
        }

        private void EnsureMageRank(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return;

            if (!HasMagicTrait(actor))
            {
                RemoveMageRanks(actor);
                return;
            }

            var targetRank = GetDesiredMageRank(actor.data.level, actor.data.kills);
            if (targetRank == null)
            {
                RemoveMageRanks(actor);
                return;
            }

            foreach (var rank in MageRankDefinitions)
            {
                if (rank.TraitId == targetRank.TraitId)
                {
                    if (!actor.hasTrait(rank.TraitId) && _mageRankTraitLookup.TryGetValue(rank.TraitId, out var trait))
                        AddTraitWithoutTracking(actor, trait);
                }
                else if (actor.hasTrait(rank.TraitId) && _mageRankTraitLookup.TryGetValue(rank.TraitId, out var removalTrait))
                {
                    actor.removeTrait(removalTrait);
                }
            }
        }

        private void RemoveMageRanks(Actor actor)
        {
            if (actor == null)
                return;

            foreach (var rank in MageRankDefinitions)
            {
                if (actor.hasTrait(rank.TraitId) && _mageRankTraitLookup.TryGetValue(rank.TraitId, out var trait))
                    actor.removeTrait(trait);
            }
        }

        private MageRankDefinition GetDesiredMageRank(int level, int kills)
        {
            for (int i = MageRankDefinitions.Count - 1; i >= 0; i--)
            {
                var rank = MageRankDefinitions[i];
                if (rank != null && level >= rank.MinLevel && kills >= rank.MinKills)
                    return rank;
            }

            return null;
        }

        private bool IsHeroPartyTrait(Actor actor)
        {
            return actor != null && actor.getTraits().Any(trait => trait != null && IsHeroPartyTrait(trait.id));
        }

        private bool IsHeroPartyTrait(string traitId)
        {
            return !string.IsNullOrEmpty(traitId) && HeroPartyTraitSet.Contains(traitId);
        }

        private bool IsHeroPartyTraitActive(string traitId)
        {
            if (string.IsNullOrEmpty(traitId) || World.world?.units == null)
                return false;

            return World.world.units.getSimpleList()
                .Any(actor => actor != null && !actor.isRekt() && actor.hasTrait(traitId));
        }

        private bool IsHeroPartyTraitBlocked(Actor hero, string traitId)
        {
            if (hero == null || hero.data == null || string.IsNullOrEmpty(traitId))
                return false;

            hero.data.get(GetHeroPartyBlockKey(traitId), out int value, 0);
            return value > 0;
        }

        private void SetHeroPartyTraitBlock(Actor hero, string traitId, bool blocked)
        {
            if (hero == null || hero.data == null || string.IsNullOrEmpty(traitId))
                return;

            string key = GetHeroPartyBlockKey(traitId);
            if (blocked)
                hero.data.set(key, 1);
            else
                hero.data.removeString(key);
        }

        private string GetHeroPartyBlockKey(string traitId) => $"{HeroPartyBlockKeyPrefix}{traitId}";

        private void ClearHeroPartyBlocks(Actor hero)
        {
            if (hero?.data == null)
                return;

            foreach (var traitId in HeroPartyTraitIds)
            {
                hero.data.removeString(GetHeroPartyBlockKey(traitId));
            }
        }

        private void BlockHeroPartyTraitForHero(string traitId)
        {
            if (string.IsNullOrEmpty(traitId))
                return;

            var hero = GetHeroActor();
            if (hero == null || hero.isRekt())
                return;

            SetHeroPartyTraitBlock(hero, traitId, true);
        }

        private void RemoveHeroPartyTraitsFromWorld()
        {
            var units = World.world?.units?.getSimpleList();
            if (units == null)
                return;

            foreach (var actor in units)
            {
                if (actor == null)
                    continue;

                foreach (var traitId in HeroPartyTraitIds)
                {
                    if (!actor.hasTrait(traitId))
                        continue;

                    var traitAsset = AssetManager.traits?.get(traitId);
                    if (traitAsset != null)
                        actor.removeTrait(traitAsset);
                }
            }
        }

        private void EnsureMentorLinked(Actor hero, Actor mentor)
        {
            if (hero == null || mentor == null)
                return;

            hero.data.set(HeroMentorKey, mentor.data.id);
            mentor.data.set(MentorHeroKey, hero.data.id);
        }

        private bool IsMentorNearby(Actor hero)
        {
            if (hero == null || hero.isRekt() || !hero.hasTrait(HeroTraitId))
                return false;

            hero.data.get(HeroMentorKey, out long mentorId, 0L);
            var mentor = GetActorById(mentorId) ?? GetMentorActor();
            if (mentor == null || mentor.isRekt() || !mentor.hasTrait(MentorTraitId))
                return false;

            if (mentor.hasStatusStunned() || mentor.hasStatusTantrum() || mentor.is_unconscious || IsActorInCombat(mentor))
                return false;

            if (hero.current_tile == null || mentor.current_tile == null)
                return false;

            return Toolbox.SquaredDistTile(hero.current_tile, mentor.current_tile) <= MentorSealDistanceSq;
        }

        private void AssignMentorToHero(Actor hero)
        {
            if (hero == null || hero.isRekt())
                return;

            var mentor = GetMentorActor();
            if (mentor == null || mentor.isRekt())
            {
                hero.data.set(HeroMentorKey, 0L);
                return;
            }

            EnsureMentorLinked(hero, mentor);
        }

        private void SetMentor(Actor mentor)
        {
            if (mentor == null || mentor.isRekt())
                return;

            if (_mentorActorId > 0 && _mentorActorId != mentor.data.id)
            {
                var previous = GetActorById(_mentorActorId);
                if (previous != null && previous.hasTrait(MentorTraitId))
                    previous.removeTrait(MentorTraitId);
            }

            _mentorActorId = mentor.data.id;
            ResetMentorFollowTarget();
            mentor.data.set(MentorHeroKey, 0L);
        }

        private void CheckSealExpiration()
        {
            if (World.world?.units == null)
                return;

            foreach (var actor in World.world.units.getSimpleList())
            {
                if (actor == null || !actor.hasTrait(DemonLordTraitId) || !actor.hasTrait(SealedTraitId))
                    continue;

                if (actor.hasTrait(PermaSealedTraitId))
                    continue;

                actor.data.get(DemonLordSealTimestampKey, out int sealedYear, -1);
                if (sealedYear < 0)
                    continue;

                int yearsSealed = Date.getCurrentYear() - sealedYear;
                if (yearsSealed >= MagiaConfig.DemonLordSealDurationYears)
                    ReleaseDemonLordSeal(actor);
            }
        }

        private void RegisterSpells()
        {
            foreach (var definition in _spellDefinitions)
            {
                _spellDefinitionLookup[definition.Id] = definition;
                if (AssetManager.spells.has(definition.Id))
                    continue;

                var asset = new SpellAsset
                {
                    id = definition.Id,
                    action = new AttackAction(definition.Action),
                    cast_target = definition.CastTarget,
                    cast_entity = definition.CastEntity,
                    chance = definition.Chance,
                    cost_mana = definition.ManaDrain,
                    min_distance = definition.MinDistance,
                    health_ratio = definition.HealthRatio,
                    can_be_used_in_combat = definition.CanBeUsedInCombat
                };

                AssetManager.spells.add(asset);
            }
        }

        private void RegisterStatuses()
        {
            RegisterStatusAsset(MagiaStatusId, MagiaTitleKey, MagiaDescriptionKey, "ui/Icons/iconSpellBoost");
            RegisterStatusAsset(ChargingMagiaStatusId, ChargingTitleKey, ChargingDescriptionKey, "ui/Icons/iconRecoverySpell");
        }

        private void RegisterStatusAsset(string id, string titleKey, string descriptionKey, string iconPath)
        {
            if (AssetManager.status.has(id))
                return;

            var asset = new StatusAsset
            {
                id = id,
                locale_id = titleKey,
                locale_description = descriptionKey,
                duration = 1f,
                path_icon = iconPath
            };

            AssetManager.status.add(asset);
        }

          private void RegisterTraits()
          {
              EnsureMagicTraitGroup();
            foreach (var type in _typeDefinitions)
            {
                if (_typeSpellMap.TryGetValue(type.Id, out var spells))
                    type.SpellIds = spells;
                else
                    type.SpellIds = Array.Empty<string>();
            }

            MagiaConfig.EnsureAffinitySpawnRates(_typeDefinitions.Where(def => def.AllowAutomatic).Select(def => def.Id));
            _autoAssignableTypes = _typeDefinitions.Where(def => def.AllowAutomatic).ToList();

            foreach (var type in _typeDefinitions)
            {
                if (AssetManager.traits.has(type.TraitId))
                {
                    UpdateTrait(AssetManager.traits.get(type.TraitId), type);
                }
                else
                {
                    var trait = new ActorTrait();
                    UpdateTrait(trait, type);
                    AssetManager.traits.add(trait);
                }
            }
        }

          private void UpdateTrait(ActorTrait trait, MagicTypeDefinition type)
          {
              trait.id = type.TraitId;
              trait.group_id = "magic";
            trait.path_icon = type.IconPath;
            bool isSealedAffinity = type.TraitId == SealedTraitId;
            bool isPermaSealedAffinity = type.TraitId == PermaSealedTraitId;
            float spawnRate = !isSealedAffinity && !isPermaSealedAffinity && type.AllowAutomatic ? MagiaConfig.GetAffinitySpawnRate(type.Id) : 0f;
            int spawnRateInt = Mathf.Max(0, Mathf.RoundToInt(spawnRate));
            trait.spawn_random_trait_allowed = !isSealedAffinity && !isPermaSealedAffinity && spawnRateInt > 0;
            trait.rate_birth = (isSealedAffinity || isPermaSealedAffinity) ? 0 : spawnRateInt;
            trait.rate_inherit = 0;
            trait.has_localized_id = true;
            trait.special_locale_description = $"trait_magic_{type.Id}_info";
            bool allowSealedAssignment = MagiaConfig.AllowSealedTraitAssignment;
            bool canAssignSealedAffinity = isSealedAffinity && allowSealedAssignment;
            bool canAssignPermaSealedAffinity = isPermaSealedAffinity;
            bool isNonSealedAffinity = !isSealedAffinity && !isPermaSealedAffinity;
            trait.can_be_given = isNonSealedAffinity || canAssignSealedAffinity || canAssignPermaSealedAffinity;
            trait.can_be_removed = isNonSealedAffinity || canAssignSealedAffinity || canAssignPermaSealedAffinity;
            trait.special_effect_interval = 1f;
            trait.spells_ids = type.SpellIds?.ToList() ?? new List<string>();
            trait.linkSpells();
        }

        private void RegisterWorldLog()
        {
            _orlLogAsset = RegisterWorldLogAsset("magic_orl_reincarnated", "worldlog_magic_orl_reincarnated", OrlIconPath);
            _heroLogBornAsset = RegisterWorldLogAsset(HeroLogBornId, "worldlog_magic_hero_born", "ui/Icons/actor_traits/iconAttractive");
            _heroLogDeathAsset = RegisterWorldLogAsset(HeroLogDeathId, "worldlog_magic_hero_fallen", "ui/Icons/actor_traits/iconAttractive");
            _demonLogBornAsset = RegisterWorldLogAsset(DemonLordLogBornId, "worldlog_magic_demonlord_born", DemonLordIconPath);
            _demonLogDeathAsset = RegisterWorldLogAsset(DemonLordLogDeathId, "worldlog_magic_demonlord_fallen", DemonLordIconPath);
            _demonLogSealAsset = RegisterWorldLogAsset(DemonLordLogSealId, "worldlog_magic_demonlord_sealed", DemonLordIconPath);
            _demonLogUnsealAsset = RegisterWorldLogAsset(DemonLordLogUnsealId, "worldlog_magic_demonlord_unsealed", DemonLordIconPath);
            _godLogBornAsset = RegisterWorldLogAsset(GodLogBornId, "worldlog_magic_god_born", GodIconPath);
            _godLogDeathAsset = RegisterWorldLogAsset(GodLogDeathId, "worldlog_magic_god_fallen", GodIconPath);
            _mentorLogBornAsset = RegisterWorldLogAsset(MentorLogBornId, "worldlog_magic_mentor_born", "ui/Icons/iconHelixDNA");
            _heroPartyLogBornAsset = RegisterWorldLogAsset(HeroPartyLogBornId, "worldlog_magic_hero_party_born", "ui/Icons/iconFavoriteStar");
            _heroPartyLogDeathAsset = RegisterWorldLogAsset(HeroPartyLogDeathId, "worldlog_magic_hero_party_fallen", "ui/Icons/iconFavoriteStar");
        }

        private WorldLogAsset RegisterWorldLogAsset(string id, string localeId, string iconPath)
        {
            if (AssetManager.world_log_library.has(id))
                return AssetManager.world_log_library.get(id);

            var asset = new WorldLogAsset
            {
                id = id,
                group = "magic",
                path_icon = iconPath,
                locale_id = localeId,
                color = Toolbox.color_log_good,
                text_replacer = new WorldLogTextFormatter(FormatMagicLog)
            };

            return AssetManager.world_log_library.add(asset);
        }

        private void FormatMagicLog(WorldLogMessage message, ref string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            text = text.Replace("$special1$", message.special1 ?? string.Empty);
            text = text.Replace("$special2$", message.special2 ?? string.Empty);
        }

        public void HandleBirth(Actor baby, Actor parent1, Actor parent2)
        {
            if (!_initialized || baby == null || HasMagicTrait(baby))
                return;

            if (!TryBlessByGod(baby))
            {
                var type = DetermineInheritedType(parent1, parent2);
                ApplyType(baby, type);
            }

            ApplyManaInheritance(baby, parent1, parent2);
        }

        public void HandleSpawn(Actor unit)
        {
            if (!_initialized || unit == null || HasParents(unit) || HasMagicTrait(unit))
                return;

            if (!TryBlessByGod(unit))
            {
                var type = ChooseRandomType();
                ApplyType(unit, type);
            }
        }

        private bool TryBlessByGod(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return false;

            if (!IsHumanoidRace(actor))
                return false;

            if (GetGodActor() == null)
                return false;

            if (TryBlessHero(actor))
                return true;

            if (!actor.isAdult() && TryBlessHeroPartyTrait(actor))
                return true;

            if (TryBlessMentor(actor))
                return true;

            return false;
        }

        private bool TryBlessHero(Actor actor)
        {
            if (actor == null || actor.isRekt() || actor.isAdult() || !IsHumanoidRace(actor))
                return false;

            if (GetHeroActor() != null)
                return false;

            var definition = GetTypeDefinition(HeroTraitId);
            if (definition == null)
                return false;

            EnsureTraitExclusive(HeroTraitId, actor);
            ApplyType(actor, definition);
            return true;
        }

        private bool TryBlessHeroPartyTrait(Actor actor)
        {
            if (actor == null || actor.isRekt() || actor.isAdult() || !IsHumanoidRace(actor))
                return false;

            if (actor.hasTrait(HeroTraitId) || actor.hasTrait(MentorTraitId) || IsHeroPartyTrait(actor))
                return false;

            var hero = GetHeroActor();
            if (hero == null || hero.isRekt())
                return false;

            foreach (var traitId in HeroPartyTraitIds)
            {
                if (IsHeroPartyTraitActive(traitId))
                    continue;

                if (IsHeroPartyTraitBlocked(hero, traitId))
                    continue;

                var definition = GetTypeDefinition(traitId);
                if (definition == null)
                    continue;

                EnsureTraitExclusive(traitId, actor);
                ApplyType(actor, definition);
                return true;
            }

            return false;
        }

        private bool TryBlessMentor(Actor actor)
        {
            if (!IsEligibleMentorCandidate(actor))
                return false;

            if (GetMentorActor() != null)
                return false;

            if (actor.hasTrait(HeroTraitId) || IsHeroPartyTrait(actor))
                return false;

            var definition = GetTypeDefinition(MentorTraitId);
            if (definition == null)
                return false;

            EnsureTraitExclusive(MentorTraitId, actor);
            ApplyType(actor, definition);
            return true;
        }

        private bool IsHumanoidRace(Actor actor)
        {
            return actor?.asset?.is_humanoid == true;
        }

        private bool IsEligibleMentorCandidate(Actor actor)
        {
            if (actor == null || actor.isRekt() || actor.hasTrait(MentorTraitId) || !IsHumanoidRace(actor))
                return false;

            if (!actor.isAdult() || !actor.isPrettyOld())
                return false;

            var hero = GetHeroActor();
            if (hero != null && !hero.isRekt() && hero.getAge() >= actor.getAge())
                return false;

            return true;
        }

        private void EnsureTraitExclusive(string traitId, Actor owner)
        {
            RemoveTraitFromOthers(traitId, owner);
        }

        private void RemoveTraitFromOthers(string traitId, Actor owner)
        {
            if (string.IsNullOrEmpty(traitId))
                return;

            var traitAsset = AssetManager.traits?.get(traitId);
            if (traitAsset == null)
                return;

            var units = World.world?.units?.getSimpleList();
            if (units == null)
                return;

            foreach (var candidate in units)
            {
                if (candidate == null || candidate == owner)
                    continue;

                if (candidate.hasTrait(traitId))
                    candidate.removeTrait(traitAsset);
            }
        }

        public void HandleTraitAdded(Actor actor, ActorTrait trait)
        {
            if (!_initialized || actor == null || trait == null)
                return;

            if (trait.id == OrlTraitId)
            {
                actor.data.get(OrlCounterKey, out int existing);
                if (existing <= 0)
                    actor.data.set(OrlCounterKey, OrlReincarnationLimit);

                actor.data.get(OrlBoostKey, out int boosts, -1);
                if (boosts < 0)
                    actor.data.set(OrlBoostKey, 0);
            }
            else if (trait.id == HeroTraitId)
            {
                actor.data.set(HeroPowerCycleKey, 0);
                EnsureHeroState(actor);
                LogWorldEvent(_heroLogBornAsset, actor);
            }
            else if (trait.id == GodTraitId)
            {
                if (actor.data != null)
                {
                    _godActorId = actor.data.id;
                    EnsureGodState(actor);
                }
                LogWorldEvent(_godLogBornAsset, actor);
            }
            else if (trait.id == DemonLordTraitId)
            {
                EnsureDemonLordState(actor);
                LogWorldEvent(_demonLogBornAsset, actor);
            }
            else if (trait.id == MentorTraitId)
            {
                SetMentor(actor);
                LogWorldEvent(_mentorLogBornAsset, actor, GetHeroForMentor(actor));
            }
            else if (IsHeroPartyTrait(trait.id))
            {
                ApplyChampionStats(actor, ChampionTier.HeroParty);
                AssignHeroPartyToHero(actor);
                LogWorldEvent(_heroPartyLogBornAsset, actor, GetHeroForHeroParty(actor));
            }
            else if ((trait.id == SealedTraitId || trait.id == PermaSealedTraitId) && !_isApplyingSealedTrait)
            {
                ApplySealedState(actor);
            }
            else if (string.Equals(trait.id, GetTypeDefinition(NoElementTypeId)?.TraitId, StringComparison.Ordinal))
            {
                EnforceNoElementState(actor);
                if (!IsPlayerAffinityTrackingSuppressed)
                    RegisterPlayerAssignedAffinity(actor, trait.id);
            }
            else if (!IsPlayerAffinityTrackingSuppressed && IsAffinityTrait(trait.id))
            {
                RegisterPlayerAssignedAffinity(actor, trait.id);
            }

            if (IsAffinityTrait(trait.id))
                ScheduleAffinityCleanup(actor);
        }

        private void RegisterMageRankTraits()
        {
            EnsureMagicTraitGroup();
            foreach (var definition in MageRankDefinitions)
            {
                if (_mageRankTraitLookup.ContainsKey(definition.TraitId))
                    continue;

                ActorTrait trait;
                if (AssetManager.traits.has(definition.TraitId))
                {
                    trait = AssetManager.traits.get(definition.TraitId);
                }
                else
                {
                    trait = new ActorTrait();
                    trait.id = definition.TraitId;
                    AssetManager.traits.add(trait);
                }

                trait.group_id = "magic";
                trait.path_icon = "ui/Icons/iconFavoriteStar";
                trait.spawn_random_trait_allowed = false;
                trait.rate_birth = 0;
                trait.rate_inherit = 0;
                trait.has_localized_id = true;
                trait.special_locale_id = definition.LocaleId;
                trait.special_locale_description = definition.DescriptionLocaleId;
                trait.can_be_given = false;
                trait.can_be_removed = false;
                trait.special_effect_interval = 1f;
                trait.special_locale_description_2 = null;
                trait.linkSpells();

                _mageRankTraitLookup[definition.TraitId] = trait;
            }
        }

        private void EnsureMagicTraitGroup()
        {
            if (AssetManager.trait_groups.has("magic"))
                return;

            var groupAsset = new ActorTraitGroupAsset
            {
                id = "magic",
                name = "trait_group_magic",
                color = "#B15FFF"
            };

            AssetManager.trait_groups.add(groupAsset);
        }

        public void HandleTraitRemoved(Actor actor, ActorTrait trait)
        {
            if (!_initialized || actor == null || trait == null)
                return;

            if (IsAffinityTrait(trait.id))
            {
                UnregisterPlayerAssignedAffinity(actor, trait.id);
                ScheduleAffinityCleanup(actor);
            }

            if (trait.id == HeroTraitId)
            {
                ClearHeroPartyBlocks(actor);
                RemoveHeroPartyTraitsFromWorld();
            }

            if (trait.id == GodTraitId && _godActorId == actor.data?.id)
            {
                _godActorId = 0;
            }

            if ((trait.id == GodTraitId || trait.id == DemonLordTraitId) && actor.data != null)
            {
                actor.data.removeFloat(GodTimePowerScaleKey);
            }

            if (IsHeroPartyTrait(trait.id))
            {
                actor.data.set(HeroPartyHeroKey, 0L);
                ClearHeroPartyTarget(actor);
                return;
            }

            if (trait.id == MentorTraitId && _mentorActorId == actor.data.id)
            {
                actor.data.get(MentorHeroKey, out long heroId, 0L);
                actor.data.set(MentorHeroKey, 0L);
                _mentorActorId = 0;
                ResetMentorFollowTarget();

                var hero = GetActorById(heroId);
                if (hero != null && hero.hasTrait(HeroTraitId))
                    hero.data.set(HeroMentorKey, 0L);
                return;
            }

            if ((trait.id == SealedTraitId || trait.id == PermaSealedTraitId) && !_isUnsealingDemonLord)
            {
                bool isDemonLord = actor.hasTrait(DemonLordTraitId);
                CleanupSealedDemon(actor, isDemonLord);
            }
        }

        private bool IsPlayerAffinityTrackingSuppressed => _playerAffinityMetadataSuppression > 0;

        private void WithPlayerAffinityTrackingSuppressed(Action action)
        {
            if (action == null)
                return;
            _playerAffinityMetadataSuppression++;
            try
            {
                action();
            }
            finally
            {
                _playerAffinityMetadataSuppression = Math.Max(0, _playerAffinityMetadataSuppression - 1);
            }
        }

        private void AddTraitWithoutTracking(Actor actor, string traitId)
        {
            if (actor == null || string.IsNullOrEmpty(traitId))
                return;
            WithPlayerAffinityTrackingSuppressed(() => actor.addTrait(traitId));
        }

        private void AddTraitWithoutTracking(Actor actor, ActorTrait trait)
        {
            if (actor == null || trait == null)
                return;
            WithPlayerAffinityTrackingSuppressed(() => actor.addTrait(trait));
        }

        private HashSet<string> GetPlayerAssignedAffinityIds(Actor actor)
        {
            if (actor?.data == null)
                return new HashSet<string>(StringComparer.Ordinal);

            actor.data.get(PlayerAffinityDataKey, out string stored, string.Empty);
            if (string.IsNullOrWhiteSpace(stored))
                return new HashSet<string>(StringComparer.Ordinal);

            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (var token in stored.Split(new[] { PlayerAffinitySeparator }, StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmed = token?.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    set.Add(trimmed);
            }

            return set;
        }

        private void SavePlayerAssignedAffinityIds(Actor actor, HashSet<string> ids)
        {
            if (actor?.data == null)
                return;

            if (ids == null || ids.Count == 0)
            {
                actor.data.removeString(PlayerAffinityDataKey);
                return;
            }

            var ordered = ids.OrderBy(id => id, StringComparer.Ordinal).ToArray();
            actor.data.set(PlayerAffinityDataKey, string.Join(PlayerAffinitySeparator.ToString(), ordered));
        }

        private void RegisterPlayerAssignedAffinity(Actor actor, string traitId)
        {
            if (actor?.data == null || string.IsNullOrEmpty(traitId))
                return;

            var set = GetPlayerAssignedAffinityIds(actor);
            if (set.Add(traitId))
                SavePlayerAssignedAffinityIds(actor, set);
        }

        private void UnregisterPlayerAssignedAffinity(Actor actor, string traitId)
        {
            if (actor?.data == null || string.IsNullOrEmpty(traitId))
                return;

            var set = GetPlayerAssignedAffinityIds(actor);
            if (set.Remove(traitId))
                SavePlayerAssignedAffinityIds(actor, set);
        }

        public void HandleActorDestroyed(Actor actor)
        {
            if (!_initialized || actor == null)
                return;

            UnregisterDemonLordSummon(actor);

            long actorId = actor.data?.id ?? 0L;
            if (actorId > 0)
            {
                if (_processedDeaths.Contains(actorId))
                    return;
                _processedDeaths.Add(actorId);
            }

            if (IsHeroPartyTrait(actor))
            {
                var heroPartyTrait = actor.getTraits()
                    .FirstOrDefault(trait => trait != null && IsHeroPartyTrait(trait.id));
                ClearHeroPartyTarget(actor);
                LogWorldEvent(_heroPartyLogDeathAsset, actor, GetHeroForHeroParty(actor));
                BlockHeroPartyTraitForHero(heroPartyTrait?.id);
            }

            if (actor.hasTrait(MentorTraitId))
                ResetMentorFollowTarget();

            if (actor.hasTrait(HeroTraitId))
            {
                ClearHeroPartyBlocks(actor);
                RemoveHeroPartyTraitsFromWorld();
                LogWorldEvent(_heroLogDeathAsset, actor);
                TriggerCalamitousExplosion(actor);
                actor.data.set(HeroMentorKey, 0L);
            }

            if (actor.hasTrait(DemonLordTraitId))
            {
                LogWorldEvent(_demonLogDeathAsset, actor);
                TriggerCalamitousExplosion(actor);
                var spawnTile = ChooseDemonReincarnationTile(actor?.current_tile);
                ScheduleDemonLordReincarnation(actor, spawnTile);
                return;
            }

            if (actor.hasTrait(GodTraitId))
                LogWorldEvent(_godLogDeathAsset, actor);

            if (actor.hasTrait(GodTraitId) && actor.data != null && _godActorId == actor.data.id)
                _godActorId = 0;

            if (TryReincarnateOrl(actor))
                actor.data.set(OrlCounterKey, 0);
        }

        private void RegisterDemonLordSummon(Actor actor)
        {
            if (actor == null || actor.data == null)
                return;

            long id = actor.data.id;
            if (id <= 0)
                return;

            _demonLordSummonIds.Add(id);
        }

        private void UnregisterDemonLordSummon(Actor actor)
        {
            if (actor == null || actor.data == null)
                return;

            _demonLordSummonIds.Remove(actor.data.id);
        }

        public bool IsDemonLordSummon(Actor actor)
        {
            if (actor == null || actor.data == null)
                return false;

            return _demonLordSummonIds.Contains(actor.data.id);
        }

        public bool CanFriendlyAttackDemonLordSummon(Actor attacker, Actor summon)
        {
            if (attacker == null || summon == null)
                return true;

            return attacker.isInAggroList(summon);
        }

        public void HandleDemonLordSummon(Actor summon, Actor demon)
        {
            if (summon == null)
                return;

            RegisterDemonLordSummon(summon);
            AssignSummonToArmy(summon, demon);
        }

        private void AssignSummonToArmy(Actor summon, Actor demon)
        {
            if (summon == null || demon == null)
                return;

            Army army = ResolveDemonLordArmy(demon);
            if (army == null)
                return;

            summon.setArmy(army);
        }

        private Army ResolveDemonLordArmy(Actor demon)
        {
            if (demon == null || demon.kingdom == null || World.world == null)
                return null;

            ArmyManager armyManager = World.world.armies;
            if (armyManager == null)
                return null;

            long kingdomId = demon.kingdom.id;
            if (_demonLordArmies.TryGetValue(kingdomId, out Army cached))
            {
                if (cached != null && !cached.isRekt())
                    return cached;
                _demonLordArmies.Remove(kingdomId);
            }

            Army army = demon.army;
            if (army != null && !army.isRekt())
            {
                _demonLordArmies[kingdomId] = army;
                return army;
            }

            army = demon.city?.army;
            if (army != null && !army.isRekt())
            {
                _demonLordArmies[kingdomId] = army;
                return army;
            }

            foreach (Army existing in armyManager)
            {
                if (existing != null && !existing.isRekt() && existing.getKingdom() == demon.kingdom)
                {
                    _demonLordArmies[kingdomId] = existing;
                    return existing;
                }
            }

            City fallbackCity = demon.city ?? demon.current_tile?.zone_city ?? demon.kingdom.capital;
            if (fallbackCity != null)
            {
                Army created = armyManager.newArmy(demon, fallbackCity);
                if (created != null)
                {
                    _demonLordArmies[kingdomId] = created;
                    return created;
                }
            }

            return null;
        }

        private void ScheduleDemonLordReincarnation(Actor fallen, WorldTile spawnTile)
        {
            if (fallen == null)
                return;

            var soulSnapshot = CaptureDemonLordSoulSnapshot(fallen);
            int delay = MagiaConfig.DemonLordReincarnationDelayYears;
            if (delay <= 0)
            {
                TryReincarnateDemonLord(fallen, soulSnapshot, spawnTile);
                return;
            }

            if (_pendingDemonReincarnations.Any(pending => pending?.Fallen == fallen))
                return;

            _pendingDemonReincarnations.Add(new PendingDemonLordReincarnation
            {
                Fallen = fallen,
                SoulSnapshot = soulSnapshot,
                SpawnTile = spawnTile,
                DueYear = Date.getCurrentYear() + delay
            });
        }

        private void ProcessPendingDemonReincarnations()
        {
            if (_pendingDemonReincarnations.Count == 0 || World.world?.units == null)
                return;

            int currentYear = Date.getCurrentYear();
            for (int i = _pendingDemonReincarnations.Count - 1; i >= 0; i--)
            {
                var pending = _pendingDemonReincarnations[i];
                if (pending == null)
                {
                    _pendingDemonReincarnations.RemoveAt(i);
                    continue;
                }

                bool hasSource = pending.SoulSnapshot != null || (pending.Fallen != null && !pending.Fallen.isRekt());
                if (!hasSource)
                {
                    _pendingDemonReincarnations.RemoveAt(i);
                    continue;
                }

                if (currentYear < pending.DueYear)
                    continue;

                if (TryReincarnateDemonLord(pending.Fallen, pending.SoulSnapshot, pending.SpawnTile))
                    _pendingDemonReincarnations.RemoveAt(i);
            }
        }

        private bool TryReincarnateDemonLord(Actor fallen, DemonLordSoulSnapshot soulSnapshot = null, WorldTile spawnTile = null)
        {
            if (soulSnapshot == null)
            {
                if (fallen == null || !fallen.hasTrait(DemonLordTraitId))
                    return false;
                soulSnapshot = CaptureDemonLordSoulSnapshot(fallen);
                if (soulSnapshot == null)
                    return false;
            }

            WorldTile tileForSpawn = spawnTile ?? fallen?.current_tile ?? GetFallbackSpawnTile();
            var target = FindKidBody(fallen);
            if (target == null)
            {
                target = CreateDemonReincarnationHost(fallen, tileForSpawn);
            }

            if (target == null)
                return false;

            ApplySoulSnapshotToHost(target, soulSnapshot);
            WorldTile placementTile = spawnTile ?? tileForSpawn;
            if (placementTile != null && target != null)
            {
                var currentTile = target.current_tile;
                if (currentTile == null || Toolbox.SquaredDistTile(currentTile, placementTile) > 0)
                {
                    target.current_tile = placementTile;
                    target.current_position = new Vector2(placementTile.posV3.x, placementTile.posV3.y);
                    target.setStatsDirty();
                }
            }
            if (!target.hasTrait(DemonLordTraitId))
                target.addTrait(DemonLordTraitId);
            return true;
        }

        private Actor CreateDemonReincarnationHost(Actor fallen, WorldTile spawnTile)
        {
            Actor host = null;
            if (fallen != null)
                host = BabyMaker.makeBaby(fallen, null, ActorSex.None, false, 0, spawnTile, pAddToFamily: false, pJoinFamily: true);

            if (host != null)
                return host;

            WorldTile fallbackTile = spawnTile ?? GetFallbackSpawnTile();
            if (fallbackTile == null || World.world?.units == null)
                return null;

            return World.world.units.spawnNewUnit("demon", fallbackTile, pSpawnSound: false, pMiracleSpawn: true, pAdultAge: false);
        }

        private DemonLordSoulSnapshot CaptureDemonLordSoulSnapshot(Actor demon)
        {
            if (demon == null || demon.data == null)
                return null;

            var snapshot = new DemonLordSoulSnapshot
            {
                Name = demon.getName(),
                CustomName = demon.data.custom_name,
                Favorite = demon.isFavorite(),
                FavoriteFood = demon.data.favorite_food,
                Data = new ActorData(),
                Stats = new List<StatSnapshot>(),
                PastNames = demon.data.past_names != null ? new List<NameEntry>(demon.data.past_names) : new List<NameEntry>(),
                TraitIds = demon.getTraits()
                    .Where(trait => trait != null)
                    .Select(trait => trait.id)
                    .Where(id => !string.IsNullOrEmpty(id))
                    .ToList(),
                Culture = demon.culture,
                Language = demon.language,
                Religion = demon.religion,
                Clan = demon.clan,
                Family = demon.family,
                Plot = demon.plot,
                City = demon.city,
                Army = demon.army,
                Kingdom = demon.kingdom,
                Nutrition = demon.getNutrition(),
                Stamina = demon.getStamina(),
                Happiness = demon.getHappiness()
            };

            var data = snapshot.Data;
            data.level = demon.data.level;
            data.experience = demon.data.experience;
            data.renown = demon.data.renown;
            data.money = demon.data.money;
            data.kills = demon.data.kills;
            data.pollen = demon.data.pollen;
            data.generation = demon.data.generation;
            data.births = demon.data.births;
            data.loot = demon.data.loot;
            data.favorite_food = demon.data.favorite_food;
            data.favorite = demon.data.favorite;
            data.cloneCustomDataFrom(demon.data);

            if (demon.stats != null)
            {
                foreach (var stat in demon.stats.getList())
                {
                    snapshot.Stats.Add(new StatSnapshot(stat.id, stat.value));
                }
            }

            return snapshot;
        }

        private void ApplySoulSnapshotToHost(Actor host, DemonLordSoulSnapshot snapshot)
        {
            if (host == null || snapshot == null)
                return;

            string soulName = string.IsNullOrWhiteSpace(snapshot.Name) ? host.getName() : snapshot.Name;
            var reincarnationHistory = BuildReincarnationPastNames(snapshot.PastNames, soulName, snapshot.CustomName);
            host.setName(soulName, false);
            host.data.favorite = snapshot.Favorite;
            host.data.favorite_food = snapshot.FavoriteFood;
            host.data.level = snapshot.Data.level;
            host.data.experience = snapshot.Data.experience;
            host.data.renown = snapshot.Data.renown;
            host.data.money = snapshot.Data.money;
            host.data.kills = snapshot.Data.kills;
            host.data.pollen = snapshot.Data.pollen;
            host.data.generation = snapshot.Data.generation;
            host.data.births = snapshot.Data.births;
            host.data.loot = snapshot.Data.loot;
            host.data.cloneCustomDataFrom(snapshot.Data);
            host.data.custom_name = snapshot.CustomName;
            host.data.past_names = reincarnationHistory;
            host.data.set(DemonLordSummonTimestampKey, Date.getCurrentYear() - DemonLordSummonCooldownYears);

            if (snapshot.Stats != null)
            {
                foreach (var stat in snapshot.Stats)
                {
                    if (string.IsNullOrEmpty(stat.Id))
                        continue;
                    host.stats[stat.Id] = stat.Value;
                }
            }

            var currentTraits = host.getTraits().ToList();
            foreach (var trait in currentTraits)
                host.removeTrait(trait);

            if (snapshot.TraitIds != null)
            {
                foreach (var traitId in snapshot.TraitIds)
                {
                    if (string.IsNullOrEmpty(traitId))
                        continue;

                    if (!AssetManager.traits.has(traitId))
                        continue;

                    AddTraitWithoutTracking(host, AssetManager.traits.get(traitId));
                }
            }

            host.setCulture(snapshot.Culture);
            host.joinLanguage(snapshot.Language);
            host.setReligion(snapshot.Religion);
            host.setClan(snapshot.Clan);
            host.setFamily(snapshot.Family);
            host.setPlot(snapshot.Plot);
            host.setCity(snapshot.City);
            host.setArmy(snapshot.Army);
            host.setKingdom(snapshot.Kingdom);
            host.setNutrition(Mathf.RoundToInt(snapshot.Nutrition), false);
            host.setStamina(Mathf.RoundToInt(snapshot.Stamina), false);
            host.setMana(host.getMaxMana());
            host.setHappiness(Mathf.RoundToInt(snapshot.Happiness), false);
            host.setHealth(host.getMaxHealth());
            host.setStatsDirty();
        }

        private sealed class PendingDemonLordReincarnation
        {
            public Actor Fallen;
            public DemonLordSoulSnapshot SoulSnapshot;
            public WorldTile SpawnTile;
            public int DueYear;
        }

        private sealed class StatSnapshot
        {
            public string Id;
            public float Value;

            public StatSnapshot(string id, float value)
            {
                Id = id;
                Value = value;
            }
        }

        private sealed class DemonLordSoulSnapshot
        {
            public string Name;
            public bool CustomName;
            public bool Favorite;
            public string FavoriteFood;
            public ActorData Data;
            public List<StatSnapshot> Stats;
            public List<NameEntry> PastNames;
            public List<string> TraitIds;
            public Culture Culture;
            public Language Language;
            public Religion Religion;
            public Clan Clan;
            public Family Family;
            public Plot Plot;
            public City City;
            public Army Army;
            public Kingdom Kingdom;
            public float Nutrition;
            public float Stamina;
            public float Happiness;
        }

        private WorldTile ChooseDemonReincarnationTile(WorldTile origin)
        {
            if (World.world == null)
                return origin;

            if (origin == null)
                return GetFallbackSpawnTile();

            int minDistance = DemonLordExplosionRadius + DemonLordReincarnationBuffer;
            int squaredDistance = minDistance * minDistance;
            int searchRadius = minDistance * 2;

            for (int i = 0; i < 12; i++)
            {
                var candidate = Toolbox.getRandomTileWithinDistance(origin, searchRadius);
                if (candidate == null)
                    continue;

                if (Toolbox.SquaredDistTile(candidate, origin) >= squaredDistance)
                    return candidate;
            }

            return origin ?? GetFallbackSpawnTile();
        }

        private WorldTile GetFallbackSpawnTile()
        {
            if (World.world?.units != null)
            {
                var tile = World.world.units.getSimpleList()
                    .FirstOrDefault(unit => unit != null && !unit.isRekt() && unit.current_tile != null)
                    ?.current_tile;
                if (tile != null)
                    return tile;
            }

            var island = World.world?.islands_calculator?.getRandomIslandGround();
            return island?.getRandomTile();
        }

        private MagicTypeDefinition DetermineInheritedType(Actor mom, Actor dad)
        {
            var momType = GetPrimaryType(mom);
            var dadType = GetPrimaryType(dad);

            if (momType != null && dadType != null)
            {
                if (momType == dadType)
                    return momType;
                return RandomlyPick(momType, dadType);
            }

            return momType ?? dadType ?? GetTypeDefinition("none");
        }

        private MagicTypeDefinition GetPrimaryType(Actor actor)
        {
            if (actor == null)
                return null;

            return actor.getTraits()
                .Select(trait => GetTypeDefinition(trait.id))
                .FirstOrDefault(def => def != null && def.AllowAutomatic);
        }

        private MagicTypeDefinition GetTypeDefinition(string traitId)
        {
            if (string.IsNullOrEmpty(traitId))
                return null;

            return _typeDefinitions.FirstOrDefault(def => string.Equals(def.TraitId, traitId, StringComparison.Ordinal));
        }

        private MagicTypeDefinition RandomlyPick(MagicTypeDefinition first, MagicTypeDefinition second)
        {
            return Randy.randomBool() ? first : second;
        }

        private MagicTypeDefinition ChooseRandomType()
        {
            if (_autoAssignableTypes == null || _autoAssignableTypes.Count == 0)
                return GetTypeDefinition("none");

            float totalWeight = 0f;
            foreach (var type in _autoAssignableTypes)
                totalWeight += Mathf.Max(0f, MagiaConfig.GetAffinitySpawnRate(type.Id));

            if (totalWeight <= 0f)
            {
                int index = Randy.randomInt(0, _autoAssignableTypes.Count - 1);
                return _autoAssignableTypes[index];
            }

            float roll = Randy.randomFloat(0f, totalWeight);
            float running = 0f;
            foreach (var type in _autoAssignableTypes)
            {
                running += Mathf.Max(0f, MagiaConfig.GetAffinitySpawnRate(type.Id));
                if (roll <= running)
                    return type;
            }

            return _autoAssignableTypes[_autoAssignableTypes.Count - 1];
        }

        public bool HasElementalAffinity(Actor actor)
        {
            if (actor == null)
                return false;

            return actor.getTraits()
                .Select(trait => GetTypeDefinition(trait.id))
                .Any(def => def != null && def.AllowAutomatic && !string.Equals(def.Id, NoElementTypeId, StringComparison.Ordinal));
        }

        private bool HasMagicTrait(Actor actor)
        {
            return actor?.getTraits().Any(trait => trait != null && trait.id.StartsWith("magic_") && trait.id != OrlTraitId) ?? false;
        }

        private bool HasParents(Actor actor)
        {
            if (actor == null || actor.data == null)
                return false;

            return actor.data.parent_id_1 >= 0 || actor.data.parent_id_2 >= 0;
        }

        private void ApplyType(Actor actor, MagicTypeDefinition type)
        {
            if (actor == null || type == null)
                return;

            RemoveOtherTypes(actor, type.TraitId);
            if (!actor.hasTrait(type.TraitId))
                AddTraitWithoutTracking(actor, type.TraitId);
            if (string.Equals(type.Id, NoElementTypeId, StringComparison.Ordinal))
                EnforceNoElementState(actor);
        }

        private void RemoveOtherTypes(Actor actor, string exceptionTrait)
        {
            var toRemove = actor.getTraits()
                .Where(trait => trait.id.StartsWith("magic_") && trait.id != exceptionTrait && trait.id != OrlTraitId)
                .ToList();

            foreach (var trait in toRemove)
                actor.removeTrait(trait);
        }

        private void EnforceNoElementState(Actor actor)
        {
            if (actor == null)
                return;

            actor.stats["mana"] = 0f;
            actor.setMaxMana();
            actor.setMana(0);
            actor.setStatsDirty();
        }

        private void ApplyManaInheritance(Actor baby, Actor parent1, Actor parent2)
        {
            if (baby == null)
                return;

            if (!HasElementalAffinity(baby))
            {
                EnforceNoElementState(baby);
                return;
            }

            float totalMana = 0f;
            if (parent1 != null)
                totalMana += parent1.getMaxMana();
            if (parent2 != null)
                totalMana += parent2.getMaxMana();

            if (totalMana <= 0)
                return;

            baby.stats["mana"] = Mathf.Max(baby.stats["mana"], totalMana);
            baby.setStatsDirty();
            baby.setMana(baby.getMaxMana());
        }

        private bool TryReincarnateOrl(Actor fallen)
        {
            if (fallen == null || !fallen.hasTrait(OrlTraitId))
                return false;

            fallen.data.get(OrlCounterKey, out int remaining);
            if (remaining <= 0)
                return false;

            var target = FindKidBody(fallen);
            if (target == null)
            {
                target = BabyMaker.makeBaby(fallen, null, ActorSex.None, false, 0, fallen.current_tile, pAddToFamily: false, pJoinFamily: true);
            }

            if (target == null)
                return false;

            remaining = Math.Max(remaining - 1, 0);
            TransferSoul(fallen, target, remaining);
            SetOrlCount(target, remaining);
            int usedDeaths = OrlReincarnationLimit - remaining;
            EnsureOrlBoostsReceived(target, usedDeaths);
            if (!target.hasTrait(OrlTraitId))
                target.addTrait(OrlTraitId);
            LogReincarnation(fallen, target);
            return true;
        }

        private Actor FindKidBody(Actor exclude)
        {
            foreach (var candidate in World.world.units.getSimpleList())
            {
                if (candidate == null || candidate == exclude || candidate.isRekt() || candidate.isAdult())
                    continue;
                return candidate;
            }

            return null;
        }

        private void TransferSoul(Actor soul, Actor host, int remaining)
        {
            var soulName = soul.getName();
            var reincarnationHistory = BuildReincarnationPastNames(soul.data?.past_names, soulName, soul.data?.custom_name ?? false);
            host.setName(soulName, false);
            host.data.favorite = soul.isFavorite();
            host.data.favorite_food = soul.data.favorite_food;
            host.data.level = soul.data.level;
            host.data.experience = soul.data.experience;
            host.data.renown = soul.data.renown;
            host.data.money = soul.data.money;
            host.data.kills = soul.data.kills;
            host.data.pollen = soul.data.pollen;
            host.data.generation = soul.data.generation;
            host.data.births = soul.data.births;
            host.data.loot = soul.data.loot;
            host.data.cloneCustomDataFrom(soul.data);
            host.data.custom_name = soul.data.custom_name;
            host.data.past_names = reincarnationHistory;

            CopySubstats(soul, host);
            CopyTraits(soul, host);

            host.setCulture(soul.culture);
            host.joinLanguage(soul.language);
            host.setReligion(soul.religion);
            host.setClan(soul.clan);
            host.setFamily(soul.family);
            host.setPlot(soul.plot);
            host.setCity(soul.city);
            host.setArmy(soul.army);
            host.setKingdom(soul.kingdom);

            host.setNutrition(soul.getNutrition(), false);
            host.setStamina(soul.getStamina(), false);
            host.setMana(host.getMaxMana());
            host.setHappiness(soul.getHappiness(), false);
            host.setHealth(host.getMaxHealth());
            host.setStatsDirty();
        }

        private static List<NameEntry> BuildReincarnationPastNames(IEnumerable<NameEntry> source, string currentName, bool currentCustomName)
        {
            var result = new List<NameEntry>();
            if (source != null)
            {
                foreach (var entry in source)
                {
                    if (string.IsNullOrWhiteSpace(entry.name))
                        continue;

                    if (result.Count > 0 && string.Equals(result[result.Count - 1].name, entry.name, StringComparison.Ordinal))
                        continue;

                    result.Add(new NameEntry(entry.name, entry.custom, entry.color_id, entry.timestamp));
                }
            }

            if (!string.IsNullOrWhiteSpace(currentName))
            {
                if (result.Count == 0 || !string.Equals(result[result.Count - 1].name, currentName, StringComparison.Ordinal))
                    result.Add(new NameEntry(currentName, currentCustomName));
            }

            return result;
        }

        private void CopySubstats(Actor source, Actor destination)
        {
            foreach (var container in source.stats.getList())
                destination.stats[container.id] = container.value;
        }

        private void CopyTraits(Actor source, Actor destination)
        {
            var current = destination.getTraits().ToList();
            foreach (var trait in current)
                destination.removeTrait(trait);
            foreach (var trait in source.getTraits())
                AddTraitWithoutTracking(destination, trait);
        }

        private void SetOrlCount(Actor actor, int value)
        {
            if (actor == null)
                return;

            actor.data.set(OrlCounterKey, value);
        }

        private void LogReincarnation(Actor soul, Actor host)
        {
            if (_orlLogAsset == null || host == null || soul == null)
                return;

            var message = new WorldLogMessage(_orlLogAsset, host.getName(), soul.getName())
            {
                unit = host,
                location = host.current_position
            };
            message.color_special_1 = SerializeColor(host.kingdom?.getColor()?.getColorMain() ?? Color.white);
            message.color_special_2 = SerializeColor(soul.kingdom?.getColor()?.getColorMain() ?? Color.white);
            message.add();
        }

        private void LogWorldEvent(
            WorldLogAsset asset,
            Actor actor,
            Actor other = null,
            string special1 = null,
            string special2 = null)
        {
            if (asset == null || actor == null)
                return;

            string first = special1 ?? actor.getName();
            string second = special2 ?? other?.getName();

            var message = new WorldLogMessage(asset, first, second)
            {
                unit = actor,
                location = actor.current_position,
                special1 = first,
                special2 = second
            };

            message.add();
        }

        private void TriggerCalamitousExplosion(Actor actor)
        {
            if (actor == null)
                return;

            SpawnJudgementExplosion(actor.current_tile, actor);
        }

        private static bool SpawnJudgementExplosion(WorldTile targetTile, Actor caster)
        {
            if (targetTile == null || World.world == null)
                return false;

            if (World.world.explosion_checker != null && World.world.explosion_checker.checkNearby(targetTile, DemonLordExplosionRadius))
                return false;

            EffectsLibrary.spawnAtTileRandomScale("fx_explosion_huge", targetTile, DemonLordExplosionScaleMin, DemonLordExplosionScaleMax);
            World.world.startShake(pIntensity: DemonLordExplosionShakeIntensity, pShakeX: true);
            MapAction.damageWorld(targetTile, DemonLordExplosionRadius, TerraformLibrary.czar_bomba, caster);
            return true;
        }

        internal bool TrySealDemonLord(Actor hero, Actor demon)
        {
            if (!_initialized || hero == null || demon == null || hero.isRekt() || demon.isRekt())
                return false;

            if (!hero.hasTrait(HeroTraitId) || !demon.hasTrait(DemonLordTraitId))
                return false;

            if (demon.hasTrait(SealedTraitId))
                return false;

            if (demon.hasTrait(PermaSealedTraitId))
                return false;

            if (hero.data.level <= demon.data.level)
                return false;

            if (!IsMentorNearby(hero))
                return false;

            ApplyDemonLordSeal(demon);
            LogWorldEvent(_demonLogSealAsset, demon, hero);
            return true;
        }

        internal bool HasSealedTrait(Actor actor)
        {
            return actor != null && actor.hasTrait(SealedTraitId);
        }

        internal bool HasPermaSealedTrait(Actor actor)
        {
            return actor != null && actor.hasTrait(PermaSealedTraitId);
        }

        internal bool IsSealedDemon(Actor actor)
        {
            return HasSealedTrait(actor) && actor.hasTrait(DemonLordTraitId);
        }

        internal bool IsPermaSealed(Actor actor)
        {
            return HasPermaSealedTrait(actor) && actor.hasTrait(DemonLordTraitId);
        }

        private void ApplyDemonLordSeal(Actor demon)
        {
            if (demon == null || demon.isRekt())
                return;

            SealActorTrait(demon, SealedTraitId);
            demon.data.set(DemonLordSealTimestampKey, Date.getCurrentYear());
        }

        private void ApplyPermaSeal(Actor demon)
        {
            if (demon == null || demon.isRekt())
                return;

            SealActorTrait(demon, PermaSealedTraitId);
            demon.data.set(DemonLordSealTimestampKey, Date.getCurrentYear());
        }

        private void SealActorTrait(Actor actor, string traitId)
        {
            if (actor == null || actor.isRekt() || string.IsNullOrEmpty(traitId))
                return;

            if (!actor.hasTrait(traitId))
            {
                _isApplyingSealedTrait = true;
                try
                {
                    AddTraitWithoutTracking(actor, traitId);
                }
                finally
                {
                    _isApplyingSealedTrait = false;
                }
            }

            ApplySealedState(actor);
        }

        private void ApplySealedState(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return;

            actor.cancelAllBeh();
            actor.clearAttackTarget();
            actor.is_ai_frozen = true;
            actor.is_immovable = true;
            actor.setStatsDirty();
            actor.setNutrition(actor.getMaxNutrition());
            actor.setHealth(actor.getMaxHealth());
            actor.setStamina(actor.getMaxStamina());
            actor.setMana(actor.getMaxMana());
            actor.addStatusEffect("invincible", float.MaxValue, pColorEffect: false);
        }

        private void ReleaseDemonLordSeal(Actor demon)
        {
            if (demon == null || demon.isRekt() || !demon.hasTrait(DemonLordTraitId) || !demon.hasTrait(SealedTraitId))
                return;

            _isUnsealingDemonLord = true;
            demon.removeTrait(SealedTraitId);
            _isUnsealingDemonLord = false;

            CleanupSealedDemon(demon, true);
        }

        private void ReleasePermaSeal(Actor demon)
        {
            if (demon == null || demon.isRekt() || !demon.hasTrait(DemonLordTraitId) || !demon.hasTrait(PermaSealedTraitId))
                return;

            _isUnsealingDemonLord = true;
            demon.removeTrait(PermaSealedTraitId);
            _isUnsealingDemonLord = false;

            CleanupSealedDemon(demon, true);
        }

        private void CleanupSealedDemon(Actor demon, bool log)
        {
            if (demon == null)
                return;

            demon.data.set(DemonLordSealTimestampKey, -1);
            demon.is_ai_frozen = false;
            demon.is_immovable = false;
            demon.setStatsDirty();
            demon.clearAttackTarget();
            demon.cancelAllBeh();
            demon.finishStatusEffect("invincible");
            if (log && demon.hasTrait(DemonLordTraitId))
                LogWorldEvent(_demonLogUnsealAsset, demon);
        }

        public SpellAsset DecorateSpellForActor(Actor actor, SpellAsset baseSpell)
        {
            if (actor == null || baseSpell == null)
                return baseSpell;

            if (!_spellDefinitionLookup.TryGetValue(baseSpell.id, out var definition) || !definition.IsAttackSpell)
                return baseSpell;

            var level = Mathf.Clamp(actor.level, 0, 60);
            var multiplier = 1f + level * 0.015f;
            var chance = Mathf.Clamp01(baseSpell.chance * multiplier);
            var maxReduction = Math.Max(0, baseSpell.cost_mana - 1);
            var reduction = Math.Min(maxReduction, (int)(level / 10f));
            var cost = Math.Max(1, baseSpell.cost_mana - reduction);

            return new SpellAsset
            {
                id = baseSpell.id,
                action = baseSpell.action,
                cast_target = baseSpell.cast_target,
                cast_entity = baseSpell.cast_entity,
                chance = chance,
                cost_mana = cost,
                min_distance = baseSpell.min_distance,
                health_ratio = baseSpell.health_ratio,
                decision_ids = baseSpell.decision_ids != null ? new List<string>(baseSpell.decision_ids) : null,
                decisions_assets = baseSpell.decisions_assets,
                can_be_used_in_combat = baseSpell.can_be_used_in_combat
            };
        }

        public bool TryPrepareSpellForCombat(
            Actor actor,
            SpellAsset spell,
            BaseSimObject target,
            out float chance,
            out MagicSpellDefinition definition)
        {
            chance = spell?.chance ?? 0f;
            definition = null;
            if (actor == null || spell == null)
                return false;

            if (!_spellDefinitionLookup.TryGetValue(spell.id, out definition))
                return true;

            if (actor.level < definition.RequiredLevel)
                return false;

            bool inCombat = IsActorInCombat(actor);
            if (definition.Cooldown > 0f && World.world != null)
            {
                actor.data.get(GetCooldownKey(definition.Id), out float lastCast);
                float currentTime = (float) World.world.getCurWorldTime();
                float effectiveCooldown = inCombat
                    ? definition.Cooldown / CombatSpellFrequencyMultiplier
                    : definition.Cooldown;
                if (currentTime - lastCast < effectiveCooldown)
                    return false;
            }

            float rangeMultiplier = CalculateRangeMultiplier(actor, target, definition);
            float rankBonus = GetMageRankChanceBonus(actor);
            chance = Mathf.Clamp01(spell.chance + definition.RangeBonus * rangeMultiplier + rankBonus);
            return true;
        }

        private float GetMageRankChanceBonus(Actor actor)
        {
            if (actor?.data == null)
                return 0f;

            var rank = GetDesiredMageRank(actor.data.level, actor.data.kills);
            if (rank == null)
                return 0f;

            int index = -1;
            for (int i = 0; i < MageRankDefinitions.Count; i++)
            {
                var definition = MageRankDefinitions[i];
                if (definition != null && definition.TraitId == rank.TraitId)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return 0f;

            return Mathf.Clamp01(0.05f * (index + 1));
        }

        public void RecordSpellCast(Actor actor, MagicSpellDefinition definition)
        {
            if (actor == null || definition == null || definition.Cooldown <= 0f || World.world == null)
                return;

            actor.data.set(GetCooldownKey(definition.Id), (float) World.world.getCurWorldTime());
        }

        private void ProcessSpellCharges()
        {
            if (_pendingSpellCharges.Count == 0)
                return;

            float delta = Time.deltaTime;
            for (int i = _pendingSpellCharges.Count - 1; i >= 0; i--)
            {
                var request = _pendingSpellCharges[i];
                if (request == null)
                {
                    _pendingSpellCharges.RemoveAt(i);
                    continue;
                }

                Actor actor = request.Actor;
                if (actor == null || actor.isRekt())
                {
                    CleanupSpellRequest(request);
                    _pendingSpellCharges.RemoveAt(i);
                    continue;
                }

                request.RemainingTime -= delta;
                if (request.RemainingTime <= 0f)
                {
                    _pendingSpellCharges.RemoveAt(i);
                    FinalizeSpellCharge(request);
                }
            }
        }

        public bool QueueSpellForCasting(Actor actor, SpellAsset spell, BaseSimObject target, MagicSpellDefinition definition)
        {
            if (actor == null || spell == null || definition == null)
                return false;

            if (definition.ChargeDuration <= 0f)
                return ExecuteSpellNow(actor, spell, target, definition);

            if (IsActorChargingSpell(actor))
                return false;

            var request = new SpellChargeRequest(actor, spell, target ?? actor, definition, definition.ChargeDuration);
            _pendingSpellCharges.Add(request);
            SetActorSpellKey(actor, ChargingMagiaSpellKey, definition.Id);
            actor.addStatusEffect(ChargingMagiaStatusId, definition.ChargeDuration, false);
            return true;
        }

        private bool ExecuteSpellNow(Actor actor, SpellAsset spell, BaseSimObject target, MagicSpellDefinition definition)
        {
            var request = new SpellChargeRequest(actor, spell, target, definition, 0f);
            return TryExecuteSpell(request);
        }

        private void FinalizeSpellCharge(SpellChargeRequest request)
        {
            if (request == null)
                return;
            TryExecuteSpell(request);
        }

        private bool TryExecuteSpell(SpellChargeRequest request)
        {
            if (request == null)
                return false;

            Actor actor = request.Actor;
            var definition = request.Definition;
            if (actor == null || actor.isRekt() || definition == null)
                return false;

            actor.finishStatusEffect(ChargingMagiaStatusId);
            RemoveActorSpellKey(actor, ChargingMagiaSpellKey);

            float magiaDuration = Mathf.Max(0.5f, definition.ChargeDuration * 0.5f + 0.5f);
            actor.addStatusEffect(MagiaStatusId, magiaDuration, false);
            SetActorSpellKey(actor, MagiaSpellKey, definition.Id);

            BaseSimObject target = request.Target ?? actor;
            WorldTile targetTile = target?.current_tile ?? actor.current_tile;

            bool castSpell = request.Spell?.action?.RunAnyTrue((BaseSimObject) actor, target, targetTile) ?? false;
            if (castSpell)
            {
                actor.doCastAnimation();
                float cooldown = definition.Cooldown > 0f ? definition.Cooldown : 5f;
                actor.addStatusEffect("recovery_spell", cooldown, false);
                RecordSpellCast(actor, definition);
            }
            else
            {
                actor.finishStatusEffect(MagiaStatusId);
                RemoveActorSpellKey(actor, MagiaSpellKey);
            }

            return castSpell;
        }

        private void CleanupSpellRequest(SpellChargeRequest request)
        {
            Actor actor = request?.Actor;
            if (actor == null)
                return;

            actor.finishStatusEffect(ChargingMagiaStatusId);
            RemoveActorSpellKey(actor, ChargingMagiaSpellKey);
        }

        private bool IsActorChargingSpell(Actor actor)
        {
            if (actor == null)
                return false;

            foreach (var request in _pendingSpellCharges)
            {
                if (request?.Actor == actor)
                    return true;
            }

            return false;
        }

        internal void OverrideStatusTooltip(Tooltip tooltip, Status status)
        {
            if (tooltip == null || status == null || status.asset == null)
                return;

            string templateKey = null;
            string storedKey = null;
            string statusId = status.asset.id;

            if (statusId == MagiaStatusId)
            {
                templateKey = MagiaDescriptionKey;
                storedKey = MagiaSpellKey;
            }
            else if (statusId == ChargingMagiaStatusId)
            {
                templateKey = ChargingDescriptionKey;
                storedKey = ChargingMagiaSpellKey;
            }

            if (string.IsNullOrEmpty(templateKey))
                return;

            string spellId = GetStoredSpellId(status.sim_object as Actor, storedKey);
            string description = FormatStatusDescription(templateKey, spellId);
            tooltip.setDescription(description);
        }

        private string FormatStatusDescription(string templateKey, string spellId)
        {
            if (string.IsNullOrEmpty(templateKey))
                return string.Empty;

            string template = LocalizedTextManager.getText(templateKey);
            if (string.IsNullOrEmpty(template))
                return string.Empty;

            if (!string.IsNullOrEmpty(spellId))
            {
                string spellName = GetLocalizedSpellName(spellId);
                if (!string.IsNullOrEmpty(spellName))
                {
                    if (template.Contains(SpellPlaceholder))
                        template = template.Replace(SpellPlaceholder, spellName);
                    else
                        template = $"{template} {spellName}";
                }
            }

            return template;
        }

        private string GetLocalizedSpellName(string spellId)
        {
            if (string.IsNullOrEmpty(spellId))
                return null;

            return LocalizedTextManager.getText($"spell_{spellId}");
        }

        private string GetStoredSpellId(Actor actor, string key)
        {
            if (actor == null || actor.data == null || string.IsNullOrEmpty(key))
                return null;

            actor.data.get(key, out string value, string.Empty);
            return value;
        }

        private void SetActorSpellKey(Actor actor, string key, string spellId)
        {
            if (actor == null || actor.data == null || string.IsNullOrEmpty(key))
                return;
            actor.data.set(key, spellId);
        }

        private void RemoveActorSpellKey(Actor actor, string key)
        {
            if (actor == null || actor.data == null || string.IsNullOrEmpty(key))
                return;
            actor.data.removeString(key);
        }

        private void EnsureOrlBoostsReceived(Actor actor, int targetBoosts)
        {
            if (actor == null)
                return;

            targetBoosts = Math.Max(0, targetBoosts);
            actor.data.get(OrlBoostKey, out int boosts, -1);
            if (boosts < 0)
            {
                boosts = 0;
                actor.data.set(OrlBoostKey, 0);
            }

            if (targetBoosts > boosts)
            {
                for (int i = boosts; i < targetBoosts; i++)
                    ApplyOrlBoost(actor);
                actor.data.set(OrlBoostKey, targetBoosts);
            }
        }

        private void ApplyOrlBoost(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return;

            float heroPower = Math.Max(GetWorldMostPowerfulActorPower(actor), 1f);
            float currentPower = Math.Max(GetActorPower(actor), 1f);
            float targetPower = heroPower * 2f;
            float multiplier = currentPower < targetPower ? targetPower / currentPower : 2f;

            if (multiplier > 1f)
                ScaleOrlStats(actor, multiplier);

            actor.setMaxHealth();
            actor.setMaxStamina();
            actor.setMaxMana();
            actor.setStatsDirty();
            actor.setHealth(actor.getMaxHealth());
            actor.setStamina(actor.getMaxStamina());
            actor.setMana(actor.getMaxMana());
        }

        private void ScaleOrlStats(Actor actor, float multiplier)
        {
            if (actor == null || multiplier <= 1f)
                return;

            foreach (var statId in OrlBoostStatIds)
            {
                float currentValue = actor.stats[statId];
                if (currentValue <= 0f)
                    continue;
                actor.stats[statId] = currentValue * multiplier;
            }
        }

        private ChampionStatTargets GetChampionTargets(ChampionTier tier)
        {
            return tier switch
            {
                ChampionTier.HeroParty => new ChampionStatTargets(1800f, 1800f, 220f, 120f, 5f, 7f, 1.5f, 0.25f),
                ChampionTier.Hero => new ChampionStatTargets(6000f, 6000f, 420f, 220f, 7f, 10f, 1.7f, 0.35f),
                ChampionTier.Legendary => new ChampionStatTargets(9600f, 9600f, 640f, 420f, 8f, 12f, 1.9f, 0.45f),
                _ => new ChampionStatTargets(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)
            };
        }

        private void EnsureMinStat(Actor actor, string statId, float minValue)
        {
            if (actor == null || actor.stats == null || string.IsNullOrEmpty(statId))
                return;

            actor.stats.TryGetValue(statId, out float currentValue);
            if (minValue > currentValue)
                actor.stats[statId] = minValue;
        }

        private void ApplyChampionStats(Actor actor, ChampionTier tier)
        {
            if (actor == null || actor.isRekt())
                return;

            var targets = GetChampionTargets(tier);
            EnsureMinStat(actor, "health", targets.Health);
            EnsureMinStat(actor, "mana", targets.Mana);
            EnsureMinStat(actor, "damage", targets.Damage);
            EnsureMinStat(actor, "armor", targets.Armor);
            EnsureMinStat(actor, "range", targets.Range);
            EnsureMinStat(actor, "speed", targets.Speed);
            EnsureMinStat(actor, "attack_speed", targets.AttackSpeed);
            EnsureMinStat(actor, "critical_chance", targets.Critical);

            actor.setMaxHealth();
            actor.setMaxStamina();
            actor.setMaxMana();
            actor.setStatsDirty();
            actor.setHealth(actor.getMaxHealth());
            actor.setStamina(actor.getMaxStamina());
            actor.setMana(actor.getMaxMana());
        }

        private void EnsureHeroPartyState(Actor member)
        {
            if (member == null || member.isRekt() || !IsHeroPartyTrait(member))
                return;

            ApplyChampionStats(member, ChampionTier.HeroParty);
        }

        private void EnsureGodState(Actor god)
        {
            if (god == null || god.isRekt() || !god.hasTrait(GodTraitId))
                return;

            EnsureImmortal(god);
            GrantAllElementalAffinities(god);
            ApplyChampionStats(god, ChampionTier.Legendary);
            ApplyGodTimePowerScaling(god);
        }

        private void EnsureImmortal(Actor actor)
        {
            if (actor == null || actor.isRekt() || actor.hasTrait(ImmortalTraitId))
                return;

            AddTraitWithoutTracking(actor, ImmortalTraitId);
        }

        private void ApplyGodTimePowerScaling(Actor actor)
        {
            if (actor == null || actor.isRekt() || !IsGodTimeActor(actor) || actor.data == null)
                return;

            int scaledAge = Math.Max(1, actor.getAge());
            float desiredScale = CalculateGodTimePowerMultiplier(scaledAge);
            actor.data.get(GodTimePowerScaleKey, out float previousScale, 1f);
            if (previousScale <= 0f)
                previousScale = 1f;

            if (Math.Abs(desiredScale - previousScale) < 0.001f)
            {
                actor.data.set(GodTimePowerScaleKey, desiredScale);
                return;
            }

            float ratio = previousScale > 0f ? desiredScale / previousScale : desiredScale;
            if (ratio <= 0f)
            {
                actor.data.set(GodTimePowerScaleKey, desiredScale);
                return;
            }

            MultiplyGodStat(actor, "health", ratio);
            MultiplyGodStat(actor, "mana", ratio);
            MultiplyGodStat(actor, "damage", ratio);
            MultiplyGodStat(actor, "skill_spell", ratio);

            actor.setStatsDirty();
            actor.setMaxHealth();
            actor.setMaxMana();
            actor.setMaxStamina();
            actor.setHealth(actor.getMaxHealth());
            actor.setMana(actor.getMaxMana());
            actor.setStamina(actor.getMaxStamina());
            actor.data.set(GodTimePowerScaleKey, desiredScale);
        }

        private static float CalculateGodTimePowerMultiplier(int godTimeAge)
        {
            if (godTimeAge < GodTimeBabyAgeMax)
                return MagiaConfig.GodTimeBabyMultiplier;
            if (godTimeAge < GodTimeTeenAgeMax)
                return MagiaConfig.GodTimeTeenMultiplier;
            if (godTimeAge < GodTimeYoungAdultAgeMax)
                return MagiaConfig.GodTimeYoungAdultMultiplier;
            if (godTimeAge < GodTimeAdultAgeMax)
                return MagiaConfig.GodTimeAdultMultiplier;
            return MagiaConfig.GodTimeElderMultiplier;
        }

        private static void MultiplyGodStat(Actor actor, string statId, float ratio)
        {
            if (actor == null || actor.stats == null || string.IsNullOrEmpty(statId) || Mathf.Approximately(ratio, 1f))
                return;

            actor.stats.TryGetValue(statId, out float currentValue);
            actor.stats[statId] = currentValue * ratio;
        }

        internal void ApplyGodTimeAgeAdjustment(Actor actor, ref int age)
        {
            if (actor == null || age <= 0 || !IsGodTimeActor(actor))
                return;

            age = ConvertToGodTimeAge(age);
        }

        private bool IsGodTimeActor(Actor actor)
        {
            return actor != null && !actor.isRekt() && (actor.hasTrait(GodTraitId) || actor.hasTrait(DemonLordTraitId));
        }

        private static int ConvertToGodTimeAge(int baseAge)
        {
            if (baseAge <= 0)
                return baseAge;

            int scaled = (int)Math.Floor(baseAge * MagiaConfig.GodTimeAgeScale);
            return Math.Max(1, scaled);
        }

        private float GetWorldMostPowerfulActorPower(Actor exclude = null)
        {
            if (World.world?.units == null)
                return 0f;

            float maxPower = 0f;
            foreach (var candidate in World.world.units.getSimpleList())
            {
                if (candidate == null || candidate == exclude || candidate.isRekt())
                    continue;

                float power = GetActorPower(candidate);
                if (power > maxPower)
                    maxPower = power;
            }

            return maxPower;
        }

        private float GetActorPower(Actor actor)
        {
            if (actor == null)
                return 0f;

            float health = actor.getMaxHealth();
            float mana = actor.getMaxMana();
            float stamina = actor.getMaxStamina();
            float damage = actor.stats["damage"];
            float armor = actor.stats["armor"];
            float speed = actor.stats["speed"];
            float attackSpeed = actor.stats["attack_speed"];
            float critical = actor.stats["critical_chance"];
            float range = actor.stats["range"];
            float levelBonus = actor.data.level * 3f;
            float kills = actor.data.kills * 2f;
            float renown = actor.data.renown * 1.5f;

            float calculated =
                health * 1f +
                mana * 0.6f +
                stamina * 0.4f +
                damage * 1.5f +
                armor * 1.1f +
                speed * 0.9f +
                attackSpeed * 1.2f +
                critical * 2f +
                range * 0.5f +
                levelBonus +
                kills +
                renown;

            return Math.Max(calculated, 1f);
        }

        private static IReadOnlyList<MageRankDefinition> MageRankDefinitions => MagiaConfig.MageRankDefinitions;

        private static float CalculateRangeMultiplier(Actor actor, BaseSimObject target, MagicSpellDefinition definition)
        {
            if (actor == null || target == null || definition.RangeFalloffDistance <= 0f)
                return 0f;

            var actorTile = actor.current_tile;
            var targetTile = target.current_tile;
            if (actorTile == null || targetTile == null)
                return 0f;

            float distance = Mathf.Sqrt(Toolbox.SquaredDistTile(actorTile, targetTile));
            float normalized = 1f - Mathf.Clamp01(distance / definition.RangeFalloffDistance);
            return Mathf.Clamp01(normalized);
        }

        private static string GetCooldownKey(string spellId) => string.IsNullOrEmpty(spellId) ? SpellCooldownKeyPrefix : $"{SpellCooldownKeyPrefix}{spellId}";

        private static bool OrlPassiveSpell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) => false;

        private static bool DemonLordEternalSpell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) => false;

        private static bool DemonLordSummonSpell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            var demon = pSelf?.a;
            if (demon == null || demon.isRekt() || World.world == null)
                return false;

            WorldTile anchor = pTile ?? pTarget?.current_tile ?? demon.current_tile;
            if (anchor == null)
                return false;

            List<Actor> enemyCandidates = null;
            if (World.world.units != null)
            {
                enemyCandidates = World.world.units.getSimpleList()
                    .Where(unit => unit != null && !unit.isRekt() && unit.kingdom != demon.kingdom)
                    .ToList();
            }

            var manager = MagicManager.Instance;

            int spawned = 0;
            for (int i = 0; i < DemonLordSummonCount; i++)
            {
                WorldTile spawnTile = Toolbox.getRandomTileWithinDistance(anchor, 10) ?? anchor;
                var summoned = World.world.units.spawnNewUnit("demon", spawnTile, pSpawnSound: true, pMiracleSpawn: true);
                if (summoned == null || summoned.isRekt())
                    continue;

                if (demon.kingdom != null)
                    summoned.setKingdom(demon.kingdom);

                manager?.HandleDemonLordSummon(summoned, demon);

                summoned.cancelAllBeh();
                summoned.clearAttackTarget();

                if (enemyCandidates != null && enemyCandidates.Count > 0)
                {
                    var enemy = Randy.getRandom(enemyCandidates);
                    if (enemy != null)
                        summoned.setAttackTarget(enemy);
                }

                spawned++;
            }

            return spawned > 0;
        }

        private static bool DemonLordJudgementSpell(
            BaseSimObject pSelf,
            BaseSimObject pTarget,
            WorldTile pTile = null)
        {
            var targetTile = pTile ?? pTarget?.current_tile ?? pSelf?.current_tile;
            if (targetTile == null || World.world == null)
                return false;

            if (!SpawnJudgementExplosion(targetTile, pSelf?.a))
                return false;

            if (pSelf?.isActor() == true)
            {
                var actor = pSelf.a;
                actor.setHealth(actor.getMaxHealth());
                actor.setStamina(actor.getMaxStamina());
                actor.setMana(actor.getMaxMana());
            }

            return true;
        }

        private sealed class SpellChargeRequest
        {
            public Actor Actor { get; }
            public SpellAsset Spell { get; }
            public BaseSimObject Target { get; }
            public MagicSpellDefinition Definition { get; }
            public float RemainingTime;

            public SpellChargeRequest(
                Actor actor,
                SpellAsset spell,
                BaseSimObject target,
                MagicSpellDefinition definition,
                float duration)
            {
                Actor = actor;
                Spell = spell;
                Target = target;
                Definition = definition;
                RemainingTime = duration;
            }
        }

        private static string SerializeColor(Color color)
        {
            Color32 color32 = color;
            return Toolbox.colorToHex(color32, false);
        }
    }
}
