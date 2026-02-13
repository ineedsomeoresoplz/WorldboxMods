// Decompiled with JetBrains decompiler
// Type: ThreadHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ThreadHelper : MonoBehaviour
{
  public static ThreadHelper instance = (ThreadHelper) null;
  private static List<Action> threadEventsQueue = new List<Action>();
  private static volatile bool threadEventsQueueEmpty = true;
  private static int _main_thread_id = -1;
  [ThreadStatic]
  private static bool? _is_main_thread;

  public static void Initialize()
  {
    if (ThreadHelper.IsActive())
      return;
    GameObject gameObject = new GameObject("MainThreadExecuter");
    ((Object) gameObject).hideFlags = (HideFlags) 61;
    Object.DontDestroyOnLoad((Object) gameObject);
    ThreadHelper.instance = gameObject.AddComponent<ThreadHelper>();
  }

  public static bool IsActive()
  {
    return Object.op_Inequality((Object) ThreadHelper.instance, (Object) null);
  }

  public void Awake() => Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);

  public static void ExecuteInUpdate(Action action)
  {
    lock (ThreadHelper.threadEventsQueue)
    {
      ThreadHelper.threadEventsQueue.Add(action);
      ThreadHelper.threadEventsQueueEmpty = false;
    }
  }

  public static void InvokeInUpdate(UnityEvent eventParam)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() => eventParam.Invoke()));
  }

  public void Update()
  {
    if (ThreadHelper.threadEventsQueueEmpty)
      return;
    List<Action> actionList = new List<Action>();
    lock (ThreadHelper.threadEventsQueue)
    {
      actionList.AddRange((IEnumerable<Action>) ThreadHelper.threadEventsQueue);
      ThreadHelper.threadEventsQueue.Clear();
      ThreadHelper.threadEventsQueueEmpty = true;
    }
    foreach (Action action in actionList)
    {
      if (action.Target != null)
        action();
    }
  }

  public void OnDisable() => ThreadHelper.instance = (ThreadHelper) null;

  [RuntimeInitializeOnLoadMethod]
  private static void initMainThread()
  {
    ThreadHelper._main_thread_id = Thread.CurrentThread.ManagedThreadId;
  }

  public static bool isMainThread()
  {
    if (!ThreadHelper._is_main_thread.HasValue)
      ThreadHelper._is_main_thread = new bool?(Thread.CurrentThread.ManagedThreadId == ThreadHelper._main_thread_id);
    return ThreadHelper._is_main_thread.Value;
  }
}
