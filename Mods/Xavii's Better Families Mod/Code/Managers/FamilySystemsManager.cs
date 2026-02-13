using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using XaviiBetterFamiliesMod.Code.Utils;
using UnityEngine;

namespace XaviiBetterFamiliesMod.Code.Managers
{
    public class FamilySystemsManager : MonoBehaviour
    {
        private struct FamilyEnemyKey : IEquatable<FamilyEnemyKey>
        {
            public long FamilyId;
            public long EnemyId;

            public FamilyEnemyKey(long familyId, long enemyId)
            {
                FamilyId = familyId;
                EnemyId = enemyId;
            }

            public bool Equals(FamilyEnemyKey other)
            {
                return FamilyId == other.FamilyId && EnemyId == other.EnemyId;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is FamilyEnemyKey))
                    return false;
                return Equals((FamilyEnemyKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (FamilyId.GetHashCode() * 397) ^ EnemyId.GetHashCode();
                }
            }
        }

        private struct CachedPrestige
        {
            public float Score;
            public float ExpiresAt;
        }

        private static readonly MethodInfo JustBornMethod = AccessTools.Method(typeof(Actor), "justBorn");

        public static FamilySystemsManager Instance { get; private set; }

        private readonly Dictionary<long, double> _widowCooldownUntil = new Dictionary<long, double>();
        private readonly Dictionary<long, long> _guardianByChild = new Dictionary<long, long>();
        private readonly Dictionary<long, double> _cadetCooldownUntil = new Dictionary<long, double>();
        private readonly Dictionary<FamilyEnemyKey, double> _familyFeudUntil = new Dictionary<FamilyEnemyKey, double>();
        private readonly Dictionary<long, double> _lastDefensePing = new Dictionary<long, double>();
        private readonly Dictionary<long, CachedPrestige> _prestigeByFamily = new Dictionary<long, CachedPrestige>();

