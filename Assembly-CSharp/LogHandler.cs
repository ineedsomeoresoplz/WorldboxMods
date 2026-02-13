// Decompiled with JetBrains decompiler
// Type: LogHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;

#nullable disable
public class LogHandler : MonoBehaviour
{
  private static string folder_base = "/logs";
  private static string dataName = "/error";
  public static string log = "";
  internal static int errorNum = 0;
  private static string lastError = "";
  private static int errorRepeated = 0;
  private static bool _init_handler = false;
  private static bool _init_instance = false;
  private static bool toggledConsole = false;
  private static string _filename = (string) null;
  private static ConcurrentQueue<LogItem> log_queue = new ConcurrentQueue<LogItem>();
  private static LogHandler _instance;

  [RuntimeInitializeOnLoadMethod]
  public static void init()
  {
    if (LogHandler._init_handler)
      return;
    LogHandler._init_handler = true;
    if (!Application.isEditor)
    {
      Application.SetStackTraceLogType((LogType) 3, (StackTraceLogType) 0);
      Application.SetStackTraceLogType((LogType) 2, (StackTraceLogType) 1);
      Application.SetStackTraceLogType((LogType) 0, (StackTraceLogType) 1);
    }
    Application.logMessageReceivedThreaded += new Application.LogCallback(LogHandler.HandleLog);
    Application.logMessageReceivedThreaded += new Application.LogCallback(WorldBoxConsole.Console.HandleLog);
    if (Directory.Exists(LogHandler.getDirPath()))
      return;
    Directory.CreateDirectory(LogHandler.getDirPath());
  }

  [RuntimeInitializeOnLoadMethod]
  public static void initInstance()
  {
    if (LogHandler._init_instance)
      return;
    LogHandler._init_instance = true;
    if (!Object.op_Equality((Object) LogHandler._instance, (Object) null))
      return;
    GameObject gameObject = new GameObject("[LogHandler]");
    LogHandler._instance = gameObject.AddComponent<LogHandler>();
    Object.DontDestroyOnLoad((Object) gameObject);
    ((Object) gameObject).hideFlags = (HideFlags) 52;
  }

  private void Update()
  {
    LogItem result;
    while (LogHandler.log_queue.TryDequeue(out result))
      LogHandler.ProcessLog(result.log, result.stack_trace, result.type);
  }

  private static void HandleLog(string pLogString, string pStackTrace, LogType pLogType)
  {
    if (ThreadHelper.isMainThread())
      LogHandler.ProcessLog(pLogString, pStackTrace, pLogType);
    else
      LogHandler.log_queue.Enqueue(new LogItem(pLogString, pStackTrace, pLogType));
  }

