using NeoModLoader;
using NeoModLoader.api;
using NeoModLoader.General;
using NeoModLoader.services;

namespace NoRaceRestrictions
{
    public static class WorldLaws
    {
        public static bool InterracialRomanceEnabled { get; private set; }
        // public static WorldLawAsset Law_Interracial_Romance { get; private set; }

        public static void Init()
        {
            var Law_Interracial_Romance = new WorldLawAsset
            {
                id = "Interracial_Romance",
                group_id = "units",
                icon_path = "ui/icons/InterracialRomance",
                default_state = false,
                can_turn_off = true,
                on_state_change = new PlayerOptionAction(opt =>
                {
                    InterracialRomanceEnabled = opt.boolVal;
                })
            };

            AssetManager.world_laws_library.add(Law_Interracial_Romance);
        }
    }
}