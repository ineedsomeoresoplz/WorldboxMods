using System;
using System.Collections.Generic;
using UnityEngine;
using NCMS;
using AIBox;

namespace AIBox.UI
{
    public class BrainInspectorUI : MonoBehaviour
    {
        public static BrainInspectorUI Instance;
        public bool Enabled = false;
        
        private Actor hoveredActor;
        private UnitMind hoveredMind;
        private SensoryMemory hoveredSenses;
        
        // UI Styles
        private GUIStyle windowStyle;
        private Texture2D bgTexture;
        private GUIStyle headerStyle;

        public static void Init()
        {
             if (Instance == null)
             {
                 GameObject go = new GameObject("BrainInspectorUI");
                 Instance = go.AddComponent<BrainInspectorUI>();
                 DontDestroyOnLoad(go);
             }
        }

        void Start()
        {
            // Initialize Terminal Style Background
            bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color(0.1f, 0.05f, 0.15f, 0.95f));
            bgTexture.Apply();

            windowStyle = new GUIStyle();
            windowStyle.normal.background = bgTexture;
            windowStyle.normal.textColor = Color.white;

            headerStyle = new GUIStyle();
            headerStyle.fontSize = 13;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = new Color(0.7f, 0.8f, 1.0f);
        }

        private Actor selectedActor; // Locked selection
        private Rect windowRect = new Rect(100, 100, 950, 550); // Wider for more info
        private Vector2 scrollPos;
        private Vector2 actionScrollPos;

        void Update()
        {
            if (!Enabled) return;
            
            // Toggle Lock on 'L'
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (selectedActor != null) selectedActor = null;
                else if (hoveredActor != null) selectedActor = hoveredActor;
            }

            // Swallow clicks logic...
            if (Input.GetMouseButtonDown(0) && selectedActor != null)
            {
                 Vector2 mousePos = Input.mousePosition;
                 Vector2 guiMouse = new Vector2(mousePos.x, Screen.height - mousePos.y);
                 if (windowRect.Contains(guiMouse)) return;
            }
            
            // Update Selected Actor Data
            if (selectedActor != null)
            {
                if(!selectedActor.isAlive()) {
                    selectedActor = null; 
                } else {
                    hoveredMind = UnitIntelligenceManager.Instance.GetPersonality(selectedActor);
                    hoveredSenses = UnitSensorySystem.Scan(selectedActor);
                    return;
                }
            }
            
