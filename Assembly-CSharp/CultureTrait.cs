// Decompiled with JetBrains decompiler
// Type: CultureTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class CultureTrait : BaseTrait<CultureTrait>
{
  public bool is_weapon_trait;
  public List<string> related_weapon_subtype_ids;
  public List<string> related_weapons_ids;
  public bool town_layout_plan;
  public PassableZoneChecker passable_zone_checker;

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_culture;
  }

  public override string typed_id => "culture_trait";

  protected override IEnumerable<ITraitsOwner<CultureTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<CultureTrait>>) World.world.cultures;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.culture_trait_groups.get(this.group_id);
  }

  public void addWeaponSpecial(string pID)
  {
    if (this.related_weapons_ids == null)
      this.related_weapons_ids = new List<string>();
    this.related_weapons_ids.Add(pID);
    this.is_weapon_trait = true;
  }

  public void addWeaponSubtype(string pSubtype)
  {
    if (this.related_weapon_subtype_ids == null)
      this.related_weapon_subtype_ids = new List<string>();
    this.related_weapon_subtype_ids.Add(pSubtype);
    this.is_weapon_trait = true;
  }

  public void setTownLayoutPlan(PassableZoneChecker pZoneCheckerDelegate)
  {
    this.town_layout_plan = true;
    this.passable_zone_checker = pZoneCheckerDelegate;
  }
}
