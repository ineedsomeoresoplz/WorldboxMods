using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ReflectionUtility;

namespace AIBox
{
    public class UnitChatter : MonoBehaviour
    {
        public Actor TargetActor;
        public string TextContent;
        
        private RectTransform rect;
        private Image bg;
        private Text text;
        private CanvasGroup group;
        private float lifetime = 8.0f; 
        private float maxLifetime = 8.0f;
        private Vector3 offset = new Vector3(0, 2.0f, 0);
        
        // Zoom settings
        private float maxZoomForVisibility = 50f; 
        private float fadeStartZoom = 200f; // Start fading at this zoom level
        
        private string unitId; // Store unit ID for unregistration
        private Vector2Int lastTilePos; // Track tile position for overlap detection

        public static UnitChatter Create(Actor actor, string text)
        {
            // 1. Create GameObject
            GameObject go = new GameObject($"Chatter_{actor.data.id}");
            
            // 2. Parent to Main Canvas (simulated world space)
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if(canvas == null) return null;
            
            go.transform.SetParent(canvas.transform, false);
            
            // 3. Add Component
            UnitChatter chatter = go.AddComponent<UnitChatter>();
            chatter.TargetActor = actor;
            chatter.TextContent = text;
            chatter.unitId = actor.data.id.ToString();
            
            // 4. Calculate lifetime based on text length
            float baseTime = 4f;
            float perCharTime = 0.05f;
            chatter.lifetime = Mathf.Clamp(baseTime + (text.Length * perCharTime), 4f, 12f);
            chatter.maxLifetime = chatter.lifetime;
            
            // 5. Get tile position for overlap tracking
            // 5. Get tile position for overlap tracking
            chatter.lastTilePos = new Vector2Int(
                Mathf.FloorToInt(actor.current_position.x),
                Mathf.FloorToInt(actor.current_position.y)
            );
            
            // 6. Register with manager (with position)
            if (ChatterManager.Instance != null)
            {
                ChatterManager.Instance.RegisterChatter(actor.data.id.ToString(), chatter.lastTilePos);
            }
            
            chatter.SetupUI();

            return chatter;
        }
        
        void OnDestroy()
        {
            // Unregister when destroyed
            // Unregister when destroyed
            if (ChatterManager.Instance != null && !string.IsNullOrEmpty(unitId))
            {
                ChatterManager.Instance.UnregisterChatter(unitId);
            }
        }

        private void SetupUI()
        {
            rect = gameObject.AddComponent<RectTransform>();
            group = gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            group.interactable = false;
            
            // Content Size Fitter approach: 
            VerticalLayoutGroup layout = gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(6, 6, 3, 3); // Smaller padding
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            
            ContentSizeFitter fitter = gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            // BACKGROUND
            bg = gameObject.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.6f); // Slightly more transparent

            // TEXT OBJECT
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(transform, false);
            Text t = textObj.AddComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.fontSize = 6;
            t.color = Color.white;
            t.alignment = TextAnchor.MiddleCenter;
            t.text = TextContent;
            
            // Allow wrapping if too long
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Overflow;
        }

        void LateUpdate()
        {
            if (TargetActor == null || TargetActor.data == null || !TargetActor.isAlive())
            {
                Destroy(gameObject);
                return;
            }

            // Check zoom level - hide when zoomed out
            float currentZoom = Camera.main != null ? Camera.main.orthographicSize : 10f;
            
            if (currentZoom > maxZoomForVisibility)
            {
                // Zoomed out too far - hide completely
                group.alpha = 0f;
                return; // Don't update position or lifetime when hidden
            }
            
            // Calculate zoom-based alpha (fade as we zoom out)
            float zoomAlpha = 1f;
            if (currentZoom > fadeStartZoom)
            {
                // Fade from 1 to 0 between fadeStartZoom and maxZoomForVisibility
                zoomAlpha = 1f - ((currentZoom - fadeStartZoom) / (maxZoomForVisibility - fadeStartZoom));
                zoomAlpha = Mathf.Clamp01(zoomAlpha);
            }

            // Tracking Logic
            Vector3 worldPos = (Vector3)TargetActor.current_position + offset;
            
            // Track current tile position for overlap detection
            Vector2Int currentTilePos = new Vector2Int(
                Mathf.FloorToInt(TargetActor.current_position.x),
                Mathf.FloorToInt(TargetActor.current_position.y)
            );
            
            // Update position if unit moved to different tile
            if (currentTilePos != lastTilePos)
            {
                lastTilePos = currentTilePos;
                if (ChatterManager.Instance != null)
                {
                    ChatterManager.Instance.UpdateChatterPosition(unitId, currentTilePos);
                }
            }
            
            // Calculate overlap offset to prevent overlapping chatters
            float overlapOffset = 0f;
            if (ChatterManager.Instance != null)
            {
                overlapOffset = ChatterManager.Instance.GetOverlapOffset(currentTilePos, unitId);
            }
            
            // Apply overlap offset to world position
            worldPos += new Vector3(0, overlapOffset, 0);
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            
            // Check if on screen
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
            bool onScreen = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
            
            if (!onScreen)
            {
                group.alpha = 0f;
                return; // Don't consume lifetime when off screen
            }
            
            transform.position = screenPos;

            // Only consume lifetime when game is not paused
            if (!Config.paused)
            {
                lifetime -= Time.deltaTime;
            }
            
            // Fade out logic (combine with zoom alpha)
            float timeAlpha = 1f;
            if (lifetime < 1.5f)
            {
                timeAlpha = lifetime / 1.5f; // Fade out over last 1.5 seconds
            }
            
            // Final alpha is the minimum of zoom and time fading
            group.alpha = Mathf.Min(zoomAlpha, timeAlpha);
            
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

