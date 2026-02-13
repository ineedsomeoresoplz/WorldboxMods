// Decompiled with JetBrains decompiler
// Type: SelectedTabsHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SelectedTabsHistory
{
  private static Stack<TabHistoryData> _stack = new Stack<TabHistoryData>();

  public static void addToHistory(NanoObject pObject)
  {
    TabHistoryData tabHistoryData1;
    if (SelectedTabsHistory._stack.TryPeek(ref tabHistoryData1) && tabHistoryData1.id == pObject.id && tabHistoryData1.meta_type == pObject.getMetaType())
      return;
    TabHistoryData tabHistoryData2 = new TabHistoryData(pObject);
    SelectedTabsHistory._stack.Push(tabHistoryData2);
  }

  public static bool showPreviousTab()
  {
    TabHistoryData tabHistoryData1;
    if (!SelectedTabsHistory._stack.TryPop(ref tabHistoryData1))
      return false;
    TabHistoryData tabHistoryData2;
    while (SelectedTabsHistory._stack.TryPop(ref tabHistoryData2))
    {
      MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(tabHistoryData2.meta_type);
      NanoObject nanoObject = asset.get(tabHistoryData2.id);
      if (!nanoObject.isRekt())
      {
        if (tabHistoryData2.meta_type == MetaType.Unit)
        {
          SelectedUnit.select(nanoObject as Actor);
          SelectedObjects.setNanoObject((NanoObject) SelectedUnit.unit);
          PowerTabController.showTabSelectedUnit();
        }
        else
          asset.selectAndInspect(nanoObject, pCheckNameplate: false);
        return true;
      }
    }
    return false;
  }

  public static bool hasHistory() => SelectedTabsHistory._stack.Count > 0;

  public static int count()
  {
    int num = 0;
    foreach (TabHistoryData tabHistoryData in SelectedTabsHistory._stack)
    {
      if (!tabHistoryData.getNanoObject().isRekt())
        ++num;
    }
    return num;
  }

  public static TabHistoryData? getPrevData()
  {
    int num1 = 1;
label_1:
    int num2 = 0;
    using (Stack<TabHistoryData>.Enumerator enumerator = SelectedTabsHistory._stack.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        TabHistoryData current = enumerator.Current;
        if (num1 != num2)
        {
          ++num2;
        }
        else
        {
          if (!current.getNanoObject().isRekt())
            return new TabHistoryData?(current);
          ++num1;
          if (num1 > SelectedTabsHistory._stack.Count - 1)
            return new TabHistoryData?();
          break;
        }
      }
      goto label_1;
    }
  }

  public static void clear() => SelectedTabsHistory._stack.Clear();
}
