// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerDrawToTileTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
namespace ai.behaviours;

public class BehFingerDrawToTileTarget : BehFingerDrawAction
{
  public BehFingerDrawToTileTarget() => this.drawing_action = true;

  public override BehResult execute(Actor pActor)
  {
    BehFingerDrawToTileTarget.pickBrush(this.finger);
    BehFingerDrawToTileTarget.pickPower(this.finger);
    using (ListPool<WorldTile> pTiles = new ListPool<WorldTile>((ICollection<WorldTile>) this.finger.target_tiles))
    {
      ExecuteEvent curved;
      if (pActor.current_tile == pActor.beh_tile_target || (double) Toolbox.DistTile(pActor.current_tile, pActor.beh_tile_target) < 6.0)
        curved = ActorMove.goToCurved(pActor, pActor.current_tile, pActor.current_tile.neighboursAll.GetRandom<WorldTile>(), pActor.current_tile.neighboursAll.GetRandom<WorldTile>(), pActor.beh_tile_target.neighboursAll.GetRandom<WorldTile>(), pActor.beh_tile_target.neighboursAll.GetRandom<WorldTile>(), pActor.beh_tile_target);
      else if (this.finger.target_tiles.Count > 10)
      {
        WorldTile tileWithinDistance1 = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 25, pTiles);
        WorldTile tileWithinDistance2 = Toolbox.getRandomTileWithinDistance(tileWithinDistance1, 25, pTiles);
        WorldTile tileWithinDistance3 = Toolbox.getRandomTileWithinDistance(pActor.beh_tile_target, 25, pTiles);
        WorldTile tileWithinDistance4 = Toolbox.getRandomTileWithinDistance(tileWithinDistance3, 25, pTiles);
        curved = ActorMove.goToCurved(pActor, pActor.current_tile, tileWithinDistance1, tileWithinDistance2, tileWithinDistance4, tileWithinDistance3, pActor.beh_tile_target);
      }
      else
        curved = ActorMove.goToCurved(pActor, pActor.current_tile, pActor.beh_tile_target);
      pActor.timer_action = 0.5f;
      return curved == ExecuteEvent.False ? BehResult.Stop : BehResult.Continue;
    }
  }

  private static void pickBrush(GodFinger pFinger)
  {
    if (pFinger.target_tiles.Count <= 0)
      return;
    int pMinSize = pFinger.target_tiles.Count / 10;
    int pMaxSize = pFinger.target_tiles.Count / 3;
    pFinger.brush = Brush.getRandom(pMinSize, pMaxSize, new Predicate<BrushData>(BehFingerDrawToTileTarget.brushFilter));
  }

  private static bool brushFilter(BrushData pBrush)
  {
    return pBrush.id.StartsWith("circ_") || pBrush.id.StartsWith("special_");
  }

  private static void pickPower(GodFinger pFinger)
  {
    bool drawingOverGround = pFinger.drawing_over_ground;
    bool drawingOverWater = pFinger.drawing_over_water;
    HashSet<WorldTile> targetTiles = pFinger.target_tiles;
    if (pFinger.god_power != null && (drawingOverWater && GodFinger.power_over_water.Contains<string>(pFinger.god_power.id) || drawingOverGround && GodFinger.power_over_ground.Contains<string>(pFinger.god_power.id)))
      return;
    Dictionary<string, int> dictionary1 = UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();
    Dictionary<TileTypeBase, int> dictionary2 = UnsafeCollectionPool<Dictionary<TileTypeBase, int>, KeyValuePair<TileTypeBase, int>>.Get();
    HashSet<WorldTile> worldTileSet = UnsafeCollectionPool<HashSet<WorldTile>, WorldTile>.Get();
    int capacity = 0;
    foreach (WorldTile worldTile1 in targetTiles)
    {
      foreach (WorldTile worldTile2 in worldTile1.neighboursAll)
      {
        if (!targetTiles.Contains(worldTile2) && worldTileSet.Add(worldTile2))
        {
          ++capacity;
          int num1;
          dictionary2.TryGetValue(worldTile2.Type, out num1);
          dictionary2[worldTile2.Type] = ++num1;
          if (drawingOverGround)
          {
            BiomeAsset biomeAsset = worldTile2.Type.biome_asset;
            if (biomeAsset != null && !biomeAsset.special_biome)
            {
              int num2;
              dictionary1.TryGetValue(biomeAsset.tile_high, out num2);
              dictionary1[biomeAsset.tile_high] = ++num2;
              int num3;
              dictionary1.TryGetValue(biomeAsset.tile_low, out num3);
              dictionary1[biomeAsset.tile_low] = ++num3;
            }
          }
        }
      }
    }
    if (drawingOverWater)
    {
      using (ListPool<string> pList = new ListPool<string>(capacity))
      {
        foreach (string str in GodFinger.power_over_water)
        {
          GodPower godPower = AssetManager.powers.get(str);
          bool flag = false;
          TileType cachedTileTypeAsset = godPower.cached_tile_type_asset;
          if (cachedTileTypeAsset != null)
          {
            flag = true;
            int pAmount;
            if (dictionary2.TryGetValue((TileTypeBase) cachedTileTypeAsset, out pAmount))
              pList.AddTimes<string>(pAmount, str);
          }
          TopTileType topTileTypeAsset = godPower.cached_top_tile_type_asset;
          if (topTileTypeAsset != null)
          {
            flag = true;
            int pAmount;
            if (dictionary2.TryGetValue((TileTypeBase) topTileTypeAsset, out pAmount))
              pList.AddTimes<string>(pAmount, str);
          }
          if (!flag)
            pList.Add(str);
        }
        string pID = Randy.getRandom<string>(pList) ?? Randy.getRandom<string>(GodFinger.power_over_water);
        pFinger.god_power = AssetManager.powers.get(pID);
      }
    }
    else if (drawingOverGround)
    {
      using (ListPool<string> pList = new ListPool<string>(capacity))
      {
        foreach (string str in GodFinger.power_over_ground)
        {
          GodPower godPower = AssetManager.powers.get(str);
          bool flag = false;
          DropAsset cachedDropAsset = godPower.cached_drop_asset;
          if (cachedDropAsset != null)
          {
            if (!string.IsNullOrEmpty(cachedDropAsset.drop_type_high))
            {
              flag = true;
              int pAmount;
              if (dictionary1.TryGetValue(cachedDropAsset.drop_type_high, out pAmount))
                pList.AddTimes<string>(pAmount, str);
            }
            if (!string.IsNullOrEmpty(cachedDropAsset.drop_type_low))
            {
              flag = true;
              int pAmount;
              if (dictionary1.TryGetValue(cachedDropAsset.drop_type_low, out pAmount))
                pList.AddTimes<string>(pAmount, str);
            }
          }
          TileType cachedTileTypeAsset = godPower.cached_tile_type_asset;
          if (cachedTileTypeAsset != null)
          {
            flag = true;
            int pAmount;
            if (dictionary2.TryGetValue((TileTypeBase) cachedTileTypeAsset, out pAmount))
              pList.AddTimes<string>(pAmount, str);
          }
          TopTileType topTileTypeAsset = godPower.cached_top_tile_type_asset;
          if (topTileTypeAsset != null)
          {
            flag = true;
            int pAmount;
            if (dictionary2.TryGetValue((TileTypeBase) topTileTypeAsset, out pAmount))
              pList.AddTimes<string>(pAmount, str);
          }
          if (!flag)
            pList.Add(str);
        }
        string pID = Randy.getRandom<string>(pList) ?? Randy.getRandom<string>(GodFinger.power_over_ground);
        pFinger.god_power = AssetManager.powers.get(pID);
      }
    }
    else
      pFinger.god_power = (GodPower) null;
    UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(dictionary1);
    UnsafeCollectionPool<Dictionary<TileTypeBase, int>, KeyValuePair<TileTypeBase, int>>.Release(dictionary2);
    UnsafeCollectionPool<HashSet<WorldTile>, WorldTile>.Release(worldTileSet);
  }
}
