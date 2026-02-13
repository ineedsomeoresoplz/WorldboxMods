using System;
using UnityEngine;
using AIBox;

namespace AIBox
{
    /// <summary>
    /// Comprehensive reaction system that updates emotions based on:
    /// - Immediate threats (enemies, fire, projectiles)
    /// - Environmental conditions (biome, weather)
    /// - Social context (allies, family, friends)
    /// - Kingdom events (war, invasion, king decisions)
    /// - Memory triggers (loss, trauma)
    /// - Physical state (health, hunger)
    /// - Actions being performed
    /// </summary>
    public static class UnitReactiveSystem
    {
        public static void UpdateReaction(Actor actor, UnitMind mind, SensoryMemory senses, float deltaTime = -1f)
        {
            if (actor == null || mind == null) return;
            PersonalityTraits traits = mind.Traits;
            EmotionalSpectrum emotions = mind.Emotions;
            NeedsHierarchy needs = mind.Needs;
            if (mind.Memory == null) mind.Memory = new UnitMemory();

            if (traits == null || emotions == null || needs == null) return;

            emotions.EnsureInitialized();
            needs.ClampAll();

            float dt = deltaTime > 0f ? deltaTime : Mathf.Max(Time.deltaTime, 0.016f);
            
            bool peaceful = true;
            
            // ===============================================================================
            // 1. IMMEDIATE PHYSICAL THREATS
            // ===============================================================================
            
            // --- ENEMIES ---
            if (senses.EnemyCount > 0)
            {
                peaceful = false;
                float proximityFactor = 1f - Mathf.Clamp01(senses.ClosestEnemyDist / UnitSensorySystem.VISION_RADIUS);
                float threatIntensity = senses.ThreatLevel * proximityFactor;
                
                // Safety need drops with threats
                needs.Safety.Value = Mathf.MoveTowards(needs.Safety.Value, 0f, Emotion.MEDIUM * threatIntensity);
                
                if (traits.Bravery > 0.6f)
                {
                    // BRAVE: Anger rises, ready to fight
                    emotions.Anger.Increase(Emotion.SMALL * proximityFactor);
                    emotions.Anticipation.Increase(Emotion.SMALL);
                    
                    // High aggression = even more anger
                    if (traits.Aggression > 0.6f)
                        emotions.Anger.Increase(Emotion.SMALL * traits.Aggression);
                }
                else
                {
                    // COWARD: Fear spikes
                    emotions.Fear.Increase(Emotion.MEDIUM * proximityFactor);
                    emotions.Anger.Decrease(Emotion.SMALL);
                }
                
                // Combat stress affects sanity based on stability
                if (traits.Stability < 0.5f)
                {
                    emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0f, Emotion.MICRO * (1f - traits.Stability));
                }
                
                // Many enemies = more fear
                if (senses.EnemyCount > 3)
                {
                    emotions.Fear.Increase(Emotion.SMALL * (senses.EnemyCount / 5f));
                }
            }
            
            // --- FIRE & ENVIRONMENTAL HAZARDS ---
            if (senses.FireTileCount > 0)
            {
                peaceful = false;
                float fireFear = (traits.Bravery > 0.7f) ? Emotion.SMALL : Emotion.MEDIUM;
                emotions.Fear.Increase(fireFear * (senses.FireTileCount / 5f));
                emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0f, Emotion.MICRO);
                needs.Safety.Drain(Emotion.MEDIUM);
            }
            
            if (senses.ProjectileCount > 0)
            {
                peaceful = false;
                emotions.Fear.Increase(Emotion.MEDIUM);
                emotions.Surprise.Increase(Emotion.LARGE);
                needs.Safety.Drain(Emotion.SMALL);
            }
            
