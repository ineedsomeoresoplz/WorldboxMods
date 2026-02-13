using System.Collections.Generic;
using UnityEngine;

namespace XaviiBetterFamiliesMod.Code.Utils
{
    internal static class KinshipUtils
    {
        private struct ActorDepth
        {
            public Actor Actor;
            public int Depth;

            public ActorDepth(Actor actor, int depth)
            {
                Actor = actor;
                Depth = depth;
            }
        }

        public static bool AreTilesOnSameIsland(WorldTile tileA, WorldTile tileB)
        {
            if (tileA == null || tileB == null)
                return false;
            if (tileA.region == null || tileB.region == null)
                return false;
            if (tileA.region.island == null || tileB.region.island == null)
                return false;
            return tileA.region.island == tileB.region.island;
        }

        public static bool IsRelated(Actor actor, Actor target)
        {
            if (actor == null || target == null || actor.isRekt() || target.isRekt())
                return false;
            if (actor == target)
                return true;
            if (actor.isChildOf(target) || actor.isParentOf(target))
                return true;
            if (actor.hasFamily() && target.hasFamily() && actor.family == target.family && actor.isSapient())
                return true;
            return AreCloseRelatives(actor, target, 4);
        }

        public static bool AreCloseRelatives(Actor actor, Actor target, int maxCombinedDepth)
        {
            if (actor == null || target == null || actor.isRekt() || target.isRekt())
                return false;
            Dictionary<long, int> actorAncestors = BuildAncestorDepthMap(actor, maxCombinedDepth);
            Dictionary<long, int> targetAncestors = BuildAncestorDepthMap(target, maxCombinedDepth);
            foreach (KeyValuePair<long, int> pair in actorAncestors)
            {
                int targetDepth;
                if (!targetAncestors.TryGetValue(pair.Key, out targetDepth))
                    continue;
                if (pair.Value + targetDepth <= maxCombinedDepth)
                    return true;
            }
            return false;
        }

        public static List<Actor> GetLivingParents(Actor actor)
        {
            List<Actor> result = new List<Actor>();
            if (actor == null || actor.isRekt())
                return result;
            foreach (Actor parent in actor.getParents())
            {
                if (parent != null && parent.isAlive() && !parent.isRekt())
                    result.Add(parent);
            }
            return result;
        }

        public static List<Actor> GetSiblings(Actor actor)
        {
            List<Actor> result = new List<Actor>();
            if (actor == null || actor.isRekt() || World.world == null)
                return result;
            IEnumerable<Actor> source = actor.hasFamily() ? actor.family.units : World.world.units.units_only_alive;
            foreach (Actor candidate in source)
            {
                if (candidate == null || candidate == actor || candidate.isRekt())
                    continue;
                if (SharesParent(actor, candidate))
                    result.Add(candidate);
            }
            return result;
        }

        public static List<Actor> GetGrandParents(Actor actor)
        {
            List<Actor> result = new List<Actor>();
            HashSet<long> added = new HashSet<long>();
            List<Actor> parents = GetLivingParents(actor);
            for (int i = 0; i < parents.Count; i++)
            {
                List<Actor> grandParents = GetLivingParents(parents[i]);
                for (int j = 0; j < grandParents.Count; j++)
                {
                    Actor grandParent = grandParents[j];
                    if (grandParent == null || grandParent.isRekt())
                        continue;
                    long id = grandParent.data.id;
                    if (added.Add(id))
                        result.Add(grandParent);
                }
            }
            return result;
        }

        public static bool SharesParent(Actor actor, Actor target)
        {
            if (actor == null || target == null || actor.isRekt() || target.isRekt())
                return false;
            long actorParent1 = actor.data.parent_id_1;
            long actorParent2 = actor.data.parent_id_2;
            long targetParent1 = target.data.parent_id_1;
            long targetParent2 = target.data.parent_id_2;
            if (actorParent1 != -1 && (actorParent1 == targetParent1 || actorParent1 == targetParent2))
                return true;
            return actorParent2 != -1 && (actorParent2 == targetParent1 || actorParent2 == targetParent2);
        }

