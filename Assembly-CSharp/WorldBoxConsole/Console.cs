// Decompiled with JetBrains decompiler
// Type: WorldBoxConsole.Console
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace WorldBoxConsole;

public class Console : MonoBehaviour
{
  private const int MAX_ELEMENTS = 2500;
  private const int MAX_LINES = 25000;
  private const int MAX_CHARS_PER_LINE = 256 /*0x0100*/;
  private static int _line_num = 0;
  private static int _warning_num = 0;
  private static int _error_num = 0;
  private UnityEngine.UI.Text _prefab;
  private static Queue<string> _texts = new Queue<string>(2500);
  private static StringBuilder _log = new StringBuilder();
  private static int _error_repeated = 0;
  private static int _warning_repeated = 0;
  private static Dictionary<string, int> _previous_errors = new Dictionary<string, int>();
  private static Dictionary<string, int> _previous_warnings = new Dictionary<string, int>();
  private static HashSet<string> _stacks = new HashSet<string>();
  private RectTransform _text_group;
  private List<UnityEngine.UI.Text> _text_obj = new List<UnityEngine.UI.Text>();
  private ObjectPoolGenericMono<UnityEngine.UI.Text> _pool_texts;
  private static ConcurrentQueue<LogItem> log_queue = new ConcurrentQueue<LogItem>();

  private void Awake()
  {
    this._text_group = ((Component) this).transform.Find("Scroll View/Viewport/Content") as RectTransform;
    this._prefab = ((Component) ((Transform) this._text_group).Find("CText")).GetComponent<UnityEngine.UI.Text>();
    this._prefab.text = "";
    this._pool_texts = new ObjectPoolGenericMono<UnityEngine.UI.Text>(this._prefab, (Transform) this._text_group);
    ((Component) this._prefab).gameObject.SetActive(false);
  }

  private void addText()
  {
    UnityEngine.UI.Text next = this._pool_texts.getNext();
    ((Object) next).name = "CText " + (this._text_obj.Count + 1).ToString();
    RectTransform component = ((Component) next).GetComponent<RectTransform>();
    ((Transform) component).localScale = Vector3.one;
    ((Transform) component).localPosition = Vector3.zero;
    this._text_obj.Add(next);
    this.truncateGameObjects();
  }

  private void truncateGameObjects()
  {
    while (this._text_obj.Count > 2500)
    {
      this._text_obj[0].text = "";
      this._pool_texts.release(this._text_obj[0]);
      this._text_obj.RemoveAt(0);
    }
  }

  internal static bool hasErrors() => Console._error_num > 0;

  private static void truncateTexts()
  {
    if (Console._texts.Count <= 2500)
      return;
    while (Console._texts.Count > 2500)
      Console._texts.Dequeue();
    Console._line_num = 0;
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    Console._line_num = 0;
    this._text_group.SetBottom(0.0f);
  }

  private void OnDisable()
  {
    Console._line_num = 0;
    foreach (UnityEngine.UI.Text text in this._text_obj)
      text.text = "";
    this._pool_texts.clear();
    this._text_obj.Clear();
  }

  public void Toggle()
  {
    if (((Component) this).gameObject.activeSelf)
      ((Component) this).gameObject.SetActive(false);
    else
      ((Component) this).gameObject.SetActive(true);
  }

  public void Hide() => ((Component) this).gameObject.SetActive(false);

  public void Show() => ((Component) this).gameObject.SetActive(true);

  public bool isActive() => ((Component) this).gameObject.activeSelf;

  public static void HandleLog(string pLogString, string pStackTrace, LogType pLogType)
  {
    if (ThreadHelper.isMainThread())
      Console.ProcessLog(pLogString, pStackTrace, pLogType, DateTime.Now);
    else
      Console.log_queue.Enqueue(new LogItem(pLogString, pStackTrace, pLogType));
  }

