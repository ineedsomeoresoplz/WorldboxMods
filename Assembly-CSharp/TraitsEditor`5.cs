// Decompiled with JetBrains decompiler
// Type: TraitsEditor`5
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public abstract class TraitsEditor<TTrait, TTraitButton, TTraitEditorButton, TTraitGroupAsset, TTraitGroup> : 
  AugmentationsEditor<TTrait, TTraitButton, TTraitEditorButton, TTraitGroupAsset, TTraitGroup, ITraitWindow<TTrait, TTraitButton>, ITraitsEditor<TTrait>>,
  ITraitsEditor<TTrait>,
  IAugmentationsEditor
  where TTrait : BaseTrait<TTrait>
  where TTraitButton : TraitButton<TTrait>
  where TTraitEditorButton : TraitEditorButton<TTraitButton, TTrait>
  where TTraitGroupAsset : BaseTraitGroupAsset
  where TTraitGroup : TraitGroupElement<TTrait, TTraitButton, TTraitEditorButton>
{
  protected virtual MetaType meta_type => throw new NotImplementedException();

  protected MetaTypeAsset meta_type_asset
  {
    get => AssetManager.meta_type_library.getAsset(this.meta_type);
  }

  protected virtual List<string> filter_traits => (List<string>) null;

  protected virtual List<string> filter_trait_groups => (List<string>) null;

  protected override void OnEnable()
  {
    if (!this.rain_editor && this.filter_traits != null)
    {
      foreach (TTraitEditorButton augmentationButton in this.all_augmentation_buttons)
        ((Component) (object) augmentationButton).gameObject.SetActive(!this.filter_traits.Contains(augmentationButton.augmentation_button.getElementAsset().id));
    }
    base.OnEnable();
  }

  protected override void metaAugmentationClick(TTraitEditorButton pButton)
  {
    TTraitButton augmentationButton = pButton.augmentation_button;
    TTrait elementAsset = pButton.augmentation_button.getElementAsset();
    bool flag1 = this.hasAugmentation(augmentationButton);
    bool flag2 = elementAsset.isAvailable();
    bool unlockedWithAchievement = elementAsset.unlocked_with_achievement;
    if (!flag2 && (!unlockedWithAchievement || unlockedWithAchievement && !flag1))
      return;
    if (elementAsset.can_be_removed)
    {
      if (flag1)
      {
        this.removeAugmentation(augmentationButton);
      }
      else
      {
        if (elementAsset.hasOppositeTraits<TTrait>())
        {
          foreach (TTrait oppositeTrait in elementAsset.opposite_traits)
            this.removeTrait(oppositeTrait);
        }
        this.addAugmentation(augmentationButton);
      }
      this.onNanoWasModified();
    }
    base.metaAugmentationClick(pButton);
  }

  protected override void rainAugmentationClick(TTraitEditorButton pButton)
  {
    if (!this.isAugmentationAvailable(pButton.augmentation_button))
      return;
    TTrait elementAsset = pButton.augmentation_button.getElementAsset();
    if (this.augmentations_hashset.Contains(elementAsset.id))
    {
      this.augmentations_hashset.Remove(elementAsset.id);
    }
    else
    {
      if (elementAsset.hasOppositeTraits<TTrait>())
      {
        foreach (TTrait oppositeTrait in elementAsset.opposite_traits)
          this.augmentations_hashset.Remove(oppositeTrait.id);
      }
      this.augmentations_hashset.Add(elementAsset.id);
    }
    base.rainAugmentationClick(pButton);
  }

  protected override void showActiveButtons() => this.augmentation_window.reloadTraits(false);

  protected override void createButton(TTrait pElement, TTraitGroup pGroup)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TraitsEditor<TTrait, TTraitButton, TTraitEditorButton, TTraitGroupAsset, TTraitGroup>.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120 = new TraitsEditor<TTrait, TTraitButton, TTraitEditorButton, TTraitGroupAsset, TTraitGroup>.\u003C\u003Ec__DisplayClass12_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass120.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass120.tEditorButton = Object.Instantiate<TTraitEditorButton>(this.prefab_editor_augmentation, pGroup.augmentation_buttons_transform);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass120.tEditorButton.augmentation_button.load(pElement.id);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass120.tEditorButton.augmentation_button.is_editor_button = true;
    // ISSUE: reference to a compiler-generated field
    this.all_augmentation_buttons.Add(cDisplayClass120.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    pGroup.augmentation_buttons.Add(cDisplayClass120.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((UnityEvent) cDisplayClass120.tEditorButton.augmentation_button.button.onClick).AddListener(new UnityAction((object) cDisplayClass120, __methodptr(\u003CcreateButton\u003Eb__0)));
  }

  protected override void startSignal()
  {
    AchievementLibrary.traits_explorer_40.checkBySignal();
    AchievementLibrary.traits_explorer_60.checkBySignal();
    AchievementLibrary.traits_explorer_90.checkBySignal();
  }

  protected override void onNanoWasModified()
  {
    if ((object) this.edited_marker_augmentation == null)
      return;
    this.addTrait(this.edited_marker_augmentation);
  }

  protected override void checkEnabledGroups()
  {
    foreach (KeyValuePair<string, TTraitGroup> dictGroup in this.dict_groups)
    {
      string key = dictGroup.Key;
      TTraitGroup traitGroup = dictGroup.Value;
      if (this.filter_trait_groups != null && this.filter_trait_groups.Contains(key))
      {
        ((Component) (object) traitGroup).gameObject.SetActive(false);
      }
      else
      {
        bool flag = traitGroup.countActiveButtons() > 0;
        ((Component) (object) traitGroup).gameObject.SetActive(flag);
      }
    }
  }

  protected override bool hasAugmentation(TTraitButton pButton)
  {
    return this.hasTrait(pButton.getElementAsset());
  }

  protected override bool addAugmentation(TTraitButton pButton)
  {
    return this.addTrait(pButton.getElementAsset());
  }

  protected override bool removeAugmentation(TTraitButton pButton)
  {
    return this.removeTrait(pButton.getElementAsset());
  }

  public virtual ITraitsOwner<TTrait> getTraitsOwner()
  {
    return (ITraitsOwner<TTrait>) this.meta_type_asset.get_selected();
  }

  public ActorAsset getActorAsset() => this.getTraitsOwner().getActorAsset();

  protected virtual bool hasTrait(TTrait pTrait) => this.getTraitsOwner().hasTrait(pTrait);

  protected virtual bool addTrait(TTrait pTrait)
  {
    this.getTraitsOwner().traitModifiedEvent();
    return this.getTraitsOwner().addTrait(pTrait);
  }

  protected virtual bool removeTrait(TTrait pTrait) => this.getTraitsOwner().removeTrait(pTrait);
}
