// Decompiled with JetBrains decompiler
// Type: AutoSaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPools;

#nullable disable
public static class AutoSaveManager
{
  private static float _time = 300f;
  private static float _interval = 300f;
  private static bool lowMemory = false;
  private static int lastClear = 0;
  private static int low_mem_count = 0;

  public static void update()
  {
    if (AutoSaveManager.lowMemory || !Config.autosaves)
      return;
    if ((double) AutoSaveManager._time > 0.0)
      AutoSaveManager._time -= Time.deltaTime;
    else if (ScrollWindow.isWindowActive() || ControllableUnit.isControllingUnit())
      AutoSaveManager._time += 10f;
    else
      AutoSaveManager.autoSave(pForce: true);
  }

  public static void autoSave(bool pSkipDelete = false, bool pForce = false)
  {
    if (!pForce && ((double) AutoSaveManager._time > 240.0 || (double) Time.realtimeSinceStartup - (double) Config.LAST_LOAD_TIME < 120.0))
      return;
    string autosavesPath = SaveManager.generateAutosavesPath(Math.Truncate(Epoch.Current()).ToString());
    try
    {
      using (AutoSaveManager.getAutoSaves())
      {
        autosavesPath = SaveManager.generateAutosavesPath(Math.Truncate(Epoch.Current()).ToString());
        SaveManager.saveWorldToDirectory(autosavesPath, false);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) "error while auto saving");
      Debug.LogError((object) ex);
      SaveManager.deleteSavePath(autosavesPath);
    }
    try
    {
      if (!pSkipDelete)
        AutoSaveManager.checkClearSaves();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error while clearing saves");
      Debug.LogError((object) ex);
    }
    AutoSaveManager.resetAutoSaveTimer();
  }

  private static void checkClearSaves()
  {
    using (ListPool<AutoSaveData> autoSaves1 = AutoSaveManager.getAutoSaves())
    {
      Dictionary<string, ListPool<AutoSaveData>> autoSavesPerMap = AutoSaveManager.getAutoSavesPerMap(autoSaves1);
      foreach (ListPool<AutoSaveData> list in autoSavesPerMap.Values)
      {
        while (list.Count > 5)
          SaveManager.deleteSavePath(list.Pop<AutoSaveData>().path);
        list.Dispose();
      }
      UnsafeCollectionPool<Dictionary<string, ListPool<AutoSaveData>>, KeyValuePair<string, ListPool<AutoSaveData>>>.Release(autoSavesPerMap);
      if (autoSaves1.Count <= 30)
        return;
      using (ListPool<AutoSaveData> autoSaves2 = AutoSaveManager.getAutoSaves())
      {
        if (autoSaves2.Count <= 30)
          return;
        for (int index = 30; index < autoSaves2.Count; ++index)
          SaveManager.deleteSavePath(autoSaves2[index].path);
      }
    }
  }

  public static void resetAutoSaveTimer() => AutoSaveManager._time = AutoSaveManager._interval;

  public static ListPool<AutoSaveData> getAutoSaves()
  {
    // ISSUE: unable to decompile the method.
  }

  public static Dictionary<string, ListPool<AutoSaveData>> getAutoSavesPerMap(
    ListPool<AutoSaveData> pDatas)
  {
    Dictionary<string, ListPool<AutoSaveData>> autoSavesPerMap = UnsafeCollectionPool<Dictionary<string, ListPool<AutoSaveData>>, KeyValuePair<string, ListPool<AutoSaveData>>>.Get();
    for (int index = 0; index < pDatas.Count; ++index)
    {
      AutoSaveData pData = pDatas[index];
      if (!autoSavesPerMap.ContainsKey(pData.name))
        autoSavesPerMap[pData.name] = new ListPool<AutoSaveData>();
      autoSavesPerMap[pData.name].Add(pData);
    }
    return autoSavesPerMap;
  }

  public static int sorter(AutoSaveData o1, AutoSaveData o2)
  {
    return o2.timestamp.CompareTo(o1.timestamp);
  }

  internal static void OnLowMemory()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading())
      return;
    ++AutoSaveManager.low_mem_count;
    if (AutoSaveManager.low_mem_count < 3)
      return;
    AutoSaveManager.resetAutoSaveTimer();
    int num = (int) Epoch.Current();
    if (AutoSaveManager.lowMemory && AutoSaveManager.lastClear - num < 30)
      return;
    AutoSaveManager.lastClear = num;
    if (!AutoSaveManager.lowMemory)
    {
      Debug.Log((object) "Running out of memory!");
      WorldTip.showNow("Low on memory(RAM)! Disabling auto-saves", false, "top");
    }
    else
    {
      Debug.Log((object) "Running out of memory!");
      WorldTip.showNow("Your device is low on memory(RAM)", false, "top");
    }
    AutoSaveManager.lowMemory = true;
    Config.forceGC("low memory");
  }
}
