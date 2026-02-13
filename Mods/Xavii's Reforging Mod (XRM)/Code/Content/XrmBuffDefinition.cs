using System.Collections.Generic;

namespace XRM.Code.Content
{
    internal sealed class XrmBuffDefinition
    {
        public readonly string Id;
        public readonly string NameKey;
        public readonly string Pool;
        public readonly Rarity Quality;
        public readonly int ModRank;
        public readonly Dictionary<string, float> Stats;
        public readonly AttackAction AttackAction;
        public readonly string Summary;

        public XrmBuffDefinition(
            string id,
            string nameKey,
            string pool,
            Rarity quality,
            int modRank,
            string summary,
            Dictionary<string, float> stats,
            AttackAction attackAction)
        {
            Id = id;
            NameKey = nameKey;
            Pool = pool;
            Quality = quality;
            ModRank = modRank;
            Summary = summary;
            Stats = stats;
            AttackAction = attackAction;
        }

        public bool AppliesTo(EquipmentType equipmentType)
        {
            string token = XrmBuffRegistry.GetPoolToken(equipmentType);
            return !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(Pool) && Pool.Contains(token);
        }
    }
}
