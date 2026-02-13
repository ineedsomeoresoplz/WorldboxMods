// Decompiled with JetBrains decompiler
// Type: Benchmark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

#nullable disable
public static class Benchmark
{
  public static void benchHashsetStart()
  {
    for (int index = 0; index < 1000; ++index)
      Benchmark.benchObjHashsetCreateVsAdd(5000);
    Debug.Log((object) ("- BenchTest - list:" + Bench.getBenchResult("BenchTest - list")));
    Debug.Log((object) ("- BenchTest - hashset:" + Bench.getBenchResult("BenchTest - hashset")));
  }

  public static void benchObjHashsetCreateVsAdd(int pAmount = 3000)
  {
    Bench.bench_enabled = true;
    List<BenchObject> benchObjectList = new List<BenchObject>();
    int num1 = pAmount;
    for (int index = 0; index < num1; ++index)
      benchObjectList.Add(new BenchObject());
    int num2 = 10;
    List<BenchObject> list = new List<BenchObject>();
    list.AddRange((IEnumerable<BenchObject>) benchObjectList);
    Bench.bench("BenchTest - list");
    for (int index = 0; index < num2; ++index)
    {
      BenchObject random = list.GetRandom<BenchObject>();
      list.Remove(random);
    }
    Bench.benchEnd("BenchTest - list");
    HashSet<BenchObject> collection = new HashSet<BenchObject>();
    collection.UnionWith((IEnumerable<BenchObject>) benchObjectList);
    Bench.bench("BenchTest - hashset");
    for (int index = 0; index < num2; ++index)
    {
      BenchObject random = benchObjectList.GetRandom<BenchObject>();
      collection.Remove(random);
    }
    list.Clear();
    list.AddRange((IEnumerable<BenchObject>) collection);
    Bench.benchEnd("BenchTest - hashset");
  }

