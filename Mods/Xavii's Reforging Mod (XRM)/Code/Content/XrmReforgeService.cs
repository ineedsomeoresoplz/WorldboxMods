using System.Collections.Generic;

namespace XRM.Code.Content
{
    internal static class XrmReforgeService
    {
        public static bool ApplyReforge(Item item, ICollection<string> selectedBuffIds)
        {
            if (item == null || item.getAsset() == null)
            {
                return false;
            }
            if (!XrmBuffRegistry.EnsureInitialized())
            {
                return false;
            }

            HashSet<string> approvedBuffs = new HashSet<string>();
            EquipmentType equipmentType = item.getAsset().equipment_type;

            if (selectedBuffIds != null)
            {
                foreach (string id in selectedBuffIds)
                {
                    XrmBuffDefinition buff = XrmBuffRegistry.GetBuff(id);
                    if (buff != null && buff.AppliesTo(equipmentType))
                    {
                        approvedBuffs.Add(id);
                    }
                }
            }

            item.reforge(0);

            foreach (string id in approvedBuffs)
            {
                item.addMod(id);
            }

            List<XrmCollisionDefinition> collisions = XrmBuffRegistry.GetTriggeredCollisions(approvedBuffs);
            for (int i = 0; i < collisions.Count; i++)
            {
                item.addMod(collisions[i].PenaltyId);
            }

            item.addMod("divine_rune");
            item.calculateValues();

            Actor actor = item.getActor();
            if (actor != null && !actor.isRekt())
            {
                actor.setStatsDirty();
            }

            return true;
        }

        public static bool ApplyVanillaReforge(Item item, ICollection<string> selectedModifierIds)
        {
            if (item == null || item.getAsset() == null)
            {
                return false;
            }

            if (AssetManager.items_modifiers == null)
            {
                return false;
            }

            string poolToken = XrmBuffRegistry.GetPoolToken(item.getAsset().equipment_type);
            if (string.IsNullOrEmpty(poolToken))
            {
                return false;
            }

            HashSet<string> approvedModifiers = new HashSet<string>();
            if (selectedModifierIds != null)
            {
                foreach (string id in selectedModifierIds)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        continue;
                    }

                    ItemModAsset modifier = AssetManager.items_modifiers.get(id);
                    if (modifier == null || !modifier.mod_can_be_given)
                    {
                        continue;
                    }

                    if (modifier.id.StartsWith("xrm_"))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(modifier.pool) || !modifier.pool.Contains(poolToken))
                    {
                        continue;
                    }

                    approvedModifiers.Add(modifier.id);
                }
            }

            item.reforge(0);

            foreach (string id in approvedModifiers)
            {
                item.addMod(id);
            }

            item.addMod("divine_rune");
            item.calculateValues();

            Actor actor = item.getActor();
            if (actor != null && !actor.isRekt())
            {
                actor.setStatsDirty();
            }

            return true;
        }
    }
}
