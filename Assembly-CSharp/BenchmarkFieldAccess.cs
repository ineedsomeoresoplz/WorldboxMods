// Decompiled with JetBrains decompiler
// Type: BenchmarkFieldAccess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BenchmarkFieldAccess
{
  public static void start()
  {
    if (!Config.game_loaded)
      return;
    int pCounter = 100000;
    Bench.bench("field_acess_test", "field_acess_total");
    Bench.bench("field_access", "field_acess_test");
    int num1 = 0;
    for (int index = 0; index < pCounter; ++index)
      num1 += World.world.tiles_list.Length;
    Bench.benchEnd("field_access", "field_acess_test", true, (long) pCounter);
    Bench.bench("temp_var", "field_acess_test");
    int num2 = 0;
    MapBox world = World.world;
    for (int index = 0; index < pCounter; ++index)
      num2 += world.tiles_list.Length;
    Bench.benchEnd("temp_var", "field_acess_test", true, (long) pCounter);
    Bench.bench("temp_var_2", "field_acess_test");
    int num3 = 0;
    WorldTile[] tilesList = World.world.tiles_list;
    for (int index = 0; index < pCounter; ++index)
    {
      int length = tilesList.Length;
      num3 += length;
    }
    Bench.benchEnd("temp_var_2", "field_acess_test", true, (long) pCounter);
    Bench.bench("result_len", "field_acess_test");
    int num4 = 0;
    int length1 = World.world.tiles_list.Length;
    for (int index = 0; index < pCounter; ++index)
      num4 += length1;
    Bench.benchEnd("result_len", "field_acess_test", true, (long) pCounter);
    Bench.benchEnd("field_acess_test", "field_acess_total");
  }
}
