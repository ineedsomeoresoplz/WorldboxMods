// Decompiled with JetBrains decompiler
// Type: ArmyStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ArmyStatsElement : ArmyElement, IStatsElement, IRefreshElement
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
    ArmyStatsElement armyStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (armyStatsElement.army == null || !armyStatsElement.army.isAlive())
      return false;
    armyStatsElement._stats_icons.showGeneralIcons<Army, ArmyData>(armyStatsElement.army);
    // ISSUE: explicit non-virtual call
    __nonvirtual (armyStatsElement.setIconValue("i_army_size", (float) armyStatsElement.army.countUnits(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (armyStatsElement.setIconValue("i_kills", (float) armyStatsElement.army.getTotalKills(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (armyStatsElement.setIconValue("i_melee", (float) armyStatsElement.army.countMelee(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (armyStatsElement.setIconValue("i_range", (float) armyStatsElement.army.countRange(), new float?(), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