        private float _nextGuardianTick;
        private float _nextFeudTick;
        private float _nextMaintenanceTick;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetAllState();
        }

        private void OnEnable()
        {
            MapBox.on_world_loaded += HandleWorldLoaded;
        }

        private void OnDisable()
        {
            MapBox.on_world_loaded -= HandleWorldLoaded;
        }

        private void Update()
        {
            if (World.world == null || World.world.units == null)
                return;
            float now = Time.unscaledTime;
            if (now >= _nextGuardianTick)
            {
                _nextGuardianTick = now + 1.25f;
                TickOrphanCare();
            }
            if (now >= _nextFeudTick)
            {
                _nextFeudTick = now + 0.75f;
                TickFamilyFeuds();
            }
            if (now >= _nextMaintenanceTick)
            {
                _nextMaintenanceTick = now + 5f;
                TickMaintenance();
            }
        }

        public void ResetAllState()
        {
            _widowCooldownUntil.Clear();
            _guardianByChild.Clear();
            _cadetCooldownUntil.Clear();
            _familyFeudUntil.Clear();
            _lastDefensePing.Clear();
            _prestigeByFamily.Clear();
            float now = Time.unscaledTime;
            _nextGuardianTick = now + 1f;
            _nextFeudTick = now + 1f;
            _nextMaintenanceTick = now + 3f;
        }

        public void InvalidateFamily(Family family)
        {
            if (family == null)
                return;
            _prestigeByFamily.Remove(family.id);
        }

        public void OnFamilyChanged(Actor actor, Family oldFamily, Family newFamily)
        {
            if (actor == null || actor.isRekt())
                return;
            if (oldFamily != null)
                InvalidateFamily(oldFamily);
            if (newFamily != null)
                InvalidateFamily(newFamily);
            if (newFamily != null && actor.isSapient() && actor.data.ancestor_family == -1)
                actor.saveOriginFamily(newFamily.id);
            if (actor.isBaby() && newFamily != null)
                _guardianByChild.Remove(actor.data.id);
        }

        public bool IsInWidowCooldown(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return false;
            double until;
            if (!_widowCooldownUntil.TryGetValue(actor.data.id, out until))
                return false;
            return until > GetNowWorldTime();
        }

        public bool IsCourtshipBlocked(Actor actor, Actor target)
        {
            if (actor == null || target == null || actor.isRekt() || target.isRekt())
                return true;
            if (IsInWidowCooldown(actor) || IsInWidowCooldown(target))
                return true;
            if (KinshipUtils.AreCloseRelatives(actor, target, 4))
                return true;
            if (actor.hasFamily() && target.hasFamily() && actor.family == target.family)
                return true;
            int ageA = actor.getAge();
            int ageB = target.getAge();
            int ageGap = Math.Abs(ageA - ageB);
            int youngest = Math.Max(1, Math.Min(ageA, ageB));
            int gapLimit = Math.Max(25, Mathf.RoundToInt(youngest * 0.85f));
            if (ageGap > gapLimit)
                return true;
            if (actor.isSapient() && target.isSapient() && actor.hasCity() && target.hasCity() && actor.city != target.city)
            {
                if (actor.current_tile == null || target.current_tile == null)
                    return true;
                if (!KinshipUtils.AreTilesOnSameIsland(actor.current_tile, target.current_tile))
                    return true;
                if (ageGap > 45)
                    return true;
            }
            return false;
        }

        public Actor FindBestLoverCandidate(Actor actor)
        {
            if (actor == null || actor.isRekt() || !actor.isAlive() || actor.current_tile == null)
                return null;
            Actor best = null;
            float bestScore = float.MinValue;
            HashSet<long> seen = new HashSet<long>();
            foreach (Actor candidate in Finder.getUnitsFromChunk(actor.current_tile, 2))
            {
                EvaluateCandidate(actor, candidate, seen, ref best, ref bestScore);
            }
            if (actor.hasCity())
            {
                foreach (Actor candidate in actor.city.getUnits())
                {
                    EvaluateCandidate(actor, candidate, seen, ref best, ref bestScore);
                }
            }
            return best;
        }

        public void OnBecomeLovers(Actor actor, Actor target)
        {
            if (actor == null || target == null || actor.isRekt() || target.isRekt())
                return;
            EnsureFamilyForParents(actor, target);
        }

        public Family EnsureFamilyForParents(Actor parentA, Actor parentB)
        {
            if (World.world == null || parentA == null || parentA.isRekt())
                return null;
            Family familyA = parentA.hasFamily() ? parentA.family : null;
            Family familyB = parentB != null && parentB.hasFamily() ? parentB.family : null;
            if (familyA != null && familyB != null && familyA == familyB)
                return familyA;
            if (familyA == null && familyB == null)
            {
                Family created = World.world.families.newFamily(parentA, parentA.current_tile, parentB);
                InvalidateFamily(created);
                return created;
            }
            if (familyA == null && familyB != null)
            {
                if (TryAssignToFamily(parentA, familyB))
                    return familyB;
                Family created = World.world.families.newFamily(parentA, parentA.current_tile, parentB);
                InvalidateFamily(created);
                return created;
            }
            if (familyA != null && familyB == null)
            {
                if (TryAssignToFamily(parentB, familyA))
                    return familyA;
                Family created = World.world.families.newFamily(parentA, parentA.current_tile, parentB);
                InvalidateFamily(created);
                return created;
            }
            Family anchor = ChooseAnchorFamily(familyA, familyB);
            Family other = anchor == familyA ? familyB : familyA;
            if (anchor != null && other != null)
            {
                int combined = anchor.countUnits() + other.countUnits();
                if (combined <= GetDynamicFamilyCap(anchor))
                {
                    MergeFamilyInto(anchor, other);
                    return anchor;
                }
                if (!anchor.isFull())
                {
                    TryAssignToFamily(parentA, anchor);
                    TryAssignToFamily(parentB, anchor);
                    InvalidateFamily(anchor);
                    InvalidateFamily(other);
                    return anchor;
                }
            }
            Family fallback = World.world.families.newFamily(parentA, parentA.current_tile, parentB);
            InvalidateFamily(fallback);
            return fallback;
        }

        public int GetDynamicFamilyCap(Family family)
        {
            if (family == null)
                return 1;
            int baseLimit = Mathf.Max(2, family.getActorAsset().family_limit);
            if (!family.isSapient())
                return baseLimit;
            int cityBonus = 0;
            HashSet<long> cityIds = new HashSet<long>();
            for (int i = 0; i < family.units.Count; i++)
            {
                Actor actor = family.units[i];
                if (actor == null || actor.isRekt() || !actor.hasCity())
                    continue;
                long cityId = actor.city.getID();
                if (!cityIds.Add(cityId))
                    continue;
                cityBonus += Mathf.Clamp(actor.city.getPopulationMaximum() / 15, 0, 7);
            }
            int housingBonus = family.countHoused() / 3;
            int moodBonus = Mathf.Clamp((family.countHappyUnits() - family.countUnhappyUnits()) / 4, -6, 12);
            int prestigeBonus = Mathf.Clamp(Mathf.RoundToInt(GetFamilyPrestige(family) / 65f), 0, 12);
            int cap = baseLimit + cityBonus + housingBonus + moodBonus + prestigeBonus;
            if (family.areMostUnitsHungry())
                cap -= 8;
            return Mathf.Max(baseLimit, cap);
        }

        public Actor SelectAlphaCandidate(Family family)
        {
            if (family == null || family.isRekt())
                return null;
            Actor best = null;
            float bestScore = float.MinValue;
            Actor oldest = null;
            double oldestCreated = double.MaxValue;
            for (int i = 0; i < family.units.Count; i++)
            {
                Actor candidate = family.units[i];
                if (candidate == null || candidate.isRekt() || !candidate.isAlive() || candidate.asset.is_boat)
                    continue;
                if (candidate.data.created_time < oldestCreated)
                {
                    oldestCreated = candidate.data.created_time;
                    oldest = candidate;
                }
                if (!candidate.isAdult())
                    continue;
                float score = 0f;
                score += candidate.intelligence;
                score += candidate.diplomacy * 1.2f;
                score += candidate.stewardship * 1.3f;
                score += candidate.warfare * 0.8f;
                score += candidate.level * 2f;
                score += candidate.getHappinessRatio() * 25f;
                score += candidate.getAge() * 0.07f;
                if (candidate.hasHouse())
                    score += 6f;
                if (candidate.hasLover())
                    score += 2f;
                if (candidate.isCityLeader())
                    score += 20f;
                if (candidate.isKing())
                    score += 30f;
                if (candidate.isUnhappy())
                    score -= 15f;
                if (score > bestScore)
                {
                    best = candidate;
                    bestScore = score;
                }
            }
            return best ?? oldest;
        }

        public bool TryCreateCadetBranch(Actor actor)
        {
            if (World.world == null || actor == null || actor.isRekt() || !actor.isAlive() || !actor.isSapient() || !actor.isAdult() || !actor.hasFamily())
                return false;
            double now = GetNowWorldTime();
            double cooldown;
            if (_cadetCooldownUntil.TryGetValue(actor.data.id, out cooldown) && cooldown > now)
                return false;
            Family oldFamily = actor.family;
            if (oldFamily == null || oldFamily.isRekt())
                return false;
            int dynamicCap = GetDynamicFamilyCap(oldFamily);
            if (oldFamily.countUnits() <= dynamicCap)
                return false;
            bool pressure = oldFamily.areMostUnitsHungry() || oldFamily.countUnhappyUnits() > oldFamily.countHappyUnits();
            if (!pressure && !Randy.randomChance(0.08f))
                return false;
            Actor partner = null;
            if (actor.hasLover() && actor.lover != null && actor.lover.isAlive() && !actor.lover.isRekt() && actor.lover.family == oldFamily)
                partner = actor.lover;
            Family newFamily = World.world.families.newFamily(actor, actor.current_tile, partner);
            if (newFamily == null)
                return false;
            MoveDependentChildrenToFamily(actor, newFamily);
            if (partner != null)
                MoveDependentChildrenToFamily(partner, newFamily);
            _cadetCooldownUntil[actor.data.id] = now + 20.0;
            if (partner != null)
                _cadetCooldownUntil[partner.data.id] = now + 20.0;
            InvalidateFamily(oldFamily);
            InvalidateFamily(newFamily);
            return true;
        }

        public Actor SelectGuardianForChild(Actor child)
        {
            if (child == null || child.isRekt() || !child.isAlive() || !child.isBaby())
                return null;
            List<Actor> parents = KinshipUtils.GetLivingParents(child);
            if (parents.Count > 0)
            {
                Actor parent = parents[0];
                _guardianByChild[child.data.id] = parent.data.id;
                return parent;
            }
            long guardianId;
            if (_guardianByChild.TryGetValue(child.data.id, out guardianId))
            {
                Actor existing = World.world.units.get(guardianId);
                if (existing != null && existing.isAlive() && !existing.isRekt())
                    return existing;
                _guardianByChild.Remove(child.data.id);
            }
            Actor fresh = KinshipUtils.FindBestGuardian(child);
            if (fresh != null)
                _guardianByChild[child.data.id] = fresh.data.id;
            return fresh;
        }

        public void TryTriggerFamilyDefense(Actor victim, Actor attacker)
        {
            if (victim == null || attacker == null || victim.isRekt() || attacker.isRekt() || !victim.hasFamily())
                return;
            double now = GetNowWorldTime();
            double lastPing;
            if (_lastDefensePing.TryGetValue(victim.data.id, out lastPing) && now - lastPing < 2.0)
                return;
            _lastDefensePing[victim.data.id] = now;
            float range = victim.isBaby() ? 40f : 26f;
            float rangeSq = range * range;
            Family family = victim.family;
            for (int i = 0; i < family.units.Count; i++)
            {
                Actor member = family.units[i];
                if (member == null || member == victim || member.isRekt() || !member.isAlive() || member.asset.is_boat || !member.isAdult())
                    continue;
                if (!KinshipUtils.AreTilesOnSameIsland(member.current_tile, attacker.current_tile))
                    continue;
                float sq = Toolbox.SquaredDistTile(member.current_tile, attacker.current_tile);
                if (sq > rangeSq)
                    continue;
                member.addAggro(attacker);
            }
        }

        public void HandleDeath(Actor victim, bool countDeath)
        {
            if (!countDeath || victim == null || victim.isRekt() || !victim.isAlive())
                return;
            if (victim.hasLover() && victim.lover != null && victim.lover.isAlive() && !victim.lover.isRekt())
                SetWidowCooldown(victim.lover, 8.0);
            ApplyExtendedGrief(victim);
            Actor killer = null;
            if (victim.attackedBy != null && !victim.attackedBy.isRekt() && victim.attackedBy.isActor())
                killer = victim.attackedBy.a;
            if (killer != null && victim.hasFamily() && victim.family != killer.family)
            {
                RegisterFamilyFeud(victim.family, killer, 30.0);
                for (int i = 0; i < victim.family.units.Count; i++)
                {
                    Actor member = victim.family.units[i];
                    if (member == null || member == victim || member.isRekt() || !member.isAlive() || member.asset.is_boat || !member.isAdult())
                        continue;
                    member.addAggro(killer);
                }
            }
            if (victim.hasFamily())
                InvalidateFamily(victim.family);
        }

        public void OnBabyBorn(Actor baby)
        {
            if (baby == null || baby.isRekt())
                return;
            if (baby.hasFamily())
                InvalidateFamily(baby.family);
            List<Actor> parents = KinshipUtils.GetLivingParents(baby);
            for (int i = 0; i < parents.Count; i++)
            {
                Actor parent = parents[i];
                if (parent != null && parent.isAlive() && !parent.isRekt())
                    parent.changeHappiness("just_had_child");
            }
            List<Actor> siblings = KinshipUtils.GetSiblings(baby);
            for (int i = 0; i < siblings.Count; i++)
            {
                Actor sibling = siblings[i];
                if (sibling == null || sibling.isRekt() || !sibling.isAlive())
                    continue;
                sibling.changeHappiness("just_played");
            }
            List<Actor> grandParents = KinshipUtils.GetGrandParents(baby);
            for (int i = 0; i < grandParents.Count; i++)
            {
                Actor grandParent = grandParents[i];
                if (grandParent == null || grandParent.isRekt() || !grandParent.isAlive())
                    continue;
                grandParent.changeHappiness("just_had_child");
            }
            Actor guardian = SelectGuardianForChild(baby);
            if (guardian != null)
                _guardianByChild[baby.data.id] = guardian.data.id;
        }

        public void ApplyEnhancedInheritance(Actor child, Actor parent1, Actor parent2)
        {
            if (child == null || child.isRekt())
                return;
            ActorTrait fromParent1 = PickTraitToInherit(parent1, child);
            ActorTrait fromParent2 = PickTraitToInherit(parent2, child);
            if (fromParent1 != null && Randy.randomChance(0.65f))
                child.addTrait(fromParent1.id);
            if (fromParent2 != null && Randy.randomChance(0.65f))
                child.addTrait(fromParent2.id);
            if (fromParent1 != null && fromParent2 != null && fromParent1.id == fromParent2.id && !child.hasTrait(fromParent1.id))
                child.addTrait(fromParent1.id);
            if (Randy.randomChance(0.12f))
            {
                ActorTrait shared = PickSharedTrait(parent1, parent2, child);
                if (shared != null && !child.hasTrait(shared.id))
                    child.addTrait(shared.id);
            }
        }

        public bool ShouldSuppressBirth(Actor parentA, Actor parentB)
        {
            Family family = ResolveFamily(parentA, parentB);
            if (family == null || !family.isSapient())
                return false;
            int adults = Math.Max(1, family.countAdults());
            int children = family.countChildren();
            bool crowded = family.countUnits() > GetDynamicFamilyCap(family) - 1;
            if (family.areMostUnitsHungry() && children >= adults && Randy.randomChance(0.65f))
                return true;
            if (family.countHomeless() > family.countHoused() && children > 2 && Randy.randomChance(0.5f))
                return true;
            if (family.countUnhappyUnits() > family.countHappyUnits() && crowded && Randy.randomChance(0.4f))
                return true;
            return false;
        }

        public void TryApplyStabilityBonusBirth(Actor parentA, Actor parentB)
        {
            Family family = ResolveFamily(parentA, parentB);
            if (family == null || !family.isSapient())
                return;
            if (!IsFamilyStable(family))
                return;
            if (family.countUnits() >= GetDynamicFamilyCap(family))
                return;
            Actor birther = ChooseBirther(parentA, parentB);
            if (birther == null || !BabyHelper.canMakeBabies(birther))
                return;
            float chance = 0.02f;
            chance += Mathf.Clamp((family.countHappyUnits() - family.countUnhappyUnits()) * 0.01f, 0f, 0.12f);
            chance += Mathf.Clamp(GetFamilyPrestige(family) / 600f, 0f, 0.08f);
            if (!Randy.randomChance(chance))
                return;
            Actor baby = BabyMaker.makeBaby(parentA, parentB, ActorSex.None, false, 0, null, false, true);
            if (baby == null)
                return;
            if (JustBornMethod != null)
                JustBornMethod.Invoke(baby, null);
            OnBabyBorn(baby);
        }

        public void OnBecomeAdult(Actor actor)
        {
            if (actor == null || actor.isRekt())
                return;
            _guardianByChild.Remove(actor.data.id);
            if (actor.hasFamily())
                return;
            List<Actor> parents = KinshipUtils.GetLivingParents(actor);
            for (int i = 0; i < parents.Count; i++)
            {
                Actor parent = parents[i];
                if (parent == null || parent.isRekt() || !parent.isAlive() || !parent.hasFamily())
                    continue;
                if (!parent.family.isFull())
                {
                    actor.setFamily(parent.family);
                    InvalidateFamily(parent.family);
                    return;
                }
            }
            if (actor.data.ancestor_family != -1)
            {
                Family origin = World.world.families.get(actor.data.ancestor_family);
                if (origin != null && !origin.isRekt() && !origin.isFull())
                {
                    actor.setFamily(origin);
                    InvalidateFamily(origin);
                }
            }
        }

        private void EvaluateCandidate(
            Actor actor,
            Actor candidate,
            HashSet<long> seen,
            ref Actor best,
            ref float bestScore)
        {
            if (candidate == null || candidate == actor || candidate.isRekt() || !candidate.isAlive())
                return;
            long id = candidate.data.id;
            if (!seen.Add(id))
                return;
            if (!candidate.canFallInLoveWith(actor))
                return;
            float score = 0f;
            if (actor.hasCity() && candidate.hasCity() && actor.city == candidate.city)
                score += 35f;
            if (KinshipUtils.AreTilesOnSameIsland(actor.current_tile, candidate.current_tile))
                score += 12f;
            if (actor.hasCulture() && candidate.hasCulture() && actor.culture == candidate.culture)
                score += 10f;
            if (actor.hasLanguage() && candidate.hasLanguage() && actor.language == candidate.language)
                score += 7f;
            if (candidate.hasFamily())
                score += GetFamilyPrestige(candidate.family) * 0.03f;
            if (candidate.hasHouse())
                score += 4f;
            score += candidate.getHappinessRatio() * 8f;
            if (candidate.isCityLeader())
                score += 6f;
            if (candidate.isKing())
                score += 10f;
            int ageGap = Math.Abs(actor.getAge() - candidate.getAge());
            score -= ageGap * 0.5f;
            if (score > bestScore)
            {
                best = candidate;
                bestScore = score;
            }
        }

        private Family ResolveFamily(Actor parentA, Actor parentB)
        {
            if (parentA != null && parentA.hasFamily())
                return parentA.family;
            if (parentB != null && parentB.hasFamily())
                return parentB.family;
            return null;
        }

        private static Actor ChooseBirther(Actor parentA, Actor parentB)
        {
            if (parentA == null)
                return parentB;
            if (parentB == null)
                return parentA;
            if (parentA.isSexFemale())
                return parentA;
            if (parentB.isSexFemale())
                return parentB;
            return Randy.randomBool() ? parentA : parentB;
        }

        private bool IsFamilyStable(Family family)
        {
            if (family == null || family.isRekt())
                return false;
            if (family.areMostUnitsHungry())
                return false;
            if (family.countUnhappyUnits() > family.countHappyUnits())
                return false;
            if (family.countHomeless() > family.countHoused() + 1)
                return false;
            return true;
        }

        private void SetWidowCooldown(Actor actor, double years)
        {
            if (actor == null || actor.isRekt())
                return;
            _widowCooldownUntil[actor.data.id] = GetNowWorldTime() + years;
        }

        private bool TryAssignToFamily(Actor actor, Family family)
        {
            if (actor == null || family == null || actor.isRekt() || family.isRekt())
                return false;
            if (actor.family == family)
                return true;
            if (family.isFull())
                return false;
            actor.setFamily(family);
            InvalidateFamily(family);
            return actor.family == family;
        }

        private Family ChooseAnchorFamily(Family familyA, Family familyB)
        {
            if (familyA == null)
                return familyB;
            if (familyB == null)
                return familyA;
            float scoreA = GetFamilyPrestige(familyA) + familyA.countUnits() * 2f;
            float scoreB = GetFamilyPrestige(familyB) + familyB.countUnits() * 2f;
            if (familyA.areMostUnitsHungry())
                scoreA -= 20f;
            if (familyB.areMostUnitsHungry())
                scoreB -= 20f;
            familyA.checkAlpha();
            familyB.checkAlpha();
            if (familyA.getAlpha() != null)
                scoreA += 6f;
            if (familyB.getAlpha() != null)
                scoreB += 6f;
            if (scoreA > scoreB)
                return familyA;
            if (scoreB > scoreA)
                return familyB;
            return familyA.getAge() >= familyB.getAge() ? familyA : familyB;
        }

        private void MergeFamilyInto(Family target, Family source)
        {
            if (target == null || source == null || target == source || target.isRekt() || source.isRekt())
                return;
            List<Actor> toMove = new List<Actor>(source.units);
            for (int i = 0; i < toMove.Count; i++)
            {
                Actor member = toMove[i];
                if (member == null || member.isRekt() || !member.isAlive() || member.family != source)
                    continue;
                member.setFamily(target);
            }
            InvalidateFamily(target);
            InvalidateFamily(source);
        }

        private void MoveDependentChildrenToFamily(Actor parent, Family target)
        {
            if (parent == null || target == null || parent.isRekt() || target.isRekt())
                return;
            foreach (Actor child in parent.getChildren(false))
            {
                if (child == null || child.isRekt() || !child.isAlive() || !child.isBaby())
                    continue;
                if (child.family == target)
                    continue;
                if (target.isFull())
                    break;
                child.setFamily(target);
            }
            InvalidateFamily(target);
        }

        private void TickOrphanCare()
        {
            List<Actor> alive = World.world.units.units_only_alive;
            for (int i = 0; i < alive.Count; i++)
            {
                Actor child = alive[i];
                if (child == null || child.isRekt() || !child.isAlive() || !child.isBaby() || child.asset.is_boat)
                    continue;
                Actor guardian = SelectGuardianForChild(child);
                if (guardian == null || guardian.isRekt() || !guardian.isAlive())
                    continue;
                if (!child.hasFamily() && guardian.hasFamily() && !guardian.family.isFull())
                    child.setFamily(guardian.family);
                if (child.isKingdomCiv() && guardian.hasCity() && (!child.hasCity() || child.city != guardian.city) && !child.isKing() && !child.isCityLeader())
                    child.joinCity(guardian.city);
                if (KinshipUtils.AreTilesOnSameIsland(child.current_tile, guardian.current_tile))
                {
                    float distance = child.distanceToObjectTarget(guardian);
                    if (distance > 30f)
                        child.beh_actor_target = guardian;
                    if (distance <= 12f && Randy.randomChance(0.05f))
                        child.changeHappiness("just_talked");
                }
            }
        }

        private void ApplyExtendedGrief(Actor victim)
        {
            HashSet<long> processed = new HashSet<long>();
            foreach (Actor child in victim.getChildren(false))
            {
                if (child == null || child.isRekt() || !child.isAlive())
                    continue;
                if (processed.Add(child.data.id))
                    child.changeHappiness("death_family_member");
            }
            List<Actor> siblings = KinshipUtils.GetSiblings(victim);
            for (int i = 0; i < siblings.Count; i++)
            {
                Actor sibling = siblings[i];
                if (sibling == null || sibling.isRekt() || !sibling.isAlive())
                    continue;
                if (processed.Add(sibling.data.id))
                    sibling.changeHappiness("death_family_member");
            }
            List<Actor> grandParents = KinshipUtils.GetGrandParents(victim);
            for (int i = 0; i < grandParents.Count; i++)
            {
                Actor grandParent = grandParents[i];
                if (grandParent == null || grandParent.isRekt() || !grandParent.isAlive())
                    continue;
                if (processed.Add(grandParent.data.id))
                    grandParent.changeHappiness("death_family_member");
            }
        }

        private void RegisterFamilyFeud(Family family, Actor enemy, double years)
        {
            if (family == null || enemy == null || family.isRekt() || enemy.isRekt())
                return;
            FamilyEnemyKey key = new FamilyEnemyKey(family.id, enemy.data.id);
            _familyFeudUntil[key] = GetNowWorldTime() + years;
        }

        private void TickFamilyFeuds()
        {
            if (_familyFeudUntil.Count == 0)
                return;
            double now = GetNowWorldTime();
            List<FamilyEnemyKey> keys = new List<FamilyEnemyKey>(_familyFeudUntil.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                FamilyEnemyKey key = keys[i];
                double until = _familyFeudUntil[key];
                if (until <= now)
                {
                    _familyFeudUntil.Remove(key);
                    continue;
                }
                Family family = World.world.families.get(key.FamilyId);
                Actor enemy = World.world.units.get(key.EnemyId);
                if (family == null || enemy == null || family.isRekt() || enemy.isRekt() || !enemy.isAlive())
                {
                    _familyFeudUntil.Remove(key);
                    continue;
                }
                for (int j = 0; j < family.units.Count; j++)
                {
                    Actor member = family.units[j];
                    if (member == null || member.isRekt() || !member.isAlive() || !member.isAdult() || member.asset.is_boat)
                        continue;
                    if (!KinshipUtils.AreTilesOnSameIsland(member.current_tile, enemy.current_tile))
                        continue;
                    if (member.distanceToObjectTarget(enemy) <= 45f)
                        member.addAggro(enemy);
                }
            }
        }

        private void TickMaintenance()
        {
            double now = GetNowWorldTime();
            CleanupDictionaryByTime(_widowCooldownUntil, now);
            CleanupDictionaryByTime(_cadetCooldownUntil, now);
            CleanupDictionaryByTime(_lastDefensePing, now - 15.0);
            CleanupGuardians();
            CleanupPrestigeCache();
        }

        private void CleanupDictionaryByTime(Dictionary<long, double> dictionary, double threshold)
        {
            if (dictionary.Count == 0)
                return;
            List<long> remove = new List<long>();
            foreach (KeyValuePair<long, double> pair in dictionary)
            {
                if (pair.Value <= threshold)
                    remove.Add(pair.Key);
            }
            for (int i = 0; i < remove.Count; i++)
                dictionary.Remove(remove[i]);
        }

        private void CleanupGuardians()
        {
            if (_guardianByChild.Count == 0)
                return;
            List<long> remove = new List<long>();
            foreach (KeyValuePair<long, long> pair in _guardianByChild)
            {
                Actor child = World.world.units.get(pair.Key);
                Actor guardian = World.world.units.get(pair.Value);
                if (child == null || child.isRekt() || !child.isAlive() || !child.isBaby() || guardian == null || guardian.isRekt() || !guardian.isAlive())
                    remove.Add(pair.Key);
            }
            for (int i = 0; i < remove.Count; i++)
                _guardianByChild.Remove(remove[i]);
        }

        private void CleanupPrestigeCache()
        {
            if (_prestigeByFamily.Count == 0)
                return;
            List<long> remove = new List<long>();
            foreach (KeyValuePair<long, CachedPrestige> pair in _prestigeByFamily)
            {
                Family family = World.world.families.get(pair.Key);
                if (family == null || family.isRekt())
                    remove.Add(pair.Key);
            }
            for (int i = 0; i < remove.Count; i++)
                _prestigeByFamily.Remove(remove[i]);
        }

        private ActorTrait PickTraitToInherit(Actor parent, Actor child)
        {
            if (parent == null || child == null || parent.isRekt() || child.isRekt())
                return null;
            int totalWeight = 0;
            foreach (ActorTrait trait in parent.getTraits())
            {
                if (trait == null)
                    continue;
                if (child.hasTrait(trait.id))
                    continue;
                if (trait.rate_inherit <= 0 && trait.rate_birth <= 0)
                    continue;
                int weight = Mathf.Max(1, trait.rate_inherit + trait.rate_birth);
                totalWeight += weight;
            }
            if (totalWeight <= 0)
                return null;
            int roll = Randy.randomInt(0, totalWeight);
            int cursor = 0;
            foreach (ActorTrait trait in parent.getTraits())
            {
                if (trait == null)
                    continue;
                if (child.hasTrait(trait.id))
                    continue;
                if (trait.rate_inherit <= 0 && trait.rate_birth <= 0)
                    continue;
                int weight = Mathf.Max(1, trait.rate_inherit + trait.rate_birth);
                cursor += weight;
                if (roll < cursor)
                    return trait;
            }
            return null;
        }

        private ActorTrait PickSharedTrait(Actor parent1, Actor parent2, Actor child)
        {
            if (parent1 == null || parent2 == null || child == null || parent1.isRekt() || parent2.isRekt())
                return null;
            List<ActorTrait> shared = new List<ActorTrait>();
            foreach (ActorTrait trait in parent1.getTraits())
            {
                if (trait == null || child.hasTrait(trait.id))
                    continue;
                if (trait.rate_inherit <= 0 && trait.rate_birth <= 0)
                    continue;
                if (parent2.hasTrait(trait.id))
                    shared.Add(trait);
            }
            if (shared.Count == 0)
                return null;
            return shared[Randy.randomInt(0, shared.Count)];
        }

        private float GetFamilyPrestige(Family family)
        {
            if (family == null || family.isRekt())
                return 0f;
            CachedPrestige cached;
            if (_prestigeByFamily.TryGetValue(family.id, out cached) && cached.ExpiresAt > Time.unscaledTime)
                return cached.Score;
            float score = CalculateFamilyPrestige(family);
            _prestigeByFamily[family.id] = new CachedPrestige
            {
                Score = score,
                ExpiresAt = Time.unscaledTime + 12f
            };
            return score;
        }

        private float CalculateFamilyPrestige(Family family)
        {
            if (family == null || family.isRekt())
                return 0f;
            float score = 0f;
            score += family.getTotalBirths() * 0.4f;
            score += family.getTotalKills() * 0.7f;
            score -= family.getTotalDeaths() * 0.2f;
            score += family.countKings() * 25f;
            score += family.countLeaders() * 12f;
            score += family.countUnits() * 1.5f;
            score += family.countHappyUnits() * 0.9f;
            score -= family.countUnhappyUnits() * 0.5f;
            score += family.countTotalMoney() * 0.03f;
            if (family.areMostUnitsHungry())
                score -= 25f;
            if (family.hasFounders())
                score += 8f;
            return Mathf.Max(0f, score);
        }

        private double GetNowWorldTime()
        {
            if (World.world == null)
                return 0.0;
            return World.world.getCurWorldTime();
        }

        private void HandleWorldLoaded()
        {
            ResetAllState();
        }
    }
}
