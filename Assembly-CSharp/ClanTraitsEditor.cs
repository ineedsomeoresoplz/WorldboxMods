// Decompiled with JetBrains decompiler
// Type: ClanTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClanTraitsEditor : 
  TraitsEditor<ClanTrait, ClanTraitButton, ClanTraitEditorButton, ClanTraitGroupAsset, ClanTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Clan;

  protected override List<ClanTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.clan_trait_groups.list;
  }

  protected override List<ClanTrait> all_augmentations_list => AssetManager.clan_traits.list;

  protected override ClanTrait edited_marker_augmentation => AssetManager.clan_traits.get("geb");

  protected override void startSignal() => AchievementLibrary.trait_explorer_clan.checkBySignal();
}
