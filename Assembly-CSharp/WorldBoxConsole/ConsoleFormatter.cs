// Decompiled with JetBrains decompiler
// Type: WorldBoxConsole.ConsoleFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
namespace WorldBoxConsole;

public class ConsoleFormatter
{
  private static string log;
  private static string start;
  private static string end;
  private static string build = "";
  private static Regex _regex = new Regex("[\\d\\.]+");

  public static string logWarning(int pWarningNum, string pLogString)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.AppendLine().Append("<color=yellow>--- warning[").Append(pWarningNum).Append($"]: ---{ConsoleFormatter.build}</color>").AppendLine();
      foreach (string str in pLogString.Trim().Split('\n', StringSplitOptions.None))
        stringBuilderPool.Append("<b><color=cyan>").Append(str).Append("</color></b>").AppendLine();
      return stringBuilderPool.ToString();
    }
  }

  public static string logError(int pErrorNum, string pLogString, string pStackTrace)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      string str1;
      try
      {
        str1 = ConsoleFormatter.getShortGameplayStateInfo();
      }
      catch (Exception ex)
      {
        str1 = "(gameplay state crashed)";
      }
      foreach (string str2 in str1.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        stringBuilderPool.Append("<size=7><b><color=#df4ec8>").Append(str2).Append("</color></b></size>").AppendLine();
      stringBuilderPool.Append("<color=red>--- error[").Append(pErrorNum).Append("]: ---").Append(ConsoleFormatter.build).Append("</color>").AppendLine();
      foreach (string str3 in pLogString.Trim().Split('\n', StringSplitOptions.None))
        stringBuilderPool.Append("<b><color=cyan>").Append(str3).Append("</color></b>").AppendLine();
      if (!string.IsNullOrEmpty(pStackTrace.Trim('\n', ' ')))
      {
        try
        {
          pStackTrace = ConsoleFormatter.formatStacktrace(pStackTrace);
        }
        catch (Exception ex)
        {
        }
        stringBuilderPool.Append("<color=red>--- stack: ---").Append(ConsoleFormatter.build).Append("</color>").AppendLine().Append(pStackTrace).AppendLine();
      }
      return stringBuilderPool.ToString();
    }
  }

  public static string addSystemInfo()
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append("-----------").AppendLine().Append("Game Version: <color=white>").Append(Application.version).Append("</color>");
      ConsoleFormatter.build = " " + Application.version;
      if (!string.IsNullOrEmpty(Config.versionCodeText))
      {
        stringBuilderPool.Append(" (<color=white>").Append(Config.versionCodeText);
        if (!string.IsNullOrEmpty(Config.gitCodeText))
          stringBuilderPool.Append("@").Append(Config.gitCodeText);
        stringBuilderPool.Append("</color>)");
        ConsoleFormatter.build = $"{ConsoleFormatter.build} ({Config.versionCodeText}";
        if (!string.IsNullOrEmpty(Config.gitCodeText))
          ConsoleFormatter.build = $"{ConsoleFormatter.build}@{Config.gitCodeText}";
        ConsoleFormatter.build += ")";
      }
      ConsoleFormatter.build += " ---";
      stringBuilderPool.AppendLine().Append("Modded: <color=white>").Append(Config.MODDED).Append("</color>").AppendLine().Append("operatingSystemFamily: <color=white>").Append((object) SystemInfo.operatingSystemFamily).Append("</color>").AppendLine().Append("deviceModel: <color=white>").Append(SystemInfo.deviceModel).Append("</color>").AppendLine().Append("deviceName: <color=white>").Append(SystemInfo.deviceName).Append("</color>").AppendLine().Append("deviceType: <color=white>").Append((object) SystemInfo.deviceType).Append("</color>").AppendLine().Append("systemMemorySize: <color=white>").Append(SystemInfo.systemMemorySize).Append("</color>").AppendLine().Append("graphicsDeviceID: <color=white>").Append(SystemInfo.graphicsDeviceID).Append("</color>").AppendLine().Append("Graphics.activeTier: <color=white>").Append(Graphics.activeTier.ToString()).Append("</color>").AppendLine().Append("GC.GetTotalMemory: <color=white>").Append((GC.GetTotalMemory(false) / 1000000L).ToString() + " mb").Append("</color>").AppendLine().Append("graphicsMemorySize: <color=white>").Append(SystemInfo.graphicsMemorySize).Append("</color>").AppendLine().Append("maxTextureSize: <color=white>").Append(SystemInfo.maxTextureSize).Append("</color>").AppendLine().Append("operatingSystem: <color=white>").Append(SystemInfo.operatingSystem).Append("</color>").AppendLine().Append("processorType: <color=white>").Append(SystemInfo.processorType).Append("</color>").AppendLine().Append("installMode: <color=white>").Append((object) Application.installMode).Append("</color>").AppendLine().Append("sandboxType: <color=white>").Append((object) Application.sandboxType).Append("</color>").AppendLine().Append("FPS: <color=white>").Append(FPS.fps).Append("</color>").AppendLine().Append("-----------");
      return stringBuilderPool.ToString();
    }
  }

  public static string logFormatter(string pLogString, string pColor = "white")
  {
    pLogString = pLogString.Trim(' ', '\n');
    return pLogString != "" && ConsoleFormatter.HasDigit(pLogString) && !pLogString.Contains("<color") ? ConsoleFormatter._regex.Replace(pLogString, $"<color={pColor}>$0</color>") : pLogString;
  }

  private static bool HasDigit(string pString)
  {
    foreach (char c in pString)
    {
      if (char.IsDigit(c))
        return true;
    }
    return false;
  }

  public static string formatStacktrace(string pStackTrace)
  {
    string[] strArray1 = pStackTrace.Split('\n', StringSplitOptions.None);
    for (int index1 = 0; index1 < strArray1.Length; ++index1)
    {
      if (strArray1[index1].Contains("(at "))
      {
        string[] strArray2 = strArray1[index1].Split(new string[1]
        {
          " (at "
        }, StringSplitOptions.None);
        ConsoleFormatter.start = strArray2[0];
        ConsoleFormatter.end = strArray2[1].Substring(0, strArray2[1].Length - 1);
      }
      else
      {
        ConsoleFormatter.start = strArray1[index1];
        ConsoleFormatter.end = "";
      }
      if (ConsoleFormatter.start.Contains("("))
      {
        string[] strArray3 = ConsoleFormatter.start.Split('(', StringSplitOptions.None);
        string str1 = strArray3[0];
        string str2 = strArray3[1].Substring(0, strArray3[1].Length - 1);
        char? nullable = new char?();
        if (str1.Contains(":"))
          nullable = new char?(':');
        else if (str1.Contains("."))
          nullable = new char?('.');
        if (nullable.HasValue)
        {
          string[] strArray4 = str1.Split(nullable.Value, StringSplitOptions.None);
          strArray4[strArray4.Length - 1] = $"<b><color=cyan>{strArray4[strArray4.Length - 1]}</color></b>";
          str1 = string.Join(nullable.Value.ToString(), strArray4);
        }
        if (str2.Trim() != string.Empty)
        {
          string[] strArray5;
          if (str2.Contains(","))
            strArray5 = str2.Split(',', StringSplitOptions.None);
          else
            strArray5 = new string[1]{ str2 };
          for (int index2 = 0; index2 < strArray5.Length; ++index2)
          {
            string str3 = strArray5[index2].Trim();
            if (str3.Contains(' '))
            {
              string[] strArray6 = str3.Split(' ', StringSplitOptions.None);
              string str4 = strArray6[0];
              if (str4.Contains("."))
              {
                string[] strArray7 = str4.Split('.', StringSplitOptions.None);
                str4 = strArray7[strArray7.Length - 1];
              }
              string str5 = strArray6[1];
              strArray5[index2] = $"<color=#FFCC1C>{str4}</color> <b><color=cyan>{str5}</color></b>";
            }
            else
              strArray5[index2] = $"<color=#FFCC1C>{str3}</color>";
            str2 = string.Join(", ", strArray5);
          }
        }
        ConsoleFormatter.start = $"{str1}({str2})";
        while (ConsoleFormatter.start.Contains("System."))
          ConsoleFormatter.start = ConsoleFormatter.start.Replace("System.", string.Empty);
      }
      if (ConsoleFormatter.end != string.Empty)
      {
        if (ConsoleFormatter.end.Contains("BuiltInPackages/"))
          ConsoleFormatter.end = ConsoleFormatter.end.Split(new string[1]
          {
            "BuiltInPackages/"
          }, StringSplitOptions.None)[1];
        if (ConsoleFormatter.end.Contains("unity/build/"))
          ConsoleFormatter.end = ConsoleFormatter.end.Split(new string[1]
          {
            "unity/build/"
          }, StringSplitOptions.None)[1];
        if (ConsoleFormatter.end.Contains("Unity.app/"))
          ConsoleFormatter.end = ConsoleFormatter.end.Split(new string[1]
          {
            "Unity.app/"
          }, StringSplitOptions.None)[1];
        if (ConsoleFormatter.end.Contains("Export/"))
          ConsoleFormatter.end = ConsoleFormatter.end.Split(new string[1]
          {
            "Export/"
          }, StringSplitOptions.None)[1];
        if (ConsoleFormatter.end.Contains("github/workspace/"))
          ConsoleFormatter.end = ConsoleFormatter.end.Split(new string[1]
          {
            "github/workspace/"
          }, StringSplitOptions.None)[1];
        if (ConsoleFormatter.end.Contains(":"))
        {
          string[] strArray8 = ConsoleFormatter.end.Split(':', StringSplitOptions.None);
          string[] strArray9 = strArray8[strArray8.Length - 2].Split('/', StringSplitOptions.None);
          strArray9[strArray9.Length - 1] = $"<size=7><b><color=cyan>{strArray9[strArray9.Length - 1]}</color></b></size>";
          strArray8[strArray8.Length - 2] = string.Join("/", strArray9);
          strArray8[strArray8.Length - 1] = $"<size=7><b><color=cyan>{strArray8[strArray8.Length - 1]}</color></b></size>";
          ConsoleFormatter.end = string.Join(":", strArray8);
        }
        ConsoleFormatter.end = $"<size=5> (at {ConsoleFormatter.end})</size>";
      }
      strArray1[index1] = $"<size=7>{ConsoleFormatter.start}</size>{ConsoleFormatter.end}";
    }
    pStackTrace = string.Join("\n", strArray1);
    return pStackTrace;
  }

  private static string getShortGameplayStateInfo()
  {
    MapBox instance = MapBox.instance;
    if (Object.op_Equality((Object) instance, (Object) null))
      return "(world not loaded)";
    WindowStats debugWindowStats = Config.debug_window_stats;
    bool? nullable = instance.quality_changer?.isLowRes();
    string id1 = PowerButtonSelector.instance?.selectedButton?.godPower?.id;
    string selectedPowerButton = Config.debug_last_selected_power_button;
    bool flag1 = SelectedUnit.isSet();
    bool flag2 = ControllableUnit.isControllingUnit();
    string str1 = Config.time_scale_asset?.id ?? "null";
    int debugWorldsLoaded = Config.debug_worlds_loaded;
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      stringBuilderPool1.Append($"spd: <H>{str1}</H>");
      if (!string.IsNullOrEmpty(id1) || !string.IsNullOrEmpty(selectedPowerButton))
      {
        stringBuilderPool1.Append(", ");
        stringBuilderPool1.Append($"pow: <H>{id1 ?? "none"}</H>");
        if (selectedPowerButton != id1)
          stringBuilderPool1.Append($" last: <H>{selectedPowerButton ?? "none"}</H>");
      }
      stringBuilderPool1.Append(", ");
      stringBuilderPool1.Append("zoom: <H>");
      stringBuilderPool1.Append(nullable.HasValue ? (nullable.Value ? "map" : "full") : "null");
      stringBuilderPool1.Append("</H>");
      stringBuilderPool1.Append(", ");
      stringBuilderPool1.Append($"win: <H>{debugWindowStats.current ?? "none"}</H> (<H>{debugWindowStats.previous ?? "none"}</H>)");
      stringBuilderPool1.Append($" (o:{debugWindowStats.opens},c:{debugWindowStats.closes},s:{debugWindowStats.shows},h:{debugWindowStats.hides})");
      stringBuilderPool1.Append(", ");
      stringBuilderPool1.Append($"worlds: {debugWorldsLoaded}");
      stringBuilderPool1.Append(", ");
      stringBuilderPool1.Append($"modded: <H>{Config.MODDED}</H>");
      stringBuilderPool1.Append(", ");
      stringBuilderPool1.Append($"db pend: <H>{DBInserter.hasCommands()}</H>");
      stringBuilderPool1.AppendLine();
      using (StringBuilderPool stringBuilderPool2 = new StringBuilderPool())
      {
        foreach (BaseSystemManager listAllSimManager in MapBox.instance.list_all_sim_managers)
        {
          string str2 = listAllSimManager.debugShort();
          if (!string.IsNullOrEmpty(str2))
          {
            if (stringBuilderPool2.Length > 0)
              stringBuilderPool2.Append(", ");
            stringBuilderPool2.Append(str2);
            if (stringBuilderPool2.Length > 78)
            {
              stringBuilderPool1.Append(stringBuilderPool2.ToString());
              stringBuilderPool1.AppendLine();
              stringBuilderPool2.Clear();
            }
          }
        }
        if (stringBuilderPool2.Length > 0)
        {
          stringBuilderPool1.Append(stringBuilderPool2.ToString());
          stringBuilderPool1.AppendLine();
        }
        using (StringBuilderPool stringBuilderPool3 = new StringBuilderPool())
        {
          if (flag1)
          {
            string id2 = SelectedUnit.unit?.asset?.id;
            stringBuilderPool3.Append($"selected: <H>{id2}</H>");
            if (SelectedUnit.multipleSelected())
            {
              int num = SelectedUnit.countSelected();
              stringBuilderPool3.Append($" ({num})");
            }
          }
          if (flag2)
          {
            if (stringBuilderPool3.Length > 0)
              stringBuilderPool3.Append(", ");
            string id3 = ControllableUnit.getControllableUnit()?.asset?.id;
            int num = ControllableUnit.count();
            stringBuilderPool3.Append($"controlling: <H>{id3}</H>");
            if (num > 1)
              stringBuilderPool3.Append($" ({num})");
          }
          if (stringBuilderPool3.Length > 0)
          {
            stringBuilderPool1.Append(stringBuilderPool3.ToString());
            stringBuilderPool1.AppendLine();
          }
          return ConsoleFormatter.logFormatter(stringBuilderPool1.ToString(), "yellow").Replace("<H>", "<color=yellow>").Replace("</H>", "</color>");
        }
      }
    }
  }

  private static string getWindowInfo()
  {
    if (!ScrollWindow.isWindowActive())
      return Config.debug_last_window;
    return ScrollWindow.getCurrentWindow()?.screen_id;
  }
}
