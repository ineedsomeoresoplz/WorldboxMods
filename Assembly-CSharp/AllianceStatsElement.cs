// Decompiled with JetBrains decompiler
// Type: AllianceStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class AllianceStatsElement : AllianceElement, IStatsElement, IRefreshElement
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
    AllianceStatsElement allianceStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (allianceStatsElement.alliance == null || !allianceStatsElement.alliance.isAlive())
      return false;
    allianceStatsElement._stats_icons.showGeneralIcons<Alliance, AllianceData>(allianceStatsElement.alliance);
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_population", (float) allianceStatsElement.alliance.countPopulation(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_army", (float) allianceStatsElement.alliance.countWarriors(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_kingdoms", (float) allianceStatsElement.alliance.countKingdoms(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_cities", (float) allianceStatsElement.alliance.countCities(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_buildings", (float) allianceStatsElement.alliance.countBuildings(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (allianceStatsElement.setIconValue("i_zones", (float) allianceStatsElement.alliance.countZones(), new float?(), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
