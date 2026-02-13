// Decompiled with JetBrains decompiler
// Type: WorldTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldTimer
{
  public bool isActive;
  private bool isStopWatch;
  private Action callback;
  private float interval;
  internal float timer;

  public WorldTimer(float pInterval, Action pCallback)
  {
    this.interval = pInterval;
    this.callback = pCallback;
    this.timer = this.interval;
  }

  public void setTime(float pNewTime) => this.timer = pNewTime;

  internal void setInterval(float pInterval) => this.interval = pInterval;

  public WorldTimer(float pInterval, bool pStopWatch)
  {
    this.isStopWatch = pStopWatch;
    this.interval = pInterval;
    this.timer = 0.0f;
    this.isActive = false;
  }

  public void startTimer(float pRate = -1f)
  {
    if ((double) pRate != -1.0)
      this.interval = pRate;
    this.timer = this.interval;
  }

  public void stop() => this.isActive = false;

  public void update(float pElapsed = -1f)
  {
    if ((double) pElapsed == -1.0)
      pElapsed = Time.deltaTime;
    if (this.isStopWatch)
    {
      if ((double) this.timer > 0.0)
      {
        this.timer -= pElapsed;
        this.isActive = true;
      }
      else
        this.isActive = false;
    }
    else if ((double) this.timer > 0.0)
    {
      this.timer -= pElapsed;
    }
    else
    {
      this.timer = this.interval;
      this.callback();
    }
  }
}
