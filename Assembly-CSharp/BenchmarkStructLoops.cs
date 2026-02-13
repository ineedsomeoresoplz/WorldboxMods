// Decompiled with JetBrains decompiler
// Type: BenchmarkStructLoops
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class BenchmarkStructLoops
{
  private static List<WorldTileDataStruct> _test_world_tiles = new List<WorldTileDataStruct>();
  private static ListPool<WorldTileDataStruct> _test_world_tiles_pool;
  private static HashSet<WorldTileDataStruct> _test_hashset = new HashSet<WorldTileDataStruct>();
  private static WorldTileDataStruct[] _test_world_tiles_arr;
  private static int _runs = 0;

  public static void start()
  {
    // ISSUE: unable to decompile the method.
  }
}