  public static void benchObjectsVsData(int pObjects)
  {
    double pTries = 1000.0;
    Bench.bench_enabled = true;
    int length = pObjects;
    Debug.Log((object) "----");
    Debug.Log((object) ("NEW BENCH - " + pObjects.ToString()));
    BenchObject[] benchObjectArray1 = new BenchObject[length];
    for (int index = 0; index < benchObjectArray1.Length; ++index)
      benchObjectArray1[index] = new BenchObject();
    Stopwatch p1 = new Stopwatch();
    p1.Start();
    for (int index1 = 0; (double) index1 < pTries; ++index1)
    {
      for (int index2 = 0; index2 < benchObjectArray1.Length; ++index2)
        benchObjectArray1[index2].update(0.0f);
    }
    p1.Stop();
    BenchObject[] benchObjectArray2 = new BenchObject[length];
    for (int index = 0; index < benchObjectArray2.Length; ++index)
      benchObjectArray2[index] = new BenchObject();
    Stopwatch p2_1 = new Stopwatch();
    p2_1.Start();
    for (int index3 = 0; (double) index3 < pTries; ++index3)
    {
      for (int index4 = 0; index4 < benchObjectArray2.Length; ++index4)
      {
        benchObjectArray2[index4].updateMove(0.0f);
        benchObjectArray2[index4].updateMove(0.0f);
        benchObjectArray2[index4].updateMove(0.0f);
        benchObjectArray2[index4].updateMove(0.0f);
        benchObjectArray2[index4].updateMove(0.0f);
      }
    }
    p2_1.Stop();
    BenchObject[] benchObjectArray3 = new BenchObject[length];
    for (int index = 0; index < benchObjectArray3.Length; ++index)
      benchObjectArray3[index] = new BenchObject();
    Stopwatch p2_2 = new Stopwatch();
    p2_2.Start();
    for (int index5 = 0; (double) index5 < pTries; ++index5)
    {
      for (int index6 = 0; index6 < benchObjectArray3.Length; ++index6)
      {
        BenchObject benchObject = benchObjectArray3[index6];
        benchObject.updateMove(0.0f);
        benchObject.updateMove(0.0f);
        benchObject.updateMove(0.0f);
        benchObject.updateMove(0.0f);
        benchObject.updateMove(0.0f);
      }
    }
    p2_2.Stop();
    BenchObject[] source = new BenchObject[length];
    for (int index = 0; index < source.Length; ++index)
      source[index] = new BenchObject();
    Stopwatch p2_3 = new Stopwatch();
    p2_3.Start();
    for (int index = 0; (double) index < pTries; ++index)
      Parallel.ForEach<BenchObject>((IEnumerable<BenchObject>) source, World.world.parallel_options, (Action<BenchObject>) (pObject =>
      {
        pObject.updateMove(0.0f);
        pObject.updateMove(0.0f);
        pObject.updateMove(0.0f);
        pObject.updateMove(0.0f);
        pObject.updateMove(0.0f);
      }));
    p2_3.Stop();
    BenchObject[] benchObjectArray4 = new BenchObject[length];
    for (int index = 0; index < benchObjectArray4.Length; ++index)
      benchObjectArray4[index] = new BenchObject();
    Stopwatch p2_4 = new Stopwatch();
    p2_4.Start();
    for (int index7 = 0; (double) index7 < pTries; ++index7)
    {
      for (int index8 = 0; index8 < benchObjectArray4.Length; ++index8)
      {
        benchObjectArray4[index8].derp += 22;
        if (benchObjectArray4[index8].derp == 1000)
        {
          benchObjectArray4[index8].derp += 10;
          if (benchObjectArray4[index8].derp < 10)
            benchObjectArray4[index8].derp += 5;
          else
            benchObjectArray4[index8].derp -= 5;
        }
      }
      for (int index9 = 0; index9 < benchObjectArray4.Length; ++index9)
      {
        benchObjectArray4[index9].derp += 22;
        if (benchObjectArray4[index9].derp == 1000)
        {
          benchObjectArray4[index9].derp += 10;
          if (benchObjectArray4[index9].derp < 10)
            benchObjectArray4[index9].derp += 5;
          else
            benchObjectArray4[index9].derp -= 5;
        }
      }
      for (int index10 = 0; index10 < benchObjectArray4.Length; ++index10)
      {
        benchObjectArray4[index10].derp += 22;
        if (benchObjectArray4[index10].derp == 1000)
        {
          benchObjectArray4[index10].derp += 10;
          if (benchObjectArray4[index10].derp < 10)
            benchObjectArray4[index10].derp += 5;
          else
            benchObjectArray4[index10].derp -= 5;
        }
      }
      for (int index11 = 0; index11 < benchObjectArray4.Length; ++index11)
      {
        benchObjectArray4[index11].derp += 22;
        if (benchObjectArray4[index11].derp == 1000)
        {
          benchObjectArray4[index11].derp += 10;
          if (benchObjectArray4[index11].derp < 10)
            benchObjectArray4[index11].derp += 5;
          else
            benchObjectArray4[index11].derp -= 5;
        }
      }
      for (int index12 = 0; index12 < benchObjectArray4.Length; ++index12)
      {
        benchObjectArray4[index12].derp += 22;
        if (benchObjectArray4[index12].derp == 1000)
        {
          benchObjectArray4[index12].derp += 10;
          if (benchObjectArray4[index12].derp < 10)
            benchObjectArray4[index12].derp += 5;
          else
            benchObjectArray4[index12].derp -= 5;
        }
      }
    }
    p2_4.Stop();
    BenchObject[] benchObjectArray5 = new BenchObject[length];
    for (int index = 0; index < benchObjectArray5.Length; ++index)
      benchObjectArray5[index] = new BenchObject();
    Stopwatch p2_5 = new Stopwatch();
    p2_5.Start();
    for (int index13 = 0; (double) index13 < pTries; ++index13)
    {
      for (int index14 = 0; index14 < benchObjectArray5.Length; ++index14)
      {
        BenchObject benchObject = benchObjectArray5[index14];
        benchObject.derp += 22;
        if (benchObject.derp == 1000)
        {
          benchObject.derp += 10;
          if (benchObject.derp < 10)
            benchObject.derp += 5;
          else
            benchObject.derp -= 5;
        }
      }
      for (int index15 = 0; index15 < benchObjectArray5.Length; ++index15)
      {
        BenchObject benchObject = benchObjectArray5[index15];
        benchObject.derp += 22;
        if (benchObject.derp == 1000)
        {
          benchObject.derp += 10;
          if (benchObject.derp < 10)
            benchObject.derp += 5;
          else
            benchObject.derp -= 5;
        }
      }
      for (int index16 = 0; index16 < benchObjectArray5.Length; ++index16)
      {
        BenchObject benchObject = benchObjectArray5[index16];
        benchObject.derp += 22;
        if (benchObject.derp == 1000)
        {
          benchObject.derp += 10;
          if (benchObject.derp < 10)
            benchObject.derp += 5;
          else
            benchObject.derp -= 5;
        }
      }
      for (int index17 = 0; index17 < benchObjectArray5.Length; ++index17)
      {
        BenchObject benchObject = benchObjectArray5[index17];
        benchObject.derp += 22;
        if (benchObject.derp == 1000)
        {
          benchObject.derp += 10;
          if (benchObject.derp < 10)
            benchObject.derp += 5;
          else
            benchObject.derp -= 5;
        }
      }
      for (int index18 = 0; index18 < benchObjectArray5.Length; ++index18)
      {
        BenchObject benchObject = benchObjectArray5[index18];
        benchObject.derp += 22;
        if (benchObject.derp == 1000)
        {
          benchObject.derp += 10;
          if (benchObject.derp < 10)
            benchObject.derp += 5;
          else
            benchObject.derp -= 5;
        }
      }
    }
    p2_5.Stop();
    Debug.Log((object) $"bench_objects {((double) p1.ElapsedTicks / pTries).ToString()} 100%");
    Debug.Log((object) ("bench_data " + Benchmark.getResult(p1, p2_1, pTries)));
    Debug.Log((object) ("bench_data_index " + Benchmark.getResult(p1, p2_4, pTries)));
    Debug.Log((object) ("bench_data_temp " + Benchmark.getResult(p1, p2_5, pTries)));
    Debug.Log((object) ("stopwatch_parallel " + Benchmark.getResult(p1, p2_3, pTries)));
    Debug.Log((object) ("stopwatch_data_optimized " + Benchmark.getResult(p1, p2_2, pTries)));
  }

