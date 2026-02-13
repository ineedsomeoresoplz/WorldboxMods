// Decompiled with JetBrains decompiler
// Type: KingdomTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class KingdomTrait : BaseTrait<KingdomTrait>
{
  public bool is_local_tax_trait;
  public bool is_tribute_tax_trait;
  public float tax_rate;

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_kingdom;
  }

  public override string typed_id => "kingdom_trait";

  protected override IEnumerable<ITraitsOwner<KingdomTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<KingdomTrait>>) World.world.kingdoms;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.kingdoms_traits_groups.get(this.group_id);
  }
}
