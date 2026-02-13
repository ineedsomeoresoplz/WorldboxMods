using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBox
{
    /// <summary>
    /// Comprehensive snapshot of what a unit perceives in their environment.
    /// Used by UnitReactiveSystem to drive personality-based reactions.
    /// </summary>
    public struct SensoryMemory
    {
        // === THREAT PERCEPTION ===
        public int EnemyCount;
        public float ClosestEnemyDist;
        public float ThreatLevel;           // 0=safe, 1=certain death
        public bool WasHurtRecently;        // Health dropped this frame
        
        // === ENVIRONMENTAL HAZARDS ===
        public int FireTileCount;
        public int ProjectileCount;
        public Vector2 DangerVector;        // Direction TO ESCAPE (normalized)
        public bool OnDangerousTile;        // Currently standing on fire/lava
        public bool OnCorruptedBiome;
        public bool OnGoodBiome;
        
        // === SOCIAL PERCEPTION ===
        public int AllyCount;
        public bool SeeFriendlyGathering;   // 3+ allies clustered
        public bool SeeFight;               // Combat happening nearby
        public bool SeeLover;               // Lover within vision
        public bool SeeFamily;              // Family member nearby
        public bool SeeClanMember;          // Clan member nearby
        
        // === STRUCTURES ===
        public bool SeeBuilding;            // Any building nearby
        public bool SeeOwnCityBuilding;     // Building from own city
        public bool NearMapEdge;
        
        // === COMPUTED STATES ===
        public bool InDanger => FireTileCount > 0 || ProjectileCount > 0 || WasHurtRecently || ThreatLevel > 0.5f;
        public bool IsSafe => !InDanger && EnemyCount == 0;
        public bool HasSocialOpportunity => AllyCount > 0 || SeeLover || SeeFamily;
        
        // === DETAILED VISION ===
        public List<string> SeenFriendlyDetails; // e.g. "My Wife", "2 My Kids", "Same Kingdom: 2 kids"
        public List<string> SeenNeutrals;        // e.g. "Sheep", "Cat"
        public List<string> SeenEnemyDetails;    // e.g. "Bear 0.15", "2 Enemies Soldiers 0.70%"
        public List<string> SeenBuildings;       // e.g. "3 Houses", "1 camp_fire"
        public List<string> SeenEvents;          // e.g. "Fire"
        
        // Reactive Triggers
        public bool SawCorpseFamily;
        public bool SawCorpseKingdom;
        public bool SawFamilyInDanger; // New: Sees wife/kid hurt or attacked

        // Logical/Memory Triggers (Computed)
        public bool MissingChild;
        public bool MissingPartner;
        public bool HomeDestroyed;

        public void Reset()
        {
            EnemyCount = 0;
            AllyCount = 0;
            ClosestEnemyDist = 999f;
            ThreatLevel = 0f;
            WasHurtRecently = false;
            
            FireTileCount = 0;
            ProjectileCount = 0;
            DangerVector = Vector2.zero;
            OnDangerousTile = false;
            
            SeeFriendlyGathering = false;
            SeeFight = false;
            SeeLover = false;
            SeeFamily = false;
            SeeClanMember = false;
            
            SeeBuilding = false;
            SeeOwnCityBuilding = false;
            NearMapEdge = false;
            
            SawCorpseFamily = false;
            SawCorpseKingdom = false;
            SawFamilyInDanger = false;

            MissingChild = false;
            MissingPartner = false;
            HomeDestroyed = false;

            SeenFriendlyDetails = new List<string>();
            SeenNeutrals = new List<string>();
            SeenEnemyDetails = new List<string>();
            SeenBuildings = new List<string>();
            SeenEvents = new List<string>();
        }
    }

    /// <summary>
    /// UnitSensorySystem - Scans the environment and builds a SensoryMemory snapshot.
    /// Designed for performance with batched scanning and frame-based updates.
    /// </summary>
    public static class UnitSensorySystem
    {
        // Vision radius - Reduced for more localized/personal awareness
        public const int VISION_RADIUS = 18;
        public const int TILE_SCAN_STEP = 2; // Finer scan for smaller radius
        
        // Reflection cache for projectiles
        private static System.Reflection.FieldInfo _projPosField;
        private static bool _reflectionTried = false;
        
        // Health tracking for damage detection
        private static Dictionary<string, int> _lastHealthValues = new Dictionary<string, int>();
        
        /// <summary>
        /// Main scan function - builds complete SensoryMemory for an actor.
        /// </summary>
        public static SensoryMemory Scan(Actor actor) // Removed 'ref' to return new struct populated
        {
            SensoryMemory memory = new SensoryMemory();
            memory.Reset();
            
            if (actor == null || !actor.isAlive()) return memory;
            
            WorldTile center = actor.current_tile;
            if (center == null) return memory;

            // Get Personality for Subjective Checks
            UnitMind mind = UnitIntelligenceManager.Instance.GetPersonality(actor);
            if (mind == null) mind = new UnitMind(); // Fallback
            
            // === 1. HEALTH DELTA (Detects instant damage like lightning) ===
            ScanHealthDelta(actor, ref memory);
            
            // === 2. IMMEDIATE TILE DANGER ===
            ScanCurrentTile(center, ref memory);
            
            // === 3. NEARBY UNITS (Enemies, Allies, Social) ===
            ScanNearbyUnits(actor, center, ref memory, mind);
            
            // === 4. ENVIRONMENTAL HAZARDS (Fire, Lava, Projectiles, Biome) ===
            ScanEnvironment(actor, center, ref memory);
            
            // === 5. BUILDINGS ===
            ScanBuildings(actor, center, ref memory);
            
            // === 6. MAP EDGE ===
            ScanMapEdge(center, ref memory);
            
            // === 7. LOGICAL CHECKS (Death/Grief) ===
            CheckRelationships(actor, mind, ref memory);
            CheckHome(actor, mind, ref memory);

            return memory;
        }

        private static void CheckHome(Actor actor, UnitMind mind, ref SensoryMemory memory)
        {
            if (actor.city != null)
            {
                // Basic check: Does the actor have a home assigned?
                // Note: The API for specific home assignment might be 'actor.home_building' or similar.
                // If not available, we can check if the city itself is destroyed (no buildings left).

                // Assuming 'actor.data.home_building_id' exists or similar logic.
                // Since I don't see it in the file dump, I will use a general "City Destruction" check
                // or check if they are homeless in a city that HAS buildings.

                /*
                bool hasHome = actor.data.home_buildingID != null;
                if (hasHome) {
                    Building b = World.world.buildings.get(actor.data.home_buildingID);
                    if (b == null || !b.isAlive()) {
                         memory.HomeDestroyed = true;
                         mind.Memory.AddEvent("HomeDestroyed", "Home", "My House", actor.current_position, 0.8f);
                    }
                }
                */

                // Fallback check removed due to API mismatch
                // if (actor.city.storage.get("house") == 0 && actor.city.status.houses > 0) {}
            }
        }

        private static void CheckRelationships(Actor actor, UnitMind mind, ref SensoryMemory memory)
        {
            // Fear check (Emotion object vs float)
            if (mind.Emotions.Fear.Value > 0.4f)
            {
                 if (actor.data.profession != UnitProfession.Warrior)
                 {
                     // 'peaceful' variable is not defined in this scope.
                     // This line will cause a compilation error if uncommented without further context.
                     // peaceful = false;
                 }
            }
            // 1. Check Lover (Death & Missing)
            if (actor.lover != null)
            {
                 if (!actor.lover.isAlive())
                 {
                     // Partner died!
                     memory.SawCorpseFamily = true;
                     mind.Memory.AddEvent("Death", actor.lover.data.id.ToString(), "Partner", actor.lover.current_position, 1.0f);

                     if (!memory.SeenEvents.Contains("Grief (Partner)")) memory.SeenEvents.Add("Grief (Partner)");
                 }
                 else
                 {
                     // Alive. Check missing status.
                     // If we SAW them this frame (SeeLover=true), update timestamp
                     if(memory.SeeLover)
                     {
                         mind.Memory.RegisterSighting(actor.lover.data.id.ToString());
                     }
                     else
                     {
                         // Not seen recently?
                         float timeSince = mind.Memory.GetTimeSinceSeen(actor.lover.data.id.ToString());
                         if(timeSince > 60f) // 60 seconds threshold
                         {
                             memory.MissingPartner = true;
                             if (!memory.SeenEvents.Contains("Missing Partner")) memory.SeenEvents.Add("Missing Partner");
                         }
                     }
                 }
            }

            // 2. Check Children (Death & Missing)
            // Need to iterate children ID list manually since Actor doesn't always keep object refs handy?
            // Actually 'actor.data.children' is a list of strings usually.
            // But we can check 'actor.clan' if implemented, or just skip if no direct child list.
            // Using internal data logic:
            /* 
            // Children tracking disabled - 'children' field missing in ActorData
            if(actor.data.children != null)
            {
                foreach(long childId in actor.data.children)
                {
                    Actor child = World.world.units.get(childId);
                    if(child != null)
                    {
                        if(!child.isAlive())
                        {
                             // Child Died
                             memory.SawCorpseFamily = true;
                             mind.Memory.AddEvent("Death", childId.ToString(), "Child", child.current_position, 1.0f);
                             if (!memory.SeenEvents.Contains("Grief (Child)")) memory.SeenEvents.Add("Grief (Child)");
                        }
                        else
                        {
                            float timeSince = mind.Memory.GetTimeSinceSeen(childId.ToString());
                            if(timeSince > 60f && child.getAge() < 18)
                            {
                                memory.MissingChild = true;
                                if (!memory.SeenEvents.Contains("Missing Child")) memory.SeenEvents.Add("Missing Child");
                            }
                        }
                    }
                }
            }
            */
        }
        
        /// <summary>
        /// Detect if actor was hurt by comparing health to last frame.
        /// </summary>
        private static void ScanHealthDelta(Actor actor, ref SensoryMemory memory)
        {
            int currentHealth = actor.data.health;
            string id = actor.data.id.ToString();
            
            if (_lastHealthValues.TryGetValue(id, out int lastHealth))
            {
                if (currentHealth < lastHealth)
                {
                    memory.WasHurtRecently = true;
                }
            }
            
            _lastHealthValues[id] = currentHealth;
        }
        
        /// <summary>
        /// Check if currently standing on dangerous tile.
        /// </summary>
        private static void ScanCurrentTile(WorldTile tile, ref SensoryMemory memory)
        {
            if (tile == null) return;
            
            string tileId = tile.Type.id;
            if (tileId.Contains("lava") || tileId.Contains("fire") || tileId.Contains("infernal"))
            {
                memory.OnDangerousTile = true;
                memory.FireTileCount++;
            }
        }
        
        /// <summary>
        /// Scan all nearby units using Finder.getUnitsFromChunk.
        /// Categorizes: enemies, allies, lovers, family, clan members, fighters.
        /// </summary>
        private static void ScanNearbyUnits(Actor actor, WorldTile center, ref SensoryMemory memory, UnitMind mind)
        {
            float myPower = actor.stats["attack"] + actor.stats["armor"] + actor.stats["health"];
            if (myPower <= 0) myPower = 1f;

            PersonalityTraits personality = mind.Traits;

            float highestThreat = 0f;
            int allyCluster = 0;
            
            Vector2 combinedThreatPos = Vector2.zero;
            int threatCount = 0;

            // Counters for Vision Details
            int sameKingdomAdults = 0;
            int sameKingdomKids = 0;
            int sameKingdomElders = 0;
            int myKids = 0;
            bool sawWife = false;
            
            
            List<string> neutralDescriptors = new List<string>(); // For cats, sheep, etc.
            List<string> enemyDescriptors = new List<string>(); // Fixed: Added missing declaration

            // Use game's Finder API to get nearby units
            // Use game's Finder API to get nearby units
            foreach (Actor other in Finder.getUnitsFromChunk(center, VISION_RADIUS, 0f, false))
            {
                if (other == null || other == actor) continue; // Removed !other.isAlive() to scan corpses
                
                float dist = Toolbox.Dist(actor.current_position.x, actor.current_position.y, 
                                          other.current_position.x, other.current_position.y);
                
                if (dist > VISION_RADIUS) continue;

                if (dist > VISION_RADIUS) continue;
                
                // NOTE: We do NOT scan corpses visually anymore per request.
                // Death is detected logically via Relationship checks elsewhere or in separate method.
                if (!other.isAlive()) continue;

                // === ENEMY CHECK ===
                bool isEnemy = false;
                if (actor.kingdom != null && other.kingdom != null)
                {
                    isEnemy = actor.kingdom.isEnemy(other.kingdom);
                }
                
                if (isEnemy)
                {
                    memory.EnemyCount++;
                    if (dist < memory.ClosestEnemyDist)
                        memory.ClosestEnemyDist = dist;
                    
                    // Subjective Threat Calculation
                    float enemyPower = other.stats["attack"] + other.stats["armor"] + other.stats["health"];
                    
                    // Base Threat Ratio
                    float powerRatio = enemyPower / myPower;
                    
                    // Personality Modifiers
                    float subjectiveFactor = 1.0f;

                    // Fear checks emotions, Aggression checks traits
                    if (mind.Emotions.Fear.Value > 0.7f) subjectiveFactor += 0.3f; // Panic makes you overestimate
                    if (personality.Bravery > 0.7f) subjectiveFactor -= 0.2f; // Brave units underestimate
                    // if (personality.Tactical > 0.6f) subjectiveFactor = 1.0f; // Removed Tactical trait for now

                    // Normalized Threat Score (0.01 - 1.0)
                    float threatScore = Mathf.Clamp(powerRatio * subjectiveFactor * 0.5f, 0.01f, 1.0f); 
                    
                    // Store detailed string
                    enemyDescriptors.Add($"{other.asset.id} {threatScore:0.00}");

                    if (threatScore > highestThreat) highestThreat = threatScore;
                    
                    // Add to threat vector
                    combinedThreatPos += new Vector2(other.current_position.x, other.current_position.y);
                    threatCount++;
                }
                else
                {
                    // === ALLY CHECK ===
                    bool isAlly = false;
                    if (actor.kingdom != null && other.kingdom != null)
                    {
                        isAlly = actor.kingdom == other.kingdom;
                    }
                    
                    if (isAlly)
                    {
                        memory.AllyCount++;
                        allyCluster++;
                        
                        // Age classification
                        int age = other.getAge(); // Use getAge() method
                        
                        if (age < 18) {
                             sameKingdomKids++;
                        } else if (age > 60) {
                             sameKingdomElders++;
                        } else {
                             sameKingdomAdults++;
                        }
                    }
                    
                    // === SOCIAL CHECKS ===
                    bool isFamily = false;
                    // Lover
                    if (actor.lover != null && actor.lover == other)
                    {
                        memory.SeeLover = true;
                        sawWife = true;
                        isFamily = true;

                        // Memory Update
                        mind.Memory.RegisterSighting(other.data.id.ToString());
                    }
                    
                    /*
                    // Family (Kids) check disabled
                    bool isChild = false;
                    // if(actor.data.children != null && actor.data.children.Contains(other.data.id)) ...
                    */

                    // CHECK SUFFERING (Hurt + Under Attack/Fighting)
                    if (isFamily)
                    {
                        bool isHurt = other.data.health < other.getMaxHealth();
                        // We use attack_target (who they are fighting) as a proxy for "In Combat" 
                        // since we don't have last_attacker access easily.
                        bool isInCombat = other.attack_target != null && other.attack_target.isAlive();
                        
                        // If hurt and fighting (or just very hurt?), flag danger
                        if (isHurt && isInCombat)
                        {
                            memory.SawFamilyInDanger = true;
                            // Add event only once to avoid spam
                            string evt = $"Family Suffering ({other.getName()})";
                            if (!memory.SeenEvents.Contains(evt)) memory.SeenEvents.Add(evt);
                        }
                    }

                    // Clan
                    if (actor.clan != null && other.clan != null && actor.clan == other.clan)
                    {
                        memory.SeeClanMember = true;
                    }
                    
                    if (!isAlly)
                    {
                        // === NEUTRALS (Not Enemy, Not Ally) ===
                        neutralDescriptors.Add(other.asset.id);
                    }
                }
                
                // === FIGHT DETECTION ===
                if (other.attack_target != null && other.attack_target.isAlive())
                {
                    memory.SeeFight = true;
                }
            }
            
            // === COMPILE FRIENDLY STRINGS ===
            if (sameKingdomAdults > 0) memory.SeenFriendlyDetails.Add($"Nearby Kingdom Adults: {sameKingdomAdults}");
            if (sameKingdomKids > 0) memory.SeenFriendlyDetails.Add($"Nearby Kingdom Kids: {sameKingdomKids}");
            if (sameKingdomElders > 0) memory.SeenFriendlyDetails.Add($"Nearby Kingdom Elders: {sameKingdomElders}");
            if (sawWife) memory.SeenFriendlyDetails.Add("My Wife/Husband");
            if (myKids > 0) memory.SeenFriendlyDetails.Add($"{myKids} my kids");

            // === COMPILE NEUTRALS === 
            if (neutralDescriptors.Count > 0)
            {
                // Group duplicates if possible, or just list
                 memory.SeenNeutrals.AddRange(neutralDescriptors);
            }

            // === COMPILE ENEMY STRINGS ===
            // Aggregate similar enemies? "2 Enemies Soldiers 0.70%"
            // For now, list raw or group simple
            if (enemyDescriptors.Count > 0)
            {
               memory.SeenEnemyDetails.AddRange(enemyDescriptors);
            }

            // Friendly gathering = 3+ allies nearby
            if (allyCluster >= 3)
            {
                memory.SeeFriendlyGathering = true;
            }
            
            memory.ThreatLevel = highestThreat;
            
            // Calculate escape vector
            if (threatCount > 0)
            {
                Vector2 avgThreatPos = combinedThreatPos / threatCount;
                memory.DangerVector = (new Vector2(actor.current_position.x, actor.current_position.y) - avgThreatPos).normalized;
            }
        }
        
        /// <summary>
        /// Scan environment: fire tiles, projectiles.
        /// </summary>
        private static void ScanEnvironment(Actor actor, WorldTile center, ref SensoryMemory memory)
        {
            int cx = center.x;
            int cy = center.y;
            
            // Tile scanning bounds
            int startX = Mathf.Max(0, cx - VISION_RADIUS);
            int startY = Mathf.Max(0, cy - VISION_RADIUS);
            int endX = Mathf.Min(MapBox.width - 1, cx + VISION_RADIUS);
            int endY = Mathf.Min(MapBox.height - 1, cy + VISION_RADIUS);
            
            Vector2 combinedHazardPos = Vector2.zero;
            int hazardCount = 0;
            
            // Scan tiles with stepping for performance
            Dictionary<string, int> buildingCounts = new Dictionary<string, int>();

            for (int x = startX; x < endX; x += TILE_SCAN_STEP)
            {
                for (int y = startY; y < endY; y += TILE_SCAN_STEP)
                {
                    WorldTile t = MapBox.instance.GetTile(x, y);
                    if (t == null) continue;
                    
                    // Fire/Lava Check
                    string tileId = t.Type.id;
                    if (tileId.Contains("lava") || tileId.Contains("fire"))
                    {
                        memory.FireTileCount++;
                        combinedHazardPos += new Vector2(x, y);
                        hazardCount++;
                        if (!memory.SeenEvents.Contains("Fire")) memory.SeenEvents.Add("Fire");
                    }

                    // Biome Check (Center tile only for simplicity, or we check average)
                    if (x == cx && y == cy)
                    {
                        if (tileId.Contains("corrupt") || tileId.Contains("infernal")) memory.OnCorruptedBiome = true;
                        if (tileId.Contains("enchanted") || tileId.Contains("grass")) memory.OnGoodBiome = true;
                    }
                }
            }
            
            // Scan projectiles
            if (World.world.projectiles != null)
            {
                System.Collections.IList list = World.world.projectiles.list as System.Collections.IList;
                if (list != null)
                {
                    foreach (object p in list)
                    {
                        if (p == null) continue;
                        
                        // Cache reflection once
                        if (!_reflectionTried)
                        {
                            System.Type t = p.GetType();
                            _projPosField = t.GetField("current_position");
                            if (_projPosField == null) _projPosField = t.GetField("currentPosition");
                            _reflectionTried = true;
                        }
                        
                        if (_projPosField != null)
                        {
                            try
                            {
                                Vector3 pos = (Vector3)_projPosField.GetValue(p);
                                float dist = Toolbox.Dist(actor.current_position.x, actor.current_position.y, pos.x, pos.y);
                                if (dist <= VISION_RADIUS)
                                {
                                    memory.ProjectileCount++;
                                    combinedHazardPos += new Vector2(pos.x, pos.y);
                                    hazardCount++;
                                    
                                    // Try get name
                                    Asset asset = (Asset)p.GetType().GetField("asset").GetValue(p);
                                    if(asset != null) memory.SeenEvents.Add(asset.id);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            
            // Combine hazard vectors with threat vectors
            if (hazardCount > 0 && memory.DangerVector == Vector2.zero)
            {
                Vector2 avgHazardPos = combinedHazardPos / hazardCount;
                memory.DangerVector = (new Vector2(actor.current_position.x, actor.current_position.y) - avgHazardPos).normalized;
            }
            else if (hazardCount > 0)
            {
                // Blend hazard direction with existing threat direction
                Vector2 avgHazardPos = combinedHazardPos / hazardCount;
                Vector2 hazardDir = (new Vector2(actor.current_position.x, actor.current_position.y) - avgHazardPos).normalized;
                memory.DangerVector = (memory.DangerVector + hazardDir).normalized;
            }
        }
        
        /// <summary>
        /// Scan for buildings in vision.
        /// </summary>
        private static void ScanBuildings(Actor actor, WorldTile center, ref SensoryMemory memory)
        {
            int cx = center.x;
            int cy = center.y;
            
            // Smaller radius for building scan (performance)
            int buildingRadius = VISION_RADIUS / 2;
            int startX = Mathf.Max(0, cx - buildingRadius);
            int startY = Mathf.Max(0, cy - buildingRadius);
            int endX = Mathf.Min(MapBox.width - 1, cx + buildingRadius);
            int endY = Mathf.Min(MapBox.height - 1, cy + buildingRadius);
            
            Dictionary<string, int> bCounts = new Dictionary<string, int>();

            for (int x = startX; x < endX; x += 4) // Larger step for buildings
            {
                for (int y = startY; y < endY; y += 4)
                {
                    WorldTile t = MapBox.instance.GetTile(x, y);
                    if (t == null) continue;
                    
                    // Check if tile has a building
                    if (t.building != null)
                    {
                        memory.SeeBuilding = true;
                        
                        if (bCounts.ContainsKey(t.building.asset.id))
                            bCounts[t.building.asset.id]++;
                        else
                            bCounts[t.building.asset.id] = 1;

                        // Check if building belongs to actor's city
                        try
                        {
                            if (actor.city != null && t.building.city == actor.city)
                            {
                                memory.SeeOwnCityBuilding = true;
                            }
                        }
                        catch { }
                    }
                }
            }

            foreach(var kvp in bCounts)
            {
                memory.SeenBuildings.Add($"{kvp.Value} {kvp.Key}");
            }
        }
        
        /// <summary>
        /// Check if near map edge.
        /// </summary>
        private static void ScanMapEdge(WorldTile center, ref SensoryMemory memory)
        {
            int edgeMargin = 5;
            if (center.x < edgeMargin || center.x > MapBox.width - edgeMargin ||
                center.y < edgeMargin || center.y > MapBox.height - edgeMargin)
            {
                memory.NearMapEdge = true;
            }
        }
        
        /// <summary>
        /// Cleanup health cache for dead/despawned actors.
        /// Call periodically to prevent memory leak.
        /// </summary>
        public static void CleanupHealthCache()
        {
            if (_lastHealthValues.Count > 1000)
            {
                _lastHealthValues.Clear();
            }
        }
    }
}