        public static Actor FindBestGuardian(Actor child)
        {
            if (child == null || child.isRekt())
                return null;
            List<Actor> parents = GetLivingParents(child);
            Actor parentCandidate = ChooseClosestOnIsland(child, parents);
            if (parentCandidate != null)
                return parentCandidate;
            if (child.hasFamily())
            {
                Actor best = null;
                float bestScore = float.MinValue;
                for (int i = 0; i < child.family.units.Count; i++)
                {
                    Actor candidate = child.family.units[i];
                    if (candidate == null || candidate == child || candidate.isRekt() || !candidate.isAlive() || !candidate.isAdult())
                        continue;
                    float score = ScoreGuardian(child, candidate);
                    if (score > bestScore)
                    {
                        best = candidate;
                        bestScore = score;
                    }
                }
                if (best != null)
                    return best;
            }
            if (child.hasCity() && child.city.leader != null && child.city.leader.isAlive() && !child.city.leader.isRekt())
                return child.city.leader;
            if (child.hasFamily() && child.family.hasFounders())
            {
                Actor founder = child.family.getRandomFounder();
                if (founder != null && founder.isAlive() && !founder.isRekt())
                    return founder;
            }
            return null;
        }

        private static Actor ChooseClosestOnIsland(Actor child, List<Actor> candidates)
        {
            if (child == null || child.isRekt() || child.current_tile == null)
                return null;
            Actor best = null;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < candidates.Count; i++)
            {
                Actor candidate = candidates[i];
                if (candidate == null || candidate.isRekt() || !candidate.isAlive() || candidate.current_tile == null)
                    continue;
                if (!AreTilesOnSameIsland(candidate.current_tile, child.current_tile))
                    continue;
                float distance = child.distanceToObjectTarget(candidate);
                if (distance < bestDistance)
                {
                    best = candidate;
                    bestDistance = distance;
                }
            }
            return best;
        }

        private static float ScoreGuardian(Actor child, Actor candidate)
        {
            float score = 0f;
            if (child.hasCity() && candidate.hasCity() && child.city == candidate.city)
                score += 30f;
            if (AreTilesOnSameIsland(child.current_tile, candidate.current_tile))
                score += 12f;
            if (candidate.hasHouse())
                score += 5f;
            if (candidate.hasLover())
                score += 2f;
            if (candidate.isCityLeader())
                score += 9f;
            if (candidate.isKing())
                score += 12f;
            score += candidate.level;
            score += candidate.getHappinessRatio() * 10f;
            if (child.current_tile != null && candidate.current_tile != null)
                score -= Mathf.Clamp(child.distanceToObjectTarget(candidate) * 0.18f, 0f, 20f);
            return score;
        }

        private static Dictionary<long, int> BuildAncestorDepthMap(Actor actor, int maxDepth)
        {
            Dictionary<long, int> result = new Dictionary<long, int>();
            if (actor == null || actor.isRekt())
                return result;
            Queue<ActorDepth> queue = new Queue<ActorDepth>();
            queue.Enqueue(new ActorDepth(actor, 0));
            while (queue.Count > 0)
            {
                ActorDepth node = queue.Dequeue();
                Actor current = node.Actor;
                int depth = node.Depth;
                if (current == null || current.isRekt())
                    continue;
                long id = current.data.id;
                int existingDepth;
                if (result.TryGetValue(id, out existingDepth) && existingDepth <= depth)
                    continue;
                result[id] = depth;
                if (depth >= maxDepth)
                    continue;
                Actor parent1 = GetUnitForAncestry(current.data.parent_id_1);
                Actor parent2 = GetUnitForAncestry(current.data.parent_id_2);
                if (parent1 != null)
                    queue.Enqueue(new ActorDepth(parent1, depth + 1));
                if (parent2 != null)
                    queue.Enqueue(new ActorDepth(parent2, depth + 1));
            }
            return result;
        }

        private static Actor GetUnitForAncestry(long id)
        {
            if (id == -1 || World.world == null)
                return null;
            Actor actor = World.world.units.get(id);
            if (actor == null || actor.isRekt())
                return null;
            return actor;
        }
    }
}
