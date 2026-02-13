// Decompiled with JetBrains decompiler
// Type: BookTypeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BookTypeAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public int writing_rate = 1;
  public string name_template;
  public string path_icons;
  public string color_text;
  public BookReadAction read_action;
  public bool save_culture = true;
  public bool save_religion = true;
  public BookRateCalc rate_calc;
  public BookRequirementCheck requirement_check;
  private Sprite[] _cached_icons;
  public BaseStats base_stats = new BaseStats();

  public string getNewIconPath()
  {
    if (this._cached_icons == null)
      this._cached_icons = SpriteTextureLoader.getSpriteList(this.getFullIconPath());
    return ((Object) this._cached_icons.GetRandom<Sprite>()).name;
  }

  public string getFullIconPath() => "books/book_icons/" + this.path_icons;

  public string getTypeID() => "book_type_" + this.id;

  public string getLocaleID() => this.getTypeID();

  public string getDescriptionID() => "book_type_info_" + this.id;

  public string getDescriptionTranslated() => LocalizedTextManager.getText(this.getDescriptionID());
}
