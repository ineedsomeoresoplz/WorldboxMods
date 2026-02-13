using System.Collections.Generic;

namespace XaviiHistorybookMod.Code.Data
{
    public enum HistoryEventCategory
    {
        Birth,
        Offspring,
        Grandchild,
        Favorite,
        Victory,
        Injury,
        Death,
        Relationship,
        Royalty,
        Action,
        Note,
        Compatibility
    }

    public class HistoryEntry
    {
        public double Timestamp { get; set; }
        public HistoryEventCategory Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long? RelatedUnitId { get; set; }
        public string RelatedName { get; set; }
        public string LocationHint { get; set; }
    }

    public class HistoryRecord
    {
        public long UnitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpeciesId { get; set; } = string.Empty;
        public string SpeciesName { get; set; } = string.Empty;
        public double BornAt { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasEverBeenFavorite { get; set; }
        public string LastLoggedTask { get; set; } = string.Empty;
        public List<HistoryEntry> Events { get; set; } = new List<HistoryEntry>();
    }

    public class HistorybookSaveData
    {
        public Dictionary<long, HistoryRecord> Records { get; set; } = new Dictionary<long, HistoryRecord>();
    }
}
