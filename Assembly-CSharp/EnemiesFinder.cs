// Decompiled with JetBrains decompiler
// Type: EnemiesFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.Pool;

#nullable disable
public static class EnemiesFinder
{
  private static Dictionary<Kingdom, EnemyFinderContainer> _cache_data = new Dictionary<Kingdom, EnemyFinderContainer>();
  public static int counter_reused = 0;

  private static EnemyFinderContainer getCacheContainer(Kingdom pKingdom)
  {
    EnemyFinderContainer cacheContainer;
    if (!EnemiesFinder._cache_data.TryGetValue(pKingdom, out cacheContainer))
    {
      cacheContainer = UnsafeGenericPool<EnemyFinderContainer>.Get();
      cacheContainer.setKingdom(pKingdom);
      EnemiesFinder._cache_data.Add(pKingdom, cacheContainer);
    }
    return cacheContainer;
  }

  internal static EnemyFinderData findEnemiesFrom(
    WorldTile pTile,
    Kingdom pKingdom,
    int pChunkRange = -1)
  {
    if (pChunkRange == -1)
      pChunkRange = SimGlobals.m.unit_chunk_sight_range;
    return EnemiesFinder.getCacheContainer(pKingdom).getData(pTile.chunk, pChunkRange);
  }

  public static void clear()
  {
    EnemiesFinder.counter_reused = 0;
    foreach (EnemyFinderContainer enemyFinderContainer in EnemiesFinder._cache_data.Values)
    {
      enemyFinderContainer.clear();
      UnsafeGenericPool<EnemyFinderContainer>.Release(enemyFinderContainer);
    }
    EnemiesFinder._cache_data.Clear();
  }

  public static void disposeAll()
  {
    foreach (EnemyFinderContainer enemyFinderContainer in EnemiesFinder._cache_data.Values)
      enemyFinderContainer.disposeAll();
    EnemiesFinder.clear();
  }
}
