// Decompiled with JetBrains decompiler
// Type: AISystemDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public static class AISystemDebug
{
  private static string dataName = "/ai_system.log";
  private static List<string> debug_list_actions = new List<string>();

  public static void clear() => AISystemDebug.debug_list_actions.Clear();

  public static void debugLog(string pString)
  {
    AISystemDebug.debug_list_actions.Add(pString);
    if (AISystemDebug.debug_list_actions.Count <= 1000)
      return;
    AISystemDebug.debug_list_actions.RemoveAt(0);
  }

  public static void log()
  {
    string contents = "";
    foreach (string debugListAction in AISystemDebug.debug_list_actions)
      contents = $"{contents}{debugListAction}\n";
    File.WriteAllText(AISystemDebug.getPath(), contents);
  }

  public static string getPath() => Application.persistentDataPath + AISystemDebug.dataName;
}