            // --- BIOME REACTIONS ---
            if (senses.OnCorruptedBiome)
            {
                emotions.Fear.Increase(Emotion.MICRO);
                emotions.Disgust.Increase(Emotion.SMALL);
                emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0f, Emotion.MICRO * 0.5f);
            }
            else if (senses.OnGoodBiome)
            {
                emotions.Joy.Increase(Emotion.MICRO);
                emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 1f, Emotion.MICRO);
            }
            
            // ===============================================================================
            // 2. SOCIAL CONTEXT
            // ===============================================================================
            
            bool isIsolated = senses.AllyCount == 0 && senses.EnemyCount == 0;
            
            if (isIsolated)
            {
                if (traits.Sociability > 0.6f)
                {
                    // Extroverts get lonely
                    emotions.Sadness.Increase(Emotion.MICRO);
                    emotions.Joy.Decrease(Emotion.MICRO);
                    needs.Companionship.Drain(Emotion.SMALL);
                }
                else
                {
                    // Introverts recover in solitude
                    emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 1f, Emotion.SMALL);
                    emotions.Joy.Increase(Emotion.MICRO);
                }
            }
            else if (senses.SeeFriendlyGathering)
            {
                if (traits.Sociability > 0.4f)
                {
                    // Being with friends
                    emotions.Joy.Increase(Emotion.SMALL);
                    emotions.Fear.Decrease(Emotion.MEDIUM);
                    emotions.Trust.Increase(Emotion.MICRO);
                    needs.Companionship.Satisfy(Emotion.MEDIUM);
                    needs.Belonging.Satisfy(Emotion.SMALL);
                }
            }
            
            // --- FAMILY NEARBY ---
            if (senses.SeeLover)
            {
                emotions.Love.Increase(Emotion.SMALL);
                emotions.Joy.Increase(Emotion.SMALL);
                emotions.Fear.Decrease(Emotion.SMALL);
                needs.Companionship.Satisfy(Emotion.LARGE);
            }
            
            if (senses.SeeFamily)
            {
                emotions.Love.Increase(Emotion.SMALL);
                emotions.Joy.Increase(Emotion.MICRO);
            }
            
            // ===============================================================================
            // 3. ACUTE TRAUMA & MEMORY TRIGGERS
            // ===============================================================================
            
            // --- FAMILY IN DANGER ---
            if (senses.SawFamilyInDanger)
            {
                peaceful = false;
                
                if (traits.Empathy > 0.4f)
                {
                    // Protective rage
                    emotions.Anger.Increase(Emotion.MAJOR);
                    emotions.Fear.Decrease(Emotion.MEDIUM); // Adrenaline
                    emotions.Love.Increase(Emotion.MEDIUM);
                }
                else
                {
                    // Self-preservation
                    emotions.Fear.Increase(Emotion.MEDIUM);
                }
            }
            
            // --- SAW FAMILY CORPSE ---
            if (senses.SawCorpseFamily)
            {
                peaceful = false;
                
                // Massive grief hit
                float griefIntensity = Emotion.SPIKE;
                if (traits.Empathy > 0.6f) griefIntensity += Emotion.MAJOR;
                
                emotions.Sadness.Increase(griefIntensity);
                emotions.Joy.Value = Mathf.MoveTowards(emotions.Joy.Value, 0f, Emotion.MAJOR);
                emotions.Surprise.Increase(Emotion.MAJOR);
                
                // Trauma
                if (traits.Stability < 0.8f)
                    emotions.Sanity = Mathf.Clamp01(emotions.Sanity - Emotion.LARGE);
                
                // Vengeance
                if (traits.Vengefulness > 0.5f)
                    emotions.Anger.Increase(Emotion.MAJOR * traits.Vengefulness);
            }
            
            // --- SAW KINGDOM MEMBER DIE ---
            if (senses.SawCorpseKingdom)
            {
                emotions.Sadness.Increase(Emotion.MEDIUM);
                if (traits.Empathy > 0.5f)
                    emotions.Sadness.Increase(Emotion.SMALL);
            }
            
            // --- MISSING LOVED ONES ---
            if (senses.MissingChild)
            {
                peaceful = false;
                emotions.Sadness.Increase(Emotion.LARGE);
                emotions.Fear.Increase(Emotion.MEDIUM); // Worry
                emotions.Joy.Decrease(Emotion.MAJOR);
                emotions.Anticipation.Value = Mathf.MoveTowards(emotions.Anticipation.Value, 0f, Emotion.MEDIUM);
            }
            
            if (senses.MissingPartner)
            {
                peaceful = false;
                emotions.Sadness.Increase(Emotion.MEDIUM);
                emotions.Love.Increase(Emotion.SMALL); // Longing
                
                if (traits.Vengefulness > 0.7f)
                    emotions.Anger.Increase(Emotion.SMALL);
            }
            
            // --- HOME DESTROYED ---
            if (senses.HomeDestroyed)
            {
                peaceful = false;
                emotions.Sadness.Increase(Emotion.LARGE);
                emotions.Anger.Increase(Emotion.MEDIUM);
                needs.Shelter.Value = 0.1f;
                needs.Safety.Drain(Emotion.MAJOR);
                
                if (traits.Stability < 0.6f)
                    emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0.3f, Emotion.MEDIUM);
            }
            
            // ===============================================================================
            // 4. PHYSICAL STATE
            // ===============================================================================
            
            // --- RECENTLY HURT ---
            if (senses.WasHurtRecently)
            {
                peaceful = false;
                emotions.Fear.Increase(Emotion.LARGE);
                emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0f, Emotion.MEDIUM);
                emotions.Surprise.Increase(Emotion.MEDIUM);
                needs.Health.Drain(Emotion.LARGE);
                
                if (traits.Bravery > 0.4f)
                    emotions.Anger.Increase(Emotion.MEDIUM);
            }
            
            // --- HUNGER ---
            // Hunger NEED is inverse of nutrition: high nutrition = low hunger need
            float nutrition = actor.data.nutrition;
            float maxNutrition = 100f; // Typical max nutrition
            
            // Calculate hunger need: 0 = full/not hungry, 1 = starving
            needs.Hunger.Value = 1f - Mathf.Clamp01(nutrition / maxNutrition);
            
            // Emotional effects of hunger
            if (nutrition < 30)
            {
                emotions.Joy.Decrease(Emotion.SMALL);
                emotions.Anger.Increase(Emotion.MICRO); // Hangry
                
                if (nutrition < 10)
                {
                    emotions.Fear.Increase(Emotion.SMALL); // Survival fear
                }
            }
            else if (nutrition > 80)
            {
                // Well fed = content
                emotions.Joy.Increase(Emotion.MICRO);
            }
            
            // --- HEALTH ---
            float hpRatio = (float)actor.data.health / actor.getMaxHealth();
            if (hpRatio < 0.3f)
            {
                emotions.Fear.Increase(Emotion.MEDIUM);
                needs.Health.Value = hpRatio;
            }
            
            // ===============================================================================
            // 5. KINGDOM & CITY EVENTS
            // ===============================================================================
            
            UpdateKingdomReactions(actor, mind, senses);
            
            // ===============================================================================
            // 6. ACTION & JOB CONTEXT
            // ===============================================================================
            
            // --- COMBAT STATE ---
            if (actor.has_attack_target)
            {
                peaceful = false;
                emotions.Fear.Increase(Emotion.SMALL);
                emotions.Anger.Increase(Emotion.SMALL);
                emotions.Anticipation.Increase(Emotion.MEDIUM);
            }
            
            // --- PROFESSION ---
            string jobID = actor.data.profession.ToString().ToLower();
            if (!string.IsNullOrEmpty(jobID))
            {
                if (jobID == "warrior" || jobID == "soldier")
                {
                    // Soldiers are hardened over time
                    traits.Bravery = Mathf.MoveTowards(traits.Bravery, 1f, Emotion.MICRO * 0.1f);
                    traits.Stability = Mathf.MoveTowards(traits.Stability, 0.7f, Emotion.MICRO * 0.1f);
                }
                else if (jobID == "farmer")
                {
                    if (peaceful) emotions.Joy.Increase(Emotion.MICRO);
                }
                else if (jobID == "miner")
                {
                    emotions.Sadness.Increase(Emotion.MICRO * 0.5f); // Hard labor
                }
            }
            
            // --- CURRENT TASK ---
            try
            {
                string task = actor.getTaskText();
                if (!string.IsNullOrEmpty(task))
                {
                    task = task.ToLower();
                    
                    if (task.Contains("harvest") || task.Contains("farm"))
                    {
                        emotions.Joy.Increase(Emotion.MICRO);
                        needs.Purpose.Satisfy(Emotion.MICRO);
                    }
                    else if (task.Contains("build") || task.Contains("construct"))
                    {
                        emotions.Pride.Increase(Emotion.SMALL);
                        emotions.Joy.Increase(Emotion.MICRO);
                        needs.Purpose.Satisfy(Emotion.SMALL);
                    }
                    else if (task.Contains("extinguish") || task.Contains("fire"))
                    {
                        emotions.Fear.Increase(Emotion.MEDIUM);
                        emotions.Pride.Increase(Emotion.MICRO); // Helping
                    }
                    else if (task.Contains("trade"))
                    {
                        emotions.Joy.Increase(Emotion.MICRO);
                        needs.Resources.Satisfy(Emotion.SMALL);
                    }
                    // ======= NEW REACTIONS =======
                    else if (task.Contains("baby") || task.Contains("breed") || task.Contains("mate") || task.Contains("love"))
                    {
                        // Making babies / romance
                        emotions.Joy.Increase(Emotion.LARGE);
                        emotions.Love.Increase(Emotion.MAJOR);
                        emotions.Anticipation.Increase(Emotion.MEDIUM);
                        needs.Companionship.Satisfy(Emotion.LARGE);
                        emotions.Fear.Decrease(Emotion.SMALL);
                        emotions.Sadness.Decrease(Emotion.MEDIUM);
                    }
                    else if (task.Contains("pregnant") || task.Contains("expecting"))
                    {
                        emotions.Joy.Increase(Emotion.MEDIUM);
                        emotions.Love.Increase(Emotion.MEDIUM);
                        emotions.Anticipation.Increase(Emotion.LARGE);
                    }
                    else if (task.Contains("give birth") || task.Contains("born"))
                    {
                        emotions.Joy.Increase(Emotion.MAJOR);
                        emotions.Love.Increase(Emotion.MAJOR);
                        emotions.Pride.Increase(Emotion.LARGE);
                    }
                    else if (task.Contains("kiss") || task.Contains("cuddle") || task.Contains("talk to lover"))
                    {
                        emotions.Joy.Increase(Emotion.SMALL);
                        emotions.Love.Increase(Emotion.SMALL);
                    }
                    else if (task.Contains("attack") || task.Contains("fight") || task.Contains("hunt"))
                    {
                        emotions.Anger.Increase(Emotion.SMALL);
                        emotions.Fear.Increase(Emotion.MICRO);
                        emotions.Anticipation.Increase(Emotion.SMALL);
                    }
                    else if (task.Contains("flee") || task.Contains("run"))
                    {
                        emotions.Fear.Increase(Emotion.MEDIUM);
                    }
                    else if (task.Contains("eat") || task.Contains("drink"))
                    {
                        emotions.Joy.Increase(Emotion.MICRO);
                        needs.Hunger.Satisfy(Emotion.MEDIUM);
                    }
                    else if (task.Contains("sleep") || task.Contains("rest"))
                    {
                        emotions.Joy.Increase(Emotion.MICRO);
                        emotions.Fear.Decrease(Emotion.SMALL);
                    }
                }
            }
            catch { }
            
            // ===============================================================================
            // 7. RECOVERY (Peaceful State)
            // ===============================================================================
            
            if (peaceful)
            {
                // Slow decay toward baselines
                emotions.DecayAll(dt);
                needs.Safety.Satisfy(Emotion.SMALL * Mathf.Clamp(dt, 0.5f, 2f));
            }
            
            // ===============================================================================
            // 8. PERSONALITY PLASTICITY (Long-term change)
            // ===============================================================================
            
            ApplyAdvancedEmotionDynamics(mind, senses, peaceful, dt);
            UpdatePersonalityPlasticity(traits, emotions, dt);
            
            // Final clamp
            emotions.ClampAll();
            traits.Clamp();
            needs.ClampAll();
        }
        
        /// <summary>
        /// React to kingdom-level events
        /// </summary>
        private static void UpdateKingdomReactions(Actor actor, UnitMind mind, SensoryMemory senses)
        {
            if (actor.kingdom == null) return;
            
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            UnitMemory memory = mind.Memory;
            NeedsHierarchy needs = mind.Needs;
            
            // Check if kingdom is at war
            bool atWar = false;
            using (ListPool<Kingdom> enemies = actor.kingdom.getEnemiesKingdoms())
            {
                atWar = enemies.Count > 0;
            }
            
            if (atWar && !memory.KingdomAtWar)
            {
                // War just declared!
                memory.KingdomAtWar = true;
                emotions.Fear.Increase(Emotion.MEDIUM);
                emotions.Anger.Increase(Emotion.SMALL);
                emotions.Anticipation.Increase(Emotion.LARGE);
                needs.Safety.Drain(Emotion.MEDIUM);
                
                if (traits.Loyalty > 0.6f)
                {
                    emotions.Pride.Increase(Emotion.SMALL); // Patriotism
                }
                
                memory.AddEvent(MemoryTypes.WAR_DECLARED, actor.kingdom.id.ToString(), 
                    actor.kingdom.name, actor.current_position, 0.7f);
            }
            else if (!atWar && memory.KingdomAtWar)
            {
                // Peace!
                memory.KingdomAtWar = false;
                emotions.Joy.Increase(Emotion.LARGE);
                emotions.Fear.Decrease(Emotion.LARGE);
                needs.Safety.Satisfy(Emotion.LARGE);
                
                memory.AddEvent(MemoryTypes.PEACE_MADE, actor.kingdom.id.ToString(),
                    actor.kingdom.name, actor.current_position, 0.5f);
            }
            
            // Ongoing war stress
            if (memory.KingdomAtWar)
            {
                emotions.Fear.Increase(Emotion.MICRO);
                emotions.Joy.Decrease(Emotion.MICRO);
                
                if (traits.Stability < 0.5f)
                    emotions.Sanity = Mathf.MoveTowards(emotions.Sanity, 0.5f, Emotion.MICRO * 0.5f);
            }
            
            // Check king status
            Actor king = actor.kingdom.king;
            if (king != null)
            {
                string kingName = king.getName();
                if (memory.LastKingName != null && memory.LastKingName != kingName)
                {
                    // King changed (died or replaced)
                    emotions.Sadness.Increase(Emotion.MEDIUM);
                    emotions.Surprise.Increase(Emotion.LARGE);
                    emotions.Fear.Increase(Emotion.SMALL); // Uncertainty
                    
                    if (traits.Loyalty > 0.5f)
                    {
                        emotions.Sadness.Increase(Emotion.SMALL);
                    }
                    
                    memory.AddEvent(MemoryTypes.KING_DIED, memory.LastKingName, 
                        memory.LastKingName, actor.current_position, 0.8f);
                }
                memory.LastKingName = kingName;
            }
            
            // City under attack
            if (actor.city != null)
            {
                // Check if city is being attacked (enemies in city zone)
                bool cityUnderAttack = false;
                try
                {
                    if (actor.city.status.population > 0)
                    {
                        // Simplified check - are there enemies nearby and in our city zone?
                        cityUnderAttack = senses.EnemyCount > 2 && senses.ClosestEnemyDist < 10f;
                    }
                }
                catch { }
                
                if (cityUnderAttack)
                {
                    if (!memory.KingdomUnderInvasion)
                    {
                        memory.KingdomUnderInvasion = true;
                        emotions.Fear.Increase(Emotion.LARGE);
                        emotions.Anger.Increase(Emotion.MEDIUM);
                        needs.Safety.Value = 0.1f;
                        
                        memory.AddEvent(MemoryTypes.INVASION, actor.city.id.ToString(),
                            actor.city.name, actor.current_position, 0.9f);
                    }
                }
                else if (memory.KingdomUnderInvasion)
                {
                    memory.KingdomUnderInvasion = false;
                    emotions.Joy.Increase(Emotion.MEDIUM);
                    needs.Safety.Satisfy(Emotion.MEDIUM);
                }
            }
        }

        private static void ApplyAdvancedEmotionDynamics(UnitMind mind, SensoryMemory senses, bool peaceful, float deltaTime)
        {
            EmotionalSpectrum emotions = mind.Emotions;
            PersonalityTraits traits = mind.Traits;
            NeedsHierarchy needs = mind.Needs;
            UnitMemory memory = mind.Memory;

            float dt = Mathf.Clamp(deltaTime, 0.01f, 2f);
            bool hasSupport = senses.AllyCount > 0 || senses.SeeLover || senses.SeeFamily || senses.SeeFriendlyGathering;
            float socialExposure = Mathf.Clamp01((senses.AllyCount / 6f) + (senses.SeeLover ? 0.35f : 0f) + (senses.SeeFamily ? 0.2f : 0f));
            float threatLevel = Mathf.Clamp01(senses.ThreatLevel + (senses.FireTileCount * 0.03f) + (senses.ProjectileCount * 0.04f) + (senses.WasHurtRecently ? 0.2f : 0f));

            if (memory != null) memory.UpdateExposure(senses.InDanger, hasSupport, dt);

            emotions.UpdateMetaState(traits, needs, memory, senses.InDanger, peaceful, socialExposure, threatLevel, dt);
            ApplyNeedEmotionCoupling(needs, emotions, traits, dt);
            ApplyMemoryEchoes(memory, emotions, traits, dt);

            if (!senses.InDanger && peaceful)
            {
                float recovery = Emotion.MICRO * Mathf.Clamp(dt, 0.5f, 2.5f);
                emotions.Fear.Decrease(recovery * (0.8f + emotions.Regulation));
                emotions.Anger.Decrease(recovery * (0.6f + emotions.Regulation));
                emotions.Sadness.Decrease(recovery * 0.4f);
                emotions.Joy.Increase(recovery * (0.3f + emotions.Mood));
            }

            if (senses.InDanger)
            {
                float pressure = Mathf.Clamp01(threatLevel + (emotions.Stress * 0.4f));
                emotions.Fear.Increase(Emotion.MICRO * pressure * (1.2f + emotions.Volatility));
                if (traits.Aggression > 0.55f || traits.Vengefulness > 0.5f)
                {
                    emotions.Anger.Increase(Emotion.MICRO * pressure * (0.8f + traits.Aggression));
                }
            }

            float chaos = (emotions.Stress + emotions.Arousal + (1f - emotions.Sanity)) / 3f;
            if (chaos > 0.7f)
            {
                emotions.Surprise.Increase(Emotion.MICRO * chaos * emotions.Volatility);
                emotions.Anticipation.Increase(Emotion.MICRO * chaos * 0.8f);
            }
        }

        private static void ApplyNeedEmotionCoupling(NeedsHierarchy needs, EmotionalSpectrum emotions, PersonalityTraits traits, float deltaTime)
        {
            if (needs == null || emotions == null || traits == null) return;

            float scale = Mathf.Clamp(deltaTime, 0.5f, 2f);
            float hunger = needs.Hunger.Urgency;
            float safety = needs.Safety.Urgency;
            float health = needs.Health.Urgency;
            float companionship = needs.Companionship.Urgency;
            float purpose = needs.Purpose.Urgency;

            if (hunger > 0.5f)
            {
                emotions.Anger.Increase(Emotion.MICRO * hunger * scale);
                emotions.Joy.Decrease(Emotion.MICRO * hunger * scale);
            }

            if (safety > 0.5f)
            {
                emotions.Fear.Increase(Emotion.MICRO * safety * scale);
                emotions.Trust.Decrease(Emotion.MICRO * safety * 0.5f * scale);
            }
            else
            {
                emotions.Trust.Increase(Emotion.MICRO * (1f - safety) * 0.6f * scale);
            }

            if (health > 0.4f)
            {
                emotions.Fear.Increase(Emotion.MICRO * health * scale);
                emotions.Sadness.Increase(Emotion.MICRO * health * 0.5f * scale);
            }

            if (companionship > 0.55f)
            {
                emotions.Sadness.Increase(Emotion.MICRO * companionship * scale);
                if (traits.Sociability > 0.45f) emotions.Love.Increase(Emotion.MICRO * companionship * 0.6f * scale);
            }
            else
            {
                emotions.Joy.Increase(Emotion.MICRO * (1f - companionship) * 0.5f * scale);
            }

            if (purpose > 0.6f)
            {
                emotions.Anticipation.Decrease(Emotion.MICRO * purpose * 0.8f * scale);
                emotions.Sadness.Increase(Emotion.MICRO * purpose * 0.5f * scale);
                emotions.Burnout = Mathf.Clamp01(emotions.Burnout + Emotion.MICRO * purpose * 0.4f * scale);
            }
            else
            {
                emotions.Pride.Increase(Emotion.MICRO * (1f - purpose) * 0.5f * scale);
            }
        }

        private static void ApplyMemoryEchoes(UnitMemory memory, EmotionalSpectrum emotions, PersonalityTraits traits, float deltaTime)
        {
            if (memory == null || emotions == null || memory.ImportantEvents == null || memory.ImportantEvents.Count == 0) return;

            float now = Time.time;
            float griefEcho = 0f;
            float fearEcho = 0f;
            float prideEcho = 0f;
            float trustEcho = 0f;
            int start = Mathf.Max(0, memory.ImportantEvents.Count - 8);

            for (int i = start; i < memory.ImportantEvents.Count; i++)
            {
                MemoryEvent evt = memory.ImportantEvents[i];
                float age = now - evt.Timestamp;
                if (age < 0f || age > 240f) continue;

                float recency = 1f - Mathf.Clamp01(age / 240f);
                float intensity = Mathf.Clamp01(evt.Intensity);
                float weight = recency * intensity * 0.2f;

                if (evt.Type == MemoryTypes.DEATH_FAMILY || evt.Type == MemoryTypes.DEATH_FRIEND || evt.Type == MemoryTypes.HEARTBREAK)
                {
                    griefEcho += weight;
                    fearEcho += weight * 0.4f;
                }
                else if (evt.Type == MemoryTypes.WAR_DECLARED || evt.Type == MemoryTypes.INVASION || evt.Type == MemoryTypes.ATTACKED)
                {
                    fearEcho += weight * 0.9f;
                }
                else if (evt.Type == MemoryTypes.VICTORY || evt.Type == MemoryTypes.PROMOTION || evt.Type == MemoryTypes.CHILD_BORN)
                {
                    prideEcho += weight * 0.9f;
                    trustEcho += weight * 0.4f;
                }
                else if (evt.Type == MemoryTypes.PEACE_MADE || evt.Type == MemoryTypes.MADE_FRIEND)
                {
                    trustEcho += weight * 0.8f;
                }
            }

            float scale = Mathf.Clamp(deltaTime, 0.5f, 2f);
            if (griefEcho > 0f)
            {
                emotions.Sadness.Increase(Emotion.MICRO * griefEcho * 8f * scale);
                emotions.Joy.Decrease(Emotion.MICRO * griefEcho * 4f * scale);
                emotions.Trauma = Mathf.Clamp01(emotions.Trauma + Emotion.MICRO * griefEcho * 3f * (1f - traits.Stability) * scale);
            }
            if (fearEcho > 0f)
            {
                emotions.Fear.Increase(Emotion.MICRO * fearEcho * 7f * scale);
                emotions.Stress = Mathf.Clamp01(emotions.Stress + Emotion.MICRO * fearEcho * 4f * scale);
            }
            if (prideEcho > 0f)
            {
                emotions.Pride.Increase(Emotion.MICRO * prideEcho * 6f * scale);
                emotions.Joy.Increase(Emotion.MICRO * prideEcho * 4f * scale);
                emotions.Burnout = Mathf.MoveTowards(emotions.Burnout, 0f, Emotion.MICRO * prideEcho * 2f * scale);
            }
            if (trustEcho > 0f)
            {
                emotions.Trust.Increase(Emotion.MICRO * trustEcho * 6f * scale);
                emotions.Fear.Decrease(Emotion.MICRO * trustEcho * 3f * scale);
            }
        }
        
        /// <summary>
        /// Chronic emotions reshape personality over time
        /// </summary>
        private static void UpdatePersonalityPlasticity(PersonalityTraits traits, EmotionalSpectrum emotions, float deltaTime)
        {
            float rateScale = Mathf.Clamp(deltaTime, 0.5f, 3f);
            float plasticityRate = Emotion.MICRO * 0.1f * rateScale;
            
            // Chronic Fear reduces Bravery
            if (emotions.Fear.IsHigh)
            {
                traits.Bravery = Mathf.MoveTowards(traits.Bravery, 0f, plasticityRate);
                traits.Stability = Mathf.MoveTowards(traits.Stability, 0f, plasticityRate);
            }
            
            // Chronic Anger increases Vengefulness
            if (emotions.Anger.IsHigh)
            {
                traits.Vengefulness = Mathf.MoveTowards(traits.Vengefulness, 1f, plasticityRate);
                traits.Empathy = Mathf.MoveTowards(traits.Empathy, 0f, plasticityRate);
            }
            
            // Chronic Joy increases Stability and Sociability
            if (emotions.Joy.IsHigh)
            {
                traits.Sociability = Mathf.MoveTowards(traits.Sociability, 1f, plasticityRate);
                traits.Stability = Mathf.MoveTowards(traits.Stability, 1f, plasticityRate);
            }
            
            // Chronic Sadness reduces Ambition
            if (emotions.Sadness.IsHigh)
            {
                traits.Ambition = Mathf.MoveTowards(traits.Ambition, 0f, plasticityRate);
                traits.Bravery = Mathf.MoveTowards(traits.Bravery, 0f, plasticityRate);
            }
            
            // High Trust increases Sociability
            if (emotions.Trust.IsHigh)
            {
                traits.Sociability = Mathf.MoveTowards(traits.Sociability, 1f, plasticityRate);
            }
            
            // High Guilt increases Morality
            if (emotions.Guilt.IsHigh)
            {
                traits.Morality = Mathf.MoveTowards(traits.Morality, 1f, plasticityRate);
                traits.Honesty = Mathf.MoveTowards(traits.Honesty, 1f, plasticityRate);
            }
            
            // High Pride increases Ambition
            if (emotions.Pride.IsHigh)
            {
                traits.Ambition = Mathf.MoveTowards(traits.Ambition, 1f, plasticityRate);
            }

            if (emotions.Stress > 0.75f)
            {
                traits.Rationality = Mathf.MoveTowards(traits.Rationality, 0f, plasticityRate * 0.7f);
                traits.Honor = Mathf.MoveTowards(traits.Honor, 0f, plasticityRate * 0.5f);
            }

            if (emotions.Regulation > 0.65f && emotions.Mood > 0.55f)
            {
                traits.Rationality = Mathf.MoveTowards(traits.Rationality, 1f, plasticityRate * 0.6f);
                traits.Honor = Mathf.MoveTowards(traits.Honor, 1f, plasticityRate * 0.4f);
            }
        }
    }
}
