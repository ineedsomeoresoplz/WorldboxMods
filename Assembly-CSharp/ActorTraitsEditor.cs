// Decompiled with JetBrains decompiler
// Type: ActorTraitsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ActorTraitsEditor : 
  TraitsEditor<ActorTrait, ActorTraitButton, ActorTraitEditorButton, ActorTraitGroupAsset, ActorTraitGroupElement>
{
  protected override MetaType meta_type => MetaType.Unit;

  protected override List<ActorTraitGroupAsset> augmentation_groups_list
  {
    get => AssetManager.trait_groups.list;
  }

  protected override ActorTrait edited_marker_augmentation
  {
    get => AssetManager.traits.get("scar_of_divinity");
  }

  protected override List<ActorTrait> all_augmentations_list => AssetManager.traits.list;

  protected override void onEnableRain()
  {
    TraitRainAsset tAsset = AssetManager.trait_rains.get(Config.selected_trait_editor);
    this.augmentations_list_link = tAsset.get_list();
    this.rain_editor_state = tAsset.get_state();
    this.rain_state_toggle_action = (ToggleRainStateAction) (() => this.toggleRainState(tAsset));
    this.art.sprite = this.rain_editor_state == RainState.Add ? tAsset.getSpriteArt() : tAsset.getSpriteArtVoid();
    this.validateRainData();
    this.rain_state_switcher.toggleState(this.rain_editor_state == RainState.Remove);
    this.augmentations_hashset.Clear();
    this.augmentations_hashset.UnionWith((IEnumerable<string>) this.augmentations_list_link);
    this.saveRainValues();
    this.loadEditorSelectedAugmentations();
    GodPower godPower = AssetManager.powers.get(Config.selected_trait_editor);
    this.window_title_text.key = godPower.getLocaleID();
    this.window_title_text.updateText();
    this.power_icon.sprite = godPower.getIconSprite();
    for (int index = 0; index < this.powers_icons.childCount; ++index)
      ((Component) ((Component) this.powers_icons).transform.GetChild(index)).GetComponent<Image>().sprite = godPower.getIconSprite();
  }

  protected override bool addTrait(ActorTrait pTrait) => base.addTrait(pTrait);

  protected override void onNanoWasModified()
  {
    base.onNanoWasModified();
    Actor traitsOwner = (Actor) this.getTraitsOwner();
    traitsOwner.makeStunnedFromUI();
    traitsOwner.updateStats();
  }

  protected override void loadEditorSelectedButton(ActorTraitButton pButton, string pAugmentationId)
  {
    base.loadEditorSelectedButton(pButton, pAugmentationId);
    pButton.load(pAugmentationId);
  }

  protected override bool isAugmentationExists(string pId) => AssetManager.traits.has(pId);

  public override void scrollToGroupStarter(GameObject pButton, bool pIgnoreTooltipCheck)
  {
    if (!this.rain_editor && !this.getActorAsset().can_edit_traits)
      return;
    base.scrollToGroupStarter(pButton, pIgnoreTooltipCheck);
  }

  protected void toggleRainState(TraitRainAsset pAsset)
  {
    RainState pState;
    if (pAsset.get_state() == RainState.Add)
    {
      pState = RainState.Remove;
      this.rain_state_switcher.toggleState(true);
      this.art.sprite = pAsset.getSpriteArtVoid();
    }
    else
    {
      pState = RainState.Add;
      this.rain_state_switcher.toggleState(false);
      this.art.sprite = pAsset.getSpriteArt();
    }
    IllustrationFadeIn component = ((Component) this.art).GetComponent<IllustrationFadeIn>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.startTween();
    pAsset.set_state(pState);
    this.rain_editor_state = pState;
    this.reloadButtons();
  }
}
