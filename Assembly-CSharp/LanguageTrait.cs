// Decompiled with JetBrains decompiler
// Type: LanguageTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class LanguageTrait : BaseTrait<LanguageTrait>
{
  public BookTraitAction read_book_trait_action;

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_language;
  }

  public override string typed_id => "language_trait";

  protected override IEnumerable<ITraitsOwner<LanguageTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<LanguageTrait>>) World.world.languages;
  }

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.language_trait_groups.get(this.group_id);
  }
}
