// Decompiled with JetBrains decompiler
// Type: ChromosomeElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ChromosomeElement : MonoBehaviour
{
  private static readonly Color color_synergy_gold = Toolbox.makeColor("#FFF841");
  private static readonly Color color_normal_blue = Toolbox.makeColor("#00B0FF");
  internal Chromosome chromosome;
  private ChromosomeClickEvent _click_event;
  public Image image;

  private void Start()
  {
    this.setupTooltip();
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(clickChromosome)));
    DraggableLayoutElement draggableLayoutElement;
    if (!((Component) this).TryGetComponent<DraggableLayoutElement>(ref draggableLayoutElement))
      return;
    draggableLayoutElement.start_being_dragged += new Action<DraggableLayoutElement>(this.onStartDrag);
  }

  protected virtual void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    this.show(((Component) pOriginalElement).GetComponent<ChromosomeElement>().chromosome, (ChromosomeClickEvent) null);
  }

  private void clickChromosome()
  {
    ChromosomeClickEvent clickEvent = this._click_event;
    if (clickEvent == null)
      return;
    clickEvent(this.chromosome);
  }

  public void show(Chromosome pChromosome, ChromosomeClickEvent pClickEvent)
  {
    this.chromosome = pChromosome;
    this._click_event = pClickEvent;
    if (pChromosome.isAllLociSynergy())
      this.image.sprite = this.chromosome.getSpriteGolden();
    else
      this.image.sprite = this.chromosome.getSpriteNormal();
  }

  protected virtual void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction(new TooltipAction(this.tooltipAction));
  }

  protected void tooltipAction()
  {
    Tooltip.show((object) this, "chromosome", new TooltipData()
    {
      chromosome = this.chromosome
    });
  }
}
