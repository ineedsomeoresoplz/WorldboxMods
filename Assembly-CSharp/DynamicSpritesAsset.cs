// Decompiled with JetBrains decompiler
// Type: DynamicSpritesAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class DynamicSpritesAsset : Asset
{
  public bool big_atlas = true;
  public bool check_wobbly_setting;
  public UnitTextureAtlasID atlas_id;
  public string export_folder_path;
  public bool buildings;
  private Dictionary<long, Sprite> _dictionary_sprites = new Dictionary<long, Sprite>();
  private UnitSpriteConstructorAtlas _atlas;

  public override void create()
  {
    base.create();
    if (this._atlas != null)
      return;
    this._atlas = new UnitSpriteConstructorAtlas(this.atlas_id, this.big_atlas);
  }

  public void resetAtlas()
  {
    this.clear();
    this._atlas.setBigAtlas(this.big_atlas);
  }

  public bool hasSprite(long pID) => this._dictionary_sprites.ContainsKey(pID);

  public void addSprite(long pHashCode, Sprite pSprite)
  {
    this._dictionary_sprites[pHashCode] = pSprite;
  }

  public Sprite getSprite(long pID)
  {
    Sprite sprite;
    this._dictionary_sprites.TryGetValue(pID, out sprite);
    return sprite;
  }

  public UnitSpriteConstructorAtlas getAtlas() => this._atlas;

  public void checkAtlasDirty() => this._atlas.checkDirty();

  public void clear()
  {
    this._dictionary_sprites.Clear();
    this._atlas.clear();
    this.checkAtlasDirty();
  }

  public int countSprites() => this._dictionary_sprites.Count;

  public int countTextures() => this._atlas.textures.Count;
}
