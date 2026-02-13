// Decompiled with JetBrains decompiler
// Type: SubspeciesListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SubspeciesListElement : WindowListElementBase<Subspecies, SubspeciesData>
{
  public Text text_name;
  public CountUpOnClick text_age;
  public CountUpOnClick text_population;
  public CountUpOnClick text_children;
  public CountUpOnClick text_deaths;
  public CountUpOnClick text_family;
  [SerializeField]
  private Text _subspecies_name;

  internal override void show(Subspecies pSubspecies)
  {
    base.show(pSubspecies);
    this.text_name.text = pSubspecies.name;
    ((Graphic) this.text_name).color = pSubspecies.getColor().getColorText();
    this.text_age.setValue(pSubspecies.getAge());
    this.text_population.setValue(pSubspecies.countUnits());
    this.text_deaths.setValue((int) pSubspecies.getTotalDeaths());
    this.text_children.setValue(pSubspecies.countChildren());
    this.text_family.setValue(pSubspecies.countCurrentFamilies());
    this._subspecies_name.text = pSubspecies.getActorAsset().getTranslatedName();
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "subspecies", new TooltipData()
    {
      subspecies = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
