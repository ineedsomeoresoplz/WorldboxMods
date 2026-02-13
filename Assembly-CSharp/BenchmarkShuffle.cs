// Decompiled with JetBrains decompiler
// Type: BenchmarkShuffle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BenchmarkShuffle
{
  public int result;
  internal int max_tiles;
  internal int amount;
  internal string benchmark_total_group_id;
  internal string benchmark_group_id;
  internal List<WorldTile> test_tiles;
  internal bool print_to_console;
  internal static Dictionary<string, BenchmarkShuffle> _benchmarks = new Dictionary<string, BenchmarkShuffle>();

  public BenchmarkShuffle(DebugToolAsset pAsset, int pAmount, int pMaxTiles)
  {
    if (BenchmarkShuffle._benchmarks.ContainsKey(pAsset.benchmark_group_id))
      return;
    this.amount = pAmount;
    this.max_tiles = pMaxTiles;
    this.benchmark_total_group_id = pAsset.benchmark_total_group;
    this.benchmark_group_id = pAsset.benchmark_group_id;
    this.test_tiles = new List<WorldTile>();
    BenchmarkShuffle._benchmarks.Add(pAsset.benchmark_group_id, this);
    this.setup();
  }

  public static void update(DebugToolAsset pAsset)
  {
    BenchmarkShuffle._benchmarks[pAsset.benchmark_group_id].run();
  }

  public void setup()
  {
    if (!Config.game_loaded)
    {
      MapBox.on_world_loaded += (Action) (() => this.setup());
    }
    else
    {
      int maxTiles = this.max_tiles;
      this.test_tiles.Clear();
      int num1 = Mathf.CeilToInt(Mathf.Sqrt((float) maxTiles));
      int num2 = num1 * num1;
      using (ListPool<WorldTile> list = new ListPool<WorldTile>(World.world.tiles_list))
      {
        list.Shuffle<WorldTile>();
        for (int index = 0; index < num2; ++index)
          this.test_tiles.Add(list.Pop<WorldTile>());
        this.test_tiles.Shuffle<WorldTile>();
      }
    }
  }

  public void run()
  {
    int amount = this.amount;
    string benchmarkTotalGroupId = this.benchmark_total_group_id;
    string benchmarkGroupId = this.benchmark_group_id;
    int num1 = 0;
    int num2 = 0;
    List<WorldTile> testTiles = this.test_tiles;
    for (int index = amount - 1; index >= 0; --index)
    {
      WorldTile worldTile = testTiles[index];
      num2 += worldTile.data.tile_id;
      ++num1;
    }
    Bench.bench(benchmarkGroupId, benchmarkTotalGroupId);
    this.test_tiles.Shuffle<WorldTile>();
    int num3 = 0;
    int pCounter1 = 0;
    Bench.bench($"no_shuffle_for_{amount}", benchmarkGroupId);
    for (int index = 0; index < amount; ++index)
    {
      WorldTile worldTile = testTiles[index];
      num3 += worldTile.data.tile_id;
      ++pCounter1;
    }
    Bench.benchEnd($"no_shuffle_for_{amount}", benchmarkGroupId, true, (long) pCounter1);
    this.test_tiles.Shuffle<WorldTile>();
    int num4 = 0;
    int pCounter2 = 0;
    Bench.bench($"shuffle_all_{amount}", benchmarkGroupId);
    testTiles.Shuffle<WorldTile>();
    for (int index = 0; index < amount; ++index)
    {
      WorldTile worldTile = testTiles[index];
      num4 += worldTile.data.tile_id;
      ++pCounter2;
    }
    Bench.benchEnd($"shuffle_all_{amount}", benchmarkGroupId, true, (long) pCounter2);
    this.test_tiles.Shuffle<WorldTile>();
    int num5 = 0;
    int pCounter3 = 0;
    Bench.bench($"shuffle_one_new_list_{amount}", benchmarkGroupId);
    ListPool<WorldTile> list = new ListPool<WorldTile>((ICollection<WorldTile>) testTiles);
    for (int index = 0; index < amount; ++index)
    {
      list.ShuffleOne<WorldTile>(index);
      WorldTile worldTile = list[index];
      num5 += worldTile.data.tile_id;
      ++pCounter3;
    }
    list.Dispose();
    Bench.benchEnd($"shuffle_one_new_list_{amount}", benchmarkGroupId, true, (long) pCounter3);
    this.test_tiles.Shuffle<WorldTile>();
    int num6 = 0;
    int pCounter4 = 0;
    Bench.bench($"shuffle_one_{amount}", benchmarkGroupId);
    for (int index = 0; index < amount; ++index)
    {
      testTiles.ShuffleOne<WorldTile>(index);
      WorldTile worldTile = testTiles[index];
      num6 += worldTile.data.tile_id;
      ++pCounter4;
    }
    Bench.benchEnd($"shuffle_one_{amount}", benchmarkGroupId, true, (long) pCounter4);
    this.test_tiles.Shuffle<WorldTile>();
    int num7 = 0;
    int pCounter5 = 0;
    Bench.bench($"shuffle_for_{amount}", benchmarkGroupId);
    int num8 = Randy.randomInt(0, amount);
    int num9 = amount + num8;
    for (int index1 = num8; index1 < num9; ++index1)
    {
      int index2 = index1 % amount;
      WorldTile worldTile = testTiles[index2];
      num7 += worldTile.data.tile_id;
      ++pCounter5;
    }
    Bench.benchEnd($"shuffle_for_{amount}", benchmarkGroupId, true, (long) pCounter5);
    this.test_tiles.Shuffle<WorldTile>();
    int num10 = 0;
    int pCounter6 = 0;
    Bench.bench($"shuffle_2for_{amount}", benchmarkGroupId);
    int num11 = Randy.randomInt(0, amount);
    for (int index = num11; index < amount; ++index)
    {
      WorldTile worldTile = testTiles[index];
      num10 += worldTile.data.tile_id;
      ++pCounter6;
    }
    for (int index = 0; index < num11; ++index)
    {
      WorldTile worldTile = testTiles[index];
      num10 += worldTile.data.tile_id;
      ++pCounter6;
    }
    Bench.benchEnd($"shuffle_2for_{amount}", benchmarkGroupId, true, (long) pCounter6);
    this.test_tiles.Shuffle<WorldTile>();
    int num12 = 0;
    int pCounter7 = 0;
    Bench.bench($"shuffle_iterator_{amount}", benchmarkGroupId);
    foreach (WorldTile worldTile in testTiles.LoopRandom<WorldTile>())
    {
      num12 += worldTile.data.tile_id;
      ++pCounter7;
      if (pCounter7 == amount)
        break;
    }
    Bench.benchEnd($"shuffle_iterator_{amount}", benchmarkGroupId, true, (long) pCounter7);
    this.test_tiles.Shuffle<WorldTile>();
    int num13 = 0;
    int pCounter8 = 0;
    Bench.bench($"shuffle_iterator_limit_{amount}", benchmarkGroupId);
    foreach (WorldTile worldTile in testTiles.LoopRandom<WorldTile>(amount))
    {
      num13 += worldTile.data.tile_id;
      ++pCounter8;
    }
    Bench.benchEnd($"shuffle_iterator_limit_{amount}", benchmarkGroupId, true, (long) pCounter8);
    this.test_tiles.Shuffle<WorldTile>();
    int num14 = 0;
    int pCounter9 = 0;
    Bench.bench($"no_shuffle_iterator_{amount}", benchmarkGroupId);
    foreach (WorldTile worldTile in testTiles)
    {
      num14 += worldTile.data.tile_id;
      ++pCounter9;
      if (pCounter9 == amount)
        break;
    }
    Bench.benchEnd($"no_shuffle_iterator_{amount}", benchmarkGroupId, true, (long) pCounter9);
    Bench.benchEnd(benchmarkGroupId, benchmarkTotalGroupId);
    if (this.print_to_console)
    {
      Debug.Log((object) ("LAST:\n" + Bench.printableBenchResults(benchmarkGroupId, false, $"no_shuffle_for_{amount}", $"no_shuffle_iterator_{amount}", $"shuffle_iterator_{amount}", $"shuffle_iterator_limit_{amount}", $"shuffle_for_{amount}", $"shuffle_2for_{amount}", $"shuffle_one_{amount}", $"shuffle_one_new_list_{amount}", $"shuffle_all_{amount}")));
      Debug.Log((object) ("AVG:\n" + Bench.printableBenchResults(benchmarkGroupId, true, $"no_shuffle_for_{amount}", $"no_shuffle_iterator_{amount}", $"shuffle_iterator_{amount}", $"shuffle_iterator_limit_{amount}", $"shuffle_for_{amount}", $"shuffle_2for_{amount}", $"shuffle_one_{amount}", $"shuffle_one_new_list_{amount}", $"shuffle_all_{amount}")));
    }
    this.result = num14;
  }
}
