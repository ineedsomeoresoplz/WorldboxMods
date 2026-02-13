// Decompiled with JetBrains decompiler
// Type: TraitTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class TraitTools
{
  public static bool hasOppositeTraits<TTrait>(this TTrait pTraitToCheck) where TTrait : BaseTrait<TTrait>
  {
    return pTraitToCheck.opposite_traits != null && pTraitToCheck.opposite_traits.Count > 0;
  }

  public static bool hasOppositeTrait<TTrait>(this TTrait pTraitToCheck, HashSet<TTrait> pTraits) where TTrait : BaseTrait<TTrait>
  {
    return pTraitToCheck.hasOppositeTraits<TTrait>() && pTraits.Overlaps((IEnumerable<TTrait>) pTraitToCheck.opposite_traits);
  }

  public static bool hasOppositeTrait(this ActorTrait pTraitToCheck, HashSet<ActorTrait> pTraits)
  {
    return pTraitToCheck.hasOppositeTraits<ActorTrait>() && pTraits.Overlaps((IEnumerable<ActorTrait>) pTraitToCheck.opposite_traits);
  }

  public static bool hasOppositeTrait(string pTraitID, HashSet<ActorTrait> pTraits)
  {
    return AssetManager.traits.get(pTraitID).hasOppositeTrait(pTraits);
  }

  public static void loadTraits(Actor pActor, List<string> pListTraitIDs)
  {
    if (pListTraitIDs == null || pListTraitIDs.Count == 0)
      return;
    pActor.clearTraitCache();
    pActor.traits.Clear();
    foreach (string pListTraitId in pListTraitIDs)
    {
      ActorTrait actorTrait = AssetManager.traits.get(pListTraitId);
      if (actorTrait != null)
        pActor.traits.Add(actorTrait);
    }
  }

  public static ActorTrait getNewRandomTrait(string pGroup, HashSet<ActorTrait> pTraits)
  {
    using (ListPool<ActorTrait> list = new ListPool<ActorTrait>())
    {
      foreach (ActorTrait pTraitToCheck in AssetManager.traits.list)
      {
        int rate = pTraitToCheck.getRate(pGroup);
        if (rate != 0 && !(pTraitToCheck.group_id != pGroup) && !pTraits.Contains(pTraitToCheck) && !pTraitToCheck.hasOppositeTrait(pTraits))
        {
          for (int index = 0; index < rate; ++index)
            list.Add(pTraitToCheck);
        }
      }
      if (list.Count != 0)
        return list.GetRandom<ActorTrait>();
      Debug.LogError((object) "No Trait Found? How possible? 2");
      return (ActorTrait) null;
    }
  }

  public static ActorTrait getMostUsedTraitFromPopulation(
    List<Actor> pUnits,
    HashSet<ActorTrait> pTraits,
    string pGroup)
  {
    Dictionary<ActorTrait, int> source = new Dictionary<ActorTrait, int>();
    foreach (Actor pUnit in pUnits)
    {
      if (pUnit.isAlive())
      {
        foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pUnit.getTraits())
        {
          if (!(trait.group_id != pGroup) && !pTraits.Contains(trait) && !trait.hasOppositeTrait(pTraits))
          {
            if (source.ContainsKey(trait))
              source[trait]++;
            else
              source[trait] = 1;
          }
        }
      }
    }
    if (source.Count == 0)
      return (ActorTrait) null;
    using (ListPool<ActorTrait> listPool = new ListPool<ActorTrait>(3))
    {
      foreach (KeyValuePair<ActorTrait, int> keyValuePair in (IEnumerable<KeyValuePair<ActorTrait, int>>) source.OrderByDescending<KeyValuePair<ActorTrait, int>, int>((Func<KeyValuePair<ActorTrait, int>, int>) (kv => kv.Value)))
      {
        listPool.Add(keyValuePair.Key);
        if (listPool.Count >= 3)
          break;
      }
      using (ListPool<ActorTrait> list = new ListPool<ActorTrait>())
      {
        ActorTrait actorTrait1 = listPool[0];
        for (int index = 0; index < actorTrait1.getRate(pGroup) * 2; ++index)
          list.Add(actorTrait1);
        if (listPool.Count > 1)
        {
          ActorTrait actorTrait2 = listPool[1];
          for (int index = 0; index < actorTrait2.getRate(pGroup); ++index)
            list.Add(actorTrait2);
        }
        if (listPool.Count > 2)
        {
          ActorTrait actorTrait3 = listPool[2];
          for (int index = 0; index < actorTrait3.getRate(pGroup); ++index)
            list.Add(actorTrait3);
        }
        return list.Count == 0 ? (ActorTrait) null : list.GetRandom<ActorTrait>();
      }
    }
  }

  public static void recalculateTraitBonuses(
    BaseStats pBaseStats,
    HashSet<ActorTrait> pTraits,
    bool pClear = true)
  {
    if (pClear)
      pBaseStats.clear();
    foreach (ActorTrait pTrait in pTraits)
      pBaseStats.mergeStats(pTrait.base_stats);
  }
}
