// Decompiled with JetBrains decompiler
// Type: ArchitectMood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ArchitectMood : Asset, ILocalizedAsset
{
  public string color_main;
  public string color_text;
  public string path_icon;
  private Color _cached_color = Color.clear;
  private Color _cached_color_text = Color.clear;
  private Sprite _cached_sprite;

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this._cached_sprite, (Object) null))
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cached_sprite;
  }

  public string getLocaleID() => "architect_mood_" + this.id;

  public Color getColor()
  {
    if (Color.op_Equality(this._cached_color, Color.clear))
      this._cached_color = Toolbox.makeColor(this.color_main);
    return this._cached_color;
  }

  public Color getColorText()
  {
    if (Color.op_Equality(this._cached_color_text, Color.clear))
      this._cached_color_text = Toolbox.makeColor(this.color_text);
    return this._cached_color_text;
  }
}
