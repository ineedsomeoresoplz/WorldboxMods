// Decompiled with JetBrains decompiler
// Type: WindowAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class WindowAsset : Asset
{
  public bool preload;
  public string related_parent_window;
  [NonSerialized]
  public MetaTypeAsset meta_type_asset;
  [DefaultValue(true)]
  public bool window_toolbar_enabled = true;
  public string icon_path = "iconAye";
  public HoveringBGIconsGetter get_hovering_icons = new HoveringBGIconsGetter(WindowAsset.getDefaultHoveringIconPath);
  [DefaultValue(true)]
  public bool is_testable = true;
  [NonSerialized]
  private Sprite _cached_icon;

  public Sprite getSprite()
  {
    if (this._cached_icon == null)
      this._cached_icon = SpriteTextureLoader.getSprite("ui/Icons/" + this.icon_path);
    return this._cached_icon;
  }

  private static IEnumerable<string> getDefaultHoveringIconPath(WindowAsset pAsset)
  {
    yield return pAsset.icon_path;
  }
}
