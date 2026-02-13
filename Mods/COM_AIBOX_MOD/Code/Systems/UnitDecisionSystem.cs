using System;
using System.Collections.Generic;
using UnityEngine;
using AIBox;

namespace AIBox
{
    /// <summary>
    /// Decision system that scores available actions based on:
    /// - Current emotional state
    /// - Personality traits
    /// - Immediate needs (hunger, safety, social)
    /// - Environmental context
    /// 
    /// Returns top-scored action or list for AI selection.
    /// </summary>
    public static class UnitDecisionSystem
    {
        // Action IDs - these map to game behaviors or custom actions
        public static class Actions
        {
            // Survival
            public const string FIND_FOOD = "find_food";
            public const string FLEE = "flee";
            public const string SEEK_SAFETY = "seek_safety";
            public const string HIDE = "hide";
            public const string REST = "rest";
            
            // Combat
            public const string ATTACK_ENEMY = "attack_enemy";
            public const string PROTECT_LOVED = "protect_loved";
            public const string DEFEND = "defend";
            public const string REVENGE = "revenge";
            
            // Social
            public const string TALK = "talk";
            public const string COMFORT_ALLY = "comfort_ally";
            public const string VISIT_FAMILY = "visit_family";
            public const string CELEBRATE = "celebrate";
            public const string MOURN = "mourn";
            public const string REGROUP = "regroup";
            
            // Work
            public const string WORK = "work";
            public const string BUILD = "build";
            
            // Self
            public const string WANDER = "wander";
            public const string IDLE = "idle";
            
            // Crime
            public const string STEAL = "steal";
            
            // Special
            public const string BERSERK = "berserk";
            public const string PANIC = "panic";
            public const string SELF_REGULATE = "self_regulate";
        }
        
        public static void UpdateDecision(Actor actor, UnitMind mind, SensoryMemory senses)
        {
            if (actor == null || !actor.isAlive()) return;
            if (mind == null || mind.Emotions == null || mind.Traits == null || mind.Needs == null) return;
            mind.Emotions.EnsureInitialized();
            mind.Needs.ClampAll();
            
            // Score all possible actions
            Dictionary<string, float> scores = new Dictionary<string, float>();
            
            ScoreSurvivalActions(actor, mind, senses, scores);
            ScoreCombatActions(actor, mind, senses, scores);
            ScoreSocialActions(actor, mind, senses, scores);
            ScoreWorkActions(actor, mind, senses, scores);
            ScoreSelfActions(actor, mind, senses, scores);
            ScoreSpecialActions(actor, mind, senses, scores);
            
            // Store scores for AI/debug
            mind.ActionScores = scores;

            string selectedAction = ChooseAction(mind, scores);
            float selectedScore = scores.TryGetValue(selectedAction, out float score) ? score : 0f;

            if (selectedScore > 0.08f)
            {
                ExecuteAction(actor, mind, senses, selectedAction, selectedScore);
            }
            else
            {
                mind.LastDecision = "Idle / Default AI";
                mind.LastDecisionReason = "No compelling action";
                mind.LastActionId = Actions.IDLE;
                mind.LastDecisionScore = selectedScore;
                mind.DecisionStreak = 0;
            }
        }
        
        // ===============================================================================
        // SCORING FUNCTIONS
        // ===============================================================================
        
        private static void ScoreSurvivalActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            NeedsHierarchy needs = mind.Needs;
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            
            // --- FIND FOOD ---
            float hungerScore = needs.Hunger.Urgency * 0.75f;
            hungerScore += (1f - emotions.Joy.Value) * 0.1f;
            hungerScore += emotions.Stress * 0.05f;
            if (actor.data.nutrition < 10) hungerScore = Mathf.Max(hungerScore, 0.95f);
            if (senses.InDanger) hungerScore *= 0.5f;
            scores[Actions.FIND_FOOD] = Mathf.Clamp01(hungerScore);
            
            // --- FLEE ---
            float fleeScore = 0f;
            if (senses.InDanger)
            {
                fleeScore = emotions.GetThreatDrive(traits) * 0.7f;
                fleeScore += (1f - traits.Bravery) * 0.15f;
                fleeScore += Mathf.Clamp01(senses.ThreatLevel) * 0.25f;
                if (senses.EnemyCount > senses.AllyCount + 1) fleeScore += 0.15f;
                if (emotions.Fear.IsCritical || emotions.Stress > 0.85f) fleeScore = Mathf.Max(fleeScore, 0.95f);
            }
            scores[Actions.FLEE] = Mathf.Clamp01(fleeScore);

