// Decompiled with JetBrains decompiler
// Type: CityStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CityStatsElement : CityElement, IStatsElement, IRefreshElement
{
  [SerializeField]
  private CityLoyaltyElement _loyalty_element;
  private StatsIconContainer _stats_icons;

  public void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    this._stats_icons.setIconValue(pName, pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  protected override void Awake()
  {
    this._stats_icons = ((Component) this).gameObject.AddOrGetComponent<StatsIconContainer>();
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    CityStatsElement cityStatsElement1 = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (cityStatsElement1.city == null || !cityStatsElement1.city.isAlive())
      return false;
    int populationPeople = cityStatsElement1.city.getPopulationPeople();
    int num2 = cityStatsElement1.city.countFoodTotal();
    cityStatsElement1._stats_icons.showGeneralIcons<City, CityData>(cityStatsElement1.city);
    if (populationPeople > cityStatsElement1.city.getPopulationMaximum())
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_population", (float) populationPeople, new float?((float) cityStatsElement1.city.getPopulationMaximum()), "#FB2C21", false, "", '/'));
    }
    else
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_population", (float) populationPeople, new float?((float) cityStatsElement1.city.getPopulationMaximum()), "", false, "", '/'));
    }
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement1.setIconValue("i_territory", (float) cityStatsElement1.city.countZones(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement1.setIconValue("i_boats", (float) cityStatsElement1.city.countBoats(), new float?(), "", false, "", '/'));
    CityStatsElement cityStatsElement2 = cityStatsElement1;
    double pMainVal = (double) num2;
    string str = num2 > populationPeople * 4 ? "#43FF43" : "#FB2C21";
    float? pMax = new float?();
    string pColor = str;
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement2.setIconValue("i_food", (float) pMainVal, pMax, pColor, false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement1.setIconValue("i_farmers", (float) cityStatsElement1.city.jobs.countOccupied(CitizenJobLibrary.farmer), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement1.setIconValue("i_books", (float) cityStatsElement1.city.countBooks(), new float?(), "", false, "", '/'));
    int loyalty = cityStatsElement1.city.getLoyalty(true);
    if (loyalty > 0)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_loyalty", (float) loyalty, new float?(), "#43FF43", false, "", '/'));
    }
    else
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_loyalty", (float) loyalty, new float?(), "#FB2C21", false, "", '/'));
    }
    cityStatsElement1._loyalty_element.setCity(cityStatsElement1.city);
    if (WorldLawLibrary.world_law_civ_army.isEnabled())
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_army", (float) cityStatsElement1.city.countWarriors(), new float?((float) cityStatsElement1.city.getMaxWarriors()), "", false, "", '/'));
    }
    else
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (cityStatsElement1.setIconValue("i_army", (float) cityStatsElement1.city.countWarriors(), new float?(), "", false, "", '/'));
    }
    // ISSUE: explicit non-virtual call
    __nonvirtual (cityStatsElement1.setIconValue("i_houses", (float) cityStatsElement1.city.getHouseCurrent(), new float?((float) cityStatsElement1.city.getHouseLimit()), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