  private static void ProcessLog(string pLogString, string pStackTrace, LogType pLogType)
  {
    pLogString = pLogString.Trim(' ', '\n');
    if (pLogType == null || pLogType == 4 || pLogType == 1)
    {
      if (LogHandler.errorNum > 100)
        return;
      LogHandler.log = "";
      if (LogHandler.errorNum == 0)
      {
        LogHandler.log = $"{LogHandler.log}Game Version: {Application.version}";
        if (!string.IsNullOrEmpty(Config.versionCodeText))
        {
          LogHandler.log = $"{LogHandler.log} ({Config.versionCodeText}";
          if (!string.IsNullOrEmpty(Config.gitCodeText))
            LogHandler.log = $"{LogHandler.log}@{Config.gitCodeText}";
          LogHandler.log += ")";
        }
        LogHandler.log = $"{LogHandler.log}\nModded: {Config.MODDED.ToString()}";
        LogHandler.log = $"{LogHandler.log}\noperatingSystemFamily: {SystemInfo.operatingSystemFamily.ToString()}";
        LogHandler.log = $"{LogHandler.log}\ndeviceModel: {SystemInfo.deviceModel}";
        LogHandler.log = $"{LogHandler.log}\ndeviceName: {SystemInfo.deviceName}";
        LogHandler.log = $"{LogHandler.log}\ndeviceType: {SystemInfo.deviceType.ToString()}";
        LogHandler.log = $"{LogHandler.log}\nsystemMemorySize: {SystemInfo.systemMemorySize.ToString()}";
        LogHandler.log = $"{LogHandler.log}\ngraphicsDeviceID: {SystemInfo.graphicsDeviceID.ToString()}";
        LogHandler.log = $"{LogHandler.log}\ngraphicsActiveTier: {Graphics.activeTier.ToString()}";
        LogHandler.log = $"{LogHandler.log}\nGC.GetTotalMemory: {(GC.GetTotalMemory(false) / 1000000L).ToString()} mb";
        LogHandler.log = $"{LogHandler.log}\ngraphicsMemorySize: {SystemInfo.graphicsMemorySize.ToString()}";
        LogHandler.log = $"{LogHandler.log}\nmaxTextureSize: {SystemInfo.maxTextureSize.ToString()}";
        LogHandler.log = $"{LogHandler.log}\noperatingSystem: {SystemInfo.operatingSystem}";
        LogHandler.log = $"{LogHandler.log}\nprocessorType: {SystemInfo.processorType}";
        LogHandler.log = $"{LogHandler.log}\ninstallMode: {Application.installMode.ToString()}";
        LogHandler.log = $"{LogHandler.log}\nsandboxType: {Application.sandboxType.ToString()}";
        try
        {
          if (Input.anyKey)
          {
            string str1 = "";
            if (HotkeyLibrary.isHoldingAlt())
              str1 += "Alt ";
            if (HotkeyLibrary.isHoldingControlForSelection())
              str1 += "Ctrl ";
            if (HotkeyLibrary.isHoldingAnyMod())
              str1 += "Mod ";
            string[] strArray = new string[9];
            strArray[0] = LogHandler.log;
            strArray[1] = "\nkeyboard: ";
            bool flag = Input.anyKey;
            strArray[2] = flag.ToString();
            strArray[3] = " ";
            flag = Input.anyKeyDown;
            strArray[4] = flag.ToString();
            strArray[5] = " ";
            strArray[6] = Input.inputString;
            strArray[7] = " ";
            strArray[8] = str1;
            LogHandler.log = string.Concat(strArray);
            if (Input.mousePresent)
            {
              string str2 = Input.GetMouseButton(0) ? "press0" : (Input.GetMouseButtonDown(0) ? "down0" : (Input.GetMouseButtonUp(0) ? "up0" : "none1"));
              string str3 = Input.GetMouseButton(1) ? "press1" : (Input.GetMouseButtonDown(1) ? "down1" : (Input.GetMouseButtonUp(1) ? "up1" : "none1"));
              string str4 = Input.GetMouseButton(2) ? "press2" : (Input.GetMouseButtonDown(2) ? "down2" : (Input.GetMouseButtonUp(2) ? "up2" : "none2"));
              string str5 = Input.mousePosition.ToString();
              LogHandler.log = $"{LogHandler.log}\nmouse: {str5} {str2} {str3} {str4}";
            }
          }
        }
        catch (Exception ex)
        {
        }
        LogHandler.log = $"{LogHandler.log}\nFPS: {FPS.fps.ToString()}";
        LogHandler.log += "\n-----------\n\n";
      }
      if (!MemoryExtensions.Trim(MemoryExtensions.AsSpan(pStackTrace)).IsEmpty && pStackTrace == LogHandler.lastError)
        ++LogHandler.errorRepeated;
      else if (MemoryExtensions.Trim(MemoryExtensions.AsSpan(pStackTrace)).IsEmpty && pLogString == LogHandler.lastError)
      {
        ++LogHandler.errorRepeated;
      }
      else
      {
        LogHandler.clearRepeat();
        LogHandler.log = $"{LogHandler.log}- error[{LogHandler.errorNum.ToString()}]: {pLogString}\n";
        LogHandler.log = $"{LogHandler.log}- stack:\n{pStackTrace}\n";
        LogHandler.lastError = pStackTrace;
        File.AppendAllText(LogHandler.getPath(), LogHandler.log);
        ++LogHandler.errorNum;
        LogHandler.openConsole();
      }
    }
    else
    {
      LogHandler.clearRepeat();
      LogHandler.log = $"{LogHandler.log}- trace: {pLogString}\n";
    }
  }

  private static void openConsole()
  {
    if (!Config.show_console_on_error || !Object.op_Inequality((Object) World.world, (Object) null) || !Object.op_Inequality((Object) World.world.console, (Object) null) || LogHandler.toggledConsole)
      return;
    LogHandler.toggledConsole = true;
    World.world.console.Show();
  }

  private static void clearRepeat()
  {
    if (LogHandler.errorRepeated <= 0)
      return;
    LogHandler.log = $"{LogHandler.log}- last error repeated {LogHandler.errorRepeated.ToString()} times\n";
    LogHandler.lastError = "";
    LogHandler.errorRepeated = 0;
  }

  public static string getDirPath() => Application.persistentDataPath + LogHandler.folder_base;

  private static string getPath()
  {
    if (LogHandler._filename == null)
      LogHandler._filename = LogHandler.getFileName();
    return LogHandler._filename;
  }

  private static string getFileName()
  {
    string str = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    return $"{LogHandler.getDirPath()}{LogHandler.dataName}_{str}.log";
  }
}