            float safetyScore = 0f;
            if (senses.InDanger)
            {
                safetyScore = fleeScore * 0.8f + emotions.Stress * 0.2f;
            }
            else
            {
                safetyScore = emotions.GetRecoveryDrive(needs) * 0.35f;
                safetyScore += needs.Safety.Urgency * 0.25f;
                safetyScore += emotions.GetThreatDrive(traits) * 0.25f;
                safetyScore += emotions.Burnout * 0.15f;
                if (senses.SeeOwnCityBuilding) safetyScore += 0.1f;
            }
            scores[Actions.SEEK_SAFETY] = Mathf.Clamp01(safetyScore);
            
            // --- REST ---
            float restScore = 0f;
            float hpRatio = (float)actor.data.health / actor.getMaxHealth();
            if (!senses.InDanger)
            {
                restScore = (1f - hpRatio) * 0.45f;
                restScore += emotions.GetRecoveryDrive(needs) * 0.35f;
                restScore += emotions.Burnout * 0.15f;
                restScore += Mathf.Max(0f, 0.45f - emotions.Sanity) * 0.25f;
            }
            scores[Actions.REST] = Mathf.Clamp01(restScore);
        }
        
        private static void ScoreCombatActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            UnitMemory memory = mind.Memory;
            
            // --- ATTACK ENEMY ---
            float attackScore = 0f;
            if (senses.EnemyCount > 0)
            {
                attackScore = 0.2f;
                attackScore += emotions.GetAggressionDrive(traits) * 0.55f;
                attackScore += traits.Bravery * 0.15f;
                attackScore += Mathf.Clamp01((senses.AllyCount - senses.EnemyCount + 2f) / 4f) * 0.1f;
                attackScore += emotions.Pride.Value * 0.05f;
                attackScore -= emotions.GetThreatDrive(traits) * 0.2f;
                attackScore -= emotions.Burnout * 0.1f;

                if (emotions.Fear.Value > 0.6f && traits.Bravery < 0.45f) attackScore *= 0.35f;
            }
            scores[Actions.ATTACK_ENEMY] = Mathf.Clamp01(attackScore);
            
            // --- PROTECT LOVED ONES ---
            float protectScore = 0f;
            if (senses.SawFamilyInDanger)
            {
                protectScore = 0.75f;
                protectScore += traits.Empathy * 0.15f;
                protectScore += emotions.Love.Value * 0.15f;
                protectScore += traits.Loyalty * 0.1f;
                protectScore += emotions.Regulation * 0.05f;
            }
            scores[Actions.PROTECT_LOVED] = Mathf.Clamp01(protectScore);
            
