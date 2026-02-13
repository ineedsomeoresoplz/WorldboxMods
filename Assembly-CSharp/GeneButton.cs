// Decompiled with JetBrains decompiler
// Type: GeneButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class GeneButton : ChainElement
{
  [SerializeField]
  private GameObject _petri_bg;
  private GeneAssetClickEvent _gene_asset_click_event;

  protected override void create()
  {
    base.create();
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  private void click()
  {
    GeneAssetClickEvent geneAssetClickEvent = this._gene_asset_click_event;
    if (geneAssetClickEvent != null)
      geneAssetClickEvent(this.gene);
    if (InputHelpers.mouseSupported)
      return;
    ((Component) this).GetComponent<TipButton>().hoverAction();
  }

  protected override void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    base.onStartDrag(pOriginalElement);
    this._petri_bg.SetActive(false);
    this.colorChains();
    ((Component) this.locked_bg).gameObject.SetActive(!this.augmentation_asset.isUnlocked());
  }

  internal void locusChild(UnityAction pAction, int pLocusIndex)
  {
    this.hideChains();
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).RemoveListener(new UnityAction((object) this, __methodptr(click)));
    ((UnityEvent) this.button.onClick).RemoveListener(pAction);
    ((UnityEvent) this.button.onClick).AddListener(pAction);
    this.locus_index = pLocusIndex;
    this.disableTooltip();
  }

  protected override void fillTooltipData(GeneAsset pElement)
  {
    Tooltip.show((object) this, "gene", this.tooltipDataBuilder());
  }

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData() { gene = this.gene };
  }

  public void addGeneClickCallback(GeneAssetClickEvent pAction)
  {
    this._gene_asset_click_event += pAction;
  }

  public void removeGeneClickCallback(GeneAssetClickEvent pAction)
  {
    this._gene_asset_click_event -= pAction;
  }
}
