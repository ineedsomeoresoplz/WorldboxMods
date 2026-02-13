// Decompiled with JetBrains decompiler
// Type: TraitRainAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class TraitRainAsset : Asset
{
  public RainListGetter get_list;
  public RainStateGetter get_state;
  public RainStateSetter set_state;
  public string path_art;
  private Sprite _sprite_art;
  public string path_art_void;
  private Sprite _sprite_art_void;

  public Sprite getSpriteArt()
  {
    if (Object.op_Equality((Object) this._sprite_art, (Object) null))
      this._sprite_art = SpriteTextureLoader.getSprite(this.path_art);
    return this._sprite_art;
  }

  public Sprite getSpriteArtVoid()
  {
    if (Object.op_Equality((Object) this._sprite_art_void, (Object) null))
      this._sprite_art_void = SpriteTextureLoader.getSprite(this.path_art_void);
    return this._sprite_art_void;
  }
}
