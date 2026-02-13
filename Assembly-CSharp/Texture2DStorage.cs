// Decompiled with JetBrains decompiler
// Type: Texture2DStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class Texture2DStorage
{
  private static Dictionary<string, SpritePool> pools = new Dictionary<string, SpritePool>();
  private static Dictionary<string, Texture2D> prefabs = new Dictionary<string, Texture2D>();

  internal static Sprite getSprite(int pW, int pH)
  {
    string key = $"{pW.ToString()}_{pH.ToString()}";
    if (Texture2DStorage.pools.ContainsKey(key))
    {
      SpritePool pool = Texture2DStorage.pools[key];
      if (pool.list.Count > 0)
      {
        Sprite sprite = pool.list[pool.list.Count - 1];
        pool.list.RemoveAt(pool.list.Count - 1);
        return sprite;
      }
    }
    if (!Texture2DStorage.prefabs.ContainsKey(key))
    {
      Texture2D texture2D1 = new Texture2D(pW, pH, (TextureFormat) 4, false);
      ((Texture) texture2D1).filterMode = (FilterMode) 0;
      Texture2D texture2D2 = texture2D1;
      ((Object) texture2D2).name = "Texture2DStorage_" + key;
      Texture2DStorage.prefabs.Add(key, texture2D2);
    }
    return Sprite.Create(Object.Instantiate<Texture2D>(Texture2DStorage.prefabs[key]), new Rect(0.0f, 0.0f, (float) pW, (float) pH), new Vector2(0.0f, 0.0f), 1f);
  }

  internal static void addToStorage(Sprite pSprite, int pW, int pH)
  {
    string key = $"{pW.ToString()}_{pH.ToString()}";
    SpritePool spritePool;
    if (Texture2DStorage.pools.ContainsKey(key))
    {
      spritePool = Texture2DStorage.pools[key];
    }
    else
    {
      spritePool = new SpritePool();
      Texture2DStorage.pools.Add(key, spritePool);
    }
    spritePool.list.Add(pSprite);
  }
}
