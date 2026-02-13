// Decompiled with JetBrains decompiler
// Type: LanguageTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class LanguageTraitsEditor : 
  TraitsEditor<LanguageTrait, LanguageTraitButton, LanguageTraitEditorButton, LanguageTraitGroupAsset, LanguageTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Language;

  protected override List<LanguageTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.language_trait_groups.list;
  }

  protected override List<LanguageTrait> all_augmentations_list
  {
    get => AssetManager.language_traits.list;
  }

  protected override LanguageTrait edited_marker_augmentation
  {
    get => AssetManager.language_traits.get("divine_encryption");
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_language.checkBySignal();
  }
}
