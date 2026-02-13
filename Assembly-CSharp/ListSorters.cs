// Decompiled with JetBrains decompiler
// Type: ListSorters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class ListSorters
{
  public static int sortUnitByAgeOldFirst(Actor pActor1, Actor pActor2)
  {
    return -pActor2.data.created_time.CompareTo(pActor1.data.created_time);
  }

  public static int sortUnitByAgeYoungFirst(Actor pActor1, Actor pActor2)
  {
    return pActor2.data.created_time.CompareTo(pActor1.data.created_time);
  }

  public static int sortUnitByKills(Actor pActor1, Actor pActor2)
  {
    return -pActor1.data.kills.CompareTo(pActor2.data.kills);
  }

  public static int sortUnitByRenown(Actor pActor1, Actor pActor2)
  {
    return -pActor1.data.renown.CompareTo(pActor2.data.renown);
  }

  public static int sortUnitByGoldCoins(Actor pActor1, Actor pActor2)
  {
    return -pActor1.data.money.CompareTo(pActor2.data.money);
  }

  public static int sortUnitByGender(Actor pActor1, Actor pActor2, ActorSex pTopGender)
  {
    if (pActor1.data.sex == pActor2.data.sex)
      return 0;
    return pActor1.data.sex == pTopGender ? -1 : 1;
  }

  public static int sortUnitByStats(Actor pActor1, Actor pActor2, string pStatId)
  {
    return -pActor1.stats.get(pStatId).CompareTo(pActor2.stats.get(pStatId));
  }

  public static Actor getUnitSortedByAgeAndTraits(ListPool<Actor> pUnits, Culture pCulture)
  {
    ListSorters.sortUnitsSortedByAgeAndTraits(pUnits, pCulture);
    return pUnits[0];
  }

  public static void sortUnitsSortedByAgeAndTraits(ListPool<Actor> pUnits, Culture pCulture)
  {
    if (pCulture == null)
    {
      pUnits.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeOldFirst));
    }
    else
    {
      if (pCulture.hasTrait("ultimogeniture"))
        pUnits.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeYoungFirst));
      else
        pUnits.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeOldFirst));
      int num = pCulture.hasTrait("diplomatic_ascension") ? 1 : 0;
      bool flag1 = pCulture.hasTrait("warriors_ascension");
      bool flag2 = pCulture.hasTrait("golden_rule");
      bool flag3 = pCulture.hasTrait("fames_crown");
      if (num != 0)
        pUnits.Sort((Comparison<Actor>) ((a1, a2) => ListSorters.sortUnitByStats(a1, a2, "diplomacy")));
      else if (flag1)
        pUnits.Sort((Comparison<Actor>) ((a1, a2) => ListSorters.sortUnitByStats(a1, a2, "warfare")));
      else if (flag3)
        pUnits.Sort((Comparison<Actor>) ((a1, a2) => ListSorters.sortUnitByRenown(a1, a2)));
      else if (flag2)
        pUnits.Sort((Comparison<Actor>) ((a1, a2) => ListSorters.sortUnitByGoldCoins(a1, a2)));
      bool flag4 = pCulture.hasTrait("patriarchy");
      bool flag5 = pCulture.hasTrait("matriarchy");
      if (!(flag4 | flag5))
        return;
      ActorSex tSex = flag4 ? ActorSex.Male : ActorSex.Female;
      pUnits.Sort((Comparison<Actor>) ((a1, a2) => ListSorters.sortUnitByGender(a1, a2, tSex)));
    }
  }
}
