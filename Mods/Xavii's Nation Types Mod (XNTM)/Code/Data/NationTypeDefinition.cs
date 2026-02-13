using System;
using System.Globalization;
using UnityEngine;

namespace XNTM.Code.Data
{
    public enum NationSuccessionMode
    {
        RoyalLine,
        Elective,
        Religious,
        Council,
        AgeRule,
        WealthRule,
        Random,
        None
    }

    public sealed class NationTypeDefinition
    {
        public string Id { get; }
        public string DisplayNameKey { get; }
        public string RulerTitleKey { get; }
        public string HeirTitleKey { get; }
        public NationSuccessionMode SuccessionMode { get; }
        public string DescriptionKey { get; }
        public string FemaleRulerTitleKey { get; }

        public NationTypeDefinition(string id, string displayNameKey, string rulerTitleKey, string heirTitleKey, NationSuccessionMode successionMode, string descriptionKey, string femaleRulerTitleKey = null)
        {
            Id = id;
            DisplayNameKey = displayNameKey;
            RulerTitleKey = rulerTitleKey;
            HeirTitleKey = heirTitleKey;
            SuccessionMode = successionMode;
            DescriptionKey = descriptionKey;
            FemaleRulerTitleKey = femaleRulerTitleKey;
        }

        public string GetLocalizedName()
        {
            if (!string.IsNullOrEmpty(DisplayNameKey) && LocalizedTextManager.stringExists(DisplayNameKey))
                return LocalizedTextManager.getText(DisplayNameKey);
            return FormatIdentifier(Id);
        }

        public string GetLocalizedRulerTitle(Actor ruler = null)
        {
            string key = DetermineRulerTitleKey(ruler);
            return GetLocalizedText(key, "Ruler");
        }

        public string DetermineRulerTitleKey(Actor ruler)
        {
            if (ruler?.isSexFemale() == true && !string.IsNullOrEmpty(FemaleRulerTitleKey))
                return FemaleRulerTitleKey;
            return RulerTitleKey;
        }

        public string DetermineRulerTitleKey(bool isFemale)
        {
            if (isFemale && !string.IsNullOrEmpty(FemaleRulerTitleKey))
                return FemaleRulerTitleKey;
            return RulerTitleKey;
        }

        public string GetLocalizedRulerTitle(bool isFemale)
        {
            string key = DetermineRulerTitleKey(isFemale);
            return GetLocalizedText(key, "Ruler");
        }

        private string GetLocalizedText(string key, string fallback)
        {
            if (string.IsNullOrEmpty(key))
                return fallback;
            if (LocalizedTextManager.stringExists(key))
                return LocalizedTextManager.getText(key);
            return fallback;
        }

        public string GetLocalizedHeirTitle()
        {
            return GetLocalizedText(HeirTitleKey, "Heir");
        }

        public string GetLocalizedDescription()
        {
            if (string.IsNullOrEmpty(DescriptionKey))
                return string.Empty;
            if (LocalizedTextManager.stringExists(DescriptionKey))
                return LocalizedTextManager.getText(DescriptionKey);
            return string.Empty;
        }

        private static string FormatIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return string.Empty;
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            string[] chunks = identifier.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i] = textInfo.ToTitleCase(chunks[i].ToLowerInvariant());
            }
            return string.Join(" ", chunks);
        }
    }
}
