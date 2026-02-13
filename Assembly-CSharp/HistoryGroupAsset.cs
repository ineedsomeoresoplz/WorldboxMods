// Decompiled with JetBrains decompiler
// Type: HistoryGroupAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class HistoryGroupAsset : Asset, ILocalizedAsset
{
  public string icon_path;
  private Sprite _icon_cache;

  public string getLocaleID() => "history_group_" + this.id;

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this._icon_cache, (Object) null))
      this._icon_cache = SpriteTextureLoader.getSprite(this.icon_path);
    return this._icon_cache;
  }
}
