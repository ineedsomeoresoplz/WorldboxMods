// Decompiled with JetBrains decompiler
// Type: ButtonGraphListCompare
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButtonGraphListCompare : MonoBehaviour
{
  public void compareListItems()
  {
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    IComponentList componentInChildren = ((Component) currentWindow).GetComponentInChildren<IComponentList>(true);
    if (componentInChildren == null)
    {
      Debug.LogError((object) ("IComponentList missing in " + ((Object) ((Component) currentWindow).gameObject).name), (Object) ((Component) currentWindow).gameObject);
    }
    else
    {
      using (ListPool<NanoObject> elements = componentInChildren.getElements())
      {
        if (elements.Count > 0)
        {
          Config.selected_objects_graph.Clear();
          for (int index = 0; index < elements.Count && index < 3; ++index)
          {
            NanoObject pObject = elements[index];
            Config.selected_objects_graph.Add(pObject);
          }
        }
        ScrollWindow.showWindow("chart_comparer");
      }
    }
  }
}
