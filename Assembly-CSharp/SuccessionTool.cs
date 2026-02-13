// Decompiled with JetBrains decompiler
// Type: SuccessionTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class SuccessionTool
{
  public static Actor findNextHeir(Kingdom pKingdom, Actor pExculdeActor = null)
  {
    return SuccessionTool.getKingFromRoyalClan(pKingdom, pExculdeActor);
  }

  public static Actor getKingFromRoyalClan(Kingdom pKingdom, Actor pExcludeActor = null)
  {
    if (!pKingdom.data.royal_clan_id.hasValue())
      return (Actor) null;
    Clan clan = World.world.clans.get(pKingdom.data.royal_clan_id);
    if (clan == null)
      return (Actor) null;
    List<Actor> units = clan.units;
    using (ListPool<Actor> pUnits = new ListPool<Actor>())
    {
      for (int index = 0; index < units.Count; ++index)
      {
        Actor pActor = units[index];
        if (pActor != pExcludeActor && clan.fitToRule(pActor, pKingdom))
          pUnits.Add(pActor);
      }
      if (pUnits.Count == 0)
        return (Actor) null;
      if (pKingdom.hasCulture())
        return ListSorters.getUnitSortedByAgeAndTraits(pUnits, pKingdom.culture);
      pUnits.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeOldFirst));
      return pUnits[0];
    }
  }

  public static Actor getKingFromLeaders(Kingdom pKingdom)
  {
    // ISSUE: unable to decompile the method.
  }
}
