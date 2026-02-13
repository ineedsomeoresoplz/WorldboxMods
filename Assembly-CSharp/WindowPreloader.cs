// Decompiled with JetBrains decompiler
// Type: WindowPreloader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WindowPreloader
{
  private static bool _windows_preloaded;
  private static readonly Dictionary<string, ScrollWindow> _preloaded_windows = new Dictionary<string, ScrollWindow>();
  private static readonly List<ResourceRequest> _windows_resources_requests = new List<ResourceRequest>();
  private static readonly Dictionary<string, AsyncInstantiateOperation<ScrollWindow>> _windows_preloading_operations = new Dictionary<string, AsyncInstantiateOperation<ScrollWindow>>();
  private static readonly Dictionary<string, ListPool<GameObject>> _windows_tabs_objects = new Dictionary<string, ListPool<GameObject>>();
  private static readonly List<string> _windows_preload_list = new List<string>();

  public static void addWindowPreloadResources()
  {
    if (WindowPreloader._windows_preloaded || !Config.preload_windows)
      return;
    foreach (WindowAsset windowAsset in AssetManager.window_library.list)
    {
      if (windowAsset.preload)
        WindowPreloader._windows_preload_list.Add(windowAsset.id);
    }
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      foreach (string windowsPreload in WindowPreloader._windows_preload_list)
        WindowPreloader.preloadWindowPrefab(windowsPreload);
    }), WindowPreloader.c("Preloading windows", 1, 5), true);
  }

  public static void addWaitForWindowResources()
  {
    if (WindowPreloader._windows_preloaded)
      return;
    SmoothLoader.add(new MapLoaderAction(WindowPreloader.finishPreloadingWindowsResources), WindowPreloader.c("Preloading windows", 2, 5), true);
  }

  private static void finishPreloadingWindowsResources()
  {
    foreach (AsyncOperation resourcesRequest in WindowPreloader._windows_resources_requests)
    {
      if (!resourcesRequest.isDone)
      {
        WindowPreloader.addWaitForWindowResources();
        return;
      }
    }
    WindowPreloader.addInstantiateWindows();
  }

  private static void addInstantiateWindows()
  {
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      foreach (string windowsPreload in WindowPreloader._windows_preload_list)
        WindowPreloader.prepareWindowPrefab(windowsPreload);
    }), WindowPreloader.c("Preloading windows", 3, 5), true);
    int num = 0;
    int count = WindowPreloader._windows_preload_list.Count;
    foreach (string windowsPreload in WindowPreloader._windows_preload_list)
    {
      string tWindowID = windowsPreload;
      SmoothLoader.add((MapLoaderAction) (() => WindowPreloader.instantiateWindow(tWindowID)), WindowPreloader.c(WindowPreloader.c("Preloading windows", 4, 5), ++num, count));
    }
    SmoothLoader.add(new MapLoaderAction(WindowPreloader.finishPreloadingWindows), WindowPreloader.c("Preloading windows", 5, 5), true, pToEnd: true);
  }

  private static void finishPreloadingWindows()
  {
    foreach (AsyncInstantiateOperation<ScrollWindow> instantiateOperation in WindowPreloader._windows_preloading_operations.Values)
    {
      if (!instantiateOperation.isDone)
        instantiateOperation.WaitForCompletion();
    }
    foreach (string key in WindowPreloader._windows_preloading_operations.Keys)
    {
      ScrollWindow pWindow = WindowPreloader._windows_preloading_operations[key].Result.First<ScrollWindow>();
      WindowPreloader.finishPreloadingWindow(key, pWindow);
      WindowPreloader.restoreWindowPrefab(key);
    }
    WindowPreloader.finalizeWindowsPreloading();
  }

  public static bool TryGetPreloadedWindow(string pWindowID, out ScrollWindow tScrollWindow)
  {
    if (!WindowPreloader._preloaded_windows.TryGetValue(pWindowID, out tScrollWindow))
      return false;
    WindowPreloader._preloaded_windows.Remove(pWindowID);
    return true;
  }

  private static void preloadWindowPrefab(string pWindowID)
  {
    string str = ScrollWindow.checkWindowID(pWindowID);
    if (WindowPreloader._preloaded_windows.ContainsKey(pWindowID))
      return;
    ResourceRequest resourceRequest = Resources.LoadAsync("windows/" + str, typeof (ScrollWindow));
    WindowPreloader._windows_resources_requests.Add(resourceRequest);
  }

  private static ScrollWindow getWindowPrefab(string pWindowID)
  {
    string str = ScrollWindow.checkWindowID(pWindowID);
    ScrollWindow windowPrefab = (ScrollWindow) Resources.Load("windows/" + str, typeof (ScrollWindow));
    if (Object.op_Equality((Object) windowPrefab, (Object) null))
    {
      Debug.LogError((object) $"Window with id {str} not found!");
      windowPrefab = (ScrollWindow) Resources.Load("windows/not_found", typeof (ScrollWindow));
    }
    return windowPrefab;
  }

  private static void prepareWindowPrefab(string pWindowID)
  {
    ScrollWindow windowPrefab = WindowPreloader.getWindowPrefab(pWindowID);
    ((Component) windowPrefab).gameObject.SetActive(false);
    WindowPreloader._windows_tabs_objects[pWindowID] = ScrollWindow.disableTabsInPrefab(windowPrefab);
  }

  private static void instantiateWindow(string pWindowID)
  {
    AsyncInstantiateOperation<ScrollWindow> instantiateOperation = Object.InstantiateAsync<ScrollWindow>(WindowPreloader.getWindowPrefab(pWindowID), CanvasMain.instance.transformWindows);
    WindowPreloader._windows_preloading_operations.Add(pWindowID, instantiateOperation);
  }

  private static void restoreWindowPrefab(string pWindowID)
  {
    ScrollWindow windowPrefab = WindowPreloader.getWindowPrefab(pWindowID);
    ScrollWindow.enableTabsInPrefab(WindowPreloader._windows_tabs_objects[pWindowID]);
    ((Component) windowPrefab).gameObject.SetActive(true);
  }

  private static void finishPreloadingWindow(string pWindowID, ScrollWindow pWindow)
  {
    ((Object) ((Component) pWindow).gameObject).name = pWindowID;
    pWindow.init();
    WindowPreloader._preloaded_windows.Add(pWindowID, pWindow);
  }

  private static void finalizeWindowsPreloading()
  {
    WindowPreloader._windows_preloaded = true;
    WindowPreloader._windows_tabs_objects.Clear();
    WindowPreloader._windows_preloading_operations.Clear();
    WindowPreloader._windows_resources_requests.Clear();
    WindowPreloader._windows_preload_list.Clear();
  }

  private static string c(string pString, int pStep, int pMax) => $"{pString} ({pStep}/{pMax})";
}
