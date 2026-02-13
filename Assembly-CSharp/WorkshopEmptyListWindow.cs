// Decompiled with JetBrains decompiler
// Type: WorkshopEmptyListWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorkshopEmptyListWindow : MonoBehaviour
{
  private void OnEnable()
  {
    if (!Config.game_loaded || !WindowHistory.hasHistory())
      return;
    WindowHistory.list.RemoveAt(WindowHistory.list.Count - 1);
  }
}
