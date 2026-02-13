using System.Collections.Generic;

namespace XRM.Code.Content
{
    internal static class XrmBuffRegistry
    {
        private static bool _initialized;
        private static readonly Dictionary<string, XrmBuffDefinition> _buffById = new Dictionary<string, XrmBuffDefinition>();
        private static readonly List<XrmBuffDefinition> _buffs = new List<XrmBuffDefinition>();
        private static readonly List<XrmCollisionDefinition> _collisions = new List<XrmCollisionDefinition>();

        public static bool EnsureInitialized()
        {
            if (_initialized)
            {
                return true;
            }
            if (AssetManager.items_modifiers == null)
            {
                return false;
            }

            BuildBuffs();
            BuildCollisions();
            RegisterBuffAssets();
            RegisterCollisionAssets();
            _initialized = true;
            return true;
        }

        public static string GetPoolToken(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Weapon:
                    return "weapon";
                case EquipmentType.Ring:
                case EquipmentType.Amulet:
                    return "accessory";
                default:
                    return "armor";
            }
        }

        public static XrmBuffDefinition GetBuff(string id)
        {
            EnsureInitialized();
            XrmBuffDefinition buff;
            return id != null && _buffById.TryGetValue(id, out buff) ? buff : null;
        }

        public static List<XrmBuffDefinition> GetAvailableBuffs(EquipmentType equipmentType)
        {
            EnsureInitialized();
            List<XrmBuffDefinition> result = new List<XrmBuffDefinition>();
            for (int i = 0; i < _buffs.Count; i++)
            {
                XrmBuffDefinition buff = _buffs[i];
                if (buff.AppliesTo(equipmentType))
                {
                    result.Add(buff);
                }
            }
            return result;
        }

        public static List<XrmBuffDefinition> GetAllBuffs()
        {
            EnsureInitialized();
            return new List<XrmBuffDefinition>(_buffs);
        }

        public static List<XrmCollisionDefinition> GetTriggeredCollisions(ISet<string> selectedIds)
        {
            EnsureInitialized();
            List<XrmCollisionDefinition> result = new List<XrmCollisionDefinition>();
            for (int i = 0; i < _collisions.Count; i++)
            {
                XrmCollisionDefinition collision = _collisions[i];
                if (collision.IsTriggered(selectedIds))
                {
                    result.Add(collision);
                }
            }
            return result;
        }

