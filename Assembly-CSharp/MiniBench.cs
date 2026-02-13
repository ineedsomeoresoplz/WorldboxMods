// Decompiled with JetBrains decompiler
// Type: MiniBench
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public readonly struct MiniBench : IDisposable
{
  private readonly string _id;
  private readonly Stopwatch _sw;
  private readonly long _dont_show_below_ms;
  private const string _COLOR_WARN = "<color=yellow>";
  private const string _COLOR_SLOW = "<color=red>";
  private static int MAX_LOG_LENGTH = 38;

  public MiniBench(string pID)
  {
    this._id = pID;
    this._sw = new Stopwatch();
    this._sw.Start();
    this._dont_show_below_ms = 0L;
  }

  public MiniBench(string pID, long pDontShowBelowMs)
  {
    this._id = pID;
    this._sw = new Stopwatch();
    this._sw.Start();
    this._dont_show_below_ms = pDontShowBelowMs;
  }

  public void Dispose()
  {
    this._sw.Stop();
    long elapsedMilliseconds = this._sw.ElapsedMilliseconds;
    if (elapsedMilliseconds < this._dont_show_below_ms)
      return;
    string str1 = elapsedMilliseconds > 999L ? "<color=red>" : (elapsedMilliseconds > 499L ? "<color=yellow>" : "");
    string str2 = elapsedMilliseconds > 499L ? "</color>" : "";
    double totalSeconds = this._sw.Elapsed.TotalSeconds;
    if (this._id.Length + 2 > MiniBench.MAX_LOG_LENGTH)
      MiniBench.MAX_LOG_LENGTH = this._id.Length + 2;
    Debug.Log((object) $"{Toolbox.fillRight($"[{this._id}]", MiniBench.MAX_LOG_LENGTH)} = {str1}{totalSeconds.ToString("F6")}{str2}");
  }
}
