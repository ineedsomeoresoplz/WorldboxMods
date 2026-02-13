using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBox
{
    public class UnitIntelligenceManager : MonoBehaviour
    {
        public static UnitIntelligenceManager Instance;

        // The "Brain Store" - Maps Actor ID to their hidden Personality
        public Dictionary<string, UnitMind> UnitPersonalities = new Dictionary<string, UnitMind>();
        // Cache Actor References to avoid slow/error-prone lookups
        public Dictionary<string, Actor> UnitRefs = new Dictionary<string, Actor>();

        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("UnitIntelligenceManager");
                Instance = go.AddComponent<UnitIntelligenceManager>();
                DontDestroyOnLoad(go);
            }
        }

        private List<string> _activeUnitIds = new List<string>();
        private int _updateIndex = 0;
        private const int UPDATES_PER_FRAME = 50; 

        public void RegisterUnit(Actor actor)
        {
            if (actor == null || actor.data == null) return;
            string id = actor.data.id.ToString();

            if (!UnitPersonalities.ContainsKey(id))
            {
                UnitMind p = PersonalityGenerator.Generate(actor);
                UnitPersonalities[id] = p;
                UnitRefs[id] = actor; // Cache Ref
                _activeUnitIds.Add(id);
            }
            else
            {
                UnitRefs[id] = actor;
                if (!_activeUnitIds.Contains(id)) _activeUnitIds.Add(id);
            }
        }

        public void UnregisterUnit(Actor actor)
        {
             if (actor == null || actor.data == null) return;
             string id = actor.data.id.ToString();
             
             UnitPersonalities.Remove(id);
             UnitRefs.Remove(id);
             _activeUnitIds.Remove(id);
        }

        public UnitMind GetPersonality(Actor actor)
        {
            if (actor == null || actor.data == null) return null;
            string id = actor.data.id.ToString(); 
            
            if (UnitPersonalities.TryGetValue(id, out UnitMind mind))
            {
                return mind;
            }
            return null;
        }

        // Main Brain Loop
        void Update() 
        {
            if (MapBox.instance == null || _activeUnitIds.Count == 0) return;

            int total = _activeUnitIds.Count;
            int updates = Math.Min(UPDATES_PER_FRAME, total);
            float updateScale = Mathf.Max(1f, total / (float)Mathf.Max(1, UPDATES_PER_FRAME));
            float stepDelta = Mathf.Max(0.016f, Time.deltaTime * updateScale);

            for (int processed = 0; processed < updates; processed++)
            {
                if (_activeUnitIds.Count == 0) break;
                if (_updateIndex >= _activeUnitIds.Count) _updateIndex = 0;
                string id = _activeUnitIds[_updateIndex];
                _updateIndex++;

                if (UnitPersonalities.TryGetValue(id, out UnitMind mind))
                {
                    if (UnitRefs.TryGetValue(id, out Actor actor))
                    {
                        if (actor != null && actor.isAlive())
                        {
                            SensoryMemory senses = UnitSensorySystem.Scan(actor);
                            UnitReactiveSystem.UpdateReaction(actor, mind, senses, stepDelta);
                            UnitDecisionSystem.UpdateDecision(actor, mind, senses);
                        }
                    }
                }
            }

            if(Time.frameCount % 600 == 0) {
                 CleanUp();
            }
        }


        private void CleanUp() {
            for (int i = _activeUnitIds.Count - 1; i >= 0; i--)
            {
                string id = _activeUnitIds[i];
                bool remove = false;

                if (!UnitPersonalities.ContainsKey(id))
                {
                    remove = true;
                }
                else if (!UnitRefs.TryGetValue(id, out Actor actor) || actor == null || !actor.isAlive())
                {
                    remove = true;
                }

                if (remove)
                {
                    UnitPersonalities.Remove(id);
                    UnitRefs.Remove(id);
                    _activeUnitIds.RemoveAt(i);
                }
            }

            if (_updateIndex >= _activeUnitIds.Count) _updateIndex = 0;
            UnitSensorySystem.CleanupHealthCache();
        }
    }
}