            // Hover Fallback
            if (MapBox.instance != null)
            {
               Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               WorldTile tile = MapBox.instance.GetTile((int)worldPos.x, (int)worldPos.y);
               
               if (tile != null)
               {
                   hoveredActor = ActionLibrary.getActorFromTile(tile);
                   if (hoveredActor != null)
                   {
                       hoveredMind = UnitIntelligenceManager.Instance.GetPersonality(hoveredActor);
                       hoveredSenses = UnitSensorySystem.Scan(hoveredActor);
                   }
                   else
                   {
                       hoveredMind = null;
                   }
               }
            }
        }

        void OnGUI()
        {
            if (!Enabled) return;
            
            if (selectedActor != null)
            {
                windowRect.width = 950;
                windowRect.height = 550;
                windowRect = GUI.Window(9909, windowRect, DrawLockedWindow, "", windowStyle);
            }
            else if (hoveredActor != null && hoveredMind != null)
            {
                // HOVER MODE
                float width = 950;
                float height = 550;
                Vector2 mouse = Event.current.mousePosition;
                float x = mouse.x + 20;
                float y = mouse.y + 20;
                if (x + width > Screen.width) x = mouse.x - width - 20;
                if (y + height > Screen.height) y = mouse.y - height - 20;

                Rect hoverRect = new Rect(x, y, width, height);
                GUI.Box(hoverRect, "", windowStyle);
                GUILayout.BeginArea(new Rect(x + 10, y + 10, width - 20, height - 20));
                DrawContent(hoveredActor, false);
                GUILayout.EndArea();
            }
        }

        void DrawLockedWindow(int id)
        {
            DrawContent(selectedActor, true);
            GUI.DragWindow();
        }

        void DrawContent(Actor target, bool isLocked)
        {
            string status = isLocked ? "(LOCKED - 'L' to Unlock)" : "(HOVER)";
            GUILayout.BeginHorizontal();
            GUILayout.Label($"<b><size=14><color=white>{target.getName()}</color></size></b> <color=#aaaaaa>{status}</color>");
            GUILayout.FlexibleSpace();
            GUILayout.Label($"<color=#888888>{target.asset.id} | Age: {target.getAge()}</color>");
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            // =========================================================================================
            // COL 1: PERSONALITY TRAITS
            // =========================================================================================
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.Label("PERSONALITY", headerStyle);
            GUILayout.Space(5);
            
            if(hoveredMind != null && hoveredMind.Traits != null)
            {
                PersonalityTraits t = hoveredMind.Traits;
                DrawBar("Bravery", t.Bravery, v => t.Bravery = v, new Color(0.9f, 0.5f, 0.2f));
                DrawBar("Empathy", t.Empathy, v => t.Empathy = v, new Color(0.9f, 0.4f, 0.6f));
                DrawBar("Stability", t.Stability, v => t.Stability = v, new Color(0.4f, 0.7f, 0.9f));
                DrawBar("Social", t.Sociability, v => t.Sociability = v, new Color(0.5f, 0.9f, 0.5f));
                DrawBar("Vengeful", t.Vengefulness, v => t.Vengefulness = v, new Color(0.6f, 0.2f, 0.2f));
                DrawBar("Ambition", t.Ambition, v => t.Ambition = v, new Color(0.8f, 0.7f, 0.2f));
                DrawBar("Honor", t.Honor, v => t.Honor = v, new Color(0.7f, 0.7f, 0.9f));
                DrawBar("Morality", t.Morality, v => t.Morality = v, new Color(0.9f, 0.9f, 0.5f));
                DrawBar("Aggress", t.Aggression, v => t.Aggression = v, new Color(0.9f, 0.3f, 0.3f));
                DrawBar("Loyalty", t.Loyalty, v => t.Loyalty = v, new Color(0.5f, 0.5f, 0.9f));
            }
            GUILayout.EndVertical();

            GUILayout.Space(15);

            // =========================================================================================
            // COL 2: EMOTIONS (12-spectrum)
            // =========================================================================================
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.Label("EMOTIONS", headerStyle);
            GUILayout.Space(5);
            
            if(hoveredMind != null && hoveredMind.Emotions != null)
            {
                EmotionalSpectrum e = hoveredMind.Emotions;

                // Primary
                DrawEmotionBar("Joy", e.Joy.Value, new Color(1f, 0.9f, 0.3f));
                DrawEmotionBar("Sadness", e.Sadness.Value, new Color(0.3f, 0.4f, 0.7f));
                DrawEmotionBar("Anger", e.Anger.Value, new Color(0.9f, 0.2f, 0.2f));
                DrawEmotionBar("Fear", e.Fear.Value, new Color(0.5f, 0.3f, 0.6f));
                DrawEmotionBar("Disgust", e.Disgust.Value, new Color(0.4f, 0.6f, 0.3f));
                
                GUILayout.Space(5);
                // Social
                DrawEmotionBar("Trust", e.Trust.Value, new Color(0.3f, 0.8f, 0.6f));
                DrawEmotionBar("Love", e.Love.Value, new Color(0.9f, 0.4f, 0.5f));
                
                GUILayout.Space(5);
                // Self
                DrawEmotionBar("Pride", e.Pride.Value, new Color(0.9f, 0.7f, 0.3f));
                DrawEmotionBar("Guilt", e.Guilt.Value, new Color(0.5f, 0.4f, 0.3f));
                DrawEmotionBar("Sanity", e.Sanity, new Color(0.8f, 0.8f, 0.9f));
                DrawEmotionBar("Mood", e.Mood, new Color(0.6f, 0.85f, 0.9f));
                DrawEmotionBar("Stress", e.Stress, new Color(0.9f, 0.45f, 0.25f));
                DrawEmotionBar("Arousal", e.Arousal, new Color(0.85f, 0.6f, 0.2f));
                DrawEmotionBar("Trauma", e.Trauma, new Color(0.7f, 0.25f, 0.25f));
                DrawEmotionBar("Burnout", e.Burnout, new Color(0.75f, 0.5f, 0.3f));
                DrawEmotionBar("Control", e.Regulation, new Color(0.35f, 0.75f, 0.9f));
                DrawEmotionBar("Battery", e.SocialBattery, new Color(0.45f, 0.85f, 0.55f));
                
                GUILayout.Space(5);
                GUILayout.Label($"<color=#AAFFAA>Dominant: {e.GetDominantEmotion()}</color>");
            }
            GUILayout.EndVertical();

            GUILayout.Space(15);

            // =========================================================================================
            // COL 3: DECISION & ACTIONS
            // =========================================================================================
            GUILayout.BeginVertical(GUILayout.Width(220));
            GUILayout.Label("DECISIONS", headerStyle);
            GUILayout.Space(5);

            string gameTask = "None";
            try { gameTask = target.getTaskText(); } catch {} 

            GUILayout.Label($"<b>Game:</b> <color=#FFFF00>{gameTask}</color>");
            
            if (hoveredMind != null)
            {
               GUILayout.Label($"<b>AI:</b> <color=#00FFFF>{hoveredMind.LastDecision ?? "..."}</color>");
               GUILayout.Label($"<color=#888888>{hoveredMind.LastDecisionReason ?? ""}</color>");
               
               GUILayout.Space(10);
               GUILayout.Label("NEEDS", headerStyle);
               if (hoveredMind.Needs != null)
               {
                   NeedsHierarchy n = hoveredMind.Needs;
                   GUILayout.Label($"Hunger: {n.Hunger.Value:F2} | Safety: {n.Safety.Value:F2}");
                   GUILayout.Label($"Social: {n.Companionship.Value:F2} | Purpose: {n.Purpose.Value:F2}");
                   
                   string urgent = n.GetMostUrgentNeed();
                   if (urgent != "None")
                   {
                       GUILayout.Label($"<color=orange>Urgent: {urgent}</color>");
                   }
               }
               
               GUILayout.Space(10);
               GUILayout.Label("ACTION SCORES", headerStyle);
               actionScrollPos = GUILayout.BeginScrollView(actionScrollPos, GUILayout.Height(100));
               if (hoveredMind.ActionScores != null)
               {
                   // Sort by score
                   var sorted = new List<KeyValuePair<string, float>>(hoveredMind.ActionScores);
                   sorted.Sort((a, b) => b.Value.CompareTo(a.Value));
                   
                   foreach (var kvp in sorted)
                   {
                       if (kvp.Value > 0.05f)
                       {
                           string color = kvp.Value > 0.5f ? "#00FF00" : (kvp.Value > 0.2f ? "#FFFF00" : "#888888");
                           GUILayout.Label($"<color={color}>{kvp.Key}: {kvp.Value:F2}</color>");
                       }
                   }
               }
               GUILayout.EndScrollView();
            }

            // Status Alerts
            GUILayout.Space(5);
            if(hoveredSenses.MissingChild) GUILayout.Label("<color=red>[!] MISSING CHILD</color>");
            if(hoveredSenses.MissingPartner) GUILayout.Label("<color=red>[!] MISSING PARTNER</color>");
            if(hoveredSenses.HomeDestroyed) GUILayout.Label("<color=orange>[!] HOMELESS</color>");
            if(hoveredSenses.InDanger) GUILayout.Label("<color=yellow>[!] IN DANGER</color>");
            if(hoveredMind != null && hoveredMind.Emotions.Sanity < 0.2f) 
                GUILayout.Label("<color=magenta>[!] INSANE</color>");
            
            GUILayout.EndVertical();

            GUILayout.Space(15);

            // =========================================================================================
            // COL 4: MEMORY & SENSES
            // =========================================================================================
            GUILayout.BeginVertical(GUILayout.Width(250));
            GUILayout.Label("MEMORY & SENSES", headerStyle);

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));

            // Kingdom Status
            if(hoveredMind != null && hoveredMind.Memory != null)
            {
                UnitMemory mem = hoveredMind.Memory;
                if (mem.KingdomAtWar) GUILayout.Label("<color=red>[WAR]</color>");
                if (mem.KingdomUnderInvasion) GUILayout.Label("<color=red>[INVASION!]</color>");
                
                GUILayout.Label("<color=#FFD700>--- Memories ---</color>");
                for (int i = mem.ImportantEvents.Count - 1; i >= Math.Max(0, mem.ImportantEvents.Count - 10); i--)
                {
                    var evt = mem.ImportantEvents[i];
                    float timeAgo = Time.time - evt.Timestamp;
                    GUILayout.Label($"<size=10>{evt.Type}: {evt.TargetName} ({timeAgo:F0}s)</size>");
                }
            }
            
            GUILayout.Space(10);

            // Current Vision
            GUILayout.Label("<color=#FFD700>--- Vision ---</color>");
            if (hoveredSenses.SeenFriendlyDetails != null)
            {
                DrawVisionSection("Events", hoveredSenses.SeenEvents, "#FFA500");
                DrawVisionSection("Family", hoveredSenses.SeenFriendlyDetails, "#88FF88");
                DrawVisionSection("Enemies", hoveredSenses.SeenEnemyDetails, "#FF8888");
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void DrawVisionSection(string title, List<string> items, string colorHex)
        {
            if (items == null || items.Count == 0) return;
            foreach (string item in items)
            {
                GUILayout.Label($"<color={colorHex}>- {item}</color>");
            }
        }

        private void DrawBar(string label, float value, Action<float> setter, Color barColor)
        {
             GUILayout.BeginHorizontal();
             GUILayout.Label($"<color=white>{label}</color>", GUILayout.Width(60));
             
             Rect r = GUILayoutUtility.GetRect(80, 16);
             
             // BG
             Color old = GUI.color;
             GUI.color = new Color(0, 0, 0, 0.5f); 
             GUI.DrawTexture(r, Texture2D.whiteTexture);
             
             // Fill
             GUI.color = barColor;
             float fillWidth = (r.width - 2) * Mathf.Clamp01(value);
             GUI.DrawTexture(new Rect(r.x + 1, r.y + 1, fillWidth, r.height - 2), Texture2D.whiteTexture);
             GUI.color = old;
             
             // Value Text Overlay
             GUI.Label(new Rect(r.x + 5, r.y, r.width, r.height), $"<size=9><b>{value:F2}</b></size>");

             // Buttons
             if (GUILayout.Button("+", GUILayout.Width(18))) setter(Mathf.Clamp01(value + 0.1f));
             if (GUILayout.Button("-", GUILayout.Width(18))) setter(Mathf.Clamp01(value - 0.1f));

             GUILayout.EndHorizontal();
        }
        
        private void DrawEmotionBar(string label, float value, Color barColor)
        {
             GUILayout.BeginHorizontal();
             GUILayout.Label($"<color=white>{label}</color>", GUILayout.Width(60));
             
             Rect r = GUILayoutUtility.GetRect(100, 14);
             
             // BG
             Color old = GUI.color;
             GUI.color = new Color(0, 0, 0, 0.5f); 
             GUI.DrawTexture(r, Texture2D.whiteTexture);
             
             // Fill
             GUI.color = barColor;
             float fillWidth = (r.width - 2) * Mathf.Clamp01(value);
             GUI.DrawTexture(new Rect(r.x + 1, r.y + 1, fillWidth, r.height - 2), Texture2D.whiteTexture);
             GUI.color = old;
             
             // Value Text
             GUI.Label(new Rect(r.x + 5, r.y, r.width, r.height), $"<size=9>{value:F2}</size>");

             GUILayout.EndHorizontal();
        }
    }
}
