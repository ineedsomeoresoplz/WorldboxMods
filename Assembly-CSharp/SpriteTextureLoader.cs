// Decompiled with JetBrains decompiler
// Type: SpriteTextureLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class SpriteTextureLoader
{
  private static readonly Dictionary<string, Sprite> _cached_sprites = new Dictionary<string, Sprite>();
  private static readonly Dictionary<string, Sprite[]> _cached_sprite_list = new Dictionary<string, Sprite[]>();
  private static int _total_sprite_list_single_sprites = 0;

  public static Sprite getSprite(string pPath)
  {
    Sprite sprite;
    if (!SpriteTextureLoader._cached_sprites.TryGetValue(pPath, out sprite))
    {
      sprite = (Sprite) Resources.Load(pPath, typeof (Sprite));
      SpriteTextureLoader._cached_sprites[pPath] = sprite;
    }
    return sprite;
  }

  public static Sprite[] getSpriteList(string pPath, bool pSkipIfEmpty = false)
  {
    Sprite[] spriteList;
    if (!SpriteTextureLoader._cached_sprite_list.TryGetValue(pPath, out spriteList))
    {
      spriteList = Resources.LoadAll<Sprite>(pPath);
      if (pSkipIfEmpty && spriteList.Length == 0)
        return (Sprite[]) null;
      SpriteTextureLoader._cached_sprite_list.Add(pPath, spriteList);
      SpriteTextureLoader._total_sprite_list_single_sprites += spriteList.Length;
    }
    return spriteList;
  }

  public static void addSprite(string pPathID, byte[] pBytes)
  {
    Texture2D texture2D = new Texture2D(1, 1);
    ((Texture) texture2D).filterMode = (FilterMode) 0;
    if (!ImageConversion.LoadImage(texture2D, pBytes))
      return;
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.5f, 0.5f);
    Sprite sprite = Sprite.Create(texture2D, rect, vector2, 1f);
    SpriteTextureLoader._cached_sprites.Add(pPathID, sprite);
  }

  public static int total_sprites => SpriteTextureLoader._cached_sprites.Count;

  public static int total_sprites_list => SpriteTextureLoader._cached_sprite_list.Count;

  public static int total_sprites_list_single_sprites
  {
    get => SpriteTextureLoader._total_sprite_list_single_sprites;
  }
}
