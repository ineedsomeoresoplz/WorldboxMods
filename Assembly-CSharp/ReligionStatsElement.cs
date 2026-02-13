// Decompiled with JetBrains decompiler
// Type: ReligionStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ReligionStatsElement : ReligionElement, IStatsElement, IRefreshElement
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
    ReligionStatsElement religionStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (religionStatsElement.religion == null || !religionStatsElement.religion.isAlive())
      return false;
    religionStatsElement._stats_icons.showGeneralIcons<Religion, ReligionData>(religionStatsElement.religion);
    // ISSUE: explicit non-virtual call
    __nonvirtual (religionStatsElement.setIconValue("i_kingdoms", (float) religionStatsElement.religion.countKingdoms(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (religionStatsElement.setIconValue("i_cities", (float) religionStatsElement.religion.countCities(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (religionStatsElement.setIconValue("i_books", (float) religionStatsElement.meta_object.books.count(), new float?(), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
