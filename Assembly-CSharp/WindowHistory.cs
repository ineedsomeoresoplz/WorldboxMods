// Decompiled with JetBrains decompiler
// Type: WindowHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class WindowHistory
{
  public static readonly List<WindowHistoryData> list = new List<WindowHistoryData>();
  public static Action historyClearCallback;

  public static void clear()
  {
    if (WindowHistory.historyClearCallback != null)
    {
      WindowHistory.historyClearCallback();
      WindowHistory.historyClearCallback = (Action) null;
    }
    WindowHistory.list.Clear();
  }

  public static void addIntoHistory(ScrollWindow pWindow)
  {
    WindowHistoryData pHistoryData = new WindowHistoryData()
    {
      index = WindowHistory.list.Count + 1,
      window = pWindow
    };
    foreach (MetaTypeAsset metaTypeAsset in AssetManager.meta_type_library.list)
    {
      MetaTypeHistoryAction historyActionUpdate = metaTypeAsset.window_history_action_update;
      if (historyActionUpdate != null)
        historyActionUpdate(ref pHistoryData);
    }
    WindowHistory.list.Add(pHistoryData);
  }

  public static void popHistory()
  {
    List<WindowHistoryData> list = WindowHistory.list;
    if (list == null)
      return;
    list.Pop<WindowHistoryData>();
  }

  public static void clickBack()
  {
    if (ScrollWindow.getCurrentWindow().historyActionEnabled && WindowHistory.returnWindowBack())
      return;
    ScrollWindow.hideAllEvent();
  }

  private static bool returnWindowBack()
  {
    if (!WindowHistory.canReturnWindowBack())
      return false;
    WindowHistoryData pHistoryData = WindowHistory.list.Pop<WindowHistoryData>();
    while (WindowHistory.list.Count > 0)
    {
      pHistoryData = WindowHistory.list.Pop<WindowHistoryData>();
      MetaTypeAsset metaTypeAsset = AssetManager.window_library.get(pHistoryData.window.screen_id).meta_type_asset;
      if (metaTypeAsset != null)
      {
        MetaTypeHistoryAction historyActionRestore = metaTypeAsset.window_history_action_restore;
        if (historyActionRestore != null)
          historyActionRestore(ref pHistoryData);
      }
      if (!pHistoryData.window.shouldClose())
        break;
    }
    if (pHistoryData.window.shouldClose())
      return false;
    pHistoryData.window.clickShowLeft();
    return true;
  }

  public static bool canReturnWindowBack()
  {
    return !WorkshopUploadingWorldWindow.uploading && WindowHistory.list.Count >= 2;
  }

  public static bool hasHistory() => WindowHistory.list.Count > 0;

  public static void debug(DebugTool pTool)
  {
    pTool.setText("hasHistory:", (object) WindowHistory.hasHistory());
    pTool.setText("canReturnWindowBack:", (object) WindowHistory.canReturnWindowBack());
    pTool.setText("list.Count:", (object) WindowHistory.list.Count);
    foreach (WindowHistoryData windowHistoryData in WindowHistory.list)
    {
      using (ListPool<string> values = new ListPool<string>())
      {
        Kingdom kingdom = windowHistoryData.kingdom;
        // ISSUE: explicit non-virtual call
        if ((kingdom != null ? (__nonvirtual (kingdom.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.kingdom.getTypeID());
        Culture culture = windowHistoryData.culture;
        // ISSUE: explicit non-virtual call
        if ((culture != null ? (__nonvirtual (culture.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.culture.getTypeID());
        Actor unit = windowHistoryData.unit;
        // ISSUE: explicit non-virtual call
        if ((unit != null ? (__nonvirtual (unit.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.unit.getTypeID());
        City city = windowHistoryData.city;
        // ISSUE: explicit non-virtual call
        if ((city != null ? (__nonvirtual (city.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.city.getTypeID());
        Clan clan = windowHistoryData.clan;
        // ISSUE: explicit non-virtual call
        if ((clan != null ? (__nonvirtual (clan.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.clan.getTypeID());
        Plot plot = windowHistoryData.plot;
        // ISSUE: explicit non-virtual call
        if ((plot != null ? (__nonvirtual (plot.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.plot.getTypeID());
        War war = windowHistoryData.war;
        // ISSUE: explicit non-virtual call
        if ((war != null ? (__nonvirtual (war.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.war.getTypeID());
        Alliance alliance = windowHistoryData.alliance;
        // ISSUE: explicit non-virtual call
        if ((alliance != null ? (__nonvirtual (alliance.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.alliance.getTypeID());
        Language language = windowHistoryData.language;
        // ISSUE: explicit non-virtual call
        if ((language != null ? (__nonvirtual (language.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.language.getTypeID());
        Subspecies subspecies = windowHistoryData.subspecies;
        // ISSUE: explicit non-virtual call
        if ((subspecies != null ? (__nonvirtual (subspecies.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.subspecies.getTypeID());
        Religion religion = windowHistoryData.religion;
        // ISSUE: explicit non-virtual call
        if ((religion != null ? (__nonvirtual (religion.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.religion.getTypeID());
        Family family = windowHistoryData.family;
        // ISSUE: explicit non-virtual call
        if ((family != null ? (__nonvirtual (family.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.family.getTypeID());
        Army army = windowHistoryData.army;
        // ISSUE: explicit non-virtual call
        if ((army != null ? (__nonvirtual (army.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.army.getTypeID());
        Item obj = windowHistoryData.item;
        // ISSUE: explicit non-virtual call
        if ((obj != null ? (__nonvirtual (obj.isAlive()) ? 1 : 0) : 0) != 0)
          values.Add(windowHistoryData.item.getTypeID());
        pTool.setText($"history {windowHistoryData.index} {windowHistoryData.window.screen_id}:", (object) string.Join(",", (IEnumerable<string>) values));
      }
    }
  }
}
