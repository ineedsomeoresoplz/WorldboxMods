// Decompiled with JetBrains decompiler
// Type: KingdomStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class KingdomStatsElement : KingdomElement, IStatsElement, IRefreshElement
{
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
    int num = this.\u003C\u003E1__state;
    KingdomStatsElement kingdomStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (kingdomStatsElement.kingdom == null || !kingdomStatsElement.kingdom.isAlive())
      return false;
    kingdomStatsElement._stats_icons.showGeneralIcons<Kingdom, KingdomData>(kingdomStatsElement.kingdom);
    // ISSUE: explicit non-virtual call
    __nonvirtual (kingdomStatsElement.setIconValue("i_population", (float) kingdomStatsElement.kingdom.getPopulationPeople(), new float?((float) kingdomStatsElement.kingdom.getPopulationTotalPossible()), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (kingdomStatsElement.setIconValue("i_army", (float) kingdomStatsElement.kingdom.countTotalWarriors(), new float?((float) kingdomStatsElement.kingdom.countWarriorsMax()), "", false, "", '/'));
    if (kingdomStatsElement.kingdom.countCities() > kingdomStatsElement.kingdom.getMaxCities())
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (kingdomStatsElement.setIconValue("i_cities", (float) kingdomStatsElement.kingdom.countCities(), new float?((float) kingdomStatsElement.kingdom.getMaxCities()), "#FB2C21", false, "", '/'));
    }
    else
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (kingdomStatsElement.setIconValue("i_cities", (float) kingdomStatsElement.kingdom.countCities(), new float?((float) kingdomStatsElement.kingdom.getMaxCities()), "", false, "", '/'));
    }
    // ISSUE: explicit non-virtual call
    __nonvirtual (kingdomStatsElement.setIconValue("i_territory", (float) kingdomStatsElement.kingdom.countZones(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (kingdomStatsElement.setIconValue("i_buildings", (float) kingdomStatsElement.kingdom.countBuildings(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (kingdomStatsElement.setIconValue("i_food", (float) kingdomStatsElement.kingdom.countTotalFood(), new float?(), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