  private static string getResult(Stopwatch p1, Stopwatch p2, double pTries)
  {
    double num1 = (double) p1.ElapsedTicks / pTries;
    double num2 = (double) p2.ElapsedTicks / pTries;
    double num3 = num2;
    double num4 = num1 / num3 * 100.0 - 100.0;
    return $"{num2.ToString()}, {num4.ToString()}%";
  }

  public static void benchNativeECSAndOOP()
  {
    Bench.bench_enabled = true;
    int length = 200000;
    NativeArray<Vector3> nativeArray1 = new NativeArray<Vector3>(length, (Allocator) 3, (NativeArrayOptions) 1);
    NativeArray<int> nativeArray2 = new NativeArray<int>(length, (Allocator) 3, (NativeArrayOptions) 1);
    NativeArray<int> nativeArray3 = new NativeArray<int>(length, (Allocator) 3, (NativeArrayOptions) 1);
    NativeArray<int> nativeArray4 = new NativeArray<int>(length, (Allocator) 3, (NativeArrayOptions) 1);
    ActorData[] actorDataArray = new ActorData[length];
    for (int index = 0; index < length; ++index)
      actorDataArray[index] = new ActorData();
    Bench.bench("test_native_vectors");
    for (int index = 0; index < length; ++index)
    {
      Vector3 vector3 = nativeArray1[index];
      vector3.x = (float) index;
      vector3.y = (float) index;
      nativeArray1[index] = vector3;
    }
    for (int index = 0; index < length; ++index)
      nativeArray4[index] = index;
    Bench.benchEnd("test_native_vectors");
    Bench.bench("test_native_x_y");
    for (int index = 0; index < length; ++index)
    {
      nativeArray2[index] = index;
      nativeArray3[index] = index;
    }
    for (int index = 0; index < length; ++index)
      nativeArray4[index] = index;
    Bench.benchEnd("test_native_x_y");
    Bench.bench("test_normal_temp_var");
    for (int index = 0; index < length; ++index)
    {
      ActorData actorData = actorDataArray[index];
      actorData.x = index;
      actorData.y = index;
    }
    for (int index = 0; index < length; ++index)
      actorDataArray[index].health = index;
    Bench.benchEnd("test_normal_temp_var");
    Bench.bench("test_normal_direct");
    for (int index = 0; index < length; ++index)
    {
      actorDataArray[index].x = index;
      actorDataArray[index].y = index;
    }
    for (int index = 0; index < length; ++index)
      actorDataArray[index].health = index;
    Bench.benchEnd("test_normal_direct");
    Debug.Log((object) "-  - - - - - - ");
    Debug.Log((object) ("- BenchTest - test_native_vectors: " + Bench.getBenchResult("test_native_vectors", pAverage: false)));
    Debug.Log((object) ("- BenchTest - test_native_x_y: " + Bench.getBenchResult("test_native_x_y", pAverage: false)));
    Debug.Log((object) ("- BenchTest - test_normal_temp_var: " + Bench.getBenchResult("test_normal_temp_var", pAverage: false)));
    Debug.Log((object) ("- BenchTest - test_normal_direct: " + Bench.getBenchResult("test_normal_direct", pAverage: false)));
    Debug.Log((object) ("- BenchTest - test_job_native_vectors: " + Bench.getBenchResult("test_job_native_vectors", pAverage: false)));
    Debug.Log((object) ("- BenchTest - test_job_native_xy: " + Bench.getBenchResult("test_job_native_xy", pAverage: false)));
    nativeArray1.Dispose();
    nativeArray2.Dispose();
    nativeArray3.Dispose();
    nativeArray4.Dispose();
  }

