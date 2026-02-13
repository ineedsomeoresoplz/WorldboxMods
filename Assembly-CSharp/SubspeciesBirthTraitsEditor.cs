// Decompiled with JetBrains decompiler
// Type: SubspeciesBirthTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SubspeciesBirthTraitsEditor : 
  TraitsEditor<ActorTrait, ActorTraitButton, ActorTraitEditorButton, ActorTraitGroupAsset, ActorTraitGroupElement>
{
  private SubspeciesWindow _subspecies_window;

  protected override MetaType meta_type => MetaType.Subspecies;

  protected override List<ActorTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.trait_groups.list;
  }

  protected override ActorTrait edited_marker_augmentation
  {
    get => AssetManager.traits.get("scar_of_divinity");
  }

  protected override List<ActorTrait> all_augmentations_list => AssetManager.traits.list;

  public override ITraitsOwner<ActorTrait> getTraitsOwner()
  {
    return (ITraitsOwner<ActorTrait>) this.getTraitsContainer();
  }

  private SubspeciesActorBirthTraits getTraitsContainer()
  {
    return this.getSelectedSubspecies().getActorBirthTraits();
  }

  protected override void create()
  {
    base.create();
    this._subspecies_window = ((Component) this).GetComponentInParent<SubspeciesWindow>();
    this.selected_editor_buttons = new ObjectPoolGenericMono<ActorTraitButton>(this.prefab_augmentation, ((Component) this.selected_editor_augmentations_grid).transform);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.augmentations_list_link = this.getSelectedSubspecies().getActorBirthTraits().getTraitsAsStrings();
    this.augmentations_hashset.Clear();
    this.augmentations_hashset.UnionWith((IEnumerable<string>) this.augmentations_list_link);
    this.loadEditorSelectedAugmentations();
  }

  protected override void onNanoWasModified()
  {
    this.getSelectedSubspecies().eventGMO();
    base.onNanoWasModified();
  }

  protected override void loadEditorSelectedButton(ActorTraitButton pButton, string pAugmentationId)
  {
    base.loadEditorSelectedButton(pButton, pAugmentationId);
    pButton.load(pAugmentationId);
  }

  protected override bool isAugmentationExists(string pId) => AssetManager.traits.has(pId);

  protected override void metaAugmentationClick(ActorTraitEditorButton pButton)
  {
    base.metaAugmentationClick(pButton);
    this.augmentations_hashset.Clear();
    this.augmentations_hashset.UnionWith((IEnumerable<string>) this.getTraitsContainer().getTraitsAsStrings());
    this.loadEditorSelectedAugmentations();
  }

  protected override void refreshAugmentationWindow()
  {
    this._subspecies_window.updateStats();
    this._subspecies_window.reloadBanner();
  }

  protected override void showActiveButtons() => this.loadEditorSelectedAugmentations();

  private Subspecies getSelectedSubspecies() => (Subspecies) this.meta_type_asset.get_selected();
}
