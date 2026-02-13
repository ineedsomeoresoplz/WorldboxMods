using System.IO;
using UnityEngine;

namespace AIBox
{
    public static class Sprites
    {
        public static Sprite LoadSprite(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[AIBox] Sprite not found at: {path}");
                return null;
            }

            byte[] data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(data);
            texture.filterMode = FilterMode.Point;
            
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
