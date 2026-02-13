// Decompiled with JetBrains decompiler
// Type: BaseUnlockableAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class BaseUnlockableAsset : Asset, ILocalizedAsset
{
  public string path_icon;
  [DefaultValue(true)]
  public bool show_for_unlockables_ui = true;
  [DefaultValue(null)]
  public BaseStats base_stats;
  public bool unlocked_with_achievement;
  [DefaultValue(true)]
  public bool needs_to_be_explored = true;
  public string achievement_id;
  [DefaultValue(true)]
  public bool show_in_knowledge_window = true;
  [NonSerialized]
  protected Sprite cached_sprite;
  [DefaultValue(true)]
  public bool has_locales = true;

  protected GameProgressData _progress_data => GameProgress.instance?.data;

  protected virtual HashSet<string> progress_elements
  {
    get => throw new NotImplementedException(this.GetType().Name);
  }

  public void setUnlockedWithAchievement(string pAchievementID)
  {
    this.unlocked_with_achievement = true;
    this.achievement_id = pAchievementID;
  }

  public virtual bool unlock(bool pSaveData = true)
  {
    if (this.progress_elements == null || this.isAvailable())
      return false;
    this.progress_elements.Add(this.id);
    if (pSaveData)
      GameProgress.saveData();
    if (!this.unlocked_with_achievement && this.has_locales)
      WorldTip.showNowTop("new_knowledge_gain".Localize().Replace("$knowledge$", this.getLocaleID().Localize()), false);
    return true;
  }

  public bool isUnlocked()
  {
    return this.isDebugUnlockedAll() || this.isCheatEnabled() || this.progress_elements.Contains(this.id);
  }

  public bool isAvailable()
  {
    if (this.isDebugUnlockedAll() || this.isCheatEnabled())
      return true;
    if (this.unlocked_with_achievement)
      return GameProgress.isAchievementUnlocked(this.achievement_id);
    return !this.needs_to_be_explored || this.isUnlocked();
  }

  public bool isUnlockedByPlayer()
  {
    if (this.unlocked_with_achievement)
      return GameProgress.isAchievementUnlocked(this.achievement_id);
    return !this.needs_to_be_explored || this.progress_elements.Contains(this.id);
  }

  protected virtual bool isDebugUnlockedAll()
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public bool isCheatEnabled() => WorldLawLibrary.world_law_cursed_world.isEnabled();

  public bool ShouldSerializebase_stats() => !this.base_stats.isEmpty();

  public virtual string getLocaleID() => !this.has_locales ? (string) null : this.id;

  public string getAchievementLocaleID() => this.getAchievement()?.getLocaleID();

  public Achievement getAchievement()
  {
    return !this.unlocked_with_achievement ? (Achievement) null : AssetManager.achievements.get(this.achievement_id);
  }

  public virtual Sprite getSprite()
  {
    if (this.cached_sprite == null)
      this.cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this.cached_sprite;
  }
}
