using UnityEngine;

namespace XNTM.Code.Features.BetterWars
{
    public static class CollateralWarManager
    {
        private const string GrievanceKeyPrefix = "xntm_keep_off_g_";
        private const string CooldownKeyPrefix = "xntm_keep_off_cd_";
        private const float WarTriggerThreshold = 4.5f;
        private const string KeepOffMyLandLawId = "world_law_keep_off_my_land";

        public static void RegisterCollateralUnitKill(Kingdom victimKingdom, Actor killer)
        {
            RegisterCollateralDamage(victimKingdom, killer?.kingdom, 1f);
        }

        public static void RegisterCollateralBuildingDestruction(Kingdom victimKingdom, BaseSimObject attacker)
        {
            RegisterCollateralDamage(victimKingdom, attacker?.kingdom, 2f);
        }

        private static void RegisterCollateralDamage(Kingdom victimKingdom, Kingdom guiltyKingdom, float severity)
        {
            if (!IsWorldLawEnabled(KeepOffMyLandLawId))
                return;
            if (victimKingdom == null || guiltyKingdom == null)
                return;
            if (!victimKingdom.isAlive() || !guiltyKingdom.isAlive())
                return;
            if (victimKingdom == guiltyKingdom)
                return;
            if (victimKingdom.isInWarWith(guiltyKingdom))
                return;
            if (!guiltyKingdom.hasEnemies())
                return;

            Alliance victimAlliance = victimKingdom.getAlliance();
            Alliance guiltyAlliance = guiltyKingdom.getAlliance();
            if (Alliance.isSame(victimAlliance, guiltyAlliance))
                return;

            if (IsOnCooldown(victimKingdom, guiltyKingdom))
                return;

            float grievance = AddGrievance(victimKingdom, guiltyKingdom, severity);
            if (grievance < WarTriggerThreshold)
                return;

            float chance = BuildJoinChance(victimKingdom, guiltyKingdom, grievance);
            if (!Randy.randomChance(Mathf.Clamp01(chance)))
                return;

            if (World.world.wars.isInWarWith(victimKingdom, guiltyKingdom))
            {
                ResetGrievance(victimKingdom, guiltyKingdom);
                return;
            }

            WarTypeAsset type = AssetManager.war_types_library.get("normal");
            if (type == null)
                return;

            War war = World.world.wars.newWar(victimKingdom, guiltyKingdom, type);
            if (war == null)
                return;

            SetCooldown(victimKingdom, guiltyKingdom, World.world.getCurWorldTime() + 40f);
            ResetGrievance(victimKingdom, guiltyKingdom);
        }

        private static float BuildJoinChance(Kingdom victimKingdom, Kingdom guiltyKingdom, float grievance)
        {
            float chance = 0.25f + grievance * 0.06f;
            Actor ruler = victimKingdom.king;
            if (ruler != null)
            {
                if (ruler.hasTrait("ambitious"))
                    chance += 0.15f;
                if (ruler.hasTrait("bloodlust"))
                    chance += 0.1f;
                if (ruler.hasTrait("peaceful"))
                    chance -= 0.2f;
                if (ruler.hasTrait("content"))
                    chance -= 0.1f;
                chance += Mathf.Clamp((ruler.stats?.get("warfare") ?? 0f) / 30f, -0.2f, 0.2f);
                chance += Mathf.Clamp((ruler.stats?.get("diplomacy") ?? 0f) / 60f, -0.1f, 0.1f);
            }

            int victimArmy = victimKingdom.countTotalWarriors();
            int guiltyArmy = guiltyKingdom.countTotalWarriors();
            if (victimArmy < guiltyArmy * 0.4f)
                chance -= 0.18f;
            else if (victimArmy > guiltyArmy * 0.8f)
                chance += 0.1f;

            int victimCities = victimKingdom.countCities();
            int guiltyCities = guiltyKingdom.countCities();
            if (victimCities >= guiltyCities)
                chance += 0.05f;

            return chance;
        }

        private static float AddGrievance(Kingdom victimKingdom, Kingdom guiltyKingdom, float delta)
        {
            victimKingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            string key = GrievanceKeyPrefix + guiltyKingdom.id;
            if (!victimKingdom.data.custom_data_float.dict.TryGetValue(key, out float current))
                current = 0f;
            current = Mathf.Clamp(current + Mathf.Max(0.25f, delta), 0f, 25f);
            victimKingdom.data.custom_data_float.dict[key] = current;
            return current;
        }

        private static void ResetGrievance(Kingdom victimKingdom, Kingdom guiltyKingdom)
        {
            if (victimKingdom?.data?.custom_data_float == null)
                return;
            victimKingdom.data.custom_data_float.dict.Remove(GrievanceKeyPrefix + guiltyKingdom.id);
        }

        private static bool IsOnCooldown(Kingdom victimKingdom, Kingdom guiltyKingdom)
        {
            if (victimKingdom?.data?.custom_data_float == null)
                return false;
            string key = CooldownKeyPrefix + guiltyKingdom.id;
            if (!victimKingdom.data.custom_data_float.dict.TryGetValue(key, out float until))
                return false;
            double now = World.world.getCurWorldTime();
            if (until > now)
                return true;
            victimKingdom.data.custom_data_float.dict.Remove(key);
            return false;
        }

        private static void SetCooldown(Kingdom victimKingdom, Kingdom guiltyKingdom, double until)
        {
            if (victimKingdom?.data == null)
                return;
            victimKingdom.data.custom_data_float ??= new CustomDataContainer<float>();
            victimKingdom.data.custom_data_float.dict[CooldownKeyPrefix + guiltyKingdom.id] = (float)until;
        }

        private static bool IsWorldLawEnabled(string lawId)
        {
            WorldLawAsset law = AssetManager.world_laws_library?.get(lawId);
            return law != null && law.isEnabled();
        }
    }
}
