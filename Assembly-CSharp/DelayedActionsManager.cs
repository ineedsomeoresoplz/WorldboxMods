// Decompiled with JetBrains decompiler
// Type: DelayedActionsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DelayedActionsManager
{
  private List<DelayedAction> _delayed_actions = new List<DelayedAction>();
  private List<DelayedAction> _to_remove = new List<DelayedAction>();

  public void update(float pElapsed, float pDeltaTime)
  {
    for (int index = 0; index < this._delayed_actions.Count; ++index)
    {
      DelayedAction delayedAction = this._delayed_actions[index];
      if (delayedAction.update(pElapsed, pDeltaTime))
        this._to_remove.Add(delayedAction);
    }
    for (int index = 0; index < this._to_remove.Count; ++index)
      this._delayed_actions.Remove(this._to_remove[index]);
    this._to_remove.Clear();
  }

  public static void addAction(Action pAction, float pDelay, bool pGameSpeed = true)
  {
    MapBox.instance.delayed_actions_manager.addActionInstance(pAction, pDelay, pGameSpeed);
  }

  private void addActionInstance(Action pAction, float pDelay, bool pGameSpeed = true)
  {
    this._delayed_actions.Add(new DelayedAction(pAction, pDelay, pGameSpeed));
  }

  public void clear()
  {
    this._delayed_actions.Clear();
    this._to_remove.Clear();
  }
}
