// Decompiled with JetBrains decompiler
// Type: BaseCategoryAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BaseCategoryAsset : Asset, ILocalizedAsset
{
  public string name;
  public string color;
  public bool show_counter = true;
  [NonSerialized]
  public Color? _color;

  public virtual string getLocaleID() => this.name;

  public Color getColor()
  {
    if (!this._color.HasValue)
      this._color = new Color?(Toolbox.makeColor(this.color));
    return this._color.Value;
  }
}
