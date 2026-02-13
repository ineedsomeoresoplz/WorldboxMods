// Decompiled with JetBrains decompiler
// Type: TileEffectAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class TileEffectAsset : Asset
{
  [DefaultValue(1)]
  public int rate = 1;
  [DefaultValue(1f)]
  public float chance = 1f;
  public string path_sprite;
  [DefaultValue(0.1f)]
  public float time_between_frames = 0.1f;
  private Sprite[] _cached_sprites;
  public HashSet<string> tile_types;

  public void addTileType(string pType)
  {
    if (this.tile_types == null)
      this.tile_types = new HashSet<string>();
    this.tile_types.Add(pType);
  }

  public void addTileTypes(params string[] pTypes)
  {
    if (this.tile_types == null)
      this.tile_types = new HashSet<string>((IEnumerable<string>) pTypes);
    else
      this.tile_types.UnionWith((IEnumerable<string>) pTypes);
  }

  public Sprite[] getSprites()
  {
    if (this._cached_sprites == null)
      this._cached_sprites = SpriteTextureLoader.getSpriteList(this.path_sprite);
    return this._cached_sprites;
  }
}
