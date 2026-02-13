// Decompiled with JetBrains decompiler
// Type: BenchmarkLoops
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BenchmarkLoops
{
  private List<WorldTile> _test_world_tiles = new List<WorldTile>();
  private ListPool<WorldTile> _test_world_tiles_pool;
  private HashSet<WorldTile> _test_hashset = new HashSet<WorldTile>();
  private WorldTile[] _test_world_tiles_arr;
  private List<WorldTile> _new_tiles = new List<WorldTile>();
  private int _runs;
  private bool _counter;
  private int _max_amount;
  private DebugToolAsset _asset;
  internal static Dictionary<string, BenchmarkLoops> _benchmarks = new Dictionary<string, BenchmarkLoops>();

  public BenchmarkLoops(DebugToolAsset pAsset, int pMaxAmount)
  {
    if (BenchmarkLoops._benchmarks.ContainsKey(pAsset.benchmark_group_id))
      return;
    BenchmarkLoops._benchmarks.Add(pAsset.benchmark_group_id, this);
    this._max_amount = pMaxAmount;
    this._asset = pAsset;
  }

  public static void update(DebugToolAsset pAsset)
  {
    BenchmarkLoops._benchmarks[pAsset.benchmark_group_id].run();
  }

  public void run()
  {
    // ISSUE: unable to decompile the method.
  }
}
