using System;
using System.Collections.Generic;
using System.Globalization;
using NeoModLoader.General;
using UnityEngine;
using XaviiHistorybookMod.Code.Compatibility;
using XaviiHistorybookMod.Code.Data;
using XaviiHistorybookMod.Code.Managers;

namespace XaviiHistorybookMod.Code
{
    public static class HistorybookEvents
    {
        private static HistorybookManager Manager => HistorybookManager.Instance;

        public static void RecordFavoriteChange(Actor actor, bool before, bool after)
        {
            if (actor == null || Manager == null)
                return;

            var record = Manager.EnsureRecord(actor, out bool isNewRecord);

            if (!after)
            {
                Manager.ClearFavorite(record.UnitId);
                Manager.NotifyChange();
                return;
            }

            Manager.SetFavorite(record.UnitId);
            if (isNewRecord)
                Manager.AddInitialSnapshot(record, actor);

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Favorite,
                Title = "Added to favorites",
                Description = $"{actor.coloredName} joined the Historybook on {Date.getDate(GetNow())}",
                Timestamp = GetNow()
            };

            Manager.AddEvent(record.UnitId, entry, true);
        }

        public static void RecordKillVictory(Actor attacker, Actor victim, AttackType attackType)
        {
            if (attacker == null || victim == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(attacker.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var victimName = victim.coloredName;
            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Victory,
                Title = "Battle won",
                Description = $"Defeated {victimName} ({attackType})",
                RelatedUnitId = victim.getID(),
                RelatedName = victim.getName(),
                Timestamp = GetNow()
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordInjury(Actor actor, float damage, AttackType type)
        {
            if (actor == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            if (damage <= actor.getMaxHealth() * 0.15f)
                return;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Injury,
                Title = "Serious injury",
                Description = $"Took {Mathf.RoundToInt(damage)} damage from {type}",
                Timestamp = GetNow()
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordDeath(Actor actor, AttackType attackType, Actor killer)
        {
            if (actor == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var killerName = killer?.coloredName ?? "an unknown threat";
            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Death,
                Title = "Departed",
                Description = $"Killed by {killerName} ({attackType})",
                Timestamp = GetNow()
            };

            Manager.AddEvent(record.UnitId, entry);
            if (actor.lover != null)
                NotifyLoverLoss(actor.lover, actor);
        }

        public static void RecordNewBirth(Actor child)
        {
            if (child == null || Manager == null)
                return;

            Manager.RecordBirth(child);
            RecordLine(child, 1, HistoryEventCategory.Offspring);
            RecordLine(child, 2, HistoryEventCategory.Grandchild);
        }

        public static void RecordLoverBond(Actor actor, Actor partner)
        {
            if (actor == null || partner == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Relationship,
                Title = "Fell in love",
                Description = $"Started a relationship with {partner.coloredName}",
                RelatedUnitId = partner.getID(),
                RelatedName = partner.getName(),
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordInterracialRomance(Actor actor, Actor partner)
        {
            if (actor == null || partner == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            if (!IsInterracialPair(actor, partner))
                return;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Compatibility,
                Title = "Interracial romance",
                Description = $"Began a relationship with {partner.coloredName} ({GetSpeciesLabel(partner)})",
                RelatedUnitId = partner.getID(),
                RelatedName = partner.getName(),
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordAction(Actor actor, string taskId)
        {
            if (actor == null || Manager == null || string.IsNullOrWhiteSpace(taskId))
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var normalized = taskId.Trim();
            if (string.IsNullOrEmpty(normalized))
                return;

            if (string.Equals(record.LastLoggedTask, normalized, StringComparison.OrdinalIgnoreCase))
                return;

            record.LastLoggedTask = normalized;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Action,
                Title = $"Action: {FormatTaskLabel(normalized)}",
                Description = $"Task {taskId} near {FormatTile(actor.current_tile)}",
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordRoyalAscension(Actor actor, Kingdom kingdom, bool fromLoad)
        {
            if (actor == null || kingdom == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var kingdomName = GetKingdomLabel(kingdom);
            string customTitle = null;
            string nationTypeName = null;
            if (CompatibilityRegistrar.IsInstalled(CompatibilityMod.NationTypes))
            {
                XNTMCompatibility.TryGetTitles(kingdom, actor, out customTitle, out nationTypeName);
            }
            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Royalty,
                Title = fromLoad ? "Restored monarch" : "Ascended monarch",
                Description = fromLoad ? $"Reclaimed rule over {kingdomName}" : $"Ascended to rule {kingdomName}",
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            if (!string.IsNullOrEmpty(customTitle))
            {
                entry.Title = fromLoad ? $"Restored {customTitle}" : $"Ascended as {customTitle}";
                entry.Description = fromLoad
                    ? $"Reclaimed rule over {kingdomName}"
                    : $"Ascended as {customTitle} of {kingdomName}";
            }

            if (!string.IsNullOrEmpty(nationTypeName))
            {
                entry.Description = $"{entry.Description} ({nationTypeName})";
            }

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordOrlSpellCast(Actor actor, SpellAsset spell)
        {
            if (actor == null || spell == null || Manager == null)
                return;

            if (!actor.hasTrait("magic_orl"))
                return;

            var record = Manager.TryGetRecord(actor.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var spellLabel = FormatSpellLabel(spell);
            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Compatibility,
                Title = $"Cast {spellLabel}",
                Description = $"Spell {spell.id}",
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        public static void RecordOrlReincarnation(Actor host, Actor soul)
        {
            if (Manager == null)
                return;

            if (host != null)
            {
                RecordOrlReincarnationInternal(host, soul, true);
            }

            if (soul != null)
            {
                RecordOrlReincarnationInternal(soul, host, false);
            }
        }

        private static void RecordOrlReincarnationInternal(Actor actor, Actor other, bool isNewHost)
        {
            if (actor == null)
                return;

            var record = isNewHost ? Manager.EnsureRecord(actor, out bool _) : Manager.TryGetRecord(actor.getID());
            if (record == null)
                return;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Compatibility,
                Title = isNewHost ? "Orl rebirth" : "Orl departed",
                Description = isNewHost
                    ? $"{other?.coloredName ?? "An Orl soul"} claimed this body" + FormatReincarnationSuffix(actor)
                    : $"Soul transferred to {other?.coloredName ?? "a new host"}",
                Timestamp = GetNow(),
                LocationHint = FormatTile(actor.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry, isNewHost);
        }

        private static string FormatSpellLabel(SpellAsset spell)
        {
            if (spell == null)
                return "Unknown spell";

            if (!string.IsNullOrEmpty(spell.id))
            {
                var key = $"spell_{spell.id}";
                if (LocalizedTextManager.stringExists(key))
                    return LocalizedTextManager.getText(key);
                return spell.id;
            }

            return "Unnamed spell";
        }

        private static string FormatReincarnationSuffix(Actor actor)
        {
            if (actor?.data == null)
                return ".";

            actor.data.get("magic_orl_reincarnations", out int remaining, -1);
            return remaining >= 0 ? $" (Reincarnations remaining: {remaining})" : ".";
        }

        private static void NotifyLoverLoss(Actor lover, Actor lostActor)
        {
            if (lover == null || lostActor == null || Manager == null)
                return;

            var record = Manager.TryGetRecord(lover.getID());
            if (record == null || !record.HasEverBeenFavorite)
                return;

            var entry = new HistoryEntry
            {
                Category = HistoryEventCategory.Relationship,
                Title = "Lover lost",
                Description = $"{lostActor.coloredName} died near {FormatTile(lover.current_tile)}",
                RelatedUnitId = lostActor.getID(),
                RelatedName = lostActor.getName(),
                Timestamp = GetNow(),
                LocationHint = FormatTile(lover.current_tile)
            };

            Manager.AddEvent(record.UnitId, entry);
        }

        private static string FormatTaskLabel(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                return "Unknown action";

            var cleaned = taskId.Replace("_", " ").ToLowerInvariant();
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cleaned);
        }

        private static string GetKingdomLabel(Kingdom kingdom)
        {
            if (kingdom == null)
                return "the kingdom";

            if (!string.IsNullOrWhiteSpace(kingdom.name))
                return kingdom.name;

            if (!string.IsNullOrWhiteSpace(kingdom.data?.name))
                return kingdom.data.name;

            if (!string.IsNullOrWhiteSpace(kingdom.asset?.id))
                return kingdom.asset.id;

            return "the kingdom";
        }

        private static void RecordLine(Actor child, int generation, HistoryEventCategory category)
        {
            var manager = Manager;
            if (manager == null)
                return;

            var childName = child.getName();
            var ancestorIds = generation == 1
                ? new[] { child.data.parent_id_1, child.data.parent_id_2 }
                : CollectAncestorParents(child);

            foreach (var ancestorId in ancestorIds)
            {
                if (ancestorId < 0)
                    continue;

                var ancestor = World.world.units.get(ancestorId);
                if (ancestor == null)
                    continue;

                var record = manager.TryGetRecord(ancestor.getID());
                if (record == null || !record.HasEverBeenFavorite)
                    continue;

                var entry = new HistoryEntry
                {
                    Timestamp = child.data.created_time > 0 ? child.data.created_time : GetNow(),
                    Category = category,
                    Title = generation == 1 ? "New child" : "New grandchild",
                    Description = $"{childName} joined the family near {FormatTile(child.current_tile)}",
                    RelatedUnitId = child.getID(),
                    RelatedName = childName
                };

                manager.AddEvent(record.UnitId, entry);
            }
        }

        private static IEnumerable<long> CollectAncestorParents(Actor child)
        {
            if (child == null)
                yield break;

            var parents = new[] { child.data.parent_id_1, child.data.parent_id_2 };
            foreach (var parentId in parents)
            {
                if (parentId < 0)
                    continue;

                var parent = World.world.units.get(parentId);
                if (parent == null)
                    continue;

                if (parent.data.parent_id_1 >= 0)
                    yield return parent.data.parent_id_1;
                if (parent.data.parent_id_2 >= 0)
                    yield return parent.data.parent_id_2;
            }
        }

        private static bool IsInterracialPair(Actor actor, Actor partner)
        {
            var actorSpecies = actor?.subspecies?.data?.species_id;
            var partnerSpecies = partner?.subspecies?.data?.species_id;

            if (string.IsNullOrEmpty(actorSpecies) || string.IsNullOrEmpty(partnerSpecies))
                return false;

            return !string.Equals(actorSpecies, partnerSpecies, StringComparison.Ordinal);
        }

        private static string GetSpeciesLabel(Actor actor)
        {
            if (actor == null)
                return "Unknown species";

            var localizedName = actor.asset?.getLocalizedName();
            if (!string.IsNullOrWhiteSpace(localizedName))
                return localizedName;

            var speciesId = actor.subspecies?.data?.species_id;
            if (!string.IsNullOrWhiteSpace(speciesId))
                return speciesId;

            return actor.subspecies?.name ?? "Unknown species";
        }

        private static double GetNow()
        {
            return World.world?.getCurWorldTime() ?? 0;
        }

        private static string FormatTile(WorldTile tile)
        {
            if (tile == null)
                return "Unknown location";
            return $"{tile.pos.x},{tile.pos.y}";
        }
    }
}
