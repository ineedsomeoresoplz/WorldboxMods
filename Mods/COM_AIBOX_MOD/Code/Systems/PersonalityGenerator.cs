using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBox
{
    public static class PersonalityGenerator
    {
        // =====================================================================================
        // TRAIT MODIFIERS - Maps in-game WorldBox traits to our personality dimensions
        // =====================================================================================
        
        private static Dictionary<string, Action<PersonalityTraits>> TraitModifiers = new Dictionary<string, Action<PersonalityTraits>>
        {
            // === COGNITIVE TRAITS ===
            { "genius", (t) => { t.Rationality += 0.4f; t.Curiosity += 0.3f; } },
            { "stupid", (t) => { t.Rationality -= 0.4f; t.Curiosity -= 0.2f; } },
            { "thief", (t) => { t.Honesty -= 0.4f; t.Morality -= 0.3f; t.Greed += 0.3f; } },
            { "wise", (t) => { t.Rationality += 0.3f; t.Stability += 0.2f; } },
            
            // === MIND TRAITS ===
            { "ambitious", (t) => { t.Ambition += 0.5f; t.Loyalty -= 0.2f; } },
            { "content", (t) => { t.Ambition -= 0.3f; t.Stability += 0.2f; t.Greed -= 0.2f; } },
            { "greedy", (t) => { t.Greed += 0.4f; t.Empathy -= 0.2f; t.Ambition += 0.2f; } },
            { "honest", (t) => { t.Honesty += 0.4f; t.Trust += 0.2f; t.Loyalty += 0.1f; } },
            { "deceitful", (t) => { t.Honesty -= 0.4f; t.Loyalty -= 0.3f; } },
            { "paranoid", (t) => { t.Trust -= 0.4f; t.Stability -= 0.2f; t.Loyalty -= 0.2f; } },
            { "hotheaded", (t) => { t.Aggression += 0.4f; t.Stability -= 0.3f; t.Rationality -= 0.2f; } },
            { "peaceful", (t) => { t.Aggression -= 0.5f; t.Empathy += 0.2f; } },
            { "psychopath", (t) => { t.Empathy = 0.05f; t.Morality -= 0.5f; t.Stability -= 0.3f; } },
            { "evil", (t) => { t.Morality -= 0.5f; t.Aggression += 0.3f; t.Empathy -= 0.3f; } },
            { "lustful", (t) => { t.Sociability += 0.3f; } },
            { "strong_minded", (t) => { t.Stability += 0.5f; t.Bravery += 0.2f; } },
            
            // === SPIRIT TRAITS ===
            { "savage", (t) => { t.Aggression += 0.4f; t.Bravery += 0.2f; t.Morality -= 0.2f; } },
            { "blessed", (t) => { t.Morality += 0.3f; t.Stability += 0.2f; } },
            { "lucky", (t) => { t.Stability += 0.1f; } },
            { "unlucky", (t) => { t.Stability -= 0.1f; } },
            { "immortal", (t) => { t.Ambition += 0.3f; t.Loyalty -= 0.3f; } },
            
            // === PHYSIQUE/BODY TRAITS ===
            { "tough", (t) => { t.Stability += 0.2f; t.Bravery += 0.1f; } },
            { "weak", (t) => { t.Bravery -= 0.2f; t.Stability -= 0.1f; } },
            
            // === SOCIAL TRAITS ===
            { "charismatic", (t) => { t.Sociability += 0.4f; t.Trust += 0.2f; } },
            { "shy", (t) => { t.Sociability -= 0.3f; } },
            
            // === SPECIAL/CURSE TRAITS ===
            { "madness", (t) => { t.Stability = 0.1f; t.Rationality -= 0.5f; } },
            { "bloodlust", (t) => { t.Aggression += 0.5f; t.Empathy -= 0.4f; t.Vengefulness += 0.3f; } },
            
            // === CUSTOM MOD TRAITS ===
            { "Corrupt", (t) => { t.Ambition += 0.4f; t.Honor -= 0.5f; t.Morality -= 0.3f; } },
            { "Trader", (t) => { t.Sociability += 0.2f; t.Ambition += 0.2f; t.Greed += 0.1f; } },
            { "well_paid", (t) => { t.Loyalty += 0.2f; t.Stability += 0.1f; } },
            { "unpaid", (t) => { t.Loyalty -= 0.3f; t.Vengefulness += 0.2f; } }
        };

        // =====================================================================================
        // RACE MODIFIERS - Baseline personality by race
        // =====================================================================================
        
        private static Dictionary<string, Action<PersonalityTraits>> RaceModifiers = new Dictionary<string, Action<PersonalityTraits>>
        {
            // Civilized Races
            { "human", (t) => { /* Baseline human - no modifiers */ } },
            { "elf", (t) => { t.Stability += 0.2f; t.Honor += 0.2f; t.Empathy += 0.1f; t.Curiosity += 0.1f; } },
            { "orc", (t) => { t.Bravery += 0.2f; t.Aggression += 0.3f; t.Empathy -= 0.2f; t.Vengefulness += 0.2f; } },
            { "dwarf", (t) => { t.Ambition += 0.2f; t.Greed += 0.3f; t.Stability += 0.2f; t.Honor += 0.1f; } },
            
            // Magic Users
            { "civ_necromancer", (t) => { t.Ambition += 0.5f; t.Stability -= 0.4f; t.Empathy -= 0.5f; t.Morality -= 0.4f; t.Curiosity += 0.3f; } },
            { "civ_evil_mage", (t) => { t.Ambition += 0.4f; t.Honor -= 0.3f; t.Vengefulness += 0.3f; t.Morality -= 0.3f; } },
            { "civ_white_mage", (t) => { t.Empathy += 0.5f; t.Honor += 0.3f; t.Stability += 0.2f; t.Morality += 0.3f; } },
            { "civ_druid", (t) => { t.Empathy += 0.4f; t.Stability += 0.2f; t.Aggression -= 0.2f; } },
            
            // Supernatural
            { "civ_demon", (t) => { t.Vengefulness += 0.6f; t.Empathy -= 0.6f; t.Stability -= 0.3f; t.Bravery += 0.4f; t.Morality -= 0.5f; } },
            { "civ_cold_one", (t) => { t.Vengefulness += 0.4f; t.Empathy -= 0.5f; t.Bravery += 0.2f; } },
            { "civ_angel", (t) => { t.Honor += 0.5f; t.Empathy += 0.4f; t.Stability += 0.4f; t.Ambition -= 0.4f; t.Morality += 0.5f; } },
            { "civ_angle", (t) => { t.Honor += 0.5f; t.Empathy += 0.4f; t.Stability += 0.4f; t.Ambition -= 0.4f; t.Morality += 0.5f; } }, // Typo in game
            { "civ_ghost", (t) => { t.Bravery -= 0.3f; t.Stability -= 0.2f; t.Sociability -= 0.4f; } },
            
            // Sci-Fi
            { "civ_alien", (t) => { t.Curiosity += 0.6f; t.Empathy -= 0.3f; t.Rationality += 0.3f; } },
            { "civ_robot", (t) => { t.Stability += 0.7f; t.Empathy -= 0.4f; t.Rationality += 0.4f; t.Sociability -= 0.3f; } },
            
            // Animals/Creatures
            { "civ_wolf", (t) => { t.Vengefulness += 0.2f; t.Sociability += 0.3f; t.Loyalty += 0.3f; } },
            { "civ_bear", (t) => { t.Vengefulness += 0.3f; t.Bravery += 0.2f; t.Aggression += 0.2f; } },
            { "civ_monkey", (t) => { t.Curiosity += 0.5f; t.Sociability += 0.4f; t.Stability -= 0.2f; } },
            { "civ_rat", (t) => { t.Bravery -= 0.3f; t.Greed += 0.3f; t.Stability -= 0.1f; } },
            
            // Special
            { "civ_bandit", (t) => { t.Ambition += 0.4f; t.Honor -= 0.5f; t.Vengefulness += 0.3f; t.Morality -= 0.4f; } },
            { "civ_capybara", (t) => { t.Sociability += 0.5f; t.Vengefulness -= 0.5f; t.Stability += 0.5f; t.Aggression -= 0.4f; } },
        };

        // =====================================================================================
        // GENERATION
        // =====================================================================================
        
        public static UnitMind Generate(Actor actor)
        {
            UnitMind mind = new UnitMind();
            PersonalityTraits t = mind.Traits;
            
            // 1. Random Variance (Genetic Base) - Normal distribution around 0.5
            t.Bravery = RandomNormal(0.5f, 0.15f);
            t.Empathy = RandomNormal(0.5f, 0.15f);
            t.Stability = RandomNormal(0.5f, 0.15f);
            t.Sociability = RandomNormal(0.5f, 0.15f);
            t.Honesty = RandomNormal(0.5f, 0.15f);
            t.Morality = RandomNormal(0.5f, 0.1f);
            t.Honor = RandomNormal(0.5f, 0.15f);
            t.Ambition = RandomNormal(0.4f, 0.2f);
            t.Greed = RandomNormal(0.3f, 0.15f);
            t.Vengefulness = RandomNormal(0.3f, 0.15f);
            t.Rationality = RandomNormal(0.5f, 0.15f);
            t.Curiosity = RandomNormal(0.5f, 0.2f);
            t.Aggression = RandomNormal(0.3f, 0.15f);
            t.Loyalty = RandomNormal(0.5f, 0.15f);
            t.Trust = RandomNormal(0.5f, 0.15f);
            
            // 2. Apply Race Modifiers
            ApplyRaceModifiers(actor, t);
            
            // 3. Apply Trait Modifiers
            if (actor.data.saved_traits != null)
            {
                foreach (string traitId in actor.data.saved_traits)
                {
                    if (TraitModifiers.ContainsKey(traitId))
                    {
                        TraitModifiers[traitId](t);
                    }
                }
            }
            
            // 4. Age-based adjustments
            ApplyAgeModifiers(actor, t);
            
            // 5. Initial emotional state based on personality
            InitializeEmotions(mind);
            
            // 6. Initialize needs based on current state
            InitializeNeeds(actor, mind);
            
            t.Clamp();
            return mind;
        }
        
        private static void ApplyRaceModifiers(Actor actor, PersonalityTraits t)
        {
            string race = actor.asset.id;
            if (string.IsNullOrEmpty(race)) return;
            
            // Normalize: unit_human -> human
            if (race.StartsWith("unit_")) race = race.Replace("unit_", "");
            
            if (RaceModifiers.ContainsKey(race))
            {
                RaceModifiers[race](t);
            }
        }
        
        private static void ApplyAgeModifiers(Actor actor, PersonalityTraits t)
        {
            int age = actor.getAge();
            
            // Children are more curious, less stable
            if (age < 18)
            {
                t.Curiosity += 0.2f;
                t.Stability -= 0.1f;
                t.Rationality -= 0.1f;
            }
            // Elders are more stable, less ambitious
            else if (age > 60)
            {
                t.Stability += 0.2f;
                t.Ambition -= 0.2f;
                t.Aggression -= 0.1f;
            }
        }
        
        private static void InitializeEmotions(UnitMind mind)
        {
            EmotionalSpectrum e = mind.Emotions;
            PersonalityTraits t = mind.Traits;

            e.Joy.Value = Mathf.Clamp01(0.3f + (t.Stability * 0.25f) + (t.Trust * 0.1f) - (t.Aggression * 0.08f));
            e.Joy.Baseline = Mathf.Clamp01(0.2f + (t.Stability * 0.3f) + (t.Honor * 0.1f));

            e.Fear.Value = Mathf.Clamp01(0.05f + ((1f - t.Bravery) * 0.18f) + ((1f - t.Stability) * 0.12f));
            e.Fear.Baseline = Mathf.Clamp01(0.02f + ((1f - t.Bravery) * 0.12f) + ((1f - t.Trust) * 0.08f));

            e.Anger.Value = Mathf.Clamp01((t.Aggression * 0.22f) + (t.Vengefulness * 0.12f));
            e.Anger.Baseline = Mathf.Clamp01(0.01f + (t.Aggression * 0.18f) + (t.Vengefulness * 0.1f));

            e.Sadness.Value = Mathf.Clamp01((1f - t.Stability) * 0.08f);
            e.Sadness.Baseline = Mathf.Clamp01((1f - t.Stability) * 0.05f);

            e.Trust.Value = Mathf.Clamp01(0.25f + (t.Sociability * 0.25f) + (t.Trust * 0.2f));
            e.Trust.Baseline = Mathf.Clamp01(0.2f + (t.Trust * 0.35f));

            e.Love.Value = Mathf.Clamp01(0.05f + (t.Empathy * 0.2f));
            e.Love.Baseline = Mathf.Clamp01(t.Empathy * 0.1f);

            e.Pride.Value = Mathf.Clamp01(0.1f + (t.Ambition * 0.2f) + (t.Honor * 0.1f));
            e.Pride.Baseline = Mathf.Clamp01(0.05f + (t.Ambition * 0.2f));

            e.Anticipation.Value = Mathf.Clamp01(0.15f + (t.Curiosity * 0.2f) + (t.Ambition * 0.1f));
            e.Anticipation.Baseline = Mathf.Clamp01(0.1f + (t.Curiosity * 0.15f) + (t.Ambition * 0.1f));

            e.Regulation = Mathf.Clamp01((t.Stability * 0.45f) + (t.Rationality * 0.35f) + (t.Bravery * 0.2f));
            e.Volatility = Mathf.Clamp01(0.1f + ((1f - t.Stability) * 0.3f) + ((1f - t.Rationality) * 0.15f));
            e.Stress = Mathf.Clamp01((1f - t.Stability) * 0.12f);
            e.Trauma = 0f;
            e.Burnout = Mathf.Clamp01((1f - t.Stability) * 0.05f);
            e.SocialBattery = Mathf.Clamp01(0.45f + ((1f - t.Sociability) * 0.35f));
            e.EmotionalDebt = 0f;
            e.Valence = Mathf.Clamp01(0.5f + ((e.Joy.Value - e.Sadness.Value - e.Fear.Value) * 0.35f));
            e.Arousal = Mathf.Clamp01((e.Fear.Value * 0.4f) + (e.Anger.Value * 0.3f) + (e.Anticipation.Value * 0.2f));
            e.Mood = e.Valence;
            e.Sanity = Mathf.Clamp01(0.8f + (t.Stability * 0.2f));
            e.ClampAll();
        }
        
        private static void InitializeNeeds(Actor actor, UnitMind mind)
        {
            NeedsHierarchy n = mind.Needs;
            
            // Hunger from nutrition
            if (actor.data.nutrition < 50)
                n.Hunger.Value = actor.data.nutrition / 100f;
            
            // Health from current HP
            float hpRatio = (float)actor.data.health / actor.getMaxHealth();
            n.Health.Value = hpRatio;
            
            // Shelter from having home
            if (actor.city != null)
            {
                n.Shelter.Value = 0.8f;
                n.Belonging.Value = 0.6f;
            }
            else
            {
                n.Shelter.Value = 0.2f;
                n.Belonging.Value = 0.3f;
            }
            
            // Companionship from family
            if (actor.lover != null)
                n.Companionship.Value += 0.3f;

            n.ClampAll();
        }
        
        // Normal distribution random (Box-Muller)
        private static float RandomNormal(float mean, float stdDev)
        {
            float u1 = UnityEngine.Random.value;
            float u2 = UnityEngine.Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
            return Mathf.Clamp01(mean + stdDev * randStdNormal);
        }
    }
}
