// Decompiled with JetBrains decompiler
// Type: FamilyListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FamilyListElement : WindowListElementBase<Family, FamilyData>
{
  public Text text_name;
  public CountUpOnClick text_age;
  public CountUpOnClick text_population;
  public CountUpOnClick text_adults;
  public CountUpOnClick text_children;
  public CountUpOnClick text_dead;
  [SerializeField]
  private Text _collective_term;

  internal override void show(Family pFamily)
  {
    base.show(pFamily);
    this.text_name.text = pFamily.name;
    ((Graphic) this.text_name).color = pFamily.getColor().getColorText();
    this.text_age.setValue(pFamily.getAge());
    this.text_population.setValue(pFamily.countUnits());
    this.text_adults.setValue(pFamily.countAdults());
    this.text_children.setValue(pFamily.countChildren());
    this.text_dead.setValue((int) pFamily.getTotalDeaths());
    this._collective_term.text = LocalizedTextManager.getText(pFamily.getActorAsset().getCollectiveTermID());
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "family", new TooltipData()
    {
      family = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
