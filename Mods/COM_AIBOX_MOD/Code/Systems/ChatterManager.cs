using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ReflectionUtility;

namespace AIBox
{
    public class ChatterManager : MonoBehaviour
    {
        public static ChatterManager Instance;
        
        private float timer = 0f;
        public float ChatterInterval = 0.8f; 
        private System.Random rnd = new System.Random();        
        public float MaxZoomForChatter = 50f; 
        
        // Track which units currently have active chatter
        private HashSet<string> unitsWithActiveChatter = new HashSet<string>();
        
        // Track chatter positions to detect overlaps
        private Dictionary<string, Vector2Int> chatterPositions = new Dictionary<string, Vector2Int>();
        
        // Toggle for enabling/disabling chatter
        public bool ChatterEnabled = true;
        
        // Offset amount for overlapping chatters
        private const float OVERLAP_OFFSET = 2f;

        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("ChatterManager");
                Instance = go.AddComponent<ChatterManager>();
                DontDestroyOnLoad(go);
            }
        }
        
        // Register/unregister units with active chatter
        public void RegisterChatter(string unitId, Vector2Int tilePos)
        {
            unitsWithActiveChatter.Add(unitId);
            chatterPositions[unitId] = tilePos;
        }
        
        public void UnregisterChatter(string unitId)
        {
            unitsWithActiveChatter.Remove(unitId);
            chatterPositions.Remove(unitId);
        }
        
        public bool HasActiveChatter(string unitId)
        {
            return unitsWithActiveChatter.Contains(unitId);
        }
        
        // Gets the vertical offset for a chatter at the given tile position.
        public float GetOverlapOffset(Vector2Int tilePos, string currentUnitId)
        {
            int overlappingCount = 0;
            
            foreach(var kvp in chatterPositions)
            {
                // Skip self
                if (kvp.Key == currentUnitId) continue;
                
                // Check if same tile
                if (kvp.Value == tilePos)
                {
                    overlappingCount++;
                }
            }
            
            return overlappingCount * OVERLAP_OFFSET;
        }
        
        // Updates the position of a chatter (for moving units)
        public void UpdateChatterPosition(string unitId, Vector2Int newPos)
        {
            if (chatterPositions.ContainsKey(unitId))
            {
                chatterPositions[unitId] = newPos;
            }
        }

        void Update()
        {
            if (World.world == null || World.world.units == null) return;
            
            // Don't generate chatter if disabled
            if (!ChatterEnabled) return;
            
            // Don't generate chatter when game is paused
            if (Config.paused) return;
            
            // Don't generate chatter when zoomed out too far
            if (Camera.main != null && Camera.main.orthographicSize > MaxZoomForChatter) return;
            
            timer += Time.deltaTime;
            if (timer >= ChatterInterval)
            {
                timer = 0f;
                SpawnRandomChatter();
            }
        }

        private void SpawnRandomChatter()
        {
            if (World.world.kingdoms.list.Count == 0) return;
            
            // Find a kingdom with units
            var civKingdoms = World.world.kingdoms.list.Where(k => k.isCiv() && k.units.Count > 0).ToList();
            if (civKingdoms.Count == 0) return;
            
            Kingdom k = civKingdoms[rnd.Next(civKingdoms.Count)];
            if (k.units.Count == 0) return;
            
            Actor unit = k.units[rnd.Next(k.units.Count)];
            
            // Skip if unit already has active chatter
            string unitId = unit.data.id.ToString();
            if (HasActiveChatter(unitId)) return;
            
            // Only show if visible on screen
            Vector3 uPos = (Vector3)unit.current_position;
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(uPos);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            
            if (onScreen)
            {
                // Context-aware phrase generation
                var ctx = ChatterContext.Extract(unit);
                var phrase = PhraseGenerator.Generate(ctx);
                UnitChatter.Create(unit, phrase);
            }
        }
        
        // Public method to get current zoom threshold
        public static bool IsZoomedInEnough()
        {
            if (Instance == null || Camera.main == null) return false;
            return Camera.main.orthographicSize <= Instance.MaxZoomForChatter;
        }
    }
}
