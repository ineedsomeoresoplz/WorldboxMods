// Decompiled with JetBrains decompiler
// Type: BehGenerateLootFromHouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehGenerateLootFromHouse : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasHouse())
      return BehResult.Stop;
    Building homeBuilding = pActor.getHomeBuilding();
    int lootGeneration = homeBuilding.asset.loot_generation;
    int num = 0;
    BiomeAsset biome = homeBuilding.current_tile.getBiome();
    if (biome != null)
      num = biome.loot_generation;
    int pLootValue = Mathf.Max(1, lootGeneration + num);
    pActor.addLoot(pLootValue);
    return BehResult.Continue;
  }
}
