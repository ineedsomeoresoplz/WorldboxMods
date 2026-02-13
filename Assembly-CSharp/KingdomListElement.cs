// Decompiled with JetBrains decompiler
// Type: KingdomListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KingdomListElement : WindowListElementBase<Kingdom, KingdomData>
{
  public CountUpOnClick textAge;
  public CountUpOnClick textPopulation;
  public CountUpOnClick textArmy;
  public CountUpOnClick textCities;
  public CountUpOnClick textHouses;
  public CountUpOnClick textZones;
  public Text kingdomName;
  public GameObject buttonCapital;
  public GameObject buttonKing;
  public UiUnitAvatarElement avatarLoader;

  internal override void show(Kingdom pKingdom)
  {
    base.show(pKingdom);
    this.kingdomName.text = pKingdom.name;
    ((Graphic) this.kingdomName).color = pKingdom.getColor().getColorText();
    this.avatarLoader.show(pKingdom.king);
    int pValue1 = 0;
    int pValue2 = 0;
    int pValue3 = 0;
    foreach (City city in pKingdom.getCities())
    {
      ++pValue3;
      pValue1 += city.zones.Count;
      pValue2 += city.buildings.Count;
    }
    this.textPopulation.setValue(pKingdom.getPopulationPeople());
    this.textArmy.setValue(pKingdom.countTotalWarriors());
    this.textZones.setValue(pValue1);
    this.textHouses.setValue(pValue2);
    this.textCities.setValue(pValue3, "/" + pKingdom.getMaxCities().ToString());
    this.textAge.setValue(pKingdom.getAge());
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "kingdom", new TooltipData()
    {
      kingdom = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
