using System.Collections.Generic;

namespace XRM.Code.Content
{
    internal sealed class XrmCollisionDefinition
    {
        public readonly string FirstId;
        public readonly string SecondId;
        public readonly string PenaltyId;
        public readonly string PenaltyNameKey;
        public readonly string Summary;
        public readonly Dictionary<string, float> PenaltyStats;

        public XrmCollisionDefinition(
            string firstId,
            string secondId,
            string penaltyId,
            string penaltyNameKey,
            string summary,
            Dictionary<string, float> penaltyStats)
        {
            FirstId = firstId;
            SecondId = secondId;
            PenaltyId = penaltyId;
            PenaltyNameKey = penaltyNameKey;
            Summary = summary;
            PenaltyStats = penaltyStats;
        }

        public bool IsTriggered(ISet<string> selectedIds)
        {
            return selectedIds != null && selectedIds.Contains(FirstId) && selectedIds.Contains(SecondId);
        }
    }
}