        private static void BuildBuffs()
        {
            if (_buffs.Count > 0)
            {
                return;
            }

            AddBuff(
                "xrm_emberforged",
                "xrm_mod_emberforged",
                "weapon",
                Rarity.R2_Epic,
                3,
                "Damage +2, status chance +8%, accuracy -1",
                new Dictionary<string, float> { { "damage", 2f }, { "status_chance", 0.08f }, { "accuracy", -1f } },
                ActionLibrary.addBurningEffectOnTarget);

            AddBuff(
                "xrm_frostweld",
                "xrm_mod_frostweld",
                "weapon",
                Rarity.R2_Epic,
                3,
                "Armor +1, status chance +8%, attack speed -1",
                new Dictionary<string, float> { { "armor", 1f }, { "status_chance", 0.08f }, { "attack_speed", -1f } },
                ActionLibrary.addFrozenEffectOnTarget20);

            AddBuff(
                "xrm_tempest_edge",
                "xrm_mod_tempest_edge",
                "weapon",
                Rarity.R2_Epic,
                3,
                "Attack speed +1.5, speed +1, armor -1",
                new Dictionary<string, float> { { "attack_speed", 1.5f }, { "speed", 1f }, { "armor", -1f } },
                ActionLibrary.addStunnedEffectOnTarget20);

            AddBuff(
                "xrm_venomkiss",
                "xrm_mod_venomkiss",
                "weapon",
                Rarity.R2_Epic,
                3,
                "Status chance +12%, damage +1, diplomacy -1",
                new Dictionary<string, float> { { "status_chance", 0.12f }, { "damage", 1f }, { "diplomacy", -1f } },
                ActionLibrary.addPoisonedEffectOnTarget);

            AddBuff(
                "xrm_stoneguard",
                "xrm_mod_stoneguard",
                "armor",
                Rarity.R2_Epic,
                3,
                "Armor +4, health +60, speed -1.5",
                new Dictionary<string, float> { { "armor", 4f }, { "health", 60f }, { "speed", -1.5f } },
                null);

            AddBuff(
                "xrm_windstep",
                "xrm_mod_windstep",
                "armor,accessory",
                Rarity.R1_Rare,
                2,
                "Speed +3, attack speed +0.7, armor -1",
                new Dictionary<string, float> { { "speed", 3f }, { "attack_speed", 0.7f }, { "armor", -1f } },
                null);

            AddBuff(
                "xrm_bloodletter",
                "xrm_mod_bloodletter",
                "weapon",
                Rarity.R3_Legendary,
                4,
                "Damage +3, crit chance +8%, health -45",
                new Dictionary<string, float> { { "damage", 3f }, { "critical_chance", 0.08f }, { "health", -45f } },
                null);

            AddBuff(
                "xrm_bulwark",
                "xrm_mod_bulwark",
                "armor",
                Rarity.R2_Epic,
                3,
                "Armor +5, stamina +15, attack speed -0.8",
                new Dictionary<string, float> { { "armor", 5f }, { "stamina", 15f }, { "attack_speed", -0.8f } },
                null);

            AddBuff(
                "xrm_duelist",
                "xrm_mod_duelist",
                "weapon,accessory",
                Rarity.R2_Epic,
                3,
                "Attack speed +2, crit chance +6%, health -30",
                new Dictionary<string, float> { { "attack_speed", 2f }, { "critical_chance", 0.06f }, { "health", -30f } },
                null);

            AddBuff(
                "xrm_siegebreaker",
                "xrm_mod_siegebreaker",
                "weapon",
                Rarity.R2_Epic,
                3,
                "Damage +4, knockback +1.5, speed -1",
                new Dictionary<string, float> { { "damage", 4f }, { "knockback", 1.5f }, { "speed", -1f } },
                null);

            AddBuff(
                "xrm_watchful",
                "xrm_mod_watchful",
                "accessory",
                Rarity.R1_Rare,
                2,
                "Accuracy +2, crit chance +3%, damage -1",
                new Dictionary<string, float> { { "accuracy", 2f }, { "critical_chance", 0.03f }, { "damage", -1f } },
                null);

            AddBuff(
                "xrm_pathfinder",
                "xrm_mod_pathfinder",
                "armor,accessory",
                Rarity.R1_Rare,
                2,
                "Speed +2, throwing range +2, armor -1",
                new Dictionary<string, float> { { "speed", 2f }, { "throwing_range", 2f }, { "armor", -1f } },
                null);

            AddBuff(
                "xrm_commander",
                "xrm_mod_commander",
                "accessory",
                Rarity.R2_Epic,
                3,
                "Warfare +2, stewardship +1, attack speed -0.5",
                new Dictionary<string, float> { { "warfare", 2f }, { "stewardship", 1f }, { "attack_speed", -0.5f } },
                null);

            AddBuff(
                "xrm_scholar",
                "xrm_mod_scholar",
                "accessory",
                Rarity.R2_Epic,
                3,
                "Intelligence +3, diplomacy +1, damage -1",
                new Dictionary<string, float> { { "intelligence", 3f }, { "diplomacy", 1f }, { "damage", -1f } },
                null);

            AddBuff(
                "xrm_zeal",
                "xrm_mod_zeal",
                "weapon,accessory",
                Rarity.R2_Epic,
                3,
                "Status chance +10%, crit damage +12%, stewardship -1",
                new Dictionary<string, float> { { "status_chance", 0.10f }, { "critical_damage_multiplier", 0.12f }, { "stewardship", -1f } },
                null);

            AddBuff(
                "xrm_gambit",
                "xrm_mod_gambit",
                "weapon,accessory",
                Rarity.R3_Legendary,
                4,
                "Crit chance +12%, crit damage +25%, accuracy -2",
                new Dictionary<string, float> { { "critical_chance", 0.12f }, { "critical_damage_multiplier", 0.25f }, { "accuracy", -2f } },
                null);

            AddBuff(
                "xrm_frenzy",
                "xrm_mod_frenzy",
                "weapon",
                Rarity.R3_Legendary,
                4,
                "Attack speed +3, damage +2, armor -2",
                new Dictionary<string, float> { { "attack_speed", 3f }, { "damage", 2f }, { "armor", -2f } },
                null);

            AddBuff(
                "xrm_sentinel",
                "xrm_mod_sentinel",
                "armor,accessory",
                Rarity.R2_Epic,
                3,
                "Health +100, armor +2, speed -2",
                new Dictionary<string, float> { { "health", 100f }, { "armor", 2f }, { "speed", -2f } },
                null);

            AddBuff(
                "xrm_shadowstep",
                "xrm_mod_shadowstep",
                "weapon,accessory",
                Rarity.R2_Epic,
                3,
                "Speed +2, attack speed +1, accuracy -1",
                new Dictionary<string, float> { { "speed", 2f }, { "attack_speed", 1f }, { "accuracy", -1f } },
                null);

            AddBuff(
                "xrm_radiant",
                "xrm_mod_radiant",
                "armor,accessory",
                Rarity.R2_Epic,
                3,
                "Health +70, diplomacy +2, crit chance -5%",
                new Dictionary<string, float> { { "health", 70f }, { "diplomacy", 2f }, { "critical_chance", -0.05f } },
                null);

            AddBuff(
                "xrm_decayward",
                "xrm_mod_decayward",
                "armor",
                Rarity.R1_Rare,
                2,
                "Armor +3, status chance +5%, health -25",
                new Dictionary<string, float> { { "armor", 3f }, { "status_chance", 0.05f }, { "health", -25f } },
                null);

            AddBuff(
                "xrm_ironwill",
                "xrm_mod_ironwill",
                "armor,accessory",
                Rarity.R2_Epic,
                3,
                "Stamina +20, warfare +1, speed -1",
                new Dictionary<string, float> { { "stamina", 20f }, { "warfare", 1f }, { "speed", -1f } },
                null);

            AddBuff(
                "xrm_glassedge",
                "xrm_mod_glassedge",
                "weapon",
                Rarity.R3_Legendary,
                5,
                "Damage +5, crit chance +10%, armor -3, health -35",
                new Dictionary<string, float> { { "damage", 5f }, { "critical_chance", 0.10f }, { "armor", -3f }, { "health", -35f } },
                null);

            AddBuff(
                "xrm_lifebloom",
                "xrm_mod_lifebloom",
                "armor,accessory",
                Rarity.R3_Legendary,
                4,
                "Health +120, health multiplier +5%, damage -2",
                new Dictionary<string, float> { { "health", 120f }, { "multiplier_health", 0.05f }, { "damage", -2f } },
                null);

            AddBuff(
                "xrm_runesmith",
                "xrm_mod_runesmith",
                "accessory",
                Rarity.R2_Epic,
                3,
                "Intelligence +2, stewardship +2, speed -1",
                new Dictionary<string, float> { { "intelligence", 2f }, { "stewardship", 2f }, { "speed", -1f } },
                null);

            AddBuff(
                "xrm_skirmisher",
                "xrm_mod_skirmisher",
                "weapon,armor",
                Rarity.R1_Rare,
                2,
                "Speed +2, attack speed +1, health -25",
                new Dictionary<string, float> { { "speed", 2f }, { "attack_speed", 1f }, { "health", -25f } },
                null);

            AddBuff(
                "xrm_leviathan",
                "xrm_mod_leviathan",
                "weapon,armor",
                Rarity.R2_Epic,
                3,
                "Health +90, damage +2, attack speed -1.5",
                new Dictionary<string, float> { { "health", 90f }, { "damage", 2f }, { "attack_speed", -1.5f } },
                null);

            AddBuff(
                "xrm_whirlwind",
                "xrm_mod_whirlwind",
                "weapon",
                Rarity.R3_Legendary,
                4,
                "Attack speed +2.5, speed +2, accuracy -1",
                new Dictionary<string, float> { { "attack_speed", 2.5f }, { "speed", 2f }, { "accuracy", -1f } },
                null);

            AddBuff(
                "xrm_oathbound",
                "xrm_mod_oathbound",
                "armor,accessory",
                Rarity.R2_Epic,
                3,
                "Diplomacy +3, warfare +1, crit damage -10%",
                new Dictionary<string, float> { { "diplomacy", 3f }, { "warfare", 1f }, { "critical_damage_multiplier", -0.10f } },
                null);

            AddBuff(
                "xrm_executioner",
                "xrm_mod_executioner",
                "weapon",
                Rarity.R3_Legendary,
                5,
                "Damage +4, crit damage +20%, diplomacy -2",
                new Dictionary<string, float> { { "damage", 4f }, { "critical_damage_multiplier", 0.20f }, { "diplomacy", -2f } },
                null);
        }

