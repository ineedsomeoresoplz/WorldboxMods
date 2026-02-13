namespace XaviiMagiaMod.Code.Extensions
{
    /// <summary>
    /// Provides helper extensions for <see cref="BaseStats"/> to mirror dictionary semantics.
    /// </summary>
    internal static class BaseStatsExtensions
    {
        /// <summary>
        /// Attempts to read a stat value without raising if the stat is missing.
        /// </summary>
        /// <param name="stats">The stats container to query.</param>
        /// <param name="statId">The stat identifier.</param>
        /// <param name="value">Output stat value if found; otherwise zero.</param>
        /// <returns>True when the stat existed and a value was returned.</returns>
        public static bool TryGetValue(this BaseStats stats, string statId, out float value)
        {
            value = 0f;
            if (stats == null || string.IsNullOrEmpty(statId))
                return false;

            if (!stats.hasStat(statId))
                return false;

            value = stats[statId];
            return true;
        }
    }
}
