// Decompiled with JetBrains decompiler
// Type: ClanStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ClanStatsElement : ClanElement, IStatsElement, IRefreshElement
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
    ClanStatsElement clanStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (clanStatsElement.clan == null || !clanStatsElement.clan.isAlive())
      return false;
    clanStatsElement._stats_icons.showGeneralIcons<Clan, ClanData>(clanStatsElement.clan);
    // ISSUE: explicit non-virtual call
    __nonvirtual (clanStatsElement.setIconValue("i_books_written", (float) clanStatsElement.clan.data.books_written, new float?(), "", false, "", '/'));
    return false;
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
