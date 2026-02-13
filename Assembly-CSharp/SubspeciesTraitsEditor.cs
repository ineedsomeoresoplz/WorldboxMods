// Decompiled with JetBrains decompiler
// Type: SubspeciesTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SubspeciesTraitsEditor : 
  TraitsEditor<SubspeciesTrait, SubspeciesTraitButton, SubspeciesTraitEditorButton, SubspeciesTraitGroupAsset, SubspeciesTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Subspecies;

  protected override List<SubspeciesTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.subspecies_trait_groups.list;
  }

  protected override List<SubspeciesTrait> all_augmentations_list
  {
    get => AssetManager.subspecies_traits.list;
  }

  protected override SubspeciesTrait edited_marker_augmentation
  {
    get => AssetManager.subspecies_traits.get("gmo");
  }

  protected override List<string> filter_traits => this.getActorAsset().trait_filter_subspecies;

  protected override List<string> filter_trait_groups
  {
    get => this.getActorAsset().trait_group_filter_subspecies;
  }

  protected override void onNanoWasModified()
  {
    ((Subspecies) this.getTraitsOwner()).eventGMO();
    base.onNanoWasModified();
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_subspecies.checkBySignal();
    AchievementLibrary.swarm.checkBySignal((object) this.getTraitsOwner());
  }
}
