using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBox
{
    // =====================================================================================
    // EMOTION SYSTEM - 12 emotions with 0.01/0.1 granularity
    // =====================================================================================
    
    [Serializable]
    public class Emotion
    {
        public float Value;           // Current value 0.0 to 1.0
        public float Baseline;        // What it decays toward
        public float DecayRate;       // How fast it returns to baseline per tick
        
        // Intensity constants - use these for consistent emotional changes
        public const float MICRO = 0.005f;    // Barely noticeable (passive effects)
        public const float SMALL = 0.01f;     // Subtle change (routine events)
        public const float MEDIUM = 0.03f;    // Noticeable (significant events)
        public const float LARGE = 0.05f;     // Strong reaction (important events)
        public const float MAJOR = 0.1f;      // Major impact (trauma, death)
        public const float SPIKE = 0.2f;      // Instant spike (acute trauma)
        
        public Emotion(float baseline = 0f, float decayRate = 0.001f)
        {
            Value = baseline;
            Baseline = baseline;
            DecayRate = decayRate;
        }
        
        public void Increase(float amount)
        {
            Value = Mathf.Clamp01(Value + amount);
        }
        
        public void Decrease(float amount)
        {
            Value = Mathf.Clamp01(Value - amount);
        }

        public void AddScaled(float amount, float scale)
        {
            float scaled = amount * scale;
            if (scaled >= 0f) Increase(scaled);
            else Decrease(-scaled);
        }

        public void MoveBaseline(float target, float speed, float deltaTime)
        {
            Baseline = Mathf.MoveTowards(Baseline, Mathf.Clamp01(target), Mathf.Max(0f, speed) * Mathf.Max(0f, deltaTime));
        }
        
        // Time-based decay - call with Time.deltaTime
        public void Decay(float deltaTime)
        {
            Value = Mathf.MoveTowards(Value, Baseline, DecayRate * deltaTime);
        }
        
        // Legacy: decays a fixed amount (use sparingly)
        public void DecayFixed()
        {
            Value = Mathf.MoveTowards(Value, Baseline, DecayRate);
        }
        
        // Quick checks
        public bool IsHigh => Value > 0.7f;
        public bool IsMedium => Value > 0.4f && Value <= 0.7f;
        public bool IsLow => Value <= 0.4f;
        public bool IsCritical => Value > 0.9f;
    }
    
    [Serializable]
    public class EmotionalSpectrum
    {
        // Primary Emotions
        public Emotion Joy;           // Happiness, contentment, pleasure
        public Emotion Sadness;       // Grief, sorrow, depression
        public Emotion Anger;         // Rage, frustration, irritation
        public Emotion Fear;          // Terror, anxiety, dread
        public Emotion Disgust;       // Revulsion, contempt
        public Emotion Surprise;      // Shock, astonishment
        
        // Social Emotions
        public Emotion Trust;         // Confidence in others
        public Emotion Love;          // Attachment to family/lover
        
        // Self-Conscious Emotions
        public Emotion Pride;         // Self-satisfaction, achievement
        public Emotion Shame;         // Embarrassment, failure
        public Emotion Guilt;         // Remorse for actions
        
        // Cognitive Emotions
        public Emotion Anticipation;  // Expectation, hope/dread
        
        // Legacy compatibility
        public float Sanity;          // Mental stability (1.0 = sane)
        public float Mood;
        public float Stress;
        public float Arousal;
        public float Valence;
        public float Burnout;
        public float Trauma;
        public float Regulation;
        public float Volatility;
        public float SocialBattery;
        public float EmotionalDebt;
        
        public EmotionalSpectrum()
        {
            // Initialize with baselines and decay rates
            // NOTE: Decay is called per-update, not per-frame. Rates are per-second equivalent.
            Joy = new Emotion(0.5f, 0.0002f);        // Baseline content, very slow decay
            Sadness = new Emotion(0f, 0.0001f);      // Baseline 0, extremely slow decay
            Anger = new Emotion(0f, 0.0005f);        // Baseline 0, slow decay
            Fear = new Emotion(0f, 0.001f);          // Baseline 0, moderate decay
            Disgust = new Emotion(0f, 0.0003f);      // Baseline 0, slow decay
            Surprise = new Emotion(0f, 0.005f);      // Baseline 0, faster decay (surprise fades)
            
            Trust = new Emotion(0.5f, 0.0001f);      // Baseline neutral, extremely slow
            Love = new Emotion(0f, 0.00005f);        // Baseline 0, almost never decays
            
            Pride = new Emotion(0.3f, 0.0002f);      // Baseline modest
            Shame = new Emotion(0f, 0.0003f);
            Guilt = new Emotion(0f, 0.0002f);
            
            Anticipation = new Emotion(0.3f, 0.0005f);
            
            Sanity = 1.0f;
            Mood = 0.5f;
            Stress = 0f;
            Arousal = 0f;
            Valence = 0.5f;
            Burnout = 0f;
            Trauma = 0f;
            Regulation = 0.5f;
            Volatility = 0.2f;
            SocialBattery = 0.7f;
            EmotionalDebt = 0f;
        }
        
        public void DecayAll(float deltaTime)
        {
            Joy.Decay(deltaTime);
            Sadness.Decay(deltaTime);
            Anger.Decay(deltaTime);
            Fear.Decay(deltaTime);
            Disgust.Decay(deltaTime);
            Surprise.Decay(deltaTime);
            Trust.Decay(deltaTime);
            Love.Decay(deltaTime);
            Pride.Decay(deltaTime);
            Shame.Decay(deltaTime);
            Guilt.Decay(deltaTime);
            Anticipation.Decay(deltaTime);
        }

        public void EnsureInitialized()
        {
            if (Joy == null) Joy = new Emotion(0.5f, 0.0002f);
            if (Sadness == null) Sadness = new Emotion(0f, 0.0001f);
            if (Anger == null) Anger = new Emotion(0f, 0.0005f);
            if (Fear == null) Fear = new Emotion(0f, 0.001f);
            if (Disgust == null) Disgust = new Emotion(0f, 0.0003f);
            if (Surprise == null) Surprise = new Emotion(0f, 0.005f);
            if (Trust == null) Trust = new Emotion(0.5f, 0.0001f);
            if (Love == null) Love = new Emotion(0f, 0.00005f);
            if (Pride == null) Pride = new Emotion(0.3f, 0.0002f);
            if (Shame == null) Shame = new Emotion(0f, 0.0003f);
            if (Guilt == null) Guilt = new Emotion(0f, 0.0002f);
            if (Anticipation == null) Anticipation = new Emotion(0.3f, 0.0005f);
            if (float.IsNaN(Sanity) || float.IsInfinity(Sanity)) Sanity = 1f;
            if (float.IsNaN(Mood) || float.IsInfinity(Mood)) Mood = 0.5f;
            if (float.IsNaN(Stress) || float.IsInfinity(Stress)) Stress = 0f;
            if (float.IsNaN(Arousal) || float.IsInfinity(Arousal)) Arousal = 0f;
            if (float.IsNaN(Valence) || float.IsInfinity(Valence)) Valence = 0.5f;
            if (float.IsNaN(Burnout) || float.IsInfinity(Burnout)) Burnout = 0f;
            if (float.IsNaN(Trauma) || float.IsInfinity(Trauma)) Trauma = 0f;
            if (float.IsNaN(Regulation) || float.IsInfinity(Regulation)) Regulation = 0.5f;
            if (float.IsNaN(Volatility) || float.IsInfinity(Volatility)) Volatility = 0.2f;
            if (float.IsNaN(SocialBattery) || float.IsInfinity(SocialBattery)) SocialBattery = 0.7f;
            if (float.IsNaN(EmotionalDebt) || float.IsInfinity(EmotionalDebt)) EmotionalDebt = 0f;
            ClampAll();
        }

        public void UpdateMetaState(PersonalityTraits traits, NeedsHierarchy needs, UnitMemory memory, bool inDanger, bool peaceful, float socialExposure, float threatLevel, float deltaTime)
        {
            EnsureInitialized();

            float dt = Mathf.Clamp(deltaTime, 0.01f, 2f);
            float stability = traits != null ? traits.Stability : 0.5f;
            float rationality = traits != null ? traits.Rationality : 0.5f;
            float bravery = traits != null ? traits.Bravery : 0.5f;
            float sociability = traits != null ? traits.Sociability : 0.5f;

            float stressTarget = 0f;
            if (needs != null)
            {
                stressTarget += needs.Hunger.Urgency * 0.2f;
                stressTarget += needs.Safety.Urgency * 0.35f;
                stressTarget += needs.Health.Urgency * 0.25f;
                stressTarget += needs.Resources.Urgency * 0.1f;
                stressTarget += needs.Shelter.Urgency * 0.1f;
            }
            stressTarget += Mathf.Clamp01(threatLevel) * 0.45f;
            if (inDanger) stressTarget += 0.25f;
            stressTarget += Trauma * 0.3f;
            stressTarget += Burnout * 0.25f;
            stressTarget -= Joy.Value * 0.2f;
            stressTarget -= Trust.Value * 0.1f;
            stressTarget = Mathf.Clamp01(stressTarget);

            float regulationTarget = Mathf.Clamp01((stability * 0.4f) + (rationality * 0.4f) + (bravery * 0.2f) - (stressTarget * 0.3f));
            Regulation = Mathf.MoveTowards(Regulation, regulationTarget, dt * 0.2f);

            float volatilityTarget = Mathf.Clamp01(0.15f + ((1f - stability) * 0.35f) + (Stress * 0.35f) + (Trauma * 0.3f) - (Regulation * 0.25f));
            Volatility = Mathf.MoveTowards(Volatility, volatilityTarget, dt * 0.25f);

            Stress = Mathf.MoveTowards(Stress, stressTarget, dt * (0.18f + (Volatility * 0.12f)));

            float socialDelta = socialExposure - 0.35f;
            float socialDrain = 0f;
            if (socialDelta > 0f) socialDrain = (1f - sociability) * socialDelta * dt * 0.15f;
            else socialDrain = sociability * (-socialDelta) * dt * 0.15f;
            SocialBattery = Mathf.Clamp01(SocialBattery - socialDrain);
            if (peaceful) SocialBattery = Mathf.Clamp01(SocialBattery + dt * 0.02f);
            if (inDanger) SocialBattery = Mathf.Clamp01(SocialBattery - dt * 0.03f);

            float traumaGain = 0f;
            if (memory != null)
            {
                traumaGain += memory.RecentLoss * 0.3f;
                traumaGain -= memory.SafeExposure * 0.05f;
                traumaGain += memory.DangerExposure * 0.1f;
            }
            if (inDanger && threatLevel > 0.7f) traumaGain += 0.08f;
            traumaGain *= (1f - (stability * 0.5f));
            Trauma = Mathf.Clamp01(Trauma + traumaGain * dt * 0.2f);
            if (peaceful && !inDanger) Trauma = Mathf.MoveTowards(Trauma, 0f, dt * (0.01f + (Regulation * 0.02f)));

            float burnoutTarget = 0f;
            burnoutTarget += Stress * 0.55f;
            burnoutTarget += (1f - SocialBattery) * 0.2f;
            if (needs != null)
            {
                burnoutTarget += needs.Purpose.Urgency * 0.15f;
                burnoutTarget += needs.Recognition.Urgency * 0.1f;
            }
            if (peaceful) burnoutTarget -= 0.1f;
            burnoutTarget = Mathf.Clamp01(burnoutTarget);
            Burnout = Mathf.MoveTowards(Burnout, burnoutTarget, dt * 0.08f);

            Valence = Mathf.Clamp01(Mathf.Lerp(Valence, ComputeValence(), Mathf.Clamp01(dt * 0.25f)));
            Arousal = Mathf.Clamp01(Mathf.Lerp(Arousal, ComputeArousal(), Mathf.Clamp01(dt * 0.35f)));

            float moodTarget = Mathf.Clamp01((Valence * 0.65f) + (Joy.Baseline * 0.2f) - (Stress * 0.25f) - (Burnout * 0.2f) + (Trust.Value * 0.1f));
            Mood = Mathf.MoveTowards(Mood, moodTarget, dt * (0.03f + (stability * 0.03f) + (Regulation * 0.02f)));

            EmotionalDebt = Mathf.Clamp01(EmotionalDebt + Mathf.Max(0f, Stress - Regulation) * dt * 0.07f);
            if (peaceful) EmotionalDebt = Mathf.MoveTowards(EmotionalDebt, 0f, dt * (0.02f + (Regulation * 0.03f)));

            ApplyBaselineDrift(traits, needs, dt);

            float sanityTarget = Mathf.Clamp01(0.95f - (Stress * 0.35f) - (Trauma * 0.35f) - (Burnout * 0.2f) + (Regulation * 0.25f) + (Mood * 0.1f));
            Sanity = Mathf.MoveTowards(Sanity, sanityTarget, dt * (0.08f + (Regulation * 0.05f)));
        }

        public float GetThreatDrive(PersonalityTraits traits)
        {
            float bravery = traits != null ? traits.Bravery : 0.5f;
            float value = (Fear.Value * 0.4f) + (Stress * 0.35f) + ((1f - Sanity) * 0.2f) + (Trauma * 0.2f) - (bravery * 0.2f);
            return Mathf.Clamp01(value);
        }

        public float GetAggressionDrive(PersonalityTraits traits)
        {
            float vengeful = traits != null ? traits.Vengefulness : 0.3f;
            float aggression = traits != null ? traits.Aggression : 0.3f;
            float value = (Anger.Value * 0.45f) + (Arousal * 0.2f) + (vengeful * 0.2f) + (aggression * 0.2f) - (Fear.Value * 0.25f) - (Burnout * 0.1f);
            return Mathf.Clamp01(value);
        }

        public float GetSocialDrive(PersonalityTraits traits, NeedsHierarchy needs)
        {
            float sociability = traits != null ? traits.Sociability : 0.5f;
            float companionshipUrgency = needs != null ? needs.Companionship.Urgency : 0.5f;
            float value = (companionshipUrgency * 0.35f) + (Love.Value * 0.2f) + (sociability * 0.2f) + (Mathf.Max(0f, 0.6f - SocialBattery) * 0.15f) - (Fear.Value * 0.15f);
            return Mathf.Clamp01(value);
        }

        public float GetRecoveryDrive(NeedsHierarchy needs)
        {
            float healthUrgency = needs != null ? needs.Health.Urgency : 0f;
            float value = (Burnout * 0.4f) + (EmotionalDebt * 0.3f) + ((1f - Sanity) * 0.2f) + (Stress * 0.2f) + (healthUrgency * 0.2f);
            return Mathf.Clamp01(value);
        }

        private float ComputeValence()
        {
            float positive = Joy.Value + Love.Value + Trust.Value + Pride.Value + (Anticipation.Value * 0.35f);
            float negative = Sadness.Value + Fear.Value + Anger.Value + Disgust.Value + Shame.Value + Guilt.Value;
            return Mathf.Clamp01(0.5f + ((positive - negative) * 0.18f) - (Stress * 0.12f) - (Burnout * 0.1f));
        }

        private float ComputeArousal()
        {
            float raw = (Fear.Value * 0.3f) + (Anger.Value * 0.25f) + (Surprise.Value * 0.2f) + (Anticipation.Value * 0.15f) + (Stress * 0.2f) - (Sadness.Value * 0.1f);
            return Mathf.Clamp01(raw);
        }

        private void ApplyBaselineDrift(PersonalityTraits traits, NeedsHierarchy needs, float deltaTime)
        {
            float stability = traits != null ? traits.Stability : 0.5f;
            float empathy = traits != null ? traits.Empathy : 0.5f;
            float vengeful = traits != null ? traits.Vengefulness : 0.3f;
            float ambition = traits != null ? traits.Ambition : 0.4f;
            float safetyUrgency = needs != null ? needs.Safety.Urgency : 0f;
            float companionshipUrgency = needs != null ? needs.Companionship.Urgency : 0f;

            float joyBase = Mathf.Clamp01(0.2f + (Mood * 0.45f) - (Burnout * 0.2f) - (safetyUrgency * 0.1f));
            float fearBase = Mathf.Clamp01(0.02f + (Stress * 0.2f) + (Trauma * 0.35f) + (safetyUrgency * 0.15f) - ((traits != null ? traits.Bravery : 0.5f) * 0.12f));
            float angerBase = Mathf.Clamp01(0.02f + (Stress * 0.15f) + (vengeful * 0.22f) + (Trauma * 0.1f) - (stability * 0.08f));
            float sadnessBase = Mathf.Clamp01((Burnout * 0.25f) + (Trauma * 0.15f) + ((1f - Mood) * 0.1f));
            float trustBase = Mathf.Clamp01(0.25f + ((traits != null ? traits.Trust : 0.5f) * 0.4f) + (Mood * 0.15f) - (Stress * 0.22f));
            float loveBase = Mathf.Clamp01((companionshipUrgency * 0.1f) + (empathy * 0.12f) + (Mood * 0.05f));
            float anticipationBase = Mathf.Clamp01(0.15f + (ambition * 0.2f) + (Arousal * 0.2f) - (Burnout * 0.2f));

            Joy.MoveBaseline(joyBase, 0.08f + (stability * 0.08f), deltaTime);
            Fear.MoveBaseline(fearBase, 0.1f + (Volatility * 0.1f), deltaTime);
            Anger.MoveBaseline(angerBase, 0.09f + (Volatility * 0.08f), deltaTime);
            Sadness.MoveBaseline(sadnessBase, 0.06f + ((1f - stability) * 0.06f), deltaTime);
            Trust.MoveBaseline(trustBase, 0.05f + (Regulation * 0.05f), deltaTime);
            Love.MoveBaseline(loveBase, 0.03f + (empathy * 0.03f), deltaTime);
            Anticipation.MoveBaseline(anticipationBase, 0.07f + (Arousal * 0.05f), deltaTime);
        }
        
        public void ClampAll()
        {
            Joy.Value = Mathf.Clamp01(Joy.Value);
            Sadness.Value = Mathf.Clamp01(Sadness.Value);
            Anger.Value = Mathf.Clamp01(Anger.Value);
            Fear.Value = Mathf.Clamp01(Fear.Value);
            Disgust.Value = Mathf.Clamp01(Disgust.Value);
            Surprise.Value = Mathf.Clamp01(Surprise.Value);
            Trust.Value = Mathf.Clamp01(Trust.Value);
            Love.Value = Mathf.Clamp01(Love.Value);
            Pride.Value = Mathf.Clamp01(Pride.Value);
            Shame.Value = Mathf.Clamp01(Shame.Value);
            Guilt.Value = Mathf.Clamp01(Guilt.Value);
            Anticipation.Value = Mathf.Clamp01(Anticipation.Value);
            Sanity = Mathf.Clamp01(Sanity);
            Mood = Mathf.Clamp01(Mood);
            Stress = Mathf.Clamp01(Stress);
            Arousal = Mathf.Clamp01(Arousal);
            Valence = Mathf.Clamp01(Valence);
            Burnout = Mathf.Clamp01(Burnout);
            Trauma = Mathf.Clamp01(Trauma);
            Regulation = Mathf.Clamp01(Regulation);
            Volatility = Mathf.Clamp01(Volatility);
            SocialBattery = Mathf.Clamp01(SocialBattery);
            EmotionalDebt = Mathf.Clamp01(EmotionalDebt);
            Joy.Baseline = Mathf.Clamp01(Joy.Baseline);
            Sadness.Baseline = Mathf.Clamp01(Sadness.Baseline);
            Anger.Baseline = Mathf.Clamp01(Anger.Baseline);
            Fear.Baseline = Mathf.Clamp01(Fear.Baseline);
            Disgust.Baseline = Mathf.Clamp01(Disgust.Baseline);
            Surprise.Baseline = Mathf.Clamp01(Surprise.Baseline);
            Trust.Baseline = Mathf.Clamp01(Trust.Baseline);
            Love.Baseline = Mathf.Clamp01(Love.Baseline);
            Pride.Baseline = Mathf.Clamp01(Pride.Baseline);
            Shame.Baseline = Mathf.Clamp01(Shame.Baseline);
            Guilt.Baseline = Mathf.Clamp01(Guilt.Baseline);
            Anticipation.Baseline = Mathf.Clamp01(Anticipation.Baseline);
        }
        
        public string GetDominantEmotion()
        {
            float max = 0f;
            string dominant = "Neutral";
            
            if (Joy.Value > max) { max = Joy.Value; dominant = "Joy"; }
            if (Sadness.Value > max) { max = Sadness.Value; dominant = "Sadness"; }
            if (Anger.Value > max) { max = Anger.Value; dominant = "Anger"; }
            if (Fear.Value > max) { max = Fear.Value; dominant = "Fear"; }
            if (Disgust.Value > max) { max = Disgust.Value; dominant = "Disgust"; }
            if (Love.Value > max) { max = Love.Value; dominant = "Love"; }
            if (Pride.Value > max) { max = Pride.Value; dominant = "Pride"; }
            if (Shame.Value > max) { max = Shame.Value; dominant = "Shame"; }
            if (Guilt.Value > max) { max = Guilt.Value; dominant = "Guilt"; }
            
            return dominant;
        }
        
        // Legacy compatibility - maps to old EmotionalState
        public float Happiness { get => Joy.Value; set => Joy.Value = value; }
        
        public void Clamp() => ClampAll();
    }

    // =====================================================================================
    // PERSONALITY TRAITS - Mapped from in-game WorldBox traits
    // =====================================================================================
    
    [Serializable]
    public class PersonalityTraits
    {
        // Core Dimensions (0.0 to 1.0)
        public float Bravery;         // Resistance to Fear. Low = Coward, High = Brave
        public float Empathy;         // Care for others. Low = Cold, High = Caring
        public float Stability;       // Emotional resilience. Low = Neurotic, High = Stoic
        public float Sociability;     // Need for interaction. Low = Introvert, High = Extrovert
        
        // Moral Dimensions
        public float Honesty;         // Truthfulness. Low = Deceitful, High = Honest
        public float Morality;        // Good vs Evil. Low = Evil, High = Good
        public float Honor;           // Adherence to codes/rules
        
        // Drive Dimensions
        public float Ambition;        // Desire for power/status
        public float Greed;           // Desire for wealth/possessions
        public float Vengefulness;    // Tendency to seek revenge
        
        // Cognitive Dimensions
        public float Rationality;     // Logic vs Impulse. Low = Impulsive
        public float Curiosity;       // Interest in new things
        
        // Combat Dimensions
        public float Aggression;      // Combat readiness. Low = Peaceful
        
        // Social Dimensions
        public float Loyalty;         // Devotion to kingdom/leader
        public float Trust;           // Default trust in others
        
        public PersonalityTraits()
        {
            // Default "Average Person"
            Bravery = 0.5f;
            Empathy = 0.5f;
            Stability = 0.5f;
            Sociability = 0.5f;
            Honesty = 0.5f;
            Morality = 0.5f;
            Honor = 0.5f;
            Ambition = 0.5f;
            Greed = 0.3f;
            Vengefulness = 0.3f;
            Rationality = 0.5f;
            Curiosity = 0.5f;
            Aggression = 0.3f;
            Loyalty = 0.5f;
            Trust = 0.5f;
        }
        
        public void Clamp()
        {
            Bravery = Mathf.Clamp01(Bravery);
            Empathy = Mathf.Clamp01(Empathy);
            Stability = Mathf.Clamp01(Stability);
            Sociability = Mathf.Clamp01(Sociability);
            Honesty = Mathf.Clamp01(Honesty);
            Morality = Mathf.Clamp01(Morality);
            Honor = Mathf.Clamp01(Honor);
            Ambition = Mathf.Clamp01(Ambition);
            Greed = Mathf.Clamp01(Greed);
            Vengefulness = Mathf.Clamp01(Vengefulness);
            Rationality = Mathf.Clamp01(Rationality);
            Curiosity = Mathf.Clamp01(Curiosity);
            Aggression = Mathf.Clamp01(Aggression);
            Loyalty = Mathf.Clamp01(Loyalty);
            Trust = Mathf.Clamp01(Trust);
        }
        
        public override string ToString()
        {
            return $"Brav:{Bravery:F2} Emp:{Empathy:F2} Stab:{Stability:F2} Amb:{Ambition:F2}";
        }
    }

    // =====================================================================================
    // MEMORY SYSTEM - Events, relationships, trauma
    // =====================================================================================
    
    [Serializable]
    public struct MemoryEvent
    {
        public string Type;           // Event type ID
        public string TargetID;       // Who/What involved
        public string TargetName;     // Display name
        public Vector2 Position;      // Where it happened
        public float Timestamp;       // When it happened
        public float Intensity;       // How impactful (0-1)
        public float EmotionalImpact; // Lasting emotional effect
        
        public override string ToString()
        {
            return $"{Type}: {TargetName} ({Intensity:F2})";
        }
    }
    
    // Memory event types
    public static class MemoryTypes
    {
        // Death & Loss
        public const string DEATH_FAMILY = "death_family";
        public const string DEATH_FRIEND = "death_friend";
        public const string DEATH_ALLY = "death_ally";
        public const string DEATH_ENEMY = "death_enemy";
        
        // Trauma
        public const string WITNESSED_VIOLENCE = "witnessed_violence";
        public const string ATTACKED = "attacked";
        public const string HOME_DESTROYED = "home_destroyed";
        public const string CITY_DESTROYED = "city_destroyed";
        
        // Kingdom Events
        public const string WAR_DECLARED = "war_declared";
        public const string PEACE_MADE = "peace_made";
        public const string KING_DIED = "king_died";
        public const string KINGDOM_FELL = "kingdom_fell";
        public const string INVASION = "invasion";
        
        // Social
        public const string MADE_FRIEND = "made_friend";
        public const string LOST_FRIEND = "lost_friend";
        public const string FELL_IN_LOVE = "fell_in_love";
        public const string HEARTBREAK = "heartbreak";
        public const string CHILD_BORN = "child_born";
        
        // Achievement
        public const string PROMOTION = "promotion";
        public const string VICTORY = "victory";
        public const string DEFEAT = "defeat";
    }

    [Serializable]
    public class UnitMemory
    {
        public List<MemoryEvent> ImportantEvents = new List<MemoryEvent>();
        public Dictionary<string, float> LastSeenTimestamps = new Dictionary<string, float>();
        public Dictionary<string, float> RelationshipScores = new Dictionary<string, float>(); // -1 to 1
        
        // Kingdom-level awareness
        public bool KingdomAtWar;
        public bool KingdomUnderInvasion;
        public string LastKingName;
        public float KingdomMorale; // 0 to 1
        public float DangerExposure;
        public float SafeExposure;
        public float SocialSupport;
        public float RecentLoss;
        public float RecentSuccess;
        public float LastEventTimestamp;
        
        public void AddEvent(string type, string targetId, string targetName, Vector2 pos, float intensity, float emotionalImpact = 0f)
        {
            // Prevent duplicate recent events
            for (int i = ImportantEvents.Count - 1; i >= 0; i--)
            {
                if (ImportantEvents[i].Type == type && ImportantEvents[i].TargetID == targetId)
                {
                    if (Time.time - ImportantEvents[i].Timestamp < 10f) return;
                }
            }
            
            MemoryEvent evt = new MemoryEvent
            {
                Type = type,
                TargetID = targetId,
                TargetName = targetName,
                Position = pos,
                Timestamp = Time.time,
                Intensity = intensity,
                EmotionalImpact = emotionalImpact
            };
            
            ImportantEvents.Add(evt);
            LastEventTimestamp = evt.Timestamp;

            bool isLoss = type == MemoryTypes.DEATH_FAMILY ||
                          type == MemoryTypes.DEATH_FRIEND ||
                          type == MemoryTypes.DEATH_ALLY ||
                          type == MemoryTypes.KING_DIED ||
                          type == MemoryTypes.KINGDOM_FELL ||
                          type == MemoryTypes.HEARTBREAK ||
                          type == MemoryTypes.HOME_DESTROYED ||
                          type == MemoryTypes.CITY_DESTROYED ||
                          type == MemoryTypes.DEFEAT;
            if (isLoss) RecentLoss = Mathf.Clamp01(RecentLoss + Mathf.Clamp01(intensity) * 0.8f);

            bool isSuccess = type == MemoryTypes.VICTORY ||
                             type == MemoryTypes.PROMOTION ||
                             type == MemoryTypes.CHILD_BORN ||
                             type == MemoryTypes.MADE_FRIEND ||
                             type == MemoryTypes.PEACE_MADE;
            if (isSuccess) RecentSuccess = Mathf.Clamp01(RecentSuccess + Mathf.Clamp01(intensity) * 0.7f);
             
            // Limit memory size (keep last 50)
            if (ImportantEvents.Count > 50)
            {
                ImportantEvents.RemoveAt(0);
            }
        }

        public void UpdateExposure(bool inDanger, bool hasSupport, float deltaTime)
        {
            float dt = Mathf.Clamp(deltaTime, 0.01f, 2f);
            if (inDanger)
            {
                DangerExposure = Mathf.Clamp01(DangerExposure + dt * 0.08f);
                SafeExposure = Mathf.MoveTowards(SafeExposure, 0f, dt * 0.04f);
            }
            else
            {
                SafeExposure = Mathf.Clamp01(SafeExposure + dt * 0.06f);
                DangerExposure = Mathf.MoveTowards(DangerExposure, 0f, dt * 0.03f);
            }

            if (hasSupport) SocialSupport = Mathf.Clamp01(SocialSupport + dt * 0.08f);
            else SocialSupport = Mathf.MoveTowards(SocialSupport, 0f, dt * 0.03f);

            RecentLoss = Mathf.MoveTowards(RecentLoss, 0f, dt * 0.02f);
            RecentSuccess = Mathf.MoveTowards(RecentSuccess, 0f, dt * 0.03f);
        }
        
        public void RegisterSighting(string id)
        {
            LastSeenTimestamps[id] = Time.time;
        }
        
        public float GetTimeSinceSeen(string id)
        {
            if (LastSeenTimestamps.TryGetValue(id, out float time))
            {
                return Time.time - time;
            }
            return 99999f;
        }
        
        public void UpdateRelationship(string id, float delta)
        {
            if (!RelationshipScores.ContainsKey(id))
                RelationshipScores[id] = 0f;
            RelationshipScores[id] = Mathf.Clamp(RelationshipScores[id] + delta, -1f, 1f);
        }
        
        public float GetRelationship(string id)
        {
            return RelationshipScores.TryGetValue(id, out float val) ? val : 0f;
        }
    }

    // =====================================================================================
    // NEEDS SYSTEM - Maslow-inspired hierarchy
    // =====================================================================================
    
    [Serializable]
    public class Need
    {
        public float Value;           // 0 = desperate, 1 = fully satisfied
        public float DecayRate;       // How fast need decreases
        public float Urgency => 1f - Value; // Higher = more urgent
        
        public Need(float initial = 0.8f, float decay = 0.001f)
        {
            Value = initial;
            DecayRate = decay;
        }
        
        public void Satisfy(float amount)
        {
            Value = Mathf.Clamp01(Value + amount);
        }

        public void Drain(float amount)
        {
            Value = Mathf.Clamp01(Value - Mathf.Abs(amount));
        }
        
        public void Decay()
        {
            Value = Mathf.Clamp01(Value - DecayRate);
        }

        public void Clamp()
        {
            Value = Mathf.Clamp01(Value);
            DecayRate = Mathf.Max(0f, DecayRate);
        }
        
        public bool IsCritical => Value < 0.2f;
        public bool IsLow => Value < 0.4f;
    }
    
    [Serializable]
    public class NeedsHierarchy
    {
        // Level 1: Survival
        public Need Hunger;           // Food
        public Need Safety;           // Physical security
        public Need Health;           // Not injured/sick
        
        // Level 2: Security
        public Need Shelter;          // Has home
        public Need Resources;        // Has supplies
        
        // Level 3: Social
        public Need Belonging;        // Part of community
        public Need Companionship;    // Has friends/family
        
        // Level 4: Esteem
        public Need Recognition;      // Respected
        public Need Status;           // Social position
        
        // Level 5: Self-Actualization
        public Need Purpose;          // Meaning in life
        
        public NeedsHierarchy()
        {
            Hunger = new Need(0.8f, 0.002f);
            Safety = new Need(0.9f, 0.001f);
            Health = new Need(1.0f, 0.0005f);
            Shelter = new Need(0.5f, 0.0005f);
            Resources = new Need(0.5f, 0.001f);
            Belonging = new Need(0.5f, 0.0005f);
            Companionship = new Need(0.5f, 0.001f);
            Recognition = new Need(0.3f, 0.0003f);
            Status = new Need(0.3f, 0.0002f);
            Purpose = new Need(0.3f, 0.0001f);
        }
        
        public void DecayAll()
        {
            Hunger.Decay();
            Safety.Decay();
            Shelter.Decay();
            Belonging.Decay();
            Companionship.Decay();
            Recognition.Decay();
            Purpose.Decay();
        }

        public void ClampAll()
        {
            Hunger.Clamp();
            Safety.Clamp();
            Health.Clamp();
            Shelter.Clamp();
            Resources.Clamp();
            Belonging.Clamp();
            Companionship.Clamp();
            Recognition.Clamp();
            Status.Clamp();
            Purpose.Clamp();
        }
        
        public string GetMostUrgentNeed()
        {
            // Priority order: Survival > Security > Social > Esteem > Self
            if (Hunger.IsCritical) return "Hunger";
            if (Safety.IsCritical) return "Safety";
            if (Health.IsCritical) return "Health";
            if (Shelter.IsLow) return "Shelter";
            if (Belonging.IsLow) return "Belonging";
            if (Companionship.IsLow) return "Companionship";
            return "None";
        }
    }

    // =====================================================================================
    // UNIT MIND - Complete mind of a unit
    // =====================================================================================
    
    [Serializable]
    public class UnitMind
    {
        public PersonalityTraits Traits;
        public EmotionalSpectrum Emotions;
        public UnitMemory Memory;
        public NeedsHierarchy Needs;
        
        // Decision tracking
        public string LastDecision;
        public string LastDecisionReason;
        public float LastDecisionTime;
        public string LastActionId;
        public float LastDecisionScore;
        public int DecisionStreak;
        
        // Action queue for AI
        public List<string> AvailableActions = new List<string>();
        public Dictionary<string, float> ActionScores = new Dictionary<string, float>();
        
        public UnitMind()
        {
            Traits = new PersonalityTraits();
            Emotions = new EmotionalSpectrum();
            Memory = new UnitMemory();
            Needs = new NeedsHierarchy();
            LastDecision = "Idle";
            LastActionId = "idle";
            LastDecisionScore = 0f;
            DecisionStreak = 0;
        }
        
        // Convenience accessors for legacy compatibility
        public float Happiness { get => Emotions.Joy.Value; set => Emotions.Joy.Value = value; }
        public float Fear { get => Emotions.Fear.Value; set => Emotions.Fear.Value = value; }
        public float Anger { get => Emotions.Anger.Value; set => Emotions.Anger.Value = value; }
        public float Sadness { get => Emotions.Sadness.Value; set => Emotions.Sadness.Value = value; }
        public float Sanity { get => Emotions.Sanity; set => Emotions.Sanity = value; }
    }
}
