// Decompiled with JetBrains decompiler
// Type: PlotListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlotListElement : WindowListElementBase<Plot, PlotData>
{
  [SerializeField]
  private Text _text_name;
  [SerializeField]
  private CountUpOnClick _members;
  [SerializeField]
  private CountUpOnClick _age;
  [SerializeField]
  private CountUpOnClick _progress;
  [SerializeField]
  private UiUnitAvatarElement _avatar_loader;
  [SerializeField]
  private StatBar _bar;
  [SerializeField]
  private GameObject _locked_effect;

  internal override void show(Plot pPlot)
  {
    base.show(pPlot);
    Actor author = pPlot.getAuthor();
    this._avatar_loader.show(author);
    ColorAsset colorAsset = (ColorAsset) null;
    if (author != null)
      colorAsset = author.kingdom.getColor();
    if (colorAsset != null)
      ((Graphic) this._text_name).color = author.kingdom.getColor().getColorText();
    else
      ((Graphic) this._text_name).color = Toolbox.color_white;
    this._text_name.text = pPlot.data.name;
    this._members.setValue(pPlot.getSupporters());
    this._progress.setValue((int) pPlot.getProgress(), "/" + pPlot.getProgressMax().ToText());
    float progress = pPlot.getProgress();
    float progressMax = pPlot.getProgressMax();
    this._bar.setBar(progress, progressMax, "/" + progressMax.ToText(), pFloat: true);
    this._age.setValue(pPlot.getAge());
    if (pPlot.getAsset().isAvailable())
      this._locked_effect.gameObject.SetActive(false);
    else
      this._locked_effect.gameObject.SetActive(true);
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "plot", new TooltipData()
    {
      plot = this.meta_object
    });
  }
}
