using HarmonyLib;

namespace AIBox
{
    public static class DiplomacyPatch
    {
        public static bool AllowDiplomacy = false;

        [HarmonyPatch(typeof(DiplomacyManager), "startWar")]
        public static class Patch_StartWar
        {
            static bool Prefix(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pAsset) 
            {
                if (World.world == null || pAsset == null)
                    return false;

                if (pAsset.total_war)
                {
                    if (pAttacker == null || !pAttacker.isAlive())
                        return false;
                }
                else
                {
                    if (pAttacker == null || pDefender == null)
                        return false;
                    if (!pAttacker.isAlive() || !pDefender.isAlive())
                        return false;
                }

                if(AllowDiplomacy || World.world.isAnyPowerSelected()) return true;
                return false;
            }
        }

        [HarmonyPatch(typeof(DiplomacyManager), "eventSpite")]
        public static class Patch_EventSpite
        {
            static bool Prefix() 
            {
                if(AllowDiplomacy || World.world.isAnyPowerSelected()) return true;
                return false;
            }
        }

        [HarmonyPatch(typeof(DiplomacyManager), "eventFriendship")]
        public static class Patch_EventFriendship
        {
            static bool Prefix() 
            {
                if(AllowDiplomacy || World.world.isAnyPowerSelected()) return true;
                return false;
            }
        }

        [HarmonyPatch(typeof(AllianceManager), "newAlliance")]
        public static class Patch_NewAlliance
        {
            static bool Prefix() 
            {
                if(AllowDiplomacy || World.world.isAnyPowerSelected()) return true;
                return false;
            }
        }

        [HarmonyPatch(typeof(Alliance), "join")]
        public static class Patch_AllianceJoin
        {
            static bool Prefix() 
            {
                if(AllowDiplomacy || World.world.isAnyPowerSelected()) return true;
                return false;
            }
        }
    }
}
