// Decompiled with JetBrains decompiler
// Type: LogItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public readonly struct LogItem
{
  public readonly string log;
  public readonly string stack_trace;
  public readonly LogType type;
  public readonly DateTime time;

  public LogItem(string pLog, string pStackTrace, LogType pType)
  {
    this.log = pLog;
    this.stack_trace = pStackTrace;
    this.type = pType;
    this.time = DateTime.Now;
  }

  public LogItem(string pLog, string pStackTrace, LogType pType, DateTime pTime)
  {
    this.log = pLog;
    this.stack_trace = pStackTrace;
    this.type = pType;
    this.time = pTime;
  }
}
