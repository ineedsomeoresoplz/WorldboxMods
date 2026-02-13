// Decompiled with JetBrains decompiler
// Type: SignalManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SignalManager
{
  public static SignalManager instance;
  private Dictionary<SignalAsset, object> _signals = new Dictionary<SignalAsset, object>();

  public SignalManager() => SignalManager.instance = this;

  public static void add(SignalAsset pSignal, object pObject = null)
  {
    if (pSignal.isBanned())
      return;
    SignalManager.instance.plan(pSignal, pObject);
  }

  private void plan(SignalAsset pSignal, object pObject = null)
  {
    this._signals.TryAdd(pSignal, pObject);
  }

  public void update()
  {
    if (this._signals.Count == 0)
      return;
    foreach (KeyValuePair<SignalAsset, object> signal in this._signals)
    {
      SignalAsset key = signal.Key;
      object obj = signal.Value;
      if (key.has_action)
        key.action(obj);
      if (key.has_action_achievement)
      {
        int num = key.action_achievement(obj) ? 1 : 0;
      }
      if (key.has_ban_check_action && key.ban_check_action(obj))
        key.ban();
    }
    this._signals.Clear();
  }
}
