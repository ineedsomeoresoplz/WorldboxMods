// Decompiled with JetBrains decompiler
// Type: WallHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WallHelper
{
  private static Dictionary<int, WallFrameContainer> _dictionary = new Dictionary<int, WallFrameContainer>();

  public static Sprite getSprite(WorldTile pTile, TopTileType pTileAsset)
  {
    WallFrameContainer wallFrameContainer;
    if (!WallHelper._dictionary.TryGetValue(pTileAsset.index_id, out wallFrameContainer))
    {
      wallFrameContainer = new WallFrameContainer();
      wallFrameContainer.sprites = SpriteTextureLoader.getSpriteList($"walls/{pTileAsset.id}/wall_sheet");
      WallHelper._dictionary.Add(pTileAsset.index_id, wallFrameContainer);
    }
    int index = !pTile.Type.animated_wall ? pTile.random_animation_seed % wallFrameContainer.sprites.Length : (int) ((double) AnimationHelper.getAnimationGlobalTime(4f) + (double) pTile.random_animation_seed) % wallFrameContainer.sprites.Length;
    return wallFrameContainer.sprites[index];
  }
}
