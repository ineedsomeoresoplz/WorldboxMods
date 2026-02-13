// Decompiled with JetBrains decompiler
// Type: KnowledgeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class KnowledgeAsset : Asset, ILocalizedAsset
{
  [DefaultValue(true)]
  public bool show_in_knowledge_window = true;
  public string path_icon;
  public string path_icon_easter_egg;
  public string button_prefab_path;
  public string window_id;
  public KnowledgeButtonLoader load_button;
  public ButtonTipLoader tip_button_loader;
  public ButtonTooltipLoader show_tooltip;
  public LibraryGetter get_library;
  public SpriteGetter get_asset_sprite;
  public OnKnowledgeIconClick click_icon_action = new OnKnowledgeIconClick(KnowledgeAsset.showWindow);
  private Sprite _cache_icon;
  private ILibraryWithUnlockables _cache_library;

  public string getLocaleID() => "knowledge_" + this.id;

  public Sprite getIcon()
  {
    if (this._cache_icon == null)
      this._cache_icon = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cache_icon;
  }

  public int countTotal() => this.get_library().countTotalKnowledge();

  public int countUnlockedByPlayer() => this.get_library().countUnlockedByPlayer();

  private static void showWindow(KnowledgeAsset pAsset)
  {
    ScrollWindow.showWindow(pAsset.window_id);
  }
}
