using System;
using System.Collections.Generic;
using UnityEngine;

namespace XNTM.Code.Utils
{
    
    
    
    internal static class TopicSpriteScaler
    {
        private const string NationPrefix = "xntm_";
        private static readonly Dictionary<Sprite, Sprite> Cache = new Dictionary<Sprite, Sprite>();
        private static float? _vanillaTopicWorldSize;

        public static Sprite ScaleIfNationType(Sprite sprite)
        {
            if (sprite == null)
                return null;
            if (!IsNationTypeSprite(sprite))
                return sprite;

            if (Cache.TryGetValue(sprite, out var cached))
                return cached;

            float targetSize = GetVanillaTopicWorldSize();
            float currentSize = GetWorldSize(sprite);
            if (currentSize <= targetSize * 1.02f) 
            {
                Cache[sprite] = sprite;
                return sprite;
            }

            float pixelsPerUnit = sprite.rect.width / targetSize;
            Vector2 pivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            Sprite scaled = Sprite.Create(sprite.texture, sprite.rect, pivot, pixelsPerUnit, 0, SpriteMeshType.Tight, sprite.border);
            scaled.name = $"{sprite.name}_xntm_scaled";
            Cache[sprite] = scaled;
            return scaled;
        }

        private static bool IsNationTypeSprite(Sprite sprite)
        {
            
            return sprite.name.StartsWith(NationPrefix, StringComparison.OrdinalIgnoreCase);
        }

        private static float GetWorldSize(Sprite sprite)
        {
            if (sprite == null || sprite.pixelsPerUnit <= 0f)
                return 0.32f; 
            return sprite.rect.width / sprite.pixelsPerUnit;
        }

        private static float GetVanillaTopicWorldSize()
        {
            if (_vanillaTopicWorldSize.HasValue)
                return _vanillaTopicWorldSize.Value;

            
            Sprite reference = SpriteTextureLoader.getSprite("ui/Icons/iconKingdom");
            _vanillaTopicWorldSize = reference != null && reference.pixelsPerUnit > 0f
                ? reference.rect.width / reference.pixelsPerUnit
                : 0.32f; 
            return _vanillaTopicWorldSize.Value;
        }
    }
}
