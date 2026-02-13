// Decompiled with JetBrains decompiler
// Type: ReligionListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class ReligionListElement : WindowListElementBase<Religion, ReligionData>
{
  public Text text_name;
  public CountUpOnClick text_age;
  public CountUpOnClick text_population;
  public CountUpOnClick text_renown;
  public CountUpOnClick text_villages;
  public CountUpOnClick text_kingdom;

  internal override void show(Religion pReligion)
  {
    base.show(pReligion);
    this.text_name.text = pReligion.name;
    ((Graphic) this.text_name).color = pReligion.getColor().getColorText();
    this.text_age.setValue(pReligion.getAge());
    this.text_population.setValue(pReligion.countUnits());
    this.text_villages.setValue(pReligion.countCities());
    this.text_kingdom.setValue(pReligion.countKingdoms());
    this.text_renown.setValue(pReligion.getRenown());
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "religion", new TooltipData()
    {
      religion = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
