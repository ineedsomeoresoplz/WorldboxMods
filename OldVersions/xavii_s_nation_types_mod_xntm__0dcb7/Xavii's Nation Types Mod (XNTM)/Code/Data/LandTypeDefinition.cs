using System;
using System.Globalization;

namespace XNTM.Code.Data
{
    
    
    
    public sealed class LandTypeDefinition
    {
        public string Id { get; }
        public string DisplayNameKey { get; }
        public string DescriptionKey { get; }
        public int MinPopulation { get; }
        public float HousingMultiplier { get; }
        public int WarriorSlotCap { get; }
        public float BuildSpeedMultiplier { get; }
        public int LoyaltyFlatModifier { get; }
        public bool RequiresOverlordNation { get; }

        public LandTypeDefinition(
            string id,
            string displayNameKey,
            string descriptionKey,
            int minPopulation,
            float housingMultiplier,
            int warriorSlotCap,
            float buildSpeedMultiplier,
            int loyaltyFlatModifier,
            bool requiresOverlordNation = false)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            DisplayNameKey = displayNameKey;
            DescriptionKey = descriptionKey;
            MinPopulation = Math.Max(0, minPopulation);
            HousingMultiplier = housingMultiplier;
            WarriorSlotCap = warriorSlotCap;
            BuildSpeedMultiplier = buildSpeedMultiplier;
            LoyaltyFlatModifier = loyaltyFlatModifier;
            RequiresOverlordNation = requiresOverlordNation;
        }

        public string GetLocalizedName()
        {
            if (!string.IsNullOrEmpty(DisplayNameKey) && LocalizedTextManager.stringExists(DisplayNameKey))
                return LocalizedTextManager.getText(DisplayNameKey);
            return FormatIdentifier(Id);
        }

        private static string FormatIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return string.Empty;
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            string[] chunks = identifier.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chunks.Length; i++)
                chunks[i] = textInfo.ToTitleCase(chunks[i].ToLowerInvariant());
            return string.Join(" ", chunks);
        }
    }
}
