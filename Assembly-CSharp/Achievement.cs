// Decompiled with JetBrains decompiler
// Type: Achievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Achievement : Asset, IDescriptionAsset, ILocalizedAsset
{
  public string play_store_id;
  public string steam_id;
  public bool hidden;
  public string group = "miscellaneous";
  public string icon;
  public string locale_key;
  public AchievementCheck action;
  public bool unlocks_something;
  public List<BaseUnlockableAsset> unlock_assets;
  [NonSerialized]
  protected Sprite cached_sprite;
  [NonSerialized]
  private SignalAsset _signal;
  public bool has_signal;

  public void checkBySignal(object pCheckData = null)
  {
    SignalManager.add(this._signal, pCheckData);
  }

  public bool check(object pCheckData = null)
  {
    if (AchievementLibrary.isUnlocked(this))
      return true;
    bool flag = true;
    if (this.action != null)
      flag = this.action(pCheckData);
    if (!flag)
      return false;
    AchievementLibrary.unlock(this);
    this.checkUnlockables();
    return true;
  }

  private void checkUnlockables()
  {
    if (!this.isUnlocked() || !this.unlocks_something)
      return;
    foreach (BaseUnlockableAsset unlockAsset in this.unlock_assets)
      unlockAsset.unlock();
  }

  public bool isUnlocked() => AchievementLibrary.isUnlocked(this);

  public string getLocaleID() => this.locale_key;

  public string getDescriptionID() => this.getLocaleID() + "_description";

  public Sprite getIcon()
  {
    if (this.cached_sprite == null)
      this.cached_sprite = SpriteTextureLoader.getSprite(this.icon);
    if (Object.op_Equality((Object) this.cached_sprite, (Object) null))
      Debug.LogError((object) ("Error: Sprite not found : " + this.icon));
    return this.cached_sprite;
  }

  public void setSignal(SignalAsset pSignal) => this._signal = pSignal;

  public SignalAsset getSignal() => this._signal;
}
