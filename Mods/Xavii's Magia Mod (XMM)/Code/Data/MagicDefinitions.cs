using ai.behaviours;

namespace XaviiMagiaMod.Code.Data
{
    public sealed class MagicSpellDefinition
    {
        public string Id { get; }
        public AttackAction Action { get; }
        public CastTarget CastTarget { get; }
        public CastEntity CastEntity { get; }
        public float Chance { get; }
        public int ManaDrain { get; }
        public int RequiredLevel { get; }
        public float Cooldown { get; }
        public float RangeBonus { get; }
        public float RangeFalloffDistance { get; }
        public float MinDistance { get; }
        public float HealthRatio { get; }
        public float ChargeDuration { get; }
        public bool IsAttackSpell { get; }
        public bool CanBeUsedInCombat { get; }

        public MagicSpellDefinition(
            string id,
            AttackAction action,
            CastTarget castTarget,
            CastEntity castEntity,
            float chance,
            int manaDrain,
            int requiredLevel = 1,
            float cooldown = 5f,
            float rangeBonus = 0f,
            float rangeFalloffDistance = 6f,
            bool isAttackSpell = true,
            bool canBeUsedInCombat = true,
            float minDistance = 0f,
            float healthRatio = 0f,
            float chargeDuration = 0f)
        {
            Id = id;
            Action = action;
            CastTarget = castTarget;
            CastEntity = castEntity;
            Chance = chance;
            ManaDrain = manaDrain;
            RequiredLevel = requiredLevel;
            Cooldown = cooldown;
            RangeBonus = rangeBonus;
            RangeFalloffDistance = rangeFalloffDistance;
            MinDistance = minDistance;
            HealthRatio = healthRatio;
            ChargeDuration = chargeDuration;
            IsAttackSpell = isAttackSpell;
            CanBeUsedInCombat = canBeUsedInCombat;
        }
    }

    internal sealed class MagicTypeDefinition
    {
        public string Id { get; }
        public string TraitId { get; }
        public string IconPath { get; }
        public bool AllowAutomatic { get; }
        public string[] SpellIds { get; set; }

        public MagicTypeDefinition(string id, string traitId, string iconPath, bool allowAutomatic = true)
        {
            Id = id;
            TraitId = traitId;
            IconPath = iconPath;
            AllowAutomatic = allowAutomatic;
        }
    }

    internal sealed class MageRankDefinition
    {
        public string TraitId { get; }
        public int MinLevel { get; }
        public int MinKills { get; }
        public string LocaleId => $"trait_{TraitId}";
        public string DescriptionLocaleId => $"{LocaleId}_info";

        public MageRankDefinition(string traitId, int minLevel, int minKills)
        {
            TraitId = traitId;
            MinLevel = minLevel;
            MinKills = minKills;
        }
    }
}
