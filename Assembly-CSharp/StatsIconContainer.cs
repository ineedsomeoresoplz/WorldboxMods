// Decompiled with JetBrains decompiler
// Type: StatsIconContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StatsIconContainer : MonoBehaviour
{
  private Dictionary<string, StatsIcon> _stats_icons = new Dictionary<string, StatsIcon>();

  protected void Awake()
  {
    foreach (StatsIcon componentsInChild in ((Component) this).GetComponentsInChildren<StatsIcon>(true))
    {
      if (!this._stats_icons.TryAdd(((Object) componentsInChild).name, componentsInChild))
        Debug.LogError((object) ("Duplicate icon name! " + ((Object) componentsInChild).name));
    }
    this.clear();
  }

  protected void OnDisable() => this.clear();

  public bool TryGetValue(string pName, out StatsIcon pIcon)
  {
    return this._stats_icons.TryGetValue(pName, out pIcon);
  }

  public void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    StatsIcon iconViaId = this.getIconViaId(pName);
    if (Object.op_Equality((Object) iconViaId, (Object) null) || iconViaId.areValuesTooClose(pMainVal))
      return;
    iconViaId.setValue(pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
    iconViaId.textScaleAnimation();
  }

  public void setText(string pName, string pText, string pColor)
  {
    StatsIcon iconViaId = this.getIconViaId(pName);
    if (Object.op_Equality((Object) iconViaId, (Object) null))
      return;
    iconViaId.enable_animation = false;
    iconViaId.checkDestroyTween();
    iconViaId.text.text = pText;
    ((Graphic) iconViaId.text).color = Toolbox.makeColor(pColor);
  }

  public StatsIcon getIconViaId(string pName)
  {
    StatsIcon iconViaId;
    this._stats_icons.TryGetValue(pName, out iconViaId);
    if (Object.op_Equality((Object) iconViaId, (Object) null))
      return (StatsIcon) null;
    ((Component) iconViaId).gameObject.SetActive(true);
    return iconViaId;
  }

  protected void clear()
  {
    foreach (Component component in this._stats_icons.Values)
      component.gameObject.SetActive(false);
  }

  public void showGeneralIcons<TMetaObject, TData>(TMetaObject pMetaObject)
    where TMetaObject : MetaObject<TData>, IMetaObject
    where TData : MetaObjectData
  {
    int num1 = pMetaObject.countHoused();
    int num2 = pMetaObject.countHomeless();
    this.setIconValue("i_renown", (float) pMetaObject.getRenown());
    this.setIconValue("i_age", (float) pMetaObject.getAge());
    this.setIconValue("i_births", (float) pMetaObject.getTotalBirths());
    this.setIconValue("i_kings", (float) pMetaObject.countKings());
    this.setIconValue("i_leaders", (float) pMetaObject.countLeaders());
    this.setIconValue("i_population", (float) pMetaObject.countUnits());
    this.setIconValue("i_members", (float) pMetaObject.countUnits());
    this.setIconValue("i_total_money", (float) pMetaObject.countTotalMoney());
    this.setIconValue("i_adults", (float) pMetaObject.countAdults());
    double pMainVal1 = (double) pMetaObject.countChildren();
    string str1 = (double) pMetaObject.getRatioChildren() > 0.5 ? "#43FF43" : string.Empty;
    float? pMax1 = new float?();
    string pColor1 = str1;
    this.setIconValue("i_children", (float) pMainVal1, pMax1, pColor1);
    double pMainVal2 = (double) pMetaObject.countMales();
    string str2 = (double) pMetaObject.getRatioMales() > 0.699999988079071 ? "#43FF43" : string.Empty;
    float? pMax2 = new float?();
    string pColor2 = str2;
    this.setIconValue("i_males", (float) pMainVal2, pMax2, pColor2);
    double pMainVal3 = (double) pMetaObject.countFemales();
    string str3 = (double) pMetaObject.getRatioFemales() > 0.699999988079071 ? "#43FF43" : string.Empty;
    float? pMax3 = new float?();
    string pColor3 = str3;
    this.setIconValue("i_females", (float) pMainVal3, pMax3, pColor3);
    this.setIconValue("i_single_females", (float) pMetaObject.countSingleFemales());
    this.setIconValue("i_single_males", (float) pMetaObject.countSingleMales());
    this.setIconValue("i_families", (float) pMetaObject.countFamilies());
    this.setIconValue("i_couples", (float) pMetaObject.countCouples());
    this.setIconValue("i_deaths", (float) pMetaObject.getTotalDeaths());
    double pMainVal4 = (double) pMetaObject.countHappyUnits();
    string str4 = (double) pMetaObject.getRatioHappy() > 0.699999988079071 ? "#43FF43" : string.Empty;
    float? pMax4 = new float?();
    string pColor4 = str4;
    this.setIconValue("i_happy_units", (float) pMainVal4, pMax4, pColor4);
    double pMainVal5 = (double) pMetaObject.countUnhappyUnits();
    string str5 = (double) pMetaObject.getRatioUnhappy() > 0.40000000596046448 ? "#FB2C21" : string.Empty;
    float? pMax5 = new float?();
    string pColor5 = str5;
    this.setIconValue("i_unhappy_units", (float) pMainVal5, pMax5, pColor5);
    double pMainVal6 = (double) pMetaObject.countSick();
    string str6 = (double) pMetaObject.getRatioSick() > 0.30000001192092896 ? "#FB2C21" : string.Empty;
    float? pMax6 = new float?();
    string pColor6 = str6;
    this.setIconValue("i_sick", (float) pMainVal6, pMax6, pColor6);
    double pMainVal7 = (double) pMetaObject.countHungry();
    string str7 = (double) pMetaObject.getRatioHungry() > 0.5 ? "#FB2C21" : string.Empty;
    float? pMax7 = new float?();
    string pColor7 = str7;
    this.setIconValue("i_hungry", (float) pMainVal7, pMax7, pColor7);
    double pMainVal8 = (double) pMetaObject.countStarving();
    string str8 = (double) pMetaObject.getRatioStarving() > 0.30000001192092896 ? "#FB2C21" : string.Empty;
    float? pMax8 = new float?();
    string pColor8 = str8;
    this.setIconValue("i_starving", (float) pMainVal8, pMax8, pColor8);
    double pMainVal9 = (double) num1;
    string str9 = (double) pMetaObject.getRatioHoused() > 0.800000011920929 ? "#43FF43" : string.Empty;
    float? pMax9 = new float?();
    string pColor9 = str9;
    this.setIconValue("i_housed", (float) pMainVal9, pMax9, pColor9);
    double pMainVal10 = (double) num2;
    string str10 = (double) pMetaObject.getRatioHomeless() > 0.30000001192092896 ? "#FB2C21" : string.Empty;
    float? pMax10 = new float?();
    string pColor10 = str10;
    this.setIconValue("i_homeless", (float) pMainVal10, pMax10, pColor10);
    this.setIconValue("i_kills", (float) pMetaObject.getTotalKills());
  }
}