  public static void ProcessLog(
    string pLogString,
    string pStackTrace,
    LogType pLogType,
    DateTime pTime)
  {
    if (pLogString.Contains("FIRAPP_DEFAULT"))
      return;
    if (pLogString.Length > 256 /*0x0100*/ && !pLogString.Contains("</"))
    {
      string[] strArray = pLogString.Split('\n', StringSplitOptions.None);
      using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          while (strArray[index].Length > 256 /*0x0100*/)
          {
            stringBuilderPool.Append(strArray[index].Substring(0, 256 /*0x0100*/));
            stringBuilderPool.Append('\n');
            strArray[index] = strArray[index].Substring(256 /*0x0100*/);
          }
          stringBuilderPool.Append(strArray[index]);
          stringBuilderPool.Append('\n');
        }
        pLogString = stringBuilderPool.ToString();
      }
    }
    pLogString = pLogString.Trim(' ', '\n');
    pLogString = ConsoleFormatter.logFormatter(pLogString);
    switch ((int) pLogType)
    {
      case 0:
      case 1:
      case 4:
        if (Console._error_num == 0)
          Console._texts.Enqueue(ConsoleFormatter.addSystemInfo().Trim('\n', ' '));
        if (Console._previous_errors.ContainsKey(pLogString))
        {
          Console._previous_errors[pLogString]++;
          ++Console._error_repeated;
          Console._log.Clear();
          return;
        }
        Console.clearRepeat();
        if (!Console._stacks.Add(pStackTrace))
          pStackTrace = "";
        Console._log.Append(ConsoleFormatter.logError(Console._error_num, pLogString, pStackTrace));
        Console._previous_errors.Add(pLogString, 1);
        ++Console._error_num;
        break;
      case 2:
        if (Console._previous_warnings.ContainsKey(pLogString))
        {
          Console._previous_warnings[pLogString]++;
          ++Console._warning_repeated;
          Console._log.Clear();
          return;
        }
        Console.clearRepeat();
        Console._log.Append(ConsoleFormatter.logWarning(Console._warning_num, pLogString));
        Console._previous_warnings.Add(pLogString, 1);
        ++Console._warning_num;
        break;
      default:
        Console.clearRepeat();
        Console._log.Append(pLogString);
        break;
    }
    Console.PrependTime(Console._log, pTime);
    Console._texts.Enqueue(Console._log.ToString().Trim('\n', ' '));
    Console._log.Clear();
    Console.truncateTexts();
  }

  private static void clearRepeat()
  {
    if (Console._error_repeated > 0)
    {
      Console._texts.Enqueue($"<color=red>( previous errors repeated {Console._error_repeated.ToString()} times )</color>");
      if (Console._error_repeated > 10)
        Console._texts.Enqueue("<color=red>YOU SHOULD RESTART THE GAME</color>");
      Console._error_repeated = 0;
    }
    if (Console._warning_repeated <= 0)
      return;
    Console._texts.Enqueue($"<color=yellow>( previous warning repeated {Console._warning_repeated.ToString()} times )</color>");
    Console._warning_repeated = 0;
  }

  private void Update()
  {
    LogItem result;
    while (Console.log_queue.TryDequeue(out result))
      Console.ProcessLog(result.log, result.stack_trace, result.type, result.time);
    if (Console._line_num == Console._texts.Count + Console._error_repeated + Console._warning_repeated)
      return;
    string str = string.Join<string>('\n', (IEnumerable<string>) Console._texts).Trim('\n', ' ');
    if (Console._error_repeated > 0)
    {
      str = $"{str}\n<color=red>( previous errors repeated {Console._error_repeated.ToString()} times )</color>";
      if (Console._error_repeated > 10)
        str += "\n<color=red>YOU SHOULD RESTART THE GAME</color>";
    }
    else if (Console._warning_repeated > 0)
      str = $"{str}\n<color=yellow>( previous warning repeated {Console._warning_repeated.ToString()} times )</color>";
    string[] sourceArray = str.Split('\n', StringSplitOptions.None);
    if (sourceArray.Length > 25000)
    {
      string[] destinationArray = new string[25000];
      Array.Copy((Array) sourceArray, sourceArray.Length - 25000, (Array) destinationArray, 0, 25000);
      sourceArray = destinationArray;
    }
    int num = -1;
    for (int index1 = 0; index1 < sourceArray.Length; ++index1)
    {
      int index2 = Mathf.CeilToInt((float) (index1 + 1) / 10f) - 1;
      for (int count = this._text_obj.Count; count < index2 + 1; ++count)
        this.addText();
      UnityEngine.UI.Text text1 = this._text_obj[index2];
      if (index2 != num)
      {
        text1.text = "";
        num = index2;
      }
      UnityEngine.UI.Text text2 = text1;
      text2.text = $"{text2.text}\n{sourceArray[index1].Trim('\n', ' ')}";
      text1.text = text1.text.Trim('\n', ' ');
    }
    Console._line_num = Console._texts.Count + Console._error_repeated + Console._warning_repeated;
  }

  public static void PrependTime(StringBuilder pStringBuilder, DateTime pDateTime)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append("[").Append("<color=white>").Append(pDateTime.Hour < 10 ? "0" : "").Append(pDateTime.Hour).Append("</color>").Append(':').Append("<color=white>").Append(pDateTime.Minute < 10 ? "0" : "").Append(pDateTime.Minute).Append("</color>").Append(':').Append("<color=white>").Append(pDateTime.Second < 10 ? "0" : "").Append(pDateTime.Second).Append("</color>").Append("] ");
      pStringBuilder.Insert(0, (object) stringBuilderPool.string_builder);
    }
  }

  public void openLogsFolder() => Application.OpenURL("file://" + Application.persistentDataPath);
}