  public static void benchReferenceVsDict()
  {
  }

  public static void testVirtual()
  {
    int num = 1000;
    BenchTest1 benchTest1 = new BenchTest1();
    BenchTest2 benchTest2 = new BenchTest2();
    Bench.bench("BenchTest - normal");
    for (int index = 0; index < num; ++index)
      benchTest1.test();
    Bench.benchEnd("BenchTest - normal");
    Bench.bench("BenchTest - virtual");
    for (int index = 0; index < num; ++index)
      benchTest2.testVirtual();
    Bench.benchEnd("BenchTest - virtual");
    Debug.Log((object) "Benchmark:");
    Debug.Log((object) ("- BenchTest - normal:" + Bench.getBenchResult("BenchTest - normal")));
    Debug.Log((object) ("- BenchTest - virtual:" + Bench.getBenchResult("BenchTest - virtual")));
  }

  public static void testQueue()
  {
    int num = 10000;
    List<TileType> tileTypeList = new List<TileType>();
    Queue<TileType> tileTypeQueue = new Queue<TileType>();
    LinkedList<TileType> linkedList = new LinkedList<TileType>();
    for (int index = 0; index < num; ++index)
    {
      tileTypeList.Add(new TileType());
      tileTypeQueue.Enqueue(new TileType());
      linkedList.AddLast(new TileType());
    }
    Bench.bench("list");
    for (int index = 0; index < tileTypeList.Count; ++index)
    {
      TileType tileType = tileTypeList[0];
      tileTypeList.RemoveAt(0);
    }
    Bench.benchEnd("list");
    Bench.bench("queue");
    for (int index = 0; index < tileTypeQueue.Count; ++index)
      tileTypeQueue.Dequeue();
    Bench.benchEnd("queue");
    Bench.bench("linked");
    for (int index = 0; index < linkedList.Count; ++index)
    {
      LinkedListNode<TileType> first = linkedList.First;
      linkedList.RemoveFirst();
    }
    Bench.benchEnd("linked");
    Debug.Log((object) ("!!!BENCH REMOVE AT 0 " + num.ToString()));
    Bench.printBenchResult("list");
    Bench.printBenchResult("queue");
    Bench.printBenchResult("linked");
  }