        private static void BuildCollisions()
        {
            if (_collisions.Count > 0)
            {
                return;
            }

            AddCollision(
                "xrm_emberforged",
                "xrm_frostweld",
                "xrm_collision_thermal_crack",
                "xrm_collision_thermal_crack",
                "Damage -2, status chance -6%",
                new Dictionary<string, float> { { "damage", -2f }, { "status_chance", -0.06f } });

            AddCollision(
                "xrm_tempest_edge",
                "xrm_leviathan",
                "xrm_collision_static_drag",
                "xrm_collision_static_drag",
                "Speed -2, attack speed -1",
                new Dictionary<string, float> { { "speed", -2f }, { "attack_speed", -1f } });

            AddCollision(
                "xrm_bloodletter",
                "xrm_shadowstep",
                "xrm_collision_blood_pulse",
                "xrm_collision_blood_pulse",
                "Health -60, crit chance -5%",
                new Dictionary<string, float> { { "health", -60f }, { "critical_chance", -0.05f } });

            AddCollision(
                "xrm_duelist",
                "xrm_sentinel",
                "xrm_collision_posture_break",
                "xrm_collision_posture_break",
                "Attack speed -1, armor -2",
                new Dictionary<string, float> { { "attack_speed", -1f }, { "armor", -2f } });

            AddCollision(
                "xrm_shadowstep",
                "xrm_radiant",
                "xrm_collision_lightwarp",
                "xrm_collision_lightwarp",
                "Accuracy -2, crit chance -4%",
                new Dictionary<string, float> { { "accuracy", -2f }, { "critical_chance", -0.04f } });

            AddCollision(
                "xrm_frenzy",
                "xrm_duelist",
                "xrm_collision_temper_strain",
                "xrm_collision_temper_strain",
                "Stamina -15, speed -1",
                new Dictionary<string, float> { { "stamina", -15f }, { "speed", -1f } });

            AddCollision(
                "xrm_glassedge",
                "xrm_leviathan",
                "xrm_collision_shatter_guard",
                "xrm_collision_shatter_guard",
                "Armor -2, damage -1",
                new Dictionary<string, float> { { "armor", -2f }, { "damage", -1f } });

            AddCollision(
                "xrm_gambit",
                "xrm_watchful",
                "xrm_collision_overfocus",
                "xrm_collision_overfocus",
                "Accuracy -1, crit damage -15%",
                new Dictionary<string, float> { { "accuracy", -1f }, { "critical_damage_multiplier", -0.15f } });

            AddCollision(
                "xrm_zeal",
                "xrm_runesmith",
                "xrm_collision_faith_logic",
                "xrm_collision_faith_logic",
                "Intelligence -2, status chance -5%",
                new Dictionary<string, float> { { "intelligence", -2f }, { "status_chance", -0.05f } });

            AddCollision(
                "xrm_leviathan",
                "xrm_whirlwind",
                "xrm_collision_turbulence",
                "xrm_collision_turbulence",
                "Attack speed -1.5, speed -1",
                new Dictionary<string, float> { { "attack_speed", -1.5f }, { "speed", -1f } });

            AddCollision(
                "xrm_stoneguard",
                "xrm_windstep",
                "xrm_collision_weight_shift",
                "xrm_collision_weight_shift",
                "Speed -1.5, attack speed -0.5",
                new Dictionary<string, float> { { "speed", -1.5f }, { "attack_speed", -0.5f } });

            AddCollision(
                "xrm_bulwark",
                "xrm_skirmisher",
                "xrm_collision_formation_drag",
                "xrm_collision_formation_drag",
                "Speed -1, attack speed -0.8",
                new Dictionary<string, float> { { "speed", -1f }, { "attack_speed", -0.8f } });
        }

