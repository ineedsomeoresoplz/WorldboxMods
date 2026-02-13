// Decompiled with JetBrains decompiler
// Type: PlotsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PlotsEditor : 
  AugmentationsEditor<PlotAsset, PlotButton, PlotEditorButton, PlotCategoryAsset, PlotGroupElement, IPlotsWindow, IPlotsEditor>,
  IPlotsEditor,
  IAugmentationsEditor
{
  protected override List<PlotCategoryAsset> augmentation_groups_list
  {
    get => AssetManager.plot_category_library.list;
  }

  protected override PlotAsset edited_marker_augmentation => (PlotAsset) null;

  protected override List<PlotAsset> all_augmentations_list => AssetManager.plots_library.list;

  protected override void metaAugmentationClick(PlotEditorButton pButton)
  {
    if (!this.isAugmentationAvailable(pButton.augmentation_button))
      return;
    PlotButton augmentationButton = pButton.augmentation_button;
    int num = this.hasAugmentation(augmentationButton) ? 1 : 0;
    if (this.getCurrentActor().hasPlot())
      this.removeAugmentation(augmentationButton);
    if (num == 0)
      this.addAugmentation(augmentationButton);
    base.metaAugmentationClick(pButton);
  }

  protected override void createButton(PlotAsset pElement, PlotGroupElement pGroup)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PlotsEditor.\u003C\u003Ec__DisplayClass7_0 cDisplayClass70 = new PlotsEditor.\u003C\u003Ec__DisplayClass7_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass70.\u003C\u003E4__this = this;
    if (!pElement.show_in_meta_editor)
      return;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass70.tEditorButton = Object.Instantiate<PlotEditorButton>(this.prefab_editor_augmentation, pGroup.augmentation_buttons_transform);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass70.tEditorButton.augmentation_button.is_editor_button = true;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass70.tEditorButton.augmentation_button.load(pElement);
    // ISSUE: reference to a compiler-generated field
    this.all_augmentation_buttons.Add(cDisplayClass70.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    pGroup.augmentation_buttons.Add(cDisplayClass70.tEditorButton);
    // ISSUE: reference to a compiler-generated field
    ((UnityEventBase) cDisplayClass70.tEditorButton.augmentation_button.button.onClick).RemoveAllListeners();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((UnityEvent) cDisplayClass70.tEditorButton.augmentation_button.button.onClick).AddListener(new UnityAction((object) cDisplayClass70, __methodptr(\u003CcreateButton\u003Eb__0)));
    // ISSUE: reference to a compiler-generated field
    ((Component) cDisplayClass70.tEditorButton).gameObject.SetActive(true);
  }

  protected override bool hasAugmentation(PlotButton pButton)
  {
    Actor currentActor = this.getCurrentActor();
    return currentActor.hasPlot() && !(currentActor.plot.getAsset().id != pButton.getElementId());
  }

  protected override bool addAugmentation(PlotButton pButton)
  {
    Actor currentActor = this.getCurrentActor();
    currentActor.addStatusEffect("voices_in_my_head");
    if (currentActor.hasPlot())
    {
      World.world.plots.cancelPlot(currentActor.plot);
      currentActor.plot = (Plot) null;
    }
    PlotAsset elementAsset = pButton.getElementAsset();
    if (!elementAsset.canBeDoneByRole(currentActor))
    {
      WorldTip.showNowTop("plot_force_not_possible");
      return false;
    }
    if (elementAsset.check_can_be_forced != null && !elementAsset.check_can_be_forced(currentActor))
    {
      WorldTip.showNowTop("plot_force_bad_conditions");
      return false;
    }
    if (!World.world.plots.tryStartPlot(currentActor, elementAsset))
    {
      WorldTip.showNowTop("plot_force_failed");
      return false;
    }
    WorldTip.showNowTop("plot_force_started");
    currentActor.cancelAllBeh();
    currentActor.setTask("check_plot");
    return true;
  }

  protected override bool removeAugmentation(PlotButton pButton)
  {
    this.getCurrentActor().setPlot((Plot) null);
    return true;
  }

  protected override void showActiveButtons()
  {
  }

  protected override void startSignal() => AchievementLibrary.plots_explorer.checkBySignal();

  protected override bool isAugmentationExists(string pId) => AssetManager.plots_library.has(pId);

  private Actor getCurrentActor() => SelectedUnit.unit;
}
