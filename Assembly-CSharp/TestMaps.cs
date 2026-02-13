// Decompiled with JetBrains decompiler
// Type: TestMaps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

#nullable disable
public static class TestMaps
{
  private static bool _initialized = false;
  private static string[] _maps;
  private static int _index = -1;

  public static void init()
  {
    if (TestMaps._initialized)
      return;
    TestMaps._initialized = true;
    using (ListPool<string> list = new ListPool<string>())
    {
      string[] files1 = Directory.GetFiles("test_maps", "*.wbox", SearchOption.AllDirectories);
      list.AddRange(files1);
      string[] files2 = Directory.GetFiles("test_maps", "*.json", SearchOption.AllDirectories);
      list.AddRange(files2);
      list.RemoveAll((Predicate<string>) (p => p.Contains("debug")));
      TestMaps._maps = list.ToArray<string>();
      TestMaps._index = Toolbox.loopIndex(Randy.randomInt(0, TestMaps._maps.Length * 100), TestMaps._maps.Length);
    }
  }

  public static void loadMap(int pIndex)
  {
    string map = TestMaps._maps[pIndex];
    Debug.Log((object) $"Loading map: {map} ({TestMaps._index + 1}/{TestMaps._maps.Length})");
    World.world.save_manager.loadWorld(SaveManager.folderPath(Path.GetDirectoryName(map)));
  }

  public static void loadNextMap()
  {
    TestMaps.init();
    TestMaps._index = Toolbox.loopIndex(TestMaps._index + 1, TestMaps._maps.Length);
    TestMaps.loadMap(TestMaps._index);
  }

  public static void loadPrevMap()
  {
    TestMaps.init();
    TestMaps._index = Toolbox.loopIndex(TestMaps._index - 1, TestMaps._maps.Length);
    TestMaps.loadMap(TestMaps._index);
  }
}
