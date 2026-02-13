// Decompiled with JetBrains decompiler
// Type: BenchmarkDist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class BenchmarkDist
{
  public long result;
  internal string benchmark_group_id;
  internal string benchmark_id;
  internal List<WorldTile> test_tiles;
  internal bool print_to_console;
  private static BenchmarkDist _instance;

  public BenchmarkDist()
  {
    if (BenchmarkDist._instance != null)
      return;
    this.benchmark_group_id = "dist_test_total";
    this.benchmark_id = "dist_test";
    this.test_tiles = new List<WorldTile>();
    BenchmarkDist._instance = this;
    this.setup();
  }

  public static void update() => BenchmarkDist._instance.run();

  public void setup()
  {
    if (!Config.game_loaded)
    {
      MapBox.on_world_loaded += (Action) (() => this.setup());
    }
    else
    {
      this.test_tiles.AddRange((IEnumerable<WorldTile>) World.world.tiles_list);
      this.test_tiles.ShuffleHalf<WorldTile>();
      this.test_tiles.RemoveRange(this.test_tiles.Count / 2, this.test_tiles.Count / 2);
    }
  }

  public void run()
  {
    string benchmarkGroupId = this.benchmark_group_id;
    string benchmarkId = this.benchmark_id;
    int maxValue1 = int.MaxValue;
    float maxValue2 = float.MaxValue;
    List<WorldTile> testTiles = this.test_tiles;
    testTiles.Shuffle<WorldTile>();
    int2[] int2Array = new int2[testTiles.Count];
    for (int index = 0; index < testTiles.Count; ++index)
      int2Array[index] = new int2(testTiles[index].x, testTiles[index].y);
    float2[] float2Array = new float2[testTiles.Count];
    for (int index = 0; index < testTiles.Count; ++index)
      float2Array[index] = new float2((float) testTiles[index].x, (float) testTiles[index].y);
    NativeArray<int2> nativeArray1 = new NativeArray<int2>(int2Array, (Allocator) 3);
    NativeArray<float2> nativeArray2 = new NativeArray<float2>(float2Array, (Allocator) 3);
    WorldTile pT1 = testTiles[0];
    Vector2Int pos1 = pT1.pos;
    Vector3 posV3 = pT1.posV3;
    int2 int2_1;
    // ISSUE: explicit constructor call
    ((int2) ref int2_1).\u002Ector(pT1.x, pT1.y);
    float2 float2_1;
    // ISSUE: explicit constructor call
    ((float2) ref float2_1).\u002Ector((float) pT1.x, (float) pT1.y);
    Bench.bench(benchmarkId, benchmarkGroupId);
    int index1 = -1;
    maxValue1 = int.MaxValue;
    float num1 = float.MaxValue;
    double num2 = 0.0;
    int num3 = 0;
    Bench.bench("DistTile", benchmarkId);
    for (int index2 = 1; index2 < testTiles.Count; ++index2)
    {
      WorldTile pT2 = testTiles[index2];
      float num4 = Toolbox.DistTile(pT1, pT2);
      if ((double) num4 < (double) num1)
      {
        num1 = num4;
        index1 = index2;
      }
      num2 += (double) num4;
      ++num3;
    }
    Bench.benchEnd("DistTile", benchmarkId, true, (long) testTiles[index1].tile_id);
    int index3 = -1;
    maxValue1 = int.MaxValue;
    float num5 = float.MaxValue;
    double num6 = 0.0;
    int num7 = 0;
    Bench.bench("DistVec2", benchmarkId);
    for (int index4 = 1; index4 < testTiles.Count; ++index4)
    {
      WorldTile worldTile = testTiles[index4];
      float num8 = Toolbox.DistVec2(pos1, worldTile.pos);
      if ((double) num8 < (double) num5)
      {
        num5 = num8;
        index3 = index4;
      }
      num6 += (double) num8;
      ++num7;
    }
    Bench.benchEnd("DistVec2", benchmarkId, true, (long) testTiles[index3].tile_id);
    int index5 = -1;
    maxValue1 = int.MaxValue;
    float num9 = float.MaxValue;
    double num10 = 0.0;
    int num11 = 0;
    Bench.bench("DistVec3", benchmarkId);
    for (int index6 = 1; index6 < testTiles.Count; ++index6)
    {
      WorldTile worldTile = testTiles[index6];
      float num12 = Toolbox.DistVec3(posV3, worldTile.posV3);
      if ((double) num12 < (double) num9)
      {
        num9 = num12;
        index5 = index6;
      }
      num10 += (double) num12;
      ++num11;
    }
    Bench.benchEnd("DistVec3", benchmarkId, true, (long) testTiles[index5].tile_id);
    int index7 = -1;
    maxValue1 = int.MaxValue;
    float num13 = float.MaxValue;
    double num14 = 0.0;
    int num15 = 0;
    Bench.bench("Dist", benchmarkId);
    for (int index8 = 1; index8 < testTiles.Count; ++index8)
    {
      WorldTile worldTile = testTiles[index8];
      float num16 = Toolbox.Dist(pT1.x, pT1.y, worldTile.x, worldTile.y);
      if ((double) num16 < (double) num13)
      {
        num13 = num16;
        index7 = index8;
      }
      num14 += (double) num16;
      ++num15;
    }
    Bench.benchEnd("Dist", benchmarkId, true, (long) testTiles[index7].tile_id);
    int index9 = -1;
    maxValue1 = int.MaxValue;
    float num17 = float.MaxValue;
    double num18 = 0.0;
    int num19 = 0;
    Bench.bench("DistFloat", benchmarkId);
    for (int index10 = 1; index10 < testTiles.Count; ++index10)
    {
      WorldTile worldTile = testTiles[index10];
      float num20 = BenchmarkDist.DistFloat((float) pT1.x, (float) pT1.y, (float) worldTile.x, (float) worldTile.y);
      if ((double) num20 < (double) num17)
      {
        num17 = num20;
        index9 = index10;
      }
      num18 += (double) num20;
      ++num19;
    }
    Bench.benchEnd("DistFloat", benchmarkId, true, (long) testTiles[index9].tile_id);
    int index11 = -1;
    maxValue1 = int.MaxValue;
    float num21 = float.MaxValue;
    double num22 = 0.0;
    int num23 = 0;
    Bench.bench("Dist.pos", benchmarkId);
    for (int index12 = 1; index12 < testTiles.Count; ++index12)
    {
      Vector2Int pos2 = testTiles[index12].pos;
      float num24 = Toolbox.Dist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
      if ((double) num24 < (double) num21)
      {
        num21 = num24;
        index11 = index12;
      }
      num22 += (double) num24;
      ++num23;
    }
    Bench.benchEnd("Dist.pos", benchmarkId, true, (long) testTiles[index11].tile_id);
    int index13 = -1;
    int num25 = int.MaxValue;
    maxValue2 = float.MaxValue;
    double num26 = 0.0;
    int num27 = 0;
    Bench.bench("FastDistTile", benchmarkId);
    for (int index14 = 1; index14 < testTiles.Count; ++index14)
    {
      WorldTile pT2 = testTiles[index14];
      int num28 = Toolbox.SquaredDistTile(pT1, pT2);
      if (num28 < num25)
      {
        num25 = num28;
        index13 = index14;
      }
      num26 += (double) num28;
      ++num27;
    }
    Bench.benchEnd("FastDistTile", benchmarkId, true, (long) testTiles[index13].tile_id);
    int index15 = -1;
    int num29 = int.MaxValue;
    maxValue2 = float.MaxValue;
    double num30 = 0.0;
    int num31 = 0;
    Bench.bench("FastDist", benchmarkId);
    for (int index16 = 1; index16 < testTiles.Count; ++index16)
    {
      WorldTile worldTile = testTiles[index16];
      int num32 = Toolbox.SquaredDist(pT1.x, pT1.y, worldTile.x, worldTile.y);
      if (num32 < num29)
      {
        num29 = num32;
        index15 = index16;
      }
      num30 += (double) num32;
      ++num31;
    }
    Bench.benchEnd("FastDist", benchmarkId, true, (long) testTiles[index15].tile_id);
    int index17 = -1;
    maxValue1 = int.MaxValue;
    float num33 = float.MaxValue;
    double num34 = 0.0;
    int num35 = 0;
    Bench.bench("FastDistFloat", benchmarkId);
    for (int index18 = 1; index18 < testTiles.Count; ++index18)
    {
      WorldTile worldTile = testTiles[index18];
      float num36 = BenchmarkDist.FastDistFloat((float) pT1.x, (float) pT1.y, (float) worldTile.x, (float) worldTile.y);
      if ((double) num36 < (double) num33)
      {
        num33 = num36;
        index17 = index18;
      }
      num34 += (double) num36;
      ++num35;
    }
    Bench.benchEnd("FastDistFloat", benchmarkId, true, (long) testTiles[index17].tile_id);
    int index19 = -1;
    int num37 = int.MaxValue;
    maxValue2 = float.MaxValue;
    double num38 = 0.0;
    int num39 = 0;
    Bench.bench("FastDistVec2", benchmarkId);
    for (int index20 = 1; index20 < testTiles.Count; ++index20)
    {
      WorldTile worldTile = testTiles[index20];
      int num40 = Toolbox.SquaredDistVec2(pos1, worldTile.pos);
      if (num40 < num37)
      {
        num37 = num40;
        index19 = index20;
      }
      num38 += (double) num40;
      ++num39;
    }
    Bench.benchEnd("FastDistVec2", benchmarkId, true, (long) testTiles[index19].tile_id);
    int index21 = -1;
    maxValue1 = int.MaxValue;
    float num41 = float.MaxValue;
    double num42 = 0.0;
    int num43 = 0;
    Bench.bench("FastDistVec3", benchmarkId);
    for (int index22 = 1; index22 < testTiles.Count; ++index22)
    {
      WorldTile worldTile = testTiles[index22];
      float num44 = Toolbox.SquaredDistVec3(posV3, worldTile.posV3);
      if ((double) num44 < (double) num41)
      {
        num41 = num44;
        index21 = index22;
      }
      num42 += (double) num44;
      ++num43;
    }
    Bench.benchEnd("FastDistVec3", benchmarkId, true, (long) testTiles[index21].tile_id);
    int index23 = -1;
    maxValue1 = int.MaxValue;
    float num45 = float.MaxValue;
    double num46 = 0.0;
    int num47 = 0;
    Bench.bench("FastDist.pos", benchmarkId);
    for (int index24 = 1; index24 < testTiles.Count; ++index24)
    {
      Vector2Int pos3 = testTiles[index24].pos;
      float num48 = (float) Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos3).x, ((Vector2Int) ref pos3).y);
      if ((double) num48 < (double) num45)
      {
        num45 = num48;
        index23 = index24;
      }
      num46 += (double) num48;
      ++num47;
    }
    Bench.benchEnd("FastDist.pos", benchmarkId, true, (long) testTiles[index23].tile_id);
    int index25 = -1;
    maxValue1 = int.MaxValue;
    float num49 = float.MaxValue;
    double num50 = 0.0;
    int num51 = 0;
    Bench.bench("distancesq", benchmarkId);
    for (int index26 = 1; index26 < testTiles.Count; ++index26)
    {
      WorldTile worldTile = testTiles[index26];
      float num52 = math.distancesq((float) pT1.x, (float) worldTile.x) + math.distancesq((float) pT1.y, (float) worldTile.y);
      if ((double) num52 < (double) num49)
      {
        num49 = num52;
        index25 = index26;
      }
      num50 += (double) num52;
      ++num51;
    }
    Bench.benchEnd("distancesq", benchmarkId, true, (long) testTiles[index25].tile_id);
    int index27 = -1;
    maxValue1 = int.MaxValue;
    float num53 = float.MaxValue;
    double num54 = 0.0;
    int num55 = 0;
    Bench.bench("float2", benchmarkId);
    for (int index28 = 1; index28 < testTiles.Count; ++index28)
    {
      WorldTile worldTile = testTiles[index28];
      float2 float2_2;
      // ISSUE: explicit constructor call
      ((float2) ref float2_2).\u002Ector((float) worldTile.x, (float) worldTile.y);
      float num56 = math.distancesq(float2_1, float2_2);
      if ((double) num56 < (double) num53)
      {
        num53 = num56;
        index27 = index28;
      }
      num54 += (double) num56;
      ++num55;
    }
    Bench.benchEnd("float2", benchmarkId, true, (long) testTiles[index27].tile_id);
    int index29 = -1;
    maxValue1 = int.MaxValue;
    float num57 = float.MaxValue;
    double num58 = 0.0;
    int num59 = 0;
    Bench.bench("int2", benchmarkId);
    for (int index30 = 1; index30 < testTiles.Count; ++index30)
    {
      WorldTile worldTile = testTiles[index30];
      int2 int2_2;
      // ISSUE: explicit constructor call
      ((int2) ref int2_2).\u002Ector(worldTile.x, worldTile.y);
      float num60 = math.distancesq(float2.op_Implicit(int2_1), float2.op_Implicit(int2_2));
      if ((double) num60 < (double) num57)
      {
        num57 = num60;
        index29 = index30;
      }
      num58 += (double) num60;
      ++num59;
    }
    Bench.benchEnd("int2", benchmarkId, true, (long) testTiles[index29].tile_id);
    int index31 = -1;
    maxValue1 = int.MaxValue;
    float num61 = float.MaxValue;
    double num62 = 0.0;
    int num63 = 0;
    Bench.bench("int2array", benchmarkId);
    for (int index32 = 1; index32 < int2Array.Length; ++index32)
    {
      float num64 = math.distancesq(float2.op_Implicit(int2_1), float2.op_Implicit(int2Array[index32]));
      if ((double) num64 < (double) num61)
      {
        num61 = num64;
        index31 = index32;
      }
      num62 += (double) num64;
      ++num63;
    }
    Bench.benchEnd("int2array", benchmarkId, true, (long) testTiles[index31].tile_id);
    int index33 = -1;
    maxValue1 = int.MaxValue;
    float num65 = float.MaxValue;
    double num66 = 0.0;
    int num67 = 0;
    Bench.bench("nint2array", benchmarkId);
    for (int index34 = 1; index34 < nativeArray2.Length; ++index34)
    {
      float num68 = math.distancesq(float2.op_Implicit(int2_1), nativeArray2[index34]);
      if ((double) num68 < (double) num65)
      {
        num65 = num68;
        index33 = index34;
      }
      num66 += (double) num68;
      ++num67;
    }
    Bench.benchEnd("nint2array", benchmarkId, true, (long) testTiles[index33].tile_id);
    int index35 = -1;
    maxValue1 = int.MaxValue;
    float num69 = float.MaxValue;
    double num70 = 0.0;
    int num71 = 0;
    Bench.bench("float2array", benchmarkId);
    for (int index36 = 1; index36 < float2Array.Length; ++index36)
    {
      float num72 = math.distancesq(float2_1, float2Array[index36]);
      if ((double) num72 < (double) num69)
      {
        num69 = num72;
        index35 = index36;
      }
      num70 += (double) num72;
      ++num71;
    }
    Bench.benchEnd("float2array", benchmarkId, true, (long) testTiles[index35].tile_id);
    int index37 = -1;
    maxValue1 = int.MaxValue;
    float num73 = float.MaxValue;
    double num74 = 0.0;
    int num75 = 0;
    Bench.bench("nfloat2array", benchmarkId);
    for (int index38 = 1; index38 < nativeArray1.Length; ++index38)
    {
      float num76 = math.distancesq(float2_1, float2.op_Implicit(nativeArray1[index38]));
      if ((double) num76 < (double) num73)
      {
        num73 = num76;
        index37 = index38;
      }
      num74 += (double) num76;
      ++num75;
    }
    Bench.benchEnd("nfloat2array", benchmarkId, true, (long) testTiles[index37].tile_id);
    nativeArray1.Dispose();
    nativeArray2.Dispose();
    Bench.benchEnd(benchmarkId, benchmarkGroupId);
    if (this.print_to_console)
    {
      Debug.Log((object) ("LAST:\n" + Bench.printableBenchResults(benchmarkId, false, "DistTile", "DistVec2", "DistVec3", "Dist", "DistFloat", "Dist.pos", "FastDistTile", "FastDistVec2", "FastDistVec3", "FastDist", "FastDistFloat", "FastDist.pos", "int2", "int2array", "nint2array", "float2", "float2array", "nfloat2array", "distancesq", "job_new", "job_prefill", "pjob_prefill", "BurstDist", "BurstDistFloat", "BurstFastDistFloat", "BurstDist.pos", "BurstFastDist", "BurstFastDist.pos")));
      Debug.Log((object) ("AVG:\n" + Bench.printableBenchResults(benchmarkId, true, "DistTile", "DistVec2", "DistVec3", "Dist", "DistFloat", "Dist.pos", "FastDistTile", "FastDistVec2", "FastDistVec3", "FastDist", "FastDistFloat", "FastDist.pos", "int2", "int2array", "nint2array", "float2", "float2array", "nfloat2array", "distancesq", "job_new", "job_prefill", "pjob_prefill", "BurstDist", "BurstDistFloat", "BurstFastDistFloat", "BurstDist.pos", "BurstFastDist", "BurstFastDist.pos")));
    }
    this.result = (long) num74;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float DistFloat(float x1, float y1, float x2, float y2)
  {
    return Mathf.Sqrt((float) (((double) x1 - (double) x2) * ((double) x1 - (double) x2) + ((double) y1 - (double) y2) * ((double) y1 - (double) y2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float FastDistFloat(float x1, float y1, float x2, float y2)
  {
    return (float) (((double) x1 - (double) x2) * ((double) x1 - (double) x2) + ((double) y1 - (double) y2) * ((double) y1 - (double) y2));
  }
}
