// Decompiled with JetBrains decompiler
// Type: CultureTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CultureTraitsEditor : 
  TraitsEditor<CultureTrait, CultureTraitButton, CultureTraitEditorButton, CultureTraitGroupAsset, CultureTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Culture;

  protected override List<CultureTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.culture_trait_groups.list;
  }

  protected override List<CultureTrait> all_augmentations_list => AssetManager.culture_traits.list;

  protected override CultureTrait edited_marker_augmentation
  {
    get => AssetManager.culture_traits.get("ethno_sculpted");
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_culture.checkBySignal();
  }

  protected override void metaAugmentationClick(CultureTraitEditorButton pButton)
  {
    base.metaAugmentationClick(pButton);
    if (pButton.augmentation_button.getElementAsset().group_id != "succession")
      return;
    AchievementLibrary.succession.check();
  }
}