        private static void RegisterBuffAssets()
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                XrmBuffDefinition buff = _buffs[i];
                RegisterModifier(buff.Id, buff.NameKey, buff.Pool, buff.Quality, buff.ModRank, buff.Stats, buff.AttackAction);
            }
        }

        private static void RegisterCollisionAssets()
        {
            HashSet<string> registered = new HashSet<string>();
            for (int i = 0; i < _collisions.Count; i++)
            {
                XrmCollisionDefinition collision = _collisions[i];
                if (registered.Contains(collision.PenaltyId))
                {
                    continue;
                }

                RegisterModifier(
                    collision.PenaltyId,
                    collision.PenaltyNameKey,
                    "weapon,accessory,armor",
                    Rarity.R1_Rare,
                    1,
                    collision.PenaltyStats,
                    null);

                registered.Add(collision.PenaltyId);
            }
        }

        private static void RegisterModifier(
            string id,
            string nameKey,
            string pool,
            Rarity quality,
            int modRank,
            Dictionary<string, float> stats,
            AttackAction action)
        {
            if (AssetManager.items_modifiers.get(id) != null)
            {
                return;
            }

            ItemModAsset mod = new ItemModAsset
            {
                id = id,
                mod_type = id,
                translation_key = nameKey,
                pool = pool,
                rarity = 0,
                mod_rank = modRank,
                quality = quality,
                mod_can_be_given = false,
                base_stats = new BaseStats(),
                action_attack_target = action
            };

            if (stats != null)
            {
                foreach (KeyValuePair<string, float> entry in stats)
                {
                    mod.base_stats[entry.Key] = entry.Value;
                }
            }

            AssetManager.items_modifiers.add(mod);
        }

        private static void AddBuff(
            string id,
            string nameKey,
            string pool,
            Rarity quality,
            int modRank,
            string summary,
            Dictionary<string, float> stats,
            AttackAction attackAction)
        {
            XrmBuffDefinition def = new XrmBuffDefinition(id, nameKey, pool, quality, modRank, summary, stats, attackAction);
            _buffs.Add(def);
            _buffById[id] = def;
        }

        private static void AddCollision(
            string firstId,
            string secondId,
            string penaltyId,
            string penaltyNameKey,
            string summary,
            Dictionary<string, float> penaltyStats)
        {
            _collisions.Add(new XrmCollisionDefinition(firstId, secondId, penaltyId, penaltyNameKey, summary, penaltyStats));
        }
    }
}
