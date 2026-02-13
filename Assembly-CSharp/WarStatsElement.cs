// Decompiled with JetBrains decompiler
// Type: WarStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class WarStatsElement : WarElement, IStatsElement, IRefreshElement
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
    WarStatsElement warStatsElement1 = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (warStatsElement1.war == null || !warStatsElement1.war.isAlive())
      return false;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_age", (float) warStatsElement1.war.getAge(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_population", (float) warStatsElement1.war.countTotalPopulation(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_total_army", (float) warStatsElement1.war.countTotalArmy(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_kingdoms", (float) warStatsElement1.war.countKingdoms(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_cities", (float) warStatsElement1.war.countCities(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement1.setIconValue("i_deaths", (float) warStatsElement1.war.getTotalDeaths(), new float?(), "", false, "", '/'));
    bool flag1 = warStatsElement1.war.countAttackersPopulation() > warStatsElement1.war.countDefendersPopulation();
    bool flag2 = warStatsElement1.war.getDeadDefenders() > warStatsElement1.war.getDeadAttackers();
    bool flag3 = warStatsElement1.war.countAttackersWarriors() > warStatsElement1.war.countDefendersWarriors();
    bool flag4 = warStatsElement1.war.countAttackersCities() > warStatsElement1.war.countDefendersCities();
    WarStatsElement warStatsElement2 = warStatsElement1;
    double pMainVal1 = (double) warStatsElement1.war.countAttackersPopulation();
    string str1 = flag1 ? "#43FF43" : "#FB2C21";
    float? pMax1 = new float?();
    string pColor1 = str1;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement2.setIconValue("i_attackers_population", (float) pMainVal1, pMax1, pColor1, false, "", '/'));
    WarStatsElement warStatsElement3 = warStatsElement1;
    double pMainVal2 = (double) warStatsElement1.war.countAttackersWarriors();
    string str2 = flag3 ? "#43FF43" : "#FB2C21";
    float? pMax2 = new float?();
    string pColor2 = str2;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement3.setIconValue("i_attackers_army", (float) pMainVal2, pMax2, pColor2, false, "", '/'));
    WarStatsElement warStatsElement4 = warStatsElement1;
    double deadAttackers = (double) warStatsElement1.war.getDeadAttackers();
    string str3 = flag2 ? "#43FF43" : "#FB2C21";
    float? pMax3 = new float?();
    string pColor3 = str3;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement4.setIconValue("i_attackers_dead", (float) deadAttackers, pMax3, pColor3, false, "", '/'));
    WarStatsElement warStatsElement5 = warStatsElement1;
    double pMainVal3 = (double) warStatsElement1.war.countAttackersCities();
    string str4 = flag4 ? "#43FF43" : "#FB2C21";
    float? pMax4 = new float?();
    string pColor4 = str4;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement5.setIconValue("i_attackers_cities", (float) pMainVal3, pMax4, pColor4, false, "", '/'));
    WarStatsElement warStatsElement6 = warStatsElement1;
    double pMainVal4 = (double) warStatsElement1.war.countDefendersPopulation();
    string str5 = flag1 ? "#FB2C21" : "#43FF43";
    float? pMax5 = new float?();
    string pColor5 = str5;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement6.setIconValue("i_defenders_population", (float) pMainVal4, pMax5, pColor5, false, "", '/'));
    WarStatsElement warStatsElement7 = warStatsElement1;
    double pMainVal5 = (double) warStatsElement1.war.countDefendersWarriors();
    string str6 = flag3 ? "#FB2C21" : "#43FF43";
    float? pMax6 = new float?();
    string pColor6 = str6;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement7.setIconValue("i_defenders_army", (float) pMainVal5, pMax6, pColor6, false, "", '/'));
    WarStatsElement warStatsElement8 = warStatsElement1;
    double deadDefenders = (double) warStatsElement1.war.getDeadDefenders();
    string str7 = flag2 ? "#FB2C21" : "#43FF43";
    float? pMax7 = new float?();
    string pColor7 = str7;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement8.setIconValue("i_defenders_dead", (float) deadDefenders, pMax7, pColor7, false, "", '/'));
    WarStatsElement warStatsElement9 = warStatsElement1;
    double pMainVal6 = (double) warStatsElement1.war.countDefendersCities();
    string str8 = flag4 ? "#FB2C21" : "#43FF43";
    float? pMax8 = new float?();
    string pColor8 = str8;
    // ISSUE: explicit non-virtual call
    __nonvirtual (warStatsElement9.setIconValue("i_defenders_cities", (float) pMainVal6, pMax8, pColor8, false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
