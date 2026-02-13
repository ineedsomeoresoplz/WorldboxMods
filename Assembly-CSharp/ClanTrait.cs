// Decompiled with JetBrains decompiler
// Type: ClanTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ClanTrait : BaseTrait<ClanTrait>
{
  public BaseStats base_stats_male = new BaseStats();
  public BaseStats base_stats_female = new BaseStats();

  protected override HashSet<string> progress_elements => this._progress_data?.unlocked_traits_clan;

  public override string typed_id => "clan_trait";

  protected override IEnumerable<ITraitsOwner<ClanTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<ClanTrait>>) World.world.clans;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.clan_trait_groups.get(this.group_id);
  }
}
