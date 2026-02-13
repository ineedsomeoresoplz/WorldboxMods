// Decompiled with JetBrains decompiler
// Type: EnemyFinderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Pool;

#nullable disable
public class EnemyFinderContainer
{
  public Dictionary<int, EnemyFinderData> dict_data = new Dictionary<int, EnemyFinderData>((int) Math.Pow(9.0, (double) SimGlobals.m.unit_chunk_sight_range));
  private Kingdom _kingdom;

  public void setKingdom(Kingdom pKingdom) => this._kingdom = pKingdom;

  public EnemyFinderData getData(MapChunk pChunk, int pRange)
  {
    int key = pChunk.id * 10000 + pRange;
    EnemyFinderData data;
    if (!this.dict_data.TryGetValue(key, out data))
    {
      EnemyFinderData pData = UnsafeGenericPool<EnemyFinderData>.Get();
      this.dict_data.Add(key, pData);
      if (!this._kingdom.asset.force_look_all_chunks)
      {
        if (pRange == 0)
        {
          EnemyFinderContainer.findEnemiesOfKingdomInChunk(pData, pChunk, this._kingdom);
          return pData;
        }
        if (Randy.randomChance(0.8f))
          EnemyFinderContainer.findEnemiesOfKingdomInChunk(pData, pChunk, this._kingdom);
      }
      if (pData.isEmpty())
      {
        for (int index = 0; index <= pRange; ++index)
        {
          this.checkRange(pData, pChunk, index, index);
          if (!pData.isEmpty() && !this._kingdom.asset.force_look_all_chunks)
            break;
        }
      }
      return pData;
    }
    ++EnemiesFinder.counter_reused;
    return data;
  }

  private void checkRange(EnemyFinderData pData, MapChunk pChunk, int pRange, int pSkipLessThan = -1)
  {
    if (pRange == 0)
    {
      EnemyFinderContainer.findEnemiesOfKingdomInChunk(pData, pChunk, this._kingdom);
    }
    else
    {
      int x = pChunk.x;
      int y = pChunk.y;
      bool flag = pSkipLessThan > 0;
      int num1 = pSkipLessThan * -1;
      int num2 = pSkipLessThan;
      for (int index1 = -pRange; index1 <= pRange; ++index1)
      {
        for (int index2 = -pRange; index2 <= pRange; ++index2)
        {
          if (!flag || index1 <= num1 || index1 >= num2 || index2 <= num1 || index2 >= num2)
          {
            MapChunk pChunk1 = World.world.map_chunk_manager.get(x + index1, y + index2);
            if (pChunk1 != null)
              EnemyFinderContainer.findEnemiesOfKingdomInChunk(pData, pChunk1, this._kingdom);
          }
        }
      }
    }
  }

  private static void findEnemiesOfKingdomInChunk(
    EnemyFinderData pData,
    MapChunk pChunk,
    Kingdom pMainKingdom)
  {
    if (pChunk.objects.kingdoms.Count == 0)
      return;
    List<long> kingdoms = pChunk.objects.kingdoms;
    bool flag = WorldLawLibrary.world_law_peaceful_monsters.isEnabled();
    if (pMainKingdom.asset.mobs & flag)
      return;
    for (int index = 0; index < kingdoms.Count; ++index)
    {
      long num = kingdoms[index];
      Kingdom civOrWildViaId = World.world.kingdoms.getCivOrWildViaID(num);
      if (civOrWildViaId != null && (!flag || !civOrWildViaId.asset.mobs) && pMainKingdom.isEnemy(civOrWildViaId))
      {
        pData.addEnemyList(pChunk.objects.getUnits(num));
        pData.addEnemyList(pChunk.objects.getBuildings(num));
      }
    }
  }

  public void clear()
  {
    foreach (EnemyFinderData enemyFinderData in this.dict_data.Values)
    {
      enemyFinderData.reset();
      UnsafeGenericPool<EnemyFinderData>.Release(enemyFinderData);
    }
    this.dict_data.Clear();
  }

  public void disposeAll()
  {
    foreach (EnemyFinderData enemyFinderData in this.dict_data.Values)
      enemyFinderData.reset();
    this._kingdom = (Kingdom) null;
  }
}
