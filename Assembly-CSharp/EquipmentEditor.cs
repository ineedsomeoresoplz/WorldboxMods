// Decompiled with JetBrains decompiler
// Type: EquipmentEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class EquipmentEditor : 
  AugmentationsEditor<EquipmentAsset, EquipmentButton, EquipmentEditorButton, ItemGroupAsset, EquipmentGroupElement, IEquipmentWindow, IEquipmentEditor>,
  IEquipmentEditor,
  IAugmentationsEditor
{
  [SerializeField]
  protected Sprite sprite_art;
  [SerializeField]
  protected Sprite sprite_art_void;

  protected override List<ItemGroupAsset> augmentation_groups_list => AssetManager.item_groups.list;

  protected override EquipmentAsset edited_marker_augmentation => (EquipmentAsset) null;

  protected override List<EquipmentAsset> all_augmentations_list => AssetManager.items.list;

  protected override void onEnableRain()
  {
    this.rain_editor_state = PlayerConfig.instance.data.equipment_editor_state;
    this.augmentations_list_link = PlayerConfig.instance.data.equipment_editor;
    this.validateRainData();
    this.augmentations_hashset.Clear();
    this.augmentations_hashset.UnionWith((IEnumerable<string>) this.augmentations_list_link);
    this.loadEditorSelectedAugmentations();
    this.rain_state_toggle_action = (ToggleRainStateAction) (() => this.toggleRainState(ref PlayerConfig.instance.data.equipment_editor_state));
    this.rain_state_switcher.toggleState(this.rain_editor_state == RainState.Remove);
  }

  protected override void OnEnable()
  {
    if (!this.rain_editor)
    {
      Actor currentActor = this.getCurrentActor();
      if (!currentActor.canEditEquipment())
        return;
      foreach (EquipmentEditorButton augmentationButton in this.all_augmentation_buttons)
      {
        EquipmentAsset elementAsset = augmentationButton.augmentation_button.getElementAsset();
        bool flag = true;
        if (!currentActor.asset.canEditItem(elementAsset))
          flag = false;
        ((Component) augmentationButton).gameObject.SetActive(flag);
      }
    }
    base.OnEnable();
  }

  protected override void metaAugmentationClick(EquipmentEditorButton pButton)
  {
    if (!this.isAugmentationAvailable(pButton.augmentation_button))
      return;
    EquipmentButton augmentationButton = pButton.augmentation_button;
    if (this.canChangeSlot(pButton.augmentation_button.getElementAsset()))
    {
      int num = this.hasAugmentation(augmentationButton) ? 1 : 0;
      if (!this.isSlotEmpty(augmentationButton))
        this.removeAugmentation(augmentationButton);
      if (num == 0)
        this.addAugmentation(augmentationButton);
    }
    this.augmentation_window.checkEquipmentTabIcon();
    base.metaAugmentationClick(pButton);
  }

  protected override void rainAugmentationClick(EquipmentEditorButton pButton)
  {
    if (!this.isAugmentationAvailable(pButton.augmentation_button))
      return;
    string id = pButton.augmentation_button.getElementAsset().id;
    if (!this.augmentations_hashset.Contains(id))
      this.augmentations_hashset.Add(id);
    else
      this.augmentations_hashset.Remove(id);
    base.rainAugmentationClick(pButton);
  }

  protected override void showActiveButtons() => this.augmentation_window.reloadEquipment();

  protected override ListPool<EquipmentAsset> getOrderedAugmentationsList()
  {
    return new ListPool<EquipmentAsset>((ICollection<EquipmentAsset>) this.all_augmentations_list);
  }

  protected override void createButton(EquipmentAsset pElement, EquipmentGroupElement pGroup)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    EquipmentEditor.\u003C\u003Ec__DisplayClass14_0 cDisplayClass140 = new EquipmentEditor.\u003C\u003Ec__DisplayClass14_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass140.\u003C\u003E4__this = this;
    if (!pElement.show_in_meta_editor)
      return;
    bool flag = true;
    if (!this.rain_editor && !this.getCurrentActor().asset.canEditItem(pElement))
      flag = false;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass140.tEditorButton = Object.Instantiate<EquipmentEditorButton>(this.prefab_editor_augmentation, pGroup.augmentation_buttons_transform);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass140.tEditorButton.augmentation_button.is_editor_button = true;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass140.tEditorButton.augmentation_button.load(pElement);
    // ISSUE: reference to a compiler-generated field
    this.all_augmentation_buttons.Add(cDisplayClass140.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    pGroup.augmentation_buttons.Add(cDisplayClass140.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    ((UnityEventBase) cDisplayClass140.tEditorButton.augmentation_button.button.onClick).RemoveAllListeners();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((UnityEvent) cDisplayClass140.tEditorButton.augmentation_button.button.onClick).AddListener(new UnityAction((object) cDisplayClass140, __methodptr(\u003CcreateButton\u003Eb__0)));
    // ISSUE: reference to a compiler-generated field
    ((Component) cDisplayClass140.tEditorButton).gameObject.SetActive(flag);
  }

  protected override void startSignal() => AchievementLibrary.equipment_explorer.checkBySignal();

  private bool canChangeSlot(EquipmentAsset pAsset)
  {
    return pAsset.can_be_given && this.getSlotFromCurrentActor(pAsset.equipment_type).canChangeSlot();
  }

  private bool isSlotEmpty(EquipmentButton pButton)
  {
    return this.getSlotFromCurrentActor(pButton.getElementAsset().equipment_type).isEmpty();
  }

  protected override bool hasAugmentation(EquipmentButton pButton)
  {
    ActorEquipmentSlot fromCurrentActor = this.getSlotFromCurrentActor(pButton.getElementAsset().equipment_type);
    if (fromCurrentActor.isEmpty())
      return false;
    Item obj = fromCurrentActor.getItem();
    string id = pButton.getElementAsset().id;
    return obj.getAsset().id == id;
  }

  protected override bool addAugmentation(EquipmentButton pButton)
  {
    Actor currentActor = this.getCurrentActor();
    Item pItem = World.world.items.generateItem(pButton.getElementAsset(), currentActor.kingdom, World.world.map_stats.player_name, pActor: currentActor, pByPlayer: true);
    pItem.addMod("divine_rune");
    currentActor.equipment.setItem(pItem, currentActor);
    return true;
  }

  protected override bool removeAugmentation(EquipmentButton pButton)
  {
    Actor currentActor = this.getCurrentActor();
    currentActor.equipment.getSlot(pButton.getElementAsset().equipment_type).takeAwayItem();
    currentActor.setStatsDirty();
    return true;
  }

  private ActorEquipmentSlot getSlotFromCurrentActor(EquipmentType pType)
  {
    return this.getCurrentActor().equipment.getSlot(pType);
  }

  private Actor getCurrentActor() => SelectedUnit.unit;

  protected override void loadEditorSelectedButton(EquipmentButton pButton, string pAugmentationId)
  {
    base.loadEditorSelectedButton(pButton, pAugmentationId);
    EquipmentAsset pElement = AssetManager.items.get(pAugmentationId);
    pButton.load(pElement);
  }

  protected override bool isAugmentationExists(string pId) => AssetManager.items.has(pId);

  protected override void toggleRainState(ref RainState pState)
  {
    base.toggleRainState(ref pState);
    this.art.sprite = pState == RainState.Add ? this.sprite_art : this.sprite_art_void;
    if (pState != RainState.Add)
      return;
    this.augmentations_hashset.Clear();
    this.augmentations_hashset.UnionWith((IEnumerable<string>) this.augmentations_list_link);
    this.reloadButtons();
  }
}
