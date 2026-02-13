// Decompiled with JetBrains decompiler
// Type: LogText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public class LogText
{
  private static string dataName = "/wb_runtime.log";
  private static bool created = false;
  internal static int offset = 0;

  public static void log(string pEvent, string pInfo = "", string pState = "")
  {
    if (!Globals.DIAGNOSTIC)
      return;
    if (!LogText.created)
    {
      LogText.created = true;
      File.WriteAllText(LogText.getPath(), "");
    }
    switch (pState)
    {
      case "st":
        ++LogText.offset;
        break;
      case "en":
        --LogText.offset;
        break;
    }
    string str = "";
    for (int index = 0; index < LogText.offset; ++index)
      str += " ";
    if (pState == "en")
      str += " ";
    else if (pState == "")
      str += " ";
    if (pState == "en")
      pState = "x";
    else if (pState == "st")
      pState = "!";
    if (pInfo != "")
    {
      pEvent = $"{str}{pEvent} :: {pInfo}";
      if (pState != "")
        pEvent = $"{pEvent} - {pState}";
    }
    File.AppendAllText(LogText.getPath(), pEvent + "\n");
  }

  public static string getPath() => Application.persistentDataPath + LogText.dataName;
}
