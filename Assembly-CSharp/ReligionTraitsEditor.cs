// Decompiled with JetBrains decompiler
// Type: ReligionTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ReligionTraitsEditor : 
  TraitsEditor<ReligionTrait, ReligionTraitButton, ReligionTraitEditorButton, ReligionTraitGroupAsset, ReligionTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Religion;

  protected override List<ReligionTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.religion_trait_groups.list;
  }

  protected override List<ReligionTrait> all_augmentations_list
  {
    get => AssetManager.religion_traits.list;
  }

  protected override ReligionTrait edited_marker_augmentation
  {
    get => AssetManager.religion_traits.get("divine_insight");
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_religion.checkBySignal();
  }
}
