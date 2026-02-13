// Decompiled with JetBrains decompiler
// Type: ReligionTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ReligionTrait : BaseTrait<ReligionTrait>
{
  public string transformation_biome_id;

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_religion;
  }

  public override string typed_id => "religion_trait";

  protected override IEnumerable<ITraitsOwner<ReligionTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<ReligionTrait>>) World.world.religions;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.religion_trait_groups.get(this.group_id);
  }
}
