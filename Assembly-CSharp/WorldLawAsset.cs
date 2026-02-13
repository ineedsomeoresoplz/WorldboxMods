// Decompiled with JetBrains decompiler
// Type: WorldLawAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
[Serializable]
public class WorldLawAsset : 
  BaseAugmentationAsset,
  IDescription2Asset,
  IDescriptionAsset,
  ILocalizedAsset
{
  public bool default_state;
  public PlayerOptionAction on_state_change;
  public PlayerOptionAction on_state_enabled;
  public OnWorldLoadAction on_world_load;
  public string icon_path;
  public bool can_turn_off = true;
  public bool requires_premium;
  private bool _cached_enabled;

  private static WorldLaws _world_laws => World.world.world_laws;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isEnabled() => this._cached_enabled;

  public bool isEnabledRaw() => this.getOption().boolVal;

  public PlayerOptionData getOption() => WorldLawAsset._world_laws.dict[this.id];

  public void updateCachedEnabled(WorldLaws pWorldLaws)
  {
    this._cached_enabled = pWorldLaws.isEnabled(this.id);
  }

  public void toggle(bool pState)
  {
    this.getOption().boolVal = pState;
    this._cached_enabled = pState;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.world_law_groups.get(this.group_id);
  }

  public override string getLocaleID() => this.id + "_title";

  public string getDescriptionID() => this.id + "_description";

  public string getDescriptionID2() => this.id + "_description_2";

  public string getTranslatedName() => LocalizedTextManager.getText(this.getLocaleID());

  public string getTranslatedDescription() => LocalizedTextManager.getText(this.getDescriptionID());

  public string getTranslatedDescription2()
  {
    return LocalizedTextManager.getText(this.getDescriptionID2());
  }
}
