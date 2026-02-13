// Decompiled with JetBrains decompiler
// Type: DelayedAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class DelayedAction
{
  public readonly Action action;
  private bool _game_speed_affected;
  private float _timer;

  public DelayedAction(Action pAction, float pDelay, bool pGameSpeedAffected)
  {
    this.action = pAction;
    this._game_speed_affected = pGameSpeedAffected;
    this._timer = pDelay;
  }

  public bool update(float pElapsed, float pDeltaTime)
  {
    if (this._game_speed_affected)
      this._timer -= pElapsed;
    else
      this._timer -= pDeltaTime;
    if ((double) this._timer > 0.0)
      return false;
    this.action();
    return true;
  }
}
