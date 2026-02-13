// Decompiled with JetBrains decompiler
// Type: KingdomTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class KingdomTraitsEditor : 
  TraitsEditor<KingdomTrait, KingdomTraitButton, KingdomTraitEditorButton, KingdomTraitGroupAsset, KingdomTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Kingdom;

  protected override List<KingdomTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.kingdoms_traits_groups.list;
  }

  protected override List<KingdomTrait> all_augmentations_list => AssetManager.kingdoms_traits.list;
}
