using UnityEngine;

namespace XWASM.Code.Content
{
    public static class TraitRegistry
    {
        public const string SapientTraitId = "xwasm_sapient";
        public const string SapientSpeciesTraitId = "xwasm_sapient_species";
        public const string GroupId = "xwasm_sapience";
        public const string DefaultKingdomId = "human";

        public static void Register()
        {
            EnsureGroups();
            RegisterActorTrait();
            RegisterSubspeciesTrait();
        }

        private static void EnsureGroups()
        {
            if (!AssetManager.trait_groups.has(GroupId))
            {
                var group = new ActorTraitGroupAsset
                {
                    id = GroupId,
                    name = "Sapience",
                    color = "#7fd7ff"
                };
                AssetManager.trait_groups.add(group);
            }

            if (!AssetManager.subspecies_trait_groups.has(GroupId))
            {
                var group = new SubspeciesTraitGroupAsset
                {
                    id = GroupId,
                    name = "Sapience",
                    color = "#7fd7ff"
                };
                AssetManager.subspecies_trait_groups.add(group);
            }
        }

        private static void RegisterActorTrait()
        {
            var trait = AssetManager.traits.has(SapientTraitId) ? AssetManager.traits.get(SapientTraitId) : new ActorTrait();
            trait.id = SapientTraitId;
            trait.group_id = GroupId;
            trait.path_icon = "ui/icons/iconBrain";
            trait.spawn_random_trait_allowed = false;
            trait.rate_birth = 0;
            trait.rate_inherit = 0;
            trait.rate_acquire_grow_up = 0;
            trait.acquire_grow_up_sapient_only = false;
            trait.has_localized_id = true;
            trait.has_description_1 = true;
            trait.has_description_2 = false;
            trait.special_locale_description = $"{trait.typed_id}_{SapientTraitId}_info";
            trait.special_locale_description_2 = string.Empty;
            trait.rarity = Rarity.R1_Rare;
            trait.likeability = 0.25f;
            trait.can_be_cured = false;
            trait.can_be_removed_by_divine_light = true;
            trait.can_be_removed_by_accelerated_healing = true;
            trait.can_be_removed = true;
            trait.can_be_given = true;
            trait.base_stats_meta ??= new BaseStats();
            AssetManager.traits.add(trait);
        }

        private static void RegisterSubspeciesTrait()
        {
            var trait = AssetManager.subspecies_traits.has(SapientSpeciesTraitId) ? AssetManager.subspecies_traits.get(SapientSpeciesTraitId) : new SubspeciesTrait();
            trait.id = SapientSpeciesTraitId;
            trait.group_id = GroupId;
            trait.path_icon = "ui/icons/iconBrain";
            trait.spawn_random_trait_allowed = false;
            trait.in_mutation_pot_add = false;
            trait.in_mutation_pot_remove = false;
            trait.has_localized_id = true;
            trait.has_description_1 = true;
            trait.has_description_2 = false;
            trait.special_locale_description = $"{trait.typed_id}_{SapientSpeciesTraitId}_info";
            trait.special_locale_description_2 = string.Empty;
            trait.rarity = Rarity.R1_Rare;
            trait.base_stats_meta ??= new BaseStats();
            trait.base_stats_meta.addTag("has_sapience");
            AssetManager.subspecies_traits.add(trait);
        }
    }
}
