// Decompiled with JetBrains decompiler
// Type: MapSizeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class MapSizeAsset : Asset, ILocalizedAsset
{
  public Sprite cached_sprite;
  public string path_icon;
  public bool show_warning;
  public int size = 1;

  public string getLocaleID() => "map_size_" + this.id;

  public Sprite getIconSprite()
  {
    if (Object.op_Equality((Object) this.cached_sprite, (Object) null))
      this.cached_sprite = SpriteTextureLoader.getSprite("ui/Icons/" + this.path_icon);
    return this.cached_sprite;
  }
}