            // --- REVENGE ---
            float revengeScore = 0f;
            if (senses.SawCorpseFamily || senses.SawCorpseKingdom)
            {
                revengeScore = traits.Vengefulness * 0.4f;
                revengeScore += emotions.Anger.Value * 0.25f;
                revengeScore += emotions.Trauma * 0.15f;
                if (memory != null) revengeScore += memory.RecentLoss * 0.2f;
                if (senses.EnemyCount > 0) revengeScore += 0.15f;
            }
            scores[Actions.REVENGE] = Mathf.Clamp01(revengeScore);
        }
        
        private static void ScoreSocialActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            NeedsHierarchy needs = mind.Needs;
            UnitMemory memory = mind.Memory;
            
            // --- TALK ---
            float talkScore = 0f;
            if (senses.AllyCount > 0 && !senses.InDanger)
            {
                talkScore = emotions.GetSocialDrive(traits, needs) * 0.55f;
                talkScore += traits.Sociability * 0.2f;
                talkScore += emotions.Mood * 0.15f;
                talkScore += (1f - emotions.GetThreatDrive(traits)) * 0.1f;
            }
            scores[Actions.TALK] = Mathf.Clamp01(talkScore);
            
            // --- COMFORT ALLY ---
            float comfortScore = 0f;
            if (senses.SeeFight || (senses.SeeFriendlyGathering && traits.Empathy > 0.45f))
            {
                comfortScore = traits.Empathy * 0.35f;
                comfortScore += emotions.Regulation * 0.2f;
                comfortScore += emotions.Trust.Value * 0.15f;
                comfortScore += senses.SeeFight ? 0.15f : 0f;
            }
            scores[Actions.COMFORT_ALLY] = Mathf.Clamp01(comfortScore);
            
            // --- VISIT FAMILY ---
            float visitScore = 0f;
            if (!senses.SeeLover && !senses.SeeFamily && !senses.InDanger)
            {
                visitScore = emotions.GetSocialDrive(traits, needs) * 0.25f;
                visitScore += emotions.Love.Value * 0.25f;
                visitScore += needs.Companionship.Urgency * 0.2f;
                visitScore += Mathf.Max(0f, 0.5f - emotions.SocialBattery) * 0.15f;

                if (actor.lover != null && actor.lover.isAlive())
                {
                    visitScore += 0.2f;
                }
                else if (actor.lover != null && !actor.lover.isAlive())
                {
                    visitScore = 0f;
                }
            }
            scores[Actions.VISIT_FAMILY] = Mathf.Clamp01(visitScore);
            
            // --- MOURN ---
            float mournScore = 0f;
            if (emotions.Sadness.Value > 0.45f || emotions.Trauma > 0.4f)
            {
                mournScore = emotions.Sadness.Value * 0.45f;
                mournScore += emotions.Trauma * 0.35f;
                mournScore += memory != null ? memory.RecentLoss * 0.25f : 0f;
                mournScore -= traits.Stability * 0.15f;
            }
            scores[Actions.MOURN] = Mathf.Clamp01(mournScore);
            
            // --- CELEBRATE ---
            float celebrateScore = 0f;
            if (emotions.Joy.Value > 0.6f && senses.SeeFriendlyGathering)
            {
                celebrateScore = emotions.Joy.Value * 0.3f;
                celebrateScore += emotions.Mood * 0.25f;
                celebrateScore += traits.Sociability * 0.25f;
                celebrateScore -= emotions.Burnout * 0.2f;
            }
            scores[Actions.CELEBRATE] = Mathf.Clamp01(celebrateScore);

            float regroupScore = 0f;
            if (senses.InDanger && senses.AllyCount > 0)
            {
                regroupScore = emotions.GetThreatDrive(traits) * 0.4f;
                regroupScore += traits.Loyalty * 0.2f;
                regroupScore += emotions.Regulation * 0.15f;
                regroupScore += Mathf.Clamp01(senses.AllyCount / 5f) * 0.2f;
            }
            scores[Actions.REGROUP] = Mathf.Clamp01(regroupScore);
        }
        
        private static void ScoreWorkActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            NeedsHierarchy needs = mind.Needs;
            PersonalityTraits traits = mind.Traits;
            EmotionalSpectrum emotions = mind.Emotions;
            
            // --- WORK ---
            float workScore = 0f;
            if (!senses.InDanger)
            {
                workScore = 0.2f;
                workScore += traits.Ambition * 0.22f;
                workScore += needs.Purpose.Urgency * 0.18f;
                workScore += emotions.Pride.Value * 0.1f;
                workScore += emotions.Anticipation.Value * 0.1f;
                workScore += needs.Hunger.Urgency * 0.1f;
                workScore -= emotions.Burnout * 0.35f;
                workScore -= emotions.Stress * 0.15f;
                if (emotions.GetRecoveryDrive(needs) > 0.65f) workScore *= 0.45f;
            }
            scores[Actions.WORK] = Mathf.Clamp01(workScore);
            
            // --- BUILD ---
            float buildScore = 0f;
            if (needs.Shelter.IsLow && !senses.InDanger)
            {
                buildScore = needs.Shelter.Urgency * 0.5f;
                buildScore += traits.Ambition * 0.15f;
                buildScore += emotions.Regulation * 0.1f;
                buildScore += emotions.Pride.Value * 0.1f;
                buildScore -= emotions.Burnout * 0.2f;
            }
            scores[Actions.BUILD] = Mathf.Clamp01(buildScore);
        }
        
        private static void ScoreSelfActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            PersonalityTraits traits = mind.Traits;
            EmotionalSpectrum emotions = mind.Emotions;
            NeedsHierarchy needs = mind.Needs;
            
            // --- WANDER ---
            float wanderScore = 0f;
            if (!senses.InDanger)
            {
                wanderScore = traits.Curiosity * 0.3f;
                wanderScore += (1f - traits.Sociability) * 0.2f;
                if (emotions.Anticipation.Value < 0.3f)
                {
                    wanderScore += 0.2f;
                }
                wanderScore += Mathf.Max(0f, 0.55f - emotions.SocialBattery) * 0.1f;
                wanderScore -= emotions.Stress * 0.2f;
            }
            scores[Actions.WANDER] = Mathf.Clamp01(wanderScore);
            
            float regulateScore = 0f;
            if (!senses.InDanger)
            {
                regulateScore = emotions.GetRecoveryDrive(needs) * 0.65f;
                regulateScore += emotions.Stress * 0.15f;
                regulateScore += emotions.Burnout * 0.1f;
                regulateScore += Mathf.Max(0f, 0.4f - emotions.Sanity) * 0.3f;
            }
            scores[Actions.SELF_REGULATE] = Mathf.Clamp01(regulateScore);

            float idleScore = 0.08f;
            idleScore += Mathf.Max(0f, 0.5f - emotions.Arousal) * 0.08f;
            idleScore += Mathf.Max(0f, 0.4f - emotions.Stress) * 0.06f;
            scores[Actions.IDLE] = Mathf.Clamp01(idleScore);
            
            // --- STEAL (if thief trait) ---
            float stealScore = 0f;
            if (actor.hasTrait("thief"))
            {
                stealScore = traits.Greed * 0.3f;
                stealScore += (1f - traits.Honesty) * 0.3f;
                stealScore += emotions.Anticipation.Value * 0.1f;
                stealScore += emotions.Stress * 0.05f;
                
                if (!senses.InDanger && senses.AllyCount > 0)
                {
                    stealScore += 0.2f;
                }
                stealScore -= mind.Emotions.Guilt.Value * 0.15f;
            }
            scores[Actions.STEAL] = Mathf.Clamp01(stealScore);
        }
        
        private static void ScoreSpecialActions(Actor actor, UnitMind mind, SensoryMemory senses, Dictionary<string, float> scores)
        {
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            NeedsHierarchy needs = mind.Needs;
            
            // --- BERSERK (low sanity + high anger) ---
            float berserkScore = 0f;
            if (emotions.Sanity < 0.45f && emotions.Anger.Value > 0.5f)
            {
                berserkScore = (1f - emotions.Sanity) * 0.45f;
                berserkScore += emotions.Anger.Value * 0.3f;
                berserkScore += emotions.Stress * 0.15f;
                berserkScore += emotions.Trauma * 0.12f;
                berserkScore += traits.Aggression * 0.1f;
            }
            scores[Actions.BERSERK] = Mathf.Clamp01(berserkScore);
            
            // --- PANIC (critical fear + low stability) ---
            float panicScore = 0f;
            if ((emotions.Fear.IsCritical || emotions.Stress > 0.75f) && emotions.Sanity < 0.65f)
            {
                panicScore = emotions.GetThreatDrive(traits) * 0.55f;
                panicScore += (1f - emotions.Sanity) * 0.25f;
                panicScore += senses.InDanger ? 0.2f : 0f;
            }
            scores[Actions.PANIC] = Mathf.Clamp01(panicScore);

            if (!scores.ContainsKey(Actions.SELF_REGULATE))
            {
                scores[Actions.SELF_REGULATE] = Mathf.Clamp01(emotions.GetRecoveryDrive(needs) * 0.5f);
            }
        }

        private static string ChooseAction(UnitMind mind, Dictionary<string, float> scores)
        {
            if (scores == null || scores.Count == 0) return Actions.IDLE;

            List<KeyValuePair<string, float>> ranked = new List<KeyValuePair<string, float>>(scores);
            ranked.Sort((a, b) => b.Value.CompareTo(a.Value));

            string bestAction = ranked[0].Key;
            float bestScore = ranked[0].Value;

            if (!string.IsNullOrEmpty(mind.LastActionId) && scores.TryGetValue(mind.LastActionId, out float lastScore))
            {
                float streakFactor = Mathf.Clamp01(mind.DecisionStreak / 8f);
                float stickiness = Mathf.Lerp(0.82f, 0.94f, streakFactor);
                if (lastScore >= bestScore * stickiness && lastScore > 0.08f)
                {
                    return mind.LastActionId;
                }
            }

            float impulsivity = 0f;
            impulsivity += (1f - mind.Traits.Rationality) * 0.5f;
            impulsivity += mind.Emotions.Stress * 0.35f;
            impulsivity += mind.Emotions.Arousal * 0.15f;
            impulsivity = Mathf.Clamp01(impulsivity);

            int candidateCount = Math.Min(3, ranked.Count);
            if (candidateCount <= 1) return bestAction;

            float exploreChance = Mathf.Lerp(0.05f, 0.32f, impulsivity);
            if (UnityEngine.Random.value > exploreChance) return bestAction;

            float totalWeight = 0f;
            for (int i = 0; i < candidateCount; i++)
            {
                float w = Mathf.Max(0.001f, ranked[i].Value);
                totalWeight += w * w;
            }

            float roll = UnityEngine.Random.value * totalWeight;
            float running = 0f;
            for (int i = 0; i < candidateCount; i++)
            {
                float w = Mathf.Max(0.001f, ranked[i].Value);
                running += w * w;
                if (roll <= running) return ranked[i].Key;
            }

            return bestAction;
        }
        
        // ===============================================================================
        // ACTION EXECUTION
        // ===============================================================================
        
        private static void ExecuteAction(Actor actor, UnitMind mind, SensoryMemory senses, string action, float score)
        {
            mind.LastDecision = action;
            mind.LastDecisionReason = $"Score: {score:F2}";
            mind.LastDecisionTime = Time.time;
            if (mind.LastActionId == action) mind.DecisionStreak++;
            else mind.DecisionStreak = 1;
            mind.LastActionId = action;
            mind.LastDecisionScore = score;
            
            // Emotional feedback for actions
            ApplyEmotionalFeedback(mind, action);
            
            // Execute the action
            switch (action)
            {
                case Actions.FIND_FOOD:
                    UnitActionSystem.FindMeatSource(actor, ai.behaviours.MeatTargetType.Meat);
                    break;
                    
                case Actions.FLEE:
                    ExecuteFlee(actor, senses);
                    break;

                case Actions.SEEK_SAFETY:
                    ExecuteFlee(actor, senses);
                    break;
                    
                case Actions.ATTACK_ENEMY:
                case Actions.REVENGE:
                    Actor enemy = FindNearestEnemy(actor);
                    if (enemy != null)
                    {
                        mind.LastDecision = $"Attack ({enemy.getName()})";
                        UnitActionSystem.AttackHuntingTarget(actor, enemy);
                    }
                    break;
                    
                case Actions.PROTECT_LOVED:
                    ExecuteProtect(actor, mind, senses);
                    break;
                    
                case Actions.TALK:
                case Actions.COMFORT_ALLY:
                    Actor ally = FindNearestAlly(actor);
                    if (ally != null)
                    {
                        mind.LastDecision = $"Talk ({ally.getName()})";
                        UnitActionSystem.Talk(actor, ally);
                    }
                    break;

                case Actions.REGROUP:
                    Actor regroupTarget = FindNearestAlly(actor);
                    if (regroupTarget != null)
                    {
                        mind.LastDecision = $"Regroup ({regroupTarget.getName()})";
                        UnitActionSystem.GoToActorTarget(actor, regroupTarget);
                    }
                    else
                    {
                        ExecuteFlee(actor, senses);
                    }
                    break;
                    
                case Actions.VISIT_FAMILY:
                    if (actor.lover != null && actor.lover.isAlive())
                    {
                        mind.LastDecision = $"Visit {actor.lover.getName()}";
                        UnitActionSystem.GoToActorTarget(actor, actor.lover);
                    }
                    break;
                    
                case Actions.STEAL:
                    Actor target = FindNearestAlly(actor);
                    if (target != null)
                    {
                        mind.LastDecision = $"Steal from {target.getName()}";
                        UnitActionSystem.StealFromTarget(actor, target);
                        
                        // Guilt!
                        if (mind.Traits.Morality > 0.3f)
                        {
                            mind.Emotions.Guilt.Increase(Emotion.MEDIUM);
                        }
                    }
                    break;
                    
                case Actions.BERSERK:
                    Actor anyTarget = FindNearestEnemy(actor) ?? FindNearestAlly(actor);
                    if (anyTarget != null)
                    {
                        mind.LastDecision = $"BERSERK! ({anyTarget.getName()})";
                        UnitActionSystem.AttackHuntingTarget(actor, anyTarget);
                    }
                    break;
                    
                case Actions.PANIC:
                    mind.LastDecision = "PANIC!";
                    ExecuteFlee(actor, senses);
                    break;
                    
                case Actions.MOURN:
                    mind.LastDecision = "Mourning";
                    // Stay still, emotions handle rest
                    break;

                case Actions.CELEBRATE:
                    mind.LastDecision = "Celebrating!";
                    // Talk to nearby friends
                    Actor friend = FindNearestAlly(actor);
                    if (friend != null) UnitActionSystem.Talk(actor, friend);
                    break;

                case Actions.SELF_REGULATE:
                    mind.LastDecision = "Self-Regulating";
                    Building calmSpot = FindNearestSafeBuilding(actor);
                    if (calmSpot != null) UnitActionSystem.GoToBuildingTarget(actor, calmSpot);
                    else UnitActionSystem.CheckNeeds(actor, 0);
                    break;
                    
                case Actions.WANDER:
                    mind.LastDecision = "Wandering";
                    // Let default AI handle movement
                    break;
                    
                case Actions.WORK:
                case Actions.BUILD:
                    mind.LastDecision = "Working";
                    // Let default job AI handle
                    break;
                    
                default:
                    mind.LastDecision = "Idle / Default AI";
                    break;
            }
        }
        
        private static void ApplyEmotionalFeedback(UnitMind mind, string action)
        {
            EmotionalSpectrum e = mind.Emotions;
            
            switch (action)
            {
                case Actions.ATTACK_ENEMY:
                case Actions.REVENGE:
                    e.Anger.Increase(Emotion.SMALL);
                    e.Fear.Decrease(Emotion.SMALL);
                    e.Sanity = Mathf.MoveTowards(e.Sanity, 0.5f, Emotion.MICRO);
                    break;
                    
                case Actions.PROTECT_LOVED:
                    e.Love.Increase(Emotion.MEDIUM);
                    e.Fear.Decrease(Emotion.SMALL);
                    e.Pride.Increase(Emotion.SMALL);
                    break;
                    
                case Actions.FLEE:
                case Actions.SEEK_SAFETY:
                    e.Fear.Decrease(Emotion.SMALL); // Relief from fleeing
                    e.Shame.Increase(Emotion.MICRO); // If brave, shame from fleeing
                    e.Stress = Mathf.MoveTowards(e.Stress, 0f, Emotion.SMALL);
                    break;
                    
                case Actions.TALK:
                case Actions.COMFORT_ALLY:
                case Actions.REGROUP:
                    e.Joy.Increase(Emotion.SMALL);
                    e.Sadness.Decrease(Emotion.SMALL);
                    e.Trust.Increase(Emotion.MICRO);
                    e.Stress = Mathf.MoveTowards(e.Stress, 0f, Emotion.MICRO);
                    break;
                    
                case Actions.CELEBRATE:
                    e.Joy.Increase(Emotion.MEDIUM);
                    e.Pride.Increase(Emotion.SMALL);
                    break;
                    
                case Actions.MOURN:
                    e.Sadness.Decrease(Emotion.MICRO); // Processing grief
                    break;
                    
                case Actions.STEAL:
                    e.Anticipation.Increase(Emotion.SMALL);
                    if (mind.Traits.Morality > 0.4f)
                    {
                        e.Guilt.Increase(Emotion.MEDIUM);
                    }
                    break;
                    
                case Actions.BERSERK:
                    e.Anger.Increase(Emotion.LARGE);
                    e.Sanity = Mathf.MoveTowards(e.Sanity, 0f, Emotion.MEDIUM);
                    break;
                    
                case Actions.PANIC:
                    e.Fear.Increase(Emotion.SMALL);
                    e.Sanity = Mathf.MoveTowards(e.Sanity, 0f, Emotion.SMALL);
                    break;

                case Actions.SELF_REGULATE:
                    e.Stress = Mathf.MoveTowards(e.Stress, 0f, Emotion.SMALL * 2f);
                    e.Burnout = Mathf.MoveTowards(e.Burnout, 0f, Emotion.SMALL);
                    e.EmotionalDebt = Mathf.MoveTowards(e.EmotionalDebt, 0f, Emotion.SMALL);
                    e.Fear.Decrease(Emotion.MICRO);
                    e.Sadness.Decrease(Emotion.MICRO);
                    break;
            }
        }
        
        private static void ExecuteFlee(Actor actor, SensoryMemory senses)
        {
            // Try to find safe building
            Building safeBuilding = FindNearestSafeBuilding(actor);
            if (safeBuilding != null)
            {
                UnitActionSystem.GoToBuildingTarget(actor, safeBuilding);
            }
            else
            {
                // Just move away from danger
                if (senses.DangerVector != Vector2.zero)
                {
                    // The danger vector points away from threat already
                    Vector2 fleeTarget = new Vector2(actor.current_position.x, actor.current_position.y) + senses.DangerVector * 10f;
                    WorldTile fleeTile = MapBox.instance.GetTile((int)fleeTarget.x, (int)fleeTarget.y);
                    if (fleeTile != null)
                    {
                        actor.goTo(fleeTile);
                    }
                }
            }
        }
        
        private static void ExecuteProtect(Actor actor, UnitMind mind, SensoryMemory senses)
        {
            // Find the family member in danger
            if (actor.lover != null && actor.lover.isAlive())
            {
                bool loverInDanger = actor.lover.data.health < actor.lover.getMaxHealth() && actor.lover.attack_target != null;
                if (loverInDanger)
                {
                    // Attack whoever is attacking lover
                    Actor attacker = (Actor)actor.lover.attack_target;
                    if (attacker != null && attacker.isAlive())
                    {
                        mind.LastDecision = $"Protect {actor.lover.getName()} from {attacker.getName()}!";
                        UnitActionSystem.AttackHuntingTarget(actor, attacker);
                        return;
                    }
                }
            }
            
            // Fallback: attack nearest enemy
            Actor enemy = FindNearestEnemy(actor);
            if (enemy != null)
            {
                mind.LastDecision = $"Defend family from {enemy.getName()}!";
                UnitActionSystem.AttackHuntingTarget(actor, enemy);
            }
        }
        
        // ===============================================================================
        // HELPER FUNCTIONS
        // ===============================================================================
        
        private static Actor FindNearestEnemy(Actor actor)
        {
            Actor nearest = null;
            float minDist = float.MaxValue;
            
            IEnumerable<Actor> units = Finder.getUnitsFromChunk(actor.current_tile, UnitSensorySystem.VISION_RADIUS);
            foreach (Actor other in units)
            {
                if (other == actor || !other.isAlive()) continue;
                if (actor.kingdom != null && actor.kingdom.isEnemy(other.kingdom))
                {
                    float dist = Toolbox.Dist(actor.current_position.x, actor.current_position.y,
                        other.current_position.x, other.current_position.y);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = other;
                    }
                }
            }
            
            return nearest;
        }
        
        private static Actor FindNearestAlly(Actor actor)
        {
            Actor nearest = null;
            float minDist = float.MaxValue;
            
            IEnumerable<Actor> units = Finder.getUnitsFromChunk(actor.current_tile, UnitSensorySystem.VISION_RADIUS);
            foreach (Actor other in units)
            {
                if (other == actor || !other.isAlive()) continue;
                if (actor.kingdom != null && actor.kingdom == other.kingdom)
                {
                    float dist = Toolbox.Dist(actor.current_position.x, actor.current_position.y,
                        other.current_position.x, other.current_position.y);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = other;
                    }
                }
            }
            
            return nearest;
        }
        
        private static Building FindNearestSafeBuilding(Actor actor)
        {
            if (actor.city == null) return null;
            
            Building nearest = null;
            float minDist = float.MaxValue;
            
            try
            {
                foreach (Building b in actor.city.buildings)
                {
                    if (b == null || !b.isAlive()) continue;
                    
                    float dist = Toolbox.Dist(actor.current_position.x, actor.current_position.y,
                        b.current_tile.x, b.current_tile.y);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = b;
                    }
                }
            }
            catch { }
            
            return nearest;
        }
    }
}