  public static void testRemoveStructs()
  {
    int num1 = 100;
    int num2 = 500;
    List<Vector3> list1 = new List<Vector3>();
    List<Vector3> vector3List1 = new List<Vector3>();
    List<Vector3> vector3List2 = new List<Vector3>();
    List<Vector3> list2 = new List<Vector3>();
    HashSet<Vector3> vector3Set = new HashSet<Vector3>();
    for (int index = 0; index < num2; ++index)
      list1.Add(new Vector3()
      {
        x = (float) Randy.randomInt(0, 1000),
        y = (float) Randy.randomInt(0, 1000),
        z = (float) Randy.randomInt(0, 1000)
      });
    list1.Shuffle<Vector3>();
    for (int index = 0; index < num1; ++index)
      vector3List1.Add(list1.GetRandom<Vector3>());
    Bench.bench("remove");
    foreach (Vector3 vector3 in list1)
      vector3List2.Add(vector3);
    for (int index = 0; index < num1; ++index)
      vector3List2.Remove(vector3List1[index]);
    Bench.benchEnd("remove");
    Bench.bench("RemoveAtSwapBack");
    foreach (Vector3 vector3 in list1)
      list2.Add(vector3);
    for (int index = 0; index < num1; ++index)
      list2.RemoveAtSwapBack<Vector3>(vector3List1[index]);
    Bench.benchEnd("RemoveAtSwapBack");
    Bench.benchEnd("remove_native");
    Bench.bench("remove_hashset");
    foreach (Vector3 vector3 in list1)
      vector3Set.Add(vector3);
    for (int index = 0; index < num1; ++index)
      vector3Set.Remove(vector3List1[index]);
    Bench.benchEnd("remove_hashset");
    Debug.Log((object) "Benchmark:");
    Debug.Log((object) ("- built-in remove:" + Bench.getBenchResult("remove")));
    Debug.Log((object) ("- own RemoveAtSwapBack: " + Bench.getBenchResult("RemoveAtSwapBack")));
    Debug.Log((object) ("- native RemoveAtSwapBack: " + Bench.getBenchResult("remove_native")));
    Debug.Log((object) ("- remove hashset: " + Bench.getBenchResult("remove_hashset")));
  }

  public static void testCapacity()
  {
    int capacity1 = 100;
    int capacity2 = 100000;
    Bench.bench("new_list");
    List<List<int>> intListList1 = new List<List<int>>(capacity1);
    for (int index1 = 0; index1 < capacity1; ++index1)
    {
      List<int> intList = new List<int>();
      intListList1.Add(intList);
      for (int index2 = 0; index2 < capacity2; ++index2)
        intList.Add(index2);
    }
    Bench.benchEnd("new_list");
    Bench.bench("new_list_reused");
    for (int index3 = 0; index3 < intListList1.Count; ++index3)
    {
      List<int> intList = intListList1[index3];
      intList.Clear();
      for (int index4 = 0; index4 < capacity2; ++index4)
        intList.Add(index4);
    }
    Bench.benchEnd("new_list_reused");
    Bench.bench("new_list_set_capacity");
    List<List<int>> intListList2 = new List<List<int>>(capacity1);
    for (int index5 = 0; index5 < capacity1; ++index5)
    {
      List<int> intList = new List<int>(capacity2);
      intListList2.Add(intList);
      for (int index6 = 0; index6 < capacity2; ++index6)
        intList.Add(index6);
    }
    Bench.benchEnd("new_list_set_capacity");
    Bench.bench("new_list_set_capacity_reused");
    for (int index7 = 0; index7 < intListList2.Count; ++index7)
    {
      List<int> intList = intListList2[index7];
      intList.Clear();
      for (int index8 = 0; index8 < capacity2; ++index8)
        intList.Add(index8);
    }
    Bench.benchEnd("new_list_set_capacity_reused");
    Bench.printBenchResult("new_list");
    Bench.printBenchResult("new_list_set_capacity");
    Bench.printBenchResult("new_list_reused");
    Bench.printBenchResult("new_list_set_capacity_reused");
  }
}
