using HarmonyLib;
using UnityEngine;
using XNTM.Code.Features.BetterWars;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(DiplomacyHelpers), nameof(DiplomacyHelpers.getWarTarget))]
    public static class DiplomacyHelpersWarTargetPatch
    {
        private static void Postfix(Kingdom pInitiatorKingdom, ref Kingdom __result)
        {
            if (__result != null && __result.isAlive())
                return;
            if (pInitiatorKingdom == null || !pInitiatorKingdom.isAlive())
                return;
            Kingdom best = null;
            float bestScore = float.MinValue;
            using (ListPool<Kingdom> neutral = World.world.wars.getNeutralKingdoms(pInitiatorKingdom, true, false))
            {
                for (int i = 0; i < neutral.Count; i++)
                {
                    Kingdom candidate = neutral[i];
                    if (candidate == null || !candidate.isAlive() || candidate == pInitiatorKingdom)
                        continue;
                    float score = candidate.countCities() * 2f + candidate.units.Count * 0.5f + candidate.getRenown();
                    if (candidate.hasEnemies())
                        score -= 6f;
                    if (pInitiatorKingdom.isOpinionTowardsKingdomGood(candidate))
                        score -= 5f;
                    score -= DistancePenalty(pInitiatorKingdom, candidate);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = candidate;
                    }
                }
            }
            __result = best;
        }

        private static float DistancePenalty(Kingdom a, Kingdom b)
        {
            WorldTile ta = a?.capital?.getTile();
            WorldTile tb = b?.capital?.getTile();
            if (ta == null || tb == null)
                return 0f;
            float dx = ta.pos.x - tb.pos.x;
            float dy = ta.pos.y - tb.pos.y;
            return Mathf.Sqrt(dx * dx + dy * dy) * 0.02f;
        }
    }

    [HarmonyPatch(typeof(DiplomacyHelpers), nameof(DiplomacyHelpers.getAllianceTarget))]
    public static class DiplomacyHelpersAllianceTargetPatch
    {
        private static void Postfix(Kingdom pKingdomStarter, ref Kingdom __result)
        {
            if (pKingdomStarter == null || !pKingdomStarter.isAlive() || pKingdomStarter.isSupreme())
                return;
            if (__result != null && __result.isAlive())
            {
                if (BetterWarsManager.AreKingdomsAllianceCompatible(pKingdomStarter, __result))
                    return;
                __result = null;
            }
            Kingdom best = null;
            float bestScore = float.MinValue;
            using (ListPool<Kingdom> neutral = World.world.wars.getNeutralKingdoms(pKingdomStarter, true, true))
            {
                for (int i = 0; i < neutral.Count; i++)
                {
                    Kingdom candidate = neutral[i];
                    if (candidate == null || !candidate.isAlive() || candidate == pKingdomStarter || candidate.isSupreme())
                        continue;
                    if (candidate.hasEnemies() || candidate.getAlliance() != null)
                        continue;
                    if (!candidate.hasKing() && !NationTypeManager.IsCouncilRepublic(candidate))
                        continue;
                    if (NationTypeManager.IsCouncilRepublic(candidate) && CouncilManager.GetRulers(candidate).Count == 0)
                        continue;
                    if (!BetterWarsManager.AreKingdomsAllianceCompatible(pKingdomStarter, candidate))
                        continue;
                    float score = candidate.getRenown() + candidate.countCities() * 3f + candidate.units.Count * 0.25f;
                    if (!DiplomacyHelpers.areKingdomsClose(candidate, pKingdomStarter))
                        score -= 12f;
                    score -= DistancePenalty(pKingdomStarter, candidate);
                    if (pKingdomStarter.isOpinionTowardsKingdomGood(candidate))
                        score += 6f;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = candidate;
                    }
                }
            }
            __result = best;
        }

        private static float DistancePenalty(Kingdom a, Kingdom b)
        {
            WorldTile ta = a?.capital?.getTile();
            WorldTile tb = b?.capital?.getTile();
            if (ta == null || tb == null)
                return 0f;
            float dx = ta.pos.x - tb.pos.x;
            float dy = ta.pos.y - tb.pos.y;
            return Mathf.Sqrt(dx * dx + dy * dy) * 0.02f;
        }
    }
}
