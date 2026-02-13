namespace XNTM.Code.Features
{
    public static class WorldLawsFeature
    {
        private const string ArmyNoCrossForeignLawId = "world_law_army_no_cross_foreign";
        private const string KeepOffMyLandLawId = "world_law_keep_off_my_land";
        private const string DiplomacyGroupId = "diplomacy";
        private const string ArmyNoCrossForeignIconPath = "ui/Icons/iconArmy";
        private const string KeepOffMyLandIconPath = "ui/Icons/iconDiplomacy";

        public static void Register()
        {
            var library = AssetManager.world_laws_library;
            if (library == null)
                return;

            RegisterOrUpdateLaw(library, ArmyNoCrossForeignLawId, ArmyNoCrossForeignIconPath);
            RegisterOrUpdateLaw(library, KeepOffMyLandLawId, KeepOffMyLandIconPath);
            SyncWithCurrentWorldLaws(ArmyNoCrossForeignLawId);
            SyncWithCurrentWorldLaws(KeepOffMyLandLawId);
        }

        private static void RegisterOrUpdateLaw(WorldLawLibrary library, string id, string iconPath)
        {
            WorldLawAsset law = library.get(id);
            if (law == null)
            {
                law = new WorldLawAsset
                {
                    id = id,
                    group_id = DiplomacyGroupId,
                    icon_path = iconPath,
                    default_state = false,
                    can_turn_off = true
                };
                library.add(law);
                return;
            }

            law.group_id = DiplomacyGroupId;
            law.icon_path = iconPath;
            law.can_turn_off = true;
        }

        private static void SyncWithCurrentWorldLaws(string id)
        {
            var world = World.world;
            var worldLaws = world?.world_laws;
            var library = AssetManager.world_laws_library;
            if (worldLaws == null || library == null)
                return;

            WorldLawAsset law = library.get(id);
            if (law == null)
                return;

            worldLaws.add(new PlayerOptionData(law.id)
            {
                boolVal = law.default_state,
                on_switch = law.on_state_change
            });
            worldLaws.updateCaches();
        }
    }
}
